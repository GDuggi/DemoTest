IF OBJECT_ID('ConfirmMgr.P_VAULT_ORIGINAL_ASSOCIATED_DOCS','P') IS NULL
EXEC ('CREATE PROCEDURE ConfirmMgr.P_VAULT_ORIGINAL_ASSOCIATED_DOCS AS RETURN 1')
GO
ALTER PROCEDURE ConfirmMgr.P_VAULT_ORIGINAL_ASSOCIATED_DOCS
@p_trade_id int   --This Variable receive a Trade_ID
AS
/******************************************************************************
*
* AUTHOR:		JAVIER MONTERO - 01/13/2016
* MODIFIED:		JAVIER MONTERO - 02/01/2016
* DB:			SQL SERVER 2012 OR HIGHER
* VERSION:		1.0
* DESCRIPTION:  PROCEDURE FOR SENT THE ASSOCIATED DOCS THAT HAVE SEND_FLAG = N
* DEPENCIES:    PROCEDURE P_INSERT_VAULT_REQUEST IS REQUIERED
				TABLES ASSOCIATED_DOCS, TRADE_RQMT, TRADE, VAULTED_DOCS
* MODIFICATION: CALLING STORED PROCEDURE P_INSERT_VAULT_REQUEST
*******************************************************************************/
DECLARE 
@associated_docs_id int,
@count				int,
@error_msg			nvarchar(max)

/*VARIABLE TABLE FOR SAVE THE VALUES RETRIEVED FROM THE SELECT, I USED THIS OPTION TO AVOID USE CURSORS, 
THE VARIABLE TABLE IS DESTROYED WHEN THE PROCEDURE IS FINISHED*/
DECLARE @ASSOCDOCID_TBL TABLE (id int)

BEGIN
	
	BEGIN TRY
		INSERT INTO @ASSOCDOCID_TBL (id)
		SELECT ad.ID
		FROM ConfirmMgr.ASSOCIATED_DOCS ad,
		ConfirmMgr.TRADE_RQMT trq,
		ConfirmMgr.TRADE tr
		WHERE ad.TRADE_RQMT_ID = trq.ID
		AND trq.TRADE_ID = tr.ID
		AND tr.ID = @p_trade_id
		EXCEPT
		SELECT ad.ID
		FROM ConfirmMgr.ASSOCIATED_DOCS ad,
		ConfirmMgr.TRADE_RQMT trq,
		ConfirmMgr.TRADE tr,
		ConfirmMgr.VAULTED_DOCS vd
		WHERE vd.ASSOCIATED_DOCS_ID = ad.ID
		AND ad.TRADE_RQMT_ID = trq.ID
		AND trq.TRADE_ID = tr.ID
		AND vd.SENT_FLAG = 'N'
		AND tr.ID = @p_trade_id;
	SELECT @count = MIN(id) FROM @ASSOCDOCID_TBL;

	WHILE (@count) IS NOT NULL	
		BEGIN
			SET @associated_docs_id = @count;
			-- DEBUG VALIDATE THE ASSOCIATED_DOCS_ID IS CORRECT
			-- PRINT 'COUNT ' + cast(@count as nvarchar) + ' ' + 'ASSO DOC ' + cast(@associated_docs_id as nvarchar)
			-- NOT IS NECESSARY USE TRANSACTION CONTROL HERE BECAUSE THE PROCEDURE P_INSERT_VAULT_REQUEST USE TRANSACTION CONTROL FOR THE DML TRANSACTION
			exec ConfirmMgr.P_INSERT_VAULT_REQUEST null, @associated_docs_id, 'N', null
			
			-- FETCHING THE NEXT RECORD 
			SELECT @count = MIN(id) FROM @ASSOCDOCID_TBL WHERE id > @count 
		END
	END TRY
	BEGIN CATCH
		SELECT @error_msg = ERROR_MESSAGE() + ' LINE: ' + CAST(ERROR_LINE() AS nvarchar);
		THROW 51000, @error_msg, 1
	END CATCH
END