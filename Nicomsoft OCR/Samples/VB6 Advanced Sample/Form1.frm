VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "COMDLG32.OCX"
Begin VB.Form fmMain 
   AutoRedraw      =   -1  'True
   Caption         =   "Nicomsoft OCR VB6 Minimal Sample"
   ClientHeight    =   7455
   ClientLeft      =   225
   ClientTop       =   555
   ClientWidth     =   15390
   FillColor       =   &H8000000C&
   LinkTopic       =   "Form1"
   ScaleHeight     =   497
   ScaleMode       =   3  'Pixel
   ScaleWidth      =   1026
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton bkOptions 
      Caption         =   "Options"
      Height          =   345
      Left            =   6075
      TabIndex        =   23
      Top             =   555
      Width           =   1125
   End
   Begin MSComDlg.CommonDialog svFile 
      Left            =   9600
      Top             =   6840
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
      Filter          =   $"Form1.frx":0000
   End
   Begin MSComDlg.CommonDialog svBlocks 
      Left            =   8760
      Top             =   6840
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.PictureBox Picture1 
      AutoSize        =   -1  'True
      Height          =   5655
      Left            =   120
      ScaleHeight     =   373
      ScaleMode       =   3  'Pixel
      ScaleWidth      =   469
      TabIndex        =   21
      Top             =   960
      Width           =   7095
   End
   Begin VB.Frame Frame2 
      Height          =   735
      Left            =   120
      TabIndex        =   14
      Top             =   6600
      Width           =   7095
      Begin VB.CheckBox cbPixLines 
         Caption         =   "Show image lines"
         Height          =   195
         Left            =   120
         TabIndex        =   20
         Top             =   480
         Width           =   1935
      End
      Begin VB.CheckBox cbBin 
         Caption         =   "Display binarized image"
         Height          =   195
         Left            =   120
         TabIndex        =   19
         Top             =   200
         Width           =   2055
      End
      Begin VB.CommandButton bkDetectBlocks 
         Caption         =   "Detect Zones"
         Height          =   345
         Left            =   5760
         TabIndex        =   18
         Top             =   240
         Width           =   1125
      End
      Begin VB.CommandButton bkClearBlocks 
         Caption         =   "Clear Zones"
         Height          =   345
         Left            =   4560
         TabIndex        =   17
         Top             =   240
         Width           =   1125
      End
      Begin VB.CommandButton bkSaveBlocks 
         Caption         =   "Save Zones"
         Height          =   345
         Left            =   3360
         TabIndex        =   16
         Top             =   240
         Width           =   1125
      End
      Begin VB.CommandButton bkLoadBlocks 
         Caption         =   "Load Zones"
         Height          =   345
         Left            =   2160
         TabIndex        =   15
         Top             =   240
         Width           =   1125
      End
   End
   Begin MSComDlg.CommonDialog opImg 
      Left            =   7560
      Top             =   6840
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
      DefaultExt      =   "pdf"
      DialogTitle     =   "Open Image File"
      Filter          =   "Image Files (bmp, jpg, tif, png, gif, pdf)|*.bmp;*.jpg;*.tif;*.png;*.gif;*.pdf|All Files|*"
      FilterIndex     =   1
   End
   Begin VB.ComboBox cbScale 
      Height          =   315
      ItemData        =   "Form1.frx":00B6
      Left            =   645
      List            =   "Form1.frx":00D2
      Style           =   2  'Dropdown List
      TabIndex        =   13
      Top             =   555
      Width           =   1950
   End
   Begin VB.CheckBox cbExactCopy 
      Caption         =   "Exact copy (do not format text)"
      Height          =   255
      Left            =   12060
      TabIndex        =   11
      Top             =   675
      Width           =   2550
   End
   Begin VB.TextBox tbText 
      BackColor       =   &H80000004&
      Height          =   5640
      Left            =   7560
      Locked          =   -1  'True
      MultiLine       =   -1  'True
      ScrollBars      =   3  'Both
      TabIndex        =   10
      Top             =   960
      Width           =   7740
   End
   Begin VB.Frame Frame1 
      Caption         =   "Languages"
      Height          =   795
      Left            =   8640
      TabIndex        =   8
      Top             =   75
      Width           =   3000
      Begin VB.CommandButton bkSelectLanguages 
         Caption         =   "Select Languages"
         Height          =   345
         Left            =   640
         TabIndex        =   9
         Top             =   300
         Width           =   1605
      End
   End
   Begin VB.CommandButton bkSetPage 
      Caption         =   "Set"
      Height          =   315
      Left            =   7800
      TabIndex        =   7
      Top             =   75
      Width           =   645
   End
   Begin VB.TextBox edPage 
      Height          =   300
      Left            =   6690
      TabIndex        =   5
      Text            =   "1"
      Top             =   90
      Width           =   495
   End
   Begin VB.CommandButton bkSave 
      Caption         =   "Save"
      Height          =   345
      Left            =   3810
      TabIndex        =   4
      Top             =   75
      Width           =   1125
   End
   Begin VB.CommandButton bkRecognize 
      Caption         =   "Recognize"
      Height          =   345
      Left            =   2595
      TabIndex        =   3
      Top             =   75
      Width           =   1125
   End
   Begin VB.CommandButton bkFile 
      Caption         =   "Open File"
      Height          =   345
      Left            =   1380
      TabIndex        =   2
      Top             =   75
      Width           =   1125
   End
   Begin VB.CommandButton bkScan 
      Caption         =   "Scan"
      Height          =   345
      Left            =   165
      TabIndex        =   1
      Top             =   75
      Width           =   1125
   End
   Begin VB.Label lbWait 
      Caption         =   "Please wait..."
      ForeColor       =   &H000000FF&
      Height          =   255
      Left            =   3240
      TabIndex        =   22
      Top             =   600
      Visible         =   0   'False
      Width           =   1335
   End
   Begin VB.Label Label2 
      Caption         =   "Scale:"
      Height          =   195
      Left            =   45
      TabIndex        =   12
      Top             =   660
      Width           =   555
   End
   Begin VB.Label lbPages 
      Caption         =   "of 1"
      Height          =   195
      Left            =   7230
      TabIndex        =   6
      Top             =   135
      Width           =   375
   End
   Begin VB.Label Label1 
      Caption         =   "Page"
      Height          =   195
      Left            =   6075
      TabIndex        =   0
      Top             =   135
      Width           =   480
   End
   Begin VB.Menu mipopup 
      Caption         =   "POPUP"
      Visible         =   0   'False
      Begin VB.Menu miMachinePrintedText 
         Caption         =   "Type: Machine-printed Text"
      End
      Begin VB.Menu miMachinePrintedDigits 
         Caption         =   "Type: Machine-printed Digits"
      End
      Begin VB.Menu miHandwrittenDigits 
         Caption         =   "Type: Handwritten Digits"
      End
      Begin VB.Menu miBarCode 
         Caption         =   "Type: Bar Code"
      End
      Begin VB.Menu miTable 
         Caption         =   "Type: Table"
      End
      Begin VB.Menu miPicture 
         Caption         =   "Type: Picture"
      End
      Begin VB.Menu miClearArea 
         Caption         =   "Type: Clear Area"
      End
      Begin VB.Menu miZoningArea 
         Caption         =   "Type: Zoning Area"
      End
      Begin VB.Menu miMRZ 
         Caption         =   "Type: MRZ"
      End
      Begin VB.Menu miSeparator1 
         Caption         =   "-"
      End
      Begin VB.Menu miImageInverted 
         Caption         =   "Image Inverted"
      End
      Begin VB.Menu miImageCW90 
         Caption         =   "Image Rotated 90 clockwise"
      End
      Begin VB.Menu miImageCCW90 
         Caption         =   "Image Rotated 90 counterclockwise"
      End
      Begin VB.Menu miImage180 
         Caption         =   "Image Rotated 180"
      End
      Begin VB.Menu miImageMirrorH 
         Caption         =   "Image Mirrored Horizontally"
      End
      Begin VB.Menu miImageMirrorV 
         Caption         =   "Image Mirrored Vertically"
      End
      Begin VB.Menu miSeparator2 
         Caption         =   "-"
      End
      Begin VB.Menu miDeleteZone 
         Caption         =   "Delete Block(Zone)"
      End
   End
