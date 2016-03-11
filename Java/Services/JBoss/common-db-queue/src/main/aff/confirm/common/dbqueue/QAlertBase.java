package aff.confirm.common.dbqueue;


import com.sun.rowset.CachedRowSetImpl;

import javax.sql.rowset.CachedRowSet;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

/**
 * User: ifrankel
 * Date: Jan 16, 2004
 * Time: 8:17:26 AM
 * To change this template use Options | File Templates.
 */
public class QAlertBase {
    public final String EMPTY_STRING = "NONE";
    protected java.sql.Connection connection;
    protected String updateSQL = EMPTY_STRING;
    protected String readyToProcessSQL = EMPTY_STRING;

    public void updateAlertRecord(double pId, String pProcessedFlag ) throws SQLException, Exception {
        if (updateSQL.equalsIgnoreCase(EMPTY_STRING))
            throw new Exception("Implementation error: updateSQL field was not assigned.");
        PreparedStatement statement = null;
        try {
            statement = connection.prepareStatement(updateSQL);
            statement.setString(1, pProcessedFlag);
            statement.setDouble(2, pId);
            statement.executeUpdate();
        } finally {
            try {
                if (statement != null)
                    statement.close();
            } catch (SQLException e) { }
            statement = null;
        }
    }

    public CachedRowSet getReadyToProcess() throws SQLException, Exception {
        if (readyToProcessSQL.equalsIgnoreCase(EMPTY_STRING))
            throw new Exception("Implementation error: readyToProcessSQL field was not assigned.");
        PreparedStatement statement = null;
        ResultSet rs = null;
        CachedRowSet crs;
        crs = new CachedRowSetImpl();
        try {
            statement = connection.prepareStatement(readyToProcessSQL);
            rs = statement.executeQuery();
            crs.populate(rs);
        } catch (SQLException e) {
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
        return crs;

    }
    /**
     * There is a bug in the rowset1.0ea4 version of CachedRowSet that
     * throws an ClassCastException when you access a date field. This
     * routine works around that problem by getting the date out in a
     * way that avoids the problem.
     * @param pCrs
     * @param pFieldName
     * @return The Date for the fieldname passed.
     * @throws SQLException
     * @throws java.text.ParseException
     */
   /* public java.sql.Date getDateFromCRSString(CachedRowSet pCrs, String pFieldName )
            throws SQLException, ParseException {
        SimpleDateFormat sdfCrsDate = new SimpleDateFormat("yyyy-MM-dd");;
        java.sql.Date crsDate;
        String dateString = pCrs.getString(pFieldName);
        Date trdDt = sdfCrsDate.parse(dateString);
        crsDate = new java.sql.Date(trdDt.getTime());
        return crsDate;
    }*/
}
