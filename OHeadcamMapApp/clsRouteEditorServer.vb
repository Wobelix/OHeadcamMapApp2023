Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Web.Script.Serialization

Public Class clsRouteEditorServer
    Implements IDisposable
    Private ReadOnly _webRoot, _exportFolder As String
    Private ReadOnly _projectLinkPath As String
    Private _projectPath As String
    Private ReadOnly _onExported As Action(Of String, String)
    Private ReadOnly _listener As New TcpListener(IPAddress.Loopback, 0)
    Private _running As Boolean
    Public Property Url As String

    Public Sub New(webRoot As String, exportFolder As String, projectPath As String, projectLinkPath As String, onExported As Action(Of String, String))
        _webRoot = Path.GetFullPath(webRoot) : _exportFolder = Path.GetFullPath(exportFolder) : _onExported = onExported
        If Not String.IsNullOrWhiteSpace(projectLinkPath) Then _projectLinkPath = Path.GetFullPath(projectLinkPath)
        If Not String.IsNullOrWhiteSpace(projectPath) Then _projectPath = Path.GetFullPath(projectPath)
        If Not String.IsNullOrWhiteSpace(_projectLinkPath) AndAlso File.Exists(_projectLinkPath) Then
            Dim linkedName = Path.GetFileName(File.ReadAllText(_projectLinkPath).Trim())
            Dim linkedPath = Path.Combine(_exportFolder, linkedName)
            If File.Exists(linkedPath) Then _projectPath = linkedPath
        End If
    End Sub
    Public Sub Start()
        Directory.CreateDirectory(_exportFolder) : _listener.Start() : _running = True
        Url = "http://127.0.0.1:" & DirectCast(_listener.LocalEndpoint, IPEndPoint).Port & "/"
        _listener.BeginAcceptTcpClient(AddressOf ClientReady, Nothing)
    End Sub
    Private Sub ClientReady(ar As IAsyncResult)
        If Not _running Then Return
        Dim client As TcpClient = Nothing
        Try
            client = _listener.EndAcceptTcpClient(ar)
            If _running Then _listener.BeginAcceptTcpClient(AddressOf ClientReady, Nothing)
            HandleClient(client)
        Catch
            If client IsNot Nothing Then client.Close()
        End Try
    End Sub
    Private Sub HandleClient(client As TcpClient)
        Using client
            client.ReceiveTimeout = 15000 : client.SendTimeout = 15000
            Dim stream = client.GetStream(), headerBytes As New List(Of Byte)(), matched As Integer = 0, marker() As Byte = {13, 10, 13, 10}
            While headerBytes.Count < 65536 AndAlso matched < 4
                Dim value = stream.ReadByte() : If value < 0 Then Return
                headerBytes.Add(CByte(value))
                If value = marker(matched) Then matched += 1 Else matched = If(value = 13, 1, 0)
            End While
            Dim lines() = Encoding.ASCII.GetString(headerBytes.ToArray()).Split(New String() {vbCrLf}, StringSplitOptions.RemoveEmptyEntries)
            If lines.Length = 0 Then Return
            Dim first() = lines(0).Split(" "c) : If first.Length < 2 Then Return
            Dim method = first(0).ToUpperInvariant(), requestPath = first(1).Split("?"c)(0), contentLength As Integer = 0
            For Each line In lines.Skip(1)
                If line.StartsWith("Content-Length:", StringComparison.OrdinalIgnoreCase) Then Integer.TryParse(line.Substring(line.IndexOf(":"c) + 1).Trim(), contentLength)
            Next
            Dim body(Math.Max(0, contentLength) - 1) As Byte, offset As Integer = 0
            While offset < contentLength
                Dim count = stream.Read(body, offset, contentLength - offset) : If count <= 0 Then Exit While
                offset += count
            End While
            If method = "POST" AndAlso requestPath = "/export" Then
                HandleExport(stream, Encoding.UTF8.GetString(body, 0, offset))
            ElseIf method = "POST" AndAlso requestPath = "/project" Then
                HandleProjectSave(stream, Encoding.UTF8.GetString(body, 0, offset))
            ElseIf method = "GET" AndAlso requestPath = "/project" Then
                HandleProjectLoad(stream)
            ElseIf method = "GET" Then
                HandleStaticFile(stream, requestPath)
            Else
                WriteResponse(stream, 405, "text/plain", Encoding.UTF8.GetBytes("Method not allowed"))
            End If
        End Using
    End Sub
    Private Sub HandleStaticFile(stream As NetworkStream, requestPath As String)
        Dim relativePath = Uri.UnescapeDataString(requestPath).TrimStart("/"c).Replace("/"c, Path.DirectorySeparatorChar)
        If relativePath = "" Then relativePath = "index.html"
        Dim rootPath = _webRoot.TrimEnd(Path.DirectorySeparatorChar) & Path.DirectorySeparatorChar
        Dim filePath = Path.GetFullPath(Path.Combine(rootPath, relativePath))
        If Not filePath.StartsWith(rootPath, StringComparison.OrdinalIgnoreCase) Then WriteResponse(stream, 403, "text/plain", Encoding.UTF8.GetBytes("Forbidden")) : Return
        If Not File.Exists(filePath) Then WriteResponse(stream, 404, "text/plain", Encoding.UTF8.GetBytes("Not found")) : Return
        WriteResponse(stream, 200, ContentType(Path.GetExtension(filePath)), File.ReadAllBytes(filePath))
    End Sub
    Private Sub HandleExport(stream As NetworkStream, json As String)
        Try
            Dim serializer As New JavaScriptSerializer() With {.MaxJsonLength = Integer.MaxValue}
            Dim request = serializer.Deserialize(Of ExportRequest)(json), safeName = SanitizeName(request.name)
            Dim extensionName = If(String.Equals(request.imageExtension, "png", StringComparison.OrdinalIgnoreCase), "png", "jpg")
            Dim imagePath = Path.Combine(_exportFolder, safeName & "." & extensionName), xmlPath = Path.Combine(_exportFolder, safeName & ".xml")
            File.WriteAllBytes(imagePath, Convert.FromBase64String(request.imageBase64))
            File.WriteAllText(xmlPath, request.xml, New UTF8Encoding(False))
            If Not String.IsNullOrWhiteSpace(request.projectJson) Then SaveProject(safeName, request.projectJson)
            If _onExported IsNot Nothing Then _onExported(xmlPath, imagePath)
            WriteJson(stream, 200, New With {.xmlName = Path.GetFileName(xmlPath), .imageName = Path.GetFileName(imagePath), .projectName = If(_projectPath Is Nothing, "", Path.GetFileName(_projectPath))})
        Catch ex As Exception
            WriteJson(stream, 500, New With {.error = ex.Message})
        End Try
    End Sub
    Private Sub HandleProjectLoad(stream As NetworkStream)
        If String.IsNullOrWhiteSpace(_projectPath) OrElse Not File.Exists(_projectPath) Then
            WriteResponse(stream, 204, "application/json; charset=utf-8", New Byte() {})
            Return
        End If
        WriteResponse(stream, 200, "application/json; charset=utf-8", File.ReadAllBytes(_projectPath))
    End Sub
    Private Sub HandleProjectSave(stream As NetworkStream, json As String)
        Try
            Dim serializer As New JavaScriptSerializer() With {.MaxJsonLength = Integer.MaxValue}
            Dim request = serializer.Deserialize(Of ProjectRequest)(json)
            Dim filePath = SaveProject(SanitizeName(request.name), request.projectJson)
            WriteJson(stream, 200, New With {.projectName = Path.GetFileName(filePath)})
        Catch ex As Exception
            WriteJson(stream, 500, New With {.error = ex.Message})
        End Try
    End Sub
    Private Function SaveProject(safeName As String, projectJson As String) As String
        If String.IsNullOrWhiteSpace(projectJson) Then Throw New InvalidDataException("The project is empty.")
        _projectPath = Path.Combine(_exportFolder, safeName & ".routeproject")
        File.WriteAllText(_projectPath, projectJson, New UTF8Encoding(False))
        If Not String.IsNullOrWhiteSpace(_projectLinkPath) Then File.WriteAllText(_projectLinkPath, Path.GetFileName(_projectPath), New UTF8Encoding(False))
        Return _projectPath
    End Function
    Private Shared Function SanitizeName(value As String) As String
        Dim result = If(String.IsNullOrWhiteSpace(value), "headcam-route", Path.GetFileNameWithoutExtension(value.Trim()))
        For Each invalidChar In Path.GetInvalidFileNameChars() : result = result.Replace(invalidChar, "-"c) : Next
        Return If(String.IsNullOrWhiteSpace(result), "headcam-route", result)
    End Function
    Private Shared Sub WriteJson(stream As NetworkStream, status As Integer, value As Object)
        WriteResponse(stream, status, "application/json; charset=utf-8", Encoding.UTF8.GetBytes(New JavaScriptSerializer().Serialize(value)))
    End Sub
    Private Shared Sub WriteResponse(stream As NetworkStream, status As Integer, contentTypeName As String, body() As Byte)
        Dim reason = If(status = 200, "OK", If(status = 404, "Not Found", "Error"))
        Dim header = "HTTP/1.1 " & status & " " & reason & vbCrLf & "Content-Type: " & contentTypeName & vbCrLf & "Content-Length: " & body.Length & vbCrLf & "Cache-Control: no-store" & vbCrLf & "Connection: close" & vbCrLf & vbCrLf
        Dim headerBytes = Encoding.ASCII.GetBytes(header) : stream.Write(headerBytes, 0, headerBytes.Length) : stream.Write(body, 0, body.Length)
    End Sub
    Private Shared Function ContentType(extensionName As String) As String
        If extensionName = ".html" Then Return "text/html; charset=utf-8"
        If extensionName = ".css" Then Return "text/css; charset=utf-8"
        If extensionName = ".js" Then Return "application/javascript; charset=utf-8"
        If extensionName = ".svg" Then Return "image/svg+xml"
        Return "application/octet-stream"
    End Function
    Public Sub Dispose() Implements IDisposable.Dispose
        _running = False : Try : _listener.Stop() : Catch : End Try
    End Sub
    Private Class ExportRequest
        Public Property name As String
        Public Property imageExtension As String
        Public Property imageBase64 As String
        Public Property xml As String
        Public Property projectJson As String
    End Class
    Private Class ProjectRequest
        Public Property name As String
        Public Property projectJson As String
    End Class
End Class
