package aff.confirm.common.efet.dao;

import aff.confirm.common.efet.datarec.EFETErrorLog_DataRec;
import aff.confirm.common.efet.datarec.EFETSubmitLog_DataRec;
import aff.confirm.common.efet.datarec.EFETTradeSummary_DataRec;
import aff.confirm.common.efet.datarec.EFETTrade_DataRec;
import aff.confirm.common.util.DAOUtils;
import com.sun.rowset.CachedRowSetImpl;
import oracle.jdbc.OracleCallableStatement;
import org.jboss.logging.Logger;

import javax.sql.rowset.CachedRowSet;
import java.sql.*;
import java.text.DecimalFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;

//import sun.jdbc.rowset.CachedRowSet;

/**
 * User: ifrankel
 * Date: Jul 25, 2003
 * Time: 7:34:11 AM
 * To change this template use Options | File Templates.
 */
public class EFET_DAO {
    static final public String BFI = "BFI";
    static final public String CNF = "CNF";
    static final public String REJ = "REJ";
    static final public String MSU = "MSU";
    static final public String MSA = "MSA";
    static final public String MSR = "MSR";
    static final public String CAN = "CAN";
    static final public String ACK = "ACK";

    static public final String EMISSION_PHASE_1 = "EUAPhase_1";
    static public final String EMISSION_PHASE_2 = "EUAPhase_2";
    static public final String EMISSION_PHASE_3 = "EUAPhase_3";

    static public final SimpleDateFormat sdfEfet = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss");
    //static final public String SEMPRA_EIC = "11XSEMPRA------0";
    private java.sql.Connection opsTrackingConnection;
    private java.sql.Connection affinityConnection;
    private java.sql.Connection efetConnection;
    private EFETAgreement_DAO efetAgreementDAO;
    private EFETCptyEIC_DAO efetCptyEICDAO;
    //private MailUtils mailUtils;
    //private String sentFromName;
    //private String sentFromAddress;
    private DecimalFormat df = new DecimalFormat("#0");


    public EFET_DAO(java.sql.Connection pOpsTrackingConnection,
                    java.sql.Connection pAffinityConnection
    ) throws SQLException, Exception {
        this.opsTrackingConnection = pOpsTrackingConnection;
        this.affinityConnection = pAffinityConnection;
        efetAgreementDAO = new EFETAgreement_DAO(opsTrackingConnection);
        efetCptyEICDAO = new EFETCptyEIC_DAO(opsTrackingConnection);
    }

    public EFET_DAO(java.sql.Connection pOpsTrackingConnection,
                    java.sql.Connection pAffinityConnection,
                    java.sql.Connection pEfetConnection) throws SQLException, Exception {
        this.opsTrackingConnection = pOpsTrackingConnection;
        this.affinityConnection = pAffinityConnection;
        this.efetConnection = pEfetConnection;
        efetAgreementDAO = new EFETAgreement_DAO(opsTrackingConnection);
        efetCptyEICDAO = new EFETCptyEIC_DAO(opsTrackingConnection);
    }

    static public String getTrdSysCode(String pDocumentId) throws Exception {
        String trdSysCode = "";
        int startChar = 0;
        startChar = pDocumentId.indexOf("_", 5);
        trdSysCode = pDocumentId.substring(startChar + 1, startChar + 2);
        if (trdSysCode.equalsIgnoreCase("A"))
            trdSysCode = "AFF";
        else if (trdSysCode.equalsIgnoreCase("J"))
            trdSysCode = "SYM";
        //let the code that calls this handle an invalid tradingSystemCode
        /*else
            throw new Exception("getTrdSysCode: trdSysCode=" + trdSysCode + " not recognized for DocumentId="+pDocumentId);*/
        return trdSysCode;
    }

    static public String getTradeId(String pDocumentId, String pTrdSysCode) {
        String tradeID = "";
        String tradeSysCode = pTrdSysCode.substring(0,1);
        int startChar = 0;
        int endChar = 0;
        //IF 6/29/2006 - Replaced A- with A
        //variable offset supports accessing old doc ids
        //startChar = pDocumentId.indexOf("-",11);
        int offset = 2;
        if (pDocumentId.charAt(12)== '-' )
            offset = 3;
        startChar = pDocumentId.lastIndexOf("_",20);
        endChar = pDocumentId.indexOf("@");
        tradeID = pDocumentId.substring(startChar + offset, endChar);
        return tradeID;
    }

    static public String getCptyDocId( String pDocumentId ){
        String cptyDocId = "";
        int startChar = 0;
        int endChar = 0;
        startChar = pDocumentId.indexOf("_",5);
        endChar = pDocumentId.indexOf("@");
        cptyDocId = pDocumentId.substring(startChar + 1, endChar);
        return cptyDocId;
    }

