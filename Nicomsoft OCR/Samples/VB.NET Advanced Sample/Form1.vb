Public Class Form1
    Public NsOCR As Object 'will be created later, in "OnMyFormLoad"

    '/////
    Public CfgObj As Integer
    Public OcrObj As Integer
    Public ImgObj As Integer
    Public ScanObj As Integer
    Public SvrObj As Integer

    Dim Dwn As Boolean
    Dim Frame As Rectangle
    Dim gr As Graphics
    Dim bmp As Image

    Dim NoEvents As Boolean
    Dim Inited As Boolean = False

    Dim pmBlockTag As Integer

    Dim fmScan As New Form22
    Dim fmWait As New Form3
    Dim fmLangs As New Form4
    Dim fmOptions As New Form5

    Public Sub ShowError(ByVal api As String, ByVal err As Integer)
        Dim s As String
        s = api + " Error #" + Hex(err)
        MsgBox(s)
    End Sub

    Private Sub OnMyFormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        If Not Inited Then Exit Sub

        If ImgObj <> 0 Then
            NsOCR.Img_Destroy(ImgObj)
        End If
        If OcrObj <> 0 Then
            NsOCR.Ocr_Destroy(OcrObj)
        End If
        If CfgObj <> 0 Then
            NsOCR.Cfg_Destroy(CfgObj)
        End If
        If ScanObj <> 0 Then
            NsOCR.Scan_Destroy(ScanObj)
        End If
        NsOCR.Engine_Uninitialize()
    End Sub

    Private Sub OnMyFormLoad(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CfgObj = 0
        OcrObj = 0
        ImgObj = 0

        'it is a good idea to create NSOCR instance here instead of creting it in the same line with "NsOCR" definition
        'this way we can handle possible errors if NSOCR is not installed
        Try
            NsOCR = New NSOCRLib.NSOCRClass
        Catch exc As Exception
            Dim cr = Chr(13) + Chr(10)
            Dim msg As String = "Cannot create NSOCR COM object instance. Possible reasons:" + cr + " - NSOCR.dll is missed." + cr + " - NSOCR.dll was not registered with Regsvr32." + cr + " - NSOCR.dll is x32 but this application is x64." + cr
            msg = msg + cr + " Exception message:" + cr + cr + exc.Message
            System.Windows.Forms.MessageBox.Show(msg)
            Close()
            Exit Sub
        End Try

        Inited = True 'NSOCR instance created successfully

        Dim val As String = ""
        NsOCR.Engine_GetVersion(val)
        Text = Text + " [ NSOCR version: " + val + " ] "

        NsOCR.Engine_SetLicenseKey("") 'required for licensed version only

        NsOCR.Engine_Initialize()
        NsOCR.Cfg_Create(CfgObj)
        NsOCR.Ocr_Create(CfgObj, OcrObj)
        NsOCR.Img_Create(OcrObj, ImgObj)
        NsOCR.Scan_Create(CfgObj, ScanObj)

        NsOCR.Cfg_LoadOptions(CfgObj, "Config.dat") 'load options, if path not specified, current folder and folder with NSOCR.dll will be checked

        bkRecognize.Enabled = False
        gr = PicBox.CreateGraphics()

        NoEvents = True
        cbScale.SelectedIndex = 0
        NoEvents = False
        bkSave.Enabled = False

        'copy some settings to GUI
        If NsOCR.Cfg_GetOption(CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/AutoScale", val) < TNSOCR.ERROR_FIRST Then
            cbScale.Enabled = (val = "1")
        End If
    End Sub


    Private Function IsImgLoaded() As Boolean
        Dim w, h As Integer
        IsImgLoaded = False
        If NsOCR.Img_GetSize(ImgObj, w, h) > TNSOCR.ERROR_FIRST Then Exit Function
        IsImgLoaded = (w > 0) And (h > 0)
    End Function

    Private Function GetCurDocScale() As Double
        If Not IsImgLoaded() Then
            GetCurDocScale = 1.0
            Exit Function
        End If

        Dim r As Rectangle
        r = splitContainer1.Bounds
        Dim w, h, ww, hh As Integer
        w = splitContainer1.SplitterDistance - 15
        h = r.Height - 45

        NsOCR.Img_GetSize(ImgObj, ww, hh)
        Dim kX, kY, k As Double
        kX = w / ww
        kY = h / hh

        If kX > kY Then k = kY Else k = kX

        GetCurDocScale = k
    End Function


    Private Sub ShowImage()
        If Not IsImgLoaded() Then Exit Sub
        Dim k As Double = GetCurDocScale()
        Dim r As Rectangle = New Rectangle()
        Dim col As Color
        Dim w As Integer = bmp.Width
        Dim h As Integer = bmp.Height
        Dim bmp2 As Bitmap = New Bitmap(w, h, gr)
        Dim graphic As Graphics = Graphics.FromImage(bmp2)
        graphic.DrawImage(bmp, 0, 0)

        If cbPixLines.Checked Then
            DrawPixLines(graphic)
        End If

        If cbCharRects.Checked Then
            DrawCharRects(graphic)
        End If

        Dim i, BlkObj, Xpos, Ypos, Width, Height As Integer
        Dim pen As Pen = New Pen(Color.Green)
        Dim cnt As Integer = NsOCR.Img_GetBlockCnt(ImgObj)

        For i = 0 To cnt - 1
            NsOCR.Img_GetBlock(ImgObj, i, BlkObj)
            NsOCR.Blk_GetRect(BlkObj, Xpos, Ypos, Width, Height)
            r.X = (k * Xpos)
            r.Y = (k * Ypos)
            r.Width = (k * Width)
            r.Height = (k * Height)

            col = System.Drawing.Color.Red
            Select Case NsOCR.Blk_GetType(BlkObj)
                Case TNSOCR.BT_OCRTEXT
                    col = System.Drawing.Color.Green
                Case TNSOCR.BT_OCRDIGIT
                    col = System.Drawing.Color.Lime
                Case TNSOCR.BT_ICRDIGIT
                    col = System.Drawing.Color.Blue
                Case TNSOCR.BT_BARCODE
                    col = System.Drawing.Color.Navy
                Case TNSOCR.BT_PICTURE
                    col = System.Drawing.Color.Aqua
                Case TNSOCR.BT_CLEAR
                    col = System.Drawing.Color.Gray
                Case TNSOCR.BT_ZONING
                    col = System.Drawing.Color.Black
                Case TNSOCR.BT_TABLE
                    col = System.Drawing.Color.Olive
                Case TNSOCR.BT_MRZ
                    col = System.Drawing.Color.Black
            End Select

            pen.Width = 2
            pen.Color = col
            graphic.DrawRectangle(pen, r)

            Dim fnt As System.Drawing.Font = New Font("Arial", 8)
            Dim br As System.Drawing.Brush = New SolidBrush(Color.Lime)
            Dim pnt As System.Drawing.PointF = New PointF(r.X, r.Y)
            r.Width = 12
            r.Height = 14
            graphic.FillRectangle(br, r)
            br = New SolidBrush(Color.Black)
            graphic.DrawString(i.ToString(), fnt, br, pnt)
        Next i

        'user is creating new block, draw its frame
        If Dwn Then
            r.X = (k * Frame.Left)
            r.Y = (k * Frame.Top)
            r.Width = (k * Frame.Width)
            r.Height = (k * Frame.Height)


            w = bmp2.Width
            h = bmp2.Height

            If r.Width >= w Then r.Width = w - 1
            If r.Height >= h Then r.Height = h - 1

            Dim pn As Pen = New Pen(System.Drawing.Color.Red)
            graphic.DrawRectangle(pn, r)
        End If

        PicBox.Image = bmp2
        GC.Collect()
    End Sub

    Private Sub DrawPixLines(ByRef graphic As Graphics)
        Dim i, cnt, x1, y1, x2, y2, w As Integer
        Dim k As Double

        k = GetCurDocScale()
        cnt = NsOCR.Img_GetPixLineCnt(ImgObj)
        For i = 0 To cnt - 1
            NsOCR.Img_GetPixLine(ImgObj, i, x1, y1, x2, y2, w)
            x1 = (k * x1)
            y1 = (k * y1)
            x2 = (k * x2)
            y2 = (k * y2)

            Dim pt1, pt2 As Point
            pt1 = New Point(x1, y1)
            pt2 = New Point(x2, y2)

            Dim pn As Pen
            pn = New Pen(Color.Red)
            pn.Width = 2
            graphic.DrawLine(pn, pt1, pt2)
        Next i
    End Sub

    Private Sub DrawCharRects(ByRef graphic As Graphics)
        Dim bcnt, BlkObj, lcnt, wcnt, ccnt, i, j, k, m, x, y, w, h As Integer
        Dim kf As Double

        kf = GetCurDocScale()

        bcnt = NsOCR.Img_GetBlockCnt(ImgObj)
        For i = 0 To bcnt - 1
            NsOCR.Img_GetBlock(ImgObj, i, BlkObj)
            lcnt = NsOCR.Blk_GetLineCnt(BlkObj)
            For j = 0 To lcnt - 1
                wcnt = NsOCR.Blk_GetWordCnt(BlkObj, j)
                For k = 0 To wcnt - 1
                    ccnt = NsOCR.Blk_GetCharCnt(BlkObj, j, k)
                    For m = 0 To ccnt - 1
                        NsOCR.Blk_GetCharRect(BlkObj, j, k, m, x, y, w, h)

                        Dim pn As New Pen(Color.Blue)
                        graphic.DrawRectangle(pn, Convert.ToInt32(kf * x), Convert.ToInt32(kf * y), Convert.ToInt32(kf * w), Convert.ToInt32(kf * h))
                    Next m
                Next k
            Next j
        Next i
    End Sub

    Private Sub AdjustDocScale()
        If Not IsImgLoaded() Then Exit Sub
        Dim ww, hh As Integer

        Dim r As Rectangle
        r = splitContainer1.Bounds

        ww = splitContainer1.SplitterDistance - 15
        hh = r.Height - 45

        If (ww <= 0) Or (hh <= 0) Then
            Exit Sub
        End If

        bmp = New Bitmap(ww, hh, gr)
        Dim gr2 As Graphics = Graphics.FromImage(bmp)
        Dim dc As IntPtr = gr2.GetHdc()
        If cbBin.Checked Then
            NsOCR.Img_DrawToDC(ImgObj, dc, 0, 0, ww, hh, TNSOCR.DRAW_BINARY)
        Else : NsOCR.Img_DrawToDC(ImgObj, dc, 0, 0, ww, hh, TNSOCR.DRAW_NORMAL)
        End If
        gr2.ReleaseHdc(dc)

        ShowImage()
    End Sub
    Private Sub OpenURL(ByVal url As String)
        System.Diagnostics.Process.Start(url)
    End Sub


    Private Sub bkFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bkFile.Click
        If opImg.ShowDialog() <> DialogResult.OK Then Exit Sub

        If cbScale.Enabled Then
            If cbScale.SelectedIndex < 1 Then 'autoscaling
                NsOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/AutoScale", "1")
                NsOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/ScaleFactor", "1.0") 'default scale if cannot detect it automatically
            Else 'fixed scaling
                NsOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/AutoScale", "0")
                Dim s As String
                s = cbScale.Items(cbScale.SelectedIndex).ToString()
                NsOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/ScaleFactor", s)
            End If
        End If

        Dim res As Integer

        'It is possible to load image from file, from memory or from bitmap data in memory, demostrate all ways
        Dim LoadMode As Integer = 0   '0 - from file; 1 - from memory; 2 - from memory bitmap; 3 - from PictureBox 

        If LoadMode = 0 Then 'load image to OCR engline from image file

            res = NsOCR.Img_LoadFile(ImgObj, opImg.FileName)

        Else
            If LoadMode = 1 Then 'load image to OCR engline from image in memory

                Dim FileArray As Array = System.IO.File.ReadAllBytes(opImg.FileName)
                res = NsOCR.Img_LoadFromMemory(ImgObj, FileArray, FileArray.Length)

            Else
                If LoadMode = 2 Then 'load image to OCR engine from memory bitmap

                    Dim myBmp As New Bitmap(opImg.FileName) 'Note .NET "Bitmap" class understands only few image formats like BMP, JPG

                    ' Lock the bitmap's bits.
                    Dim bmpData As System.Drawing.Imaging.BitmapData = myBmp.LockBits(New Rectangle(0, 0, myBmp.Width, myBmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb)

                    ' Get the address of the first line.
                    Dim ptr As IntPtr = bmpData.Scan0

                    ' Declare an array to hold the bytes of the bitmap.
                    Dim bytes As Integer = Math.Abs(bmpData.Stride) * myBmp.Height
                    Dim rgbValues() As Byte
                    ReDim rgbValues(bytes)

                    ' Copy the RGB values into the array.
                    System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes)

                    ' Unlock the bits.
                    myBmp.UnlockBits(bmpData)

                    'convert to Array type
                    Dim BytesArray As Array = rgbValues

                    'load image to OCR engine
                    res = NsOCR.Img_LoadBmpData(ImgObj, BytesArray, myBmp.Width, myBmp.Height, TNSOCR.BMP_24BIT, 0)
                Else 'load image from PictureBox
                    Dim data As Byte()
                    Dim ms As New IO.MemoryStream()
                    Dim bmp As New Bitmap(opImg.FileName)
                    PictureBox1.Image = bmp
                    PictureBox1.Image.Save(ms, Drawing.Imaging.ImageFormat.Png)
                    data = ms.ToArray()
                    res = NsOCR.Img_LoadFromMemory(ImgObj, data, data.Length)
                End If
            End If

        End If
        '//////////////////////////

        If res > TNSOCR.ERROR_FIRST Then
            If res = TNSOCR.ERROR_CANNOTLOADGS Then 'cannot load GhostScript to support PDF
                Dim s As String = "GhostSript library is needed to support PDF files. Just download and install it with default settings. Do you want to download GhostScript now?"
                If System.Windows.Forms.MessageBox.Show(s, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> System.Windows.Forms.DialogResult.Yes Then Exit Sub
                OpenURL("http://www.nicomsoft.com/files/ocr/ghostscript.htm")
            Else
                ShowError("Img_LoadFile", res)
            End If
            Exit Sub
        End If

        DoImageLoaded()
    End Sub

    Public Sub DoImageLoaded()
        Dim res As Integer

        'check if image has multiple page and ask user if he wants process and save all pages automatically
        res = NsOCR.Img_GetPageCount(ImgObj)
        If res > TNSOCR.ERROR_FIRST Then
            ShowError("Img_GetPageCount", res)
            Exit Sub
        End If
        If res > 1 Then
            If System.Windows.Forms.MessageBox.Show("Image contains multiple pages (" + res.ToString() + "). Do you want to process and save all pages automatically?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                ProcessEntireDocument()
                bkRecognize.Enabled = False
                Exit Sub
            End If
        End If


        'now apply image scaling, binarize image, deskew etc,
        'everything except OCR itself
        res = NsOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_FIRST, TNSOCR.OCRSTEP_ZONING - 1, TNSOCR.OCRFLAG_NONE)
        If res > TNSOCR.ERROR_FIRST Then
            ShowError("Img_OCR", res)
        End If

        edPage.Text = "1"
        lbPages.Text = "of " + NsOCR.Img_GetPageCount(ImgObj).ToString()
        bkSave.Enabled = False

        Frame.X = 0
        Frame.Y = 0
        Frame.Width = 0
        Frame.Height = 0
        AdjustDocScale()

        bkRecognize.Enabled = True
        tbText.Text = ""
        bkLoadBlocks.Enabled = True
        bkSaveBlocks.Enabled = True
        bkClearBlocks.Enabled = True
        bkDetectBlocks.Enabled = True
    End Sub

    Private Sub bkSetPage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bkSetPage.Click

        If Not IsImgLoaded() Then Exit Sub
        Dim cnt, n, res As Integer
        cnt = NsOCR.Img_GetPageCount(ImgObj)

        If Not Int32.TryParse(edPage.Text, n) Then n = 1
        n = n - 1
        If n < 0 Then n = 0
        If n >= cnt Then n = cnt - 1
        NsOCR.Img_SetPage(ImgObj, n)
        edPage.Text = (n + 1).ToString()
        bkSave.Enabled = False

        'now apply image scaling, binarize image, deskew etc,
        'everything except OCR itself
        res = NsOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_FIRST, TNSOCR.OCRSTEP_ZONING - 1, TNSOCR.OCRFLAG_NONE)
        If res > TNSOCR.ERROR_FIRST Then
            ShowError("Img_OCR", res)
        End If

        Dim w, h As Integer
        NsOCR.Img_GetSize(ImgObj, w, h)

        Frame.X = 0
        Frame.Y = 0
        Frame.Width = w
        Frame.Height = h
        AdjustDocScale()
    End Sub


    Private Sub bkRecognize_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bkRecognize.Click
        If Not IsImgLoaded() Then
            MessageBox.Show("Image not loaded!")
            Exit Sub
        End If

        '////
        bkRecognize.Enabled = False
        lbWait.Visible = True
        bkSave.Enabled = False
        Me.Refresh()

        Dim res As Integer
        Dim InSameThread As Integer

        InSameThread = False 'perform OCR in non-blocking mode
        'InSameThread = True 'uncomment to perform OCR from this thread (GUI will be freezed)

        'perform OCR itself
        If InSameThread Then
            res = NsOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_ZONING, TNSOCR.OCRSTEP_LAST, TNSOCR.OCRFLAG_NONE)
        Else
            'do it in non-blocking mode and then wait for result
            res = NsOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_ZONING, TNSOCR.OCRSTEP_LAST, TNSOCR.OCRFLAG_THREAD)
            If res > TNSOCR.ERROR_FIRST Then
                ShowError("Ocr_OcrImg(1)", res)
                Exit Sub
            End If
            fmWait.mode = 0
            fmWait.fmMain = Me
            fmWait.ShowDialog()
            res = fmWait.res
        End If

        lbWait.Visible = False
        bkRecognize.Enabled = True
        bkSave.Enabled = True

        If res > TNSOCR.ERROR_FIRST Then
            If res = TNSOCR.ERROR_OPERATIONCANCELLED Then
                MsgBox("Operation was cancelled.")
            Else
                ShowError("Img_OCR", res)
                Exit Sub
            End If
        End If


        '////	

        cbBin_CheckedChanged(sender, e) 'repaint img (binarized image could change)
        ShowText()
    End Sub

    Private Sub ShowText()
        Dim txt As String = ""
        Dim flags As Integer
        If cbExactCopy.Checked Then
            flags = TNSOCR.FMT_EXACTCOPY
        Else : flags = TNSOCR.FMT_EDITCOPY
        End If

        NsOCR.Img_GetImgText(ImgObj, txt, flags)

        'demonstrates how to get text of each block(zone)
        'Dim s As String
        'txt = ""
        'Dim i, cnt, BlkObj As Integer
        'Dim Size, Style As Integer

        'cnt = NsOCR.Img_GetBlockCnt(ImgObj)
        'For i = 0 To cnt - 1
        'NsOCR.Img_GetBlock(ImgObj, i, BlkObj)
        's = ""
        'NsOCR.Blk_GetText(BlkObj, s, flags)
        'txt = txt + s + " -NEXT BLOCK- "
        'Size = NsOCR.Blk_GetWordFontSize(BlkObj, 0, 0) 'also get size and style of first block word
        'Style = NsOCR.Blk_GetWordFontStyle(BlkObj, 0, 0)
        'Next i

        tbText.Text = txt
    End Sub

    Private Sub PicBoxMouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PicBox.MouseDown

        If Not IsImgLoaded() Then Exit Sub

        If e.Button = MouseButtons.Right Then
            Dim p As Point = New Point(e.X, e.Y)
            Dim kk As Double = GetCurDocScale()
            Dim cnt As Integer = NsOCR.Img_GetBlockCnt(ImgObj)
            Dim n As Integer = -1
            Dim minsize As Integer = -1
            Dim i, BlkObj, Xpos, Ypos, Width, Height, sz As Integer

            Dim r As Rectangle = New Rectangle()
            For i = 0 To cnt - 1
                NsOCR.Img_GetBlock(ImgObj, i, BlkObj)
                NsOCR.Blk_GetRect(BlkObj, Xpos, Ypos, Width, Height)
                r.X = (kk * Xpos)
                r.Y = (kk * Ypos)
                r.Width = (kk * Width)
                r.Height = (kk * Height)

                If r.Contains(p) Then
                    'need to find smallest block because blocks may overlap
                    If Width < Height Then
                        sz = Width
                    Else : sz = Height
                    End If

                    If (minsize = -1) Or (sz < minsize) Then
                        minsize = sz
                        n = i
                    End If
                End If
            Next i

            If (n = -1) Then Exit Sub 'block not found
            pmBlockTag = n 'remember block index

            p = PicBox.PointToScreen(p)
            pmBlock.Show(p.X, p.Y)
            Exit Sub
        End If

        Dim w, h As Integer
        NsOCR.Img_GetSize(ImgObj, w, h)

        Dwn = True
        Dim k As Double = GetCurDocScale()
        Frame.X = (1 / k * e.X)
        If Frame.X < 0 Then Frame.X = 0
        If Frame.X > w Then Frame.X = w
        Frame.Y = (1 / k * e.Y)
        If Frame.Y < 0 Then Frame.Y = 0
        If Frame.Y > h Then Frame.Y = h

        Frame.Width = 0
        Frame.Height = 0

        ShowImage()
    End Sub

    Private Sub PicBoxMouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PicBox.MouseMove
        GC.Collect()
        If Not Dwn Then Exit Sub

        If Not IsImgLoaded() Then Exit Sub

        Dim w, h As Integer
        NsOCR.Img_GetSize(ImgObj, w, h)

        Dim k As Double = GetCurDocScale()
        Frame.Width = (1 / k * e.X) - Frame.Left + 1
        If Frame.Width < 0 Then Frame.Width = 0
        If Frame.Width > w Then Frame.Width = w

        Frame.Height = (1 / k * e.Y) - Frame.Top + 1
        If Frame.Height < 0 Then Frame.Height = 0
        If Frame.Height > h Then Frame.Height = h

        ShowImage()
    End Sub

    Private Sub PicBoxMouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PicBox.MouseUp
        Dim BlkObj, w, h, res As Integer
        If Not Dwn Then Exit Sub
        Dwn = False

        If Not IsImgLoaded() Then Exit Sub

        NsOCR.Img_GetSize(ImgObj, w, h)
        If Frame.Right >= w Then Frame.Width = w - Frame.Left - 1
        If Frame.Bottom >= h Then Frame.Height = h - Frame.Top - 1

        w = Frame.Right - Frame.Left + 1
        h = Frame.Bottom - Frame.Top + 1
        If (w < 8) Or (h < 8) Then
            ShowImage()
            Exit Sub
        End If

        res = NsOCR.Img_AddBlock(ImgObj, Frame.Left, Frame.Top, w, h, BlkObj)
        If res > TNSOCR.ERROR_FIRST Then
            ShowError("Img_AddBlock", res)
            Exit Sub
        End If

        'detect text block inversion
        NsOCR.Blk_Inversion(BlkObj, TNSOCR.BLK_INVERSE_DETECT)

        'detect text block rotation
        NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_DETECT)

        ShowImage()
    End Sub

    Private Sub PicBoxResized(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PicBox.SizeChanged
        If Not NoEvents Then
            Exit Sub
        End If


    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbScale.SelectedIndexChanged
        If NoEvents Then Exit Sub
        bkRecognize.Enabled = False
        bkFile_Click(sender, e)
    End Sub

    Private Sub splitContainer1_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles splitContainer1.SizeChanged
        If Not Inited Then Exit Sub
        AdjustDocScale()
    End Sub

    Private Sub cbExactCopy_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbExactCopy.CheckedChanged
        ShowText()
    End Sub

    Private Sub pmBlock_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles pmBlock.Opening
        Dim BlkObj, BlockType, r, m As Integer

        NsOCR.Img_GetBlock(ImgObj, pmBlockTag, BlkObj)
        BlockType = NsOCR.Blk_GetType(BlkObj)
        If BlockType = TNSOCR.BT_OCRTEXT Then
            nnTypeOCRText.Checked = True
        Else : nnTypeOCRText.Checked = False
        End If
        If BlockType = TNSOCR.BT_OCRDIGIT Then
            nnTypeOCRDigit.Checked = True
        Else : nnTypeOCRDigit.Checked = False
        End If
        If BlockType = TNSOCR.BT_ICRDIGIT Then
            nnTypeICRDigit.Checked = True
        Else : nnTypeICRDigit.Checked = False
        End If
        If BlockType = TNSOCR.BT_BARCODE Then
            nnTypeBarCode.Checked = True
        Else : nnTypeBarCode.Checked = False
        End If
        If BlockType = TNSOCR.BT_PICTURE Then
            nnTypePicture.Checked = True
        Else : nnTypePicture.Checked = False
        End If
        If BlockType = TNSOCR.BT_CLEAR Then
            nnTypeClear.Checked = True
        Else : nnTypeClear.Checked = False
        End If
        If BlockType = TNSOCR.BT_ZONING Then
            nnTypeZoning.Checked = True
        Else : nnTypeZoning.Checked = False
        End If
        If BlockType = TNSOCR.BT_TABLE Then
            nnTypeTable.Checked = True
        Else : nnTypeTable.Checked = False
        End If
        If BlockType = TNSOCR.BT_MRZ Then
            nnTypeMRZ.Checked = True
        Else : nnTypeMRZ.Checked = False
        End If
        If NsOCR.Blk_Inversion(BlkObj, TNSOCR.BLK_INVERSE_GET) <> 0 Then
            nnInvert.Checked = True
        Else : nnInvert.Checked = False
        End If

        r = NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_GET)
        If r = TNSOCR.BLK_ROTATE_270 Then
            nnRotate90.Checked = True
        Else : nnRotate90.Checked = False
        End If
        If r = TNSOCR.BLK_ROTATE_180 Then
            nnRotate180.Checked = True
        Else : nnRotate180.Checked = False
        End If
        If r = TNSOCR.BLK_ROTATE_90 Then
            nnRotate270.Checked = True
        Else : nnRotate270.Checked = False
        End If

        m = NsOCR.Blk_Mirror(BlkObj, TNSOCR.BLK_MIRROR_GET)
        If (m And TNSOCR.BLK_MIRROR_H) <> 0 Then
            nnMirrorH.Checked = True
        Else : nnMirrorH.Checked = False
        End If
        If (m And TNSOCR.BLK_MIRROR_V) <> 0 Then
            nnMirrorV.Checked = True
        Else : nnMirrorV.Checked = False
        End If
    End Sub

    Private Sub nnTypeOCRText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nnTypeOCRText.Click
        Dim BlkObj As Integer
        NsOCR.Img_GetBlock(ImgObj, pmBlockTag, BlkObj)
        NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_OCRTEXT)
        ShowImage()
    End Sub

    Private Sub nnTypeICRDigit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nnTypeICRDigit.Click
        Dim BlkObj As Integer
        NsOCR.Img_GetBlock(ImgObj, pmBlockTag, BlkObj)
        NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_ICRDIGIT)
        ShowImage()
    End Sub

    Private Sub nnTypePicture_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nnTypePicture.Click
        Dim BlkObj As Integer
        NsOCR.Img_GetBlock(ImgObj, pmBlockTag, BlkObj)
        NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_PICTURE)
        ShowImage()
    End Sub

    Private Sub nnTypeClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nnTypeClear.Click
        Dim BlkObj As Integer
        NsOCR.Img_GetBlock(ImgObj, pmBlockTag, BlkObj)
        NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_CLEAR)
        ShowImage()
    End Sub

    Private Sub nnInvert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nnInvert.Click
        Dim BlkObj As Integer
        nnInvert.Checked = Not nnInvert.Checked
        NsOCR.Img_GetBlock(ImgObj, pmBlockTag, BlkObj)
        If nnInvert.Checked Then
            NsOCR.Blk_Inversion(BlkObj, TNSOCR.BLK_INVERSE_SET1)
        Else : NsOCR.Blk_Inversion(BlkObj, TNSOCR.BLK_INVERSE_SET0)
        End If
    End Sub

    Private Sub nnRotate90_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nnRotate90.Click
        Dim BlkObj As Integer
        nnRotate90.Checked = Not nnRotate90.Checked
        NsOCR.Img_GetBlock(ImgObj, pmBlockTag, BlkObj)
        If Not nnRotate90.Checked Then
            NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_NONE)
        Else : NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_270)
        End If
    End Sub

    Private Sub nnRotate180_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nnRotate180.Click
        Dim BlkObj As Integer
        nnRotate180.Checked = Not nnRotate180.Checked
        NsOCR.Img_GetBlock(ImgObj, pmBlockTag, BlkObj)
        If Not nnRotate180.Checked Then
            NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_NONE)
        Else : NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_180)
        End If
    End Sub

    Private Sub nnRotate270_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nnRotate270.Click
        Dim BlkObj As Integer
        nnRotate270.Checked = Not nnRotate270.Checked
        NsOCR.Img_GetBlock(ImgObj, pmBlockTag, BlkObj)
        If Not nnRotate270.Checked Then
            NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_NONE)
        Else : NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_90)
        End If
    End Sub

    Private Sub nnMirrorH_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nnMirrorH.Click
        Dim BlkObj As Integer
        nnMirrorH.Checked = Not nnMirrorH.Checked
        NsOCR.Img_GetBlock(ImgObj, pmBlockTag, BlkObj)
        Dim m As Integer = NsOCR.Blk_Mirror(BlkObj, -1)
        If nnMirrorH.Checked Then
            m = m Or TNSOCR.BLK_MIRROR_H
        Else : m = m And Not TNSOCR.BLK_MIRROR_H
        End If
        NsOCR.Blk_Mirror(BlkObj, m)
    End Sub

    Private Sub nnMirrorV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nnMirrorV.Click
        Dim BlkObj As Integer
        nnMirrorV.Checked = Not nnMirrorV.Checked
        NsOCR.Img_GetBlock(ImgObj, pmBlockTag, BlkObj)
        Dim m As Integer = NsOCR.Blk_Mirror(BlkObj, -1)
        If nnMirrorV.Checked Then
            m = m Or TNSOCR.BLK_MIRROR_V
        Else : m = m And Not TNSOCR.BLK_MIRROR_V
        End If
        NsOCR.Blk_Mirror(BlkObj, m)
    End Sub

    Private Sub nnDeleteBlock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nnDeleteBlock.Click
        Dim BlkObj As Integer
        NsOCR.Img_GetBlock(ImgObj, pmBlockTag, BlkObj)
        Dim m As Integer = NsOCR.Blk_Mirror(BlkObj, -1)
        NsOCR.Img_DeleteBlock(ImgObj, BlkObj)
        ShowImage()
    End Sub


    Private Sub cbBin_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbBin.CheckedChanged
        AdjustDocScale() 'repaint
    End Sub

    Private Sub bkLoadBlocks_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bkLoadBlocks.Click
        If opBlocks.ShowDialog() <> DialogResult.OK Then Exit Sub

        NsOCR.Img_DeleteAllBlocks(ImgObj) 'note: Img_LoadBlocks does not remove existing blocks, so remove them here
        bkSave.Enabled = False
        Dim res As Integer = NsOCR.Img_LoadBlocks(ImgObj, opBlocks.FileName)
        If res > TNSOCR.ERROR_FIRST Then
            ShowError("Img_LoadBlocks", res)
            Exit Sub
        End If
        ShowImage()
    End Sub

    Private Sub bkSaveBlocks_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bkSaveBlocks.Click
        If svBlocks.ShowDialog() <> DialogResult.OK Then Exit Sub

        Dim res As Integer = NsOCR.Img_SaveBlocks(ImgObj, svBlocks.FileName)
        If res > TNSOCR.ERROR_FIRST Then ShowError("Img_SaveBlocks", res)
    End Sub

    Private Sub bkDetectBlocks_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bkDetectBlocks.Click
        NsOCR.Img_DeleteAllBlocks(ImgObj)
        NsOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_ZONING, TNSOCR.OCRSTEP_ZONING, TNSOCR.OCRFLAG_NONE)
        ShowImage()
    End Sub

    Private Sub bkClearBlocks_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bkClearBlocks.Click
        NsOCR.Img_DeleteAllBlocks(ImgObj)
        bkSave.Enabled = False
        ShowImage()
    End Sub

    Private Function GetDocName(ByVal str As String) As String
        Dim fn As String = System.IO.Path.GetFileName(str)
        GetDocName = System.IO.Path.ChangeExtension(fn, ".pdf")
    End Function

    Private Sub bkSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bkSave.Click
        SvrObj = 0
        svFile.FileName = GetDocName(opImg.FileName)
        svFile.FilterIndex = 1
        If svFile.ShowDialog() <> DialogResult.OK Then
            Exit Sub
        End If

        Dim format As Integer = TNSOCR.SVR_FORMAT_PDF + (svFile.FilterIndex - 1)

        'image over text option for PDF
        If (format = TNSOCR.SVR_FORMAT_PDF) Or (format = TNSOCR.SVR_FORMAT_PDFA) Then
            If MsgBox("Place page image over recognized text?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                NsOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "Saver/PDF/ImageLayer", "1")
            Else
                NsOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "Saver/PDF/ImageLayer", "0")
            End If
        End If

        Dim res As Integer
        res = NsOCR.Svr_Create(CfgObj, format, SvrObj)
        If res > TNSOCR.ERROR_FIRST Then
            ShowError("Svr_Create", res)
            Exit Sub
        End If

        Dim flags As Integer
        If cbExactCopy.Checked Then
            flags = TNSOCR.FMT_EXACTCOPY
        Else : flags = TNSOCR.FMT_EDITCOPY
        End If

        NsOCR.Svr_NewDocument(SvrObj)

        If sender Is Nothing Then
            Exit Sub
        End If

        res = NsOCR.Svr_AddPage(SvrObj, ImgObj, flags)
        If res > TNSOCR.ERROR_FIRST Then
            ShowError("Svr_AddPage", res)
            NsOCR.Svr_Destroy(SvrObj)
            Exit Sub
        End If

        If (format = TNSOCR.SVR_FORMAT_PDF) Or (format = TNSOCR.SVR_FORMAT_PDFA) Or (format = TNSOCR.SVR_FORMAT_XML) Then 'demonstrate how to write PDF info
            NsOCR.Svr_SetDocumentInfo(SvrObj, TNSOCR.INFO_PDF_TITLE, "Sample Title")
        End If

        'Dim Buff As System.Array = New Byte() {}
        'res = NsOCR.Svr_SaveToMemory(SvrObj, Buff) 'you can save to memory buffer

        res = NsOCR.Svr_SaveToFile(SvrObj, svFile.FileName)
        NsOCR.Svr_Destroy(SvrObj)

        If res > TNSOCR.ERROR_FIRST Then
            ShowError("Svr_SaveToFile", res)
            Exit Sub
        End If

        'open the file
        System.Diagnostics.Process.Start(svFile.FileName)
    End Sub

    Private Sub bkScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bkScan.Click
        fmScan.fmMain = Me
        fmScan.ShowDialog()
    End Sub

    Private Sub nnTypeOCRDigit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nnTypeOCRDigit.Click
        Dim BlkObj As Integer
        NsOCR.Img_GetBlock(ImgObj, pmBlockTag, BlkObj)
        NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_OCRDIGIT)
        ShowImage()
    End Sub

    Private Sub nnTypeZoning_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nnTypeZoning.Click
        Dim BlkObj As Integer
        NsOCR.Img_GetBlock(ImgObj, pmBlockTag, BlkObj)
        NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_ZONING)
        ShowImage()
    End Sub

    Private Sub nnTypeBarCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nnTypeBarCode.Click
        Dim BlkObj As Integer
        NsOCR.Img_GetBlock(ImgObj, pmBlockTag, BlkObj)
        NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_BARCODE)
        NsOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_BARCODE, "BarCode/SearchMode", "3") 'deep scanning for manual barcode zone
        ShowImage()
    End Sub


    Private Sub nnTypeTable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nnTypeTable.Click
        Dim BlkObj As Integer
        NsOCR.Img_GetBlock(ImgObj, pmBlockTag, BlkObj)
        NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_TABLE)
        ShowImage()
    End Sub

    Private Sub nnTypeMRZ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nnTypeMRZ.Click
        Dim BlkObj As Integer
        NsOCR.Img_GetBlock(ImgObj, pmBlockTag, BlkObj)
        NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_MRZ)
        ShowImage()
    End Sub

    Private Sub ProcessEntireDocument()
        bkSave_Click(Nothing, Nothing)

        If SvrObj = 0 Then
            Exit Sub 'saving cancelled
        End If


        Dim OcrCnt, res As Integer
        Dim InSameThread As Boolean
        'recognize up to 4 images at once.
        'Note for large images ERROR_NOMEMORY can be raised
        'OcrCnt = 4

        'Use number of logical CPU cores on the system for the best performance
        OcrCnt = 0

        InSameThread = False 'perform OCR in non-blocking mode
        'InSameThread = true; //uncomment to perform OCR from this thread (GUI will be freezed)

        Dim flags As Integer
        If cbExactCopy.Checked Then
            flags = TNSOCR.FMT_EXACTCOPY
        Else
            flags = TNSOCR.FMT_EDITCOPY
        End If
        flags = flags * 256 'we need to shift FMT_XXXXX flags for this function

        'process all pages of input image and add results to SAVER object
        'this function will create (and then release) additional temporary OCR objects if OcrCnt > 1
        If InSameThread Then
            res = NsOCR.Ocr_ProcessPages(ImgObj, SvrObj, 0, -1, OcrCnt, TNSOCR.OCRFLAG_NONE Or flags)
        Else
            'do it in non-blocking mode and then wait for result
            res = NsOCR.Ocr_ProcessPages(ImgObj, SvrObj, 0, -1, OcrCnt, TNSOCR.OCRFLAG_THREAD Or flags)
            If (res > TNSOCR.ERROR_FIRST) Then

                ShowError("Ocr_ProcessPages(1)", res)
                Exit Sub
            End If
            fmWait.mode = 1
            fmWait.fmMain = Me
            fmWait.ShowDialog()
            res = fmWait.res
        End If

        'restore option
        NsOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "Main/NumKernels", "0")

        If res > TNSOCR.ERROR_FIRST Then
            If (res = TNSOCR.ERROR_OPERATIONCANCELLED) Then
                System.Windows.Forms.MessageBox.Show("Operation was cancelled.")
            Else
                ShowError("Ocr_ProcessPages", res)
                NsOCR.Svr_Destroy(SvrObj)
                Exit Sub
            End If
        End If

        'save output document
        res = NsOCR.Svr_SaveToFile(SvrObj, svFile.FileName)
        NsOCR.Svr_Destroy(SvrObj)

        If res > TNSOCR.ERROR_FIRST Then
            ShowError("Svr_SaveToFile", res)
            Return
        End If

        'open the file
        System.Diagnostics.Process.Start(svFile.FileName)
    End Sub

    Private Sub bkSelectLanguages_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bkSelectLanguages.Click
        fmLangs.fmMain = Me
        fmLangs.ShowDialog()
    End Sub

    Private Sub cbPixLines_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPixLines.CheckedChanged, cbCharRects.CheckedChanged
        AdjustDocScale() 'repaint
    End Sub

    Public Sub ShowHelp(ByVal section As String)
        Dim fn As String = "file://" + AppDomain.CurrentDomain.BaseDirectory + "..\..\..\..\Docs\NSOCR.chm"
        Help.ShowHelp(Me, fn, section)
    End Sub

    Private Sub bkHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bkHelp.Click
        ShowHelp("")
    End Sub

    Private Sub bkOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bkOptions.Click
        fmOptions.fmMain = Me
        fmOptions.ShowDialog()
    End Sub

    Private Sub nnSetRegExp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nnSetRegExp.Click
        Dim BlkObj As Integer
        NsOCR.Img_GetBlock(ImgObj, pmBlockTag, BlkObj)
        Dim s As String = InputBox("Set regular expression (for entire zone)", "New regular expression:")

        If (NsOCR.Blk_SetWordRegEx(BlkObj, -1, -1, s, TNSOCR.REGEX_SET) > 0) Then
            System.Windows.Forms.MessageBox.Show("Regular Expression syntax error!")
        End If

        'set regular expression - the same expression for all words in block
        'regular expression for amounts like '123.00$', '500' or '-100,55'
        '       If NsOCR.Blk_SetWordRegEx(BlkObj, -1, -1, "(|-)[0-9]+(\.|,)?([0-9][0-9])?\$?", TNSOCR.REGEX_SET) > 0 Then
        '        System.Windows.Forms.MessageBox.Show("Regular Expression syntax error!")
        '        End If
        '        Dim teststr As String
        '        teststr = "-55.30$" 'test string
        '        If NsOCR.Blk_SetWordRegEx(BlkObj, -1, -1, teststr, TNSOCR.REGEX_CHECK) <> 1 Then
        '        System.Windows.Forms.MessageBox.Show("String does not comply with current regular expression!")
        '        Else
        '        System.Windows.Forms.MessageBox.Show("String is good!")
        '        End If

    End Sub


End Class