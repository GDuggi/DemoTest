package aff.confirm.jboss.dbinfo;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.Statement;
import java.util.Iterator;
import java.util.Map;

public class OracleTNSParser extends AbstractOracleTNSParser {

    public OracleTNSParser() {}

   /**
    * Parse ORACLE TNSNAMES.ORA and extract the mapping
    * between db server name and host(either name or ip address).
    */
    public void parse(String fileName) throws IOException {
        InputStream input = null;
       try {
           input = new FileInputStream(fileName);
           parseInternal(input);
       } finally {
           if (input != null)
               input.close();
       }
   }

    public static void main(String[] args) throws Exception {
        Class.forName("oracle.jdbc.driver.OracleDriver");
        //String fileName = "C:\\oracle\\network\\ADMIN\\TNSNAMES.ORA";
        //String fileName = "C:\\orant\\NET80\\ADMIN\\TNSNAMES.ORA";
        String fileName = "\\\\ct-prod2\\apps\\java\\oracle\\TNSNAMES.ORA";
        try {
            IDBPropertyParser parser = new OracleTNSParser();
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
