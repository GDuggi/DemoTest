package aff.confirm.mbeans.inbound.inboundreceiver;

import aff.confirm.common.daoinbound.inbound.ejb3.InboundDocsDAOLocal;
import aff.confirm.common.daoinbound.inbound.ejb3.InboundDocsDAORemote;
import aff.confirm.common.daoinbound.inbound.model.InboundDocsEntity;
import aff.confirm.common.util.JndiUtil;
import aff.confirm.jboss.common.exceptions.LogException;
import aff.confirm.mbeans.common.CommonListenerService;
import aff.confirm.mbeans.common.exceptions.StopServiceException;
import aff.confirm.webservices.sempradocws.stub.org.tempuri.SempraDocWSStub;
import org.apache.axis2.AxisFault;
import org.apache.axis2.client.Options;
import org.apache.axis2.transport.http.HttpTransportProperties;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.NodeList;

import javax.activation.DataHandler;
import javax.activation.FileDataSource;
import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.ejb.Singleton;
import javax.ejb.Startup;
import javax.naming.Context;
import javax.naming.InitialContext;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import java.io.*;
import java.text.SimpleDateFormat;
import java.util.*;

/**
 * User: mthoresen
 * Date: Jun 23, 2009
 * Time: 2:22:55 PM
 */
@Startup
@Singleton
public class InboundReceiverService extends CommonListenerService implements InboundReceiverServiceMBean {
    public final SimpleDateFormat DatabaseDateFormat = new java.text.SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
    public final SimpleDateFormat ExpandedDateFormat = new java.text.SimpleDateFormat("MM/dd/yyyy HH:mm:ss");
    public final SimpleDateFormat ExpandedMSSqlDateFormat = new java.text.SimpleDateFormat("yyyy-MM-dd h:m:s");
    public final SimpleDateFormat StandardDateFormat = new java.text.SimpleDateFormat("M/d/yyyy h:m:s aaa");

    private InboundDocsDAOLocal adoBean;
    private InitialContext ctx;
    private SempraDocWSStub stub;
    // Attributes
    private String fileScanDir;
    private String fileFailedDir;
    private String fileProcessedDir;
    private String fileDiscardDir;
    private String ocrScanDir;
    private String vaultUrl;
    private String userName;
    private String password;
    private String domain;
    private String host;
    private String vaultFolderName;
    private String fieldNames;
    private String dslName;

    public InboundReceiverService() {
       super("affinity.inbound:service=InboundReceiverService");
    }

    @PostConstruct
    public void postConstruct() throws Exception {
        super.postConstruct();
    }

    @PreDestroy
    public void preDestroy() throws Exception {
        super.preDestroy();
    }

    public String getOcrScanDir() {
        return ocrScanDir;
    }

    public void setOcrScanDir(String ocrScanDir) {
        this.ocrScanDir = ocrScanDir;
    }

    public String getVaultUrl() {
        return vaultUrl;
    }

    public void setVaultUrl(String vaultUrl) {
        this.vaultUrl = vaultUrl;
    }

