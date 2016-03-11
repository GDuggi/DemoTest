package aff.confirm.common.ottradealert;

import aff.confirm.common.ottradealert.exceptions.RowNotFoundException;
import org.apache.commons.httpclient.HttpClient;
import org.apache.commons.httpclient.methods.GetMethod;
import org.xml.sax.InputSource;
import org.xml.sax.SAXException;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import java.io.IOException;
import java.io.StringReader;
import java.sql.Date;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.text.DecimalFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Locale;
import org.jboss.logging.Logger;

/**
 * User: ifrankel
 * Date: Jun 10, 2003
 * Time: 9:41:30 AM
 */
public class TradingSystemDATA_dao {
    private static Logger log = Logger.getLogger(TradingSystemDATA_dao.class.getName() );
    private final SimpleDateFormat localSdfDate = new SimpleDateFormat("yyyy-MM-dd", Locale.US);
    private final SimpleDateFormat localSdfDateTime = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss", Locale.US);
    //private final String webServiceDateFormat = "yyyy-MM-dd'T'HH:mm:ss";
    private final String webServiceDateFormat = "yyyy-MM-dd";
    private final String webServiceDateTimeFormat = "yyyy-MM-dd hh:mm";
//    public final String webServiceDateFormat = "MMMM dd yyyy HH:mmaaa";
    private java.sql.Connection affConnection;
//    private java.sql.Connection symphonyConnection;
    private DecimalFormat df = new DecimalFormat("#0;-#0");
    //private CdtyCodeDAO cdtyCodeDAO;
    private String tradeDataWebServiceURL;
    private String tradeDataRootTagName;

    private static int  _ECONF_PHYS_HEAT_PROD = 503;
    private static int  _ECONF_FNCL_HEAT_PROD = 602;

    public TradingSystemDATA_dao(java.sql.Connection pAffConnection,
                                 /*java.sql.Connection pSymphonyConnection,*/
                                 String pTradeDataWebServiceURL,
                                 String pTradeDataRootTagName)
            throws SQLException {
        this.affConnection = pAffConnection;
//        this.symphonyConnection = pSymphonyConnection;
        this.tradeDataWebServiceURL = pTradeDataWebServiceURL;
        this.tradeDataRootTagName = pTradeDataRootTagName;
        //Initialize this here since each time the trade id changes, there's no need to refresh this.
        //cdtyCodeDAO = new CdtyCodeDAO(affConnection);
    };

    public TradingSystemDATA_rec getTradingSystemDATA_rec(String pTradingSystem, double pTradeId, String pTradeTypeCode)
            throws SQLException, RowNotFoundException, Exception {
        TradingSystemDATA_rec tsDATA_rec;
        tsDATA_rec = new TradingSystemDATA_rec();

        // Israel 2/5/2015 -- Get web service calling code from ConfirmDemoSys version
        //tsDATA_rec = getDataFromWebService(pTradingSystem, pTradeId);

        //if (pTradingSystem.equalsIgnoreCase("SYM"))
        //    tsDATA_rec = getSymphonyDataFromWebService(pTradeId);
        //else if (pTradingSystem.equalsIgnoreCase("AFF"))
        tsDATA_rec = getAffData(pTradeId, pTradeTypeCode);

        return tsDATA_rec;
    }

