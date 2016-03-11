using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class InboundDocCallerRefDal : IInboundDocCallerRefDal
    {
        private string sqlConnStr = "";

        public InboundDocCallerRefDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        public Dictionary<string, Int32> MapCallerRef(List<InboundDocCallerRefDto> pCallerRefList)
        {
            Dictionary<string, Int32> rowsUpdatedList = new Dictionary<string, Int32>();
            Int32 rowsUpdated = 0;

            foreach (InboundDocCallerRefDto data in pCallerRefList)
            {
                rowsUpdated = MapCallerRef(data.CallerRef, data.CptyShortCode, data.RefType);
                rowsUpdatedList.Add(data.CallerRef, rowsUpdated);
            }

            return rowsUpdatedList;
        }

        public Int32 MapCallerRef(string pCallerRef, string pCptySn, string pRefType)
        {
            string updateSql = DBUtils.SCHEMA_NAME + "PKG_RQMT_CONFIRM$p_map_caller_ref";
            Int32 rowsUpdated = 0;

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(updateSql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@p_caller_ref", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pCallerRef);
                    cmd.Parameters.Add("@p_cpty_sn", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pCptySn);
                    cmd.Parameters.Add("@p_ref_type", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pRefType);

                    conn.Open();
                    rowsUpdated = cmd.ExecuteNonQuery();
                }
            }
            return rowsUpdated;
        }

        public Dictionary<string, Int32> UnmapCallerRef(List<InboundDocCallerRefDto> pCallerRefList)
        {
            Dictionary<string, Int32> rowsUpdatedList = new Dictionary<string, Int32>();
            Int32 rowsUpdated = 0;

            foreach (InboundDocCallerRefDto data in pCallerRefList)
            {
                rowsUpdated = UnmapCallerRef(data.CallerRef);
                rowsUpdatedList.Add(data.CallerRef, rowsUpdated);
            }
            return rowsUpdatedList;
        }

        public Int32 UnmapCallerRef(string pCallerRef)
        {
            string updateSql = DBUtils.SCHEMA_NAME + "PKG_RQMT_CONFIRM$p_ummap_caller_ref";
            Int32 rowsUpdated = 0;

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(updateSql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@p_caller_ref", System.Data.SqlDbType.VarChar).Value = pCallerRef;

                    conn.Open();
                    rowsUpdated = cmd.ExecuteNonQuery();
                }
            }
            return rowsUpdated;
        }

    }
}
