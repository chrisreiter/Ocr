//A Delphi wrapper class for NSOCR.dll functions

//******************************************************************************
//                        Nicomsoft OCR DLL interface
//                    Copyright (c) 2010-2021 Nicomsoft
//                 Copyright (c) 2010-2021 Mikhail Kapustin
//                           www.nsocr.com
//******************************************************************************

unit NSOCR;

                            interface

uses Classes, Windows;

////////////////////////////////////////

//Error codes
const
  ERROR_FIRST					      = $70000000;
  ERROR_FILENOTFOUND 			  = $70000001;
  ERROR_LOADFILE 				    = $70000002;
  ERROR_SAVEFILE 				    = $70000003;
  ERROR_MISSEDIMGLOADER 		= $70000004;
  ERROR_OPTIONNOTFOUND		  = $70000005;
  ERROR_NOBLOCKS			    	= $70000006;
  ERROR_BLOCKNOTFOUND			  = $70000007;
  ERROR_INVALIDINDEX			  = $70000008;
  ERROR_INVALIDPARAMETER		= $70000009;
  ERROR_FAILED				      = $7000000A;
  ERROR_INVALIDBLOCKTYPE		= $7000000B;
  ERROR_EMPTYTEXT				    = $7000000D;
  ERROR_LOADINGDICTIONARY		= $7000000E;
  ERROR_LOADCHARBASE			  = $7000000F;
  ERROR_NOMEMORY			    	= $70000010;
  ERROR_CANNOTLOADGS		  	= $70000011;
  ERROR_CANNOTPROCESSPDF		= $70000012;
  ERROR_NOIMAGE				      = $70000013;
  ERROR_MISSEDSTEP          = $70000014;
  ERROR_OUTOFIMAGE          = $70000015;
  ERROR_EXCEPTION           = $70000016;
  ERROR_NOTALLOWED          = $70000017;
  ERROR_NODEFAULTDEVICE     = $70000018;
  ERROR_NOTAPPLICABLE			  = $70000019;
  ERROR_MISSEDBARCODEDLL 		= $7000001A;
  ERROR_PENDING     	    	= $7000001B;
  ERROR_OPERATIONCANCELLED	= $7000001C;
  ERROR_TOOMANYLANGUAGES	= $7000001D;
  ERROR_OPERATIONTIMEOUT	= $7000001E;
  ERROR_LOAD_ASIAN_MODULE	= $7000001F;	
  ERROR_LOAD_ASIAN_LANG		= $70000020;
  ERROR_INVALIDOBJECT		  	= $70010000;
  ERROR_TOOMANYOBJECTS	  	= $70010001;
  ERROR_DLLNOTLOADED		  	= $70010002;
  ERROR_DEMO					      = $70010003;


//Block types
const
  BT_DEFAULT = 0; //only for Cfg_GetOpton and Cfg_SetOption, don't use with Blk_SetType
  BT_OCRTEXT = 1; //machine-printed text
  BT_ICRDIGIT = 2; //handwritten digits
  BT_CLEAR = 3; //block type to clear some area on image before ocr
  BT_PICTURE = 4; //picture
  BT_ZONING = 5; //block type to mark some area for zoning
  BT_OCRDIGIT = 6; //machine-printed digits
  BT_BARCODE = 7; //bar code
  BT_TABLE = 8; //table
  BT_MRZ = 9; //MRZ

//constants for Img_LoadBmpData
  BMP_24BIT		    = $00;
  BMP_8BIT		    = $01;
  BMP_1BIT		    = $02;
  BMP_32BIT		    = $03;
  BMP_BOTTOMTOP	  = $100;

//constantsfor Img_GetImgText, Blk_GetText and Svr_AddPage functions
  FMT_EDITCOPY = $00;
  FMT_EXACTCOPY	= $01;

//for Img_OCR
  OCRSTEP_FIRST         = $00; //first step
  OCRSTEP_PREFILTERS		= $10; //filters before binarizing: scaling, invert, rotate etc. <ImgAlizer> section in Config file
  OCRSTEP_BINARIZE	  	= $20; //binarize. <Binarizer> section in Config file
  OCRSTEP_POSTFILTERS		= $50; //bin garbage filter etc
  OCRSTEP_REMOVELINES		= $60; //find and remove lines
  OCRSTEP_ZONING        = $70; //analyze page layout
  OCRSTEP_OCR			    	= $80; //ocr itself
  OCRSTEP_LAST		    	= $FF; //last step

//for Img_OCR, "Flags" parameter
  OCRFLAG_NONE			    = $00; //default mode (blocking)
  OCRFLAG_THREAD		  	= $01; //non-blocking mode
  OCRFLAG_GETRESULT	   	= $02; //get result for non-blocking mode
  OCRFLAG_GETPROGRESS		= $03; //get progress
  OCRFLAG_CANCEL			  = $04; //cancel ocr

//for Img_DrawToDC and Img_GetBmpData
  DRAW_NORMAL           = $00;
  DRAW_BINARY           = $01;
  DRAW_GETBPP           = $100;

//values for Blk_Inversion function
  BLK_INVERSE_GET      = -1;
  BLK_INVERSE_SET0     = 0;
  BLK_INVERSE_SET1     = 1;
  BLK_INVERSE_DETECT   = $100;

//for Blk_Rotation function
  BLK_ROTATE_GET       = -1;
  BLK_ROTATE_NONE      = $00;
  BLK_ROTATE_90        = $01;
  BLK_ROTATE_180       = $02;
  BLK_ROTATE_270       = $03;
  BLK_ROTATE_ANGLE     = $100000;
  BLK_ROTATE_DETECT  = $100;

//for Blk_Mirror function
  BLK_MIRROR_GET        = -1;
  BLK_MIRROR_NONE       = $00;
  BLK_MIRROR_H          = $01;
  BLK_MIRROR_V          = $02;

//for Svr_Create function
  SVR_FORMAT_PDF          = $01;
  SVR_FORMAT_RTF          = $02;
  SVR_FORMAT_TXT_ASCII    = $03;
  SVR_FORMAT_TXT_UNICODE  = $04;
  SVR_FORMAT_XML          = $05;
  SVR_FORMAT_PDFA         = $06;

//for Scan_Enumerate function
  SCAN_GETDEFAULTDEVICE   = $01;
  SCAN_SETDEFAULTDEVICE   = $100;

//for Scan_ScanToImg and Scan_ScanToFile functions
  SCAN_NOUI               = $01;
  SCAN_SOURCEADF             = $02;
  SCAN_SOURCEAUTO         = $04;
  SCAN_DONTCLOSEDS		    = $08;
  SCAN_FILE_SEPARATE      = $10;

//for Blk_GetWordFontStyle function
  FONT_STYLE_UNDERLINED	= $01;
  FONT_STYLE_STRIKED		= $02;
  FONT_STYLE_BOLD			  = $04;
  FONT_STYLE_ITALIC		  = $08;

//for Img_GetProperty function, PropertyID parameter
  IMG_PROP_DPIX			    = $01;	//original DPI
  IMG_PROP_DPIY			    = $02;
  IMG_PROP_BPP		    	= $03;	//color depth
  IMG_PROP_WIDTH		  	= $04;	//original width
  IMG_PROP_HEIGHT		  	= $05;	//original height
  IMG_PROP_INVERTED			= $06; //image was inverted
  IMG_PROP_SKEW			    = $07;	//image skew angle after OCR_STEP_PREFILTERS step, multiplied by 1000
  IMG_PROP_SCALE			  = $08;	//image scale factor after OCR_STEP_PREFILTERS step, multiplied by 1000

//for "Blk_SetWordRegEx" function

  REGEX_SET				      = $00;
  REGEX_CLEAR			      =	$01;
  REGEX_CLEAR_ALL			  = $02;
  REGEX_DISABLE_DICT		= $04;
  REGEX_CHECK			      = $08;

//for Svr_SetDocumentInfo function
  INFO_PDF_AUTHOR			  = $01;
  INFO_PDF_CREATOR	   	= $02;
  INFO_PDF_PRODUCER	  	= $03;
  INFO_PDF_TITLE		  	= $04;
  INFO_PDF_SUBJECT	  	= $05;
  INFO_PDF_KEYWORDS	  	= $06;

//for Blk_GetBarcodeType function
  BARCODE_TYPE_EAN8		  =  $01;
  BARCODE_TYPE_UPCE		  =  $02;
  BARCODE_TYPE_ISBN10		=  $03;
  BARCODE_TYPE_UPCA		  =  $04;
  BARCODE_TYPE_EAN13		=  $05;
  BARCODE_TYPE_ISBN13		=  $06;
  BARCODE_TYPE_ZBAR_I25	=  $07;
  BARCODE_TYPE_CODE39		=  $08;
  BARCODE_TYPE_QRCODE		=  $09;
  BARCODE_TYPE_CODE128	=  $0A;

//for "BarCode/TypesMask" configuration option
  BARCODE_TYPE_MASK_EAN8		=  $01;
  BARCODE_TYPE_MASK_UPCE		=  $02;
  BARCODE_TYPE_MASK_ISBN10	=  $04;
  BARCODE_TYPE_MASK_UPCA		=  $08;
  BARCODE_TYPE_MASK_EAN13		=  $10;
  BARCODE_TYPE_MASK_ISBN13	=  $20;
  BARCODE_TYPE_MASK_ZBAR_I25 = $40;
  BARCODE_TYPE_MASK_CODE39	=  $80;
  BARCODE_TYPE_MASK_QRCODE	=  $100;
  BARCODE_TYPE_MASK_CODE128	=  $200;

