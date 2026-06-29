<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmWidgets
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWidgets))
        Me.cbTime = New System.Windows.Forms.CheckBox()
        Me.cbDistance = New System.Windows.Forms.CheckBox()
        Me.cbPace = New System.Windows.Forms.CheckBox()
        Me.cbPulse = New System.Windows.Forms.CheckBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.cbHGraph = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'cbTime
        '
        resources.ApplyResources(Me.cbTime, "cbTime")
        Me.cbTime.Name = "cbTime"
        Me.cbTime.UseVisualStyleBackColor = True
        '
        'cbDistance
        '
        resources.ApplyResources(Me.cbDistance, "cbDistance")
        Me.cbDistance.Name = "cbDistance"
        Me.cbDistance.UseVisualStyleBackColor = True
        '
        'cbPace
        '
        resources.ApplyResources(Me.cbPace, "cbPace")
        Me.cbPace.Name = "cbPace"
        Me.cbPace.UseVisualStyleBackColor = True
        '
        'cbPulse
        '
        resources.ApplyResources(Me.cbPulse, "cbPulse")
        Me.cbPulse.Name = "cbPulse"
        Me.cbPulse.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        resources.ApplyResources(Me.btnSave, "btnSave")
        Me.btnSave.Name = "btnSave"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'cbHGraph
        '
        resources.ApplyResources(Me.cbHGraph, "cbHGraph")
        Me.cbHGraph.Name = "cbHGraph"
        Me.cbHGraph.UseVisualStyleBackColor = True
        '
        'frmWidgets
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.cbHGraph)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.cbPulse)
        Me.Controls.Add(Me.cbPace)
        Me.Controls.Add(Me.cbDistance)
        Me.Controls.Add(Me.cbTime)
        Me.Name = "frmWidgets"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cbTime As CheckBox
    Friend WithEvents cbDistance As CheckBox
    Friend WithEvents cbPace As CheckBox
    Friend WithEvents cbPulse As CheckBox
    Friend WithEvents btnSave As Button
    Friend WithEvents cbHGraph As CheckBox
End Class
