
IF EXISTS(SELECT 1 FROM sys.objects WHERE name = 'TG_TRADERQMT_STATUS_ASU')
DROP TRIGGER ConfirmMgr.TG_TRADERQMT_STATUS_ASU
GO


CREATE TRIGGER [ConfirmMgr].TG_TRADERQMT_STATUS_ASU
  -- AQ PayLoad: VERSION | ID | RQMT_ID | TRADE_ID
  on [ConfirmMgr].[TRADE_RQMT]
  after update
as  

    if (@@rowcount = 0)
	return;
	  
	DECLARE @payloads as AFFMSG.TABLE_CHANGE_SEND_MSG_TYPE;  
  
	insert into @payloads  
	select 
			'1' + '|' + 
			cast(i.ID as varchar) +'|' + 
			i.RQMT +'|' + 
			i.STATUS
	from inserted i
		inner join deleted d on d.id = i.ID
	where d.status <> i.status;
   
	EXECUTE AFFMSG.TABLE_CHANGE_SEND_MSGS 'CONFIRMMGR.TRADE_RQMT_STATUS_CHANGE', @payloads
GO