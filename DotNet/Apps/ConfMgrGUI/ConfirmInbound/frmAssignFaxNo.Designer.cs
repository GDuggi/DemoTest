namespace ConfirmInbound
{
   partial class frmAssignFaxNo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAssignFaxNo));
            this.teditFaxTelexNumber = new DevExpress.XtraEditors.TextEdit();
            this.EmailAddressLabel = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnEditContractFaxCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnEditContractFaxOk = new DevExpress.XtraEditors.SimpleButton();
            this.lblFaxNumberNew = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.teditFaxTelexNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // teditFaxTelexNumber
            // 
            this.teditFaxTelexNumber.Location = new System.Drawing.Point(12, 72);
            this.teditFaxTelexNumber.Name = "teditFaxTelexNumber";
            this.teditFaxTelexNumber.Properties.MaxLength = 255;
            this.teditFaxTelexNumber.Size = new System.Drawing.Size(310, 20);
            this.teditFaxTelexNumber.TabIndex = 3;
            this.teditFaxTelexNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.teditFaxTelexNumber_KeyPress);
            // 
            // EmailAddressLabel
            // 
            this.EmailAddressLabel.Location = new System.Drawing.Point(12, 53);
            this.EmailAddressLabel.Name = "EmailAddressLabel";
            this.EmailAddressLabel.Size = new System.Drawing.Size(213, 13);
            this.EmailAddressLabel.TabIndex = 2;
            this.EmailAddressLabel.Text = "Email Address / Fax Number for this confirm:\r\n";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnEditContractFaxCancel);
            this.panelControl1.Controls.Add(this.btnEditContractFaxOk);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 104);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(337, 39);
            this.panelControl1.TabIndex = 4;
            // 
            // btnEditContractFaxCancel
            // 
            this.btnEditContractFaxCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnEditContractFaxCancel.Location = new System.Drawing.Point(169, 7);
            this.btnEditContractFaxCancel.Name = "btnEditContractFaxCancel";
            this.btnEditContractFaxCancel.Size = new System.Drawing.Size(75, 25);
            this.btnEditContractFaxCancel.TabIndex = 1;
            this.btnEditContractFaxCancel.Text = "Cancel";
            this.btnEditContractFaxCancel.Click += new System.EventHandler(this.btnEditContractFaxCancel_Click);
            // 
            // btnEditContractFaxOk
            // 
            this.btnEditContractFaxOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnEditContractFaxOk.Location = new System.Drawing.Point(78, 7);
            this.btnEditContractFaxOk.Name = "btnEditContractFaxOk";
            this.btnEditContractFaxOk.Size = new System.Drawing.Size(75, 25);
            this.btnEditContractFaxOk.TabIndex = 0;
            this.btnEditContractFaxOk.Text = "Ok";
            // 
            // lblFaxNumberNew
            // 
            this.lblFaxNumberNew.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.lblFaxNumberNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblFaxNumberNew.Location = new System.Drawing.Point(193, 18);
            this.lblFaxNumberNew.Name = "lblFaxNumberNew";
            this.lblFaxNumberNew.Size = new System.Drawing.Size(60, 13);
            this.lblFaxNumberNew.TabIndex = 2;
            this.lblFaxNumberNew.Text = "8313856683";
            this.lblFaxNumberNew.DoubleClick += new System.EventHandler(this.lblFaxNumber_DoubleClick);
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(12, 18);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(176, 13);
            this.labelControl7.TabIndex = 1;
            this.labelControl7.Text = "Default Email Address / Fax Number:";
            // 
            // frmAssignFaxNo
            // 
            this.AcceptButton = this.btnEditContractFaxOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnEditContractFaxCancel;
            this.ClientSize = new System.Drawing.Size(337, 143);
            this.Controls.Add(this.teditFaxTelexNumber);
            this.Controls.Add(this.EmailAddressLabel);
            this.Controls.Add(this.lblFaxNumberNew);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAssignFaxNo";
            this.ShowInTaskbar = false;
            this.Text = "Assign Email Address / Fax Number";
            this.Activated += new System.EventHandler(this.frmAssignFaxNo_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEditContractFax_FormClosing);
            this.Load += new System.EventHandler(this.frmEditContractFax_Load);
            ((System.ComponentModel.ISupportInitialize)(this.teditFaxTelexNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraEditors.SimpleButton btnEditContractFaxCancel;
      private DevExpress.XtraEditors.SimpleButton btnEditContractFaxOk;
      private DevExpress.XtraEditors.LabelControl EmailAddressLabel;
      private DevExpress.XtraEditors.LabelControl lblFaxNumberNew;
      private DevExpress.XtraEditors.LabelControl labelControl7;
      public DevExpress.XtraEditors.TextEdit teditFaxTelexNumber;
   }
}