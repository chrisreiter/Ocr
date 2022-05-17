Public Class Form22
    Public fmMain As Form1
    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cbSrc.SelectedIndex = 0

        cbScanners.Items.Clear()

        Dim res As Integer
        Dim buf As String

        buf = ""
        res = fmMain.NsOCR.Scan_Enumerate(fmMain.ScanObj, buf, 0)
        If res > TNSOCR.ERROR_FIRST Then
            fmMain.ShowError("Scan_Enumerate", res)
            Exit Sub
        End If

        'names are separated by comma, parse them
        Dim r As String
        Dim i As Integer
        r = ""
        For i = 0 To buf.Length - 1

            If buf(i) <> "," Then
                r = r + buf(i)
            Else
                cbScanners.Items.Add(r)
                r = ""
            End If
        Next i

        If r <> "" Then
            cbScanners.Items.Add(r)
        End If

        'now get default device index
        res = fmMain.NsOCR.Scan_Enumerate(fmMain.ScanObj, buf, TNSOCR.SCAN_GETDEFAULTDEVICE)
        If res > TNSOCR.ERROR_FIRST Then
            If res <> TNSOCR.ERROR_NODEFAULTDEVICE Then
                fmMain.ShowError("Scan_Enumerate (1)", res)
                Exit Sub
            End If
        Else
            If (res < cbScanners.Items.Count) Then
                cbScanners.SelectedIndex = res
            End If
        End If
    End Sub


    Private Sub bkCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bkCancel.Click
        Close()
    End Sub

    Private Sub bkSetDefault_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bkSetDefault.Click
        Dim res, n As Integer
        n = cbScanners.SelectedIndex
        If n < 0 Then
            Exit Sub
        End If

        Dim buf As String
        buf = ""
        res = fmMain.NsOCR.Scan_Enumerate(fmMain.ScanObj, buf, TNSOCR.SCAN_SETDEFAULTDEVICE + n)
        If res > TNSOCR.ERROR_FIRST Then
            fmMain.ShowError("Scan_Enumerate (2)", res)
            Exit Sub
        End If
    End Sub

    Private Sub bkScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bkScan.Click
        Dim n, flags, res As Integer
        n = cbScanners.SelectedIndex
        If n < 0 Then
            System.Windows.Forms.MessageBox.Show("No scanner selected!")
            Exit Sub
        End If

        flags = 0
        Select Case cbSrc.SelectedIndex
            Case 0 'flatbed
            Case 1 : flags = flags + TNSOCR.SCAN_SOURCEADF 'adf
            Case 2 : flags = flags + TNSOCR.SCAN_SOURCEAUTO 'auto
        End Select

        If cbNoUI.Checked Then
            flags = flags + TNSOCR.SCAN_NOUI
        End If

        res = fmMain.NsOCR.Scan_ScanToImg(fmMain.ScanObj, fmMain.ImgObj, n, flags)
        ' res = fmMain.NsOCR.Scan_ScanToFile(fmMain.ScanObj, "c:\temp.tif", n, flags)

        If res > TNSOCR.ERROR_FIRST Then
            fmMain.ShowError("Scan_ScanToImg", res)
            Exit Sub
        End If

        Close()
        fmMain.DoImageLoaded()
    End Sub
End Class