using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class UserFiltersOpsmgrDal : IUserFiltersOpsmgrDal
    {
        public const string SEQ_NAME = "seq_user_filters_opsmgr";
        private string sqlConnStr = "";

        public UserFiltersOpsmgrDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        #region Stub Data

        public List<UserFiltersOpsmgrDto> GetAllStub()
        {
            var result = new List<UserFiltersOpsmgrDto>();
            result.Add(new UserFiltersOpsmgrDto { 
                Id = 294, 
                UserId = "IFRANKEL", 
                Descr = "Ready For Final Approval", 
                FilterExpr = "[ReadyForFinalApprovalFlag] = ''Y''" 
            });
            result.Add(new UserFiltersOpsmgrDto
            {
                Id = 295,
                UserId = "IFRANKEL",
                Descr = "Need Contracts Assigned",
                FilterExpr = "[SetcMeth] = ''Our Paper'' And [SetcStatus] = ''NEW''"
            });
            result.Add(new UserFiltersOpsmgrDto
            {
                Id = 296,
                UserId = "IFRANKEL",
                Descr = "Ready to Send",
                FilterExpr = "[SetcMeth] = ''Our Paper'' And [SetcStatus] = ''OK_TO_SEND''"
            });
            result.Add(new UserFiltersOpsmgrDto
            {
                Id = 297,
                UserId = "IFRANKEL",
                Descr = "Broker Paper",
                FilterExpr = "[BkrMeth] = ''Broker Paper''"
            });
            result.Add(new UserFiltersOpsmgrDto
            {
                Id = 333,
                UserId = "IFRANKEL",
                Descr = "Symphony Oil Physical",
                FilterExpr = "[TrdSysCode] = ''SYM'' And [TradeTypeCode] = ''PHYSICAL''"
            });
            result.Add(new UserFiltersOpsmgrDto
            {
                Id = 334,
                UserId = "IFRANKEL",
                Descr = "Paper Not NEW",
                FilterExpr = "[SetcMeth] = ''Our Paper'' And [SetcStatus] <> ''NEW''"
            });
            return result;
        }

        #endregion

        public List<UserFiltersOpsmgrDto> GetAll(string pUserId)
        {
            var result = new List<UserFiltersOpsmgrDto>();
            string sql = "select id, user_id, descr, filter_expr " +
                         "from " + DBUtils.SCHEMA_NAME + "v_user_filters " +
                         "where user_id = @user_id ";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@user_id", System.Data.SqlDbType.VarChar).Value = pUserId;
                    conn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new UserFiltersOpsmgrDto
                                {
                                    Id = DBUtils.HandleInt32IfNull(dataReader["id"].ToString()),
                                    UserId = dataReader["user_id"].ToString(),
                                    Descr = dataReader["descr"].ToString(),
                                    FilterExpr = dataReader["filter_expr"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }

        public UserFiltersOpsmgrDto Get(Int32 pId)
        {
            var result = new UserFiltersOpsmgrDto();
            string sql = "select id, user_id, descr, filter_expr " +
                         "from " + DBUtils.SCHEMA_NAME + "v_user_filters " +
                         "where id = @id ";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = pId;

                    conn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result.Id = DBUtils.HandleInt32IfNull(dataReader["id"].ToString());
                                result.UserId = dataReader["user_id"].ToString();
                                result.Descr = dataReader["descr"].ToString();
                                result.FilterExpr = dataReader["filter_expr"].ToString();
                            }
                        }
                    }
                }
            }
            return result;
        }

        public Int32 Insert(UserFiltersOpsmgrDto pData)
        {
            Int32 newId = DBUtils.GetNextSequence(sqlConnStr, SEQ_NAME);

            string sql = "Insert into " + DBUtils.SCHEMA_NAME + "user_filters_opsmgr " +
                    "   (id, user_id, descr, filter_expr) " +
                    " Values " +
                    "   (@id, @user_id, @descr, @filter_expr) ";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = newId;
                    cmd.Parameters.Add("@user_id", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.UserId);
                    cmd.Parameters.Add("@descr", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.Descr);
                    cmd.Parameters.Add("@filter_expr", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.FilterExpr);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return newId;
        }

        public Int32 Update(UserFiltersOpsmgrDto pData)
        {
            Int32 rowsUpdated = 0;
            string sql = "update " + DBUtils.SCHEMA_NAME + "user_filters_opsmgr " +
                "set user_id = @user_id, descr = @descr, filter_expr = @filter_expr " +
                " where id = @id";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = pData.Id;
                    cmd.Parameters.Add("@user_id", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.UserId);
                    cmd.Parameters.Add("@descr", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.Descr);
                    cmd.Parameters.Add("@filter_expr", System.Data.SqlDbType.VarChar).Value = DBUtils.ValueStringOrDBNull(pData.FilterExpr);

                    conn.Open();
                    rowsUpdated = cmd.ExecuteNonQuery();
                }
            }
            return rowsUpdated;
        }

        public Int32 Delete(int pId)
        {
            Int32 rowsDeleted = 0;
            string sql = "delete from " + DBUtils.SCHEMA_NAME + "user_filters_opsmgr where id = @id";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = pId;
                    rowsDeleted = cmd.ExecuteNonQuery();
                }
            }
            return rowsDeleted;
        }

    }
}
