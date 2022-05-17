
// SampleDlg.cpp : implementation file
//

#pragma warning(disable : 4996) //remove "deprecated" warning

#include "stdafx.h"
#include "Sample.h"
#include "SampleDlg.h"
#include "TScanDlg.h"
#include "DlgWait.h"
#include "TLangsDlg.h"
#include "TOptionsDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CSampleDlg dialog

CSampleDlg* gMainDlg;


CSampleDlg::CSampleDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CSampleDlg::IDD, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	NsOCR = NULL;
	CfgObj = 0;
	OcrObj = 0;
	ImgObj = 0;

	gMainDlg = this;
}

CSampleDlg::~CSampleDlg()
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

void CSampleDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_STIMG, m_DocPic);
	DDX_Control(pDX, IDC_EDTEXT, m_edText);
	DDX_Control(pDX, IDC_BKOPEN, m_bkOpen);
	DDX_Control(pDX, IDC_BKOCR, m_bkRecognize);
	DDX_Control(pDX, IDC_STWAIT, m_lbWait);
	DDX_Control(pDX, IDC_EDPAGE, m_edPage);
	DDX_Control(pDX, IDC_STPAGES, m_lbPages);
	DDX_Control(pDX, IDC_CBSCALE, m_cbScale);
	DDX_Control(pDX, IDC_CBBIN, m_cbBin);
	DDX_Control(pDX, IDC_CB_FORMAT, m_cbExactCopy);
	DDX_Control(pDX, IDC_BKLOADBLOCKS, m_bkLoadBlocks);
	DDX_Control(pDX, IDC_BKSAVEBLOCKS, m_bkSaveBlocks);
	DDX_Control(pDX, IDC_BKCLEARBLOCKS, m_bkClearBlocks);
	DDX_Control(pDX, IDC_BKDETECTBLOCKS2, m_bkDetectBlocks);
	DDX_Control(pDX, IDC_BKSAVE, m_bkSave);
}

BEGIN_MESSAGE_MAP(CSampleDlg, CDialog)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	//}}AFX_MSG_MAP
	ON_BN_CLICKED(IDCANCEL, &CSampleDlg::OnBnClickedCancel)
	ON_BN_CLICKED(IDOK, &CSampleDlg::OnBnClickedOk)
	ON_WM_SIZE()
	ON_BN_CLICKED(IDC_BKOPEN, &CSampleDlg::OnBnClickedBkopen)
	ON_BN_CLICKED(IDC_BKOCR, &CSampleDlg::OnBnClickedBkocr)
	ON_WM_LBUTTONUP()
	ON_WM_LBUTTONDOWN()
	ON_WM_MOUSEMOVE()
	ON_BN_CLICKED(IDC_BKSETPAGE, &CSampleDlg::OnBnClickedBksetpage)
	ON_CBN_SELCHANGE(IDC_CBSCALE, &CSampleDlg::OnScaleChanged)
	ON_BN_CLICKED(IDC_CBBIN, &CSampleDlg::cbBinOnClick)
	ON_WM_RBUTTONDOWN()
	ON_BN_CLICKED(IDC_CB_FORMAT, &CSampleDlg::cbExactCopyClicked)
	ON_BN_CLICKED(IDC_BKLOADBLOCKS, &CSampleDlg::OnBnClickedBkloadblocks)
	ON_BN_CLICKED(IDC_BKSAVEBLOCKS, &CSampleDlg::OnBnClickedBksaveblocks)
	ON_BN_CLICKED(IDC_BKCLEARBLOCKS, &CSampleDlg::OnBnClickedBkclearblocks)
	ON_BN_CLICKED(IDC_BKDETECTBLOCKS2, &CSampleDlg::OnBnClickedBkdetectblocks2)
	ON_BN_CLICKED(IDC_BKSAVE, &CSampleDlg::OnBnClickedBksave)
	ON_BN_CLICKED(IDC_BKSCAN, &CSampleDlg::OnBnClickedBkscan)
	ON_BN_CLICKED(IDC_BK_LANGUAGES, &CSampleDlg::OnBnClickedBkLanguages)
	ON_BN_CLICKED(IDC_BKOPTIONS, &CSampleDlg::OnBnClickedBkoptions)
END_MESSAGE_MAP()

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

// CSampleDlg message handlers

BOOL CSampleDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon
	// TODO: Add extra initialization here
//////////////////////////////////////////////////////////////////
//load NSOCR.dll dynamically. We don't know exact path so check "bin" in all parents folders

	wchar_t fn[300], dir[300], ExeName[300];
	GetModuleFileName(NULL, ExeName, 300); //get .exe name of this application

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
			MessageBox(L"Cannot locate NSOCR.dll!");
			TerminateProcess(GetCurrentProcess(), 0);
		}
	}

