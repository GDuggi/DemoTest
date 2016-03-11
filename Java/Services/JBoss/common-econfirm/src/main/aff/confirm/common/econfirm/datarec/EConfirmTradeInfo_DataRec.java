package aff.confirm.common.econfirm.datarec;

/**
 * Created by ifrankel on 12/8/2014.
 */
public class EConfirmTradeInfo_DataRec {
        public String tradingSystemCode;
        public double tradeID;
        public int productID;
        public boolean isEConfirmBrokerDeal;
        public boolean isClickAndConfirmDeal;
        //SUBMIT, CANCEL, NONE, MATCHED
//    public String action;

        public EConfirmTradeInfo_DataRec() {
            this.init();
        }

/*
    public EConfirmTradeInfo_DataRec(String pTradingSystemCode, double pTradeId) {
        this.init();
        this.tradingSystemCode = pTradingSystemCode;
        this.tradeID = pTradeId;
    }
*/

        public void init(){
            this.tradeID = 0;
            this.productID = 0;
            this.isEConfirmBrokerDeal = false;
            this.isClickAndConfirmDeal = false;
            this.tradingSystemCode = "";

//        this.action = "NONE";
        }
}
