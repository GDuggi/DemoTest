package cnf.docflow.dao;

import cnf.docflow.util.ConversionUtils;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.text.DecimalFormat;

/**
 * Created by jvega on 7/17/2015.
 */
public class SqlDAO {
    protected java.sql.Connection dbConnection;
    private DecimalFormat df = new DecimalFormat("#0;-#0");
    public final String schemaName = "ConfirmMgr";

    protected int getNextSequence(String seqName) throws SQLException {
        int result = 0;
        PreparedStatement preparedStatement = null;
        ResultSet rs = null;
        try {
            preparedStatement = dbConnection.prepareStatement("SELECT NEXT VALUE for " + schemaName + "." + seqName + " as next_value");
            //was used for testing
            //preparedStatement = dbConnection.prepareStatement("SELECT CAST(current_value as int) current_value FROM sys.sequences WHERE name = ?");
            rs = preparedStatement.executeQuery();
            if (rs.next()) {
                result = (rs.getInt("next_value"));
            }
        } finally {
            if (preparedStatement != null) {
                preparedStatement.close();
                preparedStatement = null;
            }
            if (rs != null) {
                rs.close();
                rs = null;
            }
        }
        return result;
    }

    public int getTradeID(String pTicket, String pTradeSystemCode) throws SQLException {
        int result = 0;
        PreparedStatement preparedStatement = null;
        ResultSet rs = null;
        String statementSQL = "";
        try {
            statementSQL = "select id from " + schemaName + ".trade where trd_sys_ticket = ? and trd_sys_code = ?";
            preparedStatement = dbConnection.prepareStatement(statementSQL);
            ConversionUtils.setStatementString(1, pTicket, preparedStatement);
            ConversionUtils.setStatementString(2, pTradeSystemCode, preparedStatement);
            rs = preparedStatement.executeQuery();
            if (rs.next()) {
                result = rs.getInt("id");
            }
        } finally {
            if (rs != null) {
                rs.close();
                rs = null;
            }
            if (preparedStatement != null) {
                preparedStatement.close();
            }
        }
        return result;
    }
}
