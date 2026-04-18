Imports System.ComponentModel
Imports System.Collections.Generic
Imports System.Globalization
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Threading
Imports OHeadcamMapApp.My.Resources
'Imports System.Timers

Public Class MainForm
    Public Videolist, VideoCropStart, VideoCropEnd As String
    'OLD MAP 20260407:Public inifile As clsInifile = New clsInifile
    Public proc As clsRunprocess
    Public Shared CurrentLog As String
    Public Shared StatusCount As Integer
    Public AppFolder As String
    Private Shared ProcessFinished As Boolean
    Private Shared MeVar As MainForm
    Private uiContext As SynchronizationContext
    Private uiThreadId As Integer
    'OLD MAP 20260407:
    'Public cmd1Join As String = "VIDEOJOIN"
    'Public cmd2Deshake As String = "DESHAKE"
    'Public cmd3Mapframes As String = "MAPFRAMES"
    'Public cmd4MakeVideo As String = "MAKEVIDEO"
    'Public iniOutFile As String = "FINALVIDEO"
    'Public iniVideo As String = "VIDEO"
    'Public iniGPX As String = "GPX"
    'Public iniQRIMG As String = "QUICKROUTE_IMG"
    'Public iniQRXML As String = "QUICKROUTE_ROUTEDATA"
    'Public iniCropStart As String = "CROPSTART"
    'Public iniCropEnd As String = "CROPEND"
    'Public iniFPS As String = "FRAMERATE"
    'Public IniFN As String = "RGmapvideo.ini"
    'Public ExeF As String = "RGmapvideo.exe"
    Public FFMpegExe As String = "ffmpeg.exe"
    Public ExtraFunc As clsExtra
    Public VidstabTransform As String
    Public VidstabDetect As String
    Public JoinedFilename As String = "joined.mp4"
    Public DeshakeFilename As String = "deshaked.mp4"
    Public LogJoin, LogDeshake, LogMapF, LogMakeVideo, LogStatus As String
    Private LastSmoothAssetElapsed As TimeSpan = TimeSpan.Zero
    Private LastSmoothRenderElapsed As TimeSpan = TimeSpan.Zero
    Private LastSmoothZoomEncodeElapsed As TimeSpan = TimeSpan.Zero
    Private LastSmoothLegEncodeElapsed As TimeSpan = TimeSpan.Zero
    Private LastSmoothFrameCount As Integer = 0
    Private LastSmoothBaseMapElapsed As TimeSpan = TimeSpan.Zero
    Private LastSmoothBackgroundElapsed As TimeSpan = TimeSpan.Zero
    Private LastSmoothOverlayMapElapsed As TimeSpan = TimeSpan.Zero
    Private LastSmoothOverlayFrameElapsed As TimeSpan = TimeSpan.Zero
    Private LastSmoothDrawElapsed As TimeSpan = TimeSpan.Zero
    Private LastSmoothStreamWriteElapsed As TimeSpan = TimeSpan.Zero
    Private LastSmoothZoomRenderElapsed As TimeSpan = TimeSpan.Zero
    Private LastSmoothLegRenderElapsed As TimeSpan = TimeSpan.Zero
    Private LastWholeMapStageElapsed As TimeSpan = TimeSpan.Zero
    Private LastWholeOverlayStageElapsed As TimeSpan = TimeSpan.Zero
    Private LastWholeVideoTotalElapsed As TimeSpan = TimeSpan.Zero
    Private _autorunRenderCompareStarted As Boolean = False
    Private _autorunRenderLegStarted As Boolean = False
    Public strQuote As String = Chr(34)
    Public bMakeMapDirty, bMakeVideoInDirty, bOutputVideoDirty As Boolean
    Public bProcessIsRunning As Boolean = False ' Used to interrupt background processes
    Public RemainingTimeObj As clsTimeRemaining = New clsTimeRemaining
    Public Shared strCurrentDone As String
    Public WriteToFiles As clMapsImgsToFiles
    Public WriteToFilesComplete As Boolean = False
    Private Const SmoothOverlayFolderName As String = "temp_smooth"
    Private Const SmoothOverlayBaseName As String = "map_overlay.mp4"
    Private Class RenderCompareOptions
        Public Property Variant1 As String = "perf"
        Public Property Variant2 As String = "perf8"
        Public Property StartTime As Double = 360
        Public Property Duration As Double = 120
        Public Property VideoWidth As Integer = 1920
        Public Property OutputFile As String = ""
        Public Property LogFile As String = ""
        Public Property AutoClose As Boolean = True
    End Class

    Private Class RenderStyleOverrides
        Public Property LegWidth As Integer?
        Public Property LegHeight As Integer?
        Public Property TailDurationSeconds As Integer?
        Public Property DotSize As Integer?
        Public Property DotTailRatio As Double?
        Public Property ArrowBarb As Double?
        Public Property ArrowWidth As Double?
        Public Property DotType As String = ""
        Public Property FrameFeather As Boolean?
        Public Property TailColorArgb As Integer?
        Public Property DotColorArgb As Integer?
        Public Property TailAlpha As Integer?
        Public Property SpeedColoringEnabled As Boolean?
        Public Property PaceFastSecondsPerKm As Double?
        Public Property PaceSlowSecondsPerKm As Double?
        Public Property TailTicksEnabled As Boolean?
        Public Property TailTickIntervalSeconds As Double?
        Public Property TailTickAlpha As Integer?
        Public Property TailTickColorArgb As Integer?
    End Class

    Private Class RenderLegOptions
        Public Property RenderVariant As String = "perf8"
        Public Property StylePreset As String = ""
        Public Property StartTime As Double = 360
        Public Property Duration As Double = 120
        Public Property VideoWidth As Integer = 1920
        Public Property OutputFile As String = ""
        Public Property LogFile As String = ""
        Public Property AutoClose As Boolean = True
        Public Property XmlPath As String = ""
        Public Property ImagePath As String = ""
        Public Property FrameStepSeconds As Double = -1
        Public Property OutputFps As Double = -1
        Public Property Style As New RenderStyleOverrides()
    End Class


    'Public resources As ResourceManager = New ResourceManager("Texts", a  TypeOf MainForm)
    Public Sub InitStatusLabels()
        SetStatusLabel(iconStatusVideoIn, My.Settings.StatusInputVideo, "Inputvideo")
        SetStatusLabel(IconStatusOutput, My.Settings.StatusOutputVideo, "Outputvideo")
        SetStatusLabel(IconStatusPrepare, My.Settings.StatusPrepare, "Prepare")

        IconStatusPrepare.Parent = GroupBox1
        IconStatusOutput.Parent = GroupBox6

    End Sub
    Public Sub SaveStatusLabels()
        SaveStatusLabel(iconStatusVideoIn, "Inputvideo")
        SaveStatusLabel(IconStatusOutput, "Outputvideo")
        SaveStatusLabel(IconStatusPrepare, "Prepare")

    End Sub
    Public Sub SaveStatusLabel(LabelIn As Label, MySettingId As String)
        Dim Status As String

        If LabelIn.Image.Equals(OHeadcamMapApp.My.Resources.Resources.Failimg) Then
            Status = "Fail"
        End If
        If LabelIn.Image.Equals(OHeadcamMapApp.My.Resources.Resources.OKimg) Then
            Status = "OK"
        End If
        If LabelIn.Image.Equals(OHeadcamMapApp.My.Resources.Resources.Workimg) Then
            Status = "Work"
        End If
        If LabelIn.Image.Equals(OHeadcamMapApp.My.Resources.Resources.imgNotready) Or LabelIn.Image.Equals(OHeadcamMapApp.My.Resources.Resources.imgNotready) Then
            Status = ""
        End If
        Status = LabelIn.Tag
        Select Case MySettingId
            Case "Inputvideo"
                My.Settings.StatusInputVideo = Status
            Case "MakeMap"
                My.Settings.StatusMakeMap = Status
            Case "Outputvideo"
                My.Settings.StatusOutputVideo = Status
            Case "Prepare"
                My.Settings.StatusPrepare = Status
        End Select

    End Sub
    Public Sub SetStatusLabel(LabelIn As Label, Status As String, Optional MySettingID As String = "")
        If Me.InvokeRequired Then
            Me.BeginInvoke(Sub() SetStatusLabel(LabelIn, Status, MySettingID))
            Return
        End If

        Select Case Status
            Case "Fail"
                LabelIn.Image = OHeadcamMapApp.My.Resources.Resources.Failimg
            Case "OK"
                LabelIn.Image = OHeadcamMapApp.My.Resources.Resources.OKimg
            Case "Work"
                LabelIn.Image = OHeadcamMapApp.My.Resources.Resources.Workimg
            Case "NoVideo"
                LabelIn.Image = OHeadcamMapApp.My.Resources.Resources.imgNotready
                Status = ""
            Case "NoImage"
                LabelIn.Image = OHeadcamMapApp.My.Resources.Resources.imgNotready
                Status = ""
            Case ""
                Select Case MySettingID
                    Case "Inputvideo"
                        LabelIn.Image = OHeadcamMapApp.My.Resources.Resources.imgNotready
                    Case "MakeMap"
                        LabelIn.Image = OHeadcamMapApp.My.Resources.imgNotready
                    Case "Outputvideo"
                        LabelIn.Image = OHeadcamMapApp.My.Resources.Resources.imgNotready
                    Case "Prepare"
                        LabelIn.Image = OHeadcamMapApp.My.Resources.Resources.imgNotready
                End Select
        End Select
        LabelIn.Tag = Status
        Select Case MySettingID
            Case "Inputvideo"
                My.Settings.StatusInputVideo = Status
            Case "MakeMap"
                My.Settings.StatusMakeMap = Status
            Case "Outputvideo"
                My.Settings.StatusOutputVideo = Status
            Case "Prepare"
                My.Settings.StatusPrepare = Status
        End Select
    End Sub
    Public Sub CreateTmpfolders(name As String)
        Dim f As String
        f = AppFolder + name

        If Not My.Computer.FileSystem.DirectoryExists(f) Then
            My.Computer.FileSystem.CreateDirectory(f)
        End If
    End Sub
    Public Sub New()
        Dim lang As String
        MeVar = Me
        ' Capture UI synchronization context and thread id for cross-thread marshaling
        uiContext = SynchronizationContext.Current
        uiThreadId = Thread.CurrentThread.ManagedThreadId
        If My.Settings.Language = "English" Then
            lang = "en-US"
        ElseIf My.Settings.Language = "Chinese" Then
            lang = "zh"
        Else

            lang = "da-DK"
        End If
        Thread.CurrentThread.CurrentUICulture = New CultureInfo(lang)
        InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(New CultureInfo(lang))

        ' This call is required by the designer.
        InitializeComponent()

        'OLD MAP 20260407:inifile.SetLanguage(lang)

        ' Get the application's executable directory
        Dim exeDirectory As String = AppDomain.CurrentDomain.BaseDirectory

        ' Set the application's working directory to the executable directory
        Environment.CurrentDirectory = exeDirectory

        bMakeMapDirty = False
        bMakeVideoInDirty = False
        bOutputVideoDirty = False
        ' Add any initialization after the InitializeComponent() call.
        AppFolder = AppDomain.CurrentDomain.BaseDirectory
        AppFolder = AppFolder.Substring(0, AppDomain.CurrentDomain.BaseDirectory.LastIndexOf("\")) + "\"
        'OLD MAP 20260407:inifile.SetINIfile(AppFolder + IniFN)

        CreateTmpfolders("temp")
        CreateTmpfolders("temp1")
        CreateTmpfolders("temp2")

        If txtVideolist.Text = "" Then Videolist = ""

        ValidateFiles()

        ExtraFunc = New clsExtra

        InitVideoFormat()
        'lblMapLength.Text = ExtraFunc.SecToTimeStr(My.Settings.QRXMLTimesec)
        ExtraFunc.Language = lang
        ChangeLanguage(lang)
        ' Texts.Culture = New CultureInfo(lang)

        TrackVideoState()
        Me.Icon = My.Resources.Headcam_Orienteering_Icon
        chkMapFlipOnOff.Checked = My.Settings.MapFlipOnOff
        If Not My.Settings.MapFlipOnOff Then ' show or hide mapflip
            grpMapFlip.Visible = False
            If Not txtOutputLength.BackColor = SystemColors.Window Then 'Mapflip cut video active - deactivate
                txtOutputLength.Text = ""
                txtOutputLength.BackColor = SystemColors.Window
            End If
        End If
        MapFlip(False, False) ' deactivate map flip



    End Sub
    Public Sub InitVideoFormat()
        If My.Settings.chkHDFormat Then
            ExtraFunc.InputWidth = 1920
            ExtraFunc.InputHeight = 1080
        Else ' use the input format
            ExtraFunc.InputWidth = My.Settings.JoinFileWidth
            ExtraFunc.InputHeight = My.Settings.JoinFileHeight
        End If
        My.Settings.InfoOutVideoFormat = "Format: " & CStr(ExtraFunc.InputWidth) & "x" & CStr(Math.Round(ExtraFunc.InputWidth / 16 * 9))
    End Sub
    Function GetJoinedFilename(ForFFMPeg As Boolean) As String
        Dim FStr As String
        FStr = JoinedFilename
        If My.Settings.VideoWorkFolder = "" Then
            If ForFFMPeg Then
                Return FStr
            Else
                Return AppFolder + FStr
            End If
        Else
            If ForFFMPeg Then
                FStr = """" + My.Settings.VideoWorkFolder + FStr + """"
                'FStr = FStr.Replace("\", "\\") ' ffmpeg require double \
                'FStr = FStr.Replace(":", "\:")
                Return FStr
            Else
                Return My.Settings.VideoWorkFolder + FStr
            End If
        End If

    End Function
    Function GetDeshakedFilename(ForFFMPeg As Boolean) As String
        Dim FStr As String
        FStr = DeshakeFilename
        If My.Settings.VideoWorkFolder = "" Then
            If ForFFMPeg Then
                Return FStr
            Else
                Return AppFolder + FStr
            End If
        Else
            If ForFFMPeg Then
                FStr = """" + My.Settings.VideoWorkFolder + FStr + """"
                'FStr = FStr.Replace("\", "\\") ' ffmpeg require double \
                'FStr = FStr.Replace(":", "\:")
                Return FStr
            Else
                Return My.Settings.VideoWorkFolder + FStr
            End If
        End If
    End Function
    Sub TrackVideoState()
        lblTrackVideoInUse.Visible = My.Settings.bUseMapTrackingVideo
        GroupBox10.Enabled = Not My.Settings.bUseMapTrackingVideo 'Disable Quickroute if tracking video
        'OLD MAP: cbXMakeframes.Enabled = Not My.Settings.bUseMapTrackingVideo 'Disable Quickroute if tracking video
        If My.Settings.bUseMapTrackingVideo Then
            lblMapLength.Text = My.Settings.TrackMapVideoLength
        Else
            bMakeMapDirty = True
            CheckDirtyStatus()


        End If
    End Sub
    Sub ValidateFiles()
        If Not My.Computer.FileSystem.FileExists(AppFolder + FileNameConv(txtXMLfile.Text)) Then
            txtXMLfile.Text = ""
        End If
        If Not My.Computer.FileSystem.FileExists(AppFolder + FileNameConv(txtQRImage.Text)) Then
            txtQRImage.Text = ""
        End If
        'OLD MAP 20260407:
        'If Not My.Computer.FileSystem.FileExists(AppFolder + FileNameConv(txtGpxFile.Text)) Then
        '    txtGpxFile.Text = ""
        'End If
        Dim Missingfile As Boolean
        Missingfile = False
        'Dim files As String() = My.Settings.Videofilesfull.Split(ControlChars.CrLf.ToCharArray) ' Split laver dobb linjer
        ' For Each file As String In files
        'File = file.Replace(strQuote, "")
        'Missingfile = My.Computer.FileSystem.FileExists(file)
        'Next
        ' If Missingfile Then
        '.Settings.Videofilesfull = ""
        'txtVideolist.Text = ""
        'End If
    End Sub
    Sub CheckDirtyStatus()
        If bMakeMapDirty Then
            SetStatusLabel(IconStatusOutput, "NoVideo")
            SetStatusLabel(IconStatusPrepare, "NoImage") 'Sætter ikonet fordi det er fælles
            My.Settings.bAdjVideoIsReady = False
            bMakeMapDirty = False
            LblGPSDiff.Text = "0"
            lblMapLength.Text = "0:00:00"
            My.Settings.AdjVideoGPSDiff = LblGPSDiff.Text
        End If
        If bMakeVideoInDirty Then
            SetStatusLabel(IconStatusOutput, "NoVideo")
            SetStatusLabel(IconStatusPrepare, "NoImage") 'Sætter ikonet fordi det er fælles
            SetStatusLabel(iconStatusVideoIn, "NoVideo")
            lblVideoInLength.Text = "0:00:00"
            My.Settings.bAdjVideoIsReady = False
            bMakeVideoInDirty = False
            LblGPSDiff.Text = "0"
            My.Settings.AdjVideoGPSDiff = LblGPSDiff.Text
        End If
        If bOutputVideoDirty Then
            SetStatusLabel(IconStatusOutput, "NoVideo")
            lblOutputVideoLength.Text = "0:00:00"
            bOutputVideoDirty = False
        End If
    End Sub
    Sub StatusBarUpdate(Statustext As String)
        ' Marshal to UI thread if called from a background thread
        If Me.InvokeRequired Then
            Me.BeginInvoke(Sub() StatusBarUpdate(Statustext))
            Return
        End If

        LogStatus += Statustext + vbCrLf
        Statusbar.Text = Statustext

    End Sub
    Sub ClearProgressBar(Start As Boolean)
        EnsureInvoke(Sub()
                         StatusProgressBar1.Maximum = 20
                         StatusProgressBar1.Step = 1
                         StatusProgressBar1.Value = 0

                         If Start Then
                             StatusCount = 0
                             StatusProgressBar1.Visible = True
                             Timer2.Start()
                             StatusBarProgressText.Text = ""
                         Else
                             Timer2.Stop()
                             StatusProgressBar1.Visible = False
                         End If
                     End Sub)

    End Sub
    Sub ClearLogs()
        LogDeshake = ""
        LogJoin = ""
        LogMakeVideo = ""
        LogMapF = ""
        LogStatus = ""


    End Sub

    Function ReadyForOutputVideo() As Boolean
        If IconStatusPrepare.Tag.ToString = "OK" Then
            Return True
        Else
            Return False
        End If
    End Function
    Function FileNameConv(filename As String) As String
        Dim fn As String
        fn = filename.Replace(" ", "_")
        fn = fn.Replace("æ", "ae")
        fn = fn.Replace("ø", "o")
        fn = fn.Replace("å", "a")

        Return fn

    End Function

    Sub DeleteFileWildcard(folder As String, wildcardstr As String)
        Dim Dir As DirectoryInfo = New DirectoryInfo(folder)

        For Each Filei As FileInfo In Dir.EnumerateFiles(wildcardstr)
            Filei.Delete()
        Next

    End Sub

    'OLD MAP 20260407:
    'Private Sub btnGpxf_Click(sender As Object, e As EventArgs) Handles btnGpxf.Click
    '    Dim fn, fnx As String

    '    If OpenFileDialoggpx.ShowDialog() = DialogResult.OK Then
    '        fn = System.IO.Path.GetFileName(OpenFileDialoggpx.FileName)
    '        fnx = FileNameConv(fn)
    '        DeleteFileWildcard(AppFolder, "*.gpx")
    '        My.Computer.FileSystem.CopyFile(OpenFileDialoggpx.FileName, AppFolder + fnx, True)
    '        txtGpxFile.Text = fn
    '        inifile.SetINI(iniGPX, fnx)
    '        bMakeMapDirty = True
    '        CheckDirtyStatus()
    '    End If
    'End Sub

    Private Sub btnQRimg_Click(sender As Object, e As EventArgs) Handles btnQRimg.Click
        Dim fn, fnx As String

        If OpenFileDialogqrimg.ShowDialog() = DialogResult.OK Then
            fn = System.IO.Path.GetFileName(OpenFileDialogqrimg.FileName)
            fnx = FileNameConv(fn)
            DeleteFileWildcard(AppFolder, "*.jpg")
            My.Computer.FileSystem.CopyFile(OpenFileDialogqrimg.FileName, AppFolder + fn, True)
            txtQRImage.Text = fn
            My.Settings.QRimage = fn 'Set global 
            ' CLEANUP-CBNEWMAPF-20260407:
            'If Not My.Settings.cbNewMapF Then
            '    inifile.SetINI(iniQRIMG, fnx)
            '    bMakeMapDirty = True
            '    CheckDirtyStatus()
            'End If


        End If
    End Sub
    Private Sub btnQRimg2_Click(sender As Object, e As EventArgs) Handles btnQRimg2.Click
        Dim fn As String
        If OpenFileDialogqrimg.ShowDialog() = DialogResult.OK Then
            fn = System.IO.Path.GetFileName(OpenFileDialogqrimg.FileName)

            My.Computer.FileSystem.CopyFile(OpenFileDialogqrimg.FileName, AppFolder + fn, True)
            txtQRImage2.Text = fn
        End If
    End Sub

    Private Sub btnQRXML2_Click(sender As Object, e As EventArgs) Handles btnQRXML2.Click
        Dim fn As String
        If OpenFileDialogxml.ShowDialog() = DialogResult.OK Then
            fn = System.IO.Path.GetFileName(OpenFileDialogxml.FileName)
            My.Computer.FileSystem.CopyFile(OpenFileDialogxml.FileName, AppFolder + fn, True)
            txtXMLfile2.Text = fn
            Dim a As New clsTxtFiles
            If Not My.Computer.FileSystem.FileExists(AppFolder + fn) Then MsgBox(Texts.ErrNoFile + fn)

            time = a.FindTimeSpanQRXML(AppFolder + fn)

            My.Settings.QRXMLTimesec2 = CDec(time)

        End If
    End Sub

    Private Sub btnQRxml_Click(sender As Object, e As EventArgs) Handles btnQRxml.Click
        Dim fn, fnx, time As String

        If OpenFileDialogxml.ShowDialog() = DialogResult.OK Then
            fn = System.IO.Path.GetFileName(OpenFileDialogxml.FileName)
            fnx = FileNameConv(fn)
            DeleteFileWildcard(AppFolder, "*.xml")
            My.Computer.FileSystem.CopyFile(OpenFileDialogxml.FileName, AppFolder + fn, True)
            txtXMLfile.Text = fn
            My.Settings.QRXML = fn ' set global
            My.Settings.GPXDiff = "0" ' reset GPXdiff
            'OLD MAP 20260407:inifile.SetINI(iniQRXML, fnx)

            Dim a As New clsTxtFiles
            If Not My.Computer.FileSystem.FileExists(AppFolder + fnx) Then MsgBox(Texts.ErrNoFile + fnx)
            time = a.FindTimeSpanQRXML(AppFolder + fnx)

            My.Settings.QRXMLTimesec = CDec(time)
            lblMapLength.Text = ExtraFunc.SecToTimeStr(CDec(time))
            ' CLEANUP-CBNEWMAPF-START
            'If Not My.Settings.cbNewMapF Then
            '    bMakeMapDirty = True
            '    CheckDirtyStatus()
            'End If
        End If
    End Sub

    Private Sub AddVideofile_Click(sender As Object, e As EventArgs) Handles AddVideofile.Click

        Dim folder, fn As String, pos As Integer
        If OpenFileDialogVideo.ShowDialog() = DialogResult.OK Then
            Dim fileNamesArray() As String = OpenFileDialogVideo.FileNames
            Array.Sort(fileNamesArray)
            Dim containsSpaces As Boolean = False
            Dim ErrFileName As String = ""

            For Each filename As String In fileNamesArray
                If filename.Contains(" ") Then
                    containsSpaces = True
                    ErrFileName = filename
                    Exit For
                End If
            Next
            If containsSpaces Then
                MsgBox(Texts.ErrInpVideoSpace + ErrFileName)
            Else
                For Each File As String In fileNamesArray
                    If txtVideolist.Text <> "" Then
                        txtVideolist.Text += vbCrLf
                        My.Settings.Videofilesfull += vbCrLf
                    End If
                    pos = InStrRev(File, "\")
                    fn = Strings.Right(File, Len(File) - pos)
                    folder = Strings.Left(File, pos)

                    My.Settings.Videofilesfull += strQuote + File + strQuote
                    txtVideolist.Text += fn
                Next

                bMakeVideoInDirty = True
                CheckDirtyStatus()
            End If

        End If
    End Sub
    Private Sub btnClearVideolist_Click(sender As Object, e As EventArgs) Handles btnClearVideolist.Click
        txtVideolist.Text = ""
        My.Settings.Videofilesfull = ""
        bMakeVideoInDirty = True
        CheckDirtyStatus()
    End Sub

    Private Sub btnOutputFile_Click(sender As Object, e As EventArgs) Handles btnOutputFile.Click
        If My.Settings.MapFlipActive Then SaveFileDialogOutput.FileName = "FinalMapFlip.mp4"
        If SaveFileDialogOutput.ShowDialog() = DialogResult.OK Then
            My.Settings.Outputfile = SaveFileDialogOutput.FileName
            txtOutFilename.Text = SaveFileDialogOutput.FileName
            'inifile.SetINI(iniOutFile, """" + txtOutFilename.Text + """")
            bOutputVideoDirty = True
            'Update()

            If My.Settings.bDoMusic Then
                MsgBox(Texts.InfoFilenameMusic)
            End If
            CheckDirtyStatus()
        End If
    End Sub
    Public Sub UpdateProgressText(t As String)
        EnsureInvoke(Sub()
                         StatusBarProgressText.Text = t
                         RemainingTimeObj.SetGetRemainingTime(t)
                     End Sub)

    End Sub

    Private Sub EnsureInvoke(action As Action)
        If Me Is Nothing Then Return
        If Me.InvokeRequired Then
            Try
                Me.BeginInvoke(action)
            Catch
                ' Ignore if form closing
            End Try
        Else
            action()
        End If
    End Sub
    Public Sub UpdateTextBox(text As String, ProcessEnd As Boolean)

        If Not ProcessEnd Then

            Dim start As Integer = text.IndexOf("time")
            Dim [end] As Integer = text.IndexOf("bitrate")
            If start >= 0 And [end] > 0 Then
                Dim result As String = text.Substring(start, [end] - start + "bitrate".Length)
                result = result.Substring(5, 8) 'tidskode uden ms
                'StatusBarProgressText.Text = result
                MeVar.BeginInvoke(Sub() MeVar.UpdateProgressText(result))
            Else
                CurrentLog += text + vbCrLf
            End If

            'If CurrentLog.Length > 1000 Then CurrentLog = "..." + vbCrLf + Strings.Right(CurrentLog, 1000)
            'txtRun_CurrentOut.Text += text + vbCrLf
            'MeVar.txtRun_CurrentOut.Text = text + vbCrLf + MeVar.txtRun_CurrentOut.Text
            'MeVar.BeginInvoke(Sub() MeVar.txtRun_CurrentOut.Text = text)
            StatusCount += 1
            MeVar.BeginInvoke(Sub() MeVar.Update())
        Else

            ProcessFinished = True
        End If
    End Sub

    Private Function CheckFileWithMsg(filename As String, Optional appfolderin As String = "") As Boolean
        If appfolderin = "" Then appfolderin = AppFolder
        CheckFileWithMsg = My.Computer.FileSystem.FileExists(appfolderin + filename)
        If Not CheckFileWithMsg Then
            MsgBox(Texts.ErrNoFile + " " + appfolderin + filename)
        End If
    End Function
    'OLD MAP 20260407:
    'Private Function SetINIfilevalues() As Boolean
    '    'Videolist = txtVideolist.Text.Replace(vbCrLf, ",")
    '    'Videolist = My.Settings.Videofilesfull.Replace(vbCrLf, ",")
    '    'inifile.SetINI(iniVideo, Videolist)
    '    SetINIfilevalues = True
    '    If Not CheckFileWithMsg(FileNameConv(txtGpxFile.Text)) Then
    '        SetINIfilevalues = False
    '    ElseIf Not CheckFileWithMsg(FileNameConv(txtQRImage.Text)) Then
    '        SetINIfilevalues = False
    '    ElseIf Not CheckFileWithMsg(FileNameConv(txtXMLfile.Text)) Then
    '        SetINIfilevalues = False
    '    End If
    '    If SetINIfilevalues Then
    '        inifile.SetINI(iniGPX, FileNameConv(txtGpxFile.Text))
    '        inifile.SetINI(iniQRIMG, FileNameConv(txtQRImage.Text))
    '        inifile.SetINI(iniQRXML, FileNameConv(txtXMLfile.Text))
    '        inifile.SetINI(iniCropStart, (VideoCropStart * 1000).ToString)
    '        inifile.SetINI(iniCropEnd, (VideoCropEnd * 1000).ToString)
    '        'Kan nøjes med 1 pr sekund
    '        inifile.SetINI(iniFPS, "1")
    '    End If
    'End Function

    Function CalcFrames(fps As Integer, Optional TimeIn As String = "") As Integer
        Dim tmp, hour, min, sec, t As String, i As Integer, dhour, dmin, dsec, totsec As Single
        If TimeIn = "" Then
            t = CStr(My.Settings.QRXMLTimesec)
        Else
            t = TimeIn
        End If
        i = t.IndexOf(":")
        If i > 0 Then
            Try
                tmp = t.Substring(i + 1)
                hour = t.Substring(0, i)
                i = tmp.IndexOf(":")
                min = tmp.Substring(0, i)
                sec = tmp.Substring(i + 1)
                dhour = Convert.ToSingle(hour)
                dmin = Convert.ToSingle(min)
                dsec = Convert.ToSingle(sec)
                totsec = 3600 * dhour + 60 * dmin + dsec
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Else 'Kun sekunder ingen : i=-1
            totsec = t
        End If
        CalcFrames = CInt(totsec * fps)

    End Function


    Public Function Run_CommandX(exe As String, arg As String, ByRef Log As String) As Boolean
        Cursor = Cursors.WaitCursor
        CurrentLog = ""
        'proc.Run_Process(exe, arg)
        ProcessFinished = False

        ClearProgressBar(True)
        Log = Log + Texts.LogStartProcess + exe + " " + arg + vbCrLf
        If Not IsNothing(proc) Then proc = Nothing
        proc = New clsRunprocess(AddressOf UpdateTextBox)
        proc.WorkingDir = AppFolder
        proc.Run_Process2(exe, arg) 'TESTER
        'MsgBox("after run process2")
        'Cursor = Cursors.WaitCursor
        If proc.My_Process.HasExited Then
            'MsgBox("Proc exited before")
        End If
        Dim count As Integer = 0
        Dim p() As Process
        Do
            Thread.Sleep(200)
            Application.DoEvents()
            If count > 5 Then
                'p = Process.GetProcessesByName(proc.My_Process_Info.FileName)
                'If p.Count > 0 Then
                If proc.My_Process.HasExited Then


                    proc.ClearProcess()
                    ProcessFinished = True
                    Log = Log + "unexpected end of process - might work anyway"
                    ' Process is not running
                End If
            End If
            count += 1
        Loop While Not ProcessFinished
        proc.ClearProcess()

        ClearProgressBar(False)
        Log = Log + vbCrLf + CurrentLog + vbCrLf + "----END----" + vbCrLf

        'MsgBox("after procesfinish Runcommandx")

        Cursor = Cursors.Default
    End Function

    'CLEANUP20260407: Samme som Run_CommandX - kan nok nøjes med 1 funktion
    'Public Function Run_Command(cmd As String, Optional exefile As String = "A") As Boolean
    '    Dim file, cmdx As String
    '    CurrentLog = ""
    '    If exefile = "A" Then
    '        file = ExeF
    '    Else
    '        file = exefile
    '    End If
    '    ClearProgressBar(True)

    '    LogMapF += Texts.LogStartProcess + file + " " + cmd + vbCrLf
    '    If Not IsNothing(proc) Then proc = Nothing
    '    proc = New clsRunprocess(AddressOf UpdateTextBox)
    '    proc.WorkingDir = AppFolder
    '    proc.Run_Process2(file, cmd)
    '    'proc.Run_Process2(file, cmd + " > rgmapvideolog.txt")
    '    Cursor = Cursors.WaitCursor
    '    ProcessFinished = False

    '    Do
    '        Thread.Sleep(200)
    '        Application.DoEvents()
    '    Loop While (Not ProcessFinished) And (Not proc.My_Process.HasExited)
    '    'MsgBox("finished")
    '    proc.ClearProcess()
    '    ClearProgressBar(False)
    '    Select Case cmd
    '        Case cmd1Join
    '            LogJoin = LogJoin + CurrentLog
    '        Case cmd2Deshake
    '            LogDeshake = LogDeshake + CurrentLog
    '        Case cmd3Mapframes
    '            LogMapF = LogMapF + cmd3Mapframes + vbCrLf + LogMapF + CurrentLog + vbCrLf + "----END----" + vbCrLf
    '        Case cmd4MakeVideo
    '            LogMakeVideo = LogMakeVideo + CurrentLog
    '        Case Else
    '            LogJoin = LogJoin + CurrentLog
    '    End Select

    '    Cursor = Cursors.Default
    'End Function
    Public Function MakeMusicVideo() As Boolean
        If Not My.Computer.FileSystem.FileExists(My.Settings.AudioVideofile) Then
            MsgBox(Texts.ErrNoFile + My.Settings.AudioVideofile)
            MakeMusicVideo = False
        ElseIf Not My.Computer.FileSystem.FileExists(txtOutFilename.Text) Then
            MsgBox(Texts.ErrNoFile + My.Settings.AudioVideofile)
            MakeMusicVideo = False
        Else
            My.Settings.MusicFFMpegParam = My.Settings.MusicFFMpegParam.Replace("VIDEOFILE", txtOutFilename.Text)

            StatusBarUpdate(Texts.StatusMusicVidStart)
            LogMakeVideo += Texts.StatusMusicVidStart + vbCrLf
            MakeMusicVideo = Run_CommandX(FFMpegExe, My.Settings.MusicFFMpegParam, LogMakeVideo)
            StatusBarUpdate(Texts.StatusMusicVidFinish)
            LogMakeVideo += Texts.StatusMusicVidFinish + vbCrLf
        End If

    End Function


    Function RunPrepareVideo() As Boolean
        Dim bFailed As Boolean
        Dim joinstr, inpstr, tstr, strDuration As String
        Dim decDuration As Decimal
        Dim ProbeVideo As New clsFFMPegProbe
        bFailed = False
        If txtPrepareLength.Text <> "" Then
            tstr = " -t " + txtPrepareLength.Text
        Else
            tstr = ""
        End If
        If cbXJoin.Checked Then

            SetStatusLabel(iconStatusVideoIn, "Work")
            SetStatusLabel(IconStatusPrepare, "Work")
            Dim lines As String() = My.Settings.Videofilesfull.Split({vbCrLf}, StringSplitOptions.RemoveEmptyEntries)
            decDuration = 0
            For Each line As String In lines ' sum of input file durations
                strDuration = ExtraFunc.GetVideoFileDuration(line)
                decDuration += ExtraFunc.TimeStrToSec(strDuration)
            Next

            lblVideoInLength.Text = ""
            Videolist = My.Settings.Videofilesfull.Replace(vbCrLf, ",")
            StatusBarUpdate(Texts.Status2 + GetJoinedFilename(True))

            joinstr = ExtraFunc.FFMPeg_MakeJoinStr(Videolist, GetJoinedFilename(True), "25", "25", "faster", txtPrepareLength.Text)
            LogJoin += Texts.Status2 + GetJoinedFilename(True) + vbCrLf + joinstr + vbCrLf
            If Not txtPrepareLength.Text = "" Then
                decDuration = ExtraFunc.TimeStrToSec(txtPrepareLength.Text)
            End If
            RemainingTimeObj.StartTime(decDuration)
            EnsureInvoke(Sub() StatusRemaining.Text = "")
            TimerStatusRemaining.Start()
            Run_CommandX(FFMpegExe, joinstr, LogJoin)
            TimerStatusRemaining.Stop()
            EnsureInvoke(Sub() StatusRemaining.Text = "")
            If ProbeVideo.StoreVideoProps(GetJoinedFilename(False)) Then
                My.Settings.JoinFileLength = ProbeVideo.duration_sec
                My.Settings.JoinFileWidth = ProbeVideo.Width
                My.Settings.JoinFileHeight = ProbeVideo.Height

                InitVideoFormat()

            End If
            If Not cbXDeshake.Checked Then

                StatusBarUpdate(Texts.Status_copyfile)

                Thread.Sleep(200)
                Application.DoEvents()

                If My.Computer.FileSystem.FileExists(GetDeshakedFilename(False)) Then
                    My.Computer.FileSystem.DeleteFile(GetDeshakedFilename(False))
                End If
                My.Computer.FileSystem.RenameFile(GetJoinedFilename(False), DeshakeFilename)
                'MsgBox("Slut kopi")
                StatusBarUpdate(Texts.StatusVideo1Ready) 'Texts.Status_copyfile
                LogDeshake = Texts.LogCopyFile

            Else

                StatusBarUpdate(Texts.Status3)
                LogDeshake += Texts.Status3 + vbCrLf
                inpstr = "-i " + GetJoinedFilename(True)
                If Not My.Computer.FileSystem.FileExists(GetJoinedFilename(False)) Then
                    bFailed = True
                    StatusBarUpdate(Texts.ErrJoinedmissing + GetJoinedFilename(True))
                    SetStatusLabel(iconStatusVideoIn, "Fail")
                    SetStatusLabel(IconStatusPrepare, "Fail")
                    LogDeshake += Texts.ErrJoinedmissing + GetJoinedFilename(True) + vbCrLf
                Else
                    StatusRemaining.Text = ""

                    If rb_deshake.Checked Then

                        VidstabDetect = inpstr + " -vf " + My.Settings.ffmpegVidstabDetect + tstr + My.Settings.ffmpegVidstabDetectOut
                        VidstabTransform = inpstr + " -vf " + My.Settings.ffmpegVidstabTransform + ExtraFunc.FFMPeg_MakeOutputStr(GetDeshakedFilename(True), txtPrepareLength.Text, False, False)
                        StatusBarUpdate(Texts.Deshake1)
                        LogDeshake += Texts.Deshake1 + vbCrLf
                        RemainingTimeObj.StartTime(My.Settings.JoinFileLength)
                        EnsureInvoke(Sub() StatusRemaining.Text = "")
                        TimerStatusRemaining.Start()

                        Run_CommandX(FFMpegExe, VidstabDetect, LogDeshake)
                        TimerStatusRemaining.Stop()
                        EnsureInvoke(Sub() StatusRemaining.Text = "")
                        StatusBarUpdate(Texts.Deshake2 + GetDeshakedFilename(True)) '+ vbCrLf + VidstabTransform)
                        LogDeshake += Texts.Deshake2 + GetDeshakedFilename(True) + vbCrLf
                        RemainingTimeObj.StartTime(My.Settings.JoinFileLength)

                        TimerStatusRemaining.Start()
                        Run_CommandX(FFMpegExe, VidstabTransform, LogDeshake)
                        StatusBarUpdate(Texts.Deshake3)
                        TimerStatusRemaining.Stop()
                        StatusRemaining.Text = ""
                        LogDeshake += Texts.Deshake3 + vbCrLf

                    End If
                    'If rb_copy.Checked Then
                    'StatusBarUpdate ( Texts.Status_copyfile
                    'My.Computer.FileSystem.CopyFile(AppFolder + JoinedFilename, AppFolder + DeshakeFilename, True)
                    'LogDeshake = Texts.LogCopyFile
                    'End If
                    If RB_onlyfilter.Checked Then
                        VidstabTransform = inpstr + " " + My.Settings.No_deshake_filter + ExtraFunc.FFMPeg_MakeOutputStr(GetDeshakedFilename(True), txtPrepareLength.Text, False, False)
                        StatusBarUpdate(Texts.Filter1)
                        LogDeshake += Texts.Filter1 + vbCrLf
                        RemainingTimeObj.StartTime(My.Settings.JoinFileLength)
                        EnsureInvoke(Sub() StatusRemaining.Text = "")
                        TimerStatusRemaining.Start()
                        Run_CommandX(FFMpegExe, VidstabTransform, LogDeshake)
                        TimerStatusRemaining.Stop()
                        EnsureInvoke(Sub() StatusRemaining.Text = "")
                        StatusBarUpdate(Texts.Filter2)
                        LogDeshake += Texts.Filter2 + vbCrLf
                    End If

                End If

            End If

            If Not ProbeVideo.StoreVideoProps(GetDeshakedFilename(False)) Then
                StatusBarUpdate(Texts.ErrFailVideo1)
                LogDeshake += Texts.ErrFailVideo1 + vbCrLf
                bFailed = True
                SetStatusLabel(iconStatusVideoIn, "Fail")
                SetStatusLabel(IconStatusPrepare, "Fail")
            Else
                cbXJoin.Checked = False
                lblVideoInLength.Text = ExtraFunc.SecToTimeStr(ProbeVideo.duration_sec)
                SetStatusLabel(iconStatusVideoIn, "OK")
                SetStatusLabel(IconStatusPrepare, "OK")
            End If
        End If 'cbJoined
        Return bFailed
    End Function

    'OLD MAP 20260407:
    'Function RunMakeFrames() As Boolean
    '    Dim bFailed As Boolean

    '    If cbXMakeframes.Checked And Not My.Settings.bUseMapTrackingVideo Then ' Kortoverlay - kun hvis der ikke anvendes tracking video
    '        If Not My.Settings.bUseMapTrackingVideo Then
    '            SetStatusLabel(IconStatusMakeMap, "Work")
    '            SetStatusLabel(IconStatusPrepare, "Work")
    '            If SetINIfilevalues() Then
    '                My.Computer.FileSystem.CopyFile(AppFolder + IniFN, AppFolder + "tmpini.ini", True)
    '                StatusBarUpdate(Texts.Status1 + IniFN)
    '                LogMapF = Texts.Status1 + IniFN + vbCrLf
    '                StatusBarUpdate(Texts.Makeblack1)

    '                If txtPrepareLength.Text = "" Then
    '                    lblMapLength.Text = ExtraFunc.SecToTimeStr(My.Settings.QRXMLTimesec)
    '                    ExtraFunc.MakeBlackFrameFiles(CalcFrames(1)) ' 1 Frame pr sec
    '                    LogMapF += Texts.Makeblack1 + " " + lblMapLength.Text + vbCrLf
    '                Else
    '                    lblMapLength.Text = ExtraFunc.SecToTimeStr(txtPrepareLength.Text)
    '                    LogMapF += Texts.Makeblack1 + " " + txtPrepareLength.Text + vbCrLf
    '                    ExtraFunc.MakeBlackFrameFiles(CalcFrames(1, txtPrepareLength.Text))
    '                End If

    '                StatusBarUpdate(Texts.Makeblack2)
    '                LogMapF += Texts.Makeblack2 + vbCrLf
    '                StatusBarUpdate(Texts.Status4)
    '                LogMapF += Texts.Status4 + vbCrLf
    '                Run_Command(cmd3Mapframes)

    '                Dim dir As DirectoryInfo = New DirectoryInfo(AppFolder + "temp2\")
    '                If Not dir.Exists Then
    '                    MsgBox(Texts.ErrNoMapfolder)
    '                    StatusBarUpdate(Texts.ErrNoMapfolder)
    '                    SetStatusLabel(IconStatusMakeMap, "Fail")
    '                    SetStatusLabel(IconStatusPrepare, "Fail")
    '                    bFailed = True
    '                Else
    '                    If Not dir.GetFiles("*.jpg").Count > 0 Then
    '                        MsgBox(Texts.ErrNoMapfiles)
    '                        StatusBarUpdate(Texts.ErrNoMapfiles + GetStatusBarText())
    '                        SetStatusLabel(IconStatusMakeMap, "Fail")
    '                        SetStatusLabel(IconStatusPrepare, "Fail")
    '                        bFailed = True
    '                    Else
    '                        StatusBarUpdate(Texts.StatusCombine2)
    '                '                        If iconStatusVideoIn.Tag.ToString = "OK" Then
    '                            SetStatusLabel(IconStatusPrepare, "OK")
    '                        End If
    '                    End If
    '                End If
    '            Else
    '                SetStatusLabel(IconStatusMakeMap, "Fail")
    '                SetStatusLabel(IconStatusPrepare, "Fail")
    '                bFailed = True
    '            End If

    '        End If
    '    End If
    '    Return bFailed
    'End Function

    Private Function GetStatusBarText() As String
        If Me Is Nothing Then Return ""
        If Me.InvokeRequired Then
            Dim result As IAsyncResult = Me.BeginInvoke(New Func(Of String)(Function()
                                                                                Return Statusbar.Text
                                                                            End Function))
            Return CType(Me.EndInvoke(result), String)
        Else
            Return Statusbar.Text
        End If
    End Function
    Public Sub DeleteFolder(folderPath As String)
        If Directory.Exists(folderPath) Then
            Directory.Delete(folderPath, True)
        Else
            ' Handle if the folder does not exist '
        End If
    End Sub
    Private Sub TimerMapStatus_Tick(sender As Object, e As EventArgs) Handles TimerMapStatus.Tick
        If WriteToFiles.tasksCompleted Then WriteToFilesComplete = True
        MeVar.BeginInvoke(Sub() RemainingTimeObj.SetGetRemainingTime(WriteToFiles.get_ImageFileCounter))
        MeVar.BeginInvoke(Sub() MeVar.Update())
    End Sub

    Private Sub btnTestWriteVideo_Click(sender As Object, e As EventArgs) Handles btnTestWriteVideo.Click
        SaveFileDialogOutput.FileName = "testleg_web_420" & GetSmoothOverlayVideoExtension()
        If SaveFileDialogOutput.ShowDialog() <> DialogResult.OK Then Return
        Dim outputFile As String = SaveFileDialogOutput.FileName
        Dim outputDir As String = Path.GetDirectoryName(outputFile)
        If String.IsNullOrWhiteSpace(outputDir) Then outputDir = AppFolder
        Dim logFile As String = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(outputFile) & ".log")

        Dim options As New RenderLegOptions With {
            .OutputFile = SaveFileDialogOutput.FileName,
            .LogFile = logFile,
            .VideoWidth = 1920,
            .StartTime = 1380,
            .Duration = 60,
            .RenderVariant = "perf8",
            .StylePreset = "web",
            .AutoClose = False
        }
        options.Style.LegWidth = 940
        options.Style.LegHeight = 2140
        options.Style.DotSize = 16
        StartLegRender(options)
    End Sub

    Private Function ParseRenderCompareArgs() As RenderCompareOptions
        Dim args As String() = Environment.GetCommandLineArgs()
        If args Is Nothing OrElse args.Length <= 1 Then Return Nothing

        Dim hasRenderCompare As Boolean = False
        Dim options As New RenderCompareOptions With {
            .VideoWidth = ExtraFunc.InputWidth
        }

        For i As Integer = 1 To args.Length - 1
            Dim rawArg As String = args(i)
            If String.IsNullOrWhiteSpace(rawArg) Then Continue For
            Dim arg As String = rawArg.Trim()
            If arg.StartsWith("/") OrElse arg.StartsWith("-") Then arg = arg.Substring(1)
            Dim eqPos As Integer = arg.IndexOf("="c)
            Dim key As String = If(eqPos >= 0, arg.Substring(0, eqPos), arg)
            Dim value As String = If(eqPos >= 0, arg.Substring(eqPos + 1), "")
            key = key.Trim().ToLowerInvariant()
            value = value.Trim().Trim(""""c)

            Select Case key
                Case "rendercompare"
                    hasRenderCompare = True
                Case "variant1"
                    If value <> "" Then options.Variant1 = value.ToLowerInvariant()
                Case "variant2"
                    If value <> "" Then options.Variant2 = value.ToLowerInvariant()
                Case "start"
                    Dim parsed As Double
                    If Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, parsed) Then options.StartTime = parsed
                Case "duration"
                    Dim parsed As Double
                    If Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, parsed) Then options.Duration = parsed
                Case "width"
                    Dim parsed As Integer
                    If Integer.TryParse(value, parsed) AndAlso parsed > 0 Then options.VideoWidth = parsed
                Case "outfile", "output"
                    options.OutputFile = value
                Case "log", "logfile"
                    options.LogFile = value
                Case "autoclose", "exit"
                    If value = "" Then
                        options.AutoClose = True
                    Else
                        Dim parsed As Boolean
                        If Boolean.TryParse(value, parsed) Then options.AutoClose = parsed
                    End If
            End Select
        Next

        If Not hasRenderCompare Then Return Nothing

        If String.IsNullOrWhiteSpace(options.OutputFile) Then
            Dim baseDir As String = AppFolder
            Dim ext As String = GetSmoothOverlayVideoExtension()
            options.OutputFile = Path.Combine(baseDir, "autorender_perf8" & ext)
        End If
        If String.IsNullOrWhiteSpace(options.LogFile) Then
            Dim outDir As String = Path.GetDirectoryName(options.OutputFile)
            If String.IsNullOrWhiteSpace(outDir) Then outDir = AppFolder
            Dim outBaseName As String = Path.GetFileNameWithoutExtension(options.OutputFile)
            options.LogFile = Path.Combine(outDir, outBaseName & "_rendercompare.log")
        End If

        Return options
    End Function

    Private Function ResolveAutorunPath(pathValue As String) As String
        If String.IsNullOrWhiteSpace(pathValue) Then Return ""
        If Path.IsPathRooted(pathValue) Then Return pathValue
        Return Path.Combine(AppFolder, pathValue)
    End Function

    Private Function TryParseColorArgument(value As String, ByRef parsedColor As Color) As Boolean
        If String.IsNullOrWhiteSpace(value) Then Return False
        Dim trimmed As String = value.Trim()

        Try
            If trimmed.StartsWith("#") Then
                parsedColor = ColorTranslator.FromHtml(trimmed)
                Return True
            End If

            If trimmed.StartsWith("0x", StringComparison.OrdinalIgnoreCase) Then
                Dim argb As Integer
                If Integer.TryParse(trimmed.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, argb) Then
                    parsedColor = Color.FromArgb(argb)
                    Return True
                End If
            End If

            Dim namedColor As Color = Color.FromName(trimmed)
            If namedColor.IsKnownColor OrElse namedColor.IsNamedColor Then
                parsedColor = namedColor
                Return True
            End If
        Catch
        End Try

        Return False
    End Function

    Private Function ParseRenderLegArgs() As RenderLegOptions
        Dim args As String() = Environment.GetCommandLineArgs()
        If args Is Nothing OrElse args.Length <= 1 Then Return Nothing

        Dim hasRenderLeg As Boolean = False
        Dim options As New RenderLegOptions With {
            .VideoWidth = ExtraFunc.InputWidth
        }

        For i As Integer = 1 To args.Length - 1
            Dim rawArg As String = args(i)
            If String.IsNullOrWhiteSpace(rawArg) Then Continue For
            Dim arg As String = rawArg.Trim()
            If arg.StartsWith("/") OrElse arg.StartsWith("-") Then arg = arg.Substring(1)
            Dim eqPos As Integer = arg.IndexOf("="c)
            Dim key As String = If(eqPos >= 0, arg.Substring(0, eqPos), arg)
            Dim value As String = If(eqPos >= 0, arg.Substring(eqPos + 1), "")
            key = key.Trim().ToLowerInvariant()
            value = value.Trim().Trim(""""c)

            Select Case key
                Case "renderleg"
                    hasRenderLeg = True
                Case "variant"
                    If value <> "" Then options.RenderVariant = value.ToLowerInvariant()
                Case "stylepreset", "style"
                    options.StylePreset = value.ToLowerInvariant()
                Case "start"
                    Dim parsed As Double
                    If Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, parsed) Then options.StartTime = parsed
                Case "duration"
                    Dim parsed As Double
                    If Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, parsed) Then options.Duration = parsed
                Case "width"
                    Dim parsed As Integer
                    If Integer.TryParse(value, parsed) AndAlso parsed > 0 Then options.VideoWidth = parsed
                Case "legwidth"
                    Dim parsed As Integer
                    If Integer.TryParse(value, parsed) AndAlso parsed > 0 Then options.Style.LegWidth = parsed
                Case "legheight"
                    Dim parsed As Integer
                    If Integer.TryParse(value, parsed) AndAlso parsed > 0 Then options.Style.LegHeight = parsed
                Case "outfile", "output"
                    options.OutputFile = value
                Case "log", "logfile"
                    options.LogFile = value
                Case "autoclose", "exit"
                    If value = "" Then
                        options.AutoClose = True
                    Else
                        Dim parsed As Boolean
                        If Boolean.TryParse(value, parsed) Then options.AutoClose = parsed
                    End If
                Case "xml", "qrxml"
                    options.XmlPath = ResolveAutorunPath(value)
                Case "image", "qrimage", "map"
                    options.ImagePath = ResolveAutorunPath(value)
                Case "framestep"
                    Dim parsed As Double
                    If Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, parsed) AndAlso parsed > 0 Then options.FrameStepSeconds = parsed
                Case "fps", "outputfps"
                    Dim parsed As Double
                    If Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, parsed) AndAlso parsed > 0 Then options.OutputFps = parsed
                Case "tailduration"
                    Dim parsed As Integer
                    If Integer.TryParse(value, parsed) AndAlso parsed >= 0 Then options.Style.TailDurationSeconds = parsed
                Case "dotsize"
                    Dim parsed As Integer
                    If Integer.TryParse(value, parsed) AndAlso parsed > 0 Then options.Style.DotSize = parsed
                Case "tailratio"
                    Dim parsed As Double
                    If Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, parsed) AndAlso parsed >= 0 Then options.Style.DotTailRatio = parsed
                Case "arrowbarb"
                    Dim parsed As Double
                    If Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, parsed) AndAlso parsed >= 0 Then options.Style.ArrowBarb = parsed
                Case "arrowwidth"
                    Dim parsed As Double
                    If Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, parsed) AndAlso parsed >= 0 Then options.Style.ArrowWidth = parsed
                Case "dottype"
                    options.Style.DotType = value
                Case "framefeather"
                    Dim parsed As Boolean
                    If Boolean.TryParse(value, parsed) Then options.Style.FrameFeather = parsed
                Case "tailcolor"
                    Dim parsedColor As Color
                    If TryParseColorArgument(value, parsedColor) Then options.Style.TailColorArgb = parsedColor.ToArgb()
                Case "dotcolor"
                    Dim parsedColor As Color
                    If TryParseColorArgument(value, parsedColor) Then options.Style.DotColorArgb = parsedColor.ToArgb()
                Case "tailalpha"
                    Dim parsed As Integer
                    If Integer.TryParse(value, parsed) Then options.Style.TailAlpha = Math.Max(0, Math.Min(255, parsed))
                Case "speedcolor", "speedcoloring"
                    Dim parsed As Boolean
                    If Boolean.TryParse(value, parsed) Then options.Style.SpeedColoringEnabled = parsed
                Case "pacefast", "pacefastsec"
                    Dim parsed As Double
                    If Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, parsed) AndAlso parsed > 0 Then options.Style.PaceFastSecondsPerKm = parsed
                Case "paceslow", "paceslowsec"
                    Dim parsed As Double
                    If Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, parsed) AndAlso parsed > 0 Then options.Style.PaceSlowSecondsPerKm = parsed
                Case "tailticks", "ticks"
                    Dim parsed As Boolean
                    If Boolean.TryParse(value, parsed) Then options.Style.TailTicksEnabled = parsed
                Case "tickinterval"
                    Dim parsed As Double
                    If Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, parsed) AndAlso parsed > 0 Then options.Style.TailTickIntervalSeconds = parsed
                Case "tickalpha"
                    Dim parsed As Integer
                    If Integer.TryParse(value, parsed) Then options.Style.TailTickAlpha = Math.Max(0, Math.Min(255, parsed))
                Case "tickcolor"
                    Dim parsedColor As Color
                    If TryParseColorArgument(value, parsedColor) Then options.Style.TailTickColorArgb = parsedColor.ToArgb()
            End Select
        Next

        If Not hasRenderLeg Then Return Nothing

        If String.IsNullOrWhiteSpace(options.OutputFile) Then
            Dim baseDir As String = AppFolder
            Dim ext As String = GetSmoothOverlayVideoExtension()
            options.OutputFile = Path.Combine(baseDir, "autorender_leg_" & options.RenderVariant & ext)
        End If
        If String.IsNullOrWhiteSpace(options.LogFile) Then
            Dim outDir As String = Path.GetDirectoryName(options.OutputFile)
            If String.IsNullOrWhiteSpace(outDir) Then outDir = AppFolder
            Dim outBaseName As String = Path.GetFileNameWithoutExtension(options.OutputFile)
            options.LogFile = Path.Combine(outDir, outBaseName & ".log")
        End If

        ApplyRenderStylePreset(options)

        Return options
    End Function

    Private Sub ApplyRenderStylePreset(options As RenderLegOptions)
        If options Is Nothing OrElse String.IsNullOrWhiteSpace(options.StylePreset) Then Return

        Select Case options.StylePreset.Trim().ToLowerInvariant()
            Case "classic"
                If String.IsNullOrWhiteSpace(options.Style.DotType) Then options.Style.DotType = "Arrow"
                If Not options.Style.DotSize.HasValue Then options.Style.DotSize = 28
                If Not options.Style.DotTailRatio.HasValue Then options.Style.DotTailRatio = 0.55
                If Not options.Style.ArrowWidth.HasValue Then options.Style.ArrowWidth = 0.8
                If Not options.Style.ArrowBarb.HasValue Then options.Style.ArrowBarb = 0.22
            Case "web", "webstyle"
                If String.IsNullOrWhiteSpace(options.Style.DotType) Then options.Style.DotType = "Arrow"
                If Not options.Style.DotSize.HasValue Then options.Style.DotSize = 28
                If Not options.Style.DotTailRatio.HasValue Then options.Style.DotTailRatio = 0.55
                If Not options.Style.ArrowWidth.HasValue Then options.Style.ArrowWidth = 0.8
                If Not options.Style.ArrowBarb.HasValue Then options.Style.ArrowBarb = 0.22
                If Not options.Style.DotColorArgb.HasValue Then options.Style.DotColorArgb = Color.FromArgb(255, 140, 40).ToArgb()
                If Not options.Style.TailAlpha.HasValue Then options.Style.TailAlpha = 95
                If Not options.Style.SpeedColoringEnabled.HasValue Then options.Style.SpeedColoringEnabled = True
                If Not options.Style.PaceFastSecondsPerKm.HasValue Then options.Style.PaceFastSecondsPerKm = 270
                If Not options.Style.PaceSlowSecondsPerKm.HasValue Then options.Style.PaceSlowSecondsPerKm = 780
                If Not options.Style.TailTicksEnabled.HasValue Then options.Style.TailTicksEnabled = True
                If Not options.Style.TailTickIntervalSeconds.HasValue Then options.Style.TailTickIntervalSeconds = 2
                If Not options.Style.TailTickAlpha.HasValue Then options.Style.TailTickAlpha = 70
                If Not options.Style.TailTickColorArgb.HasValue Then options.Style.TailTickColorArgb = Color.FromArgb(150, 150, 150).ToArgb()
        End Select
    End Sub

    Private Sub ApplyRenderStyleOverrides(renderEngine As clsMapRenderEngine, style As RenderStyleOverrides)
        If renderEngine Is Nothing OrElse style Is Nothing Then Return

        If style.LegWidth.HasValue Then renderEngine.LegWidth = style.LegWidth.Value
        If style.LegHeight.HasValue Then renderEngine.LegHeight = style.LegHeight.Value
        If style.TailDurationSeconds.HasValue Then renderEngine.TailLineDurationSeconds = style.TailDurationSeconds.Value
        If style.DotSize.HasValue Then renderEngine.DotSize = style.DotSize.Value
        If style.DotTailRatio.HasValue Then renderEngine.DotTailRatio = style.DotTailRatio.Value
        If style.ArrowBarb.HasValue Then renderEngine.ArrowBarb = style.ArrowBarb.Value
        If style.ArrowWidth.HasValue Then renderEngine.ArrowWidth = style.ArrowWidth.Value
        If Not String.IsNullOrWhiteSpace(style.DotType) Then renderEngine.DotType = style.DotType
        If style.FrameFeather.HasValue Then renderEngine.FrameFeather = style.FrameFeather.Value
        If style.TailColorArgb.HasValue Then renderEngine.TailLineColor = Color.FromArgb(style.TailColorArgb.Value)
        If style.DotColorArgb.HasValue Then renderEngine.DotColor = Color.FromArgb(style.DotColorArgb.Value)
        If style.TailAlpha.HasValue Then renderEngine.TailAlpha = style.TailAlpha.Value
        If style.SpeedColoringEnabled.HasValue Then renderEngine.SpeedColoringEnabled = style.SpeedColoringEnabled.Value
        If style.PaceFastSecondsPerKm.HasValue Then renderEngine.PaceFastSecondsPerKm = style.PaceFastSecondsPerKm.Value
        If style.PaceSlowSecondsPerKm.HasValue Then renderEngine.PaceSlowSecondsPerKm = style.PaceSlowSecondsPerKm.Value
        If style.TailTicksEnabled.HasValue Then renderEngine.TailTicksEnabled = style.TailTicksEnabled.Value
        If style.TailTickIntervalSeconds.HasValue Then renderEngine.TailTickIntervalSeconds = style.TailTickIntervalSeconds.Value
        If style.TailTickAlpha.HasValue Then renderEngine.TailTickAlpha = style.TailTickAlpha.Value
        If style.TailTickColorArgb.HasValue Then renderEngine.TailTickColor = Color.FromArgb(style.TailTickColorArgb.Value)
    End Sub

    Private Function GetSavedLegRenderLayout() As String
        Dim layout As String = My.Settings.MILegRenderLayout
        If String.IsNullOrWhiteSpace(layout) Then Return "classic"
        Return layout.Trim().ToLowerInvariant()
    End Function

    Private Function PaceMinutesPerKmToSeconds(minutesPerKm As Double) As Double
        Return Math.Max(1.0, minutesPerKm * 60.0)
    End Function

    Private Function TailTransparencyToAlpha(transparency As Double) As Integer
        Dim clamped As Double = Math.Max(0.0, Math.Min(1.0, transparency))
        Return CInt(Math.Round((1.0 - clamped) * 255.0))
    End Function

    Public Function IsNewLegLayoutEnabled() As Boolean
        Select Case GetSavedLegRenderLayout()
            Case "web", "webstyle", "new"
                Return True
            Case Else
                Return False
        End Select
    End Function

    Public Sub ApplySavedLegRenderLayout(renderEngine As clsMapRenderEngine)
        If renderEngine Is Nothing Then Return

        Dim style As New RenderStyleOverrides()
        If IsNewLegLayoutEnabled() Then
            style.TailAlpha = TailTransparencyToAlpha(My.Settings.MITailTransparency)
            style.SpeedColoringEnabled = My.Settings.MITailUseSpeedColors
            style.PaceFastSecondsPerKm = PaceMinutesPerKmToSeconds(Math.Min(My.Settings.MIPaceFastMinPerKm, My.Settings.MIPaceSlowMinPerKm))
            style.PaceSlowSecondsPerKm = PaceMinutesPerKmToSeconds(Math.Max(My.Settings.MIPaceFastMinPerKm, My.Settings.MIPaceSlowMinPerKm))
            style.TailTicksEnabled = True
            style.TailTickIntervalSeconds = 5
            style.TailTickAlpha = 110
            style.TailTickColorArgb = Color.FromArgb(65, 65, 65).ToArgb()
            renderEngine.ArrowOutlineScale = Math.Max(1, My.Settings.MIArrowOutlineScale)
            renderEngine.TailTickWidth = Math.Max(1.0F, 1.1F + CSng(My.Settings.MIArrowOutlineScale - 1) * 0.22F)
        Else
            style.TailAlpha = 255
            style.SpeedColoringEnabled = False
            style.TailTicksEnabled = False
            renderEngine.ArrowOutlineScale = 1
            renderEngine.TailTickWidth = 1.0F
        End If

        ApplyRenderStyleOverrides(renderEngine, style)
    End Sub

    Private Function BuildScaledRoutePoints(source As clsQRRoutePoints, scale As Double) As clsQRRoutePoints
        Dim scaled As New clsQRRoutePoints()
        scaled.QRLogoYOffset = source.QRLogoYOffset * scale

        For Each rp As clsQRRoutePoint In source.RoutePoints
            Dim copy As New clsQRRoutePoint(rp)
            copy.ImageX *= scale
            copy.ImageY *= scale
            scaled.AddPoint(copy)
        Next

        If scaled.RoutePoints.Count > 0 Then
            scaled.EndAddPoints()
        End If

        Return scaled
    End Function

    Private Function BuildScaledBitmap(source As Bitmap, scale As Double) As Bitmap
        Dim width As Integer = Math.Max(1, CInt(Math.Round(source.Width * scale)))
        Dim height As Integer = Math.Max(1, CInt(Math.Round(source.Height * scale)))
        Dim scaled As New Bitmap(width, height)
        Using g As Graphics = Graphics.FromImage(scaled)
            g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            g.DrawImage(source, New Rectangle(0, 0, width, height))
        End Using
        Return scaled
    End Function

    Private Function RunLegRenderVariant(routePoints As clsQRRoutePoints,
                                         mapImg As Bitmap,
                                         renderVariant As String,
                                         outputFile As String,
                                         startTime As Double,
                                         duration As Double,
                                         videoWidth As Integer,
                                         Optional style As RenderStyleOverrides = Nothing,
                                         Optional frameStepSeconds As Double = -1,
                                         Optional outputFps As Double = -1) As clsMapRenderEngine
        Dim variantKey As String = renderVariant.Trim().ToLowerInvariant()
        Dim useHalfScale As Boolean = False

        If variantKey.StartsWith("half") Then
            useHalfScale = True
            variantKey = variantKey.Substring(4).TrimStart("-"c, "_"c)
            If variantKey = "" Then variantKey = "perf"
        End If

        Dim variantRoutePoints As clsQRRoutePoints = routePoints
        Dim variantMapImg As Bitmap = mapImg
        If useHalfScale Then
            variantRoutePoints = BuildScaledRoutePoints(routePoints, 0.5)
            variantMapImg = BuildScaledBitmap(mapImg, 0.5)
        End If

        Try
            Dim renderEngine As New clsMapRenderEngine(variantRoutePoints, variantMapImg)
            renderEngine.FrameStepSeconds = GetSmoothOverlayFrameStepSeconds()
            If frameStepSeconds > 0 Then renderEngine.FrameStepSeconds = frameStepSeconds
            renderEngine.ScalePixelSettings(videoWidth)
            ApplyRenderStyleOverrides(renderEngine, style)

            Select Case variantKey
                Case "base"
                    renderEngine.WriteLegVideo(outputFile, startTime:=startTime, duration:=duration, videoWidth:=videoWidth, frameStepSeconds:=frameStepSeconds, outputFps:=outputFps)
                Case "perf"
                    renderEngine.WriteLegVideoPerf(outputFile, startTime:=startTime, duration:=duration, videoWidth:=videoWidth, frameStepSeconds:=frameStepSeconds, outputFps:=outputFps)
                Case "perf3"
                    renderEngine.WriteLegVideoPerf3(outputFile, startTime:=startTime, duration:=duration, videoWidth:=videoWidth, frameStepSeconds:=frameStepSeconds, outputFps:=outputFps)
                Case "perf8"
                    renderEngine.WriteLegVideoPerf8(outputFile, startTime:=startTime, duration:=duration, videoWidth:=videoWidth, frameStepSeconds:=frameStepSeconds, outputFps:=outputFps)
                Case "selective"
                    renderEngine.WriteLegVideoSelective(outputFile, startTime:=startTime, duration:=duration, videoWidth:=videoWidth, frameStepSeconds:=frameStepSeconds, outputFps:=outputFps)
                Case Else
                    Throw New ArgumentException("Unknown render variant: " & renderVariant)
            End Select

            Return renderEngine
        Finally
            If useHalfScale Then
                variantMapImg.Dispose()
            End If
        End Try
    End Function

    Private Sub StartLegRenderComparison(options As RenderCompareOptions)
        If options Is Nothing Then Return
        If String.IsNullOrWhiteSpace(My.Settings.QRXML) Or String.IsNullOrWhiteSpace(My.Settings.QRimage) Then
            Throw New InvalidOperationException("QuickRoute XML/image settings are not configured.")
        End If

        Dim qrXmlPath As String = AppFolder + My.Settings.QRXML
        Dim qrImagePath As String = AppFolder + My.Settings.QRimage
        If Not File.Exists(qrXmlPath) Then Throw New FileNotFoundException("QR XML not found.", qrXmlPath)
        If Not File.Exists(qrImagePath) Then Throw New FileNotFoundException("QR image not found.", qrImagePath)

        btnTestWriteVideo.Enabled = False
        StatusBarUpdate("Starting test leg performance comparison...")
        SetStatusLabel(IconStatusOutput, "Work", "Outputvideo")

        Dim RPs As New clsQRRoutePoints
        Dim reader As New clsQRXMLReader(RPs)
        reader.ReadXML(qrXmlPath)
        Dim mapImg As Bitmap = New Bitmap(Image.FromFile(qrImagePath))

        Task.Run(Sub()
                     Try
                         Dim maxStartTime As Double = Math.Max(0, RPs.RoutePoints.Count - 11)
                         Dim testStartTime As Double = Math.Min(maxStartTime, Math.Max(0, options.StartTime))
                         Dim outDir As String = Path.GetDirectoryName(options.OutputFile)
                         If String.IsNullOrEmpty(outDir) Then outDir = "."
                         Dim outBaseName As String = Path.GetFileNameWithoutExtension(options.OutputFile)
                         Dim outExt As String = GetSmoothOverlayVideoExtension()
                         Dim variant1File As String = Path.Combine(outDir, outBaseName & "_" & options.Variant1 & outExt)
                         Dim variant2File As String = Path.Combine(outDir, outBaseName & "_" & options.Variant2 & outExt)

                         Dim renderEngine1 As clsMapRenderEngine = RunLegRenderVariant(RPs, mapImg, options.Variant1, variant1File, testStartTime, options.Duration, options.VideoWidth)
                         Dim variant1Total As Double = renderEngine1.LastTotalElapsed.TotalSeconds
                         Dim variant1Render As Double = renderEngine1.LastRenderElapsed.TotalSeconds
                         Dim variant1Encode As Double = renderEngine1.LastEncodeElapsed.TotalSeconds
                         Dim variant1Frames As Integer = renderEngine1.LastFrameCount

                         Dim renderEngine2 As clsMapRenderEngine = RunLegRenderVariant(RPs, mapImg, options.Variant2, variant2File, testStartTime, options.Duration, options.VideoWidth)
                         mapImg.Dispose()

                         Dim timingText As String = String.Format(CultureInfo.InvariantCulture,
                            "Test leg performance comparison finished. Start {0:0}s. {1} total {2:0.0}s render {3:0.0}s encode {4:0.0}s frames {5}. {6} total {7:0.0}s render {8:0.0}s encode {9:0.0}s frames {10}.",
                            testStartTime,
                            options.Variant1.Substring(0, 1).ToUpperInvariant() & options.Variant1.Substring(1),
                            variant1Total,
                            variant1Render,
                            variant1Encode,
                            variant1Frames,
                            options.Variant2.Substring(0, 1).ToUpperInvariant() & options.Variant2.Substring(1),
                            renderEngine2.LastTotalElapsed.TotalSeconds,
                            renderEngine2.LastRenderElapsed.TotalSeconds,
                            renderEngine2.LastEncodeElapsed.TotalSeconds,
                            renderEngine2.LastFrameCount)

                         Dim logLines As String =
                             "Render compare" & vbCrLf &
                             "Start=" & testStartTime.ToString(CultureInfo.InvariantCulture) & vbCrLf &
                             "Duration=" & options.Duration.ToString(CultureInfo.InvariantCulture) & vbCrLf &
                             "Width=" & options.VideoWidth.ToString(CultureInfo.InvariantCulture) & vbCrLf &
                             "Variant1=" & options.Variant1 & vbCrLf &
                             "Variant2=" & options.Variant2 & vbCrLf &
                             timingText & vbCrLf
                         File.WriteAllText(options.LogFile, logLines)

                         Me.BeginInvoke(Sub()
                                            StatusBarUpdate(timingText)
                                            LogMapF += timingText + vbCrLf
                                            LogMapF += "Render compare log: " & options.LogFile & vbCrLf
                                            SetStatusLabel(IconStatusOutput, "OK", "Outputvideo")
                                            btnTestWriteVideo.Enabled = True
                                            If options.AutoClose Then
                                                Close()
                                            End If
                                        End Sub)
                     Catch ex As Exception
                         Me.BeginInvoke(Sub()
                                            Dim errorText As String = "Error during test write: " & ex.ToString()
                                            Try
                                                If options IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(options.LogFile) Then
                                                    File.WriteAllText(options.LogFile, errorText)
                                                End If
                                            Catch
                                            End Try
                                            MsgBox(errorText)
                                            StatusBarUpdate("Error: " & ex.Message)
                                            SetStatusLabel(IconStatusOutput, "Fail", "Outputvideo")
                                            btnTestWriteVideo.Enabled = True
                                            If options IsNot Nothing AndAlso options.AutoClose Then
                                                Close()
                                            End If
                                        End Sub)
                     End Try
                 End Sub)
    End Sub

    Private Sub StartLegRender(options As RenderLegOptions)
        If options Is Nothing Then Return

        ' Ensure style presets are applied for both CLI and UI-triggered renders.
        ApplyRenderStylePreset(options)

        Dim qrXmlPath As String = options.XmlPath
        If String.IsNullOrWhiteSpace(qrXmlPath) Then
            If String.IsNullOrWhiteSpace(My.Settings.QRXML) Then Throw New InvalidOperationException("QuickRoute XML setting is not configured.")
            qrXmlPath = AppFolder + My.Settings.QRXML
        End If

        Dim qrImagePath As String = options.ImagePath
        If String.IsNullOrWhiteSpace(qrImagePath) Then
            If String.IsNullOrWhiteSpace(My.Settings.QRimage) Then Throw New InvalidOperationException("QuickRoute image setting is not configured.")
            qrImagePath = AppFolder + My.Settings.QRimage
        End If

        If Not File.Exists(qrXmlPath) Then Throw New FileNotFoundException("QR XML not found.", qrXmlPath)
        If Not File.Exists(qrImagePath) Then Throw New FileNotFoundException("QR image not found.", qrImagePath)

        btnTestWriteVideo.Enabled = False
        StatusBarUpdate("Starting leg render...")
        SetStatusLabel(IconStatusOutput, "Work", "Outputvideo")

        Task.Run(Sub()
                     Dim mapImg As Bitmap = Nothing
                     Try
                         Dim RPs As New clsQRRoutePoints
                         Dim reader As New clsQRXMLReader(RPs)
                         reader.ReadXML(qrXmlPath)
                         mapImg = New Bitmap(Image.FromFile(qrImagePath))

                         Dim maxStartTime As Double = Math.Max(0, RPs.RoutePoints.Count - 11)
                         Dim renderStartTime As Double = Math.Min(maxStartTime, Math.Max(0, options.StartTime))
                         Dim renderEngine As clsMapRenderEngine = RunLegRenderVariant(RPs,
                                                                                      mapImg,
                                                                                      options.RenderVariant,
                                                                                      options.OutputFile,
                                                                                      renderStartTime,
                                                                                      options.Duration,
                                                                                      options.VideoWidth,
                                                                                      options.Style,
                                                                                      options.FrameStepSeconds,
                                                                                      options.OutputFps)
                         mapImg.Dispose()
                         mapImg = Nothing
                         Dim effectiveFrameStep As Double = If(options.FrameStepSeconds > 0, options.FrameStepSeconds, GetSmoothOverlayFrameStepSeconds())
                         Dim effectiveOutputFps As Double = If(options.OutputFps > 0, options.OutputFps, 1.0 / effectiveFrameStep)

                         Dim summary As String = String.Format(CultureInfo.InvariantCulture,
                                                               "Leg render finished. Variant {0}. Start {1:0}s duration {2:0.###}s width {3}. Total {4:0.0}s render {5:0.0}s encode {6:0.0}s frames {7}.",
                                                               options.RenderVariant,
                                                               renderStartTime,
                                                               options.Duration,
                                                               options.VideoWidth,
                                                               renderEngine.LastTotalElapsed.TotalSeconds,
                                                               renderEngine.LastRenderElapsed.TotalSeconds,
                                                               renderEngine.LastEncodeElapsed.TotalSeconds,
                                                               renderEngine.LastFrameCount)

                         Dim styleLines As New List(Of String)
                         Dim rawArgs As String() = Environment.GetCommandLineArgs()
                         If rawArgs IsNot Nothing AndAlso rawArgs.Length > 1 Then
                             styleLines.Add("Args=" & String.Join(" | ", rawArgs.Skip(1)))
                         End If
                         If options.Style IsNot Nothing Then
                             If options.Style.LegWidth.HasValue Then styleLines.Add("LegWidth=" & options.Style.LegWidth.Value.ToString(CultureInfo.InvariantCulture))
                             If options.Style.LegHeight.HasValue Then styleLines.Add("LegHeight=" & options.Style.LegHeight.Value.ToString(CultureInfo.InvariantCulture))
                             If options.Style.TailDurationSeconds.HasValue Then styleLines.Add("TailDuration=" & options.Style.TailDurationSeconds.Value.ToString(CultureInfo.InvariantCulture))
                             If options.Style.DotSize.HasValue Then styleLines.Add("DotSize=" & options.Style.DotSize.Value.ToString(CultureInfo.InvariantCulture))
                             If options.Style.DotTailRatio.HasValue Then styleLines.Add("TailRatio=" & options.Style.DotTailRatio.Value.ToString(CultureInfo.InvariantCulture))
                             If options.Style.ArrowBarb.HasValue Then styleLines.Add("ArrowBarb=" & options.Style.ArrowBarb.Value.ToString(CultureInfo.InvariantCulture))
                             If options.Style.ArrowWidth.HasValue Then styleLines.Add("ArrowWidth=" & options.Style.ArrowWidth.Value.ToString(CultureInfo.InvariantCulture))
                             If Not String.IsNullOrWhiteSpace(options.Style.DotType) Then styleLines.Add("DotType=" & options.Style.DotType)
                             If options.Style.FrameFeather.HasValue Then styleLines.Add("FrameFeather=" & options.Style.FrameFeather.Value.ToString())
                             If options.Style.TailColorArgb.HasValue Then styleLines.Add("TailColor=" & Color.FromArgb(options.Style.TailColorArgb.Value).Name)
                             If options.Style.DotColorArgb.HasValue Then styleLines.Add("DotColor=" & Color.FromArgb(options.Style.DotColorArgb.Value).Name)
                             If options.Style.TailAlpha.HasValue Then styleLines.Add("TailAlpha=" & options.Style.TailAlpha.Value.ToString(CultureInfo.InvariantCulture))
                             If options.Style.SpeedColoringEnabled.HasValue Then styleLines.Add("SpeedColoring=" & options.Style.SpeedColoringEnabled.Value.ToString())
                             If options.Style.PaceFastSecondsPerKm.HasValue Then styleLines.Add("PaceFast=" & options.Style.PaceFastSecondsPerKm.Value.ToString(CultureInfo.InvariantCulture))
                             If options.Style.PaceSlowSecondsPerKm.HasValue Then styleLines.Add("PaceSlow=" & options.Style.PaceSlowSecondsPerKm.Value.ToString(CultureInfo.InvariantCulture))
                             If options.Style.TailTicksEnabled.HasValue Then styleLines.Add("TailTicks=" & options.Style.TailTicksEnabled.Value.ToString())
                             If options.Style.TailTickIntervalSeconds.HasValue Then styleLines.Add("TickInterval=" & options.Style.TailTickIntervalSeconds.Value.ToString(CultureInfo.InvariantCulture))
                             If options.Style.TailTickAlpha.HasValue Then styleLines.Add("TickAlpha=" & options.Style.TailTickAlpha.Value.ToString(CultureInfo.InvariantCulture))
                             If options.Style.TailTickColorArgb.HasValue Then styleLines.Add("TickColor=" & Color.FromArgb(options.Style.TailTickColorArgb.Value).Name)
                         End If

                         Dim logLines As String =
                             "Render leg" & vbCrLf &
                             "Variant=" & options.RenderVariant & vbCrLf &
                             "StylePreset=" & options.StylePreset & vbCrLf &
                             "Start=" & renderStartTime.ToString(CultureInfo.InvariantCulture) & vbCrLf &
                             "Duration=" & options.Duration.ToString(CultureInfo.InvariantCulture) & vbCrLf &
                             "Width=" & options.VideoWidth.ToString(CultureInfo.InvariantCulture) & vbCrLf &
                             "FrameStep=" & effectiveFrameStep.ToString(CultureInfo.InvariantCulture) & vbCrLf &
                             "OutputFps=" & effectiveOutputFps.ToString(CultureInfo.InvariantCulture) & vbCrLf &
                             "Xml=" & qrXmlPath & vbCrLf &
                             "Image=" & qrImagePath & vbCrLf

                         If styleLines.Count > 0 Then
                             logLines &= "Style=" & String.Join("; ", styleLines) & vbCrLf
                         End If
                         logLines &= summary & vbCrLf
                         File.WriteAllText(options.LogFile, logLines)

                         Me.BeginInvoke(Sub()
                                            StatusBarUpdate(summary)
                                            LogMapF += summary & vbCrLf
                                            LogMapF += "Render leg log: " & options.LogFile & vbCrLf
                                            SetStatusLabel(IconStatusOutput, "OK", "Outputvideo")
                                            btnTestWriteVideo.Enabled = True
                                            If options.AutoClose Then
                                                Close()
                                            End If
                                        End Sub)
                     Catch ex As Exception
                         If mapImg IsNot Nothing Then mapImg.Dispose()
                         Me.BeginInvoke(Sub()
                                            Dim errorText As String = "Error during leg render: " & ex.ToString()
                                            Try
                                                If options IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(options.LogFile) Then
                                                    File.WriteAllText(options.LogFile, errorText)
                                                End If
                                            Catch
                                            End Try
                                            MsgBox(errorText)
                                            StatusBarUpdate("Error: " & ex.Message)
                                            SetStatusLabel(IconStatusOutput, "Fail", "Outputvideo")
                                            btnTestWriteVideo.Enabled = True
                                            If options IsNot Nothing AndAlso options.AutoClose Then
                                                Close()
                                            End If
                                        End Sub)
                     End Try
                 End Sub)
    End Sub

    Private Sub btnMakeMapN_Click(sender As Object, e As EventArgs) Handles btnMakeMapN.Click
        StatusBarUpdate(Texts.StatusMakeMapImgs)


        MakeMapFiles()
        StatusBarUpdate(Texts.StatusCombine2)
    End Sub
    Private Function GetMapOverlayDurationSeconds() As Integer
        Dim duration As Integer
        If txtOutputLength.Text = "" Then
            duration = -1
        Else
            If My.Settings.GPXDiff = "" Then My.Settings.GPXDiff = "0"
            duration = ExtraFunc.TimeStrToSec(txtOutputLength.Text) + Math.Abs(CInt(My.Settings.GPXDiff))
            If numVideoTempo.Value <> 1 Then
                duration = CInt(duration * numVideoTempo.Value)
            End If
        End If
        Return duration
    End Function
    Private Function GetSmoothOverlayFrameStepSeconds() As Double
        Dim frameStep As Double = My.Settings.MISmoothFrameStepSeconds
        If frameStep <= 0 Then
            frameStep = 0.25
            My.Settings.MISmoothFrameStepSeconds = frameStep
        End If
        Return frameStep
    End Function
    Private Function GetSmoothOverlayVideoExtension() As String
        If My.Settings.MIFrameFeather Then Return ".mov"
        Return ".mp4"
    End Function
    Private Function IsDanishUi() As Boolean
        Return Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.Equals("da", StringComparison.OrdinalIgnoreCase)
    End Function
    Private Function GetMapVideoProgressText() As String
        If IsDanishUi() Then Return "Genererer kortvideoer..."
        Return "Generating map videos..."
    End Function
    Private Function GetMapVideoStartLogText() As String
        If IsDanishUi() Then Return "Genererer kortvideoer"
        Return "Generating map videos"
    End Function
    Private Function GetMapVideoFailedLogText() As String
        If IsDanishUi() Then Return "Generering af kortvideoer mislykkedes"
        Return "Map video generation failed"
    End Function
    Private Function GetMapVideoGeneratedLogText(folderName As String) As String
        If IsDanishUi() Then Return "Kortvideoer genereret i " & folderName
        Return "Map videos generated in " & folderName
    End Function
    Private Function GetMapVideoTimingHeaderText() As String
        If IsDanishUi() Then Return "Tider for kortvideoer:"
        Return "Map video timing:"
    End Function
    Private Function GetMapVideoTimingLabelTotal() As String
        If IsDanishUi() Then Return "  Samlet genereringstid: "
        Return "  Total asset generation: "
    End Function
    Private Function GetMapVideoTimingLabelRenderLoop() As String
        If IsDanishUi() Then Return "  Samlet rendertid: "
        Return "  Render loop: "
    End Function
    Private Function GetMapVideoTimingLabelBaseMap() As String
        If IsDanishUi() Then Return "  Basiskort-tegning: "
        Return "  Base map draw: "
    End Function
    Private Function GetMapVideoTimingLabelBackground() As String
        If IsDanishUi() Then Return "  Baggrundsudsnit: "
        Return "  Background crop: "
    End Function
    Private Function GetMapVideoTimingLabelOverlayMap() As String
        If IsDanishUi() Then Return "  Overlaymap-tegning: "
        Return "  Overlay map draw: "
    End Function
    Private Function GetMapVideoTimingLabelOverlayFrame() As String
        If IsDanishUi() Then Return "  Overlay-crop/rotate: "
        Return "  Overlay crop/rotate: "
    End Function
    Private Function GetMapVideoTimingLabelDraw() As String
        If IsDanishUi() Then Return "  Frame-tegning: "
        Return "  Frame draw: "
    End Function
    Private Function GetMapVideoTimingLabelStreamWrite() As String
        If IsDanishUi() Then Return "  Stream-skrivning: "
        Return "  Stream write: "
    End Function
    Private Function GetMapVideoTimingLabelZoomRender() As String
        If IsDanishUi() Then Return "  Zoom-rendering: "
        Return "  Zoom render: "
    End Function
    Private Function GetMapVideoTimingLabelLegRender() As String
        If IsDanishUi() Then Return "  Leg-rendering: "
        Return "  Leg render: "
    End Function
    Private Function GetMapVideoTimingLabelZoomEncode() As String
        If IsDanishUi() Then Return "  Zoom-kodning: "
        Return "  Zoom encode process: "
    End Function
    Private Function GetMapVideoTimingLabelLegEncode() As String
        If IsDanishUi() Then Return "  Leg-kodning: "
        Return "  Leg encode process: "
    End Function
    Private Function GetMapVideoTimingNoteText() As String
        If IsDanishUi() Then Return "  Bemærk: kodningstider overlapper med rendering."
        Return "  Note: encode times overlap with rendering."
    End Function
    Private Function GetSmoothOverlayUseParallelGeneration() As Boolean
        Return My.Settings.MISmoothParallelGeneration
    End Function
    Private Function MakeSmoothMapVideoAssets() As Boolean
        Dim RPs As New clsQRRoutePoints
        Dim mapImg As Bitmap
        Dim reader As New clsQRXMLReader(RPs)
        Dim totalStopwatch As Diagnostics.Stopwatch = Diagnostics.Stopwatch.StartNew()

        AppFolder = My.Application.Info.DirectoryPath + "\"
        reader.ReadXML(AppFolder + My.Settings.QRXML)
        mapImg = New Bitmap(Image.FromFile(My.Settings.QRimage))

        Dim smoothFolder As String = Path.Combine(AppFolder, SmoothOverlayFolderName)
        If Directory.Exists(smoothFolder) Then
            Directory.Delete(smoothFolder, True)
        End If
        Directory.CreateDirectory(smoothFolder)

        Dim duration As Integer = GetMapOverlayDurationSeconds()
        Dim smoothBaseName As String = Path.GetFileNameWithoutExtension(SmoothOverlayBaseName)
        Dim smoothOverlayExtension As String = GetSmoothOverlayVideoExtension()
        Dim zoomVideo As String = Path.Combine(smoothFolder, smoothBaseName & "_z" & smoothOverlayExtension)
        Dim legVideo As String = Path.Combine(smoothFolder, smoothBaseName & "_l" & smoothOverlayExtension)
        Dim zoomRenderElapsed As TimeSpan = TimeSpan.Zero
        Dim zoomEncodeElapsed As TimeSpan = TimeSpan.Zero
        Dim legRenderElapsed As TimeSpan = TimeSpan.Zero
        Dim legEncodeElapsed As TimeSpan = TimeSpan.Zero
        Dim zoomDrawElapsed As TimeSpan = TimeSpan.Zero
        Dim zoomStreamWriteElapsed As TimeSpan = TimeSpan.Zero
        Dim legDrawElapsed As TimeSpan = TimeSpan.Zero
        Dim legStreamWriteElapsed As TimeSpan = TimeSpan.Zero
        Dim zoomBackgroundElapsed As TimeSpan = TimeSpan.Zero
        Dim zoomOverlayMapElapsed As TimeSpan = TimeSpan.Zero
        Dim zoomOverlayFrameElapsed As TimeSpan = TimeSpan.Zero
        Dim legBackgroundElapsed As TimeSpan = TimeSpan.Zero
        Dim legOverlayMapElapsed As TimeSpan = TimeSpan.Zero
        Dim legOverlayFrameElapsed As TimeSpan = TimeSpan.Zero
        Dim frameCount As Integer = 0
        Dim frameStepSeconds As Double = GetSmoothOverlayFrameStepSeconds()
        Dim useParallelGeneration As Boolean = GetSmoothOverlayUseParallelGeneration()
        Dim totalVideoCount As Integer = 0
        If My.Settings.cbShowRoute Then totalVideoCount += 1
        If My.Settings.cbShowLegMAp Then totalVideoCount += 1
        Dim timelineLength As Double = If(duration > 0, duration, Math.Max(0, RPs.RoutePoints.Count - 1))
        Dim framesPerVideo As Integer = CInt(Math.Floor(timelineLength / frameStepSeconds + 0.0001)) + 1
        Dim totalFramesPlanned As Integer = Math.Max(1, framesPerVideo)
        Dim zoomEngine As clsMapRenderEngine = Nothing
        Dim legEngine As clsMapRenderEngine = Nothing

        Try
            ' Active smooth asset path now uses the render-engine implementation.
            If My.Settings.cbShowRoute Then
                zoomEngine = New clsMapRenderEngine(RPs, mapImg)
                zoomEngine.FrameStepSeconds = frameStepSeconds
                ApplySavedLegRenderLayout(zoomEngine)
            End If
            If My.Settings.cbShowLegMAp Then
                legEngine = New clsMapRenderEngine(RPs, mapImg)
                legEngine.FrameStepSeconds = frameStepSeconds
                ApplySavedLegRenderLayout(legEngine)
            End If
            ClearProgressBar(True)
            EnsureInvoke(Sub()
                             Timer2.Stop()
                             StatusRemaining.Text = ""
                             StatusBarProgressText.Text = ""
                             StatusProgressBar1.Maximum = 100
                             StatusProgressBar1.Value = 0
                         End Sub)
            RemainingTimeObj.StartTime(totalFramesPlanned)
            RemainingTimeObj.ExtraRemainingSeconds = CDec(GetStoredOverlayEstimateSeconds(ExtraFunc.InputWidth))
            Dim progressSource As clsMapRenderEngine = If(legEngine, zoomEngine)
            If progressSource IsNot Nothing Then
                progressSource.ProgressCallback =
                    Sub(doneFrames As Integer, totalFrames As Integer, phaseFile As String)
                        Dim safeDone As Integer = Math.Min(totalFramesPlanned, doneFrames)
                        Dim overlayEstimateSeconds As Double = GetStoredOverlayEstimateSeconds(ExtraFunc.InputWidth)
                        RemainingTimeObj.ExtraRemainingSeconds = CDec(overlayEstimateSeconds)
                        RemainingTimeObj.SetGetRemainingTime(CDec(safeDone))
                        EnsureInvoke(Sub()
                                         StatusRemaining.Text = RemainingTimeObj.sPercent & " " & Texts.StatusTimeLeft & RemainingTimeObj.sRemainingTime
                                         If overlayEstimateSeconds > 0 Then
                                             StatusBarProgressText.Text = GetTotalEstimateStatusText(TryParseRemainingTimeSeconds(RemainingTimeObj.sTotalRemainingTime))
                                         Else
                                             StatusBarProgressText.Text = ""
                                         End If
                                         Dim progressValue As Integer = CInt(Math.Round((safeDone / Math.Max(1.0, totalFramesPlanned)) * StatusProgressBar1.Maximum))
                                         StatusProgressBar1.Value = Math.Max(StatusProgressBar1.Minimum, Math.Min(StatusProgressBar1.Maximum, progressValue))
                                     End Sub)
                        Application.DoEvents()
                    End Sub
            End If

            If legEngine IsNot Nothing Then
                legEngine.ProgressCallback = progressSource.ProgressCallback
            End If

            If useParallelGeneration Then
                Dim zoomTask As Threading.Tasks.Task = Nothing
                Dim legTask As Threading.Tasks.Task = Nothing

                If zoomEngine IsNot Nothing Then
                    zoomTask = Threading.Tasks.Task.Run(
                        Sub()
                            zoomEngine.WriteZoomVideoPerf8(zoomVideo,
                                                           startTime:=0,
                                                           duration:=duration,
                                                           videoWidth:=ExtraFunc.InputWidth)
                        End Sub)
                End If

                If legEngine IsNot Nothing Then
                    legTask = Threading.Tasks.Task.Run(
                        Sub()
                            legEngine.WriteLegVideoPerf8(legVideo,
                                                        startTime:=0,
                                                        duration:=duration,
                                                        videoWidth:=ExtraFunc.InputWidth)
                        End Sub)
                End If

                Dim activeTasks As New List(Of Threading.Tasks.Task)
                If zoomTask IsNot Nothing Then activeTasks.Add(zoomTask)
                If legTask IsNot Nothing Then activeTasks.Add(legTask)
                If activeTasks.Count > 0 Then
                    While activeTasks.Any(Function(t) Not t.IsCompleted)
                        Application.DoEvents()
                        Threading.Thread.Sleep(50)
                    End While
                    Threading.Tasks.Task.WaitAll(activeTasks.ToArray())
                End If
            Else
                If zoomEngine IsNot Nothing Then
                    zoomEngine.WriteZoomVideoPerf8(zoomVideo,
                                                   startTime:=0,
                                                   duration:=duration,
                                                   videoWidth:=ExtraFunc.InputWidth)
                End If
                If legEngine IsNot Nothing Then
                    legEngine.WriteLegVideoPerf8(legVideo,
                                                startTime:=0,
                                                duration:=duration,
                                                videoWidth:=ExtraFunc.InputWidth)
                End If
            End If

            If zoomEngine IsNot Nothing Then
                zoomBackgroundElapsed = zoomEngine.LastBackgroundElapsed
                zoomOverlayMapElapsed = zoomEngine.LastOverlayMapElapsed
                zoomOverlayFrameElapsed = zoomEngine.LastOverlayFrameElapsed
                zoomDrawElapsed = zoomEngine.LastDrawElapsed
                zoomStreamWriteElapsed = zoomEngine.LastStreamWriteElapsed
                zoomRenderElapsed = zoomEngine.LastRenderElapsed
                zoomEncodeElapsed = zoomEngine.LastEncodeElapsed
                frameCount = Math.Max(frameCount, zoomEngine.LastFrameCount)
            End If

            If legEngine IsNot Nothing Then
                legBackgroundElapsed = legEngine.LastBackgroundElapsed
                legOverlayMapElapsed = legEngine.LastOverlayMapElapsed
                legOverlayFrameElapsed = legEngine.LastOverlayFrameElapsed
                legDrawElapsed = legEngine.LastDrawElapsed
                legStreamWriteElapsed = legEngine.LastStreamWriteElapsed
                legRenderElapsed = legEngine.LastRenderElapsed
                legEncodeElapsed = legEngine.LastEncodeElapsed
                frameCount = Math.Max(frameCount, legEngine.LastFrameCount)
            End If
        Finally
            mapImg.Dispose()
            totalStopwatch.Stop()
            LastSmoothAssetElapsed = totalStopwatch.Elapsed
            LastSmoothRenderElapsed = zoomRenderElapsed + legRenderElapsed
            LastSmoothZoomEncodeElapsed = zoomEncodeElapsed
            LastSmoothLegEncodeElapsed = legEncodeElapsed
            LastSmoothFrameCount = frameCount
            LastSmoothBaseMapElapsed = TimeSpan.Zero
            LastSmoothBackgroundElapsed = zoomBackgroundElapsed + legBackgroundElapsed
            LastSmoothOverlayMapElapsed = zoomOverlayMapElapsed + legOverlayMapElapsed
            LastSmoothOverlayFrameElapsed = zoomOverlayFrameElapsed + legOverlayFrameElapsed
            LastSmoothDrawElapsed = zoomDrawElapsed + legDrawElapsed
            LastSmoothStreamWriteElapsed = zoomStreamWriteElapsed + legStreamWriteElapsed
            LastSmoothZoomRenderElapsed = zoomRenderElapsed
            LastSmoothLegRenderElapsed = legRenderElapsed
            If zoomEngine IsNot Nothing Then zoomEngine.ProgressCallback = Nothing
            If legEngine IsNot Nothing Then legEngine.ProgressCallback = Nothing
            EnsureInvoke(Sub()
                             StatusRemaining.Text = ""
                             StatusBarProgressText.Text = ""
                         End Sub)
            ClearProgressBar(False)
        End Try

        Return ((Not My.Settings.cbShowRoute OrElse File.Exists(zoomVideo)) AndAlso
                (Not My.Settings.cbShowLegMAp OrElse File.Exists(legVideo)))
    End Function
    Private Function GetSmoothOverlayZoomVideoPath() As String
        Return Path.Combine(AppFolder, SmoothOverlayFolderName, Path.GetFileNameWithoutExtension(SmoothOverlayBaseName) & "_z" & GetSmoothOverlayVideoExtension())
    End Function
    Private Function GetSmoothOverlayLegVideoPath() As String
        Return Path.Combine(AppFolder, SmoothOverlayFolderName, Path.GetFileNameWithoutExtension(SmoothOverlayBaseName) & "_l" & GetSmoothOverlayVideoExtension())
    End Function
    Private Function FormatElapsed(elapsed As TimeSpan) As String
        Return elapsed.TotalSeconds.ToString("0.0", CultureInfo.InvariantCulture) + " s"
    End Function
    Private Function FormatShortRemaining(seconds As Double) As String
        Dim safeSeconds As Double = Math.Max(0, seconds)
        Dim ts As TimeSpan = TimeSpan.FromSeconds(safeSeconds)
        Return ts.ToString("hh\:mm\:ss")
    End Function
    Private Function GetOverlayEstimateBucketKey(videoWidth As Integer) As String
        If videoWidth <= 1920 Then Return "hd"
        If videoWidth <= 2560 Then Return "2k"
        Return "4k"
    End Function
    Private Function GetStoredOverlayEstimateSeconds(videoWidth As Integer) As Double
        Select Case GetOverlayEstimateBucketKey(videoWidth)
            Case "hd"
                Return Math.Max(0, My.Settings.OverlayEstimateHdSeconds)
            Case "2k"
                Return Math.Max(0, My.Settings.OverlayEstimate2KSeconds)
            Case Else
                Return Math.Max(0, My.Settings.OverlayEstimate4KSeconds)
        End Select
    End Function
    Private Sub StoreOverlayEstimateSeconds(videoWidth As Integer, elapsed As TimeSpan)
        Dim seconds As Double = Math.Max(0, elapsed.TotalSeconds)
        Select Case GetOverlayEstimateBucketKey(videoWidth)
            Case "hd"
                My.Settings.OverlayEstimateHdSeconds = seconds
            Case "2k"
                My.Settings.OverlayEstimate2KSeconds = seconds
            Case Else
                My.Settings.OverlayEstimate4KSeconds = seconds
        End Select
    End Sub
    Private Function GetTotalEstimateStatusText(totalSeconds As Double) As String
        If totalSeconds <= 0 Then Return ""
        If IsDanishUi() Then Return "Samlet est.: " & FormatShortRemaining(totalSeconds)
        Return "Total est.: " & FormatShortRemaining(totalSeconds)
    End Function
    Private Function TryParseRemainingTimeSeconds(value As String) As Double
        Dim parsed As TimeSpan
        If TimeSpan.TryParseExact(value, "hh\:mm\:ss", CultureInfo.InvariantCulture, parsed) Then
            Return Math.Max(0, parsed.TotalSeconds)
        End If
        Return 0
    End Function
    Private Function GetWholeVideoTimingHeaderText() As String
        If IsDanishUi() Then Return "Samlede videotider:"
        Return "Whole video timing:"
    End Function
    Private Function GetWholeVideoTimingMapLabelText() As String
        If IsDanishUi() Then Return "  Kortdel: "
        Return "  Map stage: "
    End Function
    Private Function GetWholeVideoTimingOverlayLabelText() As String
        If IsDanishUi() Then Return "  Videooverlay-del: "
        Return "  Video overlay stage: "
    End Function
    Private Function GetWholeVideoTimingTotalLabelText() As String
        If IsDanishUi() Then Return "  Samlet tid: "
        Return "  Total time: "
    End Function
    Private Sub AppendWholeVideoTimingLog()
        LogMakeVideo += GetWholeVideoTimingHeaderText() + vbCrLf
        LogMakeVideo += GetWholeVideoTimingMapLabelText() + FormatElapsed(LastWholeMapStageElapsed) + vbCrLf
        LogMakeVideo += GetWholeVideoTimingOverlayLabelText() + FormatElapsed(LastWholeOverlayStageElapsed) + vbCrLf
        LogMakeVideo += GetWholeVideoTimingTotalLabelText() + FormatElapsed(LastWholeVideoTotalElapsed) + vbCrLf
    End Sub
    Private Sub AppendSmoothTimingLog()
        LogMapF += GetMapVideoTimingHeaderText() + vbCrLf
        LogMapF += GetMapVideoTimingLabelTotal() + FormatElapsed(LastSmoothAssetElapsed) + vbCrLf
        LogMapF += GetMapVideoTimingLabelRenderLoop() + FormatElapsed(LastSmoothRenderElapsed) + " (" + LastSmoothFrameCount.ToString(CultureInfo.InvariantCulture) + " frames)" + vbCrLf
        LogMapF += GetMapVideoTimingLabelBaseMap() + FormatElapsed(LastSmoothBaseMapElapsed) + vbCrLf
        LogMapF += GetMapVideoTimingLabelBackground() + FormatElapsed(LastSmoothBackgroundElapsed) + vbCrLf
        LogMapF += GetMapVideoTimingLabelOverlayMap() + FormatElapsed(LastSmoothOverlayMapElapsed) + vbCrLf
        LogMapF += GetMapVideoTimingLabelOverlayFrame() + FormatElapsed(LastSmoothOverlayFrameElapsed) + vbCrLf
        LogMapF += GetMapVideoTimingLabelDraw() + FormatElapsed(LastSmoothDrawElapsed) + vbCrLf
        LogMapF += GetMapVideoTimingLabelStreamWrite() + FormatElapsed(LastSmoothStreamWriteElapsed) + vbCrLf
        LogMapF += GetMapVideoTimingLabelZoomRender() + FormatElapsed(LastSmoothZoomRenderElapsed) + vbCrLf
        LogMapF += GetMapVideoTimingLabelLegRender() + FormatElapsed(LastSmoothLegRenderElapsed) + vbCrLf
        LogMapF += GetMapVideoTimingLabelZoomEncode() + FormatElapsed(LastSmoothZoomEncodeElapsed) + vbCrLf
        LogMapF += GetMapVideoTimingLabelLegEncode() + FormatElapsed(LastSmoothLegEncodeElapsed) + vbCrLf
        LogMapF += GetMapVideoTimingNoteText() + vbCrLf
    End Sub
    Function MakeMapFiles() As Boolean
        Dim RPs As New clsQRRoutePoints
        Dim MapImg As Image
        Dim a As New clsQRXMLReader(RPs)
        AppFolder = My.Application.Info.DirectoryPath + "\"

        a.ReadXML(AppFolder + My.Settings.QRXML)
        MapImg = New Bitmap(Image.FromFile(My.Settings.QRimage))
        ' Legacy PNG sequence generation kept here only as reference/fallback.
        'ClearProgressBar(True)
        'StatusRemaining.Text = ""
        'DeleteFolder(AppFolder + "temp3")
        'Directory.CreateDirectory(AppFolder + "temp3")
        'WriteToFiles = New clMapsImgsToFiles(RPs, MapImg)
        'Dim Duration As Integer = GetMapOverlayDurationSeconds()
        'RemainingTimeObj.StartTime(WriteToFiles.GetTotalFiles(0, Duration))
        'StatusRemaining.Text = ""
        'StatusBarProgressText.Text = ""
        'TimerMapStatus.Start()
        'WriteToFilesComplete = False
        'TimerStatusRemaining.Start()
        'WriteToFiles.WriteImgsToFiles(6, 0, Duration, ExtraFunc.InputWidth) ' scale
        'Dim i As Integer
        'While Not WriteToFilesComplete
        '    Thread.Sleep(500)
        '    Application.DoEvents()
        '    i += 1
        '    If i > 4 Then
        '        StatusCount += 1
        '        i = 0
        '    End If
        'End While
        'TimerMapStatus.Stop()
        'TimerStatusRemaining.Stop()
        'StatusRemaining.Text = ""
        'ClearProgressBar(False)

        StatusBarUpdate(GetMapVideoProgressText())
        LogMapF += GetMapVideoStartLogText() + vbCrLf
        If Not MakeSmoothMapVideoAssets() Then
            LogMapF += GetMapVideoFailedLogText() + vbCrLf
        Else
            LogMapF += GetMapVideoGeneratedLogText(SmoothOverlayFolderName) + vbCrLf
            AppendSmoothTimingLog()
        End If
        Return True
    End Function
    Function RunMakeVideo() As Boolean
        Dim bFailed As Boolean
        Dim strDuration As String
        Dim decDuration As Integer
        Dim ProbeVideo As New clsFFMPegProbe
        Dim tmpGPXDiff As Integer
        Dim totalStopwatch As System.Diagnostics.Stopwatch = System.Diagnostics.Stopwatch.StartNew()
        Dim mapStageStopwatch As New System.Diagnostics.Stopwatch()
        Dim overlayStageStopwatch As New System.Diagnostics.Stopwatch()
        bFailed = False
        LastWholeMapStageElapsed = TimeSpan.Zero
        LastWholeOverlayStageElapsed = TimeSpan.Zero
        LastWholeVideoTotalElapsed = TimeSpan.Zero
        StatusBarUpdate(Texts.Status5)
        If Not My.Computer.FileSystem.FileExists(GetDeshakedFilename(False)) Then
            MsgBox(Texts.ErrDeshakemissing + GetDeshakedFilename(True))
        Else
            SetStatusLabel(IconStatusOutput, "Work")
            ' CLEANUP-CBNEWMAPF-START: If My.Settings.cbNewMapF And
            If Not My.Settings.bUseMapTrackingVideo Then 'Generate the new map images
                StatusBarUpdate(Texts.StatusMakeMapImgs)
                Me.Update()

                If Not cbOnlyVideo.Checked Then
                    mapStageStopwatch.Start()
                    MakeMapFiles()
                    mapStageStopwatch.Stop()
                    LastWholeMapStageElapsed = mapStageStopwatch.Elapsed
                End If
                StatusBarUpdate(Texts.StatusCombine2)
            End If
            Dim arg As String

            If My.Settings.bUseMapTrackingVideo Then

                arg = ExtraFunc.FFMPeg_MakeParamMapOnVideo(txtOutFilename.Text, My.Settings.TrackMapVideoFilename, GetDeshakedFilename(True), LblGPSDiff.Text, txtOutputLength.Text, numVideoTempo.Value, False, True, My.Settings.txtRealtimeFactor)
            Else
                ' CLEANUP-CBNEWMAPF-START:If My.Settings.cbNewMapF Then
                If Not IsNumeric(My.Settings.GPXDiff) Then My.Settings.GPXDiff = "0"
                tmpGPXDiff = CInt(My.Settings.GPXDiff)
                If My.Settings.MapFlipActive Then tmpGPXDiff -= CInt(My.Settings.MapFlipStartS) ' add startpoint for map 2
                Dim smoothZoomFile As String = GetSmoothOverlayZoomVideoPath()
                Dim smoothLegFile As String = GetSmoothOverlayLegVideoPath()
                If (Not My.Settings.cbShowRoute OrElse File.Exists(smoothZoomFile)) AndAlso
                       (Not My.Settings.cbShowLegMAp OrElse File.Exists(smoothLegFile)) Then
                    arg = ExtraFunc.FFMPeg_MakeParamSmoothMapVideosOnVideo(txtOutFilename.Text, smoothZoomFile, smoothLegFile, GetDeshakedFilename(True), CStr(tmpGPXDiff), txtOutputLength.Text, numVideoTempo.Value)
                Else
                    ' Legacy temp3 PNG overlay fallback:
                    'arg = ExtraFunc.FFMPeg_MakeParamMapOnVideo(txtOutFilename.Text, "temp3\%08d.png", GetDeshakedFilename(True), CStr(tmpGPXDiff), txtOutputLength.Text, numVideoTempo.Value)
                    Throw New FileNotFoundException("Smooth overlay video assets are missing.")
                End If
                ' CLEANUP-CBNEWMAPF-START:Else
                'arg = ExtraFunc.FFMPeg_MakeParamMapOnVideo(txtOutFilename.Text, "temp2\%08d.jpg", GetDeshakedFilename(True), LblGPSDiff.Text, txtOutputLength.Text, numVideoTempo.Value)
                'End If
            End If
            LogMakeVideo += arg + vbCrLf
            StatusBarUpdate(Texts.Status5)
            If String.IsNullOrWhiteSpace(txtOutputLength.Text) OrElse txtOutputLength.Text = "0" Then
                strDuration = ExtraFunc.GetVideoFileDuration(GetDeshakedFilename(True))
                decDuration = ExtraFunc.TimeStrToSec(strDuration)
            Else
                strDuration = txtOutputLength.Text
                decDuration = ExtraFunc.TimeStrToSec(strDuration)
            End If
            RemainingTimeObj.StartTime(decDuration)
            EnsureInvoke(Sub() StatusRemaining.Text = "")
            TimerStatusRemaining.Start()
            overlayStageStopwatch.Start()
            Run_CommandX(FFMpegExe, arg, LogMakeVideo)
            overlayStageStopwatch.Stop()
            LastWholeOverlayStageElapsed = overlayStageStopwatch.Elapsed
            StoreOverlayEstimateSeconds(ExtraFunc.InputWidth, LastWholeOverlayStageElapsed)
            My.Settings.Save()
            TimerStatusRemaining.Stop()
            EnsureInvoke(Sub() StatusRemaining.Text = "")
            If Not ProbeVideo.StoreVideoProps(txtOutFilename.Text) Then
                StatusBarUpdate(Texts.ErrFailVideoOut)
                bFailed = True
                SetStatusLabel(IconStatusOutput, "Fail")
            Else

                lblOutputVideoLength.Text = ExtraFunc.SecToTimeStr(ProbeVideo.duration_sec)
                SetStatusLabel(IconStatusOutput, "OK")
                StatusBarUpdate(Texts.StatusOutReady)

            End If

        End If
        totalStopwatch.Stop()
        LastWholeVideoTotalElapsed = totalStopwatch.Elapsed
        AppendWholeVideoTimingLog()
    End Function

    Private Sub btnPrepareInput_Click(sender As Object, e As EventArgs) Handles btnPrepareInput.Click
        Dim bFailed As Boolean
        If Not bProcessIsRunning Then
            bProcessIsRunning = True
            btnPrepareInput.Text = Texts.btnInterrupt
            ClearLogs()
            ClearProgressBar(False)
            StatusBarUpdate("")
            If cbXJoin.Checked And txtVideolist.Text = "" Then
                MsgBox(Texts.NoInputFiles)
            Else
                bFailed = RunPrepareVideo()
            End If
            ' CLEANUP-CBNEWMAPF-START:
            'If Not bFailed And Not My.Settings.cbNewMapF Then

            '    bFailed = RunMakeFrames()

            'End If
            If bFailed Then
                StatusBarUpdate(Texts.ErrFailRunning)

            Else
                ' CLEANUP-CBNEWMAPF-START:If My.Settings.bUseMapTrackingVideo  Then ' CLEANUP-CBNEWMAPF-START:Or My.Settings.cbNewMapF
                SetStatusLabel(IconStatusPrepare, "OK") 'Only the video is prepared with a trackingvideo, or new map function.
                ' CLEANUP-CBNEWMAPF-START:End If
                StatusBarUpdate(Texts.Status6)
            End If
            My.Settings.AdjTestVideoReady = False
            My.Settings.bAdjVideoIsReady = False
            SaveStatusLabels()
            btnPrepareInput.Text = Texts.btnPrepare
            bProcessIsRunning = False
        Else
            Dim result As DialogResult

            result = MessageBox.Show(Texts.QQuitProcess, "", MessageBoxButtons.YesNoCancel)

            If DialogResult.Yes Then
                bProcessIsRunning = False
                proc.StopProcess()
                btnPrepareInput.Text = Texts.btnPrepare
            End If


        End If
    End Sub
    Private Sub btnRunMakeVideo_Click(sender As Object, e As EventArgs) Handles btnRunMakeVideo.Click
        Dim bFailed, bMusic, bOutput As Boolean
        If My.Settings.MapFlipActive Then
            Dim result As DialogResult = MsgBox(Texts.MapFlipSaveWarn, MessageBoxButtons.OKCancel)
            If result = DialogResult.Cancel Then Return
        End If
        If Not bProcessIsRunning Then
            bProcessIsRunning = True
            btnRunMakeVideo.Text = Texts.btnInterrupt
            ClearLogs()
            ClearProgressBar(False)
            StatusBarUpdate("")
            bOutput = True ' Generate output video
            bMusic = True ' generate music video
            Dim result As DialogResult
            If My.Settings.StatusOutputVideo = "OK" Then
                result = MessageBox.Show(Texts.QDoOutputAgain, "", MessageBoxButtons.YesNoCancel)

                If DialogResult.Yes Then
                    bOutput = True
                End If
                If DialogResult.No Then
                    bMusic = False
                End If
            End If

            If bOutput Then ' Kun ny video hvis der er ændringer
                bFailed = RunMakeVideo()
                If bFailed Then
                    StatusBarUpdate(Texts.ErrFailRunning)
                    My.Settings.AudioVideofile = ""
                    My.Settings.bOutputfileReady = False
                Else
                    StatusBarUpdate(Texts.Status6)
                    My.Settings.AudioVideofile = txtOutFilename.Text
                    My.Settings.bOutputfileReady = True
                End If
                SaveStatusLabels()
            End If
            If My.Settings.bDoMusic And bMusic Then
                ClearProgressBar(False)
                StatusBarUpdate("")
                bFailed = MakeMusicVideo()
                If bFailed Then
                    StatusBarUpdate(Texts.ErrFailRunning)
                Else
                    StatusBarUpdate(Texts.Status6)
                End If
                SaveStatusLabels()
            End If
            btnRunMakeVideo.Text = Texts.btnMakeVideo
            bProcessIsRunning = False
        Else
            Dim result As DialogResult

            result = MessageBox.Show(Texts.QQuitProcess, "", MessageBoxButtons.YesNoCancel)

            If DialogResult.Yes Then
                bProcessIsRunning = False
                proc.StopProcess()
                btnRunMakeVideo.Text = Texts.btnMakeVideo
            End If


        End If
    End Sub

    Private Sub IndstillingerToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles IndstillingerToolStripMenuItem1.Click
        Dim FrmSettings As New frmSettings
        FrmSettings.ShowDialog()
        ChangeNewMap()
        InitVideoFormat()
    End Sub

    Private Sub OmToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OmToolStripMenuItem1.Click
        Dim Frm As New AboutApp

        Frm.Show()

    End Sub

    'OLD MAP 20260407:
    'Function CheckFiles() As Boolean
    '    If Not My.Computer.FileSystem.FileExists(AppFolder + FileNameConv(txtQRImage.Text)) Then MsgBox("QuickRoute billede mangler : " + txtQRImage.Text)
    '    If Not My.Computer.FileSystem.FileExists(AppFolder + FileNameConv(txtXMLfile.Text)) Then MsgBox("QuickRoute XML mangler : " + txtXMLfile.Text)
    '    If Not My.Computer.FileSystem.FileExists(AppFolder + FileNameConv(txtGpxFile.Text)) Then MsgBox(".GPX fil mangler : " + txtGpxFile.Text)
    '    If txtOutFilename.Text = "" Then MsgBox("Output filnavn mangler")

    'End Function

    Private Sub EnglishToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EnglishToolStripMenuItem.Click

        ChangeLanguage("en-US")

    End Sub

    'Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
    'Dim a As New clsTxtFiles
    'If Not My.Computer.FileSystem.FileExists(AppFolder + txtXMLfile.Text) Then MsgBox(Texts.ErrDeshakemissing + DeshakeFilename)

    'End Sub

    Private Sub ChineseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChineseToolStripMenuItem.Click

        ChangeLanguage("zh")

    End Sub


    Private Sub btnAddMusic_Click(sender As Object, e As EventArgs) Handles btnAddMusic.Click
        Dim frmAddAudio As New AddAudioform(txtOutFilename.Text)
        frmAddAudio.Show()
    End Sub

    Private Sub btnMapSetting_Click(sender As Object, e As EventArgs) Handles btnMapSetting.Click
        Dim a As frmMapOverlayForm = New frmMapOverlayForm(ExtraFunc)
        SetStatusLabel(IconStatusOutput, "NoVideo", "Outputvideo")
        a.Show()
    End Sub

    Private Sub HjælpToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HjælpToolStripMenuItem.Click
        System.Diagnostics.Process.Start("http://wobelix.dk/blog/instructions-for-headcam-orienteering-quickroute/")
    End Sub

    Private Sub SideOmSideVideoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SideOmSideVideoToolStripMenuItem.Click
        Dim a As New frmSideBySide(Me)
        a.Show()
    End Sub

    Private Sub BrugTrackingvideoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BrugTrackingvideoToolStripMenuItem.Click
        Dim a As New frmUseMapVideo(Me)
        a.Show()
    End Sub

    Private Sub VideoafspillerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VideoafspillerToolStripMenuItem.Click
        Dim a As New frmPlayer(txtOutFilename.Text)
        a.Show()
    End Sub
    Private Sub ResetData()
        bMakeMapDirty = True
        CheckDirtyStatus()
        txtVideolist.Text = ""
        My.Settings.Videofilesfull = ""
        bMakeVideoInDirty = True
        CheckDirtyStatus()
        bOutputVideoDirty = True
        CheckDirtyStatus()
        cbXDeshake.Checked = False
        txtPrepareLength.Text = ""
        txtInpCutfromStart.Text = ""
        txtOutputLength.Text = ""
        txtOutFilename.Text = ""
        My.Settings.txtGPSPos = ""
        My.Settings.txtVideoPos = ""
        My.Settings.GPXDiff = "0"
        My.Settings.bUseMapTrackingVideo = False
        TrackVideoState()
        My.Settings.txtMapVideoFilename = ""
        My.Settings.txtRealtimeFactor = ""
        My.Settings.ffmpegCRF = "25"
        My.Settings.ffmpegVidstabTransform = "vidstabtransform=smoothing=25:crop=black:zoom=0:optzoom=0:interpol='bicubic':input=data.trf:tripod=0,unsharp=5:5:0.8:3:3:0.4"
        ' CLEANUP-CBNEWMAPF-START:My.Settings.cbNewMapF = True
        My.Settings.VideoPadding = "L"
        With My.Settings
            .GPXDiff1 = ""
            .GPXDiff2 = ""
            .QRimage1 = ""
            .QRImage2 = ""
            .QRXML1 = ""
            .QRXML2 = ""
            .MapFlipStartS = ""
            .MapFlipStartT = ""
            .MapFlipActive = False
            MapFlip(False, False)

            .Save()
        End With
        ChangeNewMap()
    End Sub
    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Dim result As DialogResult = MessageBox.Show(Texts.QReset, Texts.QResetCaption, MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            ResetData()
        End If

    End Sub

    Private Sub txtOutFilename_TextChanged(sender As Object, e As EventArgs) Handles txtOutFilename.TextChanged
        bOutputVideoDirty = True
        CheckDirtyStatus()

    End Sub

    Private Sub TimerStatusRemaining_Tick(sender As Object, e As EventArgs) Handles TimerStatusRemaining.Tick

        StatusRemaining.Text = RemainingTimeObj.sPercent + " " + Texts.StatusTimeLeft + RemainingTimeObj.sRemainingTime
    End Sub

    Private Sub FFPlayTestafspillerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FFPlayTestafspillerToolStripMenuItem.Click
        Dim Player As ffplay = New ffplay()
        Player.Show()
    End Sub

    Private Sub NyJusteringToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Dim Slider As New frmAdjustmentPlayer_new(Me, 500)
        Slider.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim ImgAdj As New frmVideoImageAdjust
        ImgAdj.Show()
    End Sub



    Public Function MakeAdjVideo(filename As String, VLength As String, GPS0 As Boolean) As Boolean
        Dim GPStr As String

        If GPS0 Then
            GPStr = ""
        Else
            GPStr = LblGPSDiff.Text
        End If
        Dim arg As String
        If My.Settings.bUseMapTrackingVideo Then
            arg = ExtraFunc.FFMPeg_MakeParamMapOnVideo(filename, My.Settings.TrackMapVideoFilename, GetDeshakedFilename(True), GPStr, VLength, numVideoTempo.Value, True, True, My.Settings.txtRealtimeFactor)
        Else
            arg = ExtraFunc.FFMPeg_MakeParamMapOnVideo(filename, "temp2\%08d.jpg", GetDeshakedFilename(True), GPStr, VLength, numVideoTempo.Value, True)
        End If
        LogMakeVideo += arg + vbCrLf
        StatusBarUpdate(Texts.StatusAdjVidStart)
        Run_CommandX(FFMpegExe, arg, LogMakeVideo)

        If Not GPS0 Then My.Settings.AdjVideoGPSDiff = LblGPSDiff.Text
        StatusBarUpdate(Texts.StatusAdjVidFinish)

    End Function

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        If StatusCount <= StatusProgressBar1.Maximum Then
            StatusProgressBar1.Value = StatusCount
        Else
            StatusProgressBar1.Value = 0
            StatusCount = 0
        End If
    End Sub

    Private Sub btnAdjVideo_Click(sender As Object, e As EventArgs) Handles btnMakeAdjVideo.Click
        Dim ok As Boolean
        SetStatusLabel(IconStatusOutput, "NoVideo", "Outputvideo")
        ok = ReadyForOutputVideo()
        If Not ok Then
            Dim result As DialogResult = MessageBox.Show(Texts.ErrStepsNotReady, "", MessageBoxButtons.YesNoCancel)
            If result = DialogResult.Yes Then ok = True
        End If
        If ok Then
            ' CLEANUP-CBNEWMAPF-START:If My.Settings.cbNewMapF Then 'new adjust
            Try
                Dim Slider As New frmAdjustmentPlayer_new(Me, 500)
                Slider.Show()
            Catch ex As Exception

            End Try



            ' CLEANUP-CBNEWMAPF-START:Else
            'Dim Player As frmAdjustmentPlayer = New frmAdjustmentPlayer(Me, lblMapLength.Text)
            '    Player.Show()
            'End If
        End If
    End Sub

    Private Sub btnPostProcess_Click(sender As Object, e As EventArgs) Handles btnPostProcess.Click
        Dim PostProcess As frmPostProcess = New frmPostProcess
        PostProcess.Show()
    End Sub

    Private Sub ChangeFPSScriptToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangeFPSScriptToolStripMenuItem.Click
        Dim FPSdlg As frmScriptMultiFFMpeg = New frmScriptMultiFFMpeg
        FPSdlg.Show()
    End Sub

    Private Sub FixApplegpxFilToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FixApplegpxFilToolStripMenuItem.Click

        Dim filePath As String
        MsgBox(Texts.GPXFixInfo)
        If OpenFileDialoggpx.ShowDialog() = DialogResult.OK Then
            filePath = OpenFileDialoggpx.FileName



            ' Læs hele filen som en enkelt streng
            Dim contents As String = File.ReadAllText(filePath)

            ' Opret en regex til at finde <trk>...</trk> med mellemrum og linjeskift ignoreret
            Dim pattern As String = "<trk>\s*</trk>"

            ' Erstat de fundne forekomster med en tom streng
            Dim replacedContents As String = Regex.Replace(contents, pattern, "", RegexOptions.IgnoreCase Or RegexOptions.Singleline)

            ' Skriv det opdaterede indhold tilbage til filen
            File.WriteAllText(filePath, replacedContents)
            MsgBox(Texts.GPXFixed)
        End If
    End Sub
    Private Function MapFlip(Flip As Boolean, Optional filecheck As Boolean = True)
        If Not Flip Then
            My.Settings.MapFlipActive = False
            btnQRimg.Enabled = True
            btnQRxml.Enabled = True
            txtQRImage.Enabled = True
            txtXMLfile.Enabled = True
            grpMapFlip.BackColor = SystemColors.Control
            My.Settings.GPXDiff2 = My.Settings.GPXDiff ' save diff
            My.Settings.GPXDiff = My.Settings.GPXDiff1 ' restore 1
            My.Settings.QRimage = My.Settings.QRimage1
            My.Settings.QRXML = My.Settings.QRXML1
            txtOutputLength.Text = My.Settings.MapFlipStartT 'set to cut video for map 1
            If String.IsNullOrWhiteSpace(My.Settings.MapFlipStartT) Or My.Settings.MapFlipStartT = "0:00:00" Then
                txtOutputLength.Text = ""
                txtOutputLength.BackColor = SystemColors.Window
            Else
                txtOutputLength.BackColor = Color.Salmon
            End If

        Else
            'check files
            If Not File.Exists(AppFolder + My.Settings.QRImage2) And filecheck Then
                MsgBox(Texts.ErrNoFile + My.Settings.QRImage2)
            ElseIf Not File.Exists(AppFolder + My.Settings.QRXML2) And filecheck Then
                MsgBox(Texts.ErrNoFile + My.Settings.QRXML2)
            Else
                My.Settings.MapFlipActive = True
                btnQRimg.Enabled = False
                btnQRxml.Enabled = False
                txtQRImage.Enabled = False
                txtXMLfile.Enabled = False
                grpMapFlip.BackColor = Color.YellowGreen
                My.Settings.GPXDiff1 = My.Settings.GPXDiff 'save diff
                My.Settings.GPXDiff = My.Settings.GPXDiff2 ' restore 2
                My.Settings.QRimage = My.Settings.QRImage2
                My.Settings.QRXML = My.Settings.QRXML2
                txtOutputLength.Text = "" 'clear the cut length
                txtOutputLength.BackColor = SystemColors.Window

            End If
        End If
    End Function
    Private Sub btnMapFlip_Click(sender As Object, e As EventArgs) Handles btnMapFlip.Click

        MapFlip(Not My.Settings.MapFlipActive) ' change to opposite
    End Sub

    Private Sub btnMapFlipinfo_Click(sender As Object, e As EventArgs) Handles btnMapFlipinfo.Click
        MsgBox(Texts.MapFlipInfo)
    End Sub

    Private Sub chkMapFlipOnOff_CheckedChanged(sender As Object, e As EventArgs) Handles chkMapFlipOnOff.CheckedChanged
        If chkMapFlipOnOff.Checked Then ' Visning aktiv->fjern visning
            My.Settings.MapFlipOnOff = True
            grpMapFlip.Visible = True
            If String.IsNullOrWhiteSpace(My.Settings.MapFlipStartT) Or My.Settings.MapFlipStartT = "0:00:00" Then 'Set mapflip Cut value
                txtOutputLength.Text = ""
                txtOutputLength.BackColor = SystemColors.Window
            Else
                txtOutputLength.BackColor = Color.Salmon
                txtOutputLength.Text = My.Settings.MapFlipStartT
            End If
        Else
            My.Settings.MapFlipOnOff = False
            grpMapFlip.Visible = False
            If Not txtOutputLength.BackColor = SystemColors.Window Then 'Mapflip cut video active - deactivate
                txtOutputLength.Text = ""
                txtOutputLength.BackColor = SystemColors.Window
            End If
            If My.Settings.MapFlipActive Then
                MapFlip(False, False) ' deactivate map flip
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If txtOutFilename.Text.Length = 0 Then
            MsgBox(Texts.ErrNoFolder)
            Return
        End If
        Dim folderPath As String = Path.GetDirectoryName(txtOutFilename.Text)

        ' Kontroller, at stien findes
        If Directory.Exists(folderPath) Then
            ' Åbn Stifinder på den specificerede sti
            Process.Start("explorer.exe", folderPath)
        Else
            MsgBox(Texts.ErrNoFolder)
        End If
    End Sub

    Public Function PrepareTrackingMap(ImgFilename As String) As Boolean
        Dim arg As String
        Dim ProbeVideo As New clsFFMPegProbe
        PrepareTrackingMap = True
        If Not ProbeVideo.StoreVideoProps(My.Settings.TrackMapVideoFilename) Then
            MsgBox(Texts.ErrTrackMapImage)
            PrepareTrackingMap = False
        Else

            My.Settings.TrackMapWidth = ProbeVideo.Width
            My.Settings.TrackMapHeight = ProbeVideo.Height
            arg = ExtraFunc.FFMpeg_VideoFrameToImage(My.Settings.TrackMapVideoFilename, ImgFilename, 10)
            LogMakeVideo += arg + vbCrLf
            Run_CommandX(FFMpegExe, arg, LogMakeVideo)
            My.Settings.TrackMapVideoLength = ExtraFunc.SecToTimeStr(ProbeVideo.duration_sec * CInt(My.Settings.txtRealtimeFactor))
            If iconStatusVideoIn.Tag.ToString = "OK" Then SetStatusLabel(IconStatusPrepare, "OK")
        End If
    End Function
    Private Sub ViKørselslogToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViKørselslogToolStripMenuItem.Click

        Dim LogForm As New frmShowLogForm(LogJoin, LogDeshake, LogMapF, LogMakeVideo, LogStatus)

        LogForm.Show()
    End Sub

    Private Sub DanskToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DanskToolStripMenuItem.Click

        ChangeLanguage("da-DK")

    End Sub

    Private Sub ChangeLanguage(ByVal lang As String)
        Dim resources As ComponentResourceManager = New ComponentResourceManager(GetType(MainForm))
        Dim resources2 As ComponentResourceManager = New ComponentResourceManager(GetType(MainForm))
        Thread.CurrentThread.CurrentUICulture = New CultureInfo(lang)
        InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(New CultureInfo(lang))

        For Each c As Control In Me.Controls

            resources.ApplyResources(c, c.Name, New CultureInfo(lang))
            If c.HasChildren Then ' 3 levels of children
                For Each ctrl As Control In c.Controls ' 2. level
                    resources.ApplyResources(ctrl, ctrl.Name, New CultureInfo(lang))
                    Me.ToolTip2.SetToolTip(ctrl, resources.GetString(ctrl.Name + ".ToolTip"))
                    Me.ToolTip1.SetToolTip(ctrl, resources.GetString(ctrl.Name + ".ToolTip1"))
                    If ctrl.HasChildren Then '3. level
                        For Each ctrl2 As Control In ctrl.Controls
                            resources.ApplyResources(ctrl2, ctrl2.Name, New CultureInfo(lang))
                            Me.ToolTip2.SetToolTip(ctrl2, resources.GetString(ctrl2.Name + ".ToolTip"))
                            Me.ToolTip1.SetToolTip(ctrl2, resources.GetString(ctrl2.Name + ".ToolTip1"))
                            If ctrl2.HasChildren Then '4. level

                                For Each ctrl3 As Control In ctrl2.Controls
                                    resources.ApplyResources(ctrl3, ctrl3.Name, New CultureInfo(lang))
                                    Me.ToolTip2.SetToolTip(ctrl3, resources.GetString(ctrl3.Name + ".ToolTip"))
                                    Me.ToolTip1.SetToolTip(ctrl3, resources.GetString(ctrl3.Name + ".ToolTip1"))
                                Next ctrl3
                            End If
                        Next ctrl2
                    End If
                Next ctrl
            End If
        Next c
        For Each item As ToolStripMenuItem In MenuStrip1.Items
            resources.ApplyResources(item, item.Name, New CultureInfo(lang))
            For Each item2 As ToolStripMenuItem In item.DropDownItems
                resources.ApplyResources(item2, item2.Name, New CultureInfo(lang))
            Next
        Next
        'resources.ApplyResources(ToolTip1, New CultureInfo(lang))
        If lang = "en-US" Then
            My.Settings.Language = "English"
        ElseIf lang = "zh" Then
            My.Settings.Language = "Chinese"
        Else
            My.Settings.Language = "Danish"
        End If
    End Sub

    Private Sub MainForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        My.Settings.Save()
    End Sub
    Public Sub ChangeNewMap()
        ' CLEANUP-CBNEWMAPF-START:If My.Settings.cbNewMapF Then
        'OLD MAP 20260407:cbXMakeframes.Visible = False
        lblMapLength.Visible = False
        btnMapSetting.Visible = False
        'OLD MAP 20260407:
        'Label2.Visible = False
        '    txtGpxFile.Visible = False
        '    btnGpxf.Visible = False
        cbOnlyVideo.Visible = False
        btnMakeMapN.Visible = False

        ' CLEANUP-CBNEWMAPF-START:Else
        'cbOnlyVideo.Visible = False
        '    btnMakeMapN.Visible = False
        '    cbXMakeframes.Visible = True
        '    lblMapLength.Visible = True
        '    btnMapSetting.Visible = True
        '    Label2.Visible = True
        '    txtGpxFile.Visible = True
        '    btnGpxf.Visible = True
        'End If
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not My.Settings.AppVersion = ProductVersion Then
            My.Settings.AppVersion = ProductVersion
            ResetData()
        End If
        InitStatusLabels()
        If Not _autorunRenderCompareStarted Then
            Dim autorunOptions As RenderCompareOptions = ParseRenderCompareArgs()
            If autorunOptions IsNot Nothing Then
                _autorunRenderCompareStarted = True
                Me.WindowState = FormWindowState.Minimized
                Me.ShowInTaskbar = False
                BeginInvoke(Sub() StartLegRenderComparison(autorunOptions))
            End If
        End If
        If Not _autorunRenderCompareStarted AndAlso Not _autorunRenderLegStarted Then
            Dim autorunLegOptions As RenderLegOptions = ParseRenderLegArgs()
            If autorunLegOptions IsNot Nothing Then
                _autorunRenderLegStarted = True
                Me.WindowState = FormWindowState.Minimized
                Me.ShowInTaskbar = False
                BeginInvoke(Sub() StartLegRender(autorunLegOptions))
            End If
        End If
    End Sub

    Private Sub MainForm_Leave(sender As Object, e As EventArgs) Handles Me.Leave

    End Sub
End Class


