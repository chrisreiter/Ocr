using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using NSOCR_NameSpace; //Add NSOCR namespace from "NSOCR.cs"

namespace Sample
{
    public static class GlobalClass
    {
        static NSOCRLib.NSOCRClass NsOCR;
        static bool Inited = false;
        static int CfgObj, OcrObj;

        public static string OcrFile(string fn)
        {
            int res;
            if (!Inited)
            {
                Inited = true;               
                NsOCR = new NSOCRLib.NSOCRClass();
                
		NsOCR.Engine_SetLicenseKey(""); //required for licensed version only

                //init engine and create ocr-related objects
                NsOCR.Engine_Initialize();
                NsOCR.Cfg_Create(out CfgObj);
                NsOCR.Ocr_Create(CfgObj, out OcrObj);                

                //load options, if path not specified, current folder and folder with NSOCR.dll will be checked
                res = NsOCR.Cfg_LoadOptions(CfgObj, "Config.dat");
                if (res > TNSOCR.ERROR_FIRST) 
                    return "error: Cfg_LoadOptions";
            }

            int ImgObj;
            NsOCR.Img_Create(OcrObj, out ImgObj);
            //load image
            res = NsOCR.Img_LoadFile(ImgObj, fn);
            if (res > TNSOCR.ERROR_FIRST) 
                return "error: Img_LoadFile";

            res = NsOCR.Img_OCR(ImgObj, TNSOCR.OCRSTEP_FIRST, TNSOCR.OCRSTEP_LAST, 0);
            if (res > TNSOCR.ERROR_FIRST)
                return "error: Img_OCR";

            string txt;
            int i, flags;
            flags = TNSOCR.FMT_EDITCOPY; //or TNSOCR.FMT_EXACTCOPY
            NsOCR.Img_GetImgText(ImgObj, out txt, flags);

            string txt2 = "";
            for (i = 0; i < txt.Length; i++)
            {
                if ((txt[i] == '\r') && (txt[i + 1] == '\n'))
                    txt2 = txt2 + "<br>";
                else txt2 = txt2 + txt[i];
            }

            NsOCR.Img_Destroy(ImgObj);
            return txt2;
        }
    }

    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }
    }
}