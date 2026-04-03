Imports OHeadcamMapApp.My.Resources

Public Class frmAdjustmentPlayer
    Dim Leavex As Boolean
    Dim Mainform1 As MainForm
    Dim Videofile As String
    Dim setbytimer As Boolean
    Dim MaxLength As String
    Const AdjFile As String = "adjust.mp4"
    Const TestFile As String = "test.mp4"
    Dim PlayMode As String, PlayMode_play As String = "play", PlayMode_fast As String = "fast", PlayMode_slow As String = "slow", PlayMode_stop As String = "stop"

    Enum Adjstates
        NoVideo
        InTest
        InAdjustment
    End Enum
    Dim Formstate As Adjstates
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
        txtPlayerfile.Text = AdjFile

        If My.Settings.bAdjVideoIsReady Then
            If Not My.Computer.FileSystem.FileExists(txtPlayerfile.Text) Then
                MsgBox(Texts.NoPlayerFile + txtPlayerfile.Text)
                AxWindowsMediaPlayer1.URL = ""
                StatusLabel.Text = Texts.AdjVidStatusNoVideo
                txtLength.Text = MaxLength
                Formstate = Adjstates.NoVideo
            Else

                AxWindowsMediaPlayer1.URL = txtPlayerfile.Text
                AxWindowsMediaPlayer1.Ctlcontrols.currentPosition = 1
                'AxWindowsMediaPlayer1.Ctlcontrols.play()
                'While AxWindowsMediaPlayer1.currentMedia.duration = 0
                'Threading.Thread.Sleep(500)
                'AxWindowsMediaPlayer1.Ctlcontrols.play()
                'End While
                'AxWindowsMediaPlayer1.Ctlcontrols.pause()
                'HScrollBarVideo.Maximum = AxWindowsMediaPlayer1.currentMedia.duration
                'AxWindowsMediaPlayer1.Ctlcontrols.stop()
                StatusLabel.Text = ""
                Timer1.Start()

                Formstate = Adjstates.InAdjustment
            End If
        Else
            AxWindowsMediaPlayer1.URL = ""
            Formstate = Adjstates.NoVideo
            StatusLabel.Text = Texts.AdjVidStatusNoVideo
        End If
        bTrackChange = False

        pbIconWork.Visible = False
        PlayMode = PlayMode_stop
        EnableByState()

    End Sub
    Private Sub SetPlayBtnTexts(Clicked_Button As Button)
        Clicked_Button.Text = "||"
        Select Case PlayMode
            Case PlayMode_fast

                bPlay.Text = ">"
                bSlow.Text = "1/2x >"
            Case PlayMode_play
                bFForward.Text = ">>"
                bSlow.Text = "1/2x >"
            Case PlayMode_slow
                bFForward.Text = ">>"
                bPlay.Text = ">"
            Case PlayMode_stop
                bFForward.Text = ">>"
                bPlay.Text = ">"
                bSlow.Text = "1/2x >"
        End Select
    End Sub
    Private Sub bSlow_Click(sender As Object, e As EventArgs) Handles bSlow.Click
        If PlayMode = PlayMode_slow Then
            AxWindowsMediaPlayer1.Ctlcontrols.pause()

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

            PlayMode = PlayMode_stop
        Else
            AxWindowsMediaPlayer1.settings.rate = 3
            AxWindowsMediaPlayer1.Ctlcontrols.play()
            bTrackChange = False
            PlayMode = PlayMode_fast

        End If
        SetPlayBtnTexts(bFForward)
    End Sub

    Private Sub bPlay_Click(sender As Object, e As EventArgs) Handles bPlay.Click
        If PlayMode = PlayMode_play Then
            AxWindowsMediaPlayer1.Ctlcontrols.pause()

            PlayMode = PlayMode_stop
        Else
            AxWindowsMediaPlayer1.settings.rate = 1
            AxWindowsMediaPlayer1.Ctlcontrols.play()
            bTrackChange = False
            PlayMode = PlayMode_play

        End If
        SetPlayBtnTexts(bPlay)
    End Sub

    Private Sub btnSetVideoPos_Click(sender As Object, e As EventArgs) Handles btnSetVideoPos.Click
        'txtVideoPos.Text
        My.Settings.txtVideoPos = CStr(Math.Round(AxWindowsMediaPlayer1.Ctlcontrols.currentPosition)) ' = positionInSeconds;

    End Sub

    Private Sub btnSetPosGPS_Click(sender As Object, e As EventArgs) Handles btnSetPosGPS.Click
        'txtGPSPos.Text = CStr(Math.Round(AxWindowsMediaPlayer1.Ctlcontrols.currentPosition)) ' = positionInSeconds;
        My.Settings.txtGPSPos = CStr(Math.Round(AxWindowsMediaPlayer1.Ctlcontrols.currentPosition)) '
    End Sub

    Sub setGPXDiff()
        Dim vid, gps As Integer


        If IsNumeric(txtVideoPos.Text) And IsNumeric(txtGPSPos.Text) Then
            vid = CInt(txtVideoPos.Text)
            gps = CInt(txtGPSPos.Text)
            My.Settings.GPXDiff = CStr(gps - vid)
        Else
            My.Settings.GPXDiff = "0"

        End If
    End Sub
    Private Sub txtVideoPos_TextChanged(sender As Object, e As EventArgs) Handles txtVideoPos.TextChanged, txtGPSPos.TextChanged
        setGPXDiff()
    End Sub




    Private Sub Timer1_Tick_1(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Not IsNothing(AxWindowsMediaPlayer1.currentMedia) Then
            If AxWindowsMediaPlayer1.currentMedia.duration > 0 And Not Leavex Then
                TrackBar1.Maximum = AxWindowsMediaPlayer1.currentMedia.duration
                AxWindowsMediaPlayer1.Ctlcontrols.pause()
                Timer1.Stop()
            End If
        End If
    End Sub

    Private Sub TrackBar1_ValueChanged(sender As Object, e As EventArgs) Handles TrackBar1.ValueChanged
        If Not setbytimer Then

            If Not bMouseDown Then
                bTrackChange = True
                AxWindowsMediaPlayer1.Ctlcontrols.currentPosition = TrackBar1.Value
                AxWindowsMediaPlayer1.Ctlcontrols.play()
                AxWindowsMediaPlayer1.Ctlcontrols.pause()
            End If
        End If
    End Sub
    Dim bTrackChange As Boolean
    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        If Not bTrackChange Then
            lblVideoPos.Text = CStr(Math.Round(AxWindowsMediaPlayer1.Ctlcontrols.currentPosition))
            setbytimer = True

            TrackBar1.Value = Math.Round(AxWindowsMediaPlayer1.Ctlcontrols.currentPosition)
            bTrackChange = False

            setbytimer = False
        End If
    End Sub
    Sub SetInstructionText(visible As Boolean)

        LabelIns1.Visible = visible
        LabelIns2.Visible = visible
        LabelIns3.Visible = visible
        LabelIns4.Visible = visible
        Label2.Visible = visible

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
    Sub MakeVideo(filename As String, GPS0 As Boolean)


        PlayerClear()
        pbIconWork.Visible = True
        Mainform1.MakeAdjVideo(filename, txtLength.Text, GPS0)
        pbIconWork.Visible = False
        PlayerNewURL(filename)

        StatusLabel.Text = ""

        Timer1.Start()
    End Sub
    Sub EnableByState()
        Select Case Formstate
            Case Adjstates.NoVideo
                btnMakeAdjVideo.Enabled = True
                btnMakeTestVideo.Enabled = False
                btnReAdjust.Enabled = False
                EnableAdjControls(False)
                SetInstructionText(True)
            Case Adjstates.InTest
                btnMakeAdjVideo.Enabled = False
                btnMakeTestVideo.Enabled = False
                btnReAdjust.Enabled = True
                EnableAdjControls(False)
                SetInstructionText(False)
            Case Adjstates.InAdjustment
                btnMakeAdjVideo.Enabled = False
                btnMakeTestVideo.Enabled = True
                btnReAdjust.Enabled = False
                EnableAdjControls(True)
                SetInstructionText(True)
        End Select


    End Sub
    Sub EnableAdjControls(enable As Boolean)

        btnSetPosGPS.Enabled = enable
        btnSetVideoPos.Enabled = enable
        txtGPSPos.Enabled = enable
        txtVideoPos.Enabled = enable


    End Sub
    Private Sub btnMakeAdjVideo_Click(sender As Object, e As EventArgs) Handles btnMakeAdjVideo.Click

        MakeVideo("adjust.mp4", True)
        My.Settings.bAdjVideoIsReady = True
        Formstate = Adjstates.InAdjustment
        EnableByState()

    End Sub
    Private Sub btnMakeTestVideo_Click(sender As Object, e As EventArgs) Handles btnMakeTestVideo.Click
        If IsNumeric(txtGPSDiff.Text) Then

            Mainform1.LblGPSDiff.Text = txtGPSDiff.Text
            MakeVideo(TestFile, False)
            PlayerClear()
            PlayerNewURL(TestFile)
            Formstate = Adjstates.InTest
            EnableByState()
            My.Settings.AdjTestVideoReady = True
            txtPlayerfile.Text = TestFile
        End If
    End Sub
    Private Sub btnReAdjust_Click(sender As Object, e As EventArgs) Handles btnReAdjust.Click
        PlayerClear()
        PlayerNewURL(AdjFile)
        EnableAdjControls(True)
        My.Settings.AdjTestVideoReady = False
        Formstate = Adjstates.InAdjustment
        EnableByState()
    End Sub
    Dim bMouseDown As Boolean = False
    Private Sub TrackBar1_MouseDown(sender As Object, e As MouseEventArgs) Handles TrackBar1.MouseDown
        bMouseDown = True
    End Sub

    Private Sub TrackBar1_MouseUp(sender As Object, e As MouseEventArgs) Handles TrackBar1.MouseUp
        bMouseDown = False
        'AxWindowsMediaPlayer1.Ctlcontrols.currentPosition = TrackBar1.Value
        'AxWindowsMediaPlayer1.Ctlcontrols.play()
        'AxWindowsMediaPlayer1.Ctlcontrols.pause()
        'bTrackChange = True
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        bTrackChange = True
        lblVideoPos.Text = TrackBar1.Value
    End Sub

    Private Sub btnTransfer_Click(sender As Object, e As EventArgs) Handles btnTransfer.Click
        'Mainform1.LblGPSDiff.Text = txtGPSDiff.Text
        My.Settings.Save()
        Me.Close()
    End Sub



    Private Sub txtLength_TextChanged(sender As Object, e As EventArgs) Handles txtLength.TextChanged
        If IsNumeric(txtLength.Text) Then

            If CSng(txtLength.Text) > CSng(MaxLength) Then
                StatusLabel.Text = Texts.AdjLengthToBig + MaxLength
            End If
            btnMakeAdjVideo.Enabled = True
        End If
    End Sub
End Class