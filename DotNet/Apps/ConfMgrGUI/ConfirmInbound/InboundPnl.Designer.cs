using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;

namespace ConfirmInbound
{
    partial class InboundPnl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InboundPnl));
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem3 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem4 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem5 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem3 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem6 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.SuperToolTip superToolTip4 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem7 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem4 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip5 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem8 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem5 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem9 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.SuperToolTip superToolTip6 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem10 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem6 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem11 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.SuperToolTip superToolTip7 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem12 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem7 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem13 = new DevExpress.Utils.ToolTipTitleItem();
            this.pnlInbound = new DevExpress.XtraEditors.PanelControl();
            this.tabCntrlMain = new DevExpress.XtraTab.XtraTabControl();
            this.tabInboundQ = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabCntrlInboundDocs = new DevExpress.XtraTab.XtraTabControl();
            this.tabMatchedDocuments = new DevExpress.XtraTab.XtraTabPage();
            this.gridMatchedDocuments = new DevExpress.XtraGrid.GridControl();
            this.gridViewMatchedDocuments = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colAssDocId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colInboundDocsId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIndexVal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTradeRqmtId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTradeId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFileName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDocStatusCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAssociatedBy = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAssociatedDt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFinalApprovedBy = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFinalApprovedDt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDisputedBy = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDisputedDt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDiscardedBy = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridDiscardedDt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridVaultedBy = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridVaultedDt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridCdtyGroupCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCptyShortName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBrokerShortName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDocTypeCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSecondValidateReqFlag = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMultipleAssociatedDocs = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colXmitStatusCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colXmitValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSentTo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColTradeFinalApprovalFlag = new DevExpress.XtraGrid.Columns.GridColumn();
            this.Home = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.imageSmall = new System.Windows.Forms.ImageList();
            this.barBtnApprove = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnDispute = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnUnAssociate = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnFinalize = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnGrpMain = new DevExpress.XtraBars.BarButtonGroup();
            this.barBtnGrpMisc = new DevExpress.XtraBars.BarButtonGroup();
            this.barEditDocumentView = new DevExpress.XtraBars.BarEditItem();
            this.reposCmboDocsView = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.barBtnViewTradeSummaryRec = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnClearDocsFromGetAll = new DevExpress.XtraBars.BarButtonItem();
            this.barBtnApproveAndXmit = new DevExpress.XtraBars.BarButtonItem();
            this.barEditDefaultXmitDestination = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.Main = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.defaultToolTipController1 = new DevExpress.Utils.DefaultToolTipController();
            this.backgroundWorkerInbound = new System.ComponentModel.BackgroundWorker();
            this.popupMatchedDocs = new DevExpress.XtraBars.PopupMenu();
            this.colTrdSysCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTrdSysTicket = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pnlInbound)).BeginInit();
            this.pnlInbound.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tabCntrlMain)).BeginInit();
            this.tabCntrlMain.SuspendLayout();
            this.tabInboundQ.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabCntrlInboundDocs)).BeginInit();
            this.tabMatchedDocuments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMatchedDocuments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewMatchedDocuments)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Home)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reposCmboDocsView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMatchedDocs)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlInbound
            // 
            this.defaultToolTipController1.SetAllowHtmlText(this.pnlInbound, DevExpress.Utils.DefaultBoolean.Default);
            this.pnlInbound.Controls.Add(this.tabCntrlMain);
            this.pnlInbound.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInbound.Location = new System.Drawing.Point(0, 0);
            this.pnlInbound.Name = "pnlInbound";
            this.pnlInbound.Size = new System.Drawing.Size(952, 470);
            this.pnlInbound.TabIndex = 0;
            // 
            // tabCntrlMain
            // 
            this.tabCntrlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCntrlMain.Location = new System.Drawing.Point(3, 3);
            this.tabCntrlMain.Name = "tabCntrlMain";
            this.tabCntrlMain.SelectedTabPage = this.tabInboundQ;
            this.tabCntrlMain.Size = new System.Drawing.Size(946, 464);
            this.tabCntrlMain.TabIndex = 6;
            this.tabCntrlMain.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabInboundQ,
            this.tabMatchedDocuments});
            this.tabCntrlMain.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.tabCntrlMain_SelectedPageChanged);
            // 
            // tabInboundQ
            // 
            this.tabInboundQ.Controls.Add(this.xtraTabCntrlInboundDocs);
            this.tabInboundQ.Name = "tabInboundQ";
            this.tabInboundQ.Size = new System.Drawing.Size(944, 441);
            this.tabInboundQ.Text = "Inbound Documents";
            // 
            // xtraTabCntrlInboundDocs
            // 
            this.xtraTabCntrlInboundDocs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabCntrlInboundDocs.Location = new System.Drawing.Point(0, 0);
            this.xtraTabCntrlInboundDocs.Name = "xtraTabCntrlInboundDocs";
            this.xtraTabCntrlInboundDocs.Size = new System.Drawing.Size(944, 441);
            this.xtraTabCntrlInboundDocs.TabIndex = 1;
            this.xtraTabCntrlInboundDocs.SelectedPageChanged += new DevExpress.XtraTab.TabPageChangedEventHandler(this.xtraTabCntrlInboundDocs_SelectedPageChanged);
            // 
            // tabMatchedDocuments
            // 
            this.tabMatchedDocuments.Controls.Add(this.gridMatchedDocuments);
            this.tabMatchedDocuments.Controls.Add(this.Home);
            this.tabMatchedDocuments.Name = "tabMatchedDocuments";
            this.tabMatchedDocuments.Size = new System.Drawing.Size(944, 441);
            this.tabMatchedDocuments.Text = "Associated Documents";
            // 
            // gridMatchedDocuments
            // 
            this.gridMatchedDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMatchedDocuments.Location = new System.Drawing.Point(0, 118);
            this.gridMatchedDocuments.MainView = this.gridViewMatchedDocuments;
            this.gridMatchedDocuments.Name = "gridMatchedDocuments";
            this.gridMatchedDocuments.Size = new System.Drawing.Size(944, 323);
            this.gridMatchedDocuments.TabIndex = 0;
            this.gridMatchedDocuments.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewMatchedDocuments});
            this.gridMatchedDocuments.DoubleClick += new System.EventHandler(this.barEditDocumentView_EditValueChanged);
            // 
            // gridViewMatchedDocuments
            // 
            this.gridViewMatchedDocuments.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colAssDocId,
            this.colInboundDocsId,
            this.colTrdSysTicket,
            this.colTrdSysCode,
            this.colIndexVal,
            this.colTradeRqmtId,
            this.colTradeId,
            this.colFileName,
            this.colDocStatusCode,
            this.colAssociatedBy,
            this.colAssociatedDt,
            this.colFinalApprovedBy,
            this.colFinalApprovedDt,
            this.colDisputedBy,
            this.colDisputedDt,
            this.colDiscardedBy,
            this.gridDiscardedDt,
            this.gridVaultedBy,
            this.gridVaultedDt,
            this.gridCdtyGroupCode,
            this.colCptyShortName,
            this.colBrokerShortName,
            this.colDocTypeCode,
            this.colSecondValidateReqFlag,
            this.colMultipleAssociatedDocs,
            this.colXmitStatusCode,
            this.colXmitValue,
            this.colSentTo,
            this.gridColTradeFinalApprovalFlag});
            this.gridViewMatchedDocuments.CustomizationFormBounds = new System.Drawing.Rectangle(1020, 579, 208, 168);
            this.gridViewMatchedDocuments.GridControl = this.gridMatchedDocuments;
            this.gridViewMatchedDocuments.Name = "gridViewMatchedDocuments";
            this.gridViewMatchedDocuments.OptionsBehavior.Editable = false;
            this.gridViewMatchedDocuments.OptionsLayout.Columns.StoreAllOptions = true;
            this.gridViewMatchedDocuments.OptionsLayout.Columns.StoreAppearance = true;
            this.gridViewMatchedDocuments.OptionsLayout.StoreAllOptions = true;
            this.gridViewMatchedDocuments.OptionsLayout.StoreAppearance = true;
            this.gridViewMatchedDocuments.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewMatchedDocuments.OptionsSelection.MultiSelect = true;
            this.gridViewMatchedDocuments.OptionsView.ColumnAutoWidth = false;
            this.gridViewMatchedDocuments.OptionsView.ShowGroupPanel = false;
            this.gridViewMatchedDocuments.ShowGridMenu += new DevExpress.XtraGrid.Views.Grid.GridMenuEventHandler(this.gridViewMatchedDocuments_ShowGridMenu);
            this.gridViewMatchedDocuments.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewMatchedDocuments_FocusedRowChanged);
            this.gridViewMatchedDocuments.ColumnFilterChanged += new System.EventHandler(this.gridViewMatchedDocuments_ColumnFilterChanged);
            this.gridViewMatchedDocuments.DoubleClick += new System.EventHandler(this.gridViewMatchedDocuments_DoubleClick);
            this.gridViewMatchedDocuments.DataSourceChanged += new System.EventHandler(this.gridViewMatchedDocuments_DataSourceChanged);
            // 
            // colAssDocId
            // 
            this.colAssDocId.AppearanceHeader.Options.UseTextOptions = true;
            this.colAssDocId.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAssDocId.Caption = "Matched Document Id";
            this.colAssDocId.FieldName = "Id";
            this.colAssDocId.Name = "colAssDocId";
            // 
            // colInboundDocsId
            // 
            this.colInboundDocsId.AppearanceHeader.Options.UseTextOptions = true;
            this.colInboundDocsId.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colInboundDocsId.Caption = "Inbound Doc Id";
            this.colInboundDocsId.FieldName = "InboundDocsId";
            this.colInboundDocsId.Name = "colInboundDocsId";
            // 
            // colIndexVal
            // 
            this.colIndexVal.AppearanceHeader.Options.UseTextOptions = true;
            this.colIndexVal.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colIndexVal.Caption = "Index Val";
            this.colIndexVal.FieldName = "IndexVal";
            this.colIndexVal.Name = "colIndexVal";
            // 
            // colTradeRqmtId
            // 
            this.colTradeRqmtId.AppearanceHeader.Options.UseTextOptions = true;
            this.colTradeRqmtId.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colTradeRqmtId.Caption = "Trade Rqmt Id";
            this.colTradeRqmtId.FieldName = "TradeRqmtId";
            this.colTradeRqmtId.Name = "colTradeRqmtId";
            // 
            // colTradeId
            // 
            this.colTradeId.AppearanceHeader.Options.UseTextOptions = true;
            this.colTradeId.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colTradeId.Caption = "Trade Id";
            this.colTradeId.FieldName = "TradeId";
            this.colTradeId.Name = "colTradeId";
            this.colTradeId.Visible = true;
            this.colTradeId.VisibleIndex = 0;
            // 
            // colFileName
            // 
            this.colFileName.AppearanceHeader.Options.UseTextOptions = true;
            this.colFileName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colFileName.Caption = "File Name";
            this.colFileName.FieldName = "FileName";
            this.colFileName.Name = "colFileName";
            // 
            // colDocStatusCode
            // 
            this.colDocStatusCode.AppearanceHeader.Options.UseTextOptions = true;
            this.colDocStatusCode.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDocStatusCode.Caption = "Doc Status Code";
            this.colDocStatusCode.FieldName = "DocStatusCode";
            this.colDocStatusCode.Name = "colDocStatusCode";
            this.colDocStatusCode.Visible = true;
            this.colDocStatusCode.VisibleIndex = 4;
            // 
            // colAssociatedBy
            // 
            this.colAssociatedBy.AppearanceHeader.Options.UseTextOptions = true;
            this.colAssociatedBy.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAssociatedBy.Caption = "Associated By";
            this.colAssociatedBy.FieldName = "AssociatedBy";
            this.colAssociatedBy.Name = "colAssociatedBy";
            // 
            // colAssociatedDt
            // 
            this.colAssociatedDt.AppearanceHeader.Options.UseTextOptions = true;
            this.colAssociatedDt.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colAssociatedDt.Caption = "Associated Date";
            this.colAssociatedDt.DisplayFormat.FormatString = "G";
            this.colAssociatedDt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colAssociatedDt.FieldName = "AssociatedDt";
            this.colAssociatedDt.Name = "colAssociatedDt";
            // 
            // colFinalApprovedBy
            // 
            this.colFinalApprovedBy.AppearanceHeader.Options.UseTextOptions = true;
            this.colFinalApprovedBy.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colFinalApprovedBy.Caption = "Approved By";
            this.colFinalApprovedBy.FieldName = "FinalApprovedBy";
            this.colFinalApprovedBy.Name = "colFinalApprovedBy";
            // 
            // colFinalApprovedDt
            // 
            this.colFinalApprovedDt.AppearanceHeader.Options.UseTextOptions = true;
            this.colFinalApprovedDt.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colFinalApprovedDt.Caption = "Approved Date";
            this.colFinalApprovedDt.DisplayFormat.FormatString = "G";
            this.colFinalApprovedDt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colFinalApprovedDt.FieldName = "FinalApprovedDt";
            this.colFinalApprovedDt.Name = "colFinalApprovedDt";
            // 
            // colDisputedBy
            // 
            this.colDisputedBy.AppearanceHeader.Options.UseTextOptions = true;
            this.colDisputedBy.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDisputedBy.Caption = "Disputed By";
            this.colDisputedBy.FieldName = "DisputedBy";
            this.colDisputedBy.Name = "colDisputedBy";
            // 
            // colDisputedDt
            // 
            this.colDisputedDt.AppearanceHeader.Options.UseTextOptions = true;
            this.colDisputedDt.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDisputedDt.Caption = "Disputed Date";
            this.colDisputedDt.DisplayFormat.FormatString = "G";
            this.colDisputedDt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colDisputedDt.FieldName = "DisputedDt";
            this.colDisputedDt.Name = "colDisputedDt";
            // 
            // colDiscardedBy
            // 
            this.colDiscardedBy.AppearanceHeader.Options.UseTextOptions = true;
            this.colDiscardedBy.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDiscardedBy.Caption = "Discarded By";
            this.colDiscardedBy.FieldName = "DiscardedBy";
            this.colDiscardedBy.Name = "colDiscardedBy";
            // 
            // gridDiscardedDt
            // 
            this.gridDiscardedDt.AppearanceHeader.Options.UseTextOptions = true;
            this.gridDiscardedDt.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridDiscardedDt.Caption = "Discarded Dt";
            this.gridDiscardedDt.DisplayFormat.FormatString = "G";
            this.gridDiscardedDt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridDiscardedDt.FieldName = "DiscardedDt";
            this.gridDiscardedDt.Name = "gridDiscardedDt";
            // 
            // gridVaultedBy
            // 
            this.gridVaultedBy.AppearanceHeader.Options.UseTextOptions = true;
            this.gridVaultedBy.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridVaultedBy.Caption = "Vaulted By";
            this.gridVaultedBy.FieldName = "VaultedBy";
            this.gridVaultedBy.Name = "gridVaultedBy";
            // 
            // gridVaultedDt
            // 
            this.gridVaultedDt.AppearanceHeader.Options.UseTextOptions = true;
            this.gridVaultedDt.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridVaultedDt.Caption = "Vaulted Date";
            this.gridVaultedDt.DisplayFormat.FormatString = "G";
            this.gridVaultedDt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridVaultedDt.FieldName = "VaultedDt";
            this.gridVaultedDt.Name = "gridVaultedDt";
            // 
            // gridCdtyGroupCode
            // 
            this.gridCdtyGroupCode.AppearanceHeader.Options.UseTextOptions = true;
            this.gridCdtyGroupCode.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridCdtyGroupCode.Caption = "Cdty Group Code";
            this.gridCdtyGroupCode.FieldName = "CdtyGroupCode";
            this.gridCdtyGroupCode.Name = "gridCdtyGroupCode";
            this.gridCdtyGroupCode.Visible = true;
            this.gridCdtyGroupCode.VisibleIndex = 7;
            // 
            // colCptyShortName
            // 
            this.colCptyShortName.AppearanceHeader.Options.UseTextOptions = true;
            this.colCptyShortName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCptyShortName.Caption = "Cpty Short Name";
            this.colCptyShortName.FieldName = "CptyShortName";
            this.colCptyShortName.Name = "colCptyShortName";
            this.colCptyShortName.Visible = true;
            this.colCptyShortName.VisibleIndex = 5;
            // 
            // colBrokerShortName
            // 
            this.colBrokerShortName.AppearanceHeader.Options.UseTextOptions = true;
            this.colBrokerShortName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colBrokerShortName.Caption = "Broker Short Name";
            this.colBrokerShortName.FieldName = "BrokerShortName";
            this.colBrokerShortName.Name = "colBrokerShortName";
            this.colBrokerShortName.Visible = true;
            this.colBrokerShortName.VisibleIndex = 6;
            // 
            // colDocTypeCode
            // 
            this.colDocTypeCode.AppearanceHeader.Options.UseTextOptions = true;
            this.colDocTypeCode.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDocTypeCode.Caption = "Rqmt";
            this.colDocTypeCode.FieldName = "DocTypeCode";
            this.colDocTypeCode.Name = "colDocTypeCode";
            this.colDocTypeCode.Visible = true;
            this.colDocTypeCode.VisibleIndex = 1;
            // 
            // colSecondValidateReqFlag
            // 
            this.colSecondValidateReqFlag.AppearanceHeader.Options.UseTextOptions = true;
            this.colSecondValidateReqFlag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colSecondValidateReqFlag.Caption = "Second Validate Flag";
            this.colSecondValidateReqFlag.FieldName = "SecondValidateReqFlag";
            this.colSecondValidateReqFlag.Name = "colSecondValidateReqFlag";
            // 
            // colMultipleAssociatedDocs
            // 
            this.colMultipleAssociatedDocs.AppearanceHeader.Options.UseTextOptions = true;
            this.colMultipleAssociatedDocs.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colMultipleAssociatedDocs.Caption = "Multiple Associated Docs";
            this.colMultipleAssociatedDocs.FieldName = "MultipleAssociatedDocs";
            this.colMultipleAssociatedDocs.Name = "colMultipleAssociatedDocs";
            // 
            // colXmitStatusCode
            // 
            this.colXmitStatusCode.Caption = "XMIT Status";
            this.colXmitStatusCode.FieldName = "XmitStatusCode";
            this.colXmitStatusCode.Name = "colXmitStatusCode";
            this.colXmitStatusCode.Visible = true;
            this.colXmitStatusCode.VisibleIndex = 8;
            // 
            // colXmitValue
            // 
            this.colXmitValue.Caption = "XMIT Value";
            this.colXmitValue.FieldName = "XmitValue";
            this.colXmitValue.Name = "colXmitValue";
            this.colXmitValue.Visible = true;
            this.colXmitValue.VisibleIndex = 9;
            // 
            // colSentTo
            // 
            this.colSentTo.AppearanceHeader.Options.UseTextOptions = true;
            this.colSentTo.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colSentTo.Caption = "Sent To";
            this.colSentTo.FieldName = "SentTo";
            this.colSentTo.Name = "colSentTo";
            this.colSentTo.Visible = true;
            this.colSentTo.VisibleIndex = 10;
            // 
            // gridColTradeFinalApprovalFlag
            // 
            this.gridColTradeFinalApprovalFlag.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColTradeFinalApprovalFlag.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColTradeFinalApprovalFlag.Caption = "Trade Final Approval Flag";
            this.gridColTradeFinalApprovalFlag.FieldName = "TradeFinalApprovalFlag";
            this.gridColTradeFinalApprovalFlag.Name = "gridColTradeFinalApprovalFlag";
            this.gridColTradeFinalApprovalFlag.Visible = true;
            this.gridColTradeFinalApprovalFlag.VisibleIndex = 11;
            // 
            // Home
            // 
            this.Home.ExpandCollapseItem.Id = 0;
            this.Home.Images = this.imageSmall;
            this.Home.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.Home.ExpandCollapseItem,
            this.barBtnApprove,
            this.barBtnDispute,
            this.barBtnUnAssociate,
            this.barBtnFinalize,
            this.barBtnGrpMain,
            this.barBtnGrpMisc,
            this.barEditDocumentView,
            this.barBtnViewTradeSummaryRec,
            this.barBtnClearDocsFromGetAll,
            this.barBtnApproveAndXmit,
            this.barEditDefaultXmitDestination});
            this.Home.Location = new System.Drawing.Point(0, 0);
            this.Home.MaxItemId = 17;
            this.Home.Name = "Home";
            this.Home.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.Home.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.reposCmboDocsView,
            this.repositoryItemButtonEdit1,
            this.repositoryItemTextEdit1});
            this.Home.ShowCategoryInCaption = false;
            this.Home.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.Home.ShowToolbarCustomizeItem = false;
            this.Home.Size = new System.Drawing.Size(944, 118);
            this.Home.Toolbar.ItemLinks.Add(this.barEditDocumentView);
            this.Home.Toolbar.ItemLinks.Add(this.barBtnGrpMain, true);
            this.Home.Toolbar.ItemLinks.Add(this.barEditDefaultXmitDestination);
            this.Home.Toolbar.ItemLinks.Add(this.barBtnGrpMisc);
            this.Home.Toolbar.ShowCustomizeItem = false;
            this.Home.ToolTipController = this.defaultToolTipController1.DefaultController;
            this.Home.ShowCustomizationMenu += new DevExpress.XtraBars.Ribbon.RibbonCustomizationMenuEventHandler(this.Home_ShowCustomizationMenu);
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
            this.imageSmall.Images.SetKeyName(19, "arrow_switch.png");
            // 
            // barBtnApprove
            // 
            this.barBtnApprove.Caption = "Approve";
            this.barBtnApprove.Id = 0;
            this.barBtnApprove.ImageIndex = 3;
            this.barBtnApprove.Name = "barBtnApprove";
            this.barBtnApprove.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText;
            toolTipTitleItem1.Text = "Approve a Matched Inbound Document";
            toolTipItem1.LeftIndent = 6;
            toolTipTitleItem2.LeftIndent = 6;
            toolTipTitleItem2.Text = "This action will Invoke the Trade Requirement editor";
            superToolTip1.Items.Add(toolTipTitleItem1);
            superToolTip1.Items.Add(toolTipItem1);
            superToolTip1.Items.Add(toolTipTitleItem2);
            this.barBtnApprove.SuperTip = superToolTip1;
            this.barBtnApprove.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnApprove_ItemClick);
            // 
            // barBtnDispute
            // 
            this.barBtnDispute.Caption = "Dispute";
            this.barBtnDispute.Id = 1;
            this.barBtnDispute.ImageIndex = 1;
            this.barBtnDispute.Name = "barBtnDispute";
            this.barBtnDispute.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText;
            toolTipTitleItem3.Text = "Disputing a Matched Document";
            toolTipItem2.LeftIndent = 6;
            toolTipTitleItem4.LeftIndent = 6;
            toolTipTitleItem4.Text = "This action will Invoke the Trade Requirement editor";
            superToolTip2.Items.Add(toolTipTitleItem3);
            superToolTip2.Items.Add(toolTipItem2);
            superToolTip2.Items.Add(toolTipTitleItem4);
            this.barBtnDispute.SuperTip = superToolTip2;
            this.barBtnDispute.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnDispute_ItemClick);
            // 
            // barBtnUnAssociate
            // 
            this.barBtnUnAssociate.Caption = "Un-Associate";
            this.barBtnUnAssociate.Id = 2;
            this.barBtnUnAssociate.ImageIndex = 2;
            this.barBtnUnAssociate.Name = "barBtnUnAssociate";
            this.barBtnUnAssociate.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText;
            toolTipTitleItem5.Text = "Un-Associated a Matched Inbound Document";
            toolTipItem3.LeftIndent = 6;
            toolTipItem3.Text = "Allows the user to Un-Associate a matched Inbound Document.  The document could h" +
    "ave been Auto Matched (Associated)  by back end processing, or manually matched " +
    "from the Inbound Queue.";
            toolTipTitleItem6.LeftIndent = 6;
            toolTipTitleItem6.Text = "This action will Invoke the Trade Requirement editor";
            superToolTip3.Items.Add(toolTipTitleItem5);
            superToolTip3.Items.Add(toolTipItem3);
            superToolTip3.Items.Add(toolTipTitleItem6);
            this.barBtnUnAssociate.SuperTip = superToolTip3;
            this.barBtnUnAssociate.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnUnAssociate_ItemClick);
            // 
            // barBtnFinalize
            // 
            this.barBtnFinalize.Caption = "Send";
            this.barBtnFinalize.Description = "Sehd";
            this.barBtnFinalize.Id = 5;
            this.barBtnFinalize.Name = "barBtnFinalize";
            this.barBtnFinalize.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.SmallWithText;
            toolTipTitleItem7.Text = "Send";
            toolTipItem4.LeftIndent = 6;
            toolTipItem4.Text = "Transmit the selected document to the destination email address or fax number in " +
    "the box to the left. If no destination is specified the user will be prompted ";
            superToolTip4.Items.Add(toolTipTitleItem7);
            superToolTip4.Items.Add(toolTipItem4);
            this.barBtnFinalize.SuperTip = superToolTip4;
            this.barBtnFinalize.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnTransmit_ItemClick);
            // 
            // barBtnGrpMain
            // 
            this.barBtnGrpMain.Caption = "Main";
            this.barBtnGrpMain.Id = 7;
            this.barBtnGrpMain.ItemLinks.Add(this.barBtnApprove);
            this.barBtnGrpMain.ItemLinks.Add(this.barBtnDispute);
            this.barBtnGrpMain.ItemLinks.Add(this.barBtnUnAssociate);
            this.barBtnGrpMain.Name = "barBtnGrpMain";
            // 
            // barBtnGrpMisc
            // 
            this.barBtnGrpMisc.Caption = "Misc";
            this.barBtnGrpMisc.Id = 8;
            this.barBtnGrpMisc.ItemLinks.Add(this.barBtnFinalize);
            this.barBtnGrpMisc.Name = "barBtnGrpMisc";
            // 
            // barEditDocumentView
            // 
            this.barEditDocumentView.Caption = "Document View:";
            this.barEditDocumentView.Edit = this.reposCmboDocsView;
            this.barEditDocumentView.Id = 9;
            this.barEditDocumentView.ImageIndex = 9;
            this.barEditDocumentView.Name = "barEditDocumentView";
            toolTipTitleItem8.Text = "Change Matched Document View";
            toolTipItem5.LeftIndent = 6;
            toolTipItem5.Text = "Allows the user to filter the Associated Documents data by the several pre-define" +
    "d filters displayed in the list box.";
            toolTipTitleItem9.LeftIndent = 6;
            toolTipTitleItem9.Text = "Existing filters can have an impact on what is currently displayed.  Be sure to s" +
    "ee if your grid has any filters applied already.";
            superToolTip5.Items.Add(toolTipTitleItem8);
            superToolTip5.Items.Add(toolTipItem5);
            superToolTip5.Items.Add(toolTipTitleItem9);
            this.barEditDocumentView.SuperTip = superToolTip5;
            this.barEditDocumentView.Width = 125;
            this.barEditDocumentView.EditValueChanged += new System.EventHandler(this.barEditDocumentView_EditValueChanged);
            // 
            // reposCmboDocsView
            // 
            this.reposCmboDocsView.AutoHeight = false;
            this.reposCmboDocsView.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.reposCmboDocsView.Items.AddRange(new object[] {
            "All",
            "Approved",
            "Disputed"});
            this.reposCmboDocsView.Name = "reposCmboDocsView";
            // 
            // barBtnViewTradeSummaryRec
            // 
            this.barBtnViewTradeSummaryRec.Caption = "View Trade Summary Record";
            this.barBtnViewTradeSummaryRec.Id = 11;
            this.barBtnViewTradeSummaryRec.ImageIndex = 10;
            this.barBtnViewTradeSummaryRec.Name = "barBtnViewTradeSummaryRec";
            toolTipTitleItem10.Text = "View Trade Summary Record";
            toolTipItem6.LeftIndent = 6;
            toolTipItem6.Text = "Invoking this menu item will cause the Trade Summary and Trade Requirement record" +
    " to be selected in the main OpsManager trade data grid.";
            toolTipTitleItem11.LeftIndent = 6;
            toolTipTitleItem11.Text = "Always verify that the correct trade and trade requirement are selected";
            superToolTip6.Items.Add(toolTipTitleItem10);
            superToolTip6.Items.Add(toolTipItem6);
            superToolTip6.Items.Add(toolTipTitleItem11);
            this.barBtnViewTradeSummaryRec.SuperTip = superToolTip6;
            this.barBtnViewTradeSummaryRec.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnViewTradeSummaryRec_ItemClick);
            // 
            // barBtnClearDocsFromGetAll
            // 
            this.barBtnClearDocsFromGetAll.Caption = "Clear Docs from GetAll()";
            this.barBtnClearDocsFromGetAll.Id = 12;
            this.barBtnClearDocsFromGetAll.ImageIndex = 11;
            this.barBtnClearDocsFromGetAll.Name = "barBtnClearDocsFromGetAll";
            toolTipTitleItem12.Text = "Clear Docs from GetAll()";
            toolTipItem7.LeftIndent = 6;
            toolTipItem7.Text = "If the user enters a GetAll query search to pull documents back into OpsManager, " +
    "Invoking this menu item will clear those results in the Inbound Matched Document" +
    "s tab.";
            toolTipTitleItem13.LeftIndent = 6;
            toolTipTitleItem13.Text = "A Future release will provide specific filtering as in the trade summary grid";
            superToolTip7.Items.Add(toolTipTitleItem12);
            superToolTip7.Items.Add(toolTipItem7);
            superToolTip7.Items.Add(toolTipTitleItem13);
            this.barBtnClearDocsFromGetAll.SuperTip = superToolTip7;
            this.barBtnClearDocsFromGetAll.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnClearDocsFromGetAll_ItemClick);
            // 
            // barBtnApproveAndXmit
            // 
            this.barBtnApproveAndXmit.Caption = "Approve and Transmit";
            this.barBtnApproveAndXmit.Id = 13;
            this.barBtnApproveAndXmit.ImageIndex = 12;
            this.barBtnApproveAndXmit.Name = "barBtnApproveAndXmit";
            this.barBtnApproveAndXmit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barBtnApproveAndXmit_ItemClick);
            // 
            // barEditDefaultXmitDestination
            // 
            this.barEditDefaultXmitDestination.Caption = "XMIT #:";
            this.barEditDefaultXmitDestination.Edit = this.repositoryItemTextEdit1;
            this.barEditDefaultXmitDestination.Id = 15;
            this.barEditDefaultXmitDestination.Name = "barEditDefaultXmitDestination";
            this.barEditDefaultXmitDestination.Width = 100;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.Main});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "ribbonPage1";
            this.ribbonPage1.Visible = false;
            // 
            // Main
            // 
            this.Main.ItemLinks.Add(this.barBtnDispute, true);
            this.Main.ItemLinks.Add(this.barBtnApprove, true);
            this.Main.ItemLinks.Add(this.barBtnUnAssociate, true);
            this.Main.ItemLinks.Add(this.barBtnFinalize, true);
            this.Main.ItemLinks.Add(this.barEditDocumentView);
            this.Main.Name = "Main";
            this.Main.Text = "Main";
            // 
            // repositoryItemButtonEdit1
            // 
            this.repositoryItemButtonEdit1.AutoHeight = false;
            this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
            // 
            // defaultToolTipController1
            // 
            // 
            // 
            // 
            this.defaultToolTipController1.DefaultController.ToolTipType = DevExpress.Utils.ToolTipType.SuperTip;
            // 
            // backgroundWorkerInbound
            // 
            this.backgroundWorkerInbound.WorkerReportsProgress = true;
            this.backgroundWorkerInbound.WorkerSupportsCancellation = true;
            this.backgroundWorkerInbound.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerInbound_DoWork);
            this.backgroundWorkerInbound.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerInbound_ProgressChanged);
            this.backgroundWorkerInbound.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerInbound_RunWorkerCompleted);
            // 
            // popupMatchedDocs
            // 
            this.popupMatchedDocs.ItemLinks.Add(this.barBtnApproveAndXmit, true);
            this.popupMatchedDocs.ItemLinks.Add(this.barBtnViewTradeSummaryRec, true);
            this.popupMatchedDocs.ItemLinks.Add(this.barBtnClearDocsFromGetAll);
            this.popupMatchedDocs.Name = "popupMatchedDocs";
            this.popupMatchedDocs.Ribbon = this.Home;
            // 
            // colTrdSysCode
            // 
            this.colTrdSysCode.Caption = "Trading System Code";
            this.colTrdSysCode.FieldName = "TrdSysCode";
            this.colTrdSysCode.Name = "colTrdSysCode";
            this.colTrdSysCode.Visible = true;
            this.colTrdSysCode.VisibleIndex = 3;
            // 
            // colTrdSysTicket
            // 
            this.colTrdSysTicket.Caption = "Ticket";
            this.colTrdSysTicket.FieldName = "TrdSysTicket";
            this.colTrdSysTicket.Name = "colTrdSysTicket";
            this.colTrdSysTicket.Visible = true;
            this.colTrdSysTicket.VisibleIndex = 2;
            // 
            // InboundPnl
            // 
            this.defaultToolTipController1.SetAllowHtmlText(this, DevExpress.Utils.DefaultBoolean.Default);
            this.Controls.Add(this.pnlInbound);
            this.Name = "InboundPnl";
            this.Size = new System.Drawing.Size(952, 470);
            ((System.ComponentModel.ISupportInitialize)(this.pnlInbound)).EndInit();
            this.pnlInbound.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tabCntrlMain)).EndInit();
            this.tabCntrlMain.ResumeLayout(false);
            this.tabInboundQ.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabCntrlInboundDocs)).EndInit();
            this.tabMatchedDocuments.ResumeLayout(false);
            this.tabMatchedDocuments.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMatchedDocuments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewMatchedDocuments)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Home)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reposCmboDocsView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMatchedDocs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private PanelControl pnlInbound;
        public XtraTabControl tabCntrlMain;
        private XtraTabPage tabInboundQ;
        private XtraTabPage tabMatchedDocuments;
        private XtraTabControl xtraTabCntrlInboundDocs;
        private InboundQTabPnll defaultInboundPanel;
        private InboundQTabPnll discardedInboundPanel;        
        private InboundQTabPnll ignoredDocsPanel;
        private GridControl gridMatchedDocuments;
        private GridView gridViewMatchedDocuments;
        private GridColumn colAssDocId;
        private GridColumn colInboundDocsId;
        private GridColumn colIndexVal;
        private GridColumn colTradeRqmtId;
        private GridColumn colTradeId;
        private GridColumn colFileName;
        private GridColumn colDocStatusCode;
        private GridColumn colAssociatedBy;
        private GridColumn colAssociatedDt;
        private GridColumn colFinalApprovedBy;
        private GridColumn colFinalApprovedDt;
        private GridColumn colDisputedBy;
        private GridColumn colDisputedDt;
        private GridColumn colDiscardedBy;
        private GridColumn gridDiscardedDt;
        private GridColumn gridVaultedBy;
        private GridColumn gridVaultedDt;
        private GridColumn gridCdtyGroupCode;
        private GridColumn colCptyShortName;
        private GridColumn colBrokerShortName;
        private GridColumn colDocTypeCode;
        private GridColumn colSecondValidateReqFlag;
        private GridColumn colMultipleAssociatedDocs;
        private BackgroundWorker backgroundWorkerInbound;
        private PopupMenu popupMatchedDocs;
        private GridColumn colXmitStatusCode;
        private GridColumn colXmitValue;
        private GridColumn colSentTo;
        private GridColumn gridColTradeFinalApprovalFlag;
        private DefaultToolTipController defaultToolTipController1;
        private RibbonControl Home;
        private RibbonPage ribbonPage1;
        private RibbonPageGroup Main;
        private BarButtonItem barBtnApprove;
        private BarButtonItem barBtnDispute;
        private BarButtonItem barBtnUnAssociate;
        private ImageList imageSmall;
        private BarButtonItem barBtnFinalize;
        private BarButtonGroup barBtnGrpMain;
        private BarButtonGroup barBtnGrpMisc;
        private BarEditItem barEditDocumentView;
        private RepositoryItemComboBox reposCmboDocsView;
        private BarButtonItem barBtnViewTradeSummaryRec;
        private BarButtonItem barBtnClearDocsFromGetAll;
        private BarButtonItem barBtnApproveAndXmit;
        private BarEditItem barEditDefaultXmitDestination;
        private RepositoryItemTextEdit repositoryItemTextEdit1;
        private RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private GridColumn colTrdSysTicket;
        private GridColumn colTrdSysCode;
    }
}
