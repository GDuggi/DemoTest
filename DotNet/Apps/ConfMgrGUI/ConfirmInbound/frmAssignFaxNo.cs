using System;
using System.Windows.Forms;
using CommonUtils;
using DBAccess;
using DevExpress.XtraEditors;
using Sempra;

namespace ConfirmInbound
{
    public partial class frmAssignFaxNo : DevExpress.XtraEditors.XtraForm
    {
        private const string FORM_NAME = "frmAssignFaxNo";
        private const string FORM_ERROR_CAPTION = "Assign Fax No Form Error";
        private enum phoneNumberSegment { CountryCode, AreaCode, LocalNumber };
        private bool isFormCancelled = false;        

        public frmAssignFaxNo()
        {
            InitializeComponent();
        }

        public virtual TransmitDestination TransmitDestination
        {            
            get
            {
                return isFormCancelled ? null : new TransmitDestination(teditFaxTelexNumber.Text);
            }
        }



        private void ReadUserSettings()
        {
            try
            {
                //Now read user settings, ReadAppSettings() must be called first
                IniFile iniFile = new IniFile(FileNameUtils.GetUserIniFileName(InboundSettings.AppSettingsDir));

                this.Top = iniFile.ReadValue(FORM_NAME, "Top", 200);
                this.Left = iniFile.ReadValue(FORM_NAME, "Left", 300);
            }
            catch (Exception error)
            {
                XtraMessageBox.Show("An error occurred while reading the user settings file." + Environment.NewLine +
                       "Error CNF-425 in " + FORM_NAME + ".ReadUserSettings(): " + error.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WriteUserSettings()
        {
            try
            {
                IniFile iniFile = new IniFile(FileNameUtils.GetUserIniFileName(InboundSettings.AppSettingsDir));
                iniFile.WriteValue(FORM_NAME, "Top", this.Top);
                iniFile.WriteValue(FORM_NAME, "Left", this.Left);
            }
            catch (Exception error)
            {
                XtraMessageBox.Show("An error occurred while saving the user settings file." + Environment.NewLine +
                       "Error CNF-426 in " + FORM_NAME + ".WriteUserSettings(): " + error.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmEditContractFax_Load(object sender, EventArgs e)
        {
            this.ReadUserSettings();
            isFormCancelled = false;
        }

        private void frmEditContractFax_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!InboundSettings.IsProductionSystem && !isFormCancelled)
            {
                string[] destinations = teditFaxTelexNumber.Text.Split(';');

                foreach (string destin in destinations)
                {
                    var dest = new TransmitDestination(destin);
                    if (!dest.IsValidNonProdSendToAddress())
                    {
                        e.Cancel = true;
                        XtraMessageBox.Show("Please enter a valid Non-Production EMail Address or Fax Number.", "Non-Production Address Verification",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

            WriteUserSettings();
        }

        public void SetFaxNumbers(string AFaxTelexNo, string ALiveFaxnumber)
        {
            try
            {
                if (AFaxTelexNo.Trim().Length == 0)
                    lblFaxNumberNew.Text = "";
                else
                    lblFaxNumberNew.Text = AFaxTelexNo;

                //Israel 12/7/2015 -- Field wasn't being initialized so it was retaining previous trade's value.
                teditFaxTelexNumber.Text = String.Empty;

                if (ALiveFaxnumber.Trim().Length > 0)
                    teditFaxTelexNumber.Text = ALiveFaxnumber;
                else if (lblFaxNumberNew.Text.Trim().Length > 0)
                    teditFaxTelexNumber.Text = lblFaxNumberNew.Text;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("An error occurred while setting the transmission address for the following values:" + Environment.NewLine +
                    "New transmission address: " + AFaxTelexNo + ", Current transmission address: " + ALiveFaxnumber + Environment.NewLine + 
                       "Error CNF-427 in " + FORM_NAME + ".SetFaxNumbers(): " + ex.Message,
                     FORM_ERROR_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblFaxNumber_DoubleClick(object sender, EventArgs e)
        {
            if (!(sender as LabelControl).Text.Equals("[None]"))
                teditFaxTelexNumber.Text = (sender as LabelControl).Text;
        }

        private void teditFaxTelexNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.Handled = !Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != ' ';
            //if ( e.KeyChar not in [ '0'..'9', #8, #46 ] ) then
            if (e.KeyChar == ',' || e.KeyChar == ':' || e.KeyChar == '!')
                e.Handled = true;
        }

        private void frmAssignFaxNo_Activated(object sender, EventArgs e)
        {
            teditFaxTelexNumber.Focus();
        }

        private void btnEditContractFaxCancel_Click(object sender, EventArgs e)
        {
            isFormCancelled = true;
        }

        public void SetIsAssociatedDoc(bool isAssociatedDoc)
        {
            EmailAddressLabel.Text = "Email Address / Fax Number for this associated document:";
        }
    }
}