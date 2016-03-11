/*
 * User: islepini
 * Date: Oct 2, 2002
 * Time: 9:30:52 AM
 * To change template for new class use
 * Code Style | Class Templates options (Tools | IDE Options).
 */
package aff.confirm.services.talogger;

import aff.confirm.jboss.common.service.queueservice.QueueService;
import aff.confirm.jboss.common.exceptions.LogException;
import aff.confirm.jboss.common.exceptions.SQLConnectionAllocationFailure;
import aff.confirm.jboss.common.exceptions.StopServiceException;

import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import javax.ejb.Singleton;
import javax.ejb.Startup;
import javax.jms.Message;
import javax.jms.JMSException;
import javax.naming.NamingException;
import java.sql.SQLException;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.Types;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;


@Startup
@Singleton
public class TALoggerService extends QueueService implements TALoggerServiceMBean {
    public SimpleDateFormat sdfTADateTime = new SimpleDateFormat("MM/dd/yyyy HH:mm:ss", Locale.US);
    public SimpleDateFormat sdfTADate = new SimpleDateFormat("MM/dd/yyyy", Locale.US);
    //public SimpleDateFormat sdfTADateTimeSym = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss", Locale.US);

    public TALoggerService() {
        super("affinity.cwf:service=TALogger");
    }

    @PostConstruct
    public void postConstruct() throws Exception {
        super.postConstruct();
    }

    @PreDestroy
    public void preDestroy() throws Exception {
        super.preDestroy();
    }

    protected void onMessage(javax.jms.Message message) throws StopServiceException,LogException {
        super.onMessage(message);
        String ticketID = null;
        String tradeAiditID = null;
        double prmntTradeId = 0;
        double tradeAuditId = 0;
        try {
            prmntTradeId = message.getDoubleProperty("PRMNT_TRADE_ID");
            if(message.propertyExists("TRADE_AUDIT_ID"))
               tradeAuditId = message.getDoubleProperty("TRADE_AUDIT_ID");
            ticketID = df.format(prmntTradeId);
            tradeAiditID = df.format(tradeAuditId);
            insertBcLog(message);
            log.info("Logged PRMNT_TRADE_ID: "+ ticketID+" and TRADE_AUDIT_ID: " + tradeAiditID);
        } catch (Exception e) {
            throw new StopServiceException(e.getMessage());
        }
    }

    protected void onServiceStarting() throws Exception {
        log.info("Starting TALogger service...");

        log.info("TALogger startup complete.");
    }

    protected void onServiceStoping() {
    }

    private int getNextSequence(String seqName) throws SQLException, SQLConnectionAllocationFailure{
           int nextSeqNo = 0;
           PreparedStatement statement = null;
           ResultSet rs = null;
           try{
               statement = createPreparedStatement("SELECT ops_tracking." + seqName + ".nextval from dual");
               rs = statement.executeQuery();
               if (rs.next()) {
                   nextSeqNo = (rs.getInt("nextval"));
               }
           }
           finally {
               if (rs != null) {
                   rs.close();
                   rs = null;
               }
               if (statement != null){
                   statement.close();
                   statement = null;
               }
           }
           return nextSeqNo;
       }

