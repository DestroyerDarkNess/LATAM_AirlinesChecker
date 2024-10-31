Public Class Config

#Region " Constructor "

    Public Sub New()
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.BackColor = Color.Transparent
    End Sub

#End Region

    Private Sub Config_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Config_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        LoadSettings()
    End Sub

    Private Sub Guna2CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles Guna2CheckBox1.CheckedChanged
        SaveSettings()
    End Sub

    Private Sub Guna2CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles Guna2CheckBox2.CheckedChanged
        SaveSettings()
    End Sub

    Private Sub LoadSettings()
        Guna2CheckBox1.Checked = Program.Settings.ReadIni("Config", "JsBoost", False)
        Guna2CheckBox2.Checked = Program.Settings.ReadIni("Config", "JSPatchs", False)
        IsLoaded = True
    End Sub

    Private IsLoaded As Boolean = False
    Private Sub SaveSettings()
        If IsLoaded = True Then
            Program.Settings.WriteIni("Config", "JsBoost", Guna2CheckBox1.Checked)
            Program.Settings.WriteIni("Config", "JSPatchs", Guna2CheckBox2.Checked)
        End If
    End Sub

End Class