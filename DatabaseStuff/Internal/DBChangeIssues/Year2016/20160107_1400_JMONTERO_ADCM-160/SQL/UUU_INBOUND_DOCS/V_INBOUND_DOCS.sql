IF OBJECT_ID('ConfirmMgr.V_INBOUND_DOCS','V') IS NOT NULL
EXEC('DROP VIEW ConfirmMgr.V_INBOUND_DOCS')
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW ConfirmMgr.V_INBOUND_DOCS
AS
	SELECT IsNull(ct, 0) UNRESOLVEDCOUNT,
            inb.ID,
            inb.MAPPED_CPTY_SN,
            inb.caller_ref,
            inb.sent_to,
            inb.rcvd_ts,
            inb.file_name,
            inb.sender,
            inb.cmt,
            inb.doc_status_code,
            inb.has_auto_ascted_flag,
            inb.proc_flag,
            ConfirmMgr.f_associated_trade_ids (inb.id) AS TRADEIDS
       FROM ConfirmMgr.inbound_docs inb
	   LEFT OUTER JOIN
            (  SELECT inbound_docs_id, COUNT (*) ct
                 FROM ConfirmMgr.associated_docs
                WHERE associated_docs.DOC_STATUS_CODE IN
                         ('ASSOCIATED',
                          'PRE-APPROVED',
                          'UNASSOCIATED',
                          'DISPUTED')
             GROUP BY inbound_docs_id) assoc
	  ON inb.id=assoc.INBOUND_DOCS_ID
      WHERE inb.proc_flag = 'Y'


GO

GRANT SELECT TO stanford_developers
GO