//for Img_SaveToFile function
  IMG_FORMAT_BMP			      =  0;
  IMG_FORMAT_JPEG			      =  2;
  IMG_FORMAT_PNG			      = 13;
  IMG_FORMAT_TIFF           = 18;
  IMG_FORMAT_FLAG_BINARIZED = $100;

////////////////////////////////////////

type
  HCFG = integer;
  HOCR = integer;
  HIMG = integer;
  HBLK = integer;
  HSVR = integer;
  HSCAN = integer;

  t_Engine_Initialize = function():integer; stdcall;
  t_Engine_InitializeAdvanced = function(out CfgObj:HCFG; out OcrObj:HOCR; out ImgObj:HIMG):integer; stdcall;
  t_Engine_Uninitialize = function():integer; stdcall;
  t_Engine_SetDataDirectory = function(DirectoryPath: PWCHAR):integer; stdcall;
  t_Engine_GetVersion = function(VerStr: PWCHAR):integer; stdcall;
  t_Engine_SetLicenseKey = function(LicenseKey: PWCHAR):integer; stdcall;

  t_Cfg_Create = function(out CfgObj:HCFG):integer; stdcall;
  t_Cfg_Destroy = function(CfgObj:HCFG):integer; stdcall;
  t_Cfg_LoadOptions = function(CfgObj:HCFG; FileName:PWCHAR):integer; stdcall;
  t_Cfg_SaveOptions = function(CfgObj:HCFG; FileName:PWCHAR):integer; stdcall;
  t_Cfg_LoadOptionsFromString = function(CfgObj:HCFG; XMLString:PWCHAR):integer; stdcall;
  t_Cfg_SaveOptionsToString = function(CfgObj:HCFG; XMLString:PWCHAR; MaxLen:integer):integer; stdcall;
  t_Cfg_GetOption = function(CfgObj:HCFG; BlockType:integer; OptionPath:PWCHAR; OptionValue:PWCHAR; MaxLen:integer):integer; stdcall;
  t_Cfg_SetOption = function(CfgObj:HCFG; BlockType:integer; OptionPath:PWCHAR; OptionValue:PWCHAR):integer; stdcall;
  t_Cfg_DeleteOption = function(CfgObj:HCFG; BlockType:integer; OptionPath:PWCHAR):integer; stdcall;

  t_Ocr_Create = function(CfgObj:HCFG; out OcrObj:HOCR):integer; stdcall;
  t_Ocr_Destroy = function(OcrObj:HOCR):integer; stdcall;
  t_Ocr_ProcessPages = function(ImgObj:HIMG; SvrObj:HSVR; PageIndexStart:integer; PageIndexEnd:integer; OcrObjCnt:integer; Flags:integer):integer; stdcall;

  t_Img_Create = function(OcrObj:HOCR; out ImgObj:HIMG):integer; stdcall;
  t_Img_Destroy = function(ImgObj:HIMG):integer; stdcall;
  t_Img_LoadFile = function(ImgObj:HIMG; FileName:PWCHAR):integer; stdcall;
  t_Img_LoadFromMemory = function(ImgObj:HIMG; Bytes:PBYTE; DataSize:integer):integer; stdcall;
  t_Img_LoadBmpData = function(ImgObj:HIMG; Bytes:PBYTE; Width:integer; Height:integer; Flags:integer; Stride:integer):integer; stdcall;
  t_Img_Unload = function(ImgObj:integer):integer; stdcall;
  t_Img_GetPageCount = function(ImgObj:HIMG):integer; stdcall;
  t_Img_SetPage = function(ImgObj:HIMG; PageIndex:integer):integer; stdcall;
  t_Img_GetSize = function(ImgObj:HIMG; out Width:integer; out Height:integer):integer; stdcall;
  t_Img_DrawToDC = function(ImgObj:HIMG; HandleDC:cardinal; X:integer; Y:integer; var Width:integer; var Height:integer; Flags:integer):integer; stdcall;
  t_Img_DeleteAllBlocks = function(ImgObj:HIMG):integer; stdcall;
  t_Img_AddBlock = function(ImgObj:HIMG; Xpos:integer; Ypos:integer; Width:integer; Height:integer; out BlkObj:HBLK):integer; stdcall;
  t_Img_DeleteBlock = function(ImgObj:HIMG; BlkObj:HBLK):integer; stdcall;
  t_Img_GetBlockCnt = function(ImgObj:HIMG):integer; stdcall;
  t_Img_GetBlock = function(ImgObj:HIMG; BlockIndex:integer; out BlkObj:HBLK):integer; stdcall;
  t_Img_GetImgText = function(ImgObj:HIMG; TextStr:PWCHAR; MaxLen:integer; Flags:integer):integer; stdcall;
  t_Img_GetBmpData = function(ImgObj:HIMG; Bits:PBYTE; var Width:integer; var Height:integer; Flags:integer):integer; stdcall;
  t_Img_OCR = function(ImgObj:HIMG; FirstStep:integer; LastStep:integer; Flags:integer):integer; stdcall;
  t_Img_LoadBlocks = function(ImgObj:HIMG; FileName:PWCHAR):integer; stdcall;
  t_Img_SaveBlocks = function(ImgObj:HIMG; FileName:PWCHAR):integer; stdcall;
  t_Img_GetSkewAngle = function(ImgObj:HIMG):integer; stdcall;
  t_Img_GetPixLineCnt = function(ImgObj:HIMG):integer; stdcall;
  t_Img_GetPixLine = function(ImgObj:HIMG; LineInd:integer; out X1pos:integer; out Y1pos:integer; out X2pos:integer; out Y2pos:integer; out Width:integer):integer; stdcall;
  t_Img_GetScaleFactor = function(ImgObj:HIMG):integer; stdcall;
  t_Img_CalcPointPosition = function(ImgObj:HIMG; var Xpos, Ypos:integer; Mode:integer):integer; stdcall;
  t_Img_CopyCurrentPage = function(ImgObjSrc:HIMG; ImgObjDst:HIMG; Flags:integer):integer; stdcall;
  t_Img_GetProperty = function(ImgObj:HIMG; PropertyID:integer):integer; stdcall;
  t_Img_SaveToFile = function(ImgObj:HIMG; FileName:PWCHAR; Format:integer; Flags:integer):integer; stdcall;
  t_Img_SaveToMemory = function(ImgObj:HIMG; Buffer:pointer; BufferSize:integer; Format:integer; Flags:integer):integer; stdcall;

  t_Blk_GetType = function(BlkObj:HBLK):integer; stdcall;
  t_Blk_SetType = function(BlkObj:HBLK; BlockType:integer):integer; stdcall;
  t_Blk_GetRect = function(BlkObj:HBLK; out Xpos:integer; out Ypos:integer; out Width:integer; out Height:integer):integer; stdcall;
  t_Blk_GetText = function(BlkObj:HBLK; TextStr:PWCHAR; MaxLen:integer; Flags:integer):integer; stdcall;
  t_Blk_GetLineCnt = function(BlkObj:HBLK):integer; stdcall;
  t_Blk_GetLineText = function(BlkObj:HBLK; LineIndex:integer; TextStr:PWCHAR; MaxLen:integer):integer; stdcall;
  t_Blk_GetWordCnt = function(BlkObj:HBLK; LineIndex:integer):integer; stdcall;
  t_Blk_GetWordText = function(BlkObj:HBLK; LineIndex:integer; WordIndex:integer; TextStr:PWCHAR; MaxLen:integer):integer; stdcall;
  t_Blk_SetWordText = function(BlkObj:HBLK; LineIndex:integer; WordIndex:integer; TextStr:PWCHAR):integer; stdcall;
  t_Blk_GetCharCnt = function(BlkObj:HBLK; LineIndex:integer; WordIndex:integer):integer; stdcall;
  t_Blk_GetCharRect = function(BlkObj:HBLK; LineIndex:integer; WordIndex:integer; CharIndex:integer; out Xpos:integer; out Ypos:integer; out Width:integer; out Height:integer):integer; stdcall;
  t_Blk_GetCharText = function(BlkObj:HBLK; LineIndex:integer; WordIndex:integer; CharIndex:integer; ResultIndex:integer; TextStr:PWCHAR; MaxLen:integer):integer; stdcall;
  t_Blk_GetCharQual = function(BlkObj:HBLK; LineIndex:integer; WordIndex:integer; CharIndex:integer; ResultIndex:integer):integer; stdcall;
  t_Blk_GetTextRect = function(BlkObj:HBLK; LineIndex:integer; WordIndex:integer; out Xpos:integer; out Ypos:integer; out Width:integer; out Height:integer):integer; stdcall;
  t_Blk_Inversion = function(BlkObj:HBLK; Inversion:integer):integer; stdcall;
  t_Blk_Rotation = function(BlkObj:HBLK; Rotation:integer):integer; stdcall;
  t_Blk_Mirror = function(BlkObj:HBLK; Mirror:integer):integer; stdcall;
  t_Blk_IsWordInDictionary = function(BlkObj:HBLK; LineIndex:integer; WordIndex:integer):integer; stdcall;
  t_Blk_SetRect = function(BlkObj:HBLK; Xpos:integer; Ypos:integer; Width:integer; Height:integer):integer; stdcall;
  t_Blk_GetWordQual = function(BlkObj:HBLK; LineIndex:integer; WordIndex:integer):integer; stdcall;
  t_Blk_GetWordFontColor = function(BlkObj:HBLK; LineIndex:integer; WordIndex:integer):integer; stdcall;
  t_Blk_GetWordFontSize = function(BlkObj:HBLK; LineIndex:integer; WordIndex:integer):integer; stdcall;
  t_Blk_GetWordFontStyle = function(BlkObj:HBLK; LineIndex:integer; WordIndex:integer):integer; stdcall;
  t_Blk_GetBackgroundColor = function(BlkObj:HBLK):integer; stdcall;
  t_Blk_SetWordRegEx = function(BlkObj:HBLK; LineIndex:integer; WordIndex:integer; RegEx:PWCHAR; Flags:integer):integer; stdcall;
  t_Blk_GetBarcodeCnt = function(BlkObj:HBLK):integer; stdcall;
  t_Blk_GetBarcodeText = function(BlkObj:HBLK; BarcodeInd:integer; TextStr:PWCHAR; MaxLen:integer):integer; stdcall;
  t_Blk_GetBarcodeRect = function(BlkObj:HBLK; BarcodeInd:integer; out Xpos:integer; out Ypos:integer; out Width:integer; out Height:integer):integer; stdcall;
  t_Blk_GetBarcodeType = function(BlkObj:HBLK; BarcodeInd:integer):integer; stdcall;

  t_Svr_Create = function(CfgObj:HCFG; Format:integer; out SvrObj:HSVR):integer; stdcall;
  t_Svr_Destroy = function(SvrObj:HSVR):integer; stdcall;
  t_Svr_NewDocument = function(SvrObj:HSVR):integer; stdcall;
  t_Svr_AddPage = function(SvrObj:HSVR; ImgObj:HIMG; Flags:integer):integer; stdcall;
  t_Svr_SaveToFile = function(SvrObj:HSVR; FileName:PWCHAR):integer; stdcall;
  t_Svr_SaveToMemory = function(SvrObj:HSVR; Buffer:pointer; BufferSize:integer):integer; stdcall;
  t_Svr_GetText = function(SvrObj:HSVR; PageIndex:integer; TextStr:PWCHAR; MaxLen:integer):integer; stdcall;
  t_Svr_SetDocumentInfo = function(SvrObj:HSVR; InfoID:integer; InfoStr:PWCHAR):integer; stdcall;

  t_Scan_Create = function(CfgObj:HCFG; out ScanObj:HSCAN):integer; stdcall;
  t_Scan_Destroy = function(ScanObj:HSCAN):integer; stdcall;
  t_Scan_Enumerate = function(ScanObj:HSCAN; ScannerNames:PWCHAR; MaxLen:integer; Flags:integer):integer; stdcall;
  t_Scan_ScanToImg = function(ScanObj:HSCAN; ImgObj:HIMG; ScannerIndex:integer; Flags:integer):integer; stdcall;
  t_Scan_ScanToFile = function(ScanObj:HSCAN; FileName:PWCHAR; ScannerIndex:integer; Flags:integer):integer; stdcall;


