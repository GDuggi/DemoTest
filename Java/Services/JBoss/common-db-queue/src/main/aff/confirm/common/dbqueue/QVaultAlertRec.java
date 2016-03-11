package aff.confirm.common.dbqueue;



/**
 * User: ifrankel
 * Date: Nov 25, 2003
 * Time: 7:02:49 AM
 * To change this template use Options | File Templates.
 */
public class QVaultAlertRec {
    public double id;
    public String tradingSystem;
    public double tradeId;
    public int version;
    public String fileName;
    public String idxFields;
    public String idxValues;
    public double logDealsheetId;
    public String title;
    public String author;
    public String docType;
    public String description;
    public String onBehalfOf;
    public String processedFlag;

    public QVaultAlertRec(double pId, String pTradingSystem, double pTradeId, int pVersion, String pFileName,
                          String pIdxFields, String pIdxValues, double pLogDealsheetId,
                          String pTitle, String pAuthor, String pDocType, String pDescription, String pOnBehalfOf) {
        this.setFields(pId, pTradingSystem, pTradeId, pVersion, pFileName, pIdxFields, pIdxValues, pLogDealsheetId,
                          pTitle, pAuthor, pDocType, pDescription, pOnBehalfOf);
    }

    public QVaultAlertRec() {
        this.init();
    }

    public void init(){
        this.id = 0;
        this.tradingSystem = "";
        this.tradeId = 0;
        this.version = 0;
        this.fileName = "";
        this.idxFields = "";
        this.idxValues = "";
        this.logDealsheetId = 0;
        this.processedFlag = "";
        this.title = "";
        this.author = "";
        this.docType = "";
        this.description = "";
        this.onBehalfOf = "";
    }

    public void setFields(double pId, String pTradingSystem, double pTradeId, int pVersion, String pFileName,
                          String pIdxFields, String pIdxValues, double pLogDealsheetId,
                          String pTitle, String pAuthor, String pDocType, String pDescription, String pOnBehalfOf){
        this.init();
        this.id = pId;
        this.tradingSystem = pTradingSystem;
        this.tradeId = pTradeId;
        this.version = pVersion;
        this.fileName = pFileName;
        this.idxFields = pIdxFields;
        this.idxValues = pIdxValues;
        this.logDealsheetId = pLogDealsheetId;
        this.title = pTitle;
        this.author = pAuthor;
        this.docType = pDocType;
        this.description = pDescription;
        this.onBehalfOf = pOnBehalfOf;
    }
}
