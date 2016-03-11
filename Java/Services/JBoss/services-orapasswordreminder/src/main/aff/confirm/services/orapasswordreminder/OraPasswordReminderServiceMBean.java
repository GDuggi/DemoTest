package aff.confirm.services.orapasswordreminder;

import aff.confirm.jboss.common.exceptions.LogException;
import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.service.taskservice.TaskServiceMBean;

public interface OraPasswordReminderServiceMBean extends TaskServiceMBean {
    String getSmtpHost();

    void setSmtpHost(String pSMTPHost);

    String getSmtpPort();

    void setSmtpPort(String pSMTPPort);

    String getLeadDays();

    void setLeadDays(String pLeadDays);

    String getRunAtTimeOfDay();

    void setRunAtTimeOfDay(String pRunAtTimeOfDay);

    String getOraPasswordResetUrl();

    void setOraPasswordResetUrl(String pOraPasswordResetUrl);

    String getNotifyEmailDomainName();

    void setNotifyEmailDomainName(String pNotifyEmailDomainName);

    String getNotifyEmailSentFromName();

    void setNotifyEmailSentFromName(String pNotifyEmailSentFromName);

    String getNotifyEmailSubject();

    void setNotifyEmailSubject(String pNotifyEmailSubject);

    String getNotifyEmailBodyText();

    void setNotifyEmailBodyText(String pNotifyEmailBodyText);

    String getDeploymentVerifyEmailAddress();

    void setDeploymentVerifyEmailAddress(String pDeploymentVerifyEmailAddress);

    String getDeploymentVerifyEmailName();

    void setDeploymentVerifyEmailName(String pDeploymentVerifyEmailName);

    String getIsProduction();

    void setIsProduction(String pIsProduction);

    String getInitConfigFileToYesterday();

    void setInitConfigFileToYesterday(String pInitConfigFileToYesterday);

    void executeTimerEventNow() throws StopServiceException, LogException;
}
