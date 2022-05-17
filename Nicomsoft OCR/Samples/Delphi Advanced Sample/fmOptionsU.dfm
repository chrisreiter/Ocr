object fmOptions: TfmOptions
  Left = 0
  Top = 0
  BorderIcons = [biSystemMenu]
  BorderStyle = bsSingle
  Caption = 'OCR Options'
  ClientHeight = 545
  ClientWidth = 448
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  KeyPreview = True
  OldCreateOrder = False
  Position = poScreenCenter
  Scaled = False
  OnKeyPress = FormKeyPress
  OnShow = FormShow
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 8
    Top = 489
    Width = 418
    Height = 13
    Caption = 
      'NOTE: Some options will not be applied  immediately, reload imag' +
      'e to apply all changes.'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clRed
    Font.Height = -11
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object Label2: TLabel
    Left = 8
    Top = 10
    Width = 406
    Height = 13
    Caption = 
      'You can improve OCR performance and accuracy  by changing defaul' +
      't configuration.'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clRed
    Font.Height = -11
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object Label3: TLabel
    Left = 8
    Top = 29
    Width = 435
    Height = 13
    Caption = 
      'Check "Configuration" and "Performance Tips" sections of documen' +
      'tation for more options.'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clRed
    Font.Height = -11
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object Label4: TLabel
    Left = 8
    Top = 338
    Width = 71
    Height = 13
    Caption = 'Enabled chars:'
  end
  object Label5: TLabel
    Left = 8
    Top = 363
    Width = 73
    Height = 13
    Caption = 'Disabled chars:'
  end
  object Label6: TLabel
    Left = 8
    Top = 395
    Width = 207
    Height = 13
    Caption = 'Binarization threshold (0..254; 255 - auto):'
  end
  object Label7: TLabel
    Left = 8
    Top = 426
    Width = 153
    Height = 13
    Caption = 'Text quality (0..100; -1 - auto):'
  end
  object Label8: TLabel
    Left = 8
    Top = 458
    Width = 92
    Height = 13
    Caption = 'PDF rendering DPI:'
  end
  object bkOK: TButton
    Left = 146
    Top = 512
    Width = 75
    Height = 25
    Caption = 'OK'
    TabOrder = 18
    OnClick = bkOKClick
  end
  object bkCancel: TButton
    Left = 227
    Top = 512
    Width = 75
    Height = 25
    Caption = 'Cancel'
    TabOrder = 19
    OnClick = bkCancelClick
  end
  object cbFindBarcodes: TCheckBox
    Left = 8
    Top = 64
    Width = 97
    Height = 17
    Caption = 'Find barcodes'
    TabOrder = 0
  end
  object cbImgInversion: TCheckBox
    Left = 8
    Top = 84
    Width = 145
    Height = 17
    Caption = 'Detect image inversion'
    TabOrder = 1
  end
  object cbDeskew: TCheckBox
    Left = 8
    Top = 124
    Width = 145
    Height = 17
    Caption = 'Detect and fix image skew'
    TabOrder = 3
  end
  object cbRotation: TCheckBox
    Left = 8
    Top = 144
    Width = 273
    Height = 17
    Caption = 'Detect and fix image rotation 90/180/270 degrees'
    TabOrder = 4
  end
  object cbImgNoiseFilter: TCheckBox
    Left = 8
    Top = 164
    Width = 161
    Height = 17
    Caption = 'Apply noise filter for image'
    TabOrder = 5
  end
  object cbGrayMode: TCheckBox
    Left = 8
    Top = 204
    Width = 97
    Height = 17
    Caption = 'Gray mode'
    TabOrder = 7
  end
  object edEnabledChars: TEdit
    Left = 95
    Top = 333
    Width = 338
    Height = 21
    TabOrder = 13
  end
  object edDisabledChars: TEdit
    Left = 95
    Top = 360
    Width = 338
    Height = 21
    TabOrder = 14
  end
  object edBinThreshold: TEdit
    Left = 230
    Top = 392
    Width = 41
    Height = 21
    TabOrder = 15
  end
  object cbZonesInversion: TCheckBox
    Left = 8
    Top = 104
    Width = 145
    Height = 17
    Caption = 'Detect zones inversion'
    TabOrder = 2
  end
  object edTextQual: TEdit
    Left = 230
    Top = 423
    Width = 41
    Height = 21
    TabOrder = 16
  end
  object edPDFDPI: TEdit
    Left = 230
    Top = 455
    Width = 41
    Height = 21
    TabOrder = 17
  end
  object cbFastMode: TCheckBox
    Left = 8
    Top = 224
    Width = 153
    Height = 17
    Caption = 'Fast mode (less accurate)'
    TabOrder = 8
  end
  object cbRemoveLines: TCheckBox
    Left = 8
    Top = 184
    Width = 145
    Height = 17
    Caption = 'Detect and remove lines'
    TabOrder = 6
  end
  object bkHelp: TButton
    Left = 365
    Top = 512
    Width = 75
    Height = 25
    Caption = 'Help'
    TabOrder = 20
    OnClick = bkHelpClick
  end
  object cbBinTwice: TCheckBox
    Left = 8
    Top = 244
    Width = 97
    Height = 17
    Caption = 'Binarize twice'
    TabOrder = 9
  end
  object cbCorrectMixed: TCheckBox
    Left = 8
    Top = 264
    Width = 329
    Height = 17
    Caption = 'Correct mixed chars (letters and digits in same word)'
    TabOrder = 10
  end
  object cbDictionary: TCheckBox
    Left = 8
    Top = 284
    Width = 97
    Height = 17
    Caption = 'Use dictionary'
    TabOrder = 11
  end
  object cbOneColumn: TCheckBox
    Left = 8
    Top = 304
    Width = 185
    Height = 17
    Caption = 'Combine zones horizontally'
    TabOrder = 12
  end
end
