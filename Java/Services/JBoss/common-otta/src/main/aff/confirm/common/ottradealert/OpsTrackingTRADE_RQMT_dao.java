package aff.confirm.common.ottradealert;

import aff.confirm.common.dao.RqmtDAO;
import aff.confirm.common.econfirm.EConfirmTradeInfo;
import aff.confirm.common.econfirm.datarec.EConfirmTradeInfo_DataRec;
import aff.confirm.common.efet.dao.EFET_DAO;
import aff.confirm.common.util.DAOUtils;
import aff.confirm.common.util.MailUtils;
import com.sun.rowset.CachedRowSetImpl;
import oracle.jdbc.OracleCallableStatement;
import org.jboss.logging.Logger;

import javax.jms.JMSException;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Types;
import java.text.DecimalFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.Locale;

//import sun.jdbc.rowset.CachedRowSet;


/**
 * User: ifrankel
 * Date: Jun 10, 2003
 * Time: 9:41:30 AM
 * To change this template use Options | File Templates.
 */
public class OpsTrackingTRADE_RQMT_dao extends OpsTrackingDAO {
    private final String OUR_PAPER = "XQCSP";
    private final String CPTY_PAPER = "XQCCP";
    private final String ECONFIRM_CPTY = "ECONF";
    private final String EFET_CPTY = "EFET";
    private final String BROKER_PAPER = "XQBBP";
    private final String ECONFIRM_BROKER = "ECBKR";
    private final String EFET_BROKER = "EFBKR";
    private final String NO_CONF = "NOCNF";
    private final String MISC_RQMT = "MISC";
    private final String VERBAL_RQMT = "VBCP";
    private final String GET_EC_PROD_ID_METHOD_NAME = "GetEConfirmProductId";
    private final String EC_CLICK_AND_CONF_REF = "SUBMIT CLICK AND CONFIRM OK";

    private java.sql.Connection affinityConnection;
//    private java.sql.Connection ictsConnection;
    private String tradeDataWebServiceUrl = "";
    private String tradeDataRootTag = "";
    private ArrayList rqmtList = new ArrayList();
    private ArrayList rqmtRefList = new ArrayList();
    private ArrayList rqmtSecondCheckList = new ArrayList();
    private final SimpleDateFormat sdfSPDate = new SimpleDateFormat("MM/dd/yyyy", Locale.US);
    private final DecimalFormat df = new DecimalFormat("#0;-#0");
    private MailUtils mailUtils;
    private String ecFailedLogAddress;
    private RqmtDAO rqmtDAO;
    private OpsTrackingRQMT_EXT_PROCESS_DATA_dao otRqmtExtProcessData;
    //private EConfirmDAO eConfirmDAO;
    private EFET_DAO efetDAO;
    private String econfirmTradeInfoServiceUrl;
    //private OpsTrackingRulesProc otRulesProc;
    //private SecondCheckDAO secondCheckDAO;
    //private OpsTrackingSecondCheck_rec otSecondCheckRec;

    public OpsTrackingTRADE_RQMT_dao(java.sql.Connection pOpsTrackingConnection,
                                     java.sql.Connection pAffinityConnection,
                                     /*java.sql.Connection pICTSConnction,*/
                                     String pTradeDataWebServiceUrl,
                                     String pTradeDataRootTag,
                                     MailUtils pMailUtils,
                                     String pECFailedLogAddress,
                                     String pEConfirmTradeInfoServiceUrl)
            throws SQLException, Exception {

        this.opsTrackingConnection = pOpsTrackingConnection;
        this.affinityConnection = pAffinityConnection;
//        this.ictsConnection = pICTSConnction;
        this.mailUtils = pMailUtils;
        this.ecFailedLogAddress = pECFailedLogAddress;
        this.tradeDataWebServiceUrl = pTradeDataWebServiceUrl;
        this.tradeDataRootTag = pTradeDataRootTag;
        this.econfirmTradeInfoServiceUrl = pEConfirmTradeInfoServiceUrl;

        //rqmtList.clear();
        //rqmtRefList.clear();
        rqmtDAO = new RqmtDAO(opsTrackingConnection);
        otRqmtExtProcessData = new OpsTrackingRQMT_EXT_PROCESS_DATA_dao(opsTrackingConnection);

        //eConfirmDAO = new EConfirmDAO(opsTrackingConnection,affinityConnection, /*ictsConnection,*/ mailUtils, ecFailedLogAddress);
        //connect to opstracking so changes can be made on dev for testing.
        efetDAO = new EFET_DAO(opsTrackingConnection, affinityConnection);
        //secondCheckDAO = new SecondCheckDAO(opsTrackingConnection);
        //otSecondCheckRec = new OpsTrackingSecondCheck_rec();

        //IF**BringBack**
        //otRulesProc = new OpsTrackingRulesProc(opsTrackingConnection);
    }

