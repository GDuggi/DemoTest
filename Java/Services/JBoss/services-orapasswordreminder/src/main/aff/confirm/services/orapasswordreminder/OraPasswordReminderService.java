package aff.confirm.services.orapasswordreminder;

import aff.confirm.common.util.DateUtils;
import aff.confirm.common.util.MBeanUtil;
import aff.confirm.common.util.MailUtils;
import aff.confirm.jboss.common.exceptions.LogException;
import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.service.taskservice.TaskService;
import com.sun.rowset.CachedRowSetImpl;
import oracle.jdbc.OracleCallableStatement;
import org.jboss.logging.Logger;

import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.ejb.Singleton;
import javax.ejb.Startup;
import javax.mail.MessagingException;
import javax.management.ObjectName;
import javax.naming.NamingException;
import javax.sql.rowset.CachedRowSet;
import java.io.*;
import java.net.InetAddress;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.text.DecimalFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;
import java.util.Properties;

@Startup
@Singleton
public class OraPasswordReminderService extends TaskService implements OraPasswordReminderServiceMBean {
    static Logger log = Logger.getLogger( OraPasswordReminderService.class );

    private final SimpleDateFormat sdfOracleDate = new SimpleDateFormat("MM/dd/yyyy");
    private final SimpleDateFormat sdfPropFileTS = new SimpleDateFormat("yyyy-MM-dd HHmm");

    private final String CONFIG_FILE_NAME = "OraPasswordReminder.properties";
    private final String PROCESSED_DT_PROP_NAME = "ProcessedDateTime";
    private DecimalFormat df = new DecimalFormat("#0");
    private MailUtils mailUtils;
    private String smtpHost;
    private String smtpPort;
    private String sentFromAddress;
    private boolean isProduction;
    private String isProductionStr = "";
    private boolean initConfigFileToYesterday;
    private String initConfigFileToYesterdayStr = "";
    private String runAtTimeOfDay = "23";
    private String dbInfoDisplayName;
    private String leadDaysStr = "";
    private int leadDays = 0;
    private String oraPasswordResetUrl = "";
    private String notifyEmailSentFromName;
    private String notifyEmailDomainName = "";
    private String notifyEmailSubject = "";
    private String notifyEmailBodyText = "";
    private String deploymentVerifyEmailAddress = "";
    private String deploymentVerifyEmailName = "";
    private String jbossServerConfigFileDirAndName = "";

    public OraPasswordReminderService() {
       super("objectName=affinity.confirm.infrastructure:service=OraPasswordReminder");
    }

    @PostConstruct
    public void postConstruct() throws Exception {
        super.postConstruct();
    }

    @PreDestroy
    public void preDestroy() throws Exception {
        super.preDestroy();
    }

    public String getSmtpPort() {
        return smtpPort;
    }

    public void setSmtpPort(String smtpPort) {
        this.smtpPort = smtpPort;
    }

    public String getSmtpHost() {
        return smtpHost;
    }

    public void setSmtpHost(String smtpHost) {
        this.smtpHost = smtpHost;
    }

    public String getLeadDays() {
        return leadDaysStr;
    }

    public void setLeadDays(String pLeadDays) {
        this.leadDaysStr = pLeadDays;
    }

    public String getRunAtTimeOfDay() {
        return runAtTimeOfDay;
    }

    public void setRunAtTimeOfDay(String pRunAtTimeOfDay) {
        this.runAtTimeOfDay = pRunAtTimeOfDay;
    }

    public String getOraPasswordResetUrl() {
        return oraPasswordResetUrl;
    }

    public void setOraPasswordResetUrl(String pOraPasswordResetUrl) {
        this.oraPasswordResetUrl = pOraPasswordResetUrl;
    }

    public String getNotifyEmailDomainName() {
        return notifyEmailDomainName;
    }

    public void setNotifyEmailDomainName(String pNotifyEmailDomainName) {
        this.notifyEmailDomainName = pNotifyEmailDomainName;
    }

    public String getNotifyEmailSentFromName() {
        return notifyEmailSentFromName;
    }

    public void setNotifyEmailSentFromName(String pNotifyEmailSentFromName) {
        this.notifyEmailSentFromName = pNotifyEmailSentFromName;
    }

    public String getDeploymentVerifyEmailAddress() {
        return deploymentVerifyEmailAddress;
    }

    public void setDeploymentVerifyEmailAddress(String pDeploymentVerifyEmailAddress) {
        this.deploymentVerifyEmailAddress = pDeploymentVerifyEmailAddress;
    }

