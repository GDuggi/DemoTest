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
echo Upgrading CONFIRMATION MANAGER database (MS SQL Server) for issue #ADCM-138 ...
REM ********************************************************************************
echo ********************************************************************************
echo Upgrading TRADE_SUMMARY Table(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i UUU_TRADE_SUMMARY\ALTERS_TRADE_SUMMARY.sql -o logs\ALTERS_TRADE_SUMMARY.log
echo Upgrading TRADE_SUMMARY_JN Table(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i UUU_TRADE_SUMMARY_JN\ALTERS_TRADE_SUMMARY_JN.sql -o logs\ALTERS_TRADE_SUMMARY_JN.log
echo Upgrading Trigger(s) on Table TRADE_SUMMARY
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i UUU_TRADE_SUMMARY\TG_TRADE_SUMRY_JN_AER_DIU.sql -o logs\TG_TRADE_SUMRY_JN_AER_DIU.log
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i UUU_TRADE_SUMMARY\TG_TRDSUMMRY_AU.sql -o logs\TG_TRDSUMMRY_AU.log
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i UUU_TRADE_SUMMARY\TG_TRDSUMMRY_II.sql -o logs\TG_TRDSUMMRY_II.log
echo Creating Procedure(s) P_UPDATE_TRADE_SUMMARY_CPTY_TRADE_ID
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i AAA_TRADE_SUMMARY\PKG_TRADE_SUMMARY$P_UPDATE_TRADE_SUMMARY_CPTY_TRADE_ID.sql -o logs\P_UPDATE_TRADE_SUMMARY_CPTY_TRADE_ID.log
echo Updating View(s) V_PC_TRADE_SUMMARY
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i UUU_TRADE_SUMMARY\V_PC_TRADE_SUMMARY.sql -o logs\V_PC_TRADE_SUMMARY.log
REM ********************************************************************************
echo ******************************************************************************** 
echo Done
goto end
:WARNING
Echo Usage: dbupgrade_CNFMGR [server] [login] [password] [database]
:end
