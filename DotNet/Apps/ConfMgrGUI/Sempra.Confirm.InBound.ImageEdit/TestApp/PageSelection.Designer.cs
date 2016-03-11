using System.Windows.Forms;
using Leadtools.WinForms;

namespace InboundDocuments
{
    partial class PageSelection
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
            this.mainPanel = new DevExpress.XtraEditors.PanelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.imageList = new Leadtools.WinForms.RasterImageList();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.statusPanel = new DevExpress.XtraEditors.PanelControl();
            this.cancelButton = new DevExpress.XtraEditors.SimpleButton();
            this.okButton = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.flowPanel = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.mainPanel)).BeginInit();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusPanel)).BeginInit();
            this.statusPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.panelControl1);
            this.mainPanel.Controls.Add(this.toolStrip1);
            this.mainPanel.Controls.Add(this.statusPanel);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(856, 467);
            this.mainPanel.TabIndex = 0;
            this.mainPanel.Text = "mainPanel";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.imageList);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(2, 27);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(852, 347);
            this.panelControl1.TabIndex = 3;
            this.panelControl1.Text = "panelControl1";
            // 
            // imageList
            // 
            this.imageList.AutoDeselectItems = true;
            this.imageList.AutoDisposeImages = true;
            this.imageList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.imageList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageList.DoubleBuffer = true;
            this.imageList.EnableKeyboard = true;
            this.imageList.EnableRubberBandSelection = true;
            this.imageList.ItemBackColor = System.Drawing.SystemColors.Control;
            this.imageList.ItemBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageList.ItemForeColor = System.Drawing.SystemColors.ControlText;
            this.imageList.ItemImageBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageList.ItemImageSize = new System.Drawing.Size(200, 200);
            this.imageList.ItemSelectedBackColor = System.Drawing.SystemColors.Highlight;
            this.imageList.ItemSelectedForeColor = System.Drawing.SystemColors.HighlightText;
            this.imageList.ItemSize = new System.Drawing.Size(200, 225);
            this.imageList.ItemSpacingSize = new System.Drawing.Size(2, 2);
            this.imageList.Location = new System.Drawing.Point(2, 2);
            this.imageList.Name = "imageList";
            this.imageList.ScrollStyle = Leadtools.WinForms.RasterImageListScrollStyle.Vertical;
            this.imageList.SelectionMode = Leadtools.WinForms.RasterImageListSelectionMode.None;
            this.imageList.SelectUserImage = null;
            this.imageList.ShowItemText = true;
            this.imageList.Size = new System.Drawing.Size(848, 343);
            this.imageList.Sorting = System.Windows.Forms.SortOrder.None;
            this.imageList.TabIndex = 1;
            this.imageList.Text = "rasterImageList1";
            this.imageList.TopIndex = 0;
            this.imageList.ViewStyle = Leadtools.WinForms.RasterImageListViewStyle.Normal;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.toolStripComboBox1});
            this.toolStrip1.Location = new System.Drawing.Point(2, 2);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(852, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBox1.DropDownWidth = 100;
            this.toolStripComboBox1.Items.AddRange(new object[] {
            "30%",
            "40%",
            "50%",
            "60%",
            "70%",
            "80%",
            "90%",
            "100%"});
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(100, 25);
            this.toolStripComboBox1.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox1_SelectedIndexChanged);
            this.toolStripComboBox1.Click += new System.EventHandler(this.toolStripComboBox1_Click);
            // 
            // statusPanel
            // 
            this.statusPanel.Controls.Add(this.cancelButton);
            this.statusPanel.Controls.Add(this.okButton);
            this.statusPanel.Controls.Add(this.groupControl1);
            this.statusPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusPanel.Location = new System.Drawing.Point(2, 374);
            this.statusPanel.Name = "statusPanel";
            this.statusPanel.Size = new System.Drawing.Size(852, 91);
            this.statusPanel.TabIndex = 1;
            this.statusPanel.Text = "panelControl1";
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(705, 52);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(125, 29);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(705, 7);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(125, 29);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.flowPanel);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.Location = new System.Drawing.Point(2, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(689, 87);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Select Pages";
            // 
            // flowPanel
            // 
            this.flowPanel.AutoScroll = true;
            this.flowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPanel.Location = new System.Drawing.Point(2, 20);
            this.flowPanel.Name = "flowPanel";
            this.flowPanel.Size = new System.Drawing.Size(685, 65);
            this.flowPanel.TabIndex = 0;
            // 
            // PageSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 467);
            this.Controls.Add(this.mainPanel);
            this.Name = "PageSelection";
            this.Text = "Page Selection Dialog";
            ((System.ComponentModel.ISupportInitialize)(this.mainPanel)).EndInit();
            this.mainPanel.ResumeLayout(false);
            this.mainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusPanel)).EndInit();
            this.statusPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl mainPanel;
        private RasterImageViewer imageViewer;
        private DevExpress.XtraEditors.PanelControl statusPanel;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private RasterImageList imageList;
        private ToolStrip toolStrip1;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripComboBox toolStripComboBox1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton cancelButton;
        private DevExpress.XtraEditors.SimpleButton okButton;
        private FlowLayoutPanel flowPanel;


    }
}