    public String getDeploymentVerifyEmailName() {
        return deploymentVerifyEmailName;
    }

    public void setDeploymentVerifyEmailName(String pDeploymentVerifyEmailName) {
        this.deploymentVerifyEmailName = pDeploymentVerifyEmailName;
    }

    public String getNotifyEmailSubject() {
        return notifyEmailSubject;
    }

    public void setNotifyEmailSubject(String pNotifyEmailSubject) {
        this.notifyEmailSubject = pNotifyEmailSubject;
    }

    public String getNotifyEmailBodyText() {
        return notifyEmailBodyText;
    }

    public void setNotifyEmailBodyText(String pNotifyEmailBodyText) {
        this.notifyEmailBodyText = pNotifyEmailBodyText;
    }

    public String getIsProduction() {
        return isProductionStr;
    }

    public void setIsProduction(String pIsProduction) {
        this.isProductionStr = pIsProduction;
    }

    public String getInitConfigFileToYesterday() {
        return initConfigFileToYesterdayStr;
    }

    public void setInitConfigFileToYesterday(String pInitConfigFileToYesterday) {
       this.initConfigFileToYesterdayStr = pInitConfigFileToYesterday;
    }

    protected void onServiceStarting() throws Exception {
        log.info("Executing startService... ");//getSqlConnection().setAutoCommit(false);
        init();
    }

    protected void onServiceStoping() {
        try {
            close();
        } catch (Exception e) {
            log.error(e);
        }
    }

    public void close() throws Exception {
    }

    private void init() throws Exception {
        try {
            log.info("Executing init... ");
            mailUtils = new MailUtils(smtpHost, smtpPort);

            String text = "";
            text = "Timer interval = " + (getTimerPeriod() / 1000 / 60) + " minutes.";
            log.info(text);

            //This code differs from JBoss 4.02.03 version
            dbInfoDisplayName = super.getDbInfoName();
            log.info("Connected to " + dbInfoDisplayName + ".");

            leadDays = Integer.parseInt(leadDaysStr);
            isProduction = isProductionStr.equalsIgnoreCase("true");
            initConfigFileToYesterday = initConfigFileToYesterdayStr.equalsIgnoreCase("true");
            //This code differs from JBoss 4.02.03 version. System variable slightly different
//            jbossServerConfigFileDirAndName = System.getProperty("jboss.home.dir") + "\\data\\" + CONFIG_FILE_NAME;
            jbossServerConfigFileDirAndName = System.getProperty("jboss.server.data.dir") + "\\" + CONFIG_FILE_NAME;

            initMailVariables();
            log.info("LeadDays=" + leadDays);
            log.info("sentFromAddress=" + sentFromAddress);
            log.info("RunAtTimeOfDay=" + runAtTimeOfDay);
            log.info("NotifySentFromName=" + notifyEmailSentFromName);
            log.info("NotifyEmailDomainName=" + notifyEmailDomainName);
            log.info("NotifyEmailSubject=" + notifyEmailSubject);
            log.info("NotifyEmailBodyText=" + notifyEmailBodyText);
            log.info("OraPasswordResetUrl=" + oraPasswordResetUrl);
            log.info("DeploymentVerifyEmailAddress=" + deploymentVerifyEmailAddress);
            log.info("DeploymentVerifyEmailName=" + deploymentVerifyEmailName);
            log.info("jbossServerConfigFileDirAndName=" + jbossServerConfigFileDirAndName);

            if (isProduction)
                log.info("IsProduction=TRUE");
            else
                log.info("IsProduction=FALSE");

            if (initConfigFileToYesterday)
                log.info("InitConfigFileToYesterday=TRUE");
            else
                log.info("InitConfigFileToYesterday=FALSE");

            if (initConfigFileToYesterday)
                setConfigFileToYesterday();

            log.info("Init OK.");
        } catch (Exception e) {
            log.error("Failed to Init Service", e);
        }
    }

    private void setConfigFileToYesterday() throws StopServiceException {
        Calendar lastRunDtCal = DateUtils.getCalendarFromDateTime(DateUtils.getDateTimeStamp());
        lastRunDtCal.add(Calendar.DATE, -1);
        try {
            savePropChanges(PROCESSED_DT_PROP_NAME, sdfPropFileTS.format(lastRunDtCal.getTime()));
            log.info("Config file was created. Last processed Date/Time was set to: " +
                    sdfPropFileTS.format(lastRunDtCal.getTime()));
        } catch (Exception e) {
            throw new StopServiceException(e.getMessage());
        }
    }

