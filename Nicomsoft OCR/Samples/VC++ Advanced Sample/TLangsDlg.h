#pragma once
#include "afxcmn.h"


// TLangsDlg dialog

class TLangsDlg : public CDialog
{
	DECLARE_DYNAMIC(TLangsDlg)

public:
	TLangsDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~TLangsDlg();

// Dialog Data
	enum { IDD = IDD_DIALOG_LANGUAGES };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedOk();
	CListCtrl m_lvLanguages;
private:
	void LoadLanguages();
	bool ApplyLanguages();
public:
	afx_msg void OnBnClickedCancel();
	CListCtrl m_lvLanguagesAsian;
};
