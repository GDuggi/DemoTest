using System;
using System.Collections.Generic;
using System.Data;
using OpsTrackingModel;
//using GigaSpaces.Core;

namespace DataManager
{
    public class DSManager
    {
        private const string PROJ_FILE_NAME = "DSManager";
        private DataSet managedDataSet = null;
        private OpsGridManager opsGridManager = null;

        public event System.EventHandler IncMessageCounter;
        public event System.EventHandler ResetMessageCounter;
        public event System.EventHandler BeginGridUpdates;
        public event System.EventHandler EndGridUpdates;

        private string updMsgServer = null;
        private string updMsgUser = null;
        private string updMsgPwd = null;
        private string msgFilter = "";
        private string inbMsgFilter = "";
        private DAOManager daoManager = null;

        public DAOManager DaoManager
        {
            get { return daoManager; }
        }

        private int updateFromCacheTimerInterval = 90; // default

        public DSManager(string pUpdMsgServer, string pUpdMsgUser, string pUpdMsgPwd, int updateFromCacheTimerInterval,
            string msgFilter, string inbMsgFilter, List<string> pPermKeyList, bool pIsSuperUser)
        {
            try
            {
                this.updMsgServer = pUpdMsgServer;
                this.updMsgUser = pUpdMsgUser;
                this.updMsgPwd = pUpdMsgPwd;
                this.updateFromCacheTimerInterval = updateFromCacheTimerInterval;
                this.msgFilter = msgFilter;
                this.inbMsgFilter = inbMsgFilter;
                InitManager(pPermKeyList, pIsSuperUser);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating and populating internal data storage." + Environment.NewLine +
                    "Error CNF-345 in " + PROJ_FILE_NAME + ".DSManager(): " + ex.Message);
            }
        }

        private void InitManager(List<string> pPermKeyList, bool pIsSuperUser)
        {
            try
            {
                // Check out the use of CreateObjectRef......
                opsGridManager = new OpsGridManager(updMsgServer, updMsgUser, updMsgPwd, updateFromCacheTimerInterval, 
                    msgFilter, inbMsgFilter, pPermKeyList, pIsSuperUser);
                daoManager = new DAOManager();
                opsGridManager.IncMessageCounter += new EventHandler(OnIncMessageCounter);
                opsGridManager.ResetMessageCounter += new EventHandler(OnResetMessageCounter);
                opsGridManager.BeginGridUpdates += new EventHandler(OnBeginGridUpdates);
                opsGridManager.EndGridUpdates += new EventHandler(OnEndGridUpdates);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating and populating internal data storage." + Environment.NewLine +
                    "Error CNF-346 in " + PROJ_FILE_NAME + ".InitManager(): " + ex.Message);
            }
        }

        private void OnBeginGridUpdates(object sender, System.EventArgs e)
        {
            try
            {
                if (BeginGridUpdates != null)
                {
                    BeginGridUpdates(this, System.EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while initiating an update of the grid." + Environment.NewLine +
                    "Error CNF-347 in " + PROJ_FILE_NAME + ".OnBeginGridUpdates(): " + ex.Message);
            }
        }

        private void OnEndGridUpdates(object sender, System.EventArgs e)
        {
            try
            {
                if (EndGridUpdates != null)
                {
                    EndGridUpdates(this, System.EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while concluding an update of the grid." + Environment.NewLine +
                    "Error CNF-348 in " + PROJ_FILE_NAME + ".OnEndGridUpdates(): " + ex.Message);
            }
        }

        private void OnIncMessageCounter(object sender, System.EventArgs e)
        {
            if (IncMessageCounter != null)
            {
                IncMessageCounter(this, System.EventArgs.Empty);
            }
        }

        private void OnResetMessageCounter(object sender, System.EventArgs e)
        {
            if (ResetMessageCounter != null)
            {
                ResetMessageCounter(this, System.EventArgs.Empty);
            }
        }

        public DataSet ManagedDataSet
        {
            get { return managedDataSet; }
            set { managedDataSet = value; }
        }

        public void RegisterDataSet(ref DataSet appDataSet)
        {
            managedDataSet = appDataSet;
        }

        public void UnRegisterDataSet()
        {
            managedDataSet = null;
        }

        public void StartListening()
        {
            opsGridManager.StartListening();
        }

        public void ExecuteClientUpdates()
        {
            opsGridManager.ExecuteOpsTrackingUpdates();
        }

        //public void RegisterGridControl(ref DevExpress.XtraGrid.GridControl gridControl)
        public void RegisterGridControl(ref DevExpress.XtraGrid.GridControl gridControl)
        {
            opsGridManager.ManagedGridList.Add(gridControl);
        }

        public void UnRegisterGridControl(ref DevExpress.XtraGrid.GridControl gridControl)
        {
            if (opsGridManager.ManagedGridList.IndexOf(gridControl) >= 0)
            {
                opsGridManager.ManagedGridList.Remove(gridControl);
            }
        }

        public void CleanUp()
        {
            daoManager = null;
            managedDataSet = null;
            opsGridManager.Dispose();
        }

        public IList<SummaryData> GetSummaryDataCache()
        {
            return opsGridManager.GetSummaryDataCache();
        }

        public void PrepareOpsTrackingUpdates()
        {
            opsGridManager.PrepareOpsTrackingUpdates();
        }

        public IList<RqmtData> GetRqmtDataCache()
        {
            return opsGridManager.GetRqmtDataCache();
        }

        public void StopOpsTimer()
        {
            opsGridManager.OpsTrackingClock.Stop();
        }

        public void StartOpsTimer()
        {
            opsGridManager.OpsTrackingClock.Start();
        }

        public IList<InboundDocsView> GetInboundDocViewDataCache()
        {
            return opsGridManager.GetInboundDocViewDataCache();
        }

        public IList<AssociatedDoc> GetAssDocDataCache()
        {
            return opsGridManager.GetAssDocDataCache();
        }

        public IList<TradeRqmtConfirm> GetTradeRqmtConfirmCache()
        {
            return opsGridManager.GetTradeRqmtConfirmCache();
        }
    }
}
