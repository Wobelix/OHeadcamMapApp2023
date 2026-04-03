Imports System.IO
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks

Public Class clsRunprocess
    ' Define static variables shared by class methods.

    Public WorkingDir As String
    Public WithEvents My_Process As New Process
    Private Shared logMutex As Mutex = New Mutex() ' Static = shared
    Private Shared buildLogStream As StreamWriter
    Private eventHandled As Boolean
    Private Shared OutStr As String = ""
    Public Event Exited As EventHandler
    Delegate Sub UpdateTextBoxDelg(text As String, ProcessEnd As Boolean)
    Public Shared myDelegate As UpdateTextBoxDelg
    Public Shared myStream As New IO.MemoryStream
    Public My_Process_Info As New ProcessStartInfo()
    Private img As Image

    Public Sub New(updateTextBoxDelg As UpdateTextBoxDelg) 'CallForm As MainForm)
        myDelegate = updateTextBoxDelg
        ' Ensure delegate invocations happen on UI thread via SynchronizationContext if available
        'AddHandler My_Process.Exited, AddressOf My_Process_Exited

    End Sub


    'Public Sub proc_OutputDataReceived(ByVal sender As Object, ByVal e As DataReceivedEventArgs)

    '    If Me.InvokeRequired = True Then
    '        Me.Invoke(myDelegate, e.Data)
    '    Else
    '        UpdateTextBox(e.Data)
    '    End If

    'End Sub

    Public Sub StopProcess()
        Try
            My_Process.Kill()
        Catch ex As Exception
            MsgBox("Tries to stop process: " + ex.Message)

        End Try
    End Sub

    Public Async Function Run_Process_Wait(ByVal Process_Name As String,
                         Optional Process_Arguments As String = Nothing,
                         Optional Read_Output As Boolean = False,
                         Optional Process_Hide As Boolean = False,
                         Optional Process_TimeOut As Integer = 999999999) As Task
        Dim NoWindows, Redirect As Boolean
        ' Returns True if "Read_Output" argument is False and Process was finished OK
        ' Returns False if ExitCode is not "0"
        ' Returns Nothing if process can't be found or can't be started
        ' Returns "ErrorOutput" or "StandardOutput" (In that priority) if Read_Output argument is set to True.
        NoWindows = True
        Redirect = True 'xx
        Try
            AddHandler My_Process.OutputDataReceived,
                        AddressOf NetOutputDataHandler
            AddHandler My_Process.ErrorDataReceived,
                    AddressOf NetErrorDataHandler
            AddHandler My_Process.Exited, AddressOf My_Process_Exited


            My_Process_Info.WorkingDirectory = WorkingDir
            My_Process.Refresh()
            My_Process_Info.FileName = Process_Name ' Process filename
            My_Process_Info.Arguments = Process_Arguments ' Process arguments
            My_Process_Info.CreateNoWindow = True ' Show or hide the process Window
            My_Process_Info.UseShellExecute = False ' Don't use system shell to execute the process
            My_Process_Info.RedirectStandardOutput = Redirect '  Redirect (1) Output
            My_Process_Info.RedirectStandardError = Redirect ' Redirect non (1) Output
            My_Process.EnableRaisingEvents = Redirect ' Redirect ' Raise events

            My_Process.StartInfo = My_Process_Info


            OutStr = ""
            'MsgBox("ProcesStart")
            'logMutex.ReleaseMutex()
            'MsgBox("ProcesStart")
            My_Process.Start() ' Run the process NOW



            My_Process.BeginOutputReadLine()
            My_Process.BeginErrorReadLine()
            Await Task.Run(Sub() My_Process.WaitForExit())
            'Stream = My_Process.StandardOutput.BaseStream
            ' img = Image.FromStream(Stream)
        Catch ex As Exception
            MsgBox(ex.Message)
            'Return False ' Returns nothing if the process can't be found or started.
        End Try

        'Return True ' Returns True if Read_Output argument is set to False and the process finished without errors.

    End Function
    Function Run_Process2(ByVal Process_Name As String,
                         Optional Process_Arguments As String = Nothing,
                         Optional Read_Output As Boolean = False,
                         Optional Process_Hide As Boolean = False,
                         Optional Process_TimeOut As Integer = 999999999) As Boolean
        Dim NoWindows, Redirect As Boolean
        ' Returns True if "Read_Output" argument is False and Process was finished OK
        ' Returns False if ExitCode is not "0"
        ' Returns Nothing if process can't be found or can't be started
        ' Returns "ErrorOutput" or "StandardOutput" (In that priority) if Read_Output argument is set to True.
        NoWindows = True
        Redirect = True 'xx
        Try
            AddHandler My_Process.OutputDataReceived,
                        AddressOf NetOutputDataHandler
            AddHandler My_Process.ErrorDataReceived,
                    AddressOf NetErrorDataHandler
            AddHandler My_Process.Exited, AddressOf My_Process_Exited
            'myDelegate = New UpdateTextBoxDelg(AddressOf MainForm.UpdateTextBox)

            'myDelegate("Opdaterer...", False)

            My_Process_Info.WorkingDirectory = WorkingDir
            My_Process.Refresh()
            My_Process_Info.FileName = Process_Name ' Process filename
            My_Process_Info.Arguments = Process_Arguments ' Process arguments
            My_Process_Info.CreateNoWindow = True ' Show or hide the process Window
            My_Process_Info.UseShellExecute = False ' Don't use system shell to execute the process
            My_Process_Info.RedirectStandardOutput = Redirect '  Redirect (1) Output
            My_Process_Info.RedirectStandardError = Redirect ' Redirect non (1) Output
            My_Process.EnableRaisingEvents = Redirect ' Redirect ' Raise events

            My_Process.StartInfo = My_Process_Info


            OutStr = ""
            'MsgBox("ProcesStart")
            'logMutex.ReleaseMutex()
            'MsgBox("ProcesStart")
            My_Process.Start() ' Run the process NOW



            My_Process.BeginOutputReadLine()
            My_Process.BeginErrorReadLine()

            'Stream = My_Process.StandardOutput.BaseStream
            ' img = Image.FromStream(Stream)
        Catch ex As Exception
            MsgBox(ex.Message)
            Return False ' Returns nothing if the process can't be found or started.
        End Try

        Return True ' Returns True if Read_Output argument is set to False and the process finished without errors.

    End Function

    Public Sub ClearProcess()
        Try
            My_Process.CancelOutputRead()
            My_Process.CancelErrorRead()
            My_Process.CloseMainWindow() 'Try to close the main window
            If Not My_Process.HasExited Then 'Check if the process has exited
                My_Process.WaitForExit(5000) 'Wait for up to 5 seconds for the process to exit
            End If
            If Not My_Process.HasExited Then 'Check again if the process has exited
                My_Process.Kill() 'If not, force it to exit immediately
            End If
        Catch ex As Exception
            'Handle any exceptions here
        End Try

    End Sub
    Private Sub My_Process_Exited(ByVal sender As Object,
            ByVal e As System.EventArgs) Handles My_Process.Exited


        myDelegate("Exited", True)
        'My_Process.CancelOutputRead()
        'My_Process.CancelErrorRead()
        'My_Process.CloseMainWindow()
        'My_Process.Close()

        OutStr = ""
        eventHandled = True
        'MsgBox("ProcesExitet")
    End Sub

    Private Shared Sub NetOutputDataHandler(sendingProcess As Object,
          outLine As DataReceivedEventArgs)
        Dim uniEncoding As New UnicodeEncoding()
        'Dim reader As New BinaryReader(sendingProcess.getoutputstream)

        ' If Me.InvokeRequired = True Then

        ' Else
        ' Collect the net view command output.
        ' logMutex.WaitOne()
        If Not String.IsNullOrEmpty(outLine.Data) Then

            ' Add the text to the collected output.
            'OutStr = OutStr + " outp:" + outLine.Data
            'myStream.Write(uniEncoding.GetBytes(outLine.Data), 0, outLine.Data.Length)
            OutStr = outLine.Data
            'Mainform.txtResult.Text = OutStr
            myDelegate(OutStr, False)
            'UpdateTextBox(OutStr)
        End If
        ' End If
        ' logMutex.ReleaseMutex()
    End Sub

    Private Shared Sub NetErrorDataHandler(sendingProcess As Object,
     errLine As DataReceivedEventArgs)
        ' logMutex.WaitOne()
        ' Write the error text to the file if there is something to
        ' write and an error file has been specified.
        'If Me.InvokeRequired = True Then
        'Me.Invoke(myDelegate, outLine.Data)
        ' Else
        If Not String.IsNullOrEmpty(errLine.Data) Then

            'OutStr = OutStr + " err: " + errLine.Data
            OutStr = errLine.Data
            'Mainform.txtResult.Text = OutStr
            myDelegate(OutStr, False)
            If OutStr.ToLower().Contains("error") Then

            End If
            'UpdateTextBox(OutStr)
        End If
        ' End If
        ' logMutex.ReleaseMutex()
    End Sub

    Private Sub StartMultiProcesses(Exe As String, Args As List(Of String))
        Dim numberOfProcesses As Integer = Args.Count ' Number of processes to start
        Dim processParameters() As String = {"param1", "param2", "param3", "param4", "param5"} ' Parameters for each process

        For i As Integer = 1 To numberOfProcesses
            Dim processIndex As Integer = i ' Store the current index to avoid closure issues
            Task.Run(Sub()
                         Dim process As New Process()
                         process.StartInfo.FileName = Exe ' Replace with the actual path to your executable
                         process.StartInfo.Arguments = Args(processIndex - 1) ' Get the parameter for the current process
                         process.StartInfo.UseShellExecute = False
                         process.StartInfo.RedirectStandardOutput = True
                         process.StartInfo.RedirectStandardError = True
                         process.StartInfo.CreateNoWindow = True

                         ' Event handler for capturing error output
                         AddHandler process.ErrorDataReceived, Sub(senderProcess, errorLine)
                                                                   If Not String.IsNullOrEmpty(errorLine.Data) Then
                                                                       ' Display the error message
                                                                       Dim errorMessage As String = errorLine.Data
                                                                       HandleError(errorMessage)
                                                                   End If
                                                               End Sub
                         AddHandler process.OutputDataReceived, Sub(senderProcess, outputLine)
                                                                    If Not String.IsNullOrEmpty(outputLine.Data) Then
                                                                        ' Update the status text
                                                                        UpdateStatusText(outputLine.Data)
                                                                    End If
                                                                End Sub

                         process.Start()
                         process.BeginOutputReadLine()

                         process.WaitForExit()
                         ' Read the output and update the form
                         Dim output As String = process.StandardOutput.ReadToEnd()
                         UpdateStatusText(output)

                         process.WaitForExit()
                     End Sub)
        Next i
    End Sub

    Private Sub UpdateStatusText(statusText As String)
        ' Update the status text in the TextBox control
        'If InvokeRequired Then
        '    Invoke(Sub() UpdateStatusText(statusText))
        'Else
        '    StatusTextBox.Text = statusText
        '    Application.DoEvents()
        'End If
    End Sub

    Private Sub HandleError(errorMessage As String)
        ' Display the error message
        'If InvokeRequired Then
        '    Invoke(Sub() HandleError(errorMessage))
        'Else
        '    MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End If
    End Sub
End Class
