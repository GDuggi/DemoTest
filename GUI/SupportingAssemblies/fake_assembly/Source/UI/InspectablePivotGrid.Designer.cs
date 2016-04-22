using System;
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;

namespace NSRiskMgrCtrls {
    public partial class InspectablePivotGrid : XtraUserControl {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.lowerControlPanel = new System.Windows.Forms.Panel();
            this.renameEditor = new DevExpress.XtraEditors.TextEdit();
            this.btnExpand = new System.Windows.Forms.Button();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.scTabAndSelector = new System.Windows.Forms.SplitContainer();
            this.gbPivotFields = new System.Windows.Forms.GroupBox();
            this.amphoraFieldSelector1 = new NSRiskMgrCtrls.AmphoraFieldSelector();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.scPivotAndLower = new DevExpress.XtraEditors.SplitContainerControl();
            this.DetailsXtraTabControl = new DevExpress.XtraTab.XtraTabControl();
            this.PnLTabPage = new DevExpress.XtraTab.XtraTabPage();
            this.scPnlGrids = new DevExpress.XtraEditors.SplitContainerControl();
            this.PnLGroupControl = new DevExpress.XtraEditors.GroupControl();
            this.PnLGridControl = new DevExpress.XtraGrid.GridControl();
            this.gvPnL = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.grpHistPnL = new DevExpress.XtraEditors.GroupControl();
            this.HistoricalPnLGridControl = new DevExpress.XtraGrid.GridControl();
            this.HistoricalPnLGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.DistributionsTabPage = new DevExpress.XtraTab.XtraTabPage();
            this.distributionsGridControl = new DevExpress.XtraGrid.GridControl();
            this.DistributionsGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.PositionsGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.PositionsTabPage = new DevExpress.XtraTab.XtraTabPage();
            this.PositionsGridControl = new DevExpress.XtraGrid.GridControl();


            PositionNumberColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            PositionTypeColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            CommodityColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            MarketColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            PeriodColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            PricePerUnitColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            MtmPriceColumn = new DevExpress.XtraGrid.Columns.GridColumn();

            this.InventoryBuildDrawTabPage = new DevExpress.XtraTab.XtraTabPage();
            this.InventoryBuildDrawGridControl = new DevExpress.XtraGrid.GridControl();
            this.InventoryBuildDrawGridView = new DevExpress.XtraGrid.Views.Grid.GridView();

             DistributionNumberColumn = new DevExpress.XtraGrid.Columns.GridColumn();
             TradeNumberColumn = new DevExpress.XtraGrid.Columns.GridColumn();
             OrderNumberColumn = new DevExpress.XtraGrid.Columns.GridColumn();
             ItemNumberColumn = new DevExpress.XtraGrid.Columns.GridColumn();
             RiskQuantityColumn = new DevExpress.XtraGrid.Columns.GridColumn();
             PhysicalQuantityColumn = new DevExpress.XtraGrid.Columns.GridColumn();
             DiscountQuantityColumn = new DevExpress.XtraGrid.Columns.GridColumn();

             TradeCaptureColumn =new DevExpress.XtraGrid.Columns.GridColumn();
             LogisticsColumn = new DevExpress.XtraGrid.Columns.GridColumn();
             DealViewerColumn =  new DevExpress.XtraGrid.Columns.GridColumn();

             InventoryNumberColumn = new DevExpress.XtraGrid.Columns.GridColumn();
             InventoryTypeColumn = new DevExpress.XtraGrid.Columns.GridColumn();
             InventoryBuildDrawQtyColumn = new DevExpress.XtraGrid.Columns.GridColumn();
             InventoryBuildDrawCostUomCodeColumn = new DevExpress.XtraGrid.Columns.GridColumn();
             InventoryAllocationNumberColumn = new DevExpress.XtraGrid.Columns.GridColumn();
             InventoryAllocationItemNumberColumn = new DevExpress.XtraGrid.Columns.GridColumn();
             InventoryTradeNumberColumn = new DevExpress.XtraGrid.Columns.GridColumn();
             InventoryOrderNumberColumn = new DevExpress.XtraGrid.Columns.GridColumn();
             InventoryItemNumberColumn = new DevExpress.XtraGrid.Columns.GridColumn();

             TradeCaptureButton = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
             LogisticsButton = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
             DealViewerButton = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
                

