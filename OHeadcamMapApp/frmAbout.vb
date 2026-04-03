Public Class AboutApp
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        lbVersion.Text = ProductVersion
        'Dim cd = System.Deployment.Application.ApplicationDeployment.CurrentDeployment
        'Dim a As Version
        'a = ApplicationDeployment.CurrentDeployment.CurrentVersion
        'lbVersion.Text = a.ToString
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub LinkLabel1_Click(sender As Object, e As EventArgs) Handles LinkLabel1.Click
        System.Diagnostics.Process.Start("http://wobelix.dk/blog/oheadcammapapp/")
    End Sub

    Private Sub AboutApp_Load(sender As Object, e As EventArgs) Handles Me.Load
        PictureBox1.Image = My.Resources.Headcam_Orienteering_Icon.ToBitmap
    End Sub
End Class
