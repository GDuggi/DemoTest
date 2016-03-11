namespace ConfirmManager
{
   partial class frmEditContract
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditContract));
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.barStaticTemplateFieldName = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticFiller = new DevExpress.XtraBars.BarStaticItem();
            this.bbtnEditFax = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticTemplateName = new DevExpress.XtraBars.BarStaticItem();
            this.barBtnOpenClauseViewer = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnPasteClauses = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.pmenuEditContract = new DevExpress.XtraBars.PopupMenu();
            this.pnlContractEditor = new DevExpress.XtraEditors.PanelControl();
            this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
            this.btnContractOkAndSend = new DevExpress.XtraEditors.SimpleButton();
            this.btnCptyInfo = new DevExpress.XtraEditors.SimpleButton();
            this.dbtnClauses = new DevExpress.XtraEditors.DropDownButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cedDisplayCmts = new DevExpress.XtraEditors.CheckEdit();
            this.memoEditCmt = new DevExpress.XtraEditors.MemoEdit();
            this.btnEditContractCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnEditContractSave = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.comboStatus = new DevExpress.XtraEditors.ComboBoxEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.lblEconData = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lblAgreements = new DevExpress.XtraEditors.LabelControl();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.barStaticItem4 = new DevExpress.XtraBars.BarStaticItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.printDialog = new System.Windows.Forms.PrintDialog();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pmenuEditContract)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlContractEditor)).BeginInit();
            this.pnlContractEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cedDisplayCmts.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditCmt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboStatus.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.Categories.AddRange(new DevExpress.XtraBars.BarManagerCategory[] {
            new DevExpress.XtraBars.BarManagerCategory("StatusBar", new System.Guid("96cd86ce-2c01-40e0-9b22-1b5bcafa37ca")),
            new DevExpress.XtraBars.BarManagerCategory("PopupMenu", new System.Guid("923f5b53-fab1-4227-9252-645563c71d96"))});
            this.ribbonControl1.Dock = System.Windows.Forms.DockStyle.None;
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.barStaticTemplateFieldName,
            this.barStaticFiller,
            this.bbtnEditFax,
            this.barStaticTemplateName,
            this.barBtnOpenClauseViewer,
            this.barBtnPasteClauses});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 34;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Size = new System.Drawing.Size(725, 49);
            this.ribbonControl1.StatusBar = this.ribbonStatusBar;
            this.ribbonControl1.Visible = false;
            // 
            // barStaticTemplateFieldName
            // 
            this.barStaticTemplateFieldName.Caption = "Template:";
            this.barStaticTemplateFieldName.CategoryGuid = new System.Guid("96cd86ce-2c01-40e0-9b22-1b5bcafa37ca");
            this.barStaticTemplateFieldName.Id = 1;
            this.barStaticTemplateFieldName.Name = "barStaticTemplateFieldName";
            this.barStaticTemplateFieldName.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barStaticFiller
            // 
            this.barStaticFiller.CategoryGuid = new System.Guid("96cd86ce-2c01-40e0-9b22-1b5bcafa37ca");
            this.barStaticFiller.Id = 2;
            this.barStaticFiller.Name = "barStaticFiller";
            this.barStaticFiller.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // bbtnEditFax
            // 
            this.bbtnEditFax.Caption = "Confirm will be sent to...";
            this.bbtnEditFax.CategoryGuid = new System.Guid("96cd86ce-2c01-40e0-9b22-1b5bcafa37ca");
            this.bbtnEditFax.Id = 3;
            this.bbtnEditFax.Name = "bbtnEditFax";
            this.bbtnEditFax.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bbtnEditFax_ItemClick);
            // 
            // barStaticTemplateName
            // 
            this.barStaticTemplateName.Caption = "xyz...";
            this.barStaticTemplateName.CategoryGuid = new System.Guid("96cd86ce-2c01-40e0-9b22-1b5bcafa37ca");
            this.barStaticTemplateName.Id = 4;
            this.barStaticTemplateName.Name = "barStaticTemplateName";
            this.barStaticTemplateName.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barBtnOpenClauseViewer
            // 
            this.barBtnOpenClauseViewer.Caption = "Open Clause Viewer";
            this.barBtnOpenClauseViewer.CategoryGuid = new System.Guid("923f5b53-fab1-4227-9252-645563c71d96");
            this.barBtnOpenClauseViewer.Id = 28;
            this.barBtnOpenClauseViewer.Name = "barBtnOpenClauseViewer";
            this.barBtnOpenClauseViewer.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnOpenClauseViewer_ItemClick);
            // 
            // barBtnPasteClauses
            // 
            this.barBtnPasteClauses.Caption = "Paste Clauses";
            this.barBtnPasteClauses.CategoryGuid = new System.Guid("923f5b53-fab1-4227-9252-645563c71d96");
            this.barBtnPasteClauses.Id = 32;
            this.barBtnPasteClauses.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V));
            this.barBtnPasteClauses.Name = "barBtnPasteClauses";
            this.barBtnPasteClauses.ShortcutKeyDisplayString = "Ctrl+V";
            this.barBtnPasteClauses.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnPasteClauses_ItemClick);
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.ItemLinks.Add(this.barStaticTemplateFieldName);
            this.ribbonStatusBar.ItemLinks.Add(this.barStaticTemplateName);
            this.ribbonStatusBar.ItemLinks.Add(this.barStaticFiller);
            this.ribbonStatusBar.ItemLinks.Add(this.bbtnEditFax, true);
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 462);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbonControl1;
            this.ribbonStatusBar.Size = new System.Drawing.Size(793, 23);
            // 
            // pmenuEditContract
            // 
            this.pmenuEditContract.ItemLinks.Add(this.barBtnOpenClauseViewer);
            this.pmenuEditContract.ItemLinks.Add(this.barBtnPasteClauses);
            this.pmenuEditContract.MenuCaption = "Clauses";
            this.pmenuEditContract.Name = "pmenuEditContract";
            this.pmenuEditContract.Ribbon = this.ribbonControl1;
            // 
            // pnlContractEditor
            // 
            this.pnlContractEditor.Controls.Add(this.btnPrint);
            this.pnlContractEditor.Controls.Add(this.btnContractOkAndSend);
            this.pnlContractEditor.Controls.Add(this.btnCptyInfo);
            this.pnlContractEditor.Controls.Add(this.dbtnClauses);
            this.pnlContractEditor.Controls.Add(this.labelControl2);
            this.pnlContractEditor.Controls.Add(this.cedDisplayCmts);
            this.pnlContractEditor.Controls.Add(this.memoEditCmt);
            this.pnlContractEditor.Controls.Add(this.btnEditContractCancel);
            this.pnlContractEditor.Controls.Add(this.btnEditContractSave);
            this.pnlContractEditor.Controls.Add(this.labelControl1);
            this.pnlContractEditor.Controls.Add(this.comboStatus);
            this.pnlContractEditor.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlContractEditor.Location = new System.Drawing.Point(0, 0);
            this.pnlContractEditor.Name = "pnlContractEditor";
            this.pnlContractEditor.Size = new System.Drawing.Size(793, 91);
            this.pnlContractEditor.TabIndex = 5;
            // 
            // btnPrint
            // 
            this.btnPrint.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnPrint.Enabled = false;
            this.btnPrint.Location = new System.Drawing.Point(497, 8);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(65, 23);
            this.btnPrint.TabIndex = 14;
            this.btnPrint.Text = "Print";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnContractOkAndSend
            // 
            this.btnContractOkAndSend.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnContractOkAndSend.Location = new System.Drawing.Point(568, 8);
            this.btnContractOkAndSend.Name = "btnContractOkAndSend";
            this.btnContractOkAndSend.Size = new System.Drawing.Size(105, 23);
            this.btnContractOkAndSend.TabIndex = 13;
            this.btnContractOkAndSend.Text = "OkToSend && Send";
            this.btnContractOkAndSend.Click += new System.EventHandler(this.btnApproveAndSend_Click);
            // 
            // btnCptyInfo
            // 
            this.btnCptyInfo.Location = new System.Drawing.Point(426, 8);
            this.btnCptyInfo.Name = "btnCptyInfo";
            this.btnCptyInfo.Size = new System.Drawing.Size(65, 23);
            this.btnCptyInfo.TabIndex = 12;
            this.btnCptyInfo.Text = "Cpty Info";
            this.btnCptyInfo.Visible = false;
            this.btnCptyInfo.Click += new System.EventHandler(this.btnCptyInfo_Click);
            // 
            // dbtnClauses
            // 
            this.dbtnClauses.DropDownControl = this.pmenuEditContract;
            this.dbtnClauses.Enabled = false;
            this.dbtnClauses.Location = new System.Drawing.Point(682, 8);
            this.dbtnClauses.Name = "dbtnClauses";
            this.dbtnClauses.Size = new System.Drawing.Size(65, 23);
            this.dbtnClauses.TabIndex = 11;
            this.dbtnClauses.Text = "Clauses";
            this.dbtnClauses.Visible = false;
            this.dbtnClauses.Click += new System.EventHandler(this.dbtnClauses_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(5, 45);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(54, 13);
            this.labelControl2.TabIndex = 7;
            this.labelControl2.Text = "Comments:";
            // 
            // cedDisplayCmts
            // 
            this.cedDisplayCmts.EditValue = true;
            this.cedDisplayCmts.Location = new System.Drawing.Point(312, 12);
            this.cedDisplayCmts.Name = "cedDisplayCmts";
            this.cedDisplayCmts.Properties.Caption = "Show Comments:";
            this.cedDisplayCmts.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.cedDisplayCmts.Size = new System.Drawing.Size(104, 19);
            this.cedDisplayCmts.TabIndex = 8;
            this.cedDisplayCmts.CheckedChanged += new System.EventHandler(this.cedDisplayCmts_CheckedChanged);
            // 
            // memoEditCmt
            // 
            this.memoEditCmt.Location = new System.Drawing.Point(66, 43);
            this.memoEditCmt.Name = "memoEditCmt";
            this.memoEditCmt.Properties.MaxLength = 255;
            this.memoEditCmt.Size = new System.Drawing.Size(681, 35);
            this.memoEditCmt.TabIndex = 6;
            // 
            // btnEditContractCancel
            // 
            this.btnEditContractCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnEditContractCancel.Location = new System.Drawing.Point(239, 8);
            this.btnEditContractCancel.Name = "btnEditContractCancel";
            this.btnEditContractCancel.Size = new System.Drawing.Size(65, 23);
            this.btnEditContractCancel.TabIndex = 3;
            this.btnEditContractCancel.Text = "Cancel";
            // 
            // btnEditContractSave
            // 
            this.btnEditContractSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnEditContractSave.Location = new System.Drawing.Point(167, 8);
            this.btnEditContractSave.Name = "btnEditContractSave";
            this.btnEditContractSave.Size = new System.Drawing.Size(65, 23);
            this.btnEditContractSave.TabIndex = 2;
            this.btnEditContractSave.Text = "Save";
            this.btnEditContractSave.Click += new System.EventHandler(this.btnEditContractSave_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(8, 13);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(35, 13);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "Status:";
            // 
            // comboStatus
            // 
            this.comboStatus.EditValue = "";
            this.comboStatus.Location = new System.Drawing.Point(66, 10);
            this.comboStatus.Name = "comboStatus";
            this.comboStatus.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboStatus.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.comboStatus.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.comboStatus.Size = new System.Drawing.Size(94, 20);
            this.comboStatus.TabIndex = 0;
            this.comboStatus.DrawItem += new DevExpress.XtraEditors.ListBoxDrawItemEventHandler(this.comboStatus_DrawItem);
            this.comboStatus.EditValueChanged += new System.EventHandler(this.comboStatus_EditValueChanged);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.lblEconData);
            this.panelControl2.Controls.Add(this.labelControl3);
            this.panelControl2.Controls.Add(this.lblAgreements);
            this.panelControl2.Controls.Add(this.labelControl11);
            this.panelControl2.Controls.Add(this.labelControl7);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 416);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(793, 46);
            this.panelControl2.TabIndex = 6;
            // 
            // lblEconData
            // 
            this.lblEconData.Location = new System.Drawing.Point(99, 24);
            this.lblEconData.Name = "lblEconData";
            this.lblEconData.Size = new System.Drawing.Size(70, 13);
            this.lblEconData.TabIndex = 11;
            this.lblEconData.Text = "Economic Data";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl3.Location = new System.Drawing.Point(6, 24);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(86, 13);
            this.labelControl3.TabIndex = 10;
            this.labelControl3.Text = "Economic Data:";
            // 
            // lblAgreements
            // 
            this.lblAgreements.Location = new System.Drawing.Point(99, 6);
            this.lblAgreements.Name = "lblAgreements";
            this.lblAgreements.Size = new System.Drawing.Size(70, 13);
            this.lblAgreements.TabIndex = 9;
            this.lblAgreements.Text = "Agreements...";
            // 
            // labelControl11
            // 
            this.labelControl11.Location = new System.Drawing.Point(86, 65);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(0, 13);
            this.labelControl11.TabIndex = 8;
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.labelControl7.Location = new System.Drawing.Point(6, 6);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(73, 13);
            this.labelControl7.TabIndex = 4;
            this.labelControl7.Text = "Agreements:";
            // 
            // barStaticItem4
            // 
            this.barStaticItem4.Caption = "Status: NEW";
            this.barStaticItem4.Id = 0;
            this.barStaticItem4.Name = "barStaticItem4";
            this.barStaticItem4.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 412);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(793, 4);
            this.panelControl1.TabIndex = 11;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(86, 65);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(0, 13);
            this.labelControl4.TabIndex = 8;
            // 
            // printDocument
            // 
            this.printDocument.BeginPrint += new System.Drawing.Printing.PrintEventHandler(this.printDocument_BeginPrint);
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument_PrintPage);
            // 
            // printDialog
            // 
            this.printDialog.Document = this.printDocument;
            this.printDialog.UseEXDialog = true;
            // 
            // frmEditContract
            // 
            this.AcceptButton = this.btnEditContractSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnEditContractCancel;
            this.ClientSize = new System.Drawing.Size(793, 485);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.pnlContractEditor);
            this.Controls.Add(this.ribbonControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "frmEditContract";
            this.ShowInTaskbar = false;
            this.Text = "Edit Contract: 88136820";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEditContract_FormClosing);
            this.Load += new System.EventHandler(this.frmEditContract_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pmenuEditContract)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlContractEditor)).EndInit();
            this.pnlContractEditor.ResumeLayout(false);
            this.pnlContractEditor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cedDisplayCmts.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditCmt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboStatus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
      private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
      private DevExpress.XtraEditors.PanelControl pnlContractEditor;
      private DevExpress.XtraEditors.SimpleButton btnEditContractCancel;
      private DevExpress.XtraEditors.SimpleButton btnEditContractSave;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      private DevExpress.XtraEditors.LabelControl labelControl2;
      private DevExpress.XtraBars.BarStaticItem barStaticTemplateFieldName;
      private DevExpress.XtraEditors.PanelControl panelControl2;
      private DevExpress.XtraBars.BarStaticItem barStaticFiller;
      private DevExpress.XtraBars.BarButtonItem bbtnEditFax;
      private DevExpress.XtraEditors.LabelControl labelControl7;
      private DevExpress.XtraEditors.LabelControl labelControl11;
      private DevExpress.XtraBars.BarStaticItem barStaticItem4;
      public DevExpress.XtraBars.BarStaticItem barStaticTemplateName;
      public DevExpress.XtraEditors.LabelControl lblAgreements;
      private DevExpress.XtraEditors.CheckEdit cedDisplayCmts;
      private DevExpress.XtraBars.PopupMenu pmenuEditContract;
      private DevExpress.XtraEditors.DropDownButton dbtnClauses;
      private DevExpress.XtraEditors.SimpleButton btnCptyInfo;
      private DevExpress.XtraBars.BarButtonItem barBtnOpenClauseViewer;
      private DevExpress.XtraBars.BarButtonItem barBtnPasteClauses;
      public ConfirmManager.RichTextBoxPrintCtrl rtbEditContract;
      //public System.Windows.Forms.RichTextBox rtbEditContract;
      public DevExpress.XtraEditors.MemoEdit memoEditCmt;
      public DevExpress.XtraEditors.ComboBoxEdit comboStatus;
      private DevExpress.XtraEditors.SimpleButton btnContractOkAndSend;
      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraEditors.LabelControl labelControl4;
      public System.Windows.Forms.PrintDialog printDialog;
      public DevExpress.XtraEditors.SimpleButton btnPrint;
      public System.Drawing.Printing.PrintDocument printDocument;
      public DevExpress.XtraEditors.LabelControl lblEconData;
      private DevExpress.XtraEditors.LabelControl labelControl3;


   }
}