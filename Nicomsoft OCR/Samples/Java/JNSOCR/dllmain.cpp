// dllmain.cpp : Defines the entry point for the DLL application.

#ifdef _WIN32
#include <windows.h>
#else
#include "BaseTypes.h"
#include <cstddef>
//#include <stdio.h>
#include "stdlib.h"
#include "locale.h"
#include "wchar.h"
#include <dlfcn.h>
#include "string.h"
#endif

#include "JNSOCR.h"
#include "targetver.h"
#include "NSOCR.h"
#include "debug.h"
#include "PUGIXML/pugixml.hpp"

#define VAR_NAME         L"JNSOCR"

#ifdef _WIN32

#ifndef _WIN64
  #define CONFIG_FILE      L"\\jnsocr_win.config"
  #define DEFAULT_DLL_NAME L"\\NSOCR.dll"
  #define DEFAULT_LIB_PATH L".\\NSOCR"
#else
  #define CONFIG_FILE      L"\\jnsocr_win64.config"
  #define DEFAULT_DLL_NAME L"\\NSOCR.dll"
  #define DEFAULT_LIB_PATH L".\\NSOCR"
#endif

#else

#ifndef __x86_64__
#define CONFIG_FILE      L"/jnsocr_linux.config"
#define DEFAULT_DLL_NAME L"/libNSOCR.so"
#define DEFAULT_LIB_PATH L"./NSOCR"
#else
#define CONFIG_FILE      L"/jnsocr_linux64.config"
#define DEFAULT_DLL_NAME L"/libNSOCR.so"
#define DEFAULT_LIB_PATH L"./NSOCR"
#endif

#endif

TNSOCR  *OCRObject = NULL;
int      Loaded = 0;

wchar_t DLL_NAME[256] = {L'\0'};
wchar_t LIB_PATH[256] = {L'\0'};

#ifndef _WIN32
void OutLog(char* str)
{
        FILE *f = fopen("/home/now/log.txt","a+");
        fprintf(f, "%s\n\r", str);
        fclose(f);       
}

FILE* _wfopen(const wchar_t* filename, const wchar_t* mode)
{
    char fname[512];
    char attr[8];    
      
    setlocale(LC_ALL, "");
    wcsrtombs(fname, (const wchar_t **)&filename, sizeof(fname),  NULL);	
    wcsrtombs(attr, (const wchar_t **)&mode, sizeof(attr), NULL);	
	
    return fopen((const char*)fname, (const char*)attr);
}
#endif

bool isFileExist(const wchar_t* filename)
{
	FILE* f;
	f = _wfopen(filename, L"r");
	if (f)
	{
		fclose(f);
		return true;
	}
	return false;
}

//-----------------------------------------------------------------------------
bool ReadEnverVar(const wchar_t* Name, wchar_t* Value, int size)
{
#ifdef _WIN32
	 return GetEnvironmentVariable(Name, Value, size) != 0;
#else
	setlocale(LC_ALL, "");    
       
        int len = sizeof(VAR_NAME);
        char* varname = (char*) malloc(len); 
        
        wcstombs (varname, VAR_NAME, len);
        
	char* var = getenv (varname);
        
        if (var)
        {                
            mbstowcs (Value, var, size);        
        }
        free(varname);
        
        return var != NULL;
#endif
}


