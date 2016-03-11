using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data.OracleClient;
using System.Data;
using System.Xml;
using System.IO;
using System.Configuration;

namespace InboundDocuments
{
    public class FilterUtil
    {

        public static Hashtable GetUserFilters()
        {
            string fileName = (ConfigurationManager.AppSettings["Grid.Layout.Settings.Path"] + "AutoFilter.xml");
            Hashtable hs = null;

            if (File.Exists(fileName)) {
                hs = new Hashtable();
                XmlDocument document = new XmlDocument();
                document.Load(fileName);
                XmlNodeList nodeList = document.GetElementsByTagName("Code");
                ArrayList codeList = new ArrayList();
                for (int i = 0; i < nodeList.Count; i++)
                {
                    XmlNode node = nodeList[i];
                    codeList.Add(node.InnerText);
                }
                hs["Commodity"] = codeList;
                Hashtable condition = new Hashtable();
                nodeList = document.GetElementsByTagName("AppliesTo");
                condition["AppliesTo"] = "Short Code";
                if (nodeList != null && nodeList.Count > 0)
                {
                    string appliesTo = nodeList[0].InnerText;
                    if (appliesTo != null && !"".Equals(appliesTo))
                    {
                        condition["AppliesTo"] = appliesTo;
                    }
                }
                
                nodeList = document.GetElementsByTagName("Condition");
                for (int i = 0; i < nodeList.Count; i++)
                {
                    XmlNode node = nodeList[i];
                    string key = node.SelectSingleNode("Name").InnerText;
                    string value = node.SelectSingleNode("Value").InnerText;
                    condition[key] = value;
                }
                hs["Counterparty"] = condition;
                
            }
            return hs;

        }

        public static DataSet GetCommoditListFromDb()
        {
            string connectionString = GetDbConnectionString();
            string sql = "Select Code,Short_Name,Descr From infinity_mgr.cdty_grp where Active_Flag = 'Y' Order By Code";

            if (connectionString == null)
            {
                throw new Exception("Oracle.ConnectionString is not configured.");
            }
            OracleDataAdapter adapter = new OracleDataAdapter(sql,connectionString);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            return ds;

        }
        
        private static string GetDbConnectionString()
        {
            string connectString = System.Configuration.ConfigurationManager.AppSettings["Oracle.ConnectionString"];
            return connectString;
        }
        public static void SaveUserFilters(Hashtable filters){

            string fileName = (ConfigurationManager.AppSettings["Grid.Layout.Settings.Path"] + "AutoFilter.xml");
            StreamWriter stream = new StreamWriter(fileName);
            XmlTextWriter writer = new XmlTextWriter(stream);
            writer.WriteStartElement("Filter");
            writer.WriteStartElement("Commodity");
            ArrayList commodityList = (ArrayList) filters["Commodity"];
            if (commodityList != null)
            {
                int len = commodityList.Count;
                for (int i = 0; i < len; ++i)
                {
                    string code = (string)commodityList[i];
                    writer.WriteElementString("Code", code);
                    stream.Write('\n');
                }
            }
            writer.WriteEndElement();
            stream.Write('\n');

            Hashtable hs = (Hashtable)filters["Counterparty"];
            writer.WriteStartElement("Counterparty");
            writer.WriteStartElement("AppliesTo");
            if (hs != null) 
            {
                string appliesTo = (string)hs["AppliesTo"];
                if (appliesTo != null)
                {
                    writer.WriteString(appliesTo);
                }
            }    
            writer.WriteEndElement();
            stream.Write('\n');
            writer.WriteStartElement("Conditions");
            if (hs != null)
            {
                IDictionaryEnumerator iterator = hs.GetEnumerator();
                while (iterator.MoveNext())
                {
                    
                    string key = (string)iterator.Key;
                    if ("AppliesTo".Equals(key ,StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }
                    string value = (string) iterator.Value;
                    writer.WriteStartElement("Condition");
                    writer.WriteElementString("Name", key);
                    writer.WriteElementString("Value", value);
                    writer.WriteEndElement();
                    stream.Write('\n');

                }
            }
            writer.WriteEndElement();
            stream.Write('\n');
            writer.WriteEndElement();
            stream.Write('\n');
            writer.WriteEndElement();
            stream.Write('\n');
            writer.Close();
            stream.Close();
            
        }
    }
}
