package aff.confirm.opsmanager.confirm.common;

import aff.confirm.opsmanager.confirm.data.TradeRqmtRec;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import org.jboss.logging.Logger;

public class TradeRqmtDAO {
    private static Logger log = Logger.getLogger(TradeRqmtDAO.class );
	private Connection affinityConnection;

    public TradeRqmtDAO(Connection connection){
        affinityConnection = connection;
    }

     public TradeRqmtRec getTradeConfirmId(long tradeId, long tradeRqmtConfirmId) {
/*
         String sql  = "select tr.id rqmt_id,tr.rqmt,tr.status,trc.id rqmt_confirm_id,trc.template_id,trc.next_status_code," +
                 "to_char(td.trade_dt,'mm/dd/yyyy') trade_dt,td.cdty_code,td.cdty_grp_code,td.cpty_sn,td.se_cpty_sn,td.sttl_type " +
                 "from ops_tracking.trade_rqmt tr,ops_tracking.trade_rqmt_confirm trc, ops_tracking.trade_data td " +
                 "where td.trade_id = ? and  td.trade_id = tr.trade_id " +
                 " and tr.id = trc.rqmt_id  and tr.rqmt = 'XQCSP' and tr.status IN ( 'CRDT','PREP') and trc.confirm_label = 'CONTRACT' " +
                 "and trc.id = ?  order by trc.id";
*/
         //Israel 1/30/15 -- added trf.active_flag and formatted code
         String sql = "  SELECT tr.id rqmt_id, " +
                 "         tr.rqmt, " +
                 "         tr.status, " +
                 "         trc.id rqmt_confirm_id, " +
                 "         trc.template_id, " +
                 "         trc.next_status_code, " +
                 "         TO_CHAR (td.trade_dt, 'mm/dd/yyyy') trade_dt, " +
                 "         td.cdty_code, " +
                 "         td.cdty_grp_code, " +
                 "         td.cpty_sn, " +
                 "         td.se_cpty_sn, " +
                 "         td.sttl_type " +
                 "    FROM ops_tracking.trade_rqmt tr, " +
                 "         ops_tracking.trade_rqmt_confirm trc, " +
                 "         ops_tracking.trade_data td " +
                 "   WHERE  td.trade_id = ? " +
                 "         AND  td.trade_id = tr.trade_id " +
                 "         AND tr.id = trc.rqmt_id " +
                 "         AND tr.rqmt = 'XQCSP' " +
                 "         AND tr.status IN ('CRDT', 'PREP') " +
                 "         AND trc.confirm_label = 'CONTRACT' " +
                 "         AND trc.id = ? " +
                 "         and trc.active_flag = 'Y' " +
                 "ORDER BY trc.id ";
         
         TradeRqmtRec trc = new TradeRqmtRec();
         PreparedStatement ps = null;
         ResultSet rs = null;
         try {
             ps = this.affinityConnection.prepareStatement(sql);
             ps.setLong(1,tradeId);
             ps.setLong(2, tradeRqmtConfirmId);
             rs = ps.executeQuery();
             if (rs.next()){

                 trc.setTradeId(tradeId);
                 trc.setTradeRqmtConfirmId(rs.getLong("rqmt_confirm_id"));
                 trc.setTradeRqmtId(rs.getLong("rqmt_id"));
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
            log.error("Error update Trade Rqmt table" , e );
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
            log.error( "getTraderEmailTest error", e );
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
    
}
