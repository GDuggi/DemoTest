package aff.confirm.common.ottradealert;

import aff.confirm.common.dbqueue.QEFETTradeAlert;
import aff.confirm.common.econfirm.EConfirmData;
import aff.confirm.common.econfirm.EConfirmTradeInfo;
import aff.confirm.common.econfirm.datarec.EConfirmTradeInfo_DataRec;
import aff.confirm.common.efet.EFETProcessor;
import aff.confirm.common.efet.dao.EFET_DAO;
import aff.confirm.common.efet.datarec.EFETSubmitXML_DataRec;
import aff.confirm.common.util.MailUtils;
import org.jboss.logging.Logger;

import javax.jms.JMSException;
import java.sql.SQLException;
import java.text.DecimalFormat;
import java.text.ParseException;
import java.util.Calendar;

//import aff.confirm.common.dao.CdtyCodeDAO;
//import aff.confirm.common.econfirm_v1.EConfirmDAO;
//import aff.confirm.common.econfirm_v1.EConfirmAgreementDAO;

/**
 * User: ifrankel
 * Date: May 8, 2003
 * Time: 10:53:44 AM
 * Revision History:
 * 3/2/2004 IF - Improved IsEconfirm Test, basing it on opstracking AND econfirm_v1
 *               instead of just econfirm_v1.
 *
 */
public class OpsTrackingTradeAlert {
    private final String EC_SUBMIT = "SUBMIT";
    private final String EC_SUBMITTED = "SUBMITTED";
    private final String EC_CANCEL = "CANCEL";
    private final String EC_CANCELED = "CANCELED";
    private final String EC_DELETE = "DELETE";
    private final String EC_VOID = "VOID";
    private final String EC_EDIT = "EDIT";
    private final String EC_NONE = "NONE";
    private final String EC_CLICK = "CLICK";
    private final String EC_ACCEPT = "ACCPT";
    private final String EC_MATCHED = "MATCHED";
    private final String CPTY_PAPER = "XQCCP";
    private final String SEMPRA_PAPER = "XQCSP";
    private final String BROKER_PAPER = "XQBBP";
    private final String ECONF = "ECONF";
    private final String ECONFIRM_BROKER = "ECBKR";
    private final String EFET = "EFET";
    private final String EFET_BROKER = "EFBKR";
    private final String EFET_SUBMIT = "SUBMIT";
    private final String EFET_CANCEL = "CANCEL";
    //private final double BACKFILL_CUTOFF_ID = 21354700;
    private DecimalFormat df = new DecimalFormat("#0;-#0");
    private java.sql.Connection opsTrackingConnection;
    private java.sql.Connection affinityConnection;
    //private java.sql.Connection symphonyConnection;
    //private final String STATUS_OPEN = "OPEN";
    //private CdtyCodeDAO cdtyCodeDAO;
    private TradingSystemDATA_dao tsDATA_dao;
    private TradingSystemDATA_rec tsDATA_rec;
    private OpsTrackingTRADE_dao otTRADE_dao;
    private OpsTrackingTRADE_NOTIFY_dao otTRADE_NOTIFY_dao;
    private OpsTrackingTRADE_DATA_dao otTRADE_DATA_dao;
    private OpsTrackingTRADE_DATA_rec otTRADE_DATA_rec;
    private OpsTrackingTRADE_DATA_CHG_dao otTRADE_DATA_CHG_dao;
    private OpsTrackingTRADE_SUMMARY_dao otTRADE_SUMMARY_dao;
    private OpsTrackingTRADE_SUMMARY_rec otTRADE_SUMMARY_rec;
    private OpsTrackingTRADE_EXT_PROCESS_DATA_dao otTRADE_EXT_PROCESS_DATA_dao;
    private OpsTrackingTRADE_APPR_dao otTRADE_APPR_dao;
    private OpsTrackingIGNORED_NOTIFICATIONS_dao otIGNORED_NOTIFICATIONS_dao;
    private OpsTrackingArchive_dao otArchive_dao;
    //private EConfirmDAO eConfirmDAO;
    private EConfirmTradeInfo eConfirmTradeInfo;
    private EConfirmData eConfirmData;
    private EFET_DAO efetDAO;
    private OpsTrackingBasket_dao opsTrackingBasker_dao;
    private EFETProcessor efetProcessor;
    private QEFETTradeAlert qEFETTradeAlert;
    //private OpsTrackingRulesProc otRulesProc;
    private MailUtils mailUtils;
    private String ecFailedLogAddress;
    private String tradeDataWebServiceURL;
    private String tradeDataRootTagName;
    private String econfirmTradeInfoServiceUrl;
    public  OpsTrackingTRADE_RQMT_dao otTRADE_RQMT_dao;
    public  String systemsNotifyAddress;
    public  String sentFromAddress;
    public  String sentFromName;

    public OpsTrackingTradeAlert(java.sql.Connection pOpsTrackingConnection,
                                    java.sql.Connection pAffinityConnection,
//                                    java.sql.Connection pSymphonyConnection,
                                    String pTradeDataWebServiceURL,
                                    String pTradeDataRootTagName,
                                    MailUtils pMailUtils,
                                    String pECFailedLogAddress,
                                    String pEConfirmInfoServiceUrl ) throws Exception {
        this.opsTrackingConnection = pOpsTrackingConnection;
        this.affinityConnection = pAffinityConnection;
//        this.symphonyConnection = pSymphonyConnection;
        this.tradeDataWebServiceURL = pTradeDataWebServiceURL;
        this.tradeDataRootTagName = pTradeDataRootTagName;
        this.econfirmTradeInfoServiceUrl = pEConfirmInfoServiceUrl;
        this.mailUtils = pMailUtils;
        this.ecFailedLogAddress = pECFailedLogAddress;
        init();
    }

    private void init() throws SQLException, Exception {
        //cdtyCodeDAO = new CdtyCodeDAO(affinityConnection);
        tsDATA_dao = new TradingSystemDATA_dao(affinityConnection, /*symphonyConnection,*/ tradeDataWebServiceURL, tradeDataRootTagName);
        tsDATA_rec = new TradingSystemDATA_rec();
        otTRADE_dao = new OpsTrackingTRADE_dao(opsTrackingConnection);
        otTRADE_NOTIFY_dao = new OpsTrackingTRADE_NOTIFY_dao(opsTrackingConnection);
        otTRADE_DATA_dao = new OpsTrackingTRADE_DATA_dao(opsTrackingConnection);
        otTRADE_DATA_rec = new OpsTrackingTRADE_DATA_rec();
        otTRADE_DATA_CHG_dao = new OpsTrackingTRADE_DATA_CHG_dao(opsTrackingConnection);
        otTRADE_RQMT_dao = new OpsTrackingTRADE_RQMT_dao(
                opsTrackingConnection,affinityConnection, /*symphonyConnection,*/ tradeDataWebServiceURL,tradeDataRootTagName,
                mailUtils,ecFailedLogAddress, econfirmTradeInfoServiceUrl);
        otTRADE_SUMMARY_dao = new OpsTrackingTRADE_SUMMARY_dao(opsTrackingConnection);
        otTRADE_SUMMARY_rec = new OpsTrackingTRADE_SUMMARY_rec();
        otTRADE_EXT_PROCESS_DATA_dao = new OpsTrackingTRADE_EXT_PROCESS_DATA_dao(opsTrackingConnection);
        otTRADE_APPR_dao = new OpsTrackingTRADE_APPR_dao(opsTrackingConnection);
        otIGNORED_NOTIFICATIONS_dao = new OpsTrackingIGNORED_NOTIFICATIONS_dao(opsTrackingConnection);
        otArchive_dao = new OpsTrackingArchive_dao(opsTrackingConnection);
        //eConfirmDAO = new EConfirmDAO(opsTrackingConnection,affinityConnection, /*symphonyConnection,*/ mailUtils,ecFailedLogAddress);
        eConfirmData = new EConfirmData(opsTrackingConnection);
        //Israel 7/30/15 -- temporarily removed
        //eConfirmTradeInfo = new EConfirmTradeInfo(econfirmTradeInfoServiceUrl);
        efetDAO = new EFET_DAO(opsTrackingConnection,affinityConnection);
        efetProcessor = new EFETProcessor(affinityConnection,opsTrackingConnection,"","");
        qEFETTradeAlert = new QEFETTradeAlert(opsTrackingConnection);
        opsTrackingBasker_dao = new OpsTrackingBasket_dao(opsTrackingConnection);
        //otRulesProc = new OpsTrackingRulesProc(opsTrackingConnection);
    }

