package aff.confirm.updateconfirmstatus.mdb;


/*
* @author rescaraman 
* @since 2015-10-21
* copyright Amphora Inc. 2015
*/


import aff.confirm.updateconfirmstatus.dao.ExternalWorkflowNameDao;
import aff.confirm.updateconfirmstatus.dao.TradeKey;
import aff.confirm.updateconfirmstatus.dao.TradeKeyDao;
import cnf.confirmationsmanager.TradeConfirmationStatusChangeRequest;
import cnf.integration.TradeConfirmationStatusChangeResponse;
import cnf.integration.WorkflowInd;

import javax.ejb.ActivationConfigProperty;
import javax.ejb.MessageDriven;
import java.sql.SQLException;
import java.util.logging.Level;
import java.util.logging.Logger;
//queue/confirmsMgr.tradeAppr.activityAlert

@MessageDriven(name = "UpdateTradeRqmtStatusMDB", activationConfig = {
        @ActivationConfigProperty(propertyName = "destinationType", propertyValue = "javax.jms.Queue"),
        @ActivationConfigProperty(propertyName = "destination", propertyValue = "queue/confirmsMgr.tradeRqmt.statusChange"),
        @ActivationConfigProperty(propertyName = "useDLQ", propertyValue = "false"),
        @ActivationConfigProperty(propertyName = "maxSession", propertyValue = "1"),
        @ActivationConfigProperty(propertyName = "acknowledgeMode", propertyValue = "Auto-acknowledge")})
public class UpdateTradeRqmtStatusMDB extends Abstract0UpdateConfirmStatusMDB
{

    public final static Logger log = Logger.getLogger(UpdateTradeRqmtStatusMDB.class.toString());

    @Override
    public void processMessge(String message)
    {
        if (message != null && message != "")
        {
            log.info("UpdateConfirmStatusMDB.processMessge invoked");
            log.info("Message from  queue: " + message);
            String[] strArr = message.split("\\|");
            if( strArr.length < 4)
            {
                log.severe("Error - message not in expected format of 1|TRADE_RQMNT_ID|RQMT_CODE|STATUS");
                return;
            }
            int tradeRqmtId = Integer.parseInt(strArr[1]);
            String rqmtCode = strArr[2];
            String status = strArr[3];
            try
            {

                TradeKeyDao dao = new TradeKeyDao(dataSource);
                TradeKey tradeKey = dao.getByTradeRqmtId(tradeRqmtId);
                WorkflowInd workflowInd = getWorkflowInd(rqmtCode);
                if (workflowInd == null)
                {
                    log.info("No EXT_WORKFLOW_NAME defined for rqmtCode = " + rqmtCode + ", nothing to do." );
                    return;
                }

                TradeConfirmationStatusChangeRequest statusChangeRqst = new TradeConfirmationStatusChangeRequest();
                statusChangeRqst.setConfirmationStatusCode(status);
                statusChangeRqst.setWorkflowInd(workflowInd);
                statusChangeRqst.setTradingSystemCode(tradeKey.getTradeSysCode());
                statusChangeRqst.setTradingSystemKey(tradeKey.getTradeSysTicket());

                log.info("Sending Trade Confirmation Status Change Request: TradingSystemKey = " + tradeKey.getTradeSysTicket()+
                                 ", TradingSystemCode = " + tradeKey.getTradeSysCode()+ ", ConfirmationStatusCode = " +
                                 status + ", WorkflowInd = " + workflowInd);

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

    private WorkflowInd getWorkflowInd(String rqmtCode) throws SQLException
    {
        ExternalWorkflowNameDao dao = new ExternalWorkflowNameDao(dataSource);
        String workflowCode = dao.getFromRqmtCode(rqmtCode);
        if (workflowCode == null)
        {
            return null;
        }

        return WorkflowInd.fromValue(workflowCode);
    }

}
