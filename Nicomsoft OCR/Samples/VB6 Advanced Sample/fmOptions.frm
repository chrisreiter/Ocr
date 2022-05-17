VERSION 5.00
Begin VB.Form fmOptions 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "OCR Options"
   ClientHeight    =   7770
   ClientLeft      =   45
   ClientTop       =   375
   ClientWidth     =   7245
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   518
   ScaleMode       =   3  'Pixel
   ScaleWidth      =   483
   ShowInTaskbar   =   0   'False
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton bkCancel 
      Caption         =   "Cancel"
      Height          =   345
      Left            =   3420
      TabIndex        =   24
      Top             =   7275
      Width           =   1125
   End
   Begin VB.CommandButton bkOK 
      Caption         =   "OK"
      Height          =   345
      Left            =   2205
      TabIndex        =   23
      Top             =   7275
      Width           =   1125
   End
   Begin VB.TextBox edPDFDPI 
      Height          =   300
      Left            =   3315
      TabIndex        =   22
      Top             =   6270
      Width           =   795
   End
   Begin VB.TextBox edTextQual 
      Height          =   300
      Left            =   3315
      TabIndex        =   21
      Top             =   5880
      Width           =   795
   End
   Begin VB.TextBox edBinThreshold 
      Height          =   300
      Left            =   3315
      TabIndex        =   20
      Top             =   5490
      Width           =   795
   End
   Begin VB.TextBox edDisabledChars 
      Height          =   300
      Left            =   1440
      TabIndex        =   16
      Top             =   5040
      Width           =   4800
   End
   Begin VB.TextBox edEnabledChars 
      Height          =   300
      Left            =   1440
      TabIndex        =   15
      Top             =   4650
      Width           =   4800
   End
   Begin VB.CheckBox cbBinTwice 
      Caption         =   "Binarize twice"
      Height          =   255
      Left            =   225
      TabIndex        =   11
      Top             =   4140
      Width           =   1365
   End
   Begin VB.CheckBox cbFastMode 
      Caption         =   "Fast mode (less accurate)"
      Height          =   255
      Left            =   225
      TabIndex        =   10
      Top             =   3795
      Width           =   2205
   End
   Begin VB.CheckBox cbGrayMode 
      Caption         =   "Gray mode"
      Height          =   255
      Left            =   225
      TabIndex        =   9
      Top             =   3450
      Width           =   1155
   End
   Begin VB.CheckBox cbRemoveLines 
      Caption         =   "Detect and remove lines"
      Height          =   255
      Left            =   225
      TabIndex        =   8
      Top             =   3105
      Width           =   2115
   End
   Begin VB.CheckBox cbImgNoiseFilter 
      Caption         =   "Apply noise filter for image"
      Height          =   255
      Left            =   225
      TabIndex        =   7
      Top             =   2760
      Width           =   2220
   End
   Begin VB.CheckBox cbRotation 
      Caption         =   "Detect and fix image rotation 90/180/270 degrees"
      Height          =   255
      Left            =   225
      TabIndex        =   6
      Top             =   2415
      Width           =   3945
   End
   Begin VB.CheckBox cbDeskew 
      Caption         =   "Detect and fix image skew"
      Height          =   255
      Left            =   225
      TabIndex        =   5
      Top             =   2070
      Width           =   2265
   End
   Begin VB.CheckBox cbZonesInversion 
      Caption         =   "Detect zones inversion"
      Height          =   255
      Left            =   225
      TabIndex        =   4
      Top             =   1725
      Width           =   2010
   End
   Begin VB.CheckBox cbImgInversion 
      Caption         =   "Detect image inversion"
      Height          =   255
      Left            =   225
      TabIndex        =   3
      Top             =   1380
      Width           =   2010
   End
   Begin VB.CheckBox cbFindBarcodes 
      Caption         =   "Find barcodes"
      Height          =   255
      Left            =   225
      TabIndex        =   2
      Top             =   1035
      Width           =   1395
   End
   Begin VB.Label Label8 
      Caption         =   "PDF rendering DPI:"
      Height          =   195
      Left            =   180
      TabIndex        =   19
      Top             =   6315
      Width           =   1485
   End
   Begin VB.Label Label7 
      Caption         =   "Text quality (0..100; -1 - auto):"
      Height          =   195
      Left            =   180
      TabIndex        =   18
      Top             =   5925
      Width           =   2220
   End
   Begin VB.Label Label6 
      Caption         =   "Binarization threshold (0..254; 255 - auto):"
      Height          =   195
      Left            =   180
      TabIndex        =   17
      Top             =   5535
      Width           =   3045
   End
   Begin VB.Label Label5 
      Caption         =   "Disabled chars:"
      Height          =   195
      Left            =   180
      TabIndex        =   14
      Top             =   5145
      Width           =   1200
   End
   Begin VB.Label Label4 
      Caption         =   "Enabled chars:"
      Height          =   195
      Left            =   180
      TabIndex        =   13
      Top             =   4755
      Width           =   1170
   End
   Begin VB.Label Label3 
      Caption         =   "NOTE: Some options will not be applied  immediately, reload image to apply all changes."
      ForeColor       =   &H000000FF&
      Height          =   195
      Left            =   180
      TabIndex        =   12
      Top             =   6885
      Width           =   6300
   End
   Begin VB.Label Label2 
      Caption         =   "Check ""Configuration"" and ""Performance Tips"" sections of documentation for more options."
      ForeColor       =   &H000000FF&
      Height          =   195
      Left            =   180
      TabIndex        =   1
      Top             =   465
      Width           =   6570
   End
   Begin VB.Label Label1 
      Caption         =   "You can improve OCR performance by selecting only necessary features."
      ForeColor       =   &H000000FF&
      Height          =   195
      Left            =   180
      TabIndex        =   0
      Top             =   135
      Width           =   5265
   End
