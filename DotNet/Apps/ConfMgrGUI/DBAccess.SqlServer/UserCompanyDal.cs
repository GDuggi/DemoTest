using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace DBAccess.SqlServer
{
    public class UserCompanyDal : IUserCompanyDal
    {
        private string sqlConnStr = "";

        public UserCompanyDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        public List<UserCompanyDto> GetAllStub()
        {
            var result = new List<UserCompanyDto>();
            result.Add(new UserCompanyDto { CompanySn = "AMPH US", UserId = "IFRANKEL", ActiveFlag = "Y" });
            result.Add(new UserCompanyDto { CompanySn = "BTGPC SING", UserId = "IFRANKEL", ActiveFlag = "Y" });
            result.Add(new UserCompanyDto { CompanySn = "BTGPC SWIT", UserId = "IFRANKEL", ActiveFlag = "Y" });
            result.Add(new UserCompanyDto { CompanySn = "BTGPC US", UserId = "IFRANKEL", ActiveFlag = "Y" });
            return result;
        }


        public List<UserCompanyDto> GetAll(string pPermissionKeyInClause)
        {
            //List<UserCompanyDto> companyList = new List<UserCompanyDto>();
            var result = new List<UserCompanyDto>();
            //string sql = "select u.company_sn " +
            //             "from " + DBUtils.SCHEMA_NAME + "user_company u, " +
            //             "     " + DBUtils.SCHEMA_NAME + "company_mast c " +
            //             "where u.company_sn = c.company_sn " +
            //             "    and c.active_flag = 'Y' " +
            //             "    and u.user_id = @user_id " +
            //             "    and u.active_flag = 'Y'";

            string sql = "select distinct booking_co_sn " +
                         "from " + DBUtils.SCHEMA_NAME + "V_PC_TRADE_SUMMARY";

            //Israel 11/20/2015 -- Get all companies, regardless of access rights.
            //if (pPermissionKeyInClause != "")
            //    sql += " where " + pPermissionKeyInClause;

            using (SqlConnection conn = new SqlConnection(sqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    cmd.CommandType = System.Data.CommandType.Text;
                    //cmd.Parameters.Add("@user_id", System.Data.SqlDbType.VarChar).Value = pUserId;
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new UserCompanyDto
                                {
                                    CompanySn = dataReader["booking_co_sn"].ToString()
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
