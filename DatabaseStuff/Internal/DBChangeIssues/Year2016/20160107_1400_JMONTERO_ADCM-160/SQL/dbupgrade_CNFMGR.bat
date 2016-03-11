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
echo Upgrading CONFIRMATION MANAGER database (MS SQL Server) for issue #ADCM-160 ...
REM ********************************************************************************
echo ********************************************************************************
echo Upgrading ASSOCIATED_DOCS Table(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i UUU_ASSOCIATED_DOCS\UUU_ASSOCIATED_DOCS.sql -o logs\UUU_ASSOCIATED_DOCS.log
echo Upgrading ASSOCIATED_DOCS_JN Table(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i UUU_ASSOCIATED_DOCS_JN\UUU_ASSOCIATED_DOCS_JN.sql -o logs\UUU_ASSOCIATED_DOCS_JN.log
echo Upgrading INBOUND_DOCS Table(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i UUU_INBOUND_DOCS\UUU_INBOUND_DOCS.sql -o logs\UUU_INBOUND_DOCS.log
echo Upgrading INBOUND_DOCS_JN Table(s)
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i UUU_INBOUND_DOCS_JN\UUU_INBOUND_DOCS_JN.sql -o logs\UUU_INBOUND_DOCS_JN.log
echo Upgrading PROCEDURE(s) on Table ASSOCIATED_DOCS
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i UUU_ASSOCIATED_DOCS\P_UPDATE_ASSO_STATUS.sql -o logs\UUU_P_UPDATE_ASSO_STATUS.log
echo Upgrading View(s) ASSOCIATED_DOCS
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i UUU_ASSOCIATED_DOCS\V_ACTIVE_ASSOCIATED_DOCS.sql -o logs\UUU_V_ACTIVE_ASSOCIATED_DOCS.log
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i UUU_ASSOCIATED_DOCS\V_ASSOCIATED_DOCS.sql -o logs\UUU_V_ASSOCIATED_DOCS.log
echo Upgrading PROCEDURE(s) on Table INBOUND_DOCS
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i UUU_INBOUND_DOCS\PKG_INBOUND$P_UPDATE_ASSO_STATUS.sql -o logs\UUU_PKG_INBOUND$P_UPDATE_ASSO_STATUS.log
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i UUU_INBOUND_DOCS\PKG_INBOUND$P_UPDATE_INBOUND_DOC.sql -o logs\UUU_PKG_INBOUND$P_UPDATE_INBOUND_DOC.log
echo Upgrading View(s) INBOUND_DOCS
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i UUU_INBOUND_DOCS\V_INBOUND_DOCS.sql -o logs\UUU_V_INBOUND_DOCS.log

REM ********************************************************************************
echo ******************************************************************************** 
%ISQL% -S %1 -U %2 -P %3 -d %4 -w 250 -i dbversion.sql 
echo Done
goto end
:WARNING
Echo Usage: dbupgrade_CNFMGR [server] [login] [password] [database]
:end
