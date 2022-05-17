//---------------------------------------------------------------------------

#include <vcl.h>
#pragma hdrstop

#include "fmMainU.h"
//---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma resource "*.dfm"
TfmMain *fmMain;

#include "fmScanU.h"
#include "fmWaitU.h"
#include "fmLanguagesU.h"

//---------------------------------------------------------------------------
__fastcall TfmMain::TfmMain(TComponent* Owner)
	: TForm(Owner)
{
}

bool IsDelimiter(wchar_t ch)
{
	return (ch == L'\\') || (ch == L':');
}

int LastDelimiter(wchar_t* str)
{
	int res = (int)wcslen(str) - 1;
	while (res >= 0)
	{
		if ((str[res] == L'\\') || (str[res] == L':')) return res;
		res--;
	}
	return 0;
}

void ExtractFileDir(wchar_t* FileName, wchar_t* OutStr)
{
	int i;
	i = LastDelimiter(FileName);
	if ((i > 0) && (FileName[i] == L'\\') && (!IsDelimiter(FileName[i - 1]))) i--;
	if (i > 0) memcpy(OutStr, FileName, (i + 1) * sizeof(wchar_t));
	OutStr[i + 1] = 0;
}

//---------------------------------------------------------------------------
void __fastcall TfmMain::FormCreate(TObject *Sender)
{
	bmp = new Graphics::TBitmap();

//load NSOCR.dll dynamically. We don't know exact path so check "bin" in all parents folders

	wchar_t fn[300], dir[300], ExeName[300];
	GetModuleFileNameW(NULL, ExeName, 300); //get .exe name of this application

	NsOCR = new TNSOCR(L"NSOCR.dll"); //try default folders
	if (!NsOCR->IsDllLoaded()) //failed, check "bin" folder in all parent folders
	{
		ExtractFileDir(ExeName, dir); //get folder path
		while (1)
		{
			wcscpy_s(fn, 300, dir);
			if (dir[wcslen(dir) - 1] != L'\\') wcscat_s(fn, 300, L"\\");

			#ifdef _X64_ //see project options, "Preprocessor definitions"
				wcscat_s(fn, 300, L"bin_64\\NSOCR.dll");;
			#else
				wcscat_s(fn, 300, L"bin\\NSOCR.dll");
			#endif

			if (NsOCR->TryLoadDLL(fn)) break;

			if ((wcslen(dir) < 3) || (dir[wcslen(dir) - 2] == L':')) break;
			ExtractFileDir(dir, fn);
			wcscpy_s(dir, 300, fn);
		}

		if (!NsOCR->IsDllLoaded())
		{
			ShowMessage(L"Cannot locate NSOCR.dll!");
			TerminateProcess(GetCurrentProcess(), 0);
		}
	}

///////////////
	UNICODECHAR val[256], title[256];

	//get NSOCR version
	NsOCR->Engine_GetVersion(val);
	GetWindowTextW(this->Handle, title, 256);
	wcscat(title, L" [ NSOCR version: ");
	wcscat(title, val);
	wcscat(title, L" ] ");
	SetWindowTextW(this->Handle, title);

	NsOCR->Engine_SetLicenseKey(L""); //required for licensed version only

	NsOCR->Engine_Initialize(); //initialize engine. Called only once
	//NsOCR.Engine_SetDataDirectory() - not necessary in most cases, default is the folder with NSOCR.dll

	NsOCR->Cfg_Create(&CfgObj); //create Configure object
	NsOCR->Cfg_LoadOptions(CfgObj, L"Config.dat"); //load options, if path not specified, current folder and folder with NSOCR.dll will be checked

	NsOCR->Ocr_Create(CfgObj, &OcrObj); //create OCR object, it will use CfgObj for settings
	NsOCR->Img_Create(OcrObj, &ImgObj); //create Img object that can be OCRed by OcrObj object
	NsOCR->Scan_Create(CfgObj, &ScanObj); //create SCAN object

    //copy some settings to GUI
	if (NsOCR->Cfg_GetOption(CfgObj, BT_DEFAULT, L"ImgAlizer/AutoScale", val, 256) < ERROR_FIRST)
		cbScale->Enabled = !wcscmp(val, L"1");

//////////////////
	Dwn = false;

	bkOCR->Enabled = false;
   	bkSave->Enabled = false;
	cbScale->ItemIndex = 0;

}

