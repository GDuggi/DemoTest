using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class TradeRqmtDal : ITradeRqmtDal
    {
        private string sqlConnStr = "";

        public TradeRqmtDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        //Currently used only for testing.
        public List<TradeRqmtDto> GetTradeRqmts(Int32 pTradeId)
        {
            var result = new List<TradeRqmtDto>();
            string sql = "select id, trade_id, rqmt_trade_notify_id, rqmt, status, completed_dt, completed_timestamp_gmt, " +
                         " reference, cancel_trade_notify_id, cmt, second_check_flag " +
                         " from " + DBUtils.SCHEMA_NAME + "trade_rqmt where trade_id = @trade_id ";

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
                                result.Add(new TradeRqmtDto
                                {
                                    Id = DBUtils.HandleInt32IfNull(dataReader["id"].ToString()),
                                    TradeId = DBUtils.HandleInt32IfNull(dataReader["trade_id"].ToString()),
                                    RqmtTradeNotifyId = DBUtils.HandleInt32IfNull(dataReader["rqmt_trade_notify_id"].ToString()),
                                    RqmtCode = dataReader["rqmt"].ToString(),
                                    StatusCode = dataReader["status"].ToString(),
                                    CompletedDt = DBUtils.HandleDateTimeIfNull(dataReader["completed_dt"].ToString()),
                                    CompletedTimestampGmt = DBUtils.HandleDateTimeIfNull(dataReader["completed_timestamp_gmt"].ToString()),
                                    Reference = dataReader["reference"].ToString(),
                                    CancelTradeNotifyId = DBUtils.HandleInt32IfNull(dataReader["cancel_trade_notify_id"].ToString()),
                                    Cmt = dataReader["cmt"].ToString(),
                                    SecondCheckFlag = dataReader["second_check_flag"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            return result;
        } 


        public Int32 AddTradeRqmt(Int32 pTradeId, string pRqmtCode, string pReference, string pCmt)
        {
            string updateSql = DBUtils.SCHEMA_NAME + "PKG_TRADE_RQMT$P_ADD_TRADE_RQMT_WITH_ID";
            Int32 rowsInserted = 0;
            Int32 seqNo = 0;
            string seqName = "SEQ_TRADE_RQMT";
            seqNo = DBUtils.GetNextSequence(sqlConnStr, seqName);

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(updateSql, conn))
                {
                    //There are  3 overloaded versions of function, containing 3, 4 and 6 parameters.
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@p_id", System.Data.SqlDbType.Int).Value = seqNo;                  
                    cmd.Parameters.Add("@p_trade_id", System.Data.SqlDbType.Int).Value = pTradeId;
                    cmd.Parameters.Add("@p_rqmt_code", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pRqmtCode);
                    cmd.Parameters.Add("@p_reference", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pReference);
                    cmd.Parameters.Add("@p_cmt", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pCmt);

                    conn.Open();
                    rowsInserted = cmd.ExecuteNonQuery();
                }
            }
            if (rowsInserted == 0)
                return rowsInserted;
            else
                return seqNo;
        }


        public Int32 UpdateTradeRqmt(Int32 pId, DateTime pCompletedDt, string pSecondChk, string pStatusCode, string pReference, string pCmt)
        {
            string updateSql = DBUtils.SCHEMA_NAME + "PKG_TRADE_RQMT$p_update_trade_rqmt";
            Int32 rowsUpdated = 0;

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(updateSql, conn))
                {
                    //There are  3 overloaded versions of function, containing 3, 4 and 6 parameters.
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@p_rqmt_id", System.Data.SqlDbType.Int).Value = pId;
                    cmd.Parameters.Add("@p_completion_date", System.Data.SqlDbType.Date).Value = DBUtils.ValueDateTimeOrDbNull(pCompletedDt);
                    cmd.Parameters.Add("@p_second_chk", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pSecondChk);
                    cmd.Parameters.Add("@p_status", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pStatusCode);
                    cmd.Parameters.Add("@p_reference", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pReference);
                    cmd.Parameters.Add("@p_cmt", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pCmt);

                    conn.Open();
                    rowsUpdated = cmd.ExecuteNonQuery();
                }
            }
            return rowsUpdated;
        }

        public Int32 UpdateTradeRqmts(List<TradeRqmtDto> pData)
        {
            Int32 rowsUpdated = 0;
            foreach (TradeRqmtDto tradeRqmt in pData)
            {
                Int32 rowUpd = UpdateTradeRqmt(tradeRqmt.Id, tradeRqmt.CompletedDt, tradeRqmt.SecondCheckFlag, tradeRqmt.StatusCode, 
                    tradeRqmt.Reference, tradeRqmt.Cmt);
                rowsUpdated = rowsUpdated + rowUpd;
            }

            return rowsUpdated;
        }
    }
}
