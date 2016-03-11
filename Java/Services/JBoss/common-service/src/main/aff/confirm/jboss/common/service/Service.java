/**
 * User: islepini
 * Date: Jul 29, 2003
 * Time: 1:28:11 PM
 */
package aff.confirm.jboss.common.service;

import aff.confirm.jboss.common.exceptions.SQLConnectionAllocationFailure;
import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.util.DbInfoWrapper;
import org.jboss.logging.Logger;

import javax.jms.JMSException;
import javax.jms.Message;
import javax.management.MalformedObjectNameException;
import javax.management.Notification;
import javax.management.ObjectName;
import javax.naming.NamingException;
import java.io.File;
import java.io.IOException;
import java.io.RandomAccessFile;
import java.sql.*;
import java.text.DecimalFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Enumeration;
import java.util.Locale;


abstract public class Service extends BasicMBeanSupport implements ServiceMBean {
    public SimpleDateFormat sdfDate = new SimpleDateFormat("yyyy-MM-dd", Locale.US);
    public SimpleDateFormat sdfDateTime = new SimpleDateFormat("MM/dd/yyyy HH:mm:ss", Locale.US);
    public SimpleDateFormat sdfTime = new SimpleDateFormat("HH:mm:ss", Locale.US);
    public DecimalFormat df = new DecimalFormat("#0");
    public String propSection = "[PROPERTIES]";
    public String bodySection = "[BODY]";
    protected volatile boolean stoppingService = false;
    RandomAccessFile lastProcessedIDLogger;
    String lastProcessedIDFileName = System.getProperty("jboss.home.dir")+"/server/default/data/sempra/"+this.getClass().getName().substring(this.getClass().getName().lastIndexOf(".")+1)+"_LastID.log";

    private long lastProcessedID = 0;
    protected boolean started;
    protected boolean notificationIgnore = false;
    protected Logger log = Logger.getLogger(this.getClass());

    public Service(String objectNameStr) {
        super(objectNameStr);
    }

    public boolean isNotificationIgnore() {
        return notificationIgnore;
    }

    public void setNotificationIgnore(boolean notificationIgnore) {
        this.notificationIgnore = notificationIgnore;
    }

    public long getLastProcessedID() {
        return lastProcessedID;
    }

    public String getDbInfoName() {
        return dbInfoName;
    }

    public void setDbInfoName(String dbInfoName) {
        this.dbInfoName = dbInfoName;
    }

    public ObjectName getDbInfo() throws MalformedObjectNameException {
       if((dbInfoName != null) && (dbInfoName.length() > 0))
            return new ObjectName("sempra.utils:service="+dbInfoName);
        else
            return null;
    }

    private String dbInfoName;
    private java.sql.Connection dbInfoConnection;

    public String getNotifyGroupName() {
        return notifyGroupName;
    }

    public void setNotifyGroupName(String notifyGroupName) {
        this.notifyGroupName = notifyGroupName;
    }

    private String notifyGroupName = "admin";
    public void notifyEmailGroup(String subject,String content) {
        /*
        InitialContext ic = null;
        try{
            ic = new InitialContext();
            ((MailNotifier)ic.lookup("MailNotifier")).sendMailToGroup(subject,content,notifyGroupName);
        } catch (NamingException e) {
            log.error(e);
        }finally{
            if(ic !=  null){
                try {
                    ic.close();
                } catch (NamingException e) {
                    log.error(e);
                    ic = null;
                }

            }
        }
        */
    }

    private void connectToDB() throws NamingException, SQLException {
        DbInfoWrapper dbinfo = new DbInfoWrapper(dbInfoName);
        dbInfoConnection = DriverManager.getConnection (dbinfo.getDBUrl(),dbinfo.getDBUserName(),dbinfo.getDBPassword());
    }

    public java.sql.Connection getSqlConnection() throws SQLException, NamingException {
        if(dbInfoName != null){
            if(dbInfoConnection == null){
                connectToDB();
            }
            return dbInfoConnection;
        }else

            return null;

    }

    public void setSqlConnection(java.sql.Connection connection) throws SQLException, NamingException {
       dbInfoConnection = connection;
    }

    private Statement createStatement(boolean prepared, String sql)throws SQLConnectionAllocationFailure{
      if(dbInfoName != null){
          Statement stmnt = null;
          int counter = 3;
          SQLException lastException = null;
          while ((counter != 0) && (stmnt == null)){
              try{
                  counter--;
                  if(prepared)
                    stmnt = getSqlConnection().prepareStatement(sql);
                  else
                    stmnt = getSqlConnection().createStatement();
              }catch(SQLException e){
                  dbInfoConnection = null;
                  lastException = e;
                  log.warn(e+" Reconnecting in 5 sec: "+counter);
                  try {
                      Thread.sleep(5000);
                  } catch (InterruptedException e1) {
                      log.error(e);
                  }
              } catch (Exception e) {
                  log.error(e);
                  break;
              }
          }
          if(stmnt == null){
              throw new SQLConnectionAllocationFailure(lastException.getMessage());
          }
          return stmnt;
      }else return null;
    }

