//---------------------------------------------------------------------------

#include <vcl.h>
#pragma hdrstop
#include <tchar.h>
//---------------------------------------------------------------------------





USEFORM("fmMainU.cpp", fmMain);
USEFORM("fmScanU.cpp", fmScan);
USEFORM("fmWaitU.cpp", fmWait);
USEFORM("fmLanguagesU.cpp", fmLanguages);
//---------------------------------------------------------------------------
WINAPI _tWinMain(HINSTANCE, HINSTANCE, LPTSTR, int)
{
	try
	{
		Application->Initialize();
		Application->MainFormOnTaskBar = true;
		Application->Title = "Nicomsoft OCR C++ Builder Sample";
		Application->CreateForm(__classid(TfmMain), &fmMain);
		Application->CreateForm(__classid(TfmScan), &fmScan);
		Application->CreateForm(__classid(TfmWait), &fmWait);
		Application->CreateForm(__classid(TfmLanguages), &fmLanguages);
		Application->Run();
	}
	catch (Exception &exception)
	{
		Application->ShowException(&exception);
	}
	catch (...)
	{
		try
		{
			throw Exception("");
		}
		catch (Exception &exception)
		{
			Application->ShowException(&exception);
		}
	}
	return 0;
}
//---------------------------------------------------------------------------
