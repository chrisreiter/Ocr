#INCLUDE vfp_nsocr.h

Declare Long GetWindowDC IN "user32.dll" Long hWnd 
Declare Long CreateCompatibleDC IN "gdi32.dll" Long hdc
Declare Long DeleteDC IN "gdi32.dll" Long hdc
Declare Long DeleteObject IN "gdi32.dll" Long hObject
Declare Long ReleaseDC IN "user32.dll" Long hWnd, Long hdc
Declare Long CreateCompatibleBitmap IN "gdi32.dll" Long hdc , Long nWidth, Long nHeight
Declare Long BitBlt IN "gdi32.dll" Long hDestDC, Long X, Long Y, Long nWidth, Long nHeight, Long hSrcDC, Long xSrc, Long ySrc, Long dwRop
Declare Long SelectObject IN "gdi32.dll" Long hdc, Long hObject
Declare Long MoveToEx IN "gdi32.dll" Long hdc, Long X, Long Y, Long lpPoint
Declare Long LineTo IN "gdi32.dll" Long hdc, Long X, Long Y
Declare Long CreatePen IN "gdi32.dll" Long nPenStyle, Long nWidth, Long crColor
Declare Long TextOutA IN "gdi32.dll" Long hdc, Long X, Long Y, String lpString, Long nCount
Declare Long SetTextColor IN "gdi32.dll" Long hdc, Long Colr
Declare Long CreateFontA IN "gdi32.dll" Long nHeight, Long nWidth, Long nEscapement, Long nOrientation,; 
Long fnWeight, Long fdwItalic, Long fdwUnderline, Long fdwStrikeOut, Long fdwCharSet, Long fdwOutputPrecision,;
Long fdwClipPrecision, Long fdwQuality, Long fdwPitchAndFamily, String lpszFace
Declare Long GetDC IN "user32.dll" Long hWnd
Declare Long GetDeviceCaps IN "gdi32.dll" Long hdc, Long nIndex
Declare Long MulDiv IN "kernel32.dll" Long nNumber, Long nNumerator, Long nDenominator
Declare Long CreateSolidBrush IN "gdi32.dll" Long crColor
Declare Long SetBkColor IN "gdi32.dll" Long hdc, Long crColor
Declare Long FillRect IN "user32.dll" LONG hdc, String lpRect, Long hbrush
Declare Long SetRect IN "user32.dll" (String lpRect, Long X1, Long Y1, Long X2, Long Y2
DECLARE LONG GetSystemMetrics IN user32 LONG nIndex
Declare LONG ShellExecuteA IN "shell32.dll" Long hWnd, String lpOperation, String lpFile, STRING lpParameters, STRING lpDirectory, LONG nShowCmd

LOCAL lcPath
lcPath = JUSTPATH(SYS(16))
SET DEFAULT TO (lcPath)

DO FORM "..\forms\form1.scx" 
READ EVENTS
CLEAR ALL

 &&-----------------------------------------------------------------
   &&- Function:    DecToHex
   &&- Summary:     Converts decimal integers to hex strings, whether
   &&-              signed or unsigned.
   &&- Parameters:  liDecNumber - decimal number to convert.
   &&-----------------------------------------------------------------
   FUNCTION DecToHex
   PARAMETERS liDecNumber
   lsHexNumber = ""
   IF liDecNumber > 0 && It's not negative, so do a straight TRANSFORM.
      lsHexNumber = TRANSFORM(liDecNumber, "@0")
   ELSE

      &&-- The number is negative, so we'll have to do a little more work,
      &&-- since it's not as straight forward as converting and adding a
      &&-- minus sign.

      &&-- Find the length of the resulting hex string.
      lsHexNumber = TRANSFORM(ABS(liDecNumber), "@0")
      IF SUBSTR(lsHexNumber, 3, 1) = "0" && The number has filled the
                                         && eight places of DWORD.
         liLength = LEN(SUBSTR(lsHexNumber, NotAt("0", lsHexNumber, 2)))
      ELSE
         &&-- Subtract to account for "0x".
         liLength = LEN(TRANSFORM(ABS(liDecNumber), "@0")) - 2
      ENDIF

      lsTempHex = 0xFFFFFFFF
      lsHexNumber = TRANSFORM(lsTempHex-ABS(liDecNumber) + 1, "@0")
   ENDIF
   RETURN lsHexNumber

   &&-----------------------------------------------------------------
   &&- Function:    NotAt
   &&- Summary:     From within a passed string, finds the first
   &&-              occurrence of a character that is not the character
   &&-              specified.  In other words, this function works
   &&-              opposite of the manner that AT() does.
   &&- Parameters:  lsNotString -    What we don't want.  The function
   &&-                               finds the first character that is
   &&-                               not lsNotString.
   &&-              lsSearchString - The string in which to search.
   &&-              liOccurence -    Indicates that NotAt should find
   &&-                               the liOccurence of a character
   &&-                               that is not lsNotString.
   &&-----------------------------------------------------------------
   FUNCTION NotAt()
   PARAMETERS lsNotString, lsSearchString, liOccurrence

   llFound       = .F.  && Flag indicates if we've found a character that
                        && is NOT lsNotString
   llEndOfString = .F.  && Flag to indicate that we've reached the end
                        && of the string.
   lnCounter     = 0    && Tracks position during the search.
   lnLength      = LEN(lsSearchString)
   liOccurCount  = 0    && Tracks how many occurrences have been found.

   &&-- Loop until the desired character is found or the end of
   &&-- the string is reached.
   DO WHILE NOT llFound AND NOT llEndofString
      lnCounter = lnCounter + 1
      lsCompare = SUBSTR(lsSearchString, lnCounter, 1)
      IF lsCompare <> lsNotString
         liOccurCount = liOccurCount + 1

         &&-- Have we found the occurrence we want?
         IF liOccurCount = liOccurrence
            llFound = .T.
         ENDIF
      ENDIF
      IF lnCounter = lnLength
         llEndOfString = .T.
      ENDIF

   ENDDO
   RETURN lnCounter
   &&-- Code ends here.


