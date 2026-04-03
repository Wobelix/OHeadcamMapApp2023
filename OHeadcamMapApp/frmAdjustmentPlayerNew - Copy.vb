Imports OHeadcamMapApp.My.Resources

Public Class frmAdjustmentPlayer_new2
    Dim Leavex As Boolean
    Dim Mainform1 As MainForm
    Dim Videofile As String
    Dim setbytimer As Boolean
    Dim MaxLength As String
    Const AdjFile As String = "adjust.mp4"
    Const TestFile As String = "test.mp4"
    Dim PlayMode As String, PlayMode_play As String = "play", PlayMode_fast As String = "fast", PlayMode_slow As String = "slow", PlayMode_stop As String = "stop"

    Private _WantedPosition As Double
    Private CurrentTrackValue, ImageTrackValue, _MapTime As Integer
    Private _LastMaptime As Integer = -1
    Private _VideoFileName As String
    Private _MapImgHandler As clsMapImages
    Private _MapDeltaTime As Integer 'Timediff to Current Time
    Private _MapReady As Boolean = False
    Private _UpdateMouse As Boolean = False
    Private _ZoomResized As Boolean = False
    Private _LegResized As Boolean = False
    Private _ZoomMouseIsUp As Boolean = True
    Private _LegMouseIsUp As Boolean = True
    Private _ZoomMapPos As Point
    Private _LegMapPos As Point

    Private RPs As New clsQRRoutePoints
    Private MapImg As Bitmap
    Private VideoInfo As clsFFMPegProbe
    Private PBZoom_Adjust As New clsPictureBoxMoveResize
    Private PBLeg_Adjust As New clsPictureBoxMoveResize
    Private _VideoPanelScale As Double

    Public Sub New(MainF As MainForm, iMaxLength As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Mainform1 = MainF
        MaxLength = iMaxLength
        If Not MaxLength.IndexOf(":") < 0 Then 'time code
            MaxLength = Mainform1.ExtraFunc.TimeStrToSec(MaxLength)
        End If
        Me.Text = Texts.AdjPlayerTitle
        AxWindowsMediaPlayer1.uiMode = "none"
        AxWindowsMediaPlayer1.windowlessVideo = True

        Dim a As New clsQRXMLReader(RPs)
        Dim Appfolder As String = My.Application.Info.DirectoryPath + "\"


        a.ReadXML(Appfolder + My.Settings.QRXML)

        MapImg = New Bitmap(Image.FromFile(My.Settings.QRimage))
        _MapImgHandler = New clsMapImages(RPs, MapImg)
        _MapDeltaTime = 0
        '_MapImgHandler.labelx = Label1
        _MapReady = True

        bTrackChange = False


        PlayMode = PlayMode_stop
        _VideoPanelScale = 1920 / VideoPanel.Width
        If IsNumeric(My.Settings.GPXDiff) Then numGPSDelta.Value = CInt(My.Settings.GPXDiff)
        _MapImgHandler.LoadSettings() 'loading my.settings
        SetPBMapSizes()
        SetPBMapPositions(True, True, My.Settings.MIZoomMapPos, My.Settings.MILegMapPos)
        SetVideo()



    End Sub
    Private Sub SetPBMapPositions(Z As Boolean, L As Boolean, ZMapP As Point, LMapP As Point) 'Real pos to PB pos
        Dim zp, lp As Point
        If Z Then
            zp = New Point(ZMapP.X / _VideoPanelScale, ZMapP.Y / _VideoPanelScale)
            PB_Zoom.Location = zp
        End If
        If L Then
            lp = New Point(LMapP.X / _VideoPanelScale, LMapP.Y / _VideoPanelScale)
            PBLeg.Location = lp
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
    End Sub

    Private Sub SetMapPositions(Z As Boolean, L As Boolean, PBZMapP As Point, PBLMapP As Point) 'PB to real pos
        Dim zp, lp As Point
        If Z Then
            zp = New Point(PBZMapP.X * _VideoPanelScale, PBZMapP.Y * _VideoPanelScale)
            _ZoomMapPos = zp
        End If
        If L Then
            lp = New Point(PBLMapP.X * _VideoPanelScale, PBLMapP.Y * _VideoPanelScale)
            _LegMapPos = lp
        End If
    End Sub


    Private Sub SetVideo()
        VideoInfo = New clsFFMPegProbe
        VideoInfo.StoreVideoProps(Mainform1.GetDeshakedFilename(False))
        Dim file As String = Mainform1.GetDeshakedFilename(False)
        AxWindowsMediaPlayer1.URL = file
        AxWindowsMediaPlayer1.Ctlcontrols.currentPosition = 1
        AxWindowsMediaPlayer1.Height = VideoPanel.Height
        AxWindowsMediaPlayer1.Width = VideoInfo.Width * VideoPanel.Height / VideoInfo.Height
        Dim a As Point
        a.X = VideoPanel.Width - AxWindowsMediaPlayer1.Width

        a.Y = 0
        AxWindowsMediaPlayer1.Anchor = AnchorStyles.Right Or AnchorStyles.Top
        AxWindowsMediaPlayer1.Location = a


        StatusLabel.Text = ""
        Timer1.Start()
        MapImageTimer.Start()
        TrackBar1.Maximum = VideoInfo.duration_sec * 2
        TrackBar1.TickFrequency = TrackBar1.Maximum / 50
        TrackBar1.LargeChange = TrackBar1.TickFrequency
        AxWindowsMediaPlayer1.Ctlcontrols.play()
        AxWindowsMediaPlayer1.Ctlcontrols.pause()

    End Sub


    Private Sub SetPlayBtnTexts(Clicked_Button As Button)

        bFForward.Text = ">>"
        bPlay.Text = ">"
        bSlow.Text = "1/2x >"

        If Not PlayMode = PlayMode_stop Then Clicked_Button.Text = "||"


    End Sub

    Sub setTrackbarValue(time As Double)
        TrackBar1.Value = 2 * time
    End Sub
    Private Sub bSlow_Click(sender As Object, e As EventArgs) Handles bSlow.Click
        If PlayMode = PlayMode_slow Then
            AxWindowsMediaPlayer1.Ctlcontrols.pause()
            setTrackbarValue(AxWindowsMediaPlayer1.Ctlcontrols.currentPosition)
            PlayMode = PlayMode_stop
        Else
            AxWindowsMediaPlayer1.settings.rate = 0.5
            AxWindowsMediaPlayer1.Ctlcontrols.play()
            bTrackChange = False
            PlayMode = PlayMode_slow

        End If
        SetPlayBtnTexts(bSlow)
    End Sub

    Private Sub bFForward_Click(sender As Object, e As EventArgs) Handles bFForward.Click
        If PlayMode = PlayMode_fast Then
            AxWindowsMediaPlayer1.Ctlcontrols.pause()
            setTrackbarValue(AxWindowsMediaPlayer1.Ctlcontrols.currentPosition)
            PlayMode = PlayMode_stop
        Else
            AxWindowsMediaPlayer1.settings.rate = 2
            AxWindowsMediaPlayer1.Ctlcontrols.play()
            bTrackChange = False
            PlayMode = PlayMode_fast

        End If
        SetPlayBtnTexts(bFForward)
    End Sub

    Private Sub bPlay_Click(sender As Object, e As EventArgs) Handles bPlay.Click
        If PlayMode = PlayMode_play Then
            AxWindowsMediaPlayer1.Ctlcontrols.pause()
            setTrackbarValue(AxWindowsMediaPlayer1.Ctlcontrols.currentPosition)
            PlayMode = PlayMode_stop
        Else
            AxWindowsMediaPlayer1.settings.rate = 1
            AxWindowsMediaPlayer1.Ctlcontrols.play()
            bTrackChange = False
            PlayMode = PlayMode_play

        End If
        SetPlayBtnTexts(bPlay)
    End Sub


    Private Sub Timer1_Tick_1(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Not IsNothing(AxWindowsMediaPlayer1.currentMedia) Then
            If AxWindowsMediaPlayer1.currentMedia.duration > 0 And Not Leavex Then
                ' TrackBar1.Maximum = AxWindowsMediaPlayer1.currentMedia.duration
                'TrackBar1.TickFrequency = TrackBar1.Maximum / 50
                ' TrackBar1.LargeChange = TrackBar1.TickFrequency
                AxWindowsMediaPlayer1.Ctlcontrols.pause()
                Timer1.Stop()
            End If
        End If
    End Sub
    Dim bTrackChange As Boolean
    Private Sub TrackBar1_ValueChanged(sender As Object, e As EventArgs) Handles TrackBar1.ValueChanged
        Dim timeSpan As TimeSpan = timeSpan.FromSeconds(TrackBar1.Value)
        Dim timeFormat As String = timeSpan.ToString("h\:mm\:ss")
        Label2.Text = timeFormat
        _WantedPosition = TrackBar1.Value / 2

    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Dim a As Double
        If PlayMode = PlayMode_stop Then
            a = Math.Abs(_WantedPosition - AxWindowsMediaPlayer1.Ctlcontrols.currentPosition)
            If a > 0.2 Then

                AxWindowsMediaPlayer1.Ctlcontrols.currentPosition = _WantedPosition


                AxWindowsMediaPlayer1.Ctlcontrols.play()
                AxWindowsMediaPlayer1.Ctlcontrols.pause()
            End If
        Else

            setTrackbarValue(AxWindowsMediaPlayer1.Ctlcontrols.currentPosition)
        End If
        Dim timeSpan As TimeSpan = timeSpan.FromSeconds(Math.Round(AxWindowsMediaPlayer1.Ctlcontrols.currentPosition))
        Dim timeFormat As String = timeSpan.ToString("h\:mm\:ss")
        lblVideoPos.Text = timeFormat
    End Sub

    Sub PlayerClear()
        AxWindowsMediaPlayer1.Ctlcontrols.stop()
        AxWindowsMediaPlayer1.URL = ""
        AxWindowsMediaPlayer1.close()
        AxWindowsMediaPlayer1.currentPlaylist.clear()
    End Sub
    Sub PlayerNewURL(urlstr As String)
        AxWindowsMediaPlayer1.URL = urlstr
        AxWindowsMediaPlayer1.Ctlcontrols.currentPosition = 1

        Timer1.Start()
    End Sub

    Private Sub bB1_Click(sender As Object, e As EventArgs) Handles bB1.Click
        If Not PlayMode = PlayMode_stop Then
            AxWindowsMediaPlayer1.Ctlcontrols.pause()
            PlayMode = PlayMode_stop
            SetPlayBtnTexts(bPlay)
        End If
        TrackBar1.Value -= 1

    End Sub

    Private Sub bF1_Click(sender As Object, e As EventArgs) Handles bF1.Click
        If Not PlayMode = PlayMode_stop Then
            AxWindowsMediaPlayer1.Ctlcontrols.pause()
            PlayMode = PlayMode_stop
            SetPlayBtnTexts(bPlay)
        End If
        TrackBar1.Value += 1
    End Sub

    Private Sub ResizePBsToVideoPanel()

        'PBLeg.Width = _MapImgHandler.LegMapImage.Width * _VideoPanelScale
        'PBLeg.Height = _MapImgHandler.LegMapImage.Height * _VideoPanelScale
        'PB_Zoom.Width = _MapImgHandler.ZoomMapImage.Width * _VideoPanelScale
        'PB_Zoom.Height = _MapImgHandler.ZoomMapImage.Height * _VideoPanelScale
    End Sub

    Private Sub UpdateMapImgs()
        Dim w, h As Integer
        Dim c As Integer

        w = PB_Zoom.Width * _VideoPanelScale
        h = PB_Zoom.Height * _VideoPanelScale
        c = Math.Min(w, h) ' circle rounding
        PB_Zoom.Image = _MapImgHandler.ZoomImage3(_MapTime, w, h, c / 2 - 5)
        w = PBLeg.Width * _VideoPanelScale
        h = PBLeg.Height * _VideoPanelScale
        PBLeg.Image = _MapImgHandler.LapImage2(_MapTime,
                                              100, w, h, 30)
    End Sub

    Private Sub MapImageTimer_Tick(sender As Object, e As EventArgs) Handles MapImageTimer.Tick
        CurrentTrackValue = Math.Round(AxWindowsMediaPlayer1.Ctlcontrols.currentPosition) + numGPSDelta.Value

        If Not ImageTrackValue = CurrentTrackValue Then

            ImageTrackValue = CurrentTrackValue


            If _MapReady And (CurrentTrackValue < RPs.RoutePoints.Count) Then


                _MapTime = CurrentTrackValue + _MapDeltaTime
                If _MapTime > -1 And _MapTime < RPs.RoutePoints.Count Then
                    _MapImgHandler.DrawPositionOnBackgroundImage(_MapTime)
                    _LastMaptime = _MapTime

                    UpdateMapImgs()
                End If
            End If
        ElseIf _MapReady And ((_ZoomResized And _ZoomMouseIsUp) Or (_LegResized And _LegMouseIsUp)) Then
            If _LastMaptime > -1 Then
                UpdateMapImgs()
                _ZoomResized = False
                _LegResized = False
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

    Private Sub TestMap_Click(sender As Object, e As EventArgs)



    End Sub

    Dim bMouseDown As Boolean = False
    Private Sub TrackBar1_MouseDown(sender As Object, e As MouseEventArgs) Handles TrackBar1.MouseDown
        bMouseDown = True
        If Not PlayMode = PlayMode_stop Then
            AxWindowsMediaPlayer1.Ctlcontrols.pause()
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
        SetMapPositions(True, True, PB_Zoom.Location, PBLeg.Location)
        My.Settings.MIZoomMapPos = _ZoomMapPos
        My.Settings.MILegMapPos = _LegMapPos
        My.Settings.Save()
        Me.Close()
    End Sub




End Class
