unit fmOptionsU;

                              interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics,
  Controls, Forms, Dialogs, StdCtrls, NSOCR;

type
  TfmOptions = class(TForm)
    Label1: TLabel;
    bkOK: TButton;
    bkCancel: TButton;
    cbFindBarcodes: TCheckBox;
    cbImgInversion: TCheckBox;
    cbDeskew: TCheckBox;
    cbRotation: TCheckBox;
    Label2: TLabel;
    Label3: TLabel;
    cbImgNoiseFilter: TCheckBox;
    cbGrayMode: TCheckBox;
    edEnabledChars: TEdit;
    Label4: TLabel;
    Label5: TLabel;
    edDisabledChars: TEdit;
    Label6: TLabel;
    edBinThreshold: TEdit;
    cbZonesInversion: TCheckBox;
    Label7: TLabel;
    edTextQual: TEdit;
    Label8: TLabel;
    edPDFDPI: TEdit;
    cbFastMode: TCheckBox;
    cbRemoveLines: TCheckBox;
    bkHelp: TButton;
    cbBinTwice: TCheckBox;
    cbCorrectMixed: TCheckBox;
    cbDictionary: TCheckBox;
    cbOneColumn: TCheckBox;
    procedure FormKeyPress(Sender: TObject; var Key: Char);
    procedure bkCancelClick(Sender: TObject);
    procedure bkOKClick(Sender: TObject);
    procedure FormShow(Sender: TObject);
    procedure bkHelpClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  fmOptions: TfmOptions;

                             implementation

{$R *.dfm}

uses fmMainU;

procedure TfmOptions.bkCancelClick(Sender: TObject);
begin
  close;
end;

procedure TfmOptions.FormKeyPress(Sender: TObject; var Key: Char);
begin
  if key = #27 then close;
end;

procedure TfmOptions.bkHelpClick(Sender: TObject);
begin
  fmmain.ShowHelp('config.htm');
end;

procedure TfmOptions.FormShow(Sender: TObject);
var val:widestring;
begin
  with fmmain do
    begin
      NSOCR.Cfg_GetOption(CfgObj, BT_DEFAULT, 'Zoning/FindBarcodes', val);
      cbFindBarcodes.Checked := val = '1';

      NSOCR.Cfg_GetOption(CfgObj, BT_DEFAULT, 'ImgAlizer/Inversion', val);
      cbImgInversion.Checked := val = '2';

      NSOCR.Cfg_GetOption(CfgObj, BT_DEFAULT, 'Zoning/DetectInversion', val);
      cbZonesInversion.Checked := val = '1';

      NSOCR.Cfg_GetOption(CfgObj, BT_DEFAULT, 'ImgAlizer/SkewAngle', val);
      cbDeskew.Checked := val = '360';

      NSOCR.Cfg_GetOption(CfgObj, BT_DEFAULT, 'ImgAlizer/AutoRotate', val);
      cbRotation.Checked := val = '1';

      NSOCR.Cfg_GetOption(CfgObj, BT_DEFAULT, 'ImgAlizer/NoiseFilter', val);
      cbImgNoiseFilter.Checked := val = '1';

      NSOCR.Cfg_GetOption(CfgObj, BT_DEFAULT, 'PixLines/RemoveLines', val);
      cbRemoveLines.Checked := val = '1';

      NSOCR.Cfg_GetOption(CfgObj, BT_DEFAULT, 'Main/GrayMode', val);
      cbGrayMode.Checked := val = '1';

      NSOCR.Cfg_GetOption(CfgObj, BT_DEFAULT, 'Main/FastMode', val);
      cbFastMode.Checked := val = '1';

      NSOCR.Cfg_GetOption(CfgObj, BT_DEFAULT, 'Binarizer/BinTwice', val);
      cbBinTwice.Checked := val = '1';

      NSOCR.Cfg_GetOption(CfgObj, BT_DEFAULT, 'WordAlizer/CorrectMixed', val);
      cbCorrectMixed.Checked := val = '1';

      NSOCR.Cfg_GetOption(CfgObj, BT_DEFAULT, 'Dictionaries/UseDictionary', val);
      cbDictionary.Checked := val = '1';

      NSOCR.Cfg_GetOption(CfgObj, BT_DEFAULT, 'Zoning/OneColumn', val);
      cbOneColumn.Checked := val = '1';


      NSOCR.Cfg_GetOption(CfgObj, BT_DEFAULT, 'Main/EnabledChars', val);
      edEnabledChars.Text := val;

      NSOCR.Cfg_GetOption(CfgObj, BT_DEFAULT, 'Main/DisabledChars', val);
      edDisabledChars.Text := val;

      NSOCR.Cfg_GetOption(CfgObj, BT_DEFAULT, 'Binarizer/SimpleThr', val);
      edBinThreshold.Text := val;

      NSOCR.Cfg_GetOption(CfgObj, BT_DEFAULT, 'WordAlizer/TextQual', val);
      edTextQual.Text := val;

      NSOCR.Cfg_GetOption(CfgObj, BT_DEFAULT, 'Main/PdfDPI', val);
      edPDFDPI.Text := val;
    end;
end;

procedure TfmOptions.bkOKClick(Sender: TObject);
var val:widestring;
begin
  with fmmain do
    begin
      if cbFindBarcodes.Checked then val := '1'
       else val := '0';
      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'Zoning/FindBarcodes', val);

      if cbImgInversion.Checked then val := '2'
       else
        val := '0';
      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'ImgAlizer/Inversion', val);

      if cbZonesInversion.Checked then val := '1'
       else val := '0';
      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'Zoning/DetectInversion', val);

      if cbDeskew.Checked then val := '360'
       else val := '0';
      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'ImgAlizer/SkewAngle', val);

      if cbRotation.Checked then val := '1'
       else val := '0';
      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'ImgAlizer/AutoRotate', val);

      if cbImgNoiseFilter.Checked then val := '1'
       else val := '0';
      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'ImgAlizer/NoiseFilter', val);

      if cbRemoveLines.Checked then val := '1'
       else val := '0';
      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'PixLines/RemoveLines', val);
      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'PixLines/FindHorLines', val);
      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'PixLines/FindVerLines', val);

      if cbGrayMode.Checked then val := '1'
       else val := '0';
      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'Main/GrayMode', val);

      if cbFastMode.Checked then val := '1'
       else val := '0';
      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'Main/FastMode', val);

      if cbBinTwice.Checked then val := '1'
       else val := '0';
      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'Binarizer/BinTwice', val);

      if cbCorrectMixed.Checked then val := '1'
       else val := '0';
      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'WordAlizer/CorrectMixed', val);

      if cbDictionary.Checked then val := '1'
       else val := '0';
      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'Dictionaries/UseDictionary', val);

      if cbOneColumn.Checked then val := '1'
       else val := '0';
      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'Zoning/OneColumn', val);

      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'Main/EnabledChars', edEnabledChars.Text);

      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'Main/DisabledChars', edDisabledChars.Text);

      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'Binarizer/SimpleThr', edBinThreshold.Text);

      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'WordAlizer/TextQual', edTextQual.Text);

      NSOCR.Cfg_SetOption(CfgObj, BT_DEFAULT, 'Main/PdfDPI', edPDFDPI.Text);
    end;
  close;
end;

end.
