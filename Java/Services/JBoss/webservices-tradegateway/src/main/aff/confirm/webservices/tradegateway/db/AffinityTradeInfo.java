package aff.confirm.webservices.tradegateway.db;

import aff.confirm.webservices.tradegateway.common.TradeInfo;
import aff.confirm.webservices.tradegateway.data.ContractData;
import aff.confirm.webservices.tradegateway.data.GatewayConfig;
import aff.confirm.webservices.tradegateway.data.TradeAlertData;
import aff.confirm.webservices.tradegateway.data.TradeData;
import aff.confirm.webservices.tradegateway.util.DataConverter;
import oracle.jdbc.OracleTypes;
import oracle.jdbc.oracore.OracleType;
import org.hibernate.Session;
import org.hibernate.Transaction;
import org.hibernate.jdbc.ReturningWork;
import org.hibernate.jdbc.Work;

import javax.persistence.EntityManager;
import javax.persistence.EntityManagerFactory;
import javax.persistence.Persistence;
import javax.persistence.Query;
import java.sql.*;
import java.util.List;

/**
 * Created with IntelliJ IDEA.
 * User: sraj
 * Date: 1/17/13
 * Time: 10:06 AM
 */

public class AffinityTradeInfo implements TradeInfo {

    @Override
    public String getOpsTrackingTrade(String tradeSystemCode, String ticket)  throws  Exception{
//        TradeData tradeData = null;
//        EntityManager entityManager  = getEntityManager();
//        Query query = entityManager.createNamedQuery("sp_tradedata").setParameter("id",ticket);
//        List<TradeData> list= query.getResultList();
//        if (list.size() > 0) {
//            tradeData = list.get(0);
//        }
//        entityManager.close();
//        return tradeData;
///////////////////////////////////////////////////////////////
        String returnXML = null;
        final String sql = "{call ops_tracking.pkg_aff_trade.p_get_trade(?,?)}";
        returnXML = getXMLFromResultSet(ticket, sql, "TradeData");
        return returnXML;
    }

    @Override
    public String getContractData(String tradeSystemCode, String ticket) throws  Exception{
        String returnXML = null;
        final String sql = "{call infinity_mgr.pkg_contract.p_get_contract_feed(?,?)}";
        returnXML = getXMLFromResultSet(ticket, sql, "ContractData");
        return returnXML;

        /*
        //entityManager.unwrap()
        Query query = entityManager.createNamedQuery("sp_contract").setParameter("id",ticket);
        List<ContractData> list = query.getResultList();

        if (list.size()>0 ) {
            contractData = list.get(0);
        }
        entityManager.close();


        return contractData;
        */
    }

    @Override
    public String getTradeAlertMsg(String tradeSystemCode, String ticket) throws Exception {
        return null;
    }

    private String getXMLFromResultSet(String ticket, final String sql, String rootElement) throws Exception {
        String returnXML;ResultSet rs = null;
        EntityManager entityManager = getEntityManager();
        Session session = entityManager.unwrap(Session.class);
        Transaction transaction = null;

        try {
            transaction = session.beginTransaction();
            final int tradeId = Integer.parseInt(ticket);
            rs = session.doReturningWork(new ReturningWork<ResultSet>() {

                @Override
                public ResultSet execute(Connection connection) throws SQLException {

                    ResultSet rs1 = null;
                    CallableStatement stmt = null;
                    stmt = connection.prepareCall(sql);
                    stmt.registerOutParameter(1, OracleTypes.CURSOR);
                    stmt.setInt(2,tradeId);
                    stmt.execute();
                    rs1 = (ResultSet) stmt.getObject(1);
                    return  rs1;
                }
            });
            if (rs != null && rs.next()) {
                returnXML = DataConverter.convertResultSetToXML(rs, rootElement);
            }
            else {
                throw new Exception("Ticket " + ticket + " is not found.");
            }
        }
        finally {
            try {
                if (transaction != null) {
                    transaction.commit();
                }
                if (rs != null ){
                    rs.close();
                }
            }
            catch (Exception e) {}
        }
        return returnXML;
    }

    @Override
    public void setConfig(GatewayConfig config) {
    }

    private EntityManager getEntityManager() {
        EntityManagerFactory factory = Persistence.createEntityManagerFactory("affinitydb");
        return factory.createEntityManager();
    }

}
