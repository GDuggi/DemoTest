package aff.confirm.updateconfirmstatus.dao;

import javax.sql.DataSource;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.logging.Logger;

/**
 * User: hblumenf
 * Date: 11/23/2015
 * Time: 1:09 PM
 * Copyright Amphora Inc. 2015
 */
public class TradeKeyDao
{
    public final static Logger log = Logger.getLogger(TradeKeyDao.class.toString());
    private final DataSource dataSource;

    public TradeKeyDao(DataSource dataSource)
    {
        this.dataSource = dataSource;
    }

    public TradeKey getByTradeRqmtId(int id) throws TradeKeyLookupException
    {
        String SQL_SELECT = "select distinct t.trd_sys_code, t.trd_sys_ticket from confirmMgr.trade t " +
                " inner join confirmmgr.TRADE_RQMT r on r.TRADE_ID = t.ID " +
                "where r.id = ? ";
        try ( Connection conn = dataSource.getConnection();
              PreparedStatement ps = conn.prepareStatement(SQL_SELECT))
        {
            ps.setInt(1,id);
            try (ResultSet rs = ps.executeQuery())
            {
                if (rs.next())
                {
                    return resultSetToTradeKey(rs);
                }
                throw new TradeKeyLookupException("No trade defined for id = " + id);
            }
        }
        catch (SQLException e)
        {
            throw new TradeKeyLookupException("Error getting trade key for id = " + id + ":" + e.getMessage(), e);
        }
    }

    public TradeKey getById(int id)
            throws TradeKeyLookupException
    {

        String SQL_SELECT = "select TRD_SYS_CODE, TRD_SYS_TICKET from ConfirmMgr.TRADE where ID = ?";
        try ( Connection conn = dataSource.getConnection();
              PreparedStatement ps = conn.prepareStatement(SQL_SELECT))
        {
            ps.setInt(1,id);
            try (ResultSet rs = ps.executeQuery())
            {
                if (rs.next())
                {
                    return resultSetToTradeKey(rs);
                }
                throw new TradeKeyLookupException("No trade defined for id = " + id);
            }
        }
        catch (SQLException e)
        {
            throw new TradeKeyLookupException("Error getting trade key for id = " + id + ":" + e.getMessage(), e);
        }
    }

    private TradeKey resultSetToTradeKey(ResultSet rs) throws SQLException
    {
        return new TradeKey(
                rs.getString("TRD_SYS_CODE"),
                rs.getString("TRD_SYS_TICKET")
        );
    }
}
