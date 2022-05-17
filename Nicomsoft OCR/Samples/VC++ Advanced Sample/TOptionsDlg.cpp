// TOptionsDlg.cpp : implementation file
//
#pragma warning(disable : 4996) //remove "deprecated" warning

#include "stdafx.h"
#include "Sample.h"
#include "TOptionsDlg.h"
#include "SampleDlg.h"

extern CSampleDlg* gMainDlg;

// TOptionsDlg dialog

IMPLEMENT_DYNAMIC(TOptionsDlg, CDialog)

TOptionsDlg::TOptionsDlg(CWnd* pParent /*=NULL*/)
	: CDialog(TOptionsDlg::IDD, pParent)
{

}

TOptionsDlg::~TOptionsDlg()
{
}

void TOptionsDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_CB_FORMAT, m_cbFindBarcodes);
	DDX_Control(pDX, IDC_CB_FORMAT2, m_cbImgInversion);
	DDX_Control(pDX, IDC_CB_FORMAT3, m_cbZonesInversion);
	DDX_Control(pDX, IDC_CB_FORMAT5, m_cbDeskew);
	DDX_Control(pDX, IDC_CB_FORMAT4, m_cbRotation);
	DDX_Control(pDX, IDC_CB_FORMAT6, m_cbImgNoiseFilter);
	DDX_Control(pDX, IDC_CB_FORMAT7, m_cbRemoveLines);
	DDX_Control(pDX, IDC_CB_FORMAT8, m_cbGrayMode);
	DDX_Control(pDX, IDC_CB_FORMAT10, m_cbFastMode);
	DDX_Control(pDX, IDC_CB_FORMAT9, m_cbBinTwice);
	DDX_Control(pDX, IDC_EDPAGE2, m_edEnabledChars);
	DDX_Control(pDX, IDC_EDPAGE3, m_edDisabledChars);
	DDX_Control(pDX, IDC_EDPAGE4, m_edBinThreshold);
	DDX_Control(pDX, IDC_EDPAGE5, m_edTextQual);
	DDX_Control(pDX, IDC_EDPAGE6, m_edPDFDPI);
}


BEGIN_MESSAGE_MAP(TOptionsDlg, CDialog)
	ON_BN_CLICKED(IDOK, &TOptionsDlg::OnBnClickedOk)
	ON_BN_CLICKED(IDCANCEL, &TOptionsDlg::OnBnClickedCancel)
	ON_WM_CTLCOLOR()
END_MESSAGE_MAP()

HBRUSH TOptionsDlg::OnCtlColor(CDC* pDC, CWnd *pWnd, UINT nCtlColor)
{
	  HBRUSH hbr = CDialog::OnCtlColor(pDC, pWnd, nCtlColor);      
  
   if((pWnd->GetDlgCtrlID() == IDC_STWAIT) || (pWnd->GetDlgCtrlID() == IDC_STWAIT4) || (pWnd->GetDlgCtrlID() == IDC_STWAIT10))   
   {      
      //set the static text color to red      
      pDC->SetTextColor(RGB(255, 0, 0));   
   }    
   return hbr;
} 

