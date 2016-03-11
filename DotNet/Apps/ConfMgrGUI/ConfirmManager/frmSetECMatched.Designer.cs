namespace ConfirmManager
{
   partial class frmSetECMatched
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSetECMatched));
         this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
         this.btnUpdMatchedECOk = new DevExpress.XtraEditors.SimpleButton();
         this.btnUpdMatchedECCancel = new DevExpress.XtraEditors.SimpleButton();
         this.tedCptyRefId = new DevExpress.XtraEditors.TextEdit();
         this.dedUpdStatusDate = new DevExpress.XtraEditors.DateEdit();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.panelControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tedCptyRefId.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dedUpdStatusDate.Properties.VistaTimeProperties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dedUpdStatusDate.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panelControl1
         // 
         this.panelControl1.Controls.Add(this.btnUpdMatchedECOk);
         this.panelControl1.Controls.Add(this.btnUpdMatchedECCancel);
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.panelControl1.Location = new System.Drawing.Point(0, 74);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(292, 43);
         this.panelControl1.TabIndex = 21;
         // 
         // btnUpdMatchedECOk
         // 
         this.btnUpdMatchedECOk.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.btnUpdMatchedECOk.Location = new System.Drawing.Point(62, 10);
         this.btnUpdMatchedECOk.Name = "btnUpdMatchedECOk";
         this.btnUpdMatchedECOk.Size = new System.Drawing.Size(75, 23);
         this.btnUpdMatchedECOk.TabIndex = 0;
         this.btnUpdMatchedECOk.Text = "&OK";
         // 
         // btnUpdMatchedECCancel
         // 
         this.btnUpdMatchedECCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnUpdMatchedECCancel.Location = new System.Drawing.Point(156, 10);
         this.btnUpdMatchedECCancel.Name = "btnUpdMatchedECCancel";
         this.btnUpdMatchedECCancel.Size = new System.Drawing.Size(75, 23);
         this.btnUpdMatchedECCancel.TabIndex = 1;
         this.btnUpdMatchedECCancel.Text = "&Cancel";
         // 
         // tedCptyRefId
         // 
         this.tedCptyRefId.Location = new System.Drawing.Point(114, 38);
         this.tedCptyRefId.Name = "tedCptyRefId";
         this.tedCptyRefId.Properties.MaxLength = 30;
         this.tedCptyRefId.Size = new System.Drawing.Size(166, 20);
         this.tedCptyRefId.TabIndex = 1;
         this.tedCptyRefId.EditValueChanged += new System.EventHandler(this.DataEntry_EditValueChanged);
         // 
         // dedUpdStatusDate
         // 
         this.dedUpdStatusDate.EditValue = "";
         this.dedUpdStatusDate.Location = new System.Drawing.Point(114, 12);
         this.dedUpdStatusDate.Name = "dedUpdStatusDate";
         this.dedUpdStatusDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.dedUpdStatusDate.Properties.DisplayFormat.FormatString = "dd-MMM-yyyy";
         this.dedUpdStatusDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
         this.dedUpdStatusDate.Properties.EditFormat.FormatString = "dd-MMM-yyyy";
         this.dedUpdStatusDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Custom;
         this.dedUpdStatusDate.Properties.Mask.EditMask = "dd-MMM-yyyy";
         this.dedUpdStatusDate.Properties.NullDate = "";
         this.dedUpdStatusDate.Properties.VistaTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.dedUpdStatusDate.Properties.CustomDisplayText += new DevExpress.XtraEditors.Controls.CustomDisplayTextEventHandler(this.dateEdit1_Properties_CustomDisplayText);
         this.dedUpdStatusDate.Size = new System.Drawing.Size(92, 20);
         this.dedUpdStatusDate.TabIndex = 0;
         this.dedUpdStatusDate.EditValueChanged += new System.EventHandler(this.DataEntry_EditValueChanged);
         this.dedUpdStatusDate.Enter += new System.EventHandler(this.dedUpdStatusDate_Enter);
         // 
         // labelControl1
         // 
         this.labelControl1.Location = new System.Drawing.Point(9, 42);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(60, 13);
         this.labelControl1.TabIndex = 24;
         this.labelControl1.Text = "Cpty Ref Id:";
         // 
         // labelControl2
         // 
         this.labelControl2.Location = new System.Drawing.Point(9, 16);
         this.labelControl2.Name = "labelControl2";
         this.labelControl2.Size = new System.Drawing.Size(99, 13);
         this.labelControl2.TabIndex = 25;
         this.labelControl2.Text = "Update Status Date:";
         // 
         // frmUpdECMatched
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.btnUpdMatchedECCancel;
         this.ClientSize = new System.Drawing.Size(292, 117);
         this.ControlBox = false;
         this.Controls.Add(this.labelControl2);
         this.Controls.Add(this.labelControl1);
         this.Controls.Add(this.dedUpdStatusDate);
         this.Controls.Add(this.tedCptyRefId);
         this.Controls.Add(this.panelControl1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Name = "frmUpdECMatched";
         this.ShowInTaskbar = false;
         this.Text = "Update EConfirm Matched";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUpdMatchedEC_FormClosing);
         this.Load += new System.EventHandler(this.frmUpdMatchedEC_Load);
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.panelControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.tedCptyRefId.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dedUpdStatusDate.Properties.VistaTimeProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dedUpdStatusDate.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraEditors.SimpleButton btnUpdMatchedECOk;
      private DevExpress.XtraEditors.SimpleButton btnUpdMatchedECCancel;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      private DevExpress.XtraEditors.LabelControl labelControl2;
      public DevExpress.XtraEditors.TextEdit tedCptyRefId;
      public DevExpress.XtraEditors.DateEdit dedUpdStatusDate;
   }
}