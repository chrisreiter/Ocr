VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "Nicomsoft OCR VB6 Minimal Sample"
   ClientHeight    =   5670
   ClientLeft      =   120
   ClientTop       =   450
   ClientWidth     =   10260
   LinkTopic       =   "Form1"
   ScaleHeight     =   5670
   ScaleWidth      =   10260
   StartUpPosition =   3  'Windows Default
   Begin VB.TextBox Text2 
      Height          =   3735
      Left            =   120
      MultiLine       =   -1  'True
      ScrollBars      =   3  'Both
      TabIndex        =   3
      Top             =   1800
      Width           =   9975
   End
   Begin VB.CommandButton Command1 
      Caption         =   "OCR"
      Height          =   495
      Left            =   4200
      TabIndex        =   2
      Top             =   960
      Width           =   2175
   End
   Begin VB.TextBox Text1 
      Height          =   375
      Left            =   1200
      TabIndex        =   0
      Text            =   "c:\YourImage.bmp"
      Top             =   360
      Width           =   5175
   End
   Begin VB.Label Label1 
      Caption         =   "Image File:"
      Height          =   255
      Left            =   120
      TabIndex        =   1
      Top             =   480
      Width           =   975
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'see NSOCR.vb for complete list od constants
Const ERROR_FIRST = &H70000000

Const OCRSTEP_FIRST = &H0 'first step
'....
Const OCRSTEP_OCR = &H80 'ocr itself
'....
Const OCRSTEP_LAST = &HFF 'last step

'////////////////////////////////////////////////////////////
    
Dim NsOCR As New NSOCRClass 'add reference to "NSOCRLib" if you get error here
Dim CfgObj As Long
Dim OcrObj As Long
Dim ImgObj As Long


Sub ShowError(res As Long)
    MsgBox "Error #" + Hex$(res)
End Sub


Private Sub Form_Load()
    Dim res As Integer
    
    res = NsOCR.Engine_SetLicenseKey("") 'required for licensed version only
    res = NsOCR.Engine_Initialize
    res = NsOCR.Cfg_Create(CfgObj)
    res = NsOCR.Cfg_LoadOptions(CfgObj, "Config.dat")
    res = NsOCR.Ocr_Create(CfgObj, OcrObj)
    res = NsOCR.Img_Create(OcrObj, ImgObj)
    
End Sub

Private Sub Command1_Click()
    Dim res As Integer
    res = NsOCR.Img_LoadFile(ImgObj, Text1.Text)
    If res > ERROR_FIRST Then
        ShowError (res)
        Exit Sub
    End If
    
    res = NsOCR.Img_OCR(ImgObj, OCRSTEP_FIRST, OCRSTEP_LAST, 0)
 
    Dim txt As String
    res = NsOCR.Img_GetImgText(ImgObj, txt, 0)
    
    Text2.Text = txt
    
End Sub
