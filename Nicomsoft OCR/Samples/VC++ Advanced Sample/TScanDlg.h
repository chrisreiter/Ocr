#pragma once
#include "afxwin.h"


// TScanDlg dialog

class TScanDlg : public CDialog
{
	DECLARE_DYNCREATE(TScanDlg)

public:
	TScanDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~TScanDlg();

	// Dialog Data
	enum { IDD = IDD_DIALOG_SCAN, IDH = IDR_HTML_TSCANDLG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();

	DECLARE_MESSAGE_MAP()
public:
	CComboBox m_cbScanners;
	CComboBox m_cbSrc;
	afx_msg void OnBnClickedButton1();
	afx_msg void OnBnClickedOk();
	CButton m_cbNoUI;
};
