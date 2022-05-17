//******************************************************************************
//                             NPicture Class
//                    Copyright (c) 2010-2011 Nicomsoft
//                 Copyright (c) 2010-2011 Michael Kapustin
//******************************************************************************

#ifndef _NPicture_H_
#define _NPicture_H_

#include "sdkddkver.h"
#include "afxwin.h"

#include "NBitmap.h"

class TNPicture : public CStatic
{
private:
	TNBitmap bmp;
	CScrollBar scrV;
	CScrollBar scrH;
	bool IsScrollEnabled;
	void UpdateImg();
protected:
	DECLARE_MESSAGE_MAP()
	void OnPaint();
	void DoPaint(CDC *dc);
public:
	TNPicture();
	afx_msg void OnVScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar);
	afx_msg void OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	
	void AssignBitmap(TNBitmap* bm);
	void CreateScrollBars(UINT ScrIDV, UINT ScrIDH);
	void GetRealPoint(POINT* pt);

	bool MouseMoveFlag;
};


#endif //_NPicture_H_