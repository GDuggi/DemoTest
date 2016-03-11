package OpsTrackingModel;

import java.io.Serializable;

//import com.gigaspaces.annotation.pojo.SpaceClass;
//import com.gigaspaces.annotation.pojo.SpaceId;
//import com.gigaspaces.annotation.pojo.SpaceRouting;

//@SpaceClass
public class BdtaSeCptyLkup implements Serializable{

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	
	private String _cptySn = null;
	
  //  @SpaceId    
 //   @SpaceRouting
	public String get_cptySn() {
		return _cptySn;
	}
	public void set_cptySn(String sn) {
		_cptySn = sn;
	}
}
