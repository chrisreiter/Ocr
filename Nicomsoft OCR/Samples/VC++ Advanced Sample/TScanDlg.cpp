// TScanDlg.cpp : implementation file
//

#include "stdafx.h"
#include "Sample.h"
#include "TScanDlg.h"
#include "SampleDlg.h"

extern CSampleDlg* gMainDlg;


// TScanDlg dialog

IMPLEMENT_DYNCREATE(TScanDlg, CDialog)

TScanDlg::TScanDlg(CWnd* pParent /*=NULL*/)
	: CDialog(TScanDlg::IDD, pParent)
{

}

TScanDlg::~TScanDlg()
{
}

void TScanDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_COMBO1, m_cbScanners);
	DDX_Control(pDX, IDC_COMBO2, m_cbSrc);
	DDX_Control(pDX, IDC_CHECK1, m_cbNoUI);
}

BEGIN_MESSAGE_MAP(TScanDlg, CDialog)
	ON_BN_CLICKED(IDC_BUTTON1, &TScanDlg::OnBnClickedButton1)
	ON_BN_CLICKED(IDOK, &TScanDlg::OnBnClickedOk)
END_MESSAGE_MAP()


BOOL TScanDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	m_cbSrc.AddString(L"Flatbed");
	m_cbSrc.AddString(L"ADF(Automatic Document Feeder)");
	m_cbSrc.AddString(L"Auto");
	m_cbSrc.SetCurSel(0);

	m_cbScanners.ResetContent();

	int res;
	UNICODECHAR buf[1024];

	res = gMainDlg->NsOCR->Scan_Enumerate(gMainDlg->ScanObj, buf, 1024, 0);
	if (res > ERROR_FIRST)
	{
		gMainDlg->ShowError(L"Scan_Enumerate", res);
		return TRUE;
	}

//names are separated by comma, parse them
	UNICODECHAR r[100];
	int i, n;
	r[0] = 0;
	for (i = 0; i < (int)wcslen(buf); i++)
	{
		if (buf[i] != ',')
		{
			n = (int)wcslen(r);
			r[n] = buf[i];
			r[n + 1] = 0;
		}
		else
		{
			m_cbScanners.AddString(r);
			r[0] = 0;
		}
	}
	if (r[0])
		m_cbScanners.AddString(r);

//now get default device index
	res = gMainDlg->NsOCR->Scan_Enumerate(gMainDlg->ScanObj, NULL, 0, SCAN_GETDEFAULTDEVICE);
	if (res > ERROR_FIRST)
	{
		if (res != ERROR_NODEFAULTDEVICE)
		{
			gMainDlg->ShowError(L"Scan_Enumerate (1)", res);
			return TRUE;
		}
	}
	else
	{
		if (res < m_cbScanners.GetCount())
			m_cbScanners.SetCurSel(res);
	}


	return TRUE;  // return TRUE  unless you set the focus to a control
}

void TScanDlg::OnBnClickedButton1()
{
	int res, n;
	n = m_cbScanners.GetCurSel();
	if (n < 0) return;

	res = gMainDlg->NsOCR->Scan_Enumerate(gMainDlg->ScanObj, NULL, 0, SCAN_SETDEFAULTDEVICE | n);
	if (res > ERROR_FIRST)
	{
		gMainDlg->ShowError(L"Scan_Enumerate (2)", res);
		return;
	}
}

void TScanDlg::OnBnClickedOk()
{
	int n, flags, res;
	n = m_cbScanners.GetCurSel();
	if (n < 0)
	{
		MessageBox(L"No scanner selected!");
		return;
	}

	flags = 0;
	switch (m_cbSrc.GetCurSel())
	{
	case 0: break; //flatbed
	case 1: flags = flags | SCAN_SOURCEADF; break; //adf
	case 2: flags = flags | SCAN_SOURCEAUTO; break; //auto
	}

	if (m_cbNoUI.GetCheck())
		flags = flags | SCAN_NOUI;

	res = gMainDlg->NsOCR->Scan_ScanToImg(gMainDlg->ScanObj, gMainDlg->ImgObj, n, flags);
	//  res = gMainDlg->NsOCR->Scan_ScanToFile(gMainDlg->ScanObj, L"c:\\temp.tif", n, flags);

	if (res > ERROR_FIRST)
	{
		gMainDlg->ShowError(L"Scan_ScanToImg", res);
		return;
	}

	OnOK();
	gMainDlg->DoImageLoaded();
}
