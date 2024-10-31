Namespace SDK

    Public Class ProxyRotator

        Public Property Proxies As List(Of Proxy)
        Private counter As Integer = 0

        Public Sub New(ByVal proxies As List(Of Proxy))
            Me.Proxies = proxies
        End Sub

        Public Function GetNextProxy() As Proxy
            Try
                If Proxies.Count = 0 Then Return Nothing

                Dim proxy As Proxy = Proxies(counter Mod Proxies.Count)
                counter += 1

                Return proxy
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

    End Class

End Namespace

