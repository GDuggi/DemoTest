package OpsTrackingModel;

import java.io.Serializable;
import java.util.Date;

//import com.gigaspaces.annotation.pojo.SpaceId;
//import com.gigaspaces.annotation.pojo.SpaceProperty;
//import com.gigaspaces.annotation.pojo.SpaceRouting;

public class TradeRqmtConfirm implements Serializable{

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	
	private long _id = -1;
    private long _rqmtId = -1;
    private long _tradeId = -1;
    private long _templateId = -1;
    private String _confirmLabel = null;
    private String _confirmCmt = null;
    private String _faxTelexInd = null;
    private String _faxTelexNumber = null;
    private String _xmitStatusInd = null;
    private String _xmitAddr = null;
    private String _xmitCmt = null;
    private Date _xmitTimeStampGmt = null;
    private String _templateName = null;
    private String _templateCategory = null;
    private String _templateTypeInd = null;
    private String _finalApprovalFlag = null;
    private String _nextStatusCode = null;
	
//	@SpaceId    //	Sets this field (ID) to be used in the construction
 //   @SpaceRouting
 //   @SpaceProperty(nullValue="-1")
    public long get_id() {
		return _id;
	}
    
	public void set_id(long _id) {
		this._id = _id;
	}
	
//    @SpaceProperty(nullValue="-1")
    public long get_rqmtId() {
		return _rqmtId;
	}
	
	public void set_rqmtId(long id) {
		_rqmtId = id;
	}

//	@SpaceProperty(nullValue="-1")
	public long get_tradeId() {
		return _tradeId;
	}
	
	public void set_tradeId(long id) {
		_tradeId = id;
	}
	
 //   @SpaceProperty(nullValue="-1")
	public long get_templateId() {
		return _templateId;
	}
	
	public void set_templateId(long id) {
		_templateId = id;
	}
	
	public String get_confirmLabel() {
		return _confirmLabel;
	}
	
	public void set_confirmLabel(String label) {
		_confirmLabel = label;
	}
	
	public String get_confirmCmt() {
		return _confirmCmt;
	}
	
	public void set_confirmCmt(String cmt) {
		_confirmCmt = cmt;
	}
	
	public String get_faxTelexInd() {
		return _faxTelexInd;
	}
	
	public void set_faxTelexInd(String telexInd) {
		_faxTelexInd = telexInd;
	}
	
	public String get_faxTelexNumber() {
		return _faxTelexNumber;
	}
	
	public void set_faxTelexNumber(String telexNumber) {
		_faxTelexNumber = telexNumber;
	}
	
	public String get_xmitStatusInd() {
		return _xmitStatusInd;
	}
	
	public void set_xmitStatusInd(String statusInd) {
		_xmitStatusInd = statusInd;
	}
	
	public String get_xmitAddr() {
		return _xmitAddr;
	}
	
	public void set_xmitAddr(String addr) {
		_xmitAddr = addr;
	}
	
	public String get_xmitCmt() {
		return _xmitCmt;
	}
	
	public void set_xmitCmt(String cmt) {
		_xmitCmt = cmt;
	}
	
	public Date get_xmitTimeStampGmt() {
		return _xmitTimeStampGmt;
	}
	
	public void set_xmitTimeStampGmt(Date timeStampGmt) {
		_xmitTimeStampGmt = timeStampGmt;
	}
	
	public String get_templateName() {
		return _templateName;
	}
	
	public void set_templateName(String name) {
		_templateName = name;
	}
	
	public String get_templateCategory() {
		return _templateCategory;
	}
	
	public void set_templateCategory(String category) {
		_templateCategory = category;
	}
	
	public String get_templateTypeInd() {
		return _templateTypeInd;
	}
	
	public void set_templateTypeInd(String typeInd) {
		_templateTypeInd = typeInd;
	}
	
	public String get_finalApprovalFlag() {
		return _finalApprovalFlag;
	}
	
	public void set_finalApprovalFlag(String approvalFlag) {
		_finalApprovalFlag = approvalFlag;
	}
	
    public String get_nextStatusCode() {
		return _nextStatusCode;
	}

	public void set_nextStatusCode(String statusCode) {
		_nextStatusCode = statusCode;
	}

	
	@Override
	public String toString(){
    	return _id+"|"+_rqmtId+"|"+_tradeId+"|"+_templateId+"|"+_confirmLabel+"|"+_confirmCmt+"|"+_faxTelexInd+"|"+_faxTelexNumber+"|"+_xmitStatusInd+"|"+_xmitAddr+"|"+_xmitCmt+"|"+_xmitTimeStampGmt+"|"+_templateName+"|"+_templateCategory+"|"+_templateTypeInd+"|"+_finalApprovalFlag;
		
	}
}
