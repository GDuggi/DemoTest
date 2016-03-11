/*
 * User: islepini
 * Date: Oct 2, 2002
 * Time: 9:29:52 AM
 * To change template for new interface use
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.services.econfirmstatuspolling;

import aff.confirm.jboss.common.exceptions.LogException;
import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.service.taskservice.TaskServiceMBean;

import javax.management.MalformedObjectNameException;
import javax.management.ObjectName;

public interface EConfirmStatusPollingServiceMBean extends TaskServiceMBean{
    String getEConfirmAPIUrl();
    void setEConfirmAPIUrl(String pEConfirmAPIUrl);

    String getEConfirmTradeInfoServiceUrl();
    void setEConfirmTradeInfoServiceUrl(String pEConfirmTradeInfoServiceUrl);

    void setOpsTrackingDBInfoName(String pOpsTrackingDBInfoName);
    ObjectName getOpsTrackingDBInfo() throws MalformedObjectNameException;

    void setAffinityDBInfoName(String pAffinityDBInfoName);
    ObjectName getAffinityDBInfo() throws MalformedObjectNameException;

//    void setSymphonyDBInfoName(String pSymphonyDBInfoName);
//    ObjectName getSymphonyDBInfo() throws MalformedObjectNameException;

    String getSmtpHost();
    void setSmtpHost(String pSMTPHost);

    String getSmtpPort();
    void setSmtpPort(String pSMTPPort);

    String getFileStoreDir();
    void setFileStoreDir(String pFileStoreDir);

    int getFileStoreExpireDays();
    void setFileStoreExpireDays(int pFileStoreExpireDays);

    String getAllegedQueryXSL();
    void setAllegedQueryXSL(String pAllegedQueryXSL);

    String getAllegedQueryHTML();
    void setAllegedQueryHTML(String pAllegedQueryHTML);    

    String getProxyType();
    void setProxyType(String proxyType) ;

    String getProxyUrl();
    void setProxyUrl(String proxyUrl) ;

     int getProxyPort();
     void setProxyPort(int proxyPort) ;

    int getMessageLogDisplayIgnoreCount();
    void setMessageLogDisplayIgnoreCount(int pMessageLogDisplayIgnoreCount);

    void executeTimerEventNow() throws StopServiceException, LogException;

//    int getConnectRetryAttempts();
//    void setConnectRetryAttempts(int connectRetryAttempts);

    String getEnv();
    void setEnv(String pEnv);
}
