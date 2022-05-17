// JNSOCR.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "JNSOCR.h"

#include "NSOCR.h"
#include "wchar.h"
#include "stdio.h"
#include "stdlib.h"
#include "string.h"
#include "debug.h"
#include "OcrConvert.h"

#include "dllmain.h"

#define MAXSIZE 1024

//-----------------------------------------------------------------------------
jint throwNoClassDefError(JNIEnv *env, char *message )
{
	jclass		 exClass;
	char		*className = (char*)"java/lang/NoClassDefFoundError" ;

	exClass = (*env).FindClass(className);

	if ( exClass == NULL )
	{
		return throwNoClassDefError(env, className);
	}

	return (*env).ThrowNew(exClass, message);
}

//-----------------------------------------------------------------------------
jint throwOutOfMemoryError( JNIEnv *env, char *message )

{
	jclass		 exClass;
	char		*className = (char*)"java/lang/OutOfMemoryError" ;

	exClass = (*env).FindClass(className);
	if ( exClass == NULL )
	{
		return throwNoClassDefError( env, className );
	}

	return (*env).ThrowNew(exClass, message );
}
//-----------------------------------------------------------------------------
jint throwNoSuchMethodError(JNIEnv *env, char *className, char *methodName, char *signature )
{
	jclass		 exClass;
	char		*exClassName = (char*)"java/lang/NoSuchMethodError" ;
	char		*msgBuf;
	jint		 retCode;
	size_t		 nMallocSize;

	exClass = (*env).FindClass(exClassName);

	if ( exClass == NULL )
	{
		return throwNoClassDefError( env, exClassName );
	}

	nMallocSize = strlen(className) + strlen(methodName) + strlen(signature) + 8;

	msgBuf = (char*) malloc( nMallocSize );

	if ( msgBuf == NULL )
	{
		return throwOutOfMemoryError(env, (char*)"throwNoSuchMethodError: allocating msgBuf" );
	}

	memset( msgBuf, 0, nMallocSize );

	strcpy(msgBuf, className );
	strcat(msgBuf, "." );
	strcat(msgBuf, methodName );
	strcat(msgBuf, "." );
	strcat(msgBuf, signature );

	retCode = (*env).ThrowNew(exClass, msgBuf );
	free ( msgBuf );
	return retCode;
}

//-----------------------------------------------------------------------------
jint throwNoSuchFieldError( JNIEnv *env, char *message )
{
	jclass		 exClass;
	char		*className = (char*)"java/lang/NoSuchFieldError" ;

	exClass = (*env).FindClass(className );

	if ( exClass == NULL )
	{
		return throwNoClassDefError( env, className );
	}

	return (*env).ThrowNew(exClass, message );
}



//-----------------------------------------------------------------------------
bool PassValToStringBuffer(JNIEnv *env, jobject StringBuffer, wchar_t* value, int len)
{
   //wchar_t cObjectName [MAX_BINDERY_OBJECT_NAME_LEN];

   jstring   javaString;   
   jclass    cls;   

   jmethodID mid_append;   
   jmethodID mid_delete;   
   jmethodID mid_length;   

   cls = (*env).GetObjectClass(StringBuffer);
   if (cls == NULL)
   {
		throwNoClassDefError(env, (char*)"NSOCR: StringBuffer class error. (PassValToStringBuffer)");
		return false;
   }

    mid_length = (*env).GetMethodID(cls, "length", "()I");
      
   if (0 == mid_length) 
   {
	   
	   throwNoSuchMethodError(env, (char*)"StringBuffer", (char*)"length", (char*)"()I");
	   return false;
   }

   jint jlen   = (*env).CallIntMethod(StringBuffer, mid_length);

   if (jlen != 0)
   {
	   mid_delete = (*env).GetMethodID(cls, "delete","(II)Ljava/lang/StringBuffer;");
   
	   if (0 == mid_delete) 
	   {
		   throwNoSuchMethodError(env, (char*)"StringBuffer", (char*)"delete", (char*)"(II)Ljava/lang/StringBuffer;");
		   return false;
	   }

		(*env).CallObjectMethod(StringBuffer, mid_delete, 0, jlen);
   }

   mid_append = (*env).GetMethodID(cls, "append","(Ljava/lang/String;)Ljava/lang/StringBuffer;");

   if (0 == mid_append)
   {
	   throwNoSuchMethodError(env, (char*)"StringBuffer", (char*)"append", (char*)"(Ljava/lang/String;)Ljava/lang/StringBuffer;");
	   return false;
   }

#ifndef _WIN32
   TUCODEConvert StringConverter;
   StringConverter.Init(C_WCHAR, C_UCS2);

   void* ucs2_string = malloc(len*2);

   if (StringConverter.Convert(ucs2_string, len*2, value, len*sizeof(wchar_t)))
   {
	   // Create new JAVA string
	   javaString = (jstring)(*env).NewString((const jchar*) ucs2_string, len);
   }
   else 
   {
	   throwNoClassDefError(env, (char*)"WCHAR_T -> UCS2 convert failed!");
	   return false;
   }
#else 
	javaString = (jstring)(*env).NewString((const jchar*) value, len);
#endif

   // Пердолим ее в StringBuffer
   (*env).CallIntMethod(StringBuffer, mid_append, javaString);
   return true;
}

//-----------------------------------------------------------------------------
bool PassValToNSInt(JNIEnv *env, jobject NSInt, int value)
{
   jclass    cls;
   jmethodID mid;   
         
   cls = (*env).GetObjectClass(NSInt);
   if (cls == NULL)
   {
		throwNoClassDefError(env, (char*)"NSOCR: NSInt class error. (PassValToNSInt)");
		return false;
   }

   mid = (*env).GetMethodID(cls, (char*)"SetValue", (char*)"(I)V");
   if (0 == mid)
   {
	   throwNoSuchMethodError(env, (char*)"NSInt", (char*)"SetValue", (char*)"(I)V");
	   return false;
   }

   (*env).CallVoidMethod(NSInt, mid, value);
   return true;
}

//-----------------------------------------------------------------------------
int GetValFromNSInt(JNIEnv *env, jobject NSInt)
{
   jclass    cls;
   jmethodID mid;   
         
   cls = (*env).GetObjectClass(NSInt);
   if (cls == NULL)
   {
		throwNoClassDefError(env, (char*)"NSOCR: NSInt class error. (GetValFromNSInt)");
		return false;
   }

   mid = (*env).GetMethodID(cls, (char*)"GetValue","()I");
   if (0 == mid)
   {
	   throwNoSuchMethodError(env, (char*)"NSInt", (char*)"GetValue", (char*)"()I");
	   return false;
   }

   return (*env).CallIntMethod(NSInt, mid);   
}

//-----------------------------------------------------------------------------
//returns strlen + 1. 0 - error 
int GetjString(JNIEnv *env, jstring instr, wchar_t* str, int len)
{
   int StrLen = 0;
   jchar* val = (jchar*) (*env).GetStringChars(instr, NULL);

   if (val)
   {
		StrLen = (*env).GetStringLength(instr);
		if (str == NULL) 
			return StrLen + 1;

		StrLen = StrLen > (len - 1) ? (len - 1) : StrLen;

#ifndef _WIN32
		TUCODEConvert StringConverter;
		StringConverter.Init(C_UCS2, C_WCHAR);
		if (!StringConverter.Convert(str, StrLen*sizeof(wchar_t), val, StrLen*2))
		{
			throwNoClassDefError(env, (char*)"UCS2 -> WCHAR_T convert failed!");
			(*env).ReleaseStringChars(instr, val);
			return 0;
		}
#else
		wcsncpy(str, (wchar_t*) val, StrLen);
#endif
		str[StrLen] = L'\0';
		(*env).ReleaseStringChars(instr, val);
   }
   else 
   {
	   return 0;
   }
   
   return StrLen + 1;
}

//-----------------------------------------------------------------------------
//-----------------------------------------------------------------------------
//-----------------------------------------------------------------------------

/*
 * Class:     NSOCR_Engine
 * Method:    IsDllLoaded
 * Signature: ()Z
 */
