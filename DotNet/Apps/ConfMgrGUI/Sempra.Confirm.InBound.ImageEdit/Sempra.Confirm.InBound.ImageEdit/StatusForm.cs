using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sempra.Confirm.InBound.ImageEdit
{
    public partial class StatusForm : Form
    {
        private int totalPages;
        private int completedPages = 0;

        public StatusForm()
        {
            InitializeComponent();
        }

        private void StatusForm_Load(object sender, EventArgs e)
        {
            this.Location = new Point(200, 300);
        }
        public void LoadFormData(string srcFileName, string destFileName, int totalPages)
        {
            this.lblSrcFile.Text = srcFileName;
            this.lblDestFile.Text = destFileName;
            this.totalPages = totalPages;
            this.bar.Value = 0;
            UpdateProgressBar();
        }
        private void UpdateProgressBar()
        {
            var percentageComplete = (int)((double)completedPages / (double) totalPages * 100.0);
            this.bar.Value = percentageComplete;            
            this.bar.Refresh();
            if (percentageComplete >= 100)
            {
                Close();
            }
        }
        public void ShowPageInfo(int pageNum)
        {
            ++completedPages;
            lblStatus.Text = "Page Number: " + pageNum.ToString() + " completed.  Number of Pages remaining: " + (totalPages - completedPages).ToString();
            lblStatus.Refresh();
            UpdateProgressBar();

        }
    }
}