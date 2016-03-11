package aff.confirm.common.util;

import oracle.jdbc.OracleCallableStatement;

import javax.jms.JMSException;
import javax.jms.Message;
import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.sql.Types;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;

/**
 * User: ifrankel
 * Date: Feb 24, 2003
 * Time: 2:37:16 PM
 * To change this template use Options | File Templates.
 */
public class MessageUtils {
    private static SimpleDateFormat sdfDate = new SimpleDateFormat("yyyy-MM-dd", Locale.US);
    private static SimpleDateFormat sdfDateTime = new SimpleDateFormat("MM/dd/yyyy HH:mm:ss", Locale.US);
    private static SimpleDateFormat sdfTime = new SimpleDateFormat("HH:mm:ss", Locale.US);

    //Prepared Statement methods
    public static void setStatementString(int pParamIndex, String pFieldName, PreparedStatement statement, Message pMessage)
            throws JMSException, SQLException {
        if (pMessage.propertyExists(pFieldName) &&
                pMessage.getStringProperty(pFieldName) != null) {
            statement.setString(pParamIndex, pMessage.getStringProperty(pFieldName).trim());
        } else {
            statement.setNull(pParamIndex, Types.VARCHAR);
        }
    }

    public static void setStatementDouble(int pParamIndex, String pFieldName, PreparedStatement statement, Message pMessage)
            throws JMSException, SQLException {
        if (pMessage.propertyExists(pFieldName) &&
                pMessage.getStringProperty(pFieldName) != null) {
            statement.setDouble(pParamIndex, pMessage.getDoubleProperty(pFieldName));
        } else {
            statement.setNull(pParamIndex, Types.DOUBLE);
        }
    }

    public static void setStatementDateTime(int pParamIndex, String pFieldName, PreparedStatement statement, Message pMessage)
            throws JMSException, SQLException, ParseException {
        if (pMessage.propertyExists(pFieldName) &&
                pMessage.getStringProperty(pFieldName) != null)
        //!pMessage.getStringProperty(pFieldName).equalsIgnoreCase(null) )
        {
            Date trdDt = sdfDateTime.parse(pMessage.getStringProperty(pFieldName));
            statement.setDate(pParamIndex, new java.sql.Date(trdDt.getTime()));
        } else
            statement.setNull(pParamIndex, Types.DATE);
    }

    public static void setStatementDate(int pParamIndex, String pFieldName, PreparedStatement statement, Message pMessage)
            throws JMSException, SQLException, ParseException {
        if (pMessage.propertyExists(pFieldName) &&
                pMessage.getStringProperty(pFieldName) != null) {
            Date trdDt = sdfDate.parse(pMessage.getStringProperty(pFieldName));
            statement.setDate(pParamIndex, new java.sql.Date(trdDt.getTime()));
        } else
            statement.setNull(pParamIndex, Types.DATE);
    }

    public static void setStatementTime(int pParamIndex, String pFieldName, PreparedStatement statement, Message pMessage)
            throws JMSException, SQLException, ParseException {
        if (pMessage.propertyExists(pFieldName) &&
                pMessage.getStringProperty(pFieldName) != null) {
            Date trdDt = sdfTime.parse(pMessage.getStringProperty(pFieldName));
            statement.setDate(pParamIndex, new java.sql.Date(trdDt.getTime()));
        } else
            statement.setNull(pParamIndex, Types.DATE);
    }

    //Overloaded versions that support OracleCallableStatement
    public static void setStatementString(int pParamIndex, String pFieldName, OracleCallableStatement statement, Message pMessage)
            throws JMSException, SQLException {
        if (pMessage.propertyExists(pFieldName) &&
                pMessage.getStringProperty(pFieldName) != null) {
            statement.setString(pParamIndex, pMessage.getStringProperty(pFieldName).trim());
        } else {
            statement.setNull(pParamIndex, Types.VARCHAR);
        }
    }

    public static void setStatementDouble(int pParamIndex, String pFieldName, OracleCallableStatement statement, Message pMessage)
            throws JMSException, SQLException {
        if (pMessage.propertyExists(pFieldName) &&
                pMessage.getStringProperty(pFieldName) != null) {
            statement.setDouble(pParamIndex, pMessage.getDoubleProperty(pFieldName));
        } else {
            statement.setNull(pParamIndex, Types.DOUBLE);
        }
    }

    public static void setStatementDate(int pParamIndex, String pFieldName, OracleCallableStatement statement, Message pMessage)
            throws JMSException, SQLException, ParseException {
        if (pMessage.propertyExists(pFieldName) &&
                pMessage.getStringProperty(pFieldName) != null) {
            Date trdDt = sdfDateTime.parse(pMessage.getStringProperty(pFieldName));
            statement.setDate(pParamIndex, new java.sql.Date(trdDt.getTime()));
        } else {
            statement.setNull(pParamIndex, Types.DATE);
        }
    }

    public static String getMessageValue(String pFieldName, Message pMessage)
            throws JMSException {
        String msgValue = "";
        if (pMessage.propertyExists(pFieldName) &&
                pMessage.getStringProperty(pFieldName) != null) {
            msgValue = pMessage.getStringProperty(pFieldName);
        }
        return msgValue;
    }
}
