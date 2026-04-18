Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports System.Globalization

Public Class clMapsImgsToFiles
    Public MapImg As Bitmap
    Public MapImgList As New List(Of Image)
    Public MapImgIndexinUse As New List(Of Boolean)
    Private imageMutex As New Mutex()

    Public RPs As clsQRRoutePoints
    Public RPList As New List(Of clsQRRoutePoints)
    Public MapObjList As New List(Of clsMapImages)
    Public tasksCompleted As Boolean = False
    Public LastSmoothRenderElapsed As TimeSpan = TimeSpan.Zero
    Public LastSmoothZoomEncodeElapsed As TimeSpan = TimeSpan.Zero
    Public LastSmoothLegEncodeElapsed As TimeSpan = TimeSpan.Zero
    Public LastSmoothTotalElapsed As TimeSpan = TimeSpan.Zero
    Public LastSmoothFrameCount As Integer = 0
    Public LastSmoothBaseMapElapsed As TimeSpan = TimeSpan.Zero
    Public LastSmoothZoomRenderElapsed As TimeSpan = TimeSpan.Zero
    Public LastSmoothLegRenderElapsed As TimeSpan = TimeSpan.Zero
    Private legheight, legwidth, zoomwidth, zoomheight As Integer
    Private Shared ImageFileCounter As Integer = 0
    Public Sub New(inRoutePoints As clsQRRoutePoints, inMapImage As Bitmap)
        RPs = New clsQRRoutePoints(inRoutePoints)
        MapImg = New Bitmap(inMapImage)

    End Sub

    ' REPLACED WITH SMOOTH: use WriteImgsToVideoSmooth or clsMapRenderEngine for interpolated map video generation.
    Public Sub WriteImgsToVideo(outputFile As String, fps As Integer, Optional StartTime As Integer = 0, Optional Duration As Integer = -1, Optional VideoWidth As Integer = 1920)
        WriteImgsToVideoInternal(outputFile, 1.0, fps, StartTime, Duration, VideoWidth)
    End Sub

#Region "Smooth Video Rendering"
    ' Smooth rendering is driven by time spacing between frames.
    ' Example: FrameStepSeconds = 0.5 gives one rendered frame every half second and defaults to 2 fps.
    ' REPLACED WITH CLSMAPRENDERENGINE: smooth overlay video generation now runs through clsMapRenderEngine.
    Public Sub WriteImgsToVideoSmooth(outputFile As String, Optional FrameStepSeconds As Double = 0.5, Optional StartTime As Double = 0, Optional Duration As Double = -1, Optional VideoWidth As Integer = 1920, Optional OutputFps As Double = -1)
        If FrameStepSeconds <= 0 Then Throw New ArgumentOutOfRangeException(NameOf(FrameStepSeconds))
        SmoothFrameStepSeconds = FrameStepSeconds
        Dim resolvedFps As Double = ResolveSmoothOutputFps(FrameStepSeconds, OutputFps)
        WriteImgsToVideoInternal(outputFile, FrameStepSeconds, resolvedFps, StartTime, Duration, VideoWidth)
    End Sub
    Public Function GetSmoothOutputFps(Optional FrameStepSeconds As Double = -1, Optional OutputFps As Double = -1) As Double
        Dim effectiveFrameStep As Double = If(FrameStepSeconds > 0, FrameStepSeconds, SmoothFrameStepSeconds)
        If effectiveFrameStep <= 0 Then effectiveFrameStep = 0.5
        Return ResolveSmoothOutputFps(effectiveFrameStep, OutputFps)
    End Function
    Private Function ResolveSmoothOutputFps(frameStepSeconds As Double, outputFps As Double) As Double
        If outputFps > 0 Then Return outputFps
        Return 1.0 / frameStepSeconds
    End Function
    ' REPLACED WITH CLSMAPRENDERENGINE: legacy smooth video writer kept only for reference.
    Private Sub WriteImgsToVideoInternal(outputFile As String, frameStepSeconds As Double, fps As Double, Optional StartTime As Double = 0, Optional Duration As Double = -1, Optional VideoWidth As Integer = 1920)
        If frameStepSeconds <= 0 Then Throw New ArgumentOutOfRangeException(NameOf(frameStepSeconds))
        If fps <= 0 Then Throw New ArgumentOutOfRangeException(NameOf(fps))

        LastSmoothRenderElapsed = TimeSpan.Zero
        LastSmoothZoomEncodeElapsed = TimeSpan.Zero
        LastSmoothLegEncodeElapsed = TimeSpan.Zero
        LastSmoothTotalElapsed = TimeSpan.Zero
        LastSmoothFrameCount = 0
        LastSmoothBaseMapElapsed = TimeSpan.Zero
        LastSmoothZoomRenderElapsed = TimeSpan.Zero
        LastSmoothLegRenderElapsed = TimeSpan.Zero

        Dim totalStopwatch As Stopwatch = Stopwatch.StartNew()
        Dim renderStopwatch As Stopwatch = New Stopwatch()
        Dim zoomEncodeStopwatch As Stopwatch = New Stopwatch()
        Dim legEncodeStopwatch As Stopwatch = New Stopwatch()
        Dim baseMapStopwatch As Stopwatch = New Stopwatch()
        Dim zoomRenderStopwatch As Stopwatch = New Stopwatch()
        Dim legRenderStopwatch As Stopwatch = New Stopwatch()

        Dim count As Integer = RPs.RoutePoints.Count
        If count = 0 Then Return

        Dim startIdx As Double = Math.Max(0, StartTime)
        Dim maxTime As Double = count - 1
        Dim endIdx As Double = If(Duration < 0, maxTime, Math.Min(startIdx + Duration - frameStepSeconds, maxTime))
        If startIdx > endIdx Then Return

        Dim mapObj As New clsMapImages(RPs, MapImg)
        mapObj.ScalePixelSettings(VideoWidth)

        Dim sampleZoom As Bitmap = Nothing
        Dim sampleLeg As Bitmap = Nothing
        If My.Settings.cbShowRoute Then sampleZoom = mapObj.ZoomImageSmooth(startIdx)
        If My.Settings.cbShowLegMAp Then sampleLeg = mapObj.LapImageSmooth(startIdx)

        Dim zwidth As Integer = If(sampleZoom IsNot Nothing, sampleZoom.Width, 0)
        Dim zheight As Integer = If(sampleZoom IsNot Nothing, sampleZoom.Height, 0)
        Dim lwidth As Integer = If(sampleLeg IsNot Nothing, sampleLeg.Width, 0)
        Dim lheight As Integer = If(sampleLeg IsNot Nothing, sampleLeg.Height, 0)
        If lwidth + zwidth <= 0 OrElse Math.Max(lheight, zheight) <= 0 Then
            Throw New InvalidOperationException("Calculated output video dimensions are zero. Enable at least one image part.")
        End If

        Dim baseDir As String = Path.GetDirectoryName(outputFile)
        If String.IsNullOrEmpty(baseDir) Then baseDir = "."
        Dim baseName As String = Path.GetFileNameWithoutExtension(outputFile)
        Dim fpsText As String = fps.ToString("0.###", CultureInfo.InvariantCulture)

        Dim zoomProcess As Process = Nothing
        Dim legProcess As Process = Nothing
        Dim zoomStdin As Stream = Nothing
        Dim legStdin As Stream = Nothing

        Try
            If sampleZoom IsNot Nothing Then
                Dim psiZ As New ProcessStartInfo()
                psiZ.FileName = "ffmpeg"
                psiZ.Arguments = $"-y -f rawvideo -pixel_format bgr24 -video_size {zwidth}x{zheight} -framerate {fpsText} -i - -pix_fmt yuv420p -c:v libx264 -preset veryfast -crf 18 ""{Path.Combine(baseDir, baseName & "_z.mp4")}"""
                psiZ.UseShellExecute = False
                psiZ.RedirectStandardInput = True
                psiZ.RedirectStandardError = True
                psiZ.CreateNoWindow = True
                zoomProcess = Process.Start(psiZ)
                zoomEncodeStopwatch.Start()
                Dim stderrZ = zoomProcess.StandardError
                Task.Run(Sub()
                             Try
                                 stderrZ.ReadToEnd()
                             Catch
                             End Try
                         End Sub)
                zoomStdin = zoomProcess.StandardInput.BaseStream
            End If

            If sampleLeg IsNot Nothing Then
                Dim psiL As New ProcessStartInfo()
                psiL.FileName = "ffmpeg"
                psiL.Arguments = $"-y -f rawvideo -pixel_format bgr24 -video_size {lwidth}x{lheight} -framerate {fpsText} -i - -pix_fmt yuv420p -c:v libx264 -preset veryfast -crf 18 ""{Path.Combine(baseDir, baseName & "_l.mp4")}"""
                psiL.UseShellExecute = False
                psiL.RedirectStandardInput = True
                psiL.RedirectStandardError = True
                psiL.CreateNoWindow = True
                legProcess = Process.Start(psiL)
                legEncodeStopwatch.Start()
                Dim stderrL = legProcess.StandardError
                Task.Run(Sub()
                             Try
                                 stderrL.ReadToEnd()
                             Catch
                             End Try
                         End Sub)
                legStdin = legProcess.StandardInput.BaseStream
            End If

            If zoomStdin Is Nothing AndAlso legStdin Is Nothing Then Throw New InvalidOperationException("No output images enabled (zoom or leg).")

            Dim frameNo As Integer = 0
            Dim epsilon As Double = frameStepSeconds / 1000.0
            renderStopwatch.Start()
            Do
                Dim currentTime As Double = startIdx + frameNo * frameStepSeconds
                If currentTime > endIdx + epsilon Then Exit Do

                Dim imageleg As Bitmap = Nothing
                Dim imagezoom As Bitmap = Nothing
                If frameNo = 0 Then
                    imageleg = sampleLeg
                    imagezoom = sampleZoom
                Else
                    If My.Settings.cbShowLegMAp Then
                        legRenderStopwatch.Start()
                        imageleg = mapObj.LapImageSmooth(currentTime)
                        legRenderStopwatch.Stop()
                    End If
                    If My.Settings.cbShowRoute Then
                        zoomRenderStopwatch.Start()
                        imagezoom = mapObj.ZoomImageSmooth(currentTime)
                        zoomRenderStopwatch.Stop()
                    End If
                End If

                If imagezoom IsNot Nothing AndAlso zoomStdin IsNot Nothing Then WriteBitmapFrameToStream(imagezoom, zoomStdin)
                If imageleg IsNot Nothing AndAlso legStdin IsNot Nothing Then WriteBitmapFrameToStream(imageleg, legStdin)

                If frameNo > 0 Then
                    If imageleg IsNot Nothing Then imageleg.Dispose()
                    If imagezoom IsNot Nothing Then imagezoom.Dispose()
                End If

                Threading.Interlocked.Increment(ImageFileCounter)
                frameNo += 1
            Loop
            renderStopwatch.Stop()
            LastSmoothFrameCount = frameNo

            If zoomStdin IsNot Nothing Then
                zoomStdin.Close()
                zoomProcess.WaitForExit()
                zoomEncodeStopwatch.Stop()
            End If
            If legStdin IsNot Nothing Then
                legStdin.Close()
                legProcess.WaitForExit()
                legEncodeStopwatch.Stop()
            End If
        Finally
            If renderStopwatch.IsRunning Then renderStopwatch.Stop()
            If zoomEncodeStopwatch.IsRunning Then zoomEncodeStopwatch.Stop()
            If legEncodeStopwatch.IsRunning Then legEncodeStopwatch.Stop()
            totalStopwatch.Stop()
            LastSmoothRenderElapsed = renderStopwatch.Elapsed
            LastSmoothZoomEncodeElapsed = zoomEncodeStopwatch.Elapsed
            LastSmoothLegEncodeElapsed = legEncodeStopwatch.Elapsed
            LastSmoothTotalElapsed = totalStopwatch.Elapsed
            LastSmoothBaseMapElapsed = baseMapStopwatch.Elapsed
            LastSmoothZoomRenderElapsed = zoomRenderStopwatch.Elapsed
            LastSmoothLegRenderElapsed = legRenderStopwatch.Elapsed
            Try
                If zoomProcess IsNot Nothing AndAlso Not zoomProcess.HasExited Then zoomProcess.Kill()
            Catch
            End Try
            Try
                If legProcess IsNot Nothing AndAlso Not legProcess.HasExited Then legProcess.Kill()
            Catch
            End Try
        End Try
    End Sub
    Private Sub WriteBitmapFrameToStream(sourceImage As Bitmap, targetStream As Stream)
        Using tmpBmp As New Bitmap(sourceImage.Width, sourceImage.Height, PixelFormat.Format24bppRgb)
            Using g As Graphics = Graphics.FromImage(tmpBmp)
                g.Clear(Color.Magenta)
                g.DrawImage(sourceImage, 0, 0)
            End Using

            Dim rect As New Rectangle(0, 0, tmpBmp.Width, tmpBmp.Height)
            Dim bitmapData As BitmapData = tmpBmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb)
            Dim stride As Integer = Math.Abs(bitmapData.Stride)
            Dim rowBytes As Integer = tmpBmp.Width * 3
            Dim buffer(rowBytes * tmpBmp.Height - 1) As Byte

            For row As Integer = 0 To tmpBmp.Height - 1
                Dim srcPtr As IntPtr = New IntPtr(bitmapData.Scan0.ToInt64() + row * stride)
                Marshal.Copy(srcPtr, buffer, row * rowBytes, rowBytes)
            Next

            tmpBmp.UnlockBits(bitmapData)
            targetStream.Write(buffer, 0, buffer.Length)
            targetStream.Flush()
        End Using
    End Sub
