Imports OHeadcamMapApp.My.Resources

Public Class frmPlayer
    Public Sub New(filename As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        txtPlayerfile.Text = filename
        If Not My.Computer.FileSystem.FileExists(txtPlayerfile.Text) Then
            MsgBox(Texts.NoPlayerFile + txtPlayerfile.Text)
        Else

            AxWindowsMediaPlayer1.URL = txtPlayerfile.Text
            AxWindowsMediaPlayer1.Ctlcontrols.currentPosition = 1
            'AxWindowsMediaPlayer1.Ctlcontrols.play()
            'While AxWindowsMediaPlayer1.currentMedia.duration = 0
            'Threading.Thread.Sleep(500)
            'AxWindowsMediaPlayer1.Ctlcontrols.play()
            'End While
            'AxWindowsMediaPlayer1.Ctlcontrols.pause()
            'HScrollBarVideo.Maximum = AxWindowsMediaPlayer1.currentMedia.duration
            'AxWindowsMediaPlayer1.Ctlcontrols.stop()

            Timer1.Start()
        End If
    End Sub


    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        If Not IsNothing(AxWindowsMediaPlayer1.currentMedia) Then
            If AxWindowsMediaPlayer1.currentMedia.duration > 0 Then
                AxWindowsMediaPlayer1.Ctlcontrols.pause()
                Timer1.Stop()
            End If
        End If
    End Sub
End Class