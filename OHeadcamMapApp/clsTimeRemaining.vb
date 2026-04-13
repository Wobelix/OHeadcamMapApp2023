Public Class clsTimeRemaining
    Dim dtStartTime As DateTime
    Dim dtRemainingTime As DateTime
    Dim dTotal As Decimal
    Public dPercent As Decimal
    Public sPercent As String
    Public sRemainingTime As String
    Public sTotalRemainingTime As String
    Public ExtraRemainingSeconds As Decimal
    Dim ExtraObj As clsExtra = New clsExtra
    Dim stopwatch As New Stopwatch()
    Private ReadOnly _recentSamples As New Queue(Of KeyValuePair(Of Decimal, Long))
    Private Const MaxRecentSamples As Integer = 12

    Public Sub SetTotal(TotalIn As Decimal)
        dTotal = TotalIn
    End Sub

    Sub StartTime(Optional TotalIn As Decimal = 0)
        If TotalIn > 0 Then dTotal = TotalIn
        dtStartTime = DateTime.Now
        dtRemainingTime = DateTime.Now
        stopwatch.Restart()
        _recentSamples.Clear()
        sPercent = ""
        sRemainingTime = ""
        sTotalRemainingTime = ""
    End Sub

    Function SetGetRemainingTime(DonePart As String) As String
        Dim t As Decimal
        If DonePart.IndexOf(":") > 0 Then
            t = ExtraObj.TimeStrToSec(DonePart)
        Else
            t = CDec(DonePart)
        End If
        Return SetGetRemainingTime(t)
    End Function

    Function SetGetRemainingTime(DonePart As Decimal) As String
        If dTotal > 0 Then
            dPercent = DonePart / dTotal
            If dPercent <= 0 Then dPercent = 0.001D
            If dPercent > 1 Then dPercent = 1
            sPercent = (100 * dPercent).ToString("0") + "%"

            Dim elapsedMilliseconds As Long = stopwatch.ElapsedMilliseconds
            EnqueueSample(DonePart, elapsedMilliseconds)

            Dim remainingMilliseconds As Long = EstimateRemainingMilliseconds(DonePart, elapsedMilliseconds)
            If remainingMilliseconds < 0 Then remainingMilliseconds = 0

            Dim remainingTime As TimeSpan = TimeSpan.FromMilliseconds(remainingMilliseconds)
            sRemainingTime = remainingTime.ToString("hh\:mm\:ss")

            Dim totalRemainingMilliseconds As Long = remainingMilliseconds + CLng(Math.Max(0D, ExtraRemainingSeconds) * 1000D)
            Dim totalRemainingTime As TimeSpan = TimeSpan.FromMilliseconds(Math.Max(0, totalRemainingMilliseconds))
            sTotalRemainingTime = totalRemainingTime.ToString("hh\:mm\:ss")
            dtRemainingTime = DateTime.Now.Add(totalRemainingTime)
        End If

        Return sRemainingTime
    End Function

    Private Sub EnqueueSample(donePart As Decimal, elapsedMilliseconds As Long)
        If _recentSamples.Count > 0 Then
            Dim lastSample As KeyValuePair(Of Decimal, Long) = _recentSamples.Last()
            If lastSample.Key = donePart AndAlso lastSample.Value = elapsedMilliseconds Then Return
        End If

        _recentSamples.Enqueue(New KeyValuePair(Of Decimal, Long)(donePart, elapsedMilliseconds))
        While _recentSamples.Count > MaxRecentSamples
            _recentSamples.Dequeue()
        End While
    End Sub

    Private Function EstimateRemainingMilliseconds(donePart As Decimal, elapsedMilliseconds As Long) As Long
        Dim remainingWork As Decimal = Math.Max(0D, dTotal - donePart)
        If remainingWork <= 0D Then Return 0

        If _recentSamples.Count >= 2 Then
            Dim firstSample As KeyValuePair(Of Decimal, Long) = _recentSamples.Peek()
            Dim lastSample As KeyValuePair(Of Decimal, Long) = _recentSamples.Last()
            Dim deltaDone As Decimal = lastSample.Key - firstSample.Key
            Dim deltaMilliseconds As Long = lastSample.Value - firstSample.Value

            If deltaDone > 0D AndAlso deltaMilliseconds > 0L Then
                Dim millisecondsPerUnit As Decimal = CDec(deltaMilliseconds) / deltaDone
                Return CLng(Math.Round(millisecondsPerUnit * remainingWork))
            End If
        End If

        If donePart <= 0D Then Return 0

        Dim expectedTotalMilliseconds As Decimal = CDec(elapsedMilliseconds) / dPercent
        Return CLng(Math.Max(0D, expectedTotalMilliseconds - elapsedMilliseconds))
    End Function
End Class
