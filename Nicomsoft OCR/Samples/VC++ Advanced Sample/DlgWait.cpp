// DlgWait.cpp : implementation file
//

#include "stdafx.h"
#include "Sample.h"
#include "DlgWait.h"
#include "SampleDlg.h"

extern CSampleDlg* gMainDlg;

// CDlgWait dialog

IMPLEMENT_DYNAMIC(CDlgWait, CDialog)

CDlgWait::CDlgWait(CWnd* pParent /*=NULL*/)
	: CDialog(CDlgWait::IDD, pParent)
{

}

CDlgWait::~CDlgWait()
{
}

void CDlgWait::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_PROGRESS1, m_Progress1);
}


BEGIN_MESSAGE_MAP(CDlgWait, CDialog)
	ON_BN_CLICKED(IDOK, &CDlgWait::OnBnClickedOk)
	ON_WM_TIMER()
	ON_WM_CLOSE()
	ON_BN_CLICKED(IDC_BUTTON1, &CDlgWait::OnBnClickedButton1)
END_MESSAGE_MAP()

#define	TIMERID	1000

BOOL CDlgWait::OnInitDialog()
{
	CDialog::OnInitDialog();

	SetTimer(TIMERID, 100, NULL);
	m_Progress1.SetPos(0);

	return TRUE;
}

bool CDlgWait::CheckDone()
{
	if (mode == 0)
		res = gMainDlg->NsOCR->Img_OCR(gMainDlg->ImgObj, 0, 0, OCRFLAG_GETRESULT);
	else
		res = gMainDlg->NsOCR->Ocr_ProcessPages(gMainDlg->ImgObj, 0, 0, 0, 0, OCRFLAG_GETRESULT);
	return res != ERROR_PENDING;
}

// CDlgWait message handlers

void CDlgWait::OnBnClickedOk()
{
}

void CDlgWait::OnOK()
{
	KillTimer(TIMERID);

	while (!CheckDone()) //make sure that Img_OCR is done
		Sleep(10);

	CDialog::OnOK();
}

void CDlgWait::OnCancel()
{
	OnOK();
}

void CDlgWait::OnTimer(UINT_PTR nIDEvent)
{
  int val;
  if (CheckDone())
	OnOK();

  if (mode == 0)
	val = gMainDlg->NsOCR->Img_OCR(gMainDlg->ImgObj, 0, 0, OCRFLAG_GETPROGRESS);
  else
	val = gMainDlg->NsOCR->Ocr_ProcessPages(gMainDlg->ImgObj, 0, 0, 0, 0, OCRFLAG_GETPROGRESS);

  if (val < ERROR_FIRST)
	  if (m_Progress1.GetPos() != val)
	  m_Progress1.SetPos(val);

	CDialog::OnTimer(nIDEvent);
}

void CDlgWait::OnClose()
{
	CDialog::OnClose();
}

void CDlgWait::OnBnClickedButton1()
{
	//cancel work
	if (mode == 0)
		gMainDlg->NsOCR->Img_OCR(gMainDlg->ImgObj, 0, 0, OCRFLAG_CANCEL);
	else
		gMainDlg->NsOCR->Ocr_ProcessPages(gMainDlg->ImgObj, 0, 0, 0, 0, OCRFLAG_CANCEL);

	//we must wait for result anyway since OCRFLAG_CANCEL request returns immediately, work is not stopped yet
	OnOK(); //this function waits for result
}
