using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CommonUtils;
using ConfirmInbound;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
//using ConfirmManager.ConfirmServices;
using OpsTrackingModel;
using DataManager;
using DBAccess.SqlServer;



namespace ConfirmManager
{
   public partial class frmEditContract : DevExpress.XtraEditors.XtraForm
   {
      private const string FORM_NAME = "frmEditContract";
      private const string FORM_ERROR_CAPTION = "Edit Contract Form Error";
      public string settingsDir;
      private DataTable clauseHeaderTable;
      private DataTable clauseBodyTable;
      public DataTable rqmtStatusColorTable;
      public string marginToken;
      public string liveTransMethod = "";
      public string liveFaxNumber = "";
      private string oldFaxTelexNumber = "";
      private string newTransMethod = "";
      private string newFaxNumber = "";
      private bool wasMarginToken = false;
      public bool isMarginToken = false;
      private bool isSavingForm = false;
      public bool isApproveAndSend = false;
      public frmAssignFaxNo assignFaxNoForm = new frmAssignFaxNo();
      public frmClauseViewer clauseViewerForm = new frmClauseViewer();
      private const int INSERT = 0;
      private const int EDIT = 1;
      private const string SEMPRA_RQMT = "XQCSP";
      public bool isContract = false;
      private bool isContractOkAccess = false;
      private bool isPaperCreator = false;
      private string initialStatus = "";
      private int checkPrint;
      private string cptySn = "";
      public static string p_UserId = "";
      //public ConfirmationWeb confirmationService = new ConfirmationWeb();

      //private enum phoneNumberSegment { CountryCode = 0, AreaCode, LocalNumber };
      private const string FAX_NO_NONE = "**NONE**";

      public frmEditContract()
      {
         InitializeComponent();
      }

      private void frmEditContract_Load(object sender, EventArgs e)
      {
         this.ReadUserSettings();
         SetDisplayCmts();
      }

