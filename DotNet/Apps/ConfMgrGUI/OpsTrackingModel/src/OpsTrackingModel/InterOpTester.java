package OpsTrackingModel;

import java.io.Serializable;
import java.util.Date;

//import com.gigaspaces.annotation.pojo.SpaceId;
//import com.gigaspaces.annotation.pojo.SpaceProperty;
//import com.gigaspaces.annotation.pojo.SpaceRouting;

public class InterOpTester implements Serializable {

	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	private long _id = -1;
    private Date  _myDate = null;
    
    
  //  @SpaceId    //	Sets this field (ID) to be used in the construction
 //   @SpaceRouting
 //   @SpaceProperty(nullValue="-1")
	public long get_id() {
		return _id;
	}
	public void set_id(long _id) {
		this._id = _id;
	}
	public Date get_myDate() {
		return _myDate;
	}
	public void set_myDate(Date date) {
		_myDate = date;
	}
}
