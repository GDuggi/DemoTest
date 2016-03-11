using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;

namespace InboundDocuments
{
    public partial class CallerRefList : Form
    {
        private static readonly string _SPAM_INACTIVE_SQL = "update ops_tracking.inbound_doc_fax_spam set active_flag='N' where  caller_ref = :caller_ref ";
        private static readonly string _FAX_INACTIVE_SQL = "update ops_tracking.inbound_doc_caller_ref set active_flag='N' where caller_ref = :caller_ref ";

        private OracleConnection oc = null;
        private string connectionString;
 
        public CallerRefList()
        {
            InitializeComponent();
        }

        private void CallerRefList_Load(object sender, EventArgs e)
        {
            
            
        }

        public void SetOracleConnection(String connString)
        {


            connectionString = connString;
            try
            {
                RefreshCptyData();
                RefreshSpamData();
                
            }
            catch (OracleException e)
            {
                MessageBox.Show("Database Error : " + e.Message);
            }
            finally
            {
                if (oc != null && oc.State == ConnectionState.Open)
                {
                    try
                    {
                        oc.Close();
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            InboundDocuments.RefEditForm.FaxDataType dataType ;
            if (this.tabPage.SelectedTabPageIndex == 0 ){
                dataType = RefEditForm.FaxDataType.CounterPartyFax;
            }
            else {
                dataType = RefEditForm.FaxDataType.SpamFax;
            }
            RefEditForm editForm = new RefEditForm(dataType, RefEditForm.FormMode.Add, this.connectionString, "", "");
            if ( editForm.ShowDialog(this) == DialogResult.OK ){
                if (this.tabPage.SelectedTabPageIndex == 0)
                {
                    RefreshCptyData();
                }
                else
                {
                    RefreshSpamData();
                }
            }

            
        }

        private void cmdEdit_Click(object sender, EventArgs e)
        {
            InboundDocuments.RefEditForm.FaxDataType dataType;
            string faxNumber = null;
            string codeDesc = null;
            if (this.tabPage.SelectedTabPageIndex == 0)
            {
                dataType = RefEditForm.FaxDataType.CounterPartyFax;
                int rowHandle = this.gridView1.FocusedRowHandle;
                faxNumber = (string) this.gridView1.GetRowCellValue(rowHandle, "CALLER_REF");
                codeDesc = (string) this.gridView1.GetRowCellValue(rowHandle, "CPTY_SHORT_CODE");

            }
            else
            {
                dataType = RefEditForm.FaxDataType.SpamFax;
                int rowHandle = this.gridView2.FocusedRowHandle;
                faxNumber = (string)this.gridView2.GetRowCellValue(rowHandle, "CALLER_REF");
                codeDesc = (string)this.gridView2.GetRowCellValue(rowHandle, "DESCRIPTION");
            }
            RefEditForm editForm = new RefEditForm(dataType, RefEditForm.FormMode.Edit, this.connectionString, faxNumber, codeDesc);
            if (editForm.ShowDialog(this) == DialogResult.OK)
            {
                if (this.tabPage.SelectedTabPageIndex == 0)
                {
                    RefreshCptyData();
                }
                else
                {
                    RefreshSpamData();
                }
            }

        }

        private void RefreshCptyData()
        {

            
            try
            {
                oc = new OracleConnection(this.connectionString);
                this.iNBOUND_DOC_CALLER_REFTableAdapter.Connection = oc;
            
                this.iNBOUND_DOC_CALLER_REFTableAdapter.Fill(this.cptyFax.INBOUND_DOC_CALLER_REF);

            }
            catch (OracleException e)
            {
                MessageBox.Show("Database Error : " + e.Message);
            }
            finally
            {
                if (oc != null && oc.State == ConnectionState.Open)
                {
                    try
                    {
                        oc.Close();
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

        }
        private void RefreshSpamData()
        {

            
            try
            {
                oc = new OracleConnection(this.connectionString);
                this.iNBOUND_DOC_FAX_SPAMTableAdapter.Connection = oc;
                this.iNBOUND_DOC_FAX_SPAMTableAdapter.Fill(this.spamFax.INBOUND_DOC_FAX_SPAM);

            }
            catch (OracleException e)
            {
                MessageBox.Show("Database Error : " + e.Message);
            }
            finally
            {
                if (oc != null && oc.State == ConnectionState.Open)
                {
                    try
                    {
                        oc.Close();
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

        }
        private void deleteData(string sql, string faxNumber)
        {
            OracleConnection conn = null;
            OracleCommand cmd = null;

            try
            {
                conn = new OracleConnection(this.connectionString);
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.Add("caller_ref", OracleType.VarChar);
                cmd.Parameters["caller_ref"].Value = faxNumber;
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
            }
            finally
            {
                try
                {
                    if (conn != null && conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                catch (Exception e)
                {
                }

            }
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            string faxNumber = null;
            string sql = null;
            if (this.tabPage.SelectedTabPageIndex == 0)
            {
                int rowHandle = this.gridView1.FocusedRowHandle;
                faxNumber = (string)this.gridView1.GetRowCellValue(rowHandle, "CALLER_REF");
                if (MessageBox.Show("Do you want to delete the fax number " + faxNumber + "?", "Confirm", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    sql = _FAX_INACTIVE_SQL;
                    deleteData(sql, faxNumber);
                    RefreshCptyData();
                 
                }
            }
            else
            {
                int rowHandle = this.gridView2.FocusedRowHandle;
                faxNumber = (string)this.gridView2.GetRowCellValue(rowHandle, "CALLER_REF");
                if (MessageBox.Show("Do you want to delete the fax number " + faxNumber + "?", "Confirm", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    sql = _SPAM_INACTIVE_SQL;
                    deleteData(sql, faxNumber);
                    RefreshSpamData();
                }
            }

            
        }


    }
}