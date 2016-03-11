using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ConfirmInbound
{
    public partial class CommentForm : Form
    {
        private string comment;
        private string label;

        public CommentForm()
        {
            InitializeComponent();
        }

        //public CommentForm(string label)
        //{
        //    InitializeComponent();
        //    // TODO: Complete member initialization
        //    this.label = label;
        //}

        public virtual string Comment
        {
            get
            {
                comment = this.memoComment.Text;
                return comment;
            }
            set
            {
                this.memoComment.Text = value;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.memoComment.Text = "";
            comment = "";
        }
    }
}