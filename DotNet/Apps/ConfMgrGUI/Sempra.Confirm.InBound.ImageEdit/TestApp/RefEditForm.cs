using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OracleClient;
using DevExpress.XtraGrid.Columns;

namespace InboundDocuments
{
    public partial class RefEditForm : Form
    {
        private static readonly string _CPTY_SQL = "select short_name,legal_name from cpty.cpty order by short_name";
        private static readonly string _FAX_CHECK_SQL = "select * from ops_tracking.inbound_doc_caller_ref where caller_ref = :caller_ref";
        private static readonly string _FAX_UPDATE_SQL = "update ops_tracking.inbound_doc_caller_ref set caller_ref = :caller_ref, " +
                                                         "cpty_short_code = :short_code, active_flag='Y' where caller_ref_id = :ref_id";

        private static readonly string _FAX_INSERT_SQL = "insert into ops_tracking.inbound_doc_caller_ref (caller_ref_id, caller_ref,cpty_short_code,active_flag) values (" +
                                                          " ops_tracking.seq_inbound_doc_caller_ref.nextval,:caller_ref, :short_code, 'Y') ";



        private static readonly string _SPAM_CHECK_SQL = "select * from ops_tracking.inbound_doc_fax_spam where caller_ref = :caller_ref";
        private static readonly string _SPAM_UPDATE_SQL = "update ops_tracking.inbound_doc_caller_ref set caller_ref = :caller_ref, " +
                                                         " description = :short_code, active_flag='Y' where id = :ref_id";

        private static readonly string _SPAM_INSERT_SQL = "insert into ops_tracking.inbound_doc_fax_spam (id, caller_ref,description,active_flag) values (" +
                                                          " ops_tracking.seq_inbound_doc_fax_spam.nextval,:caller_ref, :short_code, 'Y') ";


        public enum FormMode {Add, Edit};
        public enum FaxDataType {CounterPartyFax,SpamFax}

        FaxDataType faxType;
        FormMode formMode;
        string faxNumber;
        string codeDesc;
        string connectionString;
        DataView dv;

        public RefEditForm(FaxDataType faxType, FormMode mode, string connectionStr, string faxNumber, string codeDesc)
        {
            InitializeComponent();
            this.faxType = faxType;
            this.formMode = mode;
            this.faxNumber = faxNumber;
            this.codeDesc = codeDesc;
            this.connectionString = connectionStr;
            if (faxType == FaxDataType.SpamFax)
            {
                PopualteSpamData();
            }
            else
            {
                PopulateCptyData();
            }
        }

        private void PopulateCptyData()
        {

            this.Text = "Cpty Fax Mapping";
            this.lblCode.Text = "Cpty Short Name:";
            this.ddCptyCode.Visible = true;
            this.txtDesc.Visible = false;
            

            DataSet ds = new DataSet();
            OracleDataAdapter adapter = new OracleDataAdapter(_CPTY_SQL, connectionString);
            adapter.Fill(ds,"Cpty");
            DataViewManager dvm = new DataViewManager(ds);
            dv = dvm.CreateDataView(ds.Tables["Cpty"]);
            dv.Sort = "SHORT_NAME";

            this.ddCptyCode.Properties.DataSource = dv;
            this.ddCptyCode.Properties.ValidateOnEnterKey = true;
            this.ddCptyCode.DataBindings.Add("EditValue", dv, "SHORT_NAME");
            this.ddCptyCode.Properties.ValueMember = "SHORT_NAME";
            this.ddCptyCode.Properties.DisplayMember = "SHORT_NAME";

            
            GridColumn col1 = ddCptyCode.Properties.View.Columns.ColumnByFieldName("SHORT_NAME");
            col1.VisibleIndex = 0;
            col1.Caption = "Short Name";
            GridColumn col2 = ddCptyCode.Properties.View.Columns.ColumnByFieldName("LEGAL_NAME");
            col2.VisibleIndex = 1;
            col2.Caption = "Name";

            this.txtFax.Text = this.faxNumber;
            this.ddCptyCode.EditValue = this.codeDesc;
           
            

        }

        private void PopualteSpamData()
        {
            this.Text = "Junk Fax Mapping";
            this.lblCode.Text = "Description";
            this.ddCptyCode.Visible = false;
            this.txtDesc.Visible = true;
            this.txtDesc.Location = this.ddCptyCode.Location;

           
        }

