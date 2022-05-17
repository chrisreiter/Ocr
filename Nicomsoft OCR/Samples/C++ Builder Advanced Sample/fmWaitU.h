//---------------------------------------------------------------------------

#ifndef fmWaitUH
#define fmWaitUH
//---------------------------------------------------------------------------
#include <Classes.hpp>
#include <Controls.hpp>
#include <StdCtrls.hpp>
#include <Forms.hpp>
#include <ComCtrls.hpp>
#include <ExtCtrls.hpp>
//---------------------------------------------------------------------------
class TfmWait : public TForm
{
__published:	// IDE-managed Components
	TLabel *fmWait;
	TProgressBar *ProgressBar1;
	TButton *bkCancel;
	TTimer *Timer1;
	void __fastcall FormShow(TObject *Sender);
	void __fastcall FormClose(TObject *Sender, TCloseAction &Action);
	void __fastcall Timer1Timer(TObject *Sender);
	void __fastcall bkCancelClick(TObject *Sender);
private:	// User declarations
	bool CheckDone();
public:		// User declarations
	__fastcall TfmWait(TComponent* Owner);

	int res;
	int mode;
};
//---------------------------------------------------------------------------
extern PACKAGE TfmWait *fmWait;
//---------------------------------------------------------------------------
#endif
