#include "debug.h"

#include "wchar.h"
#include "stdlib.h"
#include "stdio.h"

#ifdef _WIN32
UNICODECHAR DebugLogFile[256] = L"c:\\JNSOCR-TraceLog.txt";
#else
UNICODECHAR DebugLogFile[256] = L"~/JNSOCR-TraceLog.txt";
#endif

#ifdef _ENABLELOG_
void ClearDebugLog()
{
#ifndef _WIN32    
    return;
#else    
	if (!DebugLogFile[0]) return;
	DeleteFile(DebugLogFile);
#endif        
}


void DBGSTR(UNICODECHAR* fmt, ...)
{
	if (!DebugLogFile[0]) return; 

	//   return;

	va_list args;
	UNICODECHAR buf[1024], OutBuffer[4096];
	va_start(args, fmt);
	_vswprintf(buf, fmt, args);
	va_end(args);
	//wcscat(buf, "\n");

	SYSTEMTIME time;

	GetLocalTime(&time);
	swprintf(OutBuffer, L"%.2d:%.2d:%.2d:%.3d\t%s\r\n",
			time.wHour,
			time.wMinute,
			time.wSecond,
			time.wMilliseconds,
			buf); 

	// OutputDebugString(buf); 

	DWORD wr;
	HANDLE f;
	f = CreateFile(DebugLogFile, GENERIC_WRITE, FILE_SHARE_READ, NULL, OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0);
	if (f != INVALID_HANDLE_VALUE)
	{
		SetFilePointer(f, 0, 0, FILE_END);
		WriteFile(f, OutBuffer, (DWORD)(2 * wcslen(OutBuffer)), &wr, NULL);
		CloseHandle(f);
	}
}
#endif 
