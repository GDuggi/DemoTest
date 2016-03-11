	package OpsTrackingModel;

//	import com.gigaspaces.annotation.pojo.SpaceClass;
	//import com.gigaspaces.annotation.pojo.SpaceId;
	//import com.gigaspaces.annotation.pojo.SpaceProperty;
//	import com.gigaspaces.annotation.pojo.SpaceRouting;
//	import com.j_spaces.core.IJSpace;
//	import com.j_spaces.core.client.SpaceFinder;
//	import com.j_spaces.core.client.FinderException;
	import java.util.Date;
	import java.io.Serializable;
	import java.rmi.RemoteException;
//	import net.jini.core.lease.Lease;
//	import net.jini.core.transaction.TransactionException;


//	@SpaceClass
	public class RqmtData implements Serializable {
	    /**
		 * 
		 */
		public static final int BKR_METHOD = 0;
		public static final int SETC_METHOD = 1;
		public static final int CPTY_METHOD = 2;
		public static final int NOCONF_METHOD = 3;
		public static final int VERBL_METHOD = 4;
		public static final int MISC_METHOD = 5;
		public static final int NA_METHOD = 6;
		
		private static final long serialVersionUID = 1L;
		private long _id = -1;
	    private long _tradeId = -1;
	    private long _rqmtTradeNotifyId = -1;
	    private String _rqmt = null;
	    private String _status = null;
	    private Date _completedTimestampGmt = null;
	    private Date _completedDt = null;
	    private String _reference = null;
	    private long _cancelTradeNotifyId = -1;
	    private String _cmt = null;
	    private String _secondCheckFlag = null;
	    private long _transactionSeq = -1;
	    private String _finalApprovalFlag = null;
	    private String _displayText = null;
	    private String _category = null;
	    private String _terminalFlag = null;
	    private String _problemFlag = null;
	    private String _guiColorCode = null;
	    private String _delphiConstant = null;
	    private String _prelimAppr = null;
	    
	//	@SpaceId		//	Sets this field (ID) to be used in the construction
	//    @SpaceRouting
	//    @SpaceProperty(nullValue="-1")
	    public long get_id() {
	        return _id;
	    }

	    public void set_id(long _id) {
	        this._id = _id;
	    }

	//    @SpaceProperty(index = SpaceProperty.IndexType.BASIC,nullValue="-1")
	    public long get_tradeId() {
	        return _tradeId;
	    }

	    public void set_tradeId(long _tradeId) {
	        this._tradeId = _tradeId;
	    }

	    public String get_rqmt() {
	        return _rqmt;
	    }

	    public void set_rqmt(String _rqmt) {
	        this._rqmt = _rqmt;
	    }

	//    @SpaceProperty(nullValue="-1")
	    public long get_rqmtTradeNotifyId() {
	        return _rqmtTradeNotifyId;
	    }

	    public void set_rqmtTradeNotifyId(long _rqmtTradeNotifyId) {
	        this._rqmtTradeNotifyId = _rqmtTradeNotifyId;
	    }

	    public String get_status() {
	        return _status;
	    }

	    public void set_status(String _status) {
	        this._status = _status;
	    }

	//    @SpaceProperty(nullValue="-1")
	    public long get_cancelTradeNotifyId() {
	        return _cancelTradeNotifyId;
	    }

	    public void set_cancelTradeNotifyId(long _cancelTradeNotifyId) {
	        this._cancelTradeNotifyId = _cancelTradeNotifyId;
	    }

	    public String get_reference() {
	        return _reference;
	    }

	    public void set_reference(String _reference) {
	        this._reference = _reference;
	    }

	    public String get_cmt() {
	        return _cmt;
	    }

	    public void set_cmt(String _cmt) {
	        this._cmt = _cmt;
	    }

	    public String get_secondCheckFlag() {
	        return _secondCheckFlag;
	    }

	    public void set_secondCheckFlag(String _secondCheckFlag) {
	        this._secondCheckFlag = _secondCheckFlag;
	    }

	//    @SpaceProperty(index = SpaceProperty.IndexType.BASIC, nullValue="-1")
	    public long get_transactionSeq() {
	        return _transactionSeq;
	    }

	    public void set_transactionSeq(long _transactionSeq) {
	        this._transactionSeq = _transactionSeq;
	    }

	    public String get_finalApprovalFlag() {
	        return _finalApprovalFlag;
	    }

	    public void set_finalApprovalFlag(String _finalApprovalFlag) {
	        this._finalApprovalFlag = _finalApprovalFlag;
	    }

	    public String get_displayText() {
	        return _displayText;
	    }

	    public void set_displayText(String _displayText) {
	        this._displayText = _displayText;
	    }

	    public String get_category() {
	        return _category;
	    }

	    public void set_category(String _category) {
	        this._category = _category;
	    }

	    public String get_terminalFlag() {
	        return _terminalFlag;
	    }

	    public void set_terminalFlag(String _terminalFlag) {
	        this._terminalFlag = _terminalFlag;
	    }

	    public String get_problemFlag() {
	        return _problemFlag;
	    }

	    public void set_problemFlag(String _problemFlag) {
	        this._problemFlag = _problemFlag;
	    }

	    public String get_guiColorCode() {
	        return _guiColorCode;
	    }

	    public void set_guiColorCode(String _guiColorCode) {
	        this._guiColorCode = _guiColorCode;
	    }

	    public String get_delphiConstant() {
	        return _delphiConstant;
	    }

	    public void set_delphiConstant(String _delphiConstant) {
	        this._delphiConstant = _delphiConstant;
	    }

	    public String get_prelimAppr() {
	        return _prelimAppr;
	    }

	    public void set_prelimAppr(String _prelimAppr) {
	        this._prelimAppr = _prelimAppr;
	    }

	    public Date get_completedDt() {
	        return _completedDt;
	    }

	    public void set_completedDt(Date _completedDt) {
	        this._completedDt = _completedDt;
	    }

	    public Date get_completedTimestampGmt() {
	        return _completedTimestampGmt;
	    }

	    public void set_completedTimestampGmt(Date _completedTimestampGmt) {
	        this._completedTimestampGmt = _completedTimestampGmt;
	    }
	    @Override
		public String toString(){ 
	    	return _id+"|"+_tradeId+"|"+_rqmtTradeNotifyId+"|"+_rqmt+"|"+_status+"|"+_completedTimestampGmt+"|"+_completedDt+"|"+_reference+"|"+_cancelTradeNotifyId+"|"+_cmt+"|"+_secondCheckFlag+"|"+_transactionSeq+"|"+_finalApprovalFlag+"|"+_displayText+"|"+_category+"|"+_terminalFlag+"|"+_problemFlag+"|"+_guiColorCode+"|"+_delphiConstant+"|"+_prelimAppr;
	    }
	    
	    public String generateRqmtMethod(){
	    	String rqmt = this._rqmt;
   	
	    	if(rqmt.equals("XQBBP")){
    			if("Y".equals(this._secondCheckFlag)){
    				return "Broker Paper*";
    			} else return "Broker Paper";
	    	} else if (rqmt.equals("EFBKR")){
	    		return "EFET Broker";	
	    	} else if(rqmt.equals("ECONF")) {
	    		return "eConfirm Cpty";
	    	} else if(rqmt.equals("ECBKR")) {
	    		return "eConfirm Broker";
	    	} else if(rqmt.equals("EFET")){
	    		return "EFET Cpty";
	    	} else if(rqmt.equals("XQCSP")){
	    		return "Our Paper";
	    	} else if(rqmt.equals("XQCCP")){
    			if("Y".equals(this._secondCheckFlag)){
    				return "Cpty Paper*";
    			} else return "Cpty Paper";
	    	} else if(rqmt.equals("NOCNF")){
	    		return(this._reference);
	    	} else if(rqmt.equals("VBCP")){
	    		return "PHONE";
	    	} else if(rqmt.equals("MISC")){
	    		return "Misc";
	    	} else return null;
	    }
	    
	    public int generateRqmtMethodType(){
	    	String rqmt = this._rqmt;
	       	
	    	if(rqmt.equals("XQBBP")){
	    		return RqmtData.BKR_METHOD;
	    	} else if (rqmt.equals("EFBKR") || rqmt.equals("ECBKR")){
	    		return RqmtData.BKR_METHOD;
	    	} else if(rqmt.equals("ECONF")) {
	    		return RqmtData.SETC_METHOD;
	    	} else if(rqmt.equals("EFET")){
	    		return RqmtData.SETC_METHOD;
	    	} else if(rqmt.equals("XQCSP")){
	    		return RqmtData.SETC_METHOD;
	    	} else if(rqmt.equals("XQCCP")){
	    		return RqmtData.CPTY_METHOD;
	    	} else if(rqmt.equals("NOCNF")){
	    		return RqmtData.NOCONF_METHOD;
	    	} else if(rqmt.equals("VBCP")){
	    		return RqmtData.VERBL_METHOD;
	    	} else if(rqmt.equals("MISC")){
	    		return RqmtData.MISC_METHOD;
	    	}else return RqmtData.NA_METHOD;
	    	
	    }
	    
	    public static void main(String[] args) {
	//        IJSpace space;

//	        RqmtData data = new RqmtData();

	//        data.set_id(1000000);

//	        try {
	//            space = (IJSpace) SpaceFinder.find("jini://ST-XPMTHORE/./mySpace");
	 //           space.write(data, null, Lease.FOREVER);
	   //     } catch (FinderException e) {
	     //       e.printStackTrace();  //To change body of catch statement use File | Settings | File Templates.
	       // } catch (RemoteException e) {
//	            e.printStackTrace();  //To change body of catch statement use File | Settings | File Templates.
	//        } catch (TransactionException e) {
	  //          e.printStackTrace();  //To change body of catch statement use File | Settings | File Templates.
	    //    }
	    }
	}
