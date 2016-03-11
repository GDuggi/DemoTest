package aff.confirm.opsmanager.confirm.common;

import aff.confirm.opsmanager.common.util.NotifyUtil;
import aff.confirm.opsmanager.confirm.data.TradeRqmtRec;
import org.jboss.logging.Logger;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.URL;
import java.net.URLConnection;
import java.sql.SQLException;

public class CreditMarginProcessor {
	private static Logger log = Logger.getLogger( CreditMarginProcessor.class );

	private final static String _NOT_APPROVED_STATUS = "N";
	private final static String _NOT_FOUND = "TICKET NOT FOUND";
    private static String _TRADER_NOTIFY_STATUS = "TRADER";

	
	public String checkAndReplaceMarginToken(String contractText, 
											 String marginToken,
											 String tradingSystem,
											 long tradeId ,
											 String statusCheckUrl,
											 String termUrl,
											 TradeRqmtRec trr,
											 boolean creditMsgFound) {
		
		String returnStr = null;
		tradingSystem = "J".equalsIgnoreCase(tradingSystem)?"JMS":"AFF";
		if ( trr.getTradeRqmtConfirmId() > 0){ // The rqmt staus is PREP or CRDT for this trade, so process the contract data 
			
			try {
				//check the margin token found, then make call to 
				// 	Gang web service to find out whether trade is approved or not.
				int pos =contractText.indexOf(marginToken);
				if (pos > 0) { // margin token found
					String creditApproved = getStatusFromCredit(statusCheckUrl, tradingSystem, tradeId, 0) ;
					log.info("Credit Status Flag value = " + creditApproved);
				
					if ( ( creditMsgFound ) || 
							(!_NOT_APPROVED_STATUS.equalsIgnoreCase(creditApproved) && !_NOT_FOUND.equalsIgnoreCase(creditApproved)) ){ 
						// the trade is approved by credit, so get credit terms
						String creditTerms = "";
						if ("Y".equalsIgnoreCase(creditApproved)) { // if there is a credit term
							creditTerms = getCreditMarginToken(termUrl,tradingSystem,tradeId,0);
						}
						RTFInserter rtfInserter = new RTFInserter();
						returnStr =  rtfInserter.getIfMarginTokenReplaced(contractText, creditTerms, marginToken);
						Logger.getLogger("The margin token has been replaced. The credit term presents flag is " + creditApproved);
						rtfInserter = null;
					}
				}
			}
			catch (IOException e){
				log.error("Credit Web Service Call Error : " , e );
			}
			
		}
		
		return returnStr;
		
	}
	 private String getStatusFromCredit(String creditStatusCheckUrl, String tradingSystem, long tradeId, int version) throws IOException {

	        String url = creditStatusCheckUrl + "?key=" + tradingSystem + "|" + tradeId + "|" + version ;
	        return getURLData(url);
   }
	 
	private String getCreditMarginToken(String creditTermUrl,String tradingSystem, long tradeId, int version) throws IOException {

        String url =     creditTermUrl + "?key=" + tradingSystem + "|" + tradeId + "|" + version + "&output_type=RTF&font_size=8";
        log.info( "URL=" + url);
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
	        uc  = null;
	        httpUrl = null;
	        return sb.toString().trim();
	    }
	
	 public void updateRqtmStatus(TradeRqmtDAO trrDao , TradeRqmtRec trr, String traderUrl,String database) throws SQLException{
		 
		 if ("CRDT".equalsIgnoreCase(trr.getCurrentStatus())) {
			 trrDao.updateTradeRqmtStatus(trr);
             sendEMail(trrDao,trr,traderUrl,database);
         }
	 }
	 
	 private void sendEMail(TradeRqmtDAO trrDao, TradeRqmtRec trr,String traderWebUrl,String database) throws SQLException {

	        if ( _TRADER_NOTIFY_STATUS.equalsIgnoreCase(trr.getNextStatusCode())) {

	            String fromName = "OpsManager";
	            String fromAddress = "OpsManager@rbssempra.com";
	            String emailAddress = trrDao.getTraderEmailAddr(trr);
	            String subject = "Confirmation Pending: " + trr.getTradeId();
	            String body = "There is a new confirmation waiting for your approval.\n" +
	                           "Trade Id: " + trr.getTradeId() + "\n" +
	                           "Counterparty: " + trr.getCptySn() + "\n" +
	                          "Commodity: " + trr.getCdtyGroupCode() + "\n" +
	                          "To approve the trade go to: " + traderWebUrl;

	            log.info("Trade Email Address Database = " + database);
	            if (!"SEMPRA.PROD".equalsIgnoreCase(database)){
	                body = body + "\n Original email address = " + emailAddress;
	                emailAddress = "srajaman@rbssempra.com,pgangara@sempratrading.com";    
	            }

	            if (emailAddress != null) {
	                NotifyUtil.sendMail(emailAddress,subject,body);
	            }

	        }
	 }
}
