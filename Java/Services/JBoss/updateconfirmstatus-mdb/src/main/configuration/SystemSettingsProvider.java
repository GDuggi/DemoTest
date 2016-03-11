package configuration;


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
    public String get(String key){
        return config.getProperty(key);
    }

    @Override
    public String get(String key, String defaultValue) {
        return config.getProperty(key, defaultValue);
    }

    public void listAllProperties(PrintStream printStream){
        config.list(printStream);
    }


    }

