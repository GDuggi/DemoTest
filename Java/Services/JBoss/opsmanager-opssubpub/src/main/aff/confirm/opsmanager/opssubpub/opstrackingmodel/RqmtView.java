package aff.confirm.opsmanager.opssubpub.opstrackingmodel;
//import com.gigaspaces.annotation.pojo.SpaceClass;
//import com.gigaspaces.annotation.pojo.SpaceId;
//import com.gigaspaces.annotation.pojo.SpaceRouting;

import java.io.Serializable;

//@SpaceClass
public class RqmtView implements Serializable {
    /**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	private String _code = null;
    private String _descr = null;
    private String _category = null;
    private String _initialStatus = null;
    private String _displayText = null;
    private String _activeFlag = null;
    private String _detActRqmtFlag = null;
    

  //  @SpaceId    
   // @SpaceRouting
	public String get_code() {
		return _code;
	}
	public void set_code(String _code) {
		this._code = _code;
	}
	public String get_descr() {
		return _descr;
	}
	public void set_descr(String _descr) {
		this._descr = _descr;
	}
	public String get_category() {
		return _category;
	}
	public void set_category(String _category) {
		this._category = _category;
	}
	public String get_initialStatus() {
		return _initialStatus;
	}
	public void set_initialStatus(String status) {
		_initialStatus = status;
	}
	public String get_displayText() {
		return _displayText;
	}
	public void set_displayText(String text) {
		_displayText = text;
	}
	public String get_activeFlag() {
		return _activeFlag;
	}
	public void set_activeFlag(String flag) {
		_activeFlag = flag;
	}
	public String get_detActRqmtFlag() {
		return _detActRqmtFlag;
	}
	public void set_detActRqmtFlag(String actRqmtFlag) {
		_detActRqmtFlag = actRqmtFlag;
	}
}
