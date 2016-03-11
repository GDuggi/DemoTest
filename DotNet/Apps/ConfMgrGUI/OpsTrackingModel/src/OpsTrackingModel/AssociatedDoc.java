package OpsTrackingModel;

//import com.gigaspaces.annotation.pojo.*;
import java.util.Date;
import java.io.Serializable;

//@SpaceClass
public class AssociatedDoc implements Serializable {
    /**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	/**
	 * 
	 */
	public static String ASSOCIATED = "ASSOCIATED";
    public static String UNASSOCIATED = "UNASSOCIATED";
    public static String RESERVED = "RESERVED";
    public static String FINALAPPROVED = "APPROVED";
    public static String PRELIM = "PRE-APPROVED";
    public static String DISCARDED = "DISCARDED";
    public static String OPEN = "OPEN";
    public static String CLOSED = "CLOSED";
    public static String DISPUTED = "DISPUTED";
    public static String VAULTED = "VAULTED";


    private long _id = -1;
    private long _inboundDocsId = -1;
    private long _indexVal = -1;
    private long _tradeRqmtId = -1;
    private long _tradeId = -1;
    private String _fileName;
    private String _docStatusCode;
    private String _associatedBy;
    private Date _associatedDt;
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
    private boolean multipleAssociatedDocs = false;
    private String _tradeFinalApprovalFlag;
    private String _xmitStatusCode;
    private String _xmitValue;
    private String _sentTo;

//	@SpaceId    //	Sets this field (ID) to be used in the construction
//    @SpaceRouting
//    @SpaceProperty(nullValue="-1")
    public long get_id() {
        return _id;
    }

    public void set_id(long _id) {
        this._id = _id;
    }
    
//    @SpaceProperty(index = SpaceProperty.IndexType.BASIC,nullValue="-1")
    public long get_inboundDocsId() {
        return _inboundDocsId;
    }

    public void set_inboundDocsId(long _inboundDocsId) {
        this._inboundDocsId = _inboundDocsId;
    }
//    @SpaceProperty(nullValue="-1")
    public long get_indexVal() {
        return _indexVal;
    }

    public void set_indexVal(long _indexVal) {
        this._indexVal = _indexVal;
    }

//    @SpaceProperty(index = SpaceProperty.IndexType.BASIC,nullValue="-1")
    public long get_tradeRqmtId() {
        return _tradeRqmtId;
    }

    public void set_tradeRqmtId(long _tradeRqmtId) {
        this._tradeRqmtId = _tradeRqmtId;
    }

//    @SpaceProperty(index = SpaceProperty.IndexType.BASIC,nullValue="-1")
    public long get_tradeId() {
        return _tradeId;
    }

    public void set_tradeId(long _tradeId) {
        this._tradeId = _tradeId;
    }

    public String get_fileName() {
        return _fileName;
    }

    public void set_fileName(String _fileName) {
        this._fileName = _fileName;
    }

    public String get_docStatusCode() {
        return _docStatusCode;
    }

    public void set_docStatusCode(String _docStatusCode) {
        this._docStatusCode = _docStatusCode;
    }

    public String get_associatedBy() {
        return _associatedBy;
    }

    public void set_associatedBy(String _associatedBy) {
        this._associatedBy = _associatedBy;
    }

    public Date get_associatedDt() {
        return _associatedDt;
    }

    public void set_associatedDt(Date _associatedDt) {
        this._associatedDt = _associatedDt;
    }

    public String get_finalApprovedBy() {
        return _finalApprovedBy;
    }

    public void set_finalApprovedBy(String _finalApprovedBy) {
        this._finalApprovedBy = _finalApprovedBy;
    }

    public Date get_finalApprovedDt() {
        return _finalApprovedDt;
    }

    public void set_finalApprovedDt(Date _finalApprovedDt) {
        this._finalApprovedDt = _finalApprovedDt;
    }

    public String get_disputedBy() {
        return _disputedBy;
    }

