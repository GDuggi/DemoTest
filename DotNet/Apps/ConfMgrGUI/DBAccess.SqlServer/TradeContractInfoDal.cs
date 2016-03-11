using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class TradeContractInfoDal : ITradeContractInfoDal
    {
        private string sqlConnStr = "";

        public TradeContractInfoDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        #region Stubbed Data

        public List<TradeContractInfoDto> GetContractListStub()
        {
            //string dateStr = "";
            var result = new List<TradeContractInfoDto>();
            result.Add(new TradeContractInfoDto
            {
                TrdSysCode = "AFF",
                TrdSysTicket = "1493829",
                TradeRqmtConfirmId = 1725,
                TradeDt = DateTime.ParseExact("04-09-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                TemplateName = "NAESB"
            });

            return result;
        }

        #endregion

        public List<TradeContractInfoDto> GetContractList(string pTrdSysCode, string pTrdSysTicket)
        {
            var result = new List<TradeContractInfoDto>();
            string sql = "select * from " + DBUtils.SCHEMA_NAME + "v_trade_contract_list " +
                        " where trd_sys_code = @trd_sys_code " +
                        " and trd_sys_ticket = @trd_sys_ticket " +
                        " and active_flag = 'Y' ";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@trd_sys_code", System.Data.SqlDbType.VarChar).Value = pTrdSysCode;
                    cmd.Parameters.Add("@trd_sys_ticket", System.Data.SqlDbType.VarChar).Value = pTrdSysTicket;

                    conn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new TradeContractInfoDto
                                {
                                    TrdSysCode = dataReader["trd_sys_code"].ToString(),
                                    TrdSysTicket = dataReader["trd_sys_ticket"].ToString(),
                                    TradeRqmtConfirmId = DBUtils.HandleInt32IfNull(dataReader["trade_rqmt_confirm_id"].ToString()),
                                    TradeDt = DBUtils.HandleDateTimeIfNull(dataReader["trade_dt"].ToString()),
                                    TemplateName = dataReader["template_name"].ToString()
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
