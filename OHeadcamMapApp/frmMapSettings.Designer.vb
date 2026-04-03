<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMapSettings
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMapSettings))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.bFrameColor = New System.Windows.Forms.Button()
        Me.bDotColor = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.bTailColor = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.numArrowWidth = New System.Windows.Forms.NumericUpDown()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.numArrowBarb = New System.Windows.Forms.NumericUpDown()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.cbArrow = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.NumDotSize = New System.Windows.Forms.NumericUpDown()
        Me.numTailDuration = New System.Windows.Forms.NumericUpDown()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.numTailRatio = New System.Windows.Forms.NumericUpDown()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.cbFeather = New System.Windows.Forms.CheckBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.NumFrameSize = New System.Windows.Forms.NumericUpDown()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.cbCircle = New System.Windows.Forms.CheckBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.numZoomZoom = New System.Windows.Forms.NumericUpDown()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txtZCorner = New System.Windows.Forms.TextBox()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.txtLCorner = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.numLegMargin = New System.Windows.Forms.NumericUpDown()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.ColorDialogFrame = New System.Windows.Forms.ColorDialog()
        Me.DotColorDialog = New System.Windows.Forms.ColorDialog()
        Me.TailColorDialog = New System.Windows.Forms.ColorDialog()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox1.SuspendLayout()
        CType(Me.numArrowWidth, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numArrowBarb, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumDotSize, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numTailDuration, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numTailRatio, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.NumFrameSize, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox4.SuspendLayout()
        CType(Me.numZoomZoom, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox5.SuspendLayout()
        CType(Me.numLegMargin, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        Me.ToolTip1.SetToolTip(Me.Label1, resources.GetString("Label1.ToolTip"))
        '
        'bFrameColor
        '
        resources.ApplyResources(Me.bFrameColor, "bFrameColor")
        Me.bFrameColor.Name = "bFrameColor"
        Me.ToolTip1.SetToolTip(Me.bFrameColor, resources.GetString("bFrameColor.ToolTip"))
        Me.bFrameColor.UseVisualStyleBackColor = True
        '
        'bDotColor
        '
        resources.ApplyResources(Me.bDotColor, "bDotColor")
        Me.bDotColor.Name = "bDotColor"
        Me.ToolTip1.SetToolTip(Me.bDotColor, resources.GetString("bDotColor.ToolTip"))
        Me.bDotColor.UseVisualStyleBackColor = True
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        Me.ToolTip1.SetToolTip(Me.Label4, resources.GetString("Label4.ToolTip"))
        '
        'bTailColor
        '
        resources.ApplyResources(Me.bTailColor, "bTailColor")
        Me.bTailColor.Name = "bTailColor"
        Me.ToolTip1.SetToolTip(Me.bTailColor, resources.GetString("bTailColor.ToolTip"))
        Me.bTailColor.UseVisualStyleBackColor = True
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        Me.ToolTip1.SetToolTip(Me.Label6, resources.GetString("Label6.ToolTip"))
        '
        'GroupBox1
        '
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Controls.Add(Me.numArrowWidth)
        Me.GroupBox1.Controls.Add(Me.Label16)
        Me.GroupBox1.Controls.Add(Me.numArrowBarb)
        Me.GroupBox1.Controls.Add(Me.Label15)
        Me.GroupBox1.Controls.Add(Me.cbArrow)
        Me.GroupBox1.Controls.Add(Me.bDotColor)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.NumDotSize)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox1, resources.GetString("GroupBox1.ToolTip"))
        '
        'numArrowWidth
        '
        resources.ApplyResources(Me.numArrowWidth, "numArrowWidth")
        Me.numArrowWidth.DecimalPlaces = 1
        Me.numArrowWidth.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.numArrowWidth.Name = "numArrowWidth"
        Me.ToolTip1.SetToolTip(Me.numArrowWidth, resources.GetString("numArrowWidth.ToolTip"))
        Me.numArrowWidth.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label16
        '
        resources.ApplyResources(Me.Label16, "Label16")
        Me.Label16.Name = "Label16"
        Me.ToolTip1.SetToolTip(Me.Label16, resources.GetString("Label16.ToolTip"))
        '
        'numArrowBarb
        '
        resources.ApplyResources(Me.numArrowBarb, "numArrowBarb")
        Me.numArrowBarb.DecimalPlaces = 1
        Me.numArrowBarb.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.numArrowBarb.Name = "numArrowBarb"
        Me.ToolTip1.SetToolTip(Me.numArrowBarb, resources.GetString("numArrowBarb.ToolTip"))
        Me.numArrowBarb.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label15
        '
        resources.ApplyResources(Me.Label15, "Label15")
        Me.Label15.Name = "Label15"
        Me.ToolTip1.SetToolTip(Me.Label15, resources.GetString("Label15.ToolTip"))
        '
        'cbArrow
        '
        resources.ApplyResources(Me.cbArrow, "cbArrow")
        Me.cbArrow.Name = "cbArrow"
        Me.ToolTip1.SetToolTip(Me.cbArrow, resources.GetString("cbArrow.ToolTip"))
        Me.cbArrow.UseVisualStyleBackColor = True
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.BackColor = Global.OHeadcamMapApp.My.MySettings.Default.MIDotColor
        Me.Label3.DataBindings.Add(New System.Windows.Forms.Binding("BackColor", Global.OHeadcamMapApp.My.MySettings.Default, "MIDotColor", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.Label3.Name = "Label3"
        Me.ToolTip1.SetToolTip(Me.Label3, resources.GetString("Label3.ToolTip"))
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Name = "Label8"
        Me.ToolTip1.SetToolTip(Me.Label8, resources.GetString("Label8.ToolTip"))
        '
        'NumDotSize
        '
        resources.ApplyResources(Me.NumDotSize, "NumDotSize")
        Me.NumDotSize.Name = "NumDotSize"
        Me.ToolTip1.SetToolTip(Me.NumDotSize, resources.GetString("NumDotSize.ToolTip"))
        '
        'numTailDuration
        '
        resources.ApplyResources(Me.numTailDuration, "numTailDuration")
        Me.numTailDuration.Name = "numTailDuration"
        Me.ToolTip1.SetToolTip(Me.numTailDuration, resources.GetString("numTailDuration.ToolTip"))
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        Me.ToolTip1.SetToolTip(Me.Label7, resources.GetString("Label7.ToolTip"))
        '
        'Label9
        '
        resources.ApplyResources(Me.Label9, "Label9")
        Me.Label9.Name = "Label9"
        Me.ToolTip1.SetToolTip(Me.Label9, resources.GetString("Label9.ToolTip"))
        '
        'numTailRatio
        '
        resources.ApplyResources(Me.numTailRatio, "numTailRatio")
        Me.numTailRatio.Name = "numTailRatio"
        Me.ToolTip1.SetToolTip(Me.numTailRatio, resources.GetString("numTailRatio.ToolTip"))
        '
        'Button1
        '
        resources.ApplyResources(Me.Button1, "Button1")
        Me.Button1.Name = "Button1"
        Me.ToolTip1.SetToolTip(Me.Button1, resources.GetString("Button1.ToolTip"))
        Me.Button1.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        resources.ApplyResources(Me.GroupBox2, "GroupBox2")
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.numTailDuration)
        Me.GroupBox2.Controls.Add(Me.bTailColor)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.numTailRatio)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox2, resources.GetString("GroupBox2.ToolTip"))
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.BackColor = Global.OHeadcamMapApp.My.MySettings.Default.MITailColor
        Me.Label5.DataBindings.Add(New System.Windows.Forms.Binding("BackColor", Global.OHeadcamMapApp.My.MySettings.Default, "MITailColor", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.Label5.Name = "Label5"
        Me.ToolTip1.SetToolTip(Me.Label5, resources.GetString("Label5.ToolTip"))
        '
        'GroupBox3
        '
        resources.ApplyResources(Me.GroupBox3, "GroupBox3")
        Me.GroupBox3.Controls.Add(Me.cbFeather)
        Me.GroupBox3.Controls.Add(Me.Label10)
        Me.GroupBox3.Controls.Add(Me.NumFrameSize)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Controls.Add(Me.bFrameColor)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox3, resources.GetString("GroupBox3.ToolTip"))
        '
        'cbFeather
        '
        resources.ApplyResources(Me.cbFeather, "cbFeather")
        Me.cbFeather.Name = "cbFeather"
        Me.ToolTip1.SetToolTip(Me.cbFeather, resources.GetString("cbFeather.ToolTip"))
        Me.cbFeather.UseVisualStyleBackColor = True
        '
        'Label10
        '
        resources.ApplyResources(Me.Label10, "Label10")
        Me.Label10.Name = "Label10"
        Me.ToolTip1.SetToolTip(Me.Label10, resources.GetString("Label10.ToolTip"))
        '
        'NumFrameSize
        '
        resources.ApplyResources(Me.NumFrameSize, "NumFrameSize")
        Me.NumFrameSize.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
        Me.NumFrameSize.Name = "NumFrameSize"
        Me.ToolTip1.SetToolTip(Me.NumFrameSize, resources.GetString("NumFrameSize.ToolTip"))
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.BackColor = Global.OHeadcamMapApp.My.MySettings.Default.MIFrameColor
        Me.Label2.DataBindings.Add(New System.Windows.Forms.Binding("BackColor", Global.OHeadcamMapApp.My.MySettings.Default, "MIFrameColor", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.Label2.Name = "Label2"
        Me.ToolTip1.SetToolTip(Me.Label2, resources.GetString("Label2.ToolTip"))
        '
        'Label11
        '
        resources.ApplyResources(Me.Label11, "Label11")
        Me.Label11.Name = "Label11"
        Me.ToolTip1.SetToolTip(Me.Label11, resources.GetString("Label11.ToolTip"))
        '
        'cbCircle
        '
        resources.ApplyResources(Me.cbCircle, "cbCircle")
        Me.cbCircle.Name = "cbCircle"
        Me.ToolTip1.SetToolTip(Me.cbCircle, resources.GetString("cbCircle.ToolTip"))
        Me.cbCircle.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        resources.ApplyResources(Me.GroupBox4, "GroupBox4")
        Me.GroupBox4.Controls.Add(Me.numZoomZoom)
        Me.GroupBox4.Controls.Add(Me.Label14)
        Me.GroupBox4.Controls.Add(Me.txtZCorner)
        Me.GroupBox4.Controls.Add(Me.Label11)
        Me.GroupBox4.Controls.Add(Me.cbCircle)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox4, resources.GetString("GroupBox4.ToolTip"))
        '
        'numZoomZoom
        '
        resources.ApplyResources(Me.numZoomZoom, "numZoomZoom")
        Me.numZoomZoom.DecimalPlaces = 1
        Me.numZoomZoom.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.numZoomZoom.Name = "numZoomZoom"
        Me.ToolTip1.SetToolTip(Me.numZoomZoom, resources.GetString("numZoomZoom.ToolTip"))
        Me.numZoomZoom.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label14
        '
        resources.ApplyResources(Me.Label14, "Label14")
        Me.Label14.Name = "Label14"
        Me.ToolTip1.SetToolTip(Me.Label14, resources.GetString("Label14.ToolTip"))
        '
        'txtZCorner
        '
        resources.ApplyResources(Me.txtZCorner, "txtZCorner")
        Me.txtZCorner.Name = "txtZCorner"
        Me.ToolTip1.SetToolTip(Me.txtZCorner, resources.GetString("txtZCorner.ToolTip"))
        '
        'GroupBox5
        '
        resources.ApplyResources(Me.GroupBox5, "GroupBox5")
        Me.GroupBox5.Controls.Add(Me.txtLCorner)
        Me.GroupBox5.Controls.Add(Me.Label13)
        Me.GroupBox5.Controls.Add(Me.numLegMargin)
        Me.GroupBox5.Controls.Add(Me.Label12)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox5, resources.GetString("GroupBox5.ToolTip"))
        '
        'txtLCorner
        '
        resources.ApplyResources(Me.txtLCorner, "txtLCorner")
        Me.txtLCorner.Name = "txtLCorner"
        Me.ToolTip1.SetToolTip(Me.txtLCorner, resources.GetString("txtLCorner.ToolTip"))
        '
        'Label13
        '
        resources.ApplyResources(Me.Label13, "Label13")
        Me.Label13.Name = "Label13"
        Me.ToolTip1.SetToolTip(Me.Label13, resources.GetString("Label13.ToolTip"))
        '
        'numLegMargin
        '
        resources.ApplyResources(Me.numLegMargin, "numLegMargin")
        Me.numLegMargin.Maximum = New Decimal(New Integer() {600, 0, 0, 0})
        Me.numLegMargin.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numLegMargin.Name = "numLegMargin"
        Me.ToolTip1.SetToolTip(Me.numLegMargin, resources.GetString("numLegMargin.ToolTip"))
        Me.numLegMargin.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label12
        '
        resources.ApplyResources(Me.Label12, "Label12")
        Me.Label12.Name = "Label12"
        Me.ToolTip1.SetToolTip(Me.Label12, resources.GetString("Label12.ToolTip"))
        '
        'Button2
        '
        resources.ApplyResources(Me.Button2, "Button2")
        Me.Button2.Name = "Button2"
        Me.ToolTip1.SetToolTip(Me.Button2, resources.GetString("Button2.ToolTip"))
        Me.Button2.UseVisualStyleBackColor = True
        '
        'ColorDialogFrame
        '
        Me.ColorDialogFrame.Color = Global.OHeadcamMapApp.My.MySettings.Default.MIFrameColor
        '
        'DotColorDialog
        '
        Me.DotColorDialog.Color = Global.OHeadcamMapApp.My.MySettings.Default.MIDotColor
        '
        'TailColorDialog
        '
        Me.TailColorDialog.Color = Global.OHeadcamMapApp.My.MySettings.Default.MITailColor
        '
        'frmMapSettings
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "frmMapSettings"
        Me.ToolTip1.SetToolTip(Me, resources.GetString("$this.ToolTip"))
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.numArrowWidth, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numArrowBarb, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumDotSize, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numTailDuration, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numTailRatio, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.NumFrameSize, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.numZoomZoom, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        CType(Me.numLegMargin, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ColorDialogFrame As ColorDialog
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents bFrameColor As Button
    Friend WithEvents bDotColor As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents bTailColor As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents DotColorDialog As ColorDialog
    Friend WithEvents TailColorDialog As ColorDialog
    Friend WithEvents numTailDuration As NumericUpDown
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents NumDotSize As NumericUpDown
    Friend WithEvents Label9 As Label
    Friend WithEvents numTailRatio As NumericUpDown
    Friend WithEvents Button1 As Button
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents Label10 As Label
    Friend WithEvents NumFrameSize As NumericUpDown
    Friend WithEvents Label11 As Label
    Friend WithEvents cbCircle As CheckBox
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents Label13 As Label
    Friend WithEvents numLegMargin As NumericUpDown
    Friend WithEvents Label12 As Label
    Friend WithEvents Button2 As Button
    Friend WithEvents txtZCorner As TextBox
    Friend WithEvents txtLCorner As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents numZoomZoom As NumericUpDown
    Friend WithEvents numArrowBarb As NumericUpDown
    Friend WithEvents Label15 As Label
    Friend WithEvents cbArrow As CheckBox
    Friend WithEvents numArrowWidth As NumericUpDown
    Friend WithEvents Label16 As Label
    Friend WithEvents cbFeather As CheckBox
    Friend WithEvents ToolTip1 As ToolTip
End Class
