using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestWorkbench
{
    public partial class frmViewerTif : Form
    {
        public Int32 TradeId { get; set; }
        public string FileName { get; set; }
        
        public frmViewerTif()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
