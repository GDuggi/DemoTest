using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class InboundDocsDal : IInboundDocsDal
    {
        private const string SEQ_NAME = "seq_inbound_docs";
        private string sqlConnStr = "";

        public InboundDocsDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        #region Stubbed Data

        public List<InboundDocsView> GetAllStub()
        {
            //string dateStr = "";
            var result = new List<InboundDocsView>();
            result.Add(new InboundDocsView
            {
                Unresolvedcount = 0,
                Id = 8,
                CallerRef = @"email\US\15-04-23_01_2pg.pdf",
                SentTo = "US",
                RcvdTs = DateTime.ParseExact("04-28-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture), FileName = "20150428_111503_609.tif",
                DocStatusCode = "OPEN", HasAutoAsctedFlag = "N", ProcFlag = "Y"});
                result.Add(new InboundDocsView
            {
                Unresolvedcount = 0,
                Id = 11,
                CallerRef = @"email\US\15-04-23_01_pdf_A.pdf",
                SentTo = "US",
                RcvdTs = DateTime.ParseExact("04-28-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture), FileName = "20150428_111503_999.tif",
                DocStatusCode = "OPEN", HasAutoAsctedFlag = "N", ProcFlag = "Y"});
                result.Add(new InboundDocsView
            {
                Unresolvedcount = 0,
                Id = 12,
                CallerRef = @"email\US\15-04-23_02_contract.pdf",
                SentTo = "US",
                RcvdTs = DateTime.ParseExact("04-28-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture), FileName = "20150428_111504_046.tif",
                DocStatusCode = "OPEN", HasAutoAsctedFlag = "N", ProcFlag = "Y"});
                result.Add(new InboundDocsView
            {
                Unresolvedcount = 0,
                Id = 14,
                CallerRef = @"email\US\20130201_0659051272_pdf.pdf",
                SentTo = "US",
                RcvdTs = DateTime.ParseExact("04-28-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture), FileName = "20150428_111504_218.tif",
                DocStatusCode = "OPEN", HasAutoAsctedFlag = "N", ProcFlag = "Y"});
                result.Add(new InboundDocsView
            {
                Unresolvedcount = 0,
                Id = 15,
                CallerRef = @"email\US\20130204_1026260421_pdf.pdf",
                SentTo = "US",
                RcvdTs = DateTime.ParseExact("04-28-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture), FileName = "20150428_111504_296.tif",
                DocStatusCode = "OPEN", HasAutoAsctedFlag = "N", ProcFlag = "Y"});
                result.Add(new InboundDocsView
            {
                Unresolvedcount = 0,
                Id = 16,
                CallerRef = @"email\US\20130204_1028292971_4pg_pdf.pdf",
                SentTo = "US",
                RcvdTs = DateTime.ParseExact("04-28-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture), FileName = "20150428_111504_500.tif",
                DocStatusCode = "OPEN", HasAutoAsctedFlag = "N", ProcFlag = "Y"});
                result.Add(new InboundDocsView
            {
                Unresolvedcount = 0,
                Id = 17,
                CallerRef = @"email\US\20130205_0926388501_pdf.pdf",
                SentTo = "US",
                RcvdTs = DateTime.ParseExact("04-28-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture), FileName = "20150428_111504_656.tif",
                DocStatusCode = "OPEN", HasAutoAsctedFlag = "N", ProcFlag = "Y"});
                result.Add(new InboundDocsView
            {
                Unresolvedcount = 0,
                Id = 19,
                CallerRef = @"email\US\20130213_1432187921_pdf.pdf",
                SentTo = "US",
                RcvdTs = DateTime.ParseExact("04-28-2015", "MM-dd-yyyy", CultureInfo.InvariantCulture), FileName = "20150428_111504_859.tif",
                DocStatusCode = "OPEN", HasAutoAsctedFlag = "N", ProcFlag = "Y"});
                return result;
        }

        #endregion

        public List<InboundDocsView> GetAll()
        {
            var result = new List<InboundDocsView>();
            string sql = "select * from " + DBUtils.SCHEMA_NAME + "v_inbound_docs " +
                        " where doc_status_code = 'OPEN' ";

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
                                result.Add(new InboundDocsView
                                {
                                    Unresolvedcount = DBUtils.HandleInt32IfNull(dataReader["unresolvedcount"].ToString()),
                                    Id = DBUtils.HandleInt32IfNull(dataReader["id"].ToString()),
                                    MappedCptySn = dataReader["mapped_cpty_sn"].ToString(),
                                    CallerRef = dataReader["caller_ref"].ToString(),
                                    SentTo = dataReader["sent_to"].ToString(),
                                    RcvdTs = DBUtils.HandleDateTimeIfNull(dataReader["rcvd_ts"].ToString()),
                                    FileName = dataReader["file_name"].ToString(),
                                    Sender = dataReader["sender"].ToString(),
                                    Cmt = dataReader["cmt"].ToString(),
                                    DocStatusCode = dataReader["doc_status_code"].ToString(),
                                    HasAutoAsctedFlag = dataReader["has_auto_ascted_flag"].ToString(),
                                    ProcFlag = dataReader["proc_flag"].ToString(),
                                    Tradeids = dataReader["tradeids"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }

        public Int32 Insert(InboundDocsDto pData)
        {
            Int32 newId = DBUtils.GetNextSequence(sqlConnStr, SEQ_NAME);

            string sql = "Insert into " + DBUtils.SCHEMA_NAME + "inbound_docs " +
                    "   (id, caller_ref, sent_to, rcvd_ts, file_name, sender, cmt, doc_status_code, has_auto_ascted_flag, " +
                           " mapped_cpty_sn, mapped_brkr_sn, mapped_cdty_code, job_ref ) " +
                    " Values " +
                    "   (@id, @caller_ref, @sent_to, @rcvd_ts, @file_name, @sender, @cmt, @doc_status_code, @has_auto_ascted_flag, " +
                            " @mapped_cpty_sn, @mapped_brkr_sn, @mapped_cdty_code, @job_ref ) ";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = newId;
                    cmd.Parameters.Add("@caller_ref", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.CallerRef);
                    cmd.Parameters.Add("@sent_to", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.SentTo);
                    cmd.Parameters.Add("@rcvd_ts", System.Data.SqlDbType.DateTime).Value = DBUtils.ValueDateTimeOrDbNull(pData.RcvdTs);
                    cmd.Parameters.Add("@file_name", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.FileName);
                    cmd.Parameters.Add("@sender", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.Sender);
                    cmd.Parameters.Add("@cmt", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.Cmt);
                    cmd.Parameters.Add("@doc_status_code", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.DocStatusCode);
                    cmd.Parameters.Add("@has_auto_ascted_flag", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.HasAutoAsctedFlag);
                    cmd.Parameters.Add("@mapped_cpty_sn", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.MappedCptySn);
                    cmd.Parameters.Add("@mapped_brkr_sn", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.MappedBrkrSn);
                    cmd.Parameters.Add("@mapped_cdty_code", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.MappedCdtyCode);
                    cmd.Parameters.Add("@job_ref", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.JobRef);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return newId;
        }

        public Dictionary<Int32, Int32> Update(List<InboundDocsDto> pInboundDocsList)
        {
            Dictionary<Int32, Int32> rowsUpdatedList = new Dictionary<Int32, Int32>();
            int rowsUpdated = 0;

            foreach (InboundDocsDto data in pInboundDocsList)
            {
                rowsUpdated = Update(data);
                Int32 id = Convert.ToInt32(data.Id);
                rowsUpdatedList.Add(id, rowsUpdated);
            }

            return rowsUpdatedList;
        }

        public Int32 Update(InboundDocsDto pData)
        {
            string updateSql = DBUtils.SCHEMA_NAME + "PKG_INBOUND$p_update_inbound_doc";
            Int32 rowsUpdated = 0;

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(updateSql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@p_id", System.Data.SqlDbType.Int).Value = pData.Id;
                    cmd.Parameters.Add("@p_caller_ref", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.CallerRef);
                    cmd.Parameters.Add("@p_sent_to", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.SentTo);
                    cmd.Parameters.Add("@p_file_name", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.FileName);
                    cmd.Parameters.Add("@p_sender", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.Sender);
                    cmd.Parameters.Add("@p_cmt", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.Cmt);
                    cmd.Parameters.Add("@p_doc_status_code", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.DocStatusCode);

                    conn.Open();
                    rowsUpdated = cmd.ExecuteNonQuery();
                }
            }
            return rowsUpdated;
        }

        public Dictionary<Int32, Int32> UpdateStatus(Dictionary<Int32, string> pInboundDocsList)
        {
            Dictionary<Int32, Int32> rowsUpdatedList = new Dictionary<Int32, Int32>();
            int rowsUpdated = 0;

            foreach(var dictRow in pInboundDocsList)
            {
                //int id = Convert.ToInt32(data.Id);
                rowsUpdated = UpdateStatus(dictRow.Key, dictRow.Value);
                rowsUpdatedList.Add(dictRow.Key, rowsUpdated);
                rowsUpdated = 0;
            }

            return rowsUpdatedList;
        }

        public Int32 UpdateStatus(Int32 pId, string pStatus)
        {
            Int32 rowsUpdated = 0;
            string sql = "update " + DBUtils.SCHEMA_NAME + "inbound_docs " +
                " set doc_status_code = @doc_status_code " +
                " where id = @id";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = pId;
                    cmd.Parameters.Add("@doc_status_code", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pStatus);

                    conn.Open();
                    rowsUpdated = cmd.ExecuteNonQuery();
                }
            }
            return rowsUpdated;
        }

        //Israel 8/15/15 -- used strictly by unit tests.
        public Int32 Delete(Int32 pId)
        {
            Int32 rowsDeleted = 0;
            string sql = "delete from " + DBUtils.SCHEMA_NAME + "inbound_docs where id = @id";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = pId;
                    conn.Open();
                    rowsDeleted = cmd.ExecuteNonQuery();
                }
            }
            return rowsDeleted;
        }

    }
}
