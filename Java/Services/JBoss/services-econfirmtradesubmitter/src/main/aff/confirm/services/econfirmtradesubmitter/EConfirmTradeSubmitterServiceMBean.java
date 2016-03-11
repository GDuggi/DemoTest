/*
 * User: islepini
 * Date: Oct 2, 2002
 * Time: 9:29:52 AM
 * To change template for new interface use
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.services.econfirmtradesubmitter;

import aff.confirm.jboss.common.service.taskservice.TaskServiceMBean;
import aff.confirm.jboss.common.exceptions.StopServiceException;

import javax.management.MalformedObjectNameException;
import javax.management.ObjectName;

public interface EConfirmTradeSubmitterServiceMBean extends TaskServiceMBean{
    String getEConfirmAPIUrl();
    void setEConfirmAPIUrl(String pEConfirmAPIUrl);

    String getEConfirmTradeInfoServiceUrl();
    void setEConfirmTradeInfoServiceUrl(String pEConfirmTradeInfoServiceUrl);

    void setOpsTrackingDBInfoName(String pOpsTrackingDBInfoName);
    ObjectName getOpsTrackingDBInfo() throws MalformedObjectNameException;
    void setAffinityDBInfoName(String pAffinityDBInfoName);
    ObjectName getAffinityDBInfo() throws MalformedObjectNameException;
    // Samy : 07/08/11 commented to skip sybase connection
    /*
    void setICTSDBInfoName(String pICTSDBInfoName);
    ObjectName getICTSDBInfo() throws MalformedObjectNameException;
    */
    String getFileStoreDir();
    void setFileStoreDir(String pFileStoreDir);
    int getFileStoreExpireDays();
    void setFileStoreExpireDays(int pFileStoreExpireDays);

//    int getClientTimeOutPeriod();
//    void setClientTimeOutPeriod(int clientTimeOutPeriod);

    int getConnectAttempts();
    void setConnectAttempts(int connectAttempts);

    String getProxyType();
    void setProxyType(String proxyType) ;

    String getProxyUrl();
    void setProxyUrl(String proxyUrl) ;

    int getProxyPort();
    void setProxyPort(int proxyPort) ;
        

    void executeTimerEventNow() throws StopServiceException;
}
