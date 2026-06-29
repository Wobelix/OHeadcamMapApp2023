Public Class frmWidgets
    Private _loading As Boolean = True

    Private Sub frmWidgets_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _loading = True
        cbTime.Checked = My.Settings.MIWidgetTimeEnabled
        cbDistance.Checked = My.Settings.MIWidgetDistanceEnabled
        cbPace.Checked = My.Settings.MIWidgetPaceEnabled
        cbPulse.Checked = My.Settings.MIWidgetPulseEnabled
        cbHGraph.Checked = My.Settings.MIWidgetHGraphEnabled
        _loading = False
    End Sub

    Private Sub Widget_CheckedChanged(sender As Object, e As EventArgs) Handles cbTime.CheckedChanged, cbDistance.CheckedChanged, cbPace.CheckedChanged, cbPulse.CheckedChanged, cbHGraph.CheckedChanged
        If _loading Then Return
        SaveWidgetSettings()
    End Sub



    Private Sub SaveWidgetSettings()
        My.Settings.MIWidgetTimeEnabled = cbTime.Checked
        My.Settings.MIWidgetDistanceEnabled = cbDistance.Checked
        My.Settings.MIWidgetPaceEnabled = cbPace.Checked
        My.Settings.MIWidgetPulseEnabled = cbPulse.Checked
        My.Settings.MIWidgetHGraphEnabled = cbHGraph.Checked
        My.Settings.Save()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        SaveWidgetSettings()
        DialogResult = DialogResult.OK
        Close()
    End Sub
End Class
