using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CommonUtils;
using DevExpress.XtraEditors;
using OpsTrackingModel;
using DataManager;
using WSAccess;
using DBAccess.SqlServer;
//using ConfirmManager.trackingClientMain;

namespace ConfirmManager
{
    public partial class frmGetAll : DevExpress.XtraEditors.XtraForm
    {
        private const string FORM_NAME = "frmGetAll";
        private const string FORM_ERROR_CAPTION = "Get All Form Error";
        public string settingsDir;
        public string sqlConnectionStr;
        private StringBuilder whereClause = new StringBuilder();
        private bool initialized = false;
        private string tradingSystem;
        private string ticketList;
        private string cptyTradeId;
        private string bookingCoSn;
        private string cptySn;
        private string cdtyCode;
        private DateTime tradeStartDate;
        private DateTime tradeEndDate;

        public DateTime TradeEndDate
        {
            get { return tradeEndDate; }
            set { tradeEndDate = value; }
        }

        public DateTime TradeStartDate
        {
            get { return tradeStartDate; }
            set { tradeStartDate = value; }
        }

        public string CdtyCode
        {
            get { return cdtyCode; }
            set { cdtyCode = value; }
        }

        public string CptySn
        {
            get { return cptySn; }
            set { cptySn = value; }
        }

        public string BookingCoSn
        {
            get { return bookingCoSn; }
            set { bookingCoSn = value; }
        }

        public string TicketList
        {
            get { return ticketList; }
            set { ticketList = value; }
        }

        public string CptyTradeId
        {
            get { return cptyTradeId; }
            set { cptyTradeId = value; }
        }

        public string TradingSystem
        {
            get { return tradingSystem; }
            set { tradingSystem = value; }
        }

        public frmGetAll()
        {
            InitializeComponent();
            //PopulateTradeSysList();
        }

        private void PopulateTradingSysList()
        {
            try
            {
                //Israel 10/26/15 Removed hard-coded trading system codes
                //string[] semperators = {";"};
                //string[] tradeSysCodes = (Properties.Settings.Default.TradeSystems).Split(semperators, StringSplitOptions.None);

                string extSvcBaseUrl = Properties.Settings.Default.ExtSvcAPIBaseUrl;
                string extSvcUserName = Properties.Settings.Default.ExtSvcAPIUserName;
                string extSvcPassword = Properties.Settings.Default.ExtSvcAPIPassword;
                ConfirmMgrAPIDal confirmMgrAPIDal = new ConfirmMgrAPIDal(extSvcBaseUrl, extSvcUserName, extSvcPassword);
                List<string> permKeyList = new List<string>();
                bool isSuperUser = false;
                permKeyList = confirmMgrAPIDal.GetPermissionKeys(frmMain.p_UserId, out isSuperUser);
                //permKeyList.Add("AMPH US");
                //permKeyList.Add("MERC US");

                //If isSuperUser == True then pass a blank permKeyDBInClauseStr to the DB call.
                string permKeyDBInClauseStr = "";

                //Only get the IN clause if isSuperUser == false. Else, a blank IN clause will retrieve all rows
                if (!isSuperUser && permKeyList.Count > 0)
                    permKeyDBInClauseStr = confirmMgrAPIDal.GetPermissionKeyDBInClause(permKeyList);

                List<string> tradeSysCodeList = new List<string>();
                VPcTradeSummaryDal vpcTradeSummaryDal = new VPcTradeSummaryDal(sqlConnectionStr);
                tradeSysCodeList = vpcTradeSummaryDal.GetAllTradingSysCodes(permKeyDBInClauseStr);
                cmboTradingSystem.Properties.Items.Add("");
                cmboTradingSystem.Properties.Items.AddRange(tradeSysCodeList);
            }
            catch (Exception ex)
            {
            }
        }

        private void frmGetAll_Load(object sender, EventArgs e)
        {
            ReadUserSettings();
        }

        private void frmGetAll_FormClosing(object sender, FormClosingEventArgs e)
        {
            WriteUserSettings();
        }

