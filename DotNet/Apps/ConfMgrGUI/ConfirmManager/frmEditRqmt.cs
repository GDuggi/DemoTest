using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CommonUtils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;

namespace ConfirmManager
{
   public partial class frmEditRqmt : DevExpress.XtraEditors.XtraForm
   {
      private const string FORM_NAME = "frmEditRqmt";
      private const string FORM_ERROR_CAPTION = "Edit Requirement Form Error";
      public string settingsDir;
      public DataTable rqmtStatusView;
      private DataTable rqmtStatusViewSempra;     
      private DataTable rqmtStatusViewCpty;
      private DataTable rqmtStatusViewBroker;
      private DataTable rqmtStatusViewNoConf;
      private DataTable rqmtStatusViewEConfirm;
      private DataTable rqmtStatusViewEfetCpty;
      private DataTable rqmtStatusViewEfetBroker;
      private DataTable rqmtStatusViewVerbal;
      private DataTable rqmtStatusVieweEConfirmBroker;
      private DataTable rqmtStatusViewMisc;
      private string[] oldStatusCode = new string[10];
      private DateTime[] oldStatusDate = new DateTime[10];
      private bool[] oldSecondCheck = new bool[10];
      private string[] oldReference = new string[10];
      private string[] oldRqmtCmt = new string[10];
      private string oldCptyTradeId;
      private string oldTradeCmt;
      private string userName = "";
      private bool loadingData = false;
      private bool[] activeRqmts = new bool[10];
      //private string[] prelimApprover = new string[10];
      private DevExpress.XtraBars.BarStaticItem[] StatusBarItems = new DevExpress.XtraBars.BarStaticItem[10];

      public const int RQMT_TYPE_SEMPRA = 0;
      public const int RQMT_TYPE_CPTY = 1;
      public const int RQMT_TYPE_BROKER = 2;
      public const int RQMT_TYPE_NOCONF = 3;
      //public const int RQMT_TYPE_ECONFIRM = 4;
      //public const int RQMT_TYPE_ECONFIRM_BROKER = 5;
      //public const int RQMT_TYPE_EFET_CPTY = 6;
      //public const int RQMT_TYPE_EFET_BROKER = 7;
      public const int RQMT_TYPE_VERBAL = 4;
      //public const int RQMT_TYPE_MISC = 9;
      //public readonly string[] RQMT_CODES = new string[] { "XQCSP", "XQCCP", "XQBBP", "NOCNF", "ECONF", "ECBKR", "EFET", "EFBKR", "VBCP" };
      public readonly string[] RQMT_CODES = new string[] { "XQCSP", "XQCCP", "XQBBP", "NOCNF", "VBCP" };
      public const int FIELD_STATUS_CODE = 0;
      public const int FIELD_STATUS_DATE = 1;
      public const int FIELD_SECOND_CHECK = 2;
      public const int FIELD_REFERENCE = 3;
      public const int FIELD_RQMT_CMT = 4;
      public const int SINGLE = 0;
      public const int MULTI = 1;
      //Israel 6/9/2015 changed from 10 to 9 to compensate for no MISC
      public const int RQMT_ARRAY_MAX = 5;
      public const int FIELD_ARRAY_MAX = 5;

      public string[] RqmtInitialStatus = new string[9];
      private bool isSecondCheckCreateCxl = false;
      private bool isSubmitQueuedEFETTrades = false;
      public bool isFinalApprove = false;
      public int SingleOrMultiMode = 0;

      public frmEditRqmt()
      {
         InitializeComponent();
      }
       
      //****************** Public Methods *************************

