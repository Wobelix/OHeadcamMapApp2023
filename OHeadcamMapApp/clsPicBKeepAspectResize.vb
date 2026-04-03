Public Class clsPicBKeepAspectResize
    Private originalSize As Size
    Private PB As PictureBox
    Private OrigFrmSize As Size
    Private MarginR, MarginL, MarginT, MarginB As Integer
    Private WidthFactor As Single
    Private heightfactor As Single
    Private aspectRatio As Single
    Public Sub OnFormLoad(PBin As PictureBox, ByVal FrmClientSize As Size)
        PB = PBin
        originalSize = PB.Size
        OrigFrmSize = FrmClientSize
        WidthFactor = originalSize.Width / FrmClientSize.Width
        heightfactor = originalSize.Height / FrmClientSize.Height
        MarginL = PB.Location.X
        MarginR = FrmClientSize.Width - PB.Size.Width - MarginL
        MarginT = PB.Location.Y
        MarginB = FrmClientSize.Height - PB.Size.Height - MarginT
        aspectRatio = originalSize.Width / originalSize.Height

    End Sub

    Public Sub SetNewAspect()
        aspectRatio = PB.Size.Width / PB.Size.Height
    End Sub
    Public Sub OnFormResize(sender As Object)
        If IsNothing(sender) Or IsNothing(PB) Then Return
        Dim TheForm As Form = sender
        ' Calculate the new size for the PictureBox while maintaining the aspect ratio



        ' The form is smaller than the original PictureBox size, calculate new size based on the form size
        Dim newWidth As Integer = TheForm.ClientSize.Width - MarginL - MarginR
        Dim newHeight As Integer = CInt(newWidth / aspectRatio)
        If newHeight + MarginT + MarginB > TheForm.ClientSize.Height Then

            ' The calculated height is too large, so calculate new size based on the form height
            newHeight = TheForm.ClientSize.Height - MarginT - MarginB
            newWidth = CInt(newHeight * aspectRatio)
        End If

        PB.Size = New Size(newWidth, newHeight)


        ' Position the PictureBox at the top left corner
        'PB.Location = New Point(0, 0)
    End Sub
End Class
