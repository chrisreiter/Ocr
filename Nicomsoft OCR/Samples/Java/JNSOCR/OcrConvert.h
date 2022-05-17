
#ifndef _OCRCOVERTH_
#define _OCRCOVERTH_

#ifdef _WIN32
#include "windows.h"
#endif

#include "BaseTypes.h"
#include "string.h"
#include "wchar.h"


#define C_CP1251		1251
#define C_CP1252		1252

#define C_UCS2			16
#define C_WCHAR			32
#define C_UTF8			65001


#ifdef _WIN32

#define  CHANDLE void*

#else

#include "iconv.h"
#define  CHANDLE iconv_t
#define  CP_ACP         C_UTF8
#endif

class TUCODEConvert
{
private:
  CHANDLE descriptor;
  int	  SCodePage;
  int	  DCodePage;
  
public:
  TUCODEConvert()
  {
    descriptor = (void*)-1;
    SCodePage  = -1;
    DCodePage  = -1;
  }
  
  ~TUCODEConvert()
  {
    Close();
  }
  
  bool Init(int SrcCodePage, int DestCodePage);
  bool Convert(void* dest, size_t bytes_dest, const void* src, size_t bytes_src);
  void Close();
};

#endif