    public void set_disputedBy(String _disputedBy) {
        this._disputedBy = _disputedBy;
    }

    public Date get_disputedDt() {
        return _disputedDt;
    }

    public void set_disputedDt(Date _disputedDt) {
        this._disputedDt = _disputedDt;
    }

    public String get_discardedBy() {
        return _discardedBy;
    }

    public void set_discardedBy(String _discardedBy) {
        this._discardedBy = _discardedBy;
    }

    public Date get_discardedDt() {
        return _discardedDt;
    }

    public void set_discardedDt(Date _discardedDt) {
        this._discardedDt = _discardedDt;
    }

    public String get_vaultedBy() {
        return _vaultedBy;
    }

    public void set_vaultedBy(String _vaultedBy) {
        this._vaultedBy = _vaultedBy;
    }

    public Date get_vaultedDt() {
        return _vaultedDt;
    }

    public void set_vaultedDt(Date _vaultedDt) {
        this._vaultedDt = _vaultedDt;
    }

    public String get_cdtyGroupCode() {
        return _cdtyGroupCode;
    }

    public void set_cdtyGroupCode(String _cdtyGroupCode) {
        this._cdtyGroupCode = _cdtyGroupCode;
    }

    public String get_cptyShortName() {
        return _cptyShortName;
    }

    public void set_cptyShortName(String _cptyShortName) {
        this._cptyShortName = _cptyShortName;
    }

    public String get_brokerShortName() {
        return _brokerShortName;
    }

    public void set_brokerShortName(String _brokerShortName) {
        this._brokerShortName = _brokerShortName;
    }

    public String get_docTypeCode() {
        return _docTypeCode;
    }

    public void set_docTypeCode(String _docTypeCode) {
        this._docTypeCode = _docTypeCode;
    }

    public String get_secondValidateReqFlag() {
        return _secondValidateReqFlag;
    }

    public void set_secondValidateReqFlag(String _secondValidateReqFlag) {
        this._secondValidateReqFlag = _secondValidateReqFlag;
    }

 //   @SpaceExclude
    public boolean isMultipleAssociatedDocs() {
        return multipleAssociatedDocs;
    }

    public void setMultipleAssociatedDocs(boolean multipleAssociatedDocs) {
        this.multipleAssociatedDocs = multipleAssociatedDocs;
    }
    
    public String get_tradeFinalApprovalFlag() {
		return _tradeFinalApprovalFlag;
	}

	public void set_tradeFinalApprovalFlag(String _tradeFinalApprovalFlag) {
		this._tradeFinalApprovalFlag = _tradeFinalApprovalFlag;
	}

	public String get_xmitStatusCode() {
		return _xmitStatusCode;
	}

	public void set_xmitStatusCode(String xmitstatusCode) {
		_xmitStatusCode = xmitstatusCode;
	}
	
	public String get_xmitValue() {
		return _xmitValue;
	}

	public void set_xmitValue(String value) {
		_xmitValue = value;
	}
	
	public String get_sentTo() {
		return _sentTo;
	}

	public void set_sentTo(String to) {
		_sentTo = to;
	}

    @Override
    public String toString(){
    	return _id+"|"+_inboundDocsId+"|"+_indexVal+"|"+_tradeRqmtId+"|"+_tradeId+"|"+_fileName+"|"+_docStatusCode+"|"+_associatedBy+"|"+_associatedDt+"|"+_finalApprovedBy+"|"+_finalApprovedDt+"|"+_disputedBy+"|"+_disputedDt+"|"+_discardedBy+"|"+_discardedDt+"|"+_vaultedBy+"|"+_vaultedDt+"|"+_cdtyGroupCode+"|"+_cptyShortName+"|"+_brokerShortName+"|"+_docTypeCode+"|"+_secondValidateReqFlag+"|"+multipleAssociatedDocs+"|"+_tradeFinalApprovalFlag+"|"+_xmitStatusCode+"|"+_xmitValue+"|"+_sentTo;
    }
}
