using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NSOCR_NameSpace; //Add NSOCR namespace from "NSOCR.cs"

namespace Sample
{
    public partial class Form1 : Form
    {
        //NSOCRLib COM was added via "Project / Add Reference" dialog
        public NSOCRLib.NSOCRClass NsOCR; //declare only, an instance will be created later, in "OnFormLoad"
        public int CfgObj = 0;
        public int OcrObj = 0;
        public int ImgObj = 0;
        public int ScanObj = 0;
        public int SvrObj = 0;

        bool Dwn = false;
        Rectangle Frame;

        Image bmp;
        public Graphics gr;

        bool NoEvent;
        bool Inited = false;
        int pmBlockTag = -1;

        Form2 fmScan = new Form2();
        Form3 fmWait = new Form3();
        Form5 fmOptions = new Form5();
        fmLanguages fmLangs = new fmLanguages();

        public Form1()
        {
            InitializeComponent();
        }

        public void ShowError(string api, int err)
        {
            string s;
            s = api + " Error #" + err.ToString("X");
            System.Windows.Forms.MessageBox.Show(s);
        }
/*
        private void TestDirectCallsToDLL()
        {
            int BlkObj, res;
            StringBuilder sb = new StringBuilder("abcde");

            res = TNSOCR.Engine_InitializeAdvanced(out CfgObj, out OcrObj, out ImgObj);
            res = TNSOCR.Img_LoadFile(ImgObj, "c:\\SampleImage.bmp");
            res = TNSOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_FIRST, TNSOCR.OCRSTEP_LAST, 0);
            res = TNSOCR.Img_GetBlock(ImgObj, 0, out BlkObj);
            res = TNSOCR.Blk_GetCharText(BlkObj, 0, 0, 0, 0, sb, 10); //get first letter of first word of first block
            System.Windows.Forms.MessageBox.Show(sb.ToString());
        }
*/
        private void OnFormLoad(object sender, EventArgs e)
        {
            // TestDirectCallsToDLL(); //minimal code to demonstrate how to use DLL directly, without COM

            //it is a good idea to create NSOCR instance here instead of creting it in the same line with "NsOCR" definition
            //this way we can handle possible errors if NSOCR is not installed
            try
            {
                NsOCR = new NSOCRLib.NSOCRClass();
            }
            catch(Exception exc) //some error
            {
                string msg = "Cannot create NSOCR COM object instance. Possible reasons:\r\n - NSOCR.dll is missed.\r\n - NSOCR.dll was not registered with Regsvr32.\r\n - NSOCR.dll is x32 but this application is x64.\r\n";
                msg = msg + "\r\n Exception message:\r\n\r\n" + exc.Message;
                System.Windows.Forms.MessageBox.Show(msg);
                Close();
                return;
            }

            gr = PicBox.CreateGraphics();

            Inited = true; //NSOCR instance created successfully

            //get NSOCR version
            string val;
            NsOCR.Engine_GetVersion(out val);
            Text = Text + " [ NSOCR version: " + val + " ] ";

            NsOCR.Engine_SetLicenseKey(""); //required for licensed version only

            //init engine and create ocr-related objects
            if (true /*false*/) //change to "false" to reduce code and initialize engine + create main objects in one line
            {
                NsOCR.Engine_Initialize();
                NsOCR.Cfg_Create(out CfgObj);
                //load options, if path not specified, folder with NSOCR.dll will be checked
                NsOCR.Cfg_LoadOptions(CfgObj, "Config.dat"); 
                NsOCR.Ocr_Create(CfgObj, out OcrObj);
                NsOCR.Img_Create(OcrObj, out ImgObj);
            }
            else //do the same in one line
                NsOCR.Engine_InitializeAdvanced(out CfgObj, out OcrObj, out ImgObj);

            NsOCR.Scan_Create(CfgObj, out ScanObj);

            bkRecognize.Enabled = false;

            NoEvent = true;
            cbScale.SelectedIndex = 0;
            NoEvent = false;
            bkSave.Enabled = false;
            
            //copy some settings to GUI
            if (NsOCR.Cfg_GetOption(CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/AutoScale", out val) < TNSOCR.ERROR_FIRST)
                cbScale.Enabled = (val == "1");


        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            if (Inited)
            {
                if (ImgObj != 0) NsOCR.Img_Destroy(ImgObj);
                if (OcrObj != 0) NsOCR.Ocr_Destroy(OcrObj);
                if (CfgObj != 0) NsOCR.Cfg_Destroy(CfgObj);
                if (ScanObj != 0) NsOCR.Scan_Destroy(ScanObj);
                NsOCR.Engine_Uninitialize();
            }
        }

        public bool IsImgLoaded()
        {
            if (!Inited) return false;
            int w, h;
            if (NsOCR.Img_GetSize(ImgObj, out w, out h) > TNSOCR.ERROR_FIRST) return false;
            return (w > 0) && (h > 0);
        }

        public float GetCurDocScale()
        {
            if (!IsImgLoaded()) return 1.0f;
            Rectangle r;
            r = splitContainer1.Bounds;
            int w = splitContainer1.SplitterDistance - 15;
            int h = r.Height - 45;

            int ww, hh;
            NsOCR.Img_GetSize(ImgObj, out ww, out hh);
            float kX = (float)w / ww;
            float kY = (float)h / hh;
            float k;
            if (kX > kY) k = kY;
            else k = kX;

            return k;
        }

        public void ShowImage()
        {
            if (!IsImgLoaded()) return;
            float k = GetCurDocScale();

            Color col;
            Rectangle r = new Rectangle();
            int w = bmp.Width;
            int h = bmp.Height;

            Image bmp2 = new Bitmap(w, h, gr);
            Graphics graphic = Graphics.FromImage(bmp2);
            graphic.DrawImage(bmp, 0, 0);


            Image bmp2_ = new Bitmap(w, h, gr);
            Graphics graphic_ = Graphics.FromImage(bmp2_);
            graphic_.DrawImage(bmp, 0, 0);


            if (cbPixLines.Checked)
                DrawPixLines(graphic);

            if (cbCharRects.Checked)
                DrawCharRects(graphic);

            int i, BlkObj, Xpos, Ypos, Width, Height;
            Pen pen = new Pen(Color.Green);
            int cnt = NsOCR.Img_GetBlockCnt(ImgObj);
            for (i = 0; i < cnt; i++)
            {
                NsOCR.Img_GetBlock(ImgObj, i, out BlkObj);
                NsOCR.Blk_GetRect(BlkObj, out Xpos, out Ypos, out Width, out Height);
                r.X = (int)(k * Xpos);
                r.Y = (int)(k * Ypos);
                r.Width = (int)(k * Width);
                r.Height = (int)(k * Height);

                col = System.Drawing.Color.Red;
                var translateBlock = true;
                switch (NsOCR.Blk_GetType(BlkObj))
                {
                    case TNSOCR.BT_OCRTEXT: col = System.Drawing.Color.Green; translateBlock = true; break;
                    case TNSOCR.BT_OCRDIGIT: col = System.Drawing.Color.Lime; translateBlock = false; break;
                    case TNSOCR.BT_ICRDIGIT: col = System.Drawing.Color.Blue; translateBlock = false; break;
                    case TNSOCR.BT_BARCODE: col = System.Drawing.Color.Navy; translateBlock = false; break;
                    case TNSOCR.BT_PICTURE: col = System.Drawing.Color.Aqua; translateBlock = false; break;
                    case TNSOCR.BT_CLEAR: col = System.Drawing.Color.Gray; translateBlock = true; break;
                    case TNSOCR.BT_ZONING: col = System.Drawing.Color.Black; translateBlock = true; break;
                    case TNSOCR.BT_TABLE: col = System.Drawing.Color.Olive; translateBlock = true; break;
                    case TNSOCR.BT_MRZ: col = System.Drawing.Color.Black; translateBlock = true; break;
                }

                //draw block numbers and frames
                pen.Width = 2;
                pen.Color = col;
                graphic.DrawRectangle(pen, r);                
                graphic_.DrawRectangle(pen, r);

                // get block background color
                var blkBgCol_ = NsOCR.Blk_GetBackgroundColor(BlkObj); //get zone background color 
                var color = Color.FromArgb(blkBgCol_);              
                var brush_ = new SolidBrush(color);
                graphic_.FillRectangle(brush_, r);

                // write block number
                System.Drawing.Font fnt = new Font("Arial", 8);
                System.Drawing.Brush br = new SolidBrush(Color.Lime);
                System.Drawing.PointF pnt = new PointF(r.X, r.Y);
                r.Width = 12;
                r.Height = 14;
                graphic.FillRectangle(br, r);
                graphic_.FillRectangle(br, r);

                br = new SolidBrush(Color.Black);
                graphic.DrawString(i.ToString(), fnt, br, pnt);
                graphic_.DrawString(i.ToString(), fnt, br, pnt);

                if (true)//translateBlock)
                {
                    // block details
                    string txtBlock = String.Empty;

                    //NsOCR.Blk_GetRect(BlkObj, out Xpos, out Ypos, out Width, out Height); //get zone position and size
                    var res = NsOCR.Blk_Inversion(BlkObj, TNSOCR.BLK_INVERSE_GET); //get zone inversion 
                    res = NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_GET); //get zone rotation
                    res = NsOCR.Blk_Mirror(BlkObj, TNSOCR.BLK_MIRROR_GET); //get zone mirror flag
                    res = NsOCR.Blk_GetBackgroundColor(BlkObj); //get zone background color 
                    NsOCR.Blk_GetText(BlkObj, out txtBlock, TNSOCR.FMT_EDITCOPY); //get zone text

                    var lcnt = NsOCR.Blk_GetLineCnt(BlkObj); //get number of text lines for zone
                    for (var j = 0; j < lcnt; j++)
                    {
                        // todo:
                        // get max line width; auto format to fit block
                        // decide if to use whole block or line or word
                        string txtLine;
                        string txtWord;


                        NsOCR.Blk_GetTextRect(BlkObj, j, -1, out Xpos, out Ypos, out Width, out Height); //get text line position and size 
                        NsOCR.Blk_GetLineText(BlkObj, j, out txtLine); //get line text

                        //// draw covering rectangle for line
                        //var brBg = new SolidBrush(Color.LightGreen);
                        //var rBg = new Rectangle((int)(k * Xpos), (int)(k * Ypos), (int)(k * Width), (int)(k * Height));
                        //graphic_.FillRectangle(brBg, rBg);
                        //graphic.FillRectangle(brBg, rBg);

                        //blkBgCol_ = NsOCR.Blk_GetBackgroundColor(BlkObj);
                        //var bgColor = Color.FromArgb(blkBgCol_);


                        var wcnt = NsOCR.Blk_GetWordCnt(BlkObj, j); //get number of words of "j"-th text line of zone
                        for (var l = 0; l < wcnt; l++)
                        {
                            var rect = NsOCR.Blk_GetTextRect(BlkObj, j, l, out Xpos, out Ypos, out Width, out Height); //get word position and size 

                            var fntColor = NsOCR.Blk_GetWordFontColor(BlkObj, j, l); //get word font color
                            var fntSize = NsOCR.Blk_GetWordFontSize(BlkObj, j, l); //get word font size
                            var fntStyle = NsOCR.Blk_GetWordFontStyle(BlkObj, j, l); //get word font style
                            res = NsOCR.Blk_GetWordQual(BlkObj, j, l); //get word quality
                            res = NsOCR.Blk_IsWordInDictionary(BlkObj, j, l); //check if this word is in dictionary	  
                            NsOCR.Blk_GetWordText(BlkObj, j, l, out txtWord); //get word text	  


                            //blkBgCol_ = NsOCR.Blk_GetBackgroundColor(rect);

                            // draw covering rectangle
                            var bgColor = Color.Wheat;
                            var brBg = new SolidBrush(bgColor);
                            var rBg = new Rectangle((int)(k * Xpos), (int)(k * (Ypos - 4)), (int)(k * (Width + 4) + 10), (int)(k * (Height + 4) + 10));
                            //graphic.FillRectangle(brBg, rBg);
                            graphic_.FillRectangle(brBg, rBg);


                            // draw word
                            var pnt_ = new Point((int)(k * Xpos), (int)(k * Ypos));
                            var br_ = new SolidBrush(fntColor == 0 ? Color.Black : Color.FromArgb(fntColor));
                            fntSize = (int)Math.Abs(fntSize / (1000 / k));
                            var fnt_ = new Font("Arial", fntSize, FontStyle.Regular);
                            graphic_.DrawString(txtWord, fnt_, br_, pnt_);

                        }

                        //// write line to target
                        //Point pnt_ = new Point(Xpos, Ypos);
                        //var br_ = new SolidBrush(Color.Black);
                        //var fnt_ = new Font("Arial", 8);

                        //graphic_.DrawString(txtLine, fnt_, br_, pnt_);
                    }
                }
            }
            //user is creating new block, draw its frame
            if (Dwn)
            {
                r.X = (int)(k * Frame.Left);
                r.Y = (int)(k * Frame.Top);
                r.Width = (int)(k * Frame.Width);
                r.Height = (int)(k * Frame.Height);

                if (r.Width >= w) r.Width = w - 1;
                if (r.Height >= h) r.Height = h - 1;

                pen = new Pen(Color.Red);
                graphic.DrawRectangle(pen, r);
            }

            PicBox.Image = bmp2;

            picBox2.Image = bmp2_;
            GC.Collect();
        }

        public void DrawPixLines(Graphics graphic)
        {
            int i, cnt, x1, y1, x2, y2, w;
            float k;

            k = GetCurDocScale();
            cnt = NsOCR.Img_GetPixLineCnt(ImgObj);
            for (i = 0; i < cnt; i++)
            {
                NsOCR.Img_GetPixLine(ImgObj, i, out x1, out y1, out x2, out y2, out w);
                x1 = (int)(k * x1);
                y1 = (int)(k * y1);
                x2 = (int)(k * x2);
                y2 = (int)(k * y2);

                Point pt1, pt2;
                pt1 = new Point(x1, y1);
                pt2 = new Point(x2, y2);

                Pen pen = new Pen(Color.Red);
                pen.Width = 2;
                graphic.DrawLine(pen, pt1, pt2);
            }
        }

        public void DrawCharRects(Graphics graphic)
        {
            int bcnt, BlkObj, lcnt, wcnt, ccnt, i, j, k, m, x, y, w, h;
            float kf;
            kf = GetCurDocScale();

            bcnt = NsOCR.Img_GetBlockCnt(ImgObj);
            for (i = 0; i < bcnt; i++)
            {
                NsOCR.Img_GetBlock(ImgObj, i, out BlkObj);
                lcnt = NsOCR.Blk_GetLineCnt(BlkObj);
                for (j = 0; j < lcnt; j++)
                {
                    wcnt = NsOCR.Blk_GetWordCnt(BlkObj, j);
                    for (k = 0; k < wcnt; k++)
                    {
                        ccnt = NsOCR.Blk_GetCharCnt(BlkObj, j, k);
                        for (m = 0; m < ccnt; m++)
                        {
                            NsOCR.Blk_GetCharRect(BlkObj, j, k, m, out x, out y, out w, out h);

                            Pen pn = new Pen(Color.Blue);
                            graphic.DrawRectangle(pn, kf * x, kf * y, kf * w, kf * h);
                        }
                    }
                }
            }
        }

        public void AdjustDocScale()
        {
            if (!IsImgLoaded()) return;
            int ww, hh;

            Rectangle r;
            r = splitContainer1.Bounds;
            ww = splitContainer1.SplitterDistance - 15;
            hh = r.Height - 45;

            if ((ww <= 0) || (hh <= 0))
                return;
            bmp = new Bitmap(ww, hh, gr);
            Graphics gr2 = Graphics.FromImage(bmp);
            IntPtr dc = gr2.GetHdc();
            if (cbBin.Checked) NsOCR.Img_DrawToDC(ImgObj, (int)dc, 0, 0, ref ww, ref hh, TNSOCR.DRAW_BINARY);
            else NsOCR.Img_DrawToDC(ImgObj, (int)dc, 0, 0, ref ww, ref hh, TNSOCR.DRAW_NORMAL);
            gr2.ReleaseHdc(dc);

            ShowImage();
        }

        private void OpenURL(string url)
        {
            System.Diagnostics.Process.Start(url);
        }

        private void bkFile_Click(object sender, EventArgs e)
        {
            if (opImg.ShowDialog() != DialogResult.OK) return;

            if (cbScale.Enabled)
            {
                if (cbScale.SelectedIndex < 1) //autoscaling
                {
                    NsOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/AutoScale", "1");
                    NsOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/ScaleFactor", "1.0"); //default scale if cannot detect it automatically
                }
                else //fixed scaling
                {
                    NsOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/AutoScale", "0");
                    string s = cbScale.Items[cbScale.SelectedIndex].ToString();
                    NsOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/ScaleFactor", s);
                }
            }

            //It is possible to load image from file, from memory or from bitmap data in memory, demonstrate all ways
            int LoadMode = 0; //0 - from file; 1 - from memory; 2 - from memory bitmap 
            int res;
            if (LoadMode == 0) //load image to OCR engline from image file
            {
                res = NsOCR.Img_LoadFile(ImgObj, opImg.FileName);
            }
            else
                if (LoadMode == 1) //load image to OCR engline from image in memory
                {
                    Array FileArray = File.ReadAllBytes(opImg.FileName);
                    res = NsOCR.Img_LoadFromMemory(ImgObj, ref FileArray, FileArray.Length);
                }
                else //load image to OCR engine from memory bitmap
                {
                    Bitmap myBmp = new Bitmap(opImg.FileName); //Note .NET "Bitmap" class understands only few image formats like BMP, JPG

                    // Lock the bitmap's bits.
                    BitmapData bmpData = myBmp.LockBits(new Rectangle(0, 0, myBmp.Width, myBmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
         
                    // Get the address of the first line.
                    IntPtr ptr = bmpData.Scan0;

                    // Declare an array to hold the bytes of the bitmap.
                    int bytes = Math.Abs(bmpData.Stride) * myBmp.Height;
                    byte[] rgbValues = new byte[bytes];

                    // Copy the RGB values into the array.
                    System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

                    // Unlock the bits.
                    myBmp.UnlockBits(bmpData);

                    //convert to Array type
                    Array BytesArray = rgbValues;

                    //load image to OCR engine
                    res = NsOCR.Img_LoadBmpData(ImgObj, ref BytesArray, myBmp.Width, myBmp.Height, TNSOCR.BMP_24BIT, 0);
                }

            //////////////////////////
            if (res > TNSOCR.ERROR_FIRST)
            {
                if (res == TNSOCR.ERROR_CANNOTLOADGS) //cannot load GhostScript to support PDF
                {
                    string s = "GhostSript library is needed to support PDF files. Just download and install it with default settings. Do you want to download GhostScript now?";
                    if (System.Windows.Forms.MessageBox.Show(s, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes) return;
                    OpenURL("http://www.nicomsoft.com/files/ocr/ghostscript.htm");
                }
                else ShowError("Img_LoadFile", res);
                return;
            }

            DoImageLoaded();
        }

        public void DoImageLoaded()
        {
            int res, w, h;

            //check if image has multiple page and ask user if he wants process and save all pages automatically
            res = NsOCR.Img_GetPageCount(ImgObj);
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Img_GetPageCount", res);
                return;
            }
            if (res > 1)
                if (System.Windows.Forms.MessageBox.Show("Image contains multiple pages (" + res.ToString() + "). Do you want to process and save all pages automatically?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes) 
                {
                    ProcessEntireDocument();
                    bkRecognize.Enabled = false;
                    return;
                }

            //now apply image scaling, binarize image, deskew etc,
            //everything except OCR itself
            res = NsOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_FIRST, TNSOCR.OCRSTEP_ZONING - 1, TNSOCR.OCRFLAG_NONE);
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Img_OCR", res);
                return;
            }

            ShowImageParams();

            res = NsOCR.Img_GetSkewAngle(ImgObj);
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Img_GetSkewAngle", res);
                lbSkew.Text = "";
            }
            else lbSkew.Text = "Skew angle (degrees): " + (res / 1000.0);

            //pixel lines info
            res = NsOCR.Img_GetPixLineCnt(ImgObj);
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Img_GetPixLineCnt", res);
                return;
            }
            lbLines.Text = "Lines: " + res.ToString();

            res = NsOCR.Img_GetProperty(ImgObj, TNSOCR.IMG_PROP_INVERTED);
            lbInverted.Text = (res == 1) ? "Inverted: Yes" : "Inverted: No";

            edPage.Text = "1";
            lbPages.Text = "of " + NsOCR.Img_GetPageCount(ImgObj).ToString();

            //final size after pre-processing
            NsOCR.Img_GetSize(ImgObj, out w, out h);

            Frame.X = 0;
            Frame.Y = 0;
            Frame.Width = 0;
            Frame.Height = 0;
            AdjustDocScale();

            bkRecognize.Enabled = true;
            tbText.Text = "";
            bkSave.Enabled = false;

            bkLoadBlocks.Enabled = true;
            bkSaveBlocks.Enabled = true;
            bkClearBlocks.Enabled = true;
            bkDetectBlocks.Enabled = true;

            //you can save image after preprocessing to a file or to memory
            //NsOCR.Img_SaveToFile(ImgObj, "c:\\image.jpg", TNSOCR.IMG_FORMAT_JPEG, 80); //80% quality
            //save to memory
            //Array bytes;
            //NsOCR.Img_SaveToMemory(ImgObj, out bytes, TNSOCR.IMG_FORMAT_JPEG, 80); //80% quality
        }

        private void ShowImageParams()
        {
            int w, h, w0, h0, bpp, dpi;
            //original image properties
            dpi = NsOCR.Img_GetProperty(ImgObj, TNSOCR.IMG_PROP_DPIX);
            bpp = NsOCR.Img_GetProperty(ImgObj, TNSOCR.IMG_PROP_BPP);
            w0 = NsOCR.Img_GetProperty(ImgObj, TNSOCR.IMG_PROP_WIDTH);
            h0 = NsOCR.Img_GetProperty(ImgObj, TNSOCR.IMG_PROP_HEIGHT);
            //current size
            NsOCR.Img_GetSize(ImgObj, out w, out h);

            lbSize.Text = "";
            if (dpi != 0)
                lbSize.Text = "DPI: " + dpi.ToString() + ", ";
            lbSize.Text = lbSize.Text + "BPP: " + bpp.ToString() + ", ";
            lbSize.Text = lbSize.Text + "Size: " + w0.ToString() + "x" + h0.ToString() + " => " + w.ToString() + "x" + h.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!IsImgLoaded()) return;
            int cnt = NsOCR.Img_GetPageCount(ImgObj);
            int n, w0, h0;
            if (!Int32.TryParse(edPage.Text, out n)) n = 1;
            n--;
            if (n < 0) n = 0;
            if (n >= cnt) n = cnt - 1;
            NsOCR.Img_SetPage(ImgObj, n);
            edPage.Text = (n + 1).ToString();
            bkSave.Enabled = false;

            // native size
            NsOCR.Img_GetSize(ImgObj, out w0, out h0);

            //now apply image scaling, binarize image, deskew etc,
            //everything except OCR itself
            NsOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_FIRST, TNSOCR.OCRSTEP_ZONING - 1, TNSOCR.OCRFLAG_NONE);

