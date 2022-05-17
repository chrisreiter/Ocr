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
    public partial class Form5 : Form
    {
        public Form1 fmMain;

        public Form5()
        {
            InitializeComponent();
        }

        private void bkCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            string val;
            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Zoning/FindBarcodes", out val);
            cbFindBarcodes.Checked = (val == "1");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/Inversion", out val);
            cbImgInversion.Checked = (val == "2");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Zoning/DetectInversion", out val);
            cbZonesInversion.Checked = (val == "1");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/SkewAngle", out val);
            cbDeskew.Checked = (val == "360");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/AutoRotate", out val);
            cbRotation.Checked = (val == "1");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/NoiseFilter", out val);
            cbImgNoiseFilter.Checked = (val == "1");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "PixLines/RemoveLines", out val);
            cbRemoveLines.Checked = (val == "1");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/GrayMode", out val);
            cbGrayMode.Checked = (val == "1");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/FastMode", out val);
            cbFastMode.Checked = (val == "1");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Binarizer/BinTwice", out val);
            cbBinTwice.Checked = (val == "1");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "WordAlizer/CorrectMixed", out val);
            cbCorrectMixed.Checked = (val == "1");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Dictionaries/UseDictionary", out val);
            cbDictionary.Checked = (val == "1");

            fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Zoning/OneColumn", out val);
            cbOneColumn.Checked = (val == "1");

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

        private void bkOK_Click(object sender, EventArgs e)
        {
            string val;

            val = cbFindBarcodes.Checked ? "1" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Zoning/FindBarcodes", val);

            val = cbImgInversion.Checked ? "2" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/Inversion", val);

            val = cbZonesInversion.Checked ? "1" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Zoning/DetectInversion", val);

            val = cbDeskew.Checked ? "360" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/SkewAngle", val);

            val = cbRotation.Checked ? "1" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/AutoRotate", val);

            val = cbImgNoiseFilter.Checked ? "1" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/NoiseFilter", val);

            val = cbRemoveLines.Checked ? "1" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "PixLines/RemoveLines", val);
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "PixLines/FindHorLines", val);
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "PixLines/FindVerLines", val);

            val = cbGrayMode.Checked ? "1" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/GrayMode", val);

            val = cbFastMode.Checked ? "1" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/FastMode", val);

            val = cbBinTwice.Checked ? "1" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Binarizer/BinTwice", val);

            val = cbCorrectMixed.Checked ? "1" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "WordAlizer/CorrectMixed", val);

            val = cbDictionary.Checked ? "1" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Dictionaries/UseDictionary", val);

            val = cbOneColumn.Checked ? "1" : "0";
            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Zoning/OneColumn", val);

            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/EnabledChars", edEnabledChars.Text);

            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/DisabledChars", edDisabledChars.Text);

            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Binarizer/SimpleThr", edBinThreshold.Text);

            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "WordAlizer/TextQual", edTextQual.Text);

            fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/PdfDPI", edPDFDPI.Text);

            Close();
        }

        private void bkHelp_Click(object sender, EventArgs e)
        {
            fmMain.ShowHelp("config.htm");
        }


    }
}
