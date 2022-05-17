#ifndef __DEBUGH__
#define __DEBUGH__

#ifdef _WIN32 
#include "Windows.h"
#endif

#include "NSOCR.h"

#ifdef  _DEBUG
#ifdef _WIN32
#define _ENABLELOG_
#endif
#endif


#ifdef _ENABLELOG_
extern UNICODECHAR DebugLogFile[256];

void ClearDebugLog();

#ifdef _WIN32
	void DBGSTR(UNICODECHAR* fmt, ...);
#else        
    #define DBGSTR(...);
#endif        
#else
    #define DBGSTR //__noop
#endif



#endif
