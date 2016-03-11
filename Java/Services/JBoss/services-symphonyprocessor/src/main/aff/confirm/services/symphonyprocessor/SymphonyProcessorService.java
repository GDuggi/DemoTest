/*
 * User: islepini
 * Date: Oct 2, 2002
 * Time: 9:30:52 AM
 * To change template for new class use 
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.services.symphonyprocessor;

import aff.confirm.common.util.JndiUtil;
import aff.confirm.common.util.MailUtils;
import org.apache.commons.httpclient.HttpClient;
import org.apache.commons.httpclient.methods.GetMethod;
import org.w3c.dom.Document;
import org.xml.sax.InputSource;
import aff.confirm.jboss.common.service.taskservice.TaskService;
import aff.confirm.jboss.common.Sender;
import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.dbinfo.DBInfo;
//import aff.confirm.jboss.common.util.*;
import aff.confirm.common.util.XMLUtils;

import java.io.*;
import java.net.InetAddress;
import java.text.SimpleDateFormat;
import java.text.ParseException;
import java.util.*;
import java.util.Date;
import java.sql.*;
import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.management.MalformedObjectNameException;
import javax.management.ObjectName;
import javax.jms.*;
import javax.jms.Queue;
import javax.naming.*;

import org.jboss.logging.Logger;
import org.w3c.dom.NodeList;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;


public class SymphonyProcessorService extends TaskService implements SymphonyProcessorServiceMBean {


    class FileComparator implements Comparator{
        public int compare(Object o1, Object o2) {
            long modified1 = ((File)o1).lastModified();
            long modified2 = ((File)o1).lastModified();
            if(modified1 == modified2)
                return 0;
            else{
                if (modified1 < modified2)
                    return -1;
                else
                    return 1;
            }
    }
}

    public final SimpleDateFormat sdfDate = new SimpleDateFormat("MM/dd/yyyy");
    public final SimpleDateFormat sdfTime = new SimpleDateFormat("HH:mm:ss");
    public final SimpleDateFormat sdfDateTime = new SimpleDateFormat("MM/dd/yyyy HH:mm:ss");

    public final SimpleDateFormat sdfmm_dd_yyyyDate = new SimpleDateFormat("MM-dd-yyyy", Locale.US);
    public final SimpleDateFormat sdfSybaseDate = new SimpleDateFormat("MM/dd/yyyy", Locale.US);
    public final SimpleDateFormat sdfDateTimeTs = new SimpleDateFormat("yyyyddMM_hhmmss_SSSSSS", Locale.US);
    //public final String webServiceDateFormat = "yyyy-MM-dd'T'HH:mm:ss";
    public final String webServiceDateFormat = "yyyy-MM-dd";
//    public final String webServiceDateFormat = "MMMM dd yyyy HH:mmaaa";

    private final String IN_PROGRESS_DIR = "\\InProgress";
    private final String FAILED_DIR = "\\Failed";
    private File inProgressDir = null;
    private File failedDir = null;
    private String processedFileDir;
    private QueueConnection queueConnection;
    private QueueSession queueSession;
    private QueueSender senderSTA;
    private String STAQueueName;
    private String fileDropDir;
    private String scanningMode = "running";
    private int counter = 0;
    private boolean preserveXMLEnabled;
    //private String appFeedFolder;
//    private String symphonyDBInfoName;
//    private java.sql.Connection symphonyConnection;
    private String opsTrackingDBInfoName;
    private java.sql.Connection opsTrackingConnection;
    private String tradeMessageWebServiceURL;
    private String tradeMessageRootTagName;
    private enum SymProcMethodType {ENTER, VOID}
    private String stopServiceNotifyAddress;
    private boolean ignoreNonCriticalBlankFields;
    private String smtpHost;
    private String smtpPort;
    private MailUtils mailUtils;
    private String emailReturnAddress;

    public SymphonyProcessorService() {
        super("affinity.cwf:service=SymphonyProcessor");
    }

    @PostConstruct
    public void postConstruct() throws Exception {
        super.postConstruct();
    }

    @PreDestroy
    public void preDestroy() throws Exception {
        super.preDestroy();
    }

    public String getFileDropDir() {
        return fileDropDir;
    }

    public void setFileDropDir(String pFileDropDir) {
        this.fileDropDir = pFileDropDir;
    }

    public String getProcessedFileDir() {
        return processedFileDir;
    }

    public void setProcessedFileDir(String pProcessedFileDir) {
        this.processedFileDir = pProcessedFileDir;
    }

    public void setSTAQueueName(String pSTAQueueName) {
        this.STAQueueName = pSTAQueueName;
    }

    public javax.management.ObjectName getSTAQueue() throws MalformedObjectNameException {
        if(STAQueueName.length() > 0)
            return new ObjectName("jboss.mq.destination:service=Queue,name="+STAQueueName);
        else
            return null;
    }

/*
    public void setMSSqlDBInfoName(String pSymphonyDBInfoName) {
        this.symphonyDBInfoName = pSymphonyDBInfoName;
    }
*/

