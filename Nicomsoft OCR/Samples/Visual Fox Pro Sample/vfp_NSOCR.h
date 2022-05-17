&&A VFP9 wrapper class for NSOCR

&&******************************************************************************
&&                        Nicomsoft OCR DLL interface
&&                    Copyright (c) 2010-2014 Nicomsoft
&&                 Copyright (c) 2010-2014 Michael Kapustin
&&                           www.nsocr.com
&&******************************************************************************

&& error codes
#DEFINE ERROR_FIRST              0x70000000
#DEFINE ERROR_FILENOTFOUND       0x70000001
#DEFINE ERROR_LOADFILE           0x70000002
#DEFINE ERROR_SAVEFILE           0x70000003
#DEFINE ERROR_MISSEDIMGLOADER    0x70000004
#DEFINE ERROR_OPTIONNOTFOUND     0x70000005
#DEFINE ERROR_NOBLOCKS           0x70000006
#DEFINE ERROR_BLOCKNOTFOUND      0x70000007
#DEFINE ERROR_INVALIDINDEX       0x70000008
#DEFINE ERROR_INVALIDPARAMETER   0x70000009
#DEFINE ERROR_FAILED             0x7000000A
#DEFINE ERROR_INVALIDBLOCKTYPE   0x7000000B
#DEFINE ERROR_EMPTYTEXT          0x7000000D
#DEFINE ERROR_LOADINGDICTIONARY  0x7000000E
#DEFINE ERROR_LOADCHARBASE       0x7000000F
#DEFINE ERROR_NOMEMORY           0x70000010
#DEFINE ERROR_CANNOTLOADGS       0x70000011
#DEFINE ERROR_CANNOTPROCESSPDF   0x70000012
#DEFINE ERROR_NOIMAGE            0x70000013
#DEFINE ERROR_MISSEDSTEP         0x70000014
#DEFINE ERROR_OUTOFIMAGE         0x70000015
#DEFINE ERROR_EXCEPTION          0x70000016
#DEFINE ERROR_NOTALLOWED         0x70000017
#DEFINE ERROR_NODEFAULTDEVICE    0x70000018
#DEFINE ERROR_NOTAPPLICABLE      0x70000019
#DEFINE ERROR_MISSEDBARCODEDLL   0x7000001A
#DEFINE ERROR_PENDING            0x7000001B
#DEFINE ERROR_OPERATIONCANCELLED 0x7000001C

#DEFINE ERROR_INVALIDOBJECT      0x70010000
#DEFINE ERROR_TOOMANYOBJECTS     0x70010001
#DEFINE ERROR_DLLNOTLOADED       0x70010002
#DEFINE ERROR_DEMO               0x70010003

&& block types
#DEFINE BT_DEFAULT               0
#DEFINE BT_OCRTEXT               1
#DEFINE BT_ICRDIGIT              2
#DEFINE BT_CLEAR                 3
#DEFINE BT_PICTURE               4
#DEFINE BT_ZONING                5
#DEFINE BT_OCRDIGIT              6
#DEFINE BT_BARCODE               7
#DEFINE BT_TABLE                 8

&& Constants for Img_LoadBmpData function
#DEFINE BMP_24BIT                0x0
#DEFINE BMP_8BIT                 0x1
#DEFINE BMP_1BIT                 0x2
#DEFINE BMP_32BIT                0x3
#DEFINE BMP_BOTTOMTOP            0x100

&& Constants for Img_GetImgText, Blk_GetText and Svr_AddPage functions
#DEFINE FMT_EDITCOPY             0x0
#DEFINE FMT_EXACTCOPY            0x1

&& for Img_OCR
#DEFINE OCRSTEP_FIRST            0x0  && first step
#DEFINE OCRSTEP_PREFILTERS       0x10 && filters before binarizing: scaling, invert, rotate etc
#DEFINE OCRSTEP_BINARIZE         0x20 && binarize
#DEFINE OCRSTEP_POSTFILTERS      0x50 && bin garbage filter etc
#DEFINE OCRSTEP_REMOVELINES      0x60 && find and remove lines
#DEFINE OCRSTEP_ZONING           0x70 && analyze page layout
#DEFINE OCRSTEP_OCR              0x80 && ocr itself
#DEFINE OCRSTEP_LAST             0xFF && last step

&& for Img_OCR, "Flags" parameter
#DEFINE OCRFLAG_NONE             0x0 && default mode (blocking)
#DEFINE OCRFLAG_THREAD           0x1 && non-blocking mode
#DEFINE OCRFLAG_GETRESULT        0x2 && get result for non-blocking mode
#DEFINE OCRFLAG_GETPROGRESS      0x3 && get progress
#DEFINE OCRFLAG_CANCEL           0x4 && cancel ocr

