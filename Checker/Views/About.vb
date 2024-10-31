Public Class About

#Region " Constructor "

    Public Sub New()
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.BackColor = Color.Transparent
    End Sub

#End Region

    Private Sub About_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

End Class