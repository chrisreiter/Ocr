// TLangsDlg.cpp : implementation file
//

#pragma warning(disable : 4996) //remove "deprecated" warning

#include "stdafx.h"
#include "Sample.h"
#include "TLangsDlg.h"
#include "SampleDlg.h"

extern CSampleDlg* gMainDlg;

// TLangsDlg dialog

IMPLEMENT_DYNAMIC(TLangsDlg, CDialog)

TLangsDlg::TLangsDlg(CWnd* pParent /*=NULL*/)
	: CDialog(TLangsDlg::IDD, pParent)
{

}

TLangsDlg::~TLangsDlg()
{
}

void TLangsDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST2, m_lvLanguages);
	DDX_Control(pDX, IDC_LIST3, m_lvLanguagesAsian);
}


BEGIN_MESSAGE_MAP(TLangsDlg, CDialog)
	ON_BN_CLICKED(IDOK, &TLangsDlg::OnBnClickedOk)
	ON_BN_CLICKED(IDCANCEL, &TLangsDlg::OnBnClickedCancel)
END_MESSAGE_MAP()

BOOL TLangsDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	m_lvLanguages.SetExtendedStyle( m_lvLanguages.GetExtendedStyle() | LVS_EX_CHECKBOXES | LVS_EX_FULLROWSELECT);
	m_lvLanguages.InsertColumn(0, L"Language Name", 0, 150);

	m_lvLanguages.InsertItem(1000, L"Bulgarian", -1);
	m_lvLanguages.InsertItem(1000, L"Catalan", -1);
	m_lvLanguages.InsertItem(1000, L"Croatian", -1);
	m_lvLanguages.InsertItem(1000, L"Czech", -1);
	m_lvLanguages.InsertItem(1000, L"Danish", -1);

	m_lvLanguages.InsertItem(1000, L"Dutch", -1);
	m_lvLanguages.InsertItem(1000, L"English", -1);
	m_lvLanguages.InsertItem(1000, L"Estonian", -1);
	m_lvLanguages.InsertItem(1000, L"Finnish", -1);
	m_lvLanguages.InsertItem(1000, L"French", -1);

	m_lvLanguages.InsertItem(1000, L"German", -1);
	m_lvLanguages.InsertItem(1000, L"Hungarian", -1);
	m_lvLanguages.InsertItem(1000, L"Indonesian", -1);
	m_lvLanguages.InsertItem(1000, L"Italian", -1);
	m_lvLanguages.InsertItem(1000, L"Latvian", -1);

	m_lvLanguages.InsertItem(1000, L"Lithuanian", -1);
	m_lvLanguages.InsertItem(1000, L"Norwegian", -1);
	m_lvLanguages.InsertItem(1000, L"Polish", -1);
	m_lvLanguages.InsertItem(1000, L"Portuguese", -1);
	m_lvLanguages.InsertItem(1000, L"Romanian", -1);

	m_lvLanguages.InsertItem(1000, L"Russian", -1);
	m_lvLanguages.InsertItem(1000, L"Slovak", -1);
	m_lvLanguages.InsertItem(1000, L"Slovenian", -1);
	m_lvLanguages.InsertItem(1000, L"Spanish", -1);
	m_lvLanguages.InsertItem(1000, L"Swedish", -1);

	m_lvLanguages.InsertItem(1000, L"Turkish", -1);

	m_lvLanguagesAsian.SetExtendedStyle( m_lvLanguagesAsian.GetExtendedStyle() | LVS_EX_CHECKBOXES | LVS_EX_FULLROWSELECT);
	m_lvLanguagesAsian.InsertColumn(0, L"Language Name", 0, 150);

	m_lvLanguagesAsian.InsertItem(1000, L"Arabic", -1);
	m_lvLanguagesAsian.InsertItem(1000, L"Chinese_Simplified", -1);
	m_lvLanguagesAsian.InsertItem(1000, L"Chinese_Traditional", -1);
	m_lvLanguagesAsian.InsertItem(1000, L"Japanese", -1);
	m_lvLanguagesAsian.InsertItem(1000, L"Korean", -1);

	LoadLanguages();

	return TRUE;
}

// TLangsDlg message handlers

void TLangsDlg::OnBnClickedOk()
{
	if (ApplyLanguages())
		OnOK();
}

void TLangsDlg::LoadLanguages()
{
	int i;
	wchar_t val[100], str[100]; 
	for (i = 0; i < m_lvLanguages.GetItemCount(); i++)
	{
		m_lvLanguages.GetItemText(i, 0, val, 100);
		wcscpy(str, L"Languages/");
		wcscat(str, val);
		if (gMainDlg->NsOCR->Cfg_GetOption(gMainDlg->CfgObj, BT_DEFAULT, str, val, 100) < ERROR_FIRST)
			m_lvLanguages.SetCheck(i, val[0] == L'1' ? 1 : 0);
        else
            m_lvLanguages.SetCheck(i, 0);
	}
	for (i = 0; i < m_lvLanguagesAsian.GetItemCount(); i++)
	{
		m_lvLanguagesAsian.GetItemText(i, 0, val, 100);
		wcscpy(str, L"Languages/");
		wcscat(str, val);
		if (gMainDlg->NsOCR->Cfg_GetOption(gMainDlg->CfgObj, BT_DEFAULT, str, val, 100) < ERROR_FIRST)
			m_lvLanguagesAsian.SetCheck(i, val[0] == L'1' ? 1 : 0);
        else
            m_lvLanguagesAsian.SetCheck(i, 0);
	}
}

bool TLangsDlg::ApplyLanguages()
{
    int i;
    bool ch1 = false;
    for (i = 0; i < m_lvLanguages.GetItemCount(); i++)
        if (m_lvLanguages.GetCheck(i))
        {
            ch1 = true;
            break;
        }
    int ch2 = 0;
    for (i = 0; i < m_lvLanguagesAsian.GetItemCount(); i++)
        if (m_lvLanguagesAsian.GetCheck(i))
        {
            ch2++;
            break;
        }

    if (!ch1 && !ch2)
    {
        MessageBox(L"Please select at least one language for recognition.");
        return false;
    }
    if (ch1 && ch2)
    {
        MessageBox(L"Using both main and asian languages in same zone is not supported.");
        return false;
    }
    if (ch2 > 1)
    {
        MessageBox(L"Using two or more asian languages in same zone is not supported currently.");
        return false;
    }
	wchar_t val[100], str[100]; 
    for (i = 0; i < m_lvLanguages.GetItemCount(); i++)
	{
		m_lvLanguages.GetItemText(i, 0, val, 100);
		wcscpy(str, L"Languages/");
		wcscat(str, val);
        gMainDlg->NsOCR->Cfg_SetOption(gMainDlg->CfgObj, BT_DEFAULT, str, m_lvLanguages.GetCheck(i) ? L"1" : L"0");
	}
    for (i = 0; i < m_lvLanguagesAsian.GetItemCount(); i++)
	{
		m_lvLanguagesAsian.GetItemText(i, 0, val, 100);
		wcscpy(str, L"Languages/");
		wcscat(str, val);
        gMainDlg->NsOCR->Cfg_SetOption(gMainDlg->CfgObj, BT_DEFAULT, str, m_lvLanguagesAsian.GetCheck(i) ? L"1" : L"0");
	}
    return true;
}
void TLangsDlg::OnBnClickedCancel()
{
	// TODO: Add your control notification handler code here
	OnCancel();
}
