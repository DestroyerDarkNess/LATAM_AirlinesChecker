<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Config
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Guna2CheckBox1 = New Guna.UI2.WinForms.Guna2CheckBox()
        Me.Guna2HtmlToolTip1 = New Guna.UI2.WinForms.Guna2HtmlToolTip()
        Me.Guna2CheckBox2 = New Guna.UI2.WinForms.Guna2CheckBox()
        Me.SuspendLayout()
        '
        'Guna2CheckBox1
        '
        Me.Guna2CheckBox1.AutoSize = True
        Me.Guna2CheckBox1.CheckedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Guna2CheckBox1.CheckedState.BorderRadius = 3
        Me.Guna2CheckBox1.CheckedState.BorderThickness = 1
        Me.Guna2CheckBox1.CheckedState.FillColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Guna2CheckBox1.Location = New System.Drawing.Point(12, 12)
        Me.Guna2CheckBox1.Name = "Guna2CheckBox1"
        Me.Guna2CheckBox1.Size = New System.Drawing.Size(115, 17)
        Me.Guna2CheckBox1.TabIndex = 12
        Me.Guna2CheckBox1.Text = "JavaScript Booster"
        Me.Guna2HtmlToolTip1.SetToolTip(Me.Guna2CheckBox1, "Improves checker verification speed by 200%.")
        Me.Guna2CheckBox1.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(125, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(149, Byte), Integer))
        Me.Guna2CheckBox1.UncheckedState.BorderRadius = 3
        Me.Guna2CheckBox1.UncheckedState.BorderThickness = 1
        Me.Guna2CheckBox1.UncheckedState.FillColor = System.Drawing.Color.FromArgb(CType(CType(125, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(149, Byte), Integer))
        '
        'Guna2HtmlToolTip1
        '
        Me.Guna2HtmlToolTip1.AllowLinksHandling = True
        Me.Guna2HtmlToolTip1.MaximumSize = New System.Drawing.Size(0, 0)
        '
        'Guna2CheckBox2
        '
        Me.Guna2CheckBox2.AutoSize = True
        Me.Guna2CheckBox2.CheckedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Guna2CheckBox2.CheckedState.BorderRadius = 3
        Me.Guna2CheckBox2.CheckedState.BorderThickness = 1
        Me.Guna2CheckBox2.CheckedState.FillColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Guna2CheckBox2.Location = New System.Drawing.Point(12, 49)
        Me.Guna2CheckBox2.Name = "Guna2CheckBox2"
        Me.Guna2CheckBox2.Size = New System.Drawing.Size(157, 17)
        Me.Guna2CheckBox2.TabIndex = 13
        Me.Guna2CheckBox2.Text = "WEB Fingerprinting Spoofer"
        Me.Guna2HtmlToolTip1.SetToolTip(Me.Guna2CheckBox2, "Try to reduce web behavior detection and analysis, change client values.")
        Me.Guna2CheckBox2.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(125, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(149, Byte), Integer))
        Me.Guna2CheckBox2.UncheckedState.BorderRadius = 3
        Me.Guna2CheckBox2.UncheckedState.BorderThickness = 1
        Me.Guna2CheckBox2.UncheckedState.FillColor = System.Drawing.Color.FromArgb(CType(CType(125, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(149, Byte), Integer))
        '
        'Config
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(24, Byte), Integer), CType(CType(24, Byte), Integer), CType(CType(24, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(716, 407)
        Me.Controls.Add(Me.Guna2CheckBox2)
        Me.Controls.Add(Me.Guna2CheckBox1)
        Me.ForeColor = System.Drawing.Color.White
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Config"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Config"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Guna2CheckBox1 As Guna.UI2.WinForms.Guna2CheckBox
    Friend WithEvents Guna2HtmlToolTip1 As Guna.UI2.WinForms.Guna2HtmlToolTip
    Friend WithEvents Guna2CheckBox2 As Guna.UI2.WinForms.Guna2CheckBox
End Class
