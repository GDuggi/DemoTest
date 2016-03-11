package aff.confirm.common.econfirm;

import aff.confirm.common.econfirm.datarec.*;
import aff.confirm.common.util.DAOUtils;
import com.sun.rowset.CachedRowSetImpl;
import oracle.jdbc.OracleCallableStatement;

import javax.sql.rowset.CachedRowSet;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Types;
import java.util.Hashtable;
//import sun.jdbc.rowset.CachedRowSet;

/**
 * Created by ifrankel on 12/9/2014.
 */
public class EConfirmData {
    public static String EC_CLICK_AND_CONF_REF = "SUBMIT CLICK AND CONFIRM OK";
    public static final String CPTY_TYPE = "C";
    public static final String BROKER_TYPE = "B";

    private static String NO_BROKER = "**NONE**";

    private final String EC_SCHEMA = "econfirm.";
    private final String OT_SCHEMA = "ops_tracking.";
    private java.sql.Connection opsTrackingConnection;

    public EConfirmData(java.sql.Connection pOpsTrackingConnection) throws SQLException, Exception {
        this.opsTrackingConnection = pOpsTrackingConnection;
    }

    private int getNextSequence(String seqName) throws SQLException {
        int nextSeqNo = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT " + EC_SCHEMA + seqName +
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

    public String getBatchId(String pSeCptySn)
            throws Exception {
        String batchId = "";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement(
                    "select alias from cpty.v_cpty_alias  " +
                    "where alias_type_code = 'ECBAT' " +
                    "and cpty_sn = ? ");

            statement.setString(1, pSeCptySn);
            rs = statement.executeQuery();
            if (rs.next()) {
                batchId = (rs.getString("alias"));
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

        return batchId;
    }

    public String getSeCptySn(String pTradingSystem, double pTradeID) throws SQLException {
        String seCptySn = "";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("select trd_sys_code, ticket, td.se_cpty_sn " +
                    "from " + OT_SCHEMA + "trade t, " +
                    OT_SCHEMA + "trade_data td " +
                    "where t.id = td.trade_id " +
                    "and t.trd_sys_code = ? " +
                    "and t.ticket = ? ");

            statement.setString(1, pTradingSystem);
            statement.setDouble(2, pTradeID);
            rs = statement.executeQuery();
            if (rs.next()) {
                seCptySn = (rs.getString("se_cpty_sn"));
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

        return seCptySn;
    }

    public String getECUserId(String pSECptySn) throws SQLException {
        String ecUserId = "";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT EC_USER_ID from " + EC_SCHEMA +
                    "EC_CONTROL where SE_CPTY_SN = ?");
            statement.setString(1, pSECptySn);
            rs = statement.executeQuery();
            if (rs.next()) {
                ecUserId = (rs.getString("EC_USER_ID"));
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

        return ecUserId;
    }

    public String getECPassword(String pSECptySn) throws SQLException {
        String ecPassword = "";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT EC_PASSWORD from " + EC_SCHEMA +
                    "EC_CONTROL where SE_CPTY_SN = ?");
            statement.setString(1, pSECptySn);
            rs = statement.executeQuery();
            if (rs.next()) {
                ecPassword = (rs.getString("EC_PASSWORD"));
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
        return  ecPassword;
    }

    public Hashtable getUniqueUserList() throws SQLException {
        PreparedStatement statement = null;
        ResultSet rs = null;
        Hashtable hs = new Hashtable();
        //Map userPassEntries = new HashMap<String, String>();
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT distinct ec_user_id, ec_password from " + EC_SCHEMA +
                    "EC_CONTROL ");
            //statement.setString(1, pSECptySn);
            rs = statement.executeQuery();
            while (rs.next()) {
                String userName = rs.getString("ec_user_id");
                String pwd =  rs.getString("ec_password");
                hs.put(userName,pwd);
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
        return hs;
    }

    public String getLastMessageDateTime(String userName) throws SQLException {
        String lastMessageDateTime = "";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT LAST_MESSAGE_DATETIME from " + EC_SCHEMA +
                    "EC_CONTROL where ec_user_id = ?");
            statement.setString(1, userName);
            rs = statement.executeQuery();
            if (rs.next()) {
                lastMessageDateTime = (rs.getString("LAST_MESSAGE_DATETIME"));
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

        return lastMessageDateTime;
    }

    public void updateLastMessageDateTime(String pLastMessageDateTime,String userName)
            throws SQLException {
        PreparedStatement statement = null;
        try {
            String updateSQL =
                    "update " + EC_SCHEMA + "EC_CONTROL set " +
                            "LAST_MESSAGE_DATETIME = ? " +
                            "where ec_user_id = ?";

            statement = opsTrackingConnection.prepareStatement(updateSQL);
            statement.setString(1, pLastMessageDateTime);
            statement.setString(2,userName);
            //statement.setString(2, pSECptySn);
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

    public String getLastStatusDateTime(String userName) throws SQLException {
        String lastStatusDateTime = "";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT LAST_STATUS_DATETIME from " + EC_SCHEMA +
                    "EC_CONTROL where ec_user_id = ?");
            statement.setString(1, userName);
            rs = statement.executeQuery();
            if (rs.next()) {
                //lastStatusDateTime = (rs.getString("LAST_STATUS_DATETIME"));
                lastStatusDateTime = (rs.getString("LAST_STATUS_DATETIME"));
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

        return lastStatusDateTime;
    }

    public void updateLastStatusDateTime(String pLastStatusDateTime, String userName)
            throws SQLException {
        PreparedStatement statement = null;
        try {
            String updateSQL =
                    "update " + EC_SCHEMA + "EC_CONTROL set " +
                            "LAST_STATUS_DATETIME = ? " +
                            "where ec_user_id = ?";

            statement = opsTrackingConnection.prepareStatement(updateSQL);
            statement.setString(1, pLastStatusDateTime);
            statement.setString(2,userName);
            //statement.setString(2, pSECptySn);
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

    public String getLastBkrStatusDateTime(String userName) throws SQLException {
        String lastStatusDateTime = "";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT LAST_BKR_STATUS_DATETIME from " + EC_SCHEMA +
                    "EC_CONTROL where ec_user_id = ?");
            statement.setString(1, userName);
            rs = statement.executeQuery();
            if (rs.next()) {
                lastStatusDateTime = (rs.getString("LAST_BKR_STATUS_DATETIME"));
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

        return lastStatusDateTime;
    }

    public void updateLastBkrStatusDateTime(String pLastStatusDateTime,String userName)
            throws SQLException {
        PreparedStatement statement = null;
        try {
            String updateSQL =
                    "update " + EC_SCHEMA + "EC_CONTROL set " +
                            "LAST_BKR_STATUS_DATETIME = ? " +
                            "where ec_user_id = ?";

            statement = opsTrackingConnection.prepareStatement(updateSQL);
            statement.setString(1, pLastStatusDateTime);
            statement.setString(2,userName);
            //statement.setString(2, pSECptySn);
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

    public String getRqmtStatus(double pTradeId, String rqmt) throws SQLException {
        String status = "";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String statementSQL = "";
            statementSQL = "select status from ops_tracking.trade_rqmt where rqmt = ? " +
                    "and trade_id = ?";
            statement = opsTrackingConnection.prepareStatement(statementSQL);
            statement.setString(1, rqmt);
            statement.setDouble(2, pTradeId);
            rs = statement.executeQuery();
            if (rs.next()) {
                status = (rs.getString("status"));
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
        return status;
    }

    public boolean isCandCValidVerbalRqmt(double pTradeId) throws SQLException {
        boolean status = false;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String statementSQL = "";
            statementSQL = "select reference from ops_tracking.trade_rqmt where rqmt = 'VBCP' " +
                    "and trade_id = ? " + "and reference = ? " + "and status = 'CONF' ";
            statement = opsTrackingConnection.prepareStatement(statementSQL);
            statement.setDouble(1, pTradeId);
            statement.setString(2, EC_CLICK_AND_CONF_REF);
            rs = statement.executeQuery();
            if (rs.next()) {
                status = true;
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
        return status;
    }

    public boolean isBrokerExist(String pMessageDesc) throws SQLException {
        boolean status = false;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String statementSQL = "";
            statementSQL = "select * from " + EC_SCHEMA + "broker_blocked_ignore " +
                    "where broker_legal_name = ? ";

            statement = opsTrackingConnection.prepareStatement(statementSQL);
            statement.setString(1, pMessageDesc);
            rs = statement.executeQuery();
            if (rs.next()) {
                status = true;
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
        return status;
    }

/*
    public boolean isBrokerExist(String pMessageDesc) throws Exception {
        boolean brokerExists = false;
        String brokerLegalName = "";
        crsSqlStatement = "select * from econfirm.broker_blocked_ignore";
        refreshCRS();
        crs.beforeFirst();
        while (crs.next()) {
            brokerLegalName = crs.getString("BROKER_LEGAL_NAME");
            if ( pMessageDesc.indexOf(brokerLegalName) > -1) {
                brokerExists = true;
                break;
            }
        }
        return brokerExists;
    }
*/

    public int insertECSubmitLog(EConfirmSubmitLog_DataRec pEConfirmSubmitLogDataRec) throws SQLException {
        PreparedStatement statement = null;
        int nextSeqNo = -1;
        try {
            String seqName = "seq_ec_submit_log";
            nextSeqNo = getNextSequence(seqName);
            String insertSQL =
                    "Insert into " + EC_SCHEMA + "EC_SUBMIT_LOG( " +
                            "ID, " + //1
                            "TRADING_SYSTEM, " + //2
                            "TRADE_ID, " + //3
                            "TRACE_ID, " + //4
                            "SUBMIT_TIMESTAMP_GMT, " + //
                            "STATUS_MESSAGE, " + //5
                            "ACTION ) " + //6
                            "values( ?, ?, ?, ?, sysdate, ?, ? )";

            statement = opsTrackingConnection.prepareStatement(insertSQL);
            statement.setInt(1, nextSeqNo);
            DAOUtils.setStatementString(2, pEConfirmSubmitLogDataRec.tradingSystem, statement);
            DAOUtils.setStatementDouble(3, pEConfirmSubmitLogDataRec.tradeID, statement);
            DAOUtils.setStatementString(4, pEConfirmSubmitLogDataRec.traceID, statement);
            DAOUtils.setStatementString(5, pEConfirmSubmitLogDataRec.statusMessage, statement);
            DAOUtils.setStatementString(6, pEConfirmSubmitLogDataRec.action, statement);
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

    public boolean isECTradeSummaryExist(String pTradingSystem, double pTradeID) throws SQLException {
        boolean recordExists = false;
        int count = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT count(*) from " + EC_SCHEMA +
                    "EC_TRADE_SUMMARY where trading_system = ? and trade_id = ?");
            statement.setString(1, pTradingSystem);
            statement.setDouble(2, pTradeID);
            rs = statement.executeQuery();
            if (rs.next()) {
                count = (rs.getInt("count(*)"));
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

    public String getECTradeSummaryStatus(String pTradingSystem, double pTradeID) throws SQLException {
        String ecStatus = "NONE";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT ec_status from " + EC_SCHEMA +
                    "EC_TRADE_SUMMARY where trading_system = ? and trade_id = ?");
            statement.setString(1, pTradingSystem);
            statement.setDouble(2, pTradeID);
            rs = statement.executeQuery();
            if (rs.next()) {
                ecStatus = (rs.getString("ec_status"));
                if (ecStatus == null){
                    ecStatus = "NONE";
                }
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

    public int insertECTradeSummary(EConfirmSummary_DataRec pEConfirmSummaryDataRec) throws SQLException {
        PreparedStatement statement = null;
        int nextSeqNo = -1;
        try {
            String seqName = "seq_ec_trade_summary";
            nextSeqNo = getNextSequence(seqName);
            String insertSQL =
                    "Insert into " + EC_SCHEMA + "EC_TRADE_SUMMARY( " +
                            "ID, " + //1
                            "TRADING_SYSTEM, " + //2
                            "TRADE_ID, " + //3
                            "PRODUCT_ID, " + //4
                            "EC_STATUS, " + //5
                            "ERROR_FLAG," + //6
                            "LAST_UPDATE_TIMESTAMP_GMT, " + //
                            "CMT, " +
                            "BKR_STATUS,"+
                            "BKR_REF_ID) " + //7
                            "values( ?, ?, ?, ?, ?, ?, sysdate, ?,?,?)";

            statement = opsTrackingConnection.prepareStatement(insertSQL);
            statement.setInt(1, nextSeqNo);
            DAOUtils.setStatementString(2, pEConfirmSummaryDataRec.tradingSystem, statement);
            DAOUtils.setStatementDouble(3, pEConfirmSummaryDataRec.tradeID, statement);
            DAOUtils.setStatementInt(4, pEConfirmSummaryDataRec.productID, statement);
            DAOUtils.setStatementString(5, pEConfirmSummaryDataRec.status, statement);
            DAOUtils.setStatementString(6, pEConfirmSummaryDataRec.errorFlag, statement);
            statement.setNull(7, Types.VARCHAR);
            DAOUtils.setStatementString(8, pEConfirmSummaryDataRec.bkrStatus, statement);
            DAOUtils.setStatementString(9, pEConfirmSummaryDataRec.bkrTradeRefID, statement);
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

    public void updateECTradeSummary(EConfirmSummary_DataRec pEConfirmSummaryDataRec) throws SQLException {
        PreparedStatement statement = null;
        try {
            String updateSQL;

            updateSQL = "update " + EC_SCHEMA + "EC_TRADE_SUMMARY set " +
                    "EC_STATUS = ?," +
                    "ERROR_FLAG = ?," +
                    "CPTY_TRADE_REF_ID = ?";

            //There may or may not be a product id passed. If not, ignore it so
            //if avoids
            if (pEConfirmSummaryDataRec.productID > 0)
                updateSQL += ",PRODUCT_ID = ?";

            updateSQL += " where trade_id = ?";

            statement = opsTrackingConnection.prepareStatement(updateSQL);
            DAOUtils.setStatementString(1, pEConfirmSummaryDataRec.status, statement);
            DAOUtils.setStatementString(2, pEConfirmSummaryDataRec.errorFlag, statement);
            DAOUtils.setStatementString(3, pEConfirmSummaryDataRec.cptyTradeRefID, statement);

            if (pEConfirmSummaryDataRec.productID > 0){
                DAOUtils.setStatementInt(4, pEConfirmSummaryDataRec.productID, statement);
                DAOUtils.setStatementDouble(5, pEConfirmSummaryDataRec.tradeID, statement);
            }
            else
                DAOUtils.setStatementDouble(4, pEConfirmSummaryDataRec.tradeID, statement);

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

    public void insertECIgnoredStatusMessage(ECIgnoredStatusMessage_Rec pECIgnoredStatusMessageRec) throws SQLException {
        PreparedStatement statement = null;
        try {
            String insertSQL =
                    "Insert into " + EC_SCHEMA + "IGNORED_STATUS_MESSAGE( " +
                            "SENDER_TRADE_REF_ID, " + //1
                            "TRACE_ID, " + //2
                            "STATUS, " + //3
                            "BUYER, " + //4
                            "SELLER, " + //5
                            "SUBMISSION_COMPANY," + //6
                            "STATUS_DATE, " + //
                            "TRADE_DATE, " +
                            "CRTD_TIMESTAMP_GMT) " +
                            "values( ?, ?, ?, ?, ?, ?, ?, ?, sysdate )";

            statement = opsTrackingConnection.prepareStatement(insertSQL);
            DAOUtils.setStatementString(1, pECIgnoredStatusMessageRec.senderTradeRefId, statement);
            DAOUtils.setStatementString(2, pECIgnoredStatusMessageRec.traceId, statement);
            DAOUtils.setStatementString(3, pECIgnoredStatusMessageRec.status, statement);
            DAOUtils.setStatementString(4, pECIgnoredStatusMessageRec.buyer, statement);
            DAOUtils.setStatementString(5, pECIgnoredStatusMessageRec.seller, statement);
            DAOUtils.setStatementString(6, pECIgnoredStatusMessageRec.submissionCompany, statement);
            DAOUtils.setStatementString(7, pECIgnoredStatusMessageRec.statusDate,statement);
            DAOUtils.setStatementString(8, pECIgnoredStatusMessageRec.tradeDate,statement);
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


    public String getECBkrTradeSummaryStatus(String pTradingSystem, double pTradeID) throws SQLException {
        String ecStatus = "NONE";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT bkr_status from " + EC_SCHEMA +
                    "EC_TRADE_SUMMARY where trading_system = ? and trade_id = ?");
            statement.setString(1, pTradingSystem);
            statement.setDouble(2, pTradeID);
            rs = statement.executeQuery();
            if (rs.next()) {
                ecStatus = (rs.getString("bkr_status"));
                if (ecStatus == null){
                    ecStatus = "NONE";
                }
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

    public void updateBkrECTradeSummary(EConfirmSummary_DataRec pEConfirmSummaryDataRec) throws SQLException {
        PreparedStatement statement = null;
        try {
            String updateSQL;

            updateSQL = "update " + EC_SCHEMA + "EC_TRADE_SUMMARY set " +
                    "BKR_STATUS = ?," +
                    "ERROR_FLAG = ?," +
                    "BKR_REF_ID = ?";

            //There may or may not be a product id passed. If not, ignore it so
            //if avoids
            if (pEConfirmSummaryDataRec.productID > 0)
                updateSQL += ",PRODUCT_ID = ?";

            updateSQL += " where trade_id = ?";

            statement = opsTrackingConnection.prepareStatement(updateSQL);
            DAOUtils.setStatementString(1, pEConfirmSummaryDataRec.bkrStatus, statement);
            DAOUtils.setStatementString(2, pEConfirmSummaryDataRec.errorFlag, statement);
            DAOUtils.setStatementString(3, pEConfirmSummaryDataRec.bkrTradeRefID, statement);

            if (pEConfirmSummaryDataRec.productID > 0){
                DAOUtils.setStatementInt(4, pEConfirmSummaryDataRec.productID, statement);
                DAOUtils.setStatementDouble(5, pEConfirmSummaryDataRec.tradeID, statement);
            }
            else
                DAOUtils.setStatementDouble(4, pEConfirmSummaryDataRec.tradeID, statement);

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

    public int getECTradeSummaryProductId(double pTradeID) throws SQLException {
        int ecProductId = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT product_id from " + EC_SCHEMA +
                    "EC_TRADE_SUMMARY where trade_id = ?");
            statement.setDouble(1, pTradeID);
            rs = statement.executeQuery();
            if (rs.next()) {
                ecProductId = (rs.getInt("product_id"));
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

        return ecProductId;
    }

    public String getECTradeSummaryTradingSys(double pTradeID) throws SQLException {
        String tradingSys = "";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT trading_system from " + EC_SCHEMA +
                    "EC_TRADE_SUMMARY where trade_id = ?");
            statement.setDouble(1, pTradeID);
            rs = statement.executeQuery();
            if (rs.next()) {
                tradingSys = (rs.getString("trading_system"));
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

        return tradingSys;
    }

    public void setNotifyOpsTrackingSubmit(double pTradeID) throws SQLException {
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_econfirm_submit(?) }";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pTradeID);
            statement.execute();
        }
        finally {
            if (statement != null) {
                try {
                    statement.close();
                }
                catch (SQLException e) {}
            }
            statement = null;
        }
    }

    public int insertECErrorLog(EConfirmErrorLog_DataRec pEConfirmErrorLogDataRec)
            throws SQLException {
        PreparedStatement statement = null;
        int nextSeqNo = -1;
        try {
            String seqName = "seq_ec_error_log";
            nextSeqNo = getNextSequence(seqName);
            String insertSQL =
                    "Insert into " + EC_SCHEMA + "EC_ERROR_LOG( " +
                            "ID, " + //1
                            "TRADE_ID, " + //2
                            "EC_STATUS, " + //3
                            "EC_CODE, " + //4
                            "EC_TYPE, " + //5
                            "EC_STATUS_DT, " + //6
                            "EC_DESC ) " + //7
                            "values( ?, ?, ?, ?, ?, ?, ? )";

            statement = opsTrackingConnection.prepareStatement(insertSQL);
            statement.setInt(1, nextSeqNo);
            DAOUtils.setStatementDouble(2, pEConfirmErrorLogDataRec.tradeID, statement);
            DAOUtils.setStatementString(3, pEConfirmErrorLogDataRec.ecStatus, statement);
            DAOUtils.setStatementString(4, pEConfirmErrorLogDataRec.ecCode, statement);
            DAOUtils.setStatementString(5, pEConfirmErrorLogDataRec.ecType, statement);
            DAOUtils.setStatementString(6, pEConfirmErrorLogDataRec.ecStatusDt, statement);
            DAOUtils.setStatementString(7, pEConfirmErrorLogDataRec.ecDesc, statement);
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

    public int insertECMessageLog(EConfirmMessageLog_DataRec pEConfirmMessageLogDataRec)
            throws SQLException {
        PreparedStatement statement = null;
        int nextSeqNo = -1;
        try {
            String seqName = "seq_ec_message_log";
            nextSeqNo = getNextSequence(seqName);
            String insertSQL =
                    "Insert into " + EC_SCHEMA + "EC_MESSAGE_LOG( " +
                            "ID, " + //1
                            "TRADING_SYSTEM, " + //2
                            "TRADE_ID, " + //3
                            "TRACE_ID, " + //4
                            "MESSAGE_CODE, " + //5
                            "MESSAGE_TYPE, " + //6
                            "MESSAGE_STATUS_DT, " + //7
                            "MESSAGE_DESC ) " + //8
                            "values( ?, ?, ?, ?, ?, ?, ?, ? )";

            statement = opsTrackingConnection.prepareStatement(insertSQL);
            statement.setInt(1, nextSeqNo);
            DAOUtils.setStatementString(2, pEConfirmMessageLogDataRec.tradingSystem, statement);
            DAOUtils.setStatementDouble(3, pEConfirmMessageLogDataRec.tradeID, statement);
            DAOUtils.setStatementString(4, pEConfirmMessageLogDataRec.traceID, statement);
            DAOUtils.setStatementString(5, pEConfirmMessageLogDataRec.messageCode, statement);
            DAOUtils.setStatementString(6, pEConfirmMessageLogDataRec.messageType, statement);
            DAOUtils.setStatementString(7, pEConfirmMessageLogDataRec.messageStatusDt, statement);
            DAOUtils.setStatementString(8, pEConfirmMessageLogDataRec.messageDesc, statement);
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

    public CachedRowSet getECSummaryResubmit() throws SQLException {
        PreparedStatement statement = null;
        ResultSet rs = null;
        CachedRowSet crs;
        //crs = new CachedRowSet();
        crs = new CachedRowSetImpl();
        try {
            statement = opsTrackingConnection.prepareStatement("SELECT * from " + EC_SCHEMA +
                    "EC_TRADE_SUMMARY where OK_TO_RESUBMIT_IND <> 'N'");
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

    public void setECTradeSummaryOKToResubmit(String pTradingSystem, double pTradeID, String pOKToResubmitInd)
            throws SQLException {
        PreparedStatement statement = null;
        try {
            String updateSQL =
                    "update " + EC_SCHEMA + "EC_TRADE_SUMMARY set " +
                            "OK_TO_RESUBMIT_IND = ? " +
                            "where trading_system = ? " +
                            "and trade_id = ?";

            statement = opsTrackingConnection.prepareStatement(updateSQL);
            statement.setString(1, pOKToResubmitInd);
            statement.setString(2, pTradingSystem);
            statement.setDouble(3, pTradeID);
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

    public int insertECIgnoredLog(EConfirmSubmitLog_DataRec pEConfirmSubmitLogDataRec) throws SQLException {
        PreparedStatement statement = null;
        int nextSeqNo = -1;
        try {
            String seqName = "seq_ec_ignored_log";
            nextSeqNo = getNextSequence(seqName);
            String insertSQL =
                    "Insert into " + EC_SCHEMA + "EC_IGNORED_LOG( " +
                            "ID, " + //1
                            "TRADING_SYSTEM, " + //2
                            "TRADE_ID, " + //3
                            "TRACE_ID, " + //4
                            "IGNORED_TIMESTAMP_GMT, " + //
                            "STATUS_MESSAGE ) " + //5
                            "values( ?, ?, ?, ?, sysdate, ? )";

            statement = opsTrackingConnection.prepareStatement(insertSQL);
            statement.setInt(1, nextSeqNo);
            DAOUtils.setStatementString(2, pEConfirmSubmitLogDataRec.tradingSystem, statement);
            DAOUtils.setStatementDouble(3, pEConfirmSubmitLogDataRec.tradeID, statement);
            DAOUtils.setStatementString(4, pEConfirmSubmitLogDataRec.traceID, statement);
            DAOUtils.setStatementString(5, pEConfirmSubmitLogDataRec.statusMessage, statement);
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

    public String getEConfirmRqmtStatus(double pTradeId) throws SQLException {
        String status = "";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String statementSQL = "";
            statementSQL = "select status from ops_tracking.trade_rqmt where rqmt = 'ECONF' " +
                    "and trade_id = ?";
            statement = opsTrackingConnection.prepareStatement(statementSQL);
            statement.setDouble(1, pTradeId);
            rs = statement.executeQuery();
            if (rs.next()) {
                status = (rs.getString("status"));
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
        return status;
    }

    public void setNotifyOpsTrackingMatched(double pTradeID, String pCptyTradeId, String pMatchedDate )
            throws SQLException {
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_econfirm_matched(?, ?, ?) }";
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
                catch (SQLException e) {}
            }
            statement = null;
        }
    }

    public void setNotifyOpsTrackingPending(double pTradeID) throws SQLException {
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_econfirm_pending(?) }";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pTradeID);
            statement.execute();
        }
        finally {
            if (statement != null) {
                try {
                    statement.close();
                }
                catch (SQLException e) {}
            }
            statement = null;
        }
    }


    public void setNotifyOpsTrackingUnmatched(double pTradeID, String pErrorMsg) throws SQLException {
        OracleCallableStatement statement = null;
        String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_econfirm_unmatched(?, ?) }";
        //This was blowing up when there were too many broken fields.
        String cmt = "";
        if (pErrorMsg.trim().length() < 255)
            cmt = pErrorMsg;
        else
            cmt = pErrorMsg.substring(1,255);

        try {
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
                catch (SQLException e) {}
            }
            statement = null;
        }

    }

    public void setNotifyOpsTrackingError(double pTradeID, String pComment) throws SQLException {
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_econfirm_error(?, ?) }";
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
                catch (SQLException e) {}
            }
            statement = null;
        }
    }

    public void setNotifyOpsTrackingFail(double pTradeID, String pComment) throws SQLException {
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_econfirm_fail(?, ?) }";
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
                catch (SQLException e) {}
            }
            statement = null;
        }

    }

    public void setNotifyOpsTrackingCancelled(double pTradeID) throws SQLException {
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_econfirm_cancelled(?) }";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pTradeID);
            statement.execute();
        }
        finally {
            if (statement != null) {
                try {
                    statement.close();
                }
                catch (SQLException e) {}
            }
            statement = null;
        }
    }

    public void setNotifyOpsTrackingBkrSubmit(double pTradeID) throws SQLException {
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_econfirm_bkr_submit(?) }";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pTradeID);
            statement.execute();
        }
        finally {
            if (statement != null) {
                try {
                    statement.close();
                }
                catch (SQLException e) {}
            }
            statement = null;
        }
    }

    public void setNotifyOpsTrackingBkrMatched(double pTradeID, String pCptyTradeId, String pMatchedDate )
            throws SQLException {
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_econfirm_bkr_matched(?, ?, ?) }";
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
                catch (SQLException e) {}
            }
            statement = null;
        }
    }

    public void setNotifyOpsTrackingBkrUnmatched(double pTradeID, String pErrorMsg) throws SQLException {
        OracleCallableStatement statement = null;
        String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_econfirm_bkr_unmatched(?, ?) }";
        //This was blowing up when there were too many broken fields.
        String cmt = "";
        if (pErrorMsg.trim().length() < 255)
            cmt = pErrorMsg;
        else
            cmt = pErrorMsg.substring(1,255);

        try {
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
                catch (SQLException e) {}
            }
            statement = null;
        }

    }

    public void setNotifyOpsTrackingBkrError(double pTradeID, String pComment) throws SQLException {
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_econfirm_bkr_error(?, ?) }";
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
                catch (SQLException e) {}
            }
            statement = null;
        }
    }

    public void setNotifyOpsTrackingBkrFail(double pTradeID, String pComment) throws SQLException {
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_econfirm_bkr_fail(?, ?) }";
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
                catch (SQLException e) {}
            }
            statement = null;
        }

    }

    public void setNotifyOpsTrackingBkrCancelled(double pTradeID) throws SQLException {
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_econfirm_bkr_cancelled(?) }";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pTradeID);
            statement.execute();
        }
        finally {
            if (statement != null) {
                try {
                    statement.close();
                }
                catch (SQLException e) {}
            }
            statement = null;
        }
    }

    public void setNotifyOpsTrackingBkrPending(double pTradeID) throws SQLException {
        OracleCallableStatement statement = null;
        try {
            String callSqlStatement = "{call ops_tracking.PKG_EXT_NOTIFY.p_econfirm_bkr_pending(?) }";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, pTradeID);
            statement.execute();
        }
        finally {
            if (statement != null) {
                try {
                    statement.close();
                }
                catch (SQLException e) {}
            }
            statement = null;
        }
    }

}
