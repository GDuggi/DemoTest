package aff.confirm.webservices.tradegateway.common;

import aff.confirm.webservices.tradegateway.data.ContractData;
import aff.confirm.webservices.tradegateway.data.GatewayConfig;
import aff.confirm.webservices.tradegateway.data.TradeAlertData;
import aff.confirm.webservices.tradegateway.data.TradeData;

import java.io.IOException;
import java.util.StringTokenizer;

/**
 * Created with IntelliJ IDEA.
 * User: sraj
 * Date: 1/16/13
 * Time: 1:16 PM
 */
public interface TradeInfo {

    String getOpsTrackingTrade(String tradeSystemCode, String ticket) throws  Exception;
    String getContractData(String tradeSystemCode, String ticket) throws  Exception;
    String getTradeAlertMsg(String tradeSystemCode,String ticket) throws Exception;

    void setConfig(GatewayConfig config);



}
