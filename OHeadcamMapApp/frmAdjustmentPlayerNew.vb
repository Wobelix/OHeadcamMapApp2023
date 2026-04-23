Imports System.IO
Imports System.Windows
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
    Private _VideoPanelScale As Double
    Private Const TrackBarUnitsPerSecond As Integer = 10
    Private Const PreviewTailSampleStepSeconds As Double = 0.5
    Private Const PreviewTailMinimumPointDistance As Double = 1.5
    Private WithEvents cbNewLegLayout As CheckBox

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

            _LoadingForm = False
        Catch ex As Exception
            'MapImageTimer.Stop()
        End Try



    End Sub
    Private Sub CheckPBposition(pb As PictureBox)
        ' Sørg for, at PictureBox ikke går uden for venstre kant
        If pb.Left < 0 Then
            pb.Left = 0
        End If

        ' Sørg for, at PictureBox ikke går uden for toppen
        If pb.Top < 0 Then
            pb.Top = 0
        End If

        ' Sørg for, at PictureBox ikke går uden for højre kant
        If pb.Left + pb.Width > pb.Parent.ClientSize.Width Then
            pb.Left = pb.Parent.ClientSize.Width - pb.Width
        End If

        ' Sørg for, at PictureBox ikke går uden for bunden
        If pb.Top + pb.Height > pb.Parent.ClientSize.Height Then
            pb.Top = pb.Parent.ClientSize.Height - pb.Height
        End If
    End Sub
    Private Sub SetPBMapPositions(Z As Boolean, L As Boolean, ZMapP As Point, LMapP As Point, Optional D As Boolean = False, Optional DMapP As Point = Nothing) 'Real pos to PB pos
        Dim zp, lp, dp As Point
        If Z Then
            zp = New Point(ZMapP.X / _VideoPanelScale, ZMapP.Y / _VideoPanelScale)
            PB_Zoom.Location = zp
            CheckPBposition(PB_Zoom)
        End If
        If L Then
            lp = New Point(LMapP.X / _VideoPanelScale, LMapP.Y / _VideoPanelScale)
            PBLeg.Location = lp
            CheckPBposition(PBLeg)
        End If
        If D Then
            dp = New Point(DMapP.X / _VideoPanelScale, DMapP.Y / _VideoPanelScale)
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


    Private Sub SetPBMapSizes()
        PB_Zoom.Width = ScaleToPanel(_MapImgHandler.ZoomWidth)
        PB_Zoom.Height = ScaleToPanel(_MapImgHandler.ZoomHeight)
        PBLeg.Width = ScaleToPanel(_MapImgHandler.LegWidth)
        PBLeg.Height = ScaleToPanel(_MapImgHandler.LegHeight)
        PBDyn.Width = ScaleToPanel(Math.Max(1, My.Settings.MIDynamicWidth))
        PBDyn.Height = ScaleToPanel(Math.Max(1, My.Settings.MIDynamicHeight))
    End Sub

    Private Sub SetMapPositions(Z As Boolean, L As Boolean, PBZMapP As Point, PBLMapP As Point, Optional D As Boolean = False, Optional PBDMapP As Point = Nothing) 'PB to real pos
        Dim zp, lp, dp As Point
        If Z Then
            zp = New Point(PBZMapP.X * _VideoPanelScale, PBZMapP.Y * _VideoPanelScale)
            _ZoomMapPos = zp
        End If
        If L Then
            lp = New Point(PBLMapP.X * _VideoPanelScale, PBLMapP.Y * _VideoPanelScale)
            _LegMapPos = lp
        End If
        If D Then
            dp = New Point(PBDMapP.X * _VideoPanelScale, PBDMapP.Y * _VideoPanelScale)
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
        If Mainform1 Is Nothing Then Return
        If MapImg Is Nothing Then Return

        _ZoomPreviewEngine = New clsMapRenderEngine(RPs, MapImg)
        Mainform1.ApplySavedLegRenderLayout(_ZoomPreviewEngine)
        _LegPreviewEngine = New clsMapRenderEngine(RPs, MapImg)
        Mainform1.ApplySavedLegRenderLayout(_LegPreviewEngine)

        _DynPreviewEngine = New clsMapRenderEngine(RPs, MapImg)
        Mainform1.ApplySavedLegRenderLayout(_DynPreviewEngine)
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
            _ZoomPreviewEngine.ZoomWidth = w
            _ZoomPreviewEngine.ZoomHeight = h
            _ZoomPreviewEngine.ZoomRadius = rad
            _ZoomPreviewEngine.ZoomFactor = My.Settings.MIZoomZoom
            PB_Zoom.Image = _ZoomPreviewEngine.RenderZoomFramePerf8(_MapTimeSmooth)
        End If
        If _LegPreviewEngine IsNot Nothing Then
            _LegPreviewEngine.LegWidth = lw
            _LegPreviewEngine.LegHeight = lh
            _LegPreviewEngine.LegMargin = My.Settings.MILegMargin
            PBLeg.Image = _LegPreviewEngine.RenderLegFramePerf8(_MapTimeSmooth)
        End If
        If _DynPreviewEngine IsNot Nothing Then
            _DynPreviewEngine.DynamicWidth = dw
            _DynPreviewEngine.DynamicHeight = dh
            _DynPreviewEngine.DynamicRadius = Math.Max(0, My.Settings.MIDynamicRad)
            _DynPreviewEngine.DynamicZoomFactor = Math.Max(0.001, My.Settings.MIDynamicZoom)
            _DynPreviewEngine.DynamicLookBehindSeconds = Math.Max(0, My.Settings.MIDynamicLookBehindSeconds)
            _DynPreviewEngine.DynamicLookAheadSeconds = Math.Max(0, My.Settings.MIDynamicLookAheadSeconds)
            _DynPreviewEngine.DynamicMargin = Math.Max(0, My.Settings.MIDynamicMargin)
            PBDyn.Image = _DynPreviewEngine.RenderDynamicFramePerf8(_MapTimeSmooth)
        End If
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
            ElseIf (_ZoomResized And _ZoomMouseIsUp) OrElse (_LegResized And _LegMouseIsUp) OrElse (_DynResized And _DynMouseIsUp) Then
                UpdateMapImgs()
                _ZoomResized = False
                _LegResized = False
                _DynResized = False
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
        _ZoomMouseIsUp = True

    End Sub

    Private Sub PB_Zoom_MouseMove(sender As Object, e As MouseEventArgs) Handles PB_Zoom.MouseMove
        PBZoom_Adjust.PB_MouseMove(sender, e)
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
        _LegMouseIsUp = True
    End Sub

    Private Sub PBLeg_MouseMove(sender As Object, e As MouseEventArgs) Handles PBLeg.MouseMove
        PBLeg_Adjust.PB_MouseMove(sender, e)
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
        _DynMouseIsUp = True
    End Sub

    Private Sub PBDyn_MouseMove(sender As Object, e As MouseEventArgs) Handles PBDyn.MouseMove
        PBDyn_Adjust.PB_MouseMove(sender, e)
    End Sub

    Private Sub PBDyn_SizeChanged(sender As Object, e As EventArgs) Handles PBDyn.SizeChanged
        _DynResized = True
    End Sub

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
        Dim change As Integer = e.Delta / 60
        _LegMargin += change ' Scroll up

    End Sub

    Private Sub PBLeg_MouseEnter(sender As Object, e As EventArgs) Handles PBLeg.MouseEnter
        AddHandler Me.MouseWheel, AddressOf Leg_MouseWheel
    End Sub

    Private Sub PBLeg_MouseLeave(sender As Object, e As EventArgs) Handles PBLeg.MouseLeave
        RemoveHandler Me.MouseWheel, AddressOf Leg_MouseWheel
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
        _MapImgHandler.SaveSettings()
        My.Settings.GPXDiff = CStr(numGPSDelta.Value)
        SetMapPositions(True, True, PB_Zoom.Location, PBLeg.Location, True, PBDyn.Location)
        My.Settings.MIZoomMapPos = _ZoomMapPos
        My.Settings.MILegMapPos = _LegMapPos
        My.Settings.MIDynamicMapPos = _DynMapPos
        My.Settings.MIDynamicWidth = Math.Max(1, CInt(Math.Round(PBDyn.Width * _VideoPanelScale)))
        My.Settings.MIDynamicHeight = Math.Max(1, CInt(Math.Round(PBDyn.Height * _VideoPanelScale)))
        My.Settings.Save()
        Me.Close()
    End Sub

    Private Sub MediaPlayer_EncounteredError(sender As Object, e As EventArgs)
        ' Handle the encountered error
        MsgBox("fejl")
        ' Display error message or perform error handling
    End Sub

    Private Sub frmAdjustmentPlayer_new_Load(sender As Object, e As EventArgs) Handles Me.Load
        cbLeg.Checked = My.Settings.cbShowLegMAp
        cbZoom.Checked = My.Settings.cbShowRoute
        cbDyn.Checked = My.Settings.cbShowDynamicMap
        IsLoaded = True
        SetShowMaps()
        If Not _DeferredInitStarted Then
            _DeferredInitStarted = True
            BeginInvoke(New Action(AddressOf InitializePreviewAfterShow))
        End If
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
            SetPBMapSizes()
            SetPBMapPositions(True, True, My.Settings.MIZoomMapPos, My.Settings.MILegMapPos, True, My.Settings.MIDynamicMapPos)
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
