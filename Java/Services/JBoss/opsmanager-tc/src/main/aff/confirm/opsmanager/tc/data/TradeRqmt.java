package aff.confirm.opsmanager.tc.data;

import java.io.Serializable;
import java.util.Date;

import javax.xml.bind.annotation.XmlElement;

public class TradeRqmt implements Serializable{
	
	private long _id;
	private long _tradeId;
	private long _rqmtTradeNotifyId;
	private String _rqmt;
	private String _status;
	private Date _completedDt;
	private Date _completedTimestampGmt;
	private String _reference;
	private long _cancelTradeNotifyId;
	private String _cmt;
	private String _secondCheckFlag;
	private long _transactionSeq;
	private String _displayText;
	private String _category;
	private String _terminalFlag;
	private String _problemFlag;
	private String _guiColorCode;
	private String _delphiConstant;
	private String _prelimAppr;
	private String _finalApprovalFlag;
	
	@XmlElement(name="Id")
	public long get_id() {
		return _id;
	}
	public void set_id(long rqmtId) {
		this._id = rqmtId;
	}
	@XmlElement(name="TradeId")
	public long get_tradeId() {
		return _tradeId;
	}
	public void set_tradeId(long tradeId) {
		this._tradeId = tradeId;
	}
	@XmlElement(name="RqmtTradeNotifyId")
	public long get_rqmtTradeNotifyId() {
		return _rqmtTradeNotifyId;
	}
	public void set_rqmtTradeNotifyId(long rqmtNotifyId) {
		this._rqmtTradeNotifyId = rqmtNotifyId;
	}
	@XmlElement(name="Rqmt")
	public String get_rqmt() {
		return _rqmt;
	}
	public void set_rqmt(String rqmt) {
		this._rqmt = rqmt;
	}
	@XmlElement(name="Status")
	public String get_status() {
		return _status;
	}
	public void set_status(String status) {
		this._status = status;
	}
	@XmlElement(name="CompletedDt")
	public Date get_completedDt() {
		return _completedDt;
	}
	public void set_completedDt(Date completedDate) {
		this._completedDt = completedDate;
	}
	@XmlElement(name="CompletedTimestampGmt")
	public Date get_completedTimestampGmt() {
		return _completedTimestampGmt;
	}
	public void set_completedTimestampGmt(Date completedDateTime) {
		this._completedTimestampGmt = completedDateTime;
	}
	@XmlElement(name="Reference")
	public String get_reference() {
		return _reference;
	}
	public void set_reference(String reference) {
		this._reference = reference;
	}
	
	public long get_cancelTradeNotifyId() {
		return _cancelTradeNotifyId;
	}
	@XmlElement(name="CancelTradeNotifyId")
	public void set_cancelTradeNotifyId(long cancelNotifyId) {
		this._cancelTradeNotifyId = cancelNotifyId;
	}
	public String get_cmt() {
		return _cmt;
	}
	@XmlElement(name="Cmt")
	public void set_cmt(String comment) {
		this._cmt = comment;
	}
	public String get_secondCheckFlag() {
		return _secondCheckFlag;
	}
	@XmlElement(name="SecondCheckFlag")
	public void set_secondCheckFlag(String secondChkFlag) {
		this._secondCheckFlag = secondChkFlag;
	}
	public long get_transactionSeq() {
		return _transactionSeq;
	}
	@XmlElement(name="TransactionSeq")
	public void set_transactionSeq(long transSeq) {
		this._transactionSeq = transSeq;
	}
	public String get_displayText() {
		return _displayText;
	}
	@XmlElement(name="DisplayText")
	public void set_displayText(String displayText) {
		this._displayText = displayText;
	}
	public String get_category() {
		return _category;
	}
	@XmlElement(name="Category")
	public void set_category(String category) {
		this._category = category;
	}
	public String get_terminalFlag() {
		return _terminalFlag;
	}
	@XmlElement(name="TerminalFlag")
	public void set_terminalFlag(String termFalg) {
		this._terminalFlag = termFalg;
	}
	public String get_problemFlag() {
		return _problemFlag;
	}
	@XmlElement(name="ProblemFlag")
	public void set_problemFlag(String probFlag) {
		this._problemFlag = probFlag;
	}
	public String get_guiColorCode() {
		return _guiColorCode;
	}
	@XmlElement(name="GuiColorCode")
	public void set_guiColorCode(String color) {
		this._guiColorCode = color;
	}
	public String get_delphiConstant() {
		return _delphiConstant;
	}
	@XmlElement(name="DelphiConstant")
	public void set_delphiConstant(String delphCons) {
		this._delphiConstant = delphCons;
	}
	public String get_prelimAppr() {
		return _prelimAppr;
	}
	@XmlElement(name="PrelimAppr")
	public void set_prelimAppr(String premAppr) {
		this._prelimAppr = premAppr;
	}
	
	public String get_finalApprovalFlag() {
		return _finalApprovalFlag;
	}
	@XmlElement(name="FinalApprovalFlag")
	public void set_finalApprovalFlag(String approvalFlag) {
		_finalApprovalFlag = approvalFlag;
	}
	
	

}
