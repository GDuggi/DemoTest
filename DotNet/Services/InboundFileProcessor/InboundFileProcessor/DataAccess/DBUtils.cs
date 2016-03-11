using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace InboundFileProcessor
{
    public class DBUtils
    {
        public static readonly string DATE_FORMAT = "yyyy-MM-dd";
        public static readonly string DATETIME_FORMAT = "yyyy-MM-dd HH:mm";


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

        public static float HandleFloatIfNull(string pVal)
        {
            float result = 0;
            if (pVal == null || pVal.Length == 0)
                return result;
            else
                result = float.Parse(pVal);
            return result;
        }

    }
}