    public Statement createStatement()throws SQLException, SQLConnectionAllocationFailure{
      return createStatement(false,null);
    }

    public PreparedStatement createPreparedStatement(String sql)throws SQLException, SQLConnectionAllocationFailure{
      return (PreparedStatement)createStatement(true,sql);
    }

    final protected void stopService()
    {
        try {
            onInternalServiceStoping();
        } catch (Exception e) {
            log.error(e);
        }

        if(dbInfoConnection != null){
            try {
                dbInfoConnection.close();
            } catch (SQLException e) {
                log.error(e);
            }
            dbInfoConnection = null;

        }
        if(lastProcessedIDLogger != null){
            try {
                lastProcessedIDLogger.close();
            } catch (IOException e) {
                log.error(e);
            }
            lastProcessedIDLogger = null;
        }

        try {
            setStopingService(true);
            super.stopService();
        } catch (Exception e) {
            log.error(e);
        }finally{
            setStopingService(false);
        }

    }

    public void copyMessage(Message source, Message dest) throws JMSException {
        Enumeration props = source.getPropertyNames();
        while( props.hasMoreElements()){
            String propName = props.nextElement().toString();
			if (!"JMSXDeliveryCount".equalsIgnoreCase(propName)) {
				if (propName.indexOf("JMS_JBOSS") >= 0 ) {
					dest.setObjectProperty(propName,source.getObjectProperty(propName));
				}
				else {
					dest.setStringProperty(propName,source.getStringProperty(propName));
				}
			}
        }
    }

    protected void notifyEmailGroupServiceStoped(String content) {
        notifyEmailGroup(this.getName()+" notification, service stoped, use start to restart",content);
    }

    protected void notifyEmailGroupServiceFailedToProcess(String content){
        notifyEmailGroup(this.getName()+" notification, service failed to process message(task)",content);
    }

    protected void createLastProcessedIDFile() throws IOException {
        File file = new File(lastProcessedIDFileName);
        if (!file.exists())
            file.createNewFile();
        log.info("lastProcessedIDLogger=" + lastProcessedIDLogger);
        lastProcessedIDLogger = new RandomAccessFile(lastProcessedIDFileName,"rw");
    }

    public void setLastProcessedID(long lastProcessedID) throws StopServiceException {
        if(this.lastProcessedID != lastProcessedID){
            this.lastProcessedID = lastProcessedID;
            try {
                if(lastProcessedIDLogger == null)
                    createLastProcessedIDFile();
                lastProcessedIDLogger.seek(0);
                lastProcessedIDLogger.writeBytes(Long.toString(lastProcessedID));
            } catch (IOException e) {
                throw new StopServiceException(e.getMessage());
            }
        }
    }

    public long readLastProcessedID() throws StopServiceException {
        try {
            if(lastProcessedIDLogger == null)
                createLastProcessedIDFile();
            lastProcessedIDLogger.seek(0);
            String temp = lastProcessedIDLogger.readLine();
            return new Long(temp).longValue();
        } catch (IOException e) {
            throw new StopServiceException(e.getMessage());
        }
    }

    public void resultSetToMessage(ResultSet rs,Message msg) throws Exception, JMSException {
      ResultSetMetaData rsmd = rs.getMetaData();
      for(int i = 1; i <= rsmd.getColumnCount() ; i++){
           int type = rsmd.getColumnType(i);
           switch(type){
           case java.sql.Types.TIMESTAMP:
           case java.sql.Types.DATE:
               if(rs.getTimestamp(i) != null)
                msg.setStringProperty(rsmd.getColumnName(i),sdfDateTime.format(rs.getTimestamp(i)));
               else
                msg.setStringProperty(rsmd.getColumnName(i),"");
               break;
           case java.sql.Types.TIME:
               if(rs.getTime(i) != null)
                msg.setStringProperty(rsmd.getColumnName(i),sdfTime.format(rs.getTime((i))));
               else
                msg.setStringProperty(rsmd.getColumnName(i),"");
               break;
           case java.sql.Types.DOUBLE:
           case java.sql.Types.NUMERIC:
               msg.setDoubleProperty(rsmd.getColumnName(i),rs.getDouble(i));
               break;
           case java.sql.Types.INTEGER:
           case java.sql.Types.SMALLINT:
           case java.sql.Types.TINYINT:
               msg.setIntProperty(rsmd.getColumnName(i),rs.getInt(i));
               break;
           case java.sql.Types.CHAR:
           case java.sql.Types.LONGVARCHAR:
           case java.sql.Types.VARCHAR:
               msg.setStringProperty(rsmd.getColumnName(i),nullCheck(rs.getString(i)));
               break;
           case java.sql.Types.DECIMAL:
               msg.setDoubleProperty(rsmd.getColumnName(i),rs.getDouble(i));
               break;
           case java.sql.Types.FLOAT:
               msg.setFloatProperty(rsmd.getColumnName(i),rs.getFloat(i));
               break;
           case java.sql.Types.NULL:
               msg.setStringProperty(rsmd.getColumnName(i),"");
               break;
           default:
              throw new Exception("Error. Data Type not implemented: "+type+".");

           }
      }
    }

