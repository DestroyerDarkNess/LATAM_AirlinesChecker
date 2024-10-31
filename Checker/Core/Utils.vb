Namespace Core

    Public Class Utils

        Public Shared Function StringToHex(ByVal text As String) As String
            Dim hex As String = String.Empty
            For i As Integer = 0 To text.Length - 1
                hex &= Asc(text.Substring(i, 1)).ToString("x").ToUpper
            Next
            Return hex
        End Function

    End Class

End Namespace