End
Attribute VB_Name = "fmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit 'Require Variable Declaration

'used with fnWeight
Const FW_DONTCARE = 0
Const FW_THIN = 100
Const FW_EXTRALIGHT = 200
Const FW_LIGHT = 300
Const FW_NORMAL = 400
Const FW_MEDIUM = 500
Const FW_SEMIBOLD = 600
Const FW_BOLD = 700
Const FW_EXTRABOLD = 800
Const FW_HEAVY = 900
Const FW_BLACK = FW_HEAVY
Const FW_DEMIBOLD = FW_SEMIBOLD
Const FW_REGULAR = FW_NORMAL
Const FW_ULTRABOLD = FW_EXTRABOLD
Const FW_ULTRALIGHT = FW_EXTRALIGHT
'used with fdwCharSet
Const ANSI_CHARSET = 0
Const DEFAULT_CHARSET = 1
Const SYMBOL_CHARSET = 2
Const SHIFTJIS_CHARSET = 128
Const HANGEUL_CHARSET = 129
Const CHINESEBIG5_CHARSET = 136
Const OEM_CHARSET = 255
'used with fdwOutputPrecision
Const OUT_CHARACTER_PRECIS = 2
Const OUT_DEFAULT_PRECIS = 0
Const OUT_DEVICE_PRECIS = 5
'used with fdwClipPrecision
Const CLIP_DEFAULT_PRECIS = 0
Const CLIP_CHARACTER_PRECIS = 1
Const CLIP_STROKE_PRECIS = 2
'used with fdwQuality
Const DEFAULT_QUALITY = 0
Const DRAFT_QUALITY = 1
Const PROOF_QUALITY = 2
'used with fdwPitchAndFamily
Const DEFAULT_PITCH = 0
Const FIXED_PITCH = 1
Const VARIABLE_PITCH = 2
'used with SetBkMode
Const OPAQUE = 2
Const TRANSPARENT = 1

Const LOGPIXELSY = 90

Public NsOCR As Object 'will be created later, in "OnMyFormLoad"

Public CfgObj As Long
Public OcrObj As Long
Public ImgObj As Long
Public ScanObj As Long
Public SvrObj As Long

Private Type Rectangle
    Left As Long
    Top As Long
    Right As Long
    Bottom As Long
End Type

Private Type POINT
     X As Long
     Y As Long
 End Type
 
Dim Dwn As Boolean
Dim Frame As Rectangle

'Dim gr As Graphics
Dim bmp As Image

Dim NoEvents As Boolean
Dim Inited As Boolean ' !!! = False

Dim hDCwnd As Long
Dim dc1 As Long
Dim dc2 As Long
Dim ww As Long
Dim hh As Long

Dim SaveFileName As String
Dim MultiPage As Boolean

Dim pmBlockTag As Long

'API for Open URL
Private Declare Function ShellExecute Lib "shell32.dll" Alias "ShellExecuteA" (ByVal hWnd As Long, ByVal lpOperation As String, ByVal lpFile As String, ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowCmd As Long) As Long
'Lib for Bitmap
Private Declare Function GetWindowDC Lib "user32" (ByVal hWnd As Long) As Long
Private Declare Function CreateCompatibleDC Lib "gdi32" (ByVal hdc As Long) As Long
Private Declare Function DeleteDC Lib "gdi32" (ByVal hdc As Long) As Long
Private Declare Function DeleteObject Lib "gdi32" (ByVal hObject As Long) As Long
Private Declare Function ReleaseDC Lib "user32" (ByVal hWnd As Long, ByVal hdc As Long) As Long
Private Declare Function CreateCompatibleBitmap Lib "gdi32" (ByVal hdc As Long, ByVal nWidth As Long, ByVal nHeight As Long) As Long
Private Declare Function BitBlt Lib "gdi32" (ByVal hDestDC As Long, ByVal X As Long, ByVal Y As Long, ByVal nWidth As Long, ByVal nHeight As Long, ByVal hSrcDC As Long, ByVal xSrc As Long, ByVal ySrc As Long, ByVal dwRop As Long) As Long
Private Declare Function SelectObject Lib "gdi32" (ByVal hdc As Long, ByVal hObject As Long) As Long
Private Declare Function MoveTo Lib "gdi32" Alias "MoveToEx" (ByVal hdc As Long, ByVal X As Long, ByVal Y As Long, lpPoint As POINT) As Long
Private Declare Function LineTo Lib "gdi32" (ByVal hdc As Long, ByVal X As Long, ByVal Y As Long) As Long
Private Declare Function CreatePen Lib "gdi32" (ByVal nPenStyle As Long, ByVal nWidth As Long, ByVal crColor As Long) As Long
Private Declare Function TextOut Lib "gdi32" Alias "TextOutA" (ByVal hdc As Long, ByVal X As Long, ByVal Y As Long, ByVal lpString As String, ByVal nCount As Long) As Long
Private Declare Function SetTextColor Lib "gdi32" (ByVal hdc As Long, ByVal color As Long) As Long
Private Declare Function CreateFont Lib "gdi32" Alias "CreateFontA" (ByVal nHeight As Long, ByVal nWidth As Long, _
ByVal nEscapement As Long, ByVal nOrientation As Long, ByVal fnWeight As Long, ByVal fdwItalic As Boolean, _
ByVal fdwUnderline As Boolean, ByVal fdwStrikeOut As Boolean, ByVal fdwCharSet As Long, ByVal fdwOutputPrecision As Long, _
ByVal fdwClipPrecision As Long, ByVal fdwQuality As Long, ByVal fdwPitchAndFamily As Long, ByVal lpszFace As String) As Long
Private Declare Function GetDC Lib "user32" (ByVal hWnd As Long) As Long
Private Declare Function GetDeviceCaps Lib "gdi32" (ByVal hdc As Long, ByVal nIndex As Long) As Long
Private Declare Function MulDiv Lib "kernel32" (ByVal nNumber As Long, ByVal nNumerator As Long, ByVal nDenominator As Long) As Long
Private Declare Function CreateSolidBrush Lib "gdi32" (ByVal crColor As Long) As Long
Private Declare Function SetBkColor Lib "gdi32" (ByVal hdc As Long, ByVal crColor As Long) As Long
Private Declare Function FillRect Lib "user32.dll" (ByVal hdc As Long, lpRect As Rectangle, ByVal hbrush As Long) As Long
Private Declare Function SetRect Lib "user32.dll" (lpRect As Rectangle, ByVal X1 As Long, ByVal Y1 As Long, ByVal X2 As Long, ByVal Y2 As Long) As Long
                            
