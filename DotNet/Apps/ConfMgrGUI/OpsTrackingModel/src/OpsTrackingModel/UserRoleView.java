package OpsTrackingModel;

//import com.gigaspaces.annotation.pojo.SpaceClass;
//import com.gigaspaces.annotation.pojo.SpaceProperty;

import java.io.Serializable;

//@SpaceClass
public class UserRoleView implements Serializable {
    /**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	private String _userId = null;
    private String _roleCode = null;
    private String _descr = null;


  //  @SpaceProperty(index = SpaceProperty.IndexType.BASIC)
    public String get_userId() {
        return _userId;
    }

    public void set_userId(String _userId) {
        this._userId = _userId;
    }

    public String get_roleCode() {
        return _roleCode;
    }

    public void set_roleCode(String _roleCode) {
        this._roleCode = _roleCode;
    }

    public String get_descr() {
        return _descr;
    }

    public void set_descr(String _activeFlag) {
        this._descr = _activeFlag;
    }
}
