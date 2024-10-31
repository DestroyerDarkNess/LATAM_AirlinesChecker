Imports System.Runtime.InteropServices

Public Class Program

    <System.Runtime.InteropServices.DllImport("user32.dll")>
    Private Shared Function SetProcessDPIAware() As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True, ExactSpelling:=True)>
    Private Shared Function FreeConsole() As Boolean
    End Function

    Public Shared ID As String = String.Empty
    Public Shared Settings As Core.SettingProvider = New Core.SettingProvider

    <STAThread>
    Public Shared Sub Main()
        ' EO.Base.Runtime.EnableEOWP = True

        EO.WebBrowser.Runtime.AddLicense("Kb114+30EO2s3OmxGeCm3MGz8M5nzunz7fGo7vf2HaF3s7P9FOKe5ff2EL112PD9GvZ3s+X1D5+t8PT26KF+xrLUE/Go5Omzy5+v3PYEFO6ntKbC461pmaTA6bto2PD9GvZ3s/MDD+SrwPL3Gp+d2Pj26KFpqbPC3a5rp7XIzZ+v3PYEFO6ntKbC46FotcAEFOan2PgGHeR36d7SGeWawbMKFOervtrI9eBysO3XErx2s7MEFOan2PgGHeR3s7P9FOKe5ff26XXj7fQQ7azcws0X6Jzc8gQQyJ21tMbbtnCttcbcs3Wm8PoO5Kfq6doP")
        '   EO.WebEngine.Engine.Default.Options.AllowProprietaryMediaFormats()
        EO.Base.Runtime.EnableCrashReport = False
        AddHandler EO.Base.Runtime.Exception, AddressOf Runtime_Exception

        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        SetProcessDPIAware()
        ' FreeConsole()

        ID = Guid.NewGuid().ToString("N")

        Application.Run(New MainWindow)
    End Sub



    Private Shared Sub Runtime_Exception(ByVal sender As Object, ByVal e As EO.Base.ExceptionEventArgs)
        Console.WriteLine("Runtime Exception : " & e.ErrorException.Message)
    End Sub

End Class