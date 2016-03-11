package aff.confirm.jboss.jms;

import org.jboss.logging.Logger;

import java.sql.*;
import java.util.Hashtable;

/**
 * User: srajaman
 * Date: Jul 8, 2008
 * Time: 5:13:28 PM
 */
public class EmpDAO {

    private static String sql = "select user_name, emp_id from infinity_mgr.dbase_user";

    private Hashtable empList = new Hashtable();

    private String dbName;
    private String dbUserName;
    private String dbPassword;

    public EmpDAO(String dbName,String userName,String pwd) throws SQLException {
        this.dbName = dbName;
        this.dbUserName = userName;
        this.dbPassword = pwd;
        loadEmpData();
    }
    private void loadEmpData() throws SQLException {

        Connection connection = getOracleConnection();
        Statement stmt = connection.createStatement();
        ResultSet rs = stmt.executeQuery(sql);
        empList  = new Hashtable();
        while (rs.next()){
            String userName = rs.getString("user_name");
            int empId = rs.getInt("emp_id");
            empList.put(userName,new Double(empId));
        }

        stmt.close();
        connection.close();

    }
    
    private Connection   getOracleConnection() throws SQLException {
        Connection connection = DriverManager.getConnection(dbName,dbUserName,dbPassword);
        return connection;
    }
    public double getEmpId(String userId) {

        double returnEmpId = 0;
        if (userId == null){
            return returnEmpId;
        }
        userId = userId.toUpperCase();
        try {
            Double dEmp = (Double) empList.get(userId);
            if ( dEmp != null){
                refreshData();
                dEmp = (Double) empList.get(userId);
                if ( dEmp != null) {
                    returnEmpId = dEmp.doubleValue();
                }
                else {
                    returnEmpId = getFromEmpTable(userId);
                }
            }
        }
        catch (SQLException e){

        }
        return returnEmpId;
    }

    private double getFromEmpTable(String userId) throws SQLException {

        String sql = "select id from infinity_mgr.emp where substr(frst_name,1,1) || lst_name = ?";
        double returnId = 0;
        Connection connection = getOracleConnection();
        PreparedStatement ps = connection.prepareStatement(sql);
        ps.setString(1,userId);
        ResultSet rs = ps.executeQuery();
        if (rs.next()){
           returnId = rs.getDouble("id");
        }
        ps.close();
        connection.close();
        return returnId;

    }

    private void refreshData() {
        try {
            loadEmpData();
        } catch (SQLException e) {
            Logger.getLogger(this.getClass()).error( "ERROR", e );
        }
    }

}
