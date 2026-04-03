Imports System.Globalization
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Threading

Namespace FilterNameSpace
    Public Enum Filters
        Rotate = 0
        ColorImpr = 1
        Sharp = 2
        EQ = 3
        Hue = 4
        Noise = 5
        LensDist = 6
    End Enum
End Namespace
Public Class clsVideoImageGenerator

    Public ffmpegPath As String = "ffmpeg.exe"
    Public ffprobePath As String = "ffprobe.exe"
    Private Shared _videoPath As String
    Private Shared _outputImagePath As String
    Private Shared _pictureBox As PictureBox
    Private _pictureboxwidth As Integer
    Public _videoDuration, _videoSecPrFrame As Double
    Public _videoNoFrames As Integer
    Public _videoWidth, _videoHeight As Integer
    Public _videoAspect As Single
    Public _videoCodec As String
    Public FFParam As String
    Public bByParam As Boolean = False
    Private Lock As Boolean = False
    Public CurrentTime, seektime As Double
    Private processfinished As Boolean = False
    Private Myprocess As New clsRunprocess(AddressOf UpdateTextBox)
    Private ProccessOutText As String
    Public _SliderMax As Integer
    Public FilterFuncs As New clsExtra
    Public FilterList As New List(Of String)
    Public FilterString As String
    Public aspectRatio As Single
    Public ImageWidth As Integer
    Public ImageHeight As Integer



    Public Sub New(ByVal videoPath As String, ByVal outputImagePath As String, ByVal pictureBox As PictureBox)
        _videoPath = videoPath
        _outputImagePath = outputImagePath
        _pictureBox = pictureBox
        GetVideoInfo(_videoPath)
        _pictureBox.Width = CInt(_pictureBox.Height * aspectRatio)
        _pictureBox.SizeMode = PictureBoxSizeMode.StretchImage
        Dim i As Integer
        For i = 1 To 8
            FilterList.Add("")
        Next

    End Sub

    Public Sub New(OutWidth As Integer, OutHeight As Integer, Duration As Single, ByVal outputImagePath As String, ByVal pictureBox As PictureBox)
        SetVideoInfo(OutHeight, OutWidth, Duration)
        _outputImagePath = outputImagePath
        _pictureBox = pictureBox
        _pictureBox.Width = CInt(_pictureBox.Height * aspectRatio)
        _pictureBox.SizeMode = PictureBoxSizeMode.StretchImage
        bByParam = True
    End Sub



    Public Sub ShowImage()
        Using stream As New FileStream(_outputImagePath, FileMode.Open, FileAccess.Read)
            Dim OrigImage As New Bitmap(Image.FromStream(stream))


            ' Set the picture box image
            _pictureBox.Image = OrigImage
            '_pictureBox.Image = Image.FromStream(stream)
        End Using
    End Sub
    Public Function UpdateImageDelta(IsPlus As Boolean, Times As Integer) As Integer
        If IsPlus Then
            CurrentTime += _videoSecPrFrame * Times
            If CurrentTime > _videoDuration Then CurrentTime = _videoDuration
        Else
            CurrentTime -= _videoSecPrFrame * Times
            If CurrentTime < 0 Then CurrentTime = 0
        End If
        If bByParam Then
            GenerateImageByParam(CurrentTime, FFParam)
        Else
            GenerateImage(CurrentTime)
        End If

        Return CalcSliderValue()
    End Function

    Public Sub UpdateImageByParam(ByVal sliderValue As Integer, Optional force As Boolean = False)
        Dim seekToTime As Double = CalcSliderTimer(sliderValue)
        If Not seekToTime = CurrentTime Or force Then
            CurrentTime = seekToTime
            GenerateImageByParam(seekToTime, FFParam)
        End If
    End Sub
    Public Sub UpdateImage(ByVal sliderValue As Integer, Optional force As Boolean = False)

        Dim seekToTime As Double = CalcSliderTimer(sliderValue)
        If Not seekToTime = CurrentTime Or force Then
            CurrentTime = seekToTime
            GenerateImage(seekToTime)
        End If
    End Sub
    Private Function RunProcess(exe As String, arg As String) As String

        If Not Lock Then
            Lock = True
            processfinished = False
            Myprocess.Run_Process2(exe, arg) 'TESTER
            'MsgBox("after run process2")
            'Cursor = Cursors.WaitCursor
            If Myprocess.My_Process.HasExited Then
                'MsgBox("Proc exited before")
            End If

            Do
                Thread.Sleep(100)
                Application.DoEvents()
            Loop While Not processfinished
            Myprocess.ClearProcess()
            Lock = False
        End If

        Return ProccessOutText
    End Function
    Public Sub UpdateTextBox(text As String, ProcessEnd As Boolean)

        If Not ProcessEnd Then
            ProccessOutText = text

        Else

            processfinished = True
        End If
    End Sub

    Public Sub SetVideoInfo(Height As Integer, Width As Integer, Duration As Single)
        _videoHeight = Height
        _videoWidth = Width
        _videoDuration = Duration
        _videoNoFrames = Duration * 25
        _videoCodec = "H264"
        aspectRatio = CSng(_videoWidth / _videoHeight)
        _videoSecPrFrame = _videoDuration / _videoNoFrames
    End Sub

    Private Function GetVideoInfo(ByVal videoPath As String) As Boolean
        Dim output As String
        output = RunProcess(ffprobePath, $"-i ""{videoPath}"" -show_entries stream=codec_name,nb_frames,duration,width,height -select_streams v -v quiet -of csv=""p=0""")
        Dim lines() As String = output.Split(","c) 'c=Character
        Dim Codec As String = lines(0).Trim()
        Dim Width As String = lines(1).Trim()
        Dim height As String = lines(2).Trim()
        Dim durationLine As String = lines(3).Trim
        Dim framesLine As String = lines(4).Trim

        _videoHeight = Integer.Parse(height)
        _videoWidth = Integer.Parse(Width)
        _videoDuration = Double.Parse(durationLine, CultureInfo.InvariantCulture)
        _videoNoFrames = Integer.Parse(framesLine)
        _videoCodec = Codec
        aspectRatio = CSng(_videoWidth / _videoHeight)
        _videoSecPrFrame = _videoDuration / _videoNoFrames

    End Function





    Public Function ModifyFFmpegParameters(ByVal inputString As String) As String
        Dim modifiedString As String = inputString

        If String.IsNullOrEmpty(modifiedString) Then
            ' Case 1: String is empty, add -vf with eq filter (gamma=0)
            modifiedString = "-vf eq=gamma=0.80"
        ElseIf modifiedString.Contains("-vf") AndAlso Not modifiedString.Contains("eq") Then
            ' Case 2: String contains -vf but not the eq filter, add eq filter with gamma=0
            modifiedString = Regex.Replace(modifiedString, "-vf ", "-vf eq=gamma=0.80,")
        ElseIf modifiedString.Contains("-vf") AndAlso modifiedString.Contains("eq") Then
            ' Case 3: String contains both -vf and eq filter, subtract 1 from gamma filter
            modifiedString = Regex.Replace(modifiedString, "gamma=(\d+(?:\.\d{1,2})?)", AddressOf SubtractOneFromGamma)
        End If

        Return modifiedString
    End Function

    Public Function SubtractOneFromGamma(ByVal match As Match) As String
        Dim gammaValue As Double = Double.Parse(match.Groups(1).Value) / 100
        gammaValue -= 0.2

        Return "gamma=" & gammaValue.ToString("0.00", CultureInfo.InvariantCulture)
    End Function

    Public Sub GenerateImageByParam(ByVal timeInSeconds As Double, InpParam As String)
        Dim frameNumber As Integer = CInt(Math.Round(timeInSeconds / _videoDuration * _videoNoFrames))
        seektime = timeInSeconds
        FFParam = InpParam
        Dim arg As String = $"-nostdin -ss {timeInSeconds.ToString(CultureInfo.InvariantCulture)} -filter_complex ""{InpParam}"" -vcodec mjpeg -pix_fmt rgb24 -vframes 1 -q:v 2 -update 1 -y ""{_outputImagePath}"""
        RunProcess(ffmpegPath, arg)

        ShowImage()
    End Sub
    Public Sub GenerateImage(ByVal timeInSeconds As Double)
        Dim frameNumber As Integer = CInt(Math.Round(timeInSeconds / _videoDuration * _videoNoFrames))
        seektime = timeInSeconds
        '-vf ""select=eq(n\,{frameNumber})""
        'ffmpeg -nostdin - ss 410.06464 -i "F:\Videoer\TT280323\GX040829.MP4" -vcodec mjpeg -vframes 1 -q: v 2 - update 1 -y "a.jpg" -- -vf ""drawtext=fontfile=/path/to/font.ttf:fontsize=24:fontcolor=white:x=10:y=10:text='%{{pts\\:hms\\:0}}'""

        Dim Fstring As String
        If _videoCodec = "hevc" Then ' hevc h265 kræver en ændring i gamma, dette løses ved videogenerering men ikke ved billeder.
            Fstring = ModifyFFmpegParameters(FilterString)
        Else
            Fstring = FilterString
        End If

        Dim arg As String = $"-nostdin -ss {timeInSeconds.ToString(CultureInfo.InvariantCulture)} -i ""{_videoPath}"" {Fstring} -vcodec mjpeg -pix_fmt rgb24 -vframes 1 -q:v 2 -update 1 -y ""{_outputImagePath}"""
        RunProcess(ffmpegPath, arg)

        ShowImage()
    End Sub

    Public Function CalcSliderValue() As Integer
        CalcSliderValue = CInt((seektime / _videoDuration) * _SliderMax)
    End Function
    Public Function CalcSliderTimer(Slidervalue As Integer) As Double
        Dim Sseektime As Double = (Slidervalue / _SliderMax) * _videoDuration
        If Sseektime >= _videoDuration Then
            Sseektime -= 0.2
        End If
        Return Sseektime
    End Function
    Sub ResetFilter(FilterNo As Integer)
        FilterList(FilterNo) = ""
        UpdateFilter(-1, "")
    End Sub
    Function UpdateFilter(FilterNo As Integer, FilterIn As String) As String
        If Not FilterNo = -1 Then ' Update all if -1
            FilterList(FilterNo) = FilterIn
        End If

        Dim i As Integer
        FilterString = FilterList(0)
        For i = 1 To FilterList.Count - 1
            If Not FilterList(i) = "" Then
                If Not FilterString = "" Then
                    FilterString += "," + FilterList(i)
                Else
                    FilterString = FilterList(i)
                End If
            End If
        Next
        If Not FilterString = "" Then
            FilterString = "-vf " + "" + FilterString + "" + " "
        Else
            FilterString = ""
        End If

        Return FilterString
    End Function
    Private Function drawtext(inStr As String) As Boolean
        'ffmpeg -t 10 -i deshaked.mp4 -filter_complex "[0:v]drawtext=text='Hello, World!':fontfile='/Windows/Fonts/Arial.ttf':fontsize=24:fontcolor=white:x=50:y=50:start=5:duration=10" output_video.mp4
    End Function
End Class


