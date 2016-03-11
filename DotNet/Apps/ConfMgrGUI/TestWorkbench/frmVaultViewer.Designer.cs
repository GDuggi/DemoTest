namespace TestWorkbench
{
    partial class frmVaultViewer
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
            this.dmgrVaultViewer = new DevExpress.XtraBars.Docking.DockManager();
            this.dpVaultViewer = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.xtraTabControl = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPageDoc = new DevExpress.XtraTab.XtraTabPage();
            this.richEditControl = new DevExpress.XtraRichEdit.RichEditControl();
            this.xtraTabPagePdf = new DevExpress.XtraTab.XtraTabPage();
            this.pdfViewer = new DevExpress.XtraPdfViewer.PdfViewer();
            this.xtraTabPageTif = new DevExpress.XtraTab.XtraTabPage();
            this.pictureEdit = new DevExpress.XtraEditors.PictureEdit();
            this.grpCtrlDocList = new DevExpress.XtraEditors.GroupControl();
            this.listBoxDocuments = new DevExpress.XtraEditors.ListBoxControl();
            this.toolTipController = new DevExpress.Utils.ToolTipController();
            ((System.ComponentModel.ISupportInitialize)(this.dmgrVaultViewer)).BeginInit();
            this.dpVaultViewer.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl)).BeginInit();
            this.xtraTabControl.SuspendLayout();
            this.xtraTabPageDoc.SuspendLayout();
            this.xtraTabPagePdf.SuspendLayout();
            this.xtraTabPageTif.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpCtrlDocList)).BeginInit();
            this.grpCtrlDocList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listBoxDocuments)).BeginInit();
            this.SuspendLayout();
            // 
            // dmgrVaultViewer
            // 
            this.dmgrVaultViewer.Form = this;
            this.dmgrVaultViewer.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dpVaultViewer});
            this.dmgrVaultViewer.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane"});
            // 
            // dpVaultViewer
            // 
            this.dpVaultViewer.Controls.Add(this.dockPanel1_Container);
            this.dpVaultViewer.Dock = DevExpress.XtraBars.Docking.DockingStyle.Top;
            this.dpVaultViewer.FloatVertical = true;
            this.dpVaultViewer.ID = new System.Guid("a580930b-4198-479c-a554-9e856ceda3fa");
            this.dpVaultViewer.Location = new System.Drawing.Point(0, 0);
            this.dpVaultViewer.Name = "dpVaultViewer";
            this.dpVaultViewer.OriginalSize = new System.Drawing.Size(200, 528);
            this.dpVaultViewer.Size = new System.Drawing.Size(802, 528);
            this.dpVaultViewer.Text = "Vault Viewer";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.xtraTabControl);
            this.dockPanel1_Container.Controls.Add(this.grpCtrlDocList);
            this.dockPanel1_Container.Location = new System.Drawing.Point(2, 24);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(798, 502);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // xtraTabControl
            // 
            this.xtraTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl.Location = new System.Drawing.Point(145, 0);
            this.xtraTabControl.Name = "xtraTabControl";
            this.xtraTabControl.SelectedTabPage = this.xtraTabPageDoc;
            this.xtraTabControl.Size = new System.Drawing.Size(653, 502);
            this.xtraTabControl.TabIndex = 1;
            this.xtraTabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPageDoc,
            this.xtraTabPagePdf,
            this.xtraTabPageTif});
            // 
            // xtraTabPageDoc
            // 
            this.xtraTabPageDoc.Controls.Add(this.richEditControl);
            this.xtraTabPageDoc.Name = "xtraTabPageDoc";
            this.xtraTabPageDoc.Size = new System.Drawing.Size(651, 479);
            this.xtraTabPageDoc.Text = "Doc";
            // 
            // richEditControl
            // 
            this.richEditControl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.richEditControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richEditControl.EnableToolTips = true;
            this.richEditControl.Location = new System.Drawing.Point(0, 0);
            this.richEditControl.Margin = new System.Windows.Forms.Padding(0);
            this.richEditControl.Name = "richEditControl";
            this.richEditControl.Options.Bookmarks.AllowNameResolution = false;
            this.richEditControl.Options.HorizontalRuler.Visibility = DevExpress.XtraRichEdit.RichEditRulerVisibility.Hidden;
            this.richEditControl.Options.VerticalRuler.Visibility = DevExpress.XtraRichEdit.RichEditRulerVisibility.Hidden;
            this.richEditControl.ReadOnly = true;
            this.richEditControl.Size = new System.Drawing.Size(651, 479);
            this.richEditControl.TabIndex = 1;
            // 
            // xtraTabPagePdf
            // 
            this.xtraTabPagePdf.Controls.Add(this.pdfViewer);
            this.xtraTabPagePdf.Name = "xtraTabPagePdf";
            this.xtraTabPagePdf.Size = new System.Drawing.Size(651, 479);
            this.xtraTabPagePdf.Text = "Pdf";
            // 
            // pdfViewer
            // 
            this.pdfViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pdfViewer.HandTool = true;
            this.pdfViewer.Location = new System.Drawing.Point(0, 0);
            this.pdfViewer.Name = "pdfViewer";
            this.pdfViewer.Size = new System.Drawing.Size(651, 479);
            this.pdfViewer.TabIndex = 4;
            // 
            // xtraTabPageTif
            // 
            this.xtraTabPageTif.Controls.Add(this.pictureEdit);
            this.xtraTabPageTif.Name = "xtraTabPageTif";
            this.xtraTabPageTif.Size = new System.Drawing.Size(651, 479);
            this.xtraTabPageTif.Text = "Tif";
            // 
            // pictureEdit
            // 
            this.pictureEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureEdit.Location = new System.Drawing.Point(0, 0);
            this.pictureEdit.Name = "pictureEdit";
            this.pictureEdit.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit.Properties.ShowScrollBars = true;
            this.pictureEdit.Size = new System.Drawing.Size(651, 479);
            this.pictureEdit.TabIndex = 5;
            // 
            // grpCtrlDocList
            // 
            this.grpCtrlDocList.Controls.Add(this.listBoxDocuments);
            this.grpCtrlDocList.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpCtrlDocList.Location = new System.Drawing.Point(0, 0);
            this.grpCtrlDocList.Name = "grpCtrlDocList";
            this.grpCtrlDocList.Size = new System.Drawing.Size(145, 502);
            this.grpCtrlDocList.TabIndex = 0;
            this.grpCtrlDocList.Text = "Documents";
            // 
            // listBoxDocuments
            // 
            this.listBoxDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxDocuments.Location = new System.Drawing.Point(2, 22);
            this.listBoxDocuments.Name = "listBoxDocuments";
            this.listBoxDocuments.Size = new System.Drawing.Size(141, 478);
            this.listBoxDocuments.TabIndex = 0;
            this.listBoxDocuments.Click += new System.EventHandler(this.listBoxDocuments_Click);
            this.listBoxDocuments.DoubleClick += new System.EventHandler(this.listBoxDocuments_DoubleClick);
            this.listBoxDocuments.MouseLeave += new System.EventHandler(this.listBoxDocuments_MouseLeave);
            this.listBoxDocuments.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listBoxDocuments_MouseMove);
            // 
            // frmVaultViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 583);
            this.Controls.Add(this.dpVaultViewer);
            this.IsMdiContainer = true;
            this.Name = "frmVaultViewer";
            this.Text = "Test Vault Viewer";
            this.Load += new System.EventHandler(this.frmVaultViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dmgrVaultViewer)).EndInit();
            this.dpVaultViewer.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl)).EndInit();
            this.xtraTabControl.ResumeLayout(false);
            this.xtraTabPageDoc.ResumeLayout(false);
            this.xtraTabPagePdf.ResumeLayout(false);
            this.xtraTabPageTif.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpCtrlDocList)).EndInit();
            this.grpCtrlDocList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listBoxDocuments)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Docking.DockManager dmgrVaultViewer;
        private DevExpress.XtraBars.Docking.DockPanel dpVaultViewer;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraEditors.GroupControl grpCtrlDocList;
        private DevExpress.XtraEditors.ListBoxControl listBoxDocuments;
        private DevExpress.Utils.ToolTipController toolTipController;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageDoc;
        private DevExpress.XtraTab.XtraTabPage xtraTabPagePdf;
        private DevExpress.XtraTab.XtraTabPage xtraTabPageTif;
        public DevExpress.XtraRichEdit.RichEditControl richEditControl;
        public DevExpress.XtraPdfViewer.PdfViewer pdfViewer;
        public DevExpress.XtraEditors.PictureEdit pictureEdit;

    }
}