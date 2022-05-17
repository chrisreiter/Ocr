#pragma once
#include "afxcmn.h"


// CDlgWait dialog

class CDlgWait : public CDialog
{
	DECLARE_DYNAMIC(CDlgWait)

public:
	CDlgWait(CWnd* pParent = NULL);   // standard constructor
	virtual ~CDlgWait();

	int res;
	int mode;

// Dialog Data
	enum { IDD = IDD_DIALOG_WAIT };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();

	bool CheckDone();

	virtual void OnOK();
	virtual void OnCancel();

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedOk();
	CProgressCtrl m_Progress1;
	afx_msg void OnTimer(UINT_PTR nIDEvent);
	afx_msg void OnClose();
	afx_msg void OnBnClickedButton1();
};
