namespace ConfirmManager
{
   partial class frmTradeDataChanges
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTradeDataChanges));
            this.gridTradeDataChanges = new DevExpress.XtraGrid.GridControl();
            this.gridViewTradeDataChanges = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colJnDatetime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTradeId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBuySellInd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTradeDt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBookingCoSn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCptySn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBrokerSn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTradeDesc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPriceDesc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTradeStatCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLocationSn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQtyDesc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQtyTot = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTradeTypeCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCdtyCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSttlType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBook = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTransportDesc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCptyLegalName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBrokerLegalName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBrokerPrice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTrader = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCdtyGrpCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStartDt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEndDt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colXref = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRefSn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colInceptionDt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOptnPutCallInd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOptnPremPrice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOptnStrikePrice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colProfitCenter = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPermissionKey = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnExcel = new DevExpress.XtraEditors.SimpleButton();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            ((System.ComponentModel.ISupportInitialize)(this.gridTradeDataChanges)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTradeDataChanges)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridTradeDataChanges
            // 
            this.gridTradeDataChanges.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTradeDataChanges.Location = new System.Drawing.Point(0, 40);
            this.gridTradeDataChanges.MainView = this.gridViewTradeDataChanges;
            this.gridTradeDataChanges.Name = "gridTradeDataChanges";
            this.gridTradeDataChanges.Size = new System.Drawing.Size(626, 240);
            this.gridTradeDataChanges.TabIndex = 3;
            this.gridTradeDataChanges.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewTradeDataChanges});
            // 
            // gridViewTradeDataChanges
            // 
            this.gridViewTradeDataChanges.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colJnDatetime,
            this.colTradeId,
            this.colBuySellInd,
            this.colTradeDt,
            this.colBookingCoSn,
            this.colCptySn,
            this.colBrokerSn,
            this.colTradeDesc,
            this.colPriceDesc,
            this.colTradeStatCode,
            this.colLocationSn,
            this.colQtyDesc,
            this.colQtyTot,
            this.colTradeTypeCode,
            this.colCdtyCode,
            this.colSttlType,
            this.colBook,
            this.colTransportDesc,
            this.colCptyLegalName,
            this.colBrokerLegalName,
            this.colBrokerPrice,
            this.colTrader,
            this.colCdtyGrpCode,
            this.colStartDt,
            this.colEndDt,
            this.colXref,
            this.colRefSn,
            this.colInceptionDt,
            this.colOptnPutCallInd,
            this.colOptnPremPrice,
            this.colOptnStrikePrice,
            this.colProfitCenter,
            this.colPermissionKey});
            this.gridViewTradeDataChanges.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridViewTradeDataChanges.GridControl = this.gridTradeDataChanges;
            this.gridViewTradeDataChanges.Name = "gridViewTradeDataChanges";
            this.gridViewTradeDataChanges.OptionsBehavior.Editable = false;
            this.gridViewTradeDataChanges.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewTradeDataChanges.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gridViewTradeDataChanges.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.gridViewTradeDataChanges.OptionsSelection.UseIndicatorForSelection = false;
            this.gridViewTradeDataChanges.OptionsView.ColumnAutoWidth = false;
            this.gridViewTradeDataChanges.OptionsView.EnableAppearanceEvenRow = true;
            this.gridViewTradeDataChanges.OptionsView.EnableAppearanceOddRow = true;
            this.gridViewTradeDataChanges.OptionsView.ShowGroupPanel = false;
            this.gridViewTradeDataChanges.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridViewTradeDataChanges_CustomDrawCell);
            // 
            // colJnDatetime
            // 
            this.colJnDatetime.Caption = "Changed Datetime";
            this.colJnDatetime.DisplayFormat.FormatString = "u";
            this.colJnDatetime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colJnDatetime.FieldName = "JnDatetime";
            this.colJnDatetime.Name = "colJnDatetime";
            this.colJnDatetime.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.colJnDatetime.Visible = true;
            this.colJnDatetime.VisibleIndex = 0;
            // 
            // colTradeId
            // 
            this.colTradeId.Caption = "Trade Id";
            this.colTradeId.FieldName = "TradeId";
            this.colTradeId.Name = "colTradeId";
            this.colTradeId.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colTradeId.Visible = true;
            this.colTradeId.VisibleIndex = 2;
            // 
            // colBuySellInd
            // 
            this.colBuySellInd.Caption = "Buy/Sell";
            this.colBuySellInd.FieldName = "BuySellInd";
            this.colBuySellInd.Name = "colBuySellInd";
            this.colBuySellInd.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colBuySellInd.Visible = true;
            this.colBuySellInd.VisibleIndex = 3;
            // 
            // colTradeDt
            // 
            this.colTradeDt.Caption = "Trade Dt";
            this.colTradeDt.DisplayFormat.FormatString = "dd-MMM-yyyy";
            this.colTradeDt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colTradeDt.FieldName = "TradeDt";
            this.colTradeDt.Name = "colTradeDt";
            this.colTradeDt.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colTradeDt.Visible = true;
            this.colTradeDt.VisibleIndex = 4;
            // 
            // colBookingCoSn
            // 
            this.colBookingCoSn.Caption = "Booking Co Sn";
            this.colBookingCoSn.FieldName = "BookingCoSn";
            this.colBookingCoSn.Name = "colBookingCoSn";
            this.colBookingCoSn.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colBookingCoSn.Visible = true;
            this.colBookingCoSn.VisibleIndex = 1;
            // 
            // colCptySn
            // 
            this.colCptySn.Caption = "Cpty SN";
            this.colCptySn.FieldName = "CptySn";
            this.colCptySn.Name = "colCptySn";
            this.colCptySn.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colCptySn.Visible = true;
            this.colCptySn.VisibleIndex = 5;
            // 
            // colBrokerSn
            // 
            this.colBrokerSn.Caption = "Broker Sn";
            this.colBrokerSn.FieldName = "BrokerSn";
            this.colBrokerSn.Name = "colBrokerSn";
            this.colBrokerSn.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colBrokerSn.Visible = true;
            this.colBrokerSn.VisibleIndex = 6;
            // 
            // colTradeDesc
            // 
            this.colTradeDesc.Caption = "Trade Description";
            this.colTradeDesc.FieldName = "TradeDesc";
            this.colTradeDesc.Name = "colTradeDesc";
            this.colTradeDesc.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colTradeDesc.Visible = true;
            this.colTradeDesc.VisibleIndex = 7;
            // 
            // colPriceDesc
            // 
            this.colPriceDesc.Caption = "Price Description";
            this.colPriceDesc.FieldName = "PriceDesc";
            this.colPriceDesc.Name = "colPriceDesc";
            this.colPriceDesc.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colPriceDesc.Visible = true;
            this.colPriceDesc.VisibleIndex = 8;
            // 
            // colTradeStatCode
            // 
            this.colTradeStatCode.Caption = "Trade Status Code";
            this.colTradeStatCode.FieldName = "TradeStatCode";
            this.colTradeStatCode.Name = "colTradeStatCode";
            this.colTradeStatCode.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colTradeStatCode.Visible = true;
            this.colTradeStatCode.VisibleIndex = 9;
            // 
            // colLocationSn
            // 
            this.colLocationSn.Caption = "Location";
            this.colLocationSn.FieldName = "LocationSn";
            this.colLocationSn.Name = "colLocationSn";
            this.colLocationSn.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colLocationSn.Visible = true;
            this.colLocationSn.VisibleIndex = 10;
            // 
            // colQtyDesc
            // 
            this.colQtyDesc.Caption = "Qty Description";
            this.colQtyDesc.FieldName = "QtyDesc";
            this.colQtyDesc.Name = "colQtyDesc";
            this.colQtyDesc.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colQtyDesc.Visible = true;
            this.colQtyDesc.VisibleIndex = 11;
            // 
            // colQtyTot
            // 
            this.colQtyTot.Caption = "Qty Total";
            this.colQtyTot.DisplayFormat.FormatString = "##,#";
            this.colQtyTot.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colQtyTot.FieldName = "QtyTot";
            this.colQtyTot.Name = "colQtyTot";
            this.colQtyTot.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colQtyTot.Visible = true;
            this.colQtyTot.VisibleIndex = 12;
            // 
            // colTradeTypeCode
            // 
            this.colTradeTypeCode.Caption = "Trade Type";
            this.colTradeTypeCode.FieldName = "TradeTypeCode";
            this.colTradeTypeCode.Name = "colTradeTypeCode";
            this.colTradeTypeCode.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colTradeTypeCode.Visible = true;
            this.colTradeTypeCode.VisibleIndex = 13;
            // 
            // colCdtyCode
            // 
            this.colCdtyCode.Caption = "Cdty Code";
            this.colCdtyCode.FieldName = "CdtyCode";
            this.colCdtyCode.Name = "colCdtyCode";
            this.colCdtyCode.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colCdtyCode.Visible = true;
            this.colCdtyCode.VisibleIndex = 14;
            // 
            // colSttlType
            // 
            this.colSttlType.Caption = "Sttl Type";
            this.colSttlType.FieldName = "SttlType";
            this.colSttlType.Name = "colSttlType";
            this.colSttlType.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colSttlType.Visible = true;
            this.colSttlType.VisibleIndex = 15;
            // 
            // colBook
            // 
            this.colBook.Caption = "Book";
            this.colBook.FieldName = "Book";
            this.colBook.Name = "colBook";
            this.colBook.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colBook.Visible = true;
            this.colBook.VisibleIndex = 16;
            // 
            // colTransportDesc
            // 
            this.colTransportDesc.Caption = "Transport Description";
            this.colTransportDesc.FieldName = "TransportDesc";
            this.colTransportDesc.Name = "colTransportDesc";
            this.colTransportDesc.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colTransportDesc.Visible = true;
            this.colTransportDesc.VisibleIndex = 17;
            // 
            // colCptyLegalName
            // 
            this.colCptyLegalName.Caption = "Cpty Legal Name";
            this.colCptyLegalName.FieldName = "CptyLegalName";
            this.colCptyLegalName.Name = "colCptyLegalName";
            this.colCptyLegalName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colCptyLegalName.Visible = true;
            this.colCptyLegalName.VisibleIndex = 18;
            // 
            // colBrokerLegalName
            // 
            this.colBrokerLegalName.Caption = "Broker Legal Name";
            this.colBrokerLegalName.FieldName = "BrokerLegalName";
            this.colBrokerLegalName.Name = "colBrokerLegalName";
            this.colBrokerLegalName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colBrokerLegalName.Visible = true;
            this.colBrokerLegalName.VisibleIndex = 19;
            // 
            // colBrokerPrice
            // 
            this.colBrokerPrice.Caption = "Broker Price";
            this.colBrokerPrice.FieldName = "BrokerPrice";
            this.colBrokerPrice.Name = "colBrokerPrice";
            this.colBrokerPrice.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colBrokerPrice.Visible = true;
            this.colBrokerPrice.VisibleIndex = 20;
            // 
            // colTrader
            // 
            this.colTrader.Caption = "Trader";
            this.colTrader.FieldName = "Trader";
            this.colTrader.Name = "colTrader";
            this.colTrader.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colTrader.Visible = true;
            this.colTrader.VisibleIndex = 21;
            // 
            // colCdtyGrpCode
            // 
            this.colCdtyGrpCode.Caption = "Cdty Group Code";
            this.colCdtyGrpCode.FieldName = "CdtyGrpCode";
            this.colCdtyGrpCode.Name = "colCdtyGrpCode";
            this.colCdtyGrpCode.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colCdtyGrpCode.Visible = true;
            this.colCdtyGrpCode.VisibleIndex = 22;
            // 
            // colStartDt
            // 
            this.colStartDt.Caption = "Start Date";
            this.colStartDt.DisplayFormat.FormatString = "dd-MMM-yyyy";
            this.colStartDt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colStartDt.FieldName = "StartDt";
            this.colStartDt.Name = "colStartDt";
            this.colStartDt.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colStartDt.Visible = true;
            this.colStartDt.VisibleIndex = 23;
            // 
            // colEndDt
            // 
            this.colEndDt.Caption = "End Date";
            this.colEndDt.DisplayFormat.FormatString = "dd-MMM-yyyy";
            this.colEndDt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colEndDt.FieldName = "EndDt";
            this.colEndDt.Name = "colEndDt";
            this.colEndDt.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colEndDt.Visible = true;
            this.colEndDt.VisibleIndex = 24;
            // 
            // colXref
            // 
            this.colXref.Caption = "Cross Reference";
            this.colXref.FieldName = "Xref";
            this.colXref.Name = "colXref";
            this.colXref.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colXref.Visible = true;
            this.colXref.VisibleIndex = 25;
            // 
            // colRefSn
            // 
            this.colRefSn.Caption = "Ref Short Name";
            this.colRefSn.FieldName = "RefSn";
            this.colRefSn.Name = "colRefSn";
            this.colRefSn.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colRefSn.Visible = true;
            this.colRefSn.VisibleIndex = 26;
            // 
            // colInceptionDt
            // 
            this.colInceptionDt.Caption = "Inception Date";
            this.colInceptionDt.DisplayFormat.FormatString = "dd-MMM-yyyy";
            this.colInceptionDt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colInceptionDt.FieldName = "InceptionDt";
            this.colInceptionDt.Name = "colInceptionDt";
            this.colInceptionDt.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colInceptionDt.Visible = true;
            this.colInceptionDt.VisibleIndex = 27;
            // 
            // colOptnPutCallInd
            // 
            this.colOptnPutCallInd.Caption = "Optn Put/Call";
            this.colOptnPutCallInd.FieldName = "OptnPutCallInd";
            this.colOptnPutCallInd.Name = "colOptnPutCallInd";
            this.colOptnPutCallInd.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colOptnPutCallInd.Visible = true;
            this.colOptnPutCallInd.VisibleIndex = 28;
            // 
            // colOptnPremPrice
            // 
            this.colOptnPremPrice.Caption = "Optn Prem Price";
            this.colOptnPremPrice.FieldName = "OptnPremPrice ";
            this.colOptnPremPrice.Name = "colOptnPremPrice";
            this.colOptnPremPrice.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colOptnPremPrice.Visible = true;
            this.colOptnPremPrice.VisibleIndex = 29;
            // 
            // colOptnStrikePrice
            // 
            this.colOptnStrikePrice.Caption = "Optn Strike Price";
            this.colOptnStrikePrice.FieldName = "OptnStrikePrice";
            this.colOptnStrikePrice.Name = "colOptnStrikePrice";
            this.colOptnStrikePrice.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colOptnStrikePrice.Visible = true;
            this.colOptnStrikePrice.VisibleIndex = 30;
            // 
            // colProfitCenter
            // 
            this.colProfitCenter.Caption = "Profit Center";
            this.colProfitCenter.FieldName = "ProfitCenter";
            this.colProfitCenter.Name = "colProfitCenter";
            this.colProfitCenter.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colProfitCenter.Visible = true;
            this.colProfitCenter.VisibleIndex = 31;
            // 
            // colPermissionKey
            // 
            this.colPermissionKey.Caption = "Permission Key";
            this.colPermissionKey.FieldName = "PermissionKey";
            this.colPermissionKey.Name = "colPermissionKey";
            this.colPermissionKey.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colPermissionKey.Visible = true;
            this.colPermissionKey.VisibleIndex = 32;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnClose);
            this.panelControl1.Controls.Add(this.btnExcel);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(626, 40);
            this.panelControl1.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(534, 6);
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
            // ribbonControl1
            // 
            this.ribbonControl1.Dock = System.Windows.Forms.DockStyle.None;
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem});
            this.ribbonControl1.Location = new System.Drawing.Point(232, 12);
            this.ribbonControl1.MaxItemId = 1;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.Size = new System.Drawing.Size(24, 49);
            this.ribbonControl1.StatusBar = this.ribbonStatusBar1;
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 280);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.ribbonControl1;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(626, 23);
            // 
            // frmTradeDataChanges
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 303);
            this.Controls.Add(this.gridTradeDataChanges);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.ribbonControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "frmTradeDataChanges";
            this.ShowInTaskbar = false;
            this.Text = "Trade Data Changes";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTradeDataChanges_FormClosing);
            this.Load += new System.EventHandler(this.frmTradeDataChanges_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridTradeDataChanges)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTradeDataChanges)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion

      public DevExpress.XtraGrid.GridControl gridTradeDataChanges;
      private DevExpress.XtraGrid.Views.Grid.GridView gridViewTradeDataChanges;
      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraEditors.SimpleButton btnClose;
      private DevExpress.XtraEditors.SimpleButton btnExcel;
      private System.Windows.Forms.SaveFileDialog saveFileDialog;
      private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
      private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
      private DevExpress.XtraGrid.Columns.GridColumn colJnDatetime;
      private DevExpress.XtraGrid.Columns.GridColumn colBookingCoSn;
      private DevExpress.XtraGrid.Columns.GridColumn colTradeId;
      private DevExpress.XtraGrid.Columns.GridColumn colBuySellInd;
      private DevExpress.XtraGrid.Columns.GridColumn colTradeDt;
      private DevExpress.XtraGrid.Columns.GridColumn colCptySn;
      private DevExpress.XtraGrid.Columns.GridColumn colBrokerSn;
      private DevExpress.XtraGrid.Columns.GridColumn colTradeDesc;
      private DevExpress.XtraGrid.Columns.GridColumn colPriceDesc;
      private DevExpress.XtraGrid.Columns.GridColumn colTradeStatCode;
      private DevExpress.XtraGrid.Columns.GridColumn colLocationSn;
      private DevExpress.XtraGrid.Columns.GridColumn colQtyDesc;
      private DevExpress.XtraGrid.Columns.GridColumn colQtyTot;
      private DevExpress.XtraGrid.Columns.GridColumn colTradeTypeCode;
      private DevExpress.XtraGrid.Columns.GridColumn colCdtyCode;
      private DevExpress.XtraGrid.Columns.GridColumn colSttlType;
      private DevExpress.XtraGrid.Columns.GridColumn colBook;
      private DevExpress.XtraGrid.Columns.GridColumn colTransportDesc;
      private DevExpress.XtraGrid.Columns.GridColumn colCptyLegalName;
      private DevExpress.XtraGrid.Columns.GridColumn colBrokerLegalName;
      private DevExpress.XtraGrid.Columns.GridColumn colBrokerPrice;
      private DevExpress.XtraGrid.Columns.GridColumn colTrader;
      private DevExpress.XtraGrid.Columns.GridColumn colCdtyGrpCode;
      private DevExpress.XtraGrid.Columns.GridColumn colStartDt;
      private DevExpress.XtraGrid.Columns.GridColumn colEndDt;
      private DevExpress.XtraGrid.Columns.GridColumn colXref;
      private DevExpress.XtraGrid.Columns.GridColumn colRefSn;
      private DevExpress.XtraGrid.Columns.GridColumn colInceptionDt;
      private DevExpress.XtraGrid.Columns.GridColumn colOptnPutCallInd;
      private DevExpress.XtraGrid.Columns.GridColumn colOptnPremPrice;
      private DevExpress.XtraGrid.Columns.GridColumn colOptnStrikePrice;
      private DevExpress.XtraGrid.Columns.GridColumn colProfitCenter;
      private DevExpress.XtraGrid.Columns.GridColumn colPermissionKey;
   }
}