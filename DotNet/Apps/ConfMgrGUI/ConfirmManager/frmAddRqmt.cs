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
   public partial class frmAddRqmt : DevExpress.XtraEditors.XtraForm
   {
      private const string FORM_NAME = "frmAddRqmt";
      private const string FORM_ERROR_CAPTION = "Add Requirement Form Error";
      public string settingsDir;

      public frmAddRqmt()
      {
         InitializeComponent();
      }

      private void frmAddRqmt_Load(object sender, EventArgs e)
      {
         ReadUserSettings();
      }

      private void frmAddRqmt_FormClosing(object sender, FormClosingEventArgs e)
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
                    "Error CNF-201 in " + FORM_NAME + ".ReadUserSettings(): " + error.Message,
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
                   "Error CNF-202 in " + FORM_NAME + ".WriteUserSettings(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      public void InitForm()
      {
         try
         {
            for (int i = 0; i < Properties.Settings.Default.AddRqmtNoConfReason.Count; i++)
            {
               comboNoConfReason.Properties.Items.Add(
                  Properties.Settings.Default.AddRqmtNoConfReason[i]);
            }
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while setting up initial values for the form." + Environment.NewLine +
                   "Error CNF-203 in " + FORM_NAME + ".InitForm(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void lookupRqmt_EditValueChanged(object sender, EventArgs e)
      {
         try
         {
            if (lookupRqmt.EditValue.ToString() == "NOCNF")
            {
               lblNoConfReason.Visible = true;
               comboNoConfReason.Visible = true;
            }
            else
            {
               lblNoConfReason.Visible = false;
               comboNoConfReason.Visible = false;
            }
         }
         catch (Exception error)
         {
            XtraMessageBox.Show("An error occurred while setting form values based on current selection." + Environment.NewLine +
                   "Error CNF-204 in " + FORM_NAME + ".lookupRqmt_EditValueChanged(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void comboNoConfReason_InvalidValue(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
      {
         //e.ErrorText = e.Value + " is not a valid value.";
      }

      private void comboNoConfReason_Validating(object sender, CancelEventArgs e)
      {
         if (comboNoConfReason.Properties.Items.IndexOf(comboNoConfReason.EditValue.ToString()) == -1)
            e.Cancel = true;
      }

   }
}