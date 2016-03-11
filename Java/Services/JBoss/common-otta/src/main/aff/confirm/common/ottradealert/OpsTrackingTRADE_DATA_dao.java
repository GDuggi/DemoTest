package aff.confirm.common.ottradealert;

import aff.confirm.common.util.DAOUtils;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

/**
 * User: ifrankel
 * Date: Jun 10, 2003
 * Time: 9:41:30 AM
 * To change this template use Options | File Templates.
 */
public class OpsTrackingTRADE_DATA_dao extends OpsTrackingDAO {

    public OpsTrackingTRADE_DATA_dao( java.sql.Connection pOpsTrackingConnection)
            throws SQLException {
        this.opsTrackingConnection = pOpsTrackingConnection;
    };


    public OpsTrackingTRADE_DATA_rec getOpsTrackingTRADE_DATA_rec(double pTradeId) throws SQLException {
        OpsTrackingTRADE_DATA_rec otTRADE_DATA_rec;
        otTRADE_DATA_rec = new OpsTrackingTRADE_DATA_rec();
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            String selectSQL = "select distinct td.*,vbk.bk_test_book_flag from ops_tracking.TRADE_DATA td, infinity_mgr.vbk  where TRADE_ID = ? and td.book  = vbk.bk_short_name (+)";
            statement = opsTrackingConnection.prepareStatement(selectSQL);
            statement.setDouble(1, pTradeId);
            rs = statement.executeQuery();
            if (rs.next()) {
                otTRADE_DATA_rec.INCEPTION_DT = rs.getDate("INCEPTION_DT");
                otTRADE_DATA_rec.CDTY_CODE = rs.getString("CDTY_CODE");
                otTRADE_DATA_rec.TRADE_DT = rs.getDate("TRADE_DT");
                otTRADE_DATA_rec.XREF = rs.getString("XREF");
                otTRADE_DATA_rec.CPTY_SN = rs.getString("CPTY_SN");
                otTRADE_DATA_rec.QTY_TOT = rs.getDouble("QTY_TOT");
                otTRADE_DATA_rec.QTY = rs.getDouble("QTY");
                otTRADE_DATA_rec.UOM_DUR_CODE = rs.getString("UOM_DUR_CODE");
                otTRADE_DATA_rec.setLOCATION_SN(rs.getString("LOCATION_SN"));
                otTRADE_DATA_rec.PRICE_DESC = rs.getString("PRICE_DESC");
                otTRADE_DATA_rec.START_DT = rs.getDate("START_DT");
                otTRADE_DATA_rec.END_DT = rs.getDate("END_DT");
                otTRADE_DATA_rec.BOOK = rs.getString("BOOK");
                otTRADE_DATA_rec.TRADE_TYPE_CODE = rs.getString("TRADE_TYPE_CODE");
                otTRADE_DATA_rec.STTL_TYPE = rs.getString("STTL_TYPE");
                otTRADE_DATA_rec.BROKER_SN = rs.getString("BROKER_SN");
                otTRADE_DATA_rec.COMM = rs.getString("COMM");
                otTRADE_DATA_rec.setBUY_SELL_IND( rs.getString("BUY_SELL_IND"));
                otTRADE_DATA_rec.REF_SN = rs.getString("REF_SN");
                otTRADE_DATA_rec.PAY_PRICE = rs.getString("PAY_PRICE");
                otTRADE_DATA_rec.REC_PRICE = rs.getString("REC_PRICE");
                otTRADE_DATA_rec.SE_CPTY_SN = rs.getString("SE_CPTY_SN");
                otTRADE_DATA_rec.TRADE_STAT_CODE = rs.getString("TRADE_STAT_CODE");
                otTRADE_DATA_rec.CDTY_GRP_CODE = rs.getString("CDTY_GRP_CODE");
                otTRADE_DATA_rec.BROKER_PRICE = rs.getString("BROKER_PRICE");
                otTRADE_DATA_rec.OPTN_PREM_PRICE = rs.getString("OPTN_PREM_PRICE");
                otTRADE_DATA_rec.OPTN_PUT_CALL_IND = rs.getString("OPTN_PUT_CALL_IND");
                otTRADE_DATA_rec.OPTN_STRIKE_PRICE = rs.getString("OPTN_STRIKE_PRICE");
                // 5/8/2007 Israel - support EFS trades
                otTRADE_DATA_rec.EFS_FLAG = rs.getString("EFS_FLAG");
                otTRADE_DATA_rec.EFS_CPTY_SN = rs.getString("EFS_CPTY_SN");
                otTRADE_DATA_rec.TRADE_ID = pTradeId;
                otTRADE_DATA_rec.TEST_BOOK_FLAG = "Y".equalsIgnoreCase(rs.getString("BK_TEST_BOOK_FLAG"));
            }

        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) {
            }
            statement = null;
            try {
                if (rs != null) {
                    rs.close();
                    rs = null;
                }
            } catch (SQLException e) {
            }

        }
        return otTRADE_DATA_rec;
    }


    public int insertTradeDataByTrdSys(TradingSystemDATA_rec ptsDATA_rec)
            throws SQLException, Exception {
        int tradeDataID;
        if (ptsDATA_rec.tradingSystem.equalsIgnoreCase("AFF"))
            tradeDataID = insertAffTradeData(ptsDATA_rec);
        else if (ptsDATA_rec.tradingSystem.equalsIgnoreCase("SYM"))
            tradeDataID = insertSymphonyTradeData(ptsDATA_rec);
        else
            throw new Exception("Internal Error: OpsTrackingTRADE_DATA_dao.insertTradeDataByTrdSys: " +
                    "Trading System= " + ptsDATA_rec.tradingSystem );
        return tradeDataID;
    }

    private int insertAffTradeData(TradingSystemDATA_rec ptsDATA_rec)
            throws SQLException {
        PreparedStatement statement = null;
        int nextSeqNo = -1;
        try {
            String seqName = "seq_trade_data";
            nextSeqNo = getNextSequence(seqName);
            String insertSQL =
                    "Insert into ops_tracking.TRADE_DATA( " +
                    "ID, " + //1
                    "TRADE_ID, " + //2
                    "INCEPTION_DT, " + //3
                    "CDTY_CODE, " + //4
                    "TRADE_DT, " + //5
                    "XREF, " + //6
                    "CPTY_SN, " + //7
                    "QTY_TOT, " + //8
                    "QTY, " + //9
                    "UOM_DUR_CODE, " + //10
                    "LOCATION_SN, " + //11
                    "PRICE_DESC, " + //12
                    "START_DT, " + //13
                    "END_DT, " + //14
                    "BOOK, " + //15
                    "TRADE_TYPE_CODE, " + //16
                    "STTL_TYPE, " + //17
                    "BROKER_SN, " + //18
                    "COMM," + //19
                    "BUY_SELL_IND," + //20
                    "REF_SN," + //21
                    "PAY_PRICE," + //22
                    "REC_PRICE, " + //23
                    "SE_CPTY_SN, " + //24
                    "TRADE_STAT_CODE, " + //25
                    "CDTY_GRP_CODE, " + //26
                    "BROKER_PRICE, " + //27
                    "OPTN_PREM_PRICE, " + //28
                    "OPTN_PUT_CALL_IND, " + //29
                    "OPTN_STRIKE_PRICE, " + //30
                     // 5/8/2007 Israel - support EFS trades
                    "EFS_FLAG, " + //31
                    "EFS_CPTY_SN ) " + //32
                    "values( ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ? )";

            statement = opsTrackingConnection.prepareStatement(insertSQL);
            statement.setInt(1, nextSeqNo);
            statement.setDouble(2, ptsDATA_rec.TRADE_ID);
            DAOUtils.setStatementDate(3, ptsDATA_rec.INCEPTION_DT, statement);
            DAOUtils.setStatementString(4, ptsDATA_rec.CDTY_CODE, statement);
            DAOUtils.setStatementDate(5, ptsDATA_rec.TRADE_DT, statement);
            DAOUtils.setStatementString(6, ptsDATA_rec.XREF, statement);
            DAOUtils.setStatementString(7, ptsDATA_rec.CPTY_SN, statement);
            statement.setDouble(8, ptsDATA_rec.QTY_TOT);
            statement.setDouble(9, ptsDATA_rec.QTY);
            DAOUtils.setStatementString(10, ptsDATA_rec.UOM_DUR_CODE, statement);
            DAOUtils.setStatementString(11, ptsDATA_rec.getLOCATION_SN(), statement);
            DAOUtils.setStatementString(12, ptsDATA_rec.PRICE_DESC, statement);
            DAOUtils.setStatementDate(13, ptsDATA_rec.START_DT, statement);
            DAOUtils.setStatementDate(14, ptsDATA_rec.END_DT, statement);
            DAOUtils.setStatementString(15, ptsDATA_rec.BOOK, statement);
            DAOUtils.setStatementString(16, ptsDATA_rec.TRADE_TYPE_CODE, statement);
            DAOUtils.setStatementString(17, ptsDATA_rec.STTL_TYPE, statement);
            DAOUtils.setStatementString(18, ptsDATA_rec.BROKER_SN, statement);
            DAOUtils.setStatementString(19, ptsDATA_rec.COMM, statement);
            DAOUtils.setStatementString(20, ptsDATA_rec.getBUY_SELL_IND(), statement);
            DAOUtils.setStatementString(21, ptsDATA_rec.REF_SN, statement);
            DAOUtils.setStatementString(22, ptsDATA_rec.PAY_PRICE, statement);
            DAOUtils.setStatementString(23, ptsDATA_rec.REC_PRICE, statement);
            DAOUtils.setStatementString(24, ptsDATA_rec.SE_CPTY_SN, statement);
            DAOUtils.setStatementString(25, ptsDATA_rec.TRADE_STAT_CODE, statement);
            DAOUtils.setStatementString(26, ptsDATA_rec.CDTY_GRP_CODE, statement);
            DAOUtils.setStatementString(27, ptsDATA_rec.BROKER_PRICE, statement);
            DAOUtils.setStatementString(28, ptsDATA_rec.OPTN_PREM_PRICE, statement);
            DAOUtils.setStatementString(29, ptsDATA_rec.OPTN_PUT_CALL_IND, statement);
            DAOUtils.setStatementString(30, ptsDATA_rec.OPTN_STRIKE_PRICE, statement);
            // 5/8/2007 Israel - support EFS trades
            DAOUtils.setStatementString(31, ptsDATA_rec.EFS_FLAG, statement);
            DAOUtils.setStatementString(32, ptsDATA_rec.EFS_CPTY_SN, statement);
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


    private int insertSymphonyTradeData(TradingSystemDATA_rec ptsDATA_rec)
            throws SQLException {
        PreparedStatement statement = null;
        int nextSeqNo = -1;
        try {
            String seqName = "seq_trade_data";
            nextSeqNo = getNextSequence(seqName);
            String insertSQL =
                    "Insert into ops_tracking.TRADE_DATA( " +
                    "ID, " + //1
                    "TRADE_ID, " + //2
                    "INCEPTION_DT, " + //3
                    "CDTY_CODE, " + //4
                    "TRADE_DT, " + //5
                    "XREF, " + //6
                    "CPTY_SN, " + //7
                    "QTY_TOT, " + //8
                    "QTY, " + //9
                    "UOM_DUR_CODE, " + //10
                    "LOCATION_SN, " + //11
                    "PRICE_DESC, " + //12
                    "START_DT, " + //13
                    "END_DT, " + //14
                    "BOOK, " + //15
                    "TRADE_TYPE_CODE, " + //16
                    "STTL_TYPE, " + //17
                    "BROKER_SN, " + //18
                    "COMM," + //19
                    "BUY_SELL_IND," + //20
                    "REF_SN," + //21
                    "PAY_PRICE," + //22
                    "REC_PRICE, " + //23
                    "SE_CPTY_SN, " + //24
                    "TRADE_STAT_CODE, " + //25
                    "CDTY_GRP_CODE, " + //26
                    "BROKER_PRICE, " + //27
                    "OPTN_PREM_PRICE, " + //28
                    "OPTN_PUT_CALL_IND, " + //29
                    "OPTN_STRIKE_PRICE, " + //30
                     // 5/8/2007 Israel - support EFS trades
                    "EFS_FLAG, " + //31
                    "EFS_CPTY_SN ) " + //32
                    "values( ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ? )";

            statement = opsTrackingConnection.prepareStatement(insertSQL);
            statement.setInt(1, nextSeqNo);
            statement.setDouble(2, ptsDATA_rec.TRADE_ID);
            DAOUtils.setStatementDate(3, ptsDATA_rec.INCEPTION_DT, statement);
            DAOUtils.setStatementString(4, ptsDATA_rec.CDTY_CODE, statement);
            DAOUtils.setStatementDate(5, ptsDATA_rec.TRADE_DT, statement);
            DAOUtils.setStatementString(6, ptsDATA_rec.XREF, statement);
            DAOUtils.setStatementString(7, ptsDATA_rec.CPTY_SN, statement);
            statement.setDouble(8, ptsDATA_rec.QTY_TOT);
            statement.setDouble(9, ptsDATA_rec.QTY);
            DAOUtils.setStatementString(10, ptsDATA_rec.UOM_DUR_CODE, statement);
            DAOUtils.setStatementString(11, ptsDATA_rec.getLOCATION_SN(), statement);
            DAOUtils.setStatementString(12, ptsDATA_rec.PRICE_DESC, statement);
            DAOUtils.setStatementDate(13, ptsDATA_rec.START_DT, statement);
            DAOUtils.setStatementDate(14, ptsDATA_rec.END_DT, statement);
            DAOUtils.setStatementString(15, ptsDATA_rec.BOOK, statement);
            DAOUtils.setStatementString(16, ptsDATA_rec.TRADE_TYPE_CODE, statement);
            DAOUtils.setStatementString(17, ptsDATA_rec.STTL_TYPE, statement);
            DAOUtils.setStatementString(18, ptsDATA_rec.BROKER_SN, statement);
            DAOUtils.setStatementString(19, ptsDATA_rec.COMM, statement);
            DAOUtils.setStatementString(20, ptsDATA_rec.getBUY_SELL_IND(), statement);
            DAOUtils.setStatementString(21, ptsDATA_rec.REF_SN, statement);
            DAOUtils.setStatementString(22, ptsDATA_rec.PAY_PRICE, statement);
            DAOUtils.setStatementString(23, ptsDATA_rec.REC_PRICE, statement);
            DAOUtils.setStatementString(24, ptsDATA_rec.SE_CPTY_SN, statement);
            DAOUtils.setStatementString(25, ptsDATA_rec.TRADE_STAT_CODE, statement);
            DAOUtils.setStatementString(26, ptsDATA_rec.CDTY_GRP_CODE, statement);
            DAOUtils.setStatementString(27, ptsDATA_rec.BROKER_PRICE, statement);
            DAOUtils.setStatementString(28, ptsDATA_rec.OPTN_PREM_PRICE, statement);
            DAOUtils.setStatementString(29, ptsDATA_rec.OPTN_PUT_CALL_IND, statement);
            DAOUtils.setStatementString(30, ptsDATA_rec.OPTN_STRIKE_PRICE, statement);
            // 5/8/2007 Israel - support EFS trades
            DAOUtils.setStatementString(31, ptsDATA_rec.EFS_FLAG, statement);
            DAOUtils.setStatementString(32, ptsDATA_rec.EFS_CPTY_SN, statement);
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


    public void updateTradeData(TradingSystemDATA_rec ptsDATA_rec, String pAuditTypeCode)
            throws SQLException, Exception {
        if (ptsDATA_rec.tradingSystem.equalsIgnoreCase("AFF"))
            updateAffTradeData(ptsDATA_rec);
        else if (ptsDATA_rec.tradingSystem.equalsIgnoreCase("SYM"))
            updateSymphonyTradeData(ptsDATA_rec, pAuditTypeCode);
        else
            throw new Exception("Internal Error: OpsTrackingTRADE_DATA_dao.updateTradeData: " +
                    "Trading System= " + ptsDATA_rec.tradingSystem );
    }


    private void updateAffTradeData(TradingSystemDATA_rec ptsDATA_rec)
            throws SQLException {
        PreparedStatement statement = null;
        //int rowsUpdated = -1;
        try {
            String updateSQL =
                    "Update ops_tracking.TRADE_DATA SET " +
                    //"ID, " +
                    //"TRADE_ID, " +
                    //"INCEPTION_DT, " +
                    "CDTY_CODE = ?, " + //1
                    "TRADE_DT = ?, " + //2
                    "XREF = ?, " + //3
                    "CPTY_SN = ?, " + //4
                    "QTY_TOT = ?, " + //5
                    "QTY = ?, " + //6
                    "UOM_DUR_CODE = ?, " + //7
                    "LOCATION_SN = ?, " + //8
                    "PRICE_DESC = ?, " + //9
                    "START_DT = ?, " + //10
                    "END_DT = ?, " + //11
                    "BOOK = ?, " + //12
                    "TRADE_TYPE_CODE = ?, " + //13
                    "STTL_TYPE = ?, " + //14
                    "BROKER_SN = ?, " + //15
                    "COMM = ?, " + //16
                    "BUY_SELL_IND = ?, " + //17
                    "REF_SN = ?, " + //18
                    "PAY_PRICE = ?, " + //19
                    "REC_PRICE = ?, " + //20
                    "SE_CPTY_SN = ?, " + //21
                    "TRADE_STAT_CODE = ?, " + //22
                    "CDTY_GRP_CODE = ?, " + //23
                    "BROKER_PRICE = ?, " + //24
                    "OPTN_PREM_PRICE = ?, " + //25
                    "OPTN_PUT_CALL_IND = ?, " + //26
                    "OPTN_STRIKE_PRICE = ?, " + //27
                    // 5/8/2007 Israel - support EFS trades
                    "EFS_FLAG = ?, " + //28
                    "EFS_CPTY_SN = ? " + //29
                    "where TRADE_ID = ?"; //30

            statement = opsTrackingConnection.prepareStatement(updateSQL);
            DAOUtils.setStatementString(1, ptsDATA_rec.CDTY_CODE, statement);
            DAOUtils.setStatementDate(2, ptsDATA_rec.TRADE_DT, statement);
            DAOUtils.setStatementString(3, ptsDATA_rec.XREF, statement);
            DAOUtils.setStatementString(4, ptsDATA_rec.CPTY_SN, statement);
            statement.setDouble(5, ptsDATA_rec.QTY_TOT);
            statement.setDouble(6, ptsDATA_rec.QTY);
            DAOUtils.setStatementString(7, ptsDATA_rec.UOM_DUR_CODE, statement);
            DAOUtils.setStatementString(8, ptsDATA_rec.getLOCATION_SN(), statement);
            DAOUtils.setStatementString(9, ptsDATA_rec.PRICE_DESC, statement);
            DAOUtils.setStatementDate(10, ptsDATA_rec.START_DT, statement);
            DAOUtils.setStatementDate(11, ptsDATA_rec.END_DT, statement);
            DAOUtils.setStatementString(12, ptsDATA_rec.BOOK, statement);
            DAOUtils.setStatementString(13, ptsDATA_rec.TRADE_TYPE_CODE, statement);
            DAOUtils.setStatementString(14, ptsDATA_rec.STTL_TYPE, statement);
            DAOUtils.setStatementString(15, ptsDATA_rec.BROKER_SN, statement);
            DAOUtils.setStatementString(16, ptsDATA_rec.COMM, statement);
            DAOUtils.setStatementString(17, ptsDATA_rec.getBUY_SELL_IND(), statement);
            DAOUtils.setStatementString(18, ptsDATA_rec.REF_SN, statement);
            DAOUtils.setStatementString(19, ptsDATA_rec.PAY_PRICE, statement);
            DAOUtils.setStatementString(20, ptsDATA_rec.REC_PRICE, statement);
            DAOUtils.setStatementString(21, ptsDATA_rec.SE_CPTY_SN, statement);
            DAOUtils.setStatementString(22, ptsDATA_rec.TRADE_STAT_CODE, statement);
            DAOUtils.setStatementString(23, ptsDATA_rec.CDTY_GRP_CODE, statement);
            DAOUtils.setStatementString(24, ptsDATA_rec.BROKER_PRICE, statement);
            DAOUtils.setStatementString(25, ptsDATA_rec.OPTN_PREM_PRICE, statement);
            DAOUtils.setStatementString(26, ptsDATA_rec.OPTN_PUT_CALL_IND, statement);
            DAOUtils.setStatementString(27, ptsDATA_rec.OPTN_STRIKE_PRICE, statement);
            // 5/8/2007 Israel - support EFS trades
            DAOUtils.setStatementString(28, ptsDATA_rec.EFS_FLAG, statement);
            DAOUtils.setStatementString(29, ptsDATA_rec.EFS_CPTY_SN, statement);
            statement.setDouble(30, ptsDATA_rec.TRADE_ID);
            statement.executeUpdate();
            //if (rowsUpdated == 0)
              //  throw new UpdateTradeDataException("Could not update trade_data");
        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) {
            }
            statement = null;
        }
    }

    private void updateSymphonyTradeData(TradingSystemDATA_rec ptsDATA_rec, String pAuditTypeCode)
            throws SQLException {
        //int rowsUpdated = -1;
        PreparedStatement statement = null;
        try {
            String updateSQL = null;
            if (pAuditTypeCode.trim().equals("VOID")) {
                updateSQL = "Update ops_tracking.TRADE_DATA SET TRADE_STAT_CODE = 'VOID'" +
                        " where TRADE_ID = ?";// 1;
                statement = opsTrackingConnection.prepareStatement(updateSQL);
                statement.setDouble(1, ptsDATA_rec.TRADE_ID);
            } else {
                updateSQL =
                        "Update ops_tracking.TRADE_DATA SET " +
                                //"ID, " +
                                //"TRADE_ID, " +
                                //"INCEPTION_DT, " +
                                "CDTY_CODE = ?, " + //1
                                "TRADE_DT = ?, " + //2
                                "XREF = ?, " + //3
                                "CPTY_SN = ?, " + //4
                                "QTY_TOT = ?, " + //5
                                "QTY = ?, " + //6
                                "UOM_DUR_CODE = ?, " + //7
                                "LOCATION_SN = ?, " + //8
                                "PRICE_DESC = ?, " + //9
                                "START_DT = ?, " + //10
                                "END_DT = ?, " + //11
                                "BOOK = ?, " + //12
                                "TRADE_TYPE_CODE = ?, " + //13
                                "STTL_TYPE = ?, " + //14
                                "BROKER_SN = ?, " + //15
                                "COMM = ?, " + //16
                                "BUY_SELL_IND = ?, " + //17
                                "REF_SN = ?, " + //18
                                "PAY_PRICE = ?, " + //19
                                "REC_PRICE = ?, " + //20
                                "SE_CPTY_SN = ?, " + //21
                                "TRADE_STAT_CODE = ?, " + //22
                                "CDTY_GRP_CODE = ?, " + //23
                                "BROKER_PRICE = ?, " + //24
                                "OPTN_PREM_PRICE = ?, " + //25
                                "OPTN_PUT_CALL_IND = ?, " + //26
                                "OPTN_STRIKE_PRICE = ?, " + //27
                                // 5/8/2007 Israel - support EFS trades
                                "EFS_FLAG = ?, " + //28
                                "EFS_CPTY_SN = ? " + //29
                                "where TRADE_ID = ?"; //30

                statement = opsTrackingConnection.prepareStatement(updateSQL);
                DAOUtils.setStatementString(1, ptsDATA_rec.CDTY_CODE, statement);
                DAOUtils.setStatementDate(2, ptsDATA_rec.TRADE_DT, statement);
                DAOUtils.setStatementString(3, ptsDATA_rec.XREF, statement);
                DAOUtils.setStatementString(4, ptsDATA_rec.CPTY_SN, statement);
                statement.setDouble(5, ptsDATA_rec.QTY_TOT);
                statement.setDouble(6, ptsDATA_rec.QTY);
                DAOUtils.setStatementString(7, ptsDATA_rec.UOM_DUR_CODE, statement);
                DAOUtils.setStatementString(8, ptsDATA_rec.getLOCATION_SN(), statement);
                DAOUtils.setStatementString(9, ptsDATA_rec.PRICE_DESC, statement);
                DAOUtils.setStatementDate(10, ptsDATA_rec.START_DT, statement);
                DAOUtils.setStatementDate(11, ptsDATA_rec.END_DT, statement);
                DAOUtils.setStatementString(12, ptsDATA_rec.BOOK, statement);
                DAOUtils.setStatementString(13, ptsDATA_rec.TRADE_TYPE_CODE, statement);
                DAOUtils.setStatementString(14, ptsDATA_rec.STTL_TYPE, statement);
                DAOUtils.setStatementString(15, ptsDATA_rec.BROKER_SN, statement);
                DAOUtils.setStatementString(16, ptsDATA_rec.COMM, statement);
                DAOUtils.setStatementString(17, ptsDATA_rec.getBUY_SELL_IND(), statement);
                DAOUtils.setStatementString(18, ptsDATA_rec.REF_SN, statement);
                DAOUtils.setStatementString(19, ptsDATA_rec.PAY_PRICE, statement);
                DAOUtils.setStatementString(20, ptsDATA_rec.REC_PRICE, statement);
                DAOUtils.setStatementString(21, ptsDATA_rec.SE_CPTY_SN, statement);
                DAOUtils.setStatementString(22, ptsDATA_rec.TRADE_STAT_CODE, statement);
                DAOUtils.setStatementString(23, ptsDATA_rec.CDTY_GRP_CODE, statement);
                DAOUtils.setStatementString(24, ptsDATA_rec.BROKER_PRICE, statement);
                DAOUtils.setStatementString(25, ptsDATA_rec.OPTN_PREM_PRICE, statement);
                DAOUtils.setStatementString(26, ptsDATA_rec.OPTN_PUT_CALL_IND, statement);
                DAOUtils.setStatementString(27, ptsDATA_rec.OPTN_STRIKE_PRICE, statement);
                // 5/8/2007 Israel - support EFS trades
                DAOUtils.setStatementString(28, ptsDATA_rec.EFS_FLAG, statement);
                DAOUtils.setStatementString(29, ptsDATA_rec.EFS_CPTY_SN, statement);
                statement.setDouble(30, ptsDATA_rec.TRADE_ID);
                statement.executeUpdate();
                //if (rowsUpdated == 0)
                //throw new UpdateTradeDataException("Could not update trade_data");
            }

        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) {
            }
            statement = null;
        }
    }

    public void updateSymphonyTradeDataVoid(double pTradeId)
            throws SQLException {
        PreparedStatement statement = null;
        try {
            String updateSQL = null;
            updateSQL = "Update ops_tracking.TRADE_DATA SET TRADE_STAT_CODE = 'VOID'" +
                    " where TRADE_ID = ?";// 1;
            statement = opsTrackingConnection.prepareStatement(updateSQL);
            statement.setDouble(1, pTradeId);
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
}