    public String getDocumentID(String pDocType, String pTradingSystem, double pTradeId, String pSeCptySn )
            throws Exception {
        String documentId;
        final SimpleDateFormat sdfDocDate = new SimpleDateFormat("yyyyMMdd");
        //final SimpleDateFormat sdfDocDate = new SimpleDateFormat("yyMMdd-HHmmss");
        //final SimpleDateFormat sdfDocTime = new SimpleDateFormat("HHmmss");
        DecimalFormat dfLocal = new DecimalFormat("#0");
        dfLocal.setMinimumIntegerDigits(9);
        Date now = new Date();
        String eicCode = "";
        eicCode = efetCptyEICDAO.getEICCode(pSeCptySn);
        documentId = pDocType + "_" + sdfDocDate.format(now) + "_" + pTradingSystem.substring(0, 1) + //"-" +
                   dfLocal.format(pTradeId) + "@" + eicCode;

        return documentId;
    }

    public boolean isEFETCptySubmit(String pTradingSystemCode, double pTradeID)
            throws SQLException, Exception {
        EFETTrade_DataRec efetTradeDataRec;
        efetTradeDataRec = new EFETTrade_DataRec();
        efetTradeDataRec = getEFETTradeDataRec(pTradeID);

        boolean agreementOK = false;
        agreementOK = efetAgreementDAO.isAgreementExist(efetTradeDataRec.mstrCmpnyId,
                efetTradeDataRec.mstrCptyId, efetTradeDataRec.cdtyCode, efetTradeDataRec.tradeSttlTypeCode,
                efetTradeDataRec.locationSN, efetTradeDataRec.tradeDt, "C",efetTradeDataRec.tradeTypeCode);
        if (!agreementOK)
            return false;

        boolean efetOK = false;
        efetOK = isEFETCptyTrade(efetTradeDataRec);

        return efetOK;
    }

    public boolean isEFETBkrSubmit(String pTradingSystemCode, double pTradeID)
            throws SQLException, Exception {
        EFETTrade_DataRec efetTradeDataRec;
        efetTradeDataRec = new EFETTrade_DataRec();
        efetTradeDataRec = getEFETTradeDataRec(pTradeID);

        boolean agreementOK = false;
        agreementOK = efetAgreementDAO.isAgreementExist(efetTradeDataRec.mstrCmpnyId,
                efetTradeDataRec.mstrBrokerId, efetTradeDataRec.cdtyCode, efetTradeDataRec.tradeSttlTypeCode,
                efetTradeDataRec.locationSN, efetTradeDataRec.tradeDt, "B",efetTradeDataRec.tradeTypeCode);

        if (!agreementOK)
            return false;

        boolean efetOK = false;
        efetOK = isEFETCptyTrade(efetTradeDataRec);
        return efetOK;
    }

    private boolean isEFETCptyTrade(EFETTrade_DataRec pEFETTradeDataRec)
            throws Exception {
        boolean tradeTypeCodeOK = false;
        boolean stdSttl = false;
        boolean payFixed = false;
        boolean recFixed = false;
        boolean fixedOK = false;
        boolean serviceCodeOK = false;
        boolean efetCptyTrade = false;
        boolean optionTradeStyleCheck = true;

        if (!pEFETTradeDataRec.stdProductFlag.equalsIgnoreCase("Y"))
            efetCptyTrade = false;
        else {
          //  tradeTypeCodeOK = pEFETTradeDataRec.tradeTypeCode.equalsIgnoreCase("ENRGY");
            serviceCodeOK = (pEFETTradeDataRec.serviceCode.equalsIgnoreCase("FIRM"));

            payFixed = pEFETTradeDataRec.payCurve.equalsIgnoreCase("FIXED");
            recFixed = pEFETTradeDataRec.recCurve.equalsIgnoreCase("FIXED");
            fixedOK = (payFixed || recFixed);

            stdSttl = pEFETTradeDataRec.sttlPerInd.equalsIgnoreCase("M") || pEFETTradeDataRec.sttlPerInd.equalsIgnoreCase("D") ;

            if (pEFETTradeDataRec.tradeTypeCode.equalsIgnoreCase("OPEGY")) {
                // accept only european style options.
                optionTradeStyleCheck = "EURO".equalsIgnoreCase(pEFETTradeDataRec.optionStyleCode);
            }
           // efetCptyTrade = (tradeTypeCodeOK && serviceCodeOK && fixedOK && stdSttl);
             efetCptyTrade = (serviceCodeOK && fixedOK && stdSttl && optionTradeStyleCheck );
        }

        return efetCptyTrade;
    }


