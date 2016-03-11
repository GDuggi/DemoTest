declare
@errormsg		nvarchar(max),
@rowsaffected	int

SET NOCOUNT ON
BEGIN
	
	BEGIN TRAN TRN01
	BEGIN TRY
		/*FIXING SYMPHONY DATA WITH START(FROM) AND END(TO) DATE CORRECT */
					
			UPDATE td
			SET td.START_DT = B.DEL_START_DATE, td.END_DT = B.DEL_END_DATE
			FROM ConfirmMgr.TRADE_DATA td
			INNER JOIN ConfirmMgr.TRADE t
			ON td.TRADE_ID = t.ID
			INNER JOIN ConfirmMgr.TMP_MERCURIA_TRADE_DATA B
			ON t.TRD_SYS_TICKET = B.TRADE
			WHERE td.START_DT != B.DEL_START_DATE
			AND td.END_DT != B.DEL_END_DATE;
			
			SET @rowsaffected = @@ROWCOUNT					
			
	END TRY

	BEGIN CATCH
		IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION TRAN01
		PRINT '==> FAILED TO UPDATED INCORRECT START_DATE/orEND_DATE DUE TO ERROR!!!'
		SELECT @errormsg = '==> ERROR: ' + ERROR_MESSAGE() + ' LINE: ' + CAST(ERROR_LINE() AS varchar);
		THROW 51000, @errormsg, 1
		goto EoS
	END CATCH
	COMMIT TRANSACTION TRAN01;
	PRINT 'ROWS UPDATED: ' + CAST(@rowsaffected as varchar);		
END
EoS:
GO
