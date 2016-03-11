using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace DBAccess.SqlServer
{
    public class InboundFaxNosDal : IInboundFaxNosDal
    {
        private string sqlConnStr = "";

        public InboundFaxNosDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        public List<InboundFaxNosDto> GetAllStub()
        {
            var result = new List<InboundFaxNosDto>();
            result.Add(new InboundFaxNosDto { Faxno = "203-349-7693" });
            result.Add(new InboundFaxNosDto { Faxno = "203-349-7694" });
            result.Add(new InboundFaxNosDto { Faxno = "EU" });
            result.Add(new InboundFaxNosDto { Faxno = "US" });
            return result;
        }

        public List<InboundFaxNosDto> GetAll()
        {
            //List<UserCompanyDto> companyList = new List<UserCompanyDto>();
            var result = new List<InboundFaxNosDto>();
            string sql = "select faxno from " + DBUtils.SCHEMA_NAME + "inbound_fax_nos  " +
                         " where active_flag = 'Y' order by faxno asc ";

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
                                result.Add(new InboundFaxNosDto
                                {
                                    Faxno = dataReader["faxno"].ToString()
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
