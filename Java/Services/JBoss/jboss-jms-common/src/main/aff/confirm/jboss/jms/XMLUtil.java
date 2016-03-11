package aff.confirm.jboss.jms;


import org.dom4j.Document;
import org.dom4j.DocumentException;
import org.dom4j.Element;
import org.dom4j.io.SAXReader;

import java.io.File;
import java.io.StringReader;
import java.util.Enumeration;
import java.util.Hashtable;
import java.util.Iterator;
import org.jboss.logging.Logger;


/**
 * User: srajaman
 * Date: Jul 3, 2008
 * Time: 4:16:47 PM
 */
public class XMLUtil {
    private static Logger log = Logger.getLogger(XMLUtil.class);


    public static Hashtable loadEMSElementMapping(String xmlFileName) throws DocumentException {

        Hashtable hs = new Hashtable();
        
        SAXReader parser = new SAXReader();
        
       // try {
            Document document = parser.read( new File(xmlFileName));
            Element root = document.getRootElement();
            Iterator elementIterator = root.elementIterator("xml-element-mapping");

            while (elementIterator.hasNext()) {

                Element mappingNode = (Element) elementIterator.next();
                
                ElementMappingInfo emi = new ElementMappingInfo();
                Element xmlTagName = mappingNode.element(ElementMappingInfo._XML_TAG_NAME);
                emi.setXmlElementName(xmlTagName.getTextTrim());
                hs.put(xmlTagName.getTextTrim(),emi);

                Element propTagName = mappingNode.element(ElementMappingInfo._PROPERTY_TAG_NAME);
                emi.setJmsPropertyName(propTagName.getTextTrim());

                Element typeTagName = mappingNode.element(ElementMappingInfo._TYPE_TAG_NAME);
                emi.setJmsPropertyType(typeTagName.getTextTrim());

                Element formatTagName = mappingNode.element(ElementMappingInfo._FORMAT_TAG_NAME);
                emi.setDataFormat(formatTagName.getTextTrim());
            }
            
       // }

        return hs;
    }

    public static Hashtable parseMessage(String xmlData,Hashtable mappingObj ){
       Hashtable returnData = (Hashtable) mappingObj.clone();
        // clears the previous value
        Enumeration keyEnum = returnData.keys();
        while (keyEnum.hasMoreElements()){
            String key = (String) keyEnum.nextElement();
            ElementMappingInfo emi = (ElementMappingInfo) returnData.get(key);
            emi.setText("");
        }

        SAXReader parser = new SAXReader();
        StringReader reader = null;
        try {
             reader  = new StringReader(xmlData);
            Document document = parser.read(reader);
            Element root = document.getRootElement();
            for ( Iterator i = root.elementIterator(); i.hasNext();){
                Element element = (Element) i.next();
                fillXMLValues(element,returnData);
            }

        } catch (DocumentException e) {
            log.error( "LEVEL", e );
        }
        finally {
            if (reader != null) {
                reader.close();
            }
        }

        return returnData;
    }

    private static void fillXMLValues(Element element,Hashtable mappingObj){
        Iterator iterator = element.elementIterator();
        if ( iterator.hasNext() ) {
                while (iterator.hasNext()){
                    Element childElement = (Element) iterator.next();
                    fillXMLValues(childElement,mappingObj);
                }
        }
        else {
            String nodeName = element.getName();
            String nodeValue = element.getTextTrim();
            ElementMappingInfo emi = (ElementMappingInfo) mappingObj.get(nodeName);
            if (emi != null){
                emi.setText(nodeValue);
            }
            else {
                log.info(nodeName + " is not available in the config");
            }

        }

    }
}
