package aff.confirm.opsmanager.opssubpub.opstrackingmodel;

import java.io.Serializable;

//import com.gigaspaces.annotation.pojo.SpaceClass;
//import com.gigaspaces.annotation.pojo.SpaceId;
//import com.gigaspaces.annotation.pojo.SpaceRouting;

//@SpaceClass
public class BdtaCdtyLkup implements Serializable{

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	
	private String _cdtyCode = null;

 //   @SpaceId    
 //   @SpaceRouting
	public String get_cdtyCode() {
		return _cdtyCode;
	}

	public void set_cdtyCode(String _cdty) {
		this._cdtyCode = _cdty;
	}

	public static long getSerialVersionUID() {
		return serialVersionUID;
	}
	
	

}
