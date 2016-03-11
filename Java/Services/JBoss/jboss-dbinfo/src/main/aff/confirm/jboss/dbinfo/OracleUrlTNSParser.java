package aff.confirm.jboss.dbinfo;

import java.io.IOException;
import java.io.InputStream;
import java.net.URL;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.Statement;
import java.util.Iterator;
import java.util.Map;

public class OracleUrlTNSParser extends AbstractOracleTNSParser {

    public OracleUrlTNSParser() {}

    /**
     * Parse ORACLE TNSNAMES.ORA and extract the mapping
     * between db server name and host(either name or ip address).
     */
    public void parse(String fileURL) throws IOException {
        InputStream input = null;
        try {
            URL url = new URL(fileURL);
            input = url.openStream();
            parseInternal(input);

        } finally {
            if (input != null)
                input.close();
        }

    }

    public static void main(String[] args) throws Exception {
        Class.forName("oracle.jdbc.driver.OracleDriver");
      //  String fileName = "file:///G:/java/oracle\\TNSNAMES.ORA";
        String fileName = "file:\\\\fs-01\\Dev\\Database\\tnsnames\\tnsnames.ora";
        //String fileName = "file:///C:/oracle/network/ADMIN\\TNSNAMES.ORA";
//        String fileName = "file:///\\\\ct-prod2/apps/java/oracle/TNSNAMES.ORA";
        try {
            IDBPropertyParser parser = new OracleUrlTNSParser();
            parser.parse(fileName);
            String url = "jdbc:oracle:thin:@10.42.12.71:1522:prod";
            System.out.println(url);
            String devUrl = parser.getNewJDBCURL("SEMPRA.DEV", url);
            System.out.println(devUrl);

            Map dbMap = parser.getJDBCURLMap();
            Iterator it = dbMap.keySet().iterator();
            while (it.hasNext()) {
                Object key = it.next();
                System.out.println(key + " = " + dbMap.get(key));
            }

            Connection conn = DriverManager.getConnection(devUrl, "rye", "rye");
            Statement st = conn.createStatement();
            ResultSet rs = st.executeQuery("select * from rye");
            while (rs.next()) {
                System.out.println(rs.getString(1));
            }
            rs.close();
            st.close();
            conn.close();

        } catch (IOException e) {
            e.printStackTrace();
        }
    }

}