////////////////////////////////////////

type
TNSOCR = class
private
  hlib:thandle;
  loaded:boolean;
  ////
  p_Engine_Initialize:t_Engine_Initialize;
  p_Engine_InitializeAdvanced: t_Engine_InitializeAdvanced;
  p_Engine_Uninitialize:t_Engine_Uninitialize;
  p_Engine_SetDataDirectory:t_Engine_SetDataDirectory;
  p_Engine_GetVersion:t_Engine_GetVersion;
  p_Engine_SetLicenseKey:t_Engine_SetLicenseKey;

  p_Cfg_Create:t_Cfg_Create;
  p_Cfg_Destroy:t_Cfg_Destroy;
  p_Cfg_LoadOptions:t_Cfg_LoadOptions;
  p_Cfg_SaveOptions:t_Cfg_SaveOptions;
  p_Cfg_LoadOptionsFromString:t_Cfg_LoadOptionsFromString;
  p_Cfg_SaveOptionsToString:t_Cfg_SaveOptionsToString;
  p_Cfg_GetOption:t_Cfg_GetOption;
  p_Cfg_SetOption:t_Cfg_SetOption;
  p_Cfg_DeleteOption:t_Cfg_DeleteOption;

  p_Ocr_Create:t_Ocr_Create;
  p_Ocr_Destroy:t_Ocr_Destroy;
  p_Ocr_ProcessPages:t_Ocr_ProcessPages;

  p_Img_Create:t_Img_Create;
  p_Img_Destroy:t_Img_Destroy;
  p_Img_LoadFile:t_Img_LoadFile;
  p_Img_LoadFromMemory:t_Img_LoadFromMemory;
  p_Img_LoadBmpData:t_Img_LoadBmpData;
  p_Img_Unload:t_Img_Unload;
  p_Img_GetPageCount:t_Img_GetPageCount;
  p_Img_SetPage:t_Img_SetPage;
  p_Img_GetSize:t_Img_GetSize;
  p_Img_DrawToDC:t_Img_DrawToDC;
  p_Img_DeleteAllBlocks:t_Img_DeleteAllBlocks;
  p_Img_AddBlock:t_Img_AddBlock;
  p_Img_DeleteBlock:t_Img_DeleteBlock;
  p_Img_GetBlockCnt:t_Img_GetBlockCnt;
  p_Img_GetBlock:t_Img_GetBlock;
  p_Img_GetImgText:t_Img_GetImgText;
  p_Img_GetBmpData:t_Img_GetBmpData;
  p_Img_OCR:t_Img_OCR;
  p_Img_LoadBlocks:t_Img_LoadBlocks;
  p_Img_SaveBlocks:t_Img_SaveBlocks;
  p_Img_GetSkewAngle:t_Img_GetSkewAngle;
  p_Img_GetPixLineCnt:t_Img_GetPixLineCnt;
  p_Img_GetPixLine:t_Img_GetPixLine;
  p_Img_GetScaleFactor:t_Img_GetScaleFactor;
  p_Img_CalcPointPosition:t_Img_CalcPointPosition;
  p_Img_CopyCurrentPage:t_Img_CopyCurrentPage;
  p_Img_GetProperty:t_Img_GetProperty;
  p_Img_SaveToFile:t_Img_SaveToFile;
  p_Img_SaveToMemory:t_Img_SaveToMemory;

  p_Blk_GetType:t_Blk_GetType;
  p_Blk_SetType:t_Blk_SetType;
  p_Blk_GetRect:t_Blk_GetRect;
  p_Blk_GetText:t_Blk_GetText;
  p_Blk_GetLineCnt:t_Blk_GetLineCnt;
  p_Blk_GetLineText:t_Blk_GetLineText;
  p_Blk_GetWordCnt:t_Blk_GetWordCnt;
  p_Blk_GetWordText:t_Blk_GetWordText;
  p_Blk_SetWordText:t_Blk_SetWordText;
  p_Blk_GetCharCnt:t_Blk_GetCharCnt;
  p_Blk_GetCharRect:t_Blk_GetCharRect;
  p_Blk_GetCharText:t_Blk_GetCharText;
  p_Blk_GetCharQual:t_Blk_GetCharQual;
  p_Blk_GetTextRect:t_Blk_GetTextRect;
  p_Blk_Inversion:t_Blk_Inversion;
  p_Blk_Rotation:t_Blk_Rotation;
  p_Blk_Mirror:t_Blk_Mirror;
  p_Blk_IsWordInDictionary:t_Blk_IsWordInDictionary;
  p_Blk_SetRect:t_Blk_SetRect;
  p_Blk_GetWordQual:t_Blk_GetWordQual;
  p_Blk_GetWordFontColor:t_Blk_GetWordFontColor;
  p_Blk_GetWordFontSize:t_Blk_GetWordFontSize;
  p_Blk_GetWordFontStyle:t_Blk_GetWordFontStyle;
  p_Blk_GetBackgroundColor:t_Blk_GetBackgroundColor;
  p_Blk_SetWordRegEx:t_Blk_SetWordRegEx;
  p_Blk_GetBarcodeCnt:t_Blk_GetBarcodeCnt;
  p_Blk_GetBarcodeText:t_Blk_GetBarcodeText;
  p_Blk_GetBarcodeRect:t_Blk_GetBarcodeRect;
  p_Blk_GetBarcodeType:t_Blk_GetBarcodeType;

  p_Svr_Create:t_Svr_Create;
  p_Svr_Destroy:t_Svr_Destroy;
  p_Svr_NewDocument:t_Svr_NewDocument;
  p_Svr_AddPage:t_Svr_AddPage;
  p_Svr_SaveToFile:t_Svr_SaveToFile;
  p_Svr_SaveToMemory:t_Svr_SaveToMemory;
  p_Svr_GetText:t_Svr_GetText;
  p_Svr_SetDocumentInfo:t_Svr_SetDocumentInfo;

  p_Scan_Create:t_Scan_Create;
  p_Scan_Destroy:t_Scan_Destroy;
  p_Scan_Enumerate:t_Scan_Enumerate;
  p_Scan_ScanToImg:t_Scan_ScanToImg;
  p_Scan_ScanToFile:t_Scan_ScanToFile;
