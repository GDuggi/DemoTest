package aff.confirm.opsmanager.creditmargin;

import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.service.Service;
import aff.confirm.jboss.common.util.DbInfoWrapper;
import aff.confirm.opsmanager.creditmargin.common.CreditMarginMessageProcessor;
import aff.confirm.opsmanager.creditmargin.common.CreditMarginProcessor;
import aff.confirm.opsmanager.creditmargin.common.NotifyUtil;
import aff.confirm.opsmanager.creditmargin.common.TradeRqmtDAO;
import org.jboss.logging.Logger;

import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.ejb.Singleton;
import javax.ejb.Startup;
import javax.jms.JMSException;
import javax.jms.Message;
import javax.jms.MessageListener;
import javax.naming.NamingException;
import javax.swing.text.BadLocationException;
import java.io.IOException;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

/**
 * User: srajaman
 * Date: Dec 3, 2008
 * Time: 10:55:55 AM
 */
@Startup
@Singleton
public class CreditMarginService extends Service implements CreditMarginServiceMBean, MessageListener {
    private String tibcoServer;
    private String tibcoUser;
    private String tibcoPwd;
    private String tibcoQueueName;
    private String emailAddr;

    private String smtpHost;
    private String smtpPort;

    private String creditStatusUrl;
    private String creditMarginUrl;
    private String creditMarginToken;
    private String  traderUrl;
    private String vaultEJBName;

    private Connection affinityConnection;
    private String affinityDisplayName ;
    private String dbUserName;
    private String dbPwd;
    private TradeRqmtDAO rqmtDao ;
    private CreditMarginMessageProcessor cmmr;
    private CreditMarginProcessor cmr;

    public CreditMarginService() {
        super("affinity.confirm:service=CreditMargin");
    }

    @PostConstruct
    public void postConstruct() throws Exception {
        super.postConstruct();
    }

    @PreDestroy
    public void preDestroy() throws Exception {
        super.preDestroy();
    }

    public void setSmtpHost(String host) {
        smtpHost = host;
    }

    public String getSmtpHost() {
        return smtpHost;
    }

    public void setSmtpPort(String port) {
        smtpPort = port;
    }

    public String getSmtpPort() {
        return smtpPort;
    }

    public void setCreditStatusUrl(String statusUrl) {
       this.creditStatusUrl = statusUrl;
    }

    public String getCreditStatusUrl() {
        return this.creditStatusUrl;
    }

    public void setCreditMarginUrl(String marginUrl) {
        this.creditMarginUrl = marginUrl;
    }

    public String getCreditMarginUrl() {
        return this.creditMarginUrl;
    }

    public void setCreditMarginToken(String marginToken) {
        this.creditMarginToken  = marginToken;
    }

    public String getCreditMarginToken() {
        return this.creditMarginToken;
    }

    public void setTraderUrl(String traderUrl) {
        this.traderUrl = traderUrl;
    }

    public String getTraderUrl() {
        return this.traderUrl; 
    }

    public void setVaultEJBName(String ejbName) {
        this.vaultEJBName = ejbName;
    }

    public String getVaultEJBName() {
        return this.vaultEJBName;
    }

    public void setDbUserName(String userName) {
       dbUserName = userName;
    }

    public String getDbUserName() {
        return dbUserName;
    }

    public void setDbpwd(String pwd) {
       this.dbPwd = pwd;
    }

    public String getDbpwd() {
        return  this.dbPwd;  
    }


    public String getTibcoServerName() {
        return tibcoServer;
    }

    public void setTibcoServerName(String serverName) {
        this.tibcoServer = serverName;
    }

    public String getUser() {
        return tibcoUser;
    }

    public void setUser(String userId) {
        tibcoUser = userId;
    }

    public String getPwd() {
        return tibcoPwd;
    }

    public void setPwd(String pwd) {
           tibcoPwd = pwd;
    }

    public String getQueueName() {
        return tibcoQueueName;
    }

    public void setQueueName(String queueName) {
        this.tibcoQueueName = queueName;
    }

    public String getNotifyAddr() {
        return emailAddr;
    }

    public void setNotifyAddr(String emailAddr) {
        this.emailAddr = emailAddr;
    }

    public void startProcessing() throws Exception {
    }

    protected void onInternalServiceStarting() throws Exception {
       init();
    }

