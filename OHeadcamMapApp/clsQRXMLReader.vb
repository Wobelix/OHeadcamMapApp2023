Imports System.Globalization
Imports System.Threading
Imports System.Xml
Imports OHeadcamMapApp.My.Resources

Public Class clsQRXMLReader



    Dim RoutePoints As clsQRRoutePoints



    Dim doc As New XmlDocument()

    Public Sub New(ByRef iRoutepoints As clsQRRoutePoints)
        RoutePoints = iRoutepoints
    End Sub

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
            Try
                sample.Time = DateTime.Parse(sampleNode.Attributes("time").Value, CultureInfo.InvariantCulture)
            Catch ex As Exception
                ' Handle parsing failure for Time attribute
                sample.Time = DateTime.MinValue ' Assign a default value
            End Try

            Try
                sample.Longitude = Double.Parse(sampleNode.Attributes("longitude").Value, CultureInfo.InvariantCulture)
            Catch ex As Exception
                ' Handle parsing failure for Longitude attribute
                sample.Longitude = 0 ' Assign a default value
            End Try

            Try
                sample.Latitude = Double.Parse(sampleNode.Attributes("latitude").Value, CultureInfo.InvariantCulture)
            Catch ex As Exception
                ' Handle parsing failure for Latitude attribute
                sample.Latitude = 0 ' Assign a default value
            End Try

            Try
                sample.ElapsedTimeFromStart = CInt(Math.Round(Double.Parse(sampleNode.Attributes("elapsedTimeFromStart").Value, CultureInfo.InvariantCulture)))
            Catch ex As Exception
                ' Handle parsing failure for ElapsedTimeFromStart attribute
                sample.ElapsedTimeFromStart = 0 ' Assign a default value
            End Try

            Try
                sample.CircleTimeBackward = Double.Parse(sampleNode.Attributes("circleTimeBackward").Value, CultureInfo.InvariantCulture)
            Catch ex As Exception
                ' Handle parsing failure for CircleTimeBackward attribute
                sample.CircleTimeBackward = 0 ' Assign a default value
            End Try

            Try
                sample.CircleTimeForward = Double.Parse(sampleNode.Attributes("circleTimeForward").Value, CultureInfo.InvariantCulture)
            Catch ex As Exception
                ' Handle parsing failure for CircleTimeForward attribute
                sample.CircleTimeForward = 0 ' Assign a default value
            End Try

            Try
                sample.RouteDistanceFromStart = Double.Parse(sampleNode.Attributes("routeDistanceFromStart").Value, CultureInfo.InvariantCulture)
            Catch ex As Exception
                ' Handle parsing failure for RouteDistanceFromStart attribute
                sample.RouteDistanceFromStart = 0 ' Assign a default value
            End Try

            Try
                sample.Pace = Double.Parse(sampleNode.Attributes("pace").Value, CultureInfo.InvariantCulture)
            Catch ex As Exception
                ' Handle parsing failure for Pace attribute
                sample.Pace = 0 ' Assign a default value
            End Try

            Try
                sample.Speed = Double.Parse(sampleNode.Attributes("speed").Value, CultureInfo.InvariantCulture)
            Catch ex As Exception
                ' Handle parsing failure for Speed attribute
                sample.Speed = 0 ' Assign a default value
            End Try

            Try
                sample.HeartRate = Integer.Parse(sampleNode.Attributes("heartRate").Value, CultureInfo.InvariantCulture)
            Catch ex As Exception
                ' Handle parsing failure for HeartRate attribute
                sample.HeartRate = 0 ' Assign a default value
            End Try

            Try
                sample.Altitude = Double.Parse(sampleNode.Attributes("altitude").Value, CultureInfo.InvariantCulture)
            Catch ex As Exception
                ' Handle parsing failure for Altitude attribute
                sample.Altitude = 0 ' Assign a default value
            End Try

            Try
                sample.DirectionDeviationToNextLap = Double.Parse(sampleNode.Attributes("directionDeviationToNextLap").Value, CultureInfo.InvariantCulture)
            Catch ex As Exception
                ' Handle parsing failure for DirectionDeviationToNextLap attribute
                sample.DirectionDeviationToNextLap = 0 ' Assign a default value
            End Try

            Try
                sample.Direction = Double.Parse(sampleNode.Attributes("direction").Value, CultureInfo.InvariantCulture)
            Catch ex As Exception
                ' Handle parsing failure for Direction attribute
                sample.Direction = 0 ' Assign a default value
            End Try

            Try
                sample.Inclination = Double.Parse(sampleNode.Attributes("inclination").Value, CultureInfo.InvariantCulture)
            Catch ex As Exception
                sample.Inclination = 0
            End Try
            Try
                sample.AscentFromStart = Double.Parse(sampleNode.Attributes("ascentFromStart").Value, CultureInfo.InvariantCulture)
            Catch ex As Exception
                ' Handle parsing failure for AscentFromStart attribute
                sample.AscentFromStart = 0 ' Assign a default value
            End Try

            Try
                sample.DescentFromStart = Double.Parse(sampleNode.Attributes("descentFromStart").Value, CultureInfo.InvariantCulture)
            Catch ex As Exception
                ' Handle parsing failure for DescentFromStart attribute
                sample.DescentFromStart = 0 ' Assign a default value
            End Try

            Try
                sample.ImageX = Double.Parse(sampleNode.Attributes("imageX").Value, CultureInfo.InvariantCulture)
            Catch ex As Exception
                ' Handle parsing failure for ImageX attribute
                sample.ImageX = 0 ' Assign a default value
            End Try

            Try
                sample.ImageY = Double.Parse(sampleNode.Attributes("imageY").Value, CultureInfo.InvariantCulture)
            Catch ex As Exception
                ' Handle parsing failure for ImageY attribute
                sample.ImageY = 0 ' Assign a default value
            End Try

            Try
                sample.LapNumber = Integer.Parse(sampleNode.Attributes("lapNumber").Value, CultureInfo.InvariantCulture)
            Catch ex As Exception
                ' Handle parsing failure for LapNumber attribute
                sample.LapNumber = 0 ' Assign a default value
            End Try

            Try
                sample.ElapsedTime = CInt(Math.Round(Double.Parse(sampleNode.Attributes("elapsedTime").Value, CultureInfo.InvariantCulture)))
            Catch ex As Exception
                ' Handle parsing failure for ElapsedTime attribute
                sample.ElapsedTime = 0 ' Assign a default value
            End Try

            Try
                sample.RouteDistance = Double.Parse(sampleNode.Attributes("routeDistance").Value, CultureInfo.InvariantCulture)
            Catch ex As Exception
                ' Handle parsing failure for RouteDistance attribute
                sample.RouteDistance = 0 ' Assign a default value
            End Try
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
    End Function
End Class
