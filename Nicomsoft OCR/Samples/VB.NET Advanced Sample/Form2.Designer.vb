<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form22
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
        Me.bkSetDefault = New System.Windows.Forms.Button
        Me.label2 = New System.Windows.Forms.Label
        Me.cbSrc = New System.Windows.Forms.ComboBox
        Me.label1 = New System.Windows.Forms.Label
        Me.cbScanners = New System.Windows.Forms.ComboBox
        Me.cbNoUI = New System.Windows.Forms.CheckBox
        Me.bkCancel = New System.Windows.Forms.Button
        Me.bkScan = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'bkSetDefault
        '
        Me.bkSetDefault.Location = New System.Drawing.Point(393, 43)
        Me.bkSetDefault.Name = "bkSetDefault"
        Me.bkSetDefault.Size = New System.Drawing.Size(96, 23)
        Me.bkSetDefault.TabIndex = 15
        Me.bkSetDefault.Text = "Set as Default"
        Me.bkSetDefault.UseVisualStyleBackColor = True
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(9, 71)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(96, 13)
        Me.label2.TabIndex = 14
        Me.label2.Text = "Document Source:"
        '
        'cbSrc
        '
        Me.cbSrc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSrc.FormattingEnabled = True
        Me.cbSrc.Items.AddRange(New Object() {"Flatbed", "ADF(Automatic Document Feeder)", "Auto"})
        Me.cbSrc.Location = New System.Drawing.Point(111, 68)
        Me.cbSrc.Name = "cbSrc"
        Me.cbSrc.Size = New System.Drawing.Size(203, 21)
        Me.cbSrc.TabIndex = 13
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(9, 15)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(77, 13)
        Me.label1.TabIndex = 12
        Me.label1.Text = "Select Device:"
        '
        'cbScanners
        '
        Me.cbScanners.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbScanners.FormattingEnabled = True
        Me.cbScanners.Location = New System.Drawing.Point(111, 12)
        Me.cbScanners.Name = "cbScanners"
        Me.cbScanners.Size = New System.Drawing.Size(378, 21)
        Me.cbScanners.TabIndex = 11
        '
        'cbNoUI
        '
        Me.cbNoUI.AutoSize = True
        Me.cbNoUI.Location = New System.Drawing.Point(12, 132)
        Me.cbNoUI.Name = "cbNoUI"
        Me.cbNoUI.Size = New System.Drawing.Size(97, 17)
        Me.cbNoUI.TabIndex = 10
        Me.cbNoUI.Text = "No Scanner UI"
        Me.cbNoUI.UseVisualStyleBackColor = True
        '
        'bkCancel
        '
        Me.bkCancel.Location = New System.Drawing.Point(252, 230)
        Me.bkCancel.Name = "bkCancel"
        Me.bkCancel.Size = New System.Drawing.Size(75, 23)
        Me.bkCancel.TabIndex = 9
        Me.bkCancel.Text = "Cancel"
        Me.bkCancel.UseVisualStyleBackColor = True
        '
        'bkScan
        '
        Me.bkScan.Location = New System.Drawing.Point(171, 230)
        Me.bkScan.Name = "bkScan"
        Me.bkScan.Size = New System.Drawing.Size(75, 23)
        Me.bkScan.TabIndex = 8
        Me.bkScan.Text = "Scan"
        Me.bkScan.UseVisualStyleBackColor = True
        '
        'Form22
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(501, 262)
        Me.Controls.Add(Me.bkSetDefault)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.cbSrc)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.cbScanners)
        Me.Controls.Add(Me.cbNoUI)
        Me.Controls.Add(Me.bkCancel)
        Me.Controls.Add(Me.bkScan)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form22"
        Me.Text = "Scanning"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents bkSetDefault As System.Windows.Forms.Button
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents cbSrc As System.Windows.Forms.ComboBox
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents cbScanners As System.Windows.Forms.ComboBox
    Private WithEvents cbNoUI As System.Windows.Forms.CheckBox
    Private WithEvents bkCancel As System.Windows.Forms.Button
    Private WithEvents bkScan As System.Windows.Forms.Button
End Class
