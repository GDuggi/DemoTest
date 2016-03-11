package aff.confirm.webservices.tradegateway.ws;

import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerConfigurationException;
import javax.xml.transform.TransformerException;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.dom.DOMSource;
import javax.xml.transform.stream.StreamResult;
import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.StringWriter;

/**
 * Created with IntelliJ IDEA.
 * User: sraj
 * Date: 1/28/13
 * Time: 9:20 AM
 */
public class SoapUtil {

    public  static  String makeSoapRequest(String xmlRequest) {

        StringBuilder sb  = new StringBuilder();
        sb.append("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body>");
        sb.append(xmlRequest);
        sb.append("</s:Body></s:Envelope>");
        return sb.toString();

    }

    public  static String extractSoapResponse(String soapResponse,String tagName) throws ParserConfigurationException, IOException, SAXException, TransformerException {

        DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
        DocumentBuilder builder = factory.newDocumentBuilder();
        Document doc = builder.parse(new ByteArrayInputStream(soapResponse.getBytes()));
        NodeList element = doc.getElementsByTagName(tagName);
        if ( element.getLength() > 0) {

            Node node  = element.item(0);

            return node.getTextContent();

            /*
            TransformerFactory  transformerFactory = TransformerFactory.newInstance();
            Transformer  transformer = transformerFactory.newTransformer();
            StringWriter sw = new StringWriter();
            StreamResult result = new StreamResult(sw);
            DOMSource source = new DOMSource(node);
            transformer.transform(source,result);
            return sw.toString();
            */


        }
        return  null;





    }

}
