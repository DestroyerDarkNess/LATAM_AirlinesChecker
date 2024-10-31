Imports System.IO
Imports Checker.SDK
Imports Guna.UI2.WinForms

Public Class PCheker

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

    Private Sub PCheker_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        For Each tstEnum As EO.Base.ProxyType In System.Enum.GetValues(GetType(EO.Base.ProxyType))
            Guna2ComboBox1.Items.Add(tstEnum.ToString)
        Next
        Guna2ComboBox1.SelectedIndex = 0

    End Sub

    Private Sub Guna2TextBox1_Click(sender As Object, e As EventArgs) Handles Guna2TextBox1.Click
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            Guna2TextBox1.Text = OpenFileDialog1.FileName
            Guna2Panel2.Visible = True
            Me.ActiveControl = Nothing
        End If
    End Sub

    Private ProxyT As EO.Base.ProxyType = EO.Base.ProxyType.HTTP

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        Guna2Button1.Enabled = False
        InvalidLabel.Text = 0
        ValidLabel.Text = 0
        AcoountCount.Text = 0
        Table1.DataSource = Nothing


        'If StreamValid IsNot Nothing Then
        '    StreamValid.Close()
        '    StreamValid.Dispose()
        '    StreamValid = Nothing
        'End If

        System.Threading.Thread.Sleep(500)

        ProxyT = SDK.Proxy.GetProxyTypeByIndex(Guna2ComboBox1.SelectedIndex)

        Dim FileCombo As String = Guna2TextBox1.Text
        Dim Delimiter As String = Guna2TextBox2.Text

        Dim ResPath As String = IO.Path.Combine(IO.Path.GetDirectoryName(FileCombo), IO.Path.GetFileNameWithoutExtension(FileCombo))


        'If StreamValid Is Nothing Then StreamValid = New StreamWriter(ResPath & "_Valid.txt")

        StartChecker(FileCombo, Delimiter)

        Guna2Button3.Enabled = True
    End Sub


#Region " Checker "

    Private IsCancel As Boolean = False
    'Private StreamValid As StreamWriter = Nothing

    Private CacheCleanInterval As Integer = 100

    Private Sub StartChecker(ByVal FileCombo As String, ByVal Delimiter As String)
        Dim Asynctask As New Task(New Action(Async Sub()

                                                 Dim ProxyRotator As ProxyRotator = New ProxyRotator(ProxyList)
                                                 Dim BaseInterval As Integer = CacheCleanInterval

                                                 If File.Exists(FileCombo) Then
                                                     Dim AccListVerify As List(Of TradeViewModel) = New List(Of TradeViewModel)()
                                                     Dim ChangeProxy As Boolean = False
                                                     Dim FirsProxy As String = String.Empty

                                                     Dim ResPath As String = IO.Path.Combine(IO.Path.GetDirectoryName(FileCombo), IO.Path.GetFileNameWithoutExtension(FileCombo))

                                                     For Each Line As String In File.ReadLines(FileCombo)
                                                         Try
                                                             Dim DefUrl As String = Guna2TextBox3.Text

                                                             'If String.IsNullOrEmpty(DefUrl) = True Then
                                                             '    DefUrl = API.Models.URLValues.Login
                                                             'End If

                                                             If IsCancel = True Then
                                                                 IsCancel = False
                                                                 Exit For
                                                             End If

                                                             Dim ProxyEx As Proxy = Proxy.Parse(Line, Delimiter)

                                                             If ProxyEx IsNot Nothing Then

                                                                 If ProxyEx.Type = Nothing Then
                                                                     ProxyEx.Type = ProxyT
                                                                 End If

                                                                 Dim InstanceID As String = Core.Utils.StringToHex(ProxyEx.ToString)
                                                                 Dim Checker As SDK.ProxyChecker = New SDK.ProxyChecker(InstanceID, ProxyEx)

                                                                 Me.BeginInvoke(Sub()
                                                                                    Panel1.Controls.Clear()
                                                                                    Panel1.Controls.Add(Checker.WebView)
                                                                                    Checker.WebViewHost.ZoomFactor = 0.5
                                                                                End Sub)


                                                                 Dim ValidateProxy As SDK.ProxyChecker.Result = Await Checker.Validate(DefUrl)

                                                                 Dim AccountResult As TradeViewModel = New TradeViewModel() With {
                    .User = String.Format("{0}:{1}", ProxyEx.IP, ProxyEx.Port),
                    .Pass = ProxyEx.Type.ToString,
                    .Result = (ValidateProxy.ToString & String.Format("({0})", Checker.ErrorMessage)).Replace("()", "").Replace("(0)", "")
                }


                                                                 AccListVerify.Add(AccountResult)

                                                                 Me.BeginInvoke(Sub()
                                                                                    AcoountCount.Text += 1

                                                                                    If ValidateProxy = SDK.ProxyChecker.Result.Valid Then
                                                                                        Console.WriteLine("Valid: " + ProxyEx.ToString)
                                                                                        Try
                                                                                            Using StreamValid As StreamWriter = New StreamWriter(ResPath & "_Valid.txt", True)
                                                                                                StreamValid.WriteLine(ProxyEx.ToString)
                                                                                            End Using
                                                                                        Catch ex As Exception
                                                                                            Console.WriteLine("Write error : " & ex.Message)
                                                                                        End Try


                                                                                        ValidLabel.Text += 1
                                                                                    Else
                                                                                        Console.WriteLine("InValid: " + ProxyEx.ToString)
                                                                                        InvalidLabel.Text += 1
                                                                                    End If

                                                                                    Table1.DataSource = AccListVerify

                                                                                    If AcoountCount.Text >= BaseInterval Then
                                                                                        BaseInterval += CacheCleanInterval
                                                                                        Console.Clear()
                                                                                        Console.WriteLine("-------------> Cleaning Cache !!")
                                                                                        AccListVerify.Clear()
                                                                                    End If

                                                                                    If AccListVerify.Count > 0 Then
                                                                                        Table1.NotifyDataSetChanged()
                                                                                    End If

                                                                                    Core.ScrollUtils.ScrollTableToBottom(Table1)

                                                                                    Checker.Dispose()
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