BOOL TOptionsDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

    wchar_t  val[256];

    gMainDlg->NsOCR->Cfg_GetOption(gMainDlg->CfgObj, BT_DEFAULT, L"Zoning/FindBarcodes", val, 256);
	if (wcscmp(val, L"1") == 0)
      m_cbFindBarcodes.SetCheck(1);
    else
      m_cbFindBarcodes.SetCheck(0); 

    gMainDlg->NsOCR->Cfg_GetOption(gMainDlg->CfgObj, BT_DEFAULT, L"ImgAlizer/Inversion", val, 256);
	if (wcscmp(val, L"2") == 0)
      m_cbImgInversion.SetCheck(1);
    else
      m_cbImgInversion.SetCheck(0);

    gMainDlg->NsOCR->Cfg_GetOption(gMainDlg->CfgObj, BT_DEFAULT, L"Zoning/DetectInversion", val, 256);
	if (wcscmp(val, L"1") == 0)
      m_cbZonesInversion.SetCheck(1);
    else
      m_cbZonesInversion.SetCheck(0);

    gMainDlg->NsOCR->Cfg_GetOption(gMainDlg->CfgObj, BT_DEFAULT, L"ImgAlizer/SkewAngle", val, 256);
	if (wcscmp(val, L"360") == 0)
      m_cbDeskew.SetCheck(1);
    else
      m_cbDeskew.SetCheck(0);

    gMainDlg->NsOCR->Cfg_GetOption(gMainDlg->CfgObj, BT_DEFAULT, L"ImgAlizer/AutoRotate", val, 256);
	if (wcscmp(val, L"1") == 0)
      m_cbRotation.SetCheck(1);
    else
      m_cbRotation.SetCheck(0);

    gMainDlg->NsOCR->Cfg_GetOption(gMainDlg->CfgObj, BT_DEFAULT, L"ImgAlizer/NoiseFilter", val, 256);
	if (wcscmp(val, L"1") == 0)
      m_cbImgNoiseFilter.SetCheck(1);
    else
      m_cbImgNoiseFilter.SetCheck(0);

    gMainDlg->NsOCR->Cfg_GetOption(gMainDlg->CfgObj, BT_DEFAULT, L"PixLines/RemoveLines", val, 256);
	if (wcscmp(val, L"1") == 0)
      m_cbRemoveLines.SetCheck(1);
    else
      m_cbRemoveLines.SetCheck(0);

    gMainDlg->NsOCR->Cfg_GetOption(gMainDlg->CfgObj, BT_DEFAULT, L"Main/GrayMode", val, 256);
	if (wcscmp(val, L"1") == 0)
      m_cbGrayMode.SetCheck(1);
    else
      m_cbGrayMode.SetCheck(0);

    gMainDlg->NsOCR->Cfg_GetOption(gMainDlg->CfgObj, BT_DEFAULT, L"Main/FastMode", val, 256);
	if (wcscmp(val, L"1") == 0)
      m_cbFastMode.SetCheck(1);
    else
      m_cbFastMode.SetCheck(0);

    gMainDlg->NsOCR->Cfg_GetOption(gMainDlg->CfgObj, BT_DEFAULT, L"Binarizer/BinTwice", val, 256);
	if (wcscmp(val, L"1") == 0)
      m_cbBinTwice.SetCheck(1);
    else
      m_cbBinTwice.SetCheck(0);

    gMainDlg->NsOCR->Cfg_GetOption(gMainDlg->CfgObj, BT_DEFAULT, L"Main/EnabledChars", val, 256);
    m_edEnabledChars.SetWindowTextW(val);

    gMainDlg->NsOCR->Cfg_GetOption(gMainDlg->CfgObj, BT_DEFAULT, L"Main/DisabledChars", val, 256);
    m_edDisabledChars.SetWindowTextW(val);

    gMainDlg->NsOCR->Cfg_GetOption(gMainDlg->CfgObj, BT_DEFAULT, L"Binarizer/SimpleThr", val, 256);
    m_edBinThreshold.SetWindowTextW(val);

    gMainDlg->NsOCR->Cfg_GetOption(gMainDlg->CfgObj, BT_DEFAULT, L"WordAlizer/TextQual", val, 256);
    m_edTextQual.SetWindowTextW(val);

    gMainDlg->NsOCR->Cfg_GetOption(gMainDlg->CfgObj, BT_DEFAULT, L"Main/PdfDPI", val, 256);
    m_edPDFDPI.SetWindowTextW(val);

	return TRUE;
}

// TOptionsDlg message handlers

