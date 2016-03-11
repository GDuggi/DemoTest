using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class RoleDal : IRoleDal
    {
        private string sqlConnStr = "";

        public RoleDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        public List<RoleView> GetAllStub()
        {
            var result = new List<RoleView>();
            //result.Add(new RoleView { UserId = "IFRANKEL", RoleCode = "ACCESS" });
            //result.Add(new RoleView { UserId = "IFRANKEL", RoleCode = "CNTRCT-APP" });
            //result.Add(new RoleView { UserId = "IFRANKEL", RoleCode = "FNAPP" });
            //result.Add(new RoleView { UserId = "IFRANKEL", RoleCode = "FNAPP-OVR" });
            //result.Add(new RoleView { UserId = "IFRANKEL", RoleCode = "RECALCPRI" });
            //result.Add(new RoleView { UserId = "IFRANKEL", RoleCode = "SC-CRCXL" });
            //result.Add(new RoleView { UserId = "IFRANKEL", RoleCode = "SUB-EFET" });
            //result.Add(new RoleView { UserId = "IFRANKEL", RoleCode = "UPDATE" });
            return result;
        }


        public List<RoleView> GetAll()
        {
            var result = new List<RoleView>();
            string sql = "select CODE, DESCR " +
                         "from " + DBUtils.SCHEMA_NAME + "role " +
                         "where ACTIVE_FLAG = 'Y'";

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    //cmd.Parameters.Add("@user_id", System.Data.SqlDbType.VarChar).Value = pUserId.ToUpper();
                    conn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new RoleView
                                {
                                    RoleCode = dataReader["CODE"].ToString(),
                                    Descr = dataReader["DESCR"].ToString()
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
