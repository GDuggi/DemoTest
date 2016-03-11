using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class InbAttribMapValDal : IInbAttribMapValDal
    {
        public const string SEQ_NAME = "seq_inb_attrib_map_val";
        private string sqlConnStr = "";

        public InbAttribMapValDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        public List<InbAttribMapValDto> GetMapValues(string pInbAttribCode)
        {
            var result = new List<InbAttribMapValDto>();
            string sql = "select id, mapped_value, descr " + 
                        " from " + DBUtils.SCHEMA_NAME + "inb_attrib_map_val " +
                        " where inb_attrib_code = @inb_attrib_code " +
                        " and active_flag ='Y'";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@inb_attrib_code", System.Data.SqlDbType.VarChar).Value = pInbAttribCode;
                    conn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new InbAttribMapValDto
                                {
                                    //Id = Convert.ToInt32(dataReader.GetInt32(dataReader.GetOrdinal("id"))),
                                    //MappedValue = dataReader.GetString(dataReader.GetOrdinal("mapped_value")),
                                    //Descr = dataReader.GetString(dataReader.GetOrdinal("descr"))

                                    Id = DBUtils.HandleInt32IfNull(dataReader["id"].ToString()),
                                    MappedValue = dataReader["mapped_value"].ToString(),
                                    Descr = dataReader["mapped_value"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }
 
        public Int32 Insert(InbAttribMapValDto pData)
        {
            Int32 newId = DBUtils.GetNextSequence(sqlConnStr, SEQ_NAME);

            string sql = "Insert into " + DBUtils.SCHEMA_NAME + "inb_attrib_map_val " +
                    "   (id, inb_attrib_code,  mapped_value,  descr,  active_flag) " +
                    " Values " +
                    "   (@id, @inb_attrib_code, @mapped_value, @descr, @active_flag) ";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = newId;
                    cmd.Parameters.Add("@inb_attrib_code", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.InbAttribCode);
                    cmd.Parameters.Add("@mapped_value", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.MappedValue);
                    cmd.Parameters.Add("@descr", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.Descr);
                    cmd.Parameters.Add("@active_flag", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.ActiveFlag);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return newId;
        }

        public Int32 Update(InbAttribMapValDto pData)
        {
            Int32 rowsUpdated = 0;
            string sql = "update " + DBUtils.SCHEMA_NAME + "inb_attrib_map_val " +
                "set inb_attrib_code = @inb_attrib_code, mapped_value = @mapped_value, " + 
                " descr = @descr, active_flag = @active_flag " +
                " where id = @id";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = pData.Id;
                    cmd.Parameters.Add("@inb_attrib_code", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.InbAttribCode);
                    cmd.Parameters.Add("@mapped_value", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.MappedValue);
                    cmd.Parameters.Add("@descr", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.Descr);
                    cmd.Parameters.Add("@active_flag", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.ActiveFlag);

                    conn.Open();
                    rowsUpdated = cmd.ExecuteNonQuery();
                }
            }
            return rowsUpdated;
        }

        public Int32 Delete(Int32 pId)
        {
            Int32 rowsDeleted = 0;
            string sql = "delete from " + DBUtils.SCHEMA_NAME + "inb_attrib_map_val where id = @id";

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

        //Used to build test cases, not used in production or currently tested.
        //public int Delete(string pMappedValue)
        //{
        //    int rowsDeleted = 0;
        //    string sql = "delete from " + DBUtils.SCHEMA_NAME + "inb_attrib_map_val where mapped_value = @mapped_value";

        //    using (SqlConnection conn = new SqlConnection(sqlConnStr))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(sql, conn))
        //        {
        //            cmd.CommandType = System.Data.CommandType.Text;
        //            cmd.Parameters.Add("@mapped_value", System.Data.SqlDbType.VarChar).Value = pMappedValue;
        //            conn.Open();
        //            rowsDeleted = cmd.ExecuteNonQuery();
        //        }
        //    }
        //    return rowsDeleted;
        //}

    }
}
