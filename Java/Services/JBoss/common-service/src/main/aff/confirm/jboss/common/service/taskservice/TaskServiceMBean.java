/**
 * User: islepini
 * Date: Aug 8, 2003
 * Time: 9:05:08 AM
 * To change this template use Options | File Templates.
 */
package aff.confirm.jboss.common.service.taskservice;

import aff.confirm.jboss.common.service.ServiceMBean;

public interface TaskServiceMBean extends ServiceMBean{
    long getTimerPeriod();
    void setTimerPeriod(long timerPeriod);
}
