using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.IO;
//using VaultUtils.trackingClient;
using DataManager;
using DBAccess.SqlServer;
using DBAccess;

namespace VaultUtils
{
    public partial class AXPnl : UserControl
    {
        private const string PROJ_FILE_NAME = "AXPnl";
        private bool saveToDisk = false;
        private string saveToRootFolder = "";
        public string p_UserId = "";
        public static string sqlConnectionString = "";

        public string SaveToRootFolder
        {
            get { return saveToRootFolder; }
            set { saveToRootFolder = value; }
        }

        public bool SaveToDisk
        {
            get { return saveToDisk; }
            set { saveToDisk = value; }
        }

        SempraDocWs ws = null;
        //private static trackingClient.TradeConfirmService trackingMain = null;

        public AXPnl()
        {
            InitializeComponent();
        }

        public void GeneratePnls(string sourceFilePath, string fieldMappingsFilePath, string vaultUrl)
        {
            DataSet dsStore = null;
            DataSet dsStoreMappings = null;
            DataTable dtMappings = null;
            DataTable dt = null;
            try
            {
                ws = new SempraDocWs();
                ws.Url = vaultUrl;
                ws.Credentials = System.Net.CredentialCache.DefaultCredentials;

                //trackingMain.Url =
                //trackingMain = new VaultUtils.trackingClient.TradeConfirmService();
                //trackingMain.userName = GetServicesUserName();

                if (File.Exists(sourceFilePath))
                {
                    dsStoreMappings = new DataSet();
                    dsStoreMappings.ReadXml(fieldMappingsFilePath);
                    dtMappings = dsStoreMappings.Tables["AxField"];
                }

                if (File.Exists(sourceFilePath))
                {
                    dsStore = new DataSet();
                    dsStore.ReadXml(sourceFilePath);
                    dt = dsStore.Tables["AxFolders"];
                    if (dt != null)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            AxFolder newFolder = CollectionHelper.CreateObjectFromDataRow<AxFolder>(dr);
                            AddAxFolderPnl(newFolder, dtMappings);
                        }
                    }
                }
                else
                {
                    throw new Exception("Exception AXPnl GeneratePnls: File does not exist: " + Environment.NewLine +
                        "Error CNF-406 in " + PROJ_FILE_NAME + ".GeneratePnls().");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating Inbound Panels." + Environment.NewLine +
                    "Error CNF-407 in " + PROJ_FILE_NAME + ".GeneratePnls(): " + ex.Message);
            }
        }
        public void GeneratePnls(string sourceFilePath, string fieldMappingsFilePath, string vaultUrl,string trackinUrl)
        {
            DataSet dsStore = null;
            DataSet dsStoreMappings = null;
            DataTable dtMappings = null;
            DataTable dt = null;
            try
            {
                ws = new SempraDocWs();
                ws.Url = vaultUrl;
                ws.Credentials = System.Net.CredentialCache.DefaultCredentials;

                //trackingMain.Url =
                //trackingMain = new VaultUtils.trackingClient.TradeConfirmService();
                //trackingMain.Url = trackinUrl;
                //trackingMain.userName = GetServicesUserName();

                if (File.Exists(sourceFilePath))
                {
                    dsStoreMappings = new DataSet();
                    dsStoreMappings.ReadXml(fieldMappingsFilePath);
                    dtMappings = dsStoreMappings.Tables["AxField"];
                }

                if (File.Exists(sourceFilePath))
                {
                    dsStore = new DataSet();
                    dsStore.ReadXml(sourceFilePath);
                    dt = dsStore.Tables["AxFolders"];
                    if (dt != null)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            AxFolder newFolder = CollectionHelper.CreateObjectFromDataRow<AxFolder>(dr);
                            AddAxFolderPnl(newFolder, dtMappings);
                        }
                    }
                }
                else
                {
                    throw new Exception("Exception AXPnl GeneratePnls: File does not exist: " + Environment.NewLine +
                        "Error CNF-408 in " + PROJ_FILE_NAME + ".GeneratePnls().");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating Inbound Panels." + Environment.NewLine +
                    "Error CNF-409 in " + PROJ_FILE_NAME + ".GeneratePnls(): " + ex.Message);
            }
        }

        //private VaultUtils.trackingClient.@string GetServicesUserName()
        //{
        //    //string userFullName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        //    string userFullName = p_UserId;
        //    string userName = userFullName.Substring(userFullName.LastIndexOf("\\") + 1);
        //    //WebReference.@string s = new WindowsApplication2.WebReference.@string();
        //    VaultUtils.trackingClient.@string uName = new VaultUtils.trackingClient.@string();
        //    uName.Text = new string[] { userName };
        //    return uName;
        //}

        private void AddAxFolderPnl(AxFolder newFolder, DataTable axFieldMappings)
        {
            XtraTabAxFolderPage tabPage = new XtraTabAxFolderPage(newFolder, axFieldMappings, ref ws);
            xtraTabControlAxFolders.TabPages.Add(tabPage);
        }

        private void barBtnGetAllDocuments_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (barItemDefaultTradeID.EditValue != null)
            {
                string tradeId = (string)barItemDefaultTradeID.EditValue;
                SetGlobalDefaultTradeId(tradeId);
                try
                {
                    int ticket = Convert.ToInt32(tradeId);
                    if (ValidateTradeWithUser(ticket)){
                        GetDocuments();
                    }
                }
                catch (Exception ef)
                {
                }
                
            }
        }

        public void SetGlobalDefaultTradeId(string tradeId)
        {
            barItemDefaultTradeID.EditValue = tradeId;
            foreach (XtraTabAxFolderPage tabPage in xtraTabControlAxFolders.TabPages)
            {
                tabPage.DefaultTradeId = tradeId;
            }
        }

        public void GetDocuments()
        {
            foreach (XtraTabAxFolderPage tabPage in xtraTabControlAxFolders.TabPages)
            {
                if (tabPage.Text.Contains("Our Contract"))
                {
                    String test = tabPage.Text;
                }
                else
                    tabPage.GetDocuments();
            }
        }

        public void SaveDocuments(string filePath, string tradeId)
        {
            string newFileName = "";
            string newFolderName = "";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            foreach (XtraTabAxFolderPage tabPage in xtraTabControlAxFolders.TabPages)
            {
                newFolderName = filePath + tabPage.Text + @"\";
                newFileName = tradeId;
                tabPage.SaveDocuments(newFolderName, newFileName);
            }
        }

        public void SetADefaultValue(string fieldName, string value)
        {
            foreach (XtraTabAxFolderPage tabPage in xtraTabControlAxFolders.TabPages)
            {
                tabPage.SetADefaultValue(fieldName, value);
                tabPage.GetDocuments();
            }
        }

        public static bool ValidateTradeWithUser(int pTradeId)
        {
            bool validTrade = false;

            //getTradeRequest tradeRequest = new getTradeRequest();
            //tradeRequest.tradeIdList = tradeId.ToString();
            //getTrades request = new getTrades();
            //request.tradeRequest = tradeRequest;
            //getTradesResponse response =  trackingMain.getTrades(request);
            //if (response.@return.responseStatus == "OK")
            //{
            //    if (response.@return.trades != null && response.@return.trades.Length > 0)
            //    {
            //        validTrade = true;
            //    }
            //}

            VPcTradeSummaryDal pcTradeSummaryDal = new VPcTradeSummaryDal(sqlConnectionString);
            validTrade = pcTradeSummaryDal.IsValidTradeId(pTradeId);
            return validTrade;
        }
    }
}
