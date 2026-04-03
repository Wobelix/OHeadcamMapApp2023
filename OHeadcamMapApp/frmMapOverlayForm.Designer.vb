<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMapOverlayForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMapOverlayForm))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.cbGoProF = New System.Windows.Forms.CheckBox()
        Me.PB_SpeedPanel = New System.Windows.Forms.PictureBox()
        Me.PB_Route = New System.Windows.Forms.PictureBox()
        Me.PB_LegMap = New System.Windows.Forms.PictureBox()
        Me.PB_Video = New System.Windows.Forms.PictureBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.PB_HeightGraph = New System.Windows.Forms.PictureBox()
        Me.HelpProvider1 = New System.Windows.Forms.HelpProvider()
        Me.cbShowRoute = New System.Windows.Forms.CheckBox()
        Me.txtRouteH = New System.Windows.Forms.TextBox()
        Me.txtRouteV = New System.Windows.Forms.TextBox()
        Me.cbShowSpeed = New System.Windows.Forms.CheckBox()
        Me.txtSpeedH = New System.Windows.Forms.TextBox()
        Me.txtSpeedV = New System.Windows.Forms.TextBox()
        Me.cbShowLeg = New System.Windows.Forms.CheckBox()
        Me.txtLegMapH = New System.Windows.Forms.TextBox()
        Me.txtLegMapV = New System.Windows.Forms.TextBox()
        Me.numScale = New System.Windows.Forms.NumericUpDown()
        Me.NumTransparency = New System.Windows.Forms.NumericUpDown()
        Me.cbShowHeight = New System.Windows.Forms.CheckBox()
        Me.txtHeightH = New System.Windows.Forms.TextBox()
        Me.txtHeightV = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        CType(Me.PB_SpeedPanel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PB_Route, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PB_LegMap, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PB_Video, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PB_HeightGraph, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.numScale, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.NumTransparency, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        resources.ApplyResources(Me.GroupBox1, "GroupBox1")
        Me.GroupBox1.Controls.Add(Me.cbShowHeight)
        Me.GroupBox1.Controls.Add(Me.GroupBox3)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.txtHeightH)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.txtHeightV)
        Me.HelpProvider1.SetHelpKeyword(Me.GroupBox1, resources.GetString("GroupBox1.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.GroupBox1, CType(resources.GetObject("GroupBox1.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.GroupBox1, resources.GetString("GroupBox1.HelpString"))
        Me.GroupBox1.Name = "GroupBox1"
        Me.HelpProvider1.SetShowHelp(Me.GroupBox1, CType(resources.GetObject("GroupBox1.ShowHelp"), Boolean))
        Me.GroupBox1.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox1, resources.GetString("GroupBox1.ToolTip"))
        '
        'GroupBox3
        '
        resources.ApplyResources(Me.GroupBox3, "GroupBox3")
        Me.HelpProvider1.SetHelpKeyword(Me.GroupBox3, resources.GetString("GroupBox3.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.GroupBox3, CType(resources.GetObject("GroupBox3.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.GroupBox3, resources.GetString("GroupBox3.HelpString"))
        Me.GroupBox3.Name = "GroupBox3"
        Me.HelpProvider1.SetShowHelp(Me.GroupBox3, CType(resources.GetObject("GroupBox3.ShowHelp"), Boolean))
        Me.GroupBox3.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox3, resources.GetString("GroupBox3.ToolTip"))
        '
        'Label11
        '
        resources.ApplyResources(Me.Label11, "Label11")
        Me.HelpProvider1.SetHelpKeyword(Me.Label11, resources.GetString("Label11.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.Label11, CType(resources.GetObject("Label11.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.Label11, resources.GetString("Label11.HelpString"))
        Me.Label11.Name = "Label11"
        Me.HelpProvider1.SetShowHelp(Me.Label11, CType(resources.GetObject("Label11.ShowHelp"), Boolean))
        Me.ToolTip1.SetToolTip(Me.Label11, resources.GetString("Label11.ToolTip"))
        '
        'Label10
        '
        resources.ApplyResources(Me.Label10, "Label10")
        Me.HelpProvider1.SetHelpKeyword(Me.Label10, resources.GetString("Label10.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.Label10, CType(resources.GetObject("Label10.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.Label10, resources.GetString("Label10.HelpString"))
        Me.Label10.Name = "Label10"
        Me.HelpProvider1.SetShowHelp(Me.Label10, CType(resources.GetObject("Label10.ShowHelp"), Boolean))
        Me.ToolTip1.SetToolTip(Me.Label10, resources.GetString("Label10.ToolTip"))
        '
        'Label13
        '
        resources.ApplyResources(Me.Label13, "Label13")
        Me.HelpProvider1.SetHelpKeyword(Me.Label13, resources.GetString("Label13.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.Label13, CType(resources.GetObject("Label13.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.Label13, resources.GetString("Label13.HelpString"))
        Me.Label13.Name = "Label13"
        Me.HelpProvider1.SetShowHelp(Me.Label13, CType(resources.GetObject("Label13.ShowHelp"), Boolean))
        Me.ToolTip1.SetToolTip(Me.Label13, resources.GetString("Label13.ToolTip"))
        '
        'Label14
        '
        resources.ApplyResources(Me.Label14, "Label14")
        Me.HelpProvider1.SetHelpKeyword(Me.Label14, resources.GetString("Label14.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.Label14, CType(resources.GetObject("Label14.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.Label14, resources.GetString("Label14.HelpString"))
        Me.Label14.Name = "Label14"
        Me.HelpProvider1.SetShowHelp(Me.Label14, CType(resources.GetObject("Label14.ShowHelp"), Boolean))
        Me.ToolTip1.SetToolTip(Me.Label14, resources.GetString("Label14.ToolTip"))
        '
        'Label15
        '
        resources.ApplyResources(Me.Label15, "Label15")
        Me.HelpProvider1.SetHelpKeyword(Me.Label15, resources.GetString("Label15.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.Label15, CType(resources.GetObject("Label15.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.Label15, resources.GetString("Label15.HelpString"))
        Me.Label15.Name = "Label15"
        Me.HelpProvider1.SetShowHelp(Me.Label15, CType(resources.GetObject("Label15.ShowHelp"), Boolean))
        Me.ToolTip1.SetToolTip(Me.Label15, resources.GetString("Label15.ToolTip"))
        '
        'Label16
        '
        resources.ApplyResources(Me.Label16, "Label16")
        Me.HelpProvider1.SetHelpKeyword(Me.Label16, resources.GetString("Label16.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.Label16, CType(resources.GetObject("Label16.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.Label16, resources.GetString("Label16.HelpString"))
        Me.Label16.Name = "Label16"
        Me.HelpProvider1.SetShowHelp(Me.Label16, CType(resources.GetObject("Label16.ShowHelp"), Boolean))
        Me.ToolTip1.SetToolTip(Me.Label16, resources.GetString("Label16.ToolTip"))
        '
        'Label9
        '
        resources.ApplyResources(Me.Label9, "Label9")
        Me.HelpProvider1.SetHelpKeyword(Me.Label9, resources.GetString("Label9.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.Label9, CType(resources.GetObject("Label9.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.Label9, resources.GetString("Label9.HelpString"))
        Me.Label9.Name = "Label9"
        Me.HelpProvider1.SetShowHelp(Me.Label9, CType(resources.GetObject("Label9.ShowHelp"), Boolean))
        Me.ToolTip1.SetToolTip(Me.Label9, resources.GetString("Label9.ToolTip"))
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.HelpProvider1.SetHelpKeyword(Me.Label8, resources.GetString("Label8.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.Label8, CType(resources.GetObject("Label8.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.Label8, resources.GetString("Label8.HelpString"))
        Me.Label8.Name = "Label8"
        Me.HelpProvider1.SetShowHelp(Me.Label8, CType(resources.GetObject("Label8.ShowHelp"), Boolean))
        Me.ToolTip1.SetToolTip(Me.Label8, resources.GetString("Label8.ToolTip"))
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.HelpProvider1.SetHelpKeyword(Me.Label4, resources.GetString("Label4.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.Label4, CType(resources.GetObject("Label4.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.Label4, resources.GetString("Label4.HelpString"))
        Me.Label4.Name = "Label4"
        Me.HelpProvider1.SetShowHelp(Me.Label4, CType(resources.GetObject("Label4.ShowHelp"), Boolean))
        Me.ToolTip1.SetToolTip(Me.Label4, resources.GetString("Label4.ToolTip"))
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.HelpProvider1.SetHelpKeyword(Me.Label7, resources.GetString("Label7.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.Label7, CType(resources.GetObject("Label7.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.Label7, resources.GetString("Label7.HelpString"))
        Me.Label7.Name = "Label7"
        Me.HelpProvider1.SetShowHelp(Me.Label7, CType(resources.GetObject("Label7.ShowHelp"), Boolean))
        Me.ToolTip1.SetToolTip(Me.Label7, resources.GetString("Label7.ToolTip"))
        '
        'Button1
        '
        resources.ApplyResources(Me.Button1, "Button1")
        Me.HelpProvider1.SetHelpKeyword(Me.Button1, resources.GetString("Button1.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.Button1, CType(resources.GetObject("Button1.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.Button1, resources.GetString("Button1.HelpString"))
        Me.Button1.Name = "Button1"
        Me.HelpProvider1.SetShowHelp(Me.Button1, CType(resources.GetObject("Button1.ShowHelp"), Boolean))
        Me.ToolTip1.SetToolTip(Me.Button1, resources.GetString("Button1.ToolTip"))
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        '
        'GroupBox2
        '
        resources.ApplyResources(Me.GroupBox2, "GroupBox2")
        Me.GroupBox2.Controls.Add(Me.cbShowLeg)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.txtLegMapH)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.txtLegMapV)
        Me.HelpProvider1.SetHelpKeyword(Me.GroupBox2, resources.GetString("GroupBox2.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.GroupBox2, CType(resources.GetObject("GroupBox2.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.GroupBox2, resources.GetString("GroupBox2.HelpString"))
        Me.GroupBox2.Name = "GroupBox2"
        Me.HelpProvider1.SetShowHelp(Me.GroupBox2, CType(resources.GetObject("GroupBox2.ShowHelp"), Boolean))
        Me.GroupBox2.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox2, resources.GetString("GroupBox2.ToolTip"))
        '
        'GroupBox4
        '
        resources.ApplyResources(Me.GroupBox4, "GroupBox4")
        Me.GroupBox4.Controls.Add(Me.cbShowSpeed)
        Me.GroupBox4.Controls.Add(Me.Label14)
        Me.GroupBox4.Controls.Add(Me.txtSpeedH)
        Me.GroupBox4.Controls.Add(Me.Label13)
        Me.GroupBox4.Controls.Add(Me.txtSpeedV)
        Me.HelpProvider1.SetHelpKeyword(Me.GroupBox4, resources.GetString("GroupBox4.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.GroupBox4, CType(resources.GetObject("GroupBox4.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.GroupBox4, resources.GetString("GroupBox4.HelpString"))
        Me.GroupBox4.Name = "GroupBox4"
        Me.HelpProvider1.SetShowHelp(Me.GroupBox4, CType(resources.GetObject("GroupBox4.ShowHelp"), Boolean))
        Me.GroupBox4.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox4, resources.GetString("GroupBox4.ToolTip"))
        '
        'GroupBox5
        '
        resources.ApplyResources(Me.GroupBox5, "GroupBox5")
        Me.GroupBox5.Controls.Add(Me.cbShowRoute)
        Me.GroupBox5.Controls.Add(Me.Label16)
        Me.GroupBox5.Controls.Add(Me.txtRouteH)
        Me.GroupBox5.Controls.Add(Me.Label15)
        Me.GroupBox5.Controls.Add(Me.txtRouteV)
        Me.HelpProvider1.SetHelpKeyword(Me.GroupBox5, resources.GetString("GroupBox5.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.GroupBox5, CType(resources.GetObject("GroupBox5.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.GroupBox5, resources.GetString("GroupBox5.HelpString"))
        Me.GroupBox5.Name = "GroupBox5"
        Me.HelpProvider1.SetShowHelp(Me.GroupBox5, CType(resources.GetObject("GroupBox5.ShowHelp"), Boolean))
        Me.GroupBox5.TabStop = False
        Me.ToolTip1.SetToolTip(Me.GroupBox5, resources.GetString("GroupBox5.ToolTip"))
        '
        'cbGoProF
        '
        resources.ApplyResources(Me.cbGoProF, "cbGoProF")
        Me.HelpProvider1.SetHelpKeyword(Me.cbGoProF, resources.GetString("cbGoProF.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.cbGoProF, CType(resources.GetObject("cbGoProF.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.cbGoProF, resources.GetString("cbGoProF.HelpString"))
        Me.cbGoProF.Name = "cbGoProF"
        Me.HelpProvider1.SetShowHelp(Me.cbGoProF, CType(resources.GetObject("cbGoProF.ShowHelp"), Boolean))
        Me.ToolTip1.SetToolTip(Me.cbGoProF, resources.GetString("cbGoProF.ToolTip"))
        Me.cbGoProF.UseVisualStyleBackColor = True
        '
        'PB_SpeedPanel
        '
        resources.ApplyResources(Me.PB_SpeedPanel, "PB_SpeedPanel")
        Me.PB_SpeedPanel.BackgroundImage = Global.OHeadcamMapApp.My.Resources.Resources.panelimg
        Me.HelpProvider1.SetHelpKeyword(Me.PB_SpeedPanel, resources.GetString("PB_SpeedPanel.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.PB_SpeedPanel, CType(resources.GetObject("PB_SpeedPanel.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.PB_SpeedPanel, resources.GetString("PB_SpeedPanel.HelpString"))
        Me.PB_SpeedPanel.Name = "PB_SpeedPanel"
        Me.HelpProvider1.SetShowHelp(Me.PB_SpeedPanel, CType(resources.GetObject("PB_SpeedPanel.ShowHelp"), Boolean))
        Me.PB_SpeedPanel.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PB_SpeedPanel, resources.GetString("PB_SpeedPanel.ToolTip"))
        '
        'PB_Route
        '
        resources.ApplyResources(Me.PB_Route, "PB_Route")
        Me.PB_Route.BackgroundImage = Global.OHeadcamMapApp.My.Resources.Resources.routeimg
        Me.HelpProvider1.SetHelpKeyword(Me.PB_Route, resources.GetString("PB_Route.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.PB_Route, CType(resources.GetObject("PB_Route.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.PB_Route, resources.GetString("PB_Route.HelpString"))
        Me.PB_Route.Name = "PB_Route"
        Me.HelpProvider1.SetShowHelp(Me.PB_Route, CType(resources.GetObject("PB_Route.ShowHelp"), Boolean))
        Me.PB_Route.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PB_Route, resources.GetString("PB_Route.ToolTip"))
        '
        'PB_LegMap
        '
        resources.ApplyResources(Me.PB_LegMap, "PB_LegMap")
        Me.PB_LegMap.BackgroundImage = Global.OHeadcamMapApp.My.Resources.Resources.legmapimg
        Me.HelpProvider1.SetHelpKeyword(Me.PB_LegMap, resources.GetString("PB_LegMap.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.PB_LegMap, CType(resources.GetObject("PB_LegMap.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.PB_LegMap, resources.GetString("PB_LegMap.HelpString"))
        Me.PB_LegMap.Name = "PB_LegMap"
        Me.HelpProvider1.SetShowHelp(Me.PB_LegMap, CType(resources.GetObject("PB_LegMap.ShowHelp"), Boolean))
        Me.PB_LegMap.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PB_LegMap, resources.GetString("PB_LegMap.ToolTip"))
        '
        'PB_Video
        '
        resources.ApplyResources(Me.PB_Video, "PB_Video")
        Me.PB_Video.BackColor = System.Drawing.Color.Transparent
        Me.PB_Video.BackgroundImage = Global.OHeadcamMapApp.My.Resources.Resources.vid_img1920x1080
        Me.HelpProvider1.SetHelpKeyword(Me.PB_Video, resources.GetString("PB_Video.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.PB_Video, CType(resources.GetObject("PB_Video.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.PB_Video, resources.GetString("PB_Video.HelpString"))
        Me.PB_Video.Name = "PB_Video"
        Me.HelpProvider1.SetShowHelp(Me.PB_Video, CType(resources.GetObject("PB_Video.ShowHelp"), Boolean))
        Me.PB_Video.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PB_Video, resources.GetString("PB_Video.ToolTip"))
        '
        'PB_HeightGraph
        '
        resources.ApplyResources(Me.PB_HeightGraph, "PB_HeightGraph")
        Me.PB_HeightGraph.BackgroundImage = Global.OHeadcamMapApp.My.Resources.Resources.routeimg
        Me.HelpProvider1.SetHelpKeyword(Me.PB_HeightGraph, resources.GetString("PB_HeightGraph.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.PB_HeightGraph, CType(resources.GetObject("PB_HeightGraph.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.PB_HeightGraph, resources.GetString("PB_HeightGraph.HelpString"))
        Me.PB_HeightGraph.Name = "PB_HeightGraph"
        Me.HelpProvider1.SetShowHelp(Me.PB_HeightGraph, CType(resources.GetObject("PB_HeightGraph.ShowHelp"), Boolean))
        Me.PB_HeightGraph.TabStop = False
        Me.ToolTip1.SetToolTip(Me.PB_HeightGraph, resources.GetString("PB_HeightGraph.ToolTip"))
        '
        'HelpProvider1
        '
        resources.ApplyResources(Me.HelpProvider1, "HelpProvider1")
        '
        'cbShowRoute
        '
        resources.ApplyResources(Me.cbShowRoute, "cbShowRoute")
        Me.cbShowRoute.Checked = Global.OHeadcamMapApp.My.MySettings.Default.cbShowRoute
        Me.cbShowRoute.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbShowRoute.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.OHeadcamMapApp.My.MySettings.Default, "cbShowRoute", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.HelpProvider1.SetHelpKeyword(Me.cbShowRoute, resources.GetString("cbShowRoute.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.cbShowRoute, CType(resources.GetObject("cbShowRoute.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.cbShowRoute, resources.GetString("cbShowRoute.HelpString"))
        Me.cbShowRoute.Name = "cbShowRoute"
        Me.HelpProvider1.SetShowHelp(Me.cbShowRoute, CType(resources.GetObject("cbShowRoute.ShowHelp"), Boolean))
        Me.ToolTip1.SetToolTip(Me.cbShowRoute, resources.GetString("cbShowRoute.ToolTip"))
        Me.cbShowRoute.UseVisualStyleBackColor = True
        '
        'txtRouteH
        '
        resources.ApplyResources(Me.txtRouteH, "txtRouteH")
        Me.txtRouteH.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "MRouteH", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.HelpProvider1.SetHelpKeyword(Me.txtRouteH, resources.GetString("txtRouteH.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.txtRouteH, CType(resources.GetObject("txtRouteH.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.txtRouteH, resources.GetString("txtRouteH.HelpString"))
        Me.txtRouteH.Name = "txtRouteH"
        Me.HelpProvider1.SetShowHelp(Me.txtRouteH, CType(resources.GetObject("txtRouteH.ShowHelp"), Boolean))
        Me.txtRouteH.Text = Global.OHeadcamMapApp.My.MySettings.Default.MRouteH
        Me.ToolTip1.SetToolTip(Me.txtRouteH, resources.GetString("txtRouteH.ToolTip"))
        '
        'txtRouteV
        '
        resources.ApplyResources(Me.txtRouteV, "txtRouteV")
        Me.txtRouteV.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "MRouteV", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.HelpProvider1.SetHelpKeyword(Me.txtRouteV, resources.GetString("txtRouteV.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.txtRouteV, CType(resources.GetObject("txtRouteV.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.txtRouteV, resources.GetString("txtRouteV.HelpString"))
        Me.txtRouteV.Name = "txtRouteV"
        Me.HelpProvider1.SetShowHelp(Me.txtRouteV, CType(resources.GetObject("txtRouteV.ShowHelp"), Boolean))
        Me.txtRouteV.Text = Global.OHeadcamMapApp.My.MySettings.Default.MRouteV
        Me.ToolTip1.SetToolTip(Me.txtRouteV, resources.GetString("txtRouteV.ToolTip"))
        '
        'cbShowSpeed
        '
        resources.ApplyResources(Me.cbShowSpeed, "cbShowSpeed")
        Me.cbShowSpeed.Checked = Global.OHeadcamMapApp.My.MySettings.Default.cbShowSpeedPanel
        Me.cbShowSpeed.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.OHeadcamMapApp.My.MySettings.Default, "cbShowSpeedPanel", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.HelpProvider1.SetHelpKeyword(Me.cbShowSpeed, resources.GetString("cbShowSpeed.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.cbShowSpeed, CType(resources.GetObject("cbShowSpeed.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.cbShowSpeed, resources.GetString("cbShowSpeed.HelpString"))
        Me.cbShowSpeed.Name = "cbShowSpeed"
        Me.HelpProvider1.SetShowHelp(Me.cbShowSpeed, CType(resources.GetObject("cbShowSpeed.ShowHelp"), Boolean))
        Me.ToolTip1.SetToolTip(Me.cbShowSpeed, resources.GetString("cbShowSpeed.ToolTip"))
        Me.cbShowSpeed.UseVisualStyleBackColor = True
        '
        'txtSpeedH
        '
        resources.ApplyResources(Me.txtSpeedH, "txtSpeedH")
        Me.txtSpeedH.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "MSpeedH", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.HelpProvider1.SetHelpKeyword(Me.txtSpeedH, resources.GetString("txtSpeedH.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.txtSpeedH, CType(resources.GetObject("txtSpeedH.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.txtSpeedH, resources.GetString("txtSpeedH.HelpString"))
        Me.txtSpeedH.Name = "txtSpeedH"
        Me.HelpProvider1.SetShowHelp(Me.txtSpeedH, CType(resources.GetObject("txtSpeedH.ShowHelp"), Boolean))
        Me.txtSpeedH.Text = Global.OHeadcamMapApp.My.MySettings.Default.MSpeedH
        Me.ToolTip1.SetToolTip(Me.txtSpeedH, resources.GetString("txtSpeedH.ToolTip"))
        '
        'txtSpeedV
        '
        resources.ApplyResources(Me.txtSpeedV, "txtSpeedV")
        Me.txtSpeedV.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "MSpeedV", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.HelpProvider1.SetHelpKeyword(Me.txtSpeedV, resources.GetString("txtSpeedV.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.txtSpeedV, CType(resources.GetObject("txtSpeedV.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.txtSpeedV, resources.GetString("txtSpeedV.HelpString"))
        Me.txtSpeedV.Name = "txtSpeedV"
        Me.HelpProvider1.SetShowHelp(Me.txtSpeedV, CType(resources.GetObject("txtSpeedV.ShowHelp"), Boolean))
        Me.txtSpeedV.Text = Global.OHeadcamMapApp.My.MySettings.Default.MSpeedV
        Me.ToolTip1.SetToolTip(Me.txtSpeedV, resources.GetString("txtSpeedV.ToolTip"))
        '
        'cbShowLeg
        '
        resources.ApplyResources(Me.cbShowLeg, "cbShowLeg")
        Me.cbShowLeg.Checked = Global.OHeadcamMapApp.My.MySettings.Default.cbShowLegMAp
        Me.cbShowLeg.CheckState = System.Windows.Forms.CheckState.Checked
        Me.cbShowLeg.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.OHeadcamMapApp.My.MySettings.Default, "cbShowLegMAp", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.HelpProvider1.SetHelpKeyword(Me.cbShowLeg, resources.GetString("cbShowLeg.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.cbShowLeg, CType(resources.GetObject("cbShowLeg.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.cbShowLeg, resources.GetString("cbShowLeg.HelpString"))
        Me.cbShowLeg.Name = "cbShowLeg"
        Me.HelpProvider1.SetShowHelp(Me.cbShowLeg, CType(resources.GetObject("cbShowLeg.ShowHelp"), Boolean))
        Me.ToolTip1.SetToolTip(Me.cbShowLeg, resources.GetString("cbShowLeg.ToolTip"))
        Me.cbShowLeg.UseVisualStyleBackColor = True
        '
        'txtLegMapH
        '
        resources.ApplyResources(Me.txtLegMapH, "txtLegMapH")
        Me.txtLegMapH.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "MlegMapH2", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.HelpProvider1.SetHelpKeyword(Me.txtLegMapH, resources.GetString("txtLegMapH.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.txtLegMapH, CType(resources.GetObject("txtLegMapH.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.txtLegMapH, resources.GetString("txtLegMapH.HelpString"))
        Me.txtLegMapH.Name = "txtLegMapH"
        Me.HelpProvider1.SetShowHelp(Me.txtLegMapH, CType(resources.GetObject("txtLegMapH.ShowHelp"), Boolean))
        Me.txtLegMapH.Text = Global.OHeadcamMapApp.My.MySettings.Default.MlegMapH2
        Me.ToolTip1.SetToolTip(Me.txtLegMapH, resources.GetString("txtLegMapH.ToolTip"))
        '
        'txtLegMapV
        '
        resources.ApplyResources(Me.txtLegMapV, "txtLegMapV")
        Me.txtLegMapV.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "MLegMapV", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.HelpProvider1.SetHelpKeyword(Me.txtLegMapV, resources.GetString("txtLegMapV.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.txtLegMapV, CType(resources.GetObject("txtLegMapV.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.txtLegMapV, resources.GetString("txtLegMapV.HelpString"))
        Me.txtLegMapV.Name = "txtLegMapV"
        Me.HelpProvider1.SetShowHelp(Me.txtLegMapV, CType(resources.GetObject("txtLegMapV.ShowHelp"), Boolean))
        Me.txtLegMapV.Text = Global.OHeadcamMapApp.My.MySettings.Default.MLegMapV
        Me.ToolTip1.SetToolTip(Me.txtLegMapV, resources.GetString("txtLegMapV.ToolTip"))
        '
        'numScale
        '
        resources.ApplyResources(Me.numScale, "numScale")
        Me.numScale.DataBindings.Add(New System.Windows.Forms.Binding("Value", Global.OHeadcamMapApp.My.MySettings.Default, "dFfmpegScaleMap", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.numScale.DecimalPlaces = 1
        Me.HelpProvider1.SetHelpKeyword(Me.numScale, resources.GetString("numScale.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.numScale, CType(resources.GetObject("numScale.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.numScale, resources.GetString("numScale.HelpString"))
        Me.numScale.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.numScale.Maximum = New Decimal(New Integer() {4, 0, 0, 0})
        Me.numScale.Name = "numScale"
        Me.HelpProvider1.SetShowHelp(Me.numScale, CType(resources.GetObject("numScale.ShowHelp"), Boolean))
        Me.ToolTip1.SetToolTip(Me.numScale, resources.GetString("numScale.ToolTip"))
        Me.numScale.Value = Global.OHeadcamMapApp.My.MySettings.Default.dFfmpegScaleMap
        '
        'NumTransparency
        '
        resources.ApplyResources(Me.NumTransparency, "NumTransparency")
        Me.NumTransparency.DataBindings.Add(New System.Windows.Forms.Binding("Value", Global.OHeadcamMapApp.My.MySettings.Default, "Transparency", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.NumTransparency.DecimalPlaces = 1
        Me.HelpProvider1.SetHelpKeyword(Me.NumTransparency, resources.GetString("NumTransparency.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.NumTransparency, CType(resources.GetObject("NumTransparency.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.NumTransparency, resources.GetString("NumTransparency.HelpString"))
        Me.NumTransparency.Increment = New Decimal(New Integer() {1, 0, 0, 65536})
        Me.NumTransparency.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.NumTransparency.Name = "NumTransparency"
        Me.HelpProvider1.SetShowHelp(Me.NumTransparency, CType(resources.GetObject("NumTransparency.ShowHelp"), Boolean))
        Me.ToolTip1.SetToolTip(Me.NumTransparency, resources.GetString("NumTransparency.ToolTip"))
        Me.NumTransparency.Value = Global.OHeadcamMapApp.My.MySettings.Default.Transparency
        '
        'cbShowHeight
        '
        resources.ApplyResources(Me.cbShowHeight, "cbShowHeight")
        Me.cbShowHeight.Checked = Global.OHeadcamMapApp.My.MySettings.Default.cbHeightGraph
        Me.cbShowHeight.DataBindings.Add(New System.Windows.Forms.Binding("Checked", Global.OHeadcamMapApp.My.MySettings.Default, "cbHeightGraph", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.HelpProvider1.SetHelpKeyword(Me.cbShowHeight, resources.GetString("cbShowHeight.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.cbShowHeight, CType(resources.GetObject("cbShowHeight.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.cbShowHeight, resources.GetString("cbShowHeight.HelpString"))
        Me.cbShowHeight.Name = "cbShowHeight"
        Me.HelpProvider1.SetShowHelp(Me.cbShowHeight, CType(resources.GetObject("cbShowHeight.ShowHelp"), Boolean))
        Me.ToolTip1.SetToolTip(Me.cbShowHeight, resources.GetString("cbShowHeight.ToolTip"))
        Me.cbShowHeight.UseVisualStyleBackColor = True
        '
        'txtHeightH
        '
        resources.ApplyResources(Me.txtHeightH, "txtHeightH")
        Me.txtHeightH.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "MHeightH", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.HelpProvider1.SetHelpKeyword(Me.txtHeightH, resources.GetString("txtHeightH.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.txtHeightH, CType(resources.GetObject("txtHeightH.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.txtHeightH, resources.GetString("txtHeightH.HelpString"))
        Me.txtHeightH.Name = "txtHeightH"
        Me.HelpProvider1.SetShowHelp(Me.txtHeightH, CType(resources.GetObject("txtHeightH.ShowHelp"), Boolean))
        Me.txtHeightH.Text = Global.OHeadcamMapApp.My.MySettings.Default.MHeightH
        Me.ToolTip1.SetToolTip(Me.txtHeightH, resources.GetString("txtHeightH.ToolTip"))
        '
        'txtHeightV
        '
        resources.ApplyResources(Me.txtHeightV, "txtHeightV")
        Me.txtHeightV.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.OHeadcamMapApp.My.MySettings.Default, "MHeightV", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.HelpProvider1.SetHelpKeyword(Me.txtHeightV, resources.GetString("txtHeightV.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me.txtHeightV, CType(resources.GetObject("txtHeightV.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me.txtHeightV, resources.GetString("txtHeightV.HelpString"))
        Me.txtHeightV.Name = "txtHeightV"
        Me.HelpProvider1.SetShowHelp(Me.txtHeightV, CType(resources.GetObject("txtHeightV.ShowHelp"), Boolean))
        Me.txtHeightV.Text = Global.OHeadcamMapApp.My.MySettings.Default.MHeightV
        Me.ToolTip1.SetToolTip(Me.txtHeightV, resources.GetString("txtHeightV.ToolTip"))
        '
        'frmMapOverlayForm
        '
        resources.ApplyResources(Me, "$this")
        Me.AllowDrop = True
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.PB_HeightGraph)
        Me.Controls.Add(Me.cbGoProF)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.numScale)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.NumTransparency)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.PB_SpeedPanel)
        Me.Controls.Add(Me.PB_Route)
        Me.Controls.Add(Me.PB_LegMap)
        Me.Controls.Add(Me.PB_Video)
        Me.HelpButton = True
        Me.HelpProvider1.SetHelpKeyword(Me, resources.GetString("$this.HelpKeyword"))
        Me.HelpProvider1.SetHelpNavigator(Me, CType(resources.GetObject("$this.HelpNavigator"), System.Windows.Forms.HelpNavigator))
        Me.HelpProvider1.SetHelpString(Me, resources.GetString("$this.HelpString"))
        Me.Name = "frmMapOverlayForm"
        Me.HelpProvider1.SetShowHelp(Me, CType(resources.GetObject("$this.ShowHelp"), Boolean))
        Me.ToolTip1.SetToolTip(Me, resources.GetString("$this.ToolTip"))
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        CType(Me.PB_SpeedPanel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PB_Route, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PB_LegMap, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PB_Video, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PB_HeightGraph, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.numScale, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.NumTransparency, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents PB_Video As PictureBox
    Friend WithEvents PB_LegMap As PictureBox
    Friend WithEvents PB_Route As PictureBox
    Friend WithEvents PB_SpeedPanel As PictureBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents txtSpeedV As TextBox
    Friend WithEvents Label13 As Label
    Friend WithEvents txtSpeedH As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents txtRouteV As TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents txtRouteH As TextBox
    Friend WithEvents Label16 As Label
    Friend WithEvents txtHeightV As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents txtHeightH As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents txtLegMapV As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents txtLegMapH As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents NumTransparency As NumericUpDown
    Friend WithEvents Label7 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents numScale As NumericUpDown
    Friend WithEvents Timer1 As Timer
    Friend WithEvents cbShowHeight As CheckBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents cbShowLeg As CheckBox
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents cbShowSpeed As CheckBox
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents cbShowRoute As CheckBox
    Friend WithEvents cbGoProF As CheckBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents HelpProvider1 As HelpProvider
    Friend WithEvents PB_HeightGraph As PictureBox
End Class
