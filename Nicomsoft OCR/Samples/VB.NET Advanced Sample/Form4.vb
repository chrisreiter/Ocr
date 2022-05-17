Public Class Form4
    Public fmMain As Form1
    Private Sub LoadLanguages()
        Dim i As Integer
        Dim val As String = ""
        For i = 0 To cbLanguages.Items.Count - 1
            If (fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Languages/" + cbLanguages.Items(i), val) < TNSOCR.ERROR_FIRST) Then
                cbLanguages.SetItemChecked(i, val = "1")
            Else
                cbLanguages.SetItemChecked(i, False)
            End If
        Next i
        For i = 0 To cbLanguagesAsian.Items.Count - 1
            If (fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Languages/" + cbLanguagesAsian.Items(i), val) < TNSOCR.ERROR_FIRST) Then
                cbLanguagesAsian.SetItemChecked(i, val = "1")
            Else
                cbLanguagesAsian.SetItemChecked(i, False)
            End If
        Next i
    End Sub

    Private Function ApplyLanguages() As Boolean
        Dim i As Integer
        Dim ch1 As Boolean = False
        For i = 0 To cbLanguages.Items.Count - 1
            If cbLanguages.GetItemChecked(i) Then
                ch1 = True
            End If
        Next i

        Dim ch2 As Integer = 0
        For i = 0 To cbLanguagesAsian.Items.Count - 1
            If cbLanguagesAsian.GetItemChecked(i) Then
                ch2 = ch2 + 1
            End If
        Next i

        If (ch1 = False) And (ch2 = 0) Then
            System.Windows.Forms.MessageBox.Show("Please select at least one language for recognition.")
            Return False
        End If

        If (ch1 = True) And (ch2 > 0) Then
            System.Windows.Forms.MessageBox.Show("Using both main and asian languages in same zone is not supported.")
            Return False
        End If

        If ch2 > 1 Then
            System.Windows.Forms.MessageBox.Show("Using two or more asian languages in same zone is not supported currently.")
            Return False
        End If

        For i = 0 To cbLanguages.Items.Count - 1
            If cbLanguages.GetItemChecked(i) Then
                fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Languages/" + cbLanguages.Items(i), "1")
            Else
                fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Languages/" + cbLanguages.Items(i), "0")
            End If
        Next i
        For i = 0 To cbLanguagesAsian.Items.Count - 1
            If cbLanguagesAsian.GetItemChecked(i) Then
                fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Languages/" + cbLanguagesAsian.Items(i), "1")
            Else
                fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Languages/" + cbLanguagesAsian.Items(i), "0")
            End If
        Next i
        Return True
    End Function

    Private Sub Form4_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadLanguages()
    End Sub

    Private Sub bkCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bkCancel.Click
        Close()
    End Sub

    Private Sub bkOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bkOK.Click
        If ApplyLanguages() Then
            Close()
        End If
    End Sub
End Class