/*
 * User: islepini
 * Date: Oct 2, 2002
 * Time: 9:29:52 AM
 * To change template for new interface use 
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.jboss.queuemonitor;

import org.jboss.system.ServiceMBean;

public interface MonitorServiceMBean extends ServiceMBean {

    String getMonitorQueues();

    void setMonitorQueues(String monitorQueues) throws  Exception;

    String getMonitorInfo();

    void resetMonitorNotification();
}