#End Region
    Public Function GetTotalFiles(Optional StartTime As Integer = 0, Optional Duration As Integer = -1) As Integer
        Dim count As Integer = RPs.RoutePoints.Count
        Dim startIdx As Integer = Math.Max(0, StartTime) ' Ensure start index is within the range '
        Dim endIdx As Integer = If(Duration < 0, count - 1, Math.Min(startIdx + Duration - 1, count - 1)) ' Calculate end index based on start index and duration '


        ' Divide the images into equal-sized chunks for each thread '
        Return endIdx - startIdx + 1
    End Function
    ' REPLACED WITH SMOOTH: legacy image-sequence export kept for reference.
    Public Sub WriteImgsToFiles(SetnumThreads As Integer, Optional StartTime As Integer = 0, Optional Duration As Integer = -1, Optional VideoWidth As Integer = 1920)
        Dim imagePaths As New List(Of String)
        Dim multithread As Boolean = True

        ImageFileCounter = 0
        ' Code to generate the images and add their file paths to the imagePaths list '
        If My.Settings.cbShowLegMAp Or My.Settings.cbShowRoute Then
            Dim numThreads As Integer
            If SetnumThreads = -1 Then numThreads = Environment.ProcessorCount ' Use the number of CPU cores available '
            If SetnumThreads = 1 Then multithread = False
            If SetnumThreads > 1 Then numThreads = SetnumThreads

            Dim count As Integer = RPs.RoutePoints.Count
            Dim startIdx As Integer = Math.Max(0, StartTime) ' Ensure start index is within the range '
            Dim endIdx As Integer = If(Duration < 0, count - 1, Math.Min(startIdx + Duration - 1, count - 1)) ' Calculate end index based on start index and duration '


            ' Divide the images into equal-sized chunks for each thread '
            Dim chunkSize As Integer = (endIdx - startIdx + 1) / numThreads

            Dim threads(numThreads - 1) As Thread

            For i = 0 To numThreads - 1
                MapImgList.Add(New Bitmap(MapImg))
                MapImgIndexinUse.Add(False)
                RPList.Add(New clsQRRoutePoints(RPs))
                Dim mapObj As New clsMapImages(RPList(i), MapImgList(i))
                mapObj.ScalePixelSettings(VideoWidth)
                MapObjList.Add(mapObj)
                'MapObjList.Add(New clsMapImages(RPList(i), MapImgList(i)))
            Next

            ' Set the file image dimensions and positions '
            If My.Settings.cbShowLegMAp Then
                My.Settings.MIFLegPos = New Point(0, 0)
                My.Settings.MIFLegDim = New Point(MapObjList(0).LegWidth, MapObjList(0).LegHeight)
            End If
            If My.Settings.cbShowRoute Then

                If My.Settings.cbShowLegMAp Then
                    My.Settings.MIFZoomPos = New Point(MapObjList(0).LegWidth, 0) ' ZoomImage to the right of legimage
                Else
                    My.Settings.MIFZoomPos = New Point(0, 0)
                End If
                My.Settings.MIFZoomDim = New Point(MapObjList(0).ZoomWidth, MapObjList(0).ZoomHeight)
            End If
            My.Settings.Save()

            If Not multithread Then
                For i = startIdx To endIdx
                    SaveImage(i, 0) ' Serial execution
                Next
            Else
                'For i As Integer = 0 To numThreads - 1
                '    Dim threadStart As New ParameterizedThreadStart(
                '    Sub(obj As Object)
                '        Dim startIndex As Integer = DirectCast(obj, Integer)
                '        For j As Integer = startIndex To startIndex + chunkSize - 1
                '            If j <= endIdx Then ' Check if index is within the range '
                '                SaveImage(j) ' Save the image with its corresponding number '
                '            End If
                '        Next
                '    End Sub
                ')

                '    Dim threadStartIndex As Integer = startIdx + i * chunkSize
                '    threads(i) = New Thread(threadStart)
                '    threads(i).Start(threadStartIndex)
                'Next

                '' Wait for all threads to complete '
                'For i As Integer = 0 To numThreads - 1
                '    threads(i).Join()
                'Next
                Dim tasks(numThreads - 1) As Task
                tasksCompleted = False
                For i As Integer = 0 To numThreads - 1
                    'Dim threadStart As New ParameterizedThreadStart(
                    Dim threadStartIndex As Integer = startIdx + i * chunkSize
                    Dim threadNumber As Integer = i
                    tasks(i) = Task.Factory.StartNew(
                    Sub() 'obj As Object)
                        'Dim startIndex As Integer = DirectCast(obj, Integer)
                        For j As Integer = threadStartIndex To threadStartIndex + chunkSize - 1
                            If j <= endIdx And j < count - 1 Then ' Check if index is within the range '
                                SaveImage(j, threadNumber) ' Save the image with its corresponding number '
                            End If
                        Next
                    End Sub
                )

                    'Dim threadStartIndex As Integer = startIdx + i * chunkSize
                    'threads(i) = New Thread(threadStart)
                    'threads(i).Start(threadStartIndex)
                Next
                Task.Factory.StartNew(
            Sub()
                Task.WaitAll(tasks) ' Wait for all tasks to complete
                TaskCompleted()
            End Sub
        )
                ' Wait for all threads to complete '
                'For i As Integer = 0 To numThreads - 1
                '    threads(i).Join()
                'Next
            End If
        End If
    End Sub

    Private Sub TaskCompleted()
        ' This method is called when all tasks have completed
        ' Update UI or perform any other post-processing

        ' Set the flag or raise an event to indicate completion
        ' For example, you can set a boolean flag:
        tasksCompleted = True
    End Sub

    'Public Sub WriteImgsToFiles_old(SetnumThreads As Integer, Optional StartTime As Integer = 0, Optional Duration As Integer = -1)
    '    Dim imagePaths As New List(Of String)
    '    Dim multithread As Boolean = True
    '    ' Code to generate the images and add their file paths to the imagePaths list '

    '    Dim numThreads As Integer
    '    If SetnumThreads = -1 Then numThreads = Environment.ProcessorCount ' Use the number of CPU cores available '
    '    If SetnumThreads = 1 Then multithread = False
    '    If SetnumThreads > 1 Then numThreads = SetnumThreads
    '    'numThreads = 2
    '    Dim threads(numThreads - 1) As Thread

    '    Dim count As Integer = RPs.RoutePoints.Count
    '    ' Divide the images into equal-sized chunks for each thread '
    '    Dim chunkSize As Integer = count / numThreads


    '    For i = 0 To numThreads - 1
    '        MapImgList.Add(New Bitmap(MapImg))
    '        MapImgIndexinUse.Add(False)



    '        RPList.Add(New clsQRRoutePoints(RPs))
    '        MapObjList.Add(New clsMapImages(RPList(i), MapImgList(i)))
    '    Next
    '    'Set the file image dimensions and positions
    '    My.Settings.MIFLegPos = New Point(0, 0)
    '    My.Settings.MIFLegDim = New Point(MapObjList(0).LegWidth, MapObjList(0).LegHeight)
    '    My.Settings.MIFZoomPos = New Point(MapObjList(0).LegWidth, 0) ' ZoomImage to the right of legimage
    '    My.Settings.MIFZoomDim = New Point(MapObjList(0).ZoomWidth, MapObjList(0).ZoomHeight)


    '    If Not multithread Then
    '        For i = 0 To count - 1
    '            SaveImage(i) ' seriel kørsel
    '        Next
    '    Else

    '        For i As Integer = 0 To numThreads - 1
    '            Dim start As Integer = i * chunkSize
    '            Dim [end] As Integer = If(i = numThreads - 1, count - 1, (i + 1) * chunkSize - 1)
    '            Dim i2 As Integer = i
    '            Dim threadStart As New ParameterizedThreadStart(Sub(obj As Object)
    '                                                                Dim startIndex As Integer = DirectCast(obj, Integer)
    '                                                                Dim x As Integer = i2
    '                                                                For j As Integer = startIndex To [end]
    '                                                                    SaveImage(j) ' Save the image with its corresponding number '
    '                                                                Next
    '                                                            End Sub)
    '            threads(i) = New Thread(threadStart)
    '            threads(i).Start(start)
    '        Next

    '        ' Wait for all threads to complete '
    '        For i As Integer = 0 To numThreads - 1
    '            threads(i).Join()
    '        Next
    '    End If


    'End Sub
    Private Sub SaveImage(ByVal imageNumber As Integer, ByVal TNo As Integer)
        ' Code to load and process the image '
        Dim index As Integer
        Dim imgmap, imageleg, imagezoom As Image


        For index = 0 To MapImgIndexinUse.Count
            If MapImgIndexinUse(TNo) = False Then
                MapImgIndexinUse(TNo) = True
                imageMutex.WaitOne()
                'newImage = New Bitmap(MapImgList(index))

                imageMutex.ReleaseMutex()
                Exit For
            End If
        Next

        Dim zheight, zwidth, lwidth, lheight As Integer
        imgmap = MapObjList(TNo).DrawPositionOnBackgroundImage(imageNumber)
        If My.Settings.cbShowRoute Then
            imagezoom = MapObjList(TNo).ZoomImage(imageNumber)
            zheight = imagezoom.Height
            zwidth = imagezoom.Width
        End If
        If My.Settings.cbShowLegMAp Then
            imageleg = MapObjList(TNo).LapImage(imageNumber)
            lheight = imageleg.Height
            lwidth = imageleg.Width
        End If

        Dim combinedWidth As Integer = lwidth + zwidth
        Dim combinedHeight As Integer = Math.Max(lheight, zheight)



        Dim imageFileName As String = "temp3\" + imageNumber.ToString("D8") + ".png" 'png
        'imgmap.Save(imageFileName, ImageFormat.Png)



        ' Create a new bitmap with the combined size
        Dim combinedImage As New Bitmap(combinedWidth, combinedHeight) 'PixelFormat.Format24bppRgb

        ' Create a graphics object to draw onto the new bitmap
        Using g As Graphics = Graphics.FromImage(combinedImage)
            ' Draw the first image onto the left side of the combined image

            If My.Settings.cbShowLegMAp Then g.DrawImage(imageleg, 0, 0)

            ' Draw the second image onto the right side of the combined image
            If My.Settings.cbShowRoute Then g.DrawImage(imagezoom, lwidth, 0)
        End Using

        ' Save the combined image to a file

        combinedImage.Save(imageFileName, ImageFormat.Png)  'png

        ' Dispose of the images
        Threading.Interlocked.Increment(ImageFileCounter)
        combinedImage.Dispose()
        'imageleg.Save(limageFileName, ImageFormat.Png)
        'imagezoom.Save(zimageFileName, ImageFormat.Png)
        MapImgIndexinUse(TNo) = False
        If Not IsNothing(imageleg) Then imageleg.Dispose()
        If Not IsNothing(imagezoom) Then imagezoom.Dispose()
    End Sub

    Public Function get_ImageFileCounter() As Integer
        Dim currentCount As Integer = Threading.Interlocked.CompareExchange(ImageFileCounter, 0, 0)
        Return currentCount
    End Function
End Class
Public Class clsMapImages

    'Public RouteImgPoints As List(Of PointF)
    Public LastTimeCode, counttime As Integer
    Public avgAngle, prevangle, prevRVal, prevfiltered As Double
    Public labelx As Label
    Public MainMapImage, TmpMainMap, ZoomMapImage, LegMapImage, tmpZLImg As Image
    Private SmoothOverlayMapImage As Bitmap
    Private LastSmoothOverlayTime As Double = Double.NaN
    Private SmoothLegBackgroundImage As Bitmap
    Private SmoothLegBackgroundCacheKey As String = ""
    Public FrameWidth As Integer = 4
    Public FrameColor As Color = Color.IndianRed
    Public QRRoutePoints As clsQRRoutePoints
    Public RouteImgPoints As List(Of Point)
    Public tailLineDurationSeconds As Integer
    Public tailLineColor, dotColor As Color
    Public dotSize As Integer
    Public dotTailRatio, ZoomZoom, ArrowBarb, ArrowWidth, VideoScale As Double
    Public _DotType As String
    Public SqFramecolor As Color = Color.White
    Public sqFrameSize As Integer = 2
    Public ZoomWidth, ZoomHeight, LegWidth, LegHeight, ZoomRad, LegRad, LegMargin As Integer
    Public alphaMaskZoom, alphaMaskLeg As Bitmap
    Public FrameFeather As Boolean = False

#Region "Smooth Rendering Configuration"
    Private SmoothFrameStepSeconds As Double = 0.5
    Private SmoothHeadingWindowSeconds As Double = 2.0
    Private SmoothHeadingMaxWindowSeconds As Double = 8.0
    Private SmoothPositionWindowSeconds As Double = 0.75
    Private SmoothTailSampleStepSeconds As Double = 0.25
    Private SmoothTailMinimumPointDistance As Double = 1.0
    Private SmoothHeadingMinimumDistance As Double = 6.0
