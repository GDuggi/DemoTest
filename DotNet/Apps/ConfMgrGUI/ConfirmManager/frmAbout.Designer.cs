namespace ConfirmManager
{
   partial class frmAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
            this.btnAboutClose = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.pictureEdit2 = new DevExpress.XtraEditors.PictureEdit();
            this.treeListAbout = new DevExpress.XtraTreeList.TreeList();
            this.colProperty = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colValue = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.lblVersion = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListAbout)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAboutClose
            // 
            this.btnAboutClose.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.btnAboutClose.Appearance.Options.UseBackColor = true;
            this.btnAboutClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAboutClose.Location = new System.Drawing.Point(194, 349);
            this.btnAboutClose.Name = "btnAboutClose";
            this.btnAboutClose.Size = new System.Drawing.Size(107, 32);
            this.btnAboutClose.TabIndex = 0;
            this.btnAboutClose.Text = "Close";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Navy;
            this.labelControl1.Location = new System.Drawing.Point(84, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(273, 28);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "Confirmations Manager";
            // 
            // pictureEdit2
            // 
            this.pictureEdit2.EditValue = ((object)(resources.GetObject("pictureEdit2.EditValue")));
            this.pictureEdit2.Location = new System.Drawing.Point(363, 12);
            this.pictureEdit2.Name = "pictureEdit2";
            this.pictureEdit2.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pictureEdit2.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit2.Size = new System.Drawing.Size(48, 39);
            this.pictureEdit2.TabIndex = 8;
            // 
            // treeListAbout
            // 
            this.treeListAbout.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colProperty,
            this.colValue});
            this.treeListAbout.HorzScrollVisibility = DevExpress.XtraTreeList.ScrollVisibility.Always;
            this.treeListAbout.Location = new System.Drawing.Point(24, 73);
            this.treeListAbout.Name = "treeListAbout";
            this.treeListAbout.OptionsBehavior.Editable = false;
            this.treeListAbout.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.treeListAbout.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.treeListAbout.OptionsView.ShowIndicator = false;
            this.treeListAbout.OptionsView.ShowRoot = false;
            this.treeListAbout.Size = new System.Drawing.Size(446, 254);
            this.treeListAbout.TabIndex = 10;
            // 
            // colProperty
            // 
            this.colProperty.Caption = "Property";
            this.colProperty.FieldName = "Property";
            this.colProperty.Name = "colProperty";
            this.colProperty.OptionsColumn.AllowEdit = false;
            this.colProperty.OptionsColumn.AllowMove = false;
            this.colProperty.OptionsColumn.AllowSort = false;
            this.colProperty.OptionsColumn.FixedWidth = true;
            this.colProperty.OptionsColumn.ReadOnly = true;
            this.colProperty.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.colProperty.Visible = true;
            this.colProperty.VisibleIndex = 0;
            this.colProperty.Width = 144;
            // 
            // colValue
            // 
            this.colValue.Caption = "Value";
            this.colValue.FieldName = "Value";
            this.colValue.Name = "colValue";
            this.colValue.OptionsColumn.AllowEdit = false;
            this.colValue.OptionsColumn.AllowMove = false;
            this.colValue.OptionsColumn.AllowSort = false;
            this.colValue.OptionsColumn.FixedWidth = true;
            this.colValue.OptionsColumn.ReadOnly = true;
            this.colValue.Visible = true;
            this.colValue.VisibleIndex = 1;
            this.colValue.Width = 298;
            // 
            // lblVersion
            // 
            this.lblVersion.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblVersion.Appearance.ForeColor = System.Drawing.Color.Navy;
            this.lblVersion.Location = new System.Drawing.Point(198, 48);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(48, 16);
            this.lblVersion.TabIndex = 11;
            this.lblVersion.Text = "Version:";
            // 
            // frmAbout
            // 
            this.AcceptButton = this.btnAboutClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 404);
            this.ControlBox = false;
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.treeListAbout);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnAboutClose);
            this.Controls.Add(this.pictureEdit2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "frmAbout";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "About";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAbout_FormClosing);
            this.Load += new System.EventHandler(this.frmAbout_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.treeListAbout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.SimpleButton btnAboutClose;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      private DevExpress.XtraEditors.PictureEdit pictureEdit2;
      public DevExpress.XtraTreeList.TreeList treeListAbout;
      private DevExpress.XtraTreeList.Columns.TreeListColumn colProperty;
      private DevExpress.XtraTreeList.Columns.TreeListColumn colValue;
      public DevExpress.XtraEditors.LabelControl lblVersion;
   }
}