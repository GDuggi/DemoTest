using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPivotGrid;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using NSRMCommon;
using NSRMLogging;
using System.Linq;
using DevExpress.XtraWaitForm;
using System.Data;
using com.amphora.cayenne.entity.support;
using System.IO;
using System.Configuration;
using System.Globalization;
using com.amphora.cayenne.entity;
using com.amphora.cayenne.entity.service;
using DevExpress.Data.PivotGrid;


namespace NSRiskMgrCtrls {
    public partial class InspectablePivotGrid : XtraUserControl {
        #region delegates
        public delegate void PageChangedDelegate(object sender,TabPageWithUomPivot newPage);
        #endregion

        #region events
        public event ExpandedInspectorHandler InspectorExpanded;
        public event PageChangedDelegate SelectedPageChanged;
        public event TabRenameHandler TabRenamed;
        delegate void RemovePositionPivotProgressPanelHandler(ProgressPanel pp, InspectablePivotGrid PositionPivotGrid);
        delegate void AddPositionPivotGridProgressPanelToHandler( ProgressPanel ppanel, InspectablePivotGrid PositionPivotGrid);
        ProgressPanel positionPivotGridProgressPanel;

        #endregion

        #region fields
        bool expanded = false;
        //bool isFromFetchOrDistribution;
        bool requiresTweak;
        #endregion

        #region ctor
        public InspectablePivotGrid() {
            InitializeComponent();
        }
        #endregion

        #region properties
        public Type pivotDataType { get; set; }
        public IPivotDataProvider dataProvider { get; set; }
        public XtraTabControl tabControl { get { return this.xtraTabControl1; } }
        public IUomRequester uomRequester { get; set; }
        public int lowerSplitPosition { get { return this.scPivotAndLower.SplitterPosition; } set { this.scPivotAndLower.SplitterPosition = value; } }
        public int pnlSplitPosition { get { return this.scPnlGrids.SplitterPosition; } set { this.scPnlGrids.SplitterPosition = value; } }
        #endregion

        public List<RiskGroup> MainDataSource = new List<RiskGroup>();
        public int NumPositionsOnScreen { get; set; }
        public int NumRealPortfoliosOnScreen { get; set; }

        #region public methods


        
        public void clearAllDataSources()
        {
            MainDataSource.Clear();

            foreach (XtraTabPage xtraTabPage in this.tabControl.TabPages)
            {
                if (xtraTabPage is TabPageWithUomPivot)
                {
                    TabPageWithUomPivot uomPivotPage = ((TabPageWithUomPivot)xtraTabPage);

                    uomPivotPage.clearAllDataSources();
                }
            }
            
        }

       
        public void refreshCurrentTabPage() 
        {
            TabPageWithUomPivot page;

            page = this.xtraTabControl1.SelectedTabPage as TabPageWithUomPivot;

            if (page != null)
                page.RefreshDataForTab();
        }

        public TabPageWithUomPivot selectedPage() {


            if (xtraTabControl1.SelectedTabPage != null &&
                xtraTabControl1.SelectedTabPage is TabPageWithUomPivot)
                return this.xtraTabControl1.SelectedTabPage as TabPageWithUomPivot;

            return null;
        }

        public void indicatePivotFieldTweakRequired() {
            this.requiresTweak = true;
        }

        public void removeSelection()
        {
            if (scPivotAndLower.Panel2.Size.Height > 0)
            {
                clearDataSource(distributionsGridControl);
                clearDataSource(PositionsGridControl);
                clearDataSource(InventoryBuildDrawGridControl);
                clearDataSource(PnLGridControl);
                clearDataSource(this.HistoricalPnLGridControl);
            }
        }

        public void setPLRecords(IEnumerable<IPLRecord> plRecords) {
            setDataSource(PnLGridControl,NormalPnlRecord.generateDatasource(plRecords),true);
            setDataSource(HistoricalPnLGridControl,HistoricalPnlRecord.generateDatasource(plRecords));
        }

        public void updateWindowStateDefinitions(WrappedWinDef windef) {
            TabPageWithUomPivot tabPage;
            int index;

            foreach (WrappedWinPivotDef pivotDefinition in windef.allPivotDefinitions())
                if ((tabPage = findTabNamed(pivotDefinition.tabName)) != null) 
                {
                    
                    tabPage.updateWindowStateDefinition(pivotDefinition);

                    index = this.xtraTabControl1.TabPages.IndexOf(tabPage);

                    //always check if value has checked to avoid an extra "dirty" event
                    if (index >= 0 && index != pivotDefinition.tabIndex)
                        pivotDefinition.tabIndex = index;
                }
             
        }

        

        public void restoreSerializedTabs(WrappedWinDef windef,bool force) {
            string tabName;

            if (force)
                indicatePivotFieldTweakRequired();
            foreach (WrappedWinPivotDef adef in windef.allPivotDefinitions()) 
            {
                tabName = adef.tabName;
                var tab = createEmptyTabPage(this.tabControl,string.IsNullOrEmpty(tabName) ? WrappedWinPivotDef.NULL_PIVOT_NAME : tabName);
                if (force)
                    tab.pivotGrid.ForceInitialize();
                tab.updateFromPivotDef(adef);
            }
            if (this.xtraTabControl1 != null && this.xtraTabControl1.TabPages != null && this.xtraTabControl1.TabPages.Count > 0)
            {
                this.xtraTabControl1.SelectedTabPage = null;
                this.xtraTabControl1.SelectedTabPage = this.xtraTabControl1.TabPages[0];
            }
        }
        #endregion

        #region utility methods

