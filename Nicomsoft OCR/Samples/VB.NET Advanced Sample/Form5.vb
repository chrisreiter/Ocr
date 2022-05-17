Public Class Form5
    Public fmMain As Form1

    Private Sub bkHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bkHelp.Click
        fmMain.ShowHelp("config.htm")
    End Sub

    Private Sub bkCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bkCancel.Click
        Close()
    End Sub

    Private Sub Form5_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim val As String = ""

        fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Zoning/FindBarcodes", val)
        If val = "1" Then
            cbFindBarcodes.Checked = True
        Else
            cbFindBarcodes.Checked = False
        End If

        fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/Inversion", val)
        If val = "2" Then
            cbImgInversion.Checked = True
        Else
            cbImgInversion.Checked = False
        End If

        fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Zoning/DetectInversion", val)
        If val = "1" Then
            cbZonesInversion.Checked = True
        Else
            cbZonesInversion.Checked = False
        End If

        fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/SkewAngle", val)
        If val = "360" Then
            cbDeskew.Checked = True
        Else
            cbDeskew.Checked = False
        End If

        fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/AutoRotate", val)
        If val = "1" Then
            cbRotation.Checked = True
        Else
            cbRotation.Checked = False
        End If

        fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/NoiseFilter", val)
        If val = "1" Then
            cbImgNoiseFilter.Checked = True
        Else
            cbImgNoiseFilter.Checked = False
        End If

        fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "PixLines/RemoveLines", val)
        If val = "1" Then
            cbRemoveLines.Checked = True
        Else
            cbRemoveLines.Checked = False
        End If

        fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/GrayMode", val)
        If val = "1" Then
            cbGrayMode.Checked = True
        Else
            cbGrayMode.Checked = False
        End If

        fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/FastMode", val)
        If val = "1" Then
            cbFastMode.Checked = True
        Else
            cbFastMode.Checked = False
        End If

        fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Binarizer/BinTwice", val)
        If val = "1" Then
            cbBinTwice.Checked = True
        Else
            cbBinTwice.Checked = False
        End If

        fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "WordAlizer/CorrectMixed", val)
        If val = "1" Then
            cbCorrectMixed.Checked = True
        Else
            cbCorrectMixed.Checked = False
        End If

        fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Dictionaries/UseDictionary", val)
        If val = "1" Then
            cbDictionary.Checked = True
        Else
            cbDictionary.Checked = False
        End If

        fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Zoning/OneColumn", val)
        If val = "1" Then
            cbOneColumn.Checked = True
        Else
            cbOneColumn.Checked = False
        End If

        fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/EnabledChars", val)
        edEnabledChars.Text = val

        fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/DisabledChars", val)
        edDisabledChars.Text = val

        fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Binarizer/SimpleThr", val)
        edBinThreshold.Text = val

        fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "WordAlizer/TextQual", val)
        edTextQual.Text = val

        fmMain.NsOCR.Cfg_GetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/PdfDPI", val)
        edPDFDPI.Text = val

    End Sub

    Private Sub bkOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bkOK.Click
        Dim val As String

        If cbFindBarcodes.Checked Then
            val = "1"
        Else
            val = "0"
        End If
        fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Zoning/FindBarcodes", val)

        If cbImgInversion.Checked Then
            val = "2"
        Else
            val = "0"
        End If
        fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/Inversion", val)

        If cbZonesInversion.Checked Then
            val = "1"
        Else
            val = "0"
        End If
        fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Zoning/DetectInversion", val)

        If cbDeskew.Checked Then
            val = "360"
        Else
            val = "0"
        End If
        fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/SkewAngle", val)

        If cbRotation.Checked Then
            val = "1"
        Else
            val = "0"
        End If
        fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/AutoRotate", val)

        If cbImgNoiseFilter.Checked Then
            val = "1"
        Else
            val = "0"
        End If
        fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "ImgAlizer/NoiseFilter", val)

        If cbRemoveLines.Checked Then
            val = "1"
        Else
            val = "0"
        End If
        fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "PixLines/RemoveLines", val)
        fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "PixLines/FindHorLines", val)
        fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "PixLines/FindVerLines", val)

        If cbGrayMode.Checked Then
            val = "1"
        Else
            val = "0"
        End If
        fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/GrayMode", val)

        If cbFastMode.Checked Then
            val = "1"
        Else
            val = "0"
        End If
        fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/FastMode", val)

        If cbBinTwice.Checked Then
            val = "1"
        Else
            val = "0"
        End If
        fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Binarizer/BinTwice", val)

        If cbCorrectMixed.Checked Then
            val = "1"
        Else
            val = "0"
        End If
        fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "WordAlizer/CorrectMixed", val)

        If cbDictionary.Checked Then
            val = "1"
        Else
            val = "0"
        End If
        fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Dictionaries/UseDictionary", val)

        If cbOneColumn.Checked Then
            val = "1"
        Else
            val = "0"
        End If
        fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Zoning/OneColumn", val)

        fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/EnabledChars", edEnabledChars.Text)

        fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/DisabledChars", edDisabledChars.Text)

        fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Binarizer/SimpleThr", edBinThreshold.Text)

        fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "WordAlizer/TextQual", edTextQual.Text)

        fmMain.NsOCR.Cfg_SetOption(fmMain.CfgObj, TNSOCR.BT_DEFAULT, "Main/PdfDPI", edPDFDPI.Text)

        Close()

    End Sub


End Class