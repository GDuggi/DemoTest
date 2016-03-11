IF OBJECT_ID('ConfirmMgr.V_XMIT_RESULT','V') IS NOT NULL
EXEC('DROP VIEW ConfirmMgr.V_XMIT_RESULT')
GO
CREATE VIEW ConfirmMgr.V_XMIT_RESULT 
AS
SELECT
xrq.id xmit_request_id,
xrs.id xmit_result_id,
xrq.ASSOCIATED_DOCS_ID,
xrq.TRADE_RQMT_CONFIRM_ID, 
xrq.SENT_BY_USER,
xrs.XMIT_STATUS_IND,
xrs.XMIT_METHOD_IND,
xrs.XMIT_DEST,
xrs.XMIT_CMT,
xrs.XMIT_TIMESTAMP
FROM XMIT_REQUEST xrq,
xmit_result xrs
WHERE xrs.XMIT_REQUEST_ID=xrq.id
GO
PRINT 'VIEW V_XMIT_RESULT CREATED SUCCESSFULLY'
GO