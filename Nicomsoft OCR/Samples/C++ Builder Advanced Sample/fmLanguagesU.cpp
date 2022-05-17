//---------------------------------------------------------------------------

#include <vcl.h>
#pragma hdrstop

#include "fmLanguagesU.h"

#include "fmMainU.h"

//---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma resource "*.dfm"
TfmLanguages *fmLanguages;
//---------------------------------------------------------------------------
__fastcall TfmLanguages::TfmLanguages(TComponent* Owner)
	: TForm(Owner)
{
}
//---------------------------------------------------------------------------
void __fastcall TfmLanguages::bkCancelClick(TObject *Sender)
{
	Close();
}
//---------------------------------------------------------------------------
void __fastcall TfmLanguages::bkOKClick(TObject *Sender)
{
	if (ApplyLanguages())
		Close();
}
//---------------------------------------------------------------------------

void TfmLanguages::LoadLanguages()
{
	wchar_t val[256], path[256];
	int i;
	for (i = 0; i < cbLanguages->Items->Count; i++)
	{
		wcscpy(path, L"Languages/");
		wcscat(path, cbLanguages->Items->Strings[i].w_str());

		if (fmMain->NsOCR->Cfg_GetOption(fmMain->CfgObj, BT_DEFAULT, path, val, 256) < ERROR_FIRST)
			cbLanguages->Checked[i] = _wtoi(val);
		else
			cbLanguages->Checked[i] = false;
	}
	for (i = 0; i < cbLanguagesAsian->Items->Count; i++)
	{
		wcscpy(path, L"Languages/");
		wcscat(path, cbLanguagesAsian->Items->Strings[i].w_str());

		if (fmMain->NsOCR->Cfg_GetOption(fmMain->CfgObj, BT_DEFAULT, path, val, 256) < ERROR_FIRST)
			cbLanguagesAsian->Checked[i] = _wtoi(val);
		else
			cbLanguagesAsian->Checked[i] = false;
	}
}


bool TfmLanguages::ApplyLanguages()
{
	int i;
	bool ch1 = false;
	for (i = 0; i < cbLanguages->Items->Count; i++)
		if (cbLanguages->Checked[i])
		{
			ch1 = true;
			break;
		}
	int ch2 = 0;
	for (i = 0; i < cbLanguagesAsian->Items->Count; i++)
		if (cbLanguagesAsian->Checked[i])
			ch2++;
	if (!ch1 && !ch2)
	{
		ShowMessage(L"Please select at least one language for recognition.");
		return false;
	}
	if (ch1 && ch2)
	{
		ShowMessage(L"Using both main and asian languages in same zone is not supported.");
		return false;
	}
	if (ch2 > 1)
	{
		ShowMessage(L"Using two or more asian languages in same zone is not supported currently.");
		return false;
	}

	wchar_t path[256];
	for (i = 0; i < cbLanguages->Items->Count; i++)
	{
		wcscpy(path, L"Languages/");
		wcscat(path, cbLanguages->Items->Strings[i].w_str());

		fmMain->NsOCR->Cfg_SetOption(fmMain->CfgObj, BT_DEFAULT, path, cbLanguages->Checked[i] ? L"1" : L"0");
	}
	for (i = 0; i < cbLanguagesAsian->Items->Count; i++)
	{
		wcscpy(path, L"Languages/");
		wcscat(path, cbLanguagesAsian->Items->Strings[i].w_str());

		fmMain->NsOCR->Cfg_SetOption(fmMain->CfgObj, BT_DEFAULT, path, cbLanguagesAsian->Checked[i] ? L"1" : L"0");
	}
	return true;
}

void __fastcall TfmLanguages::FormShow(TObject *Sender)
{
  LoadLanguages();
}
//---------------------------------------------------------------------------
