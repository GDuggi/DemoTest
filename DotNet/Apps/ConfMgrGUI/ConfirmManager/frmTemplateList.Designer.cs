namespace ConfirmManager
{
   partial class frmTemplateList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTemplateList));
            this.barStaticFiller = new DevExpress.XtraBars.BarStaticItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnSelect = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gridTemplateList = new DevExpress.XtraGrid.GridControl();
            this.gridViewTemplateList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.simpleButton_refresh = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTemplateList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTemplateList)).BeginInit();
            this.SuspendLayout();
            // 
            // barStaticFiller
            // 
            this.barStaticFiller.CategoryGuid = new System.Guid("96cd86ce-2c01-40e0-9b22-1b5bcafa37ca");
            this.barStaticFiller.Id = 2;
            this.barStaticFiller.Name = "barStaticFiller";
            this.barStaticFiller.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Controls.Add(this.btnSelect);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(2, 432);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(2, 2, 3, 2);
            this.panelControl1.Name = "panelControl1";
            this.tableLayoutPanel1.SetRowSpan(this.panelControl1, 2);
            this.panelControl1.Size = new System.Drawing.Size(372, 38);
            this.panelControl1.TabIndex = 5;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(228, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 28);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            // 
            // btnSelect
            // 
            this.btnSelect.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSelect.Location = new System.Drawing.Point(129, 5);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(84, 28);
            this.btnSelect.TabIndex = 0;
            this.btnSelect.Text = "&Select";
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.panelControl1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.gridTemplateList, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.simpleButton_refresh, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.12674F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85.77435F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.098911F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(377, 472);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // gridTemplateList
            // 
            this.gridTemplateList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridTemplateList.Location = new System.Drawing.Point(1, 27);
            this.gridTemplateList.MainView = this.gridViewTemplateList;
            this.gridTemplateList.Margin = new System.Windows.Forms.Padding(1, 1, 3, 1);
            this.gridTemplateList.Name = "gridTemplateList";
            this.tableLayoutPanel1.SetRowSpan(this.gridTemplateList, 2);
            this.gridTemplateList.Size = new System.Drawing.Size(373, 402);
            this.gridTemplateList.TabIndex = 2;
            this.gridTemplateList.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewTemplateList});
            // 
            // gridViewTemplateList
            // 
            this.gridViewTemplateList.GridControl = this.gridTemplateList;
            this.gridViewTemplateList.Name = "gridViewTemplateList";
            this.gridViewTemplateList.OptionsBehavior.Editable = false;
            this.gridViewTemplateList.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewTemplateList.OptionsSelection.UseIndicatorForSelection = false;
            this.gridViewTemplateList.OptionsView.EnableAppearanceEvenRow = true;
            this.gridViewTemplateList.OptionsView.EnableAppearanceOddRow = true;
            this.gridViewTemplateList.OptionsView.HeaderFilterButtonShowMode = DevExpress.XtraEditors.Controls.FilterButtonShowMode.Button;
            this.gridViewTemplateList.OptionsView.ShowAutoFilterRow = true;
            this.gridViewTemplateList.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowForFocusedRow;
            this.gridViewTemplateList.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.ShowAlways;
            this.gridViewTemplateList.DoubleClick += new System.EventHandler(this.gridViewTemplateList_DoubleClick);
            // 
            // simpleButton_refresh
            // 
            this.simpleButton_refresh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton_refresh.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton_refresh.Image")));
            this.simpleButton_refresh.Location = new System.Drawing.Point(299, 3);
            this.simpleButton_refresh.Margin = new System.Windows.Forms.Padding(0, 3, 3, 0);
            this.simpleButton_refresh.Name = "simpleButton_refresh";
            this.simpleButton_refresh.Size = new System.Drawing.Size(75, 23);
            this.simpleButton_refresh.TabIndex = 6;
            this.simpleButton_refresh.Text = "Refresh";
            this.simpleButton_refresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // frmTemplateList
            // 
            this.AcceptButton = this.btnSelect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(377, 472);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "frmTemplateList";
            this.ShowInTaskbar = false;
            this.Text = "Template List";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTemplateList_FormClosing);
            this.Load += new System.EventHandler(this.frmTemplateList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTemplateList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewTemplateList)).EndInit();
            this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraBars.BarStaticItem barStaticFiller;
      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraEditors.SimpleButton btnCancel;
      private DevExpress.XtraEditors.SimpleButton btnSelect;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private DevExpress.XtraEditors.SimpleButton simpleButton_refresh;
      public DevExpress.XtraGrid.GridControl gridTemplateList;
      private DevExpress.XtraGrid.Views.Grid.GridView gridViewTemplateList;
   }
}