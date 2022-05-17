namespace Sample
{
    partial class Form2
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
            this.bkScan = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.cbNoUI = new System.Windows.Forms.CheckBox();
            this.cbScanners = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbSrc = new System.Windows.Forms.ComboBox();
            this.bkSetDefault = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bkScan
            // 
            this.bkScan.Location = new System.Drawing.Point(174, 236);
            this.bkScan.Name = "bkScan";
            this.bkScan.Size = new System.Drawing.Size(75, 23);
            this.bkScan.TabIndex = 0;
            this.bkScan.Text = "Scan";
            this.bkScan.UseVisualStyleBackColor = true;
            this.bkScan.Click += new System.EventHandler(this.bkScan_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(255, 236);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.bkCancel_Click);
            // 
            // cbNoUI
            // 
            this.cbNoUI.AutoSize = true;
            this.cbNoUI.Location = new System.Drawing.Point(15, 138);
            this.cbNoUI.Name = "cbNoUI";
            this.cbNoUI.Size = new System.Drawing.Size(97, 17);
            this.cbNoUI.TabIndex = 2;
            this.cbNoUI.Text = "No Scanner UI";
            this.cbNoUI.UseVisualStyleBackColor = true;
            // 
            // cbScanners
            // 
            this.cbScanners.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbScanners.FormattingEnabled = true;
            this.cbScanners.Location = new System.Drawing.Point(114, 18);
            this.cbScanners.Name = "cbScanners";
            this.cbScanners.Size = new System.Drawing.Size(378, 21);
            this.cbScanners.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Select Device:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Document Source:";
            // 
            // cbSrc
            // 
            this.cbSrc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSrc.FormattingEnabled = true;
            this.cbSrc.Items.AddRange(new object[] {
            "Flatbed",
            "ADF(Automatic Document Feeder)",
            "Auto"});
            this.cbSrc.Location = new System.Drawing.Point(114, 74);
            this.cbSrc.Name = "cbSrc";
            this.cbSrc.Size = new System.Drawing.Size(203, 21);
            this.cbSrc.TabIndex = 5;
            // 
            // bkSetDefault
            // 
            this.bkSetDefault.Location = new System.Drawing.Point(396, 49);
            this.bkSetDefault.Name = "bkSetDefault";
            this.bkSetDefault.Size = new System.Drawing.Size(96, 23);
            this.bkSetDefault.TabIndex = 7;
            this.bkSetDefault.Text = "Set as Default";
            this.bkSetDefault.UseVisualStyleBackColor = true;
            this.bkSetDefault.Click += new System.EventHandler(this.bkSetDefault_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 268);
            this.Controls.Add(this.bkSetDefault);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbSrc);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbScanners);
            this.Controls.Add(this.cbNoUI);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.bkScan);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scanning";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bkScan;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox cbNoUI;
        private System.Windows.Forms.ComboBox cbScanners;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbSrc;
        private System.Windows.Forms.Button bkSetDefault;
    }
}