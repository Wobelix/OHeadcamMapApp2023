Imports System.Diagnostics
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports System.Runtime.InteropServices

Public Class clsMapRenderEngine
    Private Const LegAdaptiveSourceRatioThreshold As Double = 2.0

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
    End Class

    Private Class FrameState
        Public Property RoutePoint As PointF
        Public Property HeadingAngle As Double
        Public Property TailPoints As List(Of PointF)
    End Class

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
    Private ReadOnly _routePointCache As New Dictionary(Of Integer, PointF)
    Private ReadOnly _headingAngleCache As New Dictionary(Of Integer, Double)
    Private ReadOnly _frameStateCache As New Dictionary(Of Integer, FrameState)
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
    End Sub

    Public Property FrameStepSeconds As Double = 0.25

    Public ReadOnly Property UsesAlphaVideoOutput As Boolean
        Get
            Return _legacyRenderer.FrameFeather
        End Get
    End Property

    Public Sub ScalePixelSettings(videoWidth As Integer)
        _legacyRenderer.ScalePixelSettings(videoWidth)
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

    Public Function RenderLegFramePerf2(timeSeconds As Double) As Bitmap
        Dim safeTime As Double = ClampTimeCode(timeSeconds)
        Dim info As LegRenderInfo = GetCachedLegRenderInfo(safeTime)
        Dim background As Bitmap = GetLegBackground(info.BackgroundKey, info.AngleDegrees, info.SrcRect, info.SrcSize, info.ClipWidth, info.ClipLength, info.OutWidth, info.OutHeight)
        Dim overlay As Bitmap = RenderOverlayLegFrameDirect(safeTime, info)

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

    Public Function RenderLegFramePerf4(timeSeconds As Double) As Bitmap
        Return RenderLegFramePerf(timeSeconds)
    End Function

    Public Function RenderLegFramePerf5(timeSeconds As Double) As Bitmap
        Dim safeTime As Double = ClampTimeCode(timeSeconds)
        Dim info As LegRenderInfo = GetCachedLegRenderInfo(safeTime)
        Dim background As Bitmap = GetLegBackground(info.BackgroundKey, info.AngleDegrees, info.SrcRect, info.SrcSize, info.ClipWidth, info.ClipLength, info.OutWidth, info.OutHeight)
        Dim overlay As Bitmap = RenderOverlayFrameLocal(safeTime, info.AngleDegrees, info.SrcRect, info.SrcSize, info.ClipWidth, info.ClipLength, info.OutWidth, info.OutHeight)

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

    Public Function RenderLegFramePerf6(timeSeconds As Double) As Bitmap
        Dim safeTime As Double = ClampTimeCode(timeSeconds)
        Dim info As LegRenderInfo = GetCachedLegRenderInfo(safeTime)
        Dim background As Bitmap = GetLegBackgroundFast(info.BackgroundKey & "|fastbg", info.AngleDegrees, info.SrcRect, info.SrcSize, info.ClipWidth, info.ClipLength, info.OutWidth, info.OutHeight)
        Dim overlay As Bitmap = RenderOverlayFrameLocal(safeTime, info.AngleDegrees, info.SrcRect, info.SrcSize, info.ClipWidth, info.ClipLength, info.OutWidth, info.OutHeight, True)

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

    Public Function RenderLegFramePerf7(timeSeconds As Double) As Bitmap
        Dim safeTime As Double = ClampTimeCode(timeSeconds)
        Dim info As LegRenderInfo = GetCachedLegRenderInfo(safeTime)
        Dim workSize As Integer = GetAdaptiveLegWorkSize(info)
        Dim background As Bitmap = GetLegBackgroundAdaptive(info, workSize)
        Dim overlay As Bitmap = RenderOverlayFrameLocalAdaptive(safeTime, info, workSize, True)

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

    Public Function RenderLegFramePerf8(timeSeconds As Double) As Bitmap
        Dim safeTime As Double = ClampTimeCode(timeSeconds)
        Dim info As LegRenderInfo = GetCachedLegRenderInfo(safeTime)
        Dim workSize As Integer = GetAdaptiveLegWorkSizeOutputAware(info)
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

