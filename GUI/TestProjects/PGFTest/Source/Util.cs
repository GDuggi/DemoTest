#define LOCAL_CONNECTION

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using DevExpress.Utils;
using DevExpress.XtraPivotGrid;

namespace NSRMCommon {
    /// <summary>Class containing shared-methods.</summary>
    public static class Util {
        #region constants
        public const string ITEM_SEPARATOR = " / ";
        public const string NULL_PERIOD = "-NULL-";
#   if LOCAL_CONNECTION
        public const string USER = null;
        public const string PASS = null;
        public const string DATABASE = "Test";
#if true
        public static readonly string SERVER = Dns.GetHostName();
        public const string INSTANCE = "SQLEXPRESS";
#else
        public const string SERVER = "localhost";
        public const string INSTANCE = "ProjectsV12";
#endif
        public const int PORT = 53798;
#   else
#   if DOMAIN_USER
        public const string USER = null;
        public const string PASS = null;
#   else
        public const string USER = "ictssrvr";
        public const string PASS = "ictssrvr";
#   endif
        public const string DATABASE = "DEV_riskmgr_trade";
#if false
        public const string SERVER = "HOUDB50";
#else
        public const string SERVER = "houdb30.tc.com";
#endif
        public const string INSTANCE = "sqlsvr10";
        public const int PORT = 1440;
#endif
        const string JDBC_URL_PREFIX = "jdbc:jtds:sqlserver://";
        #endregion

        #region methods

        #region logging-methods
        public static string makeSig(MethodBase mb) {
            return mb.ReflectedType.Name +
                ((ConstructorInfo.ConstructorName.CompareTo(mb.Name) == 0 ||
                ConstructorInfo.TypeConstructorName.CompareTo(mb.Name) == 0) ? string.Empty : ".") +
                mb.Name;

        }

        public static void show(MethodBase mb,string msg) {
            Debug.Print(makeSig(mb) + (string.IsNullOrEmpty(msg) ? string.Empty : (":" + msg)));
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
            Debug.Print(msg);
        }

        public static string makeErrorMessage(Exception ex) {
            return makeErrorMessage(ex,true);
        }
        #endregion

        /// <summary>Connection-string to be used for .Net database connection.</summary>
        /// <returns></returns>
        public static string dotNetConnectionString() { return dotNetConnectionString(null); }

        /// <summary>Connection-string to be used for .Net database connection.</summary>
        /// <param name="prevStr"></param>
        /// <returns></returns>
        public static string dotNetConnectionString(string prevStr) {
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(Util.USER) &&
                string.IsNullOrEmpty(Util.PASS))
                scsb.IntegratedSecurity = true;
            else {
                scsb.UserID = Util.USER;
                scsb.Password = Util.PASS;
            }
            if (!string.IsNullOrEmpty(Util.DATABASE))
                scsb.InitialCatalog = Util.DATABASE;
            sb.Append(Util.SERVER);
            if (!string.IsNullOrEmpty(Util.INSTANCE))
                sb.Append("\\" + Util.INSTANCE);
            if (Util.PORT > 0)
                sb.Append("," + Util.PORT);
            scsb.DataSource = sb.ToString();
            return scsb.ConnectionString;
        }

        public static string jdbcConnectionString() {

            string ret;
#if false
            ret = JDBC_URL_PREFIX + SERVER+":"+PORT+";databaseName=" + DATABASE + ";instance=" + INSTANCE + ";integrateSecurity=true";
#else
            StringBuilder sb = new StringBuilder();

            sb.Append(JDBC_URL_PREFIX);

            sb.Append(Util.SERVER);
            if (Util.PORT > 0)
                sb.Append(":" + Util.PORT);
            if (!string.IsNullOrEmpty(Util.INSTANCE))
                sb.Append(";instance=" + Util.INSTANCE);

            if (string.IsNullOrEmpty(Util.USER) &&
              string.IsNullOrEmpty(Util.PASS))
                sb.Append(";integratedSecurity=true");
            else {
                if (!string.IsNullOrEmpty(Util.USER))
                    sb.Append(";user=" + Util.USER);
                if (!string.IsNullOrEmpty(Util.PASS))
                    sb.Append(";password=" + Util.PASS);
            }
            if (!string.IsNullOrEmpty(Util.DATABASE))
                sb.Append(";databaseName=" + Util.DATABASE);
            ret = sb.ToString();
#endif
#if DEBUG
            Debug.Print("return java-connection-string: " + ret);
#endif
            return ret;
        }

