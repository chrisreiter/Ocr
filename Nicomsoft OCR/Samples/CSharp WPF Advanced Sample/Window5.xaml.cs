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
    /// Interaction logic for Window5.xaml
    /// </summary>
    public partial class Window5 : Window
    {
        public Window1 fmMain;

        public Window5()
        {
            InitializeComponent();
        }

        private void bkOK_Click(object sender, RoutedEventArgs e)
        {
            string val = "";

            val = (cbFindBarcodes.IsChecked == true) ? "1" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Zoning/FindBarcodes", val);

            val = (cbImgInversion.IsChecked == true) ? "2" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/Inversion", val);

            val = (cbZonesInversion.IsChecked == true) ? "1" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Zoning/DetectInversion", val);

            val = (cbDeskew.IsChecked == true) ? "360" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/SkewAngle", val);

            val = (cbRotation.IsChecked == true) ? "1" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/AutoRotate", val);

            val = (cbImgNoiseFilter.IsChecked == true) ? "1" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/NoiseFilter", val);

            val = (cbRemoveLines.IsChecked == true) ? "1" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "PixLines/RemoveLines", val);
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "PixLines/FindHorLines", val);
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "PixLines/FindVerLines", val);

            val = (cbGrayMode.IsChecked == true) ? "1" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/GrayMode", val);

            val = (cbFastMode.IsChecked == true) ? "1" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/FastMode", val);

            val = (cbBinTwice.IsChecked == true) ? "1" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Binarizer/BinTwice", val);

            val = (cbCorrectMixed.IsChecked == true) ? "1" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "WordAlizer/CorrectMixed", val);

            val = (cbDictionary.IsChecked == true) ? "1" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Dictionaries/UseDictionary", val);

            val = (cbOneColumn.IsChecked == true) ? "1" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Zoning/OneColumn", val);

            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/EnabledChars", edEnabledChars.Text);

            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/DisabledChars", edDisabledChars.Text);

            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Binarizer/SimpleThr", edBinThreshold.Text);

            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "WordAlizer/TextQual", edTextQual.Text);

            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/PdfDPI", edPDFDPI.Text);

            Close();

        }

        private void bkCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string val = "";

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Zoning/FindBarcodes", out val);
            cbFindBarcodes.IsChecked = (val == "1");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/Inversion", out val); 
            cbImgInversion.IsChecked = (val == "2");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Zoning/DetectInversion", out val);
            cbZonesInversion.IsChecked = (val == "1");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/SkewAngle", out val);
            cbDeskew.IsChecked = (val == "360");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/AutoRotate", out val);
            cbRotation.IsChecked = (val == "1");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/NoiseFilter", out val);
            cbImgNoiseFilter.IsChecked = (val == "1");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "PixLines/RemoveLines", out val); 
            cbRemoveLines.IsChecked = (val == "1");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/GrayMode", out val);
            cbGrayMode.IsChecked = (val == "1");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/FastMode", out val);
            cbFastMode.IsChecked = (val == "1");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Binarizer/BinTwice", out val);
            cbBinTwice.IsChecked = (val == "1");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "WordAlizer/CorrectMixed", out val);
            cbCorrectMixed.IsChecked = (val == "1");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Dictionaries/UseDictionary", out val);
            cbDictionary.IsChecked = (val == "1");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Zoning/OneColumn", out val);
            cbOneColumn.IsChecked = (val == "1");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/EnabledChars", out val);
            edEnabledChars.Text = val;

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/DisabledChars", out val);
            edDisabledChars.Text = val;

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Binarizer/SimpleThr", out val);
            edBinThreshold.Text = val;

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "WordAlizer/TextQual", out val);
            edTextQual.Text = val;

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/PdfDPI", out val);
            edPDFDPI.Text = val;
        }

        private void bkHelp_Click(object sender, RoutedEventArgs e)
        {
            fmMain.ShowHelp("config.htm");
        }

    }
}
