namespace ConfirmInbound
{
    partial class RefEditForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtCallerRef = new System.Windows.Forms.TextBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.ddCptyCode = new DevExpress.XtraEditors.GridLookUpEdit();
            this.gridLookUpEdit1View = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cmdSave = new DevExpress.XtraEditors.SimpleButton();
            this.cmdClose = new DevExpress.XtraEditors.SimpleButton();
            this.iNBOUNDDOCCALLERREFBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ddCptyCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iNBOUNDDOCCALLERREFBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Caller Reference:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCallerRef
            // 
            this.txtCallerRef.Enabled = false;
            this.txtCallerRef.Location = new System.Drawing.Point(154, 24);
            this.txtCallerRef.MaxLength = 40;
            this.txtCallerRef.Name = "txtCallerRef";
            this.txtCallerRef.Size = new System.Drawing.Size(165, 20);
            this.txtCallerRef.TabIndex = 1;
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.Location = new System.Drawing.Point(59, 56);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(87, 13);
            this.lblCode.TabIndex = 2;
            this.lblCode.Text = "Cpty Short Code:";
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ddCptyCode
            // 
            this.ddCptyCode.EditValue = "";
            this.ddCptyCode.Location = new System.Drawing.Point(154, 53);
            this.ddCptyCode.Name = "ddCptyCode";
            this.ddCptyCode.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ddCptyCode.Properties.NullText = "";
            this.ddCptyCode.Properties.View = this.gridLookUpEdit1View;
            this.ddCptyCode.Size = new System.Drawing.Size(165, 20);
            this.ddCptyCode.TabIndex = 3;
            // 
            // gridLookUpEdit1View
            // 
            this.gridLookUpEdit1View.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridLookUpEdit1View.Name = "gridLookUpEdit1View";
            this.gridLookUpEdit1View.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridLookUpEdit1View.OptionsView.ShowGroupPanel = false;
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(107, 93);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(79, 30);
            this.cmdSave.TabIndex = 5;
            this.cmdSave.Text = "Save";
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.Location = new System.Drawing.Point(209, 94);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(81, 29);
            this.cmdClose.TabIndex = 6;
            this.cmdClose.Text = "Cancel";
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // iNBOUNDDOCCALLERREFBindingSource
            // 
            this.iNBOUNDDOCCALLERREFBindingSource.DataMember = "INBOUND_DOC_CALLER_REF";
            // 
            // RefEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 143);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.ddCptyCode);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.txtCallerRef);
            this.Controls.Add(this.label1);
            this.Name = "RefEditForm";
            this.Text = "Inbound Documents: Caller Reference  Mapping";
            ((System.ComponentModel.ISupportInitialize)(this.ddCptyCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridLookUpEdit1View)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iNBOUNDDOCCALLERREFBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCallerRef;
        private System.Windows.Forms.Label lblCode;
        private DevExpress.XtraEditors.GridLookUpEdit ddCptyCode;
        private DevExpress.XtraGrid.Views.Grid.GridView gridLookUpEdit1View;
        private DevExpress.XtraEditors.SimpleButton cmdSave;
        private DevExpress.XtraEditors.SimpleButton cmdClose;
        private System.Windows.Forms.BindingSource iNBOUNDDOCCALLERREFBindingSource;
    }
}