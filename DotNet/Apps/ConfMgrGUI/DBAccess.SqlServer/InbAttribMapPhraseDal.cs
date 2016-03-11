using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class InbAttribMapPhraseDal : IInbAttribMapPhraseDal
    {
        public const string SEQ_NAME = "seq_inb_attrib_map_phrase";
        private string sqlConnStr = "";

        public InbAttribMapPhraseDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        public List<InbAttribMapComboDto> GetPhrases(string pMappedValue)
        {
            var result = new List<InbAttribMapComboDto>();
            string sql = "select mv.id as mapped_val_id, mv.descr, mp.phrase, mp.id as phrase_id " +
                        " from " + DBUtils.SCHEMA_NAME + "inb_attrib_map_val mv, " +
                                   DBUtils.SCHEMA_NAME + "inb_attrib_map_phrase mp " +
                        " where mv.active_flag = 'Y' and " +
                        " mv.mapped_value = @mapped_value and " +
                        " mp.inb_attrib_map_val_id = mv.id and " +
                        " mp.active_flag = 'Y' " +
                        " order by mp.inb_attrib_map_val_id, mp.id asc";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@mapped_value", System.Data.SqlDbType.VarChar).Value = pMappedValue;
                    conn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new InbAttribMapComboDto
                                {
                                    MappedValId = DBUtils.HandleInt32IfNull(dataReader["mapped_val_id"].ToString()),
                                    PhraseId = DBUtils.HandleInt32IfNull(dataReader["phrase_id"].ToString()),
                                    Phrase = dataReader["phrase"].ToString(),
                                    Descr = dataReader["descr"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }
         
        public List<InbAttribMapPhraseDto> GetPhrases(Int32 pMappedValueId)
        {
            var result = new List<InbAttribMapPhraseDto>();
            string sql = "select * from " + DBUtils.SCHEMA_NAME + "inb_attrib_map_phrase " +
                        " where active_flag = 'Y' and " +
                        " inb_attrib_map_val_id = @inb_attrib_map_val_id";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@inb_attrib_map_val_id", System.Data.SqlDbType.Int).Value = pMappedValueId;
                    conn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new InbAttribMapPhraseDto
                                {
                                    Id = DBUtils.HandleInt32IfNull(dataReader["id"].ToString()),
                                    InbAttribMapValId = DBUtils.HandleInt32IfNull(dataReader["inb_attrib_map_val_id"].ToString()),
                                    Phrase = dataReader["phrase"].ToString(),
                                    ActiveFlag = dataReader["active_flag"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            return result;
        } 

        public Int32 Insert(InbAttribMapPhraseDto pData)
        {
            Int32 newId = DBUtils.GetNextSequence(sqlConnStr, SEQ_NAME);

            string sql = "Insert into " + DBUtils.SCHEMA_NAME + "inb_attrib_map_phrase " +
                    "   (id, inb_attrib_map_val_id, phrase, active_flag) " +
                    " Values " +
                    "   (@id, @inb_attrib_map_val_id, @phrase, @active_flag) ";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = newId;
                    cmd.Parameters.Add("@inb_attrib_map_val_id", System.Data.SqlDbType.Int).Value = pData.InbAttribMapValId;
                    cmd.Parameters.Add("@phrase", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.Phrase);
                    cmd.Parameters.Add("@active_flag", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.ActiveFlag);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return newId;
        }

        public Int32 Update(InbAttribMapPhraseDto pData)
        {
            Int32 rowsUpdated = 0;
            string sql = "update " + DBUtils.SCHEMA_NAME + "inb_attrib_map_phrase " +
                "set inb_attrib_map_val_id = @inb_attrib_map_val_id, phrase = @phrase, active_flag = @active_flag " +
                " where id = @id";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = pData.Id;
                    cmd.Parameters.Add("@inb_attrib_map_val_id", System.Data.SqlDbType.Int).Value = pData.InbAttribMapValId;
                    cmd.Parameters.Add("@phrase", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.Phrase);
                    cmd.Parameters.Add("@active_flag", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.ActiveFlag);

                    conn.Open();
                    SqlTransaction trans = conn.BeginTransaction();
                    try
                    {
                        cmd.Transaction = trans;
                        rowsUpdated = cmd.ExecuteNonQuery();
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        //throw;
                    }
                }
            }
            return rowsUpdated;
        }

        public Int32 Delete(Int32 pId)
        {
            Int32 rowsDeleted = 0;
            string sql = "delete from " + DBUtils.SCHEMA_NAME + "inb_attrib_map_phrase where id = @id";

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
