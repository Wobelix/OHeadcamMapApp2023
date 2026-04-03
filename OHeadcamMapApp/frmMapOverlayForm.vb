Imports System.Drawing.Imaging

Public Class frmMapOverlayForm

    Dim LegMapLeft, LegMapBottom, RouteMapRight, RouteMapBottom, HeightGLeft, HeightGBottom, SpeedRight, SpeedBottom As Integer
    Dim LegMapHeight, LegMapWidth, RouteMapWidth, RouteMapHeight, HeightGWidth, HeightGHEight, SpeedWidth, SpeedHeight As Integer
    ReadOnly imgLegMap As Image = My.Resources.legmapimg
    Dim imgRoute As Image = My.Resources.routeimg
    ReadOnly imgSpeed As Image = My.Resources.panelimg
    Shared ReadOnly imgGraph As Bitmap = My.Resources.graphimg
    Dim bUseVideoMap As Boolean
    Dim bFormReady As Boolean = False

    Public Sub New(ByRef MapInfo_clsExtra As clsExtra)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        MapInfo = MapInfo_clsExtra
        bUseVideoMap = My.Settings.bUseMapTrackingVideo
        If bUseVideoMap Then
            imgRoute = Bitmap.FromFile(My.Settings.TrackMapImageFile)

            GroupBox2.Enabled = False
            GroupBox1.Enabled = False
            GroupBox4.Enabled = False
            PB_HeightGraph.Visible = False
            PB_LegMap.Visible = False
            PB_SpeedPanel.Visible = False
        End If
        SetAllPBTansparent()
        SetVideoScale()
        GetPictSizes()

        'UpdateAllImages()

    End Sub

    Private Sub numScale_ValueChanged(sender As Object, e As EventArgs) Handles numScale.ValueChanged
        If bFormReady Then UpdateAllImages()
    End Sub


    Dim bPBMove As Boolean


    Private Sub PB_MouseMove(sender As Object, e As MouseEventArgs) Handles PB_Route.MouseMove, PB_SpeedPanel.MouseMove, PB_LegMap.MouseMove, PB_HeightGraph.MouseMove
        Dim controlo As Control = DirectCast(sender, Control)
        Static mouseX, mouseY As Integer
        If e.Button = 0 Then
            mouseX = e.X
            mouseY = e.Y
        Else
            controlo.Left = controlo.Left + (e.X - mouseX)
            controlo.Top = controlo.Top + (e.Y - mouseY)
        End If
    End Sub

    Private Sub cbShowLeg_CheckedChanged(sender As Object, e As EventArgs) Handles cbShowLeg.CheckedChanged
        If cbShowLeg.Checked Then
            PB_LegMap.Visible = True
        Else
            PB_LegMap.Visible = False
        End If
    End Sub

    Private Sub cbShowRoute_CheckedChanged(sender As Object, e As EventArgs) Handles cbShowRoute.CheckedChanged
        If cbShowRoute.Checked Then
            PB_Route.Visible = True
        Else
            PB_Route.Visible = False

        End If
    End Sub

    Private Sub cbShowSpeed_CheckedChanged(sender As Object, e As EventArgs) Handles cbShowSpeed.CheckedChanged
        If cbShowSpeed.Checked Then
            PB_SpeedPanel.Visible = True
        Else
            PB_SpeedPanel.Visible = False
        End If
    End Sub

    Private Sub cbShowHeight_CheckedChanged(sender As Object, e As EventArgs) Handles cbShowHeight.CheckedChanged
        If cbShowHeight.Checked Then
            PB_HeightGraph.Visible = True
        Else
            PB_HeightGraph.Visible = False
        End If
    End Sub
    Public Shared Function ChangeOpacity(ByVal img As Image, ByVal opacityvalue As Single, width As Integer, height As Integer) As Image
        Dim w, h As Integer
        w = width
        h = height
        'Dim img2 As Bitmap
        Dim bmp As New Bitmap(img.Width, img.Height)


        'If Not IsNothing(img) Then
        Try

            Dim graphics__1 As Graphics = Graphics.FromImage(bmp)
            Using graphics__1
                Dim colormatrix As New ColorMatrix
                colormatrix.Matrix33 = opacityvalue
                Dim imgAttribute As New ImageAttributes
                imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.[Default], ColorAdjustType.Bitmap)
                graphics__1.DrawImage(img, New Rectangle(0, 0, w, h), 0, 0, w, h, GraphicsUnit.Pixel, imgAttribute)
            End Using

            'PictureBox1.BackgroundImage = bmp
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        'End If
        Return bmp
    End Function
    Public Sub ChangeOpacityxx()
        Dim w, h As Integer
        'w = PB_HeightGraphx.BackgroundImage.Width
        'h = PB_HeightGraphx.BackgroundImage.Height
        'Dim img2 As Bitmap
        Dim bmp As New Bitmap(Width, Height)
        Dim bmp2 As New Bitmap(w, h)

        'If Not IsNothing(img) Then
        Try




            Dim graphics__1 As Graphics = Graphics.FromImage(bmp2)
            Using graphics__1
                Dim colormatrix As New ColorMatrix
                colormatrix.Matrix33 = 0.5
                Dim imgAttribute As New ImageAttributes
                imgAttribute.SetColorMatrix(colormatrix, ColorMatrixFlag.[Default], ColorAdjustType.Bitmap)
                graphics__1.DrawImage(imgGraph, New Rectangle(0, 0, w, h), 0, 0, w, h, GraphicsUnit.Pixel, imgAttribute)
            End Using

            'PictureBox1.BackgroundImage = bmp
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        'PB_HeightGraphx.BackgroundImage = bmp2
        'End If

    End Sub
    Public Function MakePBTransparent(ByRef iPB As PictureBox, ByVal img As Image, opacity As Single) As Bitmap


        If Not iPB.BackgroundImage Is Nothing Then
            iPB.BackgroundImage.Dispose()
        End If
        'PictureBox1.Image = img
        'MsgBox(CInt(imgGraph.Width))
        iPB.BackgroundImage = ChangeOpacity(img, opacity, img.Width, img.Height)
        'PictureBox1.Image = img

    End Function
    Public Function SetBlackTransparent(ByRef iPB As PictureBox, img As Image) As Bitmap
        Dim bmp As Bitmap = New Bitmap(img.Width, img.Height)
        Dim img2 As Image

        bmp = img
        bmp.MakeTransparent(Color.FromName("Black"))


        'I 'f Not iPB.BackgroundImage Is Nothing Then
        'iPB.BackgroundImage.Dispose()
        'End If
        'iPB.BackgroundImage = bmp

    End Function
    Sub SetTransparencyValueAllPB()

        MakePBTransparent(PB_LegMap, imgLegMap, NumTransparency.Value)
        MakePBTransparent(PB_Route, imgRoute, NumTransparency.Value)
        MakePBTransparent(PB_HeightGraph, imgGraph, NumTransparency.Value)
        'ChangeOpacityxx()

        'MakePBTransparent(PB_SpeedPanel, imgSpeed, NumTransparency.Value)
    End Sub
    Private Sub NumTransparency_ValueChanged(sender As Object, e As EventArgs) Handles NumTransparency.ValueChanged
        If bFormReady Then SetTransparencyValueAllPB()

    End Sub

    Private Sub PB_MouseUp(sender As Object, e As MouseEventArgs) Handles PB_Route.MouseUp, PB_SpeedPanel.MouseUp, PB_LegMap.MouseUp, PB_HeightGraph.MouseUp
        Dim marginH, marginV As Integer
        If bPBMove = True Then
            If TypeOf sender Is PictureBox Then
                Dim pb As PictureBox = DirectCast(sender, PictureBox)
                'pb.[Text] = "Method 1" ' Text is actually inherited From Control, have use [] to access it as it is hidden in this case

                bPBMove = False

                marginV = PositionToMargin(sender, "Bottom")
                Select Case pb.Name
                    Case "PB_Route"
                        marginH = PositionToMargin(sender, "Right")
                        txtRouteH.Text = CStr(marginH)
                        txtRouteV.Text = CStr(marginV)
                    Case "PB_LegMap"
                        marginH = PositionToMargin(sender, "Left")
                        txtLegMapH.Text = CStr(marginH)
                        txtLegMapV.Text = CStr(marginV)
                    Case "PB_HeightGraph"
                        marginH = PositionToMargin(sender, "Left")
                        txtHeightH.Text = CStr(marginH)
                        txtHeightV.Text = CStr(marginV)
                    Case "PB_SpeedPanel"
                        marginH = PositionToMargin(sender, "Right")
                        txtSpeedH.Text = CStr(marginH)
                        txtSpeedV.Text = CStr(marginV)
                End Select

                If bFormReady Then UpdateAllImages()
            End If
        End If
    End Sub
    Private Sub PB_MouseDown(sender As Object, e As MouseEventArgs) Handles PB_Route.MouseDown, PB_SpeedPanel.MouseDown, PB_LegMap.MouseDown, PB_HeightGraph.MouseDown

        bPBMove = True
    End Sub
    Sub SetVideoBackG()
        If cbGoProF.Checked Then
            PB_Video.BackgroundImage = My.Resources.vid_img_1920x1080PAD
        Else
            PB_Video.BackgroundImage = My.Resources.vid_img1920x1080
        End If
    End Sub
    Private Sub cbGoProF_CheckedChanged(sender As Object, e As EventArgs) Handles cbGoProF.CheckedChanged
        SetVideoBackG()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        My.Settings.Save()
        Me.Close()
    End Sub

    Dim Scalevid, Transparency As Decimal
    Dim VideoScale As Decimal
    Dim MapInfo As clsExtra


    Sub SetAllPBTansparent()
        PB_LegMap.Parent = PB_Video
        PB_LegMap.BackColor = Color.Transparent
        PB_Route.Parent = PB_Video
        PB_Route.BackColor = Color.Transparent
        PB_SpeedPanel.Parent = PB_Video
        PB_SpeedPanel.BackColor = Color.Transparent
        PB_HeightGraph.Parent = PB_Video
        PB_HeightGraph.BackColor = Color.Transparent
        'PB_HeightGraphx.Parent = PB_Video
        'PB_HeightGraphx.BackColor = Color.Transparent

    End Sub
    Sub UpdateInputValues()
        LegMapLeft = CInt(txtLegMapH.Text)
        LegMapBottom = CInt(txtLegMapV.Text)
        RouteMapRight = CInt(txtRouteH.Text)
        RouteMapBottom = CInt(txtRouteV.Text)
        HeightGLeft = CInt(txtHeightH.Text)
        HeightGBottom = CInt(txtHeightV.Text)
        SpeedRight = CInt(txtSpeedH.Text)
        SpeedBottom = CInt(txtSpeedV.Text)
        Scalevid = numScale.Value

        Transparency = NumTransparency.Value
    End Sub
    Sub GetPictSizes()
        LegMapHeight = MapInfo.LEGMAP_HEIGHT
        LegMapWidth = MapInfo.LEGMAP_WIDTH
        If My.Settings.bUseMapTrackingVideo Then
            RouteMapHeight = My.Settings.TrackMapHeight
            RouteMapWidth = My.Settings.TrackMapWidth
        Else
            RouteMapHeight = MapInfo.MAP_HEIGHT
            RouteMapWidth = MapInfo.MAP_WIDTH
        End If
        SpeedWidth = MapInfo.PANEL_WIDTH
        SpeedHeight = MapInfo.PANEL_HEIGHT
        HeightGHEight = MapInfo.GRAPH_HEIGHT
        HeightGWidth = MapInfo.GRAPH_WIDTH
    End Sub


    Function PositionToMargin(ByRef PictBox As PictureBox, Relative As String) As Integer
        Dim Xpos, Ypos As Integer
        Dim Margin As Integer
        Xpos = PictBox.Location.X ' - PB_Video.Location.X
        Ypos = PictBox.Location.Y ' - PB_Video.Location.Y
        Select Case Relative
            Case "Left"
                Margin = Xpos
                If Margin > PB_Video.Width - PictBox.Width Then
                    Margin = PB_Video.Width - PictBox.Width
                End If

            Case "Right"
                Margin = PB_Video.Width - Xpos - PictBox.Width 'Xpos = PB_Video.Width - (PictBox.Width + dMargin)
                If Margin > PB_Video.Width - PictBox.Width Then
                    Margin = PB_Video.Width - PictBox.Width
                End If
            Case "Bottom"
                Margin = PB_Video.Height - Ypos - PictBox.Height 'Ypos = PB_Video.Height - (PictBox.Height + dMargin)
                If Margin > PB_Video.Height - PictBox.Height Then
                    Margin = PB_Video.Height - PictBox.Height
                End If
        End Select
        If Margin < 0 Then Margin = 0
        Return Margin / VideoScale ' Pixels i endelig video
    End Function

    Function MarginToPosition(Margin As Integer, ByRef PictBox As PictureBox, Relative As String) As Integer
        Dim Xpos, Ypos As Integer
        Dim dMargin As Decimal
        Xpos = PictBox.Location.X
        Ypos = PictBox.Location.Y
        dMargin = VideoScale * Margin
        Select Case Relative
            Case "Left"
                Xpos = dMargin
            Case "Right"
                Xpos = PB_Video.Width - PictBox.Width - dMargin
            Case "Bottom"
                Ypos = PB_Video.Height - PictBox.Height - dMargin
        End Select
        PictBox.Location = New Point(Xpos, Ypos)
    End Function
    Sub SetVideoScale()
        VideoScale = PB_Video.Size.Width / 1920
    End Sub
    Sub ScalePictBox(ByRef PictBox As PictureBox, Width As Integer, Height As Integer, Optional iScale As Decimal = 1)
        PictBox.Width = Width * iScale * VideoScale
        PictBox.Height = Height * iScale * VideoScale
    End Sub
    Sub UpdateAllImages()
        UpdateInputValues()
        If Not bUseVideoMap Then 'only route map if videoinput
            ScalePictBox(PB_SpeedPanel, SpeedWidth, SpeedHeight)
            MarginToPosition(SpeedRight, PB_SpeedPanel, "Right")
            MarginToPosition(SpeedBottom, PB_SpeedPanel, "Bottom")
            ScalePictBox(PB_HeightGraph, HeightGWidth, HeightGHEight)
            MarginToPosition(HeightGLeft, PB_HeightGraph, "Left")
            MarginToPosition(HeightGBottom, PB_HeightGraph, "Bottom")
            ScalePictBox(PB_LegMap, LegMapWidth, LegMapHeight, Scalevid)
            MarginToPosition(LegMapLeft, PB_LegMap, "Left")
            MarginToPosition(LegMapBottom, PB_LegMap, "Bottom")
        Else
            PB_HeightGraph.Visible = False
            PB_LegMap.Visible = False
            PB_SpeedPanel.Visible = False
        End If
        ScalePictBox(PB_Route, RouteMapWidth, RouteMapHeight, Scalevid)
        MarginToPosition(RouteMapRight, PB_Route, "Right")
        MarginToPosition(RouteMapBottom, PB_Route, "Bottom")
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick 'Delayed update Form_Shown
        If Not (My.Settings.MHeightV <> txtHeightV.Text Or My.Settings.MHeightH <> txtHeightH.Text Or My.Settings.Transparency <> NumTransparency.Value) Then
            SetVideoBackG()
            SetVideoScale()
            GetPictSizes()

            UpdateAllImages()
            SetTransparencyValueAllPB()
            'SetBlackTransparent(PB_HeightGraph, imgGraph)
            Timer1.Stop()
            bFormReady = True
        End If
    End Sub

    Private Sub frmMapOverlayForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        Timer1.Start() 'Wait for Controls to be loaded

    End Sub


End Class