using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sempra.Confirm.InBound.ImageEdit
{
    

    public partial class ImageEditForm : Form
    {
        private string fileName;
        private double imageScaleFactor = 1;
        private string userName;
        private TransmitDelegate transmitCallback;
        private bool allowEdit = true;

        public bool AllowEdit
        {
            get { return this.tifEditor.Edit; }
            set 
            {
                this.tifEditor.Edit = value;
                allowEdit = this.tifEditor.Edit;
            }
        }


        public TransmitDelegate TransmitCallback
        {
            get { return transmitCallback; }
            set 
            { 
                transmitCallback = value;
                this.tifEditor.TransDelegate = value;

            }
        }


        public string UserName
        {
            get { return userName; }
            set { 
                userName = value;
                this.tifEditor.UserName = value;
            }
        }


        public double ImageScaleFactor
        {
            get { return imageScaleFactor; }
            set { 
                imageScaleFactor = value;
                this.tifEditor.ScaleFactor = value;
            }
        }

        public string FileName
        {
            get { return fileName; }
            set { 
                fileName = value;
                if (fileName.IndexOf(".txt", StringComparison.CurrentCultureIgnoreCase) >= 0)
                {
                    this.Text = "Telex (" + fileName + ")";
                }
                else
                {
                    this.Text = "Fax (" + fileName + ")";
                }
                tifEditor.ImageFileName = fileName;
            }
        }


        public ImageEditForm()
        {
            try
            {
                InitializeComponent();
                InitalizeTifEditor();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error ImageEditForm Constructor : " + ex.Message);
            }
        }

        private void InitalizeTifEditor()
        {
            tifEditor = new TifEditor();           
            tifEditor.ExitDelegate = this.ExitHandler;
            this.tifEditor.Dock = DockStyle.Fill;
            tifEditor.Edit = allowEdit;
            this.Controls.Add(tifEditor);
            
        }

        private void ExitHandler()
        {
            this.Close();
        }

        private void ImageEditForm_Load(object sender, EventArgs e)
        {
            this.tifEditor.LoadImage();
            this.tifEditor.Edit = allowEdit;
        }

        private void ImageEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.tifEditor.ImageModified == true)
            {
                DialogResult result = MessageBox.Show("The image is modified. Do you want to save the image?", "Tif Editor", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (result == DialogResult.Yes)
                {
                    this.tifEditor.SaveToFile();
                }
            }
        }
    }
}