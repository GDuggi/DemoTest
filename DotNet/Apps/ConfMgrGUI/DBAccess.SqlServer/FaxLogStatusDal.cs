using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class FaxLogStatusDal : IFaxLogStatusDal
    {
        public const string SEQ_NAME = "seq_fax_log_status";
        private string sqlConnStr = "";

        public FaxLogStatusDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        #region Stub Data

        public List<FaxLogStatusDto> GetStub()
        {
            var result = new List<FaxLogStatusDto>();
            result.Add(new FaxLogStatusDto
            { 
                Id = 7041, 
                TradeId = 1493829,
                TradeRqmtConfirmId = 1725,
                Sender = "WASHINSH",
                CrtdTsGmt = DateTime.ParseExact("04-13-2015 15:00:21", "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                FaxTelexInd = "F",
                FaxTelexNumber = "CONFIRMATIONS@MITSUI-EP.COM",
                Cmt = "",
                FaxStatus = "Q"
            });

            result.Add(new FaxLogStatusDto
            {
                Id = 7042,
                TradeId = 1493829,
                TradeRqmtConfirmId = 1725,
                Sender = "WASHINSH",
                CrtdTsGmt = DateTime.ParseExact("04-13-2015 15:03:24", "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                FaxTelexInd = "F",
                FaxTelexNumber = "CONFIRMATIONS@MITSUI-EP.COM",
                FaxStatus = "S",
                Cmt = ""
            });

            return result;
        }

        #endregion

        public List<FaxLogStatusDto> Get(Int32 pTradeRqmtConfirmId)
        {
            var result = new List<FaxLogStatusDto>();
            string sql = "select id, trade_id, trade_rqmt_confirm_id, sender, crtd_ts_gmt, " +
                         "     fax_telex_ind, fax_telex_number, fax_status, cmt " +
                         "from  " + DBUtils.SCHEMA_NAME + "fax_log_status  " +
                         "where trade_rqmt_confirm_id = @trade_rqmt_confirm_id " + 
                         "order by id desc ";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@trade_rqmt_confirm_id", System.Data.SqlDbType.Int).Value = pTradeRqmtConfirmId;
                    conn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new FaxLogStatusDto
                                {
                                    Id = DBUtils.HandleInt32IfNull(dataReader["id"].ToString()),
                                    TradeId = DBUtils.HandleInt32IfNull(dataReader["trade_id"].ToString()),
                                    TradeRqmtConfirmId = DBUtils.HandleInt32IfNull(dataReader["trade_rqmt_confirm_id"].ToString()),
                                    Sender = dataReader["sender"].ToString(),
                                    CrtdTsGmt = DBUtils.HandleDateTimeIfNull(dataReader["crtd_ts_gmt"].ToString()),
                                    FaxTelexInd = dataReader["fax_telex_ind"].ToString(),
                                    FaxTelexNumber = dataReader["fax_telex_number"].ToString(),
                                    FaxStatus = dataReader["fax_status"].ToString(),
                                    Cmt = dataReader["cmt"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }

    }
}
