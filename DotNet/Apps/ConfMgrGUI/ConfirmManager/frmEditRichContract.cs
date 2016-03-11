using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraBars.Helpers;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using System.IO;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
//using DevExpress.XtraRichEdit;
//using DevExpress.XtraRichEdit.API;
using System.Collections;
using CommonUtils;
using ConfirmInbound;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using Sempra.Ops;


namespace ConfirmManager
{
    public partial class frmEditRichContract : RibbonForm
    {
        private const string FORM_NAME = "frmEditRichContract";
        private const string FORM_ERROR_CAPTION = "Edit Rich Contract Form Error";
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

        //public ConfirmManager.RichTextBoxPrintCtrl rtbEditContract;


        public frmEditRichContract()
        {
            InitializeComponent();
            //InitSkinGallery();
            InitializeRichEditControl();
            ribbonControl.SelectedPage = homeRibbonPage1;

            mailingsRibbonPage1.Visible = false;
            helpRibbonPage.Visible = false;
            pdfFormDataRibbonPage1.Visible = false;
            referencesRibbonPage1.Visible = false;
            ribbonPageSkins.Visible = false;
            ribbonControl.SelectedPage = ribbonPageWorkflow;
            reviewRibbonPage1.Visible = false;
            pdfRibbonPage1.Visible = false;
            pageLayoutRibbonPage1.Visible = false;
            insertRibbonPage1.Visible = true; //turn on because of Mercuria wants page break and other options..

            //defaultLookAndFeel.LookAndFeel.SkinName = "Money Twins";
            barComboWorkflowStatus.EditValue = "NEW";
            //ComboBoxItemCollection coll = barComboWorkflowStatus.Properties.Items;
            richeditConfirm.LayoutUnit = DocumentLayoutUnit.Document;  //print layout view should match with print preview..
        }

        void InitSkinGallery()
        {
            SkinHelper.InitSkinGallery(rgbiSkins, true);
        }

        void InitializeRichEditControl()
        {

        }

        private void frmEditRichContract_Load(object sender, EventArgs e)
        {
            this.ReadUserSettings();
            //SetDisplayCmts();
        }
        public void ChangeBarCommentItemsVisibility(bool flag)
        {
            if (flag)
            {
                this.newCommentItem1.Visibility = BarItemVisibility.Always;
                this.deleteCommentsItem1.Visibility = BarItemVisibility.Always;
            }
            else
            {
                this.newCommentItem1.Visibility = BarItemVisibility.Never;
                this.deleteCommentsItem1.Visibility = BarItemVisibility.Never;
            }
        }

