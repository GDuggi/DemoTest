/*
 * User: islepini
 * Date: Oct 2, 2002
 * Time: 9:29:52 AM
 * To change template for new interface use
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.services.opstrackingtradealert;

import aff.confirm.jboss.common.service.queueservice.QueueServiceMBean;
import javax.management.MalformedObjectNameException;
import javax.management.ObjectName;

public interface OpsTrackingTradeAlertServiceMBean extends QueueServiceMBean{

    void setEConfirmTradeSubmitQueueName(String pEConfirmTradeSubmitQueueName);
    void setEFETTradeSubmitQueueName(String pEFETTradeSubmitQueueName);

    void setOpsTrackingDBInfoName(String pOpsTrackingDBInfoName);
    ObjectName getOpsTrackingDBInfo() throws MalformedObjectNameException;

    void setAffinityDBInfoName(String pAffinityDBInfoName);
    ObjectName getAffinityDBInfo() throws MalformedObjectNameException;

//    void setSymphonyDBInfoName(String pSymphonyDBInfoName);
//    ObjectName getSymphonyDBInfo() throws MalformedObjectNameException;

    void setTradeDataWebServiceUrl(String pTradeDataWebServiceUrl);
    String getTradeDataWebServiceUrl();

    void setTradeDataRootTagName(String pTradeDataRootTagName);
    String getTradeDataRootTagName();

    String getSmtpHost();
    void setSmtpHost(String smtpHost);

    String getSmtpPort();
    void setSmtpPort(String smtpHost);

    String getSendToAddress();
    void setSendToAddress(String pSendToAddress);

    String getSendToName();
    void setSendToName(String pSendToName);

    String getECFailedLogAddress();
    void setECFailedLogAddress(String pECFailedLogAddress);

    String getSystemsNotifyToAddress();
    void setSystemsNotifyToAddress(String pSystemsNotifyToAddress);

    String getSystemsNotifyFromDomain();
    void setSystemsNotifyFromDomain(String pSystemsNotifyFromDomain);

    String getEFETWarningAddress();
    void setEFETWarningAddress(String pEFETWarningAddress);

    String getStopServiceNotifyAddress();
    void setStopServiceNotifyAddress(String pStopServiceNotifyAddress);

    String getEnv();
    void setEnv(String pEnv);

    String getEConfirmTradeInfoServiceUrl();
    void setEConfirmTradeInfoServiceUrl(String pEConfirmTradeInfoServiceUrl);
}
