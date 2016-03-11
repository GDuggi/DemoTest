using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class XmitResultDal : IXmitResultDal
    {
        private string sqlConnStr = "";

        public XmitResultDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        public List<XmitResultDto> GetByTradeRqmtConfirmId(Int32 pTradeRqmtConfirmId)
        {
            var result = new List<XmitResultDto>();
            string sql = "select xmit_result_id, xmit_request_id, associated_docs_id, trade_rqmt_confirm_id, " +
                         "     sent_by_user, xmit_status_ind, xmit_method_ind, xmit_dest, xmit_cmt, xmit_timestamp " +
                         "from  " + DBUtils.SCHEMA_NAME + "v_xmit_result " +
                         "where trade_rqmt_confirm_id = @trade_rqmt_confirm_id " +
                         "order by xmit_result_id desc ";

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
                                result.Add(new XmitResultDto
                                {
                                    XmitResultId = DBUtils.HandleInt32IfNull(dataReader["xmit_result_id"].ToString()),
                                    XmitRequestId = DBUtils.HandleInt32IfNull(dataReader["xmit_request_id"].ToString()),
                                    AssociatedDocsId = DBUtils.HandleInt32IfNull(dataReader["associated_docs_id"].ToString()),
                                    TradeRqmtConfirmId = DBUtils.HandleInt32IfNull(dataReader["trade_rqmt_confirm_id"].ToString()),
                                    SentByUser = dataReader["sent_by_user"].ToString(),
                                    XmitStatusInd = dataReader["xmit_status_ind"].ToString(),
                                    XmitMethodInd = dataReader["xmit_method_ind"].ToString(),
                                    XmitDest = dataReader["xmit_dest"].ToString(),
                                    XmitCmt = dataReader["xmit_cmt"].ToString(),
                                    XmitTimestamp = DBUtils.HandleDateTimeIfNull(dataReader["xmit_timestamp"].ToString())
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }

        public List<XmitResultDto> GetByAssociatedDocsId(Int32 pAssociatedDocsId)
        {
            var result = new List<XmitResultDto>();
            string sql = "select xmit_result_id, xmit_request_id, associated_docs_id, trade_rqmt_confirm_id, " +
                         "     sent_by_user, xmit_status_ind, xmit_method_ind, xmit_dest, xmit_cmt, xmit_timestamp " +
                         "from  " + DBUtils.SCHEMA_NAME + "v_xmit_result " +
                         "where associated_docs_id = @associated_docs_id " +
                         "order by xmit_result_id desc ";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@associated_docs_id", System.Data.SqlDbType.Int).Value = pAssociatedDocsId;
                    conn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new XmitResultDto
                                {
                                    XmitResultId = DBUtils.HandleInt32IfNull(dataReader["xmit_result_id"].ToString()),
                                    XmitRequestId = DBUtils.HandleInt32IfNull(dataReader["xmit_request_id"].ToString()),
                                    AssociatedDocsId = DBUtils.HandleInt32IfNull(dataReader["associated_docs_id"].ToString()),
                                    TradeRqmtConfirmId = DBUtils.HandleInt32IfNull(dataReader["trade_rqmt_confirm_id"].ToString()),
                                    SentByUser = dataReader["sent_by_user"].ToString(),
                                    XmitStatusInd = dataReader["xmit_status_ind"].ToString(),
                                    XmitMethodInd = dataReader["xmit_method_ind"].ToString(),
                                    XmitDest = dataReader["xmit_dest"].ToString(),
                                    XmitCmt = dataReader["xmit_cmt"].ToString(),
                                    XmitTimestamp = DBUtils.HandleDateTimeIfNull(dataReader["xmit_timestamp"].ToString())
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
