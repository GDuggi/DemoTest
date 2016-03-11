package aff.confirm.opsmanager.opssubpub.opstrackingmodel;

//import com.gigaspaces.annotation.pojo.SpaceClass;
//import com.gigaspaces.annotation.pojo.SpaceId;
//import com.gigaspaces.annotation.pojo.SpaceRouting;

import java.io.Serializable;

//@SpaceClass
public class RqmtStatusColor implements Serializable {
    /**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	private String _hashKey;
    private String _csColor;


  //  @SpaceId     //	Sets this field (ID) to be used in the construction
  //  @SpaceRouting
    public String get_hashKey() {
        return _hashKey;
    }

    public void set_hashKey(String _hashKey) {
        this._hashKey = _hashKey;
    }

    public String get_csColor() {
        return _csColor;
    }

    public void set_csColor(String _csColor) {
        this._csColor = _csColor;
    }
}
