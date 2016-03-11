using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

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
                         "where UPPER(user_id) = UPPER(@user_id) ";

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
    }
}