//---------------------------------------------------------------------------

 void __fastcall TfmMain::FormClose(TObject *Sender, TCloseAction &Action)
{
	if (NsOCR)
	{
		if (ImgObj) NsOCR->Img_Destroy(ImgObj);
		if (OcrObj) NsOCR->Ocr_Destroy(OcrObj);
		if (CfgObj) NsOCR->Cfg_Destroy(CfgObj);
		if (ScanObj) NsOCR->Scan_Destroy(ScanObj);
		NsOCR->Engine_Uninitialize();
		delete NsOCR;
	}
}
//---------------------------------------------------------------------------

void  TfmMain::AdjustDocScale()
{
	if (!IsImgLoaded()) return;

	bmp->SetSize(0, 0);
	bmp->SetSize(pbImg->Width, pbImg->Height);

	int w = bmp->Width;
	int h = bmp->Height;
	if (cbBin->Checked)
	  NsOCR->Img_DrawToDC(ImgObj, (int)bmp->Canvas->Handle, 0, 0, &w, &h, DRAW_BINARY);
	else
	  NsOCR->Img_DrawToDC(ImgObj, (int)bmp->Canvas->Handle, 0, 0, &w, &h, DRAW_NORMAL);

	ShowImage();
}

bool  TfmMain::IsImgLoaded()
{
	int w, h;
	if (NsOCR->Img_GetSize(ImgObj, &w, &h) > ERROR_FIRST) return false;
	else return (w > 0) && (h > 0);
}

float  TfmMain::GetCurDocScale()
{
	if (!IsImgLoaded()) return 1.0f;
	TRect r;
	r = pbImg->BoundsRect;
	int w = r.right - r.left + 1;
	int h = r.bottom - r.top + 1;

	int ww, hh;
	NsOCR->Img_GetSize(ImgObj, &ww, &hh);
	float kX = (float)w / ww;
	float kY = (float)h / hh;
	float k;
	if (kX > kY) k = kY;
	else k = kX;

	return k;
}

void  TfmMain::ShowImage()
{
	if (!IsImgLoaded()) return;

	Graphics::TBitmap* bmp2 = new Graphics::TBitmap();
	bmp2->SetSize(bmp->Width, bmp->Height);
	bmp2->Canvas->Draw(0, 0, bmp);

	float k = GetCurDocScale();
	RECT r;
	int i, BlkObj, Xpos, Ypos, Width, Height;
	TColor col;
	int cnt = NsOCR->Img_GetBlockCnt(ImgObj);

	for (i = 0; i < cnt; i++)
    {
        NsOCR->Img_GetBlock(ImgObj, i, &BlkObj);
        NsOCR->Blk_GetRect(BlkObj, &Xpos, &Ypos, &Width, &Height);

		r.left = (int)(k * Xpos);
		r.top = (int)(k * Ypos);
		r.right = (int)(k * (Xpos + Width - 1));
		r.bottom = (int)(k * (Ypos + Height - 1));

        switch (NsOCR->Blk_GetType(BlkObj))
        {
		case BT_OCRTEXT: col = clGreen; break;
		case BT_OCRDIGIT: col = clLime; break;
		case BT_ICRDIGIT: col = clBlue; break;
		case BT_BARCODE: col = clNavy; break;
		case BT_PICTURE: col = clAqua; break;
		case BT_CLEAR: col = clGray; break;
		case BT_ZONING: col = clBlack; break;
		case BT_TABLE: col = clOlive; break;
		case BT_MRZ: col = clBlack; break;
		}

		bmp2->Canvas->Brush->Color = col;
		bmp2->Canvas->FrameRect(r);
		bmp2->Canvas->TextOut(r.left + 1, r.top + 1, IntToStr(i));
    }

	//user is creating new block, draw its frame
	if (Dwn)
	{
		r.left = (int)(k * Frame.left);
		r.top = (int)(k * Frame.top);
		r.right = (int)(k * Frame.right);
		r.bottom = (int)(k * Frame.bottom);

		int	w = bmp->Width;
		int h = bmp->Height;

		if (r.right > w - 1) r.right = w - 1;
		if (r.bottom > h - 1) r.bottom = h - 1;

		bmp2->Canvas->Brush->Color = clRed;
		bmp2->Canvas->FrameRect(r);
	}

	pbImg->Canvas->Draw(0, 0, bmp2);
	delete bmp2;
}

