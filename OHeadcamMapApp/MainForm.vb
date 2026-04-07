Imports System.ComponentModel
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
    Private LastSmoothZoomRenderElapsed As TimeSpan = TimeSpan.Zero
    Private LastSmoothLegRenderElapsed As TimeSpan = TimeSpan.Zero
    Public strQuote As String = Chr(34)
    Public bMakeMapDirty, bMakeVideoInDirty, bOutputVideoDirty As Boolean
    Public bProcessIsRunning As Boolean = False ' Used to interrupt background processes
    Public RemainingTimeObj As clsTimeRemaining = New clsTimeRemaining
    Public Shared strCurrentDone As String
    Public WriteToFiles As clMapsImgsToFiles
    Public WriteToFilesComplete As Boolean = False
    Private Const SmoothOverlayFolderName As String = "temp_smooth"
    Private Const SmoothOverlayBaseName As String = "map_overlay.mp4"


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
        ' Simple test harness for render-engine zoom/leg video generation
        If String.IsNullOrWhiteSpace(My.Settings.QRXML) Or String.IsNullOrWhiteSpace(My.Settings.QRimage) Then
            MsgBox("Please set QuickRoute XML and image in the main form before testing.")
            Return
        End If

        If Not File.Exists(AppFolder + My.Settings.QRXML) Then
            MsgBox("QR XML not found: " & AppFolder & My.Settings.QRXML)
            Return
        End If
        If Not File.Exists(AppFolder + My.Settings.QRimage) Then
            MsgBox("QR image not found: " & AppFolder & My.Settings.QRimage)
            Return
        End If

        SaveFileDialogOutput.FileName = "testrender" & GetSmoothOverlayVideoExtension()
        If SaveFileDialogOutput.ShowDialog() <> DialogResult.OK Then Return
        Dim outFile As String = SaveFileDialogOutput.FileName

        btnTestWriteVideo.Enabled = False
        StatusBarUpdate("Starting test render videos...")
        SetStatusLabel(IconStatusOutput, "Work", "Outputvideo")

        ' Read XML and load image on UI thread because ReadXML may touch UI/toolstrip items
        Dim RPs As New clsQRRoutePoints
        Dim reader As New clsQRXMLReader(RPs)
        reader.ReadXML(AppFolder + My.Settings.QRXML)

        ' Load the map image on UI thread then pass to background worker
        Dim mapImg As Bitmap = New Bitmap(Image.FromFile(AppFolder + My.Settings.QRimage))

        Task.Run(Sub()
                     Try
                         Dim renderEngine As New clsMapRenderEngine(RPs, mapImg)
                         renderEngine.FrameStepSeconds = GetSmoothOverlayFrameStepSeconds()
                         Dim outDir As String = Path.GetDirectoryName(outFile)
                         If String.IsNullOrEmpty(outDir) Then outDir = "."
                         Dim outBaseName As String = Path.GetFileNameWithoutExtension(outFile)
                         Dim outExt As String = GetSmoothOverlayVideoExtension()
                         Dim legOutFile As String = Path.Combine(outDir, outBaseName & "_leg" & outExt)
                         Dim zoomOutFile As String = Path.Combine(outDir, outBaseName & "_zoom" & outExt)

                         renderEngine.WriteLegVideo(legOutFile, startTime:=0, duration:=120, videoWidth:=ExtraFunc.InputWidth)
                         Dim legTotal As Double = renderEngine.LastTotalElapsed.TotalSeconds
                         Dim legRender As Double = renderEngine.LastRenderElapsed.TotalSeconds
                         Dim legEncode As Double = renderEngine.LastEncodeElapsed.TotalSeconds
                         Dim legFrames As Integer = renderEngine.LastFrameCount

                         renderEngine.WriteZoomVideo(zoomOutFile, startTime:=0, duration:=120, videoWidth:=ExtraFunc.InputWidth)
                         ' Dispose image after writer finished
                         mapImg.Dispose()

                         Me.BeginInvoke(Sub()
                                            Dim timingText As String = String.Format(CultureInfo.InvariantCulture,
                                                "Test render videos finished. Leg total {0:0.0}s render {1:0.0}s encode {2:0.0}s frames {3}. Zoom total {4:0.0}s render {5:0.0}s encode {6:0.0}s frames {7}.",
                                                legTotal,
                                                legRender,
                                                legEncode,
                                                legFrames,
                                                renderEngine.LastTotalElapsed.TotalSeconds,
                                                renderEngine.LastRenderElapsed.TotalSeconds,
                                                renderEngine.LastEncodeElapsed.TotalSeconds,
                                                renderEngine.LastFrameCount)
                                            StatusBarUpdate(timingText)
                                            LogMapF += timingText + vbCrLf
                                            SetStatusLabel(IconStatusOutput, "OK", "Outputvideo")
                                            btnTestWriteVideo.Enabled = True
                                        End Sub)
                     Catch ex As Exception
                         ' Ensure UI updates happen on UI thread
                         Me.BeginInvoke(Sub()
                                            MsgBox("Error during test write: " & ex.ToString())
                                            StatusBarUpdate("Error: " & ex.Message)
                                            SetStatusLabel(IconStatusOutput, "Fail", "Outputvideo")
                                            btnTestWriteVideo.Enabled = True
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
            End If
            If My.Settings.cbShowLegMAp Then
                legEngine = New clsMapRenderEngine(RPs, mapImg)
                legEngine.FrameStepSeconds = frameStepSeconds
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
            Dim progressSource As clsMapRenderEngine = If(legEngine, zoomEngine)
            If progressSource IsNot Nothing Then
                progressSource.ProgressCallback =
                    Sub(doneFrames As Integer, totalFrames As Integer, phaseFile As String)
                        Dim safeDone As Integer = Math.Min(totalFramesPlanned, doneFrames)
                        RemainingTimeObj.SetGetRemainingTime(CDec(safeDone))
                        EnsureInvoke(Sub()
                                         StatusRemaining.Text = RemainingTimeObj.sPercent & " " & Texts.StatusTimeLeft & RemainingTimeObj.sRemainingTime
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
                            zoomEngine.WriteZoomVideo(zoomVideo,
                                                      startTime:=0,
                                                      duration:=duration,
                                                      videoWidth:=ExtraFunc.InputWidth)
                        End Sub)
                End If

                If legEngine IsNot Nothing Then
                    legTask = Threading.Tasks.Task.Run(
                        Sub()
                            legEngine.WriteLegVideo(legVideo,
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
                    zoomEngine.WriteZoomVideo(zoomVideo,
                                              startTime:=0,
                                              duration:=duration,
                                              videoWidth:=ExtraFunc.InputWidth)
                End If
                If legEngine IsNot Nothing Then
                    legEngine.WriteLegVideo(legVideo,
                                            startTime:=0,
                                            duration:=duration,
                                            videoWidth:=ExtraFunc.InputWidth)
                End If
            End If

            If zoomEngine IsNot Nothing Then
                zoomRenderElapsed = zoomEngine.LastRenderElapsed
                zoomEncodeElapsed = zoomEngine.LastEncodeElapsed
                frameCount = Math.Max(frameCount, zoomEngine.LastFrameCount)
            End If

            If legEngine IsNot Nothing Then
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
    Private Sub AppendSmoothTimingLog()
        LogMapF += GetMapVideoTimingHeaderText() + vbCrLf
        LogMapF += GetMapVideoTimingLabelTotal() + FormatElapsed(LastSmoothAssetElapsed) + vbCrLf
        LogMapF += GetMapVideoTimingLabelRenderLoop() + FormatElapsed(LastSmoothRenderElapsed) + " (" + LastSmoothFrameCount.ToString(CultureInfo.InvariantCulture) + " frames)" + vbCrLf
        LogMapF += GetMapVideoTimingLabelBaseMap() + FormatElapsed(LastSmoothBaseMapElapsed) + vbCrLf
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
        bFailed = False
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
                    MakeMapFiles()
                End If
                StatusBarUpdate(Texts.StatusCombine2)
            End If
            Dim arg As String

            If My.Settings.bUseMapTrackingVideo Then

                arg = ExtraFunc.FFMPeg_MakeParamMapOnVideo(txtOutFilename.Text, My.Settings.TrackMapVideoFilename, GetDeshakedFilename(True), LblGPSDiff.Text, txtOutputLength.Text, numVideoTempo.Value, False, True, My.Settings.txtRealtimeFactor)
            Else
                ' CLEANUP-CBNEWMAPF-START:If My.Settings.cbNewMapF Then
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
            Run_CommandX(FFMpegExe, arg, LogMakeVideo)
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
        My.Settings.GPXDiff = ""
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
    End Sub

    Private Sub MainForm_Leave(sender As Object, e As EventArgs) Handles Me.Leave

    End Sub
End Class


