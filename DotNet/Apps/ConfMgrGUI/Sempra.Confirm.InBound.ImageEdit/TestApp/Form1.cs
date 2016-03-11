using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Sempra.Confirm.InBound.ImageEdit;
using System.IO;
using Sempra.Confirm.InBound.Comm;
using Leadtools.Codecs;
using Leadtools;

namespace InboundDocuments
{
    public partial class Form1 : Form
    {
        ImageEditForm form = null;
        public Form1()
        {
            try
            {
                this.form = new ImageEditForm();
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Form1 Constructor : " + ex.Message);
            }
        }


        private void Load_Click_1(object sender, EventArgs e)
        {
            //ConvertFileFormat();
            DocForm form1 = new DocForm();
            form1.Show();
            /*
            MessageBox.Show(this.imgEdit.ImageModified.ToString());
            LoadAssociatedDocumentInImageControl("C:\\TEMP\\test\\test3.tif");
             * */
            /*
            this.imgEdit.UserName = "CRobotti";
            this.imgEdit.ImageFileName = "c:\\Temp\\Tif\\000046C7000.tif";
            
            //this.imgEdit.ImageFileName = "c:\\Temp\\Tif\\00004F02000.tif";
            
         //   this.imgEdit.ImageFileName = "c:\\Water lilies.jpg";
            this.imgEdit.ScaleFactor = 0.5;
            this.imgEdit.LoadImage();
            this.imgEdit.Edit = true;
         //   this.imgEdit.SaveAsFileName = "C:\\temp_2.tif";
            
             * */
        }

        private void ConvertFileFormat()
        {
            
            RasterCodecs codec = new RasterCodecs();
            //codec.Options.s
           // CodecsLoadOptions options = new CodecsLoadOptions();


            CodecsPdfLoadOptions options = codec.Options.Pdf.Load;


            //CodecsRtfLoadOptions options1 = codec.Options.Rtf.Load;
            //options1.

            RasterImage image = codec.Load(@"c:\leadtools\gas.pdf", 0, CodecsLoadByteOrder.RgbOrGray, 1, 1);
            

            codec.Save(image, @"c:\leadtools\gas.tif", RasterImageFormat.TifxFaxG4, 1, 1, image.PageCount, 1, CodecsSavePageMode.Overwrite);
            MessageBox.Show("Done");
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            /*
            this.imgEdit.Left = this.Left;
            this.imgEdit.Top = this.Top;
            //this.imgEdit.Right = this.Right;
            this.imgEdit.Height = this.Height;
             * */

         //   this.imgEdit.Size = this.Size;
            this.imgEdit.Width = this.Width - 100;
            this.imgEdit.Height = this.Height - 100;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            form.UserName = "ztraynor";
         //   form.ImageScaleFactor = 0.6;
         //   form.FileName = "c:\\Temp\\Tif\\000046C7000.tif";
          //  form.FileName = @"c:\lead\group3.tif";
          //  Stream str = new FileStream(@"c:\lead\group3.tif", FileMode.Open);
                // form.TransmitCallback = this.TransmitFile;
            //imgEdit.LoadImage(str);
          //  imgEdit.LoadImage();
         //   imgEdit.ImageFileName = @"c:\lead\20091216_022828275_WIREFAST06_13051.tif";
            imgEdit.ImageFileName = @"c:\temp\20100121_094333231_NUON_13051_1.tif";
            imgEdit.Enabled = true;
           // form.FileName = @"c:\lead\group3.tif";
         //   form.FileName = @"c:\lead\print.tif";
         //    form.AllowEdit = false;
         //   form.AllowEdit = true;
            
        //    form.ShowDialog(this);
            imgEdit.LoadImage();
            imgEdit.Edit = true;

            /*
                       // LoadAssociatedDocumentInImageControl("C:\\TEMP\\TEST.TIF");
            
                        this.imgEdit.ScaleFactor = 1;
            
                        this.imgEdit.ImageFileName = "c:\\Temp\\Tif\\000009B5000.tif";
            
                     //   this.imgEdit.ImageFileName = "c:\\TEMP\\TXT\\20061211_0949473.txt";
                       // this.imgEdit.ImageFileName = "group3.tif";
                     //   this.imgEdit.ImageFileName = null;
                        MessageBox.Show(this.imgEdit.ImageModified.ToString());
                        this.imgEdit.LoadImage();
                        this.imgEdit.Edit = false;
                        int x = 0;
                        x = x + 1;
              */

        }

