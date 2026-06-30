Imports System.Globalization
Imports System.Threading
Imports OHeadcamMapApp.My.Resources
Public Class frmSettings
    Private isFormLoading As Boolean = True
    Private Const DefaultOutputFormat As String = "Auto"
    Private Const DefaultVideoQuality As String = "High"
    Private Const DefaultVideoSpeed As String = "Balanced"
    Private Const DefaultSmoothOverlayFrameStepSeconds As Double = 0.1

    Private Class ComboOption
        Public ReadOnly Property Value As String
        Private ReadOnly DisplayText As String

        Public Sub New(value As String, displayText As String)
            Me.Value = value
            Me.DisplayText = displayText
        End Sub

        Public Overrides Function ToString() As String
            Return DisplayText
        End Function
    End Class
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
        My.Settings.VideoQuality = GetSelectedComboValue(cmbVQuality, DefaultVideoQuality)
        My.Settings.VideoEncoderSpeed = GetSelectedComboValue(cmbVSpeed, DefaultVideoSpeed)
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

    Private Sub LoadVideoQualityAndSpeed()
        cmbVQuality.DropDownStyle = ComboBoxStyle.DropDownList
        cmbVSpeed.DropDownStyle = ComboBoxStyle.DropDownList

        cmbVQuality.Items.Clear()
        cmbVSpeed.Items.Clear()

        If Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName = "da" Then
            cmbVQuality.Items.Add(New ComboOption("Normal", "Normal"))
            cmbVQuality.Items.Add(New ComboOption("High", "Høj"))
            cmbVQuality.Items.Add(New ComboOption("VeryHigh", "Meget høj"))
            cmbVSpeed.Items.Add(New ComboOption("Fast", "Hurtig"))
            cmbVSpeed.Items.Add(New ComboOption("Balanced", "Balanceret"))
        Else
            cmbVQuality.Items.Add(New ComboOption("Normal", "Normal"))
            cmbVQuality.Items.Add(New ComboOption("High", "High"))
            cmbVQuality.Items.Add(New ComboOption("VeryHigh", "Very high"))
            cmbVSpeed.Items.Add(New ComboOption("Fast", "Fast"))
            cmbVSpeed.Items.Add(New ComboOption("Balanced", "Balanced"))
        End If

        SelectComboValue(cmbVQuality, If(String.IsNullOrWhiteSpace(My.Settings.VideoQuality), DefaultVideoQuality, My.Settings.VideoQuality))
        SelectComboValue(cmbVSpeed, If(String.IsNullOrWhiteSpace(My.Settings.VideoEncoderSpeed), DefaultVideoSpeed, My.Settings.VideoEncoderSpeed))
    End Sub

    Private Sub SelectComboValue(combo As ComboBox, value As String)
        For Each item As Object In combo.Items
            Dim optionItem As ComboOption = TryCast(item, ComboOption)
            If optionItem IsNot Nothing AndAlso String.Equals(optionItem.Value, value, StringComparison.OrdinalIgnoreCase) Then
                combo.SelectedItem = item
                Return
            End If
        Next

        If combo.Items.Count > 0 Then combo.SelectedIndex = 0
    End Sub

    Private Function GetSelectedComboValue(combo As ComboBox, defaultValue As String) As String
        Dim selectedOption As ComboOption = TryCast(combo.SelectedItem, ComboOption)
        If selectedOption IsNot Nothing Then Return selectedOption.Value

        If Not String.IsNullOrWhiteSpace(combo.Text) Then Return combo.Text.Trim()

        Return defaultValue
    End Function

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        My.Settings.ffmpegVidstabDetect = GetDefault("ffmpegVidstabDetect")
        My.Settings.ffmpegOutFps = GetDefault("ffmpegOutFps")
        My.Settings.VideoPadding = GetDefault("VideoPadding")
        My.Settings.ffmpegVidstabTransform = "vidstabtransform=smoothing=25:crop=black:zoom=0:optzoom=0:interpol='bicubic':input=data.trf:tripod=0,unsharp=5:5:0.8:3:3:0.4"
        ' CLEANUP-CBNEWMAPF-START:My.Settings.cbNewMapF = True
        My.Settings.VideoPadding = "L"
        My.Settings.MISmoothFrameStepSeconds = CDbl(GetDefault("MISmoothFrameStepSeconds"))
        My.Settings.OutputFormat = CStr(GetDefault("OutputFormat"))
        My.Settings.VideoQuality = CStr(GetDefault("VideoQuality"))
        My.Settings.VideoEncoderSpeed = CStr(GetDefault("VideoEncoderSpeed"))
        rbImgMiddle.Checked = False
        rbImgRight.Checked = True
        rbImgLeft.Checked = False
        numSmooth.Value = ConvertSmoothFrameStepToFps(My.Settings.MISmoothFrameStepSeconds)
        LoadOutputFormat()
        LoadVideoQualityAndSpeed()
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
        LoadVideoQualityAndSpeed()
        LoadVideoEncoderInfoAsync()
        isFormLoading = False
    End Sub

    Private Async Sub LoadVideoEncoderInfoAsync()
        Dim encoderInfoControl As Control = FindControlRecursive(Me, "txtVideoEncoderInfo")
        If encoderInfoControl Is Nothing Then Return

        encoderInfoControl.Text = "Finder automatisk videoencoder..."
        Try
            Dim encoderResult As VideoEncoderDetectionResult =
                Await Task.Run(Function() clsVideoEncoderDetector.GetBestHevcEncoder())

            encoderInfoControl.Text = FormatVideoEncoderInfo(encoderResult)
        Catch ex As Exception
            encoderInfoControl.Text = "Kunne ikke teste videoencoder: " & ex.Message
        End Try
    End Sub

    Private Function FormatVideoEncoderInfo(encoderResult As VideoEncoderDetectionResult) As String
        If encoderResult Is Nothing Then Return "Videoencoder: ukendt"

        Dim hardwareText As String = If(encoderResult.IsHardware, "hardware", "software fallback")
        Dim elapsedText As String = ""
        If encoderResult.TestElapsed.TotalMilliseconds > 0 Then
            elapsedText = Environment.NewLine &
                          "Testtid: " &
                          encoderResult.TestElapsed.TotalSeconds.ToString("0.00", CultureInfo.InvariantCulture) &
                          " s"
        End If

        Dim ffmpegText As String = ""
        If Not String.IsNullOrWhiteSpace(encoderResult.FfmpegVersion) Then
            ffmpegText = Environment.NewLine & encoderResult.FfmpegVersion
        End If

        Return "Auto HEVC: " & encoderResult.DisplayName &
               " (" & encoderResult.EncoderName & ", " & hardwareText & ")" &
               Environment.NewLine & "Pixel format: " & encoderResult.PixelFormat &
               elapsedText &
               ffmpegText
    End Function

    Private Function FindControlRecursive(parent As Control, controlName As String) As Control
        If parent Is Nothing Then Return Nothing
        For Each child As Control In parent.Controls
            If String.Equals(child.Name, controlName, StringComparison.OrdinalIgnoreCase) Then Return child
            Dim nested As Control = FindControlRecursive(child, controlName)
            If nested IsNot Nothing Then Return nested
        Next
        Return Nothing
    End Function


End Class
