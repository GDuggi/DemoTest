@echo off
rem BCP Loading Symphony Trade Data... 
bcp %4.ConfirmMgr.TMP_MERCURIA_TRADE_DATA in AAA_MERCURIA_EXCEL_TRADE_DATA\TRADE_EXCEL_DATA.dat -n -b 200 -S %1 -U %2 -P %3
rem
