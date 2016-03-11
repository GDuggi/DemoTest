namespace ApacheNMS_Test
{
    partial class frmTestSimple
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
            this.btnStartLstn = new System.Windows.Forms.Button();
            this.btnSendMsg = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnInitForm = new System.Windows.Forms.Button();
            this.btnStopLstn = new System.Windows.Forms.Button();
            this.rtxtMsgRcv = new System.Windows.Forms.RichTextBox();
            this.btnDestroy = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnClearMsg = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStartLstn
            // 
            this.btnStartLstn.Location = new System.Drawing.Point(145, 12);
            this.btnStartLstn.Name = "btnStartLstn";
            this.btnStartLstn.Size = new System.Drawing.Size(132, 23);
            this.btnStartLstn.TabIndex = 1;
            this.btnStartLstn.Text = "Start Listening";
            this.btnStartLstn.UseVisualStyleBackColor = true;
            this.btnStartLstn.Click += new System.EventHandler(this.btnStartLstn_Click);
            // 
            // btnSendMsg
            // 
            this.btnSendMsg.Location = new System.Drawing.Point(145, 116);
            this.btnSendMsg.Name = "btnSendMsg";
            this.btnSendMsg.Size = new System.Drawing.Size(132, 23);
            this.btnSendMsg.TabIndex = 0;
            this.btnSendMsg.Text = "Send Message";
            this.btnSendMsg.UseVisualStyleBackColor = true;
            this.btnSendMsg.Click += new System.EventHandler(this.btnSendMsg_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(346, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnInitForm
            // 
            this.btnInitForm.Enabled = false;
            this.btnInitForm.Location = new System.Drawing.Point(12, 12);
            this.btnInitForm.Name = "btnInitForm";
            this.btnInitForm.Size = new System.Drawing.Size(103, 23);
            this.btnInitForm.TabIndex = 0;
            this.btnInitForm.Text = "Init Form";
            this.btnInitForm.UseVisualStyleBackColor = true;
            this.btnInitForm.Click += new System.EventHandler(this.btnInitForm_Click);
            // 
            // btnStopLstn
            // 
            this.btnStopLstn.Location = new System.Drawing.Point(145, 59);
            this.btnStopLstn.Name = "btnStopLstn";
            this.btnStopLstn.Size = new System.Drawing.Size(132, 23);
            this.btnStopLstn.TabIndex = 1;
            this.btnStopLstn.Text = "Stop  Listening";
            this.btnStopLstn.UseVisualStyleBackColor = true;
            this.btnStopLstn.Click += new System.EventHandler(this.btnStopLstn_Click);
            // 
            // rtxtMsgRcv
            // 
            this.rtxtMsgRcv.Location = new System.Drawing.Point(1, 175);
            this.rtxtMsgRcv.Name = "rtxtMsgRcv";
            this.rtxtMsgRcv.Size = new System.Drawing.Size(430, 215);
            this.rtxtMsgRcv.TabIndex = 3;
            this.rtxtMsgRcv.Text = "";
            // 
            // btnDestroy
            // 
            this.btnDestroy.Location = new System.Drawing.Point(12, 59);
            this.btnDestroy.Name = "btnDestroy";
            this.btnDestroy.Size = new System.Drawing.Size(103, 23);
            this.btnDestroy.TabIndex = 4;
            this.btnDestroy.Text = "Destroy Resources";
            this.btnDestroy.UseVisualStyleBackColor = true;
            this.btnDestroy.Click += new System.EventHandler(this.btnDestroy_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(346, 116);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 5;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnClearMsg
            // 
            this.btnClearMsg.Location = new System.Drawing.Point(12, 116);
            this.btnClearMsg.Name = "btnClearMsg";
            this.btnClearMsg.Size = new System.Drawing.Size(103, 23);
            this.btnClearMsg.TabIndex = 6;
            this.btnClearMsg.Text = "Clear Messages";
            this.btnClearMsg.UseVisualStyleBackColor = true;
            this.btnClearMsg.Click += new System.EventHandler(this.btnClearMsg_Click);
            // 
            // frmTestSimple
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 389);
            this.Controls.Add(this.btnClearMsg);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnDestroy);
            this.Controls.Add(this.rtxtMsgRcv);
            this.Controls.Add(this.btnStopLstn);
            this.Controls.Add(this.btnStartLstn);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnInitForm);
            this.Controls.Add(this.btnSendMsg);
            this.Location = new System.Drawing.Point(700, 200);
            this.Name = "frmTestSimple";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Apache-NMS Test";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmTestMain_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStartLstn;
        private System.Windows.Forms.Button btnSendMsg;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnInitForm;
        private System.Windows.Forms.Button btnStopLstn;
        private System.Windows.Forms.RichTextBox rtxtMsgRcv;
        private System.Windows.Forms.Button btnDestroy;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnClearMsg;
    }
}

