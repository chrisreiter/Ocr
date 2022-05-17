object fmMain: TfmMain
  Left = 0
  Top = 0
  Caption = 'Nicomsoft OCR Delphi Advanced Sample'
  ClientHeight = 667
  ClientWidth = 1037
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
  OnShow = FormShow
  PixelsPerInch = 96
  TextHeight = 13
  object Splitter1: TSplitter
    Left = 515
    Top = 75
    Height = 573
    Align = alRight
    ExplicitLeft = 56
    ExplicitTop = 368
    ExplicitHeight = 100
  end
  object pnTop: TPanel
    Left = 0
    Top = 0
    Width = 1037
    Height = 75
    Align = alTop
    TabOrder = 0
    object lbWait: TLabel
      Left = 202
      Top = 47
      Width = 66
      Height = 13
      Caption = 'Please wait...'
      Font.Charset = DEFAULT_CHARSET
      Font.Color = clRed
      Font.Height = -11
      Font.Name = 'Tahoma'
      Font.Style = []
      ParentFont = False
      Visible = False
    end
    object Label1: TLabel
      Left = 346
      Top = 17
      Width = 24
      Height = 13
      Caption = 'Page'
    end
    object lbPages: TLabel
      Left = 407
      Top = 17
      Width = 19
      Height = 13
      Caption = 'of 1'
    end
    object Label2: TLabel
      Left = 16
      Top = 54
      Width = 29
      Height = 13
      Caption = 'Scale:'
    end
    object bkOpen: TButton
      Left = 88
      Top = 9
      Width = 75
      Height = 25
      Hint = 'Open Image File for OCR'
      Caption = 'Open File'
      ParentShowHint = False
      ShowHint = True
      TabOrder = 1
      OnClick = bkOpenClick
    end
    object bkOCR: TButton
      Left = 169
      Top = 9
      Width = 75
      Height = 25
      Hint = 'Perform OCR'
      Caption = 'Recognize'
      ParentShowHint = False
      ShowHint = True
      TabOrder = 2
      OnClick = bkOCRClick
    end
    object edPage: TEdit
      Left = 376
      Top = 11
      Width = 25
      Height = 21
      Hint = 'Current page number for images that have multiple pages'
      MaxLength = 3
      ParentShowHint = False
      ShowHint = True
      TabOrder = 3
      Text = '1'
    end
    object bkSetPage: TButton
      Left = 452
      Top = 13
      Width = 52
      Height = 21
      Hint = 'Set selected page index'
      Caption = 'Set'
      ParentShowHint = False
      ShowHint = True
      TabOrder = 4
      OnClick = bkSetPageClick
    end
    object cbScale: TComboBox
      Left = 51
      Top = 48
      Width = 95
      Height = 21
      Hint = 'Scale image for best results'
      Style = csDropDownList
      DropDownCount = 20
      ItemIndex = 0
      ParentShowHint = False
      ShowHint = True
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
    object bkHelp: TButton
      Left = 429
      Top = 44
      Width = 75
      Height = 25
      Caption = 'Help'
      TabOrder = 6
      OnClick = bkHelpClick
    end
    object gbLanguages: TGroupBox
      Left = 510
      Top = 5
      Width = 247
      Height = 65
      Caption = 'Languages '
      TabOrder = 7
      object bkLanguages: TButton
        Left = 52
        Top = 25
        Width = 141
        Height = 25
        Caption = 'Select Languages'
        TabOrder = 0
        OnClick = bkLanguagesClick
      end
    end
    object cbExactCopy: TCheckBox
      Left = 775
      Top = 52
      Width = 177
      Height = 17
      Caption = 'Exact copy (do not format text)'
      TabOrder = 8
      OnClick = cbExactCopyClick
    end
    object bkSave: TButton
      Left = 250
      Top = 9
      Width = 75
      Height = 25
      Hint = 'Save OCR results'
      Caption = 'Save'
      ParentShowHint = False
      ShowHint = True
      TabOrder = 9
      OnClick = bkSaveClick
    end
    object bkScan: TButton
      Left = 7
      Top = 9
      Width = 75
      Height = 25
      Hint = 'Scan Image for OCR'
      Caption = 'Scan'
      ParentShowHint = False
      ShowHint = True
      TabOrder = 0
      OnClick = bkScanClick
    end
    object bkOptions: TButton
      Left = 348
      Top = 44
      Width = 75
      Height = 25
      Caption = 'Options'
      TabOrder = 10
      OnClick = bkOptionsClick
    end
  end
  object pnText: TPanel
    Left = 518
    Top = 75
    Width = 519
    Height = 573
    Align = alRight
    TabOrder = 1
    object mmText: TMemo
      Left = 1
      Top = 1
      Width = 517
      Height = 571
      Align = alClient
      ReadOnly = True
      ScrollBars = ssBoth
      TabOrder = 0
    end
  end
  object pnImg: TPanel
    Left = 0
    Top = 75
    Width = 515
    Height = 573
    Align = alClient
    TabOrder = 2
    OnResize = pnImgResize
    object pbImg: TPaintBox
      Left = 1
      Top = 1
      Width = 513
      Height = 523
      Align = alClient
      OnMouseDown = pbImgMouseDown
      OnMouseMove = pbImgMouseMove
      OnMouseUp = pbImgMouseUp
      OnPaint = pbImgPaint
      ExplicitLeft = 0
      ExplicitHeight = 536
    end
    object Panel1: TPanel
      Left = 1
      Top = 524
      Width = 513
      Height = 48
      Align = alBottom
      TabOrder = 0
      object cbBin: TCheckBox
        Left = 7
        Top = 0
        Width = 138
        Height = 17
        Caption = 'Display binarized image'
        TabOrder = 0
        OnClick = cbBinClick
      end
      object bkLoadBlocks: TButton
        Left = 168
        Top = 5
        Width = 75
        Height = 25
        Caption = 'Load Zones'
        Enabled = False
        TabOrder = 1
        OnClick = bkLoadBlocksClick
      end
      object bkSaveBlocks: TButton
        Left = 249
        Top = 5
        Width = 75
        Height = 25
        Caption = 'Save Zones'
        Enabled = False
        TabOrder = 2
        OnClick = bkSaveBlocksClick
      end
      object bkClearBlocks: TButton
        Left = 330
        Top = 5
        Width = 75
        Height = 25
        Caption = 'Clear Zones'
        Enabled = False
        TabOrder = 3
        OnClick = bkClearBlocksClick
      end
      object bkDetectBlocks: TButton
        Left = 411
        Top = 5
        Width = 75
        Height = 25
        Caption = 'Detect Zones'
        Enabled = False
        TabOrder = 4
        OnClick = bkDetectBlocksClick
      end
      object cbPixLines: TCheckBox
        Left = 7
        Top = 15
        Width = 114
        Height = 17
        Caption = 'Show image lines'
        TabOrder = 5
        OnClick = cbPixLinesClick
      end
      object cbCharRects: TCheckBox
        Left = 7
        Top = 30
        Width = 106
        Height = 17
        Caption = 'Show char rects'
        TabOrder = 6
        OnClick = cbPixLinesClick
      end
    end
  end
  object stBar: TStatusBar
    Left = 0
    Top = 648
    Width = 1037
    Height = 19
    Panels = <
      item
        Width = 300
      end
      item
        Width = 200
      end
      item
        Width = 200
      end
      item
        Width = 200
      end
      item
        Width = 200
      end>
  end
  object opFile: TOpenDialog
    Filter = 
      'Image File (JPG,BMP,PNG,TIF,GIF,PDF)|*.JPG;*JPEG;*.BMP;*.PNG;*.T' +
      'IF;*.TIFF;*.GIF;*.PDF|All Files|*'
    Title = 'Open Image for OCR'
    Left = 336
    Top = 88
  end
  object pmBlock: TPopupMenu
    OnPopup = pmBlockPopup
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
      Caption = 'Type: Zoning Area'
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
      Caption = 'Zone Inverted'
      OnClick = nnInvertClick
    end
    object nnRotate90: TMenuItem
      Caption = 'Text Rotated 90'#176' clockwise'
      OnClick = nnRotate90Click
    end
    object nnRotate270: TMenuItem
      Caption = 'Text Rotated 90'#176' counterclockwise'
      OnClick = nnRotate270Click
    end
    object nnRotate180: TMenuItem
      Caption = 'Text Rotated 180'#176
      OnClick = nnRotate180Click
    end
    object nnMirrorH: TMenuItem
      Caption = 'Text Mirrored Horizontally'
      OnClick = nnMirrorHClick
    end
    object nnMirrorV: TMenuItem
      Caption = 'Text Mirrored Vertically'
      OnClick = nnMirrorVClick
    end
    object N2: TMenuItem
      Caption = '-'
    end
    object nnSetRegExp: TMenuItem
      Caption = 'Set Regular expression'
      OnClick = nnSetRegExpClick
    end
    object N3: TMenuItem
      Caption = '-'
    end
    object nnSaveAsImage: TMenuItem
      Caption = 'Save Zone As Image File'
      OnClick = nnSaveAsImageClick
    end
    object N4: TMenuItem
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
    Title = 'Save Output Document'
    Left = 416
    Top = 88
  end
  object svImg: TSaveDialog
    DefaultExt = '*.bmp'
    Filter = 
      'BMP image (*.bmp)|*.bmp|JPEG image (*.jpg)|*.jpg|PNG image (*.pn' +
      'g)|*.png|TIFF image (*.tif)|*.tif'
    Options = [ofOverwritePrompt, ofHideReadOnly, ofEnableSizing]
    Title = 'Save As Image'
    Left = 416
    Top = 152
  end
end
