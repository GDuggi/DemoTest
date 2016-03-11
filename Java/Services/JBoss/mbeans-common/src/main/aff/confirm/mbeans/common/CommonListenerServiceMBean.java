package aff.confirm.mbeans.common;

import aff.confirm.jboss.common.service.taskservice.TaskServiceMBean;
import aff.confirm.mbeans.common.exceptions.StopServiceException;


/**
 * User: mthoresen
 * Date: Jun 19, 2009
 * Time: 11:10:25 AM
 */
public interface CommonListenerServiceMBean extends TaskServiceMBean {
    int  getTerminalErrorCount();
    void setTerminalErrorCount(int errorCount);

    String getEmailFrom();
    void  setEmailFrom(String emailFrom);

    String getEmailTo();
    void  setEmailTo(String emailTo);

    public boolean isProcessing();
    public void executeTimerEventNow();

    public void incErrorCount(int value) throws StopServiceException;
    public void resetErrorCount();
}
