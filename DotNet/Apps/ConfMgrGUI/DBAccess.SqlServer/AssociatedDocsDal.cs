using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class AssociatedDocsDal : IAssociatedDocsDal
    {
        private string sqlConnStr = "";

        public AssociatedDocsDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        #region Stubbed Data

        public List<AssociatedDoc> GetAllStub()
        {
            //string dateStr = "";
            var result = new List<AssociatedDoc>();
            result.Add(new AssociatedDoc
            {
                Id = 6,
                InboundDocsId = 4,
                TradeFinalApprovalFlag = "N",
                IndexVal = 1,
                FileName = "20150423_135943_199_1.tif",
                TradeId = 1495866,
                DocStatusCode = "APPROVED", 
                AssociatedBy = "IFRANKEL",
                AssociatedDt = DateTime.ParseExact("04-28-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                FinalApprovedBy = "IFRANKEL",
                FinalApprovedDt = DateTime.ParseExact("04-28-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture),
                CdtyGroupCode = "NGAS",
                CptySn = "MIZUHO",
                DocTypeCode = "NOCNF",
                SecValidateReqFlag = "N",
                TradeRqmtId = 280559,
                SentTo = "US"
               });           
            return result;
        }

        #endregion

        public List<AssociatedDoc> GetAll()
        {
            var result = new List<AssociatedDoc>();
            string sql = "select * from " + DBUtils.SCHEMA_NAME + "v_active_associated_docs ";
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
                                result.Add(new AssociatedDoc
                                {
                                    Id = DBUtils.HandleInt32IfNull(dataReader["id"].ToString()),
                                    InboundDocsId = DBUtils.HandleInt32IfNull(dataReader["inbound_docs_id"].ToString()),
                                    TradeFinalApprovalFlag = dataReader["trade_final_approval_flag"].ToString(),
                                    IndexVal = DBUtils.HandleInt32IfNull(dataReader["index_val"].ToString()),
                                    TrdSysTicket = dataReader["TRD_SYS_TICKET"].ToString(),
                                    TrdSysCode = dataReader["TRD_SYS_CODE"].ToString(),
                                    FileName = dataReader["file_name"].ToString(),
                                    TradeId = DBUtils.HandleInt32IfNull(dataReader["trade_id"].ToString()),
                                    DocStatusCode = dataReader["doc_status_code"].ToString(),
                                    AssociatedBy = dataReader["associated_by"].ToString(),
                                    AssociatedDt = DBUtils.HandleDateTimeIfNull(dataReader["associated_dt"].ToString()),
                                    FinalApprovedBy = dataReader["final_approved_by"].ToString(),
                                    FinalApprovedDt = DBUtils.HandleDateTimeIfNull(dataReader["final_approved_dt"].ToString()),
                                    DisputedBy = dataReader["disputed_by"].ToString(),
                                    DisputedDt = DBUtils.HandleDateTimeIfNull(dataReader["disputed_dt"].ToString()),
                                    DiscardedBy = dataReader["discarded_by"].ToString(),
                                    DiscardedDt = DBUtils.HandleDateTimeIfNull(dataReader["discarded_dt"].ToString()),
                                    VaultedBy = dataReader["vaulted_by"].ToString(),
                                    VaultedDt = DBUtils.HandleDateTimeIfNull(dataReader["vaulted_dt"].ToString()),
                                    CdtyGroupCode = dataReader["cdty_group_code"].ToString(),
                                    CptySn = dataReader["cpty_sn"].ToString(),
                                    BrokerSn = dataReader["broker_sn"].ToString(),
                                    DocTypeCode = dataReader["doc_type_code"].ToString(),
                                    SecValidateReqFlag = dataReader["sec_validate_req_flag"].ToString(),
                                    TradeRqmtId = DBUtils.HandleInt32IfNull(dataReader["trade_rqmt_id"].ToString()),
                                    XmitStatusCode = dataReader["xmit_status_code"].ToString(),
                                    XmitValue = dataReader["xmit_value"].ToString(),
                                    SentTo = dataReader["sent_to"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }

        public List<AssociatedDoc> GetAll(string pTradeIdList)
        //public List<AssociatedDoc> GetAllSelectedTrades(string pTrdSysCode, string pSeCptySn, string pCptySn,
        //            string pCdtyCode, DateTime pBeginTradeDt, DateTime pEndTradeDt, string pTrdSysTicket)
        {
            var result = new List<AssociatedDoc>();
            //string sql = "select * from " + DBUtils.SCHEMA_NAME + "v_active_associated_docs v, " +
            //             DBUtils.GetAllSqlParamStr(pTrdSysCode, pSeCptySn, pCptySn, pCdtyCode,
            //             pBeginTradeDt, pEndTradeDt, pTrdSysTicket);

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
                                result.Add(new AssociatedDoc
                                {
                                    Id = DBUtils.HandleInt32IfNull(dataReader["id"].ToString()),
                                    InboundDocsId = DBUtils.HandleInt32IfNull(dataReader["inbound_docs_id"].ToString()),
                                    TradeFinalApprovalFlag = dataReader["trade_final_approval_flag"].ToString(),
                                    IndexVal = DBUtils.HandleInt32IfNull(dataReader["index_val"].ToString()),
                                    TrdSysTicket = dataReader["TRD_SYS_TICKET"].ToString(),
                                    TrdSysCode = dataReader["TRD_SYS_CODE"].ToString(),
                                    FileName = dataReader["file_name"].ToString(),
                                    TradeId = DBUtils.HandleInt32IfNull(dataReader["trade_id"].ToString()),
                                    DocStatusCode = dataReader["doc_status_code"].ToString(),
                                    AssociatedBy = dataReader["associated_by"].ToString(),
                                    AssociatedDt = DBUtils.HandleDateTimeIfNull(dataReader["associated_dt"].ToString()),
                                    FinalApprovedBy = dataReader["final_approved_by"].ToString(),
                                    FinalApprovedDt = DBUtils.HandleDateTimeIfNull(dataReader["final_approved_dt"].ToString()),
                                    DisputedBy = dataReader["disputed_by"].ToString(),
                                    DisputedDt = DBUtils.HandleDateTimeIfNull(dataReader["disputed_dt"].ToString()),
                                    DiscardedBy = dataReader["discarded_by"].ToString(),
                                    DiscardedDt = DBUtils.HandleDateTimeIfNull(dataReader["discarded_dt"].ToString()),
                                    VaultedBy = dataReader["vaulted_by"].ToString(),
                                    VaultedDt = DBUtils.HandleDateTimeIfNull(dataReader["vaulted_dt"].ToString()),
                                    CdtyGroupCode = dataReader["cdty_group_code"].ToString(),
                                    CptySn = dataReader["cpty_sn"].ToString(),
                                    BrokerSn = dataReader["broker_sn"].ToString(),
                                    DocTypeCode = dataReader["doc_type_code"].ToString(),
                                    SecValidateReqFlag = dataReader["sec_validate_req_flag"].ToString(),
                                    TradeRqmtId = DBUtils.HandleInt32IfNull(dataReader["trade_rqmt_id"].ToString()),
                                    XmitStatusCode = dataReader["xmit_status_code"].ToString(),
                                    XmitValue = dataReader["xmit_value"].ToString(),
                                    SentTo = dataReader["sent_to"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }

        public Int32 UpdateStatus(AssociatedDocsDto pAssocDocsData)
        {
            //Note -- this procedure also inserts data
            string updateSql = DBUtils.SCHEMA_NAME + "PKG_INBOUND$p_update_asso_status";
            Int32 rowsUpdated = 0;

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(updateSql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@p_inb_id", System.Data.SqlDbType.Int).Value = pAssocDocsData.InboundDocsId;
                    cmd.Parameters.Add("@p_file_name", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pAssocDocsData.FileName);
                    cmd.Parameters.Add("@p_trade_id", System.Data.SqlDbType.Int).Value = pAssocDocsData.TradeId;
                    cmd.Parameters.Add("@p_status", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pAssocDocsData.DocStatusCode);
                    cmd.Parameters.Add("@p_cdty_grp_code", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pAssocDocsData.CdtyGroupCode);
                    cmd.Parameters.Add("@p_cpty_sn", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pAssocDocsData.CptySn);
                    cmd.Parameters.Add("@p_broker_sn", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pAssocDocsData.BrokerSn);
                    cmd.Parameters.Add("@p_rqmt_id", System.Data.SqlDbType.Int).Value = pAssocDocsData.TradeRqmtId;
                    cmd.Parameters.Add("@p_rqmt_status", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pAssocDocsData.RqmtStatus);
                    cmd.Parameters.Add("@p_rqmt_type", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pAssocDocsData.DocTypeCode);
                    cmd.Parameters.Add("@p_sec_check", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pAssocDocsData.SecValidateReqFlag);
                    cmd.Parameters.Add("@p_index_val", System.Data.SqlDbType.Int).Value = pAssocDocsData.IndexVal;
                    SqlParameter returnValue = new SqlParameter{ Direction = ParameterDirection.ReturnValue};
                    cmd.Parameters.Add(returnValue);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return Convert.ToInt32(returnValue.Value);
                }
            }
            return rowsUpdated;
        }

        public Int32 GetCount(Int32 pInboundDocsId)
        {
            Int32 result = 0;
            string sql = "select count(*) cnt from " + DBUtils.SCHEMA_NAME + "associated_docs " +
                         "where inbound_docs_id = @inbound_docs_id";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@inbound_docs_id", System.Data.SqlDbType.Int).Value = pInboundDocsId;
                    conn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result = dataReader.GetInt32(dataReader.GetOrdinal("cnt"));
                            }
                        }
                    }
                }
            }
            return result;
        }

        public Int32 GetCount(Int32 pInboundDocsId, string pDocStatusCode)
        {
            Int32 result = 0;
            string sql = "select count(*) cnt from " + DBUtils.SCHEMA_NAME + "associated_docs " +
                         "where inbound_docs_id = @inbound_docs_id and doc_status_code = @doc_status_code";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@inbound_docs_id", System.Data.SqlDbType.Int).Value = pInboundDocsId;
                    cmd.Parameters.Add("@doc_status_code", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pDocStatusCode);
                    conn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result = dataReader.GetInt32(dataReader.GetOrdinal("cnt"));
                            }
                        }
                    }
                }
            }
            return result;
        }

        public Int32 GetCurrentIndexValue(Int32 pInboundDocsId)
        {
            Int32 result = 0;
            string sql = "select isnull(max(index_val), 0) max_idx_val from " + DBUtils.SCHEMA_NAME + "associated_docs " +
                         "where inbound_docs_id = @inbound_docs_id";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@inbound_docs_id", System.Data.SqlDbType.Int).Value = pInboundDocsId;
                    conn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                var obj = dataReader["max_idx_val"];
                                result = Convert.ToInt32(obj);
                            }
                        }
                    }
                }
            }
            return result;
        }

    }
}
