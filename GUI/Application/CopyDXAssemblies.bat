@echo off
setlocal
set DEST=%~dp0Build\DevExpress
set GAC=C:\Windows\Microsoft.NET\assembly\GAC_MSIL\


if not exist "%DEST%" mkdir ""%DEST%""
echo.
echo Copying DevExpress assemblies to %DEST%
 
set FILES=BonusSkins Data Utils XtraBars XtraEditors XtraPivotGrid XtraTreeList XtraGrid
set CORE_FILES=PivotGrid
set VERSION=v14.2
set SRC=C:\Program Files (x86)\DevExpress 14.2\Components\Bin\Framework

for %%f in (%FILES%) do (
	copy /y "%SRC%\DevExpress.%%f.%VERSION%.dll" "%DEST%"
)
for %%f in (%CORE_FILES%) do (
	copy /y "%SRC%\DevExpress.%%f.%VERSION%.core.dll" "%DEST%"
)
echo.
pause Press [CR] to continue
