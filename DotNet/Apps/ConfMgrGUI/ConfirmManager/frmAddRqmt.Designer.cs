namespace ConfirmManager
{
   partial class frmAddRqmt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddRqmt));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnAddRqmtOk = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancelCreateCancel = new DevExpress.XtraEditors.SimpleButton();
            this.comboNoConfReason = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lblNoConfReason = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.memoEditRqmtCmt = new DevExpress.XtraEditors.MemoEdit();
            this.lookupRqmt = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboNoConfReason.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditRqmtCmt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupRqmt.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnAddRqmtOk);
            this.panelControl1.Controls.Add(this.btnCancelCreateCancel);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 253);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(322, 43);
            this.panelControl1.TabIndex = 20;
            // 
            // btnAddRqmtOk
            // 
            this.btnAddRqmtOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAddRqmtOk.Location = new System.Drawing.Point(81, 10);
            this.btnAddRqmtOk.Name = "btnAddRqmtOk";
            this.btnAddRqmtOk.Size = new System.Drawing.Size(75, 23);
            this.btnAddRqmtOk.TabIndex = 1;
            this.btnAddRqmtOk.Text = "&OK";
            // 
            // btnCancelCreateCancel
            // 
            this.btnCancelCreateCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelCreateCancel.Location = new System.Drawing.Point(175, 10);
            this.btnCancelCreateCancel.Name = "btnCancelCreateCancel";
            this.btnCancelCreateCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancelCreateCancel.TabIndex = 3;
            this.btnCancelCreateCancel.Text = "&Cancel";
            // 
            // comboNoConfReason
            // 
            this.comboNoConfReason.Location = new System.Drawing.Point(240, 12);
            this.comboNoConfReason.Name = "comboNoConfReason";
            this.comboNoConfReason.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboNoConfReason.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.comboNoConfReason.Properties.ImmediatePopup = true;
            this.comboNoConfReason.Size = new System.Drawing.Size(60, 20);
            this.comboNoConfReason.TabIndex = 1;
            this.comboNoConfReason.InvalidValue += new DevExpress.XtraEditors.Controls.InvalidValueExceptionEventHandler(this.comboNoConfReason_InvalidValue);
            this.comboNoConfReason.Validating += new System.ComponentModel.CancelEventHandler(this.comboNoConfReason_Validating);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(11, 15);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(65, 13);
            this.labelControl1.TabIndex = 23;
            this.labelControl1.Text = "Requirement:";
            // 
            // lblNoConfReason
            // 
            this.lblNoConfReason.Location = new System.Drawing.Point(196, 15);
            this.lblNoConfReason.Name = "lblNoConfReason";
            this.lblNoConfReason.Size = new System.Drawing.Size(40, 13);
            this.lblNoConfReason.TabIndex = 24;
            this.lblNoConfReason.Text = "Reason:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(11, 40);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(54, 13);
            this.labelControl2.TabIndex = 25;
            this.labelControl2.Text = "Comments:";
            // 
            // memoEditRqmtCmt
            // 
            this.memoEditRqmtCmt.EditValue = "";
            this.memoEditRqmtCmt.Location = new System.Drawing.Point(81, 38);
            this.memoEditRqmtCmt.Name = "memoEditRqmtCmt";
            this.memoEditRqmtCmt.Size = new System.Drawing.Size(219, 209);
            this.memoEditRqmtCmt.TabIndex = 2;
            // 
            // lookupRqmt
            // 
            this.lookupRqmt.Location = new System.Drawing.Point(82, 12);
            this.lookupRqmt.Name = "lookupRqmt";
            this.lookupRqmt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookupRqmt.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DisplayText", "Requirement")});
            this.lookupRqmt.Properties.DisplayMember = "DisplayText";
            this.lookupRqmt.Properties.NullText = "";
            this.lookupRqmt.Properties.PopupWidth = 100;
            this.lookupRqmt.Properties.ShowHeader = false;
            this.lookupRqmt.Properties.ValueMember = "Code";
            this.lookupRqmt.Size = new System.Drawing.Size(106, 20);
            this.lookupRqmt.TabIndex = 0;
            this.lookupRqmt.EditValueChanged += new System.EventHandler(this.lookupRqmt_EditValueChanged);
            // 
            // frmAddRqmt
            // 
            this.AcceptButton = this.btnAddRqmtOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelCreateCancel;
            this.ClientSize = new System.Drawing.Size(322, 296);
            this.ControlBox = false;
            this.Controls.Add(this.lookupRqmt);
            this.Controls.Add(this.memoEditRqmtCmt);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.lblNoConfReason);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.comboNoConfReason);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddRqmt";
            this.ShowInTaskbar = false;
            this.Text = "Add Requirement";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAddRqmt_FormClosing);
            this.Load += new System.EventHandler(this.frmAddRqmt_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.comboNoConfReason.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEditRqmtCmt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupRqmt.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraEditors.SimpleButton btnAddRqmtOk;
      private DevExpress.XtraEditors.SimpleButton btnCancelCreateCancel;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      private DevExpress.XtraEditors.LabelControl labelControl2;
      public DevExpress.XtraEditors.MemoEdit memoEditRqmtCmt;
      public DevExpress.XtraEditors.ComboBoxEdit comboNoConfReason;
      public DevExpress.XtraEditors.LookUpEdit lookupRqmt;
      public DevExpress.XtraEditors.LabelControl lblNoConfReason;
   }
}