package aff.confirm.common.util;

import javax.naming.NamingException;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.sql.Types;

/**
 * User: ifrankel
 * Date: Aug 5, 2003
 * Time: 2:48:13 PM
 * To change this template use Options | File Templates.
 */
public class DAOUtils {


    public static void setStatementString(int pParamIndex, String pFieldValue, PreparedStatement pStatement)
            throws SQLException {
        if (pFieldValue != null)
            pStatement.setString(pParamIndex, pFieldValue);
        else
            pStatement.setNull(pParamIndex, Types.VARCHAR);
    }


    public static void setStatementDouble(int pParamIndex, double pFieldValue, PreparedStatement pStatement)
            throws SQLException {
        //if (pFieldValue >-1)
        pStatement.setDouble(pParamIndex, pFieldValue);
        /*else
            pStatement.setNull(pParamIndex, Types.DOUBLE);*/
    }

    public static void setStatementInt(int pParamIndex, int pFieldValue, PreparedStatement pStatement)
            throws SQLException {
        //if (pFieldValue >-1)
        pStatement.setInt(pParamIndex, pFieldValue);
       /* else
            pStatement.setNull(pParamIndex, Types.INTEGER);*/
    }

    public static void setStatementDate(int pParamIndex, java.sql.Date pFieldValue, PreparedStatement pStatement)
            throws SQLException {
        if (pFieldValue != null)
            pStatement.setDate(pParamIndex, pFieldValue);
        else
            pStatement.setNull(pParamIndex, Types.DATE);
    }

    public Connection reopenConnection(String pDBInfoName, Connection con) throws NamingException {
        return con;
    }

    public boolean validateConnection(String pDBInfoName, java.sql.Connection pConnection)
            throws SQLException, NamingException {
        //java.sql.Connection dbcon;
        PreparedStatement statement;
        String sql = "select sysdate from dual";
        try {
            statement = pConnection.prepareStatement(sql);
        } catch (SQLException e) {
            pConnection = reopenConnection(pDBInfoName, pConnection);
            statement = pConnection.prepareStatement(sql);
        }
        return true;
    }

}
