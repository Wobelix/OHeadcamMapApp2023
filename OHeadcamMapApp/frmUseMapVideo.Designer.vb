<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUseMapVideo
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
        Me.OpenFileDialogVideo = New System.Windows.Forms.OpenFileDialog()
        Me.btnSelectVideo = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.cbUseMapTracking = New System.Windows.Forms.CheckBox()
        Me.txtRealtimeFactor = New System.Windows.Forms.TextBox()
        Me.txtMapVideoIn = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'OpenFileDialogVideo
        '
        Me.OpenFileDialogVideo.Filter = "Video filer|*.mp4;*.avi;*.mov"
        '
        'btnSelectVideo
        '
        Me.btnSelectVideo.Location = New System.Drawing.Point(132, 28)
        Me.btnSelectVideo.Name = "btnSelectVideo"
        Me.btnSelectVideo.Size = New System.Drawing.Size(100, 23)
        Me.btnSelectVideo.TabIndex = 1
        Me.btnSelectVideo.Text = "Choose..."
        Me.btnSelectVideo.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(135, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Choose video with tracking"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(18, 67)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(138, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "No of times of normal speed"
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(18, 161)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 5
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'cbUseMapTracking
        '
        Me.cbUseMapTracking.AutoSize = True
        Me.cbUseMapTracking.Checked = Global.OHeadcamMapApp.My.MySettings.Default.bUseMapTrackingVideo
        Me.cbUseMapTracking.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.OHeadcamMapApp.My.MySettings.Default, "bUseMapTrackingVideo", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.cbUseMapTracking.Location = New System.Drawing.Point(18, 129)
        Me.cbUseMapTracking.Name = "cbUseMapTracking"
        Me.cbUseMapTracking.Size = New System.Drawing.Size(115, 17)
        Me.cbUseMapTracking.TabIndex = 6
        Me.cbUseMapTracking.Text = "Use tracking video"
        Me.cbUseMapTracking.UseVisualStyleBackColor = True
        '
        'txtRealtimeFactor
        '
        Me.txtRealtimeFactor.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "txtRealtimeFactor", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtRealtimeFactor.Location = New System.Drawing.Point(18, 83)
        Me.txtRealtimeFactor.Name = "txtRealtimeFactor"
        Me.txtRealtimeFactor.Size = New System.Drawing.Size(64, 20)
        Me.txtRealtimeFactor.TabIndex = 2
        Me.txtRealtimeFactor.Text = Global.OHeadcamMapApp.My.MySettings.Default.txtRealtimeFactor
        '
        'txtMapVideoIn
        '
        Me.txtMapVideoIn.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "txtMapVideoFilename", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtMapVideoIn.Location = New System.Drawing.Point(18, 28)
        Me.txtMapVideoIn.Name = "txtMapVideoIn"
        Me.txtMapVideoIn.Size = New System.Drawing.Size(100, 20)
        Me.txtMapVideoIn.TabIndex = 0
        Me.txtMapVideoIn.Text = Global.OHeadcamMapApp.My.MySettings.Default.txtMapVideoFilename
        '
        'frmUseMapVideo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(316, 196)
        Me.Controls.Add(Me.cbUseMapTracking)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtRealtimeFactor)
        Me.Controls.Add(Me.btnSelectVideo)
        Me.Controls.Add(Me.txtMapVideoIn)
        Me.Name = "frmUseMapVideo"
        Me.Text = "Use tracking video"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents OpenFileDialogVideo As OpenFileDialog
    Friend WithEvents txtMapVideoIn As TextBox
    Friend WithEvents btnSelectVideo As Button
    Friend WithEvents txtRealtimeFactor As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents btnSave As Button
    Friend WithEvents cbUseMapTracking As CheckBox
End Class
