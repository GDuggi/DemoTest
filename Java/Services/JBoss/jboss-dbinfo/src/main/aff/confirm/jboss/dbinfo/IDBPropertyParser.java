package aff.confirm.jboss.dbinfo;

import java.io.IOException;
import java.util.Map;


/**
 * A Database specific server name to ip address parser.
 */
public interface IDBPropertyParser {

    /**
     * Finds the new JDBC url given a default template.
     * @deprecated
     */
    public String getNewJDBCURL(String serverName, String oldURL);

    /**
     * Finds the JDBC url for the specified serverName.
     */
    public String getJDBCURL(String serverName);

    /**
     * Return the JDBC URL map.
     * The key is the database logical name,
     * its corresponding value is URL of JDBC driver.
     */
    public Map getJDBCURLMap();

    /**
     * Parse the specified Database property file name
     * such as TNSNAMES.ORA for ORACLE.
     */
    public void parse(String fileName) throws IOException;

}
