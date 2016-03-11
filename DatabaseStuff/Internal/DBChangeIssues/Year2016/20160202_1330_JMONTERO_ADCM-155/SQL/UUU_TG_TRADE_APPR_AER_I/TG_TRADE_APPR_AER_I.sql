/****** Object:  Trigger [ConfirmMgr].[TG_TRADE_APPR_AER_I]    Script Date: 2/1/2016 3:51:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER TRIGGER [ConfirmMgr].[TG_TRADE_APPR_AER_I] 
   ON  [ConfirmMgr].[TRADE_APPR]
   AFTER INSERT
AS 
BEGIN
DECLARE 
@APP_FLAG	char(1),
@TRADE_ID	int

	SET NOCOUNT ON;
		SELECT @APP_FLAG = i.APPR_FLAG, @TRADE_ID = i.TRADE_ID  FROM inserted i;
	update ts 
		set FINAL_APPROVAL_FLAG = i.APPR_FLAG,
		FINAL_APPROVAL_TIMESTAMP_GMT = i.APPR_TIMESTAMP_GMT,
		TRANSACTION_SEQ = next value for ConfirmMgr.seq_trade_summary_transaction,
		LAST_UPDATE_TIMESTAMP_GMT = GetDate()
		from ConfirmMgr.TRADE_SUMMARY ts
		inner join inserted i on i.TRADE_ID = ts.trade_id

	IF(@APP_FLAG = 'Y' AND @TRADE_ID > 0)
	BEGIN
		EXEC ConfirmMgr.P_VAULT_ORIGINAL_ASSOCIATED_DOCS @TRADE_ID
	END

	 DECLARE @payloads as AFFMSG.TABLE_CHANGE_SEND_MSG_TYPE;  
  
	insert into @payloads
	  select distinct 
        '1' + '|' + 
        cast(i.TRADE_ID as varchar) +'|FINALAPPROVAL|' + 
        cast(i.APPR_FLAG  as varchar )
	  from inserted i ;
   
   EXECUTE AFFMSG.TABLE_CHANGE_SEND_MSGS 'CONFIRMMGR.TRADE_APPR', @payloads   		
END