            int w, h;
            int res = NsOCR.Img_GetSize(ImgObj, out w, out h);
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Img_OCR", res);
            }

            ShowImageParams();

            AdjustDocScale();
        }

        private void PicBoxOnMouseDown(object sender, MouseEventArgs e)
        {
            if (!IsImgLoaded()) return;

            if (e.Button == MouseButtons.Right)
            {
                Point p = new Point(e.X, e.Y);

                float kk = GetCurDocScale();
                int cnt = NsOCR.Img_GetBlockCnt(ImgObj);
                int n = -1;
                int minsize = -1;
                int i, BlkObj, Xpos, Ypos, Width, Height, sz;
                Rectangle r = new Rectangle();
                for (i = 0; i < cnt; i++)
                {
                    NsOCR.Img_GetBlock(ImgObj, i, out BlkObj);
                    NsOCR.Blk_GetRect(BlkObj, out Xpos, out Ypos, out Width, out Height);
                    r.X = (int)(kk * Xpos);
                    r.Y = (int)(kk * Ypos);
                    r.Width = (int)(kk * Width);
                    r.Height = (int)(kk * Height);
                    
                    if (r.Contains(p))
                    {
                        //need to find smallest block because blocks may overlap
                        if (Width < Height) sz = Width;
                        else sz = Height;

                        if ((minsize == -1) || (sz < minsize))
                        {
                            minsize = sz;
                            n = i;
                        }
                    }
                }

                if (n == - 1) return; //block not found
                pmBlockTag = n; //remember block index
                
                p = PicBox.PointToScreen(p);
                pmBlock.Show(p.X, p.Y);
                return;
            }

            int w, h;
            NsOCR.Img_GetSize(ImgObj, out w, out h);

            Dwn = true;
            float k = GetCurDocScale();
            Frame.X = (int)(1 / k * e.X);
            if (Frame.X < 0) Frame.X = 0;
            if (Frame.X > w) Frame.X = w;
            Frame.Y = (int)(1 / k * e.Y);
            if (Frame.Y < 0) Frame.Y = 0;
            if (Frame.Y > h) Frame.Y = h;

            Frame.Width = 0;
            Frame.Height = 0;

            ShowImage();
        }

        private void PicBoxOnMouseUp(object sender, MouseEventArgs e)
        {
            int BlkObj, w, h, res;
            if (!Dwn) return;
            Dwn = false;

            if (!IsImgLoaded()) return;

            NsOCR.Img_GetSize(ImgObj, out w, out h);
            if (Frame.Right >= w) Frame.Width = w - Frame.Left - 1;
            if (Frame.Bottom >= h) Frame.Height = h - Frame.Top - 1;

            w = Frame.Right - Frame.Left + 1;
            h = Frame.Bottom - Frame.Top + 1;
            if ((w < 8) || (h < 8))
            {
                ShowImage();
                return;
            }
            res = NsOCR.Img_AddBlock(ImgObj, Frame.Left, Frame.Top, w, h, out BlkObj);
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Img_AddBlock", res);
                return;
            }

            //detect text block inversion
            NsOCR.Blk_Inversion(BlkObj, TNSOCR.BLK_INVERSE_DETECT);

            //detect text block rotation
            NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_DETECT);

            ShowImage();
        }

        private void PicBoxMouseMove(object sender, MouseEventArgs e)
        {
            GC.Collect();
            if (!Dwn) return;

            if (!IsImgLoaded()) return;

            int w, h;
            NsOCR.Img_GetSize(ImgObj, out w, out h);

            float k = GetCurDocScale();
            Frame.Width = (int)(1 / k * e.X) - Frame.Left + 1;
            if (Frame.Width < 0) Frame.Width = 0;
            if (Frame.Width > w) Frame.Width = w;

            Frame.Height = (int)(1 / k * e.Y) - Frame.Top + 1;
            if (Frame.Height < 0) Frame.Height = 0;
            if (Frame.Height > h) Frame.Height = h;

            ShowImage();
        }

        private void bkRecognize_Click(object sender, EventArgs e)
        {
            if (!IsImgLoaded())
            {
                MessageBox.Show("Image not loaded!");
                return;
            }

            ////
            bkRecognize.Enabled = false;
            bkSave.Enabled = false;
            lbWait.Visible = true;
            this.Refresh();

            bool InSameThread;
            int res;

            InSameThread = false; //perform OCR in non-blocking mode
            //InSameThread = true; //uncomment to perform OCR from this thread (GUI will be freezed)

            //perform OCR itself
            if (InSameThread)
                res = NsOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_ZONING, TNSOCR.OCRSTEP_LAST, TNSOCR.OCRFLAG_NONE);
            else
            {
               //do it in non-blocking mode and then wait for result
                res = NsOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_ZONING, TNSOCR.OCRSTEP_LAST, TNSOCR.OCRFLAG_THREAD);
                if (res > TNSOCR.ERROR_FIRST)
                {
                    ShowError("Ocr_OcrImg(1)", res);
                    return;
                }
                fmWait.mode = 0;
                fmWait.fmMain = this;
                fmWait.ShowDialog();
                res = fmWait.res;
            }

            lbWait.Visible = false;
            bkRecognize.Enabled = true;
            bkSave.Enabled = true;

            if (res > TNSOCR.ERROR_FIRST)
            {
                if (res == TNSOCR.ERROR_OPERATIONCANCELLED)
                    System.Windows.Forms.MessageBox.Show("Operation was cancelled.");
                else
                {
                    ShowError("Img_OCR", res);
                    return;
                }
            }

            ////	
            cbBin_CheckedChanged(this, null); //repaint img (binarized image could change)

            //int BlkObj;
            //NsOCR.Img_GetBlock(ImgObj, 0, out BlkObj);
            //NsOCR.Blk_SetWordText(BlkObj, 0, 0, "userwordtext");

            ShowText();
        }

        private void ShowText()
        {
            int flags = cbExactCopy.Checked ? TNSOCR.FMT_EXACTCOPY : TNSOCR.FMT_EDITCOPY;
            string txt;
            NsOCR.Img_GetImgText(ImgObj, out txt, flags);
            tbText.Text = txt;
        }

        private void cbScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NoEvent) return;
            bkRecognize.Enabled = false;
            bkFile_Click(null, null);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            ShowText();
        }

        private void pmBlock_Opening(object sender, CancelEventArgs e)
        {
            int BlkObj, BlockType, r, m;
            
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            BlockType = NsOCR.Blk_GetType(BlkObj);
            nnTypeOCRText.Checked = BlockType == TNSOCR.BT_OCRTEXT;
            nnTypeOCRDigit.Checked = BlockType == TNSOCR.BT_OCRDIGIT;
            nnTypeICRDigit.Checked = BlockType == TNSOCR.BT_ICRDIGIT;
            nnTypeBarCode.Checked = BlockType == TNSOCR.BT_BARCODE;
            nnTypeTable.Checked = BlockType == TNSOCR.BT_TABLE;
            nnTypePicture.Checked = BlockType == TNSOCR.BT_PICTURE;
            nnTypeClear.Checked = BlockType == TNSOCR.BT_CLEAR;
            nnTypeZoning.Checked = BlockType == TNSOCR.BT_ZONING;
            nnTypeMRZ.Checked = BlockType == TNSOCR.BT_MRZ;

            nnInvert.Checked = NsOCR.Blk_Inversion(BlkObj, TNSOCR.BLK_INVERSE_GET) != 0;
            r = NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_GET);
            nnRotate90.Checked = r == TNSOCR.BLK_ROTATE_270;
            nnRotate180.Checked = r == TNSOCR.BLK_ROTATE_180;
            nnRotate270.Checked = r == TNSOCR.BLK_ROTATE_90;
            m = NsOCR.Blk_Mirror(BlkObj, TNSOCR.BLK_MIRROR_GET);
            nnMirrorH.Checked = (m & TNSOCR.BLK_MIRROR_H) != 0;
            nnMirrorV.Checked = (m & TNSOCR.BLK_MIRROR_V) != 0;
        }

        private void nnTypeOCRText_Click(object sender, EventArgs e)
        {
            int BlkObj;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_OCRTEXT);
            ShowImage();
        }

        private void nnTypeICRDigit_Click(object sender, EventArgs e)
        {
            int BlkObj;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_ICRDIGIT);
            ShowImage();
        }

        private void nnTypePicture_Click(object sender, EventArgs e)
        {
            int BlkObj;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_PICTURE);
            ShowImage();
        }

        private void nnTypeClear_Click(object sender, EventArgs e)
        {
            int BlkObj;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_CLEAR);
            ShowImage();
        }

        private void nnInvert_Click(object sender, EventArgs e)
        {
            int BlkObj;
            nnInvert.Checked = !nnInvert.Checked;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            if (nnInvert.Checked) NsOCR.Blk_Inversion(BlkObj, TNSOCR.BLK_INVERSE_SET1);
            else NsOCR.Blk_Inversion(BlkObj, TNSOCR.BLK_INVERSE_SET0);
        }

        private void nnRotate90_Click(object sender, EventArgs e)
        {
            int BlkObj;
            nnRotate90.Checked = !nnRotate90.Checked;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            if (!nnRotate90.Checked) NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_NONE);
            else NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_270);
        }

        private void nnRotate180_Click(object sender, EventArgs e)
        {
            int BlkObj;
            nnRotate180.Checked = !nnRotate180.Checked;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            if (!nnRotate180.Checked) NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_NONE);
            else NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_180);
        }

        private void nnRotate270_Click(object sender, EventArgs e)
        {
            int BlkObj;
            nnRotate270.Checked = !nnRotate270.Checked;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            if (!nnRotate270.Checked) NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_NONE);
            else NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_90);
        }

        private void nnMirrorH_Click(object sender, EventArgs e)
        {
            int BlkObj;
            nnMirrorH.Checked = !nnMirrorH.Checked;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            int m = NsOCR.Blk_Mirror(BlkObj, -1);
            if (nnMirrorH.Checked) m = m | TNSOCR.BLK_MIRROR_H;
            else m = m & ~TNSOCR.BLK_MIRROR_H;
            NsOCR.Blk_Mirror(BlkObj, m);
        }

        private void nnMirrorV_Click(object sender, EventArgs e)
        {
            int BlkObj;
            nnMirrorV.Checked = !nnMirrorV.Checked;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            int m = NsOCR.Blk_Mirror(BlkObj, -1);
            if (nnMirrorV.Checked) m = m | TNSOCR.BLK_MIRROR_V;
            else m = m & ~TNSOCR.BLK_MIRROR_V;
            NsOCR.Blk_Mirror(BlkObj, m);
        }

        private void nnDeleteBlock_Click(object sender, EventArgs e)
        {
            int BlkObj;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            NsOCR.Img_DeleteBlock(ImgObj, BlkObj);
            ShowImage();
        }

        private void OnPanelResize(object sender, EventArgs e)
        {
            AdjustDocScale();
        }

        private void cbBin_CheckedChanged(object sender, EventArgs e)
        {
            AdjustDocScale(); //repaint
        }

        private void bkLoadBlocks_Click(object sender, EventArgs e)
        {
            if (opBlocks.ShowDialog() != DialogResult.OK) return;

            NsOCR.Img_DeleteAllBlocks(ImgObj); //note: Img_LoadBlocks does not remove existing blocks, so remove them here
            bkSave.Enabled = false;
            int res = NsOCR.Img_LoadBlocks(ImgObj, opBlocks.FileName);
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Img_LoadBlocks", res);
                return;
            }
            
            ShowImage();
        }

        private void bkSaveBlocks_Click(object sender, EventArgs e)
        {
            if (svBlocks.ShowDialog() != DialogResult.OK) return;

            int res = NsOCR.Img_SaveBlocks(ImgObj, svBlocks.FileName);
            if (res > TNSOCR.ERROR_FIRST) ShowError("Img_SaveBlocks", res);
        }

        private void bkClearBlocks_Click(object sender, EventArgs e)
        {
            NsOCR.Img_DeleteAllBlocks(ImgObj);
            bkSave.Enabled = false;
            ShowImage();
        }

        private void bkDetectBlocks_Click(object sender, EventArgs e)
        {
            NsOCR.Img_DeleteAllBlocks(ImgObj);
            NsOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_ZONING, TNSOCR.OCRSTEP_ZONING, TNSOCR.OCRFLAG_NONE);
            ShowImage();
        }

        private string GetDocName(string str)
        {
            string fn = Path.GetFileName(str); 
            return Path.ChangeExtension(fn, ".pdf");
        }

        private void bkSave_Click(object sender, EventArgs e)
        {
            SvrObj = 0;
            svFile.FileName = GetDocName(opImg.FileName);
            svFile.FilterIndex = 1;
            if (svFile.ShowDialog() != DialogResult.OK) return;
            
            int format = TNSOCR.SVR_FORMAT_PDF + (svFile.FilterIndex - 1);

            //image over text option for PDF
            if ((format == TNSOCR.SVR_FORMAT_PDF) || (format == TNSOCR.SVR_FORMAT_PDFA))
            {
                if (System.Windows.Forms.MessageBox.Show("Place page image over recognized text?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    NsOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "Saver/PDF/ImageLayer", "1");
                else
                    NsOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "Saver/PDF/ImageLayer", "0");
            }

            int res = NsOCR.Svr_Create(CfgObj, format, out SvrObj);
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Svr_Create", res);
                return;
            }

            int flags = cbExactCopy.Checked ? TNSOCR.FMT_EXACTCOPY : TNSOCR.FMT_EDITCOPY;

            NsOCR.Svr_NewDocument(SvrObj);

            if (sender == null) return; //caller is ProcessEntireDocument

            res = NsOCR.Svr_AddPage(SvrObj, ImgObj, flags);
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Svr_AddPage", res);
                NsOCR.Svr_Destroy(SvrObj);
                return;
            }

            if ((format == TNSOCR.SVR_FORMAT_PDF) || (format == TNSOCR.SVR_FORMAT_PDFA) || (format == TNSOCR.SVR_FORMAT_XML))  //demonstrate how to write PDF info
                NsOCR.Svr_SetDocumentInfo(SvrObj, TNSOCR.INFO_PDF_TITLE, "Sample Title");

            res = NsOCR.Svr_SaveToFile(SvrObj, svFile.FileName);

            //or save to memory
            // Array bytes;
            // NsOCR.Svr_SaveToMemory(SvrObj, out bytes);


            NsOCR.Svr_Destroy(SvrObj);

            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Svr_SaveToFile", res);
                return;
            }

            //open the file
            System.Diagnostics.Process.Start(svFile.FileName);
        }

        private void bkScan_Click(object sender, EventArgs e)
        {
            fmScan.fmMain = this;
            fmScan.ShowDialog();
        }

        private void nnTypeOCRDigit_Click(object sender, EventArgs e)
        {
            int BlkObj;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_OCRDIGIT);
            ShowImage();
        }

        private void nnTypeZoning_Click(object sender, EventArgs e)
        {
            int BlkObj;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_ZONING);
            ShowImage();
        }

        private void nnTypeBarCode_Click(object sender, EventArgs e)
        {
            int BlkObj;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_BARCODE);
            NsOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_BARCODE, "BarCode/SearchMode", "3"); //deep scanning for manual barcode zone
            ShowImage();
        }

        private void nnTypeTable_Click(object sender, EventArgs e)
        {
            int BlkObj;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_TABLE);
            ShowImage();
        }

        private void nnTypeMRZ_Click(object sender, EventArgs e)
        {
            int BlkObj;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_MRZ);
            ShowImage();
        }

        private void ProcessEntireDocument()
        {
            bkSave_Click(null, null);

            if (SvrObj == 0) return; //saving cancelled

            int OcrCnt, res;
            bool InSameThread;
            //recognize up to 4 images at once.
            //Note for large images ERROR_NOMEMORY can be raised
            //OcrCnt = 4;

            //Use number of logical CPU cores on the system for the best performance
            OcrCnt = 0;

            InSameThread = false; //perform OCR in non-blocking mode
            //InSameThread = true; //uncomment to perform OCR from this thread (GUI will be freezed)

            int flags = cbExactCopy.Checked ? TNSOCR.FMT_EXACTCOPY : TNSOCR.FMT_EDITCOPY;
            flags = flags << 8; //we need to shift FMT_XXXXX flags for this function

            //process all pages of input image and add results to SAVER object
            //this function will create (and then release) additional temporary OCR objects if OcrCnt > 1
            if (InSameThread)
            {
                res = NsOCR.Ocr_ProcessPages(ImgObj, SvrObj, 0, -1, OcrCnt, TNSOCR.OCRFLAG_NONE | flags);
            }
            else
            {
                //do it in non-blocking mode and then wait for result
                res = NsOCR.Ocr_ProcessPages(ImgObj, SvrObj, 0, -1, OcrCnt, TNSOCR.OCRFLAG_THREAD | flags);
                if (res > TNSOCR.ERROR_FIRST)
                {
                    ShowError("Ocr_ProcessPages(1)", res);
                    return;
                }
                fmWait.mode = 1;
                fmWait.fmMain = this;
                fmWait.ShowDialog();
                res = fmWait.res;
            }

            //restore option
            NsOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "Main/NumKernels", "0");

            if (res > TNSOCR.ERROR_FIRST)
            {
                if (res == TNSOCR.ERROR_OPERATIONCANCELLED)
                    System.Windows.Forms.MessageBox.Show("Operation was cancelled.");
                else
                    ShowError("Ocr_ProcessPages", res);
                NsOCR.Svr_Destroy(SvrObj);
                return;
            }

            //save output document
            res = NsOCR.Svr_SaveToFile(SvrObj, svFile.FileName);
            NsOCR.Svr_Destroy(SvrObj);

            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Svr_SaveToFile", res);
                return;
            }

            //open the file
            System.Diagnostics.Process.Start(svFile.FileName);
        }

        private void bkSelectLanguages_Click(object sender, EventArgs e)
        {
            fmLangs.fmMain = this;
            fmLangs.ShowDialog();
        }

        private void cbPixLines_CheckedChanged(object sender, EventArgs e)
        {
            AdjustDocScale(); //repaint
        }

        private void bkOptions_Click(object sender, EventArgs e)
        {
            fmOptions.fmMain = this;
            fmOptions.ShowDialog();
        }

        public void ShowHelp(string section)
        {
            string fn = "file://" + AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\..\\Docs\\NSOCR.chm";
            Help.ShowHelp(this, fn, section);
        }

        private void bkHelp_Click(object sender, EventArgs e)
        {
            ShowHelp("");
        }

        //helper
        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        private void nnSetRegExp_Click(object sender, EventArgs e)
        {
            int BlkObj;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            string s = "";
            if (InputBox("Set regular expression (for entire zone)", "New regular expression:", ref s) != DialogResult.OK)
                return;

            if (NsOCR.Blk_SetWordRegEx(BlkObj, -1, -1, s, TNSOCR.REGEX_SET) > 0)
                System.Windows.Forms.MessageBox.Show("Regular Expression syntax error!");
/*
            //set regular expression - the same expression for all words in block
            //regular expression for amounts like '123.00$', '500' or '-100,55'
            if (NsOCR.Blk_SetWordRegEx(BlkObj, -1, -1, "(|-)[0-9]+(\\.|,)?([0-9][0-9])?\\$?", TNSOCR.REGEX_SET) > 0)
                System.Windows.Forms.MessageBox.Show("Regular Expression syntax error!");
            string teststr;
            teststr = "-55.30$"; //test string
            if (NsOCR.Blk_SetWordRegEx(BlkObj, -1, -1, teststr, TNSOCR.REGEX_CHECK) != 1)
                System.Windows.Forms.MessageBox.Show("String does not comply with current regular expression!");
            else
                System.Windows.Forms.MessageBox.Show("String is good!");
 /**/ 
        }

        private void tbText2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
