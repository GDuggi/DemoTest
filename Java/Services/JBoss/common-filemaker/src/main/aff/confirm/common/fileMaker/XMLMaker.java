package aff.confirm.common.fileMaker;

import org.jboss.logging.Logger;
import org.jdom.Document;
import org.jdom.Element;
import org.jdom.output.XMLOutputter;
import org.xml.sax.SAXException;

import javax.xml.parsers.ParserConfigurationException;
import java.io.*;

public class XMLMaker
{
  Element rootElement = null;
  Document xmlDocument;

  public XMLMaker(String pRoot)
    throws IOException, SAXException, ParserConfigurationException
  {
    this.rootElement = new Element(pRoot);
    this.xmlDocument = new Document(this.rootElement);
  }

  public Element addElement(String pTagName, String pValue)
  {
    Element node = new Element(pTagName);
    node.addContent(pValue);
    if (this.rootElement != null)
      this.rootElement.addContent(node);
    else
      this.xmlDocument.setRootElement(node);
    return node;
  }

  public Element addElement(Element pParent, String pTagName, String pValue) {
    Element node = new Element(pTagName);
    node.addContent(pValue);
    pParent.addContent(node);
    return node;
  }

  public String getXML() throws IOException {
    XMLOutputter outputter = new XMLOutputter();
    StringWriter writer = new StringWriter();
    outputter.output(this.xmlDocument, writer);
    writer.close();
    return writer.toString();
  }

  public String getValue(String pName) throws Exception {
    Element child = this.rootElement.getChild(pName);
    if ((child == null) || (child.getText().length() == 0)) {
      throw new Exception("No XML Elements Found For " + pName);
    }

    return child.getText();
  }

  public static void main(String[] args) {
    try {
      XMLMaker xmlMaker = new XMLMaker("Sample");
      Element parent = xmlMaker.addElement("prices", "");
      Element price = xmlMaker.addElement(parent, "price", "");
      xmlMaker.addElement(price, "rate", "0.12356");
      xmlMaker.addElement(price, "uom", "MT");
      xmlMaker.addElement(price, "ccy", "USD");
      Logger.getLogger(XMLMaker.class).info(xmlMaker.getXML());
    } catch (IOException | SAXException | ParserConfigurationException e) {
        e.printStackTrace();
    }
  }

  public Element getRoot() {
    return this.rootElement;
  }

  public Element appendChild(Element pParent, Element pChild) {
    pChild.detach();
    Element newNode = pParent.addContent(pChild);
    return newNode;
  }

  public void getStreamSource(ByteArrayOutputStream pXMLOutput) {
    XMLOutputter xmlOutputter = new XMLOutputter();
    Writer writer = new OutputStreamWriter(pXMLOutput);
    try {
      xmlOutputter.output(this.xmlDocument, writer);
    } catch (IOException e) {
      Logger.getLogger(XMLMaker.class).error(e.getMessage());
    }
  }
}