Public Sub OpenURL(ByVal URL As String)
    Dim OpenUrlBuf As Long
    OpenUrlBuf = ShellExecute(1&, "Open", URL, "", "", ByVal 1&)
End Sub

Public Sub ShowError(ByVal api As String, ByVal err As Long)
    Dim s As String
        s = api & " Error #" & Hex$(err)
        Call MsgBox(s)
End Sub

Private Sub bkClearBlocks_Click()
  NsOCR.Img_DeleteAllBlocks (ImgObj)
  bkSave.Enabled = False
  Call ShowImage
End Sub

Private Sub bkDetectBlocks_Click()
        Call NsOCR.Img_DeleteAllBlocks(ImgObj)
        Call NsOCR.Img_OCR(ImgObj, OCRSTEP_ZONING, OCRSTEP_ZONING, OCRFLAG_NONE)
        Call ShowImage
End Sub

Private Sub bkLoadBlocks_Click()
   If Not IsImgLoaded() Then Exit Sub
    
   svBlocks.CancelError = False
   svBlocks.DialogTitle = "Load Blocks"
   svBlocks.Filter = "Block files (*.blk)|*.blk|All files (*.*)|*.*"
   svBlocks.DefaultExt = ".blk"
   
   svBlocks.Action = 1

   If svBlocks.flags <> 1024 Then Exit Sub

   NsOCR.Img_DeleteAllBlocks (ImgObj) 'note: Img_LoadBlocks does not remove existing blocks, so remove them here
   bkSave.Enabled = False
   Dim res As Long
   res = NsOCR.Img_LoadBlocks(ImgObj, svBlocks.FileName)
   
   If res > ERROR_FIRST Then
     ShowError "Img_LoadBlocks", res
     Exit Sub
   End If
   
   Call ShowImage
End Sub

Private Sub bkOptions_Click()
    fmOptions.Show 1
End Sub

Private Sub bkRecognize_Click()
    If Not IsImgLoaded() Then Exit Sub
    
    If Not IsImgLoaded() Then
        MsgBox "Image not loaded!"
        Exit Sub
    End If

    '////
    bkRecognize.Enabled = False
    lbWait.Visible = True
    bkSave.Enabled = False
    
    Dim res As Long
    Dim InSameThread As Integer

    InSameThread = False 'perform OCR in non-blocking mode
    'InSameThread = True 'uncomment to perform OCR from this thread (GUI will be freezed)

    'perform OCR itself
     If InSameThread Then
        res = NsOCR.Img_OCR(ImgObj, OCRSTEP_ZONING, OCRSTEP_LAST, OCRFLAG_NONE)
     Else
        'do it in non-blocking mode and then wait for result
         res = NsOCR.Img_OCR(ImgObj, OCRSTEP_ZONING, OCRSTEP_LAST, OCRFLAG_THREAD)
         If res > ERROR_FIRST Then
            ShowError "Ocr_OcrImg(1)", res
            Exit Sub
         End If
         
         fmWait.mode = 0
         'fmWait.fmMain = Me
         fmWait.Show 1
         res = fmWait.res
         
        End If

        lbWait.Visible = False
        bkRecognize.Enabled = True
        bkSave.Enabled = True

        If res > ERROR_FIRST Then
            If res = ERROR_OPERATIONCANCELLED Then
                MsgBox "Operation was cancelled."
            Else
                ShowError "Img_OCR", res
                Exit Sub
            End If
        End If
    
    Call ShowText
    Call ShowImage
    
End Sub

Private Sub ShowText()
    If Not IsImgLoaded() Then Exit Sub
    
    Dim res As Long
    Dim flags As Long
    
    If cbExactCopy.Value Then
      flags = FMT_EXACTCOPY
    Else: flags = FMT_EDITCOPY
    End If
    
    Dim txt As String
    res = NsOCR.Img_GetImgText(ImgObj, txt, flags)
    
    tbText.Text = txt
