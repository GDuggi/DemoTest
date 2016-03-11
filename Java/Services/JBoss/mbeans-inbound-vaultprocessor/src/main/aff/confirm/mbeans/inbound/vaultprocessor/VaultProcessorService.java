package aff.confirm.mbeans.inbound.vaultprocessor;

import aff.confirm.common.daoinbound.inbound.ejb3.AssociatedDocsDAOLocal;
import aff.confirm.common.daoinbound.inbound.ejb3.TradeDataDAOLocal;
import aff.confirm.common.daoinbound.inbound.model.AssociatedDocsEntity;
import aff.confirm.common.daoinbound.inbound.model.TradeDataEntity;
import aff.confirm.common.util.JndiUtil;
import aff.confirm.mbeans.common.CommonListenerService;
import aff.confirm.mbeans.common.exceptions.StopServiceException;
import aff.confirm.webservices.sempradocws.stub.org.tempuri.SempraDocWSStub;
import org.apache.axiom.om.OMFactory;
import org.apache.axis2.AxisFault;
import org.apache.axis2.client.Options;
import org.apache.axis2.transport.http.HttpTransportProperties;
import org.w3c.dom.Document;
import org.w3c.dom.NodeList;

import javax.activation.DataHandler;
import javax.activation.FileDataSource;
import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.ejb.Singleton;
import javax.ejb.Startup;
import javax.management.ObjectName;
import javax.naming.Context;
import javax.naming.InitialContext;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import java.io.ByteArrayInputStream;
import java.io.File;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.List;
import java.util.StringTokenizer;
import org.jboss.logging.Logger;

/**
 * User: mthoresen
 * Date: Aug 31, 2009
 * Time: 11:48:29 AM
 */
@Startup
@Singleton
public class VaultProcessorService extends CommonListenerService implements VaultProcessorServiceMBean {
    private static Logger log = Logger.getLogger( VaultProcessorService.class );
    private SempraDocWSStub stub;
    private List<String> fileTypesToVault;
    private AssociatedDocsDAOLocal adoAssDocBean;
    private TradeDataDAOLocal adoTradeDataBean;


    // Attributes
    private String fileScanDir;
    private String fileFailedDir;
    private String fileProcessedDir;
    private String vaultUrl;
    private String userName;
    private String password;
    private String domain;
    private String host;
    private String fileTypes;
    private String vaultFolderName;
    private String fieldNames;
    private String dslName;
    private String chkFnlApprvFlag;
    private String locationCode;
    private ObjectName objectName;

    public enum DocTypeCode {
        XQBBP, XQCCP, XQCSP, VBCP, NOVALUE;

        public static DocTypeCode toDocTypeCode(String str) {
            try {
                return valueOf(str);
            } catch (Exception ex) {
                return NOVALUE;
            }
        }
    }

    public VaultProcessorService() {
        super("affinity.inbound:service=OCROutputDirectoryVaultProcessor");
    }

    @PostConstruct
    public void postConstruct() throws Exception {
        super.postConstruct();
    }

    @PreDestroy
    public void preDestroy() throws Exception {
        super.preDestroy();
    }

    public String getLocationCode() {
        return locationCode;
    }

    public void setLocationCode(String locationCode) {
        this.locationCode = locationCode;
    }

    public String getFileScanDir() {
        return this.fileScanDir;
    }

    public void setFileScanDir(String fileScanDir) {
        this.fileScanDir = fileScanDir;
    }