/*
    public ObjectName getMSSqlDBInfo() throws MalformedObjectNameException {
        if (symphonyDBInfoName.length() > 0)
            return new ObjectName("sempra.utils:service=" + symphonyDBInfoName);
        else
            return null;
    }
*/

    public void setOpsTrackingDBInfoName(String pOpsTrackingDBInfoName) {
        this.opsTrackingDBInfoName = pOpsTrackingDBInfoName;
    }

    public ObjectName getOpsTrackingDBInfo() throws MalformedObjectNameException {
        if (opsTrackingDBInfoName.length() > 0)
            return new ObjectName("sempra.utils:service=" + opsTrackingDBInfoName);
        else
            return null;
    }

    public void setTradeMessageWebServiceURL(String pTradeMessageWebServiceURL) {
        this.tradeMessageWebServiceURL = pTradeMessageWebServiceURL;
    }

    public String getTradeMessageWebServiceURL(){
        return tradeMessageWebServiceURL;
    }

    public void setTradeMessageRootTagName(String pTradeMessageRootTagName) {
        this.tradeMessageRootTagName = pTradeMessageRootTagName;
    }

    public String getTradeMessageRootTagName() {
        return tradeMessageRootTagName;
    }

    public String getScanningMode() {
        return scanningMode;
    }

    public void setScanningMode(String pScanningMode) {
        this.scanningMode = pScanningMode;
    }

    public void setPreserveXMLEnabled(boolean pPreserveXMLEnabled) {
        this.preserveXMLEnabled = pPreserveXMLEnabled;
    }

    public boolean getPreserveXMLEnabled(){
        return preserveXMLEnabled;
    }

    public void setIgnoreNonCriticalBlankFields(boolean pIgnoreCriticalBlankFields) {
        this.ignoreNonCriticalBlankFields = pIgnoreCriticalBlankFields;
    }

    public boolean getIgnoreNonCriticalBlankFields() {
        return ignoreNonCriticalBlankFields;
    }

    public void setStopServiceNotifyAddress(String pStopServiceNotifyAddress) {
        this.stopServiceNotifyAddress = pStopServiceNotifyAddress;
    }

    public String getSmtpHost() {
        return smtpHost;
    }

    public void setSmtpHost(String smtpHost) {
        this.smtpHost = smtpHost;
    }

    public String getSmtpPort() {
        return smtpPort;
    }

    public void setSmtpPort(String smtpPort) {
        this.smtpPort = smtpPort;
    }

    public String getStopServiceNotifyAddress() {
        return stopServiceNotifyAddress;
    }

    public void processFromTradeIDList(String pTradeIDList){

        StringTokenizer st = new StringTokenizer(pTradeIDList, ",");
        while(st.hasMoreTokens()){
            String nextToken = st.nextToken();
            try {
                double tradeID = Double.parseDouble(nextToken);
                Message message = getDataFromDataBase(tradeID);
                message.setStringProperty("DATA_FILE_NAME","recovery");
                senderSTA.send(message);
            } catch (Exception e) {
                log.error(e);
            }
        }
    }

    public void init() throws Exception {
//        Logger.getLogger(SymphonyProcessorService.class).info("Connecting symphonyConnection to " + symphonyDBInfoName + "...");
//        symphonyConnection = getMSSqlConnection();
//        Logger.getLogger(SymphonyProcessorService.class).info("Connected symphonyConnection to " + symphonyDBInfoName + ".");

        Logger.getLogger(SymphonyProcessorService.class).info("Connecting opsTrackingConnection to " + opsTrackingDBInfoName + "...");
        opsTrackingConnection = getOracleConnection(opsTrackingDBInfoName, opsTrackingConnection);
        Logger.getLogger(SymphonyProcessorService.class).info("Connected opsTrackingConnection to " + opsTrackingDBInfoName + ".");

        Logger.getLogger(SymphonyProcessorService.class).info("FileDropDir: " +  fileDropDir + ".");
        Logger.getLogger(SymphonyProcessorService.class).info("ProcessedFileDir: " +  processedFileDir + ".");

        Logger.getLogger(SymphonyProcessorService.class).info("TradeMessageWebServiceURL: " + tradeMessageWebServiceURL + ".");
        Logger.getLogger(SymphonyProcessorService.class).info("TradeMessageRootTagName: " + tradeMessageRootTagName + ".");

        mailUtils = new MailUtils(smtpHost, smtpPort);
        String hostName = InetAddress.getLocalHost().getHostName().toUpperCase();
        emailReturnAddress = "JBossOn_" + hostName + "@amphorainc.com";

        try{
            ConnectionFactory cf = JndiUtil.lookup("java:/ConnectionFactory");
            queueConnection = ((QueueConnectionFactory)cf).createQueueConnection();
            queueSession = queueConnection.createQueueSession(false, Session.CLIENT_ACKNOWLEDGE);
            Queue staQueue = JndiUtil.lookup("queue/"+STAQueueName);
            senderSTA = queueSession.createSender(staQueue);
            //initDealsheetProcessDir(processedFileDir);
        }finally{
        }
    }

   /* private void connectToLogDB(InitialContext pIc) throws Exception, SQLException {
        DBInfo logdbinfo = (DBInfo)pIc.lookup(logDbInfoName);
        if(logdbinfo!=null)
            logDbInfoConnection = DriverManager.getConnection (logdbinfo.getDBUrl(),logdbinfo.getDBUserName(),logdbinfo.getDBPassword());
        else throw new Exception("Failed to locate "+logDbInfoName);
   }   */

   protected void runTask() throws StopServiceException{
       if(scanningMode.equals("running")){
            scan();
       }
   }

    protected void onServiceStarting() throws Exception {
        log.info("Starting SymphonyProcessor...");

//       if((scanningMode != null) && (!scanningMode.equals("running"))){
//            notifyEmailGroup("SymphonyProcessor Notification","Service Started with scanning mode set to 'stopped' mode, use startScanning");
//       }
       init();
       log.info("SymphonyProcessor started.");
    }

    protected void onServiceStoping() {
        try {
            close();
        } catch (Exception e) {
            log.error(e);
        }
    }

    private void initDealsheetProcessDir(String pDealsheetProcessDir) {
       /* inProgressDir = new File(pDealsheetProcessDir + IN_PROGRESS_DIR);
        if (!inProgressDir.isDirectory() )
            inProgressDir.mkdir();
        failedDir = new File(pDealsheetProcessDir + FAILED_DIR);
        if (!failedDir.isDirectory() )
            failedDir.mkdir();    */
    }

    private String getAuditTypeFromName(String pFileName) throws Exception {
        String updateType = null;
        if (pFileName.toUpperCase().endsWith(".XML"))
            updateType = pFileName.substring(pFileName.length() - 7, pFileName.length() - 4);
        else
            updateType = pFileName.substring(pFileName.length() - 8, pFileName.length() - 5);

        if (updateType.equals("INS"))
            return "NEW";
        else if (updateType.equals("UPD"))
            return "EDIT";
        else if (updateType.equals("DEL"))
            return "VOID";
        else
            throw new Exception("Failed to parse tradeAuditType from name: "+pFileName);
    }

    private void scan() throws StopServiceException {
        if(counter>0){
            counter--;
            return;
        }
        //log.info("scanning "+dealsheetScanDir);
        File scannedDir = new File(fileDropDir);
        try{
            if(scannedDir.isDirectory()){
                //log.info("is directory");
                File [] scanFiles = scannedDir.listFiles();
                List tempList = new LinkedList();
                if(scanFiles != null){
                    //log.info("scanFiles "+scanFiles.length);
                    for (int i = 0; i < scanFiles.length; i++) {
                        File scanFile = scanFiles[i];
                        if (scanFile.isFile())
                            tempList.add(scanFile);
                    }
                    Collections.sort(tempList, new FileComparator());
                    for (int i = 0; i < tempList.size(); i++) {
                        File file = (File) tempList.get(i);
                        //log.info(new Date(file.lastModified()));
                        if(file.isFile() && file.getName().toUpperCase().endsWith(".XML")){
                            try {
                                  onNewFile(file.getAbsolutePath());
                            } catch (Exception e){
                                  log.error("file = "+file);
                                  //log.error(e);
//                                if (e instanceof StopServiceException) {
//                                    try {
//                                        String errorMessage = e.getMessage();
//                                        if (errorMessage == null)
//                                            errorMessage = "No additional error message was generated.";
//                                        mailUtils.sendMail(stopServiceNotifyAddress, "StopServiceRecipient",
//                                                emailReturnAddress, "SymphonyInterfaceProcessor",
//                                                "Symphony Oil Interface Has Stopped", errorMessage, "");
//                                    } catch (Exception e1) {
//                                        e1.printStackTrace();
//                                    }
                                    throw new StopServiceException(e.getMessage());
//                                }
                            }
                        }
                    }
                }
            } else
                  log.info("not a directory");
        }finally{
            scannedDir = null;
        }
    }

    public void onNewFile(String pDataSheetFileName) throws ParseException, SQLException, StopServiceException {
        try {
            Thread.sleep(2000);
        } catch (InterruptedException e) {
            log.error(e);
        }

        File datasheetFile = new File(pDataSheetFileName);
        File dealsheetFile = new File(pDataSheetFileName.toUpperCase().replaceAll(".XML",".html"));
        boolean filesOK = true;

        if(!datasheetFile.exists() ){
            filesOK = false;
            log.info("file not found: "+datasheetFile.getAbsolutePath());
        }
        if(!dealsheetFile.exists()){
            filesOK = false;
            log.info("file not found: "+dealsheetFile.getAbsolutePath());
        }

        if(filesOK){
            processFiles(datasheetFile, dealsheetFile);
        }else{
            File fileExisting = null;
            File fileMissing = null;
            if(dealsheetFile.exists()){
                fileExisting = dealsheetFile;
                fileMissing = datasheetFile;
            }else if(datasheetFile.exists()){
                fileExisting = datasheetFile;
                fileMissing = dealsheetFile;
            }

            log.error("Failed to locate file: "+fileMissing.getName()
                    +", File "+ " moved to "+fileExisting.getName()+failedDir.getAbsolutePath());
            File rename = new File(failedDir.getAbsolutePath() + '\\' + fileExisting.getName());
            fileExisting.renameTo(rename);

            BufferedWriter bw = null;
            try{
                bw = new BufferedWriter(new OutputStreamWriter(new DataOutputStream(new FileOutputStream(
                        failedDir.getAbsolutePath() + '\\'+fileExisting.getName().toUpperCase().replaceAll(".XML",".err")))));
                bw.write("file:"+fileMissing.getAbsolutePath()+" not found");
                bw.flush();
            }
            catch(Exception ex){
                log.error(ex);
            }
            finally{
                if(fileExisting != null)
                    fileExisting = null;
                try {
                    if(bw != null){
                        bw.close();
                        bw = null;
                    }
                } catch (IOException e) {
                    log.error(e);
                }
            }
            throw new StopServiceException("SymphonyProcessor Failure, file:"+fileMissing.getAbsolutePath()+" not found");
        }
    }

    private void processFiles(File pDatasheetFile, File pDealsheetFile) throws StopServiceException {
        double tradeID = 0;
        String auditTypeCode = null;
        try{
            DocumentBuilderFactory dbFactory = DocumentBuilderFactory.newInstance();
            DocumentBuilder dBuilder = dbFactory.newDocumentBuilder();
            Document doc = dBuilder.parse(pDatasheetFile);
            //doc.getDocumentElement().normalize();

            NodeList rootNodes = doc.getElementsByTagName("Trade");
            org.w3c.dom.Element elem = (org.w3c.dom.Element) rootNodes.item(0);
            String tradeNum = elem.getAttribute("TradeNum");
            tradeID = Double.parseDouble(tradeNum);

            Message message = null;
            log.info("Symphony Ticket: "+tradeNum);

            //message = getDataFromDataBase(tradeID);
            message = getDataFromWebServiceJBoss4(tradeID);
            if (message != null) {
                message.setStringProperty("DATA_FILE_NAME", pDatasheetFile.getName());
                double version = getLastVersion(tradeID);
                double fileDropId = updateDatabaseLog(tradeID,++version,pDatasheetFile);
                message.setDoubleProperty("SYMPHONY_FILE_DROP_ID",fileDropId);
                message.setDoubleProperty("VERSION",version);
                auditTypeCode = getAuditTypeFromName(pDatasheetFile.getName());
                message.setStringProperty("AUDIT_TYPE_CODE", auditTypeCode);
            }

            boolean success = false;
            try {
                success = pDealsheetFile.renameTo(new File(processedFileDir + "\\"  + pDealsheetFile.getName()));
                if (success) {
                    // File was successfully moved
                    log.info("Moved file: " + pDealsheetFile.getName());
                }
                else
                {
                    String tsNow = sdfDateTimeTs.format(new Date());
                    pDealsheetFile.renameTo(new File(processedFileDir + "\\" + pDealsheetFile.getName().replaceAll(".html","_" + tsNow + ".html")));
                    log.info("Renamed " + pDealsheetFile.getName() + " to " + pDealsheetFile.getName().replaceAll(".html","_" + tsNow + ".html"));
                }
            } catch (Exception e) {
                Logger.getLogger(SymphonyProcessorService.class, e.getMessage());
                //throw new LogException("failed to put dealsheet for ticket "+ticketID+", into "+appFeedFolder+", "+e);
            }

            if (message != null)
                senderSTA.send(message);

            opsTrackingConnection.commit();

            if (preserveXMLEnabled)
            {
                success = pDatasheetFile.renameTo(new File(processedFileDir + "\\"  + pDatasheetFile.getName()));
                if (!success)
                    pDatasheetFile.delete();
            }
            else
                pDatasheetFile.delete();

        }catch(Exception e){
            if (e.getClass().toString().equals("class java.sql.SQLException")){
                if(e.getMessage().indexOf("ORA-00001: unique constraint") == 0){
                    pDatasheetFile.delete();
                    pDealsheetFile.delete();
                }
            }else{
                throw new StopServiceException(e.getMessage());
            }
        }
    }

    synchronized private Message getDataFromWebServiceJBoss4(double pTradeID) throws Exception {
        Message message = null;
        Date currentTime = new Date();
        int tradeId = (int) pTradeID;
        String parmTrdSysCode = "SYM";
        String strTradeId =  String.valueOf(tradeId);
        String fieldValue = "";

//        TradeGateway tradeGatewayService = new TradeGatewayServiceLocator(tradeMessageWebServiceURL).getTradeGateway();
        String xmlResult = null;
//        xmlResult = tradeGatewayService.getTradeAlertMsg(parmTrdSysCode, strTradeId);


        DocumentBuilderFactory dbFactory = DocumentBuilderFactory.newInstance();
        DocumentBuilder dBuilder = dbFactory.newDocumentBuilder();
        Document doc = dBuilder.parse(new InputSource(new StringReader(xmlResult)));

        if (doc.getDocumentElement().getNodeName().equalsIgnoreCase("error")){
            String errorCode = doc.getElementsByTagName("code").item(0).getTextContent();
            String errorMessage = doc.getElementsByTagName("message").item(0).getTextContent();
            Logger.getLogger(SymphonyProcessorService.class).error("Error retrieving XML from WebService: Code=" + errorCode + ", \nMessage=" + errorMessage);
            Logger.getLogger(SymphonyProcessorService.class).error("Calling method parm: TradingSysCode=" + parmTrdSysCode + ", TradeId=" + strTradeId);
            Logger.getLogger(SymphonyProcessorService.class).warn("Update will be skipped for ticket: " + tradeId);
            //throw new StopServiceException("Error retrieving XML from WebService: Code=" + errorCode + ", Message=" + errorMessage);
        }
        else {
            message = queueSession.createMessage();
            message.setDoubleProperty("TRADE_AUDIT_ID",0);
            message.setDoubleProperty("PRMNT_TRADE_ID",pTradeID);
            message.setStringProperty("UPDATE_DATETIME", sdfDateTime.format(currentTime));
            message.setDoubleProperty("EMP_ID", 0);
            message.setStringProperty("UPDATE_TABLE_NAME", "N/A");
            message.setStringProperty("UPDATE_BUSN_DT",sdfDateTime.format(currentTime));

            setMessageStringProp("TRADE_TYPE_CODE", true, strTradeId, message, doc);
            setMessageStringProp("TRADE_STAT_CODE", false, strTradeId, message, doc);
            setMessageDateProp("TRADE_DT", true, strTradeId, message, doc);

            //Xml has different name from expected field name.
            fieldValue = doc.getElementsByTagName("booking_company_short_name").item(0).getTextContent();
            setMessageStringProp("CMPNY_SHORT_NAME", true, strTradeId, message, fieldValue);

            setMessageStringProp("CPTY_SHORT_NAME", true, strTradeId, message, doc);
            setMessageStringProp("CDTY_CODE", true, strTradeId, message, doc);
//            setMessageStringProp("CDTY_GRP_CODE", false, strTradeId, message, doc);
            setMessageStringProp("INST_CODE", true, strTradeId, message, doc);

            message.setStringProperty("BK_SHORT_NAME", "N/A");
            message.setStringProperty("BROKERSN", trimValue(doc.getElementsByTagName("broker_short_name").item(0).getTextContent()));
            message.setStringProperty("TRADING_SYSTEM","SYM");
            message.setStringProperty("NOTIFY_CONTRACTS_FLAG","Y");
            message.setStringProperty("RFRNCE_SHORT_NAME","N/A");
            //message.setStringProperty("EMP_NAME", trimValue(doc.getElementsByTagName("trade_mode_name").item(0).getTextContent()));
        }

        return  message;
    }

    private void setMessageStringProp(String pFieldName, boolean pIsFieldCritical, String pTradeId, Message message, Document doc) throws StopServiceException {
        final String tradingSystemInfo = "SymphonyOil: getTradeAlertOpsTrackXMLResponse";
        String xmlVal = "";
        boolean okToIgnoreIfBlank;
        okToIgnoreIfBlank = pIsFieldCritical ? false : ignoreNonCriticalBlankFields;

        try {
            xmlVal = doc.getElementsByTagName(pFieldName.toLowerCase()).item(0).getTextContent();
            if (xmlVal.length() > 0 || okToIgnoreIfBlank)
                message.setStringProperty(pFieldName.toUpperCase(), trimValue(xmlVal));
            else
                throw new Exception("BLANK FIELD");
        }
        catch (Exception e) {
            String javaException = "";
            if (!e.getMessage().contains("BLANK FIELD"))
                javaException = "\nJava Exception: " + e.getMessage();
            throw new StopServiceException("Stopped while reading XML file from: [" + tradingSystemInfo + "], TradeId: " +
                    pTradeId + ". There is no value for " + pFieldName + "." + javaException);
        }
    }

    //Overload to accommodate different xml field name from message field name
    private void setMessageStringProp(String pFieldName, boolean pIsFieldCritical, String pTradeId, Message message, String pXmlFieldValue) throws StopServiceException {
        final String tradingSystemInfo = "SymphonyOil: getTradeAlertOpsTrackXMLResponse";
        String xmlVal = "";
        boolean okToIgnoreIfBlank;
        okToIgnoreIfBlank = pIsFieldCritical ? false : ignoreNonCriticalBlankFields;

        try {
            xmlVal = pXmlFieldValue;
            if (xmlVal.length() > 0 || okToIgnoreIfBlank)
                message.setStringProperty(pFieldName.toUpperCase(), trimValue(xmlVal));
            else
                throw new Exception("BLANK FIELD");
        }
        catch (Exception e) {
            String javaException = "";
            if (!e.getMessage().contains("BLANK FIELD"))
                javaException = "\nJava Exception: " + e.getMessage();
            throw new StopServiceException("Stopped while reading XML file from: [" + tradingSystemInfo + "], TradeId: " +
                    pTradeId + ". There is no value for " + pFieldName + "." + javaException);
        }
    }

    private void setMessageDateProp(String pFieldName, boolean pIsFieldCritical, String pTradeId, Message message, Document doc) throws StopServiceException {
        final String tradingSystemInfo = "SymphonyOil: getTradeAlertOpsTrackXMLResponse";
        String xmlVal = "";
        Date tdDate;
        boolean okToIgnoreIfBlank;
        okToIgnoreIfBlank = pIsFieldCritical ? false : ignoreNonCriticalBlankFields;

        try {
            xmlVal = doc.getElementsByTagName(pFieldName.toLowerCase()).item(0).getTextContent();
            if (xmlVal.length() > 0 || okToIgnoreIfBlank) {
                tdDate = new SimpleDateFormat(webServiceDateFormat, Locale.US).parse(xmlVal);
                message.setStringProperty(pFieldName.toUpperCase(), sdfDate.format(tdDate));
            }
            else
                throw new Exception("BLANK FIELD");
        }
        catch (Exception e) {
            String javaException = "";
            if (!e.getMessage().contains("BLANK FIELD"))
                javaException = "\nJava Exception: " + e.getMessage();
            throw new StopServiceException("Stopped while reading XML file from: [" + tradingSystemInfo + "], TradeId: " +
                    pTradeId + ". There is no value for " + pFieldName + "." + javaException);
        }
    }

