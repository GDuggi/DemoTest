package aff.confirm.opsmanager.tc.data;

import java.io.Serializable;

public class JMSTradeProcessRequest implements Serializable{
	
	private long jmsTradeId;

	public long getJmsTradeId() {
		return jmsTradeId;
	}

	public void setJmsTradeId(long jmsTradeId) {
		this.jmsTradeId = jmsTradeId;
	}
	

}
