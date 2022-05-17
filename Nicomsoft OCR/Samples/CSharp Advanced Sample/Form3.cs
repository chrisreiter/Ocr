using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NSOCR_NameSpace; //Add NSOCR namespace from "NSOCR.cs"

namespace Sample
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        public Form1 fmMain;
        public int res;
        public int mode;

        private bool CheckDone()
        {
            if (mode == 0)
                res = fmMain.NsOCR.Img_OCR(fmMain.ImgObj, 0, 0, TNSOCR.OCRFLAG_GETRESULT);
            else
                res = fmMain.NsOCR.Ocr_ProcessPages(fmMain.ImgObj, 0, 0, 0, 0, TNSOCR.OCRFLAG_GETRESULT);
            return res != TNSOCR.ERROR_PENDING;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            Timer1.Enabled = true;
            ProgressBar1.Value = 0;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (CheckDone())
                Close();
            int val;

            if (mode == 0)
                val = fmMain.NsOCR.Img_OCR(fmMain.ImgObj, 0, 0, TNSOCR.OCRFLAG_GETPROGRESS);
            else
                val = fmMain.NsOCR.Ocr_ProcessPages(fmMain.ImgObj, 0, 0, 0, 0, TNSOCR.OCRFLAG_GETPROGRESS);

            if (val < TNSOCR.ERROR_FIRST)
                if (ProgressBar1.Value != val)
                    ProgressBar1.Value = val;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //cancel work
            if (mode == 0)
                fmMain.NsOCR.Img_OCR(fmMain.ImgObj, 0, 0, TNSOCR.OCRFLAG_CANCEL);
            else
                fmMain.NsOCR.Ocr_ProcessPages(fmMain.ImgObj, 0, 0, 0, 0, TNSOCR.OCRFLAG_CANCEL);

            //we must wait for result anyway since OCRFLAG_CANCEL request returns immediately, work is not stopped yet
            Close(); //this function waits for result
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Timer1.Enabled = false;
            while (!CheckDone()) //make sure that Img_OCR is done
                System.Threading.Thread.Sleep(10);
        }
    }
}
