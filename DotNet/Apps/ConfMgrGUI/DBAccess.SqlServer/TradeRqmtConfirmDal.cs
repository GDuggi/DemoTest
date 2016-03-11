using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class TradeRqmtConfirmDal : ITradeRqmtConfirmDal
    {
        private string sqlConnStr = "";

        public TradeRqmtConfirmDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        #region Stubbed Data

        public List<TradeRqmtConfirm> GetAllStub()
        {
            //string dateStr = "";
            var result = new List<TradeRqmtConfirm>();
            result.Add(new TradeRqmtConfirm
            {
                Id = 1725,
                RqmtId = 279970,
                TradeId = 1493829,
                TemplateId = 7954,
                NextStatusCode = "MGR",
                ConfirmLabel = "CONFIRM",
                FaxTelexInd = "F",
                FaxTelexNumber = "CONFIRMATIONS@MITSUI-EP.COM",
                XmitStatusInd = "Q;S",
                XmitAddr = "Q:CONFIRMATIONS@MITSUI-EP.COM;S:CONFIRMATIONS@MITSUI-EP.COM",
                XmitTimestampGmt = DateTime.ParseExact("04-13-2015 15:03:24", "MM-dd-yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                TemplateName = "NAESB",
                TemplateTypeInd = "S",
                FinalApprovalFlag = "N",
                ActiveFlag = "Y"
            });

            return result;
        }

        #endregion

        public List<TradeRqmtConfirm> GetAll()
        {
            var result = new List<TradeRqmtConfirm>();
            string sql = "select * from " + DBUtils.SCHEMA_NAME + "v_trade_rqmt_confirm " +
                        " where final_approval_flag = 'N' ";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new TradeRqmtConfirm
                                {
                                    ActiveFlag = dataReader["ACTIVE_FLAG"].ToString(),
                                    ConfirmCmt = dataReader["CONFIRM_CMT"].ToString(),
                                    ConfirmLabel = dataReader["CONFIRM_LABEL"].ToString(),
                                    FaxTelexInd = dataReader["FAX_TELEX_IND"].ToString(),
                                    FaxTelexNumber = dataReader["FAX_TELEX_NUMBER"].ToString(),
                                    FinalApprovalFlag = dataReader["FINAL_APPROVAL_FLAG"].ToString(),
                                    Id = DBUtils.HandleInt32IfNull(dataReader["ID"].ToString()),
                                    NextStatusCode = dataReader["NEXT_STATUS_CODE"].ToString(),
                                    RqmtId = DBUtils.HandleInt32IfNull(dataReader["RQMT_ID"].ToString()),
                                    //TemplateCategory = dataReader["TEMPLATE_CATEGORY"].ToString(),
                                    //TemplateId = DBUtils.HandleInt64IfNull(dataReader["TEMPLATE_ID"].ToString()),
                                    TemplateName = dataReader["TEMPLATE_NAME"].ToString(),
                                    PreparerCanSendFlag = dataReader["PREPARER_CAN_SEND_FLAG"].ToString(),
                                    //TemplateTypeInd = dataReader["TEMPLATE_TYPE_IND"].ToString(),
                                    TradeId = DBUtils.HandleInt32IfNull(dataReader["TRADE_ID"].ToString()),
                                    XmitAddr = dataReader["XMIT_ADDR"].ToString(),
                                    XmitCmt = dataReader["XMIT_CMT"].ToString(),
                                    XmitStatusInd = dataReader["XMIT_STATUS_IND"].ToString(),
                                    XmitTimestampGmt = DBUtils.HandleDateTimeIfNull(dataReader["XMIT_TIMESTAMP_GMT"].ToString())
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }

        public List<TradeRqmtConfirm> GetAll(string pTradeIdList)
        {
            var result = new List<TradeRqmtConfirm>();
            string sql = "select * from " + DBUtils.SCHEMA_NAME + "v_trade_rqmt_confirm " +
                "where trade_id in " + pTradeIdList;

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new TradeRqmtConfirm
                                {
                                    ActiveFlag = dataReader["ACTIVE_FLAG"].ToString(),
                                    ConfirmCmt = dataReader["CONFIRM_CMT"].ToString(),
                                    ConfirmLabel = dataReader["CONFIRM_LABEL"].ToString(),
                                    FaxTelexInd = dataReader["FAX_TELEX_IND"].ToString(),
                                    FaxTelexNumber = dataReader["FAX_TELEX_NUMBER"].ToString(),
                                    FinalApprovalFlag = dataReader["FINAL_APPROVAL_FLAG"].ToString(),
                                    Id = DBUtils.HandleInt32IfNull(dataReader["ID"].ToString()),
                                    NextStatusCode = dataReader["NEXT_STATUS_CODE"].ToString(),
                                    RqmtId = DBUtils.HandleInt32IfNull(dataReader["RQMT_ID"].ToString()),
                                    //TemplateCategory = dataReader["TEMPLATE_CATEGORY"].ToString(),
                                    //TemplateId = DBUtils.HandleInt32IfNull(dataReader["TEMPLATE_ID"].ToString()),
                                    TemplateName = dataReader["TEMPLATE_NAME"].ToString(),
                                    PreparerCanSendFlag = dataReader["PREPARER_CAN_SEND_FLAG"].ToString(),
                                    //TemplateTypeInd = dataReader["TEMPLATE_TYPE_IND"].ToString(),
                                    TradeId = DBUtils.HandleInt32IfNull(dataReader["TRADE_ID"].ToString()),
                                    XmitAddr = dataReader["XMIT_ADDR"].ToString(),
                                    XmitCmt = dataReader["XMIT_CMT"].ToString(),
                                    XmitStatusInd = dataReader["XMIT_STATUS_IND"].ToString(),
                                    XmitTimestampGmt = DBUtils.HandleDateTimeIfNull(dataReader["XMIT_TIMESTAMP_GMT"].ToString())
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }

        public Int32 Insert(TradeRqmtConfirm pData)
        {
            string updateSql = DBUtils.SCHEMA_NAME + "PKG_RQMT_CONFIRM$p_insert_rqmt_confirm";
            Int32 rowsInserted = 0;
            Int32 seqNo = 0;
            string seqName = "SEQ_TRADE_RQMT_CONFIRM";
            seqNo = DBUtils.GetNextSequence(sqlConnStr, seqName);

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(updateSql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@p_id", System.Data.SqlDbType.Int).Value = seqNo;
                    cmd.Parameters.Add("@p_rqmt_id", System.Data.SqlDbType.Int).Value = pData.RqmtId;
                    cmd.Parameters.Add("@p_trade_id", System.Data.SqlDbType.Int).Value = pData.TradeId;
                    cmd.Parameters.Add("@p_template_name", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.TemplateName);
                    cmd.Parameters.Add("@p_confirm_cmt", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.ConfirmCmt);
                    cmd.Parameters.Add("@p_fax_telex_ind", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.FaxTelexInd);
                    cmd.Parameters.Add("@p_fax_telex_number", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.FaxTelexNumber);
                    cmd.Parameters.Add("@p_confirm_label", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.ConfirmLabel);
                    cmd.Parameters.Add("@p_next_status_code", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.NextStatusCode);
                    
                    conn.Open();
                    rowsInserted = cmd.ExecuteNonQuery();
                }
            }
            if (rowsInserted == 0)
                return rowsInserted;
            else
                return seqNo;
        }

        public Int32 Update(TradeRqmtConfirm pData)
        {
            string updateSql = DBUtils.SCHEMA_NAME + "PKG_RQMT_CONFIRM$p_update_rqmt_confirm";
            Int32 rowsUpdated = 0;
            
            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(updateSql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@p_id", System.Data.SqlDbType.Int).Value = pData.Id;
                    cmd.Parameters.Add("@p_rqmt_id", System.Data.SqlDbType.Int).Value = pData.RqmtId;
                    cmd.Parameters.Add("@p_trade_id", System.Data.SqlDbType.Int).Value = pData.TradeId;
                    cmd.Parameters.Add("@p_template_name", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.TemplateName);
                    cmd.Parameters.Add("@p_confirm_cmt", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.ConfirmCmt);
                    cmd.Parameters.Add("@p_fax_telex_ind", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.FaxTelexInd);
                    cmd.Parameters.Add("@p_fax_telex_number", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.FaxTelexNumber);
                    cmd.Parameters.Add("@p_confirm_label", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.ConfirmLabel);
                    cmd.Parameters.Add("@p_next_status_code", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.NextStatusCode);
                    cmd.Parameters.Add("@p_active_flag", System.Data.SqlDbType.VarChar).Value = pData.ActiveFlag;

                    conn.Open();
                    rowsUpdated = cmd.ExecuteNonQuery();
                }
            }
            return rowsUpdated;
        }

        //public int UpdateCreator(Int32 pRqmtConfirmId, string pUserId)
        //{
        //    string updateSql = DBUtils.SCHEMA_NAME + "PKG_TRADE_RQMT$p_update_trade_confirm_creator";
        //    int rowsUpdated = 0;

        //    using (SqlConnection conn = new SqlConnection(sqlConnStr))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(updateSql, conn))
        //        {
        //            //There are  3 overloaded versions of function, containing 3, 4 and 6 parameters.
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //            cmd.Parameters.Add("@p_rqmt_confirm_id", System.Data.SqlDbType.Int).Value = pRqmtConfirmId;
        //            cmd.Parameters.Add("@p_user", System.Data.SqlDbType.VarChar).Value = pUserId;

        //            conn.Open();
        //            rowsUpdated = cmd.ExecuteNonQuery();
        //        }
        //    }
        //    return rowsUpdated;
        //}

        //public string GetCreator(Int32 pRqmtConfirmId)
        //{
        //    string result = String.Empty;
        //    string sql = "select user_id " +
        //                 "from " + DBUtils.SCHEMA_NAME + "trade_rqmt_confirm_creator " +
        //                 " where rqmt_confirm_id = @rqmt_confirm_id ";

        //    using (SqlConnection conn = new SqlConnection(sqlConnStr))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(sql, conn))
        //        {
        //            cmd.CommandType = System.Data.CommandType.Text;
        //            cmd.Parameters.Add("@rqmt_confirm_id", System.Data.SqlDbType.Int).Value = pRqmtConfirmId;
        //            conn.Open();
        //            using (SqlDataReader dataReader = cmd.ExecuteReader())
        //            {
        //                if (dataReader.HasRows)
        //                {
        //                    while (dataReader.Read())
        //                    {
        //                        result = dataReader["user_id"].ToString();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}   

    }
}
