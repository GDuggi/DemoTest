package aff.confirm.services.tapublisher;

import aff.confirm.jboss.common.service.ServiceMBean;


/**
 * User: srajaman
 * Date: Jul 10, 2008
 * Time: 4:50:17 PM
 */
public interface TAPublisherServiceMBean extends ServiceMBean {

    String getJmsServer();
    void setJmsServer(String serverName);

    String getUser();
    void setUser(String userId);

    String getPwd();
    void   setPwd(String pwd);

    String getReceiverQueue();
    void setReceiverQueue(String receiverQueue);

    String getSenderQueue();
    void setSenderQueue(String senderQueue);

    void setSmtpHost(String smtpHost);
	String getSmtpHost();

	void setSmtpPort(String smtpPort);
	String getSmtpPort();

    void setNextAuditId(long auditId);
    long getNextAuditId();

    String getNotifyAddr();
    void setNotifyAddr(String addr);


}