void __fastcall TfmMain::pbImgPaint(TObject *Sender)
{
	ShowImage();
}

void  TfmMain::ShowError(UNICODECHAR* ProcName, int ErrorCode)
{
	UnicodeString s;
	s = ProcName;
	s = s + " Error #" + IntToHex(ErrorCode, 8);
	ShowMessage(s);
}

void OpenURL(UNICODECHAR* url)
{
	UnicodeString args = L"url.dll,FileProtocolHandler ";
	args = args + url;
	ShellExecuteW(NULL, L"open", L"rundll32.exe", args.w_str(), NULL, SW_SHOWNORMAL);
}

void __fastcall TfmMain::bkOpenClick(TObject *Sender)
{
	if (!opFile->Execute()) return;
////////
	if (cbScale->Enabled)
	{
		if (cbScale->ItemIndex < 1) //autoscaling
		{
			NsOCR->Cfg_SetOption(CfgObj, BT_DEFAULT, L"ImgAlizer/AutoScale", L"1");
			NsOCR->Cfg_SetOption(CfgObj, BT_DEFAULT, L"ImgAlizer/ScaleFactor", L"1.0"); //default scale if cannot detect it automatically
		}
		else //fixed scaling
		{
			NsOCR->Cfg_SetOption(CfgObj, BT_DEFAULT, L"ImgAlizer/AutoScale", L"0");
			NsOCR->Cfg_SetOption(CfgObj, BT_DEFAULT, L"ImgAlizer/ScaleFactor", cbScale->Items->Strings[cbScale->ItemIndex].w_str());
		}
    }
////
	//note: there are other possible ways to load image to NSOCR
	int res = NsOCR->Img_LoadFile(ImgObj, opFile->FileName.w_str());

	if (res > ERROR_FIRST)
    {
        if (res == ERROR_CANNOTLOADGS) //cannot load GhostScript to support PDF
        {
			UNICODECHAR s[] = L"GhostSript library is needed to support PDF files. Just download and install it with default settings. Do you want to download GhostScript now?";
			if (MessageBoxExW(0, s, L"Question", MB_YESNO, 0) != IDYES) return;
			OpenURL(L"http://www.nicomsoft.com/files/ocr/ghostscript.htm");
		}
        else ShowError(L"Img_LoadFile", res);
        return;
	}

	DoImageLoaded();
}

void TfmMain::DoImageLoaded()
{
	int res;

	//check if image has multiple page and ask user if he wants process and save all pages automatically
	res = NsOCR->Img_GetPageCount(ImgObj);
	if (res > ERROR_FIRST)
	{
		ShowError(L"Img_GetPageCount", res);
		return;
	}
	if (res > 1)
		if (MessageDlg("Image contains multiple pages (" + IntToStr(res) + "). Do you want to process and save all pages automatically?", mtConfirmation, TMsgDlgButtons() << mbYes << mbCancel, 0) == mrYes)
		{
		   	ProcessEntireDocument();
			bkOCR->Enabled = false;
			pbImg->Repaint();
			return;
		}

	//now apply image scaling, binarize image, deskew etc,
	//everything except OCR itself
	res = NsOCR->Img_OCR(ImgObj, OCRSTEP_FIRST, OCRSTEP_ZONING - 1, OCRFLAG_NONE);
	if (res > ERROR_FIRST) ShowError(L"Img_OCR", res);

	edPage->Text = L"1";
	int cnt = NsOCR->Img_GetPageCount(ImgObj);
	lbPages->Caption = L"of " + IntToStr(cnt);
	bkSave->Enabled = false;

	AdjustDocScale();

	bkOCR->Enabled = true;
	mmText->Text = L"";

	bkLoadBlocks->Enabled = true;
	bkSaveBlocks->Enabled = true;
	bkClearBlocks->Enabled = true;
	bkDetectBlocks->Enabled = true;
}

//---------------------------------------------------------------------------

