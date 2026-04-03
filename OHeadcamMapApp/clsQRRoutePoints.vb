Public Class ImgLapVector
    Public Sub New(ByVal iVector As ImgLapVector)
        StartPoint = iVector.StartPoint
        EndPoint = iVector.EndPoint
    End Sub
    Public Sub New(iStartPoint As PointF, iEndPoint As PointF)
        StartPoint = iStartPoint
        EndPoint = iEndPoint
    End Sub

    Public Property StartPoint As PointF
    Public Property EndPoint As PointF

End Class
Public Class clsQRRoutePoint
    Public Time As DateTime
    Public Longitude As Double
    Public Latitude As Double
    Public ElapsedTimeFromStart As Integer
    Public CircleTimeBackward As Double
    Public CircleTimeForward As Double
    Public RouteDistanceFromStart As Double
    Public Pace As Double
    Public Speed As Double
    Public HeartRate As Integer
    Public Altitude As Double
    Public DirectionDeviationToNextLap As Double
    Public Direction As Double
    Public Inclination As Double
    Public AscentFromStart As Double
    Public DescentFromStart As Double
    Public ImageX As Double
    Public ImageY As Double
    Public LapNumber As Integer
    Public ElapsedTime As Integer
    Public RouteDistance As Double
    Public Sub New()

    End Sub
    ' Constructor that takes an object of the same class as input and copies all the members
    Public Sub New(ByVal other As clsQRRoutePoint)
        Me.Time = other.Time
        Me.Longitude = other.Longitude
        Me.Latitude = other.Latitude
        Me.ElapsedTimeFromStart = other.ElapsedTimeFromStart
        Me.CircleTimeBackward = other.CircleTimeBackward
        Me.CircleTimeForward = other.CircleTimeForward
        Me.RouteDistanceFromStart = other.RouteDistanceFromStart
        Me.Pace = other.Pace
        Me.Speed = other.Speed
        Me.HeartRate = other.HeartRate
        Me.Altitude = other.Altitude
        Me.DirectionDeviationToNextLap = other.DirectionDeviationToNextLap
        Me.Direction = other.Direction
        Me.Inclination = other.Inclination
        Me.AscentFromStart = other.AscentFromStart
        Me.DescentFromStart = other.DescentFromStart
        Me.ImageX = other.ImageX
        Me.ImageY = other.ImageY
        Me.LapNumber = other.LapNumber
        Me.ElapsedTime = other.ElapsedTime
        Me.RouteDistance = other.RouteDistance
    End Sub

End Class

Public Class clsQRRoutePoints
    Public QRLogoYOffset As Double = 65 ' 65 pixels til QR logo i QR export af jpg, forskyder Image koordinaterne i XML
    Public RoutePoints As New List(Of clsQRRoutePoint)()
    Public LapPoints As New List(Of clsQRRoutePoint)()
    Public ImgRoutePoints As New List(Of Point)()
    Public ImgLapVectors As New List(Of ImgLapVector)()
    Public NoRoutePoints, NoLaps, LapNo As Integer
    Public CurrentLap As Integer = -1
    Public PrevImgLapPoint, tImgLapStartpoint As Point
    Public Sub New()

    End Sub
    Public Sub New(ByVal inObject As clsQRRoutePoints)
        Me.QRLogoYOffset = inObject.QRLogoYOffset
        Me.NoRoutePoints = inObject.NoRoutePoints
        Me.NoLaps = inObject.NoLaps
        Me.LapNo = inObject.LapNo
        Me.CurrentLap = inObject.CurrentLap
        Me.PrevImgLapPoint = inObject.PrevImgLapPoint
        Me.tImgLapStartpoint = inObject.tImgLapStartpoint

        For Each point As clsQRRoutePoint In inObject.RoutePoints
            Me.RoutePoints.Add(New clsQRRoutePoint(point))
        Next

        For Each point As clsQRRoutePoint In inObject.LapPoints
            Me.LapPoints.Add(New clsQRRoutePoint(point))
        Next

        For Each point As PointF In inObject.ImgRoutePoints
            Me.ImgRoutePoints.Add(New Point(point.X, point.Y))
        Next

        For Each vector As ImgLapVector In inObject.ImgLapVectors
            Me.ImgLapVectors.Add(New ImgLapVector(vector))
        Next
    End Sub


    Public Sub AddPoint(QRpoint As clsQRRoutePoint)


        RoutePoints.Add(QRpoint)

        ImgRoutePoints.Add(New Point(QRpoint.ImageX, QRpoint.ImageY + QRLogoYOffset))
        NoRoutePoints += 1
        LapNo = QRpoint.LapNumber
        If LapNo <> CurrentLap Then
            LapPoints.Add(QRpoint)
            CurrentLap = LapNo
            NoLaps = LapNo
            tImgLapStartpoint = New Point(QRpoint.ImageX, QRpoint.ImageY + QRLogoYOffset)
            If NoLaps > 1 Then

                Dim LapVector As New ImgLapVector(PrevImgLapPoint, tImgLapStartpoint)

                ImgLapVectors.Add(LapVector)

            End If
            PrevImgLapPoint = tImgLapStartpoint
        End If
    End Sub
    Public Sub EndAddPoints()
        LapPoints.Add(RoutePoints(RoutePoints.Count - 1))
        tImgLapStartpoint = New Point(RoutePoints(RoutePoints.Count - 1).ImageX, RoutePoints(RoutePoints.Count - 1).ImageY + QRLogoYOffset)
        Dim LapVector As New ImgLapVector(PrevImgLapPoint, tImgLapStartpoint)

        ImgLapVectors.Add(LapVector)
    End Sub
End Class
