<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmPostOverlay
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPostOverlay))
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtOX = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtOY = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbPlacement = New System.Windows.Forms.ComboBox()
        Me.txtDur = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.txtOStart = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtSWidth = New System.Windows.Forms.TextBox()
        Me.txtSHeight = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'txtOX
        '
        resources.ApplyResources(Me.txtOX, "txtOX")
        Me.txtOX.Name = "txtOX"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'txtOY
        '
        resources.ApplyResources(Me.txtOY, "txtOY")
        Me.txtOY.Name = "txtOY"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.cmbPlacement)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.txtOX)
        Me.GroupBox1.Controls.Add(Me.txtOY)
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
        'cmbPlacement
        '
        Me.cmbPlacement.FormattingEnabled = True
        resources.ApplyResources(Me.cmbPlacement, "cmbPlacement")
        Me.cmbPlacement.Name = "cmbPlacement"
        '
        'txtDur
        '
        resources.ApplyResources(Me.txtDur, "txtDur")
        Me.txtDur.Name = "txtDur"
        '
        'Button1
        '
        resources.ApplyResources(Me.Button1, "Button1")
        Me.Button1.Name = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'txtOStart
        '
        resources.ApplyResources(Me.txtOStart, "txtOStart")
        Me.txtOStart.Name = "txtOStart"
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.txtSWidth)
        Me.GroupBox2.Controls.Add(Me.txtSHeight)
        Me.GroupBox2.Controls.Add(Me.Label8)
        resources.ApplyResources(Me.GroupBox2, "GroupBox2")
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.TabStop = False
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        '
        'txtSWidth
        '
        resources.ApplyResources(Me.txtSWidth, "txtSWidth")
        Me.txtSWidth.Name = "txtSWidth"
        '
        'txtSHeight
        '
        resources.ApplyResources(Me.txtSHeight, "txtSHeight")
        Me.txtSHeight.Name = "txtSHeight"
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Name = "Label8"
        '
        'frmPostOverlay
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtOStart)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.txtDur)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "frmPostOverlay"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As Label
    Friend WithEvents txtOX As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtOY As TextBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents txtDur As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents txtOStart As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents cmbPlacement As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label7 As Label
    Friend WithEvents txtSWidth As TextBox
    Friend WithEvents txtSHeight As TextBox
    Friend WithEvents Label8 As Label
End Class
