/*
 * User: islepini
 * Date: Oct 2, 2002
 * Time: 9:29:52 AM
 * To change template for new interface use
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.services.efettradesubmitter;

import aff.confirm.jboss.common.service.taskservice.TaskServiceMBean;
import aff.confirm.jboss.common.exceptions.StopServiceException;

import javax.management.MalformedObjectNameException;
import javax.management.ObjectName;

public interface EFETTradeSubmitterServiceMBean extends TaskServiceMBean{
    void setOpsTrackingDBInfoName(String pOpsTrackingDBInfoName);
    ObjectName getOpsTrackingDBInfo() throws MalformedObjectNameException;
    void setAffinityDBInfoName(String pAffinityDBInfoName);
    ObjectName getAffinityDBInfo() throws MalformedObjectNameException;
    String getEFETWorkDir();
    void setEFETWorkDir(String pEFETWorkDir);
    String getEFETBoxOutbox();
    void setEFETBoxOutbox(String pEFETBoxOutbox);
    String getDocumentUsage();
    void setDocumentUsage(String pDocumentUsage);
   /* int getFileStoreExpireDays();
    void setFileStoreExpireDays(int pFileStoreExpireDays);*/

    void executeTimerEventNow() throws StopServiceException;
    String getSmtpHost();
    void setSmtpHost(String pSMTPHost);

    String getSmtpPort();
    void setSmtpPort(String pSMTPPort);

    String getEnv();
    void setEnv(String pEnv);


}
