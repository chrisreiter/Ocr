using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using NSOCR_NameSpace; //Add NSOCR namespace from "NSOCR.cs"

namespace Sample
{
    /// <summary>
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        public Window1 fmMain;
        public int res;
        public int mode;

        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public Window3()
        {
            InitializeComponent();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherTimer.IsEnabled = false;
        }

        private bool CheckDone()
        {
            if (mode == 0)
                res = fmMain.NsOCR.Img_OCR(fmMain.ImgObj, 0, 0, TNSOCR.OCRFLAG_GETRESULT);
            else
                res = fmMain.NsOCR.Ocr_ProcessPages(fmMain.ImgObj, 0, 0, 0, 0, TNSOCR.OCRFLAG_GETRESULT);

            return res != TNSOCR.ERROR_PENDING;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer.IsEnabled = true;
            ProgressBar1.Value = 0;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            dispatcherTimer.IsEnabled = false;
            while (!CheckDone()) //make sure that Img_OCR is done
                System.Threading.Thread.Sleep(10);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //cancel work
            if (mode == 0)
                fmMain.NsOCR.Img_OCR(fmMain.ImgObj, 0, 0, TNSOCR.OCRFLAG_CANCEL);
            else
                fmMain.NsOCR.Ocr_ProcessPages(fmMain.ImgObj, 0, 0, 0, 0, TNSOCR.OCRFLAG_CANCEL);

            //we must wait for result anyway since OCRFLAG_CANCEL request returns immediately, work is not stopped yet
            Close(); //this function waits for result
        }
    }
}