    private void initMailVariables() throws Exception {
        String hostName = InetAddress.getLocalHost().getHostName().toUpperCase();
        //sentFromName = "OraPasswordReminder_" + hostName;
        sentFromAddress = "JBoss_" + hostName + '@' + notifyEmailDomainName;
    }

    synchronized public void executeTimerEventNow() throws StopServiceException, LogException {
        log.info("executeTimerEventNow() executing...");
        poll();
        log.info("executeTimerEventNow() done.");
    }

    protected void runTask() throws StopServiceException, LogException {
        poll();
    }

    synchronized private void poll() throws StopServiceException{
        try {
            log.info("Executing polling task...");
            Date currentDt = new Date();
            Calendar currentDtCal = Calendar.getInstance();
            currentDtCal.setTime(currentDt);

            //Has it been run yet for today
            Date lastProcessedDt = getLastProcessedDate();
            Calendar lastProcessedDtCal = new GregorianCalendar();
            lastProcessedDtCal.setTime(lastProcessedDt);

            //Check to see if last run Date is earlier than current date, handling the year change.
            boolean isDateOk = false;
            isDateOk = (currentDtCal.get(Calendar.YEAR) == lastProcessedDtCal.get(Calendar.YEAR) &&
                        currentDtCal.get(Calendar.DAY_OF_YEAR) > lastProcessedDtCal.get(Calendar.DAY_OF_YEAR)) ||
                       (currentDtCal.get(Calendar.YEAR) > lastProcessedDtCal.get(Calendar.YEAR));

            //Check to see if we have reached the right hour to run it.
            if (isDateOk) {
                Calendar runAtTimeOfDayCal = DateUtils.getCalendarFromDateTime(DateUtils.getDateTimeStamp());
                setRunAtTimeOfDayValue(runAtTimeOfDayCal);
                if (currentDtCal.get(Calendar.HOUR_OF_DAY) >= runAtTimeOfDayCal.get(Calendar.HOUR_OF_DAY) &&
                    currentDtCal.get(Calendar.MINUTE) >= runAtTimeOfDayCal.get(Calendar.MINUTE)) {
                    processExpiringPasswords();
                    savePropChanges(PROCESSED_DT_PROP_NAME, sdfPropFileTS.format(currentDtCal.getTime()));
                    log.info("Notification has been processed for today.");
                }
            }
            log.info("Execute polling task done.");
        } catch (Exception e) {
            log.error(e);
            try {
                getSqlConnection().rollback();
            } catch (Exception e1) {
                log.error(e1);
            }
            throw new StopServiceException(e.getMessage());
        }
    }

    private void setRunAtTimeOfDayValue(Calendar runAtTimeOfDayCal) {
        String hour =  runAtTimeOfDay.substring(0,2);
        String mins = runAtTimeOfDay.substring(3,5);
        runAtTimeOfDayCal.set(Calendar.HOUR_OF_DAY, Integer.valueOf(hour) );
        runAtTimeOfDayCal.set(Calendar.MINUTE, Integer.valueOf(mins) );
    }

    private Date getLastProcessedDate() throws ParseException {
        Date lastProcessedDt = new Date();
        Properties props = new Properties();
        InputStream inputStream = null;

        // First try loading from the current directory
        try {
            File propFile = new File(jbossServerConfigFileDirAndName);
            inputStream = new FileInputStream( propFile );
        }
        catch ( Exception e ) { inputStream = null; }

        try {
            if ( inputStream == null ) {
                // Try loading from classpath
                inputStream = OraPasswordReminderService.class.getResourceAsStream(jbossServerConfigFileDirAndName);
            }
            // Try loading properties from the file (if found)
            props.load( inputStream );
        }
        catch ( Exception e ) { }

        Date currentDt = new Date();
        String lastProcessedDtStr = props.getProperty(PROCESSED_DT_PROP_NAME, sdfPropFileTS.format(currentDt));
        lastProcessedDt = sdfPropFileTS.parse(lastProcessedDtStr);

        return lastProcessedDt;
    }

