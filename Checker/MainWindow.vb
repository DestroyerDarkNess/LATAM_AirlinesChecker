

Public Class MainWindow

#Region " Declare "

    Public Home As Dashboard = New Dashboard With {.TopLevel = False, .Visible = False, .FormBorderStyle = FormBorderStyle.None}
    Public ProxieChecker As PCheker = New PCheker With {.TopLevel = False, .Visible = False, .FormBorderStyle = FormBorderStyle.None}
    Public Settings As Config = New Config With {.TopLevel = False, .Visible = False, .FormBorderStyle = FormBorderStyle.None}
    Public About As About = New About With {.TopLevel = False, .Visible = False, .FormBorderStyle = FormBorderStyle.None}

#End Region

    Private Sub MainWindow_Load(sender As Object, e As EventArgs) Handles Me.Load

        PanelContainer.Controls.AddRange({Home, ProxieChecker, Settings, About})

        Guna2Button1.Checked = True

    End Sub

    Private Sub CheckChangeUI_CheckedChanged(sender As Object, e As EventArgs) Handles Guna2Button1.CheckedChanged, Guna2Button2.CheckedChanged, Guna2Button3.CheckedChanged, Guna2Button4.CheckedChanged

        Dim GunaButton As Guna.UI2.WinForms.Guna2Button = DirectCast(sender, Guna.UI2.WinForms.Guna2Button)

        If GunaButton.Checked = True Then

            If GunaButton Is Guna2Button1 Then
                Home.Visible = True
                Home.BringToFront()
            ElseIf GunaButton Is Guna2Button4 Then
                ProxieChecker.Visible = True
                ProxieChecker.BringToFront()
            ElseIf GunaButton Is Guna2Button3 Then
                Settings.Visible = True
                Settings.BringToFront()
            ElseIf GunaButton Is Guna2ButtoN2 Then
                About.Visible = True
                About.BringToFront()
            End If

        End If

    End Sub

End Class