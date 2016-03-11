using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpsManagerNotifier;
using DataManager;
using OpsTrackingModel;

using DevExpress.Skins;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

using System.IO;
using System.Reflection;


namespace ApacheNMS_Test
{
    public partial class frmTestOpsMgrAPI : Form
    {
        private ListenerManager listenerManager = null;
        private string jmsServer = "stomp:tcp://aff01inf01:61613";
        private string jmsUser = "sempra.ops.gs.service";
        private string jmsPassword = "sempra";
        private string msgFilter = "";
        private string msgTopic = "sempra.ops.opsTracking.summary.update";
        private int cacheUpdateTimerInterval = 999999; //Int32.MaxValue;
        private bool isListenerRunning = false;
        private DSManager dataManager = null;
        private DataSet dataSet = null;
        private DataTable summaryDataTable;

        public frmTestOpsMgrAPI()
        {
            InitializeComponent();
        }

        private void frmTestOpsMgrAPI_Load(object sender, EventArgs e)
        {
            rtxtMsgRcv.AppendText("Initializing form..." + timeStamp(DateTime.Now) + "\n");
            InitForm();
            rtxtMsgRcv.AppendText("Initialization complete." + timeStamp(DateTime.Now) + "\n");
        }

        public void InitForm()
        {
            try
            {
                dataManager = new DSManager(jmsServer, jmsUser, jmsPassword, cacheUpdateTimerInterval, msgFilter, msgFilter);
                dataManager.IncMessageCounter += new EventHandler(OnIncMessageCounter);

                dataSet = new DataSet();

                SummaryData summaryDataTemplate = new SummaryData();
                IList<SummaryData> summaryList;
                summaryList = dataManager.DaoManager.CreateObjList<SummaryData>(summaryDataTemplate);
                string filterText = "";  //G
                summaryDataTable = CollectionHelper.ConvertTo<SummaryData>(summaryList, filterText);
                summaryDataTable.PrimaryKey = new DataColumn[] { summaryDataTable.Columns["Id"] };

                dataSet.Tables.Add(summaryDataTable);

                dataManager.RegisterDataSet(ref dataSet);
                dataManager.RegisterGridControl(ref gridMain);
                dataManager.StartListening();

                gridMain.DataSource = dataSet.Tables["SummaryData"];
                gridMain.ForceInitialize();

            }
            catch (Exception error)
            {
                MessageBox.Show("Error:" + error.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            rtxtMsgRcv.AppendText("Message Received..." + timeStamp(DateTime.Now) + "\n");
        }

        public String timeStamp(DateTime value)
        {
            return value.ToString("yyyy-MM-dd_HH:mm:ss_ffff");
        }

        public void closeForm()
        {
            shutdownListener();
        }

        public void shutdownListener()
        {
            if (isListenerRunning)
            {
                listenerManager.StopListener();
                isListenerRunning = false;
            }
        }

        delegate void OnIncMessageCounterCallback(object sender, System.EventArgs e);
        public void OnIncMessageCounter(object sender, System.EventArgs e)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.rtxtMsgRcv.InvokeRequired)
            {
                OnIncMessageCounterCallback d = new OnIncMessageCounterCallback(OnIncMessageCounter);
                this.Invoke(d, new object[] { sender, e });
            }
            else
            {
                try
                {
                    rtxtMsgRcv.AppendText("Message Received..." + timeStamp(DateTime.Now) + "\n");
                    IList<SummaryData> summaryDataCache = new List<SummaryData>();
                    
                    //Doesn't work...
                    //summaryDataCache = (sender as DataManager.DSManager).GetSummaryDataCache();
                }
                catch (Exception except)
                {
                    throw new Exception("OnIncMessageCounter: " + except.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fullClassName = "aff.confirm.opsmanager.opssubpub.opstrackingmodel.SummaryData";
            int len = fullClassName.LastIndexOf(".");
            string className = fullClassName.Remove(0, len+1);
            rtxtMsgRcv.AppendText("fullClassName=" + fullClassName + "\n");
            rtxtMsgRcv.AppendText("className=" + className + "\n");


            //string appPath = Path.GetDirectoryName(Application.ExecutablePath);
            //Assembly assembly = Assembly.LoadFrom(appPath + @"\OpsManagerModel.dll");
            //foreach (Type t in assembly.GetTypes())
            //{
            //    rtxtMsgRcv.AppendText(t.Name.ToString() + "\n");
            //}
        }

    }
}
