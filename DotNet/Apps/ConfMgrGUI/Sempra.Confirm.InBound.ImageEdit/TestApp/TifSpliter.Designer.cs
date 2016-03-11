namespace InboundDocuments
{
    partial class TifSpliter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TifSpliter));
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.srcImageList = new Leadtools.WinForms.RasterImageList();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.cboZoom = new System.Windows.Forms.ComboBox();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.spiltImageTab = new DevExpress.XtraTab.XtraTabPage();
            this.mainPanel = new DevExpress.XtraEditors.PanelControl();
            this.destImageList = new Leadtools.WinForms.RasterImageList();
            this.buttonPanel = new DevExpress.XtraEditors.PanelControl();
            this.Delete = new DevExpress.XtraEditors.SimpleButton();
            this.saveButton = new DevExpress.XtraEditors.SimpleButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripAdd = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.spiltImageTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPanel)).BeginInit();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.buttonPanel)).BeginInit();
            this.buttonPanel.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.srcImageList);
            this.splitContainerControl1.Panel1.Controls.Add(this.panelControl1);
            this.splitContainerControl1.Panel1.Text = "splitContainerControl1_Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.panelControl2);
            this.splitContainerControl1.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainerControl1.Panel2.Text = "splitContainerControl1_Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(905, 382);
            this.splitContainerControl1.SplitterPosition = 322;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // srcImageList
            // 
            this.srcImageList.AutoDeselectItems = true;
            this.srcImageList.AutoDisposeImages = true;
            this.srcImageList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.srcImageList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.srcImageList.DoubleBuffer = true;
            this.srcImageList.EnableKeyboard = true;
            this.srcImageList.EnableRubberBandSelection = true;
            this.srcImageList.ItemBackColor = System.Drawing.SystemColors.Control;
            this.srcImageList.ItemBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.srcImageList.ItemForeColor = System.Drawing.SystemColors.ControlText;
            this.srcImageList.ItemImageBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.srcImageList.ItemImageSize = new System.Drawing.Size(96, 96);
            this.srcImageList.ItemSelectedBackColor = System.Drawing.SystemColors.Highlight;
            this.srcImageList.ItemSelectedForeColor = System.Drawing.SystemColors.HighlightText;
            this.srcImageList.ItemSize = new System.Drawing.Size(112, 122);
            this.srcImageList.ItemSpacingSize = new System.Drawing.Size(0, 0);
            this.srcImageList.Location = new System.Drawing.Point(0, 26);
            this.srcImageList.Name = "srcImageList";
            this.srcImageList.ScrollStyle = Leadtools.WinForms.RasterImageListScrollStyle.Vertical;
            this.srcImageList.SelectionMode = Leadtools.WinForms.RasterImageListSelectionMode.Single;
            this.srcImageList.SelectUserImage = null;
            this.srcImageList.ShowItemText = true;
            this.srcImageList.Size = new System.Drawing.Size(318, 352);
            this.srcImageList.Sorting = System.Windows.Forms.SortOrder.None;
            this.srcImageList.TabIndex = 1;
            this.srcImageList.Text = "rasterImageList1";
            this.srcImageList.TopIndex = 0;
            this.srcImageList.ViewStyle = Leadtools.WinForms.RasterImageListViewStyle.Explorer;
            this.srcImageList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.srcImageList_MouseDown);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnAdd);
            this.panelControl1.Controls.Add(this.cboZoom);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(318, 26);
            this.panelControl1.TabIndex = 0;
            this.panelControl1.Text = "panelControl1";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(224, 2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(89, 24);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // cboZoom
            // 
            this.cboZoom.FormattingEnabled = true;
            this.cboZoom.Items.AddRange(new object[] {
            "30%",
            "40%",
            "50%",
            "60%",
            "70%",
            "80%",
            "90%",
            "100%"});
            this.cboZoom.Location = new System.Drawing.Point(5, 3);
            this.cboZoom.Name = "cboZoom";
            this.cboZoom.Size = new System.Drawing.Size(132, 21);
            this.cboZoom.TabIndex = 0;
            this.cboZoom.SelectedIndexChanged += new System.EventHandler(this.cboZoom_SelectedIndexChanged);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.panelControl3);
            this.panelControl2.Controls.Add(this.xtraTabControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 25);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(573, 353);
            this.panelControl2.TabIndex = 2;
            this.panelControl2.Text = "panelControl2";
            // 
            // panelControl3
            // 
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl3.Location = new System.Drawing.Point(2, 319);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(569, 32);
            this.panelControl3.TabIndex = 1;
            this.panelControl3.Text = "panelControl3";
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Cursor = System.Windows.Forms.Cursors.Default;
            this.xtraTabControl1.Location = new System.Drawing.Point(27, 24);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.spiltImageTab;
            this.xtraTabControl1.Size = new System.Drawing.Size(510, 221);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.spiltImageTab});
            this.xtraTabControl1.Text = "xtraTabControl1";
            // 
            // spiltImageTab
            // 
            this.spiltImageTab.Controls.Add(this.mainPanel);
            this.spiltImageTab.Name = "spiltImageTab";
            this.spiltImageTab.Size = new System.Drawing.Size(501, 191);
            this.spiltImageTab.Text = "Untitled";
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.destImageList);
            this.mainPanel.Controls.Add(this.buttonPanel);
            this.mainPanel.Location = new System.Drawing.Point(12, 10);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(449, 180);
            this.mainPanel.TabIndex = 0;
            this.mainPanel.Text = "panelControl4";
            // 
            // destImageList
            // 
            this.destImageList.AllowDrop = true;
            this.destImageList.AutoDeselectItems = true;
            this.destImageList.AutoDisposeImages = true;
            this.destImageList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.destImageList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.destImageList.DoubleBuffer = true;
            this.destImageList.EnableKeyboard = true;
            this.destImageList.EnableRubberBandSelection = true;
            this.destImageList.ItemBackColor = System.Drawing.SystemColors.Control;
            this.destImageList.ItemBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.destImageList.ItemForeColor = System.Drawing.SystemColors.ControlText;
            this.destImageList.ItemImageBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.destImageList.ItemImageSize = new System.Drawing.Size(96, 96);
            this.destImageList.ItemSelectedBackColor = System.Drawing.SystemColors.Highlight;
            this.destImageList.ItemSelectedForeColor = System.Drawing.SystemColors.HighlightText;
            this.destImageList.ItemSize = new System.Drawing.Size(112, 122);
            this.destImageList.ItemSpacingSize = new System.Drawing.Size(0, 0);
            this.destImageList.Location = new System.Drawing.Point(2, 2);
            this.destImageList.Name = "destImageList";
            this.destImageList.ScrollStyle = Leadtools.WinForms.RasterImageListScrollStyle.Vertical;
            this.destImageList.SelectionMode = Leadtools.WinForms.RasterImageListSelectionMode.Single;
            this.destImageList.SelectUserImage = null;
            this.destImageList.ShowItemText = true;
            this.destImageList.Size = new System.Drawing.Size(445, 135);
            this.destImageList.Sorting = System.Windows.Forms.SortOrder.None;
            this.destImageList.TabIndex = 1;
            this.destImageList.Text = "rasterImageList1";
            this.destImageList.TopIndex = 0;
            this.destImageList.ViewStyle = Leadtools.WinForms.RasterImageListViewStyle.Explorer;
            this.destImageList.DragDrop += new System.Windows.Forms.DragEventHandler(this.destImageList_DragDrop);
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.Delete);
            this.buttonPanel.Controls.Add(this.saveButton);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPanel.Location = new System.Drawing.Point(2, 137);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(445, 41);
            this.buttonPanel.TabIndex = 0;
            this.buttonPanel.Text = "panelControl4";
            // 
            // Delete
            // 
            this.Delete.Location = new System.Drawing.Point(175, 6);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(111, 30);
            this.Delete.TabIndex = 1;
            this.Delete.Text = "simpleButton1";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(304, 6);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(113, 30);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBox1,
            this.toolStripAdd});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(573, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox1.Items.AddRange(new object[] {
            "30%",
            "40%",
            "50%",
            "60%",
            "70%",
            "80%",
            "90%",
            "100%"});
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripAdd
            // 
            this.toolStripAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripAdd.Image")));
            this.toolStripAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAdd.Name = "toolStripAdd";
            this.toolStripAdd.Size = new System.Drawing.Size(54, 22);
            this.toolStripAdd.Text = "Add New";
            // 
            // TifSpliter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 382);
            this.Controls.Add(this.splitContainerControl1);
            this.Name = "TifSpliter";
            this.Text = "Fax Image ";
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.spiltImageTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainPanel)).EndInit();
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.buttonPanel)).EndInit();
            this.buttonPanel.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.ComboBox cboZoom;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage spiltImageTab;
        private Leadtools.WinForms.RasterImageList srcImageList;
        private System.Windows.Forms.ToolStripButton toolStripAdd;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private DevExpress.XtraEditors.PanelControl mainPanel;
        private Leadtools.WinForms.RasterImageList destImageList;
        private DevExpress.XtraEditors.PanelControl buttonPanel;
        private DevExpress.XtraEditors.SimpleButton Delete;
        private DevExpress.XtraEditors.SimpleButton saveButton;



    }
}