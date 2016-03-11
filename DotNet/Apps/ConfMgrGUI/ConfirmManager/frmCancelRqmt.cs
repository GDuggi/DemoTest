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
   public partial class frmCancelRqmt : DevExpress.XtraEditors.XtraForm
   {
      private const string FORM_NAME = "frmCancelRqmt";
      private const string FORM_ERROR_CAPTION = "Cancel Requirement Form Error";
      public string settingsDir;

      public frmCancelRqmt()
      {
         InitializeComponent();
      }

      private void frmCancelRqmt_Load(object sender, EventArgs e)
      {
         ReadUserSettings();
      }      
      
      private void frmCancelRqmt_FormClosing(object sender, FormClosingEventArgs e)
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
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while reading the user settings file." + Environment.NewLine +
                   "Error CNF-208 in " + FORM_NAME + ".ReadUserSettings(): " + error.Message,
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
                   "Error CNF-209 in " + FORM_NAME + ".WriteUserSettings(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void frmCancelRqmt_Activated(object sender, EventArgs e)
      {
         tedComment.Focus();
      }

   }
}