///////////////
    UNICODECHAR val[256], title[256];

	//get NSOCR version
	NsOCR->Engine_GetVersion(val);
	GetWindowText(title, 256);
	wcscat(title, L" [ NSOCR version: ");
	wcscat(title, val);
	wcscat(title, L" ] ");
	SetWindowText(title);

	NsOCR->Engine_SetLicenseKey(L""); //required for licensed version only

	NsOCR->Engine_Initialize(); //initialize engine. Called only once
	//NsOCR.Engine_SetDataDirectory() - not necessary in most cases, default is the folder with NSOCR.dll

	NsOCR->Cfg_Create(&CfgObj); //create Configure object
	NsOCR->Cfg_LoadOptions(CfgObj, L"Config.dat"); //load options, if path not specified, current folder and folder with NSOCR.dll will be checked

	NsOCR->Ocr_Create(CfgObj, &OcrObj); //create OCR object, it will use CfgObj for settings
	NsOCR->Img_Create(OcrObj, &ImgObj); //create Img object that can be OCRed by OcrObj object
	NsOCR->Scan_Create(CfgObj, &ScanObj); //create SCAN object

//////////////////
	Dwn = false;

	m_bkRecognize.EnableWindow(0);
	m_bkSave.EnableWindow(0);
	AdjustSize();

	IsProcessPagesMode = false;

	m_cbScale.AddString(L"Auto");
	m_cbScale.AddString(L"0.25");
	m_cbScale.AddString(L"0.5");
	m_cbScale.AddString(L"1.0");
	m_cbScale.AddString(L"1.5");
	m_cbScale.AddString(L"2.0");
	m_cbScale.AddString(L"2.5");
	m_cbScale.AddString(L"4.0");
	m_cbScale.SetCurSel(0);

    if (NsOCR->Cfg_GetOption(CfgObj, BT_DEFAULT, L"ImgAlizer/AutoScale", val, 256) < ERROR_FIRST)
    {
		m_cbScale.EnableWindow(!wcscmp(val, L"1"));
    }

	m_bkLoadBlocks.EnableWindow(0);
	m_bkSaveBlocks.EnableWindow(0);
	m_bkClearBlocks.EnableWindow(0);
	m_bkDetectBlocks.EnableWindow(0);

	blockMenu = LoadMenu(GetModuleHandle(NULL), MAKEINTRESOURCE(IDR_BLOCKMENU));
	blockMenu = GetSubMenu(blockMenu, 0);

	return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CSampleDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CSampleDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}


void CSampleDlg::OnBnClickedCancel()
{
	// TODO: Add your control notification handler code here
	OnCancel();
}

void CSampleDlg::OnBnClickedOk()
{
	// TODO: Add your control notification handler code here
	// OnOK();
}

void CSampleDlg::AdjustSize()
{
	RECT r;
	this->GetClientRect(&r);
	int x, y, w, h, clw, clh;
	clw = r.right - r.left;
	clh = r.bottom - r.top;

	x = clw / 2 + 10;
	y = 70;
	w = clw / 2 - 20;
	h = clh - 85;
	m_edText.SetWindowPos(NULL, x, y, w, h, SWP_NOOWNERZORDER | SWP_NOZORDER | SWP_NOACTIVATE);

	x = 8;
	w = clw / 2 - 10;
	h = clh - 95;
	m_DocPic.SetWindowPos(NULL, x, y, w, h, SWP_NOOWNERZORDER | SWP_NOZORDER | SWP_NOACTIVATE);

	y = clh - 22;
	m_cbBin.SetWindowPos(NULL, 8, y, 0, 0, SWP_NOSIZE | SWP_NOOWNERZORDER | SWP_NOZORDER | SWP_NOACTIVATE);

	y = clh - 24;
	m_bkLoadBlocks.SetWindowPos(NULL, 150, y, 0, 0, SWP_NOSIZE | SWP_NOOWNERZORDER | SWP_NOZORDER | SWP_NOACTIVATE);
	m_bkSaveBlocks.SetWindowPos(NULL, 150 + 80, y, 0, 0, SWP_NOSIZE | SWP_NOOWNERZORDER | SWP_NOZORDER | SWP_NOACTIVATE);
	m_bkClearBlocks.SetWindowPos(NULL, 150 + 2 * 80, y, 0, 0, SWP_NOSIZE | SWP_NOOWNERZORDER | SWP_NOZORDER | SWP_NOACTIVATE);
	m_bkDetectBlocks.SetWindowPos(NULL, 150 + 3 * 80, y, 0, 0, SWP_NOSIZE | SWP_NOOWNERZORDER | SWP_NOZORDER | SWP_NOACTIVATE);
}

void CSampleDlg::AdjustDocScale()
{
	if (!IsImgLoaded()) return;

	int w, h;
	RECT r;
	m_DocPic.GetClientRect(&r);
	w = r.right - r.left + 1;
	h = r.bottom - r.top + 1;

	bmp.SetSize(w, h);
	if (m_cbBin.GetCheck()) NsOCR->Img_DrawToDC(ImgObj, (int)bmp.GetDC(), 0, 0, &w, &h, DRAW_BINARY);
	else NsOCR->Img_DrawToDC(ImgObj, (int)bmp.GetDC(), 0, 0, &w, &h, DRAW_NORMAL);

	ShowImage();
}

