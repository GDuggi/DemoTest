using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DBAccess.SqlServer
{
    public class CptyInfoDal : ICptyInfoDal
    {
        //private string sqlConnStr = "";
        private string sqlConnStr = "";

        public CptyInfoDal(string pSqlConnectionStr)
        {
            sqlConnStr = pSqlConnectionStr;
        }

        #region Stub Data

        public List<BdtaCptyLkup> GetOpenConfirmLookupStub()
        {
            //string dateStr = "";
            var result = new List<BdtaCptyLkup>();
            result.Add(new BdtaCptyLkup { CptySn = "DBLONDON" });
            result.Add(new BdtaCptyLkup { CptySn = "JPM SECURI" });
            result.Add(new BdtaCptyLkup { CptySn = "MIZUHO" });
            result.Add(new BdtaCptyLkup { CptySn = "MMGS INC" });
            result.Add(new BdtaCptyLkup { CptySn = "PJM" });
            result.Add(new BdtaCptyLkup { CptySn = "TESTOTC" });
            return result;
        }

        #endregion

        public List<BdtaCptyLkup> GetOpenConfirmLookup()
        {
            var result = new List<BdtaCptyLkup>();
            string sql = "select short_name from " + DBUtils.SCHEMA_NAME + "v_trade_data_cpty_lkup " +
                        " order by short_name asc";

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
                                result.Add(new BdtaCptyLkup
                                {
                                    //Two column names aren't supposed to match.
                                    CptySn = dataReader["short_name"].ToString()
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
