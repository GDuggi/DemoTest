package aff.confirm.updateconfirmstatus.mdb;

import aff.confirm.updateconfirmstatus.mdb.jaxb.ConfirmationsManager;
import aff.confirm.updateconfirmstatus.mdb.jaxb.ConfirmationsManager_Service;
import configuration.SystemSettingsProvider;

import javax.annotation.PostConstruct;
import javax.annotation.Resource;
import javax.ejb.MessageDrivenContext;
import javax.jms.JMSException;
import javax.jms.Message;
import javax.jms.MessageListener;
import javax.jms.TextMessage;
import javax.sql.DataSource;
import javax.xml.namespace.QName;
import javax.xml.ws.BindingProvider;
import java.net.URL;

/**
 * User: hblumenf
 * Date: 11/23/2015
 * Time: 12:38 PM
 * Copyright Amphora Inc. 2015
 */
public abstract class Abstract0UpdateConfirmStatusMDB implements MessageListener
{
    @Resource
    MessageDrivenContext ctx;
    @Resource(lookup = "java:jboss/datasources/Aff.SqlSvr.DS")

    protected DataSource dataSource;
    private ConfirmationsManager_Service confMagr_svcr;
    private ConfirmationsManager _confirmationsManager=null;

    @PostConstruct
    void startUp() throws Exception {
        UpdateConfirmStatusMDB.log.info("UpdateConfirmStatusMDB.start");
        if( dataSource == null )
            throw new IllegalStateException( "dataSource not injected");

        UpdateConfirmStatusMDB.log.info("UpdateConfirmStatusMDB.start done");
           // confMagr_svcr = new ConfirmationsManager_Service();
        /*String urlStr= SystemSettingsProvider.INSTANCE.get("confirmationsManagerURL", "");
        if(urlStr == null || urlStr=="")
        {
            log.info("Error: There is no confirmations manager URL");
        }
        else
        {
            URL url = new URL(urlStr);
            QName qName = new QName("ConfirmationsManager");
            confMagr_svcr = new ConfirmationsManager_Service(url,qName);
        }*/
        }

    @Override
    public void onMessage(Message message) {
        UpdateConfirmStatusMDB.log.info("UpdateConfirmStatusMDB.onMessage invoked");
        try {
            if (message instanceof TextMessage) {
                TextMessage textMessage = (TextMessage) message;
                String msg = textMessage.getText();
                processMessge(msg);
            }
        } catch (JMSException e) {
            UpdateConfirmStatusMDB.log.severe(e.getMessage());
        } finally {
        }
    }

    public abstract void processMessge(String message);

    public ConfirmationsManager getConfirmationsManager()
    {
            if (_confirmationsManager == null) {

                String urlStr = SystemSettingsProvider.INSTANCE.get("confirmationsManagerWebServiceURL", "");
                if (urlStr == null || urlStr == "") {
                    UpdateConfirmStatusMDB.log.severe("Error: Missing ConfirmationsManagerURL entry in Standalone.xml  <system-properties> section. Ex: <property name=\"confirmationsManagerWebServiceURL\" value=\"http://localhost:11111/ConfirmationsManager\"/> ");
                } else {
                    _confirmationsManager = getConfirmationsManagerService().getBasicHttpBindingConfirmationsManager();
                    final BindingProvider authenticationServiceSoapBP = (BindingProvider) _confirmationsManager;
                    authenticationServiceSoapBP.getRequestContext().put(BindingProvider.ENDPOINT_ADDRESS_PROPERTY, urlStr);//Ex urlStr: "http://localhost:11111/ConfirmationsManager?wsdl");
                }
            }
            return _confirmationsManager;
    }

    public ConfirmationsManager_Service getConfirmationsManagerService()
    {
        if(confMagr_svcr==null)
        {
            URL
                    relativEURL =	 ConfirmationsManager_Service.class.getResource("jaxbCreation/ConfirmationsManager.wsdl");
            confMagr_svcr=new ConfirmationsManager_Service(relativEURL, new QName("http://cnf/ConfirmationsManager", "ConfirmationsManager"));
           // logger.log(Level.INFO, "Initialized AuthenticationService object from the WSDL file  :" + relativEURL);
       }
        return confMagr_svcr;
    }
}