    /**
     * Inserts a new trade into ops_tracking:
     * trade
     * trade_notify
     * trade_data
     * ops_rqmt
     * ops_summary
     * @param pOpsTrackingTradeAlertDataRec
     */
    public OpsTrackingReturnDataRec processNewTrade(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec)
            throws JMSException, SQLException, ParseException, Exception {
        OpsTrackingReturnDataRec otReturnDataRec;
        otReturnDataRec = new OpsTrackingReturnDataRec();
        tsDATA_rec = tsDATA_dao.getTradingSystemDATA_rec(pOpsTrackingTradeAlertDataRec.tradingSystem,
                                 pOpsTrackingTradeAlertDataRec.tradeID,
                                 pOpsTrackingTradeAlertDataRec.tradeTypeCode);

        otTRADE_DATA_rec = otTRADE_DATA_dao.getOpsTrackingTRADE_DATA_rec(pOpsTrackingTradeAlertDataRec.tradeID);

        //Finalapproval and OpsDetActionsFlag set here
        otReturnDataRec = otTRADE_RQMT_dao.determineRqmts(pOpsTrackingTradeAlertDataRec,tsDATA_rec);

        otTRADE_dao.insertTrade(pOpsTrackingTradeAlertDataRec);
        int tradeNotifyID = -1;
        tradeNotifyID = otTRADE_NOTIFY_dao.insertTradeNotify(pOpsTrackingTradeAlertDataRec);

        pOpsTrackingTradeAlertDataRec.tradeNotifyID = tradeNotifyID;

        int tradeDataID = -1;
        tradeDataID = otTRADE_DATA_dao.insertTradeDataByTrdSys(tsDATA_rec);

        //confirmStatusInd and rqmtId set here if there is a Sempra Paper requirement
        //Requirements set in this routine that were setup by determineRqmts above.
        // MThoresen - 4-19-2007: added parameter to call for click and confirm
        otTRADE_RQMT_dao.insertTradeRqmts(pOpsTrackingTradeAlertDataRec, tsDATA_rec, false);

        int tradeSummaryID = -1;
        //pOpsTrackingTradeAlertDataRec.rqmtRuleID = otRulesProc.GetRqmtRule(tsDATA_rec);
        //if (tsDATA_rec.BROKER_SN != null && tsDATA_rec.BROKER_SN.length() > 0)
        //    pOpsTrackingTradeAlertDataRec.rqmtBrokerExcludeRuleID = otRulesProc.GetRqmtBrokerExcludeRule(tsDATA_rec);

        tradeSummaryID = otTRADE_SUMMARY_dao.execInsertTradeSummary(pOpsTrackingTradeAlertDataRec);

        if (otReturnDataRec.ecProductID > 0) {
            String extProcCode = "ECONF";
            String attribName = "EC_PRODUCT_ID";
            String attribValue = String.valueOf(otReturnDataRec.ecProductID);
            otTRADE_EXT_PROCESS_DATA_dao.insertTradeExtProcessData(pOpsTrackingTradeAlertDataRec.tradeID,
                    extProcCode, attribName, attribValue);
        }

        //If we're submitting a BFI document then insert a row into the broker_fee table.
        // This table is used on edits to see if fee data has changed.
        if (!otReturnDataRec.efetBkrAction.equalsIgnoreCase(EC_NONE)){
            EFETSubmitXML_DataRec efetSubmitXMLDataRec;
            efetSubmitXMLDataRec = new EFETSubmitXML_DataRec();
            efetSubmitXMLDataRec = efetProcessor.getEfetSubmitXMLDataRec(pOpsTrackingTradeAlertDataRec.tradeID);
            efetDAO.insertBrokerFee(pOpsTrackingTradeAlertDataRec.tradeID, efetSubmitXMLDataRec.bkrFeeTotal,
                    efetSubmitXMLDataRec.bkrFeeCcy);
        }

        return otReturnDataRec;
    }