      private void frmEditContract_FormClosing(object sender, FormClosingEventArgs e)
      {
         if (!isSavingForm)
         {
            WriteUserSettings();
            return;
         }

         isMarginToken = IsMarginToken();
         if (wasMarginToken && !isMarginToken)
         {
            XtraMessageBox.Show("Margin Token was deleted.",
               "Margin Token ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            isSavingForm = false;
            e.Cancel = true;
         }
         else if (IsNonMarginToken() && comboStatus.Text != "PREP")
         {
            XtraMessageBox.Show("Token has not been resolved.",
               "Unresolved Token ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            isSavingForm = false;
            e.Cancel = true;
         }
         else
            this.WriteUserSettings();
      }

      private void ReadUserSettings()
      {
         try
         {
            //Now read user settings, ReadAppSettings() must be called first
            Sempra.Ops.IniFile iniFile = new Sempra.Ops.IniFile(FileNameUtils.GetUserIniFileName(settingsDir));

            this.Top = iniFile.ReadValue(FORM_NAME, "Top", 200);
            this.Left = iniFile.ReadValue(FORM_NAME, "Left", 300);
            this.Width = iniFile.ReadValue(FORM_NAME, "Width", 750);
            this.Height = iniFile.ReadValue(FORM_NAME, "Height", 450);
            cedDisplayCmts.Checked = iniFile.ReadValue(FORM_NAME, "DisplayComments", true); 
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while reading the user settings file." + Environment.NewLine +
                   "Error CNF-222 in " + FORM_NAME + "ReadUserSettings(): " + error.Message,
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
            iniFile.WriteValue(FORM_NAME, "Width", this.Width);
            iniFile.WriteValue(FORM_NAME, "Height", this.Height);
            iniFile.WriteValue(FORM_NAME, "DisplayComments", cedDisplayCmts.Checked);
         }
         catch (Exception error)
         {
             XtraMessageBox.Show("An error occurred while saving the user settings file." + Environment.NewLine +
                    "Error CNF-223 in " + FORM_NAME + ".WriteUserSettings(): " + error.Message,
                  FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void SetDisplayCmts()
      {
         if (cedDisplayCmts.Checked)
            this.pnlContractEditor.SetBounds(0, 0, this.Size.Width - 8, 92);
         else
            this.pnlContractEditor.SetBounds(0, 0, this.Size.Width - 8, 40);
      }

      //private static ConfirmManager.ConfirmServices.@string GetConfirmationServiceUserName()
      //{
      //   try
      //   {
      //      //string userFullName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
      //      string userFullName = p_UserId;
      //      string userName = userFullName.Substring(userFullName.LastIndexOf("\\") + 1);
      //      ConfirmManager.ConfirmServices.@string uName = new ConfirmManager.ConfirmServices.@string();
      //      uName.Text = new string[] { userName };
      //      return uName;
      //   }
      //   catch (Exception except)
      //   {
      //      throw new Exception("frmEditContract.GetConfirmationServiceUserName: " + except.Message);
      //   }
      //}

      public void InitForm(string AContractBody, DataTable AClauseHeader, DataTable AClauseBody,
         string ATradingSystem, string ACurrentStatus, bool AContractOkAccess, string ASeCptySn,
         string ACptySn, bool AIsFaxNumber, bool AIsPaperCreator, string APreparerCanSendFlag)
      {
         try
         {
            bool isAllowChanges =
               ACurrentStatus == "NEW" ||
               ACurrentStatus == "PREP" ||
               ACurrentStatus == "TRADER" ||
               ACurrentStatus == "MGR";

            if (ACurrentStatus == "MGR" && !(AContractOkAccess || APreparerCanSendFlag == "Y"))
               isAllowChanges = false;
            bool isSelectionProtected = !isAllowChanges;

            //12/21/09 Israel -- Don't give TRADER option to London
            bool isLondonTrade =
                ASeCptySn == "SET EUROPE" ||
                ASeCptySn == "RBS SEEL" ||
                ASeCptySn == "SEMPRA KFT" ||
                ASeCptySn == "SEMPRA SRO";

            isPaperCreator = AIsPaperCreator;

            this.cptySn = ACptySn;
            //confirmationService.userName = GetConfirmationServiceUserName();

            //rtbEditContract.Clear();
            //9/15/09 Israel -- eliminate exception when processing a null contract body.
            if (AContractBody != null)
                if (AContractBody.Trim().Length > 0)
                {
                   rtbEditContract.Rtf = AContractBody;
                   rtbEditContract.SelectAll();
                   rtbEditContract.SelectionProtected = isSelectionProtected;
                   rtbEditContract.Select(5, 0);
                }

            isContractOkAccess = (AContractOkAccess || APreparerCanSendFlag == "Y");
            initialStatus = ACurrentStatus;
            SetOkAndSendVisible(initialStatus, isContractOkAccess, AIsFaxNumber, AIsPaperCreator);

            clauseHeaderTable = new DataTable();
            clauseHeaderTable = AClauseHeader.Copy();
            clauseBodyTable = new DataTable();
            clauseBodyTable = AClauseBody.Copy();

            //5/27/09 Israel -- Creator can't approve
            InitStatusComboBox(ATradingSystem, ACurrentStatus, AContractOkAccess, AIsPaperCreator, isLondonTrade,APreparerCanSendFlag);
            //comboStatus.Text = ACurrentStatus;
            comboStatus.BackColor = GetHashkeyColor(SEMPRA_RQMT + ACurrentStatus);

            InitPopupMenu();
            wasMarginToken = IsMarginToken();

            isSavingForm = false;
            isApproveAndSend = false;
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while setting up initial values for the form." + Environment.NewLine +
                   "Error CNF-224 in " + FORM_NAME + ".InitForm(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
         finally
         {
            //this.dbtnClauses.Enabled = true;
         }
      }

      private void SetOkAndSendVisible(string ACurrentStatus, bool AContractOkAccess, 
         bool AIsFaxNumber, bool AIsPaperCreator)
      {
         btnContractOkAndSend.Visible = AContractOkAccess && AIsFaxNumber &&
            (ACurrentStatus == "MGR" || ACurrentStatus == "OK_TO_SEND") &&
            !AIsPaperCreator; 
      }

      private void InitPopupMenu()
      {
         try
         {
            string categoryLabel = "";
            ArrayList categoryList = new ArrayList();
            //Load up an array list for unique, non-blank iterating
            foreach (DataRow row in clauseHeaderTable.Rows)
            {
               categoryLabel = row["Category"].ToString();
               if (categoryList.IndexOf(categoryLabel) == -1 &&
                   categoryLabel.Trim().Length > 0)
                  categoryList.Add(categoryLabel);
            }

            categoryList.Sort();
            BarManager bManager = new BarManager();
            string filterStr = "";
            //For each item in array list, select all items
            for (int i = 0; i < categoryList.Count; i++)
            {
               categoryLabel = categoryList[i].ToString();
               DevExpress.XtraBars.BarSubItem barSubItem = new DevExpress.XtraBars.BarSubItem(bManager, categoryLabel);
                
               filterStr = "Category = '" + categoryLabel + "'";
               foreach (DataRow row in clauseHeaderTable.Select(filterStr))
               {
                  BarItem item = new BarButtonItem();
                  item.Caption = row["ShortName"].ToString();
                  item.Tag = (int)row["PrmntConfirmClauseId"];
                  item.ItemClick += ClauseBtnClick;
                  barSubItem.AddItem(item);
               }
               pmenuEditContract.AddItem(barSubItem);
            }
         }
         catch (Exception ex)
         {
            throw new Exception("An error occurred while setting up the popup menu data." + Environment.NewLine +
                 "Error CNF-225 in " + FORM_NAME + ".InitPopupMenu(): " + ex.Message);
         }
      }

      private string GetClauseBodyText(int APrmntConfirmClauseId)
      {
         string bodyText = "";
         try
         {
            string filterStr = "PrmntConfirmClauseId = '" + APrmntConfirmClauseId.ToString() + "'";
            foreach (DataRow row in clauseBodyTable.Select(filterStr))
            {
               bodyText += row["Body"].ToString();
            }
            return bodyText;
         }
         catch (Exception ex)
         {
            throw new Exception("An error occurred while retrieving the clause body text." + Environment.NewLine +
                 "Error CNF-226 in " + FORM_NAME + ".GetClauseBodyText(): " + ex.Message);
         }
      }

      private void InitStatusComboBox(string ATradingSystem, string ACurrentStatusCode,
         bool AContractOkAccess, bool AIsCreator, bool AIsLondonTrade, string APreparerCanSendFlag)
      {
         try
         {
            //Initialize
            //5/27/09 Israel -- Creator can't approve
            comboStatus.Text = ACurrentStatusCode;
            bool preparerCanSendFlag = APreparerCanSendFlag == "Y" ? true : false;
            switch (ACurrentStatusCode)
            {
               case "NEW":
                  comboStatus.Properties.Items.Add("PREP");
                  if (ATradingSystem != "JMS" && !AIsLondonTrade && isContract)
                     comboStatus.Properties.Items.Add("TRADER");
                  comboStatus.Properties.Items.Add("MGR");
                  if ((AContractOkAccess || preparerCanSendFlag) && !AIsCreator)
                     comboStatus.Properties.Items.Add("OK_TO_SEND");
                  //Override for new
                  comboStatus.Text = "PREP";
                  break;
               case "PREP":
                  comboStatus.Properties.Items.Add("PREP");
                  if (ATradingSystem != "JMS" && !AIsLondonTrade && isContract)
                     comboStatus.Properties.Items.Add("TRADER");
                  comboStatus.Properties.Items.Add("MGR");
                  if ((AContractOkAccess || preparerCanSendFlag) && !AIsCreator)
                     comboStatus.Properties.Items.Add("OK_TO_SEND");
                  break;
               case "TRADER":
                  comboStatus.Properties.Items.Add("PREP");
                  if (ATradingSystem != "JMS" && !AIsLondonTrade && isContract)
                     comboStatus.Properties.Items.Add("TRADER");
                  comboStatus.Properties.Items.Add("MGR");
                  break;
               case "MGR":
                  comboStatus.Properties.Items.Add("PREP");
                  if (ATradingSystem != "JMS" && !AIsLondonTrade && isContract)
                     comboStatus.Properties.Items.Add("TRADER");
                  comboStatus.Properties.Items.Add("MGR");
                  if ((AContractOkAccess || preparerCanSendFlag) && !AIsCreator)
                     comboStatus.Properties.Items.Add("OK_TO_SEND");
                  break;
               case "OK_TO_SEND":
                  comboStatus.Properties.Items.Add("PREP");
                  if (ATradingSystem != "JMS" && !AIsLondonTrade && isContract)
                     comboStatus.Properties.Items.Add("TRADER");
                  comboStatus.Properties.Items.Add("MGR");
                  if ((AContractOkAccess || preparerCanSendFlag) && !AIsCreator)
                     comboStatus.Properties.Items.Add("OK_TO_SEND");
                  break;
               default:
                  btnEditContractSave.Enabled = false;
                  comboStatus.Text = ACurrentStatusCode;
                  comboStatus.Enabled = false;
                  break;
            }
         }
         catch (Exception ex)
         {
            throw new Exception("An error occurred while setting up the Status dropdown list for the following values:" + Environment.NewLine +
                "Trading System: " + ATradingSystem + ", Current Status Code: " + ACurrentStatusCode + ", Is Contract Access OK?: " + AContractOkAccess + Environment.NewLine +
                ", Is Contract Creator?: " + AIsCreator + ", Is London Trade?: " + AIsLondonTrade + Environment.NewLine +
                 "Error CNF-227 in " + FORM_NAME + ".InitStatusComboBox(): " + ex.Message);
         }
      }

      private bool IsMarginToken()
      {
         bool isMarginToken = false;
         try
         {
            int startPos = 0;
            int toEnd = rtbEditContract.Text.Length - startPos;
            RichTextBoxFinds rtbFinds = RichTextBoxFinds.None;
            if (rtbEditContract.Find(marginToken, startPos, toEnd, rtbFinds) > -1)
               isMarginToken = true;
            
            return isMarginToken;
         }
         catch (Exception ex)
         {
            throw new Exception("An error occurred while determining if an unresolved margin token exists in the current document." + Environment.NewLine +
                 "Error CNF-228 in " + FORM_NAME + ".IsMarginToken(): " + ex.Message);
         }
      }

      private bool IsNonMarginToken()
      {
         bool isNonMarginToken = false;
         try
         {
            int foundPos = 0;
            int foundPos2 = 0;
            int startPos = 0;
            int toEnd = rtbEditContract.Text.Length - startPos;
            RichTextBoxFinds rtbFinds = RichTextBoxFinds.None;
            while (startPos < toEnd)
            {
               foundPos = rtbEditContract.Find("[", startPos, toEnd, rtbFinds);
               foundPos2 = rtbEditContract.Find(marginToken, startPos, toEnd, rtbFinds);
               if ((foundPos > -1) &&
                   (rtbEditContract.Find(marginToken, startPos, toEnd, rtbFinds) != foundPos))
               {
                  isNonMarginToken = true;
                  break;
               }

               if (foundPos > -1)
                  startPos = foundPos + 1;
               else
                  startPos = toEnd;
            }
            
            return isNonMarginToken;
         }
         catch (Exception ex)
         {
            throw new Exception("An error occurred while determining if the currently selected token is a margin token or not." + Environment.NewLine +
                 "Error CNF-229 in " + FORM_NAME + ".IsNonMarginToken(): " + ex.Message);
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
            throw new Exception("An error occurred while retrieving the color for Hashkey: " + AHashkey + "." + Environment.NewLine +
                 "Error CNF-230 in " + FORM_NAME + ".GetHashkeyColor(): " + error.Message);
         }
         return color;
      }

      private void ClauseBtnClick(object sender, ItemClickEventArgs e)
      {
         try
         {
            int prmntConfirmClauseId = (int)e.Item.Tag;
            string clauseBodyText = GetClauseBodyText(prmntConfirmClauseId);
            Clipboard.Clear();
            TextDataFormat tdFormat = TextDataFormat.Rtf;
            Clipboard.SetText(clauseBodyText, tdFormat);
            rtbEditContract.Paste();
         }
         catch (Exception ex)
         {
            throw new Exception("An error occurred while setting the value for the selected clause." + Environment.NewLine +
                 "Error CNF-231 in " + FORM_NAME + ".ClauseBtnClick(): " + ex.Message);
         }
      }

      public void SetFaxNumbers(string ANewFaxNumber, string ALiveFaxNumber)
      {
         try
         {
            newFaxNumber = ANewFaxNumber;

            liveTransMethod = "FAX";
            if (ALiveFaxNumber.Trim().Length > 0)
            {
               //editMode = EDIT; 
               liveFaxNumber = ALiveFaxNumber;
            }
            else
            {
               //editMode = INSERT;
               if (ANewFaxNumber.Trim().Length > 0)
               {
                  liveFaxNumber = ANewFaxNumber;
               }
               else
                  liveFaxNumber = "";
            }

            SetEditFaxNumberCaption(liveFaxNumber);
         }
         catch (Exception ex)
         {
            XtraMessageBox.Show("An error occurred while setting the fax number." + Environment.NewLine +
                    "Error CNF-232 in " + FORM_NAME + ".SetFaxNumbers(): " + ex.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void SetEditFaxNumberCaption(string ALiveFaxNumber)
      {
         try
         {
            string transMethodDisplay = "faxed";
            if (ALiveFaxNumber.Contains("@"))
               transMethodDisplay = "emailed";

            if (liveFaxNumber.Trim().Length > 0)
               bbtnEditFax.Caption = "Confirm will be " + transMethodDisplay + " to " + liveFaxNumber;
            else
               bbtnEditFax.Caption = "No email address or fax number has been entered!";

            ribbonStatusBar.Refresh();
         }
         catch (Exception ex)
         {
            throw new Exception("An error occurred while setting: " + ALiveFaxNumber + " to be the current SendTo Address." + Environment.NewLine +
                 "Error CNF-233 in " + FORM_NAME + ".SetEditFaxNumberCaption(): " + ex.Message);
         }
      }

      private void ViewCptyInfo(string pCptyShortName)
      {
         //CptyInfo cptyInfoData = new CptyInfo();
         //List<CptyAgreement> cptyAgreementList = new List<CptyAgreement>();
         //List<ContractFaxNo> contractFaxNoList = new List<ContractFaxNo>();
         //CptyInfoDal cptyInfoDal = new CptyInfoDal(Properties.Settings.Default.CptyInfoAPIUrl);
         //CptyAgreementDal cptyAgreementDal = new CptyAgreementDal();
         //CptyFaxNoDal contractFaxNoDal = new CptyFaxNoDal();
         try
         {
            #region remove soon

            //ConfirmationService confirmService = new ConfirmationService();
            //confirmService.userName = GetConfirmServiceUserName();

            //getCptyInfo cptyInfo = new getCptyInfo();
            //getCptyAgreementList cptyAgreements = new getCptyAgreementList();
            //getContractFaxList contractFaxList = new getContractFaxList();

            //cptyInfoRequest req = new cptyInfoRequest();
            //agreementInfoRequest req2 = new agreementInfoRequest();
            //contractFaxRequest req3 = new contractFaxRequest();

            //req.cptySn = cptyShortName;
            //req2.cptySn = cptyShortName;
            //req3.cptySn = cptyShortName;

            //getCptyInfoResponse res = new getCptyInfoResponse();
            //getCptyAgreementListResponse res2 = new getCptyAgreementListResponse();
            //getContractFaxListResponse res3 = new getContractFaxListResponse();

            //cptyInfo.infoRequest = req;
            //cptyAgreements.agreementRequest = req2;
            //contractFaxList.contractFaxRequest = req3;

            //res = confirmationService.getCptyInfo(cptyInfo);

            //if (res.@return.responseStatus == "OK")
            //{
            //   cptyInfoData.CptyAddress1 = res.@return.address1;
            //   cptyInfoData.CptyAddress2 = res.@return.address2;
            //   cptyInfoData.CptyAddress3 = res.@return.address3;
            //   cptyInfoData.CptyCity = res.@return.city;
            //   cptyInfoData.CptyCountry = res.@return.country;
            //   cptyInfoData.CptyState = res.@return.state;
            //   cptyInfoData.CptyLegalName = res.@return.legalName;
            //   cptyInfoData.CptyShortName = res.@return.cptySn;
            //   cptyInfoData.CptyZipcode = res.@return.zip;

            //   cptyInfoData.CptyMainFaxCntryCode = res.@return.faxCountryCode;
            //   cptyInfoData.CptyMainFaxAreaCode = res.@return.faxAreaCode;
            //   cptyInfoData.CptyMainFax = res.@return.faxNumber;

            //   cptyInfoData.CptyMainPhoneCntryCode = res.@return.phoneCountryCode;
            //   cptyInfoData.CptyMainPhoneAreaCode = res.@return.phoneAreaCode;
            //   cptyInfoData.CptyMainPhone = res.@return.phoneNumber;
            //}
            //else
            //{
            //   throw new Exception("Exception: Error getting counterparty info. " + res.@return.responseText);
            //}

            //res2 = confirmationService.getCptyAgreementList(cptyAgreements);
            //if (res2.@return.responseStatus == "OK")
            //{
            //   if (res2.@return.data != null)
            //      foreach (agreementData data in res2.@return.data)
            //      {
            //         CptyAgreement agreement = new CptyAgreement();
            //         agreement.AgreementId = data.agreementId;
            //         agreement.AgrmntTypeCode = data.agreementTypeCode;
            //         agreement.Cmt = data.comment;
            //         agreement.CptyId = data.cptyId;
            //         agreement.CptyShortName = data.cptySn;
            //         agreement.DateSigned = data.dateSigned;
            //         agreement.SeAgrmntContactName = data.contactName;
            //         agreement.SeCptyId = data.sempraCompanyId;
            //         agreement.StatusInd = data.statusInd;
            //         agreement.TerminationDt = data.terminationDate;
            //         agreement.SeCptyShortName = data.sempraCompanySn;

            //         cptyInfoData.CptyAgreements.Add(agreement);
            //      }
            //}
            //else
            //{
            //   throw new Exception("Exception: Error getting counterparty agreements list. " + res2.@return.responseText);
            //}

            //res3 = confirmationService.getContractFaxList(contractFaxList);
            //if (res3.@return.responseStatus == "OK")
            //{
            //   if (res3.@return.data != null)
            //      foreach (contractFaxData data in res3.@return.data)
            //      {
            //         ContractFaxNo contractFax = new ContractFaxNo();
            //         contractFax.ActiveFlag = data.activeFlag;
            //         contractFax.AreaCode = data.areaCode;
            //         contractFax.CountryPhoneCode = data.countryPhoneCode;
            //         contractFax.CptyId = data.cptyId;
            //         contractFax.Description = data.description;
            //         contractFax.DesignationCode = data.designationCode;
            //         contractFax.DsgActiveFlag = data.dsgActiveFlag;
            //         contractFax.LocalNumber = data.localNumber;
            //         contractFax.PhoneId = data.phoneId;
            //         contractFax.PhoneTypeCode = data.phoneTypeCode;
            //         contractFax.ShortName = data.shortName;

            //         cptyInfoData.ContractFaxNumbers.Add(contractFax);
            //      }
            //}
            //else
            //{
            //   throw new Exception("Exception: Error getting Contract Fax List. " + res3.@return.responseText);
            //}

            #endregion

             //cptyInfoData = cptyInfoDal.GetInfo(pCptyShortName);
             //cptyAgreementList = cptyInfoDal.GetAgreementList(pCptyShortName);
             //cptyInfoData.CptyAgreements = cptyAgreementList;

             //contractFaxNoList = cptyInfoDal.GetFaxNoList(pCptyShortName);
             //cptyInfoData.ContractFaxNumbers = contractFaxNoList;

             //frmCptyInfo cptyInfoForm = new frmCptyInfo(cptyInfoData);
             //cptyInfoForm.ShowDialog();
         }
         catch (Exception ex)
         {
            throw new Exception("An error occurred while displaying Cpty Info for: " + pCptyShortName + "." + Environment.NewLine +
                 "Error CNF-234 in " + FORM_NAME + ".ViewCptyInfo(): " + ex.Message);
         }
      }

      private void bbtnEditFax_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
      {
         assignFaxNoForm.SetFaxNumbers(newFaxNumber, liveFaxNumber);         
         if (assignFaxNoForm.ShowDialog(this) == DialogResult.OK)
         {
            liveTransMethod = "FAX";
            //if (assignFaxNoForm.radgrpTransMethod.SelectedIndex == 1)
               //liveTransMethod = "TELEX";
            liveFaxNumber = assignFaxNoForm.teditFaxTelexNumber.Text;
            SetEditFaxNumberCaption(liveFaxNumber);

            SetOkAndSendVisible(initialStatus, isContractOkAccess, liveFaxNumber.Trim().Length > 1, isPaperCreator);
         }
      }

      private void cedDisplayCmts_CheckedChanged(object sender, EventArgs e)
      {
         SetDisplayCmts();
      }

      private void dbtnClauses_Click(object sender, EventArgs e)
      {
         clauseViewerForm.settingsDir = this.settingsDir;
         clauseViewerForm.InitForm(clauseHeaderTable, clauseBodyTable);
         if (!clauseViewerForm.Visible)
            clauseViewerForm.Show();
         else
            clauseViewerForm.Focus();
      }

      private void btnEditContractSave_Click(object sender, EventArgs e)
      {
         isSavingForm = true;
      }

      private void btnCptyInfo_Click(object sender, EventArgs e)
      {
         try
         {
            ViewCptyInfo(cptySn);
         }
         catch (Exception ex)
         {
            XtraMessageBox.Show("An error occurred while preparing to display the Cpty Info." + Environment.NewLine +
                    "Error CNF-235 in " + FORM_NAME + ".btnCptyInfo_Click(): " + ex.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void barBtnOpenClauseViewer_ItemClick(object sender, ItemClickEventArgs e)
      {
         dbtnClauses_Click(null, null);
      }

      private void barBtnPasteClauses_ItemClick(object sender, ItemClickEventArgs e)
      {
         rtbEditContract.Paste();
      }

      private void btnApproveAndSend_Click(object sender, EventArgs e)
      {
         isApproveAndSend = true;
      }

      public void btnPrint_Click(object sender, EventArgs e)
      {
         if (printDialog.ShowDialog() == DialogResult.OK)
            printDocument.Print();
         //Israel 3/19/09 - Fixes problem with print dialog cancel cancelling this form.
         else
            this.DialogResult = DialogResult.None;
      }

      private void printDocument_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
      {
         checkPrint = 0;
      }

      private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
      {
         // Print the content of RichTextBox. Store the last character printed.
         checkPrint = rtbEditContract.Print(checkPrint, rtbEditContract.TextLength, e);

         // Check for more pages
         if (checkPrint < rtbEditContract.TextLength)
            e.HasMorePages = true;
         else
            e.HasMorePages = false;
      }

      private void comboStatus_EditValueChanged(object sender, EventArgs e)
      {
         comboStatus.BackColor = GetHashkeyColor(SEMPRA_RQMT + comboStatus.Text);
      }

      private void comboStatus_DrawItem(object sender, ListBoxDrawItemEventArgs e)
      {
         e.Appearance.BackColor = GetHashkeyColor(SEMPRA_RQMT + e.Item.ToString());
         e.Appearance.BorderColor = Color.Black;
         e.Appearance.Options.UseBorderColor = true;
         if (e.State == DrawItemState.Selected)
            e.Appearance.BackColor = Color.Blue;

            //e.Graphics.DrawLine(new Pen(new SolidBrush(Color.Black)),
            //   new Point(e.Bounds.Left, e.Bounds.Top),
            //   new Point(e.Bounds.Right, e.Bounds.Top)); 
      }

   }
}