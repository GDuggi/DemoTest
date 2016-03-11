using System;
using System.Collections.Generic;
using OpsManagerNotifier;
using System.Data;
using OpsTrackingModel;
using System.Timers;

namespace DataManager
{
    public class OpsGridManager : IDisposable
    {
        private const string PROJ_FILE_NAME = "OpsGridManager";
        public delegate void EndDataGridUpdate(DevExpress.XtraGrid.Views.Base.BaseView view);

        private Timer opsTrackingClock;

        private IList<DevExpress.XtraGrid.GridControl> managedGridList = null;
        private ListenerManager listenerManager = null;

        private IList<SummaryData> sumDataCache = null;
        private IList<SummaryData> sumDataCacheCopy = null;

        private IList<TradeRqmtConfirm> tradeRqmtConfirmCache = null;
        private IList<TradeRqmtConfirm> tradeRqmtConfirmCacheCopy = null;

        private IList<RqmtData> rqmtDataCache = null;
        private IList<RqmtData> rqmtDataCacheCopy = null;

        private IList<AssociatedDoc> assDocDataCache = null;
        private IList<AssociatedDoc> assDocDataCacheCopy = null;

        private IList<InboundDocsView> inbDocViewCache = null;
        private IList<InboundDocsView> inbDocViewCacheCopy = null;

        public event System.EventHandler IncMessageCounter;
        public event System.EventHandler ResetMessageCounter;
        public event System.EventHandler BeginGridUpdates;
        public event System.EventHandler EndGridUpdates;

        public IList<TradeRqmtConfirm> TradeRqmtConfirmCache
        {
            get 
            { 
                return tradeRqmtConfirmCache; 
            }
        }

        public IList<SummaryData> SumDataCache
        {
            get 
            {
                return sumDataCache; 
            }
        }

        public IList<RqmtData> RqmtDataCache
        {
            get { return rqmtDataCache; }
        }

        public IList<AssociatedDoc> AssDocDataCache
        {
            get { return assDocDataCache; }
        }

        public IList<InboundDocsView> InbDocViewCache
        {
            get { return inbDocViewCache; }
        }

        FireAlarm myFireAlarm = null;
        FireHandlerClass myFireHandler = null;

        public OpsGridManager(string tibcoServer, string tibcoUser, string tibcoPwd, int updateFromCacheTimerInterval, string msgFilter,
            string inbMsgFilter, List<string> pPermKeyList, bool pIsSuperUser)
        {
            try
            {
                managedGridList = new List<DevExpress.XtraGrid.GridControl>();

                sumDataCache = new List<SummaryData>();
                sumDataCacheCopy = new List<SummaryData>();

                rqmtDataCache = new List<RqmtData>();
                rqmtDataCacheCopy = new List<RqmtData>();

                inbDocViewCache = new List<InboundDocsView>();
                inbDocViewCacheCopy = new List<InboundDocsView>();

                assDocDataCache = new List<AssociatedDoc>();
                assDocDataCacheCopy = new List<AssociatedDoc>();

                tradeRqmtConfirmCache = new List<TradeRqmtConfirm>();
                tradeRqmtConfirmCacheCopy = new List<TradeRqmtConfirm>();

                listenerManager = new ListenerManager(tibcoServer, tibcoUser, tibcoPwd, pPermKeyList, pIsSuperUser);
                Console.WriteLine("Filter Condition: " + msgFilter);
                listenerManager.AddListner("sempra.ops.opsTracking.summary.update", this.NotifyTradeSummaryArrived, msgFilter);
                listenerManager.AddListner("sempra.ops.opsTracking.rqmt.update", this.NotifyTradeRqmtArrived, "");
                listenerManager.AddListner("sempra.ops.opsTracking.inboundDocs.update", this.NotifyInboundDocsArrived, inbMsgFilter);
                listenerManager.AddListner("sempra.ops.opsTracking.associatedDocs.update", this.NotifyAssociatedDocsArrived, inbMsgFilter);
                listenerManager.AddListner("sempra.ops.opsTracking.tradeRqmtConfirm.update", this.NotifyTradeRqmtConfirmArrived, "");

                opsTrackingClock = new Timer((updateFromCacheTimerInterval) * 1000);
                opsTrackingClock.Elapsed += new ElapsedEventHandler(opsClockElapsed);


                myFireAlarm = new FireAlarm();
                myFireHandler = new FireHandlerClass(myFireAlarm);
            }
            catch (Exception ex)
            {
                myFireAlarm.ActivateFireAlarm("Error in the OpsGridManager constructor. " + ex.Message);

                string keyListDisplay = string.Empty;
                if (pPermKeyList.Count > 0)
                    keyListDisplay = string.Join(",", pPermKeyList.ToArray());
                else
                    keyListDisplay = "[NONE]";
                throw new Exception("An error occurred while creating a new instance of OpsGridManager using the following values:" + Environment.NewLine +
                    "Message Server: " + tibcoServer + ", Message User: " + tibcoUser + ", Update From Cache Timer Interval: " + updateFromCacheTimerInterval + Environment.NewLine +
                    "Message Filter: " + msgFilter + ", Inbound Message Filter: " + inbMsgFilter + ", Is User a SuperUser? " + pIsSuperUser + Environment.NewLine +
                    "Permission Keys: " + keyListDisplay + "." + Environment.NewLine +
                    "Error CNF-349 in " + PROJ_FILE_NAME + ".OpsGridManager(): " + ex.Message);
            }
        }

