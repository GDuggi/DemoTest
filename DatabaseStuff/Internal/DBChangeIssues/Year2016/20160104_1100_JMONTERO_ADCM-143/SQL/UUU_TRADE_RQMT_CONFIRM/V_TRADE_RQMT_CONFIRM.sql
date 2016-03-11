IF OBJECT_ID('ConfirmMgr.V_TRADE_RQMT_CONFIRM','V') IS NOT NULL
EXEC('DROP VIEW ConfirmMgr.V_TRADE_RQMT_CONFIRM')
GO

CREATE VIEW [ConfirmMgr].[V_TRADE_RQMT_CONFIRM]
AS
       SELECT c.id,
          c.rqmt_id,
          c.trade_id,
          c.NEXT_STATUS_CODE,
          c.CONFIRM_LABEL,
          c.confirm_cmt,
          c.fax_telex_ind,
          c.fax_telex_number,
          c.xmit_status_ind,
          c.xmit_addr,
          c.xmit_cmt,
          c.xmit_timestamp_gmt,
          c.template_name,
          s.final_approval_flag,
          c.active_flag,
		  c.PREPARER_CAN_SEND_FLAG
     FROM ConfirmMgr.trade_summary s
          INNER JOIN ConfirmMgr.trade_rqmt_confirm c
                ON  s.trade_id = c.trade_id

GO
PRINT ('VIEW C_TRADE_RQMT_CONFIRM CREATED WITH SUCCESS');
GO


