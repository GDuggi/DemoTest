using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApacheNMS_Test
{
    public partial class frmTestMenu : Form
    {
        public frmTestSimple testSimple = new frmTestSimple();
        public frmTestOpsMgrAPI testOpsMgrAPI = new frmTestOpsMgrAPI();

        public frmTestMenu()
        {
            InitializeComponent();
            //testSimple.InitForm();
        }

        private void btnSimple_Click(object sender, EventArgs e)
        {
            if (testSimple == null)
                testSimple = new frmTestSimple();
            testSimple.Show();
        }

        private void btnOpsMgrAPI_Click(object sender, EventArgs e)
        {
            if (testOpsMgrAPI == null)
                testOpsMgrAPI = new frmTestOpsMgrAPI();
            testOpsMgrAPI.Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            testSimple.destoryClick();
            testOpsMgrAPI.shutdownListener();
            this.Close();
        }

        private void frmTestMenu_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