        void addFields(TabPageWithUomPivot mtp,List<PivotGridField> list,string[] fields) {
            PivotGridField pgfFound;

            mtp.pivotGrid.BeginUpdate();
            foreach (PivotGridField pgf in list) 
            {
                mtp.pivotGrid.Fields.Add(pgf);
                if (fields != null &&
                    Array.IndexOf<string>(fields, pgf.Caption) >= 0 &&
                    (pgfFound = mtp.pivotGrid.Fields.GetFieldByName(pgf.Name)) != null)
                {
                    pgfFound.Visible = true;
                    if (requiresTweak && string.Compare(pgf.Caption, "period", true) == 0)
                        pgf.Area = PivotArea.RowArea;
                }
            }
            mtp.pivotGrid.EndUpdate();
        }

        void showUI() {
            var avar3 = this.amphoraFieldSelector1.PreferredSize;
            var avar4 = this.amphoraFieldSelector1.GetPreferredSize(avar3);

            if (expanded)
            {
                this.btnExpand.Text = ">>";
                this.scTabAndSelector.SplitterDistance = scTabAndSelector.Width - (avar4.Width + this.scTabAndSelector.SplitterWidth + 30);
            } else {
                this.btnExpand.Text = "<<";
                this.scTabAndSelector.SplitterDistance = scTabAndSelector.ClientSize.Width;
            }
            btnExpand.Width = avar4.Width  + 30;
            btnExpand.SetBounds(ClientSize.Width - avar4.Width - 35,0,0,0,BoundsSpecified.X);
        }

        #endregion

        #region overridden methods

        protected override void OnCreateControl() {
            base.OnCreateControl();
            renameEditor.Parent = xtraTabControl1;
            renameEditor.BackColor = LookAndFeelHelper.GetSystemColor(
                UserLookAndFeel.Default.ActiveLookAndFeel,
                SystemColors.Control);
            renameEditor.KeyDown += OnRenameEditorKeyDown;
            renameEditor.LostFocus += renameEditor_LostFocus;
            this.TabRenamed += InspectablePivotGrid_TabRenamed;
            showUI();
        }

        protected override void InitLayout() {
            base.InitLayout();
            scTabAndSelector.SplitterDistance = scTabAndSelector.ClientSize.Width;
        }

        #endregion


        void removePositionPivotProgressPanelInternal( ProgressPanel pp, InspectablePivotGrid PositionPivotGrid)
        {
            if (InvokeRequired)
                this.Invoke(new RemovePositionPivotProgressPanelHandler(this.removePositionPivotProgressPanelInternal),  pp, PositionPivotGrid);
            else
            {
                if (pp != null)
                {

                    PositionPivotGrid.panelControl1.Visible = true;
                
                    var pivotSplitControl = PositionPivotGrid.scPivotAndLower;
                    pivotSplitControl.Panel1.Controls.Remove(pp);
                    pp.Dispose();
                    pp = null; 

                }
            }
        }


        public void removePositionPivotProgressPanel()
        {
            removePositionPivotProgressPanelInternal(positionPivotGridProgressPanel, this);
        }

        public void addPositionPivotGridProgressPanel()
        {
            if (positionPivotGridProgressPanel != null)
                removePositionPivotProgressPanel();

            addPositionPivotGridProgressPanelInternal( positionPivotGridProgressPanel = new ProgressPanel(), this);
        }


        void addPositionPivotGridProgressPanelInternal(ProgressPanel ppanel, InspectablePivotGrid PositionPivotGrid)
        {
            Rectangle r;
            ppanel.LookAndFeel.SkinName = "Office 2010 Blue";

            if (this.InvokeRequired)
                this.Invoke(new AddPositionPivotGridProgressPanelToHandler(this.addPositionPivotGridProgressPanelInternal),  ppanel, PositionPivotGrid);
            else
            {

                var pivotSplitControl = PositionPivotGrid.scPivotAndLower;

                r = PositionPivotGrid.panelControl1.Bounds;
                PositionPivotGrid.panelControl1.Visible = false;
              

                ppanel.SetBounds(r.X, r.Y, r.Width, r.Height, BoundsSpecified.All);
                pivotSplitControl.Panel1.Controls.Add(ppanel);

                ppanel.Dock = DockStyle.Fill;
                ppanel.Description = "Please wait until positions are loaded...";

            }
        }


        #region action methods
        void addTab(object sender,EventArgs ea) {

            addPositionPivotGridProgressPanel();

            WrappedWinPivotDef wwpd;

            var newTabPage = createEmptyTabPage(this.xtraTabControl1);
            if (newTabPage != null)

                this.xtraTabControl1.SelectedTabPage = newTabPage;
            if (newTabAdded != null) 
            {
                if (createPivotDef != null)
                    wwpd = createPivotDef(this,newTabPage.Text);
                else 
                {
                    wwpd = new WrappedWinPivotDef();
                    wwpd.createDefault(newTabPage.Text,null);
                    wwpd.tabName = newTabPage.Text;
                }

                wwpd.tabIndex =
                    xtraTabControl1.TabPages.IndexOf(newTabPage);
                wwpd.tabName = newTabPage.Text;
                newTabAdded(this,wwpd);

                newTabPage.MainDataSource = this.MainDataSource;
                newTabPage.setCurrentDataSource();

                this.refreshCurrentTabPage();

                var tabPage = newTabPage as TabPageWithUomPivot;

                newTabPage.setNumberPositions(this.NumPositionsOnScreen);
                newTabPage.setNumberRealPortfolios(this.NumRealPortfoliosOnScreen);
                
            }

            removePositionPivotProgressPanel();

        }

        public event AddNewTabHandler newTabAdded;
        public event NewPivotDef createPivotDef;

        public event UpdatePivotDef updatePivotDef;
        public delegate void UpdatePivotDef(object sender, string name, bool showProgressIcon);

        public delegate void AddNewTabHandler(object sender,WrappedWinPivotDef aPivotDef);
        public delegate WrappedWinPivotDef NewPivotDef(object sender,string name);


