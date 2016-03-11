package aff.confirm.opsmanager.confirm.data;

import java.io.Serializable;

public class DocVaultRequest implements Serializable{

	private long tradeId;
	private long rqmtId;
	private long rqmtConfirmId;
	private String tradeDt;
	private String cptySn;
	private String seCptySn;
	private String cdtyCode;
	private String cdtyGrpCode;
	
	private byte[] documentData;
	
	
	public long getTradeId() {
		return tradeId;
	}
	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	public long getRqmtId() {
		return rqmtId;
	}
	public void setRqmtId(long rqmtId) {
		this.rqmtId = rqmtId;
	}
	public long getRqmtConfirmId() {
		return rqmtConfirmId;
	}
	public void setRqmtConfirmId(long rqmtConfirmId) {
		this.rqmtConfirmId = rqmtConfirmId;
	}
	public String getTradeDt() {
		return tradeDt;
	}
	public void setTradeDt(String tradeDt) {
		this.tradeDt = tradeDt;
	}
	public String getCptySn() {
		return cptySn;
	}
	public void setCptySn(String cptySn) {
		this.cptySn = cptySn;
	}
	public String getSeCptySn() {
		return seCptySn;
	}
	public void setSeCptySn(String seCptySn) {
		this.seCptySn = seCptySn;
	}
	public String getCdtyCode() {
		return cdtyCode;
	}
	public void setCdtyCode(String cdtyCode) {
		this.cdtyCode = cdtyCode;
	}
	public String getCdtyGrpCode() {
		return cdtyGrpCode;
	}
	public void setCdtyGrpCode(String cdtyGrpCode) {
		this.cdtyGrpCode = cdtyGrpCode;
	}
	public byte[] getDocumentData() {
		return documentData;
	}
	public void setDocumentData(byte[] documentData) {
		this.documentData = documentData;
	}
	
}
