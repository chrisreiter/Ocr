using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using NSOCR_NameSpace; //Add NSOCR namespace from "NSOCR.cs"

namespace Sample
{

    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        //NSOCRLib COM was added via "Project / Add Reference" dialog
        public NSOCRLib.NSOCRClass NsOCR; //declare only, an instance will be created later, in "OnFormLoad"

        public int CfgObj = 0;
        public int OcrObj = 0;
        public int ImgObj = 0;
        public int ScanObj = 0;
        public int SvrObj = 0;
        bool Dwn = false;
        Rect Frame;

        bool NoEvent = true;
        bool Inited = false;

        Microsoft.Win32.SaveFileDialog svFile = new Microsoft.Win32.SaveFileDialog();

        List<System.Windows.Shapes.Rectangle> Rects = new List<System.Windows.Shapes.Rectangle>();
        List<System.Windows.Controls.Label> Labels = new List<System.Windows.Controls.Label>();

        Microsoft.Win32.OpenFileDialog opImg = new Microsoft.Win32.OpenFileDialog();    

        ContextMenu pMenu = new ContextMenu();
        MenuItem nnTypeOCRText = new MenuItem();
        MenuItem nnTypeOCRDigit = new MenuItem();
        MenuItem nnTypeICRDigit = new MenuItem();
        MenuItem nnTypeBarCode = new MenuItem();
        MenuItem nnTypeTable = new MenuItem();
        MenuItem nnTypePicture = new MenuItem();
        MenuItem nnTypeClear = new MenuItem();
        MenuItem nnTypeZoning = new MenuItem();
        MenuItem nnTypeMRZ = new MenuItem();
        MenuItem nnInvert = new MenuItem();
        MenuItem nnRotate90 = new MenuItem();
        MenuItem nnRotate180 = new MenuItem();
        MenuItem nnRotate270 = new MenuItem();
        MenuItem nnMirrorH = new MenuItem();
        MenuItem nnMirrorV = new MenuItem();
        MenuItem nnDelete = new MenuItem();
        int pmBlockTag;

        public Window1()
        {
            InitializeComponent();
        }

        public void ShowError(string api, int err)
        {
            string s;
            s = api + " Error #" + err.ToString("X");
            System.Windows.MessageBox.Show(s);
        }