public
  constructor Create(DllFileName:string);
  destructor Destroy; override;
  function IsDllLoaded:boolean;
  function TryLoadDLL(DllFileName:string):boolean;

  //////////////////// DLL functions

  function Engine_Initialize():integer;
  function Engine_InitializeAdvanced(out CfgObj:HCFG; out OcrObj:HOCR; out ImgObj:HIMG):integer;
  function Engine_Uninitialize():integer;
  function Engine_SetDataDirectory(DirectoryPath: widestring):integer;
  function Engine_GetVersion(out VerStr:widestring):integer;
  function Engine_SetLicenseKey(LicenseKey:widestring):integer;

  function Cfg_Create(out CfgObj:HCFG):integer;
  function Cfg_Destroy(CfgObj:HCFG):integer;
  function Cfg_LoadOptions(CfgObj:HCFG; FileName:widestring):integer;
  function Cfg_SaveOptions(CfgObj:HCFG; FileName:widestring):integer;
  function Cfg_LoadOptionsFromString(CfgObj:HCFG; XMLString:widestring):integer;
  function Cfg_SaveOptionsToString(CfgObj:HCFG; out XMLString:widestring):integer;
  function Cfg_GetOption(CfgObj:HCFG; BlockType:integer; OptionPath:widestring; out OptionValue:widestring):integer;
  function Cfg_SetOption(CfgObj:HCFG; BlockType:integer; OptionPath:widestring; OptionValue:widestring):integer;
  function Cfg_DeleteOption(CfgObj:HCFG; BlockType:integer; OptionPath:widestring):integer;

  function Ocr_Create(CfgObj:HCFG; out OcrObj:HOCR):integer;
  function Ocr_Destroy(OcrObj:HOCR):integer;
  function Ocr_ProcessPages(ImgObj:HIMG; SvrObj:HSVR; PageIndexStart:integer; PageIndexEnd:integer; OcrObjCnt:integer; Flags:integer):integer;

  function Img_Create(OcrObj:HOCR; out ImgObj:HIMG):integer;
  function Img_Destroy(ImgObj:HIMG):integer;
  function Img_LoadFile(ImgObj:HIMG; FileName:widestring):integer;
  function Img_LoadFromMemory(ImgObj:HIMG; Bytes:PBYTE; DataSize:integer):integer;
  function Img_LoadBmpData(ImgObj:HIMG; Bits:PBYTE; Width:integer; Height:integer; Flags:integer; Stride:integer):integer;
  function Img_Unload(ImgObj:integer):integer;
  function Img_GetPageCount(ImgObj:HIMG):integer;
  function Img_SetPage(ImgObj:HIMG; PageIndex:integer):integer;
  function Img_GetSize(ImgObj:HIMG; out Width:integer; out Height:integer):integer;
  function Img_DrawToDC(ImgObj:HIMG; HandleDC:cardinal; X:integer; Y:integer; var Width:integer; var Height:integer; Flags:integer):integer;
  function Img_DeleteAllBlocks(ImgObj:HIMG):integer;
  function Img_AddBlock(ImgObj:HIMG; Xpos:integer; Ypos:integer; Width:integer; Height:integer; out BlkObj:HBLK):integer;
  function Img_DeleteBlock(ImgObj:HIMG; BlkObj:HBLK):integer;
  function Img_GetBlockCnt(ImgObj:HIMG):integer;
  function Img_GetBlock(ImgObj:HIMG; BlockIndex:integer; out BlkObj:HBLK):integer;
  function Img_GetImgText(ImgObj:HIMG; out TextStr:widestring; Flags:integer):integer;
  function Img_GetBmpData(ImgObj:HIMG; Bits:PBYTE; var Width:integer; var Height:integer; Flags:integer):integer;
  function Img_OCR(ImgObj:HIMG; FirstStep:integer; LastStep:integer; Flags:integer):integer;
  function Img_LoadBlocks(ImgObj:HIMG; FileName:widestring):integer;
  function Img_SaveBlocks(ImgObj:HIMG; FileName:widestring):integer;
  function Img_GetSkewAngle(ImgObj:HIMG):integer;
  function Img_GetPixLineCnt(ImgObj:HIMG):integer;
  function Img_GetPixLine(ImgObj:HIMG; LineInd:integer; out X1pos:integer; out Y1pos:integer; out X2pos:integer; out Y2pos:integer; out Width:integer):integer;
  function Img_GetScaleFactor(ImgObj:HIMG):integer;
  function Img_CalcPointPosition(ImgObj:HIMG; var Xpos, Ypos:integer; Mode:integer):integer;
  function Img_CopyCurrentPage(ImgObjSrc:HIMG; ImgObjDst:HIMG; Flags:integer):integer;
  function Img_GetProperty(ImgObj:HIMG; PropertyID:integer):integer;
  function Img_SaveToFile(ImgObj:HIMG; FileName:widestring; Format:integer; Flags:integer):integer;
  function Img_SaveToMemory(ImgObj:HIMG; Buffer:pointer; BufferSize:integer; Format:integer; Flags:integer):integer;

  function Blk_GetType(BlkObj:HBLK):integer;
  function Blk_SetType(BlkObj:HBLK; BlockType:integer):integer;
  function Blk_GetRect(BlkObj:HBLK; out Xpos:integer; out Ypos:integer; out Width:integer; out Height:integer):integer;
  function Blk_GetText(BlkObj:HBLK; out TextStr:widestring; Flags:integer):integer;
  function Blk_GetLineCnt(BlkObj:HBLK):integer;
  function Blk_GetLineText(BlkObj:HBLK; LineIndex:integer; out TextStr:widestring):integer;
  function Blk_GetWordCnt(BlkObj:HBLK; LineIndex:integer):integer;
  function Blk_GetWordText(BlkObj:HBLK; LineIndex:integer; WordIndex:integer; out TextStr:widestring):integer;
  function Blk_SetWordText(BlkObj:HBLK; LineIndex:integer; WordIndex:integer; TextStr:widestring):integer;
  function Blk_GetCharCnt(BlkObj:HBLK; LineIndex:integer; WordIndex:integer):integer;
  function Blk_GetCharRect(BlkObj:HBLK; LineIndex:integer; WordIndex:integer; CharIndex:integer; out Xpos:integer; out Ypos:integer; out Width:integer; out Height:integer):integer;
  function Blk_GetCharText(BlkObj:HBLK; LineIndex:integer; WordIndex:integer; CharIndex:integer; ResultIndex:integer; out TextStr:widestring):integer;
  function Blk_GetCharQual(BlkObj:HBLK; LineIndex:integer; WordIndex:integer; CharIndex:integer; ResultIndex:integer):integer;
  function Blk_GetTextRect(BlkObj:HBLK; LineIndex:integer; WordIndex:integer; out Xpos:integer; out Ypos:integer; out Width:integer; out Height:integer):integer;
  function Blk_Inversion(BlkObj:HBLK; Inversion:integer):integer;
  function Blk_Rotation(BlkObj:HBLK; Rotation:integer):integer;
  function Blk_Mirror(BlkObj:HBLK; Mirror:integer):integer;
  function Blk_IsWordInDictionary(BlkObj:HBLK; LineIndex:integer; WordIndex:integer):integer;
  function Blk_SetRect(BlkObj:HBLK; Xpos:integer; Ypos:integer; Width:integer; Height:integer):integer;
  function Blk_GetWordQual(BlkObj:HBLK; LineIndex:integer; WordIndex:integer):integer;
  function Blk_GetWordFontColor(BlkObj:HBLK; LineIndex:integer; WordIndex:integer):integer;
  function Blk_GetWordFontSize(BlkObj:HBLK; LineIndex:integer; WordIndex:integer):integer;
  function Blk_GetWordFontStyle(BlkObj:HBLK; LineIndex:integer; WordIndex:integer):integer;
  function Blk_GetBackgroundColor(BlkObj:HBLK):integer;
  function Blk_SetWordRegEx(BlkObj:HBLK; LineIndex:integer; WordIndex:integer; RegEx:widestring; Flags:integer):integer;
  function Blk_GetBarcodeCnt(BlkObj:HBLK):integer;
  function Blk_GetBarcodeText(BlkObj:HBLK; BarcodeInd:integer; out TextStr:widestring):integer;
  function Blk_GetBarcodeRect(BlkObj:HBLK; BarcodeInd:integer; out Xpos:integer; out Ypos:integer; out Width:integer; out Height:integer):integer;
  function Blk_GetBarcodeType(BlkObj:HBLK; BarcodeInd:integer):integer;

  function Svr_Create(CfgObj:HCFG; Format:integer; out SvrObj:HSVR):integer;
  function Svr_Destroy(SvrObj:HSVR):integer;
  function Svr_NewDocument(SvrObj:HSVR):integer;
  function Svr_AddPage(SvrObj:HSVR; ImgObj:HIMG; Flags:integer):integer;
  function Svr_SaveToFile(SvrObj:HSVR; FileName:widestring):integer;
  function Svr_SaveToMemory(SvrObj:HSVR; Buffer:pointer; BufferSize:integer):integer;
  function Svr_GetText(SvrObj:HSVR; PageIndex:integer; out TextStr:widestring):integer;
  function Svr_SetDocumentInfo(SvrObj:HSVR; InfoID:integer; InfoStr:widestring):integer;

  function Scan_Create(CfgObj:HCFG; out ScanObj:HSCAN):integer;
  function Scan_Destroy(ScanObj:HSCAN):integer;
  function Scan_Enumerate(ScanObj:HSCAN; out ScannerNames:widestring; Flags:integer):integer;
  function Scan_ScanToImg(ScanObj:HSCAN; ImgObj:HIMG; ScannerIndex:integer; Flags:integer):integer;
  function Scan_ScanToFile(ScanObj:HSCAN; FileName:widestring; ScannerIndex:integer; Flags:integer):integer;
end;


                         implementation





{ TNSOCR }

constructor TNSOCR.Create(DllFileName: string);
begin
  loaded:=false;
  TryLoadDLL(DllFileName);
end;

function TNSOCR.TryLoadDLL(DllFileName:string):boolean;
begin
  if loaded then
    begin
      result:=true;
      exit;
    end;

  result:=false;
  if DllFileName = '' then DllFileName := 'NSOCR.dll';
  hlib := LoadLibrary(@DllFileName[1]);
  loaded := hlib <> 0;
  if not loaded then exit;