End Sub
Private Function GetFileName(flname As String) As String

    'Get the filename without the path or extension.
    'Input Values:
    '   flname - path and filename of file.
    'Return Value:
    '   GetFileName - name of file without the extension.

    Dim posn As Integer, i As Integer
    Dim fName As String

    posn = 0
    'find the position of the last "\" character in filename
    For i = 1 To Len(flname)
        If (Mid(flname, i, 1) = "\") Then posn = i
    Next i

    'get filename without path
    fName = Right(flname, Len(flname) - posn)

    'get filename without extension
    posn = InStr(fName, ".")
        If posn <> 0 Then
            fName = Left(fName, posn - 1)
        End If
    GetFileName = fName
End Function

Private Function GetDocName(ByVal str As String) As String
    Dim fn As String
    fn = GetFileName(str)
    GetDocName = fn + ".pdf"
End Function


Private Sub bkSave_Click()
    SvrObj = 0
    
    svFile.FileName = GetDocName(opImg.FileName)
    svFile.FilterIndex = 1
    svFile.Action = 2
        
    If svFile.flags <> 1024 Then Exit Sub
    SaveFileName = svFile.FileName
    
    Dim format As Long
    format = SVR_FORMAT_PDF + (svFile.FilterIndex - 1)

    'image over text option for PDF
    If format = SVR_FORMAT_PDF Then
        If MsgBox("Place page image over recognized text?", vbYesNo) = vbYes Then
            NsOCR.Cfg_SetOption CfgObj, BT_DEFAULT, "Saver/PDF/ImageLayer", "1"
        Else
            NsOCR.Cfg_SetOption CfgObj, BT_DEFAULT, "Saver/PDF/ImageLayer", "0"
        End If
    End If

    Dim res As Long
    res = NsOCR.Svr_Create(CfgObj, format, SvrObj)
    
    If res > ERROR_FIRST Then
        ShowError "Svr_Create", res
        Exit Sub
    End If

    Dim flags As Long
    If cbExactCopy.Value Then
        flags = FMT_EXACTCOPY
    Else: flags = FMT_EDITCOPY
    End If

    NsOCR.Svr_NewDocument (SvrObj)

    If MultiPage Then
        Exit Sub
    End If

    res = NsOCR.Svr_AddPage(SvrObj, ImgObj, flags)
    If res > ERROR_FIRST Then
        ShowError "Svr_AddPage", res
        NsOCR.Svr_Destroy (SvrObj)
        Exit Sub
    End If

    If format = SVR_FORMAT_PDF Then 'demonstrate how to write PDF info
        NsOCR.Svr_SetDocumentInfo SvrObj, INFO_PDF_TITLE, "Sample Title"
    End If

    res = NsOCR.Svr_SaveToFile(SvrObj, SaveFileName)
    NsOCR.Svr_Destroy (SvrObj)

    If res > ERROR_FIRST Then
      ShowError "Svr_SaveToFile", res
      Exit Sub
    End If

    'open the file
    ShellExecute 1&, "Open", SaveFileName, "", "", ByVal 1&

End Sub

Private Sub bkSaveBlocks_Click()
    If Not IsImgLoaded() Then Exit Sub
    
   svBlocks.CancelError = False
   svBlocks.DialogTitle = "Save Blocks"
   svBlocks.Filter = "Block files (*.blk)|*.blk|All files (*.*)|*.*"
   svBlocks.DefaultExt = ".blk"
      
   svBlocks.Action = 2
'   svBlocks.ShowOpen
   
   If svBlocks.CancelError Then Exit Sub

   Dim res As Long
   res = NsOCR.Img_SaveBlocks(ImgObj, svBlocks.FileName)
   If res > ERROR_FIRST Then ShowError "Img_SaveBlocks", res
End Sub

Private Sub bkScan_Click()
  fmScan.Show 1
End Sub

Private Sub bkSelectLanguages_Click()
  fmLangs.Show 1
End Sub

Private Sub bkSetPage_Click()

  If Not IsImgLoaded() Then Exit Sub
  Dim cnt As Long
  Dim n As Long
  Dim res As Long
  
  cnt = NsOCR.Img_GetPageCount(ImgObj)
  n = val(edPage.Text)
  If n = 0 Then n = 1
  
  n = n - 1
  If n < 0 Then n = 0
  If n >= cnt Then n = cnt - 1
  
  NsOCR.Img_SetPage ImgObj, n
  edPage.Text = n + 1
  bkSave.Enabled = False

  'now apply image scaling, binarize image, deskew etc,
  'everything except OCR itself
   res = NsOCR.Img_OCR(ImgObj, OCRSTEP_FIRST, OCRSTEP_ZONING - 1, OCRFLAG_NONE)
   If res > ERROR_FIRST Then
     ShowError "Img_OCR", res
   End If

   Dim w As Long
   Dim h As Long
   NsOCR.Img_GetSize ImgObj, w, h

   Frame.Left = 0
   Frame.Top = 0
   Frame.Right = w - 1
   Frame.Bottom = h - 1
   Call AdjustDocScale

End Sub

Private Sub cbBin_Click()
  Call AdjustDocScale
End Sub

Private Sub cbExactCopy_Click()
  Call ShowText
End Sub

Private Sub cbPixLines_Click()
  Call ShowImage
End Sub

Private Sub cbScale_Click()
    If NoEvents Then Exit Sub
    bkRecognize.Enabled = False
    Call bkFile_Click
End Sub

Private Sub Form_Unload(Cancel As Integer)
    If Not Inited Then Exit Sub
    If ImgObj <> 0 Then Call NsOCR.Img_Destroy(ImgObj)
    If OcrObj <> 0 Then Call NsOCR.Ocr_Destroy(OcrObj)
    If CfgObj <> 0 Then Call NsOCR.Cfg_Destroy(CfgObj)
    If ScanObj <> 0 Then Call NsOCR.Scan_Destroy(ScanObj)
    Call NsOCR.Engine_Uninitialize
    
    Call ReleaseDC(Me.hWnd, hDCwnd)
End Sub

Private Sub Form_Load()
    Inited = False
    CfgObj = 0
    OcrObj = 0
    ImgObj = 0

    SaveFileName = ""
    MultiPage = False

    'it is a good idea to create NSOCR instance here instead of creting it in the same line with "NsOCR" definition
    'this way we can handle possible errors if NSOCR is not installed
    On Error GoTo ErrorMark        ' Enable error-handling routine.
        Set NsOCR = New NSOCRClass
    On Error GoTo 0                 ' Turn off error trapping.

    Inited = True 'NSOCR instance created successfully
 Dim val As String

    val = ""
    Call NsOCR.Engine_GetVersion(val)
    fmMain.Caption = fmMain.Caption + " [ NSOCR version: " + val + " ] "
    
    Call NsOCR.Engine_SetLicenseKey("") 'required for licensed version only

    Call NsOCR.Engine_Initialize
    Call NsOCR.Cfg_Create(CfgObj)
    Call NsOCR.Cfg_LoadOptions(CfgObj, "Config.dat")
    Call NsOCR.Ocr_Create(CfgObj, OcrObj)
    Call NsOCR.Img_Create(OcrObj, ImgObj)
    Call NsOCR.Scan_Create(CfgObj, ScanObj)
    
    Call NsOCR.Cfg_LoadOptions(CfgObj, "Config.dat")  'load options, if path not specified, current folder and folder with NSOCR.dll will be checked
  
    bkRecognize.Enabled = False

    NoEvents = True
    cbScale.ListIndex = 0
    NoEvents = False
    
    bkSave.Enabled = False
    
    hDCwnd = GetWindowDC(Me.hWnd)
    dc1 = 0
    dc2 = 0
    
    'copy some settings to GUI
    If NsOCR.Cfg_GetOption(CfgObj, BT_DEFAULT, "ImgAlizer/AutoScale", val) < ERROR_FIRST Then
        cbScale.Enabled = (val = "1")
    End If
    
    Picture1.Line (0, 0)-(Picture1.Width - 1, Picture1.Height - 1), RGB(&HFF, &HFF, &HFF), BF
    
    Exit Sub            ' Exit to avoid handler.
    
ErrorMark:              ' Error-handling routine.
    Dim msg As String
    Dim cr As String
    
    cr = Chr(13) & Chr(10)
    msg = "Cannot create NSOCR COM object instance. Possible reasons:" & cr & " - NSOCR.dll is missed." & cr & " - NSOCR.dll was not registered with Regsvr32." & cr & " - NSOCR.dll is x32 but this application is x64." & cr
    msg = msg & cr & " Exception message:" & cr & cr & err.Number & cr & err.Description
    Call MsgBox(msg)
    err.Clear       ' Clear Err object fields.
End Sub

Private Function IsImgLoaded() As Boolean
    Dim w As Long
    Dim h As Long
    
    IsImgLoaded = False
    If NsOCR.Img_GetSize(ImgObj, w, h) > ERROR_FIRST Then Exit Function
    IsImgLoaded = (w > 0) And (h > 0)
'    IsImgLoaded = True
End Function
    
Private Function GetCurDocScale() As Double
    If Not IsImgLoaded() Then
        GetCurDocScale = 1#
        Exit Function
    End If

    Dim w As Long
    Dim h As Long
    Dim ww As Long
    Dim hh As Long
    
    w = Picture1.Width - 5
    h = Picture1.Height - 5

    Call NsOCR.Img_GetSize(ImgObj, ww, hh)
    
    Dim kX As Double
    Dim kY As Double
    Dim k As Double
    
    kX = w / ww
    kY = h / hh
    If kX > kY Then k = kY Else k = kX
    GetCurDocScale = k
End Function


Private Function CreateDC(w As Long, h As Long) As Long
    Dim hDCMemory As Long
    Dim hBmp As Long
    Dim hBmpPrev As Long
       
    'Create a memory device context
    hDCMemory = CreateCompatibleDC(hDCwnd)
         
    'Create a bitmap and place it in the memory DC
    hBmp = CreateCompatibleBitmap(hDCwnd, w, h)
    hBmpPrev = SelectObject(hDCMemory, hBmp)
    Call DeleteObject(hBmpPrev)
       
    CreateDC = hDCMemory
End Function

Private Sub FreeDC(ByRef dc As Long)
    If dc <> 0 Then
        Call DeleteDC(dc)
    End If
    dc = 0
End Sub

Private Sub DrawRectangle(ByVal dc As Long, ByRef r As Rectangle, ByRef col As Long, ByRef w As Long)

  Dim pt As POINT ' needed for another API function
  Dim hPen As Long
  Dim hPenOld As Long
  Dim retval As Long
  
  hPen = CreatePen(0, w, col)
  If hPen <> 0 Then
  
    hPenOld = SelectObject(dc, hPen)
   
    MoveTo dc, r.Left, r.Top, pt
    LineTo dc, r.Right, r.Top
    LineTo dc, r.Right, r.Bottom
    LineTo dc, r.Left, r.Bottom
    LineTo dc, r.Left, r.Top
    
    SelectObject dc, hPenOld
    DeleteObject hPen
  End If
              
End Sub

Function CreateMyFont(nSize As Integer) As Long

  Dim nDegrees As Long
  nDegrees = 0
  
'Create a specified font
  CreateMyFont = CreateFont(MulDiv(nSize, GetDeviceCaps(GetDC(0), LOGPIXELSY) * -1, 72), 0, _
  nDegrees * 10, 0, FW_NORMAL, False, False, False, DEFAULT_CHARSET, _
OUT_DEFAULT_PRECIS, CLIP_DEFAULT_PRECIS, PROOF_QUALITY, DEFAULT_PITCH, _
"Arial")

End Function

Private Sub ShowImage()

    If Not IsImgLoaded() Then Exit Sub
        
    Dim retval As Long ' return value
    'declare the DrawRect variables
    Dim hPen As Long
    Dim hOldPen As Long
    Dim pt As POINT ' needed for another API function
  
 
    Picture1.AutoRedraw = False
    
    Dim hsbrush As Long
    Dim r As Rectangle

    retval = SetRect(r, 0, 0, Picture1.Width - 1, Picture1.Height - 1)
    hsbrush = CreateSolidBrush(RGB(255, 255, 255))
    retval = FillRect(dc2, r, hsbrush)
    retval = DeleteObject(hsbrush)
    
    Call BitBlt(dc2, 0, 0, ww, hh, dc1, 0, 0, vbSrcCopy)
    
    'тут на dc2 дорисовываем раммки
    '......
     'и выводим на экран картинку, чтобы потом поверх неё рисовать рамки
 '   Call BitBlt(Picture1.hDC, 0, 0, ww, hh, dc2, 0, 0, vbSrcCopy)
   'retval = MoveTo(dc2, 0, 40, pt)
   'retval = LineTo(dc2, 100, 50)
  
    Dim k As Double
    Dim col As Long
    '        Dim w As Integer
'        Dim h As Integer
''        Dim bmp2 As Bitmap
' '       Dim graphic As Graphics
    k = GetCurDocScale()
       
'        w = bmp.Width
'        h = bmp.Height
' '       bmp2 = New Bitmap(w, h, gr)
' '       graphic = Graphics.FromImage(bmp2)
' '       graphic.DrawImage(bmp, 0, 0)
'
'
'
        If cbPixLines.Value Then
            DrawPixLines dc2
        End If

        Dim i As Long
        Dim BlkObj As Long
        Dim Xpos As Long
        Dim Ypos As Long
        Dim Width As Long
        Dim Height As Long
        Dim cnt As Integer
        
        Dim font As Long
        Dim fontold As Long
        font = CreateMyFont(6)
        fontold = SelectObject(dc2, font)
        
        Dim hbrush As Long
        Dim hOldBrush As Long
        hbrush = CreateSolidBrush(RGB(255, 80, 80)) ' create a solid yellow brush
        hOldBrush = SelectObject(dc2, hbrush)

''        Pen = New Pen(Color.Green)
        cnt = NsOCR.Img_GetBlockCnt(ImgObj)
        For i = 0 To cnt - 1
            Call NsOCR.Img_GetBlock(ImgObj, i, BlkObj)
            Call NsOCR.Blk_GetRect(BlkObj, Xpos, Ypos, Width, Height)
            
            r.Left = (k * Xpos)
            r.Top = (k * Ypos)
            r.Right = r.Left + (k * Width - 1)
            r.Bottom = r.Top + (k * Height - 1)
            
            col = vbRed
            Select Case NsOCR.Blk_GetType(BlkObj)
                Case BT_OCRTEXT
                    col = RGB(0, &H80, 0)       'Green RGB=&H008000
                Case BT_OCRDIGIT
                    col = vbGreen               'Lime RGB=&H00FF00
                Case BT_ICRDIGIT
                    col = vbBlue                'Blue RGB=&H0000FF
                Case BT_BARCODE
                    col = RGB(0, 0, &H80)       'Navy RGB=&H000080
                Case BT_PICTURE
                    col = RGB(0, &HFF, &HFF)    'Aqua RGB=&H00FFFF
                Case BT_CLEAR
                    col = RGB(&H80, &H80, &H80) 'Gray RGB=&H808080
                Case BT_ZONING
                    col = vbBlack               'Black RGB=&H000000
                Case BT_TABLE
                    col = RGB(&H80, &H80, 0)    'Olive RGB=&H808000
                Case BT_MRZ
                    col = vbBlack               'Black RGB=&H000000
            End Select
            
            SetBkColor dc2, col
            DrawRectangle dc2, r, col, 2
       
            Dim num As String
            num = str(i)
            SetTextColor dc2, RGB(&HFF, &HFF, &HFF)
            TextOut dc2, r.Left, r.Top - 1, num, Len(num)
            
        Next i
        
        SelectObject dc2, fontold
        DeleteObject font
        
        SelectObject dc2, hOldBrush
        DeleteObject hbrush
        

        'user is creating new block, draw its frame
        If Dwn Then
            r.Left = (k * Frame.Left)
            r.Top = (k * Frame.Top)
            r.Right = (k * Frame.Right)
            r.Bottom = (k * Frame.Bottom)

            If r.Right - r.Left >= ww Then r.Right = r.Left + ww - 1
            If r.Bottom - r.Top >= hh Then r.Bottom = r.Top + hh - 1

            DrawRectangle dc2, r, RGB(&HFF, &H0, 0), 1
        End If
        
    'Picture1.Line (0, 0)-(Picture1.Width - 1, Picture1.Height - 1), RGB(&HFF, &HFF, &HFF), BF
    'и выводим на экран dc2, только  если перед этим рисовали на dc2
    Call BitBlt(Picture1.hdc, 0, 0, Picture1.Width, Picture1.Height, dc2, 0, 0, vbSrcCopy)
    
'    Picture1.AutoRedraw = True
    Picture1.ScaleMode = vbPixels
    

    End Sub

    Private Sub DrawPixLines(ByRef dc As Long)

        Dim i As Long
        Dim cnt As Long
        Dim X1 As Long
        Dim Y1 As Long
        Dim X2 As Long
        Dim Y2 As Long
        Dim w As Long
        Dim k As Double

        k = GetCurDocScale()
        cnt = NsOCR.Img_GetPixLineCnt(ImgObj)
        For i = 0 To cnt - 1
            Call NsOCR.Img_GetPixLine(ImgObj, i, X1, Y1, X2, Y2, w)
            X1 = (k * X1)
            Y1 = (k * Y1)
            X2 = (k * X2)
            Y2 = (k * Y2)

            Dim pt1 As POINT
            Dim pt2 As POINT
            
             pt1.X = X1
             pt1.Y = Y1
             
             pt2.X = X2
             pt2.Y = Y2

             Dim pt As POINT ' needed for another API function
             Dim hPen As Long
             Dim hPenOld As Long
             Dim retval As Long
  
             hPen = CreatePen(0, w, RGB(&HFF, &H0, &H0))
             If hPen <> 0 Then
  
               hPenOld = SelectObject(dc, hPen)
   
               MoveTo dc, pt1.X, pt1.Y, pt
               LineTo dc, pt2.X, pt2.Y
    
               SelectObject dc, hPenOld
               DeleteObject hPen
             End If

        Next i
    End Sub

Private Sub AdjustDocScale()
    If Not IsImgLoaded() Then Exit Sub
    
    ww = Picture1.Width - 5
    hh = Picture1.Height - 5

    Call FreeDC(dc1)
    dc1 = CreateDC(ww, hh)
    
    Call FreeDC(dc2)
    dc2 = CreateDC(ww, hh)

    If cbBin.Value = 1 Then
          Call NsOCR.Img_DrawToDC(ImgObj, dc1, 0, 0, ww, hh, DRAW_BINARY)
    Else: Call NsOCR.Img_DrawToDC(ImgObj, dc1, 0, 0, ww, hh, DRAW_NORMAL)
    End If

    Call ShowImage
    End Sub
    
Private Sub bkFile_Click()
  
    On Error GoTo lbCancel
    opImg.CancelError = True
    opImg.ShowOpen
    On Error GoTo 0
      
    If cbScale.Enabled Then
        If cbScale.ListIndex < 1 Then 'autoscaling
            Call NsOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, "ImgAlizer/AutoScale", "1")
            Call NsOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, "ImgAlizer/ScaleFactor", "1.0") 'default scale if cannot detect it automatically
        Else 'fixed scaling
            Call NsOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, "ImgAlizer/AutoScale", "0")
            Dim s As String
            s = str(cbScale.ItemData(cbScale.ListIndex))
            Call NsOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, "ImgAlizer/ScaleFactor", s)
        End If
    End If
      
    Dim res As Long
    
    res = NsOCR.Img_LoadFile(ImgObj, opImg.FileName)
    If res > ERROR_FIRST Then
        If res = ERROR_CANNOTLOADGS Then 'cannot load GhostScript to support PDF
            Dim PushButton As Long
            Dim s1 As String
            s1 = "GhostSript library is needed to support PDF files. Just download and install it with default settings. Do you want to download GhostScript now?"
            PushButton = MsgBox(s1, 33, "ERROR_CANNOTLOADGS")
            If PushButton <> 1 Then Exit Sub
            Call OpenURL("http://www.nicomsoft.com/files/ocr/ghostscript.htm")
        Else
            Call ShowError("Img_LoadFile", res)
        End If
        Exit Sub
    End If

   Call DoImageLoaded
