using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
namespace NSCSGen {
    public partial class CSGenerator {
        public static void generateClasses(SqlConnection connection,string[] tableNames) {
            generateClasses(connection,tableNames,null);
        }

        public static void generateClasses(SqlConnection connection,string[] tableNames,string aNamespace) {
            if (connection == null)
                throw new ArgumentNullException();
            if (connection.State != ConnectionState.Open)
                throw new InvalidOperationException("ugh");
            if (tableNames == null)
                throw new ArgumentNullException("null tables");
            if (tableNames.Length > 0)
                using (SqlCommand cmd = new SqlCommand()) {
                    cmd.Connection = connection;
                    foreach (string aTable in tableNames)
                        generateClass(connection,cmd,aTable,aNamespace);
                }
        }

        public static void generateClass(SqlConnection connection,SqlCommand cmd,string aTable,string aNamespace) {
            if (cmd == null) {
                using (SqlCommand cmd2 = new SqlCommand()) {
                    cmd2.Connection = connection;
                    generateSingleClass(connection,cmd2,aTable,aNamespace);
                }
            } else {
                generateSingleClass(connection,cmd,aTable,aNamespace);
            }
        }

        static void generateSingleClass(SqlConnection connection,SqlCommand cmd,string aTable,string aNamespace) {
            cmd.CommandText = "SELECT * FROM " + aTable + " WHERE 1=0";
            try {
                using (IDataReader idr = cmd.ExecuteReader()) {
                    generateCode(Directory.GetCurrentDirectory(),makeClassName(aTable),idr,aNamespace);
                }
            } catch (Exception ex) {
                Debug.Print("[" + ex.GetType().FullName + "] " + ex.Message);
            }
        }
    }
}