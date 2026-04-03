Imports System.Drawing.Imaging
Imports System.Text
Public Class clsTxtFiles
    Function FindTimeSpanQRXML(filename As String) As String
        Dim fileReader As String, p1 As Integer, res, res2 As String
        fileReader = My.Computer.FileSystem.ReadAllText(filename)
        p1 = InStr(fileReader, "Sample time=")
        res = Mid(fileReader, p1 + 24, 8)

        p1 = InStrRev(fileReader, "Sample time=")
        res2 = Mid(fileReader, p1 + 24, 8)

        Dim myTime = DateTime.Parse(res)
        Dim myTime2 = DateTime.Parse(res2)

        Dim result = myTime2 - myTime
        FindTimeSpanQRXML = CStr(result.TotalSeconds)

    End Function
    Public Function GetImageInfo(Filename As String) As String
        Dim imageFilePath As String = "path\to\image.jpg"
        Dim image As Image = Image.FromFile(Filename)


        Dim longitude = GetCoordinateDouble(image.PropertyItems.Single(Function(p) p.Id = 4))
        Dim latitude = GetCoordinateDouble(image.PropertyItems.Single(Function(p) p.Id = 2))

        ' read all the property items
        Dim propertyItems As PropertyItem() = image.PropertyItems

        ' iterate over all the property items and print out their names and values
        For Each propertyItem As PropertyItem In propertyItems
            ' read the property name and value
            'Dim propertyName As String = GetPropertyName(PropertyItem.Id)
            Dim propertyValue As Object = GetPropertyValue(propertyItem)
            'prop = propertyValue.ToString
            ' print out the property name and value
            'Console.WriteLine(propertyName & ": " & propertyValue.ToString())
        Next

        ' dispose the image object to free up resources
        image.Dispose()
    End Function

    ' utility function to get the property name from its ID
    Private Function GetPropertyName(id As Integer) As String
        Select Case id
        ' add cases for all the property IDs that you're interested in
            Case &H10F ' ImageDescription
                Return "ImageDescription"
            Case &H110 ' EquipmentMake
                Return "EquipmentMake"
            Case &H111 ' EquipmentModel
                Return "EquipmentModel"
            Case &H132 ' DateTime
                Return "DateTime"
            Case Else
                Return "Unknown"
        End Select
    End Function

    ' utility function to get the property value as a string
    Private Function GetPropertyValue(propertyItem As PropertyItem) As Object
        Select Case propertyItem.Type
            Case 2 ' ASCII string
                Return Encoding.ASCII.GetString(propertyItem.Value)
            Case 3 ' short
                Return BitConverter.ToInt16(propertyItem.Value, 0)
            Case 4 ' long
                Return BitConverter.ToInt32(propertyItem.Value, 0)

            Case 5 ' rational
                If propertyItem.Len = 24 Then ' GPS coordinate
                    ' extract the coordinate values from the byte array
                    ' extract the coordinate values from the byte array
                    Dim latDeg As Double = BitConverter.ToUInt32(propertyItem.Value, 0)
                    Dim latMin As Double = BitConverter.ToUInt32(propertyItem.Value, 4)
                    Dim latSec As Double = BitConverter.ToUInt32(propertyItem.Value, 8)
                    Dim latRef As Byte = propertyItem.Value(12)
                    Dim lonDeg As Double = BitConverter.ToUInt32(propertyItem.Value, 13)
                    Dim lonMin As Double = BitConverter.ToUInt32(propertyItem.Value, 17)
                    Dim lonSec As Double = BitConverter.ToUInt32(propertyItem.Value, 21)
                    Dim lonRef As Byte = propertyItem.Value(25)

                    ' convert the latitude and longitude coordinates to decimal values
                    Dim latDecimal As Double = latDeg + (latMin / 60) + (latSec / 3600)
                    Dim lonDecimal As Double = lonDeg + (lonMin / 60) + (lonSec / 3600)

                    ' determine the direction (N/S or E/W) of the coordinates
                    Dim latDir As String = If(latRef = AscW("N"), "N", "S")
                    Dim lonDir As String = If(lonRef = AscW("E"), "E", "W")

                    ' return the formatted coordinate string
                    Return String.Format("{0:F6} {1}, {2:F6} {3}", latDecimal, latDir, lonDecimal, lonDir)
                Else
                    Dim numerator As Integer = BitConverter.ToInt32(propertyItem.Value, 0)
                    Dim denominator As Integer = BitConverter.ToInt32(propertyItem.Value, 4)
                    Return numerator / denominator
                End If
            Case Else
                Return "Unknown"
        End Select

    End Function
    Private Function GetCoordinateDouble(propItem As PropertyItem) As Double
        Dim degreesNumerator As UInteger = BitConverter.ToUInt32(propItem.Value, 0)
        Dim degreesDenominator As UInteger = BitConverter.ToUInt32(propItem.Value, 4)
        Dim degrees As Double = degreesNumerator / degreesDenominator

        Dim minutesNumerator As UInteger = BitConverter.ToUInt32(propItem.Value, 8)
        Dim minutesDenominator As UInteger = BitConverter.ToUInt32(propItem.Value, 12)
        Dim minutes As Double = minutesNumerator / minutesDenominator

        Dim secondsNumerator As UInteger = BitConverter.ToUInt32(propItem.Value, 16)
        Dim secondsDenominator As UInteger = BitConverter.ToUInt32(propItem.Value, 20)
        Dim seconds As Double = secondsNumerator / secondsDenominator

        Dim coorditate As Double = degrees + (minutes / 60.0) + (seconds / 3600.0)
        Dim gpsRef As String = System.Text.Encoding.ASCII.GetString(New Byte() {propItem.Value(0)}) 'N, S, E, or W

        If gpsRef = "S" OrElse gpsRef = "W" Then
            coorditate = coorditate * -1
        End If

        Return coorditate
    End Function
End Class
