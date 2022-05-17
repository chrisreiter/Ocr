unit fmMainU;

                              interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, NSOCR, ExtCtrls, StdCtrls, ComCtrls, Menus, ShellAPI,
  Vcl.Imaging.pngimage;

type
  TfmMain = class(TForm)
    pnTop: TPanel;
    pnText: TPanel;
    pnImg: TPanel;
    Splitter1: TSplitter;
    mmText: TMemo;
    bkOpen: TButton;
    bkOCR: TButton;
    pbImg: TPaintBox;
    stBar: TStatusBar;
    opFile: TOpenDialog;
    lbWait: TLabel;
    Label1: TLabel;
    edPage: TEdit;
    lbPages: TLabel;
    bkSetPage: TButton;
    Label2: TLabel;
    cbScale: TComboBox;
    pmBlock: TPopupMenu;
    nnTypeOCRText: TMenuItem;
    nnTypeICRDigit: TMenuItem;
    N1: TMenuItem;
    nnDeleteBlock: TMenuItem;
    bkHelp: TButton;
    gbLanguages: TGroupBox;
    nnTypeClear: TMenuItem;
    cbExactCopy: TCheckBox;
    nnTypePicture: TMenuItem;
    Panel1: TPanel;
    cbBin: TCheckBox;
    N2: TMenuItem;
    nnInvert: TMenuItem;
    nnRotate90: TMenuItem;
    nnRotate180: TMenuItem;
    nnRotate270: TMenuItem;
    nnMirrorH: TMenuItem;
    nnMirrorV: TMenuItem;
    nnTypeZoning: TMenuItem;
    bkLoadBlocks: TButton;
    bkSaveBlocks: TButton;
    opBlocks: TOpenDialog;
    svBlocks: TSaveDialog;
    bkClearBlocks: TButton;
    bkDetectBlocks: TButton;
    bkSave: TButton;
    svFile: TSaveDialog;
    bkScan: TButton;
    nnTypeOCRDigit: TMenuItem;
    nnTypeBarCode: TMenuItem;
    cbPixLines: TCheckBox;
    nnTypeTable: TMenuItem;
    bkLanguages: TButton;
    bkOptions: TButton;
    N3: TMenuItem;
    nnSetRegExp: TMenuItem;
    nnSaveAsImage: TMenuItem;
    N4: TMenuItem;
    svImg: TSaveDialog;
    nnTypeMRZ: TMenuItem;
    cbCharRects: TCheckBox;
    procedure FormCreate(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure bkOpenClick(Sender: TObject);
    procedure pnImgResize(Sender: TObject);
    procedure pbImgPaint(Sender: TObject);
    procedure pbImgMouseDown(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure pbImgMouseUp(Sender: TObject; Button: TMouseButton;
      Shift: TShiftState; X, Y: Integer);
    procedure pbImgMouseMove(Sender: TObject; Shift: TShiftState;
      X, Y: Integer);
    procedure bkOCRClick(Sender: TObject);
    procedure bkSetPageClick(Sender: TObject);
    procedure cbScaleClick(Sender: TObject);
    procedure pmBlockPopup(Sender: TObject);
    procedure nnDeleteBlockClick(Sender: TObject);
    procedure nnTypeOCRTextClick(Sender: TObject);
    procedure nnTypeICRDigitClick(Sender: TObject);
    procedure FormShow(Sender: TObject);
    procedure bkHelpClick(Sender: TObject);
    procedure nnTypeClearClick(Sender: TObject);
    procedure cbExactCopyClick(Sender: TObject);
    procedure nnTypePictureClick(Sender: TObject);
    procedure cbBinClick(Sender: TObject);
    procedure nnInvertClick(Sender: TObject);
    procedure nnRotate90Click(Sender: TObject);
    procedure nnRotate180Click(Sender: TObject);
    procedure nnRotate270Click(Sender: TObject);
    procedure nnMirrorHClick(Sender: TObject);
    procedure nnMirrorVClick(Sender: TObject);
    procedure nnTypeZoningClick(Sender: TObject);
    procedure bkLoadBlocksClick(Sender: TObject);
    procedure bkSaveBlocksClick(Sender: TObject);
    procedure bkClearBlocksClick(Sender: TObject);
    procedure bkDetectBlocksClick(Sender: TObject);
    procedure bkSaveClick(Sender: TObject);
    procedure bkScanClick(Sender: TObject);
    procedure nnTypeOCRDigitClick(Sender: TObject);
    procedure nnTypeBarCodeClick(Sender: TObject);
    procedure cbPixLinesClick(Sender: TObject);
    procedure nnTypeTableClick(Sender: TObject);
    procedure bkLanguagesClick(Sender: TObject);
    procedure bkOptionsClick(Sender: TObject);
    procedure nnSetRegExpClick(Sender: TObject);
    procedure nnSaveAsImageClick(Sender: TObject);
    procedure nnTypeMRZClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
    NSOCR: TNSOCR;

    CfgObj: HCFG;
    OcrObj: HOCR;
    ImgObj: HIMG;
    SvrObj: HSVR;
    ScanObj: HSCAN;

    bmp: tbitmap; // main bmp with the image
    Frame: TRect;
    Dwn: boolean;

    procedure ShowError(FuncName: string; err: Integer);
    procedure LoadDefaultImage;

    procedure AdjustDocScale();
    function IsImgLoaded(): boolean;
    function GetCurDocScale(): double;
    procedure ShowImage();
    procedure DrawPixLines(bmp2:tbitmap);
    procedure DrawCharRects(bmp2:tbitmap);

    procedure ShowHelp(section: string);
    procedure ShowText;

    procedure DoImageLoaded;
    procedure ShowImageParams;

    procedure ProcessEntireDocument;
  end;

var
  fmMain: TfmMain;

                               implementation

uses fmScanU, fmWaitU, fmLanguagesU, fmOptionsU;
{$R *.dfm}

function ExecuteFile(Wnd: hwnd; const FileName, Params, DefaultDir: string;
  ShowCmd: Integer; Explore: boolean = false): THandle;
var
  zFileName, zParams, zDir: array [0 .. 256] of Char;
  com: string;
begin
  result := 0;
  if not Explore then
    com := 'open'
  else
    com := 'explore';
  try
    result := ShellExecute(0, pchar(com), StrPCopy(zFileName, FileName),
      StrPCopy(zParams, Params), StrPCopy(zDir, DefaultDir), ShowCmd);
  except
  end;
end;

procedure TfmMain.FormCreate(Sender: TObject);
var
  dir, fn: string;
  res: Integer;
  val: widestring;
begin
  // load NSOCR.dll dynamically. We don't know exact path so check "bin" in all parents folders

  NSOCR := TNSOCR.Create('NSOCR.dll'); // try default folders
  if not NSOCR.IsDllLoaded then // failed, check "bin" folder in all parent folders
  begin
    dir := ExtractFileDir(application.ExeName);
    while true do
    begin
      if dir[length(dir)] <> '\' then
        fn := dir + '\'
      else
        fn := dir;

      if sizeof(self) = 4 then
        fn := fn + 'bin\NSOCR.dll'
       else
        fn := fn + 'bin_64\NSOCR.dll'; //Our application is x64

      if NSOCR.TryLoadDLL(fn) then
        break;

      if (length(dir) < 3) or (dir[length(dir) - 1] = ':') then
        break;
      dir := ExtractFileDir(dir);
    end;

    if not NSOCR.IsDllLoaded then
    begin
      ShowMessage('Cannot locate NSOCR.dll!');
      halt;
    end;
  end;
  /// ////////////

  NSOCR.Engine_GetVersion(val);
  Caption := Caption + ' [ NSOCR version: ' + val + ' ]';

  NSOCR.Engine_SetLicenseKey(''); //required for licensed version only

  if true {false} then //change to "false" to reduce code and initialize engine + create main objects in one line
    begin
      NSOCR.Engine_Initialize(); // initialize engine. Called only once
      // NsOCR.Engine_SetDataDirectory();// - not necessary in most cases, default is the folder with NSOCR.dll

      NSOCR.Cfg_Create(CfgObj); // create Configure object

      res := NSOCR.Cfg_LoadOptions(CfgObj, 'Config.dat'); // load options, if path not specified, folder with NSOCR.dll will be checked

      if res > ERROR_FIRST then
        ShowError('Cfg_LoadOptions', res);

      NSOCR.Ocr_Create(CfgObj, OcrObj); // create OCR object, it will use CfgObj for settings
      NSOCR.Img_Create(OcrObj, ImgObj); // create Img object that can be OCRed by OcrObj object
    end
   else //do the same in one line
    begin
      res := NSOCR.Engine_InitializeAdvanced(CfgObj, OcrObj, ImgObj);
      if res > ERROR_FIRST then
        ShowError('Engine_InitializeAdvanced', res);
    end;

  NSOCR.Scan_Create(CfgObj, ScanObj); // create SCAN object

  bmp := tbitmap.Create;
  Dwn := false;

  // load some settings to GUI
  if NSOCR.Cfg_GetOption(CfgObj, BT_DEFAULT, 'ImgAlizer/AutoScale', val) < ERROR_FIRST then
    cbScale.Enabled := strtointdef(val, 0) = 1;
end;

procedure TfmMain.FormShow(Sender: TObject);
begin
  LoadDefaultImage;
end;

procedure TfmMain.FormClose(Sender: TObject; var Action: TCloseAction);
begin
  // release engine and all created objects. Called only once
  NSOCR.Engine_Uninitialize();
end;

procedure TfmMain.ShowError(FuncName: string; err: Integer);
begin
  ShowMessage(FuncName + ' Error #' + inttohex(err, 8));
end;

procedure TfmMain.ShowHelp(section: string);
var
  fn, wdir: string;
  buf: array [0 .. 256] of Char;
begin
  fn := extractfilepath(application.ExeName) + '../../Docs/NSOCR.chm';
  if not fileexists(fn) then
  begin
    ShowMessage('Cannot locate NSOCR.chm');
    exit;
  end;
  if section <> '' then
    fn := fn + '::/' + section;
  getwindowsdirectory(buf, 256);
  wdir := buf;
  wdir := wdir + '\hh.exe';
  ShellExecute(Handle, 'open', @wdir[1], @fn[1], nil, SW_SHOWNORMAL);
end;

procedure TfmMain.bkOpenClick(Sender: TObject);
var
  res, sz: Integer;
  s: string;
  LoadMode: Integer;
  f, rd: cardinal;
  p: pointer;
  bm: tbitmap;
  fn: widestring;
  // BlkObj, x, y:integer;
begin
  bkOCR.Font.Style := [];
  if Sender <> nil then
    if not opFile.Execute then
      exit;

  if cbScale.Enabled then
  begin
    if cbScale.ItemIndex < 1 then // autoscaling
    begin
      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'ImgAlizer/AutoScale', '1');
      // default scale if cannot detect it automatically
      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'ImgAlizer/ScaleFactor', '1.0');
    end
    else // fixed scaling
    begin
      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'ImgAlizer/AutoScale', '0');
      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'ImgAlizer/ScaleFactor', cbScale.Items[cbScale.ItemIndex]);
    end;
  end;

  // It is possible to load image from file, from memory or from bitmap data in memory, demostrate all ways
  LoadMode := 0; // 0 - from file; 1 - from memory; 2 - from memory bitmap
  fn := opFile.FileName;
  if LoadMode = 0 then // load image to OCR engline from image file
  begin
    res := NSOCR.Img_LoadFile(ImgObj, fn);
  end
  else if LoadMode = 1 then // load image to OCR engline from image in memory
  begin
    f := CreateFile(@fn[1], GENERIC_READ, FILE_SHARE_READ, nil, OPEN_EXISTING,
      FILE_ATTRIBUTE_NORMAL, 0);
    sz := GetFileSize(f, nil);
    getmem(p, sz);
    ReadFile(f, p^, sz, rd, nil);
    CloseHandle(f);
    res := NSOCR.Img_LoadFromMemory(ImgObj, p, sz);
  end
  else // load image to OCR engine from memory bitmap
  begin
    if ansiuppercase(ExtractFileExt(opFile.FileName)) <> '.BMP' then
    begin
      ShowMessage('For this mode only BMP files are supported!');
      exit;
    end;
    bm := tbitmap.Create;
    bm.LoadFromFile(opFile.FileName);
    bm.PixelFormat := pf24bit;
    p := bm.ScanLine[bm.Height - 1];
    res := NSOCR.Img_LoadBmpData(ImgObj, p, bm.Width, bm.Height, BMP_BOTTOMTOP or BMP_24BIT, 0);
    bm.Free;
  end;
  /// /

  if res > ERROR_FIRST then
  begin
    if res = ERROR_CANNOTLOADGS then // cannot load GhostScript to support PDF
    begin
      s :=
        'GhostSript library is needed to support PDF files. Just download and install it with default settings. Do you want to download GhostScript now?';
      if MessageDlg(s, mtConfirmation, [mbYes, mbCancel], 0) <> mrYes then
        exit;
      ExecuteFile(self.Handle,
        'http://www.nicomsoft.com/files/ocr/ghostscript.htm', 'open', '',
        SW_SHOW);
    end
    else
      ShowError('Img_LoadFile', res);
    exit;
  end;

  DoImageLoaded;

  //sample: set regexp for entire image    NsOCR.Blk_SetWordRegEx(ImgObj, -1, -1, '.*', 0);
