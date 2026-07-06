Imports OHeadcamMapApp.My.Resources
Public Class frmMapSettings
    Private Const DynamicCornerMinValue As Integer = 0
    Private Const DynamicCornerMaxValue As Integer = 300
    Private Const DynamicMarginMinValue As Integer = 0
    Private Const DynamicMarginMaxValue As Integer = 2000
    Private Const DynamicMarginStepValue As Integer = 10
    Private Const DynamicMinZoomMinValue As Integer = 20
    Private Const DynamicMinZoomMaxValue As Integer = 400
    Private Const DynamicMinZoomStepValue As Integer = 5
    Private Const DynamicMaxZoomMinValue As Integer = 40
    Private Const DynamicMaxZoomMaxValue As Integer = 800
    Private Const DynamicMaxZoomStepValue As Integer = 10

    Public ratio, LegCorner, ZoomCorner, LegMargin, ZoomZoom, ArrowBarb, ArrowWidth As Decimal
    Public DynamicCorner, DynamicMargin, DynamicMinZoom, DynamicMaxZoom As Decimal
    Public tailTransparency, zoomTransparency, legTransparency, dynamicTransparency, paceFastMinPerKm, paceSlowMinPerKm As Decimal
    Public arrowOutlineScale As Integer
    Public circle, feather, tailSpeedColors As Boolean
    Public DotType As String
    Public FrmAdj As frmAdjustmentPlayer_new
    Public dotcolor, framecolor, tailcolor As Color
    Public tailduration, dotsize, framewidth As Integer
    Private _syncingDynamicZoom As Boolean = False
    Private _dynamicTransparencyControl As NumericUpDown = Nothing
    Private ReadOnly _dynamicTransparencyControlNames As String() = {
        "numSmoothTransp",
        "numSmoothMapTransp",
        "numSmoothMapTransparency",
        "numDynamicTransp",
        "numDynamicTransparency",
        "numDynamicMapTransp",
        "numDynamicMapTransparency",
        "numDynTransp"
    }

    Public Sub New(iFrmAdj As frmAdjustmentPlayer_new)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'MsgBox((My.Settings))
        ' My.Settings.Reload()
        FrmAdj = iFrmAdj
    End Sub

    Private Sub bFrameColor_Click(sender As Object, e As EventArgs) Handles bFrameColor.Click

        If ColorDialogFrame.ShowDialog() = DialogResult.OK Then
            ' Retrieve the selected color
            Dim selectedColor As Color = ColorDialogFrame.Color

            ' Update the color in the Label control
            Label2.BackColor = selectedColor

            ' Do something with the selected color
            ' For example, save it in a setting
            framecolor = selectedColor
        End If
    End Sub

    Private Sub bDotColor_Click(sender As Object, e As EventArgs) Handles bDotColor.Click
        If DotColorDialog.ShowDialog() = DialogResult.OK Then
            ' Retrieve the selected color
            Dim selectedColor As Color = DotColorDialog.Color

            ' Update the color in the Label control
            Label4.BackColor = selectedColor
            Label3.BackColor = selectedColor
            ' Do something with the selected color
            ' For example, save it in a setting
            dotcolor = selectedColor
        End If
    End Sub

    Private Sub bTailColor_Click(sender As Object, e As EventArgs) Handles bTailColor.Click
        If TailColorDialog.ShowDialog() = DialogResult.OK Then
            ' Retrieve the selected color
            Dim selectedColor As Color = TailColorDialog.Color

            ' Update the color in the Label control
            Label6.BackColor = selectedColor
            Label5.BackColor = selectedColor
            ' Do something with the selected color
            ' For example, save it in a setting
            tailcolor = selectedColor
        End If
    End Sub

    Private Sub numTailDuration_ValueChanged(sender As Object, e As EventArgs) Handles numTailDuration.ValueChanged
        tailduration = CInt(numTailDuration.Value)
    End Sub

    Private Sub NumDotSize_ValueChanged(sender As Object, e As EventArgs) Handles NumDotSize.ValueChanged
        dotsize = CInt(NumDotSize.Value)
    End Sub

    Private Sub numTailRatio_ValueChanged(sender As Object, e As EventArgs) Handles numTailRatio.ValueChanged
        ratio = numTailRatio.Value
    End Sub

    Private Sub frmMapSettings_Load(sender As Object, e As EventArgs) Handles Me.Load
        numDynMargin.Minimum = DynamicMarginMinValue
        numDynMargin.Maximum = DynamicMarginMaxValue
        numDynMargin.Increment = DynamicMarginStepValue
        numDynMinZoom.Minimum = DynamicMinZoomMinValue
        numDynMinZoom.Maximum = DynamicMinZoomMaxValue
        numDynMinZoom.Increment = DynamicMinZoomStepValue
        numDynMaxZoom.Minimum = DynamicMaxZoomMinValue
        numDynMaxZoom.Maximum = DynamicMaxZoomMaxValue
        numDynMaxZoom.Increment = DynamicMaxZoomStepValue
        numZoomTransp.Minimum = 0D
        numZoomTransp.Maximum = 1D
        numLegTransp.Minimum = 0D
        numLegTransp.Maximum = 1D
        Dim smoothTransparencyControl = ResolveDynamicTransparencyControl()
        If smoothTransparencyControl IsNot Nothing Then
            smoothTransparencyControl.Minimum = 0D
            smoothTransparencyControl.Maximum = 1D
        End If

        numTailDuration.Value = My.Settings.MITailDuration
        NumDotSize.Value = My.Settings.MIDotSize
        numTailRatio.DecimalPlaces = 1
        numTailRatio.Increment = CDec(0.1)
        numTailRatio.Minimum = CDec(0.1)
        numTailRatio.Maximum = CDec(0.9)
        numTailRatio.Value = My.Settings.MITailRatio
        NumFrameSize.Value = My.Settings.MIFrameWidth
        txtLCorner.Text = My.Settings.MILegRad
        numLegMargin.Value = My.Settings.MILegMargin
        txtZCorner.Text = My.Settings.MIZoomRad
        cbCircle.Checked = My.Settings.MIZoomCircle
        cbFeather.Checked = My.Settings.MIFrameFeather
        Label2.BackColor = My.Settings.MIFrameColor
        Label3.BackColor = My.Settings.MIDotColor
        Label5.BackColor = My.Settings.MITailColor
        framecolor = My.Settings.MIFrameColor
        dotcolor = My.Settings.MIDotColor
        tailcolor = My.Settings.MITailColor
        ColorDialogFrame.Color = My.Settings.MIFrameColor
        DotColorDialog.Color = My.Settings.MIDotColor
        TailColorDialog.Color = My.Settings.MITailColor
        numZoomZoom.Value = My.Settings.MIZoomZoom
        txtDynCorner.Text = CStr(Math.Max(DynamicCornerMinValue, Math.Min(DynamicCornerMaxValue, My.Settings.MIDynamicRad)))
        numDynMargin.Value = Math.Max(numDynMargin.Minimum, Math.Min(numDynMargin.Maximum, My.Settings.MIDynamicMargin))
        numDynMinZoom.Value = Math.Max(numDynMinZoom.Minimum, Math.Min(numDynMinZoom.Maximum, My.Settings.MIDynamicMinZoom))
        numDynMaxZoom.Value = Math.Max(numDynMaxZoom.Minimum, Math.Min(numDynMaxZoom.Maximum, My.Settings.MIDynamicMaxZoom))
        numArrowBarb.Value = My.Settings.MIArrowBarb
        numArrowWidth.Value = My.Settings.MIArrowWidth
        cbTailSpeed.Checked = My.Settings.MITailUseSpeedColors
        numPaceFast.Value = CDec(My.Settings.MIPaceFastMinPerKm)
        numPaceSlow.Value = CDec(My.Settings.MIPaceSlowMinPerKm)
        numTransparent.Value = CDec(My.Settings.MITailTransparency)
        numZoomTransp.Value = Math.Max(numZoomTransp.Minimum, Math.Min(numZoomTransp.Maximum, My.Settings.MIZoomTransparency))
        numLegTransp.Value = Math.Max(numLegTransp.Minimum, Math.Min(numLegTransp.Maximum, My.Settings.MILegTransparency))
        If smoothTransparencyControl IsNot Nothing Then
            smoothTransparencyControl.Value = Math.Max(smoothTransparencyControl.Minimum, Math.Min(smoothTransparencyControl.Maximum, My.Settings.MIDynamicTransparency))
        End If
        numOutline.Value = My.Settings.MIArrowOutlineScale
        tailSpeedColors = cbTailSpeed.Checked
        paceFastMinPerKm = numPaceFast.Value
        paceSlowMinPerKm = numPaceSlow.Value
        tailTransparency = numTransparent.Value
        zoomTransparency = numZoomTransp.Value
        legTransparency = numLegTransp.Value
        dynamicTransparency = Math.Max(0D, Math.Min(1D, My.Settings.MIDynamicTransparency))
        If smoothTransparencyControl IsNot Nothing Then
            dynamicTransparency = smoothTransparencyControl.Value
        End If
        arrowOutlineScale = CInt(numOutline.Value)
        DynamicCorner = Math.Max(DynamicCornerMinValue, Math.Min(DynamicCornerMaxValue, My.Settings.MIDynamicRad))
        DynamicMargin = numDynMargin.Value
        DynamicMinZoom = numDynMinZoom.Value
        DynamicMaxZoom = numDynMaxZoom.Value
        If My.Settings.MIDotType = "Arrow" Then
            cbArrow.Checked = True
        Else
            cbArrow.Checked = False
        End If
    End Sub



    Private Sub cbCircle_CheckedChanged(sender As Object, e As EventArgs) Handles cbCircle.CheckedChanged
        circle = cbCircle.Checked
    End Sub



    Private Sub numLegMargin_ValueChanged(sender As Object, e As EventArgs) Handles numLegMargin.ValueChanged
        LegMargin = numLegMargin.Value
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SetSettings()
        FrmAdj.UpdateWithSettings()
    End Sub

    Private Sub cbArrow_CheckedChanged(sender As Object, e As EventArgs) Handles cbArrow.CheckedChanged
        If cbArrow.Checked Then
            DotType = "Arrow"
        Else
            DotType = "Dot"
        End If
    End Sub



    Private Sub cbFeather_CheckedChanged(sender As Object, e As EventArgs) Handles cbFeather.CheckedChanged
        feather = cbFeather.Checked
    End Sub

    Private Sub cbTailSpeed_CheckedChanged(sender As Object, e As EventArgs) Handles cbTailSpeed.CheckedChanged
        tailSpeedColors = cbTailSpeed.Checked
    End Sub



    Private Sub numArrowBarb_ValueChanged(sender As Object, e As EventArgs) Handles numArrowBarb.ValueChanged
        ArrowBarb = numArrowBarb.Value
    End Sub

    Private Sub numArrowWidth_ValueChanged(sender As Object, e As EventArgs) Handles numArrowWidth.ValueChanged
        ArrowWidth = numArrowWidth.Value
    End Sub

    Private Sub numZoomZoom_ValueChanged(sender As Object, e As EventArgs) Handles numZoomZoom.ValueChanged
        ZoomZoom = numZoomZoom.Value
    End Sub

    Private Sub numPaceFast_ValueChanged(sender As Object, e As EventArgs) Handles numPaceFast.ValueChanged
        paceFastMinPerKm = numPaceFast.Value
    End Sub

    Private Sub numPaceSlow_ValueChanged(sender As Object, e As EventArgs) Handles numPaceSlow.ValueChanged
        paceSlowMinPerKm = numPaceSlow.Value
    End Sub

    Private Sub numTransparent_ValueChanged(sender As Object, e As EventArgs) Handles numTransparent.ValueChanged
        tailTransparency = numTransparent.Value
    End Sub

    Private Sub numZoomTransp_ValueChanged(sender As Object, e As EventArgs) Handles numZoomTransp.ValueChanged
        zoomTransparency = numZoomTransp.Value
    End Sub

    Private Sub numLegTransp_ValueChanged(sender As Object, e As EventArgs) Handles numLegTransp.ValueChanged
        legTransparency = numLegTransp.Value
    End Sub

    Private Sub DynamicTransparency_ValueChanged(sender As Object, e As EventArgs)
        Dim numericSender = TryCast(sender, NumericUpDown)
        If numericSender IsNot Nothing Then
            dynamicTransparency = numericSender.Value
        End If
    End Sub

    Private Sub numOutline_ValueChanged(sender As Object, e As EventArgs) Handles numOutline.ValueChanged
        arrowOutlineScale = CInt(numOutline.Value)
    End Sub

    Private Sub txtZCorner_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtZCorner.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True ' Prevent the character from being entered
        End If
    End Sub

    Private Sub txtLCorner_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtLCorner.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True ' Prevent the character from being entered
        End If
    End Sub

    Private Sub txtZCorner_TextChanged(sender As Object, e As EventArgs) Handles txtZCorner.TextChanged
        If IsNumeric(txtZCorner.Text) Then
            ZoomCorner = txtZCorner.Text
        Else
            ZoomCorner = 5
        End If

    End Sub

    Private Sub txtLCorner_TextChanged(sender As Object, e As EventArgs) Handles txtLCorner.TextChanged
        If IsNumeric(txtLCorner.Text) Then
            LegCorner = CInt(txtLCorner.Text)
        Else
            LegCorner = 5
        End If
    End Sub

    Public Sub SetSettings()
        My.Settings.MITailRatio = ratio
        My.Settings.MILegRad = LegCorner
        My.Settings.MILegMargin = LegMargin
        My.Settings.MIZoomRad = ZoomCorner
        My.Settings.MIZoomCircle = circle
        My.Settings.MIDotColor = dotcolor
        My.Settings.MIDotSize = dotsize
        My.Settings.MITailDuration = tailduration
        My.Settings.MITailColor = tailcolor
        My.Settings.MIFrameWidth = framewidth
        My.Settings.MIFrameColor = framecolor
        My.Settings.MIZoomZoom = ZoomZoom
        My.Settings.MIDynamicRad = CInt(Math.Max(DynamicCornerMinValue, Math.Min(DynamicCornerMaxValue, DynamicCorner)))
        My.Settings.MIDynamicMargin = CInt(Math.Max(numDynMargin.Minimum, Math.Min(numDynMargin.Maximum, DynamicMargin)))
        My.Settings.MIDynamicMinZoom = CInt(Math.Max(numDynMinZoom.Minimum, Math.Min(numDynMinZoom.Maximum, DynamicMinZoom)))
        My.Settings.MIDynamicMaxZoom = CInt(Math.Max(numDynMaxZoom.Minimum, Math.Min(numDynMaxZoom.Maximum, DynamicMaxZoom)))
        My.Settings.MIArrowWidth = ArrowWidth
        My.Settings.MIArrowBarb = ArrowBarb
        My.Settings.MIDotType = DotType
        My.Settings.MIFrameFeather = feather
        My.Settings.MITailUseSpeedColors = tailSpeedColors
        My.Settings.MIPaceFastMinPerKm = CDbl(paceFastMinPerKm)
        My.Settings.MIPaceSlowMinPerKm = CDbl(paceSlowMinPerKm)
        My.Settings.MITailTransparency = CDbl(tailTransparency)
        My.Settings.MIZoomTransparency = zoomTransparency
        My.Settings.MILegTransparency = legTransparency
        My.Settings.MIDynamicTransparency = dynamicTransparency
        My.Settings.MIArrowOutlineScale = Math.Max(1, arrowOutlineScale)
        My.Settings.MIDynamicZoom = CDbl(My.Settings.MIDynamicMinZoom) / 100.0R
    End Sub

    Private Function ResolveDynamicTransparencyControl() As NumericUpDown
        If _dynamicTransparencyControl IsNot Nothing Then
            Return _dynamicTransparencyControl
        End If

        For Each controlName In _dynamicTransparencyControlNames
            Dim foundControl = FindControlRecursive(Me, controlName)
            If TypeOf foundControl Is NumericUpDown Then
                _dynamicTransparencyControl = DirectCast(foundControl, NumericUpDown)
                AddHandler _dynamicTransparencyControl.ValueChanged, AddressOf DynamicTransparency_ValueChanged
                Return _dynamicTransparencyControl
            End If
        Next

        Return Nothing
    End Function

    Private Function FindControlRecursive(parent As Control, controlName As String) As Control
        For Each child As Control In parent.Controls
            If String.Equals(child.Name, controlName, StringComparison.OrdinalIgnoreCase) Then
                Return child
            End If

            Dim nestedMatch = FindControlRecursive(child, controlName)
            If nestedMatch IsNot Nothing Then
                Return nestedMatch
            End If
        Next

        Return Nothing
    End Function
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SetSettings()
        FrmAdj.UpdateWithSettings()
        My.Settings.Save()
        Me.Close()
    End Sub

    Private Sub NumFrameSize_ValueChanged(sender As Object, e As EventArgs) Handles NumFrameSize.ValueChanged
        framewidth = NumFrameSize.Value
    End Sub

    Private Sub txtDynCorner_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDynCorner.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtDynCorner_TextChanged(sender As Object, e As EventArgs) Handles txtDynCorner.TextChanged
        If IsNumeric(txtDynCorner.Text) Then
            DynamicCorner = Math.Max(DynamicCornerMinValue, Math.Min(DynamicCornerMaxValue, CInt(txtDynCorner.Text)))
        Else
            DynamicCorner = DynamicCornerMinValue
        End If
    End Sub

    Private Sub numDynMargin_ValueChanged(sender As Object, e As EventArgs) Handles numDynMargin.ValueChanged
        DynamicMargin = numDynMargin.Value
    End Sub

    Private Sub numDynMinZoom_ValueChanged(sender As Object, e As EventArgs) Handles numDynMinZoom.ValueChanged
        If _syncingDynamicZoom Then Return
        _syncingDynamicZoom = True
        If numDynMinZoom.Value > numDynMaxZoom.Value Then
            numDynMaxZoom.Value = numDynMinZoom.Value
        End If
        DynamicMinZoom = numDynMinZoom.Value
        DynamicMaxZoom = numDynMaxZoom.Value
        _syncingDynamicZoom = False
    End Sub

    Private Sub numDynMaxZoom_ValueChanged(sender As Object, e As EventArgs) Handles numDynMaxZoom.ValueChanged
        If _syncingDynamicZoom Then Return
        _syncingDynamicZoom = True
        If numDynMaxZoom.Value < numDynMinZoom.Value Then
            numDynMinZoom.Value = numDynMaxZoom.Value
        End If
        DynamicMinZoom = numDynMinZoom.Value
        DynamicMaxZoom = numDynMaxZoom.Value
        _syncingDynamicZoom = False
    End Sub
End Class
