//---------------------------------------------------------------------------

#include <vcl.h>
#pragma hdrstop

#include "fmWaitU.h"
//---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma resource "*.dfm"
TfmWait *fmWait;

#include "fmMainU.h"

//---------------------------------------------------------------------------
__fastcall TfmWait::TfmWait(TComponent* Owner)
	: TForm(Owner)
{
}

bool TfmWait::CheckDone()
{
  if (mode == 0)
	res = fmMain->NsOCR->Img_OCR(fmMain->ImgObj, 0, 0, OCRFLAG_GETRESULT);
  else
    res = fmMain->NsOCR->Ocr_ProcessPages(fmMain->ImgObj, 0, 0, 0, 0, OCRFLAG_GETRESULT);
  return res != ERROR_PENDING;
}

//---------------------------------------------------------------------------
void __fastcall TfmWait::FormShow(TObject *Sender)
{
  Timer1->Enabled = true;
  ProgressBar1->Position = 0;
}
//---------------------------------------------------------------------------

void __fastcall TfmWait::FormClose(TObject *Sender, TCloseAction &Action)
{
  Timer1->Enabled=false;
  while (!CheckDone()) //make sure that Img_OCR is done
	Sleep(10);
}
//---------------------------------------------------------------------------

void __fastcall TfmWait::Timer1Timer(TObject *Sender)
{
  int val;
  if (CheckDone())
	Close();

  if (mode == 0)
	  val = fmMain->NsOCR->Img_OCR(fmMain->ImgObj, 0, 0, OCRFLAG_GETPROGRESS);
  else
	  val = fmMain->NsOCR->Ocr_ProcessPages(fmMain->ImgObj, 0, 0, 0, 0, OCRFLAG_GETPROGRESS);

  if (val < ERROR_FIRST)
	if (ProgressBar1->Position != val)
	  ProgressBar1->Position = val;
}
//---------------------------------------------------------------------------

void __fastcall TfmWait::bkCancelClick(TObject *Sender)
{
  //cancel work
  if (mode == 0)
	  fmMain->NsOCR->Img_OCR(fmMain->ImgObj, 0, 0, OCRFLAG_CANCEL);
  else
      fmMain->NsOCR->Ocr_ProcessPages(fmMain->ImgObj, 0, 0, 0, 0, OCRFLAG_CANCEL);

  //we must wait for result anyway since OCRFLAG_CANCEL request returns immediately, work is not stopped yet
  Close(); //this function waits for result
}
//---------------------------------------------------------------------------

