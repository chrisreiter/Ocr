//---------------------------------------------------------------------------

#ifndef fmScanUH
#define fmScanUH
//---------------------------------------------------------------------------
#include <Classes.hpp>
#include <Controls.hpp>
#include <StdCtrls.hpp>
#include <Forms.hpp>
//---------------------------------------------------------------------------
class TfmScan : public TForm
{
__published:	// IDE-managed Components
	TLabel *Label1;
	TLabel *Label2;
	TComboBox *cbScanners;
	TButton *bkScan;
	TButton *bkCancel;
	TCheckBox *cbNoUI;
	TButton *bkDefault;
	TComboBox *cbSrc;
	void __fastcall FormShow(TObject *Sender);
	void __fastcall bkDefaultClick(TObject *Sender);
	void __fastcall bkScanClick(TObject *Sender);
private:	// User declarations
public:		// User declarations
	__fastcall TfmScan(TComponent* Owner);
};
//---------------------------------------------------------------------------
extern PACKAGE TfmScan *fmScan;
//---------------------------------------------------------------------------
#endif
