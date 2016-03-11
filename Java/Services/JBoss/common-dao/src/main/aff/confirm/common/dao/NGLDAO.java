package aff.confirm.common.dao;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;

/**
 * User: ifrankel
 * Date: Aug 18, 2003
 * Time: 1:16:41 PM
 * To change this template use Options | File Templates.
 */
public class NGLDAO {
    public java.sql.Connection connection;
    public ArrayList cdtyList;

    public NGLDAO(java.sql.Connection pConnection) throws SQLException, Exception {
        connection = pConnection;
        cdtyList = new ArrayList();
       // refreshArrayList();
    }

    private void refreshArrayList() throws SQLException, Exception {
        ResultSet rs = null;
        //crs = new CachedRowSetImpl();
        PreparedStatement statement = null;
        String sqlStatement = "select cmdty_code from commodity_group where parent_cmdty_code = 'BIOFL'";
        try {
            statement = connection.prepareStatement(sqlStatement);
            rs = statement.executeQuery();
            while (rs.next()) {
                cdtyList.add(rs.getString("cmdty_code").trim());
            }

        } finally {
            if (statement != null) {
                statement.close();
                statement = null;
            }
            if (rs != null) {
                rs.close();
                rs = null;
            }
        }
    }


    public boolean isNGL(String pCdtyCode) throws SQLException {
        boolean isNgl = false;
        if (cdtyList.indexOf(pCdtyCode.trim())>-1)
            isNgl = true;
        return isNgl;
    }

}
