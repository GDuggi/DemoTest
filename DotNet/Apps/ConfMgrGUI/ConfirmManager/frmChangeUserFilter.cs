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
   public partial class frmChangeUserFilter : DevExpress.XtraEditors.XtraForm
   {
      private const string FORM_NAME = "frmChangeUserFilter";
      private const string FORM_ERROR_CAPTION = "Change Filter Form Error";
      public string settingsDir;

      public frmChangeUserFilter()
      {
         InitializeComponent();
      }

      public void InitForm()
      {
         tedFilterDescr.Text = "";
         btnChangeUserFilterOk.Enabled = false;
      }

      private void frmAddCustomFilter_Load(object sender, EventArgs e)
      {
         ReadUserSettings();
      }

      private void frmAddCustomFilter_FormClosing(object sender, FormClosingEventArgs e)
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
                   "Error CNF-210 in " + FORM_NAME + ".ReadUserSettings(): " + error.Message,
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
                    "Error CNF-211 in " + FORM_NAME + ".WriteUserSettings(): " + error.Message,
                  FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void frmAddUserFilter_Activated(object sender, EventArgs e)
      {
         tedFilterDescr.Focus();
      }

      private void tedFilterDescr_EditValueChanged(object sender, EventArgs e)
      {
         btnChangeUserFilterOk.Enabled = tedFilterDescr.Text.Length > 0;
      }
   }
}