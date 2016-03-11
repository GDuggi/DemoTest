package aff.confirm.webservices.tradegateway.util;

import javax.xml.bind.annotation.adapters.XmlAdapter;

/**
 * Created with IntelliJ IDEA.
 * User: sraj
 * Date: 2/1/13
 * Time: 11:22 AM
 */
public class StringAdapter extends XmlAdapter<String,String> {

    @Override
    public String unmarshal(String v) throws Exception {
        if ( v == null  ) {
            return "";
        }
        return v;
    }

    @Override
    public String marshal(String v) throws Exception {
        if (v == null) {
            return "";
        }
        return v;
    }

}
