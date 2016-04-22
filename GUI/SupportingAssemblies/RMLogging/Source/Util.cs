//#define LOCAL_CONNECTION

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace NSRMLogging {
    /// <summary>Class containing shared-methods.</summary>
    public static class Util {
        #region constants
        public const string ITEM_SEPARATOR = " / ";
        #endregion constants

        #region fields
        public static bool useParameters = true;
        #endregion fields

        #region methods

        #region logging-methods
        public static string makeSig(MethodBase mb) {
            return mb.ReflectedType.Name +
                ((ConstructorInfo.ConstructorName.CompareTo(mb.Name) == 0 ||
                ConstructorInfo.TypeConstructorName.CompareTo(mb.Name) == 0) ? string.Empty : ".") +
                mb.Name + generateParameterList(mb);

        }

        static readonly IDictionary<MethodBase,string> sigMap = new Dictionary<MethodBase,string>();

        static readonly object mapLock = new object();
        static string generateParameterList(MethodBase mb) {
            ParameterInfo[] parms;
            StringBuilder sb;
            string content = "()";
            int i = 0;

            lock (mapLock) {
                if (sigMap.ContainsKey(mb))
                    return sigMap[mb];
            }
            if (useParameters && (parms = mb.GetParameters()) != null && parms.Length > 0) {
                sb = new StringBuilder();
                foreach (ParameterInfo pi in parms) {
                    if (i > 0)
                        sb.Append(",");
                    sb.Append(pi.ParameterType.FullName + " " + pi.Name);
                    i++;
                }
                content = "(" + sb.ToString() + ")";
            }
            lock (mapLock) {
                if (!sigMap.ContainsKey(mb))
                    sigMap.Add(mb,content);
            }
            return content;
        }

        public static void show(MethodBase mb,string msg) {
            Trace.WriteLine(makeSig(mb) + (string.IsNullOrEmpty(msg) ? string.Empty : (":" + msg)));
        }

        public static void show(MethodBase mb,Exception ex) {
            StringBuilder sb = new StringBuilder();

            show(mb,makeErrorMessage(ex,true));
        }

        public static string makeErrorMessage(Exception ex,bool p) {
            Exception anException;
            StringBuilder sb = new StringBuilder();

            anException = ex;
            while (anException != null) {
                sb.AppendLine("[" + ex.GetType().Name + "] " + anException.Message);
                anException = anException.InnerException;

            }
            return sb.ToString() + (p ? ex.StackTrace : string.Empty) + "\r\n";
        }

        public static void show(MethodBase mb) {
            show(makeSig(mb));
        }

        public static void show(string msg) {
            Trace.WriteLine(msg);
        }

        public const string FIELD_SEPARATOR = ";";
//        public static string CONTENT_TYPE;
        public const string CONTENT_TYPE = "application/json";

        public static string makeErrorMessage(Exception ex) {
            return makeErrorMessage(ex,true);
        }

        public static int[] makeIntVector(string tmp2) {
            List<int> tmp = new List<int>();
            int aNewInt;

            if (!string.IsNullOrEmpty(tmp2))
                foreach (string anInt in tmp2.Split(FIELD_SEPARATOR[0],','))
                    if (Int32.TryParse(anInt,out aNewInt))
                        tmp.Add(aNewInt);
            return tmp.ToArray();
        }

        #endregion

        #endregion
    }
}