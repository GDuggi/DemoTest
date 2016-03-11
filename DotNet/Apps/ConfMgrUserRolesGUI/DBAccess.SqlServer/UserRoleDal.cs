using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DBAccess.SqlServer
{
    public class UserRoleDal : IUserRoleDal
    {
        private string sqlConnStr = "";

        public UserRoleDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        public List<UserRoleView> GetAllStub()
        {
            var result = new List<UserRoleView>();
            result.Add(new UserRoleView { UserId = "IFRANKEL", RoleCode = "ACCESS" });
            result.Add(new UserRoleView { UserId = "IFRANKEL", RoleCode = "CNTRCT-APP" });
            result.Add(new UserRoleView { UserId = "IFRANKEL", RoleCode = "FNAPP" });
            result.Add(new UserRoleView { UserId = "IFRANKEL", RoleCode = "FNAPP-OVR" });
            result.Add(new UserRoleView { UserId = "IFRANKEL", RoleCode = "RECALCPRI" });
            result.Add(new UserRoleView { UserId = "IFRANKEL", RoleCode = "SC-CRCXL" });
            result.Add(new UserRoleView { UserId = "IFRANKEL", RoleCode = "SUB-EFET" });
            result.Add(new UserRoleView { UserId = "IFRANKEL", RoleCode = "UPDATE" });
            return result;
        }


        public List<UserRoleView> GetAll(string pUserId)
        {
            var result = new List<UserRoleView>();
            string sql = "select user_id, role_code, descr " +
                         "from " + DBUtils.SCHEMA_NAME + "v_active_user_role " +
                         "where user_id = @user_id ";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@user_id", System.Data.SqlDbType.VarChar).Value = pUserId.ToUpper();
                    conn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new UserRoleView
                                {
                                    UserId = dataReader["user_id"].ToString(),
                                    RoleCode = dataReader["role_code"].ToString(),
                                    Descr = dataReader["descr"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            return result;
        }
        public bool IsUserIdValid(string pUserId)
        {
            bool retVal = false;
            var result = new List<UserRoleView>();
            string sql = "select name " +
                         "from sys.database_principals " +
                         "where name = @user_id ";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.Add("@user_id", System.Data.SqlDbType.VarChar).Value = pUserId.ToUpper();
                    conn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            retVal = true;
                        }
                    }
                }
            }
            return retVal;
        }
        public List<UserRoleView> GetAllUsers()
        {
            var result = new List<UserRoleView>();
            string sql = "select name from sys.database_principals where type='U'";

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
                                result.Add(new UserRoleView
                                {
                                    UserId = dataReader["name"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            return result;
            
        }
        public int AddRolesToUser(List<string> roles, string pUserId)
        {
            string InsertTSql="";
            int insertRowCount = 0;
            if (roles != null && pUserId != null)
            {
                foreach (string role in roles)
                {
                    InsertTSql = InsertTSql + "\n" + @"INSERT INTO "+ DBUtils.SCHEMA_NAME +" USER_ROLE(USER_ID,ROLE_CODE,ACTIVE_FLAG) VALUES('" + pUserId.ToUpper() + "','" + role + "','Y')";
                }
                using (TransactionScope ts = new TransactionScope())
                {
                    using (SqlConnection conn = new SqlConnection(sqlConnStr))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(InsertTSql, conn))
                        {
                            using (SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                            {
                                insertRowCount = rdr.RecordsAffected;
                            }
                        }
                    }
                    ts.Complete();
                }                
            }
            return insertRowCount;            
        }
        public int RemoveRolesFromUser(List<string> roles, string pUserId)
        {
            string DeleteSql = "";
            int deleteRowCount = 0;
            if (roles != null && pUserId != null)
            {
                string rolesStr = "";
                foreach (string role in roles)
                {
                    rolesStr = rolesStr+"'"+role+"',";
                    //InsertTSql = InsertTSql + "\n" + @"INSERT INTO USER_ROLE(USER_ID,ROLE_CODE,ACTIVE_FLAG) VALUES('" + pUserId.ToUpper() + "','" + role + "','Y')";
                }
                rolesStr = rolesStr.Remove((rolesStr.Length-1),1);
                DeleteSql = @"DELETE FROM " + DBUtils.SCHEMA_NAME + " USER_ROLE WHERE USER_ID='" + pUserId.ToUpper() + "' AND ROLE_CODE IN(" + rolesStr + ")";
                using (TransactionScope ts = new TransactionScope())
                {
                    using (SqlConnection conn = new SqlConnection(sqlConnStr))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand(DeleteSql, conn))
                        {
                            using (SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                            {
                                deleteRowCount = rdr.RecordsAffected;
                            }
                        }
                    }
                    ts.Complete();
                }
            }
            return deleteRowCount;
        }
    }
}
