

#include "OcrConvert.h"
#include "errno.h"

#ifndef _WIN32

//--------------------------------------------------------------------------------------------
bool TUCODEConvert::Init(int SrcCodePage, int DestCodePage)
{
  char scp_name[16] = {0};
  char dcp_name[16] = {0};
  
  if ((int)descriptor > 0) Close();
	  
  SCodePage = SrcCodePage;
  switch (SrcCodePage)
  {
    case C_CP1251: strcpy(scp_name, "CP1251"); break;
    case C_CP1252: strcpy(scp_name, "CP1252"); break;
	case C_UCS2:   strcpy(scp_name, "UCS2");   break;
    case C_WCHAR:  strcpy(scp_name, "WCHAR_T");break;    
    
    default: 
    {
      strcpy(scp_name, "UTF8"); 
      SCodePage = C_UTF8; 
      break;    
    }  
  }
  
  DCodePage = DestCodePage;
  switch (DestCodePage)
  {
    case C_CP1251: strcpy(dcp_name, "CP1251");   break;
    case C_CP1252: strcpy(dcp_name, "CP1252");   break;
	case C_UCS2:   strcpy(dcp_name, "UCS2");     break;
    case C_WCHAR:  strcpy(dcp_name, "WCHAR_T");  break;    

    default: 
    {
      strcpy(dcp_name, "UTF8"); 
      DCodePage = C_UTF8; 
      break;    
    }  
  }
  
  descriptor = iconv_open((const char*) dcp_name, (const char*) scp_name);
  return ((int)descriptor > -1);
}

//--------------------------------------------------------------------------------------------
bool TUCODEConvert::Convert(void* dest, size_t bytes_dest, const void* src, size_t bytes_src)
{
  int i;
  
  if (descriptor < 0) return false;
  
  i = iconv(descriptor, (char**) &src, &bytes_src, (char**) &dest, &bytes_dest);
  
  if (i<0)
  {
	 if (errno == E2BIG) i = 0;
  }
  
  
  return (i >= 0);
}

//--------------------------------------------------------------------------------------------
void TUCODEConvert::Close()
{
	if ((int)descriptor > 0)
	{
		iconv_close(descriptor);
		descriptor = (CHANDLE)-1;
	}
}

#else

bool TUCODEConvert::Init(int SrcCodePage, int DestCodePage)
{
	SCodePage = SrcCodePage;
	if (SCodePage == C_WCHAR) SCodePage = C_UCS2;

    DCodePage = DestCodePage;
	if (DCodePage == C_WCHAR) DCodePage = C_UCS2;

	return true;
}

bool TUCODEConvert::Convert(void* dest, size_t bytes_dest, const void* src, size_t bytes_src)
{
	wchar_t* InputString = (wchar_t*) src;
	bool result = false;

	if (dest == NULL ||  bytes_dest == 0 || src == NULL || bytes_src == 0) return false;

	if (SCodePage != C_UCS2)
	{
		int contentLength;
		contentLength = MultiByteToWideChar(SCodePage, MB_ERR_INVALID_CHARS, (LPCSTR)src, -1, NULL, 0);

	    InputString = (wchar_t*) malloc(contentLength*sizeof(wchar_t));

		if (!MultiByteToWideChar(SCodePage, MB_ERR_INVALID_CHARS, (LPCSTR)src, -1, InputString, contentLength))
		{
			free(InputString);
			return result;
		}
	}

	if (DCodePage == C_UCS2)
	{
		size_t len = (wcslen(InputString)+1)*sizeof(wchar_t);
		len = len > bytes_dest ? bytes_dest : len;

		memcpy(dest, InputString, len);
		result = true;
	}
	else 
	{
		if (WideCharToMultiByte(DCodePage, 0, InputString, -1, (LPSTR)dest, (int)bytes_dest, NULL, NULL))
		  result = true;		
	}

	if (InputString != NULL && InputString != src)
		free(InputString);
	

	return result;
}

void TUCODEConvert::Close()
{
}


#endif
