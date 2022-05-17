unit fmWaitU;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls, ComCtrls;

type
  TfmWait = class(TForm)
    fmWait: TLabel;
    Timer1: TTimer;
    ProgressBar1: TProgressBar;
    bkCancel: TButton;
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure Timer1Timer(Sender: TObject);
    procedure FormShow(Sender: TObject);
    procedure bkCancelClick(Sender: TObject);
  private
    { Private declarations }
    function CheckDone:boolean;
  public
    { Public declarations }
    mode:integer; //0- wait for Img_OCR result, 1- wait for Ocr_ProcessPages result
    res:integer;
  end;

var
  fmWait: TfmWait;

                             implementation

uses fmMainU, NSOCR;

{$R *.dfm}

function TfmWait.CheckDone: boolean;
begin
  if mode = 0 then
    res := fmmain.NSOCR.Img_OCR(fmmain.ImgObj, 0, 0, OCRFLAG_GETRESULT)
   else
    res := fmmain.NSOCR.Ocr_ProcessPages(fmmain.ImgObj, 0, 0, 0, 0, OCRFLAG_GETRESULT);

  result := res <> ERROR_PENDING;
end;

procedure TfmWait.FormShow(Sender: TObject);
begin
  Timer1.Enabled := true;
  ProgressBar1.Position := 0;
end;

procedure TfmWait.FormClose(Sender: TObject; var Action: TCloseAction);
begin
  Timer1.Enabled:=false;
  bkCancel.Enabled := false;
  while not CheckDone do //make sure that Img_OCR is done
    begin
      application.ProcessMessages;
      Sleep(10);
    end;
  bkCancel.Enabled := true;
end;

procedure TfmWait.Timer1Timer(Sender: TObject);
var val:integer;
begin
  if CheckDone then
    close;

  if mode = 0 then
    val := fmmain.NSOCR.Img_OCR(fmmain.ImgObj, 0, 0, OCRFLAG_GETPROGRESS)
   else
    val := fmmain.NSOCR.Ocr_ProcessPages(fmmain.ImgObj, 0, 0, 0, 0, OCRFLAG_GETPROGRESS);

  if val < ERROR_FIRST then
    if Progressbar1.Position <> val then
      Progressbar1.Position := val;
end;

procedure TfmWait.bkCancelClick(Sender: TObject);
begin
  //cancel work
  if mode = 0 then
    fmmain.NSOCR.Img_OCR(fmmain.ImgObj, 0, 0, OCRFLAG_CANCEL)
   else
    fmmain.NSOCR.Ocr_ProcessPages(fmmain.ImgObj, 0, 0, 0, 0, OCRFLAG_CANCEL);

  //we must wait for result anyway since OCRFLAG_CANCEL request returns immediately, work is not stopped yet
  self.Close; //this function waits for result
end;

end.
