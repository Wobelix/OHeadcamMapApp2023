<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAdjustmentPlayer
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAdjustmentPlayer))
        Me.btnMakeAdjVideo = New System.Windows.Forms.Button()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.btnSetPosGPS = New System.Windows.Forms.Button()
        Me.btnSetVideoPos = New System.Windows.Forms.Button()
        Me.bPlay = New System.Windows.Forms.Button()
        Me.bFForward = New System.Windows.Forms.Button()
        Me.bSlow = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Label20 = New System.Windows.Forms.Label()
        Me.txtPlayerfile = New System.Windows.Forms.TextBox()
        Me.TrackBar1 = New System.Windows.Forms.TrackBar()
        Me.lblVideoPos = New System.Windows.Forms.Label()
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.Label1 = New System.Windows.Forms.Label()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LabelIns1 = New System.Windows.Forms.Label()
        Me.LabelIns2 = New System.Windows.Forms.Label()
        Me.LabelIns3 = New System.Windows.Forms.Label()
        Me.LabelIns4 = New System.Windows.Forms.Label()
        Me.btnTransfer = New System.Windows.Forms.Button()
        Me.btnMakeTestVideo = New System.Windows.Forms.Button()
        Me.txtLength = New System.Windows.Forms.TextBox()
        Me.txtGPSDiff = New System.Windows.Forms.TextBox()
        Me.txtGPSPos = New System.Windows.Forms.TextBox()
        Me.txtVideoPos = New System.Windows.Forms.TextBox()
        Me.btnReAdjust = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.AxWindowsMediaPlayer1 = New AxWMPLib.AxWindowsMediaPlayer()
        Me.pbIconWork = New System.Windows.Forms.PictureBox()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.AxWindowsMediaPlayer1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbIconWork, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnMakeAdjVideo
        '
        resources.ApplyResources(Me.btnMakeAdjVideo, "btnMakeAdjVideo")
        Me.btnMakeAdjVideo.Name = "btnMakeAdjVideo"
        Me.btnMakeAdjVideo.UseVisualStyleBackColor = True
        '
        'Label26
        '
        resources.ApplyResources(Me.Label26, "Label26")
        Me.Label26.Name = "Label26"
        '
        'btnSetPosGPS
        '
        resources.ApplyResources(Me.btnSetPosGPS, "btnSetPosGPS")
        Me.btnSetPosGPS.Name = "btnSetPosGPS"
        Me.btnSetPosGPS.UseVisualStyleBackColor = True
        '
        'btnSetVideoPos
        '
        resources.ApplyResources(Me.btnSetVideoPos, "btnSetVideoPos")
        Me.btnSetVideoPos.Name = "btnSetVideoPos"
        Me.btnSetVideoPos.UseVisualStyleBackColor = True
        '
        'bPlay
        '
        resources.ApplyResources(Me.bPlay, "bPlay")
        Me.bPlay.Name = "bPlay"
        Me.bPlay.UseVisualStyleBackColor = True
        '
        'bFForward
        '
        resources.ApplyResources(Me.bFForward, "bFForward")
        Me.bFForward.Name = "bFForward"
        Me.bFForward.UseVisualStyleBackColor = True
        '
        'bSlow
        '
        resources.ApplyResources(Me.bSlow, "bSlow")
        Me.bSlow.Name = "bSlow"
        Me.bSlow.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        '
        'Label20
        '
        resources.ApplyResources(Me.Label20, "Label20")
        Me.Label20.Name = "Label20"
        '
        'txtPlayerfile
        '
        resources.ApplyResources(Me.txtPlayerfile, "txtPlayerfile")
        Me.txtPlayerfile.Name = "txtPlayerfile"
        Me.txtPlayerfile.ReadOnly = True
        '
        'TrackBar1
        '
        resources.ApplyResources(Me.TrackBar1, "TrackBar1")
        Me.TrackBar1.LargeChange = 1
        Me.TrackBar1.Name = "TrackBar1"
        '
        'lblVideoPos
        '
        resources.ApplyResources(Me.lblVideoPos, "lblVideoPos")
        Me.lblVideoPos.Name = "lblVideoPos"
        '
        'Timer2
        '
        Me.Timer2.Enabled = True
        Me.Timer2.Interval = 500
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel})
        resources.ApplyResources(Me.StatusStrip1, "StatusStrip1")
        Me.StatusStrip1.Name = "StatusStrip1"
        '
        'StatusLabel
        '
        Me.StatusLabel.Name = "StatusLabel"
        resources.ApplyResources(Me.StatusLabel, "StatusLabel")
        '
        'LabelIns1
        '
        resources.ApplyResources(Me.LabelIns1, "LabelIns1")
        Me.LabelIns1.BackColor = System.Drawing.Color.Black
        Me.LabelIns1.ForeColor = System.Drawing.Color.White
        Me.LabelIns1.Name = "LabelIns1"
        '
        'LabelIns2
        '
        resources.ApplyResources(Me.LabelIns2, "LabelIns2")
        Me.LabelIns2.BackColor = System.Drawing.Color.Black
        Me.LabelIns2.ForeColor = System.Drawing.Color.White
        Me.LabelIns2.Name = "LabelIns2"
        '
        'LabelIns3
        '
        resources.ApplyResources(Me.LabelIns3, "LabelIns3")
        Me.LabelIns3.BackColor = System.Drawing.Color.Black
        Me.LabelIns3.ForeColor = System.Drawing.Color.White
        Me.LabelIns3.Name = "LabelIns3"
        '
        'LabelIns4
        '
        resources.ApplyResources(Me.LabelIns4, "LabelIns4")
        Me.LabelIns4.BackColor = System.Drawing.Color.Black
        Me.LabelIns4.ForeColor = System.Drawing.Color.White
        Me.LabelIns4.Name = "LabelIns4"
        '
        'btnTransfer
        '
        resources.ApplyResources(Me.btnTransfer, "btnTransfer")
        Me.btnTransfer.Name = "btnTransfer"
        Me.btnTransfer.UseVisualStyleBackColor = True
        '
        'btnMakeTestVideo
        '
        resources.ApplyResources(Me.btnMakeTestVideo, "btnMakeTestVideo")
        Me.btnMakeTestVideo.Name = "btnMakeTestVideo"
        Me.btnMakeTestVideo.UseVisualStyleBackColor = True
        '
        'txtLength
        '
        Me.txtLength.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "AdjVideoLength", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.txtLength, "txtLength")
        Me.txtLength.Name = "txtLength"
        Me.txtLength.Text = Global.OHeadcamMapApp.My.MySettings.Default.AdjVideoLength
        '
        'txtGPSDiff
        '
        Me.txtGPSDiff.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "GPXDiff", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.txtGPSDiff, "txtGPSDiff")
        Me.txtGPSDiff.Name = "txtGPSDiff"
        Me.txtGPSDiff.Text = Global.OHeadcamMapApp.My.MySettings.Default.GPXDiff
        '
        'txtGPSPos
        '
        Me.txtGPSPos.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "txtGPSPos", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.txtGPSPos, "txtGPSPos")
        Me.txtGPSPos.Name = "txtGPSPos"
        Me.txtGPSPos.Text = Global.OHeadcamMapApp.My.MySettings.Default.txtGPSPos
        '
        'txtVideoPos
        '
        Me.txtVideoPos.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "txtVideoPos", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.txtVideoPos, "txtVideoPos")
        Me.txtVideoPos.Name = "txtVideoPos"
        Me.txtVideoPos.Text = Global.OHeadcamMapApp.My.MySettings.Default.txtVideoPos
        '
        'btnReAdjust
        '
        resources.ApplyResources(Me.btnReAdjust, "btnReAdjust")
        Me.btnReAdjust.Name = "btnReAdjust"
        Me.btnReAdjust.UseVisualStyleBackColor = True
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.BackColor = System.Drawing.Color.Black
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Name = "Label2"
        '
        'AxWindowsMediaPlayer1
        '
        resources.ApplyResources(Me.AxWindowsMediaPlayer1, "AxWindowsMediaPlayer1")
        Me.AxWindowsMediaPlayer1.Name = "AxWindowsMediaPlayer1"
        Me.AxWindowsMediaPlayer1.OcxState = CType(resources.GetObject("AxWindowsMediaPlayer1.OcxState"), System.Windows.Forms.AxHost.State)
        '
        'pbIconWork
        '
        Me.pbIconWork.BackColor = System.Drawing.SystemColors.ControlDark
        resources.ApplyResources(Me.pbIconWork, "pbIconWork")
        Me.pbIconWork.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbIconWork.Image = Global.OHeadcamMapApp.My.Resources.Resources.Workimg
        Me.pbIconWork.Name = "pbIconWork"
        Me.pbIconWork.TabStop = False
        Me.pbIconWork.UseWaitCursor = True
        '
        'frmAdjustmentPlayer
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pbIconWork)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.btnReAdjust)
        Me.Controls.Add(Me.btnMakeTestVideo)
        Me.Controls.Add(Me.btnTransfer)
        Me.Controls.Add(Me.LabelIns4)
        Me.Controls.Add(Me.LabelIns3)
        Me.Controls.Add(Me.LabelIns2)
        Me.Controls.Add(Me.LabelIns1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.txtLength)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblVideoPos)
        Me.Controls.Add(Me.TrackBar1)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.txtPlayerfile)
        Me.Controls.Add(Me.bSlow)
        Me.Controls.Add(Me.bFForward)
        Me.Controls.Add(Me.bPlay)
        Me.Controls.Add(Me.btnMakeAdjVideo)
        Me.Controls.Add(Me.txtGPSDiff)
        Me.Controls.Add(Me.Label26)
        Me.Controls.Add(Me.txtGPSPos)
        Me.Controls.Add(Me.txtVideoPos)
        Me.Controls.Add(Me.btnSetPosGPS)
        Me.Controls.Add(Me.btnSetVideoPos)
        Me.Controls.Add(Me.AxWindowsMediaPlayer1)
        Me.Name = "frmAdjustmentPlayer"
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.AxWindowsMediaPlayer1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbIconWork, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents AxWindowsMediaPlayer1 As AxWMPLib.AxWindowsMediaPlayer
    Friend WithEvents btnMakeAdjVideo As Button
    Friend WithEvents txtGPSDiff As TextBox
    Friend WithEvents Label26 As Label
    Friend WithEvents txtGPSPos As TextBox
    Friend WithEvents txtVideoPos As TextBox
    Friend WithEvents btnSetPosGPS As Button
    Friend WithEvents btnSetVideoPos As Button
    Friend WithEvents bPlay As Button
    Friend WithEvents bFForward As Button
    Friend WithEvents bSlow As Button
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Label20 As Label
    Friend WithEvents txtPlayerfile As TextBox
    Friend WithEvents TrackBar1 As TrackBar
    Friend WithEvents lblVideoPos As Label
    Friend WithEvents Timer2 As Timer
    Friend WithEvents Label1 As Label
    Friend WithEvents txtLength As TextBox
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents StatusLabel As ToolStripStatusLabel
    Friend WithEvents LabelIns1 As Label
    Friend WithEvents LabelIns2 As Label
    Friend WithEvents LabelIns3 As Label
    Friend WithEvents LabelIns4 As Label
    Friend WithEvents btnTransfer As Button
    Friend WithEvents btnMakeTestVideo As Button
    Friend WithEvents btnReAdjust As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents pbIconWork As PictureBox
End Class
