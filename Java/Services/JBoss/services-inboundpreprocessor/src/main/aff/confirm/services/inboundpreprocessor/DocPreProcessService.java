package aff.confirm.services.inboundpreprocessor;

import aff.confirm.common.util.MBeanUtil;
import org.jboss.logging.Logger;

import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.ejb.Singleton;
import javax.ejb.Startup;
import javax.management.ObjectName;
import javax.naming.NamingException;
import javax.mail.MessagingException;

import aff.confirm.jboss.common.service.taskservice.TaskService;
import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.common.dao.AppControlDAO;
import aff.confirm.common.util.MailUtils;

import java.sql.Connection;
import java.sql.SQLException;
import java.net.InetAddress;
import java.io.UnsupportedEncodingException;
import java.util.Vector;
import java.util.Calendar;

/**
 * User: srajaman
 * Date: Mar 6, 2007
 * Time: 11:22:45 AM
 */
@Startup
@Singleton
public class DocPreProcessService extends TaskService implements DocPreProcessServiceMBean {
    static Logger log = Logger.getLogger( DocPreProcessService.class );
    
    private static final String  _DEFAULT_EMAIL_ADDRESS = "confirmsupport@sempratrading.com";
    private String scanDirectory;
    private String smtpHost;
    private String smtpPort;
    private boolean isArchive;
    private String  archiveDir;
    private String devEmailAddress = null;
    private String emailAddress = null;
    private String sentFromName;
    private String sentFromAddress;
    private String sendToName;
    private Connection conn;
    private String errorDir;
    private String tifDir;
    private InBoundDocProcessor docProcessor ;
    private Integer tradeSearchDuration = 60;
    private boolean logMessageExecutedOnce = false;
    private boolean runNow = true;
    private java.util.Date lastDbConnection = null;
    private Calendar calc = Calendar.getInstance();

    private String searchPattern;
    private String serviceName;
    private String environment;

    public DocPreProcessService() {
       super("affinity.inbound:service=DocPreProcessor");
    }

    @PostConstruct
    public void postConstruct() throws Exception {
        super.postConstruct();
    }

    @PreDestroy
    public void preDestroy() throws Exception {
        super.preDestroy();
    }


    public String getSearchPattern() {
        return searchPattern;
    }

    public void setTradeSearchDuration(Integer tradeSearch) {
         tradeSearchDuration = tradeSearch;
    }

    public Integer getTradeSearchDuration() {
        return tradeSearchDuration;
    }

    public void setSearchPattern(String searchPattern) {
        this.searchPattern = searchPattern;
    }

    public String getScanDirectory() {
        return scanDirectory;
    }

    public void setScanDirectory(String scanDir) {
        scanDirectory  = scanDir;
    }

    public boolean getArchive() {
        return isArchive;
    }

    public void setArchive(boolean archive) {
      isArchive = archive;
    }

    public void setSmtpHost(String smtpHost) {
        this.smtpHost = smtpHost;
    }

    public String getSmtpHost() {
        return this.smtpHost;
    }

    public String getSmtpPort() {
        return this.smtpPort;
    }

    public void setSmtpPort(String port) {
        this.smtpPort = port;
    }

    public void setArchiveDirectory(String dir) {
         this.archiveDir = dir;
    }

    public String getArchiveDirectory() {
           return this.archiveDir;
    }

    public void setErrorDirectory(String dir) {
        errorDir = dir;
    }

    public String getErrorDirectory() {
        return errorDir;
    }

     public void setTifDirectory(String tifDir) {
      this.tifDir = tifDir;
    }

    public String getTifDirectory() {
        return tifDir;
    }

    protected void onServiceStarting() throws Exception {
        init();
    }

    private void init() throws Exception {
        log.info("Init is executing...");
        docProcessor = new InBoundDocProcessor();
        log.info("InboundDocProcessor created.");
        runNow = true;
        checkToReconnect(true);
        initMailValues();
        log.info("EMail values initialized.");
        loadEmailAddress();
        log.info("EMail addresses loaded.");
        serviceName = this.getName();
             //   this.getServiceName().getCanonicalName();
        log.info("Service name has been assigned: " + serviceName);
        log.info("Init is complete.");
    }

    protected void onServiceStoping() {
        close();
    }
    private void close() {
        if (conn != null) {
            try {
                conn.close();
            } catch (Exception e) {
            }
            finally {
                conn = null;
                lastDbConnection = null;
            }
        }
    }

    public void  processDirectoryNow() throws StopServiceException{
        runScanning();
    }


    public String getEnv() {
        return environment;
    }


    public void setEnv(String pEnv) {
        environment = pEnv;
    }

