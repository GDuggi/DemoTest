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

namespace ConfirmManager
{
   public partial class frmUserPrefs : DevExpress.XtraEditors.XtraForm
   {
      private const string FORM_NAME = "frmUserPrefs";
      private const string FORM_ERROR_CAPTION = "Preferences Form Error";
      public string settingsDir;
      private ArrayList prevSeCptySnChkList = new ArrayList();
      private ArrayList prevCdtyChkList = new ArrayList();
      private bool prevAutoDispDealsheet;
      private bool prevSaveToNewExcelFormat;


      public frmUserPrefs()
      {
         InitializeComponent();
      }

      private void frmUserPrefs_Load(object sender, EventArgs e)
      {
         ReadUserSettings();
      }

      private void frmUserPrefs_FormClosing(object sender, FormClosingEventArgs e)
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
                   "Error CNF-321 in " + FORM_NAME + ".ReadUserSettings(): " + error.Message,
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
                   "Error CNF-322 in " + FORM_NAME + ".WriteUserSettings(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void CheckUncheckListboxItems(CheckedListBoxControl AListBox, bool ACheckState)
      {
         try
         {

            for (int i = 0; i < AListBox.ItemCount; i++)
            {
               DataRowView row = AListBox.GetItem(i) as DataRowView;
               AListBox.SetItemChecked(i, ACheckState);
            }
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while setting selected preferences to checked/unchecked." + Environment.NewLine +
                   "Error CNF-323 in " + FORM_NAME + ".CheckUncheckListboxItems(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }


      private void CheckUncheckAll_Click(object sender, EventArgs e)
      {
         try
         {
            SimpleButton button = (SimpleButton)sender;
            bool isChecked = false;
            CheckedListBoxControl listBox = null;

            switch (button.Name)
            {
               case "btnSeCptyAll":
                  {
                     isChecked = true;
                     listBox = cklbxSeCptySn;
                     break;
                  }
               case "btnSeCptyUncheckAll":
                  {
                     isChecked = false;
                     listBox = cklbxSeCptySn;
                     break;
                  }
               case "btnCdtyGrpAll":
                  {
                     isChecked = true;
                     listBox = cklbxCdtyGrp;
                     break;
                  }
               case "btnCdtyGrpUncheckAll":
                  {
                     isChecked = false;
                     listBox = cklbxCdtyGrp;
                     break;
                  }
            }
            CheckUncheckListboxItems(listBox, isChecked);
         }
         catch (Exception except)
         {
            XtraMessageBox.Show("An error occurred while preparing to set the selcted preferences to checked/unchecked." + Environment.NewLine +
                   "Error CNF-324 in " + FORM_NAME + ".CheckUncheckAll_Click(): " + except.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void cklbxSeCptySn_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
      {
         UpdateSeCptySnDisplay();   
      }

      private void cklbxCdty_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
      {
         UpdateCdtyDisplay();
      }

      public void UpdateSeCptySnDisplay()
      {
          if (cklbxSeCptySn.CheckedItems.Count > 1)
          {
              lblSeCptySn.Text = "Our Companies:  " + cklbxSeCptySn.CheckedItems.Count.ToString();
          }
          else lblSeCptySn.Text = "Our Company:  " + cklbxSeCptySn.CheckedItems.Count.ToString();
      }

      public void UpdateCdtyDisplay()
      {
         lblCdty.Text = "Commodity Group:  " + cklbxCdtyGrp.CheckedItems.Count.ToString();
      }

      public void BackupPrefData()
      {
         try
         {
            prevSeCptySnChkList.Clear();
            for (int i = 0; i < cklbxSeCptySn.Items.Count; i++)
            {
               if (cklbxSeCptySn.Items[i].CheckState == CheckState.Checked)
                  prevSeCptySnChkList.Add(cklbxSeCptySn.Items[i].Value.ToString());
            }

            prevCdtyChkList.Clear();
            for (int i = 0; i < cklbxCdtyGrp.Items.Count; i++)
            {
               if (cklbxCdtyGrp.Items[i].CheckState == CheckState.Checked)
                  prevCdtyChkList.Add(cklbxCdtyGrp.Items[i].Value.ToString());
            }

            prevAutoDispDealsheet = cedAutoDispDealsheet.Checked;
            //prevSaveToNewExcelFormat = cedSaveToNewExcelFormat .Checked;
         }
         catch (Exception except)
         {
            XtraMessageBox.Show("An error occurred while backing up preference data prior to displaying the data entry form." + Environment.NewLine +
                   "Error CNF-325 in " + FORM_NAME + ".BackupPrefData(): " + except.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void btnCancelCreateCancel_Click(object sender, EventArgs e)
      {
         try
         {
            //Se Cpty Sn
            string chklistValue = "";
            for (int i=0; i < cklbxSeCptySn.Items.Count; i++)
               cklbxSeCptySn.SetItemChecked(i, false );

            for (int i = 0; i < prevSeCptySnChkList.Count; i++)
            {
               chklistValue = prevSeCptySnChkList[i].ToString();
               int index = cklbxSeCptySn.FindStringExact(chklistValue);
               cklbxSeCptySn.SetItemChecked(index, true);
            }

            //Commodity
            chklistValue = "";
            for (int i=0; i < cklbxCdtyGrp.Items.Count; i++)
               cklbxCdtyGrp.SetItemChecked(i, false );

            for (int i = 0; i < prevCdtyChkList.Count; i++)
            {
               chklistValue = prevCdtyChkList[i].ToString();
               int index = cklbxCdtyGrp.FindStringExact(chklistValue);
               cklbxCdtyGrp.SetItemChecked(index, true);
            }

            cedAutoDispDealsheet.Checked = prevAutoDispDealsheet;
            //cedSaveToNewExcelFormat.Checked = prevSaveToNewExcelFormat;
         }
         catch (Exception except)
         {
            XtraMessageBox.Show("An error occurred while resetting preference data to backed up values." + Environment.NewLine +
                   "Error CNF-326 in " + FORM_NAME + ".btnCancelCreateCancel_Click(): " + except.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }
   }
}