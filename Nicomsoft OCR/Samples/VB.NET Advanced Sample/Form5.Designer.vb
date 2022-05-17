<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form5
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
        Me.bkHelp = New System.Windows.Forms.Button
        Me.edPDFDPI = New System.Windows.Forms.TextBox
        Me.label8 = New System.Windows.Forms.Label
        Me.edTextQual = New System.Windows.Forms.TextBox
        Me.label6 = New System.Windows.Forms.Label
        Me.edBinThreshold = New System.Windows.Forms.TextBox
        Me.label7 = New System.Windows.Forms.Label
        Me.edDisabledChars = New System.Windows.Forms.TextBox
        Me.label5 = New System.Windows.Forms.Label
        Me.edEnabledChars = New System.Windows.Forms.TextBox
        Me.label4 = New System.Windows.Forms.Label
        Me.cbBinTwice = New System.Windows.Forms.CheckBox
        Me.cbFastMode = New System.Windows.Forms.CheckBox
        Me.cbGrayMode = New System.Windows.Forms.CheckBox
        Me.cbRemoveLines = New System.Windows.Forms.CheckBox
        Me.cbImgNoiseFilter = New System.Windows.Forms.CheckBox
        Me.cbRotation = New System.Windows.Forms.CheckBox
        Me.cbDeskew = New System.Windows.Forms.CheckBox
        Me.cbZonesInversion = New System.Windows.Forms.CheckBox
        Me.cbImgInversion = New System.Windows.Forms.CheckBox
        Me.cbFindBarcodes = New System.Windows.Forms.CheckBox
        Me.bkCancel = New System.Windows.Forms.Button
        Me.bkOK = New System.Windows.Forms.Button
        Me.label3 = New System.Windows.Forms.Label
        Me.label2 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.cbCorrectMixed = New System.Windows.Forms.CheckBox
        Me.cbDictionary = New System.Windows.Forms.CheckBox
        Me.cbOneColumn = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'bkHelp
        '
        Me.bkHelp.Location = New System.Drawing.Point(390, 540)
        Me.bkHelp.Name = "bkHelp"
        Me.bkHelp.Size = New System.Drawing.Size(75, 23)
        Me.bkHelp.TabIndex = 51
        Me.bkHelp.Text = "Help"
        Me.bkHelp.UseVisualStyleBackColor = True
        '
        'edPDFDPI
        '
        Me.edPDFDPI.Location = New System.Drawing.Point(223, 479)
        Me.edPDFDPI.Name = "edPDFDPI"
        Me.edPDFDPI.Size = New System.Drawing.Size(53, 20)
        Me.edPDFDPI.TabIndex = 50
        '
        'label8
        '
        Me.label8.AutoSize = True
        Me.label8.Location = New System.Drawing.Point(14, 482)
        Me.label8.Name = "label8"
        Me.label8.Size = New System.Drawing.Size(99, 13)
        Me.label8.TabIndex = 49
        Me.label8.Text = "PDF rendering DPI:"
        '
        'edTextQual
        '
        Me.edTextQual.Location = New System.Drawing.Point(223, 453)
        Me.edTextQual.Name = "edTextQual"
        Me.edTextQual.Size = New System.Drawing.Size(53, 20)
        Me.edTextQual.TabIndex = 48
        '
        'label6
        '
        Me.label6.AutoSize = True
        Me.label6.Location = New System.Drawing.Point(14, 456)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(148, 13)
        Me.label6.TabIndex = 47
        Me.label6.Text = "Text quality (0..100; -1 - auto):"
        '
        'edBinThreshold
        '
        Me.edBinThreshold.Location = New System.Drawing.Point(223, 427)
        Me.edBinThreshold.Name = "edBinThreshold"
        Me.edBinThreshold.Size = New System.Drawing.Size(53, 20)
        Me.edBinThreshold.TabIndex = 46
        '
        'label7
        '
        Me.label7.AutoSize = True
        Me.label7.Location = New System.Drawing.Point(14, 430)
        Me.label7.Name = "label7"
        Me.label7.Size = New System.Drawing.Size(203, 13)
        Me.label7.TabIndex = 45
        Me.label7.Text = "Binarization threshold (0..254; 255 - auto):"
        '
        'edDisabledChars
        '
        Me.edDisabledChars.Location = New System.Drawing.Point(98, 397)
        Me.edDisabledChars.Name = "edDisabledChars"
        Me.edDisabledChars.Size = New System.Drawing.Size(320, 20)
        Me.edDisabledChars.TabIndex = 44
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Location = New System.Drawing.Point(14, 404)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(80, 13)
        Me.label5.TabIndex = 43
        Me.label5.Text = "Disabled chars:"
        '
        'edEnabledChars
        '
        Me.edEnabledChars.Location = New System.Drawing.Point(98, 371)
        Me.edEnabledChars.Name = "edEnabledChars"
        Me.edEnabledChars.Size = New System.Drawing.Size(320, 20)
        Me.edEnabledChars.TabIndex = 42
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(14, 378)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(78, 13)
        Me.label4.TabIndex = 41
        Me.label4.Text = "Enabled chars:"
        '
        'cbBinTwice
        '
        Me.cbBinTwice.AutoSize = True
        Me.cbBinTwice.Location = New System.Drawing.Point(15, 276)
        Me.cbBinTwice.Name = "cbBinTwice"
        Me.cbBinTwice.Size = New System.Drawing.Size(91, 17)
        Me.cbBinTwice.TabIndex = 40
        Me.cbBinTwice.Text = "Binarize twice"
        Me.cbBinTwice.UseVisualStyleBackColor = True
        '
        'cbFastMode
        '
        Me.cbFastMode.AutoSize = True
        Me.cbFastMode.Location = New System.Drawing.Point(15, 253)
        Me.cbFastMode.Name = "cbFastMode"
        Me.cbFastMode.Size = New System.Drawing.Size(147, 17)
        Me.cbFastMode.TabIndex = 39
        Me.cbFastMode.Text = "Fast mode (less accurate)"
        Me.cbFastMode.UseVisualStyleBackColor = True
        '
        'cbGrayMode
        '
        Me.cbGrayMode.AutoSize = True
        Me.cbGrayMode.Location = New System.Drawing.Point(15, 230)
        Me.cbGrayMode.Name = "cbGrayMode"
        Me.cbGrayMode.Size = New System.Drawing.Size(77, 17)
        Me.cbGrayMode.TabIndex = 38
        Me.cbGrayMode.Text = "Gray mode"
        Me.cbGrayMode.UseVisualStyleBackColor = True
        '
        'cbRemoveLines
        '
        Me.cbRemoveLines.AutoSize = True
        Me.cbRemoveLines.Location = New System.Drawing.Point(15, 207)
        Me.cbRemoveLines.Name = "cbRemoveLines"
        Me.cbRemoveLines.Size = New System.Drawing.Size(141, 17)
        Me.cbRemoveLines.TabIndex = 37
        Me.cbRemoveLines.Text = "Detect and remove lines"
        Me.cbRemoveLines.UseVisualStyleBackColor = True
        '
        'cbImgNoiseFilter
        '
        Me.cbImgNoiseFilter.AutoSize = True
        Me.cbImgNoiseFilter.Location = New System.Drawing.Point(15, 184)
        Me.cbImgNoiseFilter.Name = "cbImgNoiseFilter"
        Me.cbImgNoiseFilter.Size = New System.Drawing.Size(148, 17)
        Me.cbImgNoiseFilter.TabIndex = 36
        Me.cbImgNoiseFilter.Text = "Apply noise filter for image"
        Me.cbImgNoiseFilter.UseVisualStyleBackColor = True
        '
        'cbRotation
        '
        Me.cbRotation.AutoSize = True
        Me.cbRotation.Location = New System.Drawing.Point(15, 161)
        Me.cbRotation.Name = "cbRotation"
        Me.cbRotation.Size = New System.Drawing.Size(263, 17)
        Me.cbRotation.TabIndex = 35
        Me.cbRotation.Text = "Detect and fix image rotation 90/180/270 degrees"
        Me.cbRotation.UseVisualStyleBackColor = True
        '
        'cbDeskew
        '
        Me.cbDeskew.AutoSize = True
        Me.cbDeskew.Location = New System.Drawing.Point(15, 138)
        Me.cbDeskew.Name = "cbDeskew"
        Me.cbDeskew.Size = New System.Drawing.Size(151, 17)
        Me.cbDeskew.TabIndex = 34
        Me.cbDeskew.Text = "Detect and fix image skew"
        Me.cbDeskew.UseVisualStyleBackColor = True
        '
        'cbZonesInversion
        '
        Me.cbZonesInversion.AutoSize = True
        Me.cbZonesInversion.Location = New System.Drawing.Point(15, 115)
        Me.cbZonesInversion.Name = "cbZonesInversion"
        Me.cbZonesInversion.Size = New System.Drawing.Size(134, 17)
        Me.cbZonesInversion.TabIndex = 33
        Me.cbZonesInversion.Text = "Detect zones inversion"
        Me.cbZonesInversion.UseVisualStyleBackColor = True
        '
        'cbImgInversion
        '
        Me.cbImgInversion.AutoSize = True
        Me.cbImgInversion.Location = New System.Drawing.Point(15, 92)
        Me.cbImgInversion.Name = "cbImgInversion"
        Me.cbImgInversion.Size = New System.Drawing.Size(134, 17)
        Me.cbImgInversion.TabIndex = 32
        Me.cbImgInversion.Text = "Detect image inversion"
        Me.cbImgInversion.UseVisualStyleBackColor = True
        '
        'cbFindBarcodes
        '
        Me.cbFindBarcodes.AutoSize = True
        Me.cbFindBarcodes.Location = New System.Drawing.Point(15, 69)
        Me.cbFindBarcodes.Name = "cbFindBarcodes"
        Me.cbFindBarcodes.Size = New System.Drawing.Size(93, 17)
        Me.cbFindBarcodes.TabIndex = 31
        Me.cbFindBarcodes.Text = "Find barcodes"
        Me.cbFindBarcodes.UseVisualStyleBackColor = True
        '
        'bkCancel
        '
        Me.bkCancel.Location = New System.Drawing.Point(226, 540)
        Me.bkCancel.Name = "bkCancel"
        Me.bkCancel.Size = New System.Drawing.Size(75, 23)
        Me.bkCancel.TabIndex = 30
        Me.bkCancel.Text = "Cancel"
        Me.bkCancel.UseVisualStyleBackColor = True
        '
        'bkOK
        '
        Me.bkOK.Location = New System.Drawing.Point(145, 540)
        Me.bkOK.Name = "bkOK"
        Me.bkOK.Size = New System.Drawing.Size(75, 23)
        Me.bkOK.TabIndex = 29
        Me.bkOK.Text = "OK"
        Me.bkOK.UseVisualStyleBackColor = True
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.ForeColor = System.Drawing.Color.Red
        Me.label3.Location = New System.Drawing.Point(10, 514)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(420, 13)
        Me.label3.TabIndex = 28
        Me.label3.Text = "NOTE: Some options will not be applied  immediately, reload image to apply all ch" & _
            "anges."
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.ForeColor = System.Drawing.Color.Red
        Me.label2.Location = New System.Drawing.Point(12, 31)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(438, 13)
        Me.label2.TabIndex = 27
        Me.label2.Text = "Check ""Configuration"" and ""Performance Tips"" sections of documentation for more o" & _
            "ptions."
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.ForeColor = System.Drawing.Color.Red
        Me.label1.Location = New System.Drawing.Point(12, 9)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(406, 13)
        Me.label1.TabIndex = 26
        Me.label1.Text = "You can improve OCR performance and accuracy by changing default configuration."
        '
        'cbCorrectMixed
        '
        Me.cbCorrectMixed.AutoSize = True
        Me.cbCorrectMixed.Location = New System.Drawing.Point(15, 299)
        Me.cbCorrectMixed.Name = "cbCorrectMixed"
        Me.cbCorrectMixed.Size = New System.Drawing.Size(269, 17)
        Me.cbCorrectMixed.TabIndex = 52
        Me.cbCorrectMixed.Text = "Correct mixed chars (letters and digits in same word)"
        Me.cbCorrectMixed.UseVisualStyleBackColor = True
        '
        'cbDictionary
        '
        Me.cbDictionary.AutoSize = True
        Me.cbDictionary.Location = New System.Drawing.Point(15, 322)
        Me.cbDictionary.Name = "cbDictionary"
        Me.cbDictionary.Size = New System.Drawing.Size(93, 17)
        Me.cbDictionary.TabIndex = 53
        Me.cbDictionary.Text = "Use dictionary"
        Me.cbDictionary.UseVisualStyleBackColor = True
        '
        'cbOneColumn
        '
        Me.cbOneColumn.AutoSize = True
        Me.cbOneColumn.Location = New System.Drawing.Point(15, 345)
        Me.cbOneColumn.Name = "cbOneColumn"
        Me.cbOneColumn.Size = New System.Drawing.Size(153, 17)
        Me.cbOneColumn.TabIndex = 54
        Me.cbOneColumn.Text = "Combine zones horizontally"
        Me.cbOneColumn.UseVisualStyleBackColor = True
        '
        'Form5
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(477, 575)
        Me.Controls.Add(Me.cbOneColumn)
        Me.Controls.Add(Me.cbDictionary)
        Me.Controls.Add(Me.cbCorrectMixed)
        Me.Controls.Add(Me.bkHelp)
        Me.Controls.Add(Me.edPDFDPI)
        Me.Controls.Add(Me.label8)
        Me.Controls.Add(Me.edTextQual)
        Me.Controls.Add(Me.label6)
        Me.Controls.Add(Me.edBinThreshold)
        Me.Controls.Add(Me.label7)
        Me.Controls.Add(Me.edDisabledChars)
        Me.Controls.Add(Me.label5)
        Me.Controls.Add(Me.edEnabledChars)
        Me.Controls.Add(Me.label4)
        Me.Controls.Add(Me.cbBinTwice)
        Me.Controls.Add(Me.cbFastMode)
        Me.Controls.Add(Me.cbGrayMode)
        Me.Controls.Add(Me.cbRemoveLines)
        Me.Controls.Add(Me.cbImgNoiseFilter)
        Me.Controls.Add(Me.cbRotation)
        Me.Controls.Add(Me.cbDeskew)
        Me.Controls.Add(Me.cbZonesInversion)
        Me.Controls.Add(Me.cbImgInversion)
        Me.Controls.Add(Me.cbFindBarcodes)
        Me.Controls.Add(Me.bkCancel)
        Me.Controls.Add(Me.bkOK)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form5"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "OCR Options"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents bkHelp As System.Windows.Forms.Button
    Private WithEvents edPDFDPI As System.Windows.Forms.TextBox
    Private WithEvents label8 As System.Windows.Forms.Label
    Private WithEvents edTextQual As System.Windows.Forms.TextBox
    Private WithEvents label6 As System.Windows.Forms.Label
    Private WithEvents edBinThreshold As System.Windows.Forms.TextBox
    Private WithEvents label7 As System.Windows.Forms.Label
    Private WithEvents edDisabledChars As System.Windows.Forms.TextBox
    Private WithEvents label5 As System.Windows.Forms.Label
    Private WithEvents edEnabledChars As System.Windows.Forms.TextBox
    Private WithEvents label4 As System.Windows.Forms.Label
    Private WithEvents cbBinTwice As System.Windows.Forms.CheckBox
    Private WithEvents cbFastMode As System.Windows.Forms.CheckBox
    Private WithEvents cbGrayMode As System.Windows.Forms.CheckBox
    Private WithEvents cbRemoveLines As System.Windows.Forms.CheckBox
    Private WithEvents cbImgNoiseFilter As System.Windows.Forms.CheckBox
    Private WithEvents cbRotation As System.Windows.Forms.CheckBox
    Private WithEvents cbDeskew As System.Windows.Forms.CheckBox
    Private WithEvents cbZonesInversion As System.Windows.Forms.CheckBox
    Private WithEvents cbImgInversion As System.Windows.Forms.CheckBox
    Private WithEvents cbFindBarcodes As System.Windows.Forms.CheckBox
    Private WithEvents bkCancel As System.Windows.Forms.Button
    Private WithEvents bkOK As System.Windows.Forms.Button
    Private WithEvents label3 As System.Windows.Forms.Label
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents cbCorrectMixed As System.Windows.Forms.CheckBox
    Private WithEvents cbDictionary As System.Windows.Forms.CheckBox
    Private WithEvents cbOneColumn As System.Windows.Forms.CheckBox
End Class