        void opsClockElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                // We are commenting out this timer event to implement windows form timer in the opsmanager's
                // main form.  for backwords compatability, we need to keep this event, but we will not do anything.
                // Users that have not implemented the form timer can still call the ExecuteOpsTrackingUpdates() method.
                
              //  ExecuteOpsTrackingUpdates();
            }
            catch (Exception ex)
            {
                //myFireAlarm.ActivateFireAlarm("Error in Ops Clock Timer Event. " + ex.Message);
                //throw new Exception("Error in Ops Clock Timer Event. " + ex.Message);
            }
        }

        public void ExecuteOpsTrackingUpdates()
        {
            try
            {
                opsTrackingClock.Stop();
                lock (sumDataCache)
                {
                    lock (rqmtDataCache)
                    {
                        lock (assDocDataCache)
                        {
                            lock (inbDocViewCache)
                            {
                                lock (tradeRqmtConfirmCache)
                                {

                                    BeginGridUpdates(this, System.EventArgs.Empty);

                                    UpdateTradeSummaryGrid();
                                    UpdateTradeRqmtGrid();

                                    UpdateAssociatedDocGrid();
                                    UpdateInboundDocGrid();

                                    UpdateTradeRqmtConfirmGrid();

                                    ResetMessageCounter(this, System.EventArgs.Empty);


                                    EndGridUpdates(this, System.EventArgs.Empty);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the grids with message data." + Environment.NewLine +
                    "Error CNF-350 in " + PROJ_FILE_NAME + ".ExecuteOpsTrackingUpdates(): " + ex.Message);
            }
        }

        public void PrepareOpsTrackingUpdates()
        {
            try
            {
                lock (sumDataCache)
                {
                    lock (rqmtDataCache)
                    {
                        lock (assDocDataCache)
                        {
                            lock (inbDocViewCache)
                            {
                                lock (tradeRqmtConfirmCache)
                                {
                                    CopyDataToBeUpdated();

                                    sumDataCache.Clear();
                                    rqmtDataCache.Clear();
                                    assDocDataCache.Clear();
                                    inbDocViewCache.Clear();
                                    tradeRqmtConfirmCache.Clear();

                                    ResetMessageCounter(this, System.EventArgs.Empty);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while locking and preparing the local data storage for updating." + Environment.NewLine +
                    "Error CNF-351 in " + PROJ_FILE_NAME + ".PrepareOpsTrackingUpdates(): " + ex.Message);
            }
        }

        private void CopyDataToBeUpdated()
        {
            sumDataCacheCopy.Clear();
            rqmtDataCacheCopy.Clear();
            assDocDataCacheCopy.Clear();
            inbDocViewCacheCopy.Clear();
            tradeRqmtConfirmCacheCopy.Clear();
            
            foreach (SummaryData data in sumDataCache)
            {
                SummaryData copyData = new SummaryData();
                copyData = data;
                sumDataCacheCopy.Add(copyData);
            }

            foreach (RqmtData data in rqmtDataCache)
            {
                RqmtData copyData = new RqmtData();
                copyData = data;
                rqmtDataCacheCopy.Add(copyData);
            }

            foreach (AssociatedDoc data in assDocDataCache)
            {
                AssociatedDoc copyData = new AssociatedDoc();
                copyData = data;
                assDocDataCacheCopy.Add(copyData);
            }

            foreach (InboundDocsView data in inbDocViewCache)
            {
                InboundDocsView copyData = new InboundDocsView();
                copyData = data;
                inbDocViewCacheCopy.Add(copyData);
            }

            foreach (TradeRqmtConfirm data in tradeRqmtConfirmCache)
            {
                TradeRqmtConfirm copyData = new TradeRqmtConfirm();
                copyData = data;
                tradeRqmtConfirmCacheCopy.Add(copyData);
            }
        }

        private void UpdateTradeRqmtConfirmGrid()
        {
            string tableName = "TradeRqmtConfirm";
            DevExpress.XtraGrid.Views.Base.BaseView activeView = null;
            DevExpress.XtraGrid.GridControl activeGrid = null;
            try
            {
                foreach (DevExpress.XtraGrid.GridControl grid in managedGridList)
                {
                    DevExpress.XtraGrid.GridControlViewCollection views = grid.Views;
                    activeGrid = grid;
                    foreach (DevExpress.XtraGrid.Views.Base.BaseView view in views)
                    {
                        DataView dv = (DataView)view.DataSource;
                        if (dv.Table.TableName == tableName)
                        {
      //                      view.BeginDataUpdate();
                            activeView = view;
                            foreach (TradeRqmtConfirm data in tradeRqmtConfirmCache)
                            {
                                DataRow rowFound = dv.Table.Rows.Find(data.Id);
                                if (rowFound != null)
                                {
                                    CollectionHelper.UpdateDataRowFromObject<TradeRqmtConfirm>(data, ref rowFound);
                                }
                                else
                                {
                                    DataRow newRow = dv.Table.NewRow();
                                    newRow = CollectionHelper.CreateDataRowFromObject<TradeRqmtConfirm>(data, newRow);
                                    dv.Table.Rows.Add(newRow);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Trade Rqmt Confirm grid." + Environment.NewLine +
                    "Error CNF-352 in " + PROJ_FILE_NAME + ".UpdateTradeRqmtConfirmGrid(): " + ex.Message);
            }
            finally
            {
                if (activeView != null)
                {
 //                   activeGrid.Invoke(new EndDataGridUpdate(EndDataUpdate), activeView);
                    tradeRqmtConfirmCache.Clear();
                }
            }
        }

        private void UpdateInboundDocGrid()
        {
            string tableName = "InboundDocsView";
            DevExpress.XtraGrid.Views.Base.BaseView activeView = null;
            DevExpress.XtraGrid.GridControl activeGrid = null;
            try
            {
                foreach (DevExpress.XtraGrid.GridControl grid in managedGridList)
                {
                    DevExpress.XtraGrid.GridControlViewCollection views = grid.Views;
                    activeGrid = grid;
                    foreach (DevExpress.XtraGrid.Views.Base.BaseView view in views)
                    {
                        DataView dv = (DataView)view.DataSource;
                        if (dv.Table.TableName == tableName)
                        {
   //                         view.BeginDataUpdate();
                            activeView = view;
                            foreach (InboundDocsView data in inbDocViewCache)
                            {
                                DataRow rowFound = dv.Table.Rows.Find(data.Id);
                                if (rowFound != null)
                                {
                                    // because inbound_doc_user_flag data is not include, we must copy this data to
                                    // the "data" object that is being used to update the data row found.
                                    if(rowFound["IgnoreFlag"] !=  System.DBNull.Value)
                                        data.IgnoreFlag = rowFound["IgnoreFlag"].ToString();

                                    if (rowFound["BookmarkFlag"] != System.DBNull.Value)
                                        data.BookmarkFlag = rowFound["BookmarkFlag"].ToString();

                                    if (rowFound["CommentFlag"] != System.DBNull.Value)
                                        data.CommentFlag = rowFound["CommentFlag"].ToString();

                                    if (rowFound["CommentUser"] != System.DBNull.Value)
                                        data.CommentUser = rowFound["CommentUser"].ToString(); 
                                    CollectionHelper.UpdateDataRowFromObject<InboundDocsView>(data, ref rowFound);
                                }
                                else
                                {
                                    DataRow newRow = dv.Table.NewRow();
                                    newRow = CollectionHelper.CreateDataRowFromObject<InboundDocsView>(data, newRow);
                                    dv.Table.Rows.Add(newRow);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Inbound Documents grid." + Environment.NewLine +
                    "Error CNF-353 in " + PROJ_FILE_NAME + ".UpdateInboundDocGrid(): " + ex.Message);
            }
            finally
            {
                if (activeView != null)
                {
  //                  activeGrid.Invoke(new EndDataGridUpdate(EndDataUpdate), activeView);
                    inbDocViewCache.Clear();
                }
            }
        }

        private void UpdateAssociatedDocGrid()
        {
            string tableName = "AssociatedDoc";
            DevExpress.XtraGrid.Views.Base.BaseView activeView = null;
            DevExpress.XtraGrid.GridControl activeGrid = null;
            try
            {
                foreach (DevExpress.XtraGrid.GridControl grid in managedGridList)
                {
                    DevExpress.XtraGrid.GridControlViewCollection views = grid.Views;
                    activeGrid = grid;
                    foreach (DevExpress.XtraGrid.Views.Base.BaseView view in views)
                    {
                        DataView dv = (DataView)view.DataSource;
                        if (dv.Table.TableName == tableName)
                        {
   //                         view.BeginDataUpdate();
                            activeView = view;
                            foreach (AssociatedDoc data in assDocDataCache)
                            {
                                DataRow rowFound = dv.Table.Rows.Find(data.Id);
                                if (rowFound != null)
                                {
                                    CollectionHelper.UpdateDataRowFromObject<AssociatedDoc>(data, ref rowFound);
                                }
                                else
                                {
                                    DataRow newRow = dv.Table.NewRow();
                                    newRow = CollectionHelper.CreateDataRowFromObject<AssociatedDoc>(data, newRow);
                                    dv.Table.Rows.Add(newRow);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Associated Documents grid." + Environment.NewLine +
                    "Error CNF-354 in " + PROJ_FILE_NAME + ".UpdateAssociatedDocGrid(): " + ex.Message);
            }
            finally
            {
                if (activeView != null)
                {
  //                  activeGrid.Invoke(new EndDataGridUpdate(EndDataUpdate), activeView);
                    assDocDataCache.Clear();
                }
            }
        }

        private void UpdateTradeRqmtGrid()
        {
            string tableName = "RqmtData";
            DevExpress.XtraGrid.Views.Base.BaseView activeView = null;
            DevExpress.XtraGrid.GridControl activeGrid = null;
            try
            {
                foreach (DevExpress.XtraGrid.GridControl grid in managedGridList)
                {
                    DevExpress.XtraGrid.GridControlViewCollection views = grid.Views;
                    activeGrid = grid;
                    foreach (DevExpress.XtraGrid.Views.Base.BaseView view in views)
                    {
                        DataView dv = (DataView)view.DataSource;
                        if (dv.Table.TableName == tableName)
                        {
   //                         view.BeginDataUpdate();
                            activeView = view;
                            foreach (RqmtData rqmtData in rqmtDataCache)
                            {
                                DataRow rowFound = dv.Table.Rows.Find(rqmtData.Id);
                                if (rowFound != null)
                                {
                                    CollectionHelper.UpdateDataRowFromObject<RqmtData>(rqmtData, ref rowFound);
                                }
                                else
                                {
                                    DataRow newRow = dv.Table.NewRow();
                                    newRow = CollectionHelper.CreateDataRowFromObject<RqmtData>(rqmtData, newRow);
                                    dv.Table.Rows.Add(newRow);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Trade Rqmts grid." + Environment.NewLine +
                    "Error CNF-355 in " + PROJ_FILE_NAME + ".UpdateTradeRqmtGrid(): " + ex.Message);
            }
            finally
            {
                if (activeView != null)
                {
 //                   activeGrid.Invoke(new EndDataGridUpdate(EndDataUpdate), activeView);
                    rqmtDataCache.Clear();
                }
            }
        }

        private void UpdateTradeSummaryGrid()
        {
            string tableName = "SummaryData";
            DevExpress.XtraGrid.Views.Base.BaseView activeView = null;
            DevExpress.XtraGrid.GridControl activeGrid = null;
            try
            {
                foreach (DevExpress.XtraGrid.GridControl grid in managedGridList)
                {
                    DevExpress.XtraGrid.GridControlViewCollection views = grid.Views;
                    activeGrid = grid;
                    foreach (DevExpress.XtraGrid.Views.Base.BaseView view in views)
                    {
                        
                        DataView dv = (DataView)view.DataSource;
                        if (dv.Table.TableName == tableName)
                        {
       //                     view.BeginDataUpdate();
                            activeView = view;
                            foreach (SummaryData sumData in sumDataCache)
                            {
                                DataRow rowFound = dv.Table.Rows.Find(sumData.Id);
                                if (rowFound != null)
                                {
                                    string qryCode = (string)rowFound["QryCode"];
                                    CollectionHelper.UpdateDataRowFromObject<SummaryData>(sumData, ref rowFound);
                                    if (!((qryCode == null) || (qryCode == "")))
                                    {
                                        rowFound["QryCode"] = qryCode; // so as to not overwrite any "Get All" results.
                                    }
                                }
                                else
                                {
                                    DataRow newRow = dv.Table.NewRow();
                                    newRow = CollectionHelper.CreateDataRowFromObject<SummaryData>(sumData, newRow);
                                    dv.Table.Rows.Add(newRow);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Trade Summary grid." + Environment.NewLine +
                    "Error CNF-356 in " + PROJ_FILE_NAME + ".UpdateTradeRqmtGrid(): " + ex.Message);
            }
            finally
            {
                if (activeView != null)
                {
 //                   activeGrid.Invoke(new EndDataGridUpdate(EndDataUpdate), activeView);
                    sumDataCache.Clear();
                }
            }
        }

        public IList<DevExpress.XtraGrid.GridControl> ManagedGridList
        {
            get { return managedGridList; }
            set { managedGridList = value; }
        }

        private void NotifyTradeSummaryArrived(object sender, object data)
        {
            lock (sumDataCache)
            {
                try
                {
                    if (data != null)
                    {
                        SummaryData sumData = (SummaryData)data;
                        sumDataCache.Add(sumData);
                        IncMessageCounter(this, System.EventArgs.Empty);
                    }
                }
                catch (Exception ex)
                {
                    myFireAlarm.ActivateFireAlarm("Error in Notify Trade Summary Arrived. " + ex.Message);
                    throw new Exception("An error occurred while updating the Trade Summary cache with newly arrived messages." + Environment.NewLine +
                        "Error CNF-357 in " + PROJ_FILE_NAME + ".NotifyTradeSummaryArrived(): " + ex.Message);
                }
            }
        }

        private void NotifyInboundDocsArrived(object sender, object data)
        {
            try
            {
                lock (inbDocViewCache)
                {
                    if (data != null)
                    {
                        inbDocViewCache.Add((InboundDocsView)data);
                        IncMessageCounter(this, System.EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                myFireAlarm.ActivateFireAlarm("Error in Notify Inbound Docs Arrived. " + ex.Message);
                throw new Exception("An error occurred while updating the Inbound Docs cache with newly arrived messages." + Environment.NewLine +
                    "Error CNF-358 in " + PROJ_FILE_NAME + ".NotifyInboundDocsArrived(): " + ex.Message);
            }
        }

        private void NotifyAssociatedDocsArrived(object sender, object data)
        {
            try
            {
                lock (assDocDataCache)
                {
                    if (data != null)
                    {
                        assDocDataCache.Add((AssociatedDoc)data);
        //                IncMessageCounter(this, System.EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                myFireAlarm.ActivateFireAlarm("Error in Notify Associated Docs Arrived. " + ex.Message);
                throw new Exception("An error occurred while updating the Associated Docs cache with newly arrived messages." + Environment.NewLine +
                    "Error CNF-359 in " + PROJ_FILE_NAME + ".NotifyAssociatedDocsArrived(): " + ex.Message);
            }
        }

        private void NotifyTradeRqmtConfirmArrived(object sender, object data)
        {
            try
            {
                lock (tradeRqmtConfirmCache)
                {
                    if (data != null)
                    {
                        tradeRqmtConfirmCache.Add((TradeRqmtConfirm)data);
    //                    IncMessageCounter(this, System.EventArgs.Empty);
    //                    IncMessageCounter(this, System.EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                myFireAlarm.ActivateFireAlarm("Error in Notify Trade Rqmt Confirm Arrived. " + ex.Message);
                throw new Exception("An error occurred while updating the Trade Rqmt Confirm cache with newly arrived messages." + Environment.NewLine +
                    "Error CNF-360 in " + PROJ_FILE_NAME + ".NotifyTradeRqmtConfirmArrived(): " + ex.Message);
            }
        }

        private void NotifyTradeRqmtArrived(object sender, object data)
        {
            try
            {
                lock (rqmtDataCache)
                {
                    if (data != null)
                    {
                        rqmtDataCache.Add((RqmtData)data);
    //                    IncMessageCounter(this, System.EventArgs.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                myFireAlarm.ActivateFireAlarm("Error in Notify Trade Rqmt Arrived. " + ex.Message);
                throw new Exception("An error occurred while updating the Trade Rqmt cache with newly arrived messages." + Environment.NewLine +
                    "Error CNF-361 in " + PROJ_FILE_NAME + ".NotifyTradeRqmtArrived(): " + ex.Message);
            }
        }

        private void EndDataUpdate(DevExpress.XtraGrid.Views.Base.BaseView view)
        {
//            view.EndDataUpdate();
        }

        internal void StartListening()
        {
            try
            {
                SummaryData.incAmount = 0;
                RqmtData.incAmount = 0;
                listenerManager.StartListener();
                opsTrackingClock.Start();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while starting the trade update Message Listener." + Environment.NewLine +
                    "Error CNF-362 in " + PROJ_FILE_NAME + ".StartListening(): " + ex.Message);
            }
        }

        public Timer OpsTrackingClock
        {
            get { return opsTrackingClock; }
            set { opsTrackingClock = value; }
        }

        internal void StopListening()
        {
            try
            {
                listenerManager.StopListener();
                opsTrackingClock.Stop();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while stopping the trade update Message Listener." + Environment.NewLine +
                    "Error CNF-363 in " + PROJ_FILE_NAME + ".StopListening(): " + ex.Message);
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                listenerManager.StopListener();
                listenerManager = null;
            }
            catch (Exception ex)
            {
                myFireAlarm.ActivateFireAlarm("Error in OpsGridManager Dispose " + ex.Message);
                throw new Exception("An error occurred while disposing the trade update Message Listener." + Environment.NewLine +
                    "Error CNF-364 in " + PROJ_FILE_NAME + ".Dispose(): " + ex.Message);
            }
        }
        #endregion

        internal IList<SummaryData> GetSummaryDataCache()
        {
            return sumDataCacheCopy;
        }

        internal IList<RqmtData> GetRqmtDataCache()
        {
            return rqmtDataCacheCopy;
        }


        internal IList<InboundDocsView> GetInboundDocViewDataCache()
        {
            return inbDocViewCacheCopy;
        }

        internal IList<AssociatedDoc> GetAssDocDataCache()
        {
            return assDocDataCacheCopy;
        }

        internal IList<TradeRqmtConfirm> GetTradeRqmtConfirmCache()
        {
            return tradeRqmtConfirmCacheCopy;
        }
    }
}
