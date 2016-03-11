package OpsTrackingModel;

//import com.gigaspaces.annotation.pojo.SpaceClass;
//import com.gigaspaces.annotation.pojo.SpaceId;
//import com.gigaspaces.annotation.pojo.SpaceRouting;

import java.io.Serializable;

//@SpaceClass
public class ColorTranslate implements Serializable {
    /**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	/**
	 * 
	 */
	private String _code;
    private String _csColor;


  //  @SpaceId    //	Sets this field (ID) to be used in the construction
  //  @SpaceRouting
    public String get_code() {
        return _code;
    }

    public void set_code(String _code) {
        this._code = _code;
    }

    public String get_csColor() {
        return _csColor;
    }

    public void set_csColor(String _csColor) {
        this._csColor = _csColor;
    }
}
