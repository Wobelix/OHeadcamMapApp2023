Public Class clsTimeRemaining
    Dim dtStartTime As DateTime
    Dim dtRemainingTime As DateTime
    Dim dTotal As Decimal
    Public dPercent As Decimal
    Public sPercent As String
    Public sRemainingTime As String
    Dim ExtraObj As clsExtra = New clsExtra
    Dim stopwatch As New Stopwatch()
    Public Sub SetTotal(TotalIn As Decimal)
        dTotal = TotalIn
    End Sub
    Sub StartTime(Optional TotalIn As Decimal = 0)
        If TotalIn > 0 Then dTotal = TotalIn
        'dtStartTime = DateTime.Now
        stopwatch.Restart()
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

        ' Do some work here...
        If dTotal > 0 Then
            Dim elapsedTime As TimeSpan = DateTime.Now - dtStartTime
            dPercent = DonePart / dTotal
            If dPercent = 0 Then dPercent = 0.1
            sPercent = (100 * dPercent).ToString("0") + "%"

            Dim elapsedMilliseconds As Long = stopwatch.ElapsedMilliseconds
            Dim expectedTotalMilliseconds As Long = elapsedMilliseconds / dPercent
            Dim remainingMilliseconds As Long = expectedTotalMilliseconds - elapsedMilliseconds

            ' Convert milliseconds to a TimeSpan...
            Dim expectedTotalTime As TimeSpan = TimeSpan.FromMilliseconds(expectedTotalMilliseconds)
            Dim remainingTime As TimeSpan = TimeSpan.FromMilliseconds(remainingMilliseconds)



            sRemainingTime = remainingTime.ToString("hh\:mm\:ss")
            'Dim elapsedTimeString As String = elapsedTime.ToString("hh\:mm\:ss")
        End If
        Return sRemainingTime
    End Function

End Class
