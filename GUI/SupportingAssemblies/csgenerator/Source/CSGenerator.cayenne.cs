using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

namespace NSCSGen {
    public partial class CSGenerator {
        public static void generateCayenneEntries(SqlConnection connection,string[] tableNames) {
            StringBuilder sb;
            XmlWriterSettings xws = new XmlWriterSettings();
            string outPath,outFile;

            sb = new StringBuilder();
            xws.Indent = true;
            xws.IndentChars = new string(' ',4);
            xws.Encoding = ASCIIEncoding.ASCII;
            xws.OmitXmlDeclaration = true;
            using (XmlWriter xw = XmlWriter.Create(sb,xws)) {
                writeXmlTo(connection,tableNames,xw);
            }
            Debug.Print("\r\n" + sb.ToString() + "\r\n");
            outPath = Path.GetFullPath(
                    Path.Combine(
                        Directory.GetCurrentDirectory(),".."));
            outFile = Path.Combine(outPath,"CayenneFields.xml");
            if (File.Exists(outFile))
                File.Delete(outFile);
            using (XmlWriter xw = XmlWriter.Create(outFile,xws)) {
                writeXmlTo(connection,tableNames,xw);
            }
        }
        static void writeXmlTo(SqlConnection connection,string[] tableNames,XmlWriter xw) {
            xw.WriteStartDocument(true);
            xw.WriteStartElement("data-map");
            generateCayenneEntries(connection,tableNames,xw);
            xw.WriteEndElement();
            xw.WriteEndDocument();
        }
        static void generateCayenneEntries(SqlConnection connection,string[] tableNames,XmlWriter xw) {
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
                        generateCayenneEntry(connection,cmd,aTable,xw);
                }
        }

        static void generateCayenneEntry(SqlConnection connection,SqlCommand cmd,string aTable,XmlWriter xw) {
            cmd.CommandText = "SELECT sc.name,length,colorder,sc.status, v.name,sc.isnullable " +
                "FROM syscolumns sc, master.dbo.spt_values v " +
                "WHERE id=object_id('" + aTable + "') AND v.type='J' AND sc.type=v.low ORDER BY colorder";
            try {
                using (SqlDataReader idr = cmd.ExecuteReader()) {
                    writeCayenneElement(xw,idr,aTable,"db-entity");
                }
            } catch (Exception ex) {
                Debug.Print("[" + ex.GetType().FullName + "] " + ex.Message);
                xw.WriteComment(" [" + ex.GetType().FullName + "] " + ex.Message + " ");
            }
        }

        static void writeCayenneElement(XmlWriter xw,SqlDataReader idr,string aTable,string p) {
            xw.WriteStartElement(p);
            xw.WriteAttributeString("name",aTable);
            readAttributes2(idr,xw);
            xw.WriteEndElement();
        }

        static void readAttributes2(SqlDataReader idr,XmlWriter xw) {
            string colName,typeName,cayenneDatatype;
            int len,isNull;
            short colLen,colOrder;
            byte colStatus;
            System.Collections.Generic.List<string> unknownTypes = new System.Collections.Generic.List<string>();

            while (idr.Read()) {
                colName = idr.GetString(0);
                colLen = idr.GetInt16(1);
                colOrder = idr.GetInt16(2);
                colStatus = idr.GetByte(3);
                typeName = idr.GetString(4);
                isNull = idr.GetInt32(5);

                cayenneDatatype = "VARCHAR";
                len = -1;

                if (string.Compare(typeName,"char",true) == 0) {
                    cayenneDatatype = "CHAR";
                    len = colLen;
                } else if (string.Compare(typeName,"varchar",true) == 0) {
                    cayenneDatatype = "VARCHAR";
                    len = colLen;
                } else if (string.Compare(typeName,"int",true) == 0 || string.Compare(typeName,"intn",true) == 0) {
                    cayenneDatatype = "INTEGER";
                } else if (string.Compare(typeName,"float",true) == 0 || string.Compare(typeName,"floatn",true) == 0) {
                    cayenneDatatype = "FLOAT";
                } else {
                    if (!unknownTypes.Contains(typeName))
                        unknownTypes.Add(typeName);
                    xw.WriteComment(" unknown type: " + typeName + " ");
                    //                    xw.WriteAttributeString("type","VARCHAR");
                }
                writeDBAttribute(xw,colName,isNull,colStatus,cayenneDatatype,len);
            }

            if (unknownTypes.Count > 0) {
                StringBuilder sb = new StringBuilder();
                System.Reflection.MethodBase mb = System.Reflection.MethodBase.GetCurrentMethod();

                sb.AppendLine("[" + mb.ReflectedType.Name + "." + mb.Name + "] unknown types:");
                if (unknownTypes.Count > 1)
                    unknownTypes.Sort();
                foreach (string aType in unknownTypes)
                    sb.AppendLine("\t\t" + aType);
                sb.AppendLine();
                Debug.Print(sb.ToString());
            }
        }

          static void writeDBAttribute(XmlWriter xw,string colName,int isNull,byte colStatus,string cayenneDatatype,int len) {

            //        if ()
            xw.WriteStartElement("db-attribute");
            xw.WriteAttributeString("name",colName);
            xw.WriteAttributeString("type",cayenneDatatype);
            if (len > 0)
                xw.WriteAttributeString("length",len.ToString());
            if (colStatus > 0)
                xw.WriteAttributeString("isPrimaryKey","true");
            if (colStatus > 0 && isNull == 0)
                xw.WriteAttributeString("isMandatory","true");

            xw.WriteEndElement();
        }
    }
}