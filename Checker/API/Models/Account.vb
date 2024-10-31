Namespace API.Models

    Public Class Account
        Public Property Username As String = String.Empty
        Public Property Password As String = String.Empty

        Public Sub New(username As String, password As String)
            Me.Username = username
            Me.Password = password
        End Sub

        Public Shared Function Parse(ByVal Data As String, ByVal Delimiter As String) As Account
            Try

                If Data.Contains(Delimiter) Then

                    Dim DataSplit As String() = Data.Split(Delimiter)

                    If DataSplit.Count = 2 Then
                        Return New Account(DataSplit(0), DataSplit(1))
                    End If

                End If

                Return Nothing
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

    End Class

End Namespace

