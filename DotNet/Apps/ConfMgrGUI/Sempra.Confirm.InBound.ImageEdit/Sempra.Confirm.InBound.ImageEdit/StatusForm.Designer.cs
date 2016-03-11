namespace Sempra.Confirm.InBound.ImageEdit
{
    partial class StatusForm
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
            this.bar = new System.Windows.Forms.ProgressBar();
            this.src = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSrcFile = new System.Windows.Forms.Label();
            this.lblDestFile = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bar
            // 
            this.bar.ForeColor = System.Drawing.Color.Green;
            this.bar.Location = new System.Drawing.Point(27, 107);
            this.bar.Name = "bar";
            this.bar.Size = new System.Drawing.Size(439, 24);
            this.bar.TabIndex = 0;
            this.bar.Value = 50;
            // 
            // src
            // 
            this.src.AutoSize = true;
            this.src.Location = new System.Drawing.Point(24, 9);
            this.src.Name = "src";
            this.src.Size = new System.Drawing.Size(94, 13);
            this.src.TabIndex = 1;
            this.src.Text = "Source File Name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Target File Name:";
            // 
            // lblSrcFile
            // 
            this.lblSrcFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblSrcFile.Location = new System.Drawing.Point(121, 9);
            this.lblSrcFile.Name = "lblSrcFile";
            this.lblSrcFile.Size = new System.Drawing.Size(345, 21);
            this.lblSrcFile.TabIndex = 3;
            this.lblSrcFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDestFile
            // 
            this.lblDestFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDestFile.Location = new System.Drawing.Point(121, 47);
            this.lblDestFile.Name = "lblDestFile";
            this.lblDestFile.Size = new System.Drawing.Size(345, 25);
            this.lblDestFile.TabIndex = 4;
            this.lblDestFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(24, 83);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(442, 21);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // StatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 160);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblDestFile);
            this.Controls.Add(this.lblSrcFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.src);
            this.Controls.Add(this.bar);
            this.Location = new System.Drawing.Point(83, 150);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StatusForm";
            this.Text = "Status";
            this.Load += new System.EventHandler(this.StatusForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar bar;
        private System.Windows.Forms.Label src;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSrcFile;
        private System.Windows.Forms.Label lblDestFile;
        private System.Windows.Forms.Label lblStatus;
    }
}