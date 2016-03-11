package aff.confirm.opsmanager.creditmargin.common;

/**
 * User: srajaman
 * Date: Feb 3, 2009
 * Time: 1:54:47 PM
 */
public class CreditLogRec {

    private long tradeId;
    private String msg;
    private String processFlag;
    private String cmt;

    public long getTradeId() {
        return tradeId;
    }

    public void setTradeId(long tradeId) {
        this.tradeId = tradeId;
    }

    public String getMsg() {
        return msg;
    }

    public void setMsg(String msg) {
        this.msg = msg;
    }

    public String getProcessFlag() {
        return processFlag;
    }

    public void setProcessFlag(String processFlag) {
        this.processFlag = processFlag;
    }

    public String getCmt() {
        return cmt;
    }

    public void setCmt(String cmt) {
        this.cmt = cmt;
    }
}
