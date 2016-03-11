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
   public partial class frmSetECMatched : DevExpress.XtraEditors.XtraForm
   {
      private string FORM_NAME = "frmSetMatchedEC";
      private const string FORM_ERROR_CAPTION = "Update EConfirm Matched Form Error";
      public string settingsDir;

      public frmSetECMatched()
      {
         InitializeComponent();
      }

      private void frmUpdMatchedEC_Load(object sender, EventArgs e)
      {
         ReadUserSettings();
      }

      private void frmUpdMatchedEC_FormClosing(object sender, FormClosingEventArgs e)
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
                   "Error CNF-305 in " + FORM_NAME + ".ReadUserSettings(): " + error.Message,
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
                   "Error CNF-306 in " + FORM_NAME + ".WriteUserSettings(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      public void InitForm()
      {
         try
         {
            tedCptyRefId.EditValue = "";
            dedUpdStatusDate.EditValue = "";
            btnUpdMatchedECOk.Enabled = false;
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while setting up initial values for the form." + Environment.NewLine +
                   "Error CNF-307 in " + FORM_NAME + ".InitForm(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void dateEdit1_Properties_CustomDisplayText(object sender, 
         DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
      {
         //if (dedUpdStatusDate.DateTime == DateTime.MinValue)
           // e.DisplayText = "";
      }

      private void DataEntry_EditValueChanged(object sender, EventArgs e)
      {
         btnUpdMatchedECOk.Enabled = (tedCptyRefId.Text.Length > 0 && 
            dedUpdStatusDate.DateTime > DateTime.MinValue);
      }

      private void dedUpdStatusDate_Enter(object sender, EventArgs e)
      {
      }
   }
}