//-----------------------------------------------------------------------------
bool InitLib()
{
    if (!Loaded)
	{
		wchar_t cfg_path[512];
		wchar_t cfg_file[512];

		cfg_path[0] = 0;
#ifdef _WIN32
		//new: by default, jnsocr.dll is located in Bin folder, check this folder firstly
		HMODULE h = GetModuleHandle(L"jnsocr.dll");
		if (h)
		{
			GetModuleFileName(h, cfg_path, 510);
			wchar_t* pos = wcsrchr(cfg_path, L'\\');
			if (pos)
			{
				*pos = L'\0';
				swprintf(DLL_NAME, sizeof(DLL_NAME)/sizeof(wchar_t)-1, L"%ls%ls", cfg_path, DEFAULT_DLL_NAME);
				if (!isFileExist(DLL_NAME))
				{
					DBGSTR(L"same folder: dll not found: %ls", DLL_NAME);
					cfg_path[0] = 0;
				}
			}
		}
#else
	wchar_t fname_ws[512];
    wchar_t fpath_ws[512];
	char fpath[512];
        
    Dl_info info;
        
    if (dladdr((void*)InitLib, &info))
	{        
		mbstowcs(fpath_ws, info.dli_fname, sizeof(fpath_ws)/sizeof(fpath_ws[0]));        

		wchar_t* pos = wcsrchr(fpath_ws, L'/');

		if (pos)
		{
			*pos = L'\0';

			wcscpy(fname_ws, fpath_ws);
			wcscat(fname_ws, DEFAULT_DLL_NAME);

			if (isFileExist(fname_ws))
				wcscpy(cfg_path, fpath_ws);
		}
	}

#endif
////////
	if (!cfg_path[0])
	{
		DBGSTR(L"Read JNSOCR var!");
		if (!ReadEnverVar(VAR_NAME, cfg_path, sizeof(cfg_path)/sizeof(wchar_t))) 
		{
			swprintf(cfg_path, sizeof(cfg_path)/sizeof(wchar_t)-1, L".");
		}
	}
	DBGSTR(L"NSOCR Bin dir: %ls", cfg_path);

	swprintf(DLL_NAME, sizeof(DLL_NAME)/sizeof(wchar_t)-1, L"%ls%ls", cfg_path, DEFAULT_DLL_NAME);                

	if (!isFileExist(DLL_NAME))
	{
		DBGSTR(L"dll not found: %ls\\nsocr.dll", cfg_path);
		swprintf(cfg_path, sizeof(cfg_path)/sizeof(wchar_t)-1, DEFAULT_LIB_PATH);
	}

	DBGSTR(L"NSOCR Bin dir: %ls", cfg_path);

	swprintf(DLL_NAME, sizeof(DLL_NAME)/sizeof(wchar_t)-1, L"%ls%ls", cfg_path, DEFAULT_DLL_NAME);                
	swprintf(cfg_file, sizeof(cfg_file)/sizeof(wchar_t)-1, L"%ls%ls", cfg_path, CONFIG_FILE);
    
    swprintf(LIB_PATH, sizeof(LIB_PATH)/sizeof(wchar_t)-1, L"%ls", cfg_path);                
                
	DBGSTR(L"NSOCR dll name: %ls", DLL_NAME);
	DBGSTR(L"NSOCR lib path: %ls", LIB_PATH);		

	//#pragma comment(lib, "User32.lib")
	//MessageBox(0, DLL_NAME, NULL, 0);

	OCRObject = new TNSOCR(DLL_NAME);
	DBGSTR(L"OCRObject: %d", OCRObject);          

	if (OCRObject)
	{
		if (!OCRObject->IsDllLoaded())
		{
			DBGSTR(L"DLL not loaded!");
			delete OCRObject;

			OCRObject = NULL;
			return false;
		}

		DBGSTR(L"DLL loaded successfully");
                        
		//OCRObject->Engine_SetDataDirectory(LIB_PATH);
		//OCRObject->Engine_Initialize();
		//OCRObject->Engine_SetDataDirectory(LIB_PATH);
	}
	else 
		return false;	

		Loaded++;
	}
	return true;
}

//-----------------------------------------------------------------------------
bool FreeLib()
{
	if (Loaded) Loaded--;

	if (!Loaded)
	{
		if (OCRObject)
		{
			//OCRObject->Engine_Uninitialize();
			delete OCRObject;

			OCRObject = NULL;
		}
	}

	return true;
}

//-----------------------------------------------------------------------------
#ifdef _WIN32 

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
		case DLL_PROCESS_ATTACH:
		{
			//printf("Process Attach\n\r");
			InitLib();
			break;
		}
		case DLL_PROCESS_DETACH:
		{
			//printf("Process Detach\n\r");
			FreeLib();
			break;
		}
		break;
	}
	return TRUE;
}

#else
// Called when loading libraries
__attribute__((constructor)) static void initialize_lib() 
{  
    InitLib();
}

// Called when unloaing library
__attribute__((destructor)) static void destroy_lib() 
{
    FreeLib();      
}

// For debug Linux only
//int main (int count, char** arg)
//{
//}

#endif
