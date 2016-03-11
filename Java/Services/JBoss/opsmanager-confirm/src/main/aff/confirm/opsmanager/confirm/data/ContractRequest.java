package aff.confirm.opsmanager.confirm.data;

import java.io.Serializable;

public class ContractRequest implements Serializable {

	private String tradeSysCode;
	private long tradeId;
	private long templateId;
	private String cptySn;
	private String sempraCptySn;
	private String dateSent;
	private String signedFlag;
	private String contract;
	private String tradeDate;
	private long tradeRqmtConfirmId;
	private String cdtyCode;
	private String cdtyGroupCode;
	private long rqmtId;
	private String settlementType;
	private long prmntConfirmId;
	
	// for old confirmation compatible fields
	private long blotAsciId;
	private String blotterNo;
	private String confirmStatusInd;
	private String action;
	private long cptyProdAreaId;
	private int tradeVersion;
	private String noConfirmReason;
	private long seCompanyId;
	private String prodAreaCode;
	
	
	
	public String getTradeSysCode() {
		return tradeSysCode;
	}
	public void setTradeSysCode(String tradeSysCode) {
		this.tradeSysCode = tradeSysCode;
	}
	public long getTradeId() {
		return tradeId;
	}
	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	public long getTemplateId() {
		return templateId;
	}
	public void setTemplateId(long contractId) {
		this.templateId = contractId;
	}
	public String getCptySn() {
		return cptySn;
	}
	public void setCptySn(String cptySn) {
		this.cptySn = cptySn;
	}
	public String getSempraCptySn() {
		return sempraCptySn;
	}
	public void setSempraCptySn(String sempraCptySn) {
		this.sempraCptySn = sempraCptySn;
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
	public String getContract() {
		return contract;
	}
	public void setContract(String contract) {
		this.contract = contract;
	}
	public long getBlotAsciId() {
		return blotAsciId;
	}
	public void setBlotAsciId(long blotAsciId) {
		this.blotAsciId = blotAsciId;
	}
	public String getBlotterNo() {
		return blotterNo;
	}
	public void setBlotterNo(String blotterNo) {
		this.blotterNo = blotterNo;
	}
	public String getConfirmStatusInd() {
		return confirmStatusInd;
	}
	public void setConfirmStatusInd(String confirmStatusInd) {
		this.confirmStatusInd = confirmStatusInd;
	}
	public String getAction() {
		return action;
	}
	public void setAction(String action) {
		this.action = action;
	}
	public long getCptyProdAreaId() {
		return cptyProdAreaId;
	}
	public void setCptyProdAreaId(long cptyProdAreaId) {
		this.cptyProdAreaId = cptyProdAreaId;
	}
	public int getTradeVersion() {
		return tradeVersion;
	}
	public void setTradeVersion(int tradeVersion) {
		this.tradeVersion = tradeVersion;
	}
	public String getNoConfirmReason() {
		return noConfirmReason;
	}
	public void setNoConfirmReason(String noConfirmReason) {
		this.noConfirmReason = noConfirmReason;
	}
	public long getSeCompanyId() {
		return seCompanyId;
	}
	public void setSeCompanyId(long seCompanyId) {
		this.seCompanyId = seCompanyId;
	}
	public String getProdAreaCode() {
		return prodAreaCode;
	}
	public void setProdAreaCode(String prodAreaCode) {
		this.prodAreaCode = prodAreaCode;
	}
	public String getTradeDate() {
		return tradeDate;
	}
	public void setTradeDate(String tradeDate) {
		this.tradeDate = tradeDate;
	}
	public long getTradeRqmtConfirmId() {
		return tradeRqmtConfirmId;
	}
	public void setTradeRqmtConfirmId(long tradeRqmtConfirmId) {
		this.tradeRqmtConfirmId = tradeRqmtConfirmId;
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
	public long getRqmtId() {
		return rqmtId;
	}
	public void setRqmtId(long rqmtId) {
		this.rqmtId = rqmtId;
	}
	public String getSettlementType() {
		return settlementType;
	}
	public void setSettlementType(String settlementType) {
		this.settlementType = settlementType;
	}
	public long getPrmntConfirmId() {
		return prmntConfirmId;
	}
	public void setPrmntConfirmId(long prmntConfirmId) {
		this.prmntConfirmId = prmntConfirmId;
	}
	
	
}
