<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPostProcess
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPostProcess))
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.btnClearVideolist = New System.Windows.Forms.Button()
        Me.AddVideofilebef = New System.Windows.Forms.Button()
        Me.AddVideofileaft = New System.Windows.Forms.Button()
        Me.OpenFileDialogVideo = New System.Windows.Forms.OpenFileDialog()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.PlayMedia = New System.Windows.Forms.Button()
        Me.btnDeleteVideoFile = New System.Windows.Forms.Button()
        Me.LVMediaList = New System.Windows.Forms.ListView()
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.txtFFArg = New System.Windows.Forms.TextBox()
        Me.btnView = New System.Windows.Forms.Button()
        Me.RemainingTimer = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.btnOverlayPlay = New System.Windows.Forms.Button()
        Me.btnOverlayDelete = New System.Windows.Forms.Button()
        Me.LV_Overlaylist = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.btnAddOverlayaft = New System.Windows.Forms.Button()
        Me.btnAddOverlaybef = New System.Windows.Forms.Button()
        Me.btnOverlaysClear = New System.Windows.Forms.Button()
        Me.PBTimer = New System.Windows.Forms.Timer(Me.components)
        Me.Timer3 = New System.Windows.Forms.Timer(Me.components)
        Me.StatusTxt = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.lbStatusProgress = New System.Windows.Forms.ToolStripStatusLabel()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.bB1 = New System.Windows.Forms.Button()
        Me.bF1 = New System.Windows.Forms.Button()
        Me.lblVideoPos = New System.Windows.Forms.Label()
        Me.TrackBar1 = New System.Windows.Forms.TrackBar()
        Me.VideoView1 = New LibVLCSharp.WinForms.VideoView()
        Me.bSlow = New System.Windows.Forms.Button()
        Me.bFForward = New System.Windows.Forms.Button()
        Me.bPlay = New System.Windows.Forms.Button()
        Me.btnOutputFile = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtOutFilename = New System.Windows.Forms.TextBox()
        Me.btnRunMakeVideo = New System.Windows.Forms.Button()
        Me.SaveFileDialogOutput = New System.Windows.Forms.SaveFileDialog()
        Me.btnAddMusic = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VideoView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Timer2
        '
        Me.Timer2.Enabled = True
        Me.Timer2.Interval = 200
        '
        'Timer1
        '
        '
        'btnClearVideolist
        '
        resources.ApplyResources(Me.btnClearVideolist, "btnClearVideolist")
        Me.btnClearVideolist.Name = "btnClearVideolist"
        Me.btnClearVideolist.UseVisualStyleBackColor = True
        '
        'AddVideofilebef
        '
        resources.ApplyResources(Me.AddVideofilebef, "AddVideofilebef")
        Me.AddVideofilebef.Name = "AddVideofilebef"
        Me.AddVideofilebef.UseVisualStyleBackColor = True
        '
        'AddVideofileaft
        '
        resources.ApplyResources(Me.AddVideofileaft, "AddVideofileaft")
        Me.AddVideofileaft.Name = "AddVideofileaft"
        Me.AddVideofileaft.UseVisualStyleBackColor = True
        '
        'OpenFileDialogVideo
        '
        resources.ApplyResources(Me.OpenFileDialogVideo, "OpenFileDialogVideo")
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.PlayMedia)
        Me.GroupBox1.Controls.Add(Me.btnDeleteVideoFile)
        Me.GroupBox1.Controls.Add(Me.LVMediaList)
        Me.GroupBox1.Controls.Add(Me.AddVideofileaft)
        Me.GroupBox1.Controls.Add(Me.AddVideofilebef)
        Me.GroupBox1.Controls.Add(Me.btnClearVideolist)
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'PlayMedia
        '
        resources.ApplyResources(Me.PlayMedia, "PlayMedia")
        Me.PlayMedia.Name = "PlayMedia"
        Me.PlayMedia.UseVisualStyleBackColor = True
        '
        'btnDeleteVideoFile
        '
        resources.ApplyResources(Me.btnDeleteVideoFile, "btnDeleteVideoFile")
        Me.btnDeleteVideoFile.Name = "btnDeleteVideoFile"
        Me.btnDeleteVideoFile.UseVisualStyleBackColor = True
        '
        'LVMediaList
        '
        Me.LVMediaList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader3, Me.ColumnHeader4})
        Me.LVMediaList.FullRowSelect = True
        Me.LVMediaList.GridLines = True
        Me.LVMediaList.HideSelection = False
        resources.ApplyResources(Me.LVMediaList, "LVMediaList")
        Me.LVMediaList.Name = "LVMediaList"
        Me.LVMediaList.UseCompatibleStateImageBehavior = False
        Me.LVMediaList.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader3
        '
        resources.ApplyResources(Me.ColumnHeader3, "ColumnHeader3")
        '
        'ColumnHeader4
        '
        resources.ApplyResources(Me.ColumnHeader4, "ColumnHeader4")
        '
        'txtFFArg
        '
        resources.ApplyResources(Me.txtFFArg, "txtFFArg")
        Me.txtFFArg.Name = "txtFFArg"
        '
        'btnView
        '
        resources.ApplyResources(Me.btnView, "btnView")
        Me.btnView.Name = "btnView"
        Me.btnView.UseVisualStyleBackColor = True
        '
        'RemainingTimer
        '
        Me.RemainingTimer.Interval = 1000
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnOverlayPlay)
        Me.GroupBox2.Controls.Add(Me.btnOverlayDelete)
        Me.GroupBox2.Controls.Add(Me.LV_Overlaylist)
        Me.GroupBox2.Controls.Add(Me.btnAddOverlayaft)
        Me.GroupBox2.Controls.Add(Me.btnAddOverlaybef)
        Me.GroupBox2.Controls.Add(Me.btnOverlaysClear)
        resources.ApplyResources(Me.GroupBox2, "GroupBox2")
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.TabStop = False
        '
        'btnOverlayPlay
        '
        resources.ApplyResources(Me.btnOverlayPlay, "btnOverlayPlay")
        Me.btnOverlayPlay.Name = "btnOverlayPlay"
        Me.btnOverlayPlay.UseVisualStyleBackColor = True
        '
        'btnOverlayDelete
        '
        resources.ApplyResources(Me.btnOverlayDelete, "btnOverlayDelete")
        Me.btnOverlayDelete.Name = "btnOverlayDelete"
        Me.btnOverlayDelete.UseVisualStyleBackColor = True
        '
        'LV_Overlaylist
        '
        Me.LV_Overlaylist.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.LV_Overlaylist.FullRowSelect = True
        Me.LV_Overlaylist.GridLines = True
        Me.LV_Overlaylist.HideSelection = False
        resources.ApplyResources(Me.LV_Overlaylist, "LV_Overlaylist")
        Me.LV_Overlaylist.Name = "LV_Overlaylist"
        Me.LV_Overlaylist.UseCompatibleStateImageBehavior = False
        Me.LV_Overlaylist.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        resources.ApplyResources(Me.ColumnHeader1, "ColumnHeader1")
        '
        'ColumnHeader2
        '
        resources.ApplyResources(Me.ColumnHeader2, "ColumnHeader2")
        '
        'btnAddOverlayaft
        '
        resources.ApplyResources(Me.btnAddOverlayaft, "btnAddOverlayaft")
        Me.btnAddOverlayaft.Name = "btnAddOverlayaft"
        Me.btnAddOverlayaft.UseVisualStyleBackColor = True
        '
        'btnAddOverlaybef
        '
        resources.ApplyResources(Me.btnAddOverlaybef, "btnAddOverlaybef")
        Me.btnAddOverlaybef.Name = "btnAddOverlaybef"
        Me.btnAddOverlaybef.UseVisualStyleBackColor = True
        '
        'btnOverlaysClear
        '
        resources.ApplyResources(Me.btnOverlaysClear, "btnOverlaysClear")
        Me.btnOverlaysClear.Name = "btnOverlaysClear"
        Me.btnOverlaysClear.UseVisualStyleBackColor = True
        '
        'PBTimer
        '
        Me.PBTimer.Interval = 500
        '
        'StatusTxt
        '
        Me.StatusTxt.Name = "StatusTxt"
        resources.ApplyResources(Me.StatusTxt, "StatusTxt")
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Name = "ProgressBar1"
        resources.ApplyResources(Me.ProgressBar1, "ProgressBar1")
        '
        'lbStatusProgress
        '
        Me.lbStatusProgress.Name = "lbStatusProgress"
        resources.ApplyResources(Me.lbStatusProgress, "lbStatusProgress")
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusTxt, Me.ProgressBar1, Me.lbStatusProgress})
        resources.ApplyResources(Me.StatusStrip1, "StatusStrip1")
        Me.StatusStrip1.Name = "StatusStrip1"
        '
        'bB1
        '
        resources.ApplyResources(Me.bB1, "bB1")
        Me.bB1.Name = "bB1"
        Me.bB1.UseVisualStyleBackColor = True
        '
        'bF1
        '
        resources.ApplyResources(Me.bF1, "bF1")
        Me.bF1.Name = "bF1"
        Me.bF1.UseVisualStyleBackColor = True
        '
        'lblVideoPos
        '
        resources.ApplyResources(Me.lblVideoPos, "lblVideoPos")
        Me.lblVideoPos.Name = "lblVideoPos"
        '
        'TrackBar1
        '
        resources.ApplyResources(Me.TrackBar1, "TrackBar1")
        Me.TrackBar1.LargeChange = 1
        Me.TrackBar1.Name = "TrackBar1"
        '
        'VideoView1
        '
        resources.ApplyResources(Me.VideoView1, "VideoView1")
        Me.VideoView1.BackColor = System.Drawing.Color.Black
        Me.VideoView1.MediaPlayer = Nothing
        Me.VideoView1.Name = "VideoView1"
        '
        'bSlow
        '
        resources.ApplyResources(Me.bSlow, "bSlow")
        Me.bSlow.Name = "bSlow"
        Me.bSlow.UseVisualStyleBackColor = True
        '
        'bFForward
        '
        resources.ApplyResources(Me.bFForward, "bFForward")
        Me.bFForward.Name = "bFForward"
        Me.bFForward.UseVisualStyleBackColor = True
        '
        'bPlay
        '
        resources.ApplyResources(Me.bPlay, "bPlay")
        Me.bPlay.Name = "bPlay"
        Me.bPlay.UseVisualStyleBackColor = True
        '
        'btnOutputFile
        '
        resources.ApplyResources(Me.btnOutputFile, "btnOutputFile")
        Me.btnOutputFile.Name = "btnOutputFile"
        Me.btnOutputFile.UseVisualStyleBackColor = True
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        '
        'txtOutFilename
        '
        resources.ApplyResources(Me.txtOutFilename, "txtOutFilename")
        Me.txtOutFilename.Name = "txtOutFilename"
        '
        'btnRunMakeVideo
        '
        resources.ApplyResources(Me.btnRunMakeVideo, "btnRunMakeVideo")
        Me.btnRunMakeVideo.Name = "btnRunMakeVideo"
        Me.btnRunMakeVideo.UseVisualStyleBackColor = True
        '
        'SaveFileDialogOutput
        '
        Me.SaveFileDialogOutput.FileName = "PostFinal.mp4"
        resources.ApplyResources(Me.SaveFileDialogOutput, "SaveFileDialogOutput")
        '
        'btnAddMusic
        '
        resources.ApplyResources(Me.btnAddMusic, "btnAddMusic")
        Me.btnAddMusic.Name = "btnAddMusic"
        Me.btnAddMusic.UseVisualStyleBackColor = True
        '
        'frmPostProcess
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnAddMusic)
        Me.Controls.Add(Me.btnRunMakeVideo)
        Me.Controls.Add(Me.btnOutputFile)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtOutFilename)
        Me.Controls.Add(Me.bSlow)
        Me.Controls.Add(Me.bFForward)
        Me.Controls.Add(Me.bPlay)
        Me.Controls.Add(Me.VideoView1)
        Me.Controls.Add(Me.bB1)
        Me.Controls.Add(Me.bF1)
        Me.Controls.Add(Me.lblVideoPos)
        Me.Controls.Add(Me.TrackBar1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.btnView)
        Me.Controls.Add(Me.txtFFArg)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "frmPostProcess"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VideoView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents Timer2 As Timer
    Friend WithEvents Timer1 As Timer
    Friend WithEvents btnClearVideolist As Button
    Friend WithEvents AddVideofilebef As Button
    Friend WithEvents AddVideofileaft As Button
    Friend WithEvents OpenFileDialogVideo As OpenFileDialog
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents txtFFArg As TextBox
    Friend WithEvents btnView As Button
    Friend WithEvents RemainingTimer As Timer
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents btnAddOverlayaft As Button
    Friend WithEvents btnAddOverlaybef As Button
    Friend WithEvents btnOverlaysClear As Button
    Friend WithEvents LV_Overlaylist As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents btnOverlayDelete As Button
    Friend WithEvents LVMediaList As ListView
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents ColumnHeader4 As ColumnHeader
    Friend WithEvents btnDeleteVideoFile As Button
    Friend WithEvents PBTimer As Timer
    Friend WithEvents Timer3 As Timer
    Friend WithEvents StatusTxt As ToolStripStatusLabel
    Friend WithEvents ProgressBar1 As ToolStripProgressBar
    Friend WithEvents lbStatusProgress As ToolStripStatusLabel
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents bB1 As Button
    Friend WithEvents bF1 As Button
    Friend WithEvents lblVideoPos As Label
    Friend WithEvents TrackBar1 As TrackBar
    Friend WithEvents VideoView1 As LibVLCSharp.WinForms.VideoView
    Friend WithEvents bSlow As Button
    Friend WithEvents bFForward As Button
    Friend WithEvents bPlay As Button
    Friend WithEvents PlayMedia As Button
    Friend WithEvents btnOverlayPlay As Button
    Friend WithEvents btnOutputFile As Button
    Friend WithEvents Label7 As Label
    Friend WithEvents txtOutFilename As TextBox
    Friend WithEvents btnRunMakeVideo As Button
    Friend WithEvents SaveFileDialogOutput As SaveFileDialog
    Friend WithEvents btnAddMusic As Button
End Class