void TOptionsDlg::OnBnClickedOk()
{
	// TODO: Add your control notification handler code here

    wchar_t  val[256];
	wcscpy(val, L"");

	if (m_cbFindBarcodes.GetCheck()) wcscpy(val, L"1"); 
    else wcscpy(val, L"0"); 
    gMainDlg->NsOCR->Cfg_SetOption(gMainDlg->CfgObj, BT_DEFAULT, L"Zoning/FindBarcodes", val);

    if (m_cbImgInversion.GetCheck()) wcscpy(val, L"2");
    else wcscpy(val, L"0");
    gMainDlg->NsOCR->Cfg_SetOption(gMainDlg->CfgObj, BT_DEFAULT, L"ImgAlizer/Inversion", val);

    if (m_cbZonesInversion.GetCheck()) wcscpy(val, L"1");
    else wcscpy(val, L"0");
    gMainDlg->NsOCR->Cfg_SetOption(gMainDlg->CfgObj, BT_DEFAULT, L"Zoning/DetectInversion", val);

    if (m_cbDeskew.GetCheck()) wcscpy(val, L"360");
    else wcscpy(val, L"0");
    gMainDlg->NsOCR->Cfg_SetOption(gMainDlg->CfgObj, BT_DEFAULT, L"ImgAlizer/SkewAngle", val);

    if (m_cbRotation.GetCheck()) wcscpy(val, L"1");
    else wcscpy(val, L"0");
    gMainDlg->NsOCR->Cfg_SetOption(gMainDlg->CfgObj, BT_DEFAULT, L"ImgAlizer/AutoRotate", val);

    if (m_cbImgNoiseFilter.GetCheck()) wcscpy(val, L"1");
    else wcscpy(val, L"0");
    gMainDlg->NsOCR->Cfg_SetOption(gMainDlg->CfgObj, BT_DEFAULT, L"ImgAlizer/NoiseFilter", val);

    if (m_cbRemoveLines.GetCheck()) wcscpy(val, L"1");
    else wcscpy(val, L"0");
    gMainDlg->NsOCR->Cfg_SetOption(gMainDlg->CfgObj, BT_DEFAULT, L"PixLines/RemoveLines", val);
    gMainDlg->NsOCR->Cfg_SetOption(gMainDlg->CfgObj, BT_DEFAULT, L"PixLines/FindHorLines", val);
    gMainDlg->NsOCR->Cfg_SetOption(gMainDlg->CfgObj, BT_DEFAULT, L"PixLines/FindVerLines", val);

    if (m_cbGrayMode.GetCheck()) wcscpy(val, L"1");
    else wcscpy(val, L"0");
    gMainDlg->NsOCR->Cfg_SetOption(gMainDlg->CfgObj, BT_DEFAULT, L"Main/GrayMode", val);

    if (m_cbFastMode.GetCheck()) wcscpy(val, L"1");
    else wcscpy(val, L"0");
    gMainDlg->NsOCR->Cfg_SetOption(gMainDlg->CfgObj, BT_DEFAULT, L"Main/FastMode", val);

    if (m_cbBinTwice.GetCheck()) wcscpy(val, L"1");
    else wcscpy(val, L"0");
    gMainDlg->NsOCR->Cfg_SetOption(gMainDlg->CfgObj, BT_DEFAULT, L"Binarizer/BinTwice", val);

	m_edEnabledChars.GetWindowTextW(val, 256);
    gMainDlg->NsOCR->Cfg_SetOption(gMainDlg->CfgObj, BT_DEFAULT, L"Main/EnabledChars", val);
	m_edDisabledChars.GetWindowTextW(val, 256);
    gMainDlg->NsOCR->Cfg_SetOption(gMainDlg->CfgObj, BT_DEFAULT, L"Main/DisabledChars", val);
	m_edBinThreshold.GetWindowTextW(val, 256);
    gMainDlg->NsOCR->Cfg_SetOption(gMainDlg->CfgObj, BT_DEFAULT, L"Binarizer/SimpleThr", val);
	m_edTextQual.GetWindowTextW(val, 256);
    gMainDlg->NsOCR->Cfg_SetOption(gMainDlg->CfgObj, BT_DEFAULT, L"WordAlizer/TextQual", val);
	m_edPDFDPI.GetWindowTextW(val, 256);
    gMainDlg->NsOCR->Cfg_SetOption(gMainDlg->CfgObj, BT_DEFAULT, L"Main/PdfDPI", val);

	OnOK();
}

void TOptionsDlg::OnBnClickedCancel()
{
	// TODO: Add your control notification handler code here
	OnCancel();
}