void __fastcall TfmMain::pnImgResize(TObject *Sender)
{
	AdjustDocScale();
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::cbScaleClick(TObject *Sender)
{
	bkOpen->Click();
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::bkSetPageClick(TObject *Sender)
{
	if (!IsImgLoaded()) return;
	int cnt = NsOCR->Img_GetPageCount(ImgObj);
	UNICODECHAR str[100];
	int n = StrToIntDef(edPage->Text, 0) - 1;
	if (n < 0) n = 0;
	if (n >= cnt) n = cnt -1;
	NsOCR->Img_SetPage(ImgObj, n);
	edPage->Text = IntToStr(n + 1);
	bkSave->Enabled = false;

	//now apply image scaling, binarize image, deskew etc,
	//everything except OCR itself
	int res = NsOCR->Img_OCR(ImgObj, OCRSTEP_FIRST, OCRSTEP_ZONING - 1, OCRFLAG_NONE);
	if (res > ERROR_FIRST) ShowError(L"Img_OCR", res);

	AdjustDocScale();
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::bkOCRClick(TObject *Sender)
{
	if (!IsImgLoaded())
	{
		ShowMessage(L"Image no loaded!");
		return;
	}

	bkOCR->Enabled = false;
	lbWait->Visible = true;
	bkSave->Enabled = false;
	this->Repaint();

	int res;
	bool InSameThread;

	InSameThread = false; //perform OCR in non-blocking mode
	//InSameThread = true; //uncomment to perform OCR from this thread (GUI will be freezed)

	// perform OCR. Do zoning step for "zone" type blocks that user could add
	if (InSameThread)
		res = NsOCR->Img_OCR(ImgObj, OCRSTEP_ZONING, OCRSTEP_LAST,  OCRFLAG_NONE);
	else
	{
      //do it in non-blocking mode and then wait for result
	  res = NsOCR->Img_OCR(ImgObj, OCRSTEP_ZONING, OCRSTEP_LAST, OCRFLAG_THREAD);
	  if (res > ERROR_FIRST)
	  {
          ShowError(L"Ocr_OcrImg(1)", res);
          exit;
	  }
	  fmWait->mode = 0;
	  fmWait->ShowModal();
	  res = fmWait->res;
	}

	if (res > ERROR_FIRST)
	{
		if (res == ERROR_OPERATIONCANCELLED)
			ShowMessage(L"Operation was cancelled.");
		else
		{
			ShowError(L"Img_OCR", res);
			return;
		}
	}

	lbWait->Visible = false;
	bkOCR->Enabled = true;
	bkSave->Enabled = true;

	cbBinClick(NULL); //repaint img (binarized image could change)
////
	ShowText();
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::cbExactCopyClick(TObject *Sender)
{
	ShowText();
}
//---------------------------------------------------------------------------

void TfmMain::ShowText()
{
	int flags;
	flags = cbExactCopy->Checked ? FMT_EXACTCOPY : FMT_EDITCOPY;
	int n = NsOCR->Img_GetImgText(ImgObj, NULL, 0, flags) + 1; //get buffer size plus terminating NULL character
  // allocate unicode buffer text length plus null-terminated zero
	UNICODECHAR* txt = (UNICODECHAR*) malloc(2 * n);
	NsOCR->Img_GetImgText(ImgObj, txt, n, flags); ////MaxLen must include terminating NULL character
	mmText->Text = txt;
	free(txt);
}
void __fastcall TfmMain::pbImgMouseUp(TObject *Sender, TMouseButton Button, TShiftState Shift,
          int X, int Y)
{
    int BlkObj, w, h, res;
    if (!Dwn) return;
    Dwn = false;

    w = Frame.right - Frame.left + 1;
    h = Frame.bottom - Frame.top + 1;
    if ((w < 8) || (h < 8))
    {
        ShowImage();
        return;
    }
	res = NsOCR->Img_AddBlock(ImgObj, Frame.left, Frame.top, w, h, &BlkObj);
    if (res > ERROR_FIRST)
	{
		ShowError(L"Img_AddBlock", res);
		return;
	}

	//detect text block inversion
	NsOCR->Blk_Inversion(BlkObj, BLK_INVERSE_DETECT);

	//detect text block rotation
	NsOCR->Blk_Rotation(BlkObj, BLK_ROTATE_DETECT);

	ShowImage();
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::pbImgMouseDown(TObject *Sender, TMouseButton Button, TShiftState Shift, int X, int Y)
{
	if (!IsImgLoaded()) return;

	if (Button == mbRight)
	{
        float kk = GetCurDocScale();
        int cnt = NsOCR->Img_GetBlockCnt(ImgObj);
        int n = -1;
        int minsize = -1;
        int i, BlkObj, Xpos, Ypos, Width, Height, sz;
		RECT r;
		POINT pt;
		pt.x = X;
		pt.y = Y;
        for (i = 0; i < cnt; i++)
        {
            NsOCR->Img_GetBlock(ImgObj, i, &BlkObj);
            NsOCR->Blk_GetRect(BlkObj, &Xpos, &Ypos, &Width, &Height);
            r.left = (int)(kk * Xpos);
            r.top = (int)(kk * Ypos);
            r.right = (int)(kk * (Xpos + Width - 1));
            r.bottom = (int)(kk * (Ypos + Height - 1));

            if (PtInRect(&r, pt))
            {
                //need to find smallest block because blocks may overlap
                if (Width < Height) sz = Width;
                else sz = Height;

                if ((minsize == -1) || (sz < minsize))
                {
                    minsize = sz;
                    n = i;
                }
            }
        }

        if (n == - 1) return; //block not found
        pmBlock->Tag = n; //remember block index
        GetCursorPos(&pt);

        int BlockType, rot, mir, inv;

        NsOCR->Img_GetBlock(ImgObj, pmBlock->Tag, &BlkObj);
		BlockType = NsOCR->Blk_GetType(BlkObj);

		nnTypeOCRText->Checked = (BlockType == BT_OCRTEXT);
		nnTypeOCRDigit->Checked = (BlockType == BT_OCRDIGIT);
		nnTypeICRDigit->Checked = (BlockType == BT_ICRDIGIT);
		nnTypeBarCode->Checked = (BlockType == BT_BARCODE);
		nnTypePicture->Checked = (BlockType == BT_PICTURE);
		nnTypeClear->Checked = (BlockType == BT_CLEAR);
		nnTypeZoning->Checked = (BlockType == BT_ZONING);
		nnTypeTable->Checked = (BlockType == BT_TABLE);
		nnTypeMRZ->Checked = (BlockType == BT_MRZ);

		inv = NsOCR->Blk_Inversion(BlkObj, BLK_INVERSE_GET);
		nnTypeClear->Checked = (inv != 0);

		rot = NsOCR->Blk_Rotation(BlkObj, BLK_ROTATE_GET);
		nnRotate90->Checked = (rot == BLK_ROTATE_270);
		nnRotate180->Checked = (rot == BLK_ROTATE_180);
		nnRotate270->Checked = (rot == BLK_ROTATE_90);

		mir = NsOCR->Blk_Mirror(BlkObj, BLK_MIRROR_GET);
		nnMirrorH->Checked = (mir & BLK_MIRROR_H != 0);
		nnMirrorV->Checked = (mir & BLK_MIRROR_V != 0);

		pt = pbImg->ClientToScreen(Point(X, Y));
		pmBlock->Popup(pt.x, pt.y);
		return;
	}

	int w, h;
	NsOCR->Img_GetSize(ImgObj, &w, &h);

	Dwn = true;
	float k = GetCurDocScale();
	Frame.left = (int)(1 / k * X);
	if (Frame.left < 0) Frame.left = 0;
	if (Frame.left > w) Frame.left = w;
	Frame.top = (int)(1 / k * Y);
	if (Frame.top < 0) Frame.top = 0;
	if (Frame.top > h) Frame.top = h;

	Frame.right = Frame.left;
	Frame.bottom = Frame.top;

	ShowImage();

}
//---------------------------------------------------------------------------

void __fastcall TfmMain::pbImgMouseMove(TObject *Sender, TShiftState Shift, int X,
          int Y)
{
	if (!Dwn) return;

	if (!IsImgLoaded()) return;

	int w, h;
	NsOCR->Img_GetSize(ImgObj, &w, &h);

	float k = GetCurDocScale();
	Frame.right = (int)(1 / k * X);
	if (Frame.right < 0) Frame.right = 0;
	if (Frame.right > w) Frame.right = w;
	Frame.bottom = (int)(1 / k * Y);
	if (Frame.bottom < 0) Frame.bottom = 0;
	if (Frame.bottom > h) Frame.bottom = h;

	ShowImage();
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::cbBinClick(TObject *Sender)
{
	AdjustDocScale();
}
//---------------------------------------------------------------------------


void __fastcall TfmMain::nnTypeOCRTextClick(TObject *Sender)
{
	int BlkObj;
	NsOCR->Img_GetBlock(ImgObj, pmBlock->Tag, &BlkObj);
	NsOCR->Blk_SetType(BlkObj, BT_OCRTEXT);
	ShowImage();
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::nnTypeICRDigitClick(TObject *Sender)
{
	int BlkObj;
	NsOCR->Img_GetBlock(ImgObj, pmBlock->Tag, &BlkObj);
	NsOCR->Blk_SetType(BlkObj, BT_ICRDIGIT);
	ShowImage();
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::nnTypePictureClick(TObject *Sender)
{
	int BlkObj;
	NsOCR->Img_GetBlock(ImgObj, pmBlock->Tag, &BlkObj);
	NsOCR->Blk_SetType(BlkObj, BT_PICTURE);
	ShowImage();
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::nnTypeClearClick(TObject *Sender)
{
	int BlkObj;
	NsOCR->Img_GetBlock(ImgObj, pmBlock->Tag, &BlkObj);
	NsOCR->Blk_SetType(BlkObj, BT_CLEAR);
	ShowImage();
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::nnInvertClick(TObject *Sender)
{
	int BlkObj;
	nnInvert->Checked = !nnInvert->Checked;
	NsOCR->Img_GetBlock(ImgObj, pmBlock->Tag, &BlkObj);
	NsOCR->Blk_Inversion(BlkObj, nnInvert->Checked ? BLK_INVERSE_SET1 : BLK_INVERSE_SET0);
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::nnRotate90Click(TObject *Sender)
{
	int BlkObj;
	nnRotate90->Checked = !nnRotate90->Checked;
	NsOCR->Img_GetBlock(ImgObj, pmBlock->Tag, &BlkObj);
	if (!nnRotate90->Checked) NsOCR->Blk_Rotation(BlkObj, BLK_ROTATE_NONE);
	else NsOCR->Blk_Rotation(BlkObj, BLK_ROTATE_270);
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::nnRotate180Click(TObject *Sender)
{
	int BlkObj;
	nnRotate180->Checked = !nnRotate180->Checked;
	NsOCR->Img_GetBlock(ImgObj, pmBlock->Tag, &BlkObj);
	if (!nnRotate180->Checked) NsOCR->Blk_Rotation(BlkObj, BLK_ROTATE_NONE);
	else NsOCR->Blk_Rotation(BlkObj, BLK_ROTATE_180);
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::nnRotate270Click(TObject *Sender)
{
	int BlkObj;
	nnRotate270->Checked = !nnRotate270->Checked;
	NsOCR->Img_GetBlock(ImgObj, pmBlock->Tag, &BlkObj);
	if (!nnRotate270->Checked) NsOCR->Blk_Rotation(BlkObj, BLK_ROTATE_NONE);
	else NsOCR->Blk_Rotation(BlkObj, BLK_ROTATE_90);
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::nnMirrorHClick(TObject *Sender)
{
	int BlkObj, m;
	nnMirrorH->Checked = !nnMirrorH->Checked;
	NsOCR->Img_GetBlock(ImgObj, pmBlock->Tag, &BlkObj);
	m = NsOCR->Blk_Mirror(BlkObj, -1);
	if (nnMirrorH->Checked) m |= BLK_MIRROR_H;
	else m &= ~BLK_MIRROR_H;
	NsOCR->Blk_Mirror(BlkObj, m);
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::nnMirrorVClick(TObject *Sender)
{
	int BlkObj, m;
	nnMirrorV->Checked = !nnMirrorV->Checked;
	NsOCR->Img_GetBlock(ImgObj, pmBlock->Tag, &BlkObj);
	m = NsOCR->Blk_Mirror(BlkObj, -1);
	if (nnMirrorV->Checked) m |= BLK_MIRROR_V;
	else m &= ~BLK_MIRROR_V;
	NsOCR->Blk_Mirror(BlkObj, m);
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::nnDeleteBlockClick(TObject *Sender)
{
	int BlkObj;
	NsOCR->Img_GetBlock(ImgObj, pmBlock->Tag, &BlkObj);
	NsOCR->Img_DeleteBlock(ImgObj, BlkObj);
	ShowImage();
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::bkLoadBlocksClick(TObject *Sender)
{
  if (!opBlocks->Execute()) return;

  NsOCR->Img_DeleteAllBlocks(ImgObj); //note: Img_LoadBlocks does not remove existing blocks, so remove them here
  bkSave->Enabled = false;
  int res = NsOCR->Img_LoadBlocks(ImgObj, opBlocks->FileName.w_str());
  if (res > ERROR_FIRST)
  {
      ShowError(L"Img_LoadBlocks", res);
	  return;
  }
  ShowImage();
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::bkSaveBlocksClick(TObject *Sender)
{
  if (!svBlocks->Execute()) return;

  int res = NsOCR->Img_SaveBlocks(ImgObj, svBlocks->FileName.w_str());
  if (res > ERROR_FIRST) ShowError(L"Img_SaveBlocks", res);
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::bkClearBlocksClick(TObject *Sender)
{
  NsOCR->Img_DeleteAllBlocks(ImgObj);
  bkSave->Enabled = false;
  ShowImage();
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::bkDetectBlocksClick(TObject *Sender)
{
  NsOCR->Img_DeleteAllBlocks(ImgObj);
  NsOCR->Img_OCR(ImgObj, OCRSTEP_ZONING, OCRSTEP_ZONING, OCRFLAG_NONE);
  ShowImage();
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::bkScanClick(TObject *Sender)
{
	fmScan->ShowModal();
}
//---------------------------------------------------------------------------


void __fastcall TfmMain::nnTypeOCRDigitClick(TObject *Sender)
{
	int BlkObj;
	NsOCR->Img_GetBlock(ImgObj, pmBlock->Tag, &BlkObj);
	NsOCR->Blk_SetType(BlkObj, BT_OCRDIGIT);
	ShowImage();
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::nnTypeZoningClick(TObject *Sender)
{
	int BlkObj;
	NsOCR->Img_GetBlock(ImgObj, pmBlock->Tag, &BlkObj);
	NsOCR->Blk_SetType(BlkObj, BT_ZONING);
	ShowImage();
}
//---------------------------------------------------------------------------

void __fastcall TfmMain::nnTypeBarCodeClick(TObject *Sender)
{
	int BlkObj;
	NsOCR->Img_GetBlock(ImgObj, pmBlock->Tag, &BlkObj);
	NsOCR->Blk_SetType(BlkObj, BT_BARCODE);
	NsOCR->Cfg_SetOption(CfgObj, BT_BARCODE, L"BarCode/SearchMode", L"3"); //deep scanning for manual barcode zone
	ShowImage();
}
//---------------------------------------------------------------------------


void __fastcall TfmMain::nnTypeTableClick(TObject *Sender)
{
 	int BlkObj;
	NsOCR->Img_GetBlock(ImgObj, pmBlock->Tag, &BlkObj);
	NsOCR->Blk_SetType(BlkObj, BT_TABLE);
	ShowImage();
}

//---------------------------------------------------------------------------

void __fastcall TfmMain::nnTypeMRZClick(TObject *Sender)
{
	int BlkObj;
	NsOCR->Img_GetBlock(ImgObj, pmBlock->Tag, &BlkObj);
	NsOCR->Blk_SetType(BlkObj, BT_MRZ);
	ShowImage();
}

//---------------------------------------------------------------------------

UnicodeString GetDocName(UnicodeString s)
{
  s = ExtractFileName(s);
  return ChangeFileExt(s, ".pdf"); //default extension
}

void __fastcall TfmMain::bkSaveClick(TObject *Sender)
{
  SvrObj = 0; //mark variable, it has no object

  svFile->FileName = GetDocName(opFile->FileName);
  svFile->FilterIndex = 1;

  if (!svFile->Execute()) return;

  int format = SVR_FORMAT_PDF + (svFile->FilterIndex - 1);

	//image over text option for PDF
	if ((format == SVR_FORMAT_PDF) || (format == SVR_FORMAT_PDFA))
	{
		if (MessageDlg("Place page image over recognized text?", mtConfirmation, TMsgDlgButtons() << mbYes << mbNo, 0) == mrYes)
			NsOCR->Cfg_SetOption(CfgObj, BT_DEFAULT, L"Saver/PDF/ImageLayer", L"1");
		else
			NsOCR->Cfg_SetOption(CfgObj, BT_DEFAULT, L"Saver/PDF/ImageLayer", L"0");
	}

  int res = NsOCR->Svr_Create(CfgObj, format, &SvrObj);
  if (res > ERROR_FIRST)
  {
	ShowError(L"Svr_Create", res);
	return;
  }

  int flags;
  flags = cbExactCopy->Checked ? FMT_EXACTCOPY : FMT_EDITCOPY;

  NsOCR->Svr_NewDocument(SvrObj);

  if (!Sender) return; //caller is ProcessEntireDocument

  res = NsOCR->Svr_AddPage(SvrObj, ImgObj, flags);
  if (res > ERROR_FIRST)
  {
	ShowError(L"Svr_AddPage", res);
	NsOCR->Svr_Destroy(SvrObj);
	return;
  }

  if ((format == SVR_FORMAT_PDF) || (format == SVR_FORMAT_XML)) //demonstrate how to write PDF info
	NsOCR->Svr_SetDocumentInfo(SvrObj, INFO_PDF_TITLE, L"Sample Title");

  res = NsOCR->Svr_SaveToFile(SvrObj, svFile->FileName.w_str());
  NsOCR->Svr_Destroy(SvrObj);

  if (res > ERROR_FIRST)
  {
	ShowError(L"Svr_SaveToFile", res);
	return;
  }

  //open the file
  ShellExecuteW(0, L"open", svFile->FileName.t_str(), L"", NULL, SW_SHOWNORMAL);
}
//---------------------------------------------------------------------------

void TfmMain::ProcessEntireDocument()
{
  bkSaveClick(NULL);
  if (!SvrObj) return; //saving cancelled

  int OcrCnt, res;
  bool InSameThread;
  //recognize up to 4 images at once.
  //Note for large images ERROR_NOMEMORY can be raised
  //OcrCnt = 4;

  //Use number of logical CPU cores on the system for the best performance
  OcrCnt = 0;

  InSameThread = false; //perform OCR in non-blocking mode
  //InSameThread = true; //uncomment to perform OCR from this thread (GUI will be freezed)

  int flags;
  flags = cbExactCopy->Checked ? FMT_EXACTCOPY : FMT_EDITCOPY;
  flags = flags << 8; //we need to shift FMT_XXXXX flags for this function

  //process all pages of input image and add results to SAVER object
  //this function will create (and then release) additional temporary OCR objects if OcrCnt > 1
  if (InSameThread)
	{
	  res = NsOCR->Ocr_ProcessPages(ImgObj, SvrObj, 0, -1, OcrCnt, OCRFLAG_NONE | flags);
	}
   else
	{
	  //do it in non-blocking mode and then wait for result
	  res = NsOCR->Ocr_ProcessPages(ImgObj, SvrObj, 0, -1, OcrCnt, OCRFLAG_THREAD | flags);
	  if (res > ERROR_FIRST)
	  {
		  ShowError(L"Ocr_ProcessPages(1)", res);
		  return;
	  }
	  fmWait->mode = 1;
	  fmWait->ShowModal();
	  res = fmWait->res;
	}

  //restore option
  NsOCR->Cfg_SetOption(CfgObj, BT_DEFAULT, L"Main/NumKernels", L"0");

  if (res > ERROR_FIRST)
  {
	if (res == ERROR_OPERATIONCANCELLED)
		ShowMessage("Operation was cancelled.");
       else
		ShowError(L"Ocr_ProcessPages", res);
    NsOCR->Svr_Destroy(SvrObj);
	return;
  }

  //save output document
  res = NsOCR->Svr_SaveToFile(SvrObj, svFile->FileName.w_str());
  NsOCR->Svr_Destroy(SvrObj);

  if (res > ERROR_FIRST)
  {
	ShowError(L"Svr_SaveToFile", res);
	return;
  }

  //open the file
  ShellExecuteW(0, L"open", svFile->FileName.t_str(), L"", NULL, SW_SHOWNORMAL );
}

//---------------------------------------------------------------------------

void __fastcall TfmMain::bkLanguagesClick(TObject *Sender)
{
  fmLanguages->ShowModal();
}
//---------------------------------------------------------------------------


