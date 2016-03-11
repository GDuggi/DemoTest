using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VaultViewer
{
    public partial class frmViewerRtf : Form
    {
        public string TickeNo { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }

        public frmViewerRtf()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
