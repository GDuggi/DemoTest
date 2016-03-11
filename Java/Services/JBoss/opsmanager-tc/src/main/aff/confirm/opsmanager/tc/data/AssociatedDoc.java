package aff.confirm.opsmanager.tc.data;

import java.io.Serializable;
import java.util.Date;

import javax.xml.bind.annotation.XmlElement;

public class AssociatedDoc implements Serializable {
	
	private long _id;
	private long _inboundDocsId;
	private int _indexVal;
	private String _fileName;
	private long _tradeId;
	private String _docStatusCode;
	private Date _associatedDt;
	private String _associatedBy;
	private String _finalApprovedBy;
	private Date _finalApprovedDt;
	private String _disputedBy;
	private Date _disputedDt;
	private String _discardedBy;
	private Date _discardedDt;
	private String _vaultedBy;
	private Date _vaultedDt;
	private String _cdtyGroupCode;
	private String _cptyShortName;
	private String _brokerShortName;
	private String _docTypeCode;
	private String _secondValidateReqFlag;
	private long _tradeRqmtId;
	private String _xmitStatusCode;
	private String _xmitValue;
	private String _sentTo;
	
	
	
	public long get_id() {
		return _id;
	}
	@XmlElement(name="Id")
	public void set_id(long _id) {
		this._id = _id;
	}
	
	public long get_inboundDocsId() {
		return _inboundDocsId;
	}
	@XmlElement(name="InboundDocsId")
	public void set_inboundDocsId(long docsId) {
		_inboundDocsId = docsId;
	}
	
	public int get_indexVal() {
		return _indexVal;
	}
	@XmlElement(name="IndexVal")
	public void set_indexVal(int val) {
		_indexVal = val;
	}
	public String get_fileName() {
		return _fileName;
	}
	@XmlElement(name="FileName")
	public void set_fileName(String name) {
		_fileName = name;
	}
	
	public long get_tradeId() {
		return _tradeId;
	}
	@XmlElement(name="TradeId")
	public void set_tradeId(long id) {
		_tradeId = id;
	}
	
	public String get_docStatusCode() {
		return _docStatusCode;
	}
	@XmlElement(name="DocStatusCode")
	public void set_docStatusCode(String statusCode) {
		_docStatusCode = statusCode;
	}
	
	public Date get_associatedDt() {
		return _associatedDt;
	}
	@XmlElement(name="AssociatedDt")
	public void set_associatedDt(Date dt) {
		_associatedDt = dt;
	}
	
	public String get_associatedBy() {
		return _associatedBy;
	}
	@XmlElement(name="AssociatedBy")
	public void set_associatedBy(String by) {
		_associatedBy = by;
	}
	
	public String get_finalApprovedBy() {
		return _finalApprovedBy;
	}
	@XmlElement(name="FinalApprovedBy")
	public void set_finalApprovedBy(String approvedBy) {
		_finalApprovedBy = approvedBy;
	}
	
	public Date get_finalApprovedDt() {
		return _finalApprovedDt;
	}
	@XmlElement(name="FinalApprovedDt")
	public void set_finalApprovedDt(Date approvedDt) {
		_finalApprovedDt = approvedDt;
	}
	
	public String get_disputedBy() {
		return _disputedBy;
	}
	@XmlElement(name="DisputedBy")
	public void set_disputedBy(String by) {
		_disputedBy = by;
	}
	
	public Date get_disputedDt() {
		return _disputedDt;
	}
	@XmlElement(name="DisputedDt")
	public void set_disputedDt(Date dt) {
		_disputedDt = dt;
	}
	
	public String get_discardedBy() {
		return _discardedBy;
	}
	@XmlElement(name="DiscardedBy")
	public void set_discardedBy(String by) {
		_discardedBy = by;
	}
	
	public Date get_discardedDt() {
		return _discardedDt;
	}
	@XmlElement(name="DiscardedDt")
	public void set_discardedDt(Date dt) {
		_discardedDt = dt;
	}
	
	public String get_vaultedBy() {
		return _vaultedBy;
	}
	@XmlElement(name="VaultedBy")
	public void set_vaultedBy(String by) {
		_vaultedBy = by;
	}
	
	public Date get_vaultedDt() {
		return _vaultedDt;
	}
	@XmlElement(name="VaultedDt")
	public void set_vaultedDt(Date dt) {
		_vaultedDt = dt;
	}
	
	public String get_cdtyGroupCode() {
		return _cdtyGroupCode;
	}
	@XmlElement(name="CdtyGroupCode")
	public void set_cdtyGroupCode(String groupCode) {
		_cdtyGroupCode = groupCode;
	}
	
	public String get_cptyShortName() {
		return _cptyShortName;
	}
	@XmlElement(name="CptyShortName")
	public void set_cptyShortName(String shortName) {
		_cptyShortName = shortName;
	}
	
	public String get_brokerShortName() {
		return _brokerShortName;
	}
	@XmlElement(name="BrokerShortName")
	public void set_brokerShortName(String shortName) {
		_brokerShortName = shortName;
	}
	
	public String get_docTypeCode() {
		return _docTypeCode;
	}
	@XmlElement(name="DocTypeCode")
	public void set_docTypeCode(String typeCode) {
		_docTypeCode = typeCode;
	}
	
	public String get_secondValidateReqFlag() {
		return _secondValidateReqFlag;
	}
	@XmlElement(name="SecondValidateReqFlag")
	public void set_secondValidateReqFlag(String validateReqFlag) {
		_secondValidateReqFlag = validateReqFlag;
	}
	
	public long get_tradeRqmtId() {
		return _tradeRqmtId;
	}
	@XmlElement(name="TradeRqmtId")
	public void set_tradeRqmtId(long rqmtId) {
		_tradeRqmtId = rqmtId;
	}
	
	public String get_xmitStatusCode() {
		return _xmitStatusCode;
	}
	@XmlElement(name="XmitStatusCode")
	public void set_xmitStatusCode(String statusCode) {
		_xmitStatusCode = statusCode;
	}
	
	public String get_xmitValue() {
		return _xmitValue;
	}
	@XmlElement(name="XmitValue")
	public void set_xmitValue(String value) {
		_xmitValue = value;
	}
	
	public String get_sentTo() {
		return _sentTo;
	}
	@XmlElement(name="SentTo")
	public void set_sentTo(String to) {
		_sentTo = to;
	}

	
	

}