    public void insertBcLog(Message pMessage) throws SQLException, JMSException, ParseException, NamingException, SQLConnectionAllocationFailure {
       PreparedStatement preparedStatement = null;
       int nextSeqNo = 0;
       try {
            nextSeqNo = getNextSequence("seq_trade_alert_log");
            String insertSQL =
                "Insert into ops_tracking.TRADE_ALERT_LOG( " +
                "ID, " +                    //  1
                "TRADING_SYSTEM, " +        //  2
                "PRMNT_TRADE_ID, " +        //  3
                "VERSION, " +               //  4
                "TRADE_AUDIT_ID, " +        //  5
                "EMP_ID, " +                //  6
                "TRADE_DT, " +              //  7
                "UPDATE_BUSN_DT, " +        //  8
                "UPDATE_DATETIME, " +       //  9
                "AUDIT_TYPE_CODE, " +       // 10
                "TRADE_TYPE_CODE, " +       // 11
                "TRADE_STAT_CODE, " +       // 12
                "INST_CODE, " +             // 13
                "CDTY_CODE, " +             // 14
                "NOTIFY_CONTRACTS_FLAG, " + // 15
                "CRTD_DT_GMT, " +           //----
                "CMPNY_SHORT_NAME, " +      // 16
                "BK_SHORT_NAME, " +         // 17
                "CPTY_SHORT_NAME, " +       // 18
                "BROKERSN, " +              // 19
                "RFRNCE_SHORT_NAME, " +     // 20
                "UPDATE_TABLE_NAME )" +     // 21
                "values( ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, SYSDATE, ?, ?, ?, ?, ?, ? )";

            preparedStatement = createPreparedStatement(insertSQL);
            preparedStatement.setInt(1, nextSeqNo);
            setStatementString(2, "TRADING_SYSTEM", preparedStatement, pMessage);
            double prmntid = pMessage.getDoubleProperty("PRMNT_TRADE_ID");
            preparedStatement.setDouble(3, prmntid);
            double version = pMessage.getDoubleProperty("VERSION");
            preparedStatement.setDouble(4, version);
            double tradeauditid = pMessage.getDoubleProperty("TRADE_AUDIT_ID");
            preparedStatement.setDouble(5, tradeauditid);
            double emplid = pMessage.getDoubleProperty("EMP_ID");
            preparedStatement.setDouble(6, emplid);
            setTADateTime(7,"TRADE_DT",preparedStatement,pMessage);
            setTADateTime(8,"UPDATE_BUSN_DT",preparedStatement,pMessage);
            setTADateTime(9,"UPDATE_DATETIME",preparedStatement,pMessage);
            setStatementString(10, "AUDIT_TYPE_CODE", preparedStatement, pMessage);
            setStatementString(11, "TRADE_TYPE_CODE", preparedStatement, pMessage);
            setStatementString(12, "TRADE_STAT_CODE", preparedStatement, pMessage);
            setStatementString(13, "INST_CODE", preparedStatement, pMessage);
            setStatementString(14, "CDTY_CODE", preparedStatement, pMessage);
            setStatementString(15, "NOTIFY_CONTRACTS_FLAG", preparedStatement, pMessage);
            setStatementString(16, "CMPNY_SHORT_NAME", preparedStatement, pMessage);
            setStatementString(17, "BK_SHORT_NAME", preparedStatement, pMessage);
            setStatementString(18, "CPTY_SHORT_NAME", preparedStatement, pMessage);
            setStatementString(19, "BROKERSN", preparedStatement, pMessage);
            setStatementString(20, "RFRNCE_SHORT_NAME", preparedStatement, pMessage);
            setStatementString(21, "UPDATE_TABLE_NAME", preparedStatement, pMessage);
            preparedStatement.executeUpdate();
            preparedStatement.getConnection().commit();
        }
        finally {
            try {
                if (preparedStatement != null)
                   preparedStatement.close();
                preparedStatement = null;
            }
              catch (SQLException e) {}
            }
    }

    public void setTADateTime(int pParamIndex, String pFieldName, PreparedStatement statement, Message pMessage)
            throws JMSException, SQLException, ParseException {
        try {
            if (pMessage.propertyExists(pFieldName) &&
                    (pMessage.getStringProperty(pFieldName).length() > 0)) {
                //Try against following format:
                //SimpleDateFormat sdfTADateTime = new SimpleDateFormat("MM/dd/yyyy HH:mm:ss", Locale.US);
                Date trdDt = sdfTADateTime.parse(pMessage.getStringProperty(pFieldName));
                statement.setDate(pParamIndex, new java.sql.Date(trdDt.getTime()));
            } else {
                statement.setNull(pParamIndex, Types.DATE);
            }
        } catch (ParseException pExcept) {
            //Now try against this format:
            if (pExcept.getMessage().contains("Unparseable date")) {
                Date trdDt = sdfTADate.parse(pMessage.getStringProperty(pFieldName));
                statement.setDate(pParamIndex, new java.sql.Date(trdDt.getTime()));
            }
        }
    }

}
