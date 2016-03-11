package cnf.docflow.configuration;

/**
 * Created by jvega on 7/29/2015.
 */
public interface SystemSetting {
    interface Key {}
    String get(Key key);
    String get(Key key, String defaultValue);
}

