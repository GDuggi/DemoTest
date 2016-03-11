package aff.confirm.opsmanager.opssubpub.opstrackingmodel;
import java.io.Serializable;

//import com.gigaspaces.annotation.pojo.SpaceClass;
//import com.gigaspaces.annotation.pojo.SpaceProperty;

//@SpaceClass
public class RqmtStatusView implements Serializable {
    /**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	private String _rqmtCode = null;
    private String _displayText = null;
    private String _initialStatus = null;
    private String _statusCode = null;
    private String _terminalFlag = null;
    private String _problemFlag = null;
    private String _colorCode = null;
    private String _descr = null;
    private long _ord;
    
    

    public String get_rqmtCode() {
		return _rqmtCode;
	}
	
	public void set_rqmtCode(String code) {
		_rqmtCode = code;
	}
	
	public String get_displayText() {
		return _displayText;
	}
	
	public void set_displayText(String text) {
		_displayText = text;
	}
	
	public String get_initialStatus() {
		return _initialStatus;
	}
	
	public void set_initialStatus(String status) {
		_initialStatus = status;
	}
	
	public String get_statusCode() {
		return _statusCode;
	}
	
	public void set_statusCode(String code) {
		_statusCode = code;
	}

	public String get_terminalFlag() {
		return _terminalFlag;
	}
	
	public void set_terminalFlag(String flag) {
		_terminalFlag = flag;
	}
	
	public String get_problemFlag() {
		return _problemFlag;
	}
	public void set_problemFlag(String flag) {
		_problemFlag = flag;
	}
	
	public String get_colorCode() {
		return _colorCode;
	}
	
	public void set_colorCode(String code) {
		_colorCode = code;
	}
	
	public static long getSerialVersionUID() {
		return serialVersionUID;
	}
	
	public String get_descr() {
		return _descr;
	}
	
	public void set_descr(String _descr) {
		this._descr = _descr;
	}
	
 //   @SpaceProperty(nullValue="-1")
    public long get_ord() {
        return _ord;
    }

    public void set_ord(long _ord) {
        this._ord = _ord;
    }
}