lbCancel:
    err.Clear       ' Clear Err object fields.
End Sub

Private Sub ProcessEntireDocument()

    MultiPage = True
    Call bkSave_Click
    MultiPage = False

    If SvrObj = 0 Then
      Exit Sub 'saving cancelled
    End If


    Dim OcrCnt, res As Long
    Dim InSameThread As Boolean
    'recognize up to 4 images at once.
    'Note for large images ERROR_NOMEMORY can be raised
    'OcrCnt = 4

    'Use number of logical CPU cores on the system for the best performance
     OcrCnt = 0

     InSameThread = False 'perform OCR in non-blocking mode
     'InSameThread = True 'uncomment to perform OCR from this thread (GUI will be freezed)

     Dim flags As Long
        If cbExactCopy.Value Then
            flags = FMT_EXACTCOPY
        Else
            flags = FMT_EDITCOPY
        End If
        flags = flags * 256 'we need to shift FMT_XXXXX flags for this function

        'process all pages of input image and add results to SAVER object
        'this function will create (and then release) additional temporary OCR objects if OcrCnt > 1
        If InSameThread Then
            res = NsOCR.Ocr_ProcessPages(ImgObj, SvrObj, 0, -1, OcrCnt, OCRFLAG_NONE Or flags)
        Else
            'do it in non-blocking mode and then wait for result
            res = NsOCR.Ocr_ProcessPages(ImgObj, SvrObj, 0, -1, OcrCnt, OCRFLAG_THREAD Or flags)
            If (res > ERROR_FIRST) Then

            ShowError "Ocr_ProcessPages(1)", res
            Exit Sub
        End If
            fmWait.mode = 1
            'fmWait.fmMain = Me
            fmWait.Show 1
            res = fmWait.res
        End If

        'restore option
        NsOCR.Cfg_SetOption CfgObj, BT_DEFAULT, "Main/NumKernels", "0"

        If res > ERROR_FIRST Then
            If (res = ERROR_OPERATIONCANCELLED) Then
                MsgBox "Operation was cancelled."
            Else
                ShowError "Ocr_ProcessPages", res
                NsOCR.Svr_Destroy (SvrObj)
                Exit Sub
            End If
        End If

        'save output document
        res = NsOCR.Svr_SaveToFile(SvrObj, SaveFileName)
        NsOCR.Svr_Destroy (SvrObj)

        If res > ERROR_FIRST Then
            ShowError "Svr_SaveToFile", res
            Exit Sub
        End If

        'open the file
        ShellExecute 1&, "Open", SaveFileName, "", "", ByVal 1&
    End Sub
    
