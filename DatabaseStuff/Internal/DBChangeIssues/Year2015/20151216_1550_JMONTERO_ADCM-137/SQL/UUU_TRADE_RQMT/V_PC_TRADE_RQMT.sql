IF OBJECT_ID('ConfirmMgr.V_PC_TRADE_RQMT','V') IS NOT NULL
EXEC ('DROP VIEW ConfirmMgr.V_PC_TRADE_RQMT')
GO

CREATE VIEW [ConfirmMgr].[V_PC_TRADE_RQMT] 
AS
	SELECT 
      tr.ID, 
      tr.TRADE_ID, 
	  t.TRD_SYS_CODE,
	  t.TRD_SYS_TICKET,
      tr.RQMT_TRADE_NOTIFY_ID, 
      tr.RQMT, 
      tr.STATUS, 
      tr.COMPLETED_DT, 
      tr.COMPLETED_TIMESTAMP_GMT, 
      tr.REFERENCE, 
      tr.CANCEL_TRADE_NOTIFY_ID, 
      tr.CMT, 
      tr.SECOND_CHECK_FLAG, 
      ts.TRANSACTION_SEQ, 
      ts.FINAL_APPROVAL_FLAG, 
      r.DISPLAY_TEXT, 
      r.CATEGORY, 
      rs.TERMINAL_FLAG, 
      rs.PROBLEM_FLAG, 
      rs.GUI_COLOR_CODE, 
      gc.DELPHI_CONSTANT
   FROM 
      ConfirmMgr.TRADE  AS t, 
      ConfirmMgr.TRADE_RQMT  AS tr, 
      ConfirmMgr.TRADE_SUMMARY  AS ts, 
      ConfirmMgr.RQMT  AS r,  
      ConfirmMgr.RQMT_STATUS  AS rs, 
      ConfirmMgr.GUI_COLOR  AS gc
   WHERE 
      t.id=tr.trade_id AND
	  tr.RQMT = r.CODE AND 
      tr.STATUS = rs.STATUS_CODE AND 
      tr.TRADE_ID = ts.TRADE_ID AND 
      r.CODE = rs.RQMT_CODE AND 
      gc.CODE = rs.GUI_COLOR_CODE;
GO

PRINT ' VIEW ConfirmMgr.V_PC_TRADE_RQMT CREATED SUCCESSFULLY!!!'
GO
