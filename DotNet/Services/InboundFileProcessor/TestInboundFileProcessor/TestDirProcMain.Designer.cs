namespace TestInboundFileProcessor
{
    partial class TestDirProcMain
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
            this.btnProcessDirs = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssLabelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnExtDirRead = new System.Windows.Forms.Button();
            this.tbFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxCallerRef = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbRootDir = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxSentTo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbOuputDir = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbProcessedDir = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbFailedDir = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnProcessDirs
            // 
            this.btnProcessDirs.Location = new System.Drawing.Point(10, 23);
            this.btnProcessDirs.Name = "btnProcessDirs";
            this.btnProcessDirs.Size = new System.Drawing.Size(100, 23);
            this.btnProcessDirs.TabIndex = 0;
            this.btnProcessDirs.Text = "Process Dirs";
            this.btnProcessDirs.UseVisualStyleBackColor = true;
            this.btnProcessDirs.Click += new System.EventHandler(this.btnProcessDirs_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(543, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssLabelStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 298);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(630, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tssLabelStatus
            // 
            this.tssLabelStatus.Name = "tssLabelStatus";
            this.tssLabelStatus.Size = new System.Drawing.Size(56, 17);
            this.tssLabelStatus.Text = "[Inactive]";
            // 
            // btnExtDirRead
            // 
            this.btnExtDirRead.Enabled = false;
            this.btnExtDirRead.Location = new System.Drawing.Point(141, 23);
            this.btnExtDirRead.Name = "btnExtDirRead";
            this.btnExtDirRead.Size = new System.Drawing.Size(105, 23);
            this.btnExtDirRead.TabIndex = 3;
            this.btnExtDirRead.Text = "Directory Read";
            this.btnExtDirRead.UseVisualStyleBackColor = true;
            this.btnExtDirRead.Visible = false;
            this.btnExtDirRead.Click += new System.EventHandler(this.btnExtDirRead_Click);
            // 
            // tbFileName
            // 
            this.tbFileName.Enabled = false;
            this.tbFileName.Location = new System.Drawing.Point(13, 534);
            this.tbFileName.Name = "tbFileName";
            this.tbFileName.Size = new System.Drawing.Size(476, 20);
            this.tbFileName.TabIndex = 4;
            this.tbFileName.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(13, 515);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "FileName to process";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(12, 423);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Caller Ref";
            this.label2.Visible = false;
            // 
            // textBoxCallerRef
            // 
            this.textBoxCallerRef.Enabled = false;
            this.textBoxCallerRef.Location = new System.Drawing.Point(12, 439);
            this.textBoxCallerRef.Name = "textBoxCallerRef";
            this.textBoxCallerRef.Size = new System.Drawing.Size(476, 20);
            this.textBoxCallerRef.TabIndex = 7;
            this.textBoxCallerRef.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "RootDir to process";
            // 
            // tbRootDir
            // 
            this.tbRootDir.Location = new System.Drawing.Point(12, 84);
            this.tbRootDir.Name = "tbRootDir";
            this.tbRootDir.Size = new System.Drawing.Size(602, 20);
            this.tbRootDir.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(12, 387);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "CallerRef/SentTo";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // textBoxSentTo
            // 
            this.textBoxSentTo.Enabled = false;
            this.textBoxSentTo.Location = new System.Drawing.Point(12, 480);
            this.textBoxSentTo.Name = "textBoxSentTo";
            this.textBoxSentTo.Size = new System.Drawing.Size(476, 20);
            this.textBoxSentTo.TabIndex = 12;
            this.textBoxSentTo.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Location = new System.Drawing.Point(12, 464);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Sent To";
            this.label4.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "OutputDir";
            // 
            // tbOuputDir
            // 
            this.tbOuputDir.Location = new System.Drawing.Point(12, 131);
            this.tbOuputDir.Name = "tbOuputDir";
            this.tbOuputDir.Size = new System.Drawing.Size(602, 20);
            this.tbOuputDir.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 161);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "ProcessedDir";
            // 
            // tbProcessedDir
            // 
            this.tbProcessedDir.Location = new System.Drawing.Point(10, 180);
            this.tbProcessedDir.Name = "tbProcessedDir";
            this.tbProcessedDir.Size = new System.Drawing.Size(602, 20);
            this.tbProcessedDir.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 205);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "FailedDir";
            // 
            // tbFailedDir
            // 
            this.tbFailedDir.Location = new System.Drawing.Point(10, 224);
            this.tbFailedDir.Name = "tbFailedDir";
            this.tbFailedDir.Size = new System.Drawing.Size(602, 20);
            this.tbFailedDir.TabIndex = 19;
            // 
            // TestDirProcMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 320);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.tbFailedDir);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbProcessedDir);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbOuputDir);
            this.Controls.Add(this.textBoxSentTo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbRootDir);
            this.Controls.Add(this.textBoxCallerRef);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbFileName);
            this.Controls.Add(this.btnExtDirRead);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnProcessDirs);
            this.Name = "TestDirProcMain";
            this.Text = "Test Directory Processor";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnProcessDirs;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tssLabelStatus;
        private System.Windows.Forms.Button btnExtDirRead;
        private System.Windows.Forms.TextBox tbFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxCallerRef;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbRootDir;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxSentTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbOuputDir;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbProcessedDir;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbFailedDir;
    }
}

