Public Class Form3
    Public fmMain As Form1
    Public res As Integer
    Public mode As Integer

    Private Function CheckDone() As Boolean
        If mode = 0 Then
            res = fmMain.NsOCR.Img_OCR(fmMain.ImgObj, 0, 0, TNSOCR.OCRFLAG_GETRESULT)
        Else
            res = fmMain.NsOCR.Ocr_ProcessPages(fmMain.ImgObj, 0, 0, 0, 0, TNSOCR.OCRFLAG_GETRESULT)
        End If

        CheckDone = res <> TNSOCR.ERROR_PENDING
    End Function

    Private Sub Form3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Timer1.Enabled = True
        ProgressBar1.Value = 0
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If CheckDone() Then
            Close()
        End If
        Dim val As Integer

        If mode = 0 Then
            val = fmMain.NsOCR.Img_OCR(fmMain.ImgObj, 0, 0, TNSOCR.OCRFLAG_GETPROGRESS)
        Else
            val = fmMain.NsOCR.Ocr_ProcessPages(fmMain.ImgObj, 0, 0, 0, 0, TNSOCR.OCRFLAG_GETPROGRESS)
        End If

        If val < TNSOCR.ERROR_FIRST Then
            If ProgressBar1.Value <> val Then
                ProgressBar1.Value = val
            End If
        End If
    End Sub

    Private Sub Form3_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Timer1.Enabled = False
        While Not CheckDone() 'make sure that Img_OCR is done
            System.Threading.Thread.Sleep(10)
        End While
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'cancel work
        If mode = 0 Then
            fmMain.NsOCR.Img_OCR(fmMain.ImgObj, 0, 0, TNSOCR.OCRFLAG_CANCEL)
        Else
            fmMain.NsOCR.Ocr_ProcessPages(fmMain.ImgObj, 0, 0, 0, 0, TNSOCR.OCRFLAG_CANCEL)
        End If

        'we must wait for result anyway since OCRFLAG_CANCEL request returns immediately, work is not stopped yet
        Close() 'this function waits for result
    End Sub
End Class