end;

procedure TfmMain.bkOptionsClick(Sender: TObject);
begin
  fmOptions.ShowModal;
end;

procedure TfmMain.DoImageLoaded;
var
  res, w, h: Integer;

//  sz:integer;
//  buf:pointer;
//  fs:tfilestream;
begin
  // clear bitmaps for old image
  bmp.Width := 0;
  bmp.Height := 0;

  // check if image has multiple page and ask user if he wants process and save all pages automatically
  res := NSOCR.Img_GetPageCount(ImgObj);
  if res > ERROR_FIRST then
  begin
    ShowError('Img_GetPageCount', res);
    exit;
  end;
  if res > 1 then
    if MessageDlg('Image contains multiple pages (' + inttostr(res) +
        '). Do you want to process and save all pages automatically?',
      mtConfirmation, [mbYes, mbCancel], 0) = mrYes then
    begin
      ProcessEntireDocument;
      bkOCR.Enabled := false;
      pbImg.Repaint;
      exit;
    end;

  bkOCR.Enabled := true;
  bkLoadBlocks.Enabled := true;
  bkSaveBlocks.Enabled := true;
  bkClearBlocks.Enabled := true;
  bkDetectBlocks.Enabled := true;

  // NSOCR.Img_AddBlock(ImgObj, 200, 100, 3, 3, BlkObj);

  // apply image inversion/rotation/mirroring/scaling,
  // then binarize image, deskew, lines removing etc,
  // everything except OCR itself because an user must have a chance to select additional blocks for OCR
  res := NSOCR.Img_OCR(ImgObj, OCRSTEP_FIRST, OCRSTEP_ZONING - 1, 0);
  if res > ERROR_FIRST then
  begin
    ShowError('Img_OCR: OCRSTEP_FIRST...OCRSTEP_ZONING-1', res);
    exit;
  end;

  ShowImageParams;

  res := NSOCR.Img_GetSkewAngle(ImgObj);
  if res > ERROR_FIRST then
  begin
    ShowError('Img_GetSkewAngle', res);
    stBar.Panels[1].Text := '';
  end
  else
    stBar.Panels[1].Text := 'Skew angle (degrees): ' + floattostr(res / 1000);


