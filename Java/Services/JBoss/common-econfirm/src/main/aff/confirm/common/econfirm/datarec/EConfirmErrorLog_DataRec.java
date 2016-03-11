package aff.confirm.common.econfirm.datarec;

import org.jdom.Document;
import org.jdom.Element;
import org.jdom.JDOMException;
import org.jdom.input.SAXBuilder;

import java.io.IOException;
import java.io.StringReader;

/**
 * User: ifrankel
 * Date: Jul 25, 2003
 * Time: 8:27:57 AM
 * This is the Java version of a Delphi TRecord.
 */
public class EConfirmErrorLog_DataRec {
    public double tradeID;
    public String ecStatus;
    public String ecCode;
    public String ecType;
    public String ecStatusDt;
    public String ecDesc;

    public EConfirmErrorLog_DataRec() {
        this.init();
    }

    public EConfirmErrorLog_DataRec(double pTradeID, String pECStatus,
                                    String pECCode, String pECType, String pECStatusDt,
                                    String pECDesc){
        this.init();
        this.tradeID = pTradeID;
        this.ecStatus = pECStatus;
        this.ecCode = pECCode;
        this.ecType = pECType;
        this.ecStatusDt = pECStatusDt;
        this.ecDesc = pECDesc;
    }

    public EConfirmErrorLog_DataRec(String pSubmitResponseXML) throws JDOMException, IOException {
        this.init();
        SAXBuilder saxBuilder = new SAXBuilder();
        Document doc = null;
        //String resultXML = "<?xml version = \"1.0\"?><Response><Status>FAILURE</Status><Message><Code>3007</Code><Type>Error</Type><Description>The request value is not valid.  Either the XML document is not well-formed or the XML document does not conform to the schema</Description><StatusDate>2003-10-13 10:34:25</StatusDate></Message></Response>";
        doc = saxBuilder.build(new StringReader(pSubmitResponseXML));
        Element rootElem = doc.getRootElement();
        Element statusElem = rootElem.getChild("Status");
        this.ecStatus = statusElem.getText();
        Element messageElem = rootElem.getChild("Message");
        this.ecCode = messageElem.getChildText("Code");
        this.ecType = messageElem.getChildText("Type");
        this.ecDesc = messageElem.getChildText("Description");
        this.ecStatusDt = messageElem.getChildText("StatusDate");
    }

    public void init(){
        this.tradeID = 0;
        this.ecStatus = "";
        this.ecCode = "";
        this.ecType = "";
        this.ecStatusDt = "";
        this.ecDesc = "";
    }


}
