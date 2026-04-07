Imports System.Globalization
Imports System.Threading
Imports OHeadcamMapApp.My.Resources
Public Class frmSettings
    Private isFormLoading As Boolean = True
    Public Sub New()
        Dim lang As String
        If My.Settings.Language = "English" Then
            lang = "en-US"
        ElseIf My.Settings.Language = "Chinese" Then
            lang = "zh"
        Else

            lang = "da-DK"
        End If
        Thread.CurrentThread.CurrentUICulture = New CultureInfo(lang)
        ' This call is required by the designer.
        InitializeComponent()
        Select Case My.Settings.VideoPadding
            Case "M"
                rbImgMiddle.Checked = True
            Case "L"
                rbImgRight.Checked = True
            Case "R"
                rbImgLeft.Checked = True

        End Select
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        My.Settings.MISmoothFrameStepSeconds = CDbl(numSmooth.Value)
        My.Settings.Save()
        Me.Close()
    End Sub

    Private Sub btnSelectFolder_Click(sender As Object, e As EventArgs) Handles btnSelectFolder.Click
        Dim dialog = New FolderBrowserDialog()
        'dialog.SelectedPath = "C:\"
        If DialogResult.OK = dialog.ShowDialog() Then
            ' User selected a folder
            txtVideoWorkfolder.Text = dialog.SelectedPath + "\"

        End If

    End Sub
    Public Sub SubChangeVideoPadding()
        If rbImgMiddle.Checked Then My.Settings.VideoPadding = "M"
        If rbImgLeft.Checked Then My.Settings.VideoPadding = "R"
        If rbImgRight.Checked Then My.Settings.VideoPadding = "L"
    End Sub
    Private Sub rbImgMiddle_CheckedChanged(sender As Object, e As EventArgs) Handles rbImgMiddle.CheckedChanged
        SubChangeVideoPadding()
        'rbImgRight.Checked = False
        'rbImgLeft.Checked = False
    End Sub

    Private Sub rbImgRight_CheckedChanged(sender As Object, e As EventArgs) Handles rbImgRight.CheckedChanged
        SubChangeVideoPadding()
        'rbImgMiddle.Checked = False
        ' rbImgLeft.Checked = False
    End Sub

    Private Sub rbImgLeft_CheckedChanged(sender As Object, e As EventArgs) Handles rbImgLeft.CheckedChanged
        SubChangeVideoPadding()
        'rbImgRight.Checked = False
        'rbImgMiddle.Checked = False
    End Sub
    Private Function GetDefault(Para As String) As Object
        Return My.Settings.Properties(Para).DefaultValue
    End Function
    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        My.Settings.ffmpegVidstabDetect = GetDefault("ffmpegVidstabDetect")
        My.Settings.ffmpegOutFps = GetDefault("ffmpegOutFps")
        'My.Settings.ffmpegPreset = GetDefault("ffmpegPreset")
        My.Settings.ffmpegCRF = "25"
        My.Settings.VideoPadding = GetDefault("VideoPadding")
        My.Settings.ffmpegVidstabTransform = "vidstabtransform=smoothing=25:crop=black:zoom=0:optzoom=0:interpol='bicubic':input=data.trf:tripod=0,unsharp=5:5:0.8:3:3:0.4"
        My.Settings.cbNewMapF = True
        My.Settings.VideoPadding = "L"
        My.Settings.MISmoothFrameStepSeconds = CDbl(GetDefault("MISmoothFrameStepSeconds"))
        rbImgMiddle.Checked = False
        rbImgRight.Checked = True
        rbImgLeft.Checked = False
        numSmooth.Value = CDec(My.Settings.MISmoothFrameStepSeconds)
        My.Settings.Save()


    End Sub



    Private Sub chkHDformat_CheckedChanged(sender As Object, e As EventArgs) Handles chkHDformat.CheckedChanged
        If Not isFormLoading Then MsgBox(Texts.HDChanged)
    End Sub

    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles Me.Load
        If My.Settings.MISmoothFrameStepSeconds <= 0 Then
            My.Settings.MISmoothFrameStepSeconds = 0.25
        End If
        numSmooth.Value = CDec(My.Settings.MISmoothFrameStepSeconds)
        isFormLoading = False
    End Sub


End Class
