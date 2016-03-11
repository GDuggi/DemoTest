using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;
using System.IO;
using System.Configuration;
using System.Data;

namespace ConfirmInbound
{
    public class FilterUtil
    {

        public static Hashtable GetUserFilters()
        {
            //string fileName = HomeDrive + (InboundPnl.settingsReader.GetSettingValue("InboundUserSettings") + "AutoFilter.xml");
            string fileName = Path.Combine(InboundPnl.appSettingsDir, "AutoFilter.xml");
            Hashtable hs = null;

            if (File.Exists(fileName))
            {
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

                nodeList = document.GetElementsByTagName("IgnoredDocsFilter");
                ArrayList filterList = new ArrayList();
                for (int i = 0; i < nodeList.Count; i++)
                {
                    XmlNode node = nodeList[i];
                    filterList.Add(node.InnerText);
                }
                hs["Filter"] = filterList;

                nodeList = document.GetElementsByTagName("FaxNo");
                ArrayList faxNoList = new ArrayList();
                for (int i = 0; i < nodeList.Count; i++)
                {
                    XmlNode node = nodeList[i];
                    faxNoList.Add(node.InnerText);
                }
                hs["FaxNos"] = faxNoList;


                nodeList = document.GetElementsByTagName("CptyCode");
                ArrayList cptyAddList = new ArrayList();
                for (int i = 0; i < nodeList.Count; i++)
                {
                    XmlNode node = nodeList[i];
                    cptyAddList.Add(node.InnerText);
                }
                hs["CptyAdditional"] = cptyAddList;

                nodeList = document.GetElementsByTagName("BrkrCode");
                ArrayList brkrAddList = new ArrayList();
                for (int i = 0; i < nodeList.Count; i++)
                {
                    XmlNode node = nodeList[i];
                    brkrAddList.Add(node.InnerText);
                }
                hs["BrkrAdditional"] = brkrAddList;

                ArrayList panelViews = new ArrayList();

                if (document.GetElementsByTagName("Panel1").Count != 0)
                {
                    panelViews.Add((document.GetElementsByTagName("Panel1"))[0].InnerText);
                    panelViews.Add((document.GetElementsByTagName("Panel2"))[0].InnerText);
                    panelViews.Add((document.GetElementsByTagName("Panel3"))[0].InnerText);
                    panelViews.Add((document.GetElementsByTagName("Panel4"))[0].InnerText);
                    hs["PanelViews"] = panelViews;
                }

                nodeList = document.GetElementsByTagName("CCYCode");
                ArrayList crncyList = new ArrayList();
                for (int i = 0; i < nodeList.Count; i++)
                {
                    XmlNode node = nodeList[i];
                    crncyList.Add(node.InnerText);
                }
                hs["Currency"] = crncyList;

                nodeList = document.GetElementsByTagName("Company");
                ArrayList companyList = new ArrayList();
                for (int i = 0; i < nodeList.Count; i++)
                {
                    XmlNode node = nodeList[i];
                    companyList.Add(node.InnerText);
                }
                hs["BookingCompany"] = companyList;


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


                Hashtable brkrCondition = new Hashtable();
                nodeList = document.GetElementsByTagName("BrkrAppliesTo");
                brkrCondition["BrkrAppliesTo"] = "Short Code";
                if (nodeList != null && nodeList.Count > 0)
                {
                    string appliesTo = nodeList[0].InnerText;
                    if (appliesTo != null && !"".Equals(appliesTo))
                    {
                        brkrCondition["BrkrAppliesTo"] = appliesTo;
                    }
                }

                nodeList = document.GetElementsByTagName("BrkrCondition");
                for (int i = 0; i < nodeList.Count; i++)
                {
                    XmlNode node = nodeList[i];
                    string key = node.SelectSingleNode("Name").InnerText;
                    string value = node.SelectSingleNode("Value").InnerText;
                    brkrCondition[key] = value;
                }
                hs["Broker"] = brkrCondition;
            }
            return hs;
        }

        public static string HomeDrive
        {
            get
            {
                return Environment.GetEnvironmentVariable("HOMEDRIVE").ToString();
            }
        }

        public static DataSet GetCommoditListFromDb()
        {
            return null;
        }

        public static DataSet GetFaxNOsFromDb()
        {
            return null;
        }