Public Sub DoImageLoaded()
    Dim res As Long

    'check if image has multiple page and ask user if he wants process and save all pages automatically
    res = NsOCR.Img_GetPageCount(ImgObj)
    If res > ERROR_FIRST Then
        Call ShowError("Img_GetPageCount", res)
        Exit Sub
    End If
    If res > 1 Then
        Dim PushButton As Long
        Dim s As String
        s = "Image contains multiple pages (" & res & "). Do you want to process and save all pages automatically?"
        PushButton = MsgBox(s, vbYesNo)
        If PushButton = vbYes Then 'If OK
            Call ProcessEntireDocument
            bkRecognize.Enabled = False
            Exit Sub
        End If
    End If


    'now apply image scaling, binarize image, deskew etc,
    'everything except OCR itself
    res = NsOCR.Img_OCR(ImgObj, OCRSTEP_FIRST, OCRSTEP_ZONING - 1, OCRFLAG_NONE)
    If res > ERROR_FIRST Then
        Call ShowError("Img_OCR", res)
    End If
      
        
    edPage.Text = "1"   'Установка номера страницы в 1
    lbPages.Caption = "of " & NsOCR.Img_GetPageCount(ImgObj) ' Из ... страниц
    bkSave.Enabled = False

    Frame.Left = 0     'Сброс размеров прямоугольника Frame
    Frame.Right = 0
    Frame.Top = 0
    Frame.Bottom = 0
    Call AdjustDocScale

    bkRecognize.Enabled = True
    tbText.Text = ""
    bkLoadBlocks.Enabled = True
    bkSaveBlocks.Enabled = True
    bkClearBlocks.Enabled = True
    bkDetectBlocks.Enabled = True
