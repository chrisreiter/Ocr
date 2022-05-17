#ifndef __MY_TYPES__
#define __MY_TYPES__


#ifndef MACHINEWORD
#define MACHINEWORD 32
#endif

// LOGIC
#ifndef BOOL
//#define BOOL   unsigned int
typedef int BOOL;
#endif

#ifndef TRUE
#define TRUE   1
#endif

#ifndef FALSE
#define FALSE  0
#endif

#ifndef NULL
#define NULL   ((void*)0x0000)
#endif

#ifndef UINT
//#define UINT   unsigned int
typedef unsigned int UINT;
#endif
// 8 bit
#ifndef UCHAR
typedef unsigned char UCHAR;
//#define UCHAR  unsigned char
#endif

#ifndef CHAR
//#define CHAR   char
typedef char CHAR;
#endif

#ifndef BYTE
//#define BYTE   unsigned char
typedef unsigned char BYTE;
#endif

#ifndef Int8
//#define Int8   char
typedef char Int8;
#endif

#ifndef uInt8
//#define uInt8  unsigned char
typedef unsigned char uInt8;
#endif

// 16 bit
#if MACHINEWORD == 16

#ifndef MAXUINT
#define MAXUINT 0xFFFF
#endif

#ifndef WORD
#define WORD   unsigned int
#endif

#ifndef Int16
#define Int16  int
#endif

#ifndef uInt16
#define uInt16 unsigned int
#endif

#else  //MACHINEWORD == 32

#ifndef MAXUINT
#define MAXUINT 0xFFFFFFFF
#endif

#ifndef WORD
//#define WORD   unsigned short int
typedef unsigned short int WORD;
#endif

#ifndef Int16
//#define Int16  short int
typedef short int Int16;
#endif

#ifndef uInt16
//#define uInt16 unsigned short int
typedef unsigned short int uInt16;
#endif

#endif

// 32 bit
#ifndef LONG
//#define LONG   long
typedef long LONG;
#endif

#ifndef ULONG
//#define ULONG  unsigned long
typedef unsigned long ULONG;
#endif

#ifndef Int32
//#define Int32  long
typedef long Int32;
#endif

#ifndef uInt32
//#define uInt32 unsigned long
typedef unsigned long uInt32;
#endif

#ifndef DWORD
//#define DWORD  unsigned long
typedef unsigned long DWORD;

// 64 bit
#if MACHINEWORD == 32
#ifndef Int64
//#define Int64  long long
typedef long long Int64;
#endif


#ifndef INT64
//#define INT64  long long
typedef long long INT64;
#endif

#ifndef uInt64
//#define uInt64 unsigned long long
typedef unsigned long long uInt64;
#endif

#ifndef UINT64
//#define UINT64 unsigned long long
typedef unsigned long long UINT64;
#endif

typedef union { // ��������� int(64 bit) �� bit
 uInt64 Bit64;
 uInt32 Bit32[2];
 uInt16 Bit16[4];
 uInt8  Bit8[8];
 struct  {
  char b0 : 1;
  char b1 : 1;
  char b2 : 1;
  char b3 : 1;
  char b4 : 1;
  char b5 : 1;
  char b6 : 1;
  char b7 : 1;
  char b8 : 1;
  char b9 : 1;
  char b10: 1;
  char b11: 1;
  char b12: 1;
  char b13: 1;
  char b14: 1;
  char b15: 1;
  char b16: 1;
  char b17: 1;
  char b18: 1;
  char b19: 1;
  char b20: 1;
  char b21: 1;
  char b22: 1;
  char b23: 1;
  char b24: 1;
  char b25: 1;
  char b26: 1;
  char b27: 1;
  char b28: 1;
  char b29: 1;
  char b30: 1;
  char b31: 1;
  char b32: 1;
  char b33: 1;
  char b34: 1;
  char b35: 1;
  char b36: 1;
  char b37: 1;
  char b38: 1;
  char b39: 1;
  char b40: 1;
  char b41: 1;
  char b42: 1;
  char b43: 1;
  char b44: 1;
  char b45: 1;
  char b46: 1;
  char b47: 1;
  char b48: 1;
  char b49: 1;
  char b50: 1;
  char b51: 1;
  char b52: 1;
  char b53: 1;
  char b54: 1;
  char b55: 1;
  char b56: 1;
  char b57: 1;
  char b58: 1;
  char b59: 1;
  char b60: 1;
  char b61: 1;
  char b62: 1;
  char b63: 1;
 }Bit;
} tBitInt64;
#endif
#endif
typedef union { // ��������� int(32 bit) �� bit
 uInt32 Bit32;
 uInt16 Bit16[2];
 uInt8  Bit8[4];
 struct  {
  char b0 : 1;
  char b1 : 1;
  char b2 : 1;
  char b3 : 1;
  char b4 : 1;
  char b5 : 1;
  char b6 : 1;
  char b7 : 1;
  char b8 : 1;
  char b9 : 1;
  char b10: 1;
  char b11: 1;
  char b12: 1;
  char b13: 1;
  char b14: 1;
  char b15: 1;
  char b16: 1;
  char b17: 1;
  char b18: 1;
  char b19: 1;
  char b20: 1;
  char b21: 1;
  char b22: 1;
  char b23: 1;
  char b24: 1;
  char b25: 1;
  char b26: 1;
  char b27: 1;
  char b28: 1;
  char b29: 1;
  char b30: 1;
  char b31: 1;
 }Bit;
} tBitLong;
#define tBitInt32 tBitLong

