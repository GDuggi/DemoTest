
namespace InboundDocuments
{
    partial class CallerRefList
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
            this.tabPage = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.grdCpty = new DevExpress.XtraGrid.GridControl();
            this.iNBOUNDDOCCALLERREFBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cptyFax = new InboundDocuments.CptyFax();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colCALLER_REF = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCPTY_SHORT_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.grdSpam = new DevExpress.XtraGrid.GridControl();
            this.iNBOUNDDOCFAXSPAMBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.spamFax = new InboundDocuments.SpamFax();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colCALLER_REF1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDESCRIPTION = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.iNBOUND_DOC_CALLER_REFTableAdapter = new InboundDocuments.CptyFaxTableAdapters.INBOUND_DOC_CALLER_REFTableAdapter();
            this.iNBOUND_DOC_FAX_SPAMTableAdapter = new InboundDocuments.SpamFaxTableAdapters.INBOUND_DOC_FAX_SPAMTableAdapter();
            this.cmdAdd = new DevExpress.XtraEditors.SimpleButton();
            this.cmdEdit = new DevExpress.XtraEditors.SimpleButton();
            this.cmdDelete = new DevExpress.XtraEditors.SimpleButton();
            this.cmdClose = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.tabPage)).BeginInit();
            this.tabPage.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCpty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iNBOUNDDOCCALLERREFBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cptyFax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSpam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iNBOUNDDOCFAXSPAMBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spamFax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            this.SuspendLayout();
            // 
            // tabPage
            // 
            this.tabPage.Location = new System.Drawing.Point(12, 23);
            this.tabPage.Name = "tabPage";
            this.tabPage.SelectedTabPage = this.xtraTabPage1;
            this.tabPage.Size = new System.Drawing.Size(660, 333);
            this.tabPage.TabIndex = 0;
            this.tabPage.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            this.tabPage.Text = "Cpty Fax  Mapping";
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.grdCpty);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(651, 303);
            this.xtraTabPage1.Text = " Cpty Fax ";
            // 
            // grdCpty
            // 
            this.grdCpty.DataSource = this.iNBOUNDDOCCALLERREFBindingSource;
            this.grdCpty.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            this.grdCpty.EmbeddedNavigator.Name = "";
            this.grdCpty.Location = new System.Drawing.Point(0, 0);
            this.grdCpty.MainView = this.gridView1;
            this.grdCpty.Name = "grdCpty";
            this.grdCpty.Size = new System.Drawing.Size(651, 303);
            this.grdCpty.TabIndex = 0;
            this.grdCpty.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // iNBOUNDDOCCALLERREFBindingSource
            // 
            this.iNBOUNDDOCCALLERREFBindingSource.DataMember = "INBOUND_DOC_CALLER_REF";
            this.iNBOUNDDOCCALLERREFBindingSource.DataSource = this.cptyFax;
            // 
            // cptyFax
            // 
            this.cptyFax.DataSetName = "CptyFax";
            this.cptyFax.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colCALLER_REF,
            this.colCPTY_SHORT_CODE});
            this.gridView1.GridControl = this.grdCpty;
            this.gridView1.Name = "gridView1";
            // 
            // colCALLER_REF
            // 
            this.colCALLER_REF.Caption = "Fax Number";
            this.colCALLER_REF.FieldName = "CALLER_REF";
            this.colCALLER_REF.Name = "colCALLER_REF";
            this.colCALLER_REF.OptionsColumn.AllowEdit = false;
            this.colCALLER_REF.OptionsColumn.ReadOnly = true;
            this.colCALLER_REF.Visible = true;
            this.colCALLER_REF.VisibleIndex = 0;
            // 
            // colCPTY_SHORT_CODE
            // 
            this.colCPTY_SHORT_CODE.Caption = "Cpty Short Code";
            this.colCPTY_SHORT_CODE.FieldName = "CPTY_SHORT_CODE";
            this.colCPTY_SHORT_CODE.Name = "colCPTY_SHORT_CODE";
            this.colCPTY_SHORT_CODE.OptionsColumn.AllowEdit = false;
            this.colCPTY_SHORT_CODE.OptionsColumn.ReadOnly = true;
            this.colCPTY_SHORT_CODE.Visible = true;
            this.colCPTY_SHORT_CODE.VisibleIndex = 1;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.grdSpam);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(651, 303);
            this.xtraTabPage2.Text = " Junk Fax ";
            // 
            // grdSpam
            // 
            this.grdSpam.DataSource = this.iNBOUNDDOCFAXSPAMBindingSource;
            this.grdSpam.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            this.grdSpam.EmbeddedNavigator.Name = "";
            this.grdSpam.Location = new System.Drawing.Point(0, 0);
            this.grdSpam.MainView = this.gridView2;
            this.grdSpam.Name = "grdSpam";
            this.grdSpam.Size = new System.Drawing.Size(651, 303);
            this.grdSpam.TabIndex = 0;
            this.grdSpam.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2,
            this.gridView3});
            // 
            // iNBOUNDDOCFAXSPAMBindingSource
            // 
            this.iNBOUNDDOCFAXSPAMBindingSource.DataMember = "INBOUND_DOC_FAX_SPAM";
            this.iNBOUNDDOCFAXSPAMBindingSource.DataSource = this.spamFax;
            // 
            // spamFax
            // 
            this.spamFax.DataSetName = "SpamFax";
            this.spamFax.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colCALLER_REF1,
            this.colDESCRIPTION});
            this.gridView2.GridControl = this.grdSpam;
            this.gridView2.Name = "gridView2";
            // 
            // colCALLER_REF1
            // 
            this.colCALLER_REF1.Caption = "Fax Number";
            this.colCALLER_REF1.FieldName = "CALLER_REF";
            this.colCALLER_REF1.Name = "colCALLER_REF1";
            this.colCALLER_REF1.OptionsColumn.AllowEdit = false;
            this.colCALLER_REF1.OptionsColumn.ReadOnly = true;
            this.colCALLER_REF1.Visible = true;
            this.colCALLER_REF1.VisibleIndex = 0;
            // 
            // colDESCRIPTION
            // 
            this.colDESCRIPTION.Caption = "Description";
            this.colDESCRIPTION.FieldName = "DESCRIPTION";
            this.colDESCRIPTION.Name = "colDESCRIPTION";
            this.colDESCRIPTION.OptionsColumn.AllowEdit = false;
            this.colDESCRIPTION.OptionsColumn.ReadOnly = true;
            this.colDESCRIPTION.Visible = true;
            this.colDESCRIPTION.VisibleIndex = 1;
            // 
            // gridView3
            // 
            this.gridView3.GridControl = this.grdSpam;
            this.gridView3.Name = "gridView3";
            // 
            // iNBOUND_DOC_CALLER_REFTableAdapter
            // 
            this.iNBOUND_DOC_CALLER_REFTableAdapter.ClearBeforeFill = true;
            // 
            // iNBOUND_DOC_FAX_SPAMTableAdapter
            // 
            this.iNBOUND_DOC_FAX_SPAMTableAdapter.ClearBeforeFill = true;
            // 
            // cmdAdd
            // 
            this.cmdAdd.Location = new System.Drawing.Point(12, 376);
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.Size = new System.Drawing.Size(90, 30);
            this.cmdAdd.TabIndex = 1;
            this.cmdAdd.Text = "Add";
            this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
            // 
            // cmdEdit
            // 
            this.cmdEdit.Location = new System.Drawing.Point(202, 376);
            this.cmdEdit.Name = "cmdEdit";
            this.cmdEdit.Size = new System.Drawing.Size(90, 30);
            this.cmdEdit.TabIndex = 2;
            this.cmdEdit.Text = "Edit";
            this.cmdEdit.Click += new System.EventHandler(this.cmdEdit_Click);
            // 
            // cmdDelete
            // 
            this.cmdDelete.Location = new System.Drawing.Point(392, 376);
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Size = new System.Drawing.Size(90, 30);
            this.cmdDelete.TabIndex = 3;
            this.cmdDelete.Text = "Delete";
            this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.Location = new System.Drawing.Point(582, 376);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(90, 30);
            this.cmdClose.TabIndex = 4;
            this.cmdClose.Text = "Close";
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // CallerRefList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 451);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.cmdDelete);
            this.Controls.Add(this.cmdEdit);
            this.Controls.Add(this.cmdAdd);
            this.Controls.Add(this.tabPage);
            this.Name = "CallerRefList";
            this.Text = "Fax Number Mapping";
            this.Load += new System.EventHandler(this.CallerRefList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabPage)).EndInit();
            this.tabPage.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCpty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iNBOUNDDOCCALLERREFBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cptyFax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSpam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iNBOUNDDOCFAXSPAMBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spamFax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl tabPage;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private System.Data.OracleClient.OracleDataAdapter oracleAdapter;
        private DevExpress.XtraGrid.GridControl grdCpty;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private InboundDocuments.CptyFax cptyFax;
        private System.Windows.Forms.BindingSource iNBOUNDDOCCALLERREFBindingSource;
        private InboundDocuments.CptyFaxTableAdapters.INBOUND_DOC_CALLER_REFTableAdapter iNBOUND_DOC_CALLER_REFTableAdapter;
        private DevExpress.XtraGrid.Columns.GridColumn colCALLER_REF;
        private DevExpress.XtraGrid.Columns.GridColumn colCPTY_SHORT_CODE;
        private DevExpress.XtraGrid.GridControl grdSpam;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private InboundDocuments.SpamFax spamFax;
        private System.Windows.Forms.BindingSource iNBOUNDDOCFAXSPAMBindingSource;
        private InboundDocuments.SpamFaxTableAdapters.INBOUND_DOC_FAX_SPAMTableAdapter iNBOUND_DOC_FAX_SPAMTableAdapter;
        private DevExpress.XtraGrid.Columns.GridColumn colCALLER_REF1;
        private DevExpress.XtraGrid.Columns.GridColumn colDESCRIPTION;
        private DevExpress.XtraEditors.SimpleButton cmdAdd;
        private DevExpress.XtraEditors.SimpleButton cmdEdit;
        private DevExpress.XtraEditors.SimpleButton cmdDelete;
        private DevExpress.XtraEditors.SimpleButton cmdClose;

    }
}