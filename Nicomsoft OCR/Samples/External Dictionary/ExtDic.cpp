// ExtDic.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "ExtDic.h"

/*   HOW TO INSTALL EXTERNAL DICTIONARY

1. Compile this project, get "ExtDic.dll" file.
2. Copy "ExtDic.dll" to "Bin" folder of NSOCR (Bin_64 for x64 version)
3. Open Config.dat in Notepad (you can also change settings in runtime) and 
change line

	<Dictionaries>
		<ExternalDLL></ExternalDLL>

to

	<Dictionaries>
		<ExternalDLL>ExtDic.dll</ExternalDLL>

By defaults, not all possible word variants are passed to dictionary,
you can change it, disable preprocessing and just pass every variant to external dictionary,
to do it change the line

		<ExternalFlags>0</ExternalFlags>

to

		<ExternalFlags>1</ExternalFlags>

*/


// external dictionary function
// return 1 if the word is good
// return 0 if the word is bad
//BlockIndex is a block index, (0 ... Img_GetBlockCnt()-1)
//Note this function can be called many thousands times, it must work quickly

NSOCRAPI IsGoodWord(wchar_t* word, int BlockIndex, void* Reserved)
{
	return 1; //just mark all words as good in this sample
}

