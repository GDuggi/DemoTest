package aff.confirm.webservices.tradegateway.util;

import javax.xml.bind.annotation.adapters.XmlAdapter;
import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * Created with IntelliJ IDEA.
 * User: sraj
 * Date: 2/1/13
 * Time: 9:13 AM
 */
public class ContractDateAdapter extends XmlAdapter<String,Date> {

    SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd");
    SimpleDateFormat sdf1 = new SimpleDateFormat("yyyyMMdd");

    @Override
    public Date unmarshal(String v) throws Exception {
        return sdf1.parse(v);
    }

    @Override
    public String marshal(Date v) throws Exception {
        return  sdf.format(v);
    }
}
