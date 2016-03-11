@ECHO OFF
echo Installing cnf-InboundFileProcessorService.exe... 
echo --------------------------------------------------- 
d:
cd D:\Homeware\cnf-InboundFileProcessorService
echo %WINDIR%\Microsoft.NET\Framework\v4.0.30319\installutil.exe cnf-InboundFileProcessorService.exe
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\installutil.exe cnf-InboundFileProcessorService.exe
echo ---------------------------------------------------
echo Done.
pause 