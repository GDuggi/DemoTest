package aff.confirm.opsmanager.creditmargin.common;

import java.io.Serializable;

/**
 * User: srajaman
 * Date: Dec 5, 2008
 * Time: 3:07:41 PM
 */
public class TradeRqmtRec implements Serializable {

    private long tradeId;
    private long tradeRqmtId;
    private String tradesystem;
    private long tradeRqmtConfirmId;
    private String cptySn;
    private long contractId;
    private String seCptySn;
    private String dateSent;
    private String signedFlag = "N";
    private String tradeDate;
    private String cdtyCode;
    private String cdtyGroupCode;
    private String sttlType;
    private String nextStatusCode;
    private long templateId;

    public String getCurrentStatus() {
        return currentStatus;
    }

    public void setCurrentStatus(String currentStatus) {
        this.currentStatus = currentStatus;
    }

    private String currentStatus;


    public long getTemplateId() {
        return templateId;
    }

    public void setTemplateId(long templateId) {
        this.templateId = templateId;
    }

    public String getNextStatusCode() {
        return nextStatusCode;
    }

    public void setNextStatusCode(String nextStatusCode) {
        this.nextStatusCode = nextStatusCode;
    }

    public String getCptySn() {
        return cptySn;
    }

    public void setCptySn(String cptySn) {
        this.cptySn = cptySn;
    }

    public long getContractId() {
        return contractId;
    }

    public void setContractId(long contractId) {
        this.contractId = contractId;
    }

    public String getSeCptySn() {
        return seCptySn;
    }

    public void setSeCptySn(String seCptySn) {
        this.seCptySn = seCptySn;
    }

    public String getDateSent() {
        return dateSent;
    }

    public void setDateSent(String dateSent) {
        this.dateSent = dateSent;
    }

    public String getSignedFlag() {
        return signedFlag;
    }

    public void setSignedFlag(String signedFlag) {
        this.signedFlag = signedFlag;
    }

    public String getTradeDate() {
        return tradeDate;
    }

    public void setTradeDate(String tradeDate) {
        this.tradeDate = tradeDate;
    }

    public String getCdtyCode() {
        return cdtyCode;
    }

    public void setCdtyCode(String cdtyCode) {
        this.cdtyCode = cdtyCode;
    }

    public String getCdtyGroupCode() {
        return cdtyGroupCode;
    }

    public void setCdtyGroupCode(String cdtyGroupCode) {
        this.cdtyGroupCode = cdtyGroupCode;
    }

    public String getSttlType() {
        return sttlType;
    }

    public void setSttlType(String sttlType) {
        this.sttlType = sttlType;
    }

    public long getTradeId() {
        return tradeId;
    }

    public String getTradesystem() {
        return tradesystem;
    }

    public void setTradesystem(String tradesystem) {
        this.tradesystem = tradesystem;
    }

    public void setTradeId(long tradeId) {
        this.tradeId = tradeId;
    }

    public long getTradeRqmtId() {
        return tradeRqmtId;
    }

    public void setTradeRqmtId(long tradeRqmtId) {
        this.tradeRqmtId = tradeRqmtId;
    }

    public long getTradeRqmtConfirmId() {
        return tradeRqmtConfirmId;
    }

    public void setTradeRqmtConfirmId(long tradeRqmtConfirmId) {
        this.tradeRqmtConfirmId = tradeRqmtConfirmId;
    }



}
