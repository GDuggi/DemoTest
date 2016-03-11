
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using DataManager;
using DevExpress.Skins;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using ConfirmInbound;
//using Microsoft.Office.Interop.Word;
using OpsTrackingModel;
using Sempra.Confirm.InBound.ImageEdit;
using System.Xml;
//using Oracle.DataAccess.Client;
using Sempra.Ops;
using CommonUtils;
using DBAccess.SqlServer;
using DBAccess;
using VaultUtils;
using DevExpress.XtraRichEdit;
using WSAccess;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.API.Native;
using log4net;
using System.Transactions;
using DevExpress.XtraVerticalGrid.Events;
using System.Globalization;
using System.Linq;

namespace ConfirmManager
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private static ILog Logger = LogManager.GetLogger(typeof(frmMain));
        private const string FORM_NAME = "frmMain";
        private const string DOCK_MGR_SETTINGS = "DockManager.xml";
        private string BROWSER_BAR_SETTINGS = "BrowserBarManager.xml";
        private const string SUMMARY_GRID_SETTINGS = "SummaryGrid.xml";
        private const string RQMT_GRID_SETTINGS = "RqmtGrid.xml";
        private const string CONFIRM_GRID_SETTINGS = "ConfirmGrid.xml";
        private const string INBOUND_GRID_SETTINGS = "InboundGrid.xml";
        private const string LOAD_SEMPRA_COMPANY = "LoadSempraCompany";
        private const string LOAD_CDTY_GROUP = "LoadCdtyGroup";
        private const string SEMPRA_RQMT = "XQCSP";
        private const string CPTY_RQMT = "XQCCP";
        private const string BKR_RQMT = "XQBBP";
        private const string NOCONF_RQMT = "NOCNF";
        private const string VERBL_RQMT = "VBCP";

        //private const string PROD_DB_NAME = "sempra.prod";
        private const string CONFIRM_LABEL_CONFIRM = "CONFIRM";
        private const string PDF_METAMORPHOSIS_SERIAL = "10048306154";
        private const string JP_IND = "J";
        private const string SEMPRA_IND = "S";
        private const string ALL_COMP_IND = "B";
        private const string PRODUCT_COMPANY = "Amphora";
        private const string PRODUCT_BRAND = "Affinity";
        private const string PRODUCT_GROUP = "Confirms";
        private const string PRODUCT_NAME = "ConfirmManager";
        private const string PRODUCT_SETTINGS = "Settings";
        private const string PRODUCT_DATA = "Data";
        private const string PRODUCT_TEMP = "Temp";
        private const string PRODUCT_FAX = "Fax";
        private const string INBOUND_PNL_SETTINGS = "ConfirmInbound.dll.config";
        private const string HORNETQ_PROPS =
            "?transport.UseInactivityMonitor=false&connection.MaxInactivityDuration=0&transport.ishornetqclient=true";
        private const string APP_NAME = "Confirm Manager";
        private const string MAIN_FORM_ERROR_CAPTION = "Main Form Error";
        private const string MAIN_FORM_STOP_CAPTION = "Main Form Stop Error";

        //private StringCollection serverList;
        //private int sempraCompanyId;
        private int finalApprovalFilterIndex;
        private int invalidFocusedRowHandle = DevExpress.XtraGrid.GridControl.InvalidRowHandle;
        //private int savedRowHandle = -1;
        //private string sempraCompanySn;
        private string toolbarOrWindowsUserId;
        //private string commandLineUserId;
        private string[] commandLineArgs;
        private string[] barChkFAHints;
        private string webServiceFilter = "";
        private string inbServiceFilter = "";
        private string companyAccessInd = "";
        private string[] filterBookList = null;
        private string[] inboundFilterSentToList; // used to implement company security
        private Boolean gsActive;
        private Boolean initComplete;
        private Boolean isFinalApprove = false;       //FNAPP
        private Boolean isForceFinalApprove = false;  //FNAPP-OVR
        private Boolean isHasAccess = false;            //ACCESS
        private Boolean isHasUpdate = false;            //UPDATE
        //private Boolean isReCalcPriorities = false;     //RECALCPRI
        private Boolean isSecondCheckCreateCxl = false; //SC-CRCXL
        private Boolean isSubmitQueuedEFETTrades = false; //SUB-EFET
        private Boolean isContractApprove = false; //CNTRCT-APP
        private bool isAutoDispDealsheet = false;
        //private bool isTestMode = false;
        //private bool isSaveToNewExcelFormat = false;
        private DSManager dataManager;
        public DataSet dataSet;
        private System.Data.DataTable summaryDataTable;
        private System.Data.DataTable rqmtDataTable;
        private System.Data.DataTable confirmDataTable;
        private System.Data.DataTable rqmtStatusColorTable;
        private System.Data.DataTable colorTranslateTable;
        private System.Data.DataTable userRoleView;
        private System.Data.DataTable userFiltersView;
        private System.Data.DataTable rqmtStatusView;
        private System.Data.DataTable rqmtView;
        private System.Data.DataTable clauseHeaderTable;
        private System.Data.DataTable clauseBodyTable;
        private System.Data.DataTable cdtyLkupTbl = null;
        private System.Data.DataTable cptyLkupTbl = null;
        private System.Data.DataTable seCptyLkupTbl = null;
        private System.Data.DataTable gridLookupContractListTable = null;
        private System.Data.DataTable inboundDocDataTable;
        private System.Data.DataTable assocDocDataTable;

        private StringCollection getAllList = new StringCollection();
        private ArrayList noEditSempraRqmtStatus;
        private readonly int CHECKED = 32;
        private readonly int UNCHECKED = 33;
        //private readonly int TRAFFIC_GREEN = 29;
        //private readonly int TRAFFIC_RED = 30;
        private readonly int NO_FILTER_VAL = 0;
        private readonly string HTTP_DELIM = "|";
        private Int32 checkPrint;
        private Int32 viewContractTradeId = 0;
        private Int32 viewContractConfirmId = 0;
        private Int32 viewContractTemplateId = 0;
        private String[] reportsURL;
        private System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer(); //Israel 12/15/2008 - Red X

        public frmEditRqmt editRqmtForm = new frmEditRqmt();
        public frmAddRqmt addRqmtForm = new frmAddRqmt();
        //public frmEditContract editContractForm = new frmEditContract();
        public frmEditRichContract editContractForm = new frmEditRichContract();
        public frmGroupTrades groupTradesForm = new frmGroupTrades();
        public frmAbout aboutForm = new frmAbout();
        //public frmSplash splashForm = new frmSplash();
        public frmAudit auditForm = new frmAudit();
        public frmXmitResultStatusLog xmitResultStatusLogForm = new frmXmitResultStatusLog();
        public frmTradeDataChanges tradeDataChangesForm = new frmTradeDataChanges();
        public frmGetAll getAllForm = new frmGetAll();
        public frmChangeUserFilter changeUserFilterForm = new frmChangeUserFilter();
        public frmUserPrefs userPrefsForm = new frmUserPrefs();
        public frmSetECMatched updECMatched = new frmSetECMatched();
        public frmTemplateList templateListForm = new frmTemplateList();
        public frmCancelRqmt cancelRqmtForm = new frmCancelRqmt();
        public frmEMailInput eMailInputForm = new frmEMailInput();
        public frmAddMultiRqmt addMultiRqmtForm = new frmAddMultiRqmt(); //Not Used
        public frmFaxCoverPageInput faxCoverPageInputForm = new frmFaxCoverPageInput();

        // Inbound 
        private InboundPnl inboundPnl1 = null;
        private GridHitInfo downHitInfo = null;
        private Sempra.Confirm.InBound.ImageEdit.TifEditor tifEditorInbound;
        private System.Data.DataTable tblPhrases = null;
        private System.Data.DataTable tblUserMappings = null;
        private StringCollection serverList;
        private string appSettingsDir = "";

        //Settings variables
        private string appTempDir = "";
        private string tempFaxDir;
        private string dataDir = "";
        private string saveToExcelDirectory = "";
        private string appDocumentsDir = "";

        public SqlConnection dbConnection;
        public string sqlConnectionStr;
        //public string sqlConnectionStrIntegratedSecurity;
        public string xmlSchemaPath;
        //public JBossVersion currentJBossVersion = 0;
        //public string jbossWSUrlConfirmation = "";
        //public string jbossWSUrlConfirmUtil = "";
        //public string jbossWSUrlInbound = "";
        //public string jbossWSUrlTradeConfirm = "";
        public static string p_UserId = "";
        private string v_UserId = "";
        private string v_Server = "";
        private string v_Database = "";
        //public string p_Password = "";
        //public string p_DatabaseName = "";
        private List<string> v_PermKeyList = new List<string>();
        private bool v_IsSuperUser = false;
        private string v_PermKeyDBInClauseStr = String.Empty;
        private bool v_IsPermKeyOverride = false;
        private List<string> v_PermKeyOverrideList = new List<string>();

        public readonly string[] DOC_TYPE = new string[] { "CNF", "INB" };
        public readonly string[] FAX_CODE = new string[] { "EMAIL", "FAX", "TELEX" };

        int processId = 0;

        #region Enum declarations

        enum CGMethod
        {
            GetContract = 0, GetFilledTemplate, GetTradeData, GetTokenValue, GetXMLData, GetValueSheet,
            GetTradeDataByField, GetTemplateList
        };

        enum SendMethod
        {
            SendPaper = 0, ManualSend, SetToOkToSendOnly, SendWithCoverPage
        };

        enum DocSendType
        {
            PDF = 0, RTF
        };

        enum DetermineActionsType
        {
            ResetToNo = 0, ResetToReprocess
        };

        public enum JBossVersion
        {
            JBoss423 = 0, JBossEAP6
        };

        public enum DocType
        {
            CNF = 0, INB
        }

        public enum FaxCode
        {
            EMAIL = 0, FAX, TELEX
        }

        public enum RequestType
        {
            Insert = 0, Update, Delete
        }

        #endregion

        #region System startup

        public frmMain(string[] args)
        {
            string currentlLoadProccess = "";
            string errorMessageText = String.Empty;
            try
            {
                Program.splashForm.ShowLoadProgress("Loading saved settings...");
                errorMessageText = "The error occurred while the system was establishing user setting directories.";
                Sempra.frmLogin dbLogin = null;

                p_UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

                //Israel 12/16/2015 -- Implement configurable root directory.
                string configFileRootDir = Properties.Settings.Default.UserSettingsRootDir;

                if (String.IsNullOrEmpty(configFileRootDir))
                {
                    configFileRootDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                }
                else
                {
                    string pathUserId = Utils.GetUserNameWithoutDomain(p_UserId);
                    configFileRootDir = Path.Combine(configFileRootDir, pathUserId);
                }

                string roamingFolderLocation = Path.Combine(configFileRootDir, PRODUCT_COMPANY, PRODUCT_BRAND, PRODUCT_GROUP);

                //string roamingFolderLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                //    PRODUCT_COMPANY, PRODUCT_BRAND, PRODUCT_GROUP);

                appSettingsDir = Path.Combine(roamingFolderLocation, PRODUCT_NAME, PRODUCT_SETTINGS);
                InboundSettings.AppSettingsDir = appSettingsDir;
                dataDir = Path.Combine(roamingFolderLocation, PRODUCT_NAME, PRODUCT_DATA);
                appTempDir = Path.Combine(roamingFolderLocation, PRODUCT_NAME, PRODUCT_TEMP);
                tempFaxDir = Path.Combine(appTempDir, PRODUCT_FAX);

                string documentsFolderLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    PRODUCT_COMPANY, PRODUCT_BRAND, PRODUCT_GROUP);
                appDocumentsDir = Path.Combine(documentsFolderLocation, PRODUCT_NAME);

                TifEditor.licenseFile = Properties.Settings.Default.LeadtoolsLicenseFile;
                TifEditor.developerKey = Properties.Settings.Default.LeadtoolsDeveloperKey;
                TifEditor.SignatureLocation = Properties.Settings.Default.InboundSignatureLocation;

                currentlLoadProccess = "DeleteConfigFiles";
                errorMessageText = "The error occurred while the system was deleting old setting files.";
                DeleteConfigFiles();

                currentlLoadProccess = "InitializeComponent";
                errorMessageText = "The error occurred while initializing the main data entry form.";
                InitializeComponent();

                currentlLoadProccess = "InstantiateTifEditor";
                errorMessageText = "The error occurred while preparing the Inbound Image Editor and applying the third-party license.";
                InstantiateTifEditor();

                //Israel 12/15/2008 - Red X
                myTimer.Tick += new EventHandler(TimerEventProcessor);
                myTimer.Interval = ((Properties.Settings.Default.UpdateFromCacheTimerIntervalSecs) * 1000);

                commandLineArgs = args;
                dbConnection = new SqlConnection();
                //serverList = Properties.Settings.Default.Servers;
                serverList = new StringCollection();
                serverList.Add(Properties.Settings.Default.MSSQLServer);

                xmlSchemaPath = appSettingsDir;

                dbLogin = new Sempra.frmLogin(
                    true,
                    5,
                    appSettingsDir,
                    appSettingsDir,
                    serverList);

                bool loginOk = true;
                //@"Server=CNF01INFDBS01\SQLSVR11;Database=cnf_mgr;integrated security=sspi;";

                currentlLoadProccess = APP_NAME + " logon with no command line parameter or a partial or mal-formed parameter.";
                dbLogin.Server = Properties.Settings.Default.MSSQLServer;
                dbLogin.Database = Properties.Settings.Default.MSSQLDatabase;
                //sqlConnectionStr = Utils.GetConnStrIntegratedSecurity(dbLogin.Server, dbLogin.Database);

                //string userName = Utils.GetParamValue("n", args);
                //string password = Utils.GetParamValue("p", args);
                string server = Utils.GetParamValue("s", args);
                string database = Utils.GetParamValue("d", args);

                if (!String.IsNullOrEmpty(server))
                    dbLogin.Server = server;
                if (!String.IsNullOrEmpty(database))
                    dbLogin.Database = database;

                //Israel 11/20/2015 -- Add command line override for testing permission keys.
                string permKeysParmStr = Utils.GetParamValue("k", args);
                errorMessageText = "An error occurred while reading the command line -k parameter of: " + permKeysParmStr;
                if (permKeysParmStr.Length > 1)
                {
                    v_IsPermKeyOverride = true;
                    string[] permKeysArray = permKeysParmStr.Split(',');
                    foreach (string permKey in permKeysArray)
                        v_PermKeyOverrideList.Add(permKey);
                }

                sqlConnectionStr = Utils.GetConnStrIntegratedSecurity(dbLogin.Server, dbLogin.Database);
                currentlLoadProccess = APP_NAME + " logon using parameters: server=" + server + " ; database=" + database;
                errorMessageText = "An error occurred while logging onto the database using these parameters: server=" + server + " ; database=" + database;

                //Israel 10/12/2015 -- Deprecate test mode
                //string testMode = Utils.GetParamValue("t", args);
                //if (testMode.Length > 1)
                //    if (testMode.ToUpper() == "TRUE")
                //        isTestMode = true;

                //currentlLoadProccess = "DocFlow logon using parameters: userName=" + userName + "; server=" + server + " ; database=" + database;
                //dbConnection.ConnectionString = Utils.GetConnectionStr(
                //    Utils.DBMSType.SQLServer,
                //    userName,
                //    password,
                //    server,
                //    database);

                //dbConnection.Open();
                //dbLogin.UserName = userName;
                //dbLogin.Password = password;

                //sqlConnectionStr = dbConnection.ConnectionString + "Password=" + password + ";";
                //}

                //sqlConnectionStr = Utils.GetConnStrIntegratedSecurity(dbLogin.Server, dbLogin.Database);

                if (!loginOk)
                {
                    Environment.Exit(-1);
                }

                dbConnection.ConnectionString = sqlConnectionStr;
                dbConnection.Open();
                if (dbConnection.State != ConnectionState.Open)
                {
                    throw new Exception("Database logon failed. Please verify your credentials exist and that the database is available.");

                }

                //sqlConnectionStr = sqlConnectionStr + "MultipleActiveResultSets=True;";
                //sqlConnectionStrIntegratedSecurity = sqlConnectionStrIntegratedSecurity + "MultipleActiveResultSets=True;";
                //p_UserId = dbLogin.UserName;
                v_Server = dbLogin.Server;
                v_Database = dbLogin.Database;

                //currentlLoadProccess = "Sempra.Ops.Utils.GetParamValue";
                Control.CheckForIllegalCrossThreadCalls = false;

                initComplete = false;
                gsActive = false;

                getAllForm.sqlConnectionStr = sqlConnectionStr;

                if (v_IsPermKeyOverride)
                    XtraMessageBox.Show("Permission Key override using the following keys: " + permKeysParmStr,
                        "Main Form Startup", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception except)
            {
                Program.splashForm.Close();

                XtraMessageBox.Show("Error occurred while executing: " + currentlLoadProccess + ". " + Environment.NewLine +
                    errorMessageText + Environment.NewLine +
                    "Error CNF-001 in " + FORM_NAME + ".frmMain(): " + except.Message,
                        MAIN_FORM_STOP_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Environment.Exit(-1);
            }
        }

        private void DeleteConfigFiles()
        {
            // open up the delete file and get a "code"
            // check this code against the .ini file
            // if the .ini file code <> "code", we delete the dockmanager.xml file
            // then set the .ini code value to "code".

            string appPath = Path.Combine(System.Windows.Forms.Application.StartupPath, "delete.ini");
            //string settingsDir = settingsDir = tempDir = Environment.GetEnvironmentVariable("HOMEDRIVE").ToString();
            //settingsDir += Properties.Settings.Default.AppSettingsDir + Properties.Settings.Default.AppName + "\\";

            //Israel 2/24/2014
            string deleteIniFileAndPath = Path.Combine(appSettingsDir, "delete.ini");
            string dockManagerFileAndPath = Path.Combine(appSettingsDir, "DockManager.xml");

            if (File.Exists(appPath))
            {
                if (File.Exists(deleteIniFileAndPath))
                {
                    if (CompareFiles(appPath, (deleteIniFileAndPath)) == false)
                    {
                        if (File.Exists(dockManagerFileAndPath))
                        {
                            File.Delete(dockManagerFileAndPath);
                        }
                        File.Copy(appPath, (deleteIniFileAndPath), true);
                    }
                }
                else
                {
                    if (File.Exists(dockManagerFileAndPath))
                    {
                        File.Delete(dockManagerFileAndPath);
                        File.Copy(appPath, (deleteIniFileAndPath));
                    }
                }
            }
        }

        private bool CompareFiles(string File1, string File2)
        {
            FileInfo FI1 = new FileInfo(File1);
            FileInfo FI2 = new FileInfo(File2);

            if (FI1.Length != FI2.Length)
                return false;

            byte[] bytesFile1 = File.ReadAllBytes(File1);
            byte[] bytesFile2 = File.ReadAllBytes(File2);

            if (bytesFile1.Length != bytesFile2.Length)
                return false;

            for (int i = 0; i <= bytesFile2.Length - 1; i++)
            {
                if (bytesFile1[i] != bytesFile2[i])
                    return false;
            }
            return true;
        }

        private void InstantiateTifEditor()
        {
            this.tifEditorInbound = new Sempra.Confirm.InBound.ImageEdit.TifEditor();
            this.tabpgImageViewer.Controls.Add(this.tifEditorInbound);

            this.tifEditorInbound.Dock = System.Windows.Forms.DockStyle.Fill;
            //      this.tifEditorInbound.Edit = false;
            this.tifEditorInbound.ExitDelegate = null;
            this.tifEditorInbound.ImageFileName = null;
            this.tifEditorInbound.Location = new System.Drawing.Point(0, 0);
            this.tifEditorInbound.Name = "tifEditorInbound";
            this.tifEditorInbound.SaveAsFileName = null;
            this.tifEditorInbound.ScaleFactor = 1;
            this.tifEditorInbound.Size = new System.Drawing.Size(410, 431);
            this.toolTipController.SetSuperTip(this.tifEditorInbound, null);
            this.tifEditorInbound.TabIndex = 0;
            this.tifEditorInbound.TransDelegate = null;
            this.tifEditorInbound.UserName = null;
            tifEditorInbound.SaveImageOverrideDelegate = SaveImageOverride;
            tifEditorInbound.GetImageDataOverride = GetImageBytesOverride;

        }

        private void rfrmMain_Load(object sender, EventArgs e)
        {
            initSkinsMenu();
            ReadAppSettings();
            ReadUserSettings();

            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.Skins.SkinManager.EnableMdiFormSkins();

            string errorMessageText = String.Empty;

            try // get the user companies setup.
            {
                //Israel 12/4/14 - Static and local both necessary
                errorMessageText = "The error occurred while assigning internal access settings for the user: " + p_UserId;
                this.v_UserId = p_UserId;
                toolbarOrWindowsUserId = p_UserId;
                InboundPnl.p_UserId = p_UserId;
                //axPnl1.p_UserId = p_UserId;
                AXPnl.sqlConnectionString = sqlConnectionStr;
                frmEditContract.p_UserId = p_UserId;

                seCptyLkupTbl = new System.Data.DataTable();
                seCptyLkupTbl.Columns.Add(new DataColumn("BookingCoSn", typeof(string)));
                seCptyLkupTbl.PrimaryKey = new DataColumn[] { seCptyLkupTbl.Columns["BookingCoSn"] };

                errorMessageText = "The error occurred while reading the companies using MSSql connection string: " + sqlConnectionStr;
                GetCompanies();
                //if (seCptyLkupTbl.Rows.Count <= 0)
                //{
                //    XtraMessageBox.Show("rfrmMain_Load[1]: There are no trades for which this user has been granted access. Please contact Help Desk.",
                //    "Stop", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //    Environment.ExitCode = -1;
                //    System.Windows.Forms.Application.Exit();
                //}
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while getting the company list from the database." + Environment.NewLine +
                    errorMessageText + Environment.NewLine +
                    "Error CNF-002 in " + FORM_NAME + ".rfrmMain_Load(): " + ex.Message,
                    MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Environment.ExitCode = -1;
                //System.Windows.Forms.Application.Exit();
            }

            try
            {
                Program.splashForm.ShowLoadProgress("Loading Initial Data...");

                string msgFilter = ""; // GetMessageFilterString("W");
                string inbFilter = ""; // GetInbMessageFilterString("W");

                errorMessageText = "The error occurred while attempting to get the user permission data from the Web Service at: " +
                    Properties.Settings.Default.ExtSvcAPIBaseUrl;
                ConfirmMgrAPIDal confirmMgrAPIDal = new ConfirmMgrAPIDal(Properties.Settings.Default.ExtSvcAPIBaseUrl,
                    Properties.Settings.Default.ExtSvcAPIUserName, Properties.Settings.Default.ExtSvcAPIPassword);
                v_PermKeyList = confirmMgrAPIDal.GetPermissionKeys(p_UserId, out v_IsSuperUser);

                errorMessageText = "The error occurred while reading the user permission settings returned from the Web Service at: " +
                    Properties.Settings.Default.ExtSvcAPIBaseUrl;

                //Israel 11/20/2015 -- Implement Permission key override from command line.
                if (v_IsPermKeyOverride)
                {
                    v_IsSuperUser = false;
                    v_PermKeyList = v_PermKeyOverrideList;
                }

                if (!v_IsSuperUser && v_PermKeyList.Count > 0)
                    v_PermKeyDBInClauseStr = confirmMgrAPIDal.GetPermissionKeyDBInClause(v_PermKeyList);

                if (!v_IsSuperUser && v_PermKeyDBInClauseStr == "")
                {
                    errorMessageText = String.Empty;
                    XtraMessageBox.Show("No access rights have been granted for User: " + p_UserId + ".",
                        "Main Form Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                    Environment.ExitCode = -2;
                    System.Windows.Forms.Application.Exit();
                }

                //Israel -- Testing stubs
                //v_IsSuperUser = false;
                //v_PermKeyList.Clear();
                //v_PermKeyList.Add("1438");
                //v_PermKeyDBInClauseStr = confirmMgrAPIDal.GetPermissionKeyDBInClause(v_PermKeyList);
                //Israel -- Testing stubs

                errorMessageText = "The error occurred while connecting to the update Message Server at: " + Properties.Settings.Default.MessageServer;
                dataManager = new DSManager(Properties.Settings.Default.MessageServer, // + HORNETQ_PROPS,
                            Properties.Settings.Default.MessageUser,
                            Properties.Settings.Default.MessagePassword,
                            Properties.Settings.Default.UpdateFromCacheTimerIntervalSecs,
                            msgFilter, inbFilter, v_PermKeyList, v_IsSuperUser);

                webServiceFilter = msgFilter;
                inbServiceFilter = inbFilter;
                dataManager.IncMessageCounter += new EventHandler(OnIncMessageCounter);
                dataManager.ResetMessageCounter += new EventHandler(OnResetMessageCounter);
                dataManager.BeginGridUpdates += new EventHandler(OnBeginGridUpdates);
                dataManager.EndGridUpdates += new EventHandler(OnEndGridUpdates);

                dataSet = new DataSet();
                errorMessageText = "The error occurred while loading the current workflow and base data from the database.";
                LoadInitialData();

                errorMessageText = "The error occurred while accessing the current workflow data just loaded from the database.";
                dataSet.Tables.Add(summaryDataTable);
                dataSet.Tables.Add(rqmtDataTable);
                dataSet.Tables.Add(confirmDataTable);
            }
            catch (Exception ex)
            {
                string innerException = ErrorMsgDump(ex);
                //SaveTextToFile(innerException, tempDir + "\\StartupException.log");
                SaveTextToFile(innerException, Path.Combine(appTempDir, "StartupException.log"));

                XtraMessageBox.Show("An error occurred while loading trade and related data." + Environment.NewLine +
                    errorMessageText + Environment.NewLine +
                                "Error CNF-003 in " + FORM_NAME + ".rfrmMain_Load(): " + ex.Message,
                    MAIN_FORM_STOP_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Environment.ExitCode = -2;
                System.Windows.Forms.Application.Exit();
            }

            try
            {
                barStaticDBName.Caption = v_Server + @":" + v_Database;
            }

            catch (Exception ex)
            {
                string innerException = ErrorMsgDump(ex);
                SaveTextToFile(innerException, Path.Combine(appTempDir, "StartupException.log"));
                Environment.ExitCode = -3;
                System.Windows.Forms.Application.Exit();
            }
            try
            {
                errorMessageText = "The error occurred while attempting to start the update message listeners.";
                dataManager.RegisterDataSet(ref dataSet);
                dataManager.RegisterGridControl(ref gridMain);
                dataManager.RegisterGridControl(ref gridRqmt);
                dataManager.RegisterGridControl(ref gridConfirm);
                Program.splashForm.ShowLoadProgress("Starting message listeners...");
                dataManager.StartListening();
            }
            catch (Exception except)
            {
                XtraMessageBox.Show("Failed to establish Update Message Server connection. " + Environment.NewLine +
                    errorMessageText + Environment.NewLine +
                                "Error CNF-004 in " + FORM_NAME + ".rfrmMain_Load(): " + except.Message,
                    MAIN_FORM_STOP_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Environment.ExitCode = -2;
                System.Windows.Forms.Application.Exit();
            }
            try
            {
                //Israel 14-11-10 -- Replace windows user id logon with the toolbar name.
                //windowsUserId = Sempra.Ops.Utils.GetUserName();
                barStaticUserId.Caption = toolbarOrWindowsUserId.ToLower();

                errorMessageText = "The error occurred while initializing the Summary data grid.";
                gridMain.DataSource = dataSet.Tables["SummaryData"];
                gridMain.ForceInitialize();

                errorMessageText = "The error occurred while initializing the Requirement data grid.";
                gridRqmt.DataSource = dataSet.Tables["RqmtData"];
                gridRqmt.ForceInitialize();

                errorMessageText = "The error occurred while initializing the Confirm data grid.";
                gridConfirm.DataSource = dataSet.Tables["TradeRqmtConfirm"];
                gridConfirm.ForceInitialize();

                errorMessageText = "The error occurred while initializing the Audit grid.";
                createAuditDataTable();
                auditForm.gridAudit.DataSource = this.dataSet.Tables["AuditDataTable"];
                auditForm.gridAudit.ForceInitialize();
                auditForm.settingsDir = this.appSettingsDir;

                //Israel 9/21/2015
                barBrowserStdBtns.Visible = false;
                barBrowserAddressBar.Visible = false;

                errorMessageText = "The error occurred while initializing the Xmit Result grid.";
                createXmitResultDataTable();
                xmitResultStatusLogForm.gridXmitResult.DataSource = this.dataSet.Tables["XmitResultTable"];
                xmitResultStatusLogForm.gridXmitResult.ForceInitialize();
                xmitResultStatusLogForm.settingsDir = this.appSettingsDir;

                templateListForm.settingsDir = this.appSettingsDir;

                errorMessageText = "The error occurred while initializing the Trade Data Change grid.";
                createTradeDataChangeTable();
                tradeDataChangesForm.gridTradeDataChanges.DataSource = this.dataSet.Tables["TradeDataChangeTable"];
                tradeDataChangesForm.gridTradeDataChanges.ForceInitialize();

                gridViewSummary.OptionsDetail.ShowDetailTabs = false;

                SetStdFiltersTableView();

                //Setup user access rights
                errorMessageText = "The error occurred while setting up the user role code access rights.";
                userRoleView = new System.Data.DataTable();
                userRoleView.Columns.Add(new DataColumn("RoleCode", typeof(string)));
                userRoleView.PrimaryKey = new DataColumn[] { userRoleView.Columns["RoleCode"] };
                CallGetUserRoles();
                SetUserAccess();
                ApplyUserAccess();
                if (!isHasAccess)
                {
                    XtraMessageBox.Show("User: " + toolbarOrWindowsUserId + " has not been granted access.",
                       "Main Form Stop Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Environment.ExitCode = -4;
                    System.Windows.Forms.Application.Exit();
                }

                //Setup User filters
                errorMessageText = "The error occurred while setting up the internal user filter settings.";
                CreateUserFiltersView();
                CallGetUserFilters();
                luedUserFilter.Properties.DataSource = userFiltersView.DefaultView;
                luedUserFilter.Properties.DisplayMember = "Descr";
                luedUserFilter.Properties.ValueMember = "Id";


                errorMessageText = "The error occurred while reading the FilterSettings > MostRecentUserDefined value in the user settings file stored at: " + appSettingsDir + ".";
                //Can't be in ReadUserSettings because of table creation order.
                try
                {
                    Sempra.Ops.IniFile iniFile = new Sempra.Ops.IniFile(FileNameUtils.GetUserIniFileName(appSettingsDir));
                    int filterIndex = iniFile.ReadValue("FilterSettings", "MostRecentUserDefined", NO_FILTER_VAL);
                    if (filterIndex > NO_FILTER_VAL)
                    {
                        ApplyGridFilterByUserFilterIndex(filterIndex);
                        luedUserFilter.EditValue = filterIndex;
                    }
                }
#pragma warning disable 0168
                //Disable warning...
                catch (Exception excep) { }
#pragma warning restore 0168

                errorMessageText = "The error occurred while initializing the Add Requirements data entry form.";
                addRqmtForm.InitForm();

                errorMessageText = "The error occurred while initializing the Edit Requirements data entry form.";
                editRqmtForm.rqmtStatusView = this.rqmtStatusView;
                editRqmtForm.settingsDir = this.appSettingsDir;
                editRqmtForm.InitForm();
                editRqmtForm.SetSecurityAccess(isSecondCheckCreateCxl, isSubmitQueuedEFETTrades, isForceFinalApprove);

                for (int i = 0; i < frmEditRqmt.RQMT_ARRAY_MAX; i++)
                    editRqmtForm.RqmtInitialStatus[i] = GetRqmtData(editRqmtForm.RQMT_CODES[i], "InitialStatus");

                //InitUserPrefDataLoad();

                errorMessageText = "The error occurred while initializing the internal list of Confirm Editor-controlled statuses.";
                noEditSempraRqmtStatus = new ArrayList();
                noEditSempraRqmtStatus.Add("NEW");
                noEditSempraRqmtStatus.Add("PREP");
                noEditSempraRqmtStatus.Add("EXT_REVIEW");
                noEditSempraRqmtStatus.Add("TRADER");
                noEditSempraRqmtStatus.Add("MGR");
                noEditSempraRqmtStatus.Add("OK_TO_SEND");

                //webbrowserEcm.StatusTextChanged += new EventHandler(WebBrowser_StatusTextChanged);
                //webbrowserEcm.CanGoBackChanged += new EventHandler(WebBrowser_CanGoBackChanged);
                //webbrowserEcm.CanGoForwardChanged += new EventHandler(WebBrowser_CanGoForwardChanged);

                //webbrowserOcc.StatusTextChanged += new EventHandler(WebBrowser_StatusTextChanged);
                //webbrowserOcc.CanGoBackChanged += new EventHandler(WebBrowser_CanGoBackChanged);
                //webbrowserOcc.CanGoForwardChanged += new EventHandler(WebBrowser_CanGoForwardChanged);

                //webbrowserAtc.StatusTextChanged += new EventHandler(WebBrowser_StatusTextChanged);
                //webbrowserAtc.CanGoBackChanged += new EventHandler(WebBrowser_CanGoBackChanged);
                //webbrowserAtc.CanGoForwardChanged += new EventHandler(WebBrowser_CanGoForwardChanged);

                //webbrowserEpm.StatusTextChanged += new EventHandler(WebBrowser_StatusTextChanged);
                //webbrowserEpm.CanGoBackChanged += new EventHandler(WebBrowser_CanGoBackChanged);
                //webbrowserEpm.CanGoForwardChanged += new EventHandler(WebBrowser_CanGoForwardChanged);

                //Uri siteUri = new Uri(Properties.Settings.Default.EConfirmUrl);
                //webbrowserEConfirm.Url = siteUri;

                //webbrowserEConfirm.Navigate(Properties.Settings.Default.EConfirmUrl);
                //webbrowserEcm.Navigate(Properties.Settings.Default.EcmUrl);

                //barEditFA.Refresh();
                //initComplete = true;
            }
            catch (Exception except)
            {
                XtraMessageBox.Show("An error occurred while applying security settings." + Environment.NewLine +
                    errorMessageText + Environment.NewLine +
                                "Error CNF-005 in " + FORM_NAME + ".rfrmMain_Load(): " + except.Message,
                   MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Application.Exit(); 
            }

            // init inbound panel
            try
            {
                InitInboundPanel();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while setting up the Inbound Documents panel." + Environment.NewLine +
                                "Error CNF-006 in " + FORM_NAME + ".rfrmMain_Load(): " + ex.Message,
                   MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // INIT AXPNL
            try
            {
                InitAXPanel();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while setting up the Applicaton Extender panel." + Environment.NewLine +
                                "Error CNF-007 in " + FORM_NAME + ".rfrmMain_Load(): " + ex.Message,
                   MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                barEditFA.Refresh();

                //Triggering ChangedFocusRow fires the event that filters associated docs
                if (gridViewSummary.RowCount > 1)
                {
                    gridViewSummary.FocusedRowHandle = 1;
                    gridViewSummary.FocusedRowHandle = 0;
                }

                initComplete = true;
            }
            catch (Exception except)
            {
                XtraMessageBox.Show("An error occurred while focusing the Main Grid to the first row." + Environment.NewLine +
                                "Error CNF-008 in " + FORM_NAME + ".rfrmMain_Load(): " + except.Message,
                   MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Program.splashForm.Close();
                myTimer.Start(); //Israel 12/15/2008 - Red X
            }
        }

        private void InitAXPanel()
        {
            //string trackUrl = jbossWSUrlTradeConfirm;
            //string vaultUrl = Properties.Settings.Default.DocVaultURL;
            string axFoldersXml = System.Windows.Forms.Application.StartupPath + @"\AxFolders.xml";
            string axFieldMappingsXml = System.Windows.Forms.Application.StartupPath + @"\AxFieldMappings.xml";
            // axPnl1.GeneratePnls(axFoldersXml, axFieldMappingsXml, vaultUrl);
            //axPnl1.GeneratePnls(axFoldersXml, axFieldMappingsXml, vaultUrl, trackUrl);
        }

        private StringCollection UpdatePrefCompanyList(StringCollection listBookingCoSn)
        {
            StringCollection returnData = new StringCollection();
            foreach (string cmpnySn in listBookingCoSn)
            {
                DataRow row = seCptyLkupTbl.Rows.Find(cmpnySn);
                if (row != null)
                {
                    returnData.Add(cmpnySn);
                }

            }
            return returnData;
        }

        private void InitUserPrefDataLoad()
        {
            try
            {
                //Add Sempra Company data to prefs list. Sort the list by adding it into an arraylist first.
                //seCptyLkupTbl.PrimaryKey = new DataColumn[] { seCptyLkupTbl.Columns["CptySn"] };
                ArrayList list = new ArrayList();
                foreach (DataRow row in seCptyLkupTbl.Rows)
                    list.Add(row["CptySn"].ToString());
                list.Sort();
                for (int i = 0; i < list.Count; i++)
                    userPrefsForm.cklbxSeCptySn.Items.Add(list[i]);

                CdtyGroupCodesDal cdtyGroupCodesDal = new CdtyGroupCodesDal(sqlConnectionStr);
                IList<GetCdtyGroupCodesDto> cdtyGroupCodesList;
                //if (isTestMode)
                //    cdtyGroupCodesList = cdtyGroupCodesDal.GetAllStub();
                //else
                cdtyGroupCodesList = cdtyGroupCodesDal.GetAll();
                if (cdtyGroupCodesList != null)
                {
                    list.Clear();
                    for (int i = 0; i < cdtyGroupCodesList.Count; i++)
                    {
                        list.Add(cdtyGroupCodesList[i].CdtyGroupCode);
                    }
                }

                //cdtyLkupTbl.PrimaryKey = new DataColumn[] { cdtyLkupTbl.Columns["CdtyCode"] };
                //list.Clear();
                //foreach (DataRow row in cdtyLkupTbl.Rows)
                //   list.Add(row["CdtyCode"].ToString());
                list.Sort();
                for (int i = 0; i < list.Count; i++)
                    userPrefsForm.cklbxCdtyGrp.Items.Add(list[i]);

                string dataLoadSettings = Path.Combine(appSettingsDir, "DataLoad.ini");
                Sempra.Ops.IniFile iniFileDataLoad = new Sempra.Ops.IniFile(dataLoadSettings);
                StringCollection loadList = new StringCollection();

                loadList = iniFileDataLoad.ReadValue(LOAD_SEMPRA_COMPANY);
                loadList = UpdatePrefCompanyList(loadList);  // validating the company list with preference
                string chklistValue = "";
                for (int i = 0; i < loadList.Count; i++)
                {
                    chklistValue = loadList[i];
                    int index = userPrefsForm.cklbxSeCptySn.FindStringExact(chklistValue);
                    userPrefsForm.cklbxSeCptySn.SetItemChecked(index, true);
                }

                loadList = iniFileDataLoad.ReadValue(LOAD_CDTY_GROUP);
                chklistValue = "";
                for (int i = 0; i < loadList.Count; i++)
                {
                    chklistValue = loadList[i];
                    int index = userPrefsForm.cklbxCdtyGrp.FindStringExact(chklistValue);
                    userPrefsForm.cklbxCdtyGrp.SetItemChecked(index, true);
                }
            }
            catch (Exception except)
            {
                throw new Exception("An error occurred while loading iniital values for Our Company and Cdty Group." + Environment.NewLine +
                    "Error CNF-009 in " + FORM_NAME + ".InitUserPrefDataLoad(): " + except.Message);
            }
        }

        private void LoadInitialData()
        {
            string currentTableLoad = "";
            try
            {
                DateTime startTime = DateTime.Now;
                Program.splashForm.ShowLoadProgress("Loading Summary data...");
                currentTableLoad = "SummaryData";

                VPcTradeSummaryDal vpcTradeSummaryDal = new VPcTradeSummaryDal(sqlConnectionStr);
                IList<SummaryData> summaryList;
                //if (isTestMode)
                //    summaryList = vpcTradeSummaryDal.GetAllStub();
                //else

                string tradeIdList = "";
                summaryList = vpcTradeSummaryDal.GetAll(v_PermKeyDBInClauseStr);
                if (summaryList.Count == 0)
                {
                    XtraMessageBox.Show("There are currently no trades for your permission set." + Environment.NewLine +
                        "The application has started normally and is listening for new trade activity.",
                        APP_NAME + " Logon", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    tradeIdList = vpcTradeSummaryDal.GetTradeIdListString(summaryList);

                //Israel 9/28/2015
                //string filterText = ""; // GetMessageFilterString("G");
                //summaryDataTable = CollectionHelper.ConvertTo<SummaryData>(summaryList, filterText);
                summaryDataTable = CollectionHelper.ConvertTo<SummaryData>(summaryList);
                summaryDataTable.PrimaryKey = new DataColumn[] { summaryDataTable.Columns["Id"] };

                Program.splashForm.ShowLoadProgress("Loading Requirement data...");
                currentTableLoad = "RqmtData";
                VPcTradeRqmtDal vpcTradeRqmtDal = new VPcTradeRqmtDal(sqlConnectionStr);
                IList<RqmtData> rqmtList;
                //if (isTestMode)
                //    rqmtList = vpcTradeRqmtDal.GetAllStub();
                //else

                if (summaryList.Count > 0)
                {
                    rqmtList = vpcTradeRqmtDal.GetAll(tradeIdList);
                    rqmtDataTable = CollectionHelper.ConvertTo<RqmtData>(rqmtList);
                }
                rqmtDataTable.PrimaryKey = new DataColumn[] { rqmtDataTable.Columns["Id"] };

                Program.splashForm.ShowLoadProgress("Loading Confirmation data...");
                currentTableLoad = "TradeRqmtConfirm";
                TradeRqmtConfirmDal tradeRqmtConfirmDal = new TradeRqmtConfirmDal(sqlConnectionStr);
                IList<TradeRqmtConfirm> confirmList;
                //if (isTestMode)
                //    confirmList = tradeRqmtConfirmDal.GetAllStub();
                //else

                if (summaryList.Count > 0)
                {
                    confirmList = tradeRqmtConfirmDal.GetAll(tradeIdList);
                    confirmDataTable = CollectionHelper.ConvertTo<TradeRqmtConfirm>(confirmList);
                }
                confirmDataTable.PrimaryKey = new DataColumn[] { confirmDataTable.Columns["Id"] };

                Program.splashForm.ShowLoadProgress("Loading Base data...");
                currentTableLoad = "RqmtStatusColor";
                RqmtStatusColorsDal rqmtStatusColorsDal = new RqmtStatusColorsDal(sqlConnectionStr);
                IList<RqmtStatusColor> rqmtStatusColorList;
                //if (isTestMode)
                //    rqmtStatusColorList = rqmtStatusColorsDal.GetAllStub();
                //else
                rqmtStatusColorList = rqmtStatusColorsDal.GetAll();
                rqmtStatusColorTable = CollectionHelper.ConvertTo<RqmtStatusColor>(rqmtStatusColorList);

                currentTableLoad = "ColorTranslate";
                ColorTranslateDal colorTranslateDal = new ColorTranslateDal(sqlConnectionStr);
                IList<ColorTranslate> colorTranslateList;
                //if (isTestMode)
                //    colorTranslateList = colorTranslateDal.GetAllStub();
                //else
                colorTranslateList = colorTranslateDal.GetAll();
                colorTranslateTable = CollectionHelper.ConvertTo<ColorTranslate>(colorTranslateList);

                currentTableLoad = "RqmtView";
                RqmtDal rqmtDal = new RqmtDal(sqlConnectionStr);
                IList<RqmtView> rqmtViewList;
                //if (isTestMode)
                //    rqmtViewList = rqmtDal.GetAllStub();
                //else
                rqmtViewList = rqmtDal.GetAll();
                rqmtView = CollectionHelper.ConvertTo<RqmtView>(rqmtViewList);

                currentTableLoad = "RqmtStatusView";
                RqmtStatusDal rqmtStatusDal = new RqmtStatusDal(sqlConnectionStr);
                IList<RqmtStatusView> rqmtStatusViewList;
                //if (isTestMode)
                //    rqmtStatusViewList = rqmtStatusDal.GetAllStub();
                //else
                rqmtStatusViewList = rqmtStatusDal.GetAll();
                rqmtStatusView = CollectionHelper.ConvertTo<RqmtStatusView>(rqmtStatusViewList);

                currentTableLoad = "TradeDataCdtyLkupView";
                CdtyCodeLkupDal cdtyCodeLkupDal = new CdtyCodeLkupDal(sqlConnectionStr);
                IList<BdtaCdtyLkup> cdtyCodeList;
                //if (isTestMode)
                //    cdtyCodeList = cdtyCodeLkupDal.GetAllStub();
                //else
                cdtyCodeList = cdtyCodeLkupDal.GetAll();
                cdtyLkupTbl = CollectionHelper.ConvertTo<BdtaCdtyLkup>(cdtyCodeList);

                currentTableLoad = "TradeDataCptyLkupView";
                CptyInfoDal cptyInfoDal = new CptyInfoDal(sqlConnectionStr);
                IList<BdtaCptyLkup> cptyList;
                //if (isTestMode)
                //    cptyList = cptyInfoDal.GetOpenConfirmLookupStub();
                //else
                cptyList = cptyInfoDal.GetOpenConfirmLookup();
                cptyLkupTbl = CollectionHelper.ConvertTo<BdtaCptyLkup>(cptyList);
                dataSet.Tables.Add(cptyLkupTbl);  // used for inbound panel's cpty ref mapping

                currentTableLoad = "InboundDocsView";
                Program.splashForm.ShowLoadProgress("Loading Inbound Documents...");
                InboundDocsDal inboundDocsDal = new InboundDocsDal(sqlConnectionStr);
                IList<InboundDocsView> inboundDocList;
                //if (isTestMode)
                //    inboundDocList = inboundDocsDal.GetAllStub();
                //else
                inboundDocList = inboundDocsDal.GetAll();
                //Israel 9/18/2015
                //inboundDocDataTable = CollectionHelper.ConvertTo(inboundDocList, ibQry);
                inboundDocDataTable = CollectionHelper.ConvertTo(inboundDocList);
                dataSet.Tables.Add(inboundDocDataTable);

                Program.splashForm.ShowLoadProgress("Loading Associated Documents...");
                currentTableLoad = "AssociatedDoc";
                AssociatedDocsDal associatedDocsDal = new AssociatedDocsDal(sqlConnectionStr);
                IList<AssociatedDoc> associatedDocList;
                //if (isTestMode)
                //    associatedDocList = associatedDocsDal.GetAllStub();
                //else
                associatedDocList = associatedDocsDal.GetAll();
                //assocDocDataTable = CollectionHelper.ConvertTo(associatedDocList, ibQry);
                assocDocDataTable = CollectionHelper.ConvertTo(associatedDocList);
                dataSet.Tables.Add(assocDocDataTable);

                DateTime endTime = DateTime.Now;
                TimeSpan elapsed = endTime - startTime;
                Program.splashForm.ShowLoadProgress("Establishing dataset relationships...");
                DataColumn[] colPk = new DataColumn[1];
                colPk[0] = rqmtStatusColorTable.Columns["Hashkey"];
                rqmtStatusColorTable.PrimaryKey = colPk;

                colPk[0] = colorTranslateTable.Columns["Code"];
                colorTranslateTable.PrimaryKey = colPk;

                rqmtView.DefaultView.RowFilter = "[ActiveFlag] = 'Y' and [DetActRqmtFlag] = 'Y'";
                rqmtView.PrimaryKey = new DataColumn[] { rqmtView.Columns["Code"] };

                addRqmtForm.lookupRqmt.Properties.DataSource = rqmtView.DefaultView;
                addRqmtForm.lookupRqmt.Properties.DisplayMember = "DisplayText";
            }
            catch (Exception except)
            {
                throw new Exception("Table being loaded=" + currentTableLoad + "." + Environment.NewLine +
                    "Error CNF-010 in " + FORM_NAME + ".LoadInitialData(): " + except.Message);
            }
        }

        private void ReadAppSettings()
        {
            try
            {
                if (!System.IO.Directory.Exists(appTempDir))
                    System.IO.Directory.CreateDirectory(appTempDir);

                if (!System.IO.Directory.Exists(tempFaxDir))
                    System.IO.Directory.CreateDirectory(tempFaxDir);

                cbeditBrowserReports.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                string innerException = ErrorMsgDump(ex);
                XtraMessageBox.Show(APP_NAME + " was unable to read application settings. " + Environment.NewLine +
                     "Error CNF-011 in " + FORM_NAME + ".ReadAppSettings(): " + ex.Message,
                    MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReadUserSettings()
        {
            try
            {
                dockManager.ForceInitialize();

                if (System.IO.File.Exists(Path.Combine(appSettingsDir, SUMMARY_GRID_SETTINGS)))
                    gridViewSummary.RestoreLayoutFromXml(Path.Combine(appSettingsDir, SUMMARY_GRID_SETTINGS));
                if (System.IO.File.Exists(Path.Combine(appSettingsDir, RQMT_GRID_SETTINGS)))
                    gridViewRqmt.RestoreLayoutFromXml(Path.Combine(appSettingsDir, RQMT_GRID_SETTINGS));
                if (System.IO.File.Exists(Path.Combine(appSettingsDir, CONFIRM_GRID_SETTINGS)))
                    gridViewConfirm.RestoreLayoutFromXml(Path.Combine(appSettingsDir, CONFIRM_GRID_SETTINGS));
                if (System.IO.File.Exists(Path.Combine(appSettingsDir, DOCK_MGR_SETTINGS)))
                    dockManager.RestoreLayoutFromXml(Path.Combine(appSettingsDir, DOCK_MGR_SETTINGS));
                if (System.IO.File.Exists(Path.Combine(appSettingsDir, BROWSER_BAR_SETTINGS)))
                    barmgrBrowser.RestoreLayoutFromXml(Path.Combine(appSettingsDir, BROWSER_BAR_SETTINGS));

                barBrowserStdBtns.Reset();
                barBrowserAddressBar.Reset();
                SetPanelMenuChecked();

                //Now read user settings, ReadAppSettings() must be called first
                Sempra.Ops.IniFile iniFile = new Sempra.Ops.IniFile(FileNameUtils.GetUserIniFileName(appSettingsDir));

                this.StartPosition = FormStartPosition.Manual;

                //Israel 1/14/2009 -- fixes -32000 problem.
                int top = iniFile.ReadValue(FORM_NAME, "Top", 100);
                int left = iniFile.ReadValue(FORM_NAME, "Left", 200);
                this.Top = top > 0 ? top : 1;
                this.Left = left > 0 ? left : 1;

                //this.Top = iniFile.ReadValue(FORM_NAME, "Top", 100);
                //this.Left = iniFile.ReadValue(FORM_NAME, "Left", 200);
                this.Width = iniFile.ReadValue(FORM_NAME, "Width", 1000);
                this.Height = iniFile.ReadValue(FORM_NAME, "Height", 700);

                //if (dpTradeData.Visibility == DockVisibility.Visible)
                //{
                this.dpTradeData.Height = iniFile.ReadValue(FORM_NAME, "TradeDataPanel_Height", 1110);
                this.dpTradeData.Width = iniFile.ReadValue(FORM_NAME, "TradeDataPanel_Width", 990);

                //The SplitterPostion didn't restore exactly as saved until I set splitContainerTradeDat.Fixed = panel1. When it was set to None
                //it was enforcing proportional spacing and not doing what it was set to.                    
                //}

                //if (dpBrowserApps.Visibility == DockVisibility.Visible)
                // {
                // this.dpBrowserApps.Width = iniFile.ReadValue(FORM_NAME, "BrowserApps_PanelWidth", 561);
                /// }

                this.dpBrowserApps.Width = iniFile.ReadValue(FORM_NAME, "BrowserAppsPanel_Width", 568);
                this.dpBrowserApps.Height = iniFile.ReadValue(FORM_NAME, "BrowserAppsPanel_Height", 805);
                this.dpVaultDocViewer.Width = iniFile.ReadValue(FORM_NAME, "VaultedDocsPanel_Width", 1500);
                this.dpVaultDocViewer.Height = iniFile.ReadValue(FORM_NAME, "VaultedDocsPanel_Height", 175);
                this.dpInboundQueue.Width = iniFile.ReadValue(FORM_NAME, "InboundQueuePanel_Width", 990);
                this.dpInboundQueue.Height = iniFile.ReadValue(FORM_NAME, "InboundQueuePanel_Height", 260);
                this.dpInboundViewer.Width = iniFile.ReadValue(FORM_NAME, "InboundViewerPanel_Width", 990);
                this.dpInboundViewer.Height = iniFile.ReadValue(FORM_NAME, "InboundViewerPanel_Height", 1110);

                barChkTradeData.Checked = iniFile.ReadValue("PanelVisibility", "TradeData", true);

                barChkInboundQueue.Checked = iniFile.ReadValue("PanelVisibility", "InboundQueue", false);
                barChkInboundViewer.Checked = iniFile.ReadValue("PanelVisibility", "InboundViewer", false);
                barChkBrowserApps.Checked = iniFile.ReadValue("PanelVisibility", "BrowserApps", false);
                barChkVaultedDocsPanel.Checked = iniFile.ReadValue("PanelVisibility", "VaultedDocsPanel", false);
                if (barChkVaultedDocsPanel.Checked)
                    this.dpVaultDocViewer.Width = iniFile.ReadValue(FORM_NAME, "VaultedDocsPanel_Width", 300);

                barChkCustomFilterPanel.Checked = iniFile.ReadValue("PanelVisibility", "CustomFilterPanel", true);
                barChkGridFilterPanel.Checked = iniFile.ReadValue("PanelVisibility", "GridFilterPanel", true);
                barChkGridGroupPanel.Checked = iniFile.ReadValue("PanelVisibility", "GridGroupPanel", true);

                string skin = iniFile.ReadValue("Preferences", "Skin", "Money Twins");
                defaultLookAndFeel.LookAndFeel.SkinName = skin;

                saveToExcelDirectory = iniFile.ReadValue("Preferences", "SaveToExcelDirectory", appDocumentsDir);
                userPrefsForm.cedAutoDispDealsheet.Checked = iniFile.ReadValue("Preferences", "AutoDisplayDealsheet", false);
                isAutoDispDealsheet = userPrefsForm.cedAutoDispDealsheet.Checked;
                //isSaveToNewExcelFormat = userPrefsForm.cedSaveToNewExcelFormat.Checked;

                barChkNB.Checked = iniFile.ReadValue("FilterSettings", "ShowNewBusiness", false);
                barChkRFA.Checked = iniFile.ReadValue("FilterSettings", "ReadyForFinalApproval", false);
                barChkHP.Checked = iniFile.ReadValue("FilterSettings", "HasProblems", false);

                SetBarCheckImage(barChkNB);
                SetBarCheckImage(barChkRFA);
                SetBarCheckImage(barChkHP);

                finalApprovalFilterIndex = iniFile.ReadValue("FilterSettings", "FinalApproved", 0);
                //Israel 1/23/09 fixed -1 problem.
                if (finalApprovalFilterIndex < 0)
                    finalApprovalFilterIndex = 0;
                string faVal = (string)barEditFAComboBox.Items[finalApprovalFilterIndex];
                barEditFA.EditValue = faVal;

                barChkFAHints = new string[3];
                barChkFAHints[0] = "Only Non-Final Approved Selected";
                barChkFAHints[1] = "Only Final Approved Selected";
                barChkFAHints[2] = "Non- and Final Approved Selected";
                barEditFA.Hint = barChkFAHints[finalApprovalFilterIndex];

                SetStdFilterHint(barChkNB);
                SetStdFilterHint(barChkHP);
                SetStdFilterHint(barChkRFA);

                this.panelContainerMain.Height = iniFile.ReadValue(FORM_NAME, "MainPanel_Height", 480);

                //splitContainerTradeData.Height = iniFile.ReadValue(FORM_NAME, "TradeDataSplitter_Height", 400); //
                //splitContainerRqmt.Height = iniFile.ReadValue(FORM_NAME, "RqmtSplitter_Height", 160); //
                splitContainerRqmt.SplitterPosition = iniFile.ReadValue(FORM_NAME, "RqmtSplitter_Position", 506);
                splitContainerTradeData.SplitterPosition = iniFile.ReadValue(FORM_NAME, "TradeDataSplitter_Position", 427);


                pnlCustomFilter.Visible = barChkCustomFilterPanel.Checked;
                gridViewSummary.OptionsView.ShowAutoFilterRow = barChkGridFilterPanel.Checked;
                gridViewSummary.OptionsView.ShowGroupPanel = barChkGridGroupPanel.Checked;
            }
            catch (Exception ex)
            {
                string innerException = ErrorMsgDump(ex);
                XtraMessageBox.Show(APP_NAME + " had a problem while reading the user''s saved settings." + Environment.NewLine +
                     "Error CNF-012 in " + FORM_NAME + ".ReadUserSettings(): " + ex.Message,
                    MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WriteUserSettings()
        {
            try
            {
                if (!System.IO.Directory.Exists(appSettingsDir))
                    System.IO.Directory.CreateDirectory(appSettingsDir);

                string dataLoadSettings = Path.Combine(appSettingsDir, "DataLoad.ini");
                if (System.IO.File.Exists(dataLoadSettings))
                    System.IO.File.Delete(dataLoadSettings);

                //DevExpress.Utils.OptionsLayoutBase.FullLayout.
                gridViewSummary.SaveLayoutToXml(Path.Combine(appSettingsDir, SUMMARY_GRID_SETTINGS));
                gridViewRqmt.SaveLayoutToXml(Path.Combine(appSettingsDir, RQMT_GRID_SETTINGS));
                gridViewConfirm.SaveLayoutToXml(Path.Combine(appSettingsDir, CONFIRM_GRID_SETTINGS));
                dockManager.SaveLayoutToXml(Path.Combine(appSettingsDir, DOCK_MGR_SETTINGS));
                barmgrBrowser.SaveLayoutToXml(Path.Combine(appSettingsDir, BROWSER_BAR_SETTINGS));

                inboundPnl1.SaveGridSettings();

                Sempra.Ops.IniFile iniFile = new Sempra.Ops.IniFile(FileNameUtils.GetUserIniFileName(appSettingsDir));
                Sempra.Ops.IniFile iniFileDataLoad = new Sempra.Ops.IniFile(dataLoadSettings);

                iniFile.WriteValue(FORM_NAME, "TradeDataPanel_Width", dpTradeData.Width);
                iniFile.WriteValue(FORM_NAME, "TradeDataPanel_Height", dpTradeData.Height);
                iniFile.WriteValue(FORM_NAME, "BrowserAppsPanel_Width", dpBrowserApps.Width);
                iniFile.WriteValue(FORM_NAME, "BrowserAppsPanel_Height", dpBrowserApps.Height);
                iniFile.WriteValue(FORM_NAME, "VaultedDocsPanel_Width", dpVaultDocViewer.Width);
                iniFile.WriteValue(FORM_NAME, "VaultedDocsPanel_Height", dpVaultDocViewer.Height);
                iniFile.WriteValue(FORM_NAME, "InboundQueuePanel_Width", dpInboundQueue.Width);
                iniFile.WriteValue(FORM_NAME, "InboundQueuePanel_Height", dpInboundQueue.Height);
                iniFile.WriteValue(FORM_NAME, "InboundViewerPanel_Width", dpInboundViewer.Width);
                iniFile.WriteValue(FORM_NAME, "InboundViewerPanel_Height", dpInboundViewer.Height);

                iniFile.WriteValue(FORM_NAME, "MainPanel_Height", panelContainerMain.Height);


                //5/19/09 Israel - Make sure if minimized when closed it doesn't write those settings here.
                if (this.WindowState != FormWindowState.Minimized)
                {
                    iniFile.WriteValue(FORM_NAME, "Top", this.Top);
                    iniFile.WriteValue(FORM_NAME, "Left", this.Left);
                    iniFile.WriteValue(FORM_NAME, "Width", this.Width);
                    iniFile.WriteValue(FORM_NAME, "Height", this.Height);
                }

                //iniFile.WriteValue(FORM_NAME, "SplitterInboundPosition", splitterInbound.SplitterPosition);
                iniFile.WriteValue(FORM_NAME, "TradeDataSplitter_Height", splitContainerTradeData.Height);
                iniFile.WriteValue(FORM_NAME, "TradeDataSplitter_Position", splitContainerTradeData.SplitterPosition);
                iniFile.WriteValue(FORM_NAME, "RqmtSplitter_Height", splitContainerRqmt.Height);
                iniFile.WriteValue(FORM_NAME, "RqmtSplitter_Position", splitContainerRqmt.SplitterPosition);

                //iniFile.WriteValue("Preferences", "SempraCompanyId", sempraCompanyId);
                iniFile.WriteValue("Preferences", "Skin", defaultLookAndFeel.LookAndFeel.SkinName.ToString());
                iniFile.WriteValue("Preferences", "SaveToExcelDirectory", saveToExcelDirectory);
                iniFile.WriteValue("Preferences", "AutoDisplayDealsheet", userPrefsForm.cedAutoDispDealsheet.Checked);
                //iniFile.WriteValue("Preferences", "SaveToNewExcelFormat", userPrefsForm.cedSaveToNewExcelFormat.Checked);

                iniFile.WriteValue("FilterSettings", "ShowNewBusiness", barChkNB.Checked);
                iniFile.WriteValue("FilterSettings", "ReadyForFinalApproval", barChkRFA.Checked);
                iniFile.WriteValue("FilterSettings", "HasProblems", barChkHP.Checked);

                int faIndex = barEditFAComboBox.Items.IndexOf(barEditFA.EditValue);
                iniFile.WriteValue("FilterSettings", "FinalApproved", faIndex);
                iniFile.WriteValue("FilterSettings", "MostRecentUserDefined", GetMostRecentUserFilterIndex());

                iniFile.WriteValue("PanelVisibility", "TradeData", barChkTradeData.Checked);
                //iniFile.WriteValue("PanelVisibility", "VaultedDocs", barChkVaultedDocs.Checked);
                iniFile.WriteValue("PanelVisibility", "InboundQueue", barChkInboundQueue.Checked);
                iniFile.WriteValue("PanelVisibility", "InboundViewer", barChkInboundViewer.Checked);
                iniFile.WriteValue("PanelVisibility", "BrowserApps", barChkBrowserApps.Checked);
                //iniFile.WriteValue("PanelVisibility", "VaultedDocsPanel", barChkVaultedDocsPanel.Checked);

                iniFile.WriteValue("PanelVisibility", "CustomFilterPanel", barChkCustomFilterPanel.Checked);
                iniFile.WriteValue("PanelVisibility", "GridFilterPanel", barChkGridFilterPanel.Checked);
                iniFile.WriteValue("PanelVisibility", "GroupFilterPanel", barChkGridGroupPanel.Checked);

                StringCollection list = new StringCollection();
                for (int i = 0; i < userPrefsForm.cklbxSeCptySn.Items.Count; i++)
                {
                    if (userPrefsForm.cklbxSeCptySn.Items[i].CheckState == CheckState.Checked)
                    {
                        list.Add(userPrefsForm.cklbxSeCptySn.Items[i].Value.ToString());
                    }
                }
                iniFileDataLoad.WriteValue(LOAD_SEMPRA_COMPANY, list);

                list.Clear();
                for (int i = 0; i < userPrefsForm.cklbxCdtyGrp.Items.Count; i++)
                {
                    if (userPrefsForm.cklbxCdtyGrp.Items[i].CheckState == CheckState.Checked)
                    {
                        list.Add(userPrefsForm.cklbxCdtyGrp.Items[i].Value.ToString());
                    }
                }
                iniFileDataLoad.WriteValue(LOAD_CDTY_GROUP, list);
            }
            catch (Exception error)
            {
                throw new Exception("An error occurred while saving user settings to: " + appSettingsDir + "." + Environment.NewLine +
                     "Error CNF-013 in " + FORM_NAME + ".WriteUserSettings(): " + error.Message);
            }
        }

        private void WriteUserDockSettings()
        {
            try
            {
                Sempra.Ops.IniFile iniFile = new Sempra.Ops.IniFile(FileNameUtils.GetUserIniFileName(appSettingsDir));
            }
            catch (Exception error)
            {
                throw new Exception("An error occurred while saving user docking settings." + Environment.NewLine +
                     "Error CNF-014 in " + FORM_NAME + ".WriteUserDockSettings(): " + error.Message);
            }
        }

        private void ReadUserDockSettings()
        {
            try
            {
                Sempra.Ops.IniFile iniFile = new Sempra.Ops.IniFile(FileNameUtils.GetUserIniFileName(appSettingsDir));
                //barChkNB.Checked = iniFile.ReadValue("FilterSettings", "ShowNewBusiness", false);
            }
            catch (Exception error)
            {
                throw new Exception("An error occurred while reading user docking settings." + Environment.NewLine +
                     "Error CNF-015 in " + FORM_NAME + ".ReadUserDockSettings(): " + error.Message);
            }
        }

        private void SetUserAccess()
        {
            try
            {
                isHasAccess = userRoleView.Rows.Contains("ACCESS");
                isHasUpdate = userRoleView.Rows.Contains("UPDATE");
                isFinalApprove = userRoleView.Rows.Contains("FNAPP");
                isForceFinalApprove = userRoleView.Rows.Contains("FNAPP-OVR");
                isSecondCheckCreateCxl = userRoleView.Rows.Contains("SC-CRCXL");
                isSubmitQueuedEFETTrades = userRoleView.Rows.Contains("SUB-EFET");
                isContractApprove = userRoleView.Rows.Contains("CNTRCT-APP");
                //isContractApprove = true;
            }
            catch (Exception error)
            {
                throw new Exception("An error occurred while setting user access rights used internally by " + APP_NAME + "." + Environment.NewLine +
                     "Error CNF-016 in " + FORM_NAME + ".SetUserAccess(): " + error.Message);
            }
        }

        private void ApplyUserAccess()
        {
            try
            {
                if (isFinalApprove)
                {
                    bbFinalApproval.Visibility = BarItemVisibility.Always;
                    bbReopenFinalApproval.Visibility = BarItemVisibility.Always;
                }
                else
                {
                    bbFinalApproval.Visibility = BarItemVisibility.Never;
                    bbReopenFinalApproval.Visibility = BarItemVisibility.Never;
                }

                if (isForceFinalApprove)
                {
                    bbForceFinalApproval.Visibility = BarItemVisibility.Always;
                    //barSubUtilities.Visibility = BarItemVisibility.Always;
                }
                else
                {
                    bbForceFinalApproval.Visibility = BarItemVisibility.Never;
                    barSubUtilities.Visibility = BarItemVisibility.Never;
                }

                if (isHasUpdate)
                {
                    bbEditRequirements.Visibility = BarItemVisibility.Always;
                    bbAddRequirement.Visibility = BarItemVisibility.Always;
                    bbDetermineActionsNo.Visibility = BarItemVisibility.Always;
                    bbDetermineActionsReprocess.Visibility = BarItemVisibility.Always;
                    //bsubEConfirm.Visibility = BarItemVisibility.Always;
                    //bsubEfet.Visibility = BarItemVisibility.Always;
                    bbGroupTrades.Visibility = BarItemVisibility.Always;
                    bbUngroupTrades.Visibility = BarItemVisibility.Always;
                    //bbCorrectTradeData.Visibility = BarItemVisibility.Always;
                }
                else
                {
                    bbEditRequirements.Visibility = BarItemVisibility.Never;
                    bbAddRequirement.Visibility = BarItemVisibility.Never;
                    bbDetermineActionsNo.Visibility = BarItemVisibility.Never;
                    bbDetermineActionsReprocess.Visibility = BarItemVisibility.Never;
                    bsubEConfirm.Visibility = BarItemVisibility.Never;
                    bsubEfet.Visibility = BarItemVisibility.Never;
                    bbGroupTrades.Visibility = BarItemVisibility.Never;
                    bbUngroupTrades.Visibility = BarItemVisibility.Never;
                    bbCorrectTradeData.Visibility = BarItemVisibility.Never;
                    barbtnReOpenInbDoc.Visibility = BarItemVisibility.Never;
                    bsubContracts.Visibility = BarItemVisibility.Never;

                }

                ChangeRQMTContextMenuVisibility(isContractApprove);

                editRqmtForm.SetSecondCheckEnabled(isSecondCheckCreateCxl);

                if (isSubmitQueuedEFETTrades)
                {
                    bbSubmitQueuedEfetTrades.Visibility = BarItemVisibility.Always;
                    bbEfetCancelCpty.Visibility = BarItemVisibility.Always;
                    bbEfetCancelBroker.Visibility = BarItemVisibility.Always;
                }
                else
                {
                    bbSubmitQueuedEfetTrades.Visibility = BarItemVisibility.Never;
                    bbEfetCancelCpty.Visibility = BarItemVisibility.Never;
                    bbEfetCancelBroker.Visibility = BarItemVisibility.Never;
                }

                //Israel 9/17/2015 remove from menu
                bbCorrectTradeData.Visibility = BarItemVisibility.Never;
                barSubUtilities.Visibility = BarItemVisibility.Never;
            }
            catch (Exception error)
            {
                throw new Exception("An error occurred while applying user access rights to application." + Environment.NewLine +
                     "Error CNF-017 in " + FORM_NAME + ".ApplyUserAccess(): " + error.Message);
            }
        }

        private void ChangeRQMTContextMenuVisibility(bool visibilytyFlag)
        {
            if ((isContractApprove || visibilytyFlag) && isHasUpdate)
            {
                bbtnOkToSendAndSend.Visibility = BarItemVisibility.Always;
                bbtnOktoSend.Visibility = BarItemVisibility.Always;
                bbtnOkToSendAndManualSend.Visibility = BarItemVisibility.Always;
                barbtnRqmtSetToOkToSend.Visibility = BarItemVisibility.Always;
                barbtnRqmtSetToOkAndSend.Visibility = BarItemVisibility.Always;
                barbtnRqmtSetToOkAndManualSend.Visibility = BarItemVisibility.Always;
                //barbtnRqmtSetToNew.Visibility = BarItemVisibility.Always;
                //barbtnRqmtCancel.Visibility = BarItemVisibility.Always;
            }
            else
            {
                bbtnOkToSendAndSend.Visibility = BarItemVisibility.Never;
                bbtnOktoSend.Visibility = BarItemVisibility.Never;
                bbtnOkToSendAndManualSend.Visibility = BarItemVisibility.Never;
                barbtnRqmtSetToOkToSend.Visibility = BarItemVisibility.Never;
                barbtnRqmtSetToOkAndSend.Visibility = BarItemVisibility.Never;
                barbtnRqmtSetToOkAndManualSend.Visibility = BarItemVisibility.Never;
                //barbtnRqmtSetToNew.Visibility = BarItemVisibility.Never;
                //barbtnRqmtCancel.Visibility = BarItemVisibility.Never;
            }
        }

        #endregion

        #region Private methods

        private void SetBarCheckImage(DevExpress.XtraBars.BarCheckItem ABarCheckItem)
        {
            try
            {
                ABarCheckItem.ImageIndex = ABarCheckItem.Checked ? CHECKED : UNCHECKED;
            }
            catch (Exception error)
            {
                throw new Exception("An error occurred while setting the status bar check status for: " + ABarCheckItem.Name + "." + Environment.NewLine +
                     "Error CNF-018 in " + FORM_NAME + ".SetBarCheckImage(): " + error.Message);
            }
        }

        private void SetStdFilterHint(BarCheckItem ABarCheckItem)
        {
            try
            {
                ABarCheckItem.Hint = ABarCheckItem.Tag + ": Filter=" +
                                     (string)(ABarCheckItem.Checked ? "On" : "Off");
            }
            catch (Exception error)
            {
                throw new Exception("An error occurred while setting the status bar filter hint for: " + ABarCheckItem.Name + "." + Environment.NewLine +
                     "Error CNF-019 in " + FORM_NAME + ".SetStdFilterHint(): " + error.Message);
            }
        }

        private Color GetHashkeyColor(string AHashkey)
        {
            Color color = Color.Transparent;
            try
            {
                DataRow row = rqmtStatusColorTable.Rows.Find(AHashkey);
                string colorStr = row["CsColor"].ToString();
                color = Color.FromName(colorStr);
            }
            catch (Exception error)
            {
                throw new Exception("An error occurred while getting display color for: " + AHashkey + "." + Environment.NewLine +
                     "Error CNF-020 in " + FORM_NAME + ".GetHashkeyColor(): " + error.Message);
            }
            return color;
        }

        private string GetHashkeyTrackingClientColor(string AHashkey)
        {
            //Gets the correct color for the rqmt/status and translates it to orignial TC color.
            string colorStr = "";
            string tcColor = "";
            try
            {
                DataRow row = rqmtStatusColorTable.Rows.Find(AHashkey);
                colorStr = row["CsColor"].ToString();

                foreach (DataRow row2 in colorTranslateTable.Rows)
                {
                    if (row2["CsColor"].ToString() == colorStr)
                        tcColor = row2["Code"].ToString();
                }
            }
            catch (Exception error)
            {
                throw new Exception("An error occurred while getting display color for: " + AHashkey + "." + Environment.NewLine +
                     "Error CNF-021 in " + FORM_NAME + ".GetHashkeyTrackingClientColor(): " + error.Message);
            }
            return tcColor;
        }

        private Color TranslateColor(string AColor)
        {
            Color color = Color.Transparent;
            try
            {
                DataRow row = colorTranslateTable.Rows.Find(AColor);
                string colorStr = row["CsColor"].ToString();
                color = Color.FromName(colorStr);
            }
            catch (Exception error)
            {
                throw new Exception("An error occurred while getting display color for: " + AColor + "." + Environment.NewLine +
                     "Error CNF-022 in " + FORM_NAME + ".TranslateColor(): " + error.Message);
            }
            return color;
        }

        private void SaveTextToFile(string AText, string AFileName)
        {
            try
            {
                // create a writer and open the file
                TextWriter tw = new StreamWriter(AFileName);

                // write a line of text to the file
                tw.WriteLine(AText);

                // close the stream
                tw.Close();
            }
            catch (Exception error)
            {
                throw new Exception("An error occurred while saving text: " + AText + " to file: " + AFileName + "." + Environment.NewLine +
                     "Error CNF-023 in " + FORM_NAME + ".SaveTextToFile(): " + error.Message);
            }
        }

        private string ErrorMsgDump(Exception err)
        {
            Exception ex2 = err;
            string errorMessage = "";
            while (ex2 != null)
            {
                errorMessage += ex2.ToString();
                ex2 = ex2.InnerException;
            }
            return errorMessage;
        }

        private void SetPanelMenuChecked()
        {
            try
            {
                if (dpTradeData.Visibility != DockVisibility.Visible)
                    barChkTradeData.Checked = false;
                //if (dpVaultedDocs.Visibility != DockVisibility.Visible)
                //barChkVaultedDocs.Checked = false;
                if (dpInboundQueue.Visibility != DockVisibility.Visible)
                    barChkInboundQueue.Checked = false;
                if (dpInboundViewer.Visibility != DockVisibility.Visible)
                    barChkInboundViewer.Checked = false;
                if (dpBrowserApps.Visibility != DockVisibility.Visible)
                    barChkBrowserApps.Checked = false;
            }
            catch (Exception error)
            {
                throw new Exception("An error occurred while setting docking panel status menu check marks." + Environment.NewLine +
                     "Error CNF-024 in " + FORM_NAME + ".SetPanelMenuChecked(): " + error.Message);
            }
        }

        private void initSkinsMenu()
        {
            try
            {
                ribbon.ForceInitialize();
                foreach (SkinContainer skin in SkinManager.Default.Skins)
                {
                    BarCheckItem item = ribbon.Items.CreateCheckItem(skin.SkinName, false);
                    item.Tag = skin.SkinName;
                    item.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(OnPaintStyleClick);
                    barSubPaintStyle.ItemLinks.Add(item);
                }
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while setting initial values for the GUI appearance Skins menu." + Environment.NewLine +
                     "Error CNF-025 in " + FORM_NAME + ".initSkinsMenu(): " + e.Message);
            }
        }

        private void SetUpdateErrorStatus(Boolean AIsError, string AErrorText)
        {
            try
            {
                barStaticUpdateError.Enabled = AIsError;
                barStaticUpdateError.Caption = AErrorText;
            }
            catch (Exception e)
            {
                throw new Exception("An error occurred while setting error text: " + AErrorText + "." + Environment.NewLine +
                     "Error CNF-026 in " + FORM_NAME + ".SetUpdateErrorStatus(): " + e.Message);
            }
        }

        private void DisplayRowCount()
        {
            try
            {
                barStaticVisibleRowCount.Caption = gridViewSummary.RowCount.ToString("0,0");
                barStaticTotalRowCount.Caption = summaryDataTable.DefaultView.Count.ToString("0,0");
                barStaticSelectedRowCount.Caption = "Rows selected: " + gridViewSummary.SelectedRowsCount.ToString("0");
                gridRqmt.Visible = (gridViewSummary.SelectedRowsCount > 0);
                gridConfirm.Visible = (gridViewSummary.SelectedRowsCount > 0);
                barbtnGetDealsheet.Enabled = (gridViewSummary.SelectedRowsCount == 1);
                barbtnGetContract.Enabled = gridConfirm.Visible && barbtnGetDealsheet.Enabled;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while setting the trade row count display." + Environment.NewLine +
                     "Error CNF-027 in " + FORM_NAME + ".DisplayRowCount(): " + ex.Message);
            }
        }

        private void DisplayTradeInfo()
        {
            try
            {
                string text = "";
                string tradeSys = "";
                string tradeId = "";
                string cptySn = "";

                GridView view = gridViewSummary;
                if (gridViewSummary.SelectedRowsCount == 1)
                {
                    int rowHandle = view.GetSelectedRows()[0];
                    tradeSys = view.GetRowCellDisplayText(rowHandle, "TrdSysCode").ToString();
                    tradeId = view.GetRowCellDisplayText(rowHandle, "TradeId").ToString();
                    cptySn = view.GetRowCellDisplayText(rowHandle, "CptySn").ToString();
                    text = tradeSys + ":" + tradeId + "  " + cptySn;
                    gridRqmt.Visible = true;
                    gridConfirm.Visible = true;
                    splitContainerTradeData.Panel2.ShowCaption = false;
                }
                else if (gridViewSummary.SelectedRowsCount > 1)
                {
                    text = "Multiple trades selected";
                    gridRqmt.Visible = false;
                    gridConfirm.Visible = false;
                    splitContainerTradeData.Panel2.ShowCaption = true;
                }
                else
                    text = "";

                barStaticCurrentTrade.Caption = text;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while setting the current trade row information display." + Environment.NewLine +
                     "Error CNF-028 in " + FORM_NAME + ".DisplayTradeInfo(): " + ex.Message);
            }
        }

        #endregion

        #region Event handlers

        private void rfrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (Environment.ExitCode >= 0)
                    WriteUserSettings();

            }
            catch (Exception except)
            {
                XtraMessageBox.Show("An error occurred while closing " + APP_NAME + "." + Environment.NewLine +
                       "Error CNF-029 in " + FORM_NAME + ".rfrmMain_FormClosing(): " + except.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Environment.ExitCode = 0;
            }
        }

        public void OnIncMessageCounter(object sender, System.EventArgs e)
        {
            try
            {
                int cachedRows = int.Parse(barStaticCachedRows.Caption);
                cachedRows++;
                barStaticCachedRows.Caption = cachedRows.ToString();
            }
            catch (Exception except)
            {
                throw new Exception("An error occurred while updating the number of currently cached data update message rows." + Environment.NewLine +
                     "Error CNF-030 in " + FORM_NAME + ".OnIncMessageCounter(): " + except.Message);
            }
        }

        public void OnResetMessageCounter(object sender, System.EventArgs e)
        {
            barStaticCachedRows.Caption = "0";
        }

        private void OnBeginGridUpdates(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                barBtnApplyCache.Enabled = false;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while beginning cached data grid update." + Environment.NewLine +
                       "Error CNF-031 in " + FORM_NAME + ".OnBeginGridUpdates(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void OnEndGridUpdates(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (gridViewSummary.FocusedRowHandle != invalidFocusedRowHandle)
                {
                    gridViewSummary.SelectRow(gridViewSummary.FocusedRowHandle);
                }
                barBtnApplyCache.Enabled = true;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while completing cached data grid update." + Environment.NewLine +
                       "Error CNF-032 in " + FORM_NAME + ".OnEndGridUpdates(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void OnPaintStyleClick(object sender, ItemClickEventArgs e)
        {
            defaultLookAndFeel.LookAndFeel.SetSkinStyle(e.Item.Tag.ToString());
        }

        private void barSubPaintStyle_Popup(object sender, EventArgs e)
        {
            try
            {
                foreach (BarItemLink link in barSubPaintStyle.ItemLinks)
                    ((BarCheckItem)link.Item).Checked = link.Item.Caption == defaultLookAndFeel.LookAndFeel.ActiveSkinName;
            }
            catch (Exception error)
            {
                XtraMessageBox.Show("An error occurred while updating current check status for GUI appearance Skin." + Environment.NewLine +
                       "Error CNF-033 in " + FORM_NAME + ".barSubPaintStyle_Popup(): " + error.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void barBtnExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            Environment.ExitCode = 0;
            //this.Close();  -- not necessary here because exit code causes form close to execute.
            System.Windows.Forms.Application.Exit();
        }

        private void barChkPanels_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            try
            {
                BarCheckItem checkItem = sender as BarCheckItem;
                dockManager.BeginUpdate();
                switch (checkItem.Name)
                {
                    case "barChkTradeData":
                        if (checkItem.Checked)
                            dpTradeData.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                        else
                            dpTradeData.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                        break;
                    //case "barChkVaultedDocs":
                    //   if (checkItem.Checked)
                    //      dpVaultedDocs.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                    //   else
                    //      dpVaultedDocs.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                    //   break;
                    case "barChkInboundQueue":
                        if (checkItem.Checked)
                            dpInboundQueue.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                        else
                            dpInboundQueue.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                        break;
                    case "barChkInboundViewer":
                        if (checkItem.Checked)
                            dpInboundViewer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                        else
                            dpInboundViewer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                        break;

                    case "barChkVaultedDocsPanel":
                        //if (checkItem.Checked)
                        //{
                        //    dpVaultDocViewer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                        //    if (dpVaultDocViewer.Dock == DockingStyle.Float)
                        //    {
                        //        dpVaultDocViewer.Location = new System.Drawing.Point(10, 10);
                        //    }
                        //}
                        //else
                        //    dpVaultDocViewer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                        if (checkItem.Checked)
                        {
                            string trdSysCode = string.Empty; string tokenNo = string.Empty;
                            trdSysCode = gridViewSummary.GetRowCellDisplayText(gridViewSummary.FocusedRowHandle, "TrdSysCode").ToString();
                            tokenNo = gridViewSummary.GetRowCellDisplayText(gridViewSummary.FocusedRowHandle, "TradeSysTicket").ToString();
                            openVaultViewer(trdSysCode, tokenNo);
                        }
                        else
                        {
                            CloseVaultViewer();

                        }
                        break;

                    case "barChkBrowserApps":
                        if (checkItem.Checked)
                            dpBrowserApps.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                        else
                            dpBrowserApps.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                        break;
                    case "barChkCustomFilterPanel":
                        pnlCustomFilter.Visible = checkItem.Checked;
                        break;
                    case "barChkGridFilterPanel":
                        gridViewSummary.OptionsView.ShowAutoFilterRow = checkItem.Checked;
                        break;
                    case "barChkGridGroupPanel":
                        gridViewSummary.OptionsView.ShowGroupPanel = checkItem.Checked;
                        break;
                    default:
                        throw new Exception("Internal Error: " + checkItem.Name + " not found");
                }
                dockManager.EndUpdate();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while updating the docking and filter panels status check marks." + Environment.NewLine +
                       "Error CNF-034 in " + FORM_NAME + ".barChkPanels_CheckedChanged(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dockPanel_ClosedPanel(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            try
            {
                DockPanel dockPanel = sender as DockPanel;
                switch (dockPanel.Name)
                {
                    case "dpTradeData":
                        barChkTradeData.Checked = false;
                        break;
                    case "dpVaultDocViewer":
                        barChkVaultedDocsPanel.Checked = false;
                        break;
                    case "dpInboundQueue":
                        barChkInboundQueue.Checked = false;
                        break;
                    case "dpInboundViewer":
                        barChkInboundViewer.Checked = false;
                        break;
                    case "dpBrowserApps":
                        barChkBrowserApps.Checked = false;
                        break;
                    default:
                        throw new Exception("Internal Error: " + dockPanel.Name + " not found");
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while updating the docking panel status check mark." + Environment.NewLine +
                       "Error CNF-035 in " + FORM_NAME + ".dockPanel_ClosedPanel(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bbCopyTradeIdToClipboard_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                GridView view = gridViewSummary;
                string tradeId = "";
                foreach (int rowHandle in view.GetSelectedRows())
                {
                    if (tradeId == "")
                        tradeId = view.GetRowCellDisplayText(rowHandle, "TradeSysTicket").ToString();
                    else
                        tradeId += Environment.NewLine + view.GetRowCellDisplayText(rowHandle, "TradeSysTicket").ToString();
                }
                Clipboard.SetText(tradeId);
            }
#pragma warning disable 0168
            //Disable warning...
            catch (Exception ex)
#pragma warning restore 0168
            {
                //XtraMessageBox.Show("miSummaryCopyTradeId_Click: " + ex.Message,
                //"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void barbtnHelp_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (barChkBrowserApps.Checked == false)
                barChkBrowserApps.Checked = true;
            //if (dpBrowserApps.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden)
            //   dpBrowserApps.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;

            tabctrlBrowserApps.SelectedTabPage = tabpgHelp;

            string address = GetDefaultWebAddress();
            //null test makes sure only loads once.
            if (address != "none" && webbrowserHelp.Document.Body.InnerHtml == null)
                GoToItem(GetCurrentVisibleBrowser(), address);
        }

        private void barBtnAbout_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                aboutForm.settingsDir = this.appSettingsDir;
                if (aboutForm.treeListAbout.Nodes.Count == 0)
                {
                    Sempra.Ops.VersionInfo versionInfo = Sempra.Ops.Utils.GetVersionInfo(System.Windows.Forms.Application.ExecutablePath);
                    aboutForm.lblVersion.Text = "Version: " + versionInfo.ProductVersion;

                    aboutForm.treeListAbout.BeginUnboundLoad();
                    string defValue = "";
                    IEnumerator enumerator = Properties.Settings.Default.Properties.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        defValue = "";
                        SettingsProperty prop = (SettingsProperty)enumerator.Current;
                        if (prop.Name.ToString() == "DisputedReasons")
                        {
                            defValue = GetFormattedCollectionProperty(Properties.Settings.Default.DisputedReasons);
                        }
                        else if (prop.Name.ToString() == "AddRqmtNoConfReason")
                        {
                            defValue = GetFormattedCollectionProperty(Properties.Settings.Default.AddRqmtNoConfReason);
                        }
                        else if (prop.Name.ToString() == "EmailDomainsDevAllowSendTo")
                        {
                            defValue = GetFormattedCollectionProperty(Properties.Settings.Default.EmailDomainsDevAllowSendTo);
                        }
                        else if (prop.Name.ToString() == "FaxNumbersDevAllowSendTo")
                        {
                            defValue = GetFormattedCollectionProperty(Properties.Settings.Default.FaxNumbersDevAllowSendTo);
                        }
                        else
                        {
                            //6/12/09 Israel -- Now displays actual instead of default values.
                            //defValue = prop.DefaultValue.ToString();
                            defValue = Properties.Settings.Default[prop.Name].ToString();
                        }

                        aboutForm.treeListAbout.AppendNode(new object[] { prop.Name, defValue }, -1);
                    }

                    //Get the solution name and add it to the property list.
                    //EnvDTE80.DTE2 dte;
                    //dte = (EnvDTE80.DTE2)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.12.0");
                    //string solFullName = dte.Solution.FullName;
                    //string solName = Path.GetFileNameWithoutExtension(solFullName);
                    //aboutForm.treeListAbout.AppendNode(new object[] { "VSBuildSolution", solName }, -1);

                    //Read the Inbound panel config files settings and place them into the about box.
                    //string currentPathName = System.Environment.CurrentDirectory + @"\" + INBOUND_PNL_SETTINGS;
                    //XmlDocument xdoc = new XmlDocument();
                    //xdoc.Load(currentPathName);
                    //XmlNode xnodes = xdoc.SelectSingleNode("/configuration/applicationSettings/ConfirmInbound.Properties.Settings");

                    //string propName = "";
                    //string propValue = "";
                    //foreach (XmlNode xnn in xnodes.ChildNodes)
                    //{
                    //    propName = "Inb: " + xnn.Attributes[0].Value;
                    //    propValue = xnn.InnerText;
                    //    aboutForm.treeListAbout.AppendNode(new object[] { propName, propValue }, -1);
                    //}              

                    aboutForm.treeListAbout.EndUnboundLoad();
                }

                if (aboutForm.ShowDialog(this) == DialogResult.OK)
                {
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while preparing to display the " + APP_NAME + " about box." + Environment.NewLine +
                       "Error CNF-036 in " + FORM_NAME + ".barBtnAbout_ItemClick(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetFormattedCollectionProperty(StringCollection pProperty)
        {
            string value = String.Empty;
            for (int i = 0; i < pProperty.Count; i++)
            {
                if (i > 0)
                    value += ",";
                value += pProperty[i];
            }
            return value;
        }

        private void bbSaveSummaryToExcel_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                saveToExcel(gridViewSummary);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while saving the Summary grid to MS Excel format." + Environment.NewLine +
                       "Error CNF-037 in " + FORM_NAME + ".bbSaveSummaryToExcel_ItemClick(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bbSaveRqmtToExcel_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (gridViewSummary.SelectedRowsCount != 1)
                {
                    XtraMessageBox.Show("You must select exactly one row to export a requirement to Excel.",
                     "Export Requirement", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                saveToExcel(gridViewRqmt);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while saving the Requirement grid to MS Excel format." + Environment.NewLine +
                       "Error CNF-038 in " + FORM_NAME + ".bbSaveRqmtToExcel_ItemClick(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bbLoadSummaryIntoExcel_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                string fileName = Path.Combine(appDocumentsDir, "~SummaryData.xls");

                DevExpress.XtraPrinting.XlsExportOptions xlOptions =
                   new DevExpress.XtraPrinting.XlsExportOptions(DevExpress.XtraPrinting.TextExportMode.Value, true);

                gridViewSummary.ExportToXls(@fileName, xlOptions);

                Process process = new Process();
                process.StartInfo.FileName = @fileName;
                process.StartInfo.UseShellExecute = true;
                process.Start();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while exporting the Summary grid to MS Excel format." + Environment.NewLine +
                       "Error CNF-039 in " + FORM_NAME + ".bbLoadSummaryIntoExcel_ItemClick(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saveToExcel(GridView AGridView)
        {
            try
            {
                saveFileDialog.Title = "Save Grid to Excel";
                saveFileDialog.InitialDirectory = saveToExcelDirectory;
                saveFileDialog.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.RestoreDirectory = true;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    saveToExcelDirectory = System.IO.Path.GetDirectoryName(saveFileDialog.FileName);
                    DevExpress.XtraPrinting.XlsExportOptions xlOptions =
                    new DevExpress.XtraPrinting.XlsExportOptions(DevExpress.XtraPrinting.TextExportMode.Value, true);
                    AGridView.ExportToXls(saveFileDialog.FileName, xlOptions);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while saving the " + AGridView.Name + " grid to MS Excel format." + Environment.NewLine +
                       "Error CNF-040 in " + FORM_NAME + ".saveToExcel(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void barStaticUpdateError_ItemDoubleClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SetUpdateErrorStatus(false, "");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while retrieving the current update error status." + Environment.NewLine +
                       "Error CNF-041 in " + FORM_NAME + ".barStaticUpdateError_ItemDoubleClick(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetStdFiltersTableView()
        {
            string filterStr = "";
            Boolean filterCriteriaSelected = false;
            try
            {
                if (barChkNB.Checked)
                {
                    //  filterStr = "InceptionDt <= CurrentBusnDt";
                }
                else
                {
                    //      filterStr = "(InceptionDt < CurrentBusnDt)";
                    filterStr = "(InceptionDt < " + "'" + DateTime.Now.ToShortDateString() + "')";
                    filterCriteriaSelected = true;
                }

                if (barChkRFA.Checked)
                {
                    if (filterCriteriaSelected)
                        filterStr += " and ";
                    filterStr += "(ReadyForFinalApprovalFlag = 'Y')";
                    filterCriteriaSelected = true;
                }
                if (barChkHP.Checked)
                {
                    if (filterCriteriaSelected)
                        filterStr += " and ";
                    filterStr += "(HasProblemFlag = 'Y')";
                    filterCriteriaSelected = true;
                }
                if (barChkQRY.Checked)
                {
                    if (filterCriteriaSelected)
                        filterStr += " and ";
                    filterStr += "(QryCode = 'Y')";
                    filterCriteriaSelected = true;
                }
                int faIndex = barEditFAComboBox.Items.IndexOf(barEditFA.EditValue);

                switch (faIndex)
                {
                    case 0:
                        if (filterCriteriaSelected)
                            filterStr += " and ";
                        filterStr += "(FinalApprovalFlag = 'N')";
                        filterCriteriaSelected = true;
                        break;
                    case 1:
                        if (filterCriteriaSelected)
                            filterStr += " and ";
                        filterStr += "(FinalApprovalFlag = 'Y')";
                        filterCriteriaSelected = true;
                        break;
                    case 2:
                        break;
                    default:
                        throw new Exception("Internal Error: Index=" + faIndex.ToString() + " not found");
                }

                //if (barChkFA.Checked)
                //{
                //   if (filterCriteriaSelected)
                //      filterStr += " and ";
                //   filterStr += "FinalApprovalFlag = 'Y'";
                //   if (!filterCriteriaSelected)
                //      filterCriteriaSelected = true;
                //}

                if (filterCriteriaSelected)
                {
                    dataSet.Tables["SummaryData"].DefaultView.RowFilter = filterStr;
                }
                else
                    dataSet.Tables["SummaryData"].DefaultView.RowFilter = "";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while setting the standard status bar filter settings to the grid data tables." + Environment.NewLine +
                       "Error CNF-042 in " + FORM_NAME + ".SetStdFiltersDataTableView(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetStdFiltersGridView()
        {
            string filterStr = "";
            Boolean filterCriteriaSelected = false;
            try
            {
                if (barChkShowNewBusiness.Checked)
                {
                    filterStr = "[InceptionDt] <= [CurrentBusnDt]";
                }
                else
                {
                    filterStr = "[InceptionDt] < [CurrentBusnDt]";
                    filterCriteriaSelected = true;
                }

                if (barChkReadyForFinalApproval.Checked)
                {
                    if (filterCriteriaSelected)
                        filterStr += " and ";
                    filterStr += "[ReadyForFinalApprovalFlag] = 'Y'";
                    if (!filterCriteriaSelected)
                        filterCriteriaSelected = true;
                }
                if (barChkHasProblems.Checked)
                {
                    if (filterCriteriaSelected)
                        filterStr += " and ";
                    filterStr += "[HasProblemFlag] = 'Y'";
                    if (!filterCriteriaSelected)
                        filterCriteriaSelected = true;
                }
                if (barChkFinalApproved.Checked)
                {
                    if (filterCriteriaSelected)
                        filterStr += " and ";
                    filterStr += "[FinalApprovalFlag] = 'Y'";
                    if (!filterCriteriaSelected)
                        filterCriteriaSelected = true;
                }

                if (filterCriteriaSelected)
                {
                    gridViewSummary.ActiveFilterString = filterStr;
                    gridViewSummary.ActiveFilterEnabled = true;
                }
                else
                    gridViewSummary.ActiveFilterEnabled = false;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while setting the standard status bar filter settings to the summary grid." + Environment.NewLine +
                       "Error CNF-043 in " + FORM_NAME + ".SetStdFiltersGridView(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void barStaticStdFilter_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (e.Item.Name == "barStaticNB")
                {
                    if (e.Item.OwnFont.Bold)
                    {
                        barChkShowNewBusiness.Checked = false;
                    }
                    else
                    {
                        barChkShowNewBusiness.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while setting the status bar New Business check mark." + Environment.NewLine +
                       "Error CNF-044 in " + FORM_NAME + ".barStaticStdFilter_ItemClick(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StdFilter_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (!initComplete)
                return;
            try
            {
                e.Item.ImageIndex = ((DevExpress.XtraBars.BarCheckItem)e.Item).Checked ? CHECKED : UNCHECKED;
                SetStdFilterHint((DevExpress.XtraBars.BarCheckItem)e.Item);
                gridMain.Invoke(new SetStdFiltersTableViewDelegate(SetStdFiltersTableView));
            }
            catch (IndexOutOfRangeException ex)
            {
                XtraMessageBox.Show("An error occurred while changing a status bar check setting." + Environment.NewLine +
                       "Error CNF-045 in " + FORM_NAME + ".StdFilter_CheckedChanged(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetStdFiltersTableViewCallStartMethod()
        {
            try
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(SetStdFiltersTableViewStartMethod));
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while starting a background update process." + Environment.NewLine +
                     "Error CNF-046 in " + FORM_NAME + ".SetStdFiltersTableViewCallStartMethod(): " + ex.Message);
            }
        }

        private void SetStdFiltersTableViewStartMethod(Object stateInfo)
        {
            try
            {
                SetStdFiltersTableView();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while requesting a process to update the standard filter." + Environment.NewLine +
                     "Error CNF-047 in " + FORM_NAME + ".SetStdFiltersTableViewStartMethod(): " + ex.Message);
            }
        }

        delegate void SetStdFiltersTableViewDelegate();

        private void barEditFA_EditValueChanged(object sender, EventArgs e)
        {
            if (!initComplete)
                return;
            try
            {
                int faIndex = barEditFAComboBox.Items.IndexOf(barEditFA.EditValue);
                barEditFA.Hint = barChkFAHints[faIndex];
                gridMain.Invoke(new SetStdFiltersTableViewDelegate(SetStdFiltersTableView));
            }
#pragma warning disable 0168
            //Disable warning...
            catch (IndexOutOfRangeException ex)
#pragma warning restore 0168
            {
            }
        }

        private void gridViewSummary_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (isHasUpdate)
                    CallEditRequirements();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while requesting the Edit Requirements form." + Environment.NewLine +
                         "Error CNF-048 in " + FORM_NAME + ".gridViewSummary_DoubleClick(): " + ex.Message,
                       MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyUserFilter(object sender, EventArgs e)
        {
            if (int.Parse(luedUserFilter.EditValue.ToString()) > NO_FILTER_VAL)
                try
                {
                    myTimer.Stop(); //Israel 12/15/2008 - Red X

                    int filterId = int.Parse(luedUserFilter.EditValue.ToString());
                    ApplyGridFilterByUserFilterIndex(filterId);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("An error occurred while applying a user filter." + Environment.NewLine +
                       "Error CNF-418 in " + FORM_NAME + ".ApplyUserFilter(): " + ex.Message,
                      MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    myTimer.Start(); //Israel 12/15/2008 - Red X
                }
            else
                XtraMessageBox.Show("No filter has been selected.");
        }

        private void barBtnApplyCache_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            ApplyCacheUpdates();
        }

        private void ApplyCacheUpdates()
        {
            try
            {
                myTimer.Stop();
                Cursor.Current = Cursors.WaitCursor;

                //Israel 12/21/2015 -- InboundQTabPnl1.AssociateDocument() throwing an execption 
                int rowHandle = gridViewSummary.FocusedRowHandle;

                //Israel 11/04/2015 Prevent 'automatic' switching of current tab page (remove SelectedTabPageIndex store/set to restore functionality)
                int currentTabPageIndex = inboundPnl1.tabCntrlMain.SelectedTabPageIndex;
                UpdateDataFromCache();
                ApplyInboundUpdates();

                inboundPnl1.tabCntrlMain.SelectedTabPageIndex = currentTabPageIndex;

                //Israel 12/21/2015 -- InboundQTabPnl1.AssociateDocument() throwing an execption 
                //  because InboundPnl.ActiveSummaryData was null after update.
                DataRow dr = gridViewSummary.GetDataRow(rowHandle);
                InboundPnl.ActiveSummaryData = CollectionHelper.CreateObjectFromDataRow<SummaryData>(dr);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while applying the cached message updates to the grids." + Environment.NewLine +
                         "Error CNF-049 in " + FORM_NAME + ".ApplyCacheUpdates(): " + ex.Message,
                       MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start();
                Cursor.Current = Cursors.Default;
            }
        }

        private void barbtnPrefs_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myTimer.Stop(); //Israel 12/15/2008 - Red X

                userPrefsForm.settingsDir = this.appSettingsDir;
                userPrefsForm.UpdateSeCptySnDisplay();
                userPrefsForm.UpdateCdtyDisplay();
                userPrefsForm.BackupPrefData();
                if (userPrefsForm.ShowDialog(this) == DialogResult.OK)
                {
                    isAutoDispDealsheet = userPrefsForm.cedAutoDispDealsheet.Checked;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while applying updates for the user preferences." + Environment.NewLine +
                         "Error CNF-050 in " + FORM_NAME + ".barbtnPrefs_ItemClick(): " + ex.Message,
                       MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start(); //Israel 12/15/2008 - Red X
            }
        }

        private void btnUserFilterAdd_Click(object sender, EventArgs e)
        {
            try
            {
                myTimer.Stop(); //Israel 12/15/2008 - Red X

                if (gridViewSummary.ActiveFilterString.Length == 0)
                {
                    XtraMessageBox.Show("Please define a filter first",
                    "Add User Filter", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    string formCaption = "Add User Filter";
                    changeUserFilterForm.settingsDir = this.appSettingsDir;
                    changeUserFilterForm.InitForm();
                    changeUserFilterForm.Text = formCaption;
                    if (changeUserFilterForm.ShowDialog(this) == DialogResult.OK)
                    {
                        bool okToUpdate = true;
                        string editFieldText = changeUserFilterForm.tedFilterDescr.Text.ToUpper();
                        foreach (DataRow row in userFiltersView.Select())
                        {
                            if (row["Descr"].ToString().ToUpper() == editFieldText)
                            {
                                XtraMessageBox.Show("Description has already been used." +
                                   Environment.NewLine + "Please select another description", formCaption);
                                okToUpdate = false;
                                break;
                            }
                        }

                        if (okToUpdate)
                        {
                            CallUpdateUserFilter(RequestType.Insert, 0,
                               changeUserFilterForm.tedFilterDescr.Text, gridViewSummary.ActiveFilterString);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while adding a new user-defined filter." + Environment.NewLine +
                         "Error CNF-051 in " + FORM_NAME + ".btnUserFilterAdd_Click(): " + ex.Message,
                       MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start(); //Israel 12/15/2008 - Red X
            }
        }

        private void btnUserFilterEdit_Click(object sender, EventArgs e)
        {
            if (int.Parse(luedUserFilter.EditValue.ToString()) > NO_FILTER_VAL)
                try
                {
                    myTimer.Stop(); //Israel 12/15/2008 - Red X

                    string formCaption = "Edit User Filter";
                    changeUserFilterForm.settingsDir = this.appSettingsDir;
                    changeUserFilterForm.InitForm();
                    changeUserFilterForm.tedFilterDescr.Text = luedUserFilter.Text;
                    changeUserFilterForm.Text = formCaption;
                    if (changeUserFilterForm.ShowDialog(this) == DialogResult.OK)
                    {
                        bool okToUpdate = true;
                        string editFieldText = changeUserFilterForm.tedFilterDescr.Text.ToUpper();
                        //Only check if they have changed the current filter setting.
                        if (editFieldText != luedUserFilter.Text.ToUpper())
                            foreach (DataRow row in userFiltersView.Select())
                            {
                                if (row["Descr"].ToString().ToUpper() == editFieldText)
                                {
                                    XtraMessageBox.Show("Description has already been used." +
                                       Environment.NewLine + "Please select another description", formCaption);
                                    okToUpdate = false;
                                    break;
                                }
                            }

                        if (okToUpdate)
                        {
                            CallUpdateUserFilter(RequestType.Update, int.Parse(luedUserFilter.EditValue.ToString()),
                               changeUserFilterForm.tedFilterDescr.Text, gridViewSummary.ActiveFilterString);
                        }
                    }
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("An error occurred while updating an existing user-defined filter." + Environment.NewLine +
                             "Error CNF-052 in " + FORM_NAME + ".btnUserFilterEdit_Click(): " + ex.Message,
                           MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    myTimer.Start(); //Israel 12/15/2008 - Red X
                }
            else
                XtraMessageBox.Show("No filter has been selected.");
        }

        private void btnUserFilterDelete_Click(object sender, EventArgs e)
        {
            if (int.Parse(luedUserFilter.EditValue.ToString()) > NO_FILTER_VAL)
                try
                {
                    myTimer.Stop(); //Israel 12/15/2008 - Red X

                    string formCaption = "Delete User Filter";
                    string msgboxText = formCaption + ": " + luedUserFilter.Text;
                    DialogResult result = XtraMessageBox.Show(msgboxText + "?", formCaption,
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.Yes)
                    {
                        CallUpdateUserFilter(RequestType.Delete, int.Parse(luedUserFilter.EditValue.ToString()),
                           changeUserFilterForm.tedFilterDescr.Text, gridViewSummary.ActiveFilterString);
                    }
                    else
                        XtraMessageBox.Show(msgboxText + " was cancelled.");
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("An error occurred while deleting an existing user-defined filter." + Environment.NewLine +
                             "Error CNF-053 in " + FORM_NAME + ".btnUserFilterDelete_Click(): " + ex.Message,
                           MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    myTimer.Start(); //Israel 12/15/2008 - Red X
                }
            else
                XtraMessageBox.Show("No filter has been selected.");
        }

        private void btnUserFilterBuilder_Click(object sender, EventArgs e)
        {
            try
            {
                myTimer.Stop(); //Israel 12/15/2008 - Red X

                gridViewSummary.ShowFilterEditor(gridViewSummary.FocusedColumn);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while requesting the grid column Filter Editor." + Environment.NewLine +
                         "Error CNF-054 in " + FORM_NAME + ".btnUserFilterBuilder_Click(): " + ex.Message,
                       MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start(); //Israel 12/15/2008 - Red X
            }
        }

        private void gridViewSummary_FilterEditorCreated(object sender, DevExpress.XtraGrid.Views.Base.FilterControlEventArgs e)
        {
            e.FilterControl.ShowGroupCommandsIcon = true;
            e.FilterControl.ShowOperandTypeIcon = true;
            e.FilterControl.ShowToolTips = true;
        }

        private void barBtnShowMessageFilter_ItemClick(object sender, ItemClickEventArgs e)
        {
            string filterMsg = webServiceFilter + "\n\n" + inbServiceFilter;
            XtraMessageBox.Show(filterMsg);
        }

        private void popupSummary_BeforePopup(object sender, CancelEventArgs e)
        {
            try
            {
                GridView view = gridViewSummary;
                bool correctTradeData = false;
                bool eConfirmCptyMatched = false;
                bool eConfirmBrokerMatched = false;
                bool eConfirmCptySent = false;
                bool eConfirmBrokerSent = false;
                bool sempraPaperNew = false;
                bool isDetActionsValueOfE = false;
                string trdSysCode = "";
                string cdtyCode = "";
                string sttlType = "";
                string rqmtCode = "";
                string rqmtStatus = "";

                if (view.SelectedRowsCount > 1)
                    correctTradeData = false;
                else if (view.SelectedRowsCount == 1)
                {
                    trdSysCode = view.GetRowCellDisplayText(view.FocusedRowHandle, "TrdSysCode").ToString();
                    cdtyCode = view.GetRowCellDisplayText(view.FocusedRowHandle, "CdtyCode").ToString();
                    sttlType = view.GetRowCellDisplayText(view.FocusedRowHandle, "SttlType").ToString();
                    correctTradeData = (trdSysCode == "JMS" ||
                       (trdSysCode == "AFF" && cdtyCode == "NGAS" && sttlType == "FNCL"));

                    //Read the RqmtData row
                    //Israel 11/7/2008 Reading summary view was skipping second rqmt for Our Paper, etc.
                    rqmtCode = "NONE";
                    rqmtStatus = "NONE";
                    string rqmtCode2 = "NONE";
                    string rqmtStatus2 = "NONE";
                    string filterStr = "TradeId = " + view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeId").ToString();
                    foreach (DataRow row in rqmtDataTable.Select(filterStr))
                    {
                        //Determine which requirement tabs will be turned on.
                        if (row["Rqmt"].ToString() == "ECONF")
                        {
                            rqmtCode = "ECONF";
                            rqmtStatus = row["Status"].ToString();
                        }
                        else if (row["Rqmt"].ToString() == "ECBKR")
                        {
                            rqmtCode2 = "ECBKR";
                            rqmtStatus2 = row["Status"].ToString();
                        }
                    }

                    //Israel 1/27/09 following 2 lines were commented on Phase II build.
                    //               rqmtCode = view.GetRowCellDisplayText(view.FocusedRowHandle, "SetcRqmt").ToString();
                    //               rqmtStatus = view.GetRowCellDisplayText(view.FocusedRowHandle, "SetcStatus").ToString();
                    eConfirmCptyMatched = (rqmtCode == "ECONF" && rqmtStatus != "MATCH" && rqmtStatus != "CXL");
                    eConfirmBrokerMatched = (rqmtCode2 == "ECBKR" && rqmtStatus2 != "MATCH" && rqmtStatus2 != "CXL");
                    eConfirmCptySent = (rqmtCode == "ECONF" && rqmtStatus == "CXL");
                    eConfirmBrokerSent = (rqmtCode2 == "ECBKR" && rqmtStatus2 == "CXL");

                    rqmtCode = "NONE";
                    rqmtStatus = "NONE";
                    foreach (DataRow row in rqmtDataTable.Select(filterStr))
                    {
                        //Determine which requirement tabs will be turned on.
                        if (row["Rqmt"].ToString() == SEMPRA_RQMT)
                        {
                            rqmtCode = SEMPRA_RQMT;
                            rqmtStatus = row["Status"].ToString();
                        }
                    }

                    sempraPaperNew = (rqmtCode == SEMPRA_RQMT && rqmtStatus != "NEW" &&
                                                             rqmtStatus != "EXT_REVIEW" &&
                                                             rqmtStatus != "TRADER");
                }

                //Israel 11/04/2015 -- Hide menu items for Set Determine Actions to Reprocess
                string determineActionFlag = "";
                GridView viewSummary = gridViewSummary;
                if (viewSummary.SelectedRowsCount > 0)
                {
                    int[] selectedRows = view.GetSelectedRows();
                    List<DataRow> rows = new List<DataRow>();

                    foreach (int rowHandle in selectedRows)
                        rows.Add(view.GetDataRow(rowHandle));

                    foreach (DataRow rowSummary in rows)
                    {
                        determineActionFlag = rowSummary["OpsDetActFlag"].ToString();
                        if (determineActionFlag == "E")
                        {
                            isDetActionsValueOfE = true;
                            break;
                        }
                    }
                }

                bbCorrectTradeData.Enabled = correctTradeData;
                barBtnEConfirmBrokerMatched.Enabled = eConfirmBrokerMatched;
                barBtnEConfirmMatched.Enabled = eConfirmCptyMatched;
                barbtnEConfirmCptySent.Enabled = eConfirmCptySent;
                barbtnEConfirmBrokerSent.Enabled = eConfirmBrokerSent;
                barbtnSempraPaperNew.Enabled = sempraPaperNew;
                barbtnReprocessJMSTrade.Enabled = trdSysCode == "JMS";
                bbDetermineActionsReprocess.Visibility = (isDetActionsValueOfE == true) ? BarItemVisibility.Always : BarItemVisibility.Never;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while preparing the Main Grid popup menu prior to display." + Environment.NewLine +
                         "Error CNF-055 in " + FORM_NAME + ".popupSummary_BeforePopup(): " + ex.Message,
                       MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region CreateCheckItem column menu
        DXMenuCheckItem CreateCheckItem(string caption, GridColumn column, FixedStyle style, Image image)
        {
            DXMenuCheckItem item = new DXMenuCheckItem(caption, column.Fixed == style, image, new EventHandler(OnFixedClick));
            item.Tag = new MenuInfo(column, style);
            if (caption == "Not Fixed")
                item.BeginGroup = true;
            return item;
        }

        void OnFixedClick(object sender, EventArgs e)
        {
            DXMenuItem item = sender as DXMenuItem;
            MenuInfo info = item.Tag as MenuInfo;
            if (info == null) return;
            info.Column.Fixed = info.Style;
        }

        class MenuInfo
        {
            public MenuInfo(GridColumn column, FixedStyle style)
            {
                this.Column = column;
                this.Style = style;
            }
            public FixedStyle Style;
            public GridColumn Column;
        }
        #endregion

        #region Update web service button events, calls, handlers

        private void CallGetUserRoles()
        {
            try
            {
                UserRoleDal userRoleDal = new UserRoleDal(sqlConnectionStr);
                IList<UserRoleView> userRoleList;
                //if (isTestMode)
                //    userRoleList = userRoleDal.GetAllStub();
                //else
                userRoleList = userRoleDal.GetAll(p_UserId);

                if (userRoleList != null)
                    for (int i = 0; i < userRoleList.Count; i++)
                    {
                        DataRow row = userRoleView.NewRow();
                        //row["UserId"] = userRoleResponse.@return.RoleList[i].UserId;
                        //row["RoleCode"] = userRoleResponse.@return.RoleList[i].Role;
                        row["RoleCode"] = userRoleList[i].RoleCode;
                        userRoleView.Rows.Add(row);
                    }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(APP_NAME + " was unable to log you on to the database." + Environment.NewLine +
                    "Error CNF-056 in CallGetUserRoles(): " + ex.Message,
                       MAIN_FORM_STOP_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Environment.ExitCode = -7;
                System.Windows.Forms.Application.Exit();
            }
        }

        private void CreateUserFiltersView()
        {
            try
            {
                userFiltersView = new System.Data.DataTable();
                userFiltersView.Columns.Add(new DataColumn("Id", typeof(int)));
                userFiltersView.Columns.Add(new DataColumn("Descr", typeof(string)));
                userFiltersView.Columns.Add(new DataColumn("FilterExpr", typeof(string)));
                userFiltersView.PrimaryKey = new DataColumn[] { userFiltersView.Columns["Id"] };
                userFiltersView.CaseSensitive = false;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the User Filter storage used internally by " + APP_NAME + "." + Environment.NewLine +
                     "Error CNF-057 in " + FORM_NAME + ".CreateUserFiltersView(): " + ex.Message);
            }
        }

        private void CallGetUserFilters()
        {
            try
            {
                UserFiltersOpsmgrDal userFiltersDal = new UserFiltersOpsmgrDal(sqlConnectionStr);
                IList<UserFiltersOpsmgrDto> userFilterList;
                //if (isTestMode)
                //    userFilterList = userFiltersDal.GetAllStub();
                //else
                userFilterList = userFiltersDal.GetAll(p_UserId);
                if (userFilterList != null)
                    for (int i = 0; i < userFilterList.Count; i++)
                    {
                        DataRow row = userFiltersView.NewRow();
                        row["Id"] = userFilterList[i].Id;
                        row["Descr"] = userFilterList[i].Descr;
                        row["FilterExpr"] = userFilterList[i].FilterExpr;
                        userFiltersView.Rows.Add(row);
                    }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while populating the User Filter data used internally by " + APP_NAME + "." + Environment.NewLine +
                     "Error CNF-058 in " + FORM_NAME + ".CallGetUserFilters(): " + ex.Message);
            }
        }

        private void CallUpdateUserFilter(RequestType ARequestType, int AFilterId, string ADescr,
           string AFilterExpr)
        {
            try
            {
                UserFiltersOpsmgrDal userFiltersDal = new UserFiltersOpsmgrDal(sqlConnectionStr);
                UserFiltersOpsmgrDto filterData = new UserFiltersOpsmgrDto();
                filterData.Id = AFilterId;
                filterData.UserId = p_UserId;
                filterData.Descr = ADescr;
                filterData.FilterExpr = AFilterExpr;

                switch (ARequestType)
                {
                    case RequestType.Insert:
                        {
                            Int32 newId = userFiltersDal.Insert(filterData);
                            if (newId > 0)
                            {
                                DataRow row = userFiltersView.NewRow();
                                row["Id"] = newId;
                                row["Descr"] = filterData.Descr;
                                row["FilterExpr"] = filterData.FilterExpr;
                                userFiltersView.Rows.Add(row);
                                luedUserFilter.EditValue = newId;
                            }
                            break;
                        }
                    case RequestType.Update:
                        {
                            Int32 rowsUpdated = userFiltersDal.Update(filterData);
                            if (rowsUpdated > 0)
                            {
                                DataRow row = userFiltersView.Rows.Find(filterData.Id);
                                row["Descr"] = filterData.Descr;
                                row["FilterExpr"] = filterData.FilterExpr;
                                userFiltersView.AcceptChanges();
                            }
                            break;
                        }
                    case RequestType.Delete:
                        {
                            Int32 rowsDeleted = userFiltersDal.Delete(filterData.Id);
                            if (rowsDeleted > 0)
                            {
                                DataRow row = userFiltersView.Rows.Find(filterData.Id);
                                row.Delete();
                                userFiltersView.AcceptChanges();
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the User Filter data used internally by " + APP_NAME + "." + Environment.NewLine +
                    "Error CNF-059 in " + FORM_NAME + ".CallUpdateUserFilter(): " + ex.Message);
            }
        }

        private void ApplyGridFilterByUserFilterIndex(int AFilterId)
        {
            try
            {
                DataRow row = userFiltersView.Rows.Find(AFilterId);
                string filterExpr = row["FilterExpr"].ToString();
                if (gridViewSummary.ActiveFilterString != filterExpr)
                    gridViewSummary.ActiveFilterString = filterExpr;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while User Filter: " + AFilterId.ToString() + " to the Summary Grid." + Environment.NewLine +
                    "Error CNF-060 in " + FORM_NAME + ".ApplyGridFilterByUserFilterIndex(): " + ex.Message);
            }
        }

        private int GetMostRecentUserFilterIndex()
        {
            //Determines if the current user filter is active. If so, it returns the
            //index. If not it returns NO_FILTER_VAL.
            int filterIndexResult = NO_FILTER_VAL;
            try
            {
                int currentIndex = int.Parse(luedUserFilter.EditValue.ToString());
                DataRow row = userFiltersView.Rows.Find(currentIndex);
                if (row != null)
                {
                    string currentFilterExpr = row["FilterExpr"].ToString();
                    if (gridViewSummary.ActiveFilterString == currentFilterExpr)
                        filterIndexResult = currentIndex;
                }
                return filterIndexResult;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the current user filter." + Environment.NewLine +
                    "Error CNF-061 in " + FORM_NAME + ".GetMostRecentUserFilterIndex(): " + ex.Message);
            }
        }

        private void CallEditRequirements()
        {
            try
            {
                //Setup...beginload prevents data entry event handlers from firing.
                myTimer.Stop(); //Israel 12/15/2008 - Red X

                editRqmtForm.BeginDataLoad();
                editRqmtForm.ClearAllFields();
                editRqmtForm.InitOldDataVariables();
                editRqmtForm.btnEditRqmtSaveAndApprove.Enabled = true;
                editRqmtForm.gluedSempraStatus.Enabled = true;  //sometimes gets set to false below...
                editRqmtForm.gluedSempraStatus.ForeColor = Color.Black;

                bool[] tabs = new bool[frmEditRqmt.RQMT_ARRAY_MAX];
                for (int i = 0; i < frmEditRqmt.RQMT_ARRAY_MAX; i++)
                    tabs[i] = false;
                string tradingSys = "***";
                string ticket = "***";
                string tradeId = "** Multiple Ids ***";
                string cptySn = "***";
                string bookingCoSn = "***";
                string cptyTradeId = "";
                string tradeCmt = "";

                GridView view = gridViewSummary;

                //Prepare to call form for single row selected.
                //Single row loads data and user performs standard data entry.
                if (view.SelectedRowsCount == 1)
                {
                    editRqmtForm.SingleOrMultiMode = frmEditRqmt.SINGLE;
                    //Setup Header data
                    tradingSys = view.GetRowCellDisplayText(view.FocusedRowHandle, "TrdSysCode").ToString();
                    ticket = view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeSysTicket").ToString();
                    tradeId = view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeId").ToString();
                    cptySn = view.GetRowCellDisplayText(view.FocusedRowHandle, "CptySn").ToString();
                    bookingCoSn = view.GetRowCellDisplayText(view.FocusedRowHandle, "BookingCoSn").ToString();

                    ArrayList sempraPaperStatusList = new ArrayList();
                    string filterStr = "TradeId = " + view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeId").ToString();
                    //Read the RqmtData row
                    //Israel 10/3/2008 Reading summary view was skipping second rqmt for Our Paper, etc.
                    foreach (DataRow row in rqmtDataTable.Select(filterStr))
                    {
                        //Determine which requirement tabs will be turned on.
                        if (row["Rqmt"].ToString() == SEMPRA_RQMT)
                        {
                            tabs[frmEditRqmt.RQMT_TYPE_SEMPRA] = true;
                            sempraPaperStatusList.Add(row["Status"].ToString());
                        }
                        else if (row["Rqmt"].ToString() == "XQCCP")
                            tabs[frmEditRqmt.RQMT_TYPE_CPTY] = true;
                        else if (row["Rqmt"].ToString() == "XQBBP")
                            tabs[frmEditRqmt.RQMT_TYPE_BROKER] = true;
                        else if (row["Rqmt"].ToString() == "NOCNF")
                            tabs[frmEditRqmt.RQMT_TYPE_NOCONF] = true;
                        //else if (row["Rqmt"].ToString() == "ECONF")
                        //    tabs[frmEditRqmt.RQMT_TYPE_ECONFIRM] = true;
                        //else if (row["Rqmt"].ToString() == "ECBKR")
                        //    tabs[frmEditRqmt.RQMT_TYPE_ECONFIRM_BROKER] = true;
                        //else if (row["Rqmt"].ToString() == "EFET")
                        //    tabs[frmEditRqmt.RQMT_TYPE_EFET_CPTY] = true;
                        //else if (row["Rqmt"].ToString() == "EFBKR")
                        //    tabs[frmEditRqmt.RQMT_TYPE_EFET_BROKER] = true;
                        else if (row["Rqmt"].ToString() == "VBCP")
                            tabs[frmEditRqmt.RQMT_TYPE_VERBAL] = true;
                        //else if (row["Rqmt"].ToString() == "MISC")
                        //tabs[frmEditRqmt.RQMT_TYPE_MISC] = true;
                    }

                    editRqmtForm.SetTabsVisible(tabs);

                    //Setup filter string to pass in following routine
                    //string filterStr = "TradeId = " + view.GetRowCellDisplayText(rowHandle, "TradeId").ToString();

                    //CallSetRqmtData sets data on form for current trade
                    if (tabs[frmEditRqmt.RQMT_TYPE_SEMPRA])
                        CallSetRqmtData(frmEditRqmt.RQMT_TYPE_SEMPRA, view, view.FocusedRowHandle, filterStr);
                    if (tabs[frmEditRqmt.RQMT_TYPE_CPTY])
                        CallSetRqmtData(frmEditRqmt.RQMT_TYPE_CPTY, view, view.FocusedRowHandle, filterStr);
                    if (tabs[frmEditRqmt.RQMT_TYPE_BROKER])
                        CallSetRqmtData(frmEditRqmt.RQMT_TYPE_BROKER, view, view.FocusedRowHandle, filterStr);
                    if (tabs[frmEditRqmt.RQMT_TYPE_NOCONF])
                        CallSetRqmtData(frmEditRqmt.RQMT_TYPE_NOCONF, view, view.FocusedRowHandle, filterStr);
                    //if (tabs[frmEditRqmt.RQMT_TYPE_ECONFIRM])
                    //    CallSetRqmtData(frmEditRqmt.RQMT_TYPE_ECONFIRM, view, view.FocusedRowHandle, filterStr);
                    //if (tabs[frmEditRqmt.RQMT_TYPE_ECONFIRM_BROKER])
                    //    CallSetRqmtData(frmEditRqmt.RQMT_TYPE_ECONFIRM_BROKER, view, view.FocusedRowHandle, filterStr);
                    //if (tabs[frmEditRqmt.RQMT_TYPE_EFET_CPTY])
                    //    CallSetRqmtData(frmEditRqmt.RQMT_TYPE_EFET_CPTY, view, view.FocusedRowHandle, filterStr);
                    //if (tabs[frmEditRqmt.RQMT_TYPE_EFET_BROKER])
                    //    CallSetRqmtData(frmEditRqmt.RQMT_TYPE_EFET_BROKER, view, view.FocusedRowHandle, filterStr);
                    if (tabs[frmEditRqmt.RQMT_TYPE_VERBAL])
                        CallSetRqmtData(frmEditRqmt.RQMT_TYPE_VERBAL, view, view.FocusedRowHandle, filterStr);
                    //if (tabs[frmEditRqmt.RQMT_TYPE_MISC])
                    //CallSetRqmtData(frmEditRqmt.RQMT_TYPE_MISC, view, view.FocusedRowHandle, filterStr);

                    //Setup cpty trade id and trade comment
                    //Israel 12/23/2015 -- Added support for CptyTradeId
                    editRqmtForm.SetCptyTradeIdEnabled(true);
                    cptyTradeId = view.GetRowCellDisplayText(view.FocusedRowHandle, "CptyTradeId").ToString();
                    tradeCmt = view.GetRowCellDisplayText(view.FocusedRowHandle, "Cmt").ToString();
                    editRqmtForm.SetTradeData(cptyTradeId, tradeCmt);
                    //For each Our Paper requirement check to see if any of the NoEdit statuses
                    //are contained in it. If so, lock down the status so it can't be changed.
                    if (sempraPaperStatusList.Count > 0)
                    {
                        for (int i = 0; i < sempraPaperStatusList.Count; i++)
                            if (noEditSempraRqmtStatus.IndexOf(sempraPaperStatusList[i]) > -1)
                            {
                                editRqmtForm.gluedSempraStatus.Enabled = false;
                                editRqmtForm.gluedSempraStatus.ForeColor = Color.Black;
                                editRqmtForm.btnEditRqmtSaveAndApprove.Enabled = false;
                            }
                    }
                }
                //Multi-Record data entry allows mass updating of multiple trades
                //Form is blank and only data that is entered updates appropriate requirements.
                else
                {
                    editRqmtForm.SingleOrMultiMode = frmEditRqmt.MULTI;
                    //editRqmtForm.btnEditRqmtSaveAndApprove.Enabled = false;

                    //Get the list ahead of time, since the foreach code changes the list's contents/order
                    int[] selectedRows = view.GetSelectedRows();
                    List<DataRow> rows = new List<DataRow>();

                    foreach (int rowHandle in selectedRows)
                        rows.Add(view.GetDataRow(rowHandle));

                    foreach (DataRow rowSummary in rows)
                    {

                        //foreach (int rowHandle in view.GetSelectedRows())
                        //{
                        string filterStr2 = "TradeId = " + rowSummary["TradeId"].ToString();
                        foreach (DataRow row in rqmtDataTable.Select(filterStr2))
                        {
                            //Since all tabs are intialized to false, this makes sure if at least one requirement
                            //of each type is selected there will be a tab for it.
                            if (row["Rqmt"].ToString() == SEMPRA_RQMT)
                                tabs[frmEditRqmt.RQMT_TYPE_SEMPRA] = true;
                            else if (row["Rqmt"].ToString() == "XQCCP")
                                tabs[frmEditRqmt.RQMT_TYPE_CPTY] = true;
                            else if (row["Rqmt"].ToString() == "XQBBP")
                                tabs[frmEditRqmt.RQMT_TYPE_BROKER] = true;
                            else if (row["Rqmt"].ToString() == "NOCNF")
                                tabs[frmEditRqmt.RQMT_TYPE_NOCONF] = true;
                            //else if (row["Rqmt"].ToString() == "ECONF")
                            //    tabs[frmEditRqmt.RQMT_TYPE_ECONFIRM] = true;
                            //else if (row["Rqmt"].ToString() == "ECBKR")
                            //    tabs[frmEditRqmt.RQMT_TYPE_ECONFIRM_BROKER] = true;
                            //else if (row["Rqmt"].ToString() == "EFET")
                            //    tabs[frmEditRqmt.RQMT_TYPE_EFET_CPTY] = true;
                            //else if (row["Rqmt"].ToString() == "EFBKR")
                            //    tabs[frmEditRqmt.RQMT_TYPE_EFET_BROKER] = true;
                            else if (row["Rqmt"].ToString() == "VBCP")
                                tabs[frmEditRqmt.RQMT_TYPE_VERBAL] = true;
                            //else if (row["Rqmt"].ToString() == "MISC")
                            //tabs[frmEditRqmt.RQMT_TYPE_MISC] = true;
                        }
                    }
                    editRqmtForm.SetTabsVisible(tabs);
                    editRqmtForm.SetCptyTradeIdEnabled(true);
                    editRqmtForm.SetTabCtrlEditRqmtEnabled(false);
                }

                editRqmtForm.SetHeaderLabels(tradingSys, ticket, cptySn, bookingCoSn);
                editRqmtForm.EndDataLoad();
                //Call the form
                if (editRqmtForm.ShowDialog(this) == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    //Get back the data that was entered
                    bool[] updatedRqmts = new bool[frmEditRqmt.RQMT_ARRAY_MAX];
                    string[] updatedStatusCodes = new string[frmEditRqmt.RQMT_ARRAY_MAX];
                    DateTime[] updatedStatusDates = new DateTime[frmEditRqmt.RQMT_ARRAY_MAX];
                    string[] updatedSecondChecks = new string[frmEditRqmt.RQMT_ARRAY_MAX];
                    string[] updatedReferences = new string[frmEditRqmt.RQMT_ARRAY_MAX];
                    string[] updatedRqmtCmts = new string[frmEditRqmt.RQMT_ARRAY_MAX];
                    string updatedCptyTradeId;
                    string updatedTradeCmt;
                    bool[,] changedFields = new bool[frmEditRqmt.RQMT_ARRAY_MAX, frmEditRqmt.FIELD_ARRAY_MAX];
                    bool isCptyTradeIdChanged = false;
                    bool isTradeCmtChanged = false;

                    updatedRqmts = editRqmtForm.GetUpdatedRqmts();
                    updatedStatusCodes = editRqmtForm.GetStatusCodes();
                    updatedStatusDates = editRqmtForm.GetStatusDates();
                    updatedSecondChecks = editRqmtForm.GetSecondChecks();
                    updatedReferences = editRqmtForm.GetReferences();
                    updatedRqmtCmts = editRqmtForm.GetRqmtCmts();
                    updatedCptyTradeId = editRqmtForm.GetCptyTradeId();
                    updatedTradeCmt = editRqmtForm.GetTradeCmt();
                    changedFields = editRqmtForm.GetChangedFields();
                    isCptyTradeIdChanged = editRqmtForm.IsCptyTradeIdChanged();
                    isTradeCmtChanged = editRqmtForm.IsTradeCmtChanged();

                    //Find out if any requirements need to be updated.
                    bool isAnyRqmtsUpdated = false;
                    for (int i = 0; i < frmEditRqmt.RQMT_ARRAY_MAX; i++)
                        if (updatedRqmts[i])
                            isAnyRqmtsUpdated = true;

                    //Call the routine that invokes the update procedure.
                    if (isAnyRqmtsUpdated && view.SelectedRowsCount == 1)
                        CallUpdateEditRqmt(updatedRqmts, updatedStatusCodes, updatedStatusDates, updatedSecondChecks,
                           updatedReferences, updatedRqmtCmts, changedFields);

                    //Israel 12/23/2015 -- Added support for CptyTradeId
                    //Int32 tradeIdInt = Int32.Parse(tradeId);
                    if (isCptyTradeIdChanged)
                        CallUpdateCptyTradeId(updatedCptyTradeId);

                    if (isTradeCmtChanged)
                        CallUpdateTradeCmt(updatedTradeCmt);

                    if (isForceFinalApprove && editRqmtForm.isFinalApprove)
                        CallUpdateFinalApproval("Y", "N");
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while requesting the Requirment Editor." + Environment.NewLine +
                        "Error CNF-062 in " + FORM_NAME + ".CallEditRequirements(): " + ex.Message,
                      MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start(); //Israel 12/15/2008 - Red X
                Cursor.Current = Cursors.Default;
            }
        }

        private void bbEditRequirements_ItemClick(object sender, ItemClickEventArgs e)
        {
            CallEditRequirements();
        }

        private void CallUpdateCptyTradeId(Int32 ATradeId, string ACptyTradeId)
        {
            //Israel 12/23/2015 -- Added support for CptyTradeId
            try
            {
                //Update the database
                TradeSummaryDal tradeSummaryDal = new TradeSummaryDal(sqlConnectionStr);
                int rowsUpdated = 0;
                rowsUpdated = tradeSummaryDal.UpdateCptyTradeId(ATradeId, ACptyTradeId);

                //Update the current row.
                GridView view = gridViewSummary;
                if (view.SelectedRowsCount == 1)
                {
                    DataRow row = view.GetDataRow(view.FocusedRowHandle);
                    UpdateLocalSummaryData(row, "CptyTradeId", ACptyTradeId);
                }
                else if (view.SelectedRowsCount == 0)
                {
                    XtraMessageBox.Show("No trade was selected. Pleaese select a trade." + Environment.NewLine +
                        "Error CNF-552 in " + FORM_NAME + ".CallUpdateCptyTradeId().",
                      MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to update a cpty trade id." + Environment.NewLine +
                    "Error CNF-553 in " + FORM_NAME + ".CallUpdateCptyTradeId(): " + ex.Message);
            }
        }

        private void CallUpdateCptyTradeId(string ACptyTradeId)
        {
            
            try
            {                
                GridView view = gridViewSummary;
                int[] selectedRows = view.GetSelectedRows();
                List<DataRow> rows = new List<DataRow>();
                foreach (int rowHandle in selectedRows)
                    rows.Add(view.GetDataRow(rowHandle));

                //Create a data input list to pass to the update process
                TradeSummaryDal tradeSummaryDal = new TradeSummaryDal(sqlConnectionStr);
                List<TradeSummaryDto> rowDataList = new List<TradeSummaryDto>();
                foreach (DataRow row in rows)
                {
                    TradeSummaryDto rowData = new TradeSummaryDto();
                    rowData.TradeId = int.Parse(row["TradeId"].ToString());
                    rowData.CptyTradeId = ACptyTradeId;
                    rowDataList.Add(rowData);
                }
                //Update the database
                int rowsUpdated = 0;
                rowsUpdated = tradeSummaryDal.UpdateCptyTradeIds(rowDataList);
                if (view.SelectedRowsCount == 1)
                {
                    DataRow row = view.GetDataRow(view.FocusedRowHandle);
                    UpdateLocalSummaryData(row, "CptyTradeId", ACptyTradeId);
                }
                else if (view.SelectedRowsCount == 0)
                {
                    XtraMessageBox.Show("No trade was selected. Pleaese select a trade." + Environment.NewLine +
                        "Error CNF-552 in " + FORM_NAME + ".CallUpdateCptyTradeId().",
                      MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to update a cpty trade id." + Environment.NewLine +
                    "Error CNF-553 in " + FORM_NAME + ".CallUpdateCptyTradeId(): " + ex.Message);
            }
        }

        private void CallUpdateTradeCmt(string ATradeCmt)
        {
            try
            {
                GridView view = gridViewSummary;

                //Read through all selected rows.
                int[] selectedRows = view.GetSelectedRows();
                List<DataRow> rows = new List<DataRow>();

                foreach (int rowHandle in selectedRows)
                    rows.Add(view.GetDataRow(rowHandle));

                //Create a data input list to pass to the update process
                TradeSummaryDal tradeSummaryDal = new TradeSummaryDal(sqlConnectionStr);
                List<TradeSummaryDto> rowDataList = new List<TradeSummaryDto>();
                foreach (DataRow row in rows)
                {
                    TradeSummaryDto rowData = new TradeSummaryDto();
                    rowData.TradeId = int.Parse(row["TradeId"].ToString());
                    rowData.Cmt = ATradeCmt;
                    rowDataList.Add(rowData);
                }

                //Update the database
                int rowsUpdated = 0;
                rowsUpdated = tradeSummaryDal.UpdateCmts(rowDataList);

                //Update the current row.
                if (view.SelectedRowsCount == 1)
                {
                    DataRow row = view.GetDataRow(view.FocusedRowHandle);
                    UpdateLocalSummaryData(row, "Cmt", ATradeCmt);
                }
                else if (view.SelectedRowsCount == 0)
                {
                    XtraMessageBox.Show("No trade was selected. Pleaese select a trade." + Environment.NewLine +
                        "Error CNF-063 in " + FORM_NAME + ".CallUpdateTradeCmt().",
                      MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to update a trade comment." + Environment.NewLine +
                    "Error CNF-064 in " + FORM_NAME + ".CallUpdateTradeCmt(): " + ex.Message);
            }
        }

        private void CallUpdateEditRqmt(bool[] AUpdatedRqmts, string[] AStatusCodes, DateTime[] AStatusDates,
                     string[] ASecondChecks, string[] ARefs, string[] ARqmtCmts, bool[,] AChangedFields)
        {
            try
            {
                string filterStr = "";
                int rqmtsToUpdate = 0;
                //Find out how many requirements need to be updated.
                for (int i = 0; i < frmEditRqmt.RQMT_ARRAY_MAX; i++)
                    if (AUpdatedRqmts[i])
                        rqmtsToUpdate++;

                GridView view = gridViewSummary;
                //Calculate how many individual requirements will be updated.
                int rowsToUpdate = view.SelectedRowsCount * rqmtsToUpdate;
                int editRqmtRecIdx = 0;
                //int prelimApprvOverride = 0;
                string finalApproval = "N";
                int finalApprovalIgnored = 0;
                //tradeRqmtRequest[] rqmtRequestArray = new tradeRqmtRequest[rowsToUpdate];
                List<TradeRqmtDto> tradeRqmtDataList = new List<TradeRqmtDto>();

                //int tradeId = 0;
                bool isSingleRow = view.SelectedRowsCount == 1;
                //if (isSingleRow)
                //tradeId = view.GetRowCellDisplayText(rowHandle, "TradeId").ToString();

                //Get the list ahead of time, since the foreach code changes the list's contents/order
                int[] selectedRows = view.GetSelectedRows();
                List<DataRow> rows = new List<DataRow>();

                foreach (int rowHandle in selectedRows)
                    rows.Add(view.GetDataRow(rowHandle));

                //Read through all selected rows.
                foreach (DataRow rowSummary in rows)
                {
                    //foreach (int rowHandle in view.GetSelectedRows())
                    //{
                    finalApproval = rowSummary["FinalApprovalFlag"].ToString();
                    if (finalApproval == "Y")
                    {
                        finalApprovalIgnored++;
                        continue;
                    }

                    //For each row read through all requirements
                    for (int i = 0; i < frmEditRqmt.RQMT_ARRAY_MAX; i++)
                        //Process only those requirements that have been edited. 
                        if (AUpdatedRqmts[i])
                        {
                            //Set a filter for the edited rqmt
                            string tradeId = rowSummary["TradeId"].ToString();
                            filterStr = "TradeId = " + tradeId +
                               " and Rqmt = '" + editRqmtForm.RQMT_CODES[i] + "'";
                            //Read the RqmtData row
                            foreach (DataRow row in rqmtDataTable.Select(filterStr))
                            {
                                TradeRqmtDto tradeRqmtDto = new TradeRqmtDto();
                                tradeRqmtDto.TradeId = Int32.Parse(tradeId);
                                tradeRqmtDto.Id = Int32.Parse(row["Id"].ToString());
                                tradeRqmtDto.RqmtCode = editRqmtForm.RQMT_CODES[i];
                                tradeRqmtDto.StatusDateSpecified = true;

                                //Single mode- since the current data value is passed to the edit form
                                //             we also get it back so just reload it.
                                //Multi mode- if data was changed on the form get the form data. 
                                //            Otherwise, reload the current data from the row.

                                string statusCode = AStatusCodes[i];
                                //Since there is no way to do data entry validation for multis we need to do it here.
                                //If the same person who prelim approved a trade tries to also approve it we catch
                                //it here and set it back to prelim.
                                //Israel 9/28/2015
                                //if (editRqmtForm.SingleOrMultiMode == frmEditRqmt.MULTI &&
                                //    AStatusCodes[i] == "APPR" &&
                                //    row["SecondCheckFlag"].ToString() == "Y" &&
                                //    toolbarOrWindowsUserId == row["PrelimAppr"].ToString())
                                //{
                                //    statusCode = "PRELIM";
                                //    prelimApprvOverride++;
                                //}

                                //6/9/09 Israel - Don't allow Our Paper status change if still in contract edit mode.
                                bool okToEdit = true;
                                if (row["Rqmt"].ToString() == SEMPRA_RQMT &&
                                    noEditSempraRqmtStatus.IndexOf(row["Status"].ToString()) > -1)
                                    okToEdit = false;

                                if ((editRqmtForm.SingleOrMultiMode == frmEditRqmt.SINGLE ||
                                    (editRqmtForm.SingleOrMultiMode == frmEditRqmt.MULTI && AChangedFields[i, frmEditRqmt.FIELD_STATUS_CODE])) &&
                                    okToEdit)
                                    //rqmtRequestArray[editRqmtRecIdx].status = statusCode;
                                    tradeRqmtDto.StatusCode = statusCode;
                                else
                                    //rqmtRequestArray[editRqmtRecIdx].status = row["Status"].ToString();
                                    tradeRqmtDto.StatusCode = row["Status"].ToString(); ;

                                if (editRqmtForm.SingleOrMultiMode == frmEditRqmt.SINGLE ||
                                   (editRqmtForm.SingleOrMultiMode == frmEditRqmt.MULTI &&
                                    AChangedFields[i, frmEditRqmt.FIELD_STATUS_DATE]))
                                    //rqmtRequestArray[editRqmtRecIdx].statusDate = AStatusDates[i];
                                    tradeRqmtDto.CompletedDt = AStatusDates[i];
                                else
                                    //rqmtRequestArray[editRqmtRecIdx].statusDate = (DateTime)row["CompletedDt"];
                                    tradeRqmtDto.CompletedDt = (DateTime)row["CompletedDt"]; ;

                                if (editRqmtForm.SingleOrMultiMode == frmEditRqmt.SINGLE ||
                                   (editRqmtForm.SingleOrMultiMode == frmEditRqmt.MULTI &&
                                    AChangedFields[i, frmEditRqmt.FIELD_SECOND_CHECK]))
                                    //rqmtRequestArray[editRqmtRecIdx].secondCheck = ASecondChecks[i];
                                    tradeRqmtDto.SecondCheckFlag = ASecondChecks[i];
                                else
                                    //rqmtRequestArray[editRqmtRecIdx].secondCheck = row["SecondCheckFlag"].ToString();
                                    tradeRqmtDto.SecondCheckFlag = row["SecondCheckFlag"].ToString();

                                if (editRqmtForm.SingleOrMultiMode == frmEditRqmt.SINGLE ||
                                   (editRqmtForm.SingleOrMultiMode == frmEditRqmt.MULTI &&
                                    AChangedFields[i, frmEditRqmt.FIELD_REFERENCE]))
                                    //rqmtRequestArray[editRqmtRecIdx].reference = ARefs[i];
                                    tradeRqmtDto.Reference = ARefs[i];
                                else
                                    //rqmtRequestArray[editRqmtRecIdx].reference = row["Reference"].ToString();
                                    tradeRqmtDto.Reference = row["Reference"].ToString();

                                if (editRqmtForm.SingleOrMultiMode == frmEditRqmt.SINGLE ||
                                   (editRqmtForm.SingleOrMultiMode == frmEditRqmt.MULTI &&
                                    AChangedFields[i, frmEditRqmt.FIELD_RQMT_CMT]))
                                    //rqmtRequestArray[editRqmtRecIdx].comment = ARqmtCmts[i];
                                    tradeRqmtDto.Cmt = ARqmtCmts[i];
                                else
                                    //rqmtRequestArray[editRqmtRecIdx].comment = row["Cmt"].ToString();
                                    tradeRqmtDto.Cmt = row["Cmt"].ToString();

                                tradeRqmtDataList.Add(tradeRqmtDto);

                                //Increment rqmtRequestArray index
                                editRqmtRecIdx++;
                                //There may be more than one requirement found so only process the first one.
                                //This situation is sorted out by the update process.
                                break;
                            }
                        }
                }

                TradeRqmtDal tradeRqmtDal = new TradeRqmtDal(sqlConnectionStr);

                //For single row update synchronously and update grids.
                //if (isSingleRow && rqmtRequestArray[0] != null)
                if (isSingleRow && tradeRqmtDataList.Count > 0)
                {
                    int rowsUpdated = 0;
                    rowsUpdated = tradeRqmtDal.UpdateTradeRqmts(tradeRqmtDataList);
                    if (rowsUpdated > 0)
                        UpdateLocalRqmtData(tradeRqmtDataList);
                }

                //Israel 9/28/2015
                //if (prelimApprvOverride > 0 || finalApprovalIgnored > 0)
                if (finalApprovalIgnored > 0)
                    XtraMessageBox.Show("Final Approved trades ignored: " + finalApprovalIgnored.ToString() + Environment.NewLine,
                         "Edit Requirements", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to edit a requirement." + Environment.NewLine +
                    "Error CNF-065 in " + FORM_NAME + ".CallUpdateEditRqmt(): " + ex.Message);
            }
        }

        private void UpdateLocalRqmtData(List<TradeRqmtDto> pTradeRqmtDataList)
        {
            try
            {
                //int rowHandle = view.FocusedRowHandle;
                string colorStr = "";
                foreach (TradeRqmtDto tradeRqmtData in pTradeRqmtDataList)
                {
                    //Process only those requirements that have been edited. 
                    string filterStr = "Id = " + tradeRqmtData.Id.ToString();
                    foreach (DataRow rqmtRow in rqmtDataTable.Select(filterStr))
                    {
                        rqmtRow["CompletedDt"] = tradeRqmtData.CompletedDt;
                        rqmtRow["Status"] = tradeRqmtData.StatusCode;
                        rqmtRow["Reference"] = tradeRqmtData.Reference;
                        rqmtRow["SecondCheckFlag"] = tradeRqmtData.SecondCheckFlag;
                        rqmtRow["Cmt"] = tradeRqmtData.Cmt;
                        colorStr = GetHashkeyTrackingClientColor(tradeRqmtData.RqmtCode +
                                                                 tradeRqmtData.StatusCode);
                        rqmtRow["GuiColorCode"] = colorStr;
                        rqmtRow.AcceptChanges();
                        rqmtRow.EndEdit();

                        string filterStr2 = "TradeId = " + tradeRqmtData.TradeId.ToString();
                        foreach (DataRow summaryRow in summaryDataTable.Select(filterStr2))
                        {
                            Int32 rqmtId = tradeRqmtData.Id;
                            string rqmtStatus = tradeRqmtData.StatusCode;
                            switch (tradeRqmtData.RqmtCode)
                            {
                                case "XQCSP":
                                    if (Convert.ToInt64(summaryRow["SetcDbUpd"].ToString()) == rqmtId)
                                        UpdateLocalSummaryData(summaryRow, "SetcStatus", rqmtStatus);
                                    break;
                                case "XQCCP":
                                    if (Convert.ToInt64(summaryRow["CptyDbUpd"].ToString()) == rqmtId)
                                        UpdateLocalSummaryData(summaryRow, "CptyStatus", rqmtStatus);
                                    break;
                                case "XQBBP":
                                    if (Convert.ToInt64(summaryRow["BkrDbUpd"].ToString()) == rqmtId)
                                        UpdateLocalSummaryData(summaryRow, "BkrStatus", rqmtStatus);
                                    break;
                                case "ECBKR":
                                    if (Convert.ToInt64(summaryRow["BkrDbUpd"].ToString()) == rqmtId)
                                        UpdateLocalSummaryData(summaryRow, "BkrStatus", rqmtStatus);
                                    break;
                                case "EFBKR":
                                    if (Convert.ToInt64(summaryRow["BkrDbUpd"].ToString()) == rqmtId)
                                        UpdateLocalSummaryData(summaryRow, "BkrStatus", rqmtStatus);
                                    break;
                                case "NOCNF":
                                    if (Convert.ToInt64(summaryRow["NoconfDbUpd"].ToString()) == rqmtId)
                                        UpdateLocalSummaryData(summaryRow, "NoconfStatus", rqmtStatus);
                                    break;
                                case "VBCP":
                                    if (Convert.ToInt64(summaryRow["VerblDbUpd"].ToString()) == rqmtId)
                                        UpdateLocalSummaryData(summaryRow, "VerblStatus", rqmtStatus);
                                    break;
                                case "ECONF":
                                    if (Convert.ToInt64(summaryRow["SetcDbUpd"].ToString()) == rqmtId)
                                        UpdateLocalSummaryData(summaryRow, "SetcStatus", rqmtStatus);
                                    break;
                                case "EFET":
                                    if (Convert.ToInt64(summaryRow["SetcDbUpd"].ToString()) == rqmtId)
                                        UpdateLocalSummaryData(summaryRow, "SetcStatus", rqmtStatus);
                                    break;
                                case "MISC":
                                    if (Convert.ToInt64(summaryRow["VerblDbUpd"].ToString()) == rqmtId)
                                        UpdateLocalSummaryData(summaryRow, "VerblStatus", rqmtStatus);
                                    break;

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to update the local requirement grid." + Environment.NewLine +
                    "Error CNF-066 in " + FORM_NAME + ".UpdateLocalRqmtData(): " + ex.Message);
            }
        }

        private void UpdateLocalSummaryData(DataRow row, string AFieldName, string ADataValue)
        {
            try
            {
                //DataRow row = gridViewSummary.GetDataRow(ARowHandle);
                row.BeginEdit();
                row[AFieldName] = ADataValue;
                row.AcceptChanges();
                row.EndEdit();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to update the: " + AFieldName + " field" + Environment.NewLine +
                    "of the local Summary Grid with a data value of: " + ADataValue + "." + Environment.NewLine +
                    "Error CNF-067 in " + FORM_NAME + ".UpdateLocalSummaryData(): " + ex.Message);
            }
        }

        private void CallSetRqmtData(int ARqmtType, GridView view, int rowHandle, string filterStr)
        {
            try
            {
                string rqmtStatus = "";
                DateTime rqmtStatusDate = DateTime.Today;
                bool secondChk = false;
                string rqmtRef = "";
                string rqmtCmt = "";
                //string prelimApprover = "";

                //rqmtStatus = view.GetRowCellDisplayText(rowHandle, RqmtStatusColNames[ARqmtType]).ToString();

                foreach (DataRow row in rqmtDataTable.Select(filterStr))
                {
                    if (row["Rqmt"].ToString() == editRqmtForm.RQMT_CODES[ARqmtType])
                    {
                        rqmtStatus = row["Status"].ToString();
                        rqmtStatusDate = (DateTime)row["CompletedDt"];
                        secondChk = row["SecondCheckFlag"].ToString() == "Y";
                        rqmtRef = row["Reference"].ToString();
                        rqmtCmt = row["Cmt"].ToString();
                        //prelimApprover = row["PrelimAppr"].ToString();
                        break;
                    }
                }
                editRqmtForm.SetRqmtData(ARqmtType, rqmtStatus, rqmtStatusDate,
                                         secondChk, rqmtRef, rqmtCmt);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to update the local requirement grid." + Environment.NewLine +
                    "Rqmt Type Ind: " + ARqmtType.ToString() + " , Row Handle: " + rowHandle.ToString() +
                    " , Filter string" + filterStr + "." + Environment.NewLine +
                    "Error CNF-068 in " + FORM_NAME + ".CallSetRqmtData(): " + ex.Message);
            }
        }

        private string GetRqmtData(string ARqmtCode, string AResultField)
        {
            string displayText = "";
            try
            {
                DataRow row = rqmtView.Rows.Find(ARqmtCode);
                displayText = row[AResultField].ToString();
                return displayText;
            }
            catch (Exception error)
            {
                throw new Exception("An error occurred while attempting to read requirement data from the grid." + Environment.NewLine +
                    "Requirement Code: " + ARqmtCode + ", Field: " + AResultField + "." + Environment.NewLine +
                    "Error CNF-069 in " + FORM_NAME + ".GetRqmtData(): " + error.Message);
            }
        }
        #region VaultVieweer
        private void bbVaultViewer_ItemClick(object sender, ItemClickEventArgs e)
        {
            string trdSysCode = string.Empty; string tokenNo = string.Empty;
            trdSysCode = gridViewSummary.GetRowCellDisplayText(gridViewSummary.FocusedRowHandle, "TrdSysCode").ToString();
            tokenNo = gridViewSummary.GetRowCellDisplayText(gridViewSummary.FocusedRowHandle, "TradeSysTicket").ToString();
            openVaultViewer(trdSysCode, tokenNo);
        }

        private void openVaultViewer(string tradSysCode, string tokenNum)
        {
            string path = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + @"..\VaultViewer\VaultViewer.exe");
            Process process = new Process();
            process.StartInfo.FileName = path;
            process.StartInfo.Arguments = tradSysCode + " " + tokenNum;

            process.Start();
            processId = process.Id;
        }
        private void CloseVaultViewer()
        {
            foreach (var process in Process.GetProcessesByName("VaultViewer"))
            {
                process.Kill();
            }
        }

        #endregion

        private void bbAddRequirement_ItemClick(object sender, ItemClickEventArgs e)
        {
            string defaultFilter = "";
            try
            {
                string rqmtCode = "";
                string refCode = "";
                string cmt = "";
                myTimer.Stop(); //Israel 12/15/2008 - Red X

                addRqmtForm.settingsDir = this.appSettingsDir;
                addRqmtForm.lookupRqmt.EditValue = "";
                addRqmtForm.comboNoConfReason.EditValue = "";
                addRqmtForm.memoEditRqmtCmt.EditValue = "";
                addRqmtForm.lblNoConfReason.Visible = false;
                addRqmtForm.comboNoConfReason.Visible = false;
                defaultFilter = "[ActiveFlag] = 'Y' and [DetActRqmtFlag] = 'Y'";
                GridView view = gridViewSummary;
                if (view.SelectedRowsCount == 1)
                {
                    string setcCode = view.GetRowCellDisplayText(view.FocusedRowHandle, "SetcRqmt").ToString();
                    if (setcCode == SEMPRA_RQMT)
                    {
                        rqmtView.DefaultView.RowFilter += " and [Code] <> 'XQCSP'";
                    }
                    //Israel 11/18/14 -- Added to prevent rows being added that broke v_pc_trade_summary, which prevented app from starting.
                    string cptyCode = view.GetRowCellDisplayText(view.FocusedRowHandle, "CptyRqmt").ToString();
                    if (cptyCode == CPTY_RQMT)
                    {
                        rqmtView.DefaultView.RowFilter += " and [Code] <> 'XQCCP'";
                    }
                    string bkrCode = view.GetRowCellDisplayText(view.FocusedRowHandle, "BkrRqmt").ToString();
                    if (bkrCode == BKR_RQMT)
                    {
                        rqmtView.DefaultView.RowFilter += " and [Code] <> 'XQBBP'";
                    }
                    string noconfCode = view.GetRowCellDisplayText(view.FocusedRowHandle, "NoconfRqmt").ToString();
                    if (noconfCode == NOCONF_RQMT)
                    {
                        rqmtView.DefaultView.RowFilter += " and [Code] <> 'NOCNF'";
                    }
                    //Israel 11/20/2015 -- Removed pending renaming as Blotter
                    //string verblCode = view.GetRowCellDisplayText(view.FocusedRowHandle, "VerblRqmt").ToString();
                    //if (verblCode == VERBL_RQMT)
                    //{
                    rqmtView.DefaultView.RowFilter += " and [Code] <> 'VBCP'";
                    //}
                }
                addRqmtForm.Visible = false;
                if (addRqmtForm.ShowDialog(this) == DialogResult.OK)
                {
                    rqmtCode = addRqmtForm.lookupRqmt.EditValue.ToString();
                    if (rqmtCode == "NOCNF")
                        refCode = addRqmtForm.comboNoConfReason.EditValue.ToString();
                    cmt = addRqmtForm.memoEditRqmtCmt.EditValue.ToString();
                    CallAddRqmt(rqmtCode, refCode, cmt);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to add a new requirement." + Environment.NewLine +
                       "Error CNF-070 in " + FORM_NAME + ".bbAddRequirement_ItemClick(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                rqmtView.DefaultView.RowFilter = defaultFilter;
                myTimer.Start(); //Israel 12/15/2008 - Red X
            }
        }

        private void CallAddRqmt(string ARqmtCode, string ANoConfReason, string ACmt)
        {
            try
            {
                string finalApproval = "N";
                int finalApprovalIgnored = 0;
                int addRqmtRow = 0;
                GridView view = gridViewSummary;
                if (view.SelectedRowsCount > 0)
                {
                    //Get the list ahead of time, since the foreach code changes the list's contents/order
                    int[] selectedRows = view.GetSelectedRows();
                    List<DataRow> rows = new List<DataRow>();

                    foreach (int rowHandle in selectedRows)
                        rows.Add(view.GetDataRow(rowHandle));

                        foreach (DataRow rowSummary in rows)
                        {
                            finalApproval = rowSummary["FinalApprovalFlag"].ToString();
                            if (finalApproval == "Y")
                            {
                                finalApprovalIgnored++;
                                continue;
                            }

                            TradeRqmtDal tradeRqmtDal = new TradeRqmtDal(sqlConnectionStr);
                            Int32 tradeId = Int32.Parse(rowSummary["TradeId"].ToString());
                            string reason = String.Empty;
                            if (ARqmtCode == "NOCNF")
                                reason = ANoConfReason;
                           Int32 rqmtId= tradeRqmtDal.AddTradeRqmt(tradeId, ARqmtCode, reason, ACmt);
                            //add traderqmtconfirm
                           if (ARqmtCode =="XQCSP")
                            UpdateTradeRqmtConfirmRow(0, tradeId,
                                        rqmtId, null, null, null, CONFIRM_LABEL_CONFIRM, null, null, "Y");
                        }

                    if (finalApprovalIgnored > 0)
                        XtraMessageBox.Show("Final Approved trades ignored: " + finalApprovalIgnored.ToString(), // + Environment.NewLine
                            //+ "Determine Action trades processed: " + determineActionRow.ToString() + Environment.NewLine,
                             "Add Rqmt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to add one or more new requirements." + Environment.NewLine +
                    "Rqmt Code: " + ARqmtCode + ", No Confirm Reason: " + ANoConfReason + ", Comment: " + ACmt + Environment.NewLine +
                     "Error CNF-071 in " + FORM_NAME + ".CallAddRqmt(): " + ex.Message);
            }
        }

        private void bbSubmitQueuedEfetTrades_ItemClick(object sender, ItemClickEventArgs e)
        {
            //try
            //{
            //    myTimer.Stop(); //Israel 12/15/2008 - Red X

            //    SubmitTradesToEcmBox();
            //}
            //catch (Exception ex)
            //{
            //    XtraMessageBox.Show("bbSubmitQueuedEfetTrades_ItemClick: " + ex.Message,
            //       "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //finally
            //{
            //    myTimer.Start(); //Israel 12/15/2008 - Red X
            //}
        }

        private void SubmitTradesToEcmBox()
        {
            try
            {
                //submitToECMBox ecmBoxSubmit = new submitToECMBox();
                //ecmBoxSubmit.request = new ecmBoxSubmitRequest();
                //ecmBoxSubmit.request.processMasterCode = "EFETSUB";

                //confirmUtilService.submitToECMBoxCompleted -=
                //   new submitToECMBoxCompletedEventHandler(SubmitToEcmBoxReturn);
                //confirmUtilService.submitToECMBoxCompleted +=
                //   new submitToECMBoxCompletedEventHandler(SubmitToEcmBoxReturn);
                //confirmUtilService.submitToECMBoxAsync(ecmBoxSubmit, new object());
            }
            catch (Exception ex)
            {
                //throw new Exception("SubmitTradesToEcmBox: " + ex.Message);
                //XtraMessageBox.Show("SubmitTradesToEcmBox: " + ex.Message,
                //   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bbDetermineActions_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myTimer.Stop(); //Israel 12/15/2008 - Red X

                //1/12/2010 Israel - Edit Sent trades issue
                DetermineActionsType detActionsType;
                if (e.Item.Name == "bbDetermineActionsNo")
                    detActionsType = DetermineActionsType.ResetToNo;
                else if (e.Item.Name == "bbDetermineActionsReprocess")
                    detActionsType = DetermineActionsType.ResetToReprocess;
                else throw new Exception("Internal Error: Unexpected sender = " + e.Item.Name);

                GridView view = gridViewSummary;
                int rowCount = view.SelectedRowsCount;
                string msgText = "Set Determine Actions to No";
                if (detActionsType == DetermineActionsType.ResetToReprocess)
                    msgText = "Set Determine Actions to Reprocess";

                string msgTextPlural = "";
                if (rowCount > 1)
                    msgTextPlural += "s?";
                else
                    msgTextPlural += "?";

                DialogResult result = XtraMessageBox.Show(msgText + " for " + rowCount.ToString() + " row" + msgTextPlural,
                    msgText, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                    CallUpdateDetermineActions(detActionsType);
                else
                    XtraMessageBox.Show(msgText + " was cancelled.", msgText);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to update the Determine Actions setting." + Environment.NewLine +
                       "Error CNF-072 in " + FORM_NAME + ".bbDetermineActions_ItemClick(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start(); //Israel 12/15/2008 - Red X
            }
        }

        private void CallUpdateDetermineActions(DetermineActionsType ADetActionsType)
        {
            try
            {
                string finalApproval = "N";

                string setDetActionsFlag;
                if (ADetActionsType == DetermineActionsType.ResetToNo)
                    setDetActionsFlag = "N";
                else
                    setDetActionsFlag = "R";

                int finalApprovedIgnored = 0;
                string determineActionFlag = "";
                int determineActionRow = 0;
                int notDetermineAction = 0;
                GridView view = gridViewSummary;
                if (view.SelectedRowsCount > 0)
                {
                    //Get the list ahead of time, since the foreach code changes the list's contents/order
                    int[] selectedRows = view.GetSelectedRows();
                    List<DataRow> rows = new List<DataRow>();

                    List<TradeSummaryDto> tradeSummaryList = new List<TradeSummaryDto>();
                    TradeSummaryDto tradeSummaryDto = null;

                    foreach (int rowHandle in selectedRows)
                        rows.Add(view.GetDataRow(rowHandle));

                    foreach (DataRow rowSummary in rows)
                    {
                        finalApproval = rowSummary["FinalApprovalFlag"].ToString();
                        if (finalApproval == "Y")
                        {
                            finalApprovedIgnored++;
                            continue;
                        }

                        determineActionFlag = rowSummary["OpsDetActFlag"].ToString();
                        //boolean allows construction of positive logic
                        bool detActionsOk = false;
                        detActionsOk = (ADetActionsType == DetermineActionsType.ResetToNo &&
                                        (determineActionFlag == "Y" || determineActionFlag == "E" || determineActionFlag == "R")) ||
                                       (ADetActionsType == DetermineActionsType.ResetToReprocess & determineActionFlag == "E");

                        if (!detActionsOk)
                        {
                            notDetermineAction++;
                            continue;
                        }
                        else
                        {
                            tradeSummaryDto = new TradeSummaryDto();
                            tradeSummaryDto.TradeId = Int32.Parse(rowSummary["TradeId"].ToString());
                            tradeSummaryDto.OpsDetActFlag = setDetActionsFlag;
                            tradeSummaryList.Add(tradeSummaryDto);
                            determineActionRow++;
                            //SetRowUpdateSubmitted(rowSummary);
                        }
                    }

                    if (determineActionRow > 0)
                    {
                        TradeSummaryDal tradeSummaryDal = new TradeSummaryDal(sqlConnectionStr);
                        tradeSummaryDal.UpdateDetermineActions(tradeSummaryList);
                    }

                    if (notDetermineAction > 0 || finalApprovedIgnored > 0)
                    {
                        if (ADetActionsType == DetermineActionsType.ResetToNo)
                            XtraMessageBox.Show("Determine Action N rows ignored: " + notDetermineAction.ToString() + Environment.NewLine
                               + "Final Approved ignored: " + finalApprovedIgnored.ToString() + Environment.NewLine
                               + "----------------------------" + Environment.NewLine
                               + "Set Determine Action to No trades processed: " + determineActionRow.ToString() + Environment.NewLine,
                                 "Set Determine Actions to No", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            XtraMessageBox.Show("Determine Action Y, N or R rows ignored: " + notDetermineAction.ToString() + Environment.NewLine
                               + "Final Approved ignored: " + finalApprovedIgnored.ToString() + Environment.NewLine
                               + "----------------------------" + Environment.NewLine
                               + "Set Determine Action to Reprocess trades processed: " + determineActionRow.ToString() + Environment.NewLine,
                                 "Set Determine Actions to Reprocess", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to update Determine Actions setting(s)." + Environment.NewLine +
                     "Error CNF-073 in " + FORM_NAME + ".CallUpdateDetermineActions(): " + ex.Message);
            }
        }

        private void bbEConfirm_ItemClick(object sender, ItemClickEventArgs e)
        {
            //string msgboxText = "";
            //string submitCode = "";
            //try
            //{
            //    GridView view = gridViewSummary;
            //    int rowCount = view.SelectedRowsCount;
            //    myTimer.Stop(); //Israel 12/15/2008 - Red X


            //    switch (e.Item.Name)
            //    {
            //        case "bbEConfirmResubmit":
            //            if (rowCount == 1)
            //                msgboxText = "Resubmit 1 trade to eConfirm";
            //            else
            //                msgboxText = "Resubmit " + rowCount.ToString() + " trades to eConfirm";
            //            submitCode = "R";
            //            break;
            //        case "bbEConfirmCancel":
            //            if (rowCount == 1)
            //                msgboxText = "Cancel 1 eConfirm trade";
            //            else
            //                msgboxText = "Cancel " + rowCount.ToString() + " eConfirm trades";
            //            submitCode = "C";
            //            break;
            //        default:
            //            throw new Exception("Internal Error: " + e.Item.Name + " not found");
            //    }

            //    DialogResult result = XtraMessageBox.Show(msgboxText + "?", msgboxText,
            //       MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            //    if (result == DialogResult.Yes)
            //        SubmitToEConfirm(submitCode);
            //    else
            //        XtraMessageBox.Show(msgboxText + " was cancelled.");
            //}
            //catch (Exception ex)
            //{
            //    XtraMessageBox.Show("bbEConfirm_ItemClick: " + ex.Message,
            //       "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //finally
            //{
            //    myTimer.Start(); //Israel 12/15/2008 - Red X
            //}
        }

        private void SubmitToEConfirm(string ASubmitCode)
        {
            //try
            //{
            //    string NONE = "**NONE**";
            //    string rqmt = "";
            //    string rqmtStatus = "";
            //    string submitLabel = "";
            //    string finalApprovalFlag = "";
            //    int econfirmRow = 0;
            //    int nonEconfirmRqmt = 0;
            //    int matchCancel = 0;
            //    int finalApproval = 0;
            //    GridView view = gridViewSummary;
            //    if (view.SelectedRowsCount > 0)
            //    {
            //        if (ASubmitCode == "R")
            //            submitLabel = "Resubmit";
            //        else if (ASubmitCode == "C")
            //            submitLabel = "Cancel";
            //        else
            //            throw new Exception("SubmitToEConfirm: Internal Error: Unrecognized ASubmitCode=" + ASubmitCode);

            //        eConfirmRequest[] econfirmRequestArray = new eConfirmRequest[view.SelectedRowsCount];

            //        //Get the list ahead of time, since the foreach code changes the list's contents/order
            //        int[] selectedRows = view.GetSelectedRows();
            //        List<DataRow> rows = new List<DataRow>();

            //        foreach (int rowHandle in selectedRows)
            //            rows.Add(view.GetDataRow(rowHandle));

            //        foreach (DataRow rowSummary in rows)
            //        {
            //            finalApprovalFlag = rowSummary["FinalApprovalFlag"].ToString();

            //            if (finalApprovalFlag == "Y")
            //            {
            //                finalApproval++;
            //                continue;
            //            }

            //            //Only read the rqmt table for ECONF rqmts.
            //            string tradeId = rowSummary["TradeId"].ToString();
            //            string filterStr = "TradeId = " + tradeId + " and (Rqmt = 'ECONF' or Rqmt = 'ECBKR')";
            //            rqmt = NONE;
            //            foreach (DataRow row in rqmtDataTable.Select(filterStr))
            //            {
            //                rqmt = row["Rqmt"].ToString();
            //                rqmtStatus = row["Status"].ToString();
            //                if ((rqmtStatus == "CXL" && ASubmitCode == "C") || (rqmtStatus == "MATCH"))
            //                {
            //                    matchCancel++;
            //                    continue;
            //                }
            //                else

            //                {
            //                    econfirmRequestArray[econfirmRow] = new eConfirmRequest();
            //                    econfirmRequestArray[econfirmRow].tradeId = int.Parse(tradeId);
            //                    econfirmRequestArray[econfirmRow].status = ASubmitCode;
            //                    econfirmRow++;
            //                    matchCancel = 0;  // samy: reset to 0 if one of them pending
            //                    SetRowUpdateSubmitted(row);
            //                }
            //            }
            //            if (rqmt == NONE)
            //            {
            //                nonEconfirmRqmt++;
            //                continue;
            //            }
            //        }
            //        //confirmUtilService.batchSubmitToEconfirmCompleted -= new batchSubmitToEconfirmCompletedEventHandler(SubmitToEConfirmReturn);
            //        //confirmUtilService.batchSubmitToEconfirmCompleted +=
            //        //   new batchSubmitToEconfirmCompletedEventHandler(SubmitToEConfirmReturn);
            //        //confirmUtilService.batchSubmitToEconfirmAsync(econfirmRequestArray, new object());

            //        if (nonEconfirmRqmt > 0 || matchCancel > 0 || finalApproval > 0)
            //            XtraMessageBox.Show("Final Approved rows ignored: " + finalApproval.ToString() + Environment.NewLine
            //               + "Non-eConfirm/eConfirm Broker rows ignored: " + nonEconfirmRqmt.ToString() + Environment.NewLine
            //               + "eConfirm/eConfirm Broker MATCH/CXL ignored: " + matchCancel.ToString() + Environment.NewLine
            //               + "------------------------------------" + Environment.NewLine
            //               + "eConfirm/eConfirm Broker " + submitLabel + " submitted: " + econfirmRow.ToString() + Environment.NewLine,
            //               "eConfirm/eConfirm Broker " + submitLabel, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("SubmitToEConfirm: " + ex.Message);
            //    //XtraMessageBox.Show("SubmitToEConfirm: " + ex.Message,
            //    //   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void bbEfet_ItemClick(object sender, ItemClickEventArgs e)
        {
            //string msgboxText = "";
            //string submitCode = "";
            //entityType varEntityType;
            //try
            //{
            //    GridView view = gridViewSummary;
            //    int rowCount = view.SelectedRowsCount;

            //    myTimer.Stop(); //Israel 12/15/2008 - Red X

            //    switch (e.Item.Name)
            //    {
            //        case "bbEfetResubmitCpty":
            //            if (rowCount == 1)
            //                msgboxText = "Resubmit 1 Cpty trade to eCM";
            //            else
            //                msgboxText = "Resubmit " + rowCount.ToString() + " Cpty trades to eCM";
            //            varEntityType = entityType.Cpty;
            //            submitCode = "R";
            //            break;
            //        case "bbEfetResubmitBroker":
            //            if (rowCount == 1)
            //                msgboxText = "Resubmit 1 Broker trade to eCM";
            //            else
            //                msgboxText = "Resubmit " + rowCount.ToString() + " Broker trades to eCM";
            //            varEntityType = entityType.Broker;
            //            submitCode = "R";
            //            break;
            //        case "bbEfetResubmitBoth":
            //            if (rowCount == 1)
            //                msgboxText = "Resubmit 1 Cpty and Broker trade to eCM";
            //            else
            //                msgboxText = "Resubmit " + rowCount.ToString() + " Cpty and Broker trades to eCM";
            //            varEntityType = entityType.Both;
            //            submitCode = "R";
            //            break;
            //        case "bbEfetCancelCpty":
            //            if (rowCount == 1)
            //                msgboxText = "Cancel 1 eCM Cpty trade";
            //            else
            //                msgboxText = "Cancel " + rowCount.ToString() + " eCM Cpty trades";
            //            varEntityType = entityType.Cpty;
            //            submitCode = "C";
            //            break;
            //        case "bbEfetCancelBroker":
            //            if (rowCount == 1)
            //                msgboxText = "Cancel 1 eCM Broker trade";
            //            else
            //                msgboxText = "Cancel " + rowCount.ToString() + " eCM Broker trades";
            //            varEntityType = entityType.Broker;
            //            submitCode = "C";
            //            break;
            //        default:
            //            throw new Exception("Internal Error: " + e.Item.Name + " not found");
            //    }

            //    DialogResult result = XtraMessageBox.Show(msgboxText + "?", msgboxText,
            //       MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            //    if (result == DialogResult.Yes)
            //        SubmitToEfet(varEntityType, submitCode);
            //    else
            //        XtraMessageBox.Show(msgboxText + " was cancelled.");
            //}
            //catch (Exception ex)
            //{
            //    XtraMessageBox.Show("bbEfet_ItemClick: " + ex.Message,
            //       "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //finally
            //{
            //    myTimer.Start(); //Israel 12/15/2008 - Red X
            //}
        }

        private void bbFinalApproval_ItemClick(object sender, ItemClickEventArgs e)
        {
            string msgboxText = "";
            string approvalFlag = "";
            string onlyIfReadyFlag = "";
            try
            {
                GridView view = gridViewSummary;
                int rowCount = view.SelectedRowsCount;

                myTimer.Stop(); //Israel 12/15/2008 - Red X

                switch (e.Item.Name)
                {
                    case "bbFinalApproval":
                        if (rowCount == 1)
                            msgboxText = "Final Approve 1 Trade";
                        else
                            msgboxText = "Final Approve " + rowCount.ToString() + " Trades";
                        approvalFlag = "Y";
                        onlyIfReadyFlag = "Y";
                        break;
                    case "bbForceFinalApproval":
                        if (rowCount == 1)
                            msgboxText = "Force Final Approve 1 Trade";
                        else
                            msgboxText = "Force Final Approve " + rowCount.ToString() + " Trades";
                        approvalFlag = "Y";
                        onlyIfReadyFlag = "N";
                        break;
                    case "bbReopenFinalApproval":
                        if (rowCount == 1)
                            msgboxText = "Reopen 1 Final Approved Trade";
                        else
                            msgboxText = "Reopen " + rowCount.ToString() + " Final Approved Trades";
                        approvalFlag = "N";
                        onlyIfReadyFlag = "N";
                        break;
                    default:
                        throw new Exception("Internal Error: " + e.Item.Name + " not found");
                }

                DialogResult result = XtraMessageBox.Show(msgboxText + "?", msgboxText,
                   MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                    CallUpdateFinalApproval(approvalFlag, onlyIfReadyFlag);
                else
                    XtraMessageBox.Show(msgboxText + " was cancelled.");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to update the Final Approval setting." + Environment.NewLine +
                       "Error CNF-074 in " + FORM_NAME + ".bbFinalApproval_ItemClick(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start(); //Israel 12/15/2008 - Red X
            }
        }

        private void CallUpdateFinalApproval(string AApprovalFlag, string AOnlyIfReadyFlag)
        {
            try
            {
                string readyForFinalApprovalFlag = "N";
                string finalApprovalFlag = "N";
                string tradeId = "";
                string notSubmittedLabel = "";
                string submittedLabel = "";
                int updateRowCounter = 0;
                int notSubmitted = 0;

                GridView view = gridViewSummary;
                if (view.SelectedRowsCount > 0)
                {
                    if (AApprovalFlag == "Y" && AOnlyIfReadyFlag == "Y")
                    {
                        notSubmittedLabel = "Not ready or already approved ignored: ";
                        submittedLabel = "Final Approval";
                    }
                    else if (AApprovalFlag == "Y" && AOnlyIfReadyFlag == "N")
                    {
                        notSubmittedLabel = "Already approved ignored: ";
                        submittedLabel = "Force Final Approval";
                    }
                    else if (AApprovalFlag == "N" && AOnlyIfReadyFlag == "N")
                    {
                        notSubmittedLabel = "Already open ignored: ";
                        submittedLabel = "Reopen Final Approval ";
                    }

                    List<TradeSummaryDto> tradeSummaryList = new List<TradeSummaryDto>();
                    TradeSummaryDto tradeSummaryDto = null;

                    //Get the list ahead of time, since the foreach code changes the list's contents/order
                    int[] selectedRows = view.GetSelectedRows();
                    List<DataRow> rows = new List<DataRow>();

                    foreach (int rowHandle in selectedRows)
                        rows.Add(view.GetDataRow(rowHandle));

                    //Israel 10/27/15 -- Invoke SetFinalApprovalFlag
                    TradeApprDto tradeApprData = new TradeApprDto();
                    List<TradeApprDto> tradeApprList = new List<TradeApprDto>();

                    foreach (DataRow rowSummary in rows)
                    {
                        readyForFinalApprovalFlag = rowSummary["ReadyForFinalApprovalFlag"].ToString();
                        finalApprovalFlag = rowSummary["FinalApprovalFlag"].ToString();
                        //finalApprovalFlag = "N";
                        tradeId = rowSummary["TradeId"].ToString();

                        tradeSummaryDto = new TradeSummaryDto();
                        tradeSummaryDto.FinalApprovalFlag = AApprovalFlag;
                        tradeSummaryDto.TradeId = Int32.Parse(tradeId);

                        switch (AApprovalFlag)
                        {
                            case "Y": //Final Approval
                                if (AOnlyIfReadyFlag == "Y") //Regular Final Approval
                                {
                                    //6/9/09 Israel - Don't allow Our Paper status change if still in contract edit mode.
                                    //Set a filter for the edited rqmt
                                    string filterStr = "TradeId = " + tradeId +
                                       " and Rqmt = '" + SEMPRA_RQMT + "'";
                                    bool okToApprove = true;
                                    //Read the RqmtData row(s)
                                    foreach (DataRow row in rqmtDataTable.Select(filterStr))
                                    {
                                        string status = row["Status"].ToString();
                                        if (noEditSempraRqmtStatus.IndexOf(status) > -1)
                                            okToApprove = false;
                                    }
                                    if (readyForFinalApprovalFlag == "Y" &&
                                        finalApprovalFlag == "N" &&
                                        okToApprove)
                                    {
                                        updateRowCounter++;
                                    }
                                    else
                                        notSubmitted++;
                                }
                                else if (AOnlyIfReadyFlag == "N") //Force Final Approval
                                {
                                    if (finalApprovalFlag == "N")
                                    {
                                        updateRowCounter++;
                                    }
                                    else

                                        notSubmitted++;
                                }
                                break;
                            case "N": //Reopen Final Approval
                                if (finalApprovalFlag == "Y")
                                {
                                    updateRowCounter++;
                                }
                                else
                                    notSubmitted++;
                                break;
                            default:
                                throw new Exception("Internal Error: Approval Flag=" + AApprovalFlag + " not found");
                        }
                        if (view.SelectedRowsCount > 0)
                        {
                            tradeSummaryList.Add(tradeSummaryDto);
                            //SetRowUpdateSubmitted(rowSummary);
                        }

                        tradeApprData = new TradeApprDto();
                        tradeApprData.TradeId = Int32.Parse(tradeId);
                        tradeApprData.OnlyIfReadyFlag = AOnlyIfReadyFlag;
                        tradeApprData.ApprovalFlag = AApprovalFlag;
                        tradeApprData.ApprByUserName = p_UserId;
                        tradeApprList.Add(tradeApprData);
                    }

                    if (updateRowCounter > 0 && view.SelectedRowsCount > 0)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        using (var ts = new TransactionScope())
                        {
                            TradeSummaryDal tradeSummaryDal = new TradeSummaryDal(sqlConnectionStr);
                            int rowsUpdated = 0;
                            rowsUpdated = tradeSummaryDal.UpdateFinalApproval(tradeSummaryList);

                            TradeApprDal tradeApprDal = new TradeApprDal(sqlConnectionStr);
                            int rowsUpdated2 = 0;
                            rowsUpdated2 = tradeApprDal.SetFinalApprovalFlag(tradeApprList);

#if NAVEEN_VAULT
                            UploadDocumentToVault(tradeSummaryList);
#endif
                            ts.Complete();
                        }

                    }
                    if (notSubmitted > 0)
                        XtraMessageBox.Show(notSubmittedLabel + notSubmitted.ToString() + Environment.NewLine
                           + "-----------------------------------------------" + Environment.NewLine
                           + submittedLabel + " submitted: " + updateRowCounter.ToString() + Environment.NewLine,
                           submittedLabel, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to update one or more Final Approval settings." + Environment.NewLine +
                       "Error CNF-075 in " + FORM_NAME + ".CallUpdateFinalApproval(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private static void UploadDocumentToVault(List<TradeSummaryDto> tradeSummaryList)
        {
            string extSvcBaseUrl = Properties.Settings.Default.ExtSvcAPIBaseUrl;
            string extSvcUserName = Properties.Settings.Default.ExtSvcAPIUserName;
            string extSvcPassword = Properties.Settings.Default.ExtSvcAPIPassword;
            //call this for each trade  
            //http://hou-121.mercuria.met:9001
            //   VaultSvcAccessor vaultSvcAccessor = VaultSvcAccessor.Instance("http://hou-121.mercuria.met:9001");
            VaultSvcAccessor vaultSvcAccessor = VaultSvcAccessor.Instance("http://hou-121.mercuria.met:9001");
            vaultSvcAccessor.UploadDocument("test", "PDF", "123", "OIL", null);
        }

        private void bbGroupTrades_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myTimer.Stop(); //Israel 12/15/2008 - Red X

                if (groupTradesForm.ShowDialog(this) == DialogResult.OK)
                {
                    CallGroupTrades(groupTradesForm.teGroupXRef.Text);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to update Trade Group settings." + Environment.NewLine +
                       "Error CNF-076 in " + FORM_NAME + ".bbGroupTrades_ItemClick(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start(); //Israel 12/15/2008 - Red X
            }
        }

        private void CallGroupTrades(string AGroupCode)
        {
            try
            {
                string tradeId = "";
                string groupXref = "";
                int updateRowCounter = 0;

                GridView view = gridViewSummary;

                if (view.SelectedRowsCount > 0)
                {
                    //Get the list ahead of time, since the foreach code changes the list's contents/order
                    int[] selectedRows = view.GetSelectedRows();
                    List<DataRow> rows = new List<DataRow>();

                    foreach (int rowHandle in selectedRows)
                        rows.Add(view.GetDataRow(rowHandle));

                    List<TradeGroupDto> tradeGroupList = new List<TradeGroupDto>();
                    TradeGroupDto tradeGroupDto = null;

                    foreach (DataRow rowSummary in rows)
                    {
                        tradeId = rowSummary["TradeId"].ToString();
                        groupXref = rowSummary["GroupXref"].ToString();

                        tradeGroupDto = new TradeGroupDto();
                        tradeGroupDto.TradeId = Int32.Parse(tradeId);
                        tradeGroupDto.Xref = AGroupCode;
                        tradeGroupList.Add(tradeGroupDto);

                        updateRowCounter++;
                    }
                    if (updateRowCounter > 0)
                    {
                        TradeGroupDal tradeGroupDal = new TradeGroupDal(sqlConnectionStr);
                        tradeGroupDal.Group(tradeGroupList);

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing the selected row(s) for trade group: " + AGroupCode + "." + Environment.NewLine +
                     "Error CNF-077 in " + FORM_NAME + ".CallGroupTrades(): " + ex.Message);
            }
        }

        private void bbUngroupTrades_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                GridView view = gridViewSummary;
                int rowCount = view.SelectedRowsCount;
                string msgText = "Ungroup " + rowCount.ToString() + " Trade";
                if (rowCount > 1)
                    msgText += "s";
                myTimer.Stop(); //Israel 12/15/2008 - Red X

                DialogResult result = XtraMessageBox.Show(msgText + "?",
                   "Ungroup Trades", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    CallUngroupTrades();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to update Trade Ungroup settings." + Environment.NewLine +
                       "Error CNF-078 in " + FORM_NAME + ".bbUngroupTrades_ItemClick(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start(); //Israel 12/15/2008 - Red X
            }
        }

        private void CallUngroupTrades()
        {
            try
            {
                Int32 tradeId = 0;
                int updateRowCounter = 0;

                GridView view = gridViewSummary;

                if (view.SelectedRowsCount > 0)
                {
                    //Get the list ahead of time, since the foreach code changes the list's contents/order
                    int[] selectedRows = view.GetSelectedRows();
                    List<DataRow> rows = new List<DataRow>();

                    foreach (int rowHandle in selectedRows)
                        rows.Add(view.GetDataRow(rowHandle));

                    List<Int32> tradeIdList = new List<Int32>();

                    foreach (DataRow rowSummary in rows)
                    {
                        tradeId = Convert.ToInt32(rowSummary["TradeId"]);
                        tradeIdList.Add(tradeId);

                        updateRowCounter++;
                    }
                    if (updateRowCounter > 0)
                    {
                        TradeGroupDal tradeGroupDal = new TradeGroupDal(sqlConnectionStr);
                        tradeGroupDal.Ungroup(tradeIdList);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing the selected row(s) for trade ungroup." + Environment.NewLine +
                     "Error CNF-079 in " + FORM_NAME + ".CallUngroupTrades(): " + ex.Message);
            }
        }

        private void bbGetAll_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myTimer.Stop(); //Israel 12/15/2008 - Red X

                getAllForm.settingsDir = this.appSettingsDir;
                getAllForm.InitForm(cdtyLkupTbl, cptyLkupTbl, seCptyLkupTbl);

                if (getAllForm.ShowDialog(this) == DialogResult.OK)
                {
                    string tradingSystem = getAllForm.TradingSystem;
                    //For now, just support a single Id
                    string ticketNo = getAllForm.TicketList;
                    string cptyTradeId = getAllForm.CptyTradeId;
                    string bookingCompanySn = getAllForm.BookingCoSn;
                    string cptySn = getAllForm.CptySn;
                    string cdtyCode = getAllForm.CdtyCode;
                    DateTime tradeStartDate = DateTime.MinValue;
                    DateTime tradeEndDate = DateTime.MinValue;

                    if (!(getAllForm.TradeStartDate == DateTime.MinValue))
                    {
                        tradeStartDate = getAllForm.TradeStartDate;
                    }
                    if (!(getAllForm.TradeEndDate == DateTime.MinValue))
                    {
                        tradeEndDate = getAllForm.TradeEndDate;
                    }

                    List<SummaryData> tradeList = new List<SummaryData>();
                    VPcTradeSummaryDal pcTradeSummaryDal = new VPcTradeSummaryDal(sqlConnectionStr);
                    string tradeIdList = "";

                    //If isSuperUser == true, then permKeyDBInClauseStr will be blank, which will cause the called routine
                    //to ignore the IN clause and simply select all rows that match the other criteria.
                    if (v_IsSuperUser || v_PermKeyDBInClauseStr != "")
                    {
                        tradeList = pcTradeSummaryDal.GetAllSelectedTrades(tradingSystem, bookingCompanySn, cptySn,
                                    cdtyCode, tradeStartDate, tradeEndDate, ticketNo, cptyTradeId, v_PermKeyDBInClauseStr);
                        tradeIdList = pcTradeSummaryDal.GetTradeIdListString(tradeList);
                    }

                    if (tradeList.Count > 0)
                    {

                        List<RqmtData> rqmtList = new List<RqmtData>();
                        VPcTradeRqmtDal pcTradeRqmtDal = new VPcTradeRqmtDal(sqlConnectionStr);
                        rqmtList = pcTradeRqmtDal.GetAll(tradeIdList);

                        List<TradeRqmtConfirm> rqmtConfirmList = new List<TradeRqmtConfirm>();
                        TradeRqmtConfirmDal tradeRqmtConfirmDal = new TradeRqmtConfirmDal(sqlConnectionStr);
                        rqmtConfirmList = tradeRqmtConfirmDal.GetAll(tradeIdList);

                        List<AssociatedDoc> assocDocList = new List<AssociatedDoc>();
                        AssociatedDocsDal assocDocsDal = new AssociatedDocsDal(sqlConnectionStr);
                        //assocDocList = assocDocsDal.GetAllSelectedTrades(tradingSystem, bookingCompanySn, cptySn,
                        //                    cdtyCode, tradeStartDate, tradeEndDate, ticketNo);                    

                        GetAllSelectedTradesResult(tradeList, rqmtList, rqmtConfirmList, assocDocList);

                        barChkNB.Checked = true;
                        barChkQRY.Checked = true;
                        barChkHP.Checked = false;
                        barChkRFA.Checked = false;
                        string faVal = (string)barEditFAComboBox.Items[2];
                        barEditFA.EditValue = faVal;
                    }
                    else
                        XtraMessageBox.Show("No trade was found matching the input criteria.", "Get All",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while getting all selected trades." + Environment.NewLine +
                       "Error CNF-080 in " + FORM_NAME + ".bbGetAll_ItemClick(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start(); //Israel 12/15/2008 - Red X
            }
        }

        private void GetAllSelectedTradesResult(List<SummaryData> pTradeList, List<RqmtData> pRqmtList,
           List<TradeRqmtConfirm> pRqmtConfirmList, List<AssociatedDoc> pAssocDocList)
        {
            string respStatus = null;

            try
            {
                if (pTradeList.Count > 0)
                {
                    ResetQryCodeColumn();
                    {
                        foreach (SummaryData rec in pTradeList)
                        {
                            DataRow drFind = summaryDataTable.Rows.Find(rec.Id);
                            if (drFind == null)
                            {
                                DataRow dr = summaryDataTable.NewRow();
                                dr = CollectionHelper.CreateDataRowFromObject<SummaryData>(rec, dr);
                                dr["QryCode"] = "Y";
                                summaryDataTable.Rows.Add(dr);
                            }
                            else  // we found this record..set qrycode column to Y
                            {
                                drFind["QryCode"] = "Y";
                                drFind.AcceptChanges();
                            }
                        }
                    }

                    if (pRqmtList.Count > 0)
                    {
                        foreach (RqmtData rec in pRqmtList)
                        {
                            DataRow drFind = rqmtDataTable.Rows.Find(rec.Id);
                            if (drFind == null)
                            {
                                DataRow dr = rqmtDataTable.NewRow();
                                dr = CollectionHelper.CreateDataRowFromObject<RqmtData>(rec, dr);
                                rqmtDataTable.Rows.Add(dr);
                            }
                            //else  // we found the row...update it
                            //{
                            //   CollectionHelper.UpdateDataRowFromObject<tradeRqmt>(rec, ref drFind);
                            //}
                        }
                    }
                    if (pRqmtConfirmList.Count > 0)
                    {
                        foreach (TradeRqmtConfirm rec in pRqmtConfirmList)
                        {
                            DataRow drFind = confirmDataTable.Rows.Find(rec.Id);
                            if (drFind == null)
                            {
                                DataRow dr = confirmDataTable.NewRow();
                                dr = CollectionHelper.CreateDataRowFromObject<TradeRqmtConfirm>(rec, dr);
                                confirmDataTable.Rows.Add(dr);
                            }
                        }
                    }
                    if (pAssocDocList.Count > 0)
                    {
                        System.Data.DataTable table = inboundPnl1.GetAssDocTable();
                        if (table == null) throw new Exception("Error CNF-532: Unable to acquire Associated Docs Table from Inbound.");

                        foreach (AssociatedDoc rec in pAssocDocList)
                        {
                            DataRow drFind = table.Rows.Find(rec.Id);
                            if (drFind == null)
                            {
                                DataRow dr = table.NewRow();
                                dr = CollectionHelper.CreateDataRowFromObject<AssociatedDoc>(rec, dr);
                                dr["TradeFinalApprovalFlag"] = "Y";
                                table.Rows.Add(dr);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                StringBuilder errorMsg = new StringBuilder();
                if (ex.Message.IndexOf("Too many rows returned") > -1)
                    errorMsg.Append(ex.Message.ToString());
                else
                    errorMsg.Append("Submit Get Trades Return Exception: " + ex.Message);
                Exception innerEx = ex.InnerException;

                while (innerEx != null)
                {
                    errorMsg.Append(innerEx.Message);
                    innerEx = innerEx.InnerException;
                }

                if (errorMsg.ToString().IndexOf("Too many rows returned") > -1)
                    XtraMessageBox.Show(errorMsg.ToString() + Environment.NewLine +
                           "Error CNF-332 in GetAllSelectedTradesResult(): " + ex.Message,
                       "Too Many Rows Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                    XtraMessageBox.Show("An error occurred while procesing all selected trades." + Environment.NewLine +
                           "Error CNF-081 in " + FORM_NAME + ".GetAllSelectedTradesResult(): " + ex.Message,
                         MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetQryCodeColumn()
        {
            try
            {
                DataRow[] rows = summaryDataTable.Select("(QryCode = 'Y')");
                if (rows != null)
                {
                    foreach (DataRow row in rows)
                    {
                        row["QryCode"] = "N";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while resetting the default Get All Query Code indicator." + Environment.NewLine +
                     "Error CNF-082 in " + FORM_NAME + ".ResetQryCodeColumn(): " + ex.Message);
            }
        }

        private void bbAudit_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myTimer.Stop(); //Israel 12/15/2008 - Red X

                auditForm.saveToExcelDirectory = this.saveToExcelDirectory;
                bool rowsFound = CallGetTradeAudit();
                if (rowsFound)
                {
                    auditForm.ShowDialog(this);
                    this.saveToExcelDirectory = auditForm.saveToExcelDirectory;
                }
                else
                    XtraMessageBox.Show("No changes have been made on this trade.");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while accessing the Audit Table display." + Environment.NewLine +
                       "Error CNF-083 in " + FORM_NAME + ".bbAudit_ItemClick(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start(); //Israel 12/15/2008 - Red X
            }
        }

        private bool CallGetTradeAudit()
        {
            bool rowsFound = false;
            try
            {
                string tradeIdStr = "";

                GridView view = gridViewSummary;

                List<TradeAuditDto> tradeAuditList = new List<TradeAuditDto>();
                TradeAuditDal tradeAuditDal = new TradeAuditDal(sqlConnectionStr);

                if (view.SelectedRowsCount == 1)
                {
                    tradeIdStr = view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeId").ToString();
                    int tradeId = Int32.Parse(tradeIdStr);
                    tradeAuditList = tradeAuditDal.GetTradeAudit(tradeId);

                    System.Data.DataTable table = this.dataSet.Tables["AuditDataTable"];
                    table.Clear();
                    if (tradeAuditList.Count > 0)
                    {
                        rowsFound = true;
                        foreach (TradeAuditDto rec in tradeAuditList)
                        {
                            DataRow row = table.NewRow();
                            row["TradeId"] = rec.TradeId;
                            row["TradeRqmtId"] = rec.TradeRqmtId;
                            row["Operation"] = rec.Operation;
                            row["MachineName"] = rec.Machine;
                            row["Timestamp"] = rec.Timestamp;
                            row["Status"] = rec.Status;
                            row["CompletedDt"] = rec.CompletedDt;
                            row["UserId"] = rec.UserId;
                            row["Rqmt"] = rec.Rqmt;
                            table.Rows.Add(row);
                        }
                    }
                }
                return rowsFound;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while building the Audit Table display data." + Environment.NewLine +
                     "Error CNF-084 in " + FORM_NAME + ".CallGetTradeAudit(): " + ex.Message);
            }
        }

        public void createAuditDataTable()
        {
            try
            {
                System.Data.DataTable auditDataTable = new System.Data.DataTable("AuditDataTable");
                auditDataTable.Columns.Add(new DataColumn("TradeId", typeof(long)));
                auditDataTable.Columns.Add(new DataColumn("TradeRqmtId", typeof(int)));
                auditDataTable.Columns.Add(new DataColumn("Operation", typeof(string)));
                auditDataTable.Columns.Add(new DataColumn("MachineName", typeof(string)));
                auditDataTable.Columns.Add(new DataColumn("Timestamp", typeof(DateTime)));
                auditDataTable.Columns.Add(new DataColumn("Status", typeof(string)));
                auditDataTable.Columns.Add(new DataColumn("CompletedDt", typeof(DateTime)));
                auditDataTable.Columns.Add(new DataColumn("UserId", typeof(string)));
                auditDataTable.Columns.Add(new DataColumn("Rqmt", typeof(string)));
                this.dataSet.Tables.Add(auditDataTable);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while building the internal Audit Table data structure." + Environment.NewLine +
                     "Error CNF-085 in " + FORM_NAME + ".createAuditDataTable(): " + ex.Message);
            }
        }

        private void bbTradeDataChanges_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                List<TradeDataChgDto> tradeDataChgList = new List<TradeDataChgDto>();
                myTimer.Stop(); //Israel 12/15/2008 - Red X
                tradeDataChangesForm.settingsDir = this.appSettingsDir;
                tradeDataChangesForm.saveToExcelDirectory = this.saveToExcelDirectory;
                tradeDataChgList = CallGetTradeDataChanges();
                if (tradeDataChgList.Count > 0)
                {
                    tradeDataChangesForm.tradeDataChangeList = tradeDataChgList;
                    tradeDataChangesForm.PopulateRowChangesList();
                    tradeDataChangesForm.ShowDialog(this);
                    this.saveToExcelDirectory = tradeDataChangesForm.saveToExcelDirectory;
                }
                else
                    XtraMessageBox.Show("No changes have been made on this trade.");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while processing the Trade Data Changes data for display." + Environment.NewLine +
                       "Error CNF-086 in " + FORM_NAME + ".bbTradeDataChanges_ItemClick(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start(); //Israel 12/15/2008 - Red X
            }
        }

        private List<TradeDataChgDto> CallGetTradeDataChanges()
        {
            //bool rowsFound = false;
            try
            {
                string tradeIdStr = "";

                GridView view = gridViewSummary;
                //getTradeDataChange dataChgRequest = new getTradeDataChange();
                //dataChgRequest.tradeDataChangeRequest = new tradeDataChangeRequest();
                //getTradeDataChangeResponse dataChgResponse = new getTradeDataChangeResponse();

                List<TradeDataChgDto> tradeDataChgList = new List<TradeDataChgDto>();
                TradeDataChgDal tradeDataChgDal = new TradeDataChgDal(sqlConnectionStr);

                if (view.SelectedRowsCount == 1)
                {
                    tradeIdStr = view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeId").ToString();
                    Int32 tradeId = Int32.Parse(tradeIdStr);
                    tradeDataChgList = tradeDataChgDal.GetTradeDataChg(tradeId);

                    System.Data.DataTable table = this.dataSet.Tables["TradeDataChangeTable"];
                    table.Clear();
                    if (tradeDataChgList.Count > 0)
                    {
                        //rowsFound = true;
                        foreach (TradeDataChgDto rec in tradeDataChgList)
                        {
                            DataRow row = table.NewRow();
                            row["JnDatetime"] = rec.JnDatetime;
                            row["TradeId"] = rec.TradeId;
                            row["BuySellInd"] = rec.BuySellInd;
                            row["TradeDt"] = rec.TradeDt;
                            row["BookingCoSn"] = rec.BookingCoSn;
                            row["CptySn"] = rec.CptySn;
                            row["BrokerSn"] = rec.BrokerSn;
                            row["TradeDesc"] = rec.TradeDesc;
                            row["PriceDesc"] = rec.PriceDesc;
                            row["TradeStatCode"] = rec.TradeStatCode;
                            row["LocationSn"] = rec.LocationSn;
                            row["QtyDesc"] = rec.QtyDesc;
                            row["QtyTot"] = rec.QtyTot;
                            row["TradeTypeCode"] = rec.TradeTypeCode;
                            row["CdtyCode"] = rec.CdtyCode;
                            row["SttlType"] = rec.SttlType;
                            row["Book"] = rec.Book;
                            row["TransportDesc"] = rec.TransportDesc;
                            row["CptyLegalName"] = rec.CptyLegalName;
                            row["BrokerLegalName"] = rec.BrokerLegalName;
                            row["BrokerPrice"] = rec.BrokerPrice;
                            row["Trader"] = rec.Trader;
                            row["CdtyGrpCode"] = rec.CdtyGrpCode;
                            row["StartDt"] = rec.StartDt;
                            row["EndDt"] = rec.EndDt;
                            row["Xref"] = rec.Xref;
                            row["RefSn"] = rec.RefSn;
                            row["InceptionDt"] = rec.InceptionDt;
                            row["OptnPutCallInd"] = rec.OptnPutCallInd;
                            row["OptnPremPrice"] = rec.OptnPremPrice;
                            row["OptnStrikePrice"] = rec.OptnStrikePrice;
                            row["ProfitCenter"] = rec.ProfitCenter;
                            row["PermissionKey"] = rec.PermissionKey;
                            //XtraMessageBox.Show("UpdBusnDt: " + dataChgResponse.@return.tradeDataChange[i].updateBusnDate.ToString("MM/dd/yyyy") + Environment.NewLine +
                            //   "CrtdTsGmt: " + dataChgResponse.@return.tradeDataChange[i].createDateTime.ToString("MM/dd/yyyy HH:mm"));
                            table.Rows.Add(row);
                        }
                    }
                }
                return tradeDataChgList;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while processing the internal Trade Data Changes data structure." + Environment.NewLine +
                     "Error CNF-087 in " + FORM_NAME + ".CallGetTradeDataChanges(): " + ex.Message);
            }
        }

        public void createTradeDataChangeTable()
        {
            try
            {
                System.Data.DataTable tradeDataChangeTable = new System.Data.DataTable("TradeDataChangeTable");
                tradeDataChangeTable.Columns.Add(new DataColumn("JnDatetime", typeof(DateTime)));
                tradeDataChangeTable.Columns.Add(new DataColumn("TradeId", typeof(Int32)));
                tradeDataChangeTable.Columns.Add(new DataColumn("BuySellInd", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("TradeDt", typeof(DateTime)));
                tradeDataChangeTable.Columns.Add(new DataColumn("BookingCoSn", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("CptySn", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("BrokerSn", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("TradeDesc", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("PriceDesc", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("TradeStatCode", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("LocationSn", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("QtyDesc", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("QtyTot", typeof(float)));
                tradeDataChangeTable.Columns.Add(new DataColumn("TradeTypeCode", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("CdtyCode", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("SttlType", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("Book", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("TransportDesc", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("CptyLegalName", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("BrokerLegalName", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("BrokerPrice", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("Trader", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("CdtyGrpCode", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("StartDt", typeof(DateTime)));
                tradeDataChangeTable.Columns.Add(new DataColumn("EndDt", typeof(DateTime)));
                tradeDataChangeTable.Columns.Add(new DataColumn("Xref", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("RefSn", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("InceptionDt", typeof(DateTime)));
                tradeDataChangeTable.Columns.Add(new DataColumn("OptnPutCallInd", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("OptnPremPrice", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("OptnStrikePrice", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("ProfitCenter", typeof(string)));
                tradeDataChangeTable.Columns.Add(new DataColumn("PermissionKey", typeof(string)));
                this.dataSet.Tables.Add(tradeDataChangeTable);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while building the internal Trade Data Changes data structure." + Environment.NewLine +
                     "Error CNF-088 in " + FORM_NAME + ".createTradeDataChangeTable(): " + ex.Message);
            }
        }

        private void barbtnGetDealsheet_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myTimer.Stop(); //Israel 12/15/2008 - Red X

                DisplayDealsheet();
                tabctrlBrowserApps.SelectedTabPage = tabpgDealsheet;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while retrieving the trade dealsheet." + Environment.NewLine +
                       "Error CNF-089 in " + FORM_NAME + ".barbtnGetDealsheet_ItemClick(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start(); //Israel 12/15/2008 - Red X
            }
        }

        private void DisplayDealsheet()
        {
            try
            {
                string trdSysTicket = "";
                string trdSysCode = "";

                GridView view = gridViewSummary;
                int rowHandle = view.FocusedRowHandle;

                if (rowHandle == DevExpress.XtraGrid.GridControl.AutoFilterRowHandle)
                {
                    rowHandle = 0;
                }
                trdSysCode = view.GetRowCellDisplayText(rowHandle, "TrdSysCode").ToString();
                trdSysTicket = view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeSysTicket").ToString();
                if (!("".Equals(trdSysTicket)))
                {
                    string getDocumentUrl = Properties.Settings.Default.ExtSvcAPIBaseUrl;
                    string svcUserName = Properties.Settings.Default.ExtSvcAPIUserName;
                    string svcPassword = Properties.Settings.Default.ExtSvcAPIPassword;
                    DealsheetAPIDal dealsheetApiDal = new DealsheetAPIDal(getDocumentUrl, svcUserName, svcPassword);
                    // DealsheetAPIDal dealsheetApiDal = new DealsheetAPIDal("http://hou-121.mercuria.met:9004", svcUserName, svcPassword);

                    string docFormatStr = "";
                    byte[] dealsheetByteStream = dealsheetApiDal.GetDealsheet(trdSysCode, trdSysTicket, out docFormatStr);
                    //   byte[] dealsheetByteStream = dealsheetApiDal.GetDealsheet("ICTS", "2658854", out docFormatStr);
                    DocumentFormat docFormat = Utils.GetDocumentFormat(docFormatStr);

                    if (dealsheetByteStream.Length > 0)
                    {
                        using (MemoryStream contractStream = new MemoryStream(dealsheetByteStream))
                        {
                            richeditDealsheetBrowser.LoadDocument(contractStream, docFormat);
                        }
                    }
                }
                else
                {
                    //webbrowserDealsheet.DocumentText = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to display the trade dealsheet." + Environment.NewLine +
                     "Error CNF-090 in " + FORM_NAME + ".DisplayDealsheet(): " + ex.Message);
            }
        }

        private void barBtnEConfirmMatched_ItemClick(object sender, ItemClickEventArgs e)
        {
            //try
            //{
            //    myTimer.Stop(); //Israel 12/15/2008 - Red X

            //    string cptyRefId = "";
            //    string statusDateStr = "";

            //    updECMatched.settingsDir = this.appSettingsDir;
            //    updECMatched.InitForm();
            //    if (updECMatched.ShowDialog(this) == DialogResult.OK)
            //    {
            //        statusDateStr = updECMatched.dedUpdStatusDate.DateTime.ToString("MM/dd/yyyy");
            //        cptyRefId = updECMatched.tedCptyRefId.EditValue.ToString();
            //        SubmitToEConfirmMatched(cptyRefId, statusDateStr);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    XtraMessageBox.Show("barBtnUpdateEConfirmMatched_ItemClick: " + ex.Message,
            //       "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //finally
            //{
            //    myTimer.Start(); //Israel 12/15/2008 - Red X
            //}
        }

        private void barBtnEConfirmBrokerMatched_ItemClick(object sender, ItemClickEventArgs e)
        {
            //try
            //{
            //    myTimer.Stop(); //Israel 12/15/2008 - Red X

            //    GridView view = gridViewSummary;
            //    int rowHandle = GetGridViewFocusedRowHandle(view);
            //    string tradeId = view.GetRowCellDisplayText(rowHandle, "TradeId").ToString();
            //    long rqmtId = 0;
            //    string reference = "";
            //    string cmt = "";
            //    string filterStr = "TradeId = " + tradeId + " and Rqmt = 'ECBKR'";
            //    foreach (DataRow row in rqmtDataTable.Select(filterStr))
            //    {
            //        rqmtId = long.Parse(row["Id"].ToString());
            //        reference = row["Reference"].ToString();
            //        cmt = row["Cmt"].ToString();
            //    }
            //    DialogResult result = XtraMessageBox.Show("Set eConfirm Broker to Matched?",
            //       "Set eConfirm Broker to Matched", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //    if (result == DialogResult.Yes)
            //    {
            //        long tradeIdLong = long.Parse(tradeId);
            //        cmt += "; Set to MATCH by Utility program";
            //        SubmitEditRqmt(tradeIdLong, rqmtId, "ECBKR", "MATCH", DateTime.Today, reference, cmt, true);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    XtraMessageBox.Show("barBtnEConfirmBrokerMatched_ItemClick: " + ex.Message,
            //       "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //finally
            //{
            //    myTimer.Start(); 
            //}
        }

        private void SubmitToEConfirmMatched(string ACptyRefId, string AStatusDateStr)
        {
            try
            {
                //GridView view = gridViewSummary;
                //if (view.SelectedRowsCount == 1)
                //{
                //eConfirmMatchRequest[] ecMatchRequest = new eConfirmMatchRequest[1];
                //ecMatchRequest[0] = new eConfirmMatchRequest();
                //ecMatchRequest[0].cptyRefId = ACptyRefId;
                //ecMatchRequest[0].statusDateStr = AStatusDateStr;
                //ecMatchRequest[0].tradeId =
                //   int.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeId"));

                //confirmUtilService.batchEConfirmMatchCompleted -=
                //   new batchEConfirmMatchCompletedEventHandler(SubmitToEConfirmMatchedReturn);
                //confirmUtilService.batchEConfirmMatchCompleted +=
                //   new batchEConfirmMatchCompletedEventHandler(SubmitToEConfirmMatchedReturn);
                //confirmUtilService.batchEConfirmMatchAsync(ecMatchRequest, new object());

                //if (finalApprovalIgnored > 0)
                //   XtraMessageBox.Show("Final Approved trades ignored: " + finalApprovalIgnored.ToString(), // + Environment.NewLine
                //      //+ "Determine Action trades processed: " + determineActionRow.ToString() + Environment.NewLine,
                //        "Add Rqmt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
            }
            catch (Exception ex)
            {
                //throw new Exception("SubmitToEConfirmMatched: " + ex.Message);
            }
        }

        private void barbtnEConfirmSent_ItemClick(object sender, ItemClickEventArgs e)
        {
            //try
            //{
            //    myTimer.Stop(); //Israel 12/15/2008 - Red X

            //    string rqmtCode = "NONE";
            //    string rqmtStatus = "NONE";
            //    string msgBoxLabel = "";
            //    long rqmtId = 0;
            //    if (e.Item.Name == barbtnEConfirmCptySent.Name)
            //    {
            //        rqmtCode = "ECONF";
            //        msgBoxLabel = "Set eConfirm Cpty to SENT";
            //    }
            //    else if (e.Item.Name == barbtnEConfirmBrokerSent.Name)
            //    {
            //        rqmtCode = "ECBKR";
            //        msgBoxLabel = "Set eConfirm Broker to SENT";
            //    }

            //    GridView view = gridViewSummary;
            //    string tradeId = view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeId").ToString();
            //    string filterStr = "TradeId = " + tradeId + " and Rqmt = '" + rqmtCode + "'";
            //    foreach (DataRow row in rqmtDataTable.Select(filterStr))
            //    {
            //        rqmtId = long.Parse(row["Id"].ToString());
            //        rqmtStatus = row["Status"].ToString();
            //    }

            //    DialogResult result = XtraMessageBox.Show(msgBoxLabel + "?",
            //       msgBoxLabel, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //    if (result == DialogResult.Yes)
            //    {
            //        long tradeIdLong = long.Parse(tradeId);
            //        SubmitToEConfirmSent(tradeIdLong, rqmtId);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    XtraMessageBox.Show("barbtnEConfirmSent_ItemClick: " + ex.Message,
            //       "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //finally
            //{
            //    myTimer.Start(); //Israel 12/15/2008 - Red X
            //}
        }

        private void SubmitToEConfirmSent(long ATradeId, long ARqmtId)
        {
            try
            {
                //GridView view = gridViewSummary;
                //if (view.SelectedRowsCount == 1)
                //{
                //updateTradeRqmtStatus ecSentRequest = new updateTradeRqmtStatus();
                //ecSentRequest.tradeRqmtRequest = new tradeRqmtRequest();
                //ecSentRequest.tradeRqmtRequest.rqmtId = ARqmtId;
                //ecSentRequest.tradeRqmtRequest.tradeId = ATradeId;
                //ecSentRequest.tradeRqmtRequest.status = "SENT";
                //ecSentRequest.tradeRqmtRequest.statusDate = DateTime.Today;
                //ecSentRequest.tradeRqmtRequest.statusDateSpecified = true;

                //tradeConfirmService.updateTradeRqmtStatusCompleted -=
                //   new updateTradeRqmtStatusCompletedEventHandler(SubmitToEConfirmSentReturn);
                //tradeConfirmService.updateTradeRqmtStatusCompleted +=
                //   new updateTradeRqmtStatusCompletedEventHandler(SubmitToEConfirmSentReturn);
                //tradeConfirmService.updateTradeRqmtStatusAsync(ecSentRequest, new object());
                //}
            }
            catch (Exception ex)
            {
                //throw new Exception("SubmitToEConfirmSent: " + ex.Message);
            }
        }

        private void barbtnSempraPaperNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                GridView view = gridViewSummary;
                dataManager.StopOpsTimer();
                myTimer.Stop(); //Israel 12/15/2008 - Red X

                DialogResult result = XtraMessageBox.Show("Set Our Paper Rqmt to NEW?",
                   "Set Our Paper Rqmt to NEW", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    SetToOurPaperNew();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while processing Set Our Paper to NEW." + Environment.NewLine +
                       "Error CNF-091 in " + FORM_NAME + ".barbtnSempraPaperNew_ItemClick(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dataManager.StartOpsTimer();
                myTimer.Start(); //Israel 12/15/2008 - Red X
            }
        }

        private void SetToOurPaperNew()
        {
            try
            {
                GridView view = gridViewSummary;
                if (view.SelectedRowsCount == 1)
                {
                    int rqmtId = int.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "SetcDbUpd"));
                    long tradeId = long.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeId"));
                    CallUpdateTradeRqmts(tradeId, rqmtId, SEMPRA_RQMT, "NEW", DateTime.Today, "", "", true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to update the new setting for the trade requirement." + Environment.NewLine +
                     "Error CNF-092 in " + FORM_NAME + ".SetToSempraPaperNew(): " + ex.Message);
            }
        }

        private void bbXmitResult_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myTimer.Stop();

                xmitResultStatusLogForm.saveToExcelDirectory = this.saveToExcelDirectory;
                bool rowsFound = CallGetXmitResultLog();
                if (rowsFound)
                {
                    xmitResultStatusLogForm.ShowDialog(this);
                    this.saveToExcelDirectory = xmitResultStatusLogForm.saveToExcelDirectory;
                }
                else
                    XtraMessageBox.Show("No status updates have occurred for this document.");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while retrieving updated information from the transmission result data." + Environment.NewLine +
                       "Error CNF-093 in" + FORM_NAME + ". bbXmitResult_ItemClick(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start();
            }
        }

        private bool CallGetXmitResultLog()
        {
            bool rowsFound = false;
            try
            {
                string confirmId = "";
                GridView view = gridViewConfirm;

                if (view.SelectedRowsCount == 1)
                {
                    confirmId = view.GetRowCellDisplayText(view.FocusedRowHandle, "Id").ToString();

                    System.Data.DataTable table = this.dataSet.Tables["XmitResultTable"];
                    table.Clear();

                    XmitResultDal xmitResultDal = new XmitResultDal(sqlConnectionStr);
                    List<XmitResultDto> xmitResultList = null;

                    Int32 confirmIdInt = 0;
                    if (!Int32.TryParse(confirmId, out confirmIdInt))
                        confirmIdInt = -1;
                    //if (isTestMode)
                    //    faxLogStatusList = faxStatusLogDal.GetStub();
                    //else

                    if (confirmIdInt > 0)
                        xmitResultList = xmitResultDal.GetByTradeRqmtConfirmId(confirmIdInt);
                    //xmitResultList = xmitResultDal.GetByAssociatedDocsId(56);

                    foreach (XmitResultDto statusLog in xmitResultList)
                    {
                        DataRow row = table.NewRow();
                        row["XmitResultId"] = statusLog.XmitResultId;
                        row["XmitRequestId"] = statusLog.XmitRequestId;
                        row["AssociatedDocsId"] = statusLog.AssociatedDocsId;
                        row["TradeRqmtConfirmId"] = statusLog.TradeRqmtConfirmId;
                        row["SentByUser"] = statusLog.SentByUser;
                        row["XmitStatusInd"] = statusLog.XmitStatusInd;
                        row["XmitMethodInd"] = statusLog.XmitMethodInd;
                        row["XmitDest"] = statusLog.XmitDest;
                        row["XmitCmt"] = statusLog.XmitCmt;
                        row["XmitTimestamp"] = statusLog.XmitTimestamp;
                        table.Rows.Add(row);
                    }

                    rowsFound = xmitResultList.Count > 0;
                }
                return rowsFound;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to display updated transmission result data." + Environment.NewLine +
                     "Error CNF-094 in " + FORM_NAME + ".CallGetXmitResultLog(): " + ex.Message);
            }
        }

        public void createXmitResultDataTable()
        {
            try
            {
                System.Data.DataTable xmitResultTable = new System.Data.DataTable("XmitResultTable");
                xmitResultTable.Columns.Add(new DataColumn("XmitResultId", typeof(Int32)));
                xmitResultTable.Columns.Add(new DataColumn("XmitRequestId", typeof(Int32)));
                xmitResultTable.Columns.Add(new DataColumn("AssociatedDocsId", typeof(Int32)));
                xmitResultTable.Columns.Add(new DataColumn("TradeRqmtConfirmId", typeof(Int32)));
                xmitResultTable.Columns.Add(new DataColumn("SentByUser", typeof(string)));
                xmitResultTable.Columns.Add(new DataColumn("XmitStatusInd", typeof(string)));
                xmitResultTable.Columns.Add(new DataColumn("XmitMethodInd", typeof(string)));
                xmitResultTable.Columns.Add(new DataColumn("XmitDest", typeof(string)));
                xmitResultTable.Columns.Add(new DataColumn("XmitCmt", typeof(string)));
                xmitResultTable.Columns.Add(new DataColumn("XmitTimestamp", typeof(DateTime)));
                this.dataSet.Tables.Add(xmitResultTable);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to build the transmission result internal storage." + Environment.NewLine +
                     "Error CNF-095 in " + FORM_NAME + ".createXmitResultDataTable(): " + ex.Message);
            }
        }

        public void GetCompanies()
        {
            try
            {
                UserCompanyDal userCompanyDal = new UserCompanyDal(sqlConnectionStr);
                //string userName = Sempra.Ops.Utils.GetUserName().ToUpper();

                //As per jack there is nothing to do with permission key s while loading comanies . We have to get all the booking companies to show in Get All panel.
                List<UserCompanyDto> companyListDto;
                //if (isTestMode)
                //    companyListDto = userCompanyDal.GetAllStub();
                //else
                companyListDto = userCompanyDal.GetAll(String.Empty);

                foreach (UserCompanyDto company in companyListDto)
                {
                    DataRow row = seCptyLkupTbl.NewRow();
                    row["BookingCoSn"] = company.CompanySn;
                    seCptyLkupTbl.Rows.Add(row);
                }
            }

            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to populate an internal company list." + Environment.NewLine +
                    "Error CNF-096 in " + FORM_NAME + ".GetCompanies(): " + ex.Message,
                   MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Grid view handlers

        private void gridViewSummary_ShowGridMenu(object sender, DevExpress.XtraGrid.Views.Grid.GridMenuEventArgs e)
        {
            try
            {
                //Israel --Adds the fixed options to column customization
                if (e.MenuType == DevExpress.XtraGrid.Views.Grid.GridMenuType.Column)
                {
                    DevExpress.XtraGrid.Menu.GridViewColumnMenu menu = e.Menu as DevExpress.XtraGrid.Menu.GridViewColumnMenu;
                    if (menu.Column != null)
                    {
                        menu.Items.Add(CreateCheckItem("Not Fixed", menu.Column, FixedStyle.None, imageList2.Images[0]));
                        menu.Items.Add(CreateCheckItem("Fixed Left", menu.Column, FixedStyle.Left, imageList2.Images[1]));
                        menu.Items.Add(CreateCheckItem("Fixed Right", menu.Column, FixedStyle.Right, imageList2.Images[2]));
                    }
                }

                if (e.Menu == null || e.Menu.GetType() == typeof(DevExpress.XtraGrid.Menu.GridViewMenu))
                {
                    popupSummary.ShowPopup(gridMain.PointToScreen(e.Point));
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to display the Summary Grid popup menu." + Environment.NewLine +
                       "Error CNF-097 in " + FORM_NAME + ".gridViewSummary_ShowGridMenu(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridViewSummary_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column == null) return;
            try
            {
                Boolean updateHeader = false;
                switch (e.Column.Name)
                {
                    case "colSummaryBkrStatus":
                        { updateHeader = true; break; }
                    case "colSummaryBkrMeth":
                        { updateHeader = true; break; }
                    case "colSummaryBkrRqmt":
                        { updateHeader = true; break; }
                    case "colSummaryCptyStatus":
                        { updateHeader = true; break; }
                    case "colSummaryCptyMeth":
                        { updateHeader = true; break; }
                    case "colSummaryCptyRqmt":
                        { updateHeader = true; break; }
                    case "colSummarySetcStatus":
                        { updateHeader = true; break; }
                    case "colSummarySetcMeth":
                        { updateHeader = true; break; }
                    case "colSummarySetcRqmt":
                        { updateHeader = true; break; }
                    case "colSummaryNoConfStatus":
                        { updateHeader = true; break; }
                    case "colSummaryNoConfMeth":
                        { updateHeader = true; break; }
                    case "colSummaryNoConfRqmt":
                        { updateHeader = true; break; }
                    case "colSummaryFinalApprovalFlag":
                        { updateHeader = true; break; }
                    case "colSummaryOpsDetActFlag":
                        { updateHeader = true; break; }
                    case "colSummaryVerblStatus":
                        { updateHeader = true; break; }
                    case "colSummaryVerblMeth":
                        { updateHeader = true; break; }
                    case "colSummaryVerblRqmt":
                        { updateHeader = true; break; }
                }

                if (updateHeader)
                    e.Info.Appearance.Font = new System.Drawing.Font(e.Info.Appearance.Font, FontStyle.Bold);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to set certain column headings display bold text." + Environment.NewLine +
                       "Error CNF-098 in " + FORM_NAME + ".gridViewSummary_CustomDrawColumnHeader(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridViewSummary_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column == null || e.RowHandle < 0) return;
            string detActionFlag;
            string hasProblemFlag;
            string readyForFinalApprovalFlag;
            string book;
            Boolean isRowSelected;
            try
            {
                GridView view = sender as GridView;
                isRowSelected = view.IsRowSelected(e.RowHandle);
                //isRowSelected = (e.RowHandle == view.FocusedRowHandle);
                detActionFlag = view.GetRowCellDisplayText(e.RowHandle, "OpsDetActFlag");
                hasProblemFlag = view.GetRowCellDisplayText(e.RowHandle, "HasProblemFlag");
                readyForFinalApprovalFlag =
                   view.GetRowCellDisplayText(e.RowHandle, "ReadyForFinalApprovalFlag");
                book = view.GetRowCellDisplayText(e.RowHandle, "Book");

                if (e.Column.Name == "colSummaryTrdSysTicket")
                {
                    if (detActionFlag == "Y")
                    {
                        e.Appearance.Font = new System.Drawing.Font(e.Appearance.Font, FontStyle.Bold);
                        SetSummaryColumnColor(isRowSelected, e, Color.FromName("SandyBrown")); //Orange
                    }
                    if (hasProblemFlag == "Y")
                        SetSummaryColumnColor(isRowSelected, e, Color.FromName("Tomato")); //Red
                    if (readyForFinalApprovalFlag == "Y")
                        SetSummaryColumnColor(isRowSelected, e, Color.FromName("LightGreen")); //Green
                }

                if (book == "Z" || book == "ZZ" || book == "TEST SG NG" || book == "TEST SG ELEC")
                    e.Appearance.Font = new System.Drawing.Font(e.Appearance.Font, FontStyle.Bold | FontStyle.Italic);

                Color color;
                Boolean isTargetColumn;
                string rqmtValue;
                string rqmtStatus;

                //Broker Requirement
                isTargetColumn = (e.Column.Name == "colSummaryBkrStatus") ||
                                 (e.Column.Name == "colSummaryBkrMeth") ||
                                 (e.Column.Name == "colSummaryBkrRqmt");
                rqmtValue = view.GetRowCellDisplayText(e.RowHandle, "BkrRqmt");
                if (isTargetColumn && rqmtValue != "")
                {
                    rqmtStatus = view.GetRowCellDisplayText(e.RowHandle, "BkrStatus");
                    color = GetHashkeyColor(rqmtValue + rqmtStatus);
                    SetSummaryColumnColor(isRowSelected, e, color);
                }

                //Cpty Requirement
                isTargetColumn = (e.Column.Name == "colSummaryCptyStatus") ||
                                 (e.Column.Name == "colSummaryCptyMeth") ||
                                 (e.Column.Name == "colSummaryCptyRqmt");
                rqmtValue = view.GetRowCellDisplayText(e.RowHandle, "CptyRqmt");
                if (isTargetColumn && rqmtValue != "")
                {
                    rqmtStatus = view.GetRowCellDisplayText(e.RowHandle, "CptyStatus");
                    color = GetHashkeyColor(rqmtValue + rqmtStatus);
                    SetSummaryColumnColor(isRowSelected, e, color);
                }

                //Sempra Requirement
                isTargetColumn = (e.Column.Name == "colSummarySetcStatus") ||
                                 (e.Column.Name == "colSummarySetcMeth") ||
                                 (e.Column.Name == "colSummarySetcRqmt");
                rqmtValue = view.GetRowCellDisplayText(e.RowHandle, "SetcRqmt");
                if (isTargetColumn && rqmtValue != "")
                {
                    rqmtStatus = view.GetRowCellDisplayText(e.RowHandle, "SetcStatus");
                    color = GetHashkeyColor(rqmtValue + rqmtStatus);
                    SetSummaryColumnColor(isRowSelected, e, color);
                }

                //NoConfirm Requirement
                isTargetColumn = (e.Column.Name == "colSummaryNoconfStatus") ||
                                 (e.Column.Name == "colSummaryNoconfMeth") ||
                                 (e.Column.Name == "colSummaryNoconfRqmt");
                rqmtValue = view.GetRowCellDisplayText(e.RowHandle, "NoconfRqmt");
                if (isTargetColumn && rqmtValue != "")
                {
                    rqmtStatus = view.GetRowCellDisplayText(e.RowHandle, "NoconfStatus");
                    color = GetHashkeyColor(rqmtValue + rqmtStatus);
                    SetSummaryColumnColor(isRowSelected, e, color);
                }

                //Verbal Requirement
                isTargetColumn = (e.Column.Name == "colSummaryVerblStatus") ||
                                 (e.Column.Name == "colSummaryVerblMeth") ||
                                 (e.Column.Name == "colSummaryVerblRqmt");
                rqmtValue = view.GetRowCellDisplayText(e.RowHandle, "VerblRqmt");
                if (isTargetColumn && rqmtValue != "")
                {
                    rqmtStatus = view.GetRowCellDisplayText(e.RowHandle, "VerblStatus");
                    color = GetHashkeyColor(rqmtValue + rqmtStatus);
                    SetSummaryColumnColor(isRowSelected, e, color);
                }

                if (e.Column.Name == "colSummaryFinalApprovalTimestampGmt")
                {
                    DateTime faTs = (DateTime)e.CellValue;
                    if (faTs < DateTime.Today.AddYears(-100))
                        //if (e.DisplayText == "01-Jan-1900 01:00 PM")
                        e.DisplayText = "";
                    else
                    {
                        e.Column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                        e.Column.DisplayFormat.FormatString = "dd-MMM-yyyy hh:mm tt";
                    }
                }
                if (e.Column.Name == "colSummaryStartDt" ||
                    e.Column.Name == "colSummaryEndDt")
                    if (e.DisplayText == "01-Jan-1900")
                        e.DisplayText = "";
                    else
                    {
                        e.Column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        e.Column.DisplayFormat.FormatString = "dd-MMM-yyyy";
                    }
                if (e.Column.Name == "colSummaryQtyTot")
                {
                    if (e.CellValue != "0")
                    {
                        double qty = Double.Parse(e.CellValue.ToString());
                        e.DisplayText = String.Format("{0:#,0.######;(#,0.######)}", qty);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to apply custom content formatting for certain columns." + Environment.NewLine +
                       "Error CNF-099 in " + FORM_NAME + ".gridViewSummary_CustomDrawCell(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridViewSummary_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;
            string archiveFlag = "";
            try
            {
                //GridView view = sender as GridView;
                //archiveFlag = view.GetRowCellDisplayText(e.RowHandle, view.Columns["ArchiveFlag"]);
                //if (archiveFlag != "N")
                //{ 
                //   e.Appearance.BackColor = Color.LightSteelBlue;
                //   e.Appearance.BackColor2 = Color.LightSteelBlue;
                //   e.Appearance.BorderColor = Color.LightSteelBlue;
                //}
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to apply a custom background color for certain columns." + Environment.NewLine +
                       "Error CNF-100 in " + FORM_NAME + ".gridViewSummary_RowCellStyle(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridViewSummary_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;
            //string archiveFlag = "";
            try
            {
                GridView view = sender as GridView;
                //archiveFlag = view.GetRowCellDisplayText(e.RowHandle, view.Columns["ArchiveFlag"]);
                //if (archiveFlag == "N")
                {
                    DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo vi =
                       view.GetViewInfo() as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo;
                    if (e.RowHandle % 2 == 0)
                        e.Appearance.Assign(vi.PaintAppearance.OddRow);
                    else
                        e.Appearance.Assign(vi.PaintAppearance.EvenRow);
                }
                //else
                //{
                //   e.Appearance.BackColor = Color.LightSteelBlue;
                //   e.Appearance.BackColor2 = Color.LightSteelBlue;
                //   e.Appearance.BorderColor = Color.LightSteelBlue;
                //}
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to apply a custom background color for certain rows." + Environment.NewLine +
                       "Error CNF-101 in " + FORM_NAME + ".gridViewSummary_RowStyle(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void SetSummaryColumnColor(Boolean isRowSelected,
           DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs eArgs, Color color)
        {
            if (eArgs.RowHandle < 0) return;
            try
            {
                if (isRowSelected)
                    eArgs.Appearance.ForeColor = color;
                else
                {
                    eArgs.Appearance.BackColor = color;
                    eArgs.Appearance.BackColor2 = color;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to apply custom column colors according to the row selection state." + Environment.NewLine +
                       "Error CNF-102 in SetSummaryColumnColor(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridViewSummary_RowCountChanged(object sender, EventArgs e)
        {
            DisplayRowCount();
        }

        private void gridViewSummary_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            InboundPnl.ActiveSummaryData = null;
            //Israel 11/04/2015 Prevent 'automatic' switching of current tab page
            int currentTabPageIndex = 0;
            if (initComplete)
                currentTabPageIndex = inboundPnl1.tabCntrlMain.SelectedTabPageIndex;

            //if (gridViewSummary.SelectedRowsCount == 0)
            //    return;

            int rowHandle = 0;
            if (e.FocusedRowHandle < 0)
            {
                rowHandle = e.PrevFocusedRowHandle;
                if (rowHandle < 0)
                    return;
            }
            else rowHandle = e.FocusedRowHandle;

            string tradeId = "0";
            DataRow dr = null;

            try
            {
                GridView view = sender as GridView;
                tradeId = view.GetRowCellDisplayText(rowHandle, "TradeId");
                if (tradeId == null || tradeId.Equals(""))
                {
                    return;
                    //tradeId = "0";
                }
                gridViewRqmt.ActiveFilterString = "[TradeId] = " + tradeId;

                if (inboundPnl1 != null)
                {
                    if ("".Equals(tradeId))
                    {
                        inboundPnl1.FilteredTradeId = 0;
                    }
                    else
                    {
                        inboundPnl1.FilteredTradeId = Convert.ToInt32(tradeId);
                    }
                    dr = gridViewSummary.GetDataRow(rowHandle);
                    InboundPnl.ActiveSummaryData = CollectionHelper.CreateObjectFromDataRow<SummaryData>(dr);
                }

                SelectedRqmtHasChanged();
                gridViewRqmt.RefreshData();

                gridViewConfirm.ActiveFilterString = "[TradeId] = " + tradeId;
                gridViewConfirm.RefreshData();

                if (isAutoDispDealsheet)
                    DisplayDealsheet();

                //DisplayTradeInfo();
                //UnselectLockedRow(sender, e.FocusedRowHandle);

                if (initComplete)
                    inboundPnl1.tabCntrlMain.SelectedTabPageIndex = currentTabPageIndex;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to perform actions normally done" + Environment.NewLine +
                    "each time a new trade is selected in the Summary grid." + Environment.NewLine +
                       "Error CNF-103 in " + FORM_NAME + ".gridViewSummary_FocusedRowChanged(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridViewSummary_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            try
            {
                DisplayRowCount();
                DisplayTradeInfo();
                //if (gridViewSummary.SelectedRowsCount == 1 && isAutoDispDealsheet)
                //DisplayDealsheet();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to apply custom column colors according to the row selection state." + Environment.NewLine +
                       "Error CNF-104 in " + FORM_NAME + ".gridViewSummary_SelectionChanged(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridViewSummary_ColumnFilterChanged(object sender, EventArgs e)
        {
            try
            {
                int rowHandle = 0;
                rowHandle = gridViewSummary.FocusedRowHandle;

                if (rowHandle == DevExpress.XtraGrid.GridControl.AutoFilterRowHandle)
                {
                    rowHandle = 0;
                }
                gridViewSummary_FocusedRowChanged(gridViewSummary,
                   new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(0, rowHandle));
                gridViewSummary.SelectRow(rowHandle);
                gridViewSummary.FocusedRowHandle = rowHandle;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to apply column-specific filter settings." + Environment.NewLine +
                       "Error CNF-105 in " + FORM_NAME + ".gridViewSummary_ColumnFilterChanged(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridViewRqmt_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;
            string colorStr;
            //Boolean isRowSelected;
            Color color;

            try
            {
                GridView view = sender as GridView;
                //isRowSelected = (view.IsRowSelected(e.RowHandle));
                //e.Appearance.Assign(view.PaintAppearance.SelectedRow);

                colorStr = view.GetRowCellDisplayText(e.RowHandle, "GuiColorCode");
                //color = Color.FromName((string)translateColors[colorStr]);
                color = TranslateColor(colorStr);
                e.Appearance.BackColor = color;
                e.Appearance.ForeColor = Color.Black;
            }
#pragma warning disable 0168
            //Disable warning...
            catch (Exception ex)
#pragma warning restore 0168
            {
                //XtraMessageBox.Show("gridViewRqmt_CustomDrawCell: " + ex.Message,
                // "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridViewRqmt_CustomDrawCell(object sender,
           DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column == null || e.RowHandle < 0) return;
            try
            {
                if (e.Column.Name == "colRqmtCompletedTimestampGmt")
                    if (e.DisplayText == "01-Jan-0001 05:00")
                        e.DisplayText = "";
                    else
                    {
                        e.Column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        e.Column.DisplayFormat.FormatString = "dd-MMM-yyyy hh:mm";
                    }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to apply column-specific data formats." + Environment.NewLine +
                       "Error CNF-106 in " + FORM_NAME + ".gridViewRqmt_CustomDrawCell(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridViewConfirm_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column == null || e.RowHandle < 0) return;
            GridView view = sender as GridView;
            Color color;
            bool isRowSelected = view.IsRowSelected(e.RowHandle);
            string faxTelexNumber = view.GetRowCellDisplayText(e.RowHandle, "FaxTelexNumber");
            string nextStatusCode = view.GetRowCellValue(e.RowHandle, "NextStatusCode").ToString();
            string confirmLabel = view.GetRowCellDisplayText(e.RowHandle, "ConfirmLabel");

            if (faxTelexNumber.Trim().Length < 3 && nextStatusCode.Trim().Length > 0)
                e.Appearance.Font = new System.Drawing.Font(e.Appearance.Font, FontStyle.Bold | FontStyle.Italic);

            if (e.Column.Name == "colConfirmTemplateTypeInd")
            {
                if (e.DisplayText == "L")
                    e.DisplayText = "LONG";
                else if (e.DisplayText == "S")
                    e.DisplayText = "SHORT";
            }
            else if (e.Column.Name == "colConfirmFaxTelexInd")
            {
                if (e.DisplayText == "F")
                    e.DisplayText = "FAX";
                else if (e.DisplayText == "T")
                    e.DisplayText = "TELEX";
                else if (e.DisplayText == "E")
                    e.DisplayText = "EMAIL";
            }
            else if (e.Column.Name == "colConfirmXmitTimeStampGmt")
            {
                if (e.DisplayText.StartsWith("01-Jan-1900"))
                {
                    e.DisplayText = "";
                }
                else
                {
                    e.Column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                    e.Column.DisplayFormat.FormatString = "dd-MMM-yyyy hh:mm tt";
                }
            }
            //else if (e.Column.Name == "colConfirmNextStatusCode" && confirmLabel != CONTRACT &&
            else if (e.Column.Name == "colConfirmNextStatusCode" && confirmLabel == CONFIRM_LABEL_CONFIRM &&
                nextStatusCode.Length > 0)
            {
                //color = GetHashkeyColor(SEMPRA_RQMT + nextStatusCode);
                //SetSummaryColumnColor(isRowSelected, e, color);
                e.DisplayText = "";
            }
        }

        private void gridViewRqmt_ShowGridMenu(object sender, GridMenuEventArgs e)
        {
            try
            {
                GridView view = gridViewRqmt;
                    
                //No need to test for single row selected since that's controlled by the grid property.
                string rqmtCode = view.GetRowCellDisplayText(view.FocusedRowHandle, "Rqmt");
                string status = view.GetRowCellDisplayText(view.FocusedRowHandle, "Status");
                string finalApprovalFlag = view.GetRowCellDisplayText(view.FocusedRowHandle, "FinalApprovalFlag");
                long tradeId = long.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeId").ToString());
                long rqmtId = long.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "Id").ToString());
                string preparerCanSendFlag = GetContractConfirmData(tradeId, "PreparerCanSendFlag");

                if (e.Menu == null || e.Menu.GetType() == typeof(DevExpress.XtraGrid.Menu.GridViewMenu))
                {
                    if (rqmtCode == SEMPRA_RQMT)
                    {
                        //string paperCreator = "";
                        //bool isPaperCreator = true;
                        bool finalApproved = (finalApprovalFlag == "Y");
                        bool cancelled = status == "CXL";
                        if (status == "NEW")
                        {
                            barBtnAddEditConfirm.Enabled = !finalApproved && isHasUpdate;
                            barBtnAddEditConfirm.Caption = "Get Confirm";
                        }
                        else
                        {
                            barBtnAddEditConfirm.Enabled = (gridViewConfirm.RowCount > 0) && !finalApproved && !cancelled && isHasUpdate;
                            barBtnAddEditConfirm.Caption = "Edit Confirm";
                            //5/27/09 Israel -- Creator can't approve
                            long rqmtConfirmId = GetContractRowId(tradeId);
                            //Israel 9/22/2015
                            //paperCreator = GetPaperCreator(rqmtConfirmId);
                            //if (Properties.Settings.Default.RunDemo.Equals("Y"))  // DEMO ENVIRONMENT...KEEP OKTOSEND ENABLED.
                            //{
                            //    isPaperCreator = false;
                            //}
                            //else
                            //{
                            //    isPaperCreator = (paperCreator == toolbarOrWindowsUserId.ToUpper());
                            //}
                        }

                        barBtnSelectManualTemplate.Enabled = (status == "NEW") && !finalApproved && !cancelled && isHasUpdate;
                        barBtnCreateAdditionalConfirm.Enabled = (gridViewConfirm.RowCount > 0) &&
                           !finalApproved && !cancelled && isHasUpdate;

                        barbtnRqmtSetToPrep.Enabled = (status == "TRADER" || status == "EXT_REVIEW" ||
                           status == "MGR" || status == "OK_TO_SEND" || status == "SENT") && !finalApproved && isHasUpdate;

                        // New and Cancel are controlled via security and are set to visible in ApplyUserAccess()
                        barbtnRqmtSetToNew.Enabled = (status == "PREP" || status == "CXL") && !finalApproved && isHasUpdate;
                        barbtnRqmtCancel.Enabled = (status != "CXL") && !finalApproved && !cancelled && isHasUpdate;

                        string faxTelexNumber = GetContractConfirmData(tradeId, "FaxTelexNumber");
                        bool enableOkAndSend = (status == "MGR") && (faxTelexNumber.Trim().Length > 2) &&
                                               (!finalApproved && !cancelled && isHasUpdate);
                        bool enableSend = (status == "OK_TO_SEND") && faxTelexNumber.Trim().Length > 2 &&
                           !finalApproved && !cancelled && isHasUpdate;
                        bool enableResend = ((status == "SENT") ||
                                             (status == "FAIL") ||
                                             (status == "DISP") ||
                                             (status == "APPR") ||
                                             (status == "SIGNED") ||
                                             (status == "ACPTD") ||
                                             (status == "RESNT")) && faxTelexNumber.Trim().Length > 2 &&
                                             !finalApproved && !cancelled && isHasUpdate;

                        ChangeRQMTContextMenuVisibility(preparerCanSendFlag == "Y");

                        barbtnRqmtSetToOkToSend.Enabled = enableOkAndSend;
                        barbtnRqmtSetToOkAndSend.Enabled = enableOkAndSend;
                        barbtnRqmtSetToOkAndManualSend.Enabled = (status == "MGR") &&
                           !finalApproved && !cancelled && isHasUpdate;

                        barbtnRqmtSend.Enabled = enableSend;
                        barbtnRqmtResend.Enabled = enableResend;
                        barbtnRqmtManualSend.Enabled = (status == "OK_TO_SEND" || status == "FAIL") &&
                           !finalApproved && !cancelled && isHasUpdate;
                        //5/22/09 Israel - Only show Print button when there is a contract to print.
                        barbtnRqmtPrint.Enabled = IsContractOkToPrint(tradeId);

                        popupSempraRqmt.ShowPopup(gridRqmt.PointToScreen(e.Point));
                    }
                    else
                    {
                        if (isHasUpdate)
                        {
                            popupAllRqmts.ShowPopup(gridRqmt.PointToScreen(e.Point));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to enable/disable Requirement Grid menu items." + Environment.NewLine +
                       "Error CNF-107 in " + FORM_NAME + ".gridViewRqmt_ShowGridMenu(): " + ex.Message,
                     MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridViewConfirm_ShowGridMenu(object sender, GridMenuEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                if (view.RowCount == 0)
                    return;
                int rqmtId = int.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "RqmtId"));
                string faxTelexNumber = view.GetRowCellDisplayText(view.FocusedRowHandle, "FaxTelexNumber");
                string status = "";
                string confirmLabel = view.GetRowCellDisplayText(view.FocusedRowHandle, "ConfirmLabel");
                string activeFlag = view.GetRowCellDisplayText(view.FocusedRowHandle, "ActiveFlag");
                bool activeConfirm = (activeFlag == "Y");
                string finalApprovalFlag = gridViewSummary.GetRowCellDisplayText(gridViewSummary.FocusedRowHandle, "FinalApprovalFlag");
                bool finalApproved = (finalApprovalFlag == "Y");

                if (confirmLabel == CONFIRM_LABEL_CONFIRM)
                    status = GetTradeRqmtData(rqmtId, "Status");
                else
                    status = view.GetRowCellValue(view.FocusedRowHandle, "NextStatusCode").ToString();
                bool cancelled = status == "CXL";

                //No need to test for single row selected since that's controlled by the grid property.
                if ((e.Menu == null || e.Menu.GetType() == typeof(DevExpress.XtraGrid.Menu.GridViewMenu)))
                {
                    bool faxTelexOk = faxTelexNumber.Trim().Length > 2;
                    bool enableSend = (status == "OK_TO_SEND") && !finalApproved && !cancelled && isHasUpdate;
                    bool enableResend = ((status == "SENT") ||
                                         (status == "FAIL") ||
                                         (status == "DISP") ||
                                         (status == "APPR") ||
                                         (status == "SIGNED") ||
                                         (status == "ACPTD") ||
                                         (status == "RESNT")) && !finalApproved && !cancelled && isHasUpdate;

                    barBtnEditConfirm.Enabled = !cancelled && !finalApproved && isHasUpdate;

                    barBtnConfirmSendPaper.Enabled = enableSend && faxTelexOk && activeConfirm;
                    barBtnConfirmResendPaper.Enabled = enableResend && faxTelexOk && activeConfirm;
                    barBtnConfirmChangeFaxNoSendResend.Enabled = (enableSend || enableResend) && activeConfirm;
                    //6/15/09 Israel -- added faxTelexOk test
                    barBtnConfirmSendResendRTF.Enabled = (enableSend || enableResend) && faxTelexOk;
                    barbtnConfirmManualSend.Enabled = (status == "OK_TO_SEND" || status == "FAIL") &&
                                                       !cancelled && !finalApproved && isHasUpdate && activeConfirm;

                    barBtnCreateAdditionalConfirm2.Enabled = gridViewConfirm.RowCount > 0 && !finalApproved && isHasUpdate;

                    if (confirmLabel == CONFIRM_LABEL_CONFIRM)
                        barBtnCancelAdditonalConfirm.Visibility = BarItemVisibility.Never;
                    else
                    {
                        barBtnCancelAdditonalConfirm.Visibility = BarItemVisibility.Always;
                        barBtnCancelAdditonalConfirm.Enabled = ((status != "SENT") && (status != "CXL")) && !finalApproved && isHasUpdate;
                    }

                    barbtnConfirmPrint.Enabled = !cancelled;

                    popupConfirm.ShowPopup(gridConfirm.PointToScreen(e.Point));
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to enable/disable Confirm Grid menu items." + Environment.NewLine +
                      "Error CNF-108 in " + FORM_NAME + ".gridViewConfirm_ShowGridMenu(): " + ex.Message,
                    MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridViewRqmt_DoubleClick(object sender, EventArgs e)
        {
            string rqmtCode = gridViewRqmt.GetRowCellDisplayText(gridViewRqmt.FocusedRowHandle, "Rqmt");
            if (isHasUpdate)
            {
                if (rqmtCode == SEMPRA_RQMT)
                {
                    GetAutoContract(gridViewRqmt);
                }
                else
                {
                    barbtnEditRqmt_ItemClick(null, null);
                }
            }
        }

        private void gridViewConfirm_DoubleClick(object sender, EventArgs e)
        {
            GridView view = sender as GridView;
            string confirmLabel = view.GetRowCellDisplayText(view.FocusedRowHandle, "ConfirmLabel");
            string status = "";
            if (isHasUpdate)
            {
                if (confirmLabel == CONFIRM_LABEL_CONFIRM)
                {
                    int rqmtId = int.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "RqmtId"));
                    status = GetTradeRqmtData(rqmtId, "Status");
                }
                else
                    status = view.GetRowCellValue(view.FocusedRowHandle, "NextStatusCode").ToString();

                //Israel 11/06/2015 -- Prevent access when there may not be a contract to view, preventing error
                //if (view.RowCount > 0 && status != "CXL")
                if ((view.RowCount > 0) &&
                    (status != "CXL") &&
                    (status != "NEW"))
                    GetAutoContract(view);
            }

        }

        #endregion

        #region WebBrowser Handlers

        private void GoToItem(WebBrowser ABrowser, string AAddress)
        {
            if (ABrowser == null) return;
            if (AAddress == null) return;

            //if (currentAddress != AAddress)
            {
                barEditBrowserAddress.EditValue = AAddress;
                ABrowser.Navigate(AAddress);
            }
        }

        bool CorrectAddress(string name)
        {
            string[] names = new string[] { "javascript:" };
            foreach (string s in names)
                if (name.IndexOf(s) == 0) return false;
            return true;
        }

        private void BrowserBtn_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                WebBrowser browser = GetCurrentVisibleBrowser();
                switch (e.Item.Name)
                {
                    case "barlgbtnBack": browser.GoBack(); break;
                    case "barlgbtnForward": browser.GoForward(); break;
                    case "barlgbtnStop": browser.Stop(); break;
                    case "barlgbtnRefresh": browser.Refresh(); break;
                    case "barlgbtnHome":
                        string address = GetDefaultWebAddress();
                        //browser.GoHome(); 
                        if (address != "none")
                            GoToItem(browser, address);
                        break;
                    case "barlgbtnSearch": browser.GoSearch(); break;
                    case "barBtnGo":
                        GoToItem(browser, barEditBrowserAddress.EditValue.ToString());
                        break;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to enable/disable internal browser navigation buttons." + Environment.NewLine +
                      "Error CNF-109 in " + FORM_NAME + ".BrowserBtn_ItemClick(): " + ex.Message,
                    MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private WebBrowser GetCurrentVisibleBrowser()
        {
            try
            {
                WebBrowser browser = null;
                switch (tabctrlBrowserApps.SelectedTabPage.Tag.ToString())
                {
                    //case "DS": browser = webbrowserDealsheet; break;
                    case "CNTRCT": browser = null; break;
                    case "FAXGATE": browser = webbrowserFaxGateway; break;
                    //case "ECM": browser = webbrowserEcm; break;
                    //case "OCC": browser = webbrowserOcc; break;
                    //case "ATC": browser = webbrowserAtc; break;
                    //case "EPM": browser = webbrowserEpm; break;
                    //case "RPT": browser = webbrowserReports; break;
                    //case "HELP": browser = webbrowserHelp; break;
                }
                return browser;
            }
            catch (Exception excep)
            {
                throw new Exception("An error occurred while attempting to retrieve the currently selected internal browser page." + Environment.NewLine +
                     "Error CNF-110 in " + FORM_NAME + ".GetCurrentVisibleBrowser(): " + excep.Message);
            }
        }

        private string GetDefaultWebAddress()
        {
            try
            {
                string address = "";
                switch (tabctrlBrowserApps.SelectedTabPage.Tag.ToString())
                {
                    case "DS": address = "none"; break;
                    case "CNTRCT": address = "none"; break;
                    case "FAXGATE": address = Properties.Settings.Default.TransmissionGatewayWebAppUrl; break;
                    //case "ECM": address = Properties.Settings.Default.EcmUrl; break;
                    //case "OCC":
                    //    address = Properties.Settings.Default.TradeCorrectionUrlOcc;
                    //    address += "&UID=" + toolbarOrWindowsUserId.ToLower();
                    //    //address += "&UID=" + "ictspass";
                    //    GridView view = gridViewSummary;
                    //    string trdSysCode = view.GetRowCellDisplayText(view.FocusedRowHandle, "TrdSysCode").ToString();
                    //    if (trdSysCode == "JMS" && view.SelectedRowsCount == 1)
                    //        address += "&tradenum=" +
                    //           view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeId").ToString();
                    //    break;
                    //case "ATC": address = Properties.Settings.Default.TradeCorrectionUrlAtc; break;
                    //case "EPM": address = Properties.Settings.Default.EpmUrl; break;
                    //case "RPT": address = reportsURL[cbeditBrowserReports.SelectedIndex]; break;
                    //case "HELP": address = Properties.Settings.Default.HelpUrl; break;
                }
                return address;
            }
            catch (Exception excep)
            {
                throw new Exception("An error occurred while attempting to retrieve the url for the currently selected internal browser." + Environment.NewLine +
                     "Error CNF-111 in " + FORM_NAME + ".GetDefaultWebAddress(): " + excep.Message);
            }
        }

        private void barlgbtnPrint_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                //System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
                //todo create pd   
                WebBrowser browser = GetCurrentVisibleBrowser();
                PrintDialog dlg = new PrintDialog();
                //dlg.Document = pd;
                //6/9/2009 Israel -- Fixed null exception from the contract viewer.
                if (browser == null)
                    return;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    //pd.Print();
                    browser.Print();
                }
            }
            catch { }
        }

        private void WebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            string s = e.Url.AbsoluteUri;
            //if (barManager1.ActiveEditor != null)
            // barManager1.ActiveEditItemLink.CloseEditor();
            if (CorrectAddress(s))
            {
                barEditBrowserAddress.EditValue = s;
                //currentAddress = s;
                //AddNewItem(s);
            }
        }

        private void WebBrowser_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            barEditBrowserProgressRepository.Maximum = (int)(e.MaximumProgress + (e.MaximumProgress == barEditBrowserProgressRepository.Minimum ? 1 : 0));
            barEditBrowserProgress.EditValue = e.CurrentProgress;
        }

        private void WebBrowser_StatusTextChanged(object sender, EventArgs e)
        {
            try
            {
                WebBrowser browser = GetCurrentVisibleBrowser();
                //6/9/2009 Israel -- Fixed null exception from the contract viewer.
                if (browser != null)
                    barStaticBrowserText.Caption = browser.StatusText;
            }
            catch (Exception ex)
            {
                //Israel 3/16/2015 -- Was throwing exceptions on application/form close.
                //XtraMessageBox.Show("WebBrowser_StatusTextChanged: " + ex.Message,
                // "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WebBrowser_CanGoForwardChanged(object sender, EventArgs e)
        {
            try
            {
                WebBrowser browser = GetCurrentVisibleBrowser();
                //6/9/2009 Israel -- Fixed null exception from the contract viewer.
                if (browser != null)
                    barlgbtnForward.Enabled = browser.CanGoForward;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to enable/disable an internal browser navigation button." + Environment.NewLine +
                      "Error CNF-112 in " + FORM_NAME + ".WebBrowser_CanGoForwardChanged(): " + ex.Message,
                    MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WebBrowser_CanGoBackChanged(object sender, EventArgs e)
        {
            try
            {
                WebBrowser browser = GetCurrentVisibleBrowser();
                //6/9/2009 Israel -- Fixed null exception from the contract viewer.
                if (browser != null)
                    barlgbtnBack.Enabled = browser.CanGoBack;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to enable/disable an internal browser navigation button." + Environment.NewLine +
                      "Error CNF-113 in " + FORM_NAME + ".WebBrowser_CanGoBackChanged(): " + ex.Message,
                    MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tabctrlBrowserApps_SelectedPageChanged(object sender,
           DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            string address = "";
            bool isBrowserActive = (e.Page != tabpgContract);
            SetBrowserButtonsEnable(true);
            barDockControl3.Visible = isBrowserActive;
            barBrowserStatusBar.Visible = isBrowserActive;
            if (e.Page == tabpgHelp)
            {
                address = GetDefaultWebAddress();
                //null test makes sure only loads once.
                if (address != "none" && webbrowserHelp.Document.Body.InnerHtml == null)
                    GoToItem(GetCurrentVisibleBrowser(), address);
            }
            else if (e.Page == tabpgDealsheet)
            {
                barDockControl3.Visible = false;
                barBrowserStatusBar.Visible = false;
            }            //This code seems redundant, but without it the browser bar still appears the first time
            //you access the contract panel.
            else if (e.Page == tabpgContract)
            {
                barDockControl3.Visible = false;
                barBrowserStatusBar.Visible = false;
            }
            else if (e.Page == tabpgOCC)
            {
                SetBrowserButtonsEnable(false);
            }
            else if (e.Page == tabpgFaxGateway ||
                e.Page == tabpgReports ||
                e.Page == tabpgECM ||
                e.Page == tabpgEPM ||
                e.Page == tabpgATC)
            {
                address = GetDefaultWebAddress();
                WebBrowser browser = GetCurrentVisibleBrowser();
                if (address != "none" && browser.Document.Body.InnerHtml == null)
                    GoToItem(browser, address);
            }
        }

        private void SetBrowserButtonsEnable(bool AEnabled)
        {
            try
            {
                barlgbtnBack.Enabled = AEnabled;
                barlgbtnForward.Enabled = AEnabled;
                barlgbtnHome.Enabled = AEnabled;
                barlgbtnPrint.Enabled = AEnabled;
                barlgbtnRefresh.Enabled = AEnabled;
                barlgbtnSearch.Enabled = AEnabled;
                barlgbtnStop.Enabled = AEnabled;
                barBtnGo.Enabled = AEnabled;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to enable/disable internal browser navigation buttons." + Environment.NewLine +
                     "Error CNF-114 in " + FORM_NAME + ".SetBrowserButtonsEnable(): " + ex.Message);
            }
        }

        private void cbeditBrowserReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string address = GetDefaultWebAddress();
                if (address != "none")
                    GoToItem(GetCurrentVisibleBrowser(), address);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to access the default url for the currently selected internal browser." + Environment.NewLine +
                     "Error CNF-115 in " + FORM_NAME + ".cbeditBrowserReports_SelectedIndexChanged(): " + ex.Message);
            }
        }

        #endregion WebBrowser Handlers

        #region Merge and Red X issue

        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            //5/20/09 Israel -- Added to keep screen from refreshing while doing a multi-select
            if (gridViewSummary.SelectedRowsCount < 2)
            {
                UpdateDataFromCache();
                ApplyInboundUpdates();
            }
        }

        private void UpdateDataFromCache()
        {
            DateTime startTime = DateTime.Now;
            try
            {
                if (!backgroundWorkerDataUpdate.IsBusy)
                {
                    barBtnApplyCache.Enabled = false;
                    dataManager.PrepareOpsTrackingUpdates();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to update grid with update message data." + Environment.NewLine +
                      "Error CNF-116 in " + FORM_NAME + ".UpdateDataFromCache(): " + ex.Message,
                    MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DateTime endTime = DateTime.Now;
                TimeSpan elapsed = endTime - startTime;
            }

            DataRow rowSumData = null;
            DataRow rowRqmtData = null;

            SummaryData currentFocusedSumRow = null;
            RqmtData currentFocusedRqmtRow = null;
            try
            {
                IList<SummaryData> summaryDataCache = new List<SummaryData>();
                summaryDataCache = dataManager.GetSummaryDataCache();

                IList<RqmtData> rqmtDataCache = new List<RqmtData>();
                rqmtDataCache = dataManager.GetRqmtDataCache();

                IList<TradeRqmtConfirm> tradeRqmtConfirmCache = new List<TradeRqmtConfirm>();
                tradeRqmtConfirmCache = dataManager.GetTradeRqmtConfirmCache();

                gridViewSummary.BeginDataUpdate();
                gridViewRqmt.BeginDataUpdate();
                gridViewConfirm.BeginDataUpdate();

                if (gridViewSummary.IsValidRowHandle(gridViewSummary.FocusedRowHandle))
                {
                    rowSumData = gridViewSummary.GetDataRow(gridViewSummary.FocusedRowHandle);
                    currentFocusedSumRow = CollectionHelper.CreateObjectFromDataRow<SummaryData>(rowSumData);
                }

                if (gridViewRqmt.IsValidRowHandle(gridViewRqmt.FocusedRowHandle))
                {
                    rowRqmtData = gridViewRqmt.GetDataRow(gridViewRqmt.FocusedRowHandle);
                    currentFocusedRqmtRow = CollectionHelper.CreateObjectFromDataRow<RqmtData>(rowRqmtData);
                }


                UpdateRqmtGrid(rqmtDataCache);
                UpdateConfirmGrid(tradeRqmtConfirmCache);
                UpdateTradeSummaryGrid(summaryDataCache);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to update grid with update message data." + Environment.NewLine +
                      "Error CNF-117 in " + FORM_NAME + ".UpdateDataFromCache(): " + ex.Message,
                    MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                int rowHandle = 0;
                int rqmtRowHandle = 0;
                SummaryData sumData = null;
                RqmtData rqmtData = null;

                try
                {
                    sumData = (SummaryData)currentFocusedSumRow;
                    if (sumData != null)
                    {
                        gridViewSummary.ClearSelection();
                        gridViewSummary_FocusedRowChanged(gridViewSummary,
                           new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(0, DevExpress.XtraGrid.GridControl.AutoFilterRowHandle));

                        rowHandle = gridViewSummary.LocateByValue(0, gridViewSummary.Columns["TradeId"], sumData.TradeId);

                        if (rowHandle < 0)
                        {
                            rowHandle = 0;
                        }
                    }

                    gridViewSummary_FocusedRowChanged(gridViewSummary,
                       new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(0, rowHandle));
                    gridViewSummary.SelectRow(rowHandle);
                    gridViewSummary.FocusedRowHandle = rowHandle;

                    rqmtData = (RqmtData)currentFocusedRqmtRow;
                    if (rqmtData != null)
                    {
                        gridViewRqmt.ClearSelection();
                        gridViewRqmt_FocusedRowChanged(gridViewRqmt,
                           new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(0, DevExpress.XtraGrid.GridControl.AutoFilterRowHandle));

                        rqmtRowHandle = gridViewRqmt.LocateByValue(0, gridViewRqmt.Columns["Id"], rqmtData.Id);

                        if (rqmtRowHandle < 0)
                        {
                            rqmtRowHandle = 0;
                        }
                    }
                    gridViewRqmt_FocusedRowChanged(gridViewRqmt,
                       new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(0, rqmtRowHandle));
                    gridViewRqmt.SelectRow(rqmtRowHandle);
                    gridViewRqmt.FocusedRowHandle = rqmtRowHandle;
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("An error occurred while attempting to update grid with update message data." + Environment.NewLine +
                          "Error CNF-118 in " + FORM_NAME + ".UpdateDataFromCache(): " + ex.Message,
                        MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    gridViewRqmt.EndDataUpdate();
                    gridViewConfirm.EndDataUpdate();
                    gridViewSummary.EndDataUpdate();

                    barBtnApplyCache.Enabled = true;
                }
            }
        }

        private void backgroundWorkerDataUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
            DataRow dr = null;
            SummaryData lastSelected = null;
            try
            {
                myTimer.Stop();

                IList<SummaryData> summaryDataCache = new List<SummaryData>();
                summaryDataCache = dataManager.GetSummaryDataCache();

                IList<RqmtData> rqmtDataCache = new List<RqmtData>();
                rqmtDataCache = dataManager.GetRqmtDataCache();

                IList<TradeRqmtConfirm> tradeRqmtConfirmCache = new List<TradeRqmtConfirm>();
                tradeRqmtConfirmCache = dataManager.GetTradeRqmtConfirmCache();


                gridViewSummary.BeginDataUpdate();
                gridViewRqmt.BeginDataUpdate();
                gridViewConfirm.BeginDataUpdate();

                if (gridViewSummary.IsValidRowHandle(gridViewSummary.FocusedRowHandle))
                {
                    dr = gridViewSummary.GetDataRow(gridViewSummary.FocusedRowHandle);
                    lastSelected = CollectionHelper.CreateObjectFromDataRow<SummaryData>(dr);
                }

                UpdateRqmtGrid(rqmtDataCache);
                UpdateConfirmGrid(tradeRqmtConfirmCache);
                UpdateTradeSummaryGrid(summaryDataCache);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to update grid with update message data." + Environment.NewLine +
                      "Error CNF-119 in " + FORM_NAME + ".backgroundWorkerDataUpdate_DoWork(): " + ex.Message,
                    MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                e.Result = lastSelected;
            }
        }

        private void ApplyInboundUpdates()
        {
            inboundPnl1.BeginGridDataUpdates();
        }

        private void UpdateTradeSummaryGrid(IList<SummaryData> summaryDataCache)
        {
            System.Data.DataTable tb = dataSet.Tables["SummaryData"];
            foreach (SummaryData sumData in summaryDataCache)
            {
                DataRow rowFound = tb.Rows.Find(sumData.Id);
                if (rowFound != null)
                {
                    string qryCode = (string)rowFound["QryCode"];
                    CollectionHelper.UpdateDataRowFromObject<SummaryData>(sumData, ref rowFound);
                    if (!((qryCode == null) || (qryCode == "")))
                    {
                        rowFound["QryCode"] = qryCode; // so as to not overwrite any "Get All" results.
                    }
                }
                else
                {
                    DataRow newRow = tb.NewRow();
                    newRow = CollectionHelper.CreateDataRowFromObject<SummaryData>(sumData, newRow);
                    tb.Rows.Add(newRow);
                }
            }
        }

        private void UpdateRqmtGrid(IList<RqmtData> rqmtDataCache)
        {
            System.Data.DataTable tb = dataSet.Tables["RqmtData"];
            foreach (RqmtData rqmtData in rqmtDataCache)
            {
                DataRow rowFound = tb.Rows.Find(rqmtData.Id);
                if (rowFound != null)
                {
                    CollectionHelper.UpdateDataRowFromObject<RqmtData>(rqmtData, ref rowFound);
                }
                else
                {
                    DataRow newRow = tb.NewRow();
                    newRow = CollectionHelper.CreateDataRowFromObject<RqmtData>(rqmtData, newRow);
                    tb.Rows.Add(newRow);
                }
            }
        }

        private void UpdateConfirmGrid(IList<TradeRqmtConfirm> tradeRqmtConfirmCache)
        {
            System.Data.DataTable tb = dataSet.Tables["TradeRqmtConfirm"];
            foreach (TradeRqmtConfirm data in tradeRqmtConfirmCache)
            {
                DataRow rowFound = tb.Rows.Find(data.Id);
                if (rowFound != null)
                {
                    CollectionHelper.UpdateDataRowFromObject<TradeRqmtConfirm>(data, ref rowFound);
                }
                else
                {
                    DataRow newRow = tb.NewRow();
                    newRow = CollectionHelper.CreateDataRowFromObject<TradeRqmtConfirm>(data, newRow);
                    tb.Rows.Add(newRow);
                }
            }
        }

        private void backgroundWorkerDataUpdate_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorkerDataUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            int rowHandle = invalidFocusedRowHandle;
            SummaryData sumData = null;
            try
            {
                sumData = (SummaryData)e.Result;
                if (sumData != null)
                {

                    gridViewSummary.ClearSelection();
                    rowHandle = gridViewSummary.LocateByValue(0, gridViewSummary.Columns["TradeId"], sumData.TradeId);
                    if (rowHandle != invalidFocusedRowHandle)
                    {
                        gridViewSummary.FocusedRowHandle = rowHandle;
                        gridViewSummary.SelectRow(rowHandle);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred after grid was updated with message data." + Environment.NewLine +
                      "Error CNF-120 in " + FORM_NAME + ".backgroundWorkerDataUpdate_RunWorkerCompleted(): " + ex.Message,
                    MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                gridViewRqmt.EndDataUpdate();
                gridViewConfirm.EndDataUpdate();
                gridViewSummary.EndDataUpdate();

                myTimer.Start();
                barBtnApplyCache.Enabled = true;
            }
        }

        #endregion

        #region Confirmation Methods

        private string GetHTTPFieldValue(string AFieldName, string AFullResponse)
        {
            try
            {
                string fieldValue = "";
                int valStart = AFullResponse.IndexOf(AFieldName) + AFieldName.Length + 1;
                int valEnd = AFullResponse.IndexOf(HTTP_DELIM, valStart);

                if (AFieldName == "CONTRACT_BODY")
                    fieldValue = AFullResponse.Substring(valStart, AFullResponse.Length - valStart);
                else
                    fieldValue = AFullResponse.Substring(valStart, valEnd - valStart);

                return fieldValue;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to retrieve a data field from the Contract Server." + Environment.NewLine +
                     "Error CNF-121 in " + FORM_NAME + ".GetHTTPFieldValue(): " + ex.Message);
            }
        }

        private void GetManualContract(bool ACreateAdditional)
        {
            try
            {
                string getDocumentUrl = Properties.Settings.Default.ExtSvcAPIBaseUrl;
                string svcUserName = Properties.Settings.Default.ExtSvcAPIUserName;
                string svcPassword = Properties.Settings.Default.ExtSvcAPIPassword;
                ConfirmDocsAPIDal confirmDocsDal = ConfirmDocsAPIDal.Instance(getDocumentUrl, svcUserName, svcPassword);

                string tradingSysCode = "";
                GridView view = gridViewSummary;
                int rowHandle = GetGridViewFocusedRowHandle(view);
                tradingSysCode = view.GetRowCellDisplayText(rowHandle, "TrdSysCode").ToString();
                bool fromCache = false;
                CGMethod cgMethod;
                if (!templateListForm.isDataBeenLoaded)
                {
                    templateListForm.RefreshFired += (sender, e) =>
                    {
                        //refresing the data from service on user required.
                        string xmlDataRefresh = confirmDocsDal.GetTemplateList(tradingSysCode, ref fromCache, dataDir, true);
                        templateListForm.LoadDataFromXml(xmlDataRefresh, fromCache);
                    };
                }
                //Israel get from xml read
                //cgMethod = CGMethod.GetTemplateList;
                //string responseText = GetContractGeneratorResponse(cgMethod, "", "", "", "");

                //// 7/29/14 Israel -- clean up junk from display list.
                //responseText = responseText.Replace("TEMPLATE_LIST=", "");
                //responseText = responseText.Replace("|", "");

                //templateListForm.LoadData(responseText);
                //string xmlData = confirmDocsDal.GetStubTemplateList();  
                string xmlData = confirmDocsDal.GetTemplateList(tradingSysCode, ref fromCache, dataDir, false);
                templateListForm.LoadDataFromXml(xmlData, fromCache);
                //  }                


                //Filter by group=REPLY for non-contract docs
                //5/28/09 Israel - Remove REPLY-only filter for reply docs.
                //templateListForm.SetGroupFilter(false);
                DialogResult result = templateListForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string tradingSysTicket = "";
                    string tradeIdStr = "";
                    string cptySn = "";
                    string cdtyCode = "";
                    string sttlType = "";
                    DateTime dtTradeDt;
                    string contractType = "NEW";
                    string templateName = templateListForm.selectedTemplate;

                    if (templateName == "<DEFAULT>")
                        cgMethod = CGMethod.GetContract;
                    else
                        cgMethod = CGMethod.GetFilledTemplate;

                    tradingSysTicket = view.GetRowCellDisplayText(rowHandle, "TradeSysTicket").ToString();
                    tradeIdStr = view.GetRowCellDisplayText(rowHandle, "TradeId").ToString();
                    cptySn = view.GetRowCellDisplayText(rowHandle, "CptySn").ToString();
                    dtTradeDt = (DateTime)view.GetRowCellValue(rowHandle, "TradeDt");
                    cdtyCode = view.GetRowCellDisplayText(rowHandle, "CdtyCode").ToString();
                    sttlType = view.GetRowCellDisplayText(rowHandle, "SttlType").ToString();
                    long tradeId = long.Parse(tradeIdStr);
                    int confirmId = 0;

                    //When creating a manual contract, there may or may not be a row already there
                    //There will be a row if the status was reset.
                    //Always force creation when invoking create additional
                    if (!ACreateAdditional)
                        confirmId = GetContractRowId(tradeId);
                    string tradeDt = String.Format("{0:MM/dd/yyyy}", dtTradeDt);

                    //string responseText = GetContractGeneratorResponse(cgMethod, tradingSystem, ticketNo,
                    //                        contractType, templateName);
                    //string contractBody = GetHTTPFieldValue("CONTRACT_BODY", responseText);
                    //long templateId = long.Parse(GetHTTPFieldValue("TEMPLATE_ID", responseText));
                    //long templateId = 0;

                    byte[] contractBody = null;
                    string docFileExt = "";
                    contractBody = confirmDocsDal.GetConfirm(tradingSysCode, tradingSysTicket, templateName, out docFileExt);
                    DocumentFormat docFormat = Utils.GetDocumentFormat(docFileExt);

                    //12/09/2015 Israel -- Allow empty body contents but show a warning.
                    if (contractBody == null || contractBody.Length < 2)
                    {
                        if (editContractForm.richeditConfirm.Document.Length > 0)
                        {
                            DocumentRange docRange = editContractForm.richeditConfirm.Document.CreateRange(0, editContractForm.richeditConfirm.Document.Length);
                            editContractForm.richeditConfirm.Document.Delete(docRange);
                        }
                        MessageBox.Show("The Confirmation Web Service call returned a blank document.", "Empty Confirm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    //confirmId > 0 handles when status is reset to NEW.
                    //docFormat = DocumentFormat.OpenXml;
                    CallEditContractForm(contractBody, docFormat, templateName, "FAX", "", confirmId, "", "PREP", "Y","N");
                }
            }
            catch (Exception ex)
            {
                //string message = "An error occurred while retrieving and filling a confirm template from the server." + Environment.NewLine +
                //      "Error CNF-122 in GetManualContract(): " + ex.Message;
                //string caption = MAIN_FORM_ERROR_CAPTION;
                //MessageBoxIcon icon = MessageBoxIcon.Error;
                //if (ex.Message.IndexOf("Contract Generator is not available") > -1)
                //{
                //    message = "Contract Generator is not available. Please contact ConfirmSupport." + Environment.NewLine;
                //    message += "When it is available just try again-- no need to restart " + APP_NAME + ".";
                //    caption = "Contract Generator Unavailable";
                //    icon = MessageBoxIcon.Exclamation;
                //}
                //else if (ex.Message.IndexOf("Contract not filled") > -1)
                //{
                //    //message = "Contract data was not found. The trade may not be available for processing." + Environment.NewLine;
                //    message = "There was a problem filling the contract template with trade data." + Environment.NewLine;
                //    message += "Please contact ConfirmSupport or your Operations business analyst." + Environment.NewLine;
                //    message += "When it is available just try again-- no need to restart " + APP_NAME + ".";
                //    caption = "Contract Template Not Filled";
                //    icon = MessageBoxIcon.Exclamation;
                //}
                //XtraMessageBox.Show(message, caption, MessageBoxButtons.OK, icon);
                XtraMessageBox.Show("An error occurred while requesting a Confirm from the GetConfirmations Web Service." + Environment.NewLine +
                     "Error CNF-312 in " + FORM_NAME + ".GetManualContract(): " + ex.Message,
                   MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetAutoContract(GridView AGridView)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                CGMethod cgMethod = CGMethod.GetContract;
                string tradingSystem = "";
                string trdSysTicket = "";
                string tradeIdStr = "";
                string rqmtId = "";
                string contractType = "NEW";
                //string contractBody = "";
                byte[] contractBody = null;
                string templateName = "";
                string preparerCanSendFlag = "";
                string savedTransMethod = "FAX";
                string savedTransInd = "F";
                string savedFaxNumber = "";
                string cmt = "";
                string nextStatusCode = "";
                string statusCode = "";
                string activeFlag = "Y";
                //Int32 templateId = 0;
                Int32 rqmtConfirmId = 0;
                Int32 tradeId = 0;

                int summaryRowHandle = GetGridViewFocusedRowHandle(gridViewSummary);
                tradingSystem = gridViewSummary.GetRowCellDisplayText(summaryRowHandle, "TrdSysCode").ToString();
                trdSysTicket = gridViewSummary.GetRowCellDisplayText(summaryRowHandle, "TradeSysTicket").ToString();
                tradeIdStr = gridViewSummary.GetRowCellDisplayText(summaryRowHandle, "TradeId").ToString();
                tradeId = Int32.Parse(tradeIdStr);
                //int tradeIdInt = int.Parse(ticketNo);

                //If adding a new confirm row, this will get the correct status. If the focus is on the confirm
                //row and the wrong rqmt row is focused, this bogus status will get replaced by the correct one below.
                statusCode = gridViewRqmt.GetRowCellDisplayText(gridViewRqmt.FocusedRowHandle, "Status").ToString();

                //If only one row they could be accessing it from the rqmt gid
                //If more then one access is only from the confirm grid so we know the focus.
                int rowHandle = 0;
                rowHandle = AGridView.FocusedRowHandle;

                //if (gridViewConfirm.RowCount == 1)
                //   rowHandle = gridViewConfirm.GetVisibleRowHandle(0);
                //else
                //   rowHandle = gridViewConfirm.FocusedRowHandle;

                //if (AGridView.Name == gridViewRqmt.Name)
                if (gridViewConfirm.RowCount > 0)
                {
                    rqmtId = gridViewRqmt.GetRowCellDisplayText(rowHandle, "Id").ToString();
                    rqmtConfirmId = GetContractRowId(tradeId);
                    savedTransInd = GetContractConfirmData(tradeId, "FaxTelexInd");
                    savedFaxNumber = GetContractConfirmData(tradeId, "FaxTelexNumber");
                    savedTransMethod = GetFaxMethodFromInd(savedTransInd);
                    templateName = GetContractConfirmData(tradeId, "TemplateName");
                    preparerCanSendFlag = GetContractConfirmData(tradeId, "PreparerCanSendFlag");
                    //templateId = GetContractTemplateId(tradeId);
                    cmt = GetContractConfirmData(tradeId, "ConfirmCmt");
                    nextStatusCode = GetContractConfirmData(tradeId, "NextStatusCode");
                    statusCode = gridViewRqmt.GetRowCellDisplayText(rowHandle, "Status").ToString();
                }
                else if (gridViewConfirm.RowCount > 0)
                {
                    rqmtId = gridViewConfirm.GetRowCellDisplayText(rowHandle, "RqmtId").ToString();
                    rqmtConfirmId = Int32.Parse(gridViewConfirm.GetRowCellDisplayText(rowHandle, "Id").ToString());
                    savedTransInd = gridViewConfirm.GetRowCellDisplayText(rowHandle, "FaxTelexInd").ToString();
                    savedFaxNumber = gridViewConfirm.GetRowCellDisplayText(rowHandle, "FaxTelexNumber").ToString();
                    savedTransMethod = GetFaxMethodFromInd(savedTransInd);
                    templateName = gridViewConfirm.GetRowCellDisplayText(rowHandle, "TemplateName").ToString();
                    preparerCanSendFlag = GetContractConfirmData(tradeId, "PreparerCanSendFlag");
                    //templateId = Int32.Parse(gridViewConfirm.GetRowCellDisplayText(rowHandle, "TemplateId").ToString());
                    cmt = gridViewConfirm.GetRowCellDisplayText(rowHandle, "ConfirmCmt").ToString();
                    nextStatusCode = gridViewConfirm.GetRowCellValue(rowHandle, "NextStatusCode").ToString();

                    int iRqmtId = int.Parse(rqmtId);
                    statusCode = GetTradeRqmtData(iRqmtId, "Status");
                }

                bool okToCallEditor = false;
                //bool contractExists = rqmtConfirmId > 0;

                string getDocumentUrl = Properties.Settings.Default.ExtSvcAPIBaseUrl;
                string svcUserName = Properties.Settings.Default.ExtSvcAPIUserName;
                string svcPassword = Properties.Settings.Default.ExtSvcAPIPassword;
                ConfirmDocsAPIDal confirmDocsDal = ConfirmDocsAPIDal.Instance(getDocumentUrl, svcUserName, svcPassword);

                string docFileExt = "";
                DocumentFormat docFormat = DocumentFormat.Undefined;
                if (statusCode == "NEW")
                {
                    try
                    {
                        //Israel 12/09/2015 -- Provide cancel when no template given.
                        bool okToCallServer = true;
                        if (String.IsNullOrEmpty(templateName))
                        {
                            DialogResult result = XtraMessageBox.Show("No template has been supplied for this trade. Continue with the request?" + Environment.NewLine +
                                "(If you choose 'No' you can retry by selecting a Manual Contract instead.)", "Missing Template",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (result == DialogResult.No)
                                okToCallServer = false;
                        }

                        if (okToCallServer)
                        {
                            contractBody = confirmDocsDal.GetConfirm(tradingSystem, trdSysTicket, templateName, out docFileExt);
                            okToCallEditor = true;
                            if (contractBody == null || contractBody.Length < 2)
                            {
                                if (editContractForm.richeditConfirm.Document.Length > 0)
                                {
                                    DocumentRange docRange = editContractForm.richeditConfirm.Document.CreateRange(0, editContractForm.richeditConfirm.Document.Length);
                                    editContractForm.richeditConfirm.Document.Delete(docRange);
                                }
                                MessageBox.Show("The Confirmation Web Service call returned a blank document.", "Empty Confirm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        okToCallEditor = false;
                        //string message = "An error occurred while requesting a Confirm from the GetConfirmations Web Service." + Environment.NewLine +
                        //    "Error CNF-282 in GetAutoContract: " + ex.Message;
                        //string caption = MAIN_FORM_ERROR_CAPTION;
                        //MessageBoxIcon icon = MessageBoxIcon.Error;
                        //if (ex.Message.IndexOf("Confirm Web Service(s) not available") > -1)
                        //{
                        //    message = "A Confirm Web Service is not available. Please contact ConfirmSupport." + Environment.NewLine;
                        //    message += "When it is available just try again-- no need to restart " + APP_NAME + ".";
                        //    caption = "Confirm Web Service Unavailable";
                        //    icon = MessageBoxIcon.Exclamation;
                        //}
                        //XtraMessageBox.Show(message, caption, MessageBoxButtons.OK, icon);
                        XtraMessageBox.Show("An error occurred while requesting a Confirm from the GetConfirmations Web Service." + Environment.NewLine +
                              "Error CNF-282 in " + FORM_NAME + ".GetAutoContract(): " + ex.Message,
                            MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else //if (contractExists)
                {
                    //Retrieve contract body from vault.
                    //contractBody = GetContractFromVault(rqmtConfirmId, 0);
                    TradeRqmtConfirmBlobDto trcBlobData = new TradeRqmtConfirmBlobDto();
                    TradeRqmtConfirmBlobDal trcBlobDal = new TradeRqmtConfirmBlobDal(sqlConnectionStr);
                    trcBlobData = trcBlobDal.Get(rqmtConfirmId);
                    contractBody = trcBlobData.DocBlob;
                    docFileExt = trcBlobData.ImageFileExt;
                    //Israel 11/20/2015 -- Broken rows caused an error when bringing up the screen.
                    if (contractBody != null)
                        okToCallEditor = true;
                }
                //else
                //    XtraMessageBox.Show("Contract doesn't exist. Status must be NEW to add new contract");

                if (nextStatusCode == "")
                    nextStatusCode = "PREP";

                //Israel 11/20/2015 -- Broken rows caused an error when calling GetDocumentFormat.
                //docFormat = Utils.GetDocumentFormat(docFileExt);
                //docFormat = DocumentFormat.OpenXml;
                if (okToCallEditor)
                {
                    docFormat = Utils.GetDocumentFormat(docFileExt);
                    CallEditContractForm(contractBody, docFormat, templateName, savedTransMethod,
                       savedFaxNumber, rqmtConfirmId, cmt, nextStatusCode, activeFlag,preparerCanSendFlag);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while requesting a Confirm from the GetConfirmations Web Service." + Environment.NewLine +
                      "Error CNF-123 in " + FORM_NAME + ".GetAutoContract(): " + ex.Message,
                    MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void CallEditContractForm(byte[] AContractBody, DocumentFormat ADocFormat, string ATemplateName,
           string ASavedTransMethod, string ASavedFaxNumber, Int32 ARqmtConfirmId, string ACmt,
           string ANextStatusCode, string AActiveFlag, string APreparerCanSendFlag)
        {

            try
            {
                //Get summary grid data
                GridView view = gridViewSummary;
                int rowHandle = GetGridViewFocusedRowHandle(view);
                string tradingSystem = view.GetRowCellDisplayText(rowHandle, "TrdSysCode").ToString();
                Int32 tradeId = Int32.Parse(view.GetRowCellDisplayText(rowHandle, "TradeId"));
                string ticketNo = view.GetRowCellDisplayText(rowHandle, "TradeSysTicket").ToString();
                string cptySn = view.GetRowCellDisplayText(rowHandle, "CptySn").ToString();
                string bookingCoSn = view.GetRowCellDisplayText(rowHandle, "BookingCoSn").ToString();
                DateTime dtTradeDt = DateTime.MinValue;
                string faxEmailSendTo = "";
                string faxEmailTypeInd = "";
                try
                {
                    dtTradeDt = (DateTime)view.GetRowCellValue(rowHandle, "TradeDt");
                }
                catch (Exception excep)
                {
                }
                string cdtyCode = view.GetRowCellDisplayText(rowHandle, "CdtyCode").ToString();
                //string cdtyGroupCode = view.GetRowCellDisplayText(rowHandle, "CdtyGrpCode").ToString();
                string sttlType = view.GetRowCellDisplayText(rowHandle, "SttlType").ToString();
                string confirmLabel = "";
                bool insertNewPaper = false;

                //Get rqmt Id from table for existing ones, from row for new ones.
                int rqmtId = 0;
                string currentStatusCode = "";
                if (ARqmtConfirmId > 0)
                {
                    rqmtId = GetConfirmRqmtId(Convert.ToInt32(ARqmtConfirmId));
                    confirmLabel = GetConfirmData(Convert.ToInt32(ARqmtConfirmId), "ConfirmLabel");
                    if (confirmLabel == CONFIRM_LABEL_CONFIRM)
                    {
                        currentStatusCode = GetTradeRqmtData(rqmtId, "Status");
                        insertNewPaper = currentStatusCode == "NEW";
                    }
                    else
                        currentStatusCode = gridViewConfirm.GetRowCellValue(gridViewConfirm.FocusedRowHandle, "NextStatusCode").ToString();

                    faxEmailSendTo = gridViewConfirm.GetRowCellValue(gridViewConfirm.FocusedRowHandle, "FaxTelexNumber").ToString();
                    faxEmailTypeInd = gridViewConfirm.GetRowCellValue(gridViewConfirm.FocusedRowHandle, "FaxTelexInd").ToString();

                }
                else //ARqmtConfirmId == 0
                {
                    rqmtId = int.Parse(gridViewRqmt.GetRowCellDisplayText(gridViewRqmt.FocusedRowHandle, "Id"));
                    insertNewPaper = true;

                    //Set this only for the first one created, i.e., for the contract.
                    if (gridViewConfirm.RowCount == 0)
                    {
                        currentStatusCode = gridViewRqmt.GetRowCellDisplayText(gridViewRqmt.FocusedRowHandle, "Status");
                        confirmLabel = CONFIRM_LABEL_CONFIRM;
                    }
                    else
                    {
                        currentStatusCode = "PREP";
                    }
                }

                //Init EditContractForm with data
                //It must be completely destroyed and re-created each time or bad things happen.

                //Israel 9/2/2015 -- implementing RichEditControl
                //if (editContractForm == null)
                //    editContractForm = new frmEditContract();
                //editContractForm.marginToken = Properties.Settings.Default.MarginToken;
                editContractForm.marginToken = "[MARGIN TOKEN]";
                editContractForm.isContract = confirmLabel == CONFIRM_LABEL_CONFIRM;

                editContractForm.statusbarTemplateName.Caption = ATemplateName;
                editContractForm.Text = "Confirmation: " + tradingSystem + "-" + ticketNo;
                string tradeDt = String.Format("{0:MM/dd/yyyy}", dtTradeDt);
                editContractForm.lblAgreements.Text = CallGetAgreementInfo(cptySn, bookingCoSn, tradeDt);
                editContractForm.barEditWorkflowComments.EditValue = ACmt;
                editContractForm.rqmtStatusColorTable = this.rqmtStatusColorTable;

                //5/19/2009 Israel -- Add economic data to Contract Edit form
                editContractForm.lblEconData.Text = GetEconomicData();

                //5/19/2009 Israel -- Only allow print of existing contracts.
                editContractForm.barBtnWorkflowPrint.Enabled = ARqmtConfirmId > 0;

                //Israel 9/18/2015 
                //Get fax number data and send to form
                //string oldFaxNo = GetInfMgrFaxNo(cptySn);
                //string[] getCptyFaxNoResult = GetCptyFaxNo(cptySn, cdtyCode, sttlType);
                //if (getCptyFaxNoResult[0] != null)
                //    transMethod = getCptyFaxNoResult[0];
                //if (getCptyFaxNoResult[1] != null)
                //    newFaxNo = getCptyFaxNoResult[1];

                editContractForm.SetFaxNumbers(faxEmailSendTo, ASavedFaxNumber);
                editContractForm.settingsDir = this.appSettingsDir;

                //bool isFaxNumber = oldFaxNo.Trim().Length > 1 ||
                //                   newFaxNo.Trim().Length > 1 ||
                //                   ASavedFaxNumber.Trim().Length > 1;

                //Always true since currently fax number = email or fax-- it gets picked apart later.
                bool isFaxNumber = true;

                //Israel 9/16/2015 -- Everyone approves.
                bool isPaperCreator = false;
                frmEditRichContract.p_UserId = p_UserId;

                editContractForm.InitForm(AContractBody, ADocFormat, clauseHeaderTable, clauseBodyTable, tradingSystem,
                   currentStatusCode, isContractApprove, bookingCoSn, cptySn, isFaxNumber, APreparerCanSendFlag);

                if (currentStatusCode == "SENT")
                {
                    editContractForm.richeditConfirm.ReadOnly = true;
                    editContractForm.ChangeBarCommentItemsVisibility(false);
                }
                else
                {
                    editContractForm.richeditConfirm.ReadOnly = false;
                    editContractForm.ChangeBarCommentItemsVisibility(true);
                }

                string transMethodInd = "";
                string faxTelexNumber = "";
                if (editContractForm.ShowDialog(this) == DialogResult.OK)
                {
                    using (var ts = new TransactionScope())
                    {
                        //long tradeRqmtConfirmId = 0;
                        transMethodInd = editContractForm.liveTransMethod.Substring(0, 1);
                        faxTelexNumber = editContractForm.liveFaxNumber;
                        string cmt = editContractForm.barEditWorkflowComments.EditValue.ToString();
                        bool isMarginToken = editContractForm.isMarginToken;
                        string selectedStatusCode = editContractForm.barComboWorkflowStatus.EditValue.ToString();
                        string nextStatusCode = selectedStatusCode;

                        //Send to Credit status if unresolved margin token
                        if (selectedStatusCode != "PREP" && isMarginToken)
                            selectedStatusCode = "CRDT";

                        if (editContractForm.isApproveAndSend)
                        {
                            selectedStatusCode = "OK_TO_SEND";
                            if (!editContractForm.isContract)
                                nextStatusCode = selectedStatusCode;
                        }

                        //Insert/Update trade_rqmt_confirm row
                        long rqmtConfirmIdUpd = UpdateTradeRqmtConfirmRow(ARqmtConfirmId, tradeId, rqmtId, ATemplateName,
                                             transMethodInd, faxTelexNumber, confirmLabel, cmt, nextStatusCode, AActiveFlag);

                        //Update trade_rqmt 
                        if (currentStatusCode != selectedStatusCode &&
                            editContractForm.isContract)
                        {
                            string reference = GetTradeRqmtData(rqmtId, "Reference");
                            string rqmtCmt = GetTradeRqmtData(rqmtId, "Cmt");
                            CallUpdateTradeRqmts(tradeId, rqmtId, SEMPRA_RQMT, selectedStatusCode, DateTime.Today, reference, rqmtCmt, true);
                        }
                        //Store contract in vault
                        string strTradeDt = dtTradeDt.ToString("MM/dd/yyyy");
                        string strToday = DateTime.Today.ToString("MM/dd/yyyy");
                        string trdSysCode = tradingSystem.Substring(0, 1);

                        //Insert or update trade_rqmt_confirm_row
                        TradeRqmtConfirmBlobDal trcBlobDal = new TradeRqmtConfirmBlobDal(sqlConnectionStr);
                        TradeRqmtConfirmBlobDto trcBlobData = new TradeRqmtConfirmBlobDto();
                        trcBlobData.ImageFileExt = Utils.GetDocFormatFileExt(ADocFormat);
                        trcBlobData.DocBlob = WSUtils.GetByteArrayFromDocument(editContractForm.richeditConfirm, ADocFormat);
                        trcBlobData.TradeRqmtConfirmId = ARqmtConfirmId;

                        int rowCount = trcBlobDal.GetCount(ARqmtConfirmId);
                        if (rowCount > 0)
                            trcBlobDal.Update(trcBlobData);
                        else
                            trcBlobDal.Insert(trcBlobData);

                        if (currentStatusCode == "SENT")
                        {
                            Vaulter vaulter = new Vaulter(sqlConnectionStr);
                            vaulter.VaultTradeRqmtConfirm(ARqmtConfirmId, null);
                        }

                        if (editContractForm.isApproveAndSend)
                            SendToFaxGateway(transMethodInd, faxTelexNumber, confirmLabel, trcBlobData.DocBlob, ADocFormat,
                               cptySn, tradeId, ticketNo, rqmtId, Convert.ToInt32(ARqmtConfirmId), false,
                               "", "", "", false);

                        ts.Complete();
                    }
                }
                else
                {
                    transMethodInd = editContractForm.liveTransMethod.Substring(0, 1);
                    faxTelexNumber = editContractForm.liveFaxNumber;
                    if (ARqmtConfirmId > 0 &&
                       ((ASavedTransMethod.Substring(0, 1) != transMethodInd) ||
                        (ASavedFaxNumber != faxTelexNumber)))
                    {
                        //Update trade_rqmt_confirm row if contract editor was cancelled but number
                        //was still changed.
                        UpdateTradeRqmtConfirmRow(ARqmtConfirmId, tradeId, rqmtId, ATemplateName,
                           transMethodInd, faxTelexNumber, confirmLabel, ACmt, ANextStatusCode, AActiveFlag);
                    }
                }

                //int rowHandle = 0;
                rowHandle = gridViewSummary.FocusedRowHandle;
                gridViewSummary_FocusedRowChanged(gridViewSummary,
                   new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(0, rowHandle));
                gridViewSummary.SelectRow(rowHandle);

                //editContractForm.Close();
                //editContractForm.Dispose();
                //editContractForm = null;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to process a confirm document." + Environment.NewLine +
                     "Error CNF-124 in " + FORM_NAME + ".CallEditContractForm(): " + ex.Message);
            }
        }

        private string GetEconomicData()
        {
            try
            {
                string econData = "";
                GridView view = gridViewSummary;
                string buySellInd = GetStringWithTest(view, "BuySellInd");
                string startDt = GetStringWithTest(view, "StartDt");
                string endDt = GetStringWithTest(view, "EndDt");

                //Israel 9/22/2015
                //string qtyTot = GetStringWithTest(view, "QtyTot");
                string qtyDesc = GetStringWithTest(view, "QuantityDescription");
                string tradeDesc = GetStringWithTest(view, "TradeDesc");
                string locationSn = GetStringWithTest(view, "LocationSn");

                //Israel 9/22/2015
                //string uomDurCode = GetStringWithTest(view, "UomDurCode");
                //string payPrice = GetStringWithTest(view, "PayPrice");
                //string recPrice = GetStringWithTest(view, "RecPrice");

                //string econData = "";
                //Israel 9/22/2015
                //if (buySellInd == "B")
                //    econData = "Buy";
                //else if (buySellInd == "S")
                //    econData = "Sell";
                //else
                //    econData = "?";

                econData = "Desc=" + tradeDesc;
                econData += ";Start=" + startDt + ";End=" + endDt;
                econData += ";Qty=" + qtyDesc + ";Loc=" + locationSn;
                //econData += ";Pay=" + payPrice + ";Rec=" + recPrice;
                return econData;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to assemble display data from the Summary Grid." + Environment.NewLine +
                     "Error CNF-125 in " + FORM_NAME + ".GetEconomicData(): " + ex.Message);
            }
        }

        private string GetStringWithTest(GridView AView, string FieldName)
        {
            try
            {
                string val = "";
                int rowHandle = GetGridViewFocusedRowHandle(AView);
                if (AView.GetRowCellDisplayText(rowHandle, FieldName).ToString().Length > 0)
                    val = AView.GetRowCellDisplayText(rowHandle, FieldName).ToString();
                return val;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to read grid: " + AView.Name + ", field: " + FieldName + "." + Environment.NewLine +
                     "Error CNF-126 in " + FORM_NAME + ".GetStringWithTest(): " + ex.Message);
            }
        }

        private string GetGroupEmailAddress(string ASempraSn, string ACdtyGroup)
        {
            try
            {
                string emailAddress = "";
                //emailAddress = Properties.Settings.Default.EMailGroupManagers;
                return emailAddress;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to get the email address for" + Environment.NewLine +
                    "Booking Company: " + ASempraSn + ", Commodity Group: " + ACdtyGroup + Environment.NewLine +
                     "Error CNF-127 in " + FORM_NAME + ".GetGroupEmailAddress(): " + ex.Message);
            }
        }

        private void SendEmailToTrader(long ATradeId)
        {
            //try
            //{
            //    GridView view = gridViewSummary;
            //    int rowHandle = GetGridViewFocusedRowHandle(view);
            //    string bookingCoSn = view.GetRowCellDisplayText(rowHandle, "BookingCoSn").ToString();
            //    //string cdtyGrpCode = view.GetRowCellDisplayText(rowHandle, "CdtyGrpCode").ToString();
            //    string cptySn = view.GetRowCellDisplayText(rowHandle, "CptySn").ToString();
            //    string cdtyCode = view.GetRowCellDisplayText(rowHandle, "CdtyCode").ToString();

            //    string fromEmailAddress = GetGroupEmailAddress(bookingCoSn, cdtyCode);
            //    //Deprecated Israel 
            //    //string traderEmailAddress = GetTraderEmailAddress(ATradeId).ToLower();
            //    string traderEmailAddress = "ifrankel";
            //    string testTraderEmailAddress = traderEmailAddress;

            //    //1/28/2015 Israel - Replaced DB name with system setting
            //    //if (barStaticDBName.Caption.ToLower() != PROD_DB_NAME)
            //    if (!Properties.Settings.Default.IsProductionSystem)
            //    {
            //        fromEmailAddress = Properties.Settings.Default.EMailGroupSupport;
            //        traderEmailAddress = Utils.GetUserNameWithoutDomain(toolbarOrWindowsUserId.ToString().ToLower()) + 
            //            "@" + Properties.Settings.Default.EMailDomain;
            //    }

            //    string subject = "Confirmation Pending: " + ATradeId.ToString();
            //    string body = "There is a new confirmation waiting for your approval." + Environment.NewLine;
            //    body += "Trade Id: " + ATradeId.ToString() + Environment.NewLine;
            //    body += "Counterparty: " + cptySn + Environment.NewLine;
            //    body += "Commodity: " + cdtyCode + Environment.NewLine;
            //    //body += "To approve the trade go to: " + Properties.Settings.Default.TraderApprovalUrl;
            //    //1/28/2015 Israel - Replaced DB name with system setting
            //    //if (barStaticDBName.Caption.ToLower() != PROD_DB_NAME)
            //    if (!Properties.Settings.Default.IsProductionSystem)
            //        body += Environment.NewLine + "**** In production the Trader EMail Address would be: " +
            //           testTraderEmailAddress + " ****";

            //    SendEmail(fromEmailAddress, traderEmailAddress, subject, body);
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("SendEmailToTrader: " + ex.Message);
            //}
        }

        private void SendEmail(string AFromAddress, string AToAddress, string ASubject, string ABody,
           params string[] AAttachFileName)
        {
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(AFromAddress,
                   AToAddress, ASubject, ABody);

                for (int i = 0; i < AAttachFileName.Length; i++)
                {
                    message.Attachments.Add(new Attachment(AAttachFileName[i]));
                }

                //Send the message.
                SmtpClient client = new SmtpClient(Properties.Settings.Default.EmailHost,
                                                   Properties.Settings.Default.EmailPort);

                try
                {
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("Error caught while executing client.Send(message) in SendEmail: "
                       + Environment.NewLine + ex.ToString());
                }
            }
            catch (Exception ex)
            {
                string value = String.Empty;
                if (AAttachFileName.Length > 0)
                    value = String.Join(", ", AAttachFileName);
                throw new Exception("An error occurred while attempting to send an email with the following settings:" + Environment.NewLine +
                    "From Address: " + AFromAddress + ", To Address: " + AToAddress + ", Subject: " + ASubject + Environment.NewLine +
                    "Body Text: " + ABody + ", Attachment File Name(s): " + value + Environment.NewLine +
                     "Error CNF-128 in " + FORM_NAME + ".SendEmail(): " + ex.Message);
            }
        }

        private Int32 UpdateTradeRqmtConfirmRow(Int32 AId, Int32 ATradeId, Int32 ARqmtId, string ATemplateName,
           string AFaxTelexInd, string AFaxTelexNumber, string AConfirmLabel, string AConfirmCmt,
           string ANextStatusCode, string AActiveFlag)
        {
            //string agreementInfo = "";
            try
            {
                TradeRqmtConfirm tradeRqmtConfirmData = new TradeRqmtConfirm();
                tradeRqmtConfirmData.Id = AId;
                tradeRqmtConfirmData.TradeId = ATradeId;
                tradeRqmtConfirmData.RqmtId = ARqmtId;
                tradeRqmtConfirmData.TemplateName = ATemplateName;
                tradeRqmtConfirmData.NextStatusCode = ANextStatusCode;
                tradeRqmtConfirmData.ConfirmLabel = AConfirmLabel;
                tradeRqmtConfirmData.ConfirmCmt = AConfirmCmt;
                tradeRqmtConfirmData.FaxTelexInd = AFaxTelexInd;
                tradeRqmtConfirmData.FaxTelexNumber = AFaxTelexNumber;
                tradeRqmtConfirmData.ConfirmLabel = AConfirmLabel;
                tradeRqmtConfirmData.ConfirmCmt = AConfirmCmt;
                tradeRqmtConfirmData.ActiveFlag = AActiveFlag;

                TradeRqmtConfirmDal tradeRqmtConfirmDal = new TradeRqmtConfirmDal(sqlConnectionStr);

                Int32 spProcReturnValue = 0;
                //Israel 11/05/2015 -- Restored Insert capability
                if (AId == 0)
                    spProcReturnValue = tradeRqmtConfirmDal.Insert(tradeRqmtConfirmData);
                else
                {
                    spProcReturnValue = tradeRqmtConfirmDal.Update(tradeRqmtConfirmData);
                }

                if (spProcReturnValue > 0)
                {
                    if (AId == 0)
                    {
                        DataRow row = confirmDataTable.NewRow();
                        //row["Id"] = updateTradeRqmtConfirmResponse.@return.request.id;
                        row["Id"] = spProcReturnValue;
                        row["RqmtId"] = ARqmtId;
                        row["TradeId"] = ATradeId;
                        //row["TemplateId"] = 0;
                        row["NextStatusCode"] = ANextStatusCode;
                        row["ConfirmLabel"] = AConfirmLabel;
                        row["ConfirmCmt"] = AConfirmCmt;
                        row["FaxTelexInd"] = AFaxTelexInd;
                        row["FaxTelexNumber"] = AFaxTelexNumber;
                        row["XmitStatusInd"] = "";
                        row["XmitAddr"] = "";
                        row["XmitCmt"] = "";
                        //row["XmitTimeStampgmt"] = System.DBNull.Value;
                        row["TemplateName"] = ATemplateName;
                        row["TemplateCategory"] = "";
                        row["TemplateTypeInd"] = "";
                        row["FinalApprovalFlag"] = "";
                        row["ActiveFlag"] = AActiveFlag;
                        confirmDataTable.Rows.Add(row);
                    }
                    else
                    {
                        string filterStr = "Id = " + AId.ToString();
                        foreach (DataRow row in confirmDataTable.Select(filterStr))
                        {
                            row.BeginEdit();
                            //row["TemplateId"] = 0;
                            row["TemplateName"] = ATemplateName;
                            row["NextStatusCode"] = ANextStatusCode;
                            row["ConfirmLabel"] = AConfirmLabel;
                            row["ConfirmCmt"] = AConfirmCmt;
                            row["FaxTelexInd"] = AFaxTelexInd;
                            row["FaxTelexNumber"] = AFaxTelexNumber;
                            row.AcceptChanges();
                            row.EndEdit();
                        }
                    }
                    return spProcReturnValue;
                }
                else
                {
                    //XtraMessageBox.Show("Error Updating: " + updateTradeRqmtConfirmResponse.@return.responseText);
                    XtraMessageBox.Show("Error Updating TradeRqmtConfirm.");
                    return -1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to update a Confirm using the following values:" + Environment.NewLine +
                    "Confirm Id: " + AId.ToString() + ", Trade Id: " + ATradeId.ToString() + ", Rqmt Id: " + ARqmtId.ToString() +
                        ", Template Name: " + ATemplateName + Environment.NewLine +
                    "Transmission Method: " + AFaxTelexInd + ", Transmission Send-To Address: " + AFaxTelexNumber + ", Active Flag: " + AActiveFlag + Environment.NewLine +
                     "Error CNF-129 in " + FORM_NAME + ".UpdateTradeRqmtConfirmRow(): " + ex.Message);
            }
        }

        private void InitGridLookupContractList()
        {
            try
            {
                gridLookupContractListTable = new System.Data.DataTable();
                gridLookupContractListTable.Columns.Add(new DataColumn("TradeDate", typeof(DateTime)));
                gridLookupContractListTable.Columns.Add(new DataColumn("TemplateName", typeof(string)));
                gridLookupContractListTable.Columns.Add(new DataColumn("TradeConfirmId", typeof(int)));
                gridLookupContractListTable.Columns.Add(new DataColumn("TradeRqmtConfirmId", typeof(int)));

                gluedContractList.Properties.DataSource = gridLookupContractListTable.DefaultView;
                gluedContractList.Properties.DisplayMember = "TradeDate";
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to initialize internal confirm lookup list:" + Environment.NewLine +
                     "Error CNF-130 in " + FORM_NAME + ".InitGridLookupContractList(): " + ex.Message);
            }
        }

        private string CallGetAgreementInfo(string ACptySn, string ABookingCoSn, string ATradeDt)
        {
            string cptyAgreementInfo = "";
            try
            {
                //Israel 10/26/15 -- Temporarily deprecated pending implementation of new CptyInfo url
                //CptyInfoAPIDal cptyInfoAPIDal = new CptyInfoAPIDal(Properties.Settings.Default.CptyInfoAPIUrl);
                //cptyAgreementInfo = cptyInfoAPIDal.GetAgreementDisplay(ACptySn, ABookingCoSn, ATradeDt);
                return cptyAgreementInfo;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to retrieve agreement information using the following values:" + Environment.NewLine +
                    "Counterparty Short Name: " + ACptySn + ", Booking Company Short Name: " + ABookingCoSn + ", Trade Date: " + ATradeDt + Environment.NewLine +
                     "Error CNF-131 in " + FORM_NAME + ".CallGetAgreementInfo(): " + ex.Message);
            }
        }

        public static string GetFaxMethodFromInd(string AFaxInd)
        {
            try
            {
                string method = "FAX";
                if (AFaxInd == "F")
                    method = "FAX";
                else if (AFaxInd == "E")
                    method = "EMAIL";
                return method;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to retrieve the transmission method using the following indicator: " + AFaxInd + Environment.NewLine +
                     "Error CNF-132 in " + FORM_NAME + ".GetFaxMethodFromInd(): " + ex.Message);
            }
        }

        public static string GetFaxIndFromMethod(string AFaxMethod)
        {
            try
            {
                string faxInd = "F";
                if (AFaxMethod == "FAX")
                    faxInd = "F";
                else if (AFaxMethod == "EMAIL")
                    faxInd = "E";
                return faxInd;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to retrieve the transmission indicator using the following method: " + AFaxMethod + Environment.NewLine +
                     "Error CNF-133 in " + FORM_NAME + ".GetFaxIndFromMethod(): " + ex.Message);
            }
        }


        public void CallUpdateTradeRqmts(long ATradeId, long ARqmtId, string ARqmtCode, string AStatusCode,
           DateTime AStatusDate, string AReference, string AComment, bool AUpdateLocalTableNow)
        {
            try
            {
                TradeRqmtDto tradeRqmtDto = new TradeRqmtDto();
                tradeRqmtDto.TradeId = Convert.ToInt32(ATradeId);
                tradeRqmtDto.Id = Convert.ToInt32(ARqmtId);
                tradeRqmtDto.RqmtCode = ARqmtCode;
                tradeRqmtDto.CompletedDt = AStatusDate;
                tradeRqmtDto.StatusCode = AStatusCode;
                tradeRqmtDto.Cmt = AComment;
                tradeRqmtDto.Reference = AReference;

                List<TradeRqmtDto> tradeRqmtList = new List<TradeRqmtDto>();
                tradeRqmtList.Add(tradeRqmtDto);

                TradeRqmtDal tradeRqmtDal = new TradeRqmtDal(sqlConnectionStr);
                if (AUpdateLocalTableNow)
                {
                    int rowsUpdated = 0;
                    rowsUpdated = tradeRqmtDal.UpdateTradeRqmts(tradeRqmtList);

                    if (rowsUpdated > 0)
                        UpdateLocalRqmtData(tradeRqmtList);
                }
            }
            catch (Exception ex)
            {
                string dateString = String.Empty;
                try
                {
                    dateString = AStatusDate.ToString("dd-MMM-yyyy");
                }
                catch (Exception except)
                {
                    dateString = "[Status Date conversion failed: " + except.Message + "]";
                }

                throw new Exception("An error occurred while attempting to update a Trade Requirment using the following values:" + Environment.NewLine +
                    "Trade Id: " + ATradeId.ToString() + ", Rqmt Id: " + ARqmtId.ToString() + ", Rqmt Code: " + ARqmtCode + ", Status Code: " + AStatusCode + Environment.NewLine +
                    "Status Date: " + dateString + ", Reference: " + AReference + ", Comment: " + AComment + ", UpdateLocalTableNow?: " + AUpdateLocalTableNow + Environment.NewLine +
                     "Error CNF-134 in " + FORM_NAME + ".CallUpdateTradeRqmts(): " + ex.Message);
            }
        }

        private int GetConfirmRqmtId(int AConfirmId)
        {
            try
            {
                int rqmtId = 0;
                string filterStr = "Id = " + AConfirmId.ToString();
                foreach (DataRow row in confirmDataTable.Select(filterStr))
                {
                    rqmtId = int.Parse(row["RqmtId"].ToString());
                }
                return rqmtId;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to retrieve the Confirm Grid Rqmt Id for Confirm Id: " + AConfirmId.ToString() + Environment.NewLine +
                     "Error CNF-135 in " + FORM_NAME + ".GetConfirmRqmtId(): " + ex.Message);
            }
        }

        private string GetConfirmData(int ARqmtConfirmId, string AFieldName)
        {
            try
            {
                string val = "";
                string filterStr = "Id = " + ARqmtConfirmId.ToString();
                foreach (DataRow row in confirmDataTable.Select(filterStr))
                {
                    val = row[AFieldName].ToString();
                    break;
                }
                return val;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to retrieve data from the Confirm Grid using the following values:" + Environment.NewLine +
                    "Confirm Id: " + ARqmtConfirmId.ToString() + ", Field Name: " + AFieldName + Environment.NewLine +
                     "Error CNF-136 in " + FORM_NAME + ".GetConfirmData(): " + ex.Message);
            }
        }

        private bool IsContractOkToPrint(long ATradeId)
        {
            try
            {
                bool okToPrint = false;
                string confirmLabel = "";
                //Israel 11/5/2015 -- added support for multiple trade_rqmt_confirm rows
                //string filterStr = "TradeId = " + ATradeId.ToString();
                string filterStr = "TradeId = " + ATradeId.ToString() + " and ActiveFlag = 'Y'";
                foreach (DataRow row in confirmDataTable.Select(filterStr))
                {
                    confirmLabel = row["ConfirmLabel"].ToString();
                    if (confirmLabel == CONFIRM_LABEL_CONFIRM)
                    {
                        okToPrint = true;
                        break;
                    }
                }
                return okToPrint;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to verify the selected confirm is active for Treade Id:" + ATradeId.ToString() + Environment.NewLine +
                     "Error CNF-137 in " + FORM_NAME + ".IsContractOkToPrint(): " + ex.Message);
            }
        }

        private int GetContractRowId(long ATradeId)
        {
            try
            {
                int confirmId = 0;
                string confirmLabel = "";
                //Israel 11/5/2015 -- added support for multiple trade_rqmt_confirm rows
                //string filterStr = "TradeId = " + ATradeId.ToString();
                string filterStr = "TradeId = " + ATradeId.ToString() + " and ActiveFlag = 'Y'";
                foreach (DataRow row in confirmDataTable.Select(filterStr))
                {
                    confirmLabel = row["ConfirmLabel"].ToString();
                    if (confirmLabel == CONFIRM_LABEL_CONFIRM)
                    {
                        confirmId = int.Parse(row["Id"].ToString());
                        break;
                    }
                }
                return confirmId;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to retrieve the active Id for the active Confirm for Trade Id:" + ATradeId.ToString() + Environment.NewLine +
                     "Error CNF-138 in " + FORM_NAME + ".GetContractRowId(): " + ex.Message);
            }
        }

        //Israel 11/25/2015 -- Deprecated since column no longer exists in DB
        //private int GetContractTemplateId(long ATradeId)
        //{
        //    try
        //    {
        //        int templateId = 0;
        //        string confirmLabel = "";
        //        //Israel 11/5/2015 -- added support for multiple trade_rqmt_confirm rows
        //        //string filterStr = "TradeId = " + ATradeId.ToString();
        //        string filterStr = "TradeId = " + ATradeId.ToString() + " and ActiveFlag = 'Y'";
        //        foreach (DataRow row in confirmDataTable.Select(filterStr))
        //        {
        //            confirmLabel = row["ConfirmLabel"].ToString();
        //            if (confirmLabel == CONFIRM_LABEL_CONFIRM)
        //            {
        //                if (row["TemplateId"].ToString().Length > 0)
        //                    templateId = int.Parse(row["TemplateId"].ToString());
        //                break;
        //            }
        //        }
        //        return templateId;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("GetContractTemplateId: " + ex.Message);
        //    }
        //}

        private string GetContractConfirmData(long ATradeId, string AFieldName)
        {
            try
            {
                string val = "";
                string confirmLabel = "";
                //Israel 11/5/2015 -- added support for multiple trade_rqmt_confirm rows
                //string filterStr = "TradeId = " + ATradeId.ToString();
                string filterStr = "TradeId = " + ATradeId.ToString() + " and ActiveFlag = 'Y'";
                foreach (DataRow row in confirmDataTable.Select(filterStr))
                {
                    confirmLabel = row["ConfirmLabel"].ToString();
                    if (confirmLabel == CONFIRM_LABEL_CONFIRM)
                    {
                        if (row[AFieldName].ToString().Length > 0)
                            val = row[AFieldName].ToString();
                        break;
                    }
                }
                return val;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to retrieve data from the Confirm Grid using the following values:" + Environment.NewLine +
                    "TradeId: " + ATradeId.ToString() + ", Field Name: " + AFieldName + Environment.NewLine +
                     "Error CNF-139 in " + FORM_NAME + ".GetContractConfirmData(): " + ex.Message);
            }
        }

        private string GetTradeRqmtData(int ARqmtId, string AFieldName)
        {
            try
            {
                string val = "";
                string filterStr = "Id = " + ARqmtId.ToString();
                foreach (DataRow row in rqmtDataTable.Select(filterStr))
                {
                    if (row["Rqmt"].ToString() == SEMPRA_RQMT)
                    {
                        val = row[AFieldName].ToString();
                        break;
                    }
                }
                return val;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to retrieve data from the Rqmt Grid using the following values:" + Environment.NewLine +
                    "Rqmt Id: " + ARqmtId.ToString() + ", Field Name: " + AFieldName + Environment.NewLine +
                     "Error CNF-140 in " + FORM_NAME + ".GetTradeRqmtData(): " + ex.Message);
            }
        }

        private string GetTradeSummaryData(Int32 ATradeId, string AFieldName)
        {
            try
            {
                //3/18/2010 Israel -- Added to make sure the Recipient is always filled in 
                //when sending faxes, even when no trade_summary row exists.
                string val = "n/a";
                string filterStr = "TradeId = " + ATradeId.ToString();
                foreach (DataRow row in summaryDataTable.Select(filterStr))
                {
                    val = row[AFieldName].ToString();
                    break;
                }
                return val;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to retrieve data from the Summary Grid using the following values:" + Environment.NewLine +
                    "Trade Id: " + ATradeId.ToString() + ", Field Name: " + AFieldName + Environment.NewLine +
                     "Error CNF-141 in " + FORM_NAME + ".GetTradeSummaryData(): " + ex.Message);
            }
        }

        private DateTime GetTradeSummaryDate(long ATradeId, string AFieldName)
        {
            try
            {
                DateTime val = DateTime.Parse("01/01/1900");
                string filterStr = "TradeId = " + ATradeId.ToString();
                foreach (DataRow row in summaryDataTable.Select(filterStr))
                {
                    val = DateTime.Parse(row[AFieldName].ToString());
                    break;
                }
                return val;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to retrieve a date from the Summary Grid using the following values:" + Environment.NewLine +
                    "Trade Id: " + ATradeId.ToString() + ", Field Name: " + AFieldName + Environment.NewLine +
                     "Error CNF-142 in " + FORM_NAME + ".GetTradeSummaryDate(): " + ex.Message);
            }
        }

        private void gridViewSummary_CustomColumnDisplayText(object sender,
           DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.Name == "colSummaryCmt")
                e.DisplayText = e.DisplayText.Replace('\n', ' ');
        }

        private void DisplayContract(int ATradeRqmtConfirmId, bool AEMailEnabled)
        {
            try
            {
                //Israel 9/21/2015
                //rtfContract.Rtf = AContractBody;
                //rtfContract.SelectAll();
                //rtfContract.SelectionProtected = true;
                //rtfContract.Select(5, 0);

                TradeRqmtConfirmBlobDal trcBlobDal = new TradeRqmtConfirmBlobDal(sqlConnectionStr);
                TradeRqmtConfirmBlobDto trcBlobData = new TradeRqmtConfirmBlobDto();
                //Int64 confirmIdLong = Convert.ToInt64(ATradeRqmtConfirmId);
                trcBlobData = trcBlobDal.Get(ATradeRqmtConfirmId);
                DocumentFormat docFormat = Utils.GetDocumentFormat(trcBlobData.ImageFileExt);

                if (trcBlobData.DocBlob.Length > 0)
                {
                    using (MemoryStream contractStream = new MemoryStream(trcBlobData.DocBlob))
                    {
                        richeditConfirmBrowser.LoadDocument(contractStream, docFormat);
                    }
                }

                tabctrlBrowserApps.SelectedTabPage = tabpgContract;
                //barDockControl3.Visible = false;
                //barBrowserStatusBar.Visible = false;
                btnEmail.Enabled = AEMailEnabled;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to display a Confirm using the following values:" + Environment.NewLine +
                     "Confirm Id: " + ATradeRqmtConfirmId.ToString() + ", EMail Option enabled?: " + AEMailEnabled + Environment.NewLine +
                      "Error CNF-143 in " + FORM_NAME + ".DisplayContract(): " + ex.Message);
            }
        }

        private void SendToFaxGateway(Int32 AConfirmId, string ACptySn, bool ARtf,
           string ATitle, string AMessage, bool ACoverPage)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string faxTelexInd = "";
                string faxTelexNumber = "";
                string confirmLabel = "";
                Int32 tradeId = 0;
                string tradeSysTicket = "";
                Int32 rqmtId = 0;
                //Int32 templateId = 0;
                string cptySn = "";

                string filterStr = "Id = " + AConfirmId.ToString();
                foreach (DataRow row in confirmDataTable.Select(filterStr))
                {
                    faxTelexInd = row["FaxTelexInd"].ToString();
                    faxTelexNumber = row["FaxTelexNumber"].ToString();
                    confirmLabel = row["ConfirmLabel"].ToString();
                    tradeId = Int32.Parse(row["TradeId"].ToString());
                    tradeSysTicket = GetTradeSummaryData(tradeId, "TradeSysTicket"); //row["TradeSysTicket"].ToString();
                    rqmtId = Int32.Parse(row["RqmtId"].ToString());
                    //templateId = Int32.Parse(row["TemplateId"].ToString());
                }

                //contractBody = GetContractFromVault(AConfirmId, 0);
                TradeRqmtConfirmBlobDal trcBlobDal = new TradeRqmtConfirmBlobDal(sqlConnectionStr);
                TradeRqmtConfirmBlobDto trcBlobData = new TradeRqmtConfirmBlobDto();
                //Int64 confirmIdLong = Convert.ToInt64(AConfirmId);
                trcBlobData = trcBlobDal.Get(AConfirmId);
                DocumentFormat docFormat = Utils.GetDocumentFormat(trcBlobData.ImageFileExt);

                string title = "";
                string message = "";
                if (ACoverPage)
                {
                    title = ATitle;
                    message = AMessage;
                }

                SendToFaxGateway(faxTelexInd, faxTelexNumber, confirmLabel, trcBlobData.DocBlob, docFormat, ACptySn, tradeId, tradeSysTicket, rqmtId,
                    Convert.ToInt32(AConfirmId), ARtf, "", title, message, ACoverPage);               

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to prepare then request SendToFaxGateway([14 parms]) using the following values:" + Environment.NewLine +
                    "Confirm Id: " + AConfirmId.ToString() + ", Counterparty Short Name: " + ACptySn + ", IsRtf?: " + ARtf + Environment.NewLine +
                    "Title: " + ATitle + ", Message: " + AMessage + ", IncludeCoverPage?: " + ACoverPage + Environment.NewLine +
                     "Error CNF-144 in " + FORM_NAME + ".SendToFaxGateway([6 parms]): " + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void SendToFaxGateway(string AFaxTelexInd, string AFaxTelexNumbers, string AConfirmLabel,
           byte[] AContractBody, DocumentFormat ADocFormat, string ARecipient, Int32 ATradeId, string ATradeSysTicket, Int32 ARqmtId, Int32 AConfirmId,
           bool ARtf, string AFromAddress, string ASubject, string AEmailBody, bool ACoverPage)
        {

            try
            {
                if (AFaxTelexNumbers != null)
                {
                    string[] AFaxTelexNumbersList = AFaxTelexNumbers.Split(';');

                    foreach (string AFaxTelexNumber in AFaxTelexNumbersList)
                    {
                        if (AContractBody == null)
                        {
                            XtraMessageBox.Show("Confirm Data was not found. Send/Resend was cancelled.",
                               "Confirm Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (!InboundSettings.IsProductionSystem)
                        {
                            var destination = new TransmitDestination(AFaxTelexNumber);
                            if (!destination.IsValidNonProdSendToAddress())
                            {
                                XtraMessageBox.Show("Please enter a valid Non-Production EMail Address or Fax Number.", "Non-Production Address Verification",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }

                        this.Cursor = Cursors.WaitCursor;
                        string faxTelexNumber = AFaxTelexNumber;

                        //5/21/09 Israel -- Create a new folder for each fax transmission.
                        string folderName = ATradeId.ToString() + "_" + String.Format("{0:yyMMddHHmmss}", DateTime.Now);
                        //string faxDir = tempFaxDir + folderName;
                        string faxDir = Path.Combine(tempFaxDir, folderName);
                        System.IO.Directory.CreateDirectory(faxDir);
                        faxDir += "\\";

                        string xmlFileNameOnly = "request.xml";
                        string xmlFileNameWithPath = faxDir + xmlFileNameOnly;
                        string rtfFileNameOnly = "Contract.rtf";
                        string rtfFileNameWithPath = faxDir + rtfFileNameOnly;
                        string pdfDocFileNameOnly = "Contract.pdf";
                        string pdfDocFileNameWithPath = faxDir + pdfDocFileNameOnly;

                        //Israel 9/3/2015 -- Replace PDFMetamorphosis with DevExpress RichEditControl
                        //SaveRtfAsPdfDoc(rtfFileNameWithPath, pdfDocFileNameWithPath);
                        WSUtils.SaveByteArrayAsPdfFile(AContractBody, ADocFormat, pdfDocFileNameWithPath);

                        //1/28/2015 Israel - Replaced DB name with system setting
                        //if (barStaticDBName.Caption.ToLower() != PROD_DB_NAME)

                        //Israel 10/26/15 Removed TestFaxNumber
                        //if (!Properties.Settings.Default.IsProductionSystem)
                        //    faxTelexNumber = Properties.Settings.Default.TestFaxNumber;

                        //PDF isn't handling E. European languages properly so send them as rtf.
                        string docFileName = pdfDocFileNameOnly;
                        string bookingCoSn = GetTradeSummaryData(ATradeId, "BookingCoSn");
                        string cdtyCode = GetTradeSummaryData(ATradeId, "CdtyCode");
                        //       bool isFreightDeal = (cdtyCode == "FRGHT");

                        string cptySn = GetTradeSummaryData(ATradeId, "CptySn");

                        //5/20/09 Israel - Handle RTF override parm
                        if (ARtf)
                            docFileName = rtfFileNameOnly;

                        string docFileNameWithPath = faxDir + docFileName;

                        TransmitDestinationType transDestType;
                        if (AFaxTelexNumber.Contains("@"))
                            transDestType = TransmitDestinationType.EMAIL;
                        else
                            transDestType = TransmitDestinationType.FAX;


                        IXmitRequestDal xmitRequestDal = new XmitRequestDal(sqlConnectionStr);
                        int xmitRequestId = xmitRequestDal.SaveTradeRqmtConfirmXmitRequest(AConfirmId, transDestType, AFaxTelexNumber, Utils.GetUserNameWithoutDomain(p_UserId));

                        string xmlText = GetFaxSubmitXML(ATradeId.ToString(),ATradeSysTicket, docFileName, AFaxTelexInd,
                           AFaxTelexNumber, ARecipient, ARqmtId.ToString(), AConfirmId.ToString(), AConfirmLabel,
                           ASubject, AEmailBody, ACoverPage, xmitRequestId.ToString());

                        System.IO.File.WriteAllText(xmlFileNameWithPath, xmlText);

                        string emailToAddress = Properties.Settings.Default.TransmissionGatewayEmailToAddress;
                        //string emailToAddress = "ifrankel@RBSSempra.com";
                        //Israel 9/28/2015
                        //string emailFromAddress = toolbarOrWindowsUserId + "@" + Properties.Settings.Default.EMailDomain;

                        //Israel 10/26/15 -- Removed FaxGatewayEmailFromAddress
                        string emailFromAddress = ""; //Properties.Settings.Default.FaxGatewayEmailFromAddress;
                        if (AFromAddress.Length > 2)
                            emailFromAddress = AFromAddress;
                        else
                            emailFromAddress = emailToAddress;
                        string emailSubject = "Confirmation of Trade: " + ATradeSysTicket;
                        if (ASubject.Length > 2)
                            emailSubject = ASubject;

                        //Changed variable name for doc file.
                        SendEmail(emailFromAddress, emailToAddress, emailSubject, AEmailBody, xmlFileNameWithPath, docFileNameWithPath);

                        string faxDocRefCode = "";
                        string templateName = "**Template Name**";

                        //Log submission in case it becomes necessary to trace it
                        //Israel 11/13/2015 -- Removed as part of move to XmitRequest/XmitResult
                        //CallInsertToFaxLogSent(ATradeId, AFaxTelexInd, AFaxTelexNumber, faxDocRefCode);

                        if (AConfirmLabel == CONFIRM_LABEL_CONFIRM && ARqmtId > 0)
                        {
                            string reference = GetTradeRqmtData(ARqmtId, "Reference");
                            string cmt = GetTradeRqmtData(ARqmtId, "Cmt");
                            CallUpdateTradeRqmts(ATradeId, ARqmtId, SEMPRA_RQMT, "SENT", DateTime.Today, reference, cmt, true);
                        }
                        else if (AConfirmId > 0)
                        {
                            string confirmCmt = GetConfirmData(AConfirmId, "ConfirmCmt");
                            UpdateTradeRqmtConfirmRow(AConfirmId, ATradeId, Convert.ToInt32(ARqmtId),
                               templateName, AFaxTelexInd, AFaxTelexNumber, AConfirmLabel, confirmCmt, "SENT", "Y");
                        }

                        string trdSysCode = GetTradeSummaryData(ATradeId, "TrdSysCode");
                        if (trdSysCode.Length > 0)
                        {
                            trdSysCode = trdSysCode.Substring(0, 1);
                            //string cdtyGrpCode = GetTradeSummaryData(ATradeId, "CdtyGrpCode");
                            string sttlType = GetTradeSummaryData(ATradeId, "SttlType");

                            DateTime dtTradeDt = GetTradeSummaryDate(ATradeId, "TradeDt");
                            string strTradeDt = dtTradeDt.ToString("MM/dd/yyyy");
                            string strToday = DateTime.Today.ToString("MM/dd/yyyy");
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to send a document to the Transmission Gateway using the following values:" + Environment.NewLine +
                    "Transmission Method: " + AFaxTelexInd + ", Transmission Send-To Address: " + AFaxTelexNumbers + ", Document Format: " + ADocFormat.ToString() +
                        ", Recipient: " + ARecipient + Environment.NewLine +
                    "Trade Id: " + ATradeId.ToString() + "Rqmt Id: " + ARqmtId.ToString() + "Confirm Id: " + AConfirmId.ToString() + ", IsRtf?: " + ARtf + Environment.NewLine +
                    "From Address: " + AFromAddress + ", Subject: " + ASubject + ", EMail Body: " + AEmailBody + ", IncludeCoverPage?: " + ACoverPage + Environment.NewLine +
                     "Error CNF-145 in " + FORM_NAME + ".SendToFaxGateway([14 parms]): " + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        //Israel 11/12/2015 -- Following seems to be deprecated. Replaced by preceding.
        private bool IsEmailDevAllowed(string AEmail)
        {
            bool isAllowed = false;
            MailAddress address = new MailAddress(AEmail); //AEmail contains ifrankel@amphorainc.com
            string domain = address.Host; // host contains amphorainc.com
            for (int i = 0; i < Properties.Settings.Default.EmailDomainsDevAllowSendTo.Count; i++)
                if (domain == Properties.Settings.Default.EmailDomainsDevAllowSendTo[i])
                {
                    isAllowed = true;
                    break;
                }
            return isAllowed;
        }

        private string GetFaxSubmitXML(string ATradeId,string ATradeSysTicket, string ADocFileName, string AFaxTelexInd,
           string AFaxTelexNumber, string ARecipient, string ARqmtId, string AConfirmId,
           string AConfirmLabel, string ATitle, string AMessage, bool ACoverPage, string AXmitRequestId)
        {
            string xmlText = "";
            string faxMethod = "";


            string[] separator = new string[1];
            separator[0] = ";";
            string[] faxTelexNumbers = AFaxTelexNumber.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            string coverRequired = "N";
            if (ACoverPage)
                coverRequired = "Y";

            string userLoginIdWithoutDomain = Utils.GetUserNameWithoutDomain(toolbarOrWindowsUserId.ToLower());

            try
            {
                xmlText = XMLUtils.XML_HEADER;
                xmlText += XMLUtils.BuildTagItem(0, "Transmission", "", XMLUtils.TAG_OPEN, "");
                xmlText += XMLUtils.BuildTagItem(1, "app_code", "CNF", XMLUtils.TAG_OPEN_CLOSED, "");
                xmlText += XMLUtils.BuildTagItem(1, "app_ref", ATradeSysTicket, XMLUtils.TAG_OPEN_CLOSED, "");
                //Israel 10/6/2015
                //xmlText += XMLUtils.BuildTagItem(1, "app_sender", toolbarOrWindowsUserId.ToLower(),
                xmlText += XMLUtils.BuildTagItem(1, "app_sender", userLoginIdWithoutDomain, XMLUtils.TAG_OPEN_CLOSED, "");
                xmlText += XMLUtils.BuildTagItem(1, "comment", "FAX REQUEST", XMLUtils.TAG_OPEN_CLOSED, "");
                xmlText += XMLUtils.BuildTagItem(1, "receipt", GetSubmitActionUrlText(AXmitRequestId, "Queued"), XMLUtils.TAG_OPEN_CLOSED, "method=\"http\"");
                xmlText += XMLUtils.BuildTagItem(1, "documents", "", XMLUtils.TAG_OPEN, "");
                xmlText += XMLUtils.BuildTagItem(2, "document", ADocFileName,
                   XMLUtils.TAG_OPEN_CLOSED, "location=\"A\"");
                xmlText += XMLUtils.BuildTagItem(1, "documents", "", XMLUtils.TAG_CLOSED, "");
                xmlText += XMLUtils.BuildTagItem(1, "transmit_instructions", "", XMLUtils.TAG_OPEN, "");

                //Iterate through for each phone number
                string localNumber = "";
                string telex = "";
                string email = "";
                for (int i = 0; i < faxTelexNumbers.Length; i++)
                {
                    //9/3/2009 Israel - allow fax/email on single send.
                    if (faxTelexNumbers[i].Contains("@"))
                        faxMethod = "EMAIL";
                    else if (AFaxTelexInd == "F")
                        faxMethod = "FAX";
                    else if (AFaxTelexInd == "T")
                        faxMethod = "TELEX";
                    else
                        throw new Exception("Internal Exception: Unsupported FaxTelexInd=" + AFaxTelexInd);

                    localNumber = "";
                    email = "";
                    telex = "";

                    switch (faxMethod)
                    {
                        case "FAX":
                            { localNumber = faxTelexNumbers[i]; break; }
                        case "EMAIL":
                            {
                                email = faxTelexNumbers[i];
                                // double 'ifs' save unnecessary processing cycles on IsEmailDevAllowed()
                                //1/28/2015 Israel - Replaced DB name with system setting
                                //if (barStaticDBName.Caption.ToLower() != PROD_DB_NAME)

                                //Israel 10/26/15 -- Removed EMailDomain property setting.
                                //if (!Properties.Settings.Default.IsProductionSystem)                                    
                                //if (!IsEmailDevAllowed(email))
                                //email = toolbarOrWindowsUserId + "@" + Properties.Settings.Default.EMailDomain;
                                //Israel 10/26/15 -- Removed EMailDomain property setting.
                                //email = userLoginIdWithoutDomain + "@" + Properties.Settings.Default.EMailDomain;
                                break;
                            }
                        case "TELEX":
                            { telex = faxTelexNumbers[i]; break; }
                    }
                    xmlText += XMLUtils.BuildTagItem(2, "sendto", "", XMLUtils.TAG_OPEN, "cover_required=\"" +
                                                                                         coverRequired + "\"");
                    //xmlText += XMLUtils.BuildTagItem(2, "sendto", "", XMLUtils.TAG_OPEN, "cover_required=\"N\"");
                    xmlText += XMLUtils.BuildTagItem(3, "method", faxMethod, XMLUtils.TAG_OPEN_CLOSED, "");
                    xmlText += XMLUtils.BuildTagItem(3, "country_code", "", XMLUtils.TAG_OPEN_CLOSED, "");
                    xmlText += XMLUtils.BuildTagItem(3, "area_code", "", XMLUtils.TAG_OPEN_CLOSED, "");
                    xmlText += XMLUtils.BuildTagItem(3, "local_number", localNumber.Trim(), XMLUtils.TAG_OPEN_CLOSED, "");
                    xmlText += XMLUtils.BuildTagItem(3, "telex", telex.Trim(), XMLUtils.TAG_OPEN_CLOSED, "");
                    xmlText += XMLUtils.BuildTagItem(3, "email", email.Trim(), XMLUtils.TAG_OPEN_CLOSED, "");
                    xmlText += XMLUtils.BuildTagItem(3, "recipient", ARecipient, XMLUtils.TAG_OPEN_CLOSED, "");

                    if (ACoverPage)
                    {
                        string dateText = String.Format("{0:dd-MMM-yyyy}", DateTime.Today);
                        string toCompany = GetTradeSummaryData(Int32.Parse(ATradeId), "CptyLegalName");
                        string bookingCoSn = GetTradeSummaryData(Int32.Parse(ATradeId), "BookingCoSn");
                        string fromCompany = bookingCoSn;

                        xmlText += XMLUtils.BuildTagItem(3, "cover_sheet_data", "", XMLUtils.TAG_OPEN, "");
                        xmlText += XMLUtils.BuildTagItem(4, "title", ATitle, XMLUtils.TAG_OPEN_CLOSED, "");
                        xmlText += XMLUtils.BuildTagItem(4, "date", dateText, XMLUtils.TAG_OPEN_CLOSED, "");
                        //xmlText += XMLUtils.BuildTagItem(4, "from_name", toolbarOrWindowsUserId, XMLUtils.TAG_OPEN_CLOSED, "");
                        xmlText += XMLUtils.BuildTagItem(4, "from_name", userLoginIdWithoutDomain, XMLUtils.TAG_OPEN_CLOSED, "");
                        xmlText += XMLUtils.BuildTagItem(4, "from_company", fromCompany, XMLUtils.TAG_OPEN_CLOSED, "");
                        xmlText += XMLUtils.BuildTagItem(4, "from_fax", "", XMLUtils.TAG_OPEN_CLOSED, "");
                        xmlText += XMLUtils.BuildTagItem(4, "from_phone", "", XMLUtils.TAG_OPEN_CLOSED, "");
                        xmlText += XMLUtils.BuildTagItem(4, "to_name", "", XMLUtils.TAG_OPEN_CLOSED, "");
                        xmlText += XMLUtils.BuildTagItem(4, "to_company", toCompany, XMLUtils.TAG_OPEN_CLOSED, "");
                        xmlText += XMLUtils.BuildTagItem(4, "to_fax", localNumber.Trim(), XMLUtils.TAG_OPEN_CLOSED, "");
                        xmlText += XMLUtils.BuildTagItem(4, "no_pages", "", XMLUtils.TAG_OPEN_CLOSED, "");
                        xmlText += XMLUtils.BuildTagItem(4, "regarding", "Our Trade Id: " + ATradeId.ToString(),
                                                         XMLUtils.TAG_OPEN_CLOSED, "");
                        xmlText += XMLUtils.BuildTagItem(4, "message", AMessage, XMLUtils.TAG_OPEN_CLOSED, "");
                        xmlText += XMLUtils.BuildTagItem(3, "cover_sheet_data", "", XMLUtils.TAG_CLOSED, "");
                    }

                    xmlText += XMLUtils.BuildTagItem(3, "actions", "", XMLUtils.TAG_OPEN, "");
                    xmlText += XMLUtils.BuildTagItem(4, "action", "", XMLUtils.TAG_OPEN, "");
                    xmlText += XMLUtils.BuildTagItem(5, "onsuccess", GetSubmitActionUrlText(AXmitRequestId, "Success"), XMLUtils.TAG_OPEN_CLOSED, "method=\"http\"");
                    xmlText += XMLUtils.BuildTagItem(5, "onfail", GetSubmitActionUrlText(AXmitRequestId, "Failed"), XMLUtils.TAG_OPEN_CLOSED, "method=\"http\"");
                    xmlText += XMLUtils.BuildTagItem(4, "action", "", XMLUtils.TAG_CLOSED, "");
                    xmlText += XMLUtils.BuildTagItem(3, "actions", "", XMLUtils.TAG_CLOSED, "");
                    xmlText += XMLUtils.BuildTagItem(2, "sendto", "", XMLUtils.TAG_CLOSED, "");
                }

                xmlText += XMLUtils.BuildTagItem(1, "transmit_instructions", "", XMLUtils.TAG_CLOSED, "");
                xmlText += XMLUtils.BuildTagItem(0, "Transmission", "", XMLUtils.TAG_CLOSED, "");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while constructing the xml document that is sent to the Transmission Gateway using the following values:" + Environment.NewLine +
                    "Trade Id: " + ATradeId.ToString() + ", Document File Name: " + ADocFileName + "Transmission Method: " + AFaxTelexInd +
                        ", Transmission Send-To Address: " + AFaxTelexNumber + ", Recipient: " + ARecipient + Environment.NewLine +
                    "Rqmt Id: " + ARqmtId.ToString() + "Confirm Id: " + AConfirmId.ToString() + ", Title: " + ATitle + Environment.NewLine +
                    "Message: " + AMessage + ", IncludeCoverPage?: " + ACoverPage + ", XMit Request Id: " + AXmitRequestId.ToString() + Environment.NewLine +
                     "Error CNF-146 in " + FORM_NAME + ".GetFaxSubmitXML(): " + ex.Message);
            }
            return xmlText;
        }

        private string GetSubmitActionUrlText(string AXmitRequestId, string AAction)
        {
            string urlText = Properties.Settings.Default.TransmissionGatewayCallbackUrl;
            try
            {
                urlText += "?xmitRequestId=" + AXmitRequestId;
                urlText += "&action=" + AAction;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while attempting to retrieve the Transmission Gateway URL text using the following values:" + Environment.NewLine +
                    "XMit Request Id: " + AXmitRequestId + ", Action: " + AAction + Environment.NewLine +
                     "Error CNF-147 in " + FORM_NAME + ".GetSubmitActionUrlText(): " + ex.Message);
            }
            return urlText;
        }

        //private string GetSubmitActionUrlText(string AAction, string ATradeId, string ARqmtId, string AConfirmId,
        //        string AFaxTelexInd, string AFaxTelexNumber, string AConfirmLabel, string ARequestId)
        //{
        //    string urlText = Properties.Settings.Default.TransmissionGatewayCallbackUrl;
        //    try
        //    {
        //        string addr = "";
        //        for (int i = 0; i < AFaxTelexNumber.Length; i++)
        //            if (AFaxTelexNumber[i] != ' ')
        //                addr += AFaxTelexNumber[i];

        //        urlText += "?action=" + AAction;
        //        urlText += "&tradeId=" + ATradeId;
        //        urlText += "&tradeRqmtId=" + ARqmtId;
        //        urlText += "&tradeRqmtConfirmId=" + AConfirmId;
        //        urlText += "&faxTelexInd=" + AFaxTelexInd;
        //        urlText += "&addr=" + addr;
        //        urlText += "&label=" + AConfirmLabel;
        //        //Israel 10/6/2015
        //        //urlText += "&sender=" + toolbarOrWindowsUserId.ToUpper();
        //        urlText += "&sender=" + Utils.GetUserNameWithoutDomain(toolbarOrWindowsUserId.ToUpper());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("GetSubmitActionUrlText: " + ex.Message);
        //    }
        //    return urlText;
        //}

        //Israel 11/13/2015 -- Removed as part of move to XmitRequest/XmitResult
        //private void CallInsertToFaxLogSent(long ATradeId, string AFaxTelexInd, string AFaxTelexNumber,
        //   string ADocRefCode)
        //{
        //    try
        //    {
        //        FaxLogSentDto faxLogSentDto = new FaxLogSentDto();
        //        faxLogSentDto.TradeId = Convert.ToInt32(ATradeId);
        //        faxLogSentDto.DocType = DOC_TYPE[(int)DocType.CNF];
        //        faxLogSentDto.Sender = p_UserId;

        //        FaxCode faxCode;
        //        if (AFaxTelexNumber.IndexOf("@") > -1)
        //            faxCode = FaxCode.EMAIL;
        //        else if (AFaxTelexInd == "F")
        //            faxCode = FaxCode.FAX;
        //        else if (AFaxTelexInd == "T")
        //            faxCode = FaxCode.TELEX;
        //        else
        //            throw new Exception("Internal Exception: Unsupported FaxTelexInd=" + AFaxTelexInd);                

        //        faxLogSentDto.FaxTelexCode = FAX_CODE[(int)faxCode];
        //        faxLogSentDto.FaxTelexNumber = AFaxTelexNumber;
        //        faxLogSentDto.DocRefCode = ADocRefCode;

        //        FaxLogSentDal faxLogSentDal = new FaxLogSentDal(sqlConnectionStr);
        //        faxLogSentDal.Insert(faxLogSentDto);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("CallInsertToFaxLogSent: " + ex.Message);
        //    }
        //}

        private void SaveRtfAsPdfDoc(string ARtfFileName, string APdfFileName)
        {
            try
            {
                SautinSoft.PdfMetamorphosis p = new SautinSoft.PdfMetamorphosis();

                //specify Metamorphosis options
                p.Serial = PDF_METAMORPHOSIS_SERIAL;
                p.PageStyle.PageOrientation.Portrait();
                //p.TextStyle.Header = @"Sample header";
                //p.PageStyle.PageNumFormat = "Page {page} of {numpages}";

                if (p != null)
                {
                    int result = p.RtfToPdfConvertFile(ARtfFileName, APdfFileName);
                    //if (result != 0)
                    //throw new Exception("Pdf conversion error");
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while saving a confirm RTF document as a PDF using the following values:" + Environment.NewLine +
                    "RTF File Name: " + ARtfFileName + ", PDF File Name: " + APdfFileName + Environment.NewLine +
                      "Error CNF-148 in " + FORM_NAME + ".SaveRtfAsPdfDoc(): " + ex.Message,
                    MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendContractForCurrentRqmt(Int32 ATradeId)
        {
            try
            {
                string cptySn = "";
                string tradeSysTicket = "";
                string filterStr = "TradeId = " + ATradeId.ToString();
                foreach (DataRow summaryRow in summaryDataTable.Select(filterStr))
                {
                    cptySn = summaryRow["CptySn"].ToString();
                    tradeSysTicket = summaryRow["TradeSysTicket"].ToString();
                }
                string ticketNo = ATradeId.ToString();
                string transMethodInd = "";
                string faxTelexNumber = "";
                string activeFlag = "";
                Int32 rqmtId = 0;
                Int32 confirmId = 0;
                //Int32 templateId = 0;

                string confirmLabel = "";
                string filterStr2 = "TradeId = " + ticketNo;
                foreach (DataRow row in confirmDataTable.Select(filterStr2))
                {
                    confirmLabel = row["ConfirmLabel"].ToString();

                    //Israel 11/19/2015 -- Added active flag test to prevent GUI-driven errors
                    activeFlag = row["ActiveFlag"].ToString();
                    if (confirmLabel == CONFIRM_LABEL_CONFIRM && activeFlag == "Y")
                    {
                        confirmId = Int32.Parse(row["Id"].ToString());
                        rqmtId = Int32.Parse(row["RqmtId"].ToString());
                        //templateId = Int32.Parse(row["TemplateId"].ToString());
                        transMethodInd = row["FaxTelexInd"].ToString();
                        faxTelexNumber = row["FaxTelexNumber"].ToString();
                        break;
                    }
                }

                //string contractBody = GetContractFromVault(confirmId, 0);
                TradeRqmtConfirmBlobDal trcBlobDal = new TradeRqmtConfirmBlobDal(sqlConnectionStr);
                TradeRqmtConfirmBlobDto trcBlobData = new TradeRqmtConfirmBlobDto();
                //Int64 confirmIdLong = Convert.ToInt64(confirmId);
                trcBlobData = trcBlobDal.Get(confirmId);
                DocumentFormat docFormat = Utils.GetDocumentFormat(trcBlobData.ImageFileExt);

                using (var ts = new TransactionScope())
                {
                    Vaulter vaulter = new Vaulter(sqlConnectionStr);
                    vaulter.VaultTradeRqmtConfirm(confirmId, null);

                    if (transMethodInd.Length > 0 && faxTelexNumber.Length > 2)
                        SendToFaxGateway(transMethodInd, faxTelexNumber, CONFIRM_LABEL_CONFIRM, trcBlobData.DocBlob, docFormat,
                              cptySn, ATradeId,tradeSysTicket, rqmtId, confirmId, false, "", "", "", false);

                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while preparing a Confirm to send to the fax gateway for the following Trade Id:" + ATradeId + Environment.NewLine +
                     "Error CNF-149 in " + FORM_NAME + ".SendContractForCurrentRqmt(): " + ex.Message);
            }
        }

        private int GetGridViewFocusedRowHandle(GridView AView)
        {
            try
            {
                int rowHandle = 0;
                if (AView.FocusedRowHandle == DevExpress.XtraGrid.GridControl.AutoFilterRowHandle)
                    rowHandle = 0;
                else
                    rowHandle = AView.FocusedRowHandle;
                return rowHandle;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while setting the current selected row for the following Grid:" + AView.Name + Environment.NewLine +
                     "Error CNF-150 in " + FORM_NAME + ".GetGridViewFocusedRowHandle(): " + ex.Message);
            }
        }

        #endregion

        #region Confirmation Event Handlers

        private void barbtnEditRqmt_ItemClick(object sender, ItemClickEventArgs e)
        {
            EditSelectedRequirement();
        }

        public string EditSelectedRequirement()
        {
            string rqmtStatusCode = "";
            try
            {
                //Setup...beginload prevents data entry event handlers from firing.
                myTimer.Stop(); //Israel 12/15/2008 - Red X
                editRqmtForm.BeginDataLoad();
                editRqmtForm.ClearAllFields();
                editRqmtForm.InitOldDataVariables();
                editRqmtForm.gluedSempraStatus.Enabled = true;  //sometimes gets set to false below...
                editRqmtForm.gluedSempraStatus.ForeColor = Color.Black;

                bool[] tabs = new bool[frmEditRqmt.RQMT_ARRAY_MAX];
                for (int i = 0; i < frmEditRqmt.RQMT_ARRAY_MAX; i++)
                    tabs[i] = false;
                string tradingSys = "***";
                string ticket = "***";
                string tradeId = "***";
                string cptySn = "***";
                string bookingCoSn = "***";
                string cptyTradeId = "";
                string tradeCmt = "";
                string rqmtCode = "";

                GridView view = gridViewSummary;
                GridView view2 = gridViewRqmt;
                int rowHandle = GetGridViewFocusedRowHandle(view);

                //Prepare to call form for single row selected.
                //Single row loads data and user performs standard data entry.
                editRqmtForm.SingleOrMultiMode = frmEditRqmt.SINGLE;
                //Setup Header data
                tradingSys = view.GetRowCellDisplayText(rowHandle, "TrdSysCode").ToString();
                ticket = view.GetRowCellDisplayText(rowHandle, "TradeSysTicket").ToString();
                tradeId = view.GetRowCellDisplayText(rowHandle, "TradeId").ToString();
                cptySn = view.GetRowCellDisplayText(rowHandle, "CptySn").ToString();
                bookingCoSn = view.GetRowCellDisplayText(rowHandle, "BookingCoSn").ToString();

                int rqmtHandle = view2.FocusedRowHandle;
                rqmtCode = view2.GetRowCellDisplayText(rqmtHandle, "Rqmt").ToString();
                int rqmtId = Int32.Parse(view2.GetRowCellDisplayText(rqmtHandle, "Id").ToString());

                if (rqmtCode == "XQCCP")
                    tabs[frmEditRqmt.RQMT_TYPE_CPTY] = true;
                else if (rqmtCode == "XQBBP")
                    tabs[frmEditRqmt.RQMT_TYPE_BROKER] = true;
                else if (rqmtCode == "NOCNF")
                    tabs[frmEditRqmt.RQMT_TYPE_NOCONF] = true;
                //else if (rqmtCode == "ECONF")
                //    tabs[frmEditRqmt.RQMT_TYPE_ECONFIRM] = true;
                //else if (rqmtCode == "ECBKR")
                //    tabs[frmEditRqmt.RQMT_TYPE_ECONFIRM_BROKER] = true;
                //else if (rqmtCode == "EFET")
                //    tabs[frmEditRqmt.RQMT_TYPE_EFET_CPTY] = true;
                //else if (rqmtCode == "EFBKR")
                //    tabs[frmEditRqmt.RQMT_TYPE_EFET_BROKER] = true;
                else if (rqmtCode == "VBCP")
                    tabs[frmEditRqmt.RQMT_TYPE_VERBAL] = true;
                else if (rqmtCode == "XQCSP")
                    tabs[frmEditRqmt.RQMT_TYPE_SEMPRA] = true;
                //else if (rqmtCode == "MISC")
                //tabs[frmEditRqmt.RQMT_TYPE_MISC] = true;

                editRqmtForm.SetTabsVisible(tabs);
                int rqmtType = 0;
                for (int i = 0; i < frmEditRqmt.RQMT_ARRAY_MAX; i++)
                    if (tabs[i])
                        rqmtType = i;

                string rqmtStatus = view2.GetRowCellDisplayText(rqmtHandle, "Status").ToString();
                DateTime rqmtStatusDate = DateTime.Parse(view2.GetRowCellDisplayText(rqmtHandle, "CompletedDt").ToString());
                bool secondChk = view2.GetRowCellDisplayText(rqmtHandle, "SecondCheckFlag").ToString() == "Y";
                string rqmtRef = view2.GetRowCellDisplayText(rqmtHandle, "Reference").ToString();
                string rqmtCmt = view2.GetRowCellDisplayText(rqmtHandle, "Cmt").ToString();
                //string prelimApprover = view2.GetRowCellDisplayText(rqmtHandle, "PrelimAppr").ToString();

                editRqmtForm.SetRqmtData(rqmtType, rqmtStatus, rqmtStatusDate,
                             secondChk, rqmtRef, rqmtCmt);

                //Setup cpty trade Id and trade comment
                //Israel 12/23/2015 -- Added support for CptyTradeId
                cptyTradeId = view.GetRowCellDisplayText(view.FocusedRowHandle, "CptyTradeId").ToString();
                tradeCmt = view.GetRowCellDisplayText(view.FocusedRowHandle, "Cmt").ToString();
                editRqmtForm.SetCptyTradeIdEnabled(true);
                editRqmtForm.SetTradeData(cptyTradeId, tradeCmt);

                editRqmtForm.SetHeaderLabels(tradingSys, ticket, cptySn, bookingCoSn);
                editRqmtForm.EndDataLoad();

                if (editRqmtForm.ShowDialog(this) == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    //Get back the data that was entered
                    bool[] updatedRqmts = new bool[frmEditRqmt.RQMT_ARRAY_MAX];
                    string[] updatedStatusCodes = new string[frmEditRqmt.RQMT_ARRAY_MAX];
                    DateTime[] updatedStatusDates = new DateTime[frmEditRqmt.RQMT_ARRAY_MAX];
                    //string[] updatedSecondChecks = new string[frmEditRqmt.RQMT_ARRAY_MAX];
                    string[] updatedReferences = new string[frmEditRqmt.RQMT_ARRAY_MAX];
                    string[] updatedRqmtCmts = new string[frmEditRqmt.RQMT_ARRAY_MAX];
                    string updatedCptyTradeId;
                    string updatedTradeCmt;
                    //bool[,] changedFields = new bool[frmEditRqmt.RQMT_ARRAY_MAX, frmEditRqmt.FIELD_ARRAY_MAX];
                    bool isCptyTradeIdChanged = false;
                    bool isTradeCmtChanged = false;

                    updatedRqmts = editRqmtForm.GetUpdatedRqmts();
                    updatedStatusCodes = editRqmtForm.GetStatusCodes();
                    updatedStatusDates = editRqmtForm.GetStatusDates();
                    //updatedSecondChecks = editRqmtForm.GetSecondChecks();
                    updatedReferences = editRqmtForm.GetReferences();
                    updatedRqmtCmts = editRqmtForm.GetRqmtCmts();
                    updatedCptyTradeId = editRqmtForm.GetCptyTradeId();
                    updatedTradeCmt = editRqmtForm.GetTradeCmt();
                    //changedFields = editRqmtForm.GetChangedFields();
                    isCptyTradeIdChanged = editRqmtForm.IsCptyTradeIdChanged();
                    isTradeCmtChanged = editRqmtForm.IsTradeCmtChanged();

                    //Find out if any requirements need to be updated.
                    bool isAnyRqmtsUpdated = false;
                    for (int i = 0; i < frmEditRqmt.RQMT_ARRAY_MAX; i++)
                        if (updatedRqmts[i])
                            isAnyRqmtsUpdated = true;

                    //Call the routine that invokes the update procedure.
                    if (isAnyRqmtsUpdated)
                    {
                        CallUpdateTradeRqmts(Int32.Parse(tradeId), rqmtId, rqmtCode, updatedStatusCodes[rqmtType],
                           updatedStatusDates[rqmtType], updatedReferences[rqmtType], updatedRqmtCmts[rqmtType],
                           true);
                        rqmtStatusCode = updatedStatusCodes[rqmtType];
                    }

                    //Israel 12/23/2015 -- Added support for CptyTradeId
                    if (isCptyTradeIdChanged)
                        CallUpdateCptyTradeId(Int32.Parse(tradeId), updatedCptyTradeId);

                    if (isTradeCmtChanged)
                        CallUpdateTradeCmt(updatedTradeCmt);

                    if (isForceFinalApprove && editRqmtForm.isFinalApprove)
                        CallUpdateFinalApproval("Y", "N");
                }
                else
                {
                    rqmtStatusCode = "CANCELLED";
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while updating the selected requirement." + Environment.NewLine +
                      "Error CNF-151 in " + FORM_NAME + ".EditSelectedRequirement(): " + ex.Message,
                    MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start(); //Israel 12/15/2008 - Red X
                Cursor.Current = Cursors.Default;
            }
            return rqmtStatusCode;
        }

        private void barBtnSelectManualTemplate_ItemClick(object sender, ItemClickEventArgs e)
        {
            GetManualContract(false);
        }

        private void barBtnAddEditConfirm_ItemClick(object sender, ItemClickEventArgs e)
        {
            GetAutoContract(gridViewRqmt);
        }

        private void barbtnGetContract_ItemClick(object sender, ItemClickEventArgs e)
        {
            getContractForBrowserAppTab();
        }

        private void getContractForBrowserAppTab()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Int32 tradeRqmtConfirmId = 0;
                GridView view = gridViewSummary;
                Int32 rowHandle = GetGridViewFocusedRowHandle(view);
                if (view.SelectedRowsCount == 1)
                {
                    var ATradeId = view.GetRowCellDisplayText(rowHandle, "TradeId").ToString();
                    if (gridViewConfirm.SelectedRowsCount == 1)
                    {
                        tradeRqmtConfirmId =
                           Int32.Parse(gridViewConfirm.GetRowCellDisplayText(gridViewConfirm.FocusedRowHandle, "Id").ToString());
                        cedTradingSystem.Text = view.GetRowCellDisplayText(rowHandle, "TrdSysCode").ToString();
                        btnedTradeId.Text = view.GetRowCellDisplayText(rowHandle, "TradeId").ToString();

                        //5/21/09 Israel - Add support for fax gateway emailing.
                        Int32 rqmtId = int.Parse(gridViewConfirm.GetRowCellDisplayText(gridViewConfirm.FocusedRowHandle, "RqmtId").ToString());
                        string contractStatus = GetTradeRqmtData(rqmtId, "Status");
                        bool okToEmail = (contractStatus == "OK_TO_SEND" || contractStatus == "SENT" || contractStatus == "FAIL" ||
                                          contractStatus == "DISP" || contractStatus == "RESNT");
                        //Israel 9/21/2015
                        //DisplayContract(contractBody, okToEmail);
                        if (contractStatus != "NEW" && contractStatus != "CXL")
                            DisplayContract(tradeRqmtConfirmId, okToEmail);

                        viewContractTradeId = Int32.Parse(btnedTradeId.Text);
                        viewContractConfirmId = tradeRqmtConfirmId;
                        //viewContractTemplateId = int.Parse(gridViewConfirm.GetRowCellDisplayText(gridViewConfirm.FocusedRowHandle, "TemplateId").ToString());
                    }
                    else
                    {
                        XtraMessageBox.Show("No contract found for trade: " + " " + ATradeId.ToString() + ".",
                           "Contract Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while retrieving the Confirm the selected trade." + Environment.NewLine +
                     "Error CNF-152 in " + FORM_NAME + ".getContractForBrowserAppTab(): " + ex.Message,
                   MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //dataManager.StartOpsTimer();
                this.Cursor = Cursors.Default;
            }
        }

        private void barBtnCancelAdditonalConfirm_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myTimer.Stop();
                DialogResult result = XtraMessageBox.Show("Cancel Confirm?", "Cancel Confirm",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (gridViewSummary.SelectedRowsCount == 1)
                    {
                        GridView view = gridViewConfirm;
                        if (view.SelectedRowsCount == 1)
                        {
                            Int32 tradeRqmtConfirmId =
                               Int32.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "Id").ToString());
                            Int32 tradeId = Int32.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeId").ToString());
                            Int32 rqmtId = Int32.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "RqmtId").ToString());
                            //Int32 templateId = Int32.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "TemplateId").ToString());
                            string templateName = view.GetRowCellDisplayText(view.FocusedRowHandle, "TemplateName").ToString();
                            string faxTelexInd = view.GetRowCellDisplayText(view.FocusedRowHandle, "FaxTelexInd").ToString();
                            string faxTelexNumber = view.GetRowCellDisplayText(view.FocusedRowHandle, "FaxTelexNumber").ToString();
                            string confirmLabel = view.GetRowCellDisplayText(view.FocusedRowHandle, "ConfirmLabel").ToString();
                            string confirmCmt = view.GetRowCellDisplayText(view.FocusedRowHandle, "ConfirmCmt").ToString();
                            string activeFlag = view.GetRowCellDisplayText(view.FocusedRowHandle, "ActiveFlag").ToString();

                            UpdateTradeRqmtConfirmRow(tradeRqmtConfirmId, tradeId, rqmtId, templateName, faxTelexInd,
                               faxTelexNumber, confirmLabel, confirmCmt, "CXL", activeFlag);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to cancel the currently selected non-Confirm document." + Environment.NewLine +
                     "Error CNF-153 in " + FORM_NAME + ".barBtnCancelAdditonalConfirm_ItemClick(): " + ex.Message,
                   MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start();
                this.Cursor = Cursors.Default;
            }
        }

        private void barBtnEditConfirm_ItemClick(object sender, ItemClickEventArgs e)
        {
            GridView view = gridViewConfirm;
            GetAutoContract(view);
        }

        private void SendPaper_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myTimer.Stop();
                DialogResult result = XtraMessageBox.Show("Send Paper?", "Send Paper",
                  MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        GridView view = gridViewConfirm;

                        //Israel 11/19/2015 -- Replaced by Enable logic at gridViewConfirm_ShowGridMenu 
                        //string activeFlag = view.GetRowCellDisplayText(view.FocusedRowHandle, "ActiveFlag");
                        //if (activeFlag != "Y")
                        //{
                        //    XtraMessageBox.Show("Please select an active row.", "Send Paper", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //    return;
                        //}

                        Int32 confirmId = Int32.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "Id"));
                        int rowHandle = GetGridViewFocusedRowHandle(gridViewSummary);
                        string cptySn = gridViewSummary.GetRowCellDisplayText(rowHandle, "CptySn").ToString();
                        using (var ts = new TransactionScope())
                        {
                            Vaulter vaulter = new Vaulter(sqlConnectionStr);
                            vaulter.VaultTradeRqmtConfirm(confirmId, null);

                            SendToFaxGateway(confirmId, cptySn, false, "", "", false);

                            ts.Complete();
                        }
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show("An error occurred while attempting to Send a Confirm Document." + Environment.NewLine +
                            "Error CNF-154 in " + FORM_NAME + ".SendPaper_ItemClick(): " + ex.Message,
                          MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            finally
            {
                myTimer.Start();
            }
        }

        private void ResendPaper_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myTimer.Stop();
                DocSendType docSendType;
                if (e.Item.Name == barBtnConfirmResendPaper.Name)
                    docSendType = DocSendType.PDF;
                else if (e.Item.Name == barBtnConfirmSendResendRTF.Name)
                    docSendType = DocSendType.RTF;
                else
                    throw new Exception("Internal Error: Invalid calling method: " + e.Item.Name);

                bool isRtf = false;
                string label = "";
                if (docSendType == DocSendType.PDF)
                    label = "Resend Paper";
                else if (docSendType == DocSendType.RTF)
                {
                    label = "Send/Resend RTF";
                    isRtf = true;
                }

                DialogResult result = XtraMessageBox.Show(label + "?", label,
                   MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    GridView view = gridViewConfirm;
                    Int32 confirmId = Int32.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "Id"));
                    int rowHandle = GetGridViewFocusedRowHandle(gridViewSummary);
                    string cptySn = gridViewSummary.GetRowCellDisplayText(rowHandle, "CptySn").ToString();

                    using (var ts = new TransactionScope())
                    {
                        Vaulter vaulter = new Vaulter(sqlConnectionStr);
                        vaulter.VaultTradeRqmtConfirm(confirmId, null);

                        SendToFaxGateway(confirmId, cptySn, isRtf, "", "", false);

                        ts.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to Resend a Confirm Document." + Environment.NewLine +
                    "Error CNF-155 in " + FORM_NAME + ".ResendPaper_ItemClick(): " + ex.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start();
            }
        }

        private void ChangeFaxNoSendResend(object sender, ItemClickEventArgs e)
        {
            try
            {
                //5/20/09 Israel - Menu item and handler created to allow change fax number and resend.
                //Get fax number data and send to form
                myTimer.Stop();
                int rowHandle = GetGridViewFocusedRowHandle(gridViewSummary);
                string cptySn = gridViewSummary.GetRowCellDisplayText(rowHandle, "CptySn").ToString();
                string cdtyCode = gridViewSummary.GetRowCellDisplayText(rowHandle, "CdtyCode").ToString();
                string sttlType = gridViewSummary.GetRowCellDisplayText(rowHandle, "SttlType").ToString();

                //Deprecated Israel 6/15/2015
                string oldFaxNo = "";

                //Israel 9/18/2015
                //string[] getCptyFaxNoResult = GetCptyFaxNo(cptySn, cdtyCode, sttlType);
                string newFaxNo = "none@domain_name.com";
                string transMethod = "EMAIL";
                //if (getCptyFaxNoResult[0] != null)
                //    transMethod = getCptyFaxNoResult[0];
                //if (getCptyFaxNoResult[1] != null)
                //    newFaxNo = getCptyFaxNoResult[1];

                GridView view = gridViewConfirm;
                string savedTransInd = view.GetRowCellDisplayText(view.FocusedRowHandle, "FaxTelexInd").ToString();
                string savedFaxNumber = view.GetRowCellDisplayText(view.FocusedRowHandle, "FaxTelexNumber").ToString();
                string savedTransMethod = GetFaxMethodFromInd(savedTransInd);

                frmAssignFaxNo assignFaxNoForm = new frmAssignFaxNo();
                assignFaxNoForm.SetFaxNumbers(newFaxNo, savedFaxNumber);
                if (assignFaxNoForm.ShowDialog(this) == DialogResult.OK)
                {
                    string liveFaxNumber = assignFaxNoForm.teditFaxTelexNumber.Text;
                    //string liveTransMethod = "FAX";
                    string liveTransMethod = liveFaxNumber.Contains("@") ? liveTransMethod = "EMAIL" : liveTransMethod = "FAX";
                    string liveTransInd = GetFaxIndFromMethod(liveTransMethod);


                    Int32 confirmId = Int32.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "Id"));
                    Int32 tradeId = Int32.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeId"));
                    Int32 rqmtId = Int32.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "RqmtId"));
                    //Int32 templateId = Int32.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "TemplateId"));
                    string templateName = view.GetRowCellDisplayText(view.FocusedRowHandle, "TemplateName");
                    string confirmLabel = view.GetRowCellDisplayText(view.FocusedRowHandle, "ConfirmLabel");
                    string cmt = view.GetRowCellDisplayText(view.FocusedRowHandle, "ConfirmCmt");
                    string nextStatusCode = view.GetRowCellValue(view.FocusedRowHandle, "NextStatusCode").ToString();
                    string activeFlag = view.GetRowCellValue(view.FocusedRowHandle, "ActiveFlag").ToString();

                    if (liveFaxNumber.Length > 2)
                    {
                        using (var ts = new TransactionScope())
                        {
                            UpdateTradeRqmtConfirmRow(confirmId, tradeId, rqmtId, templateName,
                               liveTransInd, liveFaxNumber, confirmLabel, cmt, nextStatusCode, activeFlag);

                            Vaulter vaulter = new Vaulter(sqlConnectionStr);
                            vaulter.VaultTradeRqmtConfirm(confirmId, null);

                            SendToFaxGateway(confirmId, cptySn, false, "", "SENT", false);

                            ts.Complete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to 'Change Send-To Address & Send/Resend' a Confirm Document." + Environment.NewLine +
                    "Error CNF-156 in " + FORM_NAME + ".ChangeFaxNoSendResend(): " + ex.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start();
            }
        }

        private void CreateAdditionalConfirm_ItemClick(object sender, ItemClickEventArgs e)
        {
            GetManualContract(true);
        }

        private void SetToNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myTimer.Stop();
                DialogResult result = XtraMessageBox.Show("Set to New?", "Set to New",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        GridView view;
                        Int32 confirmId = 0;
                        Int32 tradeId = 0;
                        Int32 rqmtId = 0;
                        //if (e.Item.Name == barbtnRqmtSetToNew.Name)
                        //{

                        using (var ts = new TransactionScope())
                        {

                            view = gridViewRqmt;
                            tradeId = Int32.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeId").ToString());
                            confirmId = GetContractRowId(tradeId);
                            rqmtId = Int32.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "Id").ToString());

                            //ChangeRqmtStatus(view, SEMPRA_RQMT, "NEW", "");
                            string reference = GetTradeRqmtData(rqmtId, "Reference");
                            string rqmtStatus = GetTradeRqmtData(rqmtId, "Status");

                            bool hasBeenSent = false;
                            TradeAuditDal tradeAuditDal = new TradeAuditDal(sqlConnectionStr);
                            hasBeenSent = tradeAuditDal.HasConfirmBeenSent(tradeId);
                            string activeFlag = (hasBeenSent == true) ? "N" : "Y";
                            CallUpdateTradeRqmts(tradeId, rqmtId, SEMPRA_RQMT, "NEW", DateTime.Today, reference, "", true);

                            //without confirmId test inserts rows when none exist.
                            if (confirmId > 0)
                            {
                                string templateName = GetConfirmData(confirmId, "TemplateName");
                                string faxTelexInd = GetConfirmData(confirmId, "FaxTelexInd");
                                string faxTelexNumber = GetConfirmData(confirmId, "FaxTelexNumber");
                                UpdateTradeRqmtConfirmRow(confirmId, tradeId,
                                   rqmtId, templateName, faxTelexInd, faxTelexNumber, CONFIRM_LABEL_CONFIRM, "RESET TO NEW", "CXL", activeFlag);

                                if (hasBeenSent)
                                    UpdateTradeRqmtConfirmRow(0, tradeId,
                                       rqmtId, templateName, faxTelexInd, faxTelexNumber, CONFIRM_LABEL_CONFIRM, "RESET TO NEW", "PREP", "Y");

                                //string filterStr = "Id = " + confirmId.ToString() + " and ActiveFlag = 'Y'";
                                //foreach (DataRow row in confirmDataTable.Select(filterStr))
                                //{
                                //    row.Delete();
                                //    row.AcceptChanges();
                                //}

                            }

                            ts.Complete();
                        }
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show("An error occurred while resetting an Our Paper requirement status to NEW." + Environment.NewLine +
                            "Error CNF-157 in " + FORM_NAME + ".SetToNew_ItemClick(): " + ex.Message,
                          MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            finally
            {
                myTimer.Start();
            }
        }

        private void SetToPrep_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myTimer.Stop();
                DialogResult result = XtraMessageBox.Show("Set to Prep?", "Set to Prep",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        GridView view = gridViewRqmt;
                        Int32 tradeId = Int32.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeId").ToString());
                        Int32 confirmId = GetContractRowId(tradeId);
                        Int32 rqmtId = Int32.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "Id").ToString());

                        //int templateId = GetContractTemplateId(tradeId);
                        string templateName = GetContractConfirmData(tradeId, "TemplateName");
                        string faxTelexInd = GetContractConfirmData(tradeId, "FaxTelexInd");
                        string faxTelexNumber = GetContractConfirmData(tradeId, "FaxTelexNumber");

                        //ChangeRqmtStatus(view, SEMPRA_RQMT, "PREP", "");
                        string reference = GetTradeRqmtData(rqmtId, "Reference");
                        CallUpdateTradeRqmts(tradeId, rqmtId, SEMPRA_RQMT, "PREP", DateTime.Today, reference, "", true);

                        if (confirmId > 0)
                            UpdateTradeRqmtConfirmRow(Convert.ToInt32(confirmId), tradeId,
                               Convert.ToInt32(rqmtId), templateName, faxTelexInd, faxTelexNumber, CONFIRM_LABEL_CONFIRM, "RESET TO PREP", "PREP", "Y");
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show("An error occurred while resetting an Our Paper requirement status to PREP." + Environment.NewLine +
                            "Error CNF-158 in " + FORM_NAME + ".SetToPrep_ItemClick(): " + ex.Message,
                          MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            finally
            {
                myTimer.Start();
            }
        }

        private void Cancel_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myTimer.Stop();
                try
                {
                    cancelRqmtForm.settingsDir = this.appSettingsDir;
                    cancelRqmtForm.tedComment.Text = "";
                    if (cancelRqmtForm.ShowDialog(this) == DialogResult.OK)
                    {
                        GridView view = gridViewRqmt;
                        string cmt = cancelRqmtForm.tedComment.Text;
                        Int32 tradeId = Int32.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeId").ToString());
                        Int32 confirmId = GetContractRowId(tradeId);
                        Int32 rqmtId = Int32.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "Id").ToString());

                        //ChangeRqmtStatus(view, SEMPRA_RQMT, "CXL", cmt);
                        string reference = GetTradeRqmtData(rqmtId, "Reference");
                        CallUpdateTradeRqmts(tradeId, rqmtId, SEMPRA_RQMT, "CXL", DateTime.Today, reference, cmt, true);

                        if (confirmId > 0)
                            UpdateTradeRqmtConfirmRow(Convert.ToInt32(confirmId), tradeId,
                               Convert.ToInt32(rqmtId), "", "", "", CONFIRM_LABEL_CONFIRM, "CANCEL", "CXL", "Y");
                    }
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("An error occurred while setting an Our Paper requirement status to CANCEL." + Environment.NewLine +
                        "Error CNF-159 in " + FORM_NAME + ".Cancel_ItemClick(): " + ex.Message,
                      MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                myTimer.Start();
            }
        }

        private void ManualSend_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myTimer.Stop();
                DialogResult result = XtraMessageBox.Show("Manual Send Contract?", "Manual Send Contract",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        GridView view;
                        Int32 rqmtId = 0;
                        Int32 tradeId = 0;
                        if (e.Item.Name == barbtnRqmtManualSend.Name)
                        {
                            view = gridViewRqmt;
                            tradeId = Int32.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeId").ToString());
                            rqmtId = Int32.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "Id").ToString());
                            string rqmtCmt = view.GetRowCellDisplayText(view.FocusedRowHandle, "Cmt").ToString();
                            //ChangeRqmtStatus(view, SEMPRA_RQMT, "SENT", rqmtCmt);
                            string reference = GetTradeRqmtData((int)rqmtId, "Reference");
                            CallUpdateTradeRqmts(tradeId, rqmtId, SEMPRA_RQMT, "SENT", DateTime.Today, reference, "Manual Send " + rqmtCmt, true);
                        }
                        else
                        {
                            view = gridViewConfirm;
                            Int32 tradeRqmtConfirmId =
                               Int32.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "Id").ToString());
                            tradeId = Int32.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeId").ToString());
                            rqmtId = Int32.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "RqmtId").ToString());
                            //long templateId = long.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "TemplateId").ToString());
                            string templateName = view.GetRowCellDisplayText(view.FocusedRowHandle, "TemplateName").ToString();
                            string faxTelexInd = view.GetRowCellDisplayText(view.FocusedRowHandle, "FaxTelexInd").ToString();
                            string faxTelexNumber = view.GetRowCellDisplayText(view.FocusedRowHandle, "FaxTelexNumber").ToString();
                            string confirmLabel = view.GetRowCellDisplayText(view.FocusedRowHandle, "ConfirmLabel").ToString();
                            string confirmCmt = view.GetRowCellDisplayText(view.FocusedRowHandle, "ConfirmCmt").ToString();
                            string activeFlag = view.GetRowCellDisplayText(view.FocusedRowHandle, "ActiveFlag").ToString();

                            using (var ts = new TransactionScope())
                            {
                                UpdateTradeRqmtConfirmRow(tradeRqmtConfirmId, tradeId, rqmtId, templateName, faxTelexInd,
                                   faxTelexNumber, confirmLabel, confirmCmt, "SENT", activeFlag);

                                if (confirmLabel == CONFIRM_LABEL_CONFIRM)
                                {
                                    string rqmtCmt2 = GetTradeRqmtData((int)rqmtId, "Cmt");
                                    //ChangeRqmtStatus(view, SEMPRA_RQMT, "SENT", rqmtCmt2);
                                    string reference = GetTradeRqmtData((int)rqmtId, "Reference");
                                    CallUpdateTradeRqmts(tradeId, rqmtId, SEMPRA_RQMT, "SENT", DateTime.Today, reference, "Manual Send " + rqmtCmt2, true);
                                }

                                ts.Complete();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show("An error occurred while requesting 'Manual Send' a Confirm." + Environment.NewLine +
                            "Error CNF-160 in " + FORM_NAME + ".ManualSend_ItemClick(): " + ex.Message,
                          MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            finally
            {
                myTimer.Start();
            }
        }

        private void SetToOkToSend_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myTimer.Stop();
                DialogResult result = XtraMessageBox.Show("Set to Ok To Send?", "Set to Ok To Send",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        Int32 tradeId = Int32.Parse(gridViewSummary.GetRowCellDisplayText(gridViewSummary.FocusedRowHandle, "TradeId").ToString());
                        RqmtData rqmt = GetSempraRqmtObj(tradeId, SEMPRA_RQMT);
                        CallUpdateTradeRqmts(tradeId, rqmt.Id, SEMPRA_RQMT, "OK_TO_SEND", DateTime.Today, rqmt.Reference, "", true);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show("An error occurred while setting an Our Paper requirement status to OK TO SEND." + Environment.NewLine +
                            "Error CNF-161 in " + FORM_NAME + ".SetToOkToSend_ItemClick(): " + ex.Message,
                          MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            finally
            {
                myTimer.Start();
            }
        }

        private RqmtData GetSempraRqmtObj(long tradeId, string rqmtCode)
        {
            RqmtData rqmt = new RqmtData();
            try
            {
                DataRow[] found = rqmtDataTable.Select("TradeId = " + tradeId + " and Rqmt = " + @"'" + rqmtCode + @"'");
                if (found.Length == 1)
                {
                    rqmt = CollectionHelper.CreateObjectFromDataRow<RqmtData>(found[0]);
                }
                else
                {
                    if (found.Length == 0)
                    {
                        throw new Exception("Error CNF-533: No Our Paper Rqmt found for Trade Id: " + tradeId);
                    }
                    else if (found.Length > 1)
                    {
                        throw new Exception("Error CNF-534: Multiple Our Paper Rqmts found for Trade Id: " + tradeId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the internal rqmt data for the following values:" + Environment.NewLine +
                    "Trade Id: " + tradeId + ", Rqmt Code: " + rqmtCode + Environment.NewLine +
                    "Error CNF-333 in " + FORM_NAME + ".GetSempraRqmtObj(): " + ex.Message);
            }
            return rqmt;
        }

        private void SetToOkAndSend_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myTimer.Stop();
                DialogResult result = XtraMessageBox.Show("Set to Ok To Send & Send Contract?", "Set to Ok To Send & Send Contract",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        Int32 tradeId = Int32.Parse(gridViewSummary.GetRowCellDisplayText(gridViewSummary.FocusedRowHandle, "TradeId").ToString());
                        RqmtData rqmt = GetSempraRqmtObj(tradeId, SEMPRA_RQMT);
                        CallUpdateTradeRqmts(tradeId, rqmt.Id, SEMPRA_RQMT, "OK_TO_SEND", DateTime.Today, rqmt.Reference, "", true);
                        SendContractForCurrentRqmt(tradeId);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show("An error occurred while attempting to Set to Ok To Send & Send Contract." + Environment.NewLine +
                            "Error CNF-162 in " + FORM_NAME + ".SetToOkToSend_ItemClick(): " + ex.Message,
                          MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            finally
            {
                myTimer.Start();
            }
        }

        private void SetToOkAndManualSend_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myTimer.Stop();
                DialogResult result = XtraMessageBox.Show("Set to Ok To Send & Manual Send Contract?",
                   "Set to Ok To Send & Manual Send Contract",
                   MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (var ts = new TransactionScope())
                        {
                            Int32 tradeId = Int32.Parse(gridViewSummary.GetRowCellDisplayText(gridViewSummary.FocusedRowHandle, "TradeId").ToString());
                            RqmtData rqmt = GetSempraRqmtObj(tradeId, SEMPRA_RQMT);
                            CallUpdateTradeRqmts(tradeId, rqmt.Id, SEMPRA_RQMT, "OK_TO_SEND", DateTime.Today, rqmt.Reference, "", true);
                            CallUpdateTradeRqmts(tradeId, rqmt.Id, SEMPRA_RQMT, "SENT", DateTime.Today, rqmt.Reference, "Manual Send", true);

                            ts.Complete();
                        }
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show("An error occurred while attempting to Set to Ok To Send & Manual Send Contract." + Environment.NewLine +
                            "Error CNF-163 in " + FORM_NAME + ".SetToOkAndManualSend_ItemClick(): " + ex.Message,
                          MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            finally
            {
                myTimer.Start();
            }
        }

        private void Send_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myTimer.Stop();
                string mboxLabel = "Send Contract";
                if (e.Item.Name == barbtnRqmtResend.Name)
                    mboxLabel = "Resend Contract";

                DialogResult result = XtraMessageBox.Show(mboxLabel + "?", mboxLabel,
                   MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        Int32 rowHandle = GetGridViewFocusedRowHandle(gridViewSummary);
                        Int32 tradeId = Int32.Parse(gridViewSummary.GetRowCellDisplayText(rowHandle, "TradeId").ToString());
                        SendContractForCurrentRqmt(tradeId);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show("An error occurred while attempting to a Send Contract." + Environment.NewLine +
                            "Error CNF-164 in " + FORM_NAME + ".Send_ItemClick(): " + ex.Message,
                          MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            finally
            {
                myTimer.Start();
            }
        }

        private void Print_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Int32 tradeId = 0;
                Int32 confirmId = 0;
                GridView view;

                //Israel 9/21/2015
                string componentName = "btnPrint";
                if (e != null)
                    componentName = barbtnRqmtPrint.Name;

                //if (e.Item.Name == barbtnRqmtPrint.Name)
                if (componentName == barbtnRqmtPrint.Name)
                {
                    view = gridViewRqmt;
                    tradeId = Int32.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeId").ToString());
                    confirmId = GetContractRowId(tradeId);
                }
                else
                {
                    view = gridViewConfirm;
                    if (view.RowCount > 0)
                        confirmId = int.Parse(view.GetRowCellDisplayText(view.FocusedRowHandle, "Id").ToString());
                }

                //Israel 9/18/2015
                if (confirmId > 0)
                    ExecPrintFromEditRichContract(confirmId);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to Print a Confirm." + Environment.NewLine +
                    "Error CNF-165 in " + FORM_NAME + ".Print_ItemClick(): " + ex.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void ExecPrintFromEditRichContract(int pConfirmId)
        {
            TradeRqmtConfirmBlobDal trcBlobDal = new TradeRqmtConfirmBlobDal(sqlConnectionStr);
            TradeRqmtConfirmBlobDto trcBlobData = new TradeRqmtConfirmBlobDto();
            //Int64 confirmIdLong = Convert.ToInt64(pConfirmId);
            trcBlobData = trcBlobDal.Get(pConfirmId);
            DocumentFormat docFormat = Utils.GetDocumentFormat(trcBlobData.ImageFileExt);

            if (trcBlobData.DocBlob.Length > 0)
            {
                //byte[] docBytes = System.Text.Encoding.UTF8.GetBytes(AContractBody);
                using (MemoryStream contractStream = new MemoryStream(trcBlobData.DocBlob))
                {
                    editContractForm.richeditConfirm.LoadDocument(contractStream, docFormat);
                }
            }
            editContractForm.printItem1.PerformClick();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                myTimer.Stop();
                Print_ItemClick(null, null);
            }
            finally
            {
                myTimer.Start();
            }
        }

        private void printDocument_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            checkPrint = 0;
        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string test = "";
            // Print the content of RichTextBox. Store the last character printed.
            //checkPrint = rtfContract.Print(checkPrint, rtfContract.TextLength, e);

            // Check for more pages
            //if (checkPrint < rtfContract.TextLength)
            //    e.HasMorePages = true;
            //else
            //    e.HasMorePages = false;
        }

        private void rtfContract_TextChanged(object sender, EventArgs e)
        {
            //Israel 9/21/2015
            //btnPrint.Enabled = rtfContract.Text.Trim().Length > 2;
            btnPrint.Enabled = richeditConfirmBrowser.Document.Length > 2;
        }

        private void btnEmail_Click(object sender, EventArgs e)
        {
            //Send from Contract Viewer
            try
            {
                myTimer.Stop();
                eMailInputForm.settingsDir = this.appSettingsDir;
                //Israel 10/26/15 -- Removed EMailDomain property setting.
                string fromAddress = ""; // Utils.GetUserNameWithoutDomain(toolbarOrWindowsUserId) + "@" + Properties.Settings.Default.EMailDomain;
                eMailInputForm.InitForm(fromAddress);

                if (eMailInputForm.ShowDialog(this) == DialogResult.OK)
                {
                    fromAddress = eMailInputForm.tedFromAddress.Text;
                    string toAddress = eMailInputForm.tedToAddress.Text;
                    string subject = eMailInputForm.tedSubject.Text;
                    string body = eMailInputForm.memoBody.Text;
                    bool isRtf = eMailInputForm.cedSendAsRTF.Checked;

                    Int32 tradeId = viewContractTradeId;
                    Int32 rqmtId = 0;
                    Int32 rqmtConfirmId = 0;
                    string contractLabel = "";
                    string cptySn = GetTradeSummaryData(tradeId, "CptySn");
                    string tradeSysTicket = GetTradeSummaryData(tradeId, "TradeSysTicket");
                    //int templateId = 0;
                    if (viewContractTradeId > 0)
                    {
                        rqmtConfirmId = viewContractConfirmId;
                        rqmtId = GetConfirmRqmtId(rqmtConfirmId);
                        contractLabel = GetConfirmData(rqmtConfirmId, "ConfirmLabel");
                        //templateId = viewContractTemplateId;
                    }

                    DocumentFormat docFormat = DocumentFormat.OpenXml;
                    byte[] contractBody = WSUtils.GetByteArrayFromDocument(editContractForm.richeditConfirm, docFormat);

                    using (var ts = new TransactionScope())
                    {
                        Vaulter vaulter = new Vaulter(sqlConnectionStr);
                        vaulter.VaultTradeRqmtConfirm(rqmtConfirmId, null);

                        SendToFaxGateway("F", toAddress, contractLabel, contractBody, docFormat, cptySn, tradeId,tradeSysTicket,
                           rqmtId, (Int32)rqmtConfirmId, isRtf, fromAddress, subject, body, false);

                        ts.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to EMail a Confirm." + Environment.NewLine +
                    "Error CNF-166 in " + FORM_NAME + ".btnEmail_Click(): " + ex.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start();
            }
        }

        private void gluedContractList_CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
        {
            if (e.DisplayText.Trim().Length > 1)
            {
                DateTime tradeDt = DateTime.Parse(e.DisplayText);
                e.DisplayText = String.Format("{0:dd-MMM-yyyy}", tradeDt);
            }
        }

        private void btnCopyAll_Click(object sender, EventArgs e)
        {
            try
            {
                //Israel 9/21/2015
                //rtfContract.SelectAll();
                //rtfContract.Copy();
                //rtfContract.Select(0, 0);

                DocumentPosition startPos = richeditConfirmBrowser.Document.CreatePosition(0);
                DocumentRange docRange = richeditConfirmBrowser.Document.CreateRange(startPos, richeditConfirmBrowser.Document.Length);
                richeditConfirmBrowser.Document.Selection = docRange;
                richeditConfirmBrowser.Copy();
                docRange = richeditConfirmBrowser.Document.CreateRange(0, 0);
                richeditConfirmBrowser.Document.Selection = docRange;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while attempting to copy a Confirm document." + Environment.NewLine +
                    "Error CNF-167 in " + FORM_NAME + ".btnCopyAll_Click(): " + ex.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OkToSendAndSendOrManual_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myTimer.Stop();
                int setToOkAndSent = 0;
                int skipped = 0;
                SendMethod sendMethod;
                if (e.Item.Name == bbtnOkToSendAndSend.Name)
                    sendMethod = SendMethod.SendPaper;
                else if (e.Item.Name == bbtnOkToSendAndManualSend.Name)
                    sendMethod = SendMethod.ManualSend;
                else if (e.Item.Name == bbtnOktoSend.Name)
                    sendMethod = SendMethod.SetToOkToSendOnly;
                else
                    throw new Exception("Internal Error: Invalid calling method: " + e.Item.Name);

                string label = "";
                //string newStatus = "";
                if (sendMethod == SendMethod.SendPaper)
                {
                    label = "OkToSend & Send Paper";
                    //newStatus = "OK_TO_SEND";
                }
                else if (sendMethod == SendMethod.ManualSend)
                {
                    label = "OkToSend & Manual Send";
                    //newStatus = "OK_TO_SEND";
                }
                else if (sendMethod == SendMethod.SetToOkToSendOnly)
                {
                    label = "OkToSend";
                    //newStatus = "OK_TO_SEND";
                }

                DialogResult result = XtraMessageBox.Show(label + "?", label,
                   MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    using (var ts = new TransactionScope())
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        GridView view = gridViewSummary;
                        Int32 prevRow = GetGridViewFocusedRowHandle(view);
                        //int prevRow = view.FocusedRowHandle;

                        //Get the list ahead of time, since the foreach code changes the list's contents/order
                        int[] selectedRows = view.GetSelectedRows();
                        List<DataRow> rows = new List<DataRow>();

                        foreach (int rowHandle in selectedRows)
                            rows.Add(view.GetDataRow(rowHandle));

                        foreach (DataRow row in rows)
                        {
                            Int32 tradeId = Int32.Parse(row["TradeId"].ToString());
                            string finalApprovalFlag = row["FinalApprovalFlag"].ToString();
                            bool finalApproved = (finalApprovalFlag == "Y");
                            Int32 confirmId = GetContractRowId(tradeId);
                            Int32 confirmRqmtId = GetConfirmRqmtId(confirmId);
                            string status = GetTradeRqmtData(confirmRqmtId, "Status");
                            string faxTelexNumber = GetContractConfirmData(tradeId, "FaxTelexNumber");
                            bool isFaxNumberOk = (sendMethod == SendMethod.ManualSend) || (faxTelexNumber.Trim().Length > 0);
                            string preparerCanSendFlag = GetContractConfirmData(tradeId, "PreparerCanSendFlag");
                            bool enableOkAndSend = (isContractApprove || preparerCanSendFlag == "Y") &&
                                                   (status == "MGR") &&
                                                   (isFaxNumberOk) &&
                                                   !finalApproved;
                            if (enableOkAndSend)
                            {
                                string reference = GetTradeRqmtData(confirmRqmtId, "Reference");
                                string cmt = GetTradeRqmtData(confirmRqmtId, "Cmt");
                                CallUpdateTradeRqmts(tradeId, confirmRqmtId, SEMPRA_RQMT, "OK_TO_SEND", DateTime.Today, reference, cmt, true);
                                if (sendMethod == SendMethod.SendPaper)
                                    SendContractForCurrentRqmt(tradeId);
                                else if (sendMethod == SendMethod.ManualSend)
                                    CallUpdateTradeRqmts(tradeId, confirmRqmtId, SEMPRA_RQMT, "SENT", DateTime.Today, reference, "Manual Send " + cmt, true);

                                setToOkAndSent++;
                            }
                            else
                                skipped++;
                        }

                        if (prevRow < view.RowCount)
                            view.SelectRow(prevRow);
                        else
                            view.SelectRow(view.RowCount - 1);

                        ts.Complete();
                    }

                    XtraMessageBox.Show(label + ": " + setToOkAndSent.ToString() + Environment.NewLine
                           + "Trades skipped: " + skipped.ToString(),
                           label, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while requesting 'OK To Send and Send or Manual Send'." + Environment.NewLine +
                    "Error CNF-168 in " + FORM_NAME + ".OkToSendAndSendOrManual_ItemClick(): " + ex.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start();
                Cursor.Current = Cursors.Default;
            }
        }

        private void SendPaperOrManualSend_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myTimer.Stop();
                int sent = 0;
                int skipped = 0;

                SendMethod sendMethod;
                if (e.Item.Name == bbtnSendResendPaper.Name)
                    sendMethod = SendMethod.SendPaper;
                else if (e.Item.Name == bbtnManualSend.Name)
                    sendMethod = SendMethod.ManualSend;
                else if (e.Item.Name == bbtnSendResendCoverPage.Name)
                    sendMethod = SendMethod.SendWithCoverPage;
                else
                    throw new Exception("Internal Error: Invalid calling method: " + e.Item.Name);

                string label = "";
                if (sendMethod == SendMethod.SendPaper)
                    label = "Send/Resend Paper";
                else if (sendMethod == SendMethod.ManualSend)
                    label = "Manual Send";
                else if (sendMethod == SendMethod.SendWithCoverPage)
                    label = "Send/Resend w/Cover Page";

                string title = "";
                string message = "";
                bool isCoverPage = false;
                bool isRtf = false;
                DialogResult result = 0;

                if (sendMethod == SendMethod.SendWithCoverPage)
                {
                    faxCoverPageInputForm.settingsDir = this.appSettingsDir;
                    faxCoverPageInputForm.InitForm();
                    result = faxCoverPageInputForm.ShowDialog();
                    if (result == DialogResult.Yes)
                    {
                        title = faxCoverPageInputForm.tedTitle.Text;
                        message = faxCoverPageInputForm.memoMessage.Text;
                        isCoverPage = true;
                        isRtf = faxCoverPageInputForm.cedSendAsRTF.Checked;
                    }
                }
                else
                    result = XtraMessageBox.Show(label + "?", label,
                      MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    //Get the list ahead of time, since the foreach code changes the list's contents/order 
                    GridView view = gridViewSummary;
                    int[] selectedRows = view.GetSelectedRows();
                    List<DataRow> rows = new List<DataRow>();

                    foreach (int rowHandle in selectedRows)
                        rows.Add(view.GetDataRow(rowHandle));

                    foreach (DataRow row in rows)
                    {
                        long tradeId = long.Parse(row["TradeId"].ToString());
                        string cptySn = row["CptySn"].ToString();
                        string finalApprovalFlag = row["FinalApprovalFlag"].ToString();
                        bool finalApproved = (finalApprovalFlag == "Y");
                        int confirmId = GetContractRowId(tradeId);
                        int confirmRqmtId = GetConfirmRqmtId(confirmId);
                        string status = GetTradeRqmtData(confirmRqmtId, "Status");
                        string faxTelexNumber = GetContractConfirmData(tradeId, "FaxTelexNumber");
                        bool isFaxNumberOk = (sendMethod == SendMethod.ManualSend) || (faxTelexNumber.Trim().Length > 0);

                        bool enableSend = (status == "OK_TO_SEND" || status == "SENT" || status == "FAIL") &&
                            (isFaxNumberOk) &&
                            !finalApproved;
                        if (enableSend)
                        {
                            if (sendMethod == SendMethod.SendPaper || sendMethod == SendMethod.SendWithCoverPage)
                            {
                                using (var ts = new TransactionScope())
                                {
                                    Vaulter vaulter = new Vaulter(sqlConnectionStr);
                                    vaulter.VaultTradeRqmtConfirm(confirmId, null);

                                    SendToFaxGateway(confirmId, cptySn, isRtf, title, message, isCoverPage);

                                    ts.Complete();
                                }
                            }
                            else //Manual send
                            {
                                string reference = GetContractConfirmData(confirmRqmtId, "NextStatusCode");
                                string cmt = GetTradeRqmtData(confirmRqmtId, "Cmt");
                                CallUpdateTradeRqmts(tradeId, confirmRqmtId, SEMPRA_RQMT, "SENT", DateTime.Today, reference, "Manual Send " + cmt, true);
                            }

                            sent++;
                        }
                        else
                            skipped++;
                    }
                    XtraMessageBox.Show(label + ": " + sent.ToString() + Environment.NewLine
                       + "Trades skipped: " + skipped.ToString(),
                       label, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while requesting 'Send Paper or Manual Send'." + Environment.NewLine +
                    "Error CNF-169 in " + FORM_NAME + ".SendPaperOrManualSend_ItemClick(): " + ex.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start();
                Cursor.Current = Cursors.Default;
            }
        }

        private void bbCancelSempraPaperRqmt_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                myTimer.Stop();
                int cancelsProcessed = 0;
                int skipped = 0;
                string label = "Cancel Our Paper Rqmt?";
                string cancelComments = "";

                //CommentForm commentForm = new CommentForm(label);
                CommentForm commentForm = new CommentForm();
                commentForm.Text = label;
                commentForm.Comment = cancelComments;

                commentForm.ShowDialog();
                DialogResult result = commentForm.DialogResult;

                if (result == DialogResult.OK)
                {
                    cancelComments = commentForm.Comment;
                }

                if (result == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    //Get the list ahead of time, since the foreach code changes the list's contents/order 
                    GridView view = gridViewSummary;
                    int[] selectedRows = view.GetSelectedRows();
                    List<DataRow> rows = new List<DataRow>();

                    foreach (int rowHandle in selectedRows)
                        rows.Add(view.GetDataRow(rowHandle));

                    foreach (DataRow rowSummary in rows)
                    {
                        long tradeId = long.Parse(rowSummary["TradeId"].ToString());
                        bool foundCancel = false;
                        string finalApprovalFlag = rowSummary["FinalApprovalFlag"].ToString();
                        bool finalApproved = (finalApprovalFlag == "Y");

                        string filterStr = "TradeId = " + tradeId.ToString() + " and Rqmt = '" + SEMPRA_RQMT + "'";
                        foreach (DataRow rowRqmt in rqmtDataTable.Select(filterStr))
                        {
                            long rqmtId = long.Parse(rowRqmt["Id"].ToString());
                            string rqmtStatus = rowRqmt["Status"].ToString();
                            string cmt = "";
                            if (rqmtStatus != "CXL" && !finalApproved)
                            {
                                string reference = rowRqmt["Reference"].ToString();
                                if (cancelComments != "")
                                {
                                    cmt = cancelComments;
                                }
                                else
                                {
                                    cmt = rowRqmt["Cmt"].ToString();
                                }
                                CallUpdateTradeRqmts(tradeId, rqmtId, SEMPRA_RQMT, "CXL", DateTime.Today, reference, cmt, true);
                                cancelsProcessed++;
                                foundCancel = true;
                                break;
                            }
                        }
                        if (!foundCancel)
                            skipped++;
                    }
                    XtraMessageBox.Show(label + ": " + cancelsProcessed.ToString() + Environment.NewLine
                       + "Trades skipped: " + skipped.ToString(),
                       label, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while requesting 'Cancel Our Paper' requirement." + Environment.NewLine +
                    "Error CNF-170 in " + FORM_NAME + ".bbCancelSempraPaperRqmt_ItemClick(): " + ex.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start();
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region Inbound Members

        private void InitInboundPanel()
        {
            DataRow dr = null;
            try
            {
                //string userFullName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                string userFullName = p_UserId;
                string userName = userFullName.Substring(userFullName.LastIndexOf("\\") + 1);
                this.tifEditorInbound.UserName = userName;

                inboundPnl1 = new InboundPnl(sqlConnectionStr, ref dataManager, ref dataSet, IsDocumentModified);
                dpInboundQueue.Controls.Add(inboundPnl1);

                gridRqmt.AllowDrop = true;
                tifEditorInbound.AllowDrop = true;

                inboundPnl1.ImageFilenameDelegate = LoadImageFile;
                ImagesEventManager.Instance.OnInboundDocSelected += OnInboundDocSelected;
                ImagesEventManager.Instance.OnInboundDocSaving += OnInboundDocSaving;
                inboundPnl1.PrintDocumentDelegate = PrintInboundDocument;
                inboundPnl1.DocPagesDelegate = GetDocPageCount;
                inboundPnl1.ExtractPagesDelegate = ExtractPagesWithAnnotations;
                inboundPnl1.SubmitEditRqmtDelegate = CallUpdateTradeRqmts;
                inboundPnl1.GetActiveTradeRqmt = GetActiveTradeRqmtData;
                inboundPnl1.LocateTradeSummaryRecDelegate = LocateTradeSummaryRec;
                inboundPnl1.EditTradeRqmtDelegate = EditSelectedRequirement;
                inboundPnl1.FinalizeInboundDocDelegate = FinalizeInboundDoc;
                //inboundPnl1.GetCptyFaxNoDelegate = GetCptyFaxNo;
                inboundPnl1.GetTradeSummaryDataRecDelegate = GetTradeSummaryDataObject;
                inboundPnl1.MergeInboundFilesDelegate = MergeInboundFiles;
                inboundPnl1.SetTifEditorCanEditDelegate = SetTifEditorCanEdit;
                inboundPnl1.SetTifEditorSaveAsFileName = SetTifEditorSaveAsFileName;
                inboundPnl1.UserUpdateAccces = isHasUpdate;
                inboundPnl1.ApplyUserUpdateAccess();

                string attribCode = "CPTY_SN";  // HARD CODED FOR NOW.  LATER WE WILL HAVE A DROP DOWN LIST BOX
                PopulateCptyValuesControl(cptyLkupTbl);
                richTextBox1.ContextMenuStrip = cntxMenuPhraseMapping;

                dr = gridViewSummary.GetDataRow(gridViewSummary.FocusedRowHandle);
                if (dr != null)
                {
                    InboundPnl.ActiveSummaryData = CollectionHelper.CreateObjectFromDataRow<SummaryData>(dr);
                }
                inboundPnl1.DisplayInboundDocument();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while setting up Inbound Panel initial values." + Environment.NewLine +
                     "Error CNF-334 in " + FORM_NAME + ".InitInboundPanel(): " + ex.Message);
            }
        }

        private void OnInboundDocSelected(ImagesSelectedEventArgs args)
        {
            if (args.Selected == null)
            {
                return;
            }

            byte[] imageBytes = args.Selected.MarkupImage ?? args.Selected.OriginalImage;
            if (imageBytes != null)
            {
                try
                {
                    using (var stream = new MemoryStream(imageBytes))
                    {
                        tifEditorInbound.LoadImage(stream);
                        tifEditorInbound.Edit = args.CanEditImage;
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("Error attempting to display image:" + e.Message, e);
                }
            }
        }

        private void OnInboundDocSaving(ImagesSavingEventArgs args)
        {
            tifEditorInbound.SaveToFile();
        }

        private byte[] GetImageBytesOverride()
        {
            var currentSelected = ImagesEventManager.Instance.CurrentSelected;
            return (currentSelected == null)
                ? null
                : currentSelected.MarkupImage;
        }

        private void SaveImageOverride(TifEditor editor)
        {
            try
            {
                var currentSelected = ImagesEventManager.Instance.CurrentSelected;
                if (currentSelected == null ||
                    !currentSelected.CanSave)
                {
                    XtraMessageBox.Show(FORM_NAME + ".SaveImage: Image cannot be saved.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                IImagesDal dal = new ImagesDal(sqlConnectionStr);
                currentSelected.MarkupImage = tifEditorInbound.GetImageBytes();
                dal.Update(currentSelected);
            }
            catch (Exception e)
            {
                Logger.Error("Error CNF-171: Error saving image back to database:" + e.Message, e);
                XtraMessageBox.Show("Error saving image back to database. " + Environment.NewLine +
                    FORM_NAME + ".SaveImage" + Environment.NewLine +
                    "Error CNF-171 in " + FORM_NAME + ".SaveImageOverride(): " + e.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateCptyValuesControl(System.Data.DataTable mappingValTbl)
        {
            try
            {
                if (mappingValTbl != null)
                {
                    mappingValTbl.DefaultView.Sort = "CptySn";
                    lkupMappedValue.Properties.DataSource = cptyLkupTbl;
                    lkupMappedValue.Properties.DisplayMember = "CptySn";
                    lkupMappedValue.Properties.ForceInitialize();
                }
            }
            catch (Exception error)
            {
                XtraMessageBox.Show(FORM_NAME + ".InitForm" + Environment.NewLine +
                    "Error CNF-172 in " + FORM_NAME + ".PopulateCptyValuesControl(): " + error.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetActiveTradeRqmtData()
        {
            SelectedRqmtHasChanged();
        }

        private void gridViewRqmt_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            SelectedRqmtHasChanged();
        }

        private void SelectedRqmtHasChanged()
        {
            DataRow dr = null;
            InboundPnl.ActiveTradeRqmt = null;
            try
            {
                if (gridViewRqmt.IsValidRowHandle(gridViewRqmt.FocusedRowHandle))
                {
                    dr = gridViewRqmt.GetDataRow(gridViewRqmt.FocusedRowHandle);
                    InboundPnl.ActiveTradeRqmt = CollectionHelper.CreateObjectFromDataRow<RqmtData>(dr);
                    //InboundPnl.ActiveTradeRqmt = CollectionHelper.CreateObjectFromDataRow<VPcTradeRqmtDto>(dr);
                }
                //if (inboundPnl1 != null)
                //{
                //    inboundPnl1.LocateAndDisplayTradeRqmtDocument();
                //}
            }
            catch (Exception ex)
            {
                //XtraMessageBox.Show("Error Rqmt Grid View Row Changed Event: " + ex.Message);
                XtraMessageBox.Show("An error occurred while changing the current selected row." + Environment.NewLine +
                    "Error CNF-173 in " + FORM_NAME + ".SelectedRqmtHasChanged(): " + ex.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridViewRqmt_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                if (e.Button == MouseButtons.Left && downHitInfo != null)
                {
                    Size dragSize = SystemInformation.DragSize;
                    System.Drawing.Rectangle dragRect = new System.Drawing.Rectangle(new System.Drawing.Point(downHitInfo.HitPoint.X - dragSize.Width / 2,
                        downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

                    if (!dragRect.Contains(new System.Drawing.Point(e.X, e.Y)))
                    {
                        DataRow row = view.GetDataRow(downHitInfo.RowHandle);
                        RqmtData data = CollectionHelper.CreateObjectFromDataRow<RqmtData>(row);
                        view.GridControl.DoDragDrop(data, DragDropEffects.Move);
                        downHitInfo = null;
                        DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while changing the mouse position." + Environment.NewLine +
                    "Error CNF-174 in " + FORM_NAME + ".gridViewRqmt_MouseMove(): " + ex.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridViewRqmt_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                downHitInfo = null;
                GridHitInfo hitInfo = view.CalcHitInfo(new System.Drawing.Point(e.X, e.Y));
                if (Control.ModifierKeys != Keys.None) return;
                if (e.Button == MouseButtons.Left && hitInfo.RowHandle >= 0)
                    downHitInfo = hitInfo;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while pressing the mouse button." + Environment.NewLine +
                    "Error CNF-175 in " + FORM_NAME + ".gridViewRqmt_MouseDown(): " + ex.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tifEditorInbound_DragDrop(object sender, DragEventArgs e)
        {
            RqmtData dataObj = null;
            try
            {
                dataObj = e.Data.GetData(typeof(RqmtData)) as RqmtData;
                if (dataObj != null)
                {
                    InboundPnl.ActiveTradeRqmt = dataObj;
                    inboundPnl1.AssociateDocument(false);
                }
            }
            catch (Exception ex)
            {
                //XtraMessageBox.Show("Rqmt Grid DragDrop Exception: " + ex.Message);
                XtraMessageBox.Show("An error occurred while dragging and dropping with the mouse." + Environment.NewLine +
                    "Error CNF-176 in " + FORM_NAME + ".tifEditorInbound_DragDrop(): " + ex.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tifEditorInbound_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(RqmtData)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void gridRqmt_DragDrop(object sender, DragEventArgs e)
        {
            DataRow dr = null;
            RqmtData dataObj = null;
            InboundDocsView inbObj = null;

            try
            {
                inbObj = e.Data.GetData(typeof(InboundDocsView)) as InboundDocsView;
                if (inbObj != null)
                {
                    GridView focusedView = (GridView)gridRqmt.FocusedView;
                    if (focusedView.IsValidRowHandle(focusedView.FocusedRowHandle))
                    {
                        dr = focusedView.GetDataRow(focusedView.FocusedRowHandle);
                        dataObj = CollectionHelper.CreateObjectFromDataRow<RqmtData>(dr);
                        InboundPnl.ActiveTradeRqmt = dataObj;
                        inboundPnl1.AssociateDocument(false);
                    }
                }
            }
            catch (Exception ex)
            {
                //XtraMessageBox.Show("Rqmt Grid DragDrop Exception: " + ex.Message);
                XtraMessageBox.Show("An error occurred while dragging and dropping with the mouse." + Environment.NewLine +
                    "Error CNF-177 in " + FORM_NAME + ".gridRqmt_DragDrop(): " + ex.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridRqmt_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(InboundDocsView)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        public void PrintInboundDocument()
        {
            tifEditorInbound.PrintImage();
        }

        public void PublishDocumentImage()
        {
            tifEditorInbound.Publish();
        }

        public bool IsDocumentModified()
        {
            return tifEditorInbound.ImageModified;
        }

        public void LoadImageFile(string fileName, string saveToFilename, bool canEdit)
        {
            if (tifEditorInbound.ImageFileName != fileName)
            {
                tifEditorInbound.ImageFileName = fileName;
                tifEditorInbound.LoadImage();
            }
            tifEditorInbound.SaveAsFileName = saveToFilename;
            // tifEditorInbound.Edit = (fileName == saveToFilename);
            tifEditorInbound.Edit = true;

            richTextBox1.Text = "";
            if (tabctrlInboundViewer.SelectedTabPage == tabpgTextView)
            {
                LoadTextFile(((Path.GetFileName(tifEditorInbound.ImageFileName)).ToLower().Replace(".tif", ".txt")));
            }
        }

        public void SetTifEditorSaveAsFileName(string saveToFilename)
        {
            tifEditorInbound.SaveAsFileName = saveToFilename;
        }

        public void SetTifEditorCanEdit(bool canEdit)
        {
            // everything is editable now!!
            //        tifEditorInbound.Edit = canEdit;
        }

        public string GetDocFileName()
        {
            return tifEditorInbound.ImageFileName;
        }

        public int GetDocPageCount()
        {
            return tifEditorInbound.TotalPages;
        }

        public void ExtractPagesWithAnnotations(string fileName, string fileLocation, string pageList)
        {
            TifUtil.ExtractPagesWithAnnotation(fileName, fileLocation, pageList);
        }

        private void tabctrlInboundViewer_SelectedPageChanged(object sender,
           DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                if (e.Page.Name == tabpgTextView.Name)
                {
                    LoadTextFile(((Path.GetFileName(tifEditorInbound.ImageFileName)).ToLower().Replace(".tif", ".txt")));
                }
            }
            catch (Exception excep)
            {
                //XtraMessageBox.Show("Exception handled..." + excep.Message);
                XtraMessageBox.Show("An error occurred while loading a file." + Environment.NewLine +
                    "Error CNF-178 in " + FORM_NAME + ".tabctrlInboundViewer_SelectedPageChanged(): " + excep.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTextFile(string txtFileName)
        {
            txtFileName = txtFileName.Insert(0, Properties.Settings.Default.InboundTextFilesLocation + "\\");
            if (File.Exists(txtFileName))
            {
                richTextBox1.Text = System.IO.File.ReadAllText(txtFileName);
            }
            else
            {
                richTextBox1.Text = "Unable to locate text file: " + txtFileName;
            }
        }

        private void btnPhraseEditor_Click(object sender, EventArgs e)
        {
            string mapDescr = "";
            DataRow row = null;
            try
            {
                myTimer.Stop();
                lkupMappings.Properties.BestFit();
                lkupMappings.Properties.ValueMember = "Description";
                if (lkupMappedValue.EditValue != null)
                {
                    row = ((System.Data.DataRowView)(lkupMappedValue.EditValue)).Row;
                }
                if (row != null)
                {
                    InputBoxResult result = InputBox.Show("Unique Descr:", "Cpty Short Code Mapping", mapDescr, null);
                    if (result.OK)
                    {
                        InbAttribMapValDto inbAttribMapValData = new InbAttribMapValDto();
                        mapDescr = result.Text;
                        if (mapDescr.Trim().Length > 0)
                        {
                            inbAttribMapValData.ActiveFlag = "Y";
                            inbAttribMapValData.Id = 0;
                            inbAttribMapValData.InbAttribCode = "CPTY_SN";
                            inbAttribMapValData.MappedValue = row["CptySn"].ToString().Trim();
                            inbAttribMapValData.Descr = mapDescr.Trim();
                            UpdateUserMappingValue(inbAttribMapValData);
                            lkupMappings.EditValue = inbAttribMapValData.Descr;
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("Please select a valid value for mapping.");
                }
            }
            catch (Exception ex)
            {
                //XtraMessageBox.Show("Exception in Map Phrase Event: " + ex.Message);
                XtraMessageBox.Show("An error occurred while mapping a phrase." + Environment.NewLine +
                    "Error CNF-179 in " + FORM_NAME + ".tabctrlInboundViewer_SelectedPageChanged(): " + ex.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start();
            }
            //  OpenMappedPhraseEditor("CPTY_SN");
        }

        private long UpdateUserMappingValue(InbAttribMapValDto pData)
        {
            long mappingValId = 0;
            try
            {
                InbAttribMapValDal inbAttribMapValDal = new InbAttribMapValDal(sqlConnectionStr);
                int inbAttribMapValId = inbAttribMapValDal.Insert(pData);

                if (inbAttribMapValId > 0)
                {
                    DataRow dr = tblUserMappings.NewRow();
                    dr["ID"] = inbAttribMapValId;
                    dr["Description"] = pData.Descr;

                    if (pData.ActiveFlag.Equals("Y"))
                    {
                        tblUserMappings.Rows.Add(dr);
                    }
                    mappingValId = inbAttribMapValId;
                }
                else
                {
                    throw new Exception("The new Attribute Mapping value was unexpectedly not inserted into the database.");
                }
            }
            catch (Exception err)
            {
                //throw new Exception("Error in UpdateUserMappingValue(InbAttribMapValDto pData): " + err.Message);
                throw new Exception("An error occurred while updating a mapping value." + Environment.NewLine +
                     "Error CNF-180 in " + FORM_NAME + ".UpdateUserMappingValue(): " + err.Message);
            }
            return mappingValId;
        }

        private void OpenMappedPhraseEditor(string attribCode)
        {
            DataRow row = null;
            try
            {
                myTimer.Stop();
                if (!lkupMappedValue.Text.Equals(""))
                {
                    row = ((System.Data.DataRowView)(lkupMappedValue.EditValue)).Row;
                }
                Cursor.Current = Cursors.WaitCursor;
                frmMapPhrase mapPhraseForm = new frmMapPhrase(sqlConnectionStr, attribCode, row);
                Cursor.Current = Cursors.Default;
                mapPhraseForm.settingsDir = this.appSettingsDir;
                mapPhraseForm.ResetForm();
                mapPhraseForm.Show();
            }
            catch (Exception ex)
            {
                //XtraMessageBox.Show("barBtnMapPhrase_ItemClick: " + ex.Message,
                //   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                XtraMessageBox.Show("An error occurred while requesting the Mapped Phrase Editor for Atribute Code =" + attribCode + Environment.NewLine +
                    "Error CNF-181 in " + FORM_NAME + ".OpenMappedPhraseEditor(): " + ex.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start();
                Cursor.Current = Cursors.WaitCursor;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string phrase = richTextBox1.SelectedText.Trim();
            if (lkupMappings.Text.Equals(""))
            {
                XtraMessageBox.Show("Please select a valid mapping for your phrase: " + phrase);
                return;
            }
            string mappDesc = lkupMappings.Text;
            DataRow[] rows = tblUserMappings.Select("Description = " + "'" + mappDesc.Replace("'", "''") + "'");
            if (rows.Length == 0) return;
            DataRow row = rows[0];

            if (row == null || phrase.Trim().Equals("")) return;

            InbAttribMapPhraseDto inbAttribMapPhraseData = new InbAttribMapPhraseDto();
            inbAttribMapPhraseData.Id = 0;
            inbAttribMapPhraseData.ActiveFlag = "Y";
            inbAttribMapPhraseData.InbAttribMapValId = Convert.ToInt32(row["ID"]);
            inbAttribMapPhraseData.Phrase = phrase;

            UpdateMappedPhrase(inbAttribMapPhraseData);
        }

        private void UpdateMappedPhrase(InbAttribMapPhraseDto pData)
        {
            bool isUpdate = pData.Id > 0;
            DataRow row = null;
            try
            {
                InbAttribMapPhraseDal inbAttribMapPhraseDal = new InbAttribMapPhraseDal(sqlConnectionStr);
                int methodResult = 0;
                if (isUpdate)
                    methodResult = inbAttribMapPhraseDal.Update(pData);
                else if (pData.ActiveFlag.Equals("N"))
                    methodResult = inbAttribMapPhraseDal.Delete(pData.Id);
                else
                    methodResult = inbAttribMapPhraseDal.Insert(pData);

                if (methodResult > 0)
                {
                    // update grid view with new row.
                    if (tblPhrases == null)
                    {
                        InitPhraseTable();
                    }
                    if (isUpdate)
                    {
                        row = tblPhrases.Rows.Find(pData.Id);
                        if (row != null)
                        {
                            tblPhrases.Rows[tblPhrases.Rows.IndexOf(row)].Delete();
                        }
                    }
                    else
                    {
                        row = tblPhrases.NewRow();
                        row["ID"] = methodResult;
                        row["Mapped Attribute ID"] = pData.InbAttribMapValId;
                        row["Phrase"] = pData.Phrase;
                        row["Active Flag"] = pData.ActiveFlag;
                        tblPhrases.Rows.Add(row);
                    }
                }
                else
                {
                    //throw new Exception("Error UpdateMappedPhrase(InbAttribMapPhraseDto pData)");
                    //Israel 11/25/2015 -- The purpose of numbering this exception separately is to differentiate between a general
                    //error that won't trigger this exception and one of these 0 rows updated errors.
                    throw new Exception("An error occurred while updating a mapped phrase." + Environment.NewLine +
                         "Error CNF-182 in " + FORM_NAME + ".UpdateMappedPhrase()");
                }
            }
            catch (Exception err)
            {
                //XtraMessageBox.Show(err.Message);
                XtraMessageBox.Show("An error occurred while processing a Mapped Phrase." + Environment.NewLine +
                    "Error CNF-183 in " + FORM_NAME + ".UpdateMappedPhrase(): " + err.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lkupMappedValue_TextChanged(object sender, EventArgs e)
        {
            PopulateUserMappings();
        }

        private void PopulateUserMappings()
        {

            try
            {
                if (lkupMappedValue.Text.Equals("")) return;
                resetMappingTable();
                DataRow row = ((System.Data.DataRowView)(lkupMappedValue.EditValue)).Row;

                if (row != null)
                {
                    string inbAttribCode = row["CptySn"].ToString();
                    btnPhraseEditor.ToolTip = "Create a new mapping for: " + inbAttribCode;
                    InbAttribMapValDal inbAttribMapValDal = new InbAttribMapValDal(sqlConnectionStr);
                    List<InbAttribMapValDto> inbAttribMapValList = new List<InbAttribMapValDto>();
                    inbAttribMapValList = inbAttribMapValDal.GetMapValues(inbAttribCode);

                    if (inbAttribMapValList.Count > 0)
                    {
                        foreach (InbAttribMapValDto data in inbAttribMapValList)
                        {
                            DataRow dr = tblUserMappings.NewRow();
                            dr[1] = data.Id;
                            dr[0] = data.Descr;
                            tblUserMappings.Rows.Add(dr);
                        }
                        PopulatePhrases();
                    }
                    else
                    {
                        //throw new Exception("Error PopulateUserMappings() List");
                        throw new Exception("An error occurred while populating user mappings." + Environment.NewLine +
                            "Error CNF-184 in " + FORM_NAME + ".PopulateUserMappings()");
                    }
                }
            }
            catch (Exception err)
            {
                //XtraMessageBox.Show(err.Message);
                XtraMessageBox.Show("An error occurred while processing user mappings." + Environment.NewLine +
                    "Error CNF-185 in " + FORM_NAME + ".PopulateUserMappings(): " + err.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void resetMappingTable()
        {
            if (tblUserMappings == null)
            {
                InitMappingsTable();
            }
            else
            {
                tblUserMappings.Clear();
                resetPhraseTable();
            }
        }

        private void InitMappingsTable()
        {
            tblUserMappings = new System.Data.DataTable();
            tblUserMappings.Columns.Add("Description");
            tblUserMappings.Columns.Add("ID");
            tblUserMappings.PrimaryKey = new DataColumn[] { tblUserMappings.Columns["ID"] };

            if (tblUserMappings != null)
            {
                tblUserMappings.DefaultView.Sort = "Description";
                lkupMappings.Properties.DataSource = tblUserMappings;
                lkupMappings.Properties.DisplayMember = "Description";
                lkupMappings.Properties.ForceInitialize();
            }
        }

        private void PopulatePhrases()
        {
            string mappDesc = "";
            try
            {
                if (lkupMappings.Text.Equals("")) return;
                resetPhraseTable();
                mappDesc = lkupMappings.Text;
                string select = "Description = " + "'" + mappDesc.Replace("'", "''") + "'";
                DataRow[] rows = tblUserMappings.Select(select);
                if (rows.Length == 0) return;
                DataRow row = rows[0];
                if (row != null)
                {
                    Int32 inbAttribMapValId = Convert.ToInt32(row["ID"]);
                    List<InbAttribMapPhraseDto> inbAttribMapPhraseList = new List<InbAttribMapPhraseDto>();
                    InbAttribMapPhraseDal inbAttribMapPhraseDal = new InbAttribMapPhraseDal(sqlConnectionStr);
                    inbAttribMapPhraseList = inbAttribMapPhraseDal.GetPhrases(inbAttribMapValId);

                    if (inbAttribMapPhraseList.Count > 0)
                    {
                        foreach (InbAttribMapPhraseDto data in inbAttribMapPhraseList)
                        {
                            DataRow dr = tblPhrases.NewRow();
                            dr[0] = data.Id;
                            dr[1] = data.InbAttribMapValId;
                            dr[2] = data.Phrase;
                            dr[3] = "Y";

                            tblPhrases.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        //throw new Exception("Error in PopulatePhrases()");
                        throw new Exception("An error occurred while populating phrases." + Environment.NewLine +
                            "Error CNF-186 in " + FORM_NAME + ".PopulatePhrases()");
                    }
                }
            }
            catch (Exception err)
            {
                //XtraMessageBox.Show(err.Message);
                XtraMessageBox.Show("An error occurred while processing phrases." + Environment.NewLine +
                    "Error CNF-187 in " + FORM_NAME + ".PopulatePhrases(): " + err.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void resetPhraseTable()
        {
            if (tblPhrases == null)
            {
                InitPhraseTable();
            }
            else
            {
                tblPhrases.Clear();
            }
        }

        private void InitPhraseTable()
        {
            tblPhrases = new System.Data.DataTable();
            tblPhrases.Columns.Add("ID");
            tblPhrases.Columns.Add("Mapped Attribute ID");
            tblPhrases.Columns.Add("Phrase");
            tblPhrases.Columns.Add("Active Flag");
            tblPhrases.PrimaryKey = new DataColumn[] { tblPhrases.Columns["ID"] };

            gridControlMappedPhrases.DataSource = tblPhrases;
            gridControlMappedPhrases.ForceInitialize();
        }

        private void gridViewMappedPhrases_ShowGridMenu(object sender, GridMenuEventArgs e)
        {
            popupMappedPhrases.ShowPopup(gridControlMappedPhrases.PointToScreen(e.Point));
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            DataRow dr = null;
            try
            {
                if (gridViewMappedPhrases.IsValidRowHandle(gridViewMappedPhrases.FocusedRowHandle))
                {
                    dr = gridViewMappedPhrases.GetDataRow(gridViewMappedPhrases.FocusedRowHandle);
                    InbAttribMapPhraseDto inbAttribMapPhraseData = new InbAttribMapPhraseDto();
                    inbAttribMapPhraseData.ActiveFlag = "N";
                    inbAttribMapPhraseData.Id = Convert.ToInt32(dr["ID"].ToString());
                    inbAttribMapPhraseData.InbAttribMapValId = Convert.ToInt32(dr["Mapped Attribute ID"]);
                    inbAttribMapPhraseData.Phrase = dr["Phrase"].ToString();
                    UpdateMappedPhrase(inbAttribMapPhraseData);
                }
            }
            catch (Exception ex)
            {
                //XtraMessageBox.Show("Exception in delete mapping: " + ex.Message);
                XtraMessageBox.Show("An error occurred while deleting mappings." + Environment.NewLine +
                    "Error CNF-188 in " + FORM_NAME + ".barButtonItem2_ItemClick(): " + ex.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public SummaryData LocateTradeSummaryRec(int tradeId, int rqmtId)
        {
            SummaryData sumData = null;
            if (tradeId <= 0) return sumData;
            try
            {
                DevExpress.XtraGrid.Columns.GridColumn col = gridViewSummary.Columns["TradeId"];
                int rowHandle = gridViewSummary.LocateByValue(0, col, tradeId);
                if (!(rowHandle == DevExpress.XtraGrid.GridControl.InvalidRowHandle))
                {
                    gridViewSummary.ClearSelection();
                    gridViewSummary.SelectRow(rowHandle);
                    gridViewSummary.FocusedRowHandle = rowHandle;
                    SelectTradeRqmtRow(rqmtId);
                    DataRow dr = gridViewSummary.GetDataRow(rowHandle);
                    if (dr != null)
                    {
                        sumData = CollectionHelper.CreateObjectFromDataRow<SummaryData>(dr);
                    }
                }
                else
                {
                    if (!gridViewSummary.ActiveFilter.DisplayText.Equals(""))
                    {
                        gridViewSummary.ActiveFilter.Clear();
                        LocateTradeSummaryRec(tradeId, rqmtId);
                    }
                }
            }
            catch (Exception ex)
            {
                //XtraMessageBox.Show("Exception in LocateTradeSummaryRec with TradeID: " + tradeId + ". " + ex.Message);
                XtraMessageBox.Show("An error occurred while retrieving Summary Grid data for the following values:" + Environment.NewLine +
                    "Trade Id: " + tradeId.ToString() + ", Rqmt Id =" + rqmtId.ToString() + Environment.NewLine +
                    "Error CNF-189 in " + FORM_NAME + ".LocateTradeSummaryRec(): " + ex.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return sumData;
        }

        public SummaryData GetTradeSummaryDataObject(int tradeId)
        {
            SummaryData sumData = null;
            try
            {
                DataRow[] rows = summaryDataTable.Select("TradeId = " + Convert.ToString(tradeId));
                if (rows != null && rows.Length > 0)
                {
                    sumData = CollectionHelper.CreateObjectFromDataRow<SummaryData>(rows[0]);
                }
            }
            catch (Exception ex)
            {
                //XtraMessageBox.Show("Exception in LocateTradeSummaryRec with TradeID: " + tradeId + ". " + ex.Message);
                XtraMessageBox.Show("An error occurred while retrieving Summary Grid data for Trade Id: " + tradeId.ToString() + Environment.NewLine +
                    "Error CNF-190 in GetTradeSummaryDataObject(): " + ex.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return sumData;
        }

        private void SelectTradeRqmtRow(long rqmtId)
        {
            try
            {
                DataRow[] found = rqmtDataTable.Select("Id = " + rqmtId);
                if (found != null && found.Length > 0)
                {
                    int rowHandle = gridViewRqmt.GetRowHandle(rqmtDataTable.Rows.IndexOf(found[0]));
                    if ((rowHandle >= 0))
                    {
                        gridViewRqmt.ClearSelection();
                        gridViewRqmt.SelectRow(rowHandle);
                        gridViewRqmt.FocusedRowHandle = rowHandle;
                    }
                }
            }
            catch (Exception ex)
            {
                //XtraMessageBox.Show("Exception in SelectTradeRqmtRow with RqmtID: " + rqmtId + ". " + ex.Message);
                XtraMessageBox.Show("An error occurred while retrieving Requirement Grid data for Rqmt Id: " + rqmtId.ToString() + Environment.NewLine +
                    "Error CNF-191 in " + FORM_NAME + ".SelectTradeRqmtRow(): " + ex.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MergeInboundFiles(string parrentFile, string assFile)
        {
            TifUtil.MergeFiles(parrentFile, assFile, true);
        }

        private void FinalizeInboundDoc(int ptradeId, int rqmtId)
        {
            String ticket = "";
            String tradeId = "";
            String tradingSys = "";
            String cptySn = "";
            String bookingCoSn = "";
            String cptyTradeId = "";
            String tradeCmt = "";
            try
            {
                myTimer.Stop();

                editRqmtForm.BeginDataLoad();
                editRqmtForm.ClearAllFields();
                editRqmtForm.InitOldDataVariables();
                editRqmtForm.btnEditRqmtSaveAndApprove.Enabled = true;
                editRqmtForm.gluedSempraStatus.Enabled = true;  //sometimes gets set to false below...
                editRqmtForm.gluedSempraStatus.ForeColor = Color.Black;

                bool[] tabs = new bool[frmEditRqmt.RQMT_ARRAY_MAX];
                for (int i = 0; i < frmEditRqmt.RQMT_ARRAY_MAX; i++)
                    tabs[i] = false;

                GridView view = gridViewSummary;

                //Prepare to call form for single row selected.
                //Single row loads data and user performs standard data entry.
                if (view.SelectedRowsCount == 1)
                {
                    editRqmtForm.SingleOrMultiMode = frmEditRqmt.SINGLE;
                    //Setup Header data
                    tradingSys = view.GetRowCellDisplayText(view.FocusedRowHandle, "TrdSysCode").ToString();
                    ticket = view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeSysTicket").ToString();
                    tradeId = view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeId").ToString();

                    if (tradeId != Convert.ToString(ptradeId))
                        throw new Exception("Inbound Trade ID: " + ptradeId + " does not match Trade Summary Trade ID: " + tradeId);

                    cptySn = view.GetRowCellDisplayText(view.FocusedRowHandle, "CptySn").ToString();
                    bookingCoSn = view.GetRowCellDisplayText(view.FocusedRowHandle, "BookingCoSn").ToString();

                    ArrayList sempraPaperStatusList = new ArrayList();
                    string filterStr = "TradeId = " + view.GetRowCellDisplayText(view.FocusedRowHandle, "TradeId").ToString();
                    //Read the RqmtData row
                    //Israel 10/3/2008 Reading summary view was skipping second rqmt for Our Paper, etc.
                    foreach (DataRow row in rqmtDataTable.Select(filterStr))
                    {
                        //Determine which requirement tabs will be turned on.
                        if (row["Rqmt"].ToString() == SEMPRA_RQMT)
                        {
                            tabs[frmEditRqmt.RQMT_TYPE_SEMPRA] = true;
                            sempraPaperStatusList.Add(row["Status"].ToString());
                        }
                        else if (row["Rqmt"].ToString() == "XQCCP")
                            tabs[frmEditRqmt.RQMT_TYPE_CPTY] = true;
                        else if (row["Rqmt"].ToString() == "XQBBP")
                            tabs[frmEditRqmt.RQMT_TYPE_BROKER] = true;
                        else if (row["Rqmt"].ToString() == "NOCNF")
                            tabs[frmEditRqmt.RQMT_TYPE_NOCONF] = true;
                        //else if (row["Rqmt"].ToString() == "ECONF")
                        //    tabs[frmEditRqmt.RQMT_TYPE_ECONFIRM] = true;
                        //else if (row["Rqmt"].ToString() == "ECBKR")
                        //    tabs[frmEditRqmt.RQMT_TYPE_ECONFIRM_BROKER] = true;
                        //else if (row["Rqmt"].ToString() == "EFET")
                        //    tabs[frmEditRqmt.RQMT_TYPE_EFET_CPTY] = true;
                        //else if (row["Rqmt"].ToString() == "EFBKR")
                        //    tabs[frmEditRqmt.RQMT_TYPE_EFET_BROKER] = true;
                        else if (row["Rqmt"].ToString() == "VBCP")
                            tabs[frmEditRqmt.RQMT_TYPE_VERBAL] = true;
                        //else if (row["Rqmt"].ToString() == "MISC")
                        //tabs[frmEditRqmt.RQMT_TYPE_MISC] = true;
                    }

                    editRqmtForm.SetTabsVisible(tabs);

                    //Setup filter string to pass in following routine
                    //string filterStr = "TradeId = " + view.GetRowCellDisplayText(rowHandle, "TradeId").ToString();

                    //CallSetRqmtData sets data on form for current trade
                    if (tabs[frmEditRqmt.RQMT_TYPE_SEMPRA])
                        CallSetRqmtData(frmEditRqmt.RQMT_TYPE_SEMPRA, view, view.FocusedRowHandle, filterStr);
                    if (tabs[frmEditRqmt.RQMT_TYPE_CPTY])
                        CallSetRqmtData(frmEditRqmt.RQMT_TYPE_CPTY, view, view.FocusedRowHandle, filterStr);
                    if (tabs[frmEditRqmt.RQMT_TYPE_BROKER])
                        CallSetRqmtData(frmEditRqmt.RQMT_TYPE_BROKER, view, view.FocusedRowHandle, filterStr);
                    if (tabs[frmEditRqmt.RQMT_TYPE_NOCONF])
                        CallSetRqmtData(frmEditRqmt.RQMT_TYPE_NOCONF, view, view.FocusedRowHandle, filterStr);
                    //if (tabs[frmEditRqmt.RQMT_TYPE_ECONFIRM])
                    //    CallSetRqmtData(frmEditRqmt.RQMT_TYPE_ECONFIRM, view, view.FocusedRowHandle, filterStr);
                    //if (tabs[frmEditRqmt.RQMT_TYPE_ECONFIRM_BROKER])
                    //    CallSetRqmtData(frmEditRqmt.RQMT_TYPE_ECONFIRM_BROKER, view, view.FocusedRowHandle, filterStr);
                    //if (tabs[frmEditRqmt.RQMT_TYPE_EFET_CPTY])
                    //    CallSetRqmtData(frmEditRqmt.RQMT_TYPE_EFET_CPTY, view, view.FocusedRowHandle, filterStr);
                    //if (tabs[frmEditRqmt.RQMT_TYPE_EFET_BROKER])
                    //    CallSetRqmtData(frmEditRqmt.RQMT_TYPE_EFET_BROKER, view, view.FocusedRowHandle, filterStr);
                    if (tabs[frmEditRqmt.RQMT_TYPE_VERBAL])
                        CallSetRqmtData(frmEditRqmt.RQMT_TYPE_VERBAL, view, view.FocusedRowHandle, filterStr);
                    //if (tabs[frmEditRqmt.RQMT_TYPE_MISC])
                    //CallSetRqmtData(frmEditRqmt.RQMT_TYPE_MISC, view, view.FocusedRowHandle, filterStr);

                    //Setup cpty trade id and trade comment
                    cptyTradeId = view.GetRowCellDisplayText(view.FocusedRowHandle, "CptyTradeId").ToString();
                    tradeCmt = view.GetRowCellDisplayText(view.FocusedRowHandle, "Cmt").ToString();
                    editRqmtForm.SetCptyTradeIdEnabled(true);
                    editRqmtForm.SetTradeData(cptyTradeId, tradeCmt);
                    //For each Our Paper requirement check to see if any of the NoEdit statuses
                    //are contained in it. If so, lock down the status so it can't be changed.
                    if (sempraPaperStatusList.Count > 0)
                    {
                        for (int i = 0; i < sempraPaperStatusList.Count; i++)
                            if (noEditSempraRqmtStatus.IndexOf(sempraPaperStatusList[i]) > -1)
                            {
                                editRqmtForm.gluedSempraStatus.Enabled = false;
                                editRqmtForm.gluedSempraStatus.ForeColor = Color.Black;
                                editRqmtForm.btnEditRqmtSaveAndApprove.Enabled = false;
                            }
                    }
                }

                //Multi-Record data entry allows mass updating of multiple trades
                //Form is blank and only data that is entered updates appropriate requirements.
                editRqmtForm.SetHeaderLabels(tradingSys, ticket, cptySn, bookingCoSn);
                editRqmtForm.EndDataLoad();

                //Call the form
                if (editRqmtForm.ShowDialog(this) == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    using (var ts = new TransactionScope())
                    {                     //Get back the data that was entered
                        bool[] updatedRqmts = new bool[frmEditRqmt.RQMT_ARRAY_MAX];
                        string[] updatedStatusCodes = new string[frmEditRqmt.RQMT_ARRAY_MAX];
                        DateTime[] updatedStatusDates = new DateTime[frmEditRqmt.RQMT_ARRAY_MAX];
                        string[] updatedSecondChecks = new string[frmEditRqmt.RQMT_ARRAY_MAX];
                        string[] updatedReferences = new string[frmEditRqmt.RQMT_ARRAY_MAX];
                        string[] updatedRqmtCmts = new string[frmEditRqmt.RQMT_ARRAY_MAX];
                        string updatedCptyTradeId;
                        string updatedTradeCmt;
                        bool[,] changedFields = new bool[frmEditRqmt.RQMT_ARRAY_MAX, frmEditRqmt.FIELD_ARRAY_MAX];
                        bool isCptyTradeIdChanged = false;
                        bool isTradeCmtChanged = false;

                        updatedRqmts = editRqmtForm.GetUpdatedRqmts();
                        updatedStatusCodes = editRqmtForm.GetStatusCodes();
                        updatedStatusDates = editRqmtForm.GetStatusDates();
                        updatedSecondChecks = editRqmtForm.GetSecondChecks();
                        updatedReferences = editRqmtForm.GetReferences();
                        updatedRqmtCmts = editRqmtForm.GetRqmtCmts();
                        updatedCptyTradeId = editRqmtForm.GetCptyTradeId();
                        updatedTradeCmt = editRqmtForm.GetTradeCmt();
                        changedFields = editRqmtForm.GetChangedFields();
                        isCptyTradeIdChanged = editRqmtForm.IsCptyTradeIdChanged();
                        isTradeCmtChanged = editRqmtForm.IsTradeCmtChanged();

                        //Find out if any requirements need to be updated.
                        bool isAnyRqmtsUpdated = false;
                        for (int i = 0; i < frmEditRqmt.RQMT_ARRAY_MAX; i++)
                            if (updatedRqmts[i])
                                isAnyRqmtsUpdated = true;

                        //Call the routine that invokes the update procedure.
                        if (isAnyRqmtsUpdated)
                            CallUpdateEditRqmt(updatedRqmts, updatedStatusCodes, updatedStatusDates, updatedSecondChecks,
                               updatedReferences, updatedRqmtCmts, changedFields);

                        if (isCptyTradeIdChanged)
                            CallUpdateCptyTradeId(ptradeId, cptyTradeId);

                        if (isTradeCmtChanged)
                            CallUpdateTradeCmt(updatedTradeCmt);

                        if (isForceFinalApprove && editRqmtForm.isFinalApprove)
                            CallUpdateFinalApproval("Y", "N");

                        ts.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                //XtraMessageBox.Show("CallEditRequirements: " + ex.Message,
                //   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                XtraMessageBox.Show("An error occurred while finalizing a document using the following values:" + Environment.NewLine +
                    "Ticket: " + ticket + ", Trade Id: " + ptradeId.ToString() + ", Rqmt Id: " + rqmtId.ToString() + Environment.NewLine +
                    "Error CNF-192 in " + FORM_NAME + ".FinalizeInboundDoc(): " + ex.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start(); //Israel 12/15/2008 - Red X
                Cursor.Current = Cursors.Default;
            }
        }

        private void barBtnViewAssDocs_ItemClick(object sender, ItemClickEventArgs e)
        {
            SummaryData sumData = null;
            if (!(gridViewSummary.FocusedRowHandle == DevExpress.XtraGrid.GridControl.InvalidRowHandle))
            {
                DataRow dr = gridViewSummary.GetDataRow(gridViewSummary.FocusedRowHandle);
                if (dr != null)
                {
                    sumData = CollectionHelper.CreateObjectFromDataRow<SummaryData>(dr);
                    inboundPnl1.FilteredTradeId = sumData.TradeId;
                }
            }
        }

        private void lkupMappings_TextChanged(object sender, EventArgs e)
        {
            PopulatePhrases();
        }

        private void btnDeleteUserMapping_Click(object sender, EventArgs e)
        {
            DialogResult result = XtraMessageBox.Show("Delete current user mapping?", "Delete user mapping",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                DeleteUserMapping();
            }

        }

        private void DeleteUserMapping()
        {
            string mappDesc = "";
            DataRow row = null;
            try
            {
                myTimer.Stop();
                if (lkupMappings.EditValue != null)
                {
                    mappDesc = lkupMappings.Text;
                    string select = "Description = " + "'" + mappDesc.Replace("'", "''") + "'";
                    DataRow[] rows = tblUserMappings.Select(select);
                    if (rows.Length == 0) return;
                    row = rows[0];

                }
                if (row != null)
                {
                    Int32 id = Convert.ToInt32(row["ID"]);
                    InbAttribMapValDal inbAttribMapValDal = new InbAttribMapValDal(sqlConnectionStr);
                    inbAttribMapValDal.Delete(id);

                    row.Delete();
                    resetPhraseTable();
                }
                else
                {
                    XtraMessageBox.Show("Please select a valid value for mapping.");
                }
            }
            catch (Exception ex)
            {
                //XtraMessageBox.Show("Exception in Delete User Mapping: " + ex.Message);
                XtraMessageBox.Show("An error occurred while deleting user mappings." + Environment.NewLine +
                    "Error CNF-193 in " + FORM_NAME + ".DeleteUserMapping(): " + ex.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start();
            }
        }

        private void barbtnReOpenInbDoc_ItemClick(object sender, ItemClickEventArgs e)
        {
            ReOpenAnInboundDocument();
        }

        private void ReOpenAnInboundDocument()
        {
            string docId = "";
            try
            {
                myTimer.Stop();
                InputBoxResult result = InputBox.Show("Inbound Doc ID:", "Re-Open Inbound Document", docId, null);
                if (result.OK)
                {
                    Int32 inbDocsId = Convert.ToInt32(result.Text);
                    string docStatus = AssociatedDoc.OPEN;
                    Int32 rowsUpdated = 0;
                    InboundDocsDal inboundDocsDal = new InboundDocsDal(sqlConnectionStr);
                    rowsUpdated = inboundDocsDal.UpdateStatus(inbDocsId, docStatus);
                    if (rowsUpdated == 0)
                        //throw new Exception("Error re-opening Inbound Document ID:" + inbDocsId.ToString());
                        throw new Exception("An error occurred while re-opening Inbound Document Id:" + inbDocsId.ToString() + Environment.NewLine +
                            "Error CNF-194 in " + FORM_NAME + ".ReOpenAnInboundDocument()");
                }
            }
            catch (Exception ex)
            {
                //XtraMessageBox.Show("Exception in Re-Open Inbound Document: " + ex.Message);
                XtraMessageBox.Show("An error occurred while attempting to Re-Open an Inbound Document." + Environment.NewLine +
                    "Error CNF-195 in " + FORM_NAME + ".ReOpenAnInboundDocument(): " + ex.Message,
                  MAIN_FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                myTimer.Start();
            }
        }

        #endregion

        private void bbtnBrowserViewContract_Click(object sender, EventArgs e)
        {
            getContractForBrowserAppTab();
        }

        private void barRefreshData_ItemClick(object sender, ItemClickEventArgs e)
        {
            //Implement refresh data here
        }

        private void btnViewDealsheetBrowser_Click(object sender, EventArgs e)
        {
            barbtnGetDealsheet_ItemClick(null, null);
        }

        private void btnPrintDealsheetBrowser_Click(object sender, EventArgs e)
        {
            if (!richeditDealsheetBrowser.Document.IsEmpty)
                richeditDealsheetBrowser.Print();
        }

        private void popupConfirm_BeforePopup(object sender, CancelEventArgs e)
        {
            Int32 rowHandle = GetGridViewFocusedRowHandle(gridViewSummary);
            Int32 tradeId = Int32.Parse(gridViewSummary.GetRowCellDisplayText(rowHandle, "TradeId").ToString());
            string ourPaperRqmtStatus = GetTradeSummaryData(tradeId, "SetcStatus");
            if (ourPaperRqmtStatus == "NEW" ||
                ourPaperRqmtStatus == "PREP" ||
                ourPaperRqmtStatus == "EXT_REVIEW" ||
                ourPaperRqmtStatus == "TRADER" ||
                ourPaperRqmtStatus == "MGR" ||
                ourPaperRqmtStatus == "OK_TO_SEND")
                barBtnEditConfirm.Caption = "Edit Confirm";
            else
                barBtnEditConfirm.Caption = "View Confirm";

            barBtnEditConfirm.Enabled = (ourPaperRqmtStatus == "NEW" || ourPaperRqmtStatus == "CXL") ? false : true;
        }

        private void barSubPanels_ItemClick(object sender, ItemClickEventArgs e)
        {
            bool isVaultViewerOpened = false;
            foreach (var process in Process.GetProcessesByName("VaultViewer"))
            {
                isVaultViewerOpened = true;
            }
            barChkVaultedDocsPanel.Checked = isVaultViewerOpened;
        }
    }
}