End Sub
Private Function PtInRect(r As Rectangle, p As POINT) As Boolean

PtInRect = False

If p.X >= r.Left And p.X <= r.Right Then
  If p.Y >= r.Top And p.Y <= r.Bottom Then
    PtInRect = True
    Exit Function
  End If
End If

End Function

Private Sub RefreshPopUpMenu()
        Dim BlkObj As Long
        Dim BlockType As Long
        Dim r As Long
        Dim m As Long

        NsOCR.Img_GetBlock ImgObj, pmBlockTag, BlkObj
        BlockType = NsOCR.Blk_GetType(BlkObj)
        
        If BlockType = BT_OCRTEXT Then
            miMachinePrintedText.Checked = True
        Else: miMachinePrintedText.Checked = False
        End If
        If BlockType = BT_OCRDIGIT Then
            miMachinePrintedDigits.Checked = True
        Else: miMachinePrintedDigits.Checked = False
        End If
        If BlockType = BT_ICRDIGIT Then
            miHandwrittenDigits.Checked = True
        Else: miHandwrittenDigits.Checked = False
        End If
        If BlockType = BT_BARCODE Then
            miBarCode.Checked = True
        Else: miBarCode.Checked = False
        End If
        If BlockType = BT_PICTURE Then
            miPicture.Checked = True
        Else: miPicture.Checked = False
        End If
        If BlockType = BT_CLEAR Then
            miClearArea.Checked = True
        Else: miClearArea.Checked = False
        End If
        If BlockType = BT_ZONING Then
            miZoningArea.Checked = True
        Else: miZoningArea.Checked = False
        End If
        If BlockType = BT_TABLE Then
            miTable.Checked = True
        Else: miTable.Checked = False
        End If
        If BlockType = BT_MRZ Then
            miMRZ.Checked = True
        Else: miMRZ.Checked = False
        End If
        If NsOCR.Blk_Inversion(BlkObj, BLK_INVERSE_GET) <> 0 Then
            miImageInverted.Checked = True
        Else: miImageInverted.Checked = False
        End If

        r = NsOCR.Blk_Rotation(BlkObj, BLK_ROTATE_GET)
        If r = BLK_ROTATE_270 Then
            miImageCW90.Checked = True
        Else: miImageCW90.Checked = False
        End If
        If r = BLK_ROTATE_180 Then
            miImage180.Checked = True
        Else: miImage180.Checked = False
        End If
        If r = BLK_ROTATE_90 Then
            miImageCCW90.Checked = True
        Else: miImageCCW90.Checked = False
        End If

        m = NsOCR.Blk_Mirror(BlkObj, BLK_MIRROR_GET)
        If (m And BLK_MIRROR_H) <> 0 Then
            miImageMirrorH.Checked = True
        Else: miImageMirrorH.Checked = False
        End If
        If (m And BLK_MIRROR_V) <> 0 Then
            miImageMirrorV.Checked = True
        Else: miImageMirrorV.Checked = False
        End If
    End Sub
    
Private Sub miBarCode_Click()

    Dim BlkObj As Long
    NsOCR.Img_GetBlock ImgObj, pmBlockTag, BlkObj
    NsOCR.Blk_SetType BlkObj, BT_BARCODE
    Call ShowImage

End Sub

Private Sub miClearArea_Click()
    Dim BlkObj As Long
    NsOCR.Img_GetBlock ImgObj, pmBlockTag, BlkObj
    NsOCR.Blk_SetType BlkObj, BT_CLEAR
    Call ShowImage
End Sub

Private Sub miDeleteZone_Click()
    Dim BlkObj As Long
    NsOCR.Img_GetBlock ImgObj, pmBlockTag, BlkObj
    Dim m As Long
    m = NsOCR.Blk_Mirror(BlkObj, -1)
    NsOCR.Img_DeleteBlock ImgObj, BlkObj
    Call ShowImage
End Sub

Private Sub miHandwrittenDigits_Click()
    Dim BlkObj As Long
    NsOCR.Img_GetBlock ImgObj, pmBlockTag, BlkObj
    NsOCR.Blk_SetType BlkObj, BT_ICRDIGIT
    Call ShowImage
End Sub

Private Sub miImage180_Click()
    Dim BlkObj As Long
    miImage180.Checked = Not miImage180.Checked
    NsOCR.Img_GetBlockImgObj , pmBlockTag, BlkObj
    If Not miImage180.Checked Then
        NsOCR.Blk_Rotation BlkObj, BLK_ROTATE_NONE
    Else: NsOCR.Blk_Rotation BlkObj, BLK_ROTATE_180
    End If
End Sub

Private Sub miImageCCW90_Click()
    Dim BlkObj As Long
    miImageCCW90.Checked = Not miImageCCW90.Checked
    NsOCR.Img_GetBlock ImgObj, pmBlockTag, BlkObj
    If Not miImageCCW90.Checked Then
      NsOCR.Blk_Rotation BlkObj, BLK_ROTATE_NONE
    Else: NsOCR.Blk_Rotation BlkObj, BLK_ROTATE_270
    End If
End Sub

Private Sub miImageCW90_Click()
    Dim BlkObj As Long
    miImageCW90.Checked = Not miImageCW90.Checked
    NsOCR.Img_GetBlock ImgObj, pmBlockTag, BlkObj
    If Not miImageCW90.Checked Then
      NsOCR.Blk_Rotation BlkObj, BLK_ROTATE_NONE
    Else: NsOCR.Blk_Rotation BlkObj, BLK_ROTATE_90
    End If
End Sub

Private Sub miImageInverted_Click()
    Dim BlkObj As Long
    miImageInverted.Checked = Not miImageInverted.Checked
    NsOCR.Img_GetBlock ImgObj, pmBlockTag, BlkObj
    If miImageInverted.Checked Then
      NsOCR.Blk_Inversion BlkObj, BLK_INVERSE_SET1
    Else: NsOCR.Blk_Inversion BlkObj, BLK_INVERSE_SET0
    End If
End Sub

Private Sub miImageMirrorH_Click()
    Dim BlkObj As Long
    miImageMirrorH.Checked = Not miImageMirrorH.Checked
    NsOCR.Img_GetBlock ImgObj, pmBlockTag, BlkObj
    Dim m As Long
    m = NsOCR.Blk_Mirror(BlkObj, -1)
    If miImageMirrorH.Checked Then
      m = m Or BLK_MIRROR_H
    Else: m = m And Not BLK_MIRROR_H
    End If
    NsOCR.Blk_Mirror BlkObj, m