        public  void UpdatePivotDefinitionForTab(WrappedWinPivotDef pivotDefinition, string tabName)
        { 
     
            TabPageWithUomPivot tabPage = findTabNamed(pivotDefinition.tabName);
            tabPage.updateWindowStateDefinition(pivotDefinition);
        }

        public void inspectablePivotGrid1_updatePivotDefEvent(object sender, string tabName, bool showProgressIcon)
        {

            updatePivotDef(this, tabName, showProgressIcon);

        }


        public delegate void DeleteTabHandler(object sender,string tabName);
        public event DeleteTabHandler deleteTab;

        void removeTab(object sender,EventArgs ea) {
            int n;
            string tabName;

            if (this.tabControl.SelectedTabPage != null) {
                if (MessageBox.Show(
                    "Really remove '" + (tabName = this.tabControl.SelectedTabPage.Text) + "'?",
                    "Please verify",
                    MessageBoxButtons.YesNo) == DialogResult.Yes) {
                    n = tabControl.SelectedTabPageIndex;
                    tabControl.TabPages.RemoveAt(n);
                    if (deleteTab != null)
                        deleteTab(this,tabName);
                }
            }
        }

        void btnExpand_Click(object sender,EventArgs e) {
            try {
                expanded = !expanded;
                if (this.InspectorExpanded != null)
                    this.InspectorExpanded(this,new ExpandedInspectorArgs(expanded));
                if (expanded)
                    showCurrentPivotFieldStatus();
                else
                    amphoraFieldSelector1.activePivotGrid = null;
                showUI();
            } 
            catch (Exception ex) {
                Util.show(MethodBase.GetCurrentMethod(),ex);
            }
            Util.show(MethodBase.GetCurrentMethod());
        }

        void showCurrentPivotFieldStatus() {
            TabPageWithUomPivot aPage;

            aPage = this.xtraTabControl1.SelectedTabPage as TabPageWithUomPivot;

            if (aPage != null)
                showPivotFieldStatus(aPage);
        }

        void showPivotFieldStatus(TabPageWithUomPivot aPage) 
        {
            if (aPage == null)
                throw new ArgumentNullException("aPage",typeof(TabPageWithUomPivot).Name + " is null!");

            amphoraFieldSelector1.activePivotGrid = aPage.pivotGrid;
            amphoraFieldSelector1.activePivotGridType = this.pivotDataType;
            amphoraFieldSelector1.populateSelection(aPage.pivotGrid.Fields);
        }

        void xtraTabControl1_SelectedPageChanging(object sender,TabPageChangingEventArgs e) {
            TabPageWithUomPivot aPage;

            amphoraFieldSelector1.activePivotGrid = null;
            amphoraFieldSelector1.activePivotGridType = null;
            if (!e.Cancel) 
            {

                aPage = e.Page as TabPageWithUomPivot;

                if (aPage != null)
                    showPivotFieldStatus(aPage);
                this.refreshCurrentTabPage();
            }
        }

        void xtraTabControl1_SelectedPageChanged(object sender,TabPageChangedEventArgs e) {
            TabPageWithUomPivot tabPage;

            if (!xtraTabControl1.IsLoading &&  xtraTabControl1.SelectedTabPage != null && (tabPage = this.selectedPage()) != null && !tabPage.isTabAlreadyOpened)
            {
                if (SelectedPageChanged != null)
                    this.SelectedPageChanged(this, tabPage);
                tabPage.isTabAlreadyOpened = true;
            }
        }

        void mtp_SelectedCellChanged(object sender,EventArgs e) 
        {
            PivotGridControl pgc;
      

            if (scPivotAndLower.Panel2.Size.Height > 0) {
                if ((pgc = sender as PivotGridControl) != null) 
                {
                    var FocusedCellInfo = pgc.Cells.GetFocusedCellInfo();

                    if (FocusedCellInfo != null ) 
                    {
                        PivotDrillDownDataSource drilldownsource = FocusedCellInfo.CreateDrillDownDataSource();

                        bool isRunningTotal = false;

                        var x = FocusedCellInfo.RowValueType;
                        

                        if (FocusedCellInfo.DataField.Name == "pgfRisk Rolling Total" && FocusedCellInfo.RowValueType!=PivotGridValueType.GrandTotal)
                            isRunningTotal = true;
                            

                        showDistributions(isRunningTotal, FocusedCellInfo.CreateDrillDownDataSource(),this.distributionsGridControl);
                        showPositions(isRunningTotal, FocusedCellInfo.CreateDrillDownDataSource(), this.PositionsGridControl);
                        showInventoryBuilDraws(isRunningTotal, FocusedCellInfo.CreateDrillDownDataSource(), this.InventoryBuildDrawGridControl);
                    
                    } else 
                    {
                        clearDataSource(this.distributionsGridControl);
                        clearDataSource(this.PositionsGridControl);
                        clearDataSource(this.InventoryBuildDrawGridControl);
                    }
                }

            }
        }

        private void showPositions(bool isRunningTotal, PivotDrillDownDataSource drillDownDataSource, GridControl gridControl)
        {
            TabPageWithUomPivot selectedPage1 = selectedPage();
            object dataSource = selectedPage1.pgcPositions.DataSource;

            List<RiskGroup> selectedPositions;

            if (isRunningTotal)
            {
                TabPageWithUomPivot currPage = this.selectedPage();
                selectedPositions = currPage.getRollingTotalGroups(drillDownDataSource);
            }

            else
                selectedPositions = getPositionsFromDataSource(drillDownDataSource, dataSource as List<RiskGroup>);

            

            gridControl.BeginUpdate();
            PositionsGridView.BestFitColumns();
            gridControl.DataSource = selectedPositions;
            gridControl.EndUpdate();

        }

