package aff.confirm.common.dao;

//import sun.jdbc.rowset.CachedRowSet;

import com.sun.rowset.CachedRowSetImpl;

import javax.sql.rowset.CachedRowSet;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * User: ifrankel
 * Date: Jan 15, 2004
 * Time: 1:54:20 PM
 * To change this template use Options | File Templates.
 */
public class DAOBase {
    public final String EMPTY_STRING = "NONE";
    protected java.sql.Connection connection;
    protected String crsSqlStatement = EMPTY_STRING;
    //public CachedRowSetImpl crs;
    public CachedRowSet crs;

    protected void refreshCRS() throws SQLException, Exception {
        if (crsSqlStatement.equalsIgnoreCase(EMPTY_STRING))
            throw new Exception("Class Implementation Error: crsSqlStatement not defined.");
        ResultSet rs = null;
        //crs = new CachedRowSetImpl();
        crs = new CachedRowSetImpl();
        PreparedStatement statement = null;
        String sqlStatement;
        try {
            statement = connection.prepareStatement(crsSqlStatement);
            rs = statement.executeQuery();
            crs.populate(rs);
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

    /**
     * There is a bug in the rowset1.0ea4 version of CachedRowSet that
     * throws an ClassCastException when you access a date field. This
     * routine works around that problem by getting the date out in a
     * way that avoids the problem.
     * @param pFieldName
     * @return The Date for the fieldname passed.
     * @throws SQLException
     * @throws ParseException
     */
    protected java.sql.Date getDateFromCRSString(String pFieldName )
            throws SQLException, ParseException {
        SimpleDateFormat sdfDate = new SimpleDateFormat("yyyy-MM-dd");;
        java.sql.Date crsDate;
        String dateString = crs.getString(pFieldName);
        Date trdDt = sdfDate.parse(dateString);
        crsDate = new java.sql.Date(trdDt.getTime());
        return crsDate;
    }


}
