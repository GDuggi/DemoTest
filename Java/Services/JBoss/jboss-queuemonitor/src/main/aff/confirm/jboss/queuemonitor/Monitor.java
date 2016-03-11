/**
 * User: islepini
 * Date: Jul 1, 2003
 * Time: 10:08:34 AM
 * To change this template use Options | File Templates.
 */
package aff.confirm.jboss.queuemonitor;

import aff.confirm.common.util.JndiUtil;
import org.jboss.logging.Logger;

import javax.jms.*;
import javax.naming.InitialContext;
import java.util.Enumeration;
import java.util.LinkedList;
import java.util.TimerTask;

public class Monitor extends TimerTask {
    private LinkedList monitorQueueList = new LinkedList();
    private MonitorService service;
    private final Object LOCK = new Object();

    public Monitor(MonitorService service) {
        this.service = service;
    }

    public void run() {
        synchronized(LOCK){
            getInfo();
            checkQueueDepths();
        }
    }

    private void getInfo(){
        InitialContext ic = null;
        QueueConnection qc = null;
        QueueSession  qs = null;
        try {
            ConnectionFactory cf = JndiUtil.lookup("java:/ConnectionFactory");
            qc = ((QueueConnectionFactory)cf).createQueueConnection();
            qs = qc.createQueueSession(false,Session.AUTO_ACKNOWLEDGE);
            for (int i = 0; i < monitorQueueList.size(); i++) {
                MonitorQueue monitorQueue = (MonitorQueue) monitorQueueList.get(i);
                try{
                    Queue queue = JndiUtil.lookup("queue/"+monitorQueue.getName());
                    QueueBrowser qb = qs.createBrowser(queue);
                    int counter = 0; Enumeration enum1 = qb.getEnumeration();
                    while(enum1.hasMoreElements()){
                        counter++;
                        enum1.nextElement();
                    }
                    monitorQueue.setQueueDepth(counter);
                    monitorQueue.setFailed(false);
                } catch (Exception e) {
                    monitorQueue.setFailed(true);
                }
            }
        } catch (Exception e) {
            Logger.getLogger(Monitor.class).info(e);
        } finally {
            try {
                if( qc != null ) qc.close();
            } catch (Exception e1) {}

            try {
                if( qs != null ) qs.close();
            } catch (Exception e1) {}

        }
    }

    public String getMonitorInfo(){
        synchronized(LOCK){
            String result = "";
            for (int i = 0; i < monitorQueueList.size(); i++) {
                MonitorQueue monitorQueue = (MonitorQueue) monitorQueueList.get(i);
                result = result+monitorQueue.toString()+"\r\n";
            }
            return result;
        }
    }

    public void resetMonitorNotification(){
        synchronized(LOCK){
            for (int i = 0; i < monitorQueueList.size(); i++) {
                MonitorQueue monitorQueue = (MonitorQueue) monitorQueueList.get(i);
                monitorQueue.setNotified(false);
            }
        }
    }

    private void checkQueueDepths(){
        for (int i = 0; i < monitorQueueList.size(); i++) {
            MonitorQueue monitorQueue = (MonitorQueue) monitorQueueList.get(i);
            if(!monitorQueue.isNotified()){
                if(monitorQueue.getQueueDepth() > monitorQueue.getTreshHold()){
                     monitorQueue.setNotified(true);
                     service.emailNotify("Queue: "+monitorQueue.getName()+" exceeded its threshold value of "
                             +monitorQueue.getTreshHold()+", current queue depth is "+monitorQueue.getQueueDepth());
                }
            }else{
                if(monitorQueue.getQueueDepth() <= monitorQueue.getTreshHold()){
                     monitorQueue.setNotified(false);
                     service.emailNotify("Queue: "+monitorQueue.getName()+" is back to normal depth of "+monitorQueue.getQueueDepth());
                }
            }
        }
    }

    public void clearQueueList() {
        synchronized(LOCK){
            monitorQueueList.clear();
        }
    }

    public void addMonitorQueue(String queueName, int treshHold) {
        synchronized(LOCK){
            monitorQueueList.add(new MonitorQueue(queueName,treshHold));
        }
    }
}
