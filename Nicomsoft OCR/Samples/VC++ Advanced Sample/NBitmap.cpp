//******************************************************************************
//                           TNBitmap Class
//                    Copyright (c) 2010-2011 Nicomsoft
//                 Copyright (c) 2010-2011 Michael Kapustin
//******************************************************************************


#include "Forcelib.h"

#include "NBitmap.h"


TNBitmap::TNBitmap()
{
	Width = 0;
	Height = 0;
	Handle = NULL;

	HDC DisplayDC = ::GetDC(NULL);
	DC = CreateCompatibleDC(DisplayDC);
	ReleaseDC(NULL, DisplayDC);
	Brush = CreateSolidBrush(0xFFFFFF);
	SelectObject(DC, Brush);
	Pen = CreatePen(PS_SOLID, 1, 0);
	SelectObject(DC, Pen);

	Font = NULL;
}

TNBitmap::~TNBitmap()
{
	if (Handle) DeleteObject(Handle);
	DeleteObject(Brush);
	DeleteObject(Pen);
	if (Font) DeleteObject(Font);
	DeleteDC(DC);	
}

void TNBitmap::SetSize(int w, int h)
{
	if ((w == Width) && (h == Height)) return;
	if (Handle) DeleteObject(Handle);

    ::ZeroMemory (&bmpHeader, sizeof(bmpHeader));
    bmpHeader.bmiHeader.biSize = sizeof (BITMAPINFOHEADER);
    bmpHeader.bmiHeader.biWidth = w;
    bmpHeader.bmiHeader.biHeight = h;
    bmpHeader.bmiHeader.biPlanes = 1;
    bmpHeader.bmiHeader.biBitCount = 24;
    bmpHeader.bmiHeader.biCompression = BI_RGB; //format
    bmpHeader.bmiHeader.biSizeImage = 0;  // for BI_RGB
 
	Handle = CreateDIBSection(DC, &bmpHeader, DIB_RGB_COLORS, (LPVOID *)&pBmpData, NULL, 0);

	SelectObject(DC, Handle);

	Width = w;
	Height = h;

	RECT r;
	r.left = 0;
	r.top = 0;
	r.right = Width;
	r.bottom = Height;
	HBRUSH Br = CreateSolidBrush(0xFFFFFF);
	::FillRect(DC, &r, Br);
	DeleteObject(Br);
}

void TNBitmap::SetPixelFormat(int pf)
{
	//
}

int GetPitch(int Width, int BytesPerPixel)
{
	int Pitch = Width * BytesPerPixel;
	while (Pitch & 3) Pitch++;
	return Pitch;
}

void* TNBitmap::ScanLine(int y)
{
    y = Height - y - 1; // bottom-up DIB
    LPBYTE res = pBmpData + y * GetPitch(Width, 3);
	return res;
}

void TNBitmap::Draw(int x, int y, TNBitmap* bmp)
{
	BitBlt(DC, x, y, Width, Height, bmp->GetDC(), 0, 0, SRCCOPY);
}

void TNBitmap::StretchDraw(RECT r, TNBitmap* bmp)
{
	SetStretchBltMode(DC, HALFTONE);
	StretchBlt(DC, r.left, r.top, r.right - r.left + 1, r.bottom - r.top + 1, bmp->DC, 0, 0, bmp->GetWidth(), bmp->GetHeight(), SRCCOPY);
}

int TNBitmap::TextWidthW(wchar_t* str)
{
	SIZE sz;
	GetTextExtentPoint32(DC, str, (int)wcslen(str), &sz);
	return sz.cx;
}

int TNBitmap::TextHeightW(wchar_t* str)
{
	SIZE sz;
	GetTextExtentPoint32(DC, str, (int)wcslen(str), &sz);
	return sz.cy;
}

void TNBitmap::TextOutW(int x, int y, wchar_t* str)
{
	ExtTextOut(DC, x, y, 0, NULL, str, (int)wcslen(str), NULL);
}

void TNBitmap::SetBrushColor(int color)
{
	DeleteObject(Brush);
	Brush = CreateSolidBrush(color);
	SelectObject(DC, Brush);
}

void TNBitmap::SetBrushStyle(int style)
{
	//
}

void TNBitmap::SetPenColor(int color)
{
	DeleteObject(Pen);
	Pen = CreatePen(PS_SOLID, 1, color);
	SelectObject(DC, Pen);
}

void TNBitmap::FrameRect(RECT r)
{
	::FrameRect(DC, &r, GetBrush());
}

void TNBitmap::FillRect(RECT r)
{
	::FillRect(DC, &r, GetBrush());
}

HBITMAP TNBitmap::GetBitmapHandle()
{
	return Handle;
}

HBRUSH TNBitmap::GetBrush()
{
	return Brush;
}

HDC TNBitmap::GetDC()
{
	return DC;
}

int TNBitmap::GetWidth()
{
	return Width;
}

int TNBitmap::GetHeight()
{
	return Height;
}


void TNBitmap::MoveTo(int x, int y)
{
	::MoveToEx(DC, x, y, NULL);
}

void TNBitmap::LineTo(int x, int y)
{
	::LineTo(DC, x, y);
}

void TNBitmap::SetFontSize(int sz)
{
	if (Font) DeleteObject(Font);

    LOGFONT lf;     
	ZeroMemory(&lf, sizeof(LOGFONT));     
	lf.lfHeight = sz;    
	lf.lfEscapement = 0;
	lf.lfCharSet = DEFAULT_CHARSET;    
	wcscpy_s(lf.lfFaceName, LF_FACESIZE, L"Courier New");     

	Font = CreateFontIndirect(&lf);
	 
	SelectObject(DC, Font);
}