package aff.confirm.opsmanager.common;

import aff.confirm.common.util.JndiUtil;
import aff.confirm.jboss.common.util.DbInfoWrapper;
import oracle.jdbc.OracleConnection;
import org.jboss.jca.adapters.jdbc.WrappedConnection;
import org.jboss.logging.Logger;

import javax.annotation.Resource;
import javax.ejb.SessionContext;
import javax.interceptor.AroundInvoke;
import javax.interceptor.InvocationContext;
import javax.naming.NamingException;
import javax.sql.DataSource;
import java.sql.Connection;
import java.sql.SQLException;

public abstract class BaseOpsBean {
	protected Connection affinityConnection;
	protected String  affinityDatabase;
	protected String dbUrl;
	
	
	@Resource(name="DBInfo")
	protected String dbInfoJndiName = "DBInfo";
	
	@Resource
	protected SessionContext sc;
	
	public BaseOpsBean() throws NamingException{
		initialize();
	}
	
	private void initialize() throws NamingException{
        DbInfoWrapper dbinfo = new DbInfoWrapper(dbInfoJndiName);
		affinityDatabase = dbinfo.getDatabaseName();
		dbUrl = dbinfo.getDBUrl();

        if (affinityConnection == null)
            try {
                getDbConnectionFromPool();
                Logger.getLogger(this.getClass()).info("Connected...to " + affinityDatabase);
            } catch (Exception e) {
                Logger.getLogger( this.getClass()).error( "ERROR", e );
            }

        // to be changed later
//		affinityDatabase = "SEMPRA.PROD";
	}
	
	public abstract void prepareForMethodCall();
	
	@AroundInvoke
	public Object getClientName(InvocationContext ic) throws Exception{
		Connection connection = getDbConnectionFromPool();
		prepareForMethodCall();
		try {	
			return ic.proceed();
		}
		finally {
			connection.close();
		}
		
	}
	
	private Connection getDbConnectionFromPool() throws Exception {
		DataSource ds = JndiUtil.lookup("java:jboss/datasources/Aff.SqlSvr.DS");
		Connection connection  = ds.getConnection();
        try {
            WrappedConnection wrapCon;

            affinityConnection = connection;

            if (connection.isWrapperFor(oracle.jdbc.OracleConnection.class))
                affinityConnection = connection.unwrap(OracleConnection.class).physicalConnectionWithin().getWrapper();

        } catch (SQLException e) {
            Logger.getLogger( this.getClass()).error( "ERROR", e );
        }
		return connection;
	}

	/* Not used this function anymore, bcos we are using connection pool */
	public void cleanup() {
		Logger.getLogger(this.getClass()).info("Clean up begin for the bean..." );
		if (affinityConnection != null){
			try {
				affinityConnection.close();
				Logger.getLogger(this.getClass()).info("Affinity Connection closes successfully." );
			} catch (SQLException e) {
			}
			
		}
		
	}
	
}
