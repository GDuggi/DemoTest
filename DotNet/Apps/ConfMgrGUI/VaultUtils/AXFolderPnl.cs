using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Repository;
using System.IO;
using DevExpress.XtraTab;
using DataManager;
//using VaultUtils.ConfirmServices;
using CommonUtils;
using System.Reflection;

namespace VaultUtils
{
    public partial class AXFolderPnl : UserControl
    {
        private const string FORM_NAME = "AXFolderPnl";
        private const string FORM_ERROR_CAPTION = "Folder Panel Form Error";
        private AxFolder axFolder = null;
        private DataTable axFieldMappings = null;
        private SempraDocWs sempraDocWs;
        private DataTable dtDocuments = null;
        private DataTable tblIndexes = null;
        private string defaultTradeId = null;
        //public ConfirmationWeb confirmationService = new ConfirmationWeb();
        private WebUtils webUtils;

        enum EditorType
        {
            TXT, CMBO
        };

        public AXFolderPnl()
        {
            webUtils = new WebUtils();
            webUtils.logonUser = "ifrankel";

            //confirmationService.Url = webUtils.jbossWSUrlConfirmation;
            //confirmationService.userName = GetConfirmationServiceUserName();
        }

        //private ConfirmServices.@string GetConfirmationServiceUserName()
        //{
        //    try
        //    {
        //        //string userFullName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        //        string userFullName = webUtils.logonUser;
        //        string userName = userFullName.Substring(userFullName.LastIndexOf("\\") + 1);
        //        ConfirmServices.@string uName = new ConfirmServices.@string();
        //        uName.Text = new string[] { userName };
        //        return uName;
        //    }
        //    catch (Exception except)
        //    {
        //        throw new Exception("GetConfirmationServiceUserName: " + except.Message);
        //    }
        //}

        public string DefaultTradeId
        {
            set 
            { 
                defaultTradeId = value;
                ClearSearchQryControls();
            }
        }

        private void ClearSearchQryControls()
        {
            barChkViewAllVersions.EditValue = false;
            for (int i = 0; i < rbnCntrlAxFolderPnal.Items.Count; i++)
            {
                if ((rbnCntrlAxFolderPnal.Items[i]) is DevExpress.XtraBars.BarEditItem)
                {
                    ((DevExpress.XtraBars.BarEditItem)(rbnCntrlAxFolderPnal.Items[i])).EditValue = null;
                    if (rbnCntrlAxFolderPnal.Items[i].Name.Equals("barEditItemTRD_SYS_ID"))
                    {
                        ((DevExpress.XtraBars.BarEditItem)(rbnCntrlAxFolderPnal.Items[i])).EditValue = defaultTradeId;
                    }
                    if (rbnCntrlAxFolderPnal.Items[i].Name.Equals("barEditItemSIGNED_FLAG"))
                    {
       //                 ((DevExpress.XtraBars.BarEditItem)(rbnCntrlAxFolderPnal.Items[i])).EditValue = "Y";
                    }

                    if (rbnCntrlAxFolderPnal.Items[i].Name.Equals("barEditItemSENDER_TYPE_IND"))
                    {
                        if (this.axFolder.FolderName == "INBOUND_CONFIRMS")
                        {
                            ((DevExpress.XtraBars.BarEditItem)(rbnCntrlAxFolderPnal.Items[i])).EditValue = "CS";
                        }
                        else if (this.axFolder.FolderName == "INBOUND_COUNTERPARTY")
                        {
                            ((DevExpress.XtraBars.BarEditItem)(rbnCntrlAxFolderPnal.Items[i])).EditValue = "CC";
                        }
                        else if (this.axFolder.FolderName == "INBOUND_BROKER")
                        {
                            ((DevExpress.XtraBars.BarEditItem)(rbnCntrlAxFolderPnal.Items[i])).EditValue = "B";
                        }
                    }
                }
            }
        }

