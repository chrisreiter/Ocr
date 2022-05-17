namespace Sample
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbSize = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbSkew = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbLines = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.PicBox = new System.Windows.Forms.PictureBox();
            this.tbText = new System.Windows.Forms.TextBox();
            this.bkFile = new System.Windows.Forms.Button();
            this.bkRecognize = new System.Windows.Forms.Button();
            this.lbWait = new System.Windows.Forms.Label();
            this.opImg = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.edPage = new System.Windows.Forms.TextBox();
            this.lbPages = new System.Windows.Forms.Label();
            this.bkSetPage = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cbScale = new System.Windows.Forms.ComboBox();
            this.bkSave = new System.Windows.Forms.Button();
            this.svFile = new System.Windows.Forms.SaveFileDialog();
            this.bkSelectLanguages = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.statusStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbSize,
            this.lbSkew,
            this.lbLines});
            this.statusStrip1.Location = new System.Drawing.Point(0, 466);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(845, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbSize
            // 
            this.lbSize.Name = "lbSize";
            this.lbSize.Size = new System.Drawing.Size(0, 17);
            // 
            // lbSkew
            // 
            this.lbSkew.Name = "lbSkew";
            this.lbSkew.Size = new System.Drawing.Size(0, 17);
            // 
            // lbLines
            // 
            this.lbLines.Name = "lbLines";
            this.lbLines.Size = new System.Drawing.Size(0, 17);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 68);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.PicBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tbText);
            this.splitContainer1.Size = new System.Drawing.Size(845, 395);
            this.splitContainer1.SplitterDistance = 407;
            this.splitContainer1.TabIndex = 8;
            // 
            // PicBox
            // 
            this.PicBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.PicBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PicBox.Location = new System.Drawing.Point(4, 3);
            this.PicBox.Name = "PicBox";
            this.PicBox.Size = new System.Drawing.Size(400, 420);
            this.PicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.PicBox.TabIndex = 8;
            this.PicBox.TabStop = false;
            this.PicBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PicBoxMouseMove);
            this.PicBox.Resize += new System.EventHandler(this.PicBoxResized);
            this.PicBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PicBoxOnMouseDown);
            this.PicBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PicBoxOnMouseUp);
            // 
            // tbText
            // 
            this.tbText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbText.Location = new System.Drawing.Point(3, 3);
            this.tbText.Multiline = true;
            this.tbText.Name = "tbText";
            this.tbText.ReadOnly = true;
            this.tbText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbText.Size = new System.Drawing.Size(431, 392);
            this.tbText.TabIndex = 1;
            this.tbText.WordWrap = false;
            // 
            // bkFile
            // 
            this.bkFile.Location = new System.Drawing.Point(4, 11);
            this.bkFile.Name = "bkFile";
            this.bkFile.Size = new System.Drawing.Size(75, 23);
            this.bkFile.TabIndex = 9;
            this.bkFile.Text = "Open File";
            this.bkFile.UseVisualStyleBackColor = true;
            this.bkFile.Click += new System.EventHandler(this.bkFile_Click);
            // 
            // bkRecognize
            // 
            this.bkRecognize.Location = new System.Drawing.Point(85, 11);
            this.bkRecognize.Name = "bkRecognize";
            this.bkRecognize.Size = new System.Drawing.Size(75, 23);
            this.bkRecognize.TabIndex = 10;
            this.bkRecognize.Text = "Recognize";
            this.bkRecognize.UseVisualStyleBackColor = true;
            this.bkRecognize.Click += new System.EventHandler(this.bkRecognize_Click);
            // 
            // lbWait
            // 
            this.lbWait.AutoSize = true;
            this.lbWait.Location = new System.Drawing.Point(262, 16);
            this.lbWait.Name = "lbWait";
            this.lbWait.Size = new System.Drawing.Size(70, 13);
            this.lbWait.TabIndex = 11;
            this.lbWait.Text = "Please wait...";
            this.lbWait.Visible = false;
            // 
            // opImg
            // 
            this.opImg.DefaultExt = "pdf";
            this.opImg.Filter = "Image Files (bmp, jpg, tif, png, gif, pdf)|*.bmp;*.jpg;*.tif;*.png;*.gif;*.pdf|Al" +
                "l Files|*";
            this.opImg.Title = "Open Image File";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(363, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Page";
            // 
            // edPage
            // 
            this.edPage.Location = new System.Drawing.Point(401, 11);
            this.edPage.Name = "edPage";
            this.edPage.Size = new System.Drawing.Size(28, 20);
            this.edPage.TabIndex = 13;
            this.edPage.Text = "1";
            // 
            // lbPages
            // 
            this.lbPages.AutoSize = true;
            this.lbPages.Location = new System.Drawing.Point(435, 16);
            this.lbPages.Name = "lbPages";
            this.lbPages.Size = new System.Drawing.Size(25, 13);
            this.lbPages.TabIndex = 14;
            this.lbPages.Text = "of 1";
            // 
            // bkSetPage
            // 
            this.bkSetPage.Location = new System.Drawing.Point(466, 11);
            this.bkSetPage.Name = "bkSetPage";
            this.bkSetPage.Size = new System.Drawing.Size(41, 21);
            this.bkSetPage.TabIndex = 15;
            this.bkSetPage.Text = "Set";
            this.bkSetPage.UseVisualStyleBackColor = true;
            this.bkSetPage.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Scale:";
            // 
            // cbScale
            // 
            this.cbScale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbScale.FormattingEnabled = true;
            this.cbScale.Items.AddRange(new object[] {
            "Auto",
            "0.25",
            "0.5",
            "1.0",
            "1.5",
            "2.0",
            "2.5",
            "4.0"});
            this.cbScale.Location = new System.Drawing.Point(43, 41);
            this.cbScale.Name = "cbScale";
            this.cbScale.Size = new System.Drawing.Size(133, 21);
            this.cbScale.TabIndex = 17;
            this.cbScale.SelectedIndexChanged += new System.EventHandler(this.cbScale_SelectedIndexChanged);
            // 
            // bkSave
            // 
            this.bkSave.Location = new System.Drawing.Point(167, 11);
            this.bkSave.Name = "bkSave";
            this.bkSave.Size = new System.Drawing.Size(75, 23);
            this.bkSave.TabIndex = 27;
            this.bkSave.Text = "Save";
            this.bkSave.UseVisualStyleBackColor = true;
            this.bkSave.Click += new System.EventHandler(this.bkSave_Click);
            // 
            // svFile
            // 
            this.svFile.DefaultExt = "pdf";
            this.svFile.Filter = "PDF document(*.pdf)|*.pdf|RTF document (*.rtf)|*.rtf|ASCII Text document (*.txt)|" +
                "*.txt|Unicode Text document (*.txt)|*.txt|XML document (*.xml)|*.xml";
            // 
            // bkSelectLanguages
            // 
            this.bkSelectLanguages.Location = new System.Drawing.Point(41, 20);
            this.bkSelectLanguages.Name = "bkSelectLanguages";
            this.bkSelectLanguages.Size = new System.Drawing.Size(113, 23);
            this.bkSelectLanguages.TabIndex = 0;
            this.bkSelectLanguages.Text = "Select Languages";
            this.bkSelectLanguages.UseVisualStyleBackColor = true;
            this.bkSelectLanguages.Click += new System.EventHandler(this.bkSelectLanguages_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.bkSelectLanguages);
            this.groupBox1.Location = new System.Drawing.Point(546, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 53);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Languages";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 488);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.bkSave);
            this.Controls.Add(this.cbScale);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bkSetPage);
            this.Controls.Add(this.lbPages);
            this.Controls.Add(this.edPage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbWait);
            this.Controls.Add(this.bkRecognize);
            this.Controls.Add(this.bkFile);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Form1";
            this.Text = "Nicomsoft OCR C# Invoke Sample";
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Resize += new System.EventHandler(this.PicBoxResized);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox PicBox;
        private System.Windows.Forms.TextBox tbText;
        private System.Windows.Forms.Button bkFile;
        private System.Windows.Forms.Button bkRecognize;
        private System.Windows.Forms.Label lbWait;
        private System.Windows.Forms.OpenFileDialog opImg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox edPage;
        private System.Windows.Forms.Label lbPages;
        private System.Windows.Forms.Button bkSetPage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbScale;
        private System.Windows.Forms.ToolStripStatusLabel lbSize;
        private System.Windows.Forms.ToolStripStatusLabel lbSkew;
        private System.Windows.Forms.ToolStripStatusLabel lbLines;
        private System.Windows.Forms.Button bkSave;
        private System.Windows.Forms.SaveFileDialog svFile;
        private System.Windows.Forms.Button bkSelectLanguages;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