#End Region

    Public Sub ConfigureSmoothPreview(Optional tailSampleStepSeconds As Double = -1,
                                      Optional tailMinimumPointDistance As Double = -1,
                                      Optional positionWindowSeconds As Double = -1,
                                      Optional headingWindowSeconds As Double = -1,
                                      Optional headingMinimumDistance As Double = -1)
        If tailSampleStepSeconds > 0 Then SmoothTailSampleStepSeconds = tailSampleStepSeconds
        If tailMinimumPointDistance > 0 Then SmoothTailMinimumPointDistance = tailMinimumPointDistance
        If positionWindowSeconds > 0 Then SmoothPositionWindowSeconds = positionWindowSeconds
        If headingWindowSeconds > 0 Then SmoothHeadingWindowSeconds = headingWindowSeconds
        If headingMinimumDistance > 0 Then SmoothHeadingMinimumDistance = headingMinimumDistance
        InvalidateSmoothCaches()
    End Sub

    Public Sub ResetSmoothPreviewDefaults()
        SmoothHeadingWindowSeconds = 2.0
        SmoothHeadingMaxWindowSeconds = 8.0
        SmoothPositionWindowSeconds = 0.75
        SmoothTailSampleStepSeconds = 0.25
        SmoothTailMinimumPointDistance = 1.0
        SmoothHeadingMinimumDistance = 6.0
        InvalidateSmoothCaches()
    End Sub

    Public Sub New(inQRRouteP As clsQRRoutePoints, InMainMapImage As Bitmap, Optional bLoadSettings As Boolean = True)
        LogFejl("Program started", False)
        prevangle = 0
        QRRoutePoints = inQRRouteP
        MainMapImage = New Bitmap(InMainMapImage)
        RouteImgPoints = QRRoutePoints.ImgRoutePoints
        If bLoadSettings Then
            LoadSettings()
        Else
            tailLineDurationSeconds = 60
            tailLineColor = Color.Blue
            dotColor = Color.Red
            dotSize = 8
            dotTailRatio = 0.2
            LegHeight = 600
            LegWidth = 350
            ZoomWidth = 350
            ZoomHeight = 350
            ZoomRad = 80
            LegRad = 40
            LegMargin = 50
            ZoomZoom = 1
            ArrowBarb = 0.3
            ArrowWidth = 0.7
            _DotType = "Dot"
            FrameFeather = False
        End If
        ResetAlphaMasks()
    End Sub
    Public Sub SaveSettings()
        My.Settings.MIDotSize = dotSize
        My.Settings.MIDotColor = dotColor
        My.Settings.MIFrameColor = FrameColor
        My.Settings.MIFrameWidth = FrameWidth
        My.Settings.MILegHeight = LegHeight
        My.Settings.MILegMargin = LegMargin
        My.Settings.MILegRad = LegRad
        My.Settings.MILegWidth = LegWidth
        My.Settings.MITailColor = tailLineColor
        My.Settings.MITailDuration = tailLineDurationSeconds
        My.Settings.MITailRatio = dotTailRatio
        My.Settings.MIZoomHeight = ZoomHeight
        My.Settings.MIZoomRad = ZoomRad
        My.Settings.MIZoomWidth = ZoomWidth
        My.Settings.MIZoomZoom = ZoomZoom
        My.Settings.MIArrowBarb = ArrowBarb
        My.Settings.MIArrowWidth = ArrowWidth
        My.Settings.MIDotType = _DotType
        My.Settings.MIFrameFeather = FrameFeather

    End Sub
    Public Sub LoadSettings()
        dotSize = My.Settings.MIDotSize
        dotColor = My.Settings.MIDotColor
        FrameColor = My.Settings.MIFrameColor
        FrameWidth = My.Settings.MIFrameWidth
        LegHeight = My.Settings.MILegHeight
        LegMargin = My.Settings.MILegMargin
        LegRad = My.Settings.MILegRad
        LegWidth = My.Settings.MILegWidth
        tailLineColor = My.Settings.MITailColor
        tailLineDurationSeconds = My.Settings.MITailDuration
        dotTailRatio = My.Settings.MITailRatio
        ZoomHeight = My.Settings.MIZoomHeight
        ZoomRad = My.Settings.MIZoomRad
        ZoomWidth = My.Settings.MIZoomWidth
        ZoomZoom = My.Settings.MIZoomZoom
        ArrowWidth = My.Settings.MIArrowWidth
        ArrowBarb = My.Settings.MIArrowBarb
        _DotType = My.Settings.MIDotType
        FrameFeather = My.Settings.MIFrameFeather
    End Sub
    Public Sub ResetAlphaMasks()
        If alphaMaskZoom IsNot Nothing Then
            alphaMaskZoom.Dispose()
            alphaMaskZoom = Nothing
        End If

        If alphaMaskLeg IsNot Nothing Then
            alphaMaskLeg.Dispose()
            alphaMaskLeg = Nothing
        End If

        ' Opret nye bitmaps i korrekt størrelse og format
        alphaMaskZoom = New Bitmap(ZoomWidth, ZoomHeight, PixelFormat.Format32bppArgb)
        alphaMaskLeg = New Bitmap(LegWidth, LegHeight, PixelFormat.Format32bppArgb)
    End Sub
    Public Sub InvalidateSmoothCaches()
        If SmoothOverlayMapImage IsNot Nothing Then
            SmoothOverlayMapImage.Dispose()
            SmoothOverlayMapImage = Nothing
        End If
        LastSmoothOverlayTime = Double.NaN
    End Sub
    Public Function ScalePixelSettings(OutVideoWidth As Integer, Optional InVideoWidth As Integer = 1920) As Double
        'all my.settings are done with 1920 reference - +++
        Return 1 ' scale2
        If Not OutVideoWidth = InVideoWidth Then
            VideoScale = OutVideoWidth / InVideoWidth
            dotSize = ScaleVInt(My.Settings.MIDotSize)
            FrameWidth = ScaleVInt(My.Settings.MIFrameWidth)
            LegHeight = ScaleVInt(My.Settings.MILegHeight)
            LegMargin = ScaleVInt(My.Settings.MILegMargin)
            LegRad = ScaleVInt(My.Settings.MILegRad)
            LegWidth = ScaleVInt(My.Settings.MILegWidth)
            'dotTailRatio = My.Settings.MITailRatio
            ZoomHeight = ScaleVInt(My.Settings.MIZoomHeight)
            ZoomRad = ScaleVInt(My.Settings.MIZoomRad)
            ZoomWidth = ScaleVInt(My.Settings.MIZoomWidth)
            ZoomZoom = ScaleVInt(My.Settings.MIZoomZoom)
            ArrowWidth = ScaleVInt(My.Settings.MIArrowWidth)
            'ArrowBarb = ScaleVInt(My.Settings.MIArrowBarb)
            Return VideoScale
        Else
            Return 1
        End If
    End Function
    Public Function ScaleVInt(InInteger As Integer) As Integer
        Return CInt(Math.Round(InInteger * VideoScale))
    End Function
    Function OffsetPointByLength(originalPoint As PointF, vectorStart As PointF, vectorEnd As PointF) As PointF
        ' Beregn vektorens retning
        Dim dx As Single = vectorEnd.X - vectorStart.X
        Dim dy As Single = vectorEnd.Y - vectorStart.Y

        ' Find vektorens længde
        Dim vectorLength As Single = Math.Sqrt(dx * dx + dy * dy)
        Dim length As Single = -vectorLength / 2

        ' Normaliser vektoren (gør den til en enhedsvektor)
        Dim unitVectorX As Single = dx / vectorLength
        Dim unitVectorY As Single = dy / vectorLength

        ' Multiplicer enhedsvektoren med den ønskede længde af forskydningen
        Dim offsetX As Single = unitVectorX * length
        Dim offsetY As Single = unitVectorY * length

        ' Tilføj den skalar-multiplikerede vektor til det oprindelige punkt
        Return New PointF(originalPoint.X + offsetX, originalPoint.Y + offsetY)
    End Function
    Public Sub DrawArrowWithBarbs(g As Graphics, currentPos As PointF, idirection As Single, dotSize As Integer, dotColor As Color)
        Dim direction As Single
        direction = idirection - 90
        ' Basisstørrelsen på pilehovedet
        Dim arrowLength As Integer = dotSize * 2  ' Længden på pilehovedet
        Dim RarrowWidth As Integer = dotSize * ArrowWidth ' Bredden på pilehovedets base
        Dim barbOffset As Integer = dotSize * ArrowBarb ' Forskydningen opad for modhagepunktet

        ' Opret en børste til at tegne pilehovedet
        Dim arrowBrush As New SolidBrush(dotColor)

        ' Beregn de fire punkter for pilehovedet
        Dim points(3) As PointF
        Dim offpoints(3) As PointF

        ' Spidsen af pilehovedet
        points(0) = New PointF(currentPos.X + Math.Cos(direction * Math.PI / 180) * arrowLength, currentPos.Y + Math.Sin(direction * Math.PI / 180) * arrowLength)

        ' De to nederste punkter
        points(1) = New PointF(currentPos.X + Math.Cos((direction + 90) * Math.PI / 180) * RarrowWidth, currentPos.Y + Math.Sin((direction + 90) * Math.PI / 180) * RarrowWidth)
        points(3) = New PointF(currentPos.X + Math.Cos((direction - 90) * Math.PI / 180) * RarrowWidth, currentPos.Y + Math.Sin((direction - 90) * Math.PI / 180) * RarrowWidth)

        ' Modhagepunktet, beregnet som midtpunktet mellem de to nederste punkter, forskudt opad
        Dim midBaseX As Single = (points(1).X + points(3).X) / 2
        Dim midBaseY As Single = (points(1).Y + points(3).Y) / 2
        points(2) = New PointF(midBaseX + Math.Cos(direction * Math.PI / 180) * barbOffset, midBaseY + Math.Sin(direction * Math.PI / 180) * barbOffset)
        For i = 0 To 3
            offpoints(i) = OffsetPointByLength(points(i), currentPos, points(0)) ' forskyder punkter til midt på pilen i pilens retning
        Next
        ' Tegn pilehovedet
        g.FillPolygon(arrowBrush, offpoints)

        ' Oprydning
        arrowBrush.Dispose()
    End Sub
