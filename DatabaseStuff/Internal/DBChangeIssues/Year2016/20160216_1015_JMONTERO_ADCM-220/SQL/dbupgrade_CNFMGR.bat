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
echo Upgrading Confirmation Manager database (MS SQL Server) for issue #ADCM-220 ...
REM ********************************************************************************
echo ********************************************************************************
echo Creating USER_MASTER Table(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i AAA_USER_MASTER\USER_MASTER.sql -o logs\USER_MASTER_tbl.log
echo Creating DATA on USER_MASTER Table(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i AAA_USER_MASTER\MERGEDATA_USERROLE_USERMASTER.sql -o logs\MERGEDATA_USERROLE_USERMASTER.log
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i AAA_USER_MASTER\MERGEDATA_USERFILTEROPSMGR_USERMASTER.sql -o logs\MERGEDATA_USERFILTEROPSMGR_USERMASTER.log
echo Creating Primary Key PK_USER_MASTER on USER_MASTER Table(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i AAA_USER_MASTER\PK_USER_MASTER.sql -o logs\PK_USER_MASTER.log
echo Creating Foreging Key FK_USERMASTER_USERROLE
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i AAA_USER_MASTER\FK_USERMASTER_USERROLE.sql -o logs\FK_USERMASTER_USERROLE.log
echo Creating Foreging Key FK_USERMASTER_FILTERSOPSMGR
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i AAA_USER_MASTER\FK_USERMASTER_FILTERSOPSMGR.sql -o logs\FK_USERMASTER_FILTERSOPSMGR.log
echo Creating Constraint CHK_USER_MASTER_ACTIVEFLAG
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i AAA_USER_MASTER\CHK_USER_MASTER_ACTIVEFLAG.sql -o logs\CHK_USER_MASTER_ACTIVEFLAG.log
REM ********************************************************************************
echo ******************************************************************************** 
echo Done
echo Upgrading DB version....
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i dbversion.sql -o logs\dbversion.log
echo Upgrading Confirmation Manager FINISHED
echo chao, chao
goto end
:WARNING
Echo Usage: dbdebugger_TRADE [server] [login] [password] [database]
:end