//now you can save image after preprocessing to file or memory:
//to file
//    NSOCR.Img_SaveToFile(ImgObj, 'c:\image.jpg', IMG_FORMAT_JPEG {or IMG_FORMAT_FLAG_BINARIZED}, 80); //80% quality
//to memory
{
  sz := NSOCR.Img_SaveToMemory(ImgObj, nil, 0, IMG_FORMAT_JPEG, 80); //get size
  getmem(buf, sz);
  NSOCR.Img_SaveToMemory(ImgObj, buf, sz, IMG_FORMAT_JPEG, 80); //save to buffer
  //do something with "buf"
  fs:=tfilestream.Create('c:\a.jpg', fmCreate or fmOpenWrite);
  fs.WriteBuffer(buf^, sz);
  fs.Free;
  freemem(buf);
}
  //
  edPage.Text := '1';
  lbPages.Caption := 'of ' + inttostr(NSOCR.Img_GetPageCount(ImgObj));

  NSOCR.Img_GetSize(ImgObj, w, h);

  Frame.left := 0;
  Frame.top := 0;
  Frame.right := w;
  Frame.bottom := h;
  AdjustDocScale();

  bkOCR.Enabled := true;
  bkSave.Enabled := false;
  mmText.Clear;
  pbImg.Repaint;
end;

procedure TfmMain.bkSetPageClick(Sender: TObject);
var
  n, cnt, w, h, res: Integer;
begin
  if not IsImgLoaded then
    exit;
  cnt := NSOCR.Img_GetPageCount(ImgObj);
  n := strtointdef(edPage.Text, 1) - 1;
  if n < 0 then
    n := 0;
  if n >= cnt then
    n := cnt - 1;

  bkSave.Enabled := false;
  res := NSOCR.Img_SetPage(ImgObj, n);
  if res > ERROR_FIRST then
  begin
    ShowError('Img_SetPage', res);
    exit;
  end;

  // now apply image scaling, binarize image, deskew etc,
  // everything except OCR itself because an user must have a chance to select blocks for OCR
  NSOCR.Img_OCR(ImgObj, OCRSTEP_FIRST, OCRSTEP_ZONING - 1, 0);

  ShowImageParams;

  edPage.Text := inttostr(n + 1);
  NSOCR.Img_GetSize(ImgObj, w, h);

  Frame.left := 0;
  Frame.top := 0;
  Frame.right := w;
  Frame.bottom := h;
  AdjustDocScale();
end;

procedure TfmMain.cbBinClick(Sender: TObject);
begin
  AdjustDocScale; // repaint
end;


procedure TfmMain.cbExactCopyClick(Sender: TObject);
begin
  ShowText;
end;

procedure TfmMain.cbPixLinesClick(Sender: TObject);
begin
  AdjustDocScale; // repaint
end;

procedure TfmMain.cbScaleClick(Sender: TObject);
begin
  bkOCR.Enabled := false;
  bkOpen.Click;
  bkOpen.SetFocus;
end;

function TfmMain.IsImgLoaded: boolean;
var
  w, h: Integer;
begin
  if NSOCR.Img_GetSize(ImgObj, w, h) > ERROR_FIRST then
    result := false
  else
    result := (w > 0) and (h > 0);
end;