End Sub

Private Sub miImageMirrorV_Click()
    Dim BlkObj As Long
    miImageMirrorV.Checked = Not miImageMirrorV.Checked
    NsOCR.Img_GetBlock ImgObj, pmBlockTag, BlkObj
    Dim m As Long
    m = NsOCR.Blk_Mirror(BlkObj, -1)
    If miImageMirrorV.Checked Then
      m = m Or BLK_MIRROR_V
    Else: m = m And Not BLK_MIRROR_V
    End If
    NsOCR.Blk_Mirror BlkObj, m
End Sub

Private Sub miMachinePrintedDigits_Click()
    Dim BlkObj As Long
    NsOCR.Img_GetBlock ImgObj, pmBlockTag, BlkObj
    NsOCR.Blk_SetType BlkObj, BT_OCRDIGIT
    Call ShowImage
End Sub

Private Sub miMachinePrintedText_Click()
    Dim BlkObj As Long
    NsOCR.Img_GetBlock ImgObj, pmBlockTag, BlkObj
    NsOCR.Blk_SetType BlkObj, BT_OCRTEXT
    Call ShowImage
End Sub

Private Sub miPicture_Click()
    Dim BlkObj As Long
    NsOCR.Img_GetBlock ImgObj, pmBlockTag, BlkObj
    NsOCR.Blk_SetType BlkObj, BT_PICTURE
    Call ShowImage
End Sub

Private Sub miTable_Click()
    Dim BlkObj As Long
    NsOCR.Img_GetBlock ImgObj, pmBlockTag, BlkObj
    NsOCR.Blk_SetType BlkObj, BT_TABLE
    Call ShowImage
End Sub

Private Sub miZoningArea_Click()
    Dim BlkObj As Long
    NsOCR.Img_GetBlock ImgObj, pmBlockTag, BlkObj
    NsOCR.Blk_SetType BlkObj, BT_ZONING
    Call ShowImage
End Sub

Private Sub miMRZ_Click()
    Dim BlkObj As Long
    NsOCR.Img_GetBlock ImgObj, pmBlockTag, BlkObj
    NsOCR.Blk_SetType BlkObj, BT_MRZ
    Call ShowImage
End Sub

Private Sub Picture1_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)

    If Not IsImgLoaded() Then Exit Sub

    If Button = 2 Then
       
      Dim p As POINT
      p.X = X
      p.Y = Y
            
      Dim kk As Double
      kk = GetCurDocScale()
            
      Dim cnt As Integer
            
      cnt = NsOCR.Img_GetBlockCnt(ImgObj)
      Dim n As Integer
      n = -1
            
      Dim minsize As Integer
      minsize = -1
            
      Dim i As Long
      Dim BlkObj  As Long
      Dim Xpos  As Long
      Dim Ypos  As Long
      Dim Width  As Long
      Dim Height  As Long
      Dim sz As Long
      Dim r As Rectangle
            
      For i = 0 To cnt - 1
        NsOCR.Img_GetBlock ImgObj, i, BlkObj
        NsOCR.Blk_GetRect BlkObj, Xpos, Ypos, Width, Height
                
        r.Left = (kk * Xpos)
        r.Top = (kk * Ypos)
        r.Right = r.Left + (kk * Width) - 1
        r.Bottom = r.Top + (kk * Height) - 1

        If PtInRect(r, p) Then
          'need to find smallest block because blocks may overlap
          If Width < Height Then
            sz = Width
          Else: sz = Height
          End If

          If (minsize = -1) Or (sz < minsize) Then
            minsize = sz
            n = i
          End If
        
       End If
     Next i

     If (n = -1) Then Exit Sub 'block not found
     
     pmBlockTag = n 'remember block index
     
     Call RefreshPopUpMenu
     PopupMenu mipopup
     
     Exit Sub
 End If
        
                     
 If Button = 1 Then
 
   If Dwn Then Exit Sub
   
   Dim w As Long
   Dim h As Long
            
   NsOCR.Img_GetSize ImgObj, w, h

   Dwn = True
   Dim k As Double
            
   k = GetCurDocScale()
                      
   Frame.Left = (1 / k * X)
   If Frame.Left < 0 Then Frame.Left = 0
   If Frame.Left > w Then Frame.Left = w
            
   Frame.Top = (1 / k * Y)
   If Frame.Top < 0 Then Frame.Top = 0
   If Frame.Top > h Then Frame.Top = h
            
   Frame.Right = Frame.Left
   Frame.Bottom = Frame.Top
            
  End If

  Call ShowImage
  
End Sub

Private Sub Picture1_MouseMove(Button As Integer, Shift As Integer, X As Single, Y As Single)

        If Not Dwn Then Exit Sub

        If Not IsImgLoaded() Then Exit Sub

        Dim w As Long
        Dim h As Long
        
        NsOCR.Img_GetSize ImgObj, w, h

        Dim k As Double
        k = GetCurDocScale()
        
        Frame.Right = (1 / k * X) '- Frame.Left + 1
        If Frame.Right < Frame.Left Then Frame.Right = Frame.Left
        If Frame.Right >= w Then Frame.Right = w - 1

        Frame.Bottom = (1 / k * Y) '- Frame.Top + 1
        If Frame.Bottom < Frame.Top Then Frame.Bottom = Frame.Top
        If Frame.Bottom >= h Then Frame.Bottom = h - 1

        Call ShowImage
End Sub

Private Sub Picture1_MouseUp(Button As Integer, Shift As Integer, X As Single, Y As Single)

    Dim BlkObj As Long
    Dim w As Long
    Dim h As Long
    Dim res As Long
    
    If Not Dwn Then Exit Sub
    
    Dwn = False

    If Not IsImgLoaded() Then Exit Sub

    NsOCR.Img_GetSize ImgObj, w, h
    
    If Frame.Right >= w Then Frame.Right = w - 1
    If Frame.Bottom >= h Then Frame.Bottom = h - 1

    w = Frame.Right - Frame.Left + 1
    h = Frame.Bottom - Frame.Top + 1
    
    If (w < 8) Or (h < 8) Then
        Call ShowImage
        Exit Sub
    End If

    res = NsOCR.Img_AddBlock(ImgObj, Frame.Left, Frame.Top, w, h, BlkObj)
    
    If res > ERROR_FIRST Then
        ShowError "Img_AddBlock", res
        Exit Sub
    End If

    'detect text block inversion
    NsOCR.Blk_Inversion BlkObj, BLK_INVERSE_DETECT
    Call ShowImage
    
End Sub

Private Sub Picture1_Paint()
    'Picture1.Line (0, 0)-(Picture1.Width - 1, Picture1.Height - 1), RGB(&HFF, &HFF, &HFF), BF
    'и выводим на экран dc2, только  если перед этим рисовали на dc2
    Call BitBlt(Picture1.hdc, 0, 0, Picture1.Width, Picture1.Height, dc2, 0, 0, vbSrcCopy)
End Sub
