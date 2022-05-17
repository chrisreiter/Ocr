<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form4
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
        Me.bkCancel = New System.Windows.Forms.Button
        Me.cbLanguages = New System.Windows.Forms.CheckedListBox
        Me.bkOK = New System.Windows.Forms.Button
        Me.label2 = New System.Windows.Forms.Label
        Me.cbLanguagesAsian = New System.Windows.Forms.CheckedListBox
        Me.label1 = New System.Windows.Forms.Label
        Me.label4 = New System.Windows.Forms.Label
        Me.label3 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'bkCancel
        '
        Me.bkCancel.Location = New System.Drawing.Point(203, 498)
        Me.bkCancel.Name = "bkCancel"
        Me.bkCancel.Size = New System.Drawing.Size(75, 23)
        Me.bkCancel.TabIndex = 5
        Me.bkCancel.Text = "Cancel"
        Me.bkCancel.UseVisualStyleBackColor = True
        '
        'cbLanguages
        '
        Me.cbLanguages.CheckOnClick = True
        Me.cbLanguages.FormattingEnabled = True
        Me.cbLanguages.Items.AddRange(New Object() {"Bulgarian", "Catalan", "Croatian", "Czech", "Danish", "Dutch", "English", "Estonian", "Finnish", "French", "German", "Hungarian", "Indonesian", "Italian", "Latvian", "Lithuanian", "Norwegian", "Polish", "Portuguese", "Romanian", "Russian", "Slovak", "Slovenian", "Spanish", "Swedish", "Turkish"})
        Me.cbLanguages.Location = New System.Drawing.Point(12, 22)
        Me.cbLanguages.Name = "cbLanguages"
        Me.cbLanguages.Size = New System.Drawing.Size(153, 424)
        Me.cbLanguages.Sorted = True
        Me.cbLanguages.TabIndex = 4
        '
        'bkOK
        '
        Me.bkOK.Location = New System.Drawing.Point(122, 498)
        Me.bkOK.Name = "bkOK"
        Me.bkOK.Size = New System.Drawing.Size(75, 23)
        Me.bkOK.TabIndex = 3
        Me.bkOK.Text = "OK"
        Me.bkOK.UseVisualStyleBackColor = True
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(241, 6)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(125, 13)
        Me.label2.TabIndex = 7
        Me.label2.Text = "Asian module languages:"
        '
        'cbLanguagesAsian
        '
        Me.cbLanguagesAsian.CheckOnClick = True
        Me.cbLanguagesAsian.FormattingEnabled = True
        Me.cbLanguagesAsian.Items.AddRange(New Object() {"Arabic", "Chinese_Simplified", "Chinese_Traditional", "Japanese", "Korean"})
        Me.cbLanguagesAsian.Location = New System.Drawing.Point(241, 22)
        Me.cbLanguagesAsian.Name = "cbLanguagesAsian"
        Me.cbLanguagesAsian.Size = New System.Drawing.Size(153, 139)
        Me.cbLanguagesAsian.Sorted = True
        Me.cbLanguagesAsian.TabIndex = 6
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(12, 6)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(122, 13)
        Me.label1.TabIndex = 8
        Me.label1.Text = "Main module languages:"
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(12, 475)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(394, 13)
        Me.label4.TabIndex = 10
        Me.label4.Text = "But you can use both main and asian languages on same image in different zones."
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(12, 458)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(359, 13)
        Me.label3.TabIndex = 9
        Me.label3.Text = "NOTE: Using both main and asian languages in one zone is not supported."
        '
        'Form4
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(409, 529)
        Me.Controls.Add(Me.label4)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.cbLanguagesAsian)
        Me.Controls.Add(Me.bkCancel)
        Me.Controls.Add(Me.cbLanguages)
        Me.Controls.Add(Me.bkOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form4"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form4"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents bkCancel As System.Windows.Forms.Button
    Private WithEvents cbLanguages As System.Windows.Forms.CheckedListBox
    Private WithEvents bkOK As System.Windows.Forms.Button
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents cbLanguagesAsian As System.Windows.Forms.CheckedListBox
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents label4 As System.Windows.Forms.Label
    Private WithEvents label3 As System.Windows.Forms.Label
End Class
