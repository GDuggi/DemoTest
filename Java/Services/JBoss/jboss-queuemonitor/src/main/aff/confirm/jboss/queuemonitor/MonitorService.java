/*
 * User: islepini
 * Date: Oct 2, 2002
 * Time: 9:30:52 AM
 * To change template for new class use 
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.jboss.queuemonitor;

import aff.confirm.common.util.JndiUtil;
import aff.confirm.jboss.common.service.BasicMBeanSupport;
import aff.confirm.jboss.mail.MailNotifier;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;

import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.ejb.Singleton;
import javax.ejb.Startup;
import java.util.Timer;

@Startup
@Singleton
public class MonitorService extends BasicMBeanSupport implements MonitorServiceMBean {
    private Monitor monitor;
    private String monitorQueues;
    private int intervalInSec = 30000;
    private Timer timer;

    public MonitorService() {
        super("affinity.utils:service=QueueMonitor");
        monitor = new Monitor(this);
    }

    @PostConstruct
    public void postConstruct() throws Exception {
        super.postConstruct();
    }

    @PreDestroy
    public void preDestroy() throws Exception {
        super.preDestroy();
    }

    public String getMonitorQueues() {
        return monitorQueues;
    }

    public void setMonitorQueues(String monitorQueues) throws Exception {
        parseMonitorQueues(monitorQueues);
        this.monitorQueues = monitorQueues;
    }

    public String getMonitorInfo() {
        return monitor.getMonitorInfo();
    }

    public void resetMonitorNotification() {
        monitor.resetMonitorNotification();
    }

    public void startService() throws Exception {
        init();
    }

    public void stopService() {
        close();
    }

    private void init() {
        timer = new Timer(true);
        timer.schedule(monitor, 0, intervalInSec);
    }

    private void close() {
        timer.cancel();
    }


    private void parseMonitorQueues(String element) throws Exception {
        element = element.trim();
        String queues[] = element.split((";"));
        if (queues != null) {
            for (int i = 0; i < queues.length; ++i) {
                String queueInfo = queues[i];
                if (queueInfo != null) {
                    String[] queueDetails = queueInfo.split(",");
                    String queueName = queueDetails[0].trim();
                    int limit = Integer.parseInt(queueDetails[1].trim());
                    log.info("Adding monitor for queue " + queueName + " with limit " + limit);
                    monitor.addMonitorQueue(queueName, limit);
                }
            }
        }

    }

    private void parseMonitorQueues(Element element) throws Exception {
        monitor.clearQueueList();

        String rootTag = element.getTagName();
        if (!rootTag.equals("queues")) {
            throw new Exception("Not a resource adapter deployment " +
                    "descriptor because its root tag, '" +
                    rootTag + "', is not 'queues'");
        }
        invokeChildren(element);
    }

    private void invokeChildren(Element element) throws Exception {
        NodeList children = element.getChildNodes();
        for (int i = 0; i < children.getLength(); ++i) {
            Node node = children.item(i);
            if (node.getNodeType() == Node.ELEMENT_NODE) {
                Element child = (Element) node;
                String tag = child.getTagName();
                if (!tag.equals("queue")) {
                    throw new Exception("Not a resource adapter deployment " +
                            "descriptor because its tag, '" +
                            tag + "', is not 'queue'");
                }
                if (!child.hasAttribute("name")) {
                    throw new Exception("Not a resource adapter deployment " +
                            "descriptor because its tag, '" +
                            tag + "', has not attribute 'name'");
                }
                String name = child.getAttribute("name");
                if (!child.hasAttribute("treshhold")) {
                    throw new Exception("Not a resource adapter deployment " +
                            "descriptor because its tag, '" +
                            tag + "', has not attribute 'treshhold'");
                }
                String treshHold = child.getAttribute("treshhold");
                log.info(name);
                monitor.addMonitorQueue(name, new Integer(treshHold).intValue());

            }
        }
    }

    synchronized public void emailNotify(String message) {
        MailNotifier mn = null;
        try {
            mn = JndiUtil.lookup("MailNotifier");
            mn.sendMailToGroup("Queue Monitor Service notification", message, "ContractWorkflow");
        } catch (Exception e) {
            log.error(e);
        }
    }

}
