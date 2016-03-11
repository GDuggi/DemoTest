using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class TradeAuditDal : ITradeAuditDal
    {
        private string sqlConnStr = "";

        public TradeAuditDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }


        public List<TradeAuditDto> GetTradeAudit(Int32 pTradeId)
        {
            var result = new List<TradeAuditDto>();
            string sql = "select trade_id, trade_rqmt_id, operation, rqmt, status, machine_name, " +
					     " userid, timestamp, completed_dt " + 
                         " from " + DBUtils.SCHEMA_NAME + "v_trade_audit " + 
                         " where trade_id = @trade_id " + 
                         " order by timestamp ";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@trade_id", System.Data.SqlDbType.Int).Value = pTradeId;
                    conn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new TradeAuditDto
                                {
                                    TradeId = DBUtils.HandleInt32IfNull(dataReader["trade_id"].ToString()),
                                    TradeRqmtId = DBUtils.HandleInt32IfNull(dataReader["trade_rqmt_id"].ToString()),
                                    Operation = dataReader["operation"].ToString(),
                                    Rqmt = dataReader["rqmt"].ToString(),
                                    Status = dataReader["status"].ToString(),
                                    Machine = dataReader["machine_name"].ToString(),
                                    UserId = dataReader["userid"].ToString(),
                                    Timestamp = DBUtils.HandleDateTimeIfNull(dataReader["timestamp"].ToString()),
                                    CompletedDt = DBUtils.HandleDateTimeIfNull(dataReader["completed_dt"].ToString()),
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }

        public bool HasConfirmBeenSent(Int32 pTradeId)
        {
            bool isBeenSent = false;

            List<TradeAuditDto> tradeAuditList = new List<TradeAuditDto>();
            tradeAuditList = GetTradeAudit(pTradeId);

            if (tradeAuditList.Count > 0)
            {
                foreach (TradeAuditDto rec in tradeAuditList)
                {
                    if (rec.Rqmt == "Our Paper" && rec.Status == "Sent")
                    {
                        isBeenSent = true;
                        break;
                    }
                }
            }

            return isBeenSent;
        }

    }
}