    public EFETTrade_DataRec getEFETTradeDataRec(double pTradeID)
            throws SQLException {
        EFETTrade_DataRec efetTradeDataRec = new EFETTrade_DataRec();
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = affinityConnection.prepareStatement("SELECT * from " +
                        "infinity_mgr." + "v_efet_trade_data where " +
                        "prmnt_trade_id = ? ");
            statement.setDouble(1, pTradeID);
            rs = statement.executeQuery();
            if (rs.next()) {
                efetTradeDataRec.prmntTradeId = rs.getDouble("PRMNT_TRADE_ID");
                efetTradeDataRec.companySN = rs.getString("COMPANY_SN");
                efetTradeDataRec.mstrCmpnyId = rs.getInt("MSTR_CMPNY_ID");
                efetTradeDataRec.cptySN = rs.getString("CPTY_SN");
                efetTradeDataRec.mstrCptyId = rs.getInt("MSTR_CPTY_ID");
                //Israel - New EFET broker match
                efetTradeDataRec.brokerSN = rs.getString("BROKER_SN");
                efetTradeDataRec.mstrBrokerId = rs.getInt("MSTR_BROKER_ID");

                efetTradeDataRec.tradeTypeCode = rs.getString("TRADE_TYPE_CODE");
                efetTradeDataRec.cdtyCode = rs.getString("CDTY_CODE");
                efetTradeDataRec.tradeSttlTypeCode = rs.getString("TRADE_STTL_TYPE_CODE");
                efetTradeDataRec.tradeStartDt = rs.getDate("TRADE_START_DT");
                efetTradeDataRec.tradeEndDt = rs.getDate("TRADE_END_DT");
                efetTradeDataRec.stdProductFlag = rs.getString("STD_PRODUCT_FLAG");
                efetTradeDataRec.payCurve = getStringWithoutNull( rs, "PAY_CURVE");
                efetTradeDataRec.payPriceModel = getStringWithoutNull( rs, "PAY_PRICE_MODEL");
                efetTradeDataRec.payMkt = getStringWithoutNull( rs, "PAY_MKT");
                efetTradeDataRec.payUom = getStringWithoutNull( rs, "PAY_UOM");
                efetTradeDataRec.payCcy = getStringWithoutNull( rs, "PAY_CCY");
                efetTradeDataRec.payExchRollInd = getStringWithoutNull( rs, "PAY_EXCH_ROLL_IND");
                efetTradeDataRec.recCurve = getStringWithoutNull( rs, "REC_CURVE");
                efetTradeDataRec.recPriceModel = getStringWithoutNull( rs, "REC_PRICE_MODEL");
                efetTradeDataRec.recMkt = getStringWithoutNull( rs, "REC_MKT");
                efetTradeDataRec.recUom = getStringWithoutNull( rs, "REC_UOM");
                efetTradeDataRec.recCcy = getStringWithoutNull( rs, "REC_CCY");
                efetTradeDataRec.recExchRollInd = getStringWithoutNull( rs, "REC_EXCH_ROLL_IND");
                efetTradeDataRec.qtyUomCode = rs.getString("QTY_UOM_CODE");
                efetTradeDataRec.sttlCcyCode = rs.getString("STTL_CCY_CODE");
                efetTradeDataRec.sttlPerInd = rs.getString("STTL_PER_IND");
                efetTradeDataRec.valDtModel = rs.getString("VAL_DT_MODEL");
                efetTradeDataRec.valDtMonths = rs.getInt("VAL_DT_MONTHS");
                efetTradeDataRec.valDtDays = rs.getInt("VAL_DT_DAYS");
                efetTradeDataRec.tradeDt = rs.getDate("TRADE_DT");
                efetTradeDataRec.locationSN = rs.getString("LOCATION_SN");
                efetTradeDataRec.qtyPerDurationCode = rs.getString("QTY_PER_DURATION_CODE");
                efetTradeDataRec.serviceCode = rs.getString("SERVICE_CODE");
                efetTradeDataRec.strikePriceModel = rs.getString("STRIKE_PRICE_MODEL");
                efetTradeDataRec.rfrnce = rs.getString("RFRNCE");
                efetTradeDataRec.optionStyleCode = rs.getString("OPTN_STYLE_CODE");

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
        return efetTradeDataRec;
    }

    private String getStringWithoutNull(ResultSet pRs, String pColumnName)
            throws SQLException {
        if (pRs.getString(pColumnName) != null)
            return pRs.getString(pColumnName);
        else
            return "NONE";
    }

    public int insertEfetSubmitLog(EFETSubmitLog_DataRec pEFETSubmitLogDataRec) throws SQLException {
        PreparedStatement statement = null;
        int nextSeqNo = -1;
        try {
            String seqName = "efet.seq_efet_submit_log";
            nextSeqNo = getNextSequence(seqName);
            String insertSQL =
                    "Insert into efet.EFET_SUBMIT_LOG( " +
                    "ID, " + //1
                    "TRADING_SYSTEM, " + //2
                    "TRADE_ID, " + //3
                    "SUBMIT_TIMESTAMP_GMT, " + //
                    "STATUS_MESSAGE, " + //4
                    "ACTION," +  //5
                    "DOC_TYPE ) " + //6
                    "values( ?, ?, ?, sysdate, ?, ?, ? )";

            statement = opsTrackingConnection.prepareStatement(insertSQL);
            statement.setInt(1, nextSeqNo);
            DAOUtils.setStatementString(2, pEFETSubmitLogDataRec.tradingSystem, statement);
            DAOUtils.setStatementDouble(3, pEFETSubmitLogDataRec.tradeID, statement);
            DAOUtils.setStatementString(4, pEFETSubmitLogDataRec.statusMessage, statement);
            DAOUtils.setStatementString(5, pEFETSubmitLogDataRec.action, statement);
            DAOUtils.setStatementString(6, pEFETSubmitLogDataRec.docType, statement);
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


    public void updateEfetSubmitLog(int pID, String pStatusMessage)
            throws SQLException {
        PreparedStatement statement = null;
        try {
            String updateSQL =
                    "update efet.EFET_SUBMIT_LOG set " +
                    "STATUS_MESSAGE = ? " +
                    "where id = ?";

            statement = opsTrackingConnection.prepareStatement(updateSQL);
            statement.setString(1, pStatusMessage);
            statement.setInt(2, pID);
            statement.executeUpdate();

        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) {
            }
            statement = null;
        }
    }

    public CachedRowSet getEfetSummaryResubmit() throws SQLException {
        PreparedStatement statement = null;
        ResultSet rs = null;
        CachedRowSet crs;
        crs = new CachedRowSetImpl();
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT * from " +
                    "efet.EFET_TRADE_SUMMARY where OK_TO_RESUBMIT_IND <> 'N'");
            rs = statement.executeQuery();
            crs.populate(rs);
        } catch (SQLException e) {
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

    //************ DOCUMENT ***********************


    public int insertEfetDocument(double pTradeId, String pDocType, String pDocId, int pDocVersion) throws SQLException {
        PreparedStatement statement = null;
        int nextSeqNo = -1;
        try {
            String seqName = "efet.SEQ_DOCUMENT";
            nextSeqNo = getNextSequence(seqName);
            String insertSQL =
                    "Insert into efet.DOCUMENT( " +
                    "ID, " + //1
                    "TRADE_ID, " + //2
                    "DOC_TYPE, " + //3
                    "EFET_DOC_ID, " + //4
                    "EFET_DOC_VERSION ) " + //5
                    "values( ?, ?, ?, ?, ? )";

            statement = opsTrackingConnection.prepareStatement(insertSQL);
            statement.setInt(1, nextSeqNo);
            DAOUtils.setStatementDouble(2, pTradeId, statement);
            DAOUtils.setStatementString(3, pDocType, statement);
            DAOUtils.setStatementString(4, pDocId, statement);
            DAOUtils.setStatementInt(5, pDocVersion, statement);
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


    public void updateEfetDocumentVersion(double pTradeId, String pDocType, int pDocVersion)
            throws SQLException {
        PreparedStatement statement = null;
        try {
            String updateSQL =
                    "update efet.DOCUMENT set " +
                    "EFET_DOC_VERSION = ? " +
                    "where TRADE_ID = ? " +
                    " and DOC_TYPE = ?";

            statement = opsTrackingConnection.prepareStatement(updateSQL);
            statement.setInt(1, pDocVersion);
            statement.setDouble(2, pTradeId);
            statement.setString(3, pDocType);
            statement.executeUpdate();

        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) {
            }
            statement = null;
        }
    }

    public int getEfetDocumentVersion(double pTradeId, String pDocType) throws SQLException {
        PreparedStatement statement = null;
        ResultSet rs = null;
        int efetDocVersion = 0;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT efet_doc_version from " +
                    "efet.DOCUMENT where trade_id = ? " +
                    " and doc_type = ?");
            statement.setDouble(1, pTradeId);
            statement.setString(2, pDocType);
            rs = statement.executeQuery();
            while (rs.next()) {
                efetDocVersion = rs.getInt("efet_doc_version");
            }

        } catch (SQLException e) {
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
        return efetDocVersion;
    }

    public String getEfetDocumentId(double pTradeId, String pDocType) throws SQLException {
        PreparedStatement statement = null;
        ResultSet rs = null;
        String documentId = "NONE";
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT efet_doc_id from " +
                    "efet.DOCUMENT where trade_id = ? " +
                    " and doc_type = ?");
            statement.setDouble(1, pTradeId);
            statement.setString(2, pDocType);
            rs = statement.executeQuery();
            while (rs.next()) {
                documentId = rs.getString("efet_doc_id");
            }

        } catch (SQLException e) {
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
        return documentId;
    }


    public boolean isEfetDocumentExist(double pTradeID, String pDocType) throws SQLException {
        boolean recordExists = false;
        int count = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT count(*) cnt from " +
                    "efet.DOCUMENT " +
                    "where trade_id = ? " +
                    " and doc_type = ?");
            statement.setDouble(1, pTradeID);
            statement.setString(2, pDocType);
            rs = statement.executeQuery();
            if (rs.next()) {
                count = (rs.getInt("cnt"));
            }
            recordExists = count > 0;
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

        return recordExists;
    }
    
