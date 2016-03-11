using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;

namespace ConfirmInbound
{
    public partial class RefEditForm : Form
    {
        public enum FormMode { Add, Edit };
        public enum FaxDataType { CounterPartyFax, SpamFax }

        FaxDataType faxType;

        public FaxDataType FaxType
        {
            get { return faxType; }
            set 
            {
                faxType = value; 
            }
        }

        string callerReference;
        public string CallerReference
        {
            get { return callerReference; }
            set 
            { 
                callerReference = value;
                this.txtCallerRef.Text = this.callerReference;
            }
        }
        string cptyMapping;

        public string CptyMapping
        {
            get { return cptyMapping; }
            set 
            {
                cptyMapping = value;
                this.ddCptyCode.EditValue = this.cptyMapping;
            }
        }

        public string referencedCptyName = "";

        public RefEditForm()
        {
            InitializeComponent();
        }

        private void PopulateCptyData()
        {

        }

        private void PopualteSpamData()
        {
        }

        private void SaveCallerRefMapping(string[] sqlList, string faxNum, string shortCodeDesc)
        {
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            this.cptyMapping = ddCptyCode.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        internal void InitLookup(DataTable cptyLkupTable)
        {
            cptyLkupTable.DefaultView.Sort = "CptySn";
            ddCptyCode.Properties.DataSource = cptyLkupTable;
            ddCptyCode.Properties.DisplayMember = "CptySn";
        }
    }
}