unit fmScanU;

                               interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ShellAPI, NSOCR;

type
  TfmScan = class(TForm)
    Label1: TLabel;
    cbScanners: TComboBox;
    bkScan: TButton;
    bkCancel: TButton;
    cbNoUI: TCheckBox;
    bkDefault: TButton;
    Label2: TLabel;
    cbSrc: TComboBox;
    bkScanToFile: TButton;
    sdTiff: TSaveDialog;
    procedure bkCancelClick(Sender: TObject);
    procedure bkScanClick(Sender: TObject);
    procedure FormShow(Sender: TObject);
    procedure FormKeyPress(Sender: TObject; var Key: Char);
    procedure bkDefaultClick(Sender: TObject);
    procedure bkScanToFileClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  fmScan: TfmScan;

                            implementation

uses fmMainU;

{$R *.dfm}

procedure TfmScan.bkCancelClick(Sender: TObject);
begin
  close;
end;

procedure TfmScan.FormKeyPress(Sender: TObject; var Key: Char);
begin
  if key = #27 then close;
end;

procedure TfmScan.FormShow(Sender: TObject);
var res, i:integer;
    names, tmp:widestring;
    r:string;
begin
  cbScanners.Items.Clear;

  res := fmmain.NSOCR.Scan_Enumerate(fmmain.ScanObj, names, 0);
  if res > ERROR_FIRST then
    begin
      fmmain.ShowError('Scan_Enumerate', res);
      exit;
    end;

//names are separated by comma, parse them
  r := '';
  for i := 1 to length(names) do
  if names[i] <> ',' then
    r:=r + names[i]
   else
    begin
      cbScanners.Items.Add(r);
      r:='';
    end;
  if r <> '' then
    cbScanners.Items.Add(r);

//now get default device index
  tmp := '';
  res := fmmain.NSOCR.Scan_Enumerate(fmmain.ScanObj, tmp, SCAN_GETDEFAULTDEVICE);
  if res > ERROR_FIRST then
    begin
      if res <> ERROR_NODEFAULTDEVICE then
        fmmain.ShowError('Scan_Enumerate (1)', res);
      exit;
    end
   else
    begin
      if res < cbScanners.Items.Count then
        cbScanners.ItemIndex := res;
    end;
end;

procedure TfmScan.bkDefaultClick(Sender: TObject);
var res, n:integer;
    tmp:widestring;
begin
  n := cbScanners.ItemIndex;
  if n < 0 then exit;
  tmp := '';
  res := fmmain.NSOCR.Scan_Enumerate(fmmain.ScanObj, tmp, SCAN_SETDEFAULTDEVICE or n);
  if res > ERROR_FIRST then
    begin
      fmmain.ShowError('Scan_Enumerate (2)', res);
      exit;
    end;
end;

procedure TfmScan.bkScanClick(Sender: TObject);
var n, res, flags:integer;
begin
  n := cbScanners.ItemIndex;
  if n < 0 then
    begin
      ShowMessage('No scanner selected!');
      exit;
    end;

  flags := 0;
  case cbSrc.ItemIndex of
    0: ;//flatbed
    1: flags := flags or SCAN_SOURCEADF; //adf
    2: flags := flags or SCAN_SOURCEAUTO; //auto
  end;

  if cbNoUI.Checked then
    flags := flags or SCAN_NOUI;

  res := fmmain.NSOCR.Scan_ScanToImg(fmmain.ScanObj, fmmain.ImgObj, n, flags);

  if res > ERROR_FIRST then
    begin
      fmmain.ShowError('Scan_ScanToImg', res);
      exit;
    end;

  close;
  fmmain.DoImageLoaded;
end;


procedure TfmScan.bkScanToFileClick(Sender: TObject);
var n, res, flags:integer;
begin
  n := cbScanners.ItemIndex;
  if n < 0 then
    begin
      ShowMessage('No scanner selected!');
      exit;
    end;

  if not sdTiff.Execute then exit;

  flags := 0;
  case cbSrc.ItemIndex of
    0: ;//flatbed
    1: flags := flags or SCAN_SOURCEADF; //adf
    2: flags := flags or SCAN_SOURCEAUTO; //auto
  end;

  if cbNoUI.Checked then
    flags := flags or SCAN_NOUI;

  //scan to separate files
  //flags := flags or SCAN_FILE_SEPARATE;

  res := fmmain.NSOCR.Scan_ScanToFile(fmmain.ScanObj, sdTiff.FileName, n, flags);

  if res > ERROR_FIRST then
    begin
      fmmain.ShowError('Scan_ScanToImg', res);
      exit;
    end;

  // open the file
  ShellExecute(Handle, 'open', @sdTiff.FileName[1], '', nil, SW_SHOWNORMAL);
end;


end.
