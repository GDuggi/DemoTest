namespace ConfirmManager
{
   partial class frmGetAll
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGetAll));
            this.cmboTradingSystem = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.txtTicket = new DevExpress.XtraEditors.TextEdit();
            this.dtStartDate = new DevExpress.XtraEditors.DateEdit();
            this.dtTradeDate = new DevExpress.XtraEditors.DateEdit();
            this.pnlBottomEditRqmt = new DevExpress.XtraEditors.PanelControl();
            this.btnClearSearch = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnGetTradeData = new DevExpress.XtraEditors.SimpleButton();
            this.lkupCpty = new DevExpress.XtraEditors.LookUpEdit();
            this.lkupRbsCompany = new DevExpress.XtraEditors.LookUpEdit();
            this.lkupCommidity = new DevExpress.XtraEditors.LookUpEdit();
            this.txtCptyTradeId = new DevExpress.XtraEditors.TextEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.cmboTradingSystem.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTicket.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStartDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStartDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTradeDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTradeDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBottomEditRqmt)).BeginInit();
            this.pnlBottomEditRqmt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lkupCpty.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkupRbsCompany.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkupCommidity.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCptyTradeId.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // cmboTradingSystem
            // 
            this.cmboTradingSystem.Location = new System.Drawing.Point(130, 6);
            this.cmboTradingSystem.Name = "cmboTradingSystem";
            this.cmboTradingSystem.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmboTradingSystem.Size = new System.Drawing.Size(118, 20);
            this.cmboTradingSystem.TabIndex = 0;
            this.cmboTradingSystem.ToolTip = "Disputed reason";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(13, 13);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(78, 13);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "Trading System:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(13, 38);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(32, 13);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "Ticket:";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(13, 88);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(70, 13);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "Our Company:";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(13, 113);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(69, 13);
            this.labelControl4.TabIndex = 6;
            this.labelControl4.Text = "Counterparty:";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(12, 138);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(57, 13);
            this.labelControl5.TabIndex = 7;
            this.labelControl5.Text = "Commodity:";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(12, 163);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(92, 13);
            this.labelControl6.TabIndex = 8;
            this.labelControl6.Text = "Trade Date - From:";
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(12, 188);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(80, 13);
            this.labelControl7.TabIndex = 9;
            this.labelControl7.Text = "Trade Date - To:";
            // 
            // txtTicket
            // 
            this.txtTicket.Location = new System.Drawing.Point(130, 32);
            this.txtTicket.Name = "txtTicket";
            this.txtTicket.Size = new System.Drawing.Size(118, 20);
            this.txtTicket.TabIndex = 1;
            // 
            // dtStartDate
            // 
            this.dtStartDate.EditValue = null;
            this.dtStartDate.Location = new System.Drawing.Point(130, 162);
            this.dtStartDate.Name = "dtStartDate";
            this.dtStartDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtStartDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtStartDate.Properties.DisplayFormat.FormatString = "dd-MMM-yyyy";
            this.dtStartDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtStartDate.Size = new System.Drawing.Size(118, 20);
            this.dtStartDate.TabIndex = 6;
            // 
            // dtTradeDate
            // 
            this.dtTradeDate.EditValue = null;
            this.dtTradeDate.Location = new System.Drawing.Point(130, 188);
            this.dtTradeDate.Name = "dtTradeDate";
            this.dtTradeDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtTradeDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dtTradeDate.Properties.DisplayFormat.FormatString = "dd-MMM-yyyy";
            this.dtTradeDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dtTradeDate.Size = new System.Drawing.Size(118, 20);
            this.dtTradeDate.TabIndex = 7;
            // 
            // pnlBottomEditRqmt
            // 
            this.pnlBottomEditRqmt.Controls.Add(this.btnClearSearch);
            this.pnlBottomEditRqmt.Controls.Add(this.btnCancel);
            this.pnlBottomEditRqmt.Controls.Add(this.btnGetTradeData);
            this.pnlBottomEditRqmt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottomEditRqmt.Location = new System.Drawing.Point(0, 221);
            this.pnlBottomEditRqmt.Name = "pnlBottomEditRqmt";
            this.pnlBottomEditRqmt.Size = new System.Drawing.Size(274, 40);
            this.pnlBottomEditRqmt.TabIndex = 16;
            // 
            // btnClearSearch
            // 
            this.btnClearSearch.Location = new System.Drawing.Point(115, 6);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Size = new System.Drawing.Size(70, 27);
            this.btnClearSearch.TabIndex = 1;
            this.btnClearSearch.Text = "Clear";
            this.btnClearSearch.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(193, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 27);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            // 
            // btnGetTradeData
            // 
            this.btnGetTradeData.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnGetTradeData.Location = new System.Drawing.Point(8, 7);
            this.btnGetTradeData.Name = "btnGetTradeData";
            this.btnGetTradeData.Size = new System.Drawing.Size(99, 27);
            this.btnGetTradeData.TabIndex = 0;
            this.btnGetTradeData.Text = "&Get Trade Data";
            this.btnGetTradeData.Click += new System.EventHandler(this.btnGetTradeData_Click);
            // 
            // lkupCpty
            // 
            this.lkupCpty.Location = new System.Drawing.Point(130, 110);
            this.lkupCpty.Name = "lkupCpty";
            this.lkupCpty.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkupCpty.Properties.DropDownRows = 12;
            this.lkupCpty.Properties.NullText = "";
            this.lkupCpty.Size = new System.Drawing.Size(118, 20);
            this.lkupCpty.TabIndex = 4;
            // 
            // lkupRbsCompany
            // 
            this.lkupRbsCompany.Location = new System.Drawing.Point(130, 84);
            this.lkupRbsCompany.Name = "lkupRbsCompany";
            this.lkupRbsCompany.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkupRbsCompany.Properties.DropDownRows = 12;
            this.lkupRbsCompany.Properties.NullText = "";
            this.lkupRbsCompany.Size = new System.Drawing.Size(118, 20);
            this.lkupRbsCompany.TabIndex = 3;
            // 
            // lkupCommidity
            // 
            this.lkupCommidity.Location = new System.Drawing.Point(130, 136);
            this.lkupCommidity.Name = "lkupCommidity";
            this.lkupCommidity.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkupCommidity.Properties.DropDownRows = 12;
            this.lkupCommidity.Properties.NullText = "";
            this.lkupCommidity.Size = new System.Drawing.Size(118, 20);
            this.lkupCommidity.TabIndex = 5;
            // 
            // txtCptyTradeId
            // 
            this.txtCptyTradeId.Location = new System.Drawing.Point(130, 58);
            this.txtCptyTradeId.Name = "txtCptyTradeId";
            this.txtCptyTradeId.Size = new System.Drawing.Size(118, 20);
            this.txtCptyTradeId.TabIndex = 2;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(13, 63);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(113, 13);
            this.labelControl8.TabIndex = 18;
            this.labelControl8.Text = "Counterparty Trade Id:";
            // 
            // frmGetAll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 261);
            this.Controls.Add(this.txtCptyTradeId);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.lkupCommidity);
            this.Controls.Add(this.lkupRbsCompany);
            this.Controls.Add(this.lkupCpty);
            this.Controls.Add(this.pnlBottomEditRqmt);
            this.Controls.Add(this.dtTradeDate);
            this.Controls.Add(this.dtStartDate);
            this.Controls.Add(this.txtTicket);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.cmboTradingSystem);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGetAll";
            this.ShowInTaskbar = false;
            this.Text = "Get All";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmGetAll_FormClosing);
            this.Load += new System.EventHandler(this.frmGetAll_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmboTradingSystem.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTicket.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStartDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtStartDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTradeDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtTradeDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBottomEditRqmt)).EndInit();
            this.pnlBottomEditRqmt.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lkupCpty.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkupRbsCompany.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkupCommidity.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCptyTradeId.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.ComboBoxEdit cmboTradingSystem;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      private DevExpress.XtraEditors.LabelControl labelControl2;
      private DevExpress.XtraEditors.LabelControl labelControl3;
      private DevExpress.XtraEditors.LabelControl labelControl4;
      private DevExpress.XtraEditors.LabelControl labelControl5;
      private DevExpress.XtraEditors.LabelControl labelControl6;
       private DevExpress.XtraEditors.LabelControl labelControl7;
      private DevExpress.XtraEditors.TextEdit txtTicket;
      private DevExpress.XtraEditors.DateEdit dtStartDate;
      private DevExpress.XtraEditors.DateEdit dtTradeDate;
      private DevExpress.XtraEditors.PanelControl pnlBottomEditRqmt;
      private DevExpress.XtraEditors.SimpleButton btnClearSearch;
      private DevExpress.XtraEditors.SimpleButton btnCancel;
      private DevExpress.XtraEditors.SimpleButton btnGetTradeData;
       private DevExpress.XtraEditors.LookUpEdit lkupCpty;
       private DevExpress.XtraEditors.LookUpEdit lkupRbsCompany;
       private DevExpress.XtraEditors.LookUpEdit lkupCommidity;
       private DevExpress.XtraEditors.TextEdit txtCptyTradeId;
       private DevExpress.XtraEditors.LabelControl labelControl8;
   }
}