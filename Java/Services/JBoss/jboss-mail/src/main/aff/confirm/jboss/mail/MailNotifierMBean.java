/*
 * User: islepini
 * Date: Sep 26, 2002
 * Time: 5:11:33 PM
 * To change template for new interface use 
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.jboss.mail;

import org.jboss.system.ServiceMBean;

import javax.naming.NamingException;

public interface MailNotifierMBean extends ServiceMBean {
    String getJndiName();
    void setJndiName(String jndiName) throws NamingException;

    String getStartUpShutDownEventEmailList();
    void setStartUpShutDownEventEmailList(String value);

/*
    Element getNotifyGroups();
    void setNotifyGroups(Element value) throws Exception;
*/

    void sendMail(String subject,String content, String address);
    void sendMailToGroup(String subject,String content, String groupName) throws NamingException;

    String getMessageOnStartUp();
    void setMessageOnStartUp(String messageOnStartUp);

    String getMessageOnShutDown();
    void setMessageOnShutDown(String messageOnShutDown);

    String getSubjectOnStartUp();
    void setSubjectOnStartUp(String subjectOnStartUp);

    String getSubjectOnShutDown();
    void setSubjectOnShutDown(String subjectOnShutDown);

    String getWinNTserviceName();
    void setWinNTserviceName(String winNTserviceName);

    String getFromName();
    void setFromName(String fromName);

    String testDir();



}
