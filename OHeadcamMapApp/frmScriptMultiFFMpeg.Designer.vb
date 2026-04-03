<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmScriptMultiFFMpeg
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmScriptMultiFFMpeg))
        Me.dlgInputFolder = New System.Windows.Forms.FolderBrowserDialog()
        Me.dlgOutputFolder = New System.Windows.Forms.FolderBrowserDialog()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ColorDialog1 = New System.Windows.Forms.ColorDialog()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtFPS = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cmbPreset = New System.Windows.Forms.ComboBox()
        Me.btnSelecOutFolder = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtOutFolder = New System.Windows.Forms.TextBox()
        Me.numCRF = New System.Windows.Forms.NumericUpDown()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.btnSelectFolder = New System.Windows.Forms.Button()
        Me.txtVideoWorkfolder = New System.Windows.Forms.TextBox()
        Me.cmbExt = New System.Windows.Forms.ComboBox()
        Me.btnStart = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        CType(Me.numCRF, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(239, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Input filtype"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(239, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(27, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "FPS"
        '
        'txtFPS
        '
        Me.txtFPS.Location = New System.Drawing.Point(242, 39)
        Me.txtFPS.Name = "txtFPS"
        Me.txtFPS.Size = New System.Drawing.Size(100, 20)
        Me.txtFPS.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(359, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(156, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Kvalitet(fx 23=god, 30 dårligere)"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(6, 28)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(63, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Inputmappe"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 20)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(161, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Outputmappe (filnavne tilføjes _f)"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmbPreset)
        Me.GroupBox1.Controls.Add(Me.btnSelecOutFolder)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.txtOutFolder)
        Me.GroupBox1.Controls.Add(Me.numCRF)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.txtFPS)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 131)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(536, 120)
        Me.GroupBox1.TabIndex = 10
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Outputfiler(.mp4)"
        '
        'cmbPreset
        '
        Me.cmbPreset.AllowDrop = True
        Me.cmbPreset.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "ffmpegPreset", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.cmbPreset.FormattingEnabled = True
        Me.cmbPreset.Items.AddRange(New Object() {"ultrafast", "superfast", "veryfast", "faster", "fast", "medium", "slow", "slower", "veryslow"})
        Me.cmbPreset.Location = New System.Drawing.Point(242, 88)
        Me.cmbPreset.Name = "cmbPreset"
        Me.cmbPreset.Size = New System.Drawing.Size(121, 21)
        Me.cmbPreset.TabIndex = 18
        Me.cmbPreset.Text = Global.OHeadcamMapApp.My.MySettings.Default.ffmpegPreset
        '
        'btnSelecOutFolder
        '
        Me.btnSelecOutFolder.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnSelecOutFolder.Location = New System.Drawing.Point(115, 37)
        Me.btnSelecOutFolder.Name = "btnSelecOutFolder"
        Me.btnSelecOutFolder.Size = New System.Drawing.Size(103, 23)
        Me.btnSelecOutFolder.TabIndex = 40
        Me.btnSelecOutFolder.Text = "Vælg mappe..."
        Me.btnSelecOutFolder.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label6.Location = New System.Drawing.Point(239, 72)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(252, 13)
        Me.Label6.TabIndex = 17
        Me.Label6.Text = "Preset(videobehandlingstid - hurtigere giver større fil)"
        '
        'txtOutFolder
        '
        Me.txtOutFolder.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "VideoWorkFolder", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtOutFolder.Location = New System.Drawing.Point(9, 39)
        Me.txtOutFolder.Name = "txtOutFolder"
        Me.txtOutFolder.Size = New System.Drawing.Size(100, 20)
        Me.txtOutFolder.TabIndex = 39
        Me.txtOutFolder.Text = Global.OHeadcamMapApp.My.MySettings.Default.VideoWorkFolder
        '
        'numCRF
        '
        Me.numCRF.Location = New System.Drawing.Point(362, 39)
        Me.numCRF.Maximum = New Decimal(New Integer() {30, 0, 0, 0})
        Me.numCRF.Minimum = New Decimal(New Integer() {20, 0, 0, 0})
        Me.numCRF.Name = "numCRF"
        Me.numCRF.Size = New System.Drawing.Size(120, 20)
        Me.numCRF.TabIndex = 10
        Me.numCRF.Value = New Decimal(New Integer() {20, 0, 0, 0})
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnSelectFolder)
        Me.GroupBox2.Controls.Add(Me.txtVideoWorkfolder)
        Me.GroupBox2.Controls.Add(Me.cmbExt)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 25)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(536, 87)
        Me.GroupBox2.TabIndex = 11
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Inputfiler"
        '
        'btnSelectFolder
        '
        Me.btnSelectFolder.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnSelectFolder.Location = New System.Drawing.Point(112, 43)
        Me.btnSelectFolder.Name = "btnSelectFolder"
        Me.btnSelectFolder.Size = New System.Drawing.Size(103, 23)
        Me.btnSelectFolder.TabIndex = 38
        Me.btnSelectFolder.Text = "Vælg mappe..."
        Me.btnSelectFolder.UseVisualStyleBackColor = True
        '
        'txtVideoWorkfolder
        '
        Me.txtVideoWorkfolder.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "VideoWorkFolder", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtVideoWorkfolder.Location = New System.Drawing.Point(6, 45)
        Me.txtVideoWorkfolder.Name = "txtVideoWorkfolder"
        Me.txtVideoWorkfolder.Size = New System.Drawing.Size(100, 20)
        Me.txtVideoWorkfolder.TabIndex = 37
        Me.txtVideoWorkfolder.Text = Global.OHeadcamMapApp.My.MySettings.Default.VideoWorkFolder
        '
        'cmbExt
        '
        Me.cmbExt.FormattingEnabled = True
        Me.cmbExt.Items.AddRange(New Object() {".mp4", ".avi", ".mov", ".wmv"})
        Me.cmbExt.Location = New System.Drawing.Point(242, 44)
        Me.cmbExt.Name = "cmbExt"
        Me.cmbExt.Size = New System.Drawing.Size(121, 21)
        Me.cmbExt.TabIndex = 11
        '
        'btnStart
        '
        Me.btnStart.Location = New System.Drawing.Point(12, 257)
        Me.btnStart.Name = "btnStart"
        Me.btnStart.Size = New System.Drawing.Size(75, 23)
        Me.btnStart.TabIndex = 12
        Me.btnStart.Text = "Dan script"
        Me.btnStart.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(13, 287)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(147, 13)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "Script til Windows PowerShell"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(16, 308)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(442, 20)
        Me.TextBox1.TabIndex = 14
        '
        'frmScriptMultiFFMpeg
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(595, 340)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.btnStart)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmScriptMultiFFMpeg"
        Me.Text = "Juster FPS med script"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.numCRF, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dlgInputFolder As FolderBrowserDialog
    Friend WithEvents dlgOutputFolder As FolderBrowserDialog
    Friend WithEvents Label1 As Label
    Friend WithEvents ColorDialog1 As ColorDialog
    Friend WithEvents Label2 As Label
    Friend WithEvents txtFPS As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents cmbExt As ComboBox
    Friend WithEvents numCRF As NumericUpDown
    Friend WithEvents btnSelecOutFolder As Button
    Friend WithEvents txtOutFolder As TextBox
    Friend WithEvents btnSelectFolder As Button
    Friend WithEvents txtVideoWorkfolder As TextBox
    Friend WithEvents cmbPreset As ComboBox
    Friend WithEvents Label6 As Label
    Friend WithEvents btnStart As Button
    Friend WithEvents Label7 As Label
    Friend WithEvents TextBox1 As TextBox
End Class
