package aff.confirm.webservices.tradegateway.util;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FilterInputStream;
import java.io.IOException;
import java.util.Properties;

/**
 * Created with IntelliJ IDEA.
 * User: sraj
 * Date: 1/23/13
 * Time: 3:01 PM
 */
public class ConfirmConfigReader {


    Properties  properties ;

    public  ConfirmConfigReader(String configFileName) throws IOException {

            properties = new Properties();
            properties.load(new FileInputStream(configFileName));
    }

    public String getValue(String propertyName) {

        return properties.getProperty(propertyName);
    }



}
