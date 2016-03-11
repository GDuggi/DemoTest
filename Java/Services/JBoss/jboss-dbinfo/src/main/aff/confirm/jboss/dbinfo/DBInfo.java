/*
 * User: islepini
 * Date: Oct 2, 2002
 * Time: 9:30:52 AM
 */
package aff.confirm.jboss.dbinfo;

import aff.confirm.common.util.JndiUtil;
import aff.confirm.jboss.common.service.BasicMBeanSupport;
import aff.confirm.jboss.mail.MailNotifier;
import org.jboss.logging.Logger;

import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.ejb.Singleton;
import javax.ejb.Startup;
import javax.jms.*;
import javax.management.MalformedObjectNameException;
import javax.management.ObjectName;
import javax.naming.*;
import java.io.IOException;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;

@Startup
@Singleton
public class DBInfo extends BasicMBeanSupport implements DBInfoMBean,MessageListener {

    public static SimpleDateFormat sdfmmddyyyyDate = new SimpleDateFormat("MM/dd/yyyy", Locale.US);
    private String jndiName;
    private String DBUserName = "";
    private String DBPassword = "";
    private String DBUrl = "";
    private String databaseName = "";
    private String oracleTNSPath = "";
    private boolean dbUp = false;

    private TopicSession ts;
    private TopicConnection tc;
    private String topicName;
    private TopicSubscriber tsub;

    private ObjectName objectName;

    public DBInfo() {
        super("affinity.utils:service=DBInfo");
    }

    @PostConstruct
    public void postConstruct() throws Exception {
        super.postConstruct();
    }

    @PreDestroy
    public void preDestroy() throws Exception {
        super.preDestroy();
    }

    public boolean isDbUp() {
        return dbUp;
    }

    public String getOracleTNSPath() {
        return oracleTNSPath;
    }

    public void setOracleTNSPath(String pOracleTNSPath) {
        this.oracleTNSPath = pOracleTNSPath;
    }

    public void setTopicName(String topicName) {
        this.topicName = topicName;
    }

    public javax.management.ObjectName getTopic() throws MalformedObjectNameException {
        if((topicName != null) && (topicName.length() > 0))
            return new ObjectName("jboss.mq.destination:service=Topic,name="+topicName);
        else
            return null;
    }

    private void init() throws JMSException, NamingException, IOException {
        Logger.getLogger(this.getClass()).info("Executing Init...");
        if(topicName!=null){
            if(oracleTNSPath != null){
                //TODO:RN parsing a TNS Names files is not allowed
               IDBPropertyParser parser = new OracleTNSParser();
               parser.parse(oracleTNSPath);
               Logger.getLogger(this.getClass()).info("using " + oracleTNSPath);
               DBUrl = parser.getNewJDBCURL(databaseName,"jdbc:oracle:thin:@00.00.00.00:1521:prod");
               Logger.getLogger(this.getClass()).info(databaseName + ": url :" + DBUrl);
            }
            InitialContext ic = new InitialContext();
            ConnectionFactory topicFactory = JndiUtil.lookup("java:/ConnectionFactory");
            tc = (TopicConnection) topicFactory.createConnection();
            ts = tc.createTopicSession(false, Session.AUTO_ACKNOWLEDGE);

            Topic topic = JndiUtil.lookup("topic/"+topicName);
            tsub = ts.createSubscriber(topic);
            tsub.setMessageListener(this);
            tc.start();
            Logger.getLogger(this.getClass()).info("started on Topic: " + topicName);
         }
    }

    private java.sql.Connection getConnection(){
        java.sql.Connection con = null;
        try {
            con = DriverManager.getConnection(DBUrl,DBUserName,DBPassword);
            dbUp = true;
        } catch (SQLException e) {
            log.error( "ERROR", e );
            dbUp = false;
        }
        return con;
    }

    private void close(){
        try {
            if(tsub!=null){
                tsub.close();
                tsub = null;
            }
        } catch (JMSException e) {
            log.error(e);
        }
        try {
            if(tc != null){
                tc.close();
                tc = null;
            }
        } catch (JMSException e) {
            log.error(e);
        }
    }

    public String getJndiName() {
        return jndiName;
    }

    public void setJndiName(String jndiName) throws NamingException {
       String oldName = this.jndiName;
       this.jndiName = jndiName;
       if( getState() == STARTED )
       {
           unbind(oldName);
           try
           {
               rebind();
           }
           catch(Exception e)
           {
               NamingException ne = new NamingException("Failed to update jndiName");
               ne.setRootCause(e);
               throw ne;
           }
        }
    }

    public void startService() throws Exception
    {
        rebind();
        Thread.sleep(15 * 1000);
        init();
        //TODO MSSQL MIGRATION
        //        refreshBusnDt();
    }