#Region "Experimental Scene Renderer"
    Public Function RenderLegFrameScene(timeSeconds As Double) As Bitmap
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

        Dim backgroundKey As String = String.Format(CultureInfo.InvariantCulture, "scene|{0}|{1}|{2}|{3}|{4}|{5:0.###}|{6:0.###}|{7:0.###}", lap, outWidth, outHeight, marginHeight, roundRadius, midPoint.X, midPoint.Y, angleDegrees)
        Dim background As Bitmap = GetLegBackground(backgroundKey, angleDegrees, srcRect, srcSize, clipWidth, clipLength, outWidth, outHeight)

        Return RenderSceneFrame(
            safeTime,
            outWidth,
            outHeight,
            roundRadius,
            midPoint,
            angleDegrees,
            clipWidth,
            clipLength,
            False,
            lap,
            background)
    End Function

    Public Function RenderLegBackgroundScene(timeSeconds As Double) As Bitmap
        Dim safeTime As Double = ClampTimeCode(timeSeconds)
        Dim lap As Integer = GetLapNumberAtTime(safeTime) - 1
        If lap < 0 Then lap = 0
        If lap > _routePoints.ImgLapVectors.Count - 1 Then lap = _routePoints.ImgLapVectors.Count - 1

        Dim startPoint As PointF = _routePoints.ImgLapVectors(lap).StartPoint
        Dim endPoint As PointF = _routePoints.ImgLapVectors(lap).EndPoint
        Dim outWidth As Integer = Math.Max(1, _legacyRenderer.LegWidth)
        Dim outHeight As Integer = Math.Max(1, _legacyRenderer.LegHeight)
        Dim marginHeight As Integer = _legacyRenderer.LegMargin
        Dim roundRadius As Integer = _legacyRenderer.LegRad

        Dim deltaX As Single = endPoint.X - startPoint.X
        Dim deltaY As Single = endPoint.Y - startPoint.Y
        Dim rotationAngle As Single = Math.Atan2(deltaY, deltaX)
        Dim angleDegrees As Double = (90 - rotationAngle * 180 / Math.PI) Mod 360 + 180
        Dim distance As Single = CSng(Math.Sqrt(deltaX * deltaX + deltaY * deltaY))
        Dim midPoint As New PointF((startPoint.X + endPoint.X) / 2.0F, (startPoint.Y + endPoint.Y) / 2.0F)

        Dim clipLength As Single = Math.Max(1.0F, distance + marginHeight)
        Dim scale As Single = clipLength / outHeight
        Dim clipWidth As Single = Math.Max(1.0F, outWidth * scale)
        Dim diagonal As Single = CSng(Math.Sqrt(clipWidth * clipWidth + clipLength * clipLength))
        Dim srcSize As Integer = Math.Max(1, CInt(Math.Ceiling(diagonal)))
        Dim srcRect As New RectangleF(midPoint.X - srcSize / 2.0F, midPoint.Y - srcSize / 2.0F, srcSize, srcSize)
        Dim backgroundKey As String = String.Format(CultureInfo.InvariantCulture, "scene|{0}|{1}|{2}|{3}|{4}|{5:0.###}|{6:0.###}|{7:0.###}", lap, outWidth, outHeight, marginHeight, roundRadius, midPoint.X, midPoint.Y, angleDegrees)

        Return New Bitmap(GetLegBackground(backgroundKey, angleDegrees, srcRect, srcSize, clipWidth, clipLength, outWidth, outHeight))
    End Function

    Public Function RenderZoomFrameScene(timeSeconds As Double) As Bitmap
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

        Dim backgroundKey As String = String.Format(CultureInfo.InvariantCulture, "scene|{0}|{1}|{2}|{3}|{4:0.###}|{5:0.###}|{6:0.###}", outWidth, outHeight, roundRadius, srcSize, midPoint.X, midPoint.Y, angleDegrees)
        Dim background As Bitmap = GetZoomBackground(backgroundKey, angleDegrees, srcRect, srcSize, clipWidth, clipLength, outWidth, outHeight)

        Return RenderSceneFrame(
            safeTime,
            outWidth,
            outHeight,
            roundRadius,
            midPoint,
            angleDegrees,
            clipWidth,
            clipLength,
            True,
            -1,
            background)
    End Function

    Public Function RenderZoomBackgroundScene(timeSeconds As Double) As Bitmap
        Dim safeTime As Double = ClampTimeCode(timeSeconds)
        Dim outWidth As Integer = Math.Max(1, _legacyRenderer.ZoomWidth)
        Dim outHeight As Integer = Math.Max(1, _legacyRenderer.ZoomHeight)
        Dim roundRadius As Integer = _legacyRenderer.ZoomRad
        Dim midPoint As PointF = GetRoutePointAtTime(safeTime)

        Dim zoomFactor As Double = _legacyRenderer.ZoomZoom
        If zoomFactor < 0.001 Then zoomFactor = 1
        Dim clipLength As Single = Math.Max(1.0F, CSng(Math.Round(outHeight / zoomFactor)))
        Dim clipWidth As Single = Math.Max(1.0F, CSng(Math.Round(outWidth / zoomFactor)))
        Dim angleDegrees As Double = GetHeadingAngleAtTime(safeTime) + 180
        Dim diagonal As Single = CSng(Math.Sqrt(clipWidth * clipWidth + clipLength * clipLength))
        Dim srcSize As Integer = Math.Max(1, CInt(Math.Ceiling(diagonal)))
        Dim srcRect As New RectangleF(midPoint.X - srcSize / 2.0F, midPoint.Y - srcSize / 2.0F, srcSize, srcSize)
        Dim backgroundKey As String = String.Format(CultureInfo.InvariantCulture, "scene|{0}|{1}|{2}|{3}|{4:0.###}|{5:0.###}|{6:0.###}", outWidth, outHeight, roundRadius, srcSize, midPoint.X, midPoint.Y, angleDegrees)

        Return New Bitmap(GetZoomBackground(backgroundKey, angleDegrees, srcRect, srcSize, clipWidth, clipLength, outWidth, outHeight))
    End Function

    Public Sub WriteLegVideoScene(outputFile As String,
                                  Optional startTime As Double = 0,
                                  Optional duration As Double = -1,
                                  Optional videoWidth As Integer = 1920,
                                  Optional frameStepSeconds As Double = -1,
                                  Optional outputFps As Double = -1)
        WriteVideoInternal(AddressOf RenderLegFrameScene, outputFile, startTime, duration, videoWidth, frameStepSeconds, outputFps)
    End Sub

    Public Sub WriteZoomVideoScene(outputFile As String,
                                   Optional startTime As Double = 0,
                                   Optional duration As Double = -1,
                                   Optional videoWidth As Integer = 1920,
                                   Optional frameStepSeconds As Double = -1,
                                   Optional outputFps As Double = -1)
        WriteVideoInternal(AddressOf RenderZoomFrameScene, outputFile, startTime, duration, videoWidth, frameStepSeconds, outputFps)
    End Sub
