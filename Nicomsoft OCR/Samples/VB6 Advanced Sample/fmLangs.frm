VERSION 5.00
Begin VB.Form fmLangs 
   BorderStyle     =   4  'Fixed ToolWindow
   Caption         =   "Languages"
   ClientHeight    =   8490
   ClientLeft      =   9570
   ClientTop       =   2550
   ClientWidth     =   3210
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   8490
   ScaleWidth      =   3210
   ShowInTaskbar   =   0   'False
   Begin VB.CommandButton bkCancel 
      Cancel          =   -1  'True
      Caption         =   "Cancel"
      Height          =   375
      Left            =   1680
      TabIndex        =   2
      Top             =   7920
      Width           =   1215
   End
   Begin VB.CommandButton bkOK 
      Caption         =   "OK"
      Height          =   375
      Left            =   240
      TabIndex        =   1
      Top             =   7920
      Width           =   1215
   End
   Begin VB.Frame Frame1 
      Height          =   7695
      Left            =   240
      TabIndex        =   0
      Top             =   120
      Width           =   2775
      Begin VB.ListBox cbLanguages 
         Height          =   7260
         ItemData        =   "fmLangs.frx":0000
         Left            =   120
         List            =   "fmLangs.frx":0052
         Style           =   1  'Checkbox
         TabIndex        =   3
         Top             =   240
         Width           =   2535
      End
   End
End
Attribute VB_Name = "fmLangs"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False


    Private Sub LoadLanguages()
        Dim i As Integer
        Dim val As String
        val = ""
        
        For i = 0 To cbLanguages.ListCount - 1
            If (fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, BT_DEFAULT, "Languages/" + cbLanguages.List(i), val) < ERROR_FIRST) Then
                cbLanguages.Selected(i) = (val = "1")
            Else
                cbLanguages.Selected(i) = False
            End If

        Next i
    End Sub

    Private Function ApplyLanguages() As Boolean
        Dim i As Integer
        Dim ch As Boolean
        
        ch = False
        For i = 0 To cbLanguages.ListCount - 1
            If cbLanguages.Selected(i) Then
                ch = True
            End If
        Next i
        
        If ch = False Then
            MsgBox "Please select at least one language for recognition."
            ApplyLanguages = False
            Exit Function
        End If
        
        For i = 0 To cbLanguages.ListCount - 1
            If cbLanguages.Selected(i) Then
                fmMain.NsOCR.Cfg_SetOption fmMain.CfgObj, BT_DEFAULT, "Languages/" + cbLanguages.List(i), "1"
            Else
                fmMain.NsOCR.Cfg_SetOption fmMain.CfgObj, BT_DEFAULT, "Languages/" + cbLanguages.List(i), "0"
            End If
        Next i
        
        ApplyLanguages = True
    End Function

Private Sub bkCancel_Click()
    Unload Me
End Sub

Private Sub bkOK_Click()
    If ApplyLanguages() Then
        Unload Me
    End If
End Sub

Private Sub Form_Load()
    Call LoadLanguages
End Sub
