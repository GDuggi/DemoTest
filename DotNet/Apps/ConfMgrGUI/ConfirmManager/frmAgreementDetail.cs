using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpsTrackingModel;

namespace ConfirmManager
{
    public partial class frmAgreementDetail : DevExpress.XtraEditors.XtraForm
    {
        private CptyInfo cptyInfo = null;
        private CptyAgreement details = null; 
        public frmAgreementDetail(CptyInfo cptyInfo, CptyAgreement details)
        {
            InitializeComponent();
            this.cptyInfo = cptyInfo;
            this.details = details;

            DisplayAgreementDetail();
        }

        private void DisplayAgreementDetail()
        {
            try
            {
                lblDataCptyLn.Text = "";
                lblDataCptyLn.Text = cptyInfo.CptyLegalName;

                lblDataCptySn.Text = "";
                lblDataCptySn.Text = cptyInfo.CptyShortName;

                lblDataType.Text = "";
                lblDataType.Text = details.AgrmntTypeCode;

                lblDataStatus.Text = "";
                lblDataStatus.Text = details.StatusInd;

                lblDataRbsEntity.Text = "";
                lblDataRbsEntity.Text = details.SeCptyShortName;

                lblDataDtExecuted.Text = "";
                lblDataDtExecuted.Text = details.DateSigned;

                lblDataDtTerminated.Text = "";
                lblDataDtTerminated.Text = details.TerminationDt;

                lblDataRbsContact.Text = "";
                lblDataRbsContact.Text = details.SeAgrmntContactName;

                memoCmts.Text = "";
                memoCmts.Text = details.Cmt;

            }
            catch (Exception ex)
            {

            }
        }
    }
}