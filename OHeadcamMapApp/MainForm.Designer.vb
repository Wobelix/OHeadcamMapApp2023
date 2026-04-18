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
        Dim MySettings1 As OHeadcamMapApp.My.MySettings = New OHeadcamMapApp.My.MySettings()
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
        Me.chkMapFlipOnOff = New System.Windows.Forms.CheckBox()
        Me.grpMapFlip = New System.Windows.Forms.GroupBox()
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
        Me.AddVideofile.UseVisualStyleBackColor = True
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
        'btnQRxml
        '
        resources.ApplyResources(Me.btnQRxml, "btnQRxml")
        Me.btnQRxml.Name = "btnQRxml"
        Me.btnQRxml.UseVisualStyleBackColor = True
        '
        'btnQRimg
        '
        resources.ApplyResources(Me.btnQRimg, "btnQRimg")
        Me.btnQRimg.Name = "btnQRimg"
        Me.btnQRimg.UseVisualStyleBackColor = True
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
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
        Me.numVideoTempo.DecimalPlaces = 1
        resources.ApplyResources(Me.numVideoTempo, "numVideoTempo")
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
        Me.btnMakeAdjVideo.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.btnMakeAdjVideo, "btnMakeAdjVideo")
        Me.btnMakeAdjVideo.Name = "btnMakeAdjVideo"
        Me.ToolTip1.SetToolTip(Me.btnMakeAdjVideo, resources.GetString("btnMakeAdjVideo.ToolTip"))
        Me.btnMakeAdjVideo.UseVisualStyleBackColor = True
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.Button1)
        Me.GroupBox7.Controls.Add(Me.rb_deshake)
        Me.GroupBox7.Controls.Add(Me.RB_onlyfilter)
        Me.GroupBox7.Controls.Add(Me.cbXDeshake)
        resources.ApplyResources(Me.GroupBox7, "GroupBox7")
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
        Me.rb_deshake.UseVisualStyleBackColor = True
        '
        'RB_onlyfilter
        '
        resources.ApplyResources(Me.RB_onlyfilter, "RB_onlyfilter")
        Me.RB_onlyfilter.Checked = True
        Me.RB_onlyfilter.Name = "RB_onlyfilter"
        Me.RB_onlyfilter.TabStop = True
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
        Me.txtPrepareLength.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "PrepareLength", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.txtPrepareLength, "txtPrepareLength")
        Me.txtPrepareLength.Name = "txtPrepareLength"
        Me.txtPrepareLength.Text = Global.OHeadcamMapApp.My.MySettings.Default.PrepareLength
        Me.ToolTip2.SetToolTip(Me.txtPrepareLength, resources.GetString("txtPrepareLength.ToolTip"))
        Me.ToolTip1.SetToolTip(Me.txtPrepareLength, resources.GetString("txtPrepareLength.ToolTip1"))
        '
        'txtOutputLength
        '
        Me.txtOutputLength.BackColor = System.Drawing.SystemColors.Window
        Me.txtOutputLength.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "OutputLength", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.txtOutputLength, "txtOutputLength")
        Me.txtOutputLength.Name = "txtOutputLength"
        Me.txtOutputLength.Text = Global.OHeadcamMapApp.My.MySettings.Default.OutputLength
        Me.ToolTip2.SetToolTip(Me.txtOutputLength, resources.GetString("txtOutputLength.ToolTip"))
        Me.ToolTip1.SetToolTip(Me.txtOutputLength, resources.GetString("txtOutputLength.ToolTip1"))
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
        MySettings1.AdjTestVideoReady = False
        MySettings1.AdjVideoGPSDiff = "0"
        MySettings1.AdjVideoLength = "300"
        MySettings1.AppVersion = ""
        MySettings1.Audiolevel = ""
        MySettings1.AudioVideofile = ""
        MySettings1.bAdjVideoIsReady = False
        MySettings1.bDoMusic = False
        MySettings1.bOutputfileReady = False
        MySettings1.bUseMapTrackingVideo = False
        MySettings1.cbHeightGraph = True
        MySettings1.cbLoopMusic = True
        MySettings1.cbSetImgM = False
        MySettings1.cbShowLegMAp = True
        MySettings1.cbShowRoute = True
        MySettings1.cbShowSpeedPanel = True
        MySettings1.cbXMakeframes = False
        MySettings1.chkHDFormat = False
        MySettings1.ckShowLegMap = True
        MySettings1.ckShowRouteMap = True
        MySettings1.dFfmpegScaleMap = New Decimal(New Integer() {1, 0, 0, 0})
        MySettings1.ffmpegBufsize = "40M"
        MySettings1.ffmpegCRF = "26"
        MySettings1.ffmpegJoin = "-f concat -safe 0 -i joinlist.txt"
        MySettings1.ffmpegLensCorrection = "lenscorrection=cx=0.5:cy=0.5:k1=-0.200:k2=0.000"
        MySettings1.ffmpegMaxBitr = "20M"
        MySettings1.ffmpegOutFps = "25"
        MySettings1.ffmpegPreset = "fast"
        MySettings1.ffmpegScaleMap = "1.5"
        MySettings1.ffmpegVidstabDetect = "vidstabdetect=shakiness=10:accuracy=15:mincontrast=0.200:stepsize=6:show=0:result" &
    "=data.trf:tripod=0:result=data.trf"
        MySettings1.ffmpegVidstabDetectOut = " -f null -"
        MySettings1.ffmpegVidstabTransform = "vidstabtransform=smoothing=25:crop=black:zoom=0:optzoom=0:interpol='bicubic':inpu" &
    "t=data.trf:tripod=0,unsharp=7:7:3:7:7:3"
        MySettings1.GPXDiff = "0"
        MySettings1.GPXDiff1 = "0"
        MySettings1.GPXDiff2 = "0"
        MySettings1.GPXfile = ""
        MySettings1.InfoOutVideoFormat = "Format: 1920x1080"
        MySettings1.JoinFileHeight = 1080
        MySettings1.JoinFileLength = 0!
        MySettings1.JoinFileWidth = 1920
        MySettings1.Language = ""
        MySettings1.lblMapLength = "0:00:00"
        MySettings1.MapFlipActive = False
        MySettings1.MapFlipOnOff = False
        MySettings1.MapFlipStartS = "0"
        MySettings1.MapFlipStartT = "0:00:00"
        MySettings1.MapLength = "0:00:00"
        MySettings1.MHeightH = "10"
        MySettings1.MHeightV = "10"
        MySettings1.MIArrowBarb = 0.5R
        MySettings1.MIArrowWidth = 0.7R
        MySettings1.MIDotColor = System.Drawing.Color.Red
        MySettings1.MIDotSize = 15
        MySettings1.MIDotType = "Dot"
        MySettings1.MIFLegDim = New System.Drawing.Point(0, 0)
        MySettings1.MIFLegPos = New System.Drawing.Point(0, 0)
        MySettings1.MIFrameColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        MySettings1.MIFrameFeather = False
        MySettings1.MIFrameWidth = 4
        MySettings1.MIFZoomDim = New System.Drawing.Point(0, 0)
        MySettings1.MIFZoomPos = New System.Drawing.Point(0, 0)
        MySettings1.MILegHeight = 600
        MySettings1.MILegMapPos = New System.Drawing.Point(80, 450)
        MySettings1.MILegMargin = 50
        MySettings1.MILegRad = 40
        MySettings1.MILegRenderLayout = "classic"
        MySettings1.MILegWidth = 350
        MySettings1.MISmoothFrameStepSeconds = 0.25R
        MySettings1.MISmoothParallelGeneration = True
        MySettings1.MITailColor = System.Drawing.Color.Red
        MySettings1.MITailDuration = 30
        MySettings1.MITailRatio = New Decimal(New Integer() {9, 0, 0, 65536})
        MySettings1.MIZoomCircle = True
        MySettings1.MIZoomHeight = 350
        MySettings1.MIZoomMapPos = New System.Drawing.Point(80, 100)
        MySettings1.MIZoomRad = 80
        MySettings1.MIZoomWidth = 350
        MySettings1.MIZoomZoom = 1.0R
        MySettings1.MLegMapH = "10"
        MySettings1.MlegMapH2 = "10"
        MySettings1.MLegMapV = "10"
        MySettings1.MRouteH = "10"
        MySettings1.MRouteV = "10"
        MySettings1.MSpeedH = "10"
        MySettings1.MSpeedV = "10"
        MySettings1.MusicAudiofile = ""
        MySettings1.MusicFFMpegParam = ""
        MySettings1.Musicfile = ""
        MySettings1.Musiclevel = ""
        MySettings1.MusicOutputfile = ""
        MySettings1.No_deshake_filter = "normalize=blackpt=black:whitept=white:smoothing=10:strength=0.5"
        MySettings1.NoAudio = False
        MySettings1.Outputfile = ""
        MySettings1.OutputLength = ""
        MySettings1.OutputVideoLength = "0:00:00"
        MySettings1.OverlayEstimate2KSeconds = 0R
        MySettings1.OverlayEstimate4KSeconds = 0R
        MySettings1.OverlayEstimateHdSeconds = 0R
        MySettings1.PostVideo = ""
        MySettings1.PostVideoList = ""
        MySettings1.PrepareLength = ""
        MySettings1.QRimage = ""
        MySettings1.QRimage1 = ""
        MySettings1.QRImage2 = ""
        MySettings1.QRLength = "0"
        MySettings1.QRXML = ""
        MySettings1.QRXML1 = ""
        MySettings1.QRXML2 = ""
        MySettings1.QRXMLTimesec = 0
        MySettings1.QRXMLTimesec2 = 0
        MySettings1.rbSetImgL = False
        MySettings1.rbSetImgR = False
        MySettings1.SbSLength = ""
        MySettings1.SbSLfile = ""
        MySettings1.SbSOutfile = "Out.mp4"
        MySettings1.SbSRfile = ""
        MySettings1.SettingSbSCode = "-filter_complex ""[0:v]scale=-1:1080[v0];[1:v]scale=-1:1080[v1];[v0][v1]hstack"""
        MySettings1.SettingsKey = ""
        MySettings1.StatusInputVideo = ""
        MySettings1.StatusMakeMap = ""
        MySettings1.StatusOutputVideo = ""
        MySettings1.StatusPrepare = ""
        MySettings1.trackAudiolevel = 0
        MySettings1.TrackMapHeight = 0
        MySettings1.TrackMapImageFile = ""
        MySettings1.TrackMapVideoFilename = ""
        MySettings1.TrackMapVideoLength = "0:00:00"
        MySettings1.TrackMapWidth = 0
        MySettings1.trackMusiclevel = 0
        MySettings1.Transparency = New Decimal(New Integer() {1, 0, 0, 0})
        MySettings1.txtGPSPos = ""
        MySettings1.txtInpCutFromStart = ""
        MySettings1.txtMapVideoFilename = ""
        MySettings1.txtMusicOutputfile = ""
        MySettings1.txtRealtimeFactor = ""
        MySettings1.txtVideoPos = ""
        MySettings1.VidAdj_Rot = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings1.VidAdj_Zoom = New Decimal(New Integer() {100, 0, 0, 0})
        MySettings1.VidAdjBright = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings1.VidAdjColImpr = False
        MySettings1.VidAdjContr = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings1.VidAdjGamma = New Decimal(New Integer() {1, 0, 0, 0})
        MySettings1.VidAdjHue = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings1.VidAdjLensDist = False
        MySettings1.VidAdjNoise = False
        MySettings1.VidAdjSatur = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings1.VidAdjSharp = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings1.Videofiles = ""
        MySettings1.videofiles2 = ""
        MySettings1.Videofilesfull = ""
        MySettings1.VideoInLength = "0:00:00"
        MySettings1.VideoPadding = "C"
        MySettings1.VideoWorkFolder = ""
        Me.txtInpCutfromStart.DataBindings.Add(New System.Windows.Forms.Binding("Text", MySettings1, "txtInpCutFromStart", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.txtInpCutfromStart, "txtInpCutfromStart")
        Me.txtInpCutfromStart.Name = "txtInpCutfromStart"
        Me.txtInpCutfromStart.Text = MySettings1.txtInpCutFromStart
        Me.ToolTip2.SetToolTip(Me.txtInpCutfromStart, resources.GetString("txtInpCutfromStart.ToolTip"))
        Me.ToolTip1.SetToolTip(Me.txtInpCutfromStart, resources.GetString("txtInpCutfromStart.ToolTip1"))
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
        Me.txtQRImage2.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "QRImage2", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.txtQRImage2, "txtQRImage2")
        Me.txtQRImage2.Name = "txtQRImage2"
        Me.txtQRImage2.Text = Global.OHeadcamMapApp.My.MySettings.Default.QRImage2
        Me.ToolTip2.SetToolTip(Me.txtQRImage2, resources.GetString("txtQRImage2.ToolTip"))
        Me.ToolTip1.SetToolTip(Me.txtQRImage2, resources.GetString("txtQRImage2.ToolTip1"))
        '
        'txtXMLfile2
        '
        Me.txtXMLfile2.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "QRXML2", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.txtXMLfile2, "txtXMLfile2")
        Me.txtXMLfile2.Name = "txtXMLfile2"
        Me.txtXMLfile2.Text = Global.OHeadcamMapApp.My.MySettings.Default.QRXML2
        Me.ToolTip2.SetToolTip(Me.txtXMLfile2, resources.GetString("txtXMLfile2.ToolTip"))
        Me.ToolTip1.SetToolTip(Me.txtXMLfile2, resources.GetString("txtXMLfile2.ToolTip1"))
        '
        'txtXMLfile
        '
        Me.txtXMLfile.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "QRXML1", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.txtXMLfile, "txtXMLfile")
        Me.txtXMLfile.Name = "txtXMLfile"
        Me.txtXMLfile.Text = Global.OHeadcamMapApp.My.MySettings.Default.QRXML1
        Me.ToolTip2.SetToolTip(Me.txtXMLfile, resources.GetString("txtXMLfile.ToolTip"))
        Me.ToolTip1.SetToolTip(Me.txtXMLfile, resources.GetString("txtXMLfile.ToolTip1"))
        '
        'txtQRImage
        '
        Me.txtQRImage.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "QRimage1", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.txtQRImage, "txtQRImage")
        Me.txtQRImage.Name = "txtQRImage"
        Me.txtQRImage.Text = Global.OHeadcamMapApp.My.MySettings.Default.QRimage1
        Me.ToolTip2.SetToolTip(Me.txtQRImage, resources.GetString("txtQRImage.ToolTip"))
        Me.ToolTip1.SetToolTip(Me.txtQRImage, resources.GetString("txtQRImage.ToolTip1"))
        '
        'txtVideolist
        '
        Me.txtVideolist.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "videofiles2", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.txtVideolist, "txtVideolist")
        Me.txtVideolist.Name = "txtVideolist"
        Me.txtVideolist.Text = Global.OHeadcamMapApp.My.MySettings.Default.videofiles2
        Me.ToolTip2.SetToolTip(Me.txtVideolist, resources.GetString("txtVideolist.ToolTip"))
        Me.ToolTip1.SetToolTip(Me.txtVideolist, resources.GetString("txtVideolist.ToolTip1"))
        '
        'btnReset
        '
        resources.ApplyResources(Me.btnReset, "btnReset")
        Me.btnReset.Name = "btnReset"
        Me.btnReset.UseVisualStyleBackColor = True
        '
        'btnClearVideolist
        '
        resources.ApplyResources(Me.btnClearVideolist, "btnClearVideolist")
        Me.btnClearVideolist.Name = "btnClearVideolist"
        Me.btnClearVideolist.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.IndstillingerToolStripMenuItem1, Me.LanguageToolStripMenuItem, Me.ViKørselslogToolStripMenuItem, Me.ToolStripMenuItem1, Me.OmToolStripMenuItem})
        resources.ApplyResources(Me.MenuStrip1, "MenuStrip1")
        Me.MenuStrip1.Name = "MenuStrip1"
        '
        'IndstillingerToolStripMenuItem1
        '
        Me.IndstillingerToolStripMenuItem1.Name = "IndstillingerToolStripMenuItem1"
        resources.ApplyResources(Me.IndstillingerToolStripMenuItem1, "IndstillingerToolStripMenuItem1")
        '
        'LanguageToolStripMenuItem
        '
        Me.LanguageToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DanskToolStripMenuItem, Me.EnglishToolStripMenuItem, Me.ChineseToolStripMenuItem})
        Me.LanguageToolStripMenuItem.Name = "LanguageToolStripMenuItem"
        resources.ApplyResources(Me.LanguageToolStripMenuItem, "LanguageToolStripMenuItem")
        '
        'DanskToolStripMenuItem
        '
        Me.DanskToolStripMenuItem.Name = "DanskToolStripMenuItem"
        resources.ApplyResources(Me.DanskToolStripMenuItem, "DanskToolStripMenuItem")
        '
        'EnglishToolStripMenuItem
        '
        Me.EnglishToolStripMenuItem.Name = "EnglishToolStripMenuItem"
        resources.ApplyResources(Me.EnglishToolStripMenuItem, "EnglishToolStripMenuItem")
        '
        'ChineseToolStripMenuItem
        '
        Me.ChineseToolStripMenuItem.Name = "ChineseToolStripMenuItem"
        resources.ApplyResources(Me.ChineseToolStripMenuItem, "ChineseToolStripMenuItem")
        '
        'ViKørselslogToolStripMenuItem
        '
        Me.ViKørselslogToolStripMenuItem.Name = "ViKørselslogToolStripMenuItem"
        resources.ApplyResources(Me.ViKørselslogToolStripMenuItem, "ViKørselslogToolStripMenuItem")
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SideOmSideVideoToolStripMenuItem, Me.VideoafspillerToolStripMenuItem, Me.BrugTrackingvideoToolStripMenuItem, Me.FFPlayTestafspillerToolStripMenuItem, Me.ChangeFPSScriptToolStripMenuItem, Me.FixApplegpxFilToolStripMenuItem})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        resources.ApplyResources(Me.ToolStripMenuItem1, "ToolStripMenuItem1")
        '
        'SideOmSideVideoToolStripMenuItem
        '
        Me.SideOmSideVideoToolStripMenuItem.Name = "SideOmSideVideoToolStripMenuItem"
        resources.ApplyResources(Me.SideOmSideVideoToolStripMenuItem, "SideOmSideVideoToolStripMenuItem")
        '
        'VideoafspillerToolStripMenuItem
        '
        Me.VideoafspillerToolStripMenuItem.Name = "VideoafspillerToolStripMenuItem"
        resources.ApplyResources(Me.VideoafspillerToolStripMenuItem, "VideoafspillerToolStripMenuItem")
        '
        'BrugTrackingvideoToolStripMenuItem
        '
        Me.BrugTrackingvideoToolStripMenuItem.Name = "BrugTrackingvideoToolStripMenuItem"
        resources.ApplyResources(Me.BrugTrackingvideoToolStripMenuItem, "BrugTrackingvideoToolStripMenuItem")
        '
        'FFPlayTestafspillerToolStripMenuItem
        '
        Me.FFPlayTestafspillerToolStripMenuItem.Name = "FFPlayTestafspillerToolStripMenuItem"
        resources.ApplyResources(Me.FFPlayTestafspillerToolStripMenuItem, "FFPlayTestafspillerToolStripMenuItem")
        '
        'ChangeFPSScriptToolStripMenuItem
        '
        Me.ChangeFPSScriptToolStripMenuItem.Name = "ChangeFPSScriptToolStripMenuItem"
        resources.ApplyResources(Me.ChangeFPSScriptToolStripMenuItem, "ChangeFPSScriptToolStripMenuItem")
        '
        'FixApplegpxFilToolStripMenuItem
        '
        Me.FixApplegpxFilToolStripMenuItem.Name = "FixApplegpxFilToolStripMenuItem"
        resources.ApplyResources(Me.FixApplegpxFilToolStripMenuItem, "FixApplegpxFilToolStripMenuItem")
        '
        'OmToolStripMenuItem
        '
        Me.OmToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OmToolStripMenuItem1, Me.HjælpToolStripMenuItem})
        Me.OmToolStripMenuItem.Name = "OmToolStripMenuItem"
        resources.ApplyResources(Me.OmToolStripMenuItem, "OmToolStripMenuItem")
        '
        'OmToolStripMenuItem1
        '
        Me.OmToolStripMenuItem1.Name = "OmToolStripMenuItem1"
        resources.ApplyResources(Me.OmToolStripMenuItem1, "OmToolStripMenuItem1")
        '
        'HjælpToolStripMenuItem
        '
        Me.HjælpToolStripMenuItem.Name = "HjælpToolStripMenuItem"
        resources.ApplyResources(Me.HjælpToolStripMenuItem, "HjælpToolStripMenuItem")
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.btnPostProcess)
        Me.GroupBox4.Controls.Add(Me.GroupBox1)
        Me.GroupBox4.Controls.Add(Me.GroupBox6)
        Me.GroupBox4.Controls.Add(Me.GroupBox9)
        resources.ApplyResources(Me.GroupBox4, "GroupBox4")
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.TabStop = False
        '
        'btnPostProcess
        '
        Me.btnPostProcess.BackColor = System.Drawing.SystemColors.Highlight
        resources.ApplyResources(Me.btnPostProcess, "btnPostProcess")
        Me.btnPostProcess.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnPostProcess.Name = "btnPostProcess"
        Me.btnPostProcess.UseVisualStyleBackColor = False
        '
        'GroupBox1
        '
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
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'lblMapLength
        '
        resources.ApplyResources(Me.lblMapLength, "lblMapLength")
        Me.lblMapLength.Name = "lblMapLength"
        '
        'IconStatusPrepare
        '
        Me.IconStatusPrepare.Image = Global.OHeadcamMapApp.My.Resources.Resources.imgNotready
        resources.ApplyResources(Me.IconStatusPrepare, "IconStatusPrepare")
        Me.IconStatusPrepare.Name = "IconStatusPrepare"
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.OHeadcamMapApp.My.Resources.Resources.img1
        resources.ApplyResources(Me.PictureBox1, "PictureBox1")
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.TabStop = False
        '
        'lblVideoInLength
        '
        resources.ApplyResources(Me.lblVideoInLength, "lblVideoInLength")
        Me.lblVideoInLength.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "VideoInLength", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.lblVideoInLength.Name = "lblVideoInLength"
        Me.lblVideoInLength.Text = Global.OHeadcamMapApp.My.MySettings.Default.VideoInLength
        '
        'iconStatusVideoIn
        '
        Me.iconStatusVideoIn.Image = Global.OHeadcamMapApp.My.Resources.Resources.imgNotready
        resources.ApplyResources(Me.iconStatusVideoIn, "iconStatusVideoIn")
        Me.iconStatusVideoIn.Name = "iconStatusVideoIn"
        '
        'Label21
        '
        resources.ApplyResources(Me.Label21, "Label21")
        Me.Label21.Name = "Label21"
        '
        'GroupBox6
        '
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
        resources.ApplyResources(Me.GroupBox6, "GroupBox6")
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.TabStop = False
        '
        'Label20
        '
        resources.ApplyResources(Me.Label20, "Label20")
        Me.Label20.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "InfoOutVideoFormat", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.Label20.Name = "Label20"
        Me.Label20.Text = Global.OHeadcamMapApp.My.MySettings.Default.InfoOutVideoFormat
        '
        'Button2
        '
        resources.ApplyResources(Me.Button2, "Button2")
        Me.Button2.Name = "Button2"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'btnMakeMapN
        '
        resources.ApplyResources(Me.btnMakeMapN, "btnMakeMapN")
        Me.btnMakeMapN.Name = "btnMakeMapN"
        Me.btnMakeMapN.UseVisualStyleBackColor = True
        '
        'cbOnlyVideo
        '
        resources.ApplyResources(Me.cbOnlyVideo, "cbOnlyVideo")
        Me.cbOnlyVideo.Name = "cbOnlyVideo"
        Me.cbOnlyVideo.UseVisualStyleBackColor = True
        '
        'PictureBox3
        '
        Me.PictureBox3.Image = Global.OHeadcamMapApp.My.Resources.Resources.img3
        resources.ApplyResources(Me.PictureBox3, "PictureBox3")
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.TabStop = False
        '
        'btnRunMakeVideo
        '
        resources.ApplyResources(Me.btnRunMakeVideo, "btnRunMakeVideo")
        Me.btnRunMakeVideo.Name = "btnRunMakeVideo"
        Me.btnRunMakeVideo.UseVisualStyleBackColor = True
        '
        'lblOutputVideoLength
        '
        resources.ApplyResources(Me.lblOutputVideoLength, "lblOutputVideoLength")
        Me.lblOutputVideoLength.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "OutputVideoLength", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.lblOutputVideoLength.Name = "lblOutputVideoLength"
        Me.lblOutputVideoLength.Text = Global.OHeadcamMapApp.My.MySettings.Default.OutputVideoLength
        '
        'IconStatusOutput
        '
        Me.IconStatusOutput.Image = Global.OHeadcamMapApp.My.Resources.Resources.imgNotready
        resources.ApplyResources(Me.IconStatusOutput, "IconStatusOutput")
        Me.IconStatusOutput.Name = "IconStatusOutput"
        '
        'txtOutFilename
        '
        Me.txtOutFilename.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "Outputfile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        resources.ApplyResources(Me.txtOutFilename, "txtOutFilename")
        Me.txtOutFilename.Name = "txtOutFilename"
        Me.txtOutFilename.Text = Global.OHeadcamMapApp.My.MySettings.Default.Outputfile
        '
        'GroupBox9
        '
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
        resources.ApplyResources(Me.GroupBox9, "GroupBox9")
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.TabStop = False
        '
        'Label22
        '
        resources.ApplyResources(Me.Label22, "Label22")
        Me.Label22.Name = "Label22"
        '
        'Label14
        '
        resources.ApplyResources(Me.Label14, "Label14")
        Me.Label14.Name = "Label14"
        '
        'Label12
        '
        resources.ApplyResources(Me.Label12, "Label12")
        Me.Label12.Name = "Label12"
        '
        'Label10
        '
        resources.ApplyResources(Me.Label10, "Label10")
        Me.Label10.Name = "Label10"
        '
        'Label9
        '
        resources.ApplyResources(Me.Label9, "Label9")
        Me.Label9.Name = "Label9"
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'Label16
        '
        resources.ApplyResources(Me.Label16, "Label16")
        Me.Label16.Name = "Label16"
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = Global.OHeadcamMapApp.My.Resources.Resources.img2
        resources.ApplyResources(Me.PictureBox2, "PictureBox2")
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.TabStop = False
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Name = "Label8"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Statusbar, Me.StatusProgressBar1, Me.StatusBarProgressText, Me.StatusRemaining, Me.ToolStripStatusLabel1})
        resources.ApplyResources(Me.StatusStrip1, "StatusStrip1")
        Me.StatusStrip1.Name = "StatusStrip1"
        '
        'Statusbar
        '
        Me.Statusbar.Name = "Statusbar"
        resources.ApplyResources(Me.Statusbar, "Statusbar")
        '
        'StatusProgressBar1
        '
        Me.StatusProgressBar1.Name = "StatusProgressBar1"
        resources.ApplyResources(Me.StatusProgressBar1, "StatusProgressBar1")
        '
        'StatusBarProgressText
        '
        Me.StatusBarProgressText.Name = "StatusBarProgressText"
        resources.ApplyResources(Me.StatusBarProgressText, "StatusBarProgressText")
        '
        'StatusRemaining
        '
        Me.StatusRemaining.Name = "StatusRemaining"
        resources.ApplyResources(Me.StatusRemaining, "StatusRemaining")
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        resources.ApplyResources(Me.ToolStripStatusLabel1, "ToolStripStatusLabel1")
        '
        'GroupBox10
        '
        Me.GroupBox10.Controls.Add(Me.chkMapFlipOnOff)
        Me.GroupBox10.Controls.Add(Me.grpMapFlip)
        Me.GroupBox10.Controls.Add(Me.Label4)
        Me.GroupBox10.Controls.Add(Me.btnQRimg)
        Me.GroupBox10.Controls.Add(Me.txtXMLfile)
        Me.GroupBox10.Controls.Add(Me.btnQRxml)
        Me.GroupBox10.Controls.Add(Me.Label3)
        Me.GroupBox10.Controls.Add(Me.txtQRImage)
        resources.ApplyResources(Me.GroupBox10, "GroupBox10")
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.TabStop = False
        '
        'chkMapFlipOnOff
        '
        resources.ApplyResources(Me.chkMapFlipOnOff, "chkMapFlipOnOff")
        Me.chkMapFlipOnOff.Name = "chkMapFlipOnOff"
        Me.chkMapFlipOnOff.UseVisualStyleBackColor = True
        '
        'grpMapFlip
        '
        Me.grpMapFlip.BackColor = System.Drawing.SystemColors.Control
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
        resources.ApplyResources(Me.grpMapFlip, "grpMapFlip")
        Me.grpMapFlip.Name = "grpMapFlip"
        Me.grpMapFlip.TabStop = False
        '
        'btnMapFlipinfo
        '
        Me.btnMapFlipinfo.BackColor = System.Drawing.SystemColors.Highlight
        resources.ApplyResources(Me.btnMapFlipinfo, "btnMapFlipinfo")
        Me.btnMapFlipinfo.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnMapFlipinfo.Name = "btnMapFlipinfo"
        Me.btnMapFlipinfo.UseVisualStyleBackColor = False
        '
        'lblMapFlipGPXd
        '
        resources.ApplyResources(Me.lblMapFlipGPXd, "lblMapFlipGPXd")
        Me.lblMapFlipGPXd.Name = "lblMapFlipGPXd"
        '
        'lblMapFlipStarttime
        '
        resources.ApplyResources(Me.lblMapFlipStarttime, "lblMapFlipStarttime")
        Me.lblMapFlipStarttime.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "MapFlipStartT", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.lblMapFlipStarttime.Name = "lblMapFlipStarttime"
        Me.lblMapFlipStarttime.Text = Global.OHeadcamMapApp.My.MySettings.Default.MapFlipStartT
        '
        'Label19
        '
        resources.ApplyResources(Me.Label19, "Label19")
        Me.Label19.Name = "Label19"
        '
        'Label18
        '
        resources.ApplyResources(Me.Label18, "Label18")
        Me.Label18.Name = "Label18"
        '
        'Label17
        '
        resources.ApplyResources(Me.Label17, "Label17")
        Me.Label17.Name = "Label17"
        '
        'Label11
        '
        resources.ApplyResources(Me.Label11, "Label11")
        Me.Label11.Name = "Label11"
        '
        'btnQRimg2
        '
        resources.ApplyResources(Me.btnQRimg2, "btnQRimg2")
        Me.btnQRimg2.Name = "btnQRimg2"
        Me.btnQRimg2.UseVisualStyleBackColor = True
        '
        'btnQRXML2
        '
        resources.ApplyResources(Me.btnQRXML2, "btnQRXML2")
        Me.btnQRXML2.Name = "btnQRXML2"
        Me.btnQRXML2.UseVisualStyleBackColor = True
        '
        'lblTrackVideoInUse
        '
        resources.ApplyResources(Me.lblTrackVideoInUse, "lblTrackVideoInUse")
        Me.lblTrackVideoInUse.ForeColor = System.Drawing.Color.Red
        Me.lblTrackVideoInUse.Name = "lblTrackVideoInUse"
        '
        'btnTestWriteVideo
        '
        resources.ApplyResources(Me.btnTestWriteVideo, "btnTestWriteVideo")
        Me.btnTestWriteVideo.Name = "btnTestWriteVideo"
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
End Class