End
Attribute VB_Name = "fmOptions"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Sub bkCancel_Click()
    Unload Me
End Sub

Private Sub bkOK_Click()
Dim val As String

        If cbFindBarcodes.Value = 1 Then
            val = "1"
        Else
            val = "0"
        End If
        Call fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, BT_DEFAULT, "Zoning/FindBarcodes", val)

        If cbImgInversion.Value = 1 Then
            val = "2"
        Else
            val = "0"
        End If
        Call fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, BT_DEFAULT, "ImgAlizer/Inversion", val)

        If cbZonesInversion.Value = 1 Then
            val = "1"
        Else
            val = "0"
        End If
        Call fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, BT_DEFAULT, "Zoning/DetectInversion", val)

        If cbDeskew.Value = 1 Then
            val = "360"
        Else
            val = "0"
        End If
        Call fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, BT_DEFAULT, "ImgAlizer/SkewAngle", val)

        If cbRotation.Value = 1 Then
            val = "1"
        Else
            val = "0"
        End If
        Call fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, BT_DEFAULT, "ImgAlizer/AutoRotate", val)

        If cbImgNoiseFilter.Value = 1 Then
            val = "1"
        Else
            val = "0"
        End If
        Call fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, BT_DEFAULT, "ImgAlizer/NoiseFilter", val)

        If cbRemoveLines.Value = 1 Then
            val = "1"
        Else
            val = "0"
        End If
        Call fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, BT_DEFAULT, "PixLines/RemoveLines", val)
        Call fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, BT_DEFAULT, "PixLines/FindHorLines", val)
        Call fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, BT_DEFAULT, "PixLines/FindVerLines", val)

        If cbGrayMode.Value = 1 Then
            val = "1"
        Else
            val = "0"
        End If
        Call fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, BT_DEFAULT, "Main/GrayMode", val)

        If cbFastMode.Value = 1 Then
            val = "1"
        Else
            val = "0"
        End If
        Call fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, BT_DEFAULT, "Main/FastMode", val)

        If cbBinTwice.Value = 1 Then
            val = "1"
        Else
            val = "0"
        End If
        Call fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, BT_DEFAULT, "Binarizer/BinTwice", val)

        Call fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, BT_DEFAULT, "Main/EnabledChars", edEnabledChars.Text)

        Call fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, BT_DEFAULT, "Main/DisabledChars", edDisabledChars.Text)

        Call fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, BT_DEFAULT, "Binarizer/SimpleThr", edBinThreshold.Text)

        Call fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, BT_DEFAULT, "WordAlizer/TextQual", edTextQual.Text)

        Call fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, BT_DEFAULT, "Main/PdfDPI", edPDFDPI.Text)

        Unload Me
End Sub

Private Sub Form_Load()
        Dim val As String
            
        val = ""
        
        Call fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, BT_DEFAULT, "Zoning/FindBarcodes", val)
        If val = "1" Then
            cbFindBarcodes.Value = 1
        Else
            cbFindBarcodes.Value = 0
        End If

        Call fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, BT_DEFAULT, "ImgAlizer/Inversion", val)
        If val = "2" Then
            cbImgInversion.Value = 1
        Else
            cbImgInversion.Value = 0
        End If

        Call fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, BT_DEFAULT, "Zoning/DetectInversion", val)
        If val = "1" Then
            cbZonesInversion.Value = 1
        Else
            cbZonesInversion.Value = 0
        End If

        Call fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, BT_DEFAULT, "ImgAlizer/SkewAngle", val)
        If val = "360" Then
            cbDeskew.Value = 1
        Else
            cbDeskew.Value = 0
        End If

        Call fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, BT_DEFAULT, "ImgAlizer/AutoRotate", val)
        If val = "1" Then
            cbRotation.Value = 1
        Else
            cbRotation.Value = 0
        End If

        Call fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, BT_DEFAULT, "ImgAlizer/NoiseFilter", val)
        If val = "1" Then
            cbImgNoiseFilter.Value = 1
        Else
            cbImgNoiseFilter.Value = 0
        End If

        Call fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, BT_DEFAULT, "PixLines/RemoveLines", val)
        If val = "1" Then
            cbRemoveLines.Value = 1
        Else
            cbRemoveLines.Value = 0
        End If

        Call fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, BT_DEFAULT, "Main/GrayMode", val)
        If val = "1" Then
            cbGrayMode.Value = 1
        Else
            cbGrayMode.Value = 0
        End If

        Call fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, BT_DEFAULT, "Main/FastMode", val)
        If val = "1" Then
            cbFastMode.Value = 1
        Else
            cbFastMode.Value = 0
        End If

        Call fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, BT_DEFAULT, "Binarizer/BinTwice", val)
        If val = "1" Then
            cbBinTwice.Value = 1
        Else
            cbBinTwice.Value = 0
        End If

        Call fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, BT_DEFAULT, "Main/EnabledChars", val)
        edEnabledChars.Text = val

        Call fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, BT_DEFAULT, "Main/DisabledChars", val)
        edDisabledChars.Text = val

        Call fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, BT_DEFAULT, "Binarizer/SimpleThr", val)
        edBinThreshold.Text = val

        Call fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, BT_DEFAULT, "WordAlizer/TextQual", val)
        edTextQual.Text = val

        Call fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, BT_DEFAULT, "Main/PdfDPI", val)
        edPDFDPI.Text = val

End Sub
