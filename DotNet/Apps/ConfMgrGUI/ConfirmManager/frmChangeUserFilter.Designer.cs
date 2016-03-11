namespace ConfirmManager
{
   partial class frmChangeUserFilter
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChangeUserFilter));
         this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
         this.btnChangeUserFilterOk = new DevExpress.XtraEditors.SimpleButton();
         this.btnCancelCreateCancel = new DevExpress.XtraEditors.SimpleButton();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.tedFilterDescr = new DevExpress.XtraEditors.TextEdit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.panelControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tedFilterDescr.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panelControl1
         // 
         this.panelControl1.Controls.Add(this.btnChangeUserFilterOk);
         this.panelControl1.Controls.Add(this.btnCancelCreateCancel);
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.panelControl1.Location = new System.Drawing.Point(0, 48);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(350, 39);
         this.panelControl1.TabIndex = 21;
         // 
         // btnChangeUserFilterOk
         // 
         this.btnChangeUserFilterOk.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.btnChangeUserFilterOk.Location = new System.Drawing.Point(93, 8);
         this.btnChangeUserFilterOk.Name = "btnChangeUserFilterOk";
         this.btnChangeUserFilterOk.Size = new System.Drawing.Size(75, 23);
         this.btnChangeUserFilterOk.TabIndex = 1;
         this.btnChangeUserFilterOk.Text = "&OK";
         // 
         // btnCancelCreateCancel
         // 
         this.btnCancelCreateCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnCancelCreateCancel.Location = new System.Drawing.Point(182, 8);
         this.btnCancelCreateCancel.Name = "btnCancelCreateCancel";
         this.btnCancelCreateCancel.Size = new System.Drawing.Size(75, 23);
         this.btnCancelCreateCancel.TabIndex = 3;
         this.btnCancelCreateCancel.Text = "&Cancel";
         // 
         // labelControl1
         // 
         this.labelControl1.Location = new System.Drawing.Point(12, 18);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(84, 13);
         this.labelControl1.TabIndex = 22;
         this.labelControl1.Text = "Filter Description:";
         // 
         // tedFilterDescr
         // 
         this.tedFilterDescr.Location = new System.Drawing.Point(102, 14);
         this.tedFilterDescr.Name = "tedFilterDescr";
         this.tedFilterDescr.Properties.MaxLength = 100;
         this.tedFilterDescr.Size = new System.Drawing.Size(236, 20);
         this.tedFilterDescr.TabIndex = 23;
         this.tedFilterDescr.EditValueChanged += new System.EventHandler(this.tedFilterDescr_EditValueChanged);
         // 
         // frmChangeUserFilter
         // 
         this.AcceptButton = this.btnChangeUserFilterOk;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.btnCancelCreateCancel;
         this.ClientSize = new System.Drawing.Size(350, 87);
         this.ControlBox = false;
         this.Controls.Add(this.tedFilterDescr);
         this.Controls.Add(this.labelControl1);
         this.Controls.Add(this.panelControl1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MinimizeBox = false;
         this.Name = "frmChangeUserFilter";
         this.ShowInTaskbar = false;
         this.Text = "Add User Filter";
         this.Activated += new System.EventHandler(this.frmAddUserFilter_Activated);
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAddCustomFilter_FormClosing);
         this.Load += new System.EventHandler(this.frmAddCustomFilter_Load);
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.panelControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.tedFilterDescr.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraEditors.SimpleButton btnChangeUserFilterOk;
      private DevExpress.XtraEditors.SimpleButton btnCancelCreateCancel;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      public DevExpress.XtraEditors.TextEdit tedFilterDescr;
   }
}