    public void createNewDocumentId(double pTradeID) throws SQLException {

        String sql ="{call efet.PKG_EXT_NOTIFY.p_new_document_id(?)}";
        CallableStatement statement = null;
        try {
            statement = this.opsTrackingConnection.prepareCall(sql);
            statement.setDouble(1,pTradeID);
            statement.execute();
        }
        finally {
            try {
                if (statement != null){
                    statement.close();
                }
            }
            catch (SQLException e){

            }
        }
        
       
    }

    //*******EFET_BOX.tradeconfirmation

    public int getTradeConfirmationDocumentVersion(String pDocumentId) throws SQLException {
        PreparedStatement statement = null;
        ResultSet rs = null;
        int boxResultDocVersion = 0;
        try {
            statement = efetConnection.prepareStatement("SELECT max(documentversion) max_version from " +
                    "efet_box.box_document where documentid = ? ");
            statement.setString(1, pDocumentId);
            rs = statement.executeQuery();
            while (rs.next()) {
                boxResultDocVersion = rs.getInt("max_version");
            }

        } catch (SQLException e) {
            Logger.getLogger(this.getClass()).error( "ERROR", e );

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

        return boxResultDocVersion;
    }



    //******* BROKER_FEE

    public int insertBrokerFee(double pTradeId, double pTotalFee, String pFeeCurrency) throws SQLException {
        PreparedStatement statement = null;
        int nextSeqNo = -1;
        try {
            String seqName = "efet.SEQ_BROKER_FEE";
            nextSeqNo = getNextSequence(seqName);
            String insertSQL =
                    "Insert into efet.BROKER_FEE( " +
                    "ID, " + //1
                    "TRADE_ID, " + //2
                    "TOTAL_FEE, " + //3
                    "FEE_CURRENCY ) " + //4
                    "values( ?, ?, ?, ? )";

            statement = opsTrackingConnection.prepareStatement(insertSQL);
            statement.setInt(1, nextSeqNo);
            DAOUtils.setStatementDouble(2, pTradeId, statement);
            DAOUtils.setStatementDouble(3, pTotalFee, statement);
            DAOUtils.setStatementString(4, pFeeCurrency, statement);
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

    public void updateBrokerFee(double pTradeId, double pTotalFee, String pFeeCurrency)
            throws SQLException {
        PreparedStatement statement = null;
        try {
            String updateSQL =
                    "update efet.BROKER_FEE set " +
                    "TOTAL_FEE = ?, " +
                    "FEE_CURRENCY = ?" +
                    "where TRADE_ID = ? ";

            statement = opsTrackingConnection.prepareStatement(updateSQL);
            statement.setDouble(1, pTotalFee);
            statement.setString(2, pFeeCurrency);
            statement.setDouble(3, pTradeId);
            statement.executeUpdate();

        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) {
            }
            statement = null;
        }
    }



    public boolean isBrokerFeeExist(double pTradeID) throws SQLException {
        boolean recordExists = false;
        int count = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT count(*) cnt from " +
                    "efet.BROKER_FEE " +
                    "where trade_id = ? ");
            statement.setDouble(1, pTradeID);
            rs = statement.executeQuery();
            if (rs.next()) {
                count = (rs.getInt("cnt"));
            }
            recordExists = count > 0;
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

        return recordExists;
    }


    public boolean isBrokerFeeChanged(double pTradeID, double pNewTotalFee, String pNewFeeCurrency)
            throws SQLException {
        boolean feeChanged = false;
        double oldTotalFee = 0;
        String oldFeeCurrency = "";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT total_fee, fee_currency from " +
                    "efet.BROKER_FEE " +
                    "where trade_id = ? ");
            statement.setDouble(1, pTradeID);
            rs = statement.executeQuery();
            if (rs.next()) {
                oldTotalFee = rs.getDouble("total_fee");
                oldFeeCurrency = rs.getString("fee_currency");
            }
            feeChanged = ((pNewTotalFee != oldTotalFee) ||
                           !pNewFeeCurrency.equalsIgnoreCase(oldFeeCurrency));
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

        return feeChanged;
    }


