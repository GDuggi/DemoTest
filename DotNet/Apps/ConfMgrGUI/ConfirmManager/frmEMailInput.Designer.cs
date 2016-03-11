namespace ConfirmManager
{
   partial class frmEMailInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEMailInput));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.cedSendAsRTF = new DevExpress.XtraEditors.CheckEdit();
            this.btnEMailInputCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnEMailInputOk = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.tedFromAddress = new DevExpress.XtraEditors.TextEdit();
            this.tedToAddress = new DevExpress.XtraEditors.TextEdit();
            this.tedSubject = new DevExpress.XtraEditors.TextEdit();
            this.memoBody = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cedSendAsRTF.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tedFromAddress.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tedToAddress.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tedSubject.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoBody.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.cedSendAsRTF);
            this.panelControl1.Controls.Add(this.btnEMailInputCancel);
            this.panelControl1.Controls.Add(this.btnEMailInputOk);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 224);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(359, 39);
            this.panelControl1.TabIndex = 5;
            // 
            // cedSendAsRTF
            // 
            this.cedSendAsRTF.Location = new System.Drawing.Point(14, 11);
            this.cedSendAsRTF.Name = "cedSendAsRTF";
            this.cedSendAsRTF.Properties.Caption = "Send As RTF";
            this.cedSendAsRTF.Size = new System.Drawing.Size(83, 19);
            this.cedSendAsRTF.TabIndex = 2;
            this.cedSendAsRTF.Visible = false;
            // 
            // btnEMailInputCancel
            // 
            this.btnEMailInputCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnEMailInputCancel.Location = new System.Drawing.Point(197, 7);
            this.btnEMailInputCancel.Name = "btnEMailInputCancel";
            this.btnEMailInputCancel.Size = new System.Drawing.Size(75, 25);
            this.btnEMailInputCancel.TabIndex = 1;
            this.btnEMailInputCancel.Text = "Cancel";
            this.btnEMailInputCancel.Click += new System.EventHandler(this.btnEMailInputCancel_Click);
            // 
            // btnEMailInputOk
            // 
            this.btnEMailInputOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnEMailInputOk.Location = new System.Drawing.Point(106, 7);
            this.btnEMailInputOk.Name = "btnEMailInputOk";
            this.btnEMailInputOk.Size = new System.Drawing.Size(75, 25);
            this.btnEMailInputOk.TabIndex = 0;
            this.btnEMailInputOk.Text = "Send EMail";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(11, 13);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(70, 13);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "From Address:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(11, 42);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(58, 13);
            this.labelControl2.TabIndex = 7;
            this.labelControl2.Text = "To Address:";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(11, 71);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(40, 13);
            this.labelControl3.TabIndex = 8;
            this.labelControl3.Text = "Subject:";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(11, 100);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(53, 13);
            this.labelControl4.TabIndex = 9;
            this.labelControl4.Text = "Body Text:";
            // 
            // tedFromAddress
            // 
            this.tedFromAddress.Location = new System.Drawing.Point(87, 10);
            this.tedFromAddress.Name = "tedFromAddress";
            this.tedFromAddress.Size = new System.Drawing.Size(258, 20);
            this.tedFromAddress.TabIndex = 0;
            // 
            // tedToAddress
            // 
            this.tedToAddress.Location = new System.Drawing.Point(87, 39);
            this.tedToAddress.Name = "tedToAddress";
            this.tedToAddress.Size = new System.Drawing.Size(258, 20);
            this.tedToAddress.TabIndex = 1;
            this.tedToAddress.EditValueChanged += new System.EventHandler(this.tedToAddress_EditValueChanged);
            // 
            // tedSubject
            // 
            this.tedSubject.Location = new System.Drawing.Point(87, 68);
            this.tedSubject.Name = "tedSubject";
            this.tedSubject.Size = new System.Drawing.Size(258, 20);
            this.tedSubject.TabIndex = 2;
            // 
            // memoBody
            // 
            this.memoBody.Location = new System.Drawing.Point(87, 97);
            this.memoBody.Name = "memoBody";
            this.memoBody.Size = new System.Drawing.Size(258, 119);
            this.memoBody.TabIndex = 3;
            // 
            // frmEMailInput
            // 
            this.AcceptButton = this.btnEMailInputOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnEMailInputCancel;
            this.ClientSize = new System.Drawing.Size(359, 263);
            this.Controls.Add(this.memoBody);
            this.Controls.Add(this.tedSubject);
            this.Controls.Add(this.tedToAddress);
            this.Controls.Add(this.tedFromAddress);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEMailInput";
            this.ShowInTaskbar = false;
            this.Text = "EMail Entry";
            this.Activated += new System.EventHandler(this.frmEMailInput_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEMailInput_FormClosing);
            this.Load += new System.EventHandler(this.frmEMailInput_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cedSendAsRTF.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tedFromAddress.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tedToAddress.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tedSubject.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoBody.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraEditors.SimpleButton btnEMailInputCancel;
      private DevExpress.XtraEditors.SimpleButton btnEMailInputOk;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      private DevExpress.XtraEditors.LabelControl labelControl2;
      private DevExpress.XtraEditors.LabelControl labelControl3;
      private DevExpress.XtraEditors.LabelControl labelControl4;
      public DevExpress.XtraEditors.TextEdit tedFromAddress;
      public DevExpress.XtraEditors.TextEdit tedToAddress;
      public DevExpress.XtraEditors.TextEdit tedSubject;
      public DevExpress.XtraEditors.MemoEdit memoBody;
      public DevExpress.XtraEditors.CheckEdit cedSendAsRTF;
   }
}