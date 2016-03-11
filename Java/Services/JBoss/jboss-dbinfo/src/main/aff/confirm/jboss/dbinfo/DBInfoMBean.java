/*
 * User: islepini
 * Date: Oct 2, 2002
 * Time: 9:29:52 AM
 * To change template for new interface use 
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.jboss.dbinfo;

import org.jboss.system.ServiceMBean;

import javax.naming.NamingException;

public interface DBInfoMBean extends ServiceMBean {
    String getOracleTNSPath();
    void setOracleTNSPath(String pOracleTNSPath);

    String getJndiName();
    void setJndiName(String jndiName) throws NamingException;

    String getDBUserName();
    void setDBUserName(String value);

    String getDBPassword();
    void setDBPassword(String value);

    String getDBUrl();
    void setDBUrl(String value);

    String getDatabaseName();
    void setDatabaseName(String value);

    void setTopicName(String topicName);

    boolean isDbUp();

}
