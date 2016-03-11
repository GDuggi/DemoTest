using System;
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
   public partial class frmAddMultiRqmt : DevExpress.XtraEditors.XtraForm
   {
      private const string FORM_NAME = "frmAddMultiRqmt";
      private const string FORM_ERROR_CAPTION = "Add Multiple Requirements Form Error";
      public string settingsDir;
      public DataTable rqmtStatusView;
      private DataTable rqmtStatusViewSempra;
      private DataTable rqmtStatusViewCpty;
      private DataTable rqmtStatusViewBroker;
      private DataTable rqmtStatusViewVerbal;
      //private bool isSecondCheckCreateCxl = false;
      private int EDIT_FIELD_X = 285;
      private int NOT_ACTIVE_X = 500;

      enum RqmtType
      {
         Sempra = 0, Cpty, Broker, Phone
      };

      public frmAddMultiRqmt()
      {
         InitializeComponent();
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
                   "Error CNF-198 in " + FORM_NAME + ".ReadUserSettings(): " + error.Message,
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
                    "Error CNF-199 in " + FORM_NAME + ".WriteUserSettings(): " + error.Message,
                  FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void frmAddMultiRqmt_Load(object sender, EventArgs e)
      {
         ReadUserSettings();
      }

      private void frmAddMultiRqmt_FormClosing(object sender, FormClosingEventArgs e)
      {
         WriteUserSettings();
      }

      public void InitForm()
      {
         try
         {
            for (int i = 0; i < Properties.Settings.Default.DisputedReasons.Count; i++)
            {
               comboSempraDispReason.Properties.Items.Add(Properties.Settings.Default.DisputedReasons[i]);
               comboCptyDispReason.Properties.Items.Add(Properties.Settings.Default.DisputedReasons[i]);
               comboBrokerDispReason.Properties.Items.Add(Properties.Settings.Default.DisputedReasons[i]);
               comboVerbalDispReason.Properties.Items.Add(Properties.Settings.Default.DisputedReasons[i]);
            }

            SetupStatusFieldData(ref gluedSempraStatus, ref rqmtStatusViewSempra, "XQCSP");
            SetupStatusFieldData(ref gluedCptyStatus, ref rqmtStatusViewCpty, "XQCCP");
            SetupStatusFieldData(ref gluedBrokerStatus, ref rqmtStatusViewBroker, "XQBBP");
            SetupStatusFieldData(ref gluedVerbalStatus, ref rqmtStatusViewVerbal, "VBCP");

            rqmtStatusView.PrimaryKey = new DataColumn[] 
                  { rqmtStatusView.Columns["RqmtCode"], 
                    rqmtStatusView.Columns["StatusCode"] };

            //userName = Sempra.Ops.Utils.GetUserName();
         }
         catch (Exception error)
         {
             XtraMessageBox.Show("An error occurred while setting up initial values for the form." + Environment.NewLine +
                    "Error CNF-200 in " + FORM_NAME + ".InitForm(): " + error.Message,
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
         if (AFilterValue == "XQCSP")
         {
            ALookupDataTable.DefaultView.RowFilter +=
               " and [ColorCode] <> 'SandyBrown' and [ColorCode] <> 'Gold'";
         }
         else if (AFilterValue == "XQCCP")
         {
            ALookupDataTable.DefaultView.RowFilter += " and [StatusCode] <> 'APPR'";
         }
         else if (AFilterValue == "XQBBP")
         {
            ALookupDataTable.DefaultView.RowFilter += " and [StatusCode] <> 'APPR'";
         }

         AGridLookupEdit.Properties.DataSource = ALookupDataTable.DefaultView;
         AGridLookupEdit.Properties.DisplayMember = "Descr";
         AGridLookupEdit.Properties.ValueMember = "StatusCode";
      }

      private void Rqmt_CheckedChanged(object sender, EventArgs e)
      {
         string rqmt = (sender as CheckEdit).Name.ToString();
         bool isEnabled = (sender as CheckEdit).Checked;

         switch (rqmt)
         {
            case "ckedSempra": 
               gluedSempraStatus.Enabled = isEnabled; 
               memoexedSempra.Enabled = isEnabled; 
               break;
            case "ckedCpty": gluedCptyStatus.Enabled = isEnabled; memoexedCpty.Enabled = isEnabled; break;
            case "ckedBroker": gluedBrokerStatus.Enabled = isEnabled; memoexedBroker.Enabled = isEnabled; break;
            case "ckedVerbal": gluedVerbalStatus.Enabled = isEnabled; memoexedVerbal.Enabled = isEnabled; break;               
         }


      }

      private void SetEditFields(RqmtType ARqmtType, bool ADisputed)
      {
         switch (ARqmtType)
         {
            case RqmtType.Sempra:
               if (ADisputed)
               {
                  comboSempraDispReason.Location = new System.Drawing.Point(EDIT_FIELD_X, comboSempraDispReason.Location.Y);
                  comboSempraDispReason.TabStop = true;
                  memoexedSempra.Location = new System.Drawing.Point(NOT_ACTIVE_X, memoexedSempra.Location.Y);
                  memoexedSempra.TabStop = false;
               }
               else
               {
                  comboSempraDispReason.Location = new System.Drawing.Point(NOT_ACTIVE_X, comboSempraDispReason.Location.Y);
                  comboSempraDispReason.TabStop = false;                  
                  memoexedSempra.Location = new System.Drawing.Point(EDIT_FIELD_X, memoexedSempra.Location.Y);
                  memoexedSempra.TabStop = true;
               }
               break;
         }
      }

      private void SempraStatusView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
      {
         GridView view = sender as GridView;
         if (view.FocusedRowHandle != e.RowHandle)
         {
            string colorCode = view.GetRowCellDisplayText(e.RowHandle, view.Columns["ColorCode"]);
            e.Appearance.BackColor = Color.FromName(colorCode);
         }
      }

      private void Status_EditValueChanged(object sender, EventArgs e)
      {
         GridLookUpEdit editor = sender as GridLookUpEdit;

         string colorCode = "";
         if (editor.Properties.View.FocusedRowHandle > -1)
         {
            colorCode = editor.Properties.View.GetFocusedRowCellValue("ColorCode").ToString();
            editor.Properties.Appearance.BackColor = Color.FromName(colorCode);
         }

         bool isDisputed = editor.EditValue.ToString() == "DISP";
         RqmtType rqmtType = (RqmtType)Enum.Parse(typeof(RqmtType), editor.Tag.ToString(), true);
         SetEditFields(rqmtType, isDisputed);
      }

   }
}