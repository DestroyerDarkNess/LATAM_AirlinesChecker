Namespace API.Models

    Public Class NavigationResult
        Public Property HttpStatusCode As String = String.Empty
        Public Property ErrorCode As String = String.Empty
        Public Property IsAlive As Boolean = False
        Public Property Message As String = String.Empty

        Public Sub New(HttpStatusCode As String, ErrorCode As String)
            Me.HttpStatusCode = HttpStatusCode
            Me.ErrorCode = ErrorCode
        End Sub
    End Class

End Namespace

