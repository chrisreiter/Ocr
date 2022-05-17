//******************************************************************************
//                             NPicture Class
//                    Copyright (c) 2010-2011 Nicomsoft
//                 Copyright (c) 2010-2011 Michael Kapustin
//******************************************************************************


#include "Forcelib.h"

#include "NPicture.h"



BEGIN_MESSAGE_MAP(TNPicture, CStatic)
	ON_WM_PAINT()
	ON_WM_VSCROLL()
	ON_WM_HSCROLL()
	ON_WM_SIZE()
	ON_WM_ERASEBKGND()
	ON_WM_MOUSEMOVE()
END_MESSAGE_MAP()

TNPicture::TNPicture()
{
	IsScrollEnabled = false;
	MouseMoveFlag = false;
}

void TNPicture::OnPaint()
{
//	CStatic::OnPaint();

	CRect r;
	GetClientRect(&r);

	CPaintDC dc(this);

	CDC memDC;
	memDC.CreateCompatibleDC(&dc);
	CBitmap* oldBmp;
	CBitmap memBmp;
	memBmp.CreateCompatibleBitmap(&dc, r.Width(), r.Height());
	oldBmp = memDC.SelectObject(&memBmp);

	DoPaint(&memDC);

	dc.BitBlt(r.left, r.top, r.Width(), r.Height(), &memDC, 0, 0, SRCCOPY);

	memDC.SelectObject(oldBmp);
}

void TNPicture::DoPaint(CDC *dc)
{
	// client rectangle
	CRect r;
	GetClientRect(&r);

	int h = r.Height();
	int w = r.Width();

	CBrush br(0xBFBFBF);
	dc->FillRect(r, &br);

	if (!bmp.GetWidth()) return;

	if (!IsScrollEnabled)
	{
		BitBlt(dc->m_hDC, 0, 0, w, h, bmp.GetDC(), 0, 0, SRCCOPY);
	}
	else
	{
		int vr = scrV.GetScrollPos();
		int hr = scrH.GetScrollPos();

		BitBlt(dc->m_hDC, 0, 0, w, h, bmp.GetDC(), hr, vr, SRCCOPY);
	}
}

BOOL TNPicture::OnEraseBkgnd(CDC* pDC)
{
	return TRUE;
}

void TNPicture::AssignBitmap(TNBitmap* bm)
{
	int bw = bm->GetWidth();
	int bh = bm->GetHeight();
	CRect r;
	GetClientRect(&r);
	int h = r.Height();
	int w = r.Width();
////////
	if (IsScrollEnabled)
	{
		scrV.SetScrollRange(0, bh);
		//scrV.SetScrollPos(0);

		scrH.SetScrollRange(0, bw);
		//scrH.SetScrollPos(0);
	}
////////
	bmp.SetSize(bw, bh);
	bmp.Draw(0, 0, bm);
	Invalidate();
}

void TNPicture::CreateScrollBars(UINT ScrIDV, UINT ScrIDH)
{
	CRect r;
	GetClientRect(&r);
	int h = r.Height();
	int w = r.Width();

	scrV.Create(SBS_VERT | SBS_RIGHTALIGN | WS_CHILD, CRect(w-25, 0, w, h-20), this, ScrIDV);
	scrV.ShowScrollBar();

	scrH.Create(SBS_HORZ | SBS_BOTTOMALIGN | WS_CHILD, CRect(0, h-25, w-20, h), this, ScrIDH);
	scrH.ShowScrollBar();

	IsScrollEnabled = true;
}

void TNPicture::OnVScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar)
{
	int pos = pScrollBar->GetScrollPos();
	CRect r;
	GetClientRect(&r);
	switch (nSBCode)
	{
		case SB_LINEDOWN: scrV.SetScrollPos(pos + 1); UpdateImg(); break;
		case SB_PAGEDOWN: scrV.SetScrollPos(pos + r.Height()); UpdateImg(); break;
		case SB_LINEUP: scrV.SetScrollPos(pos - 1); UpdateImg(); break;
		case SB_PAGEUP: scrV.SetScrollPos(pos - r.Height()); UpdateImg(); break;
		case SB_THUMBPOSITION: scrV.SetScrollPos(nPos); UpdateImg(); break;
		case SB_THUMBTRACK: scrV.SetScrollPos(nPos); UpdateImg(); break;
	}
}

void TNPicture::OnHScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar)
{
	int pos = pScrollBar->GetScrollPos();
	CRect r;
	GetClientRect(&r);
	switch (nSBCode)
	{
		case SB_LINERIGHT: scrH.SetScrollPos(pos + 1); UpdateImg(); break;
		case SB_PAGERIGHT: scrH.SetScrollPos(pos + r.Width()); UpdateImg(); break;
		case SB_LINELEFT: scrH.SetScrollPos(pos - 1); UpdateImg(); break;
		case SB_PAGELEFT: scrH.SetScrollPos(pos - r.Width()); UpdateImg(); break;
		case SB_THUMBPOSITION: scrH.SetScrollPos(nPos); UpdateImg(); break;
		case SB_THUMBTRACK: scrH.SetScrollPos(nPos); UpdateImg(); break;
	}
}

void TNPicture::UpdateImg()
{
	Invalidate();
}

void TNPicture::OnSize(UINT nType, int cx, int cy)
{
	CStatic::OnSize(nType, cx, cy);

	if (!IsScrollEnabled) return;

	CRect r;
	GetClientRect(&r);
	int h = r.Height();
	int w = r.Width();

	scrV.MoveWindow(w-17, 0, 17, h);
	scrH.MoveWindow(0, h-17, w, 17);
}

void TNPicture::GetRealPoint(POINT* pt)
{
	if (!IsScrollEnabled) return;
	pt->x += scrH.GetScrollPos();
	pt->y += scrV.GetScrollPos();
}

void TNPicture::OnMouseMove(UINT nFlags, CPoint point)
{
	CStatic::OnMouseMove(nFlags, point);

	MouseMoveFlag = true;
	GetParent()->SendMessage(WM_MOUSEMOVE, 0, 0);
	MouseMoveFlag = false;
}