using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class CdtyCodeLkupDal : ICdtyCodeLkupDal
    {
        private string sqlConnStr = "";

        public CdtyCodeLkupDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        #region Stubbed Data

        public List<BdtaCdtyLkup> GetAllStub()
        {
            //string dateStr = "";
            var result = new List<BdtaCdtyLkup>();
            result.Add(new BdtaCdtyLkup { CdtyCode = "COAL" });
            result.Add(new BdtaCdtyLkup { CdtyCode = "ELEC" });
            result.Add(new BdtaCdtyLkup { CdtyCode = "ICAP" });
            result.Add(new BdtaCdtyLkup { CdtyCode = "NGAS" });
            result.Add(new BdtaCdtyLkup { CdtyCode = "OIL" });            
            return result;
        }

        #endregion

        public List<BdtaCdtyLkup> GetAll()
        {
            var result = new List<BdtaCdtyLkup>();
            string sql = "select cdty_code from " + DBUtils.SCHEMA_NAME + "v_trade_data_cdty_code_lkup " +
                        " order by cdty_code asc";

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
                                result.Add(new BdtaCdtyLkup
                                {
                                    CdtyCode = dataReader["cdty_code"].ToString()
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
