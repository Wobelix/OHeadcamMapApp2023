<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AddAudioform
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AddAudioform))
        Me.btnMakeVideo = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtVideofile = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnOpenVideoFile = New System.Windows.Forms.Button()
        Me.btnOpenAudioFile = New System.Windows.Forms.Button()
        Me.OpenFileDialogVideo = New System.Windows.Forms.OpenFileDialog()
        Me.OpenFileDialogAudio = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialogOutput = New System.Windows.Forms.SaveFileDialog()
        Me.btnSaveFileName = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TrackBarMusicLevel = New System.Windows.Forms.TrackBar()
        Me.TrackAudioLevel = New System.Windows.Forms.TrackBar()
        Me.chMusicOn = New System.Windows.Forms.CheckBox()
        Me.cbLoopMusic = New System.Windows.Forms.CheckBox()
        Me.txtMusicLevel = New System.Windows.Forms.TextBox()
        Me.txtOutputFile = New System.Windows.Forms.TextBox()
        Me.txtMusicfile = New System.Windows.Forms.TextBox()
        Me.txtAudioLevel = New System.Windows.Forms.TextBox()
        CType(Me.TrackBarMusicLevel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TrackAudioLevel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnMakeVideo
        '
        resources.ApplyResources(Me.btnMakeVideo, "btnMakeVideo")
        Me.btnMakeVideo.Name = "btnMakeVideo"
        Me.btnMakeVideo.UseVisualStyleBackColor = True
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'txtVideofile
        '
        resources.ApplyResources(Me.txtVideofile, "txtVideofile")
        Me.txtVideofile.Name = "txtVideofile"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'btnOpenVideoFile
        '
        resources.ApplyResources(Me.btnOpenVideoFile, "btnOpenVideoFile")
        Me.btnOpenVideoFile.Name = "btnOpenVideoFile"
        Me.btnOpenVideoFile.UseVisualStyleBackColor = True
        '
        'btnOpenAudioFile
        '
        resources.ApplyResources(Me.btnOpenAudioFile, "btnOpenAudioFile")
        Me.btnOpenAudioFile.Name = "btnOpenAudioFile"
        Me.btnOpenAudioFile.UseVisualStyleBackColor = True
        '
        'OpenFileDialogVideo
        '
        Me.OpenFileDialogVideo.FileName = "OpenFileDialog1"
        resources.ApplyResources(Me.OpenFileDialogVideo, "OpenFileDialogVideo")
        '
        'OpenFileDialogAudio
        '
        resources.ApplyResources(Me.OpenFileDialogAudio, "OpenFileDialogAudio")
        '
        'SaveFileDialogOutput
        '
        Me.SaveFileDialogOutput.FileName = "FinalMusic.mp4"
        resources.ApplyResources(Me.SaveFileDialogOutput, "SaveFileDialogOutput")
        '
        'btnSaveFileName
        '
        resources.ApplyResources(Me.btnSaveFileName, "btnSaveFileName")
        Me.btnSaveFileName.Name = "btnSaveFileName"
        Me.btnSaveFileName.UseVisualStyleBackColor = True
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'TrackBarMusicLevel
        '
        resources.ApplyResources(Me.TrackBarMusicLevel, "TrackBarMusicLevel")
        Me.TrackBarMusicLevel.Maximum = 100
        Me.TrackBarMusicLevel.Name = "TrackBarMusicLevel"
        Me.TrackBarMusicLevel.TabStop = False
        Me.TrackBarMusicLevel.TickFrequency = 5
        Me.TrackBarMusicLevel.Value = 50
        '
        'TrackAudioLevel
        '
        resources.ApplyResources(Me.TrackAudioLevel, "TrackAudioLevel")
        Me.TrackAudioLevel.Maximum = 100
        Me.TrackAudioLevel.Name = "TrackAudioLevel"
        Me.TrackAudioLevel.TabStop = False
        Me.TrackAudioLevel.TickFrequency = 5
        Me.TrackAudioLevel.Value = 50
        '
        'chMusicOn
        '
        resources.ApplyResources(Me.chMusicOn, "chMusicOn")
        Me.chMusicOn.Checked = Global.OHeadcamMapApp.My.MySettings.Default.bDoMusic
        Me.chMusicOn.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.OHeadcamMapApp.My.MySettings.Default, "bDoMusic", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.chMusicOn.Name = "chMusicOn"
        Me.chMusicOn.UseVisualStyleBackColor = True
        '
        'cbLoopMusic
        '
        resources.ApplyResources(Me.cbLoopMusic, "cbLoopMusic")
        Me.cbLoopMusic.Checked = Global.OHeadcamMapApp.My.MySettings.Default.cbLoopMusic
        Me.cbLoopMusic.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbLoopMusic.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.OHeadcamMapApp.My.MySettings.Default, "cbLoopMusic", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.cbLoopMusic.Name = "cbLoopMusic"
        Me.cbLoopMusic.UseVisualStyleBackColor = True
        '
        'txtMusicLevel
        '
        resources.ApplyResources(Me.txtMusicLevel, "txtMusicLevel")
        Me.txtMusicLevel.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "Musiclevel", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtMusicLevel.Name = "txtMusicLevel"
        Me.txtMusicLevel.Text = Global.OHeadcamMapApp.My.MySettings.Default.Musiclevel
        '
        'txtOutputFile
        '
        resources.ApplyResources(Me.txtOutputFile, "txtOutputFile")
        Me.txtOutputFile.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "txtMusicOutputfile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtOutputFile.Name = "txtOutputFile"
        Me.txtOutputFile.Text = Global.OHeadcamMapApp.My.MySettings.Default.txtMusicOutputfile
        '
        'txtMusicfile
        '
        resources.ApplyResources(Me.txtMusicfile, "txtMusicfile")
        Me.txtMusicfile.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "MusicAudiofile", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtMusicfile.Name = "txtMusicfile"
        Me.txtMusicfile.Text = Global.OHeadcamMapApp.My.MySettings.Default.MusicAudiofile
        '
        'txtAudioLevel
        '
        resources.ApplyResources(Me.txtAudioLevel, "txtAudioLevel")
        Me.txtAudioLevel.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "Audiolevel", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.txtAudioLevel.Name = "txtAudioLevel"
        Me.txtAudioLevel.Text = Global.OHeadcamMapApp.My.MySettings.Default.Audiolevel
        '
        'AddAudioform
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.chMusicOn)
        Me.Controls.Add(Me.cbLoopMusic)
        Me.Controls.Add(Me.txtMusicLevel)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TrackBarMusicLevel)
        Me.Controls.Add(Me.btnSaveFileName)
        Me.Controls.Add(Me.txtOutputFile)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.btnOpenAudioFile)
        Me.Controls.Add(Me.btnOpenVideoFile)
        Me.Controls.Add(Me.txtMusicfile)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtAudioLevel)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TrackAudioLevel)
        Me.Controls.Add(Me.txtVideofile)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnMakeVideo)
        Me.Name = "AddAudioform"
        CType(Me.TrackBarMusicLevel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TrackAudioLevel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnMakeVideo As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents txtVideofile As TextBox
    Friend WithEvents TrackAudioLevel As TrackBar
    Friend WithEvents Label2 As Label
    Friend WithEvents txtAudioLevel As TextBox
    Friend WithEvents txtMusicfile As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents btnOpenVideoFile As Button
    Friend WithEvents btnOpenAudioFile As Button
    Friend WithEvents OpenFileDialogVideo As OpenFileDialog
    Friend WithEvents OpenFileDialogAudio As OpenFileDialog
    Friend WithEvents SaveFileDialogOutput As SaveFileDialog
    Friend WithEvents btnSaveFileName As Button
    Friend WithEvents txtOutputFile As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtMusicLevel As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents TrackBarMusicLevel As TrackBar
    Friend WithEvents cbLoopMusic As CheckBox
    Friend WithEvents chMusicOn As CheckBox
End Class
