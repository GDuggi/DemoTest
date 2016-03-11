using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using CommonUtils;
using DataManager;
using DBAccess;
using DBAccess.SqlServer;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;
using log4net;
using OpsTrackingModel;

//using ConfirmInbound.InboundServices;

namespace ConfirmInbound
{
    public delegate void TransmitDocumentDelegate();

    public delegate bool IsDocumentModifiedDelegate();

    public delegate void SaveImageToFileDelegate();

    public delegate void ImageFilenameDelegate(string fileName, string saveAsFilename, bool canEdit);

    public delegate void PrintInboundDocument();

    public delegate int InboundDocPagesDelegate();

    public delegate void ExtractPagesWithAnnotationDelegate(string fileName, string fileLocation, string pageList);

    public delegate string GetLoadedDocumentFileName();

    public delegate void SubmitEditRqmt(long ATradeId, long ARqmtId, string ARqmtCode, string AStatusCode,
        DateTime AStatusDate, string AReference, string AComment, bool AUpdateLocalTableNow);

    public delegate void GetActiveTradeRqmtDelegate();

    public delegate SummaryData LocateTradeSummaryRecDelegate(int tradeId, int rqmtId);

    public delegate string EditTradeRqmtDelegate();

    public delegate void FinalizeInboundDocumentDelegate(int tradeId, int rqmtId);

    public delegate TransmitDestination GetCptyFaxNoDelegate(string ACptySn, string ACdtyCode, string AInstType);

    public delegate SummaryData GetTradeSummaryDataRecDelegate(int tradeId);

    public delegate void MergeInboundFilesDelegate(string parrentFile, string assFile);

    public delegate void SetTifEditorCanEditDelegate(bool canEdit);

    public delegate void SetTifEditorSaveAsFileName(string saveAsFileName);


    public partial class InboundPnl : UserControl, IInbound
    {
        private const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string FORM_NAME = "InboundPnl";
        private const string FORM_ERROR_CAPTION = "Inbound Panel Form Error";
        private static readonly ILog Logger = LogManager.GetLogger(typeof(InboundPnl));        
        private static readonly string INBOUNDDOCSVIEW = "InboundDocsView";
        private static readonly string ASSOCIATEDDOC = "AssociatedDoc";
        private static readonly string BDTACPTYLKUP = "BdtaCptyLkup";
        //static private string INBOUNDDOCSVIEW = "VInboundDocsDto";
        //static private string ASSOCIATEDDOC = "VActiveAssociatedDocsDto";
        //static private string BDTACPTYLKUP = "VTradeDataCptyLkupDto";
        // inbound tabs
        private static readonly string DEFAULTDOCS = "Inbound Queue";
        private static readonly string IGNOREDDOCS = "Ignored Documents";        
        private static readonly string DISCARDEDDOCS = "Discarded Documents";
        private static readonly string PRODUCT_COMPANY = "Amphora";
        private static readonly string PRODUCT_BRAND = "Affinity";
        private static readonly string PRODUCT_GROUP = "Confirms";
        private static readonly string PRODUCT_NAME = "ConfirmManager";
        private static readonly string PRODUCT_SETTINGS = "Settings";
        private static readonly string PRODUCT_TEMP = "Temp";
        private static string PRODUCT_INBOUND = "Inbound";
        private static readonly string PRODUCT_INB_APP_CODE = "INB";
        // Associated Doc Views
        private static readonly string APPROVED_DOCS = "(DocStatusCode = '" + AssociatedDoc.APPROVED + "')";
        private static readonly string PRE_APPROVED_DOCS = "(DocStatusCode = '" + AssociatedDoc.PRELIM + "')";
        private static readonly string DISPUTED_DOCS = "(DocStatusCode = '" + AssociatedDoc.DISPUTED + "')";

        private static readonly string AUTO_ASSOCIATED_DOCS =
            "(DocStatusCode = 'ASSOCIATED' AND AssociatedBy = 'AUTO ASSOCIATE')";

        private static readonly string UNRESOLVED_DOCS = "(DocStatusCode = '" + AssociatedDoc.UNASSOCIATED + "')";
        private static readonly string VAULTED_DOCS = "(DocStatusCode = '" + AssociatedDoc.VAULTED + "')";
        private static string FROM_GET_ALL_QRY = "(TradeFinalApprovalFlag = 'Y')";
        // INBOUND DOC VIEWS
        private static readonly string INB_DISCARDED = "(DocStatusCode = '" + InboundDoc.DISCARDED + "')";        
        private static string INB_OPEN = "(DocStatusCode = '" + InboundDoc.OPEN + "')";
        private static readonly string INB_IGNORED = "(IgnoreFlag = '" + InboundDoc.IGNORED + "')";
        private static DataSet masterDataSet;
        //Settings
        public static string appSettingsDir = "";
        public static string appTempDir = "";
        public static string jbossWSUrlInbound = "";
        public static string p_UserId = "";
        private readonly IImagesDal mImagesDal;
        private readonly IVaulter vaulter;
        private readonly IXmitRequestDal xmitRequestDal;
        private readonly string[] requiredTables = {INBOUNDDOCSVIEW, ASSOCIATEDDOC, BDTACPTYLKUP};
        private readonly string sqlConnectionString = "";
        private string cptyFilter = "";
        private Int32 filteredInboundDocId;
        private Int32 filteredTradeId;
        //private InboundWebServices inboundWebServices = null;
        private GetLoadedDocumentFileName getLoadedDocumentFileNameDelegate;
        private bool isCacheUpdating;

        static InboundPnl()
        {
            ActiveTradeRqmt = null;
            ActiveSummaryData = null;            
        }

        public InboundPnl(string pSqlConnectionString, ref DSManager dsManager, ref DataSet masterDataSet,
            IsDocumentModifiedDelegate isDocumentModified)
        {
            CdtyCodeTbl = null;
            UserUpdateAccces = false;
            try
            {
                sqlConnectionString = pSqlConnectionString;
                DsManager = dsManager;
                InboundPnl.masterDataSet = masterDataSet;
                IsDocumentEdited = isDocumentModified;

                InitializeComponent();

                mImagesDal = new ImagesDal(pSqlConnectionString);
                vaulter = new Vaulter(pSqlConnectionString);
                xmitRequestDal = new XmitRequestDal(pSqlConnectionString);

                //Israel 12/16/2015 -- Implement configurable root directory.
                string configFileRootDir = InboundSettings.UserSettingsRootDir;

                if (String.IsNullOrEmpty(configFileRootDir))
                {
                    configFileRootDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                }
                else
                {
                    string pathUserId = GetUserNameWithoutDomain(p_UserId);
                    configFileRootDir = Path.Combine(configFileRootDir, pathUserId);
                }

                string roamingFolderLocation = Path.Combine(configFileRootDir, PRODUCT_COMPANY, PRODUCT_BRAND, PRODUCT_GROUP);

                //var roamingFolderLocation =
                //    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                //        PRODUCT_COMPANY, PRODUCT_BRAND, PRODUCT_GROUP);

                appSettingsDir = Path.Combine(roamingFolderLocation, PRODUCT_NAME, PRODUCT_SETTINGS);
                appTempDir = Path.Combine(roamingFolderLocation, PRODUCT_NAME, PRODUCT_TEMP);

                if (!Directory.Exists(appSettingsDir))
                {
                    Directory.CreateDirectory(appSettingsDir);
                }

                if (!Directory.Exists(appTempDir))
                {
                    Directory.CreateDirectory(appTempDir);
                }


                Dock = DockStyle.Fill;
                xtraTabCntrlInboundDocs.HeaderLocation = TabHeaderLocation.Left;
                xtraTabCntrlInboundDocs.HeaderOrientation = TabOrientation.Horizontal;


                //InitWebServices();
                AddInboundDocQPnl(DEFAULTDOCS);
                AddInboundDocQPnl(DISCARDEDDOCS);                
                AddInboundDocQPnl(IGNOREDDOCS);
                InitMasterDataSet();
                RegisterGrids();
                LoadGridSettings();
                barEditDocumentView.EditValue = "All";
            }
            catch (Exception ex)
            {
                //throw ex;
                throw new Exception("An error occurred while constructing the Inbound Panel." + Environment.NewLine +
                     "Error CNF-428 in " + FORM_NAME + ".InboundPnl(): " + ex.Message);
            }
        }


