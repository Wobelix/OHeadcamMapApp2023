<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ffplay
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
        Me.components = New System.ComponentModel.Container()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.btnVideoSelect = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.txtVideofile = New System.Windows.Forms.TextBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(295, 39)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Play"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'btnVideoSelect
        '
        Me.btnVideoSelect.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.btnVideoSelect.Location = New System.Drawing.Point(204, 39)
        Me.btnVideoSelect.Name = "btnVideoSelect"
        Me.btnVideoSelect.Size = New System.Drawing.Size(75, 23)
        Me.btnVideoSelect.TabIndex = 13
        Me.btnVideoSelect.Text = "Vælg fil"
        Me.btnVideoSelect.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'txtVideofile
        '
        Me.txtVideofile.Location = New System.Drawing.Point(13, 39)
        Me.txtVideofile.Name = "txtVideofile"
        Me.txtVideofile.Size = New System.Drawing.Size(185, 20)
        Me.txtVideofile.TabIndex = 14
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(12, 86)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(910, 512)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 16
        Me.PictureBox1.TabStop = False
        '
        'Timer1
        '
        Me.Timer1.Interval = 30
        '
        'ffplay
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(998, 661)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.txtVideofile)
        Me.Controls.Add(Me.btnVideoSelect)
        Me.Controls.Add(Me.Button1)
        Me.Name = "ffplay"
        Me.Text = "Player"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents btnVideoSelect As Button
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents txtVideofile As TextBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Timer1 As Timer
End Class
