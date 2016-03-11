/**
 * User: islepini
 * Date: Aug 8, 2003
 * Time: 9:04:41 AM
 * To change this template use Options | File Templates.
 */
package aff.confirm.jboss.common.service.taskservice;

import aff.confirm.jboss.common.exceptions.LogException;
import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.service.Service;

import java.util.Timer;
import java.util.TimerTask;

abstract public class TaskService extends Service {
    private Timer timer;
    private Task task;

    public TaskService(String objectNameStr) {
        super(objectNameStr);
    }

    public long getTimerPeriod() {
        return timerPeriod;
    }

    public void setTimerPeriod(long timerPeriod) {
        this.timerPeriod = timerPeriod;
    }

    private long timerPeriod;
    class Task extends TimerTask{
        public void run(){
           if(!started){
               started = true;
               try {
                   Thread.sleep(20*1000);
               } catch (InterruptedException e) {
                   log.error(e);
               }
           }
            synchronized(TaskService.this){
                try {
                    if(!getStopingService()){
                        runTask();
                    }

                } catch (StopServiceException e) {
                    log.error(e);
                    notifyEmailGroupServiceStoped(e.getMessage());
                    stop();
                } catch (LogException e) {
                    log.error(e);
                    notifyEmailGroupServiceFailedToProcess(e.getMessage());
                }
            }
        }
    }

    protected void runTask() throws StopServiceException,LogException {
    }

    final protected void onInternalServiceStarting() throws Exception{
        onServiceStarting();
    }

    final protected void onInternalServiceStoping(){
        stopTimer();
        onServiceStoping();
    }

    abstract protected void onServiceStarting() throws Exception;

    abstract protected void onServiceStoping();

    private void stopTimer(){
       if(timer !=  null){
           timer.cancel();
           timer = null;
       }
       if(task != null){
            task = null;
        }
    }

    public void startProcessing() throws Exception{
       stopTimer();
       timer = new Timer(true);
       task = new Task();
       timer.schedule(task,timerPeriod,timerPeriod);
    }
}
