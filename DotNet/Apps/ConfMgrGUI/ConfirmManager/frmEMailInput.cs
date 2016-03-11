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
   public partial class frmEMailInput : DevExpress.XtraEditors.XtraForm
   {
      private const string FORM_NAME = "frmEMailInput";
      private const string FORM_ERROR_CAPTION = "EMail Entry Form Error";
      public string settingsDir;

      public frmEMailInput()
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
                    "Error CNF-289 in " + FORM_NAME + ".ReadUserSettings(): " + error.Message,
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
                   "Error CNF-290 in " + FORM_NAME + ".WriteUserSettings(): " + error.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void frmEMailInput_Load(object sender, EventArgs e)
      {
         this.ReadUserSettings();
      }

      private void frmEMailInput_FormClosing(object sender, FormClosingEventArgs e)
      {
          if (!OkToClose())
          {
              XtraMessageBox.Show("No Fax numbers are allowed. Only email addresses.",
                "No Fax Numbers", MessageBoxButtons.OK, MessageBoxIcon.Stop);
              tedToAddress.Focus();
              e.Cancel = true;
          }
          else
            this.WriteUserSettings();
      }

      public void InitForm(string ADefaultFromAddress)
      {
         tedFromAddress.Text = ADefaultFromAddress;
         tedToAddress.Text = "";
         tedSubject.Text = "";
         memoBody.Text = "";
         cedSendAsRTF.Checked = false;
      }

      private void frmEMailInput_Activated(object sender, EventArgs e)
      {
         tedToAddress.Focus();
      }

      private void tedToAddress_EditValueChanged(object sender, EventArgs e)
      {
         btnEMailInputOk.Enabled = tedToAddress.Text.IndexOf("@") > 1 &&
                                   tedToAddress.Text.IndexOf("@") < tedToAddress.Text.Trim().Length - 1;
      }

      private bool OkToClose()
      {
        bool okToClose = true;

        string[] separator = new string[1];
        separator[0] = ";";
        string[] faxTelexNumbers = tedToAddress.Text.Split(separator, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < faxTelexNumbers.Length; i++)
        {
            if (!faxTelexNumbers[i].Contains("@"))
                okToClose = false;
        }

        return okToClose;
      }

       private void btnEMailInputCancel_Click(object sender, EventArgs e)
       {
           tedToAddress.Text = "";
       }
   }
}