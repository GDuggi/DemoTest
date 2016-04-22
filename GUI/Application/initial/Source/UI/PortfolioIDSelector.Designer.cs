
using System.Windows.Forms;

namespace NSRiskManager
{
    partial class PortfolioIDSelector
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
            formCurrentlyOpen = false;

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.CancelActionButton = new DevExpress.XtraEditors.SimpleButton();
            this.OKButton = new DevExpress.XtraEditors.SimpleButton();
            this.portfolioEditControl = new DevExpress.XtraEditors.TextEdit();
            this.portfolioLabel = new DevExpress.XtraEditors.LabelControl();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.portfolioEditControl.Properties)).BeginInit();
            this.SuspendLayout();

            this.errorProvider1.ContainerControl = this;

            // 
            // CancelActionButton
            // 
            this.CancelActionButton.Location = new System.Drawing.Point(128, 73);
            this.CancelActionButton.Name = "CancelActionButton";
            this.CancelActionButton.Size = new System.Drawing.Size(75, 23);
            this.CancelActionButton.TabIndex = 1;
            this.CancelActionButton.Text = "Cancel";
            this.CancelActionButton.DialogResult = DialogResult.Cancel;
            this.CancelActionButton.CausesValidation = false;
            this.CancelActionButton.Click += new System.EventHandler(this.CancelClickedHandler);

            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(31, 73);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "OK";
            this.OKButton.DialogResult = DialogResult.OK;
            this.OKButton.CausesValidation = true;
            this.OKButton.Click  += new System.EventHandler(this.OkClickedHandler);

            // 
            // portfolioEditControl
            // 
            this.portfolioEditControl.Location = new System.Drawing.Point(91, 18);
            this.portfolioEditControl.Name = "portfolioEditControl";
            this.portfolioEditControl.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.portfolioEditControl.Properties.Appearance.Options.UseBackColor = true;
            this.portfolioEditControl.Size = new System.Drawing.Size(112, 20);
            this.portfolioEditControl.TabIndex = 3;
            this.portfolioEditControl.Validating += new System.ComponentModel.CancelEventHandler(formPortfolioId_Validating);
            // 
            // portfolioLabel
            // 
            this.portfolioLabel.Location = new System.Drawing.Point(31, 21);
            this.portfolioLabel.Name = "portfolioLabel";
            this.portfolioLabel.Size = new System.Drawing.Size(54, 13);
            this.portfolioLabel.TabIndex = 4;
            this.portfolioLabel.Text = "Portfolio ID";
            // 
            // PortfolioIDSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 119);
            this.Controls.Add(this.portfolioLabel);
            this.Controls.Add(this.portfolioEditControl);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.CancelActionButton);
            this.Name = "PortfolioIDSelector";
            this.Load += new System.EventHandler(this.formLoad);


            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.portfolioEditControl.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton CancelActionButton;
        private DevExpress.XtraEditors.SimpleButton OKButton;
        private DevExpress.XtraEditors.TextEdit portfolioEditControl;
        private DevExpress.XtraEditors.LabelControl portfolioLabel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
