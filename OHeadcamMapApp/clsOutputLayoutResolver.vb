Imports System.Globalization

<Flags()>
Public Enum MapFrameAttachedEdge
    None = 0
    Left = 1
    Right = 2
    Top = 4
    Bottom = 8
End Enum

Public NotInheritable Class clsOutputLayoutResolver
    Public Const PortraitWidth As Integer = 1080
    Public Const PortraitHeight As Integer = 1920

    Private Sub New()
    End Sub

    Public Shared ReadOnly Property IsPortrait As Boolean
        Get
            Return My.Settings.OutputAspectIndex = 1
        End Get
    End Property

    Public Shared Function GetOutputSize(landscapeSize As Size) As Size
        If IsPortrait Then Return New Size(PortraitWidth, PortraitHeight)
        Return landscapeSize
    End Function

    Public Shared Function GetBounds(key As String, fallbackPosition As Point, fallbackWidth As Integer, fallbackHeight As Integer) As Rectangle
        If Not IsPortrait Then Return New Rectangle(fallbackPosition, New Size(Math.Max(1, fallbackWidth), Math.Max(1, fallbackHeight)))
        Dim parsed As Rectangle
        If TryGetPortraitBounds(key, parsed) Then Return parsed
        Return New Rectangle(Math.Max(0, Math.Min(PortraitWidth - 1, fallbackPosition.X)),
                             Math.Max(0, Math.Min(PortraitHeight - 1, fallbackPosition.Y)),
                             Math.Max(1, Math.Min(PortraitWidth, fallbackWidth)),
                             Math.Max(1, Math.Min(PortraitHeight, fallbackHeight)))
    End Function

    Public Shared Function TryGetPortraitBounds(key As String, ByRef bounds As Rectangle) As Boolean
        Dim serialized As String = My.Settings.PortraitOverlayLayout
        If String.IsNullOrWhiteSpace(serialized) Then Return False
        For Each section As String In serialized.Split("|"c)
            Dim equalsIndex As Integer = section.IndexOf("="c)
            If equalsIndex <= 0 OrElse Not String.Equals(section.Substring(0, equalsIndex), key, StringComparison.OrdinalIgnoreCase) Then Continue For
            Dim values() As String = section.Substring(equalsIndex + 1).Split(","c)
            If values.Length <> 4 Then Return False
            Dim x, y, width, height As Integer
            If Integer.TryParse(values(0), NumberStyles.Integer, CultureInfo.InvariantCulture, x) AndAlso
               Integer.TryParse(values(1), NumberStyles.Integer, CultureInfo.InvariantCulture, y) AndAlso
               Integer.TryParse(values(2), NumberStyles.Integer, CultureInfo.InvariantCulture, width) AndAlso
               Integer.TryParse(values(3), NumberStyles.Integer, CultureInfo.InvariantCulture, height) Then
                bounds = New Rectangle(Math.Max(0, x), Math.Max(0, y), Math.Max(1, width), Math.Max(1, height))
                Return True
            End If
            Return False
        Next
        Return False
    End Function

    Public Shared Function GetPortraitCropFilter() As String
        If My.Settings.PortraitModeIndex = 1 Then
            Dim verticalPosition As Double = Math.Max(0.0R, Math.Min(1.0R, My.Settings.PortraitVideoPosition))
            Return "scale=" & PortraitWidth.ToString(CultureInfo.InvariantCulture) & ":-2" &
                   ",pad=" & PortraitWidth.ToString(CultureInfo.InvariantCulture) & ":" & PortraitHeight.ToString(CultureInfo.InvariantCulture) &
                   ":0:(oh-ih)*" & verticalPosition.ToString("0.########", CultureInfo.InvariantCulture) & ":color=black"
        End If
        Dim position As Double = Math.Max(0.0R, Math.Min(1.0R, My.Settings.PortraitFramePosition))
        Return "scale=-2:" & PortraitHeight.ToString(CultureInfo.InvariantCulture) &
               ",crop=" & PortraitWidth.ToString(CultureInfo.InvariantCulture) & ":" & PortraitHeight.ToString(CultureInfo.InvariantCulture) &
               ":(iw-" & PortraitWidth.ToString(CultureInfo.InvariantCulture) & ")*" & position.ToString("0.########", CultureInfo.InvariantCulture) & ":0"
    End Function

    Public Shared Function GetFrameAttachedEdge(bounds As Rectangle, canvasSize As Size) As MapFrameAttachedEdge
        If My.Settings.MIMapFrameFormIndex <> 1 AndAlso My.Settings.MIMapFrameFormIndex <> 2 Then Return MapFrameAttachedEdge.None
        Const tolerance As Integer = 3
        Dim touchesLeft As Boolean = bounds.Left <= tolerance
        Dim touchesRight As Boolean = bounds.Right >= canvasSize.Width - tolerance
        Dim touchesTop As Boolean = bounds.Top <= tolerance
        Dim touchesBottom As Boolean = bounds.Bottom >= canvasSize.Height - tolerance

        Dim attachedEdges As MapFrameAttachedEdge = MapFrameAttachedEdge.None
        ' At most one horizontal and one vertical edge are selected. Their
        ' combination represents attachment to a video corner.
        If touchesLeft Then
            attachedEdges = attachedEdges Or MapFrameAttachedEdge.Left
        ElseIf touchesRight Then
            attachedEdges = attachedEdges Or MapFrameAttachedEdge.Right
        End If
        If touchesTop Then
            attachedEdges = attachedEdges Or MapFrameAttachedEdge.Top
        ElseIf touchesBottom Then
            attachedEdges = attachedEdges Or MapFrameAttachedEdge.Bottom
        End If
        Return attachedEdges
    End Function
End Class
