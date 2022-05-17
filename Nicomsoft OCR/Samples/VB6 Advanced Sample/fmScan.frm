VERSION 5.00
Begin VB.Form fmScan 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Scanning"
   ClientHeight    =   2655
   ClientLeft      =   7545
   ClientTop       =   5235
   ClientWidth     =   7095
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2655
   ScaleWidth      =   7095
   ShowInTaskbar   =   0   'False
   Begin VB.ComboBox cbSrc 
      Height          =   315
      ItemData        =   "fmScan.frx":0000
      Left            =   1680
      List            =   "fmScan.frx":000D
      Style           =   2  'Dropdown List
      TabIndex        =   7
      Top             =   960
      Width           =   3135
   End
   Begin VB.CheckBox cbNoUI 
      Caption         =   "No Scanner UI"
      Height          =   255
      Left            =   120
      TabIndex        =   6
      Top             =   1800
      Width           =   1455
   End
   Begin VB.CommandButton bkSetDefault 
      Caption         =   "Set as Default"
      Height          =   375
      Left            =   5640
      TabIndex        =   4
      Top             =   600
      Width           =   1335
   End
   Begin VB.ComboBox cbScanners 
      Height          =   315
      Left            =   1680
      Style           =   2  'Dropdown List
      TabIndex        =   2
      Top             =   240
      Width           =   5295
   End
   Begin VB.CommandButton bkCancel 
      Caption         =   "Cancel"
      Height          =   375
      Left            =   3480
      TabIndex        =   1
      Top             =   2160
      Width           =   1215
   End
   Begin VB.CommandButton bkScan 
      Caption         =   "Scan"
      Height          =   375
      Left            =   2040
      TabIndex        =   0
      Top             =   2160
      Width           =   1215
   End
   Begin VB.Label Label2 
      Caption         =   "Document Source:"
      Height          =   255
      Left            =   120
      TabIndex        =   5
      Top             =   1080
      Width           =   1455
   End
   Begin VB.Label Label1 
      Caption         =   "Select device:"
      Height          =   255
      Left            =   120
      TabIndex        =   3
      Top             =   240
      Width           =   1215
   End
End
Attribute VB_Name = "fmScan"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Option Explicit

Private Sub bkCancel_Click()
  Unload Me
End Sub

Private Sub bkScan_Click()

        Dim n As Long, flags As Long, res As Long
        n = cbScanners.ListIndex
        If n < 0 Then
            MsgBox "No scanner selected!"
            Exit Sub
        End If

        flags = 0
        Select Case cbSrc.ListIndex
            Case 0 'flatbed
            Case 1: flags = flags + SCAN_SOURCEADF  'adf
            Case 2: flags = flags + SCAN_SOURCEAUTO  'auto
        End Select

        If cbNoUI.Value Then
            flags = flags + SCAN_NOUI
        End If

        res = fmMain.NsOCR.Scan_ScanToImg(fmMain.ScanObj, fmMain.ImgObj, n, flags)
        ' res = fmMain.NsOCR.Scan_ScanToFile(fmMain.ScanObj, "c:\temp.tif", n, flags)

        If res > ERROR_FIRST Then
            fmMain.ShowError "Scan_ScanToImg", res
            Exit Sub
        End If

        Call fmMain.DoImageLoaded
        Unload Me
End Sub

Private Sub bkSetDefault_Click()
        Dim res As Long, n As Long
        n = cbScanners.ListIndex
        If n < 0 Then
            Exit Sub
        End If

        Dim buf As String
        buf = ""
        res = fmMain.NsOCR.Scan_Enumerate(fmMain.ScanObj, buf, SCAN_SETDEFAULTDEVICE + n)
        If res > ERROR_FIRST Then
            fmMain.ShowError "Scan_Enumerate (2)", res
            Exit Sub
        End If
End Sub

Private Sub Form_Load()
        cbSrc.ListIndex = 0
        Call cbScanners.Clear

        Dim res As Long
        Dim buf As String

        buf = ""
        res = fmMain.NsOCR.Scan_Enumerate(fmMain.ScanObj, buf, 0)
        
        If res > ERROR_FIRST Then
            fmMain.ShowError "Scan_Enumerate", res
            Exit Sub
        End If

        'names are separated by comma, parse them
        Dim r As String
        Dim i As Long
        
        r = ""
        
        For i = 1 To Len(buf)

            If Mid(buf, i, 1) <> "," Then
                r = r + Mid(buf, i, 1)
            Else
                cbScanners.AddItem (r)
                r = ""
            End If
        Next i

        If r <> "" Then
            cbScanners.AddItem (r)
        End If

        'now get default device index
        res = fmMain.NsOCR.Scan_Enumerate(fmMain.ScanObj, buf, SCAN_GETDEFAULTDEVICE)
        
        If res > ERROR_FIRST Then
            If res <> ERROR_NODEFAULTDEVICE Then
                fmMain.ShowError "Scan_Enumerate (1)", res
                Exit Sub
            End If
        Else
        
        If (res < cbScanners.ListCount) Then
                cbScanners.ListIndex = res
            End If
        End If
End Sub
