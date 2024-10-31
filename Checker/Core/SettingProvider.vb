Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Namespace Core
    Public Class SettingProvider

        <System.Runtime.InteropServices.DllImport("kernel32")>
        Private Shared Function GetPrivateProfileStringA(ByVal lpAppName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As System.Text.StringBuilder, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
        End Function

        <System.Runtime.InteropServices.DllImport("kernel32")>
        Private Shared Function WritePrivateProfileStringA(ByVal lpAppName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Integer
        End Function

        Public Property FileSetting As String = System.IO.Path.Combine(IO.Path.GetTempPath, "LatamChecker.ini")

        Public Sub New()
            If System.IO.File.Exists(FileSetting) = False Then System.IO.File.WriteAllText(FileSetting, "")
        End Sub

        Public Function ReadIni(ByVal Section As String, ByVal Key As String, ByVal Optional DefaultValue As String = Nothing) As String
            Dim buffer As System.Text.StringBuilder = New System.Text.StringBuilder(260)
            GetPrivateProfileStringA(Section, Key, DefaultValue, buffer, buffer.Capacity, FileSetting)
            Return buffer.ToString()
        End Function

        Public Function WriteIni(ByVal Section As String, ByVal Key As String, ByVal Value As String) As Boolean
            Return (WritePrivateProfileStringA(Section, Key, Value, FileSetting) <> 0)
        End Function

        Public Function WriteIniList(ByVal Section As String, ByVal Key As String, ByVal values As List(Of String), ByVal Optional Delimiter As String = ",") As Boolean
            Dim joinedValues As String = String.Join(Delimiter, values)
            Return WriteIni(Section, Key, joinedValues)
        End Function

        Public Function ReadIniList(ByVal Section As String, ByVal Key As String, ByVal Optional Delimiter As String = ",") As List(Of String)
            Dim joinedValues As String = ReadIni(Section, Key)

            If Not String.IsNullOrEmpty(joinedValues) Then
                Return joinedValues.Split(Delimiter.ToCharArray().FirstOrDefault()).ToList()
            End If

            Return New List(Of String)()
        End Function

    End Class
End Namespace
