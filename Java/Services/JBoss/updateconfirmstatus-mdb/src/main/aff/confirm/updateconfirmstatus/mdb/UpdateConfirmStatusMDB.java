package aff.confirm.updateconfirmstatus.mdb;


/*
* @author rescaraman 
* @since 2015-10-21
* copyright Amphora Inc. 2015
*/


import aff.confirm.updateconfirmstatus.dao.TradeKey;
import aff.confirm.updateconfirmstatus.dao.TradeKeyDao;
import cnf.confirmationsmanager.TradeConfirmationStatusChangeRequest;
import cnf.integration.TradeConfirmationStatusChangeResponse;
import cnf.integration.WorkflowInd;

import javax.ejb.ActivationConfigProperty;
import javax.ejb.MessageDriven;
import java.util.logging.Level;
import java.util.logging.Logger;
//queue/confirmsMgr.tradeAppr.activityAlert

@MessageDriven(name = "UpdateConfirmStatusMDB", activationConfig = {
        @ActivationConfigProperty(propertyName = "destinationType", propertyValue = "javax.jms.Queue"),
        @ActivationConfigProperty(propertyName = "destination", propertyValue = "queue/confirmsMgr.tradeAppr.activityAlert"),
        @ActivationConfigProperty(propertyName = "useDLQ", propertyValue = "false"),
        @ActivationConfigProperty(propertyName = "maxSession", propertyValue = "1"),
        @ActivationConfigProperty(propertyName = "acknowledgeMode", propertyValue = "Auto-acknowledge")})
public class UpdateConfirmStatusMDB extends Abstract0UpdateConfirmStatusMDB
{

    public final static Logger log = Logger.getLogger(UpdateConfirmStatusMDB.class.toString());

    @Override
    public void processMessge(String message)
    {
        if (message != null && message != "")
        {
            log.info("UpdateConfirmStatusMDB.processMessge invoked");
            log.info("Message from  queue: " + message);
            String[] strArr = message.split("\\|");
            int tradeId = Integer.parseInt(strArr[1]);
            String ConfirmationStatusCode = strArr[3] != null ? strArr[3].toString() : null;
            try
            {

                TradeKeyDao dao = new TradeKeyDao(dataSource);
                TradeKey tradeKey = dao.getById(tradeId);

                TradeConfirmationStatusChangeRequest statusChangeRqst = new TradeConfirmationStatusChangeRequest();
                statusChangeRqst.setConfirmationStatusCode(ConfirmationStatusCode);
                statusChangeRqst.setWorkflowInd(WorkflowInd.FINALAPPROVAL);
                statusChangeRqst.setTradingSystemCode(tradeKey.getTradeSysCode());
                statusChangeRqst.setTradingSystemKey(tradeKey.getTradeSysTicket());

                log.info("Sending Trade Confirmation Status Change Request: TradingSystemKey = " + tradeKey.getTradeSysTicket()+
                                 ", TradingSqystemCode = " + tradeKey.getTradeSysCode()+ ", ConfirmationStatusCode = " +
                                 ConfirmationStatusCode + ", WorkflowInd = " + WorkflowInd.FINALAPPROVAL);

                TradeConfirmationStatusChangeResponse response =
                        getConfirmationsManager().tradeConfirmationStatusChange(statusChangeRqst);

                if (response.isSuccess())
                {
                    log.info("Trade Confirmation Status Change Response: SUCCESS ");
                } else
                {
                    log.info("Trade Confirmation Status Change Response: FAILURE ");
                }
            }
            catch (Exception ex)
            {
                log.log(Level.SEVERE, "Error: " + ex.getMessage(), ex );
            }
        }
    }

}
