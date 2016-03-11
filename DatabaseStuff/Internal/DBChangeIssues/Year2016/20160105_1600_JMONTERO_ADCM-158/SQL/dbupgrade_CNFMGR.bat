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
echo Upgrading CONFIRMATION MANAGER database (MS SQL Server) for issue #ADCM-158 ...
REM ********************************************************************************
echo ********************************************************************************
echo Creating DATABASE_INFO Table(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i AAA_DATABASE_INFO\AAA_DATABASE_INFO.sql -o logs\AAA_DATABASE_INFO.log
echo Creating DBPGRADE_LOG Table(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i AAA_DBUPGRADE_LOG\UUU_ASSOCIATED_DOCS_JN.sql -o logs\UUU_ASSOCIATED_DOCS_JN.log
echo Creating Triggers on DATABASE_INFO Table(s)
echo Creating Trigger TG_DATABASE_INFO_ITRG on DATABASE_INFO Table(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i AAA_DATABASE_INFO\AAA_TG_DATABASE_INFO_ITRG.sql -o logs\AAA_TG_DATABASE_INFO_ITRG.log
echo Creating Trigger TG_DATABASE_INFO_UTRG on DATABASE_INFO Table(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i AAA_DATABASE_INFO\AAA_TG_DATABASE_INFO_UTRG.sql -o logs\AAA_TG_DATABASE_INFO_UTRG.log

REM ********************************************************************************
echo ******************************************************************************** 
echo Done
goto end
:WARNING
Echo Usage: dbupgrade_CNFMGR [server] [login] [password] [database]
:end
