
#ifndef __DLLMAINH__
#define __DLLMAINH__

#include "NSOCR.h"

extern TNSOCR  *OCRObject;
extern int      Loaded;

bool InitLib();
bool FreeLib();

#endif
