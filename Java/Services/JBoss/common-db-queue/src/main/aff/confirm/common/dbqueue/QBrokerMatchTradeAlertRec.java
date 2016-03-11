package aff.confirm.common.dbqueue;


/**
 * User: ifrankel
 * Date: Feb 19, 2009
 * Time: 7:02:49 AM
 */
public class QBrokerMatchTradeAlertRec {
    public double id;
    public String tradingSystem;
    public double tradeID;
    public String brokerSn;
    public int version;
    public String processedFlag;

    public QBrokerMatchTradeAlertRec(String pTradingSystem, double pTradeID, String pBrokerSn, int pVersion, String pProcesedFlag) {
        this.init();
        this.tradingSystem = pTradingSystem;
        this.tradeID = pTradeID;
        this.brokerSn = pBrokerSn;
        this.version = pVersion;
        this.processedFlag = pProcesedFlag;
    }

    public QBrokerMatchTradeAlertRec() {
        this.init();
    }

    public void init(){
        this.id = 0;
        this.tradingSystem = "";
        this.tradeID = 0;
        this.brokerSn = "";
        this.version = 0;
        this.processedFlag = "";
    }
}