/**
 * User: islepini
 * Date: Jul 29, 2003
 * Time: 1:30:42 PM
 * To change this template use Options | File Templates.
 */
package aff.confirm.jboss.common.service.queueservice;

import aff.confirm.jboss.common.service.ServiceMBean;

public interface QueueServiceMBean extends ServiceMBean {
    String getQueueName();
    void setQueueName(String queueName);
    String getSourceServerName();
    void setSourceServerName(String sourceServerName);
    boolean isTransactional();
    void setTransactional( boolean transactional );    
}
