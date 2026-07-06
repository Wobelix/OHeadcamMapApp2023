Imports System.Drawing.Drawing2D
Imports System.Drawing.Text
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports System.Diagnostics
Imports System.Runtime.InteropServices

Public Enum DataWidgetKind
    Time
    Distance
    Pace
    Pulse
    HGraph
End Enum

Public Class DataWidgetSample
    Public Property TimeSeconds As Double
    Public Property DistanceMeters As Double
    Public Property Pace As Double
    Public Property Speed As Double
    Public Property HeartRate As Double
End Class

Public Class clsDataWidgetRenderEngine
    Private ReadOnly _routePoints As clsQRRoutePoints
    Private ReadOnly _minAltitude As Double = 0
    Private ReadOnly _maxAltitude As Double = 1
    Private ReadOnly _minHeartRate As Double = 0
    Private ReadOnly _maxHeartRate As Double = 0
    Private Const PaceSmoothingWindowSeconds As Double = 12.0
    Private Shared ReadOnly _fontCollection As New PrivateFontCollection()
    Private Shared _fontsLoaded As Boolean = False
    Public Property WidgetTimeOffsetSeconds As Double = 0

    Public Sub New(routePoints As clsQRRoutePoints)
        _routePoints = New clsQRRoutePoints(routePoints)
        If _routePoints.RoutePoints.Count > 0 Then
            _minAltitude = _routePoints.RoutePoints.Min(Function(p) p.Altitude)
            _maxAltitude = _routePoints.RoutePoints.Max(Function(p) p.Altitude)
            If Math.Abs(_maxAltitude - _minAltitude) < 0.001 Then _maxAltitude = _minAltitude + 1

            Dim heartRates = _routePoints.RoutePoints.Where(Function(p) p.HeartRate > 0).Select(Function(p) CDbl(p.HeartRate)).ToList()
            If heartRates.Count > 0 Then
                _minHeartRate = heartRates.Min()
                _maxHeartRate = heartRates.Max()
                If Math.Abs(_maxHeartRate - _minHeartRate) < 0.001 Then _maxHeartRate = _minHeartRate + 1
            End If
        End If
        EnsureFontsLoaded()
    End Sub

    Public Function RenderWidget(kind As DataWidgetKind, timeSeconds As Double, width As Integer, height As Integer) As Bitmap
        If kind = DataWidgetKind.HGraph Then Return RenderHeightGraph(timeSeconds, width, height)

        Dim safeWidth As Integer = Math.Max(30, width)
        Dim safeHeight As Integer = Math.Max(30, height)
        Dim sample As DataWidgetSample = GetSampleAtTime(timeSeconds)
        Dim label As String = GetLabel(kind)
        Dim value As String = GetValue(kind, sample)
        Dim accent As Color = GetAccentColor(kind)

        Dim bmp As New Bitmap(safeWidth, safeHeight, Imaging.PixelFormat.Format32bppArgb)
        Using g As Graphics = Graphics.FromImage(bmp)
            g.SmoothingMode = SmoothingMode.AntiAlias
            g.CompositingQuality = CompositingQuality.HighQuality
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit
            g.Clear(Color.Transparent)

            Dim pad As Single = Math.Max(4.0F, safeHeight * 0.13F)
            Dim radius As Integer = CInt(Math.Max(6.0F, safeHeight * 0.18F))
            Dim rect As New Rectangle(0, 0, safeWidth - 1, safeHeight - 1)
            Using path As GraphicsPath = RoundedRect(rect, radius)
                Using bg As New SolidBrush(Color.FromArgb(150, 8, 12, 16))
                    g.FillPath(bg, path)
                End Using
                Using edgePen As New Pen(Color.FromArgb(60, Color.White), Math.Max(1.0F, safeHeight * 0.018F))
                    g.DrawPath(edgePen, path)
                End Using
            End Using

            Using accentBrush As New SolidBrush(Color.FromArgb(220, accent))
                g.FillRectangle(accentBrush, 0, 0, Math.Max(3, CInt(safeWidth * 0.025)), safeHeight)
            End Using

            Dim accentWidth As Single = Math.Max(3.0F, safeWidth * 0.025F)
            Dim textLeft As Single = pad + accentWidth * 0.6F
            Dim textWidth As Single = Math.Max(1.0F, safeWidth - textLeft - pad)
            Dim labelTop As Single = pad * 0.55F
            Dim labelHeight As Single = Math.Max(8.0F, Math.Min(safeHeight * 0.24F, safeHeight - pad * 2.0F))
            Dim gap As Single = Math.Max(1.0F, Math.Min(safeHeight * 0.055F, pad * 0.7F))
            Dim valueTop As Single = labelTop + labelHeight + gap
            Dim valueHeight As Single = Math.Max(8.0F, safeHeight - valueTop - pad * 0.65F)

            Dim labelRect As New RectangleF(textLeft, labelTop, textWidth, labelHeight)
            Dim valueRect As New RectangleF(textLeft, valueTop, textWidth, valueHeight)
            Dim labelFont As Font = CreateFittingFont(g, label.ToUpperInvariant(), labelRect.Width, labelRect.Height, FontStyle.Bold)
            Dim valueFont As Font = CreateFittingFont(g, value, valueRect.Width, valueRect.Height, FontStyle.Bold)
            Try
                Using labelFormat As New StringFormat(StringFormat.GenericTypographic)
                    labelFormat.Alignment = StringAlignment.Near
                    labelFormat.LineAlignment = StringAlignment.Center
                    labelFormat.FormatFlags = StringFormatFlags.NoWrap
                    labelFormat.Trimming = StringTrimming.EllipsisCharacter

                    Using valueFormat As New StringFormat(StringFormat.GenericTypographic)
                        valueFormat.Alignment = StringAlignment.Near
                        valueFormat.LineAlignment = StringAlignment.Center
                        valueFormat.FormatFlags = StringFormatFlags.NoWrap
                        valueFormat.Trimming = StringTrimming.EllipsisCharacter

                Using labelBrush As New SolidBrush(Color.FromArgb(180, 226, 230, 235))
                            g.DrawString(label.ToUpperInvariant(), labelFont, labelBrush, labelRect, labelFormat)
                End Using

                Using shadowBrush As New SolidBrush(Color.FromArgb(120, Color.Black))
                            Dim shadowRect As RectangleF = valueRect
                            shadowRect.Offset(1.5F, 1.5F)
                            g.DrawString(value, valueFont, shadowBrush, shadowRect, valueFormat)
                End Using
                Using valueBrush As New SolidBrush(Color.White)
                            g.DrawString(value, valueFont, valueBrush, valueRect, valueFormat)
                        End Using
                    End Using
                End Using
            Finally
                labelFont.Dispose()
                valueFont.Dispose()
            End Try
        End Using

        Return bmp
    End Function

    Public Sub WriteWidgetVideo(kind As DataWidgetKind,
                                outputFile As String,
                                width As Integer,
                                height As Integer,
                                Optional startTime As Double = 0,
                                Optional duration As Double = -1,
                                Optional frameStepSeconds As Double = 0.25,
                                Optional outputFps As Double = -1)
        Dim safeWidth As Integer = Math.Max(30, width)
        Dim safeHeight As Integer = Math.Max(30, height)
        Dim effectiveFrameStep As Double = If(frameStepSeconds > 0, frameStepSeconds, 0.25)
        Dim fps As Double = If(outputFps > 0, outputFps, 1.0 / effectiveFrameStep)
        Dim clampedStart As Double = Math.Max(0, startTime)
        Dim maxTimeline As Double = If(_routePoints Is Nothing OrElse _routePoints.RoutePoints.Count = 0, 0, Math.Max(0, _routePoints.RoutePoints.Count - 1))
        Dim endTime As Double = If(duration > 0, clampedStart + duration, maxTimeline)
        If endTime < clampedStart Then endTime = clampedStart

        Dim outputDir As String = Path.GetDirectoryName(outputFile)
        If Not String.IsNullOrWhiteSpace(outputDir) Then Directory.CreateDirectory(outputDir)

        Dim fpsText As String = fps.ToString("0.###", CultureInfo.InvariantCulture)
        Dim psi As New ProcessStartInfo()
        psi.FileName = "ffmpeg"
        psi.Arguments = $"-y -f rawvideo -pixel_format bgra -video_size {safeWidth}x{safeHeight} -framerate {fpsText} -i - -c:v qtrle -pix_fmt argb ""{outputFile}"""
        psi.UseShellExecute = False
        psi.RedirectStandardInput = True
        psi.RedirectStandardError = True
        psi.CreateNoWindow = True

        Using ffmpegProcess As Process = Process.Start(psi)
            Dim stderrReader = ffmpegProcess.StandardError
            Dim stderrTask = Threading.Tasks.Task.Run(Function()
                                                          Try
                                                              Return stderrReader.ReadToEnd()
                                                          Catch
                                                              Return ""
                                                          End Try
                                                      End Function)

            Using inputStream As Stream = ffmpegProcess.StandardInput.BaseStream
                Dim frameNo As Integer = 0
                Dim epsilon As Double = effectiveFrameStep / 1000.0
                Do
                    Dim currentTime As Double = clampedStart + frameNo * effectiveFrameStep
                    If currentTime > endTime + epsilon Then Exit Do
                    Using frame As Bitmap = RenderWidget(kind, currentTime, safeWidth, safeHeight)
                        WriteBitmapFrameToStream(frame, inputStream)
                    End Using
                    frameNo += 1
                Loop
            End Using

            ffmpegProcess.WaitForExit()
            Dim stderr As String = ""
            Try
                stderr = stderrTask.Result
            Catch
            End Try
            If ffmpegProcess.ExitCode <> 0 Then
                Throw New InvalidOperationException("ffmpeg widget encode failed: " & stderr)
            End If
        End Using
    End Sub

    Private Function GetSampleAtTime(timeSeconds As Double) As DataWidgetSample
        Dim sample As New DataWidgetSample With {.TimeSeconds = Math.Max(0, timeSeconds)}
        If _routePoints Is Nothing OrElse _routePoints.RoutePoints.Count = 0 Then Return sample

        Dim safeTime As Double = Math.Max(0, Math.Min(timeSeconds, _routePoints.RoutePoints.Count - 1))
        Dim index0 As Integer = CInt(Math.Floor(safeTime))
        Dim index1 As Integer = Math.Min(_routePoints.RoutePoints.Count - 1, index0 + 1)
        Dim t As Double = safeTime - index0
        Dim p0 As clsQRRoutePoint = _routePoints.RoutePoints(index0)
        Dim p1 As clsQRRoutePoint = _routePoints.RoutePoints(index1)

        sample.TimeSeconds = safeTime
        sample.DistanceMeters = Lerp(p0.RouteDistanceFromStart, p1.RouteDistanceFromStart, t)
        sample.Speed = Lerp(p0.Speed, p1.Speed, t)
        sample.Pace = GetSmoothedPaceAtTime(safeTime, PaceSmoothingWindowSeconds)
        If sample.Pace <= 0 Then sample.Pace = Lerp(p0.Pace, p1.Pace, t)
        sample.HeartRate = Lerp(p0.HeartRate, p1.HeartRate, t)
        Return sample
    End Function

    Private Shared Function Lerp(a As Double, b As Double, t As Double) As Double
        Return a + (b - a) * Math.Max(0, Math.Min(1, t))
    End Function

    Private Shared Function GetLabel(kind As DataWidgetKind) As String
        Select Case kind
            Case DataWidgetKind.Time
                Return GetResourceText("WidgetTime", If(IsDanishUi(), "Tid", "Time"))
            Case DataWidgetKind.Distance
                Return GetResourceText("WidgetDistance", "Distance")
            Case DataWidgetKind.Pace
                Return GetResourceText("WidgetPace", If(IsDanishUi(), "Tempo", "Pace"))
            Case DataWidgetKind.Pulse
                Return GetResourceText("WidgetPulse", If(IsDanishUi(), "Puls", "HR"))
            Case Else
                Return ""
        End Select
    End Function

    Private Shared Function GetResourceText(resourceName As String, fallback As String) As String
        Try
            Dim text As String = My.Resources.Texts.ResourceManager.GetString(resourceName, CultureInfo.CurrentUICulture)
            If Not String.IsNullOrWhiteSpace(text) Then Return text
        Catch
        End Try
        Return fallback
    End Function

    Private Function GetValue(kind As DataWidgetKind, sample As DataWidgetSample) As String
        Select Case kind
            Case DataWidgetKind.Time
                Return FormatDuration(sample.TimeSeconds + WidgetTimeOffsetSeconds)
            Case DataWidgetKind.Distance
                Return (sample.DistanceMeters / 1000.0).ToString("0.00", CultureInfo.InvariantCulture) & " km"
            Case DataWidgetKind.Pace
                Return FormatPace(sample)
            Case DataWidgetKind.Pulse
                If sample.HeartRate <= 0 Then Return "-- bpm"
                Return CInt(Math.Round(sample.HeartRate)).ToString(CultureInfo.InvariantCulture) & " bpm"
            Case DataWidgetKind.HGraph
                Return CInt(Math.Round(sample.HeartRate)).ToString(CultureInfo.InvariantCulture) & " m"
            Case Else
                Return ""
        End Select
    End Function

    Private Shared Function FormatDuration(seconds As Double) As String
        Dim totalSeconds As Integer = CInt(Math.Max(0, Math.Round(seconds)))
        Dim ts As TimeSpan = TimeSpan.FromSeconds(totalSeconds)
        If ts.TotalHours >= 1 Then Return String.Format(CultureInfo.InvariantCulture, "{0}:{1:00}:{2:00}", CInt(Math.Floor(ts.TotalHours)), ts.Minutes, ts.Seconds)
        Return String.Format(CultureInfo.InvariantCulture, "{0}:{1:00}", ts.Minutes, ts.Seconds)
    End Function

    Private Shared Function FormatPace(sample As DataWidgetSample) As String
        Dim secondsPerKm As Double = sample.Pace
        If secondsPerKm <= 0 AndAlso sample.Speed > 0 Then
            secondsPerKm = 1000.0 / sample.Speed
        End If
        If secondsPerKm > 0 AndAlso secondsPerKm < 40 Then
            secondsPerKm *= 60.0
        End If
        If secondsPerKm <= 0 Then Return "-- /km"
        Dim minutes As Integer = CInt(Math.Floor(secondsPerKm / 60.0))
        Dim seconds As Integer = CInt(Math.Round(secondsPerKm - minutes * 60))
        If seconds >= 60 Then
            minutes += 1
            seconds -= 60
        End If
        Return String.Format(CultureInfo.InvariantCulture, "{0}:{1:00} /km", minutes, seconds)
    End Function

    Private Shared Function GetAccentColor(kind As DataWidgetKind) As Color
        Select Case kind
            Case DataWidgetKind.Time
                Return Color.FromArgb(95, 190, 255)
            Case DataWidgetKind.Distance
                Return Color.FromArgb(90, 220, 150)
            Case DataWidgetKind.Pace
                Return Color.FromArgb(255, 205, 70)
            Case DataWidgetKind.Pulse
                Return Color.FromArgb(255, 90, 105)
            Case DataWidgetKind.HGraph
                Return Color.FromArgb(90, 210, 185)
            Case Else
                Return Color.White
        End Select
    End Function

    Private Function RenderHeightGraph(timeSeconds As Double, width As Integer, height As Integer) As Bitmap
        Dim safeWidth As Integer = Math.Max(80, width)
        Dim safeHeight As Integer = Math.Max(50, height)
        Dim currentAltitude As Double = GetAltitudeAtTime(timeSeconds)
        Dim bmp As New Bitmap(safeWidth, safeHeight, Imaging.PixelFormat.Format32bppArgb)

        Using g As Graphics = Graphics.FromImage(bmp)
            g.SmoothingMode = SmoothingMode.AntiAlias
            g.CompositingQuality = CompositingQuality.HighQuality
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit
            g.Clear(Color.Transparent)

            Dim pad As Single = Math.Max(5.0F, safeHeight * 0.08F)
            Dim labelWidth As Single = Math.Max(38.0F, safeWidth * 0.16F)
            Dim rect As New Rectangle(0, 0, safeWidth - 1, safeHeight - 1)
            Using path As GraphicsPath = RoundedRect(rect, CInt(Math.Max(6.0F, safeHeight * 0.12F)))
                Using bg As New SolidBrush(Color.FromArgb(150, 8, 12, 16))
                    g.FillPath(bg, path)
                End Using
                Using edgePen As New Pen(Color.FromArgb(60, Color.White), Math.Max(1.0F, safeHeight * 0.012F))
                    g.DrawPath(edgePen, path)
                End Using
            End Using

            Dim topText As String = CInt(Math.Round(_maxAltitude)).ToString(CultureInfo.InvariantCulture) & " m"
            Dim midText As String = CInt(Math.Round(currentAltitude)).ToString(CultureInfo.InvariantCulture) & " m"
            Dim bottomText As String = CInt(Math.Round(_minAltitude)).ToString(CultureInfo.InvariantCulture) & " m"
            Dim longestLabel As String = New String() {topText, midText, bottomText}.OrderByDescending(Function(s) s.Length).First()
            Dim maxLabelWidth As Single = Math.Max(48.0F, safeHeight * 0.95F)
            If safeWidth < 260 Then maxLabelWidth = Math.Min(maxLabelWidth, safeWidth * 0.32F)
            Dim labelFont As Font = CreateFittingFont(g, longestLabel, maxLabelWidth, safeHeight * 0.16F, FontStyle.Bold)
            Try
                Dim topSize As SizeF = g.MeasureString(topText, labelFont)
                Dim midSize As SizeF = g.MeasureString(midText, labelFont)
                Dim bottomSize As SizeF = g.MeasureString(bottomText, labelFont)
                labelWidth = Math.Max(topSize.Width, Math.Max(midSize.Width, bottomSize.Width)) + pad * 0.8F
                Using labelBrush As New SolidBrush(Color.FromArgb(205, 235, 240, 242))
                    g.DrawString(topText, labelFont, labelBrush, pad, pad * 0.55F)
                    g.DrawString(midText, labelFont, labelBrush, pad, (safeHeight - midSize.Height) / 2.0F)
                    g.DrawString(bottomText, labelFont, labelBrush, pad, safeHeight - bottomSize.Height - pad * 0.55F)
                End Using
            Finally
                labelFont.Dispose()
            End Try

            Dim graphLeft As Single = pad + labelWidth
            Dim graphTop As Single = pad
            Dim graphWidth As Single = Math.Max(1.0F, safeWidth - graphLeft - pad)
            Dim graphHeight As Single = Math.Max(1.0F, safeHeight - pad * 2)
            Dim graphBottom As Single = graphTop + graphHeight
            Using gridPen As New Pen(Color.FromArgb(45, Color.White), 1.0F)
                g.DrawLine(gridPen, graphLeft, graphTop, graphLeft + graphWidth, graphTop)
                g.DrawLine(gridPen, graphLeft, graphBottom, graphLeft + graphWidth, graphBottom)
            End Using

            If _routePoints.RoutePoints.Count > 1 Then
                Dim totalTime As Double = Math.Max(1.0, _routePoints.RoutePoints.Count - 1)
                Dim safeTime As Double = Math.Max(0, Math.Min(timeSeconds, totalTime))
                Dim progressX As Single = graphLeft + CSng((safeTime / totalTime) * graphWidth)
                Dim stepSeconds As Double = Math.Max(1.0, totalTime / Math.Max(12.0, graphWidth))

                Using areaPath As New GraphicsPath()
                    areaPath.StartFigure()
                    Dim lastAreaX As Single = graphLeft
                    Dim lastAreaY As Single = AltitudeToY(GetAltitudeAtTime(0), graphTop, graphHeight)
                    areaPath.AddLine(graphLeft, graphBottom, lastAreaX, lastAreaY)
                    Dim t As Double = 0
                    Do While t <= safeTime + 0.0001
                        Dim x As Single = graphLeft + CSng((t / totalTime) * graphWidth)
                        Dim y As Single = AltitudeToY(GetAltitudeAtTime(t), graphTop, graphHeight)
                        areaPath.AddLine(lastAreaX, lastAreaY, x, y)
                        lastAreaX = x
                        lastAreaY = y
                        t += stepSeconds
                    Loop
                    areaPath.AddLine(lastAreaX, lastAreaY, progressX, AltitudeToY(currentAltitude, graphTop, graphHeight))
                    areaPath.AddLine(progressX, AltitudeToY(currentAltitude, graphTop, graphHeight), progressX, graphBottom)
                    areaPath.CloseFigure()
                    Using fillBrush As New SolidBrush(Color.FromArgb(155, 62, 205, 170))
                        g.FillPath(fillBrush, areaPath)
                    End Using
                End Using

                Using linePen As New Pen(Color.FromArgb(235, 130, 255, 225), Math.Max(1.4F, safeHeight * 0.018F))
                    linePen.LineJoin = LineJoin.Round
                    Dim prevPoint As PointF? = Nothing
                    Dim t As Double = 0
                    Do While t <= safeTime + 0.0001
                        Dim point As New PointF(graphLeft + CSng((t / totalTime) * graphWidth), AltitudeToY(GetAltitudeAtTime(t), graphTop, graphHeight))
                        If prevPoint.HasValue Then g.DrawLine(linePen, prevPoint.Value, point)
                        prevPoint = point
                        t += stepSeconds
                    Loop
                End Using

                If _maxHeartRate > _minHeartRate Then
                    Using pulseFill As New GraphicsPath()
                        pulseFill.StartFigure()
                        Dim lastPulseX As Single = graphLeft
                        Dim lastPulseY As Single = HeartRateToY(GetHeartRateAtTime(0), graphTop, graphHeight)
                        pulseFill.AddLine(graphLeft, graphBottom, lastPulseX, lastPulseY)
                        Dim t As Double = 0
                        Do While t <= safeTime + 0.0001
                            Dim heartRate As Double = GetHeartRateAtTime(t)
                            If heartRate > 0 Then
                                Dim x As Single = graphLeft + CSng((t / totalTime) * graphWidth)
                                Dim y As Single = HeartRateToY(heartRate, graphTop, graphHeight)
                                pulseFill.AddLine(lastPulseX, lastPulseY, x, y)
                                lastPulseX = x
                                lastPulseY = y
                            End If
                            t += stepSeconds
                        Loop
                        pulseFill.AddLine(lastPulseX, lastPulseY, lastPulseX, graphBottom)
                        pulseFill.CloseFigure()
                        Using pulseBrush As New SolidBrush(Color.FromArgb(34, 255, 55, 70))
                            g.FillPath(pulseBrush, pulseFill)
                        End Using
                    End Using

                    Using pulsePen As New Pen(Color.FromArgb(220, 255, 65, 82), Math.Max(1.3F, safeHeight * 0.017F))
                        pulsePen.LineJoin = LineJoin.Round
                        Dim prevPoint As PointF? = Nothing
                        Dim t As Double = 0
                        Do While t <= safeTime + 0.0001
                            Dim heartRate As Double = GetHeartRateAtTime(t)
                            If heartRate > 0 Then
                                Dim point As New PointF(graphLeft + CSng((t / totalTime) * graphWidth), HeartRateToY(heartRate, graphTop, graphHeight))
                                If prevPoint.HasValue Then g.DrawLine(pulsePen, prevPoint.Value, point)
                                prevPoint = point
                            End If
                            t += stepSeconds
                        Loop
                    End Using
                End If

                Using markerPen As New Pen(Color.FromArgb(170, Color.White), 1.0F)
                    g.DrawLine(markerPen, progressX, graphTop, progressX, graphBottom)
                End Using
            End If
        End Using

        Return bmp
    End Function

    Private Function GetSmoothedPaceAtTime(timeSeconds As Double, windowSeconds As Double) As Double
        If _routePoints Is Nothing OrElse _routePoints.RoutePoints.Count < 2 Then Return 0
        Dim halfWindow As Double = Math.Max(1.0, windowSeconds / 2.0)
        Dim t0 As Double = Math.Max(0, timeSeconds - halfWindow)
        Dim t1 As Double = Math.Min(_routePoints.RoutePoints.Count - 1, timeSeconds + halfWindow)
        If t1 <= t0 + 0.001 Then Return 0

        Dim d0 As Double = GetDistanceAtTime(t0)
        Dim d1 As Double = GetDistanceAtTime(t1)
        Dim distanceMeters As Double = d1 - d0
        If distanceMeters <= 0.1 Then Return 0
        Return ((t1 - t0) / distanceMeters) * 1000.0
    End Function

    Private Function GetDistanceAtTime(timeSeconds As Double) As Double
        If _routePoints Is Nothing OrElse _routePoints.RoutePoints.Count = 0 Then Return 0
        Dim safeTime As Double = Math.Max(0, Math.Min(timeSeconds, _routePoints.RoutePoints.Count - 1))
        Dim index0 As Integer = CInt(Math.Floor(safeTime))
        Dim index1 As Integer = Math.Min(_routePoints.RoutePoints.Count - 1, index0 + 1)
        Dim t As Double = safeTime - index0
        Return Lerp(_routePoints.RoutePoints(index0).RouteDistanceFromStart, _routePoints.RoutePoints(index1).RouteDistanceFromStart, t)
    End Function

    Private Function GetAltitudeAtTime(timeSeconds As Double) As Double
        If _routePoints Is Nothing OrElse _routePoints.RoutePoints.Count = 0 Then Return 0
        Dim safeTime As Double = Math.Max(0, Math.Min(timeSeconds, _routePoints.RoutePoints.Count - 1))
        Dim index0 As Integer = CInt(Math.Floor(safeTime))
        Dim index1 As Integer = Math.Min(_routePoints.RoutePoints.Count - 1, index0 + 1)
        Dim t As Double = safeTime - index0
        Return Lerp(_routePoints.RoutePoints(index0).Altitude, _routePoints.RoutePoints(index1).Altitude, t)
    End Function

    Private Function GetHeartRateAtTime(timeSeconds As Double) As Double
        If _routePoints Is Nothing OrElse _routePoints.RoutePoints.Count = 0 Then Return 0
        Dim safeTime As Double = Math.Max(0, Math.Min(timeSeconds, _routePoints.RoutePoints.Count - 1))
        Dim index0 As Integer = CInt(Math.Floor(safeTime))
        Dim index1 As Integer = Math.Min(_routePoints.RoutePoints.Count - 1, index0 + 1)
        Dim t As Double = safeTime - index0
        Return Lerp(_routePoints.RoutePoints(index0).HeartRate, _routePoints.RoutePoints(index1).HeartRate, t)
    End Function

    Private Function AltitudeToY(altitude As Double, graphTop As Single, graphHeight As Single) As Single
        Dim normalized As Double = (altitude - _minAltitude) / Math.Max(1.0, _maxAltitude - _minAltitude)
        normalized = Math.Max(0.0, Math.Min(1.0, normalized))
        Return graphTop + CSng((1.0 - normalized) * graphHeight)
    End Function

    Private Function HeartRateToY(heartRate As Double, graphTop As Single, graphHeight As Single) As Single
        Dim normalized As Double = (heartRate - _minHeartRate) / Math.Max(1.0, _maxHeartRate - _minHeartRate)
        normalized = Math.Max(0.0, Math.Min(1.0, normalized))
        Return graphTop + CSng((1.0 - normalized) * graphHeight)
    End Function

    Private Shared Function IsDanishUi() As Boolean
        Return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("da", StringComparison.OrdinalIgnoreCase)
    End Function

    Private Shared Sub EnsureFontsLoaded()
        If _fontsLoaded Then Return
        Try
            Dim appFolder As String = My.Application.Info.DirectoryPath
            Dim regularPath As String = Path.Combine(appFolder, "Roboto-Regular.ttf")
            Dim boldPath As String = Path.Combine(appFolder, "Roboto-Bold.ttf")
            If File.Exists(regularPath) Then _fontCollection.AddFontFile(regularPath)
            If File.Exists(boldPath) Then _fontCollection.AddFontFile(boldPath)
        Catch
        Finally
            _fontsLoaded = True
        End Try
    End Sub

    Private Shared Function CreateFittingFont(g As Graphics, text As String, maxWidth As Single, maxHeight As Single, style As FontStyle) As Font
        Dim family As FontFamily = If(_fontCollection.Families.Length > 0, _fontCollection.Families(0), FontFamily.GenericSansSerif)
        Dim size As Single = Math.Max(6.0F, maxHeight)
        Dim font As New Font(family, size, style, GraphicsUnit.Pixel)
        While size > 6.0F AndAlso g.MeasureString(text, font).Width > maxWidth
            font.Dispose()
            size -= 1.0F
            font = New Font(family, size, style, GraphicsUnit.Pixel)
        End While
        Return font
    End Function

    Private Shared Function RoundedRect(rect As Rectangle, radius As Integer) As GraphicsPath
        Dim path As New GraphicsPath()
        Dim safeRadius As Integer = Math.Max(0, Math.Min(radius, Math.Min(rect.Width, rect.Height) \ 2))
        If safeRadius <= 0 Then
            path.AddRectangle(rect)
            path.CloseFigure()
            Return path
        End If

        Dim diameter As Integer = safeRadius * 2
        path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90)
        path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90)
        path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90)
        path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90)
        path.CloseFigure()
        Return path
    End Function

    Private Shared Sub WriteBitmapFrameToStream(sourceImage As Bitmap, targetStream As Stream)
        Using tmpBmp As New Bitmap(sourceImage.Width, sourceImage.Height, PixelFormat.Format32bppArgb)
            Using g As Graphics = Graphics.FromImage(tmpBmp)
                g.Clear(Color.Transparent)
                g.DrawImage(sourceImage, 0, 0)
            End Using

            Dim rect As New Rectangle(0, 0, tmpBmp.Width, tmpBmp.Height)
            Dim bitmapData As BitmapData = tmpBmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb)
            Try
                Dim stride As Integer = Math.Abs(bitmapData.Stride)
                Dim rowBytes As Integer = tmpBmp.Width * 4
                Dim buffer(rowBytes * tmpBmp.Height - 1) As Byte
                For row As Integer = 0 To tmpBmp.Height - 1
                    Dim srcPtr As IntPtr = New IntPtr(bitmapData.Scan0.ToInt64() + row * stride)
                    Marshal.Copy(srcPtr, buffer, row * rowBytes, rowBytes)
                Next
                targetStream.Write(buffer, 0, buffer.Length)
            Finally
                tmpBmp.UnlockBits(bitmapData)
            End Try
        End Using
    End Sub
End Class
