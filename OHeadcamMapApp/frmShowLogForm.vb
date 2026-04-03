Public Class frmShowLogForm
    Public Sub New(ByVal iLog1 As String, ByVal ilog2 As String, ByVal ilog3 As String, ByVal ilog4 As String, ByVal iStatuslog As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        txtLog1.Text = iLog1
        txtLog2.Text = ilog2
        txtLog3.Text = ilog3
        txtLog4.Text = ilog4
        txtStatusLog.Text = iStatuslog

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub


End Class
