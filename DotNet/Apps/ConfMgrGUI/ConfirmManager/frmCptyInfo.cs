using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using OpsTrackingModel;
using DataManager;

namespace ConfirmManager
{
    public partial class frmCptyInfo : DevExpress.XtraEditors.XtraForm
    {
        private const string FORM_NAME = "frmCptyInfo";
        private const string FORM_ERROR_CAPTION = "Cpty Info Form Error";
        private CptyInfo cptyInfo = null;
        private DataSet cptyInfoDataSet = null;
        private DataTable tblAgreements = null;
        private DataTable tblContractFaxNos = null;


        public frmCptyInfo(CptyInfo cptyInfo)
        {
            this.cptyInfo = cptyInfo;
            InitializeComponent();
            DisplayCptyInfo();
        }

        private void DisplayCptyInfo()
        {
            try
            {
                lblDataAddress1.Text = "";
                lblDataAddress1.Text = cptyInfo.CptyAddress1;

                lblDataAddress2.Text = "";
                lblDataAddress2.Text = cptyInfo.CptyAddress2;

                lblDataAddress3.Text = "";
                lblDataAddress3.Text = cptyInfo.CptyAddress3;

                lblDataCity.Text = "";
                lblDataCity.Text = cptyInfo.CptyCity;

                lblDataState.Text = "";
                lblDataState.Text = cptyInfo.CptyState;

                lblDataZipcode.Text = "";
                lblDataZipcode.Text = cptyInfo.CptyZipcode;

                lblDataCountry.Text = "";
                lblDataCountry.Text = cptyInfo.CptyCountry;

                lblDataPhone.Text = "";
                lblDataPhone.Text = cptyInfo.CptyMainPhoneCntryCode + cptyInfo.CptyMainPhoneAreaCode + cptyInfo.CptyMainPhone;
                
                lblDataFax.Text = "";
                lblDataFax.Text = cptyInfo.CptyMainFaxCntryCode + cptyInfo.CptyMainFaxAreaCode + cptyInfo.CptyMainFax;

                lblDataCptySn.Text = "";
                lblDataCptySn.Text = cptyInfo.CptyShortName;

                lblDataCptyLn.Text = "";
                lblDataCptyLn.Text = cptyInfo.CptyLegalName;

                cptyInfoDataSet = new DataSet();
                cptyInfoDataSet.EnforceConstraints = false;


                tblAgreements = CollectionHelper.ConvertTo<CptyAgreement>(cptyInfo.CptyAgreements);
                cptyInfoDataSet.Tables.Add(tblAgreements);
                tblAgreements.PrimaryKey = new DataColumn[] { tblAgreements.Columns["AgreementId"] };

                gridAgreements.DataSource = tblAgreements;
                gridAgreements.ForceInitialize();

                tblContractFaxNos = CollectionHelper.ConvertTo<ContractFaxNo>(cptyInfo.ContractFaxNumbers);
                cptyInfoDataSet.Tables.Add(tblContractFaxNos);
                tblContractFaxNos.PrimaryKey = new DataColumn[] { tblContractFaxNos.Columns["PhoneId"] };
                
                gridContractFaxNos.DataSource = tblContractFaxNos;
                gridContractFaxNos.ForceInitialize();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while loading the data to display." + Environment.NewLine +
                        "Error CNF-220 in " + FORM_NAME + ".DisplayCptyInfo(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbCntrlMain_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            btnDetails.Enabled = tbCntrlMain.SelectedTabPageIndex == 1;
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            CptyAgreement agreement = null;
            DataRow dr = null;

            try
            {
                if (gridViewAgreements.IsValidRowHandle(gridViewAgreements.FocusedRowHandle))
                {
                    dr = gridViewAgreements.GetDataRow(gridViewAgreements.FocusedRowHandle);
                    agreement = CollectionHelper.CreateObjectFromDataRow<CptyAgreement>(dr);
                    frmAgreementDetail agreementDetails = new frmAgreementDetail(cptyInfo, agreement);
                    agreementDetails.ShowDialog();
                }
                else
                {
                    XtraMessageBox.Show("Please select a valid Agreement.");
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while getting the active document filename." + Environment.NewLine +
                        "Error CNF-221 in " + FORM_NAME + ".btnDetails_Click(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

       private void gridAgreements_DoubleClick(object sender, EventArgs e)
       {
          btnDetails.PerformClick();
       }
    }
}