namespace InboundDocuments
{
    partial class UserFilterForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.cdtyList = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.legalName = new System.Windows.Forms.RadioButton();
            this.shortName = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEnd = new System.Windows.Forms.TextBox();
            this.cboEnd = new System.Windows.Forms.ComboBox();
            this.txtStart = new System.Windows.Forms.TextBox();
            this.cboStart = new System.Windows.Forms.ComboBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cdtyList)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Show Confirmations where:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkEdit1);
            this.groupBox1.Controls.Add(this.cdtyList);
            this.groupBox1.Location = new System.Drawing.Point(35, 35);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(267, 177);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Commodity ";
            // 
            // checkEdit1
            // 
            this.checkEdit1.Location = new System.Drawing.Point(6, 143);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "Select All";
            this.checkEdit1.Size = new System.Drawing.Size(108, 19);
            this.checkEdit1.TabIndex = 1;
            this.checkEdit1.CheckStateChanged += new System.EventHandler(this.checkEdit1_CheckStateChanged);
            // 
            // cdtyList
            // 
            this.cdtyList.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.cdtyList.Location = new System.Drawing.Point(6, 19);
            this.cdtyList.Name = "cdtyList";
            this.cdtyList.Size = new System.Drawing.Size(242, 118);
            this.cdtyList.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtEnd);
            this.groupBox2.Controls.Add(this.cboEnd);
            this.groupBox2.Controls.Add(this.txtStart);
            this.groupBox2.Controls.Add(this.cboStart);
            this.groupBox2.Location = new System.Drawing.Point(319, 35);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(339, 177);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Counterparty";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.legalName);
            this.groupBox3.Controls.Add(this.shortName);
            this.groupBox3.Location = new System.Drawing.Point(6, 127);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(318, 45);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Applies To ";
            // 
            // legalName
            // 
            this.legalName.AutoSize = true;
            this.legalName.Location = new System.Drawing.Point(176, 19);
            this.legalName.Name = "legalName";
            this.legalName.Size = new System.Drawing.Size(82, 17);
            this.legalName.TabIndex = 1;
            this.legalName.Text = "Legal Name";
            this.legalName.UseVisualStyleBackColor = true;
            // 
            // shortName
            // 
            this.shortName.AutoSize = true;
            this.shortName.Checked = true;
            this.shortName.Location = new System.Drawing.Point(23, 19);
            this.shortName.Name = "shortName";
            this.shortName.Size = new System.Drawing.Size(78, 17);
            this.shortName.TabIndex = 0;
            this.shortName.TabStop = true;
            this.shortName.Text = "Short Code";
            this.shortName.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(150, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "And";
            // 
            // txtEnd
            // 
            this.txtEnd.Location = new System.Drawing.Point(177, 92);
            this.txtEnd.Name = "txtEnd";
            this.txtEnd.Size = new System.Drawing.Size(144, 20);
            this.txtEnd.TabIndex = 3;
            // 
            // cboEnd
            // 
            this.cboEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEnd.FormattingEnabled = true;
            this.cboEnd.Location = new System.Drawing.Point(6, 93);
            this.cboEnd.Name = "cboEnd";
            this.cboEnd.Size = new System.Drawing.Size(162, 21);
            this.cboEnd.TabIndex = 2;
            // 
            // txtStart
            // 
            this.txtStart.Location = new System.Drawing.Point(177, 34);
            this.txtStart.Name = "txtStart";
            this.txtStart.Size = new System.Drawing.Size(144, 20);
            this.txtStart.TabIndex = 1;
            // 
            // cboStart
            // 
            this.cboStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStart.FormattingEnabled = true;
            this.cboStart.Location = new System.Drawing.Point(6, 33);
            this.cboStart.Name = "cboStart";
            this.cboStart.Size = new System.Drawing.Size(162, 21);
            this.cboStart.TabIndex = 0;
            // 
            // saveButton
            // 
            this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.saveButton.Location = new System.Drawing.Point(210, 245);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(123, 26);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "&Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeButton.Location = new System.Drawing.Point(367, 246);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(120, 25);
            this.closeButton.TabIndex = 4;
            this.closeButton.Text = "&Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // UserFilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 299);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Name = "UserFilterForm";
            this.Text = "Auto Filter";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cdtyList)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private DevExpress.XtraEditors.CheckedListBoxControl cdtyList;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private System.Windows.Forms.TextBox txtStart;
        private System.Windows.Forms.ComboBox cboStart;
        private System.Windows.Forms.ComboBox cboEnd;
        private System.Windows.Forms.TextBox txtEnd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton legalName;
        private System.Windows.Forms.RadioButton shortName;
    }
}