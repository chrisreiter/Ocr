//JScript has own format of array, we need to convert it to COM style before passing to COM functions
function JsArrayToComArray(jsArray) 
{
    var dict = new ActiveXObject("Scripting.Dictionary");
    for (var i = 0; i < jsArray.length; i++) 
    {
        dict.add(i, jsArray[i]);
    }
    return dict.Items();
}

var WshShell = WScript.CreateObject("WScript.Shell");
var NSOCR = WScript.CreateObject("NSOCRLib.NSOCR.1"); //create OCR library instance

// we cannot call res = Engine_InitializeAdvanced(CfgObj, OcrObj, ImgObj) because VBSCript does not support passing variables by reference
// so use "Engine_CallFunction" function to call "Engine_InitializeAdvanced" function:
var InputVars = JsArrayToComArray(new Array()); //no input variables
var outs = NSOCR.Engine_CallFunction("Engine_InitializeAdvanced", InputVars); //initialize OCR engine, create objects and load configuration
var OutputVars = outs.toArray(); //from COM array to JScript array
// OutputVars[0] contains function return code 
CfgObj = OutputVars[1]; //see help for Engine_InitializeAdvanced, three variables are returned
OcrObj = OutputVars[2];
ImgObj = OutputVars[3];


//example how to get config option:
//InputVars = JsArrayToComArray(new Array(CfgObj, 0, "ImgAlizer/AutoScale"));
//outs = NSOCR.Engine_CallFunction("Cfg_GetOption", InputVars);
//OutputVars = outs.toArray();
//res = WshShell.Popup(OutputVars[1], 0, "NSOCR JScript Sample", 0);

res = NSOCR.Img_LoadFile(ImgObj, "c:\SampleImage.bmp"); //load some image for OCR
res = NSOCR.Img_OCR(ImgObj, 0, 255, 0); //perform OCR

// we cannot call res = NsOCR.Img_GetImgText(ImgObj, txt, 0) because VBSCript does not support passing variables by reference
// so use "Engine_CallFunction" function to call "Img_GetImgText" function:
InputVars = JsArrayToComArray(new Array(ImgObj, 0)); //put "ImgObj" and "Flags" variables to the input array of parameters

outs = NSOCR.Engine_CallFunction("Img_GetImgText", InputVars); //get text
OutputVars = outs.toArray(); //from COM array to JScript array
// OutputVars[0] contains function return code 

res = WshShell.Popup(OutputVars[1], 0, "NSOCR JScript Sample", 0); //Img_GetImgText returns only one variable - recognized text

NSOCR.Engine_Uninitialize();