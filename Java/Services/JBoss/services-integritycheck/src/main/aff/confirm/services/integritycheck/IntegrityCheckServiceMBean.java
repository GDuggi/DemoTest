/**
 * User: islepini
 * Date: Sep 16, 2003
 * Time: 1:09:57 PM
 * To change this template use Options | File Templates.
 */
package aff.confirm.services.integritycheck;

import aff.confirm.jboss.common.service.taskservice.TaskServiceMBean;


public interface IntegrityCheckServiceMBean extends TaskServiceMBean {
    String getMonitorQueries();
    void setMonitorQueries(String monitorQueries) throws Exception;
    String getSmtpHost();
    void setSmtpHost(String pSMTPHost);

    String getSmtpPort();
    void setSmtpPort(String pSMTPPort);

    boolean getAutoResetCreditMargin();
    void setAutoResetCreditMargin(boolean autoResetCreditMargin);

    int getMaxCreditMarginResets();
    void setMaxCreditMarginResets(int maxCreditMarginResets);
}