    public String getFileFailedDir() {
        OMFactory factory = null;
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

    public String getVaultUrl() {
        return this.vaultUrl;
    }

    public void setVaultUrl(String vaultUrl) {
        this.vaultUrl = vaultUrl;
    }

    public String getUserName() {
        return this.userName;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }

    public String getPassword() {
        return this.password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public String getDomain() {
        return this.domain;
    }

    public void setDomain(String domain) {
        this.domain = domain;
    }

    public String getHost() {
        return this.host;
    }

    public void setHost(String host) {
        this.host = host;
    }

    public String getFileTypes() {
        return this.fileTypes;
    }

    public void setFileTypes(String fileTypes) {
        this.fileTypes = fileTypes;
    }

    public String getVaultFolderName() {
        return this.vaultFolderName;
    }

    public void setVaultFolderName(String vaultFolderName) {
        this.vaultFolderName = vaultFolderName;
    }

    public String getFieldNames() {
        return this.fieldNames;
    }

    public void setFieldNames(String fieldNames) {
        this.fieldNames = fieldNames;
    }

    public String getDslName() {
        return this.dslName;
    }

    public void setDslName(String dslName) {
        this.dslName = dslName;
    }

    public String getChkFnlApprvFlag() {
        return chkFnlApprvFlag;
    }

    public void setChkFnlApprvFlag(String chkFnlApprvFlag) {
        this.chkFnlApprvFlag = chkFnlApprvFlag;
    }

    @Override
    protected void executeTimerEvent() throws StopServiceException {
        super.executeTimerEvent();
        try {
            vaultFiles();
        } catch (Exception ex) {
            throw new StopServiceException(ex.getMessage());
        }
    }

    private void vaultFiles() throws StopServiceException {
        String fieldValues;
        AssociatedDocsEntity template = new AssociatedDocsEntity();
        List<AssociatedDocsEntity> list;
        template.setVaultedFlag("N");
        list = adoAssDocBean.findByExample(template, "");
        for (AssociatedDocsEntity assDoc : list) {
            if (canVaultThisDoc(assDoc)) {
                File file = new File(fileScanDir + "/" + assDoc.getFileName());
                if (file.exists()) {
                    try {
                        fieldValues = generateFieldValues(assDoc);
                        vaultDoc(file, fieldValues);
                        assDoc.setVaultedFlag("Y");
                        assDoc = adoAssDocBean.makePersistent(assDoc);
                        System.gc();
                    } catch (Exception e) {
                        sendMail("Vault Processor Service Exception", "Exception in vaultFiles for document: " + assDoc.getFileName() + ". " + e.getMessage());
                        log.error( "ERROR", e);
                    }
                }
            }
        }
    }

    private boolean canVaultThisDoc(AssociatedDocsEntity assDoc) {
        return (assDoc.getDocStatusCode().equals("APPROVED") || assDoc.getDocStatusCode().equals("FINALAPPROVED"));
    }

    private String generateFieldValues(AssociatedDocsEntity doc) throws Exception {
        String tradeSysCode;
        String tradeSysID;
        String senderTypeInd;
        String senderShortName;
        String senderRef;
        String cptyShortName;
        String seCptyShortName;
        String fieldValues = "";
        try {
            if (doc.getTradeId() > 999999)
                tradeSysCode = "A";
            else tradeSysCode = "J";
            tradeSysID = Long.toString(doc.getTradeId());
            senderTypeInd = getSenderTypeCode(doc.getDocTypeCode());
            senderShortName = doc.getDocTypeCode();
            senderRef = doc.getFinalApprovedBy();
            cptyShortName = doc.getCptySn();
            seCptyShortName = getSETCompany(doc.getTradeId());
            fieldValues = tradeSysCode + "|" + tradeSysID + "|" + senderTypeInd + "|" + senderShortName + "|" + senderRef + "|" + cptyShortName + "|" + seCptyShortName;

        } catch (Exception ex) {
            throw ex;
        }
        return fieldValues;
    }

    private String getSETCompany(Long tradeId) {
        String setCompany = "NA";
        TradeDataEntity template = new TradeDataEntity();
        List<TradeDataEntity> list;
        template.setTradeId(tradeId);
        list = adoTradeDataBean.findByExample(template, "");
        for (TradeDataEntity tradeData : list) {
            setCompany = tradeData.getSeCptySn();
        }
        return setCompany;
    }

    private String getSenderTypeCode(String docTypeCode) {
        switch (DocTypeCode.toDocTypeCode(docTypeCode)) {
            case XQBBP:
                return "B";
            case XQCCP:
                return "CC";
            case XQCSP:
                return "CS";
            case VBCP:
                return "VB";
            default:
                return "NA";
        }
    }

    private String vaultDoc(File file, String fieldValues) throws Exception {
        try {
            DataHandler dh = new DataHandler(new FileDataSource(file));
            SempraDocWSStub.ImportDocByStream request = new SempraDocWSStub.ImportDocByStream();
            request.setData(dh);
            request.setDocRepository(vaultFolderName);
            request.setFieldNames(fieldNames);
            request.setFieldValues(fieldValues);
            request.setFileType("");

            if (dslName == null) {
                request.setDslName("");
            } else request.setDslName(dslName);

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

            if (code.equals("0")) {
                return value;
            } else {
                throw new Exception("Exception in vaultDoc for File: " + file.getName() + ". Field Values: " + fieldValues + ". " + value);
            }
        } catch (Exception e) {
            throw e;
        }
    }

    @Override
    protected void initService() throws Exception {
        super.initService();

        Context ctx = new InitialContext();
        try {
            adoAssDocBean = JndiUtil.lookup(ctx, "java:global/InboundDocsDAOLib/AssociatedDocsDAOBean!aff.confirm.common.daoinbound.inbound.ejb3.AssociatedDocsDAOLocal");
            adoTradeDataBean = JndiUtil.lookup(ctx, "java:global/InboundDocsDAOLib/TradeDataDAOBean!aff.confirm.common.daoinbound.inbound.ejb3.TradeDataDAOLocal");
        } finally {
            ctx.close();
        }

        initWebClient();
        StringTokenizer st = new StringTokenizer(fileTypes, ",");
        fileTypesToVault = new ArrayList<String>();
        while (st.hasMoreTokens()) {
            fileTypesToVault.add(st.nextToken());
        }
    }

    private void initWebClient() {
        try {
            printInfo("Generating stub for URL: " + vaultUrl);
            stub = new SempraDocWSStub(vaultUrl);
            Options options = stub._getServiceClient().getOptions();
            HttpTransportProperties.Authenticator authenticator = new HttpTransportProperties.Authenticator();
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
        } catch (java.rmi.RemoteException e) {
            log.error( "ERROR", e);
        }
    }

    public static void main(String[] args) throws Exception {
/*        SempraDocWSStub stub;
        String vaultUrl = "http://stdoc1/SempraDocumentWebService/SempraDocWS.asmx";
        AssociatedDocsDAORemote adoAssDocBean;
        
        InitialContext ctx;
        TradeSummaryDAORemote adoTradeSummaryBean;


        stub = new SempraDocWSStub(vaultUrl);
        Options options = stub._getServiceClient().getOptions();
        HttpTransportProperties.Authenticator authenticator = new   HttpTransportProperties.Authenticator();
        List authScheme = new ArrayList();
        authScheme.add(HttpTransportProperties.Authenticator.NTLM);
        authenticator.setAuthSchemes(authScheme);
        authenticator.setUsername("Dealsheet");
        authenticator.setPassword("@#Vaultsheet");
        authenticator.setHost("stdoc1");
        authenticator.setDomain("SEMPRATRADING");
        options.setProperty(org.apache.axis2.transport.http.HTTPConstants.AUTHENTICATE, authenticator);
        options.setUserName("Dealsheet");
        options.setPassword("@#Vaultsheet");
        stub._getServiceClient().setOptions(options);
        SempraDocWSStub.Hello hello = new SempraDocWSStub.Hello();
        SempraDocWSStub.HelloResponse resp = stub.Hello(hello);
        String respValue = resp.getHelloResult();
        System.out.println(respValue);         


        Properties env = new Properties( );
        env.put(Context.PROVIDER_URL, "localhost:1099");
        env.put(Context.URL_PKG_PREFIXES, "org.jboss.naming:org.jnp.interfaces");
        env.put(Context.INITIAL_CONTEXT_FACTORY,"org.jnp.interfaces.NamingContextFactory");


        ctx = new InitialContext(env);
        adoAssDocBean = (AssociatedDocsDAORemote) ctx.lookup("AssociatedDocsDAOBean/remote");
  //      adoTradeSummaryBean = (TradeSummaryDAORemote) ctx.lookup("TradeSummaryDAOBean/remote");


 *//*       List<TradeSummaryEntity> list;
        TradeSummaryEntity template = new TradeSummaryEntity();
        long l = 4229163;
        template.setId(new Long(l));
        list = adoTradeSummaryBean.findByExample(template, "");   *//*

        List<AssociatedDocsEntity> list;
        AssociatedDocsEntity template = new AssociatedDocsEntity();

        template.setVaultedFlag("N");

        
   //     long l = 84809;
 //       template.setId(new Long(l));
        list = adoAssDocBean.findByExample(template, "");




        System.out.println("Total Found: " + list.size());*/
    }
}
