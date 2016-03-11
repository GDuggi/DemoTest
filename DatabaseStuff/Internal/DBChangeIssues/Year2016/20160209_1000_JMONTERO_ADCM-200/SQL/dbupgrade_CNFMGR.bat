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
echo Fixing Data On Confirmation Manager database (MS SQL Server) for issue #ADCM-200 ...
REM ********************************************************************************
echo ********************************************************************************
echo Creating TMP_MERCURIA_TRADE_DATA Table(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i AAA_MERCURIA_EXCEL_TRADE_DATA\tmp_mercuria_trade_data.sql -o logs\oil_trades_tbl.log
echo Loading DATA to TMP_MERCURIA_TRADE_DATA Table(s)
call BCP_LOAD_DATA.bat %1 %2 %3 %4 > logs\bcp_trades.log
echo Fixing Trades on Confirmation Manager Database...
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i Looking_Trades_Date_Difference.sql >> logs\cnfmgr_trades_fix.log
echo Dropping Temp Tables...
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i Drop_Tables.sql >> logs\drop_table.log
REM ********************************************************************************
echo ******************************************************************************** 
echo Done
echo fixing Data on Confirmation Manager FINISHED
echo chao, chao
goto end
:WARNING
Echo Usage: dbupgrade_CNFMGR [server] [login] [password] [database]
:end
