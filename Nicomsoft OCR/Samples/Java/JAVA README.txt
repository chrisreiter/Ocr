Instructions for "sample.jar" Java Sample project:

1. Install Java SE 6 or later from official Oracle website for your platform (Windows or Linux debian family).
2. For Java virtual machine specify directory that contains JNI library jnsocr.dll (libjnsocr.so for Linux), use 
-Djava.library.path for that. By default it is located in "Bin" directory fo x86 platform and "Bin_64" for x64 platform.
3. If you move JNI library jnsocr.dll (libjnsocr.so for Linux) to some different folder, you must set path to NSOCR binaries in "JNSOCR" environment variable.
Note: To start/debug Java Sample in IDE (NetBeans, Eclipse, etc) you must set "Djava.library.path" variable in IDE settings too.

Check "startjar.bat" and "startjar.bash" files for sample configuration.

READ MORE: http://www.nicomsoft.com/category/article/ocr/

NOTE FOR TRIAL VERSION FOR WINDOWS: Trial message appears BEHIND main Java sample window. You must close it before you can continue test Java sample.