    public OpsTrackingReturnDataRec determineRqmts(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec,
                                                   TradingSystemDATA_rec pTSData_rec)
            throws SQLException, JMSException, Exception {
        CachedRowSetImpl crs = new CachedRowSetImpl();
        OpsTrackingReturnDataRec otReturnDataRec;
        otReturnDataRec = new OpsTrackingReturnDataRec();

        int eConfirmProductID;
        int eConfirmBkrProductID;
        boolean efetCptySubmit = false;
        boolean efetBkrSubmit = false;
        boolean is1DayGas = false;
        boolean is3DayPower = false;
        boolean isBroker = false;
        // MThoresen - 5-16-2005- Click and Confirm Change: 1 day deals, and Broker = ICE...Set C&C to false.
        String isOneDayGasDeal = "N";

        clearRqmtListEntries();
        rqmtSecondCheckList.clear();
        try {
            String rqmt = "";
            String reference = "";

            //Israel 10/29/2013 - removed pending implementation of rules processing engine.
//            int ruleId = otRulesProc.GetRqmtRule(pTSData_rec);
//            int ruleId = 0;
/*            if (ruleId > 0){
                //IF 1/18/2013 -- 'backdoor' this one time to get a result list. All other access to rqmtList and rqmtRefList are via methods.
                rqmtList = otRulesProc.GetRqmtRuleResults(ruleId);
                rqmtRefList.addAll(rqmtList);
            }
            else*/ {
                //addRqmtListEntry(OUR_PAPER, "SYSTEM DEFAULT");
            }

            //           if (pTSData_rec.BROKER_SN != null && pTSData_rec.BROKER_SN.length() > 0) {
            //if (!pTSData_rec.BROKER_SN.equalsIgnoreCase("") ) {
            //Israel 10/29/2013 - removed pending implementation of rules processing engine.
//                if (otRulesProc.GetRqmtBrokerExcludeRule(pTSData_rec) == 0)
            //                   addRqmtListEntry(BROKER_PAPER);
            //         }
            //
            //May already be set from OpsTrackingTradeAlert.processEditedTrade...
            isOneDayGasDeal = is1DayGas(pTSData_rec);

            //Israel 8/27/15 -- Exclude Book Z
            boolean isTestBook = pTSData_rec.BOOK.equalsIgnoreCase("Z");

            //Uncomment to make econfirm go live
            //EConfirmTradeInfo ecTradeInfo = new EConfirmTradeInfo(econfirmTradeInfoServiceUrl);
            EConfirmTradeInfo_DataRec ecTradeInfo_DataRec;
            ecTradeInfo_DataRec = new EConfirmTradeInfo_DataRec();

            //Israel 7/29/15 -- Removed for BTG -- re-implement later.
            //ecTradeInfo_DataRec = ecTradeInfo.getEConfirmTradeInfo_DataRec(pTSData_rec.tradingSystem, pTSData_rec.TRADE_ID);
            //eConfirmProductID = ecTradeInfo_DataRec.productID;
            //isBroker = ecTradeInfo_DataRec.isEConfirmBrokerDeal;

            //Israel 8/27/15 -- Exclude Book Z
            if (isTestBook)
            {
                eConfirmProductID = 0;
            }
            //remove following when above code is uncommented
            eConfirmProductID = 0;
            isBroker = false;
            if (eConfirmProductID > 0) {
                addRqmtListEntry(ECONFIRM_CPTY, reference);
//                pOpsTrackingTradeAlertDataRec.isClickAndConfirm = (eConfirmDAO.getAgreementType() == EConfirmAgreementDAO.CLICK_AND_CONFIRM);
                pOpsTrackingTradeAlertDataRec.isClickAndConfirm = false;
            }
            else {
                //Israel 8/27/15 -- Exclude Book Z
                efetCptySubmit = false;
                if (!isTestBook)
                    efetCptySubmit = efetDAO.isEFETCptySubmit(pOpsTrackingTradeAlertDataRec.tradingSystem,
                        pOpsTrackingTradeAlertDataRec.tradeID);

                if (efetCptySubmit) {
                    addRqmtListEntry(EFET_CPTY, reference);
                }
            }

            if ( eConfirmProductID > 0 && isBroker) {
                rqmt = ECONFIRM_BROKER;
                addRqmtListEntry(rqmt, reference);
            }
            else {
                //Israel 8/27/15 -- Exclude Book Z
                efetBkrSubmit = false;
                if (!isTestBook)
                    efetBkrSubmit = efetDAO.isEFETBkrSubmit(pOpsTrackingTradeAlertDataRec.tradingSystem,
                        pOpsTrackingTradeAlertDataRec.tradeID);

                if (efetBkrSubmit) {
                    addRqmtListEntry(EFET_BROKER, reference);
                }
            }

            crs = getCwfRqmts(pTSData_rec,pOpsTrackingTradeAlertDataRec.strUpdateBusnDt);

            //Read the result set and add all requirements
            crs.beforeFirst();
            while (crs.next()) {
                //Transform EBC->XQBBP, etc.
                rqmt = translateRqmt(crs.getString("RQMT"));

                reference = "";
                if (rqmt.equalsIgnoreCase(NO_CONF)){    //"NO_CONF"
                    reference = crs.getString("METHOD");
                    //pOpsTrackingTradeAlertDataRec.insertNoConfirmReason = reference;
                }

                if (rqmt.equalsIgnoreCase("BOTH")) {
                    reference = "";
                    rqmt = CPTY_PAPER; // "XQCCP";  //ecc
                    rqmtList.add(rqmt);
                    rqmtRefList.add(reference);
                    rqmt = OUR_PAPER; // "XQCSP"; //scn
                    rqmtList.add(rqmt);
                    rqmtRefList.add(reference);
                } else {
                    rqmtList.add(rqmt);
                    rqmtRefList.add(reference);
                }
            }

            //If econfirm or efet, remove sempra, cpty paper
            if ( (rqmtList.indexOf(ECONFIRM_CPTY) > -1) ||
                    (rqmtList.indexOf(EFET_CPTY)  > -1) ){
                removeRqmtListEntry(CPTY_PAPER);
                removeRqmtListEntry(OUR_PAPER);
                removeRqmtListEntry(VERBAL_RQMT);
            }

            //IF 6/30/2006 - EFET Broker Matching
            //If there is an EFET broker rqmt or eConfirm Broker then delete the non-EFET broker rqmt.
            if ( ( (rqmtList.indexOf(EFET_BROKER) > -1) || (rqmtList.indexOf(ECONFIRM_BROKER) > -1) ) &&
                    (rqmtList.indexOf(BROKER_PAPER) > -1))
                removeRqmtListEntry(BROKER_PAPER);

            //If second check is supported for this rqmt, see if it passes the test. Add it if it does.
/*            if ((rqmtList.indexOf(ECONFIRM_CPTY) < 0) && (rqmtList.indexOf(EFET_CPTY) < 0)){
                //String rqmtCommaList = getRqmtsAsString();
                String rqmtTest = "";
                for (Iterator i = rqmtList.iterator(); i.hasNext();){
                    rqmtTest = i.next().toString();
                    //Israel 10/29/2013 -- removed pending implementing otRulesProc
//                    if( (rqmtTest.equals(BROKER_PAPER)) || (rqmtTest.equals(CPTY_PAPER) )){
//                        if (otRulesProc.GetRqmt2ndChk(rqmtTest, pTSData_rec) > 0)
//                            rqmtSecondCheckList.add(rqmtTest);
//                    }
                }
            }*/

/*            if ( (rqmtList.indexOf(ECONFIRM_CPTY) > -1) &&
                 (rqmtList.indexOf(BROKER_PAPER) == -1) &&
                 (pOpsTrackingTradeAlertDataRec.isClickAndConfirm)){
                rqmt = VERBAL_RQMT;
                reference = EConfirmDAO.EC_CLICK_AND_CONF_REF;
                addRqmtListEntry(rqmt, reference);
            }
*/

            //Israel 5/5/2014 - The cwf call adds a broker requirement == adds default broker rqmt
            //Israel 5/20/2015 - Only adds broker if no relevant entry in RULES_BROKER table.
            //Israel 5/27/2015 - Removed altogether to allow determine_actions procedure to determine.
//            if (pTSData_rec.BROKER_SN != null &&
//                    pTSData_rec.BROKER_SN.length() > 0 &&
//                    rqmtList.indexOf(BROKER_PAPER) == -1 &&
//                    !rulesBrokerDAO.isBrokerExcluded(pTSData_rec.BROKER_SN, pTSData_rec.SE_CPTY_SN, pTSData_rec.CDTY_GRP_CODE))
//                addRqmtListEntry(BROKER_PAPER);

            //Israel 5/5/2014 - The cwf call adds a paper requirement == adds default paper rqmt
            //Israel 10/30/2014 - We don't want both a paper and a no confirm requirement, so add paper
            //  only if no No Confirm requirement has been added.
            //Israel 6/25/15
/*            if( rqmtList.isEmpty() ||
//              ((rqmtList.indexOf(BROKER_PAPER) > -1) && (rqmtList.indexOf(OUR_PAPER) == -1)) )
                    ((rqmtList.indexOf(BROKER_PAPER) > -1) && (rqmtList.indexOf(OUR_PAPER) == -1) && rqmtList.indexOf(NO_CONF) == -1) )
                addRqmtListEntry(OUR_PAPER, "SYSTEM DEFAULT");*/

            //Israel 2/20/2015 - Logic was ignoring cpty sends and econfirm, efet
            boolean isConfirm =
                    (rqmtList.indexOf(ECONFIRM_CPTY) > -1) ||
                            (rqmtList.indexOf(EFET_CPTY) > -1) ||
                            (rqmtList.indexOf(OUR_PAPER) > -1) ||
                            (rqmtList.indexOf(CPTY_PAPER) > -1) |
                                    (rqmtList.indexOf(NO_CONF) > -1);

            if (rqmtList.isEmpty() && !isConfirm)
                addRqmtListEntry(OUR_PAPER, "SYSTEM DEFAULT");

            //Israel 10/30/2014 - We don't want both paper and a no confirm requirement.
            if (rqmtList.indexOf(OUR_PAPER) > -1 && rqmtList.indexOf(NO_CONF) > -1)
                removeRqmtListEntry(OUR_PAPER);
            if (rqmtList.indexOf(CPTY_PAPER) > -1 && rqmtList.indexOf(NO_CONF) > -1)
                removeRqmtListEntry(CPTY_PAPER);

            pOpsTrackingTradeAlertDataRec.finalApprovalFlag = "N";
            pOpsTrackingTradeAlertDataRec.opsDetActionsFlag = "N";
            if (rqmt == "")
                pOpsTrackingTradeAlertDataRec.opsDetActionsFlag = "Y";
        }  finally {
        }

        otReturnDataRec.ecProductID = 0;
        if (eConfirmProductID > 0) {
            otReturnDataRec.ecProductID = eConfirmProductID;
            otReturnDataRec.ecAction = "SUBMIT";
        }
        if (eConfirmProductID > 0 && isBroker ) {
            otReturnDataRec.ecProductID = eConfirmProductID;
            otReturnDataRec.ecBkrAction ="SUBMIT";
        }

        //A CNF doc is sent if it's an efet cpty or broker.
        //However, we need to indicate to whom we send it.
        if (efetCptySubmit || efetBkrSubmit){
            otReturnDataRec.efetCptyAction = "SUBMIT";
            otReturnDataRec.efetCptySubmitState = "NEW";
            if (efetCptySubmit)
                otReturnDataRec.efetCnfReceiver = "C";
            else
                otReturnDataRec.efetCnfReceiver = "B";
        }
        if (efetBkrSubmit){
            otReturnDataRec.efetBkrAction = "SUBMIT";
            otReturnDataRec.efetBkrSubmitState = "NEW";
        }
        return otReturnDataRec;
    }