bool CSampleDlg::IsImgLoaded()
{
	int w, h;
	if (NsOCR->Img_GetSize(ImgObj, &w, &h) > ERROR_FIRST) return false;
	return (w > 0) && (h > 0);
}

float CSampleDlg::GetCurDocScale()
{
	if (!IsImgLoaded()) return 1.0f;
	RECT r;
	m_DocPic.GetClientRect(&r);
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

void CSampleDlg::ShowImage()
{
	if (!IsImgLoaded()) return;

	TNBitmap bmp2;
	bmp2.SetSize(bmp.GetWidth(), bmp.GetHeight());
	bmp2.Draw(0, 0, &bmp);

	float k = GetCurDocScale();
	RECT r;
    int i, BlkObj, Xpos, Ypos, Width, Height;
    int col;
    int cnt = NsOCR->Img_GetBlockCnt(ImgObj);
    for (i = 0; i < cnt; i++)
    {
        NsOCR->Img_GetBlock(ImgObj, i, &BlkObj);
        NsOCR->Blk_GetRect(BlkObj, &Xpos, &Ypos, &Width, &Height);

		r.left = (int)(k * Xpos);
		r.top = (int)(k * Ypos);
		r.right = (int)(k * (Xpos + Width - 1)) +1;
		r.bottom = (int)(k * (Ypos + Height - 1)) +1;

        switch (NsOCR->Blk_GetType(BlkObj))
        {
        case BT_OCRTEXT: col = NclGreen; break;
		case BT_OCRDIGIT: col = NclLime; break;
        case BT_ICRDIGIT: col = NclBlue; break;
        case BT_PICTURE: col = NclAqua; break;
        case BT_CLEAR: col = NclGray; break;
		case BT_ZONING: col = NclBlack; break;
		case BT_BARCODE: col = NclNavy; break;
		case BT_TABLE: col = NclOlive; break;
		case BT_MRZ: col = NclBlack; break;
        }
    
		bmp2.SetBrushColor(col);
		bmp2.FrameRect(r);

		wchar_t s[100];
		swprintf(s, L"%d", i);
		bmp2.TextOutW(r.left + 1, r.top + 1, s);
    }

	//user is creating new block, draw its frame
	if (Dwn)
	{
		r.left = (int)(k * Frame.left);
		r.top = (int)(k * Frame.top);
		r.right = (int)(k * Frame.right);
		r.bottom = (int)(k * Frame.bottom);

		int w = bmp2.GetWidth();
		int h = bmp2.GetHeight();

		if (r.right > w - 1) r.right = w - 1;
		if (r.bottom > h - 1) r.bottom = h - 1;

		bmp2.SetBrushColor(NclRed);
		bmp2.FrameRect(r);
	}

	m_DocPic.AssignBitmap(&bmp2);
}
void CSampleDlg::OnSize(UINT nType, int cx, int cy)
{
	CDialog::OnSize(nType, cx, cy);

	if (!NsOCR) return;
////
	AdjustSize();
	AdjustDocScale();
}

void OpenURL(UNICODECHAR* url)
{
	UNICODECHAR args[256];
	swprintf(args, 256, L"url.dll,FileProtocolHandler %s", url);
	ShellExecute(NULL, L"open", L"rundll32.exe", args, NULL, SW_SHOWNORMAL);
}

void CSampleDlg::OnBnClickedBkopen()
{
	UNICODECHAR fn[512];
	OPENFILENAME ofn; 
	UNICODECHAR szFile[512]; 
	ZeroMemory(&ofn, sizeof(ofn));
	ofn.lStructSize = sizeof(ofn);
	ofn.hwndOwner = this->m_hWnd;
	ofn.lpstrFile = szFile;
	wcscpy_s(ofn.lpstrFile, 512, L"\0\0");
	ofn.nMaxFile = 260;
	ofn.lpstrFilter = L"Image Files (bmp,jpg,tif,png,gif,pdf)\0*.bmp;*.jpg;*.tif;*.tiff;*.png;*.gif;*.pdf\0";
	ofn.nFilterIndex = 1;
	ofn.lpstrFileTitle = NULL;
	ofn.nMaxFileTitle = 0;
	ofn.lpstrInitialDir = NULL;
	ofn.Flags = OFN_EXPLORER | OFN_HIDEREADONLY | OFN_NONETWORKBUTTON | OFN_NOREADONLYRETURN | OFN_PATHMUSTEXIST | OFN_FILEMUSTEXIST;// show open file dialog

	if (!GetOpenFileName(&ofn)) return;
	wcscpy_s(fn, 512, ofn.lpstrFile);
////////
	wchar_t str[100];
    if (m_cbScale.IsWindowEnabled())
    {
        if (m_cbScale.GetCurSel() < 1) //autoscaling
        {
            NsOCR->Cfg_SetOption(CfgObj, BT_DEFAULT, L"ImgAlizer/AutoScale", L"1");
            NsOCR->Cfg_SetOption(CfgObj, BT_DEFAULT, L"ImgAlizer/ScaleFactor", L"1.0"); //default scale if cannot detect it automatically
        }
        else //fixed scaling
        {
            NsOCR->Cfg_SetOption(CfgObj, BT_DEFAULT, L"ImgAlizer/AutoScale", L"0");		
			m_cbScale.GetWindowTextW(str, 100);
            NsOCR->Cfg_SetOption(CfgObj, BT_DEFAULT, L"ImgAlizer/ScaleFactor", str);
        }
    }

////
	//note: there are other possible ways to load image to NSOCR
	int res = NsOCR->Img_LoadFile(ImgObj, fn);

/*  //if you have raw pixel data you should use Img_LoadBmpData:
	HANDLE f = CreateFile(fn, GENERIC_READ, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
	DWORD sz = GetFileSize(f, NULL);
	unsigned char* buf = (unsigned char*) malloc(sz);
	DWORD rd;
	ReadFile(f, buf, sz, &rd, NULL);
	res = NsOCR->Img_LoadBmpData(ImgObj, buf, 497, 284, BMP_8BIT, 497); //you must know image size
*/

    if (res > ERROR_FIRST)
    {
        if (res == ERROR_CANNOTLOADGS) //cannot load GhostScript to support PDF
        {
            UNICODECHAR s[] = L"GhostSript library is needed to support PDF files. Just download and install it with default settings. Do you want to download GhostScript now?";
			if (MessageBoxEx(0, s, L"Question", MB_YESNO, 0) != IDYES) return;
			OpenURL(L"http://www.nicomsoft.com/files/ocr/ghostscript.htm");
        }
        else ShowError(L"Img_LoadFile", res);
        return;
    }

	DoImageLoaded();
}

void CSampleDlg::DoImageLoaded()
{
	//clear bitmaps for old image
	bmp.SetSize(0, 0);

	int res;
	wchar_t str[500];

	//check if image has multiple page and ask user if he wants process and save all pages automatically
	res = NsOCR->Img_GetPageCount(ImgObj);
	if (res > ERROR_FIRST)
	{
		ShowError(L"Img_GetPageCount", res);
		return;
	}
	if (res > 1)
	{
		swprintf(str, L"Image contains multiple pages (%d). Do you want to process and save all pages automatically?", res);
		if (MessageBox(str, L"Question", MB_YESNO) == IDYES)
		{
		   	ProcessEntireDocument();
			m_bkRecognize.EnableWindow(0);
			return;
		}
	}

	//now apply image scaling, binarize image, deskew etc,
	//everything except OCR itself
	res = NsOCR->Img_OCR(ImgObj, OCRSTEP_FIRST, OCRSTEP_ZONING - 1, OCRFLAG_NONE);
	if (res > ERROR_FIRST) ShowError(L"Img_OCR", res);

	m_edPage.SetWindowTextW(L"1");
	int cnt = NsOCR->Img_GetPageCount(ImgObj);
	swprintf(str, L"of %d", cnt);
	m_lbPages.SetWindowTextW(str);
	m_bkSave.EnableWindow(0);

	AdjustDocScale();

	m_bkRecognize.EnableWindow(1);
	m_edText.SetWindowTextW(L"");

	m_bkLoadBlocks.EnableWindow(1);
	m_bkSaveBlocks.EnableWindow(1);
	m_bkClearBlocks.EnableWindow(1);
	m_bkDetectBlocks.EnableWindow(1);
}

void CSampleDlg::OnBnClickedBksetpage()
{
	if (!IsImgLoaded()) return;
	int cnt = NsOCR->Img_GetPageCount(ImgObj);
	UNICODECHAR str[100];
	m_edPage.GetWindowTextW(str, 100);
	int n = _wtoi(str) - 1;
	if (n < 0) n = 0;
	if (n >= cnt) n = cnt -1;
	NsOCR->Img_SetPage(ImgObj, n);
	_itow_s(n + 1, str, 100, 10);  //_itow(n + 1, str, 10);
	m_edPage.SetWindowTextW(str);
	m_bkSave.EnableWindow(0);

	//now apply image scaling, binarize image, deskew etc,
	//everything except OCR itself
	int res = NsOCR->Img_OCR(ImgObj, OCRSTEP_FIRST, OCRSTEP_ZONING - 1, OCRFLAG_NONE);
	if (res > ERROR_FIRST) ShowError(L"Img_OCR", res);

	AdjustDocScale();
}

void CSampleDlg::ShowError(UNICODECHAR* ProcName, int ErrorCode)
{
    wchar_t s[256];
	wsprintf(s, L"%s Error #%X", ProcName, ErrorCode);
    MessageBox(s);
}

void CSampleDlg::OnBnClickedBkocr()
{
	if (!IsImgLoaded())
	{
		MessageBox(L"Image not loaded!");
		return;
	}

////
	m_bkRecognize.EnableWindow(0);
	m_lbWait.ShowWindow(SW_SHOW);
	m_bkSave.EnableWindow(0);
	this->RedrawWindow();

	//perform OCR itself
	int res;
	bool InSameThread;
	
	InSameThread = false; //perform OCR in non-blocking mode
	//InSameThread = true; //uncomment to perform OCR from this thread (GUI will be freezed)

	if (InSameThread)
	{
		res = NsOCR->Img_OCR(ImgObj, OCRSTEP_ZONING, OCRSTEP_LAST, OCRFLAG_NONE);
	}
	else
	{
      //do it in non-blocking mode and then wait for result
	  res = NsOCR->Img_OCR(ImgObj, OCRSTEP_ZONING, OCRSTEP_LAST, OCRFLAG_THREAD);
	  if (res > ERROR_FIRST)
	  {
          ShowError(L"Ocr_OcrImg(1)", res);
          return;
      }
	  CDlgWait DlgWait;
	  DlgWait.mode = 0;
	  DlgWait.DoModal();
	  res = DlgWait.res;
	}

	if (res > ERROR_FIRST)
	{
		if (res == ERROR_OPERATIONCANCELLED)
			MessageBox(L"Operation was cancelled.");
		else
		{
			ShowError(L"Img_OCR", res);
			return;
		}
	}

	m_lbWait.ShowWindow(SW_HIDE); 
	m_bkRecognize.EnableWindow(1);
	m_bkSave.EnableWindow(1);
////	
	cbBinOnClick(); //repaint img (binarized image could change)
	
	ShowText();
}

void CSampleDlg::ShowText()
{
	int flags = m_cbExactCopy.GetCheck() ? FMT_EXACTCOPY : FMT_EDITCOPY;
	int n = NsOCR->Img_GetImgText(ImgObj, NULL, 0, flags) + 1; //get buffer size plus terminating NULL character
  // allocate unicode buffer text length plus null-terminated zero
	UNICODECHAR* txt = (UNICODECHAR*) malloc(2 * n);
	NsOCR->Img_GetImgText(ImgObj, txt, n, flags); //MaxLen must include terminating NULL character
	m_edText.SetWindowTextW(txt);
	free(txt);
}

void CSampleDlg::OnLButtonUp(UINT nFlags, CPoint point)
{
	CDialog::OnLButtonUp(nFlags, point);
////
    int BlkObj, w, h, res;
    if (!Dwn) return;
    Dwn = false;

    if (!IsImgLoaded()) return;

    NsOCR->Img_GetSize(ImgObj, &w, &h);
    if (Frame.right >= w) Frame.right = w - 1;
    if (Frame.bottom >= h) Frame.bottom = h - 1;

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

void CSampleDlg::OnLButtonDown(UINT nFlags, CPoint point)
{
	CDialog::OnLButtonDown(nFlags, point);

	if (!IsImgLoaded()) return;

	RECT r;
	m_DocPic.GetWindowRect(&r);
	this->ScreenToClient(&r);

	if (PtInRect(&r, point))
	{
		int w, h;
		NsOCR->Img_GetSize(ImgObj, &w, &h);

		Dwn = true;
		float k = GetCurDocScale();
		Frame.left = (int)(1 / k * (point.x - r.left));
		if (Frame.left < 0) Frame.left = 0;
		if (Frame.left > w) Frame.left = w;
		Frame.top = (int)(1 / k * (point.y - r.top));
		if (Frame.top < 0) Frame.top = 0;
		if (Frame.top > h) Frame.top = h;

		Frame.right = Frame.left;
		Frame.bottom = Frame.top;

		ShowImage();
	}
}

void CSampleDlg::OnRButtonDown(UINT nFlags, CPoint point)
{
	CDialog::OnRButtonDown(nFlags, point);

	if (!IsImgLoaded()) return;

	RECT r;
	m_DocPic.GetWindowRect(&r);
	this->ScreenToClient(&r);
	POINT pt;
	if (PtInRect(&r, point))
	{
		pt = point;
		this->ClientToScreen(&pt);
		m_DocPic.ScreenToClient(&pt);

        float kk = GetCurDocScale();
        int cnt = NsOCR->Img_GetBlockCnt(ImgObj);
        int n = -1;
        int minsize = -1;
        int i, BlkObj, Xpos, Ypos, Width, Height, sz;
		RECT r;
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
        pmBlockTag = n; //remember block index
        GetCursorPos(&pt);

        int BlockType, rot, mir, inv;
        
        NsOCR->Img_GetBlock(ImgObj, pmBlockTag, &BlkObj);
        BlockType = NsOCR->Blk_GetType(BlkObj);
		CheckMenuItem(blockMenu, ID_BLK_TYPEOCR, (BlockType == BT_OCRTEXT) ? MF_CHECKED : MF_UNCHECKED);
		CheckMenuItem(blockMenu, ID_BLK_TYPEOCRDGT, (BlockType == BT_OCRDIGIT) ? MF_CHECKED : MF_UNCHECKED);
		CheckMenuItem(blockMenu, ID_BLK_TYPEICRDGT, (BlockType == BT_ICRDIGIT) ? MF_CHECKED : MF_UNCHECKED);
		CheckMenuItem(blockMenu, ID_BLK_TYPEBAR, (BlockType == BT_BARCODE) ? MF_CHECKED : MF_UNCHECKED);
		CheckMenuItem(blockMenu, ID_BLK_TYPETABLE, (BlockType == BT_TABLE) ? MF_CHECKED : MF_UNCHECKED);
		CheckMenuItem(blockMenu, ID_BLK_TYPECLR, (BlockType == BT_CLEAR) ? MF_CHECKED : MF_UNCHECKED);
		CheckMenuItem(blockMenu, ID_BLK_TYPEPIC, (BlockType == BT_PICTURE) ? MF_CHECKED : MF_UNCHECKED);
		CheckMenuItem(blockMenu, ID_BLK_TYPEZONING, (BlockType == BT_ZONING) ? MF_CHECKED : MF_UNCHECKED);	
		CheckMenuItem(blockMenu, ID_BLK_TYPEMRZ, (BlockType == BT_MRZ) ? MF_CHECKED : MF_UNCHECKED);

		inv = NsOCR->Blk_Inversion(BlkObj, BLK_INVERSE_GET);
		CheckMenuItem(blockMenu, ID_BLK_INVERT, inv ? MF_CHECKED : MF_UNCHECKED);

        rot = NsOCR->Blk_Rotation(BlkObj, BLK_ROTATE_GET);
		CheckMenuItem(blockMenu, ID_BLK_ROT90, (rot == BLK_ROTATE_270) ? MF_CHECKED : MF_UNCHECKED);
		CheckMenuItem(blockMenu, ID_BLK_ROT180, (rot == BLK_ROTATE_180) ? MF_CHECKED : MF_UNCHECKED);
		CheckMenuItem(blockMenu, ID_BLK_ROT270, (rot == BLK_ROTATE_90) ? MF_CHECKED : MF_UNCHECKED);

        mir = NsOCR->Blk_Mirror(BlkObj, BLK_MIRROR_GET);
		CheckMenuItem(blockMenu, ID_BLK_MIRH, (mir & BLK_MIRROR_H) ? MF_CHECKED : MF_UNCHECKED);
        CheckMenuItem(blockMenu, ID_BLK_MIRV, (mir & BLK_MIRROR_V) ? MF_CHECKED : MF_UNCHECKED);
		 
		int cmd = TrackPopupMenu(blockMenu, TPM_LEFTALIGN | TPM_TOPALIGN | TPM_RETURNCMD, pt.x, pt.y, 0, this->m_hWnd, NULL);

		switch (cmd)
		{
		case ID_BLK_TYPEOCR: NsOCR->Blk_SetType(BlkObj, BT_OCRTEXT); break;
		case ID_BLK_TYPEOCRDGT: NsOCR->Blk_SetType(BlkObj, BT_OCRDIGIT); break;
		case ID_BLK_TYPEICRDGT: NsOCR->Blk_SetType(BlkObj, BT_ICRDIGIT); break;
		case ID_BLK_TYPEBAR: NsOCR->Blk_SetType(BlkObj, BT_BARCODE); break;
		case ID_BLK_TYPETABLE: NsOCR->Blk_SetType(BlkObj, BT_TABLE); break;
		case ID_BLK_TYPECLR: NsOCR->Blk_SetType(BlkObj, BT_CLEAR); break;
		case ID_BLK_TYPEPIC: NsOCR->Blk_SetType(BlkObj, BT_PICTURE); break;
		case ID_BLK_TYPEZONING: NsOCR->Blk_SetType(BlkObj, BT_ZONING); break;
		case ID_BLK_TYPEMRZ: NsOCR->Blk_SetType(BlkObj, BT_MRZ); break;
		case ID_BLK_INVERT: NsOCR->Blk_Inversion(BlkObj, inv ? BLK_INVERSE_SET0 : BLK_INVERSE_SET1); break;
		case ID_BLK_ROT90: NsOCR->Blk_Rotation(BlkObj, (rot == BLK_ROTATE_270) ? BLK_ROTATE_NONE : BLK_ROTATE_270); break;
		case ID_BLK_ROT180: NsOCR->Blk_Rotation(BlkObj, (rot == BLK_ROTATE_180) ? BLK_ROTATE_NONE : BLK_ROTATE_180); break;
		case ID_BLK_ROT270: NsOCR->Blk_Rotation(BlkObj, (rot == BLK_ROTATE_90) ? BLK_ROTATE_NONE : BLK_ROTATE_90); break;
		case ID_BLK_MIRH: NsOCR->Blk_Mirror(BlkObj, (mir & BLK_MIRROR_H) ? (mir & ~BLK_MIRROR_H) : (mir | BLK_MIRROR_H)); break;
		case ID_BLK_MIRV: NsOCR->Blk_Mirror(BlkObj, (mir & BLK_MIRROR_V) ? (mir & ~BLK_MIRROR_V) : (mir | BLK_MIRROR_V)); break;
		case ID_BLK_DELETE: NsOCR->Img_DeleteBlock(ImgObj, BlkObj); break;
		}

		ShowImage();
	}
}


void CSampleDlg::OnMouseMove(UINT nFlags, CPoint point)
{
	CDialog::OnMouseMove(nFlags, point);
////
	if (!Dwn) return;

	if (!IsImgLoaded()) return;

	RECT r;
	m_DocPic.GetWindowRect(&r);
	this->ScreenToClient(&r);

	if (PtInRect(&r, point))
	{
		int w, h;
		NsOCR->Img_GetSize(ImgObj, &w, &h);

		float k = GetCurDocScale();
		Frame.right = (int)(1 / k * (point.x - r.left));
		if (Frame.right < 0) Frame.right = 0;
		if (Frame.right > w) Frame.right = w;
		Frame.bottom = (int)(1 / k * (point.y - r.top));
		if (Frame.bottom < 0) Frame.bottom = 0;
		if (Frame.bottom > h) Frame.bottom = h;

		ShowImage();
	}
}

void CSampleDlg::OnScaleChanged()
{
	m_bkRecognize.EnableWindow(0);
	OnBnClickedBkopen();
}

void CSampleDlg::cbBinOnClick()
{
	AdjustDocScale();
}


void CSampleDlg::cbExactCopyClicked()
{
	ShowText();
}

void CSampleDlg::OnBnClickedBkloadblocks()
{
	UNICODECHAR fn[512];
	OPENFILENAME ofn; 
	UNICODECHAR szFile[512]; 
	ZeroMemory(&ofn, sizeof(ofn));
	ofn.lStructSize = sizeof(ofn);
	ofn.hwndOwner = this->m_hWnd;
	ofn.lpstrFile = szFile;
	wcscpy_s(ofn.lpstrFile, 512, L"\0\0");
	ofn.nMaxFile = 260;
	ofn.lpstrFilter = L"blk files\0*.blk\0";
	ofn.nFilterIndex = 1;
	ofn.lpstrFileTitle = NULL;
	ofn.nMaxFileTitle = 0;
	ofn.lpstrInitialDir = NULL;
	ofn.Flags = OFN_EXPLORER | OFN_HIDEREADONLY | OFN_NONETWORKBUTTON | OFN_NOREADONLYRETURN | OFN_PATHMUSTEXIST | OFN_FILEMUSTEXIST;// show open file dialog

	if (!GetOpenFileName(&ofn)) return;
	wcscpy_s(fn, 512, ofn.lpstrFile);

    NsOCR->Img_DeleteAllBlocks(ImgObj); //note: Img_LoadBlocks does not remove existing blocks, so remove them here
	m_bkSave.EnableWindow(0);
    int res = NsOCR->Img_LoadBlocks(ImgObj, fn);
    if (res > ERROR_FIRST)
    {
        ShowError(L"Img_LoadBlocks", res);
        return;
    }
    ShowImage();
}

void CSampleDlg::OnBnClickedBksaveblocks()
{
	UNICODECHAR fn[512];
	OPENFILENAME ofn; 
	UNICODECHAR szFile[512]; 
	ZeroMemory(&ofn, sizeof(ofn));
	ofn.lStructSize = sizeof(ofn);
	ofn.hwndOwner = this->m_hWnd;
	ofn.lpstrFile = szFile;
	wcscpy_s(ofn.lpstrFile, 512, L"\0\0");
	ofn.nMaxFile = 260;
	ofn.lpstrFilter = L"blk files\0*.blk\0";
	ofn.nFilterIndex = 1;
	ofn.lpstrFileTitle = NULL;
	ofn.nMaxFileTitle = 0;
	ofn.lpstrInitialDir = NULL;
	ofn.lpstrDefExt = L".blk";
	ofn.Flags = OFN_EXPLORER | OFN_HIDEREADONLY | OFN_NONETWORKBUTTON | OFN_NOREADONLYRETURN | OFN_PATHMUSTEXIST | OFN_OVERWRITEPROMPT;// show save file dialog

	if (!GetSaveFileName(&ofn)) return;
	wcscpy_s(fn, 512, ofn.lpstrFile);

	int res = NsOCR->Img_SaveBlocks(ImgObj, fn);
	if (res > ERROR_FIRST) ShowError(L"Img_SaveBlocks", res);
}

void CSampleDlg::OnBnClickedBkclearblocks()
{
	NsOCR->Img_DeleteAllBlocks(ImgObj);
	m_bkSave.EnableWindow(0);
	ShowImage();
}

void CSampleDlg::OnBnClickedBkdetectblocks2()
{
	NsOCR->Img_DeleteAllBlocks(ImgObj);
	NsOCR->Img_OCR(ImgObj, OCRSTEP_ZONING, OCRSTEP_ZONING, OCRFLAG_NONE);
	ShowImage();
}

void CSampleDlg::OnBnClickedBksave()
{
	SvrObj = 0;
	bool ppMode = IsProcessPagesMode;
	IsProcessPagesMode = false;

	OPENFILENAME ofn; 
	UNICODECHAR szFile[512]; 
	ZeroMemory(&ofn, sizeof(ofn));
	ofn.lStructSize = sizeof(ofn);
	ofn.hwndOwner = this->m_hWnd;
	ofn.lpstrFile = szFile;
	wcscpy_s(ofn.lpstrFile, 512, L"\0\0");
	ofn.nMaxFile = 260;
	ofn.lpstrFilter = L"PDF document(*.pdf)\0*.pdf\0RTF document (*.rtf)\0*.rtf\0ASCII Text document (*.txt)\0*.txt\0Unicode Text document (*.txt)\0*.txt\0XML document (*.xml)\0*.xml\0PDF/A document(*.pdf)\0*.pdf\0";
	ofn.nFilterIndex = 1;
	ofn.lpstrFileTitle = NULL;
	ofn.nMaxFileTitle = 0;
	ofn.lpstrInitialDir = NULL;
	ofn.lpstrDefExt = L".pdf";
	ofn.Flags = OFN_EXPLORER | OFN_HIDEREADONLY | OFN_NONETWORKBUTTON | OFN_NOREADONLYRETURN | OFN_PATHMUSTEXIST | OFN_OVERWRITEPROMPT;// show save file dialog

	if (!GetSaveFileName(&ofn)) return;
	wcscpy_s(fn, 512, ofn.lpstrFile);

	int format = SVR_FORMAT_PDF + (ofn.nFilterIndex - 1);

    //image over text option for PDF
    if ((format == SVR_FORMAT_PDF) || (format == SVR_FORMAT_PDFA))
    {
        if (MessageBox(L"Place page image over recognized text?", L"", MB_YESNO) == IDYES)
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

	int flags = m_cbExactCopy.GetCheck() ? FMT_EXACTCOPY : FMT_EDITCOPY;

	NsOCR->Svr_NewDocument(SvrObj);

	if (ppMode) //caller is ProcessEntireDocument
		return;

	res = NsOCR->Svr_AddPage(SvrObj, ImgObj, flags);
	if (res > ERROR_FIRST)
	{
		ShowError(L"Svr_AddPage", res);
		NsOCR->Svr_Destroy(SvrObj);
		return;
	}

	if ((format == SVR_FORMAT_PDF) || (format == SVR_FORMAT_PDFA) || (format == SVR_FORMAT_XML))  //demonstrate how to write PDF info
		NsOCR->Svr_SetDocumentInfo(SvrObj, INFO_PDF_TITLE, L"Sample Title");

	//wchar_t TextStr[10000];
	//NsOCR->Svr_GetText(SvrObj, -1, TextStr, 10000);

	res = NsOCR->Svr_SaveToFile(SvrObj, fn);
	NsOCR->Svr_Destroy(SvrObj);

	if (res > ERROR_FIRST)
	{
		ShowError(L"Svr_SaveToFile", res);
		return;
	}

	//open the file
	ShellExecute(0, L"open", fn, L"", NULL, SW_SHOWNORMAL );
}

void CSampleDlg::OnBnClickedBkscan()
{
	TScanDlg ScanDlg;
	ScanDlg.DoModal();
}

void CSampleDlg::ProcessEntireDocument()
{
  IsProcessPagesMode = true;
  OnBnClickedBksave();
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

  int flags = m_cbExactCopy.GetCheck() ? FMT_EXACTCOPY : FMT_EDITCOPY;
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
	  CDlgWait DlgWait;
	  DlgWait.mode = 1;
	  DlgWait.DoModal();
	  res = DlgWait.res;
	}

  //restore option
  NsOCR->Cfg_SetOption(CfgObj, BT_DEFAULT, L"Main/NumKernels", L"0");

  if (res > ERROR_FIRST)
  {
	if (res == ERROR_OPERATIONCANCELLED)
		MessageBox(L"Operation was cancelled.");
       else
		ShowError(L"Ocr_ProcessPages", res);
    NsOCR->Svr_Destroy(SvrObj);
	return;
  }

  //save output document
	res = NsOCR->Svr_SaveToFile(SvrObj, fn);
	NsOCR->Svr_Destroy(SvrObj);

	if (res > ERROR_FIRST)
	{
		ShowError(L"Svr_SaveToFile", res);
		return;
	}

	//open the file
	ShellExecute(0, L"open", fn, L"", NULL, SW_SHOWNORMAL );
}
void CSampleDlg::OnBnClickedBkLanguages()
{
	TLangsDlg LangsDlg;
	LangsDlg.DoModal();
}

void CSampleDlg::OnBnClickedBkoptions()
{
	TOptionsDlg OptionsDlg;
	OptionsDlg.DoModal();
	// TODO: Add your control notification handler code here
}
