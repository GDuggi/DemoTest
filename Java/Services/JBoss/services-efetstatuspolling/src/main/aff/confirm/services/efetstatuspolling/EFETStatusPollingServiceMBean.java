/*
 * User: islepini
 * Date: Oct 2, 2002
 * Time: 9:29:52 AM
 * To change template for new interface use
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.services.efetstatuspolling;

import aff.confirm.jboss.common.service.taskservice.TaskServiceMBean;
import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.exceptions.LogException;

import javax.management.MalformedObjectNameException;
import javax.management.ObjectName;

public interface EFETStatusPollingServiceMBean extends TaskServiceMBean{
    void setOpsTrackingDBInfoName(String pOpsTrackingDBInfoName);
    ObjectName getOpsTrackingDBInfo() throws MalformedObjectNameException;

    void setAffinityDBInfoName(String pAffinityDBInfoName);
    ObjectName getAffinityDBInfo() throws MalformedObjectNameException;
    String getSmtpHost();
    void setSmtpHost(String pSMTPHost);
    String getSmtpPort();
    void setSmtpPort(String pSMTPPort);


    String getEFETBoxInbox();
    void setEFETBoxInbox(String pEFETBoxInbox);
    String getEFETBoxFailed();
    void setEFETBoxFailed(String pEFETBoxFailed);
    String getEFETBoxProcessed();
    void setEFETBoxProcessed(String pEFETBoxProcessed);


    void executeTimerEventNow() throws StopServiceException, LogException;

    String getEnv();
    void setEnv(String pEnv);
}
