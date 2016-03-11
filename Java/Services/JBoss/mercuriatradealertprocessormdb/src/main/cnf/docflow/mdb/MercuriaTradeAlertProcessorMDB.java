package cnf.docflow.mdb;
/**
 * Created by jvega on 7/1/2015.
 */

import cnf.docflow.configuration.SystemSettingsProvider;
import cnf.docflow.dao.*;
import cnf.docflow.data.ConfirmMessageData;
import cnf.docflow.data.ConfirmRequirementData;
import cnf.docflow.data.ConfirmRequirementSendToData;
import cnf.docflow.util.*;
import org.jboss.ejb3.annotation.ResourceAdapter;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;

import javax.annotation.Resource;
import javax.ejb.*;
import javax.jms.*;
import javax.jms.Message;
import javax.mail.Session;
import javax.sql.DataSource;
import javax.xml.XMLConstants;
import javax.xml.transform.Source;
import javax.xml.transform.stream.StreamSource;
import javax.xml.validation.Schema;
import javax.xml.validation.SchemaFactory;
import javax.xml.validation.Validator;
import java.io.IOException;
import java.io.StringReader;
import java.net.URL;
import java.sql.Connection;
import java.sql.SQLException;
import java.text.ParseException;
import java.util.*;
import java.util.logging.Level;
import java.util.logging.Logger;

