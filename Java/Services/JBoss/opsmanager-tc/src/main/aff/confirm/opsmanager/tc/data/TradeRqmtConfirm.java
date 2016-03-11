package aff.confirm.opsmanager.tc.data;

import java.io.Serializable;
import java.util.Date;

import javax.xml.bind.annotation.XmlElement;

public class TradeRqmtConfirm implements Serializable {

private static final long serialVersionUID = 1L;
	
	private long _id ;
    private long _rqmtId ;
    private long _tradeId ;
    private long _templateId ;
    private String _confirmLabel;
    private String _confirmCmt ;
    private String _faxTelexInd ;
    private String _faxTelexNumber ;
    private String _xmitStatusInd ;
    private String _xmitAddr ;
    private String _xmitCmt ;
    private Date _xmitTimeStampGmt ;
    private String _templateName ;
    private String _templateCategory ;
    private String _templateTypeInd ;
    private String _finalApprovalFlag ;
    private String _nextStatusCode ;
	private String _activeFlag ;
    
    
    @XmlElement(name="Id")
	public long get_id() {
		return _id;
	}
	public void set_id(long _id) {
		this._id = _id;
	}
	@XmlElement(name="RqmtId")
	public long get_rqmtId() {
		return _rqmtId;
	}
	public void set_rqmtId(long id) {
		_rqmtId = id;
	}
	@XmlElement(name="TradeId")
	public long get_tradeId() {
		return _tradeId;
	}
	public void set_tradeId(long id) {
		_tradeId = id;
	}
	@XmlElement(name="TemplateId")
	public long get_templateId() {
		return _templateId;
	}
	public void set_templateId(long id) {
		_templateId = id;
	}
	@XmlElement(name="ConfirmLabel")
	public String get_confirmLabel() {
		return _confirmLabel;
	}
	public void set_confirmLabel(String label) {
		_confirmLabel = label;
	}
	@XmlElement(name="ConfirmCmt")
	public String get_confirmCmt() {
		return _confirmCmt;
	}
	public void set_confirmCmt(String cmt) {
		_confirmCmt = cmt;
	}
	@XmlElement(name="FaxTextInd")
	public String get_faxTelexInd() {
		return _faxTelexInd;
	}
	public void set_faxTelexInd(String telexInd) {
		_faxTelexInd = telexInd;
	}
	
	@XmlElement(name="FaxTelexNumber")
	public String get_faxTelexNumber() {
		return _faxTelexNumber;
	}
	public void set_faxTelexNumber(String telexNumber) {
		_faxTelexNumber = telexNumber;
	}
	
	@XmlElement(name="XmitStatusInd")
	public String get_xmitStatusInd() {
		return _xmitStatusInd;
	}
	public void set_xmitStatusInd(String statusInd) {
		_xmitStatusInd = statusInd;
	}
	
	@XmlElement(name="XmitAddr")
	public String get_xmitAddr() {
		return _xmitAddr;
	}
	public void set_xmitAddr(String addr) {
		_xmitAddr = addr;
	}
	
	@XmlElement(name="XmitCmt")
	public String get_xmitCmt() {
		return _xmitCmt;
	}
	public void set_xmitCmt(String cmt) {
		_xmitCmt = cmt;
	}
	
	@XmlElement(name="XmitTimeStampGmt")
	public Date get_xmitTimeStampGmt() {
		return _xmitTimeStampGmt;
	}
	public void set_xmitTimeStampGmt(Date timeStampGmt) {
		_xmitTimeStampGmt = timeStampGmt;
	}
	
	@XmlElement(name="TemplateName")
	public String get_templateName() {
		return _templateName;
	}
	public void set_templateName(String name) {
		_templateName = name;
	}
	
	@XmlElement(name="TemplateCategory")
	public String get_templateCategory() {
		return _templateCategory;
	}
	public void set_templateCategory(String category) {
		_templateCategory = category;
	}
	
	@XmlElement(name="TemplateTypeInd")
	public String get_templateTypeInd() {
		return _templateTypeInd;
	}
	public void set_templateTypeInd(String typeInd) {
		_templateTypeInd = typeInd;
	}
	
	@XmlElement(name="FinalApprovalFlag")
	public String get_finalApprovalFlag() {
		return _finalApprovalFlag;
	}
	public void set_finalApprovalFlag(String approvalFlag) {
		_finalApprovalFlag = approvalFlag;
	}

	@XmlElement(name="NextStatusCode")
	public String get_nextStatusCode() {
		return _nextStatusCode;
	}
	public void set_nextStatusCode(String statusCode) {
		_nextStatusCode = statusCode;
	}

	@XmlElement(name="ActiveFlag")
	public String get_activeFlag() {
		return _activeFlag;
	}
	public void set_activeFlag(String pActiveFlag) {
		_activeFlag = pActiveFlag;
	}

    
    
}
