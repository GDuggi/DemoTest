namespace TestWorkbench
{
    partial class frmImageTest
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
            this.components = new System.ComponentModel.Container();
            this.components = new System.ComponentModel.Container();
            this.tabControlDocs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pictureEdit = new DevExpress.XtraEditors.PictureEdit();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pdfViewer = new DevExpress.XtraPdfViewer.PdfViewer();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.richEditControl = new DevExpress.XtraRichEdit.RichEditControl();
            //this.defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.tabControlSelect = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.gridControlMain = new DevExpress.XtraGrid.GridControl();
            this.gridViewInboundImages = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnGetAll = new System.Windows.Forms.Button();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.btnLoadDocx = new System.Windows.Forms.Button();
            this.btnLoadRtf = new System.Windows.Forms.Button();
            this.btnLoadDoc = new System.Windows.Forms.Button();
            this.btnLoadPdf = new System.Windows.Forms.Button();
            this.btnLoadTif = new System.Windows.Forms.Button();
            this.txtLoadImageId = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.comboBoxImageType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOpenFileDialog = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.txtSaveImageId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.textBoxSingleRecExt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxSingleRecDescr = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnGetRecord = new System.Windows.Forms.Button();
            this.tabControlDocs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit.Properties)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabControlSelect.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewInboundImages)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlDocs
            // 
            this.tabControlDocs.Controls.Add(this.tabPage1);
            this.tabControlDocs.Controls.Add(this.tabPage2);
            this.tabControlDocs.Controls.Add(this.tabPage3);
            this.tabControlDocs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlDocs.Location = new System.Drawing.Point(0, 192);
            this.tabControlDocs.Name = "tabControlDocs";
            this.tabControlDocs.SelectedIndex = 0;
            this.tabControlDocs.Size = new System.Drawing.Size(884, 555);
            this.tabControlDocs.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pictureEdit);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(876, 529);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Image";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // pictureEdit
            // 
            this.pictureEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureEdit.Location = new System.Drawing.Point(3, 3);
            this.pictureEdit.Name = "pictureEdit";
            this.pictureEdit.Properties.AllowScrollViaMouseDrag = true;
            this.pictureEdit.Properties.LookAndFeel.SkinName = "Money Twins";
            this.pictureEdit.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit.Properties.ShowScrollBars = true;
            this.pictureEdit.Size = new System.Drawing.Size(870, 523);
            this.pictureEdit.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.pdfViewer);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(876, 529);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "PDF";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // pdfViewer
            // 
            this.pdfViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pdfViewer.Location = new System.Drawing.Point(3, 3);
            this.pdfViewer.LookAndFeel.SkinName = "Money Twins";
            this.pdfViewer.Name = "pdfViewer";
            this.pdfViewer.Size = new System.Drawing.Size(870, 523);
            this.pdfViewer.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.richEditControl);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(876, 529);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Doc/Rtf";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // richEditControl
            // 
            this.richEditControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richEditControl.EnableToolTips = true;
            this.richEditControl.Location = new System.Drawing.Point(3, 3);
            this.richEditControl.LookAndFeel.SkinName = "Money Twins";
            this.richEditControl.Name = "richEditControl";
            this.richEditControl.Options.Bookmarks.AllowNameResolution = false;
            this.richEditControl.Size = new System.Drawing.Size(870, 523);
            this.richEditControl.TabIndex = 0;
            // 
            // defaultLookAndFeel
            // 
            //this.defaultLookAndFeel.LookAndFeel.SkinName = "Money Twins";
            // 
            // tabControlSelect
            // 
            this.tabControlSelect.Controls.Add(this.tabPage4);
            this.tabControlSelect.Controls.Add(this.tabPage5);
            this.tabControlSelect.Controls.Add(this.tabPage6);
            this.tabControlSelect.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControlSelect.Location = new System.Drawing.Point(0, 0);
            this.tabControlSelect.Name = "tabControlSelect";
            this.tabControlSelect.SelectedIndex = 0;
            this.tabControlSelect.Size = new System.Drawing.Size(884, 192);
            this.tabControlSelect.TabIndex = 11;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.gridControlMain);
            this.tabPage4.Controls.Add(this.panel1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(876, 166);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Get All";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // gridControlMain
            // 
            this.gridControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlMain.Location = new System.Drawing.Point(104, 3);
            this.gridControlMain.MainView = this.gridViewInboundImages;
            this.gridControlMain.Name = "gridControlMain";
            this.gridControlMain.Size = new System.Drawing.Size(769, 160);
            this.gridControlMain.TabIndex = 2;
            this.gridControlMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewInboundImages});
            // 
            // gridViewInboundImages
            // 
            this.gridViewInboundImages.GridControl = this.gridControlMain;
            this.gridViewInboundImages.Name = "gridViewInboundImages";
            this.gridViewInboundImages.OptionsBehavior.Editable = false;
            this.gridViewInboundImages.OptionsView.ShowGroupPanel = false;
            this.gridViewInboundImages.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewInboundImages_FocusedRowChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnGetAll);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(101, 160);
            this.panel1.TabIndex = 3;
            // 
            // btnGetAll
            // 
            this.btnGetAll.Location = new System.Drawing.Point(14, 12);
            this.btnGetAll.Name = "btnGetAll";
            this.btnGetAll.Size = new System.Drawing.Size(75, 23);
            this.btnGetAll.TabIndex = 2;
            this.btnGetAll.Text = "GetAll";
            this.btnGetAll.UseVisualStyleBackColor = true;
            this.btnGetAll.Click += new System.EventHandler(this.btnGetAll_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.groupBox2);
            this.tabPage5.Controls.Add(this.groupBox1);
            this.tabPage5.Controls.Add(this.txtLoadImageId);
            this.tabPage5.Controls.Add(this.label6);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(876, 166);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "Get Single";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // btnLoadDocx
            // 
            this.btnLoadDocx.Location = new System.Drawing.Point(202, 18);
            this.btnLoadDocx.Name = "btnLoadDocx";
            this.btnLoadDocx.Size = new System.Drawing.Size(75, 23);
            this.btnLoadDocx.TabIndex = 20;
            this.btnLoadDocx.Text = "Load Docx";
            this.btnLoadDocx.UseVisualStyleBackColor = true;
            this.btnLoadDocx.Click += new System.EventHandler(this.btnLoadDocx_Click);
            // 
            // btnLoadRtf
            // 
            this.btnLoadRtf.Location = new System.Drawing.Point(392, 18);
            this.btnLoadRtf.Name = "btnLoadRtf";
            this.btnLoadRtf.Size = new System.Drawing.Size(75, 23);
            this.btnLoadRtf.TabIndex = 19;
            this.btnLoadRtf.Text = "Load Rtf";
            this.btnLoadRtf.UseVisualStyleBackColor = true;
            this.btnLoadRtf.Click += new System.EventHandler(this.btnLoadRtf_Click);
            // 
            // btnLoadDoc
            // 
            this.btnLoadDoc.Location = new System.Drawing.Point(297, 18);
            this.btnLoadDoc.Name = "btnLoadDoc";
            this.btnLoadDoc.Size = new System.Drawing.Size(75, 23);
            this.btnLoadDoc.TabIndex = 18;
            this.btnLoadDoc.Text = "Load Doc";
            this.btnLoadDoc.UseVisualStyleBackColor = true;
            this.btnLoadDoc.Click += new System.EventHandler(this.btnLoadDoc_Click);
            // 
            // btnLoadPdf
            // 
            this.btnLoadPdf.Location = new System.Drawing.Point(107, 18);
            this.btnLoadPdf.Name = "btnLoadPdf";
            this.btnLoadPdf.Size = new System.Drawing.Size(75, 23);
            this.btnLoadPdf.TabIndex = 17;
            this.btnLoadPdf.Text = "Load Pdf";
            this.btnLoadPdf.UseVisualStyleBackColor = true;
            this.btnLoadPdf.Click += new System.EventHandler(this.btnLoadPdf_Click);
            // 
            // btnLoadTif
            // 
            this.btnLoadTif.Location = new System.Drawing.Point(17, 19);
            this.btnLoadTif.Name = "btnLoadTif";
            this.btnLoadTif.Size = new System.Drawing.Size(75, 23);
            this.btnLoadTif.TabIndex = 16;
            this.btnLoadTif.Text = "Load Tif";
            this.btnLoadTif.UseVisualStyleBackColor = true;
            this.btnLoadTif.Click += new System.EventHandler(this.btnLoadTif_Click);
            // 
            // txtLoadImageId
            // 
            this.txtLoadImageId.Location = new System.Drawing.Point(64, 25);
            this.txtLoadImageId.Name = "txtLoadImageId";
            this.txtLoadImageId.Size = new System.Drawing.Size(62, 20);
            this.txtLoadImageId.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Image Id:";
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.comboBoxImageType);
            this.tabPage6.Controls.Add(this.label4);
            this.tabPage6.Controls.Add(this.btnOpenFileDialog);
            this.tabPage6.Controls.Add(this.btnSave);
            this.tabPage6.Controls.Add(this.txtDescription);
            this.tabPage6.Controls.Add(this.txtFilename);
            this.tabPage6.Controls.Add(this.txtSaveImageId);
            this.tabPage6.Controls.Add(this.label3);
            this.tabPage6.Controls.Add(this.label2);
            this.tabPage6.Controls.Add(this.label1);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(876, 166);
            this.tabPage6.TabIndex = 2;
            this.tabPage6.Text = "Insert Row";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // comboBoxImageType
            // 
            this.comboBoxImageType.FormattingEnabled = true;
            this.comboBoxImageType.Items.AddRange(new object[] {
            "TIF",
            "PDF",
            "DOC",
            "DOCX"});
            this.comboBoxImageType.Location = new System.Drawing.Point(244, 15);
            this.comboBoxImageType.Name = "comboBoxImageType";
            this.comboBoxImageType.Size = new System.Drawing.Size(84, 21);
            this.comboBoxImageType.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(176, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Image Type:";
            // 
            // btnOpenFileDialog
            // 
            this.btnOpenFileDialog.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFileDialog.Location = new System.Drawing.Point(646, 40);
            this.btnOpenFileDialog.Name = "btnOpenFileDialog";
            this.btnOpenFileDialog.Size = new System.Drawing.Size(20, 20);
            this.btnOpenFileDialog.TabIndex = 17;
            this.btnOpenFileDialog.Text = "...";
            this.btnOpenFileDialog.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnOpenFileDialog.UseVisualStyleBackColor = true;
            this.btnOpenFileDialog.Click += new System.EventHandler(this.btnOpenFileDialog_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(591, 9);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(90, 64);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(576, 20);
            this.txtDescription.TabIndex = 15;
            // 
            // txtFilename
            // 
            this.txtFilename.Location = new System.Drawing.Point(90, 40);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.Size = new System.Drawing.Size(550, 20);
            this.txtFilename.TabIndex = 14;
            // 
            // txtSaveImageId
            // 
            this.txtSaveImageId.Location = new System.Drawing.Point(90, 16);
            this.txtSaveImageId.Name = "txtSaveImageId";
            this.txtSaveImageId.Size = new System.Drawing.Size(62, 20);
            this.txtSaveImageId.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Description:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Filename:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Image Id:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 725);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(884, 22);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // textBoxSingleRecExt
            // 
            this.textBoxSingleRecExt.Location = new System.Drawing.Point(92, 17);
            this.textBoxSingleRecExt.Name = "textBoxSingleRecExt";
            this.textBoxSingleRecExt.Size = new System.Drawing.Size(62, 20);
            this.textBoxSingleRecExt.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Image File Ext:";
            // 
            // textBoxSingleRecDescr
            // 
            this.textBoxSingleRecDescr.Location = new System.Drawing.Point(236, 17);
            this.textBoxSingleRecDescr.Name = "textBoxSingleRecDescr";
            this.textBoxSingleRecDescr.Size = new System.Drawing.Size(597, 20);
            this.textBoxSingleRecDescr.TabIndex = 24;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(173, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "Description:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLoadDoc);
            this.groupBox1.Controls.Add(this.btnLoadTif);
            this.groupBox1.Controls.Add(this.btnLoadPdf);
            this.groupBox1.Controls.Add(this.btnLoadRtf);
            this.groupBox1.Controls.Add(this.btnLoadDocx);
            this.groupBox1.Location = new System.Drawing.Point(139, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(483, 58);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Get Specific Type";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnGetRecord);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBoxSingleRecExt);
            this.groupBox2.Controls.Add(this.textBoxSingleRecDescr);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(10, 72);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(845, 77);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Get Full Record";
            // 
            // btnGetRecord
            // 
            this.btnGetRecord.Location = new System.Drawing.Point(16, 46);
            this.btnGetRecord.Name = "btnGetRecord";
            this.btnGetRecord.Size = new System.Drawing.Size(87, 23);
            this.btnGetRecord.TabIndex = 25;
            this.btnGetRecord.Text = "Get Record";
            this.btnGetRecord.UseVisualStyleBackColor = true;
            this.btnGetRecord.Click += new System.EventHandler(this.btnGetRecord_Click);
            // 
            // frmImageTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 747);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControlDocs);
            this.Controls.Add(this.tabControlSelect);
            this.Name = "frmImageTest";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Image Test -- ACCESSES TESTIMAGE DATABASE";
            this.tabControlDocs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit.Properties)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabControlSelect.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewInboundImages)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlDocs;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private DevExpress.XtraPdfViewer.PdfViewer pdfViewer;
        private System.Windows.Forms.TabPage tabPage3;
        private DevExpress.XtraRichEdit.RichEditControl richEditControl;
        private DevExpress.XtraEditors.PictureEdit pictureEdit;
        //private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel;
        private System.Windows.Forms.TabControl tabControlSelect;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button btnLoadDocx;
        private System.Windows.Forms.Button btnLoadRtf;
        private System.Windows.Forms.Button btnLoadDoc;
        private System.Windows.Forms.Button btnLoadPdf;
        private System.Windows.Forms.Button btnLoadTif;
        private System.Windows.Forms.TextBox txtLoadImageId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.ComboBox comboBoxImageType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOpenFileDialog;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtFilename;
        private System.Windows.Forms.TextBox txtSaveImageId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraGrid.GridControl gridControlMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewInboundImages;
        private System.Windows.Forms.Button btnGetAll;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnGetRecord;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxSingleRecExt;
        private System.Windows.Forms.TextBox textBoxSingleRecDescr;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}