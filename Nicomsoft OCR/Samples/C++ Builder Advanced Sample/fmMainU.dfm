object fmMain: TfmMain
  Left = 0
  Top = 0
  Caption = 'Nicomsoft OCR C++ Builder Advanced Sample'
  ClientHeight = 584
  ClientWidth = 1081
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  Position = poScreenCenter
  Scaled = False
  OnClose = FormClose
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object Splitter1: TSplitter
    Left = 559
    Top = 75
    Height = 490
    Align = alRight
    ExplicitLeft = 515
    ExplicitTop = 11
    ExplicitHeight = 573
  end
  object pnTop: TPanel
    Left = 0
    Top = 0
    Width = 1081
    Height = 75
    Align = alTop
    TabOrder = 0
    object lbWait: TLabel
      Left = 259
      Top = 49
      Width = 66
      Height = 13
      Caption = 'Please wait...'
      Visible = False
    end
    object Label1: TLabel
      Left = 338
      Top = 17
      Width = 24
      Height = 13
      Caption = 'Page'
    end
    object lbPages: TLabel
      Left = 399
      Top = 17
      Width = 19
      Height = 13
      Caption = 'of 1'
    end
    object Label2: TLabel
      Left = 8
      Top = 50
      Width = 29
      Height = 13
      Caption = 'Scale:'
    end
    object bkOpen: TButton
      Left = 88
      Top = 9
      Width = 75
      Height = 25
      Caption = 'Open File'
      TabOrder = 1
      OnClick = bkOpenClick
    end
    object bkOCR: TButton
      Left = 169
      Top = 9
      Width = 75
      Height = 25
      Caption = 'Recognize'
      TabOrder = 2
      OnClick = bkOCRClick
    end
    object edPage: TEdit
      Left = 368
      Top = 11
      Width = 25
      Height = 21
      MaxLength = 3
      TabOrder = 3
      Text = '1'
    end
    object bkSetPage: TButton
      Left = 435
      Top = 11
      Width = 52
      Height = 21
      Caption = 'Set'
      TabOrder = 4
      OnClick = bkSetPageClick
    end
    object cbScale: TComboBox
      Left = 43
      Top = 46
      Width = 111
      Height = 21
      Style = csDropDownList
      ItemIndex = 0
      TabOrder = 5
      Text = 'Auto'
      OnClick = cbScaleClick
      Items.Strings = (
        'Auto'
        '0.25'
        '0.5'
        '1.0'
        '1.5'
        '2.0'
        '2.5'
        '4.0')
    end
    object gbLanguages: TGroupBox
      Left = 506
      Top = 3
      Width = 247
      Height = 67
      Caption = 'Languages '
      TabOrder = 6
      object bkLanguages: TButton
        Left = 56
        Top = 25
        Width = 141
        Height = 25
        Caption = 'Select Languages'
        TabOrder = 0
        OnClick = bkLanguagesClick
      end
    end
    object cbExactCopy: TCheckBox
      Left = 759
      Top = 53
      Width = 177
      Height = 17
      Caption = 'Exact copy (do not format text)'
      TabOrder = 7
      OnClick = cbExactCopyClick
    end
    object bkSave: TButton
      Left = 250
      Top = 9
      Width = 75
      Height = 25
      Caption = 'Save'
      TabOrder = 8
      OnClick = bkSaveClick
    end
    object bkScan: TButton
      Left = 7
      Top = 9
      Width = 75
      Height = 25
      Caption = 'Scan'
      TabOrder = 0
      OnClick = bkScanClick
    end
  end
  object pnText: TPanel
    Left = 562
    Top = 75
    Width = 519
    Height = 490
    Align = alRight
    TabOrder = 1
    object mmText: TMemo
      Left = 1
      Top = 1
      Width = 517
      Height = 488
      Align = alClient
      ReadOnly = True
      ScrollBars = ssBoth
      TabOrder = 0
    end
  end
  object pnImg: TPanel
    Left = 0
    Top = 75
    Width = 559
    Height = 490
    Align = alClient
    TabOrder = 2
    OnResize = pnImgResize
    object pbImg: TPaintBox
      Left = 1
      Top = 1
      Width = 557
      Height = 459
      Align = alClient
      OnMouseDown = pbImgMouseDown
      OnMouseMove = pbImgMouseMove
      OnMouseUp = pbImgMouseUp
      OnPaint = pbImgPaint
      ExplicitLeft = 0
      ExplicitTop = -2
      ExplicitHeight = 467
    end
    object Panel1: TPanel
      Left = 1
      Top = 460
      Width = 557
      Height = 29
      Align = alBottom
      TabOrder = 0
      object cbBin: TCheckBox
        Left = 15
        Top = 0
        Width = 146
        Height = 17
        Caption = 'Display binarized image'
        TabOrder = 0
        OnClick = cbBinClick
      end
      object bkLoadBlocks: TButton
        Left = 168
        Top = 1
        Width = 75
        Height = 25
        Caption = 'Load Zones'
        Enabled = False
        TabOrder = 1
        OnClick = bkLoadBlocksClick
      end
      object bkSaveBlocks: TButton
        Left = 249
        Top = 1
        Width = 75
        Height = 25
        Caption = 'Save Zones'
        Enabled = False
        TabOrder = 2
        OnClick = bkSaveBlocksClick
      end
      object bkClearBlocks: TButton
        Left = 330
        Top = 1
        Width = 75
        Height = 25
        Caption = 'Clear Zones'
        Enabled = False
        TabOrder = 3
        OnClick = bkClearBlocksClick
      end
      object bkDetectBlocks: TButton
        Left = 411
        Top = 1
        Width = 75
        Height = 25
        Caption = 'Detect Zones'
        Enabled = False
        TabOrder = 4
        OnClick = bkDetectBlocksClick
      end
    end
  end
  object stBar: TStatusBar
    Left = 0
    Top = 565
    Width = 1081
    Height = 19
    Panels = <
      item
        Width = 250
      end
      item
        Width = 250
      end
      item
        Width = 250
      end>
  end
  object opFile: TOpenDialog
    Filter = 
      'Image File (JPG,BMP,PNG,TIF,GIF,PDF)|*.JPG;*.BMP;*.PNG;*.TIF;*.T' +
      'IFF;*.GIF;*.PDF|All Files|*'
    Left = 336
    Top = 88
  end
  object pmBlock: TPopupMenu
    Left = 360
    Top = 176
    object nnTypeOCRText: TMenuItem
      Caption = 'Type: Machine-printed Text'
      OnClick = nnTypeOCRTextClick
    end
    object nnTypeOCRDigit: TMenuItem
      Caption = 'Type: Machine-printed Digits'
      OnClick = nnTypeOCRDigitClick
    end
    object nnTypeICRDigit: TMenuItem
      Caption = 'Type: Handwritten Digits'
      OnClick = nnTypeICRDigitClick
    end
    object nnTypeBarCode: TMenuItem
      Caption = 'Type: Bar Code'
      OnClick = nnTypeBarCodeClick
    end
    object nnTypeTable: TMenuItem
      Caption = 'Type: Table'
      OnClick = nnTypeTableClick
    end
    object nnTypePicture: TMenuItem
      Caption = 'Type: Picture'
      OnClick = nnTypePictureClick
    end
    object nnTypeClear: TMenuItem
      Caption = 'Type: Clear Area'
      OnClick = nnTypeClearClick
    end
    object nnTypeZoning: TMenuItem
      Caption = 'Type: Zoning area'
      OnClick = nnTypeZoningClick
    end
    object nnTypeMRZ: TMenuItem
      Caption = 'Type: MRZ'
      OnClick = nnTypeMRZClick
    end
    object N1: TMenuItem
      Caption = '-'
    end
    object nnInvert: TMenuItem
      Caption = 'Image Inverted'
      OnClick = nnInvertClick
    end
    object nnRotate90: TMenuItem
      Caption = 'Image Rotated 90'#176' clockwise'
      OnClick = nnRotate90Click
    end
    object nnRotate270: TMenuItem
      Caption = 'Image Rotated 90'#176' counterclockwise'
      OnClick = nnRotate270Click
    end
    object nnRotate180: TMenuItem
      Caption = 'Image Rotated 180'#176
      OnClick = nnRotate180Click
    end
    object nnMirrorH: TMenuItem
      Caption = 'Image Mirrored Horizontally'
      OnClick = nnMirrorHClick
    end
    object nnMirrorV: TMenuItem
      Caption = 'Image Mirrored Vertically'
      OnClick = nnMirrorVClick
    end
    object N2: TMenuItem
      Caption = '-'
    end
    object nnDeleteBlock: TMenuItem
      Caption = 'Delete Block(Zone)'
      OnClick = nnDeleteBlockClick
    end
  end
  object opBlocks: TOpenDialog
    DefaultExt = '.blk'
    Filter = '.blk files|*.blk'
    Options = [ofPathMustExist, ofFileMustExist, ofEnableSizing]
    Left = 352
    Top = 288
  end
  object svBlocks: TSaveDialog
    DefaultExt = '.blk'
    Filter = '.blk files|*.blk'
    Options = [ofOverwritePrompt, ofPathMustExist, ofEnableSizing]
    Left = 408
    Top = 288
  end
  object svFile: TSaveDialog
    DefaultExt = '*.pdf'
    Filter = 
      'PDF document(*.pdf)|*.pdf|RTF document (*.rtf)|*.rtf|ASCII Text ' +
      'document (*.txt)|*.txt|Unicode Text document (*.txt)|*.txt|XML d' +
      'ocument (*.xml)|*.xml|PDF/A document(*.pdf)|*.pdf'
    Options = [ofOverwritePrompt, ofHideReadOnly, ofEnableSizing]
    Left = 416
    Top = 88
  end
end
