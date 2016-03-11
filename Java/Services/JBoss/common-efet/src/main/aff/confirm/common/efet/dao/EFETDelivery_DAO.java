package aff.confirm.common.efet.dao;

import com.sun.rowset.CachedRowSetImpl;

import javax.sql.rowset.CachedRowSet;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;

//import sun.jdbc.rowset.CachedRowSet;

/**
 * User: ifrankel
 * Date: Aug 18, 2003
 * Time: 1:16:41 PM
 * To change this template use Options | File Templates.
 */
public class EFETDelivery_DAO {
    private java.sql.Connection connection;

    public EFETDelivery_DAO(java.sql.Connection pConnection) throws SQLException, Exception {
        connection = pConnection;
        //refreshCRS();
    }

    public CachedRowSet getCRS(double pTradeId) throws SQLException, Exception {
        CachedRowSet crs;
        ResultSet rs = null;
        //crs = new CachedRowSet();
        crs = new CachedRowSetImpl();
        PreparedStatement statement = null;
        String sqlStatement;
        try {
            sqlStatement = " select * from infinity_mgr.dlvry"   +
                            " where active_flag = 'Y'"   +
                            " and exp_dt = '31-dec-2299'"   +
                            " and dlvry_ind = 'N'"   +
                            " and prmnt_trade_leg_id in "   +
                            " (select prmnt_id from infinity_mgr.trade_leg"   +
                            " where prmnt_trade_id = ?"   +
                            " and exp_dt = '31-dec-2299'"   +
                            " and active_flag = 'Y') order by dlvry_dt"  ;
            //sqlStatement = "select * from infinity_mgr.trade_leg where prmnt_trade_id = ?";
            statement = connection.prepareStatement(sqlStatement);
            statement.setDouble(1,pTradeId);
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
        return crs;
    }

    /**
     * There is a bug in the rowset1.0ea4 version of CachedRowSet that
     * throws an ClassCastException when you access a date field. This
     * routine works around that problem by getting the date out in a
     * way that avoids the problem.
     * @param pFieldName
     * @return The Date for the fieldname passed.
     * @throws java.sql.SQLException
     * @throws java.text.ParseException
     */
    public java.sql.Date getSqlDateFromCRSString(String pFieldName, CachedRowSet pCrs )
            throws SQLException, ParseException {
        SimpleDateFormat sdfDate = new SimpleDateFormat("yyyy-MM-dd");;
        java.sql.Date crsDate;
        String dateString = pCrs.getString(pFieldName);
        Date trdDt = sdfDate.parse(dateString);
        crsDate = new java.sql.Date(trdDt.getTime());
        return crsDate;
    }

    public Date getUtilDateFromCRSString(String pFieldName, CachedRowSet pCrs )
            throws SQLException, ParseException {
        SimpleDateFormat sdfDate = new SimpleDateFormat("yyyy-MM-dd");;
        //java.util.Date crsDate;
        String dateString = pCrs.getString(pFieldName);
        Date trdDt = sdfDate.parse(dateString);
        //crsDate = new java.util.Date(trdDt.getTime());
        return trdDt;
    }

}
