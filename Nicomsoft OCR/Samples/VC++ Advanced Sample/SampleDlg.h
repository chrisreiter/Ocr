
// SampleDlg.h : header file
//

#pragma once

#include "../NSOCR.h"
#include "afxwin.h"

#include "NPicture.h"


// CSampleDlg dialog
class CSampleDlg : public CDialog
{
// Construction
public:
	CSampleDlg(CWnd* pParent = NULL);	// standard constructor
	~CSampleDlg();
// Dialog Data
	enum { IDD = IDD_SAMPLE_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()

	void AdjustSize();
	void AdjustDocScale();
	bool IsImgLoaded();
	float GetCurDocScale();
	void ShowImage();
	void ShowText();

	void ProcessEntireDocument();
public:
	afx_msg void OnBnClickedCancel();
	afx_msg void OnBnClickedOk();

	TNSOCR* NsOCR;

    HCFG CfgObj;
    HOCR OcrObj;
    HIMG ImgObj;
	HSCAN ScanObj;
	HSVR SvrObj;

	RECT Frame;
	bool Dwn;
	bool IsProcessPagesMode;
	UNICODECHAR fn[512];

	TNBitmap bmp;

	HMENU blockMenu;
	int pmBlockTag;

	void ShowError(UNICODECHAR* ProcName, int ErrorCode);
	void DoImageLoaded();

	TNPicture m_DocPic;
	CEdit m_edText;
	CButton m_bkOpen;
	CButton m_bkRecognize;
	CStatic m_lbWait;
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnBnClickedBkopen();
	afx_msg void OnBnClickedBkocr();
	afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	CEdit m_edPage;
	CStatic m_lbPages;
	afx_msg void OnBnClickedBksetpage();
	CComboBox m_cbScale;
	afx_msg void OnScaleChanged();
	CButton m_cbBin;
	afx_msg void cbBinOnClick();
	afx_msg void OnRButtonDown(UINT nFlags, CPoint point);
	CButton m_cbExactCopy;
	afx_msg void cbExactCopyClicked();
	CButton m_bkLoadBlocks;
	CButton m_bkSaveBlocks;
	CButton m_bkClearBlocks;
	afx_msg void OnBnClickedBkloadblocks();
	afx_msg void OnBnClickedBksaveblocks();
	afx_msg void OnBnClickedBkclearblocks();
	CButton m_bkDetectBlocks;
	afx_msg void OnBnClickedBkdetectblocks2();
	CButton m_bkSave;
	afx_msg void OnBnClickedBksave();
	afx_msg void OnBnClickedBkscan();
	afx_msg void OnBnClickedBkLanguages();
	afx_msg void OnBnClickedBkoptions();
};
