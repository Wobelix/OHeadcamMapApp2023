Imports System.Diagnostics
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports System.Runtime.InteropServices

Public Class clsMapRenderEngine
    Private Const LegAdaptiveSourceRatioThreshold As Double = 2.0
    Private Const DynamicRunnerLookAheadSeconds As Double = 2.0
    Private Const DynamicSafeLeftRatio As Double = 0.12
    Private Const DynamicSafeRightRatio As Double = 0.88
    Private Const DynamicSafeTopRatio As Double = 0.12
    Private Const DynamicSafeBottomRatio As Double = 0.9
    Private Const DynamicSoftLeftRatio As Double = 0.22
    Private Const DynamicSoftRightRatio As Double = 0.78
    Private Const DynamicSoftTopRatio As Double = 0.18
    Private Const DynamicSoftBottomRatio As Double = 0.78

    Private Class LegRenderInfo
        Public Property Lap As Integer
        Public Property AngleDegrees As Double
        Public Property ClipWidth As Single
        Public Property ClipLength As Single
        Public Property SrcSize As Integer
        Public Property SrcRect As RectangleF
        Public Property MidPoint As PointF
        Public Property OutWidth As Integer
        Public Property OutHeight As Integer
        Public Property MarginHeight As Integer
        Public Property RoundRadius As Integer
        Public Property BackgroundKey As String
        Public Property LegDistance As Single
    End Class

    Private Structure TailSample
        Public Property TimeCode As Double
        Public Property Position As PointF
    End Structure

    Private ReadOnly _routePoints As clsQRRoutePoints
    Private ReadOnly _mapImage As Bitmap
    Private ReadOnly _legacyRenderer As clsMapImages
    Private _overlayMap As Bitmap
    Private _overlayTime As Double = Double.NaN
    Private _legBackground As Bitmap
    Private _legBackgroundKey As String = ""
    Private _zoomBackground As Bitmap
    Private _zoomBackgroundKey As String = ""
    Private _zoomFeatherMask As Bitmap
    Private _zoomFeatherMaskKey As String = ""
    Private _legFeatherMask As Bitmap
    Private _legFeatherMaskKey As String = ""
    Private _legRenderInfoCache As New Dictionary(Of String, LegRenderInfo)
    Private _hasExplicitLegWidth As Boolean = False
    Private _hasExplicitLegHeight As Boolean = False
    Private ReadOnly _routePointCache As New Dictionary(Of Integer, PointF)
    Private ReadOnly _headingAngleCache As New Dictionary(Of Integer, Double)
    Private ReadOnly _paceCache As New Dictionary(Of Integer, Double)
    Public LastBackgroundElapsed As TimeSpan = TimeSpan.Zero
    Public LastOverlayMapElapsed As TimeSpan = TimeSpan.Zero
    Public LastOverlayFrameElapsed As TimeSpan = TimeSpan.Zero
    Public LastDrawElapsed As TimeSpan = TimeSpan.Zero
    Public LastStreamWriteElapsed As TimeSpan = TimeSpan.Zero
    Public LastRenderElapsed As TimeSpan = TimeSpan.Zero
    Public LastEncodeElapsed As TimeSpan = TimeSpan.Zero
    Public LastTotalElapsed As TimeSpan = TimeSpan.Zero
    Public LastFrameCount As Integer = 0
    Public ProgressCallback As Action(Of Integer, Integer, String)

    Public Sub New(routePoints As clsQRRoutePoints, mapImage As Bitmap, Optional loadSettings As Boolean = True)
        _routePoints = New clsQRRoutePoints(routePoints)
        _mapImage = New Bitmap(mapImage)
        _legacyRenderer = New clsMapImages(_routePoints, _mapImage, loadSettings)
        If loadSettings Then LoadDynamicSettings()
    End Sub

    Public Property FrameStepSeconds As Double = 0.25
    Public Property TailAlpha As Integer = 255
    Public Property SpeedColoringEnabled As Boolean = False
    Public Property PaceFastSecondsPerKm As Double = 270.0
    Public Property PaceSlowSecondsPerKm As Double = 780.0
    Public Property TailTicksEnabled As Boolean = False
    Public Property TailTickIntervalSeconds As Double = 2.0
    Public Property TailTickLengthScale As Double = 0.8
    Public Property TailTickWidth As Single = 1.0F
    Public Property TailTickColor As Color = Color.Gray
    Public Property TailTickAlpha As Integer = 140
    Public Property ArrowOutlineScale As Integer = 1
    Public Property DynamicWidth As Integer = 430
    Public Property DynamicHeight As Integer = 430
    Public Property DynamicRadius As Integer = 80
    Public Property DynamicZoomFactor As Double = 1.0
    Public Property DynamicLookBehindSeconds As Double = 20.0
    Public Property DynamicLookAheadSeconds As Double = 45.0
    Public Property DynamicMargin As Integer = 120
    Public Property DynamicTransitionSeconds As Double = 10.0

    Private Sub LoadDynamicSettings()
        DynamicWidth = Math.Max(1, My.Settings.MIDynamicWidth)
        DynamicHeight = Math.Max(1, My.Settings.MIDynamicHeight)
        DynamicRadius = Math.Max(0, My.Settings.MIDynamicRad)
        DynamicZoomFactor = Math.Max(0.001, My.Settings.MIDynamicZoom)
        DynamicLookBehindSeconds = Math.Max(0, My.Settings.MIDynamicLookBehindSeconds)
        DynamicLookAheadSeconds = Math.Max(0, My.Settings.MIDynamicLookAheadSeconds)
        DynamicMargin = Math.Max(0, My.Settings.MIDynamicMargin)
    End Sub

    Public Property TailLineDurationSeconds As Integer
        Get
            Return _legacyRenderer.tailLineDurationSeconds
        End Get
        Set(value As Integer)
            _legacyRenderer.tailLineDurationSeconds = Math.Max(0, value)
        End Set
    End Property

    Public Property TailLineColor As Color
        Get
            Return _legacyRenderer.tailLineColor
        End Get
        Set(value As Color)
            _legacyRenderer.tailLineColor = value
        End Set
    End Property

    Public Property DotColor As Color
        Get
            Return _legacyRenderer.dotColor
        End Get
        Set(value As Color)
            _legacyRenderer.dotColor = value
        End Set
    End Property

    Public Property LegWidth As Integer
        Get
            Return _legacyRenderer.LegWidth
        End Get
        Set(value As Integer)
            _legacyRenderer.LegWidth = Math.Max(1, value)
            _hasExplicitLegWidth = True
        End Set
    End Property

    Public Property LegHeight As Integer
        Get
            Return _legacyRenderer.LegHeight
        End Get
        Set(value As Integer)
            _legacyRenderer.LegHeight = Math.Max(1, value)
            _hasExplicitLegHeight = True
        End Set
    End Property

    Public Property LegMargin As Integer
        Get
            Return _legacyRenderer.LegMargin
        End Get
        Set(value As Integer)
            _legacyRenderer.LegMargin = Math.Max(1, value)
        End Set
    End Property

    Public Property ZoomWidth As Integer
        Get
            Return _legacyRenderer.ZoomWidth
        End Get
        Set(value As Integer)
            _legacyRenderer.ZoomWidth = Math.Max(1, value)
        End Set
    End Property

    Public Property ZoomHeight As Integer
        Get
            Return _legacyRenderer.ZoomHeight
        End Get
        Set(value As Integer)
            _legacyRenderer.ZoomHeight = Math.Max(1, value)
        End Set
    End Property

    Public Property ZoomRadius As Integer
        Get
            Return _legacyRenderer.ZoomRad
        End Get
        Set(value As Integer)
            _legacyRenderer.ZoomRad = Math.Max(0, value)
        End Set
    End Property

    Public Property ZoomFactor As Double
        Get
            Return _legacyRenderer.ZoomZoom
        End Get
        Set(value As Double)
            _legacyRenderer.ZoomZoom = Math.Max(0.001, value)
        End Set
    End Property

    Public Property DotSize As Integer
        Get
            Return _legacyRenderer.dotSize
        End Get
        Set(value As Integer)
            _legacyRenderer.dotSize = Math.Max(1, value)
        End Set
    End Property

    Public Property DotTailRatio As Double
        Get
            Return _legacyRenderer.dotTailRatio
        End Get
        Set(value As Double)
            _legacyRenderer.dotTailRatio = Math.Max(0, value)
        End Set
    End Property

    Public Property ArrowBarb As Double
        Get
            Return _legacyRenderer.ArrowBarb
        End Get
        Set(value As Double)
            _legacyRenderer.ArrowBarb = Math.Max(0, value)
        End Set
    End Property

    Public Property ArrowWidth As Double
        Get
            Return _legacyRenderer.ArrowWidth
        End Get
        Set(value As Double)
            _legacyRenderer.ArrowWidth = Math.Max(0, value)
        End Set
    End Property

    Public Property DotType As String
        Get
            Return _legacyRenderer._DotType
        End Get
        Set(value As String)
            _legacyRenderer._DotType = value
        End Set
    End Property

    Public Property FrameFeather As Boolean
        Get
            Return _legacyRenderer.FrameFeather
        End Get
        Set(value As Boolean)
            _legacyRenderer.FrameFeather = value
        End Set
    End Property

    Public ReadOnly Property UsesAlphaVideoOutput As Boolean
        Get
            Return _legacyRenderer.FrameFeather
        End Get
    End Property

    Public Sub ScalePixelSettings(videoWidth As Integer)
        Dim preservedLegWidth As Integer = _legacyRenderer.LegWidth
        Dim preservedLegHeight As Integer = _legacyRenderer.LegHeight
        _legacyRenderer.ScalePixelSettings(videoWidth)
        If _hasExplicitLegWidth Then _legacyRenderer.LegWidth = Math.Max(1, preservedLegWidth)
        If _hasExplicitLegHeight Then _legacyRenderer.LegHeight = Math.Max(1, preservedLegHeight)
    End Sub

    Public Function RenderLegFrame(timeSeconds As Double) As Bitmap
        Dim safeTime As Double = ClampTimeCode(timeSeconds)
        Dim info As LegRenderInfo = BuildLegRenderInfo(safeTime)
        Dim background As Bitmap = GetLegBackground(info.BackgroundKey, info.AngleDegrees, info.SrcRect, info.SrcSize, info.ClipWidth, info.ClipLength, info.OutWidth, info.OutHeight)
        Dim overlay As Bitmap = RenderOverlayLegFrame(safeTime, info.AngleDegrees, info.SrcRect, info.SrcSize, info.ClipWidth, info.ClipLength, info.OutWidth, info.OutHeight)

        Dim result As New Bitmap(background)
        Using g As Graphics = Graphics.FromImage(result)
            g.InterpolationMode = InterpolationMode.HighQualityBicubic
            g.DrawImage(overlay, 0, 0)
        End Using
        overlay.Dispose()

        If _legacyRenderer.FrameFeather Then
            ApplyFeatherFrame(result, info.RoundRadius, False)
        Else
            ApplyHardFrame(result, info.RoundRadius)
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

        If _legacyRenderer.FrameFeather Then
            ApplyFeatherFrame(result, roundRadius, True)
        Else
            ApplyHardFrame(result, roundRadius)
        End If

        Return result
    End Function

    Public Function RenderZoomFramePerf8(timeSeconds As Double) As Bitmap
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
        Dim result As New Bitmap(background)
        Dim overlayStopwatch As Stopwatch = Stopwatch.StartNew()
        Using g As Graphics = Graphics.FromImage(result)
            g.SmoothingMode = SmoothingMode.AntiAlias
            g.PixelOffsetMode = PixelOffsetMode.HighQuality
            g.CompositingQuality = CompositingQuality.HighQuality
            DrawOverlayDirectProjected(g, safeTime, midPoint, angleDegrees, clipWidth, clipLength, outWidth, outHeight, 1.0F)
        End Using
        overlayStopwatch.Stop()
        LastOverlayFrameElapsed += overlayStopwatch.Elapsed

        If _legacyRenderer.FrameFeather Then
            ApplyFeatherFrame(result, roundRadius, True)
        Else
            ApplyHardFrame(result, roundRadius)
        End If

        Return result
    End Function

    Public Function RenderDynamicFramePerf8(timeSeconds As Double) As Bitmap
        Dim safeTime As Double = ClampTimeCode(timeSeconds)
        Dim info As LegRenderInfo = BuildDynamicRenderInfo(safeTime)
        Dim background As Bitmap = GetZoomBackground(info.BackgroundKey, info.AngleDegrees, info.SrcRect, info.SrcSize, info.ClipWidth, info.ClipLength, info.OutWidth, info.OutHeight)
        Dim result As New Bitmap(background)
        Dim overlayStopwatch As Stopwatch = Stopwatch.StartNew()
        Using g As Graphics = Graphics.FromImage(result)
            g.SmoothingMode = SmoothingMode.AntiAlias
            g.PixelOffsetMode = PixelOffsetMode.HighQuality
            g.CompositingQuality = CompositingQuality.HighQuality
            DrawOverlayDirectProjected(g, safeTime, info.MidPoint, info.AngleDegrees, info.ClipWidth, info.ClipLength, info.OutWidth, info.OutHeight, 1.0F)
        End Using
        overlayStopwatch.Stop()
        LastOverlayFrameElapsed += overlayStopwatch.Elapsed

        If _legacyRenderer.FrameFeather Then
            ApplyFeatherFrame(result, info.RoundRadius, True)
        Else
            ApplyHardFrame(result, info.RoundRadius)
        End If

        Return result
    End Function

    Public Function RenderLegFramePerf(timeSeconds As Double) As Bitmap
        Dim safeTime As Double = ClampTimeCode(timeSeconds)
        Dim info As LegRenderInfo = GetCachedLegRenderInfo(safeTime)
        Dim background As Bitmap = GetLegBackground(info.BackgroundKey, info.AngleDegrees, info.SrcRect, info.SrcSize, info.ClipWidth, info.ClipLength, info.OutWidth, info.OutHeight)
        Dim overlay As Bitmap = RenderOverlayLegFrame(safeTime, info.AngleDegrees, info.SrcRect, info.SrcSize, info.ClipWidth, info.ClipLength, info.OutWidth, info.OutHeight)

        Dim result As New Bitmap(background)
        Using g As Graphics = Graphics.FromImage(result)
            g.InterpolationMode = InterpolationMode.HighQualityBicubic
            g.DrawImage(overlay, 0, 0)
        End Using
        overlay.Dispose()

        If _legacyRenderer.FrameFeather Then
            ApplyFeatherFrame(result, info.RoundRadius, False)
        Else
            ApplyHardFrame(result, info.RoundRadius)
        End If

        Return result
    End Function

    Public Function RenderLegFramePerf3(timeSeconds As Double) As Bitmap
        Dim safeTime As Double = ClampTimeCode(timeSeconds)
        Dim info As LegRenderInfo = GetCachedLegRenderInfo(safeTime)
        Dim workSize As Integer = GetAdaptiveLegWorkSize(info)
        Dim background As Bitmap = GetLegBackgroundAdaptive(info, workSize)
        Dim overlay As Bitmap = RenderOverlayLegFrameAdaptive(safeTime, info, workSize)

        Dim result As New Bitmap(background)
        Using g As Graphics = Graphics.FromImage(result)
            g.InterpolationMode = InterpolationMode.HighQualityBicubic
            g.DrawImage(overlay, 0, 0)
        End Using
        overlay.Dispose()

        If _legacyRenderer.FrameFeather Then
            ApplyFeatherFrame(result, info.RoundRadius, False)
        Else
            ApplyHardFrame(result, info.RoundRadius)
        End If

        Return result
    End Function

    Public Function RenderLegFrameSelective(timeSeconds As Double) As Bitmap
        Dim safeTime As Double = ClampTimeCode(timeSeconds)
        Dim info As LegRenderInfo = GetCachedLegRenderInfo(safeTime)
        Dim sourceRatio As Double = info.SrcSize / Math.Max(1.0, CDbl(info.OutHeight))

        If sourceRatio > LegAdaptiveSourceRatioThreshold Then
            Return RenderLegFramePerf3(safeTime)
        End If

        Return RenderLegFramePerf(safeTime)
    End Function

    Public Function RenderLegFramePerf8(timeSeconds As Double) As Bitmap
        Dim safeTime As Double = ClampTimeCode(timeSeconds)
        Dim info As LegRenderInfo = GetCachedLegRenderInfo(safeTime)
        Dim workSize As Integer = GetAdaptiveLegWorkSizeOutputAware(info)
        Dim background As Bitmap = GetLegBackgroundAdaptive(info, workSize)
        Dim result As New Bitmap(background)
        Dim overlayStopwatch As Stopwatch = Stopwatch.StartNew()
        Using g As Graphics = Graphics.FromImage(result)
            g.SmoothingMode = SmoothingMode.AntiAlias
            g.PixelOffsetMode = PixelOffsetMode.HighQuality
            g.CompositingQuality = CompositingQuality.HighQuality
            DrawLegOverlayDirect(g, safeTime, info)
        End Using
        overlayStopwatch.Stop()
        LastOverlayFrameElapsed += overlayStopwatch.Elapsed

        If _legacyRenderer.FrameFeather Then
            ApplyFeatherFrame(result, info.RoundRadius, False)
        Else
            ApplyHardFrame(result, info.RoundRadius)
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

    Public Sub WriteLegVideoPerf(outputFile As String,
                                 Optional startTime As Double = 0,
                                 Optional duration As Double = -1,
                                 Optional videoWidth As Integer = 1920,
                                 Optional frameStepSeconds As Double = -1,
                                 Optional outputFps As Double = -1)
        WriteVideoInternal(AddressOf RenderLegFramePerf, outputFile, startTime, duration, videoWidth, frameStepSeconds, outputFps)
    End Sub

    Public Sub WriteLegVideoPerf3(outputFile As String,
                                  Optional startTime As Double = 0,
                                  Optional duration As Double = -1,
                                  Optional videoWidth As Integer = 1920,
                                  Optional frameStepSeconds As Double = -1,
                                  Optional outputFps As Double = -1)
        WriteVideoInternal(AddressOf RenderLegFramePerf3, outputFile, startTime, duration, videoWidth, frameStepSeconds, outputFps)
    End Sub

    Public Sub WriteLegVideoPerf8(outputFile As String,
                                  Optional startTime As Double = 0,
                                  Optional duration As Double = -1,
                                  Optional videoWidth As Integer = 1920,
                                  Optional frameStepSeconds As Double = -1,
                                  Optional outputFps As Double = -1)
        WriteVideoInternal(AddressOf RenderLegFramePerf8, outputFile, startTime, duration, videoWidth, frameStepSeconds, outputFps)
    End Sub

    Public Sub WriteLegVideoSelective(outputFile As String,
                                      Optional startTime As Double = 0,
                                      Optional duration As Double = -1,
                                      Optional videoWidth As Integer = 1920,
                                      Optional frameStepSeconds As Double = -1,
                                      Optional outputFps As Double = -1)
        WriteVideoInternal(AddressOf RenderLegFrameSelective, outputFile, startTime, duration, videoWidth, frameStepSeconds, outputFps)
    End Sub

    Public Sub WriteZoomVideo(outputFile As String,
                              Optional startTime As Double = 0,
                              Optional duration As Double = -1,
                              Optional videoWidth As Integer = 1920,
                              Optional frameStepSeconds As Double = -1,
                              Optional outputFps As Double = -1)
        WriteVideoInternal(AddressOf RenderZoomFrame, outputFile, startTime, duration, videoWidth, frameStepSeconds, outputFps)
    End Sub

    Public Sub WriteZoomVideoPerf8(outputFile As String,
                                   Optional startTime As Double = 0,
                                   Optional duration As Double = -1,
                                   Optional videoWidth As Integer = 1920,
                                   Optional frameStepSeconds As Double = -1,
                                   Optional outputFps As Double = -1)
        WriteVideoInternal(AddressOf RenderZoomFramePerf8, outputFile, startTime, duration, videoWidth, frameStepSeconds, outputFps)
    End Sub

    Public Sub WriteDynamicVideoPerf8(outputFile As String,
                                      Optional startTime As Double = 0,
                                      Optional duration As Double = -1,
                                      Optional videoWidth As Integer = 1920,
                                      Optional frameStepSeconds As Double = -1,
                                      Optional outputFps As Double = -1)
        WriteVideoInternal(AddressOf RenderDynamicFramePerf8, outputFile, startTime, duration, videoWidth, frameStepSeconds, outputFps)
    End Sub

    Private Sub WriteVideoInternal(renderFrame As Func(Of Double, Bitmap),
                                   outputFile As String,
                                   startTime As Double,
                                   duration As Double,
                                   videoWidth As Integer,
                                   frameStepSeconds As Double,
                                   outputFps As Double)
        Dim useAlphaOutput As Boolean = UsesAlphaVideoOutput
        Dim effectiveFrameStep As Double = Me.FrameStepSeconds
        If frameStepSeconds > 0 Then effectiveFrameStep = frameStepSeconds
        If effectiveFrameStep <= 0 Then Throw New ArgumentOutOfRangeException(NameOf(frameStepSeconds))

        LastBackgroundElapsed = TimeSpan.Zero
        LastOverlayMapElapsed = TimeSpan.Zero
        LastOverlayFrameElapsed = TimeSpan.Zero
        LastDrawElapsed = TimeSpan.Zero
        LastStreamWriteElapsed = TimeSpan.Zero
        LastRenderElapsed = TimeSpan.Zero
        LastEncodeElapsed = TimeSpan.Zero
        LastTotalElapsed = TimeSpan.Zero
        LastFrameCount = 0
        Dim totalStopwatch As Stopwatch = Stopwatch.StartNew()
        Dim drawStopwatch As New Stopwatch()
        Dim streamWriteStopwatch As New Stopwatch()
        Dim encodeStopwatch As New Stopwatch()

        ScalePixelSettings(videoWidth)

        Dim fps As Double = If(outputFps > 0, outputFps, 1.0 / effectiveFrameStep)
        If fps <= 0 Then Throw New ArgumentOutOfRangeException(NameOf(outputFps))
        If _routePoints.RoutePoints.Count = 0 Then Return

        Dim maxTime As Double = _routePoints.RoutePoints.Count - 1
        Dim clampedStart As Double = Math.Max(0, startTime)
        Dim endTime As Double = If(duration < 0, maxTime, Math.Min(clampedStart + duration - effectiveFrameStep, maxTime))
        If clampedStart > endTime Then Return
        Dim totalFrames As Integer = CInt(Math.Floor((endTime - clampedStart) / effectiveFrameStep + 0.0001)) + 1

        drawStopwatch.Start()
        Using sampleFrame As Bitmap = renderFrame(clampedStart)
            drawStopwatch.Stop()
            If sampleFrame Is Nothing Then Throw New InvalidOperationException("Render frame callback returned Nothing.")
            Dim outputFrameWidth As Integer = sampleFrame.Width
            Dim outputFrameHeight As Integer = sampleFrame.Height
            If Not useAlphaOutput Then
                outputFrameWidth = EnsureEvenDimension(outputFrameWidth)
                outputFrameHeight = EnsureEvenDimension(outputFrameHeight)
            End If

            Dim baseDir As String = Path.GetDirectoryName(outputFile)
            If String.IsNullOrEmpty(baseDir) Then baseDir = "."
            Dim fpsText As String = fps.ToString("0.###", CultureInfo.InvariantCulture)

            Dim psi As New ProcessStartInfo()
            psi.FileName = "ffmpeg"
            If useAlphaOutput Then
                psi.Arguments = $"-y -f rawvideo -pixel_format bgra -video_size {outputFrameWidth}x{outputFrameHeight} -framerate {fpsText} -i - -c:v qtrle -pix_fmt argb ""{outputFile}"""
            Else
                psi.Arguments = $"-y -f rawvideo -pixel_format bgr24 -video_size {outputFrameWidth}x{outputFrameHeight} -framerate {fpsText} -i - -pix_fmt yuv420p -c:v libx264 -preset veryfast -crf 18 ""{outputFile}"""
            End If
            psi.UseShellExecute = False
            psi.RedirectStandardInput = True
            psi.RedirectStandardError = True
            psi.CreateNoWindow = True

            Using ffmpegProcess As Process = Process.Start(psi)
                encodeStopwatch.Start()
                Dim stderrReader = ffmpegProcess.StandardError
                Dim stderrTask = Threading.Tasks.Task.Run(Function()
                                                              Try
                                                                  Return stderrReader.ReadToEnd()
                                                              Catch
                                                                  Return ""
                                                              End Try
                                                          End Function)

                Using inputStream As Stream = ffmpegProcess.StandardInput.BaseStream
                    streamWriteStopwatch.Start()
                    Using preparedSampleFrame As Bitmap = PrepareFrameForVideo(sampleFrame, outputFrameWidth, outputFrameHeight, useAlphaOutput)
                        WriteBitmapFrameToStream(preparedSampleFrame, inputStream, useAlphaOutput)
                    End Using
                    streamWriteStopwatch.Stop()
                    LastFrameCount = 1
                    ReportProgress(1, totalFrames, outputFile)

                    Dim frameNo As Integer = 1
                    Dim epsilon As Double = effectiveFrameStep / 1000.0
                    Do
                        Dim currentTime As Double = clampedStart + frameNo * effectiveFrameStep
                        If currentTime > endTime + epsilon Then Exit Do

                        drawStopwatch.Start()
                        Using frame As Bitmap = renderFrame(currentTime)
                            drawStopwatch.Stop()
                            streamWriteStopwatch.Start()
                            Using preparedFrame As Bitmap = PrepareFrameForVideo(frame, outputFrameWidth, outputFrameHeight, useAlphaOutput)
                                WriteBitmapFrameToStream(preparedFrame, inputStream, useAlphaOutput)
                            End Using
                            streamWriteStopwatch.Stop()
                        End Using

                        frameNo += 1
                        LastFrameCount = frameNo
                        If frameNo = totalFrames OrElse frameNo Mod 4 = 0 Then
                            ReportProgress(frameNo, totalFrames, outputFile)
                        End If
                    Loop
                End Using

                ffmpegProcess.WaitForExit()
                encodeStopwatch.Stop()
                Dim ffmpegErrorOutput As String = ""
                Try
                    ffmpegErrorOutput = stderrTask.Result
                Catch
                End Try
                If ffmpegProcess.ExitCode <> 0 Then
                    Throw New InvalidOperationException("ffmpeg failed while writing " &
                                                        Path.GetFileName(outputFile) &
                                                        " (exit code " &
                                                        ffmpegProcess.ExitCode.ToString(CultureInfo.InvariantCulture) &
                                                        ")." &
                                                        If(String.IsNullOrWhiteSpace(ffmpegErrorOutput), "", Environment.NewLine & ffmpegErrorOutput))
                End If
            End Using
        End Using

        If Not File.Exists(outputFile) Then
            Throw New FileNotFoundException("Expected overlay video was not created.", outputFile)
        End If

        Dim outputInfo As New FileInfo(outputFile)
        If outputInfo.Length <= 0 Then
            Throw New InvalidOperationException("Overlay video was created with zero length: " & outputInfo.FullName)
        End If

        totalStopwatch.Stop()
        LastDrawElapsed = drawStopwatch.Elapsed
        LastStreamWriteElapsed = streamWriteStopwatch.Elapsed
        LastRenderElapsed = LastDrawElapsed + LastStreamWriteElapsed
        LastEncodeElapsed = encodeStopwatch.Elapsed
        LastTotalElapsed = totalStopwatch.Elapsed
    End Sub

    Private Sub ReportProgress(doneFrames As Integer, totalFrames As Integer, outputFile As String)
        If ProgressCallback Is Nothing Then Return
        Dim safeTotal As Integer = Math.Max(1, totalFrames)
        ProgressCallback(Math.Min(doneFrames, safeTotal), safeTotal, Path.GetFileName(outputFile))
    End Sub

    Private Function EnsureEvenDimension(value As Integer) As Integer
        Dim safeValue As Integer = Math.Max(1, value)
        If safeValue Mod 2 <> 0 Then safeValue += 1
        Return safeValue
    End Function

    Private Function PrepareFrameForVideo(sourceImage As Bitmap,
                                          targetWidth As Integer,
                                          targetHeight As Integer,
                                          includeAlpha As Boolean) As Bitmap
        If sourceImage.Width = targetWidth AndAlso sourceImage.Height = targetHeight Then
            Return DirectCast(sourceImage.Clone(), Bitmap)
        End If

        Dim pixelFormat As PixelFormat = If(includeAlpha, PixelFormat.Format32bppArgb, PixelFormat.Format24bppRgb)
        Dim preparedFrame As New Bitmap(targetWidth, targetHeight, pixelFormat)
        Using g As Graphics = Graphics.FromImage(preparedFrame)
            If includeAlpha Then
                g.Clear(Color.Transparent)
            Else
                g.Clear(Color.Magenta)
            End If
            g.DrawImage(sourceImage, 0, 0, sourceImage.Width, sourceImage.Height)
        End Using

        Return preparedFrame
    End Function

    Private Sub WriteBitmapFrameToStream(sourceImage As Bitmap, targetStream As Stream, includeAlpha As Boolean)
        If includeAlpha Then
            Using tmpBmp As New Bitmap(sourceImage.Width, sourceImage.Height, PixelFormat.Format32bppArgb)
                Using g As Graphics = Graphics.FromImage(tmpBmp)
                    g.Clear(Color.Transparent)
                    g.DrawImage(sourceImage, 0, 0)
                End Using

                Dim rect As New Rectangle(0, 0, tmpBmp.Width, tmpBmp.Height)
                Dim bitmapData As BitmapData = tmpBmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb)
                Dim stride As Integer = Math.Abs(bitmapData.Stride)
                Dim rowBytes As Integer = tmpBmp.Width * 4
                Dim buffer(rowBytes * tmpBmp.Height - 1) As Byte

                For row As Integer = 0 To tmpBmp.Height - 1
                    Dim srcPtr As IntPtr = New IntPtr(bitmapData.Scan0.ToInt64() + row * stride)
                    Marshal.Copy(srcPtr, buffer, row * rowBytes, rowBytes)
                Next

                tmpBmp.UnlockBits(bitmapData)
                targetStream.Write(buffer, 0, buffer.Length)
                targetStream.Flush()
            End Using
        Else
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
        End If
    End Sub

    Private Function ClampTimeCode(timeCode As Double) As Double
        If _routePoints Is Nothing OrElse _routePoints.RoutePoints.Count = 0 Then Return 0
        If timeCode < 0 Then Return 0
        Dim maxTimeCode As Double = _routePoints.RoutePoints.Count - 1
        If timeCode > maxTimeCode Then Return maxTimeCode
        Return timeCode
    End Function

    Private Function GetTimeCacheKey(timeCode As Double) As Integer
        Return CInt(Math.Round(ClampTimeCode(timeCode) * 1000.0, MidpointRounding.AwayFromZero))
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

    Private Function ComputeRoutePointAtTime(timeCode As Double) As PointF
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

        If totalWeight <= 0 Then
            Return GetInterpolatedRoutePointAtTime(clampedTime)
        End If
        Return New PointF(CSng(weightedX / totalWeight), CSng(weightedY / totalWeight))
    End Function

    Private Function GetRoutePointAtTime(timeCode As Double) As PointF
        Dim clampedTime As Double = ClampTimeCode(timeCode)
        Dim cacheKey As Integer = GetTimeCacheKey(clampedTime)
        If _routePointCache.ContainsKey(cacheKey) Then
            Return _routePointCache(cacheKey)
        End If
        Dim result As PointF = ComputeRoutePointAtTime(clampedTime)
        _routePointCache(cacheKey) = result
        Return result
    End Function

    Private Function ComputeHeadingAngleAtTime(timeCode As Double) As Double
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

    Private Function GetInterpolatedPaceAtTime(timeCode As Double) As Double
        Dim clampedTime As Double = ClampTimeCode(timeCode)
        Dim cacheKey As Integer = GetTimeCacheKey(clampedTime)
        If _paceCache.ContainsKey(cacheKey) Then
            Return _paceCache(cacheKey)
        End If

        Dim lowerIdx, upperIdx As Integer
        Dim blend As Double
        ResolveTimeSegment(clampedTime, lowerIdx, upperIdx, blend)
        Dim pace1 As Double = _routePoints.RoutePoints(lowerIdx).Pace
        Dim pace2 As Double = _routePoints.RoutePoints(upperIdx).Pace
        Dim result As Double

        If pace1 <= 0 AndAlso pace2 > 0 Then
            result = pace2
        ElseIf pace2 <= 0 AndAlso pace1 > 0 Then
            result = pace1
        ElseIf pace1 > 0 AndAlso pace2 > 0 Then
            result = pace1 + (pace2 - pace1) * blend
        Else
            result = 0
        End If

        _paceCache(cacheKey) = result
        Return result
    End Function

    Private Function GetHeadingAngleAtTime(timeCode As Double) As Double
        Dim clampedTime As Double = ClampTimeCode(timeCode)
        Dim cacheKey As Integer = GetTimeCacheKey(clampedTime)
        If _headingAngleCache.ContainsKey(cacheKey) Then
            Return _headingAngleCache(cacheKey)
        End If
        Dim result As Double = ComputeHeadingAngleAtTime(clampedTime)
        _headingAngleCache(cacheKey) = result
        Return result
    End Function

    Private Function GetArrowDirectionAtTime(timeCode As Double) As Double
        Dim cameraAngle As Double = GetHeadingAngleAtTime(timeCode)
        Return (180 - cameraAngle + 360) Mod 360
    End Function

    Private Sub GetHeadingSamplePoints(timeCode As Double, ByRef startPoint As PointF, ByRef endPoint As PointF)
        Dim clampedTime As Double = ClampTimeCode(timeCode)
        Dim currentWindow As Double = 2.0
        Dim maxWindow As Double = 8.0
        startPoint = GetRoutePointAtTime(clampedTime)
        endPoint = startPoint

        Do
            startPoint = GetRoutePointAtTime(Math.Max(0, clampedTime - currentWindow))
            endPoint = GetRoutePointAtTime(Math.Min(_routePoints.RoutePoints.Count - 1, clampedTime + currentWindow))
            If GetDistanceBetweenPoints(startPoint, endPoint) >= 6.0 Then Exit Do
            If currentWindow >= maxWindow Then Exit Do
            currentWindow = Math.Min(maxWindow, currentWindow * 1.5)
        Loop
    End Sub

    Private Function GetLapNumberAtTime(timeCode As Double) As Integer
        Dim idx As Integer = CInt(Math.Floor(ClampTimeCode(timeCode)))
        Return _routePoints.RoutePoints(idx).LapNumber
    End Function

    Private Function BuildLegRenderInfo(timeCode As Double) As LegRenderInfo
        Dim safeTime As Double = ClampTimeCode(timeCode)
        Dim lap As Integer = GetLapNumberAtTime(safeTime) - 1
        If lap < 0 Then lap = 0
        If lap > _routePoints.ImgLapVectors.Count - 1 Then lap = _routePoints.ImgLapVectors.Count - 1

        Dim startPoint As PointF = _routePoints.ImgLapVectors(lap).StartPoint
        Dim endPoint As PointF = _routePoints.ImgLapVectors(lap).EndPoint
        Dim outWidth As Integer = Math.Max(1, _legacyRenderer.LegWidth)
        Dim outHeight As Integer = Math.Max(1, _legacyRenderer.LegHeight)
        Dim marginHeight As Integer = Math.Max(100, _legacyRenderer.LegMargin)
        Dim roundRadius As Integer = _legacyRenderer.LegRad

        Dim deltaX As Single = endPoint.X - startPoint.X
        Dim deltaY As Single = endPoint.Y - startPoint.Y
        Dim rotationAngle As Single = Math.Atan2(deltaY, deltaX)
        Dim angleDegrees As Double = (90 - rotationAngle * 180 / Math.PI) Mod 360 + 180
        Dim distance As Single = CSng(Math.Sqrt(deltaX * deltaX + deltaY * deltaY))
        Dim midPoint As New PointF((startPoint.X + endPoint.X) / 2.0F, (startPoint.Y + endPoint.Y) / 2.0F)

        ' Keep a fixed map-pixel margin before start-center and after end-center.
        ' The margin is measured in source map pixels, not output pixels.
        Dim clipLength As Single = Math.Max(1.0F, distance + (marginHeight * 2.0F))
        Dim scale As Single = clipLength / Math.Max(1, outHeight)
        Dim clipWidth As Single = Math.Max(1.0F, outWidth * scale)
        Dim diagonal As Single = CSng(Math.Sqrt(clipWidth * clipWidth + clipLength * clipLength))
        Dim srcSize As Integer = Math.Max(1, CInt(Math.Ceiling(diagonal)))
        Dim srcRect As New RectangleF(midPoint.X - srcSize / 2.0F, midPoint.Y - srcSize / 2.0F, srcSize, srcSize)
        Dim backgroundKey As String = String.Format(CultureInfo.InvariantCulture, "{0}|{1}|{2}|{3}|{4}|{5:0.###}|{6:0.###}|{7:0.###}", lap, outWidth, outHeight, marginHeight, roundRadius, midPoint.X, midPoint.Y, angleDegrees)

        Return New LegRenderInfo With {
            .Lap = lap,
            .AngleDegrees = angleDegrees,
            .ClipWidth = clipWidth,
            .ClipLength = clipLength,
            .SrcSize = srcSize,
            .SrcRect = srcRect,
            .MidPoint = midPoint,
            .OutWidth = outWidth,
            .OutHeight = outHeight,
            .MarginHeight = marginHeight,
            .RoundRadius = roundRadius,
            .BackgroundKey = backgroundKey,
            .LegDistance = distance
        }
    End Function

    Private Function BuildDynamicRenderInfo(timeCode As Double) As LegRenderInfo
        Dim safeTime As Double = ClampTimeCode(timeCode)
        Dim outWidth As Integer = Math.Max(1, DynamicWidth)
        Dim outHeight As Integer = Math.Max(1, DynamicHeight)
        Dim currentLeg As Integer = GetLegIndexAtTime(safeTime)
        Dim currentInfo As LegRenderInfo = BuildDynamicLegRenderInfo(currentLeg, outWidth, outHeight)

        If currentLeg > 0 Then
            Dim previousInfo As LegRenderInfo = BuildDynamicLegRenderInfo(currentLeg - 1, outWidth, outHeight)
            Dim transitionSeconds As Double = GetDynamicTransitionSeconds(previousInfo, currentInfo)
            Dim halfTransitionSeconds As Double = transitionSeconds / 2.0
            Dim legStartTime As Double = GetLegEndTimeCode(currentLeg - 1)
            Dim transitionStart As Double = Math.Max(0, legStartTime - halfTransitionSeconds)
            Dim transitionEnd As Double = Math.Min(_routePoints.RoutePoints.Count - 1, legStartTime + halfTransitionSeconds)
            If safeTime >= transitionStart AndAlso safeTime < transitionEnd Then
                Dim blend As Double = SmoothStep((safeTime - transitionStart) / Math.Max(0.001, transitionEnd - transitionStart))
                Return AdjustDynamicRenderInfoForRunner(BlendDynamicLegRenderInfo(previousInfo, currentInfo, blend), safeTime)
            End If
        End If

        Dim nextLeg As Integer = currentLeg + 1
        If nextLeg <= _routePoints.ImgLapVectors.Count - 1 Then
            Dim nextInfo As LegRenderInfo = BuildDynamicLegRenderInfo(nextLeg, outWidth, outHeight)
            Dim transitionSeconds As Double = GetDynamicTransitionSeconds(currentInfo, nextInfo)
            Dim halfTransitionSeconds As Double = transitionSeconds / 2.0
            Dim legEndTime As Double = GetLegEndTimeCode(currentLeg)
            Dim transitionStart As Double = Math.Max(0, legEndTime - halfTransitionSeconds)
            Dim transitionEnd As Double = Math.Min(_routePoints.RoutePoints.Count - 1, legEndTime + halfTransitionSeconds)
            If safeTime >= transitionStart AndAlso safeTime < transitionEnd Then
                Dim blend As Double = SmoothStep((safeTime - transitionStart) / Math.Max(0.001, transitionEnd - transitionStart))
                Return AdjustDynamicRenderInfoForRunner(BlendDynamicLegRenderInfo(currentInfo, nextInfo, blend), safeTime)
            End If
        End If

        Return AdjustDynamicRenderInfoForRunner(currentInfo, safeTime)
    End Function

    Private Function BuildDynamicLegRenderInfo(legIndex As Integer, outWidth As Integer, outHeight As Integer) As LegRenderInfo
        If legIndex < 0 Then legIndex = 0
        If legIndex > _routePoints.ImgLapVectors.Count - 1 Then legIndex = _routePoints.ImgLapVectors.Count - 1

        Dim startPoint As PointF = _routePoints.ImgLapVectors(legIndex).StartPoint
        Dim endPoint As PointF = _routePoints.ImgLapVectors(legIndex).EndPoint
        Dim marginHeight As Integer = Math.Max(0, DynamicMargin)
        Dim roundRadius As Integer = Math.Max(0, DynamicRadius)
        Dim deltaX As Single = endPoint.X - startPoint.X
        Dim deltaY As Single = endPoint.Y - startPoint.Y
        Dim rotationAngle As Single = Math.Atan2(deltaY, deltaX)
        Dim angleDegrees As Double = (90 - rotationAngle * 180 / Math.PI) Mod 360 + 180
        Dim distance As Single = CSng(Math.Sqrt(deltaX * deltaX + deltaY * deltaY))
        Dim midPoint As New PointF((startPoint.X + endPoint.X) / 2.0F, (startPoint.Y + endPoint.Y) / 2.0F)
        Dim desiredClipLength As Double = Math.Max(1.0, distance + (marginHeight * 2.0))
        Dim desiredZoomFactor As Double = outHeight / desiredClipLength
        Dim minZoomFactor As Double = Math.Max(0.01, My.Settings.MIDynamicMinZoom / 100.0R)
        Dim maxZoomFactor As Double = Math.Max(minZoomFactor, My.Settings.MIDynamicMaxZoom / 100.0R)
        Dim zoomFactor As Double = Math.Max(minZoomFactor, Math.Min(maxZoomFactor, desiredZoomFactor))
        Dim clipLength As Single = CSng(Math.Max(1.0, outHeight / zoomFactor))
        Dim clipWidth As Single = CSng(Math.Max(1.0, outWidth / zoomFactor))
        Return CreateDynamicRenderInfo(legIndex, angleDegrees, midPoint, clipWidth, clipLength, outWidth, outHeight, marginHeight, roundRadius, distance)
    End Function

    Private Function AdjustDynamicRenderInfoForRunner(info As LegRenderInfo, timeCode As Double) As LegRenderInfo
        Dim currentRunner As PointF = GetRoutePointAtTime(timeCode)
        Dim lookAheadTime As Double = Math.Min(_routePoints.RoutePoints.Count - 1, ClampTimeCode(timeCode) + DynamicRunnerLookAheadSeconds)
        Dim futureRunner As PointF = GetRoutePointAtTime(lookAheadTime)
        Dim legEndPoint As PointF = _routePoints.ImgLapVectors(info.Lap).EndPoint
        Dim currentProjected As PointF = ProjectLegPoint(info, currentRunner)
        Dim futureProjected As PointF = ProjectLegPoint(info, futureRunner)
        Dim endProjected As PointF = ProjectLegPoint(info, legEndPoint)
        Dim preferredRunnerY As Single = CSng(info.OutHeight * DynamicSoftBottomRatio)
        Dim preferredEndTop As Single = CSng(info.OutHeight * DynamicSoftTopRatio)
        Dim desiredFollowShiftY As Single = preferredRunnerY - currentProjected.Y
        Dim fullLegClipLength As Double = info.LegDistance + (info.MarginHeight * 2.0)
        Dim legIsZoomConstrained As Boolean = info.ClipLength + 0.5 < fullLegClipLength

        If legIsZoomConstrained Then
            desiredFollowShiftY = CSng(Math.Min(desiredFollowShiftY, preferredEndTop - endProjected.Y))
        Else
            desiredFollowShiftY = 0
        End If

        Dim currentShift As PointF = GetDynamicRunnerScreenShift(currentProjected,
                                                                 CSng(info.OutWidth * DynamicSafeLeftRatio),
                                                                 CSng(info.OutWidth * DynamicSafeRightRatio),
                                                                 CSng(info.OutHeight * DynamicSafeTopRatio),
                                                                 CSng(info.OutHeight * DynamicSafeBottomRatio))
        Dim futureShift As PointF = GetDynamicRunnerScreenShift(futureProjected,
                                                                CSng(info.OutWidth * DynamicSoftLeftRatio),
                                                                CSng(info.OutWidth * DynamicSoftRightRatio),
                                                                CSng(info.OutHeight * DynamicSoftTopRatio),
                                                                CSng(info.OutHeight * DynamicSoftBottomRatio))

        Dim combinedShiftX As Double = GetLargestMagnitude(currentShift.X, futureShift.X)
        Dim combinedShiftY As Double = GetLargestMagnitude(desiredFollowShiftY, GetLargestMagnitude(currentShift.Y, futureShift.Y))
        If Math.Abs(combinedShiftX) < 0.01 AndAlso Math.Abs(combinedShiftY) < 0.01 Then
            Return info
        End If

        Dim mapShiftX As Double = combinedShiftX / Math.Max(1.0, info.OutWidth) * info.ClipWidth
        Dim mapShiftY As Double = combinedShiftY / Math.Max(1.0, info.OutHeight) * info.ClipLength
        Dim adjustedMidPoint As PointF = OffsetPointByProjectedMapDelta(info.MidPoint, info.AngleDegrees, mapShiftX, mapShiftY)

        Return CreateDynamicRenderInfo(info.Lap,
                                       info.AngleDegrees,
                                       adjustedMidPoint,
                                       info.ClipWidth,
                                       info.ClipLength,
                                       info.OutWidth,
                                       info.OutHeight,
                                       info.MarginHeight,
                                       info.RoundRadius,
                                       info.LegDistance)
    End Function

    Private Function BlendDynamicLegRenderInfo(fromInfo As LegRenderInfo, toInfo As LegRenderInfo, blend As Double) As LegRenderInfo
        Dim safeBlend As Double = Math.Max(0.0, Math.Min(1.0, blend))
        Dim angleDegrees As Double = LerpAngleDegrees(fromInfo.AngleDegrees, toInfo.AngleDegrees, safeBlend)
        Dim midPoint As New PointF(CSng(Lerp(fromInfo.MidPoint.X, toInfo.MidPoint.X, safeBlend)),
                                   CSng(Lerp(fromInfo.MidPoint.Y, toInfo.MidPoint.Y, safeBlend)))
        Dim clipLength As Single = CSng(Lerp(fromInfo.ClipLength, toInfo.ClipLength, safeBlend))
        Dim clipWidth As Single = CSng(Lerp(fromInfo.ClipWidth, toInfo.ClipWidth, safeBlend))
        Dim legDistance As Single = CSng(Lerp(fromInfo.LegDistance, toInfo.LegDistance, safeBlend))
        Return CreateDynamicRenderInfo(toInfo.Lap, angleDegrees, midPoint, clipWidth, clipLength, fromInfo.OutWidth, fromInfo.OutHeight, fromInfo.MarginHeight, fromInfo.RoundRadius, legDistance)
    End Function

    Private Function CreateDynamicRenderInfo(legIndex As Integer,
                                             angleDegrees As Double,
                                             midPoint As PointF,
                                             clipWidth As Single,
                                             clipLength As Single,
                                             outWidth As Integer,
                                             outHeight As Integer,
                                             marginHeight As Integer,
                                             roundRadius As Integer,
                                             legDistance As Single) As LegRenderInfo
        Dim diagonal As Single = CSng(Math.Sqrt(clipWidth * clipWidth + clipLength * clipLength))
        Dim srcSize As Integer = Math.Max(1, CInt(Math.Ceiling(diagonal)))
        Dim srcRect As New RectangleF(midPoint.X - srcSize / 2.0F, midPoint.Y - srcSize / 2.0F, srcSize, srcSize)
        Dim backgroundKey As String = String.Format(CultureInfo.InvariantCulture, "dynleg|{0}|{1}|{2}|{3}|{4:0.###}|{5:0.###}|{6:0.###}|{7:0.###}|{8:0.###}", legIndex, outWidth, outHeight, roundRadius, clipWidth, clipLength, midPoint.X, midPoint.Y, angleDegrees)

        Return New LegRenderInfo With {
            .Lap = legIndex,
            .AngleDegrees = angleDegrees,
            .ClipWidth = clipWidth,
            .ClipLength = clipLength,
            .SrcSize = srcSize,
            .SrcRect = srcRect,
            .MidPoint = midPoint,
            .OutWidth = outWidth,
            .OutHeight = outHeight,
            .MarginHeight = marginHeight,
            .RoundRadius = roundRadius,
            .BackgroundKey = backgroundKey,
            .LegDistance = legDistance
        }
    End Function

    Private Function GetDynamicTransitionSeconds(fromInfo As LegRenderInfo, toInfo As LegRenderInfo) As Double
        Dim baseSeconds As Double = Math.Max(0.001, DynamicTransitionSeconds)
        Dim angleDelta As Double = Math.Abs(GetShortestAngleDeltaDegrees(fromInfo.AngleDegrees, toInfo.AngleDegrees))
        Dim angleSeconds As Double = angleDelta / 12.0
        Dim zoomRatio As Double = Math.Max(fromInfo.ClipLength, toInfo.ClipLength) / Math.Max(1.0, Math.Min(fromInfo.ClipLength, toInfo.ClipLength))
        Dim zoomSeconds As Double = Math.Log(Math.Max(1.0, zoomRatio), 2.0) * 8.0

        Return Math.Min(24.0, Math.Max(baseSeconds, Math.Max(angleSeconds, zoomSeconds)))
    End Function

    Private Function GetLegIndexAtTime(timeCode As Double) As Integer
        Dim legIndex As Integer = GetLapNumberAtTime(timeCode) - 1
        If legIndex < 0 Then legIndex = 0
        If legIndex > _routePoints.ImgLapVectors.Count - 1 Then legIndex = _routePoints.ImgLapVectors.Count - 1
        Return legIndex
    End Function

    Private Function GetLegEndTimeCode(legIndex As Integer) As Double
        Dim nextLapNumber As Integer = legIndex + 2
        For i As Integer = 0 To _routePoints.RoutePoints.Count - 1
            If _routePoints.RoutePoints(i).LapNumber >= nextLapNumber Then
                Return i
            End If
        Next
        Return _routePoints.RoutePoints.Count - 1
    End Function

    Private Function SmoothStep(value As Double) As Double
        Dim t As Double = Math.Max(0.0, Math.Min(1.0, value))
        Return t * t * (3.0 - 2.0 * t)
    End Function

    Private Function Lerp(startValue As Double, endValue As Double, amount As Double) As Double
        Return startValue + (endValue - startValue) * amount
    End Function

    Private Function LerpAngleDegrees(startAngle As Double, endAngle As Double, amount As Double) As Double
        Dim delta As Double = GetShortestAngleDeltaDegrees(startAngle, endAngle)
        Return (startAngle + delta * amount + 360.0) Mod 360.0
    End Function

    Private Function GetShortestAngleDeltaDegrees(startAngle As Double, endAngle As Double) As Double
        Return ((endAngle - startAngle + 540.0) Mod 360.0) - 180.0
    End Function

    Private Function GetDynamicRunnerScreenShift(projectedPoint As PointF,
                                                 minX As Single,
                                                 maxX As Single,
                                                 minY As Single,
                                                 maxY As Single) As PointF
        Dim shiftX As Single = 0
        Dim shiftY As Single = 0

        If projectedPoint.X < minX Then
            shiftX = minX - projectedPoint.X
        ElseIf projectedPoint.X > maxX Then
            shiftX = maxX - projectedPoint.X
        End If

        If projectedPoint.Y < minY Then
            shiftY = minY - projectedPoint.Y
        ElseIf projectedPoint.Y > maxY Then
            shiftY = maxY - projectedPoint.Y
        End If

        Return New PointF(shiftX, shiftY)
    End Function

    Private Function GetLargestMagnitude(firstValue As Double, secondValue As Double) As Double
        If Math.Abs(secondValue) > Math.Abs(firstValue) Then
            Return secondValue
        End If
        Return firstValue
    End Function

    Private Function OffsetPointByProjectedMapDelta(midPoint As PointF,
                                                    angleDegrees As Double,
                                                    projectedMapDeltaX As Double,
                                                    projectedMapDeltaY As Double) As PointF
        Dim angleRadians As Double = angleDegrees * Math.PI / 180.0
        Dim mapDeltaX As Double = projectedMapDeltaX * Math.Cos(angleRadians) + projectedMapDeltaY * Math.Sin(angleRadians)
        Dim mapDeltaY As Double = -projectedMapDeltaX * Math.Sin(angleRadians) + projectedMapDeltaY * Math.Cos(angleRadians)
        Return New PointF(CSng(midPoint.X - mapDeltaX), CSng(midPoint.Y - mapDeltaY))
    End Function

    Private Function GetCachedLegRenderInfo(timeCode As Double) As LegRenderInfo
        Dim info As LegRenderInfo = BuildLegRenderInfo(timeCode)
        Dim cacheKey As String = String.Format(CultureInfo.InvariantCulture, "{0}|{1}|{2}|{3}|{4}", info.Lap, info.OutWidth, info.OutHeight, info.MarginHeight, info.RoundRadius)

        If Not _legRenderInfoCache.ContainsKey(cacheKey) Then
            _legRenderInfoCache(cacheKey) = info
        End If

        Return _legRenderInfoCache(cacheKey)
    End Function

    Private Function GetAdaptiveLegWorkSize(info As LegRenderInfo) As Integer
        Dim oversample As Double = 1.5
        Dim scaleX As Double = info.OutWidth / Math.Max(1.0, CDbl(info.ClipWidth))
        Dim scaleY As Double = info.OutHeight / Math.Max(1.0, CDbl(info.ClipLength))
        Dim workScale As Double = Math.Min(1.0, Math.Max(scaleX, scaleY) * oversample)
        If workScale < 0.2 Then workScale = 0.2
        Return Math.Max(1, CInt(Math.Ceiling(info.SrcSize * workScale)))
    End Function

    Private Function GetAdaptiveLegWorkSizeOutputAware(info As LegRenderInfo) As Integer
        Dim sourceRatio As Double = info.SrcSize / Math.Max(1.0, CDbl(info.OutHeight))
        Dim targetRatio As Double

        If sourceRatio <= 1.8 Then
            targetRatio = sourceRatio
        ElseIf sourceRatio <= 2.6 Then
            targetRatio = 1.9
        ElseIf sourceRatio <= 3.8 Then
            targetRatio = 1.7
        Else
            targetRatio = 1.5
        End If

        Dim targetSize As Integer = CInt(Math.Ceiling(info.OutHeight * targetRatio))
        Dim minSize As Integer = Math.Max(info.OutHeight, CInt(Math.Ceiling(info.SrcSize * 0.35)))
        Dim workSize As Integer = Math.Max(minSize, Math.Min(info.SrcSize, targetSize))

        Return Math.Max(1, workSize)
    End Function

    Private Function ProjectViewPoint(viewMidPoint As PointF,
                                      angleDegrees As Double,
                                      clipWidth As Single,
                                      clipLength As Single,
                                      outWidth As Integer,
                                      outHeight As Integer,
                                      sourcePoint As PointF) As PointF
        Dim angleRadians As Double = angleDegrees * Math.PI / 180.0
        Dim dx As Double = sourcePoint.X - viewMidPoint.X
        Dim dy As Double = sourcePoint.Y - viewMidPoint.Y

        Dim rotatedX As Double = dx * Math.Cos(angleRadians) - dy * Math.Sin(angleRadians)
        Dim rotatedY As Double = dx * Math.Sin(angleRadians) + dy * Math.Cos(angleRadians)

        Dim normalizedX As Double = (rotatedX + clipWidth / 2.0) / Math.Max(1.0, clipWidth)
        Dim normalizedY As Double = (rotatedY + clipLength / 2.0) / Math.Max(1.0, clipLength)

        Return New PointF(CSng(normalizedX * outWidth), CSng(normalizedY * outHeight))
    End Function

    Private Function ProjectLegPoint(info As LegRenderInfo, sourcePoint As PointF) As PointF
        Return ProjectViewPoint(info.MidPoint, info.AngleDegrees, info.ClipWidth, info.ClipLength, info.OutWidth, info.OutHeight, sourcePoint)
    End Function

    Private Function ComputeTailSamplesAtTime(timeCode As Double) As List(Of TailSample)
        Dim samples As New List(Of TailSample)
        Dim clampedTime As Double = ClampTimeCode(timeCode)
        Dim startTime As Double = Math.Max(0, clampedTime - _legacyRenderer.tailLineDurationSeconds)
        Dim sampleStep As Double = 0.25
        Dim sampleTime As Double = startTime

        Do While sampleTime <= clampedTime + sampleStep / 2
            Dim currentSampleTime As Double = Math.Min(sampleTime, clampedTime)
            Dim currentPoint As PointF = GetRoutePointAtTime(currentSampleTime)
            If samples.Count = 0 OrElse GetDistanceBetweenPoints(samples(samples.Count - 1).Position, currentPoint) >= 1.0 Then
                samples.Add(New TailSample With {.TimeCode = currentSampleTime, .Position = currentPoint})
            End If
            sampleTime += sampleStep
        Loop

        Return samples
    End Function

    Private Function GetViewMapToScreenScale(clipLength As Single, outHeight As Integer) As Double
        Return outHeight / Math.Max(1.0, CDbl(clipLength))
    End Function

    Private Function GetLegMapToScreenScale(info As LegRenderInfo) As Double
        Return GetViewMapToScreenScale(info.ClipLength, info.OutHeight)
    End Function

    Private Function GetViewEffectiveDotSize(clipLength As Single, outHeight As Integer, symbolScale As Single) As Integer
        Return Math.Max(4, CInt(Math.Round(_legacyRenderer.dotSize * GetViewMapToScreenScale(clipLength, outHeight) * symbolScale)))
    End Function

    Private Function GetLegEffectiveDotSize(info As LegRenderInfo, symbolScale As Single) As Integer
        Return GetViewEffectiveDotSize(info.ClipLength, info.OutHeight, symbolScale)
    End Function

    Private Function FilterTailSamplesForProjectedView(viewMidPoint As PointF,
                                                       angleDegrees As Double,
                                                       clipWidth As Single,
                                                       clipLength As Single,
                                                       outWidth As Integer,
                                                       outHeight As Integer,
                                                       rawSamples As List(Of TailSample),
                                                       symbolScale As Single) As List(Of TailSample)
        If rawSamples Is Nothing OrElse rawSamples.Count <= 1 Then Return rawSamples

        Dim filtered As New List(Of TailSample)
        Dim projectedTailWidth As Double = GetViewEffectiveDotSize(clipLength, outHeight, symbolScale) * Math.Max(0.05, _legacyRenderer.dotTailRatio)
        ' Keep only a mild spacing in output pixels, but let it follow the actual
        ' projected tail width. A fixed distance makes long-leg tails look thinner
        ' and more transparent than short-leg tails.
        Dim minProjectedDistance As Double = Math.Max(1.25, Math.Min(3.0, projectedTailWidth * 0.35))

        filtered.Add(rawSamples(0))

        For i As Integer = 1 To rawSamples.Count - 1
            Dim previousProjected As PointF = ProjectViewPoint(viewMidPoint, angleDegrees, clipWidth, clipLength, outWidth, outHeight, filtered(filtered.Count - 1).Position)
            Dim currentProjected As PointF = ProjectViewPoint(viewMidPoint, angleDegrees, clipWidth, clipLength, outWidth, outHeight, rawSamples(i).Position)
            If GetDistanceBetweenPoints(previousProjected, currentProjected) >= minProjectedDistance Then
                filtered.Add(rawSamples(i))
            ElseIf i = rawSamples.Count - 1 Then
                ' Always keep the newest sample so the tail reaches the runner/arrow.
                filtered(filtered.Count - 1) = rawSamples(i)
            End If
        Next

        Return filtered
    End Function

    Private Function FilterTailSamplesForLeg(info As LegRenderInfo, rawSamples As List(Of TailSample), symbolScale As Single) As List(Of TailSample)
        Return FilterTailSamplesForProjectedView(info.MidPoint, info.AngleDegrees, info.ClipWidth, info.ClipLength, info.OutWidth, info.OutHeight, rawSamples, symbolScale)
    End Function

    Private Function ComputeArrowDirectionProjected(info As LegRenderInfo, timeCode As Double) As Double
        Dim startPoint, endPoint As PointF
        GetHeadingSamplePoints(timeCode, startPoint, endPoint)
        Dim projectedStart As PointF = ProjectLegPoint(info, startPoint)
        Dim projectedEnd As PointF = ProjectLegPoint(info, endPoint)
        Dim angle As Double = Math.Atan2(projectedEnd.Y - projectedStart.Y, projectedEnd.X - projectedStart.X)
        Dim cameraAngle As Double = (90 - angle * 180 / Math.PI) Mod 360
        Return (180 - cameraAngle + 360) Mod 360
    End Function

    Private Function ComputeHeadingAngleProjectedRadians(info As LegRenderInfo, timeCode As Double) As Double
        Return ComputeHeadingAngleProjectedRadiansForView(info.MidPoint, info.AngleDegrees, info.ClipWidth, info.ClipLength, info.OutWidth, info.OutHeight, timeCode)
    End Function

    Private Function ComputeHeadingAngleProjectedRadiansForView(viewMidPoint As PointF,
                                                                angleDegrees As Double,
                                                                clipWidth As Single,
                                                                clipLength As Single,
                                                                outWidth As Integer,
                                                                outHeight As Integer,
                                                                timeCode As Double) As Double
        Dim startPoint, endPoint As PointF
        GetHeadingSamplePoints(timeCode, startPoint, endPoint)
        Dim projectedStart As PointF = ProjectViewPoint(viewMidPoint, angleDegrees, clipWidth, clipLength, outWidth, outHeight, startPoint)
        Dim projectedEnd As PointF = ProjectViewPoint(viewMidPoint, angleDegrees, clipWidth, clipLength, outWidth, outHeight, endPoint)
        Return Math.Atan2(projectedEnd.Y - projectedStart.Y, projectedEnd.X - projectedStart.X)
    End Function

    Private Function ComputeArrowDirectionProjectedForView(viewMidPoint As PointF,
                                                           angleDegrees As Double,
                                                           clipWidth As Single,
                                                           clipLength As Single,
                                                           outWidth As Integer,
                                                           outHeight As Integer,
                                                           timeCode As Double) As Double
        Dim startPoint, endPoint As PointF
        GetHeadingSamplePoints(timeCode, startPoint, endPoint)
        Dim projectedStart As PointF = ProjectViewPoint(viewMidPoint, angleDegrees, clipWidth, clipLength, outWidth, outHeight, startPoint)
        Dim projectedEnd As PointF = ProjectViewPoint(viewMidPoint, angleDegrees, clipWidth, clipLength, outWidth, outHeight, endPoint)
        Dim angle As Double = Math.Atan2(projectedEnd.Y - projectedStart.Y, projectedEnd.X - projectedStart.X)
        Dim cameraAngle As Double = (90 - angle * 180 / Math.PI) Mod 360
        Return (180 - cameraAngle + 360) Mod 360
    End Function

    Private Function GetAdaptiveSymbolScale(info As LegRenderInfo) As Single
        ' Keep symbol size fixed for now. The leg framing should be controlled by
        ' source-map margins, not by shrinking symbols on short legs.
        Return 1.0F
    End Function

    Private Sub DrawTailSideMarkers(g As Graphics, projectedPoints() As PointF, tailWidth As Single)
        If projectedPoints Is Nothing OrElse projectedPoints.Length < 3 Then Return

        Dim outlineMultiplier As Single = Math.Max(1.0F, CSng(ArrowOutlineScale))
        Dim markerThickness As Single = Math.Max(1.35F, tailWidth * 0.18F * outlineMultiplier)
        Dim markerLength As Single = Math.Max(2.5F, tailWidth * (0.45F + 0.05F * outlineMultiplier))
        Dim sideOffset As Single = Math.Max(markerThickness, tailWidth * 0.52F)
        Using sidePen As New Pen(Color.Black, markerThickness)
            sidePen.StartCap = LineCap.Round
            sidePen.EndCap = LineCap.Round
            For i As Integer = 1 To projectedPoints.Length - 2 Step 2
                Dim prevPt As PointF = projectedPoints(i - 1)
                Dim nextPt As PointF = projectedPoints(i + 1)
                Dim dx As Single = nextPt.X - prevPt.X
                Dim dy As Single = nextPt.Y - prevPt.Y
                Dim length As Double = Math.Sqrt(dx * dx + dy * dy)
                If length < 0.001 Then Continue For

                Dim tangentX As Single = CSng(dx / length)
                Dim tangentY As Single = CSng(dy / length)
                Dim perpX As Single = -tangentY
                Dim perpY As Single = tangentX
                Dim center As PointF = projectedPoints(i)
                Dim halfLenX As Single = tangentX * markerLength / 2.0F
                Dim halfLenY As Single = tangentY * markerLength / 2.0F

                Dim leftCenter As New PointF(center.X + perpX * sideOffset, center.Y + perpY * sideOffset)
                Dim rightCenter As New PointF(center.X - perpX * sideOffset, center.Y - perpY * sideOffset)

                g.DrawLine(sidePen,
                           leftCenter.X - halfLenX,
                           leftCenter.Y - halfLenY,
                           leftCenter.X + halfLenX,
                           leftCenter.Y + halfLenY)
                g.DrawLine(sidePen,
                           rightCenter.X - halfLenX,
                           rightCenter.Y - halfLenY,
                           rightCenter.X + halfLenX,
                           rightCenter.Y + halfLenY)
            Next
        End Using
    End Sub

    Private Sub DrawLegOverlayDirect(g As Graphics, currentTime As Double, info As LegRenderInfo)
        DrawOverlayDirectProjected(g, currentTime, info.MidPoint, info.AngleDegrees, info.ClipWidth, info.ClipLength, info.OutWidth, info.OutHeight, GetAdaptiveSymbolScale(info))
    End Sub

    Private Sub DrawOverlayDirectProjected(g As Graphics,
                                           currentTime As Double,
                                           viewMidPoint As PointF,
                                           angleDegrees As Double,
                                           clipWidth As Single,
                                           clipLength As Single,
                                           outWidth As Integer,
                                           outHeight As Integer,
                                           symbolScale As Single)
        Dim safeTime As Double = ClampTimeCode(currentTime)
        Dim tailSamples As List(Of TailSample) = FilterTailSamplesForProjectedView(viewMidPoint, angleDegrees, clipWidth, clipLength, outWidth, outHeight, ComputeTailSamplesAtTime(safeTime), symbolScale)
        ' The legacy renderer defines symbol sizes in source map pixels. When we draw
        ' directly into the leg output, we need to convert that source-pixel size to
        ' output pixels using the current leg view scale.
        Dim mapToScreenScale As Double = GetViewMapToScreenScale(clipLength, outHeight)
        Dim effectiveDotSize As Integer = GetViewEffectiveDotSize(clipLength, outHeight, symbolScale)

        If tailSamples.Count > 1 Then
            Dim tailWidth As Single = CSng(effectiveDotSize * _legacyRenderer.dotTailRatio)
            Dim projectedPoints(tailSamples.Count - 1) As PointF
            For i As Integer = 0 To tailSamples.Count - 1
                projectedPoints(i) = ProjectViewPoint(viewMidPoint, angleDegrees, clipWidth, clipLength, outWidth, outHeight, tailSamples(i).Position)
            Next

            If Not SpeedColoringEnabled Then
                Using tailPen As New Pen(Color.FromArgb(Math.Max(0, Math.Min(255, TailAlpha)), _legacyRenderer.tailLineColor), tailWidth)
                    tailPen.StartCap = LineCap.Round
                    tailPen.EndCap = LineCap.Round
                    tailPen.LineJoin = LineJoin.Round
                    g.DrawLines(tailPen, projectedPoints)
                End Using
            Else
                For i As Integer = 1 To tailSamples.Count - 1
                    Dim startPoint As PointF = projectedPoints(i - 1)
                    Dim endPoint As PointF = projectedPoints(i)
                    Using tailPen As New Pen(GetTailColorForPace(GetInterpolatedPaceAtTime(tailSamples(i).TimeCode)), tailWidth)
                        tailPen.StartCap = LineCap.Round
                        tailPen.EndCap = LineCap.Round
                        tailPen.LineJoin = LineJoin.Round
                        g.DrawLine(tailPen, startPoint, endPoint)
                    End Using
                Next
                DrawTailSideMarkers(g, projectedPoints, tailWidth)
            End If

            If TailTicksEnabled AndAlso TailTickIntervalSeconds > 0 Then
                Dim startTime As Double = Math.Max(0, safeTime - _legacyRenderer.tailLineDurationSeconds)
                Dim firstTick As Double = Math.Ceiling(startTime / TailTickIntervalSeconds) * TailTickIntervalSeconds
                Dim tickLength As Single = CSng(Math.Max(2.0, tailWidth * 0.9))
                Dim tickColor As Color = Color.FromArgb(Math.Max(0, Math.Min(255, TailTickAlpha)), TailTickColor)

                Using tickPen As New Pen(tickColor, Math.Max(1.4F, CSng(TailTickWidth * mapToScreenScale * symbolScale * 1.15F)))
                    Dim tickTime As Double = firstTick
                    ' TODO-LAYOUT: add an alternate "floating" tick mode where tick spacing is
                    ' still time-based, but anchored relative to the runner/tail window so the
                    ' ticks move with the tail instead of absolute route time.
                    Do While tickTime <= safeTime + 0.0001
                        Dim center As PointF = ProjectViewPoint(viewMidPoint, angleDegrees, clipWidth, clipLength, outWidth, outHeight, GetRoutePointAtTime(tickTime))
                        Dim perpendicularRadians As Double = ComputeHeadingAngleProjectedRadiansForView(viewMidPoint, angleDegrees, clipWidth, clipLength, outWidth, outHeight, tickTime) + (Math.PI / 2.0)
                        Dim dx As Single = CSng(Math.Cos(perpendicularRadians) * tickLength / 2.0)
                        Dim dy As Single = CSng(Math.Sin(perpendicularRadians) * tickLength / 2.0)
                        g.DrawLine(tickPen, center.X - dx, center.Y - dy, center.X + dx, center.Y + dy)
                        tickTime += TailTickIntervalSeconds
                    Loop
                End Using
            End If
        End If

        Dim currentPos As PointF = ProjectViewPoint(viewMidPoint, angleDegrees, clipWidth, clipLength, outWidth, outHeight, GetRoutePointAtTime(safeTime))
        If _legacyRenderer._DotType = "Arrow" Then
            DrawArrowWithBarbs(g, currentPos, CSng(ComputeArrowDirectionProjectedForView(viewMidPoint, angleDegrees, clipWidth, clipLength, outWidth, outHeight, safeTime)), effectiveDotSize, _legacyRenderer.dotColor)
        Else
            Using dotBrush As New SolidBrush(_legacyRenderer.dotColor)
                g.FillEllipse(dotBrush, currentPos.X - effectiveDotSize / 2.0F, currentPos.Y - effectiveDotSize / 2.0F, effectiveDotSize, effectiveDotSize)
            End Using
        End If
    End Sub

    Private Function ComputeTailLinePointsAtTime(timeCode As Double) As List(Of PointF)
        Dim tailPoints As New List(Of PointF)
        Dim clampedTime As Double = ClampTimeCode(timeCode)
        Dim startTime As Double = Math.Max(0, clampedTime - _legacyRenderer.tailLineDurationSeconds)
        Dim sampleStep As Double = 0.25
        Dim sampleTime As Double = startTime

        Do While sampleTime <= clampedTime + sampleStep / 2
            Dim currentSampleTime As Double = Math.Min(sampleTime, clampedTime)
            Dim currentPoint As PointF = GetRoutePointAtTime(currentSampleTime)
            If tailPoints.Count = 0 OrElse GetDistanceBetweenPoints(tailPoints(tailPoints.Count - 1), currentPoint) >= 1.0 Then
                tailPoints.Add(currentPoint)
            End If
            sampleTime += sampleStep
        Loop

        Return tailPoints
    End Function

    Private Function GetTailColorForPace(paceSecondsPerKm As Double) As Color
        Dim baseAlpha As Integer = Math.Max(0, Math.Min(255, TailAlpha))
        If Not SpeedColoringEnabled OrElse paceSecondsPerKm <= 0 Then
            Return Color.FromArgb(baseAlpha, _legacyRenderer.tailLineColor)
        End If

        Dim fastPace As Double = Math.Max(1.0, PaceFastSecondsPerKm)
        Dim slowPace As Double = Math.Max(fastPace + 0.001, PaceSlowSecondsPerKm)
        Dim normalized As Double = (paceSecondsPerKm - fastPace) / (slowPace - fastPace)
        normalized = Math.Max(0.0, Math.Min(1.0, normalized))

        Dim greenColor As Color = Color.FromArgb(0, 210, 70)
        Dim yellowColor As Color = Color.FromArgb(245, 220, 40)
        Dim orangeColor As Color = Color.FromArgb(245, 145, 40)
        Dim redColor As Color = Color.FromArgb(220, 50, 50)

        Dim result As Color
        If normalized <= (1.0 / 3.0) Then
            result = LerpColor(greenColor, yellowColor, normalized * 3.0)
        ElseIf normalized <= (2.0 / 3.0) Then
            result = LerpColor(yellowColor, orangeColor, (normalized - 1.0 / 3.0) * 3.0)
        Else
            result = LerpColor(orangeColor, redColor, (normalized - 2.0 / 3.0) * 3.0)
        End If

        Return Color.FromArgb(baseAlpha, result)
    End Function

    Private Function LerpColor(startColor As Color, endColor As Color, amount As Double) As Color
        amount = Math.Max(0.0, Math.Min(1.0, amount))
        Dim startR As Integer = startColor.R
        Dim startG As Integer = startColor.G
        Dim startB As Integer = startColor.B
        Dim endR As Integer = endColor.R
        Dim endG As Integer = endColor.G
        Dim endB As Integer = endColor.B
        Dim r As Integer = CInt(Math.Round(startR + (endR - startR) * amount))
        Dim g As Integer = CInt(Math.Round(startG + (endG - startG) * amount))
        Dim b As Integer = CInt(Math.Round(startB + (endB - startB) * amount))
        Return Color.FromArgb(r, g, b)
    End Function

    Private Sub DrawTail(g As Graphics, tailPoints As List(Of PointF), currentTime As Double)
        Dim tailWidth As Single = CSng(_legacyRenderer.dotSize * _legacyRenderer.dotTailRatio)
        If tailPoints Is Nothing OrElse tailPoints.Count <= 1 Then Return

        If Not SpeedColoringEnabled Then
            Using tailPen As New Pen(Color.FromArgb(Math.Max(0, Math.Min(255, TailAlpha)), _legacyRenderer.tailLineColor), tailWidth)
                tailPen.StartCap = LineCap.Round
                tailPen.EndCap = LineCap.Round
                tailPen.LineJoin = LineJoin.Round
                g.DrawLines(tailPen, tailPoints.ToArray())
            End Using
            Return
        End If

        Dim clampedTime As Double = ClampTimeCode(currentTime)
        Dim startTime As Double = Math.Max(0, clampedTime - _legacyRenderer.tailLineDurationSeconds)
        Dim sampleStep As Double = 0.25
        Dim previousPoint As PointF? = Nothing
        Dim sampleTime As Double = startTime

        Do While sampleTime <= clampedTime + sampleStep / 2
            Dim currentSampleTime As Double = Math.Min(sampleTime, clampedTime)
            Dim currentPoint As PointF = GetRoutePointAtTime(currentSampleTime)
            If previousPoint Is Nothing Then
                previousPoint = currentPoint
            ElseIf GetDistanceBetweenPoints(previousPoint.Value, currentPoint) >= 1.0 Then
                Using tailPen As New Pen(GetTailColorForPace(GetInterpolatedPaceAtTime(currentSampleTime)), tailWidth)
                    tailPen.StartCap = LineCap.Round
                    tailPen.EndCap = LineCap.Round
                    tailPen.LineJoin = LineJoin.Round
                    g.DrawLine(tailPen, previousPoint.Value, currentPoint)
                End Using
                previousPoint = currentPoint
            End If
            sampleTime += sampleStep
        Loop
    End Sub

    Private Sub DrawTailTicks(g As Graphics, currentTime As Double)
        If Not TailTicksEnabled Then Return
        If TailTickIntervalSeconds <= 0 Then Return

        Dim clampedTime As Double = ClampTimeCode(currentTime)
        Dim startTime As Double = Math.Max(0, clampedTime - _legacyRenderer.tailLineDurationSeconds)
        Dim firstTick As Double = Math.Ceiling(startTime / TailTickIntervalSeconds) * TailTickIntervalSeconds
        Dim tickLength As Single = CSng(Math.Max(2.0, _legacyRenderer.dotSize * _legacyRenderer.dotTailRatio * 0.9))
        Dim tickColor As Color = Color.FromArgb(Math.Max(0, Math.Min(255, TailTickAlpha)), TailTickColor)

        Using tickPen As New Pen(tickColor, TailTickWidth)
            Dim tickTime As Double = firstTick
            Do While tickTime <= clampedTime + 0.0001
                Dim center As PointF = GetRoutePointAtTime(tickTime)
                Dim startPoint, endPoint As PointF
                GetHeadingSamplePoints(tickTime, startPoint, endPoint)
                Dim perpendicularRadians As Double = Math.Atan2(endPoint.Y - startPoint.Y, endPoint.X - startPoint.X) + (Math.PI / 2.0)
                Dim dx As Single = CSng(Math.Cos(perpendicularRadians) * tickLength / 2.0)
                Dim dy As Single = CSng(Math.Sin(perpendicularRadians) * tickLength / 2.0)
                g.DrawLine(tickPen, center.X - dx, center.Y - dy, center.X + dx, center.Y + dy)
                tickTime += TailTickIntervalSeconds
            Loop
        End Using
    End Sub

    Private Function GetTailLinePointsAtTime(timeCode As Double) As List(Of PointF)
        Dim clampedTime As Double = ClampTimeCode(timeCode)
        Dim cacheKey As Integer = GetTimeCacheKey(clampedTime)
        Return ComputeTailLinePointsAtTime(clampedTime)
    End Function

    Private Function GetOverlayMap(currentTime As Double) As Bitmap
        Dim safeTime As Double = ClampTimeCode(currentTime)
        If _overlayMap Is Nothing OrElse _overlayMap.Width <> _mapImage.Width OrElse _overlayMap.Height <> _mapImage.Height OrElse Double.IsNaN(_overlayTime) OrElse Math.Abs(_overlayTime - safeTime) > 0.0001 Then
            Dim stopwatch As Stopwatch = Stopwatch.StartNew()
            If _overlayMap IsNot Nothing Then _overlayMap.Dispose()
            _overlayMap = New Bitmap(_mapImage.Width, _mapImage.Height, PixelFormat.Format32bppArgb)

            Dim currentPos As PointF = GetRoutePointAtTime(safeTime)
            Dim tailPoints As List(Of PointF) = GetTailLinePointsAtTime(safeTime)
            Using g As Graphics = Graphics.FromImage(_overlayMap)
                g.SmoothingMode = SmoothingMode.AntiAlias
                g.PixelOffsetMode = PixelOffsetMode.HighQuality
                g.CompositingQuality = CompositingQuality.HighQuality
                g.Clear(Color.Transparent)
                If tailPoints.Count > 1 Then
                    DrawTail(g, tailPoints, safeTime)
                    DrawTailTicks(g, safeTime)
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
            stopwatch.Stop()
            LastOverlayMapElapsed += stopwatch.Elapsed
        End If

        Return _overlayMap
    End Function

    Private Function GetLegBackground(cacheKey As String, angleDegrees As Double, srcRect As RectangleF, srcSize As Integer, clipWidth As Single, clipLength As Single, outWidth As Integer, outHeight As Integer) As Bitmap
        srcSize = Math.Max(1, srcSize)
        outWidth = Math.Max(1, outWidth)
        outHeight = Math.Max(1, outHeight)
        clipWidth = Math.Max(1.0F, clipWidth)
        clipLength = Math.Max(1.0F, clipLength)
        If _legBackground Is Nothing OrElse _legBackgroundKey <> cacheKey Then
            Dim stopwatch As Stopwatch = Stopwatch.StartNew()
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
            stopwatch.Stop()
            LastBackgroundElapsed += stopwatch.Elapsed
        End If

        Return _legBackground
    End Function

    Private Function GetLegBackgroundAdaptive(info As LegRenderInfo, workSize As Integer) As Bitmap
        Dim adaptiveKey As String = info.BackgroundKey & "|w" & workSize.ToString(CultureInfo.InvariantCulture)
        Dim workWidth As Single = CSng(workSize)
        Dim scaledClipWidth As Single = Math.Max(1.0F, CSng(workWidth * info.ClipWidth / Math.Max(1.0F, info.SrcSize)))
        Dim scaledClipLength As Single = Math.Max(1.0F, CSng(workWidth * info.ClipLength / Math.Max(1.0F, info.SrcSize)))
        Return GetLegBackground(adaptiveKey, info.AngleDegrees, info.SrcRect, workSize, scaledClipWidth, scaledClipLength, info.OutWidth, info.OutHeight)
    End Function

    Private Function GetZoomBackground(cacheKey As String, angleDegrees As Double, srcRect As RectangleF, srcSize As Integer, clipWidth As Single, clipLength As Single, outWidth As Integer, outHeight As Integer) As Bitmap
        srcSize = Math.Max(1, srcSize)
        outWidth = Math.Max(1, outWidth)
        outHeight = Math.Max(1, outHeight)
        clipWidth = Math.Max(1.0F, clipWidth)
        clipLength = Math.Max(1.0F, clipLength)
        If _zoomBackground Is Nothing OrElse _zoomBackgroundKey <> cacheKey Then
            Dim stopwatch As Stopwatch = Stopwatch.StartNew()
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
            stopwatch.Stop()
            LastBackgroundElapsed += stopwatch.Elapsed
        End If

        Return _zoomBackground
    End Function

    Private Function RenderOverlayLegFrame(currentTime As Double, angleDegrees As Double, srcRect As RectangleF, srcSize As Integer, clipWidth As Single, clipLength As Single, outWidth As Integer, outHeight As Integer) As Bitmap
        Return RenderOverlayFrame(currentTime, angleDegrees, srcRect, srcSize, clipWidth, clipLength, outWidth, outHeight)
    End Function

    Private Function RenderOverlayLegFrameAdaptive(currentTime As Double, info As LegRenderInfo, workSize As Integer) As Bitmap
        Dim scaledClipWidth As Single = Math.Max(1.0F, CSng(workSize * info.ClipWidth / Math.Max(1.0F, info.SrcSize)))
        Dim scaledClipLength As Single = Math.Max(1.0F, CSng(workSize * info.ClipLength / Math.Max(1.0F, info.SrcSize)))
        Return RenderOverlayFrame(currentTime, info.AngleDegrees, info.SrcRect, workSize, scaledClipWidth, scaledClipLength, info.OutWidth, info.OutHeight)
    End Function

    Private Function RenderOverlayFrame(currentTime As Double, angleDegrees As Double, srcRect As RectangleF, srcSize As Integer, clipWidth As Single, clipLength As Single, outWidth As Integer, outHeight As Integer) As Bitmap
        Dim overlayMap As Bitmap = GetOverlayMap(currentTime)
        Dim stopwatch As Stopwatch = Stopwatch.StartNew()
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
            stopwatch.Stop()
            LastOverlayFrameElapsed += stopwatch.Elapsed
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

        Dim outlineMultiplier As Single = Math.Max(1.0F, CSng(ArrowOutlineScale))
        Dim outlineScale As Single = Math.Max(1.5F, dotSize * 0.18F * outlineMultiplier)
        Dim outlinePoints(3) As PointF
        For i As Integer = 0 To 3
            outlinePoints(i) = ScalePointFromCenter(offPoints(i), currentPos, 1.0F + outlineScale / Math.Max(1.0F, CSng(dotSize)))
        Next

        Using outlineBrush As New SolidBrush(Color.FromArgb(210, 45, 45, 45))
            g.FillPolygon(outlineBrush, outlinePoints)
        End Using

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

    Private Function ScalePointFromCenter(point As PointF, center As PointF, scale As Single) As PointF
        Return New PointF(center.X + (point.X - center.X) * scale, center.Y + (point.Y - center.Y) * scale)
    End Function

    Private Function FillRoundedRectangle(rect As Rectangle, radius As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        Dim safeWidth As Integer = Math.Max(1, rect.Width)
        Dim safeHeight As Integer = Math.Max(1, rect.Height)
        Dim safeRadius As Integer = Math.Max(0, Math.Min(radius, Math.Min(safeWidth, safeHeight) \ 2))

        If safeRadius <= 0 Then
            path.AddRectangle(New Rectangle(rect.X, rect.Y, safeWidth, safeHeight))
            path.CloseFigure()
            Return path
        End If

        Dim diameter As Integer = safeRadius * 2
        path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90)
        path.AddLine(rect.X + safeRadius, rect.Y, rect.X + safeWidth - safeRadius, rect.Y)
        path.AddArc(rect.X + safeWidth - diameter, rect.Y, diameter, diameter, 270, 90)
        path.AddLine(rect.X + safeWidth, rect.Y + safeRadius, rect.X + safeWidth, rect.Y + safeHeight - safeRadius)
        path.AddArc(rect.X + safeWidth - diameter, rect.Y + safeHeight - diameter, diameter, diameter, 0, 90)
        path.AddLine(rect.X + safeWidth - safeRadius, rect.Y + safeHeight, rect.X + safeRadius, rect.Y + safeHeight)
        path.AddArc(rect.X, rect.Y + safeHeight - diameter, diameter, diameter, 90, 90)
        path.AddLine(rect.X, rect.Y + safeHeight - safeRadius, rect.X, rect.Y + safeRadius)
        path.CloseFigure()
        Return path
    End Function


    Private Sub ApplyFeatherFrame(target As Bitmap, radius As Integer, isZoom As Boolean)
        Dim mask As Bitmap = GetFeatherMask(target.Size, radius, _legacyRenderer.FrameWidth, isZoom)
        ApplyMaskAlpha(target, mask)
    End Sub

    Private Function GetFeatherMask(size As Size, radius As Integer, featherWidth As Integer, isZoom As Boolean) As Bitmap
        featherWidth = Math.Max(1, featherWidth)

        If Not isZoom Then
            Dim widthRatio As Double = _legacyRenderer.LegWidth / Math.Max(1.0, _legacyRenderer.ZoomWidth)
            featherWidth = CInt(Math.Round(featherWidth * Math.Max(1.0, widthRatio)))
        End If

        Dim maxFeatherWidth As Integer
        If isZoom Then
            maxFeatherWidth = Math.Min(size.Width, size.Height) \ 2 - 1
        Else
            maxFeatherWidth = size.Width \ 2 - 1
        End If

        featherWidth = Math.Min(featherWidth, Math.Max(1, maxFeatherWidth))
        If featherWidth < 1 Then featherWidth = 1

        Dim cacheKey As String = $"{size.Width}|{size.Height}|{radius}|{featherWidth}"
        Dim cachedMask As Bitmap = If(isZoom, _zoomFeatherMask, _legFeatherMask)
        Dim cachedKey As String = If(isZoom, _zoomFeatherMaskKey, _legFeatherMaskKey)

        If cachedMask Is Nothing OrElse cachedKey <> cacheKey Then
            If cachedMask IsNot Nothing Then cachedMask.Dispose()
            cachedMask = CreateFeatherMask(size, radius, featherWidth)
            cachedKey = cacheKey

            If isZoom Then
                _zoomFeatherMask = cachedMask
                _zoomFeatherMaskKey = cachedKey
            Else
                _legFeatherMask = cachedMask
                _legFeatherMaskKey = cachedKey
            End If
        End If

        Return cachedMask
    End Function

    Private Function CreateFeatherMask(size As Size, radius As Integer, featherWidth As Integer) As Bitmap
        Dim mask As New Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb)
        Dim rect As New Rectangle(0, 0, size.Width, size.Height)
        Dim bmpData As BitmapData = mask.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb)

        Try
            Dim stride As Integer = bmpData.Stride
            Dim buffer(Math.Abs(stride) * size.Height - 1) As Byte

            For y As Integer = 0 To size.Height - 1
                Dim rowStart As Integer = y * stride
                For x As Integer = 0 To size.Width - 1
                    Dim pixelCenterX As Double = x + 0.5
                    Dim pixelCenterY As Double = y + 0.5
                    Dim outerDistance As Double = SignedDistanceToRoundedRect(pixelCenterX, pixelCenterY, size.Width, size.Height, radius)
                    Dim alpha As Integer

                    If outerDistance >= 0 Then
                        alpha = 0
                    ElseIf outerDistance <= -featherWidth Then
                        alpha = 255
                    Else
                        Dim blend As Double = Math.Max(0.0, Math.Min(1.0, (-outerDistance) / Math.Max(1.0, featherWidth)))
                        ' Smoothstep gives a softer edge without a visible inner border.
                        blend = blend * blend * (3.0 - 2.0 * blend)
                        alpha = CInt(Math.Round(blend * 255.0))
                    End If

                    Dim pixelIndex As Integer = rowStart + x * 4
                    buffer(pixelIndex) = 255
                    buffer(pixelIndex + 1) = 255
                    buffer(pixelIndex + 2) = 255
                    buffer(pixelIndex + 3) = CByte(Math.Max(0, Math.Min(255, alpha)))
                Next
            Next

            Marshal.Copy(buffer, 0, bmpData.Scan0, buffer.Length)
        Finally
            mask.UnlockBits(bmpData)
        End Try

        Return mask
    End Function

    Private Function SignedDistanceToRoundedRect(x As Double, y As Double, width As Integer, height As Integer, radius As Integer) As Double
        Dim clampedRadius As Double = Math.Max(0, Math.Min(radius, Math.Min(width, height) / 2.0))
        Dim halfWidth As Double = width / 2.0
        Dim halfHeight As Double = height / 2.0
        Dim qx As Double = Math.Abs(x - halfWidth) - (halfWidth - clampedRadius)
        Dim qy As Double = Math.Abs(y - halfHeight) - (halfHeight - clampedRadius)
        Dim dx As Double = Math.Max(qx, 0)
        Dim dy As Double = Math.Max(qy, 0)
        Return Math.Sqrt(dx * dx + dy * dy) + Math.Min(Math.Max(qx, qy), 0) - clampedRadius
    End Function

    Private Sub ApplyMaskAlpha(target As Bitmap, mask As Bitmap)
        Dim rect As New Rectangle(0, 0, target.Width, target.Height)
        Dim targetData As BitmapData = target.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb)
        Dim maskData As BitmapData = mask.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb)

        Try
            Dim targetStride As Integer = targetData.Stride
            Dim maskStride As Integer = maskData.Stride
            Dim bytesPerPixel As Integer = 4
            Dim targetBuffer(Math.Abs(targetStride) * target.Height - 1) As Byte
            Dim maskBuffer(Math.Abs(maskStride) * mask.Height - 1) As Byte

            Marshal.Copy(targetData.Scan0, targetBuffer, 0, targetBuffer.Length)
            Marshal.Copy(maskData.Scan0, maskBuffer, 0, maskBuffer.Length)

            For y As Integer = 0 To target.Height - 1
                Dim targetRow As Integer = y * targetStride
                Dim maskRow As Integer = y * maskStride
                For x As Integer = 0 To target.Width - 1
                    Dim targetIdx As Integer = targetRow + x * bytesPerPixel
                    Dim maskIdx As Integer = maskRow + x * bytesPerPixel
                    targetBuffer(targetIdx + 3) = maskBuffer(maskIdx + 3)
                Next
            Next

            Marshal.Copy(targetBuffer, 0, targetData.Scan0, targetBuffer.Length)
        Finally
            target.UnlockBits(targetData)
            mask.UnlockBits(maskData)
        End Try
    End Sub

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