    //Search for IF**BringBack** and uncomment everything found (gets rid compile errors)
    public OpsTrackingReturnDataRec determineRqmts_NOT_YET_READY_FOR_PRIMETIME(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec,
                                                                               TradingSystemDATA_rec pTSData_rec)
            throws SQLException, JMSException, Exception {
        OpsTrackingReturnDataRec otReturnDataRec;
        otReturnDataRec = new OpsTrackingReturnDataRec();

        int eConfirmProductID;
        int eConfirmBkrProductID;
        boolean efetCptySubmit = false;
        boolean efetBkrSubmit = false;
        //boolean is1DayGas = false;
        //boolean is3DayPower = false;
        // MThoresen - 5-16-2005- Click and Confirm Change: 1 day deals, and Broker = ICE...Set C&C to false.
        String isOneDayGasDeal = "N";

        clearRqmtListEntries();
        rqmtSecondCheckList.clear();
        try {
            String rqmt = "";
            String reference = "";

            //IF**BringBack**
            /*
            int ruleId = otRulesProc.GetRqmtRule(pTSData_rec);
            if (ruleId > 0){
                //IF 1/18/2013 -- 'backdoor' this one time to get a result list. All other access to rqmtList and rqmtRefList are via methods.
                rqmtList = otRulesProc.GetRqmtRuleResults(ruleId);
                //IF**BringBack**
                //rqmtRefList = otRulesProc.GetRqmtRuleResultReference(ruleId);
            }
            else {
                addRqmtListEntry(OUR_PAPER, "SYSTEM DEFAULT");
            }  */

            //IF**BringBack**
            /*
            if (!pTSData_rec.BROKER_SN.isEmpty()) {
                if (otRulesProc.GetRqmtBrokerExcludeRule(pTSData_rec) == 0)
                    addRqmtListEntry(BROKER_PAPER);
            }  */

            eConfirmProductID = 0;
/*            eConfirmProductID = eConfirmDAO.getEConfirmSubmitProductID(
                    pOpsTrackingTradeAlertDataRec.tradingSystem,
                    pOpsTrackingTradeAlertDataRec.tradeID,
                    pTSData_rec.SE_CPTY_SN,
                    pTSData_rec.CPTY_SN,
                    pTSData_rec.STTL_TYPE,
                    pTSData_rec.CDTY_CODE,
                    pTSData_rec.TRADE_DT,
                    pTSData_rec.EFS_FLAG,
                    isOneDayGasDeal,
                    pTSData_rec.BROKER_SN,
                    EConfirmAgreementDAO.CPTY_TYPE)*/;

            if (eConfirmProductID > 0) {
                addRqmtListEntry(ECONFIRM_CPTY, reference);
                //pOpsTrackingTradeAlertDataRec.isClickAndConfirm = (eConfirmDAO.getAgreementType() == EConfirmAgreementDAO.CLICK_AND_CONFIRM);
            }
            // Samy : 07/05/2011 following commented to skip the efet testing for SGE
            /*
            else {
                efetCptySubmit = efetDAO.isEFETCptySubmit(pOpsTrackingTradeAlertDataRec.tradingSystem,
                        pOpsTrackingTradeAlertDataRec.tradeID);
                if (efetCptySubmit) {
                    addRqmtListEntry(EFET_CPTY, reference);
                }
            }
            */

            eConfirmBkrProductID = 0;
/*            eConfirmBkrProductID = eConfirmDAO.getEConfirmSubmitProductID(
                    pOpsTrackingTradeAlertDataRec.tradingSystem,
                    pOpsTrackingTradeAlertDataRec.tradeID,
                    pTSData_rec.SE_CPTY_SN,
                    pTSData_rec.CPTY_SN,
                    pTSData_rec.STTL_TYPE,
                    pTSData_rec.CDTY_CODE,
                    pTSData_rec.TRADE_DT,
                    pTSData_rec.EFS_FLAG,
                    isOneDayGasDeal,
                    pTSData_rec.BROKER_SN,
                    EConfirmAgreementDAO.BROKER_TYPE);*/

            if ( eConfirmBkrProductID > 0) {
                rqmt = ECONFIRM_BROKER;
                addRqmtListEntry(rqmt, reference);
            }

            //IF 6/30/2006 - EFET Broker Matching
            // Samy: 7/5/2011 following commented to bypass efet testing
            /*
            else {
                efetBkrSubmit = efetDAO.isEFETBkrSubmit(pOpsTrackingTradeAlertDataRec.tradingSystem,
                        pOpsTrackingTradeAlertDataRec.tradeID);
                if (efetBkrSubmit) {
                    addRqmtListEntry(EFET_BROKER, reference);
                }
            }
            */

            //crs = getCwfRqmts(pTSData_rec,pOpsTrackingTradeAlertDataRec.strUpdateBusnDt);

            //If econfirm_v1 or efet, remove sempra, cpty paper
            if ( (rqmtList.indexOf(ECONFIRM_CPTY) > -1) ||
                    (rqmtList.indexOf(EFET_CPTY)  > -1) ){
                removeRqmtListEntry(CPTY_PAPER);
                removeRqmtListEntry(OUR_PAPER);
                removeRqmtListEntry(VERBAL_RQMT);
            }

            //IF 11/16/2005 - If no sempra or cpty then kill verbal
/*
            if ( (rqmtList.indexOf(OUR_PAPER) == -1) &&
                    (rqmtList.indexOf(CPTY_PAPER) == -1) ){
                removeRqmtListEntry(VERBAL_RQMT, rqmtList, rqmtRefList);
            }
*/

            //IF 6/30/2006 - EFET Broker Matching
            //If there is an EFET broker rqmt or eConfirm Broker then delete the non-EFET broker rqmt.
            if ( ( (rqmtList.indexOf(EFET_BROKER) > -1) || (rqmtList.indexOf(ECONFIRM_BROKER) > -1) ) &&
                    (rqmtList.indexOf(BROKER_PAPER) > -1))
                removeRqmtListEntry(BROKER_PAPER);

            //If second check is supported for this rqmt, see if it passes the test. Add it if it does.
            if ((rqmtList.indexOf(ECONFIRM_CPTY) < 0) && (rqmtList.indexOf(EFET_CPTY) < 0)){
                //String rqmtCommaList = getRqmtsAsString();
                String rqmtTest = "";
                for (Iterator i = rqmtList.iterator(); i.hasNext();){
                    rqmtTest = i.next().toString();
                    if( (rqmtTest.equals(BROKER_PAPER)) || (rqmtTest.equals(CPTY_PAPER) )){
                        //IF**BringBack**
//                        if (otRulesProc.GetRqmt2ndChk(rqmtTest, pTSData_rec) > 0)
                        rqmtSecondCheckList.add(rqmtTest);
                    }
                }
            }

/*
            if ( (rqmtList.indexOf(ECONFIRM_CPTY) > -1) &&
                    (rqmtList.indexOf(BROKER_PAPER) == -1) &&
                    (pOpsTrackingTradeAlertDataRec.isClickAndConfirm)){
                rqmt = VERBAL_RQMT;
                reference = EConfirmDAO.EC_CLICK_AND_CONF_REF;
                addRqmtListEntry(rqmt, reference);
            }
*/
            pOpsTrackingTradeAlertDataRec.finalApprovalFlag = "N";
            pOpsTrackingTradeAlertDataRec.opsDetActionsFlag = "N";
            if (rqmt == "")
                pOpsTrackingTradeAlertDataRec.opsDetActionsFlag = "Y";
        }  finally {
        }

        otReturnDataRec.ecProductID = 0;
        if (eConfirmProductID > 0) {
            otReturnDataRec.ecProductID = eConfirmProductID;
            otReturnDataRec.ecAction = "SUBMIT";
        }
        if (eConfirmBkrProductID > 0) {
            otReturnDataRec.ecProductID = eConfirmBkrProductID;
            otReturnDataRec.ecBkrAction ="SUBMIT";
        }

        //A CNF doc is sent if it's an efet cpty or broker.
        //However, we need to indicate to whom we send it.
        if (efetCptySubmit || efetBkrSubmit){
            otReturnDataRec.efetCptyAction = "SUBMIT";
            otReturnDataRec.efetCptySubmitState = "NEW";
            if (efetCptySubmit)
                otReturnDataRec.efetCnfReceiver = "C";
            else
                otReturnDataRec.efetCnfReceiver = "B";
        }
        if (efetBkrSubmit){
            otReturnDataRec.efetBkrAction = "SUBMIT";
            otReturnDataRec.efetBkrSubmitState = "NEW";
        }
        return otReturnDataRec;
    }

