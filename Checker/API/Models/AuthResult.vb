Namespace API.Models

    Public Class AuthResult
        Public Property IsAlive As Boolean = False
        Public Property IsUserValid As Boolean = False
        Public Property RequiredProxyChanged As Boolean = False
        Public Property ErrorMessage As String = String.Empty
        Public Property Status As String = String.Empty

        Public Sub New(ByVal Alive As Boolean, Optional ByVal Msg As String = "")
            Me.IsAlive = Alive
            If String.IsNullOrEmpty(Msg) = False Then Me.ErrorMessage = Msg
        End Sub
    End Class

End Namespace

