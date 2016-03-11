package cnf.docflow.util;

import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.sql.Types;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;

/**
 * Created by jvega on 7/17/2015.
 */
public class ConversionUtils {
    //private static SimpleDateFormat sdfDate = new SimpleDateFormat("dd-MMM-yyyy", Locale.US);
    public static SimpleDateFormat sdfDateShort = new SimpleDateFormat("MM/dd/yyyy", Locale.US);
    //private static SimpleDateFormat sdfDateTime = new SimpleDateFormat("dd-MMM-yyyy HH:mm:ss", Locale.US);
    public static SimpleDateFormat sdfTime = new SimpleDateFormat("HH:mm:ss", Locale.US);
    public static SimpleDateFormat sdfDateTimeMillis = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSS", Locale.US);
    public static SimpleDateFormat sdfDateTimeSecs = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.US);
    public static SimpleDateFormat sdfDateTimeMins = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm", Locale.US);

    //general
    public static boolean isEmptyString(String textVal) {
        return textVal == null || textVal.equals("") || textVal.isEmpty();
    }

    public static String getDatePattern(String textVal) {
        String result = "";
        if (textVal.length() == sdfDateTimeMillis.toPattern().length()-2) {
            result = sdfDateTimeMillis.toPattern();
        } else if (textVal.length()+2 == sdfDateTimeSecs.toPattern().length()-2) {
            result = sdfDateTimeSecs.toPattern();
        } else if (textVal.length()+2 == sdfDateTimeMins.toPattern().length()-2) {
            result = sdfDateTimeMins.toPattern();
        } else System.out.println("WARNING: getDatePattern ("+textVal+") - No matching pattern length found. Available ("+sdfDateTimeMillis.toPattern()+","+sdfDateTimeSecs.toPattern()+","+sdfDateTimeMins.toPattern()+")");
        return result;
    }

    public static java.sql.Date convertToDateShort(String textVal) throws ParseException {
        java.sql.Date ret = null;
        Date utilDate = sdfDateShort.parse(textVal);
        ret = new java.sql.Date(utilDate.getTime());
        return ret;
    }

    public static java.sql.Date convertToDateLong(String textVal) throws ParseException {
        java.sql.Date ret = null;
        Date utilDate=null;
        if (textVal.length()==sdfDateTimeSecs.toPattern().length()-2) {
            utilDate = sdfDateTimeSecs.parse(textVal);
        } else if (textVal.length()==sdfDateTimeMillis.toPattern().length()-2) {
                utilDate = sdfDateTimeMillis.parse(textVal);
        } else if (textVal.length() == sdfDateTimeMins.toPattern().length()-2) {
                   utilDate = sdfDateTimeMins.parse(textVal);
        }  else throw new ParseException("convertToDateLong - No matching pattern length found.",0);
        ret = new java.sql.Date(utilDate.getTime());
        return ret;
    }

    public static java.sql.Date convertToDateTime(String textVal) throws ParseException {
        java.sql.Date ret = null;
        Date utilDate = null;
        if (textVal.length()==sdfDateTimeMillis.toPattern().length()-2) {
            utilDate = sdfDateTimeMillis.parse(textVal);
        } else if (textVal.length()==sdfDateTimeSecs.toPattern().length()-2) {
                utilDate = sdfDateTimeSecs.parse(textVal);
        } else if (textVal.length() == sdfDateTimeMins.toPattern().length()-2) {
            utilDate = sdfDateTimeMins.parse(textVal);
        } else throw new ParseException("convertToDateTime - No matching pattern length found.",0);
        ret = new java.sql.Date(utilDate.getTime());
        return ret;
    }

    public static java.sql.Timestamp  convertToTimeStamp(String textVal) throws ParseException {
        java.sql.Timestamp ret = null;
        Date utilDate = null;
        if (textVal.length()==sdfDateTimeMillis.toPattern().length()-2) {
            utilDate = sdfDateTimeMillis.parse(textVal);
        } else if (textVal.length()==sdfDateTimeSecs.toPattern().length()-2) {
                utilDate = sdfDateTimeSecs.parse(textVal);
        } else if (textVal.length() == sdfDateTimeMins.toPattern().length()-2) {
            utilDate = sdfDateTimeMins.parse(textVal);
        } else throw new ParseException("convertToTimeStamp - No matching pattern length found.",0);
        ret = new java.sql.Timestamp(utilDate.getTime());
        return ret;
    }

    public static java.sql.Date  convertToTime(String textVal) throws ParseException {
        java.sql.Date ret = null;
        Date utilDate = sdfTime.parse(textVal);
        ret = new java.sql.Date(utilDate.getTime());
        return ret;
    }

    public static Double convertToDouble(String textVal) throws NumberFormatException {
        Double ret = 0.0;
        ret = Double.parseDouble(textVal);
        return ret;
    }

    public static int convertToInteger(String textVal) throws NumberFormatException {
        int ret = 0;
        ret = Integer.parseInt(textVal);
        return ret;
    }

    //Prepared Statement methods
    public static void setStatementString(int pParamIndex, String pValue, PreparedStatement pStatement)
            throws SQLException {
        if (!isEmptyString(pValue))
            pStatement.setString(pParamIndex, pValue);
        else
            pStatement.setNull(pParamIndex, Types.VARCHAR);
    }


    public static void setStatementDouble(int pParamIndex, String pValue, PreparedStatement pStatement)
            throws SQLException {
        if (!isEmptyString(pValue))
            pStatement.setDouble(pParamIndex, convertToDouble(pValue));
        else
            pStatement.setNull(pParamIndex, Types.NUMERIC);
    }

    public static void setStatementInt(int pParamIndex, String pValue, PreparedStatement pStatement)
            throws SQLException {
        if (!isEmptyString(pValue))
            pStatement.setInt(pParamIndex, convertToInteger(pValue));
        else
            pStatement.setNull(pParamIndex, Types.NUMERIC);
    }

    public static void setStatementDateShort(int pParamIndex, String pValue, PreparedStatement pStatement)
            throws SQLException,ParseException {
        if (!isEmptyString(pValue))
            pStatement.setDate(pParamIndex, convertToDateShort(pValue));
        else
            pStatement.setNull(pParamIndex, Types.DATE);
    }

    public static void setStatementDateLong(int pParamIndex, String pValue, PreparedStatement pStatement)
            throws SQLException,ParseException {
        if (!isEmptyString(pValue))
            pStatement.setDate(pParamIndex, convertToDateLong(pValue));
        else
            pStatement.setNull(pParamIndex, Types.DATE);
    }

    public static void setStatementDateTime(int pParamIndex, String pValue, PreparedStatement pStatement)
            throws SQLException,ParseException {
        if (!isEmptyString(pValue))
            pStatement.setDate(pParamIndex, convertToDateTime(pValue));
        else
            pStatement.setNull(pParamIndex, Types.DATE);
    }

    public static void setStatementTimeStamp(int pParamIndex, String pValue, PreparedStatement pStatement)
            throws SQLException,ParseException {
        if (!isEmptyString(pValue))
            pStatement.setTimestamp(pParamIndex, convertToTimeStamp(pValue));
        else
            pStatement.setNull(pParamIndex, Types.TIMESTAMP);
    }

    public static void setStatementTime(int pParamIndex, String pValue, PreparedStatement pStatement)
            throws SQLException,ParseException {
        if (!isEmptyString(pValue))
            pStatement.setDate(pParamIndex, convertToTime(pValue));
        else
            pStatement.setNull(pParamIndex, Types.DATE);
    }

}
