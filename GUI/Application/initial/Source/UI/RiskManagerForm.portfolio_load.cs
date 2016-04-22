using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraWaitForm;
using NSRMCommon;
using NSRMLogging;
using com.amphora.cayenne.entity;
using com.amphora.cayenne.entity.support;
using NSRiskMgrCtrls;
using DevExpress.XtraPivotGrid	;
using DevExpress.XtraTab;
using System.Collections.Generic;

namespace NSRiskManager
{
    public partial class RiskManagerForm
    {
        #region delegates
        delegate void ProgressPanelHandler(XtraForm xf, ProgressPanel ppanel, TreeList c);
       
        delegate void RemoveProgressPanelHandler(XtraForm xf, ProgressPanel pp, TreeList tl);
        delegate void SetTextHandler(XtraForm c, string text);
        delegate void SetupTopLevelTreelistHandler(TreeList treeList);
        delegate void LoadCachedNodesForPortfolioHandler(int portNum);

        #endregion

        #region constants
        const string PREV_PORT_PATH = "Previous Portfolio Path";
        #endregion

        #region fields
        bool setupWorker;
        DateTime portFetchStart;
        DateTime portFetchEnd;
        ProgressPanel pp;
  
        bool forcingInitialSelection;
        bool portfolioHierarchyLoading = false;
        int selectedPathIndexCount = 0;
        
        private static object mylock = new object();

        #endregion

        #region background worker methods

        void DataLoadBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            addProgressPanelTo(this, pp = new ProgressPanel(), this.treeList1);
            setWindowText(ConnectionUtil.dotNetConnectionString());

            lock (mylock)
            {

                setupTopLevelTreeList(this.treeList1);
           
                SharedContext.loadPortfolioInspectorData();
            }
        }

        void DataLoadBackgroundWorker_Disposed(object sender, EventArgs e)
        {
            Util.show(MethodBase.GetCurrentMethod());
        }

        void DataLoadBackgroundWorker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            portFetchEnd = DateTime.Now;

            if (pp != null)
            {
                removeProgressPanel(this, pp, treeList1);
                pp.Dispose();
                pp = null;
            }
            
            enableUI(true);
            Trace.WriteLine("port-fetch took " + (portFetchEnd - portFetchStart).TotalMilliseconds / 1000 + " seconds.");
           