typedef union{ // ��������� int(16 bit) �� bit
 uInt16 Bit16;
 uInt8  Bit8[2];
 struct {
  char b0 : 1;
  char b1 : 1;
  char b2 : 1;
  char b3 : 1;
  char b4 : 1;
  char b5 : 1;
  char b6 : 1;
  char b7 : 1;
  char b8 : 1;
  char b9 : 1;
  char b10: 1;
  char b11: 1;
  char b12: 1;
  char b13: 1;
  char b14: 1;
  char b15: 1;
 }Bit;
} tBitWord;
#define tBitInt16 tBitWord

typedef union{ // ��������� byte(8 bit) �� bit
 uInt8  Bit8;
 struct {
  char b0 : 1;
  char b1 : 1;
  char b2 : 1;
  char b3 : 1;
  char b4 : 1;
  char b5 : 1;
  char b6 : 1;
  char b7 : 1;
 }Bit;
} tBitByte;
#define tBitInt8  tBitByte

//-------------------------------------------------------
// For Windows code compatibility
//-------------------------------------------------------
#ifndef _WIN32

typedef uInt64 LONGLONG;

typedef union _LARGE_INTEGER 
{
  struct 
  {
    DWORD LowPart;
    LONG  HighPart;
  };
  struct 
  {
    DWORD LowPart;
    LONG  HighPart;
  } u;
  LONGLONG QuadPart;
} LARGE_INTEGER, *PLARGE_INTEGER; 

typedef struct tagBITMAPFILEHEADER 
{
  WORD  bfType;
  DWORD bfSize;
  WORD  bfReserved1;
  WORD  bfReserved2;
  DWORD bfOffBits;
} BITMAPFILEHEADER, *PBITMAPFILEHEADER;

typedef struct tagBITMAPINFOHEADER
{
  DWORD biSize;
  LONG  biWidth; 
  LONG  biHeight; 
  WORD  biPlanes; 
  WORD  biBitCount;
  DWORD biCompression; 
  DWORD biSizeImage; 
  LONG  biXPelsPerMeter; 
  LONG  biYPelsPerMeter; 
  DWORD biClrUsed; 
  DWORD biClrImportant;
} BITMAPINFOHEADER, *PBITMAPINFOHEADER; 

#define COLORORDER_RGB 0
#define COLORORDER_BGR 1

#ifndef COLORORDER
#define COLORORDER COLORORDER_RGB
#endif

typedef struct tagRGBQUAD 
{
#if COLORORDER == COLORORDER_BGR
  BYTE rgbBlue;
  BYTE rgbGreen;
  BYTE rgbRed;
#else
  BYTE rgbRed;
  BYTE rgbGreen;
  BYTE rgbBlue;
#endif // FREEIMAGE_COLORORDER
  BYTE rgbReserved;
} RGBQUAD;

typedef struct tagBITMAPINFO 
{ 
  BITMAPINFOHEADER bmiHeader; 
  RGBQUAD          bmiColors[1];
} BITMAPINFO, *PBITMAPINFO;

#endif
#endif
