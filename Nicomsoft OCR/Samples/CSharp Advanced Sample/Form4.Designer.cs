namespace Sample
{
    partial class fmLanguages
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
            this.bkOK = new System.Windows.Forms.Button();
            this.cbLanguages = new System.Windows.Forms.CheckedListBox();
            this.bkCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbLanguagesAsian = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bkOK
            // 
            this.bkOK.Location = new System.Drawing.Point(118, 487);
            this.bkOK.Name = "bkOK";
            this.bkOK.Size = new System.Drawing.Size(75, 23);
            this.bkOK.TabIndex = 0;
            this.bkOK.Text = "OK";
            this.bkOK.UseVisualStyleBackColor = true;
            this.bkOK.Click += new System.EventHandler(this.bkOK_Click);
            // 
            // cbLanguages
            // 
            this.cbLanguages.CheckOnClick = true;
            this.cbLanguages.FormattingEnabled = true;
            this.cbLanguages.Items.AddRange(new object[] {
            "Bulgarian",
            "Catalan",
            "Croatian",
            "Czech",
            "Danish",
            "Dutch",
            "English",
            "Estonian",
            "Finnish",
            "French",
            "German",
            "Hungarian",
            "Indonesian",
            "Italian",
            "Latvian",
            "Lithuanian",
            "Norwegian",
            "Polish",
            "Portuguese",
            "Romanian",
            "Russian",
            "Slovak",
            "Slovenian",
            "Spanish",
            "Swedish",
            "Turkish"});
            this.cbLanguages.Location = new System.Drawing.Point(12, 25);
            this.cbLanguages.Name = "cbLanguages";
            this.cbLanguages.Size = new System.Drawing.Size(153, 409);
            this.cbLanguages.Sorted = true;
            this.cbLanguages.TabIndex = 1;
            // 
            // bkCancel
            // 
            this.bkCancel.Location = new System.Drawing.Point(199, 487);
            this.bkCancel.Name = "bkCancel";
            this.bkCancel.Size = new System.Drawing.Size(75, 23);
            this.bkCancel.TabIndex = 2;
            this.bkCancel.Text = "Cancel";
            this.bkCancel.UseVisualStyleBackColor = true;
            this.bkCancel.Click += new System.EventHandler(this.bkCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Main module languages:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(238, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Asian module languages:";
            // 
            // cbLanguagesAsian
            // 
            this.cbLanguagesAsian.CheckOnClick = true;
            this.cbLanguagesAsian.FormattingEnabled = true;
            this.cbLanguagesAsian.Items.AddRange(new object[] {
            "Arabic",
            "Chinese_Simplified",
            "Chinese_Traditional",
            "Japanese",
            "Korean"});
            this.cbLanguagesAsian.Location = new System.Drawing.Point(241, 25);
            this.cbLanguagesAsian.Name = "cbLanguagesAsian";
            this.cbLanguagesAsian.Size = new System.Drawing.Size(153, 139);
            this.cbLanguagesAsian.Sorted = true;
            this.cbLanguagesAsian.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 447);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(359, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "NOTE: Using both main and asian languages in one zone is not supported.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 463);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(394, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "But you can use both main and asian languages on same image in different zones.";
            // 
            // fmLanguages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 517);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbLanguagesAsian);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bkCancel);
            this.Controls.Add(this.cbLanguages);
            this.Controls.Add(this.bkOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmLanguages";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Languages";
            this.Load += new System.EventHandler(this.fmLanguages_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bkOK;
        private System.Windows.Forms.CheckedListBox cbLanguages;
        private System.Windows.Forms.Button bkCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox cbLanguagesAsian;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}