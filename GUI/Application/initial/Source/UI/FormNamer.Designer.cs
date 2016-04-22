namespace NSRiskManager {
    partial class FormNamer {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNamer));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.teNewName = new DevExpress.XtraEditors.TextEdit();
            this.sbOK = new DevExpress.XtraEditors.SimpleButton();
            this.sbCancel = new DevExpress.XtraEditors.SimpleButton();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.teNewName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(13, 22);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(58, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Form Name:";
            // 
            // teNewName
            // 
            this.teNewName.Location = new System.Drawing.Point(74, 19);
            this.teNewName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.teNewName.Name = "teNewName";
            this.teNewName.Size = new System.Drawing.Size(179, 20);
            this.teNewName.TabIndex = 0;
            this.teNewName.Validating += new System.ComponentModel.CancelEventHandler(this.formNameText_Validating);

            // 
            // sbOK
            // 
            this.sbOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.sbOK.Location = new System.Drawing.Point(74, 58);
            this.sbOK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.sbOK.Name = "sbOK";
            this.sbOK.Size = new System.Drawing.Size(64, 19);
            this.sbOK.TabIndex = 0;
            this.sbOK.Text = "OK";
            this.sbOK.CausesValidation = true;
            this.sbOK.Click += new System.EventHandler(this.OkClickedHandler);
            
            // 
            // sbCancel
            // 
            this.sbCancel.CausesValidation = false;
            this.sbCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.sbCancel.Location = new System.Drawing.Point(154, 58);
            this.sbCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.sbCancel.Name = "sbCancel";
            this.sbCancel.Size = new System.Drawing.Size(64, 19);
            this.sbCancel.TabIndex = 2;
            this.sbCancel.Text = "Cancel";
            this.sbCancel.Click += new System.EventHandler(this.CancelClickedHandler);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // FormNamer
            // 

            this.Text = "Save As";
            this.AcceptButton = this.sbOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.sbCancel;
            this.ClientSize = new System.Drawing.Size(268, 103);
            this.Controls.Add(this.sbCancel);
            this.Controls.Add(this.sbOK);
            this.Controls.Add(this.teNewName);
            this.Controls.Add(this.labelControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNamer";
            this.ShowInTaskbar = false;
            this.Activated += new System.EventHandler(this.FormNamer_Activated);
            this.Load += new System.EventHandler(this.FormNamer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.teNewName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit teNewName;
        private DevExpress.XtraEditors.SimpleButton sbOK;
        private DevExpress.XtraEditors.SimpleButton sbCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}