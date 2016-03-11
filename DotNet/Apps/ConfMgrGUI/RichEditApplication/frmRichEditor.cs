using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraBars.Helpers;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using System.IO;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;


namespace RichEditApplication
{
    public partial class frmRichEditor : RibbonForm
    {
        private string FORM_NAME = "frmRichEditor";
        public string settingsDir;

        public frmRichEditor()
        {
            InitializeComponent();
            InitSkinGallery();
            InitializeRichEditControl();
            ribbonControl.SelectedPage = homeRibbonPage1;
            mailingsRibbonPage1.Visible = false;
            helpRibbonPage.Visible = false;
            pdfFormDataRibbonPage1.Visible = false;
            referencesRibbonPage1.Visible = false;
            ribbonPageSkins.Visible = false;

            defaultLookAndFeel.LookAndFeel.SkinName = "Money Twins";
            barComboWorkflowStatus.EditValue = "NEW";
            //ComboBoxItemCollection coll = barComboWorkflowStatus.Properties.Items;

            
        }

        void InitSkinGallery()
        {
            SkinHelper.InitSkinGallery(rgbiSkins, true);
        }

        void InitializeRichEditControl()
        {

        }

        private void ReadUserSettings()
        {
            try
            {
                //Now read user settings, ReadAppSettings() must be called first
                //Sempra.Ops.IniFile iniFile = new Sempra.Ops.IniFile(Sempra.Ops.Utils.GetUserIniFileName(settingsDir));

                //this.Top = iniFile.ReadValue(FORM_NAME, "Top", 200);
                //this.Left = iniFile.ReadValue(FORM_NAME, "Left", 300);
                //this.Width = iniFile.ReadValue(FORM_NAME, "Width", 750);
                //this.Height = iniFile.ReadValue(FORM_NAME, "Height", 450);
            }
            catch (Exception error)
            {
                XtraMessageBox.Show(FORM_NAME + ".ReadUserSettings: " + error.Message,
                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WriteUserSettings()
        {
            try
            {
                //Sempra.Ops.IniFile iniFile = new Sempra.Ops.IniFile(Sempra.Ops.Utils.GetUserIniFileName(settingsDir));
                //iniFile.WriteValue(FORM_NAME, "Top", this.Top);
                //iniFile.WriteValue(FORM_NAME, "Left", this.Left);
                //iniFile.WriteValue(FORM_NAME, "Width", this.Width);
                //iniFile.WriteValue(FORM_NAME, "Height", this.Height);
                //iniFile.WriteValue(FORM_NAME, "DisplayComments", cedDisplayCmts.Checked);
            }
            catch (Exception error)
            {
                XtraMessageBox.Show(FORM_NAME + ".WriteUserSettings: " + error.Message,
                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void barButtonExportPdf_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                
                SaveFileDialog saveFileExportPdf = new SaveFileDialog();
                saveFileExportPdf.InitialDirectory = @"C:\";
                saveFileExportPdf.Title = "Export to PDF";
                //saveFileExportPdf.CheckFileExists = true;
                saveFileExportPdf.CheckPathExists = true;
                saveFileExportPdf.DefaultExt = "txt";
                saveFileExportPdf.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
                saveFileExportPdf.FilterIndex = 1;
                saveFileExportPdf.RestoreDirectory = true;

                if (saveFileExportPdf.ShowDialog() == DialogResult.OK)
                {
                    string fileName = "";
                    fileName = saveFileExportPdf.FileName;
                    richEditControl.ExportToPdf(fileName);
                    pdfViewer.LoadDocument(fileName);
                }
            }
            catch (Exception except)
            {
                MessageBox.Show("ExportPdf: " + except.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void barButtonOpenPdf_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                xtraTabControlDocs.SelectedTabPage = xtraTabPagePDFViewer;

                OpenFileDialog openFileDialogPdf = new OpenFileDialog();
                openFileDialogPdf.Filter = "PDF files (*.pdf)|*.pdf";
                openFileDialogPdf.InitialDirectory = "C:";
                openFileDialogPdf.Title = "Open PDF File";
                if (openFileDialogPdf.ShowDialog() == DialogResult.OK)
                {
                    string fileName = "";
                    fileName = openFileDialogPdf.FileName;
                    pdfViewer.LoadDocument(fileName);
                }
            }
            catch (Exception except)
            {
                MessageBox.Show("OpenPdf: " + except.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


    }
}