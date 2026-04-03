<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmPostFileProps
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPostFileProps))
        Me.cmbFT = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtFDur = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtDur = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmbFT
        '
        Me.cmbFT.FormattingEnabled = True
        Me.cmbFT.Items.AddRange(New Object() {resources.GetString("cmbFT.Items"), resources.GetString("cmbFT.Items1"), resources.GetString("cmbFT.Items2"), resources.GetString("cmbFT.Items3"), resources.GetString("cmbFT.Items4"), resources.GetString("cmbFT.Items5"), resources.GetString("cmbFT.Items6"), resources.GetString("cmbFT.Items7"), resources.GetString("cmbFT.Items8"), resources.GetString("cmbFT.Items9"), resources.GetString("cmbFT.Items10"), resources.GetString("cmbFT.Items11"), resources.GetString("cmbFT.Items12"), resources.GetString("cmbFT.Items13"), resources.GetString("cmbFT.Items14"), resources.GetString("cmbFT.Items15"), resources.GetString("cmbFT.Items16"), resources.GetString("cmbFT.Items17"), resources.GetString("cmbFT.Items18"), resources.GetString("cmbFT.Items19"), resources.GetString("cmbFT.Items20"), resources.GetString("cmbFT.Items21"), resources.GetString("cmbFT.Items22"), resources.GetString("cmbFT.Items23"), resources.GetString("cmbFT.Items24"), resources.GetString("cmbFT.Items25"), resources.GetString("cmbFT.Items26"), resources.GetString("cmbFT.Items27"), resources.GetString("cmbFT.Items28"), resources.GetString("cmbFT.Items29"), resources.GetString("cmbFT.Items30"), resources.GetString("cmbFT.Items31"), resources.GetString("cmbFT.Items32"), resources.GetString("cmbFT.Items33"), resources.GetString("cmbFT.Items34"), resources.GetString("cmbFT.Items35"), resources.GetString("cmbFT.Items36"), resources.GetString("cmbFT.Items37"), resources.GetString("cmbFT.Items38"), resources.GetString("cmbFT.Items39"), resources.GetString("cmbFT.Items40"), resources.GetString("cmbFT.Items41"), resources.GetString("cmbFT.Items42"), resources.GetString("cmbFT.Items43"), resources.GetString("cmbFT.Items44"), resources.GetString("cmbFT.Items45"), resources.GetString("cmbFT.Items46"), resources.GetString("cmbFT.Items47"), resources.GetString("cmbFT.Items48"), resources.GetString("cmbFT.Items49"), resources.GetString("cmbFT.Items50"), resources.GetString("cmbFT.Items51"), resources.GetString("cmbFT.Items52"), resources.GetString("cmbFT.Items53"), resources.GetString("cmbFT.Items54"), resources.GetString("cmbFT.Items55"), resources.GetString("cmbFT.Items56"), resources.GetString("cmbFT.Items57")})
        resources.ApplyResources(Me.cmbFT, "cmbFT")
        Me.cmbFT.Name = "cmbFT"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'txtFDur
        '
        resources.ApplyResources(Me.txtFDur, "txtFDur")
        Me.txtFDur.Name = "txtFDur"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.cmbFT)
        Me.GroupBox1.Controls.Add(Me.txtFDur)
        Me.GroupBox1.Controls.Add(Me.Label2)
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.TabStop = False
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'txtDur
        '
        resources.ApplyResources(Me.txtDur, "txtDur")
        Me.txtDur.Name = "txtDur"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'Button1
        '
        resources.ApplyResources(Me.Button1, "Button1")
        Me.Button1.Name = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'frmPostFileProps
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.txtDur)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "frmPostFileProps"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmbFT As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtFDur As TextBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtDur As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Button1 As Button
End Class
