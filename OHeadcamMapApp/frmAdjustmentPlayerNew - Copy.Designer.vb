<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmAdjustmentPlayer_new2
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAdjustmentPlayer_new))
        Me.Label26 = New System.Windows.Forms.Label()
        Me.bPlay = New System.Windows.Forms.Button()
        Me.bFForward = New System.Windows.Forms.Button()
        Me.bSlow = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.TrackBar1 = New System.Windows.Forms.TrackBar()
        Me.lblVideoPos = New System.Windows.Forms.Label()
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.btnTransfer = New System.Windows.Forms.Button()
        Me.txtGPSDiff = New System.Windows.Forms.TextBox()
        Me.PB_Zoom = New System.Windows.Forms.PictureBox()
        Me.PBLeg = New System.Windows.Forms.PictureBox()
        Me.MapImageTimer = New System.Windows.Forms.Timer(Me.components)
        Me.numGPSDelta = New System.Windows.Forms.NumericUpDown()
        Me.AxWindowsMediaPlayer1 = New AxWMPLib.AxWindowsMediaPlayer()
        Me.VideoPanel = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.bF1 = New System.Windows.Forms.Button()
        Me.bB1 = New System.Windows.Forms.Button()
        Me.ResizeUpdate = New System.Windows.Forms.Timer(Me.components)
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.PB_Zoom, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PBLeg, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numGPSDelta, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AxWindowsMediaPlayer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.VideoPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label26
        '
        resources.ApplyResources(Me.Label26, "Label26")
        Me.Label26.Name = "Label26"
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
        Me.Timer2.Interval = 200
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
        'btnTransfer
        '
        resources.ApplyResources(Me.btnTransfer, "btnTransfer")
        Me.btnTransfer.Name = "btnTransfer"
        Me.btnTransfer.UseVisualStyleBackColor = True
        '
        'txtGPSDiff
        '
        Me.txtGPSDiff.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "GPXDiff", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.txtGPSDiff, "txtGPSDiff")
        Me.txtGPSDiff.Name = "txtGPSDiff"
        Me.txtGPSDiff.Text = Global.OHeadcamMapApp.My.MySettings.Default.GPXDiff
        '
        'PB_Zoom
        '
        Me.PB_Zoom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.PB_Zoom, "PB_Zoom")
        Me.PB_Zoom.Name = "PB_Zoom"
        Me.PB_Zoom.TabStop = False
        '
        'PBLeg
        '
        Me.PBLeg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.PBLeg, "PBLeg")
        Me.PBLeg.Name = "PBLeg"
        Me.PBLeg.TabStop = False
        '
        'MapImageTimer
        '
        '
        'numGPSDelta
        '
        resources.ApplyResources(Me.numGPSDelta, "numGPSDelta")
        Me.numGPSDelta.Maximum = New Decimal(New Integer() {2000, 0, 0, 0})
        Me.numGPSDelta.Minimum = New Decimal(New Integer() {100, 0, 0, -2147483648})
        Me.numGPSDelta.Name = "numGPSDelta"
        '
        'AxWindowsMediaPlayer1
        '
        resources.ApplyResources(Me.AxWindowsMediaPlayer1, "AxWindowsMediaPlayer1")
        Me.AxWindowsMediaPlayer1.Name = "AxWindowsMediaPlayer1"
        Me.AxWindowsMediaPlayer1.OcxState = CType(resources.GetObject("AxWindowsMediaPlayer1.OcxState"), System.Windows.Forms.AxHost.State)
        '
        'VideoPanel
        '
        Me.VideoPanel.BackColor = System.Drawing.SystemColors.Desktop
        Me.VideoPanel.Controls.Add(Me.PB_Zoom)
        Me.VideoPanel.Controls.Add(Me.PBLeg)
        Me.VideoPanel.Controls.Add(Me.AxWindowsMediaPlayer1)
        resources.ApplyResources(Me.VideoPanel, "VideoPanel")
        Me.VideoPanel.Name = "VideoPanel"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'bF1
        '
        resources.ApplyResources(Me.bF1, "bF1")
        Me.bF1.Name = "bF1"
        Me.bF1.UseVisualStyleBackColor = True
        '
        'bB1
        '
        resources.ApplyResources(Me.bB1, "bB1")
        Me.bB1.Name = "bB1"
        Me.bB1.UseVisualStyleBackColor = True
        '
        'ResizeUpdate
        '
        Me.ResizeUpdate.Interval = 200
        '
        'frmAdjustmentPlayer_new
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.bB1)
        Me.Controls.Add(Me.bF1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.VideoPanel)
        Me.Controls.Add(Me.numGPSDelta)
        Me.Controls.Add(Me.btnTransfer)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.lblVideoPos)
        Me.Controls.Add(Me.TrackBar1)
        Me.Controls.Add(Me.bSlow)
        Me.Controls.Add(Me.bFForward)
        Me.Controls.Add(Me.bPlay)
        Me.Controls.Add(Me.txtGPSDiff)
        Me.Controls.Add(Me.Label26)
        Me.Name = "frmAdjustmentPlayer_new"
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.PB_Zoom, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PBLeg, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numGPSDelta, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AxWindowsMediaPlayer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.VideoPanel.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents AxWindowsMediaPlayer1 As AxWMPLib.AxWindowsMediaPlayer
    Friend WithEvents txtGPSDiff As TextBox
    Friend WithEvents Label26 As Label
    Friend WithEvents bPlay As Button
    Friend WithEvents bFForward As Button
    Friend WithEvents bSlow As Button
    Friend WithEvents Timer1 As Timer
    Friend WithEvents TrackBar1 As TrackBar
    Friend WithEvents lblVideoPos As Label
    Friend WithEvents Timer2 As Timer
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents StatusLabel As ToolStripStatusLabel
    Friend WithEvents btnTransfer As Button
    Friend WithEvents PB_Zoom As PictureBox
    Friend WithEvents PBLeg As PictureBox
    Friend WithEvents MapImageTimer As Timer
    Friend WithEvents numGPSDelta As NumericUpDown
    Friend WithEvents VideoPanel As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents bF1 As Button
    Friend WithEvents bB1 As Button
    Friend WithEvents ResizeUpdate As Timer
End Class
