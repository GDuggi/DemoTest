package aff.confirm.webservices.tradegateway;

import aff.confirm.webservices.tradegateway.common.TradeInfo;
import aff.confirm.webservices.tradegateway.data.ContractData;
import aff.confirm.webservices.tradegateway.data.TradeAlertData;
import aff.confirm.webservices.tradegateway.data.TradeData;
import aff.confirm.webservices.tradegateway.exception.GatewayException;

import javax.servlet.ServletContext;
import javax.ws.rs.*;
import javax.ws.rs.core.Context;
import javax.ws.rs.core.MediaType;
import java.io.IOException;


@Path("/tradefeed")
public class TradeFeed {

    @Context
    ServletContext context;


    @GET()
    @Path("/opstrade")
    @Produces({MediaType.TEXT_XML})
    public String  getOpsTrackingData(
            @QueryParam("tradesyscode") String tradeSysCode,
            @QueryParam("ticket") String ticket) throws GatewayException {

        // Israel 9/16/2013 - changing return from TradeAlertData to String
        //TradeData data = null;
        String alertDataXML = null;
        try {
            Controller controller = Controller.getController(getConfigFileName());
            TradeInfo tradeInfo = controller.createTradeInstance(tradeSysCode);
            // Israel 9/16/2013 - changing return from TradeAlertData to String
            //data = tradeInfo.getOpsTrackingTrade(tradeSysCode,ticket);
            alertDataXML = tradeInfo.getOpsTrackingTrade(tradeSysCode,ticket);
            //Not necessary since calling app sets these values based on the feed it is calling.
            //if (data != null) {
            //    data.setTrade_sys_code(tradeSysCode);
            //   data.setTicket(ticket);
            //}
        } catch (Exception e) {
            throw new GatewayException(e);
        }

        return alertDataXML;
    }

    @GET()
    @Path("/tradealert")
    @Produces({MediaType.TEXT_XML})
    public String getTradeAlertData(
            @QueryParam("tradesyscode") String tradeSysCode,
            @QueryParam("ticket") String ticket) throws GatewayException {

        //System.out.println("Trade Alert is called.....");
        // Israel 9/16/2013 - changing return from TradeAlertData to String
        //TradeAlertData alertData = null;
        String alertDataXML = null;

        Controller controller = null;
        try {
            controller = Controller.getController(getConfigFileName());
            TradeInfo tradeInfo = controller.createTradeInstance(tradeSysCode);
            alertDataXML = tradeInfo.getTradeAlertMsg(tradeSysCode,ticket);
            // Israel 9/16/2013 - changing return from TradeAlertData to String
            //if (alertData != null){
            //    alertData.setTrade_type_code(tradeSysCode);
            //}

        }  catch (Exception e) {
            throw new GatewayException(e);
        }
        return alertDataXML;
    }



    @GET()
    @Path("/contract")
    @Produces({MediaType.TEXT_XML})
    public String getContractData(
               @QueryParam("tradesyscode") String tradeSysCode,
               @QueryParam("ticket") String ticket) throws GatewayException {

        String data = null;

        try {
            Controller controller = Controller.getController(getConfigFileName());
            TradeInfo tradeInfo = controller.createTradeInstance(tradeSysCode);
            data = tradeInfo.getContractData(tradeSysCode,ticket);
            /*
            if (data != null) {
                data.setTrading_system_code(tradeSysCode);
            }
            */
        }  catch (Exception e) {
            throw new GatewayException(e);
        }
        return data;


    }

    private String getConfigFileName() {

        String fileName = context.getInitParameter("ConfigFile");
        //System.out.println("Config File = " + fileName);
        return fileName;
    }


}
