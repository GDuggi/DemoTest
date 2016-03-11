package cnf.docflow.dao;

import cnf.docflow.data.ConfirmMessageData;
import cnf.docflow.util.ConversionUtils;

import java.sql.*;
import java.text.ParseException;

/**
 * Created by jvega on 7/20/2015.
 */
public class TradeDataDAO  extends SqlDAO {

    public TradeDataDAO(Connection connection) throws SQLException {
        this.dbConnection = connection;
    }

    public boolean alreadyExists(String pTradeID, String pTradeSystemCode) throws SQLException {
        double result = 0;
        PreparedStatement preparedStatement = null;
        ResultSet rs = null;
        String statementSQL = "";
        try{
            statementSQL = "select count(*) cnt from "+ schemaName + ".trade t, "+ schemaName + ".trade_data td where t.ID = td.TRADE_ID and trd_sys_ticket = ? and trd_sys_code = ?";
            preparedStatement = dbConnection.prepareStatement(statementSQL);
            ConversionUtils.setStatementString(1, pTradeID, preparedStatement);
            ConversionUtils.setStatementString(2, pTradeSystemCode, preparedStatement);
            rs = preparedStatement.executeQuery();
            if (rs.next()) {
                result = rs.getDouble("cnt");
            }
        }
        finally {
            if (rs != null) {
                rs.close();
                rs = null;
            }
            if (preparedStatement != null){
                preparedStatement.close();
            }
        }
        return result >= 1;
    }

