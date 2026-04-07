<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSideBySide
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
        Dim MySettings1 As OHeadcamMapApp.My.MySettings = New OHeadcamMapApp.My.MySettings()
        Dim MySettings2 As OHeadcamMapApp.My.MySettings = New OHeadcamMapApp.My.MySettings()
        Dim MySettings3 As OHeadcamMapApp.My.MySettings = New OHeadcamMapApp.My.MySettings()
        Dim MySettings4 As OHeadcamMapApp.My.MySettings = New OHeadcamMapApp.My.MySettings()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.btnSbSOutfile = New System.Windows.Forms.Button()
        Me.btnSbSRun = New System.Windows.Forms.Button()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtSbSOut = New System.Windows.Forms.TextBox()
        Me.txtSbSLength = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtSbSLeft = New System.Windows.Forms.TextBox()
        Me.btnSbSRfile = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.btnSbSLfile = New System.Windows.Forms.Button()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.txtSbSRight = New System.Windows.Forms.TextBox()
        Me.OpenFileDialogsbslfile = New System.Windows.Forms.OpenFileDialog()
        Me.OpenFileDialogsbsrfile = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialogsbsOut = New System.Windows.Forms.SaveFileDialog()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.btnSbSOutfile)
        Me.GroupBox3.Controls.Add(Me.btnSbSRun)
        Me.GroupBox3.Controls.Add(Me.Label19)
        Me.GroupBox3.Controls.Add(Me.txtSbSOut)
        Me.GroupBox3.Controls.Add(Me.txtSbSLength)
        Me.GroupBox3.Controls.Add(Me.Label18)
        Me.GroupBox3.Controls.Add(Me.txtSbSLeft)
        Me.GroupBox3.Controls.Add(Me.btnSbSRfile)
        Me.GroupBox3.Controls.Add(Me.Label11)
        Me.GroupBox3.Controls.Add(Me.btnSbSLfile)
        Me.GroupBox3.Controls.Add(Me.Label17)
        Me.GroupBox3.Controls.Add(Me.txtSbSRight)
        Me.GroupBox3.Location = New System.Drawing.Point(26, 12)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(439, 247)
        Me.GroupBox3.TabIndex = 30
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Side om side video(til sammenligning)"
        '
        'btnSbSOutfile
        '
        Me.btnSbSOutfile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnSbSOutfile.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnSbSOutfile.Location = New System.Drawing.Point(225, 180)
        Me.btnSbSOutfile.Name = "btnSbSOutfile"
        Me.btnSbSOutfile.Size = New System.Drawing.Size(104, 23)
        Me.btnSbSOutfile.TabIndex = 29
        Me.btnSbSOutfile.Text = "Gennemse..."
        Me.btnSbSOutfile.UseVisualStyleBackColor = True
        '
        'btnSbSRun
        '
        Me.btnSbSRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnSbSRun.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnSbSRun.Location = New System.Drawing.Point(6, 218)
        Me.btnSbSRun.Name = "btnSbSRun"
        Me.btnSbSRun.Size = New System.Drawing.Size(75, 23)
        Me.btnSbSRun.TabIndex = 31
        Me.btnSbSRun.Text = "Kør"
        Me.btnSbSRun.UseVisualStyleBackColor = True
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label19.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label19.Location = New System.Drawing.Point(5, 164)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(117, 16)
        Me.Label19.TabIndex = 28
        Me.Label19.Text = "Output video(MP4)"
        '
        'txtSbSOut
        '
        MySettings1.AdjTestVideoReady = False
        MySettings1.AdjVideoGPSDiff = "0"
        MySettings1.AdjVideoLength = "300"
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
        MySettings1.ckShowLegMap = True
        MySettings1.ckShowRouteMap = True
        MySettings1.dFfmpegScaleMap = New Decimal(New Integer() {1, 0, 0, 0})
        MySettings1.ffmpegBufsize = "40M"
        MySettings1.ffmpegCRF = "26"
        MySettings1.ffmpegJoin = "-f concat -safe 0 -i joinlist.txt"
        MySettings1.ffmpegLensCorrection = "lenscorrection=cx=0.5:cy=0.5:k1=-0.200:k2=0.000"
        MySettings1.ffmpegMaxBitr = "20M"
        MySettings1.ffmpegOutFps = "25"
        'MySettings1.ffmpegPreset = "fast"
        MySettings1.ffmpegScaleMap = "1.5"
        MySettings1.ffmpegVidstabDetect = "vidstabdetect=shakiness=10:accuracy=15:mincontrast=0.200:stepsize=6:show=0:result" &
    "=data.trf:tripod=0:result=data.trf"
        MySettings1.ffmpegVidstabDetectOut = " -f null -"
        MySettings1.ffmpegVidstabTransform = "vidstabtransform=smoothing=25:crop=black:zoom=0:optzoom=0:interpol='bicubic':inpu" &
    "t=data.trf:tripod=0,unsharp=7:7:3:7:7:3"
        MySettings1.GPXDiff = "0"
        MySettings1.GPXfile = ""
        MySettings1.JoinFileLength = 0!
        MySettings1.Language = ""
        MySettings1.lblMapLength = "0:00:00"
        MySettings1.MapLength = "0:00:00"
        MySettings1.MHeightH = "10"
        MySettings1.MHeightV = "10"
        MySettings1.MIDotColor = System.Drawing.Color.Red
        MySettings1.MIDotSize = 15
        MySettings1.MIFLegDim = New System.Drawing.Point(0, 0)
        MySettings1.MIFLegPos = New System.Drawing.Point(0, 0)
        MySettings1.MIFrameColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        MySettings1.MIFrameWidth = 4
        MySettings1.MIFZoomDim = New System.Drawing.Point(0, 0)
        MySettings1.MIFZoomPos = New System.Drawing.Point(0, 0)
        MySettings1.MILegHeight = 600
        MySettings1.MILegMapPos = New System.Drawing.Point(80, 450)
        MySettings1.MILegMargin = 50
        MySettings1.MILegRad = 40
        MySettings1.MILegWidth = 350
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
        MySettings1.PrepareLength = ""
        MySettings1.QRimage = ""
        MySettings1.QRLength = "0"
        MySettings1.QRXML = ""
        MySettings1.QRXMLTimesec = 0
        MySettings1.rbSetImgL = False
        MySettings1.rbSetImgR = False
        MySettings1.SbSLength = ""
        MySettings1.SbSLfile = ""
        MySettings1.SbSOutfile = "Out.mp4"
        MySettings1.SbSRfile = ""
        MySettings1.SettingSbSCode = "-filter_complex hstack"
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
        Me.txtSbSOut.DataBindings.Add(New System.Windows.Forms.Binding("Text", MySettings1, "SbSOutfile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtSbSOut.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.txtSbSOut.Location = New System.Drawing.Point(6, 180)
        Me.txtSbSOut.Name = "txtSbSOut"
        Me.txtSbSOut.Size = New System.Drawing.Size(213, 22)
        Me.txtSbSOut.TabIndex = 27
        Me.txtSbSOut.Text = MySettings1.SbSOutfile
        '
        'txtSbSLength
        '
        MySettings2.AdjTestVideoReady = False
        MySettings2.AdjVideoGPSDiff = "0"
        MySettings2.AdjVideoLength = "300"
        MySettings2.Audiolevel = ""
        MySettings2.AudioVideofile = ""
        MySettings2.bAdjVideoIsReady = False
        MySettings2.bDoMusic = False
        MySettings2.bOutputfileReady = False
        MySettings2.bUseMapTrackingVideo = False
        MySettings2.cbHeightGraph = True
        MySettings2.cbLoopMusic = True
        MySettings2.cbSetImgM = False
        MySettings2.cbShowLegMAp = True
        MySettings2.cbShowRoute = True
        MySettings2.cbShowSpeedPanel = True
        MySettings2.cbXMakeframes = False
        MySettings2.ckShowLegMap = True
        MySettings2.ckShowRouteMap = True
        MySettings2.dFfmpegScaleMap = New Decimal(New Integer() {1, 0, 0, 0})
        MySettings2.ffmpegBufsize = "40M"
        MySettings2.ffmpegCRF = "26"
        MySettings2.ffmpegJoin = "-f concat -safe 0 -i joinlist.txt"
        MySettings2.ffmpegLensCorrection = "lenscorrection=cx=0.5:cy=0.5:k1=-0.200:k2=0.000"
        MySettings2.ffmpegMaxBitr = "20M"
        MySettings2.ffmpegOutFps = "25"
        'MySettings2.ffmpegPreset = "fast"
        MySettings2.ffmpegScaleMap = "1.5"
        MySettings2.ffmpegVidstabDetect = "vidstabdetect=shakiness=10:accuracy=15:mincontrast=0.200:stepsize=6:show=0:result" &
    "=data.trf:tripod=0:result=data.trf"
        MySettings2.ffmpegVidstabDetectOut = " -f null -"
        MySettings2.ffmpegVidstabTransform = "vidstabtransform=smoothing=25:crop=black:zoom=0:optzoom=0:interpol='bicubic':inpu" &
    "t=data.trf:tripod=0,unsharp=7:7:3:7:7:3"
        MySettings2.GPXDiff = "0"
        MySettings2.GPXfile = ""
        MySettings2.JoinFileLength = 0!
        MySettings2.Language = ""
        MySettings2.lblMapLength = "0:00:00"
        MySettings2.MapLength = "0:00:00"
        MySettings2.MHeightH = "10"
        MySettings2.MHeightV = "10"
        MySettings2.MIDotColor = System.Drawing.Color.Red
        MySettings2.MIDotSize = 15
        MySettings2.MIFLegDim = New System.Drawing.Point(0, 0)
        MySettings2.MIFLegPos = New System.Drawing.Point(0, 0)
        MySettings2.MIFrameColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        MySettings2.MIFrameWidth = 4
        MySettings2.MIFZoomDim = New System.Drawing.Point(0, 0)
        MySettings2.MIFZoomPos = New System.Drawing.Point(0, 0)
        MySettings2.MILegHeight = 600
        MySettings2.MILegMapPos = New System.Drawing.Point(80, 450)
        MySettings2.MILegMargin = 50
        MySettings2.MILegRad = 40
        MySettings2.MILegWidth = 350
        MySettings2.MITailColor = System.Drawing.Color.Red
        MySettings2.MITailDuration = 30
        MySettings2.MITailRatio = New Decimal(New Integer() {9, 0, 0, 65536})
        MySettings2.MIZoomCircle = True
        MySettings2.MIZoomHeight = 350
        MySettings2.MIZoomMapPos = New System.Drawing.Point(80, 100)
        MySettings2.MIZoomRad = 80
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
        MySettings2.Outputfile = ""
        MySettings2.OutputLength = ""
        MySettings2.OutputVideoLength = "0:00:00"
        MySettings2.PrepareLength = ""
        MySettings2.QRimage = ""
        MySettings2.QRLength = "0"
        MySettings2.QRXML = ""
        MySettings2.QRXMLTimesec = 0
        MySettings2.rbSetImgL = False
        MySettings2.rbSetImgR = False
        MySettings2.SbSLength = ""
        MySettings2.SbSLfile = ""
        MySettings2.SbSOutfile = "Out.mp4"
        MySettings2.SbSRfile = ""
        MySettings2.SettingSbSCode = "-filter_complex hstack"
        MySettings2.SettingsKey = ""
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
        MySettings2.Videofiles = ""
        MySettings2.videofiles2 = ""
        MySettings2.Videofilesfull = ""
        MySettings2.VideoInLength = "0:00:00"
        MySettings2.VideoPadding = "C"
        MySettings2.VideoWorkFolder = ""
        Me.txtSbSLength.DataBindings.Add(New System.Windows.Forms.Binding("Text", MySettings2, "SbSLength", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtSbSLength.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.txtSbSLength.Location = New System.Drawing.Point(8, 45)
        Me.txtSbSLength.Name = "txtSbSLength"
        Me.txtSbSLength.Size = New System.Drawing.Size(75, 22)
        Me.txtSbSLength.TabIndex = 30
        Me.txtSbSLength.Text = MySettings2.SbSLength
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label18.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label18.Location = New System.Drawing.Point(3, 26)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(123, 16)
        Me.Label18.TabIndex = 29
        Me.Label18.Text = "Video længde(sek)"
        '
        'txtSbSLeft
        '
        MySettings3.AdjTestVideoReady = False
        MySettings3.AdjVideoGPSDiff = "0"
        MySettings3.AdjVideoLength = "300"
        MySettings3.Audiolevel = ""
        MySettings3.AudioVideofile = ""
        MySettings3.bAdjVideoIsReady = False
        MySettings3.bDoMusic = False
        MySettings3.bOutputfileReady = False
        MySettings3.bUseMapTrackingVideo = False
        MySettings3.cbHeightGraph = True
        MySettings3.cbLoopMusic = True
        MySettings3.cbSetImgM = False
        MySettings3.cbShowLegMAp = True
        MySettings3.cbShowRoute = True
        MySettings3.cbShowSpeedPanel = True
        MySettings3.cbXMakeframes = False
        MySettings3.ckShowLegMap = True
        MySettings3.ckShowRouteMap = True
        MySettings3.dFfmpegScaleMap = New Decimal(New Integer() {1, 0, 0, 0})
        MySettings3.ffmpegBufsize = "40M"
        MySettings3.ffmpegCRF = "26"
        MySettings3.ffmpegJoin = "-f concat -safe 0 -i joinlist.txt"
        MySettings3.ffmpegLensCorrection = "lenscorrection=cx=0.5:cy=0.5:k1=-0.200:k2=0.000"
        MySettings3.ffmpegMaxBitr = "20M"
        MySettings3.ffmpegOutFps = "25"
        MySettings3.ffmpegPreset = "fast"
        MySettings3.ffmpegScaleMap = "1.5"
        MySettings3.ffmpegVidstabDetect = "vidstabdetect=shakiness=10:accuracy=15:mincontrast=0.200:stepsize=6:show=0:result" &
    "=data.trf:tripod=0:result=data.trf"
        MySettings3.ffmpegVidstabDetectOut = " -f null -"
        MySettings3.ffmpegVidstabTransform = "vidstabtransform=smoothing=25:crop=black:zoom=0:optzoom=0:interpol='bicubic':inpu" &
    "t=data.trf:tripod=0,unsharp=7:7:3:7:7:3"
        MySettings3.GPXDiff = "0"
        MySettings3.GPXfile = ""
        MySettings3.JoinFileLength = 0!
        MySettings3.Language = ""
        MySettings3.lblMapLength = "0:00:00"
        MySettings3.MapLength = "0:00:00"
        MySettings3.MHeightH = "10"
        MySettings3.MHeightV = "10"
        MySettings3.MIDotColor = System.Drawing.Color.Red
        MySettings3.MIDotSize = 15
        MySettings3.MIFLegDim = New System.Drawing.Point(0, 0)
        MySettings3.MIFLegPos = New System.Drawing.Point(0, 0)
        MySettings3.MIFrameColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        MySettings3.MIFrameWidth = 4
        MySettings3.MIFZoomDim = New System.Drawing.Point(0, 0)
        MySettings3.MIFZoomPos = New System.Drawing.Point(0, 0)
        MySettings3.MILegHeight = 600
        MySettings3.MILegMapPos = New System.Drawing.Point(80, 450)
        MySettings3.MILegMargin = 50
        MySettings3.MILegRad = 40
        MySettings3.MILegWidth = 350
        MySettings3.MITailColor = System.Drawing.Color.Red
        MySettings3.MITailDuration = 30
        MySettings3.MITailRatio = New Decimal(New Integer() {9, 0, 0, 65536})
        MySettings3.MIZoomCircle = True
        MySettings3.MIZoomHeight = 350
        MySettings3.MIZoomMapPos = New System.Drawing.Point(80, 100)
        MySettings3.MIZoomRad = 80
        MySettings3.MIZoomWidth = 350
        MySettings3.MIZoomZoom = 1.0R
        MySettings3.MLegMapH = "10"
        MySettings3.MlegMapH2 = "10"
        MySettings3.MLegMapV = "10"
        MySettings3.MRouteH = "10"
        MySettings3.MRouteV = "10"
        MySettings3.MSpeedH = "10"
        MySettings3.MSpeedV = "10"
        MySettings3.MusicAudiofile = ""
        MySettings3.MusicFFMpegParam = ""
        MySettings3.Musicfile = ""
        MySettings3.Musiclevel = ""
        MySettings3.MusicOutputfile = ""
        MySettings3.No_deshake_filter = "normalize=blackpt=black:whitept=white:smoothing=10:strength=0.5"
        MySettings3.NoAudio = False
        MySettings3.Outputfile = ""
        MySettings3.OutputLength = ""
        MySettings3.OutputVideoLength = "0:00:00"
        MySettings3.PrepareLength = ""
        MySettings3.QRimage = ""
        MySettings3.QRLength = "0"
        MySettings3.QRXML = ""
        MySettings3.QRXMLTimesec = 0
        MySettings3.rbSetImgL = False
        MySettings3.rbSetImgR = False
        MySettings3.SbSLength = ""
        MySettings3.SbSLfile = ""
        MySettings3.SbSOutfile = "Out.mp4"
        MySettings3.SbSRfile = ""
        MySettings3.SettingSbSCode = "-filter_complex hstack"
        MySettings3.SettingsKey = ""
        MySettings3.StatusInputVideo = ""
        MySettings3.StatusMakeMap = ""
        MySettings3.StatusOutputVideo = ""
        MySettings3.StatusPrepare = ""
        MySettings3.trackAudiolevel = 0
        MySettings3.TrackMapHeight = 0
        MySettings3.TrackMapImageFile = ""
        MySettings3.TrackMapVideoFilename = ""
        MySettings3.TrackMapVideoLength = "0:00:00"
        MySettings3.TrackMapWidth = 0
        MySettings3.trackMusiclevel = 0
        MySettings3.Transparency = New Decimal(New Integer() {1, 0, 0, 0})
        MySettings3.txtGPSPos = ""
        MySettings3.txtInpCutFromStart = ""
        MySettings3.txtMapVideoFilename = ""
        MySettings3.txtMusicOutputfile = ""
        MySettings3.txtRealtimeFactor = ""
        MySettings3.txtVideoPos = ""
        MySettings3.VidAdj_Rot = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings3.VidAdj_Zoom = New Decimal(New Integer() {100, 0, 0, 0})
        MySettings3.VidAdjBright = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings3.VidAdjColImpr = False
        MySettings3.VidAdjContr = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings3.VidAdjGamma = New Decimal(New Integer() {1, 0, 0, 0})
        MySettings3.VidAdjHue = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings3.VidAdjLensDist = False
        MySettings3.VidAdjNoise = False
        MySettings3.VidAdjSatur = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings3.VidAdjSharp = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings3.Videofiles = ""
        MySettings3.videofiles2 = ""
        MySettings3.Videofilesfull = ""
        MySettings3.VideoInLength = "0:00:00"
        MySettings3.VideoPadding = "C"
        MySettings3.VideoWorkFolder = ""
        Me.txtSbSLeft.DataBindings.Add(New System.Windows.Forms.Binding("Text", MySettings3, "SbSLfile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtSbSLeft.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.txtSbSLeft.Location = New System.Drawing.Point(6, 86)
        Me.txtSbSLeft.Name = "txtSbSLeft"
        Me.txtSbSLeft.Size = New System.Drawing.Size(213, 22)
        Me.txtSbSLeft.TabIndex = 8
        Me.txtSbSLeft.Text = MySettings3.SbSLfile
        '
        'btnSbSRfile
        '
        Me.btnSbSRfile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnSbSRfile.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnSbSRfile.Location = New System.Drawing.Point(225, 131)
        Me.btnSbSRfile.Name = "btnSbSRfile"
        Me.btnSbSRfile.Size = New System.Drawing.Size(104, 23)
        Me.btnSbSRfile.TabIndex = 28
        Me.btnSbSRfile.Text = "Gennemse..."
        Me.btnSbSRfile.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label11.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label11.Location = New System.Drawing.Point(3, 70)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(90, 16)
        Me.Label11.TabIndex = 9
        Me.Label11.Text = "Venstre video"
        '
        'btnSbSLfile
        '
        Me.btnSbSLfile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.btnSbSLfile.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnSbSLfile.Location = New System.Drawing.Point(225, 86)
        Me.btnSbSLfile.Name = "btnSbSLfile"
        Me.btnSbSLfile.Size = New System.Drawing.Size(104, 23)
        Me.btnSbSLfile.TabIndex = 27
        Me.btnSbSLfile.Text = "Gennemse..."
        Me.btnSbSLfile.UseVisualStyleBackColor = True
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.Label17.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.Label17.Location = New System.Drawing.Point(8, 115)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(77, 16)
        Me.Label17.TabIndex = 11
        Me.Label17.Text = "Højre video"
        '
        'txtSbSRight
        '
        MySettings4.AdjTestVideoReady = False
        MySettings4.AdjVideoGPSDiff = "0"
        MySettings4.AdjVideoLength = "300"
        MySettings4.Audiolevel = ""
        MySettings4.AudioVideofile = ""
        MySettings4.bAdjVideoIsReady = False
        MySettings4.bDoMusic = False
        MySettings4.bOutputfileReady = False
        MySettings4.bUseMapTrackingVideo = False
        MySettings4.cbHeightGraph = True
        MySettings4.cbLoopMusic = True
        MySettings4.cbSetImgM = False
        MySettings4.cbShowLegMAp = True
        MySettings4.cbShowRoute = True
        MySettings4.cbShowSpeedPanel = True
        MySettings4.cbXMakeframes = False
        MySettings4.ckShowLegMap = True
        MySettings4.ckShowRouteMap = True
        MySettings4.dFfmpegScaleMap = New Decimal(New Integer() {1, 0, 0, 0})
        MySettings4.ffmpegBufsize = "40M"
        MySettings4.ffmpegCRF = "26"
        MySettings4.ffmpegJoin = "-f concat -safe 0 -i joinlist.txt"
        MySettings4.ffmpegLensCorrection = "lenscorrection=cx=0.5:cy=0.5:k1=-0.200:k2=0.000"
        MySettings4.ffmpegMaxBitr = "20M"
        MySettings4.ffmpegOutFps = "25"
        MySettings4.ffmpegPreset = "fast"
        MySettings4.ffmpegScaleMap = "1.5"
        MySettings4.ffmpegVidstabDetect = "vidstabdetect=shakiness=10:accuracy=15:mincontrast=0.200:stepsize=6:show=0:result" &
    "=data.trf:tripod=0:result=data.trf"
        MySettings4.ffmpegVidstabDetectOut = " -f null -"
        MySettings4.ffmpegVidstabTransform = "vidstabtransform=smoothing=25:crop=black:zoom=0:optzoom=0:interpol='bicubic':inpu" &
    "t=data.trf:tripod=0,unsharp=7:7:3:7:7:3"
        MySettings4.GPXDiff = "0"
        MySettings4.GPXfile = ""
        MySettings4.JoinFileLength = 0!
        MySettings4.Language = ""
        MySettings4.lblMapLength = "0:00:00"
        MySettings4.MapLength = "0:00:00"
        MySettings4.MHeightH = "10"
        MySettings4.MHeightV = "10"
        MySettings4.MIDotColor = System.Drawing.Color.Red
        MySettings4.MIDotSize = 15
        MySettings4.MIFLegDim = New System.Drawing.Point(0, 0)
        MySettings4.MIFLegPos = New System.Drawing.Point(0, 0)
        MySettings4.MIFrameColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        MySettings4.MIFrameWidth = 4
        MySettings4.MIFZoomDim = New System.Drawing.Point(0, 0)
        MySettings4.MIFZoomPos = New System.Drawing.Point(0, 0)
        MySettings4.MILegHeight = 600
        MySettings4.MILegMapPos = New System.Drawing.Point(80, 450)
        MySettings4.MILegMargin = 50
        MySettings4.MILegRad = 40
        MySettings4.MILegWidth = 350
        MySettings4.MITailColor = System.Drawing.Color.Red
        MySettings4.MITailDuration = 30
        MySettings4.MITailRatio = New Decimal(New Integer() {9, 0, 0, 65536})
        MySettings4.MIZoomCircle = True
        MySettings4.MIZoomHeight = 350
        MySettings4.MIZoomMapPos = New System.Drawing.Point(80, 100)
        MySettings4.MIZoomRad = 80
        MySettings4.MIZoomWidth = 350
        MySettings4.MIZoomZoom = 1.0R
        MySettings4.MLegMapH = "10"
        MySettings4.MlegMapH2 = "10"
        MySettings4.MLegMapV = "10"
        MySettings4.MRouteH = "10"
        MySettings4.MRouteV = "10"
        MySettings4.MSpeedH = "10"
        MySettings4.MSpeedV = "10"
        MySettings4.MusicAudiofile = ""
        MySettings4.MusicFFMpegParam = ""
        MySettings4.Musicfile = ""
        MySettings4.Musiclevel = ""
        MySettings4.MusicOutputfile = ""
        MySettings4.No_deshake_filter = "normalize=blackpt=black:whitept=white:smoothing=10:strength=0.5"
        MySettings4.NoAudio = False
        MySettings4.Outputfile = ""
        MySettings4.OutputLength = ""
        MySettings4.OutputVideoLength = "0:00:00"
        MySettings4.PrepareLength = ""
        MySettings4.QRimage = ""
        MySettings4.QRLength = "0"
        MySettings4.QRXML = ""
        MySettings4.QRXMLTimesec = 0
        MySettings4.rbSetImgL = False
        MySettings4.rbSetImgR = False
        MySettings4.SbSLength = ""
        MySettings4.SbSLfile = ""
        MySettings4.SbSOutfile = "Out.mp4"
        MySettings4.SbSRfile = ""
        MySettings4.SettingSbSCode = "-filter_complex hstack"
        MySettings4.SettingsKey = ""
        MySettings4.StatusInputVideo = ""
        MySettings4.StatusMakeMap = ""
        MySettings4.StatusOutputVideo = ""
        MySettings4.StatusPrepare = ""
        MySettings4.trackAudiolevel = 0
        MySettings4.TrackMapHeight = 0
        MySettings4.TrackMapImageFile = ""
        MySettings4.TrackMapVideoFilename = ""
        MySettings4.TrackMapVideoLength = "0:00:00"
        MySettings4.TrackMapWidth = 0
        MySettings4.trackMusiclevel = 0
        MySettings4.Transparency = New Decimal(New Integer() {1, 0, 0, 0})
        MySettings4.txtGPSPos = ""
        MySettings4.txtInpCutFromStart = ""
        MySettings4.txtMapVideoFilename = ""
        MySettings4.txtMusicOutputfile = ""
        MySettings4.txtRealtimeFactor = ""
        MySettings4.txtVideoPos = ""
        MySettings4.VidAdj_Rot = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings4.VidAdj_Zoom = New Decimal(New Integer() {100, 0, 0, 0})
        MySettings4.VidAdjBright = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings4.VidAdjColImpr = False
        MySettings4.VidAdjContr = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings4.VidAdjGamma = New Decimal(New Integer() {1, 0, 0, 0})
        MySettings4.VidAdjHue = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings4.VidAdjLensDist = False
        MySettings4.VidAdjNoise = False
        MySettings4.VidAdjSatur = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings4.VidAdjSharp = New Decimal(New Integer() {0, 0, 0, 0})
        MySettings4.Videofiles = ""
        MySettings4.videofiles2 = ""
        MySettings4.Videofilesfull = ""
        MySettings4.VideoInLength = "0:00:00"
        MySettings4.VideoPadding = "C"
        MySettings4.VideoWorkFolder = ""
        Me.txtSbSRight.DataBindings.Add(New System.Windows.Forms.Binding("Text", MySettings4, "SbSRfile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtSbSRight.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!)
        Me.txtSbSRight.Location = New System.Drawing.Point(8, 131)
        Me.txtSbSRight.Name = "txtSbSRight"
        Me.txtSbSRight.Size = New System.Drawing.Size(213, 22)
        Me.txtSbSRight.TabIndex = 10
        Me.txtSbSRight.Text = MySettings4.SbSRfile
        '
        'OpenFileDialogsbslfile
        '
        Me.OpenFileDialogsbslfile.Filter = "Video filer|*.mp4;*.avi;*.mov"
        '
        'OpenFileDialogsbsrfile
        '
        Me.OpenFileDialogsbsrfile.Filter = "Video filer|*.mp4;*.avi;*.mov"
        '
        'SaveFileDialogsbsOut
        '
        Me.SaveFileDialogsbsOut.Filter = "Video filer|*.mp4;*.avi"
        '
        'frmSideBySide
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(528, 295)
        Me.Controls.Add(Me.GroupBox3)
        Me.Name = "frmSideBySide"
        Me.Text = "Side om side video"
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents btnSbSOutfile As Button
    Friend WithEvents btnSbSRun As Button
    Friend WithEvents Label19 As Label
    Friend WithEvents txtSbSOut As TextBox
    Friend WithEvents txtSbSLength As TextBox
    Friend WithEvents Label18 As Label
    Friend WithEvents txtSbSLeft As TextBox
    Friend WithEvents btnSbSRfile As Button
    Friend WithEvents Label11 As Label
    Friend WithEvents btnSbSLfile As Button
    Friend WithEvents Label17 As Label
    Friend WithEvents txtSbSRight As TextBox
    Friend WithEvents OpenFileDialogsbslfile As OpenFileDialog
    Friend WithEvents OpenFileDialogsbsrfile As OpenFileDialog
    Friend WithEvents SaveFileDialogsbsOut As SaveFileDialog
End Class
