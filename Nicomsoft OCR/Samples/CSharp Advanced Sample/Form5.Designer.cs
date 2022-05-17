namespace Sample
{
    partial class Form5
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.bkOK = new System.Windows.Forms.Button();
            this.bkCancel = new System.Windows.Forms.Button();
            this.cbFindBarcodes = new System.Windows.Forms.CheckBox();
            this.cbImgInversion = new System.Windows.Forms.CheckBox();
            this.cbZonesInversion = new System.Windows.Forms.CheckBox();
            this.cbDeskew = new System.Windows.Forms.CheckBox();
            this.cbRotation = new System.Windows.Forms.CheckBox();
            this.cbImgNoiseFilter = new System.Windows.Forms.CheckBox();
            this.cbRemoveLines = new System.Windows.Forms.CheckBox();
            this.cbGrayMode = new System.Windows.Forms.CheckBox();
            this.cbFastMode = new System.Windows.Forms.CheckBox();
            this.cbBinTwice = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.edEnabledChars = new System.Windows.Forms.TextBox();
            this.edDisabledChars = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.edTextQual = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.edBinThreshold = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.edPDFDPI = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.bkHelp = new System.Windows.Forms.Button();
            this.cbCorrectMixed = new System.Windows.Forms.CheckBox();
            this.cbDictionary = new System.Windows.Forms.CheckBox();
            this.cbOneColumn = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(406, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "You can improve OCR performance and accuracy by changing default configuration.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(12, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(438, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Check \"Configuration\" and \"Performance Tips\" sections of documentation for more o" +
                "ptions.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(14, 519);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(420, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "NOTE: Some options will not be applied  immediately, reload image to apply all ch" +
                "anges.";
            // 
            // bkOK
            // 
            this.bkOK.Location = new System.Drawing.Point(149, 545);
            this.bkOK.Name = "bkOK";
            this.bkOK.Size = new System.Drawing.Size(75, 27);
            this.bkOK.TabIndex = 3;
            this.bkOK.Text = "OK";
            this.bkOK.UseVisualStyleBackColor = true;
            this.bkOK.Click += new System.EventHandler(this.bkOK_Click);
            // 
            // bkCancel
            // 
            this.bkCancel.Location = new System.Drawing.Point(230, 545);
            this.bkCancel.Name = "bkCancel";
            this.bkCancel.Size = new System.Drawing.Size(75, 27);
            this.bkCancel.TabIndex = 4;
            this.bkCancel.Text = "Cancel";
            this.bkCancel.UseVisualStyleBackColor = true;
            this.bkCancel.Click += new System.EventHandler(this.bkCancel_Click);
            // 
            // cbFindBarcodes
            // 
            this.cbFindBarcodes.AutoSize = true;
            this.cbFindBarcodes.Location = new System.Drawing.Point(15, 69);
            this.cbFindBarcodes.Name = "cbFindBarcodes";
            this.cbFindBarcodes.Size = new System.Drawing.Size(93, 17);
            this.cbFindBarcodes.TabIndex = 5;
            this.cbFindBarcodes.Text = "Find barcodes";
            this.cbFindBarcodes.UseVisualStyleBackColor = true;
            // 
            // cbImgInversion
            // 
            this.cbImgInversion.AutoSize = true;
            this.cbImgInversion.Location = new System.Drawing.Point(15, 92);
            this.cbImgInversion.Name = "cbImgInversion";
            this.cbImgInversion.Size = new System.Drawing.Size(134, 17);
            this.cbImgInversion.TabIndex = 6;
            this.cbImgInversion.Text = "Detect image inversion";
            this.cbImgInversion.UseVisualStyleBackColor = true;
            // 
            // cbZonesInversion
            // 
            this.cbZonesInversion.AutoSize = true;
            this.cbZonesInversion.Location = new System.Drawing.Point(15, 115);
            this.cbZonesInversion.Name = "cbZonesInversion";
            this.cbZonesInversion.Size = new System.Drawing.Size(134, 17);
            this.cbZonesInversion.TabIndex = 7;
            this.cbZonesInversion.Text = "Detect zones inversion";
            this.cbZonesInversion.UseVisualStyleBackColor = true;
            // 
            // cbDeskew
            // 
            this.cbDeskew.AutoSize = true;
            this.cbDeskew.Location = new System.Drawing.Point(15, 138);
            this.cbDeskew.Name = "cbDeskew";
            this.cbDeskew.Size = new System.Drawing.Size(151, 17);
            this.cbDeskew.TabIndex = 8;
            this.cbDeskew.Text = "Detect and fix image skew";
            this.cbDeskew.UseVisualStyleBackColor = true;
            // 
            // cbRotation
            // 
            this.cbRotation.AutoSize = true;
            this.cbRotation.Location = new System.Drawing.Point(15, 161);
            this.cbRotation.Name = "cbRotation";
            this.cbRotation.Size = new System.Drawing.Size(263, 17);
            this.cbRotation.TabIndex = 9;
            this.cbRotation.Text = "Detect and fix image rotation 90/180/270 degrees";
            this.cbRotation.UseVisualStyleBackColor = true;
            // 
            // cbImgNoiseFilter
            // 
            this.cbImgNoiseFilter.AutoSize = true;
            this.cbImgNoiseFilter.Location = new System.Drawing.Point(15, 184);
            this.cbImgNoiseFilter.Name = "cbImgNoiseFilter";
            this.cbImgNoiseFilter.Size = new System.Drawing.Size(148, 17);
            this.cbImgNoiseFilter.TabIndex = 10;
            this.cbImgNoiseFilter.Text = "Apply noise filter for image";
            this.cbImgNoiseFilter.UseVisualStyleBackColor = true;
            // 
            // cbRemoveLines
            // 
            this.cbRemoveLines.AutoSize = true;
            this.cbRemoveLines.Location = new System.Drawing.Point(15, 207);
            this.cbRemoveLines.Name = "cbRemoveLines";
            this.cbRemoveLines.Size = new System.Drawing.Size(141, 17);
            this.cbRemoveLines.TabIndex = 11;
            this.cbRemoveLines.Text = "Detect and remove lines";
            this.cbRemoveLines.UseVisualStyleBackColor = true;
            // 
            // cbGrayMode
            // 
            this.cbGrayMode.AutoSize = true;
            this.cbGrayMode.Location = new System.Drawing.Point(15, 230);
            this.cbGrayMode.Name = "cbGrayMode";
            this.cbGrayMode.Size = new System.Drawing.Size(77, 17);
            this.cbGrayMode.TabIndex = 12;
            this.cbGrayMode.Text = "Gray mode";
            this.cbGrayMode.UseVisualStyleBackColor = true;
            // 
            // cbFastMode
            // 
            this.cbFastMode.AutoSize = true;
            this.cbFastMode.Location = new System.Drawing.Point(15, 253);
            this.cbFastMode.Name = "cbFastMode";
            this.cbFastMode.Size = new System.Drawing.Size(147, 17);
            this.cbFastMode.TabIndex = 13;
            this.cbFastMode.Text = "Fast mode (less accurate)";
            this.cbFastMode.UseVisualStyleBackColor = true;
            // 
            // cbBinTwice
            // 
            this.cbBinTwice.AutoSize = true;
            this.cbBinTwice.Location = new System.Drawing.Point(15, 276);
            this.cbBinTwice.Name = "cbBinTwice";
            this.cbBinTwice.Size = new System.Drawing.Size(91, 17);
            this.cbBinTwice.TabIndex = 14;
            this.cbBinTwice.Text = "Binarize twice";
            this.cbBinTwice.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 377);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Enabled chars:";
            // 
            // edEnabledChars
            // 
            this.edEnabledChars.Location = new System.Drawing.Point(98, 370);
            this.edEnabledChars.Name = "edEnabledChars";
            this.edEnabledChars.Size = new System.Drawing.Size(320, 20);
            this.edEnabledChars.TabIndex = 16;
            // 
            // edDisabledChars
            // 
            this.edDisabledChars.Location = new System.Drawing.Point(98, 396);
            this.edDisabledChars.Name = "edDisabledChars";
            this.edDisabledChars.Size = new System.Drawing.Size(320, 20);
            this.edDisabledChars.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 403);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Disabled chars:";
            // 
            // edTextQual
            // 
            this.edTextQual.Location = new System.Drawing.Point(223, 452);
            this.edTextQual.Name = "edTextQual";
            this.edTextQual.Size = new System.Drawing.Size(53, 20);
            this.edTextQual.TabIndex = 22;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 455);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(148, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Text quality (0..100; -1 - auto):";
            // 
            // edBinThreshold
            // 
            this.edBinThreshold.Location = new System.Drawing.Point(223, 426);
            this.edBinThreshold.Name = "edBinThreshold";
            this.edBinThreshold.Size = new System.Drawing.Size(53, 20);
            this.edBinThreshold.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 429);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(203, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Binarization threshold (0..254; 255 - auto):";
            // 
            // edPDFDPI
            // 
            this.edPDFDPI.Location = new System.Drawing.Point(223, 478);
            this.edPDFDPI.Name = "edPDFDPI";
            this.edPDFDPI.Size = new System.Drawing.Size(53, 20);
            this.edPDFDPI.TabIndex = 24;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 481);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(99, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "PDF rendering DPI:";
            // 
            // bkHelp
            // 
            this.bkHelp.Location = new System.Drawing.Point(394, 545);
            this.bkHelp.Name = "bkHelp";
            this.bkHelp.Size = new System.Drawing.Size(75, 27);
            this.bkHelp.TabIndex = 25;
            this.bkHelp.Text = "Help";
            this.bkHelp.UseVisualStyleBackColor = true;
            this.bkHelp.Click += new System.EventHandler(this.bkHelp_Click);
            // 
            // cbCorrectMixed
            // 
            this.cbCorrectMixed.AutoSize = true;
            this.cbCorrectMixed.Location = new System.Drawing.Point(15, 299);
            this.cbCorrectMixed.Name = "cbCorrectMixed";
            this.cbCorrectMixed.Size = new System.Drawing.Size(269, 17);
            this.cbCorrectMixed.TabIndex = 26;
            this.cbCorrectMixed.Text = "Correct mixed chars (letters and digits in same word)";
            this.cbCorrectMixed.UseVisualStyleBackColor = true;
            // 
            // cbDictionary
            // 
            this.cbDictionary.AutoSize = true;
            this.cbDictionary.Location = new System.Drawing.Point(15, 322);
            this.cbDictionary.Name = "cbDictionary";
            this.cbDictionary.Size = new System.Drawing.Size(93, 17);
            this.cbDictionary.TabIndex = 27;
            this.cbDictionary.Text = "Use dictionary";
            this.cbDictionary.UseVisualStyleBackColor = true;
            // 
            // cbOneColumn
            // 
            this.cbOneColumn.AutoSize = true;
            this.cbOneColumn.Location = new System.Drawing.Point(15, 345);
            this.cbOneColumn.Name = "cbOneColumn";
            this.cbOneColumn.Size = new System.Drawing.Size(153, 17);
            this.cbOneColumn.TabIndex = 28;
            this.cbOneColumn.Text = "Combine zones horizontally";
            this.cbOneColumn.UseVisualStyleBackColor = true;
            // 
            // Form5
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 584);
            this.Controls.Add(this.cbOneColumn);
            this.Controls.Add(this.cbDictionary);
            this.Controls.Add(this.cbCorrectMixed);
            this.Controls.Add(this.bkHelp);
            this.Controls.Add(this.edPDFDPI);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.edTextQual);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.edBinThreshold);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.edDisabledChars);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.edEnabledChars);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbBinTwice);
            this.Controls.Add(this.cbFastMode);
            this.Controls.Add(this.cbGrayMode);
            this.Controls.Add(this.cbRemoveLines);
            this.Controls.Add(this.cbImgNoiseFilter);
            this.Controls.Add(this.cbRotation);
            this.Controls.Add(this.cbDeskew);
            this.Controls.Add(this.cbZonesInversion);
            this.Controls.Add(this.cbImgInversion);
            this.Controls.Add(this.cbFindBarcodes);
            this.Controls.Add(this.bkCancel);
            this.Controls.Add(this.bkOK);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form5";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OCR Options";
            this.Load += new System.EventHandler(this.Form5_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bkOK;
        private System.Windows.Forms.Button bkCancel;
        private System.Windows.Forms.CheckBox cbFindBarcodes;
        private System.Windows.Forms.CheckBox cbImgInversion;
        private System.Windows.Forms.CheckBox cbZonesInversion;
        private System.Windows.Forms.CheckBox cbDeskew;
        private System.Windows.Forms.CheckBox cbRotation;
        private System.Windows.Forms.CheckBox cbImgNoiseFilter;
        private System.Windows.Forms.CheckBox cbRemoveLines;
        private System.Windows.Forms.CheckBox cbGrayMode;
        private System.Windows.Forms.CheckBox cbFastMode;
        private System.Windows.Forms.CheckBox cbBinTwice;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox edEnabledChars;
        private System.Windows.Forms.TextBox edDisabledChars;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox edTextQual;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox edBinThreshold;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox edPDFDPI;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button bkHelp;
        private System.Windows.Forms.CheckBox cbCorrectMixed;
        private System.Windows.Forms.CheckBox cbDictionary;
        private System.Windows.Forms.CheckBox cbOneColumn;
    }
}