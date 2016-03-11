IF OBJECT_ID('ConfirmMgr.V_ACTIVE_ASSOCIATED_DOCS','V') IS NOT NULL
EXEC('DROP VIEW ConfirmMgr.V_ACTIVE_ASSOCIATED_DOCS')
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW ConfirmMgr.V_ACTIVE_ASSOCIATED_DOCS
AS
		SELECT   
         ad.id,
          ad.inbound_docs_id,
		  t.trd_sys_code,
		  t.trd_sys_ticket,
          'N' AS trade_final_approval_flag,
          ad.index_val,
          ad.file_name,
          ad.trade_id,
          ad.doc_status_code,
          ad.associated_by,
          ad.associated_dt,
          ad.final_approved_by,
          ad.final_approved_dt,
          ad.disputed_by,
          ad.disputed_dt,
          ad.discarded_by,
          ad.discarded_dt,
          ad.vaulted_by,
          ad.vaulted_dt,
          ad.cdty_group_code,
          ad.cpty_sn,
          ad.broker_sn,
          ad.doc_type_code,
          ad.sec_validate_req_flag,
          ad.trade_rqmt_id,
          ad.xmit_status_code,
          ad.xmit_value,
          ib.sent_to
     FROM 
		ConfirmMgr.trade as t,
			ConfirmMgr.associated_docs ad
		  INNER JOIN ConfirmMgr.inbound_docs ib
		  ON ad.inbound_docs_id = ib.id
		  INNER JOIN ConfirmMgr.trade_summary ts		  
		  ON ad.trade_id = ts.trade_id
    WHERE ts.final_approval_flag = 'N'
	and ts.TRADE_ID=t.id
           
   UNION ALL
		SELECT ad.id,
          ad.inbound_docs_id,
		  null trd_sys_code,
		  null trd_sys_ticket,
          'N' AS trade_final_approval_flag,
          ad.index_val,
          ad.file_name,
          ad.trade_id,
          ad.doc_status_code,
          ad.associated_by,
          ad.associated_dt,
          ad.final_approved_by,
          ad.final_approved_dt,
          ad.disputed_by,
          ad.disputed_dt,
          ad.discarded_by,
          ad.discarded_dt,
          ad.vaulted_by,
          ad.vaulted_dt,
          ad.cdty_group_code,
          ad.cpty_sn,
          ad.broker_sn,
          ad.doc_type_code,
          ad.sec_validate_req_flag,
          ad.trade_rqmt_id,
          ad.xmit_status_code,
          ad.xmit_value,
          ib.sent_to
		 FROM 
		 ConfirmMgr.associated_docs ad 
		 INNER JOIN	ConfirmMgr.inbound_docs ib
		 ON ad.inbound_docs_id = ib.id
		WHERE ad.trade_id = 0


GO

GRANT SELECT TO stanford_developers
GO