    private void init() throws Exception{

        try {
            log.info("Credit Margin is starting....");
            NotifyUtil.smtpHost = this.smtpHost;
            NotifyUtil.smtpPort = this.smtpPort;
            getDbConnection();
            rqmtDao = new TradeRqmtDAO(this.affinityConnection);
            cmr = new CreditMarginProcessor(rqmtDao,this.creditStatusUrl,this.creditMarginUrl,this.creditMarginToken,this.traderUrl,this.vaultEJBName,this.dbUserName,this.affinityDisplayName);
            cmmr = new CreditMarginMessageProcessor(this.tibcoServer,this.tibcoUser,this.tibcoPwd,this.tibcoQueueName,this.affinityDisplayName,cmr);
            cmmr.startProcessing(this);
        } catch (SQLException e) {
            log.error("Error during startup :", e);
            NotifyUtil.sendMail(emailAddr,"Credit Margin Startup Error ","The Credit Margin has NOT been started : " + e.getMessage());
            throw new StopServiceException(e.getMessage());
        } catch (NamingException e) {
            log.error("Error during startup :", e);
            NotifyUtil.sendMail(emailAddr,"Credit Margin Startup Error ","The Credit Margin has NOT been started : " + e.getMessage());
            throw new StopServiceException(e.getMessage());
        } catch (JMSException e) {
            log.error("Error during startup :" , e );
            NotifyUtil.sendMail(emailAddr,"Credit Margin Startup Error ","The Credit Margin has NOT been started : " + e.getMessage());
            throw new StopServiceException(e.getMessage());
        }
    }

    private void getDbConnection() throws SQLException, NamingException {

        log.info("Creating DbConnection....");

        DbInfoWrapper dbInfo = new DbInfoWrapper(this.getDbInfoName());
        this.affinityDisplayName  = dbInfo.getDatabaseName();
        String dbName =  dbInfo.getDBUrl();
        String userid = this.dbUserName; // dbInfo.getDBUserName();
        String pwd = this.dbPwd; //dbInfo.getDBPassword();
        this.affinityConnection= DriverManager.getConnection(dbName,userid,pwd);
        log.info("Created DbConnection sucessfully....");
        
    }

    protected void onInternalServiceStoping() {

        log.info("Stopping is called..");
        closeDbConnection();
        if ( cmmr != null) {
            try {
                cmmr.stopProcessing();
            }
            catch (Exception e){
                log.error(e.getMessage());
            }
            finally {
                cmmr = null;
            }
        }
    }

    private void closeDbConnection() {

        try {
            if (this.affinityConnection != null){
                this.affinityConnection.close();
            }
        }
        catch (Exception e) {

        }
        finally {
            this.affinityConnection = null;
        }
    }


    public void onMessage(Message message){

        if (!this.getStopingService()) {
            try {
                cmmr.processMessage(message);
                message.acknowledge();
            } /*
          catch (JMSException e) {
             log.error("Error during processing :" , e );
             NotifyUtil.sendMail(emailAddr,"Credit Margin Processing Error ","The Credit Margin has been stopped for processing " + message +  "; Message: " + e.getMessage());
             stop();
             //onInternalServiceStoping();
          } catch (NamingException e) {
              log.error("Error during processing :" , e );
              NotifyUtil.sendMail(emailAddr,"Credit Margin Processing Error ","The Credit Margin has been stopped for processing " + message +  "; Message: " + e.getMessage());
              stop();
              //onInternalServiceStoping();
          } catch (IOException e) {
              log.error("Error during processing :" , e );
              NotifyUtil.sendMail(emailAddr,"Credit Margin Processing Error ","The Credit Margin has been stopped for processing " + message +  "; Message: " + e.getMessage());
              stop();
             // onInternalServiceStoping();
          } catch (BadLocationException e) {
              log.error("Error during processing :" , e );
              NotifyUtil.sendMail(emailAddr,"Credit Margin Processing Error ","The Credit Margin has been stopped for processing " + message +  "; Message: " + e.getMessage());
              stop();
              //onInternalServiceStoping();
          } catch (SQLException e) {
              log.error("Error during processing :" , e );
              NotifyUtil.sendMail(emailAddr,"Credit Margin Processing Error ","The Credit Margin has been stopped for processing " + message +  "; Message: " + e.getMessage());
              stop();
              //onInternalServiceStoping();
          }  */
            catch (Exception e){
                log.error("Error during processing :" , e );
                NotifyUtil.sendMail(emailAddr,"Credit Margin Processing Error ","The Credit Margin has been stopped for processing " + message +  "; Message: " + e.getMessage());
                try {
                   stop();
                }
                catch (Exception ef){
                
                }
              
            }
        }

    }

    public String applyCreditMargin(String tradeSystem, long tradeId, int version) {
        String returnStr = "OK";
        log.info("applyCreditMargin is called ..." );
        try {
            cmr.processCreditMarginNotification(tradeSystem,tradeId,version,"ApplyCreditMargin");
        } catch (IOException e) {
            returnStr = e.getMessage();
            log.error("Error during processing :" , e );
        } catch (NamingException e) {
            returnStr = e.getMessage();
            log.error("Error during processing :" , e );
        } catch (BadLocationException e) {
            returnStr = e.getMessage();
            log.error("Error during processing :" , e );
        } catch (SQLException e) {
            returnStr = e.getMessage();
            log.error("Error during processing :" , e );
        }
        return returnStr;
    }
   
}
