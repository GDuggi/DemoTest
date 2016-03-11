package aff.confirm.common.ottradealert;

import oracle.jdbc.OracleCallableStatement;

import java.sql.Date;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.text.DecimalFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;

/**
 * User: ifrankel
 * Date: Jun 26, 2009
 * Time: 9:41:30 AM
 * To change this template use Options | File Templates.
 */
public class OpsTrackingTRADE_DATA_CHG_dao extends OpsTrackingDAO {
    private DecimalFormat decFmt = new DecimalFormat("#0.####");
    private final SimpleDateFormat sdfDate = new SimpleDateFormat("dd-MMM-yyyy");
    private OpsTrackingODB_CANCEL_CORRECT_EXCLUDE_dao otOdbCxlCorrExcludeDao;
    private OpsTrackingODB_CANCEL_CORRECT_EXCLUDE_rec otOdbCxlCorrExcludeRec;
    private OpsTrackingTRADE_DATA_CHG_rec otTradeDataChgRec;
    private OpsTrackingTRADE_dao otTRADE_dao;
    private final int COL_COUNT = 28;
    private ArrayList resetColsList;
    private String[] columnNames = {"BOOK", "BROKER_PRICE", "BROKER_SN", "BUY_SELL_IND", "CDTY_CODE", "CDTY_GRP_CODE",
                                     "COMM", "CPTY_SN", "END_DT", "LOCATION_SN", "OPTN_PREM_PRICE", "OPTN_PUT_CALL_IND",
                                     "OPTN_STRIKE_PRICE", "PAY_PRICE", "PRICE_DESC", "QTY", "REC_PRICE", "REF_SN", "SE_CPTY_SN",
                                     "START_DT", "STTL_TYPE", "TRADE_DT", "TRADE_STAT_CODE", "TRADE_TYPE_CODE", "UOM_DUR_CODE",
                                     "XREF", "EFS_FLAG", "EFS_CPTY_SN"};

    public OpsTrackingTRADE_DATA_CHG_dao( java.sql.Connection pOpsTrackingConnection) throws Exception {
        this.opsTrackingConnection = pOpsTrackingConnection;
        otOdbCxlCorrExcludeDao = new OpsTrackingODB_CANCEL_CORRECT_EXCLUDE_dao(this.opsTrackingConnection);
        otOdbCxlCorrExcludeRec = new OpsTrackingODB_CANCEL_CORRECT_EXCLUDE_rec();
        otTradeDataChgRec = new OpsTrackingTRADE_DATA_CHG_rec();
        otTRADE_dao = new OpsTrackingTRADE_dao(this.opsTrackingConnection);
        resetColsList = new ArrayList(40);
        resetColsList = getResetColsList();
    };