        private void fmMain_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                NsOCR = new NSOCRLib.NSOCRClass();
            }
            catch (Exception exc) //some error
            {
                string msg = "Cannot create NSOCR COM object instance. Possible reasons:\r\n - NSOCR.dll is missed.\r\n - NSOCR.dll was not registered with Regsvr32.\r\n - NSOCR.dll is x32 but this application is x64.\r\n";
                msg = msg + "\r\n Exception message:\r\n\r\n" + exc.Message;
                System.Windows.MessageBox.Show(msg);
                Close();
                return;
            }

            Inited = true; //NSOCR instance created successfully

            //get NSOCR version
            string val;
            NsOCR.Engine_GetVersion(out val);
            Title = Title + " [ NSOCR version: " + val + " ] ";
            opImg.Filter = "Image Files (bmp, jpg, tif, png, gif, pdf)|*.bmp;*.jpg;*.tif;*.png;*.gif;*.pdf|All Files|*";

            NsOCR.Engine_SetLicenseKey(""); //required for licensed version only

            NsOCR.Engine_Initialize();

            NsOCR.Cfg_Create(out CfgObj);
            NsOCR.Ocr_Create(CfgObj, out OcrObj);
            NsOCR.Img_Create(OcrObj, out ImgObj);
            NsOCR.Scan_Create(CfgObj, out ScanObj);

            NsOCR.Cfg_LoadOptions(CfgObj, "Config.dat"); //load options, if path not specified, current folder and folder with NSOCR.dll will be checked

            bkRecognize.IsEnabled = false;
            //gr = PicBox.CreateGraphics();

            NoEvent = true;
            cbScale.SelectedIndex = 0;
            NoEvent = false;
            lbWait.Visibility = System.Windows.Visibility.Hidden;
            bkSave.IsEnabled = false;

            rFrame.Height = 0;

            //copy some settings to GUI
            if (NsOCR.Cfg_GetOption(CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/AutoScale", out val) < TNSOCR.ERROR_FIRST)
                cbScale.IsEnabled = (val == "1");

            ////popup menu
            Separator sp = new Separator();

            nnTypeOCRText.Header = "Type: OCR Text";
            nnTypeOCRText.Click += new RoutedEventHandler(mmSetOcrType_Click);
            pMenu.Items.Add(nnTypeOCRText);

            nnTypeOCRDigit.Header = "Type: Machine-printed Digits";
            nnTypeOCRDigit.Click += new RoutedEventHandler(mmSetOcrDgtType_Click);
            pMenu.Items.Add(nnTypeOCRDigit);

            nnTypeICRDigit.Header = "Type: Handwritten Digits";
            nnTypeICRDigit.Click += new RoutedEventHandler(mmSetIcrDgtType_Click);
            pMenu.Items.Add(nnTypeICRDigit);

            nnTypeBarCode.Header = "Type: Bar Code";
            nnTypeBarCode.Click += new RoutedEventHandler(mmSetTypeBarCodeType_Click);
            pMenu.Items.Add(nnTypeBarCode);

            nnTypeTable.Header = "Type: Table";
            nnTypeTable.Click += new RoutedEventHandler(mmSetTypeTableType_Click);
            pMenu.Items.Add(nnTypeTable);

            nnTypePicture.Header = "Type: Picture";
            nnTypePicture.Click += new RoutedEventHandler(mmSetPicType_Click);
            pMenu.Items.Add(nnTypePicture);

            nnTypeClear.Header = "Type: Clear Area";
            nnTypeClear.Click += new RoutedEventHandler(mmSetClearType_Click);
            pMenu.Items.Add(nnTypeClear);

            nnTypeZoning.Header = "Type: Zoning Area";
            nnTypeZoning.Click += new RoutedEventHandler(mmSetZoningType_Click);
            pMenu.Items.Add(nnTypeZoning);

            nnTypeMRZ.Header = "Type: MRZ";
            nnTypeMRZ.Click += new RoutedEventHandler(mmSetMRZType_Click);
            pMenu.Items.Add(nnTypeMRZ);

            pMenu.Items.Add(sp);

            nnInvert.Header = "Image Inverted";
            nnInvert.Click += new RoutedEventHandler(mmInvert_Click);
            pMenu.Items.Add(nnInvert);

            nnRotate90.Header = "Image Rotated 90° clockwise";
            nnRotate90.Click += new RoutedEventHandler(mmRotate90_Click);
            pMenu.Items.Add(nnRotate90);

            nnRotate270.Header = "Image Rotated 90° counterclockwise";
            nnRotate270.Click += new RoutedEventHandler(mmRotate270_Click);
            pMenu.Items.Add(nnRotate270);

            nnRotate180.Header = "Image Rotated 180°";
            nnRotate180.Click += new RoutedEventHandler(mmRotate180_Click);
            pMenu.Items.Add(nnRotate180);

            nnMirrorH.Header = "Image Mirrored Horizontally";
            nnMirrorH.Click += new RoutedEventHandler(mmMirrorH_Click);
            pMenu.Items.Add(nnMirrorH);

            nnMirrorV.Header = "Image Mirrored Vertically";
            nnMirrorV.Click += new RoutedEventHandler(mmMirrorV_Click);
            pMenu.Items.Add(nnMirrorV);

            sp = new Separator();      
            pMenu.Items.Add(sp);

            nnDelete.Header = "Delete Block(Zone)";
            nnDelete.Click += new RoutedEventHandler(mmDelete_Click);
            pMenu.Items.Add(nnDelete);

            pMenu.Opened +=new RoutedEventHandler(pMenu_Opened);
        }

        void pMenu_Opened(object sender, RoutedEventArgs e)
        {
            Rectangle rr = pMenu.PlacementTarget as Rectangle;
            pmBlockTag = (int)rr.Tag;

            int BlkObj, BlockType, r, m;

            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            BlockType = NsOCR.Blk_GetType(BlkObj);
            nnTypeOCRText.IsChecked = BlockType == TNSOCR.BT_OCRTEXT;
            nnTypeOCRDigit.IsChecked = BlockType == TNSOCR.BT_OCRDIGIT;
            nnTypeICRDigit.IsChecked = BlockType == TNSOCR.BT_ICRDIGIT;
            nnTypeBarCode.IsChecked = BlockType == TNSOCR.BT_BARCODE;
            nnTypePicture.IsChecked = BlockType == TNSOCR.BT_PICTURE;
            nnTypeClear.IsChecked = BlockType == TNSOCR.BT_CLEAR;
            nnTypeZoning.IsChecked = BlockType == TNSOCR.BT_ZONING;
            nnTypeTable.IsChecked = BlockType == TNSOCR.BT_TABLE;
            nnTypeMRZ.IsChecked = BlockType == TNSOCR.BT_MRZ;

            nnInvert.IsChecked = NsOCR.Blk_Inversion(BlkObj, TNSOCR.BLK_INVERSE_GET) != 0;
            r = NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_GET);
            nnRotate90.IsChecked = r == TNSOCR.BLK_ROTATE_270;
            nnRotate180.IsChecked = r == TNSOCR.BLK_ROTATE_180;
            nnRotate270.IsChecked = r == TNSOCR.BLK_ROTATE_90;
            m = NsOCR.Blk_Mirror(BlkObj, TNSOCR.BLK_MIRROR_GET);
            nnMirrorH.IsChecked = (m & TNSOCR.BLK_MIRROR_H) != 0;
            nnMirrorV.IsChecked = (m & TNSOCR.BLK_MIRROR_V) != 0;
        }
        void mmSetOcrType_Click(object sender, RoutedEventArgs e)
        {
            int BlkObj;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_OCRTEXT);
            ShowImage();
        }
        void mmSetOcrDgtType_Click(object sender, RoutedEventArgs e)
        {
            int BlkObj;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_OCRDIGIT);
            ShowImage();
        }
        void mmSetIcrDgtType_Click(object sender, RoutedEventArgs e)
        {
            int BlkObj;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_ICRDIGIT);
            ShowImage();
        }
        void mmSetTypeBarCodeType_Click(object sender, RoutedEventArgs e)
        {
            int BlkObj;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_BARCODE);
            NsOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_BARCODE, "BarCode/SearchMode", "3"); //deep scanning for manual barcode zone
            ShowImage();
        }
        void mmSetTypeTableType_Click(object sender, RoutedEventArgs e)
        {
            int BlkObj;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_TABLE);
            ShowImage();
        }
        void mmSetPicType_Click(object sender, RoutedEventArgs e)
        {
            int BlkObj;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_PICTURE);
            ShowImage();
        }
        void mmSetClearType_Click(object sender, RoutedEventArgs e)
        {
            int BlkObj;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_CLEAR);
            ShowImage();
        }
        void mmSetZoningType_Click(object sender, RoutedEventArgs e)
        {
            int BlkObj;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_ZONING);
            ShowImage();
        }
        void mmSetMRZType_Click(object sender, RoutedEventArgs e)
        {
            int BlkObj;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            NsOCR.Blk_SetType(BlkObj, TNSOCR.BT_MRZ);
            ShowImage();
        }
        void mmInvert_Click(object sender, RoutedEventArgs e)
        {
            int BlkObj;
            nnInvert.IsChecked = !nnInvert.IsChecked;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            if (nnInvert.IsChecked) NsOCR.Blk_Inversion(BlkObj, TNSOCR.BLK_INVERSE_SET1);
            else NsOCR.Blk_Inversion(BlkObj, TNSOCR.BLK_INVERSE_SET0);
        }
        void mmRotate90_Click(object sender, RoutedEventArgs e)
        {
            int BlkObj;
            nnRotate90.IsChecked = !nnRotate90.IsChecked;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            if (!nnRotate90.IsChecked) NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_NONE);
            else NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_270);
        }
        void mmRotate180_Click(object sender, RoutedEventArgs e)
        {
            int BlkObj;
            nnRotate180.IsChecked = !nnRotate180.IsChecked;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            if (!nnRotate180.IsChecked) NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_NONE);
            else NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_180);
        }
        void mmRotate270_Click(object sender, RoutedEventArgs e)
        {
            int BlkObj;
            nnRotate270.IsChecked = !nnRotate270.IsChecked;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            if (!nnRotate270.IsChecked) NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_NONE);
            else NsOCR.Blk_Rotation(BlkObj, TNSOCR.BLK_ROTATE_90);
        }
        void mmMirrorH_Click(object sender, RoutedEventArgs e)
        {
            int BlkObj;
            nnMirrorH.IsChecked = !nnMirrorH.IsChecked;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            int m = NsOCR.Blk_Mirror(BlkObj, -1);
            if (nnMirrorH.IsChecked) m = m | TNSOCR.BLK_MIRROR_H;
            else m = m & ~TNSOCR.BLK_MIRROR_H;
            NsOCR.Blk_Mirror(BlkObj, m);
        }
        void mmMirrorV_Click(object sender, RoutedEventArgs e)
        {
            int BlkObj;
            nnMirrorV.IsChecked = !nnMirrorV.IsChecked;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            int m = NsOCR.Blk_Mirror(BlkObj, -1);
            if (nnMirrorV.IsChecked) m = m | TNSOCR.BLK_MIRROR_V;
            else m = m & ~TNSOCR.BLK_MIRROR_V;
            NsOCR.Blk_Mirror(BlkObj, m);
        }
        void mmDelete_Click(object sender, RoutedEventArgs e)
        {
            int BlkObj;
            NsOCR.Img_GetBlock(ImgObj, pmBlockTag, out BlkObj);
            NsOCR.Img_DeleteBlock(ImgObj, BlkObj);
            ShowImage();
        }

        private void fmMain_Closed(object sender, EventArgs e)
        {
            if (Inited)
            {
                if (ImgObj != 0) NsOCR.Img_Destroy(ImgObj);
                if (OcrObj != 0) NsOCR.Ocr_Destroy(OcrObj);
                if (CfgObj != 0) NsOCR.Cfg_Destroy(CfgObj);
                if (ScanObj != 0) NsOCR.Scan_Destroy(ScanObj);
                NsOCR.Engine_Uninitialize();
                
                Application.Current.Shutdown(); //against WPF bug
            }
        }

        private void OpenURL(string url)
        {
            System.Diagnostics.Process.Start(url);
        }

        private void bkOpen_Click(object sender, RoutedEventArgs e)
        {
            if (opImg.ShowDialog() != true) return;

            if (cbScale.IsEnabled)
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

            //refer to "C# Sample" project for other ways to load image to NSOCR
            int res = NsOCR.Img_LoadFile(ImgObj, opImg.FileName);

            if (res > TNSOCR.ERROR_FIRST)
            {
                if (res == TNSOCR.ERROR_CANNOTLOADGS) //cannot load GhostScript to support PDF
                {
                    string s = "GhostSript library is needed to support PDF files. Just download and install it with default settings. Do you want to download GhostScript now?";
                    if (System.Windows.MessageBox.Show(s, "", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
                    OpenURL("http://www.nicomsoft.com/files/ocr/ghostscript.htm");
                }
                else ShowError("Img_LoadFile", res);
                return;
            }

            DoImageLoaded();
        }

        public void GetPictureBitmap(out BitmapSource bmp1)
        {
            Array bitsArray;
            int bpp;
            int w, h, w0, h0;

            w = (int)fmMain.ActualWidth / 2;
            h = (int)fmMain.ActualHeight - 145;
            w0 = w;
            h0 = h;

            if (cbBin.IsChecked == true)
            {
                bpp = NsOCR.Img_GetBmpData(ImgObj, out bitsArray, ref w, ref h, TNSOCR.DRAW_BINARY | TNSOCR.DRAW_GETBPP); //bits per pixel
                w = w0; //w and h were changed to actually returned image size, return them back
                h = h0;
                NsOCR.Img_GetBmpData(ImgObj, out bitsArray, ref w, ref h, TNSOCR.DRAW_BINARY);
            }
            else
            {
                bpp = NsOCR.Img_GetBmpData(ImgObj, out bitsArray, ref w, ref h, TNSOCR.DRAW_NORMAL | TNSOCR.DRAW_GETBPP); //bits per pixel
                w = w0; //w and h were changed to actually returned image size, return them back
                h = h0;
                NsOCR.Img_GetBmpData(ImgObj, out bitsArray, ref w, ref h, TNSOCR.DRAW_NORMAL);
            }

            //now w and h contain actual returned picture size, image size could be changed because OCR cares about image proportions
            int stride = (bpp / 8) * w; //bytes, in one line
            while (4 * (stride / 4) != stride)
                stride++; //lines in bmp are aligned by 4bytes

            if (bpp == 8)
                bmp1 = BitmapSource.Create(w, h, 96, 96, PixelFormats.Gray8, null, bitsArray, stride);
            else
                bmp1 = BitmapSource.Create(w, h, 96, 96, PixelFormats.Bgr24, null, bitsArray, stride);
            ////
        }

        public void DoImageLoaded()
        {
            int res;

            //check if image has multiple page and ask user if he wants process and save all pages automatically
            res = NsOCR.Img_GetPageCount(ImgObj);
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Img_GetPageCount", res);
                return;
            }
            if (res > 1)
                if (System.Windows.MessageBox.Show("Image contains multiple pages (" + res.ToString() + "). Do you want to process and save all pages automatically?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    ProcessEntireDocument();
                    bkRecognize.IsEnabled = false;
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

            edPage.Text = "1";
            lbPages.Content = "of " + NsOCR.Img_GetPageCount(ImgObj).ToString();
            bkSave.IsEnabled = false;

            Frame.X = 0;
            Frame.Y = 0;
            Frame.Width = 0;
            Frame.Height = 0;

            fmMain_SizeChanged(null, null);

            bkRecognize.IsEnabled = true;
            edText.Text = "";
            bkLoadBlocks.IsEnabled = true;
            bkSaveBlocks.IsEnabled = true;
            bkClearBlocks.IsEnabled = true;
            bkDetectBlocks.IsEnabled = true;

            AdjustDocScale();
        }

        public bool IsImgLoaded()
        {
            int w, h;
            if (NsOCR.Img_GetSize(ImgObj, out w, out h) > TNSOCR.ERROR_FIRST) return false;
            return (w > 0) && (h > 0);
        }

        public float GetCurDocScale()
        {
            if (!IsImgLoaded()) return 1.0f;

            int w = (int)fmMain.ActualWidth / 2;
            int h = (int)fmMain.ActualHeight - 145;

            int ww, hh;
            NsOCR.Img_GetSize(ImgObj, out ww, out hh);
            float kX = (float)w / ww;
            float kY = (float)h / hh;
            float k;
            if (kX > kY) k = kY;
            else k = kX;

            return k;
        }

        public void CreateRectangles()
        {
            //remove all rects
            int i;
            for (i = 0; i < Rects.Count; i++)
            {
                edCanvas.Children.Remove(Rects[i]);
                edCanvas.Children.Remove(Labels[i]);
            }
            Rects.Clear();
            Labels.Clear();

            if (!IsImgLoaded()) return;
            float k = GetCurDocScale();

            int cnt, BlkObj, Xpos, Ypos, Width, Height;
            cnt = NsOCR.Img_GetBlockCnt(ImgObj);
            for (i = 0; i < cnt; i++)
            {
                NsOCR.Img_GetBlock(ImgObj, i, out BlkObj);
                NsOCR.Blk_GetRect(BlkObj, out Xpos, out Ypos, out Width, out Height);

                Rect r = new Rect();
                r.X = (int)(k * Xpos);
                r.Y = (int)(k * Ypos);
                r.Width = (int)(k * Width);
                r.Height = (int)(k * Height);

                Color col = Colors.Red;
                switch (NsOCR.Blk_GetType(BlkObj))
                {
                    case TNSOCR.BT_OCRTEXT: col = Colors.Green; break;
                    case TNSOCR.BT_OCRDIGIT: col = Colors.Lime; break;
                    case TNSOCR.BT_ICRDIGIT: col = Colors.Blue; break;
                    case TNSOCR.BT_BARCODE: col = Colors.Navy; break;
                    case TNSOCR.BT_PICTURE: col = Colors.Aqua; break;
                    case TNSOCR.BT_CLEAR: col = Colors.Gray; break;
                    case TNSOCR.BT_ZONING: col = Colors.Black; break;
                    case TNSOCR.BT_TABLE: col = Colors.Olive; break;
                    case TNSOCR.BT_MRZ: col = Colors.Black; break;
                }

                Rectangle rr = new Rectangle();

                Canvas.SetLeft(rr, r.Left);
                Canvas.SetTop(rr, r.Top);

                rr.Width = r.Width;
                rr.Height = r.Height;
                SolidColorBrush myBrush = new SolidColorBrush(Colors.Transparent);
                rr.Stroke = new SolidColorBrush(col);
                rr.StrokeThickness = 2;
                rr.Fill = myBrush;
                rr.Tag = i;
                rr.ContextMenu = pMenu;
                edCanvas.Children.Add(rr);

                Label lb = new Label();
                lb.FontSize = 8;
                lb.Content = i.ToString();
                lb.Background = new SolidColorBrush(Colors.Lime); 
                Canvas.SetLeft(lb, r.Left);
                Canvas.SetTop(lb, r.Top);
                edCanvas.Children.Add(lb);

                Rects.Add(rr);
                Labels.Add(lb);
            }            
        }

        public void ShowImage()
        {
            if (!IsImgLoaded()) return;
            float k = GetCurDocScale();

            //draw blocks
            CreateRectangles();

            //user is creating new block, draw its frame
            if (Dwn)
            {
                Rect r = new Rect();
                r.X = (int)(k * Frame.Left);
                r.Y = (int)(k * Frame.Top);
                r.Width = (int)(k * Frame.Width);
                r.Height = (int)(k * Frame.Height);

                int w, h;
                NsOCR.Img_GetSize(ImgObj, out w, out h);

                if (r.Width >= w) r.Width = w - 1;
                if (r.Height >= h) r.Height = h - 1;

                Canvas.SetLeft(rFrame, r.Left);
                Canvas.SetTop(rFrame, r.Top);

                rFrame.Width = r.Right - r.Left;
                rFrame.Height = r.Bottom - r.Top;
            }
            else
            {
                rFrame.Width = 0;
                rFrame.Height = 0;
            }
        }

        public void AdjustDocScale()
        {
            if (!IsImgLoaded()) return;

            //get image
            BitmapSource bmp1;
            GetPictureBitmap(out bmp1);
      
            imDoc.Source = bmp1;
            ShowImage();

            GC.Collect();
        }

        private void fmMain_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!Inited) return;

            imDoc.Height = fmMain.ActualHeight - 145;
            imDoc.Width = fmMain.ActualWidth / 2;
            Canvas.SetLeft(edText, fmMain.ActualWidth / 2 + 5);
            Canvas.SetTop(cbBin, fmMain.ActualHeight - 135);
            Canvas.SetTop(bkLoadBlocks, fmMain.ActualHeight - 140);
            Canvas.SetTop(bkSaveBlocks, fmMain.ActualHeight - 140);
            Canvas.SetTop(bkClearBlocks, fmMain.ActualHeight - 140);
            Canvas.SetTop(bkDetectBlocks, fmMain.ActualHeight - 140);
            edText.Width = fmMain.ActualWidth / 2 - 35;
            edText.Height = fmMain.ActualHeight - 115;
            AdjustDocScale();
        }

        private void imDoc_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsImgLoaded()) return;

            if (e.LeftButton != MouseButtonState.Pressed) return;

            int w, h;
            NsOCR.Img_GetSize(ImgObj, out w, out h);

            Point position = e.GetPosition(imDoc);
            Dwn = true;
            float k = GetCurDocScale();
            Frame.X = (int)(1 / k * position.X);
            if (Frame.X < 0) Frame.X = 0;
            if (Frame.X > w) Frame.X = w;
            Frame.Y = (int)(1 / k * position.Y);
            if (Frame.Y < 0) Frame.Y = 0;
            if (Frame.Y > h) Frame.Y = h;

            Frame.Width = 0;
            Frame.Height = 0;

            ShowImage();
        }

        private void imDoc_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Dwn) return;

            if (!IsImgLoaded()) return;

            int w, h;
            NsOCR.Img_GetSize(ImgObj, out w, out h);

            Point position = e.GetPosition(imDoc);
            float k = GetCurDocScale();
            int ww = (int)((1 / k * position.X) - Frame.Left + 1);
            if (ww < 0) ww = 0;
            if (ww > w) ww = w;
            Frame.Width = ww;

            int hh = (int)((1 / k * position.Y) - Frame.Top + 1);
            if (hh < 0) hh = 0;
            if (hh > h) hh = h;
            Frame.Height = hh;

            ShowImage();
        }

        private void imDoc_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int BlkObj, w, h, res;
            if (!Dwn) return;
            Dwn = false;

            if (!IsImgLoaded()) return;

            NsOCR.Img_GetSize(ImgObj, out w, out h);
            if (Frame.Right >= w) Frame.Width = w - Frame.Left - 1;
            if (Frame.Bottom >= h) Frame.Height = h - Frame.Top - 1;

            w = (int)(Frame.Right - Frame.Left + 1);
            h = (int)(Frame.Bottom - Frame.Top + 1);
            if ((w < 8) || (h < 8))
            {
                ShowImage();
                return;
            }
            res = NsOCR.Img_AddBlock(ImgObj, (int)Frame.Left, (int)Frame.Top, w, h, out BlkObj);
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

        public delegate void emptyDelegate();

        private void bkRecognize_Click(object sender, RoutedEventArgs e)
        {
            if (!IsImgLoaded())
            {
                MessageBox.Show("Image not loaded!");
                return;
            }

            ////
            bkRecognize.IsEnabled = false;
            lbWait.Visibility = System.Windows.Visibility.Visible;
            bkSave.IsEnabled = false;

            //WPF trick to repaint a label
            lbWait.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Render, (emptyDelegate)delegate { });

            bool InSameThread;
            int res;

            InSameThread = false; //perform OCR in non-blocking mode
            //InSameThread = true; //uncomment to perform OCR from this thread (GUI will be freezed)

            //perform OCR itself
            if (InSameThread)
                res = NsOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_ZONING, TNSOCR.OCRSTEP_LAST, TNSOCR.OCRFLAG_NONE);
            else
            {
                Window3 fmWait = new Window3();

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

            lbWait.Visibility = System.Windows.Visibility.Hidden;
            bkRecognize.IsEnabled = true;
            bkSave.IsEnabled = true;

            if (res > TNSOCR.ERROR_FIRST)
            {
                if (res == TNSOCR.ERROR_OPERATIONCANCELLED)
                    System.Windows.MessageBox.Show("Operation was cancelled.");
                else
                {
                    ShowError("Img_OCR", res);
                    return;
                }
            }

            ////	

            cbBin_Click(null, null); //repaint img (binarized image could change)

            ShowText();
        }

        private void ShowText()
        {
            string txt = new string(' ', 0);
            int flags;
            flags = (cbExactCopy.IsChecked == true) ? flags = TNSOCR.FMT_EXACTCOPY : flags = TNSOCR.FMT_EDITCOPY;
            NsOCR.Img_GetImgText(ImgObj, out txt, flags); //get buffer text length plus null-terminated zero
            edText.Text = txt;
        }

        private void bkSetPage_Click(object sender, RoutedEventArgs e)
        {
            if (!IsImgLoaded()) return;
            int cnt = NsOCR.Img_GetPageCount(ImgObj);
            int n;
            if (!Int32.TryParse(edPage.Text, out n)) n = 1;
            n--;
            if (n < 0) n = 0;
            if (n >= cnt) n = cnt - 1;
            NsOCR.Img_SetPage(ImgObj, n);
            edPage.Text = (n + 1).ToString();
            bkSave.IsEnabled = false;

            //now apply image scaling, binarize image, deskew etc,
            //everything except OCR itself
            NsOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_FIRST, TNSOCR.OCRSTEP_ZONING - 1, TNSOCR.OCRFLAG_NONE);

            int w, h;
            NsOCR.Img_GetSize(ImgObj, out w, out h);

            Frame.X = 0;
            Frame.Y = 0;
            Frame.Width = w;
            Frame.Height = h;
            AdjustDocScale();
        }

        private void cbScale_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NoEvent) return;
            bkRecognize.IsEnabled = false;
            bkOpen_Click(null, null);
        }

        private void cbExactCopy_Click(object sender, RoutedEventArgs e)
        {
            ShowText();
        }

        private void cbBin_Click(object sender, RoutedEventArgs e)
        {
            AdjustDocScale(); //repaint
        }

        private void bkLoadBlocks_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog opBlocks = new Microsoft.Win32.OpenFileDialog();
            if (opBlocks.ShowDialog() != true) return;

            NsOCR.Img_DeleteAllBlocks(ImgObj); //note: Img_LoadBlocks does not remove existing blocks, so remove them here
            bkSave.IsEnabled = false;
            int res = NsOCR.Img_LoadBlocks(ImgObj, opBlocks.FileName);
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Img_LoadBlocks", res);
                return;
            }
            ShowImage();
        }

        private void bkSaveBlocks_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog svBlocks = new Microsoft.Win32.OpenFileDialog();
            if (svBlocks.ShowDialog() != true) return;

            int res = NsOCR.Img_SaveBlocks(ImgObj, svBlocks.FileName);
            if (res > TNSOCR.ERROR_FIRST) ShowError("Img_SaveBlocks", res);
        }

        private void bkClearBlocks_Click(object sender, RoutedEventArgs e)
        {
            NsOCR.Img_DeleteAllBlocks(ImgObj);
            bkSave.IsEnabled = false;
            ShowImage();
        }

        private void bkDetectBlocks_Click(object sender, RoutedEventArgs e)
        {
            NsOCR.Img_DeleteAllBlocks(ImgObj);
            NsOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_ZONING, TNSOCR.OCRSTEP_ZONING, TNSOCR.OCRFLAG_NONE);
            ShowImage();
        }

        private string GetDocName(string str)
        {
            string fn = System.IO.Path.GetFileName(str);
            return System.IO.Path.ChangeExtension(fn, ".pdf");
        }

        private void bkSave_Click(object sender, RoutedEventArgs e)
        {
            SvrObj = 0;            
            svFile.DefaultExt = ".pdf";
            svFile.Filter = "PDF document(*.pdf)|*.pdf|RTF document (*.rtf)|*.rtf|ASCII Text document (*.txt)|*.txt|Unicode Text document (*.txt)|*.txt|XML document (*.xml)|*.xml|PDF/A document(*.pdf)|*.pdf";
            svFile.FileName = GetDocName(opImg.FileName);
            svFile.FilterIndex = 1;
            if (svFile.ShowDialog() != true) return;

            int format = TNSOCR.SVR_FORMAT_PDF + (svFile.FilterIndex - 1);

            //image over text option for PDF
            if ((format == TNSOCR.SVR_FORMAT_PDF) || (format == TNSOCR.SVR_FORMAT_PDFA))
            {
                if (System.Windows.MessageBox.Show("Place page image over recognized text?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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

            int flags = (cbExactCopy.IsChecked == true) ? flags = TNSOCR.FMT_EXACTCOPY : flags = TNSOCR.FMT_EDITCOPY;

            NsOCR.Svr_NewDocument(SvrObj);

            if (sender == null)
                return;

            res = NsOCR.Svr_AddPage(SvrObj, ImgObj, flags);
            if (res > TNSOCR.ERROR_FIRST)
            {
                ShowError("Svr_AddPage", res);
                NsOCR.Svr_Destroy(SvrObj);
                return;
            }

            if ((format == TNSOCR.SVR_FORMAT_PDF) || (format == TNSOCR.SVR_FORMAT_PDFA) || (format == TNSOCR.SVR_FORMAT_XML)) //demonstrate how to write PDF info
                NsOCR.Svr_SetDocumentInfo(SvrObj, TNSOCR.INFO_PDF_TITLE, "Sample Title");

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

        private void bkScan_Click(object sender, RoutedEventArgs e)
        {
            Window2 fmScan = new Window2();
            fmScan.fmMain = this;
            fmScan.ShowDialog();
        }

        private void ProcessEntireDocument()
        {
            bkSave_Click(null, null);

            if (SvrObj == 0) 
                return; //saving cancelled

            int OcrCnt, res;
            bool InSameThread;
            //recognize up to 4 images at once.
            //Note for large images ERROR_NOMEMORY can be raised
            //OcrCnt = 4;

            //Use number of logical CPU cores on the system for the best performance
            OcrCnt = 0;

            InSameThread = false; //perform OCR in non-blocking mode
            //InSameThread = true; //uncomment to perform OCR from this thread (GUI will be freezed)

            int flags = (cbExactCopy.IsChecked == true) ? TNSOCR.FMT_EXACTCOPY : TNSOCR.FMT_EDITCOPY;
            flags = flags << 8; //we need to shift FMT_XXXXX flags for this function

            //process all pages of input image and add results to SAVER object
            //this function will create (and then release) additional temporary OCR objects if OcrCnt > 1
            if (InSameThread)
            {
                res = NsOCR.Ocr_ProcessPages(ImgObj, SvrObj, 0, -1, OcrCnt, TNSOCR.OCRFLAG_NONE | flags);
            }
            else
            {
                Window3 fmWait = new Window3();

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
                    System.Windows.MessageBox.Show("Operation was cancelled.");
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

        private void bkLanguages_Click(object sender, RoutedEventArgs e)
        {
            Window4 fmLangs = new Window4();
            fmLangs.fmMain = this;
            fmLangs.ShowDialog();
        }

        private void bkOptions_Click(object sender, RoutedEventArgs e)
        {
            Window5 fmOptions = new Window5();
            fmOptions.fmMain = this;
            fmOptions.ShowDialog();
        }

        public void ShowHelp(string section)
        {
            string fn = "file://" + AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\..\\Docs\\NSOCR.chm";
            System.Windows.Forms.Help.ShowHelp(null, fn, System.Windows.Forms.HelpNavigator.Topic, section);
        }
        private void bkHelp_Click(object sender, RoutedEventArgs e)
        {
            ShowHelp("overview.htm");
        }
    }
}