    public OpsTrackingReturnDataRec processEditedTrade(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec)
            throws JMSException, SQLException, ParseException, Exception {
        String tradingSystem = null;
        double tradeID = -1;
        tradingSystem = pOpsTrackingTradeAlertDataRec.tradingSystem;
        tradeID = pOpsTrackingTradeAlertDataRec.tradeID;
        String ticketId = df.format(tradeID);
        int tradeNotifyID = -1;

        tsDATA_rec = tsDATA_dao.getTradingSystemDATA_rec(tradingSystem,tradeID,pOpsTrackingTradeAlertDataRec.tradeTypeCode);
        otTRADE_DATA_rec = otTRADE_DATA_dao.getOpsTrackingTRADE_DATA_rec(pOpsTrackingTradeAlertDataRec.tradeID);
        otTRADE_SUMMARY_rec = otTRADE_SUMMARY_dao.getOpsTrackingTRADE_SUMMARY_rec(pOpsTrackingTradeAlertDataRec.tradeID);

        /*If the tradeId is 0 it means no record was found.
          Only do backfill if it was older than the first prmnt_trade_id from Oct 1, 2003.
          If it is newer, then we are missing a version 1 record that should be available.
          In this case only we do a backfill. */
        boolean backfillMode = false;
        boolean existsInIgnoredNotify = false;
        if (otTRADE_DATA_rec.TRADE_ID == 0) {  // data not in trade_data table...now check the archive tables...
            int foundItInArchive = -1;
            //1=found it and restored it 0=trade not found in archive
            foundItInArchive = otArchive_dao.restoreTradeData(tradingSystem, tradeID);
            if (foundItInArchive == 1) {
                otTRADE_DATA_rec = otTRADE_DATA_dao.getOpsTrackingTRADE_DATA_rec(pOpsTrackingTradeAlertDataRec.tradeID);
                otTRADE_SUMMARY_rec = otTRADE_SUMMARY_dao.getOpsTrackingTRADE_SUMMARY_rec(pOpsTrackingTradeAlertDataRec.tradeID);
            } else {
                // 9-24-08: MThoresen: Added existsInIgnoredNotify - HOUSE trades being edited to non HOUSE trades are
                // not flowing through the system.  This check will force it to go through if version 1 exists in
                // ignored notification
                existsInIgnoredNotify = otIGNORED_NOTIFICATIONS_dao.existsInIgnoredNotify(tradeID);
                // not found in the trade_data table, but we now need to process this  as a new trade because it is in the ignored notification table
                if(existsInIgnoredNotify){
                    Logger.getLogger(OpsTrackingTradeAlert.class).info("An Edited Ignored Trade is now being Processed as NEW: " +
                            tradingSystem + " " + ticketId );

                    return processNewTrade(pOpsTrackingTradeAlertDataRec);
                }
//                if ((tradeID < BACKFILL_CUTOFF_ID) || existsInIgnoredNotify) {
                if (existsInIgnoredNotify) {
                    backfillMode = true;
                } else
                    throw new Exception("Record out of sequence: Ticket=" + ticketId);
            }
        }

        // if OLD.detact='N' then det_act=getDetermineActions
        // else NEW.detact_flag=old.determine_act_flag (Y)
        String oldDetActFlag = otTRADE_SUMMARY_rec.OPS_DET_ACT_FLAG;
        String newDetActFlag = "";
        if (oldDetActFlag.equalsIgnoreCase("N")) {
            if ( !isTodayTrade(tsDATA_rec.TRADE_DT) ){
                newDetActFlag = getDetermineActionsFlag(pOpsTrackingTradeAlertDataRec);
            }
        }
        else
            newDetActFlag = oldDetActFlag;  //=Y
        pOpsTrackingTradeAlertDataRec.opsDetActionsFlag = newDetActFlag;

        // if Old.final_approval='Y' and new.determine_actions='Y' then new.final_approve='N'
        //else new.final_approval_flag=old.final_approval_flag
        String oldFinalApprovalFlag = otTRADE_SUMMARY_rec.FINAL_APPROVAL_FLAG;
        String newFinalApprovalFlag = "";
        //5/28/09 Israel - If Reopening approved trade, insert row in TradeAppr
        if (oldFinalApprovalFlag.equalsIgnoreCase("Y") &&
            newDetActFlag.equalsIgnoreCase("Y"))
        {
            newFinalApprovalFlag = "N";
            otTRADE_APPR_dao.insertTradeApprover(pOpsTrackingTradeAlertDataRec.tradeID,"N","TRADE_DATA_CHANGE");
        }
        else
            newFinalApprovalFlag = oldFinalApprovalFlag;
        pOpsTrackingTradeAlertDataRec.finalApprovalFlag = newFinalApprovalFlag;

        pOpsTrackingTradeAlertDataRec.openRqtsFlag = "";

        if (backfillMode){
            Logger.getLogger(OpsTrackingTradeAlert.class).info("Backfill Trade Records: " +
                    tradingSystem + " " + ticketId + " ...");
            //5/21/2004 IF -- Sometimes the Trade record is the only one out there for a backfill trade.
            if (!otTRADE_dao.isTradeExist(pOpsTrackingTradeAlertDataRec.tradeID))
                otTRADE_dao.insertTrade(pOpsTrackingTradeAlertDataRec);
        }

        tradeNotifyID = otTRADE_NOTIFY_dao.insertTradeNotify(pOpsTrackingTradeAlertDataRec);
        pOpsTrackingTradeAlertDataRec.tradeNotifyID = tradeNotifyID;
        boolean editReconcileDone = false;
        
        //7/7/09 Israel -- Added ACCPT test and else if  to speed things up
        if (!backfillMode)
        {
            otTRADE_DATA_dao.updateTradeData(tsDATA_rec,pOpsTrackingTradeAlertDataRec.auditTypeCode);
            
            //7/6/09 Israel-- insert trade_data_chg row as necessary
            if (!pOpsTrackingTradeAlertDataRec.auditTypeCode.equalsIgnoreCase(EC_ACCEPT))
                otTRADE_DATA_CHG_dao.insertTradeDataChgRows(otTRADE_DATA_rec, tsDATA_rec, pOpsTrackingTradeAlertDataRec.auditTypeCode,
                                                            pOpsTrackingTradeAlertDataRec.empName);
            
            //7/17/09 Israel -- Cancel Requirements for voided trade.
            if (pOpsTrackingTradeAlertDataRec.auditTypeCode.equalsIgnoreCase("VOID"))
                otTRADE_RQMT_dao.CancelAllRqmts(otTRADE_DATA_rec.TRADE_ID,"Trade was voided.");

            if (isBrokerRemoved(otTRADE_DATA_rec, tsDATA_rec))
                otTRADE_RQMT_dao.CancelRqmt(otTRADE_DATA_rec.TRADE_ID,"XQBBP","Broker removed from Trade.", true);

            if (isCptyChanged(otTRADE_DATA_rec, tsDATA_rec) ||
                isBuySellChanged(otTRADE_DATA_rec, tsDATA_rec))
            {
                //If not econfirm_v1/efet it is handled there...
                //String isOneDayGasDeal = otTRADE_RQMT_dao.is1DayGas(tsDATA_rec);
                //int eConfirmProductID = eConfirmDAO.getEConfirmSubmitProductID(pOpsTrackingTradeAlertDataRec.tradingSystem,
                //pOpsTrackingTradeAlertDataRec.tradeID, tsDATA_rec.SE_CPTY_SN, tsDATA_rec.CPTY_SN, tsDATA_rec.STTL_TYPE,
                //tsDATA_rec.CDTY_CODE, tsDATA_rec.TRADE_DT, tsDATA_rec.EFS_FLAG, isOneDayGasDeal, tsDATA_rec.BROKER_SN, EConfirmAgreementDAO.CPTY_TYPE);

                //Israel 8/20/15 -- Added to force wasn't EFET/Is not EFET to get processed farther down below.
                boolean efetCptySubmit = efetDAO.isEFETCptySubmit(pOpsTrackingTradeAlertDataRec.tradingSystem,
                        pOpsTrackingTradeAlertDataRec.tradeID);

                //Israel 7/30/15 -- temporarily removed
                //int eConfirmProductID = eConfirmTradeInfo.getEConfirmProductId(pOpsTrackingTradeAlertDataRec.tradingSystem,
                //                          pOpsTrackingTradeAlertDataRec.tradeID);
                int eConfirmProductID  = 0;

                if (!otTRADE_RQMT_dao.IsAutoConfirm(otTRADE_DATA_rec.TRADE_ID) &&
                        eConfirmProductID == 0 && !efetCptySubmit)
                {
                    otTRADE_RQMT_dao.determineRqmts(pOpsTrackingTradeAlertDataRec, tsDATA_rec);
                    otTRADE_RQMT_dao.ReconcileRqmtLists(otTRADE_DATA_rec.TRADE_ID);
                    otTRADE_RQMT_dao.insertTradeRqmts(pOpsTrackingTradeAlertDataRec, tsDATA_rec, false);
                    editReconcileDone = true;
                }
            }

            //11/10/2009 (Happy B-Day Marines!) Israel - Resets Cpty and Sempra paper requirements when specified cols change. 
            if (pOpsTrackingTradeAlertDataRec.auditTypeCode.equalsIgnoreCase(EC_EDIT) &&  !isTodayTrade(tsDATA_rec.TRADE_DT )) {
                boolean hasResetRqmts = false;
                hasResetRqmts = otTRADE_DATA_CHG_dao.hasRqmtResetColChanged(otTRADE_DATA_rec, tsDATA_rec);
                if (hasResetRqmts) {
                    //1/11/2010 Israel -- Changed spec, now setting determineActions flag
                    //TestAndResetRqmt(tsDATA_rec.TRADE_ID, CPTY_PAPER);
                    //TestAndResetRqmt(tsDATA_rec.TRADE_ID, SEMPRA_PAPER);
                    if (otTRADE_RQMT_dao.HasRqmt(tsDATA_rec.TRADE_ID, CPTY_PAPER) ||
                        otTRADE_RQMT_dao.HasRqmt(tsDATA_rec.TRADE_ID, SEMPRA_PAPER))  
                    {
                        //Just update ready for final approval. The others get updated in following code.
                        otTRADE_SUMMARY_dao.updateEditedSentTrade(tsDATA_rec.TRADE_ID,"N");
                        pOpsTrackingTradeAlertDataRec.finalApprovalFlag = "N";
                        pOpsTrackingTradeAlertDataRec.opsDetActionsFlag = "E";
                       otTRADE_SUMMARY_dao.updateTradeSummary(pOpsTrackingTradeAlertDataRec);
                        if (oldFinalApprovalFlag.equalsIgnoreCase("Y"))
                            otTRADE_APPR_dao.insertTradeApprover(pOpsTrackingTradeAlertDataRec.tradeID,"N","SENT_TRADE_EDIT");
                    }
                    else {
                        pOpsTrackingTradeAlertDataRec.opsDetActionsFlag = "Y";    
                    }
                }
            }

        }
        else if (backfillMode){
            int tradeDataID = -1;
            tradeDataID = otTRADE_DATA_dao.insertTradeDataByTrdSys(tsDATA_rec);
            Logger.getLogger(OpsTrackingTradeAlert.class).info("Backfill Trade Records: " +
                    tradingSystem + " " + ticketId + " OK.");
        }

        //7/7/09 Israel -- Added ACCPT test and else if to speed things up
        if (!backfillMode)
            otTRADE_SUMMARY_dao.updateTradeSummary(pOpsTrackingTradeAlertDataRec);
        else {
            pOpsTrackingTradeAlertDataRec.opsDetActionsFlag = "Y";
            pOpsTrackingTradeAlertDataRec.finalApprovalFlag = "N";
            otTRADE_SUMMARY_dao.execInsertTradeSummary(pOpsTrackingTradeAlertDataRec);
        }

        //Process edits that occur on same day trade was edited.
        /*if (pOpsTrackingTradeAlertDataRec.tradeDt.compareTo(pOpsTrackingTradeAlertDataRec.updateBusnDt) == 0){
            int eConfirmProductID = -1;
            eConfirmProductID = otTRADE_RQMT_dao.determineRqmts(pOpsTrackingTradeAlertDataRec,tsDATA_rec);
            otTRADE_RQMT_dao.insertTradeRqmts(pOpsTrackingTradeAlertDataRec, true);
        }*/

        //Test for econfirm_v1 cancel, submit, ignore
        OpsTrackingReturnDataRec otReturnDataRec;
        otReturnDataRec = new OpsTrackingReturnDataRec();

        String ecStatus = "";
        //ecStatus = eConfirmDAO.getECTradeSummaryStatus(pOpsTrackingTradeAlertDataRec.tradeID);
        ecStatus = eConfirmData.getECTradeSummaryStatus(pOpsTrackingTradeAlertDataRec.tradingSystem, pOpsTrackingTradeAlertDataRec.tradeID);

        /**
         * This could be slightly more efficient by conditionally calling this routine,
         * but then either the ec_status or isEConfirmPrep would have to be set with literals
         * to achieve the desired results, and in either case they would not always contain accurate data.
         */
        boolean isEConfirmPrep = false;
        isEConfirmPrep = otTRADE_RQMT_dao.isEConfPrep(pOpsTrackingTradeAlertDataRec.tradeID);
        /**
         * Determine whether we need to submit or cancel only if it
         * is not a backfill or a trade ACCPT
         * 1/9/2004 IF -- If it is already matched, leave it alone. Set the action
         * matched and send it back.
         * 3/3/2004 IF -- There is a gap between the time it is created (PREP) and we submit
         * it to econfirm_v1 (SENT). As soon as it is submitted an EC_TRADE_SUMMARY record is
         * created and it has a status (anything but NONE). The isEConfirmPrep flag is used
         * to close that gap.
         */
        if (isEConfirmPrep && ecStatus.equalsIgnoreCase(EC_NONE))
           otReturnDataRec.ecAction = EC_NONE;
        else if (!backfillMode && !pOpsTrackingTradeAlertDataRec.auditTypeCode.equalsIgnoreCase(EC_ACCEPT) && !ecStatus.equalsIgnoreCase(EC_MATCHED)){
            otReturnDataRec = getEConfirmEditAction(pOpsTrackingTradeAlertDataRec, ecStatus, tsDATA_rec,EConfirmData.CPTY_TYPE);
            // IF ITS CLICK AND CONFIRM, AND EC-PRODUCTID > 0, AND THERE ARE NO VRBL OR BROKER RQMTS...ADD THE C&C VERBAL..
            if ((pOpsTrackingTradeAlertDataRec.isClickAndConfirm) && (otReturnDataRec.ecProductID > 0) &&
                (!otTRADE_RQMT_dao.hasClickAndConfirmVerbalRqmt(pOpsTrackingTradeAlertDataRec.tradeID)) &&
                (!otTRADE_RQMT_dao.hasBrokerRqmt(pOpsTrackingTradeAlertDataRec.tradeID))){
                otTRADE_RQMT_dao.insertTradeRqmt(pOpsTrackingTradeAlertDataRec, "VBCP", EConfirmData.EC_CLICK_AND_CONF_REF, "N", 0);
            }else if((!pOpsTrackingTradeAlertDataRec.isClickAndConfirm) && (otTRADE_RQMT_dao.hasClickAndConfirmVerbalRqmt(pOpsTrackingTradeAlertDataRec.tradeID))){ // MAKE SURE THERE IS NO VRBL C&C RQMT...
                otTRADE_RQMT_dao.cancelRqmt(pOpsTrackingTradeAlertDataRec.tradeID,"VBCP", "1 DAY GAS, OR ICE");
            }
        }
        else if (ecStatus.equalsIgnoreCase(EC_MATCHED))
            otReturnDataRec.ecAction = EC_MATCHED;

//////////////////////////////////////////////
         // Samy : 06/01/2009    eConfirm Broker Enhancements
        int ecCptyProductId = otReturnDataRec.ecProductID; 
        String ecBkrStatus = "";
        //ecBkrStatus = eConfirmDAO.getECBkrTradeSummaryStatus(pOpsTrackingTradeAlertDataRec.tradeID);
        ecBkrStatus = eConfirmData.getECBkrTradeSummaryStatus(pOpsTrackingTradeAlertDataRec.tradingSystem, pOpsTrackingTradeAlertDataRec.tradeID);

        /**
         * This could be slightly more efficient by conditionally calling this routine,
         * but then either the ec_status or isEConfirmPrep would have to be set with literals
         * to achieve the desired results, and in either case they would not always contain accurate data.
         */
        boolean isBkrEConfirmPrep = false;
        isBkrEConfirmPrep = otTRADE_RQMT_dao.isBkrEConfPrep(pOpsTrackingTradeAlertDataRec.tradeID);
        /**
         * Determine whether we need to submit or cancel only if it
         * is not a backfill or a trade ACCPT
         * 1/9/2004 IF -- If it is already matched, leave it alone. Set the action
         * matched and send it back.
         * 3/3/2004 IF -- There is a gap between the time it is created (PREP) and we submit
         * it to econfirm_v1 (SENT). As soon as it is submitted an EC_TRADE_SUMMARY record is
         * created and it has a status (anything but NONE). The isEConfirmPrep flag is used
         * to close that gap.
         */

        if (isBkrEConfirmPrep && ecBkrStatus.equalsIgnoreCase(EC_NONE))
             otReturnDataRec.ecBkrAction = EC_NONE;
        else if (!backfillMode && !pOpsTrackingTradeAlertDataRec.auditTypeCode.equalsIgnoreCase(EC_ACCEPT) && !ecBkrStatus.equalsIgnoreCase(EC_MATCHED)){
            OpsTrackingReturnDataRec otBkrReturnDataRec = getEConfirmBkrEditAction(pOpsTrackingTradeAlertDataRec, ecBkrStatus, tsDATA_rec,EConfirmData.BROKER_TYPE);
            otReturnDataRec.ecBkrAction =      otBkrReturnDataRec.ecBkrAction;
            otReturnDataRec.ecProductID = otBkrReturnDataRec.ecProductID;
            // add the broker requirment if the broker is changed from eConfirm broker to non eConfirm broker
            if (!isBrokerRemoved(otTRADE_DATA_rec, tsDATA_rec) && EC_CANCELED.equalsIgnoreCase(otReturnDataRec.ecBkrAction)){
                // insert broker requirement only if the cpty is not changed
                // if the cpty is changed the broker change  is also taken care.
                  if (!editReconcileDone){
                      String secondCheck = otTRADE_RQMT_dao.isSecondCheck(tsDATA_rec,"XQBBP","XQBBP");
//                      int rqmt2ndChkRuleId = otRulesProc.GetRqmt2ndChk("XQBBP", tsDATA_rec);
                      int rqmt2ndChkRuleId = 0;
                      otTRADE_RQMT_dao.insertTradeRqmt(pOpsTrackingTradeAlertDataRec, "XQBBP", "INSERTED FROM EDIT", secondCheck, rqmt2ndChkRuleId);
                  }
            }
        }
        else if (ecStatus.equalsIgnoreCase(EC_MATCHED))
            otReturnDataRec.ecBkrAction = EC_MATCHED;

        // samy: 07-09-09 to keep the cpty product id;
        if (ecCptyProductId > 0) { // if the cpty is econfirm_v1, then bring that.
            otReturnDataRec.ecProductID = ecCptyProductId;
        }
        //Test efet status
        //boolean isEFETPrep = false;
        //isEFETPrep = otTRADE_RQMT_dao.isEFETPrep(pOpsTrackingTradeAlertDataRec.tradeID);
        String efetCptyStatus = "";
        efetCptyStatus = efetDAO.getEfetTradeSummaryStatus(pOpsTrackingTradeAlertDataRec.tradeID,"C");
        boolean isEFETCptyQueued = false;
        isEFETCptyQueued = qEFETTradeAlert.isEFETQueued(pOpsTrackingTradeAlertDataRec.tradeID,EFET_SUBMIT,EFET_DAO.CNF,"C");

        String[] efetCptyResult = new String[4];
        efetCptyResult[0] = EC_NONE;
        efetCptyResult[1] = EC_NONE;
        efetCptyResult[2] = EC_NONE;
        efetCptyResult[3] = "N"; 

        //This test is meaningless in a world of delayed batch submissions.
        //if (isEFETPrep && efetStatus.equalsIgnoreCase(EC_NONE))
            //efetResult[0] = EC_NONE;
        //else if (!backfillMode &&
        /*if (pOpsTrackingTradeAlertDataRec.auditTypeCode.equalsIgnoreCase(EC_VOID) && isEFETQueued){
            qEFETTradeAlert.updateAlertRecordByTradeId(pOpsTrackingTradeAlertDataRec.tradeID,"Y");
            efetResult[0] = EC_NONE;
        }*/

        if (!backfillMode &&
            !pOpsTrackingTradeAlertDataRec.auditTypeCode.equalsIgnoreCase(EC_ACCEPT) &&
            !efetCptyStatus.equalsIgnoreCase(EC_MATCHED)) {
                if (!tsDATA_rec.BOOK.equalsIgnoreCase("Z") && !tsDATA_rec.BOOK.equalsIgnoreCase("ZZ") &&
                    !tsDATA_rec.BOOK.equalsIgnoreCase("TEST SG NG") &&  !tsDATA_rec.BOOK.equalsIgnoreCase("TEST SG ELEC")) {
                    efetCptyResult = getEFETCptyEditAction(pOpsTrackingTradeAlertDataRec, efetCptyStatus, isEFETCptyQueued);
                }
        }
        else if (efetCptyStatus.equalsIgnoreCase(EC_MATCHED))
            efetCptyResult[0] = EC_MATCHED;

        otReturnDataRec.efetCptyAction = efetCptyResult[0];
        otReturnDataRec.efetCptySubmitState = efetCptyResult[1];
        //If it was a cpty efet deal and now it's not, we submit an edit for the CNF
        //but change the receiver from C to B.
        if (efetCptyResult[2].equalsIgnoreCase("B"))
            otReturnDataRec.efetCnfReceiver = "B";
        else if (!otReturnDataRec.efetCptyAction.equalsIgnoreCase(EC_NONE))
                otReturnDataRec.efetCnfReceiver = "C";

        //IF 7/5/2006 - EFET Broker matching
        String efetBkrStatus = "";
        efetBkrStatus = efetDAO.getEfetTradeSummaryStatus(pOpsTrackingTradeAlertDataRec.tradeID,"B");
        //boolean isEFETBkrCNFQueued = false;
        boolean isEFETBkrBFIQueued = false;
        isEFETBkrBFIQueued = qEFETTradeAlert.isEFETQueued(pOpsTrackingTradeAlertDataRec.tradeID,EFET_SUBMIT,EFET_DAO.BFI,"B");

        String[] efetBkrResult = new String[2];
        efetBkrResult[0] = EC_NONE;
        efetBkrResult[1] = EC_NONE;

        if (!backfillMode &&
            !pOpsTrackingTradeAlertDataRec.auditTypeCode.equalsIgnoreCase(EC_ACCEPT) &&
            !efetBkrStatus.equalsIgnoreCase(EC_MATCHED)) {
            if (!tsDATA_rec.BOOK.equalsIgnoreCase("Z") && !tsDATA_rec.BOOK.equalsIgnoreCase("ZZ") &&
                                !tsDATA_rec.BOOK.equalsIgnoreCase("TEST SG NG") &&  !tsDATA_rec.BOOK.equalsIgnoreCase("TEST SG ELEC")) {
                efetBkrResult = getEFETBkrEditAction(pOpsTrackingTradeAlertDataRec, efetBkrStatus, isEFETBkrBFIQueued, tsDATA_rec.BROKER_SN,efetCptyResult[3]);
            }
        }
        else if (efetBkrStatus.equalsIgnoreCase(EC_MATCHED))
            efetBkrResult[0] = EC_MATCHED;

        otReturnDataRec.efetBkrAction = efetBkrResult[0];
        otReturnDataRec.efetBkrSubmitState = efetBkrResult[1];
        if (!otReturnDataRec.efetBkrAction.equalsIgnoreCase(EC_NONE) &&
            !otReturnDataRec.efetCnfReceiver.equalsIgnoreCase("C"))
           otReturnDataRec.efetCnfReceiver = "B";

        // if it is a efet cpty and non efet broker  now
        // undo the cancelling on efet broker document bcos
        // if efet cpty doc is submitted that wil cancel the prev efet broker submission
        if ( !otReturnDataRec.efetCptyAction.equalsIgnoreCase(EC_NONE) &&
             otReturnDataRec.efetBkrAction.equalsIgnoreCase(EFET_CANCEL)) {
             otReturnDataRec.efetBkrAction = EC_NONE;
        }
        boolean isEfetBkr = false;
        //It currently is an efet broker deal (before) and was or will still be one (on completion of this trx)
        isEfetBkr = (efetDAO.isEFETBkrSubmit(pOpsTrackingTradeAlertDataRec.tradingSystem, pOpsTrackingTradeAlertDataRec.tradeID)) &&
                    (efetDAO.isTradeRqmtExist(pOpsTrackingTradeAlertDataRec.tradeID, EFET_BROKER) ||
                     otReturnDataRec.efetBkrAction.equalsIgnoreCase("SUBMIT"));

        //Handles a trade edit where only the CNF needs to be resubmitted for an EFET broker
        if (!efetDAO.isTradeRqmtExist(pOpsTrackingTradeAlertDataRec.tradeID, "EFET")&&
             otReturnDataRec.efetCptyAction.equalsIgnoreCase(EC_NONE) &&
             isEfetBkr){
            // cancel the trade in the EFET box
            if (pOpsTrackingTradeAlertDataRec.auditTypeCode.equalsIgnoreCase(EC_VOID) ){
                otReturnDataRec.efetCptyAction = "CANCEL";
            }
            else {
                 otReturnDataRec.efetCptyAction = "SUBMIT";

            }
            //This must be new when a new BFI is submitted but edit otherwise.
            if (otReturnDataRec.efetBkrSubmitState.equalsIgnoreCase("NEW"))
                otReturnDataRec.efetCptySubmitState = "NEW";
            else
                otReturnDataRec.efetCptySubmitState = "EDIT";
            otReturnDataRec.efetCnfReceiver = "B";
        }
        else {  //  not a efet broker now
            // paper  cpty, previously efet broker, now it is not efet broker
             // so submit cancel to the document.
             if ( otReturnDataRec.efetCptyAction.equalsIgnoreCase(EC_NONE) &&  otReturnDataRec.efetBkrAction.equalsIgnoreCase(EFET_CANCEL) ) {
                    otReturnDataRec.efetCptyAction = "CANCEL";
                    otReturnDataRec.efetBkrAction = "NONE";
             }

        }

        return otReturnDataRec;
    }

