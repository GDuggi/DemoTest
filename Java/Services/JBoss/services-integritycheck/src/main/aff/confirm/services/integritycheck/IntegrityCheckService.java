/**
 * User: islepini
 * Date: Sep 16, 2003
 * Time: 1:10:46 PM
 * To change this template use Options | File Templates.
 */
package aff.confirm.services.integritycheck;

import aff.confirm.jboss.common.service.taskservice.TaskService;
import aff.confirm.jboss.common.exceptions.StopServiceException;
import aff.confirm.jboss.common.exceptions.LogException;
import aff.confirm.jboss.common.exceptions.SQLConnectionAllocationFailure;
import org.w3c.dom.Element;
import org.w3c.dom.NodeList;
import org.w3c.dom.Node;

import org.jboss.logging.Logger;

import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.ejb.Singleton;
import javax.ejb.Startup;
import javax.naming.NamingException;
import java.util.LinkedList;
import java.io.*;
import java.sql.*;
import java.net.InetAddress;

@Startup
@Singleton
public class IntegrityCheckService extends TaskService implements IntegrityCheckServiceMBean {
    private static final String _EMAIL_MODULE_NAME = "INTEGRITY_CHECK";
    private static final String _DEFAULT_EMAIL_ADDRESS = "ifrankel@amphorainc.com";

    private static String dataDir = System.getProperty("jboss.server.config.dir")+"/affinity/";
    private String monitorQueries;
    private LinkedList monitorQueryList = new LinkedList();
    private String smtpHost;
    private String smtpPort;

    private String devEmailAddress = null;
    private String emailAddress = null;
    private String sentFromName;
    private String sentFromAddress;
    private String sendToName;

    // 7-23-08:  MThoresen - Auto reset credit margin and email notifications...
    private boolean autoResetCreditMargin = true;
    private int currentCreditMarginResets = 0;
    private int maxCreditMarginResets;


    public IntegrityCheckService() {
        super("affinity.utils:service=IntegrityCheckService");
    }

    @PostConstruct
    public void postConstruct() throws Exception {
        super.postConstruct();
    }

    @PreDestroy
    public void preDestroy() throws Exception {
        super.preDestroy();
    }

    public int getMaxCreditMarginResets() {
        return maxCreditMarginResets;
    }

    public void setMaxCreditMarginResets(int maxCreditMarginResets) {
        this.maxCreditMarginResets = maxCreditMarginResets;
    }

    public boolean getAutoResetCreditMargin(){
        return this.autoResetCreditMargin;
    }

    public void setAutoResetCreditMargin(boolean autoResetCreditMargin){
        this.autoResetCreditMargin = autoResetCreditMargin;
    }

    public String getSmtpHost(){
        return smtpHost;
    }
    public void setSmtpHost(String smtpHost){
        this.smtpHost = smtpHost;
    }

    public String getSmtpPort(){
        return smtpPort;
    }
    public void setSmtpPort(String smtpPort) {
        this.smtpPort = smtpPort;
    }

    public String getMonitorQueries() {
        return monitorQueries;
    }
    public void setMonitorQueries(String monitorQueries) throws  Exception{
        parseMonitorQueries(monitorQueries);
        this.monitorQueries = monitorQueries;
    }

    private void parseMonitorQueries(String element) throws Exception {

        element = element.trim();
        String queries[] = element.split((";"));
        if (queries != null) {
            for (int i=0;i<queries.length;++i){
                String queueInfo = queries[i];
                if ( queueInfo != null ) {
                    String[] queueDetails =  queueInfo.split(",");
                    String name = queueDetails[0].trim();
                    String limit = queueDetails[1];
                    log.info(name);
                    StringBuffer sql= new StringBuffer();
                    BufferedReader br = null;
                    try {
                        br = new BufferedReader(new InputStreamReader(new FileInputStream(dataDir+name)));
                        while(br.ready()){
                            sql.append(br.readLine()+"\n");
                        }
                    } catch (FileNotFoundException e) {
                        log.error(e);
                        throw new Exception("File "+dataDir+name+" not found");
                    } catch (IOException e) {
                        log.error(e);
                        throw new Exception("Failed to read from file "+dataDir+name+" not found");
                    } finally{
                        if(br != null){
                            try {
                                br.close();
                            } catch (IOException e) {
                                log.error(e);
                            }
                            br  = null;
                        }
                    }

                    MonitorQuery mq = new MonitorQuery(name,sql.toString(),new Integer(limit).intValue());
                    monitorQueryList.add(mq);


                }
            }
        }
    }

     private void parseMonitorQueries(Element element) throws Exception {
        monitorQueryList.clear();
        String rootTag = element.getTagName();
        if (!rootTag.equals("queries"))
        {
            throw new Exception("Not a resource adapter deployment " +
            "descriptor because its root tag, '" +
            rootTag + "', is not 'queries'");
        }
        invokeChildren(element);
    }