////
  p_Engine_Initialize := GetProcAddress(hlib, 'Engine_Initialize');
  p_Engine_InitializeAdvanced := GetProcAddress(hlib, 'Engine_InitializeAdvanced');
  p_Engine_Uninitialize := GetProcAddress(hlib, 'Engine_Uninitialize');
  p_Engine_SetDataDirectory := GetProcAddress(hlib, 'Engine_SetDataDirectory');
  p_Engine_GetVersion := GetProcAddress(hlib, 'Engine_GetVersion');
  p_Engine_SetLicenseKey := GetProcAddress(hlib, 'Engine_SetLicenseKey');

  p_Cfg_Create := GetProcAddress(hlib, 'Cfg_Create');
  p_Cfg_Destroy := GetProcAddress(hlib, 'Cfg_Destroy');
  p_Cfg_LoadOptions := GetProcAddress(hlib, 'Cfg_LoadOptions');
  p_Cfg_SaveOptions := GetProcAddress(hlib, 'Cfg_SaveOptions');
  p_Cfg_LoadOptionsFromString := GetProcAddress(hlib, 'Cfg_LoadOptionsFromString');
  p_Cfg_SaveOptionsToString := GetProcAddress(hlib, 'Cfg_SaveOptionsToString');
  p_Cfg_GetOption := GetProcAddress(hlib, 'Cfg_GetOption');
  p_Cfg_SetOption := GetProcAddress(hlib, 'Cfg_SetOption');
  p_Cfg_DeleteOption := GetProcAddress(hlib, 'Cfg_DeleteOption');

  p_Ocr_Create := GetProcAddress(hlib, 'Ocr_Create');
  p_Ocr_Destroy := GetProcAddress(hlib, 'Ocr_Destroy');
  p_Ocr_ProcessPages := GetProcAddress(hlib, 'Ocr_ProcessPages');

  p_Img_Create := GetProcAddress(hlib, 'Img_Create');
  p_Img_Destroy := GetProcAddress(hlib, 'Img_Destroy');
  p_Img_LoadFile := GetProcAddress(hlib, 'Img_LoadFile');
  p_Img_LoadBmpData := GetProcAddress(hlib, 'Img_LoadBmpData');
  p_Img_LoadFromMemory := GetProcAddress(hlib, 'Img_LoadFromMemory');
  p_Img_Unload := GetProcAddress(hlib, 'Img_Unload');
  p_Img_GetPageCount := GetProcAddress(hlib, 'Img_GetPageCount');
  p_Img_SetPage := GetProcAddress(hlib, 'Img_SetPage');
  p_Img_GetSize := GetProcAddress(hlib, 'Img_GetSize');
  p_Img_DrawToDC := GetProcAddress(hlib, 'Img_DrawToDC');
  p_Img_DeleteAllBlocks := GetProcAddress(hlib, 'Img_DeleteAllBlocks');
  p_Img_AddBlock := GetProcAddress(hlib, 'Img_AddBlock');
  p_Img_DeleteBlock := GetProcAddress(hlib, 'Img_DeleteBlock');
  p_Img_GetBlockCnt := GetProcAddress(hlib, 'Img_GetBlockCnt');
  p_Img_GetBlock := GetProcAddress(hlib, 'Img_GetBlock');
  p_Img_GetImgText := GetProcAddress(hlib, 'Img_GetImgText');
  p_Img_GetBmpData := GetProcAddress(hlib, 'Img_GetBmpData');
  p_Img_OCR := GetProcAddress(hlib, 'Img_OCR');
  p_Img_LoadBlocks := GetProcAddress(hlib, 'Img_LoadBlocks');
  p_Img_SaveBlocks := GetProcAddress(hlib, 'Img_SaveBlocks');
  p_Img_GetSkewAngle := GetProcAddress(hlib, 'Img_GetSkewAngle');
  p_Img_GetPixLineCnt := GetProcAddress(hlib, 'Img_GetPixLineCnt');
  p_Img_GetPixLine := GetProcAddress(hlib, 'Img_GetPixLine');
  p_Img_GetScaleFactor := GetProcAddress(hlib, 'Img_GetScaleFactor');
  p_Img_CalcPointPosition := GetProcAddress(hlib, 'Img_CalcPointPosition');
  p_Img_CopyCurrentPage := GetProcAddress(hlib, 'Img_CopyCurrentPage');
  p_Img_GetProperty := GetProcAddress(hlib, 'Img_GetProperty');
  p_Img_SaveToFile := GetProcAddress(hlib, 'Img_SaveToFile');
  p_Img_SaveToMemory := GetProcAddress(hlib, 'Img_SaveToMemory');

  p_Blk_GetType := GetProcAddress(hlib, 'Blk_GetType');
  p_Blk_SetType := GetProcAddress(hlib, 'Blk_SetType');
  p_Blk_GetRect := GetProcAddress(hlib, 'Blk_GetRect');
  p_Blk_GetText := GetProcAddress(hlib, 'Blk_GetText');
  p_Blk_GetLineCnt := GetProcAddress(hlib, 'Blk_GetLineCnt');
  p_Blk_GetLineText := GetProcAddress(hlib, 'Blk_GetLineText');
  p_Blk_GetWordCnt := GetProcAddress(hlib, 'Blk_GetWordCnt');
  p_Blk_GetWordText := GetProcAddress(hlib, 'Blk_GetWordText');
  p_Blk_SetWordText := GetProcAddress(hlib, 'Blk_SetWordText');
  p_Blk_GetCharCnt := GetProcAddress(hlib, 'Blk_GetCharCnt');
  p_Blk_GetCharRect := GetProcAddress(hlib, 'Blk_GetCharRect');
  p_Blk_GetCharText := GetProcAddress(hlib, 'Blk_GetCharText');
  p_Blk_GetCharQual := GetProcAddress(hlib, 'Blk_GetCharQual');
  p_Blk_GetTextRect := GetProcAddress(hlib, 'Blk_GetTextRect');
  p_Blk_Inversion := GetProcAddress(hlib, 'Blk_Inversion');
  p_Blk_Rotation := GetProcAddress(hlib, 'Blk_Rotation');
  p_Blk_Mirror := GetProcAddress(hlib, 'Blk_Mirror');
  p_Blk_IsWordInDictionary := GetProcAddress(hlib, 'Blk_IsWordInDictionary');
  p_Blk_SetRect := GetProcAddress(hlib, 'Blk_SetRect');
  p_Blk_GetWordQual := GetProcAddress(hlib, 'Blk_GetWordQual');
  p_Blk_GetWordFontColor := GetProcAddress(hlib, 'Blk_GetWordFontColor');
  p_Blk_GetWordFontSize := GetProcAddress(hlib, 'Blk_GetWordFontSize');
  p_Blk_GetWordFontStyle := GetProcAddress(hlib, 'Blk_GetWordFontStyle');
  p_Blk_GetBackgroundColor := GetProcAddress(hlib, 'Blk_GetBackgroundColor');
  p_Blk_SetWordRegEx := GetProcAddress(hlib, 'Blk_SetWordRegEx');
  p_Blk_GetBarcodeCnt := GetProcAddress(hlib, 'Blk_GetBarcodeCnt');
  p_Blk_GetBarcodeText := GetProcAddress(hlib, 'Blk_GetBarcodeText');
  p_Blk_GetBarcodeRect := GetProcAddress(hlib, 'Blk_GetBarcodeRect');
  p_Blk_GetBarcodeType := GetProcAddress(hlib, 'Blk_GetBarcodeType');

  p_Svr_Create := GetProcAddress(hlib, 'Svr_Create');
  p_Svr_Destroy := GetProcAddress(hlib, 'Svr_Destroy');
  p_Svr_NewDocument := GetProcAddress(hlib, 'Svr_NewDocument');
  p_Svr_AddPage := GetProcAddress(hlib, 'Svr_AddPage');
  p_Svr_SaveToFile := GetProcAddress(hlib, 'Svr_SaveToFile');
  p_Svr_SaveToMemory := GetProcAddress(hlib, 'Svr_SaveToMemory');
  p_Svr_GetText := GetProcAddress(hlib, 'Svr_GetText');
  p_Svr_SetDocumentInfo := GetProcAddress(hlib, 'Svr_SetDocumentInfo');

  p_Scan_Create := GetProcAddress(hlib, 'Scan_Create');
  p_Scan_Destroy := GetProcAddress(hlib, 'Scan_Destroy');
  p_Scan_Enumerate := GetProcAddress(hlib, 'Scan_Enumerate');
  p_Scan_ScanToImg := GetProcAddress(hlib, 'Scan_ScanToImg');
  p_Scan_ScanToFile := GetProcAddress(hlib, 'Scan_ScanToFile');

  result:=true;
end;

destructor TNSOCR.Destroy;
begin
  if loaded then FreeLibrary(hlib);
end;

function TNSOCR.IsDllLoaded: boolean;
begin
  result := loaded;
end;

////////////////////////////////////

function TNSOCR.Engine_Initialize: integer;
begin
  if not loaded or not Assigned(p_Engine_Initialize) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Engine_Initialize();
end;

function TNSOCR.Engine_InitializeAdvanced(out CfgObj: HCFG; out OcrObj: HOCR; out ImgObj: HIMG): integer;
begin
  if not loaded or not Assigned(p_Engine_InitializeAdvanced) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Engine_InitializeAdvanced(CfgObj, OcrObj, ImgObj);
end;

function TNSOCR.Engine_Uninitialize: integer;
begin
  if not loaded or not Assigned(p_Engine_Uninitialize) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Engine_Uninitialize();
end;

function TNSOCR.Engine_SetDataDirectory(DirectoryPath: widestring):integer;
begin
  if not loaded or not Assigned(p_Engine_SetDataDirectory) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Engine_SetDataDirectory(PWideChar(DirectoryPath));
