<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.components = New System.ComponentModel.Container
        Me.lbWait = New System.Windows.Forms.Label
        Me.bkRecognize = New System.Windows.Forms.Button
        Me.bkFile = New System.Windows.Forms.Button
        Me.statusStrip1 = New System.Windows.Forms.StatusStrip
        Me.splitContainer1 = New System.Windows.Forms.SplitContainer
        Me.cbPixLines = New System.Windows.Forms.CheckBox
        Me.bkDetectBlocks = New System.Windows.Forms.Button
        Me.bkClearBlocks = New System.Windows.Forms.Button
        Me.bkSaveBlocks = New System.Windows.Forms.Button
        Me.bkLoadBlocks = New System.Windows.Forms.Button
        Me.cbBin = New System.Windows.Forms.CheckBox
        Me.PicBox = New System.Windows.Forms.PictureBox
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.tbText = New System.Windows.Forms.TextBox
        Me.opImg = New System.Windows.Forms.OpenFileDialog
        Me.Label1 = New System.Windows.Forms.Label
        Me.edPage = New System.Windows.Forms.TextBox
        Me.lbPages = New System.Windows.Forms.Label
        Me.bkSetPage = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.cbScale = New System.Windows.Forms.ComboBox
        Me.cbExactCopy = New System.Windows.Forms.CheckBox
        Me.pmBlock = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.nnTypeOCRText = New System.Windows.Forms.ToolStripMenuItem
        Me.nnTypeOCRDigit = New System.Windows.Forms.ToolStripMenuItem
        Me.nnTypeICRDigit = New System.Windows.Forms.ToolStripMenuItem
        Me.nnTypeBarCode = New System.Windows.Forms.ToolStripMenuItem
        Me.nnTypeTable = New System.Windows.Forms.ToolStripMenuItem
        Me.nnTypePicture = New System.Windows.Forms.ToolStripMenuItem
        Me.nnTypeClear = New System.Windows.Forms.ToolStripMenuItem
        Me.nnTypeZoning = New System.Windows.Forms.ToolStripMenuItem
        Me.nnTypeMRZ = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        Me.nnInvert = New System.Windows.Forms.ToolStripMenuItem
        Me.nnRotate90 = New System.Windows.Forms.ToolStripMenuItem
        Me.nnRotate270 = New System.Windows.Forms.ToolStripMenuItem
        Me.nnRotate180 = New System.Windows.Forms.ToolStripMenuItem
        Me.nnMirrorH = New System.Windows.Forms.ToolStripMenuItem
        Me.nnMirrorV = New System.Windows.Forms.ToolStripMenuItem
        Me.toolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator
        Me.nnSetRegExp = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator
        Me.nnDeleteBlock = New System.Windows.Forms.ToolStripMenuItem
        Me.opBlocks = New System.Windows.Forms.OpenFileDialog
        Me.svBlocks = New System.Windows.Forms.SaveFileDialog
        Me.bkSave = New System.Windows.Forms.Button
        Me.svFile = New System.Windows.Forms.SaveFileDialog
        Me.bkScan = New System.Windows.Forms.Button
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.bkSelectLanguages = New System.Windows.Forms.Button
        Me.bkOptions = New System.Windows.Forms.Button
        Me.bkHelp = New System.Windows.Forms.Button
        Me.cbCharRects = New System.Windows.Forms.CheckBox
        Me.splitContainer1.Panel1.SuspendLayout()
        Me.splitContainer1.Panel2.SuspendLayout()
        Me.splitContainer1.SuspendLayout()
        CType(Me.PicBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pmBlock.SuspendLayout()
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lbWait
        '
        Me.lbWait.AutoSize = True
        Me.lbWait.Location = New System.Drawing.Point(259, 43)
        Me.lbWait.Name = "lbWait"
        Me.lbWait.Size = New System.Drawing.Size(70, 13)
        Me.lbWait.TabIndex = 16
        Me.lbWait.Text = "Please wait..."
        Me.lbWait.Visible = False
        '
        'bkRecognize
        '
        Me.bkRecognize.Location = New System.Drawing.Point(173, 5)
        Me.bkRecognize.Name = "bkRecognize"
        Me.bkRecognize.Size = New System.Drawing.Size(75, 23)
        Me.bkRecognize.TabIndex = 15
        Me.bkRecognize.Text = "Recognize"
        Me.bkRecognize.UseVisualStyleBackColor = True
        '
        'bkFile
        '
        Me.bkFile.Location = New System.Drawing.Point(92, 5)
        Me.bkFile.Name = "bkFile"
        Me.bkFile.Size = New System.Drawing.Size(75, 23)
        Me.bkFile.TabIndex = 14
        Me.bkFile.Text = "Open File"
        Me.bkFile.UseVisualStyleBackColor = True
        '
        'statusStrip1
        '
        Me.statusStrip1.Location = New System.Drawing.Point(0, 426)
        Me.statusStrip1.Name = "statusStrip1"
        Me.statusStrip1.Size = New System.Drawing.Size(1006, 22)
        Me.statusStrip1.TabIndex = 12
        Me.statusStrip1.Text = "statusStrip1"
        '
        'splitContainer1
        '
        Me.splitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.splitContainer1.Location = New System.Drawing.Point(0, 65)
        Me.splitContainer1.Name = "splitContainer1"
        '
        'splitContainer1.Panel1
        '
        Me.splitContainer1.Panel1.Controls.Add(Me.cbCharRects)
        Me.splitContainer1.Panel1.Controls.Add(Me.cbPixLines)
        Me.splitContainer1.Panel1.Controls.Add(Me.bkDetectBlocks)
        Me.splitContainer1.Panel1.Controls.Add(Me.bkClearBlocks)
        Me.splitContainer1.Panel1.Controls.Add(Me.bkSaveBlocks)
        Me.splitContainer1.Panel1.Controls.Add(Me.bkLoadBlocks)
        Me.splitContainer1.Panel1.Controls.Add(Me.cbBin)
        Me.splitContainer1.Panel1.Controls.Add(Me.PicBox)
        '
        'splitContainer1.Panel2
        '
        Me.splitContainer1.Panel2.Controls.Add(Me.PictureBox1)
        Me.splitContainer1.Panel2.Controls.Add(Me.tbText)
        Me.splitContainer1.Size = New System.Drawing.Size(1006, 358)
        Me.splitContainer1.SplitterDistance = 483
        Me.splitContainer1.TabIndex = 13
        '
        'cbPixLines
        '
        Me.cbPixLines.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cbPixLines.AutoSize = True
        Me.cbPixLines.Location = New System.Drawing.Point(6, 326)
        Me.cbPixLines.Name = "cbPixLines"
        Me.cbPixLines.Size = New System.Drawing.Size(108, 17)
        Me.cbPixLines.TabIndex = 34
        Me.cbPixLines.Text = "Show image lines"
        Me.cbPixLines.UseVisualStyleBackColor = True
        '
        'bkDetectBlocks
        '
        Me.bkDetectBlocks.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bkDetectBlocks.Enabled = False
        Me.bkDetectBlocks.Location = New System.Drawing.Point(391, 329)
        Me.bkDetectBlocks.Name = "bkDetectBlocks"
        Me.bkDetectBlocks.Size = New System.Drawing.Size(89, 23)
        Me.bkDetectBlocks.TabIndex = 33
        Me.bkDetectBlocks.Text = "Detect Zones"
        Me.bkDetectBlocks.UseVisualStyleBackColor = True
        '
        'bkClearBlocks
        '
        Me.bkClearBlocks.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bkClearBlocks.Enabled = False
        Me.bkClearBlocks.Location = New System.Drawing.Point(310, 329)
        Me.bkClearBlocks.Name = "bkClearBlocks"
        Me.bkClearBlocks.Size = New System.Drawing.Size(75, 23)
        Me.bkClearBlocks.TabIndex = 32
        Me.bkClearBlocks.Text = "Clear Zones"
        Me.bkClearBlocks.UseVisualStyleBackColor = True
        '
        'bkSaveBlocks
        '
        Me.bkSaveBlocks.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bkSaveBlocks.Enabled = False
        Me.bkSaveBlocks.Location = New System.Drawing.Point(229, 329)
        Me.bkSaveBlocks.Name = "bkSaveBlocks"
        Me.bkSaveBlocks.Size = New System.Drawing.Size(75, 23)
        Me.bkSaveBlocks.TabIndex = 31
        Me.bkSaveBlocks.Text = "Save Zones"
        Me.bkSaveBlocks.UseVisualStyleBackColor = True
        '
        'bkLoadBlocks
        '
        Me.bkLoadBlocks.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bkLoadBlocks.Enabled = False
        Me.bkLoadBlocks.Location = New System.Drawing.Point(148, 329)
        Me.bkLoadBlocks.Name = "bkLoadBlocks"
        Me.bkLoadBlocks.Size = New System.Drawing.Size(75, 23)
        Me.bkLoadBlocks.TabIndex = 30
        Me.bkLoadBlocks.Text = "Load Zones"
        Me.bkLoadBlocks.UseVisualStyleBackColor = True
        '
        'cbBin
        '
        Me.cbBin.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cbBin.AutoSize = True
        Me.cbBin.Location = New System.Drawing.Point(6, 311)
        Me.cbBin.Name = "cbBin"
        Me.cbBin.Size = New System.Drawing.Size(136, 17)
        Me.cbBin.TabIndex = 29
        Me.cbBin.Text = "Display binarized image"
        Me.cbBin.UseVisualStyleBackColor = True
        '
        'PicBox
        '
        Me.PicBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PicBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PicBox.Location = New System.Drawing.Point(6, 3)
        Me.PicBox.Name = "PicBox"
        Me.PicBox.Size = New System.Drawing.Size(499, 305)
        Me.PicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PicBox.TabIndex = 8
        Me.PicBox.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(376, 268)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(100, 50)
        Me.PictureBox1.TabIndex = 35
        Me.PictureBox1.TabStop = False
        Me.PictureBox1.Visible = False
        '
        'tbText
        '
        Me.tbText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbText.Location = New System.Drawing.Point(3, 3)
        Me.tbText.Multiline = True
        Me.tbText.Name = "tbText"
        Me.tbText.ReadOnly = True
        Me.tbText.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbText.Size = New System.Drawing.Size(516, 355)
        Me.tbText.TabIndex = 1
        Me.tbText.WordWrap = False
        '
        'opImg
        '
        Me.opImg.DefaultExt = "pdf"
        Me.opImg.Filter = "Image Files (bmp, jpg, tif, png, gif, pdf)|*.bmp;*.jpg;*.tif;*.png;*.gif;*.pdf|Al" & _
            "l Files|*"
        Me.opImg.Title = "Open Image File"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(405, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 13)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Page"
        '
        'edPage
        '
        Me.edPage.Location = New System.Drawing.Point(443, 6)
        Me.edPage.Name = "edPage"
        Me.edPage.Size = New System.Drawing.Size(33, 20)
        Me.edPage.TabIndex = 18
        Me.edPage.Text = "1"
        '
        'lbPages
        '
        Me.lbPages.AutoSize = True
        Me.lbPages.Location = New System.Drawing.Point(482, 9)
        Me.lbPages.Name = "lbPages"
        Me.lbPages.Size = New System.Drawing.Size(25, 13)
        Me.lbPages.TabIndex = 19
        Me.lbPages.Text = "of 1"
        '
        'bkSetPage
        '
        Me.bkSetPage.Location = New System.Drawing.Point(520, 4)
        Me.bkSetPage.Name = "bkSetPage"
        Me.bkSetPage.Size = New System.Drawing.Size(43, 21)
        Me.bkSetPage.TabIndex = 20
        Me.bkSetPage.Text = "Set"
        Me.bkSetPage.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 44)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(37, 13)
        Me.Label2.TabIndex = 21
        Me.Label2.Text = "Scale:"
        '
        'cbScale
        '
        Me.cbScale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbScale.FormattingEnabled = True
        Me.cbScale.Items.AddRange(New Object() {"Auto", "0.25", "0.5", "1.0", "1.5", "2.0", "2.5", "4.0"})
        Me.cbScale.Location = New System.Drawing.Point(43, 37)
        Me.cbScale.Name = "cbScale"
        Me.cbScale.Size = New System.Drawing.Size(130, 21)
        Me.cbScale.TabIndex = 22
        '
        'cbExactCopy
        '
        Me.cbExactCopy.AutoSize = True
        Me.cbExactCopy.Location = New System.Drawing.Point(804, 45)
        Me.cbExactCopy.Name = "cbExactCopy"
        Me.cbExactCopy.Size = New System.Drawing.Size(170, 17)
        Me.cbExactCopy.TabIndex = 28
        Me.cbExactCopy.Text = "Exact copy (do not format text)"
        Me.cbExactCopy.UseVisualStyleBackColor = True
        '
        'pmBlock
        '
        Me.pmBlock.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.nnTypeOCRText, Me.nnTypeOCRDigit, Me.nnTypeICRDigit, Me.nnTypeBarCode, Me.nnTypeTable, Me.nnTypePicture, Me.nnTypeClear, Me.nnTypeZoning, Me.nnTypeMRZ, Me.toolStripMenuItem1, Me.nnInvert, Me.nnRotate90, Me.nnRotate270, Me.nnRotate180, Me.nnMirrorH, Me.nnMirrorV, Me.toolStripMenuItem2, Me.nnSetRegExp, Me.ToolStripMenuItem3, Me.nnDeleteBlock})
        Me.pmBlock.Name = "pmBlock"
        Me.pmBlock.Size = New System.Drawing.Size(267, 396)
        '
        'nnTypeOCRText
        '
        Me.nnTypeOCRText.Name = "nnTypeOCRText"
        Me.nnTypeOCRText.Size = New System.Drawing.Size(266, 22)
        Me.nnTypeOCRText.Text = "Type: Machine-printed Text"
        '
        'nnTypeOCRDigit
        '
        Me.nnTypeOCRDigit.Name = "nnTypeOCRDigit"
        Me.nnTypeOCRDigit.Size = New System.Drawing.Size(266, 22)
        Me.nnTypeOCRDigit.Text = "Type: Machine-printed Digits"
        '
        'nnTypeICRDigit
        '
        Me.nnTypeICRDigit.Name = "nnTypeICRDigit"
        Me.nnTypeICRDigit.Size = New System.Drawing.Size(266, 22)
        Me.nnTypeICRDigit.Text = "Type: Handwritten Digits"
        '
        'nnTypeBarCode
        '
        Me.nnTypeBarCode.Name = "nnTypeBarCode"
        Me.nnTypeBarCode.Size = New System.Drawing.Size(266, 22)
        Me.nnTypeBarCode.Text = "Type: Bar Code"
        '
        'nnTypeTable
        '
        Me.nnTypeTable.Name = "nnTypeTable"
        Me.nnTypeTable.Size = New System.Drawing.Size(266, 22)
        Me.nnTypeTable.Text = "Type: Table"
        '
        'nnTypePicture
        '
        Me.nnTypePicture.Name = "nnTypePicture"
        Me.nnTypePicture.Size = New System.Drawing.Size(266, 22)
        Me.nnTypePicture.Text = "Type: Picture"
        '
        'nnTypeClear
        '
        Me.nnTypeClear.Name = "nnTypeClear"
        Me.nnTypeClear.Size = New System.Drawing.Size(266, 22)
        Me.nnTypeClear.Text = "Type: Clear Area"
        '
        'nnTypeZoning
        '
        Me.nnTypeZoning.Name = "nnTypeZoning"
        Me.nnTypeZoning.Size = New System.Drawing.Size(266, 22)
        Me.nnTypeZoning.Text = "Type: Zoning area"
        '
        'nnTypeMRZ
        '
        Me.nnTypeMRZ.Name = "nnTypeMRZ"
        Me.nnTypeMRZ.Size = New System.Drawing.Size(266, 22)
        Me.nnTypeMRZ.Text = "Type: MRZ"
        '
        'toolStripMenuItem1
        '
        Me.toolStripMenuItem1.Name = "toolStripMenuItem1"
        Me.toolStripMenuItem1.Size = New System.Drawing.Size(263, 6)
        '
        'nnInvert
        '
        Me.nnInvert.Name = "nnInvert"
        Me.nnInvert.Size = New System.Drawing.Size(266, 22)
        Me.nnInvert.Text = "Image Inverted"
        '
        'nnRotate90
        '
        Me.nnRotate90.Name = "nnRotate90"
        Me.nnRotate90.Size = New System.Drawing.Size(266, 22)
        Me.nnRotate90.Text = "Image Rotated 90° clockwise"
        '
        'nnRotate270
        '
        Me.nnRotate270.Name = "nnRotate270"
        Me.nnRotate270.Size = New System.Drawing.Size(266, 22)
        Me.nnRotate270.Text = "Image Rotated 90° counterclockwise"
        '
        'nnRotate180
        '
        Me.nnRotate180.Name = "nnRotate180"
        Me.nnRotate180.Size = New System.Drawing.Size(266, 22)
        Me.nnRotate180.Text = "Image Rotated 180°"
        '
        'nnMirrorH
        '
        Me.nnMirrorH.Name = "nnMirrorH"
        Me.nnMirrorH.Size = New System.Drawing.Size(266, 22)
        Me.nnMirrorH.Text = "Image Mirrored Horizontally"
        '
        'nnMirrorV
        '
        Me.nnMirrorV.Name = "nnMirrorV"
        Me.nnMirrorV.Size = New System.Drawing.Size(266, 22)
        Me.nnMirrorV.Text = "Image Mirrored Vertically"
        '
        'toolStripMenuItem2
        '
        Me.toolStripMenuItem2.Name = "toolStripMenuItem2"
        Me.toolStripMenuItem2.Size = New System.Drawing.Size(263, 6)
        '
        'nnSetRegExp
        '
        Me.nnSetRegExp.Name = "nnSetRegExp"
        Me.nnSetRegExp.Size = New System.Drawing.Size(266, 22)
        Me.nnSetRegExp.Text = "Set Regular Expression"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(263, 6)
        '
        'nnDeleteBlock
        '
        Me.nnDeleteBlock.Name = "nnDeleteBlock"
        Me.nnDeleteBlock.Size = New System.Drawing.Size(266, 22)
        Me.nnDeleteBlock.Text = "Delete Block(Zone)"
        '
        'opBlocks
        '
        Me.opBlocks.DefaultExt = "blk"
        Me.opBlocks.Filter = "blk files|*.blk"
        '
        'svBlocks
        '
        Me.svBlocks.DefaultExt = "blk"
        Me.svBlocks.Filter = "blk files|*.blk"
        '
        'bkSave
        '
        Me.bkSave.Location = New System.Drawing.Point(254, 5)
        Me.bkSave.Name = "bkSave"
        Me.bkSave.Size = New System.Drawing.Size(75, 23)
        Me.bkSave.TabIndex = 31
        Me.bkSave.Text = "Save"
        Me.bkSave.UseVisualStyleBackColor = True
        '
        'svFile
        '
        Me.svFile.DefaultExt = "pdf"
        Me.svFile.Filter = "PDF document(*.pdf)|*.pdf|RTF document (*.rtf)|*.rtf|ASCII Text document (*.txt)|" & _
            "*.txt|Unicode Text document (*.txt)|*.txt|XML document (*.xml)|*.xml|PDF/A docum" & _
            "ent(*.pdf)|*.pdf"
        '
        'bkScan
        '
        Me.bkScan.Location = New System.Drawing.Point(11, 5)
        Me.bkScan.Name = "bkScan"
        Me.bkScan.Size = New System.Drawing.Size(75, 23)
        Me.bkScan.TabIndex = 32
        Me.bkScan.Text = "Scan"
        Me.bkScan.UseVisualStyleBackColor = True
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.bkSelectLanguages)
        Me.groupBox1.Location = New System.Drawing.Point(577, 5)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(200, 53)
        Me.groupBox1.TabIndex = 33
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "Languages"
        '
        'bkSelectLanguages
        '
        Me.bkSelectLanguages.Location = New System.Drawing.Point(41, 20)
        Me.bkSelectLanguages.Name = "bkSelectLanguages"
        Me.bkSelectLanguages.Size = New System.Drawing.Size(113, 23)
        Me.bkSelectLanguages.TabIndex = 0
        Me.bkSelectLanguages.Text = "Select Languages"
        Me.bkSelectLanguages.UseVisualStyleBackColor = True
        '
        'bkOptions
        '
        Me.bkOptions.Location = New System.Drawing.Point(405, 37)
        Me.bkOptions.Name = "bkOptions"
        Me.bkOptions.Size = New System.Drawing.Size(75, 23)
        Me.bkOptions.TabIndex = 34
        Me.bkOptions.Text = "Options"
        Me.bkOptions.UseVisualStyleBackColor = True
        '
        'bkHelp
        '
        Me.bkHelp.Location = New System.Drawing.Point(488, 37)
        Me.bkHelp.Name = "bkHelp"
        Me.bkHelp.Size = New System.Drawing.Size(75, 23)
        Me.bkHelp.TabIndex = 35
        Me.bkHelp.Text = "Help"
        Me.bkHelp.UseVisualStyleBackColor = True
        '
        'cbCharRects
        '
        Me.cbCharRects.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cbCharRects.AutoSize = True
        Me.cbCharRects.Location = New System.Drawing.Point(6, 341)
        Me.cbCharRects.Name = "cbCharRects"
        Me.cbCharRects.Size = New System.Drawing.Size(103, 17)
        Me.cbCharRects.TabIndex = 35
        Me.cbCharRects.Text = "Show char rects"
        Me.cbCharRects.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1006, 448)
        Me.Controls.Add(Me.bkHelp)
        Me.Controls.Add(Me.bkOptions)
        Me.Controls.Add(Me.groupBox1)
        Me.Controls.Add(Me.bkScan)
        Me.Controls.Add(Me.cbScale)
        Me.Controls.Add(Me.bkSave)
        Me.Controls.Add(Me.cbExactCopy)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.bkSetPage)
        Me.Controls.Add(Me.bkRecognize)
        Me.Controls.Add(Me.lbPages)
        Me.Controls.Add(Me.edPage)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lbWait)
        Me.Controls.Add(Me.bkFile)
        Me.Controls.Add(Me.statusStrip1)
        Me.Controls.Add(Me.splitContainer1)
        Me.Name = "Form1"
        Me.Text = "Nicomsoft OCR VB.NET Advanced Sample"
        Me.splitContainer1.Panel1.ResumeLayout(False)
        Me.splitContainer1.Panel1.PerformLayout()
        Me.splitContainer1.Panel2.ResumeLayout(False)
        Me.splitContainer1.Panel2.PerformLayout()
        Me.splitContainer1.ResumeLayout(False)
        CType(Me.PicBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pmBlock.ResumeLayout(False)
        Me.groupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents lbWait As System.Windows.Forms.Label
    Private WithEvents bkRecognize As System.Windows.Forms.Button
    Private WithEvents bkFile As System.Windows.Forms.Button
    Private WithEvents statusStrip1 As System.Windows.Forms.StatusStrip
    Private WithEvents splitContainer1 As System.Windows.Forms.SplitContainer
    Private WithEvents PicBox As System.Windows.Forms.PictureBox
    Private WithEvents tbText As System.Windows.Forms.TextBox
    Private WithEvents opImg As System.Windows.Forms.OpenFileDialog
    Private WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents edPage As System.Windows.Forms.TextBox
    Private WithEvents lbPages As System.Windows.Forms.Label
    Private WithEvents bkSetPage As System.Windows.Forms.Button
    Private WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cbScale As System.Windows.Forms.ComboBox
    Friend WithEvents cbExactCopy As System.Windows.Forms.CheckBox
    Private WithEvents pmBlock As System.Windows.Forms.ContextMenuStrip
    Private WithEvents nnTypeOCRText As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents nnTypeICRDigit As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents nnTypePicture As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents nnTypeClear As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents toolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents nnInvert As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents nnRotate90 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents nnRotate180 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents nnRotate270 As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents nnMirrorH As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents nnMirrorV As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents toolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Private WithEvents nnDeleteBlock As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cbBin As System.Windows.Forms.CheckBox
    Friend WithEvents bkClearBlocks As System.Windows.Forms.Button
    Friend WithEvents bkSaveBlocks As System.Windows.Forms.Button
    Friend WithEvents bkLoadBlocks As System.Windows.Forms.Button
    Friend WithEvents opBlocks As System.Windows.Forms.OpenFileDialog
    Friend WithEvents svBlocks As System.Windows.Forms.SaveFileDialog
    Friend WithEvents bkDetectBlocks As System.Windows.Forms.Button
    Private WithEvents bkSave As System.Windows.Forms.Button
    Private WithEvents svFile As System.Windows.Forms.SaveFileDialog
    Private WithEvents bkScan As System.Windows.Forms.Button
    Friend WithEvents nnTypeOCRDigit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents nnTypeZoning As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents nnTypeBarCode As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents nnTypeTable As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents groupBox1 As System.Windows.Forms.GroupBox
    Private WithEvents bkSelectLanguages As System.Windows.Forms.Button
    Private WithEvents cbPixLines As System.Windows.Forms.CheckBox
    Friend WithEvents bkOptions As System.Windows.Forms.Button
    Friend WithEvents bkHelp As System.Windows.Forms.Button
    Friend WithEvents nnSetRegExp As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents nnTypeMRZ As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents cbCharRects As System.Windows.Forms.CheckBox

End Class
