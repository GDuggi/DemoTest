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
   public partial class frmAudit : DevExpress.XtraEditors.XtraForm
   {
      private const string FORM_NAME = "frmAudit";
      private const string FORM_ERROR_CAPTION = "Audit Form Error";
      private const string AUDIT_GRID_SETTINGS = "AuditGrid.xml";
      public string settingsDir;
      public string saveToExcelDirectory;

      public frmAudit()
      {
         InitializeComponent();
      }

      private void frmAudit_Load(object sender, EventArgs e)
      {
         ReadUserSettings();
      }

      private void frmAudit_FormClosing(object sender, FormClosingEventArgs e)
      {
         WriteUserSettings();
      }

      private void ReadUserSettings()
      {
         try
         {
             if (System.IO.File.Exists(settingsDir + "\\" + AUDIT_GRID_SETTINGS))
                 gridViewAudit.RestoreLayoutFromXml(settingsDir + "\\" + AUDIT_GRID_SETTINGS);

            //Now read user settings, ReadAppSettings() must be called first
            Sempra.Ops.IniFile iniFile = new Sempra.Ops.IniFile(FileNameUtils.GetUserIniFileName(settingsDir));

            this.StartPosition = FormStartPosition.Manual;
            this.Top = iniFile.ReadValue(FORM_NAME, "Top", 200);
            this.Left = iniFile.ReadValue(FORM_NAME, "Left", 300);
            this.Width = iniFile.ReadValue(FORM_NAME, "Width", 750);
            this.Height = iniFile.ReadValue(FORM_NAME, "Height", 450);
         }
         catch (Exception error)
         {
             XtraMessageBox.Show("An error occurred while reading the user settings file." + Environment.NewLine +
                    "Error CNF-205 in " + FORM_NAME + ".ReadUserSettings(): " + error.Message,
                  FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void WriteUserSettings()
      {
         try
         {
             gridViewAudit.SaveLayoutToXml(settingsDir + "\\" + AUDIT_GRID_SETTINGS);

            Sempra.Ops.IniFile iniFile = new Sempra.Ops.IniFile(FileNameUtils.GetUserIniFileName(settingsDir));
            iniFile.WriteValue(FORM_NAME, "Top", this.Top);
            iniFile.WriteValue(FORM_NAME, "Left", this.Left);
            iniFile.WriteValue(FORM_NAME, "Width", this.Width);
            iniFile.WriteValue(FORM_NAME, "Height", this.Height);
         }
         catch (Exception error)
         {
             XtraMessageBox.Show("An error occurred while saving the user settings file." + Environment.NewLine +
                    "Error CNF-206 in " + FORM_NAME + ".WriteUserSettings(): " + error.Message,
                  FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

      private void btnClose_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      private void btnExcel_Click(object sender, EventArgs e)
      {
         try
         {
            saveFileDialog.Title = "Save Grid to Excel";
            saveFileDialog.InitialDirectory = saveToExcelDirectory;
            saveFileDialog.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
               saveToExcelDirectory = System.IO.Path.GetDirectoryName(saveFileDialog.FileName);
               DevExpress.XtraPrinting.XlsExportOptions xlOptions =
                   new DevExpress.XtraPrinting.XlsExportOptions(DevExpress.XtraPrinting.TextExportMode.Value, true);
               gridViewAudit.ExportToXls(saveFileDialog.FileName, xlOptions);
            }
         }
         catch (Exception ex)
         {
            XtraMessageBox.Show("An error occurred while saving the grid to MS Excel format." + Environment.NewLine +
                   "Error CNF-207 in " + FORM_NAME + ".btnExcel_Click(): " + ex.Message,
                 FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }

   }
}