    public void InsertData(ConfirmMessageData msgData) throws SQLException, ParseException {
        PreparedStatement preparedStatement = null;
        try {
                int tradeId = msgData.getOther_TradeID();
                String seqName = "seq_trade_data";
                int nextSeqNo = 0;
                nextSeqNo = getNextSequence(seqName);
                String stmtSQL =
                        "Insert into " + schemaName + ".TRADE_DATA ( " +
                                "ID, " +
                                "TRADE_ID, " +
                                "INCEPTION_DT, " +
                                "CDTY_CODE, " +
                                "CDTY_GRP_CODE, " +
                                "TRADE_DT, " +
                                "XREF, " +
                                "CPTY_SN, " +
                                "CPTY_ID, " +
                                "QTY_TOT, " +
                                "LOCATION_SN, " +
                                "PRICE_DESC, " +
                                "START_DT, " +
                                "END_DT, " +
                                "BOOK, " +
                                "TRADE_TYPE_CODE, " +
                                "STTL_TYPE, " +
                                "BROKER_SN, " +
                                "BROKER_LEGAL_NAME, " +
                                "BROKER_ID, " +
                                "BUY_SELL_IND, " +
                                "REF_SN, " +
                                //"SE_CPTY_SN, " +
                                "BOOKING_CO_SN, " +
                                "BOOKING_CO_ID, " +
                                "TRADE_STAT_CODE, " +
                                "BROKER_PRICE, " +
                                "OPTN_STRIKE_PRICE, " +
                                "OPTN_PREM_PRICE, " +
                                "OPTN_PUT_CALL_IND, " +
                                "PERMISSION_KEY, " +
                                "PROFIT_CENTER, " +
                                "CPTY_LEGAL_NAME, " +
                                "QTY_DESC, " +
                                "TRADE_DESC, " +
                                "TRADER, " +
                                "TRANSPORT_DESC " +
                                ") values( ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";


                preparedStatement = dbConnection.prepareStatement(stmtSQL);
                
                int i = 0;
                preparedStatement.setObject(++i, nextSeqNo, Types.NUMERIC);
                preparedStatement.setObject(++i, tradeId, Types.NUMERIC);
                ConversionUtils.setStatementDateTime(++i, msgData.getTrade_CreationDt(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_CdtySn(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_CdtyGroup(), preparedStatement);
                ConversionUtils.setStatementDateLong(++i, msgData.getTrade_TradeDt(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_Xref(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_CptySn(), preparedStatement);
                ConversionUtils.setStatementInt(++i, msgData.getTrade_CptyId(), preparedStatement);
                ConversionUtils.setStatementDouble(++i, msgData.getTrade_QtyTot(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_LocationSn(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_PriceDesc(), preparedStatement);
                ConversionUtils.setStatementDateShort(++i, msgData.getTrade_StartDt(), preparedStatement);
                ConversionUtils.setStatementDateShort(++i, msgData.getTrade_EndDt(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_Book(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_TradeTypeCode(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_SttlType(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_BrokerSn(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_BrokerLegalName(), preparedStatement);
                ConversionUtils.setStatementInt(++i, msgData.getTrade_BrokerId(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_BuySellInd(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_RefSn(), preparedStatement);
                //ConversionUtils.setStatementString(++i, msgData.getTrade_BookingCompany(), preparedStatement); //SE_CPTY_SN
                ConversionUtils.setStatementString(++i, msgData.getTrade_BookingCompany(), preparedStatement); //BOOKING_CO_SN
                ConversionUtils.setStatementInt(++i, msgData.getTrade_BookingCompanyId(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_TradeStatCode(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_BrokerPriceDesc(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_OptnStrikePrice(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_OptnPremPrice(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_OptnPutCallInd(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_PermissionKey(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_ProfitCenter(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_CptyLegalName(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_QtyDesc(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_TradeDesc(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_Trader(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_TransportDesc(), preparedStatement);
				System.out.println("TradeDataDAO: sql = " + stmtSQL);
				System.out.println("TradeDataDAO: ID="+nextSeqNo);
                System.out.println("TradeDataDAO: TRADE_ID="+tradeId);
                System.out.println("TradeDataDAO: INCEPTION_DT="+msgData.getTrade_CreationDt());
                System.out.println("TradeDataDAO: CDTY_CODE="+msgData.getTrade_CdtySn());
                System.out.println("TradeDataDAO: CDTY_GRP_CODE="+msgData.getTrade_CdtyGroup());
                System.out.println("TradeDataDAO: TRADE_DT="+msgData.getTrade_TradeDt());
                System.out.println("TradeDataDAO: XREF="+msgData.getTrade_Xref());
                System.out.println("TradeDataDAO: CPTY_SN="+msgData.getTrade_CptySn());
                System.out.println("TradeDataDAO: CPTY_ID="+msgData.getTrade_CptyId());
                System.out.println("TradeDataDAO: QTY_TOT="+msgData.getTrade_QtyTot());
                System.out.println("TradeDataDAO: LOCATION_SN="+msgData.getTrade_LocationSn());
                System.out.println("TradeDataDAO: PRICE_DESC="+msgData.getTrade_PriceDesc());
                System.out.println("TradeDataDAO: START_DT="+msgData.getTrade_StartDt());
                System.out.println("TradeDataDAO: END_DT="+msgData.getTrade_EndDt());
                System.out.println("TradeDataDAO: BOOK="+msgData.getTrade_Book());
                System.out.println("TradeDataDAO: TRADE_TYPE_CODE="+msgData.getTrade_TradeTypeCode());
                System.out.println("TradeDataDAO: STTL_TYPE="+msgData.getTrade_SttlType());
                System.out.println("TradeDataDAO: BROKER_SN="+msgData.getTrade_BrokerSn());
                System.out.println("TradeDataDAO: BROKER_LEGAL_NAME="+msgData.getTrade_BrokerLegalName());
                System.out.println("TradeDataDAO: BROKER_ID="+msgData.getTrade_BrokerId());
                System.out.println("TradeDataDAO: BUY_SELL_IND="+msgData.getTrade_BuySellInd());
                System.out.println("TradeDataDAO: REF_SN="+msgData.getTrade_RefSn());
                System.out.println("TradeDataDAO: BOOKING_CO_SN="+msgData.getTrade_BookingCompany());
                System.out.println("TradeDataDAO: BOOKING_CO_ID="+msgData.getTrade_BookingCompanyId());
                System.out.println("TradeDataDAO: TRADE_STAT_CODE="+msgData.getTrade_TradeStatCode());
                System.out.println("TradeDataDAO: BROKER_PRICE="+msgData.getTrade_BrokerPriceDesc());
                System.out.println("TradeDataDAO: OPTN_STRIKE_PRICE="+msgData.getTrade_OptnStrikePrice());
                System.out.println("TradeDataDAO: OPTN_PREM_PRICE="+msgData.getTrade_OptnPremPrice());
                System.out.println("TradeDataDAO: OPTN_PUT_CALL_IND="+msgData.getTrade_OptnPutCallInd());
                System.out.println("TradeDataDAO: PERMISSION_KEY="+ msgData.getTrade_PermissionKey());
                System.out.println("TradeDataDAO: PROFIT_CENTER="+msgData.getTrade_ProfitCenter());
                System.out.println("TradeDataDAO: CPTY_LEGAL_NAME="+msgData.getTrade_CptyLegalName());
                System.out.println("TradeDataDAO: QTY_DESC="+msgData.getTrade_QtyDesc());
                System.out.println("TradeDataDAO: TRADE_DESC="+msgData.getTrade_TradeDesc());
                System.out.println("TradeDataDAO: TRADER="+msgData.getTrade_Trader());
                System.out.println("TradeDataDAO: TRANSPORT_DESC="+msgData.getTrade_TransportDesc());

                preparedStatement.executeUpdate();

        } finally {
            try {
                if (preparedStatement != null)
                    preparedStatement.close();
            } catch (SQLException e) {
            }
        }
    }

    public void UpdateData(ConfirmMessageData msgData) throws SQLException, ParseException {
        PreparedStatement preparedStatement = null;
        try {
                int tradeId = getTradeID(msgData.getTrade_TradingSystemTicket(),msgData.getTrade_TradingSystemCode());
                //int tradeId = msgData.getOther_TradeID();
                String stmtSQL =
                        "update " + schemaName + ".TRADE_DATA SET " +
                                //"ID, " +
                               // "TRADE_ID, " +
                                //"INCEPTION_DT, " +
                                "CDTY_CODE = ?, " +
                                "CDTY_GRP_CODE = ?, " +
                                "TRADE_DT = ?, " +
                                "XREF = ?, " +
                                "CPTY_SN = ?, " +
                                "CPTY_ID = ?, " +
                                "QTY_TOT = ?, " +
                                "LOCATION_SN = ?, " +
                                "PRICE_DESC = ?, " +
                                "START_DT = ?, " +
                                "END_DT = ?, " +
                                "BOOK = ?, " +
                                "TRADE_TYPE_CODE = ?, " +
                                "STTL_TYPE = ?, " +
                                "BROKER_SN = ?, " +
                                "BROKER_LEGAL_NAME = ?, " +
                                "BROKER_ID = ?, " +
                                "BUY_SELL_IND = ?, " +
                                "REF_SN = ?, " +
                                "BOOKING_CO_SN = ?, " +
                                "BOOKING_CO_ID = ?, " +
                                "TRADE_STAT_CODE = ?, " +
                                "BROKER_PRICE = ?, " +
                                "OPTN_STRIKE_PRICE = ?, " +
                                "OPTN_PREM_PRICE = ?, " +
                                "OPTN_PUT_CALL_IND = ?, " +
                                "PERMISSION_KEY = ?, " +
                                "PROFIT_CENTER = ?, " +
                                "CPTY_LEGAL_NAME = ?, " +
                                "QTY_DESC = ?, " +
                                "TRADE_DESC = ?, " +
                                "TRADER = ?, " +
                                "TRANSPORT_DESC = ? " +
                                "where TRADE_ID = ? ";

                preparedStatement = dbConnection.prepareStatement(stmtSQL);
                int i = 0;
                ConversionUtils.setStatementString(++i, msgData.getTrade_CdtySn(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_CdtyGroup(), preparedStatement);
                ConversionUtils.setStatementDateLong(++i, msgData.getTrade_TradeDt(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_Xref(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_CptySn(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_CptyId(), preparedStatement);
                ConversionUtils.setStatementDouble(++i, msgData.getTrade_QtyTot(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_LocationSn(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_PriceDesc(), preparedStatement);
                ConversionUtils.setStatementDateShort(++i, msgData.getTrade_StartDt(), preparedStatement);
                ConversionUtils.setStatementDateShort(++i, msgData.getTrade_EndDt(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_Book(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_TradeTypeCode(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_SttlType(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_BrokerSn(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_BrokerLegalName(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_BrokerId(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_BuySellInd(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_RefSn(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_BookingCompany(), preparedStatement); //BOOKING_CO_SN
                ConversionUtils.setStatementInt(++i, msgData.getTrade_BookingCompanyId(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_TradeStatCode(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_BrokerPriceDesc(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_OptnStrikePrice(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_OptnPremPrice(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_OptnPutCallInd(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_PermissionKey(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_ProfitCenter(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_CptyLegalName(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_QtyDesc(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_TradeDesc(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_Trader(), preparedStatement);
                ConversionUtils.setStatementString(++i, msgData.getTrade_TransportDesc(), preparedStatement);
                preparedStatement.setObject(++i, tradeId, Types.NUMERIC);
                System.out.println("TradeDataDAO: sql = " + stmtSQL);
                System.out.println("TradeDataDAO: CDTY_CODE="+msgData.getTrade_CdtySn());
                System.out.println("TradeDataDAO: CDTY_GRP_CODE="+msgData.getTrade_CdtyGroup());
                System.out.println("TradeDataDAO: TRADE_DT="+msgData.getTrade_TradeDt());
                System.out.println("TradeDataDAO: XREF="+msgData.getTrade_Xref());
                System.out.println("TradeDataDAO: CPTY_SN="+msgData.getTrade_CptySn());
                System.out.println("TradeDataDAO: CPTY_ID="+msgData.getTrade_CptyId());
                System.out.println("TradeDataDAO: QTY_TOT="+msgData.getTrade_QtyTot());
                System.out.println("TradeDataDAO: LOCATION_SN="+msgData.getTrade_LocationSn());
                System.out.println("TradeDataDAO: PRICE_DESC="+msgData.getTrade_PriceDesc());
                System.out.println("TradeDataDAO: START_DT="+msgData.getTrade_StartDt());
                System.out.println("TradeDataDAO: END_DT="+msgData.getTrade_EndDt());
                System.out.println("TradeDataDAO: BOOK="+msgData.getTrade_Book());
                System.out.println("TradeDataDAO: TRADE_TYPE_CODE="+msgData.getTrade_TradeTypeCode());
                System.out.println("TradeDataDAO: STTL_TYPE="+msgData.getTrade_SttlType());
                System.out.println("TradeDataDAO: BROKER_SN="+msgData.getTrade_BrokerSn());
                System.out.println("TradeDataDAO: BROKER_LEGAL_NAME="+msgData.getTrade_BrokerLegalName());
                System.out.println("TradeDataDAO: BROKER_ID="+msgData.getTrade_BrokerId());
                System.out.println("TradeDataDAO: BUY_SELL_IND="+msgData.getTrade_BuySellInd());
                System.out.println("TradeDataDAO: REF_SN="+msgData.getTrade_RefSn());
                System.out.println("TradeDataDAO: BOOKING_CO_SN="+msgData.getTrade_BookingCompany());
                System.out.println("TradeDataDAO: BOOKING_CO_ID="+msgData.getTrade_BookingCompanyId());
                System.out.println("TradeDataDAO: TRADE_STAT_CODE="+msgData.getTrade_TradeStatCode());
                System.out.println("TradeDataDAO: BROKER_PRICE="+msgData.getTrade_BrokerPriceDesc());
                System.out.println("TradeDataDAO: OPTN_STRIKE_PRICE="+msgData.getTrade_OptnStrikePrice());
                System.out.println("TradeDataDAO: OPTN_PREM_PRICE="+msgData.getTrade_OptnPremPrice());
                System.out.println("TradeDataDAO: OPTN_PUT_CALL_IND="+msgData.getTrade_OptnPutCallInd());
                System.out.println("TradeDataDAO: PERMISSION_KEY="+ msgData.getTrade_PermissionKey());
                System.out.println("TradeDataDAO: PROFIT_CENTER="+msgData.getTrade_ProfitCenter());
                System.out.println("TradeDataDAO: CPTY_LEGAL_NAME="+msgData.getTrade_CptyLegalName());
                System.out.println("TradeDataDAO: QTY_DESC="+msgData.getTrade_QtyDesc());
                System.out.println("TradeDataDAO: TRADE_DESC="+msgData.getTrade_TradeDesc());
                System.out.println("TradeDataDAO: TRADER="+msgData.getTrade_Trader());
                System.out.println("TradeDataDAO: TRANSPORT_DESC="+msgData.getTrade_TransportDesc());
                System.out.println("TradeDataDAO: TRADE_ID="+tradeId);
                int iUpd = preparedStatement.executeUpdate();
                System.out.println("TradeDataDAO: Records Affected="+iUpd);
        } finally {
            try {
                if (preparedStatement != null)
                    preparedStatement.close();
            } catch (SQLException e) {
            }
        }
    }

    //using for VOID trade
    public void VoidData(ConfirmMessageData msgData) throws SQLException, ParseException {
        PreparedStatement preparedStatement = null;
        try {
            int tradeId = getTradeID(msgData.getTrade_TradingSystemTicket(), msgData.getTrade_TradingSystemCode());
            String stmtSQL =
                    "update " + schemaName + ".TRADE_DATA SET " +
                            "TRADE_STAT_CODE = ? " +
                            "where TRADE_ID = ?";

            preparedStatement = dbConnection.prepareStatement(stmtSQL);
            //ConversionUtils.setStatementString(1, msgData.getTrade_TradeStatCode(), preparedStatement);
            ConversionUtils.setStatementString(1, "VOID", preparedStatement);
            preparedStatement.setObject(2, tradeId, Types.NUMERIC);
            // System.out.println("JVC: set sql - " + stmtSQL);
            preparedStatement.executeUpdate();
        } finally {
            try {
                if (preparedStatement != null)
                    preparedStatement.close();
            } catch (SQLException e) {
            }
        }
    }

    //for testing only not to be confused with Voiding Data
    public void DeleteData(ConfirmMessageData msgData) throws SQLException, ParseException {
       // System.out.println("JVC: delete TRADE_DATA");
        PreparedStatement preparedStatement = null;
        try {
            int tradeId = getTradeID(msgData.getTrade_TradingSystemTicket(), msgData.getTrade_TradingSystemCode());
            //int tradeId = msgData.getOther_TradeID();
            String stmtSQL =
                    "Delete from " + schemaName + ".TRADE_DATA where " +
                            "TRADE_ID = ?";

            preparedStatement = dbConnection.prepareStatement(stmtSQL);
            ConversionUtils.setStatementString(1, msgData.getTrade_TradeStatCode(), preparedStatement);
            preparedStatement.setObject(1, tradeId, Types.NUMERIC);
            // System.out.println("JVC: set sql - " + stmtSQL);
            preparedStatement.executeUpdate();
        } finally {
            try {
                if (preparedStatement != null)
                    preparedStatement.close();
            } catch (SQLException e) {
            }
        }
    }


}