JNIEXPORT jboolean JNICALL Java_NSOCR_Engine_IsDllLoaded(JNIEnv *env, jclass self)
{
   
	DBGSTR(L"IsDllLoaded method called");
	if (Loaded && OCRObject)
	{
		int result;

		result = OCRObject->IsDllLoaded();
		DBGSTR(L"IsDllLoaded: %d", result);

		return result;
	}
	DBGSTR(L"IsDllLoaded: Object not found!");
	return false;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Engine_GetVersion
 * Signature: (Ljava/lang/StringBuffer;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Engine_1GetVersion(JNIEnv *env, jclass self, jobject strbuf)
{
	DBGSTR(L"Java_NSOCR_Engine_Engine_1GetVersion begin...");
	if (Loaded && OCRObject)
	{
		UNICODECHAR ver[128];
		int result = OCRObject->Engine_GetVersion(ver);

		if (result < ERROR_FIRST)
		{
			PassValToStringBuffer(env, strbuf, ver, (int)wcslen(ver));
		}

		DBGSTR(L"Java_NSOCR_Engine_Engine_1GetVersion END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Engine_1GetVersion END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Engine_Initialize
 * Signature: ()Z
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Engine_1Initialize(JNIEnv *env, jclass self)
{
	DBGSTR(L"Java_NSOCR_Engine_Engine_Initialize begin...");
	if (Loaded && OCRObject)
	{
		int result = OCRObject->Engine_Initialize();
		DBGSTR(L"Java_NSOCR_Engine_Engine_Initialize END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Engine_Initialize END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}


//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Engine_InitializeAdvanced
 * Signature: (LNSOCR/HCFG;LNSOCR/HOCR;LNSOCR/HIMG;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Engine_1InitializeAdvanced(JNIEnv *env, jclass self, jobject hcfg, jobject hocr, jobject himg)
{
	DBGSTR(L"Java_NSOCR_Engine_Engine_InitializeAdvanced begin...");
	if (Loaded && OCRObject)
	{
		int CfgObj, OcrObj, ImgObj;
		int result = OCRObject->Engine_InitializeAdvanced(&CfgObj, &OcrObj, &ImgObj);
		if (result < ERROR_FIRST)
		{
			PassValToNSInt(env, hcfg, CfgObj);
			PassValToNSInt(env, hocr, OcrObj);
			PassValToNSInt(env, himg, ImgObj);
		}
		DBGSTR(L"Java_NSOCR_Engine_Engine_InitializeAdvanced END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Engine_InitializeAdvanced END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Engine_Uninitialize
 * Signature: ()Z
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Engine_1Uninitialize(JNIEnv *env, jclass self)
{
	DBGSTR(L"Java_NSOCR_Engine_Engine_Uninitialize begin...");
	if (Loaded && OCRObject)
	{
		int result = OCRObject->Engine_Uninitialize();
		DBGSTR(L"Java_NSOCR_Engine_Engine_Uninitialize END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Engine_Uninitialize END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Engine_SetDataDirectory
 * Signature: (Ljava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Engine_1SetDataDirectory(JNIEnv *env, jclass self, jstring directory)
{
	int result = ERROR_FIRST;
	DBGSTR(L"Java_NSOCR_Engine_Engine_1SetDataDirectory begin...");

	if (Loaded && OCRObject)
	{
		wchar_t filename_wc[MAXSIZE];
		if (GetjString(env, directory, filename_wc, sizeof(filename_wc)/sizeof(wchar_t)))
		{
			DBGSTR(L"Java_NSOCR_Engine_Engine_1SetDataDirectory DIRECTORY: %ls", filename_wc);

			result = OCRObject->Engine_SetDataDirectory(filename_wc);
		}
		
		DBGSTR(L"Java_NSOCR_Engine_Engine_1SetDataDirectory END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Engine_1SetDataDirectory END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Engine_SetLicenseKey
 * Signature: (Ljava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Engine_1SetLicenseKey(JNIEnv *env, jclass self, jstring licensekey)
{
	int result = ERROR_FIRST;
	DBGSTR(L"Java_NSOCR_Engine_Engine_1SetLicenseKey begin...");

	if (Loaded && OCRObject)
	{
		wchar_t licensekey_wc[MAXSIZE];
		if (GetjString(env, licensekey, licensekey_wc, sizeof(licensekey_wc)/sizeof(wchar_t)))
		{
			DBGSTR(L"Java_NSOCR_Engine_Engine_1SetLicenseKey key: %ls", licensekey_wc);

			result = OCRObject->Engine_SetLicenseKey(licensekey_wc);
		}
		
		DBGSTR(L"Java_NSOCR_Engine_Engine_1SetLicenseKey END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Engine_1SetLicenseKey END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Cfg_Create
 * Signature: (LNSOCR/HCFG;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Cfg_1Create(JNIEnv *env, jclass self, jobject hcfg)
{
	DBGSTR(L"Java_NSOCR_Engine_Cfg_1Create begin...");

	if (Loaded && OCRObject)
	{
		HCFG hndl;
		int result = OCRObject->Cfg_Create(&hndl);

		if (result == 0)
		{
			 PassValToNSInt(env, hcfg, hndl);
		}

		DBGSTR(L"Java_NSOCR_Engine_Cfg_1Create END: %d", result);
		return result;
	 }

    DBGSTR(L"Java_NSOCR_Engine_Cfg_1Create END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Cfg_Destroy
 * Signature: (LNSOCR/HCFG;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Cfg_1Destroy(JNIEnv *env, jclass self, jobject hcfg)
{
	DBGSTR(L"Java_NSOCR_Engine_Cfg_1Destroy begin...");

	if (Loaded && OCRObject)
	{
		HCFG hndl;

		hndl = GetValFromNSInt(env, hcfg);
		DBGSTR(L"Java_NSOCR_Engine_Cfg_1Destroy HCFG:%d", hndl);

		int result = OCRObject->Cfg_Destroy(hndl);

		DBGSTR(L"Java_NSOCR_Engine_Cfg_1Destroy END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Cfg_1Destroy END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Cfg_LoadOptions
 * Signature: (LNSOCR/HCFG;Ljava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Cfg_1LoadOptions(JNIEnv *env, jclass self, jobject hcfg, jstring filename)
{
	int result = ERROR_FIRST;
	DBGSTR(L"Java_NSOCR_Engine_Cfg_1LoadOptions begin...");

	if (Loaded && OCRObject)
	{
		wchar_t filename_wc[MAXSIZE];
		HCFG hndl;

		hndl = GetValFromNSInt(env, hcfg);

		if (GetjString(env, filename, filename_wc, sizeof(filename_wc)/sizeof(wchar_t)))
		{
			DBGSTR(L"Java_NSOCR_Engine_Cfg_1LoadOptions HCFG:%d", hndl);
			DBGSTR(L"Java_NSOCR_Engine_Cfg_1LoadOptions FILENAME: %ls", filename_wc);

			result = OCRObject->Cfg_LoadOptions(hndl, filename_wc);
		}
		
		DBGSTR(L"Java_NSOCR_Engine_Cfg_1LoadOptions END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Cfg_1LoadOptions END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Cfg_SaveOptions
 * Signature: (LNSOCR/HCFG;Ljava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Cfg_1SaveOptions(JNIEnv *env, jclass self, jobject hcfg, jstring filename)
{
	int result = ERROR_FIRST;
	DBGSTR(L"Java_NSOCR_Engine_Cfg_1SaveOptions begin...");

	if (Loaded && OCRObject)
	{
		wchar_t filename_wc[MAXSIZE];
		HCFG hndl;

		hndl = GetValFromNSInt(env, hcfg);

		if (GetjString(env, filename, filename_wc, sizeof(filename_wc)/sizeof(wchar_t)))
		{
			DBGSTR(L"Java_NSOCR_Engine_Cfg_1SaveOptions HCFG:%d", hndl);
			DBGSTR(L"Java_NSOCR_Engine_Cfg_1SaveOptions FILENAME: %ls", filename_wc);

			result = OCRObject->Cfg_SaveOptions(hndl, filename_wc);
		}
		
		DBGSTR(L"Java_NSOCR_Engine_Cfg_1SaveOptions END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Cfg_1SaveOptions END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Cfg_GetOption
 * Signature: (LNSOCR/HCFG;ILjava/lang/String;Ljava/lang/StringBuffer;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Cfg_1GetOption(JNIEnv *env, jclass self, jobject hcfg, jint BlockType, jstring OptionPath, jobject OptionValue)
{
	int result = ERROR_FIRST;
	DBGSTR(L"Java_NSOCR_Engine_Cfg_1GetOption begin...");

	if (Loaded && OCRObject)
	{
		wchar_t OptionPath_wc[MAXSIZE];
		wchar_t Value_wc[MAXSIZE];
		HCFG    hndl;

		hndl = GetValFromNSInt(env, hcfg);

		if (GetjString(env, OptionPath, OptionPath_wc, sizeof(OptionPath_wc)/sizeof(wchar_t)))
		{
			DBGSTR(L"Java_NSOCR_Engine_Cfg_1GetOption HCFG:%d", hndl);
			DBGSTR(L"Java_NSOCR_Engine_Cfg_1GetOption OPTIONPATH: %ls", OptionPath_wc);

			result = OCRObject->Cfg_GetOption(hndl, BlockType, OptionPath_wc, Value_wc, sizeof(Value_wc)/sizeof(wchar_t));

			if (result < ERROR_FIRST)
			{
				DBGSTR(L"Java_NSOCR_Engine_Cfg_1GetOption VALUE: %ls", Value_wc);
				PassValToStringBuffer(env, OptionValue, Value_wc, (int)wcslen(Value_wc));
			}
		}
		
		DBGSTR(L"Java_NSOCR_Engine_Cfg_1GetOption END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Cfg_1GetOption END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Cfg_SetOption
 * Signature: (LNSOCR/HCFG;ILjava/lang/String;Ljava/lang/String;)I
 */

//#pragma comment(lib, "User32.lib")


JNIEXPORT jint JNICALL Java_NSOCR_Engine_Cfg_1SetOption(JNIEnv *env, jclass self, jobject hcfg, jint BlockType, jstring OptionPath, jstring OptionValue)
{
	int result = ERROR_FIRST;
	DBGSTR(L"Java_NSOCR_Engine_Cfg_1SetOption begin...");
	if (Loaded && OCRObject)
	{
		wchar_t OptionPath_wc[MAXSIZE];
		wchar_t Value_wc[MAXSIZE];
		HCFG    hndl;

		hndl = GetValFromNSInt(env, hcfg);
		if (GetjString(env, OptionPath, OptionPath_wc, sizeof(OptionPath_wc)/sizeof(wchar_t)))
		{
			if (GetjString(env, OptionValue, Value_wc, sizeof(Value_wc)/sizeof(wchar_t)))
			{
				//MessageBox(0, OptionPath_wc, Value_wc, 0);
				DBGSTR(L"Java_NSOCR_Engine_Cfg_1SetOption HCFG:%d", hndl);
				DBGSTR(L"Java_NSOCR_Engine_Cfg_1SetOption OPTIONPATH: %ls", OptionPath_wc);
				//printf("Java_NSOCR_Engine_Cfg_1SetOption VALUE: %ls", Value_wc);

				result = OCRObject->Cfg_SetOption(hndl, BlockType, OptionPath_wc, Value_wc);
			}
		}
		
		DBGSTR(L"Java_NSOCR_Engine_Cfg_1SetOption END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Cfg_1SetOption END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Cfg_DeleteOption
 * Signature: (LNSOCR/HCFG;ILjava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Cfg_1DeleteOption(JNIEnv *env, jclass self, jobject hcfg, jint BlockType, jstring OptionPath)
{
	int result = ERROR_FIRST;
	DBGSTR(L"Java_NSOCR_Engine_Cfg_1DeleteOption begin...");

	if (Loaded && OCRObject)
	{
		wchar_t OptionPath_wc[MAXSIZE];
		HCFG    hndl;

		hndl = GetValFromNSInt(env, hcfg);

		if (GetjString(env, OptionPath, OptionPath_wc, sizeof(OptionPath_wc)/sizeof(wchar_t)))
		{
			DBGSTR(L"Java_NSOCR_Engine_Cfg_1DeleteOption HCFG:%d", hndl);
			DBGSTR(L"Java_NSOCR_Engine_Cfg_1DeleteOption OPTIONPATH: %ls", OptionPath_wc);

			result = OCRObject->Cfg_DeleteOption(hndl, BlockType, OptionPath_wc);
		}
		
		DBGSTR(L"Java_NSOCR_Engine_Cfg_1DeleteOption END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Cfg_1DeleteOption END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Cfg_LoadOptionsFromString
 * Signature: (LNSOCR/HCFG;Ljava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Cfg_1LoadOptionsFromString(JNIEnv *env, jclass self, jobject hcfg, jstring XMLString)
{
	int result = ERROR_FIRST;
	DBGSTR(L"Java_NSOCR_Engine_Cfg_1LoadOptionsFromString begin...");

	if (Loaded && OCRObject)
	{
		wchar_t XMLString_wc[MAXSIZE];
		HCFG    hndl;

		hndl = GetValFromNSInt(env, hcfg);

		if (GetjString(env, XMLString, XMLString_wc, sizeof(XMLString_wc)/sizeof(wchar_t)))
		{
			DBGSTR(L"Java_NSOCR_Engine_Cfg_1LoadOptionsFromString HCFG:%d", hndl);
			DBGSTR(L"Java_NSOCR_Engine_Cfg_1LoadOptionsFromString XMLString: %ls", XMLString_wc);

			result = OCRObject->Cfg_LoadOptionsFromString(hndl, XMLString_wc);
		}
		
		DBGSTR(L"Java_NSOCR_Engine_Cfg_1LoadOptionsFromString END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Cfg_1LoadOptionsFromString END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Cfg_SaveOptionsToString
 * Signature: (LNSOCR/HCFG;Ljava/lang/StringBuffer;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Cfg_1SaveOptionsToString(JNIEnv *env, jclass self, jobject hcfg, jobject XMLString)
{
	int result = ERROR_FIRST;
	DBGSTR(L"Java_NSOCR_Engine_Cfg_1SaveOptionsToString begin...");

	if (Loaded && OCRObject)
	{
		wchar_t XMLString_wc[256];
		HCFG    hndl;

		hndl = GetValFromNSInt(env, hcfg);

		int len = sizeof(XMLString_wc)/sizeof(wchar_t)-1;
		result = OCRObject->Cfg_SaveOptionsToString(hndl, XMLString_wc, len+1);

		if (result < ERROR_FIRST)
		{
			 PassValToStringBuffer(env, XMLString, XMLString_wc, len+1);
		}
	
		DBGSTR(L"Java_NSOCR_Engine_Cfg_1SaveOptionsToString END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Cfg_1SaveOptionsToString END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Ocr_Create
 * Signature: (LNSOCR/HCFG;LNSOCR/HOCR;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Ocr_1Create(JNIEnv *env, jclass self, jobject hcfg, jobject hocr)
{
	DBGSTR(L"Java_NSOCR_Engine_Ocr_1Create begin...");

	if (Loaded && OCRObject)
	{
		HCFG hndl_hcfg;
		HCFG hndl_hocr;

		hndl_hcfg = GetValFromNSInt(env, hcfg);

		int result = OCRObject->Ocr_Create(hndl_hcfg, &hndl_hocr);

		if (result == 0)
		{
			 PassValToNSInt(env, hocr, hndl_hocr);
			 DBGSTR(L"Java_NSOCR_Engine_Ocr_1Create HOCR: %d", hndl_hocr);
		}

		DBGSTR(L"Java_NSOCR_Engine_Ocr_1Create END: %d", result);
		return result;
	 }

    DBGSTR(L"Java_NSOCR_Engine_Ocr_1Create END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Ocr_Destroy
 * Signature: (LNSOCR/HOCR;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Ocr_1Destroy(JNIEnv *env, jclass self, jobject hocr)
{
	DBGSTR(L"Java_NSOCR_Engine_Ocr_1Destroy begin...");

	if (Loaded && OCRObject)
	{
		HCFG hndl;

		hndl = GetValFromNSInt(env, hocr);
		DBGSTR(L"Java_NSOCR_Engine_Ocr_1Destroy HOCR:%d", hndl);

		int result = OCRObject->Ocr_Destroy(hndl);

		DBGSTR(L"Java_NSOCR_Engine_Ocr_1Destroy END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Ocr_1Destroy END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Ocr_ProcessPages
 * Signature: (LNSOCR/HIMG;LNSOCR/HSVR;IIII)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Ocr_1ProcessPages
  (JNIEnv *env, jclass self, jobject himg, jobject hsvr, jint PageIndexStart, jint PageIndexEnd, jint OcrObjCnt, jint flags)
{
	DBGSTR(L"Java_NSOCR_Engine_Ocr_1ProcessPages begin...");

	if (Loaded && OCRObject)
	{
		HCFG hndl_himg;
		HCFG hndl_hsvr;

		hndl_himg = GetValFromNSInt(env, himg);
		hndl_hsvr = GetValFromNSInt(env, hsvr);

		DBGSTR(L"Java_NSOCR_Engine_Ocr_1ProcessPages HIMG: %d", hndl_himg);
		DBGSTR(L"Java_NSOCR_Engine_Ocr_1ProcessPages HSVR: %d", hndl_hsvr);

		DBGSTR(L"Java_NSOCR_Engine_Ocr_1ProcessPages PageIndexStart: %d", PageIndexStart);
		DBGSTR(L"Java_NSOCR_Engine_Ocr_1ProcessPages PageIndexEnd: %d", PageIndexEnd);
		DBGSTR(L"Java_NSOCR_Engine_Ocr_1ProcessPages OcrObjCnt: %d", OcrObjCnt);
		DBGSTR(L"Java_NSOCR_Engine_Ocr_1ProcessPages flags: %d", flags);
		
		int result = OCRObject->Ocr_ProcessPages(hndl_himg, hndl_hsvr, PageIndexStart, PageIndexEnd, OcrObjCnt, flags);

		DBGSTR(L"Java_NSOCR_Engine_Ocr_1ProcessPages END: %d", result);
		return result;
	}

    DBGSTR(L"Java_NSOCR_Engine_Ocr_1ProcessPages END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_Create
 * Signature: (LNSOCR/HOCR;LNSOCR/HIMG;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1Create(JNIEnv *env, jclass self, jobject hocr, jobject himg)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1Create begin...");

	if (Loaded && OCRObject)
	{
		HCFG hndl_himg;
		HCFG hndl_hocr;

		hndl_hocr = GetValFromNSInt(env, hocr);

		int result = OCRObject->Img_Create(hndl_hocr, &hndl_himg);

		if (result == 0)
		{
			 PassValToNSInt(env, himg, hndl_himg);
			 DBGSTR(L"Java_NSOCR_Engine_Img_1Create HIMG: %d", hndl_himg);
		}

		DBGSTR(L"Java_NSOCR_Engine_Img_1Create END: %d", result);
		return result;
	 }

    DBGSTR(L"Java_NSOCR_Engine_Img_1Create END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_Destroy
 * Signature: (LNSOCR/HIMG;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1Destroy(JNIEnv *env, jclass self, jobject himg)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1Destroy begin...");

	if (Loaded && OCRObject)
	{
		HIMG hndl;

		hndl = GetValFromNSInt(env, himg);
		DBGSTR(L"Java_NSOCR_Engine_Img_1Destroy HIMG:%d", hndl);

		int result = OCRObject->Img_Destroy(hndl);

		DBGSTR(L"Java_NSOCR_Engine_Img_1Destroy END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Img_1Destroy END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_LoadFile
 * Signature: (LNSOCR/HIMG;Ljava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1LoadFile(JNIEnv *env, jclass self, jobject himg, jstring filename)
{
	int result = ERROR_FIRST;
	DBGSTR(L"Java_NSOCR_Engine_Img_1LoadFile begin...");

	if (Loaded && OCRObject)
	{
		wchar_t filename_wc[MAXSIZE];
		HIMG hndl;

		hndl = GetValFromNSInt(env, himg);

		if (GetjString(env, filename, filename_wc, sizeof(filename_wc)/sizeof(wchar_t)))
		{
			DBGSTR(L"Java_NSOCR_Engine_Img_1LoadFile HIMG:%d", hndl);
			DBGSTR(L"Java_NSOCR_Engine_Img_1LoadFile FILENAME: %ls", filename_wc);

			result = OCRObject->Img_LoadFile(hndl, filename_wc);
		}
		
		DBGSTR(L"Java_NSOCR_Engine_Img_1LoadFile END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Img_1LoadFile END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_Unload
 * Signature: (LNSOCR/HIMG;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1Unload(JNIEnv *env, jclass self, jobject himg)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1Unload begin...");

	if (Loaded && OCRObject)
	{
		HIMG hndl;

		hndl = GetValFromNSInt(env, himg);
		DBGSTR(L"Java_NSOCR_Engine_Img_1Unload HIMG:%d", hndl);

		int result = OCRObject->Img_Unload(hndl);

		DBGSTR(L"Java_NSOCR_Engine_Img_1Unload END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Img_1Unload END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_GetPageCount
 * Signature: (LNSOCR/HIMG;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1GetPageCount(JNIEnv *env, jclass self, jobject himg)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1GetPageCount begin...");

	if (Loaded && OCRObject)
	{
		HIMG hndl;

		hndl = GetValFromNSInt(env, himg);
		DBGSTR(L"Java_NSOCR_Engine_Img_1GetPageCount HIMG:%d", hndl);

		int result = OCRObject->Img_GetPageCount(hndl);

		DBGSTR(L"Java_NSOCR_Engine_Img_1GetPageCount END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Img_1GetPageCount END: 0");
	return 0;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_SetPage
 * Signature: (LNSOCR/HIMG;I)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1SetPage(JNIEnv *env, jclass self, jobject himg, jint pageindex)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1SetPage begin...");

	if (Loaded && OCRObject)
	{
		HIMG hndl;

		hndl = GetValFromNSInt(env, himg);
		DBGSTR(L"Java_NSOCR_Engine_Img_1SetPage HIMG:%d", hndl);

		int result = OCRObject->Img_SetPage(hndl, pageindex);

		DBGSTR(L"Java_NSOCR_Engine_Img_1SetPage END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Img_1SetPage END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_GetSize
 * Signature: (LNSOCR/HIMG;LNSOCR/NSInt;LNSOCR/NSInt;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1GetSize(JNIEnv *env, jclass self, jobject himg, jobject width, jobject height)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1GetSize begin...");

	if (Loaded && OCRObject)
	{
		HIMG hndl;
		int  iwidth;
		int  iheight;

		hndl = GetValFromNSInt(env, himg);
		DBGSTR(L"Java_NSOCR_Engine_Img_1GetSize HIMG:%d", hndl);

		int result = OCRObject->Img_GetSize(hndl, &iwidth, &iheight);

		if (result == 0)
		{
			PassValToNSInt(env, width,  iwidth);
			PassValToNSInt(env, height, iheight);

			DBGSTR(L"Java_NSOCR_Engine_Img_1GetSize W: %d", iwidth);
			DBGSTR(L"Java_NSOCR_Engine_Img_1GetSize H: %d", iheight);
		}

		DBGSTR(L"Java_NSOCR_Engine_Img_1GetSize END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Img_1GetSize END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_DeleteAllBlocks
 * Signature: (LNSOCR/HIMG;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1DeleteAllBlocks(JNIEnv *env, jclass self, jobject himg)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1DeleteAllBlocks begin...");

	if (Loaded && OCRObject)
	{
		HIMG hndl;

		hndl = GetValFromNSInt(env, himg);
		DBGSTR(L"Java_NSOCR_Engine_Img_1DeleteAllBlocks HIMG:%d", hndl);

		int result = OCRObject->Img_DeleteAllBlocks(hndl);

		DBGSTR(L"Java_NSOCR_Engine_Img_1DeleteAllBlocks END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Img_1GetSize END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_AddBlock
 * Signature: (LNSOCR/HIMG;IIIILNSOCR/HBLK;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1AddBlock(JNIEnv *env, jclass self, jobject himg, jint XPos, jint YPos, jint Width, jint Height, jobject hblk)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1AddBlock begin...");

	if (Loaded && OCRObject)
	{
		HCFG hndl_himg;
		HCFG hndl_hblk;

		hndl_himg = GetValFromNSInt(env, himg);

		DBGSTR(L"Java_NSOCR_Engine_Img_1AddBlock HIMG: %d", hndl_himg);
		DBGSTR(L"Java_NSOCR_Engine_Img_1AddBlock XPos: %d", XPos);
		DBGSTR(L"Java_NSOCR_Engine_Img_1AddBlock YPos: %d", YPos);
		DBGSTR(L"Java_NSOCR_Engine_Img_1AddBlock Width: %d",  Width);
		DBGSTR(L"Java_NSOCR_Engine_Img_1AddBlock Height: %d", Height);

		int result = OCRObject->Img_AddBlock(hndl_himg, XPos, YPos, Width, Height, &hndl_hblk);

		if (result < ERROR_FIRST)
		{
			 PassValToNSInt(env, hblk, hndl_hblk);
			 DBGSTR(L"Java_NSOCR_Engine_Img_1AddBlock HBLK: %d", hndl_hblk);
		}

		DBGSTR(L"Java_NSOCR_Engine_Img_1AddBlock END: %d", result);
		return result;
	 }

    DBGSTR(L"Java_NSOCR_Engine_Img_1AddBlock END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_DeleteBlock
 * Signature: (LNSOCR/HIMG;LNSOCR/HBLK;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1DeleteBlock(JNIEnv *env, jclass self, jobject himg, jobject hblk)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1DeleteBlock begin...");

	if (Loaded && OCRObject)
	{
		HCFG hndl_himg;
		HCFG hndl_hblk;

		hndl_himg = GetValFromNSInt(env, himg);
		hndl_hblk = GetValFromNSInt(env, hblk);

		DBGSTR(L"Java_NSOCR_Engine_Img_1DeleteBlock HIMG: %d", hndl_himg);
		DBGSTR(L"Java_NSOCR_Engine_Img_1DeleteBlock HBLK: %d", hndl_hblk);

		int result = OCRObject->Img_DeleteBlock(hndl_himg, hndl_hblk);

		DBGSTR(L"Java_NSOCR_Engine_Img_1DeleteBlock END: %d", result);
		return result;
	 }

    DBGSTR(L"Java_NSOCR_Engine_Img_1DeleteBlock END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_GetBlockCnt
 * Signature: (LNSOCR/HIMG;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1GetBlockCnt(JNIEnv *env, jclass self, jobject himg)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1GetBlockCnt begin...");

	if (Loaded && OCRObject)
	{
		HIMG hndl;

		hndl = GetValFromNSInt(env, himg);
		DBGSTR(L"Java_NSOCR_Engine_Img_1GetBlockCnt HIMG:%d", hndl);

		int result = OCRObject->Img_GetBlockCnt(hndl);

		DBGSTR(L"Java_NSOCR_Engine_Img_1GetBlockCnt END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Img_1GetBlockCnt END: 0");
	return 0;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_GetBlock
 * Signature: (LNSOCR/HIMG;ILNSOCR/HBLK;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1GetBlock(JNIEnv *env, jclass self, jobject himg, jint BlockIndex, jobject hblk)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1GetBlock begin...");

	if (Loaded && OCRObject)
	{
		HCFG hndl_himg;
		HCFG hndl_hblk;

		hndl_himg = GetValFromNSInt(env, himg);

		DBGSTR(L"Java_NSOCR_Engine_Img_1GetBlock HIMG: %d", hndl_himg);
		DBGSTR(L"Java_NSOCR_Engine_Img_1GetBlock BlockIndex: %d", BlockIndex);

		int result = OCRObject->Img_GetBlock(hndl_himg, BlockIndex, &hndl_hblk);

		if (result == 0)
		{
			 PassValToNSInt(env, hblk, hndl_hblk);
			 DBGSTR(L"Java_NSOCR_Engine_Img_1AddBlock HBLK: %d", hndl_hblk);
		}

		DBGSTR(L"Java_NSOCR_Engine_Img_1AddBlock END: %d", result);
		return result;
	 }

    DBGSTR(L"Java_NSOCR_Engine_Img_1AddBlock END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_GetImgText
 * Signature: (LNSOCR/HIMG;Ljava/lang/StringBuffer;I)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1GetImgText(JNIEnv *env, jclass self, jobject himg, jobject text, jint flags)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1GetImgText begin...");

	if (Loaded && OCRObject)
	{
		HIMG hndl_himg;

		hndl_himg = GetValFromNSInt(env, himg);

		DBGSTR(L"Java_NSOCR_Engine_Img_1GetImgText HIMG: %d", hndl_himg);

		int  len = OCRObject->Img_GetImgText(hndl_himg, NULL, 0, flags);

		if (len > ERROR_FIRST)
		{
			DBGSTR(L"Java_NSOCR_Engine_Img_1GetImgText String Length END: %d", len);
			return len;
		}

		wchar_t* wtext = (wchar_t*) malloc(sizeof(wchar_t)*(len+1));

		if (!wtext)
		{
			throwOutOfMemoryError(env, (char*)"JNSOCR Internal error: No memory for string");
			DBGSTR(L"Java_NSOCR_Engine_Img_1GetImgText String Length OUTOFMEMORY: No memory for string");
			return ERROR_FIRST;
		}

		int result = OCRObject->Img_GetImgText(hndl_himg, wtext, len+1, flags);

		if (result < ERROR_FIRST)
		{
			 //DBGSTR(L"Java_NSOCR_Engine_Img_1GetImgText TEXT: %ls", wtext);
			 PassValToStringBuffer(env, text, wtext, len+1);
		}

		free(wtext);

		DBGSTR(L"Java_NSOCR_Engine_Img_1GetImgText END: %d", result);
		return result;
	 }

    DBGSTR(L"Java_NSOCR_Engine_Img_1GetImgText END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_OCR
 * Signature: (LNSOCR/HIMG;III)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1OCR(JNIEnv *env, jclass self, jobject himg, jint FirstStep, jint LastStep, jint flags)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1OCR begin...");

	if (Loaded && OCRObject)
	{
		HIMG hndl;

		hndl = GetValFromNSInt(env, himg);
		DBGSTR(L"Java_NSOCR_Engine_Img_1OCR HIMG:%d", hndl);

		int result = OCRObject->Img_OCR(hndl, FirstStep, LastStep, flags);

		DBGSTR(L"Java_NSOCR_Engine_Img_1OCR END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Img_1OCR END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_LoadBlocks
 * Signature: (LNSOCR/HIMG;Ljava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1LoadBlocks(JNIEnv *env, jclass self, jobject himg, jstring filename)
{
	int result = ERROR_FIRST;
	DBGSTR(L"Java_NSOCR_Engine_Img_1LoadBlocks begin...");

	if (Loaded && OCRObject)
	{
		wchar_t filename_wc[MAXSIZE];
		HCFG hndl;

		hndl = GetValFromNSInt(env, himg);

		if (GetjString(env, filename, filename_wc, sizeof(filename_wc)/sizeof(wchar_t)))
		{
			DBGSTR(L"Java_NSOCR_Engine_Img_1LoadBlocks HIMG:%d", hndl);
			DBGSTR(L"Java_NSOCR_Engine_Img_1LoadBlocks FILENAME: %ls", filename_wc);

			result = OCRObject->Img_LoadBlocks(hndl, filename_wc);
		}
		
		DBGSTR(L"Java_NSOCR_Engine_Img_1LoadBlocks END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Img_1LoadBlocks END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_SaveBlocks
 * Signature: (LNSOCR/HIMG;Ljava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1SaveBlocks(JNIEnv *env, jclass self, jobject himg, jstring filename)
{
	int result = ERROR_FIRST;
	DBGSTR(L"Java_NSOCR_Engine_Img_1SaveBlocks begin...");

	if (Loaded && OCRObject)
	{
		wchar_t filename_wc[MAXSIZE];
		HCFG hndl;

		hndl = GetValFromNSInt(env, himg);

		if (GetjString(env, filename, filename_wc, sizeof(filename_wc)/sizeof(wchar_t)))
		{
			DBGSTR(L"Java_NSOCR_Engine_Img_1SaveBlocks HIMG:%d", hndl);
			DBGSTR(L"Java_NSOCR_Engine_Img_1SaveBlocks FILENAME: %ls", filename_wc);

			result = OCRObject->Img_SaveBlocks(hndl, filename_wc);
		}
		
		DBGSTR(L"Java_NSOCR_Engine_Img_1SaveBlocks END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Img_1SaveBlocks END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_GetSkewAngle
 * Signature: (LNSOCR/HIMG;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1GetSkewAngle(JNIEnv *env, jclass self, jobject himg)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1GetSkewAngle begin...");

	if (Loaded && OCRObject)
	{
		HIMG hndl;

		hndl = GetValFromNSInt(env, himg);
		DBGSTR(L"Java_NSOCR_Engine_Img_1GetSkewAngle HIMG:%d", hndl);

		int result = OCRObject->Img_GetSkewAngle(hndl);

		DBGSTR(L"Java_NSOCR_Engine_Img_1GetSkewAngle END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Img_1GetSkewAngle END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_GetPixLineCnt
 * Signature: (LNSOCR/HIMG;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1GetPixLineCnt(JNIEnv *env, jclass self, jobject himg)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1GetPixLineCnt begin...");

	if (Loaded && OCRObject)
	{
		HIMG hndl;

		hndl = GetValFromNSInt(env, himg);
		DBGSTR(L"Java_NSOCR_Engine_Img_1GetPixLineCnt HIMG:%d", hndl);

		int result = OCRObject->Img_GetPixLineCnt(hndl);

		DBGSTR(L"Java_NSOCR_Engine_Img_1GetPixLineCnt END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Img_1GetPixLineCnt END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_GetPixLine
 * Signature: (LNSOCR/HIMG;ILNSOCR/NSInt;LNSOCR/NSInt;LNSOCR/NSInt;LNSOCR/NSInt;LNSOCR/NSInt;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1GetPixLine(JNIEnv *env, jclass self, jobject himg, jint LineInd, jobject x1, jobject y1, jobject x2, jobject y2, jobject width)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1GetPixLine begin...");

	if (Loaded && OCRObject)
	{
		HIMG hndl;
		int ix1, ix2, iy1, iy2, iwidth;

		hndl = GetValFromNSInt(env, himg);
		DBGSTR(L"Java_NSOCR_Engine_Img_1GetPixLine HIMG:%d", hndl);

		int result = OCRObject->Img_GetPixLine(hndl, LineInd, &ix1, &iy1, &ix2, &iy2, &iwidth);

		if (result < ERROR_FIRST)
		{
			 DBGSTR(L"Java_NSOCR_Engine_Img_1GetPixLine x1: %d",    ix1);
			 DBGSTR(L"Java_NSOCR_Engine_Img_1GetPixLine y1: %d",    iy1);
			 DBGSTR(L"Java_NSOCR_Engine_Img_1GetPixLine x2: %d",    ix2);
			 DBGSTR(L"Java_NSOCR_Engine_Img_1GetPixLine y2: %d",    iy2);
			 DBGSTR(L"Java_NSOCR_Engine_Img_1GetPixLine width: %d", iwidth);

			 PassValToNSInt(env, x1, ix1);
			 PassValToNSInt(env, y1, iy1);
			 PassValToNSInt(env, x2, ix2);
			 PassValToNSInt(env, y2, iy2);
			 PassValToNSInt(env, width, iwidth);
		}

		DBGSTR(L"Java_NSOCR_Engine_Img_1GetPixLine END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Img_1GetPixLine END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_GetScaleFactor
 * Signature: (LNSOCR/HIMG;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1GetScaleFactor(JNIEnv *env, jclass self, jobject himg)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1GetScaleFactor begin...");

	if (Loaded && OCRObject)
	{
		HIMG hndl;

		hndl = GetValFromNSInt(env, himg);
		DBGSTR(L"Java_NSOCR_Engine_Img_1GetScaleFactor HIMG:%d", hndl);

		int result = OCRObject->Img_GetScaleFactor(hndl);

		DBGSTR(L"Java_NSOCR_Engine_Img_1GetScaleFactor END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Img_1GetScaleFactor END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_CalcPointPosition
 * Signature: (LNSOCR/HIMG;LNSOCR/NSInt;LNSOCR/NSInt;I)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1CalcPointPosition(JNIEnv *env, jclass self, jobject himg, jobject xpos, jobject ypos, jint mode)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1CalcPointPosition begin...");

	if (Loaded && OCRObject)
	{
		HIMG hndl;
		int ix, iy;

		hndl = GetValFromNSInt(env, himg);
		ix   = GetValFromNSInt(env, xpos);
		iy   = GetValFromNSInt(env, ypos);

		DBGSTR(L"Java_NSOCR_Engine_Img_1CalcPointPosition HIMG:%d", hndl);
		DBGSTR(L"Java_NSOCR_Engine_Img_1CalcPointPosition XPOS:%d", ix);
		DBGSTR(L"Java_NSOCR_Engine_Img_1CalcPointPosition YPOS:%d", iy);

		int result = OCRObject->Img_CalcPointPosition(hndl, &ix, &iy, mode);

		if (result < ERROR_FIRST)
		{
			 DBGSTR(L"Java_NSOCR_Engine_Img_1CalcPointPosition x: %d",    ix);
			 DBGSTR(L"Java_NSOCR_Engine_Img_1CalcPointPosition y: %d",    iy);

			 PassValToNSInt(env, xpos, ix);
			 PassValToNSInt(env, ypos, iy);
		}

		DBGSTR(L"Java_NSOCR_Engine_Img_1CalcPointPosition END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Img_1CalcPointPosition END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_CopyCurrentPage
 * Signature: (LNSOCR/HIMG;LNSOCR/HIMG;I)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1CopyCurrentPage(JNIEnv *env, jclass self, jobject himgsrc, jobject himgdst, jint Flags)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1CopyCurrentPage begin...");

	if (Loaded && OCRObject)
	{
		HIMG shndl, dhndl;

		shndl = GetValFromNSInt(env, himgsrc);
		dhndl = GetValFromNSInt(env, himgdst);

		DBGSTR(L"Java_NSOCR_Engine_Img_1CopyCurrentPage SHIMG:%d", shndl);
		DBGSTR(L"Java_NSOCR_Engine_Img_1CopyCurrentPage DHIMG:%d", dhndl);

		int result = OCRObject->Img_CopyCurrentPage(shndl, dhndl, Flags);


		DBGSTR(L"Java_NSOCR_Engine_Img_1CopyCurrentPage END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Img_1CopyCurrentPage END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_GetProperty
 * Signature: (LNSOCR/HIMG;I)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1GetProperty(JNIEnv *env, jclass self, jobject himg, jint PropertyID)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1CalcPointPosition begin...");

	if (Loaded && OCRObject)
	{
		HIMG hndl;

		hndl = GetValFromNSInt(env, himg);

		DBGSTR(L"Java_NSOCR_Engine_Img_1CalcPointPosition HIMG:%d", hndl);

		int result = OCRObject->Img_GetProperty(hndl, PropertyID);

		DBGSTR(L"Java_NSOCR_Engine_Img_1CalcPointPosition END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Img_1CalcPointPosition END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_SaveToFile
 * Signature: (LNSOCR/HIMG;Ljava/lang/String;II)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1SaveToFile(JNIEnv *env, jclass self, jobject himg, jstring filename, jint format, jint flags)
{
	int result = ERROR_FIRST;
	DBGSTR(L"Java_NSOCR_Engine_Img_1SaveToFile begin...");

	if (Loaded && OCRObject)
	{
		wchar_t filename_wc[MAXSIZE];
		HCFG hndl;

		hndl = GetValFromNSInt(env, himg);

		if (GetjString(env, filename, filename_wc, sizeof(filename_wc)/sizeof(wchar_t)))
		{
			DBGSTR(L"Java_NSOCR_Engine_Img_1SaveToFile HIMG:%d", hndl);
			DBGSTR(L"Java_NSOCR_Engine_Img_1SaveToFile FILENAME: %ls", filename_wc);

			result = OCRObject->Img_SaveToFile(hndl, filename_wc, format, flags);
		}
		
		DBGSTR(L"Java_NSOCR_Engine_Img_1SaveToFile END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Img_1SaveToFile END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;

}
//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_SaveToMemory
 * Signature: (LNSOCR/HIMG;[BII)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1SaveToMemory(JNIEnv *env, jclass self, jobject himg, jbyteArray mem, jint format, jint flags)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1SaveToMemory begin...");

	if (Loaded && OCRObject)
	{
		HIMG hndl;
	
		hndl = GetValFromNSInt(env, himg);
		DBGSTR(L"Java_NSOCR_Engine_Img_1SaveToMemory HIMG:%d", hndl);

		int SizeInBytes = OCRObject->Img_SaveToMemory(hndl, NULL, 0, format, flags);

		if (SizeInBytes >= ERROR_FIRST || SizeInBytes == 0) 
		{
			DBGSTR(L"Java_NSOCR_Engine_Img_1SaveToMemory get array len END:%d", SizeInBytes);
			return SizeInBytes;
		}

		int Javasize = SizeInBytes;

		if(env->GetArrayLength(mem) <= SizeInBytes)
		{
			return Javasize;
		}

		unsigned char* java_mem = (unsigned char*) malloc(Javasize);

		int result = OCRObject->Img_SaveToMemory( hndl, (char*)java_mem, Javasize, format, flags);

		if (result > ERROR_FIRST)
		{
			goto Img_1SaveToMemoryFinal;
		}

		(*env).SetByteArrayRegion(mem, 0, Javasize, (const jbyte*) java_mem);


Img_1SaveToMemoryFinal:

	free( java_mem);

	DBGSTR(L"Java_NSOCR_Engine_Img_1SaveToMemory END: %d", result);
	return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Img_1SaveToMemory END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_GetType
 * Signature: (LNSOCR/HBLK;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1GetType(JNIEnv *env, jclass self, jobject hblk)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetType begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;

		hndl = GetValFromNSInt(env, hblk);
		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetType HBLK:%d", hndl);

		int result = OCRObject->Blk_GetType(hndl);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetType END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetType END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_SetType
 * Signature: (LNSOCR/HBLK;I)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1SetType(JNIEnv *env, jclass self, jobject hblk, jint BlockType)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1SetType begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;

		hndl = GetValFromNSInt(env, hblk);
		DBGSTR(L"Java_NSOCR_Engine_Blk_1SetType HBLK:%d", hndl);

		int result = OCRObject->Blk_SetType(hndl, BlockType);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1SetType END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1SetType END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_GetRect
 * Signature: (LNSOCR/HBLK;LNSOCR/NSInt;LNSOCR/NSInt;LNSOCR/NSInt;LNSOCR/NSInt;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1GetRect(JNIEnv *env, jclass self, jobject hblk, jobject Xpos, jobject Ypos, jobject Width, jobject Height)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetRectn begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;
		int ix, iy, iwidth, iheight;

		hndl = GetValFromNSInt(env, hblk);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetRect HBLK:%d", hndl);

		int result = OCRObject->Blk_GetRect(hndl, &ix, &iy, &iwidth, &iheight);

		if (result < ERROR_FIRST)
		{
			 DBGSTR(L"Java_NSOCR_Engine_Blk_1GetRect x: %d",    ix);
			 DBGSTR(L"Java_NSOCR_Engine_Blk_1GetRect y: %d",    iy);
			 DBGSTR(L"Java_NSOCR_Engine_Blk_1GetRect w: %d",    iwidth);
			 DBGSTR(L"Java_NSOCR_Engine_Blk_1GetRect h: %d",    iheight);

			 PassValToNSInt(env, Xpos, ix);
			 PassValToNSInt(env, Ypos, iy);
			 PassValToNSInt(env, Width,  iwidth);
			 PassValToNSInt(env, Height, iheight);
		}

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetRect END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetRect END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_GetText
 * Signature: (LNSOCR/HBLK;Ljava/lang/StringBuffer;I)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1GetText(JNIEnv *env, jclass self, jobject hblk, jobject text, jint flags)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetText begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl_hblk;

		hndl_hblk = GetValFromNSInt(env, hblk);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetText HBLK: %d", hndl_hblk);

		int  len = OCRObject->Blk_GetText(hndl_hblk, NULL, 0, flags);

		if (len > ERROR_FIRST)
		{
			DBGSTR(L"Java_NSOCR_Engine_Blk_1GetText String Length END: %d", len);
			return len;
		}

		wchar_t* wtext = (wchar_t*) malloc(sizeof(wchar_t)*(len+1));

		if (!wtext)
		{
			throwOutOfMemoryError(env, (char*)"JNSOCR Internal error: No memory for string");
			DBGSTR(L"Java_NSOCR_Engine_Blk_1GetText String Length OUTOFMEMORY: No memory for string");
			return ERROR_FIRST;
		}

		int result = OCRObject->Blk_GetText(hndl_hblk, wtext, len+1, flags);

		if (result < ERROR_FIRST)
		{
			 //DBGSTR(L"Java_NSOCR_Engine_Blk_1GetText TEXT: %ls", wtext);
			 PassValToStringBuffer(env, text, wtext, len+1);
		}

		free(wtext);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetText END: %d", result);
		return result;
	 }

    DBGSTR(L"Java_NSOCR_Engine_Blk_1GetText END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_GetLineCnt
 * Signature: (LNSOCR/HBLK;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1GetLineCnt(JNIEnv *env, jclass self, jobject hblk)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetLineCnt begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;

		hndl = GetValFromNSInt(env, hblk);
		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetLineCnt HBLK:%d", hndl);

		int result = OCRObject->Blk_GetLineCnt(hndl);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetLineCnt END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetLineCnt END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_GetLineText
 * Signature: (LNSOCR/HBLK;ILjava/lang/StringBuffer;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1GetLineText(JNIEnv *env, jclass self, jobject hblk, jint LineIndex, jobject text)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetLineText begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl_hblk;

		hndl_hblk = GetValFromNSInt(env, hblk);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetLineText HIMG: %d", hndl_hblk);

		int  len = OCRObject->Blk_GetLineText(hndl_hblk, LineIndex, NULL, 0);

		if (len > ERROR_FIRST)
		{
			DBGSTR(L"Java_NSOCR_Engine_Blk_1GetLineText String Length END: %d", len);
			return len;
		}

		wchar_t* wtext = (wchar_t*) malloc(sizeof(wchar_t)*(len+1));

		if (!wtext)
		{
			throwOutOfMemoryError(env, (char*)"JNSOCR Internal error: No memory for string");
			DBGSTR(L"Java_NSOCR_Engine_Blk_1GetLineText String Length OUTOFMEMORY: No memory for string");
			return ERROR_FIRST;
		}

		int result = OCRObject->Blk_GetLineText(hndl_hblk, LineIndex, wtext, len+1);

		if (result < ERROR_FIRST)
		{
			 //DBGSTR(L"Java_NSOCR_Engine_Blk_1GetLineText TEXT: %ls", wtext);
			 PassValToStringBuffer(env, text, wtext, len+1);
		}

		free(wtext);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetLineText END: %d", result);
		return result;
	 }

    DBGSTR(L"Java_NSOCR_Engine_Blk_1GetLineText END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_GetWordCnt
 * Signature: (LNSOCR/HBLK;I)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1GetWordCnt(JNIEnv *env, jclass self, jobject hblk, jint LineIndex)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordCnt begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;

		hndl = GetValFromNSInt(env, hblk);
		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordCnt HBLK:%d", hndl);

		int result = OCRObject->Blk_GetWordCnt(hndl, LineIndex);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordCnt END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordCnt END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_GetWordText
 * Signature: (LNSOCR/HBLK;IILjava/lang/StringBuffer;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1GetWordText(JNIEnv *env, jclass self, jobject hblk, jint LineIndex, jint WordIndex, jobject text)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordText begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl_hblk;

		hndl_hblk = GetValFromNSInt(env, hblk);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordText HIMG: %d", hndl_hblk);

		int  len = OCRObject->Blk_GetWordText(hndl_hblk, LineIndex, WordIndex, NULL, 0);

		if (len > ERROR_FIRST)
		{
			DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordText String Length END: %d", len);
			return len;
		}

		wchar_t* wtext = (wchar_t*) malloc(sizeof(wchar_t)*(len+1));

		if (!wtext)
		{
			throwOutOfMemoryError(env, (char*)"JNSOCR Internal error: No memory for string");
			DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordText String Length OUTOFMEMORY: No memory for string");
			return ERROR_FIRST;
		}

		int result = OCRObject->Blk_GetWordText(hndl_hblk, LineIndex, WordIndex, wtext, len+1);

		if (result < ERROR_FIRST)
		{
			 //DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordText TEXT: %ls", wtext);
			 PassValToStringBuffer(env, text, wtext, len+1);
		}

		free(wtext);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordText END: %d", result);
		return result;
	 }

    DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordText END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_SetWordText
 * Signature: (LNSOCR/HBLK;Ljava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1SetWordText(JNIEnv *env, jclass self, jobject hblk, jint LineIndex, jint WordIndex, jstring text)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1SetWordText begin...");

	int result = ERROR_FIRST;
	DBGSTR(L"Java_NSOCR_Engine_Cfg_1SetOption begin...");

	if (Loaded && OCRObject)
	{
		wchar_t Value_wc[MAXSIZE];
		HBLK hndl_hblk;

		hndl_hblk = GetValFromNSInt(env, hblk);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1SetWordText HIMG: %d", hndl_hblk);

		if (GetjString(env, text, Value_wc, sizeof(Value_wc)/sizeof(wchar_t)))
		{
			result = OCRObject->Blk_SetWordText(hndl_hblk, LineIndex, WordIndex, Value_wc);
		}
		
		DBGSTR(L"Java_NSOCR_Engine_Cfg_1SetOption END: %d", result);
		return result;
	}

    DBGSTR(L"Java_NSOCR_Engine_Blk_1SetWordText END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_GetCharCnt
 * Signature: (LNSOCR/HBLK;II)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1GetCharCnt(JNIEnv *env, jclass self, jobject hblk, jint LineIndex, jint WordIndex)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetCharCnt begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;

		hndl = GetValFromNSInt(env, hblk);
		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetCharCnt HBLK:%d", hndl);

		int result = OCRObject->Blk_GetCharCnt(hndl, LineIndex, WordIndex);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetCharCnt END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetCharCnt END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_GetCharRect
 * Signature: (LNSOCR/HBLK;IIILNSOCR/NSInt;LNSOCR/NSInt;LNSOCR/NSInt;LNSOCR/NSInt;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1GetCharRect(JNIEnv *env, jclass self, jobject hblk, jint LineIndex, jint WordIndex, jint CharIndex, jobject XPos, jobject YPos, jobject Width, jobject Height)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetCharRect begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;
		int ix, iy, iwidth, iheight;

		hndl = GetValFromNSInt(env, hblk);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetCharRect HBLK:%d", hndl);

		int result = OCRObject->Blk_GetCharRect(hndl, LineIndex, WordIndex, CharIndex, &ix, &iy, &iwidth, &iheight);

		if (result < ERROR_FIRST)
		{
			 DBGSTR(L"Java_NSOCR_Engine_Blk_1GetCharRect x: %d",    ix);
			 DBGSTR(L"Java_NSOCR_Engine_Blk_1GetCharRect y: %d",    iy);
			 DBGSTR(L"Java_NSOCR_Engine_Blk_1GetCharRect w: %d",    iwidth);
			 DBGSTR(L"Java_NSOCR_Engine_Blk_1GetCharRect h: %d",    iheight);

			 PassValToNSInt(env, XPos, ix);
			 PassValToNSInt(env, YPos, iy);
			 PassValToNSInt(env, Width,  iwidth);
			 PassValToNSInt(env, Height, iheight);
		}

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetRect END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetRect END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_GetCharText
 * Signature: (LNSOCR/HBLK;IIIILjava/lang/StringBuffer;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1GetCharText(JNIEnv *env, jclass self, jobject hblk, jint LineIndex, jint WordIndex, jint CharIndex, jint ResultIndex, jobject text)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetCharText begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl_hblk;

		hndl_hblk = GetValFromNSInt(env, hblk);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetCharText HIMG: %d", hndl_hblk);

		int  len = OCRObject->Blk_GetCharText(hndl_hblk, LineIndex, WordIndex, CharIndex, ResultIndex,  NULL, 0);

		if (len > ERROR_FIRST)
		{
			DBGSTR(L"Java_NSOCR_Engine_Blk_1GetCharText String Length END: %d", len);
			return len;
		}

		wchar_t* wtext = (wchar_t*) malloc(sizeof(wchar_t)*(len+1));

		if (!wtext)
		{
			throwOutOfMemoryError(env, (char*)"JNSOCR Internal error: No memory for string");
			DBGSTR(L"Java_NSOCR_Engine_Blk_1GetCharText String Length OUTOFMEMORY: No memory for string");
			return ERROR_FIRST;
		}

		int result = OCRObject->Blk_GetCharText(hndl_hblk, LineIndex, WordIndex, CharIndex, ResultIndex, wtext, len+1);

		if (result < ERROR_FIRST)
		{
			 DBGSTR(L"Java_NSOCR_Engine_Blk_1GetCharText TEXT: %ls", wtext);
			 PassValToStringBuffer(env, text, wtext, len+1);
		}

		free(wtext);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetCharText END: %d", result);
		return result;
	 }

    DBGSTR(L"Java_NSOCR_Engine_Blk_1GetCharText END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_GetCharQual
 * Signature: (LNSOCR/HBLK;IIII)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1GetCharQual(JNIEnv *env, jclass self, jobject hblk, jint LineIndex, jint WordIndex, jint CharIndex, jint ResultIndex)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetCharQual begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;

		hndl = GetValFromNSInt(env, hblk);
		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetCharQual HBLK:%d", hndl);

		int result = OCRObject->Blk_GetCharQual(hndl, LineIndex, WordIndex, CharIndex, ResultIndex);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetCharQual END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetCharQual END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_GetTextRect
 * Signature: (LNSOCR/HBLK;IILNSOCR/NSInt;LNSOCR/NSInt;LNSOCR/NSInt;LNSOCR/NSInt;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1GetTextRect(JNIEnv *env, jclass self, jobject hblk, jint LineIndex, jint WordIndex, jobject XPos, jobject YPos, jobject Width, jobject Height)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetTextRect begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;
		int ix, iy, iwidth, iheight;

		hndl = GetValFromNSInt(env, hblk);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetTextRect HBLK:%d", hndl);

		int result = OCRObject->Blk_GetTextRect(hndl, LineIndex, WordIndex, &ix, &iy, &iwidth, &iheight);

		if (result < ERROR_FIRST)
		{
			 DBGSTR(L"Java_NSOCR_Engine_Blk_1GetTextRect x: %d",    ix);
			 DBGSTR(L"Java_NSOCR_Engine_Blk_1GetTextRect y: %d",    iy);
			 DBGSTR(L"Java_NSOCR_Engine_Blk_1GetTextRect w: %d",    iwidth);
			 DBGSTR(L"Java_NSOCR_Engine_Blk_1GetTextRect h: %d",    iheight);

			 PassValToNSInt(env, XPos, ix);
			 PassValToNSInt(env, YPos, iy);
			 PassValToNSInt(env, Width,  iwidth);
			 PassValToNSInt(env, Height, iheight);
		}

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetTextRect END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetTextRect END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_Inversion
 * Signature: (LNSOCR/HBLK;I)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1Inversion(JNIEnv *env, jclass self, jobject hblk, jint Inversion)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1Inversion begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;

		hndl = GetValFromNSInt(env, hblk);
		DBGSTR(L"Java_NSOCR_Engine_Blk_1Inversion HBLK:%d", hndl);

		int result = OCRObject->Blk_Inversion(hndl, Inversion);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1Inversion END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1Inversion END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_Rotation
 * Signature: (LNSOCR/HBLK;I)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1Rotation(JNIEnv *env, jclass self, jobject hblk, jint Rotation)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1Rotation begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;

		hndl = GetValFromNSInt(env, hblk);
		DBGSTR(L"Java_NSOCR_Engine_Blk_1Rotation HBLK:%d", hndl);

		int result = OCRObject->Blk_Rotation(hndl, Rotation);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1Rotation END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1Rotation END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_Mirror
 * Signature: (LNSOCR/HBLK;I)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1Mirror(JNIEnv *env, jclass self, jobject hblk, jint Mirror)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1Mirror begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;

		hndl = GetValFromNSInt(env, hblk);
		DBGSTR(L"Java_NSOCR_Engine_Blk_1Mirror HBLK:%d", hndl);

		int result = OCRObject->Blk_Mirror(hndl, Mirror);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1Mirror END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1Mirror END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_IsWordInDictionary
 * Signature: (LNSOCR/HBLK;II)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1IsWordInDictionary(JNIEnv *env, jclass self, jobject hblk, jint LineIndex, jint WordIndex)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1IsWordInDictionary begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;

		hndl = GetValFromNSInt(env, hblk);
		DBGSTR(L"Java_NSOCR_Engine_Blk_1IsWordInDictionary HBLK:%d", hndl);

		int result = OCRObject->Blk_IsWordInDictionary(hndl, LineIndex, WordIndex);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1IsWordInDictionary END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1IsWordInDictionary END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_SetRect
 * Signature: (LNSOCR/HBLK;IIII)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1SetRect(JNIEnv *env, jclass self, jobject hblk, jint XPos, jint YPos, jint Width, jint Height)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1SetRect begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;

		hndl = GetValFromNSInt(env, hblk);
		DBGSTR(L"Java_NSOCR_Engine_Blk_1SetRect HBLK:%d", hndl);

		int result = OCRObject->Blk_SetRect(hndl, XPos, YPos, Width, Height);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1SetRect END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1SetRect END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_GetWordQual
 * Signature: (LNSOCR/HBLK;II)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1GetWordQual(JNIEnv *env, jclass self, jobject hblk, jint LineIndex, jint WordIndex)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordQual begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;

		hndl = GetValFromNSInt(env, hblk);
		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordQual HBLK:%d", hndl);

		int result = OCRObject->Blk_GetWordQual(hndl, LineIndex, WordIndex);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordQual END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordQual END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_GetWordFontColor
 * Signature: (LNSOCR/HBLK;II)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1GetWordFontColor(JNIEnv *env, jclass self, jobject hblk, jint LineIndex, jint WordIndex)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordFontColor begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;

		hndl = GetValFromNSInt(env, hblk);
		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordFontColor HBLK:%d", hndl);

		int result = OCRObject->Blk_GetWordFontColor(hndl, LineIndex, WordIndex);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordFontColor END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordFontColor END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_GetWordFontSize
 * Signature: (LNSOCR/HBLK;II)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1GetWordFontSize(JNIEnv *env, jclass self, jobject hblk, jint LineIndex, jint WordIndex)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordFontSize begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;

		hndl = GetValFromNSInt(env, hblk);
		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordFontSize HBLK:%d", hndl);

		int result = OCRObject->Blk_GetWordFontSize(hndl, LineIndex, WordIndex);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordFontSize END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordFontSize END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_GetWordFontStyle
 * Signature: (LNSOCR/HBLK;II)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1GetWordFontStyle(JNIEnv *env, jclass self, jobject hblk, jint LineIndex, jint WordIndex)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordFontStyle begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;

		hndl = GetValFromNSInt(env, hblk);
		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordFontStyle HBLK:%d", hndl);

		int result = OCRObject->Blk_GetWordFontStyle(hndl, LineIndex, WordIndex);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordFontStyle END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetWordFontStyle END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_GetBackgroundColor
 * Signature: (LNSOCR/HBLK;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1GetBackgroundColor(JNIEnv *env, jclass self, jobject hblk)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBackgroundColor begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;

		hndl = GetValFromNSInt(env, hblk);
		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBackgroundColor HBLK:%d", hndl);

		int result = OCRObject->Blk_GetBackgroundColor(hndl);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBackgroundColor END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBackgroundColor END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_SetWordRegEx
 * Signature: (LNSOCR/HBLK;IILjava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1SetWordRegEx(JNIEnv *env, jclass self, jobject hblk, jint LineIndex, jint WordIndex, jstring RegEx, jint flags)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1SetWordRegEx begin...");
	int result = ERROR_FIRST;
	if (Loaded && OCRObject)
	{
		HBLK hndl;
		wchar_t wtext[MAXSIZE];

		hndl = GetValFromNSInt(env, hblk);
		DBGSTR(L"Java_NSOCR_Engine_Blk_1SetWordRegEx HBLK:%d", hndl);

		if (GetjString(env, RegEx, wtext, sizeof(wtext)/sizeof(wchar_t)))
		{
			result = OCRObject->Blk_SetWordRegEx(hndl, LineIndex, WordIndex, wtext, flags);
		}

		DBGSTR(L"Java_NSOCR_Engine_Blk_1SetWordRegEx END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1SetWordRegEx END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}
//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_GetBarcodeCnt
 * Signature: (LNSOCR/HBLK;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1GetBarcodeCnt(JNIEnv *env, jclass self, jobject hblk)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBarcodeCnt begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;

		hndl = GetValFromNSInt(env, hblk);
		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBarcodeCnt HBLK:%d", hndl);

		int result = OCRObject->Blk_GetBarcodeCnt(hndl);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBarcodeCnt END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBarcodeCnt END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_GetBarcodeText
 * Signature: (LNSOCR/HBLK;ILjava/lang/StringBuffer;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1GetBarcodeText(JNIEnv *env, jclass self, jobject hblk, jint BarCodeInd, jobject TextStr)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBarcodeText begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;

		hndl = GetValFromNSInt(env, hblk);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBarcodeText HBLK:%d", hndl);

		int  len = OCRObject->Blk_GetBarcodeText(hndl, BarCodeInd, NULL, 0);

		if (len > ERROR_FIRST)
		{
			DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBarcodeText String Length END: %d", len);
			return len;
		}

		wchar_t* wtext = (wchar_t*) malloc(sizeof(wchar_t)*(len+1));

		if (!wtext)
		{
			throwOutOfMemoryError(env, (char*)"JNSOCR Internal error: No memory for string");
			DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBarcodeText String Length OUTOFMEMORY: No memory for string");
			return ERROR_FIRST;
		}

		int result = OCRObject->Blk_GetBarcodeText(hndl, BarCodeInd, wtext, len+1);

		if (result < ERROR_FIRST)
		{
			PassValToStringBuffer(env, TextStr, wtext, len+1);
		}

		free(wtext);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBarcodeText END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBarcodeText END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_GetBarcodeRect
 * Signature: (LNSOCR/HBLK;ILNSOCR/NSInt;LNSOCR/NSInt;LNSOCR/NSInt;LNSOCR/NSInt;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1GetBarcodeRect(JNIEnv *env, jclass self, jobject hblk, jint BarCodeInd, jobject Xpos, jobject Ypos, jobject Width, jobject Height)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBarcodeRect begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;
		int ix, iy, iwidth, iheight;

		hndl = GetValFromNSInt(env, hblk);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBarcodeRect HBLK:%d", hndl);

		int result = OCRObject->Blk_GetBarcodeRect(hndl, BarCodeInd, &ix, &iy, &iwidth, &iheight);

		if (result < ERROR_FIRST)
		{
			 DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBarcodeRect x: %d",    ix);
			 DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBarcodeRect y: %d",    iy);
			 DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBarcodeRect w: %d",    iwidth);
			 DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBarcodeRect h: %d",    iheight);

			 PassValToNSInt(env, Xpos, ix);
			 PassValToNSInt(env, Ypos, iy);
			 PassValToNSInt(env, Width,  iwidth);
			 PassValToNSInt(env, Height, iheight);
		}

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBarcodeRect END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBarcodeRect END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Blk_GetBarcodeType
 * Signature: (LNSOCR/HBLK;I)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Blk_1GetBarcodeType(JNIEnv *env, jclass self, jobject hblk, jint BarCodeInd)
{
	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBarcodeType begin...");

	if (Loaded && OCRObject)
	{
		HBLK hndl;

		hndl = GetValFromNSInt(env, hblk);
		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBarcodeType HBLK:%d", hndl);

		int result = OCRObject->Blk_GetBarcodeType(hndl, BarCodeInd);

		DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBarcodeType END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Blk_1GetBarcodeType END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Svr_Create
 * Signature: (LNSOCR/HCFG;ILNSOCR/HSVR;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Svr_1Create(JNIEnv *env, jclass self, jobject hcfg, jint format, jobject hsvr)
{
	DBGSTR(L"Java_NSOCR_Engine_Svr_1Create begin...");

	if (Loaded && OCRObject)
	{
		HCFG hndl_hcfg;
		HCFG hndl_hsvr;

		hndl_hcfg = GetValFromNSInt(env, hcfg);

		int result = OCRObject->Svr_Create(hndl_hcfg, format, &hndl_hsvr);

		if (result == 0)
		{
			 PassValToNSInt(env, hsvr, hndl_hsvr);
			 DBGSTR(L"Java_NSOCR_Engine_Svr_1Create HSVR: %d", hndl_hsvr);
		}

		DBGSTR(L"Java_NSOCR_Engine_Svr_1Create END: %d", result);
		return result;
	 }

    DBGSTR(L"Java_NSOCR_Engine_Svr_1Create END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Svr_Destroy
 * Signature: (LNSOCR/HSVR;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Svr_1Destroy(JNIEnv *env, jclass self, jobject hsvr)
{
	DBGSTR(L"Java_NSOCR_Engine_Svr_1Destroy begin...");

	if (Loaded && OCRObject)
	{
		HSVR hndl;

		hndl = GetValFromNSInt(env, hsvr);
		DBGSTR(L"Java_NSOCR_Engine_Svr_1Destroy HSVR:%d", hndl);

		int result = OCRObject->Svr_Destroy(hndl);

		DBGSTR(L"Java_NSOCR_Engine_Svr_1Destroy END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Svr_1Destroy END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Svr_NewDocument
 * Signature: (LNSOCR/HSVR;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Svr_1NewDocument(JNIEnv *env, jclass self, jobject hsvr)
{
	DBGSTR(L"Java_NSOCR_Engine_Svr_1NewDocument begin...");

	if (Loaded && OCRObject)
	{
		HSVR hndl;

		hndl = GetValFromNSInt(env, hsvr);
		DBGSTR(L"Java_NSOCR_Engine_Svr_1NewDocument HSVR:%d", hndl);

		int result = OCRObject->Svr_NewDocument(hndl);

		DBGSTR(L"Java_NSOCR_Engine_Svr_1NewDocument END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Svr_1NewDocument END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Svr_AddPage
 * Signature: (LNSOCR/HSVR;LNSOCR/HIMG;I)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Svr_1AddPage(JNIEnv *env, jclass self, jobject hsvr, jobject himg, jint flags)
{
	DBGSTR(L"Java_NSOCR_Engine_Svr_1AddPage begin...");

	if (Loaded && OCRObject)
	{
		HSVR hndl_hsvr;
		HSVR hndl_himg;

		hndl_hsvr = GetValFromNSInt(env, hsvr);
		hndl_himg = GetValFromNSInt(env, himg);

		DBGSTR(L"Java_NSOCR_Engine_Svr_1AddPage HSVR:%d", hndl_hsvr);
		DBGSTR(L"Java_NSOCR_Engine_Svr_1AddPage HIMG:%d", hndl_himg);

		int result = OCRObject->Svr_AddPage(hndl_hsvr, hndl_himg, flags);

		DBGSTR(L"Java_NSOCR_Engine_Svr_1AddPage END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Svr_1AddPage END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Svr_SaveToFile
 * Signature: (LNSOCR/HSVR;Ljava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Svr_1SaveToFile(JNIEnv *env, jclass self, jobject hsvr, jstring filename)
{
	int result = ERROR_FIRST;
	DBGSTR(L"Java_NSOCR_Engine_Svr_1SaveToFile begin...");

	if (Loaded && OCRObject)
	{
		wchar_t filename_wc[MAXSIZE];
		HSVR hndl;

		hndl = GetValFromNSInt(env, hsvr);

		if (GetjString(env, filename, filename_wc, sizeof(filename_wc)/sizeof(wchar_t)))
		{
			DBGSTR(L"Java_NSOCR_Engine_Svr_1SaveToFile HSVR:%d", hndl);
			DBGSTR(L"Java_NSOCR_Engine_Svr_1SaveToFile FILENAME: %ls", filename_wc);

			result = OCRObject->Svr_SaveToFile(hndl, filename_wc);
		}
		
		DBGSTR(L"Java_NSOCR_Engine_Svr_1SaveToFile END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Svr_1SaveToFile END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}
//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Svr_SaveToMemory
 * Signature: (LNSOCR/HSVR;[I)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Svr_1SaveToMemory(JNIEnv *env, jclass self, jobject hsvr, jbyteArray mem)
{
	DBGSTR(L"Java_NSOCR_Engine_Svr_1SaveToMemory begin...");

	if (Loaded && OCRObject)
	{
		HSVR hndl;
	
		hndl = GetValFromNSInt(env, hsvr);
		DBGSTR(L"Java_NSOCR_Engine_Svr_1SaveToMemory HSVR:%d", hndl);

		int SizeInBytes = OCRObject->Svr_SaveToMemory(hndl, NULL, 0);

		if (SizeInBytes >= ERROR_FIRST || SizeInBytes == 0) 
		{
			DBGSTR(L"Java_NSOCR_Engine_Svr_1SaveToMemory get array len END:%d", SizeInBytes);
			return SizeInBytes;
		}

		int Javasize = SizeInBytes;

		if(env->GetArrayLength(mem) <= SizeInBytes)
		{
			return Javasize;
		}

		unsigned char* java_mem = (unsigned char*) malloc(Javasize);

		int result = OCRObject->Svr_SaveToMemory( hndl, (char*)java_mem, Javasize);

		if (result > ERROR_FIRST)
		{
			goto Svr_1SaveToMemoryFinal;
		}

		(*env).SetByteArrayRegion(mem, 0, Javasize, (const jbyte*) java_mem);


Svr_1SaveToMemoryFinal:

	free( java_mem);

	DBGSTR(L"Java_NSOCR_Engine_Svr_1SaveToMemory END: %d", result);
	return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Svr_1SaveToMemorya END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Svr_GetText
 * Signature: (LNSOCR/HSVR;ILjava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Svr_1GetText(JNIEnv *env, jclass self, jobject hsvr, jint PageIndex, jobject text)
{
	DBGSTR(L"Java_NSOCR_Engine_Svr_1GetText begin...");

	if (Loaded && OCRObject)
	{
		HSVR hndl_hsvr;

		hndl_hsvr = GetValFromNSInt(env, hsvr);

		DBGSTR(L"Java_NSOCR_Engine_Svr_1GetText HSVR: %d", hndl_hsvr);

		int  len = OCRObject->Svr_GetText(hndl_hsvr, PageIndex, NULL, 0);

		if (len > ERROR_FIRST)
		{
			DBGSTR(L"Java_NSOCR_Engine_Svr_1GetText String Length END: %d", len);
			return len;
		}

		wchar_t* wtext = (wchar_t*) malloc(sizeof(wchar_t)*(len+1));

		if (!wtext)
		{
			throwOutOfMemoryError(env, (char*)"JNSOCR Internal error: No memory for string");
			DBGSTR(L"Java_NSOCR_Engine_Svr_1GetText String Length OUTOFMEMORY: No memory for string");
			return ERROR_FIRST;
		}

		int result = OCRObject->Svr_GetText(hndl_hsvr, PageIndex, wtext, len+1);

		if (result < ERROR_FIRST)
		{
			 //DBGSTR(L"Java_NSOCR_Engine_Svr_1GetText TEXT: %ls", wtext);
			 PassValToStringBuffer(env, text, wtext, len+1);
		}

		free(wtext);

		DBGSTR(L"Java_NSOCR_Engine_Svr_1GetText END: %d", result);
		return result;
	 }

    DBGSTR(L"Java_NSOCR_Engine_Svr_1GetText END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Scan_Create
 * Signature: (LNSOCR/HCFG;LNSOCR/HSCAN;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Scan_1Create(JNIEnv *env, jclass self, jobject hcfg, jobject hscan)
{
	DBGSTR(L"Java_NSOCR_Engine_Scan_1Create begin...");

	if (Loaded && OCRObject)
	{
		HCFG  hndl_hcfg;
		HSCAN hndl_hscan;

		hndl_hcfg = GetValFromNSInt(env, hcfg);
		DBGSTR(L"Java_NSOCR_Engine_Scan_1Create HCFG: %d", hndl_hcfg);

		int result = OCRObject->Scan_Create(hndl_hcfg, &hndl_hscan);

		if (result == 0)
		{
			 DBGSTR(L"Java_NSOCR_Engine_Scan_1Create HSCAN: %d", hndl_hscan);
			 PassValToNSInt(env, hscan, hndl_hscan);
		}

		DBGSTR(L"Java_NSOCR_Engine_Scan_1Create END: %d", result);
		return result;
	 }

    DBGSTR(L"Java_NSOCR_Engine_Scan_1Create END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Scan_Enumerate
 * Signature: (LNSOCR/HSCAN;Ljava/lang/String;II)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Scan_1Enumerate(JNIEnv *env, jclass self, jobject hscan, jobject ScannerNames, jint flags)
{
	DBGSTR(L"Java_NSOCR_Engine_Scan_1Enumerate begin...");

	if (Loaded && OCRObject)
	{
		HSCAN hndl_hscan;

		hndl_hscan = GetValFromNSInt(env, hscan);

		DBGSTR(L"Java_NSOCR_Engine_Scan_1Enumerate HSCAN: %d", hndl_hscan);

		int  len = OCRObject->Scan_Enumerate(hndl_hscan, NULL, 0, flags);

		if (len > ERROR_FIRST)
		{
			DBGSTR(L"Java_NSOCR_Engine_Scan_1Enumerate String Length END: %d", len);
			return len;
		}

		wchar_t* wtext = (wchar_t*) malloc(sizeof(wchar_t)*(len+1));

		if (!wtext)
		{
			throwOutOfMemoryError(env, (char*)"JNSOCR Internal error: No memory for string");
			DBGSTR(L"Java_NSOCR_Engine_Scan_1Enumerate String Length OUTOFMEMORY: No memory for string");
			return ERROR_FIRST;
		}

		int result = OCRObject->Scan_Enumerate(hndl_hscan, wtext, len+1, flags);

		if (result < ERROR_FIRST)
		{
			 DBGSTR(L"Java_NSOCR_Engine_Scan_1Enumerate TEXT: %ls", wtext);
			 if (ScannerNames) PassValToStringBuffer(env, ScannerNames, wtext, len+1);
		}

		free(wtext);

		DBGSTR(L"Java_NSOCR_Engine_Scan_1Enumerate END: %d", result);
		return result;
	 }

    DBGSTR(L"Java_NSOCR_Engine_Scan_1Enumerate END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Scan_ScanToImg
 * Signature: (LNSOCR/HSCAN;LNSOCR/HIMG;II)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Scan_1ScanToImg(JNIEnv *env, jclass self, jobject hscan, jobject himg, jint ScannerIndex , jint flags)
{
	DBGSTR(L"Java_NSOCR_Engine_Scan_1ScanToImg begin...");

	if (Loaded && OCRObject)
	{
		HIMG  hndl_himg;
		HSCAN hndl_hscan;

		hndl_himg  = GetValFromNSInt(env, himg);
		hndl_hscan = GetValFromNSInt(env, hscan);

		DBGSTR(L"Java_NSOCR_Engine_Scan_1ScanToImg HSCAN: %d", hndl_hscan);
		DBGSTR(L"Java_NSOCR_Engine_Scan_1ScanToImg HIMG: %d", hndl_hscan);

		int result = OCRObject->Scan_ScanToImg(hndl_hscan, hndl_himg, ScannerIndex, flags);

		DBGSTR(L"Java_NSOCR_Engine_Scan_1ScanToImg END: %d", result);
		return result;
	 }

    DBGSTR(L"Java_NSOCR_Engine_Scan_1ScanToImg END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Scan_ScanToFile
 * Signature: (LNSOCR/HSCAN;Ljava/lang/String;II)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Scan_1ScanToFile(JNIEnv *env, jclass self, jobject hscan, jstring filename, jint ScannerIndex, jint flags)
{
	int result = ERROR_FIRST;
	DBGSTR(L"Java_NSOCR_Engine_Scan_1ScanToFile begin...");

	if (Loaded && OCRObject)
	{
		wchar_t filename_wc[MAXSIZE];
		HSCAN hndl;

		hndl = GetValFromNSInt(env, hscan);

		if (GetjString(env, filename, filename_wc, sizeof(filename_wc)/sizeof(wchar_t)))
		{
			DBGSTR(L"Java_NSOCR_Engine_Scan_1ScanToFile HSCAN:%d", hndl);
			DBGSTR(L"Java_NSOCR_Engine_Scan_1ScanToFile FILENAME: %ls", filename_wc);

			result = OCRObject->Scan_ScanToFile(hndl, filename_wc, ScannerIndex, flags);
		}
		
		DBGSTR(L"Java_NSOCR_Engine_Scan_1ScanToFile END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Scan_1ScanToFile END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Scan_Destroy
 * Signature: (LNSOCR/HSCAN;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Scan_1Destroy(JNIEnv *env, jclass self, jobject hscan)
{
	DBGSTR(L"Java_NSOCR_Engine_Scan_1Destroy begin...");

	if (Loaded && OCRObject)
	{
		HSCAN hndl_hscan;

		hndl_hscan = GetValFromNSInt(env, hscan);
		DBGSTR(L"Java_NSOCR_Engine_Scan_1Destroy HSCAN: %d", hndl_hscan);

		int result = OCRObject->Scan_Destroy(hndl_hscan);

		DBGSTR(L"Java_NSOCR_Engine_Scan_1Destroy END: %d", result);
		return result;
	 }

    DBGSTR(L"Java_NSOCR_Engine_Scan_1Destroy END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_LoadFromMemory
 * Signature: (LNSOCR/HIMG;[BI)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1LoadFromMemory(JNIEnv *env, jclass self, jobject himg, jbyteArray ByteArray, jint flags)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1LoadFromMemory begin...");

	if (Loaded && OCRObject)
	{
		HIMG   hndl_himg;
		jbyte *arr;

		hndl_himg = GetValFromNSInt(env, himg);
		DBGSTR(L"Java_NSOCR_Engine_Img_1LoadFromMemory HIMG: %d", hndl_himg);

        arr = (*env).GetByteArrayElements(ByteArray, 0);
		int jsize = (*env).GetArrayLength(ByteArray);
      
		int result = OCRObject->Img_LoadFromMemory(hndl_himg, (unsigned char*) arr, jsize);

        (*env).ReleaseByteArrayElements(ByteArray, arr, 0);

		DBGSTR(L"Java_NSOCR_Engine_Img_1LoadFromMemory END: %d", result);
		return result;
	 }

    DBGSTR(L"Java_NSOCR_Engine_Img_1LoadFromMemory END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_LoadBmpData
 * Signature: (LNSOCR/HIMG;[IIII)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1LoadBmpData(JNIEnv *env, jclass self, jobject himg, jintArray IntArray, jint Width, jint Height, jint flags)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1LoadBmpData begin...");

	if ((flags & BMP_32BIT) == 0) //only BMP_32BIT is supported
	{
		DBGSTR(L"Java_NSOCR_Engine_Img_1LoadBmpData END: ERROR_INVALIDPARAMETER");
		return ERROR_INVALIDPARAMETER;
	}

	if (Loaded && OCRObject)
	{
		HIMG hndl;
		
		hndl = GetValFromNSInt(env, himg);
		DBGSTR(L"Java_NSOCR_Engine_Img_1LoadBmpData HIMG:%d", hndl);

		// Любая картинка которая пришла из java 32-битная ARGB
        jint *arr = (*env).GetIntArrayElements(IntArray, 0);
		int jsize = (*env).GetArrayLength(IntArray);
		if (jsize < Width * Height)
		{
			(*env).ReleaseIntArrayElements(IntArray, arr, 0);
			DBGSTR(L"Java_NSOCR_Engine_Img_1LoadBmpData END: ERROR_INVALIDPARAMETER");
			return ERROR_INVALIDPARAMETER;
		}

		int result = OCRObject->Img_LoadBmpData(hndl, (unsigned char*) arr, Width, Height, flags, 0);
		(*env).ReleaseIntArrayElements(IntArray, arr, 0);

		DBGSTR(L"Java_NSOCR_Engine_Img_1LoadBmpData END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Img_1LoadBmpData END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Img_GetBmpData
 * Signature: (LNSOCR/HIMG;[ILNSOCR/NSInt;LNSOCR/NSInt;I)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Img_1GetBmpData(JNIEnv *env, jclass self, jobject himg, jintArray IntArray, jobject Width, jobject Height, jint flags)
{
	DBGSTR(L"Java_NSOCR_Engine_Img_1GetBmpData begin...");

	if (Loaded && OCRObject)
	{
		HIMG hndl;
		int  iwidth;
		int  iheight;
		
		hndl = GetValFromNSInt(env, himg);
		DBGSTR(L"Java_NSOCR_Engine_Img_1GetBmpData HIMG:%d", hndl);

		iwidth  = GetValFromNSInt(env, Width);
		iheight = GetValFromNSInt(env, Height);

		DBGSTR(L"Java_NSOCR_Engine_Img_1GetBmpData inp W: %d", iwidth);
		DBGSTR(L"Java_NSOCR_Engine_Img_1GetBmpData inp H: %d", iheight);

		int SizeInBytes = OCRObject->Img_GetBmpData(hndl, NULL, &iwidth, &iheight, flags);

		if (SizeInBytes >= ERROR_FIRST || SizeInBytes == 0) 
		{
			PassValToNSInt(env, Width,  0);
			PassValToNSInt(env, Height, 0);

			DBGSTR(L"Java_NSOCR_Engine_Img_1GetBmpData get array len END:%d", SizeInBytes);
			return SizeInBytes;
		}

		int Javasize = iwidth*iheight*4;

		if(env->GetArrayLength(IntArray) == 0)
		{
			PassValToNSInt(env, Width,  iwidth);
			PassValToNSInt(env, Height, iheight);
			return Javasize;
		}

		// BufferedImage.SetRGB может работать только с 4-х байтными пикселями ARGB.
		// Выделям память дважды! Первый раз для RAW представления второй для JAVA ARGB
		int bpp = OCRObject->Img_GetBmpData(hndl, NULL, &iwidth, &iheight, flags | DRAW_GETBPP);


		unsigned char* raw_bmp = (unsigned char*) malloc(SizeInBytes);
		unsigned char* java_bmp = (unsigned char*) malloc(Javasize);

		int result = OCRObject->Img_GetBmpData(hndl, raw_bmp, &iwidth, &iheight, flags);

		if (result > ERROR_FIRST)
		{
			PassValToNSInt(env, Width,  0);
			PassValToNSInt(env, Height, 0);

			goto GetBmpDataFinal;
		}
		else
		{
			PassValToNSInt(env, Width,  iwidth);
			PassValToNSInt(env, Height, iheight);

			// Серое изображение. C Выравниванием!!!.
			int n = 0;
			int m = 0;

			if (bpp == 8)
			{
				DBGSTR(L"Java_NSOCR_Engine_Img_1GetBmpData BPP:8");
				for (int y = 0; y<iheight; y++)
				{
					for (int x = 0; x<iwidth; x++)
					{
						int Pixel;
						Pixel = (0xFF000000) | (int)raw_bmp[n] | ((int)raw_bmp[n] << 8) | ((int)raw_bmp[n] << 16);
						((int*)java_bmp)[m] = Pixel;
						n++; m++;
					}
					
					if (((iwidth) % 4))
					  n += 4 - ((iwidth) % 4);
				}
			}
			// Цветное изображение 
			else if (bpp == 24)
			{
				DBGSTR(L"Java_NSOCR_Engine_Img_1GetBmpData BPP:24");
				for (int y = 0; y<iheight; y++)
				{
					for (int x = 0; x<iwidth; x++)
					{
						int Pixel;
						Pixel  = (0xFF000000) | (((int)raw_bmp[n++]));
						Pixel |= (((int)raw_bmp[n++]) << 8); 
						Pixel |= ((int)raw_bmp[n++] << 16);

						((int*)java_bmp)[m] = Pixel;
						m++;
					}

					if (((iwidth * 3) % 4))
					  n += 4 - ((iwidth * 3) % 4);
				}
			}
			else 
			{
				result = ERROR_FIRST;
				goto GetBmpDataFinal;
			}

			(*env).SetIntArrayRegion(IntArray, 0, m, (const jint*) java_bmp);

			DBGSTR(L"Java_NSOCR_Engine_Img_1GetBmpData JElements: %d", m);
			DBGSTR(L"Java_NSOCR_Engine_Img_1GetBmpData W: %d", iwidth);
			DBGSTR(L"Java_NSOCR_Engine_Img_1GetBmpData H: %d", iheight);

		}

GetBmpDataFinal:

		free(raw_bmp);
		free(java_bmp);

		DBGSTR(L"Java_NSOCR_Engine_Img_1GetBmpData END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Img_1GetBmpData END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}

//-----------------------------------------------------------------------------
/*
 * Class:     NSOCR_Engine
 * Method:    Svr_SetDocumentInfo
 * Signature: (LNSOCR/HSVR;ILjava/lang/String;)I
 */
JNIEXPORT jint JNICALL Java_NSOCR_Engine_Svr_1SetDocumentInfo(JNIEnv *env, jclass self, jobject hsvr, jint infoID, jstring InfoString)
{
	int result = ERROR_FIRST;
	DBGSTR(L"Java_NSOCR_Engine_Svr_1SetDocumentInfo begin...");

	if (Loaded && OCRObject)
	{
		wchar_t InfoString_wc[MAXSIZE];
		HSVR hndl;

		hndl = GetValFromNSInt(env, hsvr);

		if (GetjString(env, InfoString, InfoString_wc, sizeof(InfoString_wc)/sizeof(wchar_t)))
		{
			DBGSTR(L"Java_NSOCR_Engine_Svr_1SetDocumentInfo HSVR:%d", hndl);
			DBGSTR(L"Java_NSOCR_Engine_Svr_1SetDocumentInfo InfoId:%d", infoID);
			DBGSTR(L"Java_NSOCR_Engine_Svr_1SetDocumentInfo InfoString: %ls", InfoString_wc);

			result = OCRObject->Svr_SetDocumentInfo(hndl, infoID, InfoString_wc);
		}
		
		DBGSTR(L"Java_NSOCR_Engine_Svr_1SetDocumentInfo END: %d", result);
		return result;
	}

	DBGSTR(L"Java_NSOCR_Engine_Svr_1SetDocumentInfo END: ERROR_DLLNOTLOADED");
	return ERROR_DLLNOTLOADED;
}