    private TradingSystemDATA_rec getDataRecFromXml(String pMethodParm, String pXml)
            throws ParserConfigurationException, ParseException, SQLException, IOException, SAXException {
        TradingSystemDATA_rec tsDATA_rec;
        tsDATA_rec = new TradingSystemDATA_rec();
        //java.util.Date currentTime = new java.util.Date();
        String strDate = "";
        String strTradeId = "";

        DocumentBuilderFactory dbFactory = DocumentBuilderFactory.newInstance();
        DocumentBuilder dBuilder = dbFactory.newDocumentBuilder();
        org.w3c.dom.Document doc = dBuilder.parse(new InputSource(new StringReader(pXml)));

        if (doc.getDocumentElement().getNodeName().equalsIgnoreCase("error")){
            String errorCode = doc.getElementsByTagName("code").item(0).getTextContent();
            String errorMessage = doc.getElementsByTagName("message").item(0).getTextContent();
            log.error( "Error reading XML from WebService: Code=" + errorCode + ", errorMessage=" + errorMessage + ", Calling method parm: " + pMethodParm );
        }
        else {
            String s = doc.getElementsByTagName("trade_id").item(0).getTextContent();

            tsDATA_rec.TRADE_ID = Double.parseDouble(doc.getElementsByTagName("trade_id").item(0).getTextContent());

            strDate = doc.getElementsByTagName("inception_dt").item(0).getTextContent();
            if (!strDate.isEmpty())
                tsDATA_rec.INCEPTION_DT = tsDATA_rec.getJavaSqlDateFromXmlDate(strDate, webServiceDateFormat);

            tsDATA_rec.CDTY_CODE = doc.getElementsByTagName("cdty_code").item(0).getTextContent();

            strDate = doc.getElementsByTagName("trade_dt").item(0).getTextContent();
            if (!strDate.isEmpty())
                tsDATA_rec.TRADE_DT = tsDATA_rec.getJavaSqlDateFromXmlDate(strDate, webServiceDateFormat);

            tsDATA_rec.XREF = doc.getElementsByTagName("xref").item(0).getTextContent();
            tsDATA_rec.CPTY_SN = doc.getElementsByTagName("cpty_sn").item(0).getTextContent();

            if (doc.getElementsByTagName("qty_tot").item(0).getTextContent().isEmpty())
                tsDATA_rec.QTY_TOT = 0;
            else
                tsDATA_rec.QTY_TOT =  Double.parseDouble(doc.getElementsByTagName("qty_tot").item(0).getTextContent());

            if (doc.getElementsByTagName("qty").item(0).getTextContent().isEmpty())
                tsDATA_rec.QTY = 0;
            else
                tsDATA_rec.QTY = Double.parseDouble(doc.getElementsByTagName("qty").item(0).getTextContent());

            tsDATA_rec.UOM_DUR_CODE = doc.getElementsByTagName("uom_dur_code").item(0).getTextContent();
            tsDATA_rec.setLOCATION_SN(doc.getElementsByTagName("location_sn").item(0).getTextContent());
            tsDATA_rec.PRICE_DESC = doc.getElementsByTagName("price_desc").item(0).getTextContent();

            strDate = doc.getElementsByTagName("start_dt").item(0).getTextContent();
            if (!strDate.isEmpty())
                tsDATA_rec.START_DT = tsDATA_rec.getJavaSqlDateFromXmlDate(strDate, webServiceDateFormat);

            strDate = doc.getElementsByTagName("end_dt").item(0).getTextContent();
            if (!strDate.isEmpty())
                tsDATA_rec.END_DT = tsDATA_rec.getJavaSqlDateFromXmlDate(strDate, webServiceDateFormat);

            tsDATA_rec.BOOK = doc.getElementsByTagName("book").item(0).getTextContent();
            tsDATA_rec.TRADE_TYPE_CODE = doc.getElementsByTagName("trade_type_code").item(0).getTextContent();
            tsDATA_rec.STTL_TYPE = doc.getElementsByTagName("sttl_type").item(0).getTextContent();
            tsDATA_rec.BROKER_SN = doc.getElementsByTagName("broker_short_name").item(0).getTextContent();
            tsDATA_rec.COMM = doc.getElementsByTagName("comm").item(0).getTextContent();
            tsDATA_rec.setBUY_SELL_IND(doc.getElementsByTagName("buy_sell_ind").item(0).getTextContent());
            tsDATA_rec.REF_SN = doc.getElementsByTagName("ref_sn").item(0).getTextContent();
            tsDATA_rec.PAY_PRICE = doc.getElementsByTagName("pay_price").item(0).getTextContent();
            tsDATA_rec.REC_PRICE = doc.getElementsByTagName("rec_price").item(0).getTextContent();
            tsDATA_rec.SE_CPTY_SN = doc.getElementsByTagName("se_cpty_sn").item(0).getTextContent();
            tsDATA_rec.TRADE_STAT_CODE = doc.getElementsByTagName("trade_stat_code").item(0).getTextContent();
            //tsDATA_rec.CDTY_GRP_CODE = getSymphonyCdtyGrpCode(cdtyCode);
            tsDATA_rec.OPTN_PREM_PRICE = doc.getElementsByTagName("optn_prem_price").item(0).getTextContent();
            tsDATA_rec.OPTN_PUT_CALL_IND = doc.getElementsByTagName("optn_put_call_ind").item(0).getTextContent();
            tsDATA_rec.OPTN_STRIKE_PRICE = doc.getElementsByTagName("optn_strike_price").item(0).getTextContent();
            tsDATA_rec.BROKER_PRICE = doc.getElementsByTagName("broker_price").item(0).getTextContent();
        }

        //System.out.println( tsDATA_rec.ShowValues() );
        return tsDATA_rec;
    }

    private TradingSystemDATA_rec getDataFromWebService(String pTradingSystem, double pTradeID) throws Exception{
        TradingSystemDATA_rec tsDATA_rec;
        int tradeId = (int) pTradeID;
        String cdtyGrpCode = "";
        String parmTrdSysCode = "?tradesyscode=" + pTradingSystem;
        String parmTradeId = "&ticket=" + tradeId;
        String methodParm = tradeDataWebServiceURL + parmTrdSysCode + parmTradeId;

        GetMethod method = new GetMethod(methodParm);
        HttpClient client = new HttpClient();
        client.executeMethod(method);
        String xmlResult = method.getResponseBodyAsString();

        tsDATA_rec = getDataRecFromXml(methodParm, xmlResult);
        tsDATA_rec.tradingSystem = pTradingSystem;
        //tsDATA_rec.TRADE_ID = pTradeID;
        if (pTradingSystem.equalsIgnoreCase("SYM")){
            tsDATA_rec.CDTY_GRP_CODE = getSymphonyCdtyGrpCode(tsDATA_rec.CDTY_CODE);
            tsDATA_rec.EFS_FLAG = "Y";
            tsDATA_rec.EFS_CPTY_SN = "";
        }
        else if (pTradingSystem.equalsIgnoreCase("AFF")){
            cdtyGrpCode = tsDATA_rec.CDTY_GRP_CODE;
            tsDATA_rec.CDTY_GRP_CODE = getAffCdtyGrpCode(tsDATA_rec.CDTY_CODE, cdtyGrpCode);

            // 5/8/2007 Israel - support EFS trades
            if (tsDATA_rec.TRADE_TYPE_CODE.equalsIgnoreCase("FWD")){
                tsDATA_rec.EFS_FLAG = "N";
                tsDATA_rec.EFS_CPTY_SN = "";
            }
        }

        return tsDATA_rec;
    }