#End Region

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

    Public Sub WriteLegVideoPerf2(outputFile As String,
                                  Optional startTime As Double = 0,
                                  Optional duration As Double = -1,
                                  Optional videoWidth As Integer = 1920,
                                  Optional frameStepSeconds As Double = -1,
                                  Optional outputFps As Double = -1)
        WriteVideoInternal(AddressOf RenderLegFramePerf2, outputFile, startTime, duration, videoWidth, frameStepSeconds, outputFps)
    End Sub

    Public Sub WriteLegVideoPerf3(outputFile As String,
                                  Optional startTime As Double = 0,
                                  Optional duration As Double = -1,
                                  Optional videoWidth As Integer = 1920,
                                  Optional frameStepSeconds As Double = -1,
                                  Optional outputFps As Double = -1)
        WriteVideoInternal(AddressOf RenderLegFramePerf3, outputFile, startTime, duration, videoWidth, frameStepSeconds, outputFps)
    End Sub

    Public Sub WriteLegVideoPerf4(outputFile As String,
                                  Optional startTime As Double = 0,
                                  Optional duration As Double = -1,
                                  Optional videoWidth As Integer = 1920,
                                  Optional frameStepSeconds As Double = -1,
                                  Optional outputFps As Double = -1)
        Dim effectiveFrameStep As Double = Me.FrameStepSeconds
        If frameStepSeconds > 0 Then effectiveFrameStep = frameStepSeconds
        PrecomputeFrameStates(startTime, duration, effectiveFrameStep)
        Try
            WriteVideoInternal(AddressOf RenderLegFramePerf4, outputFile, startTime, duration, videoWidth, frameStepSeconds, outputFps)
        Finally
            _frameStateCache.Clear()
        End Try
    End Sub

    Public Sub WriteLegVideoPerf5(outputFile As String,
                                  Optional startTime As Double = 0,
                                  Optional duration As Double = -1,
                                  Optional videoWidth As Integer = 1920,
                                  Optional frameStepSeconds As Double = -1,
                                  Optional outputFps As Double = -1)
        WriteVideoInternal(AddressOf RenderLegFramePerf5, outputFile, startTime, duration, videoWidth, frameStepSeconds, outputFps)
    End Sub

    Public Sub WriteLegVideoPerf6(outputFile As String,
                                  Optional startTime As Double = 0,
                                  Optional duration As Double = -1,
                                  Optional videoWidth As Integer = 1920,
                                  Optional frameStepSeconds As Double = -1,
                                  Optional outputFps As Double = -1)
        WriteVideoInternal(AddressOf RenderLegFramePerf6, outputFile, startTime, duration, videoWidth, frameStepSeconds, outputFps)
    End Sub

    Public Sub WriteLegVideoPerf7(outputFile As String,
                                  Optional startTime As Double = 0,
                                  Optional duration As Double = -1,
                                  Optional videoWidth As Integer = 1920,
                                  Optional frameStepSeconds As Double = -1,
                                  Optional outputFps As Double = -1)
        WriteVideoInternal(AddressOf RenderLegFramePerf7, outputFile, startTime, duration, videoWidth, frameStepSeconds, outputFps)
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

            Dim baseDir As String = Path.GetDirectoryName(outputFile)
            If String.IsNullOrEmpty(baseDir) Then baseDir = "."
            Dim fpsText As String = fps.ToString("0.###", CultureInfo.InvariantCulture)

            Dim psi As New ProcessStartInfo()
            psi.FileName = "ffmpeg"
            If useAlphaOutput Then
                psi.Arguments = $"-y -f rawvideo -pixel_format bgra -video_size {sampleFrame.Width}x{sampleFrame.Height} -framerate {fpsText} -i - -c:v qtrle -pix_fmt argb ""{outputFile}"""
            Else
                psi.Arguments = $"-y -f rawvideo -pixel_format bgr24 -video_size {sampleFrame.Width}x{sampleFrame.Height} -framerate {fpsText} -i - -pix_fmt yuv420p -c:v libx264 -preset veryfast -crf 18 ""{outputFile}"""
            End If
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
                    streamWriteStopwatch.Start()
                    WriteBitmapFrameToStream(sampleFrame, inputStream, useAlphaOutput)
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
                            WriteBitmapFrameToStream(frame, inputStream, useAlphaOutput)
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
            End Using
        End Using

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
        If _frameStateCache.ContainsKey(cacheKey) Then
            Return _frameStateCache(cacheKey).RoutePoint
        End If
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

    Private Function GetHeadingAngleAtTime(timeCode As Double) As Double
        Dim clampedTime As Double = ClampTimeCode(timeCode)
        Dim cacheKey As Integer = GetTimeCacheKey(clampedTime)
        If _frameStateCache.ContainsKey(cacheKey) Then
            Return _frameStateCache(cacheKey).HeadingAngle
        End If
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
        Dim marginHeight As Integer = _legacyRenderer.LegMargin
        Dim roundRadius As Integer = _legacyRenderer.LegRad

        Dim deltaX As Single = endPoint.X - startPoint.X
        Dim deltaY As Single = endPoint.Y - startPoint.Y
        Dim rotationAngle As Single = Math.Atan2(deltaY, deltaX)
        Dim angleDegrees As Double = (90 - rotationAngle * 180 / Math.PI) Mod 360 + 180
        Dim distance As Single = CSng(Math.Sqrt(deltaX * deltaX + deltaY * deltaY))
        Dim midPoint As New PointF((startPoint.X + endPoint.X) / 2.0F, (startPoint.Y + endPoint.Y) / 2.0F)

        Dim clipLength As Single = Math.Max(1.0F, distance + marginHeight)
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
            .BackgroundKey = backgroundKey
        }
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

    Private Function ComputeTailLinePointsAtTime(timeCode As Double) As List(Of PointF)
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

    Private Function GetTailLinePointsAtTime(timeCode As Double) As List(Of PointF)
        Dim clampedTime As Double = ClampTimeCode(timeCode)
        Dim cacheKey As Integer = GetTimeCacheKey(clampedTime)
        If _frameStateCache.ContainsKey(cacheKey) Then
            Return _frameStateCache(cacheKey).TailPoints
        End If
        Return ComputeTailLinePointsAtTime(clampedTime)
    End Function

    Private Sub PrecomputeFrameStates(startTime As Double, duration As Double, frameStepSeconds As Double)
        _frameStateCache.Clear()
        If frameStepSeconds <= 0 Then Return
        If _routePoints Is Nothing OrElse _routePoints.RoutePoints.Count = 0 Then Return

        Dim maxTime As Double = _routePoints.RoutePoints.Count - 1
        Dim clampedStart As Double = Math.Max(0, startTime)
        Dim endTime As Double = If(duration < 0, maxTime, Math.Min(clampedStart + duration - frameStepSeconds, maxTime))
        If clampedStart > endTime Then Return

        Dim currentTime As Double = clampedStart
        Do While currentTime <= endTime + frameStepSeconds / 2
            Dim safeTime As Double = Math.Min(currentTime, endTime)
            Dim cacheKey As Integer = GetTimeCacheKey(safeTime)
            If Not _frameStateCache.ContainsKey(cacheKey) Then
                Dim state As New FrameState With {
                    .RoutePoint = GetRoutePointAtTime(safeTime),
                    .HeadingAngle = GetHeadingAngleAtTime(safeTime),
                    .TailPoints = ComputeTailLinePointsAtTime(safeTime)
                }
                _frameStateCache(cacheKey) = state
            End If
            currentTime += frameStepSeconds
        Loop
    End Sub

    Private Function GetLapLinePoints(lap As Integer) As List(Of PointF)
        Dim linePoints As New List(Of PointF)
        If lap < 0 Then Return linePoints
        Dim targetLapNumber As Integer = lap + 1

        For Each rp As clsQRRoutePoint In _routePoints.RoutePoints
            If rp.LapNumber = targetLapNumber Then
                linePoints.Add(New PointF(CSng(rp.ImageX), CSng(rp.ImageY + _routePoints.QRLogoYOffset)))
            End If
        Next

        Return linePoints
    End Function

    Private Function CreateWorldToScreenTransform(outWidth As Integer, outHeight As Integer, centerWorld As PointF, angleDegrees As Double, clipWidth As Single, clipLength As Single) As Matrix
        Dim scaleX As Single = CSng(outWidth / Math.Max(1.0F, clipWidth))
        Dim scaleY As Single = CSng(outHeight / Math.Max(1.0F, clipLength))
        Dim transform As New Matrix()

        transform.Translate(outWidth / 2.0F, outHeight / 2.0F, MatrixOrder.Append)
        transform.Scale(scaleX, scaleY, MatrixOrder.Append)
        transform.Rotate(CSng(angleDegrees), MatrixOrder.Append)
        transform.Translate(-centerWorld.X, -centerWorld.Y, MatrixOrder.Append)

        Return transform
    End Function

    Private Function TransformPoint(sourcePoint As PointF, transform As Matrix) As PointF
        Dim points() As PointF = {sourcePoint}
        transform.TransformPoints(points)
        Return points(0)
    End Function

    Private Function GetScreenArrowDirectionAtTime(timeCode As Double, transform As Matrix) As Double
        Dim startPoint As PointF
        Dim endPoint As PointF
        GetHeadingSamplePoints(timeCode, startPoint, endPoint)
        Dim points() As PointF = {startPoint, endPoint}
        transform.TransformPoints(points)
        Dim dx As Double = points(1).X - points(0).X
        Dim dy As Double = points(1).Y - points(0).Y
        Dim vectorAngle As Double = Math.Atan2(dy, dx) * 180.0 / Math.PI
        Return (vectorAngle + 90.0 + 360.0) Mod 360.0
    End Function

    Private Function RenderSceneFrame(currentTime As Double,
                                      outWidth As Integer,
                                      outHeight As Integer,
                                      roundRadius As Integer,
                                      centerWorld As PointF,
                                      angleDegrees As Double,
                                      clipWidth As Single,
                                      clipLength As Single,
                                      isZoom As Boolean,
                                      Optional lap As Integer = -1,
                                      Optional background As Bitmap = Nothing) As Bitmap
        outWidth = Math.Max(1, outWidth)
        outHeight = Math.Max(1, outHeight)
        Dim result As Bitmap
        If background IsNot Nothing Then
            result = New Bitmap(background)
        Else
            result = New Bitmap(outWidth, outHeight, PixelFormat.Format32bppArgb)
        End If

        Using worldToScreen As Matrix = CreateWorldToScreenTransform(outWidth, outHeight, centerWorld, angleDegrees, clipWidth, clipLength)
            Using g As Graphics = Graphics.FromImage(result)
                If background Is Nothing Then g.Clear(Color.Transparent)
                g.SmoothingMode = SmoothingMode.AntiAlias
                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                g.CompositingQuality = CompositingQuality.HighQuality
                g.PixelOffsetMode = PixelOffsetMode.HighQuality

                If lap >= 0 Then
                    Dim lapLine As List(Of PointF) = GetLapLinePoints(lap)
                    If lapLine.Count > 1 Then
                        Dim lapArray As PointF() = lapLine.ToArray()
                        worldToScreen.TransformPoints(lapArray)
                        Using routePen As New Pen(Color.FromArgb(80, _legacyRenderer.tailLineColor), Math.Max(1.0F, _legacyRenderer.dotSize * 0.6F))
                            routePen.LineJoin = LineJoin.Round
                            routePen.StartCap = LineCap.Round
                            routePen.EndCap = LineCap.Round
                            g.DrawLines(routePen, lapArray)
                        End Using
                    End If
                End If

                Dim tailPoints As List(Of PointF) = GetTailLinePointsAtTime(currentTime)
                If tailPoints.Count > 1 Then
                    Dim tailArray As PointF() = tailPoints.ToArray()
                    worldToScreen.TransformPoints(tailArray)
                    Using tailPen As New Pen(_legacyRenderer.tailLineColor, Math.Max(1.0F, _legacyRenderer.dotSize * _legacyRenderer.dotTailRatio))
                        tailPen.LineJoin = LineJoin.Round
                        tailPen.StartCap = LineCap.Round
                        tailPen.EndCap = LineCap.Round
                        g.DrawLines(tailPen, tailArray)
                    End Using
                End If

                Dim currentPos As PointF = TransformPoint(GetRoutePointAtTime(currentTime), worldToScreen)

                If _legacyRenderer._DotType = "Arrow" Then
                    DrawArrowWithBarbs(g, currentPos, CSng(GetScreenArrowDirectionAtTime(currentTime, worldToScreen)), _legacyRenderer.dotSize, _legacyRenderer.dotColor)
                Else
                    Using dotBrush As New SolidBrush(_legacyRenderer.dotColor)
                        g.FillEllipse(dotBrush,
                                      currentPos.X - _legacyRenderer.dotSize / 2.0F,
                                      currentPos.Y - _legacyRenderer.dotSize / 2.0F,
                                      _legacyRenderer.dotSize,
                                      _legacyRenderer.dotSize)
                    End Using
                End If
            End Using
        End Using

        Return result
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

    Private Function GetLegBackgroundFast(cacheKey As String, angleDegrees As Double, srcRect As RectangleF, srcSize As Integer, clipWidth As Single, clipLength As Single, outWidth As Integer, outHeight As Integer) As Bitmap
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
                    g.SmoothingMode = SmoothingMode.HighSpeed
                    g.InterpolationMode = InterpolationMode.Bilinear
                    g.PixelOffsetMode = PixelOffsetMode.HighSpeed
                    g.TranslateTransform(tmpBackground.Width / 2.0F, tmpBackground.Height / 2.0F)
                    g.RotateTransform(CSng(angleDegrees))
                    g.TranslateTransform(-tmpBackground.Width / 2.0F, -tmpBackground.Height / 2.0F)
                    g.DrawImage(_mapImage, New RectangleF(0, 0, tmpBackground.Width, tmpBackground.Height), srcRect, GraphicsUnit.Pixel)
                End Using

                _legBackground = New Bitmap(outWidth, outHeight)
                Using g As Graphics = Graphics.FromImage(_legBackground)
                    g.InterpolationMode = InterpolationMode.Bilinear
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

    Private Function RenderOverlayLegFrameDirect(currentTime As Double, info As LegRenderInfo) As Bitmap
        Dim overlayMap As Bitmap = GetOverlayMap(currentTime)
        Dim result As New Bitmap(info.OutWidth, info.OutHeight, PixelFormat.Format32bppArgb)

        Using worldToScreen As Matrix = CreateWorldToScreenTransform(info.OutWidth, info.OutHeight, info.MidPoint, info.AngleDegrees, info.ClipWidth, info.ClipLength)
            Using g As Graphics = Graphics.FromImage(result)
                g.Clear(Color.Transparent)
                g.SmoothingMode = SmoothingMode.HighQuality
                g.InterpolationMode = InterpolationMode.HighQualityBicubic
                g.CompositingQuality = CompositingQuality.HighQuality
                g.PixelOffsetMode = PixelOffsetMode.HighQuality
                g.Transform = worldToScreen
                g.DrawImage(overlayMap, New Rectangle(0, 0, overlayMap.Width, overlayMap.Height))
                g.ResetTransform()
            End Using
        End Using

        Return result
    End Function

    Private Function RenderOverlayLegFrameAdaptive(currentTime As Double, info As LegRenderInfo, workSize As Integer) As Bitmap
        Dim scaledClipWidth As Single = Math.Max(1.0F, CSng(workSize * info.ClipWidth / Math.Max(1.0F, info.SrcSize)))
        Dim scaledClipLength As Single = Math.Max(1.0F, CSng(workSize * info.ClipLength / Math.Max(1.0F, info.SrcSize)))
        Return RenderOverlayFrame(currentTime, info.AngleDegrees, info.SrcRect, workSize, scaledClipWidth, scaledClipLength, info.OutWidth, info.OutHeight)
    End Function

    Private Function RenderOverlayFrameLocalAdaptive(currentTime As Double, info As LegRenderInfo, workSize As Integer, Optional fastQuality As Boolean = False) As Bitmap
        Dim scaledClipWidth As Single = Math.Max(1.0F, CSng(workSize * info.ClipWidth / Math.Max(1.0F, info.SrcSize)))
        Dim scaledClipLength As Single = Math.Max(1.0F, CSng(workSize * info.ClipLength / Math.Max(1.0F, info.SrcSize)))
        Return RenderOverlayFrameLocal(currentTime, info.AngleDegrees, info.SrcRect, workSize, scaledClipWidth, scaledClipLength, info.OutWidth, info.OutHeight, fastQuality)
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

    Private Function RenderOverlayFrameLocal(currentTime As Double,
                                             angleDegrees As Double,
                                             srcRect As RectangleF,
                                             srcSize As Integer,
                                             clipWidth As Single,
                                             clipLength As Single,
                                             outWidth As Integer,
                                             outHeight As Integer,
                                             Optional fastQuality As Boolean = False) As Bitmap
        Dim stopwatch As Stopwatch = Stopwatch.StartNew()
        srcSize = Math.Max(1, srcSize)
        outWidth = Math.Max(1, outWidth)
        outHeight = Math.Max(1, outHeight)
        clipWidth = Math.Max(1.0F, clipWidth)
        clipLength = Math.Max(1.0F, clipLength)

        Using tmpOverlay As New Bitmap(srcSize, srcSize, PixelFormat.Format32bppArgb)
            Using g As Graphics = Graphics.FromImage(tmpOverlay)
                g.Clear(Color.Transparent)
                g.SmoothingMode = If(fastQuality, SmoothingMode.HighSpeed, SmoothingMode.HighQuality)
                g.CompositingQuality = If(fastQuality, CompositingQuality.HighSpeed, CompositingQuality.HighQuality)
                g.PixelOffsetMode = If(fastQuality, PixelOffsetMode.HighSpeed, PixelOffsetMode.HighQuality)
                g.InterpolationMode = If(fastQuality, InterpolationMode.Bilinear, InterpolationMode.HighQualityBicubic)
                g.TranslateTransform(-srcRect.X, -srcRect.Y, MatrixOrder.Append)
                g.TranslateTransform(tmpOverlay.Width / 2.0F, tmpOverlay.Height / 2.0F)
                g.RotateTransform(CSng(angleDegrees))
                g.TranslateTransform(-tmpOverlay.Width / 2.0F, -tmpOverlay.Height / 2.0F)

                Dim tailPoints As List(Of PointF) = GetTailLinePointsAtTime(currentTime)
                If tailPoints.Count > 1 Then
                    Using tailPen As New Pen(_legacyRenderer.tailLineColor, _legacyRenderer.dotSize * _legacyRenderer.dotTailRatio)
                        tailPen.LineJoin = LineJoin.Round
                        tailPen.StartCap = LineCap.Round
                        tailPen.EndCap = LineCap.Round
                        g.DrawLines(tailPen, tailPoints.ToArray())
                    End Using
                End If

                Dim currentPos As PointF = GetRoutePointAtTime(currentTime)
                If _legacyRenderer._DotType = "Arrow" Then
                    DrawArrowWithBarbs(g, currentPos, CSng(GetArrowDirectionAtTime(currentTime)), _legacyRenderer.dotSize, _legacyRenderer.dotColor)
                Else
                    Using dotBrush As New SolidBrush(_legacyRenderer.dotColor)
                        g.FillEllipse(dotBrush,
                                      currentPos.X - _legacyRenderer.dotSize / 2.0F,
                                      currentPos.Y - _legacyRenderer.dotSize / 2.0F,
                                      _legacyRenderer.dotSize,
                                      _legacyRenderer.dotSize)
                    End Using
                End If
                g.ResetTransform()
            End Using

            Dim result As New Bitmap(outWidth, outHeight, PixelFormat.Format32bppArgb)
            Using g As Graphics = Graphics.FromImage(result)
                g.InterpolationMode = If(fastQuality, InterpolationMode.Bilinear, InterpolationMode.HighQualityBicubic)
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
