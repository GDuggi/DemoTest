namespace VaultUtils
{
    partial class AXPnl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AXPnl));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.xtraTabControlAxFolders = new DevExpress.XtraTab.XtraTabControl();
            this.pnlCntrlToolBar = new DevExpress.XtraEditors.PanelControl();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.imageSmall = new System.Windows.Forms.ImageList();
            this.barItemDefaultTradeID = new DevExpress.XtraBars.BarEditItem();
            this.reposTxtEditDefaultTradeID = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barBtnGetAllDocuments = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControlAxFolders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlCntrlToolBar)).BeginInit();
            this.pnlCntrlToolBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reposTxtEditDefaultTradeID)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.xtraTabControlAxFolders);
            this.panelControl1.Controls.Add(this.pnlCntrlToolBar);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1223, 569);
            this.panelControl1.TabIndex = 0;
            // 
            // xtraTabControlAxFolders
            // 
            this.xtraTabControlAxFolders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControlAxFolders.Location = new System.Drawing.Point(3, 32);
            this.xtraTabControlAxFolders.Name = "xtraTabControlAxFolders";
            this.xtraTabControlAxFolders.Size = new System.Drawing.Size(1217, 534);
            this.xtraTabControlAxFolders.TabIndex = 0;
            // 
            // pnlCntrlToolBar
            // 
            this.pnlCntrlToolBar.Controls.Add(this.ribbonControl1);
            this.pnlCntrlToolBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCntrlToolBar.Location = new System.Drawing.Point(3, 3);
            this.pnlCntrlToolBar.Name = "pnlCntrlToolBar";
            this.pnlCntrlToolBar.Size = new System.Drawing.Size(1217, 29);
            this.pnlCntrlToolBar.TabIndex = 1;
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Images = this.imageSmall;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.barItemDefaultTradeID,
            this.barBtnGetAllDocuments});
            this.ribbonControl1.Location = new System.Drawing.Point(3, 3);
            this.ribbonControl1.MaxItemId = 4;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.reposTxtEditDefaultTradeID});
            this.ribbonControl1.Size = new System.Drawing.Size(1211, 49);
            this.ribbonControl1.Toolbar.ItemLinks.Add(this.barItemDefaultTradeID);
            this.ribbonControl1.Toolbar.ItemLinks.Add(this.barBtnGetAllDocuments, true);
            // 
            // imageSmall
            // 
            this.imageSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageSmall.ImageStream")));
            this.imageSmall.TransparentColor = System.Drawing.Color.Transparent;
            this.imageSmall.Images.SetKeyName(0, "accept.png");
            this.imageSmall.Images.SetKeyName(1, "exclamation.png");
            this.imageSmall.Images.SetKeyName(2, "table_delete.png");
            this.imageSmall.Images.SetKeyName(3, "table_add.png");
            this.imageSmall.Images.SetKeyName(4, "lightning_go.png");
            this.imageSmall.Images.SetKeyName(5, "lightning.png");
            this.imageSmall.Images.SetKeyName(6, "lightning_add.png");
            this.imageSmall.Images.SetKeyName(7, "lightning_delete.png");
            this.imageSmall.Images.SetKeyName(8, "database_add.png");
            this.imageSmall.Images.SetKeyName(9, "eye.png");
            this.imageSmall.Images.SetKeyName(10, "find.png");
            this.imageSmall.Images.SetKeyName(11, "cancel.png");
            this.imageSmall.Images.SetKeyName(12, "transmit.png");
            this.imageSmall.Images.SetKeyName(13, "user_edit.png");
            this.imageSmall.Images.SetKeyName(14, "feed_go.png");
            this.imageSmall.Images.SetKeyName(15, "lightbulb_off.png");
            this.imageSmall.Images.SetKeyName(16, "lightbulb.png");
            this.imageSmall.Images.SetKeyName(17, "table_row_insert.png");
            this.imageSmall.Images.SetKeyName(18, "table_row_delete.png");
            // 
            // barItemDefaultTradeID
            // 
            this.barItemDefaultTradeID.Caption = "Trade Id";
            this.barItemDefaultTradeID.Edit = this.reposTxtEditDefaultTradeID;
            this.barItemDefaultTradeID.Id = 0;
            this.barItemDefaultTradeID.Name = "barItemDefaultTradeID";
            this.barItemDefaultTradeID.Width = 120;
            // 
            // reposTxtEditDefaultTradeID
            // 
            this.reposTxtEditDefaultTradeID.AutoHeight = false;
            this.reposTxtEditDefaultTradeID.Name = "reposTxtEditDefaultTradeID";
            // 
            // barBtnGetAllDocuments
            // 
            this.barBtnGetAllDocuments.Caption = "Get All Documents";
            this.barBtnGetAllDocuments.Id = 3;
            this.barBtnGetAllDocuments.ImageIndex = 6;
            this.barBtnGetAllDocuments.Name = "barBtnGetAllDocuments";
            this.barBtnGetAllDocuments.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnGetAllDocuments_ItemClick);
            // 
            // AXPnl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl1);
            this.Name = "AXPnl";
            this.Size = new System.Drawing.Size(1223, 569);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControlAxFolders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlCntrlToolBar)).EndInit();
            this.pnlCntrlToolBar.ResumeLayout(false);
            this.pnlCntrlToolBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reposTxtEditDefaultTradeID)).EndInit();
            this.ResumeLayout(false);

}

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControlAxFolders;
        private DevExpress.XtraEditors.PanelControl pnlCntrlToolBar;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private System.Windows.Forms.ImageList imageSmall;
        private DevExpress.XtraBars.BarEditItem barItemDefaultTradeID;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit reposTxtEditDefaultTradeID;
        private DevExpress.XtraBars.BarButtonItem barBtnGetAllDocuments;


    }
}
