IF OBJECT_ID('ConfirmMgr.TG_TRADERQMT_AS_IU','TR') IS NOT NULL
EXEC ('DROP TRIGGER ConfirmMgr.TG_TRADERQMT_AS_IU')
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [ConfirmMgr].[TG_TRADERQMT_AS_IU]
    ON [ConfirmMgr].[TRADE_RQMT]
    FOR INSERT, UPDATE
    AS
BEGIN
	
	if (@@rowcount = 0)
		return;

    SET NOCOUNT ON

	DECLARE @TradeCounts TABLE
	(
		TRADE_ID int not null primary key,
		NON_TERMINAL_COUNT INT NOT NULL default(0),
		PROBLEM_COUNT INT NOT NULL DEFAULT(0),		
		READY_FOR_FINAL_APPROVAL varchar(1) not null default 'N',
		PROBLEM_FLAG VARCHAR(1) not null default 'N'
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
			 inner join inserted i 
				on i.TRADE_ID = tr.TRADE_ID
             inner join ConfirmMgr.rqmt_status rs 
				on tr.RQMT = rs.RQMT_CODE and tr.STATUS = rs.STATUS_CODE
		     

        group by tr.trade_id;

	update @TradeCounts 
		set READY_FOR_FINAL_APPROVAL = 
				case when NON_TERMINAL_COUNT = 0 then 'Y'
					 else 'N'
				end,
			PROBLEM_FLAG = 
				case when PROBLEM_COUNT > 0 then 'Y'
					else 'N'
				end			
	update tc
		set tc.READY_FOR_FINAL_APPROVAL = 'N'
		from @TradeCounts tc
		inner join ConfirmMgr.TRADE_SUMMARY ts
			on tc.TRADE_ID = ts.TRADE_ID
		where ts.OPS_DET_ACT_FLAG in ('E', 'R')

	--select * from @TradeCounts
	print N'TG_TRADERQMT_AS_IU - updating ready_for_final_approval and HAS_PROBLEM_FLAG on TRADE_SUMMARY'
	
	update ts 
		set ts.READY_FOR_FINAL_APPROVAL_FLAG = tc.READY_FOR_FINAL_APPROVAL,
			ts.HAS_PROBLEM_FLAG = tc.PROBLEM_FLAG,
			ts.TRANSACTION_SEQ = next value for ConfirmMgr.SEQ_TRADE_SUMMARY_TRANSACTION,
			ts.LAST_UPDATE_TIMESTAMP_GMT = GetDate()			
			
		from ConfirmMgr.TRADE_SUMMARY ts
			inner join @TradeCounts tc on tc.TRADE_ID = ts.TRADE_ID
	
END





GO

EXEC sp_settriggerorder @triggername=N'[ConfirmMgr].[TG_TRADERQMT_AS_IU]', @order=N'Last', @stmttype=N'INSERT'
GO

EXEC sp_settriggerorder @triggername=N'[ConfirmMgr].[TG_TRADERQMT_AS_IU]', @order=N'Last', @stmttype=N'UPDATE'
GO

