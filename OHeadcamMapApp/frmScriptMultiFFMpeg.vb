Public Class frmScriptMultiFFMpeg

    Private Sub btnSelectFolder_Click(sender As Object, e As EventArgs) Handles btnSelectFolder.Click
        Dim dialog = New FolderBrowserDialog()
        'dialog.SelectedPath = "C:\"
        If DialogResult.OK = dialog.ShowDialog() Then
            ' User selected a folder
            txtVideoWorkfolder.Text = dialog.SelectedPath + "\"
            txtOutFolder.Text = dialog.SelectedPath + "\"
        End If
    End Sub

    Private Sub btnSelecOutFolder_Click(sender As Object, e As EventArgs) Handles btnSelecOutFolder.Click
        Dim dialog = New FolderBrowserDialog()
        'dialog.SelectedPath = "C:\"
        If DialogResult.OK = dialog.ShowDialog() Then
            ' User selected a folder
            txtOutFolder.Text = dialog.SelectedPath + "\"

        End If
    End Sub

    Private Sub frmScriptMultiFFMpeg_Load(sender As Object, e As EventArgs) Handles Me.Load
        txtFPS.Text = 25
        cmbExt.SelectedIndex = 0
        cmbPreset.SelectedIndex = 0
    End Sub

    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        Dim ffmpegargs As String
        If Not IsNumeric(txtFPS.Text) Then
            MsgBox("Use number value for FPS")
            Return
        End If
        If txtVideoWorkfolder.Text = "" Or txtOutFolder.Text = "" Then
            MsgBox("Choose folders")
            Return
        End If
        ffmpegargs = $"-c:v libx264 -crf {numCRF.Value} -c:a aac -ac 2 -ar 48000 -b:a 128k -r {txtFPS.Text} -preset {cmbPreset.SelectedItem} -pix_fmt yuvj420p -y"
        clsExtra.PS_multi_FF_Script(txtVideoWorkfolder.Text, txtOutFolder.Text, cmbExt.SelectedItem, ffmpegargs)
    End Sub
End Class