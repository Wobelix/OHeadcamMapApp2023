Imports System.Globalization

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Dim MySettings2 As OHeadcamMapApp.My.MySettings = New OHeadcamMapApp.My.MySettings()
        Me.OpenFileDialogVideo = New System.Windows.Forms.OpenFileDialog()
        Me.AddVideofile = New System.Windows.Forms.Button()
        Me.btnOutputFile = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btnQRxml = New System.Windows.Forms.Button()
        Me.btnQRimg = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.OpenFileDialoggpx = New System.Windows.Forms.OpenFileDialog()
        Me.OpenFileDialogqrimg = New System.Windows.Forms.OpenFileDialog()
        Me.OpenFileDialogxml = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialogOutput = New System.Windows.Forms.SaveFileDialog()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.numVideoTempo = New System.Windows.Forms.NumericUpDown()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.cbXDeshake = New System.Windows.Forms.CheckBox()
        Me.cbXJoin = New System.Windows.Forms.CheckBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.btnPrepareInput = New System.Windows.Forms.Button()
        Me.btnAddMusic = New System.Windows.Forms.Button()
        Me.btnMapSetting = New System.Windows.Forms.Button()
        Me.btnMakeAdjVideo = New System.Windows.Forms.Button()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.rb_deshake = New System.Windows.Forms.RadioButton()
        Me.RB_onlyfilter = New System.Windows.Forms.RadioButton()
        Me.btnMapFlip = New System.Windows.Forms.Button()
        Me.txtPrepareLength = New System.Windows.Forms.TextBox()
        Me.txtOutputLength = New System.Windows.Forms.TextBox()
        Me.chbNoAudio = New System.Windows.Forms.CheckBox()
        Me.txtInpCutfromStart = New System.Windows.Forms.TextBox()
        Me.LblGPSDiff = New System.Windows.Forms.Label()
        Me.txtQRImage2 = New System.Windows.Forms.TextBox()
        Me.txtXMLfile2 = New System.Windows.Forms.TextBox()
        Me.txtXMLfile = New System.Windows.Forms.TextBox()
        Me.txtQRImage = New System.Windows.Forms.TextBox()
        Me.txtVideolist = New System.Windows.Forms.TextBox()
        Me.btnReset = New System.Windows.Forms.Button()
        Me.btnClearVideolist = New System.Windows.Forms.Button()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.IndstillingerToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.LanguageToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DanskToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EnglishToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChineseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViKørselslogToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SideOmSideVideoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VideoafspillerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BrugTrackingvideoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FFPlayTestafspillerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChangeFPSScriptToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FixApplegpxFilToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OmToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OmToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.HjælpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.btnPostProcess = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblMapLength = New System.Windows.Forms.Label()
        Me.IconStatusPrepare = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.lblVideoInLength = New System.Windows.Forms.Label()
        Me.iconStatusVideoIn = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.btnMakeMapN = New System.Windows.Forms.Button()
        Me.cbOnlyVideo = New System.Windows.Forms.CheckBox()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.btnRunMakeVideo = New System.Windows.Forms.Button()
        Me.lblOutputVideoLength = New System.Windows.Forms.Label()
        Me.IconStatusOutput = New System.Windows.Forms.Label()
        Me.txtOutFilename = New System.Windows.Forms.TextBox()
        Me.GroupBox9 = New System.Windows.Forms.GroupBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.Statusbar = New System.Windows.Forms.ToolStripStatusLabel()
        Me.StatusProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.StatusBarProgressText = New System.Windows.Forms.ToolStripStatusLabel()
        Me.StatusRemaining = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.GroupBox10 = New System.Windows.Forms.GroupBox()
        Me.chkUseQuickRouteFiles = New System.Windows.Forms.CheckBox()
        Me.btnRouteEditor = New System.Windows.Forms.Button()
        Me.chkMapFlipOnOff = New System.Windows.Forms.CheckBox()
        Me.grpMapFlip = New System.Windows.Forms.GroupBox()
        Me.chkUseQuickRouteFiles2 = New System.Windows.Forms.CheckBox()
        Me.btnRouteEditor2 = New System.Windows.Forms.Button()
        Me.btnMapFlipinfo = New System.Windows.Forms.Button()
        Me.lblMapFlipGPXd = New System.Windows.Forms.Label()
        Me.lblMapFlipStarttime = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.btnQRimg2 = New System.Windows.Forms.Button()
        Me.btnQRXML2 = New System.Windows.Forms.Button()
        Me.lblTrackVideoInUse = New System.Windows.Forms.Label()
        Me.btnTestWriteVideo = New System.Windows.Forms.Button()
        Me.ToolTip2 = New System.Windows.Forms.ToolTip(Me.components)
        Me.TimerRGCheck = New System.Windows.Forms.Timer(Me.components)
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.TimerStatusRemaining = New System.Windows.Forms.Timer(Me.components)
        Me.TimerMapStatus = New System.Windows.Forms.Timer(Me.components)
        CType(Me.numVideoTempo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox7.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox6.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox9.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        Me.GroupBox10.SuspendLayout()
        Me.grpMapFlip.SuspendLayout()
        Me.SuspendLayout()
        '
        'OpenFileDialogVideo
        '
        resources.ApplyResources(Me.OpenFileDialogVideo, "OpenFileDialogVideo")
        Me.OpenFileDialogVideo.Multiselect = True
        '
        'AddVideofile
        '
        resources.ApplyResources(Me.AddVideofile, "AddVideofile")
        Me.AddVideofile.Name = "AddVideofile"
        Me.ToolTip1.SetToolTip(Me.AddVideofile, resources.GetString("AddVideofile.ToolTip"))
        Me.ToolTip2.SetToolTip(Me.AddVideofile, resources.GetString("AddVideofile.ToolTip1"))
        Me.AddVideofile.UseVisualStyleBackColor = True
        '
        'btnOutputFile
        '
        resources.ApplyResources(Me.btnOutputFile, "btnOutputFile")
        Me.btnOutputFile.Name = "btnOutputFile"
        Me.ToolTip1.SetToolTip(Me.btnOutputFile, resources.GetString("btnOutputFile.ToolTip"))
        Me.ToolTip2.SetToolTip(Me.btnOutputFile, resources.GetString("btnOutputFile.ToolTip1"))
        Me.btnOutputFile.UseVisualStyleBackColor = True
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        Me.ToolTip1.SetToolTip(Me.Label7, resources.GetString("Label7.ToolTip"))
        Me.ToolTip2.SetToolTip(Me.Label7, resources.GetString("Label7.ToolTip1"))
        '
        'btnQRxml
        '
        resources.ApplyResources(Me.btnQRxml, "btnQRxml")
        Me.btnQRxml.Name = "btnQRxml"
        Me.ToolTip1.SetToolTip(Me.btnQRxml, resources.GetString("btnQRxml.ToolTip"))
        Me.ToolTip2.SetToolTip(Me.btnQRxml, resources.GetString("btnQRxml.ToolTip1"))
        Me.btnQRxml.UseVisualStyleBackColor = True
        '
        'btnQRimg
        '
        resources.ApplyResources(Me.btnQRimg, "btnQRimg")
        Me.btnQRimg.Name = "btnQRimg"
        Me.ToolTip1.SetToolTip(Me.btnQRimg, resources.GetString("btnQRimg.ToolTip"))
        Me.ToolTip2.SetToolTip(Me.btnQRimg, resources.GetString("btnQRimg.ToolTip1"))
        Me.btnQRimg.UseVisualStyleBackColor = True
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        Me.ToolTip1.SetToolTip(Me.Label4, resources.GetString("Label4.ToolTip"))
        Me.ToolTip2.SetToolTip(Me.Label4, resources.GetString("Label4.ToolTip1"))
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        Me.ToolTip1.SetToolTip(Me.Label3, resources.GetString("Label3.ToolTip"))
        Me.ToolTip2.SetToolTip(Me.Label3, resources.GetString("Label3.ToolTip1"))
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        Me.ToolTip1.SetToolTip(Me.Label1, resources.GetString("Label1.ToolTip"))
        Me.ToolTip2.SetToolTip(Me.Label1, resources.GetString("Label1.ToolTip1"))
        '
        'OpenFileDialoggpx
        '
        resources.ApplyResources(Me.OpenFileDialoggpx, "OpenFileDialoggpx")
        '
        'OpenFileDialogqrimg
        '
        resources.ApplyResources(Me.OpenFileDialogqrimg, "OpenFileDialogqrimg")
        '
        'OpenFileDialogxml
        '
        resources.ApplyResources(Me.OpenFileDialogxml, "OpenFileDialogxml")
        '
        'SaveFileDialogOutput
        '
        Me.SaveFileDialogOutput.FileName = "Final.mp4"
        resources.ApplyResources(Me.SaveFileDialogOutput, "SaveFileDialogOutput")
        '
        'ToolTip1
        '
        Me.ToolTip1.AutomaticDelay = 100
        Me.ToolTip1.AutoPopDelay = 5000
        Me.ToolTip1.InitialDelay = 100
        Me.ToolTip1.IsBalloon = True
        Me.ToolTip1.ReshowDelay = 20
        '
        'numVideoTempo
        '
        resources.ApplyResources(Me.numVideoTempo, "numVideoTempo")
        Me.numVideoTempo.DecimalPlaces = 1
        Me.numVideoTempo.Increment = New Decimal(New Integer() {5, 0, 0, 65536})
        Me.numVideoTempo.Maximum = New Decimal(New Integer() {4, 0, 0, 0})
        Me.numVideoTempo.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.numVideoTempo.Name = "numVideoTempo"
        Me.ToolTip1.SetToolTip(Me.numVideoTempo, resources.GetString("numVideoTempo.ToolTip"))
        Me.numVideoTempo.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'Label13
        '
        resources.ApplyResources(Me.Label13, "Label13")
        Me.Label13.Name = "Label13"
        Me.ToolTip1.SetToolTip(Me.Label13, resources.GetString("Label13.ToolTip"))
        '
        'cbXDeshake
        '
        resources.ApplyResources(Me.cbXDeshake, "cbXDeshake")
        Me.cbXDeshake.Name = "cbXDeshake"
        Me.ToolTip1.SetToolTip(Me.cbXDeshake, resources.GetString("cbXDeshake.ToolTip"))
        Me.cbXDeshake.UseVisualStyleBackColor = True
        '
        'cbXJoin
        '
        resources.ApplyResources(Me.cbXJoin, "cbXJoin")
        Me.cbXJoin.Checked = True
        Me.cbXJoin.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbXJoin.Name = "cbXJoin"
        Me.ToolTip1.SetToolTip(Me.cbXJoin, resources.GetString("cbXJoin.ToolTip"))
        Me.cbXJoin.UseVisualStyleBackColor = True
        '
        'Label15
        '
        resources.ApplyResources(Me.Label15, "Label15")
        Me.Label15.Name = "Label15"
        Me.ToolTip1.SetToolTip(Me.Label15, resources.GetString("Label15.ToolTip"))
        '
        'btnPrepareInput
        '
        resources.ApplyResources(Me.btnPrepareInput, "btnPrepareInput")
        Me.btnPrepareInput.Name = "btnPrepareInput"
        Me.ToolTip1.SetToolTip(Me.btnPrepareInput, resources.GetString("btnPrepareInput.ToolTip"))
        Me.btnPrepareInput.UseVisualStyleBackColor = True
        '
        'btnAddMusic
        '
        resources.ApplyResources(Me.btnAddMusic, "btnAddMusic")
        Me.btnAddMusic.Name = "btnAddMusic"
        Me.ToolTip1.SetToolTip(Me.btnAddMusic, resources.GetString("btnAddMusic.ToolTip"))
        Me.btnAddMusic.UseVisualStyleBackColor = True
        '
        'btnMapSetting
        '
        resources.ApplyResources(Me.btnMapSetting, "btnMapSetting")
        Me.btnMapSetting.Name = "btnMapSetting"
        Me.ToolTip1.SetToolTip(Me.btnMapSetting, resources.GetString("btnMapSetting.ToolTip"))
        Me.btnMapSetting.UseVisualStyleBackColor = True
        '
        'btnMakeAdjVideo
        '
        resources.ApplyResources(Me.btnMakeAdjVideo, "btnMakeAdjVideo")
        Me.btnMakeAdjVideo.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnMakeAdjVideo.Name = "btnMakeAdjVideo"
        Me.ToolTip1.SetToolTip(Me.btnMakeAdjVideo, resources.GetString("btnMakeAdjVideo.ToolTip"))
        Me.btnMakeAdjVideo.UseVisualStyleBackColor = True
        '
        'GroupBox7
        '
        resources.ApplyResources(Me.GroupBox7, "GroupBox7")
        Me.GroupBox7.Controls.Add(Me.Button1)
        Me.GroupBox7.Controls.Add(Me.rb_deshake)
        Me.GroupBox7.Controls.Add(Me.RB_onlyfilter)
        Me.GroupBox7.Controls.Add(Me.cbXDeshake)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox7, resources.GetString("GroupBox7.ToolTip"))
        '
        'Button1
        '
        resources.ApplyResources(Me.Button1, "Button1")
        Me.Button1.Name = "Button1"
        Me.ToolTip1.SetToolTip(Me.Button1, resources.GetString("Button1.ToolTip"))
        Me.Button1.UseVisualStyleBackColor = True
        '
        'rb_deshake
        '
        resources.ApplyResources(Me.rb_deshake, "rb_deshake")
        Me.rb_deshake.Name = "rb_deshake"
        Me.ToolTip1.SetToolTip(Me.rb_deshake, resources.GetString("rb_deshake.ToolTip"))
        Me.rb_deshake.UseVisualStyleBackColor = True
        '
        'RB_onlyfilter
        '
        resources.ApplyResources(Me.RB_onlyfilter, "RB_onlyfilter")
        Me.RB_onlyfilter.Checked = True
        Me.RB_onlyfilter.Name = "RB_onlyfilter"
        Me.RB_onlyfilter.TabStop = True
        Me.ToolTip1.SetToolTip(Me.RB_onlyfilter, resources.GetString("RB_onlyfilter.ToolTip"))
        Me.RB_onlyfilter.UseVisualStyleBackColor = True
        '
        'btnMapFlip
        '
        resources.ApplyResources(Me.btnMapFlip, "btnMapFlip")
        Me.btnMapFlip.Name = "btnMapFlip"
        Me.ToolTip1.SetToolTip(Me.btnMapFlip, resources.GetString("btnMapFlip.ToolTip"))
        Me.btnMapFlip.UseVisualStyleBackColor = True
        '
        'txtPrepareLength
        '
        resources.ApplyResources(Me.txtPrepareLength, "txtPrepareLength")
        Me.txtPrepareLength.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "PrepareLength", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtPrepareLength.Name = "txtPrepareLength"
        Me.txtPrepareLength.Text = Global.OHeadcamMapApp.My.MySettings.Default.PrepareLength
        Me.ToolTip1.SetToolTip(Me.txtPrepareLength, resources.GetString("txtPrepareLength.ToolTip"))
        '
        'txtOutputLength
        '
        resources.ApplyResources(Me.txtOutputLength, "txtOutputLength")
        Me.txtOutputLength.BackColor = System.Drawing.SystemColors.Window
        Me.txtOutputLength.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "OutputLength", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtOutputLength.Name = "txtOutputLength"
        Me.txtOutputLength.Text = Global.OHeadcamMapApp.My.MySettings.Default.OutputLength
        Me.ToolTip1.SetToolTip(Me.txtOutputLength, resources.GetString("txtOutputLength.ToolTip"))
        '
        'chbNoAudio
        '
        resources.ApplyResources(Me.chbNoAudio, "chbNoAudio")
        Me.chbNoAudio.Checked = Global.OHeadcamMapApp.My.MySettings.Default.NoAudio
        Me.chbNoAudio.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.OHeadcamMapApp.My.MySettings.Default, "NoAudio", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.chbNoAudio.Name = "chbNoAudio"
        Me.ToolTip1.SetToolTip(Me.chbNoAudio, resources.GetString("chbNoAudio.ToolTip"))
        Me.chbNoAudio.UseVisualStyleBackColor = True
        '
        'txtInpCutfromStart
        '
        resources.ApplyResources(Me.txtInpCutfromStart, "txtInpCutfromStart")
        MySettings2.AdjTestVideoReady = False
        MySettings2.AdjVideoGPSDiff = "0"
        MySettings2.AdjVideoLength = "300"
        MySettings2.AppVersion = ""
        MySettings2.Audiolevel = ""
        MySettings2.AudioVideofile = ""
        MySettings2.bAdjVideoIsReady = False
        MySettings2.bDoMusic = False
        MySettings2.bOutputfileReady = False
        MySettings2.bUseMapTrackingVideo = False
        MySettings2.cbHeightGraph = True
        MySettings2.cbLoopMusic = True
        MySettings2.cbSetImgM = False
        MySettings2.cbShowDynamicMap = True
        MySettings2.cbShowLegMAp = False
        MySettings2.cbShowRoute = False
        MySettings2.cbShowSpeedPanel = True
        MySettings2.cbXMakeframes = False
        MySettings2.chkHDFormat = False
        MySettings2.ckShowLegMap = True
        MySettings2.ckShowRouteMap = True
        MySettings2.dFfmpegScaleMap = New Decimal(New Integer() {1, 0, 0, 0})
        MySettings2.ffmpegBufsize = "40M"
        MySettings2.ffmpegCRF = "26"
        MySettings2.ffmpegJoin = "-f concat -safe 0 -i joinlist.txt"
        MySettings2.ffmpegLensCorrection = "lenscorrection=cx=0.5:cy=0.5:k1=-0.200:k2=0.000"
        MySettings2.ffmpegMaxBitr = "20M"
        MySettings2.ffmpegOutFps = "25"
        MySettings2.ffmpegPreset = "fast"
        MySettings2.ffmpegScaleMap = "1.5"
        MySettings2.ffmpegVidstabDetect = "vidstabdetect=shakiness=10:accuracy=15:mincontrast=0.200:stepsize=6:show=0:result" &
    "=data.trf:tripod=0:result=data.trf"
        MySettings2.ffmpegVidstabDetectOut = " -f null -"
        MySettings2.ffmpegVidstabTransform = "vidstabtransform=smoothing=25:crop=black:zoom=0:optzoom=0:interpol='bicubic':inpu" &
    "t=data.trf:tripod=0,unsharp=7:7:3:7:7:3"
        MySettings2.GPXDiff = "0"
        MySettings2.GPXDiff1 = "0"
        MySettings2.GPXDiff2 = "0"
        MySettings2.GPXfile = ""
        MySettings2.InfoOutVideoFormat = "Format: 1920x1080"
        MySettings2.JoinFileHeight = 1080
        MySettings2.JoinFileLength = 0!
        MySettings2.JoinFileWidth = 1920
        MySettings2.LandscapeOutputResolutionIndex = 0
        MySettings2.LandscapeOverlayVisibility = ""
        MySettings2.Language = ""
        MySettings2.LayoutPresetIndex = 0
        MySettings2.lblMapLength = "0:00:00"
        MySettings2.MapFlipActive = False
        MySettings2.MapFlipOnOff = False
        MySettings2.MapFlipStartS = "0"
        MySettings2.MapFlipStartT = "0:00:00"
        MySettings2.MapLength = "0:00:00"
        MySettings2.MHeightH = "10"
        MySettings2.MHeightV = "10"
        MySettings2.MIArrowBarb = 0.3R
        MySettings2.MIArrowOutlineScale = 2
        MySettings2.MIArrowWidth = 0.9R
        MySettings2.MIDotColor = System.Drawing.Color.Red
        MySettings2.MIDotSize = 17
        MySettings2.MIDotType = "Arrow"
        MySettings2.MIDynamicHeight = 430
        MySettings2.MIDynamicLookAheadSeconds = 45.0R
        MySettings2.MIDynamicLookBehindSeconds = 20.0R
        MySettings2.MIDynamicMapPos = New System.Drawing.Point(80, 800)
        MySettings2.MIDynamicMargin = 150
        MySettings2.MIDynamicMaxZoom = 250
        MySettings2.MIDynamicMinZoom = 100
        MySettings2.MIDynamicRad = 80
        MySettings2.MIDynamicTransparency = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings2.MIDynamicWidth = 430
        MySettings2.MIDynamicZoom = 1.0R
        MySettings2.MIFLegDim = New System.Drawing.Point(0, 0)
        MySettings2.MIFLegPos = New System.Drawing.Point(0, 0)
        MySettings2.MIFrameColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        MySettings2.MIFrameFeather = False
        MySettings2.MIFrameWidth = 1
        MySettings2.MIFZoomDim = New System.Drawing.Point(0, 0)
        MySettings2.MIFZoomPos = New System.Drawing.Point(0, 0)
        MySettings2.MILegHeight = 600
        MySettings2.MILegMapPos = New System.Drawing.Point(80, 450)
        MySettings2.MILegMargin = 50
        MySettings2.MILegRad = 40
        MySettings2.MILegRenderLayout = "web"
        MySettings2.MILegTransparency = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings2.MILegWidth = 350
        MySettings2.MIMapFrameFormIndex = 0
        MySettings2.MIPaceFastMinPerKm = 5.0R
        MySettings2.MIPaceSlowMinPerKm = 14.0R
        MySettings2.MISmoothFrameStepSeconds = 0.1R
        MySettings2.MISmoothParallelGeneration = True
        MySettings2.MITailColor = System.Drawing.Color.Red
        MySettings2.MITailDuration = 30
        MySettings2.MITailRatio = New Decimal(New Integer() {6, 0, 0, 65536})
        MySettings2.MITailTransparency = 0.4R
        MySettings2.MITailUseSpeedColors = True
        MySettings2.MIWidgetDistanceEnabled = True
        MySettings2.MIWidgetDistanceHeight = 82
        MySettings2.MIWidgetDistanceMapPos = New System.Drawing.Point(1160, 175)
        MySettings2.MIWidgetDistanceWidth = 240
        MySettings2.MIWidgetHGraphEnabled = True
        MySettings2.MIWidgetHGraphHeight = 150
        MySettings2.MIWidgetHGraphMapPos = New System.Drawing.Point(880, 870)
        MySettings2.MIWidgetHGraphWidth = 720
        MySettings2.MIWidgetPaceEnabled = True
        MySettings2.MIWidgetPaceHeight = 82
        MySettings2.MIWidgetPaceMapPos = New System.Drawing.Point(1160, 270)
        MySettings2.MIWidgetPaceWidth = 240
        MySettings2.MIWidgetPulseEnabled = True
        MySettings2.MIWidgetPulseHeight = 82
        MySettings2.MIWidgetPulseMapPos = New System.Drawing.Point(1160, 365)
        MySettings2.MIWidgetPulseWidth = 190
        MySettings2.MIWidgetTimeEnabled = True
        MySettings2.MIWidgetTimeHeight = 82
        MySettings2.MIWidgetTimeMapPos = New System.Drawing.Point(1160, 80)
        MySettings2.MIWidgetTimeOffsetSeconds = 0R
        MySettings2.MIWidgetTimeWidth = 220
        MySettings2.MIZoomCircle = True
        MySettings2.MIZoomHeight = 350
        MySettings2.MIZoomMapPos = New System.Drawing.Point(80, 100)
        MySettings2.MIZoomRad = 80
        MySettings2.MIZoomTransparency = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings2.MIZoomWidth = 350
        MySettings2.MIZoomZoom = 1.0R
        MySettings2.MLegMapH = "10"
        MySettings2.MlegMapH2 = "10"
        MySettings2.MLegMapV = "10"
        MySettings2.MRouteH = "10"
        MySettings2.MRouteV = "10"
        MySettings2.MSpeedH = "10"
        MySettings2.MSpeedV = "10"
        MySettings2.MusicAudiofile = ""
        MySettings2.MusicFFMpegParam = ""
        MySettings2.Musicfile = ""
        MySettings2.Musiclevel = ""
        MySettings2.MusicOutputfile = ""
        MySettings2.No_deshake_filter = "normalize=blackpt=black:whitept=white:smoothing=10:strength=0.5"
        MySettings2.NoAudio = False
        MySettings2.OutputAspectIndex = 0
        MySettings2.Outputfile = ""
        MySettings2.OutputFormat = "Auto"
        MySettings2.OutputLength = ""
        MySettings2.OutputResolutionIndex = 0
        MySettings2.OutputVideoLength = "0:00:00"
        MySettings2.OverlayEstimate2KSeconds = 0R
        MySettings2.OverlayEstimate4KSeconds = 0R
        MySettings2.OverlayEstimateHdSeconds = 0R
        MySettings2.PortraitFramePosition = 0.5R
        MySettings2.PortraitModeIndex = 0
        MySettings2.PortraitOverlayLayout = ""
        MySettings2.PortraitOverlayVisibility = ""
        MySettings2.PortraitVideoPosition = 0R
        MySettings2.PostVideo = ""
        MySettings2.PostVideoList = ""
        MySettings2.PrepareLength = ""
        MySettings2.QRimage = ""
        MySettings2.QRimage1 = ""
        MySettings2.QRImage2 = ""
        MySettings2.QRLength = "0"
        MySettings2.QRXML = ""
        MySettings2.QRXML1 = ""
        MySettings2.QRXML2 = ""
        MySettings2.QRXMLTimesec = 0
        MySettings2.QRXMLTimesec2 = 0
        MySettings2.rbSetImgL = False
        MySettings2.rbSetImgR = False
        MySettings2.SbSLength = ""
        MySettings2.SbSLfile = ""
        MySettings2.SbSOutfile = "Out.mp4"
        MySettings2.SbSRfile = ""
        MySettings2.SettingSbSCode = "-filter_complex ""[0:v]scale=-1:1080[v0];[1:v]scale=-1:1080[v1];[v0][v1]hstack"""
        MySettings2.SettingsKey = ""
        MySettings2.SettingsUpgradeRequired = True
        MySettings2.ShowOutputSafeZone = True
        MySettings2.StatusInputVideo = ""
        MySettings2.StatusMakeMap = ""
        MySettings2.StatusOutputVideo = ""
        MySettings2.StatusPrepare = ""
        MySettings2.trackAudiolevel = 0
        MySettings2.TrackMapHeight = 0
        MySettings2.TrackMapImageFile = ""
        MySettings2.TrackMapVideoFilename = ""
        MySettings2.TrackMapVideoLength = "0:00:00"
        MySettings2.TrackMapWidth = 0
        MySettings2.trackMusiclevel = 0
        MySettings2.Transparency = New Decimal(New Integer() {1, 0, 0, 0})
        MySettings2.txtGPSPos = ""
        MySettings2.txtInpCutFromStart = ""
        MySettings2.txtMapVideoFilename = ""
        MySettings2.txtMusicOutputfile = ""
        MySettings2.txtRealtimeFactor = ""
        MySettings2.txtVideoPos = ""
        MySettings2.VidAdj_Rot = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings2.VidAdj_Zoom = New Decimal(New Integer() {100, 0, 0, 0})
        MySettings2.VidAdjBright = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings2.VidAdjColImpr = False
        MySettings2.VidAdjContr = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings2.VidAdjGamma = New Decimal(New Integer() {1, 0, 0, 0})
        MySettings2.VidAdjHue = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings2.VidAdjLensDist = False
        MySettings2.VidAdjNoise = False
        MySettings2.VidAdjSatur = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings2.VidAdjSharp = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings2.VideoEncoderSpeed = "Balanced"
        MySettings2.Videofiles = ""
        MySettings2.videofiles2 = ""
        MySettings2.Videofilesfull = ""
        MySettings2.VideoInLength = "0:00:00"
        MySettings2.VideoPadding = "C"
        MySettings2.VideoQuality = "High"
        MySettings2.VideoWorkFolder = ""
        Me.txtInpCutfromStart.DataBindings.Add(New System.Windows.Forms.Binding("Text", MySettings2, "txtInpCutFromStart", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtInpCutfromStart.Name = "txtInpCutfromStart"
        Me.txtInpCutfromStart.Text = MySettings2.txtInpCutFromStart
        Me.ToolTip1.SetToolTip(Me.txtInpCutfromStart, resources.GetString("txtInpCutfromStart.ToolTip"))
        '
        'LblGPSDiff
        '
        resources.ApplyResources(Me.LblGPSDiff, "LblGPSDiff")
        Me.LblGPSDiff.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "GPXDiff", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.LblGPSDiff.Name = "LblGPSDiff"
        Me.LblGPSDiff.Text = Global.OHeadcamMapApp.My.MySettings.Default.GPXDiff
        Me.ToolTip1.SetToolTip(Me.LblGPSDiff, resources.GetString("LblGPSDiff.ToolTip"))
        '
        'txtQRImage2
        '
        resources.ApplyResources(Me.txtQRImage2, "txtQRImage2")
        Me.txtQRImage2.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "QRImage2", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtQRImage2.Name = "txtQRImage2"
        Me.txtQRImage2.Text = Global.OHeadcamMapApp.My.MySettings.Default.QRImage2
        Me.ToolTip1.SetToolTip(Me.txtQRImage2, resources.GetString("txtQRImage2.ToolTip"))
        '
        'txtXMLfile2
        '
        resources.ApplyResources(Me.txtXMLfile2, "txtXMLfile2")
        Me.txtXMLfile2.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "QRXML2", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtXMLfile2.Name = "txtXMLfile2"
        Me.txtXMLfile2.Text = Global.OHeadcamMapApp.My.MySettings.Default.QRXML2
        Me.ToolTip1.SetToolTip(Me.txtXMLfile2, resources.GetString("txtXMLfile2.ToolTip"))
        '
        'txtXMLfile
        '
        resources.ApplyResources(Me.txtXMLfile, "txtXMLfile")
        Me.txtXMLfile.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "QRXML1", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtXMLfile.Name = "txtXMLfile"
        Me.txtXMLfile.Text = Global.OHeadcamMapApp.My.MySettings.Default.QRXML1
        Me.ToolTip1.SetToolTip(Me.txtXMLfile, resources.GetString("txtXMLfile.ToolTip"))
        '
        'txtQRImage
        '
        resources.ApplyResources(Me.txtQRImage, "txtQRImage")
        Me.txtQRImage.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "QRimage1", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtQRImage.Name = "txtQRImage"
        Me.txtQRImage.Text = Global.OHeadcamMapApp.My.MySettings.Default.QRimage1
        Me.ToolTip1.SetToolTip(Me.txtQRImage, resources.GetString("txtQRImage.ToolTip"))
        '
        'txtVideolist
        '
        resources.ApplyResources(Me.txtVideolist, "txtVideolist")
        Me.txtVideolist.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "videofiles2", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtVideolist.Name = "txtVideolist"
        Me.txtVideolist.Text = Global.OHeadcamMapApp.My.MySettings.Default.videofiles2
        Me.ToolTip1.SetToolTip(Me.txtVideolist, resources.GetString("txtVideolist.ToolTip"))
        '
        'btnReset
        '
        resources.ApplyResources(Me.btnReset, "btnReset")
        Me.btnReset.Name = "btnReset"
        Me.ToolTip1.SetToolTip(Me.btnReset, resources.GetString("btnReset.ToolTip"))
        Me.btnReset.UseVisualStyleBackColor = True
        '
        'btnClearVideolist
        '
        resources.ApplyResources(Me.btnClearVideolist, "btnClearVideolist")
        Me.btnClearVideolist.Name = "btnClearVideolist"
        Me.ToolTip1.SetToolTip(Me.btnClearVideolist, resources.GetString("btnClearVideolist.ToolTip"))
        Me.btnClearVideolist.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        resources.ApplyResources(Me.MenuStrip1, "MenuStrip1")
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.IndstillingerToolStripMenuItem1, Me.LanguageToolStripMenuItem, Me.ViKørselslogToolStripMenuItem, Me.ToolStripMenuItem1, Me.OmToolStripMenuItem})
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.ToolTip1.SetToolTip(Me.MenuStrip1, resources.GetString("MenuStrip1.ToolTip"))
        '
        'IndstillingerToolStripMenuItem1
        '
        resources.ApplyResources(Me.IndstillingerToolStripMenuItem1, "IndstillingerToolStripMenuItem1")
        Me.IndstillingerToolStripMenuItem1.Name = "IndstillingerToolStripMenuItem1"
        '
        'LanguageToolStripMenuItem
        '
        resources.ApplyResources(Me.LanguageToolStripMenuItem, "LanguageToolStripMenuItem")
        Me.LanguageToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DanskToolStripMenuItem, Me.EnglishToolStripMenuItem, Me.ChineseToolStripMenuItem})
        Me.LanguageToolStripMenuItem.Name = "LanguageToolStripMenuItem"
        '
        'DanskToolStripMenuItem
        '
        resources.ApplyResources(Me.DanskToolStripMenuItem, "DanskToolStripMenuItem")
        Me.DanskToolStripMenuItem.Name = "DanskToolStripMenuItem"
        '
        'EnglishToolStripMenuItem
        '
        resources.ApplyResources(Me.EnglishToolStripMenuItem, "EnglishToolStripMenuItem")
        Me.EnglishToolStripMenuItem.Name = "EnglishToolStripMenuItem"
        '
        'ChineseToolStripMenuItem
        '
        resources.ApplyResources(Me.ChineseToolStripMenuItem, "ChineseToolStripMenuItem")
        Me.ChineseToolStripMenuItem.Name = "ChineseToolStripMenuItem"
        '
        'ViKørselslogToolStripMenuItem
        '
        resources.ApplyResources(Me.ViKørselslogToolStripMenuItem, "ViKørselslogToolStripMenuItem")
        Me.ViKørselslogToolStripMenuItem.Name = "ViKørselslogToolStripMenuItem"
        '
        'ToolStripMenuItem1
        '
        resources.ApplyResources(Me.ToolStripMenuItem1, "ToolStripMenuItem1")
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SideOmSideVideoToolStripMenuItem, Me.VideoafspillerToolStripMenuItem, Me.BrugTrackingvideoToolStripMenuItem, Me.FFPlayTestafspillerToolStripMenuItem, Me.ChangeFPSScriptToolStripMenuItem, Me.FixApplegpxFilToolStripMenuItem})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        '
        'SideOmSideVideoToolStripMenuItem
        '
        resources.ApplyResources(Me.SideOmSideVideoToolStripMenuItem, "SideOmSideVideoToolStripMenuItem")
        Me.SideOmSideVideoToolStripMenuItem.Name = "SideOmSideVideoToolStripMenuItem"
        '
        'VideoafspillerToolStripMenuItem
        '
        resources.ApplyResources(Me.VideoafspillerToolStripMenuItem, "VideoafspillerToolStripMenuItem")
        Me.VideoafspillerToolStripMenuItem.Name = "VideoafspillerToolStripMenuItem"
        '
        'BrugTrackingvideoToolStripMenuItem
        '
        resources.ApplyResources(Me.BrugTrackingvideoToolStripMenuItem, "BrugTrackingvideoToolStripMenuItem")
        Me.BrugTrackingvideoToolStripMenuItem.Name = "BrugTrackingvideoToolStripMenuItem"
        '
        'FFPlayTestafspillerToolStripMenuItem
        '
        resources.ApplyResources(Me.FFPlayTestafspillerToolStripMenuItem, "FFPlayTestafspillerToolStripMenuItem")
        Me.FFPlayTestafspillerToolStripMenuItem.Name = "FFPlayTestafspillerToolStripMenuItem"
        '
        'ChangeFPSScriptToolStripMenuItem
        '
        resources.ApplyResources(Me.ChangeFPSScriptToolStripMenuItem, "ChangeFPSScriptToolStripMenuItem")
        Me.ChangeFPSScriptToolStripMenuItem.Name = "ChangeFPSScriptToolStripMenuItem"
        '
        'FixApplegpxFilToolStripMenuItem
        '
        resources.ApplyResources(Me.FixApplegpxFilToolStripMenuItem, "FixApplegpxFilToolStripMenuItem")
        Me.FixApplegpxFilToolStripMenuItem.Name = "FixApplegpxFilToolStripMenuItem"
        '
        'OmToolStripMenuItem
        '
        resources.ApplyResources(Me.OmToolStripMenuItem, "OmToolStripMenuItem")
        Me.OmToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OmToolStripMenuItem1, Me.HjælpToolStripMenuItem})
        Me.OmToolStripMenuItem.Name = "OmToolStripMenuItem"
        '
        'OmToolStripMenuItem1
        '
        resources.ApplyResources(Me.OmToolStripMenuItem1, "OmToolStripMenuItem1")
        Me.OmToolStripMenuItem1.Name = "OmToolStripMenuItem1"
        '
        'HjælpToolStripMenuItem
        '
        resources.ApplyResources(Me.HjælpToolStripMenuItem, "HjælpToolStripMenuItem")
        Me.HjælpToolStripMenuItem.Name = "HjælpToolStripMenuItem"
        '
        'GroupBox4
        '
        resources.ApplyResources(Me.GroupBox4, "GroupBox4")
        Me.GroupBox4.Controls.Add(Me.btnPostProcess)
        Me.GroupBox4.Controls.Add(Me.GroupBox1)
        Me.GroupBox4.Controls.Add(Me.GroupBox6)
        Me.GroupBox4.Controls.Add(Me.GroupBox9)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox4, resources.GetString("GroupBox4.ToolTip"))
        '
        'btnPostProcess
        '
        resources.ApplyResources(Me.btnPostProcess, "btnPostProcess")
        Me.btnPostProcess.BackColor = System.Drawing.SystemColors.Highlight
        Me.btnPostProcess.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnPostProcess.Name = "btnPostProcess"
        Me.ToolTip1.SetToolTip(Me.btnPostProcess, resources.GetString("btnPostProcess.ToolTip"))
        Me.btnPostProcess.UseVisualStyleBackColor = False
        '
        'GroupBox1
        '
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.BackColor = System.Drawing.Color.MistyRose
        Me.GroupBox1.Controls.Add(Me.lblMapLength)
        Me.GroupBox1.Controls.Add(Me.IconStatusPrepare)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.txtPrepareLength)
        Me.GroupBox1.Controls.Add(Me.PictureBox1)
        Me.GroupBox1.Controls.Add(Me.Label15)
        Me.GroupBox1.Controls.Add(Me.btnPrepareInput)
        Me.GroupBox1.Controls.Add(Me.lblVideoInLength)
        Me.GroupBox1.Controls.Add(Me.iconStatusVideoIn)
        Me.GroupBox1.Controls.Add(Me.GroupBox7)
        Me.GroupBox1.Controls.Add(Me.Label21)
        Me.GroupBox1.Controls.Add(Me.cbXJoin)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox1, resources.GetString("GroupBox1.ToolTip"))
        '
        'lblMapLength
        '
        resources.ApplyResources(Me.lblMapLength, "lblMapLength")
        Me.lblMapLength.Name = "lblMapLength"
        Me.ToolTip1.SetToolTip(Me.lblMapLength, resources.GetString("lblMapLength.ToolTip"))
        '
        'IconStatusPrepare
        '
        resources.ApplyResources(Me.IconStatusPrepare, "IconStatusPrepare")
        Me.IconStatusPrepare.Image = Global.OHeadcamMapApp.My.Resources.Resources.imgNotready
        Me.IconStatusPrepare.Name = "IconStatusPrepare"
        Me.ToolTip1.SetToolTip(Me.IconStatusPrepare, resources.GetString("IconStatusPrepare.ToolTip"))
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        Me.ToolTip1.SetToolTip(Me.Label6, resources.GetString("Label6.ToolTip"))
        '
        'PictureBox1
        '
        resources.ApplyResources(Me.PictureBox1, "PictureBox1")
        Me.PictureBox1.Image = Global.OHeadcamMapApp.My.Resources.Resources.img1
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox1, resources.GetString("PictureBox1.ToolTip"))
        '
        'lblVideoInLength
        '
        resources.ApplyResources(Me.lblVideoInLength, "lblVideoInLength")
        Me.lblVideoInLength.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "VideoInLength", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.lblVideoInLength.Name = "lblVideoInLength"
        Me.lblVideoInLength.Text = Global.OHeadcamMapApp.My.MySettings.Default.VideoInLength
        Me.ToolTip1.SetToolTip(Me.lblVideoInLength, resources.GetString("lblVideoInLength.ToolTip"))
        '
        'iconStatusVideoIn
        '
        resources.ApplyResources(Me.iconStatusVideoIn, "iconStatusVideoIn")
        Me.iconStatusVideoIn.Image = Global.OHeadcamMapApp.My.Resources.Resources.imgNotready
        Me.iconStatusVideoIn.Name = "iconStatusVideoIn"
        Me.ToolTip1.SetToolTip(Me.iconStatusVideoIn, resources.GetString("iconStatusVideoIn.ToolTip"))
        '
        'Label21
        '
        resources.ApplyResources(Me.Label21, "Label21")
        Me.Label21.Name = "Label21"
        Me.ToolTip1.SetToolTip(Me.Label21, resources.GetString("Label21.ToolTip"))
        '
        'GroupBox6
        '
        resources.ApplyResources(Me.GroupBox6, "GroupBox6")
        Me.GroupBox6.BackColor = System.Drawing.Color.Aquamarine
        Me.GroupBox6.Controls.Add(Me.Label20)
        Me.GroupBox6.Controls.Add(Me.Button2)
        Me.GroupBox6.Controls.Add(Me.btnMakeMapN)
        Me.GroupBox6.Controls.Add(Me.cbOnlyVideo)
        Me.GroupBox6.Controls.Add(Me.PictureBox3)
        Me.GroupBox6.Controls.Add(Me.btnRunMakeVideo)
        Me.GroupBox6.Controls.Add(Me.btnOutputFile)
        Me.GroupBox6.Controls.Add(Me.lblOutputVideoLength)
        Me.GroupBox6.Controls.Add(Me.IconStatusOutput)
        Me.GroupBox6.Controls.Add(Me.Label7)
        Me.GroupBox6.Controls.Add(Me.txtOutFilename)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox6, resources.GetString("GroupBox6.ToolTip"))
        '
        'Label20
        '
        resources.ApplyResources(Me.Label20, "Label20")
        Me.Label20.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "InfoOutVideoFormat", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.Label20.Name = "Label20"
        Me.Label20.Text = Global.OHeadcamMapApp.My.MySettings.Default.InfoOutVideoFormat
        Me.ToolTip1.SetToolTip(Me.Label20, resources.GetString("Label20.ToolTip"))
        '
        'Button2
        '
        resources.ApplyResources(Me.Button2, "Button2")
        Me.Button2.Name = "Button2"
        Me.ToolTip1.SetToolTip(Me.Button2, resources.GetString("Button2.ToolTip"))
        Me.Button2.UseVisualStyleBackColor = True
        '
        'btnMakeMapN
        '
        resources.ApplyResources(Me.btnMakeMapN, "btnMakeMapN")
        Me.btnMakeMapN.Name = "btnMakeMapN"
        Me.ToolTip1.SetToolTip(Me.btnMakeMapN, resources.GetString("btnMakeMapN.ToolTip"))
        Me.btnMakeMapN.UseVisualStyleBackColor = True
        '
        'cbOnlyVideo
        '
        resources.ApplyResources(Me.cbOnlyVideo, "cbOnlyVideo")
        Me.cbOnlyVideo.Name = "cbOnlyVideo"
        Me.ToolTip1.SetToolTip(Me.cbOnlyVideo, resources.GetString("cbOnlyVideo.ToolTip"))
        Me.cbOnlyVideo.UseVisualStyleBackColor = True
        '
        'PictureBox3
        '
        resources.ApplyResources(Me.PictureBox3, "PictureBox3")
        Me.PictureBox3.Image = Global.OHeadcamMapApp.My.Resources.Resources.img3
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox3, resources.GetString("PictureBox3.ToolTip"))
        '
        'btnRunMakeVideo
        '
        resources.ApplyResources(Me.btnRunMakeVideo, "btnRunMakeVideo")
        Me.btnRunMakeVideo.Name = "btnRunMakeVideo"
        Me.ToolTip1.SetToolTip(Me.btnRunMakeVideo, resources.GetString("btnRunMakeVideo.ToolTip"))
        Me.btnRunMakeVideo.UseVisualStyleBackColor = True
        '
        'lblOutputVideoLength
        '
        resources.ApplyResources(Me.lblOutputVideoLength, "lblOutputVideoLength")
        Me.lblOutputVideoLength.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "OutputVideoLength", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.lblOutputVideoLength.Name = "lblOutputVideoLength"
        Me.lblOutputVideoLength.Text = Global.OHeadcamMapApp.My.MySettings.Default.OutputVideoLength
        Me.ToolTip1.SetToolTip(Me.lblOutputVideoLength, resources.GetString("lblOutputVideoLength.ToolTip"))
        '
        'IconStatusOutput
        '
        resources.ApplyResources(Me.IconStatusOutput, "IconStatusOutput")
        Me.IconStatusOutput.Image = Global.OHeadcamMapApp.My.Resources.Resources.imgNotready
        Me.IconStatusOutput.Name = "IconStatusOutput"
        Me.ToolTip1.SetToolTip(Me.IconStatusOutput, resources.GetString("IconStatusOutput.ToolTip"))
        '
        'txtOutFilename
        '
        resources.ApplyResources(Me.txtOutFilename, "txtOutFilename")
        Me.txtOutFilename.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "Outputfile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtOutFilename.Name = "txtOutFilename"
        Me.txtOutFilename.Text = Global.OHeadcamMapApp.My.MySettings.Default.Outputfile
        Me.ToolTip1.SetToolTip(Me.txtOutFilename, resources.GetString("txtOutFilename.ToolTip"))
        '
        'GroupBox9
        '
        resources.ApplyResources(Me.GroupBox9, "GroupBox9")
        Me.GroupBox9.BackColor = System.Drawing.Color.LemonChiffon
        Me.GroupBox9.Controls.Add(Me.Label22)
        Me.GroupBox9.Controls.Add(Me.Label14)
        Me.GroupBox9.Controls.Add(Me.Label12)
        Me.GroupBox9.Controls.Add(Me.btnAddMusic)
        Me.GroupBox9.Controls.Add(Me.Label10)
        Me.GroupBox9.Controls.Add(Me.Label9)
        Me.GroupBox9.Controls.Add(Me.numVideoTempo)
        Me.GroupBox9.Controls.Add(Me.Label5)
        Me.GroupBox9.Controls.Add(Me.Label16)
        Me.GroupBox9.Controls.Add(Me.txtOutputLength)
        Me.GroupBox9.Controls.Add(Me.chbNoAudio)
        Me.GroupBox9.Controls.Add(Me.btnMapSetting)
        Me.GroupBox9.Controls.Add(Me.PictureBox2)
        Me.GroupBox9.Controls.Add(Me.btnMakeAdjVideo)
        Me.GroupBox9.Controls.Add(Me.txtInpCutfromStart)
        Me.GroupBox9.Controls.Add(Me.LblGPSDiff)
        Me.GroupBox9.Controls.Add(Me.Label13)
        Me.GroupBox9.Controls.Add(Me.Label8)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox9, resources.GetString("GroupBox9.ToolTip"))
        '
        'Label22
        '
        resources.ApplyResources(Me.Label22, "Label22")
        Me.Label22.Name = "Label22"
        Me.ToolTip1.SetToolTip(Me.Label22, resources.GetString("Label22.ToolTip"))
        '
        'Label14
        '
        resources.ApplyResources(Me.Label14, "Label14")
        Me.Label14.Name = "Label14"
        Me.ToolTip1.SetToolTip(Me.Label14, resources.GetString("Label14.ToolTip"))
        '
        'Label12
        '
        resources.ApplyResources(Me.Label12, "Label12")
        Me.Label12.Name = "Label12"
        Me.ToolTip1.SetToolTip(Me.Label12, resources.GetString("Label12.ToolTip"))
        '
        'Label10
        '
        resources.ApplyResources(Me.Label10, "Label10")
        Me.Label10.Name = "Label10"
        Me.ToolTip1.SetToolTip(Me.Label10, resources.GetString("Label10.ToolTip"))
        '
        'Label9
        '
        resources.ApplyResources(Me.Label9, "Label9")
        Me.Label9.Name = "Label9"
        Me.ToolTip1.SetToolTip(Me.Label9, resources.GetString("Label9.ToolTip"))
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        Me.ToolTip1.SetToolTip(Me.Label5, resources.GetString("Label5.ToolTip"))
        '
        'Label16
        '
        resources.ApplyResources(Me.Label16, "Label16")
        Me.Label16.Name = "Label16"
        Me.ToolTip1.SetToolTip(Me.Label16, resources.GetString("Label16.ToolTip"))
        '
        'PictureBox2
        '
        resources.ApplyResources(Me.PictureBox2, "PictureBox2")
        Me.PictureBox2.Image = Global.OHeadcamMapApp.My.Resources.Resources.img2
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PictureBox2, resources.GetString("PictureBox2.ToolTip"))
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Name = "Label8"
        Me.ToolTip1.SetToolTip(Me.Label8, resources.GetString("Label8.ToolTip"))
        '
        'StatusStrip1
        '
        resources.ApplyResources(Me.StatusStrip1, "StatusStrip1")
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Statusbar, Me.StatusProgressBar1, Me.StatusBarProgressText, Me.StatusRemaining, Me.ToolStripStatusLabel1})
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.ToolTip1.SetToolTip(Me.StatusStrip1, resources.GetString("StatusStrip1.ToolTip"))
        '
        'Statusbar
        '
        resources.ApplyResources(Me.Statusbar, "Statusbar")
        Me.Statusbar.Name = "Statusbar"
        '
        'StatusProgressBar1
        '
        resources.ApplyResources(Me.StatusProgressBar1, "StatusProgressBar1")
        Me.StatusProgressBar1.Name = "StatusProgressBar1"
        '
        'StatusBarProgressText
        '
        resources.ApplyResources(Me.StatusBarProgressText, "StatusBarProgressText")
        Me.StatusBarProgressText.Name = "StatusBarProgressText"
        '
        'StatusRemaining
        '
        resources.ApplyResources(Me.StatusRemaining, "StatusRemaining")
        Me.StatusRemaining.Name = "StatusRemaining"
        '
        'ToolStripStatusLabel1
        '
        resources.ApplyResources(Me.ToolStripStatusLabel1, "ToolStripStatusLabel1")
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        '
        'GroupBox10
        '
        resources.ApplyResources(Me.GroupBox10, "GroupBox10")
        Me.GroupBox10.Controls.Add(Me.chkUseQuickRouteFiles)
        Me.GroupBox10.Controls.Add(Me.btnRouteEditor)
        Me.GroupBox10.Controls.Add(Me.chkMapFlipOnOff)
        Me.GroupBox10.Controls.Add(Me.grpMapFlip)
        Me.GroupBox10.Controls.Add(Me.Label4)
        Me.GroupBox10.Controls.Add(Me.btnQRimg)
        Me.GroupBox10.Controls.Add(Me.txtXMLfile)
        Me.GroupBox10.Controls.Add(Me.btnQRxml)
        Me.GroupBox10.Controls.Add(Me.Label3)
        Me.GroupBox10.Controls.Add(Me.txtQRImage)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox10, resources.GetString("GroupBox10.ToolTip"))
        '
        'chkUseQuickRouteFiles
        '
        resources.ApplyResources(Me.chkUseQuickRouteFiles, "chkUseQuickRouteFiles")
        Me.chkUseQuickRouteFiles.Name = "chkUseQuickRouteFiles"
        Me.ToolTip1.SetToolTip(Me.chkUseQuickRouteFiles, resources.GetString("chkUseQuickRouteFiles.ToolTip"))
        Me.chkUseQuickRouteFiles.UseVisualStyleBackColor = True
        '
        'btnRouteEditor
        '
        resources.ApplyResources(Me.btnRouteEditor, "btnRouteEditor")
        Me.btnRouteEditor.Name = "btnRouteEditor"
        Me.ToolTip1.SetToolTip(Me.btnRouteEditor, resources.GetString("btnRouteEditor.ToolTip"))
        Me.btnRouteEditor.UseVisualStyleBackColor = True
        '
        'chkMapFlipOnOff
        '
        resources.ApplyResources(Me.chkMapFlipOnOff, "chkMapFlipOnOff")
        Me.chkMapFlipOnOff.Name = "chkMapFlipOnOff"
        Me.ToolTip1.SetToolTip(Me.chkMapFlipOnOff, resources.GetString("chkMapFlipOnOff.ToolTip"))
        Me.chkMapFlipOnOff.UseVisualStyleBackColor = True
        '
        'grpMapFlip
        '
        resources.ApplyResources(Me.grpMapFlip, "grpMapFlip")
        Me.grpMapFlip.BackColor = System.Drawing.SystemColors.Control
        Me.grpMapFlip.Controls.Add(Me.chkUseQuickRouteFiles2)
        Me.grpMapFlip.Controls.Add(Me.btnRouteEditor2)
        Me.grpMapFlip.Controls.Add(Me.btnMapFlipinfo)
        Me.grpMapFlip.Controls.Add(Me.btnMapFlip)
        Me.grpMapFlip.Controls.Add(Me.lblMapFlipGPXd)
        Me.grpMapFlip.Controls.Add(Me.lblMapFlipStarttime)
        Me.grpMapFlip.Controls.Add(Me.Label19)
        Me.grpMapFlip.Controls.Add(Me.Label18)
        Me.grpMapFlip.Controls.Add(Me.Label17)
        Me.grpMapFlip.Controls.Add(Me.Label11)
        Me.grpMapFlip.Controls.Add(Me.btnQRimg2)
        Me.grpMapFlip.Controls.Add(Me.txtQRImage2)
        Me.grpMapFlip.Controls.Add(Me.btnQRXML2)
        Me.grpMapFlip.Controls.Add(Me.txtXMLfile2)
        Me.grpMapFlip.Name = "grpMapFlip"
        Me.grpMapFlip.TabStop = False
        Me.ToolTip1.SetToolTip(Me.grpMapFlip, resources.GetString("grpMapFlip.ToolTip"))
        '
        'chkUseQuickRouteFiles2
        '
        resources.ApplyResources(Me.chkUseQuickRouteFiles2, "chkUseQuickRouteFiles2")
        Me.chkUseQuickRouteFiles2.Name = "chkUseQuickRouteFiles2"
        Me.ToolTip1.SetToolTip(Me.chkUseQuickRouteFiles2, resources.GetString("chkUseQuickRouteFiles2.ToolTip"))
        Me.chkUseQuickRouteFiles2.UseVisualStyleBackColor = True
        '
        'btnRouteEditor2
        '
        resources.ApplyResources(Me.btnRouteEditor2, "btnRouteEditor2")
        Me.btnRouteEditor2.Name = "btnRouteEditor2"
        Me.ToolTip1.SetToolTip(Me.btnRouteEditor2, resources.GetString("btnRouteEditor2.ToolTip"))
        Me.btnRouteEditor2.UseVisualStyleBackColor = True
        '
        'btnMapFlipinfo
        '
        resources.ApplyResources(Me.btnMapFlipinfo, "btnMapFlipinfo")
        Me.btnMapFlipinfo.BackColor = System.Drawing.SystemColors.Highlight
        Me.btnMapFlipinfo.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnMapFlipinfo.Name = "btnMapFlipinfo"
        Me.ToolTip1.SetToolTip(Me.btnMapFlipinfo, resources.GetString("btnMapFlipinfo.ToolTip"))
        Me.btnMapFlipinfo.UseVisualStyleBackColor = False
        '
        'lblMapFlipGPXd
        '
        resources.ApplyResources(Me.lblMapFlipGPXd, "lblMapFlipGPXd")
        Me.lblMapFlipGPXd.Name = "lblMapFlipGPXd"
        Me.ToolTip1.SetToolTip(Me.lblMapFlipGPXd, resources.GetString("lblMapFlipGPXd.ToolTip"))
        '
        'lblMapFlipStarttime
        '
        resources.ApplyResources(Me.lblMapFlipStarttime, "lblMapFlipStarttime")
        Me.lblMapFlipStarttime.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "MapFlipStartT", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.lblMapFlipStarttime.Name = "lblMapFlipStarttime"
        Me.lblMapFlipStarttime.Text = Global.OHeadcamMapApp.My.MySettings.Default.MapFlipStartT
        Me.ToolTip1.SetToolTip(Me.lblMapFlipStarttime, resources.GetString("lblMapFlipStarttime.ToolTip"))
        '
        'Label19
        '
        resources.ApplyResources(Me.Label19, "Label19")
        Me.Label19.Name = "Label19"
        Me.ToolTip1.SetToolTip(Me.Label19, resources.GetString("Label19.ToolTip"))
        '
        'Label18
        '
        resources.ApplyResources(Me.Label18, "Label18")
        Me.Label18.Name = "Label18"
        Me.ToolTip1.SetToolTip(Me.Label18, resources.GetString("Label18.ToolTip"))
        '
        'Label17
        '
        resources.ApplyResources(Me.Label17, "Label17")
        Me.Label17.Name = "Label17"
        Me.ToolTip1.SetToolTip(Me.Label17, resources.GetString("Label17.ToolTip"))
        '
        'Label11
        '
        resources.ApplyResources(Me.Label11, "Label11")
        Me.Label11.Name = "Label11"
        Me.ToolTip1.SetToolTip(Me.Label11, resources.GetString("Label11.ToolTip"))
        '
        'btnQRimg2
        '
        resources.ApplyResources(Me.btnQRimg2, "btnQRimg2")
        Me.btnQRimg2.Name = "btnQRimg2"
        Me.ToolTip1.SetToolTip(Me.btnQRimg2, resources.GetString("btnQRimg2.ToolTip"))
        Me.btnQRimg2.UseVisualStyleBackColor = True
        '
        'btnQRXML2
        '
        resources.ApplyResources(Me.btnQRXML2, "btnQRXML2")
        Me.btnQRXML2.Name = "btnQRXML2"
        Me.ToolTip1.SetToolTip(Me.btnQRXML2, resources.GetString("btnQRXML2.ToolTip"))
        Me.btnQRXML2.UseVisualStyleBackColor = True
        '
        'lblTrackVideoInUse
        '
        resources.ApplyResources(Me.lblTrackVideoInUse, "lblTrackVideoInUse")
        Me.lblTrackVideoInUse.ForeColor = System.Drawing.Color.Red
        Me.lblTrackVideoInUse.Name = "lblTrackVideoInUse"
        Me.ToolTip1.SetToolTip(Me.lblTrackVideoInUse, resources.GetString("lblTrackVideoInUse.ToolTip"))
        '
        'btnTestWriteVideo
        '
        resources.ApplyResources(Me.btnTestWriteVideo, "btnTestWriteVideo")
        Me.btnTestWriteVideo.Name = "btnTestWriteVideo"
        Me.ToolTip1.SetToolTip(Me.btnTestWriteVideo, resources.GetString("btnTestWriteVideo.ToolTip"))
        Me.btnTestWriteVideo.UseVisualStyleBackColor = True
        '
        'TimerRGCheck
        '
        Me.TimerRGCheck.Interval = 5000
        '
        'Timer2
        '
        Me.Timer2.Interval = 500
        '
        'TimerStatusRemaining
        '
        Me.TimerStatusRemaining.Interval = 2000
        '
        'TimerMapStatus
        '
        Me.TimerMapStatus.Interval = 500
        '
        'MainForm
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnTestWriteVideo)
        Me.Controls.Add(Me.btnReset)
        Me.Controls.Add(Me.lblTrackVideoInUse)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox10)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.btnClearVideolist)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.AddVideofile)
        Me.Controls.Add(Me.txtVideolist)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "MainForm"
        Me.ToolTip1.SetToolTip(Me, resources.GetString("$this.ToolTip"))
        Me.ToolTip2.SetToolTip(Me, resources.GetString("$this.ToolTip1"))
        CType(Me.numVideoTempo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox9.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.GroupBox10.ResumeLayout(False)
        Me.GroupBox10.PerformLayout()
        Me.grpMapFlip.ResumeLayout(False)
        Me.grpMapFlip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OpenFileDialogVideo As OpenFileDialog
    Friend WithEvents AddVideofile As Button
    Friend WithEvents txtVideolist As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtXMLfile As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtQRImage As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents btnQRxml As Button
    Friend WithEvents btnQRimg As Button
    Friend WithEvents OpenFileDialoggpx As OpenFileDialog
    Friend WithEvents OpenFileDialogqrimg As OpenFileDialog
    Friend WithEvents OpenFileDialogxml As OpenFileDialog
    Friend WithEvents btnOutputFile As Button
    Friend WithEvents Label7 As Label
    Friend WithEvents txtOutFilename As TextBox
    Friend WithEvents SaveFileDialogOutput As SaveFileDialog
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents ToolTip2 As ToolTip
    Friend WithEvents btnClearVideolist As Button
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents OmToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents IndstillingerToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents LanguageToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DanskToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EnglishToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ChineseToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OmToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents HjælpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ViKørselslogToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents txtPrepareLength As TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox7 As GroupBox
    Friend WithEvents rb_deshake As RadioButton
    Friend WithEvents RB_onlyfilter As RadioButton
    Friend WithEvents cbXJoin As CheckBox
    Friend WithEvents cbXDeshake As CheckBox
    Friend WithEvents Label21 As Label
    Friend WithEvents GroupBox6 As GroupBox
    Friend WithEvents GroupBox9 As GroupBox
    Friend WithEvents LblGPSDiff As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents txtInpCutfromStart As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents chbNoAudio As CheckBox
    Friend WithEvents Label16 As Label
    Friend WithEvents numVideoTempo As NumericUpDown
    Friend WithEvents btnMakeAdjVideo As Button
    Friend WithEvents btnAddMusic As Button
    Friend WithEvents TimerRGCheck As Timer
    Friend WithEvents iconStatusVideoIn As Label
    Friend WithEvents lblVideoInLength As Label
    Friend WithEvents IconStatusOutput As Label
    Friend WithEvents lblOutputVideoLength As Label
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents Statusbar As ToolStripStatusLabel
    Friend WithEvents StatusProgressBar1 As ToolStripProgressBar
    Friend WithEvents Timer2 As Timer
    Friend WithEvents GroupBox10 As GroupBox
    Friend WithEvents btnMapSetting As Button
    Friend WithEvents btnPrepareInput As Button
    Friend WithEvents btnRunMakeVideo As Button
    Friend WithEvents Label5 As Label
    Friend WithEvents txtOutputLength As TextBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents PictureBox3 As PictureBox
    Friend WithEvents Label6 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label22 As Label
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents SideOmSideVideoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents VideoafspillerToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BrugTrackingvideoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents lblTrackVideoInUse As Label
    Friend WithEvents IconStatusPrepare As Label
    Friend WithEvents btnReset As Button
    Friend WithEvents StatusBarProgressText As ToolStripStatusLabel
    Friend WithEvents TimerStatusRemaining As Timer
    Friend WithEvents StatusRemaining As ToolStripStatusLabel
    Friend WithEvents FFPlayTestafspillerToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents cbOnlyVideo As CheckBox
    Friend WithEvents Button1 As Button
    Friend WithEvents TimerMapStatus As Timer
    Friend WithEvents btnMakeMapN As Button
    Friend WithEvents btnPostProcess As Button
    Friend WithEvents ChangeFPSScriptToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FixApplegpxFilToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Label11 As Label
    Friend WithEvents grpMapFlip As GroupBox
    Friend WithEvents btnQRimg2 As Button
    Friend WithEvents txtQRImage2 As TextBox
    Friend WithEvents txtXMLfile2 As TextBox
    Friend WithEvents Label17 As Label
    Friend WithEvents btnQRXML2 As Button
    Friend WithEvents lblMapFlipGPXd As Label
    Friend WithEvents lblMapFlipStarttime As Label
    Friend WithEvents Label19 As Label
    Friend WithEvents Label18 As Label
    Friend WithEvents btnMapFlip As Button
    Friend WithEvents btnMapFlipinfo As Button
    Friend WithEvents chkMapFlipOnOff As CheckBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Label20 As Label
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents btnTestWriteVideo As Button
    Friend WithEvents lblMapLength As Label
    Friend WithEvents chkUseQuickRouteFiles As CheckBox
    Friend WithEvents btnRouteEditor As Button
    Friend WithEvents btnRouteEditor2 As Button
    Friend WithEvents chkUseQuickRouteFiles2 As CheckBox
End Class
