package aff.confirm.opsmanager.creditmargin.common;


import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import org.jboss.logging.Logger;


/**
 * User: srajaman
 * Date: Dec 5, 2008
 * Time: 3:05:48 PM
 */
public class TradeRqmtDAO {
    private static Logger log = Logger.getLogger(TradeRqmtDAO.class.getName());
    private Connection affinityConnection;

    public TradeRqmtDAO(Connection connection){
        affinityConnection = connection;
    }

     public TradeRqmtRec getTradeConfirmId(String tradingSystem,long tradeId,int version) {
        String sql  = "select tr.id rqmt_id,tr.rqmt,tr.status,trc.id rqmt_confirm_id,trc.template_id,trc.next_status_code," +
                      "to_char(td.trade_dt,'mm/dd/yyyy') trade_dt,td.cdty_code,td.cdty_grp_code,td.cpty_sn,td.se_cpty_sn,td.sttl_type " +
                      "from ops_tracking.trade_rqmt tr,ops_tracking.trade_rqmt_confirm trc, ops_tracking.trade_data td " +
                      "where td.trade_id = ? and  td.trade_id = tr.trade_id " +
					  " and tr.id = trc.rqmt_id  and tr.rqmt = 'XQCSP' and tr.status IN ( 'CRDT','PREP') and trc.confirm_label = 'CONTRACT' " +
                      "order by trc.id";
         
         TradeRqmtRec trc = new TradeRqmtRec();
         PreparedStatement ps = null;
         ResultSet rs = null;
         try {
             ps = this.affinityConnection.prepareStatement(sql);
             ps.setLong(1,tradeId);
             rs = ps.executeQuery();
             if (rs.next()){

                 trc.setTradeId(tradeId);
                 trc.setTradeRqmtConfirmId(rs.getLong("rqmt_confirm_id"));
                 trc.setTradeRqmtId(rs.getLong("rqmt_id"));
                 trc.setTradesystem(tradingSystem);
                 trc.setNextStatusCode(rs.getString("next_status_code"));
                 trc.setTradeDate(rs.getString("trade_dt"));
                 trc.setCdtyCode(rs.getString("cdty_code"));
                 trc.setCdtyGroupCode(rs.getString("cdty_grp_code"));
                 trc.setSeCptySn(rs.getString("se_cpty_sn"));
                 trc.setCptySn(rs.getString("cpty_sn"));
                 trc.setContractId(rs.getLong("template_id"));
                 trc.setSttlType(rs.getString("sttl_type"));
                 trc.setTemplateId(rs.getLong("template_id"));
                 trc.setCurrentStatus(rs.getString("status"));
             }

         } catch (SQLException e) {
             log.error( "ERROR", e);
         }
         finally {
             try {

                 if (rs != null){
                     rs.close();
                 }
                 if (ps != null){
                     ps.close();
                 }
             }
             catch (SQLException e) {

             }
         }

         return trc;
    }

    public void updateTradeRqmtStatus(TradeRqmtRec rec) throws SQLException {

        String sql = "update ops_tracking.trade_rqmt " +
                    " set status = ? " +
                    " where id = ?";


        PreparedStatement ps  = null;
        try {

            log.info(sql + " param = " + rec.getNextStatusCode() + " value = " + rec.getTradeRqmtId());
            ps = this.affinityConnection.prepareStatement(sql);
            ps.setString(1,rec.getNextStatusCode());
            ps.setLong(2,rec.getTradeRqmtId());
            ps.executeUpdate();


        } catch (SQLException e) {
            log.error( "Error update Trade Rqmt table", e);
            throw e;
        }
        finally {
            try {
                if (ps != null) {
                    ps.close();
                }
            }
            catch (Exception e) {}
        }
    }
    public  String getTraderEmailAddr( TradeRqmtRec rec) throws SQLException {

        PreparedStatement stmt = null;
		ResultSet rs = null;
		String sql = "select e.email_addr from infinity_mgr.trade t, infinity_mgr.emp e " +
					" where t.TRADER_ID = e.ID 	and e.ACTIVE_FLAG = 'Y' and t.EXP_DT = '31-dec-2299' " +
					" and t.prmnt_id = ?";

        String emailAddress = null;

		try {
			stmt =  affinityConnection.prepareStatement(sql);
			stmt.setLong(1,rec.getTradeId() );
			rs = stmt.executeQuery();
			if (rs.next()){
				emailAddress = rs.getString("email_addr");
			}

		}
		catch (SQLException e){
			log.error( "getTraderEmailTest error", e);
            throw e;
        }
		finally {
			try {
				if (rs != null){
					rs.close();
				}
				if (stmt != null) {
					stmt.close();
				}
			}
			catch (SQLException e) {}
		}

		return emailAddress;
    }
    public void insertIntoLog(CreditLogRec clc) throws SQLException {
        String sql = "insert into ops_tracking.credit_margin_log(id,trade_id,msg,processed_flag,cmt) values " +
                    "(ops_tracking.seq_credit_margin_log.nextval,?,?,?,?)";

        PreparedStatement stmt = null;
		try {
            stmt =  affinityConnection.prepareStatement(sql);
            stmt.setLong(1,clc.getTradeId());
            stmt.setString(2,clc.getMsg());
            stmt.setString(3,clc.getProcessFlag());
            stmt.setString(4,clc.getCmt());
            stmt.executeUpdate();

        }
        catch (SQLException e) {
            log.error( "insert Into Log error", e );
            throw e;
        }
        finally {
            try {
                    if (stmt != null){
                        stmt.close();
                    }
                }
            catch (SQLException e){
                
            }
        }

    }
}
