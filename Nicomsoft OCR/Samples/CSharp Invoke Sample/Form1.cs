using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NSOCR_NameSpace;

namespace Sample
{
    public partial class Form1 : Form
    {
        public int CfgObj = 0;
        int OcrObj = 0;
        int ImgObj = 0;
        bool Dwn = false;
        Rectangle Frame;
        Image bmp;
        public Graphics gr;
        bool NoEvent;

        Form2 fmLangs = new Form2();

        public Form1()
        {
            InitializeComponent();
        }

        private void ShowError(string api, int err)
        {
            string s;
            s = api + " Error #" + err.ToString("X");
            System.Windows.Forms.MessageBox.Show(s);
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            //get NSOCR version
            StringBuilder val;
            val = new StringBuilder(256);
            TNSOCR.Engine_GetVersion(val); //if you get "BadImageFormatException" error here: find and check "LIBNAME" constant in "NSOCR.cs"
            Text = Text + " [ NSOCR version: " + val + " ] ";

            TNSOCR.Engine_SetLicenseKey(""); //required for licensed version only

            //init engine and create ocr-related objects
            TNSOCR.Engine_Initialize();
            TNSOCR.Cfg_Create(out CfgObj);
            TNSOCR.Ocr_Create(CfgObj, out OcrObj);
            TNSOCR.Img_Create(OcrObj, out ImgObj);

            TNSOCR.Cfg_LoadOptions(CfgObj, "Config.dat"); //load options, if path not specified, current folder and folder with NSOCR.dll will be checked

            bkRecognize.Enabled = false;
            gr = PicBox.CreateGraphics();

            NoEvent = true;
            cbScale.SelectedIndex = 0;
            NoEvent = false;
            bkSave.Enabled = false;
            
            //copy some settings to GUI
            val = new StringBuilder(256);
            if (TNSOCR.Cfg_GetOption(CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/AutoScale", val, 256) < TNSOCR.ERROR_FIRST)
                cbScale.Enabled = (val.ToString() == "1");

            //by default this option is disabled because it takes about 10% of total recognition time
            //enable it to demonstrate this feature
            TNSOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "Zoning/FindBarcodes", "1");
            //also enable auto-detection of image inversion
            TNSOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/Inversion", "2");
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            if (ImgObj != 0) TNSOCR.Img_Destroy(ImgObj);
            if (OcrObj != 0) TNSOCR.Ocr_Destroy(OcrObj);
            if (CfgObj != 0) TNSOCR.Cfg_Destroy(CfgObj);
            TNSOCR.Engine_Uninitialize();
        }

        public bool IsImgLoaded()
        {
            int w, h;
            if (TNSOCR.Img_GetSize(ImgObj, out w, out h) > TNSOCR.ERROR_FIRST) return false;
            return (w > 0) && (h > 0);
        }

        public float GetCurDocScale()
        {
            if (!IsImgLoaded()) return 1.0f;
            Rectangle r;
            r = splitContainer1.Bounds;
            int w = splitContainer1.SplitterDistance - 8;
            int h = r.Height - 8;

            int ww, hh;
            TNSOCR.Img_GetSize(ImgObj, out ww, out hh);
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

            int i, BlkObj, Xpos, Ypos, Width, Height;
            Pen pen = new Pen(Color.Green);
            int cnt = TNSOCR.Img_GetBlockCnt(ImgObj);
            for (i = 0; i < cnt; i++)
            {
                TNSOCR.Img_GetBlock(ImgObj, i, out BlkObj);
                TNSOCR.Blk_GetRect(BlkObj, out Xpos, out Ypos, out Width, out Height);
                r.X = (int)(k * Xpos);
                r.Y = (int)(k * Ypos);
                r.Width = (int)(k * Width);
                r.Height = (int)(k * Height);

                col = System.Drawing.Color.Red;
                switch (TNSOCR.Blk_GetType(BlkObj))
                {
                    case TNSOCR.BT_OCRTEXT: col = System.Drawing.Color.Green; break;
                    case TNSOCR.BT_OCRDIGIT: col = System.Drawing.Color.Lime; break;
                    case TNSOCR.BT_ICRDIGIT: col = System.Drawing.Color.Blue; break;
                    case TNSOCR.BT_BARCODE: col = System.Drawing.Color.Navy; break;
                    case TNSOCR.BT_PICTURE: col = System.Drawing.Color.Aqua; break;
                    case TNSOCR.BT_CLEAR: col = System.Drawing.Color.Gray; break;
                    case TNSOCR.BT_ZONING: col = System.Drawing.Color.Black; break;
                    case TNSOCR.BT_TABLE: col = System.Drawing.Color.Olive; break;
                }

                pen.Width = 2;
                pen.Color = col;
                graphic.DrawRectangle(pen, r);

                System.Drawing.Font fnt = new Font("Arial", 8);
                System.Drawing.Brush br = new SolidBrush(Color.Lime);
                System.Drawing.PointF pnt = new PointF(r.X, r.Y);
                r.Width = 12;
                r.Height = 14;
                graphic.FillRectangle(br, r);
                br = new SolidBrush(Color.Black);
                graphic.DrawString(i.ToString(), fnt, br, pnt);
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
            GC.Collect();
        }

        public void AdjustDocScale()
        {
            if (!IsImgLoaded()) return;
            int ww, hh;

            Rectangle r;
            r = splitContainer1.Bounds;
            ww = splitContainer1.SplitterDistance - 8;
            hh = r.Height - 8;

            bmp = new Bitmap(ww, hh, gr);
            Graphics gr2 = Graphics.FromImage(bmp);
            IntPtr dc = gr2.GetHdc();
            TNSOCR.Img_DrawToDC(ImgObj, (int)dc, 0, 0, ref ww, ref hh, TNSOCR.DRAW_NORMAL);
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
                    TNSOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/AutoScale", "1");
                    TNSOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/ScaleFactor", "1.0"); //default scale if cannot detect it automatically
                }
                else //fixed scaling
                {
                    TNSOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/AutoScale", "0");
                    string s = cbScale.Items[cbScale.SelectedIndex].ToString();
                    TNSOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/ScaleFactor", s);
                }
            }

            //refer to "C# Sample" project for other ways to load image to NSOCR
            int res = TNSOCR.Img_LoadFile(ImgObj, opImg.FileName);

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

            int w0, h0, w, h;
            //native size
            TNSOCR.Img_GetSize(ImgObj, out w0, out h0);

            //now apply image scaling, binarize image, deskew etc,
            //everything except zoning and OCR
            res = TNSOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_FIRST, TNSOCR.OCRSTEP_ZONING - 1, 0);
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Img_OCR", res);
                return;
            }

