package aff.confirm.updateconfirmstatus.dao;

import javax.sql.DataSource;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

/**
 * User: hblumenf
 * Date: 11/23/2015
 * Time: 4:14 PM
 * Copyright Amphora Inc. 2015
 */
public class ExternalWorkflowNameDao
{
    private final DataSource dataSource;

    public ExternalWorkflowNameDao(DataSource dataSource)
    {
        this.dataSource = dataSource;
    }

    public String getFromRqmtCode(String rqmtCode) throws SQLException
    {
        String SQL_SELECT = "select EXT_WORKFLOW_NAME from ConfirmMgr.RQMT r where r.CODE = ?";
        try (Connection conn = dataSource.getConnection();
             PreparedStatement ps = conn.prepareStatement(SQL_SELECT))
        {
            ps.setString(1, rqmtCode);
            try (ResultSet rs = ps.executeQuery())
            {
                if (rs.next())
                {
                    return rs.getString("EXT_WORKFLOW_NAME");
                }
                return null;
            }
        }
    }
}
