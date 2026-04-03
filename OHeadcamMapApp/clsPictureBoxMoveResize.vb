Public Class clsPictureBoxMoveResize
    Private isResizingX As Boolean = False, isResizingY As Boolean = False, isResizingXY As Boolean = False, isMoving As Boolean = False
    Private isUp As Boolean = False, IsLeft As Boolean = False
    Private lastLocation, resizeStart, resizeStartPos, prevdelta As Point
    Private lastX, lastY As Integer
    Private resizeStartSize, prevsize As Size
    Enum Direction
        Topleft
        TopRight
        Left
        Right
        Top
        Bottom
        BottomLeft
        BottomRight
        Middle
    End Enum
    Private MouseDir As Direction
    Private MDown As Boolean = False

    Public Function IsResizing() As Boolean
        Return isResizingX Or isResizingXY Or isResizingY
    End Function
    Public Sub PB_MouseDown(sender As Object, e As MouseEventArgs)
        Dim pb As PictureBox = DirectCast(sender, PictureBox)

        If e.Button = MouseButtons.Left And Not MDown Then
            MouseDir = Direction.Middle
            isMoving = False
            'MDown = True
            resizeStart = New Point(e.X, e.Y)
            resizeStartSize = pb.Size
            resizeStartPos = pb.Location
            If e.X >= pb.Width - 5 And e.Y >= pb.Height - 5 Then
                MouseDir = Direction.BottomRight
                isResizingXY = True
            ElseIf e.X <= 5 And e.Y <= 5 Then
                'pb.Cursor = Cursors.SizeNWSE
                MouseDir = Direction.Topleft
                isResizingXY = True
            ElseIf e.X <= 5 And e.Y >= pb.Height - 5 Then
                'pb.Cursor = Cursors.SizeNESW
                MouseDir = Direction.BottomLeft
                isResizingXY = True
            ElseIf e.X >= pb.Width - 5 And e.Y <= 5 Then
                'pb.Cursor = Cursors.SizeNESW
                MouseDir = Direction.TopRight
                isResizingXY = True
            ElseIf e.X >= 5 And e.X <= pb.Width - 5 And e.Y >= pb.Height - 5 Then
                'pb.Cursor = Cursors.SizeNS
                MouseDir = Direction.Bottom
                isResizingY = True

            ElseIf e.X >= 5 And e.X <= pb.Width - 5 And e.Y <= 5 Then
                'pb.Cursor = Cursors.SizeNS
                MouseDir = Direction.Top
                isResizingY = True
            ElseIf e.X <= 5 And e.Y >= 5 And e.Y <= pb.Height - 5 Then
                'pb.Cursor = Cursors.SizeWE
                MouseDir = Direction.Left
                isResizingX = True


            ElseIf e.X >= pb.Width - 5 And e.Y >= 5 And e.Y <= pb.Height - 5 Then
                'pb.Cursor = Cursors.SizeWE
                MouseDir = Direction.Right
                isResizingX = True
            Else
                'pb.Cursor = Cursors.SizeAll
                isMoving = True
                isResizingX = False
                isResizingY = False
                isResizingXY = False

            End If

        End If
        lastLocation = e.Location
    End Sub

    Public Sub PB_MouseMove(sender As Object, e As MouseEventArgs)
        Dim pb As PictureBox = DirectCast(sender, PictureBox)
        'Dim ff As ffplay = DirectCast(sender.parent, ffplay)
        Dim pbtop, pbheight, pbleft, pbwidth As Integer
        If isResizingX Or isResizingXY Or isResizingY Then

            Dim deltaX As Integer = e.X - resizeStart.X
            Dim deltaY As Integer = e.Y - resizeStart.Y


            pbtop = pb.Top
            pbleft = pb.Left
            pbwidth = pb.Width
            pbheight = pb.Height
            'ff.Label1.Text = e.X.ToString + "," + e.Y.ToString + "Start: " + resizeStart.ToString + "--" + MouseDir.ToString
            If MouseDir = Direction.Top Or MouseDir = Direction.Topleft Or MouseDir = Direction.TopRight Then
                pbtop += e.Y - resizeStart.Y
                pbheight -= e.Y - resizeStart.Y
            End If
            If MouseDir = Direction.TopRight Or MouseDir = Direction.BottomRight Or MouseDir = Direction.Right Then
                pbwidth = e.X
            End If
            If MouseDir = Direction.Topleft Or MouseDir = Direction.Left Or MouseDir = Direction.BottomLeft Then
                pbwidth -= deltaX
                pbleft += deltaX
            End If
            If MouseDir = Direction.BottomRight Or MouseDir = Direction.Bottom Or MouseDir = Direction.BottomLeft Then
                pbheight = resizeStartSize.Height - (resizeStart.Y - e.Y)
            End If

            If pbwidth < 30 Then pbwidth = 30
            If pbheight < 40 Then pbheight = 40
            ' Ensure that the PictureBox remains inside its parent
            If pbleft < 0 Then
                pbleft = 0
            End If


            If pbtop < 0 Then
                pbtop = 0

            End If
            If pbleft + pbwidth > pb.Parent.ClientSize.Width Then

                pbleft = pb.Parent.ClientSize.Width - pbwidth

            End If
            If pbtop + pbheight > pb.Parent.ClientSize.Height Then

                pbtop = pb.Parent.ClientSize.Height - pb.Height

            End If
            pb.Top = pbtop
            pb.Left = pbleft
            pb.Width = pbwidth
            pb.Height = pbheight
        ElseIf isMoving Then
            pb.Left += e.X - lastLocation.X
            pb.Top += e.Y - lastLocation.Y

        End If
        If Not (isResizingX Or isResizingXY Or isResizingY Or isMoving) Then
            If e.X >= pb.Width - 5 And e.Y >= pb.Height - 5 Then
                pb.Cursor = Cursors.SizeNWSE
            ElseIf e.X <= 5 And e.Y <= 5 Then
                pb.Cursor = Cursors.SizeNWSE
            ElseIf e.X <= 5 And e.Y >= pb.Height - 5 Then
                pb.Cursor = Cursors.SizeNESW
            ElseIf e.X >= pb.Width - 5 And e.Y <= 5 Then
                pb.Cursor = Cursors.SizeNESW
            ElseIf e.X >= 5 And e.X <= pb.Width - 5 And e.Y >= pb.Height - 5 Then
                pb.Cursor = Cursors.SizeNS

            ElseIf e.X >= 5 And e.X <= pb.Width - 5 And e.Y <= 5 Then
                pb.Cursor = Cursors.SizeNS

            ElseIf e.X <= 5 And e.Y >= 5 And e.Y <= pb.Height - 5 Then
                pb.Cursor = Cursors.SizeWE

            ElseIf e.X >= pb.Width - 5 And e.Y >= 5 And e.Y <= pb.Height - 5 Then
                pb.Cursor = Cursors.SizeWE

            Else
                pb.Cursor = Cursors.SizeAll


            End If
        End If

    End Sub

    Public Sub PB_MouseUp(sender As Object, e As MouseEventArgs)
        Dim pb As PictureBox = DirectCast(sender, PictureBox)
        isResizingX = False
        isResizingXY = False
        isResizingY = False
        isMoving = False
        MDown = False
        pb.Cursor = Cursors.Default

        If pb.Left < 0 Then
            pb.Left = 0
        End If
        If pb.Top < 0 Then
            pb.Top = 0
        End If
        If pb.Left + pb.Width > pb.Parent.ClientSize.Width Then
            pb.Left = pb.Parent.ClientSize.Width - pb.Width
        End If
        If pb.Top + pb.Height > pb.Parent.ClientSize.Height Then
            pb.Top = pb.Parent.ClientSize.Height - pb.Height
        End If
    End Sub


End Class
