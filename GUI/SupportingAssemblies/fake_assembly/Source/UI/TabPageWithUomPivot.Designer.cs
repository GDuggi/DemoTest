using DevExpress.XtraEditors;
using DevExpress.XtraPivotGrid;
using DevExpress.Data.PivotGrid;
using DevExpress.XtraTab;
using System;
using System.Windows.Forms;
using System.Drawing;
using DevExpress.Utils;
using System.ComponentModel;
using System.Collections.Generic;
using com.amphora.cayenne.entity;
using com.amphora.cayenne.entity.support;
using JMAP = java.util.Map;
using DevExpress.Xpf.Core;

namespace NSRiskMgrCtrls {


    

    public partial class TabPageWithUomPivot : XtraTabPage {


        #region fields
        PanelControl pcUomAndDecimals;
        LabelControl UOMLabel;
        LabelControl DecimalsLabel;
        LabelControl asOfDateLabel;
        public LookUpEdit UOMInputControl;
        public DateEdit asOfDateEditor;
        public SpinEdit DecimalsInputControl;
        public PivotGridControl pgcPositions;
        private CheckEdit isEquivCheckbox;
        private CheckEdit ZeroCheckbox;
        private LabelControl RealPortfolioCountLabel;
        private LabelControl UpdatedPositionCountLabel;
     

        #endregion