      public void InitForm()
      {
         try
         {
            for (int i = 0; i < Properties.Settings.Default.DisputedReasons.Count; i++)
            {
               comboSempraDispReason.Properties.Items.Add(Properties.Settings.Default.DisputedReasons[i]);
               comboCptyDispReason.Properties.Items.Add(Properties.Settings.Default.DisputedReasons[i]);
               comboBrokerDispReason.Properties.Items.Add(Properties.Settings.Default.DisputedReasons[i]);
               comboNoConfDispReason.Properties.Items.Add(Properties.Settings.Default.DisputedReasons[i]);
               comboEConfirmDispReason.Properties.Items.Add(Properties.Settings.Default.DisputedReasons[i]);
               comboEConfirmBrokerDispReason.Properties.Items.Add(Properties.Settings.Default.DisputedReasons[i]);
               comboEfetCptyDispReason.Properties.Items.Add(Properties.Settings.Default.DisputedReasons[i]);
               comboEfetBrokerDispReason.Properties.Items.Add(Properties.Settings.Default.DisputedReasons[i]);
               comboVerbalDispReason.Properties.Items.Add(Properties.Settings.Default.DisputedReasons[i]);
               comboMiscDispReason.Properties.Items.Add(Properties.Settings.Default.DisputedReasons[i]);
           }

            SetupStatusFieldData(ref gluedSempraStatus, ref rqmtStatusViewSempra, "XQCSP");
            SetupStatusFieldData(ref gluedCptyStatus, ref rqmtStatusViewCpty, "XQCCP");
            SetupStatusFieldData(ref gluedBrokerStatus, ref rqmtStatusViewBroker, "XQBBP");
            SetupStatusFieldData(ref gluedNoConfStatus, ref rqmtStatusViewNoConf, "NOCNF");
            //SetupStatusFieldData(ref gluedEConfirmStatus, ref rqmtStatusViewEConfirm, "ECONF");
            //SetupStatusFieldData(ref gluedEConfirmBrokerStatus, ref rqmtStatusVieweEConfirmBroker, "ECBKR");
            //SetupStatusFieldData(ref gluedEfetCptyStatus, ref rqmtStatusViewEfetCpty, "EFET");
            //SetupStatusFieldData(ref gluedEfetBrokerStatus, ref rqmtStatusViewEfetBroker, "EFBKR");
            SetupStatusFieldData(ref gluedVerbalStatus, ref rqmtStatusViewVerbal, "VBCP");
            //SetupStatusFieldData(ref gluedMiscStatus, ref rqmtStatusViewMisc, "MISC");

            rqmtStatusView.PrimaryKey = new DataColumn[] 
                  { rqmtStatusView.Columns["RqmtCode"], 
                    rqmtStatusView.Columns["StatusCode"] };

            StatusBarItems[RQMT_TYPE_SEMPRA] = bstatSempra;
            StatusBarItems[RQMT_TYPE_CPTY] = bstatCpty;
            StatusBarItems[RQMT_TYPE_BROKER] = bstatBroker;
            StatusBarItems[RQMT_TYPE_NOCONF] = bstatNoConf;
            //StatusBarItems[RQMT_TYPE_ECONFIRM] = bstatEConfirm;
            //StatusBarItems[RQMT_TYPE_ECONFIRM_BROKER] = bstatEConfirmBroker;
            //StatusBarItems[RQMT_TYPE_EFET_CPTY] = bstatEfetCpty;
            //StatusBarItems[RQMT_TYPE_EFET_BROKER] = bstatEfetBroker;
            StatusBarItems[RQMT_TYPE_VERBAL] = bstatVerbal;
            //StatusBarItems[RQMT_TYPE_MISC] = bstatMisc;

            tabpgEditRqmtEConfirm.PageVisible = false;
            tabpgEditRqmtEConfirmBroker.PageVisible = false;
            tabpgEditRqmtEfetCpty.PageVisible = false;
            tabpgEditRqmtEfetBroker.PageVisible = false;
            tabpgEditRqmtMisc.PageVisible = false;

            bstatEConfirm.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            bstatEConfirmBroker.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            
            userName = Sempra.Ops.Utils.GetUserName();
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while setting up initial values for the form." + Environment.NewLine +
                   "Error CNF-247 in " + FORM_NAME + ".InitForm(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      public void SetSecurityAccess(bool AIsSecondCheckCreateCxl, bool AIsSubmitQueuedEFETTrades, 
         bool AIsForceFinalApproval)
      {
         try
         {
            isSecondCheckCreateCxl = AIsSecondCheckCreateCxl;
            isSubmitQueuedEFETTrades = AIsSubmitQueuedEFETTrades;
            gluedEfetCptyStatus.Enabled = isSubmitQueuedEFETTrades;
            gluedEfetBrokerStatus.Enabled = isSubmitQueuedEFETTrades;
             //Israel 11/05/2015 -- Removed
            //btnEditRqmtSaveAndApprove.Visible = AIsForceFinalApproval;
            btnEditRqmtSaveAndApprove.Visible = false;
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while setting the form security access." + Environment.NewLine +
                   "Error CNF-248 in " + FORM_NAME + ".SetSecurityAccess(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }
      
      public void InitOldDataVariables()
      {
         try
         {
            for (int i=0;i<RQMT_ARRAY_MAX;i++)
            {
               oldStatusCode[i] = "";
               oldStatusDate[i] = DateTime.MinValue;
               oldSecondCheck[i] = false;
               oldReference[i] = "";
               oldRqmtCmt[i] = "";
               StatusBarItems[i].Enabled = false;
               activeRqmts[i] = false;
            }
            oldCptyTradeId = "";
            oldTradeCmt = "";
            bstatCptyTradeId.Enabled = false;
            bstatTradeCmt.Enabled = false;
         }
         catch (Exception error)
         {
             XtraMessageBox.Show("An error occurred while initializing the previous data field local storage." + Environment.NewLine +
                   "Error CNF-249 in " + FORM_NAME + ".InitOldDataVariables(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }
      
      public void ClearAllFields()
      {
         try
         {
            for (int i = 0; i < RQMT_ARRAY_MAX; i++)
            {
               GetStatusCodeEditor(i).EditValue = "";
               GetStatusCodeEditor(i).Properties.Appearance.BackColor = Color.White;

               GetDispReasonEditor(i).Visible = false;
               GetDispReasonEditor(i).EditValue = "";
               GetStatusDateEditor(i).EditValue = null;
               GetSecondCheckEditor(i).CheckState = CheckState.Indeterminate;
               GetReferenceEditor(i).EditValue = "";
               GetRqmtCmtEditor(i).EditValue = "";
            }

            txtedCptyTradeId.EditValue = "";
            memoTradeCmt.EditValue = "";
            gluedCptyStatus.Properties.View.ActiveFilter.Clear();
            gluedBrokerStatus.Properties.View.ActiveFilter.Clear();

            btnEditRqmtSave.Enabled = false;
            isFinalApprove = false;
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while clearing the current data entry fields local data storage." + Environment.NewLine +
                   "Error CNF-250 in " + FORM_NAME + "ClearAllFields(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }
      
      public void SetSecondCheckEnabled(Boolean AEnabled)
      {
         try
         {
            ckedSempra2ndChck.Enabled = AEnabled;
            ckedCpty2ndChck.Enabled = AEnabled;
            ckedBroker2ndChck.Enabled = AEnabled;
            ckedNoConf2ndChck.Enabled = AEnabled;
            ckedEConfirm2ndChck.Enabled = AEnabled;
            ckedEConfirmBroker2ndChck.Enabled = AEnabled;
            ckedEfetCpty2ndChck.Enabled = AEnabled;
            ckedEfetBroker2ndChck.Enabled = AEnabled;
            ckedVerbal2ndChck.Enabled = AEnabled;
            ckedMisc2ndChck.Enabled = AEnabled;
        }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while enabling/disabling data entry Second Check-related fields." + Environment.NewLine +
                   "Value of attempted setting: " + AEnabled + Environment.NewLine +
                   "Error CNF-251 in " + FORM_NAME + ".SetSecondCheckEnabled(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      public void SetHeaderLabels(string ATradingSystem, string ATicket, string ACptySn, string ASempraSn)
      {
         try
         {
            lblEditRqmtTradingSystem.Text = ATradingSystem;
            lblEditRqmtTicket.Text = ATicket;
            lblCptySn.Text = ACptySn;
            lblRBSSempraSn.Text = ASempraSn;
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while setting field labels using the following values:" + Environment.NewLine +
                   "Trading System: " + ATradingSystem + ", Ticket: " + ATicket + ", Cpty Short Name: " + ACptySn + "," + Environment.NewLine +
                   "Booking Company Short Name: " + ASempraSn + Environment.NewLine +
                   "Error CNF-252 in " + FORM_NAME + ".SetHeaderLabels(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      public void SetCptyTradeIdEnabled(bool pIsOneRowSelectedOnly)
      {
          try
          {
              txtedCptyTradeId.Enabled = pIsOneRowSelectedOnly;
              if (txtedCptyTradeId.Enabled)
                  txtedCptyTradeId.EditValue = "";
              else
                  txtedCptyTradeId.EditValue = "*** Multiple Trades Selected ***";
          }
          catch (Exception error)
          {
              XtraMessageBox.Show("An error occurred while setting the Cpty Trade Id field to be enabled or disabled." + Environment.NewLine +
                     "Error CNF-551 in " + FORM_NAME + ".SetCptyTradeIdEnabled(): " + error.Message,
                   FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
      }
      public void SetTabCtrlEditRqmtEnabled(bool isEnable)
      {
          try
          {
              tabCtrlEditRqmt.Enabled = isEnable;

          }
          catch (Exception error)
          {
              XtraMessageBox.Show("An error occurred while setting the Tab Control Edit Requirement field to be enabled or disabled." + Environment.NewLine +
                     "Error CNF-551 in " + FORM_NAME + ".SetCptyTradeIdEnabled(): " + error.Message,
                   FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
      }


      public void SetTabsVisible(bool[] tabs)
      {
         try
         {
            for (int i = 0; i < RQMT_ARRAY_MAX; i++)
            {
               tabCtrlEditRqmt.TabPages[i].PageVisible = tabs[i];
               activeRqmts[i] = tabs[i];
               if (tabs[i])
                  StatusBarItems[i].Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
               else
                  StatusBarItems[i].Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while setting form tabs to either display or be hidden." + Environment.NewLine +
                   "Error CNF-253 in " + FORM_NAME + ".SetTabsVisible(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }
      
      public void SetRqmtData(int ARqmtType, string AStatusCode, DateTime AStatusDate,
             bool ASecondCheck, string AReference, string ARqmtCmt)
      {
         try
         {
            GridLookUpEdit statusEditor = GetStatusCodeEditor(ARqmtType);
            statusEditor.EditValue = AStatusCode;
            string colorCode = GetRqmtStatusData(RQMT_CODES[ARqmtType], AStatusCode, "ColorCode");
            statusEditor.Properties.Appearance.BackColor = Color.FromName(colorCode);

            if (statusEditor.EditValue.ToString() == "DISP")
            {
               GetDispReasonEditor(ARqmtType).EditValue = ARqmtCmt;
               GetDispReasonEditor(ARqmtType).Visible = true;
            }

            //If Second Check and not yet PRELIM then don't show the APPR.
            //if (ASecondCheck &&
            //    APrelimApprover == "" &&
            //Israel 9/28/2015
            //if (ARqmtType == RQMT_TYPE_CPTY || ARqmtType == RQMT_TYPE_BROKER)
              // GetStatusCodeEditor(ARqmtType).Properties.View.ActiveFilterString = "[StatusCode] <> 'APPR'";

            //if (ARqmtType == RQMT_TYPE_SEMPRA)
               //GetStatusCodeEditor(ARqmtType).Properties.View.rows
                             
            GetStatusDateEditor(ARqmtType).EditValue = AStatusDate;

            GetSecondCheckEditor(ARqmtType).Visible = ASecondCheck;
            GetSecondCheckEditor(ARqmtType).Checked = ASecondCheck;
            
            GetReferenceEditor(ARqmtType).EditValue = AReference;
            GetRqmtCmtEditor(ARqmtType).EditValue = ARqmtCmt;

            //Store data in old data variables
            oldStatusCode[ARqmtType] = AStatusCode;
            oldStatusDate[ARqmtType] = AStatusDate;
            oldSecondCheck[ARqmtType] = ASecondCheck;
            oldReference[ARqmtType] = AReference;
            oldRqmtCmt[ARqmtType] = ARqmtCmt;

            //prelimApprover[ARqmtType] = APrelimApprover; 
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while setting data entry fields using the following values:" + Environment.NewLine +
                   "Rqmt Type (numeric value: 0-4): " + ARqmtType + ", Status Code: " + AStatusCode + ", Status Date: " + AStatusDate.ToString("dd-MMM-yyyy") + "," + Environment.NewLine +
                   "Second Check (true/false): " + ASecondCheck + ", Reference: " + AReference + ", Rqmt Comment: " + ARqmtCmt + Environment.NewLine +
                   "Error CNF-254 in " + FORM_NAME + ".SetRqmtData(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      public void SetTradeData(string ACptyTradeId, string ATradeCmt)
      {
         try
         {
             //Israel 12/23/2015 -- Added support for CptyTradeId
             txtedCptyTradeId.EditValue = ACptyTradeId;
             oldCptyTradeId = ACptyTradeId;

            memoTradeCmt.EditValue = ATradeCmt;
            oldTradeCmt = ATradeCmt;
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while setting the Trade Comment data entry field to: " + ATradeCmt + "." + Environment.NewLine +
                   "Error CNF-255 in " + FORM_NAME + ".SetTradeData(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      public bool IsRqmtDataChanged(int ARqmtType)
      {
         bool hasChanged = false;
         if (loadingData)
            return hasChanged;
         try
         {
            bool statusCodeChanged = GetStatusCodeEditor(ARqmtType).EditValue.ToString() != oldStatusCode[ARqmtType];
            bool statusDateChanged = GetStatusDateEditor(ARqmtType).DateTime != oldStatusDate[ARqmtType];
            bool secondCheckChanged = GetSecondCheckEditor(ARqmtType).Checked != oldSecondCheck[ARqmtType];
            bool referenceChanged = GetReferenceEditor(ARqmtType).EditValue.ToString() != oldReference[ARqmtType];
            bool rqmtCmtChanged = GetRqmtCmtEditor(ARqmtType).EditValue.ToString() != oldRqmtCmt[ARqmtType];

            hasChanged = statusCodeChanged || statusDateChanged || secondCheckChanged ||
                         referenceChanged || rqmtCmtChanged;
            return hasChanged;
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while determining if data has changed for Rqmt Type (numeric value: 0-4): " + ARqmtType + "." + Environment.NewLine +
                   "Error CNF-256 in " + FORM_NAME + ".IsRqmtDataChanged(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return hasChanged;
         }
      }

      public bool IsCptyTradeIdChanged()
      {
          //Israel 12/23/2015 -- Added support for CptyTradeId
          bool hasChanged = false;
          if (loadingData)
              return hasChanged;
          try
          {
              hasChanged = txtedCptyTradeId.EditValue.ToString() != oldCptyTradeId;
              return hasChanged;
          }
          catch (Exception error)
          {
              XtraMessageBox.Show("An error occurred while determining if Cpty Trade Id data has been entered." + Environment.NewLine +
                    "Error CNF-549 in " + FORM_NAME + ".IsCptyTradeIdChanged(): " + error.Message,
                  FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
              return hasChanged;
          }
      }

      public bool IsTradeCmtChanged()
      {
         bool hasChanged = false;
         if (loadingData)
            return hasChanged;
         try
         {
            hasChanged = memoTradeCmt.EditValue.ToString() != oldTradeCmt;
            return hasChanged;
         }
         catch (Exception error)
         {
             XtraMessageBox.Show("An error occurred while determining if Trade Comment data has been entered." + Environment.NewLine +
                   "Error CNF-257 in " + FORM_NAME + ".IsTradeCmtChanged(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return hasChanged;
         }
      }

      public void BeginDataLoad()
      {
         try
         {
            loadingData = true;
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while starting the data load process." + Environment.NewLine +
                   "Error CNF-258 in " + FORM_NAME + ".BeginDataLoad(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      public void EndDataLoad()
      {
         try
         {
            loadingData = false;
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while concluding the data load process." + Environment.NewLine +
                   "Error CNF-259 in " + FORM_NAME + ".EndDataLoad(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      public bool[] GetUpdatedRqmts()
      {
         bool[] updatedRqmts = new bool[RQMT_ARRAY_MAX];
         try
         {
            for (int i = 0; i < RQMT_ARRAY_MAX; i++)
               updatedRqmts[i] = false;
            
            for (int i = 0; i < RQMT_ARRAY_MAX; i++)
            {
               if (activeRqmts[i])
               {
                  if (IsRqmtDataChanged(i))
                  {
                     updatedRqmts[i] = true;
                  }
               }
            }
            return updatedRqmts;
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while determining if any Rqmt data has been entered." + Environment.NewLine +
                   "Error CNF-260 in " + FORM_NAME + ".GetUpdatedRqmts(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return updatedRqmts;
         }
      }

      public string[] GetStatusCodes()
      {
         string[] statusCodes = new string[RQMT_ARRAY_MAX];
         try
         {
            for (int i = 0; i < RQMT_ARRAY_MAX; i++)
               statusCodes[i] = "";

            for (int i = 0; i < RQMT_ARRAY_MAX; i++)
            {
               if (activeRqmts[i])
               {
                  if (IsRqmtDataChanged(i))
                  {
                     statusCodes[i] = GetStatusCodeEditor(i).EditValue.ToString();
                  }
               }
            }
            return statusCodes;
         }
         catch (Exception error)
         {
             XtraMessageBox.Show("An error occurred while determining if any Status code data has been entered." + Environment.NewLine +
                   "Error CNF-261 in " + FORM_NAME + ".GetStatusCodes(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return statusCodes;
         }
      }

      public DateTime[] GetStatusDates()
      {
         DateTime[] statusDates = new DateTime[RQMT_ARRAY_MAX];
         try
         {
            for (int i = 0; i < RQMT_ARRAY_MAX; i++)
               statusDates[i] = DateTime.MinValue;

            for (int i = 0; i < RQMT_ARRAY_MAX; i++)
            {
               if (activeRqmts[i])
               {
                  if (IsRqmtDataChanged(i))
                  {
                     statusDates[i] = GetStatusDateEditor(i).DateTime;
                  }
               }
            }
            return statusDates;
         }
         catch (Exception error)
         {
             XtraMessageBox.Show("An error occurred while determining which if any Status date data has been entered." + Environment.NewLine +
                   "Error CNF-262 in " + FORM_NAME + ".GetStatusDates(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return statusDates;
         }
      }

      public string[] GetSecondChecks()
      {
         string[] secondChecks = new string[RQMT_ARRAY_MAX];
         try
         {
            for (int i = 0; i < RQMT_ARRAY_MAX; i++)
               secondChecks[i] = "N";

            for (int i = 0; i < RQMT_ARRAY_MAX; i++)
            {
               if (activeRqmts[i])
               {
                  if (IsRqmtDataChanged(i))
                  {
                     if (GetSecondCheckEditor(i).Checked)
                        secondChecks[i] = "Y";
                  }
               }
            }
            return secondChecks;
         }
         catch (Exception error)
         {
             XtraMessageBox.Show("An error occurred while determining if any Second Check indication data has been entered." + Environment.NewLine +
                   "Error CNF-263 in " + FORM_NAME + ".GetSecondChecks(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return secondChecks;
         }
      }

      public string[] GetReferences()
      {
         string[] references = new string[RQMT_ARRAY_MAX];
         try
         {
            for (int i = 0; i < RQMT_ARRAY_MAX; i++)
               references[i] = "";

            for (int i = 0; i < RQMT_ARRAY_MAX; i++)
            {
               if (activeRqmts[i])
               {
                  if (IsRqmtDataChanged(i))
                  {
                     references[i] = GetReferenceEditor(i).EditValue.ToString();
                  }
               }
            }
            return references;
         }
         catch (Exception error)
         {
             XtraMessageBox.Show("An error occurred while determining if any Reference data has been entered." + Environment.NewLine +
                   "Error CNF-264 in " + FORM_NAME + ".GetReferences(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return references;
         }
      }

      public string[] GetRqmtCmts()
      {
         string[] rqmtCmts = new string[RQMT_ARRAY_MAX];
         try
         {
            for (int i = 0; i < RQMT_ARRAY_MAX; i++)
               rqmtCmts[i] = "";

            for (int i = 0; i < RQMT_ARRAY_MAX; i++)
            {
               if (activeRqmts[i])
               {
                  if (IsRqmtDataChanged(i))
                  {
                     rqmtCmts[i] = GetRqmtCmtEditor(i).EditValue.ToString();
                  }
               }
            }
            return rqmtCmts;
         }
         catch (Exception error)
         {
             XtraMessageBox.Show("An error occurred while determining if any Rqmt Comment data has been entered." + Environment.NewLine +
                   "Error CNF-265 in " + FORM_NAME + ".GetRqmtCmts(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return rqmtCmts;
         }
      }

      public bool[,] GetChangedFields()
      {
         bool[,] changedFields = new bool[RQMT_ARRAY_MAX,FIELD_ARRAY_MAX];
         try
         {
            for (int i = 0; i < RQMT_ARRAY_MAX; i++)
               for (int j = 0; j < FIELD_ARRAY_MAX; j++)
                  changedFields[i, j] = false;

            for (int rqmtType = 0; rqmtType < RQMT_ARRAY_MAX; rqmtType++)
            {
               if (activeRqmts[rqmtType])
               {
                  if (IsRqmtDataChanged(rqmtType))
                  {
                     changedFields[rqmtType, FIELD_STATUS_CODE] = GetStatusCodeEditor(rqmtType).EditValue.ToString() != oldStatusCode[rqmtType];
                     changedFields[rqmtType, FIELD_STATUS_DATE] = GetStatusDateEditor(rqmtType).DateTime != oldStatusDate[rqmtType];
                     changedFields[rqmtType, FIELD_SECOND_CHECK] = GetSecondCheckEditor(rqmtType).Checked != oldSecondCheck[rqmtType];
                     changedFields[rqmtType, FIELD_REFERENCE] = GetReferenceEditor(rqmtType).EditValue.ToString() != oldReference[rqmtType];
                     changedFields[rqmtType, FIELD_RQMT_CMT] = GetRqmtCmtEditor(rqmtType).EditValue.ToString() != oldRqmtCmt[rqmtType];
                  }
               }
            }
            return changedFields;
         }
         catch (Exception error)
         {
             XtraMessageBox.Show("An error occurred while determining if any data has been entered." + Environment.NewLine +
                   "Error CNF-266 in " + FORM_NAME + ".GetChangedFields(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return changedFields;
         }
      }

      public string GetCptyTradeId()
      {
          string cptyTradeId = "";
          try
          {
              cptyTradeId = txtedCptyTradeId.EditValue.ToString();
              return cptyTradeId;
          }
          catch (Exception error)
          {
              XtraMessageBox.Show("An error occurred while retrieving the Cpty Trade Id from the data entry field." + Environment.NewLine +
                     "Error CNF-550 in " + FORM_NAME + ".GetCptyTradeId(): " + error.Message,
                   FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
              return cptyTradeId;
          }
      }

      public string GetTradeCmt()
      {
         string tradeCmt = "";
         try
         {
            tradeCmt = memoTradeCmt.EditValue.ToString();
            return tradeCmt;
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while retrieving the Trade comment from the data entry field." + Environment.NewLine +
                   "Error CNF-267 in " + FORM_NAME + ".GetTradeCmt(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return tradeCmt;
         }
      }

      //****************** Private Methods *************************

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
                    "Error CNF-268 in " + FORM_NAME + ".ReadUserSettings(): " + error.Message,
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
                    "Error CNF-269 in " + FORM_NAME + ".WriteUserSettings(): " + error.Message,
                  FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void SetupStatusFieldData(ref DevExpress.XtraEditors.GridLookUpEdit AGridLookupEdit,
          ref DataTable ALookupDataTable, string AFilterValue)
      {
         ALookupDataTable = new DataTable();
         ALookupDataTable = rqmtStatusView.Copy();
         ALookupDataTable.PrimaryKey = new DataColumn[] 
                  { ALookupDataTable.Columns["RqmtCode"], 
                    ALookupDataTable.Columns["Ord"] };
         ALookupDataTable.DefaultView.RowFilter = "[RqmtCode] = '" + AFilterValue + "'";

         AGridLookupEdit.Properties.DataSource = ALookupDataTable.DefaultView;
         AGridLookupEdit.Properties.DisplayMember = "Descr";
         AGridLookupEdit.Properties.ValueMember = "StatusCode";
      }

      private GridLookUpEdit GetStatusCodeEditor(int ARqmtType)
      {
         GridLookUpEdit gluedResult = null;
         try
         {
            switch (ARqmtType)
            {
               case RQMT_TYPE_SEMPRA: gluedResult = gluedSempraStatus; break;
               case RQMT_TYPE_CPTY: gluedResult = gluedCptyStatus; break;
               case RQMT_TYPE_BROKER: gluedResult = gluedBrokerStatus; break;
               case RQMT_TYPE_NOCONF: gluedResult = gluedNoConfStatus; break;
               //case RQMT_TYPE_ECONFIRM: gluedResult = gluedEConfirmStatus; break;
               //case RQMT_TYPE_ECONFIRM_BROKER: gluedResult = gluedEConfirmBrokerStatus; break;
               //case RQMT_TYPE_EFET_CPTY: gluedResult = gluedEfetCptyStatus; break;
               //case RQMT_TYPE_EFET_BROKER: gluedResult = gluedEfetBrokerStatus; break;
               case RQMT_TYPE_VERBAL: gluedResult = gluedVerbalStatus; break;
               //case RQMT_TYPE_MISC: gluedResult = gluedMiscStatus; break;
               default: throw new Exception("Internal Exception: Invalid RqmtType=" + ARqmtType.ToString());
            }
            return gluedResult;
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while retrieving the Status lookup editor for Rqmt Type (numeric value: 0-4): " + ARqmtType + "." + Environment.NewLine +
                   "Error CNF-270 in " + FORM_NAME + ".GetStatusCodeEditor(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return gluedResult;
         }
      }

      private DateEdit GetStatusDateEditor(int ARqmtType)
      {
         DateEdit dedResult = null;
         try
         {
            switch (ARqmtType)
            {
               case RQMT_TYPE_SEMPRA: dedResult = dedSempraStatusDt; break;
               case RQMT_TYPE_CPTY: dedResult = dedCptyStatusDt; break;
               case RQMT_TYPE_BROKER: dedResult = dedBrokerStatusDt; break;
               case RQMT_TYPE_NOCONF: dedResult = dedNoConfStatusDt; break;
               //case RQMT_TYPE_ECONFIRM: dedResult = dedEConfirmStatusDt; break;
               //case RQMT_TYPE_ECONFIRM_BROKER: dedResult = dedEConfirmBrokerStatusDt; break;
               //case RQMT_TYPE_EFET_CPTY: dedResult = dedEfetCptyStatusDt; break;
               //case RQMT_TYPE_EFET_BROKER: dedResult = dedEfetBrokerStatusDt; break;
               case RQMT_TYPE_VERBAL: dedResult = dedVerbalStatusDt; break;
               //case RQMT_TYPE_MISC: dedResult = dedMiscStatusDt; break;
               default: throw new Exception("Internal Exception: Invalid RqmtType=" + ARqmtType.ToString());
            }
            return dedResult;
         }
         catch (Exception error)
         {
             XtraMessageBox.Show("An error occurred while retrieving the Status Date editor for Rqmt Type (numeric value: 0-4): " + ARqmtType + "." + Environment.NewLine +
                    "Error CNF-271 in " + FORM_NAME + ".GetStatusDateEditor(): " + error.Message,
                  FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
             return dedResult;
         }
      }

      private CheckEdit GetSecondCheckEditor(int ARqmtType)
      {
         CheckEdit ckedResult = null;
         try
         {
            switch (ARqmtType)
            {
               case RQMT_TYPE_SEMPRA: ckedResult = ckedSempra2ndChck; break;
               case RQMT_TYPE_CPTY: ckedResult = ckedCpty2ndChck; break;
               case RQMT_TYPE_BROKER: ckedResult = ckedBroker2ndChck; break;
               case RQMT_TYPE_NOCONF: ckedResult = ckedNoConf2ndChck; break;
               //case RQMT_TYPE_ECONFIRM: ckedResult = ckedEConfirm2ndChck; break;
               //case RQMT_TYPE_ECONFIRM_BROKER: ckedResult = ckedEConfirmBroker2ndChck; break;
               //case RQMT_TYPE_EFET_CPTY: ckedResult = ckedEfetCpty2ndChck; break;
               //case RQMT_TYPE_EFET_BROKER: ckedResult = ckedEfetBroker2ndChck; break;
               case RQMT_TYPE_VERBAL: ckedResult = ckedVerbal2ndChck; break;
               //case RQMT_TYPE_MISC: ckedResult = ckedMisc2ndChck; break;
               default: throw new Exception("Internal Exception: Invalid RqmtType=" + ARqmtType.ToString());
            }
            return ckedResult;
         }
         catch (Exception error)
         {
             XtraMessageBox.Show("An error occurred while retrieving the Second Check checkmark editor for Rqmt Type (numeric value: 0-4): " + ARqmtType + "." + Environment.NewLine +
                    "Error CNF-272 in " + FORM_NAME + ".GetSecondCheckEditor(): " + error.Message,
                  FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
             return ckedResult;
         }
      }

      private TextEdit GetReferenceEditor(int ARqmtType)
      {
         TextEdit txtedResult = null;
         try
         {
            switch (ARqmtType)
            {
               case RQMT_TYPE_SEMPRA: txtedResult = txtedSempraRef; break;
               case RQMT_TYPE_CPTY: txtedResult = txtedCptyRef; break;
               case RQMT_TYPE_BROKER: txtedResult = txtedBrokerRef; break;
               case RQMT_TYPE_NOCONF: txtedResult = txtedNoConfRef; break;
               //case RQMT_TYPE_ECONFIRM: txtedResult = txtedEConfirmRef; break;
               //case RQMT_TYPE_ECONFIRM_BROKER: txtedResult = txtedEConfirmBrokerRef; break;
               //case RQMT_TYPE_EFET_CPTY: txtedResult = txtedEfetCptyRef; break;
               //case RQMT_TYPE_EFET_BROKER: txtedResult = txtedEfetBrokerRef; break;
               case RQMT_TYPE_VERBAL: txtedResult = txtedVerbalRef; break;
               //case RQMT_TYPE_MISC: txtedResult = txtedMiscRef; break;
               default: throw new Exception("Internal Exception: Invalid RqmtType=" + ARqmtType.ToString());
            }
            return txtedResult;
         }
         catch (Exception error)
         {
             XtraMessageBox.Show("An error occurred while retrieving the Reference text editor for Rqmt Type (numeric value: 0-4): " + ARqmtType + "." + Environment.NewLine +
                    "Error CNF-273 in " + FORM_NAME + ".GetReferenceEditor(): " + error.Message,
                  FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
             return txtedResult;
         }
      }

      private MemoEdit GetRqmtCmtEditor(int ARqmtType)
      {
         MemoEdit memoResult = null;
         try
         {
            switch (ARqmtType)
            {
               case RQMT_TYPE_SEMPRA: memoResult = memoSempraCmt; break;
               case RQMT_TYPE_CPTY: memoResult = memoCptyCmt; break;
               case RQMT_TYPE_BROKER: memoResult = memoBrokerCmt; break;
               case RQMT_TYPE_NOCONF: memoResult = memoNoConfCmt; break;
               //case RQMT_TYPE_ECONFIRM: memoResult = memoEConfirmCmt; break;
               //case RQMT_TYPE_ECONFIRM_BROKER: memoResult = memoEConfirmBrokerCmt; break;
               //case RQMT_TYPE_EFET_CPTY: memoResult = memoEfetCptyCmt; break;
               //case RQMT_TYPE_EFET_BROKER: memoResult = memoEfetBrokerCmt; break;
               case RQMT_TYPE_VERBAL: memoResult = memoVerbalCmt; break;
               //case RQMT_TYPE_MISC: memoResult = memoMiscCmt; break;
               default: throw new Exception("Internal Exception: Invalid RqmtType=" + ARqmtType.ToString());
            }
            return memoResult;
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while retrieving the Rqmt Comment text editor for Rqmt Type (numeric value: 0-4): " + ARqmtType + "." + Environment.NewLine +
                   "Error CNF-274 in " + FORM_NAME + ".GetRqmtCmtEditor(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return memoResult;
         }
      }

      private ComboBoxEdit GetDispReasonEditor(int ARqmtType)
      {
         ComboBoxEdit comboResult = null;
         try
         {
            switch (ARqmtType)
            {
               case RQMT_TYPE_SEMPRA: comboResult = comboSempraDispReason; break;
               case RQMT_TYPE_CPTY: comboResult = comboCptyDispReason; break;
               case RQMT_TYPE_BROKER: comboResult = comboBrokerDispReason; break;
               case RQMT_TYPE_NOCONF: comboResult = comboNoConfDispReason; break;
               //case RQMT_TYPE_ECONFIRM: comboResult = comboEConfirmDispReason; break;
               //case RQMT_TYPE_ECONFIRM_BROKER: comboResult = comboEConfirmBrokerDispReason; break;
               //case RQMT_TYPE_EFET_CPTY: comboResult = comboEfetCptyDispReason; break;
               //case RQMT_TYPE_EFET_BROKER: comboResult = comboEfetBrokerDispReason; break;
               case RQMT_TYPE_VERBAL: comboResult = comboVerbalDispReason; break;
               //case RQMT_TYPE_MISC: comboResult = comboMiscDispReason; break;
               default: throw new Exception("Internal Exception: Invalid RqmtType=" + ARqmtType.ToString());
            }
            return comboResult;
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while retrieving the Disputed Reason text editor for Rqmt Type (numeric value: 0-4): " + ARqmtType + "." + Environment.NewLine +
                   "Error CNF-275 in " + FORM_NAME + ".GetDispReasonEditor(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return comboResult;
         }
      }

      private string GetRqmtStatusData(string ARqmtCode, string AStatusCode, string AResultColName)
      {
         string colorCode = "";
         try
         {
            string[] lookupKeys = new string[2];
            lookupKeys[0] = ARqmtCode;
            lookupKeys[1] = AStatusCode;
            DataRow row = rqmtStatusView.Rows.Find(lookupKeys);
            colorCode = row[AResultColName].ToString();
            return colorCode;
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while retrieving the Color code using the following lookup values: " + Environment.NewLine +
                "Rqmt Code: " + ARqmtCode + ", Status Code: " + AStatusCode + ", Column Name: " + AResultColName + Environment.NewLine +
                "Error CNF-276 in " + FORM_NAME + "GetRqmtStatusData(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return colorCode;
         }
      }

      private void SetSaveBtnVisible()
      {
         try
         {
            if (loadingData)
               return;

            bool changesMade = false;
            for (int i = 0; i < RQMT_ARRAY_MAX; i++)
            {
               if (activeRqmts[i])
               {
                  if (IsRqmtDataChanged(i))
                  {
                     changesMade = true;
                     break;
                  }
               }
            }

            if (IsTradeCmtChanged() || IsCptyTradeIdChanged())
               changesMade = true;

            btnEditRqmtSave.Enabled = changesMade;
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while setting the Save Button to enabled/disabled." + Environment.NewLine +
                "Error CNF-277 in " + FORM_NAME + ".SetSaveBtnVisible(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      //********************** Event Handlers ************************

      private void frmEditRqmt_Load(object sender, EventArgs e)
      {
         ReadUserSettings();
      }

      private void frmEditRqmt_FormClosing(object sender, FormClosingEventArgs e)
      {
         WriteUserSettings();
      }

      private void gridlueStatus_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
      {
         GridView view = sender as GridView;
         if (view.FocusedRowHandle != e.RowHandle)
         {
            string colorCode = view.GetRowCellDisplayText(e.RowHandle, view.Columns["ColorCode"]);
            e.Appearance.BackColor = Color.FromName(colorCode);
         }
      }

      private void gluedStatus_EditValueChanged(object sender, EventArgs e)
      {
         try
         {
            if (loadingData)
               return;
            GridLookUpEdit editor = sender as GridLookUpEdit;
            int rqmtType = int.Parse(editor.Tag.ToString());
            string colorCode = "";

            if (RQMT_CODES[rqmtType] == "XQCSP")
            {
               if (editor.EditValue.ToString() == "NEW" ||
                  editor.EditValue.ToString() == "PREP" ||
                  editor.EditValue.ToString() == "EXT_REVIEW" ||
                  editor.EditValue.ToString() == "TRADER" ||
                  editor.EditValue.ToString() == "MGR" ||
                  editor.EditValue.ToString() == "OK_TO_SEND")
               {
                  if (SingleOrMultiMode == SINGLE)
                  {
                     editor.Text = oldStatusCode[rqmtType];
                     colorCode = GetRqmtStatusData(RQMT_CODES[rqmtType], oldStatusCode[rqmtType], "ColorCode");
                     editor.Properties.Appearance.BackColor = Color.FromName(colorCode);
                     GetStatusDateEditor(rqmtType).DateTime = oldStatusDate[rqmtType];
                  }
                  else
                  {
                     editor.Text = "";
                     editor.Properties.Appearance.BackColor = Color.White;
                     GetStatusDateEditor(rqmtType).EditValue = null;
                  }
                  XtraMessageBox.Show("This status can only be changed from Confirm Editor.");
                  return;
               }
            }

            if (SingleOrMultiMode == SINGLE)
            {
                //Israel 9/28/2015
               //if (editor.EditValue.ToString() == "APPR" &&
               //    GetSecondCheckEditor(rqmtType).Checked &&
               //    userName == prelimApprover[rqmtType])
               //{
               //   editor.EditValue = "PRELIM";
               //   XtraMessageBox.Show("Approval is required by a different person.");
               //   return;
               //}

               if (isSecondCheckCreateCxl && GetSecondCheckEditor(rqmtType).Visible)
               {
                  string terminalFlag = GetRqmtStatusData(RQMT_CODES[rqmtType], editor.EditValue.ToString(),
                                          "TerminalFlag");
                  GetSecondCheckEditor(rqmtType).Enabled = terminalFlag != "Y" &&
                                                           editor.EditValue.ToString() != "PRELIM";
               }
            }


            if (editor.Properties.View.FocusedRowHandle > -1)
            {
               colorCode = editor.Properties.View.GetFocusedRowCellValue("ColorCode").ToString();
               editor.Properties.Appearance.BackColor = Color.FromName(colorCode);
            }

            GetDispReasonEditor(rqmtType).Visible = editor.EditValue.ToString() == "DISP";
            if (editor.EditValue.ToString() != RqmtInitialStatus[rqmtType] &&
                editor.EditValue.ToString() != "")
               GetStatusDateEditor(rqmtType).DateTime = DateTime.Today;

            StatusBarItems[rqmtType].Enabled = IsRqmtDataChanged(rqmtType);
            SetSaveBtnVisible();
         }
         catch (Exception error)
         {
             XtraMessageBox.Show("An error occurred while processing a Status Code entry." + Environment.NewLine +
                "Error CNF-278 in " + FORM_NAME + ".gluedStatus_EditValueChanged(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void comboDispReason_EditValueChanged(object sender, EventArgs e)
      {
         try
         {
            if (loadingData)
               return;
            ComboBoxEdit editor = sender as ComboBoxEdit;
            int rqmtType = int.Parse(editor.Tag.ToString());
            MemoEdit memo = GetRqmtCmtEditor(rqmtType);
            memo.Text = editor.EditValue.ToString();
         }
         catch (Exception error)
         {
             XtraMessageBox.Show("An error occurred while processing a Disputed Reason entry." + Environment.NewLine +
                "Error CNF-279 in " + FORM_NAME + ".comboDispReason_EditValueChanged(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
     }

      private void dedStatusDt_DateTimeChanged(object sender, EventArgs e)
      {
         try
         {
            //focused not working...
            if (loadingData)
               return;
            DateEdit editor = sender as DateEdit;
            int rqmtType = int.Parse(editor.Tag.ToString());
            StatusBarItems[rqmtType].Enabled = IsRqmtDataChanged(rqmtType);
            SetSaveBtnVisible();
         }
         catch (Exception error)
         {
             XtraMessageBox.Show("An error occurred while processing a Status Date entry." + Environment.NewLine +
                "Error CNF-280 in " + FORM_NAME + ".dedStatusDt_DateTimeChanged(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void cked2ndChck_EditValueChanged(object sender, EventArgs e)
      {
         try
         {
            if (loadingData)
               return;
            CheckEdit editor = sender as CheckEdit;
            int rqmtType = int.Parse(editor.Tag.ToString());
            StatusBarItems[rqmtType].Enabled = IsRqmtDataChanged(rqmtType);
            SetSaveBtnVisible();
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while processing a Second Check entry." + Environment.NewLine +
                "Error CNF-281 in " + FORM_NAME + ".cked2ndChck_EditValueChanged(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void txtedRef_EditValueChanged(object sender, EventArgs e)
      {
         try
         {
            if (loadingData)
               return;
            TextEdit editor = sender as TextEdit;
            int rqmtType = int.Parse(editor.Tag.ToString());
            StatusBarItems[rqmtType].Enabled = IsRqmtDataChanged(rqmtType);
            SetSaveBtnVisible();
         }
         catch (Exception error)
         {
             //12/09/2015 Israel -- Skipped message sequence number to add one to frmMain.
            XtraMessageBox.Show("An error occurred while processing a Reference entry." + Environment.NewLine +
                "Error CNF-283 in " + FORM_NAME + ".txtedRef_EditValueChanged(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void memoCmt_EditValueChanged(object sender, EventArgs e)
      {
         try
         {
            if (loadingData)
               return;
            MemoEdit editor = sender as MemoEdit;
            int rqmtType = int.Parse(editor.Tag.ToString());
            StatusBarItems[rqmtType].Enabled = IsRqmtDataChanged(rqmtType);
            SetSaveBtnVisible();
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while processing a Comment entry." + Environment.NewLine +
                "Error CNF-284 in " + FORM_NAME + ".memoCmt_EditValueChanged(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void txtedCptyTradeId_EditValueChanged(object sender, EventArgs e)
      {
          try
          {
              if (loadingData)
                  return;
              SetSaveBtnVisible();
              bstatCptyTradeId.Enabled = IsCptyTradeIdChanged();
          }
          catch (Exception error)
          {
              XtraMessageBox.Show("An error occurred while processing a Cpty Trade Id entry." + Environment.NewLine +
                  "Error CNF-548 in " + FORM_NAME + ".txtedCptyTradeId_EditValueChanged(): " + error.Message,
                   FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
          }
      }

      private void memoTradeCmt_EditValueChanged(object sender, EventArgs e)
      {
         try
         {
            if (loadingData)
               return;
            SetSaveBtnVisible();
            bstatTradeCmt.Enabled = IsTradeCmtChanged();
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while processing a Trade Comment entry." + Environment.NewLine +
                "Error CNF-285 in " + FORM_NAME + ".memoTradeCmt_EditValueChanged(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void btnEditRqmtSaveAndApprove_Click(object sender, EventArgs e)
      {
         try
         {
            for (int i = 0; i < RQMT_ARRAY_MAX; i++)
            {
                //Israel 8/13/15 -- removed ECONFIRM and EFET
               //if (activeRqmts[i] && 
               //    i != RQMT_TYPE_ECONFIRM &&
               //    i != RQMT_TYPE_EFET_CPTY &&
               //    i != RQMT_TYPE_EFET_BROKER)
               if (activeRqmts[i])
                   {
                  string statusCode = "";
                  if (i == RQMT_TYPE_VERBAL)
                     statusCode = "CONF";
                  else
                     statusCode = "APPR";

                  string rqmtCode = RQMT_CODES[i];
                  string colorCode = GetRqmtStatusData(rqmtCode, statusCode, "ColorCode");
                  GetStatusCodeEditor(i).EditValue = statusCode;
                  GetStatusCodeEditor(i).Properties.Appearance.BackColor = Color.FromName(colorCode);
               }
            }
            isFinalApprove = true;
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while processing the Save and Approve button click." + Environment.NewLine +
                "Error CNF-286 in " + FORM_NAME + ".btnEditRqmtSaveAndApprove_Click(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void barbtnClearStatus_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         try
         {
            if (SingleOrMultiMode == MULTI)
            {
               for (int i=0;i < RQMT_ARRAY_MAX;i++)
               {
                  if (GetStatusCodeEditor(i).Focused)
                  {
                     GetStatusCodeEditor(i).EditValue = "";
                     GetStatusCodeEditor(i).Properties.Appearance.BackColor = Color.White;
                     GetStatusDateEditor(i).EditValue = null;
                     break;
                  }
               }               
            }
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while processing the Clear Status button click." + Environment.NewLine +
                "Error CNF-287 in " + FORM_NAME + ".barbtnClearStatus_ItemClick(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void popupStatusCode_BeforePopup(object sender, CancelEventArgs e)
      {
         try
         {
            if (SingleOrMultiMode != MULTI)
               e.Cancel = true;
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while processing the ." + Environment.NewLine +
                "Error CNF-288 in " + FORM_NAME + ".popupStatusCode_BeforePopup(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

   }

}