program Sample;

uses
  Forms,
  SysUtils,
  fmMainU in 'fmMainU.pas' {fmMain},
  NSOCR in '..\NSOCR.pas',
  fmScanU in 'fmScanU.pas' {fmScan},
  fmWaitU in 'fmWaitU.pas' {fmWait},
  fmLanguagesU in 'fmLanguagesU.pas' {fmLanguages},
  fmOptionsU in 'fmOptionsU.pas' {fmOptions};

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfmMain, fmMain);
  Application.CreateForm(TfmScan, fmScan);
  Application.CreateForm(TfmWait, fmWait);
  Application.CreateForm(TfmLanguages, fmLanguages);
  Application.CreateForm(TfmOptions, fmOptions);

  //DecimalSeparator := '.'; //for old Delphi versions
  FormatSettings.DecimalSeparator := '.'; //for newest Delphi versions

  Application.Run;
end.