        public static string GetUserNameWithoutDomain(string pNameWithDomain)
        {
            string userName = "";
            int delimPos = pNameWithDomain.IndexOf(@"\");
            userName = pNameWithDomain.Substring(delimPos + 1, pNameWithDomain.Length - (delimPos + 1));
            return userName;
        }


        public static DataTable MasterInboundDocsTable
        {
            get { return masterDataSet.Tables[INBOUNDDOCSVIEW]; }
        }

        public static DataTable AssociatedDocsTable
        {
            get { return masterDataSet.Tables[ASSOCIATEDDOC]; }
        }

        public ImagesSelectedEventHandler ImagesSelectedEventHandler { get; set; }
        public Boolean UserUpdateAccces { get; set; }
        public DataTable CdtyCodeTbl { get; set; }

        public Int32 FilteredInboundDocId
        {
            get { return filteredInboundDocId; }
            set
            {
                filteredInboundDocId = value;
                var inboundDocIdFilter = "(InboundDocsId = " + value + ")";

                barEditDocumentView.EditValue = "InboundDocsId: " + value;
                SetMatchedDocumentsView(inboundDocIdFilter);
                tabCntrlMain.SelectedTabPageIndex = 1;
            }
        }

        public Int32 FilteredRqmtId { get; set; }

        public Int32 FilteredTradeId
        {
            get { return filteredTradeId; }
            set
            {
                filteredTradeId = value;
                var tradeIdFilter = "(TradeId = " + value + ")";

                //Israel 12/2/2015 -- Changed display to Ticket No
                var summaryData = GetTradeSummaryDataRec(value);
                string ticket = summaryData.TradeSysTicket;

                //barEditDocumentView.EditValue = "TradeId: " + value;
                barEditDocumentView.EditValue = "Ticket: " + ticket;

                SetMatchedDocumentsView(tradeIdFilter);
                tabCntrlMain.SelectedTabPageIndex = 1;
                var assDoc = GetActiveAssDoc();
                if (assDoc != null)
                {
                    var inboundImagesDto = mImagesDal.GetByDocId(assDoc.Id, ImagesDtoType.Associated);
                    ImagesEventManager.Instance.Raise(new ImagesSelectedEventArgs(
                        inboundImagesDto,
                        inboundImagesDto != null)
                        );
                }
                else
                {
                    ImagesEventManager.Instance.Raise(new ImagesSelectedEventArgs(null, false));
                }
            }
        }

        public static SummaryData ActiveSummaryData { get; set; }
        public static RqmtData ActiveTradeRqmt { get; set; }
        public SetTifEditorSaveAsFileName SetTifEditorSaveAsFileName { get; set; }
        public SetTifEditorCanEditDelegate SetTifEditorCanEditDelegate { get; set; }
        public MergeInboundFilesDelegate MergeInboundFilesDelegate { get; set; }
        public GetTradeSummaryDataRecDelegate GetTradeSummaryDataRecDelegate { get; set; }
        public GetCptyFaxNoDelegate GetCptyFaxNoDelegate { get; set; }
        public FinalizeInboundDocumentDelegate FinalizeInboundDocDelegate { get; set; }
        public EditTradeRqmtDelegate EditTradeRqmtDelegate { get; set; }
        public LocateTradeSummaryRecDelegate LocateTradeSummaryRecDelegate { get; set; }
        public GetActiveTradeRqmtDelegate GetActiveTradeRqmt { get; set; }
        public SubmitEditRqmt SubmitEditRqmtDelegate { get; set; }
        public ExtractPagesWithAnnotationDelegate ExtractPagesDelegate { get; set; }
        public InboundDocPagesDelegate DocPagesDelegate { get; set; }
        public PrintDocumentDelegate PrintDocumentDelegate { get; set; }
        public ImageFilenameDelegate ImageFilenameDelegate { get; set; }
        public SaveImageToFileDelegate SaveToFileDelegate { get; set; }
        public IsDocumentModifiedDelegate IsDocumentEdited { get; set; }
        public TransmitDocumentDelegate PreTransmitApplyDocChanges { get; set; }

        public virtual string AppSettingsLocation
        {
            get
            {
                //return HomeDrive + InboundPnl.settingsReader.GetSettingValue("InboundUserSettings");
                return appSettingsDir;
            }
        }

        public virtual string HomeDrive
        {
            get { return Environment.GetEnvironmentVariable("HOMEDRIVE"); }
        }

        public static DSManager DsManager { get; set; }

        public void EndGridDataUpdates()
        {
            try
            {
                gridViewMatchedDocuments.EndDataUpdate();
                foreach (InboundQTabPage tabPage in xtraTabCntrlInboundDocs.TabPages)
                {
                    tabPage.EndGridDataUpdates();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while processing Inbound Updates." + Environment.NewLine +
                       "Error CNF-429 in " + FORM_NAME + ".EndGridDataUpdates(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region IInbound Members

        public void DisplayInboundDocument()
        {
            if (IsInboundTabSelected)
            {
                IInbound inbound = (xtraTabCntrlInboundDocs.SelectedTabPage as InboundQTabPage);
                inbound.DisplayInboundDocument();
            }
            else // matched tab document will be displayed
            {
                LoadDocumentInImageControl();
            }
        }

        #endregion

        public void BeginDataGridUpdates()
        {
        }        

        private void LoadGridSettings()
        {
            try
            {
                var gridViewSettingsFile = Path.Combine(appSettingsDir, "MatchedDocuments.xml");
                //if (File.Exists(HomeDrive + InboundPnl.settingsReader.GetSettingValue("InboundUserSettings") + "gridViewMatchedDocuments.xml"))
                if (File.Exists(gridViewSettingsFile))
                {
                    gridViewMatchedDocuments.RestoreLayoutFromXml(gridViewSettingsFile);
                }

                foreach (InboundQTabPage tabPage in xtraTabCntrlInboundDocs.TabPages)
                {
                    tabPage.LoadGridSettings(tabPage.Text);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while loading the grid settings." + Environment.NewLine +
                     "Error CNF-430 in " + FORM_NAME + ".LoadGridSettings(): " + ex.Message);
            }
        }

        public void SaveGridSettings()
        {
            try
            {
                var gridViewSettingsFile = Path.Combine(appSettingsDir, "MatchedDocuments.xml");
                //gridViewMatchedDocuments.SaveLayoutToXml(HomeDrive + InboundPnl.settingsReader.GetSettingValue("InboundUserSettings") + "gridViewMatchedDocuments.xml");
                gridViewMatchedDocuments.SaveLayoutToXml(gridViewSettingsFile);

                foreach (InboundQTabPage tabPage in xtraTabCntrlInboundDocs.TabPages)
                {
                    tabPage.SaveGridSettings(tabPage.Text);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving the grid settings." + Environment.NewLine +
                     "Error CNF-431 in " + FORM_NAME + ".SaveGridSettings(): " + ex.Message);
            }
        }

        private void AddInboundDocQPnl(string caption)
        {
            try
            {
                //InboundQTabPage tabPage = new InboundQTabPage(caption, ref inboundWebServices, p_UserId);
                var tabPage = new InboundQTabPage(sqlConnectionString, caption, p_UserId);
                tabPage.InboundQPnl.CptyLkupTable = masterDataSet.Tables[BDTACPTYLKUP];
                tabPage.InboundQPnl.PrintDocumentDelegate = PrintInboundDoc;                
                tabPage.InboundQPnl.ViewAssDocsByInbDocIdDelegate = SetInboundDocIdFilterValue;
                tabPage.InboundQPnl.SubmitEditRqmtDelegate = SubmitEditRqmtFromInbound;                                
                tabPage.InboundQPnl.IsDocumentEdited = IsDocumentEdited;

                AddInboundTabPage(tabPage);
            }
            catch (Exception ex)
            {
                //throw ex;
                throw new Exception("An error occurred while setting up the Inbound Tab for: " + caption + "." + Environment.NewLine +
                     "Error CNF-432 in " + FORM_NAME + ".AddInboundDocQPnl(): " + ex.Message);
            }
        }

        public string SubmitEditRqmtFromInbound(long ATradeId, long ARqmtId, string ARqmtCode, string AStatusCode,
            DateTime AStatusDate, string AReference, string AComment, bool AUpdateLocalTableNow, bool isUnAssociated)
        {
            GetActiveTradeRqmt.Invoke();
            var currentRqmtStatus = "";
            if (ActiveTradeRqmt != null)
            {
                currentRqmtStatus = ActiveTradeRqmt.Status;
                if (isUnAssociated)
                {
                    SubmitEditRqmtDelegate.Invoke(ATradeId, ARqmtId, ARqmtCode, "EXPCT", AStatusDate, AReference,
                        AComment, AUpdateLocalTableNow);
                    return "EXPCT";
                }
                if (AssociatedDoc.RS_DISPUTED.Equals(AStatusCode))
                {
                    if (ActiveTradeRqmt.Cmt != null)
                    {
                        AComment = ActiveTradeRqmt.Cmt;
                    }
                    if (ActiveTradeRqmt.Reference != null)
                    {
                        AReference = ActiveTradeRqmt.Reference;
                    }
                    SubmitEditRqmtDelegate.Invoke(ATradeId, ARqmtId, ARqmtCode, AStatusCode, AStatusDate, AReference,
                        AComment, AUpdateLocalTableNow);
                    AStatusCode = EditTradeRqmt();
                    if (AStatusCode.Equals("CANCELLED"))
                    {
                        AStatusDate = DateTime.Now;
                        SubmitEditRqmtDelegate.Invoke(ATradeId, ARqmtId, ARqmtCode, currentRqmtStatus, AStatusDate,
                            AReference, AComment, AUpdateLocalTableNow);
                    }
                }
                else
                {
                    AStatusCode = EditTradeRqmt();
                }
            }
            return AStatusCode;
        }

        private void AddInboundTabPage(InboundQTabPage tabPage)
        {
            try
            {
                xtraTabCntrlInboundDocs.TabPages.Add(tabPage);

                if (tabPage.Text.Equals(DEFAULTDOCS))
                {
                    defaultInboundPanel = tabPage.InboundQPnl;
                }
                else if (tabPage.Text.Equals(DISCARDEDDOCS))
                {
                    discardedInboundPanel = tabPage.InboundQPnl;
                }
                else if (tabPage.Text.Equals(IGNOREDDOCS))
                {
                    ignoredDocsPanel = tabPage.InboundQPnl;
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                throw new Exception("An error occurred while setting up the Inbound Tab page: " + tabPage.Name + "." + Environment.NewLine +
                     "Error CNF-433 in " + FORM_NAME + ".AddInboundTabPage(): " + ex.Message);
            }
        }

        private void InitMasterDataSet()
        {
            try
            {
                masterDataSet.EnforceConstraints = false;
                // verify ALL required tables are present in the dataset
                for (var i = requiredTables.Length - 1; i >= 0; i--)
                {
                    if (masterDataSet.Tables[requiredTables[i]] == null)
                    {
                        throw new Exception("Table not found in Master Dataset object.  Table " + requiredTables[i] +
                                            " NOT found.");
                    }
                }
                CreateInboundAssociatedRelationship();
                SetupTabDatasources();
            }
            catch (Exception ex)
            {
                //throw ex;
                throw new Exception("An error occurred while setting up the Inbound internal data storage." + Environment.NewLine +
                     "Error CNF-434 in " + FORM_NAME + ".InitMasterDataSet(): " + ex.Message);
            }
        }

        private void RegisterGrids()
        {
            // since the data cache is done to the actual table
            var gridControl = defaultInboundPanel.MainDataGrid;
            DsManager.RegisterGridControl(ref gridControl);

            DsManager.RegisterGridControl(ref gridMatchedDocuments);
        }

        private void SetupTabDatasources()
        {
            try
            {
                SetInboundDefaultDataSource();
                SetDiscardedDocumentsDataSource();                
                SetIgnoredDocumentsDataSource();
                SetDefaultMatchedDocumentsDataSource(AUTO_ASSOCIATED_DOCS);
            }
            catch (Exception ex)
            {
                //throw ex;
                throw new Exception("An error occurred while setting up the Inbound internal data storage." + Environment.NewLine +
                     "Error CNF-435 in " + FORM_NAME + ".SetupTabDatasources(): " + ex.Message);
            }
        }

        private void SetIgnoredDocumentsDataSource()
        {
            try
            {
                var table = masterDataSet.Tables[INBOUNDDOCSVIEW];

                var dataView = new DataView(table, INB_IGNORED, "ID", DataViewRowState.CurrentRows);

                ignoredDocsPanel.DefaultView = dataView;

                ignoredDocsPanel.SetGridDataSource(ref dataView);

                ViewProperties props;
                props = ViewProperties.IgnoredView;


                ignoredDocsPanel.InitViewProperties(props);
            }
            catch (Exception ex)
            {
                //throw ex;
                throw new Exception("An error occurred while setting up the Ignored Documents internal data storage." + Environment.NewLine +
                     "Error CNF-436 in " + FORM_NAME + ".SetIgnoredDocumentsDataSource(): " + ex.Message);
            }
        }        

        private void SetDiscardedDocumentsDataSource()
        {
            try
            {
                var table = masterDataSet.Tables[INBOUNDDOCSVIEW];

                var discardedDocsView = new DataView(table, INB_DISCARDED, "ID", DataViewRowState.CurrentRows);

                discardedInboundPanel.DefaultView = discardedDocsView;

                discardedInboundPanel.SetGridDataSource(ref discardedDocsView);

                ViewProperties props;
                props = ViewProperties.DiscardedView;


                discardedInboundPanel.InitViewProperties(props);


                //GridControl gridControl = discardedInboundPanel.MainDataGrid;
                //dsManager.RegisterGridControl(ref gridControl);
            }
            catch (Exception ex)
            {
                //throw ex;
                throw new Exception("An error occurred while setting up the Discarded Documents internal data storage." + Environment.NewLine +
                     "Error CNF-437 in " + FORM_NAME + ".SetDiscardedDocumentsDataSource(): " + ex.Message);
            }
        }

        private void SetDefaultMatchedDocumentsDataSource(string viewName)
        {
            try
            {
                var table = masterDataSet.Tables[ASSOCIATEDDOC];

                var newView = new DataView(table, viewName, "ID", DataViewRowState.CurrentRows);

                gridMatchedDocuments.DataSource = newView;

                gridMatchedDocuments.ForceInitialize();
            }
            catch (Exception ex)
            {
                //throw ex;
                throw new Exception("An error occurred while setting up the Default Matched Documents internal data storage." + Environment.NewLine +
                     "Error CNF-438 in " + FORM_NAME + ".SetDefaultMatchedDocumentsDataSource(): " + ex.Message);
            }
        }

        private void SetMatchedDocumentsView(string viewName)
        {
            DataView newView = null;
            var matchedCptyFilter = cptyFilter;
            try
            {
                var table = masterDataSet.Tables[ASSOCIATEDDOC];
                if (matchedCptyFilter.Trim().Length > 0)
                {
                    matchedCptyFilter = matchedCptyFilter.Replace("MappedCptySn", "CptyShortName");
                    //                 matchedCptyFilter = matchedCptyFilter + " OR (CptyShortName is Null)";
                    matchedCptyFilter = "(" + matchedCptyFilter + ")";
                }

                if (viewName == null)
                {
                    viewName = GetInboundFaxPrefFilter();
                    newView = new DataView(table, viewName, "ID", DataViewRowState.CurrentRows);
                }
                else
                {
                    var faxFilter = GetInboundFaxPrefFilter();
                    if (faxFilter.Trim().Length > 0)
                    {
                        if (matchedCptyFilter.Trim().Length > 0)
                        {
                            viewName = viewName + " AND " + faxFilter + " AND " + matchedCptyFilter;
                        }
                        else
                        {
                            viewName = viewName + " AND " + faxFilter;
                        }
                    }
                    else
                    {
                        if (matchedCptyFilter.Trim().Length > 0)
                        {
                            viewName = viewName + " AND " + matchedCptyFilter;
                        }
                    }
                    newView = new DataView(table, viewName, "ID", DataViewRowState.CurrentRows);
                }
                gridMatchedDocuments.DataSource = newView;
            }
            catch (Exception ex)
            {
                //throw ex;
                throw new Exception("An error occurred while setting up the Default Matched Documents Grid View." + Environment.NewLine +
                     "Error CNF-439 in " + FORM_NAME + ".SetMatchedDocumentsView(): " + ex.Message);
            }
        }

        private void SetInboundDefaultDataSource()
        {
            try
            {
                var table = masterDataSet.Tables[INBOUNDDOCSVIEW];
                SetUserFlagColumns(ref table);
                var userFilter = GetInboundPrefFilter();
                defaultInboundPanel.TabFilter = userFilter;
                var dataView = new DataView(table, userFilter, "Id", DataViewRowState.CurrentRows);
                defaultInboundPanel.DefaultView = dataView;
                defaultInboundPanel.SetGridDataSource(ref dataView);
                ViewProperties props;
                props = ViewProperties.DefaultView;
                defaultInboundPanel.InitViewProperties(props);
            }
            catch (Exception ex)
            {
                //throw ex;
                throw new Exception("An error occurred while setting up the default internal data storage." + Environment.NewLine +
                     "Error CNF-440 in " + FORM_NAME + ".SetInboundDefaultDataSource(): " + ex.Message);
            }
        }

        private void SetUserFlagColumns(ref DataTable table)
        {
            DataRow drFind = null;
            var flagType = "";
            try
            {
                //getUserFlags userFlags = new getUserFlags();
                //getUserFlagRequest ufr = new getUserFlagRequest();
                //ufr.userName = (inboundWebServices.userName.Text[0]).ToUpper();
                //userFlags.userFlagRequest = ufr;
                //getUserFlagsResponse userFlagResponse = inboundWebServices.getUserFlags(userFlags);

                var inboundDocUserFlagDal = new InboundDocUserFlagDal(sqlConnectionString);
                var userFlagList = new List<InboundDocUserFlagDto>();
                userFlagList = inboundDocUserFlagDal.Get(p_UserId);

                //if (userFlagResponse.@return.userFlagData != null)
                if (userFlagList.Count > 0)
                {
                    //for (int i = 0; i < userFlagResponse.@return.userFlagData.Length; i++)
                    foreach (var data in userFlagList)
                    {
                        //drFind = table.Rows.Find(userFlagResponse.@return.userFlagData[i].inboundDocId);
                        drFind = table.Rows.Find(data.InboundDocId);
                        if (drFind != null)
                        {
                            drFind["UserComments"] = InboundDocsUserFlags.CLEAR;
                            //flagType = userFlagResponse.@return.userFlagData[i].flagType;
                            flagType = data.FlagType;
                            switch (flagType)
                            {
                                case "BOOKMARK":
                                {
                                    drFind["BookmarkFlag"] = InboundDocsUserFlags.BOOKMARK;
                                    break;
                                }
                                case "IGNORE":
                                {
                                    drFind["IgnoreFlag"] = InboundDocsUserFlags.IGNORE;
                                    break;
                                }
                                case "COMMENT":
                                {
                                    drFind["CommentFlag"] = InboundDocsUserFlags.COMMENT;
                                    //drFind["CommentUser"] = userFlagResponse.@return.userFlagData[i].comments;
                                    drFind["CommentUser"] = data.Comments;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                throw new Exception("An error occurred while setting the grid filter values." + Environment.NewLine +
                     "Error CNF-441 in " + FORM_NAME + ".SetUserFlagColumns(): " + ex.Message);
            }
        }

        private void CreateInboundAssociatedRelationship()
        {
            //DataColumn colMaster = null;
            //DataColumn colDetails = null;
            try
            {
                masterDataSet.Tables[INBOUNDDOCSVIEW].PrimaryKey = new[]
                {masterDataSet.Tables[INBOUNDDOCSVIEW].Columns["Id"]};
                masterDataSet.Tables[ASSOCIATEDDOC].PrimaryKey = new[]
                {masterDataSet.Tables[ASSOCIATEDDOC].Columns["Id"]};

                //colMaster = masterDataSet.Tables[INBOUNDDOCSVIEW].Columns["Id"];
                //colMaster.Unique = true;
                //colDetails = masterDataSet.Tables[ASSOCIATEDDOC].Columns["InboundDocsId"];
                //DataRelation relInboundAssociated;
                //relInboundAssociated = new DataRelation("InboundAssociated", colMaster, colDetails);
                //masterDataSet.Relations.Add(relInboundAssociated);
            }
            catch (Exception ex)
            {
                //throw ex;
                throw new Exception("An error occurred while setting up the Associated Docs internal storage structure." + Environment.NewLine +
                     "Error CNF-442 in " + FORM_NAME + ".CreateInboundAssociatedRelationship(): " + ex.Message);
            }
        }

        public string GetInboundFaxPrefFilter()
        {
            var fileName = AppSettingsLocation + "AutoFilter.xml";
            var reader = new XmlTextReader(fileName);
            XmlNodeType node;

            var faxFilter = "";
            var userFilter = ""; // The combined user pref filter...

            var excludeIgnoredDocs = true;

            userFilter = "(IgnoreFlag is Null) AND (DocStatusCode = 'OPEN')";

            if (File.Exists(fileName))
            {
                while (reader.Read())
                {
                    node = reader.NodeType;
                    if (node == XmlNodeType.Element)
                    {
                        if (reader.Name.Equals("FaxNo"))
                        {
                            reader.Read();
                            if (faxFilter == "")
                                faxFilter = "(SentTo = ";
                            faxFilter = faxFilter + "'" + reader.Value + "'" + " OR SentTo = ";
                        }
                        else if (reader.Name.Equals("IgnoredDocsFilter"))
                        {
                            reader.Read();
                            excludeIgnoredDocs = reader.Value.Equals("EXCLUDED");
                        }
                    }
                }

                if (faxFilter != "")
                {
                    faxFilter = faxFilter.Trim();
                    faxFilter = faxFilter.Remove((faxFilter.Length - " OR SentTo = ".Length + 1)) + ")";
                    userFilter = userFilter + "AND" + faxFilter;
                }

                userFilter = userFilter.Insert(0, "(");
                userFilter = userFilter = userFilter + ")";
            }
            reader.Close();
            return faxFilter;
        }

        public string GetInboundPrefFilter()
        {
            var fileName = AppSettingsLocation + "AutoFilter.xml";
            var reader = new XmlTextReader(fileName);
            XmlNodeType node;

            var cdtyFilter = "";
            var brkrAddFilter = "";

            var faxFilter = "";
            var cptyAddFilter = "";
            var userFilter = ""; // The combined user pref filter...

            var excludeIgnoredDocs = true;
            cptyFilter = "";

            userFilter = "(IgnoreFlag is Null) AND (DocStatusCode = 'OPEN')";

            if (File.Exists(fileName))
            {
                while (reader.Read())
                {
                    node = reader.NodeType;
                    if (node == XmlNodeType.Element)
                    {
                        if (reader.Name.Equals("FaxNo"))
                        {
                            reader.Read();
                            if (faxFilter == "")
                                faxFilter = "(SentTo = ";
                            faxFilter = faxFilter + "'" + reader.Value + "'" + " OR SentTo = ";
                        }
                        else if (reader.Name.Equals("CptyCode"))
                        {
                            reader.Read();
                            cptyAddFilter = cptyAddFilter + "(MappedCptySn >= " + "'" + reader.Value + "'" +
                                            " AND MappedCptySn <= " + "'" +
                                            GetLessThanValue(Convert.ToChar(reader.Value)) + "'" + ")" + " OR ";
                        }
                        else if (reader.Name.Equals("IgnoredDocsFilter"))
                        {
                            reader.Read();
                            excludeIgnoredDocs = reader.Value.Equals("EXCLUDED");
                        }
                        else
                        {
                            if (reader.Name.Equals("Name"))
                            {
                                reader.Read();
                                cptyFilter = cptyFilter + "MappedCptySn " + reader.Value;
                            }

                            if (reader.Name.Equals("Value"))
                            {
                                reader.Read();
                                cptyFilter = cptyFilter + " '" + reader.Value + "' AND ";
                            }
                        }
                    }
                }

                if (faxFilter != "")
                {
                    faxFilter = faxFilter.Trim();
                    faxFilter = faxFilter.Remove((faxFilter.Length - " OR SentTo = ".Length + 1)) + ")";
                    userFilter = userFilter + "AND" + faxFilter;
                }

                if (cptyFilter != "")
                {
                    cptyFilter = cptyFilter.Trim();
                    cptyFilter = cptyFilter.Remove((cptyFilter.Length - " AND ".Length + 1)) + ")";
                    cptyFilter = cptyFilter.Insert(0, " OR (MappedCptySn is Null) OR (");
                }

                if (cptyAddFilter != "")
                {
                    cptyAddFilter = cptyAddFilter.Trim();
                    cptyAddFilter = cptyAddFilter.Remove((cptyAddFilter.Length - " OR ".Length + 1));
                }

                cptyFilter = (cptyFilter.Insert(0, cptyAddFilter)).Trim();

                if (cptyFilter != "")
                {
                    if (cptyFilter.IndexOf("OR ", 0) == 0)
                    {
                        cptyFilter = cptyFilter.Remove(0, 3);
                    }

                    if (userFilter.Equals(""))
                    {
                        userFilter = cptyFilter;
                    }
                    else
                    {
                        userFilter = userFilter + " AND " + "(" + cptyFilter + ")";
                    }
                }

                userFilter = userFilter.Insert(0, "(");
                userFilter = userFilter = userFilter + ")";
            }
            reader.Close();
            return userFilter;
        }

        private string GetLessThanValue(char letter)
        {
            if (Convert.ToString(letter) != "Z")
                return Convert.ToString(alphabet[alphabet.IndexOf(letter) + 1]);
            return "Z";
        }

        private void ChangeMatchedDocsFilter(string editValue)
        {
            switch (editValue)
            {
                case "Auto Matched":
                    SetMatchedDocumentsView(AUTO_ASSOCIATED_DOCS);
                    break;
                case "Pre-Approved":
                    SetMatchedDocumentsView(PRE_APPROVED_DOCS);
                    break;
                case "Disputed":
                    SetMatchedDocumentsView(DISPUTED_DOCS);
                    break;
                case "Approved":
                    SetMatchedDocumentsView(APPROVED_DOCS);
                    break;
                case "Un-Associated":
                    SetMatchedDocumentsView(UNRESOLVED_DOCS);
                    break;
                case "Vaulted":
                    SetMatchedDocumentsView(VAULTED_DOCS);
                    break;
                case "All":
                    SetMatchedDocumentsView(null);
                    break;
            }

            //switch (combo.SelectedIndex)
            //{
            //    case 0: SetMatchedDocumentsView(AUTO_ASSOCIATED_DOCS); break;
            //    case 1: SetMatchedDocumentsView(PRE_APPROVED_DOCS); break;
            //    case 2: SetMatchedDocumentsView(DISPUTED_DOCS); break;
            //    case 3: SetMatchedDocumentsView(APPROVED_DOCS); break;
            //    case 4: SetMatchedDocumentsView(UNRESOLVED_DOCS); break;
            //    case 5: SetMatchedDocumentsView(VAULTED_DOCS); break;
            //    case 6: SetMatchedDocumentsView(null); break;
            //}

            // Enable/Disable buttons....
            //switch (cmboActiveDocumentView.SelectedIndex)
            //{
            //    case 0: btnUnAssociate.Visible = true; break;
            //    case 1: btnUnAssociate.Visible = true; break;
            //    case 2: btnUnAssociate.Visible = true; break;
            //    case 3: btnUnAssociate.Visible = true; break;
            //    case 4: btnUnAssociate.Visible = false; break;
            //}
            DisplayInboundDocument();
        }

        private void barBtnUnAssociate_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (gridViewMatchedDocuments.SelectedRowsCount > 1)
            {
                XtraMessageBox.Show("Only one document can be Un-Associated at a time.");
            }
            else
            {
                UnAssociateDocument();
            }
        }

        private void UnAssociateDocument()
        {
            try
            {
                using (new CursorBlock(Cursors.WaitCursor))
                {
                    if (gridViewMatchedDocuments.IsValidRowHandle(gridViewMatchedDocuments.FocusedRowHandle))
                    {
                        var dr = gridViewMatchedDocuments.GetDataRow(gridViewMatchedDocuments.FocusedRowHandle);
                        var assDoc = CollectionHelper.CreateObjectFromDataRow<AssociatedDoc>(dr);

                        ValidateAssociation(assDoc, true);

                        if (assDoc.TradeRqmtId > 0) // auto matched / unassociated
                        {
                            // Only Cpty and Broker paper can have 2nd check required..so, use this status code
                            assDoc.TradeRqmtId = ActiveTradeRqmt.Id;
                            assDoc.DocTypeCode = ActiveTradeRqmt.Rqmt;
                        }
                        assDoc.UnAssociate();

                        var assocDocsData = DtoViewConverter.CreateAssociatedDocsDto(assDoc);                        
                        assocDocsData.DocStatusCode = AssociatedDoc.UNASSOCIATED;
                        
                        if (assDoc.TradeRqmtId > 0)
                        {                            
                            var statusResult = SubmitEditRqmtFromInbound(assocDocsData.TradeId,
                                assocDocsData.TradeRqmtId,
                                assocDocsData.DocTypeCode, assocDocsData.RqmtStatus, DateTime.Now, "", "", true,
                                true);
                            if (statusResult == "CANCELLED")
                            {
                                return;
                            }
                        }

                        UpdateAssociatedDocumentStatus(assocDocsData);
                        var currentDto = mImagesDal.GetByDocId(assocDocsData.Id, ImagesDtoType.Associated);
                        if (currentDto == null)
                        {
                            throw new ApplicationException("CNF-481: Unable to find an image for associated document with AssocDocs Id: " +
                                                           assocDocsData.Id);
                        }
                        mImagesDal.SwitchImagesDtoType(currentDto, assocDocsData.InboundDocsId);
                        ApplyAssoicatedDocRemoveToView(assDoc);
                        ReOpenInboundDocument(assocDocsData.InboundDocsId);                        
                        
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayException("An error occurred while Unassociating an inbound document." + Environment.NewLine +
                     "Error CNF-443 in " + FORM_NAME + ".UnAssociateDocument():", ex);
            }
            finally
            {
                DisplayInboundDocument();                
            }
        }

        private static void ApplyAssoicatedDocRemoveToView(AssociatedDoc assDoc)
        {
            DataTable table = InboundPnl.AssociatedDocsTable;
            DataRow drFind = table.Rows.Find(assDoc.Id);
            if (drFind != null)
            {
                drFind.Delete();                                
            }
        }

        private void ReOpenInboundDocument(long inbDocId)
        {            
            var inboundDocsDal = new InboundDocsDal(sqlConnectionString);
            var id = Convert.ToInt32(inbDocId);
            var rowsUpdated = inboundDocsDal.UpdateStatus(id, AssociatedDoc.OPEN);

            if (rowsUpdated <= 0)
            {                
                throw new Exception("An error occurred while re-opening inbound document Id: " + inbDocId + "." + Environment.NewLine +
                     "Error CNF-444 in " + FORM_NAME + ".ReOpenInboundDocument().");
            }

            var dataRow = MasterInboundDocsTable.Rows.Find(id);
            dataRow["DocStatusCode"] = AssociatedDoc.OPEN;
        }

        private void barBtnDispute_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Perform the 2nd check and final approval here.
            if (gridViewMatchedDocuments.SelectedRowsCount > 1)
            {
                XtraMessageBox.Show("Only one document can be Disputed at a time.");
            }
            else
            {
                UpdateAssociatedDocStatusToDispute();
            }
        }

        private void UpdateAssociatedDocStatusToDispute()
        {
            try
            {
                using (new CursorBlock(Cursors.WaitCursor))
                {
                    if (gridViewMatchedDocuments.IsValidRowHandle(gridViewMatchedDocuments.FocusedRowHandle))
                    {
                        var dr = gridViewMatchedDocuments.GetDataRow(gridViewMatchedDocuments.FocusedRowHandle);
                        var assDoc = CollectionHelper.CreateObjectFromDataRow<AssociatedDoc>(dr);

                        ValidateAssociation(assDoc, false);

                        if (assDoc.TradeRqmtId <= 0) // this is an auto match.
                        {
                            ValidateRqmt();
                        }

                        // Only Cpty and Broker paper can have 2nd check required..so, use this status code
                        assDoc.TradeRqmtId = ActiveTradeRqmt.Id;
                        assDoc.DocTypeCode = ActiveTradeRqmt.Rqmt;
                        assDoc.DocStatusCode = AssociatedDoc.DISPUTED;
                        assDoc.TradeId = ActiveTradeRqmt.TradeId;

                        // Trade Summary Data
                        assDoc.BrokerShortName = ActiveSummaryData.BrokerSn;
                        assDoc.CdtyGroupCode = ActiveSummaryData.CdtyGrpCode;
                        assDoc.CptyShortName = ActiveSummaryData.CptySn;
                        assDoc.SecondValidateReqFlag = assDoc.SecondValidateReqFlag ?? "N";

                        var assocDocsData = DtoViewConverter.CreateAssociatedDocsDto(assDoc);
                        assocDocsData.RqmtStatus = assDoc.DocStatusCode;
                        assocDocsData.DocStatusCode = AssociatedDoc.DISPUTED;

                        var rqstTime = DateTime.Now;
                        var statusResult = SubmitEditRqmtFromInbound(assDoc.TradeId, assDoc.TradeRqmtId,
                            assDoc.DocTypeCode,
                            AssociatedDoc.RS_DISPUTED, rqstTime, "", "", true, false);

                        if (statusResult != "CANCELLED")
                        {
                            UpdateAssociatedDocumentStatus(assocDocsData);                            
                            ApplyAssoicatedDocChangeToView(assDoc);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayException("An error occurred while re-opening inbound document Id." + Environment.NewLine +
                     "Error CNF-445 in " + FORM_NAME + ".ReOpenInboundDocument().", ex);
            }
            finally
            {
                DisplayInboundDocument();              
            }
        }

        private void ValidateRqmt()
        {
            var validRqmt = false;
            if (ActiveTradeRqmt.Rqmt == AssociatedDoc.SEMPRA_PAPER)
            {
                foreach (var statusCode in AssociatedDoc.sempraPaperCodes)
                {
                    if (ActiveTradeRqmt.Status.Equals(statusCode))
                    {
                        throw new Exception("Unable to perform this Document Match due to Requirement status code state." + Environment.NewLine +
                             "Error CNF-446 in " + FORM_NAME + ".ValidateRqmt().");
                    }
                }
            }

            validRqmt = (
                (ActiveTradeRqmt.Rqmt == AssociatedDoc.BROKER_PAPER) ||
                (ActiveTradeRqmt.Rqmt == AssociatedDoc.CPTY_PAPER) ||
                (ActiveTradeRqmt.Rqmt == AssociatedDoc.SEMPRA_PAPER)
                );

            if (!validRqmt)
            {
                throw new Exception("Requirement to associate is NOT a valid paper requirement." + Environment.NewLine +
                     "Error CNF-447 in " + FORM_NAME + ".ValidateRqmt().");
            }
        }

        private void barBtnApprove_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Perform the 2nd check and final approval here.
            if (gridViewMatchedDocuments.SelectedRowsCount > 1)
            {
                XtraMessageBox.Show("Only one document can be approved at a time.");
            }
            else
            {
                UpdateAssociatedDocStatus(false);
            }
        }

        private void UpdateAssociatedDocStatus(bool xmitAfterAprove)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                if (gridViewMatchedDocuments.IsValidRowHandle(gridViewMatchedDocuments.FocusedRowHandle))
                {
                    var dr = gridViewMatchedDocuments.GetDataRow(gridViewMatchedDocuments.FocusedRowHandle);
                    var assDoc = CollectionHelper.CreateObjectFromDataRow<AssociatedDoc>(dr);

                    ValidateAssociation(assDoc, false);

                    if (assDoc.TradeRqmtId <= 0)
                    {
                        ValidateRqmt();
                    }

                    // Only Cpty and Broker paper can have 2nd check required..so, use this status code
                    assDoc.TradeRqmtId = ActiveTradeRqmt.Id;
                    assDoc.DocTypeCode = ActiveTradeRqmt.Rqmt;
                    assDoc.TradeId = ActiveTradeRqmt.TradeId;

                    assDoc.CdtyGroupCode = ActiveSummaryData.CdtyGrpCode;
                    assDoc.CptyShortName = ActiveSummaryData.CptySn;
                    assDoc.BrokerShortName = ActiveSummaryData.BrokerSn;

                    var assocDocsData = DtoViewConverter.CreateAssociatedDocsDto(assDoc);                    

                    var rqstTime = DateTime.Now;
                    assocDocsData.RqmtStatus = SubmitEditRqmtFromInbound(assocDocsData.TradeId,
                        assocDocsData.TradeRqmtId,
                        assocDocsData.DocTypeCode, assocDocsData.RqmtStatus, rqstTime, "", "", true, false);
                    if (assocDocsData.RqmtStatus.Equals("CANCELLED"))
                    {
                        return;
                    }

                    assocDocsData.DocStatusCode = DetermineDocStatusCode(assocDocsData);                                      
                    UpdateAssociatedDocumentStatus(assocDocsData);

                    var canXmit = isTerminalStatus(assocDocsData.RqmtStatus);
                    if (canXmit && xmitAfterAprove)
                    {
                        TransmitDocument(assDoc);
                    }                   
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayException("An error occurred while updating the Associated Documents status." + Environment.NewLine +
                     "Error CNF-448 in " + FORM_NAME + ".UpdateAssociatedDocStatus().", ex);
            }
            finally
            {
                DisplayInboundDocument();                
            }
        }

        public static InboundDocsView GetInboundDocObj(long id)
        {
            InboundDocsView inbDoc = null;
            var tbl = masterDataSet.Tables[INBOUNDDOCSVIEW];

            try
            {
                var rows = tbl.Select("Id = " + Convert.ToString(id));
                if (rows != null && rows.Length > 0)
                {
                    inbDoc = CollectionHelper.CreateObjectFromDataRow<InboundDocsView>(rows[0]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating internal data storage." + Environment.NewLine +
                     "Error CNF-449 in " + FORM_NAME + ".GetInboundDocObj(): " + ex.Message);
            }
            return inbDoc;
        }

        public static bool isTerminalStatus(string status)
        {
            var isTerminal = false;
            foreach (var statusCode in AssociatedDoc.terminalPaperCodes)
            {
                if (status.Equals(statusCode))
                {
                    return true;
                }
            }
            return isTerminal;
        }

        private void ValidateAssociation(AssociatedDoc assDoc, bool isBeingUnAssociated)
        {
            if (!isBeingUnAssociated)
            {
                var currentSelected = ImagesEventManager.Instance.CurrentSelected;
                if (currentSelected == null)
                {
                    throw new Exception("No document is currently selected." + Environment.NewLine +
                         "Error CNF-450 in " + FORM_NAME + ".ValidateAssociation().");
                }

                if (currentSelected.Type != ImagesDtoType.Associated &&
                    currentSelected.DocsId != assDoc.Id)
                {
                    throw new Exception("Display document does not match current document selected.  Please reload selected document." + Environment.NewLine +
                         "Error CNF-451 in " + FORM_NAME + ".ValidateAssociation().");
                }
            }

            if (gridViewMatchedDocuments.GetSelectedRows().Length > 1)
            {
                throw new Exception("Cannot perform this operation on multiple documents." + Environment.NewLine +
                     "Error CNF-452 in " + FORM_NAME + ".ValidateAssociation().");
            }

            if ((!gridViewMatchedDocuments.IsValidRowHandle(gridViewMatchedDocuments.FocusedRowHandle)) ||
                assDoc == null)
            {
                throw new Exception("No Associated Document is currently selected." + Environment.NewLine +
                     "Error CNF-453 in " + FORM_NAME + ".ValidateAssociation().");
            }


            if (assDoc.DocStatusCode.Equals(AssociatedDoc.ASSOCIATED)) // this is an Auto-Match user Confirmation
            {
                if (!isBeingUnAssociated)
                {
                    if (assDoc.TradeId != ActiveTradeRqmt.TradeId)
                        // we can't use rqmt id here, because its auto associated..no rqmt id is know right now, so just verify the trade id is correct.
                    {
                        var result =
                            XtraMessageBox.Show(
                                "Currently selected trade does not match the auto matched trade ID.  Do you wish to break the current match and re-assign the current document?",
                                "Re-Assign Document",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (result == DialogResult.No)
                        {
                            throw new Exception("Auto Matched Trade Id: " + assDoc.TradeId +
                                                               " Does not match currently selected Trade Id: " +
                                                               ActiveTradeRqmt.TradeId + Environment.NewLine +
                                 "Error CNF-454 in " + FORM_NAME + ".ValidateAssociation().");
                        }
                        assDoc.TradeId = ActiveTradeRqmt.TradeId;
                    }
                    if (ActiveTradeRqmt.FinalApprovalFlag.Equals("Y"))
                    {
                        throw new Exception("Please re-open Trade Id: " + assDoc.TradeId +
                                            ". No changes are allowed to a Final Approved Trade / Document." + Environment.NewLine +
                             "Error CNF-455 in " + FORM_NAME + ".ValidateAssociation().");
                    }
                }
            }

            if (assDoc.TradeRqmtId != 0)
            {
                if ((assDoc.TradeRqmtId != ActiveTradeRqmt.Id) || (ActiveTradeRqmt == null))
                {
                    throw new Exception("Currently selected trade rqmt ID " + ActiveTradeRqmt.Id +
                                        " does not match with Inbound Document: " + assDoc.TradeRqmtId + Environment.NewLine +
                        "Error CNF-456 in " + FORM_NAME + ".ValidateAssociation().");
                }
                var sumData = GetTradeSummaryDataRec(assDoc.TradeId);
                if (sumData.FinalApprovalFlag.Equals("Y"))
                {
                    throw new Exception("Please re-open Trade Id: " + assDoc.TradeId +
                                        ". No changes are allowed to a Final Approved Trade / Document." + Environment.NewLine +
                         "Error CNF-457 in " + FORM_NAME + ".ValidateAssociation().");
                }
            }
        }
        
        private void UpdateAssociatedDocumentStatus(AssociatedDocsDto pAssocDocsData)
        {            
            var associatedDocsDal = new AssociatedDocsDal(sqlConnectionString);
            var returnedId = associatedDocsDal.UpdateStatus(pAssocDocsData);

            if (returnedId <= 0)
            {
                throw new Exception("There was an error submitting the document association request." + Environment.NewLine +
                     "Error CNF-458 in " + FORM_NAME + ".UpdateAssociatedDocumentStatus().");
            }                        
        }

        private void LoadDocumentInImageControl()
        {
            AssociatedDoc assDoc = null;
            DataRow dr = null;
            barEditDefaultXmitDestination.EditValue = "";
            try
            {
                using(new CursorBlock(Cursors.WaitCursor))
                {
                    barBtnUnAssociate.Enabled = false;

                    if (gridViewMatchedDocuments.IsValidRowHandle(gridViewMatchedDocuments.FocusedRowHandle))
                    {
                        dr = gridViewMatchedDocuments.GetDataRow(gridViewMatchedDocuments.FocusedRowHandle);
                        if (dr != null)
                        {
                            assDoc = CollectionHelper.CreateObjectFromDataRow<AssociatedDoc>(dr);
                            if (assDoc != null && UserUpdateAccces)
                            {
                                barBtnUnAssociate.Enabled = !(assDoc.DocStatusCode.Equals(AssociatedDoc.UNASSOCIATED));
                            }

                            DisplayImageForAssociatedDoc(assDoc);
                            var faxNumber = GetCptyFaxNo();
                            barEditDefaultXmitDestination.EditValue = faxNumber != null ? faxNumber.Value : "";
                        }
                        else DisplayImageForAssociatedDoc(null);
                    }
                    else
                    {
                        DisplayImageForAssociatedDoc(null);
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayException("An error occurred while loading a document into the image control processor." + Environment.NewLine +
                     "Error CNF-459 in " + FORM_NAME + ".LoadDocumentInImageControl().", ex);
            }            
        }

        private static void LogAndDisplayException(string errorMessagePrefix, Exception ex)
        {
            Logger.Error("CNF-460: " + errorMessagePrefix + ex.Message, ex);
            XtraMessageBox.Show("An error occurred while performing the following process: " + errorMessagePrefix + "." + Environment.NewLine +
                   "Error CNF-460 in " + FORM_NAME + ".LogAndDisplayException(): " + ex.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void DisplayImageForAssociatedDoc(AssociatedDoc assDoc)
        {
            if (assDoc == null)
            {
                ImagesEventManager.Instance.Raise(new ImagesSelectedEventArgs(null, false));
                return;
            }

            var imageDto = mImagesDal.GetByDocId(assDoc.Id, ImagesDtoType.Associated);
            if (imageDto == null)
            {
                Logger.WarnFormat("Attempt to display non-existant image for associated doc with id = {0}", assDoc.Id);
            }
            ImagesEventManager.Instance.Raise(new ImagesSelectedEventArgs(imageDto, (imageDto != null)));
        }

        private void tabCntrlMain_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            switch (e.Page.Name)
            {
                case "tabMatchedDocuments":
                    LoadDocumentInImageControl();
                    break;
                default:
                {
                    IInbound inbound = (xtraTabCntrlInboundDocs.SelectedTabPage as InboundQTabPage);
                    inbound.DisplayInboundDocument();
                    barEditDefaultXmitDestination.EditValue = "";
                    break;
                }
            }
        }

        private void xtraTabCntrlInboundDocs_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            DisplayInboundDocument();
        }

        private void gridViewMatchedDocuments_ColumnFilterChanged(object sender, EventArgs e)
        {
            LoadDocumentInImageControl();
        }

        private void barBtnInboundPrefs_ItemClick(object sender, ItemClickEventArgs e)
        {
            DataTable dt = null;

            try
            {
                dt = GetFaxNumbers();

                var userFilterForm = new UserFilterForm();                
                userFilterForm.SetFaxNOsFromDb(dt);
                userFilterForm.ShowDialog();

                var applyChanges = userFilterForm.applyChanges;                
                if (applyChanges)
                {
                    SetInboundDefaultDataSource();
                    ChangeMatchedDocsFilter(barEditDocumentView.EditValue.ToString());
                }
            }
            catch (Exception err)
            {
                LogAndDisplayException("An error occurred while attempting to show the user preferences form." + Environment.NewLine +
                     "Error CNF-461 in " + FORM_NAME + ".barBtnInboundPrefs_ItemClick().", err);
            }
        }

        private DataTable GetFaxNumbers()
        {
            DataTable dt = null;
            try
            {
                var inbFaxNosDal = new InboundFaxNosDal(sqlConnectionString);
                var inbFaxNoList = new List<InboundFaxNosDto>();
                inbFaxNoList = inbFaxNosDal.GetAll();

                dt = new DataTable();
                dt.Columns.Add("FAXNO");
                                
                foreach (var data in inbFaxNoList)
                {
                    var dr = dt.NewRow();
                    dr[0] = data.Faxno;
                    dt.Rows.Add(dr);
                }                
            }
            catch (Exception err)
            {
                LogAndDisplayException("An error occurred while retrieving the transmission address from the database." + Environment.NewLine +
                     "Error CNF-462 in " + FORM_NAME + ".GetFaxNumbers().", err);
            }
            return dt;
        }

        public TransmitDestination TransmitDestination
        {
            get
            {
                var trimmedInputValue = barEditDefaultXmitDestination.EditValue.ToString().Trim();
                if (!trimmedInputValue.Equals(""))
                {
                    return new TransmitDestination(trimmedInputValue);
                }
                return new TransmitDestination("");
            }
        }

        public void TransmitDocument(AssociatedDoc assDoc)
        {                        
            try
            {
                var doc = assDoc ?? GetActiveAssDoc();
                if (doc == null)
                {
                    XtraMessageBox.Show("Error CNF-506 in " + FORM_NAME + ".TransmitDocument: Unable to generate Active Associated Document.");
                    return;
                }

                var sumData = GetTradeSummaryDataRec(doc.TradeId);
                var transmitter = new DocumentTransmitter(mImagesDal, vaulter, xmitRequestDal, TransmitDestination);
                transmitter.SendToGateway(doc, sumData);
                UpdateViewWithTransmitInformation(doc, transmitter);
            }
            catch (Exception ex)
            {
                LogAndDisplayException("An error occurred while transmitting the document." + Environment.NewLine +
                     "Error CNF-463 in " + FORM_NAME + ".TransmitDocument().", ex);
            }
        }

        private void UpdateViewWithTransmitInformation(AssociatedDoc doc, DocumentTransmitter transmitter)
        {
            if (transmitter.UserCanceled)
            {
                return;
            }

            doc.XmitValue = transmitter.TransmitDestination.Value;
            doc.XmitStatusCode = "Queued";

            ApplyAssoicatedDocChangeToView(doc);
        }

        private void barBtnTransmit_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {                

                if (IsAssociatedTabSelected)
                {
                    if (gridViewMatchedDocuments.GetSelectedRows().Length > 1)
                    {
                        TransmitDocuments();
                    }
                    else
                    {
                        TransmitDocument(null);
                    }
                }
            }
            catch (Exception ex)
            {
                LogAndDisplayException("An error occurred while transmitting the document." + Environment.NewLine +
                     "Error CNF-464 in " + FORM_NAME + ".barBtnTransmit_ItemClick().", ex);
            }
        }

        private void backgroundWorkerInbound_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                gridViewMatchedDocuments.BeginDataUpdate();
                foreach (InboundQTabPage tabPage in xtraTabCntrlInboundDocs.TabPages)
                {
                    tabPage.BeginDataGridUpdates();
                }

                IList<InboundDocsView> inbDataCache = new List<InboundDocsView>();
                inbDataCache = DsManager.GetInboundDocViewDataCache();

                IList<AssociatedDoc> assDocCache = new List<AssociatedDoc>();
                assDocCache = DsManager.GetAssDocDataCache();

                var tb = masterDataSet.Tables["InboundDocsView"];
                foreach (var data in inbDataCache)
                {
                    var rowFound = tb.Rows.Find(data.Id);
                    if (rowFound != null)
                    {
                        CollectionHelper.UpdateDataRowFromObject<InboundDocsView>(data, ref rowFound);
                    }
                    else
                    {
                        var newRow = tb.NewRow();
                        newRow = CollectionHelper.CreateDataRowFromObject<InboundDocsView>(data, newRow);
                        tb.Rows.Add(newRow);
                    }
                }

                var tb2 = masterDataSet.Tables["AssociatedDoc"];
                foreach (var data in assDocCache)
                {
                    var rowFound = tb.Rows.Find(data.Id);
                    if (rowFound != null)
                    {
                        CollectionHelper.UpdateDataRowFromObject<AssociatedDoc>(data, ref rowFound);
                    }
                    else
                    {
                        var newRow = tb.NewRow();
                        newRow = CollectionHelper.CreateDataRowFromObject<AssociatedDoc>(data, newRow);
                        tb.Rows.Add(newRow);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while processing data in the background." + Environment.NewLine +
                       "Error CNF-465 in " + FORM_NAME + ".backgroundWorkerInbound_DoWork(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }

        private void backgroundWorkerInbound_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void backgroundWorkerInbound_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            gridViewMatchedDocuments.EndDataUpdate();
            foreach (InboundQTabPage tabPage in xtraTabCntrlInboundDocs.TabPages)
            {
                tabPage.EndGridDataUpdates();
            }
        }

        public void PrintInboundDoc()
        {
            PrintDocumentDelegate.Invoke();
        }

        private void gridViewMatchedDocuments_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            if (IsAssociatedTabSelected)
            {
                LoadDocumentInImageControl();
            }
        }

        public SummaryData LocateTradeSummaryRec()
        {
            AssociatedDoc doc = null;
            SummaryData tradeSummaryRec = null;
            if (LocateTradeSummaryRecDelegate != null)
            {
                if (gridViewMatchedDocuments.IsValidRowHandle(gridViewMatchedDocuments.FocusedRowHandle))
                {
                    var dr = gridViewMatchedDocuments.GetDataRow(gridViewMatchedDocuments.FocusedRowHandle);
                    doc = CollectionHelper.CreateObjectFromDataRow<AssociatedDoc>(dr);
                    tradeSummaryRec = LocateTradeSummaryRecDelegate.Invoke(doc.TradeId, doc.TradeRqmtId);
                }
            }
            return tradeSummaryRec;
        }

        public void SetTradeIdFilterValue(Int32 tradeId)
        {
            FilteredTradeId = tradeId;
        }

        public void SetInboundDocIdFilterValue(Int32 inbDocId)
        {
            FilteredInboundDocId = inbDocId;
        }

        private void gridViewMatchedDocuments_ShowGridMenu(object sender, GridMenuEventArgs e)
        {
            popupMatchedDocs.ShowPopup(gridMatchedDocuments.PointToScreen(e.Point));
        }

        private void gridViewMatchedDocuments_DoubleClick(object sender, EventArgs e)
        {
            LocateTradeSummaryRec();
        }

        public string EditTradeRqmt()
        {
            if (EditTradeRqmtDelegate != null)
            {
                return EditTradeRqmtDelegate.Invoke();
            }
            return null;
        }

        private void FinalizeInboundDoc()
        {
            AssociatedDoc doc = null;
            DataRow dr = null;
            if (FinalizeInboundDocDelegate != null)
            {
                if (gridViewMatchedDocuments.IsValidRowHandle(gridViewMatchedDocuments.FocusedRowHandle))
                {
                    dr = gridViewMatchedDocuments.GetDataRow(gridViewMatchedDocuments.FocusedRowHandle);
                    doc = CollectionHelper.CreateObjectFromDataRow<AssociatedDoc>(dr);
                }
                if ((doc != null) && (doc.TradeRqmtId != 0) && (doc.TradeId != 0))
                {
                    var sumData = LocateTradeSummaryRec();
                    if (sumData != null)
                    {
                        FinalizeInboundDocDelegate.Invoke(doc.TradeId, doc.TradeRqmtId);
                    }
                }
                else
                {
                    XtraMessageBox.Show("An error occurred while attempting to finalize the document for the following values:" +
                            "Trade Id: " + doc.TradeId + ", Rqmt Id: " + doc.TradeRqmtId + Environment.NewLine +
                           "Error CNF-466 in " + FORM_NAME + ".FinalizeInboundDoc().",
                         FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void barBtnFinalize_ItemClick(object sender, ItemClickEventArgs e)
        {
            FinalizeInboundDoc();
        }

        private TransmitDestination GetCptyFaxNo()
        {
            barEditDefaultXmitDestination.EditValue = "";            
            if (GetCptyFaxNoDelegate != null)
            {
                var doc = GetActiveAssDoc();
                if (doc != null)
                {
                    var sumData = GetTradeSummaryDataRec(doc.TradeId);
                    if (sumData != null)
                    {
                        return GetCptyFaxNoDelegate.Invoke(sumData.CptySn, sumData.CdtyCode, sumData.SttlType);
                    }
                }
            }
            return new TransmitDestination(TransmitDestinationType.EMAIL, "");
        }

        private TransmitDestination GetCptyFaxNo(SummaryData sumData)
        {
            if (GetCptyFaxNoDelegate == null)
            {
                return new TransmitDestination(TransmitDestinationType.EMAIL, "");
            }

            if (sumData != null)
            {
                return GetCptyFaxNoDelegate.Invoke(sumData.CptySn, sumData.CdtyCode, sumData.SttlType);
            }
                
            return new TransmitDestination(TransmitDestinationType.EMAIL, "");
        }

        private SummaryData GetTradeSummaryDataRec(int tradeId)
        {
            try
            {
                SummaryData sumData = null;
                if (GetTradeSummaryDataRecDelegate != null)
                {
                    sumData = GetTradeSummaryDataRecDelegate.Invoke(tradeId);
                }
                return sumData;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the Trade Summary data for Trade Id: " + tradeId + "." + Environment.NewLine +
                     "Error CNF-467 in " + FORM_NAME + ".GetTradeSummaryDataRec(): " + ex.Message);
            }
        }

        public DataTable GetAssDocTable()
        {
            var table = masterDataSet.Tables[ASSOCIATEDDOC];
            return table;
        }

        public void ClearFinalApprovedTradeDocs()
        {
            var table = masterDataSet.Tables[ASSOCIATEDDOC];
            try
            {
                gridViewMatchedDocuments.BeginDataUpdate();
                var found = table.Select("TradeFinalApprovalFlag = 'Y'");
                foreach (var row in found)
                {
                    row.Delete();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while removing final approved trades from the Associated Docs grid." +
                       "Error CNF-468 in " + FORM_NAME + ".ClearFinalApprovedTradeDocs(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                gridViewMatchedDocuments.EndDataUpdate();
            }
        }

        private void barEditDocumentView_EditValueChanged(object sender, EventArgs e)
        {
            ChangeMatchedDocsFilter(barEditDocumentView.EditValue.ToString());
        }

        private void barBtnViewTradeSummaryRec_ItemClick(object sender, ItemClickEventArgs e)
        {
            LocateTradeSummaryRec();
        }

        private void barBtnClearDocsFromGetAll_ItemClick(object sender, ItemClickEventArgs e)
        {
            ClearFinalApprovedTradeDocs();
        }

        private void gridViewMatchedDocuments_DataSourceChanged(object sender, EventArgs e)
        {
            LoadDocumentInImageControl();
        }

        private void barBtnApproveAndXmit_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Perform the 2nd check and final approval here.
            if (gridViewMatchedDocuments.SelectedRowsCount > 1)
            {
                XtraMessageBox.Show("Only one document can be approved at a time.");
            }
            else
            {
                UpdateAssociatedDocStatus(true);
            }
        }

        public void ApplyUserUpdateAccess()
        {
            barBtnApprove.Enabled = UserUpdateAccces;
            barBtnDispute.Enabled = UserUpdateAccces;
            barBtnFinalize.Enabled = UserUpdateAccces;
            barBtnUnAssociate.Enabled = UserUpdateAccces;         
            barBtnApproveAndXmit.Enabled = UserUpdateAccces;            
            barEditDefaultXmitDestination.Enabled = UserUpdateAccces;
            defaultInboundPanel.ApplyUserUpdateAcces(UserUpdateAccces);
            discardedInboundPanel.ApplyUserUpdateAcces(UserUpdateAccces);            
            ignoredDocsPanel.ApplyUserUpdateAcces(UserUpdateAccces);
        }

        private void ribbonControl1_ShowCustomizationMenu(object sender, RibbonCustomizationMenuEventArgs e)
        {
            e.CustomizationMenu.ItemLinks.Clear();
            e.ShowCustomizationMenu = false;
        }

        private void Home_ShowCustomizationMenu(object sender, RibbonCustomizationMenuEventArgs e)
        {
            e.CustomizationMenu.ItemLinks.Clear();
            e.ShowCustomizationMenu = false;
        }

        #region IInbound Members

        public void PrintDocument()
        {
            throw new Exception("The method or operation is not implemented." + Environment.NewLine +
                 "Error CNF-469 in " + FORM_NAME + ".PrintDocument().");
        }

        public void DiscardDocument()
        {
            throw new Exception("The method or operation is not implemented." + Environment.NewLine +
                 "Error CNF-470 in " + FORM_NAME + ".DiscardDocument().");
        }

        public void CopyDocument()
        {
            throw new Exception("The method or operation is not implemented." + Environment.NewLine +
                 "Error CNF-471 in " + FORM_NAME + ".CopyDocument().");
        }        

        public void MapCallerReference()
        {
            throw new Exception("The method or operation is not implemented." + Environment.NewLine +
                 "Error CNF-472 in " + FORM_NAME + ".MapCallerReference().");
        }

        public void UnMapCallerReference()
        {
            throw new Exception("The method or operation is not implemented." + Environment.NewLine +
                 "Error CNF-473 in " + FORM_NAME + ".UnMapCallerReference().");
        }

        public void BookmarkDocument()
        {
            throw new Exception("The method or operation is not implemented." + Environment.NewLine +
                 "Error CNF-474 in " + FORM_NAME + ".BookmarkDocument().");
        }

        public void CreateUserComment()
        {
            throw new Exception("The method or operation is not implemented." + Environment.NewLine +
                 "Error CNF-475 in " + FORM_NAME + ".CreateUserComment().");
        }

        public void IgnoreDocument()
        {
            throw new Exception("The method or operation is not implemented." + Environment.NewLine +
                 "Error CNF-476 in " + FORM_NAME + ".IgnoreDocument().");
        }

        public void CreateDocumentComment()
        {
            throw new Exception("The method or operation is not implemented." + Environment.NewLine +
                 "Error CNF-477 in " + FORM_NAME + ".CreateDocumentComment().");
        }

        public void AssociateDocument(bool xmitDocument)
        {
            // Get the Active Inbound Tab....
            // execute its Interface to associated document.
            var inbound = xtraTabCntrlInboundDocs.SelectedTabPage as InboundQTabPage;
            inbound.AssociateDocument(xmitDocument);
        }

        public void DisputeDocument()
        {
            throw new Exception("The method or operation is not implemented." + Environment.NewLine +
                 "Error CNF-478 in " + FORM_NAME + ".DisputeDocument().");
        }

        public void LocateAndDisplayTradeRqmtDocument()
        {
            // Get the Active Inbound Tab....
            // execute its Interface to associated document.
            var table = masterDataSet.Tables[ASSOCIATEDDOC];
            if (ActiveTradeRqmt != null)
            {
                try
                {
                    var dr = table.Select("TradeRqmtId = " + ActiveTradeRqmt.Id);
                    if (dr.Length > 0)
                    {
                        var doc = CollectionHelper.CreateObjectFromDataRow<AssociatedDoc>(dr[0]);
                        if (doc != null)
                        {
                            tabCntrlMain.SelectedTabPageIndex = 1;

                            if (doc.DocStatusCode == AssociatedDoc.APPROVED)
                            {
                                //            cmboActiveDocumentView.SelectedIndex = 3;
                                barEditDocumentView.EditValue = "Approved";
                            }
                            else if (doc.DocStatusCode == AssociatedDoc.DISPUTED)
                            {
                                //        cmboActiveDocumentView.SelectedIndex = 2;
                                barEditDocumentView.EditValue = "Disputed";
                            }
                            else if (doc.DocStatusCode == AssociatedDoc.ASSOCIATED)
                            {
                                //             cmboActiveDocumentView.SelectedIndex = 0;
                                barEditDocumentView.EditValue = "Auto Matched";
                            }
                            else if (doc.DocStatusCode == AssociatedDoc.PRELIM)
                            {
                                //                cmboActiveDocumentView.SelectedIndex = 1;
                                barEditDocumentView.EditValue = "Pre-Approved";
                            }
                            else if (doc.DocStatusCode == AssociatedDoc.UNASSOCIATED)
                            {
                                barEditDocumentView.EditValue = "Un-Associated";
                                //            cmboActiveDocumentView.SelectedIndex = 4;
                            }
                            else
                            {
                                throw new Exception("Invalid Doc Status Code: " + doc.DocStatusCode);
                            }
                            gridViewMatchedDocuments.FocusedRowHandle =
                                (FindRowHandleByDataRow(gridViewMatchedDocuments, dr[0]));
                        }
                    }
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("An error occurred while locating and displaying the Trade Rqmt Id." +
                           "Error CNF-479 in " + FORM_NAME + ".LocateAndDisplayTradeRqmtDocument(): " + ex.Message,
                         FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private int FindRowHandleByDataRow(GridView view, DataRow row)
        {
            if (row != null)
                for (var i = 0; i < view.DataRowCount; i++)
                    if (view.GetDataRow(i) == row)
                        return i;
            return GridControl.InvalidRowHandle;
        }

        #endregion

        private bool IsAssociatedTabSelected
        {
            get { return tabCntrlMain.SelectedTabPageIndex == 1; }
        }

        private bool IsInboundTabSelected
        {
            get { return tabCntrlMain.SelectedTabPageIndex == 0; }
        }

        public AssociatedDoc GetActiveAssDoc()
        {            
            if (!IsAssociatedTabSelected)
            {
                return null;
            }
            
            if (gridViewMatchedDocuments.IsValidRowHandle(gridViewMatchedDocuments.FocusedRowHandle))
            {
                var dr = gridViewMatchedDocuments.GetDataRow(gridViewMatchedDocuments.FocusedRowHandle);
                if (dr != null)
                {
                    return CollectionHelper.CreateObjectFromDataRow<AssociatedDoc>(dr);
                }
            }            
            return null;                        
        }

        public void BeginGridDataUpdates()
        {
            AssociatedDoc currentFocusedAssDoc = null;

            //Israel 11/4/2015
            //int currentTabIndex = xtraTabCntrlInboundDocs.SelectedTabPageIndex;
            int currentTabIndex = tabCntrlMain.SelectedTabPageIndex;
            XtraTabPage currentTab = tabCntrlMain.SelectedTabPage;
            try
            {                
                isCacheUpdating = true;
                gridViewMatchedDocuments.FocusedRowChanged -= gridViewMatchedDocuments_FocusedRowChanged;

                gridViewMatchedDocuments.BeginDataUpdate();

                currentFocusedAssDoc = GetActiveAssDoc();

                foreach (InboundQTabPage tabPage in xtraTabCntrlInboundDocs.TabPages)
                {
                    tabPage.BeginDataGridUpdates();
                }

                IList<InboundDocsView> inbDataCache = new List<InboundDocsView>();
                inbDataCache = DsManager.GetInboundDocViewDataCache();

                IList<AssociatedDoc> assDocCache = new List<AssociatedDoc>();
                assDocCache = DsManager.GetAssDocDataCache();

                var tb = masterDataSet.Tables["InboundDocsView"];
                foreach (var data in inbDataCache)
                {
                    var rowFound = tb.Rows.Find(data.Id);
                    if (rowFound != null)
                    {
                        // WE NEED TO KEEP ALL CURRENT USER FLAG SETTINGS FOR THIS ROW.....IT GETS OVERWRITTEN ON APPLY UPDATES..
                        // BECAUSE THESE CHANGES DO NOT GET EMS MESSAGES...
                        
                        if (rowFound["BookmarkFlag"].ToString().Equals("BOOKMARK"))
                        {
                            data.BookmarkFlag = rowFound["BookmarkFlag"].ToString();
                        }
                        if (rowFound["IgnoreFlag"].ToString().Equals("IGNORE"))
                        {
                            data.IgnoreFlag = rowFound["IgnoreFlag"].ToString();
                        }

                        if (rowFound["CommentFlag"].ToString().Equals("COMMENT"))
                        {
                            data.CommentUser = rowFound["CommentUser"].ToString();
                            data.CommentFlag = rowFound["CommentFlag"].ToString();
                        }
                        
                        CollectionHelper.UpdateDataRowFromObject<InboundDocsView>(data, ref rowFound);
                    }
                    else
                    {
                        var newRow = tb.NewRow();
                        newRow = CollectionHelper.CreateDataRowFromObject<InboundDocsView>(data, newRow);
                        tb.Rows.Add(newRow);
                    }
                }

                var tb2 = masterDataSet.Tables["AssociatedDoc"];
                foreach (var data in assDocCache)
                {
                    var rowFound = tb2.Rows.Find(data.Id);
                    if (rowFound != null)
                    {
                        if ("N".Equals(data.TradeFinalApprovalFlag))
                        {
                            CollectionHelper.UpdateDataRowFromObject<AssociatedDoc>(data, ref rowFound);
                        }
                        else // this associated doc's trade has been final approved...get it off the grid..
                        {
                            tb2.Rows[tb2.Rows.IndexOf(rowFound)].Delete();
                        }
                    }
                    else
                    {
                        var newRow = tb2.NewRow();
                        newRow = CollectionHelper.CreateDataRowFromObject<AssociatedDoc>(data, newRow);
                        tb2.Rows.Add(newRow);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while beginning the Inbound data grid updates." +
                       "Error CNF-480 in " + FORM_NAME + ".BeginGridDataUpdates(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (currentFocusedAssDoc != null)
                {
                    var tbl5 = masterDataSet.Tables["AssociatedDoc"];
                    var found = tbl5.Select("Id = " + currentFocusedAssDoc.Id);
                    if (found != null && found.Length > 0)
                    {
                        var col = gridViewMatchedDocuments.Columns["Id"];
                        var rowHandle = gridViewMatchedDocuments.LocateByValue(0, col, currentFocusedAssDoc.Id);
                        if ((rowHandle >= 0))
                        {
                            gridViewMatchedDocuments.ClearSelection();
                            gridViewMatchedDocuments.SelectRow(rowHandle);
                            gridViewMatchedDocuments.FocusedRowHandle = rowHandle;
                        }
                    }
                }

                gridViewMatchedDocuments.EndDataUpdate();
                gridViewMatchedDocuments.FocusedRowChanged += gridViewMatchedDocuments_FocusedRowChanged;
                foreach (InboundQTabPage tabPage in xtraTabCntrlInboundDocs.TabPages)
                {
                    tabPage.EndGridDataUpdates();
                }
                DisplayInboundDocument();
                isCacheUpdating = false;
            }
        }

        public void TransmitDocuments()
        {
            IList<AssociatedDoc> failedDocs = new List<AssociatedDoc>();
            IList<AssociatedDoc> xmitDocs = new List<AssociatedDoc>();
            
            foreach (var rowHandle in gridViewMatchedDocuments.GetSelectedRows())
            {
                gridViewMatchedDocuments.FocusedRowHandle = rowHandle;
                var dr = gridViewMatchedDocuments.GetDataRow(rowHandle);
                
                if (dr != null)
                {
                    var doc = CollectionHelper.CreateObjectFromDataRow<AssociatedDoc>(dr);
                    if (doc != null)
                    {
                        var sumData = GetTradeSummaryDataRec(doc.TradeId);
                        var transmitDest = GetCptyFaxNo(sumData);

                        if (transmitDest == null || !transmitDest.IsValid)
                        {
                            failedDocs.Add(doc);
                        }
                        else
                        {              
                            var transmitter = new DocumentTransmitter(mImagesDal, vaulter, xmitRequestDal, transmitDest);
                            transmitter.SendToGateway(doc, sumData);
                            xmitDocs.Add(doc);
                        }
                    }
                }
            }
            XtraMessageBox.Show(failedDocs.Count + " documents were not able to be transmitted due to no default fax or email address associated with documents. " + xmitDocs.Count +
                                " documents were actually sent.");
            gridViewMatchedDocuments.ClearSelection();
            foreach (var assDoc in failedDocs)
            {
                SelectAssociatedDoc(assDoc);
            }
        }

        private void SelectAssociatedDoc(AssociatedDoc doc)
        {
            var col = gridViewMatchedDocuments.Columns["Id"];
            var rowHandle = gridViewMatchedDocuments.LocateByValue(0, col, doc.Id);
            if (rowHandle != GridControl.InvalidRowHandle)
            {
                gridViewMatchedDocuments.SelectRow(rowHandle);
            }
        }

        public void SetTifEditorSaveAsFileNameDelegate(string saveAsFileName)
        {
            if (SetTifEditorSaveAsFileName != null)
            {
                SetTifEditorSaveAsFileName.Invoke(saveAsFileName);
            }
        }

        internal static string DetermineDocStatusCode(AssociatedDocsDto assocDocsData)
        {
            if (isTerminalStatus(assocDocsData.RqmtStatus))
            {
                return AssociatedDoc.APPROVED;
            }

            if (assocDocsData.RqmtStatus.Equals(AssociatedDoc.RS_DISPUTED))
            {
                return AssociatedDoc.DISPUTED;
            }

            return AssociatedDoc.ASSOCIATED;
        }

        internal static void ApplyAssoicatedDocChangeToView(AssociatedDoc assDoc)
        {
            DataTable table = InboundPnl.AssociatedDocsTable;
            DataRow drFind = table.Rows.Find(assDoc.Id);
            if (drFind == null)
            {
                DataRow dr = table.NewRow();
                dr = CollectionHelper.CreateDataRowFromObject<AssociatedDoc>(assDoc, dr);
                table.Rows.Add(dr);
            }
            else
            {
                CollectionHelper.UpdateDataRowFromObject<AssociatedDoc>(assDoc,ref drFind);
            }
        }
    }
}