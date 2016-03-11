/**
 * User: islepini
 * Date: Aug 19, 2003
 * Time: 4:18:02 PM
 * To change this template use Options | File Templates.
 */
package aff.confirm.jboss.common.service.messageresender;

import aff.confirm.jboss.common.service.queueservice.QueueServiceMBean;

public interface MessageResenderMBean  extends QueueServiceMBean {
    String getSenderQueueNames();

    void setSenderQueueNames(String senderQueues);
}
