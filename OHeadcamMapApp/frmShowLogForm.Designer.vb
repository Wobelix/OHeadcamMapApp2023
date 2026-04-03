<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmShowLogForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmShowLogForm))
        Me.txtLog1 = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.txtLog2 = New System.Windows.Forms.TextBox()
        Me.txtLog3 = New System.Windows.Forms.TextBox()
        Me.txtLog4 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtStatusLog = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txtLog1
        '
        Me.txtLog1.AcceptsReturn = True
        resources.ApplyResources(Me.txtLog1, "txtLog1")
        Me.txtLog1.Name = "txtLog1"
        '
        'Button1
        '
        resources.ApplyResources(Me.Button1, "Button1")
        Me.Button1.Name = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'txtLog2
        '
        Me.txtLog2.AcceptsReturn = True
        resources.ApplyResources(Me.txtLog2, "txtLog2")
        Me.txtLog2.Name = "txtLog2"
        '
        'txtLog3
        '
        Me.txtLog3.AcceptsReturn = True
        resources.ApplyResources(Me.txtLog3, "txtLog3")
        Me.txtLog3.Name = "txtLog3"
        '
        'txtLog4
        '
        Me.txtLog4.AcceptsReturn = True
        resources.ApplyResources(Me.txtLog4, "txtLog4")
        Me.txtLog4.Name = "txtLog4"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'txtStatusLog
        '
        Me.txtStatusLog.AcceptsReturn = True
        resources.ApplyResources(Me.txtStatusLog, "txtStatusLog")
        Me.txtStatusLog.Name = "txtStatusLog"
        '
        'ShowLogForm
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtStatusLog)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtLog4)
        Me.Controls.Add(Me.txtLog3)
        Me.Controls.Add(Me.txtLog2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.txtLog1)
        Me.Name = "ShowLogForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtLog1 As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents txtLog2 As TextBox
    Friend WithEvents txtLog3 As TextBox
    Friend WithEvents txtLog4 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents txtStatusLog As TextBox
End Class
