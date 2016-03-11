namespace ConfirmManager
{
   partial class frmMapPhrase
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
          this.components = new System.ComponentModel.Container();
          System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMapPhrase));
          this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
          this.btnMapPhraseCancel = new DevExpress.XtraEditors.SimpleButton();
          this.btnMapPhraseSave = new DevExpress.XtraEditors.SimpleButton();
          this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
          this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
          this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
          this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
          this.tedPhrase = new DevExpress.XtraEditors.TextEdit();
          this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
          this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
          this.tedAttribCode = new DevExpress.XtraEditors.TextEdit();
          this.tedAttribDescr = new DevExpress.XtraEditors.TextEdit();
          this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
          this.lkupMappedValue = new DevExpress.XtraEditors.LookUpEdit();
          this.gridControlMappedPhrases = new DevExpress.XtraGrid.GridControl();
          this.gridViewMappedPhrases = new DevExpress.XtraGrid.Views.Grid.GridView();
          this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
          this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
          this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
          this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
          this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
          this.popupMappedPhrases = new DevExpress.XtraBars.PopupMenu(this.components);
          ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
          this.panelControl1.SuspendLayout();
          ((System.ComponentModel.ISupportInitialize)(this.tedPhrase.Properties)).BeginInit();
          ((System.ComponentModel.ISupportInitialize)(this.tedAttribCode.Properties)).BeginInit();
          ((System.ComponentModel.ISupportInitialize)(this.tedAttribDescr.Properties)).BeginInit();
          ((System.ComponentModel.ISupportInitialize)(this.lkupMappedValue.Properties)).BeginInit();
          ((System.ComponentModel.ISupportInitialize)(this.gridControlMappedPhrases)).BeginInit();
          ((System.ComponentModel.ISupportInitialize)(this.gridViewMappedPhrases)).BeginInit();
          ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
          ((System.ComponentModel.ISupportInitialize)(this.popupMappedPhrases)).BeginInit();
          this.SuspendLayout();
          // 
          // panelControl1
          // 
          this.panelControl1.Controls.Add(this.btnMapPhraseCancel);
          this.panelControl1.Controls.Add(this.btnMapPhraseSave);
          this.panelControl1.Controls.Add(this.ribbonControl1);
          this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
          this.panelControl1.Location = new System.Drawing.Point(0, 353);
          this.panelControl1.Name = "panelControl1";
          this.panelControl1.Size = new System.Drawing.Size(608, 39);
          this.panelControl1.TabIndex = 6;
          // 
          // btnMapPhraseCancel
          // 
          this.btnMapPhraseCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
          this.btnMapPhraseCancel.Location = new System.Drawing.Point(305, 9);
          this.btnMapPhraseCancel.Name = "btnMapPhraseCancel";
          this.btnMapPhraseCancel.Size = new System.Drawing.Size(75, 25);
          this.btnMapPhraseCancel.TabIndex = 1;
          this.btnMapPhraseCancel.Text = "Close";
          this.btnMapPhraseCancel.Click += new System.EventHandler(this.btnMapPhraseCancel_Click);
          // 
          // btnMapPhraseSave
          // 
          this.btnMapPhraseSave.DialogResult = System.Windows.Forms.DialogResult.OK;
          this.btnMapPhraseSave.Location = new System.Drawing.Point(214, 9);
          this.btnMapPhraseSave.Name = "btnMapPhraseSave";
          this.btnMapPhraseSave.Size = new System.Drawing.Size(75, 25);
          this.btnMapPhraseSave.TabIndex = 0;
          this.btnMapPhraseSave.Text = "Map Phrase";
          this.btnMapPhraseSave.ToolTip = "Map phrase to selected value.";
          this.btnMapPhraseSave.Click += new System.EventHandler(this.btnMapPhraseSave_Click);
          // 
          // ribbonControl1
          // 
          this.ribbonControl1.ApplicationButtonKeyTip = "";
          this.ribbonControl1.ApplicationIcon = null;
          this.ribbonControl1.Dock = System.Windows.Forms.DockStyle.None;
          this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barSubItem1});
          this.ribbonControl1.Location = new System.Drawing.Point(2, 2);
          this.ribbonControl1.MaxItemId = 1;
          this.ribbonControl1.Name = "ribbonControl1";
          this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
          this.ribbonControl1.SelectedPage = this.ribbonPage1;
          this.ribbonControl1.Size = new System.Drawing.Size(87, 142);
          this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Above;
          this.ribbonControl1.Visible = false;
          // 
          // barSubItem1
          // 
          this.barSubItem1.Caption = "Delete Mapping";
          this.barSubItem1.Id = 0;
          this.barSubItem1.Name = "barSubItem1";
          this.barSubItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barDeleteMapping_ItemClick);
          // 
          // ribbonPage1
          // 
          this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
          this.ribbonPage1.KeyTip = "";
          this.ribbonPage1.Name = "ribbonPage1";
          this.ribbonPage1.Text = "ribbonPage1";
          // 
          // ribbonPageGroup1
          // 
          this.ribbonPageGroup1.KeyTip = "";
          this.ribbonPageGroup1.Name = "ribbonPageGroup1";
          this.ribbonPageGroup1.Text = "ribbonPageGroup1";
          // 
          // tedPhrase
          // 
          this.tedPhrase.Location = new System.Drawing.Point(72, 44);
          this.tedPhrase.Name = "tedPhrase";
          this.tedPhrase.Size = new System.Drawing.Size(516, 20);
          this.tedPhrase.TabIndex = 7;
          // 
          // labelControl1
          // 
          this.labelControl1.Location = new System.Drawing.Point(12, 47);
          this.labelControl1.Name = "labelControl1";
          this.labelControl1.Size = new System.Drawing.Size(37, 13);
          this.labelControl1.TabIndex = 8;
          this.labelControl1.Text = "Phrase:";
          // 
          // labelControl2
          // 
          this.labelControl2.Location = new System.Drawing.Point(12, 17);
          this.labelControl2.Name = "labelControl2";
          this.labelControl2.Size = new System.Drawing.Size(47, 13);
          this.labelControl2.TabIndex = 9;
          this.labelControl2.Text = "Attribute:";
          // 
          // tedAttribCode
          // 
          this.tedAttribCode.EditValue = "CPTY_SN";
          this.tedAttribCode.Location = new System.Drawing.Point(72, 12);
          this.tedAttribCode.Name = "tedAttribCode";
          this.tedAttribCode.Properties.ReadOnly = true;
          this.tedAttribCode.Size = new System.Drawing.Size(56, 20);
          this.tedAttribCode.TabIndex = 10;
          // 
          // tedAttribDescr
          // 
          this.tedAttribDescr.EditValue = "Cpty Short Name";
          this.tedAttribDescr.Location = new System.Drawing.Point(134, 12);
          this.tedAttribDescr.Name = "tedAttribDescr";
          this.tedAttribDescr.Properties.ReadOnly = true;
          this.tedAttribDescr.Size = new System.Drawing.Size(155, 20);
          this.tedAttribDescr.TabIndex = 12;
          // 
          // labelControl3
          // 
          this.labelControl3.Location = new System.Drawing.Point(295, 15);
          this.labelControl3.Name = "labelControl3";
          this.labelControl3.Size = new System.Drawing.Size(71, 13);
          this.labelControl3.TabIndex = 13;
          this.labelControl3.Text = "Mapped Value:";
          // 
          // lkupMappedValue
          // 
          this.lkupMappedValue.Location = new System.Drawing.Point(372, 12);
          this.lkupMappedValue.Name = "lkupMappedValue";
          this.lkupMappedValue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
          this.lkupMappedValue.Properties.DropDownRows = 12;
          this.lkupMappedValue.Properties.NullText = "";
          this.lkupMappedValue.Properties.PopupWidth = 350;
          this.lkupMappedValue.Size = new System.Drawing.Size(216, 20);
          this.lkupMappedValue.TabIndex = 14;
          this.lkupMappedValue.TextChanged += new System.EventHandler(this.lkupMappedValue_TextChanged);
          // 
          // gridControlMappedPhrases
          // 
          this.gridControlMappedPhrases.EmbeddedNavigator.Name = "";
          this.gridControlMappedPhrases.Location = new System.Drawing.Point(12, 94);
          this.gridControlMappedPhrases.MainView = this.gridViewMappedPhrases;
          this.gridControlMappedPhrases.Name = "gridControlMappedPhrases";
          this.gridControlMappedPhrases.Size = new System.Drawing.Size(576, 240);
          this.gridControlMappedPhrases.TabIndex = 15;
          this.gridControlMappedPhrases.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewMappedPhrases,
            this.gridView2});
          // 
          // gridViewMappedPhrases
          // 
          this.gridViewMappedPhrases.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4});
          this.gridViewMappedPhrases.GridControl = this.gridControlMappedPhrases;
          this.gridViewMappedPhrases.Name = "gridViewMappedPhrases";
          this.gridViewMappedPhrases.OptionsBehavior.Editable = false;
          this.gridViewMappedPhrases.OptionsSelection.EnableAppearanceFocusedCell = false;
          this.gridViewMappedPhrases.OptionsView.ShowGroupPanel = false;
          this.gridViewMappedPhrases.ShowGridMenu += new DevExpress.XtraGrid.Views.Grid.GridMenuEventHandler(this.gridView1_ShowGridMenu);
          // 
          // gridColumn1
          // 
          this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
          this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
          this.gridColumn1.Caption = "Mapped Phrase";
          this.gridColumn1.FieldName = "Phrase";
          this.gridColumn1.Name = "gridColumn1";
          this.gridColumn1.Visible = true;
          this.gridColumn1.VisibleIndex = 0;
          // 
          // gridColumn2
          // 
          this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
          this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
          this.gridColumn2.Caption = "ID";
          this.gridColumn2.FieldName = "ID";
          this.gridColumn2.Name = "gridColumn2";
          // 
          // gridColumn3
          // 
          this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
          this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
          this.gridColumn3.Caption = "Mapped Attribute ID";
          this.gridColumn3.FieldName = "Mapped Attribute ID";
          this.gridColumn3.Name = "gridColumn3";
          // 
          // gridColumn4
          // 
          this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
          this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
          this.gridColumn4.Caption = "Active Flag";
          this.gridColumn4.FieldName = "Active Flag";
          this.gridColumn4.Name = "gridColumn4";
          this.gridColumn4.Visible = true;
          this.gridColumn4.VisibleIndex = 1;
          // 
          // gridView2
          // 
          this.gridView2.GridControl = this.gridControlMappedPhrases;
          this.gridView2.Name = "gridView2";
          // 
          // popupMappedPhrases
          // 
          this.popupMappedPhrases.ItemLinks.Add(this.barSubItem1);
          this.popupMappedPhrases.Name = "popupMappedPhrases";
          this.popupMappedPhrases.Ribbon = this.ribbonControl1;
          // 
          // frmMapPhrase
          // 
          this.AcceptButton = this.btnMapPhraseSave;
          this.AllowDrop = true;
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
          this.CancelButton = this.btnMapPhraseCancel;
          this.ClientSize = new System.Drawing.Size(608, 392);
          this.Controls.Add(this.gridControlMappedPhrases);
          this.Controls.Add(this.lkupMappedValue);
          this.Controls.Add(this.labelControl3);
          this.Controls.Add(this.tedAttribDescr);
          this.Controls.Add(this.tedAttribCode);
          this.Controls.Add(this.labelControl2);
          this.Controls.Add(this.tedPhrase);
          this.Controls.Add(this.labelControl1);
          this.Controls.Add(this.panelControl1);
          this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
          this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
          this.MaximizeBox = false;
          this.MinimizeBox = false;
          this.Name = "frmMapPhrase";
          this.ShowInTaskbar = false;
          this.Text = "Attribute Phrase Mapping";
          this.TopMost = true;
          this.Load += new System.EventHandler(this.frmMapPhrase_Load);
          this.Activated += new System.EventHandler(this.frmMapPhrase_Activated);
          this.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmMapPhrase_DragDrop);
          this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMapPhrase_FormClosing);
          this.DragOver += new System.Windows.Forms.DragEventHandler(this.frmMapPhrase_DragOver);
          ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
          this.panelControl1.ResumeLayout(false);
          ((System.ComponentModel.ISupportInitialize)(this.tedPhrase.Properties)).EndInit();
          ((System.ComponentModel.ISupportInitialize)(this.tedAttribCode.Properties)).EndInit();
          ((System.ComponentModel.ISupportInitialize)(this.tedAttribDescr.Properties)).EndInit();
          ((System.ComponentModel.ISupportInitialize)(this.lkupMappedValue.Properties)).EndInit();
          ((System.ComponentModel.ISupportInitialize)(this.gridControlMappedPhrases)).EndInit();
          ((System.ComponentModel.ISupportInitialize)(this.gridViewMappedPhrases)).EndInit();
          ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
          ((System.ComponentModel.ISupportInitialize)(this.popupMappedPhrases)).EndInit();
          this.ResumeLayout(false);
          this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.PanelControl panelControl1;
      private DevExpress.XtraEditors.SimpleButton btnMapPhraseCancel;
      private DevExpress.XtraEditors.SimpleButton btnMapPhraseSave;
      public DevExpress.XtraEditors.TextEdit tedPhrase;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      private DevExpress.XtraEditors.LabelControl labelControl2;
      public DevExpress.XtraEditors.TextEdit tedAttribCode;
      public DevExpress.XtraEditors.TextEdit tedAttribDescr;
      private DevExpress.XtraEditors.LabelControl labelControl3;
      public DevExpress.XtraEditors.LookUpEdit lkupMappedValue;
       private DevExpress.XtraGrid.GridControl gridControlMappedPhrases;
       private DevExpress.XtraGrid.Views.Grid.GridView gridViewMappedPhrases;
       private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
       private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
       private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
       private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
       private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
       private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
       private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
       private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
       private DevExpress.XtraBars.PopupMenu popupMappedPhrases;
       private DevExpress.XtraBars.BarSubItem barSubItem1;
   }
}