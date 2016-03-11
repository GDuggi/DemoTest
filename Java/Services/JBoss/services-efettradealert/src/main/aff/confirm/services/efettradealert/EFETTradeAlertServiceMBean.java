/*
 * User: islepini
 * Date: Oct 2, 2002
 * Time: 9:29:52 AM
 * To change template for new interface use
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.services.efettradealert;

import aff.confirm.jboss.common.service.queueservice.QueueServiceMBean;

import javax.management.ObjectName;
import javax.management.MalformedObjectNameException;

public interface EFETTradeAlertServiceMBean extends QueueServiceMBean{
    void setOpsTrackingDBInfoName(String pOpsTrackingDBInfoName);
    ObjectName getOpsTrackingDBInfo() throws MalformedObjectNameException;

    void setAffinityDBInfoName(String pAffinityDBInfoName);
    ObjectName getAffinityDBInfo() throws MalformedObjectNameException;

    //void setSymphonyDBInfoName(String pSymphonyDBInfoName);
    //ObjectName getSymphonyDBInfo() throws MalformedObjectNameException;
}
