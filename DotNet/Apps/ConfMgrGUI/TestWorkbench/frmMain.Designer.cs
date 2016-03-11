namespace TestWorkbench
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtFileExt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.txtSavePhotoId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabControlImage = new System.Windows.Forms.TabControl();
            this.tabPageTif = new System.Windows.Forms.TabPage();
            this.dxPictureEdit = new DevExpress.XtraEditors.PictureEdit();
            this.tabPagePdf = new System.Windows.Forms.TabPage();
            this.pdfViewer = new DevExpress.XtraPdfViewer.PdfViewer();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnLoad = new System.Windows.Forms.Button();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtLoadPhotoId = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnClose = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControlImage.SuspendLayout();
            this.tabPageTif.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dxPictureEdit.Properties)).BeginInit();
            this.tabPagePdf.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.txtFileExt);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.txtDescription);
            this.groupBox1.Controls.Add(this.txtFilename);
            this.groupBox1.Controls.Add(this.txtSavePhotoId);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(22, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(610, 104);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Insert Doc Image";
            // 
            // txtFileExt
            // 
            this.txtFileExt.Location = new System.Drawing.Point(233, 20);
            this.txtFileExt.Name = "txtFileExt";
            this.txtFileExt.ReadOnly = true;
            this.txtFileExt.Size = new System.Drawing.Size(62, 20);
            this.txtFileExt.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(170, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Image Type:";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(570, 44);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(20, 20);
            this.button1.TabIndex = 7;
            this.button1.Text = "...";
            this.button1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.openFileDialog_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(515, 13);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(87, 70);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(503, 20);
            this.txtDescription.TabIndex = 5;
            // 
            // txtFilename
            // 
            this.txtFilename.Location = new System.Drawing.Point(87, 44);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.Size = new System.Drawing.Size(477, 20);
            this.txtFilename.TabIndex = 4;
            // 
            // txtSavePhotoId
            // 
            this.txtSavePhotoId.Location = new System.Drawing.Point(87, 20);
            this.txtSavePhotoId.Name = "txtSavePhotoId";
            this.txtSavePhotoId.Size = new System.Drawing.Size(62, 20);
            this.txtSavePhotoId.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Description:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Filename:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Image Id:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tabControlImage);
            this.groupBox2.Controls.Add(this.btnLoad);
            this.groupBox2.Controls.Add(this.lblDescription);
            this.groupBox2.Controls.Add(this.txtLoadPhotoId);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(22, 123);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(610, 636);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Insert Doc Image";
            // 
            // tabControlImage
            // 
            this.tabControlImage.Controls.Add(this.tabPageTif);
            this.tabControlImage.Controls.Add(this.tabPagePdf);
            this.tabControlImage.Controls.Add(this.tabPage1);
            this.tabControlImage.Location = new System.Drawing.Point(87, 71);
            this.tabControlImage.Name = "tabControlImage";
            this.tabControlImage.SelectedIndex = 0;
            this.tabControlImage.Size = new System.Drawing.Size(503, 544);
            this.tabControlImage.TabIndex = 10;
            // 
            // tabPageTif
            // 
            this.tabPageTif.Controls.Add(this.dxPictureEdit);
            this.tabPageTif.Location = new System.Drawing.Point(4, 22);
            this.tabPageTif.Name = "tabPageTif";
            this.tabPageTif.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTif.Size = new System.Drawing.Size(495, 518);
            this.tabPageTif.TabIndex = 0;
            this.tabPageTif.Text = "TIF";
            this.tabPageTif.UseVisualStyleBackColor = true;
            // 
            // dxPictureEdit
            // 
            this.dxPictureEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dxPictureEdit.Location = new System.Drawing.Point(3, 3);
            this.dxPictureEdit.Name = "dxPictureEdit";
            this.dxPictureEdit.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.dxPictureEdit.Size = new System.Drawing.Size(489, 512);
            this.dxPictureEdit.TabIndex = 9;
            // 
            // tabPagePdf
            // 
            this.tabPagePdf.Controls.Add(this.pdfViewer);
            this.tabPagePdf.Location = new System.Drawing.Point(4, 22);
            this.tabPagePdf.Name = "tabPagePdf";
            this.tabPagePdf.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePdf.Size = new System.Drawing.Size(495, 518);
            this.tabPagePdf.TabIndex = 1;
            this.tabPagePdf.Text = "PDF";
            this.tabPagePdf.UseVisualStyleBackColor = true;
            // 
            // pdfViewer
            // 
            this.pdfViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pdfViewer.Location = new System.Drawing.Point(3, 3);
            this.pdfViewer.Name = "pdfViewer";
            this.pdfViewer.Size = new System.Drawing.Size(489, 512);
            this.pdfViewer.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(495, 518);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(515, 19);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 8;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(87, 47);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(0, 13);
            this.lblDescription.TabIndex = 6;
            // 
            // txtLoadPhotoId
            // 
            this.txtLoadPhotoId.Location = new System.Drawing.Point(87, 20);
            this.txtLoadPhotoId.Name = "txtLoadPhotoId";
            this.txtLoadPhotoId.Size = new System.Drawing.Size(62, 20);
            this.txtLoadPhotoId.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Image:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Description:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Image Id:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.InitialDirectory = "C:\\Users\\ifrankel\\DevTools\\Sample Docs\\";
            this.openFileDialog1.Title = "Select Image File";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(537, 769);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(376, 13);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Test";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 800);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Location = new System.Drawing.Point(500, 300);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Doc Image Form";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControlImage.ResumeLayout(false);
            this.tabPageTif.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dxPictureEdit.Properties)).EndInit();
            this.tabPagePdf.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtFilename;
        private System.Windows.Forms.TextBox txtSavePhotoId;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtLoadPhotoId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnClose;
        private DevExpress.XtraEditors.PictureEdit dxPictureEdit;
        private System.Windows.Forms.TabControl tabControlImage;
        private System.Windows.Forms.TabPage tabPageTif;
        private System.Windows.Forms.TabPage tabPagePdf;
        private DevExpress.XtraPdfViewer.PdfViewer pdfViewer;
        private System.Windows.Forms.TextBox txtFileExt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button button2;
    }
}

