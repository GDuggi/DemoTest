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
    public partial class frmXmitResultStatusLog : DevExpress.XtraEditors.XtraForm
    {
        private const string FORM_NAME = "frmFaxStatusLog";
        private const string FORM_ERROR_CAPTION = "Add Requirement Form Error";
        private string FAX_STATUS_LOG_GRID_SETTINGS = "FaxStatusLog.xml";
        public string settingsDir;
        public string saveToExcelDirectory;

        public frmXmitResultStatusLog()
        {
            // Required for Windows Form Designer support
            InitializeComponent();

            // TODO: Add any constructor code after InitializeComponent call
        }

        private void frmXmitResultStatusLog_Load(object sender, EventArgs e)
        {
            ReadUserSettings();
        }

        private void frmXmitResultStatusLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            WriteUserSettings();
        }

        private void ReadUserSettings()
        {
            try
            {
                if (System.IO.File.Exists(settingsDir + "\\" + FAX_STATUS_LOG_GRID_SETTINGS))
                    gridViewXmitResult.RestoreLayoutFromXml(settingsDir + "\\" + FAX_STATUS_LOG_GRID_SETTINGS);

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
                       "Error CNF-327 in " + FORM_NAME + ".ReadUserSettings(): " + error.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WriteUserSettings()
        {
            try
            {
                gridViewXmitResult.SaveLayoutToXml(settingsDir + "\\" + FAX_STATUS_LOG_GRID_SETTINGS);

                Sempra.Ops.IniFile iniFile = new Sempra.Ops.IniFile(FileNameUtils.GetUserIniFileName(settingsDir));
                iniFile.WriteValue(FORM_NAME, "Top", this.Top);
                iniFile.WriteValue(FORM_NAME, "Left", this.Left);
                iniFile.WriteValue(FORM_NAME, "Width", this.Width);
                iniFile.WriteValue(FORM_NAME, "Height", this.Height);
            }
            catch (Exception error)
            {
                XtraMessageBox.Show("An error occurred while saving the user settings file." + Environment.NewLine +
                       "Error CNF-328 in " + FORM_NAME + ".WriteUserSettings(): " + error.Message,
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
                    //      new DevExpress.XtraPrinting.XlsExportOptions(true, true);
                    gridViewXmitResult.ExportToXls(saveFileDialog.FileName, xlOptions);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while exporting the grid data to an MS Excel file." + Environment.NewLine +
                       "Error CNF-329 in " + FORM_NAME + ".btnExcel_Click(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridViewXmitResult_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column == null || e.RowHandle < 0) return;
            try
            {
                if (e.Column.Name == "colXmitMethodInd")
                {
                    switch (e.DisplayText)
                    {
                        case "F": e.DisplayText = "Fax"; break;
                        case "T": e.DisplayText = "Telex"; break;
                        case "E": e.DisplayText = "EMail"; break;
                    }
                }
                else if (e.Column.Name == "colXmitStatusInd")
                {
                    switch (e.DisplayText)
                    {
                        case "Q": e.DisplayText = "Queued"; break;
                        case "S": e.DisplayText = "Success"; break;
                        case "F": e.DisplayText = "Failed"; break;
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while setting the display text value for Trans Method and Status." + Environment.NewLine +
                       "Error CNF-330 in " + FORM_NAME + ".gridViewXmitResult_CustomDrawCell(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmXmitResultStatusLog_Activated(object sender, EventArgs e)
        {
            btnClose.Focus();
        }
    }
}