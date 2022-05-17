#
# Generated Makefile - do not edit!
#
# Edit the Makefile in the project folder instead (../Makefile). Each target
# has a -pre and a -post target defined where you can add customized code.
#
# This makefile implements configuration specific macros and targets.


# Environment
MKDIR=mkdir
CP=cp
GREP=grep
NM=nm
CCADMIN=CCadmin
RANLIB=ranlib
CC=gcc
CCC=g++
CXX=g++
FC=gfortran
AS=as

# Macros
CND_PLATFORM=GNU-Linux-x86
CND_DLIB_EXT=so
CND_CONF=Release_64
CND_DISTDIR=dist
CND_BUILDDIR=build

# Include project Makefile
include Makefile

# Object Directory
OBJECTDIR=${CND_BUILDDIR}/${CND_CONF}/${CND_PLATFORM}

# Object Files
OBJECTFILES= \
	${OBJECTDIR}/_ext/2124829536/NSOCR.o \
	${OBJECTDIR}/_ext/1472/JNSOCR.o \
	${OBJECTDIR}/_ext/1472/OcrConvert.o \
	${OBJECTDIR}/_ext/1193346143/pugixml.o \
	${OBJECTDIR}/_ext/1472/debug.o \
	${OBJECTDIR}/_ext/1472/dllmain.o


# C Compiler Flags
CFLAGS=-m64

# CC Compiler Flags
CCFLAGS=-m64 -fpermissive
CXXFLAGS=-m64 -fpermissive

# Fortran Compiler Flags
FFLAGS=

# Assembler Flags
ASFLAGS=

# Link Libraries and Options
LDLIBSOPTIONS=-lpthread -ldl

# Build Targets
.build-conf: ${BUILD_SUBPROJECTS}
	"${MAKE}"  -f nbproject/Makefile-${CND_CONF}.mk ${CND_DISTDIR}/${CND_CONF}/${CND_PLATFORM}/libJNSOCR.${CND_DLIB_EXT}/libjnsocr.so

${CND_DISTDIR}/${CND_CONF}/${CND_PLATFORM}/libJNSOCR.${CND_DLIB_EXT}/libjnsocr.so: ${OBJECTFILES}
	${MKDIR} -p ${CND_DISTDIR}/${CND_CONF}/${CND_PLATFORM}/libJNSOCR.${CND_DLIB_EXT}
	${LINK.cc} -o ${CND_DISTDIR}/${CND_CONF}/${CND_PLATFORM}/libJNSOCR.${CND_DLIB_EXT}/libjnsocr.so ${OBJECTFILES} ${LDLIBSOPTIONS} -Wl,--wrap=memcpy -shared -fPIC

${OBJECTDIR}/_ext/2124829536/NSOCR.o: ../../../NSOCR.cpp 
	${MKDIR} -p ${OBJECTDIR}/_ext/2124829536
	${RM} "$@.d"
	$(COMPILE.cc) -O2 -I/usr/java/jdk1.7.0_79/include/linux -I/usr/java/jdk1.7.0_79/include -I../../.. -I../../../../Include -fPIC  -MMD -MP -MF "$@.d" -o ${OBJECTDIR}/_ext/2124829536/NSOCR.o ../../../NSOCR.cpp

${OBJECTDIR}/_ext/1472/JNSOCR.o: ../JNSOCR.cpp 
	${MKDIR} -p ${OBJECTDIR}/_ext/1472
	${RM} "$@.d"
	$(COMPILE.cc) -O2 -I/usr/java/jdk1.7.0_79/include/linux -I/usr/java/jdk1.7.0_79/include -I../../.. -I../../../../Include -fPIC  -MMD -MP -MF "$@.d" -o ${OBJECTDIR}/_ext/1472/JNSOCR.o ../JNSOCR.cpp

${OBJECTDIR}/_ext/1472/OcrConvert.o: ../OcrConvert.cpp 
	${MKDIR} -p ${OBJECTDIR}/_ext/1472
	${RM} "$@.d"
	$(COMPILE.cc) -O2 -I/usr/java/jdk1.7.0_79/include/linux -I/usr/java/jdk1.7.0_79/include -I../../.. -I../../../../Include -fPIC  -MMD -MP -MF "$@.d" -o ${OBJECTDIR}/_ext/1472/OcrConvert.o ../OcrConvert.cpp

${OBJECTDIR}/_ext/1193346143/pugixml.o: ../PUGIXML/pugixml.cpp 
	${MKDIR} -p ${OBJECTDIR}/_ext/1193346143
	${RM} "$@.d"
	$(COMPILE.cc) -O2 -I/usr/java/jdk1.7.0_79/include/linux -I/usr/java/jdk1.7.0_79/include -I../../.. -I../../../../Include -fPIC  -MMD -MP -MF "$@.d" -o ${OBJECTDIR}/_ext/1193346143/pugixml.o ../PUGIXML/pugixml.cpp

${OBJECTDIR}/_ext/1472/debug.o: ../debug.cpp 
	${MKDIR} -p ${OBJECTDIR}/_ext/1472
	${RM} "$@.d"
	$(COMPILE.cc) -O2 -I/usr/java/jdk1.7.0_79/include/linux -I/usr/java/jdk1.7.0_79/include -I../../.. -I../../../../Include -fPIC  -MMD -MP -MF "$@.d" -o ${OBJECTDIR}/_ext/1472/debug.o ../debug.cpp

${OBJECTDIR}/_ext/1472/dllmain.o: ../dllmain.cpp 
	${MKDIR} -p ${OBJECTDIR}/_ext/1472
	${RM} "$@.d"
	$(COMPILE.cc) -O2 -I/usr/java/jdk1.7.0_79/include/linux -I/usr/java/jdk1.7.0_79/include -I../../.. -I../../../../Include -fPIC  -MMD -MP -MF "$@.d" -o ${OBJECTDIR}/_ext/1472/dllmain.o ../dllmain.cpp

# Subprojects
.build-subprojects:

# Clean Targets
.clean-conf: ${CLEAN_SUBPROJECTS}
	${RM} -r ${CND_BUILDDIR}/${CND_CONF}
	${RM} ${CND_DISTDIR}/${CND_CONF}/${CND_PLATFORM}/libJNSOCR.${CND_DLIB_EXT}/libjnsocr.so

# Subprojects
.clean-subprojects:

# Enable dependency checking
.dep.inc: .depcheck-impl

include .dep.inc
