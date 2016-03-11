package aff.confirm.opsmanager.opsmanagerweb.data;

import java.io.Serializable;
import java.util.Date;

public class TraderGetData implements Serializable{

	private long tradeId;
	private Date tradeDate;
	private String seCmpnySn;
	private String cptySn;
	private String cptyGroupCode;
	private String brokerSn;
	private String buySell;
	private String sttlType;
	private double totalQty;
	private long tradeRqmtId;
	private long tradeRqmtConfirmId;
	
	public long getTradeId() {
		return tradeId;
	}
	public void setTradeId(long tradeId) {
		this.tradeId = tradeId;
	}
	public Date getTradeDate() {
		return tradeDate;
	}
	public void setTradeDate(Date tradeDate) {
		this.tradeDate = tradeDate;
	}
	public String getSeCmpnySn() {
		return seCmpnySn;
	}
	public void setSeCmpnySn(String seCmpnySn) {
		this.seCmpnySn = seCmpnySn;
	}
	public String getCptySn() {
		return cptySn;
	}
	public void setCptySn(String cptySn) {
		this.cptySn = cptySn;
	}
	public String getCptyGroupCode() {
		return cptyGroupCode;
	}
	public void setCptyGroupCode(String cptyGroupCode) {
		this.cptyGroupCode = cptyGroupCode;
	}
	public String getBrokerSn() {
		return brokerSn;
	}
	public void setBrokerSn(String brokerSn) {
		this.brokerSn = brokerSn;
	}
	public String getBuySell() {
		return buySell;
	}
	public void setBuySell(String buySell) {
		this.buySell = buySell;
	}
	public String getSttlType() {
		return sttlType;
	}
	public void setSttlType(String sttlType) {
		this.sttlType = sttlType;
	}
	public double getTotalQty() {
		return totalQty;
	}
	public void setTotalQty(double totalQty) {
		this.totalQty = totalQty;
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
