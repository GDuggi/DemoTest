using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class TradeSummaryDal : ITradeSummaryDal
    {
        private string sqlConnStr = "";

        public TradeSummaryDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        public Int32 UpdateCptyTradeId(Int32 pTradeId, string pCptyTradeId)
        {
            string updateSql = DBUtils.SCHEMA_NAME + "pkg_trade_summary$p_update_trade_summary_cpty_trade_id";
            Int32 rowsUpdated = 0;

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(updateSql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@p_trade_id", System.Data.SqlDbType.Int).Value = pTradeId;
                    cmd.Parameters.Add("@p_cpty_trade_id", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pCptyTradeId);

                    conn.Open();
                    rowsUpdated = cmd.ExecuteNonQuery();
                }
            }
            return rowsUpdated;
        }
        public Int32 UpdateCptyTradeIds(List<TradeSummaryDto> pData)
        {
            Int32 rowsUpdated = 0;
            foreach (TradeSummaryDto tradeSumm in pData)
            {
                Int32 rowUpd = this.UpdateCptyTradeId(tradeSumm.TradeId, tradeSumm.CptyTradeId);
                rowsUpdated = rowsUpdated + rowUpd;
            }

            return rowsUpdated;
        }

        public Int32 UpdateCmt(Int32 pTradeId, string pCmt)
        {
            string updateSql = DBUtils.SCHEMA_NAME + "pkg_trade_summary$p_update_trade_summary_cmt";
            Int32 rowsUpdated = 0;

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(updateSql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@p_trade_id", System.Data.SqlDbType.Int).Value = pTradeId;
                    cmd.Parameters.Add("@p_cmt", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pCmt);

                    conn.Open();
                    rowsUpdated = cmd.ExecuteNonQuery();
                }
            }
            return rowsUpdated;
        }

        public Int32 UpdateCmts(List<TradeSummaryDto> pData)
        {
            Int32 rowsUpdated = 0;
            foreach (TradeSummaryDto tradeSumm in pData)
            {
                Int32 rowUpd = this.UpdateCmt(tradeSumm.TradeId, tradeSumm.Cmt);
                rowsUpdated = rowsUpdated + rowUpd;
            }

            return rowsUpdated;
        }

        public Int32 UpdateDetermineActions(Int32 pTradeId, string pOptsDetActFlag)
        {
            string updateSql = DBUtils.SCHEMA_NAME + "pkg_trade_summary$p_update_determine_actions";
            Int32 rowsUpdated = 0;

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(updateSql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@p_trade_id", System.Data.SqlDbType.Int).Value = pTradeId;
                    cmd.Parameters.Add("@p_ops_det_act_flag", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pOptsDetActFlag);

                    conn.Open();
                    rowsUpdated = cmd.ExecuteNonQuery();
                }
            }
            return rowsUpdated;
        }

        public Int32 UpdateDetermineActions(List<TradeSummaryDto> pData)
        {
            Int32 rowsUpdated = 0;
            foreach (TradeSummaryDto tradeSumm in pData)
            {
                Int32 rowUpd = this.UpdateDetermineActions(tradeSumm.TradeId, tradeSumm.OpsDetActFlag);
                rowsUpdated = rowsUpdated + rowUpd;
            }

            return rowsUpdated;
        }

        public Int32 UpdateFinalApproval(Int32 pTradeId, string pFinalApprovalStatus)
        {
            string updateSql = DBUtils.SCHEMA_NAME + "pkg_trade_summary$p_update_final_approval";
            Int32 rowsUpdated = 0;

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(updateSql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@p_trade_id", System.Data.SqlDbType.Int).Value = pTradeId;
                    cmd.Parameters.Add("@p_fnlapp_status", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pFinalApprovalStatus);

                    conn.Open();
                    rowsUpdated = cmd.ExecuteNonQuery();
                }
            }
            return rowsUpdated;
        }

        public Int32 UpdateFinalApproval(List<TradeSummaryDto> pData)
        {
            Int32 rowsUpdated = 0;
            foreach (TradeSummaryDto tradeSumm in pData)
            {
                Int32 rowUpd = this.UpdateFinalApproval(tradeSumm.TradeId, tradeSumm.FinalApprovalFlag);
                rowsUpdated = rowsUpdated + rowUpd;
            }

            return rowsUpdated;
        }

    }
}
