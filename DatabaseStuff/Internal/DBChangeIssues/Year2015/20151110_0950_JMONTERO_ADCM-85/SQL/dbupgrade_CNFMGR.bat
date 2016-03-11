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
echo Upgrading CNF_MGR database (MS SQL Server) for issue #ADCM-85 ...
REM ********************************************************************************
echo ********************************************************************************
REM ********************************************************************************
echo Creating XMIT_REQUEST object(s) ...
echo Creating Table...
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i NewTables\XMIT_REQUEST_TBL.sql -o logs\XMIT_REQUEST_TBL.log
echo Creating Objects...
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i AAA_XMIT_REQUEST\SEQ_XMIT_REQUEST.sql -o logs\SEQ_XMIT_REQUEST.log
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i AAA_XMIT_REQUEST\P_INSERT_XMIT_REQUEST.sql -o logs\P_INSERT_XMIT_REQUEST.log
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i AAA_XMIT_REQUEST\TR_XMIT_REQUEST_BER_IU.sql -o logs\TR_XMIT_REQUEST_BER_IU.log
echo Creating XMIT_RESULT object(s) ...
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i AAA_XMIT_REQUEST\SEQ_XMIT_RESULT.sql -o logs\SEQ_XMIT_RESULT.log
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i AAA_XMIT_REQUEST\P_INSERT_XMIT_RESULT.sql -o logs\P_INSERT_XMIT_RESULT.log
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i AAA_XMIT_REQUEST\V_XMIT_RESULT.sql -o logs\V_XMIT_RESULT.log
echo Creating XMIT_RESULT object(s) ...
echo Creating Table(s)...
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 600 -i NewTables\XMIT_STATUS_TBL.sql -o logs\XMIT_STATUS_TBL.log
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
