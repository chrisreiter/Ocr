//---------------------------------------------------------------------------

#include <vcl.h>
#pragma hdrstop

#include "fmScanU.h"
//---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma resource "*.dfm"
TfmScan *fmScan;

#include "fmMainU.h"

//---------------------------------------------------------------------------
__fastcall TfmScan::TfmScan(TComponent* Owner)
	: TForm(Owner)
{
}
//---------------------------------------------------------------------------
void __fastcall TfmScan::FormShow(TObject *Sender)
{
	cbScanners->Items->Clear();

	int res;
	wchar_t buf[1024];

	res = fmMain->NsOCR->Scan_Enumerate(fmMain->ScanObj, buf, 1024, 0);
	if (res > ERROR_FIRST)
	{
		fmMain->ShowError(L"Scan_Enumerate", res);
		return;
	}

//names are separated by comma, parse them
	UNICODECHAR r[100];
	int i, n;
	r[0] = 0;
	for (i = 0; i < (int)wcslen(buf); i++)
	{
		if (buf[i] != ',')
		{
			n = wcslen(r);
			r[n] = buf[i];
			r[n + 1] = 0;
		}
		else
		{
			cbScanners->Items->Add(r);
			r[0] = 0;
		}
	}
	if (r[0])
		cbScanners->Items->Add(r);

//now get default device index
	res = fmMain->NsOCR->Scan_Enumerate(fmMain->ScanObj, NULL, 0, SCAN_GETDEFAULTDEVICE);
	if (res > ERROR_FIRST)
	{
		if (res != ERROR_NODEFAULTDEVICE)
		{
			fmMain->ShowError(L"Scan_Enumerate (1)", res);
			return;
		}
	}
	else
	{
		if (res < cbScanners->Items->Count)
			cbScanners->ItemIndex = res;
	}
}
//---------------------------------------------------------------------------
void __fastcall TfmScan::bkDefaultClick(TObject *Sender)
{
	int res, n;
	n = cbScanners->ItemIndex;
	if (n < 0) return;

	res = fmMain->NsOCR->Scan_Enumerate(fmMain->ScanObj, NULL, 0, SCAN_SETDEFAULTDEVICE | n);
	if (res > ERROR_FIRST)
	{
		fmMain->ShowError(L"Scan_Enumerate (2)", res);
		return;
	}
}
//---------------------------------------------------------------------------
void __fastcall TfmScan::bkScanClick(TObject *Sender)
{
	int n, flags, res;
	n = cbScanners->ItemIndex;
	if (n < 0)
	{
		ShowMessage(L"No scanner selected!");
		return;
	}

	flags = 0;
	switch (cbSrc->ItemIndex)
	{
	case 0: break; //flatbed
	case 1: flags = flags | SCAN_SOURCEADF; break; //adf
	case 2: flags = flags | SCAN_SOURCEAUTO; break; //auto
	}

	if (cbNoUI->Checked)
		flags = flags | SCAN_NOUI;

	res = fmMain->NsOCR->Scan_ScanToImg(fmMain->ScanObj, fmMain->ImgObj, n, flags);
	//  res = fmMain->NsOCR->Scan_ScanToFile(fmMain->ScanObj, L"c:\\temp.tif", n, flags);

	if (res > ERROR_FIRST)
	{
		fmMain->ShowError(L"Scan_ScanToImg", res);
		return;
	}

	Close();
	fmMain->DoImageLoaded();
}
//---------------------------------------------------------------------------
