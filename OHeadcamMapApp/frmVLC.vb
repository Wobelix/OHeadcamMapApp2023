Imports System.IO
Imports LibVLCSharp.Shared
'Imports AXVLC
'Imports LibVLCSharp.[Shared]
'Imports VideoLAN.LibVLC
'Imports VideoLAN.LibVLC.Windows.Forms
Public Class frmVLC
    Private _mp As MediaPlayer
    Private _libVLC As LibVLC
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        Core.Initialize()
        ' Add any initialization after the InitializeComponent() call.
        _libVLC = New LibVLC()
        _mp = New MediaPlayer(_libVLC)
        VideoView1.MediaPlayer = _mp
        'If IntPtr.Size = 4 Then
        '    VlcControl1.VlcLibDirectory = New DirectoryInfo(Application.StartupPath + "\libvlc\win-x64\")
        'Else
        '    VlcControl1.VlcLibDirectory = New DirectoryInfo(Application.StartupPath + "\libvlc\win-x86\")
        'End If
    End Sub
    'Private vlcControl As AXVLC.VLCPlugin2
    Private Sub frmVLC_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Dim m As MediaList()
        'm.as
        'Dim a As New MediaPlayer()
        'vlcControl = New AXVLC.VLCPlugin2
        'Me.Controls.Add(vlcControl)
        'vlcControl.BeginInit()
        'vlcControl.VlcLibDirectoryNeeded += AddressOf VlcControl_Vlc_LibDirectoryNeeded
        'Me.Controls.Add(vlcControl)
        Dim file As String = "F:\Videoer\SC15JUN23\deshaked.mp4"
        Dim fi As FileInfo = New FileInfo("F:\Videoer\SC15JUN23\deshaked.mp4")
        _mp.Play(New Media(_libVLC, File))
        'VlcControl1.SetMedia(fi)
        'VlcControl1.Play()
    End Sub

    'Private Sub VlcControl1_VlcLibDirectoryNeeded(sender As Object, e As Vlc.DotNet.Forms.VlcLibDirectoryNeededEventArgs)
    '    If IntPtr.Size = 4 Then
    '        e.VlcLibDirectory = New DirectoryInfo(Application.StartupPath + "\libvlc\win-x86")
    '        sender.VlcLibDirectory = New DirectoryInfo(Application.StartupPath + "\libvlc\win-x86")
    '        VlcControl1.VlcLibDirectory = New DirectoryInfo(Application.StartupPath + "\libvlc\win-x86")
    '    Else
    '        sender.VlcLibDirectory = New DirectoryInfo(Application.StartupPath + "\libvlc\win-x64")
    '        e.VlcLibDirectory = New DirectoryInfo(Application.StartupPath + "\libvlc\win-x64")
    '        VlcControl1.VlcLibDirectory = New DirectoryInfo(Application.StartupPath + "\libvlc\win-x64")
    '    End If
    'End Sub


End Class