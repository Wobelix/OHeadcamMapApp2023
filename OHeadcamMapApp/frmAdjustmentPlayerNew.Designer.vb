<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmAdjustmentPlayer_new
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
        Me.PB_Zoom = New System.Windows.Forms.PictureBox()
        Me.PBLeg = New System.Windows.Forms.PictureBox()
        Me.MapImageTimer = New System.Windows.Forms.Timer(Me.components)
        Me.numGPSDelta = New System.Windows.Forms.NumericUpDown()
        Me.VideoPanel = New System.Windows.Forms.Panel()
        Me.pbHGraph = New System.Windows.Forms.PictureBox()
        Me.pbPulse = New System.Windows.Forms.PictureBox()
        Me.pbPace = New System.Windows.Forms.PictureBox()
        Me.pbDistance = New System.Windows.Forms.PictureBox()
        Me.pbTime = New System.Windows.Forms.PictureBox()
        Me.PBDyn = New System.Windows.Forms.PictureBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblMap2Active = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.VideoView1 = New LibVLCSharp.WinForms.VideoView()
        Me.bF1 = New System.Windows.Forms.Button()
        Me.bB1 = New System.Windows.Forms.Button()
        Me.ResizeUpdate = New System.Windows.Forms.Timer(Me.components)
        Me.bMapSettings = New System.Windows.Forms.Button()
        Me.cbZoom = New System.Windows.Forms.CheckBox()
        Me.cbLeg = New System.Windows.Forms.CheckBox()
        Me.btnSetMapFlipTime = New System.Windows.Forms.Button()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblMapFlipGPSMap1 = New System.Windows.Forms.Label()
        Me.grpMapFlip = New System.Windows.Forms.GroupBox()
        Me.btnMoveS1 = New System.Windows.Forms.Button()
        Me.btnMoveS2 = New System.Windows.Forms.Button()
        Me.lblMapFlipStarttime = New System.Windows.Forms.Label()
        Me.txtGPSDiff = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbJumpControl = New System.Windows.Forms.ComboBox()
        Me.cbDyn = New System.Windows.Forms.CheckBox()
        Me.btnWidgets = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnWdgTimeMinus = New System.Windows.Forms.Button()
        Me.btnWdgTimePlus = New System.Windows.Forms.Button()
        Me.btnWdgTime0 = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtWdgTimeOffset = New System.Windows.Forms.TextBox()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.PB_Zoom, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PBLeg, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numGPSDelta, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.VideoPanel.SuspendLayout()
        CType(Me.pbHGraph, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPulse, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPace, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbDistance, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbTime, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PBDyn, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.VideoView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpMapFlip.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
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
        resources.ApplyResources(Me.StatusStrip1, "StatusStrip1")
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel})
        Me.StatusStrip1.Name = "StatusStrip1"
        '
        'StatusLabel
        '
        resources.ApplyResources(Me.StatusLabel, "StatusLabel")
        Me.StatusLabel.Name = "StatusLabel"
        '
        'btnTransfer
        '
        resources.ApplyResources(Me.btnTransfer, "btnTransfer")
        Me.btnTransfer.Name = "btnTransfer"
        Me.btnTransfer.UseVisualStyleBackColor = True
        '
        'PB_Zoom
        '
        resources.ApplyResources(Me.PB_Zoom, "PB_Zoom")
        Me.PB_Zoom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PB_Zoom.Name = "PB_Zoom"
        Me.PB_Zoom.TabStop = False
        '
        'PBLeg
        '
        resources.ApplyResources(Me.PBLeg, "PBLeg")
        Me.PBLeg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
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
        Me.numGPSDelta.Minimum = New Decimal(New Integer() {2000, 0, 0, -2147483648})
        Me.numGPSDelta.Name = "numGPSDelta"
        '
        'VideoPanel
        '
        resources.ApplyResources(Me.VideoPanel, "VideoPanel")
        Me.VideoPanel.BackColor = System.Drawing.SystemColors.Desktop
        Me.VideoPanel.Controls.Add(Me.pbHGraph)
        Me.VideoPanel.Controls.Add(Me.pbPulse)
        Me.VideoPanel.Controls.Add(Me.pbPace)
        Me.VideoPanel.Controls.Add(Me.pbDistance)
        Me.VideoPanel.Controls.Add(Me.pbTime)
        Me.VideoPanel.Controls.Add(Me.PBDyn)
        Me.VideoPanel.Controls.Add(Me.Label6)
        Me.VideoPanel.Controls.Add(Me.lblMap2Active)
        Me.VideoPanel.Controls.Add(Me.Label3)
        Me.VideoPanel.Controls.Add(Me.Label2)
        Me.VideoPanel.Controls.Add(Me.Label1)
        Me.VideoPanel.Controls.Add(Me.PB_Zoom)
        Me.VideoPanel.Controls.Add(Me.PBLeg)
        Me.VideoPanel.Controls.Add(Me.VideoView1)
        Me.VideoPanel.Name = "VideoPanel"
        '
        'pbHGraph
        '
        resources.ApplyResources(Me.pbHGraph, "pbHGraph")
        Me.pbHGraph.Name = "pbHGraph"
        Me.pbHGraph.TabStop = False
        '
        'pbPulse
        '
        resources.ApplyResources(Me.pbPulse, "pbPulse")
        Me.pbPulse.Name = "pbPulse"
        Me.pbPulse.TabStop = False
        '
        'pbPace
        '
        resources.ApplyResources(Me.pbPace, "pbPace")
        Me.pbPace.Name = "pbPace"
        Me.pbPace.TabStop = False
        '
        'pbDistance
        '
        resources.ApplyResources(Me.pbDistance, "pbDistance")
        Me.pbDistance.Name = "pbDistance"
        Me.pbDistance.TabStop = False
        '
        'pbTime
        '
        resources.ApplyResources(Me.pbTime, "pbTime")
        Me.pbTime.Name = "pbTime"
        Me.pbTime.TabStop = False
        '
        'PBDyn
        '
        resources.ApplyResources(Me.PBDyn, "PBDyn")
        Me.PBDyn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PBDyn.Name = "PBDyn"
        Me.PBDyn.TabStop = False
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.ForeColor = System.Drawing.Color.GreenYellow
        Me.Label6.Name = "Label6"
        '
        'lblMap2Active
        '
        resources.ApplyResources(Me.lblMap2Active, "lblMap2Active")
        Me.lblMap2Active.BackColor = System.Drawing.Color.Red
        Me.lblMap2Active.ForeColor = System.Drawing.Color.Lime
        Me.lblMap2Active.Name = "lblMap2Active"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.ForeColor = System.Drawing.Color.White
        Me.Label3.Name = "Label3"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Name = "Label2"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Name = "Label1"
        '
        'VideoView1
        '
        resources.ApplyResources(Me.VideoView1, "VideoView1")
        Me.VideoView1.BackColor = System.Drawing.Color.Black
        Me.VideoView1.MediaPlayer = Nothing
        Me.VideoView1.Name = "VideoView1"
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
        'bMapSettings
        '
        resources.ApplyResources(Me.bMapSettings, "bMapSettings")
        Me.bMapSettings.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.bMapSettings.Name = "bMapSettings"
        Me.bMapSettings.UseVisualStyleBackColor = False
        '
        'cbZoom
        '
        resources.ApplyResources(Me.cbZoom, "cbZoom")
        Me.cbZoom.Checked = True
        Me.cbZoom.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbZoom.Name = "cbZoom"
        Me.cbZoom.UseVisualStyleBackColor = True
        '
        'cbLeg
        '
        resources.ApplyResources(Me.cbLeg, "cbLeg")
        Me.cbLeg.Checked = True
        Me.cbLeg.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbLeg.Name = "cbLeg"
        Me.cbLeg.UseVisualStyleBackColor = True
        '
        'btnSetMapFlipTime
        '
        resources.ApplyResources(Me.btnSetMapFlipTime, "btnSetMapFlipTime")
        Me.btnSetMapFlipTime.BackColor = System.Drawing.SystemColors.Info
        Me.btnSetMapFlipTime.Name = "btnSetMapFlipTime"
        Me.btnSetMapFlipTime.UseVisualStyleBackColor = False
        '
        'Label18
        '
        resources.ApplyResources(Me.Label18, "Label18")
        Me.Label18.Name = "Label18"
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'lblMapFlipGPSMap1
        '
        resources.ApplyResources(Me.lblMapFlipGPSMap1, "lblMapFlipGPSMap1")
        Me.lblMapFlipGPSMap1.Name = "lblMapFlipGPSMap1"
        '
        'grpMapFlip
        '
        resources.ApplyResources(Me.grpMapFlip, "grpMapFlip")
        Me.grpMapFlip.Controls.Add(Me.btnMoveS1)
        Me.grpMapFlip.Controls.Add(Me.btnMoveS2)
        Me.grpMapFlip.Controls.Add(Me.btnSetMapFlipTime)
        Me.grpMapFlip.Controls.Add(Me.lblMapFlipGPSMap1)
        Me.grpMapFlip.Controls.Add(Me.Label18)
        Me.grpMapFlip.Controls.Add(Me.Label4)
        Me.grpMapFlip.Controls.Add(Me.lblMapFlipStarttime)
        Me.grpMapFlip.Name = "grpMapFlip"
        Me.grpMapFlip.TabStop = False
        '
        'btnMoveS1
        '
        resources.ApplyResources(Me.btnMoveS1, "btnMoveS1")
        Me.btnMoveS1.Name = "btnMoveS1"
        Me.btnMoveS1.UseVisualStyleBackColor = True
        '
        'btnMoveS2
        '
        resources.ApplyResources(Me.btnMoveS2, "btnMoveS2")
        Me.btnMoveS2.Name = "btnMoveS2"
        Me.btnMoveS2.UseVisualStyleBackColor = True
        '
        'lblMapFlipStarttime
        '
        resources.ApplyResources(Me.lblMapFlipStarttime, "lblMapFlipStarttime")
        Me.lblMapFlipStarttime.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "MapFlipStartT", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.lblMapFlipStarttime.Name = "lblMapFlipStarttime"
        Me.lblMapFlipStarttime.Text = Global.OHeadcamMapApp.My.MySettings.Default.MapFlipStartT
        '
        'txtGPSDiff
        '
        resources.ApplyResources(Me.txtGPSDiff, "txtGPSDiff")
        Me.txtGPSDiff.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "GPXDiff", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtGPSDiff.Name = "txtGPSDiff"
        Me.txtGPSDiff.Text = Global.OHeadcamMapApp.My.MySettings.Default.GPXDiff
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'cmbJumpControl
        '
        resources.ApplyResources(Me.cmbJumpControl, "cmbJumpControl")
        Me.cmbJumpControl.FormattingEnabled = True
        Me.cmbJumpControl.Name = "cmbJumpControl"
        '
        'cbDyn
        '
        resources.ApplyResources(Me.cbDyn, "cbDyn")
        Me.cbDyn.Checked = True
        Me.cbDyn.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbDyn.Name = "cbDyn"
        Me.cbDyn.UseVisualStyleBackColor = True
        '
        'btnWidgets
        '
        resources.ApplyResources(Me.btnWidgets, "btnWidgets")
        Me.btnWidgets.Name = "btnWidgets"
        Me.btnWidgets.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Controls.Add(Me.btnWdgTimeMinus)
        Me.GroupBox1.Controls.Add(Me.btnWdgTimePlus)
        Me.GroupBox1.Controls.Add(Me.btnWdgTime0)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.txtWdgTimeOffset)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'btnWdgTimeMinus
        '
        resources.ApplyResources(Me.btnWdgTimeMinus, "btnWdgTimeMinus")
        Me.btnWdgTimeMinus.BackColor = System.Drawing.Color.Red
        Me.btnWdgTimeMinus.Name = "btnWdgTimeMinus"
        Me.btnWdgTimeMinus.UseVisualStyleBackColor = False
        '
        'btnWdgTimePlus
        '
        resources.ApplyResources(Me.btnWdgTimePlus, "btnWdgTimePlus")
        Me.btnWdgTimePlus.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnWdgTimePlus.Name = "btnWdgTimePlus"
        Me.btnWdgTimePlus.UseVisualStyleBackColor = False
        '
        'btnWdgTime0
        '
        resources.ApplyResources(Me.btnWdgTime0, "btnWdgTime0")
        Me.btnWdgTime0.Name = "btnWdgTime0"
        Me.btnWdgTime0.UseVisualStyleBackColor = True
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        '
        'txtWdgTimeOffset
        '
        resources.ApplyResources(Me.txtWdgTimeOffset, "txtWdgTimeOffset")
        Me.txtWdgTimeOffset.Name = "txtWdgTimeOffset"
        '
        'frmAdjustmentPlayer_new
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnWidgets)
        Me.Controls.Add(Me.cbDyn)
        Me.Controls.Add(Me.cmbJumpControl)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.grpMapFlip)
        Me.Controls.Add(Me.cbLeg)
        Me.Controls.Add(Me.cbZoom)
        Me.Controls.Add(Me.bMapSettings)
        Me.Controls.Add(Me.bB1)
        Me.Controls.Add(Me.bF1)
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
        Me.VideoPanel.ResumeLayout(False)
        Me.VideoPanel.PerformLayout()
        CType(Me.pbHGraph, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPulse, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPace, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbDistance, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbTime, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PBDyn, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.VideoView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpMapFlip.ResumeLayout(False)
        Me.grpMapFlip.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
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
    Friend WithEvents bF1 As Button
    Friend WithEvents bB1 As Button
    Friend WithEvents ResizeUpdate As Timer
    Friend WithEvents VideoView1 As LibVLCSharp.WinForms.VideoView
    Friend WithEvents bMapSettings As Button
    Friend WithEvents cbZoom As CheckBox
    Friend WithEvents cbLeg As CheckBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents btnSetMapFlipTime As Button
    Friend WithEvents lblMapFlipStarttime As Label
    Friend WithEvents Label18 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents lblMapFlipGPSMap1 As Label
    Friend WithEvents grpMapFlip As GroupBox
    Friend WithEvents btnMoveS1 As Button
    Friend WithEvents btnMoveS2 As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents cmbJumpControl As ComboBox
    Friend WithEvents lblMap2Active As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents PBDyn As PictureBox
    Friend WithEvents cbDyn As CheckBox
    Friend WithEvents btnWidgets As Button
    Friend WithEvents pbTime As PictureBox
    Friend WithEvents pbPulse As PictureBox
    Friend WithEvents pbPace As PictureBox
    Friend WithEvents pbDistance As PictureBox
    Friend WithEvents pbHGraph As PictureBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents btnWdgTime0 As Button
    Friend WithEvents Label7 As Label
    Friend WithEvents txtWdgTimeOffset As TextBox
    Friend WithEvents btnWdgTimeMinus As Button
    Friend WithEvents btnWdgTimePlus As Button
End Class
