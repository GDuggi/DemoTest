namespace ConfirmManager
{
   partial class frmGroupTrades
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGroupTrades));
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.teGroupXRef = new DevExpress.XtraEditors.TextEdit();
         this.btnGroupTradesOK = new DevExpress.XtraEditors.SimpleButton();
         this.btnGroupTradesCancel = new DevExpress.XtraEditors.SimpleButton();
         this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
         ((System.ComponentModel.ISupportInitialize)(this.teGroupXRef.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.panelControl1.SuspendLayout();
         this.SuspendLayout();
         // 
         // labelControl1
         // 
         this.labelControl1.Location = new System.Drawing.Point(13, 24);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(59, 13);
         this.labelControl1.TabIndex = 0;
         this.labelControl1.Text = "Group XRef:";
         // 
         // teGroupXRef
         // 
         this.teGroupXRef.Location = new System.Drawing.Point(79, 24);
         this.teGroupXRef.Name = "teGroupXRef";
         this.teGroupXRef.Size = new System.Drawing.Size(128, 20);
         this.teGroupXRef.TabIndex = 1;
         // 
         // btnGroupTradesOK
         // 
         this.btnGroupTradesOK.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.btnGroupTradesOK.Location = new System.Drawing.Point(35, 6);
         this.btnGroupTradesOK.Name = "btnGroupTradesOK";
         this.btnGroupTradesOK.Size = new System.Drawing.Size(75, 23);
         this.btnGroupTradesOK.TabIndex = 2;
         this.btnGroupTradesOK.Text = "OK";
         // 
         // btnGroupTradesCancel
         // 
         this.btnGroupTradesCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnGroupTradesCancel.Location = new System.Drawing.Point(126, 6);
         this.btnGroupTradesCancel.Name = "btnGroupTradesCancel";
         this.btnGroupTradesCancel.Size = new System.Drawing.Size(75, 23);
         this.btnGroupTradesCancel.TabIndex = 3;
         this.btnGroupTradesCancel.Text = "Cancel";
         // 
         // panelControl1
         // 
         this.panelControl1.Controls.Add(this.btnGroupTradesCancel);
         this.panelControl1.Controls.Add(this.btnGroupTradesOK);
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.panelControl1.Location = new System.Drawing.Point(0, 62);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(236, 35);
         this.panelControl1.TabIndex = 4;
         // 
         // frmGroupTrades
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(236, 97);
         this.ControlBox = false;
         this.Controls.Add(this.panelControl1);
         this.Controls.Add(this.teGroupXRef);
         this.Controls.Add(this.labelControl1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "frmGroupTrades";
         this.ShowInTaskbar = false;
         this.Text = "Group Trades";
         ((System.ComponentModel.ISupportInitialize)(this.teGroupXRef.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.panelControl1.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.LabelControl labelControl1;
      private DevExpress.XtraEditors.SimpleButton btnGroupTradesOK;
      private DevExpress.XtraEditors.SimpleButton btnGroupTradesCancel;
      public DevExpress.XtraEditors.TextEdit teGroupXRef;
      private DevExpress.XtraEditors.PanelControl panelControl1;
   }
}