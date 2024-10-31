Imports System.Runtime.InteropServices

Namespace Core

    Public Class ScrollUtils
        Private Const WM_VSCROLL As Integer = &H115
        Private Const SB_BOTTOM As Integer = 7

        <DllImport("user32.dll", SetLastError:=True)>
        Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
        End Function

        Public Shared Sub ScrollTableToBottom(ByVal tableControl As Control)
            If tableControl IsNot Nothing AndAlso tableControl.Handle <> IntPtr.Zero Then
                SendMessage(tableControl.Handle, WM_VSCROLL, CType(SB_BOTTOM, IntPtr), IntPtr.Zero)
            End If
        End Sub
    End Class

End Namespace