#Region "Smooth Rendering Helpers"
    Private Function ClampTimeCode(timeCode As Double) As Double
        If QRRoutePoints Is Nothing OrElse QRRoutePoints.RoutePoints.Count = 0 Then Return 0
        If timeCode < 0 Then Return 0
        Dim maxTimeCode As Double = QRRoutePoints.RoutePoints.Count - 1
        If timeCode > maxTimeCode Then Return maxTimeCode
        Return timeCode
    End Function
    Private Sub ResolveTimeSegment(timeCode As Double, ByRef lowerIdx As Integer, ByRef upperIdx As Integer, ByRef blend As Double)
        Dim clampedTime As Double = ClampTimeCode(timeCode)
        lowerIdx = CInt(Math.Floor(clampedTime))
        upperIdx = CInt(Math.Ceiling(clampedTime))
        If lowerIdx < 0 Then lowerIdx = 0
        If upperIdx >= QRRoutePoints.RoutePoints.Count Then upperIdx = QRRoutePoints.RoutePoints.Count - 1
        If upperIdx < lowerIdx Then upperIdx = lowerIdx
        blend = clampedTime - lowerIdx
        If upperIdx = lowerIdx Then blend = 0
    End Sub
    Private Function InterpolateValue(startValue As Double, endValue As Double, blend As Double) As Double
        Return startValue + (endValue - startValue) * blend
    End Function
    Private Function InterpolatePoint(startPoint As PointF, endPoint As PointF, blend As Double) As PointF
        Return New PointF(
            CSng(InterpolateValue(startPoint.X, endPoint.X, blend)),
            CSng(InterpolateValue(startPoint.Y, endPoint.Y, blend))
        )
    End Function
    Private Function InterpolateAngle(startAngle As Double, endAngle As Double, blend As Double) As Double
        Dim delta As Double = ((endAngle - startAngle + 540) Mod 360) - 180
        Return (startAngle + delta * blend + 360) Mod 360
    End Function
    Private Function GetInterpolatedRoutePointAtTime(timeCode As Double) As PointF
        Dim lowerIdx, upperIdx As Integer
        Dim blend As Double
        ResolveTimeSegment(timeCode, lowerIdx, upperIdx, blend)

        Dim startPoint As clsQRRoutePoint = QRRoutePoints.RoutePoints(lowerIdx)
        Dim endPoint As clsQRRoutePoint = QRRoutePoints.RoutePoints(upperIdx)
        Dim yOffset As Double = QRRoutePoints.QRLogoYOffset
        Dim p1 As New PointF(CSng(startPoint.ImageX), CSng(startPoint.ImageY + yOffset))
        Dim p2 As New PointF(CSng(endPoint.ImageX), CSng(endPoint.ImageY + yOffset))
        Return InterpolatePoint(p1, p2, blend)
    End Function
    Private Function GetDistanceBetweenPoints(startPoint As PointF, endPoint As PointF) As Double
        Dim dx As Double = endPoint.X - startPoint.X
        Dim dy As Double = endPoint.Y - startPoint.Y
        Return Math.Sqrt(dx * dx + dy * dy)
    End Function
    Private Function GetSmoothedRoutePointAtTime(timeCode As Double) As PointF
        Dim clampedTime As Double = ClampTimeCode(timeCode)
        If SmoothPositionWindowSeconds <= 0 Then Return GetInterpolatedRoutePointAtTime(clampedTime)

        Dim sampleStep As Double = Math.Max(0.05, SmoothTailSampleStepSeconds)
        Dim startTime As Double = Math.Max(0, clampedTime - SmoothPositionWindowSeconds)
        Dim endTime As Double = Math.Min(ClampTimeCode(Double.MaxValue), clampedTime + SmoothPositionWindowSeconds)
        Dim weightedX As Double = 0
        Dim weightedY As Double = 0
        Dim totalWeight As Double = 0
        Dim sampleTime As Double = startTime

        Do While sampleTime <= endTime + sampleStep / 2
            Dim currentSampleTime As Double = Math.Min(sampleTime, endTime)
            Dim samplePoint As PointF = GetInterpolatedRoutePointAtTime(currentSampleTime)
            Dim distanceToCenter As Double = Math.Abs(currentSampleTime - clampedTime)
            Dim weight As Double = (SmoothPositionWindowSeconds + sampleStep) - distanceToCenter
            If weight > 0 Then
                weightedX += samplePoint.X * weight
                weightedY += samplePoint.Y * weight
                totalWeight += weight
            End If
            sampleTime += sampleStep
        Loop

        If totalWeight <= 0 Then Return GetInterpolatedRoutePointAtTime(clampedTime)
        Return New PointF(CSng(weightedX / totalWeight), CSng(weightedY / totalWeight))
    End Function
    Private Function GetRoutePointAtTime(timeCode As Double) As PointF
        Return GetSmoothedRoutePointAtTime(timeCode)
    End Function
    Private Function GetHeadingAngleAtTime(timeCode As Double) As Double
        Dim clampedTime As Double = ClampTimeCode(timeCode)
        Dim currentWindow As Double = Math.Max(0.25, SmoothHeadingWindowSeconds)
        Dim maxWindow As Double = Math.Max(currentWindow, SmoothHeadingMaxWindowSeconds)
        Dim startPoint As PointF = GetRoutePointAtTime(clampedTime)
        Dim endPoint As PointF = startPoint

        Do
            startPoint = GetRoutePointAtTime(Math.Max(0, clampedTime - currentWindow))
            endPoint = GetRoutePointAtTime(Math.Min(ClampTimeCode(Double.MaxValue), clampedTime + currentWindow))

            If GetDistanceBetweenPoints(startPoint, endPoint) >= SmoothHeadingMinimumDistance Then Exit Do
            If currentWindow >= maxWindow Then Exit Do
            currentWindow = Math.Min(maxWindow, currentWindow * 1.5)
        Loop

        If GetDistanceBetweenPoints(startPoint, endPoint) < 0.001 Then
            Return GetDirectionAtTime(timeCode)
        End If

        Dim angle As Double = Math.Atan2(endPoint.Y - startPoint.Y, endPoint.X - startPoint.X)
        Return (90 - angle * 180 / Math.PI) Mod 360
    End Function
    Private Function GetArrowDirectionAtTime(timeCode As Double) As Double
        Dim cameraAngle As Double = GetHeadingAngleAtTime(timeCode)
        Return (180 - cameraAngle + 360) Mod 360
    End Function
    Private Function GetDirectionAtTime(timeCode As Double) As Double
        Dim lowerIdx, upperIdx As Integer
        Dim blend As Double
        ResolveTimeSegment(timeCode, lowerIdx, upperIdx, blend)
        Return InterpolateAngle(QRRoutePoints.RoutePoints(lowerIdx).Direction, QRRoutePoints.RoutePoints(upperIdx).Direction, blend)
    End Function
    Private Function GetLapNumberAtTime(timeCode As Double) As Integer
        Dim idx As Integer = CInt(Math.Floor(ClampTimeCode(timeCode)))
        Return QRRoutePoints.RoutePoints(idx).LapNumber
    End Function
    Private Function GetTailLinePointsAtTime(timeCode As Double) As List(Of PointF)
        Dim tailPoints As New List(Of PointF)
        Dim clampedTime As Double = ClampTimeCode(timeCode)
        Dim startTime As Double = Math.Max(0, clampedTime - tailLineDurationSeconds)
        Dim sampleStep As Double = Math.Max(0.05, SmoothTailSampleStepSeconds)
        Dim sampleTime As Double = startTime

        Do While sampleTime <= clampedTime + sampleStep / 2
            Dim currentSampleTime As Double = Math.Min(sampleTime, clampedTime)
            Dim currentPoint As PointF = GetRoutePointAtTime(currentSampleTime)
            If tailPoints.Count = 0 OrElse GetDistanceBetweenPoints(tailPoints(tailPoints.Count - 1), currentPoint) >= SmoothTailMinimumPointDistance Then
                tailPoints.Add(currentPoint)
            End If
            sampleTime += sampleStep
        Loop

        Dim endPoint As PointF = GetRoutePointAtTime(clampedTime)
        If tailPoints.Count = 0 OrElse GetDistanceBetweenPoints(tailPoints(tailPoints.Count - 1), endPoint) > 0.01 Then
            tailPoints.Add(endPoint)
        Else
            tailPoints(tailPoints.Count - 1) = endPoint
        End If

        Return tailPoints
    End Function
    Private Function GetSmoothedAngleAtTime(timeCode As Double, windowSize As Integer) As Double
        Return GetHeadingAngleAtTime(timeCode)
    End Function
    Private Function GetSmoothOverlayMap(currentTime As Double) As Bitmap
        Dim safeTime As Double = ClampTimeCode(currentTime)
        If SmoothOverlayMapImage Is Nothing OrElse SmoothOverlayMapImage.Width <> MainMapImage.Width OrElse SmoothOverlayMapImage.Height <> MainMapImage.Height OrElse Double.IsNaN(LastSmoothOverlayTime) OrElse Math.Abs(LastSmoothOverlayTime - safeTime) > 0.0001 Then
            If SmoothOverlayMapImage IsNot Nothing Then SmoothOverlayMapImage.Dispose()
            SmoothOverlayMapImage = New Bitmap(MainMapImage.Width, MainMapImage.Height, PixelFormat.Format32bppArgb)

            Dim currentPos As PointF = GetRoutePointAtTime(safeTime)
            Dim tailLinePoints As List(Of PointF) = GetTailLinePointsAtTime(safeTime)
            Using g As Graphics = Graphics.FromImage(SmoothOverlayMapImage)
                g.Clear(Color.Transparent)
                If tailLinePoints.Count > 1 Then
                    Using tailLinePen As New Pen(tailLineColor, dotSize * dotTailRatio)
                        g.DrawLines(tailLinePen, tailLinePoints.ToArray())
                    End Using
                End If

                Dim dotSizeWithTail As Integer = dotSize
                Dim direction As Double = GetArrowDirectionAtTime(safeTime)
                If _DotType = "Arrow" Then
                    DrawArrowWithBarbs(g, currentPos, CSng(direction), dotSizeWithTail, dotColor)
                Else
                    Using dotBrush As New SolidBrush(dotColor)
                        g.FillEllipse(dotBrush, CSng(currentPos.X - dotSizeWithTail / 2), CSng(currentPos.Y - dotSizeWithTail / 2), CSng(dotSizeWithTail), CSng(dotSizeWithTail))
                    End Using
                End If
            End Using

            LastSmoothOverlayTime = safeTime
        End If

        Return SmoothOverlayMapImage
    End Function
    Private Function GetSmoothLegBackground(cacheKey As String, angleDegrees As Double, srcRect As RectangleF,
                                            srcSize As Integer, clipWidth As Single, clipLength As Single,
                                            outwidth As Integer, outheight As Integer) As Bitmap
        If SmoothLegBackgroundImage Is Nothing OrElse SmoothLegBackgroundCacheKey <> cacheKey Then
            If SmoothLegBackgroundImage IsNot Nothing Then SmoothLegBackgroundImage.Dispose()

            Using tmpLegBackground As New Bitmap(srcSize, srcSize)
                Using g As Graphics = Graphics.FromImage(tmpLegBackground)
                    g.TranslateTransform(tmpLegBackground.Width / 2.0F, tmpLegBackground.Height / 2.0F)
                    If IsNumeric(angleDegrees) Then g.RotateTransform(CSng(angleDegrees))
                    g.TranslateTransform(-tmpLegBackground.Width / 2.0F, -tmpLegBackground.Height / 2.0F)
                    g.DrawImage(MainMapImage, New RectangleF(0, 0, tmpLegBackground.Width, tmpLegBackground.Height), srcRect, GraphicsUnit.Pixel)
                End Using

                SmoothLegBackgroundImage = New Bitmap(outwidth, outheight)
                Using graphics As Graphics = Graphics.FromImage(SmoothLegBackgroundImage)
                    graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                    graphics.Clear(Color.Transparent)

                    Dim srcRectF As New RectangleF(tmpLegBackground.Width / 2.0F - clipWidth / 2.0F, tmpLegBackground.Height / 2.0F - clipLength / 2.0F, clipWidth, clipLength)
                    Dim dstRect As New RectangleF(0, 0, outwidth, outheight)
                    graphics.DrawImage(tmpLegBackground, dstRect, srcRectF, GraphicsUnit.Pixel)
                End Using
            End Using

            SmoothLegBackgroundCacheKey = cacheKey
        End If

        Return SmoothLegBackgroundImage
    End Function
#End Region
    ' REPLACED WITH SMOOTH: use DrawPositionOnBackgroundImageSmooth for interpolated map rendering.
    Public Function DrawPositionOnBackgroundImage(currentTimecode As Integer) As Bitmap
        ' Get the current position index based on the elapsed time
        Dim currentPosIndex As Integer = currentTimecode
        If Not IsNothing(TmpMainMap) Then
            TmpMainMap.Dispose()
        End If
        TmpMainMap = New Bitmap(MainMapImage.Width, MainMapImage.Height)

        ' Get the current position and previous positions for the tail line
        Dim currentPos As PointF = RouteImgPoints(currentPosIndex)
        Dim tailLineStartIndex As Integer = currentPosIndex - tailLineDurationSeconds
        If tailLineStartIndex < 0 Then tailLineStartIndex = 0
        Dim tailLineEndIndex As Integer = currentPosIndex
        Dim tailLinePoints As New List(Of PointF)
        For i As Integer = tailLineStartIndex To tailLineEndIndex
            tailLinePoints.Add(RouteImgPoints(i))
        Next

        ' Draw the dot and tail line on the background image
        Using g As Graphics = Graphics.FromImage(TmpMainMap)
            g.DrawImage(MainMapImage, 0, 0)

            ' Draw the tail line
            Dim tailLinePen As New Pen(tailLineColor, dotSize * dotTailRatio) '20240229 dotSize / 2
            If tailLinePoints.Count > 1 Then
                g.DrawLines(tailLinePen, tailLinePoints.ToArray())
            End If

            ' Draw the dot at the current position
            Dim dotSizeWithTail As Integer = dotSize '-20240229 CInt(dotSize * (1 + dotTailRatio))
            '+20240229
            Dim direction As Double
            direction = QRRoutePoints.RoutePoints(currentTimecode).Direction
            If _DotType = "Arrow" Then
                DrawArrowWithBarbs(g, currentPos, direction, dotSizeWithTail, dotColor)
            Else
                Dim dotBrush As New SolidBrush(dotColor)
                g.FillEllipse(dotBrush, CSng(currentPos.X - dotSizeWithTail / 2), CSng(currentPos.Y - dotSizeWithTail / 2), CSng(dotSizeWithTail), CSng(dotSizeWithTail))
            End If
        End Using

        Return TmpMainMap

    End Function
#Region "Smooth Map Rendering"
    Public Function DrawPositionOnBackgroundImageSmooth(currentTimecode As Double) As Bitmap
        Dim currentTime As Double = ClampTimeCode(currentTimecode)
        If Not IsNothing(TmpMainMap) Then
            TmpMainMap.Dispose()
        End If
        TmpMainMap = New Bitmap(MainMapImage.Width, MainMapImage.Height)

        Dim currentPos As PointF = GetRoutePointAtTime(currentTime)
        Dim tailLinePoints As List(Of PointF) = GetTailLinePointsAtTime(currentTime)

        Using g As Graphics = Graphics.FromImage(TmpMainMap)
            g.DrawImage(MainMapImage, 0, 0)

            Using tailLinePen As New Pen(tailLineColor, dotSize * dotTailRatio)
                If tailLinePoints.Count > 1 Then
                    g.DrawLines(tailLinePen, tailLinePoints.ToArray())
                End If
            End Using

            Dim dotSizeWithTail As Integer = dotSize
            Dim direction As Double = GetArrowDirectionAtTime(currentTime)
            If _DotType = "Arrow" Then
                DrawArrowWithBarbs(g, currentPos, CSng(direction), dotSizeWithTail, dotColor)
            Else
                Using dotBrush As New SolidBrush(dotColor)
                    g.FillEllipse(dotBrush, CSng(currentPos.X - dotSizeWithTail / 2), CSng(currentPos.Y - dotSizeWithTail / 2), CSng(dotSizeWithTail), CSng(dotSizeWithTail))
                End Using
            End If
        End Using

        Return TmpMainMap
    End Function
