package aff.confirm.opsmanager.creditmargin.common;


import aff.confirm.common.util.JndiUtil;
import aff.confirm.opsmanager.confirm.Confirmation;
import aff.confirm.opsmanager.confirm.data.ContractRequest;
import aff.confirm.opsmanager.confirm.data.ContractResponse;
import org.jboss.logging.Logger;

import javax.naming.NamingException;
import javax.swing.text.BadLocationException;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.URL;
import java.net.URLConnection;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * User: srajaman
 * Date: Dec 4, 2008
 * Time: 12:12:09 PM
 */
public class CreditMarginProcessor {
    private static Logger log = Logger.getLogger( CreditMarginProcessor.class );


    static {
         try {
             Class.forName("oracle.jdbc.OracleDriver");
         } catch (ClassNotFoundException e) {
             log.error( e );
         }

    }

    private static String _TRADER_NOTIFY_STATUS = "TRADER";

    private String creditStatusCheckUrl;
    private String creditTermUrl;
    private String contractBeanName = "OpsManagerCnf/Confirmation/remote";
    private TradeRqmtDAO dao;
    private Confirmation confirmation = null;
    private String marginToken;
    private String traderWebUrl;
    private RTFInserter rtfInserter;
    private String dbUserName;
    private String database;


    public CreditMarginProcessor(TradeRqmtDAO dao,String statusCheckUrl,String termUrl,String marginToken,String traderUrl,String ejbName,String userName,String dbName) throws NamingException {
        this.dao = dao;
        this.creditStatusCheckUrl = statusCheckUrl;
        this.creditTermUrl = termUrl;
        this.marginToken = marginToken;
        this.traderWebUrl = traderUrl;
        this.contractBeanName = ejbName;
        this.dbUserName = userName;
        this.database = dbName;
        rtfInserter = new RTFInserter();

    }

    private void createContractEJB() throws NamingException {
        Object obj = JndiUtil.lookup(contractBeanName);
        confirmation = (Confirmation) obj;
    }

    public void processCreditMarginNotification(String tradingSystem, long tradeId, int version,String msg) throws IOException, NamingException, BadLocationException, SQLException {

        if (confirmation == null){
            createContractEJB();
        }
        String tradeStatus = "";
        tradeStatus = getStatusFromCredit(tradingSystem,tradeId,version);
        CreditLogRec clr = updateCreditMarginInVault(tradingSystem,tradeId,version,tradeStatus);
        clr.setMsg(msg);
        dao.insertIntoLog(clr);


    }

    private CreditLogRec updateCreditMarginInVault(String tradingSystem, long tradeId, int version, String tokenStatus) throws IOException, NamingException, BadLocationException, SQLException {

        CreditLogRec clr = new CreditLogRec();
        clr.setTradeId(tradeId);
        clr.setProcessFlag("N");
        clr.setCmt("No Record Found");
        TradeRqmtRec trr = dao.getTradeConfirmId(tradingSystem,tradeId,version);
        if (trr.getTradeRqmtConfirmId() > 0) {
            String creditMarginToken = "";
            if ("Y".equalsIgnoreCase(tokenStatus)) {
                creditMarginToken = getCreditMarginToken(tradingSystem,tradeId,version);
            }
            Logger.getLogger(CreditMarginProcessor.class).info("Trade Id = " + tradeId + "; Token = " + creditMarginToken);
            ContractRequest cr = new ContractRequest();
            cr.setTradeRqmtConfirmId(trr.getTradeRqmtConfirmId());
            ContractResponse response = confirmation.getContractFromVault(cr,dbUserName);
            // update the vault only if the contract in the vault
            if ( response.getContract() != null  & !"".equalsIgnoreCase(response.getContract())) {
                String marginInsertedContract = rtfInserter.insertMarginToken(response.getContract(),creditMarginToken,marginToken,clr);
                storeContractInVault(trr,marginInsertedContract);
                if ("CRDT".equalsIgnoreCase(trr.getCurrentStatus())) {
                    dao.updateTradeRqmtStatus(trr);
                    sendEMail(trr);
                }
                clr.setProcessFlag("Y");
            }
            else {
                clr.setCmt("No Contract Found in the Vault");                
            }

        }
        else {
            Logger.getLogger(this.getClass()).info("No Rqmt confirm id found in db for the trade " +tradeId);
        }
        return clr;
    }