        public static void SaveUserFilters(Hashtable filters)
        {
            //string fileName = HomeDrive + InboundPnl.settingsReader.GetSettingValue("InboundUserSettings") + "AutoFilter.xml";
            string fileName = Path.Combine(InboundPnl.appSettingsDir, "AutoFilter.xml");

            //Israel 2/24/14 -- No longer necessary since making the Inbound settings directory the same as regular OpsTracking.
            //if (!Directory.Exists(InboundPnl.appSettingsDir))
            //{
            //    Directory.CreateDirectory(InboundPnl.appSettingsDir);
            //}
            if (File.Exists(fileName))
                File.Delete(fileName);
            StreamWriter stream = new StreamWriter(fileName);
            XmlTextWriter writer = new XmlTextWriter(stream);
            ArrayList commodityList = (ArrayList)filters["Commodity"];
            ArrayList faxNoList = (ArrayList)filters["FaxNos"];
            ArrayList ignoreFilterList = (ArrayList)filters["Filters"];
            ArrayList currencyList = (ArrayList)filters["Currency"];
            ArrayList companyList = (ArrayList)filters["BookingCompany"];
            ArrayList addCptyList = (ArrayList)filters["CptyInclude"];
            ArrayList panelViewList = (ArrayList)filters["PanelViews"];
            ArrayList addBrkrList = (ArrayList)filters["BrkrInclude"];


            writer.WriteStartElement("Filter");
            writer.WriteStartElement("Commodity");
            if (commodityList != null)
            {
                string code = "";
                int len = commodityList.Count;
                for (int i = 0; i < len; ++i)
                {
                    code = (string)commodityList[i];
                    writer.WriteElementString("Code", code);
                    stream.Write('\n');
                }
            }
            writer.WriteEndElement();
            stream.Write('\n');




            writer.WriteStartElement("IgnoredFilter");
            if (ignoreFilterList != null)
            {
                string ignoredFilter = "";
                int len = ignoreFilterList.Count;
                for (int i = 0; i < len; ++i)
                {
                    ignoredFilter = (string)ignoreFilterList[i];
                    writer.WriteElementString("IgnoredDocsFilter", ignoredFilter);
                    stream.Write('\n');
                }
            }
            writer.WriteEndElement();
            stream.Write('\n');



            writer.WriteStartElement("FaxNos");
            if (faxNoList != null)
            {
                string faxNo = "";
                int len = faxNoList.Count;
                for (int i = 0; i < len; ++i)
                {
                    faxNo = (string)faxNoList[i];
                    writer.WriteElementString("FaxNo", faxNo);
                    stream.Write('\n');
                }
            }
            writer.WriteEndElement();
            stream.Write('\n');


            writer.WriteStartElement("CptyInclude");
            if (addCptyList != null)
            {
                string code = "";
                int len = addCptyList.Count;
                for (int i = 0; i < len; ++i)
                {
                    code = (string)addCptyList[i];
                    writer.WriteElementString("CptyCode", code);
                    stream.Write('\n');
                }
            }
            writer.WriteEndElement();
            stream.Write('\n');

            writer.WriteStartElement("BrkrInclude");
            if (addBrkrList != null)
            {
                string code = "";
                int len = addBrkrList.Count;
                for (int i = 0; i < len; ++i)
                {
                    code = (string)addBrkrList[i];
                    writer.WriteElementString("BrkrCode", code);
                    stream.Write('\n');
                }
            }
            writer.WriteEndElement();
            stream.Write('\n');


            
            
            
            writer.WriteStartElement("Panels");
            stream.Write('\n');
            if (panelViewList != null)
            {
                string code = "";
                int len = panelViewList.Count;
                for (int i = 0; i < len; ++i)
                {
                    code = (string)panelViewList[i];
                    writer.WriteElementString("Panel" + Convert.ToString(i + 1), code);
                    stream.Write('\n');
                }
            }
            writer.WriteEndElement();
            stream.Write('\n');

            writer.WriteStartElement("Currency");
            if (currencyList != null)
            {
                int len = currencyList.Count;
                string ccyCode = "";
                for (int i = 0; i < len; ++i)
                {
                    ccyCode = (string)currencyList[i];
                    writer.WriteElementString("CCYCode", ccyCode);
                    stream.Write('\n');
                }
            }
            writer.WriteEndElement();
            stream.Write('\n');

            writer.WriteStartElement("BookingCompany");
            if (companyList != null)
            {
                string company = "";
                int len = companyList.Count;
                for (int i = 0; i < len; ++i)
                {
                    company = (string)companyList[i];
                    writer.WriteElementString("Company", company);
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
            stream.Write('\n'); // End Applies To
            writer.WriteStartElement("Conditions");
            if (hs != null)
            {
                IDictionaryEnumerator iterator = hs.GetEnumerator();
                while (iterator.MoveNext())
                {

                    string key = (string)iterator.Key;
                    if ("AppliesTo".Equals(key, StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }
                    string value = (string)iterator.Value;
                    writer.WriteStartElement("Condition");
                    writer.WriteElementString("Name", key);
                    writer.WriteElementString("Value", value);
                    writer.WriteEndElement();
                    stream.Write('\n');
                }
            }
            writer.WriteEndElement();
            stream.Write('\n');        // end conditions
            writer.WriteEndElement();
            stream.Write('\n');        // end counterparty

//========================================================================================
            hs = (Hashtable)filters["Broker"];
            writer.WriteStartElement("Broker");
            writer.WriteStartElement("BrkrAppliesTo");
            if (hs != null)
            {
                string appliesTo = (string)hs["BrkrAppliesTo"];
                if (appliesTo != null)
                {
                    writer.WriteString(appliesTo);
                }
            }
            writer.WriteEndElement();
            stream.Write('\n'); // End Applies To
            writer.WriteStartElement("BrkrConditions");
            if (hs != null)
            {
                IDictionaryEnumerator iterator = hs.GetEnumerator();
                while (iterator.MoveNext())
                {

                    string key = (string)iterator.Key;
                    if ("BrkrAppliesTo".Equals(key, StringComparison.CurrentCultureIgnoreCase))
                    {
                        continue;
                    }
                    string value = (string)iterator.Value;
                    writer.WriteStartElement("BrkrCondition");
                    writer.WriteElementString("Name", key);
                    writer.WriteElementString("Value", value);
                    writer.WriteEndElement();
                    stream.Write('\n');
                }
            }
            writer.WriteEndElement();
            stream.Write('\n');        // end BrkrCondition
            writer.WriteEndElement();
            stream.Write('\n');        // end Broker

            writer.WriteEndElement();
            stream.Write('\n');        // end filter 
            writer.Close();
            stream.Close();
        }

        internal static DataSet GetCurrencyListFromDb()
        {
            return null;
        }

        internal static DataSet GetSempraCompanyListFromDb()
        {
            return null;
        }
    }
}
