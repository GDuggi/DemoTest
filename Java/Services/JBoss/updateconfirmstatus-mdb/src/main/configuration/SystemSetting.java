package  configuration;

/**
 * Created by jvega on 7/29/2015.
 */
public interface SystemSetting {
    interface Key {}
    String get(String key);
    String get(String key, String defaultValue);
}