            this.lowerControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.renameEditor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scTabAndSelector)).BeginInit();
            this.scTabAndSelector.Panel1.SuspendLayout();
            this.scTabAndSelector.Panel2.SuspendLayout();
            this.scTabAndSelector.SuspendLayout();
            this.gbPivotFields.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scPivotAndLower)).BeginInit();
            this.scPivotAndLower.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DetailsXtraTabControl)).BeginInit();
            this.DetailsXtraTabControl.SuspendLayout();
            this.PnLTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scPnlGrids)).BeginInit();
            this.scPnlGrids.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PnLGroupControl)).BeginInit();
            this.PnLGroupControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PnLGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPnL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpHistPnL)).BeginInit();
            this.grpHistPnL.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HistoricalPnLGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HistoricalPnLGridView)).BeginInit();
            this.DistributionsTabPage.SuspendLayout();
            this.PositionsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.distributionsGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DistributionsGridView)).BeginInit();

            ((System.ComponentModel.ISupportInitialize)(this.PositionsGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionsGridView)).BeginInit();
           
            this.InventoryBuildDrawTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InventoryBuildDrawGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InventoryBuildDrawGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // lowerControlPanel
            // 
            this.lowerControlPanel.Controls.Add(this.renameEditor);
            this.lowerControlPanel.Controls.Add(this.btnExpand);
            this.lowerControlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lowerControlPanel.Location = new System.Drawing.Point(2, 256);
            this.lowerControlPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lowerControlPanel.Name = "lowerControlPanel";
            this.lowerControlPanel.Size = new System.Drawing.Size(762, 22);
            this.lowerControlPanel.TabIndex = 1;
            // 
            // renameEditor
            // 
            this.renameEditor.Location = new System.Drawing.Point(168, 3);
            this.renameEditor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.renameEditor.Name = "renameEditor";
            this.renameEditor.Size = new System.Drawing.Size(86, 20);
            this.renameEditor.TabIndex = 1;
            this.renameEditor.Visible = false;
            // 
            // btnExpand
            // 
            this.btnExpand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExpand.Location = new System.Drawing.Point(677, 1);
            this.btnExpand.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(64, 19);
            this.btnExpand.TabIndex = 0;
            this.btnExpand.Text = ">>";
            this.btnExpand.UseVisualStyleBackColor = true;
            this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.AllowDrop = true;
            this.xtraTabControl1.ContextMenuStrip = this.contextMenuStrip1;
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.Size = new System.Drawing.Size(351, 254);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabControl1_SelectedPageChanged);
            this.xtraTabControl1.SelectedPageChanging += new DevExpress.XtraTab.TabPageChangingEventHandler(this.xtraTabControl1_SelectedPageChanging);
            this.xtraTabControl1.DragOver += new System.Windows.Forms.DragEventHandler(this.xtraTabControl1_DragOver);
            this.xtraTabControl1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnXtraTabControlDoubleClick);
            this.xtraTabControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.xtraTabControl1_MouseDown);
            this.xtraTabControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.xtraTabControl1_MouseMove);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxAdd,
            this.ctxRemove});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(118, 48);
            // 
            // ctxAdd
            // 
            this.ctxAdd.Name = "ctxAdd";
            this.ctxAdd.Size = new System.Drawing.Size(117, 22);
            this.ctxAdd.Text = "Add";
            this.ctxAdd.Click += new System.EventHandler(this.addTab);
            // 
            // ctxRemove
            // 
            this.ctxRemove.Name = "ctxRemove";
            this.ctxRemove.Size = new System.Drawing.Size(117, 22);
            this.ctxRemove.Text = "Remove";
            this.ctxRemove.Click += new System.EventHandler(this.removeTab);
            // 
            // scTabAndSelector
            // 
            this.scTabAndSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scTabAndSelector.Location = new System.Drawing.Point(2, 2);
            this.scTabAndSelector.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.scTabAndSelector.Name = "scTabAndSelector";
            // 
            // scTabAndSelector.Panel1
            // 
            this.scTabAndSelector.Panel1.Controls.Add(this.xtraTabControl1);
            // 
            // scTabAndSelector.Panel2
            // 
            this.scTabAndSelector.Panel2.Controls.Add(this.gbPivotFields);
            this.scTabAndSelector.Panel2MinSize = 0;
            this.scTabAndSelector.Size = new System.Drawing.Size(762, 254);
            this.scTabAndSelector.SplitterDistance = 351;
            this.scTabAndSelector.SplitterWidth = 3;
            this.scTabAndSelector.TabIndex = 2;
            // 
            // gbPivotFields
            // 
            this.gbPivotFields.Controls.Add(this.amphoraFieldSelector1);
            this.gbPivotFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbPivotFields.Location = new System.Drawing.Point(0, 0);
            this.gbPivotFields.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbPivotFields.Name = "gbPivotFields";
            this.gbPivotFields.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbPivotFields.Size = new System.Drawing.Size(408, 254);
            this.gbPivotFields.TabIndex = 0;
            this.gbPivotFields.TabStop = false;
            this.gbPivotFields.Text = "Pivot Fields";
            // 
            // amphoraFieldSelector1
            // 
            this.amphoraFieldSelector1.aColor = System.Drawing.Color.Empty;
            this.amphoraFieldSelector1.AutoScroll = true;
            this.amphoraFieldSelector1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.amphoraFieldSelector1.Location = new System.Drawing.Point(3, 16);
            this.amphoraFieldSelector1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.amphoraFieldSelector1.Name = "amphoraFieldSelector1";
            this.amphoraFieldSelector1.Size = new System.Drawing.Size(402, 236);
            this.amphoraFieldSelector1.TabIndex = 0;
            this.amphoraFieldSelector1.Load += new System.EventHandler(this.amphoraFieldSelector1_Load);
            
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.scTabAndSelector);
            this.panelControl1.Controls.Add(this.lowerControlPanel);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(766, 280);
            this.panelControl1.TabIndex = 3;
            // 
            // scPivotAndLower
            // 
            this.scPivotAndLower.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scPivotAndLower.Horizontal = false;
            this.scPivotAndLower.Location = new System.Drawing.Point(0, 0);
            this.scPivotAndLower.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.scPivotAndLower.Name = "scPivotAndLower";
            this.scPivotAndLower.Panel1.Controls.Add(this.panelControl1);
            this.scPivotAndLower.Panel1.Text = "Panel1";
            this.scPivotAndLower.Panel2.Controls.Add(this.DetailsXtraTabControl);
            this.scPivotAndLower.Panel2.Text = "Panel2";
            this.scPivotAndLower.Size = new System.Drawing.Size(766, 466);
            this.scPivotAndLower.SplitterPosition = 280;
            this.scPivotAndLower.TabIndex = 4;
            this.scPivotAndLower.Text = "splitContainerControl1";
            // 
            // xtcDetails
            // 
            this.DetailsXtraTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DetailsXtraTabControl.Location = new System.Drawing.Point(0, 0);
            this.DetailsXtraTabControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DetailsXtraTabControl.Name = "xtcDetails";
            this.DetailsXtraTabControl.SelectedPageChanged += new TabPageChangedEventHandler(DetailsXtraTabControl_SelectedPageChanged);
            this.DetailsXtraTabControl.SelectedTabPage = this.PnLTabPage;
            this.DetailsXtraTabControl.Size = new System.Drawing.Size(766, 181);
            this.DetailsXtraTabControl.TabIndex = 0;
            this.DetailsXtraTabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.PnLTabPage,
            this.DistributionsTabPage,
            this.PositionsTabPage, this.InventoryBuildDrawTabPage});
           
            // 
            // tpPnL
            // 
            this.PnLTabPage.Controls.Add(this.scPnlGrids);
            this.PnLTabPage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PnLTabPage.Name = "tpPnL";
            this.PnLTabPage.Size = new System.Drawing.Size(760, 153);
            this.PnLTabPage.Text = "P/L";
            // 
            // scPnlGrids
            // 
            this.scPnlGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scPnlGrids.Location = new System.Drawing.Point(0, 0);
            this.scPnlGrids.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.scPnlGrids.Name = "scPnlGrids";
            this.scPnlGrids.Panel1.Controls.Add(this.PnLGroupControl);
            this.scPnlGrids.Panel1.Text = "Panel1";
            this.scPnlGrids.Panel2.Controls.Add(this.grpHistPnL);
            this.scPnlGrids.Panel2.Text = "Panel2";
            this.scPnlGrids.Size = new System.Drawing.Size(760, 153);
            this.scPnlGrids.SplitterPosition = 318;
            this.scPnlGrids.TabIndex = 3;
            this.scPnlGrids.Text = "splitContainerControl2";
            // 
            // grpPnL
            // 
            this.PnLGroupControl.Controls.Add(this.PnLGridControl);
            this.PnLGroupControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnLGroupControl.Location = new System.Drawing.Point(0, 0);
            this.PnLGroupControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PnLGroupControl.Name = "grpPnL";
            this.PnLGroupControl.Size = new System.Drawing.Size(318, 153);
            this.PnLGroupControl.TabIndex = 1;
            this.PnLGroupControl.Text = "P/L";
            // 
            // gcPnL
            // 
            this.PnLGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnLGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PnLGridControl.Location = new System.Drawing.Point(2, 21);
            this.PnLGridControl.MainView = this.gvPnL;
            this.PnLGridControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PnLGridControl.Name = "gcPnL";
            this.PnLGridControl.Size = new System.Drawing.Size(314, 130);
            this.PnLGridControl.TabIndex = 0;
            this.PnLGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvPnL});
            // 
            // gvPnL
            // 
            this.gvPnL.GridControl = this.PnLGridControl;
            this.gvPnL.Name = "gvPnL";
            this.gvPnL.OptionsView.ShowGroupPanel = false;
            this.gvPnL.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gvPnL_CustomDrawRowIndicator);
            // 
            // grpHistPnL
            // 
            this.grpHistPnL.Controls.Add(this.HistoricalPnLGridControl);
            this.grpHistPnL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpHistPnL.Location = new System.Drawing.Point(0, 0);
            this.grpHistPnL.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grpHistPnL.Name = "grpHistPnL";
            this.grpHistPnL.Size = new System.Drawing.Size(437, 153);
            this.grpHistPnL.TabIndex = 2;
            this.grpHistPnL.Text = "Historical P/L";
            // 
            // gcHistoricalPnL
            // 
            this.HistoricalPnLGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HistoricalPnLGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.HistoricalPnLGridControl.Location = new System.Drawing.Point(2, 21);
            this.HistoricalPnLGridControl.MainView = this.HistoricalPnLGridView;
            this.HistoricalPnLGridControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.HistoricalPnLGridControl.Name = "gcHistoricalPnL";
            this.HistoricalPnLGridControl.Size = new System.Drawing.Size(433, 130);
            this.HistoricalPnLGridControl.TabIndex = 0;
            this.HistoricalPnLGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.HistoricalPnLGridView});
            // 
            // gvHistoricalPnL
            // 
            this.HistoricalPnLGridView.GridControl = this.HistoricalPnLGridControl;
            this.HistoricalPnLGridView.Name = "gvHistoricalPnL";
            this.HistoricalPnLGridView.OptionsView.ShowGroupPanel = false;



            // 
            // positions
            // 
            this.PositionsTabPage.Controls.Add(this.PositionsGridControl);
            this.PositionsTabPage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PositionsTabPage.Name = "PositionsTabPage";
            this.PositionsTabPage.Size = new System.Drawing.Size(760, 153);
            this.PositionsTabPage.Text = "Positions";
            // 
            // 
            // 
            this.PositionsGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PositionsGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PositionsGridControl.Location = new System.Drawing.Point(0, 0);
            this.PositionsGridControl.MainView = this.PositionsGridView;
            this.PositionsGridControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PositionsGridControl.Name = "PositionsGridControl";
            this.PositionsGridControl.Size = new System.Drawing.Size(760, 153);
            this.PositionsGridControl.TabIndex = 0;
            this.PositionsGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.PositionsGridView});
            // 
            // 
            // 
            this.PositionsGridView.GridControl = this.PositionsGridControl;
            this.PositionsGridView.Name = "PositionsGridView";
            this.PositionsGridView.OptionsView.ShowGroupPanel = false;
            this.PositionsGridView.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.positionsCustomColumnDisplayText);
            this.PositionsGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] 
            {
                PositionNumberColumn,
                PositionTypeColumn,
                CommodityColumn,
                MarketColumn,
                PeriodColumn,
                PricePerUnitColumn,
                MtmPriceColumn
            });


            this.PositionNumberColumn.Caption = "Position Num";
            this.PositionNumberColumn.FieldName = "positionNumber";
            this.PositionNumberColumn.Name = "PositionNumberColumn;";
            this.PositionNumberColumn.Visible = true;
            this.PositionNumberColumn.VisibleIndex = 0;

            this.PositionTypeColumn.Caption = "Position Type";
            this.PositionTypeColumn.FieldName = "displayPosSource";
            this.PositionTypeColumn.Name = "PositionTypeColumn;";
            this.PositionTypeColumn.Visible = true;
            this.PositionTypeColumn.VisibleIndex = 1;

            this.CommodityColumn.Caption = "Commodity";
            this.CommodityColumn.FieldName = "cmdtyName";
            this.CommodityColumn.Name = "CommodityColumn;";
            this.CommodityColumn.Visible = true;
            this.CommodityColumn.VisibleIndex = 2;


            this.MarketColumn.Caption = "Market";
            this.MarketColumn.FieldName = "mktName";
            this.MarketColumn.Name = "MarketColumn;";
            this.MarketColumn.Visible = true;
            this.MarketColumn.VisibleIndex = 3;


            this.PeriodColumn.Caption = "Period";
            this.PeriodColumn.FieldName = "formattedTradePeriod";
            this.PeriodColumn.Name = "PeriodColumn;";
            this.PeriodColumn.Visible = true;
            this.PeriodColumn.VisibleIndex = 4;


            this.PricePerUnitColumn.Caption = "Price/Unit";
            this.PricePerUnitColumn.FieldName = "avgPurchaseSellPrice";
            this.PricePerUnitColumn.Name = "PricePerUnitColumn;";
            this.PricePerUnitColumn.Visible = true;
            this.PricePerUnitColumn.VisibleIndex = 5;


            this.MtmPriceColumn.Caption = "MTM Price";
            this.MtmPriceColumn.FieldName = "lastMtmPrice";
            this.MtmPriceColumn.Name = "MtmPriceColumn;";
            this.MtmPriceColumn.Visible = true;
            this.MtmPriceColumn.VisibleIndex = 6;


            //
            //tpInventoryBuildDraw
            //
            this.InventoryBuildDrawTabPage.Controls.Add(this.InventoryBuildDrawGridControl);
            this.InventoryBuildDrawTabPage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.InventoryBuildDrawTabPage.Name = "tpInventoryBuildDraw";
            this.InventoryBuildDrawTabPage.Size = new System.Drawing.Size(760, 153);
            this.InventoryBuildDrawTabPage.Text = "Inventory Build/Draw";
            // 
            // gcInventoryBuildDraw
            // 
            this.InventoryBuildDrawGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InventoryBuildDrawGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.InventoryBuildDrawGridControl.Location = new System.Drawing.Point(0, 0);
            this.InventoryBuildDrawGridControl.MainView = this.InventoryBuildDrawGridView;
            this.InventoryBuildDrawGridControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.InventoryBuildDrawGridControl.Name = "gcInventoryBuildDraw";
            this.InventoryBuildDrawGridControl.Size = new System.Drawing.Size(760, 153);
            this.InventoryBuildDrawGridControl.TabIndex = 0;
            this.InventoryBuildDrawGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.InventoryBuildDrawGridView});
            // 
            // gvInventoryBuildDraw
            // 
            this.InventoryBuildDrawGridView.GridControl = this.InventoryBuildDrawGridControl;
            this.InventoryBuildDrawGridView.Name = "gvInventoryBuildDraw";
            this.InventoryBuildDrawGridView.OptionsView.ShowGroupPanel = false;
            //this.InventoryBuildDrawGridView.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.distributionsCustomColumnDisplayText);
            this.InventoryBuildDrawGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { 
               InventoryNumberColumn,InventoryTypeColumn,InventoryBuildDrawQtyColumn,InventoryBuildDrawCostUomCodeColumn,
            InventoryAllocationNumberColumn,InventoryAllocationItemNumberColumn,InventoryTradeNumberColumn,InventoryOrderNumberColumn,
            InventoryItemNumberColumn});

            this.InventoryNumberColumn.Caption = "Inventory Num";
            this.InventoryNumberColumn.FieldName = "invNum";
            this.InventoryNumberColumn.Name = "InventoryNumberColumn;";
            this.InventoryNumberColumn.Visible = true;
            this.InventoryNumberColumn.VisibleIndex = 0;

            this.InventoryTypeColumn.Caption = "Type";
            this.InventoryTypeColumn.FieldName = "invBDType";
            this.InventoryTypeColumn.Name = "InventoryTypeColumn;";
            this.InventoryTypeColumn.Visible = true;
            this.InventoryTypeColumn.VisibleIndex = 1;

            this.InventoryBuildDrawQtyColumn.Caption = "Quantity";
            this.InventoryBuildDrawQtyColumn.FieldName = "invBDQty";
            this.InventoryBuildDrawQtyColumn.Name = "InventoryBuildDrawQtyColumn;";
            this.InventoryBuildDrawQtyColumn.Visible = true;
            this.InventoryBuildDrawQtyColumn.VisibleIndex = 2;

            this.InventoryBuildDrawCostUomCodeColumn.Caption = "Uom";
            this.InventoryBuildDrawCostUomCodeColumn.FieldName = "invBDCostUomCode";
            this.InventoryBuildDrawCostUomCodeColumn.Name = "InventoryBuildDrawCostUomCodeColumn;";
            this.InventoryBuildDrawCostUomCodeColumn.Visible = true;
            this.InventoryBuildDrawCostUomCodeColumn.VisibleIndex = 3;

            this.InventoryAllocationNumberColumn.Caption = "Allocation Num";
            this.InventoryAllocationNumberColumn.FieldName = "allocNum";
            this.InventoryAllocationNumberColumn.Name = "InventoryAllocationNumberColumn;";
            this.InventoryAllocationNumberColumn.Visible = true;
            this.InventoryAllocationNumberColumn.VisibleIndex = 4;

            this.InventoryAllocationItemNumberColumn.Caption = "Allocation Item Num";
            this.InventoryAllocationItemNumberColumn.FieldName = "allocItemNum";
            this.InventoryAllocationItemNumberColumn.Name = "InventoryAllocationItemNumberColumn;";
            this.InventoryAllocationItemNumberColumn.Visible = true;
            this.InventoryAllocationItemNumberColumn.VisibleIndex = 5;

            this.InventoryTradeNumberColumn.Caption = "Trade Num";
            this.InventoryTradeNumberColumn.FieldName = "tradeNum";
            this.InventoryTradeNumberColumn.Name = "InventoryTradeNumberColumn;";
            this.InventoryTradeNumberColumn.Visible = true;
            this.InventoryTradeNumberColumn.VisibleIndex = 6;

            this.InventoryOrderNumberColumn.Caption = "Order Num";
            this.InventoryOrderNumberColumn.FieldName = "orderNum";
            this.InventoryOrderNumberColumn.Name = "InventoryOrderNumberColumn;";
            this.InventoryOrderNumberColumn.Visible = true;
            this.InventoryOrderNumberColumn.VisibleIndex = 7;

            this.InventoryItemNumberColumn.Caption = "Item Num";
            this.InventoryItemNumberColumn.FieldName = "itemNum";
            this.InventoryItemNumberColumn.Name = "InventoryItemNumberColumn;";
            this.InventoryItemNumberColumn.Visible = true;
            this.InventoryItemNumberColumn.VisibleIndex = 8;



            // 
            // tpDistributions
            // 
            this.DistributionsTabPage.Controls.Add(this.distributionsGridControl);
            this.DistributionsTabPage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DistributionsTabPage.Name = "tpDistributions";
            this.DistributionsTabPage.Size = new System.Drawing.Size(760, 153);
            this.DistributionsTabPage.Text = "Distributions";
            // 
            // gcDistributions
            // 
            this.distributionsGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.distributionsGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.distributionsGridControl.Location = new System.Drawing.Point(0, 0);
            this.distributionsGridControl.MainView = this.DistributionsGridView;
            this.distributionsGridControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.distributionsGridControl.Name = "gcDistributions";
            this.distributionsGridControl.Size = new System.Drawing.Size(760, 153);
            this.distributionsGridControl.TabIndex = 0;
            this.distributionsGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.DistributionsGridView});
            // 
            // gvDistributions
            // 
            this.DistributionsGridView.GridControl = this.distributionsGridControl;
            this.DistributionsGridView.Name = "gvDistributions";
            this.DistributionsGridView.OptionsView.ShowGroupPanel = false;
            this.DistributionsGridView.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.distributionsCustomColumnDisplayText);
            this.DistributionsGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { 
                DistributionNumberColumn,TradeNumberColumn, OrderNumberColumn,ItemNumberColumn,
                RiskQuantityColumn,PhysicalQuantityColumn,DiscountQuantityColumn,
                TradeCaptureColumn, LogisticsColumn,DealViewerColumn});


            this.DistributionNumberColumn.Caption = "Distribution Num";
            this.DistributionNumberColumn.FieldName = "distNum";
            this.DistributionNumberColumn.Name = "DistributionNumberColumn";
            this.DistributionNumberColumn.Visible = true;
            this.DistributionNumberColumn.VisibleIndex = 0;

            this.TradeNumberColumn.Caption = "Trade Num";
            this.TradeNumberColumn.FieldName = "tradeNum";
            this.TradeNumberColumn.Name = "TradeNumberColumn;";
            this.TradeNumberColumn.Visible = true;
            this.TradeNumberColumn.VisibleIndex = 1;

            this.OrderNumberColumn.Caption = "Order Num";
            this.OrderNumberColumn.FieldName = "orderNum";
            this.OrderNumberColumn.Name = "OrderNumberColumn";
            this.OrderNumberColumn.Visible = true;
            this.OrderNumberColumn.VisibleIndex = 2;

            this.ItemNumberColumn.Caption = "Item Num";
            this.ItemNumberColumn.FieldName = "itemNum";
            this.ItemNumberColumn.Name = "ItemNumberColumn";
            this.ItemNumberColumn.Visible = true;
            this.ItemNumberColumn.VisibleIndex = 3;

            this.RiskQuantityColumn.Caption = "Risk Qty";
            this.RiskQuantityColumn.FieldName = "RiskQty";
            this.RiskQuantityColumn.Name = "RiskQuantityColumn";
            this.RiskQuantityColumn.Visible = true;
            this.RiskQuantityColumn.VisibleIndex = 4;

            this.PhysicalQuantityColumn.Caption = "Physical Qty";
            this.PhysicalQuantityColumn.FieldName = "PhysicalQty";
            this.PhysicalQuantityColumn.Name = "PhysicalQuantityColumn";
            this.PhysicalQuantityColumn.Visible = true;
            this.PhysicalQuantityColumn.VisibleIndex = 5;

            this.DiscountQuantityColumn.Caption = "Discount Qty";
            this.DiscountQuantityColumn.FieldName = "DiscountQty";
            this.DiscountQuantityColumn.Name = "DiscountQuantityColumn";
            this.DiscountQuantityColumn.Visible = true;
            this.DiscountQuantityColumn.VisibleIndex = 6;

            this.TradeCaptureColumn.Caption = "";
            this.TradeCaptureColumn.FieldName = "Trade Capture";
            this.TradeCaptureColumn.Name = "TradeCaptureColumn";
            this.TradeCaptureColumn.Visible = true;
            this.TradeCaptureColumn.VisibleIndex = 7;
            this.TradeCaptureColumn.UnboundType =   DevExpress.Data.UnboundColumnType.Object;
            this.TradeCaptureColumn.ColumnEdit = this.TradeCaptureButton; 


            this.LogisticsColumn.Caption = "";
            this.LogisticsColumn.FieldName = "Logistics";
            this.LogisticsColumn.Name = "LogisticsColumn";
            this.LogisticsColumn.Visible = true;
            this.LogisticsColumn.VisibleIndex = 8;
            this.LogisticsColumn.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.LogisticsColumn.ColumnEdit = this.LogisticsButton; 



            this.DealViewerColumn.Caption = "";
            this.DealViewerColumn.FieldName = "Deal Viewer";
            this.DealViewerColumn.Name = "DealViewerColumn";
            this.DealViewerColumn.Visible = true;
            this.DealViewerColumn.VisibleIndex = 9;
            this.DealViewerColumn.UnboundType = DevExpress.Data.UnboundColumnType.Object;
            this.DealViewerColumn.ColumnEdit = this.DealViewerButton;


            DevExpress.XtraEditors.Controls.EditorButton tradeCaptureEditorButton = new DevExpress.XtraEditors.Controls.EditorButton();
            tradeCaptureEditorButton.Caption = "Trade Capture";
            tradeCaptureEditorButton.Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;

            DevExpress.XtraEditors.Controls.EditorButton dealViewerEditorButton = new DevExpress.XtraEditors.Controls.EditorButton();
            dealViewerEditorButton.Caption = "Deal Viewer";
            dealViewerEditorButton.Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;

            DevExpress.XtraEditors.Controls.EditorButton logisticsEditorButton = new DevExpress.XtraEditors.Controls.EditorButton();
            logisticsEditorButton.Caption = "Logistics";
            logisticsEditorButton.Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;

            TradeCaptureButton.AutoHeight = false;
            TradeCaptureButton.Buttons.Clear();
            TradeCaptureButton.Buttons.Add(tradeCaptureEditorButton);
            TradeCaptureButton.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            TradeCaptureButton.Name = "TradeCaptureButton";
            TradeCaptureButton.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(tradeCaptureClickedHandler);

            LogisticsButton.AutoHeight = false;
            LogisticsButton.Buttons.Clear();
            LogisticsButton.Buttons.Add(logisticsEditorButton);
            LogisticsButton.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            LogisticsButton.Name = "LogisticsButton";
            LogisticsButton.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(logisticsClickedHandler);

            DealViewerButton.AutoHeight = false;
            DealViewerButton.Buttons.Clear();
            DealViewerButton.Buttons.Add(dealViewerEditorButton);
            DealViewerButton.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            DealViewerButton.Name = "DealViewerButton";
            DealViewerButton.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(dealViewerClickedHandler);
            

            // InspectablePivotGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scPivotAndLower);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "InspectablePivotGrid";
            this.Size = new System.Drawing.Size(766, 466);
            this.lowerControlPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.renameEditor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.scTabAndSelector.Panel1.ResumeLayout(false);
            this.scTabAndSelector.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scTabAndSelector)).EndInit();
            this.scTabAndSelector.ResumeLayout(false);
            this.gbPivotFields.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scPivotAndLower)).EndInit();
            this.scPivotAndLower.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DetailsXtraTabControl)).EndInit();
            this.DetailsXtraTabControl.ResumeLayout(false);
            this.PnLTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scPnlGrids)).EndInit();
            this.scPnlGrids.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PnLGroupControl)).EndInit();
            this.PnLGroupControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PnLGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPnL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpHistPnL)).EndInit();
            this.grpHistPnL.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.HistoricalPnLGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HistoricalPnLGridView)).EndInit();
            this.DistributionsTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.distributionsGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DistributionsGridView)).EndInit();

            this.PositionsTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PositionsGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionsGridView)).EndInit();
            this.InventoryBuildDrawTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.InventoryBuildDrawGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InventoryBuildDrawGridView)).EndInit();
            this.ResumeLayout(false);

        }

       
        #endregion Component Designer generated code

        ContextMenuStrip contextMenuStrip1;
        ToolStripMenuItem ctxAdd;
        ToolStripMenuItem ctxRemove;

        public XtraTabControl xtraTabControl1;
        Button btnExpand;
        public SplitContainer scTabAndSelector;
        GroupBox gbPivotFields;
        public AmphoraFieldSelector amphoraFieldSelector1;
        Panel lowerControlPanel;
        public PanelControl panelControl1;
        public SplitContainerControl scPivotAndLower;
        private XtraTabControl DetailsXtraTabControl;
        private XtraTabPage PnLTabPage;
        private XtraTabPage DistributionsTabPage;
        private XtraTabPage PositionsTabPage;

        private DevExpress.XtraGrid.GridControl PnLGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gvPnL;
        private TextEdit renameEditor;
        private DevExpress.XtraGrid.GridControl distributionsGridControl;
        private DevExpress.XtraGrid.GridControl PositionsGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView DistributionsGridView;
        private DevExpress.XtraGrid.Views.Grid.GridView PositionsGridView;
        private GroupControl grpHistPnL;
        private DevExpress.XtraGrid.GridControl HistoricalPnLGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView HistoricalPnLGridView;
        private GroupControl PnLGroupControl;
        public SplitContainerControl scPnlGrids;

        private XtraTabPage InventoryBuildDrawTabPage;
        private DevExpress.XtraGrid.GridControl InventoryBuildDrawGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView InventoryBuildDrawGridView;

        private DevExpress.XtraGrid.Columns.GridColumn DistributionNumberColumn;
        private DevExpress.XtraGrid.Columns.GridColumn TradeNumberColumn;
        private DevExpress.XtraGrid.Columns.GridColumn OrderNumberColumn;
        private DevExpress.XtraGrid.Columns.GridColumn ItemNumberColumn;
        private DevExpress.XtraGrid.Columns.GridColumn RiskQuantityColumn;
        private DevExpress.XtraGrid.Columns.GridColumn PhysicalQuantityColumn;
        private DevExpress.XtraGrid.Columns.GridColumn DiscountQuantityColumn;

        private DevExpress.XtraGrid.Columns.GridColumn TradeCaptureColumn;
        private DevExpress.XtraGrid.Columns.GridColumn LogisticsColumn;
        private DevExpress.XtraGrid.Columns.GridColumn DealViewerColumn;

        private DevExpress.XtraGrid.Columns.GridColumn InventoryNumberColumn;
        private DevExpress.XtraGrid.Columns.GridColumn InventoryTypeColumn;
        private DevExpress.XtraGrid.Columns.GridColumn InventoryBuildDrawQtyColumn;
        private DevExpress.XtraGrid.Columns.GridColumn InventoryBuildDrawCostUomCodeColumn;
        private DevExpress.XtraGrid.Columns.GridColumn InventoryAllocationNumberColumn;
        private DevExpress.XtraGrid.Columns.GridColumn InventoryAllocationItemNumberColumn;
        private DevExpress.XtraGrid.Columns.GridColumn InventoryTradeNumberColumn;
        private DevExpress.XtraGrid.Columns.GridColumn InventoryOrderNumberColumn;
        private DevExpress.XtraGrid.Columns.GridColumn InventoryItemNumberColumn;


        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit TradeCaptureButton;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit LogisticsButton;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit DealViewerButton;


        private DevExpress.XtraGrid.Columns.GridColumn PositionNumberColumn;
        private DevExpress.XtraGrid.Columns.GridColumn PositionTypeColumn;
        private DevExpress.XtraGrid.Columns.GridColumn CommodityColumn;
        private DevExpress.XtraGrid.Columns.GridColumn MarketColumn;
        private DevExpress.XtraGrid.Columns.GridColumn PeriodColumn;
        private DevExpress.XtraGrid.Columns.GridColumn PricePerUnitColumn;
        private DevExpress.XtraGrid.Columns.GridColumn MtmPriceColumn;
    }
}