using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace NSRiskManager
{
    public partial class PortfolioIDSelector : XtraForm
    {
        public event ValidIdHandler validateId;
        public delegate bool ValidIdHandler(object sender, int portfolioID);
        private bool closeErrorExists = false;

        public static bool formCurrentlyOpen = false;

        public int PortfolioId 
        {
            get
            {
                int portfolioId = 0;
                bool isNumeric = int.TryParse(this.portfolioEditControl.Text, out portfolioId);

                if (isNumeric)
                    return portfolioId;
                else
                    return 0;
            }
        }

        public PortfolioIDSelector()
        {
            InitializeComponent();
        }

        void formLoad(object sender, EventArgs ea)
        {

            formCurrentlyOpen = true;
            this.portfolioEditControl.SelectAll();
        }


        protected override void OnClosing(CancelEventArgs e)
        {

            if (this.DialogResult != DialogResult.Cancel)
            {
                if (closeErrorExists)
                    e.Cancel = true;

            }

            base.OnClosing(e);

        }

        void CancelClickedHandler(object sender, EventArgs e)
        {
            closeErrorExists = false;
        }

        void OkClickedHandler(object sender, EventArgs e)
        {
            CancelEventArgs cancelArgs = new CancelEventArgs();

            closeErrorExists = false;

            formPortfolioId_Validating(sender, cancelArgs);

            if (cancelArgs.Cancel)
                closeErrorExists = true;

        }


        void formPortfolioId_Validating(object sender, CancelEventArgs e)
        {

            errorProvider1.SetError(this.portfolioEditControl, string.Empty);
           

            if (string.IsNullOrEmpty(this.portfolioEditControl.Text) || PortfolioId <=0)
            {
               
                errorProvider1.SetError(this.portfolioEditControl, "You must provide valid numeric id for portfolio!");
                e.Cancel = true;
               
            }
            else if (validateId != null)
            {
                if (!validateId(this, Convert.ToInt32( this.portfolioEditControl.Text)))
                {
                    errorProvider1.SetError(this.portfolioEditControl,  "You must provide valid numeric id for portfolio!");
                    e.Cancel = true;
                }
            }
        }
    }
}