    public String getUserName() {
        return userName;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public String getDomain() {
        return domain;
    }

    public void setDomain(String domain) {
        this.domain = domain;
    }

    public String getHost() {
        return host;
    }

    public void setHost(String host) {
        this.host = host;
    }

    public String getVaultFolderName() {
        return vaultFolderName;
    }

    public void setVaultFolderName(String vaultFolderName) {
        this.vaultFolderName = vaultFolderName;
    }

    public String getFieldNames() {
        return fieldNames;
    }

    public void setFieldNames(String fieldNames) {
        this.fieldNames = fieldNames;
    }

    public String getDslName() {
        return dslName;
    }

    public void setDslName(String dslName) {
        this.dslName = dslName;
    }

    public String getFileScanDir() {
        return this.fileScanDir;
    }

    public void setFileScanDir(String fileScanDir) {
        this.fileScanDir = fileScanDir;
    }

    public String getFileFailedDir() {
        return this.fileFailedDir;
    }

    public void setFileFailedDir(String fileFailedDir) {
        this.fileFailedDir = fileFailedDir;
    }

    public String getFileProcessedDir() {
        return this.fileProcessedDir;
    }

    public void setFileProcessedDir(String fileProcessedDir) {
        this.fileProcessedDir = fileProcessedDir;
    }

    public String getFileDiscardDir() {
        return this.fileDiscardDir;
    }

    public void setFileDiscardDir(String fileDiscardDir) {
        this.fileDiscardDir = fileDiscardDir;
    }

    @Override
    protected void executeTimerEvent() throws StopServiceException {
        super.executeTimerEvent();
        List fileList = getFileList();
        sortFiles(fileList);
        processFiles(fileList);
    }

    @Override
    protected void runTask() throws aff.confirm.jboss.common.exceptions.StopServiceException, LogException {
        poll();
    }

    @Override
    protected void initService() throws Exception{
        printInfo("initService starting...");
        String text = "Timer interval = " + (getTimerPeriod() / 1000 / 60) + " minutes.";
        printInfo(text);

        try {
            super.initService();

            adoBean = JndiUtil.lookup("java:global/InboundDocsDAOLib/InboundDocsDAOBean!" + InboundDocsDAOLocal.class.getName());
            initDirectories();
            initWebClient();

            printInfo("initService complete.");
        }
        catch (Exception e) {
            log.error( "ERROR", e );
            throw  e;
        }
    }

    private void initDirectories() {
        File fileDir = new File(this.fileScanDir);
        if(!fileDir.exists()){
            fileDir.mkdirs();
        }

        fileDir = new File(this.fileFailedDir);
        if(!fileDir.exists()){
            fileDir.mkdirs();
        }

        fileDir = new File(this.fileProcessedDir);
        if(!fileDir.exists()){
            fileDir.mkdirs();
        }

        fileDir = new File(this.fileDiscardDir);
        if(!fileDir.exists()){
            fileDir.mkdirs();
        }
    }

    class FileComparator implements Comparator {
        public int compare(Object o1, Object o2){
            long modified1 = ((File) o1).lastModified();
            long modified2 = ((File) o2).lastModified();
            if (modified1 == modified2)
              return 0;
            else
            {
              if (modified1 < modified2)
                return -1;
              else
                return 1;
            }
        }
    }

    private void processFiles(List fileList) throws StopServiceException {
        InboundDocsEntity entity;
        try {
            for (Object aFileList : fileList) {
                File inboundDocFile = (File) aFileList;
                if (documentExists(inboundDocFile)) {
                    entity = processInboundDocument(inboundDocFile);
                    if (entity != null) {
                        entity = adoBean.makePersistent(entity);
                        moveFileToProcessed(inboundDocFile);
                        vaultProcessedFile(entity);
                    }
                } else {
                    // raise some type of warning that the coresponding document
                    // is not in the scan directory.
                    printError("Inbound Document not found in the scan directory. File: " + inboundDocFile.getName().replace(".xml", ".tif"), null);
                    moveFileToFailedDirectory(inboundDocFile);
                }
            }
        } catch (Exception e) {
            throw new StopServiceException( getClass().getSimpleName() + ": Process Files Error: " + e.getMessage());
        } 
    }

    private String generateFieldValues(InboundDocsEntity doc) throws Exception {
        String INBOUND_DOC_ID;
        String CALLER_REF;
        String SENT_TO;
        String RCVD_TS;
        String FILE_NAME;
        String SENDER;
        String MAPPED_CPTY_SN;
        String MAPPED_BRKR_SN;
        String MAPPED_CDTY_CODE;
        String fieldValues = "";
        try{
            INBOUND_DOC_ID = Long.toString(doc.getId());
            CALLER_REF = doc.getCallerRef();
            SENT_TO = doc.getSentTo();
            StringBuilder rcvdTs = new StringBuilder( DatabaseDateFormat.format( doc.getRcvdTs() ) );
            RCVD_TS = rcvdTs.toString();
            FILE_NAME = doc.getFileName();
            SENDER = "";
            MAPPED_CPTY_SN = "";
            MAPPED_BRKR_SN = "";
            MAPPED_CDTY_CODE = "";
            fieldValues = INBOUND_DOC_ID + "|" + CALLER_REF + "|" + SENT_TO + "|" + RCVD_TS + "|" + FILE_NAME + "|" + SENDER + "|" + MAPPED_CPTY_SN + "|" + MAPPED_BRKR_SN + "|" + MAPPED_CDTY_CODE;
        }   catch(Exception ex){
            throw ex;
        }
        return fieldValues;
    }


    private void vaultProcessedFile(InboundDocsEntity inbDoc) {
        String tifFile;
        String fieldValues;
        File processedFile = new File(fileProcessedDir + "/" + inbDoc.getFileName());
        if(processedFile.exists()){
            try {
                fieldValues = generateFieldValues(inbDoc);
                vaultDoc(processedFile, fieldValues);
                System.gc();
                processedFile.delete();
            } catch (Exception e) {
                sendMail("Inbound Receiver Service Vaulting Exception", "Exception in vaultProcessedFile for document: " + inbDoc.getFileName() + ". " + e.getMessage());
                log.error("ERROR", e);
            }
        }
    }

    private String vaultDoc(File file, String fieldValues) throws Exception {
        try {
            DataHandler dh = new DataHandler(new FileDataSource(file));
            SempraDocWSStub.ImportDocByStream request = new SempraDocWSStub.ImportDocByStream();
            request.setData(dh);
            request.setDocRepository(vaultFolderName);
            if(dslName == null){
                request.setDslName("");
            } else request.setDslName(dslName);
            request.setFieldNames(fieldNames);
            request.setFieldValues(fieldValues);

            int mid = file.getName().lastIndexOf(".");

            String ext = file.getName().substring(mid,file.getName().length());

            request.setFileType(ext);
            
            printInfo("VaultFolderName: " + vaultFolderName);
            printInfo("FieldNames: " + fieldNames);
            printInfo("FieldValues: " + fieldValues);
            printInfo("File Type:" +ext);

            SempraDocWSStub.ImportDocByStreamResponse resp = stub.ImportDocByStream(request);

            String resultXML = resp.getImportDocByStreamResult();

            DocumentBuilderFactory builderFactory = DocumentBuilderFactory.newInstance();
            DocumentBuilder builder = builderFactory.newDocumentBuilder();

            resultXML = resultXML.trim();
            InputStream in = new ByteArrayInputStream(resultXML.getBytes("UTF-8"));
            Document response = builder.parse(in);

            in.close();

            NodeList resultCode = response.getElementsByTagName("ResultCode");
            NodeList resultValue = response.getElementsByTagName("ResultValue");

           String code = resultCode.item(0).getFirstChild().getNodeValue();
           String value = resultValue.item(0).getFirstChild().getNodeValue();

            if (code.equals("0")){
                 return value;
            } else {
                throw new Exception("Exception in vaultDoc for File: " + file.getName() + ". Field Values: " + fieldValues + ". " + value);
            }
        } catch (Exception e) {
            throw e;
        }
    }
    

    private void initWebClient() {
        try {
            printInfo("Generating stub for URL: " + vaultUrl);
            stub = new SempraDocWSStub(vaultUrl);
            Options options = stub._getServiceClient().getOptions();
            HttpTransportProperties.Authenticator authenticator = new   HttpTransportProperties.Authenticator();
            List authScheme = new ArrayList();
            authScheme.add(HttpTransportProperties.Authenticator.NTLM);
            authenticator.setAuthSchemes(authScheme);
            authenticator.setUsername(userName);
            authenticator.setPassword(password);
            authenticator.setHost(host);
            authenticator.setDomain(domain);
            options.setProperty(org.apache.axis2.transport.http.HTTPConstants.AUTHENTICATE, authenticator);
            options.setUserName(userName);
            options.setPassword(password);
            printInfo("Setting AXIS Authenticator options.");
            stub._getServiceClient().setOptions(options);
            SempraDocWSStub.Hello hello = new SempraDocWSStub.Hello();
            printInfo("Invoking HELLO webservice test call...");
            SempraDocWSStub.HelloResponse resp = stub.Hello(hello);
            String respValue = resp.getHelloResult();
            printInfo("Hello Response-> " + respValue);
        } catch (AxisFault axisFault) {
            log.error("ERROR", axisFault);
        } catch (java.rmi.RemoteException e) {
            log.error("ERROR", e );
        }
    }

    private boolean documentExists(File inboundDocFile)  {
        File tifImageFile = new File(fileScanDir + "/" + inboundDocFile.getName().replace(".xml", ".tif"));
        File telexFile = new File(fileScanDir + "/" + inboundDocFile.getName().replace(".xml", ".txt"));
        return tifImageFile.exists() || telexFile.exists();
    }

    private void moveFileToProcessed(File inboundDocFile) throws Exception{
        // All .xml files can be discarded....
        File tifImageFile = new File(fileScanDir + "/" + inboundDocFile.getName().replace(".xml", ".tif"));
        File telexFile = new File(fileScanDir + "/" + inboundDocFile.getName().replace(".xml", ".txt"));

        // Move processed XML file to discard
        File discardedFile = new File(fileDiscardDir + "/" + inboundDocFile.getName());
        inboundDocFile.renameTo(discardedFile);

        System.gc();
        discardedFile.delete();

        if(tifImageFile.exists()){
            // move XML coresponding tif file to processed directory
            File processedFile = new File(fileProcessedDir + "/" + tifImageFile.getName());
            tifImageFile.renameTo(processedFile);
            printInfo("Moved To Processed Directory: " + processedFile.getName());
            // Now, Copy processed File to PrimeOCRScan Directory
            copyFile(fileProcessedDir + "/" + tifImageFile.getName(), ocrScanDir + "/" + tifImageFile.getName());
        } else if (telexFile.exists()){
            // move XML coresponding telex file to processed directory
            File processedFile = new File(fileProcessedDir + "/" + telexFile.getName());
            telexFile.renameTo(processedFile);
            printInfo("Moved To Processed Directory: " + processedFile.getName());
        }
    }

    private void copyFile(String srFile, String dtFile) throws Exception {
        File f1 = new File(srFile);
        File f2 = new File(dtFile);
        InputStream in = new FileInputStream(f1);

        if(f2.exists()){
            f2.delete();
        }

        OutputStream out = new FileOutputStream(f2);

        byte[] buf = new byte[1024];
        int len;
        while ((len = in.read(buf)) > 0){
          out.write(buf, 0, len);
        }
        in.close();
        out.close();
        printInfo("Moved To OCR Scan Directory: " + f2.getName());
    }

    private InboundDocsEntity processInboundDocument(File xmlFileName) throws StopServiceException {
        FileInputStream stream;
        InboundDocsEntity ibDoc = new InboundDocsEntity();
        ibDoc.setFileName(xmlFileName.getName().replace(".xml", ".tif"));
        ibDoc.setHasAutoAsctedFlag("N");
        DocumentBuilderFactory docBuilderFactory = DocumentBuilderFactory.newInstance();
        DocumentBuilder docBuilder;
        try {
            docBuilder = docBuilderFactory.newDocumentBuilder();
            if(xmlFileName.exists()){
                String xmlName = xmlFileName.getAbsolutePath();
                stream = new FileInputStream(xmlName);
                Document doc = docBuilder.parse (stream);
                // normalize text representation
                Element root =  doc.getDocumentElement ();
                root.normalize();
                // SENT TO
                Element sentTo = ((Element)doc.getElementsByTagName("sent_to").item(0));
                if(sentTo != null){
                    if(sentTo.getChildNodes().item(0) != null){
                        ibDoc.setSentTo(sentTo.getChildNodes().item(0).getNodeValue().trim());
                    }
                }
                // CALLER REFERENCE
                /*Element callerRef = ((Element)doc.getElementsByTagName("CSID").item(0));
                if(callerRef != null){
                    if(callerRef.getChildNodes().item(0) != null)      {
                        String callerRefVal = callerRef.getChildNodes().item(0).getNodeValue().trim();
                        if(callerRefVal.indexOf("(") >=0){
                           callerRefVal = callerRefVal.substring(0, callerRefVal.indexOf("(")).trim();
                        }
                        ibDoc.setCallerRef(callerRefVal);
                    }
                }*/

                //Added by Israel 2/3/2015 to replace actual caller ref
                //Merged by Israel 2/9/2015 to replace actual caller ref
                Element actualFileName = ((Element)doc.getElementsByTagName("Actual_File_Name").item(0));
                if (actualFileName != null) {
                    if (actualFileName.getChildNodes().item(0) != null) {
                        String callerRefVal = actualFileName.getChildNodes().item(0).getNodeValue().trim();
                        if(callerRefVal.indexOf("(") >=0){
                            callerRefVal = callerRefVal.substring(0, callerRefVal.indexOf("(")).trim();
                        }
                        ibDoc.setCallerRef(callerRefVal);
                    }
                }

                // RECEIVED TIME STAMP
                Element timeStamp = ((Element)doc.getElementsByTagName("TimeStamp").item(0));
                String dateAndTime = timeStamp.getChildNodes().item(0).getNodeValue().trim();
                ibDoc.setRcvdTs(new java.sql.Timestamp( StandardDateFormat.parse(dateAndTime).getTime()));

                // JOB REFERENCE NUMBER
                Element jobRef = ((Element)doc.getElementsByTagName("Job_Reference").item(0));
                if(jobRef != null){
                    if(jobRef.getChildNodes().item(0) != null)      {
                        String jobRefVal = jobRef.getChildNodes().item(0).getNodeValue().trim();
                        ibDoc.setJobRef(jobRefVal);
                    }
                }
            }
        } catch (Exception e) {
            try {
                ibDoc = null;
                moveFileToFailedDirectory(xmlFileName);
                incErrorCount(1);
            } catch (StopServiceException e1) {
                printError("ERROR moveFileToFailedDirectory", e1);
                throw new StopServiceException(e1.getMessage() + ". Error:" + e.getMessage());
            }
        }
        return ibDoc;
    }

    private void moveFileToFailedDirectory(File inboundDocFile) {
        File tifImageFile = new File(fileScanDir + "/" + inboundDocFile.getName().replace(".xml", ".tif"));
        File telexFile = new File(fileScanDir + "/" + inboundDocFile.getName().replace(".xml", ".txt"));

        // Move failed XML file to failed directory
        File discardedFile = new File(fileFailedDir + "/" + inboundDocFile.getName());
        inboundDocFile.renameTo(discardedFile);

        if(tifImageFile.exists()){
            // move XML coresponding tif file to failed directory
            File processedFile = new File(fileFailedDir + "/" + tifImageFile.getName());
            tifImageFile.renameTo(processedFile);

        } else if (telexFile.exists()){
            // move XML coresponding telex file to failed directory
            File processedFile = new File(fileFailedDir + "/" + telexFile.getName());
            telexFile.renameTo(processedFile);
        }
    }

    private void sortFiles(List fileList) {
        if(fileList.size() > 0)
         Collections.sort(fileList, new FileComparator());
    }

    private List getFileList() {
        File scannedDir = new File(this.fileScanDir);
        List fileList = new LinkedList();
        File[] scannedFile = scannedDir.listFiles();
        if (scannedDir.isDirectory())
        {
          if (scannedFile != null)
          {
              for (File scanFile : scannedFile) {
                  if (scanFile.isFile()) {
                      // we only want the xml files because they have the data for the
                      // coresponding .tif / .txt file
                      if (scanFile.getName().indexOf(".xml") > 0)
                          fileList.add(scanFile);
                  }
              }
          }
        }
        scannedDir = null;
        scannedFile = null;
        return fileList;
    }

    public static void main(String[] args) {
        InitialContext ctx;
        InboundDocsDAORemote bean;
        List<InboundDocsEntity> myList;
        InboundDocsEntity entity = new InboundDocsEntity();
     //   entity.setTradeId(new Long(1000370));

        try {
            Properties env = new Properties( );
            env.put(Context.PROVIDER_URL, "localhost:1099");
            env.put(Context.URL_PKG_PREFIXES, "org.jboss.naming:org.jnp.interfaces");
            env.put(Context.INITIAL_CONTEXT_FACTORY,"org.jnp.interfaces.NamingContextFactory");
            ctx = new InitialContext(env);

            bean = (InboundDocsDAORemote) ctx.lookup("InboundDocsDAOBean/remote");

            entity = bean.findById(new Long(999264), false);

  //          VPcTradeSummaryEntity template = new VPcTradeSummaryEntity();
//            template.setTradeId(new Long(1050310));
//            myList = bean.findByExample(template, "");
            System.out.println("Total Found: ");
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
