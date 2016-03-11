using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace DBAccess.SqlServer
{
    public class DBUtils
    {
        public static readonly string SCHEMA_NAME = "ConfirmMgr.";
        public static readonly string DATE_FORMAT = "yyyy-MM-dd";
        public static readonly string DATETIME_FORMAT = "yyyy-MM-dd HH:mm";

        public static Int32 GetNextSequence(string pSqlConnStr, string pSeqName)
        {
            Int32 result = 0;
            string sql = "SELECT NEXT VALUE FOR " + SCHEMA_NAME + pSeqName;

            using (SqlConnection conn = new SqlConnection(pSqlConnStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = System.Data.CommandType.Text;
                    conn.Open();
                    var seqNo = cmd.ExecuteScalar();
                    result = Convert.ToInt32(seqNo);
                }
            }
            return result;            
        }

        public static Image ConvertByteStreamToImage(byte[] pByteStream)
        {
            Image imgResult;
            if (pByteStream == null)
                return null;
            else
            {
                using (var memStream = new MemoryStream(pByteStream))
                {
                    imgResult = Image.FromStream(memStream);
                    return imgResult;
                }
            }
        }

        public byte[] ConvertImageToByteArray(Image pImage, ImageFormat pImageFormat)
        {
            byte[] byteArrayResult;
            using (var memStream = new MemoryStream())
            {
                pImage.Save(memStream, pImageFormat);
                byteArrayResult = memStream.ToArray();
                return byteArrayResult;
            }
        }

        //Used for command input values
        //(Overloading these two with the same name was sometimes causing invalid parameter type(!) errors so changed name.
        public static object ValueStringOrDBNull(string pVal)
        {
            if (pVal == null) return DBNull.Value;
            return pVal;
        }

        public static object ValueDateTimeOrDbNull(DateTime pVal)
        {
            if (pVal == null) return DBNull.Value;
            return pVal;
        }

        //All following are used for results
        public static DateTime HandleDateTimeIfNull(string pVal)
        {
            DateTime result = new DateTime();
            if (pVal == null || pVal.Length == 0)
                return result;
            else
                result = Convert.ToDateTime(pVal);
            return result;
        }

        public static Int16 HandleInt16IfNull(string pVal)
        {
            Int16 result = 0;
            if (pVal == null || pVal.Length == 0)
                return result;
            else
                result = Convert.ToInt16(pVal);
            return result;
        }

        public static Int32 HandleInt32IfNull(string pVal)
        {
            Int32 result = 0;
            if (pVal == null || pVal.Length == 0)
                return result;
            else
                result = Convert.ToInt32(pVal);
            return result;
        }

        public static Int64 HandleInt64IfNull(string pVal)
        {
            Int64 result = 0;
            if (pVal == null || pVal.Length == 0)
                return result;
            else
                result = Convert.ToInt64(pVal);
            return result;
        }

        public static long HandleLongIfNull(string pVal)
        {
            return string.IsNullOrEmpty(pVal) ? 0 : Convert.ToInt64(pVal);
        }

        public static float HandleFloatIfNull(string pVal)
        {
            float result = 0;
            if (pVal == null || pVal.Length == 0)
                return result;
            else
                result = float.Parse(pVal);
            return result;
        }

        public static string GetAllSqlParamStr(string pTrdSysCode, string pSeCptySn, string pCptySn, string pCdtyCode,
            DateTime pBeginTradeDt, DateTime pEndTradeDt, string pTrdSysTicket)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            string sql = DBUtils.SCHEMA_NAME + "trade t, " +
                         DBUtils.SCHEMA_NAME + "trade_data d " +
                        " where v.trade_id = t.trade_id " +
                        " and v.trade_id = t.id ";

            sqlBuilder.Append(sql);

            if (!String.IsNullOrEmpty(pTrdSysTicket))
            {
                sql = " and t.trd_sys_ticket = '" + pTrdSysTicket + "' ";
                sqlBuilder.Append(sql);
            }
            else
            {
                if (!String.IsNullOrEmpty(pTrdSysCode))
                {
                    sql = " and d.trd_sys_code = '" + pTrdSysCode + "' ";
                    sqlBuilder.Append(sql);
                }
                if (!String.IsNullOrEmpty(pSeCptySn))
                {
                    sql = " and d.se_cpty_sn = '" + pSeCptySn + "' ";
                    sqlBuilder.Append(sql);
                }
                if (!String.IsNullOrEmpty(pCptySn))
                {
                    sql = " and d.cpty_sn = '" + pCptySn + "' ";
                    sqlBuilder.Append(sql);
                }
                if (!String.IsNullOrEmpty(pCdtyCode))
                {
                    sql = " and d.cdty_code = '" + pCdtyCode + "' ";
                    sqlBuilder.Append(sql);
                }
                if (pBeginTradeDt > DateTime.MinValue && pEndTradeDt > DateTime.MinValue)
                {
                    string beginDtStr = pBeginTradeDt.ToString("yyyy-MM-dd");
                    string endDtStr = pEndTradeDt.ToString("yyyy-MM-dd");

                    sql = " and d.trade_dt >= '" + beginDtStr + "' and " +
                         " and d.trade_dt <= '" + endDtStr + "' ";
                    sqlBuilder.Append(sql);
                }
            }

            return sqlBuilder.ToString();
        }

        //public static string getUrlStr(string pUrl, string pMethod, string[] pParmList)
        //{
        //    string resultStr = pUrl;
        //    resultStr += !pMethod.EndsWith("/") ? "/" + pMethod : pMethod;

        //    if (pParmList != null)
        //        foreach (string parm in pParmList)
        //        {
        //            if (!resultStr.EndsWith("/"))
        //                resultStr += "/";

        //            resultStr += parm;
        //        }

        //    return resultStr;
        //}

        //public static string getWebServiceUrlResult(string pUrl)
        //{
        //    HttpClient client = new HttpClient();
        //    HttpResponseMessage wcfResponse = client.GetAsync(pUrl).Result;
        //    HttpContent stream = wcfResponse.Content;
        //    var data = stream.ReadAsStringAsync();
        //    string xmlText = data.Result;

        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(xmlText);
        //    XmlNodeList nodes = doc.GetElementsByTagName("string");
        //    string innerXmlText = nodes[0].InnerText;
        //    return innerXmlText;
        //}


    }
}
