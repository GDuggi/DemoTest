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
echo Upgrading CONFIRMATION MANAGER database (MS SQL Server) for issue #ADCM-143 ...
REM ********************************************************************************
echo ********************************************************************************
echo Upgrading TRADE_RQMT_CONFIRM Table(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i UUU_TRADE_RQMT_CONFIRM\UUU_TRADE_RQMT_CONFIRM.sql -o logs\UUU_TRADE_RQMT_CONFIRM.log
echo Upgrading TRADE_RQMT_CONFIRM_JN Table(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i UUU_TRADE_RQMT_CONFIRM_JN\UUU_TRADE_RQMT_CONFIRM_JN.sql -o logs\UUU_TRADE_RQMT_CONFIRM_JN.log
echo Upgrading Trigger(s) on Table TRADE_RQMT_CONFIRM
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i UUU_TRADE_RQMT_CONFIRM\UUU_TG_TRADERQMTCONFIRM_JN_AER_DIU.sql -o logs\UUU_TG_TRADERQMTCONFIRM_JN_AER_DIU.log
echo Upgrading View(s) V_TRADE_RQMT_CONFIRM
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i UUU_TRADE_RQMT_CONFIRM\V_TRADE_RQMT_CONFIRM.sql -o logs\V_TRADE_RQMT_CONFIRM.log
echo Creating Check Constraint(s) CHK_PREPARER_CAN_SEND_FLAG on TRADE_RQMT_CONFIRM
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i AAA_TRADE_RQMT_CONFIRM\CHK_PREPARER_CAN_SEND_FLAG.sql -o logs\CHK_PREPARER_CAN_SEND_FLAG.log


REM ********************************************************************************
echo ******************************************************************************** 
echo Done
goto end
:WARNING
Echo Usage: dbupgrade_CNFMGR [server] [login] [password] [database]
:end
