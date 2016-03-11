package aff.confirm.services.efetmonitor;

import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.service.taskservice.TaskServiceMBean;
import javax.management.MalformedObjectNameException;
import javax.management.ObjectName;

public interface EFETMonitorServiceMBean extends TaskServiceMBean{
    void setOpsTrackingDBInfoName(String pOpsTrackingDBInfoName);
    ObjectName getOpsTrackingDBInfo() throws MalformedObjectNameException;
    void setAffinityDBInfoName(String pAffinityDBInfoName);
    ObjectName getAffinityDBInfo() throws MalformedObjectNameException;
    void executeTimerEventNow() throws StopServiceException;
}