    //************ EFET_TRADE_SUMMARY

    public EFETTradeSummary_DataRec getEfetSummaryDataRec(double pTradeId, String pEntityType) throws SQLException {
        EFETTradeSummary_DataRec efetTradeSummaryDataRec;
        efetTradeSummaryDataRec = new EFETTradeSummary_DataRec();

        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT * from " +
                    "efet.EFET_TRADE_SUMMARY where trade_id = ?" +
                    " and entity_type = ?");
            statement.setDouble(1, pTradeId);
            statement.setString(2, pEntityType);
            rs = statement.executeQuery();
            while (rs.next()) {
                efetTradeSummaryDataRec.id = rs.getInt("id");
                efetTradeSummaryDataRec.tradingSystem = rs.getString("trading_system");
                efetTradeSummaryDataRec.tradeID = rs.getDouble("trade_id");
                efetTradeSummaryDataRec.status = rs.getString("efet_status");
                efetTradeSummaryDataRec.cmt = rs.getString("cmt");
                efetTradeSummaryDataRec.cptyTradeRefID = rs.getString("cpty_trade_ref_id");
                efetTradeSummaryDataRec.okToResubmitInd = rs.getString("ok_to_resubmit_ind");
                efetTradeSummaryDataRec.errorFlag = rs.getString("error_flag");
                efetTradeSummaryDataRec.senderId = rs.getString("sender_id");
                efetTradeSummaryDataRec.receiverId = rs.getString("receiver_id");
                efetTradeSummaryDataRec.entityType = rs.getString("entity_type");
            }

        } catch (SQLException e) {
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
        return efetTradeSummaryDataRec;
    }


    public void setEfetTradeSummaryOKToResubmit(double pTradeID, String pEntityType, String pOKToResubmitInd)
            throws SQLException {
        PreparedStatement statement = null;
        try {
            String updateSQL =
                    "update efet.EFET_TRADE_SUMMARY set " +
                    "OK_TO_RESUBMIT_IND = ? " +
                    "where trade_id = ? " +
                    " and entity_type = ?";

            statement = opsTrackingConnection.prepareStatement(updateSQL);
            statement.setString(1, pOKToResubmitInd);
            statement.setDouble(2, pTradeID);
            statement.setString(3, pEntityType);
            statement.executeUpdate();
        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) {
            }
            statement = null;
        }
    }

    public boolean isEfetTradeSummaryExist(double pTradeID, String pEntityType) throws SQLException {
        boolean recordExists = false;
        int count = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT count(*) cnt from " +
                    "efet.EFET_TRADE_SUMMARY " +
                    "where trade_id = ? " +
                    " and entity_type = ?");
            statement.setDouble(1, pTradeID);
            statement.setString(2, pEntityType);
            rs = statement.executeQuery();
            if (rs.next()) {
                count = (rs.getInt("cnt"));
            }
            recordExists = count > 0;
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

        return recordExists;
    }


    public int insertEfetTradeSummary(EFETTradeSummary_DataRec pEfetTradeSummaryDataRec)
            throws SQLException {
        PreparedStatement statement = null;
        int nextSeqNo = -1;
        try {
            String seqName = "efet.seq_efet_trade_summary";
            nextSeqNo = getNextSequence(seqName);
            String insertSQL =
                    "Insert into " + "efet.EFET_TRADE_SUMMARY( " +
                    "ID, " + //1
                    "TRADING_SYSTEM, " + //2
                    "TRADE_ID, " + //3
                    "EFET_STATUS, " + //4
                    "ERROR_FLAG," + //5
                    "LAST_UPDATE_TIMESTAMP_GMT, " + //sysdate
                    "CMT," + //6
                    "SENDER_ID," + //7
                    "RECEIVER_ID, " + //8
                    "ENTITY_TYPE ) " + //9
                    "values( ?, ?, ?, ?, ?, sysdate, ?, ?, ?, ? )";

            statement = opsTrackingConnection.prepareStatement(insertSQL);
            statement.setInt(1, nextSeqNo);
            DAOUtils.setStatementString(2, pEfetTradeSummaryDataRec.tradingSystem, statement);
            DAOUtils.setStatementDouble(3, pEfetTradeSummaryDataRec.tradeID, statement);
            DAOUtils.setStatementString(4, pEfetTradeSummaryDataRec.status, statement);
            DAOUtils.setStatementString(5, pEfetTradeSummaryDataRec.errorFlag, statement);
            statement.setNull(6, Types.VARCHAR);
            DAOUtils.setStatementString(7, pEfetTradeSummaryDataRec.senderId, statement);
            DAOUtils.setStatementString(8, pEfetTradeSummaryDataRec.receiverId, statement);
            DAOUtils.setStatementString(9, pEfetTradeSummaryDataRec.entityType, statement);
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


    public void updateEfetTradeSummary(double pTradeID, String pEfetStatus, String pErrorFlag,
                                       String pSenderId, String pReceiverId, String pEntityType)
            throws SQLException {
        PreparedStatement statement = null;
        try {
            String updateSQL =
                    "update efet.EFET_TRADE_SUMMARY set " +
                    "EFET_STATUS = ?," +  //1
                    "ERROR_FLAG = ?," + //2
                    "SENDER_ID = ?," + //3
                    "RECEIVER_ID = ?" + //4
                    "where trade_id = ? " + //5
                    "and entity_type = ?"; //6

            statement = opsTrackingConnection.prepareStatement(updateSQL);
            DAOUtils.setStatementString(1, pEfetStatus, statement);
            DAOUtils.setStatementString(2, pErrorFlag, statement);
            DAOUtils.setStatementString(3, pSenderId, statement);
            DAOUtils.setStatementString(4, pReceiverId, statement);
            DAOUtils.setStatementDouble(5, pTradeID, statement);
            DAOUtils.setStatementString(6, pEntityType, statement);
            statement.executeUpdate();

        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) {
            }
            statement = null;
        }
    }


    public void updateEfetTradeSummary(EFETTradeSummary_DataRec pEfetTradeSummaryDataRec) throws SQLException {
        PreparedStatement statement = null;
        try {
            String updateSQL =
                    "update " + "efet.EFET_TRADE_SUMMARY set " +
                    "EFET_STATUS = ?," +
                    "ERROR_FLAG = ?," +
                    //"CMT = ?," +
                    "CPTY_TRADE_REF_ID = ? " +
                    "where trade_id = ? " +
                    "and entity_type = ?";

            statement = opsTrackingConnection.prepareStatement(updateSQL);
            DAOUtils.setStatementString(1, pEfetTradeSummaryDataRec.status, statement);
            DAOUtils.setStatementString(2, pEfetTradeSummaryDataRec.errorFlag, statement);
            DAOUtils.setStatementString(3, pEfetTradeSummaryDataRec.cptyTradeRefID, statement);
            DAOUtils.setStatementDouble(4, pEfetTradeSummaryDataRec.tradeID, statement);
            DAOUtils.setStatementString(5, pEfetTradeSummaryDataRec.entityType, statement);
            statement.executeUpdate();

        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) {
            }
            statement = null;
        }
    }

    public String getEfetTradeSummaryStatus(double pTradeID, String pEntityType) throws SQLException {
        String ecStatus = "NONE";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT efet_status from "  +
                    "efet.EFET_TRADE_SUMMARY where trade_id = ?" +
                    "and entity_type = ?");
            statement.setDouble(1, pTradeID);
            statement.setString(2, pEntityType);
            rs = statement.executeQuery();
            if (rs.next()) {
                ecStatus = (rs.getString("efet_status"));
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

        return ecStatus;
    }


    public int insertEfetErrorLog(EFETErrorLog_DataRec pEfetErrorLogDataRec)
            throws SQLException {
        PreparedStatement statement = null;
        int nextSeqNo = -1;
        try {
            String seqName = "efet.seq_efet_error_log";
            nextSeqNo = getNextSequence(seqName);
            String insertSQL =
                    "Insert into " + "efet.EFET_ERROR_LOG( " +
                    "ID, " + //1
                    "TRADE_ID, " + //2
                    "EFET_STATE, " + //3
                    "EFET_TIMESTAMP, " + //4
                    "REASON_CODE, " + //5
                    "REASON_TEXT, " + //6
                    "DOC_ID, " + //7
                    "DOC_VERSION, " + //8
                    "EBXML_MESSAGE_ID, " + //9
                    "DOC_TYPE  ) " + //10
                    "values( ?, ?, ?, ?, ?, ?, ?, ?, ?, ? )";

            statement = opsTrackingConnection.prepareStatement(insertSQL);
            statement.setInt(1, nextSeqNo);
            DAOUtils.setStatementDouble(2, pEfetErrorLogDataRec.tradeID, statement);
            DAOUtils.setStatementString(3, pEfetErrorLogDataRec.efetState, statement);
            DAOUtils.setStatementString(4, pEfetErrorLogDataRec.efetTimestamp, statement);
            DAOUtils.setStatementString(5, pEfetErrorLogDataRec.reasonCode, statement);
            DAOUtils.setStatementString(6, pEfetErrorLogDataRec.reasonText, statement);
            DAOUtils.setStatementString(7, pEfetErrorLogDataRec.docId, statement);
            DAOUtils.setStatementString(8, pEfetErrorLogDataRec.docVersion, statement);
            DAOUtils.setStatementString(9, pEfetErrorLogDataRec.ebXmlMessageId, statement);
            DAOUtils.setStatementString(10, pEfetErrorLogDataRec.docType, statement);
            statement.executeUpdate();

        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) { }
            statement = null;
        }
        return nextSeqNo;
    }


    public boolean isTradeRqmtExist(double pTradeID, String pRqmtCode) throws SQLException {
        boolean recordExists = false;
        int count = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT count(*) cnt from " +
                    "ops_tracking.TRADE_RQMT " +
                    "where trade_id = ? " +
                    " and rqmt = ?" +
                    " and status <> 'CXL'");
            statement.setDouble(1, pTradeID);
            statement.setString(2, pRqmtCode);
            rs = statement.executeQuery();
            if (rs.next()) {
                count = (rs.getInt("cnt"));
            }
            recordExists = count > 0;
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
        return recordExists;
    }


    public void setNotifyOpsTrackingCptySubmit(double pTradeID) throws SQLException {
        //when record is inserted efet status is set to QUEUE
        //efet status is set to PREP
        OracleCallableStatement statement = null;
        String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_efet_cpty_submit(?) }";
        try {
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pTradeID);
            statement.execute();
        }
        finally {
            if (statement != null) {
                try {
                    statement.close();
                }
                catch (SQLException e){

                }
            }
        }
        statement = null;
    }

    public void setNotifyOpsTrackingCptyPending(double pTradeID) throws SQLException {
        //efet status is set to SENT
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_efet_cpty_pending(?) }";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pTradeID);
            statement.execute();
        }
        finally {
            if (statement != null) {
                try {
                    statement.close();
                }
                catch (SQLException e){
             }
             statement = null;
            }
        }

    }


    public void setNotifyOpsTrackingCptyMatched(double pTradeID, String pCptyTradeId, String pMatchedDate )
            throws SQLException {
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_efet_cpty_matched(?, ?, ?) }";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pTradeID);
            statement.setString(2, pCptyTradeId);
            statement.setString(3, pMatchedDate);
            statement.execute();
        }
        finally {
             if (statement != null) {
                try {
                    statement.close();
                }
                catch (SQLException e){

                }
            }
            statement = null;
        }

    }



    public void setNotifyOpsTrackingCptyUnmatched(double pTradeID, String pErrorMsg) throws SQLException {

        OracleCallableStatement statement = null;

        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_efet_cpty_unmatched(?, ?) }";
        //This was blowing up when there were too many broken fields.
            String cmt = "";
            if (pErrorMsg.trim().length() < 255)
                cmt = pErrorMsg;
            else
                cmt = pErrorMsg.substring(1,255);

            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pTradeID);
            statement.setString(2, cmt);
            statement.execute();
        }
        finally {
            if (statement != null) {
                try {
                    statement.close();
                }
                catch (SQLException e){

                }
            }
            statement = null;
        }
    }

    public void setNotifyOpsTrackingCptyError(double pTradeID, String pComment) throws SQLException {
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_efet_cpty_error(?, ?) }";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pTradeID);
            statement.setString(2, pComment);
            statement.executeQuery();
        }
        finally {
            if (statement != null) {
                try {
                    statement.close();
                }
                catch (SQLException e){

                }
            }
            statement = null;
        }
    }


    public void setNotifyOpsTrackingCptyFail(double pTradeID, String pComment) throws SQLException {
        OracleCallableStatement statement = null;

        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_efet_cpty_failed(?, ?) }";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pTradeID);
            statement.setString(2, pComment);
            statement.executeQuery();
        }
        finally {
         if (statement != null) {
                try {
                    statement.close();
                }
                catch (SQLException e){

                }
            }
            statement = null;
        }
    }

    public void setNotifyOpsTrackingCptyCancelled(double pTradeID) throws SQLException {
        OracleCallableStatement statement = null;

        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_efet_cpty_cancelled(?) }";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pTradeID);
            statement.execute();
        }
        finally {
            try {
                statement.close();
            }
            catch (SQLException e) {
                Logger.getLogger( this.getClass() ).error( "ERROR", e );
            }
            statement = null;
        }
    }


    public void setNotifyOpsTrackingBkrSubmit(double pTradeID) throws SQLException {
        //when record is inserted efet status is set to QUEUE
        //efet status is set to PREP
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_efet_bkr_submit(?) }";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pTradeID);
            statement.execute();
        }
        finally {
            if (statement != null) {
                try {
                    statement.close();
                }
                catch (SQLException e){

                }
            }
            statement = null;
        }

    }

    public void setNotifyOpsTrackingBkrPending(double pTradeID) throws SQLException {
        //efet status is set to SENT
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_efet_bkr_pending(?) }";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pTradeID);
            statement.executeQuery();
        }
        finally {
            if (statement != null) {
                try {
                    statement.close();
                }
                catch (SQLException e){

                }
            }
            statement = null;
        }

    }


    public void setNotifyOpsTrackingBkrValid(double pTradeID )
            throws SQLException {
        OracleCallableStatement statement =null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_efet_bkr_valid(?) }";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pTradeID);
            statement.execute();
        }
        finally {
            if (statement != null) {
                try {
                    statement.close();
                }
                catch (SQLException e){

                }
            }
            statement = null;
        }

    }


    public void setNotifyOpsTrackingBkrPrematched(double pTradeID )
            throws SQLException {
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_efet_bkr_prematched(?) }";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pTradeID);
            statement.execute();
        }
        finally {
            if (statement != null) {
                try {
                    statement.close();
                }
                catch (SQLException e){

                }
            }
            statement = null;
        }

    }


    public void setNotifyOpsTrackingBkrMatched(double pTradeID, String pCptyTradeId, String pMatchedDate )
            throws SQLException {
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_efet_bkr_matched(?, ?, ?) }";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pTradeID);
            statement.setString(2, pCptyTradeId);
            statement.setString(3, pMatchedDate);
            statement.execute();
        }
        finally {
            if (statement != null) {
                try {
                    statement.close();
                }
                catch (SQLException e){

                }
            }
            statement = null;
        }
    }


    public void setNotifyOpsTrackingBkrError(double pTradeID, String pComment) throws SQLException {
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_efet_bkr_error(?, ?) }";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pTradeID);
            statement.setString(2, pComment);
            statement.execute();
        }
        finally {
            if (statement != null) {
                try {
                    statement.close();
                }
                catch (SQLException e){

                }
            }
            statement = null;
        }

    }


    public void setNotifyOpsTrackingBkrFail(double pTradeID, String pComment) throws SQLException {
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_efet_bkr_failed(?, ?) }";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pTradeID);
            statement.setString(2, pComment);
            statement.execute();
        }
        finally {
            if (statement != null) {
                try {
                    statement.close();
                }
                catch (SQLException e){

                }
            }
            statement = null;
        }

    }

    public void setNotifyOpsTrackingBkrCancelled(double pTradeID) throws SQLException {
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_efet_bkr_cancelled(?) }";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pTradeID);
            statement.executeQuery();
        }
        finally{
            if (statement != null) {
                try {
                    statement.close();
                }
                catch (SQLException e){

                }
            }
            statement = null;
        }

    }



   private int getNextSequence(String seqName) throws SQLException {
        int nextSeqNo = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT " + seqName +
                    ".nextval from dual");
            rs = statement.executeQuery();
            if (rs.next()) {
                nextSeqNo = (rs.getInt("nextval"));
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
        return nextSeqNo;
    }


    public boolean isTradeRqmtCanceled(double pTradeID, String pRqmtCode) throws SQLException {
        boolean recordExists = false;
        int count = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT count(*) cnt from " +
                    "ops_tracking.TRADE_RQMT " +
                    "where trade_id = ? " +
                    " and rqmt = ?" +
                    " and status = 'CXL'");
            statement.setDouble(1, pTradeID);
            statement.setString(2, pRqmtCode);
            rs = statement.executeQuery();
            if (rs.next()) {
                count = (rs.getInt("cnt"));
            }
            recordExists = count > 0;
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
        return recordExists;
    }
}
