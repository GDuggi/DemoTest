/*
 * User: islepini
 * Date: Sep 27, 2002
 * Time: 10:18:08 AM
 * To change template for new class use 
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.jboss.queuemgr;

import org.jboss.system.ServiceMBean;

import javax.jms.JMSException;
import javax.naming.NamingException;
import java.sql.SQLException;

public interface QueueMgrMBean extends ServiceMBean {

    String writeQueueMessagesIntoFileUsingFilter(String queueName,String fileName,String filterExpr);

    String removeMessagesFromQueueUsingFilter(String queueName, String filterExpr);

    String publishIntoFileFromTradeAuditIDToTradeAuditID(String fileName, int startTradeAuditID, int endTradeAuditID) throws Exception, SQLException, JMSException;

    String publishIntoQueueFromTradeAuditWhere(String queueName, String sqlExpr) throws Exception, NamingException, SQLException;

    String publishIntoFileFromTradeAuditWhere(String queueName, String sqlExpr) throws Exception, NamingException, SQLException;

    String publishIntoQueueFromTradeAuditIDToTradeAuditID(String queueName, int startTradeAuditID, int endTradeAuditID) throws Exception, NamingException, SQLException;

    String publishIntoQueueTradeAuditIDCommaList(String queueName, String idList) throws Exception, NamingException, SQLException;

    String publishFromFileIntoQueue(String fileName, String queueName);

    String publishFromFileIntoTopic(String fileName, String topicName);
}

