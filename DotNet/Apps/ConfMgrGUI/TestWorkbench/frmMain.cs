using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Transactions;
using DevExpress.XtraEditors;

namespace TestWorkbench
{
    public partial class frmMain : Form
    {
        private const string PRODUCT_COMPANY = "Amphora";
        private const string PRODUCT_BRAND = "Affinity";
        private const string PRODUCT_GROUP = "Confirms";
        private const string PRODUCT_NAME = "ConfirmManager";
        private const string PRODUCT_SETTINGS = "Settings";
        private const string PRODUCT_TEMP = "Temp";
        private string appTempDir;

        public frmMain()
        {
            InitializeComponent();
            string PRODUCT_COMPANY = "Amphora";

            string roamingFolderLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                PRODUCT_COMPANY, PRODUCT_BRAND, PRODUCT_GROUP);
            appTempDir = Path.Combine(roamingFolderLocation, PRODUCT_NAME, PRODUCT_TEMP);
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                int photoId = int.Parse(this.txtSavePhotoId.Text);
                string desc = this.txtDescription.Text;
                string filename = this.txtFilename.Text;
                string fileext = Path.GetExtension(filename).ToUpper().Replace(".", "");
                txtFileExt.Text = fileext;
                ImageData.InsertDocImage(photoId, desc, filename, fileext);
            }
            catch (Exception excep)
            {
                MessageBox.Show("Error on Save: " + excep.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                int docImageId = int.Parse(this.txtLoadPhotoId.Text);
                string desc;
                string fileext;
                //Image docImage = ImageData.SelectDocImage(photoId, out desc, out fileext);
                ImageData.SelectDocImage(docImageId, out desc, out fileext);
                this.lblDescription.Text = desc;
                //this.picImage.Image = photo;
                fileext = fileext.ToUpper();

                if (fileext.Equals("TIF"))
                {
                    tabControlImage.SelectedIndex = 0;
                    Image docImage = Image.FromFile(ImageData.TEMP_FILENAME + "TIF");
                    dxPictureEdit.Image = docImage;
                }
                else if (fileext.Equals("PDF"))
                {
                    tabControlImage.SelectedIndex = 1;
                    string pdfFileName = ImageData.TEMP_FILENAME + "PDF";
                    pdfViewer.DocumentFilePath = pdfFileName;
                }

               
            }
            catch (Exception excep)
            {
                MessageBox.Show("Error on Load: " + excep.Message);
            }
        }

        private void openFileDialog_Click(object sender, EventArgs e)
        {
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "All Files (*.*)|*.*|Image Files (.tif)|*.tif|Pdf Files (.pdf)|*.pdf";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            // Process input if the user clicked OK.
            {
                txtFilename.Text = openFileDialog1.FileName;
                txtFileExt.Text = Path.GetExtension(txtFilename.Text).ToUpper().Replace(".", "");
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string ticket = txtFilename.Text;
            string sql = "";
            if (ticket.Contains("*"))
            {
                int end = ticket.IndexOf("*");
                sql = " where cpty_trade_id like '" + ticket.Substring(0, end).ToUpper() + "%' ";
            }


            //try
            //{
            //    throw new Exception("An error occurred while setting up the category's display list." + Environment.NewLine +
            //        "Error CNF-215's in InitTreeList(): " );

            //}
            //catch (Exception ex)
            //{
            //    XtraMessageBox.Show("An error occurred while reading the user's setting's file." + Environment.NewLine +
            //            "Error CNF-196 in ReadUserSettings(): " + ex.Message,
            //         "Caption...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

            //string testStr = @"ASH\ifrankel";
            //string domain = testStr.

        }

    }
}