        private void showInventoryBuilDraws(bool isRunningTotal, PivotDrillDownDataSource drillDownDataSource, GridControl gridControl)
        {
            try
            {
                TabPageWithUomPivot selectedPage1 = selectedPage();
                object dataSource = selectedPage1.pgcPositions.DataSource;

                List<RiskGroup> selectedPositions;

                if (isRunningTotal)
                {
                    TabPageWithUomPivot currPage = this.selectedPage();
                    selectedPositions = currPage.getRollingTotalGroups(drillDownDataSource);
                }

                else
                    selectedPositions = getPositionsFromDataSource(drillDownDataSource, dataSource as List<RiskGroup>);

                if (selectedPositions != null && selectedPositions.Count > 0)
                {
                    List<int> realKids = new List<int>();
                    foreach (RiskGroup rg in selectedPositions)
                    {
                        if(rg != null && rg.atoms != null && rg.atoms.Count > 0)
                        {
                            foreach(RiskGroupAtom atm in rg.atoms)
                            {
                                if(atm.posType != null && (atm.posType.Equals("Z") || atm.posType.Equals("I")))
                                    realKids.Add(rg.positionNumber);
                            }
                        }
                    }
                    java.lang.Integer[] positionNumbers;
                    InventoryBuildDrawEntityService iBDService = new InventoryBuildDrawEntityService(LocalCayenneService.sharedInstance);
                    positionNumbers = SharedContext.makeVector(realKids.ToArray());
                    java.util.List selectedInventoryBuildDraws = iBDService.findInventoryBuildDrawForPosNums(positionNumbers);

                    List<WrappedInventoryBuildDraw> lstInvBuildDraws = new List<WrappedInventoryBuildDraw>();
                    for (int invCnt = 0; invCnt < selectedInventoryBuildDraws.size(); invCnt++)
                    {
                        InventoryBuildDraw invbd = selectedInventoryBuildDraws.get(invCnt) as InventoryBuildDraw;
                        WrappedInventoryBuildDraw invtryBD = new WrappedInventoryBuildDraw(invbd);
                        lstInvBuildDraws.Add(invtryBD);
                    }
                    if (lstInvBuildDraws != null && lstInvBuildDraws.Count > 0)
                    {
                        gridControl.BeginUpdate();
                        InventoryBuildDrawGridView.BestFitColumns();
                        gridControl.DataSource = lstInvBuildDraws;
                        gridControl.EndUpdate();
                    }
                    else
                    {
                        clearDataSource(gridControl);
                    }
                }
            }
            catch (Exception exp)
            {
            }
        }

        void DetailsXtraTabControl_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
        {
            if(e.Page.Name.Equals("tpInventoryBuildDraw"))
            {
                this.InventoryBuildDrawGridControl.Refresh();
            }
        }

        void showDistributions(bool isRunningTotal, PivotDrillDownDataSource drillDownDataSource,GridControl gc )
        {
            GridView gv;
              
            TabPageWithUomPivot selectedPage1 = selectedPage();
            object dataSource = selectedPage1.pgcPositions.DataSource;
            int decimals = Convert.ToInt32(selectedPage1.DecimalsInputControl.Value);
            string UOM = selectedPage1.UOMInputControl.EditValue.ToString() ;

            Dictionary<int, char> positionTypes; 
            Dictionary<int, string> commodityCodes;
            Dictionary<int, string> originalUOMs;

            this.getPositionAttributesFromDataSource(isRunningTotal, drillDownDataSource, dataSource as List<RiskGroup>, out positionTypes, out commodityCodes, out originalUOMs);

            int[] posNumsRequired = positionTypes.Keys.ToArray();

            if (posNumsRequired.Length > 0) 
            {
                var distributions = dataProvider.findDistributions(posNumsRequired.ToArray(), positionTypes, commodityCodes, originalUOMs, decimals, UOM, selectedPage1.IsEqivChecked);
                
                gc.BeginUpdate();
                gc.DataSource = distributions; 
                gc.EndUpdate();

                GridView gridView = gc.MainView as GridView;

                if (gridView  != null) 
                {
                    if (gridView.OptionsView.ColumnAutoWidth)
                        gridView.OptionsView.ColumnAutoWidth = !gridView.OptionsView.ColumnAutoWidth;
                    foreach (GridColumn gcol in gridView.Columns)
                        gcol.OptionsColumn.ReadOnly = true;

                    gridView.BestFitColumns(true);
                }

            } else {
                clearDataSource(gc);
            }
        }

        #region constants
        const string PROP_NAME_POS_NUM = "positionNumber";
        const string PROP_NAME_PORT = "portfolio";
        #endregion




