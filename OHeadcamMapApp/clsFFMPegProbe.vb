Imports System.Globalization
Imports System.IO
Imports System.Text
Public Class clsFFMPegProbe



    Dim thisLock As New Object

    Public ArgStr As String
    Public FFDir As String
    Public FFprobeproc As String = "ffprobe.exe"
    Public FFMpegproc As String = "ffmpeg.exe"
    ' Define static variables shared by class methods.
    Private Shared StdOutputStr As StringBuilder = Nothing
    Private Shared ErrOutputStr As StringBuilder = Nothing
    Public SetInputVideoFName, SetInput2FName, Input2FName As String
    Public InputVideoFName As String
    Public SetFrameTime, Inp2Time As String
    Public SetInp2Time, SetInp2Speed As Single
    Public Shared texta As String
    Public SetFFparams As String
    Public FFparams As String
    Public FrameTime As String
    Public FrameImg As Image
    Public FrameImgFName As String
    Public Probestr As String
    Public RunResult As String
    Public VideoProbetxt As String
    Public Videofilename As String
    Public duration_sec As Single
    Public no_frames As Integer
    Public framerate As Single
    Public Width As Integer
    Public Height As Integer

    Public Sub New()
        FFDir = Application.ExecutablePath
        FFDir = FFDir.Substring(0, Application.ExecutablePath.LastIndexOf("\")) + "\"
    End Sub

    Function StoreVideoProps(filename As String) As Boolean
        Dim tmp, t2 As String, i As Integer

        If Not String.IsNullOrWhiteSpace(filename) Then
            If Not File.Exists(filename) Then
                'MsgBox("Cant find file property: " + filename)
                Return False
            Else
                Videofilename = filename

                VideoProbetxt = ProbeVideo()
                If Not InStr(VideoProbetxt, "Error") > 0 And Not String.IsNullOrEmpty(VideoProbetxt) Then
                    dstr = GetVideoProperty(VideoProbetxt, "duration") '.Replace(".", ",")
                    'If IsNumeric(dstr) Then duration_sec = CSng(dstr)
                    If dstr.Contains(".") Then
                        Dim parts() As String = dstr.Split("."c)
                        Dim Wholepart As Single = 0
                        Wholepart = CSng(parts(0))
                        Dim fractionalStr As String = "0." & parts(1)
                        Dim fractionalPart As Single = 0
                        Single.TryParse(fractionalStr, Globalization.NumberStyles.Any, Globalization.CultureInfo.InvariantCulture, fractionalPart)

                        ' Læg sammen
                        duration_sec = wholePart + fractionalPart
                    Else
                        duration_sec = CSng(dstr)
                    End If
                    fstr = GetVideoProperty(VideoProbetxt, "nb_frames")
                    If IsNumeric(fstr) Then no_frames = CInt(fstr)
                    tmp = GetVideoProperty(VideoProbetxt, "Width")

                    Width = CInt(GetVideoProperty(VideoProbetxt, "width"))
                    Height = CInt(GetVideoProperty(VideoProbetxt, "height"))


                    tmp = GetVideoProperty(VideoProbetxt, "avg_frame_rate")
                    i = InStr(tmp, "/")
                    t2 = tmp + ": " + tmp.Substring(0, i - 1) + " / " + Right(tmp, tmp.Length - i)
                    ' MsgBox(duration_sec)
                    If IsNumeric(tmp.Substring(0, i - 1)) Then
                        framerate = CInt(tmp.Substring(0, i - 1)) / CInt(Right(tmp, tmp.Length - i))
                    End If
                    Return True
                    Else
                        Return False
                End If
            End If
        End If
    End Function
    Function GetVideoProperty(ByRef ffprobetxt As String, Title As String) As String
        Dim i, codecpos, i2 As Integer
        Dim tmpstr As String
        i2 = InStr(ffprobetxt, "codec_name")
        codecpos = InStr(ffprobetxt, "codec_name=aac")
        If i2 >= codecpos And Not codecpos = 0 Then ' if sound is before video
            codecpos = InStr(ffprobetxt.Substring(codecpos + 14), "codec_name") ' dont find properties for sound(aac), find next codec
            tmpstr = ffprobetxt.Substring(codecpos + 10)

        Else
            tmpstr = ffprobetxt
        End If
        i = InStr(tmpstr, Title + "=")
        If i > 0 Then
            i = InStr(i, tmpstr, "=")
            GetVideoProperty = tmpstr.Substring(i, InStr(i, tmpstr, Chr(13)) - i)
        Else
            GetVideoProperty = ""
        End If
    End Function
    Public Function HasAudio() As Boolean
        If InStr(VideoProbetxt, "codec_type=audio") > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function gettexta() As String
        gettexta = texta
    End Function
    Public Function IsChanged() As Boolean
        If SetInputVideoFName <> InputVideoFName Or SetFFparams <> FFparams Or SetFrameTime <> FrameTime Or SetInp2Time <> Inp2Time Then
            IsChanged = True
        Else
            IsChanged = False
        End If
    End Function
    Public Function IsOK(Optional IsFrame As Boolean = False) As Boolean
        Dim tmpOK As Boolean = False
        If FFDir <> "" And SetInputVideoFName <> "" Then
            tmpOK = True
            If IsFrame And (SetFrameTime = "" Or FrameImgFName = "") Then
                tmpOK = False
            End If

        End If
        IsOK = tmpOK
    End Function
    Public Function ProbeVideo() As String
        SyncLock thisLock
            ' If IsOK() Then
            InputVideoFName = Videofilename
            ArgStr = " -v error -show_format -show_streams " + ControlChars.Quote + InputVideoFName + ControlChars.Quote
            Probestr = RunTheScript(FFprobeproc)
            ' End If
        End SyncLock
        ProbeVideo = Probestr
    End Function
    Function GetTimeStr() As String

        ' Create a TimeSpan object from the number of seconds
        Dim timeSpan As New TimeSpan(0, 0, duration_sec)

        ' Create a time string in the format "h:mm:ss"
        Dim timeString As String = $"{timeSpan.Hours}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}"

        ' Display the time string
        Return timeString

    End Function
    Public Function MakeFrame() As String
        Dim Tmp, overlayStr As String
        Dim Maint, inp2time, inp2speed As Single
        texta += "In MF " + Now().ToString("mm:ss.fff") + vbNewLine
        SyncLock thisLock
            If IsOK(True) Then
                FFparams = SetFFparams
                FrameTime = SetFrameTime
                InputVideoFName = SetInputVideoFName

                Maint = Convert.ToSingle(SetFrameTime, CultureInfo.InvariantCulture)
                If SetInp2Speed = 0 Then
                    inp2speed = 1
                Else
                    inp2speed = SetInp2Speed
                End If
                inp2time = SetInp2Time + Maint * inp2speed 'frametime+ offset for video 2 input
                overlayStr = " -ss " + inp2time.ToString("N2", CultureInfo.InvariantCulture)

                If SetInput2FName <> "" Then
                    overlayStr += " -i " + ControlChars.Quote + SetInput2FName + ControlChars.Quote
                Else
                    overlayStr = ""
                End If
                texta += "StartScriptMF " + Now().ToString("mm:ss.fff") + vbNewLine
                ArgStr = " -v error -stats -ss " & FrameTime & " -i " + ControlChars.Quote + InputVideoFName + ControlChars.Quote + overlayStr + " -vframes 1" + FFparams & " -y " + FrameImgFName
                Tmp = RunTheScript(FFDir + FFMpegproc)
                texta += "EndscriptMF " + Now().ToString("mm:ss.fff") + vbNewLine
                imagefromFile()
                texta += "Imagefromfile end " + Now().ToString("mm:ss.fff") + vbNewLine
            Else
                RunResult = "Error in setup"
            End If
        End SyncLock
        MakeFrame = RunResult
    End Function
    Public Function RunTheScript(Process_name As String) As String

        Try

            Dim My_Process As New Process()
            Dim My_Process_Info As New ProcessStartInfo()
            My_Process_Info.WorkingDirectory = FFDir
            My_Process_Info.FileName = "" + Process_name + "" ' Process filename
            My_Process_Info.Arguments = ArgStr ' Process arguments
            My_Process_Info.CreateNoWindow = True ' Show or hide the process Window
            My_Process_Info.UseShellExecute = False ' Don't use system shell to execute the process
            My_Process_Info.RedirectStandardOutput = True '  Redirect (1) Output
            My_Process_Info.RedirectStandardError = True ' Redirect non (1) Output
            My_Process.EnableRaisingEvents = True ' Raise events
            My_Process.StartInfo = My_Process_Info
            AddHandler My_Process.OutputDataReceived, AddressOf OutputHandler
            AddHandler My_Process.ErrorDataReceived, AddressOf ErrorOutHandler
            StdOutputStr = New StringBuilder()
            ErrOutputStr = New StringBuilder()
            My_Process.Start() ' Run the process NOW
            My_Process.BeginOutputReadLine()
            My_Process.BeginErrorReadLine()


            My_Process.WaitForExit() ' Wait X ms to kill the process (Default value is 999999999 ms which is 277 Hours)

            Dim ERRORLEVEL = My_Process.ExitCode ' Stores the ExitCode of the process
            ''If Not ERRORLEVEL = 0 Then Return False ' Returns the Exitcode if is not 0


            'Dim Process_Output As String = My_Process.StandardError.ReadToEnd() ' Stores the Error Output (If any)

            RunResult = "Std: " + StdOutputStr.ToString + Chr(13)
            If ErrOutputStr.ToString <> "" Then
                RunResult = RunResult + "Error: " + ErrOutputStr.ToString ' Returns the StandardOutput (if any)
            End If

        Catch ex As Exception
            MsgBox("Run process err: " + ex.Message)
            RunResult = "" ' Returns nothing if the process can't be found or started.
        End Try



        Return RunResult ' Returns True if Read_Output argument is set to False and the process finished without errors.

    End Function
    Private Shared Sub OutputHandler(sendingProcess As Object,
         outLine As DataReceivedEventArgs)

        ' Collect the sort command output.
        If Not String.IsNullOrEmpty(outLine.Data) Then
            ' Add the text to the collected output.
            StdOutputStr.Append(outLine.Data + Chr(13))
        End If
    End Sub
    Private Shared Sub ErrorOutHandler(sendingProcess As Object,
         outLine As DataReceivedEventArgs)

        ' Collect the sort command output.
        If Not String.IsNullOrEmpty(outLine.Data) Then
            ' Add the text to the collected output.
            ErrOutputStr.Append(outLine.Data + Chr(13))
        End If
    End Sub
    Public Sub imagefromFile()
        Dim fs As System.IO.FileStream
        Dim tmpimg As Image
        Try
            ' Specify a valid picture file path on your computer.
            fs = New System.IO.FileStream(FrameImgFName, IO.FileMode.Open, IO.FileAccess.Read)
            tmpimg = System.Drawing.Image.FromStream(fs)
            fs.Close()
            FrameImg = tmpimg
        Catch ex As Exception
            MsgBox("Image from file err: " + ex.Message)

        End Try
    End Sub

    Public Function IsImage() As Boolean
        Dim validExtensions As String() = {".jpg", ".jpeg", ".png", ".bmp"}
        Dim fileExtension As String = Path.GetExtension(Videofilename).ToLower()
        Return validExtensions.Contains(fileExtension)
    End Function
End Class