        private void ReadUserSettings()
        {
            try
            {
                //Now read user settings, ReadAppSettings() must be called first
                Sempra.Ops.IniFile iniFile = new Sempra.Ops.IniFile(FileNameUtils.GetUserIniFileName(settingsDir));

                this.Top = iniFile.ReadValue(FORM_NAME, "Top", 120);
                this.Left = iniFile.ReadValue(FORM_NAME, "Left", 300);
                this.Width = iniFile.ReadValue(FORM_NAME, "Width", 1100);
                this.Height = iniFile.ReadValue(FORM_NAME, "Height", 1000);
                //cedDisplayCmts.Checked = iniFile.ReadValue(FORM_NAME, "DisplayComments", true);
            }
            catch (Exception error)
            {
                XtraMessageBox.Show("An error occurred while reading the user settings file." + Environment.NewLine +
                       "Error CNF-236 in " + FORM_NAME + ".ReadUserSettings(): " + error.Message,
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
                //iniFile.WriteValue(FORM_NAME, "DisplayComments", cedDisplayCmts.Checked);
            }
            catch (Exception error)
            {
                XtraMessageBox.Show("An error occurred while saving the user settings file." + Environment.NewLine +
                       "Error CNF-237 in " + FORM_NAME + ".WriteUserSettings(): " + error.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmEditRichContract_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isSavingForm)
            {
                WriteUserSettings();
                return;
            }

            //isMarginToken = IsMarginToken();
            //if (wasMarginToken && !isMarginToken)
            //{
            //    XtraMessageBox.Show("Margin Token was deleted.",
            //       "Margin Token ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    isSavingForm = false;
            //    e.Cancel = true;
            //}
            //else if (IsNonMarginToken() && barComboWorkflowStatus.EditValue != "PREP")
            //{
            //    XtraMessageBox.Show("Token has not been resolved.",
            //       "Unresolved Token ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    isSavingForm = false;
            //    e.Cancel = true;
            //}
            //else
            //    this.WriteUserSettings();

            if (this.richeditConfirm.Document.Length < 2)
            {
                XtraMessageBox.Show("Please enter something before saving the Confirm. Otherwise use the Cancel button to exit instead.",
                   "Empty Confirm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                isSavingForm = false;
                e.Cancel = true;
            }
            else
                this.WriteUserSettings();
        }

 
        private void barButtonExportPdf_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                
                SaveFileDialog saveFileExportPdf = new SaveFileDialog();
                saveFileExportPdf.InitialDirectory = @"C:\";
                saveFileExportPdf.Title = "Export to PDF";
                //saveFileExportPdf.CheckFileExists = true;
                saveFileExportPdf.CheckPathExists = true;
                saveFileExportPdf.DefaultExt = "txt";
                saveFileExportPdf.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
                saveFileExportPdf.FilterIndex = 1;
                saveFileExportPdf.RestoreDirectory = true;

                //Israel 11/02/15 -- removed pdf support
                //if (saveFileExportPdf.ShowDialog() == DialogResult.OK)
                //{
                //    string fileName = "";
                //    fileName = saveFileExportPdf.FileName;
                //    richeditConfirm.ExportToPdf(fileName);
                //    pdfViewer.LoadDocument(fileName);
                //}
            }
            catch (Exception except)
            {
                XtraMessageBox.Show("An error occurred while exporting the document as a PDF file." + Environment.NewLine +
                       "Error CNF-238 in " + FORM_NAME + ".barButtonExportPdf_ItemClick(): " + except.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void barButtonOpenPdf_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //Israel 11/02/15 -- removed pdf support
                //xtraTabControlDocs.SelectedTabPage = xtraTabPagePDFViewer;

                OpenFileDialog openFileDialogPdf = new OpenFileDialog();
                openFileDialogPdf.Filter = "PDF files (*.pdf)|*.pdf";
                openFileDialogPdf.InitialDirectory = "C:";
                openFileDialogPdf.Title = "Open PDF File";
                if (openFileDialogPdf.ShowDialog() == DialogResult.OK)
                {
                    string fileName = "";
                    fileName = openFileDialogPdf.FileName;
                    //pdfViewer.LoadDocument(fileName);
                }
            }
            catch (Exception except)
            {
                XtraMessageBox.Show("An error occurred while opening the file." + Environment.NewLine +
                       "Error CNF-239 in " + FORM_NAME + ".barButtonOpenPdf_ItemClick(): " + except.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        public void InitForm(byte[] AContractBody, DocumentFormat ADocFormat, DataTable AClauseHeader, DataTable AClauseBody,
           string ATradingSystem, string ACurrentStatus, bool AContractOkAccess, string ASeCptySn,
           string ACptySn, bool AIsFaxNumber, string APreparerCanSendFlag)
        {
            try
            {
                bool isAllowChanges =
                   ACurrentStatus == "NEW" ||
                   ACurrentStatus == "PREP" ||
                   ACurrentStatus == "EXT_REVIEW" ||
                   ACurrentStatus == "TRADER" ||
                   ACurrentStatus == "MGR";

                if (ACurrentStatus == "MGR" && !(AContractOkAccess || APreparerCanSendFlag =="Y"))
                    isAllowChanges = false;
                bool isSelectionProtected = !isAllowChanges;

                //isPaperCreator = AIsPaperCreator;

                this.cptySn = ACptySn;
                //confirmationService.userName = GetConfirmationServiceUserName();

                //Israel 9/16/2015 - Clear previous document so new one can be displayed.
                //if (richeditConfirm.Document.Length > 1)
                //{
                    //DocumentRange docRange = richeditConfirm.Document.CreateRange(1, richeditConfirm.Document.Length);
                    //richeditConfirm.Document.Delete(docRange);
                //}

                if (AContractBody != null)
                    if (AContractBody.Length > 0)
                    {
                        richeditConfirm.Document.BeginUpdate();
                        //byte[] docBytes = System.Text.Encoding.UTF8.GetBytes(AContractBody);
                        using (MemoryStream contractStream = new MemoryStream(AContractBody))
                        {
                            richeditConfirm.LoadDocument(contractStream, ADocFormat);
                        }
                        richeditConfirm.Document.EndUpdate();
                    }

                isContractOkAccess = (AContractOkAccess || APreparerCanSendFlag == "Y");
                initialStatus = ACurrentStatus;
                SetOkAndSendVisible(initialStatus, isContractOkAccess, AIsFaxNumber);

                //clauseHeaderTable = new DataTable();
                //clauseHeaderTable = AClauseHeader.Copy();
                //clauseBodyTable = new DataTable();
                //clauseBodyTable = AClauseBody.Copy();

                //5/27/09 Israel -- Creator can't approve
                InitStatusComboBox(ATradingSystem, ACurrentStatus, AContractOkAccess, false, APreparerCanSendFlag);
                //comboStatus.Text = ACurrentStatus;
                barComboWorkflowStatus.ItemAppearance.Normal.BackColor = GetHashkeyColor(SEMPRA_RQMT + ACurrentStatus); 

                //InitPopupMenu();
                wasMarginToken = IsMarginToken();

                isSavingForm = false;
                isApproveAndSend = false;
                richeditConfirm.Options.Comments.Author = p_UserId;
                //richeditConfirm.Options.Comments.Author = Utils.GetUserNameWithoutDomain(p_UserId);
            }
            catch (Exception error)
            {
                XtraMessageBox.Show("An error occurred while setting up initial values for the form." + Environment.NewLine +
                       "Error CNF-240 in " + FORM_NAME + ".InitForm(): " + error.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //this.dbtnClauses.Enabled = true;
            }
        }

        private void SetOkAndSendVisible(string ACurrentStatus, bool AContractOkAccess,
           bool AIsFaxNumber)
        {
            barBtnWorkflowOkToSendAndSend.Enabled = AContractOkAccess && AIsFaxNumber &&
               (ACurrentStatus == "MGR" || ACurrentStatus == "OK_TO_SEND"); 
        }

        //private void InitPopupMenu()
        //{
        //    try
        //    {
        //        string categoryLabel = "";
        //        ArrayList categoryList = new ArrayList();
        //        //Load up an array list for unique, non-blank iterating
        //        foreach (DataRow row in clauseHeaderTable.Rows)
        //        {
        //            categoryLabel = row["Category"].ToString();
        //            if (categoryList.IndexOf(categoryLabel) == -1 &&
        //                categoryLabel.Trim().Length > 0)
        //                categoryList.Add(categoryLabel);
        //        }

        //        categoryList.Sort();
        //        BarManager bManager = new BarManager();
        //        string filterStr = "";
        //        //For each item in array list, select all items
        //        for (int i = 0; i < categoryList.Count; i++)
        //        {
        //            categoryLabel = categoryList[i].ToString();
        //            DevExpress.XtraBars.BarSubItem barSubItem = new DevExpress.XtraBars.BarSubItem(bManager, categoryLabel);

        //            filterStr = "Category = '" + categoryLabel + "'";
        //            foreach (DataRow row in clauseHeaderTable.Select(filterStr))
        //            {
        //                BarItem item = new BarButtonItem();
        //                item.Caption = row["ShortName"].ToString();
        //                item.Tag = (int)row["PrmntConfirmClauseId"];
        //                item.ItemClick += ClauseBtnClick;
        //                barSubItem.AddItem(item);
        //            }
        //            pmenuEditContract.AddItem(barSubItem);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(FORM_NAME + ".InitPopupMenu: " + ex.Message);
        //    }
        //}

        //private string GetClauseBodyText(int APrmntConfirmClauseId)
        //{
        //    string bodyText = "";
        //    try
        //    {
        //        string filterStr = "PrmntConfirmClauseId = '" + APrmntConfirmClauseId.ToString() + "'";
        //        foreach (DataRow row in clauseBodyTable.Select(filterStr))
        //        {
        //            bodyText += row["Body"].ToString();
        //        }
        //        return bodyText;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(FORM_NAME + ".GetClauseBodyText: " + ex.Message);
        //    }
        //}

        private void InitStatusComboBox(string ATradingSystem, string ACurrentStatusCode,
           bool AContractOkAccess, bool AIsLondonTrade, string APreparerCanSendFlag)
        {
            try
            {
                //Initialize
                //5/27/09 Israel -- Creator can't approve
                barComboWorkflowStatus.EditValue = ACurrentStatusCode;

                ComboBoxItemCollection collection = (barComboWorkflowStatus.Edit as RepositoryItemComboBox).Items;
                collection.BeginUpdate();

                //Israel 9/17/2015 -- previous editor was re-created, making clearing it unnecessary.
                collection.Clear();

                //Israel 9/18/2015
                barBtnWorkflowSave.Enabled = true;
                barComboWorkflowStatus.Enabled = true;
                APreparerCanSendFlag = (!string.IsNullOrEmpty(APreparerCanSendFlag)) ? APreparerCanSendFlag.ToUpper() : "";
                switch (ACurrentStatusCode)
                {
                    case "NEW":
                        collection.Add("PREP");
                        collection.Add("EXT_REVIEW");
                        //if (isContract)
                        //  collection.Add("TRADER");
                        collection.Add("MGR");
                        if (AContractOkAccess || (APreparerCanSendFlag.Equals("Y")))
                            collection.Add("OK_TO_SEND");
                        //Override for new
                        barComboWorkflowStatus.EditValue = "PREP";
                        break;
                    case "PREP":
                        collection.Add("EXT_REVIEW");
                        //if (isContract)
                        //    collection.Add("TRADER");
                        collection.Add("MGR");
                        if (AContractOkAccess || (APreparerCanSendFlag.Equals("Y")))
                            collection.Add("OK_TO_SEND");
                        break;
                    case "EXT_REVIEW":
                        collection.Add("PREP");
                        //if (isContract)
                        //    collection.Add("TRADER");
                        collection.Add("MGR");
                        if (AContractOkAccess || (APreparerCanSendFlag.Equals("Y")))
                            collection.Add("OK_TO_SEND");
                        break;
                    case "TRADER":
                        collection.Add("PREP");
                        collection.Add("EXT_REVIEW");
                        //if (isContract)
                        //    collection.Add("TRADER");
                        collection.Add("MGR");
                        break;
                    case "MGR":
                        collection.Add("PREP");
                        collection.Add("EXT_REVIEW");
                        //if (isContract)
                        //    collection.Add("TRADER");
                        collection.Add("MGR");
                        if (AContractOkAccess || (APreparerCanSendFlag.Equals("Y")))
                            collection.Add("OK_TO_SEND");
                        break;
                    case "OK_TO_SEND":
                        collection.Add("PREP");
                        collection.Add("EXT_REVIEW");
                        //if (isContract)
                        //    collection.Add("TRADER");
                        collection.Add("MGR");
                        if (AContractOkAccess || (APreparerCanSendFlag.Equals("Y")))
                            collection.Add("OK_TO_SEND");
                        break;
                    default:
                        barBtnWorkflowSave.Enabled = false;
                        barComboWorkflowStatus.EditValue = ACurrentStatusCode;
                        barComboWorkflowStatus.Enabled = false;
                        break;
                }
                collection.EndUpdate();

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while setting up the status display list." + Environment.NewLine +
                     "Error CNF-241 in " + FORM_NAME + ".InitStatusComboBox(): " + ex.Message);
            }
        }

        private bool IsMarginToken()
        {
            bool isMarginToken = false;
            try
            {
                //Israel -- Implement later when Affinity Contracts support is added.
                //int startPos = 0;
                //int toEnd = rtbEditContract.Text.Length - startPos;
                //RichTextBoxFinds rtbFinds = RichTextBoxFinds.None;
                //if (rtbEditContract.Find(marginToken, startPos, toEnd, rtbFinds) > -1)
                //    isMarginToken = true;

                return isMarginToken;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while evaluating the token." + Environment.NewLine +
                     "Error CNF-242 in " + FORM_NAME + ".IsMarginToken(): " + ex.Message);
            }
        }

        private bool IsNonMarginToken()
        {
            bool isNonMarginToken = false;
            try
            {
                //Israel -- Implement later when Affinity Contracts support is added.
                //int foundPos = 0;
                //int foundPos2 = 0;
                //int startPos = 0;
                //int toEnd = rtbEditContract.Text.Length - startPos;
                //RichTextBoxFinds rtbFinds = RichTextBoxFinds.None;
                //while (startPos < toEnd)
                //{
                //    foundPos = rtbEditContract.Find("[", startPos, toEnd, rtbFinds);
                //    foundPos2 = rtbEditContract.Find(marginToken, startPos, toEnd, rtbFinds);
                //    if ((foundPos > -1) &&
                //        (rtbEditContract.Find(marginToken, startPos, toEnd, rtbFinds) != foundPos))
                //    {
                //        isNonMarginToken = true;
                //        break;
                //    }

                //    if (foundPos > -1)
                //        startPos = foundPos + 1;
                //    else
                //        startPos = toEnd;
                //}

                return isNonMarginToken;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while evaluating the token." + Environment.NewLine +
                     "Error CNF-243 in " + FORM_NAME + ".IsNonMarginToken(): " + ex.Message);
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
                     "Error CNF-244 in " + FORM_NAME + ".GetHashkeyColor(): " + error.Message);
            }
            return color;
        }

        //Israel -- support for legacy Affinity Clauses dropped.
        //private void ClauseBtnClick(object sender, ItemClickEventArgs e)
        //{
        //    try
        //    {
        //        int prmntConfirmClauseId = (int)e.Item.Tag;
        //        string clauseBodyText = GetClauseBodyText(prmntConfirmClauseId);
        //        Clipboard.Clear();
        //        TextDataFormat tdFormat = TextDataFormat.Rtf;
        //        Clipboard.SetText(clauseBodyText, tdFormat);
        //        rtbEditContract.Paste();
        //    }
        //    catch (Exception ex)
        //    {
        //        XtraMessageBox.Show(FORM_NAME + ".ClauseBtnClick: " + ex.Message,
        //           "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

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
                XtraMessageBox.Show("An error occurred while setting the SendTo Address caption using the following:" + Environment.NewLine +
                    "New Address: " + ANewFaxNumber + ", Current/Previous Address: " + ALiveFaxNumber + Environment.NewLine +
                       "Error CNF-245 in " + FORM_NAME + ".SetFaxNumbers(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetEditFaxNumberCaption(string ALiveFaxNumber)
        {
            try
            {
                string transMethodDisplay = "faxed";
                if (ALiveFaxNumber.Contains("@"))
                {
                    transMethodDisplay = "emailed";
                    liveTransMethod = "EMAIL";
                }

                if (liveFaxNumber.Trim().Length > 0)
                    bbtnEditFax.Caption = "Confirm will be " + transMethodDisplay + " to: " + liveFaxNumber;
                else
                    bbtnEditFax.Caption = "No email address or fax number has been entered!";

                ribbonStatusBar.Refresh();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while setting: " + ALiveFaxNumber + " to be the current SendTo Address." + Environment.NewLine +
                     "Error CNF-246 in " + FORM_NAME + ".SetEditFaxNumberCaption(): " + ex.Message);
            }
        }

        private void barBtnWorkflowSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            isSavingForm = true;
            this.DialogResult = DialogResult.OK;
        }

        private void barBtnWorkflowCancel_ItemClick(object sender, ItemClickEventArgs e)
        {
            isSavingForm = false;
            this.DialogResult = DialogResult.Cancel;
        }

        private void barBtnWorkflowOkToSendAndSend_ItemClick(object sender, ItemClickEventArgs e)
        {
            isApproveAndSend = true;
            isSavingForm = true;
            this.DialogResult = DialogResult.OK;
        }

        private void bbtnEditFax_ItemClick(object sender, ItemClickEventArgs e)
        {
            assignFaxNoForm.SetFaxNumbers(newFaxNumber, liveFaxNumber);            
            if (assignFaxNoForm.ShowDialog(this) == DialogResult.OK)
            {
                //throw new Exception("Please enter a valid non-Production .");
                    //liveTransMethod = "FAX";
                //if (assignFaxNoForm.radgrpTransMethod.SelectedIndex == 1)
                //liveTransMethod = "TELEX";
                liveFaxNumber = assignFaxNoForm.teditFaxTelexNumber.Text;
                liveTransMethod = liveFaxNumber.Contains("@") ? liveTransMethod = "EMAIL" : liveTransMethod = "FAX";
                                
                SetEditFaxNumberCaption(liveFaxNumber);

                SetOkAndSendVisible(initialStatus, isContractOkAccess, liveFaxNumber.Trim().Length > 1);
            }
        }

    }
}