        private void button2_Click(object sender, EventArgs e)
        {
        
            /*
           // form.UserName = "CRobotti";
           // form.UserName = "ZTraynor";
            form.ImageScaleFactor = 1;
            form.FileName = "c:\\Temp\\Tif\\000046C7000.tif";
            form.FileName = @"C:\temp\tif\20071128_074503_DCP Midstream_6615.tif";
            //form.FileName = @"C:\tif\20071218_1012263041.tif";
            form.FileName = @"C:\tif\testquality.tif";
            form.TransmitCallback = this.TransmitFile;
            //form.FileName = "c:\\TEMP\\TXT\\20061211_0949473.txt"; ;
           // form.AllowEdit = false;
            form.AllowEdit = true;
            form.ShowDialog(this);
            */

            
            imgEdit.Visible = true;
           // imgEdit.ImageFileName = @"C:\tif\testquality.tif";
       //     imgEdit.ImageFileName = @"c:\temp\test\test3.Tif";
          //  imgEdit.ImageFileName = @"\\Stocrqa1\InboundDocuments\OCROutputDirectory\20090218_1323125576.tif";
         //   imgEdit.ImageFileName = @"\\STOCRQA1\InboundDocuments\OCROutputDirectory\20090402_171922930_212_66121.tif";
         //   imgEdit.ImageFileName = @"c:\temp\tif\20071128_074503_DCP Midstream_6615.tif";
           // imgEdit.ImageFileName = @"C:\test5.tif";
            imgEdit.ImageFileName = @"c:\tools\tif\SharePointDoc.tif";
     //     Stream str = new FileStream(@"c:\tools\txt\pending.txt", FileMode.Open);
            imgEdit.LoadImage();
            imgEdit.ScaleFactor = 1;
            imgEdit.TransDelegate = this.TransmitFile;
            imgEdit.Enabled = true;
        //    str.Close();
          //  this.imgEdit.Edit = true;
          //  this.imgEdit.Publish();
        
        }
        private void TransmitFile()
        {
            imgEdit.Publish();
            MessageBox.Show("Transmit called");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.imgEdit.ImageModified == true)
            {
                if (MessageBox.Show("The image is modified? Save?","TifEditor",MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.imgEdit.SaveToFile();
                }
            }
        }

        private void LoadAssociatedDocumentInImageControl(string fileName)
        {

            try
            {

                string DocumentsLocation = "C:\\TEMP\\TIF\\";

                this.Cursor = Cursors.WaitCursor;

                string parentFileName = "";

                if (!File.Exists(fileName))
                {

                    parentFileName = DocumentsLocation + GetInboundActiveFileName();

                    if (!File.Exists(parentFileName))
                    {

                        throw new Exception("Associated Doc and Parrent Doc files do not exist. Please contact confirm support");

                    }

                    else this.imgEdit.ImageFileName = parentFileName;

                }

                else this.imgEdit.ImageFileName = fileName;

                this.imgEdit.LoadImage();

                this.imgEdit.SaveAsFileName = fileName;

                this.imgEdit.Edit = true;

            }

            finally
            {

                this.Cursor = Cursors.Default;

            }

        }

        private string GetInboundActiveFileName()
        {
           return "20061206_112428_6600.tif" ;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

            UserFilterForm form = new UserFilterForm();
           // form.Show();
            form.ShowDialog();
//            Emailer email = new Emailer();
          //  email.SendToFaxGateway("mailhost", "srajaman@sempratrading.com","DevFaxgateway@sempratrading.com", GetRequest(), "c:\\temp");
          //  email.SendToFaxGateway("mailhost", "srajaman@sempratrading.com", "srajaman@sempratrading.com", GetRequest(), "c:\\temp");
           // email.SendToFaxGateway("mailhost", "srajaman@sempratrading.com", "Faxgateway@sempratrading.com", GetRequest(), "c:\\temp");
  //          email.Send("mailhost","srajaman@sempratrading.com","srajaman@sempratrading.com","Test Mail","Test Body",new string[]{@"c:\temp\test\test3.Tif"});
           // email.SendToFaxGateway("10.32.99.61", "srajaman@sempratrading.com", "srajaman@sempratrading.com", null, "");
    //        MessageBox.Show("Done");
        }
        private FaxRequest GetRequest()
        {
            FaxRequest request = new FaxRequest();

            request.AppCode = "CNF";
            request.AppReference = "293023";
            request.AppSender = "srajaman";
            request.ReceiptMethodType = "email";
            request.ReceiptMethodValue = "srajaman@sempratrading.com";
            request.Recipient = "Samy Raj";
            request.AddDocument(@"c:\temp\test\test3.Tif", 1);
            request.FaxNumber = "3555330";
            request.Action.SucessMethodType = "email";
            request.Action.SucessMethodValue = "srajaman@sempratrading.com";
            request.Action.FailMethodType = "email";
            request.Action.FailMethodValue = "srajaman@sempratrading.com";


            return request;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //this.imgEdit.Publish();
           // PageSelection ps = new PageSelection();
           // ps.ShowDialog();
            TifUtil.ExtractPagesWithAnnotation(@"C:\lead\20091216_022828275_WIREFAST06_13051.tif", @"C:\test1.tif", "1");
         //   TifUtil.ExtractPagesWithAnnotation(@"C:\temp\tif\20090414_093651276_Rightfax_66311.tif", @"C:\test1.tif", "1,2");
            
            MessageBox.Show("Done");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TifUtil.MergeFiles(@"C:\temp\tif\20071128_074503_DCP Midstream_6615.tif", @"C:\test5.tif", true);
        }


    }
}