object fmWait: TfmWait
  Left = 0
  Top = 0
  BorderIcons = [biSystemMenu]
  BorderStyle = bsSingle
  Caption = 'Progress'
  ClientHeight = 113
  ClientWidth = 368
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  Position = poScreenCenter
  OnClose = FormClose
  OnShow = FormShow
  PixelsPerInch = 96
  TextHeight = 13
  object fmWait: TLabel
    Left = 150
    Top = 8
    Width = 66
    Height = 13
    Caption = 'Please wait...'
  end
  object ProgressBar1: TProgressBar
    Left = 36
    Top = 40
    Width = 293
    Height = 17
    TabOrder = 0
  end
  object bkCancel: TButton
    Left = 145
    Top = 80
    Width = 75
    Height = 25
    Caption = 'Cancel'
    TabOrder = 1
    OnClick = bkCancelClick
  end
  object Timer1: TTimer
    Enabled = False
    Interval = 100
    OnTimer = Timer1Timer
    Left = 320
    Top = 64
  end
end
