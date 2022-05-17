unit fmLanguagesU;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics,
  Controls, Forms, Dialogs, StdCtrls, CheckLst;

type
  TfmLanguages = class(TForm)
    bkOK: TButton;
    bkCancel: TButton;
    cbLanguages: TCheckListBox;
    Label1: TLabel;
    cbLanguagesAsian: TCheckListBox;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    procedure FormShow(Sender: TObject);
    procedure bkOKClick(Sender: TObject);
    procedure bkCancelClick(Sender: TObject);
    procedure FormKeyPress(Sender: TObject; var Key: Char);
  private
    { Private declarations }
    asian:boolean;

    procedure LoadLanguages;
    function ApplyLanguages: boolean;
  public
    { Public declarations }

  end;

var
  fmLanguages: TfmLanguages;

implementation

{$R *.dfm}

uses NSOCR, fmMainU;

procedure TfmLanguages.LoadLanguages;
var val: widestring;
    i:integer;
begin
  asian := false;
  with fmmain do
    for i := 0 to cbLanguages.Items.Count - 1 do
      begin
        if NSOCR.Cfg_GetOption(CfgObj, BT_DEFAULT, 'Languages/' + cbLanguages.Items[i], val) < ERROR_FIRST then
          cbLanguages.Checked[i] := strtointdef(val, 1) <> 0
         else
          cbLanguages.Checked[i] := false;
    end;
  with fmmain do
    for i := 0 to cbLanguagesAsian.Items.Count - 1 do
      begin
        if NSOCR.Cfg_GetOption(CfgObj, BT_DEFAULT, 'Languages/' + cbLanguagesAsian.Items[i], val) < ERROR_FIRST then
          begin
            cbLanguagesAsian.Checked[i] := strtointdef(val, 1) <> 0;
            if cbLanguagesAsian.Checked[i] then
              asian := true;
          end
         else
          cbLanguagesAsian.Checked[i] := false;
    end;
end;

function TfmLanguages.ApplyLanguages: boolean;
var i:integer;
    ch1:boolean;
    ch2:integer;
begin
  result := false;
  ch1 := false;
  for i := 0 to cbLanguages.Items.Count - 1 do
    if cbLanguages.Checked[i] then
      begin
        ch1 := true;
        break;
      end;
  ch2 := 0;
  for i := 0 to cbLanguagesAsian.Items.Count - 1 do
    if cbLanguagesAsian.Checked[i] then
      ch2 := ch2 + 1;

  if not ch1 and (ch2 = 0) then
    begin
      ShowMessage('Please select at least one language for recognition.');
      exit;
    end;
  if ch1 and (ch2 > 0) then
    begin
      ShowMessage('Using both main and asian languages in same zone is not supported.');
      exit;
    end;
  if ch2 > 1 then
    begin
      ShowMessage('Using two or more asian languages in same zone is not supported currently.');
      exit;
    end;

  with fmmain do
    for i := 0 to cbLanguages.Items.Count - 1 do
      begin
        if cbLanguages.Checked[i] then
          NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'Languages/' + cbLanguages.Items[i], '1')
         else
          NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'Languages/' + cbLanguages.Items[i], '0');
    end;
  with fmmain do
    for i := 0 to cbLanguagesAsian.Items.Count - 1 do
      begin
        if cbLanguagesAsian.Checked[i] then
          NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'Languages/' + cbLanguagesAsian.Items[i], '1')
         else
          NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'Languages/' + cbLanguagesAsian.Items[i], '0');
    end;

    if asian <> (ch2 <> 0) then
      ShowMessage('Asian languages require different image rescaling, please re-open current image for best results.');

  result := true;
end;

procedure TfmLanguages.bkCancelClick(Sender: TObject);
begin
  close;
end;

procedure TfmLanguages.bkOKClick(Sender: TObject);
begin
  if ApplyLanguages then
    close;
end;

procedure TfmLanguages.FormKeyPress(Sender: TObject; var Key: Char);
begin
  if key = #27 then close;
end;

procedure TfmLanguages.FormShow(Sender: TObject);
begin
  LoadLanguages;
end;

end.