    /*private TradingSystemDATA_rec getSymphonyDataFromWebService(double pTradeID) throws Exception{
        TradingSystemDATA_rec tsDATA_rec;
        IOpsDataFeedProxy stub = new IOpsDataFeedProxy(tradeDataWebServiceURL);
        int tradeId = (int) pTradeID;
        String xmlResult = stub.getOpsTrackingTrade(tradeId);

        tsDATA_rec = getDataRecFromXml(xmlResult);
        tsDATA_rec.sourceSystemCode = "SYM";
        //tsDATA_rec.TRADE_ID = pTradeID;
        tsDATA_rec.CDTY_GRP_CODE = getSymphonyCdtyGrpCode(tsDATA_rec.CDTY_CODE);
        tsDATA_rec.EFS_FLAG = "Y";
        tsDATA_rec.EFS_CPTY_SN = "";

        return tsDATA_rec;
    }*/

    private TradingSystemDATA_rec getSymphonyData(double pTradeID)
            throws SQLException, RowNotFoundException {
        TradingSystemDATA_rec tsDATA_rec;
        tsDATA_rec = new TradingSystemDATA_rec();
        PreparedStatement statement = null;
        ResultSet rs = null;
        boolean foundIt = false;
        try {
            String selectSQL;
            //String tradeID = null;
            String cdtyCode = "";
            //tradeID = df.format(pTradeID);
            selectSQL = "exec dbo.trade_alert_ops_track ?";
//            statement = symphonyConnection.prepareStatement(selectSQL);
            statement = null;
            statement.setDouble(1, pTradeID);
            rs = statement.executeQuery();

            while (rs.next()) {
                foundIt = true;
                tsDATA_rec.tradingSystem = "SYM";
                tsDATA_rec.TRADE_ID = pTradeID;
                tsDATA_rec.INCEPTION_DT = rs.getDate("INCEPTION_DT");
                //This is used below to get the cdtyGroupCode
                cdtyCode = rs.getString("cdty_code");
                tsDATA_rec.CDTY_CODE = cdtyCode;
                tsDATA_rec.TRADE_DT = rs.getDate("trade_dt");
                tsDATA_rec.XREF = rs.getString("xref");
                tsDATA_rec.CPTY_SN = rs.getString("cpty_sn");
                tsDATA_rec.QTY_TOT = rs.getDouble("qty_tot");
                tsDATA_rec.QTY = rs.getDouble("qty");
                tsDATA_rec.UOM_DUR_CODE = rs.getString("uom_dur_code");
                tsDATA_rec.setLOCATION_SN(rs.getString("location_sn"));
                tsDATA_rec.PRICE_DESC = rs.getString("price_desc");
                tsDATA_rec.START_DT = rs.getDate("start_dt");
                tsDATA_rec.END_DT = rs.getDate("end_dt");
                tsDATA_rec.BOOK = rs.getString("book");
                tsDATA_rec.TRADE_TYPE_CODE = rs.getString("trade_type_code");
                tsDATA_rec.STTL_TYPE = rs.getString("sttl_type");
                tsDATA_rec.BROKER_SN = rs.getString("broker_short_name");
                tsDATA_rec.COMM = rs.getString("comm");
                tsDATA_rec.setBUY_SELL_IND( rs.getString("buy_sell_ind"));
                tsDATA_rec.REF_SN = rs.getString("ref_sn");
                tsDATA_rec.PAY_PRICE = rs.getString("pay_price");
                tsDATA_rec.REC_PRICE = rs.getString("rec_price");
                tsDATA_rec.SE_CPTY_SN = rs.getString("se_cpty_sn");
                tsDATA_rec.TRADE_STAT_CODE = rs.getString("trade_stat_code");
                tsDATA_rec.CDTY_GRP_CODE = getSymphonyCdtyGrpCode(cdtyCode);
                tsDATA_rec.OPTN_PREM_PRICE = rs.getString("OPTN_PREM_PRICE");
                tsDATA_rec.OPTN_PUT_CALL_IND = rs.getString("OPTN_PUT_CALL_IND");
                tsDATA_rec.OPTN_STRIKE_PRICE = rs.getString("OPTN_STRIKE_PRICE");
                tsDATA_rec.BROKER_PRICE = rs.getString("BROKER_PRICE");
                // 5/8/2007 Israel - support EFS trades
                // 10/26/2012 Israel - stubbed out EFS pending final resolution of web service deployment.
                //tsDATA_rec.EFS_FLAG = rs.getString("efs_ind");
                tsDATA_rec.EFS_FLAG = "Y";
                tsDATA_rec.EFS_CPTY_SN = "";
                //foundIt = false;  //Testing stub
            }
            if (!foundIt)
                throw new RowNotFoundException("TradingSystemDATA_dao.getSymphonyData failed. SQL statement=" +
                        selectSQL + " for TradeID=" + pTradeID);
        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) {
                //e.printStackTrace();
            }
            statement = null;

            try {
                if (rs != null)
                    rs.close();
            } catch (SQLException e) {
                //e.printStackTrace();
            }
            rs = null;
        }
        return tsDATA_rec;
    }

    private String getSymphonyCdtyGrpCode(String pCdtyCode)
            throws SQLException {
        String cdtyCode = "";
        String cdtyGrpCode = "";

        //For some trades the cdty code is null from Sybase.
        try {
            cdtyCode = pCdtyCode.trim();
        } catch (NullPointerException e) {
            cdtyCode = "";
        }

        if (cdtyCode != null && !cdtyCode.equalsIgnoreCase("")) {
            //cdtyGrpCode = cdtyCodeDAO.getCdtyGrpCode(cdtyCode);

            //if there is no group code or it is various, make it OTHER
            if (cdtyGrpCode == null ||
                    cdtyGrpCode.equalsIgnoreCase("") ||
                    cdtyGrpCode.equalsIgnoreCase("VARIOUS"))
                cdtyGrpCode = "OTHER";
        }
        else
            cdtyGrpCode = "OTHER";

        return cdtyGrpCode;
    }

    private TradingSystemDATA_rec getAffData(double pTradeID, String pTradeTypeCode)
            throws SQLException, RowNotFoundException {
        TradingSystemDATA_rec tsDATA_rec = new TradingSystemDATA_rec();
        PreparedStatement statement = null;
        ResultSet rs = null;
        boolean foundIt = false;
        String cdtyGrpCode = "";

        int counter = 1;
        int maxCounter = 30;


        while (foundIt == false)
        {
            try {
                String selectSQL;
                selectSQL = getAffDataSelectSQL(pTradeTypeCode);

                statement = affConnection.prepareStatement(selectSQL);
                statement.setDouble(1, pTradeID);
                rs = statement.executeQuery();

                while (rs.next()) {
                    foundIt = true;
                    tsDATA_rec.tradingSystem = "AFF";
                    tsDATA_rec.TRADE_ID = pTradeID;
                    tsDATA_rec.INCEPTION_DT = rs.getDate("INCEPTION_DT");
                    tsDATA_rec.CDTY_CODE = rs.getString("CDTY_CODE");
                    tsDATA_rec.TRADE_DT = rs.getDate("TRADE_DT");
                    tsDATA_rec.XREF = rs.getString("XREF");
                    tsDATA_rec.CPTY_SN = rs.getString("CPTY_SN");
                    tsDATA_rec.QTY_TOT = rs.getDouble("TOTAL_NOM_QTY");
                    tsDATA_rec.QTY = rs.getDouble("QTY_PER");
                    tsDATA_rec.UOM_DUR_CODE = rs.getString("UOM_DUR_CODE");
                    tsDATA_rec.setLOCATION_SN( rs.getString("LOCATION_SN"));
                    tsDATA_rec.PRICE_DESC = "";
                    tsDATA_rec.START_DT = rs.getDate("START_DT");
                    tsDATA_rec.END_DT = rs.getDate("END_DT");
                    tsDATA_rec.BOOK = rs.getString("BOOK");
                    tsDATA_rec.TRADE_TYPE_CODE = rs.getString("TRADE_TYPE_CODE");
                    tsDATA_rec.STTL_TYPE = rs.getString("TRADE_STTL_TYPE_CODE");
                    tsDATA_rec.BROKER_SN = rs.getString("BROKER_SN");
                    tsDATA_rec.COMM = rs.getString("BKRG_RATE");
                    tsDATA_rec.setBUY_SELL_IND( rs.getString("BUY_SELL_IND"));
                    tsDATA_rec.REF_SN = rs.getString("REFERENCE");
                    tsDATA_rec.PAY_PRICE = rs.getString("PAY_PRICE");
                    tsDATA_rec.REC_PRICE = rs.getString("REC_PRICE");
                    tsDATA_rec.SE_CPTY_SN = rs.getString("SE_CPTY_SN");
                    tsDATA_rec.TRADE_STAT_CODE = rs.getString("TRADE_STAT_CODE");

                    cdtyGrpCode = rs.getString("CDTY_GRP_CODE");
                    tsDATA_rec.CDTY_GRP_CODE = getAffCdtyGrpCode(tsDATA_rec.CDTY_CODE, cdtyGrpCode);

                    tsDATA_rec.OPTN_PREM_PRICE = rs.getString("OPTN_PREM_PRICE");
                    tsDATA_rec.OPTN_PUT_CALL_IND = rs.getString("OPTN_PUT_CALL_IND");
                    tsDATA_rec.OPTN_STRIKE_PRICE = rs.getString("OPTN_STRIKE_PRICE");
                    tsDATA_rec.BROKER_PRICE = rs.getString("BROKER_PRICE");
                    // 5/8/2007 Israel - support EFS trades
                    if (!pTradeTypeCode.equalsIgnoreCase("FWD")){
                        tsDATA_rec.EFS_FLAG = rs.getString("EFS_FLAG");
                        tsDATA_rec.EFS_CPTY_SN = rs.getString("EFS_CPTY_SN");
                    }
                    else {
                        tsDATA_rec.EFS_FLAG = "N";
                        tsDATA_rec.EFS_CPTY_SN = "";
                    }
                    tsDATA_rec.TEST_BOOK_FLAG = "Y".equalsIgnoreCase(rs.getString("BK_TEST_BOOK_FLAG"));
                }

                if (!foundIt){
                    if(counter >= maxCounter){
                        throw new RowNotFoundException("TradingSystemDATA_dao.getAffData failed. SQL statement=" +
                                selectSQL + " for TradeID=" + pTradeID);
                    } else {
                        counter = counter + 1;
                        try {
                            Thread.sleep(2000);
                        } catch (InterruptedException e) {
                            log.error( "ERROR", e);
                        }
                    }
                }

            } finally {
                try {
                    if (statement != null)
                        statement.close();
                } catch (SQLException e) { }
                statement = null;

                try {
                    if (rs != null)
                        rs.close();
                } catch (SQLException e) { }
                rs = null;
            }
        }
        return tsDATA_rec;
    }

    private String getAffCdtyGrpCode(String pCdtyCode, String pCdtyGrpCode)
            throws SQLException {
        String cdtyCode = "";
        String cdtyGrpCode = "";

        //For some trades the cdty code is null from Sybase.
        try {
            cdtyCode = pCdtyCode;
        } catch (NullPointerException e){
            cdtyCode = "";
        }

        try {
            cdtyGrpCode = pCdtyGrpCode;
        } catch (NullPointerException e){
            cdtyGrpCode = "";
        }

        //There's a valid cdtyGrpCode so use it, unless it's VARIOUS then convert it.
        if ( cdtyGrpCode != null && !cdtyGrpCode.equalsIgnoreCase("")){
            if (cdtyGrpCode.equalsIgnoreCase("VARIOUS"))
                cdtyGrpCode = "OTHER";
        }
        //if not, and there's a cdtyCode, then use it to find the grp code
        //else if (( cdtyCode != null && !cdtyCode.equalsIgnoreCase("")) ){
          //  cdtyGrpCode = cdtyCodeDAO.getCdtyGrpCode(cdtyCode);
        //}
        //if cdty_code is empty then make the group code OTHER
        else
            //pStatement.setNull(pParmNo, Types.VARCHAR);
            cdtyGrpCode = "OTHER";

        return cdtyGrpCode;
    }

    private String getAffDataSelectSQL(String pTradeTypeCode) {
        String selectSQL;

        if (pTradeTypeCode.equalsIgnoreCase("BASKT")){
            selectSQL = "Select * from infinity_mgr.v_ops_tracking_data_bskt where prmnt_trade_id = ?";
        }
        else if (pTradeTypeCode.equalsIgnoreCase("FWD")) {
            selectSQL = "Select * from infinity_mgr.v_ops_tracking_data_fx where prmnt_trade_id = ?";
        }
        else{
            selectSQL = "Select * from infinity_mgr.v_ops_tracking_data where prmnt_trade_id = ?";
        }
        return selectSQL;
    }

    private String getNoNull(String pFieldValue){
        if (pFieldValue == null)
            return "";
        else
            return pFieldValue;
    }

    private Date getNoNull(Date pFieldValue) {
        Date returnVal = null;
        try {
            returnVal = java.sql.Date.valueOf( "2001-01-01" );
            if (pFieldValue != null) {
                return pFieldValue;
            } else
                return returnVal;
        }
        catch (Exception e) {
            return returnVal;
        }
    }

    public boolean isOTTradeDataOnlyXREFChanged(TradingSystemDATA_rec pTSDataRec, OpsTrackingTRADE_DATA_rec pOTDataRec)
            throws SQLException {
        boolean isTradeDataExceptXREFSame = false;
        boolean isXREFSame = false;
        boolean isOnlyXREFChanged = false;
        isTradeDataExceptXREFSame =
                (pOTDataRec.TRADE_ID == pTSDataRec.TRADE_ID) &&
                (getNoNull(pOTDataRec.INCEPTION_DT).compareTo(getNoNull(pTSDataRec.INCEPTION_DT)) == 0) &&
                (getNoNull(pOTDataRec.CDTY_CODE).equalsIgnoreCase(getNoNull(pTSDataRec.CDTY_CODE))) &&
                (getNoNull(pOTDataRec.TRADE_DT).compareTo(getNoNull(pTSDataRec.TRADE_DT)) == 0) &&
                //(getNoNull(pOTDataRec.XREF).equalsIgnoreCase(getNoNull(pTSDataRec.XREF))) &&
                (getNoNull(pOTDataRec.CPTY_SN).equalsIgnoreCase(getNoNull(pTSDataRec.CPTY_SN))) &&
                (pOTDataRec.QTY_TOT == pTSDataRec.QTY_TOT) &&
                (pOTDataRec.QTY == pTSDataRec.QTY) &&
                (getNoNull(pOTDataRec.UOM_DUR_CODE).equalsIgnoreCase(getNoNull(pTSDataRec.UOM_DUR_CODE))) &&
                (getNoNull(pOTDataRec.getLOCATION_SN()).equalsIgnoreCase(getNoNull(pTSDataRec.getLOCATION_SN()))) &&
                (getNoNull(pOTDataRec.PRICE_DESC).equalsIgnoreCase(getNoNull(pTSDataRec.PRICE_DESC))) &&
                (getNoNull(pOTDataRec.START_DT).compareTo(getNoNull(pTSDataRec.START_DT)) == 0) &&
                (getNoNull(pOTDataRec.END_DT).compareTo(getNoNull(pTSDataRec.END_DT)) == 0) &&
                (getNoNull(pOTDataRec.BOOK).equalsIgnoreCase(getNoNull(pTSDataRec.BOOK))) &&
                (getNoNull(pOTDataRec.TRADE_TYPE_CODE).equalsIgnoreCase(getNoNull(pTSDataRec.TRADE_TYPE_CODE))) &&
                (getNoNull(pOTDataRec.STTL_TYPE).equalsIgnoreCase(getNoNull(pTSDataRec.STTL_TYPE))) &&
                (getNoNull(pOTDataRec.BROKER_SN).equalsIgnoreCase(getNoNull(pTSDataRec.BROKER_SN))) &&
                (getNoNull(pOTDataRec.COMM).equalsIgnoreCase(getNoNull(pTSDataRec.COMM))) &&
                (pOTDataRec.getBUY_SELL_IND().equalsIgnoreCase(pTSDataRec.getBUY_SELL_IND())) &&
                (getNoNull(pOTDataRec.REF_SN).equalsIgnoreCase(getNoNull(pTSDataRec.REF_SN))) &&
                (getNoNull(pOTDataRec.PAY_PRICE).equalsIgnoreCase(getNoNull(pTSDataRec.PAY_PRICE))) &&
                (getNoNull(pOTDataRec.REC_PRICE).equalsIgnoreCase(getNoNull(pTSDataRec.REC_PRICE))) &&
                (getNoNull(pOTDataRec.SE_CPTY_SN).equalsIgnoreCase(getNoNull(pTSDataRec.SE_CPTY_SN))) &&
                (getNoNull(pOTDataRec.TRADE_STAT_CODE).equalsIgnoreCase(getNoNull(pTSDataRec.TRADE_STAT_CODE))) &&
                (getNoNull(pOTDataRec.CDTY_GRP_CODE).equalsIgnoreCase(getNoNull(pTSDataRec.CDTY_GRP_CODE))) &&
                (getNoNull(pOTDataRec.BROKER_PRICE).equalsIgnoreCase(getNoNull(pTSDataRec.BROKER_PRICE))) &&
                (getNoNull(pOTDataRec.OPTN_PREM_PRICE).equalsIgnoreCase(getNoNull(pTSDataRec.OPTN_PREM_PRICE))) &&
                (getNoNull(pOTDataRec.OPTN_PUT_CALL_IND).equalsIgnoreCase(getNoNull(pTSDataRec.OPTN_PUT_CALL_IND))) &&
                (getNoNull(pOTDataRec.OPTN_STRIKE_PRICE).equalsIgnoreCase(getNoNull(pTSDataRec.OPTN_STRIKE_PRICE))) &&
                (getNoNull(pOTDataRec.EFS_FLAG).equalsIgnoreCase(getNoNull(pTSDataRec.EFS_FLAG))) &&
                (getNoNull(pOTDataRec.EFS_CPTY_SN).equalsIgnoreCase(getNoNull(pTSDataRec.EFS_CPTY_SN)));
            isXREFSame = (getNoNull(pOTDataRec.XREF).equalsIgnoreCase(getNoNull(pTSDataRec.XREF)));
            isOnlyXREFChanged = isTradeDataExceptXREFSame && !isXREFSame;
        return isOnlyXREFChanged;
    }

    public boolean isOTTradeDataAnyChanged(TradingSystemDATA_rec pTSDataRec, OpsTrackingTRADE_DATA_rec pOTDataRec)
            throws SQLException {
        boolean isTradeDataSame = false;
        isTradeDataSame =
                (pOTDataRec.TRADE_ID == pTSDataRec.TRADE_ID) &&
                (getNoNull(pOTDataRec.INCEPTION_DT).compareTo(getNoNull(pTSDataRec.INCEPTION_DT)) == 0) &&
                (getNoNull(pOTDataRec.CDTY_CODE).equalsIgnoreCase(getNoNull(pTSDataRec.CDTY_CODE))) &&
                (getNoNull(pOTDataRec.TRADE_DT).compareTo(getNoNull(pTSDataRec.TRADE_DT)) == 0) &&
                (getNoNull(pOTDataRec.XREF).equalsIgnoreCase(getNoNull(pTSDataRec.XREF))) &&
                (getNoNull(pOTDataRec.CPTY_SN).equalsIgnoreCase(getNoNull(pTSDataRec.CPTY_SN))) &&
                (pOTDataRec.QTY_TOT == pTSDataRec.QTY_TOT) &&
                (pOTDataRec.QTY == pTSDataRec.QTY) &&
                (getNoNull(pOTDataRec.UOM_DUR_CODE).equalsIgnoreCase(getNoNull(pTSDataRec.UOM_DUR_CODE))) &&
                (getNoNull(pOTDataRec.getLOCATION_SN()).equalsIgnoreCase(getNoNull(pTSDataRec.getLOCATION_SN()))) &&
                (getNoNull(pOTDataRec.PRICE_DESC).equalsIgnoreCase(getNoNull(pTSDataRec.PRICE_DESC))) &&
                (getNoNull(pOTDataRec.START_DT).compareTo(getNoNull(pTSDataRec.START_DT)) == 0) &&
                (getNoNull(pOTDataRec.END_DT).compareTo(getNoNull(pTSDataRec.END_DT)) == 0) &&
                (getNoNull(pOTDataRec.BOOK).equalsIgnoreCase(getNoNull(pTSDataRec.BOOK))) &&
                (getNoNull(pOTDataRec.TRADE_TYPE_CODE).equalsIgnoreCase(getNoNull(pTSDataRec.TRADE_TYPE_CODE))) &&
                (getNoNull(pOTDataRec.STTL_TYPE).equalsIgnoreCase(getNoNull(pTSDataRec.STTL_TYPE))) &&
                (getNoNull(pOTDataRec.BROKER_SN).equalsIgnoreCase(getNoNull(pTSDataRec.BROKER_SN))) &&
                (getNoNull(pOTDataRec.COMM).equalsIgnoreCase(getNoNull(pTSDataRec.COMM))) &&
                (pOTDataRec.getBUY_SELL_IND().equalsIgnoreCase(pTSDataRec.getBUY_SELL_IND())) &&
                (getNoNull(pOTDataRec.REF_SN).equalsIgnoreCase(getNoNull(pTSDataRec.REF_SN))) &&
                (getNoNull(pOTDataRec.PAY_PRICE).equalsIgnoreCase(getNoNull(pTSDataRec.PAY_PRICE))) &&
                (getNoNull(pOTDataRec.REC_PRICE).equalsIgnoreCase(getNoNull(pTSDataRec.REC_PRICE))) &&
                (getNoNull(pOTDataRec.SE_CPTY_SN).equalsIgnoreCase(getNoNull(pTSDataRec.SE_CPTY_SN))) &&
                (getNoNull(pOTDataRec.TRADE_STAT_CODE).equalsIgnoreCase(getNoNull(pTSDataRec.TRADE_STAT_CODE))) &&
                (getNoNull(pOTDataRec.CDTY_GRP_CODE).equalsIgnoreCase(getNoNull(pTSDataRec.CDTY_GRP_CODE))) &&
                (getNoNull(pOTDataRec.BROKER_PRICE).equalsIgnoreCase(getNoNull(pTSDataRec.BROKER_PRICE))) &&
                (getNoNull(pOTDataRec.OPTN_PREM_PRICE).equalsIgnoreCase(getNoNull(pTSDataRec.OPTN_PREM_PRICE))) &&
                (getNoNull(pOTDataRec.OPTN_PUT_CALL_IND).equalsIgnoreCase(getNoNull(pTSDataRec.OPTN_PUT_CALL_IND))) &&
                (getNoNull(pOTDataRec.OPTN_STRIKE_PRICE).equalsIgnoreCase(getNoNull(pTSDataRec.OPTN_STRIKE_PRICE))) &&
                (getNoNull(pOTDataRec.EFS_FLAG).equalsIgnoreCase(getNoNull(pTSDataRec.EFS_FLAG))) &&
                (getNoNull(pOTDataRec.EFS_CPTY_SN).equalsIgnoreCase(getNoNull(pTSDataRec.EFS_CPTY_SN)));
        // samy 10/05/2009 add to check for heat rate trades
        // Israel 6/22/15 -- Removed
/*        if ( isTradeDataSame) {
            if (isHeatRateProcessRequired(pTSDataRec.CDTY_CODE,pTSDataRec.TRADE_ID)){
                isTradeDataSame = false;
            }
        }*/
        return !isTradeDataSame;
    }

    private boolean isHeatRateProcessRequired(String cdtyCode, double tradeId) {
        boolean required = false;
        String sql = "select price_cnv_factor " +
                     " from infinity_mgr.trade_leg tl, " +
                    " infinity_mgr.trade_leg_price tlp " +
                    " where  tl.exp_dt = '31-DEC-2299' " +
                    " and tl.active_flag = 'Y' " +
                    " and tl.prmnt_id = tlp.prmnt_trade_leg_id " +
                    " and tlp.exp_dt = '31-DEC-2299' " +
                    " and tlp.active_flag = 'Y' " +
                    " and tl.prmnt_trade_id = ? ";
        String eConfSql = "select product_id from econfirm.ec_trade_summary where trade_id = ?";

        PreparedStatement ps = null;
        ResultSet rs = null;

        PreparedStatement psEConf = null;
        ResultSet rsEConf = null;

        double heatRate = 1;
        
        if ("ELEC".equalsIgnoreCase(cdtyCode)){

            try {
                ps = this.affConnection.prepareStatement(sql);
                ps.setDouble(1  ,tradeId);
                rs = ps.executeQuery();
                while ( rs.next()){
                    heatRate = rs.getDouble("price_cnv_factor");
                    if ( heatRate > 1) {
                        required = true;
                        break;
                    }
                }
                // if the heat rate is 1, then check whether this trade
                // was eConfirm heat rate before
                if ( !required) {
                    psEConf = this.affConnection.prepareStatement(eConfSql);
                    psEConf.setDouble(1,tradeId);
                    rsEConf = psEConf.executeQuery();
                    if ( rsEConf.next()) {
                        int productId = rsEConf.getInt("product_id");
                        if (productId == _ECONF_PHYS_HEAT_PROD  || productId ==_ECONF_FNCL_HEAT_PROD ) {
                            required = true;
                        }
                    }
                    
                }

            }
            catch (Exception e){

            }
            finally {
                try {

                    if ( rsEConf != null){
                         rsEConf.close();
                    }
                    if ( psEConf != null){
                         psEConf.close();
                    }
                    if (rs != null){
                        rs.close();
                    }
                    if ( ps != null){
                        ps.close();
                    }
                    rsEConf = null;
                    psEConf = null;
                    rs  = null;
                    ps = null;
                }
                catch (Exception e) {
                    
                }

            }
        }

        return required;
    }

    public boolean isExoticBasketTrade(double tradeId) throws SQLException {

       // this function is used to process the basket trades and basker
        // memeber trades, the basket trades are generally ignored
        // except exotic basket trades.
        boolean exoticBasket = false;
        String sql = "select btp.code,btp.notify_contracts_flag " +
                     " from infinity_mgr.trade t," +
                     " infinity_mgr.basket_type btp," +
                     " infinity_mgr.basket_trade bt " +
                     " where t.prmnt_id = ? " +
                     " and t.exp_dt = '31-dec-2299' " +
                     " and bt.exp_dt = '31-dec-2299' " +
                     " and bt.prmnt_trade_id = t.prmnt_id " +
                     " and btp.code = bt.basket_type_code";

        PreparedStatement ps = null;
        ResultSet rs = null;

        try {
              ps = this.affConnection.prepareStatement(sql);
              ps.setDouble(1  ,tradeId);
              rs = ps.executeQuery();
              if  ( rs.next()){
                  exoticBasket = "Y".equalsIgnoreCase(rs.getString("notify_contracts_flag"));
              }
        }
        finally {
            try {
                if (rs != null){
                    rs.close();
                }
                if (ps != null){
                    ps.close();
                }
                rs = null;
                ps = null;
            }
            catch (Exception e){

            }
        }
        return exoticBasket;

    }

    public String getBasketMemberProcessingFlag(double tradeId) throws SQLException {
        // returns the a given trade is a member exotic member
        // id, these exotic member trades need to be removed
        // from confirmation.
        String processFlag = "N";
        String sql = "select t.trade_stat_code,t.parent_basket_trade_id from " +
                     " infinity_mgr.trade t " +
                     " where t.prmnt_id = ? " +
                     " order by version desc";

        
        PreparedStatement ps = null;
        ResultSet rs = null;
        String exoticBasketFlag = "Y";
        String currentTradeStatus = "OPEN";

        try {
              ps = this.affConnection.prepareStatement(sql);
              ps.setDouble(1  ,tradeId);
              rs = ps.executeQuery();
              while  ( rs.next()){
                  double parentTradeId = 0;
                  if ("VOID".equalsIgnoreCase(rs.getString("trade_stat_code"))) {
                     currentTradeStatus = "VOID"; 
                  }
                  parentTradeId = rs.getDouble("parent_basket_trade_id");
                  if ( parentTradeId > 0 && !"VOID".equalsIgnoreCase(currentTradeStatus)) {
                      processFlag = isExoticBasketTrade(parentTradeId)?exoticBasketFlag:"N";
                      break;
                  }
                  exoticBasketFlag = "R"; // removed the trade from exotic basket.

              }
        }
        finally {
            try {
                if (rs != null){
                    rs.close();
                }
                if (ps != null){
                    ps.close();
                }
                rs = null;
                ps = null;
            }
            catch (Exception e){

            }
        }
        return processFlag;
    }

    public boolean isExoticBasketMember(double tradeId) throws SQLException {

        String sql = "select t.parent_basket_trade_id from " +
                     " infinity_mgr.trade t " +
                     " where t.prmnt_id = ? " +
                     " and exp_dt = '31-dec-2299'";

        PreparedStatement ps = null;
        ResultSet rs = null;
        boolean exoticBasketMember = false;


        try {
              ps = this.affConnection.prepareStatement(sql);
              ps.setDouble(1  ,tradeId);
              rs = ps.executeQuery();
              if  ( rs.next()){
                  double parentTradeId = 0;
                  parentTradeId = rs.getDouble("parent_basket_trade_id");
                  if ( parentTradeId > 0 ) {
                      exoticBasketMember = isExoticBasketTrade(parentTradeId);
                  }
              }
        }
        finally {
            try {
                if (rs != null){
                    rs.close();
                }
                if (ps != null){
                    ps.close();
                }
                rs = null;
                ps = null;
            }
            catch (Exception e){

            }
        }
        return exoticBasketMember;
    }

}
