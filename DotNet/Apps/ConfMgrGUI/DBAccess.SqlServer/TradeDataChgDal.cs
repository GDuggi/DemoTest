using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class TradeDataChgDal : ITradeDataChgDal
    {
        private string sqlConnStr = "";

        public TradeDataChgDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }


        public List<TradeDataChgDto> GetTradeDataChg(Int32 pTradeId)
        {
            var result = new List<TradeDataChgDto>();

            string sql = "select jn_datetime, trade_id, buy_sell_ind, trade_dt, booking_co_sn, cpty_sn, " +
                         "broker_sn, trade_desc, price_desc, trade_stat_code, location_sn, qty_desc, " + 
                         "qty_tot, trade_type_code, cdty_code, sttl_type, book, transport_desc, cpty_legal_name, " +
                         "broker_legal_name, broker_price, trader, cdty_grp_code, start_dt, end_dt, xref, " +
                         "ref_sn, inception_dt, optn_put_call_ind, optn_prem_price, optn_strike_price, " + 
                         "profit_center, permission_key " +
                         " from " + DBUtils.SCHEMA_NAME + "trade_data_jn " + 
                         " where trade_id = @trade_id " +
                         " order by jn_datetime ";

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
                                result.Add(new TradeDataChgDto
                                {
                                    Book = dataReader["book"].ToString(),
                                    BookingCoSn = dataReader["booking_co_sn"].ToString(),
                                    BrokerLegalName = dataReader["broker_legal_name"].ToString(),
                                    BrokerPrice = dataReader["broker_price"].ToString(),
                                    BrokerSn = dataReader["broker_sn"].ToString(),
                                    BuySellInd = dataReader["buy_sell_ind"].ToString(),
                                    CdtyCode = dataReader["cdty_code"].ToString(),
                                    CdtyGrpCode = dataReader["cdty_grp_code"].ToString(),
                                    CptyLegalName = dataReader["cpty_legal_name"].ToString(),
                                    CptySn = dataReader["cpty_sn"].ToString(),
                                    EndDt = DBUtils.HandleDateTimeIfNull(dataReader["end_dt"].ToString()),
                                    InceptionDt = DBUtils.HandleDateTimeIfNull(dataReader["inception_dt"].ToString()),
                                    JnDatetime = DBUtils.HandleDateTimeIfNull(dataReader["jn_datetime"].ToString()),
                                    LocationSn = dataReader["location_sn"].ToString(),
                                    OptnPremPrice = dataReader["optn_prem_price"].ToString(),
                                    OptnPutCallInd = dataReader["optn_put_call_ind"].ToString(),
                                    OptnStrikePrice = dataReader["optn_strike_price"].ToString(),
                                    PermissionKey = dataReader["permission_key"].ToString(),
                                    PriceDesc = dataReader["price_desc"].ToString(),
                                    ProfitCenter = dataReader["profit_center"].ToString(),
                                    QtyDesc = dataReader["qty_desc"].ToString(),
                                    QtyTot = DBUtils.HandleFloatIfNull(dataReader["qty_tot"].ToString()),
                                    RefSn = dataReader["ref_sn"].ToString(),
                                    StartDt = DBUtils.HandleDateTimeIfNull(dataReader["start_dt"].ToString()),
                                    SttlType = dataReader["sttl_type"].ToString(),
                                    TradeDesc = dataReader["trade_desc"].ToString(),
                                    TradeDt = DBUtils.HandleDateTimeIfNull(dataReader["trade_dt"].ToString()),
                                    TradeId = DBUtils.HandleInt32IfNull(dataReader["trade_id"].ToString()),
                                    TradeStatCode = dataReader["trade_stat_code"].ToString(),
                                    TradeTypeCode = dataReader["trade_type_code"].ToString(),
                                    Trader = dataReader["trader"].ToString(),
                                    TransportDesc = dataReader["transport_desc"].ToString(),
                                    Xref = dataReader["xref"].ToString()
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
