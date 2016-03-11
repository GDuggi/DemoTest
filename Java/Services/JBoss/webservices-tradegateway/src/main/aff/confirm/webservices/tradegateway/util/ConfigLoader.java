package aff.confirm.webservices.tradegateway.util;

import aff.confirm.webservices.tradegateway.data.GatewayConfig;
import org.jboss.logging.Logger;

import javax.xml.stream.XMLEventReader;
import javax.xml.stream.XMLInputFactory;
import javax.xml.stream.XMLStreamConstants;
import javax.xml.stream.XMLStreamException;
import javax.xml.stream.events.Characters;
import javax.xml.stream.events.EndElement;
import javax.xml.stream.events.StartElement;
import javax.xml.stream.events.XMLEvent;
import java.io.StringReader;
import java.util.HashMap;


/**
 * User: sraj
 * Date: 1/23/13
 * Time: 4:25 PM
 */
public  class ConfigLoader {

    public static HashMap<String,GatewayConfig> getConfig(String xmlConfig) {

        HashMap<String,GatewayConfig> hashMap = new HashMap<>();

        hashMap= parseData(xmlConfig);
        return hashMap;
   }

    private static HashMap<String,GatewayConfig>  parseData(String xml) {

        HashMap<String,GatewayConfig> hashMap = new HashMap<>();
        XMLInputFactory factory = XMLInputFactory.newFactory();
        GatewayConfig gatewayConfig = null;
        String tagName = null;
        StartElement startElement = null;

        try {
            XMLEventReader eventReader = factory.createXMLEventReader(new StringReader(xml));
            while (eventReader.hasNext()) {
                XMLEvent event = eventReader.nextEvent();
                if ( event.getEventType() == XMLStreamConstants.START_ELEMENT) {


                    startElement = event.asStartElement();
                    if ("trading-system".equalsIgnoreCase(startElement.getName().getLocalPart())) {
                        gatewayConfig = new GatewayConfig();

                    }


                }
                else if (event.getEventType() == XMLStreamConstants.CHARACTERS) {
                    Characters chars =  event.asCharacters();
                    String data = chars.getData().trim();
                    if (!"".equalsIgnoreCase(data)) {

                        tagName = startElement.getName().getLocalPart();

                        switch (tagName) {
                            case  "code" :
                                gatewayConfig.setTradingSystemCode(data);
                                break;
                            case "description":
                                gatewayConfig.setDescription(data);
                                break;
                            case "method":
                                gatewayConfig.setMethod(data);
                                break;
                            case "sub-method":
                                gatewayConfig.setSubMethod(data);
                                break;
                            case "user-id":
                                gatewayConfig.setUserId(data);
                                break;
                            case "web-header-element":
                                gatewayConfig.setWebHeaderElement(data);
                                break;
                            case "class-name":
                                gatewayConfig.setClassName(data);
                                break;
                            case "web-service-url":
                               gatewayConfig.setWebServiceURL(data);
                        }

                    }
                }
                else if (event.getEventType() == XMLStreamConstants.END_ELEMENT) {
                    EndElement endElement = event.asEndElement();

                    if ("trading-system".equalsIgnoreCase(endElement.getName().getLocalPart())) {
                        hashMap.put(gatewayConfig.getTradingSystemCode(),gatewayConfig);
                    }

                }

            }
        } catch (XMLStreamException e) {
            Logger.getLogger(ConfigLoader.class).error( "ERROR", e);
        }
        return hashMap;

    }

}
