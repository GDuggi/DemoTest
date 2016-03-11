namespace ConfirmManager
{
   partial class frmClauseViewer
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmClauseViewer));
         this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
         this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
         this.splitContainerClauses = new DevExpress.XtraEditors.SplitContainerControl();
         this.treeListClauses = new DevExpress.XtraTreeList.TreeList();
         this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
         this.richTextClauses = new System.Windows.Forms.RichTextBox();
         this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
         this.btnCopy = new DevExpress.XtraEditors.SimpleButton();
         this.btnCopyAll = new DevExpress.XtraEditors.SimpleButton();
         this.btnClose = new DevExpress.XtraEditors.SimpleButton();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerClauses)).BeginInit();
         this.splitContainerClauses.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.treeListClauses)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
         this.panelControl1.SuspendLayout();
         this.SuspendLayout();
         // 
         // ribbonControl1
         // 
         this.ribbonControl1.ApplicationButtonKeyTip = "";
         this.ribbonControl1.ApplicationIcon = null;
         this.ribbonControl1.Dock = System.Windows.Forms.DockStyle.None;
         this.ribbonControl1.Location = new System.Drawing.Point(490, 49);
         this.ribbonControl1.Name = "ribbonControl1";
         this.ribbonControl1.Size = new System.Drawing.Size(12, 21);
         this.ribbonControl1.StatusBar = this.ribbonStatusBar;
         this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Above;
         // 
         // ribbonStatusBar
         // 
         this.ribbonStatusBar.Location = new System.Drawing.Point(0, 411);
         this.ribbonStatusBar.Name = "ribbonStatusBar";
         this.ribbonStatusBar.Ribbon = this.ribbonControl1;
         this.ribbonStatusBar.Size = new System.Drawing.Size(502, 24);
         // 
         // splitContainerClauses
         // 
         this.splitContainerClauses.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainerClauses.Location = new System.Drawing.Point(0, 38);
         this.splitContainerClauses.Name = "splitContainerClauses";
         this.splitContainerClauses.Panel1.Controls.Add(this.treeListClauses);
         this.splitContainerClauses.Panel1.Text = "Treeview";
         this.splitContainerClauses.Panel2.Controls.Add(this.richTextClauses);
         this.splitContainerClauses.Panel2.Text = "Editor";
         this.splitContainerClauses.Size = new System.Drawing.Size(502, 373);
         this.splitContainerClauses.SplitterPosition = 243;
         this.splitContainerClauses.TabIndex = 2;
         // 
         // treeListClauses
         // 
         this.treeListClauses.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1});
         this.treeListClauses.Dock = System.Windows.Forms.DockStyle.Fill;
         this.treeListClauses.KeyFieldName = "";
         this.treeListClauses.Location = new System.Drawing.Point(0, 0);
         this.treeListClauses.Name = "treeListClauses";
         this.treeListClauses.OptionsBehavior.Editable = false;
         this.treeListClauses.OptionsBehavior.ImmediateEditor = false;
         this.treeListClauses.OptionsView.EnableAppearanceEvenRow = true;
         this.treeListClauses.OptionsView.EnableAppearanceOddRow = true;
         this.treeListClauses.ParentFieldName = "";
         this.treeListClauses.Size = new System.Drawing.Size(239, 369);
         this.treeListClauses.TabIndex = 0;
         this.treeListClauses.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeListClauses_FocusedNodeChanged);
         // 
         // treeListColumn1
         // 
         this.treeListColumn1.Caption = "Clauses";
         this.treeListColumn1.FieldName = "treeListColumn1";
         this.treeListColumn1.Name = "treeListColumn1";
         this.treeListColumn1.Visible = true;
         this.treeListColumn1.VisibleIndex = 0;
         // 
         // richTextClauses
         // 
         this.richTextClauses.AcceptsTab = true;
         this.richTextClauses.BackColor = System.Drawing.SystemColors.Info;
         this.richTextClauses.Dock = System.Windows.Forms.DockStyle.Fill;
         this.richTextClauses.ForeColor = System.Drawing.SystemColors.WindowText;
         this.richTextClauses.Location = new System.Drawing.Point(0, 0);
         this.richTextClauses.Name = "richTextClauses";
         this.richTextClauses.ReadOnly = true;
         this.richTextClauses.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
         this.richTextClauses.Size = new System.Drawing.Size(247, 369);
         this.richTextClauses.TabIndex = 0;
         this.richTextClauses.Text = "";
         // 
         // panelControl1
         // 
         this.panelControl1.Controls.Add(this.btnCopy);
         this.panelControl1.Controls.Add(this.btnCopyAll);
         this.panelControl1.Controls.Add(this.btnClose);
         this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panelControl1.Location = new System.Drawing.Point(0, 0);
         this.panelControl1.Name = "panelControl1";
         this.panelControl1.Size = new System.Drawing.Size(502, 38);
         this.panelControl1.TabIndex = 3;
         // 
         // btnCopy
         // 
         this.btnCopy.Location = new System.Drawing.Point(166, 7);
         this.btnCopy.Name = "btnCopy";
         this.btnCopy.Size = new System.Drawing.Size(70, 23);
         this.btnCopy.TabIndex = 2;
         this.btnCopy.Text = "Copy";
         this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
         // 
         // btnCopyAll
         // 
         this.btnCopyAll.Location = new System.Drawing.Point(242, 7);
         this.btnCopyAll.Name = "btnCopyAll";
         this.btnCopyAll.Size = new System.Drawing.Size(70, 23);
         this.btnCopyAll.TabIndex = 1;
         this.btnCopyAll.Text = "Copy All";
         this.btnCopyAll.Click += new System.EventHandler(this.btnCopyAll_Click);
         // 
         // btnClose
         // 
         this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.btnClose.Location = new System.Drawing.Point(15, 7);
         this.btnClose.Name = "btnClose";
         this.btnClose.Size = new System.Drawing.Size(70, 23);
         this.btnClose.TabIndex = 0;
         this.btnClose.Text = "Close";
         this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
         // 
         // frmClauseViewer
         // 
         this.AcceptButton = this.btnClose;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(502, 435);
         this.ControlBox = false;
         this.Controls.Add(this.splitContainerClauses);
         this.Controls.Add(this.ribbonStatusBar);
         this.Controls.Add(this.ribbonControl1);
         this.Controls.Add(this.panelControl1);
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MinimizeBox = false;
         this.Name = "frmClauseViewer";
         this.ShowInTaskbar = false;
         this.Text = "Clause Viewer";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmClauseViewer_FormClosing);
         this.Load += new System.EventHandler(this.frmClauseViewer_Load);
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerClauses)).EndInit();
         this.splitContainerClauses.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.treeListClauses)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
         this.panelControl1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
      private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
      private DevExpress.XtraEditors.SplitContainerControl splitContainerClauses;
      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraEditors.SimpleButton btnClose;
      private DevExpress.XtraEditors.SimpleButton btnCopyAll;
      private DevExpress.XtraTreeList.TreeList treeListClauses;
      private System.Windows.Forms.RichTextBox richTextClauses;
      private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
      private DevExpress.XtraEditors.SimpleButton btnCopy;

   }
}