/*
    private double getTSDataNumberFieldFromXml(String pInputData, String pTradingSystemInfo, String strTradeId, String pFieldName) throws StopServiceException {
        try {
            double rtnVal = 0;
            rtnVal =  Double.parseDouble(pInputData);
            return rtnVal;
        }
        catch (NumberFormatException ne) {
            throw new StopServiceException("Reading [" + pTradingSystemInfo + "], TradeId: " + strTradeId + ". There is no value for " + pFieldName + ".");
        }
    }

    private java.sql.Date getTSDataDateFieldFromXml(String pInputData, TradingSystemDATA_rec pTsDataRec, String pTradingSystemInfo,
                                                    String strTradeId, String pFieldName) throws StopServiceException {
        try {
            java.sql.Date rtnVal = null;
            rtnVal = pTsDataRec.getJavaSqlDateFromXmlDate(pInputData, webServiceDateFormat);
            return rtnVal;
        }
        catch (Exception e) {
            throw new StopServiceException("Reading [" + pTradingSystemInfo + "], TradeId: " + strTradeId + ". There is no value for " + pFieldName + ".");
        }
    }*/


    synchronized private Message getDataFromWebServiceJBoss7(double pTradeID) throws Exception {
        Message message = null;
        Date currentTime = new Date();
        int tradeId = (int) pTradeID;

        String parmTrdSysCode = "?tradesyscode=SYM";
        String parmTradeId = "&ticket=" + tradeId;

        //IOpsDataFeedProxy stub = new IOpsDataFeedProxy(tradeMessageWebServiceURL);
        //String xmlResult = stub.getTradeAlertMessage(tradeId);

        String getMethodParm = tradeMessageWebServiceURL + parmTrdSysCode + parmTradeId;
        GetMethod method = new GetMethod(getMethodParm);
        HttpClient client = new HttpClient();
        client.executeMethod(method);
        String xmlResult = method.getResponseBodyAsString();

        DocumentBuilderFactory dbFactory = DocumentBuilderFactory.newInstance();
        DocumentBuilder dBuilder = dbFactory.newDocumentBuilder();
        Document doc = dBuilder.parse(new InputSource(new StringReader(xmlResult)));
        doc.getDocumentElement().normalize();

        if (doc.getDocumentElement().getNodeName().equalsIgnoreCase("error")){
            String errorCode = doc.getElementsByTagName("code").item(0).getTextContent();
            String errorMessage = doc.getElementsByTagName("message").item(0).getTextContent();
            Logger.getLogger(SymphonyProcessorService.class).error("Error retrieving XML from WebService: Code=" + errorCode + ", \nMessage=" + errorMessage);
            Logger.getLogger(SymphonyProcessorService.class).error("Calling method parm: " + getMethodParm);
            Logger.getLogger(SymphonyProcessorService.class).warn("Update will be skipped for ticket: " + tradeId);
            //throw new StopServiceException("Error retrieving XML from WebService: Code=" + errorCode + ", Message=" + errorMessage);
        }
        else {
            NodeList rootNodes = doc.getElementsByTagName(tradeMessageRootTagName);
            NodeList nodes = rootNodes.item(0).getChildNodes();

            message = queueSession.createMessage();
            message.setDoubleProperty("TRADE_AUDIT_ID",0);
            message.setDoubleProperty("PRMNT_TRADE_ID",pTradeID);
            message.setStringProperty("UPDATE_DATETIME",sdfDateTime.format(currentTime));
            message.setDoubleProperty("EMP_ID",0);
            message.setStringProperty("UPDATE_TABLE_NAME","N/A");
            message.setStringProperty("UPDATE_BUSN_DT",sdfDateTime.format(currentTime));
            message.setStringProperty("TRADE_TYPE_CODE", trimValue(doc.getElementsByTagName("trade_type_code").item(0).getTextContent()));
            message.setStringProperty("TRADE_STAT_CODE", trimValue(doc.getElementsByTagName("trade_stat_code").item(0).getTextContent()));

            String strDate = doc.getElementsByTagName("trade_dt").item(0).getTextContent();
            Date tdDate = new SimpleDateFormat(webServiceDateFormat, Locale.US).parse(strDate);
            message.setStringProperty("TRADE_DT",sdfDate.format(tdDate));

            message.setStringProperty("CMPNY_SHORT_NAME", trimValue(doc.getElementsByTagName("booking_company_short_name").item(0).getTextContent()) );
            message.setStringProperty("BK_SHORT_NAME", "N/A");
            message.setStringProperty("CPTY_SHORT_NAME", trimValue(doc.getElementsByTagName("cpty_short_name").item(0).getTextContent()));
            message.setStringProperty("CDTY_CODE", trimValue(doc.getElementsByTagName("cdty_code").item(0).getTextContent()));
            message.setStringProperty("BROKERSN", trimValue(doc.getElementsByTagName("broker_short_name").item(0).getTextContent()));
            message.setStringProperty("TRADING_SYSTEM","SYM");
            message.setStringProperty("INST_CODE", trimValue(doc.getElementsByTagName("inst_code").item(0).getTextContent()));
            message.setStringProperty("NOTIFY_CONTRACTS_FLAG","Y");
            message.setStringProperty("RFRNCE_SHORT_NAME","N/A");
            message.setStringProperty("EMP_NAME", trimValue(doc.getElementsByTagName("trade_mode_name").item(0).getTextContent()));
        }

        return  message;
    }

    synchronized private Message getDataFromDataBase(double pTradeID) throws Exception {
        int rowCount = 0;
        //Statement statement = null;
        PreparedStatement statement = null;
        ResultSet rs = null;
        //Calendar cal = Calendar.getInstance();
        //cal.add(Calendar.DATE,1);
        //java.util.Date utilDate = new java.util.Date();
        //java.sql.Date sqlDate = new java.sql.Date(utilDate.getTime());
        Date currentTime = new Date();
        //log.info("Date parm="+  sdfSybaseDate.format(utilDate.getTime()));

        Message message = null;
        try{
            //statement = createStatement();
            //String sql = "dbo.trade_alert_message "+(int)pTradeID+",'"+sdfSybaseDate.format(cal.getTime())+"'";
            String selectSQL = "exec dbo.trade_alert_message ?";
//            statement = symphonyConnection.prepareStatement(selectSQL);
            statement.setDouble(1, pTradeID);
            //statement.setString(2, "09/20/2012");
            rs = statement.executeQuery();
            message = queueSession.createMessage();
            while(rs.next()){
                if(rs.getDate("trade_dt") == null){
                    log.info("no data found");
                    throw new Exception("Error. Did not find a trade with TradeID = "+(int)pTradeID);
                }
                message.setDoubleProperty("TRADE_AUDIT_ID",0);
                message.setDoubleProperty("PRMNT_TRADE_ID",pTradeID);
                message.setStringProperty("UPDATE_DATETIME",sdfDateTime.format(currentTime));
                message.setDoubleProperty("EMP_ID",0);
                message.setStringProperty("UPDATE_TABLE_NAME","N/A");
                message.setStringProperty("UPDATE_BUSN_DT",sdfDateTime.format(currentTime));
                message.setStringProperty("TRADE_TYPE_CODE", trimValue(rs.getString("trade_type_code")));
                message.setStringProperty("TRADE_STAT_CODE", trimValue(rs.getString("trade_stat_code")));
                message.setStringProperty("TRADE_DT",sdfDateTime.format(rs.getDate("trade_dt")));
                message.setStringProperty("CMPNY_SHORT_NAME", trimValue(rs.getString("booking_company_short_name")) );
                message.setStringProperty("BK_SHORT_NAME", "N/A");
                message.setStringProperty("CPTY_SHORT_NAME", trimValue(rs.getString("cpty_short_name")));
                message.setStringProperty("CDTY_CODE", trimValue(rs.getString("cdty_code")));
                message.setStringProperty("BROKERSN", trimValue(rs.getString("broker_short_name")));
                message.setStringProperty("TRADING_SYSTEM","SYM");
                message.setStringProperty("INST_CODE", trimValue(rs.getString("inst_code")));
                message.setStringProperty("NOTIFY_CONTRACTS_FLAG","Y");
                message.setStringProperty("RFRNCE_SHORT_NAME","N/A");
                message.setStringProperty("EMP_NAME", trimValue(rs.getString("trade_mode_name")));
                rowCount++;
            }
        }finally{
            //cal = null;
            currentTime = null;
            if(rs != null){
                rs.close();
                rs = null;
            }
            if(statement != null){
                statement.close();
                statement = null;
            }
        }
        if (rowCount > 1){
            throw new Exception("Error. Received "+rowCount+" rows in result, expected one. TradeID "+(int)pTradeID);
        }
        else if (rowCount == 0){
            log.info("No data found.");
            throw new Exception("Error. Did not find a trade with TradeID = "+(int)pTradeID);
        }
        return message;

    }

    void saveFileData(String pFileName, byte[] pFiledata) throws IOException {
         FileOutputStream fos = null;
         try{
            fos = new FileOutputStream(pFileName);
            fos.write(pFiledata);
         }
         finally{
            if(fos !=  null){
                fos.close();
                fos = null;
            }
         }
    }

    private double getLastVersion(double pTradeID) throws SQLException {
       double result = 0;
       PreparedStatement statement = null;
       ResultSet rs = null;
       String statementSQL = "";
       try{
           statementSQL = "select max(version) version from jbossusr.symphony_file_drop where trade_id = ?";
           statement = opsTrackingConnection.prepareStatement(statementSQL);
           statement.setDouble(1,pTradeID);
           rs = statement.executeQuery();
           if (rs.next()) {
               result = rs.getDouble("version");
           }
       }
       finally {
           if (rs != null) {
               rs.close();
               rs = null;
           }
           if (statement != null){
               statement.close();
               statement = null;
           }
       }
       return result;
    }

    private double updateDatabaseLog(double pTradeID, double pVersion, File pDatasheetFile) throws SQLException, IOException {
        opsTrackingConnection.setAutoCommit(false);

        double newID = 0;
        ResultSet rs = null;
        PreparedStatement statement = null;
        String statementSQL = "";
        try{
            statementSQL = "select jbossusr.seq_symphony_file_drop.nextval from dual";
            statement = opsTrackingConnection.prepareStatement(statementSQL);
            rs = statement.executeQuery();
            if (rs.next()) {
                newID = rs.getDouble("NEXTVAL");
            }
        }
        finally {
            if (rs != null) {
                rs.close();
                rs = null;
            }
            if (statement != null){
                statement.close();
                statement = null;
            }
        }

        //log.info("path "+pDealsheetFile.getAbsolutePath());
        StringBuffer sql = new StringBuffer();
        sql.append("insert into jbossusr.symphony_file_drop(\n");
        sql.append("id,trade_id,data_file_name,processed_flag,version)\n");
        sql.append("values(?,?,?,?,?)");
        PreparedStatement stmnt = opsTrackingConnection.prepareCall(sql.toString());
        //InputStreamReader isr = null;
        //FileInputStream fis = null;
        try{
            stmnt.setDouble(1,newID);
            stmnt.setDouble(2,pTradeID);
            stmnt.setString(3,pDatasheetFile.getName());
            stmnt.setString(4, "N");
            stmnt.setDouble(5,pVersion);
//            log.info(sql.toString());
            stmnt.execute();

        }finally{

            /* if(isr != null){
               isr.close();
               isr = null;
            }
            if(fis != null){
               fis.close();
               fis = null;
            }  */
            if(stmnt != null){
               stmnt.close();
               stmnt = null;
            }
        }

       return newID;
    }

    byte[] getFileData(String pFileName) throws IOException {
         byte[] fileData = null;
         if(! new File(pFileName).exists())
            throw new FileNotFoundException("Error. File "+pFileName+" not found");
         FileInputStream fis = null;
         try{
             fis = new FileInputStream(pFileName);
             int size = fis.available();
             fileData = new byte[size];
             fis.read(fileData);
         }finally{
             if(fis != null){
                fis.close();
                fis = null;
             }
         }
         return fileData;
    }

    private String trimValue(String value){
        if(value != null)
            return value.trim();
        else return null;
    }

    public void close() {
        failedDir = null;
        inProgressDir = null;
        try {
            if(senderSTA != null){
                senderSTA.close();
                senderSTA = null;
            }
        } catch (JMSException e) {
            log.error(e);
        }
        try {
            if(queueConnection != null){
                queueConnection.close();
                queueConnection = null;
            }
        } catch (JMSException e) {
            log.error(e);
        }

        queueConnection = null;
/*        if(symphonyConnection != null){
            try {
                symphonyConnection.close();
            } catch (SQLException e) {
                log.error(e);
            }
            symphonyConnection = null;
        }*/

        if(opsTrackingConnection != null){
            try {
                opsTrackingConnection.close();
            } catch (SQLException e) {
                log.error(e);
            }
            opsTrackingConnection = null;
        }
    }

    synchronized public String startScanning(){
       String result = "started";
       scanningMode = "running";
       return result;
   }

    synchronized public String stopScanning(){
       String result = "stopped";
       scanningMode = "stopped";
       return result;
   }

    protected Sender createSender(String pQueueName) throws NamingException, SQLException, JMSException {
       InitialContext ic = null;
       try{
           ConnectionFactory cf = JndiUtil.lookup("java:/ConnectionFactory");
           QueueConnection localQueueConnection = ((QueueConnectionFactory)cf).createQueueConnection();
           QueueSession localQueueSession = localQueueConnection.createQueueSession(false,Session.CLIENT_ACKNOWLEDGE);
           Queue queue = JndiUtil.lookup("queue/"+pQueueName);
           log.info("creating sender on queue: "+ pQueueName);
           return new Sender(null,localQueueSession,queue);
       }finally{
       }
   }

    //public String getAppFeedFolder() {
    //    return appFeedFolder;
    //}

    //public void setAppFeedFolder(String appFeedFolder) {
    //    this.appFeedFolder = appFeedFolder;
    //}

