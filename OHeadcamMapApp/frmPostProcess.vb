Imports System.ComponentModel
Imports System.IO
Imports System.Text

Imports System.Text.RegularExpressions
Imports System.Threading
'Imports System.Web.UI.WebControls
'Imports System.Web.UI.WebControls
'Imports System.Web.UI.WebControls
Imports LibVLCSharp.Shared
Imports OHeadcamMapApp.My.Resources

Public Class frmPostProcess
    Private _VideoFileName, AppFolder As String
    'Private PlayMode As String, PlayMode_play As String = "play", PlayMode_fast As String = "fast", PlayMode_slow As String = "slow", PlayMode_stop As String = "stop"
    'Private _mp As MediaPlayer
    'Private _libVLC As LibVLC
    'Private bTrackChange, bMouseDown As Boolean
    'Private _WantedPosition As Double
    'Private CurrentTrackValue, ImageTrackValue As Integer
    Public tmpC As Integer
    Public strQuote As String = Chr(34)
    Private PostVideoList_Info As New List(Of clsFFMPegProbe)
    Private PostVideoList_Prop As New List(Of clsPostFProps)
    Private PostOverlayList_Info As New List(Of clsFFMPegProbe)
    Private PostOverlayList_Overlay As New List(Of clsPostOverlay)
    Private FF_Func As New clsExtra
    Private PostVideoAddPosition, PostOverlayAddPosition As Integer
    Private PostImgToVideoFunc As String = "Video" 'Overlay or Video
    Private LVselectedIndex As Integer = -1
    Private LVInsBefore As Boolean = True
    Private LVColumnStr As String
    Private tmpVideoO As clsPostOverlay
    Private tmpVideoP As clsPostFProps
    Private tmpVideo, VideoInfo As clsFFMPegProbe
    Private _TotalDuration As Integer
    Private randomno As Random = New Random
    Private progressIncrement As Integer = 10
    Private WithEvents ffmpegProcess As Process
    Private outputBuffer As New StringBuilder()
    Private framesTotal As Integer
    Private startTime As DateTime
    Private FFImgToVideoArg As String
    Private FFLogLevel As String = "error -stats"
    Delegate Sub UpdateTextBoxDelg(text As String, ProcessEnd As Boolean)
    Public Shared myDelegate As UpdateTextBoxDelg
    Public Shared MeVar As frmPostProcess
    Public Shared ProcessEnded As Boolean
    Public Shared FFError As Boolean = False
    Public Shared FFErrorTxt As String = ""
    Public ffProc As clsRunprocess = New clsRunprocess(AddressOf FFUpdate)
    Public RemainingTimeObj As clsTimeRemaining = New clsTimeRemaining

    '*** ImgVideoPlayer
    'Dim VideoImageGenerator As clsVideoImageGenerator
    'Private isMouseDown As Boolean = False
    'Private isTrackchanged As Boolean = False
    'Private lock As Boolean = False
    'Private _VideoReady As Boolean = False
    'Private IsFilterChanged As Boolean = False
    'Private CurrentTrackValue, ImageTrackValue As Integer
    'Private ResizeVideoPB As clsPicBKeepAspectResize = New clsPicBKeepAspectResize

    '**** VLC VideoView player
    Dim PlayMode As String, PlayMode_play As String = "play", PlayMode_fast As String = "fast", PlayMode_slow As String = "slow", PlayMode_stop As String = "stop"
    Private _mp As MediaPlayer
    Private _libVLC As LibVLC
    Private _WantedPosition As Double
    Private CurrentTrackValue, ImageTrackValue, _MapTime As Integer
    Private bTrackChange As Boolean

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        MeVar = Me
        AppFolder = AppDomain.CurrentDomain.BaseDirectory
        AppFolder = AppFolder.Substring(0, AppDomain.CurrentDomain.BaseDirectory.LastIndexOf("\")) + "\"
        ' Add any initialization after the InitializeComponent() call.
        '_libVLC = New LibVLC()
        '_mp = New MediaPlayer(_libVLC)
        'VideoView1.MediaPlayer = _mp

        'AddHandler _mp.EncounteredError, AddressOf MediaPlayer_EncounteredError
        'PlayMode = PlayMode_stop
        _VideoFileName = My.Settings.Outputfile
        '***VLC videoviewer
        Core.Initialize()
        ' Add any initialization after the InitializeComponent() call.
        _libVLC = New LibVLC()
        _mp = New MediaPlayer(_libVLC)
        VideoView1.MediaPlayer = _mp
        AddHandler _mp.EncounteredError, AddressOf MediaPlayer_EncounteredError

    End Sub


    'Private Sub SetVideo2()
    '    VideoInfo = New clsFFMPegProbe
    '    VideoInfo.StoreVideoProps(_VideoFileName)

    '    txtVideofile.Text = _VideoFileName
    '    If File.Exists(_VideoFileName) Then
    '        TotalDuration = VideoInfo.duration_sec
    '        _VideoReady = True
    '        InitFileList(True)
    '        'InitImage()
    '    Else
    '        MsgBox(Texts.ErrNoFile + _VideoFileName)
    '    End If
    'End Sub

    Private Function GetPostFileSettings(PostFInfo As clsFFMPegProbe) As clsPostFProps
        Dim dialog As New frmPostFileProps(PostFInfo)
        Dim result As DialogResult = dialog.ShowDialog()
        Dim returnedValues As clsPostFProps = New clsPostFProps

        returnedValues = dialog.ReturnedValues
        ' Access the returned values

        Return returnedValues
    End Function
    Private Function GetPostOverlaySettings(PostFInfo As clsFFMPegProbe) As clsPostOverlay
        Dim dialog As New frmPostOverlay(PostFInfo)
        Dim result As DialogResult = dialog.ShowDialog()
        Dim returnedValues As clsPostOverlay = New clsPostOverlay

        returnedValues = dialog.ReturnedValues
        ' Access the returned values

        Return returnedValues
    End Function
    Sub OnOffControls(bEnabled As Boolean)
        btnClearVideolist.Enabled = bEnabled
        btnView.Enabled = bEnabled
        AddVideofileaft.Enabled = bEnabled
        AddVideofilebef.Enabled = bEnabled

    End Sub
    Private Sub ImgToVideo(vid As clsFFMPegProbe, duration As Integer)
        Dim DoPad As Boolean
        Dim FileNameTag As String = ""
        If vid.IsImage Then
            If PostImgToVideoFunc = "Video" Then
                DoPad = True
            End If
            If PostImgToVideoFunc = "Overlay" Then
                DoPad = False
                FileNameTag = "O" ' To avoid same .mp4 for mainvideos and overlays
            End If
            Me.Cursor = Cursors.WaitCursor
            OnOffControls(False)
            arg = FF_Func.FF_ImgToVideo(vid, 1, "", DoPad, FileNameTag) ' duration=1 because of using loop, "O"=overlay tag in filename.
            arg = " -loglevel " + FFLogLevel + arg
            framesTotal = 25 * duration
            StatusTxt.Text = Texts.PostStatusImgVideo
            FFImgToVideoArg = arg
            RemainingTimeObj.StartTime(duration)
            lbStatusProgress.Text = ""
            RemainingTimer.Start()
            RunFFmpegCommand(arg)
        End If
    End Sub

    Private Sub UpdateVideoListColumn()
        If LVMediaList.Items.Count = PostVideoList_Prop.Count Then
            Dim VidStart As Integer
            _TotalDuration = 0
            For i As Integer = 0 To PostVideoList_Prop.Count - 1
                Dim currentDuration As Integer = PostVideoList_Prop(i).iDuration
                Dim TDuration As Integer = 0

                If Not Integer.TryParse(PostVideoList_Prop(i).sTDuration, TDuration) Then ' find the offset for video moving start back
                    TDuration = 0
                End If
                If i = 0 Then TDuration = 0 'Cannot start before 0:00
                VidStart = _TotalDuration - TDuration
                _TotalDuration = VidStart + currentDuration
                Dim VidStartStr = FF_Func.SecToTimeStr(VidStart)
                Dim VidEndStr = FF_Func.SecToTimeStr(_TotalDuration)
                LVMediaList.Items(i).SubItems(1).Text = $"{VidStartStr}-{VidEndStr}"

            Next
        Else
            MessageBox.Show("Number of items in ListView does not match PostVideoList_Prop count.")
        End If
    End Sub
    Sub AddPostVideo()

        LVColumnStr = Path.GetFileName(tmpVideo.Videofilename) + ", "
        If LVInsBefore Then
            VLinsertListViewBefore(LVMediaList, LVColumnStr)
        Else
            VLinsertListViewAfter(LVMediaList, LVColumnStr)

        End If
        If LVselectedIndex >= 0 Then

            PostVideoList_Prop.Insert(LVselectedIndex, tmpVideoP)
            PostVideoList_Info.Insert(LVselectedIndex, tmpVideo)

        Else
            PostVideoList_Info.Add(tmpVideo)
            PostVideoList_Prop.Add(tmpVideoP) ' Insert at the end
        End If
        UpdateVideoListColumn()
    End Sub
    Sub AddPostOverlay()

        sDur = FF_Func.SecToTimeStr(tmpVideoO.sDuration)
        SStart = FF_Func.SecToTimeStr(tmpVideoO.sStart)
        LVColumnStr = $"{Path.GetFileName(tmpVideo.Videofilename)},{SStart} ({sDur})"
        If LVInsBefore Then
            VLinsertListViewBefore(LV_Overlaylist, LVColumnStr)
        Else
            VLinsertListViewAfter(LV_Overlaylist, LVColumnStr)

        End If
        If LVselectedIndex >= 0 Then

            PostOverlayList_Overlay.Insert(LVselectedIndex, tmpVideoO)
            PostOverlayList_Info.Insert(LVselectedIndex, tmpVideo)

        Else
            PostOverlayList_Info.Add(tmpVideo)
            PostOverlayList_Overlay.Add(tmpVideoO) ' Insert at the end
        End If

    End Sub
    Private Sub AddVideofilebef_Click(sender As Object, e As EventArgs) Handles AddVideofilebef.Click
        PostVideoAddPosition = 0
        ClearStatus()
        If OpenFileDialogVideo.ShowDialog() = DialogResult.OK Then
            tmpVideo = New clsFFMPegProbe
            tmpVideo.StoreVideoProps(OpenFileDialogVideo.FileName)
            tmpVideoP = GetPostFileSettings(tmpVideo)
            LVInsBefore = True
            If tmpVideo.IsImage Then
                PostImgToVideoFunc = "Video"
                ImgToVideo(tmpVideo, tmpVideoP.iDuration)
            Else
                AddPostVideo()
            End If

        End If
    End Sub

    Private Sub AddVideofileaft_Click(sender As Object, e As EventArgs) Handles AddVideofileaft.Click
        ClearStatus()
        PostVideoAddPosition = -1 'Add to list
        If OpenFileDialogVideo.ShowDialog() = DialogResult.OK Then
            tmpVideo = New clsFFMPegProbe
            tmpVideo.StoreVideoProps(OpenFileDialogVideo.FileName)
            tmpVideoP = GetPostFileSettings(tmpVideo)
            LVInsBefore = False
            If tmpVideo.IsImage Then
                PostImgToVideoFunc = "Video"
                ImgToVideo(tmpVideo, tmpVideoP.iDuration)
            Else
                If Not IsNothing(tmpVideo) Then
                    AddPostVideo()
                End If
            End If

        End If
    End Sub

    Private Sub btnDeleteVideoFile_Click(sender As Object, e As EventArgs) Handles btnDeleteVideoFile.Click
        ClearStatus()
        If LVMediaList.SelectedItems.Count > 0 Then
            Dim selectedIndexes As New List(Of Integer)
            For Each selectedItem As ListViewItem In LVMediaList.SelectedItems

                selectedIndexes.Add(selectedItem.Index)

            Next
            selectedIndexes.Sort()
            selectedIndexes.Reverse()
            For Each selectedIndex As Integer In selectedIndexes
                PostVideoList_Info.RemoveAt(selectedIndex)
                PostVideoList_Prop.RemoveAt(selectedIndex)
                LVMediaList.Items.RemoveAt(selectedIndex)
            Next
        End If
        If LVMediaList.Items.Count > 0 Then
            UpdateVideoListColumn()
        End If
    End Sub

    Private Sub btnClearVideolist_Click(sender As Object, e As EventArgs) Handles btnClearVideolist.Click
        ClearStatus()
        InitFileList(True)
    End Sub
    Private Sub LVMediaList_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles LVMediaList.MouseDoubleClick
        ClearStatus()
        If LVMediaList.SelectedItems.Count > 0 Then
            Dim index As Integer = LVMediaList.SelectedIndices(0)
            Dim dialog As New frmPostFileProps(PostVideoList_Prop(index), PostVideoList_Info(index))
            Dim result As DialogResult = dialog.ShowDialog()
            UpdateVideoListColumn()
        End If
    End Sub
    Private Function VLinsertListViewBefore(lv As ListView, colStr As String) As Integer
        If Not String.IsNullOrEmpty(colStr) Then
            Dim columns As String() = colStr.Split(","c) ' Split the input string using comma as delimiter
            Dim item As New ListViewItem(columns)
            ' Get the index of the selected item (if any)

            If lv.SelectedItems.Count > 0 Then
                LVselectedIndex = lv.SelectedIndices(0)
            End If
            If LVselectedIndex < 0 Then
                LVselectedIndex = 0
            End If
            lv.Items.Insert(LVselectedIndex, item)
        End If
    End Function
    Private Function VLinsertListViewAfter(lv As ListView, colStr As String) As Integer
        If Not String.IsNullOrEmpty(colStr) Then
            Dim columns As String() = colStr.Split(","c) ' Split the input string using comma as delimiter
            Dim item As New ListViewItem(columns)
            ' Get the index of the selected item (if any)

            If lv.SelectedItems.Count > 0 Then
                LVselectedIndex = lv.SelectedIndices(0) + 1
                If LVselectedIndex >= lv.Items.Count - 1 Then LVselectedIndex = -1
            Else
                LVselectedIndex = -1
            End If

            If LVselectedIndex > 0 Then
                lv.Items.Insert(LVselectedIndex, item)
            Else
                lv.Items.Add(item) ' Insert at the end
            End If

        End If
    End Function
    Private Sub VLClearSelection(lv As ListView)
        For Each item As ListViewItem In lv.Items
            item.Selected = False
        Next
    End Sub
    Private Sub btnAddOverlaybef_Click(sender As Object, e As EventArgs) Handles btnAddOverlaybef.Click
        ClearStatus()
        PostOverlayAddPosition = 0

        If OpenFileDialogVideo.ShowDialog() = DialogResult.OK Then
            tmpVideo = New clsFFMPegProbe
            tmpVideo.StoreVideoProps(OpenFileDialogVideo.FileName)

            tmpVideoO = GetPostOverlaySettings(tmpVideo)
            LVInsBefore = True


            If tmpVideo.IsImage Then
                PostImgToVideoFunc = "Overlay"
                If tmpVideoO.sDuration = "-1" Then
                    tmpVideoO.sDuration = 10 ' Should be updated to videolength
                End If
                ImgToVideo(tmpVideo, CInt(tmpVideoO.sDuration))
            Else
                AddPostOverlay()
            End If

        End If
    End Sub

    Private Sub btnAddOverlayaft_Click(sender As Object, e As EventArgs) Handles btnAddOverlayaft.Click
        ClearStatus()
        PostOverlayAddPosition = -1 'Add to list
        If OpenFileDialogVideo.ShowDialog() = DialogResult.OK Then
            tmpVideo = New clsFFMPegProbe
            tmpVideo.StoreVideoProps(OpenFileDialogVideo.FileName)
            tmpVideoO = GetPostOverlaySettings(tmpVideo)
            LVInsBefore = False
            If Not IsNothing(tmpVideo) Then
                If tmpVideo.IsImage Then
                    PostImgToVideoFunc = "Overlay"
                    ImgToVideo(tmpVideo, CInt(tmpVideoO.sDuration))
                Else
                    AddPostOverlay()
                End If
            End If

        End If
    End Sub

    Private Sub btnOverlayDelete_Click(sender As Object, e As EventArgs) Handles btnOverlayDelete.Click
        ClearStatus()
        If LV_Overlaylist.SelectedItems.Count > 0 Then
            Dim selectedIndexes As New List(Of Integer)
            For Each selectedItem As ListViewItem In LV_Overlaylist.SelectedItems

                selectedIndexes.Add(selectedItem.Index)

            Next
            selectedIndexes.Sort()
            selectedIndexes.Reverse()
            For Each selectedIndex As Integer In selectedIndexes

                PostOverlayList_Info.RemoveAt(selectedIndex)
                PostOverlayList_Overlay.RemoveAt(selectedIndex)
                LV_Overlaylist.Items.RemoveAt(selectedIndex)
            Next

        End If

        'If LV_Overlaylist.SelectedItems.Count > 0 Then
        '    LV_Overlaylist.Items.Remove(LV_Overlaylist.SelectedItems(0))
        'End If
    End Sub

    Private Sub btnOverlaysClear_Click(sender As Object, e As EventArgs) Handles btnOverlaysClear.Click
        ClearStatus()
        LV_Overlaylist.Items.Clear()
        PostOverlayList_Info.Clear()
        PostOverlayList_Overlay.Clear()
    End Sub

    Private Function MakeFFOutArg(EndTag As String) As String
        Dim Out As String
        Out = FF_Func.FF_movie_input(PostVideoList_Info, PostVideoList_Prop)
        Dim tag As String
        tag = "[vid0]"
        If PostVideoList_Prop.Count > 1 Then
            Out += ";" + FF_Func.FF_movie_xfade(PostVideoList_Prop)
            tag = "[fadeout]"
        End If
        If PostOverlayList_Overlay.Count > 0 Then
            Out += FF_Func.FF_movie_overlays(tag, PostOverlayList_Info, PostOverlayList_Overlay)
            tag = "[overout]"
        End If
        Out = Out.Replace(tag, EndTag)
        'VideoImageGenerator.FFParam = Out
        Return Out
    End Function
    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        ClearStatus()

        txtFFArg.Text = MakeFFOutArg("[out]")

        Dim a As New ffplay(txtFFArg.Text)
        'a.Show()
        a.Play(txtFFArg.Text, 0, 0, 800, 100, False, "Tester")
    End Sub

    Sub InitFileList(Clear As Boolean)
        If Clear Then
            LVMediaList.Items.Clear()
            PostVideoList_Info.Clear()
            PostVideoList_Prop.Clear()
            tmpVideo = New clsFFMPegProbe
            tmpVideoP = New clsPostFProps

            tmpVideo.StoreVideoProps(_VideoFileName)
            tmpVideoP.iDuration = CInt(Math.Abs(tmpVideo.duration_sec))
            tmpVideoP.sDuration = tmpVideoP.iDuration.ToString
            LVselectedIndex = -1
            AddPostVideo()
        End If
        'UpdateVideoList()
    End Sub

    Private Sub LV_OverlayList_Update()
        LV_Overlaylist.Items.Clear()
        For i = 0 To PostOverlayList_Overlay.Count - 1
            sDur = FF_Func.SecToTimeStr(PostOverlayList_Overlay(i).sDuration)
            SStart = FF_Func.SecToTimeStr(PostOverlayList_Overlay(i).sStart)


            LVColumnStr = $"{Path.GetFileName(PostOverlayList_Info(i).Videofilename)},{SStart} ({sDur})"
            Dim columns As String() = LVColumnStr.Split(","c) ' Split the input string using comma as delimiter
            Dim item As New ListViewItem(columns)
            LV_Overlaylist.Items.Add(item)
        Next
    End Sub


    Private Sub LV_Overlaylist_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles LV_Overlaylist.MouseDoubleClick
        ClearStatus()
        If LV_Overlaylist.SelectedItems.Count > 0 Then
            Dim index As Integer = LV_Overlaylist.SelectedIndices(0)
            Dim dialog As New frmPostOverlay(PostOverlayList_Overlay(index), PostOverlayList_Info(index))
            Dim result As DialogResult = dialog.ShowDialog()

            ' Access the returned values
            LV_OverlayList_Update()

        End If
    End Sub


    Private Sub frmPostProcess_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetVideo()
        Timer1.Start()
    End Sub
    Private Async Function RunFFmpegCommand(arguments As String) As Task
        ffmpegpath = AppFolder + "ffmpeg.exe"
        FFError = False
        FFErrorTxt = ""
        Me.Cursor = Cursors.WaitCursor
        Await ffProc.Run_Process_Wait(ffmpegpath, arguments)
        Me.Cursor = Cursors.Default
        ffProc.ClearProcess()
        ' MsgBox("Slut async")
    End Function

    Private Sub FFUpdate(Text As String, Ended As Boolean)
        MeVar.BeginInvoke(Sub() MeVar.UpdateProgress(Text, Ended))
    End Sub

    Private Function FFLegalstatsouput(text As String) As Boolean
        If text.ToLower().Contains("frame=") Or text.ToLower().Contains("size=") Then ' size= is audio files
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub UpdateProgress(Text As String, Ended As Boolean)

        If Not Ended Then
            ProcessEnded = False
            'Important - loglevel has to be "error -stats" for this to work
            If (Text.ToLower().Contains("error") Or (Not FFLegalstatsouput(Text) And Not String.IsNullOrWhiteSpace(Text))) And Not FFError Then 'only FFError first time
                'RemainingTimer.Stop()
                FFErrorTxt = Text
                'StatusTxt.Text = Texts.ErrFailVideoOut + " : " + FFErrorTxt
                'lbStatusProgress.Text = ""
                'ProgressBar1.Value = 0
                FFError = True
                Return
            End If
            Dim start As Integer = Text.IndexOf("time")
            Dim [end] As Integer = Text.IndexOf("bitrate")
            If start >= 0 And [end] > 0 Then
                Dim result As String = Text.Substring(start, [end] - start + "bitrate".Length)
                result = result.Substring(5, 8) 'tidskode uden ms
                RemainingTimeObj.SetGetRemainingTime(result)
                'lbStatusProgress.Text = "xx"

            End If
        Else
            If Not ProcessEnded Then
                ProcessEnded = True
                RemainingTimer.Stop()
                lbStatusProgress.Text = ""
                ProgressBar1.Value = 0
                'Dim out As String = DirectCast(e.Result, String)

                If Not FFError Then
                    StatusTxt.Text = Texts.StatusOutReady
                    'ImgToVideo = tmpVideo
                Else
                    StatusTxt.Text = Texts.ErrFailVideoOut + " : " + FFErrorTxt
                    'ImgToVideo = Nothing
                End If
                If PostImgToVideoFunc = "Overlay" Or PostImgToVideoFunc = "Video" Then
                    imgFn = GetFilenameFromFFString(FFImgToVideoArg)
                    tmpVideo.StoreVideoProps(imgFn)
                    If PostImgToVideoFunc = "Overlay" Then
                        AddPostOverlay()
                    Else
                        AddPostVideo()
                    End If
                End If
                Me.Cursor = Cursors.Default
                    OnOffControls(True)
                End If
            End If
    End Sub

    Private Sub RemainingTimer_Tick(sender As Object, e As EventArgs) Handles RemainingTimer.Tick
        lbStatusProgress.Text = RemainingTimeObj.sPercent + " " + Texts.StatusTimeLeft + RemainingTimeObj.sRemainingTime
        v = Math.Abs(RemainingTimeObj.dPercent) * 100
        If v > 100 Then v = 100
        ProgressBar1.Value = v
    End Sub



    Private Sub FFProcess_Exited(ByVal sender As Object,
            ByVal e As System.EventArgs) Handles ffmpegProcess.Exited

        myDelegate("Exited", True)

        OutStr = ""
        eventHandled = True

    End Sub

    Function GetFilenameFromFFString(arg As String) As String
        Dim outputFilenameStart As Integer = arg.LastIndexOf("-y ") + 3
        Dim outputFilename As String = arg.Substring(outputFilenameStart).Trim(""""c)
        Return outputFilename
    End Function

    '******  VideoImageGenerator ******
    'Private Sub InitImage()
    '    VideoImageGenerator = New clsVideoImageGenerator(1920, 1080, TotalDuration, "_postimg.jpg", PictureBox1)
    '    'ResizeVideoPB.SetNewAspect()
    '    TrackBar1.SmallChange = 1
    '    TrackBar1.Maximum = CInt(VideoImageGenerator._videoDuration * 2)
    '    VideoImageGenerator._SliderMax = TrackBar1.Maximum
    '    SetTickFreq(50)
    '    SetlblTValue(0)

    '    VideoImageGenerator.GenerateImageByParam(0, MakeFFOutArg(""))
    '    PBTimer.Enabled = True
    'End Sub
    'Private Sub PBTimer_Tick(sender As Object, e As EventArgs) Handles PBTimer.Tick
    '    If ((Not ImageTrackValue = CurrentTrackValue) Or IsFilterChanged) And Not lock Then
    '        lock = True
    '        ImageTrackValue = CurrentTrackValue
    '        tmpC += 1
    '        TextBox1.Text = $"{tmpC}"
    '        VideoImageGenerator.UpdateImageByParam(ImageTrackValue, IsFilterChanged)
    '        tmpC += 1
    '        TextBox1.Text = $"{tmpC}"
    '        IsFilterChanged = False
    '        SetlblTValue(VideoImageGenerator.seektime)
    '        lock = False
    '    End If
    'End Sub
    'Private Sub SetTickFreq(numTicks As Integer)

    '    Dim tickFrequency As Integer = CInt(TrackBar1.Maximum / numTicks)

    '    ' Set the TickFrequency property to the calculated value
    '    TrackBar1.TickFrequency = tickFrequency
    '    TrackBar1.LargeChange = tickFrequency

    'End Sub
    'Private Sub SetlblTValue(seconds As Double)
    '    Dim timeSpan As TimeSpan = TimeSpan.FromSeconds(seconds)
    '    Dim formattedTime As String = timeSpan.ToString("h\:mm\:ss")
    '    lblTValue.Text = formattedTime
    'End Sub
    'Private Sub TrackBar1_MouseDown(sender As Object, e As MouseEventArgs) Handles TrackBar1.MouseDown
    '    isMouseDown = True
    'End Sub

    'Private Sub TrackBar1_MouseUp(sender As Object, e As MouseEventArgs) Handles TrackBar1.MouseUp
    '    isMouseDown = False
    'End Sub
    'Private Sub TrackBar1_ValueChanged(sender As Object, e As EventArgs) Handles TrackBar1.ValueChanged
    '    CurrentTrackValue = TrackBar1.Value
    '    If _VideoReady Then SetlblTValue(VideoImageGenerator.CalcSliderTimer(CurrentTrackValue))
    'End Sub

    'Private Sub btnMinus_Click(sender As Object, e As EventArgs) Handles btnMinus.Click
    '    If _VideoReady Then
    '        setSliderValue(VideoImageGenerator.UpdateImageDelta(False, 1))
    '    End If
    'End Sub
    'Private Sub setSliderValue(InVal As Integer)
    '    lock = True
    '    CurrentTrackValue = InVal
    '    ImageTrackValue = CurrentTrackValue
    '    TrackBar1.Value = InVal
    '    SetlblTValue(VideoImageGenerator.seektime)
    '    lock = False
    'End Sub
    'Private Sub BtnPlus_Click(sender As Object, e As EventArgs) Handles BtnPlus.Click
    '    If _VideoReady Then setSliderValue(VideoImageGenerator.UpdateImageDelta(True, 1))
    'End Sub

    '**** VLC viewer
    Private Sub bSlow_Click(sender As Object, e As EventArgs) Handles bSlow.Click
        ClearStatus()
        If PlayMode = PlayMode_slow Then
            _mp.Pause()

            'AxWindowsMediaPlayer1.Ctlcontrols.pause()
            'setTrackbarValue(AxWindowsMediaPlayer1.Ctlcontrols.currentPosition)
            PlayMode = PlayMode_stop
        Else
            'AxWindowsMediaPlayer1.settings.rate = 0.5
            'AxWindowsMediaPlayer1.Ctlcontrols.play()
            _mp.SetRate(0.5)
            _mp.Play()
            bTrackChange = False
            PlayMode = PlayMode_slow

        End If
        SetPlayBtnTexts(bSlow)
    End Sub

    Private Sub bFForward_Click(sender As Object, e As EventArgs) Handles bFForward.Click
        ClearStatus()
        If PlayMode = PlayMode_fast Then
            _mp.Pause()
            PlayMode = PlayMode_stop
        Else
            _mp.SetRate(2)
            _mp.Play()
            bTrackChange = False
            PlayMode = PlayMode_fast

        End If
        SetPlayBtnTexts(bFForward)
    End Sub

    Private Sub bPlay_Click(sender As Object, e As EventArgs) Handles bPlay.Click
        ClearStatus()
        If PlayMode = PlayMode_play Then
            _mp.Pause()
            PlayMode = PlayMode_stop
        Else
            _mp.SetRate(1)
            _mp.Play()
            bTrackChange = False
            PlayMode = PlayMode_play

        End If
        SetPlayBtnTexts(bPlay)
    End Sub
    Private Sub bB1_Click(sender As Object, e As EventArgs) Handles bB1.Click
        If Not PlayMode = PlayMode_stop Then
            _mp.Pause()
            PlayMode = PlayMode_stop
            SetPlayBtnTexts(bPlay)
        End If
        If TrackBar1.Value > 0 Then TrackBar1.Value -= 1

    End Sub

    Private Sub bF1_Click(sender As Object, e As EventArgs) Handles bF1.Click
        ClearStatus()
        If Not PlayMode = PlayMode_stop Then
            _mp.Pause()
            PlayMode = PlayMode_stop
            SetPlayBtnTexts(bPlay)
        End If
        If TrackBar1.Value < TrackBar1.Maximum Then TrackBar1.Value += 1
    End Sub
    Private Sub TrackBar1_ValueChanged(sender As Object, e As EventArgs) Handles TrackBar1.ValueChanged
        Dim timeSpan As TimeSpan = TimeSpan.FromSeconds(TrackBar1.Value)
        Dim timeFormat As String = timeSpan.ToString("h\:mm\:ss")

        _WantedPosition = TrackBar1.Value / TrackBar1.Maximum

    End Sub
    Sub setTrackbarValue(time As Double)
        TrackBar1.Value = 2 * time
    End Sub

    Private Sub PlayMedia_Click(sender As Object, e As EventArgs) Handles PlayMedia.Click
        ClearStatus()
        ChangeViewVideo(LVMediaList, PostVideoList_Info)
    End Sub

    Private Sub btnOverlayPlay_Click(sender As Object, e As EventArgs) Handles btnOverlayPlay.Click
        ClearStatus()
        ChangeViewVideo(LV_Overlaylist, PostOverlayList_Info)
    End Sub

    Private Sub btnOutputFile_Click(sender As Object, e As EventArgs) Handles btnOutputFile.Click

        ClearStatus()
        If SaveFileDialogOutput.ShowDialog() = DialogResult.OK Then
            txtOutFilename.Text = SaveFileDialogOutput.FileName
        End If
    End Sub
    Private Sub MakeAudioFile()


    End Sub
    Private Async Function MakeOutputs() As Task
        Dim HasAudio As Boolean
        Dim i As Integer
        If Not txtOutFilename.Text = "" Then
            TmpFilenameWOE = Path.GetFileNameWithoutExtension(txtOutFilename.Text) + "_tmp"
            TmpPath = Path.GetDirectoryName(txtOutFilename.Text)
            TmpExt = Path.GetExtension(txtOutFilename.Text)
            TmpFilename = Path.Combine(TmpPath, TmpFilenameWOE + TmpExt)
            ProgressBar1.Visible = True
            PostImgToVideoFunc = "OutputVideo"
            StrArg = MakeFFOutArg("")
            'StrOut = $" -c:v libx264 -crf {My.Settings.ffmpegCRF} -c:a copy -r 25 -preset {My.Settings.ffmpegPreset} -pix_fmt yuvj420p -y ""{TmpFilename}"""
            StrOut = $" -c:v libx264 -crf {My.Settings.ffmpegCRF} -c:a aac -ac 2 -ar 48000 -b:a 128k -r 25 -preset {My.Settings.ffmpegPreset} -pix_fmt yuvj420p -y ""{TmpFilename}"""
            StatusTxt.Text = Texts.StatusMakingVideo
            FFImgToVideoArg = "-loglevel " + FFLogLevel + " -filter_complex """ + StrArg + """" + StrOut
            RemainingTimeObj.StartTime(_TotalDuration)
            lbStatusProgress.Text = ""
            RemainingTimer.Start()

            Await RunFFmpegCommand(FFImgToVideoArg)
            'While Not ProcessEnded
            '    Thread.Sleep(100)
            '    Me.Refresh()
            'End While
            ProgressBar1.Visible = False
            If FFError Then Return

            'AUDIO FILE
            HasAudio = False
            For Each prop In PostVideoList_Prop
                If prop.bHasAudio Then
                    HasAudio = True
                End If
            Next
            If HasAudio Then
                TmpAudFilename = Path.Combine(TmpPath, TmpFilenameWOE + ".aac")
                PostImgToVideoFunc = "OutputVideo"
                StrOut = FF_Func.FF_audio_input(PostVideoList_Info, PostVideoList_Prop)
                If PostVideoList_Prop.Count > 1 Then ' no fade filter when 0 or 1 video
                    StrFilter = $" -filter_complex ""{FF_Func.FF_audio_afade(PostVideoList_Prop)}"""
                Else
                    StrFilter = ""
                End If
                StrOut += $"{StrFilter} -c:a aac -y ""{TmpAudFilename}"""

                StatusTxt.Text = Texts.StatusMakingAudio
                ProgressBar1.Visible = True
                FFImgToVideoArg = "-loglevel " + FFLogLevel + StrOut
                RemainingTimeObj.StartTime(_TotalDuration)
                lbStatusProgress.Text = ""
                RemainingTimer.Start()
                Await RunFFmpegCommand(FFImgToVideoArg)
                ProgressBar1.Visible = False
                If FFError Then Return
                StatusTxt.Text = Texts.StatusAudioReady

                'MERGE AUDIO and VIDEO
                FFImgToVideoArg = $"-loglevel {FFLogLevel} -i ""{TmpFilename}"" -i ""{TmpAudFilename}"" -map 0:v -map 1:a -c copy -y ""{txtOutFilename.Text}"""
                StatusTxt.Text = Texts.StatusMakingVideo
                RemainingTimeObj.StartTime(_TotalDuration)
                lbStatusProgress.Text = ""
                RemainingTimer.Start()
                Await RunFFmpegCommand(FFImgToVideoArg)
                ProgressBar1.Visible = False
                If FFError Then Return
                StatusTxt.Text = Texts.StatusVideoReady
            Else
                FileSystem.Rename(TmpFilename, txtOutFilename.Text)
                StatusTxt.Text = Texts.StatusVideoReady
            End If
        Else
                MsgBox(Texts.ErrNoOutpFile)
        End If
    End Function

    Private Async Sub btnRunMakeVideo_Click(sender As Object, e As EventArgs) Handles btnRunMakeVideo.Click
        ClearStatus()
        Await MakeOutputs()

    End Sub

    Private Sub btnAddMusic_Click(sender As Object, e As EventArgs) Handles btnAddMusic.Click
        ClearStatus()
        Dim frmAddAudio As New AddAudioform(txtOutFilename.Text)
        frmAddAudio.Show()
    End Sub

    Private Sub TrackBar1_MouseDown(sender As Object, e As MouseEventArgs) Handles TrackBar1.MouseDown
        bMouseDown = True
        If Not PlayMode = PlayMode_stop Then
            _mp.Pause()
            PlayMode = PlayMode_stop
            SetPlayBtnTexts(bPlay)
        End If
    End Sub

    Private Sub TrackBar1_MouseUp(sender As Object, e As MouseEventArgs) Handles TrackBar1.MouseUp
        bMouseDown = False
        bTrackChange = True
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        bTrackChange = True
        'lblVideoPos.Text = TrackBar1.Value
    End Sub
    Private Sub Timer1_Tick_1(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Not IsNothing(_mp.Media) Then
            If _mp.Media.Duration > 0 Then
                _mp.Play()
                _mp.Pause()
                Timer1.Stop()
            End If

        End If
    End Sub
    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Dim a As Double
        If Not IsNothing(_mp.Media) Then
            If PlayMode = PlayMode_stop Then

                a = Math.Abs(_WantedPosition - _mp.Position)
                If a > 0.0001 Then
                    _mp.Position = _WantedPosition
                    VideoView1.Refresh()
                End If
            Else
                'Dim Position As Integer = TrackBar1.Value
                If Math.Round(_mp.Position * TrackBar1.Maximum) <> TrackBar1.Value Then
                    TrackBar1.Value = Math.Round(_mp.Position * TrackBar1.Maximum)

                End If
            End If
            Dim timeSpan As TimeSpan = TimeSpan.FromSeconds(Math.Round(_mp.Position * _mp.Media.Duration / 1000))
            Dim timeFormat As String = timeSpan.ToString("h\:mm\:ss")
            lblVideoPos.Text = timeFormat
        End If
    End Sub
    Private Sub MediaPlayer_EncounteredError(sender As Object, e As EventArgs)
        ' Handle the encountered error
        MsgBox("fejl")
        ' Display error message or perform error handling
    End Sub
    Private Sub SetPlayBtnTexts(Clicked_Button As Button)

        bFForward.Text = ">>"
        bPlay.Text = ">"
        bSlow.Text = "1/2x >"

        If Not PlayMode = PlayMode_stop Then Clicked_Button.Text = "||"


    End Sub

    Private Sub ChangeViewVideo(lv As ListView, videolist As List(Of clsFFMPegProbe))
        If lv.SelectedItems.Count > 0 Then
            Dim index As Integer = lv.SelectedIndices(0)
            Dim vid As clsFFMPegProbe = videolist(index)
            Vmedia = New Media(_libVLC, vid.Videofilename)
            _mp.Media = Vmedia
            TrackBar1.Maximum = vid.duration_sec * 2
            TrackBar1.TickFrequency = TrackBar1.Maximum / 50
            TrackBar1.LargeChange = TrackBar1.TickFrequency
            _mp.Play()
            _mp.Pause()
            Timer1.Start()
        End If

    End Sub
    Private Sub ClearStatus()
        StatusTxt.Text = ""
        ProgressBar1.Visible = False
        lbStatusProgress.Text = ""
    End Sub
    Private Sub SetVideo()
        VideoInfo = New clsFFMPegProbe
        VideoInfo.StoreVideoProps(_VideoFileName)
        Dim vfile As String = _VideoFileName

        Dim Vmedia As Media
        If File.Exists(vfile) Then
            Vmedia = New Media(_libVLC, vfile)
            _mp.Media = Vmedia
            InitFileList(True)
        Else
            MsgBox(Texts.ErrNoFile + vfile)
        End If




        TrackBar1.Maximum = VideoInfo.duration_sec * 2
        TrackBar1.TickFrequency = TrackBar1.Maximum / 50
        TrackBar1.LargeChange = TrackBar1.TickFrequency
        _mp.Play()
        _mp.Pause()


    End Sub
End Class