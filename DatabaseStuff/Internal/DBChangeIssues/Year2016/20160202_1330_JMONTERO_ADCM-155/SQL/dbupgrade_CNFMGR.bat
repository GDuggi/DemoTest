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
echo Upgrading CONFIRMATION MANAGER database (MS SQL Server) for issue #ADCM-155 ...
REM ********************************************************************************
echo ********************************************************************************
echo ALTERING TG_TRADE_APPR_AER_I Trigger(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i uuu_TG_TRADE_APPR_AER_I\TG_TRADE_APPR_AER_I.sql -o logs\TG_TRADE_APPR_AER_I.log
echo ALTERING P_INSERT_VAULT_REQUEST Procedure(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i uuu_P_INSERT_VAULT_REQUEST\P_INSERT_VAULT_REQUEST.sql -o logs\P_INSERT_VAULT_REQUEST.log
echo UPDATING P_VAULT_ORIGINAL_ASSOCIATED_DOCS Procedure(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i uuu_P_VAULT_ORIGINAL_ASSOCIATED_DOCS\P_VAULT_ORIGINAL_ASSOCIATED_DOCS.sql -o logs\P_VAULT_ORIGINAL_ASSOCIATED_DOCS.log
echo UPDATING V_DOCS_TO_VAULT View(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i UUU_V_DOCS_TO_VAULT\V_DOCS_TO_VAULT.sql -o logs\V_DOCS_TO_VAULT.log
REM ********************************************************************************
echo ******************************************************************************** 
echo Updating DB Version Info ......
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i dbversion.sql
echo Done
goto end
:WARNING
Echo Usage: dbupgrade_CNFMGR [server] [login] [password] [database]
:end
