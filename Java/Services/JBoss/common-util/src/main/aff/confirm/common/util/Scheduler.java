package aff.confirm.common.util;

import com.sun.rowset.CachedRowSetImpl;

import javax.sql.rowset.CachedRowSet;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * User: ifrankel
 * Date: Jun 14, 2004
 * Time: 6:24:23 AM
 * 1. Read in template for scheduled times.
 * 2. Query schedule to see if time has arrived.
 *    a. If it has, check to see if has already been updated in database.
 *
 *
 *    b. If not, just return false
 */
public class Scheduler {
    private Connection connection;
    private String scheduleCode;
    private CachedRowSet templateCRS;

    public Scheduler(String pScheduleCode, Connection pConnection) throws SQLException, Exception {
        this.connection = pConnection;
        this.scheduleCode = pScheduleCode;
        refreshTemplateCRS();
    }

    public boolean isEventArrived(java.sql.Date pDate, String pTime) throws SQLException {
        boolean eventArrived = false;
        String templateTime = "";
        templateCRS.beforeFirst();
        while (templateCRS.next()) {
            templateTime = templateCRS.getString("SCHED_TIME");
            //Has the time arrived for the scheduled event
            if (templateTime.compareToIgnoreCase(pTime) >= 0){
                //Has it already executed
                if (!isEventComplete(pDate,pTime)){
                    eventArrived = true;

                }
            }
        }


        return eventArrived;
    }


    private boolean isEventComplete(java.sql.Date pDate, String pTime) throws SQLException {
        boolean eventComplete = false;
        int count = 0;
        PreparedStatement statement = null;
        ResultSet rs = null;
        try {
            statement = connection.prepareStatement("SELECT count(*) cnt from " +
                    "ifrankel.sched_complete where SCHED_MAST_CODE = ? and SCHED_TIME = ? and COMPLETED_DT = ?");
            statement.setString(1, scheduleCode);
            statement.setString(2, pTime);
            statement.setDate(3, pDate);
            rs = statement.executeQuery();
            if (rs.next()) {
                count = (rs.getInt("cnt"));
            }
            eventComplete = count > 0;
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
         return eventComplete;
    }



    private void refreshTemplateCRS() throws SQLException, Exception {
        String crsSqlStatement;
        crsSqlStatement = "select * from ifrankel.sched_template where code = ?";
        ResultSet rs = null;
        //crs = new CachedRowSetImpl();
        templateCRS = new CachedRowSetImpl();
        PreparedStatement statement = null;
        try {
            statement = connection.prepareStatement(crsSqlStatement);
            statement.setString(1, scheduleCode);
            rs = statement.executeQuery();
            templateCRS.populate(rs);
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
     * @throws java.text.ParseException
     */
    private java.sql.Date getDateFromCRSString(String pFieldName )
            throws SQLException, ParseException {
        SimpleDateFormat sdfDate = new SimpleDateFormat("yyyy-MM-dd");;
        java.sql.Date crsDate;
        String dateString = templateCRS.getString(pFieldName);
        Date trdDt = sdfDate.parse(dateString);
        crsDate = new java.sql.Date(trdDt.getTime());
        return crsDate;
    }

}
