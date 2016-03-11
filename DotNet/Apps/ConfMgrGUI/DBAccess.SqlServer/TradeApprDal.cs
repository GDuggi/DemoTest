using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class TradeApprDal : ITradeApprDal
    {
        private string sqlConnStr = "";

        public TradeApprDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        public Int32 SetFinalApprovalFlag(Int32 pTradeId, string pFinalApprovalFlag, string pOnlyIfReadyFlag, string pUserName)
        {
            string updateSql = DBUtils.SCHEMA_NAME + "pkg_trade_appr$p_set_final_approval_flag";
            Int32 rowsUpdated = 0;

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(updateSql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@p_trade_id", System.Data.SqlDbType.Int).Value = pTradeId;
                    cmd.Parameters.Add("@p_approval_flag", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pFinalApprovalFlag);
                    cmd.Parameters.Add("@p_only_if_ready_flag", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pOnlyIfReadyFlag);
                    cmd.Parameters.Add("@p_appr_by_username", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pUserName);
                    cmd.Parameters.Add("@open_rqmt_ct", System.Data.SqlDbType.Int).Direction = ParameterDirection.Output;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    rowsUpdated = Convert.ToInt32(cmd.Parameters["@open_rqmt_ct"].Value);
                }
            }
            return rowsUpdated;
        }

        public Int32 SetFinalApprovalFlag(List<TradeApprDto> pData)
        {
            Int32 rowsUpdated = 0;
            foreach (TradeApprDto tradeAppr in pData)
            {
                Int32 rowUpd = this.SetFinalApprovalFlag(tradeAppr.TradeId, tradeAppr.ApprovalFlag, tradeAppr.OnlyIfReadyFlag, tradeAppr.ApprByUserName);
                rowsUpdated = rowsUpdated + rowUpd;
            }

            return rowsUpdated;
        }

    }
}
