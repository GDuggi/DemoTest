package aff.confirm.jboss.utils;

import org.jboss.logging.Logger;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.text.SimpleDateFormat;
import java.util.Locale;

public class Store
{
  public static SimpleDateFormat sdfLocalStoreDateTime = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss", Locale.US);

  public static void create(String storeName, String[] fieldNames, String[] fieldTypes, Connection con) throws SQLException { boolean tableExist = false;
    Statement stmnt = null;
    ResultSet rs = null;
    if (fieldNames.length != fieldTypes.length)
      throw new SQLException("FieldNames and FieldTypes arrays have different sizes");
    try {
      String sql = "SELECT count(*) FROM SYSTEM_TABLES WHERE TABLE_NAME = '" + storeName.toUpperCase() + "'";
      stmnt = con.createStatement();
      rs = stmnt.executeQuery(sql);
      if (rs.next()) {
        tableExist = rs.getInt(1) == 1;
      }
      if (!tableExist) {
        String createFields = "";
        String insFields = "";
        String values = "";
        for (int i = 0; i < fieldNames.length; i++) {
          createFields = createFields + fieldNames[i] + " " + fieldTypes[i] + ",";
          insFields = insFields + fieldNames[i] + ",";
          values = values + "null,";
        }
        if (createFields.length() > 0) {
          createFields = createFields.substring(0, createFields.length() - 1);
          insFields = insFields.substring(0, insFields.length() - 1);
          values = values.substring(0, values.length() - 1);
        }
        sql = "CREATE TABLE " + storeName + "(" + createFields + ")";
        stmnt.execute(sql);
        sql = "INSERT INTO " + storeName + " (" + insFields + ") VALUES(" + values + ")";
        stmnt.execute(sql);
        Logger.getLogger(Store.class).info("created table " + storeName);
      } else {
        Logger.getLogger(Store.class).info("table " + storeName + " exist");
      }
    } finally { rs.close();
      rs = null;
      stmnt.close();
      stmnt = null; } }

  public static void drop(String storeName, Connection con)
    throws SQLException
  {
    Statement stmnt = null;
    try {
      String sql = "DROP TABLE " + storeName;
      stmnt = con.createStatement();
      stmnt.execute(sql);
    } finally {
      stmnt.close();
      stmnt = null;
    }
  }

  public static void empty(String storeName, Connection con) throws SQLException {
    String sql = "DELETE FROM " + storeName;
    Statement stmnt = null;
    try {
      stmnt = con.createStatement();
      stmnt.execute(sql);
    } finally {
      stmnt.close();
      stmnt = null;
    }
  }

  public static void set(String storeName, Connection con, String setExpr) throws SQLException {
    String sql = "UPDATE " + storeName + " SET " + setExpr;
    Statement stmnt = null;
    try {
      stmnt = con.createStatement();
      stmnt.execute(sql);
    } finally {
      stmnt.close();
      stmnt = null;
    }
  }

  public static int getInt(String storeName, Connection con, String fieldName) throws SQLException {
    String sql = "SELECT * FROM " + storeName;
    Statement stmnt = null;
    ResultSet rs = null;
    int result = -1;
    try {
      stmnt = con.createStatement();
      rs = stmnt.executeQuery(sql);
      if (rs.next())
        result = rs.getInt(fieldName);
    } finally {
      rs.close();
      rs = null;
      stmnt.close();
      stmnt = null;
    }
    return result;
  }
}
