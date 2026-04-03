Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports OHeadcamMapApp.My

Public Class ffplay
    Public WithEvents ffplay As Process = New Process()
    Private eventHandled As Boolean
    Private FfmpegFunc As New clsExtra
    Private Shared ProcessFinished, x, y As Boolean
    Public ffplay_Info As New ProcessStartInfo()
    Public Event Exited As EventHandler
    Delegate Sub UpdateTextBoxDelg(text As String, ProcessEnd As Boolean)
    Public Shared myDelegate As UpdateTextBoxDelg
    Public Shared myStream As New IO.MemoryStream
    Public WorkingDir, Appfolder As String
    Public Shared outstr As String
    Public ffplay_winhandle As IntPtr
    Private FFFunc As New clsExtra
    Dim imgout As Bitmap
    Declare Auto Function MoveWindow1 Lib "user32" (ByVal hWnd As IntPtr, ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal bRepaint As Boolean) As Boolean
    Declare Auto Function SetParent Lib "user32" (ByVal hWndChild As IntPtr, ByVal hWndNewParent As IntPtr) As IntPtr
    Declare Auto Function FindWindow Lib "user32" (ByVal zero As IntPtr, ByVal lpWindowName As String) As IntPtr
    Public Declare Function MoveWindow Lib "user32.dll" (ByVal hwnd As IntPtr, ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal bRepaint As Boolean) As Boolean
    Public Declare Function SetWindowPos Lib "user32" (ByVal hWnd As IntPtr, ByVal hWndInsertAfter As IntPtr, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal uFlags As Integer) As Boolean
    Public Declare Function SetForegroundWindow Lib "user32.dll" (ByVal hwnd As Integer) As Integer
    Public Declare Function TranslateMessage Lib "user32.dll" (ByRef lpMsg As MSG) As Integer

    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Public Shared Function PostMessage(hWnd As IntPtr, Msg As UInt32, wParam As IntPtr, lParam As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Public Shared Function FindWindow2(ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Public Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
    End Function

    Public Const KEYEVENTF_EXTENDEDKEY As Integer = &H1
    Public Const KEYEVENTF_KEYUP As Integer = &H2
    Public Const VK_SPACE As Integer = &H20

    <StructLayout(LayoutKind.Sequential)>
    Public Structure INPUT
        Public type As Integer
        Public ki As KEYBDINPUT
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Public Structure KEYBDINPUT
        Public wVk As Short
        Public wScan As Short
        Public dwFlags As Integer
        Public time As Integer
        Public dwExtraInfo As IntPtr
    End Structure

    Public Declare Function SendInput Lib "user32.dll" (ByVal nInputs As Integer, ByVal pInputs As INPUT(), ByVal cbSize As Integer) As Integer

    Public Const WM_KEYDOWN As Integer = &H100
    Public Const WM_KEYUP As Integer = &H101
    Private Const WM_RBUTTONDOWN As Integer = &H204
    Private Const WM_RBUTTONUP As Integer = &H205
    Private FFArg As String
    Public Sub New(Optional Arg As String = "")

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        FFArg = Arg
    End Sub

    Public Structure MSG
        Public hwnd As Integer
        Public message As Integer
        Public wParam As Integer
        Public lParam As Integer
        Public time As Integer
        Public pt As Integer
    End Structure


    Function Play(arg As String, x As Integer, y As Integer, width As Integer, height As Integer, Optional WinHndl As Boolean = True, Optional WindowTitle As String = "", Optional TimeCode As Boolean = True)
        Dim Windowh As IntPtr
        'If Not IsNothing(ffplay) Then
        'Try
        'If Not ffplay.HasExited Then
        'ffplay.Kill()
        'ClearProcess()
        'End If
        'Catch ex As Exception
        'End Try
        'End If

        Dim processes2() As Process = Process.GetProcessesByName("ffplay")
        If processes2.Length > 0 Then
            For Each p As Process In processes2
                'If p.MainWindowTitle <> "" Then
                p.Kill()

                'End If
            Next

        End If
        ClearProcess()
        myDelegate = New UpdateTextBoxDelg(AddressOf UpdateTextBox)
        ffplay_Info.WorkingDirectory = WorkingDir
        ffplay.Refresh()
        ffplay_Info.FileName = "ffplay.exe" ' Process filename
        If WinHndl Then
            ffplay_Info.Arguments = arg ' Process arguments
        Else
            Tc = ";[out]drawtext=text='%{pts\:hms}':x=(w-tw)/2:y=h-th-50:fontsize=50:fontfile=Roboto-Bold.ttf:fontcolor=white"
            If TimeCode Then
                eparam = Tc
            Else
                eparam = ""
            End If
            ffplay_Info.Arguments = "-x " + width.ToString + " -window_title """ + WindowTitle + """ -f lavfi -i """ + arg + eparam + """"
        End If
        ffplay_Info.CreateNoWindow = True ' Show or hide the process Window
        ffplay_Info.UseShellExecute = False ' Don't use system shell to execute the process
        ffplay_Info.RedirectStandardOutput = True '  Redirect (1) Output
        ffplay_Info.RedirectStandardError = True ' Redirect non (1) Output
        ffplay_Info.RedirectStandardInput = True
        ffplay.EnableRaisingEvents = True ' Raise events

        ffplay.StartInfo = ffplay_Info
        AddHandler ffplay.OutputDataReceived,
                            AddressOf NetOutputDataHandler
        AddHandler ffplay.ErrorDataReceived,
                            AddressOf NetErrorDataHandler
        AddHandler ffplay.Exited, AddressOf ffplay_Exited

        outstr = ""

        ffplay.Start() ' Run the process NOW
        ffplay.BeginOutputReadLine()
        ffplay.BeginErrorReadLine()
        'Thread.Sleep(1000)
        ' Get the handle of the window
        'ffplay_winhandle = ffplay.MainWindowHandle
        'ffplay.WaitForInputIdle()

        ' Get the handle of the window
        If WinHndl Then
            ffplay_winhandle = ffplay.MainWindowHandle
            maxAttempts = 20
            attempt = 0

            While attempt < maxAttempts
                Try
                    ' Get the handle of the window
                    ffplay_winhandle = ffplay.MainWindowHandle
                    If ffplay_winhandle <> IntPtr.Zero Then
                        Exit While ' Break the loop if the handle exists
                    End If

                Catch ex As Exception
                    ' Handle the exception if the handle is not available yet

                End Try

                attempt += 1
                Threading.Thread.Sleep(50)
            End While
            ' Set the parent of the FFplay window to the PictureBox
            SetParent(ffplay_winhandle, Me.PictureBox1.Handle)



            ' SetParent(PictureBox2.Handle, ffplay.MainWindowHandle)
            MoveWindow(ffplay_winhandle, 0, 0, width, height, True)


        End If

        'SetForegroundWindow(ffplay.MainWindowHandle)
        ProcessFinished = False
        Do
            Thread.Sleep(50)
            Application.DoEvents()
        Loop While Not ProcessFinished
        ClearProcess()
    End Function
    Private Sub ffplay_Exited(ByVal sender As Object,
            ByVal e As System.EventArgs) Handles ffplay.Exited


        'myDelegate("", True)



        ProcessFinished = True
        eventHandled = True
    End Sub
    Private Shared Sub NetErrorDataHandler(sendingProcess As Object,
     errLine As DataReceivedEventArgs)

        ' Write the error text to the file if there is something to
        ' write and an error file has been specified.
        'If Me.InvokeRequired = True Then
        'Me.Invoke(myDelegate, outLine.Data)
        ' Else
        If Not String.IsNullOrEmpty(errLine.Data) Then

            'OutStr = OutStr + " err: " + errLine.Data
            outstr += errLine.Data
            'Mainform.txtResult.Text = OutStr
            'myDelegate(outstr, False)
            'UpdateTextBox(OutStr)
        End If
        ' End If
    End Sub
    Private Shared Sub NetOutputDataHandler(sendingProcess As Object,
          outLine As DataReceivedEventArgs)

        'Dim reader As New BinaryReader(sendingProcess.getoutputstream)

        ' If Me.InvokeRequired = True Then

        ' Else
        ' Collect the net view command output.
        If Not String.IsNullOrEmpty(outLine.Data) Then

            ' Add the text to the collected output.
            'OutStr = OutStr + " outp:" + outLine.Data
            'myStream.Write(uniEncoding.GetBytes(outLine.Data), 0, outLine.Data.Length)
            'OutStr = OutStr + outLine.Data
            'Mainform.txtResult.Text = OutStr
            'myDelegate(outLine.Data, False)
            'UpdateTextBox(OutStr)
        End If
        ' End If
    End Sub

    Private Sub btnVideoSelect_Click(sender As Object, e As EventArgs) Handles btnVideoSelect.Click
        Dim folder, fn As String, pos As Integer
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            txtVideofile.Text = OpenFileDialog1.FileName
            pos = InStrRev(OpenFileDialog1.FileName, "\")
            fn = Strings.Right(OpenFileDialog1.FileName, Len(OpenFileDialog1.FileName) - pos)
            folder = Strings.Left(OpenFileDialog1.FileName, pos)
        End If


    End Sub
    Public Sub SendKeyToWindow(ByVal windowHandle As IntPtr, ByVal key As Keys)
        Dim x As Integer = 100 ' X position of the mouse click
        Dim y As Integer = 100 ' Y position of the mouse click
        Dim lParam As Integer = (y << 16) Or x
        SendMessage(ffplay_winhandle, WM_RBUTTONDOWN, 0, lParam)
        SendMessage(ffplay_winhandle, WM_RBUTTONUP, 0, lParam)
        SendMessage(ffplay_winhandle, WM_KEYDOWN, New IntPtr(key), IntPtr.Zero)
        SendMessage(ffplay_winhandle, WM_KEYUP, New IntPtr(key), IntPtr.Zero)
    End Sub




    Public Sub UpdateTextBox(text As String, ProcessEnd As Boolean)



        If Not ProcessEnd Then
            'If CurrentLog.Length > 1000 Then CurrentLog = ""
            'CurrentLog += text + vbCrLf

            'Me.BeginInvoke(Sub() Me.txtRun_CurrentOut.Text = text + vbCrLf)
        Else

            ProcessFinished = True
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Appfolder = Application.ExecutablePath
        'Appfolder = Appfolder.Substring(0, Application.ExecutablePath.LastIndexOf("\")) + "\"
        'WorkingDir = Appfolder
        Dim arg, arg2 As String
        'Get the handle of the PictureBox control
        arg2 = "-hwnd " & PictureBox1.Handle.ToInt32() & " -noborder " & txtVideofile.Text
        Dim hwnd As IntPtr = PictureBox1.Handle
        fn = FFFunc.FFPlay_Filename(txtVideofile.Text)

        'scale=" & PictureBox1.Width & ":" & PictureBox1.Height & " -left " & PictureBox1.Left & " -top " & PictureBox1.Top
        'arg = "-noborder -hide_banner -autoscale 1 -i """ & txtVideofile.Text & """ -vf " & MainFormo.ExtraFunc.FFMpeg_ScalePadFHD(PictureBox1.Width) & " -left " & PictureBox1.Left & " -top " & PictureBox1.Top
        If Not FFArg = "" Then
            param = FFArg
        Else
            param = "movie=" + fn + "[out]"
        End If
        arg = "-noborder -hide_banner " + "-f lavfi -i """ + param + ";[out]scale=" + PictureBox1.Width.ToString + ":-1"""
        MsgBox(arg)
        Play(arg, PictureBox1.Left, PictureBox1.Top, PictureBox1.Width, PictureBox1.Height)
    End Sub

    Function GenerateFfplayArguments(videoList As List(Of String), musicFilename As String) As String
        Dim sb As New StringBuilder()

        For Each videoFilename In videoList
            sb.Append($"movie={videoFilename},fade=out:st=3:d=1:alpha=1[fade];")
        Next

        sb.Append("nullsrc=size=1920x1080,fade=in:st=0:d=1:alpha=1[fadein];")

        Dim overlayInputs As New List(Of String)
        For i = 0 To videoList.Count - 1
            overlayInputs.Add($"[fadein][{i}:v]overlay=tmp{i}")
        Next

        Dim overlayString As String = String.Join(";", overlayInputs)

        sb.Append($"{overlayString};")

        sb.Append($"[{videoList.Count - 1}:v]overlay,format=yuv420p[main];")

        sb.Append($"amovie={musicFilename}:loop=0,apad=pad_len=10000:pad_dur=0[am];")

        sb.Append("[main][am]overlay[final] -loop 0")

        Return sb.ToString()
    End Function





    Public Sub ClearProcess()
        Try
            ffplay.CancelOutputRead()
            ffplay.CancelErrorRead()
            'My_Process.CloseMainWindow()
            ffplay.Close()
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

    End Sub

    Public Sub StartFFplaySimple()
        Dim ffplayPath As String = "path\to\ffplay.exe" ' Replace with the actual path to ffplay.exe
        Dim arguments As String = "-x 800 -y 600 -showmode 2 -window_title ""My FFplay Window"" input.mp4"

        Dim startInfo As New ProcessStartInfo()
        startInfo.FileName = ffplayPath
        startInfo.Arguments = arguments
        startInfo.UseShellExecute = False
        startInfo.CreateNoWindow = False

        Dim ffplayProcess As Process = Process.Start(startInfo)

        ' Optionally, you can wait for the process to exit, if needed.
        ' ffplayProcess.WaitForExit()
    End Sub


    Private Sub ffplay_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            ffplay.Kill()
        Catch ex As Exception

        End Try

    End Sub
End Class