            //final size after pre-processing
            TNSOCR.Img_GetSize(ImgObj, out w, out h);
            lbSize.Text = "Size: " + w0.ToString() + "x" + h0.ToString() + " => " + w.ToString() + "x" + h.ToString();

            res = TNSOCR.Img_GetSkewAngle(ImgObj);
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Img_GetSkewAngle", res);
                lbSkew.Text = "";
            }
            else lbSkew.Text = "Skew angle (degrees): " + (res / 1000.0);

            //pixel lines info
            res = TNSOCR.Img_GetPixLineCnt(ImgObj);
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Img_GetPixLineCnt", res);
                return;
            }
            lbLines.Text = "Lines: " + res.ToString();

            edPage.Text = "1";
            lbPages.Text = "of " + TNSOCR.Img_GetPageCount(ImgObj).ToString();

            AdjustDocScale();

            bkRecognize.Enabled = true;
            bkSave.Enabled = false;
            tbText.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!IsImgLoaded()) return;
            int cnt = TNSOCR.Img_GetPageCount(ImgObj);
            int n;
            if (!Int32.TryParse(edPage.Text, out n)) n = 1;
            n--;
            if (n < 0) n = 0;
            if (n >= cnt) n = cnt - 1;
            TNSOCR.Img_SetPage(ImgObj, n);
            edPage.Text = (n + 1).ToString();
            bkSave.Enabled = false;

            //now apply image scaling, binarize image, deskew etc,
            //everything except OCR itself
            TNSOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_FIRST, TNSOCR.OCRSTEP_ZONING - 1, 0);

            AdjustDocScale();
        }

        private void PicBoxOnMouseDown(object sender, MouseEventArgs e)
        {
            if (!IsImgLoaded()) return;

            int w, h;
            TNSOCR.Img_GetSize(ImgObj, out w, out h);

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
            Dwn = false;
        }

        private void PicBoxMouseMove(object sender, MouseEventArgs e)
        {
            GC.Collect();
            if (!Dwn) return;

            if (!IsImgLoaded()) return;

            int w, h;
            TNSOCR.Img_GetSize(ImgObj, out w, out h);

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

            TNSOCR.Img_DeleteAllBlocks(ImgObj);

            int res;
            if (Frame.Width > 0)
            {
                Rectangle r = Frame;
                int w, h;
                TNSOCR.Img_GetSize(ImgObj, out w, out h);

                if (r.X < 0) r.X = 0;
                if (r.Y < 0) r.Y = 0;
                if (r.Width > w) r.Width = w;
                if (r.Height > h) r.Height = h;

                int BlkObj;
                res = TNSOCR.Img_AddBlock(ImgObj, r.Left, r.Top, r.Width, r.Height, out BlkObj);
                if (res > TNSOCR.ERROR_FIRST)
                {
                    ShowError("Img_AddBlock", res);
                    return;
                }

                //detect text block inversion
                TNSOCR.Blk_Inversion(BlkObj, TNSOCR.BLK_INVERSE_DETECT);

                Frame.Width = 0;
            }

            ////
            bkRecognize.Enabled = false;
            lbWait.Visible = true;
            bkSave.Enabled = false;
            this.Refresh();

            //zoning will not be executed since we have created text block
            res = TNSOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_ZONING, TNSOCR.OCRSTEP_LAST, 0);
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Img_OCR", res);
                return;
            }

            lbWait.Visible = false;
            bkRecognize.Enabled = true;
            bkSave.Enabled = true;
            ////	

            int flags = TNSOCR.FMT_EDITCOPY; //or TNSOCR.FMT_EXACTCOPY
            StringBuilder txt = new StringBuilder(0);
            int n = TNSOCR.Img_GetImgText(ImgObj, txt, 0, flags); //get buffer text length plus null-terminated zero
            txt = new StringBuilder(n + 1);
            TNSOCR.Img_GetImgText(ImgObj, txt, n + 1, flags);
            tbText.Text = txt.ToString();

            ShowImage();
        }

        private void PicBoxResized(object sender, EventArgs e)
        {
            AdjustDocScale();
        }

        private void cbScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (NoEvent) return;
            bkRecognize.Enabled = false;
            bkFile_Click(null, null);
        }

        private string GetDocName(string str)
        {
            string fn = Path.GetFileName(str);
            return Path.ChangeExtension(fn, ".pdf");
        }

        private void bkSave_Click(object sender, EventArgs e)
        {
            svFile.FileName = GetDocName(opImg.FileName);
            svFile.FilterIndex = 1;
            if (svFile.ShowDialog() != DialogResult.OK) return;

            int format = TNSOCR.SVR_FORMAT_PDF + (svFile.FilterIndex - 1);
            int SvrObj;
            int res = TNSOCR.Svr_Create(CfgObj, format, out SvrObj);
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Svr_Create", res);
                return;
            }

            TNSOCR.Svr_NewDocument(SvrObj);
            res = TNSOCR.Svr_AddPage(SvrObj, ImgObj, 0);
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Svr_AddPage", res);
                TNSOCR.Svr_Destroy(SvrObj);
                return;
            }

            res = TNSOCR.Svr_SaveToFile(SvrObj, svFile.FileName);
            TNSOCR.Svr_Destroy(SvrObj);

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
    }
}
