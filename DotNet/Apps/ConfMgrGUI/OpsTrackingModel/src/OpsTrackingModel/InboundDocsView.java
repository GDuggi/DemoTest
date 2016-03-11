package OpsTrackingModel;

import java.io.Serializable;
import java.util.Date;

//import com.gigaspaces.annotation.pojo.SpaceClass;
//import com.gigaspaces.annotation.pojo.SpaceId;
//import com.gigaspaces.annotation.pojo.SpaceProperty;
//import com.gigaspaces.annotation.pojo.SpaceRouting;

//@SpaceClass
public class InboundDocsView implements Serializable{

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
    private long _id = -1;
    private long _unresolvedCount = -1;
    private String _callerRef;
    private String _sentTo;
    private Date _rcvdTs = null;
    private String _fileName;
    private String _sender;
    private String _cmt;
    private String _docStatusCode;
    private String _hasAutoAsctedFlag;
    private String _tradeIds;
    private String _bookmarkFlag;
    private String _bookmarkUser;
    private String _ignoreFlag;
    private String _ignoredUser;
    private String _commentFlag;
    private String _commentUser;
    private String _userComments;
    private String _mappedCptySn;
    private String _procFlag;
	
//	@SpaceId    //	Sets this field (ID) to be used in the construction
  //  @SpaceRouting
 //   @SpaceProperty(nullValue="-1")
    public long get_id() {
 		return _id;
 	}
 	
    public void set_id(long _id) {
 		this._id = _id;
 	}
  //  @SpaceProperty(nullValue="-1")
 	public long get_unresolvedCount() {
 		return _unresolvedCount;
 	}

    public void set_unresolvedCount(long count) {
 		_unresolvedCount = count;
 	}
 	
 	public String get_callerRef() {
 		return _callerRef;
 	}
 	
 	public void set_callerRef(String ref) {
 		_callerRef = ref;
 	}
 	
 	public String get_sentTo() {
 		return _sentTo;
 	}
 	
 	public void set_sentTo(String to) {
 		_sentTo = to;
 	}
 	
 	public Date get_rcvdTs() {
 		return _rcvdTs;
 	}
 	
 	public void set_rcvdTs(Date ts) {
 		_rcvdTs = ts;
 	}
 	
 	public String get_fileName() {
 		return _fileName;
 	}
 	
 	public void set_fileName(String name) {
 		_fileName = name;
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
 	
 	public void set_docStatusCode(String statusCode) {
 		_docStatusCode = statusCode;
 	}
 	
 	public String get_hasAutoAsctedFlag() {
 		return _hasAutoAsctedFlag;
 	}
 	
 	public void set_hasAutoAsctedFlag(String autoAsctedFlag) {
 		_hasAutoAsctedFlag = autoAsctedFlag;
 	}
 	
 	public String get_tradeIds() {
 		return _tradeIds;
 	}
 	
 	public void set_tradeIds(String ids) {
 		_tradeIds = ids;
 	}
 	
 	public String get_bookmarkFlag() {
 		return _bookmarkFlag;
 	}
 	
 	public void set_bookmarkFlag(String flag) {
 		_bookmarkFlag = flag;
 	}
 	
 	public String get_bookmarkUser() {
 		return _bookmarkUser;
 	}
 	
 	public void set_bookmarkUser(String user) {
 		_bookmarkUser = user;
 	}
 	
 	public String get_ignoreFlag() {
 		return _ignoreFlag;
 	}
 	
 	public void set_ignoreFlag(String flag) {
 		_ignoreFlag = flag;
 	}
 	
 	public String get_ignoredUser() {
 		return _ignoredUser;
 	}
 	
 	public void set_ignoredUser(String user) {
 		_ignoredUser = user;
 	}
 	
 	public String get_commentFlag() {
 		return _commentFlag;
 	}
 	
 	public void set_commentFlag(String flag) {
 		_commentFlag = flag;
 	}
 	
 	public String get_commentUser() {
 		return _commentUser;
 	}
 	
 	public void set_commentUser(String user) {
 		_commentUser = user;
 	}
 	
 	public String get_userComments() {
 		return _userComments;
 	}
 	
 	public void set_userComments(String comments) {
 		_userComments = comments;
 	}
 	
 	public String get_mappedCptySn() {
 		return _mappedCptySn;
 	}
 	
 	public void set_mappedCptySn(String cptySn) {
 		_mappedCptySn = cptySn;
 	}
 	
 	public String get_procFlag() {
		return _procFlag;
	}

	public void set_procFlag(String procflag) {
		_procFlag = procflag;
	}
	
	@Override
 	public String toString(){
 		return _id+"|"+_unresolvedCount+"|"+_callerRef+"|"+_sentTo+"|"+_rcvdTs+"|"+_fileName+"|"+_sender+"|"+_cmt+"|"+_docStatusCode+"|"+_hasAutoAsctedFlag+"|"+_tradeIds+"|"+_bookmarkFlag+"|"+_bookmarkUser+"|"+_ignoreFlag+"|"+_ignoredUser+"|"+_commentFlag+"|"+_commentUser+"|"+_userComments+"|"+_mappedCptySn+"|"+_procFlag;
 	}
 	
}
