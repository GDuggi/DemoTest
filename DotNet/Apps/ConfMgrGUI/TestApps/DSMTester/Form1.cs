using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DataManager;
using OpsTrackingModel;

namespace DSMTester
{
    public partial class Form1 : Form
    {
        private DSManager dataManager;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataManager = new DSManager("","","",10,"", "");
            SummaryData summaryDataTemplate = new SummaryData();
            summaryDataTemplate.FinalApprovalFlag = "N";
            IList<SummaryData> summaryList = dataManager.DaoManager.CreateObjList<SummaryData>(summaryDataTemplate);
        }
    }
}
