Imports System.Diagnostics
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports System.Runtime.InteropServices

Public Class clsMapRenderEngine
    Private ReadOnly _routePoints As clsQRRoutePoints
    Private ReadOnly _mapImage As Bitmap
    Private ReadOnly _legacyRenderer As clsMapImages
    Private _overlayMap As Bitmap
    Private _overlayTime As Double = Double.NaN
    Private _legBackground As Bitmap
    Private _legBackgroundKey As String = ""
    Private _zoomBackground As Bitmap
    Private _zoomBackgroundKey As String = ""
    Public LastRenderElapsed As TimeSpan = TimeSpan.Zero
    Public LastEncodeElapsed As TimeSpan = TimeSpan.Zero
    Public LastTotalElapsed As TimeSpan = TimeSpan.Zero
    Public LastFrameCount As Integer = 0

    Public Sub New(routePoints As clsQRRoutePoints, mapImage As Bitmap, Optional loadSettings As Boolean = True)
        _routePoints = New clsQRRoutePoints(routePoints)
        _mapImage = New Bitmap(mapImage)
        _legacyRenderer = New clsMapImages(_routePoints, _mapImage, loadSettings)
    End Sub

    Public Property FrameStepSeconds As Double = 0.25

    Public Sub ScalePixelSettings(videoWidth As Integer)
        _legacyRenderer.ScalePixelSettings(videoWidth)
    End Sub

    Public Function RenderLegFrame(timeSeconds As Double) As Bitmap
        Dim safeTime As Double = ClampTimeCode(timeSeconds)
        Dim lap As Integer = GetLapNumberAtTime(safeTime) - 1
        If lap < 0 Then lap = 0
        If lap > _routePoints.ImgLapVectors.Count - 1 Then lap = _routePoints.ImgLapVectors.Count - 1

        Dim startPoint As PointF = _routePoints.ImgLapVectors(lap).StartPoint
        Dim endPoint As PointF = _routePoints.ImgLapVectors(lap).EndPoint
        Dim outWidth As Integer = _legacyRenderer.LegWidth
        Dim outHeight As Integer = _legacyRenderer.LegHeight
        Dim marginHeight As Integer = _legacyRenderer.LegMargin
        Dim roundRadius As Integer = _legacyRenderer.LegRad

        Dim deltaX As Single = endPoint.X - startPoint.X
        Dim deltaY As Single = endPoint.Y - startPoint.Y
        Dim rotationAngle As Single = Math.Atan2(deltaY, deltaX)
        Dim angleDegrees As Double = (90 - rotationAngle * 180 / Math.PI) Mod 360 + 180
        Dim distance As Single = CSng(Math.Sqrt(deltaX * deltaX + deltaY * deltaY))
        Dim midPoint As New PointF((startPoint.X + endPoint.X) / 2.0F, (startPoint.Y + endPoint.Y) / 2.0F)

        Dim clipLength As Single = distance + marginHeight
        Dim scale As Single = clipLength / outHeight
        Dim clipWidth As Single = outWidth * scale
        Dim diagonal As Single = CSng(Math.Sqrt(clipWidth * clipWidth + clipLength * clipLength))
        Dim srcSize As Integer = Math.Max(1, CInt(Math.Ceiling(diagonal)))
        Dim srcRect As New RectangleF(midPoint.X - srcSize / 2.0F, midPoint.Y - srcSize / 2.0F, srcSize, srcSize)

        Dim backgroundKey As String = String.Format(CultureInfo.InvariantCulture, "{0}|{1}|{2}|{3}|{4}|{5:0.###}|{6:0.###}|{7:0.###}", lap, outWidth, outHeight, marginHeight, roundRadius, midPoint.X, midPoint.Y, angleDegrees)
        Dim background As Bitmap = GetLegBackground(backgroundKey, angleDegrees, srcRect, srcSize, clipWidth, clipLength, outWidth, outHeight)
        Dim overlay As Bitmap = RenderOverlayLegFrame(safeTime, angleDegrees, srcRect, srcSize, clipWidth, clipLength, outWidth, outHeight)

        Dim result As New Bitmap(background)
        Using g As Graphics = Graphics.FromImage(result)
            g.InterpolationMode = InterpolationMode.HighQualityBicubic
            g.DrawImage(overlay, 0, 0)
        End Using
        overlay.Dispose()

        If Not _legacyRenderer.FrameFeather Then
            ApplyHardFrame(result, roundRadius)
        End If

        Return result
    End Function

    Public Function RenderZoomFrame(timeSeconds As Double) As Bitmap
        Dim safeTime As Double = ClampTimeCode(timeSeconds)
        Dim outWidth As Integer = _legacyRenderer.ZoomWidth
        Dim outHeight As Integer = _legacyRenderer.ZoomHeight
        Dim roundRadius As Integer = _legacyRenderer.ZoomRad
        Dim midPoint As PointF = GetRoutePointAtTime(safeTime)

        Dim zoomFactor As Double = _legacyRenderer.ZoomZoom
        If zoomFactor < 0.001 Then zoomFactor = 1
        Dim clipLength As Single = CSng(Math.Round(outHeight / zoomFactor))
        Dim clipWidth As Single = CSng(Math.Round(outWidth / zoomFactor))
        Dim angleDegrees As Double = GetHeadingAngleAtTime(safeTime) + 180

        Dim diagonal As Single = CSng(Math.Sqrt(clipWidth * clipWidth + clipLength * clipLength))
        Dim srcSize As Integer = Math.Max(1, CInt(Math.Ceiling(diagonal)))
        Dim srcRect As New RectangleF(midPoint.X - srcSize / 2.0F, midPoint.Y - srcSize / 2.0F, srcSize, srcSize)

        Dim backgroundKey As String = String.Format(CultureInfo.InvariantCulture, "{0}|{1}|{2}|{3}|{4:0.###}|{5:0.###}|{6:0.###}", outWidth, outHeight, roundRadius, srcSize, midPoint.X, midPoint.Y, angleDegrees)
        Dim background As Bitmap = GetZoomBackground(backgroundKey, angleDegrees, srcRect, srcSize, clipWidth, clipLength, outWidth, outHeight)
        Dim overlay As Bitmap = RenderOverlayFrame(safeTime, angleDegrees, srcRect, srcSize, clipWidth, clipLength, outWidth, outHeight)

        Dim result As New Bitmap(background)
        Using g As Graphics = Graphics.FromImage(result)
            g.InterpolationMode = InterpolationMode.HighQualityBicubic
            g.DrawImage(overlay, 0, 0)
        End Using
        overlay.Dispose()

        If Not _legacyRenderer.FrameFeather Then
            ApplyHardFrame(result, roundRadius)
        End If

        Return result
    End Function

    Public Sub WriteLegVideo(outputFile As String,
                             Optional startTime As Double = 0,
                             Optional duration As Double = -1,
                             Optional videoWidth As Integer = 1920,
                             Optional frameStepSeconds As Double = -1,
                             Optional outputFps As Double = -1)
        WriteVideoInternal(AddressOf RenderLegFrame, outputFile, startTime, duration, videoWidth, frameStepSeconds, outputFps)
    End Sub

    Public Sub WriteZoomVideo(outputFile As String,
                              Optional startTime As Double = 0,
                              Optional duration As Double = -1,
                              Optional videoWidth As Integer = 1920,
                              Optional frameStepSeconds As Double = -1,
                              Optional outputFps As Double = -1)
        WriteVideoInternal(AddressOf RenderZoomFrame, outputFile, startTime, duration, videoWidth, frameStepSeconds, outputFps)
    End Sub

    Private Sub WriteVideoInternal(renderFrame As Func(Of Double, Bitmap),
                                   outputFile As String,
                                   startTime As Double,
                                   duration As Double,
                                   videoWidth As Integer,
                                   frameStepSeconds As Double,
                                   outputFps As Double)
        Dim effectiveFrameStep As Double = Me.FrameStepSeconds
        If frameStepSeconds > 0 Then effectiveFrameStep = frameStepSeconds
        If effectiveFrameStep <= 0 Then Throw New ArgumentOutOfRangeException(NameOf(frameStepSeconds))

        LastRenderElapsed = TimeSpan.Zero
        LastEncodeElapsed = TimeSpan.Zero
        LastTotalElapsed = TimeSpan.Zero
        LastFrameCount = 0
        Dim totalStopwatch As Stopwatch = Stopwatch.StartNew()
        Dim renderStopwatch As New Stopwatch()
        Dim encodeStopwatch As New Stopwatch()

        ScalePixelSettings(videoWidth)

        Dim fps As Double = If(outputFps > 0, outputFps, 1.0 / effectiveFrameStep)
        If fps <= 0 Then Throw New ArgumentOutOfRangeException(NameOf(outputFps))
        If _routePoints.RoutePoints.Count = 0 Then Return

        Dim maxTime As Double = _routePoints.RoutePoints.Count - 1
        Dim clampedStart As Double = Math.Max(0, startTime)
        Dim endTime As Double = If(duration < 0, maxTime, Math.Min(clampedStart + duration - effectiveFrameStep, maxTime))
        If clampedStart > endTime Then Return

        Using sampleFrame As Bitmap = renderFrame(clampedStart)
            If sampleFrame Is Nothing Then Throw New InvalidOperationException("Render frame callback returned Nothing.")

            Dim baseDir As String = Path.GetDirectoryName(outputFile)
            If String.IsNullOrEmpty(baseDir) Then baseDir = "."
            Dim fpsText As String = fps.ToString("0.###", CultureInfo.InvariantCulture)

            Dim psi As New ProcessStartInfo()
            psi.FileName = "ffmpeg"
            psi.Arguments = $"-y -f rawvideo -pixel_format bgr24 -video_size {sampleFrame.Width}x{sampleFrame.Height} -framerate {fpsText} -i - -pix_fmt yuv420p -c:v libx264 -preset veryfast -crf 18 ""{outputFile}"""
            psi.UseShellExecute = False
            psi.RedirectStandardInput = True
            psi.RedirectStandardError = True
            psi.CreateNoWindow = True

            Using ffmpegProcess As Process = Process.Start(psi)
                encodeStopwatch.Start()
                Dim stderrReader = ffmpegProcess.StandardError
                Threading.Tasks.Task.Run(Sub()
                                             Try
                                                 stderrReader.ReadToEnd()
                                             Catch
                                             End Try
                                         End Sub)

                Using inputStream As Stream = ffmpegProcess.StandardInput.BaseStream
                    renderStopwatch.Start()
                    WriteBitmapFrameToStream(sampleFrame, inputStream)
                    renderStopwatch.Stop()
                    LastFrameCount = 1

                    Dim frameNo As Integer = 1
                    Dim epsilon As Double = effectiveFrameStep / 1000.0
                    Do
                        Dim currentTime As Double = clampedStart + frameNo * effectiveFrameStep
                        If currentTime > endTime + epsilon Then Exit Do

                        renderStopwatch.Start()
                        Using frame As Bitmap = renderFrame(currentTime)
                            WriteBitmapFrameToStream(frame, inputStream)
                        End Using
                        renderStopwatch.Stop()

                        frameNo += 1
                        LastFrameCount = frameNo
                    Loop
                End Using

                ffmpegProcess.WaitForExit()
                encodeStopwatch.Stop()
            End Using
        End Using

        totalStopwatch.Stop()
        LastRenderElapsed = renderStopwatch.Elapsed
        LastEncodeElapsed = encodeStopwatch.Elapsed
        LastTotalElapsed = totalStopwatch.Elapsed
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

    Private Function ClampTimeCode(timeCode As Double) As Double
        If _routePoints Is Nothing OrElse _routePoints.RoutePoints.Count = 0 Then Return 0
        If timeCode < 0 Then Return 0
        Dim maxTimeCode As Double = _routePoints.RoutePoints.Count - 1
        If timeCode > maxTimeCode Then Return maxTimeCode
        Return timeCode
    End Function

    Private Sub ResolveTimeSegment(timeCode As Double, ByRef lowerIdx As Integer, ByRef upperIdx As Integer, ByRef blend As Double)
        Dim clampedTime As Double = ClampTimeCode(timeCode)
        lowerIdx = CInt(Math.Floor(clampedTime))
        upperIdx = CInt(Math.Ceiling(clampedTime))
        If lowerIdx < 0 Then lowerIdx = 0
        If upperIdx >= _routePoints.RoutePoints.Count Then upperIdx = _routePoints.RoutePoints.Count - 1
        If upperIdx < lowerIdx Then upperIdx = lowerIdx
        blend = clampedTime - lowerIdx
        If upperIdx = lowerIdx Then blend = 0
    End Sub

    Private Function InterpolatePoint(startPoint As PointF, endPoint As PointF, blend As Double) As PointF
        Return New PointF(
            CSng(startPoint.X + (endPoint.X - startPoint.X) * blend),
            CSng(startPoint.Y + (endPoint.Y - startPoint.Y) * blend)
        )
    End Function

    Private Function GetInterpolatedRoutePointAtTime(timeCode As Double) As PointF
        Dim lowerIdx, upperIdx As Integer
        Dim blend As Double
        ResolveTimeSegment(timeCode, lowerIdx, upperIdx, blend)
        Dim startPoint As clsQRRoutePoint = _routePoints.RoutePoints(lowerIdx)
        Dim endPoint As clsQRRoutePoint = _routePoints.RoutePoints(upperIdx)
        Dim yOffset As Double = _routePoints.QRLogoYOffset
        Dim p1 As New PointF(CSng(startPoint.ImageX), CSng(startPoint.ImageY + yOffset))
        Dim p2 As New PointF(CSng(endPoint.ImageX), CSng(endPoint.ImageY + yOffset))
        Return InterpolatePoint(p1, p2, blend)
    End Function

    Private Function GetDistanceBetweenPoints(startPoint As PointF, endPoint As PointF) As Double
        Dim dx As Double = endPoint.X - startPoint.X
        Dim dy As Double = endPoint.Y - startPoint.Y
        Return Math.Sqrt(dx * dx + dy * dy)
    End Function

    Private Function GetRoutePointAtTime(timeCode As Double) As PointF
        Dim clampedTime As Double = ClampTimeCode(timeCode)
        Dim windowSeconds As Double = 0.75
        Dim sampleStep As Double = 0.25
        Dim startTime As Double = Math.Max(0, clampedTime - windowSeconds)
        Dim endTime As Double = Math.Min(_routePoints.RoutePoints.Count - 1, clampedTime + windowSeconds)
        Dim weightedX As Double = 0
        Dim weightedY As Double = 0
        Dim totalWeight As Double = 0
        Dim sampleTime As Double = startTime

        Do While sampleTime <= endTime + sampleStep / 2
            Dim currentSampleTime As Double = Math.Min(sampleTime, endTime)
            Dim samplePoint As PointF = GetInterpolatedRoutePointAtTime(currentSampleTime)
            Dim distanceToCenter As Double = Math.Abs(currentSampleTime - clampedTime)
            Dim weight As Double = (windowSeconds + sampleStep) - distanceToCenter
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

    Private Function GetHeadingAngleAtTime(timeCode As Double) As Double
        Dim clampedTime As Double = ClampTimeCode(timeCode)
        Dim currentWindow As Double = 2.0
        Dim maxWindow As Double = 8.0
        Dim startPoint As PointF = GetRoutePointAtTime(clampedTime)
        Dim endPoint As PointF = startPoint

        Do
            startPoint = GetRoutePointAtTime(Math.Max(0, clampedTime - currentWindow))
            endPoint = GetRoutePointAtTime(Math.Min(_routePoints.RoutePoints.Count - 1, clampedTime + currentWindow))
            If GetDistanceBetweenPoints(startPoint, endPoint) >= 6.0 Then Exit Do
            If currentWindow >= maxWindow Then Exit Do
            currentWindow = Math.Min(maxWindow, currentWindow * 1.5)
        Loop

        Dim angle As Double = Math.Atan2(endPoint.Y - startPoint.Y, endPoint.X - startPoint.X)
        Return (90 - angle * 180 / Math.PI) Mod 360
    End Function

    Private Function GetArrowDirectionAtTime(timeCode As Double) As Double
        Dim cameraAngle As Double = GetHeadingAngleAtTime(timeCode)
        Return (180 - cameraAngle + 360) Mod 360
    End Function

    Private Function GetLapNumberAtTime(timeCode As Double) As Integer
        Dim idx As Integer = CInt(Math.Floor(ClampTimeCode(timeCode)))
        Return _routePoints.RoutePoints(idx).LapNumber
    End Function

    Private Function GetTailLinePointsAtTime(timeCode As Double) As List(Of PointF)
        Dim tailPoints As New List(Of PointF)
        Dim clampedTime As Double = ClampTimeCode(timeCode)
        Dim startTime As Double = Math.Max(0, clampedTime - _legacyRenderer.tailLineDurationSeconds)
        Dim sampleStep As Double = 0.25
        Dim sampleTime As Double = startTime

        Do While sampleTime <= clampedTime + sampleStep / 2
            Dim currentSampleTime As Double = Math.Min(sampleTime, clampedTime)
            Dim currentPoint As PointF = GetRoutePointAtTime(currentSampleTime)
            If tailPoints.Count = 0 OrElse GetDistanceBetweenPoints(tailPoints(tailPoints.Count - 1), currentPoint) >= 2.0 Then
                tailPoints.Add(currentPoint)
            Else
                tailPoints(tailPoints.Count - 1) = currentPoint
            End If
            sampleTime += sampleStep
        Loop

        Return tailPoints
    End Function

    Private Function GetOverlayMap(currentTime As Double) As Bitmap
        Dim safeTime As Double = ClampTimeCode(currentTime)
        If _overlayMap Is Nothing OrElse _overlayMap.Width <> _mapImage.Width OrElse _overlayMap.Height <> _mapImage.Height OrElse Double.IsNaN(_overlayTime) OrElse Math.Abs(_overlayTime - safeTime) > 0.0001 Then
            If _overlayMap IsNot Nothing Then _overlayMap.Dispose()
            _overlayMap = New Bitmap(_mapImage.Width, _mapImage.Height, PixelFormat.Format32bppArgb)

            Dim currentPos As PointF = GetRoutePointAtTime(safeTime)
            Dim tailPoints As List(Of PointF) = GetTailLinePointsAtTime(safeTime)
            Using g As Graphics = Graphics.FromImage(_overlayMap)
                g.Clear(Color.Transparent)
                If tailPoints.Count > 1 Then
                    Using tailPen As New Pen(_legacyRenderer.tailLineColor, _legacyRenderer.dotSize * _legacyRenderer.dotTailRatio)
                        g.DrawLines(tailPen, tailPoints.ToArray())
                    End Using
                End If

                If _legacyRenderer._DotType = "Arrow" Then
                    DrawArrowWithBarbs(g, currentPos, CSng(GetArrowDirectionAtTime(safeTime)), _legacyRenderer.dotSize, _legacyRenderer.dotColor)
                Else
                    Using dotBrush As New SolidBrush(_legacyRenderer.dotColor)
                        g.FillEllipse(dotBrush, currentPos.X - _legacyRenderer.dotSize / 2.0F, currentPos.Y - _legacyRenderer.dotSize / 2.0F, _legacyRenderer.dotSize, _legacyRenderer.dotSize)
                    End Using
                End If
            End Using

            _overlayTime = safeTime
        End If

        Return _overlayMap
    End Function

    Private Function GetLegBackground(cacheKey As String, angleDegrees As Double, srcRect As RectangleF, srcSize As Integer, clipWidth As Single, clipLength As Single, outWidth As Integer, outHeight As Integer) As Bitmap
        If _legBackground Is Nothing OrElse _legBackgroundKey <> cacheKey Then
            If _legBackground IsNot Nothing Then _legBackground.Dispose()

            Using tmpBackground As New Bitmap(srcSize, srcSize)
                Using g As Graphics = Graphics.FromImage(tmpBackground)
                    g.TranslateTransform(tmpBackground.Width / 2.0F, tmpBackground.Height / 2.0F)
                    g.RotateTransform(CSng(angleDegrees))
                    g.TranslateTransform(-tmpBackground.Width / 2.0F, -tmpBackground.Height / 2.0F)
                    g.DrawImage(_mapImage, New RectangleF(0, 0, tmpBackground.Width, tmpBackground.Height), srcRect, GraphicsUnit.Pixel)
                End Using

                _legBackground = New Bitmap(outWidth, outHeight)
                Using g As Graphics = Graphics.FromImage(_legBackground)
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic
                    g.Clear(Color.Transparent)
                    Dim srcRectF As New RectangleF(tmpBackground.Width / 2.0F - clipWidth / 2.0F, tmpBackground.Height / 2.0F - clipLength / 2.0F, clipWidth, clipLength)
                    g.DrawImage(tmpBackground, New RectangleF(0, 0, outWidth, outHeight), srcRectF, GraphicsUnit.Pixel)
                End Using
            End Using

            _legBackgroundKey = cacheKey
        End If

        Return _legBackground
    End Function

    Private Function GetZoomBackground(cacheKey As String, angleDegrees As Double, srcRect As RectangleF, srcSize As Integer, clipWidth As Single, clipLength As Single, outWidth As Integer, outHeight As Integer) As Bitmap
        If _zoomBackground Is Nothing OrElse _zoomBackgroundKey <> cacheKey Then
            If _zoomBackground IsNot Nothing Then _zoomBackground.Dispose()

            Using tmpBackground As New Bitmap(srcSize, srcSize)
                Using g As Graphics = Graphics.FromImage(tmpBackground)
                    g.TranslateTransform(tmpBackground.Width / 2.0F, tmpBackground.Height / 2.0F)
                    g.RotateTransform(CSng(angleDegrees))
                    g.TranslateTransform(-tmpBackground.Width / 2.0F, -tmpBackground.Height / 2.0F)
                    g.DrawImage(_mapImage, New RectangleF(0, 0, tmpBackground.Width, tmpBackground.Height), srcRect, GraphicsUnit.Pixel)
                End Using

                _zoomBackground = New Bitmap(outWidth, outHeight)
                Using g As Graphics = Graphics.FromImage(_zoomBackground)
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic
                    g.Clear(Color.Transparent)
                    Dim srcRectF As New RectangleF(tmpBackground.Width / 2.0F - clipWidth / 2.0F, tmpBackground.Height / 2.0F - clipLength / 2.0F, clipWidth, clipLength)
                    g.DrawImage(tmpBackground, New RectangleF(0, 0, outWidth, outHeight), srcRectF, GraphicsUnit.Pixel)
                End Using
            End Using

            _zoomBackgroundKey = cacheKey
        End If

        Return _zoomBackground
    End Function

    Private Function RenderOverlayLegFrame(currentTime As Double, angleDegrees As Double, srcRect As RectangleF, srcSize As Integer, clipWidth As Single, clipLength As Single, outWidth As Integer, outHeight As Integer) As Bitmap
        Return RenderOverlayFrame(currentTime, angleDegrees, srcRect, srcSize, clipWidth, clipLength, outWidth, outHeight)
    End Function

    Private Function RenderOverlayFrame(currentTime As Double, angleDegrees As Double, srcRect As RectangleF, srcSize As Integer, clipWidth As Single, clipLength As Single, outWidth As Integer, outHeight As Integer) As Bitmap
        Dim overlayMap As Bitmap = GetOverlayMap(currentTime)
        Using tmpOverlay As New Bitmap(srcSize, srcSize)
            Using g As Graphics = Graphics.FromImage(tmpOverlay)
                g.Clear(Color.Transparent)
                g.TranslateTransform(tmpOverlay.Width / 2.0F, tmpOverlay.Height / 2.0F)
                g.RotateTransform(CSng(angleDegrees))
                g.TranslateTransform(-tmpOverlay.Width / 2.0F, -tmpOverlay.Height / 2.0F)
                g.DrawImage(overlayMap, New RectangleF(0, 0, tmpOverlay.Width, tmpOverlay.Height), srcRect, GraphicsUnit.Pixel)
            End Using

            Dim result As New Bitmap(outWidth, outHeight, PixelFormat.Format32bppArgb)
            Using g As Graphics = Graphics.FromImage(result)
                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                g.Clear(Color.Transparent)
                Dim srcRectF As New RectangleF(tmpOverlay.Width / 2.0F - clipWidth / 2.0F, tmpOverlay.Height / 2.0F - clipLength / 2.0F, clipWidth, clipLength)
                g.DrawImage(tmpOverlay, New RectangleF(0, 0, outWidth, outHeight), srcRectF, GraphicsUnit.Pixel)
            End Using
            Return result
        End Using
    End Function

    Private Sub DrawArrowWithBarbs(g As Graphics, currentPos As PointF, directionInput As Single, dotSize As Integer, dotColor As Color)
        Dim direction As Single = directionInput - 90
        Dim arrowLength As Integer = dotSize * 2
        Dim arrowWidth As Integer = dotSize * _legacyRenderer.ArrowWidth
        Dim barbOffset As Integer = dotSize * _legacyRenderer.ArrowBarb

        Dim points(3) As PointF
        Dim offPoints(3) As PointF
        points(0) = New PointF(currentPos.X + Math.Cos(direction * Math.PI / 180) * arrowLength, currentPos.Y + Math.Sin(direction * Math.PI / 180) * arrowLength)
        points(1) = New PointF(currentPos.X + Math.Cos((direction + 90) * Math.PI / 180) * arrowWidth, currentPos.Y + Math.Sin((direction + 90) * Math.PI / 180) * arrowWidth)
        points(3) = New PointF(currentPos.X + Math.Cos((direction - 90) * Math.PI / 180) * arrowWidth, currentPos.Y + Math.Sin((direction - 90) * Math.PI / 180) * arrowWidth)

        Dim midBaseX As Single = (points(1).X + points(3).X) / 2
        Dim midBaseY As Single = (points(1).Y + points(3).Y) / 2
        points(2) = New PointF(midBaseX + Math.Cos(direction * Math.PI / 180) * barbOffset, midBaseY + Math.Sin(direction * Math.PI / 180) * barbOffset)

        For i As Integer = 0 To 3
            offPoints(i) = OffsetPointByLength(points(i), currentPos, points(0))
        Next

        Using arrowBrush As New SolidBrush(dotColor)
            g.FillPolygon(arrowBrush, offPoints)
        End Using
    End Sub

    Private Function OffsetPointByLength(originalPoint As PointF, vectorStart As PointF, vectorEnd As PointF) As PointF
        Dim dx As Single = vectorEnd.X - vectorStart.X
        Dim dy As Single = vectorEnd.Y - vectorStart.Y
        Dim vectorLength As Single = CSng(Math.Sqrt(dx * dx + dy * dy))
        If vectorLength = 0 Then Return originalPoint
        Dim length As Single = -vectorLength / 2
        Dim unitVectorX As Single = dx / vectorLength
        Dim unitVectorY As Single = dy / vectorLength
        Return New PointF(originalPoint.X + unitVectorX * length, originalPoint.Y + unitVectorY * length)
    End Function

    Private Function FillRoundedRectangle(rect As Rectangle, radius As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90)
        path.AddLine(rect.X + radius, rect.Y, rect.X + rect.Width - radius, rect.Y)
        path.AddArc(rect.X + rect.Width - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90)
        path.AddLine(rect.X + rect.Width, rect.Y + radius, rect.X + rect.Width, rect.Y + rect.Height - radius)
        path.AddArc(rect.X + rect.Width - radius * 2, rect.Y + rect.Height - radius * 2, radius * 2, radius * 2, 0, 90)
        path.AddLine(rect.X + rect.Width - radius, rect.Y + rect.Height, rect.X + radius, rect.Y + rect.Height)
        path.AddArc(rect.X, rect.Y + rect.Height - radius * 2, radius * 2, radius * 2, 90, 90)
        path.AddLine(rect.X, rect.Y + rect.Height - radius, rect.X, rect.Y + radius)
        path.CloseFigure()
        Return path
    End Function

    Private Sub ApplyHardFrame(target As Bitmap, radius As Integer)
        Using sourceCopy As New Bitmap(target)
            Using g As Graphics = Graphics.FromImage(target)
                g.SmoothingMode = SmoothingMode.AntiAlias
                g.CompositingMode = CompositingMode.SourceOver
                g.CompositingQuality = CompositingQuality.HighQuality
                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                g.Clear(Color.Transparent)

                Dim outerRect As New Rectangle(0, 0, target.Width, target.Height)
                Dim innerRect As New Rectangle(_legacyRenderer.FrameWidth, _legacyRenderer.FrameWidth, target.Width - _legacyRenderer.FrameWidth * 2, target.Height - _legacyRenderer.FrameWidth * 2)
                Dim outerPath As GraphicsPath = FillRoundedRectangle(outerRect, radius)
                Dim innerPath As GraphicsPath = FillRoundedRectangle(innerRect, radius)

                Using frameBrush As New SolidBrush(_legacyRenderer.FrameColor)
                    g.SetClip(outerPath)
                    g.FillPath(frameBrush, outerPath)
                End Using

                g.SetClip(innerPath)
                g.DrawImage(sourceCopy, 0, 0)
                g.ResetClip()
            End Using
        End Using
    End Sub
End Class
