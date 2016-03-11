package aff.confirm.webservices.tradegateway;

import aff.confirm.webservices.tradegateway.common.TradeInfo;
import aff.confirm.webservices.tradegateway.data.GatewayConfig;
import aff.confirm.webservices.tradegateway.util.ConfigLoader;
import aff.confirm.webservices.tradegateway.util.ConfirmConfigReader;
import org.jboss.logging.Logger;

import java.io.IOException;
import java.util.HashMap;

/**
 * User: sraj
 * Date: 1/23/13
 * Time: 4:07 PM
 */
public class Controller {
    private static Logger log = Logger.getLogger( Controller.class );


    private HashMap<String,GatewayConfig> gatewayConfig;

    private static Controller controller;

    private Controller(String confirmConfigFile) throws IOException {

        loadConfiguration(confirmConfigFile);
    }

    private void loadConfiguration(String confirmConfigFile) throws IOException {

        ConfirmConfigReader reader = new ConfirmConfigReader(confirmConfigFile);
        String gatewayXML = reader.getValue("aff.confirm.trade.gateway");
        log.info(gatewayXML);
        gatewayConfig = ConfigLoader.getConfig(gatewayXML);

        log.info("Gateway Config= " + gatewayConfig);

    }

    public static  Controller getController(String confirmConfigFile) throws IOException {

        if ( controller == null) {
            controller = new Controller(confirmConfigFile);
        }
        return controller;
    }


    public TradeInfo createTradeInstance(String tradeSystemCode) throws Exception, IllegalAccessException, InstantiationException {

        TradeInfo tradeInfo = null;

        if ( tradeSystemCode == null  || "".equals(tradeSystemCode))
        {
            throw new Exception("Trading System code is missing");
        }

        GatewayConfig config = gatewayConfig.get(tradeSystemCode.toUpperCase());

        if ( config == null) {
            throw new Exception("Configuration is not available for trading system " + tradeSystemCode);
        }

        String className = config.getClassName();
        if (className == null ||  "".equalsIgnoreCase(className)) {
            throw new Exception("Implementation details is missing for the trading system " + tradeSystemCode);
        }

        tradeInfo = (TradeInfo) Class.forName(className).newInstance();
        tradeInfo.setConfig(config);

        return tradeInfo;

    }


}