        private void SaveCptyFax(string[] sqlList,string faxNum,string shortCodeDesc)
        {
            OracleConnection conn = null;
            OracleCommand cmd = null;
            OracleCommand updCmd = null;

            string selectSQL = sqlList[0];
            string insertSQL = sqlList[1];
            string updateSQl = sqlList[2];

            try
            {
                conn = new OracleConnection(this.connectionString);
                conn.Open();
                cmd = conn.CreateCommand();
                cmd.CommandText = selectSQL;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("caller_ref", OracleType.VarChar);
                cmd.Parameters["caller_ref"].Value = faxNum;
                OracleDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    String activeFlag = reader.GetString(reader.GetOrdinal("ACTIVE_FLAG"));
                    if ("Y".Equals(activeFlag, StringComparison.CurrentCultureIgnoreCase) && !faxNum.Equals(this.faxNumber))
                    {
                        MessageBox.Show("The fax number is already mapped, please enter a new fax number");
                        return;
                    }
                    // inactive fax 
                    
                     int refId = reader.GetInt32(reader.GetOrdinal("CALLER_REF_ID"));
                     updCmd = conn.CreateCommand();
                     updCmd.CommandText = updateSQl;
                     updCmd.Parameters.Add("caller_ref",OracleType.VarChar);
                     updCmd.Parameters["caller_ref"].Value = faxNum;
                     updCmd.Parameters.Add("short_code",OracleType.VarChar);
                     updCmd.Parameters["short_code"].Value = shortCodeDesc;
                     updCmd.Parameters.Add("ref_id",OracleType.Int32);
                     updCmd.Parameters["ref_id"].Value = refId;
                     updCmd.ExecuteNonQuery();

                    
                }
                else  // insert new fax
                {

                    updCmd = conn.CreateCommand();
                    updCmd.CommandText = insertSQL;
                    updCmd.Parameters.Add("caller_ref",OracleType.VarChar);
                    updCmd.Parameters["caller_ref"].Value = faxNum;
                    updCmd.Parameters.Add("short_code",OracleType.VarChar);
                    updCmd.Parameters["short_code"].Value = shortCodeDesc;
                    updCmd.ExecuteNonQuery();

                }
                this.DialogResult = DialogResult.OK;
                this.Close();

            }
            catch (OracleException e)
            {
                MessageBox.Show("Database Error: " + e.Message);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
            finally
            {
                try
                {
                    cmd = null;
                    updCmd = null;
                    if (conn != null)
                    {
                        conn.Close();
                    }

                }
                catch (Exception e)
                {
                }
            }

        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            string[] sqlList = new string[3];
            string faxNumInfo = null;
            string shortCodeDesc = null;

            if (this.faxType == FaxDataType.SpamFax)
            {
                if ("".Equals(this.txtFax.Text))
                {
                    MessageBox.Show("Please enter the fax number");
                    this.txtFax.Focus();
                    return;
                }
                if ("".Equals(this.txtDesc.Text))
                {
                    MessageBox.Show("Please enter the description");
                    this.txtDesc.Focus();
                    return;
                }
                faxNumInfo = this.txtFax.Text.Trim();
                shortCodeDesc = this.txtDesc.Text;
                sqlList = new string[] { _SPAM_CHECK_SQL, _SPAM_INSERT_SQL, _SPAM_UPDATE_SQL };

            }
            else
            {
                if ("".Equals(this.txtFax.Text))
                {
                    MessageBox.Show("Please enter the fax number");
                    this.txtFax.Focus();
                    return;
                }
                if ("".Equals(this.ddCptyCode.EditValue.ToString()))
                {
                    MessageBox.Show("Please select the cpty");
                    this.ddCptyCode.Focus();
                    return;
                }
                faxNumInfo = this.txtFax.Text.Trim();
                shortCodeDesc = this.ddCptyCode.EditValue.ToString().Trim();
                sqlList = new string[]{_FAX_CHECK_SQL,_FAX_INSERT_SQL,_FAX_UPDATE_SQL};
            }

            SaveCptyFax(sqlList,faxNumInfo,shortCodeDesc);
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void RefEditForm_Load(object sender, EventArgs e)
        {
            this.txtFax.Text = this.faxNumber;
            if (this.faxType == FaxDataType.SpamFax)
            {
                this.txtDesc.Text = this.codeDesc;
            }
            else
            {
                this.ddCptyCode.EditValue = this.codeDesc;
            }
        }
        
    }

}