            selectPortfolioPath();
        }

        #endregion

        #region other methods
        void loadData()
        {
           
            if (!setupWorker)
            {
                setupWorker = true;
                dataLoadBackgroundWorker.Disposed += DataLoadBackgroundWorker_Disposed;
                dataLoadBackgroundWorker.DoWork += DataLoadBackgroundWorker_DoWork;
                dataLoadBackgroundWorker.RunWorkerCompleted += DataLoadBackgroundWorker_Completed;
            }
           

            enableUI(false);
            portFetchStart = DateTime.Now;
            dataLoadBackgroundWorker.RunWorkerAsync();
        }

        void enableUI(bool enable)
        {
            inspectablePivotGrid1.Enabled = enable;
        }

        void addProgressPanelTo(XtraForm xf, ProgressPanel ppanel, TreeList tl)
        {
            Rectangle r;

            if (this.InvokeRequired)
                this.Invoke(new ProgressPanelHandler(this.addProgressPanelTo), xf, ppanel, tl);
            else
            {
                r = tl.Bounds;
                tl.Visible = false;
                ppanel.SetBounds(r.X, r.Y, r.Width, r.Height, BoundsSpecified.All);
                this.splitContainerControl1.Panel1.Controls.Add(ppanel);
                ppanel.Dock = DockStyle.Fill;
                ppanel.Description = "Loading portfolio hierarchy...";
            }
        }


        

        void setWindowText(string connStr)
        {
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();

            scsb.ConnectionString = connStr;
            setText(this, this.formName);
        }

        void setText(XtraForm xf, string p)
        {
            if (xf.InvokeRequired)
                xf.Invoke(new SetTextHandler(this.setText), xf, p);
            else
                xf.Text = p;
        }


        private int portfolioIdToExpand = -1;

        void setupTopLevelTreeList(TreeList treeList)
        {

            if (InvokeRequired)
                this.Invoke(new SetupTopLevelTreelistHandler(setupTopLevelTreeList), treeList);

            else
            {
                addTreelistColumns(treeList1, new string[] { "Portfolio", "ID", "Data" , "UniqueID"}); ;

                var portfolioSupport = PortfolioEntityDTOSupportImpl.Builder.portfolioSupport();
    
                portfolioHierarchyLoading = true;
                portfolioIdToExpand = Int32.MinValue;
                portfolioSupport.loadTopLevelPortfolio();

                treeList.OptionsView.AutoWidth = true;
           
                sortByFirstVisibleColumn(treeList);
            }
        }

        void loadCachedChildrenNodesForPortfolio(int parentPortNum)
        {
            if (InvokeRequired)
                this.Invoke(new LoadCachedNodesForPortfolioHandler(loadCachedChildrenNodesForPortfolio), parentPortNum);
            else
            {

                portfolioIdToExpand = parentPortNum;

                var portfolioSupport = PortfolioEntityDTOSupportImpl.Builder.portfolioSupport();
                java.lang.Integer[] portfolioIds = { new java.lang.Integer(parentPortNum) };

                try
                {
                    portfolioSupport.loadPortfolioHeirarchy(portfolioIds);
                }
                catch (Exception e)
                { 
                }
            }

        }


        void removeProgressPanel(XtraForm xf, ProgressPanel pp, TreeList tl)
        {
            if (InvokeRequired)
                this.Invoke(new RemoveProgressPanelHandler(this.removeProgressPanel), xf, pp, tl);
            else
            {
                if (pp != null)
                {
                    pp.Parent.Controls.Remove(pp);
                    if (tl != null && !tl.Visible)
                        tl.Visible = true;
                }
            }
        }
     

        void selectPortfolioPath( )
        {
            // forcingInitialSelection should already be set to true
            ExpandCurrentNodeInDefaultPath(new List<string>());
        
        }

        void ExpandCurrentNodeInDefaultPath(List<string> alreadyExpandedChildNodes)
        {
            try
            {

                if (forcingInitialSelection == false)
                {  
                    //we got here because while expanding the path in one window, the updates were also received in another window
                    //so we need to make sure we re-create the updates again. Please note that node.expanded will cause an event to
                    // fire to get the children, which will likely resemble another expand fired from the other window 


                    foreach (string portIdKey in alreadyExpandedChildNodes)
                    {
                       List<int> alreadyExpandedNodeIds = findTreeNodeIdsForPortfolioId(portIdKey);

                       foreach (int alreadyExpandedNodeId in alreadyExpandedNodeIds)
                       {
                           TreeListNode node = treeList1.FindNodeByFieldValue("UniqueID", alreadyExpandedNodeId);
                           if (node != null)
                           {
                               // node.Expanded = true;
                           }
                       }
                    }
                    
                    return;
                }
                   

                if (initialPortPathVector.Length == 0)
                {
                    forcingInitialSelection = false;
                    return;
                }
                if (selectedPathIndexCount >= initialPortPathVector.Length)
                {
                    forcingInitialSelection = false;
                    return;
                }

                int portfolioId = initialPortPathVector[selectedPathIndexCount];
                
                int parentPortId;
                if (selectedPathIndexCount < 1)
                    parentPortId = Int32.MinValue;
                else
                    parentPortId = initialPortPathVector[selectedPathIndexCount - 1];


                List<int> nodeIDs = findTreeNodeIdsForPortfolioId(portfolioId, parentPortId);

                if (nodeIDs.Count == 0)
                    return;

                //OKSANA_TODO: do we need to look at all paths?
                TreeListNode node2 = treeList1.FindNodeByFieldValue("UniqueID", nodeIDs[0]);
                    
                selectedPathIndexCount++;

               // no need to select that last node
                if (initialPortPathVector.Length == selectedPathIndexCount)
                {
                    // finished expanding.
                    forcingInitialSelection = false;
                    node2.Selected = true;

                    portfolioIdToExpand = -1;
                }
                else
                {
                    node2.Expanded = true;
                  
                }
                
            }
            catch(Exception ex)
            {

            }
       

        }


        static void addTreelistColumns(TreeList tl, string[] treeListColumnNames)
        {
            TreeListColumn v1;

            removeTreelistColumns(tl);
            if (treeListColumnNames != null)
                foreach (string colName in treeListColumnNames)
                {
                    v1 = tl.Columns.Add();
                    v1.VisibleIndex = tl.Columns.Count - 1;
                    v1.Caption = colName;
                    v1.OptionsColumn.AllowEdit = false;
                    v1.OptionsColumn.ReadOnly = true;

                    if (colName == "Data" || colName == "UniqueID")
                    {
                        v1.Visible = false;
                    }
                }


        }


        static void removeTreelistColumns(TreeList tl)
        {
            while (tl.Columns.Count > 0)
                tl.Columns.RemoveAt(0);
        }

        int[] convertStringToIntVector(string portList)
        {
            if (!string.IsNullOrEmpty(portList))
                return Util.makeIntVector(portList);
            return new int[0];
        }


        static void sortByFirstVisibleColumn(TreeList tl)
        {
            int minIndex = Int32.MaxValue, ncol = 0;

            if (tl.Columns.Count > 0)
            {
                tl.BestFitColumns(true);
                while (ncol < tl.Columns.Count)
                {
                    if (tl.Columns[ncol].VisibleIndex >= 0 && tl.Columns[ncol].AbsoluteIndex < minIndex)
                        minIndex = tl.Columns[ncol].AbsoluteIndex;
                    ncol++;
                }
                if (minIndex >= 0)
                    tl.Columns[minIndex].SortIndex = tl.SortedColumnCount + 1;
            }
        }
        #endregion
    }
}