    private void TestAndResetRqmt(double pTradeId,  String pRqmtCode) throws SQLException {
        String initialStatus;
        String cmt;
        java.util.Date today = new java.util.Date();
        java.sql.Date completedDate = new java.sql.Date(today.getTime());

        if (otTRADE_RQMT_dao.HasRqmt(pTradeId, pRqmtCode)){
            initialStatus = otTRADE_RQMT_dao.getInitialStatus(pRqmtCode);
            cmt = "Status was reset to " + initialStatus + " due to trade edit.";
            otTRADE_RQMT_dao.updateRqmts(pTradeId,pRqmtCode,initialStatus,completedDate,"",cmt);
        }
    }

    private boolean isBrokerRemoved(OpsTrackingTRADE_DATA_rec otTRADE_DATA_rec, TradingSystemDATA_rec tsDATA_rec)
    {   boolean isRemoved = false;
        String oldBroker = "";
        String newBroker = "";
        if (otTRADE_DATA_rec.BROKER_SN != null)
            oldBroker = otTRADE_DATA_rec.BROKER_SN;
        if (tsDATA_rec.BROKER_SN != null)
            newBroker = tsDATA_rec.BROKER_SN;
        if (oldBroker.length() > 0 && newBroker.length() == 0)
            isRemoved = true;
        return isRemoved;
    }

    private boolean isCptyChanged(OpsTrackingTRADE_DATA_rec otTRADE_DATA_rec, TradingSystemDATA_rec tsDATA_rec)
    {   boolean isChanged = false;
        String oldCpty = "NONE";
        String newCpty = "NONE";
        if (otTRADE_DATA_rec.CPTY_SN != null)
            oldCpty = otTRADE_DATA_rec.CPTY_SN;
        if (tsDATA_rec.CPTY_SN != null)
            newCpty = tsDATA_rec.CPTY_SN;
        if (!oldCpty.equalsIgnoreCase(newCpty))
            isChanged = true;
        return isChanged;
    }

