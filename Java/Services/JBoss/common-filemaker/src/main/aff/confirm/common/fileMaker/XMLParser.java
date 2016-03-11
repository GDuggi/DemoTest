package aff.confirm.common.fileMaker;

import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.InputStream;

 public class XMLParser
 {
   Element rootElement = null;
   Document xmlDocument;

   public XMLParser(InputStream pInputStream)
     throws IOException, SAXException, ParserConfigurationException
   {
     String xml = null;
     ByteArrayInputStream xmlStream = null;
     try {
       xml = readXML(pInputStream);
       DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
       DocumentBuilder builder = factory.newDocumentBuilder();
       xmlStream = new ByteArrayInputStream(xml.getBytes());
       this.xmlDocument = builder.parse(xmlStream);
     } finally {
       xml = null;
       if (xmlStream != null) try {
           xmlStream.close(); } catch (Exception ex) {
         } xmlStream = null;
     }
   }

   public String readXML(InputStream pInputStream) throws IOException
   {
     int numBytes = 0;
     StringBuffer xmlBuffer = new StringBuffer();
     byte[] byteBuffer = new byte[1];
     do {
       numBytes = pInputStream.read(byteBuffer);
       if (byteBuffer[0] != 4)
         xmlBuffer.append(new String(byteBuffer, 0, numBytes));
     }
     while (byteBuffer[0] != 4);

     return xmlBuffer.toString();
   }

   public XMLParser(String pXml) throws IOException, SAXException, ParserConfigurationException {
     DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
     DocumentBuilder builder = factory.newDocumentBuilder();
     ByteArrayInputStream xmlStream = new ByteArrayInputStream(pXml.getBytes());
     this.xmlDocument = builder.parse(xmlStream);
   }

   public String getValue(String pName) throws Exception
   {
     NodeList nodeList = this.xmlDocument.getElementsByTagName(pName);
     if ((nodeList == null) || (nodeList.getLength() == 0))
       throw new Exception("No XML Elements Found For " + pName);
     Element nodeListElement = (Element)nodeList.item(0);
     if (nodeListElement == null)
       throw new Exception("No XML Elements Found For " + pName);
     NodeList childElements = nodeListElement.getChildNodes();
     if ((childElements == null) || (childElements.getLength() == 0))
       throw new Exception("No XML Value Found For " + pName);
     Node valueNode = childElements.item(0);
     if (valueNode == null)
       throw new Exception("No XML Value Found For " + pName);
     return valueNode.getNodeValue();
   }
 }