/*    public java.sql.Connection getMSSqlConnection() throws SQLException, NamingException {
        if (symphonyConnection == null) {
            connectToSQLServer();
        }
        return symphonyConnection;
    }*/

/*    private void connectToSQLServer() throws NamingException, SQLException {
        InitialContext ic = new InitialContext();
        DBInfo dbinfo = (DBInfo) ic.lookup(symphonyDBInfoName);
        symphonyConnection = DriverManager.getConnection(dbinfo.getDBUrl(), dbinfo.getDBUserName(), dbinfo.getDBPassword());
        //Logger.getLogger(OpsTrackingTradeAlertService.class).info(symphonyDBInfoName+"="+dbinfo.getDatabaseName());
        Logger.getLogger(SymphonyProcessorService.class).info("Symphony URL="+dbinfo.getDBUrl());
        symphonyConnection.setAutoCommit(false);
    }*/

    public java.sql.Connection getOracleConnection(String pConnectionName, java.sql.Connection pConnection) throws
            SQLException, NamingException {
        if (pConnection == null) {
            pConnection = connectToOracle(pConnectionName);
        }
        return pConnection;
    }

    private java.sql.Connection connectToOracle(String pConnectionName) throws NamingException, SQLException {
        java.sql.Connection result = null;
        DBInfo dbinfo = JndiUtil.lookup(pConnectionName);
        Logger.getLogger(SymphonyProcessorService.class).info(pConnectionName+"="+dbinfo.getDatabaseName());
        result = DriverManager.getConnection(dbinfo.getDBUrl(), dbinfo.getDBUserName(), dbinfo.getDBPassword());
        result.setAutoCommit(false);
        return result;
    }

    public String testDir() {
        String result = null;
        File scannedDir = new File(fileDropDir);
        if(scannedDir.isDirectory()){
            result = scannedDir.getName() + " exists.";
        }else
            result = scannedDir.getName() + " DOES NOT EXIST.";
        return result;
    }

    synchronized public String publishSymphonyMessageIntoQueueFromIDCommaList(String pQueueName, String pCommaList) throws SQLException {
        String result = "";
        if(pQueueName !=  null)
            pQueueName = pQueueName.trim();
        else{
            result = "queueName is null";
            return result;
        }
        if(scanningMode.equals("running") )
            stopScanning();
        //result = "Please stop scanner, before using this function";

        Statement statement = null;
        ResultSet rs = null;
        InitialContext ic = null;
        String statementSQL;
        int counter = 0;
        try{
            Queue queue = JndiUtil.lookup("queue/"+pQueueName);
            QueueSender qs = queueSession.createSender(queue);

            statement = opsTrackingConnection.createStatement();
            rs = statement.executeQuery("select * from jbossusr.symphony_file_drop where id in ("+pCommaList + ")");

            log.info("query");
            while (rs.next()) {
                //InputStream is = null;
                //oracle.sql.CLOB fdclob = null;
                //FileOutputStream fos = null;
                //byte[] fileData = null;
                try{
                    long ID = rs.getLong("id");
                    long tradeID = rs.getLong("trade_id");
                    String fileName = rs.getString("data_file_name");
                    int version = rs.getInt("version");
                    Message message = getDataFromDataBase(tradeID);
                    message.setStringProperty("DATA_FILE_NAME", fileName);
                    message.setDoubleProperty("SYMPHONY_FILE_DROP_ID",ID);
                    message.setDoubleProperty("VERSION",version);
                    String auditTypeCode = getAuditTypeFromName(fileName);
                    message.setStringProperty("AUDIT_TYPE_CODE", auditTypeCode);
                    /*       fdclob = (oracle.sql.CLOB) rs.getClob("FILE_DATA");
                  is = fdclob.getAsciiStream();
                  int size = is.available();
                  fileData = new byte[size];
                  is.read(fileData);
                  File file = new File(dealsheetProcessDir+"/"+fileName);
                  file.createNewFile();
                  fos = new FileOutputStream(dealsheetProcessDir+"/"+fileName);
                  fos.write(fileData);  */
                    qs.send(message);
                    counter++;
                }finally{
                    /* if(is != null)
                 is.close();
                if(fdclob != null)
                 fdclob = null;
                if(fos != null)
                 fos.close();
                fileData = null;  */
                }
            }

        }
        catch(Exception ex){
            result = ex.getMessage();
            log.error(ex);
        }
        finally {
            if (rs != null) {
                rs.close();
                rs = null;
            }
            if (statement != null){
                statement.close();
                statement = null;
            }
            if(ic != null){
                try {
                    ic.close();
                } catch (NamingException e) {
                    log.error(e.getMessage());
                }
                ic = null;
            }
            if(scanningMode.equals("stopped") )
                startScanning();
        }
        result = result+ ", sent "+counter+" messages to the queue "+pQueueName;
        return result;
    }

    synchronized public String publishSymphonyMessageIntoQueueFromIDToID(String pQueueName, int pStartID, int pEndID) throws SQLException {
        String result = "";
        if(pQueueName !=  null)
            pQueueName = pQueueName.trim();
        else{
            result = "queueName is null";
            return result;
        }
        if(scanningMode.equals("running") )
            stopScanning();
        //    result = "Please stop scanner, before using this function";

        PreparedStatement statement = null;
        ResultSet rs = null;
        InitialContext ic = null;
        int counter = 0;
        String statementSQL = "";
        try{
            Queue queue = JndiUtil.lookup("queue/"+pQueueName);
            QueueSender qs = queueSession.createSender(queue);

            statementSQL = "select * from jbossusr.symphony_file_drop where id >= ? and id <= ?";
            statement = opsTrackingConnection.prepareStatement(statementSQL);
            statement.setInt(1,pStartID);
            statement.setInt(2,pEndID);
            rs = statement.executeQuery();

            log.info("query");

            while (rs.next()) {
                /* InputStream is = null;
               oracle.sql.CLOB fdclob = null;
               FileOutputStream fos = null;
               byte[] fileData = null;      */
                try{
                    long ID = rs.getLong("id");
                    long tradeID = rs.getLong("trade_id");
                    String fileName = rs.getString("data_file_name");
                    int version = rs.getInt("version");
                    Message message = getDataFromDataBase(tradeID);
                    message.setStringProperty("DATA_FILE_NAME", fileName);
                    message.setDoubleProperty("SYMPHONY_FILE_DROP_ID",ID);
                    message.setDoubleProperty("VERSION",version);
                    String auditTypeCode = getAuditTypeFromName(fileName);
                    message.setStringProperty("AUDIT_TYPE_CODE", auditTypeCode);
                    //           fdclob = (oracle.sql.CLOB) rs.getClob("FILE_DATA");
                    //            is = fdclob.getAsciiStream();
                    //           int size = is.available();
                    //              fileData = new byte[size];
                    //              is.read(fileData);
                    //              File file = new File(dealsheetProcessDir+"/"+fileName);
                    //               file.createNewFile();
                    //               fos = new FileOutputStream(dealsheetProcessDir+"/"+fileName);
                    //                fos.write(fileData);
                    qs.send(message);
                    counter++;
                }finally{
                    /* if(is != null)
                 is.close();
                if(fdclob != null)
                 fdclob = null;
                if(fos != null)
                 fos.close();
                fileData = null;  */
                }
            }

        }
        catch(Exception ex){
            result = ex.getMessage();
            log.error(ex);
        }
        finally {
            if (rs != null) {
                rs.close();
                rs = null;
            }
            if (statement != null){
                statement.close();
                statement = null;
            }
            if(ic != null){
                try {
                    ic.close();
                } catch (NamingException e) {
                    log.error(e.getMessage());
                }
                ic = null;
            }
            if(scanningMode.equals("stopped") )
                startScanning();
        }
        result = result+ ", sent "+counter+" messages to the queue "+pQueueName;
        return result;
    }

    private int getNumOfOccurrences(String pStringToTest, String pFindStr){
        int lastIndex = 0;
        int count = 0;

        while(lastIndex != -1){
            lastIndex = pStringToTest.indexOf(pFindStr,lastIndex);

            if( lastIndex != -1){
                count ++;
                lastIndex+=pFindStr.length();
            }
        }
        return count;
    }

    private String storeAsTextFile(String pDirectoryPath, String pFileName, String pTextToStore)
            throws ParseException, IOException {
        FileWriter fileWriter = null;
        String result = "OK";
        try {
            Date now = new Date();
            String filePathName = pDirectoryPath + "\\" + pFileName;
            fileWriter = new FileWriter(filePathName);
            fileWriter.write(pTextToStore);
            fileWriter.flush();
        }
        catch(Exception ex){
            result = ex.getMessage();
            log.error("File creation error: " + ex);
        }
        finally {
            fileWriter.close();
            fileWriter = null;
        }
        return result;
    }

    private boolean isOpsTrackingTradeExist(double pTradeID) throws SQLException {
        double result = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        String statementSQL = "";
        try{
            statementSQL = "select count(*) cnt from ops_tracking.trade where ticket = ?";
            statement = opsTrackingConnection.prepareStatement(statementSQL);
            statement.setDouble(1,pTradeID);
            rs = statement.executeQuery();
            if (rs.next()) {
                result = rs.getDouble("cnt");
            }
        }
        finally {
            if (rs != null) {
                rs.close();
                rs = null;
            }
            if (statement != null){
                statement.close();
                statement = null;
            }
        }
        return result >= 1;
    }

    private String createInterfaceXml(String pTicketNoStr, Message pMessage, SymProcMethodType pMethodType) throws JMSException {
        String xmlText = XMLUtils.XML_HEADER;
        xmlText = xmlText + XMLUtils.buildTagItem(0, "Trade", "", XMLUtils.TAG_OPEN, "TradeNum=\""+pTicketNoStr+"\"");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "Counterparty", pMessage.getStringProperty("CPTY_SHORT_NAME"), XMLUtils.TAG_OPEN_CLOSED, "");
        xmlText = xmlText + XMLUtils.buildTagItem(1, "Operation", "", XMLUtils.TAG_OPEN_CLOSED, "");
        if (pMethodType == SymProcMethodType.ENTER)
        {
            xmlText = xmlText + XMLUtils.buildTagItem(2, "TradeOrders", "", XMLUtils.TAG_OPEN, "OrderNum=\""+pTicketNoStr+"\"");
            xmlText = xmlText + XMLUtils.buildTagItem(3, "TradeOrder", "", XMLUtils.TAG_OPEN_CLOSED, "");
            xmlText = xmlText + XMLUtils.buildTagItem(2, "TradeOrders", "", XMLUtils.TAG_CLOSED, "OrderNum=\""+pTicketNoStr+"\"");
        }
        xmlText = xmlText + XMLUtils.buildTagItem(0, "Trade", "", XMLUtils.TAG_CLOSED, "");
        return xmlText;
    }

    private String getFileName(String ticketNoStr, double ticketNo, SymProcMethodType pMethodType) throws SQLException {
        Date currentDate = new Date();
        SimpleDateFormat sdfDate = new SimpleDateFormat("yyyyMMdd");
        SimpleDateFormat sdfTime = new SimpleDateFormat("HHmmss");
        String formattedDate = sdfDate.format(currentDate);
        String formattedTime = sdfTime.format(currentDate);

        boolean isEdit = isOpsTrackingTradeExist(ticketNo);
        if (!isEdit && pMethodType == SymProcMethodType.VOID)
            return "FAILED";

        String tradeOpType = null;

        if (isEdit)
        {
            if (pMethodType == SymProcMethodType.ENTER)
                tradeOpType = "UPD";
            else
                tradeOpType = "DEL";
        }
        else
            tradeOpType = "INS";

        return ticketNoStr + "-" + formattedDate + "-" + formattedTime + "-" + tradeOpType;
    }

    private String createHtmlText() {
        return "<HTML> \n" +
                "<HEAD> \n" +
                "</HEAD> \n" +
                " \n"   +
                "<BODY BGCOLOR=#FFFFFF> \n" +
                "SymphonyProcessor generated this file. No trade data is contained here." +
                "</BODY> \n" +
                "</HTML>";
    }

    synchronized public String enterTradeIntoOpsManagerByTicketCommaList(String pTicketIdOrCommaList) throws SQLException {
        String result = pTicketIdOrCommaList + " created.";
        if(pTicketIdOrCommaList !=  null)
            pTicketIdOrCommaList = pTicketIdOrCommaList.trim();
        else{
            result = "Ticket comma list is null";
            return result;
        }

        try {
            if(scanningMode.equals("running") )
                stopScanning();

            int commaCount = getNumOfOccurrences(pTicketIdOrCommaList,",");
            if (commaCount == 0)
                commaCount = 1;
            String[] ticketList = new String[commaCount];

            if (pTicketIdOrCommaList.contains(","))
                ticketList = pTicketIdOrCommaList.split(",");
            else
                ticketList[0] = pTicketIdOrCommaList;

           // String[] ticketList = pTicketIdOrCommaList.split(",");
           for( int i = 0; i < ticketList.length; i++)
              {
                  String ticketNoStr = ticketList[i];
                  log.info("Processing ticket no: " + ticketNoStr + "...");
                  double ticketNo = Double.parseDouble(ticketNoStr);
                  Message message = getDataFromDataBase(ticketNo);
                  //If not found, an exception is raised and nothing past here executes.

                  String xmlText = createInterfaceXml(ticketNoStr, message, SymProcMethodType.ENTER);
                  String fileName = getFileName(ticketNoStr, ticketNo, SymProcMethodType.ENTER);

                  String fileResult = storeAsTextFile(fileDropDir, fileName + ".xml", xmlText);
                  if (fileResult != "OK")
                  {
                      result = "XML File creation error: " + fileResult;
                      log.info(result);
                      continue;
                  }

                  String htmlText = createHtmlText();
                  fileResult = storeAsTextFile(fileDropDir, fileName + ".HTML", htmlText);
                  if (fileResult != "OK")
                  {
                      log.info("HTML File creation error: " + fileResult);
                      result = result + "\nFile creation error: " + fileResult;
                  }

                  log.info("Ticket = " + fileName + " successfully created.");
              }
        }
        catch(Exception ex){
            result = ex.getMessage();
            log.error(ex);
        }
        finally {
            if(scanningMode.equals("stopped") )
                startScanning();
        }

        return result;
    }


 /*   synchronized public String voidOpsManagerTradeByTicketCommaList(String pTicketIdOrCommaList) throws SQLException {
        String result = pTicketIdOrCommaList + " created.";
        if(pTicketIdOrCommaList !=  null)
            pTicketIdOrCommaList = pTicketIdOrCommaList.trim();
        else{
            result = "Ticket comma list is null";
            return result;
        }

        try {
            if(scanningMode.equals("running") )
                stopScanning();

            int commaCount = getNumOfOccurrences(pTicketIdOrCommaList,",");
            if (commaCount == 0)
                commaCount = 1;
            String[] ticketList = new String[commaCount];

            if (pTicketIdOrCommaList.contains(","))
                ticketList = pTicketIdOrCommaList.split(",");
            else
                ticketList[0] = pTicketIdOrCommaList;

            for( int i = 0; i < ticketList.length; i++)
            {
                String ticketNoStr = ticketList[i];
                log.info("Processing ticket no: " + ticketNoStr + "...");
                double ticketNo = Double.parseDouble(ticketNoStr);
                Message message = getDataFromDataBase(ticketNo);
                //If not found, an exception is raised and nothing past here executes.

                String xmlText = createInterfaceXml(ticketNoStr, message, SymProcMethodType.VOID);
                String fileName = getFileName(ticketNoStr, ticketNo, SymProcMethodType.VOID);

                String fileResult = storeAsTextFile(fileDropDir, fileName + ".xml", xmlText);
                if (fileResult != "OK")
                {
                    result = "XML File creation error: " + fileResult;
                    log.info(result);
                    continue;
                }

                String htmlText = createHtmlText();
                fileResult = storeAsTextFile(fileDropDir, fileName + ".HTML", htmlText);
                if (fileResult != "OK")
                {
                    log.info("HTML File creation error: " + fileResult);
                    result = result + "\nFile creation error: " + fileResult;
                }

                log.info("Ticket = " + fileName + " successfully created.");
            }
        }
        catch(Exception ex){
            result = ex.getMessage();
            log.error(ex);
        }
        finally {
            if(scanningMode.equals("stopped") )
                startScanning();
        }

        return result;
    }
*/
}
