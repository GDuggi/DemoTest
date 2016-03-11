using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class InboundDocUserFlagDal : IInboundDocUserFlagDal
    {
        private string sqlConnStr = "";

        public InboundDocUserFlagDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        public List<InboundDocUserFlagDto> Get(string pInboundUser)
        {
            var result = new List<InboundDocUserFlagDto>();
            string sql = "select id, inbound_doc_id, inbound_user, flag_type, comments " +
                        " from " + DBUtils.SCHEMA_NAME + "inbound_doc_user_flag " +
                        " where inbound_user = @inbound_user ";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@inbound_user", System.Data.SqlDbType.VarChar).Value = pInboundUser;
                    conn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new InboundDocUserFlagDto
                                {
                                    Id = DBUtils.HandleInt32IfNull(dataReader["id"].ToString()),
                                    InboundDocId = DBUtils.HandleInt32IfNull(dataReader["inbound_doc_id"].ToString()),
                                    InboundUser = dataReader["inbound_user"].ToString(),
                                    FlagType = dataReader["flag_type"].ToString(),
                                    Comments = dataReader["comments"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }

        public Dictionary<Int32, Int32> UpdateFlags(List<InboundDocUserFlagDto> pUserFlagList)
        {
            Dictionary<Int32, Int32> rowsUpdatedList = new Dictionary<Int32, Int32>();
            Int32 rowsUpdated = 0;

            foreach (InboundDocUserFlagDto data in pUserFlagList)
            {
                rowsUpdated = UpdateFlag(data);
                rowsUpdatedList.Add(data.InboundDocId, rowsUpdated);
            }

            return rowsUpdatedList;
        }

        public Int32 UpdateFlag(InboundDocUserFlagDto pUserFlagData)
        {
            string updateSql = DBUtils.SCHEMA_NAME + "PKG_INBOUND_EXT$p_update_user_flag";
            Int32 rowsUpdated = 0;

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(updateSql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@p_inbound_doc_id", System.Data.SqlDbType.Int).Value = pUserFlagData.InboundDocId;
                    cmd.Parameters.Add("@p_inbound_user", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pUserFlagData.InboundUser);
                    cmd.Parameters.Add("@p_flag_type", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pUserFlagData.FlagType);
                    cmd.Parameters.Add("@p_comment", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pUserFlagData.Comments);
                    cmd.Parameters.Add("@p_update_delete_ind", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pUserFlagData.UpdateDeleteInd);

                    conn.Open();
                    rowsUpdated = cmd.ExecuteNonQuery();
                }
            }
            return rowsUpdated;
        }


    }
}
