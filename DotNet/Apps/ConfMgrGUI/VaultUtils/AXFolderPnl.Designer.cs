namespace VaultUtils
{
    partial class AXFolderPnl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AXFolderPnl));
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.barBtnExecuteQry = new DevExpress.XtraBars.BarButtonItem();
            this.barChkViewAllVersions = new DevExpress.XtraBars.BarEditItem();
            this.reposChkEditAllVersions = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.barBtnXmit = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnPrint = new DevExpress.XtraBars.BarButtonItem();
            this.rbnCntrlAxFolderPnal = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.imageSmall = new System.Windows.Forms.ImageList();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribnPageGrpAxFields = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            ((System.ComponentModel.ISupportInitialize)(this.reposChkEditAllVersions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbnCntrlAxFolderPnal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ribbonStatusBar1.ItemLinks.Add(this.barBtnExecuteQry);
            this.ribbonStatusBar1.ItemLinks.Add(this.barChkViewAllVersions, true);
            this.ribbonStatusBar1.ItemLinks.Add(this.barBtnXmit, true);
            this.ribbonStatusBar1.ItemLinks.Add(this.barBtnPrint);
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 0);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.rbnCntrlAxFolderPnal;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(1156, 23);
            // 
            // barBtnExecuteQry
            // 
            this.barBtnExecuteQry.Caption = "Get Documents";
            this.barBtnExecuteQry.Id = 1;
            this.barBtnExecuteQry.ImageIndex = 5;
            this.barBtnExecuteQry.Name = "barBtnExecuteQry";
            this.barBtnExecuteQry.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnExecuteQry_ItemClick);
            // 
            // barChkViewAllVersions
            // 
            this.barChkViewAllVersions.Caption = "View All Versions";
            this.barChkViewAllVersions.Edit = this.reposChkEditAllVersions;
            this.barChkViewAllVersions.Id = 0;
            this.barChkViewAllVersions.Name = "barChkViewAllVersions";
            this.barChkViewAllVersions.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barChkViewAllVersions_ItemClick);
            // 
            // reposChkEditAllVersions
            // 
            this.reposChkEditAllVersions.AutoHeight = false;
            this.reposChkEditAllVersions.AutoWidth = true;
            this.reposChkEditAllVersions.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.reposChkEditAllVersions.Caption = "Check";
            this.reposChkEditAllVersions.Name = "reposChkEditAllVersions";
            this.reposChkEditAllVersions.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            this.reposChkEditAllVersions.NullText = "False";
            this.reposChkEditAllVersions.ValueGrayed = false;
            // 
            // barBtnXmit
            // 
            this.barBtnXmit.Caption = "Transmit Document";
            this.barBtnXmit.Id = 2;
            this.barBtnXmit.ImageIndex = 12;
            this.barBtnXmit.Name = "barBtnXmit";
            this.barBtnXmit.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barBtnXmit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnXmit_ItemClick);
            // 
            // barBtnPrint
            // 
            this.barBtnPrint.Caption = "Print Document";
            this.barBtnPrint.Id = 3;
            this.barBtnPrint.ImageIndex = 19;
            this.barBtnPrint.Name = "barBtnPrint";
            this.barBtnPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnPrint_ItemClick);
            // 
            // rbnCntrlAxFolderPnal
            // 
            this.rbnCntrlAxFolderPnal.ExpandCollapseItem.Id = 0;
            this.rbnCntrlAxFolderPnal.Images = this.imageSmall;
            this.rbnCntrlAxFolderPnal.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.rbnCntrlAxFolderPnal.ExpandCollapseItem,
            this.barChkViewAllVersions,
            this.barBtnExecuteQry,
            this.barBtnXmit,
            this.barBtnPrint});
            this.rbnCntrlAxFolderPnal.Location = new System.Drawing.Point(0, 23);
            this.rbnCntrlAxFolderPnal.MaxItemId = 5;
            this.rbnCntrlAxFolderPnal.Name = "rbnCntrlAxFolderPnal";
            this.rbnCntrlAxFolderPnal.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.rbnCntrlAxFolderPnal.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.reposChkEditAllVersions});
            this.rbnCntrlAxFolderPnal.Size = new System.Drawing.Size(1156, 140);
            this.rbnCntrlAxFolderPnal.StatusBar = this.ribbonStatusBar1;
            this.rbnCntrlAxFolderPnal.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Above;
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
            this.imageSmall.Images.SetKeyName(19, "printer.png");
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribnPageGrpAxFields});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Query";
            this.ribbonPage1.Visible = false;
            // 
            // ribnPageGrpAxFields
            // 
            this.ribnPageGrpAxFields.Name = "ribnPageGrpAxFields";
            this.ribnPageGrpAxFields.Text = "Query Fields";
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 163);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.Size = new System.Drawing.Size(1156, 365);
            this.xtraTabControl1.TabIndex = 2;
            // 
            // AXFolderPnl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.rbnCntrlAxFolderPnal);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Name = "AXFolderPnl";
            this.Size = new System.Drawing.Size(1156, 528);
            ((System.ComponentModel.ISupportInitialize)(this.reposChkEditAllVersions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rbnCntrlAxFolderPnal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraBars.Ribbon.RibbonControl rbnCntrlAxFolderPnal;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribnPageGrpAxFields;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraBars.BarEditItem barChkViewAllVersions;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit reposChkEditAllVersions;
        private DevExpress.XtraBars.BarButtonItem barBtnExecuteQry;
        private System.Windows.Forms.ImageList imageSmall;
        private DevExpress.XtraBars.BarButtonItem barBtnXmit;
        private DevExpress.XtraBars.BarButtonItem barBtnPrint;
    }
}