        public const string COMPLEX_FRM = "CMPLX";

        public static string formatPeriod(string tradingPrd) {
            int ordinal;

            if (string.Compare(tradingPrd,COMPLEX_FRM) == 0)
                return COMPLEX_FRM;
            else if (Regex.IsMatch(tradingPrd,"^[0-9]{6}$")) {
                return DateTime.ParseExact(tradingPrd,"yyyyMM",null).ToString("MMM-yy");
            } else {
                if (Regex.IsMatch(tradingPrd,"^[0-9]{6}W[1-5]$")) {
                    DateTime dt = DateTime.ParseExact(tradingPrd.Substring(0,6),"yyyyMM",null);
                    return dt.ToString("MMM-yy") + " [Week " + tradingPrd[7] + "]";
                } else if (Regex.IsMatch(tradingPrd,"SPOT[0-9]{2}")) {
                    switch (ordinal = Convert.ToInt32(tradingPrd.Substring(4,2))) {
                        case 1: return ordinal + "st Nearby";
                        case 2: return ordinal + "nd Nearby";
                        case 3: return ordinal + "rd Nearby";
                        default: return ordinal + "th Nearby";
                    }
                }
            }
            return "*" + tradingPrd + "*";
        }

        /// <summary>Generate a string of formatted trading periods.</summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string catenatePeriods(string[] p) {
            return catenate(p,true);
        }

        /// <summary>Generate a string of items, optionally formatted as trading periods.</summary>
        /// <param name="p"></param>
        /// <param name="doPeriodFormat"></param>
        /// <returns></returns>
        public static string catenate(string[] p,bool doPeriodFormat) {
            StringBuilder sb = new StringBuilder();
            int nperiods = 0;

            foreach (string aPrd in p) {
                if (!string.IsNullOrEmpty(aPrd)) {
                    if (nperiods > 0)
                        sb.Append(Util.ITEM_SEPARATOR);

                    sb.Append(doPeriodFormat ? formatPeriod(aPrd) : aPrd);
                    nperiods++;
                }
            }
            return sb.ToString();
        }
        #endregion

        public static void setupPivotGrid(PivotGridControl pgc,Type type) {
            setupPivotGrid(pgc,type,false);
        }

        public static void setupPivotGrid(PivotGridControl pgc,Type type,bool addIfNotDefined) {
            pgc.OptionsBehavior.BestFitMode = PivotGridBestFitMode.FieldValue | PivotGridBestFitMode.FieldHeader | PivotGridBestFitMode.Cell;
            while (pgc.Fields.Count > 0)
                pgc.Fields.RemoveAt(0);
            //        addFieldsTo(pgc,findPivotFields(type,true));
        }

        static void addFieldsTo(PivotGridControl pgc,List<PivotGridField> list) {
            if (list != null)
                foreach (var avar in list) {
                    pgc.Fields.Add(avar);
                    //pgc.Fields.GetFieldByName(avar.Name).Visible = false;
                }
        }

        [Obsolete]
        public static void findPivotProperties(PivotGridControl pgc,Type t,Type tattr,bool addIfNotDefined) {
            object[] attrs;

            foreach (PropertyInfo pi in t.GetProperties())
                if ((attrs = pi.GetCustomAttributes(tattr,true)) != null &&
                    attrs.Length > 0)
                    addPivotField(pgc,pi.Name,attrs[0] as DesiredPivotGridFieldAttribute);
                else if (addIfNotDefined)
                    addPivotField(pgc,pi.Name,new DesiredPivotGridFieldAttribute(pi.Name,PivotGridAvail.Filter));
        }

        [Obsolete]
        public static void addPivotField(PivotGridControl pgc,string p,DesiredPivotGridFieldAttribute dpgfa) {
            PivotGridField f;

            if (!string.IsNullOrEmpty(dpgfa.Caption) && dpgfa.pivotGridAvailability != PivotGridAvail.NONE) {
                f = pgc.Fields.Add();
                f.Caption = dpgfa.Caption;
                f.FieldName = p;
                if (!string.IsNullOrEmpty(dpgfa.DisplayFolder))
                    f.DisplayFolder = dpgfa.DisplayFolder;
                else
                    f.DisplayFolder = "dummy";
                switch (dpgfa.pivotGridAvailability) {
                    case PivotGridAvail.Row: f.Area = PivotArea.RowArea; break;
                    case PivotGridAvail.Column: f.Area = PivotArea.ColumnArea; break;
                    case PivotGridAvail.Filter: f.Area = PivotArea.FilterArea; break;
                    case PivotGridAvail.Data: f.Area = PivotArea.DataArea; break;
                    default: throw new InvalidOperationException("unhandled " + dpgfa.pivotGridAvailability.GetType().FullName + ": " + dpgfa.pivotGridAvailability);
                }
                if (!string.IsNullOrEmpty(dpgfa.NumericFormat)) {
                    f.CellFormat.FormatType = dpgfa.IsDate ? FormatType.DateTime : FormatType.Numeric;
                    f.CellFormat.FormatString = dpgfa.NumericFormat;

                    f.ValueFormat.FormatType = f.CellFormat.FormatType;
                    f.ValueFormat.FormatString = f.CellFormat.FormatString;
                }
            }
        }

        public static List<PivotGridField> findPivotFields(Type t) {
            return findPivotFields(t,true);
        }

        public static List<PivotGridField> findPivotFields(Type t,bool addIfNotDefined) {
            object[] attrs;
            List<PivotGridField> ret = new List<PivotGridField>();
            PivotGridField pgf = null;
            DesiredPivotGridFieldAttribute defAttr = new DesiredPivotGridFieldAttribute(string.Empty,PivotGridAvail.Filter);
            Type tattr = typeof(DesiredPivotGridFieldAttribute);

            foreach (PropertyInfo pi in t.GetProperties()) {
                if ((attrs = pi.GetCustomAttributes(tattr,true)) != null &&
                    attrs.Length > 0)
                    pgf = addPivotField(pi.Name,attrs[0] as DesiredPivotGridFieldAttribute);
                //                    addPivotField(pgc,pi.Name,attrs[0] as DesiredPivotGridFieldAttribute);
                else if (addIfNotDefined) {
                    defAttr.Caption = pi.Name;
                    pgf = addPivotField(pi.Name,defAttr);
                }
                //                    addPivotField(pgc,pi.Name,defAttr);
                if (pgf != null)
                    ret.Add(pgf);
            }
            return ret;
        }

        public static PivotGridField addPivotField(string p,DesiredPivotGridFieldAttribute dpgfa) {
            PivotGridField f = null;
            PivotArea pa;

            if (string.IsNullOrEmpty(dpgfa.Caption))
                throw new ArgumentNullException("dpgfa.Caption","caption is null!");
            if (dpgfa.pivotGridAvailability == PivotGridAvail.NONE)
                throw new InvalidOperationException("PivotGrid availability is not defined!");

            pa = PivotArea.ColumnArea;
            switch (dpgfa.pivotGridAvailability) {
                case PivotGridAvail.Row: pa = PivotArea.RowArea; break;
                case PivotGridAvail.Column: pa = PivotArea.ColumnArea; break;
                case PivotGridAvail.Filter: pa = PivotArea.FilterArea; break;
                case PivotGridAvail.Data: pa = PivotArea.DataArea; break;
                default: throw new InvalidOperationException("unhandled " + dpgfa.pivotGridAvailability.GetType().FullName + ": " + dpgfa.pivotGridAvailability);
            }
            f = new PivotGridField(p,pa);
            f.Caption = dpgfa.Caption;
            f.Name = "pgf" + dpgfa.Caption;

            if (!string.IsNullOrEmpty(dpgfa.DisplayFolder))
                f.DisplayFolder = dpgfa.DisplayFolder;
            else
                f.DisplayFolder = "dummy";
            if (!string.IsNullOrEmpty(dpgfa.NumericFormat)) {
                f.CellFormat.FormatType = dpgfa.IsDate ? FormatType.DateTime : FormatType.Numeric;
                f.CellFormat.FormatString = dpgfa.NumericFormat;

                f.ValueFormat.FormatType = f.CellFormat.FormatType;
                f.ValueFormat.FormatString = f.CellFormat.FormatString;
            }
            return f;
        }

        public static void findSubclassesOf(Type type2) {
            foreach (var avar in AppDomain.CurrentDomain.GetAssemblies())
                foreach (var avar2 in avar.GetExportedTypes())
                    if (typeImplements(avar2,type2))
                        Debug.Print("found: " + avar2.FullName);
        }

        static bool typeImplements(Type srcType,Type itype) {
            return srcType.GetInterface(itype.FullName) != null;
        }
    }
}