    static private String nullCheck(String source){
       if (source == null)
        return "";
       else
        return source;
    }

    public String messageToString(Message msg){
        StringBuffer result = new StringBuffer();
        try{
            result.append(propSection+"\n");
            Enumeration props = msg.getPropertyNames();
            while( props.hasMoreElements()){
                String propName = props.nextElement().toString();
                result.append(propName+"="+msg.getStringProperty(propName)+"\n");
            }
            result.append(bodySection+"\n");
            result.append(msg.toString());
        } catch (Exception e) {
           log.error(e);
        }
        return result.toString();
    }

    public void setStatementString(int pParamIndex, String pFieldName, PreparedStatement statement, Message pMessage)
        throws JMSException, SQLException {
        if (pMessage.propertyExists(pFieldName)){
            statement.setString(pParamIndex, pMessage.getStringProperty(pFieldName));
        }
        else {
            statement.setNull(pParamIndex, Types.VARCHAR );
        }
    }

    public void setStatementDouble(int pParamIndex, String pFieldName, PreparedStatement statement, Message pMessage)
        throws JMSException, SQLException {
        if (pMessage.propertyExists(pFieldName)){
            statement.setDouble(pParamIndex, pMessage.getDoubleProperty(pFieldName));
        }
        else {
            statement.setNull(pParamIndex, Types.DOUBLE );
        }
    }

    public void setStatementDateTime(int pParamIndex, String pFieldName, PreparedStatement statement, Message pMessage)
        throws JMSException, SQLException, ParseException {
        if ( pMessage.propertyExists(pFieldName) &&
             (pMessage.getStringProperty(pFieldName).length() > 0)) {
            Date trdDt = sdfDateTime.parse(pMessage.getStringProperty(pFieldName));
            statement.setDate(pParamIndex, new java.sql.Date(trdDt.getTime()));
        }
        else {
            statement.setNull(pParamIndex, Types.DATE );
        }
    }

    public void handleNotification(Notification notification, Object o){
        if(notificationIgnore){
            log.info("Notification "+notification.getType().equals("ORACLE_STARTUP")+" ignored");
            return;
        }
        if((this.getStateString().equals("Started"))
           && notification.getType().equals("ORACLE_STARTUP")){
           log.info("Message ORACLE_STARTUP, Service State = "+this.getStateString()+", message ignored");
           return;
        }
        if((this.getStateString().equals("Stopped"))
           && notification.getType().equals("ORACLE_SHUTDOWN")){
           log.info("Message ORACLE_SHUTDOWN, Service State = "+this.getStateString()+", message ignored");
           return;
        }

        if(notification.getType().equals("ORACLE_STARTUP")){
            try {
                start();
            } catch (Exception e) {
               notifyEmailGroupServiceStoped("Failed to restart on ORACLE_STARTUP message"+e);
            }
        }else if(notification.getType().equals("ORACLE_SHUTDOWN")){
            try {
                stop();

            } catch (Exception e) {

            }
        }else
            log.error("Unknown notification: "+notification.getMessage());
    }

    final protected void startService() throws Exception {
        super.startService();
        try{
            onInternalServiceStarting();
            stoppingService = false;
            startProcessing();
        }
        catch(Exception e){
            onServiceFailedToStart(e);
            throw new Exception(e);
        }
    }

    protected void onServiceFailedToStart(Exception e){
        notifyEmailGroup(this.getName()+" notification, service failed to start",e.getMessage());
    }

    abstract public void startProcessing() throws Exception;

    abstract protected void onInternalServiceStarting() throws Exception;

    abstract protected void onInternalServiceStoping();

    synchronized public void setStopingService(boolean value){
        stoppingService = value;
    }

    synchronized public boolean getStopingService(){
        return stoppingService;
    }

}
