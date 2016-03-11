using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class RqmtStatusColorsDal : IRqmtStatusColorsDal
    {
        private string sqlConnStr = "";

        public RqmtStatusColorsDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        public List<RqmtStatusColor> GetAllStub()
        {
            var result = new List<RqmtStatusColor>();
            result.Add(new RqmtStatusColor { Hashkey = "NOCNFOPEN",    CsColor = "SkyBlue"});
            result.Add(new RqmtStatusColor { Hashkey = "NOCNFCXL",     CsColor = "LightGray" });
            result.Add(new RqmtStatusColor { Hashkey = "NOCNFAPPR",    CsColor = "LightGreen" });
            result.Add(new RqmtStatusColor { Hashkey = "XQCSPACPTD",   CsColor = "LightGreen" });
            result.Add(new RqmtStatusColor { Hashkey = "XQCSPAPPR",    CsColor = "LightGreen" });
            result.Add(new RqmtStatusColor { Hashkey = "XQCSPAUTO",    CsColor = "Gold" });
            result.Add(new RqmtStatusColor { Hashkey = "XQCSPCPRCV",   CsColor = "LightGreen" });
            result.Add(new RqmtStatusColor { Hashkey = "XQCSPEXT_REVIEW",    CsColor = "Gold" });
            result.Add(new RqmtStatusColor { Hashkey = "XQCSPCXL",     CsColor = "LightGray" });
            result.Add(new RqmtStatusColor { Hashkey = "XQCSPDISP",    CsColor = "Tomato" });
            result.Add(new RqmtStatusColor { Hashkey = "XQCSPSIGNED", CsColor = "LightGreen" });
            result.Add(new RqmtStatusColor { Hashkey = "XQCSPFAIL",    CsColor = "Tomato" });
            result.Add(new RqmtStatusColor { Hashkey = "XQCSPMGR",     CsColor = "Gold" });
            result.Add(new RqmtStatusColor { Hashkey = "XQCSPNEW",     CsColor = "SkyBlue" });
            result.Add(new RqmtStatusColor { Hashkey = "XQCSPOK_TO_SEND", CsColor = "Gold" });
            result.Add(new RqmtStatusColor { Hashkey = "XQCSPPREP",    CsColor = "SandyBrown" });
            result.Add(new RqmtStatusColor { Hashkey = "XQCSPPSTVL",   CsColor = "LightGreen" });
            result.Add(new RqmtStatusColor { Hashkey = "XQCSPRESNT",   CsColor = "Tomato" });
            result.Add(new RqmtStatusColor { Hashkey = "XQCSPSENT",    CsColor = "Gold" });
            result.Add(new RqmtStatusColor { Hashkey = "XQCSPTRADER",  CsColor = "Gold" });
            result.Add(new RqmtStatusColor { Hashkey = "XQCSPVERBL",   CsColor = "LightGreen" });
            return result;
        }

        public List<RqmtStatusColor> GetAll()
        {
            var result = new List<RqmtStatusColor>();
            string sql = "select hashkey, cs_color " +
                         "from " + DBUtils.SCHEMA_NAME + "v_rqmt_status_colors ";

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
                                result.Add(new RqmtStatusColor
                                {
                                    Hashkey = dataReader["hashkey"].ToString(),
                                    CsColor = dataReader["cs_color"].ToString()
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
