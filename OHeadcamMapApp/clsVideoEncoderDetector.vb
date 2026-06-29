Imports System.Diagnostics
Imports System.Globalization
Imports System.IO

Public Enum VideoEncoderKind
    SoftwareX265
    NvidiaNvenc
    IntelQuickSync
    AmdAmf
    MediaFoundation
End Enum

Public Class VideoEncoderDetectionResult
    Public Property Kind As VideoEncoderKind
    Public Property EncoderName As String
    Public Property DisplayName As String
    Public Property PixelFormat As String
    Public Property TestElapsed As TimeSpan
    Public Property FfmpegVersion As String
    Public Property DetectionLog As String
    Public Property IsHardware As Boolean
End Class

Public NotInheritable Class clsVideoEncoderDetector
    Private Shared ReadOnly SyncRoot As New Object()
    Private Shared CachedResult As VideoEncoderDetectionResult

    Private Sub New()
    End Sub

    Public Shared Function GetBestHevcEncoder(Optional forceRefresh As Boolean = False) As VideoEncoderDetectionResult
        SyncLock SyncRoot
            If CachedResult IsNot Nothing AndAlso Not forceRefresh Then Return CachedResult
            CachedResult = DetectBestHevcEncoder()
            Return CachedResult
        End SyncLock
    End Function

    Private Shared Function DetectBestHevcEncoder() As VideoEncoderDetectionResult
        Dim ffmpegPath As String = GetFfmpegPath()
        Dim versionOutput As String = RunFfmpeg(ffmpegPath, "-hide_banner -version", 5000).Output
        Dim versionLine As String = FirstNonEmptyLine(versionOutput)
        Dim encodersResult As ProcessResult = RunFfmpeg(ffmpegPath, "-hide_banner -encoders", 10000)
        Dim enabledEncoders As String = encodersResult.Output
        Dim logLines As New List(Of String) From {
            "FFmpeg: " & If(String.IsNullOrWhiteSpace(versionLine), "version unknown", versionLine)
        }
        Dim successful As New List(Of VideoEncoderDetectionResult)

        TestCandidate(ffmpegPath, enabledEncoders, VideoEncoderKind.NvidiaNvenc, "hevc_nvenc", "Nvidia NVENC", "yuv420p",
                      "-preset p5 -cq 25", successful, logLines)
        TestCandidate(ffmpegPath, enabledEncoders, VideoEncoderKind.IntelQuickSync, "hevc_qsv", "Intel Quick Sync", "nv12",
                      "-preset medium -global_quality 25", successful, logLines)
        TestCandidate(ffmpegPath, enabledEncoders, VideoEncoderKind.AmdAmf, "hevc_amf", "AMD AMF", "nv12",
                      "-quality balanced -rc cqp -qp_i 25 -qp_p 25", successful, logLines)
        TestCandidate(ffmpegPath, enabledEncoders, VideoEncoderKind.MediaFoundation, "hevc_mf", "Windows Media Foundation", "nv12",
                      "-hw_encoding 1 -quality 75", successful, logLines)

        Dim selected As VideoEncoderDetectionResult
        If successful.Count > 0 Then
            selected = successful.OrderBy(Function(candidate) candidate.TestElapsed).First()
        Else
            selected = New VideoEncoderDetectionResult With {
                .Kind = VideoEncoderKind.SoftwareX265,
                .EncoderName = "libx265",
                .DisplayName = "Software HEVC (libx265)",
                .PixelFormat = "yuv420p",
                .TestElapsed = TimeSpan.Zero,
                .IsHardware = False
            }
            logLines.Add("Selected software HEVC because no hardware HEVC encoder completed the test.")
        End If

        selected.FfmpegVersion = versionLine
        logLines.Add("Selected encoder: " & selected.DisplayName)
        selected.DetectionLog = String.Join(Environment.NewLine, logLines)
        Return selected
    End Function

    Private Shared Sub TestCandidate(ffmpegPath As String,
                                     enabledEncoders As String,
                                     kind As VideoEncoderKind,
                                     encoderName As String,
                                     displayName As String,
                                     pixelFormat As String,
                                     encoderOptions As String,
                                     successful As List(Of VideoEncoderDetectionResult),
                                     logLines As List(Of String))
        If enabledEncoders.IndexOf(encoderName, StringComparison.OrdinalIgnoreCase) < 0 Then
            logLines.Add(displayName & ": not included in this FFmpeg build.")
            Return
        End If

        Dim args As String =
            "-hide_banner -loglevel error -f lavfi -i color=c=black:s=1280x720:r=30:d=1 " &
            "-an -frames:v 30 -pix_fmt " & pixelFormat & " -c:v " & encoderName & " " & encoderOptions & " -f null -"
        Dim stopwatch As Stopwatch = Stopwatch.StartNew()
        Dim result As ProcessResult = RunFfmpeg(ffmpegPath, args, 20000)
        stopwatch.Stop()

        If result.ExitCode = 0 AndAlso Not result.TimedOut Then
            successful.Add(New VideoEncoderDetectionResult With {
                .Kind = kind,
                .EncoderName = encoderName,
                .DisplayName = displayName,
                .PixelFormat = pixelFormat,
                .TestElapsed = stopwatch.Elapsed,
                .IsHardware = True
            })
            logLines.Add(displayName & ": OK (" & stopwatch.Elapsed.TotalSeconds.ToString("0.00", CultureInfo.InvariantCulture) & " s).")
        Else
            Dim reason As String = FirstNonEmptyLine(result.Output)
            If String.IsNullOrWhiteSpace(reason) Then reason = If(result.TimedOut, "test timed out", "encoder test failed")
            logLines.Add(displayName & ": unavailable (" & reason & ").")
        End If
    End Sub

    Private Shared Function GetFfmpegPath() As String
        Dim localPath As String = Path.Combine(My.Application.Info.DirectoryPath, "ffmpeg.exe")
        If File.Exists(localPath) Then Return localPath
        Return "ffmpeg.exe"
    End Function

    Private Shared Function RunFfmpeg(executable As String, arguments As String, timeoutMilliseconds As Integer) As ProcessResult
        Dim psi As New ProcessStartInfo With {
            .FileName = executable,
            .Arguments = arguments,
            .UseShellExecute = False,
            .CreateNoWindow = True,
            .RedirectStandardOutput = True,
            .RedirectStandardError = True,
            .WorkingDirectory = My.Application.Info.DirectoryPath
        }

        Try
            Using process As New Process()
                process.StartInfo = psi
                process.Start()
                Dim stdoutTask = process.StandardOutput.ReadToEndAsync()
                Dim stderrTask = process.StandardError.ReadToEndAsync()
                Dim exited As Boolean = process.WaitForExit(timeoutMilliseconds)
                If Not exited Then
                    Try
                        process.Kill()
                    Catch
                    End Try
                End If
                process.WaitForExit()
                Dim output As String = stdoutTask.Result & Environment.NewLine & stderrTask.Result
                Return New ProcessResult With {
                    .ExitCode = If(exited, process.ExitCode, -1),
                    .Output = output.Trim(),
                    .TimedOut = Not exited
                }
            End Using
        Catch ex As Exception
            Return New ProcessResult With {
                .ExitCode = -1,
                .Output = ex.Message,
                .TimedOut = False
            }
        End Try
    End Function

    Private Shared Function FirstNonEmptyLine(text As String) As String
        If String.IsNullOrWhiteSpace(text) Then Return ""
        For Each line As String In text.Split({ControlChars.Cr, ControlChars.Lf}, StringSplitOptions.RemoveEmptyEntries)
            If Not String.IsNullOrWhiteSpace(line) Then Return line.Trim()
        Next
        Return ""
    End Function

    Private Class ProcessResult
        Public Property ExitCode As Integer
        Public Property Output As String
        Public Property TimedOut As Boolean
    End Class
End Class
