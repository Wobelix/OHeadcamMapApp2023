Public Class frmVideoImageAdjust
    Dim VideoImageGenerator As clsVideoImageGenerator
    Private isMouseDown As Boolean = False
    Private isTrackchanged As Boolean = False
    Private lock As Boolean = False
    Private _VideoReady As Boolean = False
    Private IsFilterChanged As Boolean = False
    Private CurrentTrackValue, ImageTrackValue As Integer
    Private _VideoFileName As String
    Private _EQContrast, _EQbrightness, _EQsaturation, _EQGamma As Single
    Private ResizeVideoPB As clsPicBKeepAspectResize = New clsPicBKeepAspectResize




    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub btnVideoSelect_Click(sender As Object, e As EventArgs) Handles btnVideoSelect.Click

        Dim folder, fn As String, pos As Integer
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            txtVideofile.Text = OpenFileDialog1.FileName
            pos = InStrRev(OpenFileDialog1.FileName, "\")
            fn = Strings.Right(OpenFileDialog1.FileName, Len(OpenFileDialog1.FileName) - pos)
            folder = Strings.Left(OpenFileDialog1.FileName, pos)
            _VideoFileName = OpenFileDialog1.FileName
        End If
        If Not _VideoFileName = "" Then
            InitImage()
            _VideoReady = True
            InitFilters()
        End If
    End Sub
    Private Sub InitImage()
        VideoImageGenerator = New clsVideoImageGenerator(_VideoFileName, "a.jpg", PictureBox1)
        ResizeVideoPB.SetNewAspect()
        TrackBar1.SmallChange = 1
        TrackBar1.Maximum = CInt(VideoImageGenerator._videoDuration * 2)
        VideoImageGenerator._SliderMax = TrackBar1.Maximum
        SetTickFreq(50)
        SetlblTValue(0)
        VideoImageGenerator.GenerateImage(0)
        Timer1.Enabled = True
    End Sub

    Private Sub InitFilters()
        SetRot()
        SetSharp()
        SetColorImpr()
        EQ_Changed()
        SetHue()
        setNoise()
    End Sub
    Private Sub SetTickFreq(numTicks As Integer)

        Dim tickFrequency As Integer = CInt(TrackBar1.Maximum / numTicks)

        ' Set the TickFrequency property to the calculated value
        TrackBar1.TickFrequency = tickFrequency
        TrackBar1.LargeChange = tickFrequency

    End Sub

    Private Sub TrackBar1_MouseDown(sender As Object, e As MouseEventArgs) Handles TrackBar1.MouseDown
        isMouseDown = True
    End Sub

    Private Sub TrackBar1_MouseUp(sender As Object, e As MouseEventArgs) Handles TrackBar1.MouseUp
        isMouseDown = False

    End Sub

    Private Sub SetlblTValue(seconds As Double)
        Dim timeSpan As TimeSpan = TimeSpan.FromSeconds(seconds)
        Dim formattedTime As String = timeSpan.ToString("h\:mm\:ss")
        lblTValue.Text = formattedTime
    End Sub
    Private Sub TrackBar1_ValueChanged(sender As Object, e As EventArgs) Handles TrackBar1.ValueChanged
        CurrentTrackValue = TrackBar1.Value
        If _VideoReady Then SetlblTValue(VideoImageGenerator.CalcSliderTimer(CurrentTrackValue))
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub
    Sub SetRot()
        If CInt(numRot.Value) = 0 And CInt(numZoom.Value) = 100 Then
            VideoImageGenerator.ResetFilter(CInt(FilterNameSpace.Filters.Rotate))
        Else
            VideoImageGenerator.UpdateFilter(0, VideoImageGenerator.FilterFuncs.FF_Rotatefilter(CInt(numRot.Value), VideoImageGenerator.ImageWidth,
                                                                                                VideoImageGenerator.ImageHeight, CSng(numZoom.Value / 100)))
        End If
        IsFilterChanged = True
    End Sub
    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles numRot.ValueChanged
        If _VideoReady Then SetRot()
    End Sub
    Sub SetColorImpr()
        If cbColorImp.Checked Then
            VideoImageGenerator.UpdateFilter(CInt(FilterNameSpace.Filters.ColorImpr), VideoImageGenerator.FilterFuncs.FF_ColorImprovefilter)
        Else
            VideoImageGenerator.ResetFilter(CInt(FilterNameSpace.Filters.ColorImpr))
        End If
        IsFilterChanged = True
    End Sub
    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles cbColorImp.CheckedChanged
        If _VideoReady Then SetColorImpr()
    End Sub

    Private Sub btnMinus_Click(sender As Object, e As EventArgs) Handles btnMinus.Click
        If _VideoReady Then
            setSliderValue(VideoImageGenerator.UpdateImageDelta(False, 1))
        End If
    End Sub

    Private Sub setSliderValue(InVal As Integer)
        lock = True
        CurrentTrackValue = InVal
        ImageTrackValue = CurrentTrackValue
        TrackBar1.Value = InVal
        SetlblTValue(VideoImageGenerator.seektime)
        lock = False
    End Sub
    Private Sub BtnPlus_Click(sender As Object, e As EventArgs) Handles BtnPlus.Click
        If _VideoReady Then setSliderValue(VideoImageGenerator.UpdateImageDelta(True, 1))
    End Sub
    Sub SetSharp()
        Dim sharpval As Integer
        sharpval = nunSharp.Value * 2 + 3

        If CInt(nunSharp.Value) = 0 Then
            VideoImageGenerator.ResetFilter(CInt(FilterNameSpace.Filters.Sharp))
        Else
            VideoImageGenerator.UpdateFilter(CInt(FilterNameSpace.Filters.Sharp), VideoImageGenerator.FilterFuncs.FF_SharpFilter(sharpval))
        End If
        IsFilterChanged = True
    End Sub
    Private Sub NumericUpDown2_ValueChanged(sender As Object, e As EventArgs) Handles nunSharp.ValueChanged
        If _VideoReady Then SetSharp()
    End Sub
    Private Sub EQ_Changed()
        _EQContrast = numContr.Value / 10 + 1 '
        _EQbrightness = numBright.Value / 20
        _EQsaturation = numSat.Value / 5 + 1
        _EQGamma = numGamma.Value / 10 + 1
        If _EQContrast = 1 And _EQbrightness = 0 And _EQsaturation = 1 And _EQGamma = 1 Then
            VideoImageGenerator.ResetFilter(CInt(FilterNameSpace.Filters.EQ))
        Else
            VideoImageGenerator.UpdateFilter(CInt(FilterNameSpace.Filters.EQ), VideoImageGenerator.FilterFuncs.FF_ImageEQFilter(_EQbrightness, _EQContrast, _EQsaturation, _EQGamma))
        End If
        IsFilterChanged = True
    End Sub

    Private Sub numSat_ValueChanged(sender As Object, e As EventArgs) Handles numSat.ValueChanged
        If _VideoReady Then EQ_Changed()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If _VideoReady Then My.Settings.No_deshake_filter = VideoImageGenerator.FilterString
        My.Settings.Save()
        Close()
    End Sub

    Private Sub SetHue()
        Dim hueval As Single
        hueval = numHue.Value / 10

        If numHue.Value = 0 Then
            VideoImageGenerator.ResetFilter(CInt(FilterNameSpace.Filters.Hue))
        Else
            VideoImageGenerator.UpdateFilter(CInt(FilterNameSpace.Filters.Hue), VideoImageGenerator.FilterFuncs.FF_ImageHueFilter(hueval))
        End If
        IsFilterChanged = True
    End Sub
    Private Sub numHue_ValueChanged_1(sender As Object, e As EventArgs) Handles numHue.ValueChanged
        If _VideoReady Then SetHue()
    End Sub
    Private Sub setNoise()
        If cbNoise.Checked Then
            VideoImageGenerator.UpdateFilter(CInt(FilterNameSpace.Filters.Noise), VideoImageGenerator.FilterFuncs.FF_DenoiseFilter(0, 0, 0, 0))
        Else
            VideoImageGenerator.ResetFilter(CInt(FilterNameSpace.Filters.Noise))
        End If
        IsFilterChanged = True
    End Sub
    Private Sub cbNoise_CheckedChanged(sender As Object, e As EventArgs) Handles cbNoise.CheckedChanged
        If _VideoReady Then setNoise()
    End Sub

    Private Sub frmVideoImageAdjust_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ResizeVideoPB.OnFormLoad(PictureBox1, Me.ClientSize)
    End Sub

    Private Sub frmVideoImageAdjust_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If Not IsNothing(sender) Then ResizeVideoPB.OnFormResize(sender)
    End Sub
    Sub SetLensDist()
        If cbLensDist.Checked Then
            VideoImageGenerator.UpdateFilter(CInt(FilterNameSpace.Filters.LensDist), VideoImageGenerator.FilterFuncs.FF_LensCorrection)
        Else
            VideoImageGenerator.ResetFilter(CInt(FilterNameSpace.Filters.LensDist))
        End If
        IsFilterChanged = True
    End Sub
    Private Sub cbLensDist_CheckedChanged_1(sender As Object, e As EventArgs) Handles cbLensDist.CheckedChanged
        If _VideoReady Then SetLensDist()
    End Sub

    Private Sub numZoom_ValueChanged(sender As Object, e As EventArgs) Handles numZoom.ValueChanged
        If _VideoReady Then SetRot()
    End Sub

    Private Sub numGamma_ValueChanged(sender As Object, e As EventArgs) Handles numGamma.ValueChanged
        If _VideoReady Then EQ_Changed()
    End Sub

    Private Sub numBright_ValueChanged(sender As Object, e As EventArgs) Handles numBright.ValueChanged
        If _VideoReady Then EQ_Changed()
    End Sub

    Private Sub numContr_ValueChanged(sender As Object, e As EventArgs) Handles numContr.ValueChanged
        If _VideoReady Then EQ_Changed()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        If ((Not ImageTrackValue = CurrentTrackValue) Or IsFilterChanged) And Not lock Then
            lock = True
            ImageTrackValue = CurrentTrackValue
            VideoImageGenerator.UpdateImage(ImageTrackValue, IsFilterChanged)
            IsFilterChanged = False
            SetlblTValue(VideoImageGenerator.seektime)

            lock = False

        End If
    End Sub

End Class