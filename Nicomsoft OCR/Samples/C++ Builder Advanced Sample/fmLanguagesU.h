//---------------------------------------------------------------------------

#ifndef fmLanguagesUH
#define fmLanguagesUH
//---------------------------------------------------------------------------
#include <System.Classes.hpp>
#include <Vcl.Controls.hpp>
#include <Vcl.StdCtrls.hpp>
#include <Vcl.Forms.hpp>
#include <Vcl.CheckLst.hpp>
//---------------------------------------------------------------------------
class TfmLanguages : public TForm
{
__published:	// IDE-managed Components
	TCheckListBox *cbLanguages;
	TButton *bkOK;
	TButton *bkCancel;
	TLabel *Label1;
	TLabel *Label2;
	TCheckListBox *cbLanguagesAsian;
	TLabel *Label3;
	TLabel *Label4;
	void __fastcall bkCancelClick(TObject *Sender);
	void __fastcall bkOKClick(TObject *Sender);
	void __fastcall FormShow(TObject *Sender);
private:	// User declarations
   void LoadLanguages();
   bool ApplyLanguages();
public:		// User declarations
	__fastcall TfmLanguages(TComponent* Owner);
};
//---------------------------------------------------------------------------
extern PACKAGE TfmLanguages *fmLanguages;
//---------------------------------------------------------------------------
#endif
