Dim NSOCR, CfgObj, OcrObj, ImgObj, res, txt
Set NSOCR = WScript.CreateObject("NSOCRLib.NSOCR.1") 'create OCR library instance

' we cannot call res = Engine_InitializeAdvanced(CfgObj, OcrObj, ImgObj) because VBSCript does not support passing variables by reference
' so use "Engine_CallFunction" function to call "Engine_InitializeAdvanced" function:
InputVars = Array() 'no input variables
OutputVars = NSOCR.Engine_CallFunction("Engine_InitializeAdvanced", InputVars) 'initialize OCR engine, create objects and load configuration
' OutputVars(0) contains function return code 
CfgObj = OutputVars(1) 'see help for Engine_InitializeAdvanced, three variables are returned
OcrObj = OutputVars(2)
ImgObj = OutputVars(3)

'example how to get config option:
'InputVars = Array(CfgObj, 0, "ImgAlizer/AutoScale")
'OutputVars = NSOCR.Engine_CallFunction("Cfg_GetOption", InputVars)
'MsgBox OutputVars(1)

res = NSOCR.Img_LoadFile(ImgObj, "c:\SampleImage.bmp") 'load some image for OCR
res = NSOCR.Img_OCR(ImgObj, 0, 255, 0) 'perform OCR

' we cannot call res = NsOCR.Img_GetImgText(ImgObj, txt, 0) because VBSCript does not support passing variables by reference
' so use "Engine_CallFunction" function to call "Img_GetImgText" function:
InputVars = Array(ImgObj, 0) 'put "ImgObj" and "Flags" variables to the input array of parameters
OutputVars = NSOCR.Engine_CallFunction("Img_GetImgText", InputVars) 'get text
' OutputVars(0) contains function return code 

MsgBox OutputVars(1) 'Img_GetImgText returns only one variable - recognized text

NSOCR.Engine_Uninitialize