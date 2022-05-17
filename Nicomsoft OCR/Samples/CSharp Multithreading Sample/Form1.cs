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

        const int THRCNT = 8; //number of threads

        private System.Object lockThis = new System.Object();
        string TotalTxt;

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
            NsOCR.Engine_Initialize();
            NsOCR.Cfg_Create(out CfgObj);

            //load options, if path not specified, current folder and folder with NSOCR.dll will be checked
            NsOCR.Cfg_LoadOptions(CfgObj, "Config.dat"); 

            //here you can adjust configuration (scale image, select languages, etc) - refer to other C# sample projects
            //
            //

            //since we have multiple threads and OCR objects, we do not need multithreading inside every OCR object
            NsOCR.Cfg_SetOption(CfgObj, TNSOCR.BT_DEFAULT, "Main/NumKernels", "1");

            ////
            string s = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            s = System.IO.Path.GetDirectoryName(s);
            edFile.Text = s + "\\..\\..\\..\\Sample.jpg";
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (CfgObj != 0) NsOCR.Cfg_Destroy(CfgObj);
            NsOCR.Engine_Uninitialize();
        }

        static void RecognizeImage(object o)
        {
            ParamI p = (ParamI)o;
            int OcrObj, ImgObj, w, h, BlkObj, n, flags;

            p.NsOCR.Ocr_Create(p.CfgObj, out OcrObj);
            p.NsOCR.Img_Create(OcrObj, out ImgObj);

            p.NsOCR.Img_LoadFile(ImgObj, p.FileName);
            p.NsOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_FIRST, TNSOCR.OCRSTEP_LAST, 0); //all OCR steps

            flags = TNSOCR.FMT_EDITCOPY; //or TNSOCR.FMT_EXACTCOPY
            string txt;
            n = p.NsOCR.Img_GetImgText(ImgObj, out txt, flags); //get buffer text length plus null-terminated zero
            p.Form.AddResult(txt);

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
            TotalTxt = "";
            bkStart.Enabled = false;
            tbText.Clear();

            for (i = 0; i < THRCNT; i++)
            {
                ParamI p = new ParamI();
                p.Form = this;
                p.NsOCR = NsOCR;
                p.CfgObj = CfgObj;
                p.FileName = edFile.Text; //we use the same image for all threads, in real application every thread will have own image to recognize

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

            //display results
            tbText.Text = TotalTxt;
            bkStart.Enabled = true;
        }

        public void AddResult(string txt)
        {
            lock (lockThis)
            {
                TotalTxt = TotalTxt + txt + "\r\n\r\n";
            }
        }
    }


//////////////////
    public class ParamI
    {
        public NSOCRLib.NSOCRClass NsOCR;
        public int CfgObj;
        public string FileName;
        public Form1 Form;
    }
}
