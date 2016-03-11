package aff.confirm.opsmanager.opssubpub.opstrackingmodel;

//import com.gigaspaces.annotation.pojo.SpaceClass;
//import com.gigaspaces.annotation.pojo.SpaceExclude;
//import com.gigaspaces.annotation.pojo.SpaceId;
//import com.gigaspaces.annotation.pojo.SpaceRouting;

import java.io.Serializable;
import java.util.Date;

//@SpaceClass
public class InboundDoc implements Serializable {
    /**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	public static String DISCARDED = "DISCARDED";
    public static String OPEN = "OPEN";
    public static String CLOSED = "CLOSED";

    private long _id;
    private String _callerRef;
    private String _sentTo;
    private Date _rcvdTS;
    private String _fileName;
    private String _sender;
    private String _cmt;
    private String _docStatusCode;
    private String _hasAutoAsctedFlag;
    private String _docUserName;
    private long _workingTradeID;


  //  @SpaceId    //	Sets this field (ID) to be used in the construction
 //   @SpaceRouting
    public long get_id() {
        return _id;
    }

    public void set_id(long _id) {
        this._id = _id;
    }

    public String get_callerRef() {
        return _callerRef;
    }

    public void set_callerRef(String _callerRef) {
        this._callerRef = _callerRef;
    }

    public String get_sentTo() {
        return _sentTo;
    }

    public void set_sentTo(String _sentTo) {
        this._sentTo = _sentTo;
    }

    public Date get_rcvdTS() {
        return _rcvdTS;
    }

    public void set_rcvdTS(Date _rcvdTS) {
        this._rcvdTS = _rcvdTS;
    }

    public String get_fileName() {
        return _fileName;
    }

    public void set_fileName(String _fileName) {
        this._fileName = _fileName;
    }

    public String get_sender() {
        return _sender;
    }

    public void set_sender(String _sender) {
        this._sender = _sender;
    }

    public String get_cmt() {
        return _cmt;
    }

    public void set_cmt(String _cmt) {
        this._cmt = _cmt;
    }

    public String get_docStatusCode() {
        return _docStatusCode;
    }

    public void set_docStatusCode(String _docStatusCode) {
        this._docStatusCode = _docStatusCode;
    }

    public String get_hasAutoAsctedFlag() {
        return _hasAutoAsctedFlag;
    }

    public void set_hasAutoAsctedFlag(String _hasAutoAsctedFlag) {
        this._hasAutoAsctedFlag = _hasAutoAsctedFlag;
    }

 //   @SpaceExclude
    public String get_docUserName() {
        return _docUserName;
    }

    public void set_docUserName(String _docUserName) {
        this._docUserName = _docUserName;
    }

//    @SpaceExclude
    public long get_workingTradeID() {
        return _workingTradeID;
    }

    public void set_workingTradeID(long _workingTradeID) {
        this._workingTradeID = _workingTradeID;
    }
}
