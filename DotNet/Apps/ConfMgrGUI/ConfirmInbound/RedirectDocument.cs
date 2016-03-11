using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ConfirmInbound
{
    public partial class RedirectDocument : DevExpress.XtraEditors.XtraForm
    {
        private DataTable dtFaxNumbers;
        private string newFaxNum;

        public string NewFaxNum
        {
            get { return newFaxNum; }
            set { newFaxNum = value; }
        }

        public RedirectDocument(DataTable dt)
        {
            dtFaxNumbers = dt;
            InitializeComponent();
            InitLookup();
        }

        internal void InitLookup()
        {
            dtFaxNumbers.DefaultView.Sort = "FAXNO";
            ddCptyCode.Properties.DataSource = dtFaxNumbers;
            ddCptyCode.Properties.DisplayMember = "FAXNO";
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            this.newFaxNum = ddCptyCode.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.newFaxNum = "";
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}