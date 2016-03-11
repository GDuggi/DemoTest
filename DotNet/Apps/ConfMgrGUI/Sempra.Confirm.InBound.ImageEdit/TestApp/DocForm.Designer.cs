namespace InboundDocuments
{
    partial class DocForm
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
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.tifEditor1 = new Sempra.Confirm.InBound.ImageEdit.TifEditor();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanel1});
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl"});
            // 
            // dockPanel1
            // 
            this.dockPanel1.Controls.Add(this.dockPanel1_Container);
            this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.dockPanel1.FloatVertical = true;
            this.dockPanel1.ID = new System.Guid("cc07e860-a1fb-457c-95bc-51c3f0698a74");
            this.dockPanel1.Location = new System.Drawing.Point(0, 116);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(711, 340);
            this.dockPanel1.Text = "dockPanel1";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.tifEditor1);
            this.dockPanel1_Container.Location = new System.Drawing.Point(3, 25);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(705, 312);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // tifEditor1
            // 
            this.tifEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tifEditor1.Edit = false;
            this.tifEditor1.ExitDelegate = null;
            this.tifEditor1.ImageFileName = null;
            this.tifEditor1.Location = new System.Drawing.Point(0, 0);
            this.tifEditor1.Name = "tifEditor1";
            this.tifEditor1.SaveAsFileName = null;
            this.tifEditor1.ScaleFactor = 1;
            this.tifEditor1.Size = new System.Drawing.Size(705, 312);
            this.tifEditor1.TabIndex = 0;
            this.tifEditor1.TransDelegate = null;
            this.tifEditor1.UserName = null;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(148, 24);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(292, 37);
            this.simpleButton1.TabIndex = 1;
            this.simpleButton1.Text = "simpleButton1";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // DocForm
            // 
            this.ClientSize = new System.Drawing.Size(711, 456);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.dockPanel1);
            this.Name = "DocForm";
            this.Text = "DocForm";
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private Sempra.Confirm.InBound.ImageEdit.TifEditor tifEditor1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}