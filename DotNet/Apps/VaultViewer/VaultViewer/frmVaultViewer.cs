using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VaultViewer
{
    public partial class frmVaultViewer : Form
    {
        private const string PRODUCT_COMPANY = "Amphora";
        private const string PRODUCT_BRAND = "Affinity";
        //private const string PRODUCT_GROUP = "Confirms";
        private const string PRODUCT_NAME = "VaultViewer";
        private const string PRODUCT_SETTINGS = "Settings";
        private const string PRODUCT_DATA = "Data";
        private const string PROPS = "properties";
        private List<string> docViewerExtList = new List<string>();
        private List<frmViewerRtf> viewerRtfList = new List<frmViewerRtf>();
        private List<frmViewerPdf> viewerPdfList = new List<frmViewerPdf>();
        private List<frmViewerTif> viewerTifList = new List<frmViewerTif>();
        private string currentTicketNo = "";
        private string currentTradingSys = "";
        private string appSettingsDir = "";
        private string userSettingsFile = "";
        //private string svcApiUrl = "";
        //private string svcApiUserName = "";
        //private string svcApiPassword = "";
        private VaultServiceDal vaultSvcDal = null;


        public enum DocViewerType
        {
            None = 0, Rtf, Pdf, Tif, Doc, Docx
        };

        public frmVaultViewer()
        {
            InitializeComponent();
            //svcApiUrl = Properties.Settings.Default.ExtSvcAPIBaseUrl;
            //svcApiUserName = Properties.Settings.Default.ExtSvcAPIUserName;
            //svcApiPassword = Properties.Settings.Default.ExtSvcAPIPassword;
            vaultSvcDal = new VaultServiceDal(Properties.Settings.Default.ExtSvcAPIBaseUrl,
                Properties.Settings.Default.ExtSvcAPIUserName, Properties.Settings.Default.ExtSvcAPIPassword);
        }
        public frmVaultViewer(string tradeSysCode,string tokenNum)
        {
            InitializeComponent();
            vaultSvcDal = new VaultServiceDal(Properties.Settings.Default.ExtSvcAPIBaseUrl,
                Properties.Settings.Default.ExtSvcAPIUserName, Properties.Settings.Default.ExtSvcAPIPassword);

            LoadVaultViewer(tradeSysCode, tokenNum);
        }
        public void LoadVaultViewer(string tradeSysCode, string tokenNum)
        {
            
            comboEditTradingSys.Text = tradeSysCode;
            textEditTicketNo.Text = tokenNum;

            if ((comboEditTradingSys.Text != currentTradingSys) ||
                (textEditTicketNo.Text != currentTicketNo))
            {
                LoadTreeListForTicket(comboEditTradingSys.Text, textEditTicketNo.Text, null);
                currentTradingSys = comboEditTradingSys.Text;
                currentTicketNo = textEditTicketNo.Text;
            }
        }
        private void frmVaultViewer_Load(object sender, EventArgs e)
        {
            string roamingFolderLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                PRODUCT_COMPANY, PRODUCT_BRAND);
            appSettingsDir = Path.Combine(roamingFolderLocation, PRODUCT_NAME, PRODUCT_SETTINGS);
            //dataDir = Path.Combine(roamingFolderLocation, PRODUCT_NAME, PRODUCT_DATA);
            //appTempDir = Path.Combine(roamingFolderLocation, PRODUCT_NAME, PRODUCT_TEMP);

            if (!Directory.Exists(appSettingsDir))
                Directory.CreateDirectory(appSettingsDir);

            //load
            userSettingsFile = appSettingsDir + @"\VaultViewer.properties";
            PropSettings config = new PropSettings(userSettingsFile);
            //get value whith default value
            this.Top = Convert.ToInt32(config.get("MainFormPos_Top", "200"));
            this.Left = Convert.ToInt32(config.get("MainFormPos_Left", "800"));
            this.Height = Convert.ToInt32(config.get("MainFormPos_Height", "450"));
            this.Width = Convert.ToInt32(config.get("MainFormPos_Width", "400"));

            colCategory.Width = Convert.ToInt32(config.get("CategoryColumn_Width", "150"));
            colMetadata.Width = Convert.ToInt32(config.get("CategoryMetadata_Width", "400"));

            string formIsTopmost = config.get("MainFormPos_StayOnTop", "FALSE");
            checkEditStayOnTop.Checked = (formIsTopmost == "TRUE") ? true : false;
            this.TopMost = checkEditStayOnTop.Checked ? true : false;

            docViewerExtList.Add("DOC");
            docViewerExtList.Add("DOCX");
            docViewerExtList.Add("RTF");
            docViewerExtList.Add("HTML");
            docViewerExtList.Add("TXT");

            //PopulateTicketList();
            PopulateTradingSysList();
        }

        private void frmVaultViewer_FormClosed(object sender, FormClosedEventArgs e)
        {
            PropSettings config = new PropSettings(userSettingsFile);
            config.set("MainFormPos_Top", this.Top.ToString());
            config.set("MainFormPos_Left", this.Left.ToString());
            config.set("MainFormPos_Height", this.Height.ToString());
            config.set("MainFormPos_Width", this.Width.ToString());
            config.set("CategoryColumn_Width", colCategory.Width.ToString());
            config.set("CategoryMetadata_Width", colMetadata.Width.ToString());

            string formIsTopmost = (checkEditStayOnTop.Checked == true) ? "TRUE" : "FALSE";
            config.set("MainFormPos_StayOnTop", formIsTopmost);

            config.Save();

            Process pro = (Process.GetCurrentProcess());
            pro.Kill();
        }

        private void listBoxDocuments_MouseMove(object sender, MouseEventArgs e)
        {
            //ListBoxControl listBoxControl = sender as ListBoxControl;
            //int index = listBoxControl.IndexFromPoint(new Point(e.X, e.Y));
            //if (index != -1)
            //{
            //    //string item = listBoxControl.GetItem(index) as string;
            //    string item = docTipList[index];
            //    toolTipController.ShowHint(item, listBoxControl.PointToScreen(new Point(e.X, e.Y)));
            //}
            //else
            //{
            //    toolTipController.HideHint();
            //}
        }

        private void listBoxDocuments_MouseLeave(object sender, EventArgs e)
        {
            toolTipController.HideHint();
        }

        private void treeListDocs_DoubleClick(object sender, EventArgs e)
        {
            ShowDocForCurrentNode();
        }

        private void ShowDocForCurrentNode()
        {
            try
            {
                //TreeList treeListControl = sender as TreeList;
                TreeListColumn colFileExt = treeListDocs.Columns["FileExt"];
                TreeListColumn colURL = treeListDocs.Columns["URL"];
                TreeListColumn colMetadata = treeListDocs.Columns["Metadata"];
                TreeListNode currentNode = null;
                currentNode = treeListDocs.FocusedNode;

                if (currentNode != null)
                {
                    string docFileName = currentNode.GetValue(colFileName).ToString();
                    string docFileExt = currentNode.GetValue(colFileExt).ToString();
                    string docURL = currentNode.GetValue(colURL).ToString();
                    string docMetadata = currentNode.GetValue(colMetadata).ToString();

                    switch (docFileExt.ToUpper())
                    {
                        case "DOC":
                        case "DOCX":
                        case "RTF":
                        case "HTML":
                        case "TXT":
                            //PopupViewerMultiDocs(docURL, docMetadata, docFileExt);
                            PopupViewerMultiDocs(docFileName, docMetadata, docFileExt, docURL);
                            break;
                        case "PDF":
                            //PopupViewerPdf(docURL, docMetadata);
                            PopupViewerPdf(docFileName, docMetadata, docURL);
                            break;
                        case "TIF":
                            //PopupViewerTif(docURL, docMetadata);
                            PopupViewerTif(docFileName, docMetadata, docURL);
                            break;
                        default:
                            throw new Exception("Internal Error: " + docFileExt + " not found.");
                    }
                }
            }
            catch (Exception excep)
            {
                XtraMessageBox.Show(excep.Message, "Show Document Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region PopUp Forms

        private void PopupViewerMultiDocs(string pFileName, string pMetadata, string pFileExt, string pDocURL)
        {
            bool isDocExists = false;

            //for (int i = 0; i < viewerRtfList.Count - 1; i++ )
            foreach (frmViewerRtf viewDoc in viewerRtfList)
            {
                //if (viewDoc.Url == pUrl)
                if (viewDoc.FileName == pFileName)
                {
                    if (!viewDoc.Visible)
                    {
                        if (viewDoc.IsDisposed)
                        {
                            continue;
                        }                        
                        viewDoc.Show();
                    }
                    else
                    {
                        viewDoc.Focus();
                    }
                    isDocExists = true;
                    break;
                }
            }

            if (!isDocExists)
            {
                frmViewerRtf rtfViewer = new frmViewerRtf();
                //Israel -- Whether filename or URL, this value must be unique.
                //rtfViewer.Url = pUrl;
                rtfViewer.FileName = pFileName;
                rtfViewer.Text = pMetadata;

                DocumentFormat docFormat = GetDocumentFormat(pFileExt);
                //byte[] docSvcByteStream = vaultSvcDal.GetDocumentForURL(pUrl);
                byte[] docSvcByteStream = vaultSvcDal.GetDocumentForFileName(pFileName, pMetadata, pDocURL);                    
                    
                if (docSvcByteStream.Length > 0)
                {
                    using (MemoryStream docMemStream = new MemoryStream(docSvcByteStream))
                    {
                        rtfViewer.richEditControl.LoadDocument(docMemStream, docFormat);
                    }
                }

                viewerRtfList.Add(rtfViewer);
                rtfViewer.Show();
            }
        }

        private void PopupViewerPdf(string pFileName, string pMetadata, string pDocURL)
        {
            bool isDocExists = false;

            foreach (frmViewerPdf viewPdf in viewerPdfList)
            {
                //if (viewPdf.Url == pUrl)
                if (viewPdf.FileName == pFileName)
                    {
                    if (!viewPdf.Visible)
                    {
                        viewPdf.Show();
                    }
                    else
                    {
                        viewPdf.Focus();
                    }
                    isDocExists = true;
                    break;
                }
            }

            if (!isDocExists)
            {
                frmViewerPdf pdfViewerForm = new frmViewerPdf();
                //pdfViewerForm.Url = pUrl;
                pdfViewerForm.FileName = pFileName;
                pdfViewerForm.Text = pMetadata;

                //byte[] docSvcByteStream = vaultSvcDal.GetDocumentForURL(pUrl);
                byte[] docSvcByteStream = vaultSvcDal.GetDocumentForFileName(pFileName, pMetadata, pDocURL);                    

                if (docSvcByteStream.Length > 0)
                {
                    using (MemoryStream docMemStream = new MemoryStream(docSvcByteStream))
                    {
                        pdfViewerForm.pdfViewer.LoadDocument(docMemStream);
                    }
                }

                viewerPdfList.Add(pdfViewerForm);
                pdfViewerForm.Show();
            }
        }

        private void PopupViewerTif(string pFileName, string pMetadata, string pDocURL)
        {
            bool isDocExists = false;

            foreach (frmViewerTif viewTif in viewerTifList)
            {
                //if (viewTif.Url == pUrl)
                if (viewTif.FileName == pFileName)
                    {
                    if (!viewTif.Visible)
                    {
                        viewTif.Show();
                    }
                    else
                    {
                        viewTif.Focus();
                    }
                    viewTif.Show();
                    isDocExists = true;
                    break;
                }
            }

            if (!isDocExists)
            {
                frmViewerTif tifViewerForm = new frmViewerTif();
                //tifViewerForm.Url = pUrl;
                tifViewerForm.FileName = pFileName;
                tifViewerForm.Text = pMetadata;

                //Image img = Image.FromFile(pFileName);
                Image img = null;

                //byte[] docSvcByteStream = vaultSvcDal.GetDocumentForURL(pUrl);
                byte[] docSvcByteStream = vaultSvcDal.GetDocumentForFileName(pFileName, pMetadata, pDocURL);                    

                if (docSvcByteStream.Length > 0)
                {
                    using (MemoryStream docMemStream = new MemoryStream(docSvcByteStream))
                    {
                        img = Image.FromStream(docMemStream);
                    }
                }                    
                    
                tifViewerForm.pictureEdit.Image = img;
                viewerTifList.Add(tifViewerForm);

                tifViewerForm.Show();
            }
        }

        #endregion 

        #region GetDataValue routines

        private DocViewerType GetViewerType(string pFileExt)
        {
            DocViewerType resultType = DocViewerType.None;

            if (docViewerExtList.IndexOf(pFileExt) > -1)
                resultType = DocViewerType.Rtf;
            else if (pFileExt.ToUpper() == "PDF")
                resultType = DocViewerType.Pdf;
            else if (pFileExt.ToUpper() == "TIF")
                resultType = DocViewerType.Tif;

            return resultType;
        }

        private DocumentFormat GetDocumentFormat(string pFileExtension)
        {
            DocumentFormat docFormat = DocumentFormat.Undefined;
            switch (pFileExtension.ToUpper())
            {
                case "DOC":
                    docFormat = DocumentFormat.Doc;
                    break;
                case "DOCX":
                    docFormat = DocumentFormat.OpenXml;
                    break;
                case "RTF":
                    docFormat = DocumentFormat.Rtf;
                    break;
                case "HTML":
                    docFormat = DocumentFormat.Html;
                    break;
                case "TXT":
                    docFormat = DocumentFormat.PlainText;
                    break;
            }//switch DBMS

            return docFormat;
        }

        private string GetDocFormatFileExt(DocumentFormat pDocFormat)
        {
            string fileExt = "";
            if (pDocFormat == DocumentFormat.Doc)
                fileExt = "DOC";
            else if (pDocFormat == DocumentFormat.OpenXml)
                fileExt = "DOCX";
            else if (pDocFormat == DocumentFormat.Rtf)
                fileExt = "RTF";
            else if (pDocFormat == DocumentFormat.Html)
                fileExt = "HTML";
            else if (pDocFormat == DocumentFormat.PlainText)
                fileExt = "TXT";
            return fileExt;
        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGetTradeList_Click(object sender, EventArgs e)
        {
            if ((comboEditTradingSys.Text != currentTradingSys) ||
                (textEditTicketNo.Text != currentTicketNo))
            {
                LoadTreeListForTicket(comboEditTradingSys.Text, textEditTicketNo.Text, null);
                currentTradingSys = comboEditTradingSys.Text;
                currentTicketNo = textEditTicketNo.Text;
            }
        }

        private void LoadTreeListForTicket(string pTradingSysCode, string pTicketNo, Dictionary<string, string> pQueryValues)
        {
            //VaultServiceDal vaultServiceDal = new VaultServiceDal(svcApiUrl, svcApiUserName, svcApiPassword);
            //Dictionary<string, string> docQueryDic = new Dictionary<string, string>();
            //docQueryDic.Add("key", "value");

            List<Dictionary<string, string>> docItemList = new List<Dictionary<string, string>>();
            docItemList = vaultSvcDal.GetDocumentInfoList(pTradingSysCode, pTicketNo, null);

            treeListDocs.Nodes.Clear();
            foreach (Dictionary<string, string> docItem in docItemList)
            {
                string category = "[None]";
                string metaData = String.Empty;
                string url = String.Empty;
                string fileExt = String.Empty;
                string fileName = String.Empty;
                bool isFirst = true;

                foreach (KeyValuePair<string, string> entry in docItem)
                {
                    if (entry.Key != "URL" && entry.Key != "DocType")
                    {
                        if (!isFirst)
                            metaData += "; ";
                        else
                            isFirst = false;
                        metaData += entry.Key + "=" + entry.Value;
                    }

                    if (entry.Key == "DocType")
                        category = entry.Value;
                    if (entry.Key == "URL")
                        url = entry.Value;
                    if (entry.Key == "FileType")
                        fileExt = entry.Value;
                    if (entry.Key == "DocName")
                        fileName = entry.Value;
                }

                treeListDocs.AppendNode(new object[] { category, metaData, url, fileExt, fileName }, -1);
            }

            CloseAllOpenForms();
        }


        private void LoadTreeListForTicket(string pTicket)
        {
            //Original stub -- reads sample data from file system directories
            string currentDir = Path.Combine(Properties.Settings.Default.TestRootDir, pTicket);
            string propsFileName = pTicket + "." + PROPS;
            string propsFileAndPath = Path.Combine(currentDir, propsFileName);
            PropSettings configFile = new PropSettings(propsFileAndPath);
            treeListDocs.Nodes.Clear();

            string[] fileEntries = Directory.GetFiles(currentDir);
            foreach (string fileName in fileEntries)
            {
                string fileExt = Path.GetExtension(fileName).Remove(0,1).ToUpper();
                if (fileExt != PROPS.ToUpper())
                {
                    string docCategory = Path.GetFileNameWithoutExtension(fileName);
                    docCategory = docCategory.Substring(0, docCategory.IndexOf("_"));

                    string fileNameWithoutPath = Path.GetFileName(fileName);
                    string docMetaData = configFile.get(fileNameWithoutPath, "[None]");                    
                    treeListDocs.AppendNode(new object[] { docCategory, docMetaData, fileExt, fileName }, -1);
                }
            }

            CloseAllOpenForms();
        }

        private void CloseAllOpenForms()
        {
            foreach (frmViewerRtf viewDoc in viewerRtfList)
            {
                viewDoc.Close();
                viewDoc.Dispose();
            }

            viewerRtfList.Clear();

            foreach (frmViewerPdf viewPdf in viewerPdfList)
            {
                viewPdf.Close();
                viewPdf.Dispose();
            }

            viewerPdfList.Clear();

            foreach (frmViewerTif viewTif in viewerTifList)
            {
                viewTif.Close();
                viewTif.Dispose();
            }

            viewerTifList.Clear();
        }

        private void PopulateTicketList()
        {
            //string rootDir = Properties.Settings.Default.TestRootDir;
            //string[] subdirectoryEntries = Directory.GetDirectories(rootDir);
            //string folderName = "";
            //foreach (string subdirectory in subdirectoryEntries)
            //{
            //    folderName = Path.GetFileName(subdirectory);
            //    comboEditTicketList.Properties.Items.Add(folderName);
            //}
        }

        private void PopulateTradingSysList()
        {
            foreach (string item in Properties.Settings.Default.TradingSysCodes)
            {
                comboEditTradingSys.Properties.Items.Add(item);
            }
        }

        private void TestVaultServiceAccess()
        {
            //VaultServiceDal vaultServiceDal = new VaultServiceDal(svcApiUrl, svcApiUserName, svcApiPassword);
            string tradingSysCode = "SYMPH";
            string ticketNo = "IF-1106-09";            
            //Dictionary<string, string> docQueryDic = new Dictionary<string, string>();
            //docQueryDic.Add("key", "value");

            List<Dictionary<string, string>> dicList = new List<Dictionary<string, string>>();
            dicList = vaultSvcDal.GetDocumentInfoList(tradingSysCode, ticketNo, null);
        }

        private void checkEditStayOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = checkEditStayOnTop.Checked ? true : false;
        }

        private void btnDocForms_Click(object sender, EventArgs e)
        {
            CloseAllOpenForms();
        }



    }
}