        private void ReadUserSettings()
        {
            try
            {
                //Now read user settings, ReadAppSettings() must be called first
                Sempra.Ops.IniFile iniFile = new Sempra.Ops.IniFile(FileNameUtils.GetUserIniFileName(settingsDir));

                this.Top = iniFile.ReadValue(FORM_NAME, "Top", 200);
                this.Left = iniFile.ReadValue(FORM_NAME, "Left", 300);
                //this.Width = iniFile.ReadValue(FORM_NAME, "Width", 750);
                //this.Height = iniFile.ReadValue(FORM_NAME, "Height", 450);
            }
            catch (Exception error)
            {
                XtraMessageBox.Show("An error occurred while reading the user settings file." + Environment.NewLine +
                       "Error CNF-293 in " + FORM_NAME + ".ReadUserSettings(): " + error.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WriteUserSettings()
        {
            try
            {
                Sempra.Ops.IniFile iniFile = new Sempra.Ops.IniFile(FileNameUtils.GetUserIniFileName(settingsDir));
                iniFile.WriteValue(FORM_NAME, "Top", this.Top);
                iniFile.WriteValue(FORM_NAME, "Left", this.Left);
                //iniFile.WriteValue(FORM_NAME, "Width", this.Width);
                //iniFile.WriteValue(FORM_NAME, "Height", this.Height);
            }
            catch (Exception error)
            {
                XtraMessageBox.Show("An error occurred while saving the user settings file." + Environment.NewLine +
                       "Error CNF-294 in " + FORM_NAME + ".WriteUserSettings(): " + error.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        internal void InitForm(DataTable cdtyLkupTbl, DataTable cptyLkupTbl, DataTable seCptyLkupTbl)
        {
            try
            {
                if (initialized == false)
                {
                    PopulateTradingSysList();

                    DataRow null1 = seCptyLkupTbl.NewRow();
                    //null1["BookingCoSn"] = "";
                    //seCptyLkupTbl.Rows.Add(null1);

                    DataRow null2 = cdtyLkupTbl.NewRow();
                    null2["CdtyCode"] = "";
                    cdtyLkupTbl.Rows.Add(null2);

                    DataRow null3 = cptyLkupTbl.NewRow();
                    null3["CptySn"] = "";
                    cptyLkupTbl.Rows.Add(null3);

                    seCptyLkupTbl.DefaultView.Sort = "BookingCoSn";
                    lkupRbsCompany.Properties.DataSource = seCptyLkupTbl;
                    lkupRbsCompany.Properties.DisplayMember = "BookingCoSn";
                    lkupRbsCompany.Properties.ForceInitialize();

                    cdtyLkupTbl.DefaultView.Sort = "CdtyCode";
                    lkupCommidity.Properties.DataSource = cdtyLkupTbl;
                    lkupCommidity.Properties.DisplayMember = "CdtyCode";
                    lkupCommidity.Properties.ForceInitialize();

                    cptyLkupTbl.DefaultView.Sort = "CptySn";
                    lkupCpty.Properties.DataSource = cptyLkupTbl;
                    lkupCpty.Properties.DisplayMember = "CptySn";
                    lkupCpty.Properties.ForceInitialize();

                    initialized = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while setting up initial values for the form." + Environment.NewLine +
                     "Error CNF-295 in " + FORM_NAME + ".InitForm(): " + ex.Message);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            ClearSearchFields();
        }

        private void ClearSearchFields()
        {
            try
            {
                lkupRbsCompany.EditValue = null;
                lkupCommidity.EditValue = null;
                lkupCpty.EditValue = null;
                cmboTradingSystem.EditValue = null;
                txtTicket.Text = "";
                txtCptyTradeId.Text = "";
                dtStartDate.EditValue = null;
                dtTradeDate.EditValue = null;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while resetting the data entry fields for the form." + Environment.NewLine +
                     "Error CNF-296 in " + FORM_NAME + ".ClearSearchFields(): " + ex.Message);
            }
        }

        private void btnGetTradeData_Click(object sender, EventArgs e)
        {
            tradingSystem = cmboTradingSystem.Text;
            ticketList = txtTicket.Text;
            cptyTradeId = txtCptyTradeId.Text;
            bookingCoSn = lkupRbsCompany.Text;
            cptySn = lkupCpty.Text;
            cdtyCode = lkupCommidity.Text;
            tradeStartDate = dtStartDate.DateTime;
            tradeEndDate = dtTradeDate.DateTime;
        }
    }
}