    private ArrayList getResetColsList() throws SQLException, Exception {
         ArrayList resultList = new ArrayList(40);
         ResultSet rs = null;
         PreparedStatement statement = null;
         String sqlStatement =
             "select col_name "
              + "from ops_tracking.trade_rqmt_reset_cols "
              + "order by col_name ";
         try {
             statement = opsTrackingConnection.prepareStatement(sqlStatement);
             rs = statement.executeQuery();
             while (rs.next()) {
                 resultList.add(rs.getString("col_name").trim());
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
        return resultList;
     }


    private java.sql.Date getMinBusnDt() throws SQLException {
        java.sql.Date minBusnDt = null;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String statementSQL = "";
            statementSQL = "select min(busn_dt) minBusnDt from infinity_mgr.close_dt where stat_ind = 'O' ";
            statement = opsTrackingConnection.prepareStatement(statementSQL);
            rs = statement.executeQuery();
            if (rs.next()) {
                minBusnDt = (rs.getDate("minBusnDt"));
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
        return minBusnDt;
    }

    private void insertTradeDataChgRow(OpsTrackingTRADE_DATA_CHG_rec otTradeDataChgRec )
            throws SQLException {
            OracleCallableStatement statement;
            String callSqlStatement = "{call ops_tracking.PKG_TRADE_DATA_CHG.p_insert_trade_data_chg(?,?,?,?,?,?,?,?,?) }";
            statement = (OracleCallableStatement) opsTrackingConnection.prepareCall(callSqlStatement);
            statement.setDouble(1, otTradeDataChgRec.TradeId);
            statement.setString(2, otTradeDataChgRec.ColName);
            statement.setString(3, otTradeDataChgRec.OldValue);
            statement.setString(4, otTradeDataChgRec.NewValue);
            statement.setDate(5, otTradeDataChgRec.UpdBusnDt);
            statement.setString(6, otTradeDataChgRec.UserName);
            statement.setString(7, otTradeDataChgRec.AuditTypeCode);
            statement.setString(8, otTradeDataChgRec.ODBIncludeFlag);
            statement.setDouble(9, otTradeDataChgRec.ODBCxlCorrExcludeId);
            statement.executeUpdate();
            statement.close();
            statement = null;
    }

    public void insertTradeDataChgRows(OpsTrackingTRADE_DATA_rec otTRADE_DATA_rec, TradingSystemDATA_rec tsDATA_rec,
                                       String pAuditTypeCode, String pUserName)
            throws SQLException, ParseException {
        //7/31/09 Israel -- Added for safety
        if (!otTRADE_dao.isTradeExist(tsDATA_rec.TRADE_ID))
            return;
        //java.sql.Date closedBusnDt = getMinBusnDt();
        java.sql.Date closedBusnDt = new Date(new java.util.Date().getTime());
        int excludeId = 0;
        String includeFlag = "N";
        String[] oldDataValues = new String[COL_COUNT];
        String[] newDataValues = new String[COL_COUNT];
        oldDataValues = getOldDataValues(otTRADE_DATA_rec);
        newDataValues = getNewDataValues(tsDATA_rec);
        boolean isDeleteOrVoid = nullValue(newDataValues[22]).equalsIgnoreCase("DELETE") || 
                                 nullValue(newDataValues[22]).equalsIgnoreCase("VOID");

        otOdbCxlCorrExcludeRec.init();
        otOdbCxlCorrExcludeRec.auditTypeCode = pAuditTypeCode;
        otOdbCxlCorrExcludeRec.bookSn = tsDATA_rec.BOOK;
        otOdbCxlCorrExcludeRec.cptySn = tsDATA_rec.CPTY_SN;
        otOdbCxlCorrExcludeRec.seCptySn = tsDATA_rec.SE_CPTY_SN;

        //Iterate through all fields, test and insert where appropriate
        for (int i=0;i<COL_COUNT;i++){
            if (!nullValue(oldDataValues[i]).equalsIgnoreCase(nullValue(newDataValues[i]))) {
                if (columnNames[i].equalsIgnoreCase("TRADE_STAT_CODE") && !isDeleteOrVoid)
                    continue;
                otOdbCxlCorrExcludeRec.colName = columnNames[i];
                excludeId = otOdbCxlCorrExcludeDao.isDoExclude(otOdbCxlCorrExcludeRec);
                includeFlag = getIncludeFlag(excludeId);
                otTradeDataChgRec.setFields(tsDATA_rec.TRADE_ID, otOdbCxlCorrExcludeRec.colName, oldDataValues[i], newDataValues[i],
                                            closedBusnDt, pUserName, pAuditTypeCode, includeFlag, excludeId);
                insertTradeDataChgRow(otTradeDataChgRec);
                }
            }

        //Do the Broker Change field which has different rules.
        if (!nullValue(oldDataValues[1]).equalsIgnoreCase(nullValue(newDataValues[1])) ||
            !nullValue(oldDataValues[2]).equalsIgnoreCase(nullValue(newDataValues[2])) ||
            !nullValue(oldDataValues[6]).equalsIgnoreCase(nullValue(newDataValues[6]))) {
            otOdbCxlCorrExcludeRec.colName = "BROKER_CHG";
            excludeId = otOdbCxlCorrExcludeDao.isDoExclude(otOdbCxlCorrExcludeRec);
            includeFlag = getIncludeFlag(excludeId);
            otTradeDataChgRec.setFields(tsDATA_rec.TRADE_ID, otOdbCxlCorrExcludeRec.colName, oldDataValues[2], newDataValues[2],
                                        closedBusnDt, pUserName, pAuditTypeCode, includeFlag, excludeId);
            insertTradeDataChgRow(otTradeDataChgRec);
            }
        }
       
    public boolean hasRqmtResetColChanged(OpsTrackingTRADE_DATA_rec otTRADE_DATA_rec, TradingSystemDATA_rec tsDATA_rec)
            throws SQLException, ParseException {
        boolean hasChanged = false;
        if (!otTRADE_dao.isTradeExist(tsDATA_rec.TRADE_ID))
            return hasChanged;
        String[] oldDataValues = new String[COL_COUNT];
        String[] newDataValues = new String[COL_COUNT];
        oldDataValues = getOldDataValues(otTRADE_DATA_rec);
        newDataValues = getNewDataValues(tsDATA_rec);

        for (int i=0;i<COL_COUNT;i++){
            if (!nullValue(oldDataValues[i]).equalsIgnoreCase(nullValue(newDataValues[i])) &&
                resetColsList.indexOf(columnNames[i]) > -1) {
                hasChanged = true;
                break;
                }
            }

        return hasChanged;
        }

    private String[] getOldDataValues(OpsTrackingTRADE_DATA_rec otTRADE_DATA_rec){
        String[] oldDataValues = new String[COL_COUNT];
        oldDataValues[0] = otTRADE_DATA_rec.BOOK;
        oldDataValues[1] = otTRADE_DATA_rec.BROKER_PRICE;
        oldDataValues[2] = otTRADE_DATA_rec.BROKER_SN;
        oldDataValues[3] = otTRADE_DATA_rec.getBUY_SELL_IND();
        oldDataValues[4] = otTRADE_DATA_rec.CDTY_CODE;
        oldDataValues[5] = otTRADE_DATA_rec.CDTY_GRP_CODE;
        oldDataValues[6] = otTRADE_DATA_rec.COMM;
        oldDataValues[7] = otTRADE_DATA_rec.CPTY_SN;
        oldDataValues[8] = GetDateValue(otTRADE_DATA_rec.END_DT);
        oldDataValues[9] = otTRADE_DATA_rec.getLOCATION_SN();
        oldDataValues[10] = otTRADE_DATA_rec.OPTN_PREM_PRICE;
        oldDataValues[11] = otTRADE_DATA_rec.OPTN_PUT_CALL_IND;
        oldDataValues[12] = otTRADE_DATA_rec.OPTN_STRIKE_PRICE;
        oldDataValues[13] = otTRADE_DATA_rec.PAY_PRICE;
        oldDataValues[14] = otTRADE_DATA_rec.PRICE_DESC;
        oldDataValues[15] = Double.toString(otTRADE_DATA_rec.QTY);
        oldDataValues[16] = otTRADE_DATA_rec.REC_PRICE;
        oldDataValues[17] = otTRADE_DATA_rec.REF_SN;
        oldDataValues[18] = otTRADE_DATA_rec.SE_CPTY_SN;
        oldDataValues[19] = GetDateValue(otTRADE_DATA_rec.START_DT);
        oldDataValues[20] = otTRADE_DATA_rec.STTL_TYPE;
        oldDataValues[21] = GetDateValue(otTRADE_DATA_rec.TRADE_DT);
        oldDataValues[22] = otTRADE_DATA_rec.TRADE_STAT_CODE;
        oldDataValues[23] = otTRADE_DATA_rec.TRADE_TYPE_CODE;
        oldDataValues[24] = otTRADE_DATA_rec.UOM_DUR_CODE;
        oldDataValues[25] = otTRADE_DATA_rec.XREF;
        oldDataValues[26] = otTRADE_DATA_rec.EFS_FLAG;
        oldDataValues[27] = otTRADE_DATA_rec.EFS_CPTY_SN;
        return oldDataValues;
    }

    private String[] getNewDataValues(TradingSystemDATA_rec tsDATA_rec){
        String[] newDataValues = new String[COL_COUNT];
         newDataValues[0] = tsDATA_rec.BOOK;
         newDataValues[1] = tsDATA_rec.BROKER_PRICE;
         newDataValues[2] = tsDATA_rec.BROKER_SN;
         newDataValues[3] = tsDATA_rec.getBUY_SELL_IND();
         newDataValues[4] = tsDATA_rec.CDTY_CODE;
         newDataValues[5] = tsDATA_rec.CDTY_GRP_CODE;
         newDataValues[6] = tsDATA_rec.COMM;
         newDataValues[7] = tsDATA_rec.CPTY_SN;
         newDataValues[8] = GetDateValue(tsDATA_rec.END_DT);
         newDataValues[9] = tsDATA_rec.getLOCATION_SN();
         newDataValues[10] = tsDATA_rec.OPTN_PREM_PRICE;
         newDataValues[11] = tsDATA_rec.OPTN_PUT_CALL_IND;
         newDataValues[12] = tsDATA_rec.OPTN_STRIKE_PRICE;
         newDataValues[13] = tsDATA_rec.PAY_PRICE;
         newDataValues[14] = tsDATA_rec.PRICE_DESC;
         newDataValues[15] = Double.toString(tsDATA_rec.QTY);
         newDataValues[16] = tsDATA_rec.REC_PRICE;
         newDataValues[17] = tsDATA_rec.REF_SN;
         newDataValues[18] = tsDATA_rec.SE_CPTY_SN;
         newDataValues[19] = GetDateValue(tsDATA_rec.START_DT);
         newDataValues[20] = tsDATA_rec.STTL_TYPE;
         newDataValues[21] = GetDateValue(tsDATA_rec.TRADE_DT);
         newDataValues[22] = tsDATA_rec.TRADE_STAT_CODE;
         newDataValues[23] = tsDATA_rec.TRADE_TYPE_CODE;
         newDataValues[24] = tsDATA_rec.UOM_DUR_CODE;
         newDataValues[25] = tsDATA_rec.XREF;
         newDataValues[26] = tsDATA_rec.EFS_FLAG;
         newDataValues[27] = tsDATA_rec.EFS_CPTY_SN;
        return  newDataValues;
    }

    private String getIncludeFlag(int pExcludeId) {
        String includeFlag = "N";
        if (pExcludeId > 0)
            includeFlag = "N";
        else
            includeFlag = "Y";

        return includeFlag;
    }

    private String nullValue(String pValue){
        String value = "NULL";
        if (pValue != null && pValue.length() > 0)
            value = pValue;
        return value;
    }

    private String GetDateValue(Date pDate) {
        String arrayVal = "";
        if (pDate != null)
            arrayVal = sdfDate.format(pDate);
        return arrayVal;
    }


}

