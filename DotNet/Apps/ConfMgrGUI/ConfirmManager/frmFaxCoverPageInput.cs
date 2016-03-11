using System;
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
   public partial class frmFaxCoverPageInput : DevExpress.XtraEditors.XtraForm
   {
      private const string FORM_NAME = "frmFaxCoverPageInput";
      private const string FORM_ERROR_CAPTION = "Fax CoverPage Inout Form Error";
      public string settingsDir;
      public frmFaxCoverPageInput()
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
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while reading the user settings file." + Environment.NewLine +
                   "Error CNF-291 in " + FORM_NAME + ".ReadUserSettings(): " + error.Message,
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
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while saving the user settings file." + Environment.NewLine +
                   "Error CNF-292 in " + FORM_NAME + ".WriteUserSettings(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void frmFaxCoverPageInput_Load(object sender, EventArgs e)
      {
         this.ReadUserSettings();
      }

      private void frmFaxCoverPageInput_FormClosing(object sender, FormClosingEventArgs e)
      {
         this.WriteUserSettings();
      }

      public void InitForm()
      {
         tedTitle.Text = "";
         memoMessage.Text = "";
         cedSendAsRTF.Checked = false;
      }

      private void frmFaxCoverPageInput_Activated(object sender, EventArgs e)
      {
         tedTitle.Focus();
      }

      private void memoMessage_EditValueChanged(object sender, EventArgs e)
      {
         btnFaxlInputOk.Enabled = memoMessage.Text.Length > 0;
      }
   }
}