end;

function TNSOCR.Engine_SetLicenseKey(LicenseKey: widestring): integer;
begin
  if not loaded or not Assigned(p_Engine_SetLicenseKey) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Engine_SetLicenseKey(PWideChar(LicenseKey));
end;

function TNSOCR.Engine_GetVersion(out VerStr: widestring): integer;
var buf:array[0..100] of widechar;
begin
  if not loaded or not Assigned(p_Engine_GetVersion) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;


  result := p_Engine_GetVersion(buf);
  VerStr := buf;
end;

//////////////////

function TNSOCR.Cfg_Create(out CfgObj:HCFG):integer;
begin
  if not loaded or not Assigned(p_Cfg_Create) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Cfg_Create(CfgObj);
end;

function TNSOCR.Cfg_Destroy(CfgObj:HCFG):integer;
begin
  if not loaded or not Assigned(p_Cfg_Destroy) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Cfg_Destroy(CfgObj);
end;

function TNSOCR.Cfg_LoadOptions(CfgObj:HCFG; FileName:widestring):integer;
begin
  if not loaded or not Assigned(p_Cfg_LoadOptions) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Cfg_LoadOptions(CfgObj, PWideChar(FileName));
end;

function TNSOCR.Cfg_SaveOptions(CfgObj:HCFG; FileName:widestring):integer;
begin
  if not loaded or not Assigned(p_Cfg_SaveOptions) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Cfg_SaveOptions(CfgObj, PWideChar(FileName));
end;

function TNSOCR.Cfg_LoadOptionsFromString(CfgObj:HCFG; XMLString:widestring):integer;
begin
  if not loaded or not Assigned(p_Cfg_LoadOptionsFromString) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Cfg_LoadOptionsFromString(CfgObj, PWideChar(XMLString));
end;

function TNSOCR.Cfg_SaveOptionsToString(CfgObj:HCFG; out XMLString:widestring):integer;
var txt: PWCHAR;
    cnt:integer;
begin
  if not loaded or not Assigned(p_Cfg_SaveOptionsToString) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  //get text length
  cnt := p_Cfg_SaveOptionsToString(CfgObj, nil, 0);
  if cnt > ERROR_FIRST then
    begin
      result := cnt;
      exit;
    end;
  // get buffer size plus terminating NULL character
  // allocate unicode buffer text length plus null-terminated zero
  inc(cnt);
  getmem(txt, 2 * cnt);
  cnt := p_Cfg_SaveOptionsToString(CfgObj, txt, cnt); // MaxLen must include terminating NULL character

  XMLString := txt;
  freemem(txt);
  result := cnt;
end;

function TNSOCR.Cfg_GetOption(CfgObj:HCFG; BlockType:integer; OptionPath:widestring; out OptionValue:widestring):integer;
var txt: PWCHAR;
    cnt:integer;
begin
  if not loaded or not Assigned(p_Cfg_GetOption) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  //get text length
  cnt := p_Cfg_GetOption(CfgObj, BlockType, PWideChar(OptionPath), nil, 0);
  if cnt > ERROR_FIRST then
    begin
      result := cnt;
      exit;
    end;
  // get buffer size plus terminating NULL character
  // allocate unicode buffer text length plus null-terminated zero
  inc(cnt);
  getmem(txt, 2 * cnt);
  cnt := p_Cfg_GetOption(CfgObj, BlockType, PWideChar(OptionPath), txt, cnt); // MaxLen must include terminating NULL character

  OptionValue := txt;
  freemem(txt);
  result := cnt;
end;

function TNSOCR.Cfg_SetOption(CfgObj:HCFG; BlockType:integer; OptionPath:widestring; OptionValue:widestring):integer;
begin
  if not loaded or not Assigned(p_Cfg_SetOption) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Cfg_SetOption(CfgObj, BlockType, PWideChar(OptionPath), PWideChar(OptionValue));
end;

function TNSOCR.Cfg_DeleteOption(CfgObj: HCFG; BlockType: integer; OptionPath: widestring): integer;
begin
  if not loaded or not Assigned(p_Cfg_DeleteOption) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Cfg_DeleteOption(CfgObj, BlockType, PWideChar(OptionPath));
end;

///////////////////

function TNSOCR.Ocr_Create(CfgObj:HCFG; out OcrObj:HOCR):integer;
begin
  if not loaded or not Assigned(p_Ocr_Create) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Ocr_Create(CfgObj, OcrObj);
end;

function TNSOCR.Ocr_Destroy(OcrObj:HOCR):integer;
begin
  if not loaded or not Assigned(p_Ocr_Destroy) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Ocr_Destroy(OcrObj);
end;

function TNSOCR.Ocr_ProcessPages(ImgObj:HIMG; SvrObj:HSVR; PageIndexStart:integer; PageIndexEnd:integer; OcrObjCnt:integer; Flags:integer):integer;
begin
  if not loaded or not Assigned(p_Ocr_ProcessPages) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Ocr_ProcessPages(ImgObj, SvrObj, PageIndexStart, PageIndexEnd, OcrObjCnt, Flags);
end;

///////////////////

function TNSOCR.Img_Create(OcrObj:HOCR; out ImgObj:HIMG):integer;
begin
  if not loaded or not Assigned(p_Img_Create) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_Create(OcrObj, ImgObj);
end;

function TNSOCR.Img_Destroy(ImgObj:HIMG):integer;
begin
  if not loaded or not Assigned(p_Img_Destroy) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_Destroy(ImgObj);
end;

function TNSOCR.Img_LoadFile(ImgObj:HIMG; FileName:widestring):integer;
begin
  if not loaded or not Assigned(p_Img_LoadFile) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_LoadFile(ImgObj, PWideChar(FileName));
end;

function TNSOCR.Img_LoadFromMemory(ImgObj: HIMG; Bytes: PBYTE; DataSize: integer): integer;
begin
  if not loaded or not Assigned(p_Img_LoadFromMemory) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_LoadFromMemory(ImgObj, Bytes, DataSize);
end;

function TNSOCR.Img_LoadBmpData(ImgObj: HIMG; Bits: PBYTE; Width, Height, Flags: integer; Stride:integer): integer;
begin
  if not loaded or not Assigned(p_Img_LoadBmpData) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_LoadBmpData(ImgObj, Bits, Width, Height, Flags, Stride);
end;

function TNSOCR.Img_Unload(ImgObj: integer): integer;
begin
  if not loaded or not Assigned(p_Img_Unload) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_Unload(ImgObj);
end;

function TNSOCR.Img_GetPageCount(ImgObj:HIMG):integer;
begin
  if not loaded or not Assigned(p_Img_GetPageCount) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_GetPageCount(ImgObj);
end;

function TNSOCR.Img_SetPage(ImgObj:HIMG; PageIndex:integer):integer;
begin
  if not loaded or not Assigned(p_Img_SetPage) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_SetPage(ImgObj, PageIndex);
end;

function TNSOCR.Img_GetSize(ImgObj:HIMG; out Width:integer; out Height:integer):integer;
begin
  if not loaded or not Assigned(p_Img_GetSize) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_GetSize(ImgObj, Width, Height);
end;

function TNSOCR.Img_DrawToDC(ImgObj:HIMG; HandleDC:cardinal; X:integer; Y:integer; var Width:integer; var Height:integer; Flags:integer):integer;
begin
  if not loaded or not Assigned(p_Img_DrawToDC) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_DrawToDC(ImgObj, HandleDC, X, Y, Width, Height, Flags);
end;

function TNSOCR.Img_DeleteAllBlocks(ImgObj:HIMG):integer;
begin
  if not loaded or not Assigned(p_Img_DeleteAllBlocks) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_DeleteAllBlocks(ImgObj);
end;

function TNSOCR.Img_AddBlock(ImgObj:HIMG; Xpos:integer; Ypos:integer; Width:integer; Height:integer; out BlkObj:HBLK):integer;
begin
  if not loaded or not Assigned(p_Img_AddBlock) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_AddBlock(ImgObj, Xpos, Ypos, Width, Height, BlkObj);
end;

function TNSOCR.Img_DeleteBlock(ImgObj:HIMG; BlkObj:HBLK):integer;
begin
  if not loaded or not Assigned(p_Img_DeleteBlock) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_DeleteBlock(ImgObj, BlkObj);
end;

function TNSOCR.Img_GetBlockCnt(ImgObj:HIMG):integer;
begin
  if not loaded or not Assigned(p_Img_GetBlockCnt) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_GetBlockCnt(ImgObj);
end;

function TNSOCR.Img_GetBlock(ImgObj:HIMG; BlockIndex:integer; out BlkObj:HBLK):integer;
begin
  if not loaded or not Assigned(p_Img_GetBlock) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_GetBlock(ImgObj, BlockIndex, BlkObj);
end;

function TNSOCR.Img_GetImgText(ImgObj:HIMG; out TextStr:widestring; Flags:integer):integer;
var txt: PWCHAR;
    cnt:integer;
begin
  if not loaded or not Assigned(p_Img_GetImgText) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  //get text length
  cnt := p_Img_GetImgText(ImgObj, nil, 0, Flags);
  if cnt > ERROR_FIRST then
    begin
      result := cnt;
      exit;
    end;
  // get buffer size plus terminating NULL character
  // allocate unicode buffer text length plus null-terminated zero
  inc(cnt);
  getmem(txt, 2 * cnt);
  cnt := p_Img_GetImgText(ImgObj, txt, cnt, flags); // MaxLen must include terminating NULL character

  TextStr := txt;
  freemem(txt);
  result := cnt;
end;

