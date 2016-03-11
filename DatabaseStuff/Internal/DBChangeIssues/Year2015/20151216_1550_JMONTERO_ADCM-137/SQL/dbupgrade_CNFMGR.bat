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
echo Starting the Process....
REM
echo Upgrading CNF_MGR database (MS SQL Server) for issue #ADCM-137 ...
REM ********************************************************************************
echo ********************************************************************************
REM ********************************************************************************
echo Upgrading Views(s) V_PC_TRADE_RQMT...
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i UUU_TRADE_RQMT\V_PC_TRADE_RQMT.sql -o logs\V_PC_TRADE_RQMT.log
echo Upgrading Views(s) V_ACTIVE_ASSOCIATED_DOCS...
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i UUU_ASSOCIATED_DOCS\V_ACTIVE_ASSOCIATED_DOCS.sql -o logs\V_ACTIVE_ASSOCIATED_DOCS.log
echo Done
echo ********************************************************************************
echo NOTE:
echo Please validated all is working Fine.
echo Have a great day!!!!
echo ********************************************************************************
goto end
:WARNING
Echo Usage: dbupgrade_TRADE [server] [login] [password] [database]
:end
