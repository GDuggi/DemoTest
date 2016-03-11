using System;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using System.Collections;

namespace ConfirmInbound
{
    public partial class UserFilterForm : Form
    {     

        public bool applyChanges = false;

        public UserFilterForm()
        {
            InitializeComponent();            
        }

        public void SetFaxNOsFromDb(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                DataRow row = dt.Rows[i];
                object code = row["FAXNO"];
                CheckedListBoxItem item = new CheckedListBoxItem(code);
                chklistFaxNos.Items.Add(item);
            }

            PopulateDropDown(cboStart);
            PopulateDropDown(cboEnd);
            LoadInitialValues();
        }

        private void LoadInitialValues()
        {
            Hashtable hs = FilterUtil.GetUserFilters();

            if (hs == null) return;

            ArrayList panelViewList = (ArrayList)hs["PanelViews"];

            if (panelViewList != null)
            {
                for (int i = 0; i < cmboPanel1View.Properties.Items.Count; i++)
                {
                    if ((string)cmboPanel1View.Properties.Items[i] == (string)panelViewList[0])
                        cmboPanel1View.SelectedIndex = i;
                }

                for (int i = 0; i < cmboPanel2View.Properties.Items.Count; i++)
                {
                    if ((string)cmboPanel2View.Properties.Items[i] == (string)panelViewList[1])
                        cmboPanel2View.SelectedIndex = i;
                }

                for (int i = 0; i < cmboPanel3View.Properties.Items.Count; i++)
                {
                    if ((string)cmboPanel3View.Properties.Items[i] == (string)panelViewList[2])
                        cmboPanel3View.SelectedIndex = i;
                }

                for (int i = 0; i < cmboPanel4View.Properties.Items.Count; i++)
                {
                    if ((string)cmboPanel4View.Properties.Items[i] == (string)panelViewList[3])
                        cmboPanel4View.SelectedIndex = i;
                }
            }

            int totalChecked = 0;
            if (hs != null)
            {
                ArrayList filterCdty = (ArrayList)hs["Commodity"];
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


                ArrayList filterFaxNos = (ArrayList)hs["FaxNos"];
                totalChecked = 0;
                if (filterFaxNos != null)
                {
                    for (int i = 0; i < filterFaxNos.Count; ++i)
                    {
                        string faxNo = (string)filterFaxNos[i];
                        for (int j = 0; j < chklistFaxNos.Items.Count; ++j)
                        {
                            if (chklistFaxNos.Items[j].Value.ToString().Equals(faxNo))
                            {
                                chklistFaxNos.Items[j].CheckState = CheckState.Checked;
                                totalChecked++;
                                break;
                            }
                        }
                    }
                }
                if (totalChecked == chklistFaxNos.Items.Count)
                {
                    checkEdit5.CheckState = CheckState.Checked;
                }

                ArrayList filterDocs = (ArrayList)hs["Filter"];
                if (filterDocs != null)
                {
                    for (int i = 0; i < filterDocs.Count; ++i)
                    {
                        chkExcludeIgnoredDocs.Checked = ((string)filterDocs[i]).Equals("EXCLUDED");
                    }
                }


                ArrayList filterCCY = (ArrayList)hs["Currency"];
                totalChecked = 0;
                if (filterCCY != null)
                {
                    for (int i = 0; i < filterCCY.Count; ++i)
                    {
                        string code = (string)filterCCY[i];
                        for (int j = 0; j < ccyList.Items.Count; ++j)
                        {
                            if (ccyList.Items[j].Value.ToString().Equals(code))
                            {
                                ccyList.Items[j].CheckState = CheckState.Checked;
                                totalChecked++;
                                break;
                            }
                        }
                    }
                }
                if (totalChecked == ccyList.Items.Count)
                {
                    checkEdit2.CheckState = CheckState.Checked;
                }


                ArrayList cptyAddList = (ArrayList)hs["CptyAdditional"];
                totalChecked = 0;
                if (filterCCY != null)
                {
                    for (int i = 0; i < cptyAddList.Count; ++i)
                    {
                        string code = (string)cptyAddList[i];
                        for (int j = 0; j < additionalCprtsList.Items.Count; ++j)
                        {
                            if (additionalCprtsList.Items[j].Value.ToString().Equals(code))
                            {
                                additionalCprtsList.Items[j].CheckState = CheckState.Checked;
                                totalChecked++;
                                break;
                            }
                        }
                    }
                }
                if (totalChecked == additionalCprtsList.Items.Count)
                {
                    checkEdit4.CheckState = CheckState.Checked;
                }

                ArrayList filterCompany = (ArrayList)hs["BookingCompany"];
                totalChecked = 0;
                if (filterCompany != null)
                {
                    for (int i = 0; i < filterCompany.Count; ++i)
                    {
                        string code = (string)filterCompany[i];
                        for (int j = 0; j < cmpnyList.Items.Count; ++j)
                        {
                            if (cmpnyList.Items[j].Value.ToString().Equals(code))
                            {
                                cmpnyList.Items[j].CheckState = CheckState.Checked;
                                totalChecked++;
                                break;
                            }
                        }
                    }
                }
                if (totalChecked == cmpnyList.Items.Count)
                {
                    checkEdit3.CheckState = CheckState.Checked;
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
            for (int i = 0; i < cdtyList.Items.Count; ++i)
            {
                cdtyList.Items[i].CheckState = checkEdit1.CheckState;
            }
            //     }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (ValidData())
            {
                SaveData();
                this.applyChanges = true;
            }
            else
            {
                this.DialogResult = DialogResult.None;
                this.applyChanges = false;
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

            ArrayList panels = new ArrayList();
            panels.Add(0);
            panels.Add(1);
            panels.Add(2);
            panels.Add(3);

            if ((cmboPanel1View.SelectedIndex == -1) || (cmboPanel2View.SelectedIndex == -1) || (cmboPanel3View.SelectedIndex == -1) || (cmboPanel4View.SelectedIndex == -1))
            {
                MessageBox.Show("All panels have not been assigned.  Please assigne all panel locations.", "Inbound");
                return false;
            }

            int itemIndex = -1;

            itemIndex = cmboPanel1View.SelectedIndex;
            if (panels.Contains(itemIndex))
                panels.Remove(itemIndex);
            else
            {
                MessageBox.Show("Duplicate panels have been assigned.  Please check panel locations.", "Inbound");
                return false;
            }

            itemIndex = cmboPanel2View.SelectedIndex;
            if (panels.Contains(itemIndex))
                panels.Remove(itemIndex);
            else
            {
                MessageBox.Show("Duplicate panels have been assigned.  Please check panel locations.", "Inbound");
                return false;
            }

            itemIndex = cmboPanel3View.SelectedIndex;
            if (panels.Contains(itemIndex))
                panels.Remove(itemIndex);
            else
            {
                MessageBox.Show("Duplicate panels have been assigned.  Please check panel locations.", "Inbound");
                return false;
            }

            itemIndex = cmboPanel4View.SelectedIndex;
            if (panels.Contains(itemIndex))
                panels.Remove(itemIndex);
            else
            {
                MessageBox.Show("Duplicate panels have been assigned.  Please check panel locations.", "Inbound");
                return false;
            }

            return true;
        }
        private void SaveData()
        {
            Hashtable hs = new Hashtable();
            ArrayList commoditList = new ArrayList();
            ArrayList faxNoList = new ArrayList();
            ArrayList ignoreFilterList = new ArrayList();
            ArrayList currencyList = new ArrayList();
            ArrayList sempraCmpnyList = new ArrayList();
            ArrayList addCptyList = new ArrayList();
            ArrayList panelViewsList = new ArrayList();


            for (int i = 0; i < cdtyList.Items.Count; ++i)
            {
                if (cdtyList.Items[i].CheckState == CheckState.Checked)
                {
                    commoditList.Add(cdtyList.Items[i].Value);
                }
            }

            for (int i = 0; i < chklistFaxNos.Items.Count; ++i)
            {
                if (chklistFaxNos.Items[i].CheckState == CheckState.Checked)
                {
                    faxNoList.Add(chklistFaxNos.Items[i].Value);
                }
            }

            if (chkExcludeIgnoredDocs.CheckState == CheckState.Checked)
            {
                ignoreFilterList.Add("EXCLUDED");
            }
            else
            {
                ignoreFilterList.Add("INCLUDED");
            }

            for (int i = 0; i < additionalCprtsList.Items.Count; ++i)
            {
                if (additionalCprtsList.Items[i].CheckState == CheckState.Checked)
                {
                    addCptyList.Add(additionalCprtsList.Items[i].Value);
                }
            }

            for (int i = 0; i < ccyList.Items.Count; ++i)
            {
                if (ccyList.Items[i].CheckState == CheckState.Checked)
                {
                    currencyList.Add(ccyList.Items[i].Value);
                }
            }

            for (int i = 0; i < cmpnyList.Items.Count; ++i)
            {
                if (cmpnyList.Items[i].CheckState == CheckState.Checked)
                {
                    sempraCmpnyList.Add(cmpnyList.Items[i].Value);
                }
            }
            panelViewsList.Add(cmboPanel1View.Text);
            panelViewsList.Add(cmboPanel2View.Text);
            panelViewsList.Add(cmboPanel3View.Text);
            panelViewsList.Add(cmboPanel4View.Text);

            hs["Commodity"] = commoditList;
            hs["Currency"] = currencyList;
            hs["BookingCompany"] = sempraCmpnyList;
            hs["CptyInclude"] = addCptyList;
            hs["PanelViews"] = panelViewsList;
            hs["FaxNos"] = faxNoList;
            hs["Filters"] = ignoreFilterList;


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

        private void checkEdit2_CheckStateChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < ccyList.Items.Count; ++i)
            {
                ccyList.Items[i].CheckState = checkEdit2.CheckState;
            }
        }

        private void checkEdit3_CheckStateChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < cmpnyList.Items.Count; ++i)
            {
                cmpnyList.Items[i].CheckState = checkEdit3.CheckState;
            }
        }

        private void checkEdit4_CheckStateChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < additionalCprtsList.Items.Count; ++i)
            {
                additionalCprtsList.Items[i].CheckState = checkEdit4.CheckState;
            }
        }

        private void checkEdit5_CheckStateChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < chklistFaxNos.Items.Count; ++i)
            {
                chklistFaxNos.Items[i].CheckState = checkEdit5.CheckState;
            }

        }

        private void cdtyList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}