#End Region


    Private Function FillRoundedRectangle(brush As Brush, rect As Rectangle, radius As Integer) As GraphicsPath
        Dim path As New Drawing2D.GraphicsPath()
        path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90)
        path.AddLine(rect.X + radius, rect.Y, rect.X + rect.Width - radius, rect.Y)
        path.AddArc(rect.X + rect.Width - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90)
        path.AddLine(rect.X + rect.Width, rect.Y + radius, rect.X + rect.Width, rect.Y + rect.Height - radius)
        path.AddArc(rect.X + rect.Width - radius * 2, rect.Y + rect.Height - radius * 2, radius * 2, radius * 2, 0, 90)
        path.AddLine(rect.X + rect.Width - radius, rect.Y + rect.Height, rect.X + radius, rect.Y + rect.Height)
        path.AddArc(rect.X, rect.Y + rect.Height - radius * 2, radius * 2, radius * 2, 90, 90)
        path.AddLine(rect.X, rect.Y + rect.Height - radius, rect.X, rect.Y + radius)
        path.CloseFigure()
        Dim gp As New GraphicsPath()
        gp.AddPath(path, True)
        Return gp
    End Function
    Function roundrectangle(originalImage As Bitmap, radius As Integer, Optional squareframe As Boolean = False) As Bitmap
        Dim roundedImage As New Bitmap(originalImage.Width, originalImage.Height)
        Dim graphics As Graphics = Graphics.FromImage(roundedImage)
        Using graphics
            graphics.Clear(Color.Transparent)
            graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

            graphics.CompositingMode = Drawing2D.CompositingMode.SourceOver
            graphics.CompositingQuality = Drawing2D.CompositingQuality.HighQuality
            Dim rect As New Rectangle(0, 0, originalImage.Width, originalImage.Height)
            Dim path As GraphicsPath = FillRoundedRectangle(New SolidBrush(Color.White), rect, radius)
            graphics.SetClip(path)
            Using brush As New SolidBrush(FrameColor)
                graphics.FillPath(brush, path)
            End Using
            Dim rect2 As New Rectangle(FrameWidth, FrameWidth, originalImage.Width - FrameWidth * 2, originalImage.Height - FrameWidth * 2)
            Dim path2 As GraphicsPath = FillRoundedRectangle(New SolidBrush(Color.White), rect2, radius)
            graphics.SetClip(path2)
            graphics.DrawImage(originalImage, 0, 0)
            If squareframe Then
                ' Apply square frame
                graphics.ResetClip()
                Dim frameRect As New Rectangle(0, 0, originalImage.Width, originalImage.Height)
                frameRect.Inflate(-sqFrameSize, -sqFrameSize)
                Using pen As New Pen(SqFramecolor, sqFrameSize)
                    graphics.DrawRectangle(pen, frameRect)
                End Using
            End If
        End Using
        Return roundedImage

    End Function





    ' Hjælpefunktion til afrundet rektangel-sti
    Private Function RoundedRectanglePath(rect As Rectangle, radius As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        Dim d As Integer = radius * 2

        path.AddArc(rect.X, rect.Y, d, d, 180, 90)
        path.AddArc(rect.X + rect.Width - d, rect.Y, d, d, 270, 90)
        path.AddArc(rect.X + rect.Width - d, rect.Y + rect.Height - d, d, d, 0, 90)
        path.AddArc(rect.X, rect.Y + rect.Height - d, d, d, 90, 90)
        path.CloseFigure()

        Return path
    End Function

    ' LEGACY UNUSED HELPER: older angle smoothing variant kept for reference.
    Function SmoothedAngle(timeCode As Integer, windowSize As Integer) As Double
        ' Calculate the start and end indices of the moving average window

        Dim endIdx As Integer
        Dim avgAngle As Double

        'If (timeCode - LastTimeCode) > windowSize Then
        endIdx = timeCode - windowSize
        If endIdx < 0 Then endIdx = 0
        avgAngle = CalculateAngleV(RouteImgPoints(endIdx).X, RouteImgPoints(endIdx).Y, RouteImgPoints(timeCode).X, RouteImgPoints(timeCode).Y)
        'labelx.Text = points(timeCode).X.ToString("N0") + "," + points(timeCode).Y.ToString("N0") + ":" + points(endIdx).X.ToString("N0") + "," + points(endIdx).Y.ToString("N0") + ":" + avgAngle.ToString("N0")



        ' Apply a low-pass filter to the angle
        Dim filterstrength As Double = 0.05
        'If Double.IsInfinity(prevangle) Or Double.IsNaN(prevangle) Then prevangle = 0
        Dim diff As Double = avgAngle - prevangle
        If diff > 180 Then diff = 360 - diff
        If diff < -180 Then diff = 360 + diff
        Dim filteredAngle As Double = prevangle + filterstrength * diff
        prevangle = filteredAngle
        'labelx.Text += ":" + filteredAngle.ToString("N0")
        'If Double.IsInfinity(filteredAngle) Or Double.IsNaN(filteredAngle) Then filteredAngle = 0
        Return filteredAngle
        'Return avgAngle
    End Function
    ' LEGACY HELPER FOR REPLACED METHODS: only used by older non-smooth zoom/leg variants.
    Function SmoothedAngle2(timeCode As Integer, windowSize As Integer) As Double
        ' Calculate the start and end indices of the moving average window
        Dim endIdx As Integer = timeCode - windowSize
        If endIdx < 0 Then endIdx = 0

        ' Calculate the angle between the last point and the first point
        Dim startPoint As Point = RouteImgPoints(endIdx)
        Dim endPoint As Point = RouteImgPoints(timeCode)
        Dim deltaX As Double = endPoint.X - startPoint.X
        Dim deltaY As Double = startPoint.Y - endPoint.Y ' Invert deltaY to account for downward y-axis
        'Dim angle As Double = Math.Atan2(deltaY, deltaX) * (180 / Math.PI)
        Dim angle As Double = Math.Atan2(endPoint.Y - startPoint.Y, endPoint.X - startPoint.X)
        Dim angleDegrees As Double = (90 - angle * 180 / Math.PI) Mod 360

        Return angleDegrees
    End Function
    Function CalculateAngleV(x As Double, y As Double, v As Double, w As Double) As Double
        ' Calculate the direction vectors from the starting point

        Dim vec2x As Double = v - x
        Dim vec2y As Double = y - w
        Dim angle As Double
        If vec2y < 0 Then
            angle = (Math.PI - Math.Atan(vec2x / Math.Abs(vec2y))) * 180 / Math.PI
        End If
        If vec2y > 0 Then
            angle = (Math.Atan(vec2x / Math.Abs(vec2y))) * 180 / Math.PI
            If angle < 0 Then angle += 360
        End If
        If vec2y = 0 Then
            If vec2x > 0 Then angle = 90
            If vec2x < 0 Then angle = 360 - 90
            If vec2x = 0 Then angle = 0
        End If
        'labelx.Text = angle.ToString("N1")


        Return angle
    End Function
    ' REPLACED WITH SMOOTH: use ZoomImageSmooth for interpolated zoom rendering.
    Function ZoomImage(timeCode As Integer) As Bitmap
        Return ZoomImage3(timeCode, ZoomWidth, ZoomHeight, ZoomRad)
    End Function
#Region "Smooth Zoom Entry Point"
    Function ZoomImageSmooth(timeCode As Double) As Bitmap
        Return ZoomImage3Smooth(timeCode, ZoomWidth, ZoomHeight, ZoomRad)
    End Function
#End Region

    ' LEGACY BACKUP COPY: older zoom implementation kept for comparison/reference.
    Function ZoomImage2(timeCode As Integer, pxwidth As Integer, pxheight As Integer, pxroundrad As Integer) As Bitmap
        Dim height As Double = pxheight
        Dim width As Double = pxwidth
        Dim angle As Double
        Dim Middlepoint As Point = RouteImgPoints(timeCode)
        ZoomWidth = pxwidth
        ZoomHeight = pxheight
        ZoomRad = pxroundrad
        If Not IsNothing(tmpZLImg) Then
            tmpZLImg.Dispose()
        End If
        If IsNothing(TmpMainMap) Then MessageBox.Show("Intet billede")
        tmpZLImg = New Bitmap(TmpMainMap.Width, TmpMainMap.Height)
        Using g As Graphics = Graphics.FromImage(tmpZLImg)
            g.TranslateTransform(Middlepoint.X, Middlepoint.Y)
            angle = SmoothedAngle2(timeCode, 25)
            If IsNumeric(angle) Then g.RotateTransform(angle + 180)
            g.TranslateTransform(-Middlepoint.X, -Middlepoint.Y)
            g.DrawImage(TmpMainMap, New PointF(0, 0))
        End Using
        Dim startx, starty As Integer
        startx = Middlepoint.X - width / 2
        If startx < 0 Then startx = 0
        starty = Middlepoint.Y - height / 2
        If starty < 0 Then starty = 0
        Dim rect As Rectangle = New Rectangle(startx, starty, width, height)

        ' Create a new bitmap to hold the cut image
        If Not IsNothing(ZoomMapImage) Then
            ZoomMapImage.Dispose()
        End If
        ZoomMapImage = New Bitmap(rect.Width, rect.Height)


        ' Create a Graphics object from the new bitmap
        Using graphics As Graphics = Graphics.FromImage(ZoomMapImage)
            graphics.Clear(Color.Transparent)
            graphics.SmoothingMode = SmoothingMode.AntiAlias
            graphics.CompositingMode = CompositingMode.SourceOver
            graphics.CompositingQuality = CompositingQuality.HighQuality

            ' Create a rounded rectangle path for clipping
            Dim path As GraphicsPath = FillRoundedRectangle(New SolidBrush(Color.White), New Rectangle(0, 0, ZoomMapImage.Width, ZoomMapImage.Height), pxroundrad)
            graphics.SetClip(path)
            Using brush As New SolidBrush(FrameColor)
                graphics.FillPath(brush, path)
            End Using

            ' Create a rectangle path for the inner image
            Dim innerRect As New Rectangle(FrameWidth, FrameWidth, ZoomMapImage.Width - FrameWidth * 2, ZoomMapImage.Height - FrameWidth * 2)
            Dim innerPath As GraphicsPath = FillRoundedRectangle(New SolidBrush(Color.White), innerRect, pxroundrad)
            graphics.SetClip(innerPath)
            graphics.DrawImage(tmpZLImg, -rect.X + FrameWidth, -rect.Y + FrameWidth)
        End Using

        ' Draw the cut image onto the new bitmap
        Return ZoomMapImage
    End Function
    ' LEGACY BACKUP COPY: superseded older zoom variant kept for reference.
    Function ZoomImage3_backup(timeCode As Integer, OutWidth As Integer, OutHeight As Integer, pxroundrad As Integer, Optional fromAdj As Boolean = True) As Bitmap
        Dim startPoint As Point
        Dim pxClipLength As Integer
        Dim pxClipWidth As Single
        'Try
        Dim angle As Double
        If timeCode < 0 Then timeCode = 0
        If timeCode > RouteImgPoints.Count - 1 Then timeCode = RouteImgPoints.Count - 1
        Dim Middlepoint As Point = RouteImgPoints(timeCode)
        ZoomWidth = OutWidth
        ZoomHeight = OutHeight
        ZoomRad = pxroundrad
        Dim Windowsize As Integer = 25 'smooth frames
        'Dim pxClipLength As Integer = Int(Math.Round(200 * ZoomZoom)) '-25022024 the size of the zoomimage
        If ZoomZoom < 0.001 Then ZoomZoom = 1
        pxClipLength = Int(Math.Round(OutHeight / ZoomZoom)) 'Y length +25022024
        pxClipWidth = Int(Math.Round(OutWidth / ZoomZoom)) 'X length +25022024
        Dim endIdx As Integer = timeCode - Windowsize
        If endIdx < 0 Then endIdx = 0

        ' Calculate the angle between the last point and the first point
        startPoint = RouteImgPoints(endIdx)
        Dim endPoint As Point = RouteImgPoints(timeCode)
        Dim deltaX As Integer = endPoint.X - startPoint.X
        Dim deltaY As Integer = endPoint.Y - startPoint.Y
        Dim distance As Single = (deltaX * deltaX + deltaY * deltaY) ^ 0.5F
        Dim UnitV As New PointF

        If distance = 0 Then
            UnitV.X = 1
            UnitV.Y = 1
        Else
            UnitV.X = (endPoint.X - startPoint.X) / distance
            UnitV.Y = (endPoint.Y - startPoint.Y) / distance
        End If
        angle = SmoothedAngle2(timeCode, Windowsize)

        'Dim Scale As Single = pxClipLength / OutHeight '-25022024
        'Dim pxClipWidth As Single = OutWidth * Scale '-25022024 Scale to output width

        'Ny
        Dim rWH As Single = (pxClipWidth ^ 2 + pxClipLength ^ 2) ^ 0.5
        Dim MinX As Single = Middlepoint.X - rWH / 2
        Dim MinY As Single = Middlepoint.Y - rWH / 2
        Dim SrcRect As New Rectangle(MinX, MinY, rWH, rWH) ' kvadrat
        'Dim SrcRect As New Rectangle(MinX, MinY, MaxWH, MaxWH)
        If Not IsNothing(tmpZLImg) Then
            'tmpZLImg.Dispose()
        End If
        If IsNothing(TmpMainMap) Then MessageBox.Show("Intet billede")
        tmpZLImg = New Bitmap(SrcRect.Width, SrcRect.Height)
        Using g As Graphics = Graphics.FromImage(tmpZLImg)
            g.TranslateTransform(tmpZLImg.Width / 2, tmpZLImg.Height / 2)

            If IsNumeric(angle) Then g.RotateTransform(angle + 180)
            g.TranslateTransform(-tmpZLImg.Width / 2, -tmpZLImg.Height / 2)
            g.DrawImage(TmpMainMap, New RectangleF(0, 0, tmpZLImg.Width, tmpZLImg.Height), SrcRect, GraphicsUnit.Pixel)
        End Using


        ' Create a new bitmap to hold the cut image
        If Not IsNothing(ZoomMapImage) Then
            ZoomMapImage.Dispose()
        End If
        'ZoomMapImage = New Bitmap(rect.Width, rect.Height)
        ZoomMapImage = New Bitmap(OutWidth, OutHeight)


        ' Create a Graphics object from the new bitmap
        Using graphics As Graphics = Graphics.FromImage(ZoomMapImage)
            '*** Clip and round rectangle
            graphics.Clear(Color.Transparent)
            graphics.SmoothingMode = SmoothingMode.AntiAlias
            graphics.CompositingMode = CompositingMode.SourceOver
            graphics.CompositingQuality = CompositingQuality.HighQuality



            Dim path As GraphicsPath = FillRoundedRectangle(New SolidBrush(Color.White), New Rectangle(0, 0, ZoomMapImage.Width, ZoomMapImage.Height), pxroundrad)
            graphics.SetClip(path)
            Using brush As New SolidBrush(FrameColor)
                graphics.FillPath(brush, path)
            End Using

            ' Create a rectangle path for the inner image
            Dim innerRect As New Rectangle(FrameWidth, FrameWidth, ZoomMapImage.Width - FrameWidth * 2, ZoomMapImage.Height - FrameWidth * 2)
            Dim innerPath As GraphicsPath = FillRoundedRectangle(New SolidBrush(Color.White), innerRect, pxroundrad)
            graphics.SetClip(innerPath)
            '*** End Clip and round


            'Dim angleRad As Double = (angle + 180) * PI / 180

            '' Calculate the width and height in the X and Y coordinate system
            'Dim rectWidth As Single = Max(OutWidth, OutHeight) '= Abs(width * Cos(angleRad)) + Abs(height * Sin(angleRad))
            'Dim rectHeight As Single = rectWidth '= Abs(width * Sin(angleRad)) + Abs(height * Cos(angleRad))

            Dim dstRect As New Rectangle(0, 0, ZoomMapImage.Width, ZoomMapImage.Height)

            graphics.DrawImage(tmpZLImg, dstRect, New RectangleF(tmpZLImg.Width / 2 - pxClipWidth / 2, tmpZLImg.Height / 2 - pxClipLength / 2, pxClipWidth, pxClipLength), GraphicsUnit.Pixel)

        End Using
        Return ZoomMapImage
        'Catch ex As Exception
        '   LogFejl($"{ex.Message}: t={timeCode} w={OutWidth} h={OutHeight} px={pxroundrad} xmln={RouteImgPoints.Count} mpx={RouteImgPoints(timeCode).X} mpy={RouteImgPoints(timeCode).Y} sta={startPoint.X},{startPoint.Y} zz={ZoomZoom} cll={pxClipLength} clw{pxClipWidth}")
        ' Disposér tidligere ZoomMapImage hvis det findes
        '  If Not IsNothing(ZoomMapImage) Then
        ' ZoomMapImage.Dispose()
        'End If

        ' Opret ny tom bitmap med ønsket størrelse
        'ZoomMapImage = New Bitmap(OutWidth, OutHeight)
        'Using g As Graphics = Graphics.FromImage(ZoomMapImage)
        'g.Clear(Color.LightGray) ' eller fx Color.LightGray for "fejlindikator"
        'End Using

        ' Du kan evt. logge fejlen her hvis nødvendigt

        'Return ZoomMapImage
        'End Try
        ' Draw the cut image onto the new bitmap
        'Return tmpZLImg

    End Function

    ' REPLACED WITH SMOOTH: use ZoomImage3Smooth for interpolated zoom rendering.
    Function ZoomImage3(timeCode As Integer, OutWidth As Integer, OutHeight As Integer, pxroundrad As Integer, Optional fromAdj As Boolean = True) As Bitmap
        Dim startPoint As Point
        Dim pxClipLength As Integer
        Dim pxClipWidth As Single
        'Try
        Dim angle As Double
        If timeCode < 0 Then timeCode = 0
        If timeCode > RouteImgPoints.Count - 1 Then timeCode = RouteImgPoints.Count - 1
        Dim Middlepoint As Point = RouteImgPoints(timeCode)
        ZoomWidth = OutWidth
        ZoomHeight = OutHeight
        ZoomRad = pxroundrad
        Dim Windowsize As Integer = 25 'smooth frames
        'Dim pxClipLength As Integer = Int(Math.Round(200 * ZoomZoom)) '-25022024 the size of the zoomimage
        If ZoomZoom < 0.001 Then ZoomZoom = 1
        pxClipLength = Int(Math.Round(OutHeight / ZoomZoom)) 'Y length +25022024
        pxClipWidth = Int(Math.Round(OutWidth / ZoomZoom)) 'X length +25022024
        Dim endIdx As Integer = timeCode - Windowsize
        If endIdx < 0 Then endIdx = 0

        ' Calculate the angle between the last point and the first point
        startPoint = RouteImgPoints(endIdx)
        Dim endPoint As Point = RouteImgPoints(timeCode)
        Dim deltaX As Integer = endPoint.X - startPoint.X
        Dim deltaY As Integer = endPoint.Y - startPoint.Y
        Dim distance As Single = (deltaX * deltaX + deltaY * deltaY) ^ 0.5F
        Dim UnitV As New PointF

        If distance = 0 Then
            UnitV.X = 1
            UnitV.Y = 1
        Else
            UnitV.X = (endPoint.X - startPoint.X) / distance
            UnitV.Y = (endPoint.Y - startPoint.Y) / distance
        End If
        angle = SmoothedAngle2(timeCode, Windowsize)

        'Dim Scale As Single = pxClipLength / OutHeight '-25022024
        'Dim pxClipWidth As Single = OutWidth * Scale '-25022024 Scale to output width

        'Ny
        Dim rWH As Single = (pxClipWidth ^ 2 + pxClipLength ^ 2) ^ 0.5
        Dim MinX As Single = Middlepoint.X - rWH / 2
        Dim MinY As Single = Middlepoint.Y - rWH / 2
        Dim SrcRect As New Rectangle(MinX, MinY, rWH, rWH) ' kvadrat
        'Dim SrcRect As New Rectangle(MinX, MinY, MaxWH, MaxWH)
        If Not IsNothing(tmpZLImg) Then
            'tmpZLImg.Dispose()
        End If
        If IsNothing(TmpMainMap) Then MessageBox.Show("Intet billede")
        tmpZLImg = New Bitmap(SrcRect.Width, SrcRect.Height)
        Using g As Graphics = Graphics.FromImage(tmpZLImg)
            g.TranslateTransform(tmpZLImg.Width / 2, tmpZLImg.Height / 2)

            If IsNumeric(angle) Then g.RotateTransform(angle + 180)
            g.TranslateTransform(-tmpZLImg.Width / 2, -tmpZLImg.Height / 2)
            g.DrawImage(TmpMainMap, New RectangleF(0, 0, tmpZLImg.Width, tmpZLImg.Height), SrcRect, GraphicsUnit.Pixel)
        End Using


        ' Create a new bitmap to hold the cut image
        If Not IsNothing(ZoomMapImage) Then
            ZoomMapImage.Dispose()
        End If
        'ZoomMapImage = New Bitmap(rect.Width, rect.Height)
        ZoomMapImage = New Bitmap(OutWidth, OutHeight)


        ' Create a Graphics object from the new bitmap
        ' ny ramme funktion med feathered
        ' Create a Graphics object from the new bitmap
        Using graphics As Graphics = Graphics.FromImage(ZoomMapImage)
            graphics.Clear(Color.Transparent)
            graphics.SmoothingMode = SmoothingMode.AntiAlias
            graphics.CompositingMode = CompositingMode.SourceOver
            graphics.CompositingQuality = CompositingQuality.HighQuality
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic

            Dim dstRect As New Rectangle(0, 0, ZoomMapImage.Width, ZoomMapImage.Height)

            ' Tegn først billedet direkte uden clip
            graphics.DrawImage(tmpZLImg, dstRect,
                       New RectangleF(tmpZLImg.Width / 2 - pxClipWidth / 2, tmpZLImg.Height / 2 - pxClipLength / 2, pxClipWidth, pxClipLength),
                       GraphicsUnit.Pixel)

            ' Vælg mellem fast eller feathered ramme
            If FrameFeather Then
                ' Opret alpha-masken først

                Dim alphaMask As Bitmap = CreateAlphaMask(ZoomMapImage.Size, pxroundrad, FrameWidth, True)
                Dim featheredImage As Bitmap = ApplyAlphaMask(ZoomMapImage, alphaMask)

                ZoomMapImage.Dispose()


                ZoomMapImage = featheredImage
            Else
                '*** Fast afrundet ramme ***
                graphics.Clear(Color.Transparent)

                ' Tegn rammen først
                Dim outerPath As GraphicsPath = FillRoundedRectangle(New SolidBrush(Color.White), dstRect, pxroundrad)
                graphics.SetClip(outerPath)
                Using brush As New SolidBrush(FrameColor)
                    graphics.FillPath(brush, outerPath)
                End Using

                ' Klip det indre område til billedet
                Dim innerRect As New Rectangle(FrameWidth, FrameWidth, ZoomMapImage.Width - FrameWidth * 2, ZoomMapImage.Height - FrameWidth * 2)
                Dim innerPath As GraphicsPath = FillRoundedRectangle(New SolidBrush(Color.White), innerRect, pxroundrad)
                graphics.SetClip(innerPath)

                ' Tegn billedet kun i det indre område
                graphics.DrawImage(tmpZLImg, dstRect,
                           New RectangleF(tmpZLImg.Width / 2 - pxClipWidth / 2,
                                          tmpZLImg.Height / 2 - pxClipLength / 2,
                                          pxClipWidth, pxClipLength),
                           GraphicsUnit.Pixel)

            End If
        End Using

        Return ZoomMapImage
        'Catch ex As Exception
        '   LogFejl($"{ex.Message}: t={timeCode} w={OutWidth} h={OutHeight} px={pxroundrad} xmln={RouteImgPoints.Count} mpx={RouteImgPoints(timeCode).X} mpy={RouteImgPoints(timeCode).Y} sta={startPoint.X},{startPoint.Y} zz={ZoomZoom} cll={pxClipLength} clw{pxClipWidth}")
        ' Disposér tidligere ZoomMapImage hvis det findes
        '  If Not IsNothing(ZoomMapImage) Then
        ' ZoomMapImage.Dispose()
        'End If

        ' Opret ny tom bitmap med ønsket størrelse
        'ZoomMapImage = New Bitmap(OutWidth, OutHeight)
        'Using g As Graphics = Graphics.FromImage(ZoomMapImage)
        'g.Clear(Color.LightGray) ' eller fx Color.LightGray for "fejlindikator"
        'End Using

        ' Du kan evt. logge fejlen her hvis nødvendigt

        'Return ZoomMapImage
        'End Try
        ' Draw the cut image onto the new bitmap
        'Return tmpZLImg

    End Function
#Region "Smooth Zoom Implementation"
    Function ZoomImage3Smooth(timeCode As Double, OutWidth As Integer, OutHeight As Integer, pxroundrad As Integer, Optional fromAdj As Boolean = True) As Bitmap
        Dim pxClipLength As Integer
        Dim pxClipWidth As Single
        Dim safeTimeCode As Double = ClampTimeCode(timeCode)
        Dim middlepoint As PointF = GetRoutePointAtTime(safeTimeCode)
        ZoomWidth = OutWidth
        ZoomHeight = OutHeight
        ZoomRad = pxroundrad
        Dim windowsize As Integer = CInt(Math.Round(SmoothHeadingWindowSeconds))

        If ZoomZoom < 0.001 Then ZoomZoom = 1
        pxClipLength = Int(Math.Round(OutHeight / ZoomZoom))
        pxClipWidth = Int(Math.Round(OutWidth / ZoomZoom))
        Dim angle As Double = GetSmoothedAngleAtTime(safeTimeCode, windowsize)

        Dim rWH As Single = CSng((pxClipWidth ^ 2 + pxClipLength ^ 2) ^ 0.5)
        Dim srcSize As Integer = Math.Max(1, CInt(Math.Ceiling(rWH)))
        Dim srcRect As New RectangleF(middlepoint.X - srcSize / 2.0F, middlepoint.Y - srcSize / 2.0F, srcSize, srcSize)
        Dim overlayMap As Bitmap = GetSmoothOverlayMap(safeTimeCode)
        Dim tmpOverlayImg As Bitmap = Nothing

        If Not IsNothing(tmpZLImg) Then tmpZLImg.Dispose()
        tmpZLImg = New Bitmap(srcSize, srcSize)
        Using g As Graphics = Graphics.FromImage(tmpZLImg)
            g.TranslateTransform(tmpZLImg.Width / 2.0F, tmpZLImg.Height / 2.0F)
            If IsNumeric(angle) Then g.RotateTransform(CSng(angle + 180))
            g.TranslateTransform(-tmpZLImg.Width / 2.0F, -tmpZLImg.Height / 2.0F)
            g.DrawImage(MainMapImage, New RectangleF(0, 0, tmpZLImg.Width, tmpZLImg.Height), srcRect, GraphicsUnit.Pixel)
        End Using
        tmpOverlayImg = New Bitmap(srcSize, srcSize)
        Using g As Graphics = Graphics.FromImage(tmpOverlayImg)
            g.Clear(Color.Transparent)
            g.TranslateTransform(tmpOverlayImg.Width / 2.0F, tmpOverlayImg.Height / 2.0F)
            If IsNumeric(angle) Then g.RotateTransform(CSng(angle + 180))
            g.TranslateTransform(-tmpOverlayImg.Width / 2.0F, -tmpOverlayImg.Height / 2.0F)
            g.DrawImage(overlayMap, New RectangleF(0, 0, tmpOverlayImg.Width, tmpOverlayImg.Height), srcRect, GraphicsUnit.Pixel)
        End Using

        If Not IsNothing(ZoomMapImage) Then ZoomMapImage.Dispose()
        ZoomMapImage = New Bitmap(OutWidth, OutHeight)

        Using graphics As Graphics = Graphics.FromImage(ZoomMapImage)
            graphics.Clear(Color.Transparent)
            graphics.SmoothingMode = SmoothingMode.AntiAlias
            graphics.CompositingMode = CompositingMode.SourceOver
            graphics.CompositingQuality = CompositingQuality.HighQuality
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic

            Dim dstRect As New Rectangle(0, 0, ZoomMapImage.Width, ZoomMapImage.Height)
            graphics.DrawImage(tmpZLImg, dstRect, New RectangleF(tmpZLImg.Width / 2.0F - pxClipWidth / 2.0F, tmpZLImg.Height / 2.0F - pxClipLength / 2.0F, pxClipWidth, pxClipLength), GraphicsUnit.Pixel)
            graphics.DrawImage(tmpOverlayImg, dstRect, New RectangleF(tmpOverlayImg.Width / 2.0F - pxClipWidth / 2.0F, tmpOverlayImg.Height / 2.0F - pxClipLength / 2.0F, pxClipWidth, pxClipLength), GraphicsUnit.Pixel)

            If FrameFeather Then
                Dim alphaMask As Bitmap = CreateAlphaMask(ZoomMapImage.Size, pxroundrad, FrameWidth, True)
                Dim featheredImage As Bitmap = ApplyAlphaMask(ZoomMapImage, alphaMask)
                ZoomMapImage.Dispose()
                ZoomMapImage = featheredImage
            Else
                graphics.Clear(Color.Transparent)

                Dim outerPath As GraphicsPath = FillRoundedRectangle(New SolidBrush(Color.White), dstRect, pxroundrad)
                graphics.SetClip(outerPath)
                Using brush As New SolidBrush(FrameColor)
                    graphics.FillPath(brush, outerPath)
                End Using

                Dim innerRect As New Rectangle(FrameWidth, FrameWidth, ZoomMapImage.Width - FrameWidth * 2, ZoomMapImage.Height - FrameWidth * 2)
                Dim innerPath As GraphicsPath = FillRoundedRectangle(New SolidBrush(Color.White), innerRect, pxroundrad)
                graphics.SetClip(innerPath)
                graphics.DrawImage(tmpZLImg, dstRect, New RectangleF(tmpZLImg.Width / 2.0F - pxClipWidth / 2.0F, tmpZLImg.Height / 2.0F - pxClipLength / 2.0F, pxClipWidth, pxClipLength), GraphicsUnit.Pixel)
                graphics.DrawImage(tmpOverlayImg, dstRect, New RectangleF(tmpOverlayImg.Width / 2.0F - pxClipWidth / 2.0F, tmpOverlayImg.Height / 2.0F - pxClipLength / 2.0F, pxClipWidth, pxClipLength), GraphicsUnit.Pixel)
            End If
        End Using
        If tmpOverlayImg IsNot Nothing Then tmpOverlayImg.Dispose()

        Return ZoomMapImage
    End Function
#End Region
    'alternativ alphamask
    Private Function CreateAlphaMask(size As Size, radius As Integer, featherWidth As Integer, IsZoom As Boolean) As Bitmap
        Dim bmp As Bitmap
        ' Begræns featherWidth til halvdelen af billeddimension
        featherWidth = Math.Min(featherWidth, Math.Min(size.Width, size.Height) \ 2 - 1)
        'Mask bmp's created in ResetAlphaMasks
        If IsZoom Then
            bmp = alphaMaskZoom
        Else
            bmp = alphaMaskLeg
        End If

        Using g As Graphics = Graphics.FromImage(bmp)
            g.Clear(Color.Transparent)
            g.SmoothingMode = SmoothingMode.AntiAlias

            ' Yderrammen (inkl. feather)
            Dim outerRect As New Rectangle(0, 0, size.Width, size.Height)
            Dim outerPath As GraphicsPath = RoundedRectanglePath2(outerRect, radius)

            Using pgb As New PathGradientBrush(outerPath)
                ' Midten skal være fuldt opak (255), kanten skal være helt transparent (0)
                pgb.CenterColor = Color.FromArgb(255, 255, 255, 255)
                pgb.SurroundColors = {Color.FromArgb(0, 255, 255, 255)}

                ' Feather-effekten skal strække sig indad fra kanten
                Dim focus = 1 - (featherWidth / (Math.Min(size.Width, size.Height) / 2))
                pgb.FocusScales = New PointF(focus, focus)

                g.FillPath(pgb, outerPath)
            End Using
        End Using

        Return bmp
    End Function



    Private Function ApplyAlphaMask(source As Bitmap, mask As Bitmap) As Bitmap
        Dim result As New Bitmap(source.Width, source.Height, PixelFormat.Format32bppArgb)

        For y As Integer = 0 To source.Height - 1
            For x As Integer = 0 To source.Width - 1
                Dim srcPixel As Color = source.GetPixel(x, y)
                Dim maskPixel As Color = mask.GetPixel(x, y)

                ' Brug A-kanalen fra masken
                Dim newAlpha As Byte = maskPixel.A
                Dim newColor As Color = Color.FromArgb(newAlpha, srcPixel.R, srcPixel.G, srcPixel.B)
                result.SetPixel(x, y, newColor)
            Next
        Next

        Return result
    End Function

    ' Ny funktion kun til feathered path (da din FillRoundedRectangle har brush parameter unødigt her)
    Private Function RoundedRectanglePath2(rect As Rectangle, radius As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        Dim d As Integer = radius * 2

        path.AddArc(rect.X, rect.Y, d, d, 180, 90)
        path.AddArc(rect.X + rect.Width - d, rect.Y, d, d, 270, 90)
        path.AddArc(rect.X + rect.Width - d, rect.Y + rect.Height - d, d, d, 0, 90)
        path.AddArc(rect.X, rect.Y + rect.Height - d, d, d, 90, 90)
        path.CloseFigure()

        Return path
    End Function


    Private Sub LogFejl(besked As String, Optional append As Boolean = True)
        Try
            Dim logMappe As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            Dim logFilPath As String = Path.Combine(logMappe, "HeadcamErr.txt")
            Using writer As StreamWriter = New StreamWriter(logFilPath, append)
                writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {besked}")
            End Using
        Catch ex As Exception
            ' Hvis logning fejler, kan du evt. vise en besked, logge andetsteds eller ignorere det
        End Try
    End Sub
    ' REPLACED WITH SMOOTH: use LapImageSmooth for interpolated leg rendering.
    Function LapImage(CurrentTime As Integer) As Bitmap
        Return LapImage2(CurrentTime, LegMargin, LegWidth, LegHeight, LegRad)
    End Function
#Region "Smooth Leg Entry Point"
    Function LapImageSmooth(CurrentTime As Double) As Bitmap
        Return LapImage2Smooth(CurrentTime, LegMargin, LegWidth, LegHeight, LegRad)
    End Function
#End Region
    ' LEGACY BACKUP COPY: superseded older leg variant kept for reference.
    Function LapImage2_backup(CurrentTime As Integer, pxmarginheight As Integer, outwidth As Integer, outheight As Integer, pxroundrad As Integer, Optional SqFrame As Boolean = False) As Bitmap
        If CurrentTime < 0 Then CurrentTime = 0
        If CurrentTime > QRRoutePoints.RoutePoints.Count - 1 Then CurrentTime = QRRoutePoints.RoutePoints.Count - 1
        Dim Lap As Integer = QRRoutePoints.RoutePoints(CurrentTime).LapNumber - 1
        Dim startpointL As PointF = Point.Round(QRRoutePoints.ImgLapVectors(Lap).StartPoint)
        Dim endPointL As PointF = Point.Round(QRRoutePoints.ImgLapVectors(Lap).EndPoint)

        LegWidth = outwidth
        LegHeight = outheight
        LegRad = pxroundrad
        LegMargin = pxmarginheight
        ' Calculate the rotation angle of the line
        Dim deltaX As Integer = endPointL.X - startpointL.X
        Dim deltaY As Integer = endPointL.Y - startpointL.Y
        Dim rotationAngle As Single = Math.Atan2(endPointL.Y - startpointL.Y, endPointL.X - startpointL.X)
        Dim angleDegrees As Double = (90 - rotationAngle * 180 / Math.PI) Mod 360 + 180

        ' Calculate the distance and midpoint between the two points
        Dim distance As Single = (deltaX * deltaX + deltaY * deltaY) ^ 0.5F
        Dim midpoint As New Point((startpointL.X + endPointL.X) \ 2, (startpointL.Y + endPointL.Y) \ 2)



        If Not IsNothing(LegMapImage) Then
            LegMapImage.Dispose()
        End If

        LegMapImage = New Bitmap(outwidth, outheight)
        Dim ClipLength As Single = distance + pxmarginheight
        Dim Scale As Single = ClipLength / outheight
        Dim ClipWidth As Single = outwidth * Scale

        'Ny
        Dim rWH As Single = (ClipWidth ^ 2 + ClipLength ^ 2) ^ 0.5
        Dim MinX As Single = midpoint.X - rWH / 2
        Dim MinY As Single = midpoint.Y - rWH / 2
        Dim SrcRect As New Rectangle(MinX, MinY, rWH, rWH) ' kvadrat
        If Not IsNothing(tmpZLImg) Then tmpZLImg.Dispose()
        'tmpZLImg = New Bitmap(TmpMainMap.Width, TmpMainMap.Height)
        tmpZLImg = New Bitmap(SrcRect.Width, SrcRect.Height)
        Using g As Graphics = Graphics.FromImage(tmpZLImg)
            g.TranslateTransform(tmpZLImg.Width / 2, tmpZLImg.Height / 2)

            If IsNumeric(angleDegrees) Then g.RotateTransform(angleDegrees)
            g.TranslateTransform(-tmpZLImg.Width / 2, -tmpZLImg.Height / 2)
            g.DrawImage(TmpMainMap, New RectangleF(0, 0, tmpZLImg.Width, tmpZLImg.Height), SrcRect, GraphicsUnit.Pixel)
        End Using

        ' Vælg mellem fast eller feathered ramme
        If FrameFeather Then
            ' Brug tmpZLImg direkte og anvend alpha-masken
            Dim alphaMask As Bitmap = CreateAlphaMask(tmpZLImg.Size, pxroundrad, FrameWidth, False)
            Dim featheredImage As Bitmap = ApplyAlphaMask(tmpZLImg, alphaMask)

            If LegMapImage IsNot Nothing Then LegMapImage.Dispose()
            LegMapImage = featheredImage
        Else
            Using graphics As Graphics = Graphics.FromImage(LegMapImage)
                Dim NewMidpoint As New PointF(tmpZLImg.Width / 2, tmpZLImg.Height / 2)
                ' Set the interpolation mode to high quality for better image quality
                graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic

                graphics.Clear(Color.Transparent)
                graphics.SmoothingMode = SmoothingMode.AntiAlias
                graphics.CompositingMode = CompositingMode.SourceOver
                graphics.CompositingQuality = CompositingQuality.HighQuality



                '*** Fast afrundet ramme ***
                graphics.Clear(Color.Transparent)
                ' Create a rounded rectangle path for clipping
                Dim path As GraphicsPath = FillRoundedRectangle(New SolidBrush(Color.White), New Rectangle(0, 0, LegMapImage.Width, LegMapImage.Height), pxroundrad)
                graphics.SetClip(path)
                Using brush As New SolidBrush(FrameColor)
                    graphics.FillPath(brush, path)
                End Using

                ' Create a rectangle path for the inner image
                Dim innerRect As New Rectangle(FrameWidth, FrameWidth, LegMapImage.Width - FrameWidth * 2, LegMapImage.Height - FrameWidth * 2)
                Dim innerPath As GraphicsPath = FillRoundedRectangle(New SolidBrush(Color.White), innerRect, pxroundrad)
                graphics.SetClip(innerPath)

                ' Draw the cropped region onto the graphics object
                Dim dstRect As New Rectangle(0, 0, LegMapImage.Width, LegMapImage.Height)
                graphics.DrawImage(tmpZLImg, dstRect, New RectangleF(tmpZLImg.Width / 2 - ClipWidth / 2, tmpZLImg.Height / 2 - ClipLength / 2, ClipWidth, ClipLength), GraphicsUnit.Pixel)


            End Using
        End If

        Return LegMapImage

    End Function
    ' REPLACED WITH SMOOTH: use LapImage2Smooth for interpolated leg rendering.
    Function LapImage2(CurrentTime As Integer, pxmarginheight As Integer, outwidth As Integer, outheight As Integer, pxroundrad As Integer, Optional SqFrame As Boolean = False) As Bitmap
        If CurrentTime < 0 Then CurrentTime = 0
        If CurrentTime > QRRoutePoints.RoutePoints.Count - 1 Then CurrentTime = QRRoutePoints.RoutePoints.Count - 1
        Dim Lap As Integer = QRRoutePoints.RoutePoints(CurrentTime).LapNumber - 1
        Dim startpointL As PointF = Point.Round(QRRoutePoints.ImgLapVectors(Lap).StartPoint)
        Dim endPointL As PointF = Point.Round(QRRoutePoints.ImgLapVectors(Lap).EndPoint)

        LegWidth = outwidth
        LegHeight = outheight
        LegRad = pxroundrad
        LegMargin = pxmarginheight
        ' Calculate the rotation angle of the line
        Dim deltaX As Integer = endPointL.X - startpointL.X
        Dim deltaY As Integer = endPointL.Y - startpointL.Y
        Dim rotationAngle As Single = Math.Atan2(endPointL.Y - startpointL.Y, endPointL.X - startpointL.X)
        Dim angleDegrees As Double = (90 - rotationAngle * 180 / Math.PI) Mod 360 + 180

        ' Calculate the distance and midpoint between the two points
        Dim distance As Single = (deltaX * deltaX + deltaY * deltaY) ^ 0.5F
        Dim midpoint As New Point((startpointL.X + endPointL.X) \ 2, (startpointL.Y + endPointL.Y) \ 2)

        Dim ClipLength As Single = distance + pxmarginheight
        Dim Scale As Single = ClipLength / outheight
        Dim ClipWidth As Single = outwidth * Scale

        'Ny
        Dim rWH As Single = (ClipWidth ^ 2 + ClipLength ^ 2) ^ 0.5
        Dim MinX As Single = midpoint.X - rWH / 2
        Dim MinY As Single = midpoint.Y - rWH / 2
        Dim SrcRect As New Rectangle(MinX, MinY, rWH, rWH) ' kvadrat
        If Not IsNothing(tmpZLImg) Then tmpZLImg.Dispose()
        'tmpZLImg = New Bitmap(TmpMainMap.Width, TmpMainMap.Height)
        tmpZLImg = New Bitmap(SrcRect.Width, SrcRect.Height)
        Using g As Graphics = Graphics.FromImage(tmpZLImg)
            g.TranslateTransform(tmpZLImg.Width / 2, tmpZLImg.Height / 2)

            If IsNumeric(angleDegrees) Then g.RotateTransform(angleDegrees)
            g.TranslateTransform(-tmpZLImg.Width / 2, -tmpZLImg.Height / 2)
            g.DrawImage(TmpMainMap, New RectangleF(0, 0, tmpZLImg.Width, tmpZLImg.Height), SrcRect, GraphicsUnit.Pixel)
        End Using

        ' Vælg mellem fast eller feathered ramme
        ' Først tegn tmpZLImg én gang på LegMapImage:
        If LegMapImage IsNot Nothing Then LegMapImage.Dispose()
        LegMapImage = New Bitmap(outwidth, outheight)

        Using graphics As Graphics = Graphics.FromImage(LegMapImage)
            graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            graphics.Clear(Color.Transparent)

            ' Beregn kilde- og målrektangler
            Dim srcRectF As New RectangleF(tmpZLImg.Width / 2 - ClipWidth / 2, tmpZLImg.Height / 2 - ClipLength / 2, ClipWidth, ClipLength)
            Dim dstRect As New RectangleF(0, 0, outwidth, outheight)

            ' Tegn det skalerede udsnit af tmpZLImg
            graphics.DrawImage(tmpZLImg, dstRect, srcRectF, GraphicsUnit.Pixel)
        End Using

        ' Herefter vælger du mellem de to behandlinger:
        If FrameFeather Then
            ' Feathered ramme
            Dim alphaMask As Bitmap = CreateAlphaMask(LegMapImage.Size, pxroundrad, FrameWidth, False)
            Dim featheredImage As Bitmap = ApplyAlphaMask(LegMapImage, alphaMask)
            LegMapImage.Dispose()
            LegMapImage = featheredImage
        Else
            ' Fast ramme
            Using graphics As Graphics = Graphics.FromImage(LegMapImage)
                graphics.SmoothingMode = SmoothingMode.AntiAlias
                graphics.CompositingMode = CompositingMode.SourceOver
                graphics.CompositingQuality = CompositingQuality.HighQuality

                Dim outerPath As GraphicsPath = FillRoundedRectangle(New SolidBrush(Color.White), New Rectangle(0, 0, outwidth, outheight), pxroundrad)
                Dim innerRect As New Rectangle(FrameWidth, FrameWidth, outwidth - FrameWidth * 2, outheight - FrameWidth * 2)
                Dim innerPath As GraphicsPath = FillRoundedRectangle(New SolidBrush(Color.White), innerRect, pxroundrad)

                Dim framePath As New GraphicsPath()
                framePath.AddPath(outerPath, False)
                framePath.AddPath(innerPath, False)

                Using region As New Region(outerPath)
                    region.Exclude(innerPath)
                    graphics.SetClip(region, CombineMode.Replace)
                    graphics.Clear(Color.Transparent)
                    Using brush As New SolidBrush(FrameColor)
                        graphics.FillPath(brush, outerPath)
                    End Using
                End Using
            End Using
        End If


        Return LegMapImage

    End Function
#Region "Smooth Leg Implementation"
    Function LapImage2Smooth(CurrentTime As Double, pxmarginheight As Integer, outwidth As Integer, outheight As Integer, pxroundrad As Integer, Optional SqFrame As Boolean = False) As Bitmap
        Dim safeTime As Double = ClampTimeCode(CurrentTime)
        Dim lap As Integer = GetLapNumberAtTime(safeTime) - 1
        If lap < 0 Then lap = 0
        If lap > QRRoutePoints.ImgLapVectors.Count - 1 Then lap = QRRoutePoints.ImgLapVectors.Count - 1

        Dim startpointL As PointF = QRRoutePoints.ImgLapVectors(lap).StartPoint
        Dim endPointL As PointF = QRRoutePoints.ImgLapVectors(lap).EndPoint

        LegWidth = outwidth
        LegHeight = outheight
        LegRad = pxroundrad
        LegMargin = pxmarginheight

        Dim deltaX As Single = endPointL.X - startpointL.X
        Dim deltaY As Single = endPointL.Y - startpointL.Y
        Dim rotationAngle As Single = Math.Atan2(deltaY, deltaX)
        Dim angleDegrees As Double = (90 - rotationAngle * 180 / Math.PI) Mod 360 + 180
        Dim distance As Single = CSng((deltaX * deltaX + deltaY * deltaY) ^ 0.5F)
        Dim midpoint As New PointF((startpointL.X + endPointL.X) / 2.0F, (startpointL.Y + endPointL.Y) / 2.0F)

        Dim clipLength As Single = distance + pxmarginheight
        Dim scale As Single = clipLength / outheight
        Dim clipWidth As Single = outwidth * scale

        Dim rWH As Single = CSng((clipWidth ^ 2 + clipLength ^ 2) ^ 0.5)
        Dim srcSize As Integer = Math.Max(1, CInt(Math.Ceiling(rWH)))
        Dim srcRect As New RectangleF(midpoint.X - srcSize / 2.0F, midpoint.Y - srcSize / 2.0F, srcSize, srcSize)
        Dim legBackgroundKey As String = String.Format(CultureInfo.InvariantCulture, "{0}|{1}|{2}|{3}|{4}|{5:0.###}|{6:0.###}|{7:0.###}", lap, outwidth, outheight, pxmarginheight, pxroundrad, midpoint.X, midpoint.Y, angleDegrees)
        Dim cachedLegBackground As Bitmap = GetSmoothLegBackground(legBackgroundKey, angleDegrees, srcRect, srcSize, clipWidth, clipLength, outwidth, outheight)
        Dim overlayMap As Bitmap = GetSmoothOverlayMap(safeTime)
        Dim tmpOverlayImg As Bitmap = Nothing
        tmpOverlayImg = New Bitmap(srcSize, srcSize)
        Using g As Graphics = Graphics.FromImage(tmpOverlayImg)
            g.Clear(Color.Transparent)
            g.TranslateTransform(tmpOverlayImg.Width / 2.0F, tmpOverlayImg.Height / 2.0F)
            If IsNumeric(angleDegrees) Then g.RotateTransform(CSng(angleDegrees))
            g.TranslateTransform(-tmpOverlayImg.Width / 2.0F, -tmpOverlayImg.Height / 2.0F)
            g.DrawImage(overlayMap, New RectangleF(0, 0, tmpOverlayImg.Width, tmpOverlayImg.Height), srcRect, GraphicsUnit.Pixel)
        End Using

        If LegMapImage IsNot Nothing Then LegMapImage.Dispose()
        LegMapImage = New Bitmap(cachedLegBackground)

        Using graphics As Graphics = Graphics.FromImage(LegMapImage)
            graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            Dim dstRect As New RectangleF(0, 0, outwidth, outheight)
            graphics.DrawImage(tmpOverlayImg, dstRect, New RectangleF(tmpOverlayImg.Width / 2.0F - clipWidth / 2.0F, tmpOverlayImg.Height / 2.0F - clipLength / 2.0F, clipWidth, clipLength), GraphicsUnit.Pixel)
        End Using

        If FrameFeather Then
            Dim alphaMask As Bitmap = CreateAlphaMask(LegMapImage.Size, pxroundrad, FrameWidth, False)
            Dim featheredImage As Bitmap = ApplyAlphaMask(LegMapImage, alphaMask)
            LegMapImage.Dispose()
            LegMapImage = featheredImage
        Else
            Using graphics As Graphics = Graphics.FromImage(LegMapImage)
                graphics.SmoothingMode = SmoothingMode.AntiAlias
                graphics.CompositingMode = CompositingMode.SourceOver
                graphics.CompositingQuality = CompositingQuality.HighQuality

                Dim outerPath As GraphicsPath = FillRoundedRectangle(New SolidBrush(Color.White), New Rectangle(0, 0, outwidth, outheight), pxroundrad)
                Dim innerRect As New Rectangle(FrameWidth, FrameWidth, outwidth - FrameWidth * 2, outheight - FrameWidth * 2)
                Dim innerPath As GraphicsPath = FillRoundedRectangle(New SolidBrush(Color.White), innerRect, pxroundrad)

                Using region As New Region(outerPath)
                    region.Exclude(innerPath)
                    graphics.SetClip(region, CombineMode.Replace)
                    graphics.Clear(Color.Transparent)
                    Using brush As New SolidBrush(FrameColor)
                        graphics.FillPath(brush, outerPath)
                    End Using
                End Using
            End Using
        End If
        If tmpOverlayImg IsNot Nothing Then tmpOverlayImg.Dispose()

        Return LegMapImage
    End Function
#End Region

    Public Function GenerateSpeedometer(ByVal speed As Integer, ByVal minSpeed As Integer, ByVal maxSpeed As Integer, ByVal width As Integer, ByVal height As Integer) As Image
        Dim img As New Bitmap(width, height)
        Dim font As New Font("Arial", 10)
        Using g As Graphics = Graphics.FromImage(img)
            g.SmoothingMode = SmoothingMode.AntiAlias

            ' Define the size and position of the arc
            Dim rect As New RectangleF(0.1F * width, 0.1F * height, 0.8F * width, 0.8F * height)
            Dim startAngle As Single = 150
            Dim sweepAngle As Single = 240

            ' Draw the arc and the tick marks
            g.DrawArc(Pens.Black, rect, startAngle, sweepAngle)
            Dim tickCount As Integer = 10
            Dim tickSpacing As Single = sweepAngle / (tickCount - 1)
            For i As Integer = 0 To tickCount - 1
                Dim tickAngle As Single = startAngle + i * tickSpacing
                Dim tickStart As New PointF(rect.X + rect.Width / 2 + (rect.Width / 2 - 5) * CSng(Math.Cos(Math.PI * tickAngle / 180)), rect.Y + rect.Height / 2 + (rect.Height / 2 - 5) * CSng(Math.Sin(Math.PI * tickAngle / 180)))
                Dim tickEnd As New PointF(rect.X + rect.Width / 2 + (rect.Width / 2 - 20) * CSng(Math.Cos(Math.PI * tickAngle / 180)), rect.Y + rect.Height / 2 + (rect.Height / 2 - 20) * CSng(Math.Sin(Math.PI * tickAngle / 180)))
                g.DrawLine(Pens.Black, tickStart, tickEnd)
                Dim tickValue As Single = minSpeed + i * (maxSpeed - minSpeed) / (tickCount - 1)
                Dim tickValueSize As SizeF = g.MeasureString(Math.Round(tickValue).ToString(), font)
                'Dim tickValuePoint As New PointF(rect.X + rect.Width / 2 + (rect.Width / 2 + 5) * CSng(Math.Cos(Math.PI * tickAngle / 180)) - tickValueSize.Width / 2, rect.Y + rect.Height / 2 + (rect.Height / 2 + 5) * CSng(Math.Sin(Math.PI * tickAngle / 180)) - tickValueSize.Height / 2)
                Dim tickValuePoint As New PointF(tickStart.X, tickStart.Y)
                '= tickEnd - tickStart
                Dim v As PointF
                v.X = tickStart.X - tickEnd.X
                v.Y = tickStart.Y - tickEnd.Y
                Dim l As Single = Math.Sqrt(v.X * v.X + v.Y * v.Y)
                v.X = v.X / l
                v.Y = v.Y / l
                Dim l2 = Math.Sqrt((tickValueSize.Width / 2) ^ 2 + (tickValueSize.Height / 2) ^ 2)
                tickValuePoint.X = (tickStart.X + v.X * (20)) - Math.Sign(v.X) * tickValueSize.Width / 2
                tickValuePoint.Y = (tickStart.Y + v.Y * (20)) - Math.Sign(v.Y) * tickValueSize.Height / 2
                'tickValuePoint.X = tickStart.X
                'tickValuePoint.Y = tickStart.Y
                g.DrawString(Math.Round(tickValue).ToString(), font, Brushes.Black, tickValuePoint)
                g.DrawLine(Pens.Red, tickStart, tickValuePoint)
            Next

            ' Draw the needle
            Dim needleAngle As Single = startAngle + (speed - minSpeed) * sweepAngle / (maxSpeed - minSpeed)
            Dim needleStart As New PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2)
            Dim needleEnd As New PointF(rect.X + rect.Width / 2 + (rect.Width / 2 - 30) * CSng(Math.Cos(Math.PI * needleAngle / 180)), rect.Y + rect.Height / 2 + (rect.Height / 2 - 30) * CSng(Math.Sin(Math.PI * needleAngle / 180)))
            g.DrawLine(Pens.Red, needleStart, needleEnd)
            g.FillEllipse(Brushes.Red, needleStart.X - 3, needleStart.Y - 3, 6, 6) 'rect.X + rect.Width / 2 - 10, rect.Y + rect.Height / 2 - 10, 5, 5)
        End Using
        Return img
    End Function

    Public Function AverageRouteVal(RouteP As clsQRRoutePoints, TimeCode As Integer, Span As Integer, Optional ValueField As String = "Pace") As Double
        Dim j, k, count As Integer
        Dim avgval, total As Double
        j = TimeCode - Span / 2
        k = TimeCode + Span / 2
        If j < 0 Then j = 0
        If k > RouteP.RoutePoints.Count - 1 Then k = RouteP.RoutePoints.Count - 1

        count = 0
        avgval = 0
        For i = j To k
            If ValueField = "Pace" Then
                total += RouteP.RoutePoints(i).Pace
            Else
                total += RouteP.RoutePoints(i).Speed
            End If
            count += 1
        Next
        avgval = Math.Round(total / count)
        avgval = 1000 / (RouteP.RoutePoints(k).RouteDistanceFromStart - RouteP.RoutePoints(j).RouteDistanceFromStart)
        avgval = (k - j) * avgval


        'avgval = RouteP.RoutePoints(TimeCode).Pace
        Dim filterstrength As Double = 0.5
        Dim filtered As Double = prevRVal + filterstrength * (avgval - prevRVal)
        'prevRVal = filtered


        prevRVal = filtered
        'If counttime < 10 Then
        'counttime += 1
        'Return prevfiltered
        'Else
        'counttime = 0
        'prevfiltered = filtered
        Return filtered
        ' End If



        'Return avgval 'filtered

    End Function
    Public Function AddTextToPictureBox(ByVal text As String, ByVal marginSize As Integer, ByVal pictureBox As PictureBox) As Bitmap
        ' Create a new Bitmap object with the same size as the picture box
        Dim bmp As New Bitmap(pictureBox.Width, pictureBox.Height)
        Dim mtext As String = "88:88"
        ' Create a Graphics object from the bitmap
        Dim g As Graphics = Graphics.FromImage(bmp)

        ' Clear the bitmap with a transparent background
        g.Clear(Color.Transparent)

        ' Set up the font properties
        Dim fontName As String = "Arial"
        Dim font As Font
        Dim brush As New SolidBrush(Color.IndianRed)

        ' Calculate the maximum font size that will fit the text within the available space
        Dim maxFontSize As Integer = pictureBox.Width - (2 * marginSize)
        'Dim fonta As New Font(fontName, 10, FontStyle.Regular, GraphicsUnit.Pixel)

        ' Calculate the scaling factor to increase the font size
        'Dim scaleFactor As Single = maxFontSize / fonta.Size

        ' Scale the font size using the scaling factor
        'font = New Font(fontName, fonta.Size * scaleFactor, FontStyle.Regular, GraphicsUnit.Pixel)

        font = New Font(fontName, maxFontSize, FontStyle.Regular, GraphicsUnit.Pixel)
        Dim textSize As SizeF = g.MeasureString(mtext, font)
        While textSize.Width > pictureBox.Width - (2 * marginSize)
            maxFontSize -= 1
            'scaleFactor = maxFontSize / fonta.Size
            font = New Font(fontName, maxFontSize, FontStyle.Regular, GraphicsUnit.Pixel)
            textSize = g.MeasureString(mtext, font)
        End While

        ' Calculate the position to draw the text based on the margin and text size
        Dim xPos As Integer = (bmp.Width - textSize.Width) / 2
        Dim yPos As Integer = (bmp.Height - textSize.Height) / 2

        ' Draw the text onto the bitmap
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
        g.DrawString(text, font, brush, xPos, yPos)
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.SystemDefault
        ' Add a margin to the bitmap
        Dim marginRect As New Rectangle(marginSize, marginSize, bmp.Width - (marginSize * 2), bmp.Height - (marginSize * 2))
        Dim marginPath As New GraphicsPath()
        marginPath.AddRectangle(marginRect)
        'g.SetClip(marginPath)
        'g.Clear(Color.Transparent)

        ' Set the bitmap as the picture box's image
        pictureBox.Image = bmp

        ' Return the bitmap in case you need it for further processing
        Return bmp
    End Function



End Class

