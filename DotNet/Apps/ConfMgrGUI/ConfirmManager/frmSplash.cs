using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace ConfirmManager
{
   public partial class frmSplash : DevExpress.XtraEditors.XtraForm
   {
      public frmSplash()
      {
         InitializeComponent();
      }

      public void ShowLoadProgress(string AProgressText)
      {
         lblCurrentActivity.Text = AProgressText;
         this.progressBar.PerformStep();
         this.Update();
      }

      private void pictureEdit1_EditValueChanged(object sender, EventArgs e)
      {

      }

   }
}