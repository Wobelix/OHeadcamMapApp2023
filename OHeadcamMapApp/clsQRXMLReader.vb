Imports System.Globalization
Imports System.Threading
Imports System.Xml
Imports OHeadcamMapApp.My.Resources

Public Class clsQRXMLReader



    Dim RoutePoints As clsQRRoutePoints
    Private Shared ReadOnly Invariant As CultureInfo = CultureInfo.InvariantCulture



    Dim doc As New XmlDocument()

    Public Sub New(ByRef iRoutepoints As clsQRRoutePoints)
        RoutePoints = iRoutepoints
    End Sub

    Private Function GetAttributeValue(sampleNode As XmlNode, attributeName As String) As String
        Dim attribute As XmlAttribute = sampleNode.Attributes(attributeName)
        If attribute Is Nothing Then Return Nothing
        Return attribute.Value
    End Function

    Private Function GetDateTimeAttribute(sampleNode As XmlNode, attributeName As String, Optional defaultValue As DateTime = Nothing) As DateTime
        Dim value As String = GetAttributeValue(sampleNode, attributeName)
        Dim result As DateTime
        If Not String.IsNullOrWhiteSpace(value) AndAlso DateTime.TryParse(value, Invariant, DateTimeStyles.None, result) Then
            Return result
        End If
        Return defaultValue
    End Function

    Private Function GetDoubleAttribute(sampleNode As XmlNode, attributeName As String, Optional defaultValue As Double = 0) As Double
        Dim value As String = GetAttributeValue(sampleNode, attributeName)
        Dim result As Double
        If Not String.IsNullOrWhiteSpace(value) AndAlso Double.TryParse(value, NumberStyles.Float Or NumberStyles.AllowThousands, Invariant, result) Then
            Return result
        End If
        Return defaultValue
    End Function

    Private Function GetIntegerAttribute(sampleNode As XmlNode, attributeName As String, Optional defaultValue As Integer = 0) As Integer
        Dim value As String = GetAttributeValue(sampleNode, attributeName)
        Dim result As Integer
        If Not String.IsNullOrWhiteSpace(value) AndAlso Integer.TryParse(value, NumberStyles.Integer, Invariant, result) Then
            Return result
        End If
        Return defaultValue
    End Function

    Private Function GetRoundedIntegerAttribute(sampleNode As XmlNode, attributeName As String, Optional defaultValue As Integer = 0) As Integer
        Dim value As String = GetAttributeValue(sampleNode, attributeName)
        Dim result As Double
        If Not String.IsNullOrWhiteSpace(value) AndAlso Double.TryParse(value, NumberStyles.Float Or NumberStyles.AllowThousands, Invariant, result) Then
            Return CInt(Math.Round(result))
        End If
        Return defaultValue
    End Function

    Public Function ReadXML(Filename As String) As Boolean
        Dim mainForm As MainForm = CType(Application.OpenForms("MainForm"), MainForm)
        If mainForm IsNot Nothing Then
            mainForm.StatusProgressBar1.Visible = True
            'mainForm.StatusBarProgressText.Text = "Begins reading XML"
        End If
        ReadXML = True
        Try
            doc.Load(Filename)
        Catch e As Exception
            MsgBox(Texts.ErrXML + e.Message)
            Return False
        End Try
        Dim i, CurrentLap, LapNo, i10 As Integer
        i = 0
        CurrentLap = -1
        LapNo = 0
        For Each sampleNode As XmlNode In doc.SelectNodes("/Route/Segment/Sample")
            Dim sample As New clsQRRoutePoint
            sample.Time = GetDateTimeAttribute(sampleNode, "time", DateTime.MinValue)
            sample.Longitude = GetDoubleAttribute(sampleNode, "longitude")
            sample.Latitude = GetDoubleAttribute(sampleNode, "latitude")
            sample.ElapsedTimeFromStart = GetRoundedIntegerAttribute(sampleNode, "elapsedTimeFromStart")
            sample.CircleTimeBackward = GetDoubleAttribute(sampleNode, "circleTimeBackward")
            sample.CircleTimeForward = GetDoubleAttribute(sampleNode, "circleTimeForward")
            sample.RouteDistanceFromStart = GetDoubleAttribute(sampleNode, "routeDistanceFromStart")
            sample.Pace = GetDoubleAttribute(sampleNode, "pace")
            sample.Speed = GetDoubleAttribute(sampleNode, "speed")
            sample.HeartRate = GetIntegerAttribute(sampleNode, "heartRate")
            sample.Altitude = GetDoubleAttribute(sampleNode, "altitude")
            sample.DirectionDeviationToNextLap = GetDoubleAttribute(sampleNode, "directionDeviationToNextLap")
            sample.Direction = GetDoubleAttribute(sampleNode, "direction")
            sample.Inclination = GetDoubleAttribute(sampleNode, "inclination")
            sample.AscentFromStart = GetDoubleAttribute(sampleNode, "ascentFromStart")
            sample.DescentFromStart = GetDoubleAttribute(sampleNode, "descentFromStart")
            sample.ImageX = GetDoubleAttribute(sampleNode, "imageX")
            sample.ImageY = GetDoubleAttribute(sampleNode, "imageY")
            sample.LapNumber = GetIntegerAttribute(sampleNode, "lapNumber")
            sample.ElapsedTime = GetRoundedIntegerAttribute(sampleNode, "elapsedTime")
            sample.RouteDistance = GetDoubleAttribute(sampleNode, "routeDistance")
            'sample.Time = DateTime.Parse(sampleNode.Attributes("time").Value, CultureInfo.InvariantCulture)
            'sample.Longitude = Double.Parse(sampleNode.Attributes("longitude").Value, CultureInfo.InvariantCulture)
            'sample.Latitude = Double.Parse(sampleNode.Attributes("latitude").Value, CultureInfo.InvariantCulture)
            'sample.ElapsedTimeFromStart = Integer.Parse(sampleNode.Attributes("elapsedTimeFromStart").Value, CultureInfo.InvariantCulture)
            'sample.CircleTimeBackward = Double.Parse(sampleNode.Attributes("circleTimeBackward").Value, CultureInfo.InvariantCulture)
            'sample.CircleTimeForward = Double.Parse(sampleNode.Attributes("circleTimeForward").Value, CultureInfo.InvariantCulture)
            'sample.RouteDistanceFromStart = Double.Parse(sampleNode.Attributes("routeDistanceFromStart").Value, CultureInfo.InvariantCulture)
            'sample.Pace = Double.Parse(sampleNode.Attributes("pace").Value, CultureInfo.InvariantCulture)
            'sample.Speed = Double.Parse(sampleNode.Attributes("speed").Value, CultureInfo.InvariantCulture)
            'sample.HeartRate = Integer.Parse(sampleNode.Attributes("heartRate").Value, CultureInfo.InvariantCulture)
            'sample.Altitude = Double.Parse(sampleNode.Attributes("altitude").Value, CultureInfo.InvariantCulture)
            'sample.DirectionDeviationToNextLap = Double.Parse(sampleNode.Attributes("directionDeviationToNextLap").Value, CultureInfo.InvariantCulture)
            'sample.Direction = Double.Parse(sampleNode.Attributes("direction").Value, CultureInfo.InvariantCulture)
            'sample.Inclination = Double.Parse(sampleNode.Attributes("inclination").Value, CultureInfo.InvariantCulture)
            'sample.AscentFromStart = Double.Parse(sampleNode.Attributes("ascentFromStart").Value, CultureInfo.InvariantCulture)
            'sample.DescentFromStart = Double.Parse(sampleNode.Attributes("descentFromStart").Value, CultureInfo.InvariantCulture)
            'sample.ImageX = Double.Parse(sampleNode.Attributes("imageX").Value, CultureInfo.InvariantCulture)
            'sample.ImageY = Double.Parse(sampleNode.Attributes("imageY").Value, CultureInfo.InvariantCulture)
            'sample.LapNumber = Integer.Parse(sampleNode.Attributes("lapNumber").Value, CultureInfo.InvariantCulture)
            'sample.ElapsedTime = Integer.Parse(sampleNode.Attributes("elapsedTime").Value, CultureInfo.InvariantCulture)
            'sample.RouteDistance = Double.Parse(sampleNode.Attributes("routeDistance").Value, CultureInfo.InvariantCulture)
            RoutePoints.AddPoint(sample)

            'If i Mod 10 = 0 Then
            'mainForm.StatusBarProgressText.Text = "Reading XML Line : " + CStr(i)
            'Application.DoEvents()
            'End If
            'i += 1
        Next
        RoutePoints.EndAddPoints()
        'mainForm.StatusBarProgressText.Text = "XML Finished"
        Application.DoEvents()
        Return True
    End Function
End Class