    private void sendEMail(TradeRqmtRec trr) throws SQLException {

        if ( _TRADER_NOTIFY_STATUS.equalsIgnoreCase(trr.getNextStatusCode())) {

            String fromName = "OpsManager";
            String fromAddress = "OpsManager@rbssempra.com";
            String emailAddress = dao.getTraderEmailAddr(trr);
            String subject = "Confirmation Pending: " + trr.getTradeId();
            String body = "There is a new confirmation waiting for your approval.\n" +
                           "Trade Id: " + trr.getTradeId() + "\n" +
                           "Counterparty: " + trr.getCptySn() + "\n" +
                          "Commodity: " + trr.getCdtyGroupCode() + "\n" +
                          "To approve the trade go to: " + traderWebUrl;

            if (!"SEMPRA.PROD".equalsIgnoreCase(this.database)){
                body = body + "\n Original email address = " + emailAddress;
                emailAddress = "pgangara@rbssempra.com";    
            }

            if (emailAddress != null) {
                NotifyUtil.sendMail(emailAddress,subject,body);
            }

        }
        

    }

    private void storeContractInVault(TradeRqmtRec trr, String marginInsertedContract) {

        ContractRequest cr = new ContractRequest();
        cr.setTradeSysCode(trr.getTradesystem());
        cr.setTemplateId(trr.getTemplateId());
        cr.setSempraCptySn(trr.getSeCptySn());
        cr.setCptySn(trr.getCptySn());
        cr.setDateSent(getDateSent());
        cr.setSignedFlag(trr.getSignedFlag());
        cr.setTradeId(trr.getTradeId());
        cr.setRqmtId(trr.getTradeRqmtId());
        cr.setTradeRqmtConfirmId(trr.getTradeRqmtConfirmId());
        cr.setTradeDate(trr.getTradeDate());
        cr.setCdtyCode(trr.getCdtyCode());
        cr.setCdtyGroupCode(trr.getCdtyGroupCode());
        cr.setSettlementType(trr.getSttlType());
        cr.setContract(marginInsertedContract);
        ContractResponse response =confirmation.storeContractInVault(cr,dbUserName);

        log.info("Store Contract response status = " + response.getResponseStatus());
        log.info("Store Contract response = " + response.getResponseText());
        

    }

    private String getDateSent() {

        SimpleDateFormat sdf = new SimpleDateFormat("MM/dd/yyyy");
        Date dt= new Date();
        return sdf.format(dt);
    }

    private String getCreditMarginToken(String tradingSystem, long tradeId, int version) throws IOException {

        String url =     creditTermUrl + "?key=" + tradingSystem + "|" + tradeId + "|" + version + "&output_type=RTF&font_size=8";
        log.info( "URL=" + url);
        return getURLData(url);
    }

    private String getStatusFromCredit(String tradingSystem, long tradeId, int version) throws IOException {

        String url = creditStatusCheckUrl + "?key=" + tradingSystem + "|" + tradeId + "|" + version ;
        return getURLData(url);
    }

    private String getURLData(String url) throws IOException {

        URL  httpUrl =  new URL(url);
        URLConnection uc = httpUrl.openConnection();
        BufferedReader br = new BufferedReader(new InputStreamReader(uc.getInputStream()));
        StringBuffer sb = new StringBuffer();
        String inputLine = null;
        while( (inputLine = br.readLine()) != null){
            sb.append(inputLine);
        }
        return sb.toString();
    }
    /*
    private Hashtable getEnv(){

        Hashtable hs = new Hashtable();
        hs.put(InitialContext.INITIAL_CONTEXT_FACTORY,"org.jnp.interfaces.NamingContextFactory");
        hs.put(InitialContext.PROVIDER_URL,"jnp://localhost:1099");
        hs.put(InitialContext.URL_PKG_PREFIXES,"org.jboss.naming:org.jnp.interfaces");
        return hs;
    }
     */
    public static void main(String arg[]) throws NamingException, SQLException, BadLocationException {

        Connection connection  =  DriverManager.getConnection("jdbc:oracle:thin:@yonoradb7:1521:PROD", "srajaman", "srajaman");
        TradeRqmtDAO dao =new TradeRqmtDAO(connection);
        CreditMarginProcessor cmp = new CreditMarginProcessor(dao,"http://st-xpgzhang1/creditlib/IsMarginExists.aspx","http://st-xpgzhang1/creditlib/creditterms.aspx","[MARGIN TOKEN]","","OpsManagerCnf/Confirmation/remote","jbossusr","SEMPRA.DEV");
        try {
            cmp.processCreditMarginNotification("JMS",533522 ,1,"");
        } catch (IOException e) {
            e.printStackTrace();
        }
        connection.close();
    }
}
