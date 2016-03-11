package aff.confirm.webservices.tradegateway.util;

import javax.xml.bind.annotation.adapters.XmlAdapter;
import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * Created with IntelliJ IDEA.
 * User: sraj
 * Date: 1/31/13
 * Time: 9:00 AM
 */
public class DateAdapter extends XmlAdapter<String,Date> {

    SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd");
    @Override
    public Date unmarshal(String v) throws Exception {
        return sdf.parse(v);
    }

    @Override
    public String marshal(Date v) throws Exception {
        return  sdf.format(v);
    }

}
