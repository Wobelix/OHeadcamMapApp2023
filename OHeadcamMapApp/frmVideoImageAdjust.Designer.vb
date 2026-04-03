<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmVideoImageAdjust
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmVideoImageAdjust))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.TrackBar1 = New System.Windows.Forms.TrackBar()
        Me.txtVideofile = New System.Windows.Forms.TextBox()
        Me.btnVideoSelect = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.lblTValue = New System.Windows.Forms.Label()
        Me.Splitter1 = New System.Windows.Forms.Splitter()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.BtnPlus = New System.Windows.Forms.Button()
        Me.btnMinus = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.grpAdjust = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.numZoom = New System.Windows.Forms.NumericUpDown()
        Me.cbLensDist = New System.Windows.Forms.CheckBox()
        Me.cbNoise = New System.Windows.Forms.CheckBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.numHue = New System.Windows.Forms.NumericUpDown()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.numGamma = New System.Windows.Forms.NumericUpDown()
        Me.numRot = New System.Windows.Forms.NumericUpDown()
        Me.numSat = New System.Windows.Forms.NumericUpDown()
        Me.cbColorImp = New System.Windows.Forms.CheckBox()
        Me.nunSharp = New System.Windows.Forms.NumericUpDown()
        Me.numContr = New System.Windows.Forms.NumericUpDown()
        Me.numBright = New System.Windows.Forms.NumericUpDown()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpAdjust.SuspendLayout()
        CType(Me.numZoom, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numHue, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numGamma, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numRot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numSat, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nunSharp, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numContr, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numBright, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.SystemColors.Desktop
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.PictureBox1, "PictureBox1")
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.TabStop = False
        '
        'TrackBar1
        '
        resources.ApplyResources(Me.TrackBar1, "TrackBar1")
        Me.TrackBar1.LargeChange = 1
        Me.TrackBar1.Maximum = 100
        Me.TrackBar1.Name = "TrackBar1"
        '
        'txtVideofile
        '
        resources.ApplyResources(Me.txtVideofile, "txtVideofile")
        Me.txtVideofile.Name = "txtVideofile"
        '
        'btnVideoSelect
        '
        resources.ApplyResources(Me.btnVideoSelect, "btnVideoSelect")
        Me.btnVideoSelect.Name = "btnVideoSelect"
        Me.btnVideoSelect.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Timer1
        '
        Me.Timer1.Interval = 500
        '
        'lblTValue
        '
        resources.ApplyResources(Me.lblTValue, "lblTValue")
        Me.lblTValue.Name = "lblTValue"
        '
        'Splitter1
        '
        resources.ApplyResources(Me.Splitter1, "Splitter1")
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.TabStop = False
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'BtnPlus
        '
        resources.ApplyResources(Me.BtnPlus, "BtnPlus")
        Me.BtnPlus.Name = "BtnPlus"
        Me.BtnPlus.UseVisualStyleBackColor = True
        '
        'btnMinus
        '
        resources.ApplyResources(Me.btnMinus, "btnMinus")
        Me.btnMinus.Name = "btnMinus"
        Me.btnMinus.UseVisualStyleBackColor = True
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        '
        'grpAdjust
        '
        Me.grpAdjust.Controls.Add(Me.Label1)
        Me.grpAdjust.Controls.Add(Me.numZoom)
        Me.grpAdjust.Controls.Add(Me.cbLensDist)
        Me.grpAdjust.Controls.Add(Me.cbNoise)
        Me.grpAdjust.Controls.Add(Me.Label8)
        Me.grpAdjust.Controls.Add(Me.numHue)
        Me.grpAdjust.Controls.Add(Me.Label7)
        Me.grpAdjust.Controls.Add(Me.numGamma)
        Me.grpAdjust.Controls.Add(Me.Label2)
        Me.grpAdjust.Controls.Add(Me.Label6)
        Me.grpAdjust.Controls.Add(Me.numRot)
        Me.grpAdjust.Controls.Add(Me.numSat)
        Me.grpAdjust.Controls.Add(Me.cbColorImp)
        Me.grpAdjust.Controls.Add(Me.Label5)
        Me.grpAdjust.Controls.Add(Me.nunSharp)
        Me.grpAdjust.Controls.Add(Me.numContr)
        Me.grpAdjust.Controls.Add(Me.Label3)
        Me.grpAdjust.Controls.Add(Me.Label4)
        Me.grpAdjust.Controls.Add(Me.numBright)
        resources.ApplyResources(Me.grpAdjust, "grpAdjust")
        Me.grpAdjust.Name = "grpAdjust"
        Me.grpAdjust.TabStop = False
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'numZoom
        '
        Me.numZoom.DataBindings.Add(New System.Windows.Forms.Binding("Value", Global.OHeadcamMapApp.My.MySettings.Default, "VidAdj_Zoom", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.numZoom.Increment = New Decimal(New Integer() {5, 0, 0, 0})
        resources.ApplyResources(Me.numZoom, "numZoom")
        Me.numZoom.Name = "numZoom"
        Me.numZoom.Value = Global.OHeadcamMapApp.My.MySettings.Default.VidAdj_Zoom
        '
        'cbLensDist
        '
        resources.ApplyResources(Me.cbLensDist, "cbLensDist")
        Me.cbLensDist.Checked = Global.OHeadcamMapApp.My.MySettings.Default.VidAdjLensDist
        Me.cbLensDist.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.OHeadcamMapApp.My.MySettings.Default, "VidAdjLensDist", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.cbLensDist.Name = "cbLensDist"
        Me.cbLensDist.UseVisualStyleBackColor = True
        '
        'cbNoise
        '
        resources.ApplyResources(Me.cbNoise, "cbNoise")
        Me.cbNoise.Checked = Global.OHeadcamMapApp.My.MySettings.Default.VidAdjNoise
        Me.cbNoise.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.OHeadcamMapApp.My.MySettings.Default, "VidAdjNoise", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.cbNoise.Name = "cbNoise"
        Me.cbNoise.UseVisualStyleBackColor = True
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Name = "Label8"
        '
        'numHue
        '
        Me.numHue.DataBindings.Add(New System.Windows.Forms.Binding("Value", Global.OHeadcamMapApp.My.MySettings.Default, "VidAdjHue", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.numHue, "numHue")
        Me.numHue.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.numHue.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.numHue.Name = "numHue"
        Me.numHue.Value = Global.OHeadcamMapApp.My.MySettings.Default.VidAdjHue
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        '
        'numGamma
        '
        Me.numGamma.DataBindings.Add(New System.Windows.Forms.Binding("Value", Global.OHeadcamMapApp.My.MySettings.Default, "VidAdjGamma", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.numGamma, "numGamma")
        Me.numGamma.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.numGamma.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.numGamma.Name = "numGamma"
        Me.numGamma.Value = Global.OHeadcamMapApp.My.MySettings.Default.VidAdjGamma
        '
        'numRot
        '
        Me.numRot.DataBindings.Add(New System.Windows.Forms.Binding("Value", Global.OHeadcamMapApp.My.MySettings.Default, "VidAdj_Rot", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.numRot, "numRot")
        Me.numRot.Maximum = New Decimal(New Integer() {180, 0, 0, 0})
        Me.numRot.Minimum = New Decimal(New Integer() {180, 0, 0, -2147483648})
        Me.numRot.Name = "numRot"
        Me.numRot.Value = Global.OHeadcamMapApp.My.MySettings.Default.VidAdj_Rot
        '
        'numSat
        '
        Me.numSat.DataBindings.Add(New System.Windows.Forms.Binding("Value", Global.OHeadcamMapApp.My.MySettings.Default, "VidAdjSatur", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.numSat, "numSat")
        Me.numSat.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.numSat.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.numSat.Name = "numSat"
        Me.numSat.Value = Global.OHeadcamMapApp.My.MySettings.Default.VidAdjSatur
        '
        'cbColorImp
        '
        resources.ApplyResources(Me.cbColorImp, "cbColorImp")
        Me.cbColorImp.Checked = Global.OHeadcamMapApp.My.MySettings.Default.VidAdjColImpr
        Me.cbColorImp.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.OHeadcamMapApp.My.MySettings.Default, "VidAdjColImpr", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.cbColorImp.Name = "cbColorImp"
        Me.cbColorImp.UseVisualStyleBackColor = True
        '
        'nunSharp
        '
        Me.nunSharp.DataBindings.Add(New System.Windows.Forms.Binding("Value", Global.OHeadcamMapApp.My.MySettings.Default, "VidAdjSharp", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.nunSharp, "nunSharp")
        Me.nunSharp.Maximum = New Decimal(New Integer() {9, 0, 0, 0})
        Me.nunSharp.Name = "nunSharp"
        Me.nunSharp.Value = Global.OHeadcamMapApp.My.MySettings.Default.VidAdjSharp
        '
        'numContr
        '
        Me.numContr.DataBindings.Add(New System.Windows.Forms.Binding("Value", Global.OHeadcamMapApp.My.MySettings.Default, "VidAdjContr", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.numContr, "numContr")
        Me.numContr.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.numContr.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.numContr.Name = "numContr"
        Me.numContr.Value = Global.OHeadcamMapApp.My.MySettings.Default.VidAdjContr
        '
        'numBright
        '
        Me.numBright.DataBindings.Add(New System.Windows.Forms.Binding("Value", Global.OHeadcamMapApp.My.MySettings.Default, "VidAdjBright", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.numBright, "numBright")
        Me.numBright.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.numBright.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.numBright.Name = "numBright"
        Me.numBright.Value = Global.OHeadcamMapApp.My.MySettings.Default.VidAdjBright
        '
        'btnSave
        '
        resources.ApplyResources(Me.btnSave, "btnSave")
        Me.btnSave.Name = "btnSave"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'frmVideoImageAdjust
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.grpAdjust)
        Me.Controls.Add(Me.btnMinus)
        Me.Controls.Add(Me.BtnPlus)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.lblTValue)
        Me.Controls.Add(Me.txtVideofile)
        Me.Controls.Add(Me.btnVideoSelect)
        Me.Controls.Add(Me.TrackBar1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Name = "frmVideoImageAdjust"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpAdjust.ResumeLayout(False)
        Me.grpAdjust.PerformLayout()
        CType(Me.numZoom, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numHue, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numGamma, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numRot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numSat, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nunSharp, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numContr, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numBright, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents TrackBar1 As TrackBar
    Friend WithEvents txtVideofile As TextBox
    Friend WithEvents btnVideoSelect As Button
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents Timer1 As Timer
    Friend WithEvents lblTValue As Label
    Friend WithEvents Splitter1 As Splitter
    Friend WithEvents numRot As NumericUpDown
    Friend WithEvents Label2 As Label
    Friend WithEvents cbColorImp As CheckBox
    Friend WithEvents BtnPlus As Button
    Friend WithEvents btnMinus As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents nunSharp As NumericUpDown
    Friend WithEvents Label4 As Label
    Friend WithEvents numBright As NumericUpDown
    Friend WithEvents Label5 As Label
    Friend WithEvents numContr As NumericUpDown
    Friend WithEvents Label6 As Label
    Friend WithEvents numSat As NumericUpDown
    Friend WithEvents grpAdjust As GroupBox
    Friend WithEvents btnSave As Button
    Friend WithEvents Label7 As Label
    Friend WithEvents numGamma As NumericUpDown
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents Label8 As Label
    Friend WithEvents numHue As NumericUpDown
    Friend WithEvents cbNoise As CheckBox
    Friend WithEvents cbLensDist As CheckBox
    Friend WithEvents Label1 As Label
    Friend WithEvents numZoom As NumericUpDown
End Class
