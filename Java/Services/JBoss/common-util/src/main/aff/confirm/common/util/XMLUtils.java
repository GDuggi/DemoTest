package aff.confirm.common.util;

import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;
import java.util.Set;

/**
 * User: ifrankel
 * Date: May 10, 2005
 * Time: 11:42:12 AM
 */
public class XMLUtils {
    static final public int TAG_OPEN_CLOSED = 0;
    static final public int TAG_OPEN = 1;
    static final public int TAG_CLOSED = 2;
    static final public String XML_HEADER = "<?xml version = \"1.0\" encoding=\"UTF-8\" ?>\n";
    static final public String EFET_XML_HEADER = "<?xml version = \"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?>\n";
    static final public String EFET_HEADER_ATTRIBS = "SchemaVersion=\"3\" SchemaRelease=\"1\"";
/*
    static final public String EFET_HEADER_ATTRIBS = "xsi:noNamespaceSchemaLocation=" +
            "\"http://www.efet.org/ecm/schemas/v3r2/EFET-CNF-V3R2.xsd\"\n" +
            "\txmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" SchemaVersion=" +
            "\"3\"\tSchemaRelease=\"2\"";
*/

//    static final public String EFET_HEADER_ATTRIBS_CNF = "xsi:noNamespaceSchemaLocation=" +
//            "\"http://www.efet.org/ecm/schemas/v3r2/EFET-CNF-V3R2.xsd\"\n" +
//            "\txmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" SchemaVersion=" +
//            "\"3\"\tSchemaRelease=\"2\"";
//
//    static final public String EFET_HEADER_ATTRIBS_CAN = "xsi:noNamespaceSchemaLocation=" +
//            "\"http://www.efet.org/ecm/schemas/v3r2/EFET-CAN-V3R2.xsd\"\n" +
//            "\txmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" SchemaVersion=" +
//            "\"3\"\tSchemaRelease=\"2\"";
//
//    static final public String EFET_HEADER_ATTRIBS_BFI = "xsi:noNamespaceSchemaLocation=" +
//            "\"http://www.efet.org/ecm/schemas/v3r2/EFET-CNF-V3R2.xsd\"\n" +
//            "\txmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" SchemaVersion=" +
//            "\"3\"\tSchemaRelease=\"2\"";

    /**
     * Each iteration is cumulative, otherwise all possible characters won't be checked.
     * @param pTest
     * @return
     */
    static public String getEntityRefs(String pTest){
        String testString = pTest;
        String resultString = "";
        resultString = testString.replaceAll("&","&amp;");
        testString = resultString;
        resultString = testString.replaceAll("<","&lt;");
        testString = resultString;
        resultString = testString.replaceAll(">","&gt;");
        testString = resultString;
        resultString = testString.replaceAll("'","&apos;");
        testString = resultString;
        resultString = testString.replaceAll("\"","&quot;");
        return resultString;
    }

    static public String buildTagItem(int pTabs, String pTagName, String pTagContent,
                                      int pTagStyle, String pAttributes){
        final String CRLF = "\n";
        String tagItem = "";
        String startTag = "";
        if (pAttributes.length() > 0)
            startTag = getTabString(pTabs) + "<" + pTagName + " " + pAttributes + ">";
        else
            startTag = getTabString(pTabs) + "<" + pTagName + ">";

        if (pTagStyle == TAG_OPEN)
            tagItem = startTag + CRLF;
        else if (pTagStyle == TAG_CLOSED)
            tagItem = getTabString(pTabs) + "</" + pTagName + ">" + CRLF;
        else
            tagItem = startTag + getEntityRefs(pTagContent) + "</" + pTagName + ">" + CRLF;

        return tagItem;
    }

    static public String buildTagItem(int pTabs, String pTagName, String pTagContent,
                                      int pTagStyle, HashMap pAttribMap){
        final String CRLF = "\n";
        String tagItem = "";
        String startTag = "";

        //Process the attribute list
        //An empty hash map is ok
        Set set = pAttribMap.entrySet();
        Iterator i = set.iterator();

        String attribs = "";
        while(i.hasNext()){
            Map.Entry me = (Map.Entry)i.next();
            attribs += " " + me.getKey() + "=\"" + me.getValue() + "\"";
        }
        
        startTag = getTabString(pTabs) + "<" + pTagName + " " + attribs + ">";

        if (pTagStyle == TAG_OPEN)
            tagItem = startTag + CRLF;
        else if (pTagStyle == TAG_CLOSED)
            tagItem = getTabString(pTabs) + "</" + pTagName + ">" + CRLF;
        else
            tagItem = startTag + getEntityRefs(pTagContent) + "</" + pTagName + ">" + CRLF;

        return tagItem;
    }

    static private String getTabString(int pTabs){
        String tabString = "";
        int i;
        for (i=0; i<pTabs; i++)
            tabString += "\t";

        return tabString;
    }

}