        public void SetADefaultValue(string fieldName, string value)
        {
            fieldName = "barEditItem" + fieldName;
            for (int i = 0; i < rbnCntrlAxFolderPnal.Items.Count; i++)
            {
                if ((rbnCntrlAxFolderPnal.Items[i]) is DevExpress.XtraBars.BarEditItem)
                {
                    ((DevExpress.XtraBars.BarEditItem)(rbnCntrlAxFolderPnal.Items[i])).EditValue = null;
                    if (rbnCntrlAxFolderPnal.Items[i].Name.Equals(fieldName))
                    {
                        ((DevExpress.XtraBars.BarEditItem)(rbnCntrlAxFolderPnal.Items[i])).EditValue = value;
                    }
                }
            }
        }


        public AXFolderPnl(AxFolder axFolder, DataTable axFieldMappings, ref SempraDocWs sempraDocWs)
        {
            InitializeComponent();
            barChkViewAllVersions.EditValue = false;
            this.sempraDocWs = sempraDocWs;
            this.axFolder = axFolder;
            this.axFieldMappings = axFieldMappings;
            CreateFieldEditors();
        }

        private void CreateFieldEditors()
        {
            int count = 0;
            AXFieldMapping fieldMapping = null;
            if (axFolder.Fields.Length > 0)
            {
                string[] fields = axFolder.Fields.Split('|');

                foreach (string field in fields)
                {
                    fieldMapping = GetAxFieldMapping(field);
                    RepositoryItem[] repositoryItems = GetRepositoryEditor(fieldMapping);
                    rbnCntrlAxFolderPnal.RepositoryItems.AddRange(repositoryItems);

                    BarEditItem barEditItem = new DevExpress.XtraBars.BarEditItem();

                    barEditItem.Caption = field + ":";
                    barEditItem.Edit = repositoryItems[0];
                    barEditItem.Id = count;
                    barEditItem.Name = "barEditItem" + field;
                    barEditItem.Width = 100;

                    if (! fieldMapping.DefaultValue.Contains(","))
                    {
                        barEditItem.EditValue = fieldMapping.DefaultValue;
                    }
    
                    if (fieldMapping != null)
                    {
                        if (fieldMapping.DisplayName != "")
                        {
                            barEditItem.Caption = fieldMapping.DisplayName + ":";
                        }
                        if (fieldMapping.Width != "")
                        {
                            barEditItem.Width = Convert.ToInt16(fieldMapping.Width);
                        }

                        DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
                        DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
                        DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();

                        toolTipTitleItem1.Text = "Field Information";
                        toolTipItem1.LeftIndent = 6;
                        toolTipItem1.Text = fieldMapping.HelpDisplay;
                        superToolTip1.Items.Add(toolTipTitleItem1);
                        superToolTip1.Items.Add(toolTipItem1);
                        barEditItem.SuperTip = superToolTip1;
                    }
                    rbnCntrlAxFolderPnal.Items.AddRange(new DevExpress.XtraBars.BarItem[] { barEditItem });
                    rbnCntrlAxFolderPnal.Toolbar.ItemLinks.Add(barEditItem);
                    ribnPageGrpAxFields.ItemLinks.Add(barEditItem);
                    count++;
                }
            }
        }

        private RepositoryItem[] GetRepositoryEditor(AXFieldMapping fieldMapping)
        {
            RepositoryItem[] items = new DevExpress.XtraEditors.Repository.RepositoryItem[1];
            EditorType editorType = (EditorType)Enum.Parse(typeof(EditorType), fieldMapping.EditorType, true);

            if (fieldMapping != null)
            {
                switch (editorType)
                {
                    case EditorType.TXT:
                        {
                            RepositoryItemTextEdit repositoryItemEditor = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
                            ((System.ComponentModel.ISupportInitialize)(repositoryItemEditor)).BeginInit();
                            repositoryItemEditor.AutoHeight = false;
                            repositoryItemEditor.Name = "repositoryItemTextEdit" + fieldMapping.FieldName;
                            items[0] = repositoryItemEditor;
                            break;
                        }
                    case EditorType.CMBO:
                        {
                            RepositoryItemComboBox repositoryItemEditor = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
                            ((System.ComponentModel.ISupportInitialize)(repositoryItemEditor)).BeginInit();

                            repositoryItemEditor.AutoHeight = false;
                            repositoryItemEditor.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
                            repositoryItemEditor.Name = "repositoryItemTextEdit" + fieldMapping.FieldName;
                            string[] values = fieldMapping.DefaultValue.Split(',');
                            repositoryItemEditor.Items.AddRange(values);
                            items[0] = repositoryItemEditor;
                            break;
                        }
                }
            }
            return items;
        }

