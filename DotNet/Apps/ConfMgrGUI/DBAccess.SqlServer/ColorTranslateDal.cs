using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DBAccess.SqlServer
{
    public class ColorTranslateDal : IColorTranslateDal
    {
        private string sqlConnStr = "";

        public ColorTranslateDal(string pSqlConn)
        {
            sqlConnStr = pSqlConn;
        }

        public List<ColorTranslate> GetAllStub()
        {
            var result = new List<ColorTranslate>();
            result.Add(new ColorTranslate { Code = "PURPLE", CsColor = "SkyBlue" });
            result.Add(new ColorTranslate { Code = "GRAY",   CsColor = "LightGray" });
            result.Add(new ColorTranslate { Code = "RED",    CsColor = "Tomato" });
            result.Add(new ColorTranslate { Code = "YELLOW", CsColor = "Gold" });
            result.Add(new ColorTranslate { Code = "ORANGE", CsColor = "SandyBrown" });
            result.Add(new ColorTranslate { Code = "GREEN",  CsColor = "LightGreen" });
            return result;
        }

        public List<ColorTranslate> GetAll()
        {
            var result = new List<ColorTranslate>();
            string sql = "select code, cs_color " +
                         "from " + DBUtils.SCHEMA_NAME + "color_translate ";

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
                                result.Add(new ColorTranslate
                                {
                                    Code = dataReader["code"].ToString(),
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
