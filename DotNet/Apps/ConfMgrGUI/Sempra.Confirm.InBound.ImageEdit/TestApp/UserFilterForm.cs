using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using DevExpress.XtraEditors.Controls;

namespace InboundDocuments
{
    public partial class UserFilterForm : Form
    {
        public UserFilterForm()
        {
            InitializeComponent();
            InitializeControlsValue();
        }

        private void InitializeControlsValue()
        {
            DataSet ds = FilterUtil.GetCommoditListFromDb();
            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; ++i)
                {
                    DataRow row = dt.Rows[i];
                    object code =  row["Code"];
                    CheckedListBoxItem item = new CheckedListBoxItem(code);
                    cdtyList.Items.Add(item);
                }

            }
            PopulateDropDown(cboStart);
            PopulateDropDown(cboEnd);
            LoadInitialValues();
        }

        private void LoadInitialValues()
        {
            Hashtable hs = FilterUtil.GetUserFilters();
            int totalChecked = 0;
            if (hs != null)
            {
                ArrayList filterCdty = (ArrayList) hs["Commodity"];
                if (filterCdty != null)
                {
                    for (int i = 0; i < filterCdty.Count; ++i)
                    {
                        string code = (string)filterCdty[i];
                        for (int j = 0; j < cdtyList.Items.Count; ++j)
                        {
                            if (cdtyList.Items[j].Value.ToString().Equals(code))
                            {
                                cdtyList.Items[j].CheckState = CheckState.Checked;
                                totalChecked++;
                                break;
                            }
                        }
                    }
                }
                if (totalChecked == cdtyList.Items.Count)
                {
                    checkEdit1.CheckState = CheckState.Checked;
                }
                Hashtable condition = (Hashtable)hs["Counterparty"];
                int index = 0;
                if (condition != null)
                {

                    IDictionaryEnumerator iterator = condition.GetEnumerator();
                    shortName.Checked = true;
                    while (iterator.MoveNext())
                    {

                        string key = (string)iterator.Key;
                        string value = (string)iterator.Value;
                        if ("AppliesTo".Equals(key, StringComparison.CurrentCultureIgnoreCase))
                        {
                            if ("Legal Name".Equals(value, StringComparison.CurrentCultureIgnoreCase))
                            {
                                legalName.Checked = true;
                            }
                            continue;
                        }

                        
                        if (index == 0)
                        {
                            cboStart.SelectedItem = key;
                            txtStart.Text = value;
                        }
                        else
                        {
                            cboEnd.SelectedItem = key;
                            txtEnd.Text = value;
                        }
                        index++;
                    }
                }
            }
        }
        private void PopulateDropDown(ComboBox drop)
        {
            object item = "Please select";
            drop.Items.Add(item);
            drop.SelectedIndex = 0;
            item = ">";
            drop.Items.Add(item);
            item = ">=";
            drop.Items.Add(item);
            item = "<";
            drop.Items.Add(item);
            item = "<=";
            drop.Items.Add(item);
        }

        private void checkEdit1_CheckStateChanged(object sender, EventArgs e)
        {
       //     if (checkEdit1.Checked)
       //     {
                for ( int i = 0;i<cdtyList.Items.Count; ++i) {
                    cdtyList.Items[i].CheckState = checkEdit1.CheckState;
                }
       //     }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (ValidData())
            {
                SaveData();
            }
            else
            {
                this.DialogResult = DialogResult.None;
            }
            
        }

        private bool ValidData()
        {
            if ((cboStart.SelectedIndex > 0) && "".Equals(txtStart.Text.Trim()))
            {
                MessageBox.Show("Please enter the starting filter condition value.", "Inbound");
                txtStart.Focus();
                return false;
            }
            if ((cboEnd.SelectedIndex > 0) && "".Equals(txtEnd.Text.Trim()))
            {
                MessageBox.Show("Please enter the ending filter condition value.", "Inbound");
                txtEnd.Focus();
                return false;
            }
            return true;
        }
        private void SaveData()
        {
            
            Hashtable hs = new Hashtable();
            ArrayList commoditList = new ArrayList();
            for (int i = 0; i < cdtyList.Items.Count; ++i)
            {
                if (cdtyList.Items[i].CheckState == CheckState.Checked)
                {
                    commoditList.Add(cdtyList.Items[i].Value);
                }
            }
            hs["Commodity"] = commoditList;
            Hashtable condition = new Hashtable();
            if (this.legalName.Checked)
            {
                condition["AppliesTo"] = this.legalName.Text;
            }
            else
            {
                condition["AppliesTo"] = this.shortName.Text;
            }

            if (!"Please select".Equals(cboStart.SelectedItem))
            {
                condition[cboStart.SelectedItem] = txtStart.Text;
            }
            if (!"Please select".Equals(cboEnd.SelectedItem))
            {
                condition[cboEnd.SelectedItem] = txtEnd.Text;
            }
            hs["Counterparty"] = condition;
            FilterUtil.SaveUserFilters(hs);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}