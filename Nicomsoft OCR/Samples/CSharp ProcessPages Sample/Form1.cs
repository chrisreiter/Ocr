//THIS SAMPLE DEMONSTRATES HOW TO PROCESS SEVERAL PAGES AT ONCE.
//"OCR_PROCESSPAGES" FUNCTION DOES THE SAME THING, BUT IT DOES NOT ALLOW TO ANALYZE 
//OCR RESULTS, IT JUST SAVES RECOGNIZED TEXT TO SVR OBJECT.
//IF YOU NEED TO PERFORM SOME ADDITIONAL ACTIONS THEN USE THIS SAMPLE INSTEAD OF "OCR_PROCESSPAGES".

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using NSOCR_NameSpace;

using System.IO;
using System.Drawing.Imaging;


namespace Sample
{
    public partial class Form1 : Form
    {
        //NSOCRLib COM was added via "Project / Add Reference" dialog
        public NSOCRLib.NSOCRClass NsOCR; //declare only, an instance will be created later, in "OnFormLoad"
        int CfgObj = 0;
        int OcrObj = 0;
        int ImgObj = 0;
        int CurPageInd, PageCnt;
        const int THRCNT = 4; //number of threads

        private System.Object lockImg = new System.Object();
        private System.Object lockRes = new System.Object();

        public struct TRes
        {
            public int PageInd;
            public string Text;
        };
        public List<TRes> list = new List<TRes>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //it is a good idea to create NSOCR instance here instead of creting it in the same line with "NsOCR" definition
            //this way we can handle possible errors if NSOCR is not installed
            try
            {
                NsOCR = new NSOCRLib.NSOCRClass();
            }
            catch (Exception exc) //some error
            {
                string msg = "Cannot create NSOCR COM object instance. Possible reasons:\r\n - NSOCR.dll is missed.\r\n - NSOCR.dll was not registered with Regsvr32.\r\n - NSOCR.dll is x32 but this application is x64.\r\n";
                msg = msg + "\r\n Exception message:\r\n\r\n" + exc.Message;
                System.Windows.Forms.MessageBox.Show(msg);
                Close();
                return;
            }

            //get NSOCR version
            string val;
            NsOCR.Engine_GetVersion(out val);
            Text = Text + " [ NSOCR version: " + val + " ] ";

            NsOCR.Engine_SetLicenseKey(""); //required for licensed version only

            //init engine and create ocr-related objects
            NsOCR.Engine_InitializeAdvanced(out CfgObj, out OcrObj, out ImgObj);

            //load options, if path not specified, current folder and folder with NSOCR.dll will be checked
            NsOCR.Cfg_LoadOptions(CfgObj, "Config.dat"); 

            //here you can adjust configuration (scale image, select languages, etc) - refer to other C# sample projects
            //

            edFile.Text = "c:\\Sample.tif";

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (CfgObj != 0) NsOCR.Cfg_Destroy(CfgObj);
            NsOCR.Engine_Uninitialize();
        }

        static void RecognizeImage(object o)
        {
            ParamI p = (ParamI)o;
            int OcrObj, ImgObj, PageInd;

            p.NsOCR.Ocr_Create(p.CfgObj, out OcrObj);
            p.NsOCR.Img_Create(OcrObj, out ImgObj);

            while (p.Form.GetNextImage(ImgObj, out PageInd))
            {
                p.NsOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_FIRST, TNSOCR.OCRSTEP_LAST, 0); //all OCR steps

                //here perform some additional analyzing of OCR results of the page. 
                //If you don't need it, then just use OCR_ProcessPages instead of this sample application
                //OCR_ProcessPages performs the same actions as this sample application does, but it does not allow to analyze OCR results, it just saves results to SVR object

                string txt;
                p.NsOCR.Img_GetImgText(ImgObj, out txt, TNSOCR.FMT_EDITCOPY); //get buffer text length plus null-terminated zero
                //note results can be added not in the same order, for example, firstly can be added page #2, then page #1.
                //to fix it we also save page number
                p.Form.AddResult(txt, PageInd); 
            }

            p.NsOCR.Img_Destroy(ImgObj);
            p.NsOCR.Ocr_Destroy(OcrObj);
        }

        private void bkStart_Click(object sender, EventArgs e)
        {
            if (!File.Exists(edFile.Text))
            {
                System.Windows.Forms.MessageBox.Show("Cannot find file " + edFile.Text);
                return;
            }

            int i;
            List<Thread> thrs = new List<Thread>();
            bkStart.Enabled = false;
            tbText.Clear();
            CurPageInd = 0;
            if (NsOCR.Img_LoadFile(ImgObj, edFile.Text) != 0)
            {
                System.Windows.Forms.MessageBox.Show("Cannot load file " + edFile.Text);
                return;
            }
            PageCnt = NsOCR.Img_GetPageCount(ImgObj);

            for (i = 0; i < THRCNT; i++)
            {
                ParamI p = new ParamI();
                p.Form = this;
                p.NsOCR = NsOCR;
                p.CfgObj = CfgObj;

                //IMPORTANT: see "Program.cs" file, we set [MTAThread] attribute there to call COM methods in multithreading

                Thread t = new Thread(() => RecognizeImage(p));

                t.Start();

                thrs.Add(t);
            }

            //wait until all threads are finished
            for (i = 0; i < THRCNT; i++)
            {
                thrs[i].Join();
            }

            ////display results
            //sort results by page index
            list.Sort((x, y) => x.PageInd.CompareTo(y.PageInd));

            string TotalTxt = "";
            for (i = 0; i < list.Count; i++)
                TotalTxt = TotalTxt + "PAGE # " + i.ToString() + "\r\n\r\n" + list[i].Text + "\r\n\r\n";
            
            tbText.Text = TotalTxt;
            bkStart.Enabled = true;
        }

        public bool GetNextImage(int DstImgObj, out int PageInd)
        {
            PageInd = 0;
            lock (lockImg)
            {
                if (CurPageInd >= PageCnt)
                    return false;

                NsOCR.Img_SetPage(ImgObj, CurPageInd);
                NsOCR.Img_CopyCurrentPage(ImgObj, DstImgObj, 0);

                PageInd = CurPageInd;
                CurPageInd++;
                return true;
            }
        }

        public void AddResult(string txt, int PageInd)
        {
            lock (lockRes)
            {
                TRes res;
                res.PageInd = PageInd;
                res.Text = txt;
                list.Add(res);
            }
        }
    }


//////////////////
    public class ParamI
    {
        public NSOCRLib.NSOCRClass NsOCR;
        public int CfgObj;
        public Form1 Form;
    }
}
