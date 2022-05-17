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
    public partial class fmLanguages : Form
    {
        public Form1 fmMain;

        public fmLanguages()
        {
            InitializeComponent();
        }

        public void LoadLanguages()
        {
            int i;
            string val;
            for (i = 0; i < cbLanguages.Items.Count; i++)
            {
                if (fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Languages/" + cbLanguages.Items[i], out val) < TNSOCR.ERROR_FIRST)
                    cbLanguages.SetItemChecked(i, val == "1");
                else
                    cbLanguages.SetItemChecked(i, false);
            }
            for (i = 0; i < cbLanguagesAsian.Items.Count; i++)
            {
                if (fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Languages/" + cbLanguagesAsian.Items[i], out val) < TNSOCR.ERROR_FIRST)
                    cbLanguagesAsian.SetItemChecked(i, val == "1");
                else
                    cbLanguagesAsian.SetItemChecked(i, false);
            }
        }

        public bool ApplyLanguages()
        {
            int i;
            bool ch1 = false;
            for (i = 0; i < cbLanguages.Items.Count; i++)
                if (cbLanguages.GetItemChecked(i))
                {
                        ch1 = true;
                        break;
                }

            int ch2 = 0;
            for (i = 0; i < cbLanguagesAsian.Items.Count; i++)
                if (cbLanguagesAsian.GetItemChecked(i))
                    ch2++;

            if (!ch1 && (ch2 == 0))
            {
                System.Windows.Forms.MessageBox.Show("Please select at least one language for recognition.");
                return false;
            }
            if (ch1 && (ch2 > 0))
            {
                System.Windows.Forms.MessageBox.Show("Using both main and asian languages in same zone is not supported.");
                return false;
            }
            if (ch2 > 1)
            {
                System.Windows.Forms.MessageBox.Show("Using two or more asian languages in same zone is not supported currently.");
                return false;
            }

            for (i = 0; i < cbLanguages.Items.Count; i++)
                fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Languages/" + cbLanguages.Items[i], cbLanguages.GetItemChecked(i) ? "1" : "0");
            for (i = 0; i < cbLanguagesAsian.Items.Count; i++)
                fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Languages/" + cbLanguagesAsian.Items[i], cbLanguagesAsian.GetItemChecked(i) ? "1" : "0");

            return true;
        }

        private void fmLanguages_Load(object sender, EventArgs e)
        {
            LoadLanguages();
        }

        private void bkCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bkOK_Click(object sender, EventArgs e)
        {
            if (ApplyLanguages())
                Close();
        }

    }
}
