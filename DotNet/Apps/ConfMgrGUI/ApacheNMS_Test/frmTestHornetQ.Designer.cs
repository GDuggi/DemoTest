namespace ApacheNMS_Test
{
    partial class frmTestHornetQ
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
            this.cboxDestinationType = new System.Windows.Forms.ComboBox();
            this.ststripMain = new System.Windows.Forms.StatusStrip();
            this.toolstripMessageServerUriLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tstripMessageServerUri = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripLabelUserId = new System.Windows.Forms.ToolStripStatusLabel();
            this.tstripUserId = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripLabelPassword = new System.Windows.Forms.ToolStripStatusLabel();
            this.tstripPassword = new System.Windows.Forms.ToolStripStatusLabel();
            this.cboxQueueList = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboxTopicList = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tboxSimpleMessage = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tboxReceivedMessage = new System.Windows.Forms.TextBox();
            this.btnPublish = new System.Windows.Forms.Button();
            this.btnSetup = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.ststripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Destination Type:";
            // 
            // cboxDestinationType
            // 
            this.cboxDestinationType.Enabled = false;
            this.cboxDestinationType.FormattingEnabled = true;
            this.cboxDestinationType.Items.AddRange(new object[] {
            "Queue",
            "Topic"});
            this.cboxDestinationType.Location = new System.Drawing.Point(104, 12);
            this.cboxDestinationType.Name = "cboxDestinationType";
            this.cboxDestinationType.Size = new System.Drawing.Size(68, 21);
            this.cboxDestinationType.TabIndex = 1;
            this.cboxDestinationType.SelectedIndexChanged += new System.EventHandler(this.cboxDestinationType_SelectedIndexChanged);
            // 
            // ststripMain
            // 
            this.ststripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.ststripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolstripMessageServerUriLabel,
            this.tstripMessageServerUri,
            this.toolStripLabelUserId,
            this.tstripUserId,
            this.toolStripLabelPassword,
            this.tstripPassword});
            this.ststripMain.Location = new System.Drawing.Point(0, 379);
            this.ststripMain.Name = "ststripMain";
            this.ststripMain.Size = new System.Drawing.Size(683, 22);
            this.ststripMain.TabIndex = 4;
            this.ststripMain.Text = "statusStrip1";
            // 
            // toolstripMessageServerUriLabel
            // 
            this.toolstripMessageServerUriLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolstripMessageServerUriLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.toolstripMessageServerUriLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolstripMessageServerUriLabel.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.toolstripMessageServerUriLabel.Name = "toolstripMessageServerUriLabel";
            this.toolstripMessageServerUriLabel.Size = new System.Drawing.Size(108, 20);
            this.toolstripMessageServerUriLabel.Text = "Message Server Uri:";
            // 
            // tstripMessageServerUri
            // 
            this.tstripMessageServerUri.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tstripMessageServerUri.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.tstripMessageServerUri.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tstripMessageServerUri.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.tstripMessageServerUri.Name = "tstripMessageServerUri";
            this.tstripMessageServerUri.Size = new System.Drawing.Size(112, 20);
            this.tstripMessageServerUri.Text = "[Message Server Uri]";
            // 
            // toolStripLabelUserId
            // 
            this.toolStripLabelUserId.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripLabelUserId.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.toolStripLabelUserId.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripLabelUserId.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.toolStripLabelUserId.Name = "toolStripLabelUserId";
            this.toolStripLabelUserId.Size = new System.Drawing.Size(50, 20);
            this.toolStripLabelUserId.Text = "User Id:";
            // 
            // tstripUserId
            // 
            this.tstripUserId.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tstripUserId.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.tstripUserId.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tstripUserId.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.tstripUserId.Name = "tstripUserId";
            this.tstripUserId.Size = new System.Drawing.Size(54, 20);
            this.tstripUserId.Text = "[User Id]";
            // 
            // toolStripLabelPassword
            // 
            this.toolStripLabelPassword.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripLabelPassword.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.toolStripLabelPassword.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripLabelPassword.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.toolStripLabelPassword.Name = "toolStripLabelPassword";
            this.toolStripLabelPassword.Size = new System.Drawing.Size(61, 20);
            this.toolStripLabelPassword.Text = "Password:";
            // 
            // tstripPassword
            // 
            this.tstripPassword.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tstripPassword.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.tstripPassword.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tstripPassword.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.tstripPassword.Name = "tstripPassword";
            this.tstripPassword.Size = new System.Drawing.Size(65, 20);
            this.tstripPassword.Text = "[Password]";
            // 
            // cboxQueueList
            // 
            this.cboxQueueList.Enabled = false;
            this.cboxQueueList.FormattingEnabled = true;
            this.cboxQueueList.Location = new System.Drawing.Point(104, 39);
            this.cboxQueueList.Name = "cboxQueueList";
            this.cboxQueueList.Size = new System.Drawing.Size(295, 21);
            this.cboxQueueList.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Queue:";
            // 
            // cboxTopicList
            // 
            this.cboxTopicList.FormattingEnabled = true;
            this.cboxTopicList.Location = new System.Drawing.Point(104, 66);
            this.cboxTopicList.Name = "cboxTopicList";
            this.cboxTopicList.Size = new System.Drawing.Size(295, 21);
            this.cboxTopicList.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Topic:";
            // 
            // tboxSimpleMessage
            // 
            this.tboxSimpleMessage.Location = new System.Drawing.Point(104, 94);
            this.tboxSimpleMessage.Name = "tboxSimpleMessage";
            this.tboxSimpleMessage.Size = new System.Drawing.Size(444, 20);
            this.tboxSimpleMessage.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Simple Message:";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(598, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Received Message:";
            // 
            // tboxReceivedMessage
            // 
            this.tboxReceivedMessage.Location = new System.Drawing.Point(104, 120);
            this.tboxReceivedMessage.Multiline = true;
            this.tboxReceivedMessage.Name = "tboxReceivedMessage";
            this.tboxReceivedMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tboxReceivedMessage.Size = new System.Drawing.Size(444, 224);
            this.tboxReceivedMessage.TabIndex = 13;
            // 
            // btnPublish
            // 
            this.btnPublish.Location = new System.Drawing.Point(554, 92);
            this.btnPublish.Name = "btnPublish";
            this.btnPublish.Size = new System.Drawing.Size(75, 23);
            this.btnPublish.TabIndex = 14;
            this.btnPublish.Text = "Publish";
            this.btnPublish.UseVisualStyleBackColor = true;
            this.btnPublish.Click += new System.EventHandler(this.btnPublish_Click);
            // 
            // btnSetup
            // 
            this.btnSetup.Location = new System.Drawing.Point(405, 64);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(143, 23);
            this.btnSetup.TabIndex = 15;
            this.btnSetup.Text = "Setup Selected Topic";
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(405, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 16;
            this.button2.Text = "TestDate";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // frmTestHornetQ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 401);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnSetup);
            this.Controls.Add(this.btnPublish);
            this.Controls.Add(this.tboxReceivedMessage);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tboxSimpleMessage);
            this.Controls.Add(this.cboxTopicList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboxQueueList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ststripMain);
            this.Controls.Add(this.cboxDestinationType);
            this.Controls.Add(this.label1);
            this.Name = "frmTestHornetQ";
            this.Text = "Test HornetQ";
            this.ststripMain.ResumeLayout(false);
            this.ststripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboxDestinationType;
        private System.Windows.Forms.StatusStrip ststripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolstripMessageServerUriLabel;
        private System.Windows.Forms.ToolStripStatusLabel tstripMessageServerUri;
        private System.Windows.Forms.ToolStripStatusLabel toolStripLabelUserId;
        private System.Windows.Forms.ToolStripStatusLabel tstripUserId;
        private System.Windows.Forms.ToolStripStatusLabel toolStripLabelPassword;
        private System.Windows.Forms.ToolStripStatusLabel tstripPassword;
        private System.Windows.Forms.ComboBox cboxQueueList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboxTopicList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tboxSimpleMessage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tboxReceivedMessage;
        private System.Windows.Forms.Button btnPublish;
        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.Button button2;
    }
}