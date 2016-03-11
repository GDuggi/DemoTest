package aff.confirm.opsmanager.servlet.transmissiongateway;


import org.jboss.logging.Logger;

import javax.naming.InitialContext;
import javax.naming.NamingException;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.sql.DataSource;
import java.io.IOException;
import java.io.PrintWriter;
import java.sql.CallableStatement;
import java.sql.Connection;
import java.sql.SQLException;
import java.sql.Types;
import java.util.Enumeration;

public class XmitResultServlet extends HttpServlet
{

    private static final Logger log = Logger.getLogger(XmitResultServlet.class);

    private DataSource dataSource = null;
    private String databaseJdbcJndi = null;

    public void destroy()
    {
        super.destroy();
    }

    public void init() throws ServletException
    {
        super.init();
        databaseJdbcJndi = System.getProperty("aff.db.confirm.datasource", "java:jboss/datasources/Aff.SqlSvr.DS");

    }

    protected void doGet(HttpServletRequest httpServletRequest, HttpServletResponse httpServletResponse) throws
            ServletException, IOException
    {

        log.info("Incoming parameter" + httpServletRequest.getQueryString());
        try
        {
            updateStatusInDb(httpServletRequest);
        }
        catch (Exception e)
        {
            log.error(e.getMessage(), e);
            throw new ServletException(e);
        }
        PrintWriter output = httpServletResponse.getWriter();
        output.write("OK");
    }

    private void updateStatusInDb(HttpServletRequest httpServletRequest) throws Exception
    {
        int xmitRequestId = Integer.parseInt(getParameterValue(httpServletRequest, "xmitRequestId"));
        String action = getParameterValue(httpServletRequest, "action");
        String methodInd = getParameterValue(httpServletRequest, "methodInd");
        String destination = getParameterValue(httpServletRequest, "destination");
        String comment = getParameterValue(httpServletRequest, "comment");

        GatewayStatus status = GatewayStatus.parse(action);
        persistXmitResult(xmitRequestId, status, methodInd, destination, comment);
        log.info("Saved XMIT_RESULT for xmitRequestId = " + xmitRequestId);
    }

    private String getParameterValue(HttpServletRequest request, String paramName)
    {

        String paramValue = "";
        Enumeration paramNameList = request.getParameterNames();
        while (paramNameList.hasMoreElements())
        {
            String currentParamName = (String) paramNameList.nextElement();
            if (currentParamName.equalsIgnoreCase(paramName))
            {
                return request.getParameter(currentParamName);
            }
        }
        return null;
    }

    @Override
    protected void doPost(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException
    {
        doGet(req, resp);
    }

    public DataSource getDataSource() throws NamingException
    {
        if (dataSource == null)
        {
            InitialContext ctx = new InitialContext();
            dataSource = (DataSource) ctx.lookup(databaseJdbcJndi);
        }
        return dataSource;
    }

    private void persistXmitResult(int xmitRequestId, GatewayStatus status, String methodInd, String destination,
                                   String comment) throws Exception
    {
        try (Connection affinityConnection = getDataSource().getConnection();
             CallableStatement stmnt = affinityConnection.prepareCall(
                     "{ call ConfirmMgr.P_INSERT_XMIT_RESULT(?,?,?,?, ?) }"))
        {
            stmnt.setInt(1, xmitRequestId);
            stmnt.setString(2, status.getDatabaseInd());
            setPossiblyNullString(methodInd, stmnt, 3);
            setPossiblyNullString(destination, stmnt, 4);
            setPossiblyNullString(comment, stmnt, 5);
            stmnt.execute();
        }
    }

    private void setPossiblyNullString(String methodInd, CallableStatement stmnt, int index) throws SQLException
    {
        if (methodInd == null || methodInd.trim().length() <= 0)
        {
            stmnt.setNull(index, Types.VARCHAR);
        } else
        {
            stmnt.setString(index, methodInd);
        }
    }
}
