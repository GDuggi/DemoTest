/**
 * User: islepini
 * Date: Jul 29, 2003
 * Time: 1:28:43 PM
 */
package aff.confirm.jboss.common.service;

import javax.management.MalformedObjectNameException;
import javax.management.ObjectName;

public interface ServiceMBean extends org.jboss.system.ServiceMBean {
    String getDbInfoName();
    void setDbInfoName(String dbInfoName);
    ObjectName getDbInfo() throws MalformedObjectNameException;
    String getNotifyGroupName();
    void setNotifyGroupName(String notifyGroupName);
    boolean isNotificationIgnore();
    void setNotificationIgnore(boolean notificationIgnore);
}