    public ArrayList getRqmtList(){
        ArrayList resultList = new ArrayList();
        resultList.addAll(rqmtList);
        return resultList;
    }

    public void insertTradeRqmts(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec, TradingSystemDATA_rec tsDATA_rec, boolean pReconcileEditedTrade)
            throws SQLException, ParseException, JMSException {
        //  Removed for withdrawl of Confirmations project
        //if (pReconcileEditedTrade)
            //reconcileEditedRqmts(pOpsTrackingTradeAlertDataRec);

        String rqmt;
        String reference;
        String secondCheckFlag;
        //The list is complete, now insert requirements
        int index = -1;
        for (Iterator i = rqmtList.iterator(); i.hasNext();){
            index++;
            rqmt = "";
            rqmt = i.next().toString();
            reference = "";
            reference = rqmtRefList.get(index).toString();
            secondCheckFlag = "N";
            int rqmt2ndChkRuleId = 0;

            //Israel 10/29/13 - removed pending implementation of 2nd Rules check
//            if (rqmtSecondCheckList.indexOf(rqmt)>-1) {
//                secondCheckFlag = "Y";
//                rqmt2ndChkRuleId = otRulesProc.GetRqmt2ndChk(rqmt, tsDATA_rec);
//            }
            double rqmtId = -1;
            rqmtId = insertTradeRqmt(pOpsTrackingTradeAlertDataRec, rqmt, reference, secondCheckFlag, rqmt2ndChkRuleId);
            /*if (rqmt.equalsIgnoreCase("XQCSP")) {
                pOpsTrackingTradeAlertDataRec.insertTradeConfirm = true;
                pOpsTrackingTradeAlertDataRec.insertRqmtId = rqmtId;
                pOpsTrackingTradeAlertDataRec.insertConfirmStatusInd = "N";
            }*/
        }
    }

/*  Removed for withdrawl of Confirmations project
    private void reconcileEditedRqmts(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec)
            throws SQLException {
        ArrayList currRqmtList = new ArrayList();
        ArrayList currRqmtStatusList = new ArrayList();
        String currRqmt = "";
        String currStatus = "";
        CachedRowSet crs = getTradeRqmts(pOpsTrackingTradeAlertDataRec.tradeID);

        crs.beforeFirst();
        while (crs.next()) {
            currRqmt = crs.getString("RQMT");
            currStatus = crs.getString("STATUS");
            currRqmtList.add(currRqmt);
            currRqmtStatusList.add(currStatus);
        }

        reconcileRqmtLists(pOpsTrackingTradeAlertDataRec, "XQCSP", currRqmtList, currRqmtStatusList);
        reconcileRqmtLists(pOpsTrackingTradeAlertDataRec, "XQCCP", currRqmtList, currRqmtStatusList);

        currRqmtList.clear();
        currRqmtStatusList.clear();
        currRqmtList = null;
        currRqmtStatusList.clear();
        crs.close();
        crs = null;
    }

    private void reconcileRqmtLists(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec,
                                    String pRqmt, ArrayList pCurrRqmtList, ArrayList pCurrRqmtStatusList)
            throws SQLException {
        String currStatus;
        if (!rqmtList.contains(pRqmt) && pCurrRqmtList.contains(pRqmt)){
            int index = pCurrRqmtList.indexOf(pRqmt);
            currStatus = pCurrRqmtStatusList.get(index).toString();
            String initialStatus = rqmtDAO.getInitialStatus(pRqmt);
            if (currStatus.equalsIgnoreCase(initialStatus)){
                double rqmtId = 0;
                rqmtId = getRqmtId(pOpsTrackingTradeAlertDataRec.tradeID,pRqmt,true);
                String extProcessCode = "AFCNF";
                String attribName = "PRMNT_TRD_CONFIRM_ID";
                String strPrmntTradeConfirmId = null;
                double prmntTradeConfirmId = 0;
                strPrmntTradeConfirmId = otRqmtExtProcessData.getAttribValue(rqmtId,extProcessCode,attribName);
                prmntTradeConfirmId = Double.parseDouble(strPrmntTradeConfirmId);

                //String action = "UPDATE";
                String confirmStatusInd = "X";
                String noConfirmReason = "Trade edited on same day it was started.";

                pOpsTrackingTradeAlertDataRec.cancelTradeConfirm = true;
                pOpsTrackingTradeAlertDataRec.cancelRqmtId = rqmtId;
                pOpsTrackingTradeAlertDataRec.cancelPrmntTradeConfirmId = 0;
                pOpsTrackingTradeAlertDataRec.cancelConfirmStatusInd = confirmStatusInd;
                pOpsTrackingTradeAlertDataRec.cancelNoConfirmReason = noConfirmReason;
                //sendMessage(pOpsTrackingTradeAlertDataRec,rqmtId,action,prmntTradeConfirmId,confirmStatusInd,noConfirmReason);
                String cmt = "Canceled by system: Trade edited on same day it was started.";
                cancelRqmt(pOpsTrackingTradeAlertDataRec.tradeID, pRqmt, cmt);
            }
        }

        //It was on the list before and it's on the list now, so take it off
        //so it doesn't get created a second time.
        if (rqmtList.contains(pRqmt) && pCurrRqmtList.contains(pRqmt)){
            removeRqmtListEntry(pRqmt, rqmtList, rqmtRefList);
        }
    }*/

