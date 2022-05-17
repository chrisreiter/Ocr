//******************************************************************************
//                           TNBitmap Class
//                    Copyright (c) 2010-2011 Nicomsoft
//                 Copyright (c) 2010-2011 Michael Kapustin
//******************************************************************************


#ifndef _NBitmapH_
#define _NBitmapH_

//#include "sdkddkver.h"
//#include "afxwin.h"
#include "windows.h"


const int Npf24bit = 3;

const int NbsSolid = 0;
const int NbsClear  = 1;


const int NclRed = 0x0000FF;
const int NclBlack = 0x00;
const int NclWhite = 0xFFFFFF;
const int NclBlue = 0xFF0000;
const int NclGreen = 0x008000;
const int NclLime = 0x00FF00;
const int NclTeal = 0x808000;
const int NclGray = 0x808080;
const int NclAqua = 0xFFFF00;
const int NclNavy = 0x800000;
const int NclOlive = 0x008080;

class TNBitmap
{
private:
	HBITMAP Handle;
	HBRUSH Brush;
	HPEN Pen;
	HFONT Font;
	HDC DC;
	int Width;
	int Height;

	BITMAPINFO bmpHeader;
	LPBYTE pBmpData;
public:
	TNBitmap();
	~TNBitmap();

	int GetWidth();
	int GetHeight();

	void SetSize(int w, int h);
	void SetPixelFormat(int pf);
	void* ScanLine(int y);

	void Draw(int x, int y, TNBitmap* bmp);
	void StretchDraw(RECT r, TNBitmap* bmp);
	int TextHeightW(wchar_t* str);
	int TextWidthW(wchar_t* str);
	void TextOutW(int x, int y, wchar_t* str);

	void FillRect(RECT r);
	void FrameRect(RECT r);
	void SetBrushColor(int color);
	void SetBrushStyle(int style);
	void SetPenColor(int color);

	void MoveTo(int x, int y);
	void LineTo(int x, int y);

	HBITMAP GetBitmapHandle();
	HBRUSH GetBrush();
	HDC GetDC();

	void SetFontSize(int sz);
};


#endif //_NBitmapH_
