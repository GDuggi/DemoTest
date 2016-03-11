@ECHO OFF
echo Installing Vault Uploader Service
echo --------------------------------------------------- 
echo running command -- %WINDIR%\Microsoft.NET\Framework\v4.0.30319\installutil.exe %~dp0\..\VaultUploader.Service.exe
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\installutil.exe %~dp0\..\VaultUploader.Service.exe
echo ---------------------------------------------------
echo Completed.
pause 