﻿namespace Sample
{
    partial class Form1
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
            this.bkStart = new System.Windows.Forms.Button();
            this.tbText = new System.Windows.Forms.TextBox();
            this.edFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bkStart
            // 
            this.bkStart.Location = new System.Drawing.Point(12, 48);
            this.bkStart.Name = "bkStart";
            this.bkStart.Size = new System.Drawing.Size(75, 23);
            this.bkStart.TabIndex = 0;
            this.bkStart.Text = "Start";
            this.bkStart.UseVisualStyleBackColor = true;
            this.bkStart.Click += new System.EventHandler(this.bkStart_Click);
            // 
            // tbText
            // 
            this.tbText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbText.Location = new System.Drawing.Point(0, 77);
            this.tbText.Multiline = true;
            this.tbText.Name = "tbText";
            this.tbText.ReadOnly = true;
            this.tbText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbText.Size = new System.Drawing.Size(673, 295);
            this.tbText.TabIndex = 2;
            this.tbText.WordWrap = false;
            // 
            // edFile
            // 
            this.edFile.Location = new System.Drawing.Point(107, 48);
            this.edFile.Name = "edFile";
            this.edFile.Size = new System.Drawing.Size(553, 20);
            this.edFile.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(316, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "This sample demonstrates how to process several images at once";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 371);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.edFile);
            this.Controls.Add(this.tbText);
            this.Controls.Add(this.bkStart);
            this.Name = "Form1";
            this.Text = "Nicomsoft OCR C# Multithreading Sample";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bkStart;
        private System.Windows.Forms.TextBox tbText;
        private System.Windows.Forms.TextBox edFile;
        private System.Windows.Forms.Label label1;
    }
}
