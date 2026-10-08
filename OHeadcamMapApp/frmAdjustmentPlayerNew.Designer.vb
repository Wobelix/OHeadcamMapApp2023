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
        Me.lblMap2Active = New System.Windows.Forms.Label()
        Me.VideoView1 = New LibVLCSharp.WinForms.VideoView()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
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
        Me.pnlOutputLayout = New System.Windows.Forms.Panel()
        Me.tabOutputLayout = New System.Windows.Forms.TabControl()
        Me.tabOutputFormat = New System.Windows.Forms.TabPage()
        Me.chkShowSafeZone = New System.Windows.Forms.CheckBox()
        Me.cboPortraitMode = New System.Windows.Forms.ComboBox()
        Me.lblPortraitMode = New System.Windows.Forms.Label()
        Me.lblOutputInfo = New System.Windows.Forms.Label()
        Me.cboOutputResolution = New System.Windows.Forms.ComboBox()
        Me.lblOutputResolution = New System.Windows.Forms.Label()
        Me.cboOutputAspect = New System.Windows.Forms.ComboBox()
        Me.lblOutputAspect = New System.Windows.Forms.Label()
        Me.tabLayoutPresets = New System.Windows.Forms.TabPage()
        Me.btnManageLayoutPresets = New System.Windows.Forms.Button()
        Me.btnSaveLayoutPreset = New System.Windows.Forms.Button()
        Me.btnApplyLayoutPreset = New System.Windows.Forms.Button()
        Me.cboLayoutPreset = New System.Windows.Forms.ComboBox()
        Me.lblLayoutPreset = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.numMapAnimationFps = New System.Windows.Forms.NumericUpDown()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
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
        Me.pnlOutputLayout.SuspendLayout()
        Me.tabOutputLayout.SuspendLayout()
        Me.tabOutputFormat.SuspendLayout()
        Me.tabLayoutPresets.SuspendLayout()
        CType(Me.numMapAnimationFps, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.numGPSDelta.Minimum = New Decimal(New Integer() {2000, 0, 0, -2147483648})
        Me.numGPSDelta.Name = "numGPSDelta"
        '
        'VideoPanel
        '
        Me.VideoPanel.BackColor = System.Drawing.SystemColors.Desktop
        Me.VideoPanel.Controls.Add(Me.pbHGraph)
        Me.VideoPanel.Controls.Add(Me.pbPulse)
        Me.VideoPanel.Controls.Add(Me.pbPace)
        Me.VideoPanel.Controls.Add(Me.pbDistance)
        Me.VideoPanel.Controls.Add(Me.pbTime)
        Me.VideoPanel.Controls.Add(Me.PBDyn)
        Me.VideoPanel.Controls.Add(Me.lblMap2Active)
        Me.VideoPanel.Controls.Add(Me.PB_Zoom)
        Me.VideoPanel.Controls.Add(Me.PBLeg)
        Me.VideoPanel.Controls.Add(Me.VideoView1)
        resources.ApplyResources(Me.VideoPanel, "VideoPanel")
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
        Me.PBDyn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        resources.ApplyResources(Me.PBDyn, "PBDyn")
        Me.PBDyn.Name = "PBDyn"
        Me.PBDyn.TabStop = False
        '
        'lblMap2Active
        '
        resources.ApplyResources(Me.lblMap2Active, "lblMap2Active")
        Me.lblMap2Active.BackColor = System.Drawing.Color.Red
        Me.lblMap2Active.ForeColor = System.Drawing.Color.Lime
        Me.lblMap2Active.Name = "lblMap2Active"
        '
        'VideoView1
        '
        Me.VideoView1.BackColor = System.Drawing.Color.Black
        resources.ApplyResources(Me.VideoView1, "VideoView1")
        Me.VideoView1.MediaPlayer = Nothing
        Me.VideoView1.Name = "VideoView1"
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.ForeColor = System.Drawing.Color.IndianRed
        Me.Label6.Name = "Label6"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.ForeColor = System.Drawing.Color.IndianRed
        Me.Label3.Name = "Label3"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.ForeColor = System.Drawing.Color.IndianRed
        Me.Label2.Name = "Label2"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ForeColor = System.Drawing.Color.IndianRed
        Me.Label1.Name = "Label1"
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
        Me.bMapSettings.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        resources.ApplyResources(Me.bMapSettings, "bMapSettings")
        Me.bMapSettings.Name = "bMapSettings"
        Me.ToolTip1.SetToolTip(Me.bMapSettings, resources.GetString("bMapSettings.ToolTip"))
        Me.bMapSettings.UseVisualStyleBackColor = False
        '
        'cbZoom
        '
        resources.ApplyResources(Me.cbZoom, "cbZoom")
        Me.cbZoom.Checked = True
        Me.cbZoom.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbZoom.Name = "cbZoom"
        Me.ToolTip1.SetToolTip(Me.cbZoom, resources.GetString("cbZoom.ToolTip"))
        Me.cbZoom.UseVisualStyleBackColor = True
        '
        'cbLeg
        '
        resources.ApplyResources(Me.cbLeg, "cbLeg")
        Me.cbLeg.Checked = True
        Me.cbLeg.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbLeg.Name = "cbLeg"
        Me.ToolTip1.SetToolTip(Me.cbLeg, resources.GetString("cbLeg.ToolTip"))
        Me.cbLeg.UseVisualStyleBackColor = True
        '
        'btnSetMapFlipTime
        '
        Me.btnSetMapFlipTime.BackColor = System.Drawing.SystemColors.Info
        resources.ApplyResources(Me.btnSetMapFlipTime, "btnSetMapFlipTime")
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
        Me.grpMapFlip.Controls.Add(Me.btnMoveS1)
        Me.grpMapFlip.Controls.Add(Me.btnMoveS2)
        Me.grpMapFlip.Controls.Add(Me.btnSetMapFlipTime)
        Me.grpMapFlip.Controls.Add(Me.lblMapFlipGPSMap1)
        Me.grpMapFlip.Controls.Add(Me.Label18)
        Me.grpMapFlip.Controls.Add(Me.Label4)
        Me.grpMapFlip.Controls.Add(Me.lblMapFlipStarttime)
        resources.ApplyResources(Me.grpMapFlip, "grpMapFlip")
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
        Me.txtGPSDiff.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "GPXDiff", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.txtGPSDiff, "txtGPSDiff")
        Me.txtGPSDiff.Name = "txtGPSDiff"
        Me.txtGPSDiff.Text = Global.OHeadcamMapApp.My.MySettings.Default.GPXDiff
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        Me.ToolTip1.SetToolTip(Me.Label5, resources.GetString("Label5.ToolTip"))
        '
        'cmbJumpControl
        '
        Me.cmbJumpControl.FormattingEnabled = True
        resources.ApplyResources(Me.cmbJumpControl, "cmbJumpControl")
        Me.cmbJumpControl.Name = "cmbJumpControl"
        Me.ToolTip1.SetToolTip(Me.cmbJumpControl, resources.GetString("cmbJumpControl.ToolTip"))
        '
        'cbDyn
        '
        resources.ApplyResources(Me.cbDyn, "cbDyn")
        Me.cbDyn.Checked = True
        Me.cbDyn.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbDyn.Name = "cbDyn"
        Me.ToolTip1.SetToolTip(Me.cbDyn, resources.GetString("cbDyn.ToolTip"))
        Me.cbDyn.UseVisualStyleBackColor = True
        '
        'btnWidgets
        '
        resources.ApplyResources(Me.btnWidgets, "btnWidgets")
        Me.btnWidgets.Name = "btnWidgets"
        Me.ToolTip1.SetToolTip(Me.btnWidgets, resources.GetString("btnWidgets.ToolTip"))
        Me.btnWidgets.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnWdgTimeMinus)
        Me.GroupBox1.Controls.Add(Me.btnWdgTimePlus)
        Me.GroupBox1.Controls.Add(Me.btnWdgTime0)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.txtWdgTimeOffset)
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'btnWdgTimeMinus
        '
        Me.btnWdgTimeMinus.BackColor = System.Drawing.Color.Red
        resources.ApplyResources(Me.btnWdgTimeMinus, "btnWdgTimeMinus")
        Me.btnWdgTimeMinus.Name = "btnWdgTimeMinus"
        Me.btnWdgTimeMinus.UseVisualStyleBackColor = False
        '
        'btnWdgTimePlus
        '
        Me.btnWdgTimePlus.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        resources.ApplyResources(Me.btnWdgTimePlus, "btnWdgTimePlus")
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
        'pnlOutputLayout
        '
        Me.pnlOutputLayout.Controls.Add(Me.tabOutputLayout)
        resources.ApplyResources(Me.pnlOutputLayout, "pnlOutputLayout")
        Me.pnlOutputLayout.Name = "pnlOutputLayout"
        '
        'tabOutputLayout
        '
        Me.tabOutputLayout.Controls.Add(Me.tabOutputFormat)
        Me.tabOutputLayout.Controls.Add(Me.tabLayoutPresets)
        resources.ApplyResources(Me.tabOutputLayout, "tabOutputLayout")
        Me.tabOutputLayout.Name = "tabOutputLayout"
        Me.tabOutputLayout.SelectedIndex = 0
        '
        'tabOutputFormat
        '
        Me.tabOutputFormat.Controls.Add(Me.numMapAnimationFps)
        Me.tabOutputFormat.Controls.Add(Me.Label10)
        Me.tabOutputFormat.Controls.Add(Me.chkShowSafeZone)
        Me.tabOutputFormat.Controls.Add(Me.cboPortraitMode)
        Me.tabOutputFormat.Controls.Add(Me.lblPortraitMode)
        Me.tabOutputFormat.Controls.Add(Me.lblOutputInfo)
        Me.tabOutputFormat.Controls.Add(Me.cboOutputResolution)
        Me.tabOutputFormat.Controls.Add(Me.lblOutputResolution)
        Me.tabOutputFormat.Controls.Add(Me.cboOutputAspect)
        Me.tabOutputFormat.Controls.Add(Me.lblOutputAspect)
        resources.ApplyResources(Me.tabOutputFormat, "tabOutputFormat")
        Me.tabOutputFormat.Name = "tabOutputFormat"
        Me.tabOutputFormat.UseVisualStyleBackColor = True
        '
        'chkShowSafeZone
        '
        resources.ApplyResources(Me.chkShowSafeZone, "chkShowSafeZone")
        Me.chkShowSafeZone.Name = "chkShowSafeZone"
        Me.ToolTip1.SetToolTip(Me.chkShowSafeZone, resources.GetString("chkShowSafeZone.ToolTip"))
        Me.chkShowSafeZone.UseVisualStyleBackColor = True
        '
        'cboPortraitMode
        '
        Me.cboPortraitMode.FormattingEnabled = True
        Me.cboPortraitMode.Items.AddRange(New Object() {resources.GetString("cboPortraitMode.Items"), resources.GetString("cboPortraitMode.Items1")})
        resources.ApplyResources(Me.cboPortraitMode, "cboPortraitMode")
        Me.cboPortraitMode.Name = "cboPortraitMode"
        '
        'lblPortraitMode
        '
        resources.ApplyResources(Me.lblPortraitMode, "lblPortraitMode")
        Me.lblPortraitMode.Name = "lblPortraitMode"
        '
        'lblOutputInfo
        '
        resources.ApplyResources(Me.lblOutputInfo, "lblOutputInfo")
        Me.lblOutputInfo.ForeColor = System.Drawing.Color.DimGray
        Me.lblOutputInfo.Name = "lblOutputInfo"
        '
        'cboOutputResolution
        '
        Me.cboOutputResolution.FormattingEnabled = True
        Me.cboOutputResolution.Items.AddRange(New Object() {resources.GetString("cboOutputResolution.Items"), resources.GetString("cboOutputResolution.Items1")})
        resources.ApplyResources(Me.cboOutputResolution, "cboOutputResolution")
        Me.cboOutputResolution.Name = "cboOutputResolution"
        '
        'lblOutputResolution
        '
        resources.ApplyResources(Me.lblOutputResolution, "lblOutputResolution")
        Me.lblOutputResolution.Name = "lblOutputResolution"
        '
        'cboOutputAspect
        '
        Me.cboOutputAspect.FormattingEnabled = True
        Me.cboOutputAspect.Items.AddRange(New Object() {resources.GetString("cboOutputAspect.Items"), resources.GetString("cboOutputAspect.Items1")})
        resources.ApplyResources(Me.cboOutputAspect, "cboOutputAspect")
        Me.cboOutputAspect.Name = "cboOutputAspect"
        '
        'lblOutputAspect
        '
        resources.ApplyResources(Me.lblOutputAspect, "lblOutputAspect")
        Me.lblOutputAspect.Name = "lblOutputAspect"
        '
        'tabLayoutPresets
        '
        Me.tabLayoutPresets.Controls.Add(Me.btnManageLayoutPresets)
        Me.tabLayoutPresets.Controls.Add(Me.btnSaveLayoutPreset)
        Me.tabLayoutPresets.Controls.Add(Me.btnApplyLayoutPreset)
        Me.tabLayoutPresets.Controls.Add(Me.cboLayoutPreset)
        Me.tabLayoutPresets.Controls.Add(Me.lblLayoutPreset)
        resources.ApplyResources(Me.tabLayoutPresets, "tabLayoutPresets")
        Me.tabLayoutPresets.Name = "tabLayoutPresets"
        Me.tabLayoutPresets.UseVisualStyleBackColor = True
        '
        'btnManageLayoutPresets
        '
        resources.ApplyResources(Me.btnManageLayoutPresets, "btnManageLayoutPresets")
        Me.btnManageLayoutPresets.Name = "btnManageLayoutPresets"
        Me.btnManageLayoutPresets.UseVisualStyleBackColor = True
        '
        'btnSaveLayoutPreset
        '
        resources.ApplyResources(Me.btnSaveLayoutPreset, "btnSaveLayoutPreset")
        Me.btnSaveLayoutPreset.Name = "btnSaveLayoutPreset"
        Me.btnSaveLayoutPreset.UseVisualStyleBackColor = True
        '
        'btnApplyLayoutPreset
        '
        resources.ApplyResources(Me.btnApplyLayoutPreset, "btnApplyLayoutPreset")
        Me.btnApplyLayoutPreset.Name = "btnApplyLayoutPreset"
        Me.btnApplyLayoutPreset.UseVisualStyleBackColor = True
        '
        'cboLayoutPreset
        '
        Me.cboLayoutPreset.FormattingEnabled = True
        Me.cboLayoutPreset.Items.AddRange(New Object() {resources.GetString("cboLayoutPreset.Items"), resources.GetString("cboLayoutPreset.Items1")})
        resources.ApplyResources(Me.cboLayoutPreset, "cboLayoutPreset")
        Me.cboLayoutPreset.Name = "cboLayoutPreset"
        '
        'lblLayoutPreset
        '
        resources.ApplyResources(Me.lblLayoutPreset, "lblLayoutPreset")
        Me.lblLayoutPreset.Name = "lblLayoutPreset"
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.ForeColor = System.Drawing.Color.IndianRed
        Me.Label8.Name = "Label8"
        '
        'Label9
        '
        resources.ApplyResources(Me.Label9, "Label9")
        Me.Label9.ForeColor = System.Drawing.Color.IndianRed
        Me.Label9.Name = "Label9"
        '
        'Label10
        '
        resources.ApplyResources(Me.Label10, "Label10")
        Me.Label10.Name = "Label10"
        Me.ToolTip1.SetToolTip(Me.Label10, resources.GetString("Label10.ToolTip"))
        '
        'numMapAnimationFps
        '
        resources.ApplyResources(Me.numMapAnimationFps, "numMapAnimationFps")
        Me.numMapAnimationFps.Maximum = New Decimal(New Integer() {15, 0, 0, 0})
        Me.numMapAnimationFps.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numMapAnimationFps.Name = "numMapAnimationFps"
        Me.ToolTip1.SetToolTip(Me.numMapAnimationFps, resources.GetString("numMapAnimationFps.ToolTip"))
        Me.numMapAnimationFps.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'frmAdjustmentPlayer_new
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.pnlOutputLayout)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnWidgets)
        Me.Controls.Add(Me.cbDyn)
        Me.Controls.Add(Me.cmbJumpControl)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.grpMapFlip)
        Me.Controls.Add(Me.cbLeg)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cbZoom)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.bMapSettings)
        Me.Controls.Add(Me.Label1)
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
        Me.pnlOutputLayout.ResumeLayout(False)
        Me.tabOutputLayout.ResumeLayout(False)
        Me.tabOutputFormat.ResumeLayout(False)
        Me.tabOutputFormat.PerformLayout()
        Me.tabLayoutPresets.ResumeLayout(False)
        Me.tabLayoutPresets.PerformLayout()
        CType(Me.numMapAnimationFps, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents pnlOutputLayout As Panel
    Friend WithEvents tabOutputLayout As TabControl
    Friend WithEvents tabOutputFormat As TabPage
    Friend WithEvents lblOutputAspect As Label
    Friend WithEvents tabLayoutPresets As TabPage
    Friend WithEvents cboOutputResolution As ComboBox
    Friend WithEvents lblOutputResolution As Label
    Friend WithEvents cboOutputAspect As ComboBox
    Friend WithEvents lblOutputInfo As Label
    Friend WithEvents cboPortraitMode As ComboBox
    Friend WithEvents lblPortraitMode As Label
    Friend WithEvents chkShowSafeZone As CheckBox
    Friend WithEvents btnManageLayoutPresets As Button
    Friend WithEvents btnSaveLayoutPreset As Button
    Friend WithEvents btnApplyLayoutPreset As Button
    Friend WithEvents cboLayoutPreset As ComboBox
    Friend WithEvents lblLayoutPreset As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents numMapAnimationFps As NumericUpDown
    Friend WithEvents Label10 As Label
    Friend WithEvents ToolTip1 As ToolTip
End Class