        private AXFieldMapping GetAxFieldMapping(string field)
        {
            AXFieldMapping fieldMapping = null;

            if (axFieldMappings != null)
            {
                DataRow[] rows = axFieldMappings.Select("FieldName = '" + field + "'");
                if ((rows != null) && (rows.Length > 0))
                {
                    DataRow dr = rows[0];
                    fieldMapping = CollectionHelper.CreateObjectFromDataRow<AXFieldMapping>(dr);
                }
            }
            if (fieldMapping == null)  // create a default field 
            {
                fieldMapping = new AXFieldMapping(field);
            }
            return fieldMapping;
        }

        private void barBtnExecuteQry_ItemClick(object sender, ItemClickEventArgs e)
        {
     
           if (validateTradeID()) {
                GetDocuments();
           }
        }
        private bool validateTradeID()
        {
            bool returnValue = true;
            foreach ( BarItem item in rbnCntrlAxFolderPnal.Items) {
                if (item.Caption.ToLower().StartsWith("trade id"))
                {
                    BarEditItem editItem = (BarEditItem)item;
                    if (editItem.EditValue.ToString() == "")
                    {
                        MessageBox.Show("Please enter the trade ID.");
                        returnValue = false;
                        return returnValue;
                    }
                    try
                    {
                        int tradeId = Convert.ToInt32(editItem.EditValue.ToString());
                        returnValue = AXPnl.ValidateTradeWithUser(tradeId);

                    }
                    catch (Exception e)
                    {
                        XtraMessageBox.Show("Please enter a valid trade ID." + Environment.NewLine +
                               "Error CNF-410 in " + FORM_NAME + ".validateTradeID(): " + e.Message,
                             FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        returnValue = false;
                    }

                }
            }
            return returnValue;

        }
       
        public void GetDocuments()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                GetDocumentListFromVault();
                CreateDocumentViewers();
                LabelAXDocumentViewers();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        public void SaveDocuments(string path, string fileName)
        {
            XtraAxTabPage tabPage = GetActiveTabPage();

            if (tabPage != null)
            {
                IAXTab document = (IAXTab)tabPage;
                document.SaveDocument(path, fileName);
            }
        }

        private void LabelAXDocumentViewers()
        {
            bool isGrouped = false;
            foreach (XtraTabPage tabPage in xtraTabControl1.TabPages)
            {
                foreach (Control control in tabPage.Controls)
                {
                    if (control is XtraTabControl)
                    {
                        isGrouped = true;
                        XtraTabControl tabCntrl = (XtraTabControl)control;
                        for (int i = 0; i < tabCntrl.TabPages.Count; i++)
                        {
                            XtraTabPage tabVersion = tabCntrl.TabPages[i];
                            if (i == 0)
                            {
                                tabVersion.Text = "Latest Version";
                            }
                            else
                            {
                                tabVersion.Text = "Version (" + ((tabCntrl.TabPages.Count - i)) + ")";
                            }
                        }
                    }
                }
            }
            if (isGrouped == false)
            {
                for (int i = 0; i < xtraTabControl1.TabPages.Count; i++)
                {
                    XtraTabPage tabVersion = xtraTabControl1.TabPages[i];
                    if (i == 0)
                    {
                        tabVersion.Text = "Latest Version";
                    }
                    else
                    {
                        tabVersion.Text = "Version (" + ((xtraTabControl1.TabPages.Count - i)) + ")";
                    }
                }
            }
        }

        private void CreateDocumentViewers()
        {
            // we want latest version, ONLY if view all verions is NOT checked, AND there is not a specific version selected
            bool displayLatestDocOnly = true;
            if (barChkViewAllVersions.EditValue != null)
            {
                displayLatestDocOnly = (barChkViewAllVersions.EditValue.Equals(false));
            }

            string resouceId = null;
            int versionCounter = 0;
            string versionIndexField = "";
            IList<string> docTypes = new List<string>();
            if (dtDocuments != null)
            {
                // CREATE THIS TABLE ONLY IF THE CURRENT INDEX QRY HAS CHANGED.  ALSO, DO NOT CLEAR THE DOCUMENTS AS WELL
                ClearDocs();


                if (axFolder.GroupByIndexField != "")
                {
                    docTypes = CreateParrentTabs(tblIndexes);
                }

                versionCounter = dtDocuments.Rows.Count;

                //create for all rows
                foreach (DataRow dr in dtDocuments.Rows)
                {
                    resouceId = (string)dr[0];
                    XtraAxTabPage axTabPage = null;

                    versionIndexField = "";
                    if (axFolder.GroupByIndexField != "")
                    {
                        versionIndexField = (string)tblIndexes.Rows[versionCounter - 1][axFolder.GroupByIndexField];
                    }
                    if (versionIndexField != "")  // WE HAVE A VERSION TAB
                    {
                        XtraTabPage versionTabPage = GetVersionTabPage(versionIndexField);
                        XtraTabControl versionTabControl = GetVersionTabControl(versionTabPage, versionIndexField);
                        if (versionTabControl.TabPages.Count == 0)  // Latest document always is the first tab created
                        {
                            axTabPage = new XtraAxTabPage(axFolder, resouceId, ref sempraDocWs);
                            if (axTabPage.Controls.Count > 0)
                            {
                                versionTabControl.TabPages.AddRange(new XtraAxTabPage[] { axTabPage });
                            }
                        }
                        else  // loading all versions
                        {
                            if (displayLatestDocOnly == false)
                            {
                                axTabPage = new XtraAxTabPage(axFolder, resouceId, ref sempraDocWs);
                                if (axTabPage.Controls.Count > 0)
                                {
                                    versionTabControl.TabPages.AddRange(new XtraAxTabPage[] { axTabPage });
                                }
                            }
                        }
                    }
                    else  // these types of documents don't have parrent tabs....
                    {
                        if (xtraTabControl1.TabPages.Count == 0)  // Latest document always is the first tab created
                        {
                            axTabPage = new XtraAxTabPage(axFolder, resouceId, ref sempraDocWs);
                            xtraTabControl1.TabPages.AddRange(new XtraAxTabPage[] { axTabPage });
                        }
                        else  // loading all versions
                        {
                            if (displayLatestDocOnly == false)
                            {
                                axTabPage = new XtraAxTabPage(axFolder, resouceId, ref sempraDocWs);
                                xtraTabControl1.TabPages.AddRange(new XtraAxTabPage[] { axTabPage });
                            }
                        }
                    }
                    versionCounter--;
                }
            }
        }

        private IList<string> CreateParrentTabs(DataTable tblIndexes)
        {
            IList<string> indexList = new List<string>();
            string value = "";


            foreach (DataRow row in tblIndexes.Rows)
            {
                value = (string)row[axFolder.GroupByIndexField];
                if (value != "")
                {
                    if (!indexList.Contains(value))
                    {
                        indexList.Add(value);
                    }
                }
            }

            foreach (string tabName in indexList)
            {
                XtraTabPage versionTabPage = new XtraTabPage();
                versionTabPage.Name = "tabPage_" + tabName;
                versionTabPage.Text = tabName;
                xtraTabControl1.TabPages.AddRange(new XtraTabPage[] { versionTabPage });

                // create the tabControl to hold the document version for this type of document...
                XtraTabControl tabControl = null;
                if (tabControl == null)
                {
                    tabControl = new XtraTabControl();
                    tabControl.Dock = DockStyle.Fill;
                    tabControl.Name = "tabCntrl_" + tabName;
                    tabControl.TabPages.Clear();
                    versionTabPage.Controls.Add(tabControl);
                }
            }

            return indexList;
        }

        private XtraTabControl GetVersionTabControl(XtraTabPage versionTabPage, string versionIndexField)
        {
            string tabControlName = "tabCntrl_" + versionIndexField;
            XtraTabControl tabControl = null;

            foreach (Control control in versionTabPage.Controls)
            {
                if (control is XtraTabControl)
                {
                    if (tabControlName.Equals(control.Name))
                    {
                        tabControl = (XtraTabControl)control;
                        break;
                    }
                }
            }
            if (tabControl == null)
            {
                tabControl = new XtraTabControl();
                tabControl.Dock = DockStyle.Fill;
                tabControl.Name = tabControlName;
                tabControl.TabPages.Clear();
                versionTabPage.Controls.Add(tabControl);
            }
            return tabControl;
        }

        private XtraTabPage GetVersionTabPage(string versionIndexField)
        {
            XtraTabPage versionTabPage = null;
            string caption = "tabPage_" + versionIndexField;
            foreach (XtraTabPage tabPage in xtraTabControl1.TabPages)
            {
                if (caption.Equals(tabPage.Name))
                {
                    versionTabPage = tabPage;
                    break;
                }
            }
            if (versionTabPage == null)
            {
                versionTabPage = new XtraTabPage();
                versionTabPage.Name = caption;
                versionTabPage.Text = versionIndexField;
                xtraTabControl1.TabPages.AddRange(new XtraTabPage[] { versionTabPage });
            }
            return versionTabPage;
        }

        private void ClearDocs()
        {
            xtraTabControl1.TabPages.Clear();
        }

        public void GetDocumentListFromVault()
        {
            string savePath = Environment.GetEnvironmentVariable("HOMEDRIVE").ToString() + @"\temp\";
            string fieldValues = GenerateFieldValues();
            string vaultFolderName = this.axFolder.FolderName;

            if (vaultFolderName.Contains("INBOUND"))
            {
                vaultFolderName = "INBOUND_CONFIRMS";
            }


            DataSet dataStore = null;
            if (tblIndexes != null)
            {
                tblIndexes.Clear();
            }
            if (dtDocuments != null)
            {
                dtDocuments.Clear();
            }
            try
            {
                if (sempraDocWs != null)
                {

                    string RetXML = sempraDocWs.GetDocumentList(vaultFolderName, this.axFolder.Fields, fieldValues, false, axFolder.DslName);
                    System.Xml.XmlDocument RetXMLNode = new System.Xml.XmlDocument();
                    RetXMLNode.LoadXml(RetXML);

                    if (RetXMLNode.DocumentElement.SelectSingleNode("//CallResult/ResultCode") != null)
                    {
                        string ResultCode = RetXMLNode.DocumentElement.SelectSingleNode("//CallResult/ResultCode").InnerText;
                        string ResultValue = RetXMLNode.DocumentElement.SelectSingleNode("//CallResult/ResultValue").InnerText;
                        if (ResultValue.ToUpper().Contains("NO RECORDS FOUND"))
                        {
                            // don't display any message...
                        }
                        else  // show exception.
                        {
                            XtraMessageBox.Show("No data was retrieved. Result from server: " + ResultValue + "." + Environment.NewLine +
                                  "Error CNF-546 in " + FORM_NAME + ".GetDocumentListFromVault().",
                                FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        RetXMLNode.Save(savePath + "DocList.xml");
                        if (File.Exists(savePath + "DocList.xml"))
                        {
                            dataStore = new DataSet();
                            dataStore.ReadXml(savePath + "DocList.xml");
                            dtDocuments = dataStore.Tables["Doc"];
                            tblIndexes = dataStore.Tables["Indexes"];
                            File.Delete(savePath + "DocList.xml");
                        }
                    }
                }
                else
                {
                    throw new Exception("DocumentVault Webservice is not Intitialized." + Environment.NewLine +
                         "Error CNF-411 in " + FORM_NAME + ".GetDocumentListFromVault().");
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while retrieving a document from the vault." + Environment.NewLine +
                      "Error CNF-412 in " + FORM_NAME + ".GetDocumentListFromVault(): " + ex.Message,
                    FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetContractFromVault(long ATradeRqmtConfirmId, long APrmntConfirmId)
        {
            try
            {
                string contractBody = "";
                //getContract gc = new getContract();

                //contractRequest request = new contractRequest();
                //request.tradeRqmtConfirmId = ATradeRqmtConfirmId;
                //request.prmntConfirmId = APrmntConfirmId;

                //gc.vaultRequest = request;
                //getContractResponse resp = confirmationService.getContract(gc);
                //if (resp.@return.responseStatus != "OK")
                //{
                //    string emailBody = "Failed to get contract from  vault." + Environment.NewLine;
                //    emailBody += "responseStatus=" + resp.@return.responseStatus + Environment.NewLine;
                //    emailBody += "responseText=" + resp.@return.responseText + Environment.NewLine;
                //    emailBody += "request.tradeRqmtConfirmId=" + resp.@return.request.tradeRqmtConfirmId.ToString() + Environment.NewLine;
                //    emailBody += "----------------------------------------------------" + Environment.NewLine;
                //    emailBody += "responseStackError=" + resp.@return.responseStackError + Environment.NewLine;
                //    //SendEmail(toolbarOrWindowsUserId + "@amphorainc.com", Properties.Settings.Default.EMailGroupSupport,
                //      // "Failed to get contract from vault", emailBody);
                //    throw new Exception(Environment.NewLine + "Error getting contract from vault."); // +
                //       //"Please contract ConfirmSupport." +
                //       //Environment.NewLine + "An email has been sent to ConfirmSupport by the system.");
                //}
                //else
                //    contractBody = resp.@return.contract;

                return contractBody;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving a document from the vault." + Environment.NewLine +
                     "Error CNF-413 in " + FORM_NAME + ".GetContractFromVault(): " + ex.Message);
            }
        }


        private void formatFields(out string myFields, out string fieldValues)
        {
            throw new NotImplementedException("An error occurred while formatting fields." + Environment.NewLine +
                         "Error CNF-412 in " + FORM_NAME + ".formatFields().");
        }

        private string GenerateFieldValues()
        {
            string[] fields = axFolder.Fields.Split('|');
            string[] fieldValues = new string[fields.Length];
            int count = 0;
            string fieldVals;

            foreach (string field in fields)
            {
                BarEditItem editItem = (BarEditItem)rbnCntrlAxFolderPnal.Items["barEditItem" + field];
                if (editItem != null)
                {
                    if (editItem.EditValue != null)
                    {
                        fieldValues[count] = (string)editItem.EditValue;
                    }
                    else
                    {
                        fieldValues[count] = "";
                        if (editItem.Name.Equals("barEditItemSENDER_TYPE_IND"))
                        {
                            if (this.axFolder.FolderName == "INBOUND_CONFIRMS")
                            {
                                fieldValues[count] = "CS";
                            }
                            else if (this.axFolder.FolderName == "INBOUND_COUNTERPARTY")
                            {
                                fieldValues[count] = "CC";
                            }
                            else if (this.axFolder.FolderName == "INBOUND_BROKER")
                            {
                                fieldValues[count] = "B";
                            }
                        }
                    }
                    count++;
                }
            }
            fieldVals = string.Join("|", fieldValues);
            return fieldVals;
        }

        private void barBtnPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            XtraAxTabPage tabPage = GetActiveTabPage();

            if (tabPage != null)
            {
                IAXTab document = (IAXTab)tabPage;
                document.PrintDocument();
            }
        }

        private void barBtnXmit_ItemClick(object sender, ItemClickEventArgs e)
        {
            XtraAxTabPage tabPage = GetActiveTabPage();

            if (tabPage != null)
            {
                IAXTab document = (IAXTab)tabPage;
                document.TransmitDocument();
            }
        }

        private XtraAxTabPage GetActiveTabPage()
        {
            XtraTabControl tabControl = xtraTabControl1;
            XtraTabPage tabPage = null;
            XtraAxTabPage returnVal = null;

            if (tabControl.TabPages != null)
            {
                tabPage = xtraTabControl1.TabPages[xtraTabControl1.SelectedTabPageIndex];
                foreach (Control control in tabPage.Controls)
                {
                    if (control is XtraTabControl) // it is a grouping tab..
                    {
                        tabControl = (XtraTabControl)control;
                        break;
                    }
                }
            }

            if (tabControl.SelectedTabPage != null)
            {
                returnVal = (XtraAxTabPage)tabControl.TabPages[tabControl.SelectedTabPageIndex];
            }

            return returnVal;
        }

        private void barChkViewAllVersions_ItemClick(object sender, ItemClickEventArgs e)
        {
            // nothing...
        }
    }
}
