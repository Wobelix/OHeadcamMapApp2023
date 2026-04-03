Imports OHeadcamMapApp.My.Resources

Public Class frmUseMapVideo
    Public MainFormIn As MainForm
    Public bFormReady As Boolean = False
    Public Sub New(ByRef MainForminput As MainForm)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        MainFormIn = MainForminput
        bFormReady = True
    End Sub

    Private Sub btnSelectVideo_Click(sender As Object, e As EventArgs) Handles btnSelectVideo.Click
        If OpenFileDialogVideo.ShowDialog() = DialogResult.OK Then
            txtMapVideoIn.Text = System.IO.Path.GetFileName(OpenFileDialogVideo.FileName)
            My.Settings.TrackMapVideoFilename = OpenFileDialogVideo.FileName
        End If

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        My.Settings.Save()
        MainFormIn.TrackVideoState() 'Set/unset trackvideo state
        Me.Close()
    End Sub
    Function ValidateInput() As Boolean
        ValidateInput = True
        If txtMapVideoIn.Text = My.Settings.txtMapVideoFilename And txtRealtimeFactor.Text = My.Settings.txtRealtimeFactor Then ' startup check
            If Not My.Computer.FileSystem.FileExists(My.Settings.TrackMapVideoFilename) Then
                MsgBox(Texts.ErrNoFile + txtMapVideoIn.Text)
                ValidateInput = False
            ElseIf Not IsNumeric(txtRealtimeFactor.Text) Then
                MsgBox(Texts.ErrNotNumeric)
                ValidateInput = False
            End If
        End If

    End Function
    Private Sub cbUseMapTracking_CheckedChanged(sender As Object, e As EventArgs) Handles cbUseMapTracking.CheckedChanged
        If cbUseMapTracking.Checked And bFormReady Then
            If ValidateInput() Then
                My.Settings.TrackMapImageFile = MainFormIn.AppFolder + "TrackMap.jpg"
                My.Settings.Save()
                If Not MainFormIn.PrepareTrackingMap(My.Settings.TrackMapImageFile) Then
                    cbUseMapTracking.Checked = False
                End If

            End If
        End If
    End Sub

    Private Sub txtRealtimeFactor_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtRealtimeFactor.KeyPress

        If Char.IsDigit(e.KeyChar) Or Char.IsControl(e.KeyChar) Then
            'Allow numeric characters or control keys (such as backspace)
            e.Handled = False
        Else
            'Block all other characters
            e.Handled = True
        End If
    End Sub
End Class