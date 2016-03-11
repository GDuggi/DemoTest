package cnf.docflow.configuration;

import java.io.PrintStream;
import java.util.Properties;

/**
 * Created by jvega on 7/29/2015.
 */
public enum SystemSettingsProvider implements SystemSetting {
    INSTANCE;

    private final Properties config;

    SystemSettingsProvider() {
         config = System.getProperties();
    }

    @Override
    public String get(SystemSetting.Key key){
        return config.getProperty(key.toString());
    }

    @Override
    public String get(SystemSetting.Key key, String defaultValue) {
        return config.getProperty(key.toString(), defaultValue);
    }

    public void listAllProperties(PrintStream printStream){
        config.list(printStream);
    }

    public enum Key implements SystemSetting.Key {
        DEBUG_ENABLED("cnf.docflow.mdb.MercuriaTradeAlertProcessorMDB.DebugLogging"),
        MAIL_TO("cnf.docflow.mdb.MercuriaTradeAlertProcessorMDB.EmailNotify"),
        MAIL_DOMAIN("cnf.docflow.mdb.MercuriaTradeAlertProcessorMDB.EmailDomain"),
        MAIL_MAXDELIVERYATTEMPTS("cnf.docflow.mdb.MercuriaTradeAlertProcessorMDB.MaxDeliveryAttemptsforEmail"),
        JBOSS_HOST_NAME("jboss.host.name"),
        USER_NAME("user.name");

        private final String Code;

        Key(String code) { this.Code = code; }
        @Override
        public String toString() { return Code; }

    }


}