&& for Img_DrawToDC and Img_GetBmpData
#DEFINE DRAW_NORMAL              0x0
#DEFINE DRAW_BINARY              0x1
#DEFINE DRAW_GETBPP              0x100

&& values for Blk_Inversion function
#DEFINE BLK_INVERSE_GET         -1
#DEFINE BLK_INVERSE_SET0         0
#DEFINE BLK_INVERSE_SET1         1
#DEFINE BLK_INVERSE_DETECT       0x100

&& for Blk_Rotation function
#DEFINE BLK_ROTATE_GET          -1
#DEFINE BLK_ROTATE_NONE          0
#DEFINE BLK_ROTATE_90            0x1
#DEFINE BLK_ROTATE_180           0x2
#DEFINE BLK_ROTATE_270           0x3
#DEFINE BLK_ROTATE_ANGLE         0x100000

&& for Blk_Mirror function
#DEFINE BLK_MIRROR_GET          -1
#DEFINE BLK_MIRROR_H             0x1
#DEFINE BLK_MIRROR_V             0x2

&& for Svr_Create function
#DEFINE SVR_FORMAT_PDF           0x1
#DEFINE SVR_FORMAT_RTF           0x2
#DEFINE SVR_FORMAT_TXT_ASCII     0x3
#DEFINE SVR_FORMAT_TXT_UNICODE   0x4

&& for Scan_Enumerate function
#DEFINE SCAN_GETDEFAULTDEVICE    0x1
#DEFINE SCAN_SETDEFAULTDEVICE    0x100

&& for Scan_ScanToImg and Scan_ScanToFile functions
#DEFINE SCAN_NOUI                0x1
#DEFINE SCAN_SOURCEADF           0x2
#DEFINE SCAN_SOURCEAUTO          0x4
#DEFINE SCAN_DONTCLOSEDS         0x8

&& for Blk_GetWordFontStyle function
#DEFINE FONT_STYLE_UNDERLINED    0x1
#DEFINE FONT_STYLE_STRIKED       0x2
#DEFINE FONT_STYLE_BOLD          0x4
#DEFINE FONT_STYLE_ITALIC        0x8

&& for Img_GetProperty function, PropertyID parameter
#DEFINE IMG_PROP_DPIX            0x1   && original DPI
#DEFINE IMG_PROP_DPIY            0x2
#DEFINE IMG_PROP_BPP             0x3   && color depth
#DEFINE IMG_PROP_WIDTH           0x4   && original width
#DEFINE IMG_PROP_HEIGHT          0x5   && original height
#DEFINE IMG_PROP_INVERTED        0x6   && image inversion flag after OCR_STEP_PREFILTERS step
#DEFINE IMG_PROP_SKEW            0x7   && image skew angle after OCR_STEP_PREFILTERS step, multiplied by 1000
#DEFINE IMG_PROP_SCALE           0x8   && image scale factor after OCR_STEP_PREFILTERS step, multiplied by 1000

&& for "Blk_SetWordRegEx" function
#DEFINE REGEX_SET                0x0
#DEFINE REGEX_CLEAR              0x1
#DEFINE REGEX_CLEAR_ALL          0x2
#DEFINE REGEX_DISABLE_DICT       0x4

&& for Svr_SetDocumentInfo function
#DEFINE INFO_PDF_AUTHOR          0x1
#DEFINE INFO_PDF_CREATOR         0x2
#DEFINE INFO_PDF_PRODUCER        0x3
#DEFINE INFO_PDF_TITLE           0x4
#DEFINE INFO_PDF_SUBJECT         0x5
#DEFINE INFO_PDF_KEYWORDS        0x6

&& for "BarCode/TypesMask" configuration option
#define BARCODE_TYPE_MASK_EAN8     0x01
#define BARCODE_TYPE_MASK_UPCE     0x02
#define BARCODE_TYPE_MASK_ISBN10   0x04
#define BARCODE_TYPE_MASK_UPCA     0x08
#define BARCODE_TYPE_MASK_EAN13    0x10
#define BARCODE_TYPE_MASK_ISBN13   0x20
#define BARCODE_TYPE_MASK_ZBAR_I25 0x40
#define BARCODE_TYPE_MASK_CODE39   0x80
#define BARCODE_TYPE_MASK_QRCODE   0x100
#define BARCODE_TYPE_MASK_CODE128  0x200

