using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VaultViewer
{
    public static class VVUtils
    {
        public static bool IsNumericPropertyType(string pPropTypeName)
        {
            switch (pPropTypeName)
            {
                case "Byte":
                case "SByte":
                case "UInt16":
                case "UInt32":
                case "UInt64":
                case "Int16":
                case "Int32":
                case "Int64":
                case "Decimal":
                case "Double":
                case "Single":
                    return true;
                default:
                    return false;
            }
        }
        
        public static bool IsNumericType(this object pObj)
        {
            switch (Type.GetTypeCode(pObj.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
}
