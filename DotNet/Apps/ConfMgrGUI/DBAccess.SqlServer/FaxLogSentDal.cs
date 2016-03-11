using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class FaxLogSentDal : IFaxLogSentDal
    {
        public const string SEQ_NAME = "seq_fax_log_sent";
        private string sqlConnStr = "";

        public FaxLogSentDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        public void Insert(FaxLogSentDto pData)
        {
            //int newId = DBUtils.GetNextSequence(sqlConnStr, SEQ_NAME);
            string insertSql = DBUtils.SCHEMA_NAME + "PKG_RQMT_CONFIRM$p_insert_fax_log_sent";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(insertSql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@p_trade_id", System.Data.SqlDbType.Int).Value = pData.TradeId;
                    cmd.Parameters.Add("@p_doc_type", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.DocType);
                    cmd.Parameters.Add("@p_sender", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.Sender);
                    cmd.Parameters.Add("@p_telex_code", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.FaxTelexCode);
                    cmd.Parameters.Add("@p_telex_number", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.FaxTelexNumber);
                    cmd.Parameters.Add("@p_doc_ref_code", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.DocRefCode);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            //return newId;
        }
    }
}