    public void stopService() {
        try {
            close();
            unbind(jndiName);
        } catch (Exception e) {
            log.error(e);
        }
    }

    private static Context createContext(Context rootContext, Name name) throws NamingException {
        Context subctx = rootContext;
        for (int n = 0; n < name.size(); n++) {
            String atom = name.get(n);
            try {
                Object obj = subctx.lookup(atom);
                subctx = (Context) obj;
            } catch (NamingException e) {    // No binding exists, create a subcontext
                subctx = subctx.createSubcontext(atom);
            }
        }

        return subctx;
    }

    private void rebind() throws NamingException {
        InitialContext rootCtx = new InitialContext();
        // Get the parent context into which we are to bind
        Name fullName = rootCtx.getNameParser("").parse(jndiName);
        Name parentName = fullName;
        if (fullName.size() > 1)
            parentName = fullName.getPrefix(fullName.size() - 1);
        else
            parentName = new CompositeName();
        Context parentCtx = createContext(rootCtx, parentName);
        Name atomName = fullName.getSuffix(fullName.size() - 1);
        String atom = atomName.get(0);
        //parentCtx.bind(atom,this);
        //NonSerializableFactory.rebind(parentCtx, atom, this);
    }

    private void unbind(String jndiName) {
        try {
            Context rootCtx = new InitialContext();
            rootCtx.unbind(jndiName);
//         NonSerializableFactory.unbind(jndiName);
        } catch (NamingException e) {
            log.error(e);
        }
    }

    public String getDBUserName() {
            return DBUserName;
    }

    public void setDBUserName(String value) {
        DBUserName = value;
    }

    public String getDBPassword() {
        return DBPassword;
    }

    public void setDBPassword(String value) {
        DBPassword = value;
    }

    public String getDBUrl() {
        return DBUrl;
    }

    public void setDBUrl(String value) {
        DBUrl = value;
    }

    public String getDatabaseName() {
       return databaseName;
    }

   public void setDatabaseName(String value) {
       if( getState() == STARTED )
      {
         if (!value.equals(databaseName)){
             databaseName = value;
         }
      }else
           databaseName = value;
   }

   public void refreshBusnDt() throws SQLException {
//TODO MSSQL MIGRATION
//      java.sql.Connection con = null;
      try{
//          con = getConnection();
//          if((con!=null) && (DBUrl.indexOf("oracle")>0)){
//            busnDate = getLastUpdateBusnDate(con);
//          }
      }finally{
//       if(con != null)
//          con.close();
//       con = null;
      }

   }

   public Date getLastUpdateBusnDate(java.sql.Connection con) throws SQLException {
      Date result = null;
      Statement stmnt = null;
      try{
          log.info(databaseName);
          String sql = "select update_busn_dt from infinity_mgr.trade_audit where id = (select max(id) from infinity_mgr.trade_audit)";
          stmnt = con.createStatement();
          ResultSet rs = stmnt.executeQuery(sql);
          if(rs.next()){
            result = rs.getDate(1);
          }

      } finally{
          stmnt.close();
          stmnt = null;
      }
      return result;
   }

    public void onMessage(Message message) {
        try {
            String dbName = message.getStringProperty("SOURCE_DB");
            if(dbName.equals(databaseName)){
                String action = message.getStringProperty("ACTION");
                if(action.equals("STARTUP")){
                    dbUp = true;
                    log.info("DB "+databaseName+" STARTUP");
                    String msg = "ORACLE_STARTUP";
                //    sendNotification(new Notification(msg,msg,getNextNotificationSequenceNumber(),new Date().getTime()));
                    emailNotify("DB "+databaseName+" STARTUP");
                }
                else
                  if(action.equals("SHUTDOWN")){
                     dbUp = false;
                     log.info("DB "+databaseName+" SHUTDOWN");
                     String msg = "ORACLE_SHUTDOWN";
                 //    sendNotification(new Notification(msg,msg,getNextNotificationSequenceNumber(),new Date().getTime()));
                     emailNotify("DB "+databaseName+" SHUTDOWN");
                  }
            }

        } catch (JMSException e) {
            log.error(e);
        }
    }

    public Reference getReference() throws NamingException {
        return new Reference("sempra.jboss.queueservice.DBInfo", new StringRefAddr("name", jndiName), "sempra.jboss.queueservice.DBInfoFactory", null);
    }

    private void emailNotify(String message){
       MailNotifier mn = null;
       try {
           mn = JndiUtil.lookup("MailNotifier");
           mn.sendMailToGroup(jndiName+" notification",message,"admin");
       } catch (Exception e) {
           log.error(e);
       }
   }
}
