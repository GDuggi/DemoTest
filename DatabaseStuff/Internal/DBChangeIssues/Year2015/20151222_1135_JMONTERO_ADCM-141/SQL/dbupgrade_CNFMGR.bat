@echo off
REM Test parameters
IF %1NOTHING==NOTHING GOTO WARNING
IF %2NOTHING==NOTHING GOTO WARNING
IF %3NOTHING==NOTHING GOTO WARNING
IF %4NOTHING==NOTHING GOTO WARNING
REM
set ISQL="sqlcmd.exe"
set SQLCMDLOGINTIMEOUT=0
echo Validating Logs Folder
REM
cd logs
if errorlevel 1 goto dirnotexists
if exist *.log del *.log
cd ..
goto start
:dirnotexists
echo So, the folder 'logs' doesn't exist...create it now
mkdir logs

:start
REM
echo Upgrading CONFIRMATION MANAGER database (MS SQL Server) for issue #ADCM-141 ...
REM ********************************************************************************
echo ********************************************************************************
echo Upgrading TRADE_DATA Table(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i UUU_TRADE_DATA\ALTERS_TRADE_DATA.sql -o logs\ALTERS_TRADE_DATA.log
echo Upgrading TRADE_TRADE_DATA_JN Table(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i UUU_TRADE_DATA_JN\ALTERS_TRADE_DATA_JN.sql -o logs\ALTERS_TRADE_DATA_JN.log
REM ********************************************************************************
echo ******************************************************************************** 
echo Done
goto end
:WARNING
Echo Usage: dbupgrade_CNFMGR [server] [login] [password] [database]
:end
