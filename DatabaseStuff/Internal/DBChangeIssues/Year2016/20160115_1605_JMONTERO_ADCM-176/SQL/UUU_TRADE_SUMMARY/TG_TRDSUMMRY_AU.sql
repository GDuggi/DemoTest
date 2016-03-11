IF OBJECT_ID('ConfirmMgr.TG_TRDSUMMRY_AU','TR') IS NOT NULL
EXEC ('DROP TRIGGER ConfirmMgr.TG_TRDSUMMRY_AU')
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE TRIGGER [ConfirmMgr].[TG_TRDSUMMRY_AU]
   ON  [ConfirmMgr].[TRADE_SUMMARY]
   AFTER UPDATE
AS 
BEGIN
	
	SET NOCOUNT ON;

	-- UPDATE() just checks whether the column was included in the update statement. 
	-- it does not compare old values to new so we have to check again in the where clause.
	if (NOT UPDATE(TRANSACTION_SEQ) AND 
		(UPDATE(CMT) or UPDATE(OPS_DET_ACT_FLAG) or UPDATE(CPTY_TRADE_ID)) )	
	BEGIN		
		-- Update the transaction sequence if we are updating a column that does not update when 
		-- a child record is touched (which would cause the transaction_seq to be updated).
		update ts
			set TRANSACTION_SEQ = next value for ConfirmMgr.SEQ_TRADE_SUMMARY_TRANSACTION,
			LAST_UPDATE_TIMESTAMP_GMT = GetDate()
			from TRADE_SUMMARY ts 
			inner join deleted old on ts.id = old.id
			inner join inserted new on ts.id = new.id		
			where
				((old.CMT <> new.CMT) 
				or (old.OPS_DET_ACT_FLAG <> new.OPS_DET_ACT_FLAG)
				or (old.CPTY_TRADE_ID <> new.CPTY_TRADE_ID))
				and old.TRANSACTION_SEQ = new.TRANSACTION_SEQ; 
			
	END

	-- If the user re-opened a confirmation need to fire AQ messages so that the confirms appear on the screen
	IF (UPDATE(FINAL_APPROVAL_FLAG))
	BEGIN
		DECLARE @TradeRqmtConfirmNotification as AFFMSG.TABLE_CHANGE_SEND_MSG_TYPE;  
  
		insert into @TradeRqmtConfirmNotification
		select 
			'1' + '|' + 
			cast(trc.id as varchar) +'|' + 
			cast(trc.rqmt_id as varchar ) +'|' + 
			cast(trc.trade_id as varchar ) as payload               
		from inserted new
			inner join deleted old on old.id = new.id
			inner join TRADE_RQMT_CONFIRM trc on trc.TRADE_ID = new.TRADE_ID
		where old.FINAL_APPROVAL_FLAG = 'Y' and new.FINAL_APPROVAL_FLAG = 'N';

		if EXISTS (SELECT 1 from @TradeRqmtConfirmNotification)
		BEGIN
			EXECUTE AFFMSG.TABLE_CHANGE_SEND_MSGS 'CONFIRMMGR.TRADE_RQMT_CONFIRM', @TradeRqmtConfirmNotification;
		END
	END
    
END



GO

EXEC sp_settriggerorder @triggername=N'[ConfirmMgr].[TG_TRDSUMMRY_AU]', @order=N'First', @stmttype=N'UPDATE'
GO