function TNSOCR.Img_GetBmpData(ImgObj: HIMG; Bits: PBYTE; var Width:integer; var Height:integer; Flags: integer): integer;
begin
  if not loaded or not Assigned(p_Img_GetBmpData) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_GetBmpData(ImgObj, Bits, Width, Height, Flags);
end;

function TNSOCR.Img_OCR(ImgObj:HIMG; FirstStep:integer; LastStep:integer; Flags:integer): integer;
begin
  if not loaded or not Assigned(p_Img_OCR) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_OCR(ImgObj, FirstStep, LastStep, Flags);
end;

function TNSOCR.Img_LoadBlocks(ImgObj: HIMG; FileName: widestring): integer;
begin
  if not loaded or not Assigned(p_Img_LoadBlocks) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_LoadBlocks(ImgObj, PWideChar(FileName));
end;

function TNSOCR.Img_SaveBlocks(ImgObj: HIMG; FileName: widestring): integer;
begin
  if not loaded or not Assigned(p_Img_SaveBlocks) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_SaveBlocks(ImgObj, PWideChar(FileName));
end;

function TNSOCR.Img_GetSkewAngle(ImgObj: HIMG): integer;
begin
  if not loaded or not Assigned(p_Img_GetSkewAngle) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_GetSkewAngle(ImgObj);
end;

function TNSOCR.Img_GetPixLineCnt(ImgObj: HIMG): integer;
begin
  if not loaded or not Assigned(p_Img_GetPixLineCnt) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_GetPixLineCnt(ImgObj);
end;

function TNSOCR.Img_GetPixLine(ImgObj: HIMG; LineInd: integer; out X1pos, Y1pos, X2pos, Y2pos, Width: integer): integer;
begin
  if not loaded or not Assigned(p_Img_GetPixLine) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_GetPixLine(ImgObj, LineInd, X1pos, Y1pos, X2pos, Y2pos, Width);
end;

function TNSOCR.Img_GetScaleFactor(ImgObj: HIMG): integer;
begin
  if not loaded or not Assigned(p_Img_GetScaleFactor) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_GetScaleFactor(ImgObj);
end;

function TNSOCR.Img_CalcPointPosition(ImgObj: HIMG; var Xpos, Ypos: integer; Mode: integer): integer;
begin
  if not loaded or not Assigned(p_Img_CalcPointPosition) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_CalcPointPosition(ImgObj, Xpos, Ypos, Mode);
end;

function TNSOCR.Img_CopyCurrentPage(ImgObjSrc:HIMG; ImgObjDst:HIMG; Flags:integer):integer;
begin
  if not loaded or not Assigned(p_Img_CopyCurrentPage) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_CopyCurrentPage(ImgObjSrc, ImgObjDst, Flags);
end;

function TNSOCR.Img_GetProperty(ImgObj: HIMG; PropertyID: integer): integer;
begin
  if not loaded or not Assigned(p_Img_GetProperty) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_GetProperty(ImgObj, PropertyID);
end;

function TNSOCR.Img_SaveToFile(ImgObj: HIMG; FileName: widestring; Format, Flags: integer): integer;
begin
  if not loaded or not Assigned(p_Img_SaveToFile) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_SaveToFile(ImgObj, PWideChar(FileName), Format, Flags);
end;

function TNSOCR.Img_SaveToMemory(ImgObj: HIMG; Buffer: pointer; BufferSize, Format, Flags: integer): integer;
begin
  if not loaded or not Assigned(p_Img_SaveToMemory) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Img_SaveToMemory(ImgObj, Buffer, BufferSize, Format, Flags);
end;

///////////////////////

function TNSOCR.Blk_GetType(BlkObj: HBLK): integer;
begin
  if not loaded or not Assigned(p_Blk_GetType) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_GetType(BlkObj);
end;

function TNSOCR.Blk_SetType(BlkObj: HBLK; BlockType:integer): integer;
begin
  if not loaded or not Assigned(p_Blk_SetType) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_SetType(BlkObj, BlockType);
end;

function TNSOCR.Blk_GetRect(BlkObj:HBLK; out Xpos:integer; out Ypos:integer; out Width:integer; out Height:integer):integer;
begin
  if not loaded or not Assigned(p_Blk_GetRect) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_GetRect(BlkObj, Xpos, Ypos, Width, Height);
end;

function TNSOCR.Blk_GetText(BlkObj:HBLK; out TextStr:widestring; Flags:integer):integer;
var txt: PWCHAR;
    cnt:integer;
begin
  if not loaded or not Assigned(p_Blk_GetText) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  //get text length
  cnt := p_Blk_GetText(BlkObj, nil, 0, Flags);
  if cnt > ERROR_FIRST then
    begin
      result := cnt;
      exit;
    end;
  // get buffer size plus terminating NULL character
  // allocate unicode buffer text length plus null-terminated zero
  inc(cnt);
  getmem(txt, 2 * cnt);
  cnt := p_Blk_GetText(BlkObj, txt, cnt, Flags); // MaxLen must include terminating NULL character

  TextStr := txt;
  freemem(txt);
  result := cnt;
end;

function TNSOCR.Blk_GetLineCnt(BlkObj:HBLK):integer;
begin
  if not loaded or not Assigned(p_Blk_GetLineCnt) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_GetLineCnt(BlkObj);
end;

function TNSOCR.Blk_GetLineText(BlkObj:HBLK; LineIndex:integer; out TextStr:widestring):integer;
var txt: PWCHAR;
    cnt:integer;
begin
  if not loaded or not Assigned(p_Blk_GetLineText) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  //get text length
  cnt := p_Blk_GetLineText(BlkObj, LineIndex, nil, 0);
  if cnt > ERROR_FIRST then
    begin
      result := cnt;
      exit;
    end;
  // get buffer size plus terminating NULL character
  // allocate unicode buffer text length plus null-terminated zero
  inc(cnt);
  getmem(txt, 2 * cnt);
  cnt := p_Blk_GetLineText(BlkObj, LineIndex, txt, cnt); // MaxLen must include terminating NULL character

  TextStr := txt;
  freemem(txt);
  result := cnt;
end;

function TNSOCR.Blk_GetWordCnt(BlkObj:HBLK; LineIndex:integer):integer;
begin
  if not loaded or not Assigned(p_Blk_GetWordCnt) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_GetWordCnt(BlkObj, LineIndex);
end;

function TNSOCR.Blk_GetWordText(BlkObj:HBLK; LineIndex:integer; WordIndex:integer; out TextStr:widestring):integer;
var txt: PWCHAR;
    cnt:integer;
begin
  if not loaded or not Assigned(p_Blk_GetWordText) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  //get text length
  cnt := p_Blk_GetWordText(BlkObj, LineIndex, WordIndex, nil, 0);
  if cnt > ERROR_FIRST then
    begin
      result := cnt;
      exit;
    end;
  // get buffer size plus terminating NULL character
  // allocate unicode buffer text length plus null-terminated zero
  inc(cnt);
  getmem(txt, 2 * cnt);
  cnt := p_Blk_GetWordText(BlkObj, LineIndex, WordIndex, txt, cnt); // MaxLen must include terminating NULL character

  TextStr := txt;
  freemem(txt);
  result := cnt;
end;

function TNSOCR.Blk_SetWordText(BlkObj:HBLK; LineIndex:integer; WordIndex:integer; TextStr:widestring):integer;
begin
  if not loaded or not Assigned(p_Blk_SetWordText) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_SetWordText(BlkObj, LineIndex, WordIndex, PWideChar(TextStr));
end;

function TNSOCR.Blk_GetCharCnt(BlkObj:HBLK; LineIndex:integer; WordIndex:integer):integer;
begin
  if not loaded or not Assigned(p_Blk_GetCharCnt) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_GetCharCnt(BlkObj, LineIndex, WordIndex);
end;

function TNSOCR.Blk_GetCharRect(BlkObj:HBLK; LineIndex:integer; WordIndex:integer; CharIndex:integer; out Xpos:integer; out Ypos:integer; out Width:integer; out Height:integer):integer;
begin
  if not loaded or not Assigned(p_Blk_GetCharRect) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_GetCharRect(BlkObj, LineIndex, WordIndex, CharIndex, Xpos, Ypos, Width, Height);
end;

function TNSOCR.Blk_GetCharText(BlkObj:HBLK; LineIndex:integer; WordIndex:integer; CharIndex:integer; ResultIndex:integer; out TextStr:widestring):integer;
var txt: PWCHAR;
    cnt:integer;
begin
  if not loaded or not Assigned(p_Blk_GetCharText) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  //get text length
  cnt := p_Blk_GetCharText(BlkObj, LineIndex, WordIndex, CharIndex, ResultIndex, nil, 0);
  if cnt > ERROR_FIRST then
    begin
      result := cnt;
      exit;
    end;
  // get buffer size plus terminating NULL character
  // allocate unicode buffer text length plus null-terminated zero
  inc(cnt);
  getmem(txt, 2 * cnt);
  cnt := p_Blk_GetCharText(BlkObj, LineIndex, WordIndex, CharIndex, ResultIndex, txt, cnt); // MaxLen must include terminating NULL character

  TextStr := txt;
  freemem(txt);
  result := cnt;
end;

function TNSOCR.Blk_GetCharQual(BlkObj: HBLK; LineIndex, WordIndex, CharIndex: integer; ResultIndex:integer): integer;
begin
  if not loaded or not Assigned(p_Blk_GetCharQual) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_GetCharQual(BlkObj, LineIndex, WordIndex, CharIndex, ResultIndex);
end;