    private boolean isBuySellChanged(OpsTrackingTRADE_DATA_rec otTRADE_DATA_rec, TradingSystemDATA_rec tsDATA_rec)
    {   boolean isChanged = false;
        String oldBuySell = "NONE";
        String newBuySell = "NONE";
        oldBuySell = otTRADE_DATA_rec.getBUY_SELL_IND();
        newBuySell = tsDATA_rec.getBUY_SELL_IND();
        if (!oldBuySell.equalsIgnoreCase(newBuySell))
            isChanged = true;
        return isChanged;
    }
    private String[] getEFETBkrEditAction(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec,
                                       String pEfetBkrStatus, boolean isEFETBkrBFIQueued, String pBrokerSn, String documentIdChangeFlag)
            throws Exception {
        String[] efetBkrResult = new String[2];
        String efetBkrAction = EC_NONE;
        String efetBkrSubmitState = EC_NONE;

        boolean isEfetBkrTrade = false;
        // exclude exchange trade from submitting to the box
        if   ( pOpsTrackingTradeAlertDataRec.cptySn.equalsIgnoreCase("NEWEDGE") ) {
              isEfetBkrTrade = false;
        }
       else {
             isEfetBkrTrade = efetDAO.isEFETBkrSubmit(pOpsTrackingTradeAlertDataRec.tradingSystem,
                                                    pOpsTrackingTradeAlertDataRec.tradeID);
      }

        /** If ec_Summary status = none, it means no row was found
         * in the ec_summary table, which means it wasn't econfirm_v1.
         */
        boolean isEFETBkrNow = false;
        //if (!pEfetBkrStatus.equalsIgnoreCase(EC_NONE) &&
        if (efetDAO.isTradeRqmtExist(pOpsTrackingTradeAlertDataRec.tradeID,EFET_BROKER))
            isEFETBkrNow = true;

        //Was it EFET before, i.e., already submitted?
        if (isEFETBkrNow) {
            //If yes and void, then cancel if not already canceled
            if (pOpsTrackingTradeAlertDataRec.auditTypeCode.equalsIgnoreCase(EC_VOID)){
                if (!pEfetBkrStatus.equalsIgnoreCase(EC_CANCELED)){
                    //efetBkrAction = EFET_CANCEL;
                    //According to Ponton we don't cancel a BFI document-- it just fades away.
                    efetBkrAction = "NONE";
                    efetBkrSubmitState = "NONE";
                }
                //Get rid of any pending trades in the queue, whether it's been submitted or not, for both BFI & CNF.
                //if (isEFETBkrBFIQueued){
               qEFETTradeAlert.updateAlertRecordByTradeId(pOpsTrackingTradeAlertDataRec.tradeID,"Y",EFET_DAO.BFI,"B");
               qEFETTradeAlert.updateAlertRecordByTradeId(pOpsTrackingTradeAlertDataRec.tradeID,"Y",EFET_DAO.CNF,"B");
            }
            //If not then is it still Efet? If so, see if the fee data has changed. If so then submit, otherwise cancel
            else if (isEfetBkrTrade){
                    EFETSubmitXML_DataRec efetSubmitXMLDataRec = new EFETSubmitXML_DataRec();
                    efetSubmitXMLDataRec = efetProcessor.getEfetSubmitXMLDataRec(pOpsTrackingTradeAlertDataRec.tradeID);
                    if (efetDAO.isBrokerFeeChanged(pOpsTrackingTradeAlertDataRec.tradeID,
                            efetSubmitXMLDataRec.bkrFeeTotal,efetSubmitXMLDataRec.bkrFeeCcy)
                            || ("Y".equalsIgnoreCase(documentIdChangeFlag) )  // samy 4-27-09 : to resubmit if document id changed
                            ){
                        if (!isEFETBkrBFIQueued){
                            efetBkrAction = EFET_SUBMIT;
                            efetBkrSubmitState = "EDIT";
                        }
                        efetDAO.updateBrokerFee(pOpsTrackingTradeAlertDataRec.tradeID,
                            efetSubmitXMLDataRec.bkrFeeTotal,efetSubmitXMLDataRec.bkrFeeCcy);
                    }
            }
            //If no longer efet then cancel
            else if (!isEfetBkrTrade){
                efetBkrAction = "NONE";

                boolean isEFETCancelQueued;
                isEFETCancelQueued = qEFETTradeAlert.isEFETQueued(pOpsTrackingTradeAlertDataRec.tradeID,EFET_CANCEL,EFET_DAO.BFI,"B");
                if (!isEFETCancelQueued ){
                    efetBkrAction = EFET_CANCEL;
                }
                efetBkrSubmitState = "NONE";
                qEFETTradeAlert.updateAlertRecordByTradeId(pOpsTrackingTradeAlertDataRec.tradeID,"Y",EFET_DAO.BFI,"B");
                qEFETTradeAlert.updateAlertRecordByTradeId(pOpsTrackingTradeAlertDataRec.tradeID,"Y",EFET_DAO.CNF,"B");
                updateTradeRqmt(pOpsTrackingTradeAlertDataRec, "XQBBP",pBrokerSn);
            }
          //Is it efet now?
          // Also includes efet trades queued but not yet sent.
        } else if (isEfetBkrTrade){
            //If it is efet and it isn't void then submit it
            //Only submit if there isn't already a submission in the queue
            if (!pOpsTrackingTradeAlertDataRec.auditTypeCode.equalsIgnoreCase(EC_VOID) &&
                !isEFETBkrBFIQueued){
                updateTradeRqmt(pOpsTrackingTradeAlertDataRec, EFET_BROKER, pBrokerSn);

                efetBkrAction = EFET_SUBMIT;
                if (efetDAO.isEfetDocumentExist(pOpsTrackingTradeAlertDataRec.tradeID,EFET_DAO.BFI))
                    efetBkrSubmitState = "EDIT";
                else
                    efetBkrSubmitState = "NEW";

                EFETSubmitXML_DataRec efetSubmitXMLDataRec = new EFETSubmitXML_DataRec();
                efetSubmitXMLDataRec = efetProcessor.getEfetSubmitXMLDataRec(pOpsTrackingTradeAlertDataRec.tradeID);
                efetDAO.insertBrokerFee(pOpsTrackingTradeAlertDataRec.tradeID, efetSubmitXMLDataRec.bkrFeeTotal,
                        efetSubmitXMLDataRec.bkrFeeCcy);
                //otTRADE_RQMT_dao.cancelRqmt(pOpsTrackingTradeAlertDataRec.tradeID,"XQBBP","Broker changed to EFET broker");
            }
            else if (pOpsTrackingTradeAlertDataRec.auditTypeCode.equalsIgnoreCase(EC_VOID)){
                otTRADE_RQMT_dao.cancelRqmt(pOpsTrackingTradeAlertDataRec.tradeID,EFET_BROKER,"Trade was voided");
                //This is necessary to pick out anything that is sitting in a queue
                //if (isEFETBkrBFIQueued){
                qEFETTradeAlert.updateAlertRecordByTradeId(pOpsTrackingTradeAlertDataRec.tradeID,"Y",EFET_DAO.BFI,"B");
                //if (!efetDAO.isTradeRqmtExist(pOpsTrackingTradeAlertDataRec.tradeID,"EFET"))
                qEFETTradeAlert.updateAlertRecordByTradeId(pOpsTrackingTradeAlertDataRec.tradeID,"Y",EFET_DAO.CNF,"B");

            }
            else {
                EFETSubmitXML_DataRec efetSubmitXMLDataRec = new EFETSubmitXML_DataRec();
                efetSubmitXMLDataRec = efetProcessor.getEfetSubmitXMLDataRec(pOpsTrackingTradeAlertDataRec.tradeID);
                if (efetDAO.isBrokerFeeChanged(pOpsTrackingTradeAlertDataRec.tradeID,
                        efetSubmitXMLDataRec.bkrFeeTotal,efetSubmitXMLDataRec.bkrFeeCcy)){
                    if (!isEFETBkrBFIQueued){
                        efetBkrAction = EFET_SUBMIT;
                        efetBkrSubmitState = "EDIT";
                    }
                    efetDAO.updateBrokerFee(pOpsTrackingTradeAlertDataRec.tradeID,
                        efetSubmitXMLDataRec.bkrFeeTotal,efetSubmitXMLDataRec.bkrFeeCcy);
                }
            }
          //Was an efet trade but was never submitted, or was trade-corrected so is no longer is one.
        } else if (!isEFETBkrNow) {
            //if (isEFETBkrBFIQueued){
            qEFETTradeAlert.updateAlertRecordByTradeId(pOpsTrackingTradeAlertDataRec.tradeID,"Y",EFET_DAO.BFI,"B");
            qEFETTradeAlert.updateAlertRecordByTradeId(pOpsTrackingTradeAlertDataRec.tradeID,"Y",EFET_DAO.CNF,"B");

            //5/3/2007 Israel -- was creating a broker requirement for every trade edited.
            //updateTradeRqmt(pOpsTrackingTradeAlertDataRec, "XQBBP", pBrokerSn);
        }

        efetBkrResult[0] = efetBkrAction;
        efetBkrResult[1] = efetBkrSubmitState;
        return efetBkrResult;
    }

