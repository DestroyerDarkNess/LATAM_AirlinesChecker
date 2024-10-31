Namespace SDK

    Public Class Proxy
        Public Property IP As String = String.Empty
        Public Property Port As String = String.Empty
        Public Property Type As EO.Base.ProxyType = Nothing

        Public Shared Function Parse(ByVal Data As String, ByVal Delimiter As String) As Proxy
            'Try

            If Data.Contains("//") Then

                Dim splitProxy As String() = Data.Replace("//", "").Split(":")
                If splitProxy.Count = 3 Then
                    Dim newProxy As New Proxy With {
           .Type = GetProxyType(splitProxy(0)),
           .IP = splitProxy(1),
           .Port = splitProxy(2)
       }
                    Return newProxy
                End If

            ElseIf Data.Contains(Delimiter) Then

                Dim DataSplit As String() = Data.Split(Delimiter)

                If DataSplit.Count = 2 Then
                    Return New Proxy With {.IP = DataSplit(0), .Port = DataSplit(1)}
                End If

            End If

            Return Nothing
            'Catch ex As Exception
            '    Return Nothing
            'End Try
        End Function

        Private Shared Function GetProxyType(type As String) As EO.Base.ProxyType
            Select Case type.ToLower()
                Case "http"
                    Return EO.Base.ProxyType.HTTP
                Case "https"
                    Return EO.Base.ProxyType.HTTPS
                Case "socks4"
                    Return EO.Base.ProxyType.Socks4
                Case "socks4a"
                    Return EO.Base.ProxyType.Socks4A
                Case "socks5"
                    Return EO.Base.ProxyType.Socks5
                Case Else
                    Throw New ArgumentException($"Invalid proxy type: {type}")
            End Select
        End Function

        Public Shared Function GetProxyTypeByIndex(ByVal Index As Integer) As EO.Base.ProxyType
            Return System.Enum.GetValues(GetType(EO.Base.ProxyType))(Index)
        End Function

        Public Overrides Function ToString() As String
            If Me.Type = Nothing Then
                Return String.Format("{0}:{1}", IP, Port)
            Else
                Return String.Format("{0}://{1}:{2}", Me.Type.ToString, Me.IP, Me.Port)
            End If
        End Function

    End Class

End Namespace

