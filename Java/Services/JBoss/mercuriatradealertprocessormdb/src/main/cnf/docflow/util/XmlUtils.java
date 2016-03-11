package cnf.docflow.util;

import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.InputStream;

/**
 * Created by jvega on 7/22/2015.
 */
public class XmlUtils {

    public static String getMessageItem(Element pElement,String pTag){
        String result = "";
        NodeList nodeList = pElement.getElementsByTagName(pTag);
        if (nodeList.getLength() > 0) {
            result = nodeList.item(0).getTextContent();
        }
        return result;
    }

    public static Document loadXMLfromMessage(String xmlMsg) throws SAXException, IOException {
        return parseXMLMsg(new ByteArrayInputStream(xmlMsg.getBytes()));
    }

    public static Document parseXMLMsg(InputStream is) throws SAXException, IOException {
        DocumentBuilderFactory dbFactory = null;
        DocumentBuilder docBuilder = null;
        Document doc = null;
        try {
            dbFactory = DocumentBuilderFactory.newInstance();
            dbFactory.setNamespaceAware(true);
            docBuilder = dbFactory.newDocumentBuilder();
            doc = docBuilder.parse(is);
        } catch (ParserConfigurationException ex) {
        } finally {
            is.close();
        }
        return doc;
    }
}
