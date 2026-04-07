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
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
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
        Me.chkHDformat = New System.Windows.Forms.CheckBox()
        Me.txtVideoWorkfolder = New System.Windows.Forms.TextBox()
        Me.cmbPreset = New System.Windows.Forms.ComboBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.txtSettingSbSCode = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.txtCRF = New System.Windows.Forms.TextBox()
        Me.txtFPS = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.numSmooth, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label12
        '
        resources.ApplyResources(Me.Label12, "Label12")
        Me.Label12.Name = "Label12"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'btnSave
        '
        resources.ApplyResources(Me.btnSave, "btnSave")
        Me.btnSave.Name = "btnSave"
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
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'GroupBox1
        '
        Me.GroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.None
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
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
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
        'TextBox4
        '
        Me.TextBox4.BackColor = System.Drawing.SystemColors.Info
        resources.ApplyResources(Me.TextBox4, "TextBox4")
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.ReadOnly = True
        '
        'btnSelectFolder
        '
        resources.ApplyResources(Me.btnSelectFolder, "btnSelectFolder")
        Me.btnSelectFolder.Name = "btnSelectFolder"
        Me.btnSelectFolder.UseVisualStyleBackColor = True
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.rbImgMiddle)
        Me.GroupBox2.Controls.Add(Me.rbImgRight)
        Me.GroupBox2.Controls.Add(Me.rbImgLeft)
        resources.ApplyResources(Me.GroupBox2, "GroupBox2")
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.TabStop = False
        '
        'rbImgMiddle
        '
        resources.ApplyResources(Me.rbImgMiddle, "rbImgMiddle")
        Me.rbImgMiddle.Name = "rbImgMiddle"
        Me.rbImgMiddle.UseVisualStyleBackColor = True
        '
        'rbImgRight
        '
        resources.ApplyResources(Me.rbImgRight, "rbImgRight")
        Me.rbImgRight.Checked = True
        Me.rbImgRight.Name = "rbImgRight"
        Me.rbImgRight.TabStop = True
        Me.rbImgRight.UseVisualStyleBackColor = True
        '
        'rbImgLeft
        '
        resources.ApplyResources(Me.rbImgLeft, "rbImgLeft")
        Me.rbImgLeft.Name = "rbImgLeft"
        Me.rbImgLeft.UseVisualStyleBackColor = True
        '
        'btnReset
        '
        resources.ApplyResources(Me.btnReset, "btnReset")
        Me.btnReset.Name = "btnReset"
        Me.btnReset.UseVisualStyleBackColor = True
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Name = "Label8"
        '
        'Label9
        '
        resources.ApplyResources(Me.Label9, "Label9")
        Me.Label9.Name = "Label9"
        '
        'numSmooth
        '
        Me.numSmooth.DecimalPlaces = 2
        Me.numSmooth.Increment = New Decimal(New Integer() {5, 0, 0, 131072})
        resources.ApplyResources(Me.numSmooth, "numSmooth")
        Me.numSmooth.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numSmooth.Minimum = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.numSmooth.Name = "numSmooth"
        Me.numSmooth.Value = New Decimal(New Integer() {25, 0, 0, 131072})
        '
        'chkHDformat
        '
        resources.ApplyResources(Me.chkHDformat, "chkHDformat")
        Me.chkHDformat.Checked = Global.OHeadcamMapApp.My.MySettings.Default.chkHDFormat
        Me.chkHDformat.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.OHeadcamMapApp.My.MySettings.Default, "chkHDFormat", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.chkHDformat.Name = "chkHDformat"
        Me.chkHDformat.UseVisualStyleBackColor = True
        '
        'txtVideoWorkfolder
        '
        Me.txtVideoWorkfolder.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "VideoWorkFolder", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.txtVideoWorkfolder, "txtVideoWorkfolder")
        Me.txtVideoWorkfolder.Name = "txtVideoWorkfolder"
        Me.txtVideoWorkfolder.Text = Global.OHeadcamMapApp.My.MySettings.Default.VideoWorkFolder
        '
        'cmbPreset
        '
        Me.cmbPreset.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "ffmpegPreset", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.cmbPreset.FormattingEnabled = True
        Me.cmbPreset.Items.AddRange(New Object() {resources.GetString("cmbPreset.Items"), resources.GetString("cmbPreset.Items1"), resources.GetString("cmbPreset.Items2"), resources.GetString("cmbPreset.Items3"), resources.GetString("cmbPreset.Items4"), resources.GetString("cmbPreset.Items5"), resources.GetString("cmbPreset.Items6"), resources.GetString("cmbPreset.Items7"), resources.GetString("cmbPreset.Items8"), resources.GetString("cmbPreset.Items9")})
        resources.ApplyResources(Me.cmbPreset, "cmbPreset")
        Me.cmbPreset.Name = "cmbPreset"
        Me.cmbPreset.Text = Global.OHeadcamMapApp.My.MySettings.Default.ffmpegPreset
        '
        'TextBox2
        '
        Me.TextBox2.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "ffmpegVidstabDetect", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.TextBox2, "TextBox2")
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Text = Global.OHeadcamMapApp.My.MySettings.Default.ffmpegVidstabDetect
        '
        'TextBox1
        '
        Me.TextBox1.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "No_deshake_filter", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.TextBox1, "TextBox1")
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Text = Global.OHeadcamMapApp.My.MySettings.Default.No_deshake_filter
        '
        'txtSettingSbSCode
        '
        Me.txtSettingSbSCode.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "SettingSbSCode", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.txtSettingSbSCode, "txtSettingSbSCode")
        Me.txtSettingSbSCode.Name = "txtSettingSbSCode"
        Me.txtSettingSbSCode.Text = Global.OHeadcamMapApp.My.MySettings.Default.SettingSbSCode
        '
        'TextBox3
        '
        Me.TextBox3.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "ffmpegVidstabTransform", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.TextBox3, "TextBox3")
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Text = Global.OHeadcamMapApp.My.MySettings.Default.ffmpegVidstabTransform
        '
        'txtCRF
        '
        Me.txtCRF.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "ffmpegCRF", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.txtCRF, "txtCRF")
        Me.txtCRF.Name = "txtCRF"
        Me.txtCRF.Text = Global.OHeadcamMapApp.My.MySettings.Default.ffmpegCRF
        '
        'txtFPS
        '
        Me.txtFPS.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "ffmpegOutFps", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.txtFPS, "txtFPS")
        Me.txtFPS.Name = "txtFPS"
        Me.txtFPS.Text = Global.OHeadcamMapApp.My.MySettings.Default.ffmpegOutFps
        '
        'frmSettings
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.numSmooth)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.chkHDformat)
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
    Friend WithEvents chkHDformat As CheckBox
    Friend WithEvents cmbPreset As ComboBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents numSmooth As NumericUpDown
End Class