    private String[] getEFETCptyEditAction(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec,
                                       String pEfetStatus, boolean pIsEFETSubmitQueued)
            throws Exception {
        String[] efetCptyResult = new String[4];  // samy 04-27-09 increase the array to capture document id change
        String efetCptyAction = EC_NONE;
        String efetCptySubmitState = EC_NONE;
        String efetReceiver = EC_NONE;
        String documentIdChangeFlag = "N"; // samy 04-27-09 added for document id change

        boolean isEfetCptyTrade = false;
        isEfetCptyTrade = efetDAO.isEFETCptySubmit(pOpsTrackingTradeAlertDataRec.tradingSystem,
                                           pOpsTrackingTradeAlertDataRec.tradeID);


        // 3-25-2008:  MThoresen - efetDAO.isTradeRqmtExist returns false when a CXL record exists.  This is actually
        // not correct in some situations.  I cannot remove the WHERE clause becuase to many test cases are dependent
        // upon this call.
        boolean isEfetCptyRqmtCanceled = (efetDAO.isTradeRqmtCanceled(pOpsTrackingTradeAlertDataRec.tradeID,EFET));

        /** If ec_Summary status = none, it means no row was found
         * in the ec_summary table, which means it wasn't econfirm_v1.
         */
        boolean isEFETCptyNow = false;
        if (efetDAO.isTradeRqmtExist(pOpsTrackingTradeAlertDataRec.tradeID,EFET) || (isEfetCptyRqmtCanceled))
        //if (!pEfetStatus.equalsIgnoreCase(EC_NONE))
            isEFETCptyNow = true;

        //Was it EFET before, i.e., already submitted?
        if (isEFETCptyNow) {
            //If yes and void, then cancel if not already canceled
            if (pOpsTrackingTradeAlertDataRec.auditTypeCode.equalsIgnoreCase(EC_VOID)){
                if (!pEfetStatus.equalsIgnoreCase(EC_CANCELED)){
                    efetCptyAction = EC_CANCEL;
                    efetCptySubmitState = "NONE";
                }
                //Get rid of any pending trades in the queue, whether it's been submitted or not.
                if (pIsEFETSubmitQueued)
                   qEFETTradeAlert.updateAlertRecordByTradeId(pOpsTrackingTradeAlertDataRec.tradeID,"Y",EFET_DAO.CNF,"C");
            }
            //If not then is it still Efet? If so, submit, otherwise cancel
            else if (isEfetCptyTrade){
                if(isEfetCptyRqmtCanceled){
                    efetCptyAction = EC_NONE;
                    efetCptySubmitState = "NONE";
                }

                else if (!pIsEFETSubmitQueued){
                    efetCptyAction = EC_SUBMIT;
                    efetCptySubmitState = "EDIT";
                }
            }
            //It is no longer an efet cpty trade but still is an efet broker.
            //We need to edit the CNF and change the receiver from C to B.
            else if (!isEfetCptyTrade && efetDAO.isTradeRqmtExist(pOpsTrackingTradeAlertDataRec.tradeID,EFET_BROKER)){
                if (!pEfetStatus.equalsIgnoreCase(EC_NONE)){
                    efetCptySubmitState = "EDIT";
                    efetDAO.createNewDocumentId(pOpsTrackingTradeAlertDataRec.tradeID);
                    documentIdChangeFlag = "Y"; // to pass to broker processing
                }
                else if (qEFETTradeAlert.isEFETQueued(pOpsTrackingTradeAlertDataRec.tradeID,EFET_SUBMIT,EFET_DAO.CNF,"C")){
                    qEFETTradeAlert.updateAlertRecordByTradeId(pOpsTrackingTradeAlertDataRec.tradeID,"Y",EFET_DAO.CNF,"C");
                    efetCptySubmitState = "NEW";
                }
                efetCptyAction = EC_SUBMIT;
                efetReceiver = "B";
                otTRADE_RQMT_dao.cancelRqmt(pOpsTrackingTradeAlertDataRec.tradeID,EFET,"Changed to non-EFET Trade");
            }
            //If no longer efet then cancel
            else if (!isEfetCptyTrade) {
                //Cancel if has been submitted
                if (!qEFETTradeAlert.isEFETQueued(pOpsTrackingTradeAlertDataRec.tradeID, EFET_CANCEL, EFET_DAO.CNF, "C")){
                    if (!pEfetStatus.equalsIgnoreCase(EC_NONE)) {
                        efetCptyAction = EC_CANCEL;
                        efetCptySubmitState = "NONE";
                    }
                } //If there is an outstanding new/edit submit in the queue then whack it.
                qEFETTradeAlert.updateAlertRecordByTradeId(pOpsTrackingTradeAlertDataRec.tradeID, "Y", EFET_DAO.CNF, "C");
                otTRADE_RQMT_dao.cancelRqmt(pOpsTrackingTradeAlertDataRec.tradeID, EFET, "Changed to non-EFET Trade");
            }
          //Is it efet now?
          // Also includes efet trades queued but not yet sent.
        } else if (isEfetCptyTrade){
            //It was an efet broker but now it's still an efet broker but it's also an efet cpty
            //we need to edit the CNF and change the receiver from B to C.
            if (efetDAO.isTradeRqmtExist(pOpsTrackingTradeAlertDataRec.tradeID,EFET_BROKER)){
                // Samy : 04-22-09 commented to make new document id
             //   if (!pEfetStatus.equalsIgnoreCase(EC_NONE)){
            ///        efetCptySubmitState = "EDIT";
           //     }

                 if (qEFETTradeAlert.isEFETQueued(pOpsTrackingTradeAlertDataRec.tradeID,EFET_SUBMIT,EFET_DAO.CNF,"B")){
                    qEFETTradeAlert.updateAlertRecordByTradeId(pOpsTrackingTradeAlertDataRec.tradeID,"Y",EFET_DAO.CNF,"B");
                    //Check to see if it was submitted, cancelled, then now it's being resubmitted.
                }
                if (efetDAO.isEfetDocumentExist(pOpsTrackingTradeAlertDataRec.tradeID,EFET_DAO.CNF)) {
                   efetCptySubmitState = "EDIT";
                   // change the document id for the trade
                   efetDAO.createNewDocumentId(pOpsTrackingTradeAlertDataRec.tradeID);
                   documentIdChangeFlag = "Y"; // to pass to broker processing
                }
                else {
                    efetCptySubmitState = "NEW";
                }
                efetCptyAction = EC_SUBMIT;
                //efetReceiver = "C";
                updateTradeRqmt(pOpsTrackingTradeAlertDataRec, EFET,"NONE");
            }
            //If it is efet and it isn't void then submit it
            //Only submit if there isn't already a submission in the queue
            else if (!pOpsTrackingTradeAlertDataRec.auditTypeCode.equalsIgnoreCase(EC_VOID) &&
                !pIsEFETSubmitQueued){
                updateTradeRqmt(pOpsTrackingTradeAlertDataRec, EFET,"NONE");
                efetCptyAction = EC_SUBMIT;
                //If there was a CNF, it got ignored because it was a broker deal then it wasn't, bring it back
                //because it doesn't get cancelled if it's a broker deal then the broker deal is "canceled"
                //because it doesn't really get cancelled, just ignoredf.
                if (efetDAO.isEfetDocumentExist(pOpsTrackingTradeAlertDataRec.tradeID,EFET_DAO.CNF))
                    efetCptySubmitState = "EDIT";
                else
                    efetCptySubmitState = "NEW";
            }
            else if (pOpsTrackingTradeAlertDataRec.auditTypeCode.equalsIgnoreCase(EC_VOID)){
                //This is necessary to pick out anything that is sitting in a queue
                if (pIsEFETSubmitQueued)
                   qEFETTradeAlert.updateAlertRecordByTradeId(pOpsTrackingTradeAlertDataRec.tradeID,"Y",EFET_DAO.CNF,"C");
                otTRADE_RQMT_dao.cancelRqmt(pOpsTrackingTradeAlertDataRec.tradeID,EFET,"Trade was voided");
            }
          //Was an efet trade but was never submitted, was trade-corrected so is no longer is one.
        } else if (!isEFETCptyNow) {
            if (pIsEFETSubmitQueued)
               qEFETTradeAlert.updateAlertRecordByTradeId(pOpsTrackingTradeAlertDataRec.tradeID,"Y",EFET_DAO.CNF,"C");
            otTRADE_RQMT_dao.cancelRqmt(pOpsTrackingTradeAlertDataRec.tradeID,EFET,"Changed to non-EFET Trade");
        }

        efetCptyResult[0] = efetCptyAction;
        efetCptyResult[1] = efetCptySubmitState;
        efetCptyResult[2] = efetReceiver;
        efetCptyResult[3] = documentIdChangeFlag;
        return efetCptyResult;
    }


    private OpsTrackingReturnDataRec getEConfirmEditAction(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec,
                                                           String pECStatus, TradingSystemDATA_rec pTSData_rec,String pEntityType)
            throws Exception {
        String isOneDayGasDeal = "N";
        OpsTrackingReturnDataRec ecEditActionDataRec;
        ecEditActionDataRec = new OpsTrackingReturnDataRec();
        //EConfirmDAO eConfirmDAO;
        //eConfirmDAO = new EConfirmDAO(opsTrackingConnection, affinityConnection);
        int eConfirmProductID = 0;
        //10-12-2005 IF - When edited to book Z was not being excluded from EConfirm
        //6-8-2006 IF - Added test for book ZZ
        //Samy 9/21/11 added to test flag to check , can be removed book name , but leave them for defensive programming
        //if (!pTSData_rec.BOOK.equalsIgnoreCase("Z") && !pTSData_rec.BOOK.equalsIgnoreCase("ZZ") &&
            //!pTSData_rec.BOOK.equalsIgnoreCase("TEST SG NG") &&  !pTSData_rec.BOOK.equalsIgnoreCase("TEST SG ELEC") && !pTSData_rec.TEST_BOOK_FLAG){
            // MThoresen: 5-16-2007: Added params isOneDayGasDeal and BROKER_SN.  changes made for C&C: 1 day gass, broker = ICE...C&C is false
            //isOneDayGasDeal = otTRADE_RQMT_dao.is1DayGas(pTSData_rec);
            //eConfirmProductID = eConfirmDAO.getEConfirmSubmitProductID(pOpsTrackingTradeAlertDataRec.tradingSystem,
                //pOpsTrackingTradeAlertDataRec.tradeID, pTSData_rec.SE_CPTY_SN, pTSData_rec.CPTY_SN, pTSData_rec.STTL_TYPE,
                //pTSData_rec.CDTY_CODE, pTSData_rec.TRADE_DT, pTSData_rec.EFS_FLAG, isOneDayGasDeal, pTSData_rec.BROKER_SN,pEntityType);
        //}
        //else
            //eConfirmProductID = 0;

        //Israel 7/30/15 -- temporarily removed
        //eConfirmProductID = eConfirmTradeInfo.getEConfirmProductId(pOpsTrackingTradeAlertDataRec.tradingSystem,
        //                      pOpsTrackingTradeAlertDataRec.tradeID);
        eConfirmProductID = 0;
        ecEditActionDataRec.ecProductID = eConfirmProductID;
        // MThoresen - 4-18-2007 Added for click and confirm project
        //pOpsTrackingTradeAlertDataRec.isClickAndConfirm = (eConfirmDAO.getAgreementType() == EConfirmAgreementDAO.CLICK_AND_CONFIRM);
        pOpsTrackingTradeAlertDataRec.isClickAndConfirm = false;
        /**
         * Problem with stranded bad product id
         * If ecprep && (prepproductid <> eConfirmProductID) then
         *  **problem exists!!
         *  update jbossdbq if not successful
         *  throw exception to stop service
         */

        /** If ec_Summary status = none, it means no row was found
         * in the ec_summary table, which means it wasn't econfirm_v1.
         */
        boolean wasEConfirmBeforeEdit = false;
        if (!pECStatus.equalsIgnoreCase(EC_NONE))
            wasEConfirmBeforeEdit = true;
        // Was it Econfirm before the edit....?
        if (wasEConfirmBeforeEdit) {
            //If yes and void, then cancel if not already canceled
            if (pOpsTrackingTradeAlertDataRec.auditTypeCode.equalsIgnoreCase(EC_VOID)){
                if (!pECStatus.equalsIgnoreCase(EC_CANCELED))
                    ecEditActionDataRec.ecAction = EC_CANCEL;
            }
            //Is it still EC? If so, submit, otherwise cancel
            else if (eConfirmProductID > 0){
                ecEditActionDataRec.ecAction = EC_SUBMIT;
            }
            // If this is not an ECONFIRM trade, but was one before, CANCEL....if not already canceled.
            else if (!pECStatus.equalsIgnoreCase(EC_CANCELED)){
                ecEditActionDataRec.ecAction = EC_CANCEL;
            }
          //Is it econfirm_v1 now?
        } else if (eConfirmProductID > 0){
            //If it is econfirm_v1 and it isn't void then submit it
            if (!pOpsTrackingTradeAlertDataRec.auditTypeCode.equalsIgnoreCase(EC_VOID)){
                updateTradeRqmt(pOpsTrackingTradeAlertDataRec, ECONF, "NONE");
                ecEditActionDataRec.ecAction = EC_SUBMIT;
            }
        }
        return ecEditActionDataRec;
    }

