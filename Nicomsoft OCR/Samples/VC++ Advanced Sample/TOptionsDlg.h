#pragma once
#include "afxwin.h"


// TOptionsDlg dialog

class TOptionsDlg : public CDialog
{
	DECLARE_DYNAMIC(TOptionsDlg)

public:
	TOptionsDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~TOptionsDlg();

// Dialog Data
	enum { IDD = IDD_DIALOG_OPTIONS };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();
	virtual HBRUSH OnCtlColor(CDC* pDC, CWnd *pWnd, UINT nCtlColor);
	DECLARE_MESSAGE_MAP()
public:
	CButton m_cbFindBarcodes;
	CButton m_cbImgInversion;
	CButton m_cbZonesInversion;
	CButton m_cbDeskew;
	CButton m_cbRotation;
	CButton m_cbImgNoiseFilter;
	CButton m_cbRemoveLines;
	CButton m_cbGrayMode;
	CButton m_cbFastMode;
	CButton m_cbBinTwice;
	CEdit m_edEnabledChars;
	CEdit m_edDisabledChars;
	CEdit m_edBinThreshold;
	CEdit m_edTextQual;
	CEdit m_edPDFDPI;
	afx_msg void OnBnClickedOk();
	afx_msg void OnBnClickedCancel();
};
