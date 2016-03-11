package aff.confirm.jboss.jms;

import java.io.Serializable;

/**
 * User: srajaman
 * Date: Jul 3, 2008
 * Time: 3:16:11 PM
 */
public class ElementMappingInfo  implements Serializable {

    public static final String _STRING_TYPE = "S";
    public static final String _INTEGER_TYPE = "I";
    public static final String _DATE_TYPE = "D";
    public static final String _DOUBLE_TYPE = "F";
    public static final String _BOOL_TYPE = "B";

    public static final String _XML_TAG_NAME = "xml-element-name";
    public static final String _PROPERTY_TAG_NAME = "jms-property-name";
    public static final String _TYPE_TAG_NAME = "jms-property-data-type";
    public static final String _FORMAT_TAG_NAME = "data-format";


    
    
    private String xmlElementName;
    private String jmsPropertyName ;
    private String jmsPropertyType = _STRING_TYPE;
    private String dataFormat;


    private String text = "";

    public String getText() {
        return text;
    }

    public void setText(String text) {
        this.text = text;
    }

    public String getXmlElementName() {
        return xmlElementName;
    }

    public void setXmlElementName(String xmlElementName) {
        this.xmlElementName = xmlElementName;
    }

    public String getJmsPropertyName() {
        return jmsPropertyName;
    }

    public void setJmsPropertyName(String jmsPropertyName) {
        this.jmsPropertyName = jmsPropertyName;
    }

    public String getJmsPropertyType() {
        return jmsPropertyType;
    }

    public void setJmsPropertyType(String jmsPropertyType) {
        this.jmsPropertyType = jmsPropertyType;
    }

    public String getDataFormat() {
        return dataFormat;
    }

    public void setDataFormat(String dataFormat) {
        this.dataFormat = dataFormat;
    }
    
}
