@ECHO OFF
echo Uninstalling cnf-InboundFileProcessorService... 
echo --------------------------------------------------- 
d:
cd D:\Homeware\cnf-InboundFileProcessorService
echo %WINDIR%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /u cnf-InboundFileProcessorService.exe
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /u cnf-InboundFileProcessorService.exe
echo ---------------------------------------------------
echo Done.
pause