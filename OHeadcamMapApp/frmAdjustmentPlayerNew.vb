Imports System.IO
Imports System.Windows
Imports System.Globalization
Imports System.Text
Imports System.Xml.Linq
Imports System.ComponentModel
Imports LibVLCSharp.Shared
Imports OHeadcamMapApp.My.Resources
Public Class frmAdjustmentPlayer_new
    Dim Leavex As Boolean
    Dim Mainform1 As MainForm
    Dim Videofile As String
    Dim setbytimer As Boolean
    Dim IsLoaded As Boolean = False
    Dim MaxLength As String
    Const AdjFile As String = "adjust.mp4"
    Const TestFile As String = "test.mp4"
    Dim PlayMode As String, PlayMode_play As String = "play", PlayMode_fast As String = "fast", PlayMode_slow As String = "slow", PlayMode_stop As String = "stop"
    Private _mp As MediaPlayer
    Private _libVLC As LibVLC
    Private _media As Media

    Private _WantedPosition As Double
    Private CurrentTrackValue, ImageTrackValue, _MapTime As Integer
    Private _LastMaptime As Integer = -1
    Private _MapTimeSmooth As Double
    Private _LastMapTimeSmooth As Double = -1
    Private _VideoFileName As String
    Private _MapImgHandler As clsMapImages
    Private _ZoomPreviewEngine As clsMapRenderEngine
    Private _LegPreviewEngine As clsMapRenderEngine
    Private _DynPreviewEngine As clsMapRenderEngine
    Private _MapDeltaTime As Integer 'Timediff to Current Time
    Private _MapReady As Boolean = False
    Private _MapInit As Boolean = False
    Private _UpdateMouse As Boolean = False
    Private _ZoomResized As Boolean = False
    Private _LegResized As Boolean = False
    Private _DynResized As Boolean = False
    Private _ZoomMouseIsUp As Boolean = True
    Private _LegMouseIsUp As Boolean = True
    Private _DynMouseIsUp As Boolean = True
    Private _ZoomMapPos As Point
    Private _LegMapPos As Point
    Private _DynMapPos As Point
    Private _ZoomZoom As Decimal
    Private _LegMargin As Integer
    Private _LoadingForm As Boolean
    Private _DeferredInitStarted As Boolean = False
    Private RPs As New clsQRRoutePoints
    Private MapImg As Bitmap
    Private VideoInfo As clsFFMPegProbe
    Private PBZoom_Adjust As New clsPictureBoxMoveResize
    Private PBLeg_Adjust As New clsPictureBoxMoveResize
    Private PBDyn_Adjust As New clsPictureBoxMoveResize
    Private PBTime_Adjust As New clsPictureBoxMoveResize
    Private PBDistance_Adjust As New clsPictureBoxMoveResize
    Private PBPace_Adjust As New clsPictureBoxMoveResize
    Private PBPulse_Adjust As New clsPictureBoxMoveResize
    Private PBHGraph_Adjust As New clsPictureBoxMoveResize
    Private _WidgetsPreviewEngine As clsDataWidgetRenderEngine
    Private _WidgetLayoutDirty As Boolean = False
    Private _WidgetMouseIsUp As Boolean = True
    Private _UpdatingWidgetTimeOffsetText As Boolean = False
    Private _VideoPanelScale As Double
    Private Const TrackBarUnitsPerSecond As Integer = 10
    Private Const PreviewTailSampleStepSeconds As Double = 0.5
    Private Const PreviewTailMinimumPointDistance As Double = 1.5
    Private WithEvents cbNewLegLayout As CheckBox
    Private ReadOnly _PortraitFrameLines As New List(Of Panel)()
    Private ReadOnly _PortraitSafeZoneLines As New List(Of Panel)()
    Private _PortraitFrameRectangle As Rectangle = Rectangle.Empty
    Private _PortraitFrameDragging As Boolean = False
    Private _PortraitFrameDragOffsetX As Integer
    Private _PortraitVideoDragging As Boolean = False
    Private _PortraitVideoDragOffsetY As Integer
    Private _PortraitVideoDragHandle As Label
    Private Const PortraitFrameLineWidth As Integer = 3
    Private Const PortraitOutputWidth As Integer = 1080
    Private Const PortraitOutputHeight As Integer = 1920
    Private _ActiveLayoutAspectIndex As Integer = 0
    Private _ApplyingOutputLayout As Boolean = False
    Private _LayoutCoordinateAspectOverride As Integer = -1
    Private ReadOnly _LayoutPresetFiles As New List(Of String)()
    Private ReadOnly _OutputResolutionValues As New List(Of Integer)()
    Private _UpdatingOutputResolutionItems As Boolean = False

    ' Required by the WinForms designer. Runtime code uses the constructor below.
    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(MainF As MainForm, iMaxLength As String)
        Dim prevLap, tmpLap As Integer
        ' This call is required by the designer.
        InitializeComponent()
        _LoadingForm = True
        ' Add any initialization after the InitializeComponent() call.
        Mainform1 = MainF
        MaxLength = iMaxLength
        Try
            If Not MaxLength.IndexOf(":") < 0 Then 'time code
                MaxLength = Mainform1.ExtraFunc.TimeStrToSec(MaxLength)
            End If
            Me.Text = Texts.AdjPlayerTitle
            Core.Initialize()
            ' Add any initialization after the InitializeComponent() call.
            _libVLC = New LibVLC()
            _mp = New MediaPlayer(_libVLC)
            VideoView1.MediaPlayer = _mp

            AddHandler _mp.EncounteredError, AddressOf MediaPlayer_EncounteredError
            Dim a As New clsQRXMLReader(RPs)
            Dim Appfolder As String = My.Application.Info.DirectoryPath + "\"


            If Not a.ReadXML(Appfolder + My.Settings.QRXML) Then Me.Close()

            Try
                MapImg = New Bitmap(Image.FromFile(My.Settings.QRimage))
            Catch e As Exception
                MsgBox(Texts.ErrQRImg + e.Message)
                Me.Close()
                Throw
            End Try

            bTrackChange = False
            prevLap = -1
            For Each lap As clsQRRoutePoint In RPs.LapPoints
                If lap.LapNumber > 1 Then

                    If prevLap = lap.LapNumber Then ' last lap - end control
                        tmpLap = lap.LapNumber ' last control has an end in same lap.
                    Else
                        tmpLap = lap.LapNumber - 1 ' controls is one less
                    End If
                    cmbJumpControl.Items.Add(CStr(tmpLap) + " " + Mainform1.ExtraFunc.SecToTimeStr(lap.ElapsedTime))
                    prevLap = lap.LapNumber
                End If
            Next
            If cmbJumpControl.Items.Count > 1 Then
                cmbJumpControl.SelectedIndex = 0
            End If
            PlayMode = PlayMode_stop
            '_VideoPanelScale = 1920 / VideoPanel.Width
            _VideoPanelScale = Mainform1.ExtraFunc.InputWidth / VideoPanel.Width ' scale
            If IsNumeric(My.Settings.GPXDiff) Then numGPSDelta.Value = CInt(My.Settings.GPXDiff)
            lblMapFlipGPSMap1.Text = Mainform1.ExtraFunc.SecToTimeStr(CInt(My.Settings.QRXMLTimesec) + numGPSDelta.Value) 'Set end of map 1 GPS time in video
            grpMapFlip.Visible = My.Settings.MapFlipOnOff
            If My.Settings.MapFlipOnOff And My.Settings.MapFlipActive Then
                lblMap2Active.Visible = True
                lblMap2Active.Text = Texts.MapFlipActive
            Else
                lblMap2Active.Visible = False

            End If
            MapImageTimer.Interval = 50
            InitializeLayoutToggle()
            FormatWidgetTimeOffsetText()

            _LoadingForm = False
        Catch ex As Exception
            'MapImageTimer.Stop()
        End Try



    End Sub
    Private Sub CheckPBposition(pb As PictureBox)
        Dim canvas As Rectangle = GetActiveCanvasRectangle()
        ' Sørg for, at PictureBox ikke går uden for venstre kant
        If pb.Left < canvas.Left Then
            pb.Left = canvas.Left
        End If

        ' Sørg for, at PictureBox ikke går uden for toppen
        If pb.Top < canvas.Top Then
            pb.Top = canvas.Top
        End If

        ' Sørg for, at PictureBox ikke går uden for højre kant
        If pb.Left + pb.Width > canvas.Right Then
            pb.Left = canvas.Right - pb.Width
        End If

        ' Sørg for, at PictureBox ikke går uden for bunden
        If pb.Top + pb.Height > canvas.Bottom Then
            pb.Top = canvas.Bottom - pb.Height
        End If
        If pb.Left < canvas.Left Then pb.Left = canvas.Left
        If pb.Top < canvas.Top Then pb.Top = canvas.Top

        If My.Settings.MIMapFrameFormIndex >= 1 AndAlso (pb Is PB_Zoom OrElse pb Is PBLeg OrElse pb Is PBDyn) Then
            Const snapDistance As Integer = 10
            Dim leftDistance As Integer = Math.Abs(pb.Left - canvas.Left)
            Dim rightDistance As Integer = Math.Abs(canvas.Right - pb.Right)
            Dim topDistance As Integer = Math.Abs(pb.Top - canvas.Top)
            Dim bottomDistance As Integer = Math.Abs(canvas.Bottom - pb.Bottom)
            Dim nearestDistance As Integer = Math.Min(Math.Min(leftDistance, rightDistance), Math.Min(topDistance, bottomDistance))

            If nearestDistance <= snapDistance Then
                If leftDistance = nearestDistance Then
                    pb.Left = canvas.Left
                ElseIf rightDistance = nearestDistance Then
                    pb.Left = canvas.Right - pb.Width
                ElseIf topDistance = nearestDistance Then
                    pb.Top = canvas.Top
                Else
                    pb.Top = canvas.Bottom - pb.Height
                End If
            End If
        End If
    End Sub
    Private Sub SetPBMapPositions(Z As Boolean, L As Boolean, ZMapP As Point, LMapP As Point, Optional D As Boolean = False, Optional DMapP As Point = Nothing) 'Real pos to PB pos
        Dim zp, lp, dp As Point
        If Z Then
            zp = LayoutPointToPanel(ZMapP)
            PB_Zoom.Location = zp
            CheckPBposition(PB_Zoom)
        End If
        If L Then
            lp = LayoutPointToPanel(LMapP)
            PBLeg.Location = lp
            CheckPBposition(PBLeg)
        End If
        If D Then
            dp = LayoutPointToPanel(DMapP)
            PBDyn.Location = dp
            CheckPBposition(PBDyn)
        End If

    End Sub
    Private Function ScaleToPanel(inValue As Integer, Optional FromReal As Boolean = True) As Double
        If FromReal Then
            Return inValue / _VideoPanelScale
        Else
            Return inValue * _VideoPanelScale
        End If



    End Function

    Private Function GetActiveCanvasRectangle() As Rectangle
        Dim aspectIndex As Integer = If(_LayoutCoordinateAspectOverride >= 0, _LayoutCoordinateAspectOverride, If(cboOutputAspect Is Nothing, 0, cboOutputAspect.SelectedIndex))
        If aspectIndex = 1 AndAlso Not _PortraitFrameRectangle.IsEmpty Then
            Return _PortraitFrameRectangle
        End If
        Return VideoPanel.ClientRectangle
    End Function

    Private Sub UpdateActiveCanvasScale()
        Dim canvas As Rectangle = GetActiveCanvasRectangle()
        Dim outputWidth As Integer = If(cboOutputAspect.SelectedIndex = 1, PortraitOutputWidth, Mainform1.ExtraFunc.InputWidth)
        _VideoPanelScale = outputWidth / CDbl(Math.Max(1, canvas.Width))
    End Sub

    Private Function LayoutPointToPanel(layoutPoint As Point) As Point
        Dim canvas As Rectangle = GetActiveCanvasRectangle()
        Return New Point(canvas.Left + CInt(Math.Round(layoutPoint.X / _VideoPanelScale)),
                         canvas.Top + CInt(Math.Round(layoutPoint.Y / _VideoPanelScale)))
    End Function

    Private Function PanelPointToLayout(panelPoint As Point) As Point
        Dim canvas As Rectangle = GetActiveCanvasRectangle()
        Return New Point(CInt(Math.Round((panelPoint.X - canvas.Left) * _VideoPanelScale)),
                         CInt(Math.Round((panelPoint.Y - canvas.Top) * _VideoPanelScale)))
    End Function


    Private Sub SetPBMapSizes()
        PB_Zoom.Width = ScaleToPanel(_MapImgHandler.ZoomWidth)
        PB_Zoom.Height = ScaleToPanel(_MapImgHandler.ZoomHeight)
        PBLeg.Width = ScaleToPanel(_MapImgHandler.LegWidth)
        PBLeg.Height = ScaleToPanel(_MapImgHandler.LegHeight)
        PBDyn.Width = ScaleToPanel(Math.Max(1, My.Settings.MIDynamicWidth))
        PBDyn.Height = ScaleToPanel(Math.Max(1, My.Settings.MIDynamicHeight))
        pbTime.Width = ScaleToPanel(Math.Max(1, My.Settings.MIWidgetTimeWidth))
        pbTime.Height = ScaleToPanel(Math.Max(1, My.Settings.MIWidgetTimeHeight))
        pbDistance.Width = ScaleToPanel(Math.Max(1, My.Settings.MIWidgetDistanceWidth))
        pbDistance.Height = ScaleToPanel(Math.Max(1, My.Settings.MIWidgetDistanceHeight))
        pbPace.Width = ScaleToPanel(Math.Max(1, My.Settings.MIWidgetPaceWidth))
        pbPace.Height = ScaleToPanel(Math.Max(1, My.Settings.MIWidgetPaceHeight))
        pbPulse.Width = ScaleToPanel(Math.Max(1, My.Settings.MIWidgetPulseWidth))
        pbPulse.Height = ScaleToPanel(Math.Max(1, My.Settings.MIWidgetPulseHeight))
        pbHGraph.Width = ScaleToPanel(Math.Max(1, My.Settings.MIWidgetHGraphWidth))
        pbHGraph.Height = ScaleToPanel(Math.Max(1, My.Settings.MIWidgetHGraphHeight))
    End Sub

    Private Sub SetWidgetPictureBoxPositions()
        SetWidgetPictureBoxPosition(pbTime, My.Settings.MIWidgetTimeMapPos)
        SetWidgetPictureBoxPosition(pbDistance, My.Settings.MIWidgetDistanceMapPos)
        SetWidgetPictureBoxPosition(pbPace, My.Settings.MIWidgetPaceMapPos)
        SetWidgetPictureBoxPosition(pbPulse, My.Settings.MIWidgetPulseMapPos)
        SetWidgetPictureBoxPosition(pbHGraph, My.Settings.MIWidgetHGraphMapPos)
    End Sub

    Private Sub SetWidgetPictureBoxPosition(pb As PictureBox, realPosition As Point)
        pb.Location = LayoutPointToPanel(realPosition)
        CheckPBposition(pb)
    End Sub

    Private Function GetRealWidgetPosition(pb As PictureBox) As Point
        Return PanelPointToLayout(pb.Location)
    End Function

    Private Function GetRealWidgetWidth(pb As PictureBox) As Integer
        Return Math.Max(1, CInt(Math.Round(pb.Width * _VideoPanelScale)))
    End Function

    Private Function GetRealWidgetHeight(pb As PictureBox) As Integer
        Return Math.Max(1, CInt(Math.Round(pb.Height * _VideoPanelScale)))
    End Function

    Private Function GetPictureBoxLayoutBounds(pb As PictureBox) As Rectangle
        Return New Rectangle(PanelPointToLayout(pb.Location), New Size(GetRealWidgetWidth(pb), GetRealWidgetHeight(pb)))
    End Function

    Private Sub SetMapPositions(Z As Boolean, L As Boolean, PBZMapP As Point, PBLMapP As Point, Optional D As Boolean = False, Optional PBDMapP As Point = Nothing) 'PB to real pos
        Dim zp, lp, dp As Point
        If Z Then
            zp = PanelPointToLayout(PBZMapP)
            _ZoomMapPos = zp
        End If
        If L Then
            lp = PanelPointToLayout(PBLMapP)
            _LegMapPos = lp
        End If
        If D Then
            dp = PanelPointToLayout(PBDMapP)
            _DynMapPos = dp
        End If
    End Sub


    Private Sub SetVideo()
        VideoInfo = New clsFFMPegProbe
        VideoInfo.StoreVideoProps(Mainform1.GetDeshakedFilename(False))
        Dim vfile As String = Mainform1.GetDeshakedFilename(False)
        If File.Exists(vfile) Then
            If Not IsNothing(_media) Then
                _media.Dispose()
                _media = Nothing
            End If
            _media = New Media(_libVLC, vfile)
        Else
            MsgBox(Texts.ErrNoFile + vfile)
            Me.Close()
            Return
        End If
        _mp.Media = _media

        VideoView1.Height = VideoPanel.Height
        VideoView1.Width = VideoInfo.Width * VideoPanel.Height / VideoInfo.Height
        Dim a As Point
        a.X = VideoPanel.Width - VideoView1.Width

        a.Y = 0
        'AxWindowsMediaPlayer1.Anchor = AnchorStyles.Right Or AnchorStyles.Top
        'AxWindowsMediaPlayer1.Location = a
        VideoView1.Anchor = AnchorStyles.Right Or AnchorStyles.Top
        VideoView1.Location = a
        ApplyVideoPreviewLayout()

        StatusLabel.Text = ""
        Timer1.Start()
        _MapInit = True
        MapImageTimer.Start()
        TrackBar1.Maximum = VideoInfo.duration_sec * TrackBarUnitsPerSecond
        TrackBar1.TickFrequency = TrackBar1.Maximum / 50
        TrackBar1.LargeChange = TrackBar1.TickFrequency
        _mp.Play()
        _mp.Pause()

        If My.Settings.MapFlipActive Then setTrackbarValue(CDbl(My.Settings.MapFlipStartS))
        'AxWindowsMediaPlayer1.Ctlcontrols.play()
        'AxWindowsMediaPlayer1.Ctlcontrols.pause()

    End Sub


    Private Sub SetPlayBtnTexts(Clicked_Button As Button)

        bFForward.Text = ">>"
        bPlay.Text = ">"
        bSlow.Text = "1/2x >"

        If Not PlayMode = PlayMode_stop Then Clicked_Button.Text = "||"


    End Sub

    Sub setTrackbarValue(time As Double)
        Dim targetValue As Integer = CInt(Math.Round(time * TrackBarUnitsPerSecond))
        If targetValue > TrackBar1.Maximum Then
            TrackBar1.Value = TrackBar1.Maximum
        Else
            TrackBar1.Value = Math.Max(TrackBar1.Minimum, targetValue)
        End If

    End Sub
    Private Function IsDanishUi() As Boolean
        Return Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("da", StringComparison.OrdinalIgnoreCase)
    End Function
    Private Sub InitializeLayoutToggle()
        If cbNewLegLayout IsNot Nothing Then Return

        cbNewLegLayout = New CheckBox()
        cbNewLegLayout.Name = "cbNewLegLayout"
        cbNewLegLayout.AutoSize = True
        cbNewLegLayout.Text = If(IsDanishUi(), "Nyt leg-layout", "New leg layout")
        cbNewLegLayout.Checked = True
        cbNewLegLayout.UseVisualStyleBackColor = True
        cbNewLegLayout.Visible = False
        cbNewLegLayout.Location = New Point(bMapSettings.Left, bMapSettings.Bottom + 6)
        Controls.Add(cbNewLegLayout)
        cbNewLegLayout.BringToFront()
    End Sub
    Private Sub RebuildPreviewRenderEngines()
        _ZoomPreviewEngine = Nothing
        _LegPreviewEngine = Nothing
        _DynPreviewEngine = Nothing
        _WidgetsPreviewEngine = Nothing
        If Mainform1 Is Nothing Then Return
        If MapImg Is Nothing Then Return

        _ZoomPreviewEngine = New clsMapRenderEngine(RPs, MapImg)
        Mainform1.ApplySavedLegRenderLayout(_ZoomPreviewEngine)
        _LegPreviewEngine = New clsMapRenderEngine(RPs, MapImg)
        Mainform1.ApplySavedLegRenderLayout(_LegPreviewEngine)

        _DynPreviewEngine = New clsMapRenderEngine(RPs, MapImg)
        Mainform1.ApplySavedLegRenderLayout(_DynPreviewEngine)

        _WidgetsPreviewEngine = New clsDataWidgetRenderEngine(RPs)
        _WidgetsPreviewEngine.WidgetTimeOffsetSeconds = My.Settings.MIWidgetTimeOffsetSeconds
    End Sub
    Private Sub cbNewLegLayout_CheckedChanged(sender As Object, e As EventArgs) Handles cbNewLegLayout.CheckedChanged
        ' REPLACED-CLASSIC-LAYOUT-20260423:
        ' The hidden checkbox is retained temporarily for compatibility, but the app
        ' now always persists the new web layout.
        cbNewLegLayout.Checked = True
        My.Settings.MILegRenderLayout = "web"
        My.Settings.Save()
        RebuildPreviewRenderEngines()
        If _MapImgHandler Is Nothing OrElse Not _MapReady Then Return
        If HasValidMapPreviewTime() Then
            UpdateMapImgs()
        End If
    End Sub
    Private Sub bSlow_Click(sender As Object, e As EventArgs) Handles bSlow.Click
        If PlayMode = PlayMode_slow Then
            _mp.Pause()

            'AxWindowsMediaPlayer1.Ctlcontrols.pause()
            'setTrackbarValue(AxWindowsMediaPlayer1.Ctlcontrols.currentPosition)
            PlayMode = PlayMode_stop
        Else
            'AxWindowsMediaPlayer1.settings.rate = 0.5
            'AxWindowsMediaPlayer1.Ctlcontrols.play()
            _mp.SetRate(0.5)
            _mp.Play()
            bTrackChange = False
            PlayMode = PlayMode_slow

        End If
        SetPlayBtnTexts(bSlow)
    End Sub

    Private Sub bFForward_Click(sender As Object, e As EventArgs) Handles bFForward.Click
        If PlayMode = PlayMode_fast Then
            _mp.Pause()
            'AxWindowsMediaPlayer1.Ctlcontrols.pause()
            'setTrackbarValue(AxWindowsMediaPlayer1.Ctlcontrols.currentPosition)
            PlayMode = PlayMode_stop
        Else
            _mp.SetRate(2)
            _mp.Play()

            'AxWindowsMediaPlayer1.settings.rate = 2
            'AxWindowsMediaPlayer1.Ctlcontrols.play()
            bTrackChange = False
            PlayMode = PlayMode_fast

        End If
        SetPlayBtnTexts(bFForward)
    End Sub

    Private Sub bPlay_Click(sender As Object, e As EventArgs) Handles bPlay.Click
        If PlayMode = PlayMode_play Then
            _mp.Pause()
            'AxWindowsMediaPlayer1.Ctlcontrols.pause()
            'setTrackbarValue(AxWindowsMediaPlayer1.Ctlcontrols.currentPosition)
            PlayMode = PlayMode_stop
        Else
            _mp.SetRate(1)
            _mp.Play()


            'AxWindowsMediaPlayer1.settings.rate = 1
            'AxWindowsMediaPlayer1.Ctlcontrols.play()
            bTrackChange = False
            PlayMode = PlayMode_play

        End If
        SetPlayBtnTexts(bPlay)
    End Sub


    Private Sub Timer1_Tick_1(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Not IsNothing(_mp.Media) Then
            If _mp.Media.Duration > 0 And Not Leavex Then
                _mp.Pause()
                Timer1.Stop()
            End If

        End If
    End Sub
    Dim bTrackChange As Boolean
    Private Sub TrackBar1_ValueChanged(sender As Object, e As EventArgs) Handles TrackBar1.ValueChanged
        Dim timeSpan As TimeSpan = TimeSpan.FromSeconds(TrackBar1.Value / CDbl(TrackBarUnitsPerSecond))
        Dim timeFormat As String = timeSpan.ToString("h\:mm\:ss")

        _WantedPosition = TrackBar1.Value / CDbl(TrackBar1.Maximum)

    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Dim a As Double
        If Not IsNothing(_mp.Media) Then
            If PlayMode = PlayMode_stop Then

                a = Math.Abs(_WantedPosition - _mp.Position)
                If a > 0.0001 Then
                    _mp.Position = _WantedPosition
                    VideoView1.Refresh()
                End If
            Else
                'Dim Position As Integer = TrackBar1.Value
                If Math.Round(_mp.Position * TrackBar1.Maximum) <> TrackBar1.Value Then
                    TrackBar1.Value = Math.Round(_mp.Position * TrackBar1.Maximum)

                End If
            End If
            Dim timeSpan As TimeSpan = TimeSpan.FromSeconds(Math.Round(_mp.Position * _mp.Media.Duration / 1000))
            Dim timeFormat As String = timeSpan.ToString("h\:mm\:ss")
            If numGPSDelta.Value < 0 Then
                timeSpan = timeSpan + TimeSpan.FromSeconds(CInt(numGPSDelta.Value))
            End If
            lblVideoPos.Text = timeSpan.ToString 'timeFormat
        End If
    End Sub

    Sub PlayerClear()
        'AxWindowsMediaPlayer1.Ctlcontrols.stop()
        'AxWindowsMediaPlayer1.URL = ""
        'AxWindowsMediaPlayer1.close()
        'AxWindowsMediaPlayer1.currentPlaylist.clear()
    End Sub
    Sub PlayerNewURL(urlstr As String)
        'AxWindowsMediaPlayer1.URL = urlstr
        'AxWindowsMediaPlayer1.Ctlcontrols.currentPosition = 1

        Timer1.Start()
    End Sub

    Private Sub bB1_Click(sender As Object, e As EventArgs) Handles bB1.Click
        If Not PlayMode = PlayMode_stop Then
            _mp.Pause()
            PlayMode = PlayMode_stop
            SetPlayBtnTexts(bPlay)
        End If
        If TrackBar1.Value > 0 Then TrackBar1.Value -= 1

    End Sub

    Private Sub bF1_Click(sender As Object, e As EventArgs) Handles bF1.Click
        If Not PlayMode = PlayMode_stop Then
            _mp.Pause()
            PlayMode = PlayMode_stop
            SetPlayBtnTexts(bPlay)
        End If
        If TrackBar1.Value < TrackBar1.Maximum Then TrackBar1.Value += 1
    End Sub

    Private Sub ResizePBsToVideoPanel()

        'PBLeg.Width = _MapImgHandler.LegMapImage.Width * _VideoPanelScale
        'PBLeg.Height = _MapImgHandler.LegMapImage.Height * _VideoPanelScale
        'PB_Zoom.Width = _MapImgHandler.ZoomMapImage.Width * _VideoPanelScale
        'PB_Zoom.Height = _MapImgHandler.ZoomMapImage.Height * _VideoPanelScale
    End Sub
    Public Sub UpdateWithSettings()
        Mainform1.bMakeMapDirty = True
        Mainform1.bOutputVideoDirty = True
        Mainform1.CheckDirtyStatus()
        _MapImgHandler.dotColor = My.Settings.MIDotColor
        _MapImgHandler.tailLineDurationSeconds = My.Settings.MITailDuration
        _MapImgHandler.tailLineColor = My.Settings.MITailColor
        _MapImgHandler.dotSize = My.Settings.MIDotSize
        _MapImgHandler.dotTailRatio = My.Settings.MITailRatio
        _MapImgHandler.FrameColor = My.Settings.MIFrameColor
        _MapImgHandler.FrameWidth = My.Settings.MIFrameWidth
        _MapImgHandler.LegRad = My.Settings.MILegRad
        _MapImgHandler.LegMargin = My.Settings.MILegMargin
        _MapImgHandler.ZoomRad = My.Settings.MIZoomRad
        _MapImgHandler.ZoomZoom = My.Settings.MIZoomZoom
        _MapImgHandler.ArrowBarb = My.Settings.MIArrowBarb
        _MapImgHandler.ArrowWidth = My.Settings.MIArrowWidth
        _MapImgHandler._DotType = My.Settings.MIDotType
        _MapImgHandler.FrameFeather = My.Settings.MIFrameFeather
        _MapImgHandler.ResetAlphaMasks()
        _MapImgHandler.InvalidateSmoothCaches()
        '20260423_B_ApplyPreviewRenderTuning()
        RebuildPreviewRenderEngines()
        _ZoomZoom = My.Settings.MIZoomZoom
        _LegMargin = My.Settings.MILegMargin

        If HasValidMapPreviewTime() Then
            _LastMapTimeSmooth = _MapTimeSmooth
            UpdateMapImgs()
        End If
    End Sub
    Private Function GetCurrentMapPreviewTime() As Double
        If IsNothing(_mp) OrElse IsNothing(_mp.Media) OrElse _mp.Media.Duration <= 0 Then Return -1
        Dim currentVideoSeconds As Double
        If _mp.Time >= 0 Then
            currentVideoSeconds = _mp.Time / 1000.0
        Else
            currentVideoSeconds = _mp.Position * _mp.Media.Duration / 1000.0
        End If
        Dim mapTime As Double = currentVideoSeconds + CDbl(numGPSDelta.Value)

        If My.Settings.MapFlipActive Then
            mapTime -= CDbl(My.Settings.MapFlipStartS)
        End If

        mapTime += _MapDeltaTime
        Return mapTime
    End Function

    Private Function GetCurrentWidgetReferenceTime() As Double
        Dim previewTime As Double = GetCurrentMapPreviewTime()
        If previewTime >= 0 Then Return previewTime
        If IsNothing(_mp) OrElse IsNothing(_mp.Media) OrElse _mp.Media.Duration <= 0 Then Return 0
        If _mp.Time >= 0 Then Return _mp.Time / 1000.0
        Return _mp.Position * _mp.Media.Duration / 1000.0
    End Function

    Private Shared Function FormatSignedWidgetOffset(seconds As Double) As String
        Dim roundedSeconds As Integer = CInt(Math.Round(seconds))
        Dim sign As String = If(roundedSeconds < 0, "-", "+")
        Dim absSeconds As Integer = Math.Abs(roundedSeconds)
        Dim ts As TimeSpan = TimeSpan.FromSeconds(absSeconds)
        Dim timeText As String
        If ts.TotalHours >= 1 Then
            timeText = String.Format(Globalization.CultureInfo.InvariantCulture, "{0}:{1:00}:{2:00}", CInt(Math.Floor(ts.TotalHours)), ts.Minutes, ts.Seconds)
        Else
            timeText = String.Format(Globalization.CultureInfo.InvariantCulture, "{0}:{1:00}", ts.Minutes, ts.Seconds)
        End If
        Return sign & timeText
    End Function

    Private Shared Function TryParseWidgetOffsetText(text As String, ByRef seconds As Double) As Boolean
        Dim value As String = If(text, "").Trim()
        If value = "" Then
            seconds = 0
            Return True
        End If

        Dim sign As Double = 1
        If value.StartsWith("+", StringComparison.Ordinal) Then
            value = value.Substring(1).Trim()
        ElseIf value.StartsWith("-", StringComparison.Ordinal) Then
            sign = -1
            value = value.Substring(1).Trim()
        End If

        If value = "" Then
            seconds = 0
            Return True
        End If

        Dim parts() As String = value.Split(":"c)
        Dim parsedParts As New List(Of Integer)
        For Each part As String In parts
            Dim parsed As Integer
            If Not Integer.TryParse(part.Trim(), parsed) OrElse parsed < 0 Then Return False
            parsedParts.Add(parsed)
        Next

        Select Case parsedParts.Count
            Case 1
                seconds = sign * parsedParts(0)
            Case 2
                seconds = sign * (parsedParts(0) * 60 + parsedParts(1))
            Case 3
                seconds = sign * (parsedParts(0) * 3600 + parsedParts(1) * 60 + parsedParts(2))
            Case Else
                Return False
        End Select

        Return True
    End Function

    Private Sub FormatWidgetTimeOffsetText()
        _UpdatingWidgetTimeOffsetText = True
        Try
            txtWdgTimeOffset.Text = FormatSignedWidgetOffset(My.Settings.MIWidgetTimeOffsetSeconds)
        Finally
            _UpdatingWidgetTimeOffsetText = False
        End Try
    End Sub

    Private Sub ApplyWidgetTimeOffset(seconds As Double)
        My.Settings.MIWidgetTimeOffsetSeconds = seconds
        My.Settings.Save()
        If _WidgetsPreviewEngine IsNot Nothing Then _WidgetsPreviewEngine.WidgetTimeOffsetSeconds = seconds
        FormatWidgetTimeOffsetText()
        If HasValidMapPreviewTime() Then UpdateWidgetPreviewImages()
    End Sub

    Private Sub CommitWidgetTimeOffsetText()
        If _UpdatingWidgetTimeOffsetText Then Return
        Dim seconds As Double
        If TryParseWidgetOffsetText(txtWdgTimeOffset.Text, seconds) Then
            ApplyWidgetTimeOffset(seconds)
        Else
            FormatWidgetTimeOffsetText()
        End If
    End Sub

    Private Sub txtWdgTimeOffset_Leave(sender As Object, e As EventArgs) Handles txtWdgTimeOffset.Leave
        CommitWidgetTimeOffsetText()
    End Sub

    Private Sub txtWdgTimeOffset_KeyDown(sender As Object, e As KeyEventArgs) Handles txtWdgTimeOffset.KeyDown
        If e.KeyCode <> Keys.Enter Then Return
        CommitWidgetTimeOffsetText()
        e.SuppressKeyPress = True
    End Sub

    Private Sub btnWdgTimeMinus_Click(sender As Object, e As EventArgs) Handles btnWdgTimeMinus.Click
        CommitWidgetTimeOffsetText()
        ApplyWidgetTimeOffset(My.Settings.MIWidgetTimeOffsetSeconds - 1)
    End Sub

    Private Sub btnWdgTimePlus_Click(sender As Object, e As EventArgs) Handles btnWdgTimePlus.Click
        CommitWidgetTimeOffsetText()
        ApplyWidgetTimeOffset(My.Settings.MIWidgetTimeOffsetSeconds + 1)
    End Sub

    Private Sub btnWdgTime0_Click(sender As Object, e As EventArgs) Handles btnWdgTime0.Click
        ApplyWidgetTimeOffset(-GetCurrentWidgetReferenceTime())
    End Sub

    Private Function HasValidMapPreviewTime() As Boolean
        Return _MapTimeSmooth > -1 AndAlso _MapTimeSmooth < RPs.RoutePoints.Count
    End Function
    'Private Sub UpdatePreviewBaseMap()
    '20260423_MapImgHandler.DrawPositionOnBackgroundImageSmooth(_MapTimeSmooth)
    '_LastMapTimeSmooth = _MapTimeSmooth
    'End Sub
    Public Sub UpdateMapImgs()
        Dim w, h, lw, lh, dw, dh As Integer
        Dim c As Integer

        w = PB_Zoom.Width * _VideoPanelScale
        h = PB_Zoom.Height * _VideoPanelScale
        Dim rad As Integer
        If My.Settings.MIZoomCircle Then
            c = Math.Min(w, h) ' circle rounding
            rad = c / 2 - 5
        Else
            rad = My.Settings.MIZoomRad
        End If
        _MapImgHandler.ZoomRad = rad
        _MapImgHandler.ZoomHeight = h
        _MapImgHandler.ZoomWidth = w
        lw = PBLeg.Width * _VideoPanelScale
        lh = PBLeg.Height * _VideoPanelScale
        _MapImgHandler.LegRad = My.Settings.MILegRad
        _MapImgHandler.LegWidth = lw
        _MapImgHandler.LegHeight = lh
        dw = PBDyn.Width * _VideoPanelScale
        dh = PBDyn.Height * _VideoPanelScale
        _MapImgHandler.ResetAlphaMasks()
        If _ZoomPreviewEngine Is Nothing OrElse _LegPreviewEngine Is Nothing OrElse _DynPreviewEngine Is Nothing Then RebuildPreviewRenderEngines()
        If _ZoomPreviewEngine IsNot Nothing Then
            _ZoomPreviewEngine.FrameAttachedEdge = clsOutputLayoutResolver.GetFrameAttachedEdge(GetPictureBoxLayoutBounds(PB_Zoom), New Size(If(cboOutputAspect.SelectedIndex = 1, PortraitOutputWidth, Mainform1.ExtraFunc.InputWidth), If(cboOutputAspect.SelectedIndex = 1, PortraitOutputHeight, Mainform1.ExtraFunc.InputHeight)))
            _ZoomPreviewEngine.ZoomWidth = w
            _ZoomPreviewEngine.ZoomHeight = h
            _ZoomPreviewEngine.ZoomRadius = rad
            _ZoomPreviewEngine.ZoomFactor = My.Settings.MIZoomZoom
            PB_Zoom.Image = _ZoomPreviewEngine.RenderZoomFramePerf8(_MapTimeSmooth)
        End If
        If _LegPreviewEngine IsNot Nothing Then
            _LegPreviewEngine.FrameAttachedEdge = clsOutputLayoutResolver.GetFrameAttachedEdge(GetPictureBoxLayoutBounds(PBLeg), New Size(If(cboOutputAspect.SelectedIndex = 1, PortraitOutputWidth, Mainform1.ExtraFunc.InputWidth), If(cboOutputAspect.SelectedIndex = 1, PortraitOutputHeight, Mainform1.ExtraFunc.InputHeight)))
            _LegPreviewEngine.LegWidth = lw
            _LegPreviewEngine.LegHeight = lh
            _LegPreviewEngine.LegMargin = My.Settings.MILegMargin
            PBLeg.Image = _LegPreviewEngine.RenderLegFramePerf8(_MapTimeSmooth)
        End If
        If _DynPreviewEngine IsNot Nothing Then
            _DynPreviewEngine.FrameAttachedEdge = clsOutputLayoutResolver.GetFrameAttachedEdge(GetPictureBoxLayoutBounds(PBDyn), New Size(If(cboOutputAspect.SelectedIndex = 1, PortraitOutputWidth, Mainform1.ExtraFunc.InputWidth), If(cboOutputAspect.SelectedIndex = 1, PortraitOutputHeight, Mainform1.ExtraFunc.InputHeight)))
            _DynPreviewEngine.DynamicWidth = dw
            _DynPreviewEngine.DynamicHeight = dh
            _DynPreviewEngine.DynamicRadius = Math.Max(0, My.Settings.MIDynamicRad)
            _DynPreviewEngine.DynamicZoomFactor = Math.Max(0.001, My.Settings.MIDynamicZoom)
            _DynPreviewEngine.DynamicLookBehindSeconds = Math.Max(0, My.Settings.MIDynamicLookBehindSeconds)
            _DynPreviewEngine.DynamicLookAheadSeconds = Math.Max(0, My.Settings.MIDynamicLookAheadSeconds)
            _DynPreviewEngine.DynamicMargin = Math.Max(0, My.Settings.MIDynamicMargin)
            PBDyn.Image = _DynPreviewEngine.RenderDynamicFramePerf8(_MapTimeSmooth)
        End If
        UpdateWidgetPreviewImages()
    End Sub

    Private Sub UpdateWidgetPreviewImages()
        If _WidgetsPreviewEngine Is Nothing Then _WidgetsPreviewEngine = New clsDataWidgetRenderEngine(RPs)
        _WidgetsPreviewEngine.WidgetTimeOffsetSeconds = My.Settings.MIWidgetTimeOffsetSeconds
        RenderWidgetPreview(pbTime, DataWidgetKind.Time, My.Settings.MIWidgetTimeEnabled)
        RenderWidgetPreview(pbDistance, DataWidgetKind.Distance, My.Settings.MIWidgetDistanceEnabled)
        RenderWidgetPreview(pbPace, DataWidgetKind.Pace, My.Settings.MIWidgetPaceEnabled)
        RenderWidgetPreview(pbPulse, DataWidgetKind.Pulse, My.Settings.MIWidgetPulseEnabled)
        RenderWidgetPreview(pbHGraph, DataWidgetKind.HGraph, My.Settings.MIWidgetHGraphEnabled)
    End Sub

    Private Sub RenderWidgetPreview(pb As PictureBox, kind As DataWidgetKind, enabled As Boolean)
        pb.Visible = enabled
        If Not enabled Then
            If pb.Image IsNot Nothing Then
                pb.Image.Dispose()
                pb.Image = Nothing
            End If
            Return
        End If

        Dim oldImage As Image = pb.Image
        pb.Image = _WidgetsPreviewEngine.RenderWidget(kind, _MapTimeSmooth, Math.Max(30, pb.Width), Math.Max(30, pb.Height))
        If oldImage IsNot Nothing Then oldImage.Dispose()
    End Sub

    Private Sub MapImageTimer_Tick(sender As Object, e As EventArgs) Handles MapImageTimer.Tick
        Dim tmpCurrentTrackValue As Integer
        Dim previewTime As Double = GetCurrentMapPreviewTime()
        CurrentTrackValue = CInt(Math.Round(previewTime))
        tmpCurrentTrackValue = CurrentTrackValue

        If _MapReady AndAlso previewTime > -1 AndAlso previewTime < RPs.RoutePoints.Count Then
            _MapTimeSmooth = previewTime
            If Math.Abs(previewTime - _LastMapTimeSmooth) >= 0.02 OrElse _MapInit Then
                _MapInit = False
                _LastMapTimeSmooth = _MapTimeSmooth
                UpdateMapImgs()
            ElseIf (_ZoomResized And _ZoomMouseIsUp) OrElse (_LegResized And _LegMouseIsUp) OrElse (_DynResized And _DynMouseIsUp) OrElse (_WidgetLayoutDirty And _WidgetMouseIsUp) Then
                UpdateMapImgs()
                _ZoomResized = False
                _LegResized = False
                _DynResized = False
                _WidgetLayoutDirty = False
            ElseIf _ZoomZoom <> My.Settings.MIZoomZoom Then
                My.Settings.MIZoomZoom = _ZoomZoom
                _MapImgHandler.ZoomZoom = My.Settings.MIZoomZoom
                UpdateMapImgs()
            ElseIf _LegMargin <> My.Settings.MILegMargin Then
                My.Settings.MILegMargin = _LegMargin
                _MapImgHandler.LegMargin = My.Settings.MILegMargin
                UpdateMapImgs()
            End If
            Return
        End If

        If _MapReady And _ZoomZoom <> My.Settings.MIZoomZoom Then
            If HasValidMapPreviewTime() Then
                My.Settings.MIZoomZoom = _ZoomZoom
                _MapImgHandler.ZoomZoom = My.Settings.MIZoomZoom
                UpdateMapImgs()
            End If
        ElseIf _MapReady And _LegMargin <> My.Settings.MILegMargin Then
            If HasValidMapPreviewTime() Then
                My.Settings.MILegMargin = _LegMargin
                _MapImgHandler.LegMargin = My.Settings.MILegMargin
                UpdateMapImgs()
            End If
        End If
    End Sub

    Private Sub PB_Zoom_MouseDown(sender As Object, e As MouseEventArgs) Handles PB_Zoom.MouseDown
        PBZoom_Adjust.PB_MouseDown(sender, e)

        _ZoomMouseIsUp = False
    End Sub

    Private Sub PB_Zoom_MouseUp(sender As Object, e As MouseEventArgs) Handles PB_Zoom.MouseUp
        PBZoom_Adjust.PB_MouseUp(sender, e)
        CheckPBposition(PB_Zoom)
        _ZoomMouseIsUp = True

    End Sub

    Private Sub PB_Zoom_MouseMove(sender As Object, e As MouseEventArgs) Handles PB_Zoom.MouseMove
        PBZoom_Adjust.PB_MouseMove(sender, e)
        UpdateMapFlatEdgeDuringDrag(PB_Zoom, _ZoomPreviewEngine)
        'If PBZoom_Adjust.IsResizing Then
        'PB_Zoom.BorderStyle = BorderStyle.FixedSingle
        'Else
        'PB_Zoom.BorderStyle = BorderStyle.None
        'End If
    End Sub

    Private Sub PB_Zoom_SizeChanged(sender As Object, e As EventArgs) Handles PB_Zoom.SizeChanged
        'ResizeUpdate.Start()
        _ZoomResized = True
    End Sub

    Private Sub PBLeg_MouseDown(sender As Object, e As MouseEventArgs) Handles PBLeg.MouseDown
        PBLeg_Adjust.PB_MouseDown(sender, e)
        _LegMouseIsUp = False
    End Sub

    Private Sub PBLeg_MouseUp(sender As Object, e As MouseEventArgs) Handles PBLeg.MouseUp
        PBLeg_Adjust.PB_MouseUp(sender, e)
        CheckPBposition(PBLeg)
        _LegMouseIsUp = True
    End Sub

    Private Sub PBLeg_MouseMove(sender As Object, e As MouseEventArgs) Handles PBLeg.MouseMove
        PBLeg_Adjust.PB_MouseMove(sender, e)
        UpdateMapFlatEdgeDuringDrag(PBLeg, _LegPreviewEngine)
    End Sub

    Private Sub PBLeg_SizeChanged(sender As Object, e As EventArgs) Handles PBLeg.SizeChanged
        _LegResized = True
    End Sub

    Private Sub PBDyn_MouseDown(sender As Object, e As MouseEventArgs) Handles PBDyn.MouseDown
        PBDyn_Adjust.PB_MouseDown(sender, e)
        _DynMouseIsUp = False
    End Sub

    Private Sub PBDyn_MouseUp(sender As Object, e As MouseEventArgs) Handles PBDyn.MouseUp
        PBDyn_Adjust.PB_MouseUp(sender, e)
        CheckPBposition(PBDyn)
        _DynMouseIsUp = True
    End Sub

    Private Sub PBDyn_MouseMove(sender As Object, e As MouseEventArgs) Handles PBDyn.MouseMove
        PBDyn_Adjust.PB_MouseMove(sender, e)
        UpdateMapFlatEdgeDuringDrag(PBDyn, _DynPreviewEngine)
    End Sub

    Private Sub UpdateMapFlatEdgeDuringDrag(pb As PictureBox, engine As clsMapRenderEngine)
        If engine Is Nothing OrElse My.Settings.MIMapFrameFormIndex < 1 Then Return

        CheckPBposition(pb)
        Dim outputSize As New Size(If(cboOutputAspect.SelectedIndex = 1, PortraitOutputWidth, Mainform1.ExtraFunc.InputWidth),
                                   If(cboOutputAspect.SelectedIndex = 1, PortraitOutputHeight, Mainform1.ExtraFunc.InputHeight))
        Dim newEdge As MapFrameAttachedEdge = clsOutputLayoutResolver.GetFrameAttachedEdge(GetPictureBoxLayoutBounds(pb), outputSize)
        If engine.FrameAttachedEdge = newEdge Then Return

        engine.FrameAttachedEdge = newEdge
        UpdateMapImgs()
    End Sub

    Private Sub PBDyn_SizeChanged(sender As Object, e As EventArgs) Handles PBDyn.SizeChanged
        _DynResized = True
    End Sub

    Private Sub DataWidget_MouseDown(sender As Object, e As MouseEventArgs) Handles pbTime.MouseDown, pbDistance.MouseDown, pbPace.MouseDown, pbPulse.MouseDown, pbHGraph.MouseDown
        GetWidgetAdjuster(DirectCast(sender, PictureBox)).PB_MouseDown(sender, e)
        _WidgetMouseIsUp = False
    End Sub

    Private Sub DataWidget_MouseUp(sender As Object, e As MouseEventArgs) Handles pbTime.MouseUp, pbDistance.MouseUp, pbPace.MouseUp, pbPulse.MouseUp, pbHGraph.MouseUp
        Dim widget As PictureBox = DirectCast(sender, PictureBox)
        GetWidgetAdjuster(widget).PB_MouseUp(sender, e)
        CheckPBposition(widget)
        _WidgetMouseIsUp = True
        _WidgetLayoutDirty = True
    End Sub

    Private Sub DataWidget_MouseMove(sender As Object, e As MouseEventArgs) Handles pbTime.MouseMove, pbDistance.MouseMove, pbPace.MouseMove, pbPulse.MouseMove, pbHGraph.MouseMove
        GetWidgetAdjuster(DirectCast(sender, PictureBox)).PB_MouseMove(sender, e)
    End Sub

    Private Sub DataWidget_SizeChanged(sender As Object, e As EventArgs) Handles pbTime.SizeChanged, pbDistance.SizeChanged, pbPace.SizeChanged, pbPulse.SizeChanged, pbHGraph.SizeChanged
        _WidgetLayoutDirty = True
    End Sub

    Private Function GetWidgetAdjuster(pb As PictureBox) As clsPictureBoxMoveResize
        If pb Is pbTime Then Return PBTime_Adjust
        If pb Is pbDistance Then Return PBDistance_Adjust
        If pb Is pbPace Then Return PBPace_Adjust
        If pb Is pbHGraph Then Return PBHGraph_Adjust
        Return PBPulse_Adjust
    End Function

    Private Sub TestMap_Click(sender As Object, e As EventArgs)



    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        VideoView1.MediaPlayer = Nothing
        VideoView1.MediaPlayer.ToggleFullscreen()
    End Sub
    Public Function retrat() As Decimal
        Return My.Settings.MITailRatio
    End Function
    Private Sub ApplyPreviewRenderTuning()
        If IsNothing(_MapImgHandler) Then Return
        _MapImgHandler.ConfigureSmoothPreview(tailSampleStepSeconds:=PreviewTailSampleStepSeconds,
                                              tailMinimumPointDistance:=PreviewTailMinimumPointDistance)
    End Sub
    Private Sub OpenMapSettingsDialog()
        Dim fMapSet As frmMapSettings = New frmMapSettings(Me)
        If Not PlayMode = PlayMode_stop Then
            _mp.Pause()
            PlayMode = PlayMode_stop
        End If
        fMapSet.ShowDialog()
        _MapImgHandler.LoadSettings()
        '20260423_B_ApplyPreviewRenderTuning()
    End Sub
    Private Sub bMapSettings_Click(sender As Object, e As EventArgs) Handles bMapSettings.Click
        OpenMapSettingsDialog()
    End Sub

    Private Sub btnWidgets_Click(sender As Object, e As EventArgs) Handles btnWidgets.Click
        If Not PlayMode = PlayMode_stop Then
            _mp.Pause()
            PlayMode = PlayMode_stop
        End If

        Using fWidgets As New frmWidgets()
            fWidgets.ShowDialog(Me)
        End Using
        ApplyWidgetVisibility()
        If HasValidMapPreviewTime() Then UpdateWidgetPreviewImages()
    End Sub

    Private Sub ApplyWidgetVisibility()
        pbTime.Visible = My.Settings.MIWidgetTimeEnabled
        pbDistance.Visible = My.Settings.MIWidgetDistanceEnabled
        pbPace.Visible = My.Settings.MIWidgetPaceEnabled
        pbPulse.Visible = My.Settings.MIWidgetPulseEnabled
        pbHGraph.Visible = My.Settings.MIWidgetHGraphEnabled
    End Sub

    Private Sub SetShowMaps()
        If IsLoaded Then
            My.Settings.cbShowLegMAp = cbLeg.Checked
            My.Settings.cbShowRoute = cbZoom.Checked
            My.Settings.cbShowDynamicMap = cbDyn.Checked
            PBLeg.Visible = cbLeg.Checked
            PB_Zoom.Visible = cbZoom.Checked
            PBDyn.Visible = cbDyn.Checked
        End If
    End Sub
    Private Sub cbZoom_CheckedChanged(sender As Object, e As EventArgs) Handles cbZoom.CheckedChanged
        SetShowMaps()
    End Sub

    Private Sub cbLeg_CheckedChanged(sender As Object, e As EventArgs) Handles cbLeg.CheckedChanged
        SetShowMaps()
    End Sub

    Private Sub cbDyn_CheckedChanged(sender As Object, e As EventArgs) Handles cbDyn.CheckedChanged
        SetShowMaps()
    End Sub

    Dim bMouseDown As Boolean = False

    Private Sub Zoom_MouseWheel(sender As Object, e As MouseEventArgs)
        If e.Delta > 0 Then
            _ZoomZoom += 0.1 ' Scroll up
        Else
            _ZoomZoom -= 0.1 ' Scroll down
            If _ZoomZoom < 0.1 Then _ZoomZoom = 0.1
        End If
    End Sub

    Private Sub PB_Zoom_MouseEnter(sender As Object, e As EventArgs) Handles PB_Zoom.MouseEnter
        AddHandler Me.MouseWheel, AddressOf Zoom_MouseWheel
    End Sub

    Private Sub PB_Zoom_MouseLeave(sender As Object, e As EventArgs) Handles PB_Zoom.MouseLeave
        RemoveHandler Me.MouseWheel, AddressOf Zoom_MouseWheel
    End Sub

    Private Sub Leg_MouseWheel(sender As Object, e As MouseEventArgs)
        Dim change As Integer = e.Delta \ 60
        If change = 0 Then Return
        _LegMargin = Math.Max(0, _LegMargin + change)
        My.Settings.MILegMargin = _LegMargin
        If _MapImgHandler IsNot Nothing Then _MapImgHandler.LegMargin = _LegMargin
        If _LegPreviewEngine IsNot Nothing Then _LegPreviewEngine.LegMargin = _LegMargin
        If HasValidMapPreviewTime() Then UpdateMapImgs()
    End Sub

    Private Sub PBLeg_MouseEnter(sender As Object, e As EventArgs) Handles PBLeg.MouseEnter
        AddHandler Me.MouseWheel, AddressOf Leg_MouseWheel
    End Sub

    Private Sub PBLeg_MouseLeave(sender As Object, e As EventArgs) Handles PBLeg.MouseLeave
        RemoveHandler Me.MouseWheel, AddressOf Leg_MouseWheel
    End Sub

    Private Sub Dynamic_MouseWheel(sender As Object, e As MouseEventArgs)
        Dim change As Integer = Math.Sign(e.Delta) * 15
        If change = 0 Then Return
        My.Settings.MIDynamicMargin = Math.Max(0, My.Settings.MIDynamicMargin + change)
        If _DynPreviewEngine IsNot Nothing Then _DynPreviewEngine.DynamicMargin = My.Settings.MIDynamicMargin
        If HasValidMapPreviewTime() Then UpdateMapImgs()
    End Sub

    Private Sub PBDyn_MouseEnter(sender As Object, e As EventArgs) Handles PBDyn.MouseEnter
        AddHandler Me.MouseWheel, AddressOf Dynamic_MouseWheel
    End Sub

    Private Sub PBDyn_MouseLeave(sender As Object, e As EventArgs) Handles PBDyn.MouseLeave
        RemoveHandler Me.MouseWheel, AddressOf Dynamic_MouseWheel
    End Sub

    Private Sub btnSetMapFlipTime_Click(sender As Object, e As EventArgs) Handles btnSetMapFlipTime.Click
        Dim CurrentVideoPos As Single
        CurrentVideoPos = Math.Round(_mp.Position * _mp.Media.Duration / 1000) 'seconds
        My.Settings.MapFlipStartS = CStr(CurrentVideoPos)
        My.Settings.MapFlipStartT = Mainform1.ExtraFunc.SecToTimeStr(CurrentVideoPos)
        Mainform1.txtOutputLength.Text = My.Settings.MapFlipStartT 'set to cut video for map 1
        If String.IsNullOrWhiteSpace(My.Settings.MapFlipStartT) Or My.Settings.MapFlipStartT = "0:00:00" Or My.Settings.MapFlipActive Then
            Mainform1.txtOutputLength.Text = ""
            Mainform1.txtOutputLength.BackColor = SystemColors.Window

        Else
            Mainform1.txtOutputLength.BackColor = Color.LightPink
        End If

    End Sub

    Private Sub numGPSDelta_ValueChanged(sender As Object, e As EventArgs) Handles numGPSDelta.ValueChanged

        lblMapFlipGPSMap1.Text = Mainform1.ExtraFunc.SecToTimeStr(CInt(My.Settings.QRXMLTimesec) - numGPSDelta.Value) 'Set end of map 1 GPS time in video
        Application.DoEvents()
    End Sub

    Private Sub btnMoveS2_Click(sender As Object, e As EventArgs) Handles btnMoveS2.Click
        setTrackbarValue(CDbl(My.Settings.MapFlipStartS))
    End Sub

    Private Sub btnMoveS1_Click(sender As Object, e As EventArgs) Handles btnMoveS1.Click

        setTrackbarValue(CDbl(Mainform1.ExtraFunc.TimeStrToSec(lblMapFlipGPSMap1.Text)))
    End Sub

    Private Sub cmbJumpControl_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbJumpControl.SelectedIndexChanged
        Dim timeinsec As Double
        Dim Txt As String
        If Not _LoadingForm Then ' avoid update problem
            Txt = cmbJumpControl.Items(cmbJumpControl.SelectedIndex)

            Dim parts() As String = Txt.Split(" "c) ' get the timecode part

            ' Get the number before the time code
            Dim Laptime As String = parts(1)
            If Laptime.Contains(":") Then
                timeinsec = Mainform1.ExtraFunc.TimeStrToSec(Laptime)
                If My.Settings.MapFlipActive And IsNumeric(My.Settings.MapFlipStartS) Then
                    timeinsec += CInt(My.Settings.MapFlipStartS)
                End If
                If numGPSDelta.Value < 0 Then
                    timeinsec -= CInt(numGPSDelta.Value) ' Adding The negative offset to the time (--=+, pos label is set 0 at neg offset)
                End If
                setTrackbarValue(timeinsec)
            End If
        End If
    End Sub



    Private Sub PB_Zoom_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles PB_Zoom.MouseDoubleClick
        OpenMapSettingsDialog()
    End Sub

    Private Sub PBLeg_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles PBLeg.MouseDoubleClick
        OpenMapSettingsDialog()
    End Sub

    Private Sub PBDyn_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles PBDyn.MouseDoubleClick
        OpenMapSettingsDialog()
    End Sub

    Private Sub TrackBar1_MouseDown(sender As Object, e As MouseEventArgs) Handles TrackBar1.MouseDown
        bMouseDown = True
        If Not PlayMode = PlayMode_stop Then
            _mp.Pause()
            PlayMode = PlayMode_stop
            SetPlayBtnTexts(bPlay)
        End If
    End Sub

    Private Sub TrackBar1_MouseUp(sender As Object, e As MouseEventArgs) Handles TrackBar1.MouseUp
        bMouseDown = False

        bTrackChange = True
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        bTrackChange = True
        'lblVideoPos.Text = TrackBar1.Value
    End Sub

    Private Sub btnTransfer_Click(sender As Object, e As EventArgs) Handles btnTransfer.Click
        'Mainform1.LblGPSDiff.Text = txtGPSDiff.Text
        SaveActiveOverlayLayout()
        My.Settings.GPXDiff = CStr(numGPSDelta.Value)
        My.Settings.Save()
        Me.Close()
    End Sub

    Private Function GetLayoutControls() As Dictionary(Of String, PictureBox)
        Return New Dictionary(Of String, PictureBox)(StringComparer.OrdinalIgnoreCase) From {
            {"Z", PB_Zoom}, {"L", PBLeg}, {"D", PBDyn},
            {"T", pbTime}, {"DI", pbDistance}, {"P", pbPace}, {"PU", pbPulse}, {"H", pbHGraph}
        }
    End Function

    Private Sub SaveActiveOverlayLayout()
        If Not _MapReady OrElse _VideoPanelScale <= 0 Then Return
        _LayoutCoordinateAspectOverride = _ActiveLayoutAspectIndex
        Try
            If _ActiveLayoutAspectIndex = 1 Then
                My.Settings.PortraitOverlayLayout = SerializeCurrentOverlayLayout()
                My.Settings.PortraitOverlayVisibility = SerializeCurrentOverlayVisibility()
                CommitWidgetTimeOffsetText()
            Else
                _MapImgHandler.SaveSettings()
                SetMapPositions(True, True, PB_Zoom.Location, PBLeg.Location, True, PBDyn.Location)
                My.Settings.MIZoomMapPos = _ZoomMapPos
                My.Settings.MILegMapPos = _LegMapPos
                My.Settings.MIDynamicMapPos = _DynMapPos
                My.Settings.MIDynamicWidth = GetRealWidgetWidth(PBDyn)
                My.Settings.MIDynamicHeight = GetRealWidgetHeight(PBDyn)
                SaveWidgetLayoutSettings()
                My.Settings.LandscapeOverlayVisibility = SerializeCurrentOverlayVisibility()
            End If
        Finally
            _LayoutCoordinateAspectOverride = -1
        End Try
    End Sub

    Private Function SerializeCurrentOverlayVisibility() As String
        Return $"Z={If(cbZoom.Checked, 1, 0)}|L={If(cbLeg.Checked, 1, 0)}|D={If(cbDyn.Checked, 1, 0)}|T={If(My.Settings.MIWidgetTimeEnabled, 1, 0)}|DI={If(My.Settings.MIWidgetDistanceEnabled, 1, 0)}|P={If(My.Settings.MIWidgetPaceEnabled, 1, 0)}|PU={If(My.Settings.MIWidgetPulseEnabled, 1, 0)}|H={If(My.Settings.MIWidgetHGraphEnabled, 1, 0)}"
    End Function

    Private Sub ApplyOverlayVisibility(serialized As String)
        If String.IsNullOrWhiteSpace(serialized) Then Return
        Dim values As New Dictionary(Of String, Boolean)(StringComparer.OrdinalIgnoreCase)
        For Each part As String In serialized.Split("|"c)
            Dim pair() As String = part.Split("="c)
            If pair.Length = 2 Then values(pair(0)) = (pair(1) = "1")
        Next
        If values.ContainsKey("Z") Then My.Settings.cbShowRoute = values("Z")
        If values.ContainsKey("L") Then My.Settings.cbShowLegMAp = values("L")
        If values.ContainsKey("D") Then My.Settings.cbShowDynamicMap = values("D")
        If values.ContainsKey("T") Then My.Settings.MIWidgetTimeEnabled = values("T")
        If values.ContainsKey("DI") Then My.Settings.MIWidgetDistanceEnabled = values("DI")
        If values.ContainsKey("P") Then My.Settings.MIWidgetPaceEnabled = values("P")
        If values.ContainsKey("PU") Then My.Settings.MIWidgetPulseEnabled = values("PU")
        If values.ContainsKey("H") Then My.Settings.MIWidgetHGraphEnabled = values("H")
        cbZoom.Checked = My.Settings.cbShowRoute
        cbLeg.Checked = My.Settings.cbShowLegMAp
        cbDyn.Checked = My.Settings.cbShowDynamicMap
        ApplyWidgetVisibility()
        SetShowMaps()
    End Sub

    Private Function SerializeCurrentOverlayLayout() As String
        Dim result As New StringBuilder("1")
        For Each item In GetLayoutControls()
            Dim pb As PictureBox = item.Value
            Dim position As Point = PanelPointToLayout(pb.Location)
            Dim width As Integer = GetRealWidgetWidth(pb)
            Dim height As Integer = GetRealWidgetHeight(pb)
            result.Append("|").Append(item.Key).Append("=")
            result.Append(position.X.ToString(CultureInfo.InvariantCulture)).Append(",")
            result.Append(position.Y.ToString(CultureInfo.InvariantCulture)).Append(",")
            result.Append(width.ToString(CultureInfo.InvariantCulture)).Append(",")
            result.Append(height.ToString(CultureInfo.InvariantCulture))
        Next
        Return result.ToString()
    End Function

    Private Function TryApplyPortraitOverlayLayout(serialized As String) As Boolean
        If String.IsNullOrWhiteSpace(serialized) Then Return False
        Dim controls = GetLayoutControls()
        Dim applied As Boolean = False
        For Each section As String In serialized.Split("|"c)
            Dim equalsIndex As Integer = section.IndexOf("="c)
            If equalsIndex <= 0 Then Continue For
            Dim key As String = section.Substring(0, equalsIndex)
            Dim pb As PictureBox = Nothing
            If Not controls.TryGetValue(key, pb) Then Continue For
            Dim values() As String = section.Substring(equalsIndex + 1).Split(","c)
            If values.Length <> 4 Then Continue For
            Dim x, y, width, height As Integer
            If Not Integer.TryParse(values(0), NumberStyles.Integer, CultureInfo.InvariantCulture, x) OrElse
               Not Integer.TryParse(values(1), NumberStyles.Integer, CultureInfo.InvariantCulture, y) OrElse
               Not Integer.TryParse(values(2), NumberStyles.Integer, CultureInfo.InvariantCulture, width) OrElse
               Not Integer.TryParse(values(3), NumberStyles.Integer, CultureInfo.InvariantCulture, height) Then Continue For
            pb.Location = LayoutPointToPanel(New Point(Math.Max(0, x), Math.Max(0, y)))
            pb.Width = Math.Max(1, CInt(Math.Round(Math.Max(1, width) / _VideoPanelScale)))
            pb.Height = Math.Max(1, CInt(Math.Round(Math.Max(1, height) / _VideoPanelScale)))
            CheckPBposition(pb)
            applied = True
        Next
        Return applied
    End Function

    Private Sub LoadActiveOverlayLayout()
        If Not _MapReady Then Return
        _ApplyingOutputLayout = True
        Try
            If _ActiveLayoutAspectIndex = 1 Then
                If Not TryApplyPortraitOverlayLayout(My.Settings.PortraitOverlayLayout) Then
                    SetPBMapSizes()
                    SetPBMapPositions(True, True, My.Settings.MIZoomMapPos, My.Settings.MILegMapPos, True, My.Settings.MIDynamicMapPos)
                    SetWidgetPictureBoxPositions()
                    For Each pb In GetLayoutControls().Values
                        CheckPBposition(pb)
                    Next
                    My.Settings.PortraitOverlayLayout = SerializeCurrentOverlayLayout()
                End If
                If String.IsNullOrWhiteSpace(My.Settings.PortraitOverlayVisibility) Then My.Settings.PortraitOverlayVisibility = SerializeCurrentOverlayVisibility()
                ApplyOverlayVisibility(My.Settings.PortraitOverlayVisibility)
            Else
                _MapImgHandler.LoadSettings()
                SetPBMapSizes()
                SetPBMapPositions(True, True, My.Settings.MIZoomMapPos, My.Settings.MILegMapPos, True, My.Settings.MIDynamicMapPos)
                SetWidgetPictureBoxPositions()
                If String.IsNullOrWhiteSpace(My.Settings.LandscapeOverlayVisibility) Then My.Settings.LandscapeOverlayVisibility = SerializeCurrentOverlayVisibility()
                ApplyOverlayVisibility(My.Settings.LandscapeOverlayVisibility)
            End If
            UpdateMapImgs()
            UpdateWidgetPreviewImages()
        Finally
            _ApplyingOutputLayout = False
        End Try
    End Sub

    Private Sub SaveWidgetLayoutSettings()
        My.Settings.MIWidgetTimeMapPos = GetRealWidgetPosition(pbTime)
        My.Settings.MIWidgetDistanceMapPos = GetRealWidgetPosition(pbDistance)
        My.Settings.MIWidgetPaceMapPos = GetRealWidgetPosition(pbPace)
        My.Settings.MIWidgetPulseMapPos = GetRealWidgetPosition(pbPulse)
        My.Settings.MIWidgetHGraphMapPos = GetRealWidgetPosition(pbHGraph)
        My.Settings.MIWidgetTimeWidth = GetRealWidgetWidth(pbTime)
        My.Settings.MIWidgetTimeHeight = GetRealWidgetHeight(pbTime)
        My.Settings.MIWidgetDistanceWidth = GetRealWidgetWidth(pbDistance)
        My.Settings.MIWidgetDistanceHeight = GetRealWidgetHeight(pbDistance)
        My.Settings.MIWidgetPaceWidth = GetRealWidgetWidth(pbPace)
        My.Settings.MIWidgetPaceHeight = GetRealWidgetHeight(pbPace)
        My.Settings.MIWidgetPulseWidth = GetRealWidgetWidth(pbPulse)
        My.Settings.MIWidgetPulseHeight = GetRealWidgetHeight(pbPulse)
        My.Settings.MIWidgetHGraphWidth = GetRealWidgetWidth(pbHGraph)
        My.Settings.MIWidgetHGraphHeight = GetRealWidgetHeight(pbHGraph)
        CommitWidgetTimeOffsetText()
    End Sub

    Private Sub MediaPlayer_EncounteredError(sender As Object, e As EventArgs)
        ' Handle the encountered error
        MsgBox("fejl")
        ' Display error message or perform error handling
    End Sub

    Private Sub frmAdjustmentPlayer_new_Load(sender As Object, e As EventArgs) Handles Me.Load
        If DesignMode OrElse LicenseManager.UsageMode = LicenseUsageMode.Designtime OrElse Mainform1 Is Nothing Then Return
        InitializeOutputLayoutControls()
        cbLeg.Checked = My.Settings.cbShowLegMAp
        cbZoom.Checked = My.Settings.cbShowRoute
        cbDyn.Checked = My.Settings.cbShowDynamicMap
        FormatWidgetTimeOffsetText()
        ApplyWidgetVisibility()
        IsLoaded = True
        SetShowMaps()
        If Not _DeferredInitStarted Then
            _DeferredInitStarted = True
            BeginInvoke(New Action(AddressOf InitializePreviewAfterShow))
        End If
    End Sub

    Private Sub frmAdjustmentPlayer_new_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        SaveOutputLayoutControlSettings()
        SaveActiveOverlayLayout()
        My.Settings.Save()
    End Sub

    Private Sub InitializeOutputLayoutControls()
        SetSelectedIndexOrDefault(cboOutputAspect, My.Settings.OutputAspectIndex)
        BuildOutputResolutionItems()
        SetSelectedIndexOrDefault(cboPortraitMode, My.Settings.PortraitModeIndex)
        SetMapAnimationFpsControl()
        RefreshLayoutPresetList()
        chkShowSafeZone.Checked = My.Settings.ShowOutputSafeZone
        InitializePortraitFrameGuide()
        UpdateOutputLayoutPreview()
        _ActiveLayoutAspectIndex = cboOutputAspect.SelectedIndex
    End Sub

    Private Sub SetMapAnimationFpsControl()
        Dim frameStep As Double = My.Settings.MISmoothFrameStepSeconds
        If frameStep <= 0 Then frameStep = 0.1R
        Dim fps As Decimal = CDec(Math.Round(1.0R / frameStep))
        numMapAnimationFps.Value = Math.Max(numMapAnimationFps.Minimum, Math.Min(numMapAnimationFps.Maximum, fps))
    End Sub

    Private Sub numMapAnimationFps_ValueChanged(sender As Object, e As EventArgs) Handles numMapAnimationFps.ValueChanged
        If Not IsLoaded OrElse numMapAnimationFps.Value <= 0 Then Return
        My.Settings.MISmoothFrameStepSeconds = 1.0R / CDbl(numMapAnimationFps.Value)
        Mainform1.bMakeMapDirty = True
        Mainform1.CheckDirtyStatus()
    End Sub

    Private Sub BuildOutputResolutionItems()
        _UpdatingOutputResolutionItems = True
        Try
            cboOutputResolution.Items.Clear()
            _OutputResolutionValues.Clear()
            If cboOutputAspect.SelectedIndex = 1 Then
                cboOutputResolution.Items.Add("1080x1920 (HD)")
                _OutputResolutionValues.Add(1)
                cboOutputResolution.SelectedIndex = 0
                My.Settings.OutputResolutionIndex = 1
            Else
                cboOutputResolution.Items.Add("Auto (= input)")
                cboOutputResolution.Items.Add("1920x1080 (HD)")
                cboOutputResolution.Items.Add("2560x1440 (2K)")
                cboOutputResolution.Items.Add("3840x2160 (4K)")
                _OutputResolutionValues.AddRange(New Integer() {0, 1, 2, 3})
                Dim savedValue As Integer = Math.Max(0, Math.Min(3, My.Settings.LandscapeOutputResolutionIndex))
                cboOutputResolution.SelectedIndex = savedValue
                My.Settings.OutputResolutionIndex = savedValue
            End If
        Finally
            _UpdatingOutputResolutionItems = False
        End Try
    End Sub

    Private Shared Sub SetSelectedIndexOrDefault(combo As ComboBox, savedIndex As Integer)
        If combo Is Nothing OrElse combo.Items.Count = 0 Then Return
        combo.SelectedIndex = Math.Max(0, Math.Min(combo.Items.Count - 1, savedIndex))
    End Sub

    Private Sub SaveOutputLayoutControlSettings()
        If cboOutputAspect.SelectedIndex >= 0 Then My.Settings.OutputAspectIndex = cboOutputAspect.SelectedIndex
        If cboOutputResolution.SelectedIndex >= 0 AndAlso cboOutputResolution.SelectedIndex < _OutputResolutionValues.Count Then
            My.Settings.OutputResolutionIndex = _OutputResolutionValues(cboOutputResolution.SelectedIndex)
            If cboOutputAspect.SelectedIndex = 0 Then My.Settings.LandscapeOutputResolutionIndex = My.Settings.OutputResolutionIndex
        End If
        If cboPortraitMode.SelectedIndex >= 0 Then My.Settings.PortraitModeIndex = cboPortraitMode.SelectedIndex
        If cboLayoutPreset.SelectedIndex >= 0 Then My.Settings.LayoutPresetIndex = cboLayoutPreset.SelectedIndex
        My.Settings.ShowOutputSafeZone = chkShowSafeZone.Checked
    End Sub

    Private Sub OutputLayoutSelectionChanged(sender As Object, e As EventArgs) Handles cboOutputAspect.SelectedIndexChanged,
                                                                                         cboOutputResolution.SelectedIndexChanged,
                                                                                         cboPortraitMode.SelectedIndexChanged,
                                                                                         chkShowSafeZone.CheckedChanged
        If Not IsLoaded Then Return
        If _UpdatingOutputResolutionItems Then Return
        Dim aspectChanged As Boolean = (sender Is cboOutputAspect AndAlso cboOutputAspect.SelectedIndex <> _ActiveLayoutAspectIndex)
        Dim resolutionChanged As Boolean = (sender Is cboOutputResolution)
        If (aspectChanged OrElse resolutionChanged) AndAlso _MapReady Then SaveActiveOverlayLayout()
        If aspectChanged Then BuildOutputResolutionItems()
        SaveOutputLayoutControlSettings()
        Mainform1.InitVideoFormat()
        If sender Is cboOutputAspect OrElse sender Is cboOutputResolution Then
            Mainform1.bMakeMapDirty = True
            Mainform1.bOutputVideoDirty = True
            Mainform1.CheckDirtyStatus()
        ElseIf sender Is cboPortraitMode Then
            Mainform1.bOutputVideoDirty = True
            Mainform1.CheckDirtyStatus()
        End If
        UpdateOutputLayoutPreview()
        UpdateActiveCanvasScale()
        If aspectChanged Then
            _ActiveLayoutAspectIndex = cboOutputAspect.SelectedIndex
            If _MapReady Then LoadActiveOverlayLayout()
            RefreshLayoutPresetList()
        ElseIf resolutionChanged AndAlso _MapReady Then
            LoadActiveOverlayLayout()
        End If
    End Sub

    Private Sub InitializePortraitFrameGuide()
        If _PortraitFrameLines.Count = 0 Then
            For i As Integer = 0 To 3
                Dim line As New Panel With {
                    .BackColor = Color.Orange,
                    .Cursor = Cursors.SizeWE,
                    .TabStop = False,
                    .Visible = False
                }
                AddHandler line.MouseDown, AddressOf PortraitFrameLine_MouseDown
                AddHandler line.MouseMove, AddressOf PortraitFrameLine_MouseMove
                AddHandler line.MouseUp, AddressOf PortraitFrameLine_MouseUp
                VideoPanel.Controls.Add(line)
                _PortraitFrameLines.Add(line)
            Next
        End If

        If _PortraitSafeZoneLines.Count = 0 Then
            For i As Integer = 0 To 3
                Dim line As New Panel With {
                    .BackColor = Color.FromArgb(215, 215, 215),
                    .TabStop = False,
                    .Visible = False
                }
                VideoPanel.Controls.Add(line)
                _PortraitSafeZoneLines.Add(line)
            Next
        End If
    End Sub

    Private Sub UpdateOutputLayoutPreview()
        Dim portrait As Boolean = (cboOutputAspect.SelectedIndex = 1)
        cboPortraitMode.Enabled = portrait
        chkShowSafeZone.Enabled = portrait

        Dim resolutionWidth As Integer = Mainform1.ExtraFunc.InputWidth
        Dim resolutionHeight As Integer = Mainform1.ExtraFunc.InputHeight
        lblOutputInfo.Text = String.Format(Globalization.CultureInfo.InvariantCulture, "Output: {0} x {1}", resolutionWidth, resolutionHeight)

        If Not portrait OrElse VideoPanel.ClientSize.Height <= 0 Then
            SetPortraitGuideVisibility(False)
            _PortraitFrameRectangle = Rectangle.Empty
            Return
        End If

        Dim frameHeight As Integer = VideoPanel.ClientSize.Height
        Dim frameWidth As Integer = Math.Max(1, CInt(Math.Round(frameHeight * 9.0R / 16.0R)))
        frameWidth = Math.Min(frameWidth, VideoPanel.ClientSize.Width)
        Dim availableX As Integer = Math.Max(0, VideoPanel.ClientSize.Width - frameWidth)
        Dim savedPosition As Double = Math.Max(0.0R, Math.Min(1.0R, My.Settings.PortraitFramePosition))
        Dim frameX As Integer
        If cboPortraitMode.SelectedIndex = 1 Then
            frameX = availableX \ 2
        Else
            frameX = CInt(Math.Round(availableX * savedPosition))
        End If

        If _PortraitVideoDragHandle Is Nothing Then
            _PortraitVideoDragHandle = New Label With {
                .AutoSize = False,
                .BackColor = Color.Gold,
                .BorderStyle = BorderStyle.FixedSingle,
                .Cursor = Cursors.SizeNS,
                .Font = New Font(Font.FontFamily, 8.25F, FontStyle.Bold),
                .Size = New Size(104, 26),
                .Text = If(IsDanishUi(), "↕ Træk video", "↕ Move video"),
                .TextAlign = ContentAlignment.MiddleCenter,
                .Visible = False
            }
            AddHandler _PortraitVideoDragHandle.MouseDown, AddressOf PortraitVideoDragHandle_MouseDown
            AddHandler _PortraitVideoDragHandle.MouseMove, AddressOf PortraitVideoDragHandle_MouseMove
            AddHandler _PortraitVideoDragHandle.MouseUp, AddressOf PortraitVideoDragHandle_MouseUp
            VideoPanel.Controls.Add(_PortraitVideoDragHandle)
        End If
        _PortraitFrameRectangle = New Rectangle(frameX, 0, frameWidth, frameHeight)

        PositionGuideRectangle(_PortraitFrameLines, _PortraitFrameRectangle, PortraitFrameLineWidth)

        Dim safeInsetX As Integer = Math.Max(8, CInt(Math.Round(frameWidth * 0.08R)))
        Dim safeInsetY As Integer = Math.Max(8, CInt(Math.Round(frameHeight * 0.06R)))
        Dim safeRectangle As New Rectangle(_PortraitFrameRectangle.Left + safeInsetX,
                                           _PortraitFrameRectangle.Top + safeInsetY,
                                           Math.Max(1, _PortraitFrameRectangle.Width - 2 * safeInsetX),
                                           Math.Max(1, _PortraitFrameRectangle.Height - 2 * safeInsetY))
        PositionGuideRectangle(_PortraitSafeZoneLines, safeRectangle, 1)
        SetPortraitGuideVisibility(True)
        ApplyVideoPreviewLayout()
    End Sub

    Private Shared Sub PositionGuideRectangle(lines As List(Of Panel), bounds As Rectangle, lineWidth As Integer)
        If lines Is Nothing OrElse lines.Count < 4 Then Return
        lines(0).Bounds = New Rectangle(bounds.Left, bounds.Top, bounds.Width, lineWidth)
        lines(1).Bounds = New Rectangle(bounds.Right - lineWidth, bounds.Top, lineWidth, bounds.Height)
        lines(2).Bounds = New Rectangle(bounds.Left, bounds.Bottom - lineWidth, bounds.Width, lineWidth)
        lines(3).Bounds = New Rectangle(bounds.Left, bounds.Top, lineWidth, bounds.Height)
    End Sub

    Private Sub SetPortraitGuideVisibility(visible As Boolean)
        For Each line As Panel In _PortraitFrameLines
            line.Visible = visible
            If visible Then line.BringToFront()
        Next
        Dim showSafeZone As Boolean = visible AndAlso chkShowSafeZone.Checked
        For Each line As Panel In _PortraitSafeZoneLines
            line.Visible = showSafeZone
            If showSafeZone Then line.BringToFront()
        Next
        If visible Then
            For Each line As Panel In _PortraitFrameLines
                line.BringToFront()
            Next
        End If
    End Sub

    Private Sub PortraitFrameLine_MouseDown(sender As Object, e As MouseEventArgs)
        If e.Button <> MouseButtons.Left OrElse _PortraitFrameRectangle.IsEmpty OrElse cboPortraitMode.SelectedIndex = 1 Then Return
        Dim mousePoint As Point = VideoPanel.PointToClient(Cursor.Position)
        _PortraitFrameDragging = True
        _PortraitFrameDragOffsetX = mousePoint.X - _PortraitFrameRectangle.Left
        DirectCast(sender, Control).Capture = True
    End Sub

    Private Sub PortraitFrameLine_MouseMove(sender As Object, e As MouseEventArgs)
        If Not _PortraitFrameDragging Then Return
        Dim mousePoint As Point = VideoPanel.PointToClient(Cursor.Position)
        Dim availableX As Integer = Math.Max(0, VideoPanel.ClientSize.Width - _PortraitFrameRectangle.Width)
        Dim newX As Integer = Math.Max(0, Math.Min(availableX, mousePoint.X - _PortraitFrameDragOffsetX))
        Dim deltaX As Integer = newX - _PortraitFrameRectangle.Left
        My.Settings.PortraitFramePosition = If(availableX > 0, newX / CDbl(availableX), 0.5R)
        UpdateOutputLayoutPreview()
        If deltaX <> 0 AndAlso _ActiveLayoutAspectIndex = 1 Then
            For Each pb In GetLayoutControls().Values
                pb.Left += deltaX
            Next
        End If
    End Sub

    Private Sub PortraitFrameLine_MouseUp(sender As Object, e As MouseEventArgs)
        If Not _PortraitFrameDragging Then Return
        _PortraitFrameDragging = False
        DirectCast(sender, Control).Capture = False
    End Sub

    Private Sub VideoPanel_SizeChanged(sender As Object, e As EventArgs) Handles VideoPanel.SizeChanged
        If IsLoaded Then
            UpdateOutputLayoutPreview()
            UpdateActiveCanvasScale()
            If _MapReady Then LoadActiveOverlayLayout()
        End If
    End Sub

    Private Sub cboLayoutPreset_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboLayoutPreset.SelectedIndexChanged
        If IsLoaded AndAlso cboLayoutPreset.SelectedIndex >= 0 Then My.Settings.LayoutPresetIndex = cboLayoutPreset.SelectedIndex
    End Sub

    Private Function GetLayoutPresetFolder() As String
        Return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Headcam Orienteering", "Layout presets")
    End Function

    Private Function GetCurrentLayoutOutputSize() As Size
        If cboOutputAspect.SelectedIndex = 1 Then Return New Size(PortraitOutputWidth, PortraitOutputHeight)
        Return New Size(Math.Max(1, Mainform1.ExtraFunc.InputWidth), Math.Max(1, Mainform1.ExtraFunc.InputHeight))
    End Function

    Private Function GetCurrentAspectKey() As String
        Return If(cboOutputAspect.SelectedIndex = 1, "9x16", "16x9")
    End Function

    Private Sub RefreshLayoutPresetList(Optional selectPath As String = Nothing)
        Dim previousPath As String = Nothing
        If cboLayoutPreset.SelectedIndex >= 0 AndAlso cboLayoutPreset.SelectedIndex < _LayoutPresetFiles.Count Then previousPath = _LayoutPresetFiles(cboLayoutPreset.SelectedIndex)
        cboLayoutPreset.Items.Clear()
        _LayoutPresetFiles.Clear()
        Dim folder As String = GetLayoutPresetFolder()
        Directory.CreateDirectory(folder)
        For Each file As String In Directory.GetFiles(folder, "*.ohmpreset.xml").OrderBy(Function(value) Path.GetFileName(value))
            Try
                Dim root As XElement = XDocument.Load(file).Root
                If root Is Nothing OrElse CStr(root.Attribute("aspect")) <> GetCurrentAspectKey() Then Continue For
                _LayoutPresetFiles.Add(file)
                cboLayoutPreset.Items.Add(CStr(root.Attribute("name")))
            Catch
                ' Ignore malformed files in the shared preset folder.
            End Try
        Next
        Dim wanted As String = If(selectPath, previousPath)
        Dim selected As Integer = If(String.IsNullOrEmpty(wanted), -1, _LayoutPresetFiles.FindIndex(Function(value) String.Equals(value, wanted, StringComparison.OrdinalIgnoreCase)))
        If selected < 0 AndAlso cboLayoutPreset.Items.Count > 0 Then selected = Math.Max(0, Math.Min(cboLayoutPreset.Items.Count - 1, My.Settings.LayoutPresetIndex))
        cboLayoutPreset.SelectedIndex = selected
    End Sub

    Private Sub btnSaveLayoutPreset_Click(sender As Object, e As EventArgs) Handles btnSaveLayoutPreset.Click
        If Not _MapReady Then Return
        Dim presetName As String = InputBox("Navn på layout-preset:", "Gem layout-preset", "Layout").Trim()
        If presetName = "" Then Return
        SaveActiveOverlayLayout()
        Dim size As Size = GetCurrentLayoutOutputSize()
        Dim safeName As String = String.Concat(presetName.Select(Function(ch) If(Path.GetInvalidFileNameChars().Contains(ch), "_"c, ch))).Trim()
        Dim file As String = Path.Combine(GetLayoutPresetFolder(), $"{safeName}_{GetCurrentAspectKey()}_{size.Width}x{size.Height}.ohmpreset.xml")
        Dim layout As New XElement("Layout")
        For Each item In GetLayoutControls()
            Dim bounds As Rectangle = GetPictureBoxLayoutBounds(item.Value)
            layout.Add(New XElement("Overlay", New XAttribute("key", item.Key), New XAttribute("x", bounds.X), New XAttribute("y", bounds.Y), New XAttribute("width", bounds.Width), New XAttribute("height", bounds.Height)))
        Next
        Dim style As New XElement("Style",
            New XAttribute("frameForm", My.Settings.MIMapFrameFormIndex), New XAttribute("frameWidth", My.Settings.MIFrameWidth),
            New XAttribute("frameColor", My.Settings.MIFrameColor.ToArgb()), New XAttribute("feather", My.Settings.MIFrameFeather),
            New XAttribute("zoomRadius", My.Settings.MIZoomRad), New XAttribute("legRadius", My.Settings.MILegRad), New XAttribute("dynamicRadius", My.Settings.MIDynamicRad),
            New XAttribute("zoomTransparency", My.Settings.MIZoomTransparency), New XAttribute("legTransparency", My.Settings.MILegTransparency), New XAttribute("dynamicTransparency", My.Settings.MIDynamicTransparency),
            New XAttribute("zoomFactor", My.Settings.MIZoomZoom), New XAttribute("legMargin", My.Settings.MILegMargin),
            New XAttribute("showZoom", My.Settings.cbShowRoute), New XAttribute("showLeg", My.Settings.cbShowLegMAp), New XAttribute("showDynamic", My.Settings.cbShowDynamicMap),
            New XAttribute("showTime", My.Settings.MIWidgetTimeEnabled), New XAttribute("showDistance", My.Settings.MIWidgetDistanceEnabled), New XAttribute("showPace", My.Settings.MIWidgetPaceEnabled),
            New XAttribute("showPulse", My.Settings.MIWidgetPulseEnabled), New XAttribute("showHeight", My.Settings.MIWidgetHGraphEnabled))
        Dim document As New XDocument(New XElement("OHeadcamLayoutPreset", New XAttribute("version", 1), New XAttribute("name", presetName), New XAttribute("aspect", GetCurrentAspectKey()), New XAttribute("width", size.Width), New XAttribute("height", size.Height), layout, style))
        Directory.CreateDirectory(GetLayoutPresetFolder())
        document.Save(file)
        RefreshLayoutPresetList(file)
        MessageBox.Show("Layout-preset gemt:" & Environment.NewLine & file, "Layout-preset", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnApplyLayoutPreset_Click(sender As Object, e As EventArgs) Handles btnApplyLayoutPreset.Click
        If cboLayoutPreset.SelectedIndex < 0 OrElse cboLayoutPreset.SelectedIndex >= _LayoutPresetFiles.Count Then Return
        ApplyLayoutPreset(_LayoutPresetFiles(cboLayoutPreset.SelectedIndex))
    End Sub

    Private Sub cboLayoutPreset_DropDown(sender As Object, e As EventArgs) Handles cboLayoutPreset.DropDown
        RefreshLayoutPresetList()
    End Sub

    Private Sub btnManageLayoutPresets_Click(sender As Object, e As EventArgs) Handles btnManageLayoutPresets.Click
        Directory.CreateDirectory(GetLayoutPresetFolder())
        Diagnostics.Process.Start("explorer.exe", GetLayoutPresetFolder())
        RefreshLayoutPresetList()
    End Sub

    Private Sub ApplyLayoutPreset(file As String)
        UseWaitCursor = True
        Cursor.Current = Cursors.WaitCursor
        Try
            Dim root As XElement = XDocument.Load(file).Root
            If root Is Nothing Then Throw New InvalidDataException("Presetfilen mangler et rodelement.")
            If CStr(root.Attribute("aspect")) <> GetCurrentAspectKey() Then
                MessageBox.Show("Dette preset har et andet videoformat end det aktuelle output.", "Layout-preset", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim sourceWidth As Integer = Math.Max(1, ReadIntAttribute(root, "width", GetCurrentLayoutOutputSize().Width))
            Dim sourceHeight As Integer = Math.Max(1, ReadIntAttribute(root, "height", GetCurrentLayoutOutputSize().Height))
            Dim targetSize As Size = GetCurrentLayoutOutputSize()
            Dim scaleX As Double = targetSize.Width / CDbl(sourceWidth)
            Dim scaleY As Double = targetSize.Height / CDbl(sourceHeight)
            Dim controls = GetLayoutControls()
            Dim layout As XElement = root.Element("Layout")
            If layout IsNot Nothing Then
                For Each overlay As XElement In layout.Elements("Overlay")
                    Dim pb As PictureBox = Nothing
                    If Not controls.TryGetValue(CStr(overlay.Attribute("key")), pb) Then Continue For
                    Dim x As Integer = CInt(Math.Round(ReadIntAttribute(overlay, "x", 0) * scaleX))
                    Dim y As Integer = CInt(Math.Round(ReadIntAttribute(overlay, "y", 0) * scaleY))
                    Dim w As Integer = Math.Max(1, CInt(Math.Round(ReadIntAttribute(overlay, "width", 1) * scaleX)))
                    Dim h As Integer = Math.Max(1, CInt(Math.Round(ReadIntAttribute(overlay, "height", 1) * scaleY)))
                    pb.Location = LayoutPointToPanel(New Point(x, y))
                    pb.Width = Math.Max(1, CInt(Math.Round(w / _VideoPanelScale)))
                    pb.Height = Math.Max(1, CInt(Math.Round(h / _VideoPanelScale)))
                    CheckPBposition(pb)
                Next
            End If

            Dim style As XElement = root.Element("Style")
            If style IsNot Nothing Then
                Dim pixelScale As Double = Math.Min(scaleX, scaleY)
                My.Settings.MIMapFrameFormIndex = ReadIntAttribute(style, "frameForm", My.Settings.MIMapFrameFormIndex)
                My.Settings.MIFrameWidth = ScalePresetPixel(ReadIntAttribute(style, "frameWidth", My.Settings.MIFrameWidth), pixelScale)
                My.Settings.MIZoomRad = ScalePresetPixel(ReadIntAttribute(style, "zoomRadius", My.Settings.MIZoomRad), pixelScale)
                My.Settings.MILegRad = ScalePresetPixel(ReadIntAttribute(style, "legRadius", My.Settings.MILegRad), pixelScale)
                My.Settings.MIDynamicRad = ScalePresetPixel(ReadIntAttribute(style, "dynamicRadius", My.Settings.MIDynamicRad), pixelScale)
                My.Settings.MILegMargin = ScalePresetPixel(ReadIntAttribute(style, "legMargin", My.Settings.MILegMargin), pixelScale)
                My.Settings.MIFrameColor = Color.FromArgb(ReadIntAttribute(style, "frameColor", My.Settings.MIFrameColor.ToArgb()))
                My.Settings.MIFrameFeather = ReadBooleanAttribute(style, "feather", My.Settings.MIFrameFeather)
                My.Settings.MIZoomTransparency = ReadDecimalAttribute(style, "zoomTransparency", My.Settings.MIZoomTransparency)
                My.Settings.MILegTransparency = ReadDecimalAttribute(style, "legTransparency", My.Settings.MILegTransparency)
                My.Settings.MIDynamicTransparency = ReadDecimalAttribute(style, "dynamicTransparency", My.Settings.MIDynamicTransparency)
                My.Settings.MIZoomZoom = ReadDecimalAttribute(style, "zoomFactor", My.Settings.MIZoomZoom)
                My.Settings.cbShowRoute = ReadBooleanAttribute(style, "showZoom", My.Settings.cbShowRoute)
                My.Settings.cbShowLegMAp = ReadBooleanAttribute(style, "showLeg", My.Settings.cbShowLegMAp)
                My.Settings.cbShowDynamicMap = ReadBooleanAttribute(style, "showDynamic", My.Settings.cbShowDynamicMap)
                My.Settings.MIWidgetTimeEnabled = ReadBooleanAttribute(style, "showTime", My.Settings.MIWidgetTimeEnabled)
                My.Settings.MIWidgetDistanceEnabled = ReadBooleanAttribute(style, "showDistance", My.Settings.MIWidgetDistanceEnabled)
                My.Settings.MIWidgetPaceEnabled = ReadBooleanAttribute(style, "showPace", My.Settings.MIWidgetPaceEnabled)
                My.Settings.MIWidgetPulseEnabled = ReadBooleanAttribute(style, "showPulse", My.Settings.MIWidgetPulseEnabled)
                My.Settings.MIWidgetHGraphEnabled = ReadBooleanAttribute(style, "showHeight", My.Settings.MIWidgetHGraphEnabled)
            End If

            cbZoom.Checked = My.Settings.cbShowRoute
            cbLeg.Checked = My.Settings.cbShowLegMAp
            cbDyn.Checked = My.Settings.cbShowDynamicMap
            ApplyWidgetVisibility()
            SetShowMaps()
            UpdateWithSettings()
            SaveActiveOverlayLayout()
            My.Settings.Save()
        Catch ex As Exception
            MessageBox.Show("Presetfilen kunne ikke indlæses:" & Environment.NewLine & ex.Message, "Layout-preset", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            UseWaitCursor = False
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    Private Shared Function ReadIntAttribute(element As XElement, name As String, fallback As Integer) As Integer
        Dim value As Integer
        If element IsNot Nothing AndAlso Integer.TryParse(CStr(element.Attribute(name)), NumberStyles.Integer, CultureInfo.InvariantCulture, value) Then Return value
        Return fallback
    End Function

    Private Shared Function ReadBooleanAttribute(element As XElement, name As String, fallback As Boolean) As Boolean
        Dim value As Boolean
        If element IsNot Nothing AndAlso Boolean.TryParse(CStr(element.Attribute(name)), value) Then Return value
        Return fallback
    End Function

    Private Shared Function ReadDecimalAttribute(element As XElement, name As String, fallback As Decimal) As Decimal
        Dim value As Decimal
        If element IsNot Nothing AndAlso Decimal.TryParse(CStr(element.Attribute(name)), NumberStyles.Float, CultureInfo.InvariantCulture, value) Then Return value
        Return fallback
    End Function

    Private Shared Function ScalePresetPixel(value As Integer, scale As Double) As Integer
        If value <= 0 Then Return Math.Max(0, value)
        Return Math.Max(1, CInt(Math.Round(value * scale)))
    End Function

    Private Sub ApplyVideoPreviewLayout()
        If VideoInfo Is Nothing OrElse VideoInfo.Width <= 0 OrElse VideoInfo.Height <= 0 Then Return
        If cboOutputAspect.SelectedIndex = 1 AndAlso cboPortraitMode.SelectedIndex = 1 AndAlso Not _PortraitFrameRectangle.IsEmpty Then
            Dim videoWidth As Integer = _PortraitFrameRectangle.Width
            Dim videoHeight As Integer = Math.Max(1, CInt(Math.Round(videoWidth * VideoInfo.Height / CDbl(VideoInfo.Width))))
            videoHeight = Math.Min(videoHeight, _PortraitFrameRectangle.Height)
            Dim availableY As Integer = Math.Max(0, _PortraitFrameRectangle.Height - videoHeight)
            Dim position As Double = Math.Max(0.0R, Math.Min(1.0R, My.Settings.PortraitVideoPosition))
            VideoView1.Anchor = AnchorStyles.None
            VideoView1.Bounds = New Rectangle(_PortraitFrameRectangle.Left,
                                               _PortraitFrameRectangle.Top + CInt(Math.Round(availableY * position)),
                                               videoWidth,
                                               videoHeight)
            VideoView1.Cursor = Cursors.SizeNS
            _PortraitVideoDragHandle.Location = New Point(VideoView1.Left + Math.Max(0, (VideoView1.Width - _PortraitVideoDragHandle.Width) \ 2),
                                                          VideoView1.Top + Math.Max(0, (VideoView1.Height - _PortraitVideoDragHandle.Height) \ 2))
            _PortraitVideoDragHandle.Visible = True
            _PortraitVideoDragHandle.BringToFront()
        Else
            VideoView1.Height = VideoPanel.Height
            VideoView1.Width = Math.Max(1, CInt(Math.Round(VideoInfo.Width * VideoPanel.Height / CDbl(VideoInfo.Height))))
            VideoView1.Anchor = AnchorStyles.Right Or AnchorStyles.Top
            VideoView1.Location = New Point(VideoPanel.Width - VideoView1.Width, 0)
            VideoView1.Cursor = Cursors.Default
            If _PortraitVideoDragHandle IsNot Nothing Then _PortraitVideoDragHandle.Visible = False
        End If
        VideoView1.SendToBack()
        SetPortraitGuideVisibility(cboOutputAspect.SelectedIndex = 1)
    End Sub

    Private Sub VideoView1_MouseDown(sender As Object, e As MouseEventArgs) Handles VideoView1.MouseDown
        If e.Button <> MouseButtons.Left OrElse cboOutputAspect.SelectedIndex <> 1 OrElse cboPortraitMode.SelectedIndex <> 1 Then Return
        Dim mousePoint As Point = VideoPanel.PointToClient(Cursor.Position)
        _PortraitVideoDragging = True
        _PortraitVideoDragOffsetY = mousePoint.Y - VideoView1.Top
        VideoView1.Capture = True
    End Sub

    Private Sub VideoView1_MouseMove(sender As Object, e As MouseEventArgs) Handles VideoView1.MouseMove
        If Not _PortraitVideoDragging Then Return
        UpdatePortraitVideoDrag()
    End Sub

    Private Sub VideoView1_MouseUp(sender As Object, e As MouseEventArgs) Handles VideoView1.MouseUp
        If Not _PortraitVideoDragging Then Return
        _PortraitVideoDragging = False
        VideoView1.Capture = False
    End Sub

    Private Sub PortraitVideoDragHandle_MouseDown(sender As Object, e As MouseEventArgs)
        If e.Button <> MouseButtons.Left Then Return
        Dim mousePoint As Point = VideoPanel.PointToClient(Cursor.Position)
        _PortraitVideoDragging = True
        _PortraitVideoDragOffsetY = mousePoint.Y - VideoView1.Top
        _PortraitVideoDragHandle.Capture = True
    End Sub

    Private Sub PortraitVideoDragHandle_MouseMove(sender As Object, e As MouseEventArgs)
        If _PortraitVideoDragging Then UpdatePortraitVideoDrag()
    End Sub

    Private Sub PortraitVideoDragHandle_MouseUp(sender As Object, e As MouseEventArgs)
        If Not _PortraitVideoDragging Then Return
        _PortraitVideoDragging = False
        _PortraitVideoDragHandle.Capture = False
    End Sub

    Private Sub UpdatePortraitVideoDrag()
        Dim mousePoint As Point = VideoPanel.PointToClient(Cursor.Position)
        Dim availableY As Integer = Math.Max(0, _PortraitFrameRectangle.Height - VideoView1.Height)
        Dim newY As Integer = Math.Max(_PortraitFrameRectangle.Top,
                                       Math.Min(_PortraitFrameRectangle.Top + availableY, mousePoint.Y - _PortraitVideoDragOffsetY))
        VideoView1.Top = newY
        _PortraitVideoDragHandle.Top = VideoView1.Top + Math.Max(0, (VideoView1.Height - _PortraitVideoDragHandle.Height) \ 2)
        My.Settings.PortraitVideoPosition = If(availableY > 0, (newY - _PortraitFrameRectangle.Top) / CDbl(availableY), 0.0R)
    End Sub

    Private Sub InitializePreviewAfterShow()
        If IsDisposed Then Return

        Try
            UseWaitCursor = True
            StatusLabel.Text = "Loading preview..."
            Application.DoEvents()

            _MapImgHandler = New clsMapImages(RPs, MapImg)
            _MapDeltaTime = 0
            _MapReady = True
            _MapImgHandler.LoadSettings()
            '20260423_B_ApplyPreviewRenderTuning()
            RebuildPreviewRenderEngines()
            UpdateActiveCanvasScale()
            LoadActiveOverlayLayout()
            ApplyWidgetVisibility()
            _ZoomZoom = My.Settings.MIZoomZoom
            _LegMargin = My.Settings.MILegMargin
            SetVideo()
            StatusLabel.Text = ""
        Catch ex As Exception
            StatusLabel.Text = ex.Message
        Finally
            UseWaitCursor = False
        End Try
    End Sub

    Private Sub ReleaseVideoResources()
        Timer1.Stop()
        Timer2.Stop()
        MapImageTimer.Stop()

        Try
            If Not IsNothing(_mp) Then
                _mp.Stop()
            End If
        Catch
        End Try

        Try
            VideoView1.MediaPlayer = Nothing
        Catch
        End Try

        Try
            If Not IsNothing(_media) Then
                _media.Dispose()
                _media = Nothing
            End If
        Catch
        End Try

        Try
            If Not IsNothing(_mp) Then
                RemoveHandler _mp.EncounteredError, AddressOf MediaPlayer_EncounteredError
                _mp.Dispose()
                _mp = Nothing
            End If
        Catch
        End Try

        Try
            If Not IsNothing(_libVLC) Then
                _libVLC.Dispose()
                _libVLC = Nothing
            End If
        Catch
        End Try
    End Sub

    Private Sub frmAdjustmentPlayer_new_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        ReleaseVideoResources()
    End Sub
End Class
