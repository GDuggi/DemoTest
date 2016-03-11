namespace ConfirmManager
{
   partial class frmUserPrefs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserPrefs));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnAddRqmtOk = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancelCreateCancel = new DevExpress.XtraEditors.SimpleButton();
            this.cklbxSeCptySn = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.lblSeCptySn = new DevExpress.XtraEditors.LabelControl();
            this.tabctrlUserPref = new DevExpress.XtraTab.XtraTabControl();
            this.tabpgDataLoadSettings = new DevExpress.XtraTab.XtraTabPage();
            this.btnCdtyGrpUncheckAll = new DevExpress.XtraEditors.SimpleButton();
            this.btnCdtyGrpAll = new DevExpress.XtraEditors.SimpleButton();
            this.btnSeCptyUncheckAll = new DevExpress.XtraEditors.SimpleButton();
            this.btnSeCptyAll = new DevExpress.XtraEditors.SimpleButton();
            this.cklbxCdtyGrp = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.lblCdty = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.tabpgPrefs = new DevExpress.XtraTab.XtraTabPage();
            this.cedAutoDispDealsheet = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cklbxSeCptySn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabctrlUserPref)).BeginInit();
            this.tabctrlUserPref.SuspendLayout();
            this.tabpgDataLoadSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cklbxCdtyGrp)).BeginInit();
            this.tabpgPrefs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cedAutoDispDealsheet.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnAddRqmtOk);
            this.panelControl1.Controls.Add(this.btnCancelCreateCancel);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 312);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(300, 39);
            this.panelControl1.TabIndex = 22;
            // 
            // btnAddRqmtOk
            // 
            this.btnAddRqmtOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAddRqmtOk.Location = new System.Drawing.Point(64, 8);
            this.btnAddRqmtOk.Name = "btnAddRqmtOk";
            this.btnAddRqmtOk.Size = new System.Drawing.Size(75, 23);
            this.btnAddRqmtOk.TabIndex = 1;
            this.btnAddRqmtOk.Text = "&OK";
            // 
            // btnCancelCreateCancel
            // 
            this.btnCancelCreateCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelCreateCancel.Location = new System.Drawing.Point(153, 8);
            this.btnCancelCreateCancel.Name = "btnCancelCreateCancel";
            this.btnCancelCreateCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancelCreateCancel.TabIndex = 3;
            this.btnCancelCreateCancel.Text = "&Cancel";
            this.btnCancelCreateCancel.Click += new System.EventHandler(this.btnCancelCreateCancel_Click);
            // 
            // cklbxSeCptySn
            // 
            this.cklbxSeCptySn.CheckOnClick = true;
            this.cklbxSeCptySn.DisplayMember = "CptySn";
            this.cklbxSeCptySn.Location = new System.Drawing.Point(14, 54);
            this.cklbxSeCptySn.Name = "cklbxSeCptySn";
            this.cklbxSeCptySn.Size = new System.Drawing.Size(117, 193);
            this.cklbxSeCptySn.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.cklbxSeCptySn.TabIndex = 23;
            this.cklbxSeCptySn.ValueMember = "CptySn";
            this.cklbxSeCptySn.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.cklbxSeCptySn_ItemCheck);
            // 
            // lblSeCptySn
            // 
            this.lblSeCptySn.Location = new System.Drawing.Point(17, 38);
            this.lblSeCptySn.Name = "lblSeCptySn";
            this.lblSeCptySn.Size = new System.Drawing.Size(66, 13);
            this.lblSeCptySn.TabIndex = 24;
            this.lblSeCptySn.Text = "Our Company";
            // 
            // tabctrlUserPref
            // 
            this.tabctrlUserPref.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabctrlUserPref.Location = new System.Drawing.Point(0, 0);
            this.tabctrlUserPref.Name = "tabctrlUserPref";
            this.tabctrlUserPref.SelectedTabPage = this.tabpgDataLoadSettings;
            this.tabctrlUserPref.Size = new System.Drawing.Size(300, 312);
            this.tabctrlUserPref.TabIndex = 26;
            this.tabctrlUserPref.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabpgDataLoadSettings,
            this.tabpgPrefs});
            // 
            // tabpgDataLoadSettings
            // 
            this.tabpgDataLoadSettings.Controls.Add(this.btnCdtyGrpUncheckAll);
            this.tabpgDataLoadSettings.Controls.Add(this.btnCdtyGrpAll);
            this.tabpgDataLoadSettings.Controls.Add(this.btnSeCptyUncheckAll);
            this.tabpgDataLoadSettings.Controls.Add(this.btnSeCptyAll);
            this.tabpgDataLoadSettings.Controls.Add(this.cklbxCdtyGrp);
            this.tabpgDataLoadSettings.Controls.Add(this.lblCdty);
            this.tabpgDataLoadSettings.Controls.Add(this.labelControl2);
            this.tabpgDataLoadSettings.Controls.Add(this.cklbxSeCptySn);
            this.tabpgDataLoadSettings.Controls.Add(this.lblSeCptySn);
            this.tabpgDataLoadSettings.Name = "tabpgDataLoadSettings";
            this.tabpgDataLoadSettings.Size = new System.Drawing.Size(298, 289);
            this.tabpgDataLoadSettings.Text = "DataLoad";
            // 
            // btnCdtyGrpUncheckAll
            // 
            this.btnCdtyGrpUncheckAll.Location = new System.Drawing.Point(213, 253);
            this.btnCdtyGrpUncheckAll.Name = "btnCdtyGrpUncheckAll";
            this.btnCdtyGrpUncheckAll.Size = new System.Drawing.Size(61, 23);
            this.btnCdtyGrpUncheckAll.TabIndex = 31;
            this.btnCdtyGrpUncheckAll.Tag = "3";
            this.btnCdtyGrpUncheckAll.Text = "UncheckAll";
            this.btnCdtyGrpUncheckAll.Click += new System.EventHandler(this.CheckUncheckAll_Click);
            // 
            // btnCdtyGrpAll
            // 
            this.btnCdtyGrpAll.Location = new System.Drawing.Point(157, 253);
            this.btnCdtyGrpAll.Name = "btnCdtyGrpAll";
            this.btnCdtyGrpAll.Size = new System.Drawing.Size(52, 23);
            this.btnCdtyGrpAll.TabIndex = 30;
            this.btnCdtyGrpAll.Tag = "2";
            this.btnCdtyGrpAll.Text = "Check All";
            this.btnCdtyGrpAll.Click += new System.EventHandler(this.CheckUncheckAll_Click);
            // 
            // btnSeCptyUncheckAll
            // 
            this.btnSeCptyUncheckAll.Location = new System.Drawing.Point(70, 253);
            this.btnSeCptyUncheckAll.Name = "btnSeCptyUncheckAll";
            this.btnSeCptyUncheckAll.Size = new System.Drawing.Size(61, 23);
            this.btnSeCptyUncheckAll.TabIndex = 29;
            this.btnSeCptyUncheckAll.Tag = "1";
            this.btnSeCptyUncheckAll.Text = "UncheckAll";
            this.btnSeCptyUncheckAll.Click += new System.EventHandler(this.CheckUncheckAll_Click);
            // 
            // btnSeCptyAll
            // 
            this.btnSeCptyAll.Location = new System.Drawing.Point(14, 253);
            this.btnSeCptyAll.Name = "btnSeCptyAll";
            this.btnSeCptyAll.Size = new System.Drawing.Size(52, 23);
            this.btnSeCptyAll.TabIndex = 28;
            this.btnSeCptyAll.Tag = "0";
            this.btnSeCptyAll.Text = "Check All";
            this.btnSeCptyAll.Click += new System.EventHandler(this.CheckUncheckAll_Click);
            // 
            // cklbxCdtyGrp
            // 
            this.cklbxCdtyGrp.CheckOnClick = true;
            this.cklbxCdtyGrp.DisplayMember = "CdtyCode";
            this.cklbxCdtyGrp.Location = new System.Drawing.Point(157, 54);
            this.cklbxCdtyGrp.Name = "cklbxCdtyGrp";
            this.cklbxCdtyGrp.Size = new System.Drawing.Size(117, 193);
            this.cklbxCdtyGrp.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.cklbxCdtyGrp.TabIndex = 26;
            this.cklbxCdtyGrp.ValueMember = "CdtyCode";
            this.cklbxCdtyGrp.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.cklbxCdty_ItemCheck);
            // 
            // lblCdty
            // 
            this.lblCdty.Location = new System.Drawing.Point(160, 38);
            this.lblCdty.Name = "lblCdty";
            this.lblCdty.Size = new System.Drawing.Size(85, 13);
            this.lblCdty.TabIndex = 27;
            this.lblCdty.Text = "Commodity Group";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.labelControl2.Location = new System.Drawing.Point(9, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(276, 26);
            this.labelControl2.TabIndex = 25;
            this.labelControl2.Text = "Data will be loaded only for company/cdty selected here. If no entries are checke" +
    "d it will select all.";
            // 
            // tabpgPrefs
            // 
            this.tabpgPrefs.Controls.Add(this.cedAutoDispDealsheet);
            this.tabpgPrefs.Name = "tabpgPrefs";
            this.tabpgPrefs.Size = new System.Drawing.Size(298, 289);
            this.tabpgPrefs.Text = "Prefs";
            // 
            // cedAutoDispDealsheet
            // 
            this.cedAutoDispDealsheet.Location = new System.Drawing.Point(9, 14);
            this.cedAutoDispDealsheet.Name = "cedAutoDispDealsheet";
            this.cedAutoDispDealsheet.Properties.Caption = "Auto Display Dealsheet:";
            this.cedAutoDispDealsheet.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.cedAutoDispDealsheet.Size = new System.Drawing.Size(154, 19);
            this.cedAutoDispDealsheet.TabIndex = 0;
            // 
            // frmUserPrefs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 351);
            this.ControlBox = false;
            this.Controls.Add(this.tabctrlUserPref);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUserPrefs";
            this.ShowInTaskbar = false;
            this.Text = "Preferences";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUserPrefs_FormClosing);
            this.Load += new System.EventHandler(this.frmUserPrefs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cklbxSeCptySn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabctrlUserPref)).EndInit();
            this.tabctrlUserPref.ResumeLayout(false);
            this.tabpgDataLoadSettings.ResumeLayout(false);
            this.tabpgDataLoadSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cklbxCdtyGrp)).EndInit();
            this.tabpgPrefs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cedAutoDispDealsheet.Properties)).EndInit();
            this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraEditors.SimpleButton btnAddRqmtOk;
      private DevExpress.XtraEditors.SimpleButton btnCancelCreateCancel;
      public DevExpress.XtraEditors.CheckedListBoxControl cklbxSeCptySn;
      private DevExpress.XtraTab.XtraTabControl tabctrlUserPref;
      private DevExpress.XtraTab.XtraTabPage tabpgDataLoadSettings;
      private DevExpress.XtraEditors.LabelControl labelControl2;
      public DevExpress.XtraEditors.CheckedListBoxControl cklbxCdtyGrp;
      private DevExpress.XtraEditors.LabelControl lblCdty;
      private DevExpress.XtraEditors.SimpleButton btnSeCptyUncheckAll;
      private DevExpress.XtraEditors.SimpleButton btnSeCptyAll;
      private DevExpress.XtraEditors.SimpleButton btnCdtyGrpUncheckAll;
      private DevExpress.XtraEditors.SimpleButton btnCdtyGrpAll;
      public DevExpress.XtraEditors.LabelControl lblSeCptySn;
      private DevExpress.XtraTab.XtraTabPage tabpgPrefs;
      public DevExpress.XtraEditors.CheckEdit cedAutoDispDealsheet;
   }
}