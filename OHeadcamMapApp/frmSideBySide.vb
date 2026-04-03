Public Class frmSideBySide
    Dim CMainform As MainForm
    Public Sub New(iMainform As MainForm)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        CMainform = iMainform
    End Sub

    Private Sub btnSbSLfile_Click(sender As Object, e As EventArgs) Handles btnSbSLfile.Click
        If OpenFileDialogsbslfile.ShowDialog() = DialogResult.OK Then

            txtSbSLeft.Text = OpenFileDialogsbslfile.FileName


        End If
    End Sub

    Private Sub btnSbSRfile_Click(sender As Object, e As EventArgs) Handles btnSbSRfile.Click
        If OpenFileDialogsbsrfile.ShowDialog() = DialogResult.OK Then

            txtSbSRight.Text = OpenFileDialogsbsrfile.FileName


        End If
    End Sub

    Private Sub btnSbSOutfile_Click(sender As Object, e As EventArgs) Handles btnSbSOutfile.Click
        If SaveFileDialogsbsOut.ShowDialog() = DialogResult.OK Then
            txtSbSOut.Text = SaveFileDialogsbsOut.FileName

        End If
    End Sub

    Private Sub btnSbSRun_Click(sender As Object, e As EventArgs) Handles btnSbSRun.Click
        Dim Param As String
        If My.Computer.FileSystem.FileExists(txtSbSLeft.Text) And My.Computer.FileSystem.FileExists(txtSbSRight.Text) Then
            CMainform.LogDeshake = ""
            Param = "-i " + """" + txtSbSLeft.Text + """" + " -i " + """" + txtSbSRight.Text + """" + " " + My.Settings.SettingSbSCode + CMainform.ExtraFunc.FFMPeg_MakeOutputStr(txtSbSOut.Text, txtSbSLength.Text, False, False)
            CMainform.Run_CommandX(CMainform.FFMpegExe, Param, CMainform.LogDeshake)
            Close() ' luk billede
        Else
            MsgBox("Der mangler filangivelse")
        End If
    End Sub
End Class