        void InitializeComponent() {
            this.pgcPositions = new DevExpress.XtraPivotGrid.PivotGridControl();
            this.pcUomAndDecimals = new DevExpress.XtraEditors.PanelControl();
            this.isEquivCheckbox = new DevExpress.XtraEditors.CheckEdit();
            this.ZeroCheckbox = new DevExpress.XtraEditors.CheckEdit();
            this.UOMLabel = new DevExpress.XtraEditors.LabelControl();
            this.DecimalsLabel = new DevExpress.XtraEditors.LabelControl();
            this.asOfDateLabel = new LabelControl();
            this.asOfDateEditor = new DateEdit();
            this.UOMInputControl = new DevExpress.XtraEditors.LookUpEdit();
            this.DecimalsInputControl = new DevExpress.XtraEditors.SpinEdit();
            this.ExportToExcelButton = new System.Windows.Forms.Button();
            this.PrintButton = new System.Windows.Forms.Button();
            this.RealPortfolioCountLabel = new DevExpress.XtraEditors.LabelControl();
            this.UpdatedPositionCountLabel = new DevExpress.XtraEditors.LabelControl();
            this.PrintPreviewButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pgcPositions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcUomAndDecimals)).BeginInit();
            this.pcUomAndDecimals.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.isEquivCheckbox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZeroCheckbox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UOMInputControl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.asOfDateEditor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DecimalsInputControl.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pgcPositions
            // 
            this.pgcPositions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgcPositions.Location = new System.Drawing.Point(0, 0);
            this.pgcPositions.Name = "pgcPositions";
            this.pgcPositions.OptionsBehavior.UseAsyncMode = true;
            this.pgcPositions.OptionsFilterPopup.ShowOnlyAvailableItems = true;
            this.pgcPositions.OptionsFilterPopup.ShowToolbar = false;
            this.pgcPositions.OptionsLayout.Columns.AddNewColumns = false;
            this.pgcPositions.OptionsLayout.Columns.RemoveOldColumns = false;
            this.pgcPositions.OptionsLayout.Columns.StoreAllOptions = true;
            this.pgcPositions.OptionsLayout.Columns.StoreAppearance = true;
            this.pgcPositions.OptionsLayout.StoreAllOptions = true;
            this.pgcPositions.OptionsLayout.StoreAppearance = true;
            this.pgcPositions.OptionsLayout.StoreFormatRules = true;
            this.pgcPositions.OptionsLayout.StoreLayoutOptions = true;
            this.pgcPositions.OptionsView.ShowColumnTotals = false;
            this.pgcPositions.OptionsView.ShowGrandTotalsForSingleValues = true;
            this.pgcPositions.Size = new System.Drawing.Size(553, 412);
            this.pgcPositions.TabIndex = 0;
            this.pgcPositions.FieldFilterChanged += new DevExpress.XtraPivotGrid.PivotFieldEventHandler(this.pgcPositions_FieldFilterChanged);
            this.pgcPositions.FieldAreaChanged += new DevExpress.XtraPivotGrid.PivotFieldEventHandler(this.pgcPositions_fieldAreaChanged);
            this.pgcPositions.FieldValueDisplayText += new DevExpress.XtraPivotGrid.PivotFieldDisplayTextEventHandler(this.pivotGridControl1_FieldValueDisplayText);
            this.pgcPositions.CustomCellValue += new System.EventHandler<DevExpress.XtraPivotGrid.PivotCellValueEventArgs>(this.pgcPositions_CustomCellValue);
            this.pgcPositions.FocusedCellChanged += new System.EventHandler(this.pivotGridControl1_FocusedCellChanged);
            this.pgcPositions.CellSelectionChanged += new System.EventHandler(this.pivotGridControl1_CellSelectionChanged);
            this.pgcPositions.CustomDrawCell += new DevExpress.XtraPivotGrid.PivotCustomDrawCellEventHandler(this.pivotGrid_CustomDrawCell);
            this.pgcPositions.FieldValueCollapsing += new DevExpress.XtraPivotGrid.PivotFieldValueCancelEventHandler(this.pgcPositions_FieldValueCollapsing);
            this.pgcPositions.FieldValueExpanding += new DevExpress.XtraPivotGrid.PivotFieldValueCancelEventHandler(this.pgcPositions_FieldValueExpanding);
            this.pgcPositions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pgcPositions_MouseDown);
            this.pgcPositions.CustomSummary += new PivotGridCustomSummaryEventHandler(this.pgcPositions_CustomSummary);

            
            

            // 
            // pcUomAndDecimals
            // 
            this.pcUomAndDecimals.Controls.Add(this.isEquivCheckbox);
            this.pcUomAndDecimals.Controls.Add(this.ZeroCheckbox);
            this.pcUomAndDecimals.Controls.Add(this.UOMLabel);
            this.pcUomAndDecimals.Controls.Add(this.DecimalsLabel);
            this.pcUomAndDecimals.Controls.Add(this.asOfDateLabel);
            this.pcUomAndDecimals.Controls.Add(this.asOfDateEditor);
            this.pcUomAndDecimals.Controls.Add(this.UOMInputControl);
            this.pcUomAndDecimals.Controls.Add(this.DecimalsInputControl);
            this.pcUomAndDecimals.Controls.Add(this.ExportToExcelButton);
            this.pcUomAndDecimals.Controls.Add(this.PrintPreviewButton);
            this.pcUomAndDecimals.Controls.Add(this.PrintButton);
            this.pcUomAndDecimals.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pcUomAndDecimals.Location = new System.Drawing.Point(0, 412);
            this.pcUomAndDecimals.Name = "pcUomAndDecimals";
            this.pcUomAndDecimals.Size = new System.Drawing.Size(553, 25);
            this.pcUomAndDecimals.TabIndex = 1;
            // 
            // ceIsEquiv
            // 
            this.isEquivCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.isEquivCheckbox.Location = new System.Drawing.Point(430, 1);
            this.isEquivCheckbox.Name = "isEquivCheckbox";
            this.isEquivCheckbox.Properties.Caption = "Show equiv. positions";
            this.isEquivCheckbox.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.isEquivCheckbox.Size = new System.Drawing.Size(120, 19);
            this.isEquivCheckbox.TabIndex = 1;
            this.isEquivCheckbox.CheckedChanged += new System.EventHandler(this.ceIsEquiv_CheckedChanged);
            // 
            // ceIsZeroChecked
            // 
            this.ZeroCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ZeroCheckbox.Location = new System.Drawing.Point(348, 1);
            this.ZeroCheckbox.Name = "ZeroCheckbox";
            this.ZeroCheckbox.Properties.Caption = "Show Zeros";
            this.ZeroCheckbox.Properties.GlyphAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ZeroCheckbox.Size = new System.Drawing.Size(78, 19);
            this.ZeroCheckbox.TabIndex = 1;
            this.ZeroCheckbox.CheckedChanged += new System.EventHandler(this.ceIsZero_CheckedChanged);
            // 
            // label1
            // 
            this.UOMLabel.Location = new System.Drawing.Point(3, 3);
            this.UOMLabel.Name = "UOMLabel";
            this.UOMLabel.Size = new System.Drawing.Size(20, 13);
            this.UOMLabel.TabIndex = 0;
            this.UOMLabel.Text = "UOM:";
            // 
            // label2
            // 
            this.DecimalsLabel.Location = new System.Drawing.Point(130, 3);
            this.DecimalsLabel.Name = "DecimalsLabel";
            this.DecimalsLabel.Size = new System.Drawing.Size(90, 13);
            this.DecimalsLabel.TabIndex = 0;
            this.DecimalsLabel.Text = "Displayed decimals:";
            // 
            // lueUnits
            // 
            this.UOMInputControl.Location = new System.Drawing.Point(30, 1);
            this.UOMInputControl.Name = "UOMInputControl";
            this.UOMInputControl.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.UOMInputControl.Size = new System.Drawing.Size(90, 20);
            this.UOMInputControl.TabIndex = 0;
            this.UOMInputControl.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.UOMInputControl_Closed);
            // 
            // seDecimals
            // 
            this.DecimalsInputControl.EditValue = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.DecimalsInputControl.Location = new System.Drawing.Point(225, 1);
            this.DecimalsInputControl.Name = "DecimalsInputControl";
            this.DecimalsInputControl.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DecimalsInputControl.Size = new System.Drawing.Size(40, 20);
            this.DecimalsInputControl.TabIndex = 0;
            this.DecimalsInputControl.EditValueChanged += new System.EventHandler(this.seDecimals_EditValueChanged);
            this.DecimalsInputControl.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.seDecimals_EditValueChanging);

            //Label for asofDate
            this.asOfDateLabel.Location = new System.Drawing.Point(272, 3);
            this.asOfDateLabel.Name = "asOfDateLabel";
            this.asOfDateLabel.Size = new System.Drawing.Size(90, 13);
            this.asOfDateLabel.TabIndex = 0;
            this.asOfDateLabel.Text = "As Of Date:";

            this.asOfDateEditor.Location = new System.Drawing.Point(330, 1);
            this.asOfDateEditor.Name = "asOfDateEditorControl";
            this.asOfDateEditor.Size = new System.Drawing.Size(60, 20);
            this.asOfDateEditor.EditValue = "";
            this.asOfDateEditor.TabIndex = 0;
            this.asOfDateEditor.Enabled = false;
            this.asOfDateEditor.EditValueChanged += new EventHandler(asOfDateEditor_EditValueChanged);
           
            //asOfDateEditor.CausesValidation = true;
          

            //((TextBox)this.asOfDateEditor).EditCore.TextChanged += (o, args) => { BeginInvoke(new Action(() => asOfDateEditor.DoValidate())); };
            // 
            // ExportToExcelButton
            // 
            this.ExportToExcelButton.Location = new System.Drawing.Point(395, 0);
            this.ExportToExcelButton.Name = "ExportToExcelButton";
            this.ExportToExcelButton.Size = new System.Drawing.Size(90, 23);
            this.ExportToExcelButton.TabIndex = 0;
            this.ExportToExcelButton.Text = "Export To Excel";
            this.ExportToExcelButton.UseVisualStyleBackColor = true;
            this.ExportToExcelButton.Click += new System.EventHandler(this.HandleExportToExcelClick);
            // 
            // PrintButton
            // 
            this.PrintButton.Location = new System.Drawing.Point(490, 0);
            this.PrintButton.Name = "PrintButton";
            this.PrintButton.Size = new System.Drawing.Size(50, 23);
            this.PrintButton.TabIndex = 0;
            this.PrintButton.Text = "Print";
            this.PrintButton.UseVisualStyleBackColor = true;
            this.PrintButton.Click += new System.EventHandler(this.HandlePrintButtonClick);


            // 
            // PrintPreviewButton
            // 
            this.PrintPreviewButton.Location = new System.Drawing.Point(545, 0);
            this.PrintPreviewButton.Name = "PrintPreviewButton";
            this.PrintPreviewButton.Size = new System.Drawing.Size(80, 23);
            this.PrintPreviewButton.TabIndex = 0;
            this.PrintPreviewButton.Text = "Print Preview";
            this.PrintPreviewButton.UseVisualStyleBackColor = true;
            this.PrintPreviewButton.Click += new System.EventHandler(this.HandlePrintPreviewButtonClick);

            // 
            // RealPortfolioCountLabel
            // 
            this.RealPortfolioCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RealPortfolioCountLabel.Location = new System.Drawing.Point(400, 3);
            
            this.RealPortfolioCountLabel.BackColor = Color.LightSteelBlue;
            this.RealPortfolioCountLabel.ForeColor = Color.Black;

            this.RealPortfolioCountLabel.Name = "RealPortfolioCountLabel";
            this.RealPortfolioCountLabel.Size = new System.Drawing.Size(100, 13);
            this.RealPortfolioCountLabel.TabIndex = 0;
            this.RealPortfolioCountLabel.Text = "Real Portfolio Count:";
            // 
            // UpdatedPositionCountLabel
            // 
            this.UpdatedPositionCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UpdatedPositionCountLabel.Location = new System.Drawing.Point(240, 3);

            this.UpdatedPositionCountLabel.BackColor = Color.LightSteelBlue;
            this.UpdatedPositionCountLabel.ForeColor = Color.Black;

            this.UpdatedPositionCountLabel.Name = "UpdatedPositionCountLabel";
            this.UpdatedPositionCountLabel.Size = new System.Drawing.Size(117, 13);
            this.UpdatedPositionCountLabel.TabIndex = 0;
            this.UpdatedPositionCountLabel.Text = "Updated Position Count:";
        
            // 
            // TabPageWithUomPivot
            // 
            this.Controls.Add(this.RealPortfolioCountLabel);
            this.Controls.Add(this.UpdatedPositionCountLabel);
            this.Controls.Add(this.pgcPositions);
            this.Controls.Add(this.pcUomAndDecimals);

            this.Size = new System.Drawing.Size(553, 437);
            ((System.ComponentModel.ISupportInitialize)(this.pgcPositions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcUomAndDecimals)).EndInit();
            this.pcUomAndDecimals.ResumeLayout(false);
            this.pcUomAndDecimals.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.isEquivCheckbox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZeroCheckbox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UOMInputControl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DecimalsInputControl.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

      

        
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point pnt);
        void pgcPositions_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (sender is DevExpress.XtraPivotGrid.PivotGridControl && e.Button.ToString().Equals("Right"))
            {
                if (((System.Windows.Forms.Control)(this.Parent.ContextMenuStrip)).Name.Equals("contextMenuStrip1"))
                {
                    for (int i = 0; i < this.Parent.ContextMenuStrip.Items.Count; i++)
                    {
                        this.Parent.ContextMenuStrip.Items[i].Visible = false;
                    }
                }
            }
        }

        private Button PrintButton;
        private Button ExportToExcelButton;
        private Button PrintPreviewButton;
    }

    

}