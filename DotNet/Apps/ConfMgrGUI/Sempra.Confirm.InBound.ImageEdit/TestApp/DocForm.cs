using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace InboundDocuments
{
    public partial class DocForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        public DocForm()
        {
            // Required for Windows Form Designer support
            InitializeComponent();
            this.tifEditor1.AllowDrop = true;

            this.tifEditor1.Dock = System.Windows.Forms.DockStyle.Fill;

            this.tifEditor1.Edit = false;

            this.tifEditor1.ExitDelegate = null;

            this.tifEditor1.ImageFileName = null;

            this.tifEditor1.Location = new System.Drawing.Point(0, 0);

            this.tifEditor1.Name = "tifEditor1";

            this.tifEditor1.SaveAsFileName = null;

            this.tifEditor1.ScaleFactor = 1;

            this.tifEditor1.Size = new System.Drawing.Size(143, 451);

            
            this.tifEditor1.TabIndex = 1;

            this.tifEditor1.TransDelegate = null;

            this.tifEditor1.UserName = null;


            // TODO: Add any constructor code after InitializeComponent call
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.tifEditor1.Visible = true;
            // imgEdit.ImageFileName = @"C:\tif\testquality.tif";
            tifEditor1.ImageFileName = @"c:\temp\test\test3.Tif";
            tifEditor1.LoadImage();
            tifEditor1.ScaleFactor = 1;
            tifEditor1.TransDelegate = this.TransmitFile;
            tifEditor1.Enabled = true;
            this.tifEditor1.Edit = true;
        }
        private void TransmitFile()
        {
            tifEditor1.Publish();
            MessageBox.Show("Transmit called");
        }
    }
}