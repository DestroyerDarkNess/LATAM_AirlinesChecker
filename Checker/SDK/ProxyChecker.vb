Imports EO.WebBrowser

Namespace SDK

    Public Class ProxyChecker : Implements IDisposable

        Public Enum Result As Integer
            Unknown = -1
            Valid = 0
            Invalid = 1
            Timeout = 2
            [Error] = 3
        End Enum

        Public Property Instance As String = "P1"
        Private Engine As EO.WebEngine.Engine = Nothing
        Private PResult As Result = Result.Unknown
        Private Execution_Watcher As Stopwatch = Nothing
        Public WithEvents WebViewHost As EO.WebBrowser.WebView = Nothing
        Public WebView As EO.WinForm.WebControl = Nothing

        Public Sub New(ByVal ID As String, ByVal Proxy As Proxy)
            Instance = ID

            Execution_Watcher = New Stopwatch
            Engine = EO.WebEngine.Engine.Create(Instance)
            Engine.Options.Proxy = New EO.Base.ProxyInfo(Proxy.Type, Proxy.IP, Proxy.Port)
            Engine.Options.CachePath = "ProxyChecker" & "\" & Instance
            Engine.Options.DisableBuiltInPlugIns = True
           
            WebViewHost = New EO.WebBrowser.WebView() With {.Engine = Engine}
            WebView = New EO.WinForm.WebControl With {.WebView = WebViewHost, .Dock = DockStyle.Fill}

            'AddHandler WebViewHost.LoadFailed, Sub(sender As Object, e As LoadFailedEventArgs)
            '                                       ErrorMessage = e.ErrorMessage
            '                                       If e.ErrorCode = ErrorCode.ProxyConnectionFailed Then
            '                                           PResult = Result.Invalid
            '                                       Else
            '                                           PResult = Result.Error
            '                                       End If
            '                                   End Sub


            AddHandler WebViewHost.BeforeDownload, Sub(sender As Object, e As BeforeDownloadEventArgs)
                                                       e.ShowDialog = False
                                                   End Sub

            AddHandler WebViewHost.NeedCredentials, Sub(ByVal sender As Object, ByVal e As NeedCredentialsEventArgs)
                                                        PResult = Result.Valid
                                                        'e.[Continue]("username", "password")
                                                    End Sub

            AddHandler WebViewHost.CertificateError, Sub(sender As Object, e As CertificateErrorEventArgs)
                                                         e.Continue()
                                                     End Sub
        End Sub

        Public Property ErrorMessage As String = String.Empty

        Public Async Function Validate(ByVal URLTarget As String) As Task(Of Result)

            Try
                If String.IsNullOrEmpty(URLTarget) Then URLTarget = "httpbin.org/ip"

                Dim Navigate As NavigationTask = WebViewHost.LoadUrl(URLTarget)

                Navigate.OnDone(Sub()

                                    If PResult = Result.Unknown Then
                                        Console.WriteLine("Result: " & Navigate.HttpStatusCode)
                                        If Navigate.HttpStatusCode = 200 Then
                                            PResult = Result.Valid
                                        Else
                                            ErrorMessage = Navigate.HttpStatusCode
                                            PResult = Result.Invalid
                                        End If

                                    End If

                                End Sub)

                Execution_Watcher.Start()

            Catch ex As Exception
                ErrorMessage = ex.Message
                PResult = Result.Error
            End Try

            For i As Integer = 0 To 2

                If DomAvailable() = True Then
                    If String.IsNullOrEmpty(ErrorMessage) = True Then
                        PResult = Result.Valid
                    Else
                        PResult = Result.Invalid
                    End If
                    Exit For
                ElseIf Execution_Watcher.Elapsed.Seconds > 30 Then
                    PResult = Result.Timeout
                    Exit For
                ElseIf Not PResult = Result.Unknown Then
                    Exit For
                End If

                Threading.Thread.Sleep(1000)
                Application.DoEvents()
                i -= 1
            Next


            Try
                Execution_Watcher.Stop()
                WebViewHost.StopLoad()
                WebViewHost.StopAllFindSessions()
            Catch ex As Exception

            End Try

            Return PResult
        End Function

        Private Function DomAvailable() As Boolean
            Try
                Dim DOM As EO.WebBrowser.DOM.Window = WebViewHost.GetDOMWindow()
                If DOM Is Nothing Then
                    Return False
                End If
                Dim Doc As EO.WebBrowser.DOM.Document = DOM.document
                If Doc Is Nothing Then
                    Return False
                End If
                Console.WriteLine(Doc.title)
                Return (String.IsNullOrEmpty(Doc.title) = False)
            Catch ex As Exception
                Return False
            End Try
        End Function


#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    Engine.CookieManager.DeleteCookies()
                    Engine.Stop(True)
                    WebView.Dispose()
                    WebViewHost.Dispose()
                    Try
                        Dim FolderPath As String = IO.Path.Combine(IO.Path.GetTempPath, WebViewHost.Engine.Options.CachePath)
                        If System.IO.Directory.Exists(FolderPath) Then System.IO.Directory.Delete(FolderPath, True)
                    Catch ex As Exception

                    End Try
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

#End Region


    End Class

End Namespace

