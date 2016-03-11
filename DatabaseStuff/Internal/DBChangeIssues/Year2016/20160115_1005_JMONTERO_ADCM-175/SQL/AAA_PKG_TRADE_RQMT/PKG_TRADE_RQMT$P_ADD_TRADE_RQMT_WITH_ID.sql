IF OBJECT_ID('ConfirmMgr.PKG_TRADE_RQMT$P_ADD_TRADE_RQMT_WITH_ID','P') IS NOT NULL
EXEC ('DROP PROCEDURE ConfirmMgr.PKG_TRADE_RQMT$P_ADD_TRADE_RQMT_WITH_ID')
GO
CREATE PROCEDURE [ConfirmMgr].[PKG_TRADE_RQMT$P_ADD_TRADE_RQMT_WITH_ID]
@p_id           int,
@p_trade_id		int,
@p_rqmt_code	varchar(10),
@p_reference	varchar(max),
@p_cmt			varchar(max)

AS

DECLARE
@error_msg		nvarchar(max),
@error_id		int,
@error_sev		int,
@error_sta		int,
@error_line		int

BEGIN

	
	BEGIN TRY
	
		INSERT INTO ConfirmMgr.TRADE_RQMT
		(
			ID,
			TRADE_ID,
			RQMT,
			REFERENCE,
			STATUS,
			CMT,
			SECOND_CHECK_FLAG

		)

		VALUES

		(
			@p_id,
			@p_trade_id,
			@p_rqmt_code,
			@p_reference,
			ConfirmMgr.PKG_TRADE_RQMT$F_GET_INITIAL_STATUS(@p_rqmt_code),
			@p_cmt,
			'N'	
		)

	END TRY
	BEGIN CATCH
		IF @@ERROR > 0
			SELECT @error_msg  = 'PROCEDURE PKG_TRADE_RQMT$P_ADD_TRADE_RQMT FAIL: ' + ERROR_MESSAGE(),
				   @error_id = ERROR_NUMBER(),
				   @error_sev = ERROR_SEVERITY(),
				   @error_sta = ERROR_STATE(),
				   @error_line = ERROR_LINE();
			RAISERROR(@error_msg, @error_id, @error_sev, @error_sta, @error_line) WITH LOG
			
	END CATCH

END
GO
PRINT 'PROCEDURE PKG_TRADE_RQMT$P_ADD_TRADE_RQMT_WITH_ID CREATED WITH SUCCESS!!! '
GO