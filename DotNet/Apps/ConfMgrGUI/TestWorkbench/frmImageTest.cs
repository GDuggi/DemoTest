using DBAccess;
using DBAccess.SqlServer;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraRichEdit;
using DevExpress.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestWorkbench
{
    public partial class frmImageTest : Form
    {
        private const string TestImageConnStr = @"database=TestImage;server=CNF01INFDBS01\SQLSVR11;integrated security=sspi;";
        private const string FORM_NAME = "frmImageTest";
        public List<ImagesDto> inbImageList = new List<ImagesDto>();
        private MemoryStream pdfMemStream;        

        public frmImageTest()
        {
            InitializeComponent();
        }

        private byte[] getBlobByteArray(Int32 pImageId)
        {
            byte[] imageBytes = null;
            //ImagesDal inbImgDal = new ImagesDal(TestImageConnStr);
            //imageBytes = inbImgDal.TestGetByteArray(pImageId);
            return imageBytes;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //ImagesDal inbImgDal = new ImagesDal(TestImageConnStr);
            this.Cursor = Cursors.WaitCursor;
            try
            {
                int imageId = int.Parse(this.txtSaveImageId.Text);
                string desc = this.txtDescription.Text;
                string filename = this.txtFilename.Text;
                string fileext = Path.GetExtension(filename).ToUpper().Replace(".", "");
                comboBoxImageType.Text = fileext;
                //inbImgDal.TestInsertDocImage(imageId, desc, filename, fileext);
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

        private void btnLoadTif_Click(object sender, EventArgs e)
        {
            byte[] imageBytes;
            try
            {
                tabControlDocs.SelectedIndex = 0;
                this.Cursor = Cursors.WaitCursor;
                Int32 imageId = Convert.ToInt32(txtLoadImageId.Text);
                imageBytes = getBlobByteArray(imageId);
                pictureEdit.Image = DBUtils.ConvertByteStreamToImage(imageBytes);
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message);
            }
            finally
            {                
                this.Cursor = Cursors.Default;                
            }
        }

        private void btnLoadPdf_Click(object sender, EventArgs e)
        {
            byte[] imageBytes;
            try
            {
                tabControlDocs.SelectedIndex = 1;
                this.Cursor = Cursors.WaitCursor;
                Int32 imageId = Convert.ToInt32(txtLoadImageId.Text);
                imageBytes = getBlobByteArray(imageId);
                //NOTE: the memory stream used to load Pdf cannot be closed while Pdf is displaying.
                pdfMemStream = new MemoryStream(imageBytes);
                pdfViewer.LoadDocument(pdfMemStream);

                //using (var memStream = new MemoryStream(imageBytes))
                //{
                //    pdfViewer.LoadDocument(memStream);
                //}
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnLoadDocx_Click(object sender, EventArgs e)
        {
            byte[] imageBytes;
            try
            {
                tabControlDocs.SelectedIndex = 2;
                this.Cursor = Cursors.WaitCursor;
                Int32 imageId = Convert.ToInt32(txtLoadImageId.Text);
                imageBytes = getBlobByteArray(imageId);

                using (var memStream = new MemoryStream(imageBytes))
                {
                    richEditControl.Document.LoadDocument(memStream, DevExpress.XtraRichEdit.DocumentFormat.OpenXml);
                }
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnLoadDoc_Click(object sender, EventArgs e)
        {
            byte[] imageBytes;
            try
            {
                tabControlDocs.SelectedIndex = 2;
                this.Cursor = Cursors.WaitCursor;
                Int32 imageId = Convert.ToInt32(txtLoadImageId.Text);
                imageBytes = getBlobByteArray(imageId);

                using (var memStream = new MemoryStream(imageBytes))
                {
                    richEditControl.Document.LoadDocument(memStream, DevExpress.XtraRichEdit.DocumentFormat.Doc);
                }
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnLoadRtf_Click(object sender, EventArgs e)
        {
            byte[] imageBytes;
            try
            {
                tabControlDocs.SelectedIndex = 2;
                this.Cursor = Cursors.WaitCursor;
                Int32 imageId = Convert.ToInt32(txtLoadImageId.Text);
                imageBytes = getBlobByteArray(imageId);

                using (var memStream = new MemoryStream(imageBytes))
                {
                    richEditControl.Document.LoadDocument(memStream, DevExpress.XtraRichEdit.DocumentFormat.Rtf);
                }
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnOpenFileDialog_Click(object sender, EventArgs e)
        {
            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "All Files (*.*)|*.*|Image Files (.tif)|*.tif|PDF Files (.pdf)|*.pdf";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.Multiselect = false;

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            // Process input if the user clicked OK.
            {
                txtFilename.Text = openFileDialog1.FileName;
                comboBoxImageType.Text = Path.GetExtension(txtFilename.Text).ToUpper().Replace(".", "");
            }
        }

        private void btnGetAll_Click(object sender, EventArgs e)
        {
            //ImagesDal inbImgDal = new ImagesDal(TestImageConnStr);
            try
            {
                //this.Cursor = Cursors.WaitCursor;
                //inbImageList = inbImgDal.TestGetAll();
                //BindingList<ImagesDto> listDataSource = new BindingList<ImagesDto>();
                //Int32 index = 0;

                //foreach (ImagesDto data in inbImageList)
                //{
                //    listDataSource.Add(new ImagesDto
                //    {
                //        ImageId = data.ImageId,
                //        //RowId = data.RowId,
                //        RowId = index,                        
                //        OriginalImageFileExt = data.OriginalImageFileExt,
                //        OriginalImage = data.OriginalImage
                //    });

                //    index++;
                //}

                //gridControlMain.DataSource = listDataSource;
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void gridViewInboundImages_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int rowHandle = 0;
            if (e.FocusedRowHandle < 0)
            {
                rowHandle = e.PrevFocusedRowHandle;
                if (rowHandle < 0)
                    return;
            }
            else rowHandle = e.FocusedRowHandle;
   
            GridView view = sender as GridView;
            Int32 rowId = 0;
            rowId = Convert.ToInt32( view.GetRowCellDisplayText(rowHandle, "RowId"));
            displayImageFromGrid(rowId);

        }

        private void displayImageFromGrid(Int32 pRowId)
        {
            string imageExt = "";
            byte[] imageByteArray;
            imageExt = inbImageList[pRowId].OriginalImageFileExt;
            imageByteArray = inbImageList[pRowId].OriginalImage;
            DocumentFormat docFormat;

            switch (imageExt.ToUpper())
            {
                case "TIF":
                    tabControlDocs.SelectedIndex = 0;
                    pictureEdit.Image = DBUtils.ConvertByteStreamToImage(imageByteArray);
                    break;
                case "PDF":
                    //NOTE: the memory stream used to load Pdf cannot be closed while Pdf is displaying.
                    tabControlDocs.SelectedIndex = 1;
                    pdfMemStream = new MemoryStream(imageByteArray);
                    pdfViewer.LoadDocument(pdfMemStream);
                    break;
                case "RTF":
                case "DOC":
                case "DOCX":
                    tabControlDocs.SelectedIndex = 2;
                    docFormat = getDocumentFormatType(imageExt);
                    using (var memStream = new MemoryStream(imageByteArray))
                    {
                        richEditControl.Document.LoadDocument(memStream, docFormat);
                    }
                    break;
            }
        }

        private void displayImageFromRec(string pImageFileExt, byte[] pImageByteArray) 
        {
            DocumentFormat docFormat;
            switch (pImageFileExt.ToUpper())
            {
                case "TIF":
                    tabControlDocs.SelectedIndex = 0;
                    pictureEdit.Image = DBUtils.ConvertByteStreamToImage(pImageByteArray);
                    break;
                case "PDF":
                    //NOTE: the memory stream used to load Pdf cannot be closed while Pdf is displaying.
                    tabControlDocs.SelectedIndex = 1;
                    pdfMemStream = new MemoryStream(pImageByteArray);
                    pdfViewer.LoadDocument(pdfMemStream);
                    break;
                case "RTF":
                case "DOC":
                case "DOCX":
                    tabControlDocs.SelectedIndex = 2;
                    docFormat = getDocumentFormatType(pImageFileExt.ToUpper());
                    using (var memStream = new MemoryStream(pImageByteArray))
                    {
                        richEditControl.Document.LoadDocument(memStream, docFormat);
                    }
                    break;
            }
        }


        private DocumentFormat getDocumentFormatType(string imageExt)
        {
            DocumentFormat docFormat;
            switch (imageExt.ToUpper())
            {
                case "DOC":
                    docFormat = DocumentFormat.Doc;
                    break;
                case "DOCX":
                    docFormat = DocumentFormat.OpenXml;
                    break;
                case "RTF":
                    docFormat = DocumentFormat.Rtf;
                    break;
                default:
                    throw new Exception(String.Format("Document format for: " + imageExt + " was not found.") + Environment.NewLine +
                         "Error CNF-544 in " + FORM_NAME + ".getDocumentFormatType().");
                    break;
            }
            return docFormat;
        }

        private void btnGetRecord_Click(object sender, EventArgs e)
        {
            //ImagesDal inbImgDal = new ImagesDal(TestImageConnStr);
            //ImagesDto inbImgData = new ImagesDto();
            try
            {
                //this.Cursor = Cursors.WaitCursor;
                //Int32 imageId = Convert.ToInt32(txtLoadImageId.Text);
                //inbImgData = inbImgDal.TestGet(imageId);
                //textBoxSingleRecExt.Text = inbImgData.OriginalImageFileExt;                
                //displayImageFromRec(inbImgData.OriginalImageFileExt, inbImgData.OriginalImage);
            }
            catch (Exception except)
            {
                MessageBox.Show(except.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }


    }
}
