IF OBJECT_ID('ConfirmMgr.TG_TRADE_APPR_AER_I','TR') IS NOT NULL
EXEC ('DROP TRIGGER ConfirmMgr.TG_TRADE_APPR_AER_I')
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [ConfirmMgr].[TG_TRADE_APPR_AER_I] 
   ON  [ConfirmMgr].[TRADE_APPR]
   AFTER INSERT
AS 
BEGIN

	SET NOCOUNT ON;
		
	update ts 
		set FINAL_APPROVAL_FLAG = i.APPR_FLAG,
		FINAL_APPROVAL_TIMESTAMP_GMT = i.APPR_TIMESTAMP_GMT,
		TRANSACTION_SEQ = next value for ConfirmMgr.seq_trade_summary_transaction,
		LAST_UPDATE_TIMESTAMP_GMT = GetDate()			

		from ConfirmMgr.TRADE_SUMMARY ts
		inner join inserted i on i.TRADE_ID = ts.trade_id


	 DECLARE @payloads as AFFMSG.TABLE_CHANGE_SEND_MSG_TYPE;  
  
	insert into @payloads
	  select distinct 
        '1' + '|' + 
        cast(i.TRADE_ID as varchar) +'|FINALAPPROVAL|' + 
        cast(i.APPR_FLAG  as varchar )
	  from inserted i ;
   
   EXECUTE AFFMSG.TABLE_CHANGE_SEND_MSGS 'CONFIRMMGR.TRADE_APPR', @payloads   		
END

GO