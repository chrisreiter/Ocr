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
using NSOCR_NameSpace; //Add NSOCR namespace from "NSOCR.cs"

namespace Sample
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }

        public Window1 fmMain;

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbSrc.SelectedIndex = 0;

            cbScanners.Items.Clear();

            int res;
            string buf;

            res = fmMain.NsOCR.Scan_Enumerate(fmMain.ScanObj, out buf, 0);
            if (res > TNSOCR.ERROR_FIRST)
           {
                fmMain.ShowError("Scan_Enumerate", res);
                return;
            }

            //names are separated by comma, parse them
            string r;
            int i;
            r = "";
            for (i = 0; i < buf.Length; i++)
            {
                if (buf[i] != ',')
                    r = r + buf[i];
                else
                {
                    cbScanners.Items.Add(r);
                    r = "";
                }
            }
            if (r != "")
                cbScanners.Items.Add(r);

            //now get default device index
            res = fmMain.NsOCR.Scan_Enumerate(fmMain.ScanObj, out buf, TNSOCR.SCAN_GETDEFAULTDEVICE);
            if (res > TNSOCR.ERROR_FIRST)
            {
                if (res != TNSOCR.ERROR_NODEFAULTDEVICE)
                {
                    fmMain.ShowError("Scan_Enumerate (1)", res);
                    return;
                }
            }
            else
            {
                if (res < cbScanners.Items.Count)
                    cbScanners.SelectedIndex = res;
            }
        }

        private void bkSetDefault_Click(object sender, RoutedEventArgs e)
        {
            int res, n;
            n = cbScanners.SelectedIndex;
            if (n < 0) return;

            string buf;
            res = fmMain.NsOCR.Scan_Enumerate(fmMain.ScanObj, out buf, TNSOCR.SCAN_SETDEFAULTDEVICE | n);
            if (res > TNSOCR.ERROR_FIRST)
            {
                fmMain.ShowError("Scan_Enumerate (2)", res);
                return;
            }
        }

        private void bkCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void bkScan_Click(object sender, RoutedEventArgs e)
        {
            int n, flags, res;
            n = cbScanners.SelectedIndex;
            if (n < 0)
            {
                System.Windows.MessageBox.Show("No scanner selected!");
                return;
            }

            flags = 0;
            switch (cbSrc.SelectedIndex)
            {
                case 0: break; //flatbed
                case 1: flags = flags | TNSOCR.SCAN_SOURCEADF; break; //adf
                case 2: flags = flags | TNSOCR.SCAN_SOURCEAUTO; break; //auto
            }

            if (cbNoUI.IsChecked == true)
                flags = flags | TNSOCR.SCAN_NOUI;

            res = fmMain.NsOCR.Scan_ScanToImg(fmMain.ScanObj, fmMain.ImgObj, n, flags);
            //  res = fmMain.NsOCR.Scan_ScanToFile(fmMain.ScanObj, "c:\\temp.tif", n, flags);

            if (res > TNSOCR.ERROR_FIRST)
            {
                fmMain.ShowError("Scan_ScanToImg", res);
                return;
            }

            Close();
            fmMain.DoImageLoaded();
        }
    }
}