    private OpsTrackingReturnDataRec getEConfirmBkrEditAction(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec,
                                                           String pECStatus, TradingSystemDATA_rec pTSData_rec,String pEntityType)
            throws Exception {
        String isOneDayGasDeal = "N";
        OpsTrackingReturnDataRec ecEditActionDataRec;
        ecEditActionDataRec = new OpsTrackingReturnDataRec();
        EConfirmTradeInfo_DataRec ecTradeInfoData = new EConfirmTradeInfo_DataRec();

        //EConfirmDAO eConfirmDAO;
        //eConfirmDAO = new EConfirmDAO(opsTrackingConnection, affinityConnection);
        int eConfirmProductID = 0;
        boolean isEConfirmBrokerDeal;

        //10-12-2005 IF - When edited to book Z was not being excluded from EConfirm
        //6-8-2006 IF - Added test for book ZZ
        //Samy 9/21/11 added to test flag to check , can be removed book name , but leave them for defensive programming
        //if (!pTSData_rec.BOOK.equalsIgnoreCase("Z") && !pTSData_rec.BOOK.equalsIgnoreCase("ZZ")&&
         //            !pTSData_rec.BOOK.equalsIgnoreCase("TEST SG NG") && !pTSData_rec.BOOK.equalsIgnoreCase("TEST SG ELEC") && !pTSData_rec.TEST_BOOK_FLAG){
            // MThoresen: 5-16-2007: Added params isOneDayGasDeal and BROKER_SN.  changes made for C&C: 1 day gass, broker = ICE...C&C is false
        //isOneDayGasDeal = otTRADE_RQMT_dao.is1DayGas(pTSData_rec);
        //eConfirmProductID = eConfirmDAO.getEConfirmSubmitProductID(pOpsTrackingTradeAlertDataRec.tradingSystem,
         //       pOpsTrackingTradeAlertDataRec.tradeID, pTSData_rec.SE_CPTY_SN, pTSData_rec.BROKER_SN, pTSData_rec.STTL_TYPE,
           //     pTSData_rec.CDTY_CODE, pTSData_rec.TRADE_DT, pTSData_rec.EFS_FLAG, isOneDayGasDeal, pTSData_rec.BROKER_SN,pEntityType);
        // }
        // else
        //    eConfirmProductID = 0;

        //Israel 7/30/15 -- temporarily removed
        //ecTradeInfoData = eConfirmTradeInfo.getEConfirmTradeInfo_DataRec(pOpsTrackingTradeAlertDataRec.tradingSystem, pOpsTrackingTradeAlertDataRec.tradeID);
        eConfirmProductID = ecTradeInfoData.productID;
        isEConfirmBrokerDeal = (ecTradeInfoData.isEConfirmBrokerDeal &&
                (pTSData_rec.BROKER_SN.length() > 0));

        ecEditActionDataRec.ecProductID = eConfirmProductID;

        /** If ec_Summary status = none, it means no row was found
         * in the ec_summary table, which means it wasn't econfirm_v1.
         */
        boolean wasEConfirmBeforeEdit = false;
        if (!pECStatus.equalsIgnoreCase(EC_NONE))
            wasEConfirmBeforeEdit = true;
        // Was it Econfirm before the edit....?
        if (wasEConfirmBeforeEdit) {
            //If yes and void, then cancel if not already canceled
            if (pOpsTrackingTradeAlertDataRec.auditTypeCode.equalsIgnoreCase(EC_VOID)){
                if (!pECStatus.equalsIgnoreCase(EC_CANCELED))
                    ecEditActionDataRec.ecBkrAction = EC_CANCEL;
            }
            //Is it still EC? If so, submit, otherwise cancel
            else if (eConfirmProductID > 0  && isEConfirmBrokerDeal){
                ecEditActionDataRec.ecBkrAction = EC_SUBMIT;
            }
            // If this is not an ECONFIRM trade, but was one before, CANCEL....if not already canceled.
            else if (!pECStatus.equalsIgnoreCase(EC_CANCELED)){
                ecEditActionDataRec.ecBkrAction = EC_CANCEL;
            }
          //Is it econfirm_v1 now?
        } else if (eConfirmProductID > 0 && isEConfirmBrokerDeal){
            //If it is econfirm_v1 and it isn't void then submit it
            if (!pOpsTrackingTradeAlertDataRec.auditTypeCode.equalsIgnoreCase(EC_VOID)){
                updateTradeRqmt(pOpsTrackingTradeAlertDataRec, ECONFIRM_BROKER,pTSData_rec.BROKER_SN );
                ecEditActionDataRec.ecBkrAction = EC_SUBMIT;
            }
        }
        return ecEditActionDataRec;
    }

    public void processBackdoorTrade(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec)
            throws Exception {
        String ticketNo = df.format(pOpsTrackingTradeAlertDataRec.tradeID);
        Logger.getLogger(OpsTrackingTradeAlert.class).info(" Processing back door trade id: " + ticketNo);
        tsDATA_rec = tsDATA_dao.getTradingSystemDATA_rec(pOpsTrackingTradeAlertDataRec.tradingSystem,
                pOpsTrackingTradeAlertDataRec.tradeID,
                pOpsTrackingTradeAlertDataRec.tradeTypeCode);

        //Test for backfill
        if (!otTRADE_dao.isTradeExist(pOpsTrackingTradeAlertDataRec.tradeID)) {
           // if (pOpsTrackingTradeAlertDataRec.tradeID < BACKFILL_CUTOFF_ID) {
                Logger.getLogger(OpsTrackingTradeAlert.class).info("Backfill of Backdoor Trade Record: " +
                        pOpsTrackingTradeAlertDataRec.tradingSystem + " " +
                        df.format(pOpsTrackingTradeAlertDataRec.tradeID) + " ...");
                otTRADE_dao.insertTrade(pOpsTrackingTradeAlertDataRec);

                /**
                 * IF 4/19/2004 - Fixing problem with backdoor backfill not having a trade
                 * data record. All we really want here is trade_data and trade_summary--
                 * trade_rqmt is not really necessary.
                 */
                otTRADE_DATA_dao.insertTradeDataByTrdSys(tsDATA_rec);
                pOpsTrackingTradeAlertDataRec.opsDetActionsFlag = "Y";
                pOpsTrackingTradeAlertDataRec.finalApprovalFlag = "N";
                otTRADE_SUMMARY_dao.execInsertTradeSummary(pOpsTrackingTradeAlertDataRec);
           // } else
           //     throw new Exception("Backdoor backfill record out of sequence: Ticket=" + ticketNo);
        }

        otTRADE_NOTIFY_dao.insertTradeNotify(pOpsTrackingTradeAlertDataRec);
        otTRADE_DATA_dao.updateTradeData(tsDATA_rec, pOpsTrackingTradeAlertDataRec.auditTypeCode);
    }

    private String getDetermineActionsFlag(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec) {
        int determineActions = 0;

        if (pOpsTrackingTradeAlertDataRec.tradingSystem.equalsIgnoreCase("AFF") &&
            //pOpsTrackingTradeAlertDataRec.message.getStringProperty("AUDIT_TYPE_CODE").equalsIgnoreCase("ACCPT") &&
            pOpsTrackingTradeAlertDataRec.auditTypeCode.equalsIgnoreCase("ACCPT")  )
            determineActions = 1;

        final String HOUSE = "HOUSE";
        if (tsDATA_rec.CPTY_SN.equalsIgnoreCase(HOUSE)  &&
            otTRADE_DATA_rec.CPTY_SN.equalsIgnoreCase(HOUSE))
            determineActions = 2;

     //   final String BOOK_Z = "Z";
    //    final String BOOK_ZZ = "ZZ";
    //    final String BOOK_TEST_SG_NG ="TEST SG NG";
    //    final String BOOK_TEST_SG_ELEC = "TEST SG ELEC";
        //ICTS voids will have a null value here.
        //Samy 9/21/11 added to test flag to check , can be removed book name , but leave them for defensive programming

        /*
        if (tsDATA_rec.BOOK != null) {
            if ((tsDATA_rec.BOOK.equalsIgnoreCase(BOOK_Z) || tsDATA_rec.BOOK.equalsIgnoreCase(BOOK_ZZ) ||
                tsDATA_rec.BOOK.equalsIgnoreCase(BOOK_TEST_SG_NG) || tsDATA_rec.BOOK.equalsIgnoreCase(BOOK_TEST_SG_ELEC) )  || tsDATA_rec.TEST_BOOK_FLAG &&
                    (otTRADE_DATA_rec.BOOK.equalsIgnoreCase(BOOK_Z) || otTRADE_DATA_rec.BOOK.equalsIgnoreCase(BOOK_ZZ) ||
                     otTRADE_DATA_rec.BOOK.equalsIgnoreCase(BOOK_TEST_SG_NG) || otTRADE_DATA_rec.BOOK.equalsIgnoreCase(BOOK_TEST_SG_ELEC)  ||
                            otTRADE_DATA_rec.TEST_BOOK_FLAG
                    )                    )
                determineActions = 3;
        }
        */

        String determineActionsFlag = "N";
        if (determineActions == 0)
            determineActionsFlag = "Y";

        return determineActionsFlag;
    }
    
