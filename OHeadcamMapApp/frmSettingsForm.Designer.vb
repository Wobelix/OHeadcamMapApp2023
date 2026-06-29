<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSettings
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSettings))
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtVideoEncoderInfo = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbPreset = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.txtSettingSbSCode = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.btnSelectFolder = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.rbImgMiddle = New System.Windows.Forms.RadioButton()
        Me.rbImgRight = New System.Windows.Forms.RadioButton()
        Me.rbImgLeft = New System.Windows.Forms.RadioButton()
        Me.btnReset = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.numSmooth = New System.Windows.Forms.NumericUpDown()
        Me.txtVideoWorkfolder = New System.Windows.Forms.TextBox()
        Me.txtCRF = New System.Windows.Forms.TextBox()
        Me.txtFPS = New System.Windows.Forms.TextBox()
        Me.cmbOutputFormat = New System.Windows.Forms.ComboBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.numSmooth, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label12
        '
        resources.ApplyResources(Me.Label12, "Label12")
        Me.Label12.Name = "Label12"
        Me.ToolTip1.SetToolTip(Me.Label12, resources.GetString("Label12.ToolTip"))
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        Me.ToolTip1.SetToolTip(Me.Label1, resources.GetString("Label1.ToolTip"))
        '
        'btnSave
        '
        resources.ApplyResources(Me.btnSave, "btnSave")
        Me.btnSave.Name = "btnSave"
        Me.ToolTip1.SetToolTip(Me.btnSave, resources.GetString("btnSave.ToolTip"))
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'Label17
        '
        resources.ApplyResources(Me.Label17, "Label17")
        Me.Label17.Name = "Label17"
        Me.ToolTip1.SetToolTip(Me.Label17, resources.GetString("Label17.ToolTip"))
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        Me.ToolTip1.SetToolTip(Me.Label5, resources.GetString("Label5.ToolTip"))
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        Me.ToolTip1.SetToolTip(Me.Label6, resources.GetString("Label6.ToolTip"))
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        Me.ToolTip1.SetToolTip(Me.Label2, resources.GetString("Label2.ToolTip"))
        '
        'GroupBox1
        '
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.txtVideoEncoderInfo)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.cmbPreset)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.TextBox4)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label17)
        Me.GroupBox1.Controls.Add(Me.TextBox2)
        Me.GroupBox1.Controls.Add(Me.TextBox1)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.txtSettingSbSCode)
        Me.GroupBox1.Controls.Add(Me.TextBox3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox1, resources.GetString("GroupBox1.ToolTip"))
        '
        'Label10
        '
        resources.ApplyResources(Me.Label10, "Label10")
        Me.Label10.Name = "Label10"
        Me.ToolTip1.SetToolTip(Me.Label10, resources.GetString("Label10.ToolTip"))
        '
        'txtVideoEncoderInfo
        '
        resources.ApplyResources(Me.txtVideoEncoderInfo, "txtVideoEncoderInfo")
        Me.txtVideoEncoderInfo.Name = "txtVideoEncoderInfo"
        Me.txtVideoEncoderInfo.ReadOnly = True
        Me.ToolTip1.SetToolTip(Me.txtVideoEncoderInfo, resources.GetString("txtVideoEncoderInfo.ToolTip"))
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        Me.ToolTip1.SetToolTip(Me.Label3, resources.GetString("Label3.ToolTip"))
        '
        'cmbPreset
        '
        resources.ApplyResources(Me.cmbPreset, "cmbPreset")
        Me.cmbPreset.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "ffmpegPreset", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.cmbPreset.FormattingEnabled = True
        Me.cmbPreset.Items.AddRange(New Object() {resources.GetString("cmbPreset.Items"), resources.GetString("cmbPreset.Items1"), resources.GetString("cmbPreset.Items2"), resources.GetString("cmbPreset.Items3"), resources.GetString("cmbPreset.Items4"), resources.GetString("cmbPreset.Items5"), resources.GetString("cmbPreset.Items6"), resources.GetString("cmbPreset.Items7"), resources.GetString("cmbPreset.Items8"), resources.GetString("cmbPreset.Items9")})
        Me.cmbPreset.Name = "cmbPreset"
        Me.cmbPreset.Text = Global.OHeadcamMapApp.My.MySettings.Default.ffmpegPreset
        Me.ToolTip1.SetToolTip(Me.cmbPreset, resources.GetString("cmbPreset.ToolTip"))
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        Me.ToolTip1.SetToolTip(Me.Label4, resources.GetString("Label4.ToolTip"))
        '
        'TextBox4
        '
        resources.ApplyResources(Me.TextBox4, "TextBox4")
        Me.TextBox4.BackColor = System.Drawing.SystemColors.Info
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.ReadOnly = True
        Me.ToolTip1.SetToolTip(Me.TextBox4, resources.GetString("TextBox4.ToolTip"))
        '
        'TextBox2
        '
        resources.ApplyResources(Me.TextBox2, "TextBox2")
        Me.TextBox2.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "ffmpegVidstabDetect", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Text = Global.OHeadcamMapApp.My.MySettings.Default.ffmpegVidstabDetect
        Me.ToolTip1.SetToolTip(Me.TextBox2, resources.GetString("TextBox2.ToolTip"))
        '
        'TextBox1
        '
        resources.ApplyResources(Me.TextBox1, "TextBox1")
        Me.TextBox1.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "No_deshake_filter", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Text = Global.OHeadcamMapApp.My.MySettings.Default.No_deshake_filter
        Me.ToolTip1.SetToolTip(Me.TextBox1, resources.GetString("TextBox1.ToolTip"))
        '
        'txtSettingSbSCode
        '
        resources.ApplyResources(Me.txtSettingSbSCode, "txtSettingSbSCode")
        Me.txtSettingSbSCode.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "SettingSbSCode", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtSettingSbSCode.Name = "txtSettingSbSCode"
        Me.txtSettingSbSCode.Text = Global.OHeadcamMapApp.My.MySettings.Default.SettingSbSCode
        Me.ToolTip1.SetToolTip(Me.txtSettingSbSCode, resources.GetString("txtSettingSbSCode.ToolTip"))
        '
        'TextBox3
        '
        resources.ApplyResources(Me.TextBox3, "TextBox3")
        Me.TextBox3.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "ffmpegVidstabTransform", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Text = Global.OHeadcamMapApp.My.MySettings.Default.ffmpegVidstabTransform
        Me.ToolTip1.SetToolTip(Me.TextBox3, resources.GetString("TextBox3.ToolTip"))
        '
        'btnSelectFolder
        '
        resources.ApplyResources(Me.btnSelectFolder, "btnSelectFolder")
        Me.btnSelectFolder.Name = "btnSelectFolder"
        Me.ToolTip1.SetToolTip(Me.btnSelectFolder, resources.GetString("btnSelectFolder.ToolTip"))
        Me.btnSelectFolder.UseVisualStyleBackColor = True
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        Me.ToolTip1.SetToolTip(Me.Label7, resources.GetString("Label7.ToolTip"))
        '
        'GroupBox2
        '
        resources.ApplyResources(Me.GroupBox2, "GroupBox2")
        Me.GroupBox2.Controls.Add(Me.rbImgMiddle)
        Me.GroupBox2.Controls.Add(Me.rbImgRight)
        Me.GroupBox2.Controls.Add(Me.rbImgLeft)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox2, resources.GetString("GroupBox2.ToolTip"))
        '
        'rbImgMiddle
        '
        resources.ApplyResources(Me.rbImgMiddle, "rbImgMiddle")
        Me.rbImgMiddle.Name = "rbImgMiddle"
        Me.ToolTip1.SetToolTip(Me.rbImgMiddle, resources.GetString("rbImgMiddle.ToolTip"))
        Me.rbImgMiddle.UseVisualStyleBackColor = True
        '
        'rbImgRight
        '
        resources.ApplyResources(Me.rbImgRight, "rbImgRight")
        Me.rbImgRight.Checked = True
        Me.rbImgRight.Name = "rbImgRight"
        Me.rbImgRight.TabStop = True
        Me.ToolTip1.SetToolTip(Me.rbImgRight, resources.GetString("rbImgRight.ToolTip"))
        Me.rbImgRight.UseVisualStyleBackColor = True
        '
        'rbImgLeft
        '
        resources.ApplyResources(Me.rbImgLeft, "rbImgLeft")
        Me.rbImgLeft.Name = "rbImgLeft"
        Me.ToolTip1.SetToolTip(Me.rbImgLeft, resources.GetString("rbImgLeft.ToolTip"))
        Me.rbImgLeft.UseVisualStyleBackColor = True
        '
        'btnReset
        '
        resources.ApplyResources(Me.btnReset, "btnReset")
        Me.btnReset.Name = "btnReset"
        Me.ToolTip1.SetToolTip(Me.btnReset, resources.GetString("btnReset.ToolTip"))
        Me.btnReset.UseVisualStyleBackColor = True
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Name = "Label8"
        Me.ToolTip1.SetToolTip(Me.Label8, resources.GetString("Label8.ToolTip"))
        '
        'Label9
        '
        resources.ApplyResources(Me.Label9, "Label9")
        Me.Label9.Name = "Label9"
        Me.ToolTip1.SetToolTip(Me.Label9, resources.GetString("Label9.ToolTip"))
        '
        'numSmooth
        '
        resources.ApplyResources(Me.numSmooth, "numSmooth")
        Me.numSmooth.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
        Me.numSmooth.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numSmooth.Name = "numSmooth"
        Me.ToolTip1.SetToolTip(Me.numSmooth, resources.GetString("numSmooth.ToolTip"))
        Me.numSmooth.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'txtVideoWorkfolder
        '
        resources.ApplyResources(Me.txtVideoWorkfolder, "txtVideoWorkfolder")
        Me.txtVideoWorkfolder.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "VideoWorkFolder", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtVideoWorkfolder.Name = "txtVideoWorkfolder"
        Me.txtVideoWorkfolder.Text = Global.OHeadcamMapApp.My.MySettings.Default.VideoWorkFolder
        Me.ToolTip1.SetToolTip(Me.txtVideoWorkfolder, resources.GetString("txtVideoWorkfolder.ToolTip"))
        '
        'txtCRF
        '
        resources.ApplyResources(Me.txtCRF, "txtCRF")
        Me.txtCRF.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "ffmpegCRF", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtCRF.Name = "txtCRF"
        Me.txtCRF.Text = Global.OHeadcamMapApp.My.MySettings.Default.ffmpegCRF
        Me.ToolTip1.SetToolTip(Me.txtCRF, resources.GetString("txtCRF.ToolTip"))
        '
        'txtFPS
        '
        resources.ApplyResources(Me.txtFPS, "txtFPS")
        Me.txtFPS.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "ffmpegOutFps", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtFPS.Name = "txtFPS"
        Me.txtFPS.Text = Global.OHeadcamMapApp.My.MySettings.Default.ffmpegOutFps
        Me.ToolTip1.SetToolTip(Me.txtFPS, resources.GetString("txtFPS.ToolTip"))
        '
        'cmbOutputFormat
        '
        resources.ApplyResources(Me.cmbOutputFormat, "cmbOutputFormat")
        Me.cmbOutputFormat.FormattingEnabled = True
        Me.cmbOutputFormat.Items.AddRange(New Object() {resources.GetString("cmbOutputFormat.Items"), resources.GetString("cmbOutputFormat.Items1"), resources.GetString("cmbOutputFormat.Items2"), resources.GetString("cmbOutputFormat.Items3")})
        Me.cmbOutputFormat.Name = "cmbOutputFormat"
        Me.ToolTip1.SetToolTip(Me.cmbOutputFormat, resources.GetString("cmbOutputFormat.ToolTip"))
        '
        'frmSettings
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.cmbOutputFormat)
        Me.Controls.Add(Me.numSmooth)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.btnReset)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.btnSelectFolder)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtVideoWorkfolder)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.txtCRF)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtFPS)
        Me.Controls.Add(Me.Label12)
        Me.Name = "frmSettings"
        Me.ToolTip1.SetToolTip(Me, resources.GetString("$this.ToolTip"))
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.numSmooth, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtFPS As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents txtCRF As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents btnSave As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtSettingSbSCode As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label17 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents txtVideoWorkfolder As TextBox
    Friend WithEvents btnSelectFolder As Button
    Friend WithEvents Label7 As Label
    Friend WithEvents rbImgMiddle As RadioButton
    Friend WithEvents rbImgRight As RadioButton
    Friend WithEvents rbImgLeft As RadioButton
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents btnReset As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents cmbPreset As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents numSmooth As NumericUpDown
    Friend WithEvents cmbOutputFormat As ComboBox
    Friend WithEvents Label10 As Label
    Friend WithEvents txtVideoEncoderInfo As TextBox
End Class
