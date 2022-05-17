object fmScan: TfmScan
  Left = 0
  Top = 0
  BorderIcons = [biSystemMenu]
  BorderStyle = bsSingle
  Caption = 'Scanning'
  ClientHeight = 206
  ClientWidth = 519
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  Position = poScreenCenter
  OnShow = FormShow
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 8
    Top = 32
    Width = 67
    Height = 13
    Caption = 'Select device:'
  end
  object Label2: TLabel
    Left = 8
    Top = 81
    Width = 88
    Height = 13
    Caption = 'Document Source:'
  end
  object cbScanners: TComboBox
    Left = 104
    Top = 29
    Width = 402
    Height = 21
    Style = csDropDownList
    TabOrder = 0
  end
  object bkScan: TButton
    Left = 183
    Top = 173
    Width = 75
    Height = 25
    Caption = 'Scan'
    TabOrder = 1
    OnClick = bkScanClick
  end
  object bkCancel: TButton
    Left = 264
    Top = 173
    Width = 75
    Height = 25
    Caption = 'Cancel'
    TabOrder = 2
  end
  object cbNoUI: TCheckBox
    Left = 8
    Top = 127
    Width = 97
    Height = 17
    Caption = 'No Scanner UI'
    TabOrder = 3
  end
  object bkDefault: TButton
    Left = 408
    Top = 56
    Width = 98
    Height = 25
    Caption = 'Set as Default'
    TabOrder = 4
    OnClick = bkDefaultClick
  end
  object cbSrc: TComboBox
    Left = 104
    Top = 78
    Width = 145
    Height = 21
    Style = csDropDownList
    ItemIndex = 0
    TabOrder = 5
    Text = 'Flatbed'
    Items.Strings = (
      'Flatbed'
      'ADF(Automatic Document Feeder)'
      'Auto')
  end
end