    private void invokeChildren(Element element) throws Exception
    {
        NodeList children = element.getChildNodes();
        for (int i = 0; i < children.getLength(); ++i){
            Node node = children.item(i);
            if (node.getNodeType() == Node.ELEMENT_NODE)
            {
                Element child = (Element)node;
                String tag = child.getTagName();
                if (!child.hasAttribute("name"))
                {
                    throw new Exception("Not a resource adapter deployment " +
                    "descriptor because its tag, '" +
                    tag + "', has not attribute 'name'");
                }
                String name = child.getAttribute("name");
                if (!child.hasAttribute("treshhold"))
                {
                    throw new Exception("Not a resource adapter deployment " +
                    "descriptor because its tag, '" +
                    tag + "', has not attribute 'treshhold'");
                }
                String treshHold = child.getAttribute("treshhold");
                log.info(name);
                StringBuffer sql= new StringBuffer();
                BufferedReader br = null;
                try {
                    br = new BufferedReader(new InputStreamReader(new FileInputStream(dataDir+name)));
                    while(br.ready()){
                        sql.append(br.readLine()+"\n");
                    }
                } catch (FileNotFoundException e) {
                   log.error(e);
                   throw new Exception("File "+dataDir+name+" not found");
                } catch (IOException e) {
                   log.error(e);
                   throw new Exception("Faile to read from file "+dataDir+name+" not found");
                } finally{
                    if(br != null){
                        try {
                            br.close();
                        } catch (IOException e) {
                            log.error(e);
                        }
                        br  = null;
                    }
                }

                MonitorQuery mq = new MonitorQuery(name,sql.toString(),new Integer(treshHold).intValue());
                monitorQueryList.add(mq);
            }
        }
    }
    private void init() throws Exception {
        initMailValues();
    }

    private void initMailValues() throws Exception{

        Logger.getLogger(IntegrityCheckService.class).info("Loading Mail Info from Database.");
        String hostName = InetAddress.getLocalHost().getHostName().toUpperCase();
        sentFromName = "IntegrityChecker_" + hostName;
        sentFromAddress = "JBossOn" + hostName + "@sempratrading.com";
        sendToName = "IntegrityChecker_Recipients";
        sentFromAddress = "JBossOn" + hostName + "@sempratrading.com";
    }
    protected void onServiceStarting() throws Exception {
        init();
    }

    protected void onServiceStoping() {
        devEmailAddress = null ;
        emailAddress = null;
    }

    protected void runTask() throws StopServiceException,LogException {
        for (int i = 0; i < monitorQueryList.size(); i++) {
            int rowCount = 0;
            MonitorQuery monitorQuery = (MonitorQuery) monitorQueryList.get(i);
            Statement statement = null;
            ResultSet rs = null;
            try {
                statement = createStatement();
                rs = statement.executeQuery(monitorQuery.getSql());
                while(rs.next()){
                    rowCount++;
                }
                if(rowCount > monitorQuery.getTreshhold()){
                    if(monitorQuery.getName().equalsIgnoreCase("CREDITMARGIN.TXT")){
                        //CREDIT MARGIN THRESHOLD IS ALWAYS 0!!!
                        resetMarginRespProcFlag(monitorQuery, rowCount);
                    }
                    else{
                        throw new LogException("Warning. RowCount for query "+monitorQuery.getName()+" = "+rowCount+" , threshold "+monitorQuery.getTreshhold());
                    }
                } else {
                    if(monitorQuery.getName().equalsIgnoreCase("CREDITMARGIN.TXT")){
                        currentCreditMarginResets = 0;
                    }
                }
            } catch (SQLException e) {
                log.error(e);
                throw new StopServiceException(e.getMessage());
            } catch (SQLConnectionAllocationFailure sqlConnectionAllocationFailure) {
                log.error(sqlConnectionAllocationFailure);
                throw new StopServiceException(sqlConnectionAllocationFailure.getMessage());
            } finally{
                if(rs != null){
                    try {
                        rs.close();
                    } catch (SQLException e) {
                        log.error(e);
                    }
                    rs = null;
                }
                if(statement != null){
                    try {
                        statement.close();
                    } catch (SQLException e) {
                        log.error(e);
                    }
                    statement = null;
                }
            }
        }
    }

    public String resetMarginRespProcFlag(MonitorQuery monitorQuery, int rowCount) throws LogException, SQLException {
        if(autoResetCreditMargin){
            if(currentCreditMarginResets <= maxCreditMarginResets){
                try {
                    callResetMarginRespProcFlag();
                    getSqlConnection().commit();
                    currentCreditMarginResets = currentCreditMarginResets + 1;
                } catch (SQLConnectionAllocationFailure sqlConnectionAllocationFailure) {
                    log.error( "ERROR", sqlConnectionAllocationFailure );
                } catch (NamingException e) {
                    log.error( "ERROR", e );
                }
            }
            else{
                throw new LogException("Reset Credit Margin Response Flag has been re-set several times.  Please check: "+monitorQuery.getName()+" = "+rowCount+" , threshold "+monitorQuery.getTreshhold());
            }
        }
        return null;
    }

    private void callResetMarginRespProcFlag()
            throws SQLException, SQLConnectionAllocationFailure, NamingException {
        String sql = "{call infinity_mgr.ResetMarginRespProcFlag() }";
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = createPreparedStatement(sql);
            rs = statement.executeQuery();
        }  finally {
            if (rs != null) {
                try {
                    rs.close();
                } catch (SQLException e) {
                }
                rs = null;
            }
            if (statement != null) {
                try {
                    statement.close();
                } catch (SQLException e) {
                }
                statement = null;
            }
        }
    }
}