        void logisticsClickedHandler(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
       {
           try
           {
               int[] selectedRows = DistributionsGridView.GetSelectedRows();

               if (selectedRows.Length > 0)
               {
                   WrappedDistribution dataRow = DistributionsGridView.GetRow(selectedRows[0] ) as WrappedDistribution;

                   var tradeId = dataRow.tradeNum;

                   OpenLogisticsPanel(tradeId.ToString());
               }
           }
           catch (Exception ex)
           { 
           }
        }

        void dealViewerClickedHandler(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int[] selectedRows = DistributionsGridView.GetSelectedRows();

                if (selectedRows.Length > 0)
                {
                    WrappedDistribution dataRow = DistributionsGridView.GetRow(selectedRows[0]) as WrappedDistribution;

                    var tradeId = dataRow.tradeNum;

                    OpenDealViewerPanel(tradeId.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Broken link: Application Deal Viewer not found. Please contact the Administrator");
            }
        }


        private string WriteIdIntoNotepadFile(string serviceName, string CommandParameter)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            int i = path.LastIndexOf('\\');
            path = path.Substring(0, i);
            if (serviceName == "LC" || serviceName == "RiskCover")
                path = path + "\\CornerstoneShellICTSService" + ".plist";
            else
                path = path + "\\" + serviceName + "ICTSService" + ".txt";

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (FileStream file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(file))
            {
                sw.Write(CommandParameter);
            }
            return path;
        }

      
        private void OpenLogisticsPanel(string CommandParameter)
        {

            //todo: put this in the cofig file
            string ybdealsheetpath = ConfigurationSettings.AppSettings["LogisticsPath"]; 

            if (File.Exists(ybdealsheetpath))
            {
                Process returnprocess = Process.Start(ybdealsheetpath);
            }
            else
            {
                MessageBox.Show("Broken link: Application not found at path " + ybdealsheetpath + ". Please contact the Administrator");
            }
        }

        private void OpenTradeCapturePanel(string CommandParameter)
        {
            string path = WriteIdIntoNotepadFile("TradeCapture", CommandParameter);

            //todo: put this in the cofig file
            string ybdealsheetpath = ConfigurationSettings.AppSettings["TradeCapturePath"]; 

            string tradeNumsToOpen = "Open," + CommandParameter;

            if (File.Exists(ybdealsheetpath))
            {
                Process returnprocess = Process.Start(ybdealsheetpath, tradeNumsToOpen);
            }
            else
            {
                MessageBox.Show("Broken link: Application not found at path " + ybdealsheetpath + ". Please contact the Administrator");
            }

        }


        private void OpenDealViewerPanel(string CommandParameter)
        {
            string path = WriteIdIntoNotepadFile("DealViewer", CommandParameter);

            //todo: put this in the cofig file
            string ybdealsheetpath = ConfigurationSettings.AppSettings["DealViewerPath"];

            if (File.Exists(ybdealsheetpath))
            {
                Process returnprocess = Process.Start(ybdealsheetpath, "\"" + path + "\"");
            }
            else
            {
                MessageBox.Show("Broken link: Application not found at path " + ybdealsheetpath + ". Please contact the Administrator");
            }
        }

        void tradeCaptureClickedHandler(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                int[] selectedRows = DistributionsGridView.GetSelectedRows();

                if (selectedRows.Length > 0)
                {
                    WrappedDistribution dataRow = DistributionsGridView.GetRow(selectedRows[0]) as WrappedDistribution;

                    var tradeId = dataRow.tradeNum;

                    OpenTradeCapturePanel(tradeId.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Broken link: Application Trade Capture not found. Please contact the Administrator");
            }
        }
      

       private List<RiskGroup> getPositionsFromDataSource(PivotDrillDownDataSource drillDownDataSource, List<RiskGroup> DataSource)
       {
           List<RiskGroup> positions = new List<RiskGroup>();


           TabPageWithUomPivot selectedPage1 = selectedPage();
           object dataSource = selectedPage1.pgcPositions.DataSource;
           int decimals = Convert.ToInt32(selectedPage1.DecimalsInputControl.Value);

           foreach (PivotDrillDownDataRow row in drillDownDataSource)
           {
               RiskGroup selectedRiskGroup = DataSource[row.ListSourceRowIndex] as RiskGroup;
               selectedRiskGroup.decimalPrecision = decimals;

               positions.Add(selectedRiskGroup);

           }

           return positions;
       }


       private static string getUOMFromRiskGroup(RiskGroup riskGroup)
       {
           string UOMFromRiskGroup = riskGroup.qtyUom;
           string finalUOM = "";

           if (!string.IsNullOrEmpty(UOMFromRiskGroup))
               finalUOM = UOMFromRiskGroup;

           return finalUOM;
       }


       private static string getCommodityCodeFromRiskGroup(RiskGroup riskGroup)
       {
           string commodityTypeFromRiskGroup = riskGroup.cmdtyName;
           string finalCommodityType = "";

           if (!string.IsNullOrEmpty(commodityTypeFromRiskGroup))
               finalCommodityType = commodityTypeFromRiskGroup;
           
           return finalCommodityType; 
       }

       private static char getPositionTypeFromRiskGroup(RiskGroup riskGroup)
       {
           char positionTypeChar = ' ';

            string posSource = riskGroup.posSource;

            if (!string.IsNullOrEmpty(posSource))
                positionTypeChar = posSource[0];

            return positionTypeChar;
       }

        void  getPositionAttributesFromDataSource(bool isRunningTotal, PivotDrillDownDataSource drillDownDataSource, List<RiskGroup> DataSource,
                                                                         out Dictionary<int, char> positionTypes, out  Dictionary<int, string> commodityCodes, out Dictionary<int, string> UOMs)
        {
            positionTypes = new Dictionary<int, char>();
            commodityCodes = new Dictionary<int, string>();
            UOMs = new Dictionary<int, string>();

            if (isRunningTotal)
            {
                TabPageWithUomPivot currPage = this.selectedPage();
                List<RiskGroup> selectedPositions = currPage.getRollingTotalGroups(drillDownDataSource);
                foreach (RiskGroup selectedRiskGroup in selectedPositions)
                {
                    positionTypes.Add(selectedRiskGroup.positionNumber, getPositionTypeFromRiskGroup(selectedRiskGroup));
                    commodityCodes.Add(selectedRiskGroup.positionNumber, getCommodityCodeFromRiskGroup(selectedRiskGroup));
                    UOMs.Add(selectedRiskGroup.positionNumber, getUOMFromRiskGroup(selectedRiskGroup));
                   
                }

            }

            else
            {
                foreach (PivotDrillDownDataRow row in drillDownDataSource)
                {
                    RiskGroup selectedRiskGroup = DataSource[row.ListSourceRowIndex] as RiskGroup;

                    positionTypes.Add(selectedRiskGroup.positionNumber, getPositionTypeFromRiskGroup(selectedRiskGroup));
                    commodityCodes.Add(selectedRiskGroup.positionNumber, getCommodityCodeFromRiskGroup(selectedRiskGroup));
                    UOMs.Add(selectedRiskGroup.positionNumber, getUOMFromRiskGroup(selectedRiskGroup));
                  
                }
            }

           
        }

        static List<int> readSelectionPosNums(PivotDrillDownDataSource ds) 
        {
            List<int> posNumsRequired = new List<int>();

            PropertyDescriptorCollection props;
            PropertyDescriptor pdPosNums,pdPort;
            int[] posNumVector;
            int colIndex;

           
            props = ds.GetItemProperties(null);

            pdPosNums = props[PROP_NAME_POS_NUM];

            if (pdPosNums != null &&
                (colIndex = props.IndexOf(pdPosNums)) >= 0)
            {
                for (int i = 0; i < ds.RowCount; i++)
                {
                    int tempPositionNum = (int)ds[i][colIndex];

                    posNumsRequired.Add(tempPositionNum);
                }
            }
            return posNumsRequired;
        }

   
        #endregion

        #region tab-page dragging
        /* from DevExpress article: Q215084 */

        /// <summary>mouse-down location from page-drag.</summary>
        /// <remarks><para>from DevExpress article: Q215084</para></remarks>
        Point pageDragLocation = Point.Empty;
        /// <summary>The page being dragged.</summary>
        /// <remarks><para>from DevExpress article: Q215084</para></remarks>
        XtraTabPage pageBeingMoved = null;

        /// <summary>Mouse-down handler within the tab-control.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks><para>from DevExpress article: Q215084</para></remarks>
        void xtraTabControl1_MouseDown(object sender,MouseEventArgs e) {
            XtraTabControl c = sender as XtraTabControl;
            pageDragLocation = new Point(e.X,e.Y);
            XtraTabHitInfo hi = c.CalcHitInfo(pageDragLocation);
            pageBeingMoved = hi.Page;
            if (hi.Page == null)
                pageDragLocation = Point.Empty;
        }

        void xtraTabControl1_MouseMove(object sender,MouseEventArgs e) {
            if (e.Button == MouseButtons.Left)
                if ((pageDragLocation != Point.Empty) && (
                        (Math.Abs(e.X - pageDragLocation.X) > SystemInformation.DragSize.Width) ||
                        (Math.Abs(e.Y - pageDragLocation.Y) > SystemInformation.DragSize.Height)))
                    xtraTabControl1.DoDragDrop(sender,DragDropEffects.Move);
        }

        void xtraTabControl1_DragOver(object sender,DragEventArgs e) {
            XtraTabControl c = sender as XtraTabControl;
            if (c == null)
                return;
            XtraTabHitInfo hi = c.CalcHitInfo(c.PointToClient(new Point(e.X,e.Y)));
            if (hi.Page != null) {
                if (hi.Page != pageBeingMoved) {
                    if (c.TabPages.IndexOf(hi.Page) < c.TabPages.IndexOf(pageBeingMoved))
                        c.TabPages.Move(c.TabPages.IndexOf(hi.Page),pageBeingMoved);
                    else
                        c.TabPages.Move(c.TabPages.IndexOf(hi.Page) + 1,pageBeingMoved);
                }
                e.Effect = DragDropEffects.Move;
            } else
                e.Effect = DragDropEffects.None;
        }
        #endregion

        #region tab-page renaming
        /* from DevExpress example E2681 */

        void OnXtraTabControlDoubleClick(object sender,MouseEventArgs e) {
            if (e.Button == MouseButtons.Left && e.Clicks == 2) {
                XtraTabControl tabControl = sender as XtraTabControl;
                XtraTabHitInfo hitInfo = tabControl.CalcHitInfo(e.Location);
                XtraTabPage tabPage = tabControl.SelectedTabPage;
                if (hitInfo.HitTest == XtraTabHitTest.PageHeader)
                    SetUpRenameEditor(tabPage);
            }
        }

        void SetUpRenameEditor(XtraTabPage tabPage) {
            renameEditor.Visible = true;
            renameEditor.Bounds = GetRenameEditorBounds(tabPage);
            renameEditor.Text = tabPage.Text;
            renameEditor.Select();
            renameEditor.SelectAll();
        }

        Rectangle GetRenameEditorBounds(XtraTabPage tabPage) {
            PropertyInfo pr = tabPage.GetType().GetProperty("PageViewInfo",System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            BaseTabPageViewInfo tabInfo = pr.GetValue(tabPage,null) as BaseTabPageViewInfo;
            Rectangle rect = tabInfo.Bounds;
            return new Rectangle(rect.X + 2,rect.Y + 2,rect.Width - 4,rect.Height - 2);
        }

        void OnRenameEditorKeyDown(object sender,KeyEventArgs e) {
            if (e.KeyData == Keys.Enter)
                renameAndHide();
        }

        void renameAndHide() {
            string prev,newName;

            prev = xtraTabControl1.SelectedTabPage.Text;
            xtraTabControl1.SelectedTabPage.Text = newName = renameEditor.Text;
            renameEditor.Visible = false;
            if (TabRenamed != null)
                TabRenamed(this,prev,newName);
        }

        void renameEditor_LostFocus(object sender,EventArgs e) {
            if (renameEditor.Visible)
                renameAndHide();
        }

        void InspectablePivotGrid_TabRenamed(object sender,string oldName,string newName) {
            Util.show(MethodBase.GetCurrentMethod(),"from '" + oldName + "' to '" + newName + "'.");
        }
        #endregion



        private string getDecimalPrecisionStringFromDecimal(int decimalPrecision)
        {
            string precisionString = "0.";

            for (int i = 0; i < decimalPrecision; i++)
            {
                precisionString+="0";
            }

            return precisionString;
        }

        #region action methods



        void positionsCustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            try
            {

                double dval;
                int ival;

                if (e.Column != null)
                {
                    if (e.Column.ColumnType.Equals(typeof(double)))
                    {

                        dval = double.Parse(e.Value.ToString(), NumberStyles.Any);

                        if (dval == double.MinValue)
                            e.DisplayText = "N/A";

                        else
                        {
   
                            int dataRowIndex = e.ListSourceRowIndex;
                            List<RiskGroup> dataSource = this.PositionsGridControl.DataSource as List<RiskGroup>;

                            RiskGroup selectedItem = dataSource[dataRowIndex];
                        

                            if (e.DisplayText != "N/A")
                            {
                                int decimalPrecision = selectedItem.decimalPrecision;

                                if (decimalPrecision >= 0)
                                {
                                    string customFormat = SharedContext.getCustomFormatForDoubles(decimalPrecision);

                                    e.DisplayText = dval.ToString(customFormat);
                                }

                                else
                                    e.DisplayText = "N/A";
                            }

                        }
                    }
                    else if (e.Column.ColumnType.Equals(typeof(int)))
                    {
                        ival = Convert.ToInt32(e.Value);
                        if (ival.Equals(Int32.MinValue))
                            e.DisplayText = "N/A";
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }



        void distributionsCustomColumnDisplayText(object sender,CustomColumnDisplayTextEventArgs e) {
            double dval;
            int ival;

            try
            {

                if (e.Column != null)
                {
                    if (e.Column.ColumnType.Equals(typeof(double)))
                    {

                        dval = double.Parse(e.Value.ToString(), NumberStyles.Any);

                        if (dval == double.MinValue)
                            e.DisplayText = "";

                        else
                        {


                            IUOMEntityCache entCache = UOMEntityCacheImpl.Builder.uomCache();

                            int dataRowIndex = e.ListSourceRowIndex;
                            List<WrappedDistribution> dataSource = this.distributionsGridControl.DataSource as List<WrappedDistribution>;

                            WrappedDistribution selectedItem = dataSource[dataRowIndex];

                            try
                            {
                                if (selectedItem.originalquantityUom != selectedItem.finalUomToConvertTo)
                                {
                                    //distribution itself has a conversion
                                    if (selectedItem.originalquantityUom != selectedItem.originalUomToConvertTo)
                                    {
                                        dval = dval * selectedItem.qtyUomConvRate;
                                    }

                                    //units still don't match what user wants on the screen, so convert them using UOMCache
                                    if (selectedItem.originalUomToConvertTo != selectedItem.finalUomToConvertTo)
                                    {
                                        dval = entCache.convertUom(dval, selectedItem.commodityCode, selectedItem.originalUomToConvertTo, selectedItem.finalUomToConvertTo).doubleValue();
                                    }
                                }

                            }

                            catch (Exception ex)
                            {
                                e.DisplayText = "N/A";
                            }

                            if (e.DisplayText != "N/A")
                            {
                                int decimalPrecision = selectedItem.decimalPrecision;

                                if (decimalPrecision >= 0)
                                {
                                    string customFormat = SharedContext.getCustomFormatForDoubles(decimalPrecision);

                                    e.DisplayText = dval.ToString(customFormat);
                                }

                                else
                                    e.DisplayText = "N/A";
                            }

                        }
                    }
                    else if (e.Column.ColumnType.Equals(typeof(int)))
                    {
                        ival = Convert.ToInt32(e.Value);
                        if (ival.Equals(Int32.MinValue))
                            e.DisplayText = "N/A";
                    }

                }
            }
            catch (Exception ex)
            { 
            }
        }

        void gvPnL_CustomDrawRowIndicator(object sender,RowIndicatorCustomDrawEventArgs e) {
            GridView gv;
            NormalPnlRecord rec;
            int nrow;

            if (!e.Info.IsRowIndicator) return;
            if (e.RowHandle != GridControl.InvalidRowHandle) {
                if ((gv = sender as GridView) != null) {
                    nrow = gv.GetDataSourceRowIndex(e.RowHandle);
                    if ((rec = gv.GetRow(nrow) as NormalPnlRecord) != null) {
                        // https://www.devexpress.com/Support/Center/Question/Details/Q147257
                        if (gv.IndicatorWidth < 0)
                            gv.IndicatorWidth = 100;
                        e.Info.DisplayText = rec.rowType;
                    }
                }
            }
        }
        #endregion action methods

        TabPageWithUomPivot createEmptyTabPage(XtraTabControl xtc) {
            return createEmptyTabPage(xtc,getNewTabPageName());
        }
        public string getNewTabPageName()
        {
            if (this.xtraTabControl1 != null)
            {
                if (this.xtraTabControl1.TabPages == null)
                    return WrappedWinPivotDef.NULL_PIVOT_NAME;
                else if (this.xtraTabControl1.TabPages.Count > 0)
                {
                    int tabCounter = 1;
                    foreach (XtraTabPage xTab in this.xtraTabControl1.TabPages)
                    {
                        if (xTab != null && xTab.Text != null && xTab.Text.Contains("Untitled"))
                        {
                            int localCountr = Convert.ToInt32(xTab.Text.Substring(xTab.Text.Length - 1, 1));
                            if (localCountr > tabCounter)
                            {
                                tabCounter = localCountr;
                                tabCounter++;
                            }
                            else
                                tabCounter++;
                        }
                    }
                    return "Untitled" + tabCounter;
                }
            }
            return WrappedWinPivotDef.NULL_PIVOT_NAME;
        }

        TabPageWithUomPivot createEmptyTabPage(XtraTabControl xtc,string tabName) {
            TabPageWithUomPivot tabPage;

            if (pivotDataType == null)
                throw new InvalidOperationException("'pivotDataType' property is null");
            tabPage = new TabPageWithUomPivot(tabName,this.uomRequester);
            tabPage.updatePivotDef += inspectablePivotGrid1_updatePivotDefEvent;
         
            xtc.TabPages.Add(tabPage);
            tabPage.resetDistributionDatasource += mtp_resetDistributionDatasource;
            tabPage.pivotDataType = this.pivotDataType;
            DXUtil.setupPivotGrid(tabPage.pivotGrid,pivotDataType);
            addFields(tabPage,DXUtil.findPivotFields(pivotDataType),new string[] { "Risk Quantity","Commodity","Market","Period" });

          
            tabPage.SelectedCellChanged += mtp_SelectedCellChanged;
            xtc.MouseDown += new MouseEventHandler(xtc_MouseDown);

           

           
           
            return tabPage;
        }
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point pnt);
        void xtc_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button.ToString().Equals("Right"))
            {
                IntPtr hWnd = WindowFromPoint(Control.MousePosition);
                if (hWnd != IntPtr.Zero )
                {
                    Control ctl = Control.FromHandle(hWnd);
                    if (this.contextMenuStrip1 != null)
                    {
                        for (int i = 0; i < this.contextMenuStrip1.Items.Count; i++)
                        {
                            this.contextMenuStrip1.Items[i].Visible = true;
                        }
                    }
                }
            }
        }
        
        void mtp_resetDistributionDatasource(object sender,EventArgs e)
        {
            if (scPivotAndLower.Panel2.Size.Height > 0) {
                doClearDataSource(this.distributionsGridControl);
            }
        }

        delegate void ZZZZ(GridControl gc);

        void doClearDataSource(GridControl gc) 
        {
            if (this.InvokeRequired)
                this.Invoke(new ZZZZ(clearDataSource),gc);
            else
                clearDataSource(gc);
        }

        static void clearDataSource(GridControl gc) {
            gc.BeginUpdate();
            gc.DataSource = null;
            gc.EndUpdate();
        }

        static void setDataSource(GridControl gc,object aDatasource) {
            setDataSource(gc,aDatasource,false);
        }

        static void setDataSource(GridControl gc,object aDatasource,bool fixIndicator) {
            GridView gv;

            gc.BeginUpdate();
            gc.DataSource = aDatasource;

            if ((gv = gc.MainView as GridView) != null) {
                if (!gv.OptionsView.ColumnAutoWidth)
                    gv.OptionsView.ColumnAutoWidth = true;
                gv.OptionsBehavior.ReadOnly = true;
                gv.OptionsBehavior.Editable = false;

                if (string.Compare(gc.Name,"gcpnl",true) == 0) {
                    if (gv.Columns != null) {
                        foreach (GridColumn gc1 in gv.Columns) {
                            if (string.Compare(gc1.Name,"colopenpl",true) == 0 ||
                                string.Compare(gc1.Name,"colclosedpl",true) == 0 ||
                                string.Compare(gc1.Name,"colliquidatedpl",true) == 0 ||
                                string.Compare(gc1.Name,"coltotalpl",true) == 0) {
                                gc1.DisplayFormat.FormatType = FormatType.Numeric;
                                gc1.DisplayFormat.FormatString = "$#,##0.00 ;($#,##0.00)";
                            } else {
                                Debug.Print("unhandled '" + gc1.Caption + "' in grid called " + gc.Name);
                            }
                        }
                    }
                } else if (string.Compare(gc.Name,"gcHistoricalPnL",true) == 0) {
                    if (gv.Columns != null) {
                        foreach (GridColumn gc1 in gv.Columns) {
                            if (string.Compare(gc1.Name,"coldate",true) == 0) {
                                gc1.DisplayFormat.FormatType = FormatType.DateTime;
                                gc1.DisplayFormat.FormatString = "dd-MMM-yy";
                            } else if (string.Compare(gc1.Name,"colprofitloss",true) == 0 ||
                                string.Compare(gc1.Name,"coldelta",true) == 0) {
                                gc1.DisplayFormat.FormatType = FormatType.Numeric;
                                gc1.DisplayFormat.FormatString = "$#,##0.00 ;($#,##0.00)";
                            } else {
                                Debug.Print("unhandled '" + gc1.Caption + "' in grid called " + gc.Name);
                            }
                        }
                    }
                }
                if (fixIndicator && gv.IndicatorWidth < 0)
                    gv.IndicatorWidth = 100;
                gv.BestFitColumns();
            }
            gc.EndUpdate();
        }

        TabPageWithUomPivot findTabNamed(string p) {
            TabPageWithUomPivot tpup;

            foreach (var avar in this.xtraTabControl1.TabPages)
                if (avar is TabPageWithUomPivot &&
                    string.Compare(p,(tpup = avar as TabPageWithUomPivot).Text,true) == 0)
                    return tpup;
            return null;
        }

        private void amphoraFieldSelector1_Load(object sender, EventArgs e)
        {

        }
    }

    [SuppressMessage("CodeRush","Type name does not correspond to file name")]
    public delegate void TabRenameHandler(object sender,string oldName,string newName);

    [SuppressMessage("CodeRush","Type name does not correspond to file name")]
    public delegate void ExpandedInspectorHandler(object sender,ExpandedInspectorArgs e);

    [SuppressMessage("CodeRush","Type name does not correspond to file name")]
    public class ExpandedInspectorArgs : EventArgs {
        public ExpandedInspectorArgs() { }
        public ExpandedInspectorArgs(bool isExpanded) : this() { this.expanded = isExpanded; }
        public bool expanded { get; set; }
    }
}