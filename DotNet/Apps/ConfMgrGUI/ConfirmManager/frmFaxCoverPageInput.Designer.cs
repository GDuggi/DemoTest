namespace ConfirmManager
{
   partial class frmFaxCoverPageInput
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFaxCoverPageInput));
         this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
         this.btnEMailInputCancel = new DevExpress.XtraEditors.SimpleButton();
         this.btnFaxlInputOk = new DevExpress.XtraEditors.SimpleButton();
         this.memoMessage = new DevExpress.XtraEditors.MemoEdit();
         this.tedTitle = new DevExpress.XtraEditors.TextEdit();
         this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
         this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
         this.cedSendAsRTF = new DevExpress.XtraEditors.CheckEdit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.panelControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.memoMessage.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tedTitle.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cedSendAsRTF.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panelControl1
         // 
         this.panelControl1.Controls.Add(this.cedSendAsRTF);
         this.panelControl1.Controls.Add(this.btnEMailInputCancel);
         this.panelControl1.Controls.Add(this.btnFaxlInputOk);
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.panelControl1.Location = new System.Drawing.Point(0, 195);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(443, 39);
         this.panelControl1.TabIndex = 6;
         // 
         // btnEMailInputCancel
         // 
         this.btnEMailInputCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnEMailInputCancel.Location = new System.Drawing.Point(232, 7);
         this.btnEMailInputCancel.Name = "btnEMailInputCancel";
         this.btnEMailInputCancel.Size = new System.Drawing.Size(75, 25);
         this.btnEMailInputCancel.TabIndex = 1;
         this.btnEMailInputCancel.Text = "Cancel";
         // 
         // btnFaxlInputOk
         // 
         this.btnFaxlInputOk.DialogResult = System.Windows.Forms.DialogResult.Yes;
         this.btnFaxlInputOk.Location = new System.Drawing.Point(141, 7);
         this.btnFaxlInputOk.Name = "btnFaxlInputOk";
         this.btnFaxlInputOk.Size = new System.Drawing.Size(75, 25);
         this.btnFaxlInputOk.TabIndex = 0;
         this.btnFaxlInputOk.Text = "Send";
         // 
         // memoMessage
         // 
         this.memoMessage.Location = new System.Drawing.Point(62, 41);
         this.memoMessage.Name = "memoMessage";
         this.memoMessage.Size = new System.Drawing.Size(365, 144);
         this.memoMessage.TabIndex = 11;
         this.memoMessage.EditValueChanged += new System.EventHandler(this.memoMessage_EditValueChanged);
         // 
         // tedTitle
         // 
         this.tedTitle.Location = new System.Drawing.Point(62, 12);
         this.tedTitle.Name = "tedTitle";
         this.tedTitle.Size = new System.Drawing.Size(365, 20);
         this.tedTitle.TabIndex = 10;
         // 
         // labelControl4
         // 
         this.labelControl4.Location = new System.Drawing.Point(7, 44);
         this.labelControl4.Name = "labelControl4";
         this.labelControl4.Size = new System.Drawing.Size(46, 13);
         this.labelControl4.TabIndex = 13;
         this.labelControl4.Text = "Message:";
         // 
         // labelControl3
         // 
         this.labelControl3.Location = new System.Drawing.Point(7, 15);
         this.labelControl3.Name = "labelControl3";
         this.labelControl3.Size = new System.Drawing.Size(24, 13);
         this.labelControl3.TabIndex = 12;
         this.labelControl3.Text = "Title:";
         // 
         // cedSendAsRTF
         // 
         this.cedSendAsRTF.Location = new System.Drawing.Point(50, 11);
         this.cedSendAsRTF.Name = "cedSendAsRTF";
         this.cedSendAsRTF.Properties.Caption = "Send As RTF";
         this.cedSendAsRTF.Size = new System.Drawing.Size(83, 18);
         this.cedSendAsRTF.TabIndex = 3;
         // 
         // frmFaxCoverPageInput
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(443, 234);
         this.Controls.Add(this.memoMessage);
         this.Controls.Add(this.tedTitle);
         this.Controls.Add(this.labelControl4);
         this.Controls.Add(this.labelControl3);
         this.Controls.Add(this.panelControl1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "frmFaxCoverPageInput";
         this.ShowInTaskbar = false;
         this.Text = "Fax Cover Page";
         this.Activated += new System.EventHandler(this.frmFaxCoverPageInput_Activated);
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmFaxCoverPageInput_FormClosing);
         this.Load += new System.EventHandler(this.frmFaxCoverPageInput_Load);
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.panelControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.memoMessage.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tedTitle.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cedSendAsRTF.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraEditors.SimpleButton btnEMailInputCancel;
      private DevExpress.XtraEditors.SimpleButton btnFaxlInputOk;
      public DevExpress.XtraEditors.MemoEdit memoMessage;
      public DevExpress.XtraEditors.TextEdit tedTitle;
      private DevExpress.XtraEditors.LabelControl labelControl4;
      private DevExpress.XtraEditors.LabelControl labelControl3;
      public DevExpress.XtraEditors.CheckEdit cedSendAsRTF;
   }
}