namespace ConfirmManager
{
   partial class frmXmitResultStatusLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmXmitResultStatusLog));
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnExcel = new DevExpress.XtraEditors.SimpleButton();
            this.gridXmitResult = new DevExpress.XtraGrid.GridControl();
            this.gridViewXmitResult = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colXmitResultId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colXmitRequestId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTradeRqmtConfirmId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAssociatedDocsId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSentByUser = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colXmitStatusInd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colXmitMethodInd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colXmitDest = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colXmitCmt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colXmitTimestamp = new DevExpress.XtraGrid.Columns.GridColumn();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridXmitResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewXmitResult)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 273);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.ribbonControl1;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(585, 23);
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.Dock = System.Windows.Forms.DockStyle.None;
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem});
            this.ribbonControl1.Location = new System.Drawing.Point(230, 2);
            this.ribbonControl1.MaxItemId = 1;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Size = new System.Drawing.Size(56, 49);
            this.ribbonControl1.StatusBar = this.ribbonStatusBar1;
            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Above;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnClose);
            this.panelControl1.Controls.Add(this.btnExcel);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(585, 40);
            this.panelControl1.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(495, 6);
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
            this.btnExcel.Visible = false;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // gridXmitResult
            // 
            this.gridXmitResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridXmitResult.Location = new System.Drawing.Point(0, 40);
            this.gridXmitResult.MainView = this.gridViewXmitResult;
            this.gridXmitResult.Name = "gridXmitResult";
            this.gridXmitResult.Size = new System.Drawing.Size(585, 233);
            this.gridXmitResult.TabIndex = 4;
            this.gridXmitResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewXmitResult});
            // 
            // gridViewXmitResult
            // 
            this.gridViewXmitResult.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colXmitResultId,
            this.colXmitRequestId,
            this.colTradeRqmtConfirmId,
            this.colAssociatedDocsId,
            this.colSentByUser,
            this.colXmitStatusInd,
            this.colXmitMethodInd,
            this.colXmitDest,
            this.colXmitCmt,
            this.colXmitTimestamp});
            this.gridViewXmitResult.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridViewXmitResult.GridControl = this.gridXmitResult;
            this.gridViewXmitResult.Name = "gridViewXmitResult";
            this.gridViewXmitResult.OptionsBehavior.Editable = false;
            this.gridViewXmitResult.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewXmitResult.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gridViewXmitResult.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.gridViewXmitResult.OptionsSelection.UseIndicatorForSelection = false;
            this.gridViewXmitResult.OptionsView.ColumnAutoWidth = false;
            this.gridViewXmitResult.OptionsView.EnableAppearanceEvenRow = true;
            this.gridViewXmitResult.OptionsView.EnableAppearanceOddRow = true;
            this.gridViewXmitResult.OptionsView.ShowGroupPanel = false;
            this.gridViewXmitResult.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridViewXmitResult_CustomDrawCell);
            // 
            // colXmitResultId
            // 
            this.colXmitResultId.Caption = "Id";
            this.colXmitResultId.FieldName = "XmitResultId";
            this.colXmitResultId.MinWidth = 10;
            this.colXmitResultId.Name = "colXmitResultId";
            this.colXmitResultId.Visible = true;
            this.colXmitResultId.VisibleIndex = 0;
            this.colXmitResultId.Width = 38;
            // 
            // colXmitRequestId
            // 
            this.colXmitRequestId.Caption = "Request Id";
            this.colXmitRequestId.FieldName = "XmitRequestId";
            this.colXmitRequestId.Name = "colXmitRequestId";
            this.colXmitRequestId.Visible = true;
            this.colXmitRequestId.VisibleIndex = 1;
            this.colXmitRequestId.Width = 63;
            // 
            // colTradeRqmtConfirmId
            // 
            this.colTradeRqmtConfirmId.Caption = "Trade Rqmt Id";
            this.colTradeRqmtConfirmId.FieldName = "TradeRqmtConfirmId";
            this.colTradeRqmtConfirmId.Name = "colTradeRqmtConfirmId";
            this.colTradeRqmtConfirmId.Visible = true;
            this.colTradeRqmtConfirmId.VisibleIndex = 2;
            // 
            // colAssociatedDocsId
            // 
            this.colAssociatedDocsId.Caption = "Assoc Docs Id";
            this.colAssociatedDocsId.FieldName = "AssociatedDocsId";
            this.colAssociatedDocsId.Name = "colAssociatedDocsId";
            this.colAssociatedDocsId.Visible = true;
            this.colAssociatedDocsId.VisibleIndex = 3;
            this.colAssociatedDocsId.Width = 81;
            // 
            // colSentByUser
            // 
            this.colSentByUser.Caption = "Sent By";
            this.colSentByUser.FieldName = "SentByUser";
            this.colSentByUser.Name = "colSentByUser";
            this.colSentByUser.Visible = true;
            this.colSentByUser.VisibleIndex = 4;
            this.colSentByUser.Width = 114;
            // 
            // colXmitStatusInd
            // 
            this.colXmitStatusInd.Caption = "Status";
            this.colXmitStatusInd.FieldName = "XmitStatusInd";
            this.colXmitStatusInd.Name = "colXmitStatusInd";
            this.colXmitStatusInd.Visible = true;
            this.colXmitStatusInd.VisibleIndex = 5;
            this.colXmitStatusInd.Width = 53;
            // 
            // colXmitMethodInd
            // 
            this.colXmitMethodInd.Caption = "Trans Method";
            this.colXmitMethodInd.FieldName = "XmitMethodInd";
            this.colXmitMethodInd.Name = "colXmitMethodInd";
            this.colXmitMethodInd.Visible = true;
            this.colXmitMethodInd.VisibleIndex = 6;
            this.colXmitMethodInd.Width = 83;
            // 
            // colXmitDest
            // 
            this.colXmitDest.Caption = "Destination";
            this.colXmitDest.FieldName = "XmitDest";
            this.colXmitDest.Name = "colXmitDest";
            this.colXmitDest.Visible = true;
            this.colXmitDest.VisibleIndex = 7;
            this.colXmitDest.Width = 208;
            // 
            // colXmitCmt
            // 
            this.colXmitCmt.Caption = "Comment";
            this.colXmitCmt.FieldName = "XmitCmt";
            this.colXmitCmt.Name = "colXmitCmt";
            this.colXmitCmt.Visible = true;
            this.colXmitCmt.VisibleIndex = 8;
            this.colXmitCmt.Width = 121;
            // 
            // colXmitTimestamp
            // 
            this.colXmitTimestamp.Caption = "Timestamp";
            this.colXmitTimestamp.DisplayFormat.FormatString = "dd-MMM-yyyy hh:mm tt";
            this.colXmitTimestamp.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.colXmitTimestamp.FieldName = "XmitTimestamp";
            this.colXmitTimestamp.Name = "colXmitTimestamp";
            this.colXmitTimestamp.SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
            this.colXmitTimestamp.Visible = true;
            this.colXmitTimestamp.VisibleIndex = 9;
            this.colXmitTimestamp.Width = 121;
            // 
            // frmXmitResultStatusLog
            // 
            this.ClientSize = new System.Drawing.Size(585, 296);
            this.Controls.Add(this.gridXmitResult);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Controls.Add(this.ribbonControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmXmitResultStatusLog";
            this.ShowInTaskbar = false;
            this.Text = "Transmission Status Log";
            this.Activated += new System.EventHandler(this.frmXmitResultStatusLog_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmXmitResultStatusLog_FormClosing);
            this.Load += new System.EventHandler(this.frmXmitResultStatusLog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridXmitResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewXmitResult)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
      private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraEditors.SimpleButton btnClose;
      private DevExpress.XtraEditors.SimpleButton btnExcel;
      public DevExpress.XtraGrid.GridControl gridXmitResult;
      private DevExpress.XtraGrid.Views.Grid.GridView gridViewXmitResult;
      private DevExpress.XtraGrid.Columns.GridColumn colXmitResultId;
      private DevExpress.XtraGrid.Columns.GridColumn colAssociatedDocsId;
      private DevExpress.XtraGrid.Columns.GridColumn colXmitTimestamp;
      private DevExpress.XtraGrid.Columns.GridColumn colXmitStatusInd;
      private DevExpress.XtraGrid.Columns.GridColumn colXmitMethodInd;
      private DevExpress.XtraGrid.Columns.GridColumn colSentByUser;
      private DevExpress.XtraGrid.Columns.GridColumn colXmitRequestId;
      private DevExpress.XtraGrid.Columns.GridColumn colTradeRqmtConfirmId;
      private DevExpress.XtraGrid.Columns.GridColumn colXmitCmt;
      private System.Windows.Forms.SaveFileDialog saveFileDialog;
      private DevExpress.XtraGrid.Columns.GridColumn colXmitDest;
   }
}