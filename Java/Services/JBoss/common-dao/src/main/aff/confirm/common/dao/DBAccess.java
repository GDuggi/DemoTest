package aff.confirm.common.dao;

/**
 * User: ifrankel
 * Date: Mar 25, 2004
 * Time: 7:21:26 AM
 * To change this template use Options | File Templates.
 */

public class DBAccess {
    /*private String dbInfoName;
    private java.sql.Connection dbInfoConnection;

    public String getDbInfoName() {
        return dbInfoName;
    }

    public void setDbInfoName(String dbInfoName) {
        this.dbInfoName = dbInfoName;
    }

    private void connectToDB() throws NamingException, SQLException {
        DBInfo dbinfo = getDbInfo();
        if(dbinfo!=null)
            dbInfoConnection = DriverManager.getConnection (dbinfo.getDBUrl(),dbinfo.getDBUserName(),dbinfo.getDBPassword());
    }

    public java.sql.Connection getSqlConnection() throws SQLException, NamingException {
        if(dbInfoName != null){
            if(dbInfoConnection == null){
                connectToDB();
            }
            return dbInfoConnection;
        }else
            return null;

    }

    private Statement createStatement(boolean prepared, String sql)throws SQLConnectionAllocationFailure{
      if(dbInfoName != null){
          Statement stmnt = null;
          int counter = 3;
          SQLException lastException = null;
          while ((counter != 0) && (stmnt == null)){
              try{
                  counter--;
                  if(prepared)
                    stmnt = getSqlConnection().prepareStatement(sql);
                  else
                    stmnt = getSqlConnection().createStatement();
              }catch(SQLException e){
                  dbInfoConnection = null;
                  lastException = e;
                  Logger.getLogger(e+" Reconnecting in 5 sec: "+counter);
                  try {
                      Thread.sleep(5000);
                  } catch (InterruptedException e1) {
                      Logger.getLogger(e1);
                  }
              } catch (Exception e) {
                  log.error(e);
                  break;
              }
          }
          if(stmnt == null){
              throw new SQLConnectionAllocationFailure(lastException.getMessage());
          }
          return stmnt;
      }else return null;
    }

    public Statement createStatement()throws SQLException, SQLConnectionAllocationFailure{
      return createStatement(false,null);
    }

    public PreparedStatement createPreparedStatement(String sql)throws SQLException, SQLConnectionAllocationFailure{
      return (PreparedStatement)createStatement(true,sql);
    }*/

}
