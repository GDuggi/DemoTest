namespace ApacheNMS_Test
{
    partial class frmTestMenu
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
            this.btnSimple = new System.Windows.Forms.Button();
            this.btnOpsMgrAPI = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSimple
            // 
            this.btnSimple.Location = new System.Drawing.Point(32, 24);
            this.btnSimple.Name = "btnSimple";
            this.btnSimple.Size = new System.Drawing.Size(147, 23);
            this.btnSimple.TabIndex = 0;
            this.btnSimple.Text = "Simple Test";
            this.btnSimple.UseVisualStyleBackColor = true;
            this.btnSimple.Click += new System.EventHandler(this.btnSimple_Click);
            // 
            // btnOpsMgrAPI
            // 
            this.btnOpsMgrAPI.Enabled = false;
            this.btnOpsMgrAPI.Location = new System.Drawing.Point(32, 65);
            this.btnOpsMgrAPI.Name = "btnOpsMgrAPI";
            this.btnOpsMgrAPI.Size = new System.Drawing.Size(147, 23);
            this.btnOpsMgrAPI.TabIndex = 0;
            this.btnOpsMgrAPI.Text = "Ops Manager API Test";
            this.btnOpsMgrAPI.UseVisualStyleBackColor = true;
            this.btnOpsMgrAPI.Click += new System.EventHandler(this.btnOpsMgrAPI_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(32, 120);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(147, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmTestMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(248, 273);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOpsMgrAPI);
            this.Controls.Add(this.btnSimple);
            this.Location = new System.Drawing.Point(400, 300);
            this.Name = "frmTestMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Apache Test Main";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTestMenu_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSimple;
        private System.Windows.Forms.Button btnOpsMgrAPI;
        private System.Windows.Forms.Button btnClose;
    }
}