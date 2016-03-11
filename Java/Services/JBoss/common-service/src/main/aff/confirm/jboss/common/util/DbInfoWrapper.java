package aff.confirm.jboss.common.util;

import org.jboss.logging.Logger;


public class DbInfoWrapper {
    private static Logger log = Logger.getLogger( DbInfoWrapper.class );
    private String serviceName;

    public  DbInfoWrapper(String serviceName){
        this.serviceName = serviceName;
    }

    public String getDBUrl(){
        String val = System.getProperty(serviceName + "." + "DBUrl");
        log.info("DbInfoWrapper: DBUrl=" + val);
        return val;
    }

    public  String getDatabaseName() {
        String val = System.getProperty(serviceName + "." + "DatabaseName");
        log.info("DbInfoWrapper: DatabaseName=" + val);
        return val;
    }

    public String getDBUserName() {
        String val = System.getProperty(serviceName + "." + "DBUserName");
        log.info("DbInfoWrapper: DBUserName=" + val);
        return val;
    }

    public String getDBPassword() {
        String val = System.getProperty(serviceName + "." + "DBPassword");
        log.info("DbInfoWrapper: DBPassword=" + val);
        return val;
    }
}
