IF OBJECT_ID('ConfirmMgr.TMP_MERCURIA_TRADE_DATA','U') IS NOT NULL
	EXEC ('DROP TABLE ConfirmMgr.TMP_MERCURIA_TRADE_DATA')
GO

PRINT 'TEMP TABLE MERCURIA_TRADE_DATA DROPPED WITH SUCCESS!!!!'
GO