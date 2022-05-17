object fmLanguages: TfmLanguages
  Left = 0
  Top = 0
  BorderIcons = [biSystemMenu]
  BorderStyle = bsSingle
  Caption = 'fmLanguages'
  ClientHeight = 498
  ClientWidth = 404
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
    Left = 10
    Top = 13
    Width = 115
    Height = 13
    Caption = 'Main module languages:'
  end
  object Label2: TLabel
    Left = 224
    Top = 13
    Width = 119
    Height = 13
    Caption = 'Asian module languages:'
  end
  object Label3: TLabel
    Left = 8
    Top = 413
    Width = 354
    Height = 13
    Caption = 
      'NOTE: Using both main and asian languages in one zone is not sup' +
      'ported.'
  end
  object Label4: TLabel
    Left = 8
    Top = 432
    Width = 393
    Height = 13
    Caption = 
      'But you can use both main and asian languages on same image in d' +
      'ifferent zones.'
  end
  object cbLanguages: TCheckListBox
    Left = 10
    Top = 32
    Width = 169
    Height = 361
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
    TabOrder = 0
  end
  object bkOK: TButton
    Left = 124
    Top = 465
    Width = 75
    Height = 25
    Caption = 'OK'
    TabOrder = 1
    OnClick = bkOKClick
  end
  object bkCancel: TButton
    Left = 205
    Top = 465
    Width = 75
    Height = 25
    Caption = 'Cancel'
    TabOrder = 2
    OnClick = bkCancelClick
  end
  object cbLanguagesAsian: TCheckListBox
    Left = 224
    Top = 32
    Width = 169
    Height = 153
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
