using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class CdtyGroupCodesDal : ICdtyGroupCodesDal
    {
        private string sqlConnStr = "";

        public CdtyGroupCodesDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        #region Stubbed Data

        public List<GetCdtyGroupCodesDto> GetAllStub()
        {
            var result = new List<GetCdtyGroupCodesDto>();
            result.Add(new GetCdtyGroupCodesDto { CdtyGroupCode = "COAL" });
            result.Add(new GetCdtyGroupCodesDto { CdtyGroupCode = "ELEC" });
            result.Add(new GetCdtyGroupCodesDto { CdtyGroupCode = "NGAS" });
            result.Add(new GetCdtyGroupCodesDto { CdtyGroupCode = "OIL" });            
            return result;
        }

        #endregion

        public List<GetCdtyGroupCodesDto> GetAll()
        {
            var result = new List<GetCdtyGroupCodesDto>();
            string sql = "select distinct cdty_grp_code from " + DBUtils.SCHEMA_NAME + "trade_data " +
                        " where cdty_grp_code is not null " +
                        " order by cdty_grp_code ";

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
                                result.Add(new GetCdtyGroupCodesDto
                                {
                                    CdtyGroupCode = dataReader.GetString(dataReader.GetOrdinal("cdty_grp_code"))
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