    private void  runScanning() throws StopServiceException {
        if ( runNow == false ) {
            return;
        }
        runNow = false;

        if (!logMessageExecutedOnce)
            log.info("First Run Task has been called for " + serviceName + "...");
        try {

            Vector errorList= docProcessor.processDirectory(conn,getScanDirectory(),getArchiveDirectory(),errorDir,tifDir,getArchive(),searchPattern,tradeSearchDuration.intValue());

            if (!logMessageExecutedOnce){
                log.info("First Run Task has completed for " + serviceName + ".");
                logMessageExecutedOnce = true;
            }
            int errorCount = errorList.size();
            if (errorCount > 0) {
                for (int i=0; i<errorCount;++i){
                    String fileName= (String) errorList.get(i);
                    notifyEmailGroup("Inbound File Processing Error",
                            " The file name " + fileName + " is not found in the InBound_Docs table");
                }
            }

        } catch (SQLException e) {
             notifyEmailGroup("Inbound File Pre Processing Error (" + serviceName + ")","The Doc preprocessor service has been stopped. \n" + e.getMessage() );
            throw new StopServiceException(e.getMessage());
        } catch (Exception e) {
            notifyEmailGroup("Inbound File Pre Processing Error (" + serviceName + ")","The Doc preprocessor service has been stopped. \n" + e.getMessage());
            throw new StopServiceException(e.getMessage());
        }
        finally {
            runNow = true;
        }
    }
    protected void runTask() throws StopServiceException {
//        checkToReconnect(false);
        runScanning();
    }
    private void initMailValues() throws Exception{

        log.info("Loading Mail Info from Database.");
        String hostName = InetAddress.getLocalHost().getHostName().toUpperCase();
        sentFromName = "InBoundDocuments_" + hostName;
        sentFromAddress = "JBossOn" + hostName + "@sempratrading.com";
        sendToName = "InBoundDocuments_Recipients";
        sentFromAddress = "JBossOn" + hostName + "@sempratrading.com";

    }
    private void loadEmailAddress() {
        AppControlDAO appControlDAO;
        log.info("Before Dev Email Address = " + devEmailAddress + ", Email Address = " + emailAddress);
        if (devEmailAddress == null && emailAddress == null) {
            try {
                appControlDAO = new AppControlDAO(getSqlConnection(),"INBND");
                devEmailAddress = appControlDAO.getValue("EMAIL_ADDRESS_TEST");
                emailAddress = appControlDAO.getValue("EMAIL_ADDRESS");
                log.info("After Dev Email Address = " + devEmailAddress + ", Email Address = " + emailAddress);
            }
            catch (NamingException e) {
                log.info(e.getStackTrace());
            }
            catch (SQLException e) {
                log.info(e.getStackTrace());
            }
            catch ( Exception e) {
                log.info(e.getStackTrace());
            }
        }
    }
    public void notifyEmailGroup(String subject,String content) {

        log.info("Email Notification is called");
        String toAddress;
        String dbName = getDbInfoName();
        if (content == null) { content = "" ;}
        toAddress = environment.equalsIgnoreCase("PROD")?emailAddress:devEmailAddress;
        toAddress = environment.equalsIgnoreCase("PROD") && "None".equalsIgnoreCase(toAddress)?_DEFAULT_EMAIL_ADDRESS:toAddress;
        if (toAddress == null || "".equals(toAddress)){
            log.info("To address is null");
            return ;
        }
        MailUtils mail = new MailUtils(smtpHost,smtpPort);
        try {
              mail.sendMail(toAddress,sendToName,sentFromAddress,sentFromName,subject,content,"");
         } catch (MessagingException e) {
            log.error("ERROR", e);
        } catch (UnsupportedEncodingException e) {
            log.error( "ERROR", e );
        }
    }
    private void checkToReconnect(boolean newConnection) throws StopServiceException {

        boolean reInitialize  = false;
        if (newConnection == true) {
            reInitialize  =  true;
        }
        else {
            java.util.Date dateNow = new java.util.Date();
            if (lastDbConnection == null) {
                reInitialize  = true;
            }
            else {
                calc.setTime(dateNow);
                int dayNow = calc.get(Calendar.DATE);
                calc.setTime(lastDbConnection);
                int lastRefreshDay = calc.get(Calendar.DATE);
                if (dayNow != lastRefreshDay){
                    reInitialize = true;
                }
            }
        }
        if  (reInitialize){
            log.info("The connection will be reinitialized for the service.");
            close();
            try {
                conn = getSqlConnection();
                lastDbConnection = new java.util.Date();
            } catch (Exception e) {
                throw new StopServiceException(e.getMessage());
            }
        }
    }
}
