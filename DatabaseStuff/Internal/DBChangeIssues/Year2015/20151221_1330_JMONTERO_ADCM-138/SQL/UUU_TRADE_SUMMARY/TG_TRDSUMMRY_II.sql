ALTER TRIGGER [ConfirmMgr].[TG_TRDSUMMRY_II]
   ON  [ConfirmMgr].[TRADE_SUMMARY]
   INSTEAD OF INSERT
AS 
BEGIN
	SET NOCOUNT ON;

    -- Calculate computed columns for READY_FOR_FINAL_APPROVAL and HAS_PROBLEMS_FLAG based 
	-- on child records, then insert all columns
	DECLARE @TradeCounts TABLE
	(
		TRADE_ID int not null primary key,
		NON_TERMINAL_COUNT INT NOT NULL default(0),
		PROBLEM_COUNT INT NOT NULL DEFAULT(0)
	);

	INSERT INTO @TradeCounts(TRADE_ID, NON_TERMINAL_COUNT, PROBLEM_COUNT)
	select tr.trade_id,					   
           sum( CASE 
					when terminal_flag = 'N' then 1
					else 0 
				end ) as non_terminal_count,		   
		   sum( CASE 
					when problem_flag = 'Y' then 1
					else 0 
				end ) as problem_count
        from ConfirmMgr.trade_rqmt tr
             inner join ConfirmMgr.rqmt_status rs on
				tr.RQMT = rs.RQMT_CODE and tr.STATUS = rs.STATUS_CODE
		     inner join inserted i on i.ID = tr.ID        
        group by tr.trade_id;  

		INSERT INTO [ConfirmMgr].[TRADE_SUMMARY]
           ([ID]
           ,[TRADE_ID]
           ,[OPEN_RQMTS_FLAG]
           ,[CATEGORY]
           ,[LAST_UPDATE_TIMESTAMP_GMT]
           ,[FINAL_APPROVAL_FLAG]
           ,[CMT]
           ,[LAST_TRD_EDIT_TIMESTAMP_GMT]
           ,[OPS_DET_ACT_FLAG]
           ,[READY_FOR_FINAL_APPROVAL_FLAG]
           ,[HAS_PROBLEM_FLAG]
           ,[TRANSACTION_SEQ]
           ,[FINAL_APPROVAL_TIMESTAMP_GMT]
		   ,[CPTY_TRADE_ID])
		SELECT 
           i.id,
           i.trade_id,
		   i.OPEN_RQMTS_FLAG,
		   i.category,
		   ISNULL(i.LAST_UPDATE_TIMESTAMP_GMT,GETDATE()),
           i.FINAL_APPROVAL_FLAG,
           i.CMT ,
           ISNULL(i.LAST_TRD_EDIT_TIMESTAMP_GMT,GETDATE()),
           i.OPS_DET_ACT_FLAG,
           CASE 
				when (tc.NON_TERMINAL_COUNT = 0 and i.OPS_DET_ACT_FLAG not in ('E','R')) then 'Y'
				else 'N'
		   end as READY_FOR_FINAL_APPROVAL_FLAG,
           case 
				WHEN (tc.PROBLEM_COUNT > 0) then 'Y'
				else 'N'
		   end as HAS_PROBLEM_FLAG,           
           i.TRANSACTION_SEQ, 
           i.FINAL_APPROVAL_TIMESTAMP_GMT,
		   i.CPTY_TRADE_ID
		   from inserted i 
			left outer join @TradeCounts tc on i.TRADE_ID = tc.TRADE_ID	
END
GO
PRINT 'TRIGGER TG_TRDSUMMRY_II ALTERED WITH SUCCESS'
GO