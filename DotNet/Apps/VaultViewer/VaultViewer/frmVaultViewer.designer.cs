namespace VaultViewer
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
            this.toolTipController = new DevExpress.Utils.ToolTipController();
            this.panelControlMain = new DevExpress.XtraEditors.PanelControl();
            this.treeListDocs = new DevExpress.XtraTreeList.TreeList();
            this.colCategory = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colMetadata = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colURL = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colFileExt = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colFileName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnDocForms = new DevExpress.XtraEditors.SimpleButton();
            this.checkEditStayOnTop = new DevExpress.XtraEditors.CheckEdit();
            this.panelControlButtons = new DevExpress.XtraEditors.PanelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.comboEditTradingSys = new DevExpress.XtraEditors.ComboBoxEdit();
            this.textEditTicketNo = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnGetTradeList = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlMain)).BeginInit();
            this.panelControlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListDocs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEditStayOnTop.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlButtons)).BeginInit();
            this.panelControlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboEditTradingSys.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditTicketNo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControlMain
            // 
            this.panelControlMain.Controls.Add(this.treeListDocs);
            this.panelControlMain.Controls.Add(this.panelControl1);
            this.panelControlMain.Controls.Add(this.panelControlButtons);
            this.panelControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControlMain.Location = new System.Drawing.Point(0, 0);
            this.panelControlMain.Name = "panelControlMain";
            this.panelControlMain.Size = new System.Drawing.Size(407, 418);
            this.panelControlMain.TabIndex = 2;
            // 
            // treeListDocs
            // 
            this.treeListDocs.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colCategory,
            this.colMetadata,
            this.colURL,
            this.colFileExt,
            this.colFileName});
            this.treeListDocs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeListDocs.HorzScrollVisibility = DevExpress.XtraTreeList.ScrollVisibility.Always;
            this.treeListDocs.Location = new System.Drawing.Point(3, 56);
            this.treeListDocs.Name = "treeListDocs";
            this.treeListDocs.OptionsBehavior.Editable = false;
            this.treeListDocs.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.treeListDocs.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.treeListDocs.OptionsSelection.UseIndicatorForSelection = true;
            this.treeListDocs.OptionsView.AutoWidth = false;
            this.treeListDocs.OptionsView.ShowRoot = false;
            this.treeListDocs.Size = new System.Drawing.Size(401, 321);
            this.treeListDocs.TabIndex = 16;
            this.treeListDocs.ToolTipController = this.toolTipController;
            this.treeListDocs.DoubleClick += new System.EventHandler(this.treeListDocs_DoubleClick);
            // 
            // colCategory
            // 
            this.colCategory.Caption = "Category";
            this.colCategory.FieldName = "Category";
            this.colCategory.Name = "colCategory";
            this.colCategory.OptionsColumn.AllowEdit = false;
            this.colCategory.OptionsColumn.AllowMove = false;
            this.colCategory.OptionsColumn.FixedWidth = true;
            this.colCategory.OptionsColumn.ReadOnly = true;
            this.colCategory.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.colCategory.Visible = true;
            this.colCategory.VisibleIndex = 0;
            this.colCategory.Width = 35;
            // 
            // colMetadata
            // 
            this.colMetadata.Caption = "Metadata";
            this.colMetadata.FieldName = "Metadata";
            this.colMetadata.Name = "colMetadata";
            this.colMetadata.OptionsColumn.AllowEdit = false;
            this.colMetadata.OptionsColumn.AllowFocus = false;
            this.colMetadata.OptionsColumn.AllowMove = false;
            this.colMetadata.OptionsColumn.ReadOnly = true;
            this.colMetadata.Visible = true;
            this.colMetadata.VisibleIndex = 1;
            this.colMetadata.Width = 298;
            // 
            // colURL
            // 
            this.colURL.Caption = "colURL";
            this.colURL.FieldName = "URL";
            this.colURL.Name = "colURL";
            // 
            // colFileExt
            // 
            this.colFileExt.Caption = "FileExt";
            this.colFileExt.FieldName = "FileExt";
            this.colFileExt.Name = "colFileExt";
            this.colFileExt.OptionsColumn.ReadOnly = true;
            // 
            // colFileName
            // 
            this.colFileName.Caption = "File Name";
            this.colFileName.FieldName = "FileName";
            this.colFileName.Name = "colFileName";
            this.colFileName.OptionsColumn.ReadOnly = true;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnDocForms);
            this.panelControl1.Controls.Add(this.checkEditStayOnTop);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(3, 377);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(401, 38);
            this.panelControl1.TabIndex = 18;
            // 
            // btnDocForms
            // 
            this.btnDocForms.Location = new System.Drawing.Point(10, 9);
            this.btnDocForms.Name = "btnDocForms";
            this.btnDocForms.Size = new System.Drawing.Size(100, 23);
            this.btnDocForms.TabIndex = 5;
            this.btnDocForms.Text = "Close Doc Forms";
            this.btnDocForms.Click += new System.EventHandler(this.btnDocForms_Click);
            // 
            // checkEditStayOnTop
            // 
            this.checkEditStayOnTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkEditStayOnTop.Location = new System.Drawing.Point(304, 11);
            this.checkEditStayOnTop.Name = "checkEditStayOnTop";
            this.checkEditStayOnTop.Properties.Caption = "Stay on Top";
            this.checkEditStayOnTop.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkEditStayOnTop.Size = new System.Drawing.Size(87, 19);
            this.checkEditStayOnTop.TabIndex = 2;
            this.checkEditStayOnTop.CheckedChanged += new System.EventHandler(this.checkEditStayOnTop_CheckedChanged);
            // 
            // panelControlButtons
            // 
            this.panelControlButtons.Controls.Add(this.labelControl2);
            this.panelControlButtons.Controls.Add(this.comboEditTradingSys);
            this.panelControlButtons.Controls.Add(this.textEditTicketNo);
            this.panelControlButtons.Controls.Add(this.labelControl1);
            this.panelControlButtons.Controls.Add(this.btnGetTradeList);
            this.panelControlButtons.Controls.Add(this.btnClose);
            this.panelControlButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControlButtons.Location = new System.Drawing.Point(3, 3);
            this.panelControlButtons.Name = "panelControlButtons";
            this.panelControlButtons.Size = new System.Drawing.Size(401, 53);
            this.panelControlButtons.TabIndex = 0;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 6);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(88, 13);
            this.labelControl2.TabIndex = 7;
            this.labelControl2.Text = "Trading Sys Code:";
            // 
            // comboEditTradingSys
            // 
            this.comboEditTradingSys.Location = new System.Drawing.Point(12, 24);
            this.comboEditTradingSys.Name = "comboEditTradingSys";
            this.comboEditTradingSys.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboEditTradingSys.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.comboEditTradingSys.Size = new System.Drawing.Size(100, 20);
            this.comboEditTradingSys.TabIndex = 6;
            // 
            // textEditTicketNo
            // 
            this.textEditTicketNo.Location = new System.Drawing.Point(126, 24);
            this.textEditTicketNo.Name = "textEditTicketNo";
            this.textEditTicketNo.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textEditTicketNo.Size = new System.Drawing.Size(100, 20);
            this.textEditTicketNo.TabIndex = 5;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(126, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 13);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "Ticket No:";
            // 
            // btnGetTradeList
            // 
            this.btnGetTradeList.Location = new System.Drawing.Point(236, 22);
            this.btnGetTradeList.Name = "btnGetTradeList";
            this.btnGetTradeList.Size = new System.Drawing.Size(72, 23);
            this.btnGetTradeList.TabIndex = 2;
            this.btnGetTradeList.Text = "Get Doc List";
            this.btnGetTradeList.Click += new System.EventHandler(this.btnGetTradeList_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(349, 21);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(45, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Exit";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmVaultViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 418);
            this.Controls.Add(this.panelControlMain);
            this.IsMdiContainer = true;
            this.Name = "frmVaultViewer";
            this.Text = "Vault Viewer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmVaultViewer_FormClosed);
            this.Load += new System.EventHandler(this.frmVaultViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlMain)).EndInit();
            this.panelControlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeListDocs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkEditStayOnTop.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlButtons)).EndInit();
            this.panelControlButtons.ResumeLayout(false);
            this.panelControlButtons.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboEditTradingSys.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditTicketNo.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.ToolTipController toolTipController;
        private DevExpress.XtraEditors.PanelControl panelControlMain;
        private DevExpress.XtraEditors.PanelControl panelControlButtons;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.SimpleButton btnGetTradeList;
        private DevExpress.XtraEditors.SimpleButton btnDocForms;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        public DevExpress.XtraTreeList.TreeList treeListDocs;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colCategory;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colMetadata;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colFileExt;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colFileName;
        private DevExpress.XtraEditors.CheckEdit checkEditStayOnTop;
        private DevExpress.XtraEditors.TextEdit textEditTicketNo;
        private DevExpress.XtraEditors.ComboBoxEdit comboEditTradingSys;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colURL;

    }
}