namespace ConfirmInbound
{
    partial class DocumentEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DocumentEditor));
            this.mainPanel = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.txtDoc = new System.Windows.Forms.RichTextBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cmbbxTemplateName = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.cmbbxServer = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.cmbbxPort = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnFetch = new System.Windows.Forms.ToolStripButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.fontNameList = new System.Windows.Forms.ToolStripComboBox();
            this.fontSizeList = new System.Windows.Forms.ToolStripComboBox();
            this.boldButton = new System.Windows.Forms.ToolStripButton();
            this.italicButton = new System.Windows.Forms.ToolStripButton();
            this.underButton = new System.Windows.Forms.ToolStripButton();
            this.bottomPanel = new DevExpress.XtraEditors.PanelControl();
            this.cmdClose = new DevExpress.XtraEditors.SimpleButton();
            this.cmdSave = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.mainPanel)).BeginInit();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bottomPanel)).BeginInit();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.panelControl2);
            this.mainPanel.Controls.Add(this.panelControl1);
            this.mainPanel.Controls.Add(this.bottomPanel);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(793, 534);
            this.mainPanel.TabIndex = 0;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.txtDoc);
            this.panelControl2.Controls.Add(this.toolStrip2);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(2, 39);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(789, 427);
            this.panelControl2.TabIndex = 3;
            this.panelControl2.Text = "panelControl2";
            // 
            // txtDoc
            // 
            this.txtDoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDoc.Location = new System.Drawing.Point(2, 27);
            this.txtDoc.Name = "txtDoc";
            this.txtDoc.Size = new System.Drawing.Size(785, 398);
            this.txtDoc.TabIndex = 1;
            this.txtDoc.Text = "";
            this.txtDoc.Enter += new System.EventHandler(this.txtDoc_Enter);
            this.txtDoc.Protected += new System.EventHandler(this.txtDoc_Protected);
            this.txtDoc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDoc_KeyDown);
            this.txtDoc.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDoc_KeyPress);
            this.txtDoc.Click += new System.EventHandler(this.txtDoc_Click);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.cmbbxTemplateName,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.cmbbxServer,
            this.toolStripLabel3,
            this.cmbbxPort,
            this.toolStripSeparator2,
            this.btnFetch});
            this.toolStrip2.Location = new System.Drawing.Point(2, 2);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(785, 25);
            this.toolStrip2.TabIndex = 2;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(106, 22);
            this.toolStripLabel1.Text = "Document Template:";
            // 
            // cmbbxTemplateName
            // 
            this.cmbbxTemplateName.Items.AddRange(new object[] {
            "REPLY.SWAP.FO.LETTER"});
            this.cmbbxTemplateName.Name = "cmbbxTemplateName";
            this.cmbbxTemplateName.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(88, 22);
            this.toolStripLabel2.Text = "Contract Server:";
            // 
            // cmbbxServer
            // 
            this.cmbbxServer.Name = "cmbbxServer";
            this.cmbbxServer.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(31, 22);
            this.toolStripLabel3.Text = "Port:";
            // 
            // cmbbxPort
            // 
            this.cmbbxPort.Name = "cmbbxPort";
            this.cmbbxPort.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnFetch
            // 
            this.btnFetch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnFetch.Image = ((System.Drawing.Image)(resources.GetObject("btnFetch.Image")));
            this.btnFetch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFetch.Name = "btnFetch";
            this.btnFetch.Size = new System.Drawing.Size(81, 22);
            this.btnFetch.Text = "Load Template";
            this.btnFetch.Click += new System.EventHandler(this.btnFetch_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.toolStripContainer1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(2, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(789, 37);
            this.panelControl1.TabIndex = 2;
            this.panelControl1.Text = "panelControl1";
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(785, 3);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(2, 2);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(785, 33);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fontNameList,
            this.fontSizeList,
            this.boldButton,
            this.italicButton,
            this.underButton});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(334, 30);
            this.toolStrip1.TabIndex = 0;
            // 
            // fontNameList
            // 
            this.fontNameList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fontNameList.Name = "fontNameList";
            this.fontNameList.Size = new System.Drawing.Size(121, 30);
            this.fontNameList.SelectedIndexChanged += new System.EventHandler(this.fontNameList_SelectedIndexChanged);
            // 
            // fontSizeList
            // 
            this.fontSizeList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fontSizeList.Name = "fontSizeList";
            this.fontSizeList.Size = new System.Drawing.Size(121, 30);
            this.fontSizeList.SelectedIndexChanged += new System.EventHandler(this.fontSizeList_SelectedIndexChanged);
            // 
            // boldButton
            // 
            this.boldButton.CheckOnClick = true;
            this.boldButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.boldButton.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boldButton.Image = ((System.Drawing.Image)(resources.GetObject("boldButton.Image")));
            this.boldButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.boldButton.Name = "boldButton";
            this.boldButton.Size = new System.Drawing.Size(27, 27);
            this.boldButton.Text = "B";
            this.boldButton.CheckedChanged += new System.EventHandler(this.boldButton_CheckedChanged);
            this.boldButton.CheckStateChanged += new System.EventHandler(this.boldButton_CheckStateChanged);
            // 
            // italicButton
            // 
            this.italicButton.CheckOnClick = true;
            this.italicButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.italicButton.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.italicButton.Image = ((System.Drawing.Image)(resources.GetObject("italicButton.Image")));
            this.italicButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.italicButton.Name = "italicButton";
            this.italicButton.Size = new System.Drawing.Size(23, 27);
            this.italicButton.Text = "I";
            this.italicButton.CheckedChanged += new System.EventHandler(this.boldButton_CheckedChanged);
            this.italicButton.CheckStateChanged += new System.EventHandler(this.italicButton_CheckStateChanged);
            // 
            // underButton
            // 
            this.underButton.CheckOnClick = true;
            this.underButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.underButton.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.underButton.Image = ((System.Drawing.Image)(resources.GetObject("underButton.Image")));
            this.underButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.underButton.Name = "underButton";
            this.underButton.Size = new System.Drawing.Size(28, 27);
            this.underButton.Text = "&U";
            this.underButton.CheckedChanged += new System.EventHandler(this.boldButton_CheckedChanged);
            this.underButton.CheckStateChanged += new System.EventHandler(this.underButton_CheckStateChanged);
            // 
            // bottomPanel
            // 
            this.bottomPanel.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.bottomPanel.Appearance.Options.UseBackColor = true;
            this.bottomPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.bottomPanel.Controls.Add(this.cmdClose);
            this.bottomPanel.Controls.Add(this.cmdSave);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(2, 466);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(789, 66);
            this.bottomPanel.TabIndex = 1;
            // 
            // cmdClose
            // 
            this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdClose.Location = new System.Drawing.Point(387, 20);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(109, 26);
            this.cmdClose.TabIndex = 3;
            this.cmdClose.Text = "Cancel Attachment";
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // cmdSave
            // 
            this.cmdSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdSave.Location = new System.Drawing.Point(263, 20);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(109, 26);
            this.cmdSave.TabIndex = 2;
            this.cmdSave.Text = "Attach Document";
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // DocumentEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 534);
            this.Controls.Add(this.mainPanel);
            this.Name = "DocumentEditor";
            this.Text = "Document Editor";
            this.Load += new System.EventHandler(this.DocumentEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mainPanel)).EndInit();
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bottomPanel)).EndInit();
            this.bottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl mainPanel;
        private DevExpress.XtraEditors.PanelControl bottomPanel;
        private DevExpress.XtraEditors.SimpleButton cmdClose;
        private DevExpress.XtraEditors.SimpleButton cmdSave;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox fontNameList;
        private System.Windows.Forms.ToolStripComboBox fontSizeList;
        private System.Windows.Forms.ToolStripButton boldButton;
        private System.Windows.Forms.ToolStripButton italicButton;
        private System.Windows.Forms.ToolStripButton underButton;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private System.Windows.Forms.RichTextBox txtDoc;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cmbbxTemplateName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox cmbbxServer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnFetch;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripComboBox cmbbxPort;
    }
}