    public boolean isTodayTrade(java.util.Date tradeDt) {
        Calendar c1 = Calendar.getInstance();
        c1.setTime(tradeDt);
        Calendar c2 = Calendar.getInstance();
        c2.setTime(new java.util.Date());
        return c1.get(Calendar.YEAR) == c2.get(Calendar.YEAR) && c1.get(Calendar.MONTH) == c2.get(Calendar.MONTH)
                && c1.get(Calendar.DATE) == c2.get(Calendar.DATE); 
    }

    private void updateTradeRqmt(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec, String pRqmt, String pBrokerSn)
            throws Exception {
        String brokerSn;
        if (pBrokerSn == null)
            brokerSn = "NONE";
        else if (pBrokerSn.equalsIgnoreCase("") || pBrokerSn.length() == 0)
            brokerSn = "NONE";
        else
            brokerSn = pBrokerSn;

        //Since this is now an econfirm_v1/efet and it wasn't before, the trade_rqmt table needs to be updated:
        //1. Cancel Sempra Paper (XQCSP) and Cpty Paper (XQCCP)
//        String cmt = "NOT ASSIGNED -- RQMT=" + pRqmt;
//        if (pRqmt.equalsIgnoreCase(ECONF))
//            cmt = "Changed by system to eConfirm.";
//        else if (pRqmt.equalsIgnoreCase(EFET))
//            cmt = "Changed by system to EFET.";
//        else if (pRqmt.equalsIgnoreCase(EFET_BROKER))
//            cmt = "Changed by system to EFBKR.";
//        else if (pRqmt.equalsIgnoreCase("XQBBP"))
//            cmt = "Changed by system to XQBBP.";
//        else if (pRqmt.equalsIgnoreCase(ECONFIRM_BROKER))  {
//            cmt = "Changed by system to ECBKR";
//        }
//        else
//            throw new Exception("OpsTrackingTradeAlert.updateTradeRqmt: Unsupported rqmt=" + pRqmt + " Ticket=" +
//                    df.format(pOpsTrackingTradeAlertDataRec.tradeID));

        boolean sendOurPaperNotify = false;
        boolean sendCptyNotify = false;
        boolean sendBrokerNotify = false;
        boolean sendEFETBrokerNotify = false;
        boolean sendEConfirmBrokerNotify = false;
        if (pRqmt.equalsIgnoreCase(ECONF) || pRqmt.equalsIgnoreCase(EFET)){
            sendOurPaperNotify = otTRADE_RQMT_dao.isTradeRqmtIntitalStatus(pOpsTrackingTradeAlertDataRec.tradeID, SEMPRA_PAPER);
            otTRADE_RQMT_dao.deleteRqmt(pOpsTrackingTradeAlertDataRec.tradeID, SEMPRA_PAPER);

            sendCptyNotify = otTRADE_RQMT_dao.isTradeRqmtIntitalStatus(pOpsTrackingTradeAlertDataRec.tradeID, CPTY_PAPER);
            otTRADE_RQMT_dao.deleteRqmt(pOpsTrackingTradeAlertDataRec.tradeID, CPTY_PAPER);
        }
        else if (pRqmt.equalsIgnoreCase(EFET_BROKER) || pRqmt.equalsIgnoreCase(ECONFIRM_BROKER)){
            sendBrokerNotify = otTRADE_RQMT_dao.isTradeRqmtIntitalStatus(pOpsTrackingTradeAlertDataRec.tradeID, BROKER_PAPER);
            otTRADE_RQMT_dao.deleteRqmt(pOpsTrackingTradeAlertDataRec.tradeID, BROKER_PAPER);
        }
        else if (pRqmt.equalsIgnoreCase(BROKER_PAPER)){
            sendEFETBrokerNotify = otTRADE_RQMT_dao.isTradeRqmtIntitalStatus(pOpsTrackingTradeAlertDataRec.tradeID, EFET_BROKER);
            otTRADE_RQMT_dao.deleteRqmt(pOpsTrackingTradeAlertDataRec.tradeID, EFET_BROKER);

            sendEConfirmBrokerNotify = otTRADE_RQMT_dao.isTradeRqmtIntitalStatus(pOpsTrackingTradeAlertDataRec.tradeID, ECONFIRM_BROKER);
            otTRADE_RQMT_dao.deleteRqmt(pOpsTrackingTradeAlertDataRec.tradeID, ECONFIRM_BROKER);
        }

        boolean sendNotify = (sendOurPaperNotify || sendCptyNotify || sendBrokerNotify || sendEFETBrokerNotify || sendEConfirmBrokerNotify);
        if (sendNotify){
            String emailText = "";
            String displayName = "";
            String message_1 = "Due to a trade edit, an electronic confirmation replaced the ";
            String message_2 = " requirement, which was deleted.";

            if (sendOurPaperNotify) {
                displayName = otTRADE_RQMT_dao.GetRqmtDisplayName(SEMPRA_PAPER);
                emailText = message_1 + displayName + message_2;
            }
            if (sendCptyNotify) {
                displayName = otTRADE_RQMT_dao.GetRqmtDisplayName(CPTY_PAPER);
                emailText += "\n" + message_1 + displayName + message_2;
            }
            if (sendBrokerNotify) {
                displayName = otTRADE_RQMT_dao.GetRqmtDisplayName(BROKER_PAPER);
                emailText += "\n" + message_1 + displayName + message_2;
            }
            if (sendEFETBrokerNotify) {
                displayName = otTRADE_RQMT_dao.GetRqmtDisplayName(EFET_BROKER);
                emailText += "\n" + message_1 + displayName + message_2;
            }
            if (sendEConfirmBrokerNotify) {
                displayName = otTRADE_RQMT_dao.GetRqmtDisplayName(ECONFIRM_BROKER);
                emailText += "\n" + message_1 + displayName + message_2;
            }

            String subject = "Electronic confirmation override notification";
            mailUtils.sendMail(systemsNotifyAddress, "Systems Notify Recipients", sentFromAddress, sentFromName, subject, emailText, "");
        }

        //2. Insert econfirm_v1/efet requirement only if it doesn't already exist.
        if (!pRqmt.equalsIgnoreCase(BROKER_PAPER) ||
            (pRqmt.equalsIgnoreCase(BROKER_PAPER) && !brokerSn.equalsIgnoreCase("NONE")) &&
            !efetDAO.isTradeRqmtExist(pOpsTrackingTradeAlertDataRec.tradeID,BROKER_PAPER))
            otTRADE_RQMT_dao.insertTradeRqmt(pOpsTrackingTradeAlertDataRec, pRqmt, "INSERTED FROM EDIT", "N", 0);

        // MThoresen - 4-18-2007, ADD VERBAL FOR CLICK AND CONFIRM, AND NO BROKER RQMT FOUND AND ECONF TRADE
        if ((pRqmt.equalsIgnoreCase(ECONF)) &&
            (pOpsTrackingTradeAlertDataRec.isClickAndConfirm) &&
            (!otTRADE_RQMT_dao.hasClickAndConfirmVerbalRqmt(pOpsTrackingTradeAlertDataRec.tradeID)) &&
            (!otTRADE_RQMT_dao.hasBrokerRqmt(pOpsTrackingTradeAlertDataRec.tradeID))){
            otTRADE_RQMT_dao.insertTradeRqmt(pOpsTrackingTradeAlertDataRec, "VBCP", EConfirmData.EC_CLICK_AND_CONF_REF, "N", 0);
        }else if (pRqmt.equalsIgnoreCase(ECONF)){
            // MAKE SURE THERE IS NO CLICK AND CONFIRM VRBL RQMT!!
            if (otTRADE_RQMT_dao.hasClickAndConfirmVerbalRqmt(pOpsTrackingTradeAlertDataRec.tradeID)){
                otTRADE_RQMT_dao.cancelRqmt(pOpsTrackingTradeAlertDataRec.tradeID,"VBCP", "1 DAY GAS, OR ICE");
            }
        }
        /*
        if ( pRqmt.equalsIgnoreCase(ECONFIRM_BROKER)) {
            if ( !efetDAO.isTradeRqmtExist(pOpsTrackingTradeAlertDataRec.tradeID,ECONFIRM_BROKER) )
            {
                otTRADE_RQMT_dao.insertTradeRqmt(pOpsTrackingTradeAlertDataRec, pRqmt, "INSERTED FROM EDIT", "N");
            }
        }
        */
    }
    // MThoresen 4-17-2007: Added for click and confirm
    //public EConfirmDAO geteConfirmDAO() {
      //  return eConfirmDAO;
    //}

    public OpsTrackingReturnDataRec processBasketMemberTrade(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec,String processFlag) throws Exception {
        OpsTrackingReturnDataRec returnDataRec = null;
        returnDataRec = new  OpsTrackingReturnDataRec();
        returnDataRec.init();

        // final approve or reopen only if the trade exists in the ops_tracking schema
        // putting the FUTURE or other exchange trade  in the basket
        // does not affect the status of the trade in the ops tracking
        if ( otTRADE_dao.isTradeExist(pOpsTrackingTradeAlertDataRec.tradeID )) {
            boolean isFinalApproved = otTRADE_SUMMARY_dao.isTradeFinalApproved(pOpsTrackingTradeAlertDataRec.tradeID);

            if ("Y".equalsIgnoreCase(processFlag)){  // the trade is put into the basket
                if  (!isFinalApproved){
                opsTrackingBasker_dao.finalApprove(pOpsTrackingTradeAlertDataRec.tradeID,"Y"); // final approve the  trade
            }
            otTRADE_NOTIFY_dao.insertTradeNotify(pOpsTrackingTradeAlertDataRec);
            }
            else { // process flag is R   trade has been removed from the basket.
                if (isFinalApproved){
                opsTrackingBasker_dao.finalApprove(pOpsTrackingTradeAlertDataRec.tradeID,"N"); // reopen the trade.
                }
                // if the trade is removed from the basket
                // need to update the latest trade into the ops tracking schema
                // process like an edit.
                returnDataRec = processEditedTrade(pOpsTrackingTradeAlertDataRec);
            }
        }
        return returnDataRec;
    }

}