procedure TfmMain.pbImgMouseDown(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
var
  k: double;
  w, h, n, i, cnt, minsize, sz: Integer;
  BlkObj: Integer;
  Xpos, Ypos, Width, Height: Integer;
  pt: TPoint;
  r: TRect;
begin
  if Button = mbRight then // right-click menu
  begin
    k := GetCurDocScale();
    cnt := NSOCR.Img_GetBlockCnt(ImgObj);
    n := -1;
    minsize := -1;
    for i := 0 to cnt - 1 do
    begin
      NSOCR.Img_GetBlock(ImgObj, i, BlkObj);
      NSOCR.Blk_GetRect(BlkObj, Xpos, Ypos, Width, Height);
      r.left := round(k * Xpos);
      r.top := round(k * Ypos);
      r.right := round(k * (Xpos + Width - 1));
      r.bottom := round(k * (Ypos + Height - 1));
      if PtInRect(r, point(X, Y)) then
      begin
        // need to find smallest block because blocks may overlap
        if Width < Height then
          sz := Width
        else
          sz := Height;

        if (minsize = -1) or (sz < minsize) then
        begin
          minsize := sz;
          n := i;
        end;
      end;
    end;

    if n = -1 then
      exit; // block not found
    pmBlock.Tag := n; // remember block index
    pt := pbImg.ClientToScreen(point(X, Y));
    pmBlock.Popup(pt.X, pt.Y);
    exit;
  end;

  if Button <> mbLeft then
    exit;

  if Dwn then
    exit;
  Dwn := true;

  if not IsImgLoaded then
    exit;

  k := GetCurDocScale();
  NSOCR.Img_GetSize(ImgObj, w, h);

  Frame.left := trunc(1 / k * X);
  if Frame.left < 0 then
    Frame.left := 0;
  if Frame.left > w then
    Frame.left := w;
  Frame.top := trunc(1 / k * Y);
  if Frame.top < 0 then
    Frame.top := 0;
  if Frame.top > h then
    Frame.top := h;

  Frame.right := Frame.left;
  Frame.bottom := Frame.top;

  ShowImage();
end;

procedure TfmMain.pbImgMouseMove(Sender: TObject; Shift: TShiftState;
  X, Y: Integer);
var
  k: double;
  w, h: Integer;
begin
  if not IsImgLoaded() then
    exit;

  if not Dwn then
    exit;

  k := GetCurDocScale();
  NSOCR.Img_GetSize(ImgObj, w, h);

  Frame.right := trunc(1 / k * X);
  if Frame.right < 0 then
    Frame.right := 0;
  if Frame.right > w then
    Frame.right := w;
  Frame.bottom := trunc(1 / k * Y);
  if Frame.bottom < 0 then
    Frame.bottom := 0;
  if Frame.bottom > h then
    Frame.bottom := h;

  ShowImage();
end;

procedure TfmMain.pbImgMouseUp(Sender: TObject; Button: TMouseButton;
  Shift: TShiftState; X, Y: Integer);
var
  BlkObj, w, h, res: Integer;
begin
  if not Dwn then
    exit;
  Dwn := false;

  if NSOCR.Img_GetSize(ImgObj, w, h) > ERROR_FIRST then
    exit;
  if Frame.right >= w then
    Frame.right := w - 1;
  if Frame.bottom >= h then
    Frame.bottom := h - 1;

  w := Frame.right - Frame.left + 1;
  h := Frame.bottom - Frame.top + 1;
  if (w < 8) or (h < 8) then
  begin
    ShowImage();
    exit;
  end;
  res := NSOCR.Img_AddBlock(ImgObj, Frame.left, Frame.top, w, h, BlkObj);
  if res > ERROR_FIRST then
  begin
    ShowError('Img_AddBlock', res);
    exit;
  end;

  // detect text block inversion
  NSOCR.Blk_Inversion(BlkObj, BLK_INVERSE_DETECT);

  //detect text block rotation
  NSOCR.Blk_Rotation(BlkObj, BLK_ROTATE_DETECT);

  // for example, if you know that text in block rotated 10 degrees counterclockwise, you can notify OCR about it:
  // NSOCR.Blk_Rotation(BlkObj, BLK_ROTATE_ANGLE or (10 * 1000)); //rotate block 10degrees clockwise, function accepts angle in 1/1000 degrees
  // Negative values are not allowed, so for 20 degrees counterclockwise use ((360-20) * 1000)

  // for example, to set own allowed chars set only for this block:
  // NsOCR.Blk_SetType(BlkObj, BT_OCRTEXT + 1 * 256);
  // NsOCR.Cfg_setoption(CfgObj, BT_OCRTEXT + 1 * 256, 'Main/EnabledChars', '1234567890-.,');

  ShowImage();
end;

procedure TfmMain.pbImgPaint(Sender: TObject);
begin
  ShowImage;
end;

procedure TfmMain.pnImgResize(Sender: TObject);
begin
  AdjustDocScale();
end;

function TfmMain.GetCurDocScale: double;
var
  w, h, ww, hh: Integer;
  k, kX, kY: double;
begin
  if not IsImgLoaded() then
  begin
    result := 1;
    exit;
  end;

  w := pbImg.Width;
  h := pbImg.Height;
  NSOCR.Img_GetSize(ImgObj, ww, hh);

  kX := w / ww;
  kY := h / hh;

  if kX > kY then
    k := kY
  else
    k := kX;

  result := k;
end;

procedure TfmMain.ShowImage;
var
  k: double;
  r: TRect;
  i, cnt: Integer;
  BlkObj, BlockType: Integer;
  Xpos, Ypos, Width, Height: Integer;
  bmp2:tbitmap;
begin
  if not IsImgLoaded() then
    exit;
  k := GetCurDocScale();

  // temporary bitmap
  bmp2:=tbitmap.Create;
  bmp2.Width := bmp.Width;
  bmp2.Height := bmp.Height;
  bmp2.Canvas.Draw(0, 0, bmp);

  if cbPixLines.Checked then
    DrawPixLines(bmp2);

  if cbCharRects.Checked then
    DrawCharRects(bmp2);

  // draw blocks
  cnt := NSOCR.Img_GetBlockCnt(ImgObj);
  for i := 0 to cnt - 1 do
  begin
    NSOCR.Img_GetBlock(ImgObj, i, BlkObj);
    NSOCR.Blk_GetRect(BlkObj, Xpos, Ypos, Width, Height);

    r.left := trunc(k * Xpos);
    r.top := trunc(k * Ypos);
    r.right := trunc(k * (Xpos + Width)) + 1;
    r.bottom := trunc(k * (Ypos + Height)) + 1;

    BlockType := NSOCR.Blk_GetType(BlkObj) and 255;
    case BlockType of
      BT_OCRTEXT:
        bmp2.Canvas.Pen.Color := clGreen;
      BT_OCRDIGIT:
        bmp2.Canvas.Pen.Color := clLime;
      BT_ICRDIGIT:
        bmp2.Canvas.Pen.Color := clBlue;
      BT_PICTURE:
        bmp2.Canvas.Pen.Color := clAqua;
      BT_CLEAR:
        bmp2.Canvas.Pen.Color := clGray;
      BT_ZONING:
        bmp2.Canvas.Pen.Color := clBlack;
      BT_BARCODE:
        bmp2.Canvas.Pen.Color := clNavy;
      BT_TABLE:
        bmp2.Canvas.Pen.Color := clOlive;
      BT_MRZ:
        bmp2.Canvas.Pen.Color := clBlack;
    end;
    bmp2.Canvas.Pen.Width := 2;
    bmp2.Canvas.Brush.Style := bsClear;
    bmp2.Canvas.Rectangle(r.left, r.top, r.right, r.bottom);
    bmp2.Canvas.Brush.Style := bsSolid;
    bmp2.Canvas.Pen.Width := 1;

    bmp2.Canvas.Brush.Color := clLime;
    bmp2.Canvas.TextOut(r.left, r.top, inttostr(i));
  end;

  // user is creating new block, draw its frame
  if Dwn then
  begin
    r.left := trunc(k * Frame.left);
    r.top := trunc(k * Frame.top);
    r.right := trunc(k * Frame.right);
    r.bottom := trunc(k * Frame.bottom);
    if (r.right > bmp2.Width - 1) then
      r.right := bmp2.Width - 1;
    if (r.bottom > bmp2.Height - 1) then
      r.bottom := bmp2.Height - 1;
    bmp2.Canvas.Brush.Color := clRed;
    bmp2.Canvas.FrameRect(r);
  end;

  pbImg.Canvas.Draw(0, 0, bmp2);
  bmp2.Free;
end;

procedure TfmMain.DrawPixLines(bmp2:tbitmap);
var i, cnt, x1, y1, x2, y2, w:integer;
    k:double;
begin
  k := GetCurDocScale();
  cnt := NsOCR.Img_GetPixLineCnt(ImgObj);
  for i := 0 to cnt - 1 do
    begin
      NsOCR.Img_GetPixLine(ImgObj, i, x1, y1, x2, y2, w);
      x1 := trunc(k * x1);
      y1 := trunc(k * y1);
      x2 := trunc(k * x2);
      y2 := trunc(k * y2);

      bmp2.Canvas.Pen.Width := 2;
      bmp2.Canvas.Pen.Color := clRed;
      bmp2.Canvas.MoveTo(x1, y1);
      bmp2.Canvas.LineTo(x2, y2);
      bmp2.Canvas.Pen.Width := 1;
    end;
end;

procedure TfmMain.DrawCharRects(bmp2: tbitmap);
var bcnt, BlkObj, lcnt, wcnt, ccnt, i, j, k, m, x, y, w, h:integer;
    kf:double;
    r:TRect;
begin
  kf := GetCurDocScale();
  bcnt := NsOCR.Img_GetBlockCnt(ImgObj);
  for i := 0 to bcnt-1 do
    begin
      NsOCR.Img_GetBlock(ImgObj, i, BlkObj);
      lcnt := NsOCR.Blk_GetLineCnt(BlkObj);
      for j := 0 to lcnt-1 do
        begin
          wcnt := NsOCR.Blk_GetWordCnt(BlkObj, j);
          for k := 0 to wcnt-1 do
            begin
              ccnt := NsOCR.Blk_GetCharCnt(BlkObj, j, k);
              for m := 0 to ccnt-1 do
                begin
                  NsOCR.Blk_GetCharRect(BlkObj, j, k, m, x, y, w, h);

                  r.Left := trunc(kf * x);
                  r.Top := trunc(kf * y);
                  r.Right := trunc(kf * (x + w)) + 1;
                  r.Bottom := trunc(kf * (y + h)) + 1;

                  bmp2.Canvas.Brush.Color := clBlue;
                  bmp2.Canvas.FrameRect(r);
                end;
            end;
        end;
    end;
end;

procedure TfmMain.ShowImageParams;
var w, h, dpi, w0, h0, bpp, res:integer;
begin
  //original image properties
  dpi := NSOCR.Img_GetProperty(ImgObj, IMG_PROP_DPIX);
  bpp := NSOCR.Img_GetProperty(ImgObj, IMG_PROP_BPP);
  w0 := NSOCR.Img_GetProperty(ImgObj, IMG_PROP_WIDTH);
  h0 := NSOCR.Img_GetProperty(ImgObj, IMG_PROP_HEIGHT);
  // final size after pre-processing
  NSOCR.Img_GetSize(ImgObj, w, h);

  stBar.Panels[0].Text := '';
  if dpi <> 0 then
    stBar.Panels[0].Text := 'DPI: ' + inttostr(dpi) +', ';

  stBar.Panels[0].Text := stBar.Panels[0].Text + 'BPP: ' + inttostr(bpp) +', ';

  stBar.Panels[0].Text := stBar.Panels[0].Text + 'Size: ' + inttostr(w0) + 'x' + inttostr(h0)
    + ' => ' + inttostr(w) + 'x' + inttostr(h);

  res := NsOCR.Img_GetProperty(ImgObj, IMG_PROP_INVERTED);
  if res = 1 then
    stBar.Panels[4].Text := 'Inverted: Yes'
   else
    stBar.Panels[4].Text := 'Inverted: No';
end;

procedure TfmMain.AdjustDocScale;
var w, h: Integer;
begin
  if (not IsImgLoaded()) then
    exit;

  bmp.Width := 0;
  bmp.Height := 0;
  bmp.Width := pbImg.Width;
  bmp.Height := pbImg.Height;
  w := bmp.Width;
  h := bmp.Height;
  if cbBin.Checked then
    NSOCR.Img_DrawToDC(ImgObj, bmp.Canvas.Handle, 0, 0, w, h, DRAW_BINARY)
  else
    NSOCR.Img_DrawToDC(ImgObj, bmp.Canvas.Handle, 0, 0, w, h, DRAW_NORMAL);

  ShowImage();
end;

procedure TfmMain.bkHelpClick(Sender: TObject);
begin
  ShowHelp('');
end;

procedure TfmMain.bkOCRClick(Sender: TObject);
var
  res: Integer;
  InSameThread: boolean;
 // BlkObj, t, i, n, x, y, w, h: Integer;
//  s:widestring;
begin
  bkOCR.Font.Style := [];
  if not IsImgLoaded() then
  begin
    ShowMessage('Image not loaded!');
    exit;
  end;

  mmText.Clear;
  bkOCR.Enabled := false;
  bkSave.Enabled := false;
  lbWait.Visible := true;
  self.Repaint;

  InSameThread := false; // perform OCR in non-blocking mode
  // InSameThread := true; //uncomment to perform OCR from this thread (GUI will be freezed)

  // perform OCR. Do zoning step for "zone" type blocks that user could add
  if InSameThread then
  begin
    res := NSOCR.Img_OCR(ImgObj, OCRSTEP_ZONING, OCRSTEP_LAST, 0);
    // do in this thread
  end
  else
  begin
    // do it in non-blocking mode and then wait for result
    res := NSOCR.Img_OCR(ImgObj, OCRSTEP_ZONING, OCRSTEP_LAST, OCRFLAG_THREAD);
    if res > ERROR_FIRST then
    begin
      ShowError('Ocr_OcrImg(1)', res);
      exit;
    end;
    fmWait.mode := 0;
    fmWait.ShowModal;
    res := fmWait.res;
  end;

  lbWait.Visible := false;
  bkOCR.Enabled := true;
  bkSave.Enabled := true;

  if res > ERROR_FIRST then
  begin
    if res = ERROR_OPERATIONCANCELLED then
      ShowMessage('Operation was cancelled.')
    else
      ShowError('Ocr_OcrImg(2)', res);
    exit;
  end;

  // NsOCR.Img_GetBlock(ImgObj, 3, BlkObj);
  // NsOCR.Blk_GetCharRect(BlkObj, 0, 0, 0, x, y, w, h);
  // x := NsOCR.Blk_IsWordInDictionary(BlkObj, 0, 0);

  /// /
  cbBinClick(nil); // repaint img (binarized image could change)

  //NSOCR.Img_GetBlock(ImgObj, 0, BlkObj);
  //OCR.Blk_SetWordText(BlkObj, 0, 0, 'userwordtext');

  //NSOCR.Img_GetBlock(ImgObj, 0, BlkObj);
  //NSOCR.Img_SaveToFile({ImgObj}BlkObj, 'c:\a.bmp', IMG_FORMAT_BMP, 0);

  ShowText;

  { NSOCR.Img_GetBlock(ImgObj, 0, BlkObj);
   res := NSOCR.Blk_GetCharCnt(BlkObj, 0, 0);
    //res := NSOCR.Blk_GetCharQual(BlkObj, 0, 0, 0, 0);
    res := NSOCR.Blk_GetWordQual(BlkObj, 0, 0);
    if res > 100 then halt; { }

   //get position of first char
   { NSOCR.Img_GetBlock(ImgObj, 0, BlkObj);
    NSOCR.Blk_GetCharRect(BlkObj, 0, 0, 0, x, y, w, h); { }

  // get font color of first word
  // NSOCR.Img_GetBlock(ImgObj, 0, BlkObj);
  // x := NSOCR.Blk_GetWordFontColor(BlkObj, 0, 0);
  // get block background color
  // x := NSOCR.Blk_GetBackgroundColor(BlkObj);

  // get font size of first block
  // NSOCR.Img_GetBlock(ImgObj, 0, BlkObj);
  // x := NSOCR.Blk_GetWordFontSize(BlkObj, -1, -1);

  // get font style of first word
  // NSOCR.Img_GetBlock(ImgObj, 0, BlkObj);
  // x := NSOCR.Blk_GetWordFontStyle(BlkObj, 0, 0);

  //NsOCR.Img_Unload(ImgOBj); //unload image
end;

procedure TfmMain.ShowText;
var
  flags: Integer;
  txt: widestring;
begin
  if cbExactCopy.Checked then
    flags := FMT_EXACTCOPY
  else
    flags := FMT_EDITCOPY;

  NSOCR.Img_GetImgText(ImgObj, txt, flags);

  mmText.Text := txt;
end;

procedure TfmMain.pmBlockPopup(Sender: TObject);
var
  BlkObj, BlockType, r, m: Integer;
begin
  NSOCR.Img_GetBlock(ImgObj, pmBlock.Tag, BlkObj);
  BlockType := NSOCR.Blk_GetType(BlkObj);
  nnTypeOCRText.Checked := BlockType = BT_OCRTEXT;
  nnTypeICRDigit.Checked := BlockType = BT_ICRDIGIT;
  nnTypePicture.Checked := BlockType = BT_PICTURE;
  nnTypeClear.Checked := BlockType = BT_CLEAR;
  nnTypeZoning.Checked := BlockType = BT_ZONING;
  nnTypeOCRDigit.Checked := BlockType = BT_OCRDIGIT;
  nnTypeBarCode.Checked := BlockType = BT_BARCODE;
  nnTypeTable.Checked := BlockType = BT_TABLE;
  nnTypeMRZ.Checked := BlockType = BT_MRZ;

  nnInvert.Checked := NSOCR.Blk_Inversion(BlkObj, BLK_INVERSE_GET) <> 0;
  r := NSOCR.Blk_Rotation(BlkObj, BLK_ROTATE_GET);
  nnRotate90.Checked := r = BLK_ROTATE_270;
  nnRotate180.Checked := r = BLK_ROTATE_180;
  nnRotate270.Checked := r = BLK_ROTATE_90;
  m := NSOCR.Blk_Mirror(BlkObj, BLK_MIRROR_GET);
  nnMirrorH.Checked := (m and BLK_MIRROR_H) <> 0;
  nnMirrorV.Checked := (m and BLK_MIRROR_V) <> 0;
end;

procedure TfmMain.nnDeleteBlockClick(Sender: TObject);
var
  BlkObj: Integer;
begin
  NSOCR.Img_GetBlock(ImgObj, pmBlock.Tag, BlkObj);
  NSOCR.Img_DeleteBlock(ImgObj, BlkObj);
  ShowImage;
end;

procedure TfmMain.nnInvertClick(Sender: TObject);
var
  BlkObj: Integer;
begin
  nnInvert.Checked := not nnInvert.Checked;
  NSOCR.Img_GetBlock(ImgObj, pmBlock.Tag, BlkObj);
  NSOCR.Blk_Inversion(BlkObj, ord(nnInvert.Checked));
end;

procedure TfmMain.nnMirrorHClick(Sender: TObject);
var
  BlkObj, m: Integer;
begin
  nnMirrorH.Checked := not nnMirrorH.Checked;
  NSOCR.Img_GetBlock(ImgObj, pmBlock.Tag, BlkObj);
  m := NSOCR.Blk_Mirror(BlkObj, -1);
  if nnMirrorH.Checked then
    m := m or BLK_MIRROR_H
  else
    m := m and not BLK_MIRROR_H;
  NSOCR.Blk_Mirror(BlkObj, m);
end;

procedure TfmMain.nnMirrorVClick(Sender: TObject);
var
  BlkObj, m: Integer;
begin
  nnMirrorV.Checked := not nnMirrorV.Checked;
  NSOCR.Img_GetBlock(ImgObj, pmBlock.Tag, BlkObj);
  m := NSOCR.Blk_Mirror(BlkObj, -1);
  if nnMirrorV.Checked then
    m := m or BLK_MIRROR_V
  else
    m := m and not BLK_MIRROR_V;
  NSOCR.Blk_Mirror(BlkObj, m);
end;

procedure TfmMain.nnRotate90Click(Sender: TObject);
var
  BlkObj: Integer;
begin
  nnRotate90.Checked := not nnRotate90.Checked;
  NSOCR.Img_GetBlock(ImgObj, pmBlock.Tag, BlkObj);
  if not nnRotate90.Checked then
    NSOCR.Blk_Rotation(BlkObj, BLK_ROTATE_NONE)
  else
    NSOCR.Blk_Rotation(BlkObj, BLK_ROTATE_270);
end;

procedure TfmMain.nnSaveAsImageClick(Sender: TObject);
var BlkObj, res, format, flags:integer;
begin
  NSOCR.Img_GetBlock(ImgObj, pmBlock.Tag, BlkObj);

  if not svImg.Execute then
    exit;

  format := 0;
  case svImg.FilterIndex of
    0: format := IMG_FORMAT_BMP;
    1: format := IMG_FORMAT_JPEG;
    2: format := IMG_FORMAT_PNG;
    3: format := IMG_FORMAT_TIFF;
  end;

  flags := 0;
  if svImg.FilterIndex = 1 then //jpeg
    flags := 85; //jpeg quality

  res := NSOCR.Img_SaveToFile(BlkObj, svImg.FileName, format, flags);

  if res > ERROR_FIRST then
    ShowError('Img_SaveToFile', res);
end;

procedure TfmMain.nnSetRegExpClick(Sender: TObject);
var
  BlkObj: Integer;
  s:string;
//  teststr:widestring;
begin
  NSOCR.Img_GetBlock(ImgObj, pmBlock.Tag, BlkObj);
  s := '';
  if not InputQuery('Set regular expression (for entire area)', 'New regular expression:', s) then
    exit;
  if NsOCR.Blk_SetWordRegEx(BlkObj, -1, -1, s, REGEX_SET) > 0 then
    begin
      ShowMessage('Regular Expression syntax error!');
      exit;
    end;

{
  //set regular expression - the same expression for all words in block
  //regular expression for amounts like '123.00$', '500' or '-100,55'
  if NsOCR.Blk_SetWordRegEx(BlkObj, -1, -1, '(|-)[0-9]+(\.|,)?([0-9][0-9])?\$?', REGEX_SET) > 0 then
    ShowMessage('Regular Expression syntax error!');

  //now we can check any string if it is good
  teststr := '55.30$';
  if NsOCR.Blk_SetWordRegEx(BlkObj, -1, -1, teststr, REGEX_CHECK) <> 1 then
    ShowMessage('String "' + teststr + '" does not comply with current regular expression!')
   else
    ShowMessage('String "' + teststr + '" is good!');
}
end;

procedure TfmMain.nnRotate180Click(Sender: TObject);
var
  BlkObj: Integer;
begin
  nnRotate180.Checked := not nnRotate180.Checked;
  NSOCR.Img_GetBlock(ImgObj, pmBlock.Tag, BlkObj);
  if not nnRotate180.Checked then
    NSOCR.Blk_Rotation(BlkObj, BLK_ROTATE_NONE)
  else
    NSOCR.Blk_Rotation(BlkObj, BLK_ROTATE_180);
end;

procedure TfmMain.nnRotate270Click(Sender: TObject);
var
  BlkObj: Integer;
begin
  nnRotate270.Checked := not nnRotate270.Checked;
  NSOCR.Img_GetBlock(ImgObj, pmBlock.Tag, BlkObj);
  if not nnRotate270.Checked then
    NSOCR.Blk_Rotation(BlkObj, BLK_ROTATE_NONE)
  else
    NSOCR.Blk_Rotation(BlkObj, BLK_ROTATE_90);
end;

procedure TfmMain.nnTypeBarCodeClick(Sender: TObject);
var
  BlkObj: Integer;
begin
  NSOCR.Img_GetBlock(ImgObj, pmBlock.Tag, BlkObj);
  NSOCR.Blk_SetType(BlkObj, BT_BARCODE);
  NSOCR.Cfg_SetOption(CfgObj, BT_BARCODE, 'BarCode/SearchMode', '3'); //deep scanning for manual barcode zone
  ShowImage;
end;

procedure TfmMain.nnTypeTableClick(Sender: TObject);
var
  BlkObj: Integer;
begin
  NSOCR.Img_GetBlock(ImgObj, pmBlock.Tag, BlkObj);
  NSOCR.Blk_SetType(BlkObj, BT_TABLE);
  ShowImage;
end;

procedure TfmMain.nnTypeClearClick(Sender: TObject);
var
  BlkObj: Integer;
begin
  NSOCR.Img_GetBlock(ImgObj, pmBlock.Tag, BlkObj);
  NSOCR.Blk_SetType(BlkObj, BT_CLEAR);
  ShowImage;
end;

procedure TfmMain.nnTypeICRDigitClick(Sender: TObject);
var
  BlkObj: Integer;
begin
  NSOCR.Img_GetBlock(ImgObj, pmBlock.Tag, BlkObj);
  NSOCR.Blk_SetType(BlkObj, BT_ICRDIGIT);
  ShowImage;
end;

procedure TfmMain.nnTypeMRZClick(Sender: TObject);
var
  BlkObj: Integer;
begin
  NSOCR.Img_GetBlock(ImgObj, pmBlock.Tag, BlkObj);
  NSOCR.Blk_SetType(BlkObj, BT_MRZ);
  ShowImage;
end;

procedure TfmMain.nnTypeOCRTextClick(Sender: TObject);
var
  BlkObj: Integer;
begin
  NSOCR.Img_GetBlock(ImgObj, pmBlock.Tag, BlkObj);
  NSOCR.Blk_SetType(BlkObj, BT_OCRTEXT);
  ShowImage;
end;

procedure TfmMain.nnTypeOCRDigitClick(Sender: TObject);
var
  BlkObj: Integer;
begin
  NSOCR.Img_GetBlock(ImgObj, pmBlock.Tag, BlkObj);
  NSOCR.Blk_SetType(BlkObj, BT_OCRDIGIT);
  ShowImage;
end;

procedure TfmMain.nnTypePictureClick(Sender: TObject);
var
  BlkObj: Integer;
begin
  NSOCR.Img_GetBlock(ImgObj, pmBlock.Tag, BlkObj);
  NSOCR.Blk_SetType(BlkObj, BT_PICTURE);
  ShowImage;
end;

procedure TfmMain.nnTypeZoningClick(Sender: TObject);
var
  BlkObj: Integer;
begin
  NSOCR.Img_GetBlock(ImgObj, pmBlock.Tag, BlkObj);
  NSOCR.Blk_SetType(BlkObj, BT_ZONING);
  ShowImage;
end;

procedure TfmMain.LoadDefaultImage;
var
  fn: string;
begin
  cbScale.ItemIndex := 0;

  fn := extractfilepath(application.ExeName) + '..\SampleImage.bmp';

  if not fileexists(fn) then
    exit;
  opFile.FileName := fn;
  bkOpenClick(nil);

  ShowImage;
  bkOCR.SetFocus;
  bkOCR.Font.Style := [fsBold];
end;

procedure TfmMain.bkLanguagesClick(Sender: TObject);
begin
  fmLanguages.ShowModal;
end;

procedure TfmMain.bkLoadBlocksClick(Sender: TObject);
var
  res: Integer;
begin
  if not opBlocks.Execute then
    exit;

  NSOCR.Img_DeleteAllBlocks(ImgObj); // note: Img_LoadBlocks does not remove existing blocks, so remove them here
  bkSave.Enabled := false;

  res := NSOCR.Img_LoadBlocks(ImgObj, opBlocks.FileName);
  // res := NSOCR.Img_LoadBlocks(ImgObj, ''); //load from inner memory buffer

  if res > ERROR_FIRST then
  begin
    ShowError('Img_LoadBlocks', res);
    exit;
  end;
  ShowImage;
end;

procedure TfmMain.bkSaveBlocksClick(Sender: TObject);
var
  res: Integer;
begin
  if not svBlocks.Execute then
    exit;

  res := NSOCR.Img_SaveBlocks(ImgObj, svBlocks.FileName);
  // res := NSOCR.Img_SaveBlocks(ImgObj, ''); //save to inner memory buffer

  if res > ERROR_FIRST then
    ShowError('Img_SaveBlocks', res);
end;

procedure TfmMain.bkClearBlocksClick(Sender: TObject);
begin
  NSOCR.Img_DeleteAllBlocks(ImgObj);
  bkSave.Enabled := false;
  ShowImage;
end;

procedure TfmMain.bkDetectBlocksClick(Sender: TObject);
begin
  NSOCR.Img_DeleteAllBlocks(ImgObj);
  bkSave.Enabled := false;
  NSOCR.Img_OCR(ImgObj, OCRSTEP_ZONING, OCRSTEP_ZONING, 0);
  ShowImage;
end;

procedure TfmMain.bkScanClick(Sender: TObject);
begin
  fmScan.ShowModal;
end;

function GetDocName(s: string): string;
begin
  s := extractfilename(s);
  result := ChangeFileExt(s, '.pdf'); // default extension
end;

procedure TfmMain.bkSaveClick(Sender: TObject);
var
  format, flags: Integer;
  res: Integer;

//  buf:pointer;
//  fs:tfilestream;
//  sz:integer;
begin
  SvrObj := 0; // mark variable, it has no object

  svFile.FileName := GetDocName(opFile.FileName);
  svFile.FilterIndex := 1;
  if not svFile.Execute then
    exit;

  format := SVR_FORMAT_PDF + (svFile.FilterIndex - 1);

  // image over text option for PDF
  if (format = SVR_FORMAT_PDF) or (format = SVR_FORMAT_PDFA) then
    if MessageDlg('Place page image over recognized text?', mtConfirmation,
      [mbYes, mbNo], 0) = mrYes then
      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'Saver/PDF/ImageLayer', '1')
    else
      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'Saver/PDF/ImageLayer', '0');

//////////////////you can set DPI for images
{
  res := NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'Saver/PDF/ImageDPI', '100');
  if res > ERROR_FIRST then
  begin
    ShowError('Cfg_SetOption Saver/PDF/ImageDPI', res);
    NSOCR.Svr_Destroy(SvrObj);
    exit;
  end;
{}
/////////////////

  res := NSOCR.Svr_Create(CfgObj, format, SvrObj);
  if res > ERROR_FIRST then
  begin
    ShowError('Svr_Create', res);
    exit;
  end;

  if cbExactCopy.Checked then
    flags := FMT_EXACTCOPY
  else
    flags := FMT_EDITCOPY;

  NSOCR.Svr_NewDocument(SvrObj);

  if Sender = nil then
    exit; // caller is ProcessEntireDocument

  res := NSOCR.Svr_AddPage(SvrObj, ImgObj, flags);
  // res := NsOCR.Svr_AddPage(SvrObj, ImgObj, flags);

  if res > ERROR_FIRST then
  begin
    ShowError('Svr_AddPage', res);
    NSOCR.Svr_Destroy(SvrObj);
    exit;
  end;

  if (format = SVR_FORMAT_PDF) or (format = SVR_FORMAT_PDFA) or (format = SVR_FORMAT_XML) then //demonstrate how to write PDF info
    NSOCR.Svr_SetDocumentInfo(SvrObj, INFO_PDF_TITLE, 'Sample Title');

  res := NSOCR.Svr_SaveToFile(SvrObj, svFile.FileName);

////or save to memory
{
  sz := NSOCR.Svr_SaveToMemory(SvrObj, nil, 0); //get size
  getmem(buf, sz);
  NSOCR.Svr_SaveToMemory(SvrObj, buf, sz); //save
  //do something with "buf"
  fs:=tfilestream.Create('c:\a.pdf', fmCreate or fmOpenWrite);
  fs.WriteBuffer(buf^, sz);
  fs.Free;
  freemem(buf);
{}
////

  NSOCR.Svr_Destroy(SvrObj);

  if res > ERROR_FIRST then
  begin
    ShowError('Svr_SaveToFile', res);
    exit;
  end;

  // open the file
  ShellExecute(Handle, 'open', @svFile.FileName[1], '', nil, SW_SHOWNORMAL);
end;

procedure TfmMain.ProcessEntireDocument;
var
  res, OcrCnt, flags: Integer;
  InSameThread: boolean;
  //txt:widestring;

//  buf:pointer;
//  fs:tfilestream;
//  sz:integer;
begin
  bkSaveClick(nil);
  if SvrObj = 0 then
    exit; // saving cancelled

  // recognize up to 4 images at once.
  // Note for large images ERROR_NOMEMORY can be raised
  // OcrCnt := 4;

  // Use number of logical CPU cores on the system for the best performance
  OcrCnt := 0;

  InSameThread := false; // perform OCR in non-blocking mode
  // InSameThread := true; //uncomment to perform OCR from this thread (GUI will be freezed)

  if cbExactCopy.Checked then
    flags := FMT_EXACTCOPY
  else
    flags := FMT_EDITCOPY;
  flags := flags shl 8; // we need to shift FMT_XXXXX flags for this function

  // process all pages of input image and add results to SAVER object
  // this function will create (and then release) additional temporary OCR objects if OcrCnt > 1
  if InSameThread then
  begin
    res := NSOCR.Ocr_ProcessPages(ImgObj, SvrObj, 0, -1, OcrCnt,
      OCRFLAG_NONE or flags);
  end
  else
  begin
    // do it in non-blocking mode and then wait for result
    res := NSOCR.Ocr_ProcessPages(ImgObj, SvrObj, 0, -1, OcrCnt,
      OCRFLAG_THREAD or flags);
    if res > ERROR_FIRST then
    begin
      ShowError('Ocr_ProcessPages(1)', res);
      exit;
    end;
    fmWait.mode := 1;
    fmWait.ShowModal;
    res := fmWait.res;
  end;

  // restore option
  NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'Main/NumKernels', '0');

  if res > ERROR_FIRST then
  begin
    if res = ERROR_OPERATIONCANCELLED then
      ShowMessage('Operation was cancelled.')
    else
      ShowError('Ocr_ProcessPages', res);
    NSOCR.Svr_Destroy(SvrObj);
    exit;
  end;

  // get output document text string
  //NSOCR.Svr_GetText(SvrObj, -1, txt); //-1 - all pages
  //mmText.Text := txt;

  // save output document
  res := NSOCR.Svr_SaveToFile(SvrObj, svFile.FileName);


{  //or save to memory
  sz := NSOCR.Svr_SaveToMemory(SvrObj, nil, 0); //get size
  getmem(buf, sz);
  NSOCR.Svr_SaveToMemory(SvrObj, buf, sz); //save
  //do something with "buf"
  fs:=tfilestream.Create('c:\a.txt', fmCreate or fmOpenWrite);
  fs.WriteBuffer(buf^, sz);
  fs.Free;
  freemem(buf);
{}

  NSOCR.Svr_Destroy(SvrObj);

  if res > ERROR_FIRST then
  begin
    ShowError('Svr_SaveToFile', res);
    exit;
  end;

  // open the file
  ShellExecute(Handle, 'open', @svFile.FileName[1], '', nil, SW_SHOWNORMAL);
end;

end.


