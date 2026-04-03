Imports OHeadcamMapApp.My.Resources

Public Class AddAudioform
    Public Outputfile As String
    Public strQuote As String = Chr(34)
    Public oMainform As MainForm


    Public Sub New(Optional InVideo As String = "")

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'oMainform = iMainform
        My.Settings.AudioVideofile = InVideo
        txtOutputFile.Text = My.Settings.MusicOutputfile
        'txtVideofile.Text = System.IO.Path.GetFileName(My.Settings.AudioVideofile)
        txtAudioLevel.Text = My.Settings.Audiolevel
        txtMusicLevel.Text = My.Settings.Musiclevel
        Try
            TrackAudioLevel.Value = CInt(txtAudioLevel.Text)
            TrackBarMusicLevel.Value = CInt(txtMusicLevel.Text)
        Catch ex As Exception
            txtAudioLevel.Text = ""
            txtMusicLevel.Text = ""
            TrackAudioLevel.Value = 0
            TrackBarMusicLevel.Value = 0
        End Try
    End Sub



    Private Sub btnOpenAudioFile_Click(sender As Object, e As EventArgs) Handles btnOpenAudioFile.Click
        Dim fn As String
        If OpenFileDialogAudio.ShowDialog() = DialogResult.OK Then
            My.Settings.Musicfile = OpenFileDialogAudio.FileName
            fn = System.IO.Path.GetFileName(My.Settings.Musicfile)
            txtMusicfile.Text = fn

        End If
    End Sub

    Private Sub btnSaveFileName_Click(sender As Object, e As EventArgs) Handles btnSaveFileName.Click
        Dim fn As String
        If SaveFileDialogOutput.ShowDialog() = DialogResult.OK Then
            My.Settings.MusicOutputfile = SaveFileDialogOutput.FileName
            fn = System.IO.Path.GetFileName(My.Settings.MusicOutputfile)
            txtOutputFile.Text = fn
            My.Settings.Save()
        End If
    End Sub

    Private Sub btnOpenVideoFile_Click(sender As Object, e As EventArgs) Handles btnOpenVideoFile.Click
        Dim fn As String
        If OpenFileDialogVideo.ShowDialog() = DialogResult.OK Then
            My.Settings.AudioVideofile = OpenFileDialogVideo.FileName
            fn = System.IO.Path.GetFileName(My.Settings.AudioVideofile)
            txtVideofile.Text = fn
            My.Settings.Save()
        End If
    End Sub
    Public Function Quot(a As String) As String
        Quot = strQuote + a + strQuote
    End Function

    Private Sub TrackBarMusicLevel_ValueChanged(sender As Object, e As EventArgs) Handles TrackBarMusicLevel.Scroll
        txtMusicLevel.Text = CStr(TrackBarMusicLevel.Value)
    End Sub
    Private Sub TrackAudioLevel_ValueChanged(sender As Object, e As EventArgs) Handles TrackAudioLevel.Scroll
        txtAudioLevel.Text = CStr(TrackAudioLevel.Value)
    End Sub

    Public Function Validate_input() As Boolean
        Validate_input = True
        'If Not My.Computer.FileSystem.FileExists(My.Settings.AudioVideofile) Then
        'MsgBox(Texts.ErrNoFile + txtVideofile.Text)
        'Validate_input = False
        'End If
        If txtOutputFile.Text = My.Settings.MusicOutputfile And txtMusicfile.Text = My.Settings.Musicfile Then ' startup check
            If Not My.Computer.FileSystem.FileExists(My.Settings.Musicfile) Then
                MsgBox(Texts.ErrNoFile + txtMusicfile.Text)
                Validate_input = False
            End If
            If txtOutputFile.Text = "" Then
                MsgBox(Texts.ErrNoOutpFile)
                Validate_input = False
            End If
        End If


    End Function
    Private Sub btnMakeVideo_Click(sender As Object, e As EventArgs) Handles btnMakeVideo.Click
        Dim Ffmpeg_param, video, audio, volumeAudio, volumeMusic, out, mixparam, audioin_filter, shorteststr As String
        Dim Musiclevel, AudioLevel As Decimal
        '-i video1.mp4 -i audio1.mp3 -filter_complex "[0:a] volume=0.5 [music];[music][1:a] amix=inputs=2:duration=longest [audio_out] -map 0:v -map "[audio_out]" -y output.mp4 

        If Not IsNumeric(txtMusicLevel.Text) Then txtMusicLevel.Text = "0"
        If Not IsNumeric(txtAudioLevel.Text) Then txtAudioLevel.Text = "0"

        Musiclevel = CDec(txtMusicLevel.Text) / 100
        AudioLevel = CDec(txtAudioLevel.Text) / 100

        If Validate_input() Then
            'video = Quot(My.Settings.AudioVideofile) ' input file - full path not textbox
            audio = "'" + My.Settings.Musicfile + "'" 'Full path not textbox,  cannot use " in amovie in ffmpeg
            audio = audio.Replace("\", "\\") ' escape \ for ffmpeg
            audio = audio.Replace(":", "\:") ' escape : for ffmpeg


            out = Quot(My.Settings.MusicOutputfile)
            volumeAudio = String.Format("{0:0.#}", AudioLevel)
            volumeAudio = Replace(volumeAudio, ",", ".")
            volumeMusic = String.Format("{0:0.#}", Musiclevel)
            volumeMusic = Replace(volumeMusic, ",", ".")
            '-i "a.mp4" -c:v copy -filter_complex "amovie='C\:\\Users\\mmo21\\Downloads\\000706832-acoustic-loop-26.m4a':loop=0,volume=0.5[music];[0:a]volume=0.5[vaudio];[vaudio][music] amix=inputs=2:duration=longest [audio_out]" -map 0:v -map [audio_out] -shortest -y "b.mp4"
            If cbLoopMusic.Checked Then 'loop then
                audioin_filter = "amovie=" + audio + ":loop=0,volume=" + volumeMusic + "[music];[0:a]volume=" + volumeAudio + "[vaudio];"
                shorteststr = " -shortest" ' need to limit to videolength because of loop-
            Else
                audioin_filter = "amovie=" + audio + ":volume=" + volumeMusic + "[music];[0:a]volume=" + volumeAudio + "[vaudio];"
                shorteststr = ""
            End If
            'GAMMEL
            'mixparam = "[vaudio][music] amix=inputs=2:duration=longest [audio_out]" + strQuote + " -map 0:v -map " + strQuote + "[audio_out]" + strQuote
            'Ffmpeg_param = " -i " + video + " -i " + audio + " -c:v copy" + " -filter_complex " + strQuote + "[0:a]volume=" + volumeAudio + "[vaudio];" + "[1:a]volume=" + volumeMusic + "[music];" + mixparam + " -y " + out

            'NY
            mixparam = "[vaudio][music] amix=inputs=2:duration=longest [audio_out]" + strQuote + " -map 0:v -map " + strQuote + "[audio_out]" + strQuote
            Ffmpeg_param = " -i " + "VIDEOFILE" + " -c:v copy" + " -filter_complex " + strQuote + audioin_filter + mixparam + shorteststr + " -y " + out ' videoname is used for replacement
            My.Settings.MusicFFMpegParam = Ffmpeg_param

            'NY end
            'oMainform.MakeMusicVideo(Ffmpeg_param)
        Else
            chMusicOn.Checked = False
        End If
        My.Settings.Save()
        Me.Close()
    End Sub

    Private Sub chMusicOn_CheckedChanged(sender As Object, e As EventArgs) Handles chMusicOn.CheckedChanged
        If chMusicOn.Checked Then
            If Not Validate_input() Then
                chMusicOn.Checked = False
            End If
        End If
    End Sub
End Class