@MessageDriven(
            activationConfig={
                    //@ActivationConfigProperty(propertyName="destinationType", propertyValue="javax.jms.Queue"),
                    // The exported JNDI name defined in ejb-jar.xml
                    //prod
                    //@ActivationConfigProperty(propertyName="destination", propertyValue="mercuria.confirmsMgr.tradeNotification"),
                    //test
                    //@ActivationConfigProperty(propertyName="destination", propertyValue="jms/queue/sempra.sif.ping"),  // ""
                    //@ActivationConfigProperty(propertyName="useDLQ", propertyValue="false"),
                    //@ActivationConfigProperty(propertyName="maxSession", propertyValue="10"),

                    //************************* RECONNECT PARAMERTERS  *****************************
                    // @ActivationConfigProperty(propertyName="reconnectAttempts", propertyValue="60"),
                    //@ActivationConfigProperty(propertyName="reconnectInterval", propertyValue="10")
            }
    )
   @TransactionManagement(value= TransactionManagementType.CONTAINER)
   @TransactionAttribute(value= TransactionAttributeType.REQUIRED)
   //@ResourceAdapter("inf01-hornetq-ra.rar")
    public class MercuriaTradeAlertProcessorMDB implements MessageListener {


    public final static Logger log = Logger.getLogger(MercuriaTradeAlertProcessorMDB.class.toString());
    private static Boolean IsDebugMode;

    @Resource
    MessageDrivenContext ctx;

    @Resource(lookup = "java:jboss/datasources/Aff.SqlSvr.DS")
    private DataSource dataSource;

    @Resource(mappedName = "java:/Mail")
    private Session mailSession;

    public void onMessage(Message message) {
        log.log(Level.INFO, "Message has been Received by " + this.getClass().getSimpleName());
        java.sql.Connection conn = null;
        long delvryCnt = 0;
        long delvryCntMax = Long.parseLong(SystemSettingsProvider.INSTANCE.get(SystemSettingsProvider.Key.MAIL_MAXDELIVERYATTEMPTS, "10")); //temp setting
        String debugsetting = SystemSettingsProvider.INSTANCE.get(SystemSettingsProvider.Key.DEBUG_ENABLED, "TRUE");
        String hostName = SystemSettingsProvider.INSTANCE.get(SystemSettingsProvider.Key.JBOSS_HOST_NAME, "");
        String emailDomain = SystemSettingsProvider.INSTANCE.get(SystemSettingsProvider.Key.MAIL_DOMAIN, "");
        String emailfrom = this.getClass().getSimpleName() + "_" + hostName + emailDomain;
        String emailTo = SystemSettingsProvider.INSTANCE.get(SystemSettingsProvider.Key.MAIL_TO, "");
        log.log(Level.INFO, SystemSettingsProvider.Key.DEBUG_ENABLED + "=" + debugsetting);
        String inMsg = null;
        IsDebugMode = SetDebugMode(debugsetting);
        //if (IsDebugMode){
        //    SystemSettingsProvider.INSTANCE.listAllProperties(System.out);
        //}
        try {
            if (message instanceof TextMessage) {
                TextMessage textMessage = (TextMessage) message;
                try {
                    if (message.getJMSRedelivered()) {
                        delvryCnt = message.getLongProperty("JMSXDeliveryCount");
                        if (IsDebugMode) {
                            log.log(Level.INFO, "Message DeliveryCount=" + delvryCnt);
                        }
                    }
                    inMsg = textMessage.getText();
                    validateXML(inMsg);
                    if (!inMsg.isEmpty()) {
                        ConfirmMessageData objConfirmData = loadDatafromMessage(inMsg);
                        objConfirmData.validate();
                        if (IsDebugMode) {
                            log.log(Level.INFO, "Message validated.");
                        }

                        if (IsDebugMode) {
                            log.log(Level.INFO, "Getting Connection from dataSource.");
                        }
                        conn = dataSource.getConnection();
                        if (IsDebugMode) {
                            log.log(Level.INFO, "ProcessMessageData start");
                        }
                        ProcessMessageData(conn, objConfirmData);
                        if (IsDebugMode) {
                            log.log(Level.INFO, "ProcessMessageData end");
                        }
                    }
                }
                catch(InvalidMessageFormatException e) {
                    log.severe("onMessage InvalidMessageFormatException: " + e.getMessage());
                    SendFailNotification(this.getClass().getSimpleName(), mailSession, emailfrom, emailTo, inMsg, e.getMessage());
                }
                catch(TradeDoesNotExistException e) {
                    log.severe("onMessage TradeDoesNotExistException: " + e.getMessage());
                    SendFailNotification(this.getClass().getSimpleName(), mailSession, emailfrom, emailTo, inMsg, e.getMessage());
                }
                catch (JMSException e) {
                    log.severe("onMessage JMSException: " + e.getMessage());
                    e.printStackTrace();
                    ctx.setRollbackOnly();
                    if (delvryCnt>=delvryCntMax) {
                        SendFailNotification(this.getClass().getSimpleName(), mailSession, emailfrom, emailTo, inMsg, e.getMessage());
                    }
                } catch (SAXException e) {
                    log.severe("onMessage SAXException: " + e.getMessage());
                    e.printStackTrace();
                    ctx.setRollbackOnly();
                    if (delvryCnt>=delvryCntMax) {
                        SendFailNotification(this.getClass().getSimpleName(), mailSession, emailfrom, emailTo, inMsg, e.getMessage());
                    }
                } catch (IOException e) {
                    log.severe("onMessage IOException: " + e.getMessage());
                    e.printStackTrace();
                    ctx.setRollbackOnly();
                    if (delvryCnt>=delvryCntMax) {
                        SendFailNotification(this.getClass().getSimpleName(), mailSession, emailfrom, emailTo, inMsg, e.getMessage());
                    }
                } catch (SQLException e) {
                    log.severe("onMessage SQLException: " + e.getMessage());
                    e.printStackTrace();
                    ctx.setRollbackOnly();
                    if (delvryCnt>=delvryCntMax) {
                        SendFailNotification(this.getClass().getSimpleName(), mailSession, emailfrom, emailTo, inMsg, e.getMessage());
                    }
                } catch (java.lang.Exception e) {
                    log.severe("onMessage Exception: " + e.getMessage());
                    e.printStackTrace();
                    ctx.setRollbackOnly();
                    if (delvryCnt>=delvryCntMax) {
                        SendFailNotification(this.getClass().getSimpleName(), mailSession, emailfrom, emailTo, inMsg, e.getMessage());
                    }
                }
            }
        } finally {
            try {
                if (conn != null && !conn.isClosed()) {
                    conn.close();
                }
            } catch (Exception e){
            }

        }
    }

    private Boolean SetDebugMode(String envSetting) {
        if (envSetting.equalsIgnoreCase((DebuggingType.Enabled.getCode()))) {
            return true;
        } else {
            return false;
        }
    }

    private void validateXML(String xmlMsg) throws InvalidMessageFormatException{
        if (IsDebugMode) {
            log.log(Level.INFO, "Message format - Validating...");
        }
        SchemaFactory schemaFactory = SchemaFactory.newInstance(XMLConstants.W3C_XML_SCHEMA_NS_URI);
        Source xmlFile = new StreamSource(new StringReader(xmlMsg));
        try {
            Source xsd = new StreamSource(getClass().getClassLoader().getResourceAsStream("cnf/docflow/resources/Mercuria_ConfirmMessage.xsd"));
            Schema schema = schemaFactory.newSchema(xsd);
            Validator validator = schema.newValidator();
            validator.validate(xmlFile);
            if (IsDebugMode) {
                log.log(Level.INFO, "Message format - Valid.");
            }
        } catch (Exception e) {
            throw new InvalidMessageFormatException("\n\r"+e.getMessage());
        }
    }

    private static ConfirmMessageData loadDatafromMessage(String xmlMsg) throws SAXException, IOException {
        Document xmlDoc = null;
        ConfirmMessageData objMsgData = new ConfirmMessageData();
        try {
            xmlDoc = XmlUtils.loadXMLfromMessage(xmlMsg);
            xmlDoc.getDocumentElement().normalize();
            Node nRootNode = xmlDoc.getDocumentElement();
            if (nRootNode.getNodeName().compareToIgnoreCase(MessageSectionType.ConfirmMessage.getCode()) > -1) {
                if (IsDebugMode) {
                    log.log(Level.INFO, "Root Message element: " + nRootNode.getNodeName());
                }

                NodeList nlConfirmData = xmlDoc.getElementsByTagName(MessageSectionType.ConfirmMessage.getCode());
                if (IsDebugMode) {
                    log.log(Level.INFO, "ConfirmMessage(" + nlConfirmData.getLength() + ")");
                }
                for (int x = 0; x < nlConfirmData.getLength(); x++) {

                    Node nConfirmNode = nlConfirmData.item(x);
                    if (nConfirmNode.getNodeType() == nConfirmNode.ELEMENT_NODE) {

                        Element eElement = (Element) nConfirmNode;

                        objMsgData.setConfirm_NotifyType(XmlUtils.getMessageItem(eElement, MessageTagType.NotifyType.getCode()));
                        objMsgData.setConfirm_NotifyTsGmt(XmlUtils.getMessageItem(eElement, MessageTagType.NotifyTsGmt.getCode()));
                    }

                    NodeList nlTradeData = xmlDoc.getElementsByTagName(MessageSectionType.TradeData.getCode());
                    if (IsDebugMode) {
                        log.log(Level.INFO, "TradeNodes(" + nlTradeData.getLength() + ")");
                    }
                    for (int i = 0; i < nlTradeData.getLength(); i++) {
                        Node nTradeNode = nlTradeData.item(i);

                        if (nTradeNode.getNodeType() == Node.ELEMENT_NODE) {

                            Element eElement = (Element) nTradeNode;

                            objMsgData.setTrade_TradingSystemCode(XmlUtils.getMessageItem(eElement, MessageTagType.TradingSystemCode.getCode()));
                            objMsgData.setTrade_TradingSystemTicket(XmlUtils.getMessageItem(eElement, MessageTagType.TradingSystemTicket.getCode()));
                            objMsgData.setTrade_CreationDt(XmlUtils.getMessageItem(eElement, MessageTagType.CreationDt.getCode()));
                            objMsgData.setTrade_CdtySn(XmlUtils.getMessageItem(eElement, MessageTagType.CdtySn.getCode()));
                            objMsgData.setTrade_CdtyGroup(XmlUtils.getMessageItem(eElement, MessageTagType.CdtyGroup.getCode()));
                            objMsgData.setTrade_TradeDt(XmlUtils.getMessageItem(eElement, MessageTagType.TradeDt.getCode()));
                            objMsgData.setTrade_Xref(XmlUtils.getMessageItem(eElement, MessageTagType.Xref.getCode()));
                            objMsgData.setTrade_CptySn(XmlUtils.getMessageItem(eElement, MessageTagType.CptySn.getCode()));
                            objMsgData.setTrade_CptyLegalName(XmlUtils.getMessageItem(eElement, MessageTagType.CptyLegalName.getCode()));
                            objMsgData.setTrade_CptyId(XmlUtils.getMessageItem(eElement, MessageTagType.CptyId.getCode()));
                            objMsgData.setTrade_QtyTot(XmlUtils.getMessageItem(eElement, MessageTagType.QtyTot.getCode()));
                            objMsgData.setTrade_LocationSn(XmlUtils.getMessageItem(eElement, MessageTagType.LocationSn.getCode()));
                            objMsgData.setTrade_PriceDesc(XmlUtils.getMessageItem(eElement, MessageTagType.PriceDesc.getCode()));
                            objMsgData.setTrade_StartDt(XmlUtils.getMessageItem(eElement, MessageTagType.StartDt.getCode()));
                            objMsgData.setTrade_EndDt(XmlUtils.getMessageItem(eElement, MessageTagType.EndDt.getCode()));
                            objMsgData.setTrade_Book(XmlUtils.getMessageItem(eElement, MessageTagType.Book.getCode()));
                            objMsgData.setTrade_TradeTypeCode(XmlUtils.getMessageItem(eElement, MessageTagType.TradeTypeCode.getCode()));
                            objMsgData.setTrade_SttlType(XmlUtils.getMessageItem(eElement, MessageTagType.SttlType.getCode()));
                            objMsgData.setTrade_BrokerSn(XmlUtils.getMessageItem(eElement, MessageTagType.BrokerSn.getCode()));
                            objMsgData.setTrade_BrokerLegalName(XmlUtils.getMessageItem(eElement, MessageTagType.BrokerLegalName.getCode()));
                            objMsgData.setTrade_BrokerId(XmlUtils.getMessageItem(eElement, MessageTagType.BrokerId.getCode()));
                            objMsgData.setTrade_BuySellInd(XmlUtils.getMessageItem(eElement, MessageTagType.BuySellInd.getCode()));
                            objMsgData.setTrade_RefSn(XmlUtils.getMessageItem(eElement, MessageTagType.RefSn.getCode()));
                            objMsgData.setTrade_BookingCompany(XmlUtils.getMessageItem(eElement, MessageTagType.BookingCompany.getCode()));
                            objMsgData.setTrade_BookingCompanyId(XmlUtils.getMessageItem(eElement, MessageTagType.BookingCompanyId.getCode()));
                            objMsgData.setTrade_ProfitCenter(XmlUtils.getMessageItem(eElement, MessageTagType.ProfitCenter.getCode()));
                            objMsgData.setTrade_TradeStatCode(XmlUtils.getMessageItem(eElement, MessageTagType.TradeStatCode.getCode()));
                            objMsgData.setTrade_BrokerPriceDesc(XmlUtils.getMessageItem(eElement, MessageTagType.BrokerPriceDesc.getCode()));
                            objMsgData.setTrade_OptnStrikePrice(XmlUtils.getMessageItem(eElement, MessageTagType.OptnStrikePrice.getCode()));
                            objMsgData.setTrade_OptnPremPrice(XmlUtils.getMessageItem(eElement, MessageTagType.OptnPremPrice.getCode()));
                            objMsgData.setTrade_OptnPutCallInd(XmlUtils.getMessageItem(eElement, MessageTagType.OptnPutCallInd.getCode()));
                            objMsgData.setTrade_PermissionKey(XmlUtils.getMessageItem(eElement, MessageTagType.PermissionKey.getCode()));
                            objMsgData.setTrade_QtyDesc(XmlUtils.getMessageItem(eElement, MessageTagType.QtyDesc.getCode()));
                            objMsgData.setTrade_TradeDesc(XmlUtils.getMessageItem(eElement, MessageTagType.TradeDesc.getCode()));
                            objMsgData.setTrade_Trader(XmlUtils.getMessageItem(eElement, MessageTagType.Trader.getCode()));
                            objMsgData.setTrade_TransportDesc(XmlUtils.getMessageItem(eElement, MessageTagType.TransportDesc.getCode()));
                        }
                    }

                    //Rqmts
                    NodeList nlRqmtsData = xmlDoc.getElementsByTagName(MessageSectionType.Rqmt.getCode());
                    if (IsDebugMode) {
                        log.log(Level.INFO, "RqmtNodes(" + nlRqmtsData.getLength() + ")");
                    }
                    List<ConfirmRequirementData> confirmRequirementDataList = new LinkedList<ConfirmRequirementData>();
                    for (int i = 0; i < nlRqmtsData.getLength(); i++) {
                        Node nRqmtNode = nlRqmtsData.item(i);

                        if (nRqmtNode.getNodeType() == Node.ELEMENT_NODE) {

                            Element eRqmtNode = (Element) nRqmtNode;

                            ConfirmRequirementData objRequirementData = new ConfirmRequirementData();

                            objRequirementData.setRqmt_Workflow(XmlUtils.getMessageItem(eRqmtNode, MessageTagType.Workflow.getCode()));
                            objRequirementData.setRqmt_Template(XmlUtils.getMessageItem(eRqmtNode, MessageTagType.Template.getCode()));
                            objRequirementData.setRqmt_PreparerCanSend(XmlUtils.getMessageItem(eRqmtNode, MessageTagType.PreparerCanSend.getCode()));

                            //SendTos
                                NodeList nlSendTosData = eRqmtNode.getElementsByTagName(MessageSectionType.SendTo.getCode());
                                if (IsDebugMode) {
                                   log.log(Level.INFO, "SendToNodes(" + nlSendTosData.getLength() + ")");
                                }
                                List<ConfirmRequirementSendToData> confirmRequirementSendToDataList = new LinkedList<ConfirmRequirementSendToData>();
                                for (int j = 0; j < nlSendTosData.getLength(); j++) {
                                    Node nSendToData = nlSendTosData.item(j);

                                    if (nSendToData.getNodeType() == Node.ELEMENT_NODE) {

                                        Element eSendToData = (Element) nSendToData;

                                        ConfirmRequirementSendToData objRequirementSendToData = new ConfirmRequirementSendToData();

                                        objRequirementSendToData.setRqmtSendTo_transmitMethodInd(XmlUtils.getMessageItem(eSendToData, MessageTagType.transmitMethodInd.getCode()));
                                        objRequirementSendToData.setRqmtSendTo_faxCountryCode(XmlUtils.getMessageItem(eSendToData, MessageTagType.faxCountryCode.getCode()));
                                        objRequirementSendToData.setRqmtSendTo_faxAreaCode(XmlUtils.getMessageItem(eSendToData, MessageTagType.faxAreaCode.getCode()));
                                        objRequirementSendToData.setRqmtSendTo_faxLocalNumber(XmlUtils.getMessageItem(eSendToData, MessageTagType.faxLocalNumber.getCode()));
                                        objRequirementSendToData.setRqmtSendTo_emailAddress(XmlUtils.getMessageItem(eSendToData, MessageTagType.emailAddress.getCode()));
                                        confirmRequirementSendToDataList.add(objRequirementSendToData);
                                    }

                                }
                            if (!confirmRequirementSendToDataList.isEmpty()) {
                                objRequirementData.setConfRqmtSendToList(confirmRequirementSendToDataList);
                            }
                            confirmRequirementDataList.add(objRequirementData);
                        }
                    }
                    if (!confirmRequirementDataList.isEmpty()) {
                        objMsgData.setConfRqmtList(confirmRequirementDataList);
                    }
                    if (IsDebugMode) {
                        log.log(Level.INFO, "ConfirmMessageData = " + objMsgData.toString());
                    }
                }
            } else {
                if (IsDebugMode) {
                    log.log(Level.INFO, "Not a ConfirmMessage");
                }
            }

        } catch (SAXException e) {
            objMsgData = null;
            throw (e);
        } catch (IOException e) {
            objMsgData = null;
            throw (e);
        }
        return objMsgData;
    }


    private static void ProcessMessageData(Connection conn, ConfirmMessageData msgData) throws SQLException, ParseException, TradeDoesNotExistException {
        if (IsDebugMode) {
            log.log(Level.INFO, "Trade Notify Type: "+ msgData.getConfirm_NotifyType());
        }

        //NEW
        if (msgData.getConfirm_NotifyType().equalsIgnoreCase(TradeNotifyType.NEW.getCode())) {
            TradeDAO tradeDAO = new TradeDAO(conn);
            if (!tradeDAO.alreadyExists(msgData.getTrade_TradingSystemTicket(), msgData.getTrade_TradingSystemCode())) {
                if (IsDebugMode) {
                    log.log(Level.INFO, "TRADE - Saving Data for Code: " + msgData.getTrade_TradingSystemCode() + " Ticket: " + msgData.getTrade_TradingSystemTicket());
                }
                msgData.setOther_TradeID(tradeDAO.InsertData(msgData));
                if (IsDebugMode) {
                    log.log(Level.INFO, "TRADE - Done.");
                }

                TradeNotifyDAO tradeNotifyDAO = new TradeNotifyDAO(conn);
                if (!tradeNotifyDAO.alreadyExists(msgData.getTrade_TradingSystemTicket(), msgData.getTrade_TradingSystemCode())) {
                    if (IsDebugMode) {
                        log.log(Level.INFO, "TRADE_NOTIFY - Saving Data for Code: " + msgData.getTrade_TradingSystemCode() + " Ticket: " + msgData.getTrade_TradingSystemTicket());
                    }
                    msgData.setOther_TradeNotifyID(tradeNotifyDAO.InsertData(msgData));
                    if (IsDebugMode) {
                        log.log(Level.INFO, "TRADE_NOTIFY - Done.");
                    }
                } else {
                    if (IsDebugMode) {
                        log.log(Level.WARNING, "TRADE_NOTIFY Already Exists for Code: " + msgData.getTrade_TradingSystemCode() + " Ticket: " + msgData.getTrade_TradingSystemTicket());
                    }
                }

                TradeDataDAO tradeDataDAO = new TradeDataDAO(conn);
                if (!tradeDataDAO.alreadyExists(msgData.getTrade_TradingSystemTicket(), msgData.getTrade_TradingSystemCode())) {
                    if (IsDebugMode) {
                        log.log(Level.INFO, "TRADE_DATA - Saving Data for Code: " + msgData.getTrade_TradingSystemCode() + " Ticket: " + msgData.getTrade_TradingSystemTicket());
                    }
                    tradeDataDAO.InsertData(msgData);
                    if (IsDebugMode) {
                        log.log(Level.INFO, "TRADE_DATA - Done.");
                    }
                } else {
                    if (IsDebugMode) {
                        log.log(Level.WARNING, "TRADE_DATA Already Exists for Code: " + msgData.getTrade_TradingSystemCode() + " Ticket: " + msgData.getTrade_TradingSystemTicket());
                    }
                }

                TradeRqmtDAO tradeRqmtDAO = new TradeRqmtDAO(conn);
                if (!tradeRqmtDAO.alreadyExists(msgData.getTrade_TradingSystemTicket(), msgData.getTrade_TradingSystemCode())) {
                    if (IsDebugMode) {
                        log.log(Level.INFO, "TRADE_RQMT - Saving Data for Code: " + msgData.getTrade_TradingSystemCode() + " Ticket: " + msgData.getTrade_TradingSystemTicket());
                    }
                    tradeRqmtDAO.InsertData(msgData);
                    if (IsDebugMode) {
                        log.log(Level.INFO, "TRADE_RQMT - Done.");
                    }
                } else {
                    if (IsDebugMode) {
                        log.log(Level.WARNING, "TRADE_RQMT Already Exists for Code: " + msgData.getTrade_TradingSystemCode() + " Ticket: " + msgData.getTrade_TradingSystemTicket());
                    }
                }

                TradeRqmtConfirmDAO tradeRqmtConfirmDAO = new TradeRqmtConfirmDAO(conn);
                if (!tradeRqmtConfirmDAO.alreadyExists(msgData.getTrade_TradingSystemTicket(), msgData.getTrade_TradingSystemCode())) {
                    if (IsDebugMode) {
                        log.log(Level.INFO, "TRADE_RQMT_CONFIRM - Saving Data for Code: " + msgData.getTrade_TradingSystemCode() + " Ticket: " + msgData.getTrade_TradingSystemTicket());
                    }
                    tradeRqmtConfirmDAO.InsertData(msgData);
                    if (IsDebugMode) {
                        log.log(Level.INFO, "TRADE_RQMT_CONFIRM - Done.");
                    }
                } else {
                    if (IsDebugMode) {
                        log.log(Level.WARNING, "TRADE_RQMT_CONFIRM Already Exists for Code: " + msgData.getTrade_TradingSystemCode() + " Ticket: " + msgData.getTrade_TradingSystemTicket());
                    }
                }

                TradeSummaryDAO tradeSummaryDAO = new TradeSummaryDAO(conn);
                if (!tradeSummaryDAO.alreadyExists(msgData.getTrade_TradingSystemTicket(), msgData.getTrade_TradingSystemCode())) {
                    if (IsDebugMode) {
                        log.log(Level.INFO, "TRADE_SUMMARY - Saving Data for Code: " + msgData.getTrade_TradingSystemCode() + " Ticket: " + msgData.getTrade_TradingSystemTicket());
                    }
                    tradeSummaryDAO.InsertData(msgData);
                    if (IsDebugMode) {
                        log.log(Level.INFO, "TRADE_SUMMARY - Done.");
                    }
                } else {
                    if (IsDebugMode) {
                        log.log(Level.WARNING, "TRADE_SUMMARY Already Exists for Code: " + msgData.getTrade_TradingSystemCode() + " Ticket: " + msgData.getTrade_TradingSystemTicket());
                    }
                }
            } else {
                if (IsDebugMode) {
                    log.log(Level.WARNING, "TRADE Already Exists for Code: " + msgData.getTrade_TradingSystemCode() + " Ticket: " + msgData.getTrade_TradingSystemTicket());
                }
            }

        }
        else {
            //EDIT or VOID
            TradeDataDAO tradeDataDAO = new TradeDataDAO(conn);
            if (tradeDataDAO.alreadyExists(msgData.getTrade_TradingSystemTicket(), msgData.getTrade_TradingSystemCode())) {
                if (IsDebugMode) {
                    log.log(Level.INFO, "TRADE_NOTIFY - " + msgData.getConfirm_NotifyType() + " for Code: " + msgData.getTrade_TradingSystemCode() + " Ticket: " + msgData.getTrade_TradingSystemTicket());
                }

                TradeNotifyDAO tradeNotifyDAO = new TradeNotifyDAO(conn);
                msgData.setOther_TradeNotifyID(tradeNotifyDAO.InsertData(msgData));
                if (IsDebugMode) {
                    log.log(Level.INFO, "TRADE_NOTIFY - Done.");
                }

                if (IsDebugMode) {
                    log.log(Level.INFO, "TRADE_DATA - " + msgData.getConfirm_NotifyType() + " for Code: " + msgData.getTrade_TradingSystemCode() + " Ticket: " + msgData.getTrade_TradingSystemTicket());
                }
                //Only for Update
                if (msgData.getConfirm_NotifyType().equalsIgnoreCase(TradeNotifyType.EDIT.getCode())) {
                    tradeDataDAO.UpdateData(msgData);
                    if (IsDebugMode) {
                        log.log(Level.INFO, "TRADE_DATA - Done.");
                    }
                }
                //Only for VOID
                else if (msgData.getConfirm_NotifyType().equalsIgnoreCase(TradeNotifyType.VOID.getCode()) ||
                        msgData.getConfirm_NotifyType().equalsIgnoreCase(TradeNotifyType.CANCEL.getCode())) {

                    tradeDataDAO.VoidData(msgData);
                    if (IsDebugMode) {
                        log.log(Level.INFO, "TRADE_DATA - Done.");
                    }
                    if (IsDebugMode) {
                        log.log(Level.INFO, "TRADE_RQMT - " + msgData.getConfirm_NotifyType() + " Data for Code: " + msgData.getTrade_TradingSystemCode() + " Ticket: " + msgData.getTrade_TradingSystemTicket());
                    }
                    TradeRqmtDAO tradeRqmtDAO = new TradeRqmtDAO(conn);
                    tradeRqmtDAO.CancelAllRqmts(msgData);
                    if (IsDebugMode) {
                        log.log(Level.INFO, "TRADE_RQMT - Done.");
                    }
                }
                if (IsDebugMode) {
                    log.log(Level.INFO, "TRADE_APPR - " + msgData.getConfirm_NotifyType() + " for Code: " + msgData.getTrade_TradingSystemCode() + " Ticket: " + msgData.getTrade_TradingSystemTicket());
                }

                TradeApprDAO tradeApprDAO = new TradeApprDAO(conn);
                if (tradeApprDAO.getCanInsert(msgData)) {
                    tradeApprDAO.InsertData(msgData);
                    if (IsDebugMode) {
                        log.log(Level.INFO, "TRADE_APPR - Done.");
                    }
                } else {
                    if (IsDebugMode) {
                        log.log(Level.INFO, "TRADE_APPR - Not Done; Not Applicable. ");
                    }
                }

                if (IsDebugMode) {
                    log.log(Level.INFO, "TRADE_SUMMARY - Update for Code: " + msgData.getTrade_TradingSystemCode() + " Ticket: " + msgData.getTrade_TradingSystemTicket());
                }
                TradeSummaryDAO tradeSummaryDAO = new TradeSummaryDAO(conn);
                tradeSummaryDAO.UpdateData(msgData);
                if (IsDebugMode) {
                    log.log(Level.INFO, "TRADE_SUMMARY - Done.");
                }

            } else {
                String msg = "TRADE Data Not Found for Code: " + msgData.getTrade_TradingSystemCode() + " Ticket: " + msgData.getTrade_TradingSystemTicket();
                throw new TradeDoesNotExistException(msg);
            }

        }

    }


    //for testing only
    private static void DeleteTradeData(Connection conn, ConfirmMessageData msgData) throws SQLException, ParseException {

        if (msgData.getConfirm_NotifyType().equalsIgnoreCase(TradeNotifyType.NEW.getCode())) {
            if (IsDebugMode) {
                log.log(Level.INFO, "Deleting All Data for Code: " + msgData.getTrade_TradingSystemCode() + " Ticket: " + msgData.getTrade_TradingSystemTicket());
            }
            //reverse order
            TradeSummaryDAO tradeSummaryDAO = new TradeSummaryDAO(conn);
            if (tradeSummaryDAO.alreadyExists(msgData.getTrade_TradingSystemTicket(), msgData.getTrade_TradingSystemCode())) {
                tradeSummaryDAO.DeleteData(msgData);
                if (IsDebugMode) {
                    log.log(Level.INFO, "New TRADE_SUMMARY Deleted.");
                }
            }

            TradeRqmtDAO tradeRqmtDAO = new TradeRqmtDAO(conn);
            if (tradeRqmtDAO.alreadyExists(msgData.getTrade_TradingSystemTicket(), msgData.getTrade_TradingSystemCode())) {
                tradeRqmtDAO.DeleteData(msgData);
                if (IsDebugMode) {
                    log.log(Level.INFO, "New TRADE_RQMT Deleted.");
                }
            }

            TradeDataDAO tradeDataDAO = new TradeDataDAO(conn);
            if (tradeDataDAO.alreadyExists(msgData.getTrade_TradingSystemTicket(), msgData.getTrade_TradingSystemCode())) {
                tradeDataDAO.DeleteData(msgData);
                if (IsDebugMode) {
                    log.log(Level.INFO, "New TRADE_DATA Deleted.");
                }
            }

            TradeNotifyDAO tradeNotifyDAO = new TradeNotifyDAO(conn);
            if (tradeNotifyDAO.alreadyExists(msgData.getTrade_TradingSystemTicket(), msgData.getTrade_TradingSystemCode())) {
                tradeNotifyDAO.DeleteData(msgData);
                if (IsDebugMode) {
                    log.log(Level.INFO, "New TRADE_NOTIFY Deleted.");
                }
            }

            TradeDAO tradeDAO = new TradeDAO(conn);
            if (tradeDAO.alreadyExists(msgData.getTrade_TradingSystemTicket(), msgData.getTrade_TradingSystemCode())) {
                tradeDAO.DeleteData(msgData);
                if (IsDebugMode) {
                    log.log(Level.INFO, "New TRADE Deleted.");
                }
            }
        }
    }

    private static void SendFailNotification(String processName, Session mailSession, String emailFrom, String emailTo, String Msg, String errMsg) {
        try {
            if (mailSession != null) {
                log.log(Level.INFO, "Failed Message notification sent from(" + emailFrom + ") to(" + emailTo + ")");
                EmailUtils.sendMail(mailSession, emailFrom, emailTo, "Failed Message Notification", "Failed to process the message received by " + processName + ".  \r\n"+errMsg, Msg);//,fileAttachments);
            }
        } catch (Exception e) {
            log.log(Level.SEVERE, "Could not send Failed Message notification.");
        }
    }


}