    public double insertTradeRqmt(OpsTrackingTradeAlertDataRec pOpsTrackingTradeAlertDataRec, String pRqmt,
                                  String pReference, String pSecondCheckFlag, int pRqmt2ndChkRuleId)
            throws SQLException {
        String initialStatus = null;
        //initialStatus = getInitialStatus(pRqmt);
        initialStatus = rqmtDAO.getInitialStatus(pRqmt);
        //Change the value only for noconf. The gui uses
        //the database setting.
        if (pRqmt.equalsIgnoreCase("NOCNF"))
            initialStatus = "APPR";

        // MThoresen - 4-18-2007 if econfirm_v1, and click and confirm, set initial status to click
        if ((pRqmt.equalsIgnoreCase("ECONF")) && (pOpsTrackingTradeAlertDataRec.isClickAndConfirm))
            initialStatus = "CLICK";

        PreparedStatement statement = null;
        double nextSeqNo = -1;
        try {
            String seqName = "seq_trade_rqmt";
            nextSeqNo = (double) getNextSequence(seqName);
            String insertSQL =
                    "Insert into ops_tracking.TRADE_RQMT( " +
                    "ID, " + //1
                    "TRADE_ID, " + //2
                    "RQMT_TRADE_NOTIFY_ID, " + //3
                    "RQMT, " + //4
                    "STATUS, " + //5
                    "COMPLETED_DT, " + //6
                    "COMPLETED_TIMESTAMP_GMT, " + //7
                    "REFERENCE, " + //8
                    "CANCEL_TRADE_NOTIFY_ID," + //9
                    "SECOND_CHECK_FLAG ) " + //10
                    "values( ?, ?, ?, ?, ?, ?, ?, ?, ?, ? )";

//IF Stub
//  Add column for Rqmt_2nd_chk_rule_id
            statement = opsTrackingConnection.prepareStatement(insertSQL);
            statement.setDouble(1, nextSeqNo);
            statement.setDouble(2, pOpsTrackingTradeAlertDataRec.tradeID);
            statement.setDouble(3, pOpsTrackingTradeAlertDataRec.tradeNotifyID );
            statement.setString(4, pRqmt);
            statement.setString(5, initialStatus);
            statement.setNull(6, Types.DATE);
            statement.setNull(7, Types.DATE);

            if (pReference != null)
                statement.setString(8, pReference);
            else
                statement.setNull(8, Types.VARCHAR);

            statement.setNull(9, Types.DOUBLE);
            statement.setString(10, pSecondCheckFlag);
//IF Stub
            //statement.setDouble(11, pRqmt2ndChkRuleId);
            statement.executeUpdate();

        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) {
            }
            statement = null;
        }
        return nextSeqNo;
    }

    public String getInitialStatus(String pRqmtCode)
            throws SQLException {
        String intialStatus = "";
        String callSqlStatement = "";
        callSqlStatement = "{? = call ops_tracking.pkg_trade_rqmt.f_get_initial_status(?) }";
        OracleCallableStatement statement = null;
        statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
        statement.registerOutParameter(1, oracle.jdbc.OracleTypes.VARCHAR);
        statement.setString(2, pRqmtCode);
        statement.execute();
        intialStatus = statement.getString(1);
        statement.close();
        statement = null;
        return intialStatus;
    }

    public void updateTradeRqmt(double pRqmtID, String pNewStatus, java.sql.Date pCompletedDate, String pCmt )
            throws SQLException {
        String stringDate = "";
        stringDate = sdfSPDate.format(pCompletedDate);
        OracleCallableStatement statement;
        //String callSqlStatement = "{call ops_tracking.PKG_TRADE_RQMT.p_update_trade_rqmt(?, ?, ?, ?) }";
        String callSqlStatement = "{call ops_tracking.PKG_TRADE_RQMT.p_update_mult_trade_rqmts(?,?,?,?,?,?,?,?,?) }";
        statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
        statement.setDouble(1, pRqmtID);
        statement.setString(2, pNewStatus);
        statement.setString(3, stringDate);
        statement.setString(4, "");
        statement.setString(5, pCmt);
        statement.setString(6, "Y");
        statement.setString(7, "Y");
        statement.setString(8, "N");
        statement.setString(9, "Y");
        statement.executeQuery();
        statement.close();
        statement = null;
    }

        public void updateRqmts(double pTradeID, String pRqmtCode, String pRqmtStatus,
                                java.sql.Date pCompletedDate, String pReference, String pCmt )
            throws SQLException {
        String updateRqmtStatusFlag = (pRqmtStatus == null || pRqmtStatus == "") ? "N" : "Y";
        String updateCompletedDtFlag = pCompletedDate == null ? "N" : "Y";
        String updateReferenceFlag = (pReference == null || pReference == "") ? "N" : "Y";
        String updateCmtFlag = (pCmt == null || pCmt == "") ? "N" : "Y";
        String stringDate = "";
        if (updateCompletedDtFlag.equalsIgnoreCase("Y"))
            stringDate = sdfSPDate.format(pCompletedDate);
        OracleCallableStatement statement;
        String callSqlStatement = "{call ops_tracking.PKG_TRADE_RQMT.p_update_rqmts(?,?,?,?,?,?,?,?,?,?) }";
        statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
        statement.setDouble(1, pTradeID);
        statement.setString(2, pRqmtCode);
        statement.setString(3, pRqmtStatus);
        statement.setString(4, stringDate);
        statement.setString(5, pReference);
        statement.setString(6, pCmt);
        statement.setString(7, updateRqmtStatusFlag);
        statement.setString(8, updateCompletedDtFlag);
        statement.setString(9, updateReferenceFlag);
        statement.setString(10, updateCmtFlag);
        statement.executeQuery();
        statement.close();
        statement = null;
    }

    /*public void updateSecondCheckFlag(double pTradeId, String pRqmt, String pSecondCheckFlag)
            throws SQLException {
        PreparedStatement statement = null;
        try {
            String updateSQL =
                    "update ops_tracking.TRADE_RQMT " +
                    "set second_check_flag = ? " + //1
                    "where trade_id = ? " + //2
                    "and rqmt = ?";  //3

            statement = opsTrackingConnection.prepareStatement(updateSQL);
            statement.setString(1, pSecondCheckFlag);
            statement.setDouble(2, pTradeId);
            statement.setString(3, pRqmt);
            statement.executeUpdate();

        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) {
            }
            statement = null;
        }
    }*/

    public void ReconcileRqmtLists(double pTradeId) throws SQLException {
        CachedRowSetImpl crs = new CachedRowSetImpl();
        crs = getTradeRqmts(pTradeId);
        String rqmtCode = "";
        String status = "";

        //Prevents creation of duplicate requirements.
        crs.beforeFirst();
        while (crs.next()) {
            rqmtCode = crs.getString("RQMT");
            status = crs.getString("STATUS");
            //If already exists...
            if ( rqmtList.indexOf(rqmtCode) > -1)
            {
                //Reopen if cancelled
                if (status.equalsIgnoreCase("CXL"))
                {
                    String initialStatus = rqmtDAO.getInitialStatus(rqmtCode);
                    double rqmtId = crs.getDouble("ID");
                    java.sql.Date reopenDt = new java.sql.Date(System.currentTimeMillis());
                    updateTradeRqmt(rqmtId, initialStatus, reopenDt, "Reopened requirement due to trade correction.");
                }
                //Remove to prevent dup from being created.
                removeRqmtListEntry(rqmtCode);
            }
        }
    }

    public boolean isTradeRqmtIntitalStatus(double pTradeId, String pRqmtCode) throws SQLException {
        int rowsFound = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String statementSQL = "";
            statementSQL = "select count(*) cnt " +
                    "from ops_tracking.trade_rqmt r, " +
                    "ops_tracking.v_rqmt_status v " +
                    "where r.trade_id = ? " +
                    "and r.rqmt = ?  " +
                    "and r.rqmt = v.rqmt_code " +
                    "and r.status = v.status_code  " +
                    "and r.status = v.initial_status ";

            statement = opsTrackingConnection.prepareStatement(statementSQL);
            statement.setDouble(1, pTradeId);
            statement.setString(2, pRqmtCode);
            rs = statement.executeQuery();
            if (rs.next()) {
                rowsFound = (rs.getInt("cnt"));
            }
        } finally {
            if (statement != null) {
                statement.close();
                statement = null;
            }
            if (rs != null) {
                rs.close();
                rs = null;
            }
        }

        return rowsFound == 1;
    }

    public String GetRqmtDisplayName(String pRqmtCode) throws SQLException {
        String displayName = "";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String statementSQL = "";
            statementSQL = "select display_text from ops_tracking.v_rqmt " +
                    "where code = ? ";

            statement = opsTrackingConnection.prepareStatement(statementSQL);
            statement.setString(1, pRqmtCode);
            rs = statement.executeQuery();
            if (rs.next()) {
                displayName = (rs.getString("display_text"));
            }
        } finally {
            if (statement != null) {
                statement.close();
                statement = null;
            }
            if (rs != null) {
                rs.close();
                rs = null;
            }
        }

        return displayName;
    }

    public void deleteRqmt(double pTradeId, String pRqmt) throws SQLException {
        if (pRqmt.equalsIgnoreCase(OUR_PAPER)){
            deleteTradeRqmtConfirmCreator((pTradeId));
            deleteTradeRqmtConfirm(pTradeId);
        }

        PreparedStatement stmt = null;
        String sql = "delete from ops_tracking.trade_rqmt where trade_id = ? and rqmt = ?";

        try {
            stmt =  opsTrackingConnection.prepareStatement(sql);
            stmt.setDouble(1, pTradeId);
            stmt.setString(2, pRqmt);
            stmt.executeUpdate();
        }
        catch (SQLException e){
            Logger.getLogger(this.getClass()).error("Error deleting trade_rqmt:" + e.getMessage());
        }
        finally {
            try {
                if (stmt != null) {
                    stmt.close();
                }
            }
            catch (SQLException e) {}
        }
    }

    public void deleteTradeRqmtConfirm(double pTradeId){
        PreparedStatement stmt = null;
        String sql = "delete from ops_tracking.trade_rqmt_confirm where trade_id = ?";

        try {
            stmt =  opsTrackingConnection.prepareStatement(sql);
            stmt.setDouble(1, pTradeId);
            stmt.executeUpdate();
        }
        catch (SQLException e){
            Logger.getLogger(this.getClass()).error("Error deleting trade_rqmt_confirm: " + e.getMessage());
        }
        finally {
            try {
                if (stmt != null) {
                    stmt.close();
                }
            }
            catch (SQLException e) {}
        }
    }

    public void deleteTradeRqmtConfirmCreator(double pTradeId){
        PreparedStatement stmt = null;
        String sql = "delete from ops_tracking.trade_rqmt_confirm_creator " +
                "where rqmt_confirm_id = " +
                "(select rqmt_confirm_id " +
                "from ops_tracking.trade_rqmt_confirm cnf, " +
                "ops_tracking.trade_rqmt_confirm_creator cr " +
                "where cnf.trade_id = ? " +
                "and cnf.id = cr.rqmt_confirm_id)  ";

        try {
            stmt =  opsTrackingConnection.prepareStatement(sql);
            stmt.setDouble(1, pTradeId);
            stmt.executeUpdate();
        }
        catch (SQLException e){
            Logger.getLogger(this.getClass()).error("Error deleting trade_rqmt_confirm_creator: " + e.getMessage());
        }
        finally {
            try {
                if (stmt != null) {
                    stmt.close();
                }
            }
            catch (SQLException e) {}
        }
    }

    public void cancelRqmt(double pTradeId, String pRqmt, String pCmt) throws SQLException {
        //1. Does a requirement with the initial status exist?
        double rqmtId = 0;
        rqmtId = getRqmtId(pTradeId,pRqmt,false);

        //2. If so, then cancel it.
        if (rqmtId > 0){
            String newStatus = "CXL";
            java.sql.Date cancelDt;
            cancelDt = new java.sql.Date(System.currentTimeMillis());
            //String reference = "CHANGED TO ECONF";
            updateTradeRqmt(rqmtId, newStatus, cancelDt, pCmt);
        }
    }

    public void CancelRqmt(double pTradeId, String pRqmt, String pCmt, boolean pCheckInitStatus) throws SQLException {
        //1. Does a requirement with the initial status exist?
        double rqmtId = 0;
        rqmtId = getRqmtId(pTradeId,pRqmt,pCheckInitStatus);

        //2. If so, then cancel it.
        if (rqmtId > 0){
            String newStatus = "CXL";
            java.sql.Date cancelDt;
            cancelDt = new java.sql.Date(System.currentTimeMillis());
            updateTradeRqmt(rqmtId, newStatus, cancelDt, pCmt);
        }
    }

    public void CancelAllRqmts(double pTradeId, String pCmt)
            throws SQLException {
        OracleCallableStatement statement;
        String callSqlStatement = "{call ops_tracking.PKG_TRADE_RQMT.p_cancel_all_rqmts(?,?) }";
        statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
        statement.setDouble(1, pTradeId);
        statement.setString(2, pCmt);
        statement.executeUpdate();
        statement.close();
        statement = null;
    }

    public String getRqmtsAsString(){
        String rqmts = "";
        String rqmt = "";
        int index = 0;

        for (Iterator i = rqmtList.iterator(); i.hasNext();){
            rqmt = "";
            rqmt = i.next().toString();
            if (index == 0)
                rqmts = rqmt;
            else
                rqmts = rqmts + "," + rqmt;
            index++;
        }
        return rqmts;
    }

    public boolean IsAutoConfirm (double pTradeId) throws Exception {
        boolean isAutoConfirm = false;
        CachedRowSetImpl crs = new CachedRowSetImpl();
        crs = getTradeRqmts(pTradeId);
        String rqmtCode = "";
        String status = "";

        crs.beforeFirst();
        while (crs.next()) {
            rqmtCode = crs.getString("RQMT");
            status = crs.getString("STATUS");

            if ((rqmtCode.equalsIgnoreCase("ECONF") ||
                 rqmtCode.equalsIgnoreCase("ECBKR") ||
                 rqmtCode.equalsIgnoreCase("EFET")  ||
                 rqmtCode.equalsIgnoreCase("EFBKR") ) &&
                 !status.equalsIgnoreCase("CXL"))
                isAutoConfirm = true;
        }
        return isAutoConfirm;
    }

    private CachedRowSetImpl getTradeRqmts(double pTradeId) throws SQLException {
        PreparedStatement statement = null;
        ResultSet rs = null;
        CachedRowSetImpl crs = null;
        crs = new CachedRowSetImpl();
        try {
            String statementSQL = "";
            statementSQL = "select * from ops_tracking.trade_rqmt where trade_id = ?";
            statement = opsTrackingConnection.prepareStatement(statementSQL);
            statement.setDouble(1, pTradeId);
            rs = statement.executeQuery();
            crs.populate(rs);
        } finally {
            if (statement != null) {
                statement.close();
                statement = null;
            }
            if (rs != null) {
                rs.close();
                rs = null;
            }
        }
        return crs;
    }

    public boolean isEConfPrep(double pTradeId) throws SQLException {
        int rowsFound = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String statementSQL = "";
            statementSQL = "select count(*) count from ops_tracking.trade_rqmt where rqmt = 'ECONF' " +
                           "and status = 'PREP' and trade_id = ?";
            statement = opsTrackingConnection.prepareStatement(statementSQL);
            statement.setDouble(1, pTradeId);
            rs = statement.executeQuery();
            if (rs.next()) {
                rowsFound = (rs.getInt("count"));
            }
        } finally {
            if (statement != null) {
                statement.close();
                statement = null;
            }
            if (rs != null) {
                rs.close();
                rs = null;
            }
        }

        return rowsFound == 1;
    }

    public boolean isBkrEConfPrep(double pTradeId) throws SQLException {
        int rowsFound = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String statementSQL = "";
            statementSQL = "select count(*) count from ops_tracking.trade_rqmt where rqmt = 'ECBKR' " +
                           "and status = 'PREP' and trade_id = ?";
            statement = opsTrackingConnection.prepareStatement(statementSQL);
            statement.setDouble(1, pTradeId);
            rs = statement.executeQuery();
            if (rs.next()) {
                rowsFound = (rs.getInt("count"));
            }
        } finally {
            if (statement != null) {
                statement.close();
                statement = null;
            }
            if (rs != null) {
                rs.close();
                rs = null;
            }
        }

        return rowsFound == 1;
    }

    public boolean hasBrokerRqmt(double pTradeId) throws SQLException {
        int rowsFound = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String statementSQL = "";
            statementSQL = "select count(*) count from ops_tracking.trade_rqmt where rqmt = 'XQBBP' " +
                           "and status <> 'CXL' and trade_id = ?";
            statement = opsTrackingConnection.prepareStatement(statementSQL);
            statement.setDouble(1, pTradeId);
            rs = statement.executeQuery();
            if (rs.next()) {
                rowsFound = (rs.getInt("count"));
            }
        } finally {
            if (statement != null) {
                statement.close();
                statement = null;
            }
            if (rs != null) {
                rs.close();
                rs = null;
            }
        }

        return rowsFound == 1;
    }

    public boolean HasRqmt(double pTradeId, String pRqmt) throws SQLException {
        int rowsFound = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String statementSQL = "";
            statementSQL = "select count(*) count from ops_tracking.trade_rqmt where rqmt = ? " +
                           "and status <> 'CXL' and trade_id = ?";
            statement = opsTrackingConnection.prepareStatement(statementSQL);
            statement.setString(1, pRqmt);
            statement.setDouble(2, pTradeId);
            rs = statement.executeQuery();
            if (rs.next()) {
                rowsFound = (rs.getInt("count"));
            }
        } finally {
            if (statement != null) {
                statement.close();
                statement = null;
            }
            if (rs != null) {
                rs.close();
                rs = null;
            }
        }

        return rowsFound > 0;
    }

    public boolean hasClickAndConfirmVerbalRqmt(double pTradeId) throws SQLException {
        int rowsFound = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String statementSQL = "";
            statementSQL = "select count(*) count from ops_tracking.trade_rqmt where rqmt = 'VBCP' " +
                           "and status <> 'CXL' and trade_id = ? and reference = ? ";
            statement = opsTrackingConnection.prepareStatement(statementSQL);
            statement.setDouble(1, pTradeId);
            statement.setString(2, EC_CLICK_AND_CONF_REF);
            rs = statement.executeQuery();
            if (rs.next()) {
                rowsFound = (rs.getInt("count"));
            }
        } finally {
            if (statement != null) {
                statement.close();
                statement = null;
            }
            if (rs != null) {
                rs.close();
                rs = null;
            }
        }

        return rowsFound == 1;
    }

    public boolean isEFETPrep(double pTradeId) throws SQLException {
        int rowsFound = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String statementSQL = "";
            statementSQL = "select count(*) count from ops_tracking.trade_rqmt where rqmt = 'EFET' " +
                           //"and status = 'PREP' and trade_id = ?";
                           "and status in ('QUEUE','PREP','SENT','ERROR','FAIL') and trade_id = ?";
            statement = opsTrackingConnection.prepareStatement(statementSQL);
            statement.setDouble(1, pTradeId);
            rs = statement.executeQuery();
            if (rs.next()) {
                rowsFound = (rs.getInt("count"));
            }
        } finally {
            if (statement != null) {
                statement.close();
                statement = null;
            }
            if (rs != null) {
                rs.close();
                rs = null;
            }
        }

        return rowsFound == 1;
    }

    private double getRqmtId(double pTradeId, String pRqmt, boolean pTestInitialStatus)
            throws SQLException {
        PreparedStatement statement = null;
        ResultSet rs = null;
        double rqmtId = 0;
        try {
            String sql;
            sql = getRqmtIdSql(pTestInitialStatus);
            statement = opsTrackingConnection.prepareStatement(sql);
            statement.setDouble(1,pTradeId);
            DAOUtils.setStatementString(2, pRqmt, statement);
            rs = statement.executeQuery();
            if (rs.next()) {
                rqmtId = (rs.getDouble("id"));
            }
        } finally {
            if (statement != null) {
                statement.close();
                statement = null;
            }
            if (rs != null) {
                rs.close();
                rs = null;
            }
        }
        return rqmtId;
    }

    private String getRqmtIdSql(boolean pTestInitialStatus) {
        String statement = "";
        if (pTestInitialStatus)
            statement = "select id from ops_tracking.trade_rqmt tr " +
                    "where trade_id = ? " +
                    "and rqmt = ? "  +
                    "and status in " +
                    "(select initial_status " +
                    "from ops_tracking.rqmt r " +
                    "where r.code = tr.rqmt)";
        else
            statement = "select id from ops_tracking.trade_rqmt tr " +
                    "where trade_id = ? " +
                    "and rqmt = ? ";
        return statement;
    }

    public String isSecondCheck(TradingSystemDATA_rec pTSData_rec, String pRqmtCode, String pRqmtList)
            throws SQLException {
        String isSecondCheck = "N";
        OracleCallableStatement statement = null;
        // 4-2-08:  MThoresen: Adding Location SN to param list..
        String callSqlStatement = "{? = call ops_tracking.pkg_second_check.f_is_second_check(?,?,?,?,?,?,?,?,?,?,?,?,?)}";
        statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
        statement.registerOutParameter(1, oracle.jdbc.OracleTypes.VARCHAR);
        DAOUtils.setStatementString(2, pRqmtCode, statement);
        DAOUtils.setStatementString(3, pTSData_rec.SE_CPTY_SN, statement);
        DAOUtils.setStatementString(4, pTSData_rec.CPTY_SN, statement);
        DAOUtils.setStatementString(5, pTSData_rec.CDTY_CODE, statement);
        DAOUtils.setStatementString(6, pTSData_rec.STTL_TYPE, statement);
        DAOUtils.setStatementString(7, pTSData_rec.TRADE_TYPE_CODE, statement);
        DAOUtils.setStatementString(8, pTSData_rec.BROKER_SN, statement);
        DAOUtils.setStatementString(9, pTSData_rec.getBUY_SELL_IND(), statement);
        DAOUtils.setStatementString(10, pTSData_rec.START_DT_AsString(), statement);
        DAOUtils.setStatementString(11, pTSData_rec.END_DT_AsString(), statement);
        DAOUtils.setStatementString(12, pTSData_rec.BOOK, statement);
        DAOUtils.setStatementString(13, pTSData_rec.getLOCATION_SN(), statement);
        DAOUtils.setStatementString(14, pRqmtList, statement);
        statement.execute();
        isSecondCheck = statement.getString(1);
        statement.close();
        statement = null;
        return isSecondCheck;
    }

    /*public String isSecondCheckBase(TradingSystemDATA_rec pTSData_rec, String pRqmtCode) throws SQLException {
        String isSecondCheck = "N";
        OracleCallableStatement statement = null;
        String callSqlStatement = "{? = call ops_tracking.is_second_check_base(?,?,?,?,?) }";
        statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
        statement.registerOutParameter(1, oracle.jdbc.driver.OracleTypes.VARCHAR);
        DAOUtils.setStatementString(2, pRqmtCode, statement);
        DAOUtils.setStatementString(3, pTSData_rec.SE_CPTY_SN, statement);
        DAOUtils.setStatementString(4, pTSData_rec.CPTY_SN, statement);
        DAOUtils.setStatementString(5, pTSData_rec.CDTY_CODE, statement);
        DAOUtils.setStatementString(6, pTSData_rec.STTL_TYPE, statement);
        statement.execute();
        isSecondCheck = statement.getString(1);
        statement.close();
        statement = null;
        return isSecondCheck;
    }

    public String isSecondCheckBroker(TradingSystemDATA_rec pTSData_rec, String pRqmtList) throws SQLException {
        String isSecondCheck = "N";
        OracleCallableStatement statement = null;
        String callSqlStatement = "{? = call ops_tracking.is_second_check_broker(?,?,?,?) }";
        statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
        statement.registerOutParameter(1, oracle.jdbc.driver.OracleTypes.VARCHAR);
        DAOUtils.setStatementString(2, pTSData_rec.BROKER_SN, statement);
        DAOUtils.setStatementString(3, pTSData_rec.START_DT_AsString(), statement);
        DAOUtils.setStatementString(4, pTSData_rec.END_DT_AsString(), statement);
        DAOUtils.setStatementString(5, pRqmtList, statement);
        statement.execute();
        isSecondCheck = statement.getString(1);
        statement.close();
        statement = null;
        return isSecondCheck;
    }*/

    //5/23/08 Israel - Made public for OpsDashboard changes.
    public String is3DayPower(TradingSystemDATA_rec pTSData_rec) throws SQLException {
        String is3DayPower = "N";
        OracleCallableStatement statement = null;
        String callSqlStatement = "{? = call ops_tracking.pkg_det_act.is_3day_power(?,?,?,?,?,?,?,?,?) }";
        statement = (OracleCallableStatement) affinityConnection.prepareCall(callSqlStatement);
        statement.registerOutParameter(1, oracle.jdbc.OracleTypes.VARCHAR);
        DAOUtils.setStatementString(2, pTSData_rec.INCEPTION_DT_AsString(), statement);
        DAOUtils.setStatementString(3, pTSData_rec.TRADE_DT_AsString(), statement);
        DAOUtils.setStatementString(4, pTSData_rec.TRADE_TYPE_CODE, statement);
        DAOUtils.setStatementString(5, pTSData_rec.CPTY_SN, statement);
        DAOUtils.setStatementString(6, pTSData_rec.SE_CPTY_SN, statement);
        DAOUtils.setStatementString(7, pTSData_rec.CDTY_CODE, statement);
        DAOUtils.setStatementString(8, pTSData_rec.STTL_TYPE, statement);
        DAOUtils.setStatementString(9, pTSData_rec.START_DT_AsString(), statement);
        DAOUtils.setStatementString(10, pTSData_rec.END_DT_AsString(), statement);
        statement.execute();
        is3DayPower = statement.getString(1);
        statement.close();
        statement = null;
        return is3DayPower;
    }

    // MThoresen: 5-16-2007: Made public.  changes made for C&C: 1 day gass, broker = ICE...C&C is false
    public String is1DayGas(TradingSystemDATA_rec pTSData_rec) throws SQLException {
        String is1DayGas = "N";
        OracleCallableStatement statement = null;
        String callSqlStatement = "{? = call ops_tracking.pkg_det_act.is_1day_gas(?,?,?,?,?,?,?,?) }";
        statement = (OracleCallableStatement) affinityConnection.prepareCall(callSqlStatement);
        statement.registerOutParameter(1, oracle.jdbc.OracleTypes.VARCHAR);
        DAOUtils.setStatementString(2, pTSData_rec.INCEPTION_DT_AsString(), statement);
        DAOUtils.setStatementString(3, pTSData_rec.TRADE_TYPE_CODE, statement);
        DAOUtils.setStatementString(4, pTSData_rec.CPTY_SN, statement);
        DAOUtils.setStatementString(5, pTSData_rec.SE_CPTY_SN, statement);
        DAOUtils.setStatementString(6, pTSData_rec.CDTY_CODE, statement);
        DAOUtils.setStatementString(7, pTSData_rec.STTL_TYPE, statement);
        DAOUtils.setStatementString(8, pTSData_rec.START_DT_AsString(), statement);
        DAOUtils.setStatementString(9, pTSData_rec.END_DT_AsString(), statement);
        statement.execute();
        is1DayGas = statement.getString(1);
        statement.close();
        statement = null;
        return is1DayGas;
    }

    private CachedRowSetImpl getCwfRqmts(TradingSystemDATA_rec pVOpsTrackingData_rec, String pUpdateBusnDt)
            throws SQLException {
        OracleCallableStatement statement;
        //String callSqlStatement = "{call ops_tracking.pkg_det_act.p_get_cwf_rqmts(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,? ) }";
        String callSqlStatement = "{call ops_tracking.pkg_det_act.p_get_cwf_rqmts(?,?,?,?,?,?,?,?,?,?,?,?,?,?,? ) }";
        statement = (OracleCallableStatement) affinityConnection.prepareCall(callSqlStatement);
        // 5/8/07 - Added for EFS support
        // Make sure there are no NullPointer surprises.
        try {
            if (pVOpsTrackingData_rec.EFS_FLAG.equalsIgnoreCase("Y") && pVOpsTrackingData_rec.EFS_CPTY_SN.length() > 0)
                DAOUtils.setStatementString(1, pVOpsTrackingData_rec.EFS_CPTY_SN, statement);
            else
                DAOUtils.setStatementString(1, pVOpsTrackingData_rec.SE_CPTY_SN, statement);
        }
        catch (NullPointerException e) {
            DAOUtils.setStatementString(1, pVOpsTrackingData_rec.SE_CPTY_SN, statement);
        }
        DAOUtils.setStatementString(2, pVOpsTrackingData_rec.CPTY_SN, statement);
        DAOUtils.setStatementString(3, pVOpsTrackingData_rec.BROKER_SN, statement);
        DAOUtils.setStatementString(4, pVOpsTrackingData_rec.TRADE_TYPE_CODE, statement);
        DAOUtils.setStatementString(5, pVOpsTrackingData_rec.STTL_TYPE, statement);
        DAOUtils.setStatementString(6, pVOpsTrackingData_rec.CDTY_CODE, statement);
        DAOUtils.setStatementString(7, pVOpsTrackingData_rec.REF_SN, statement);
        statement.setString(8, pVOpsTrackingData_rec.getBUY_SELL_IND());
        statement.setString(9, pVOpsTrackingData_rec.getLOCATION_SN());
        DAOUtils.setStatementString(10, pVOpsTrackingData_rec.CDTY_GRP_CODE, statement);
        DAOUtils.setStatementString(11, pVOpsTrackingData_rec.TRADE_DT_AsString(), statement);
        DAOUtils.setStatementString(12, pVOpsTrackingData_rec.START_DT_AsString(), statement);
        DAOUtils.setStatementString(13, pVOpsTrackingData_rec.END_DT_AsString(), statement);
        DAOUtils.setStatementString(14, pVOpsTrackingData_rec.BOOK, statement);
        //DAOUtils.setStatementString(15, pUpdateBusnDt, statement);
        statement.registerOutParameter(15, oracle.jdbc.OracleTypes.CURSOR);
        statement.executeQuery();
        ResultSet rs = null;
        CachedRowSetImpl crs = null;
        crs = new CachedRowSetImpl();
        rs = statement.getCursor(15);
        crs.populate(rs);
        statement.close();
        statement = null;
        rs.close();
        rs = null;
        return crs;
    }

    private void clearRqmtListEntries(){
        rqmtList.clear();
        rqmtRefList.clear();
    }

    private void addRqmtListEntry(String pRqmt) {
        String reference = "";
        rqmtList.add(pRqmt);
        rqmtRefList.add(reference);
    }

    private void addRqmtListEntry(String pRqmt, String pReference ) {
        String reference = "";
        if (pReference != null)
            reference = pReference;

        rqmtList.add(pRqmt);
        rqmtRefList.add(reference);
    }

    private void removeRqmtListEntry(String pRqmt) {
        int index = -2;
        index = rqmtList.indexOf(pRqmt);
        if (index > -1){
            rqmtList.remove(index);
            rqmtRefList.remove(index);
        }
    }

    private String translateRqmt(String pRqmt) {
           String newRqmt = null;
           if (pRqmt.equalsIgnoreCase("EBC"))
               newRqmt = "XQBBP";
           else if (pRqmt.equalsIgnoreCase("SCN"))
               newRqmt = "XQCSP";
           else if (pRqmt.equalsIgnoreCase("ECC"))
               newRqmt = "XQCCP";
           else if (pRqmt.equalsIgnoreCase("VRB"))
               newRqmt = "VBCP";
           else if (pRqmt.equalsIgnoreCase("*NRQ*"))
               newRqmt = "NOCNF";
           else if (pRqmt.equalsIgnoreCase("E+S"))
               newRqmt = "BOTH";
           else
               newRqmt = "?-" + pRqmt + "-?";
           return newRqmt;
       }

    //This is temporary - replace with a data call
    /*private String getInitialStatus( String pRqmt ){
        String initialStatus = null;
        if ( (pRqmt == "XQBBP") || (pRqmt == "XQCCP") )
            initialStatus = "EXPCT";
        else if ( (pRqmt == "XQCSP") || (pRqmt == "ECONF") )
            initialStatus = "PREP";
        else if (pRqmt == "DTACT")
            initialStatus = "OPEN";
        else if (pRqmt == "NOCNF")
            initialStatus = "APPR";
        else
            initialStatus = "*UNKNOWN*";
        return initialStatus;
    }*/

}
