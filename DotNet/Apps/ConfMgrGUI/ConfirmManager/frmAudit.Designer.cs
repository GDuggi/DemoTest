namespace ConfirmManager
{
   partial class frmAudit
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
          System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAudit));
          this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
          this.btnClose = new DevExpress.XtraEditors.SimpleButton();
          this.btnExcel = new DevExpress.XtraEditors.SimpleButton();
          this.gridAudit = new DevExpress.XtraGrid.GridControl();
          this.gridViewAudit = new DevExpress.XtraGrid.Views.Grid.GridView();
          this.colRqmt = new DevExpress.XtraGrid.Columns.GridColumn();
          this.colUserId = new DevExpress.XtraGrid.Columns.GridColumn();
          this.colTimestamp = new DevExpress.XtraGrid.Columns.GridColumn();
          this.colOperation = new DevExpress.XtraGrid.Columns.GridColumn();
          this.colMachineName = new DevExpress.XtraGrid.Columns.GridColumn();
          this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
          this.colCompletedDt = new DevExpress.XtraGrid.Columns.GridColumn();
          this.ColTradeId = new DevExpress.XtraGrid.Columns.GridColumn();
          this.colTradeRqmtId = new DevExpress.XtraGrid.Columns.GridColumn();
          this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
          this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
          this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
          ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
          this.panelControl1.SuspendLayout();
          ((System.ComponentModel.ISupportInitialize)(this.gridAudit)).BeginInit();
          ((System.ComponentModel.ISupportInitialize)(this.gridViewAudit)).BeginInit();
          ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
          this.SuspendLayout();
          // 
          // panelControl1
          // 
          this.panelControl1.Controls.Add(this.btnClose);
          this.panelControl1.Controls.Add(this.btnExcel);
          this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
          this.panelControl1.Location = new System.Drawing.Point(0, 0);
          this.panelControl1.Name = "panelControl1";
          this.panelControl1.Size = new System.Drawing.Size(597, 40);
          this.panelControl1.TabIndex = 0;
          // 
          // btnClose
          // 
          this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
          this.btnClose.Location = new System.Drawing.Point(507, 6);
          this.btnClose.Name = "btnClose";
          this.btnClose.Size = new System.Drawing.Size(78, 28);
          this.btnClose.TabIndex = 1;
          this.btnClose.Text = "&Close";
          this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
          // 
          // btnExcel
          // 
          this.btnExcel.Location = new System.Drawing.Point(12, 6);
          this.btnExcel.Name = "btnExcel";
          this.btnExcel.Size = new System.Drawing.Size(84, 28);
          this.btnExcel.TabIndex = 0;
          this.btnExcel.Text = "Export to &Excel";
          this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
          // 
          // gridAudit
          // 
          this.gridAudit.Dock = System.Windows.Forms.DockStyle.Fill;
          this.gridAudit.Location = new System.Drawing.Point(0, 40);
          this.gridAudit.MainView = this.gridViewAudit;
          this.gridAudit.Name = "gridAudit";
          this.gridAudit.Size = new System.Drawing.Size(597, 221);
          this.gridAudit.TabIndex = 1;
          this.gridAudit.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewAudit});
          // 
          // gridViewAudit
          // 
          this.gridViewAudit.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colRqmt,
            this.colUserId,
            this.colTimestamp,
            this.colOperation,
            this.colMachineName,
            this.colStatus,
            this.colCompletedDt,
            this.ColTradeId,
            this.colTradeRqmtId});
          this.gridViewAudit.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
          this.gridViewAudit.GridControl = this.gridAudit;
          this.gridViewAudit.Name = "gridViewAudit";
          this.gridViewAudit.OptionsBehavior.Editable = false;
          this.gridViewAudit.OptionsSelection.EnableAppearanceFocusedCell = false;
          this.gridViewAudit.OptionsSelection.EnableAppearanceFocusedRow = false;
          this.gridViewAudit.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
          this.gridViewAudit.OptionsSelection.UseIndicatorForSelection = false;
          this.gridViewAudit.OptionsView.ColumnAutoWidth = false;
          this.gridViewAudit.OptionsView.EnableAppearanceEvenRow = true;
          this.gridViewAudit.OptionsView.EnableAppearanceOddRow = true;
          this.gridViewAudit.OptionsView.ShowGroupPanel = false;
          // 
          // colRqmt
          // 
          this.colRqmt.Caption = "Rqmt";
          this.colRqmt.FieldName = "Rqmt";
          this.colRqmt.Name = "colRqmt";
          this.colRqmt.Visible = true;
          this.colRqmt.VisibleIndex = 0;
          this.colRqmt.Width = 96;
          // 
          // colUserId
          // 
          this.colUserId.Caption = "User Id";
          this.colUserId.FieldName = "UserId";
          this.colUserId.Name = "colUserId";
          this.colUserId.Visible = true;
          this.colUserId.VisibleIndex = 1;
          this.colUserId.Width = 88;
          // 
          // colTimestamp
          // 
          this.colTimestamp.Caption = "Timestamp";
          this.colTimestamp.DisplayFormat.FormatString = "dd-MMM-yyyy hh:mm tt";
          this.colTimestamp.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
          this.colTimestamp.FieldName = "Timestamp";
          this.colTimestamp.Name = "colTimestamp";
          this.colTimestamp.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
          this.colTimestamp.Visible = true;
          this.colTimestamp.VisibleIndex = 2;
          this.colTimestamp.Width = 92;
          // 
          // colOperation
          // 
          this.colOperation.Caption = "Operation";
          this.colOperation.FieldName = "Operation";
          this.colOperation.Name = "colOperation";
          this.colOperation.Visible = true;
          this.colOperation.VisibleIndex = 3;
          this.colOperation.Width = 86;
          // 
          // colMachineName
          // 
          this.colMachineName.Caption = "Machine Name";
          this.colMachineName.FieldName = "MachineName";
          this.colMachineName.Name = "colMachineName";
          this.colMachineName.Visible = true;
          this.colMachineName.VisibleIndex = 4;
          this.colMachineName.Width = 106;
          // 
          // colStatus
          // 
          this.colStatus.Caption = "Status";
          this.colStatus.FieldName = "Status";
          this.colStatus.Name = "colStatus";
          this.colStatus.Visible = true;
          this.colStatus.VisibleIndex = 5;
          // 
          // colCompletedDt
          // 
          this.colCompletedDt.Caption = "Completed Dt";
          this.colCompletedDt.DisplayFormat.FormatString = "dd-MMM-yyyy";
          this.colCompletedDt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
          this.colCompletedDt.FieldName = "CompletedDt";
          this.colCompletedDt.Name = "colCompletedDt";
          this.colCompletedDt.Visible = true;
          this.colCompletedDt.VisibleIndex = 6;
          this.colCompletedDt.Width = 129;
          // 
          // ColTradeId
          // 
          this.ColTradeId.Caption = "Trade Id";
          this.ColTradeId.FieldName = "TradeId";
          this.ColTradeId.Name = "ColTradeId";
          // 
          // colTradeRqmtId
          // 
          this.colTradeRqmtId.Caption = "Trade Rqmt Id";
          this.colTradeRqmtId.FieldName = "TradeRqmtId";
          this.colTradeRqmtId.Name = "colTradeRqmtId";
          // 
          // ribbonControl1
          // 
          this.ribbonControl1.Dock = System.Windows.Forms.DockStyle.None;
          this.ribbonControl1.ExpandCollapseItem.Id = 0;
          this.ribbonControl1.ExpandCollapseItem.Name = "";
          this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem});
          this.ribbonControl1.Location = new System.Drawing.Point(315, 9);
          this.ribbonControl1.MaxItemId = 1;
          this.ribbonControl1.Name = "ribbonControl1";
          this.ribbonControl1.Size = new System.Drawing.Size(19, 22);
          this.ribbonControl1.StatusBar = this.ribbonStatusBar1;
          // 
          // ribbonStatusBar1
          // 
          this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 261);
          this.ribbonStatusBar1.Name = "ribbonStatusBar1";
          this.ribbonStatusBar1.Ribbon = this.ribbonControl1;
          this.ribbonStatusBar1.Size = new System.Drawing.Size(597, 27);
          // 
          // frmAudit
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(597, 288);
          this.Controls.Add(this.gridAudit);
          this.Controls.Add(this.panelControl1);
          this.Controls.Add(this.ribbonControl1);
          this.Controls.Add(this.ribbonStatusBar1);
          this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
          this.MinimizeBox = false;
          this.Name = "frmAudit";
          this.ShowInTaskbar = false;
          this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
          this.Text = "Audit";
          this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAudit_FormClosing);
          this.Load += new System.EventHandler(this.frmAudit_Load);
          ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
          this.panelControl1.ResumeLayout(false);
          ((System.ComponentModel.ISupportInitialize)(this.gridAudit)).EndInit();
          ((System.ComponentModel.ISupportInitialize)(this.gridViewAudit)).EndInit();
          ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
          this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraGrid.Views.Grid.GridView gridViewAudit;
      private DevExpress.XtraEditors.SimpleButton btnClose;
      private DevExpress.XtraEditors.SimpleButton btnExcel;
      private DevExpress.XtraGrid.Columns.GridColumn colRqmt;
      private DevExpress.XtraGrid.Columns.GridColumn colUserId;
      private DevExpress.XtraGrid.Columns.GridColumn colTimestamp;
      private DevExpress.XtraGrid.Columns.GridColumn colOperation;
      private DevExpress.XtraGrid.Columns.GridColumn colMachineName;
      private DevExpress.XtraGrid.Columns.GridColumn colStatus;
      private DevExpress.XtraGrid.Columns.GridColumn colCompletedDt;
      private DevExpress.XtraGrid.Columns.GridColumn ColTradeId;
      private DevExpress.XtraGrid.Columns.GridColumn colTradeRqmtId;
      public DevExpress.XtraGrid.GridControl gridAudit;
      private System.Windows.Forms.SaveFileDialog saveFileDialog;
      private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
      private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;



   }
}