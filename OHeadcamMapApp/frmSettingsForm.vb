Imports System.Globalization
Imports System.Threading
Imports OHeadcamMapApp.My.Resources
Public Class frmSettings
    Private isFormLoading As Boolean = True
    Private Const DefaultOutputFormat As String = "Auto"
    Private Const DefaultSmoothOverlayFrameStepSeconds As Double = 0.25
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
        My.Settings.MISmoothFrameStepSeconds = ConvertSmoothFpsToFrameStep(CDbl(numSmooth.Value))
        My.Settings.OutputFormat = GetSelectedOutputFormat()
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

    Private Function ConvertSmoothFrameStepToFps(frameStepSeconds As Double) As Decimal
        Dim safeFrameStep As Double = frameStepSeconds
        If safeFrameStep <= 0 Then safeFrameStep = DefaultSmoothOverlayFrameStepSeconds
        Return CDec(Math.Max(CDbl(numSmooth.Minimum), Math.Min(CDbl(numSmooth.Maximum), Math.Round(1.0 / safeFrameStep))))
    End Function

    Private Function ConvertSmoothFpsToFrameStep(fps As Double) As Double
        Dim safeFps As Double = fps
        If safeFps <= 0 Then safeFps = 1.0 / DefaultSmoothOverlayFrameStepSeconds
        Return 1.0 / safeFps
    End Function

    Private Function GetSelectedOutputFormat() As String
        If cmbOutputFormat.SelectedItem IsNot Nothing Then
            Return cmbOutputFormat.SelectedItem.ToString()
        End If

        If Not String.IsNullOrWhiteSpace(cmbOutputFormat.Text) Then
            Return cmbOutputFormat.Text.Trim()
        End If

        Return DefaultOutputFormat
    End Function
    Private Sub LoadOutputFormat()
        Dim savedValue As String = My.Settings.OutputFormat
        If String.IsNullOrWhiteSpace(savedValue) Then savedValue = DefaultOutputFormat

        If cmbOutputFormat.Items.Contains(savedValue) Then
            cmbOutputFormat.SelectedItem = savedValue
        ElseIf cmbOutputFormat.Items.Count > 0 Then
            cmbOutputFormat.SelectedIndex = 0
        End If
    End Sub
    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        My.Settings.ffmpegVidstabDetect = GetDefault("ffmpegVidstabDetect")
        My.Settings.ffmpegOutFps = GetDefault("ffmpegOutFps")
        'My.Settings.ffmpegPreset = GetDefault("ffmpegPreset")
        My.Settings.ffmpegCRF = "25"
        My.Settings.VideoPadding = GetDefault("VideoPadding")
        My.Settings.ffmpegVidstabTransform = "vidstabtransform=smoothing=25:crop=black:zoom=0:optzoom=0:interpol='bicubic':input=data.trf:tripod=0,unsharp=5:5:0.8:3:3:0.4"
        ' CLEANUP-CBNEWMAPF-START:My.Settings.cbNewMapF = True
        My.Settings.VideoPadding = "L"
        My.Settings.MISmoothFrameStepSeconds = CDbl(GetDefault("MISmoothFrameStepSeconds"))
        My.Settings.OutputFormat = CStr(GetDefault("OutputFormat"))
        rbImgMiddle.Checked = False
        rbImgRight.Checked = True
        rbImgLeft.Checked = False
        numSmooth.Value = ConvertSmoothFrameStepToFps(My.Settings.MISmoothFrameStepSeconds)
        LoadOutputFormat()
        My.Settings.Save()


    End Sub



    Private Sub chkHDformat_CheckedChanged(sender As Object, e As EventArgs)
        If Not isFormLoading Then MsgBox(Texts.HDChanged)
    End Sub

    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles Me.Load
        If My.Settings.MISmoothFrameStepSeconds <= 0 Then
            My.Settings.MISmoothFrameStepSeconds = DefaultSmoothOverlayFrameStepSeconds
        End If
        numSmooth.Value = ConvertSmoothFrameStepToFps(My.Settings.MISmoothFrameStepSeconds)
        LoadOutputFormat()
        isFormLoading = False
    End Sub


End Class