    private void savePropChanges(String pPropName, String pValue) throws Exception {
        Properties props;
        File propsFile;
        try {
            props = new Properties();
            props.setProperty(pPropName, pValue);
            propsFile = new File(jbossServerConfigFileDirAndName);
            OutputStream outputStream = new FileOutputStream(propsFile);
            props.store(outputStream, "Updated by OraPasswordReminder Service");
        } catch (IOException e) {
            throw new Exception("IOException occurred in savePropChanges: " + e.getMessage());
        }
    }

    private void processExpiringPasswords() throws SQLException, NamingException,
            MessagingException, UnsupportedEncodingException, ParseException {
        Date current = new Date();
        String passwordDt = sdfOracleDate.format(current);
        String isDataFound = "";
        CachedRowSet crs = new CachedRowSetImpl();
        isDataFound = getExpiringPasswords(passwordDt, leadDays, crs);
        if (isDataFound.equalsIgnoreCase("OK")){
            crs.beforeFirst();
            String userName;
            String expiryDate;
            while (crs.next()) {
                userName = crs.getString("username");
                expiryDate = crs.getString("expiry_date");
                sendNotifyEMail(userName, expiryDate);
                log.info("userName=" + userName + ", expiryDate=" + expiryDate);
                try {
                    //second email sends a blank body.
                    Thread.sleep(2000);
                } catch (InterruptedException e) {
                    log.error(e.getMessage());
                }
            }
        } else {
            log.info("No data was found.");
            if (!isProduction) {
                sendNotifyEMail("NoUsersCurrentlyExpire",passwordDt);
                log.info("Deployment verification email was sent.");
            }
        }
    }

    private String getExpiringPasswords(String pWarnDt, int pLeadDays, CachedRowSet pCrs ) throws SQLException, NamingException {
        OracleCallableStatement statement = null;
        String callSqlStatement = "{? = call dbadm.pkg_password_utils.f_get_expiring_passwords(?,?,?)}";
        statement = (OracleCallableStatement) getSqlConnection().prepareCall(callSqlStatement);
        statement.registerOutParameter(1, oracle.jdbc.OracleTypes.VARCHAR);
        statement.setString(2, pWarnDt);
        statement.setInt(3, pLeadDays);
        statement.registerOutParameter(4, oracle.jdbc.OracleTypes.CURSOR);
        statement.executeQuery();
        String isDataFound;
        isDataFound = statement.getString(1);
        if (isDataFound.equalsIgnoreCase("OK")){
            //CachedRowSet crs = null;
            //crs = new CachedRowSet();
            ResultSet rs = null;
            rs = statement.getCursor(4);
            pCrs.populate(rs);
            statement.close();
            statement = null;
            rs.close();
            rs = null;
        }
        return isDataFound;
    }

    private void sendNotifyEMail(String pUserName, String pExpiryDate)
            throws MessagingException, UnsupportedEncodingException, ParseException {
        final SimpleDateFormat sdfDisplayDate = new SimpleDateFormat("dd-MMM-yyyy");
        String subject;
        String mailText;
        String deploymentVerifyMailText;
        String passwordExpiresText;
        String passwordResetText;
        String displayDate = "";
        String emailAddress = "";
        String sendToName = "";
        Date tempDt = new Date();
        tempDt = sdfOracleDate.parse(pExpiryDate);
        displayDate = sdfDisplayDate.format(tempDt);

        passwordExpiresText = "The password for " + pUserName + " on " + dbInfoDisplayName + " expires on " + displayDate + ". ";
        passwordResetText = "Please use the following link to enter a new password: " +
                "\n" + oraPasswordResetUrl + "?" + pUserName.toLowerCase();
        subject = notifyEmailSubject;
        mailText = passwordExpiresText + "\n" + notifyEmailBodyText + "\n" + passwordResetText;

        if (isProduction) {
            emailAddress = pUserName + "@" + notifyEmailDomainName;
            sendToName = pUserName;
        } else {
            emailAddress = deploymentVerifyEmailAddress;
            sendToName = deploymentVerifyEmailName;
            deploymentVerifyMailText = "THIS IS A TEST. It has been sent only to the 'DeploymentVerifyEmailAddress' assigned" +
                    " in the service config file and is intended to verify OraPasswordReminderService settings and operation." +
                    " Change the config file 'IsProduction' setting to 'true' and messages will go out directly" +
                    " to each user as required. This text will read as follows:";
            mailText = deploymentVerifyMailText + "\n----------\n" + mailText;
        }

        mailUtils.sendMail(emailAddress, sendToName, sentFromAddress, notifyEmailSentFromName,
                subject, mailText, "");
    }

}
