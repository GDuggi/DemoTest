package OpsTrackingModel;

//import com.gigaspaces.annotation.pojo.SpaceId;
//import com.gigaspaces.annotation.pojo.SpaceProperty;
//import com.gigaspaces.annotation.pojo.SpaceRouting;
import java.io.Serializable;


public class TransactionSequence implements Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	private long _id = -1;

  //  @SpaceId    //	Sets this field (ID) to be used in the construction
 //   @SpaceRouting
 //   @SpaceProperty(nullValue="-1")
	public long get_id() {
		return _id;
	}

	public void set_id(long _id) {
		this._id = _id;
	}
}
