namespace ConfirmManager
{
   partial class frmCancelRqmt
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCancelRqmt));
         this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
         this.btnAddRqmtOk = new DevExpress.XtraEditors.SimpleButton();
         this.btnCancelCreateCancel = new DevExpress.XtraEditors.SimpleButton();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.tedComment = new DevExpress.XtraEditors.TextEdit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.panelControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tedComment.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panelControl1
         // 
         this.panelControl1.Controls.Add(this.btnAddRqmtOk);
         this.panelControl1.Controls.Add(this.btnCancelCreateCancel);
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.panelControl1.Location = new System.Drawing.Point(0, 41);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(379, 39);
         this.panelControl1.TabIndex = 21;
         // 
         // btnAddRqmtOk
         // 
         this.btnAddRqmtOk.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.btnAddRqmtOk.Location = new System.Drawing.Point(109, 8);
         this.btnAddRqmtOk.Name = "btnAddRqmtOk";
         this.btnAddRqmtOk.Size = new System.Drawing.Size(75, 23);
         this.btnAddRqmtOk.TabIndex = 1;
         this.btnAddRqmtOk.Text = "&OK";
         // 
         // btnCancelCreateCancel
         // 
         this.btnCancelCreateCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnCancelCreateCancel.Location = new System.Drawing.Point(194, 8);
         this.btnCancelCreateCancel.Name = "btnCancelCreateCancel";
         this.btnCancelCreateCancel.Size = new System.Drawing.Size(75, 23);
         this.btnCancelCreateCancel.TabIndex = 3;
         this.btnCancelCreateCancel.Text = "&Cancel";
         // 
         // labelControl1
         // 
         this.labelControl1.Location = new System.Drawing.Point(9, 13);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(49, 13);
         this.labelControl1.TabIndex = 22;
         this.labelControl1.Text = "Comment:";
         // 
         // tedComment
         // 
         this.tedComment.Location = new System.Drawing.Point(65, 11);
         this.tedComment.Name = "tedComment";
         this.tedComment.Properties.MaxLength = 500;
         this.tedComment.Size = new System.Drawing.Size(305, 20);
         this.tedComment.TabIndex = 23;
         // 
         // frmCancelRqmt
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(379, 80);
         this.ControlBox = false;
         this.Controls.Add(this.tedComment);
         this.Controls.Add(this.labelControl1);
         this.Controls.Add(this.panelControl1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MaximizeBox = false;
         this.Name = "frmCancelRqmt";
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
         this.Text = "Cancel Rqmt";
         this.Activated += new System.EventHandler(this.frmCancelRqmt_Activated);
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCancelRqmt_FormClosing);
         this.Load += new System.EventHandler(this.frmCancelRqmt_Load);
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.panelControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.tedComment.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraEditors.SimpleButton btnAddRqmtOk;
      private DevExpress.XtraEditors.SimpleButton btnCancelCreateCancel;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      public DevExpress.XtraEditors.TextEdit tedComment;
   }
}