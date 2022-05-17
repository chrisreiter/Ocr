//---------------------------------------------------------------------------

#ifndef fmMainUH
#define fmMainUH
//---------------------------------------------------------------------------
#include <Classes.hpp>
#include <Controls.hpp>
#include <StdCtrls.hpp>
#include <Forms.hpp>
#include <ComCtrls.hpp>
#include <Dialogs.hpp>
#include <ExtCtrls.hpp>
#include <Menus.hpp>


#include "../NSOCR.h"

//---------------------------------------------------------------------------
class TfmMain : public TForm
{
__published:	// IDE-managed Components
	TSplitter *Splitter1;
	TPanel *pnTop;
	TLabel *lbWait;
	TLabel *Label1;
	TLabel *lbPages;
	TLabel *Label2;
	TButton *bkOpen;
	TButton *bkOCR;
	TEdit *edPage;
	TButton *bkSetPage;
	TComboBox *cbScale;
	TGroupBox *gbLanguages;
	TPanel *pnText;
	TMemo *mmText;
	TPanel *pnImg;
	TPaintBox *pbImg;
	TStatusBar *stBar;
	TOpenDialog *opFile;
	TCheckBox *cbExactCopy;
	TPanel *Panel1;
	TCheckBox *cbBin;
	TPopupMenu *pmBlock;
	TMenuItem *nnTypeOCRText;
	TMenuItem *nnTypeICRDigit;
	TMenuItem *nnTypePicture;
	TMenuItem *nnTypeClear;
	TMenuItem *N1;
	TMenuItem *nnInvert;
	TMenuItem *nnRotate90;
	TMenuItem *nnRotate180;
	TMenuItem *nnRotate270;
	TMenuItem *nnMirrorH;
	TMenuItem *nnMirrorV;
	TMenuItem *N2;
	TMenuItem *nnDeleteBlock;
	TButton *bkLoadBlocks;
	TButton *bkSaveBlocks;
	TButton *bkClearBlocks;
	TOpenDialog *opBlocks;
	TSaveDialog *svBlocks;
	TButton *bkDetectBlocks;
	TButton *bkSave;
	TSaveDialog *svFile;
	TButton *bkScan;
	TMenuItem *nnTypeOCRDigit;
	TMenuItem *nnTypeZoning;
	TMenuItem *nnTypeBarCode;
	TMenuItem *nnTypeTable;
	TButton *bkLanguages;
	TMenuItem *nnTypeMRZ;
	void __fastcall FormCreate(TObject *Sender);
	void __fastcall pbImgPaint(TObject *Sender);
	void __fastcall bkOpenClick(TObject *Sender);
	void __fastcall pnImgResize(TObject *Sender);
	void __fastcall cbScaleClick(TObject *Sender);
	void __fastcall bkSetPageClick(TObject *Sender);
	void __fastcall bkOCRClick(TObject *Sender);
	void __fastcall cbExactCopyClick(TObject *Sender);
	void __fastcall pbImgMouseUp(TObject *Sender, TMouseButton Button, TShiftState Shift,
          int X, int Y);
	void __fastcall pbImgMouseDown(TObject *Sender, TMouseButton Button, TShiftState Shift,
          int X, int Y);
	void __fastcall pbImgMouseMove(TObject *Sender, TShiftState Shift, int X, int Y);
	void __fastcall cbBinClick(TObject *Sender);
	void __fastcall nnTypeOCRTextClick(TObject *Sender);
	void __fastcall nnTypeICRDigitClick(TObject *Sender);
	void __fastcall nnTypePictureClick(TObject *Sender);
	void __fastcall nnTypeClearClick(TObject *Sender);
	void __fastcall nnInvertClick(TObject *Sender);
	void __fastcall nnRotate90Click(TObject *Sender);
	void __fastcall nnRotate180Click(TObject *Sender);
	void __fastcall nnRotate270Click(TObject *Sender);
	void __fastcall nnMirrorHClick(TObject *Sender);
	void __fastcall nnMirrorVClick(TObject *Sender);
	void __fastcall nnDeleteBlockClick(TObject *Sender);
	void __fastcall bkLoadBlocksClick(TObject *Sender);
	void __fastcall bkSaveBlocksClick(TObject *Sender);
	void __fastcall bkClearBlocksClick(TObject *Sender);
	void __fastcall bkDetectBlocksClick(TObject *Sender);
	void __fastcall bkSaveClick(TObject *Sender);
	void __fastcall bkScanClick(TObject *Sender);
	void __fastcall FormClose(TObject *Sender, TCloseAction &Action);
	void __fastcall nnTypeOCRDigitClick(TObject *Sender);
	void __fastcall nnTypeZoningClick(TObject *Sender);
	void __fastcall nnTypeBarCodeClick(TObject *Sender);
	void __fastcall nnTypeTableClick(TObject *Sender);
	void __fastcall bkLanguagesClick(TObject *Sender);
	void __fastcall nnTypeMRZClick(TObject *Sender);

private:	// User declarations
	void AdjustDocScale();
	bool IsImgLoaded();
	float GetCurDocScale();
	void ShowImage();
	void ShowText();
	void ProcessEntireDocument();
public:		// User declarations
	__fastcall TfmMain(TComponent* Owner);

	TNSOCR* NsOCR;

    HCFG CfgObj;
    HOCR OcrObj;
	HIMG ImgObj;
	HSCAN ScanObj;
	HSVR SvrObj;

	RECT Frame;
	bool Dwn;

	void ShowError(UNICODECHAR* ProcName, int ErrorCode);
	void DoImageLoaded();

	Graphics::TBitmap* bmp;
};
//---------------------------------------------------------------------------
extern PACKAGE TfmMain *fmMain;
//---------------------------------------------------------------------------
#endif
