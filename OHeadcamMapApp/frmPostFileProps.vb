
Public Class frmPostFileProps
    Private ChgMode As Boolean = False
    Private _EditProps As clsPostFProps
    Private _PostVideoInf As clsFFMPegProbe
    Private FFMp As clsExtra = New clsExtra
    Public Sub New(EditProps As clsPostFProps, PostVideoInf As clsFFMPegProbe)

        ' This call is required by the designer.
        InitializeComponent()
        _PostVideoInf = PostVideoInf
        _EditProps = EditProps
        ChgMode = True
    End Sub

    Public Sub New(PostVideoInf As clsFFMPegProbe)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _PostVideoInf = PostVideoInf

    End Sub

    Public Property ReturnedValues As clsPostFProps = New clsPostFProps

    Private Sub frmPostFileProps_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = _PostVideoInf.Videofilename

        If ChgMode Then
            txtDur.Text = FFMp.SecToTimeStr(_EditProps.iDuration)
            txtFDur.Text = _EditProps.sTDuration
            cmbFT.SelectedItem = _EditProps.TTransition
            If _PostVideoInf.IsImage Then
                txtDur.Enabled = True
            Else
                txtDur.Enabled = False
            End If
        Else
            If _PostVideoInf.IsImage Then
                txtDur.Text = "10"
                ReturnedValues.bIsImage = True
                txtDur.Enabled = True
            Else
                txtDur.Text = _PostVideoInf.GetTimeStr() 'Math.Truncate(_PostVideoInf.duration_sec).ToString
                txtDur.Enabled = False
            End If
            ReturnedValues.bHasAudio = _PostVideoInf.HasAudio
            cmbFT.SelectedIndex = 0
        End If
    End Sub
    Private Sub GetProps(inProp As clsPostFProps)
        inProp.iDuration = FFMp.TimeStrToSec(txtDur.Text)
        inProp.sDuration = FFMp.SecToTimeStr(inProp.iDuration)
        If IsNumeric(inProp.sTDuration) Then
            inProp.sTDuration = txtFDur.Text
            inProp.iTDuration = CInt(txtFDur.Text)
        End If

        inProp.TTransition = cmbFT.SelectedItem.ToString
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If ChgMode Then
            GetProps(_EditProps)
        Else
            GetProps(ReturnedValues)
        End If
        DialogResult = DialogResult.OK
        Me.Close()
    End Sub
End Class

Public Class clsPostFProps
    Public Property sDuration As String = ""
    Public Property iDuration As Integer = 0
    Public Property TTransition As String = "fade"
    Public Property sTDuration As String = "5"
    Public Property iTDuration As Integer = 5
    Public Property bIsImage As Boolean = False
    Public Property bHasAudio As Boolean = True
    ' Add more properties as needed
End Class