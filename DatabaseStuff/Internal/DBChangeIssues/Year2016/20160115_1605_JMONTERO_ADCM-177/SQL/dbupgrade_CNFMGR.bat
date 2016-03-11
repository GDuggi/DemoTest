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
echo Upgrading CONFIRMATION MANAGER database (MS SQL Server) for issue #ADCM-177 ...
REM ********************************************************************************
echo ********************************************************************************
echo Altering TG_TRADERQMT_AS_IU Procedure(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i UUU_TG_TRADERQMT_AS_IU\TG_TRADERQMT_AS_IU.sql -o logs\TG_TRADERQMT_AS_IU.log
REM ********************************************************************************
echo ******************************************************************************** 
echo Updating DB version ........
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i dbversion.sql
echo Done
goto end
:WARNING
Echo Usage: dbupgrade_CNFMGR [server] [login] [password] [database]
:end
