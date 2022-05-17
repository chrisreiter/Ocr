VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.1#0"; "MSCOMCTL.OCX"
Begin VB.Form fmWait 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Please Wait"
   ClientHeight    =   1455
   ClientLeft      =   8160
   ClientTop       =   5445
   ClientWidth     =   6030
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   1455
   ScaleWidth      =   6030
   ShowInTaskbar   =   0   'False
   Begin VB.Timer Timer1 
      Enabled         =   0   'False
      Interval        =   100
      Left            =   360
      Top             =   960
   End
   Begin MSComctlLib.ProgressBar ProgressBar1 
      Height          =   375
      Left            =   120
      TabIndex        =   1
      Top             =   360
      Width           =   5775
      _ExtentX        =   10186
      _ExtentY        =   661
      _Version        =   393216
      Appearance      =   1
   End
   Begin VB.CommandButton CancelButton 
      Caption         =   "Cancel"
      Height          =   375
      Left            =   2160
      TabIndex        =   0
      Top             =   960
      Width           =   1215
   End
End
Attribute VB_Name = "fmWait"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False

Option Explicit

Public res As Long
Public mode As Long

Private Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Long)

Private Function CheckDone() As Boolean
    If mode = 0 Then
        res = fmMain.NsOCR.Img_OCR(fmMain.ImgObj, 0, 0, OCRFLAG_GETRESULT)
    Else
        res = fmMain.NsOCR.Ocr_ProcessPages(fmMain.ImgObj, 0, 0, 0, 0, OCRFLAG_GETRESULT)
    End If

    CheckDone = res <> ERROR_PENDING
End Function

Private Sub CancelButton_Click()
        'cancel work
        If mode = 0 Then
            fmMain.NsOCR.Img_OCR fmMain.ImgObj, 0, 0, OCRFLAG_CANCEL
        Else
            fmMain.NsOCR.Ocr_ProcessPages fmMain.ImgObj, 0, 0, 0, 0, OCRFLAG_CANCEL
        End If

        'we must wait for result anyway since OCRFLAG_CANCEL request returns immediately, work is not stopped yet
        Unload Me 'this function waits for result
End Sub

Private Sub Form_Load()
    Timer1.Enabled = True
    ProgressBar1.Value = 0
End Sub

Private Sub Form_Unload(Cancel As Integer)
    Timer1.Enabled = False
    
    Do While Not CheckDone() 'make sure that Img_OCR is done
        Sleep (10)
    Loop
End Sub

Private Sub Timer1_Timer()
        If CheckDone() Then
            Unload Me
        End If
        
        Dim val As Integer

        If mode = 0 Then
            val = fmMain.NsOCR.Img_OCR(fmMain.ImgObj, 0, 0, OCRFLAG_GETPROGRESS)
        Else
            val = fmMain.NsOCR.Ocr_ProcessPages(fmMain.ImgObj, 0, 0, 0, 0, OCRFLAG_GETPROGRESS)
        End If

        If val < ERROR_FIRST Then
            If ProgressBar1.Value <> val Then
                ProgressBar1.Value = val
            End If
        End If
End Sub
