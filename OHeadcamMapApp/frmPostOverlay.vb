
Imports OHeadcamMapApp.Namespace_XFFMPG

Public Class frmPostOverlay
    Private ChgMode As Boolean = False
    Private _EditOverl As clsPostOverlay
    Private _PostVideoInf As clsFFMPegProbe
    Private FFMp As clsExtra = New clsExtra
    Public Sub New(EditOverl As clsPostOverlay, PostVideoInf As clsFFMPegProbe)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _PostVideoInf = PostVideoInf
        _EditOverl = EditOverl
        ChgMode = True

    End Sub

    Public Sub New(PostVideoInf As clsFFMPegProbe)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _PostVideoInf = PostVideoInf

    End Sub

    Public Property ReturnedValues As clsPostOverlay = New clsPostOverlay

    Private Sub frmPostOverlay_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Populate ComboBox with enum values
        cmbPlacement.DataSource = [Enum].GetValues(GetType(OverlayPosition))
        Me.Text = _PostVideoInf.Videofilename

        If ChgMode Then
            txtOStart.Text = FFMp.SecToTimeStr(CSng(_EditOverl.sStart))
            If _EditOverl.sDuration = "-1" Or _EditOverl.sDuration = "" Then
                txtDur.Text = ""
            Else
                txtDur.Text = FFMp.SecToTimeStr(CSng(_EditOverl.sDuration))
            End If

            cmbPlacement.SelectedItem = _EditOverl.PlacementT
            txtOX.Text = _EditOverl.sX
            txtOY.Text = _EditOverl.sY
            txtSHeight.Text = _EditOverl.sSHeight
            txtSWidth.Text = _EditOverl.sSWidth
        Else
            cmbPlacement.SelectedIndex = 0
            If _PostVideoInf.IsImage Then
                ReturnedValues.bIsImage = True '1 sec video is generated
            Else
                txtDur.Text = _PostVideoInf.GetTimeStr()
                ReturnedValues.bIsImage = False
            End If

        End If
    End Sub

    Private Sub GetFromFields(InOInfo As clsPostOverlay)
        InOInfo.sStart = FFMp.TimeStrToSec(txtOStart.Text)
        If txtDur.Text = "" Then
            InOInfo.sDuration = "-1"
        Else
            InOInfo.sDuration = FFMp.TimeStrToSec(txtDur.Text)
        End If
        InOInfo.sDuration = FFMp.TimeStrToSec(txtDur.Text)
        InOInfo.PlacementT = cmbPlacement.SelectedItem
        InOInfo.sX = txtOX.Text
        InOInfo.sY = txtOY.Text
        InOInfo.sSHeight = txtSHeight.Text
        InOInfo.sSWidth = txtSWidth.Text
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'ReturnedValues.Duration = txtDur.Text
        'ReturnedValues.TDuration = txtFDur.Text
        'ReturnedValues.TOffset = txtFOff.Text
        'ReturnedValues.TTransition = cmbFT.SelectedItem.ToString
        If ChgMode Then
            GetFromFields(_EditOverl)
        Else
            GetFromFields(ReturnedValues)
        End If
        DialogResult = DialogResult.OK
        Me.Close()
    End Sub


End Class

Public Class clsPostOverlay
    Public Property sStart As String
    Public Property sDuration As String
    Public Property PlacementT As OverlayPosition
    Public Property sX As String
    Public Property sY As String
    Public Property sSWidth As String
    Public Property sSHeight As String
    Public Property bIsImage As Boolean = False

    ' Add more properties as needed
End Class