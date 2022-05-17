object fmLanguages: TfmLanguages
  Left = 0
  Top = 0
  BorderIcons = [biSystemMenu]
  BorderStyle = bsSingle
  Caption = 'Languages'
  ClientHeight = 470
  ClientWidth = 409
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  KeyPreview = True
  OldCreateOrder = False
  Position = poScreenCenter
  OnKeyPress = FormKeyPress
  OnShow = FormShow
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 8
    Top = 8
    Width = 115
    Height = 13
    Caption = 'Main module languages:'
  end
  object Label2: TLabel
    Left = 232
    Top = 8
    Width = 119
    Height = 13
    Caption = 'Asian module languages:'
  end
  object Label3: TLabel
    Left = 8
    Top = 395
    Width = 354
    Height = 13
    Caption = 
      'NOTE: Using both main and asian languages in one zone is not sup' +
      'ported.'
  end
  object Label4: TLabel
    Left = 8
    Top = 414
    Width = 393
    Height = 13
    Caption = 
      'But you can use both main and asian languages on same image in d' +
      'ifferent zones.'
  end
  object bkOK: TButton
    Left = 115
    Top = 438
    Width = 75
    Height = 25
    Caption = 'OK'
    TabOrder = 0
    OnClick = bkOKClick
  end
  object bkCancel: TButton
    Left = 196
    Top = 438
    Width = 75
    Height = 25
    Caption = 'Cancel'
    TabOrder = 1
    OnClick = bkCancelClick
  end
  object cbLanguages: TCheckListBox
    Left = 8
    Top = 27
    Width = 169
    Height = 358
    ItemHeight = 13
    Items.Strings = (
      'Bulgarian'
      'Catalan'
      'Croatian'
      'Czech'
      'Danish'
      'Dutch'
      'English'
      'Estonian'
      'Finnish'
      'French'
      'German'
      'Hungarian'
      'Indonesian'
      'Italian'
      'Latvian'
      'Lithuanian'
      'Norwegian'
      'Polish'
      'Portuguese'
      'Romanian'
      'Russian'
      'Slovak'
      'Slovenian'
      'Spanish'
      'Swedish'
      'Turkish')
    Sorted = True
    TabOrder = 2
  end
  object cbLanguagesAsian: TCheckListBox
    Left = 232
    Top = 27
    Width = 169
    Height = 110
    ItemHeight = 13
    Items.Strings = (
      'Arabic'
      'Chinese_Simplified'
      'Chinese_Traditional'
      'Japanese'
      'Korean')
    Sorted = True
    TabOrder = 3
  end
end
