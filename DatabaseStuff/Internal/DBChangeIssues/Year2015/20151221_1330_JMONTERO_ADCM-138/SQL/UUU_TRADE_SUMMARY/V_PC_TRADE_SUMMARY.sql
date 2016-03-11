IF OBJECT_ID('ConfirmMgr.V_PC_TRADE_SUMMARY','V') IS NOT NULL
EXEC ('DROP VIEW ConfirmMgr.V_PC_TRADE_SUMMARY')
GO

CREATE VIEW ConfirmMgr.V_PC_TRADE_SUMMARY
AS

SELECT 
         DISTINCT
          T.TRD_SYS_CODE,
          T.TRD_SYS_TICKET,
          (SELECT MAX (TRADE_VERSION)
            FROM CONFIRMMGR.TRADE_NOTIFY
            WHERE TRADE_ID = TS.TRADE_ID
            GROUP BY TRADE_ID)
            VERSION,
          BD.CURRENT_BUSN_DT,
--          GREATEST (--  5 - ROUND ( (BD.RIGHTNOW - LAST_TRD_EDIT_TIMESTAMP_GMT) * 90,0),  --0)
          0 RECENT_IND,
          TS.CMT,
		  TS.CPTY_TRADE_ID,
          TS.LAST_UPDATE_TIMESTAMP_GMT,
          TS.LAST_TRD_EDIT_TIMESTAMP_GMT,
          TS.READY_FOR_FINAL_APPROVAL_FLAG,
          TS.HAS_PROBLEM_FLAG,
          TS.FINAL_APPROVAL_FLAG,
          TS.FINAL_APPROVAL_TIMESTAMP_GMT,
          TS.OPS_DET_ACT_FLAG,
          TS.TRANSACTION_SEQ,
          -- DECODE(NVL(VB.RQMT,
          -- 'N'),
          -- 'N',
          -- 'N',
          -- 'Y') BKR_REQD,
          VB.RQMT BKR_RQMT,
                CASE ISNULL(VB.RQMT,'N')
                WHEN 'N' THEN NULL
                WHEN 'XQBBP' THEN 'BROKER PAPER'+
                CASE VB.SECOND_CHECK_FLAG
                     WHEN 'Y' THEN '*'
                     ELSE ''
              END
              WHEN 'EFBKR' THEN 'EFET BROKER'
              WHEN 'ECBKR' THEN 'ECONFIRM BROKER'
              ELSE '?'
              END AS BKR_METH,
              VB.STATUS BKR_STATUS,
        VB.ID BKR_DB_UPD,
        VS.RQMT SETC_RQMT,
              CASE ISNULL(VS.RQMT,'N')
              WHEN 'N' THEN NULL
              WHEN 'ECONF' THEN ' ECONFIRM CPTY '
              WHEN 'EFET' THEN 'EFET CPTY'
              WHEN 'XQCSP' THEN 'OUR PAPER'
              ELSE '?'
              END AS SETC_METH,
              VS.STATUS SETC_STATUS,
        VS.ID SETC_DB_UPD,
        VC.RQMT CPTY_RQMT,
              CASE ISNULL(VC.RQMT,'N')
              WHEN 'N' THEN NULL
              WHEN 'XQCCP' THEN 'CPTY PAPER'+
              CASE VC.SECOND_CHECK_FLAG
                     WHEN 'Y' THEN '*'
                     ELSE ''
              END
              ELSE '?'
              END AS CPTY_METH,
              VC.STATUS CPTY_STATUS,
        VC.ID CPTY_DB_UPD,
        VX.RQMT NOCONF_RQMT,  
              CASE ISNULL(VX.RQMT,'N')
              WHEN 'N' THEN NULL
              WHEN 'NOCNF' THEN VX.REFERENCE
              ELSE '?'
              END AS NOCONF_METH,
              VX.STATUS NOCONF_STATUS,
        VX.ID NOCONF_DB_UPD,
        VV.RQMT VERBL_RQMT, 
              CASE ISNULL(VV.RQMT,'N')
              WHEN 'N' THEN NULL
              WHEN 'VBCP' THEN 'PHONE'
              WHEN 'MISC' THEN 'MISC'
              ELSE '?'
              END AS VERBL_METH,
              VV.STATUS VERBL_STATUS,
        VV.ID VERBL_DB_UPD,
        TG.XREF GROUP_XREF,
        TD.ID,
        TS.TRADE_ID,
        TD.INCEPTION_DT,
              TD.PERMISSION_KEY,
        TD.CDTY_CODE,
        TD.TRADE_DT,
        TD.TRADER,
        TD.XREF,
        TD.CPTY_SN,
        TD.CPTY_LEGAL_NAME,
              TD.CPTY_ID,
              TD.TRADE_DESC,
        TD.QTY_DESC,
        TD.QTY_TOT,
        0 QTY,
        '' UOM_DUR_CODE,
        TD.LOCATION_SN,
        TD.PRICE_DESC,
        TD.TRANSPORT_DESC,
        TD.START_DT,
        TD.END_DT,
        TD.BOOK,
        TD.TRADE_TYPE_CODE,
        TD.STTL_TYPE,
        TD.BROKER_SN,
              TD.BROKER_LEGAL_NAME,
              TD.BROKER_ID,
        '' COMM,
        TD.BUY_SELL_IND,
        TD.REF_SN,
        '' PAY_PRICE,
        '' REC_PRICE,
        '' SE_CPTY_SN,
              TD.BOOKING_CO_SN,
              TD.BOOKING_CO_ID,
        TD.TRADE_STAT_CODE,
        TD.CDTY_GRP_CODE,
        TD.BROKER_PRICE,
        TD.OPTN_STRIKE_PRICE,
        TD.OPTN_PREM_PRICE,
        TD.OPTN_PUT_CALL_IND,
        TP.PRIORITY,
        TP.PL_AMT,
        '' EFS_FLAG,
        '' EFS_CPTY_SN,
        'N' ARCHIVE_FLAG,
              --CASE RQMT.TRADE_ID 
              --WHEN NULL THEN 'N'
              --ELSE 'Y'
              --END 
              'N' RPLY_RDY_TO_SND_FLAG,
              'G' MIGRATE_IND,
        'N/A' ANALYST_NAME,
              --          (SELECT V.NEXT_STATUS_CODE
--             FROM CONFIRMMGR.V_TRADE_RQMT_CONFIRM V
--            WHERE     FINAL_APPROVAL_FLAG = 'N'
--                  AND V.TRADE_ID = T.ID
--                  AND V.CONFIRM_LABEL IS NULL
--                  AND V.NEXT_STATUS_CODE = 'MGR')
--             AS ADDITIONAL_CONFIRM_SENT,
              NULL ADDITIONAL_CONFIRM_SENT,
              'N' AS IS_TEST_BOOK
              FROM
              CONFIRMMGR.TRADE T
              LEFT OUTER JOIN CONFIRMMGR.V_TRADE_RQMT_BKR VB
        ON VB.TRADE_ID=T.ID
              LEFT OUTER JOIN CONFIRMMGR.V_TRADE_RQMT_CPTY VC
        ON VC.TRADE_ID=T.ID
              LEFT OUTER JOIN CONFIRMMGR.V_TRADE_RQMT_SETC VS
        ON VS.TRADE_ID=T.ID
              LEFT OUTER JOIN CONFIRMMGR.V_TRADE_RQMT_NO_CONFIRM VX
        ON VX.TRADE_ID=T.ID
              LEFT OUTER JOIN CONFIRMMGR.V_TRADE_RQMT_VERBL VV
        ON VV.TRADE_ID=T.ID
              LEFT OUTER JOIN CONFIRMMGR.TRADE_PRIORITY TP
       ON TP.TRADE_ID=T.ID
--              LEFT OUTER JOIN CONFIRMMGR.TRADE_RQMT_CONFIRM RQMT
--       ON RQMT.TRADE_ID=T.ID
--       AND RQMT.CONFIRM_LABEL IS NULL
--        AND RQMT.NEXT_STATUS_CODE = 'MGR'
              LEFT OUTER JOIN CONFIRMMGR.TRADE_GROUP TG
       ON TG.TRADE_ID=T.ID,                         
              CONFIRMMGR.TRADE_SUMMARY TS,
        CONFIRMMGR.TRADE_DATA TD,
              (SELECT GETDATE() CURRENT_BUSN_DT, GETDATE() RIGHTNOW) BD
        WHERE  TS.TRADE_ID = T.ID
        AND TD.TRADE_ID = TS.TRADE_ID;




GO

GRANT SELECT TO stanford_developers;
GO