function TNSOCR.Blk_GetTextRect(BlkObj: HBLK; LineIndex, WordIndex: integer; out Xpos, Ypos, Width, Height: integer): integer;
begin
  if not loaded or not Assigned(p_Blk_GetTextRect) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_GetTextRect(BlkObj, LineIndex, WordIndex, Xpos, Ypos, Width, Height);
end;

function TNSOCR.Blk_Inversion(BlkObj: HBLK; Inversion: integer): integer;
begin
  if not loaded or not Assigned(p_Blk_Inversion) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_Inversion(BlkObj, Inversion);
end;

function TNSOCR.Blk_Rotation(BlkObj: HBLK; Rotation: integer): integer;
begin
  if not loaded or not Assigned(p_Blk_Rotation) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_Rotation(BlkObj, Rotation);
end;

function TNSOCR.Blk_Mirror(BlkObj: HBLK; Mirror: integer): integer;
begin
  if not loaded or not Assigned(p_Blk_Mirror) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_Mirror(BlkObj, Mirror);
end;

function TNSOCR.Blk_IsWordInDictionary(BlkObj: HBLK; LineIndex, WordIndex: integer): integer;
begin
  if not loaded or not Assigned(p_Blk_IsWordInDictionary) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_IsWordInDictionary(BlkObj, LineIndex, WordIndex);
end;

function TNSOCR.Blk_SetRect(BlkObj: HBLK; Xpos, Ypos, Width, Height: integer): integer;
begin
  if not loaded or not Assigned(p_Blk_SetRect) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_SetRect(BlkObj, Xpos, Ypos, Width, Height);
end;

function TNSOCR.Blk_GetWordQual(BlkObj: HBLK; LineIndex, WordIndex: integer): integer;
begin
  if not loaded or not Assigned(p_Blk_GetWordQual) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_GetWordQual(BlkObj, LineIndex, WordIndex);
end;

function TNSOCR.Blk_GetWordFontColor(BlkObj:HBLK; LineIndex:integer; WordIndex:integer):integer;
begin
  if not loaded or not Assigned(p_Blk_GetWordFontColor) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_GetWordFontColor(BlkObj, LineIndex, WordIndex);
end;

function TNSOCR.Blk_GetWordFontSize(BlkObj:HBLK; LineIndex:integer; WordIndex:integer):integer;
begin
  if not loaded or not Assigned(p_Blk_GetWordFontSize) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_GetWordFontSize(BlkObj, LineIndex, WordIndex);
end;

function TNSOCR.Blk_GetWordFontStyle(BlkObj:HBLK; LineIndex:integer; WordIndex:integer):integer;
begin
  if not loaded or not Assigned(p_Blk_GetWordFontStyle) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_GetWordFontStyle(BlkObj, LineIndex, WordIndex);
end;

function TNSOCR.Blk_GetBackgroundColor(BlkObj:HBLK):integer;
begin
  if not loaded or not Assigned(p_Blk_GetBackgroundColor) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_GetBackgroundColor(BlkObj);
end;

function TNSOCR.Blk_SetWordRegEx(BlkObj: HBLK; LineIndex, WordIndex: integer; RegEx: widestring; Flags: integer): integer;
begin
  if not loaded or not Assigned(p_Blk_SetWordRegEx) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_SetWordRegEx(BlkObj, LineIndex, WordIndex, PWideChar(RegEx), Flags);
end;

function TNSOCR.Blk_GetBarcodeCnt(BlkObj:HBLK):integer;
begin
  if not loaded or not Assigned(p_Blk_GetBarcodeCnt) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_GetBarcodeCnt(BlkObj);
end;

function TNSOCR.Blk_GetBarcodeText(BlkObj:HBLK; BarcodeInd:integer; out TextStr:widestring):integer;
var txt: PWCHAR;
    cnt:integer;
begin
  if not loaded or not Assigned(p_Blk_GetBarcodeText) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  //get text length
  cnt := p_Blk_GetBarcodeText(BlkObj, BarcodeInd, nil, 0);
  if cnt > ERROR_FIRST then
    begin
      result := cnt;
      exit;
    end;
  // get buffer size plus terminating NULL character
  // allocate unicode buffer text length plus null-terminated zero
  inc(cnt);
  getmem(txt, 2 * cnt);
  cnt := p_Blk_GetBarcodeText(BlkObj, BarcodeInd, txt, cnt); // MaxLen must include terminating NULL character

  TextStr := txt;
  freemem(txt);
  result := cnt;
end;

function TNSOCR.Blk_GetBarcodeRect(BlkObj:HBLK; BarcodeInd:integer; out Xpos:integer; out Ypos:integer; out Width:integer; out Height:integer):integer;
begin
  if not loaded or not Assigned(p_Blk_GetBarcodeRect) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_GetBarcodeRect(BlkObj, BarcodeInd, Xpos, Ypos, Width, Height);
end;

function TNSOCR.Blk_GetBarcodeType(BlkObj:HBLK; BarcodeInd:integer):integer;
begin
  if not loaded or not Assigned(p_Blk_GetBarcodeType) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Blk_GetBarcodeType(BlkObj, BarcodeInd);
end;


///////////////////////

function TNSOCR.Svr_Create(CfgObj:HCFG; Format:integer; out SvrObj:HSVR):integer;
begin
  if not loaded or not Assigned(p_Svr_Create) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Svr_Create(CfgObj, Format, SvrObj);
end;

function TNSOCR.Svr_Destroy(SvrObj:HSVR):integer;
begin
  if not loaded or not Assigned(p_Svr_Destroy) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Svr_Destroy(SvrObj);
end;

function TNSOCR.Svr_NewDocument(SvrObj:HSVR):integer;
begin
  if not loaded or not Assigned(p_Svr_NewDocument) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Svr_NewDocument(SvrObj);
end;

function TNSOCR.Svr_AddPage(SvrObj:HSVR; ImgObj:HIMG; Flags:integer):integer;
begin
  if not loaded or not Assigned(p_Svr_AddPage) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Svr_AddPage(SvrObj, ImgObj, Flags);
end;

function TNSOCR.Svr_SaveToFile(SvrObj:HSVR; FileName:widestring):integer;
begin
  if not loaded or not Assigned(p_Svr_SaveToFile) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Svr_SaveToFile(SvrObj, PWideChar(FileName));
end;

function TNSOCR.Svr_SaveToMemory(SvrObj: HSVR; Buffer: pointer; BufferSize: integer): integer;
begin
  if not loaded or not Assigned(p_Svr_SaveToMemory) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Svr_SaveToMemory(SvrObj, Buffer, BufferSize);
end;

function TNSOCR.Svr_GetText(SvrObj: HSVR; PageIndex: integer; out TextStr: widestring): integer;
var txt: PWCHAR;
    cnt:integer;
begin
  if not loaded or not Assigned(p_Svr_GetText) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  //get text length
  cnt := p_Svr_GetText(SvrObj, PageIndex, nil, 0);
  if cnt > ERROR_FIRST then
    begin
      result := cnt;
      exit;
    end;
  // get buffer size plus terminating NULL character
  // allocate unicode buffer text length plus null-terminated zero
  inc(cnt);
  getmem(txt, 2 * cnt);
  cnt := p_Svr_GetText(SvrObj, PageIndex, txt, cnt); // MaxLen must include terminating NULL character

  TextStr := txt;
  freemem(txt);
  result := cnt;
end;

function TNSOCR.Svr_SetDocumentInfo(SvrObj:HSVR; InfoID:integer; InfoStr:widestring):integer;
begin
  if not loaded or not Assigned(p_Svr_SetDocumentInfo) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Svr_SetDocumentInfo(SvrObj, InfoID, PWideChar(InfoStr));
end;

///////////////////////

function TNSOCR.Scan_Create(CfgObj:HCFG; out ScanObj:HSCAN):integer;
begin
  if not loaded or not Assigned(p_Scan_Create) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Scan_Create(CfgObj, ScanObj);
end;

function TNSOCR.Scan_Enumerate(ScanObj:HSCAN; out ScannerNames:widestring; Flags:integer):integer;
var txt: PWCHAR;
    cnt:integer;
begin
  if not loaded or not Assigned(p_Scan_Enumerate) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  //get text length
  cnt := p_Scan_Enumerate(ScanObj, nil, 0, Flags);
  if cnt > ERROR_FIRST then
    begin
      result := cnt;
      exit;
    end;
  // get buffer size plus terminating NULL character
  // allocate unicode buffer text length plus null-terminated zero
  inc(cnt);
  getmem(txt, 2 * cnt);
  txt[0] := #0;
  cnt := p_Scan_Enumerate(ScanObj, txt, cnt, Flags); // MaxLen must include terminating NULL character

  ScannerNames := txt;
  freemem(txt);
  result := cnt;
end;

function TNSOCR.Scan_ScanToImg(ScanObj:HSCAN; ImgObj:HIMG; ScannerIndex:integer; Flags:integer):integer;
begin
  if not loaded or not Assigned(p_Scan_ScanToImg) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Scan_ScanToImg(ScanObj, ImgObj, ScannerIndex, Flags);
end;

function TNSOCR.Scan_ScanToFile(ScanObj: HSCAN; FileName: widestring; ScannerIndex, Flags: integer): integer;
begin
  if not loaded or not Assigned(p_Scan_ScanToFile) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Scan_ScanToFile(ScanObj, PWideChar(FileName), ScannerIndex, Flags);
end;

function TNSOCR.Scan_Destroy(ScanObj:HSCAN):integer;
begin
  if not loaded or not Assigned(p_Scan_Destroy) then
    begin
      result := ERROR_DLLNOTLOADED;
      exit;
    end;

  result := p_Scan_Destroy(ScanObj);
end;



end.