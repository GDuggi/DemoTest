using System.Windows.Forms;

namespace NSRiskManager
{
    partial class OpenFormChooser
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FormManagerGridControl = new DevExpress.XtraGrid.GridControl();
            this.ForManagerGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.OpenSelectedButton = new System.Windows.Forms.Button();
            this.CancelSelectedButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.FormManagerGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ForManagerGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // FormManagerGridControl
            // 
            this.FormManagerGridControl.Location = new System.Drawing.Point(1, 0);
            this.FormManagerGridControl.MainView = this.ForManagerGridView;
            this.FormManagerGridControl.Name = "FormManagerGridControl";
            this.FormManagerGridControl.Size = new System.Drawing.Size(381, 272);
            this.FormManagerGridControl.TabIndex = 0;
            this.FormManagerGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.ForManagerGridView});
            // 
            // ForManagerGridView
            // 
            this.ForManagerGridView.GridControl = this.FormManagerGridControl;
            this.ForManagerGridView.Name = "ForManagerGridView";
            this.ForManagerGridView.OptionsBehavior.Editable = false;
            this.ForManagerGridView.OptionsView.ShowGroupPanel = false;
            // 
            // OpenSelectedButton
            // 
            this.OpenSelectedButton.Location = new System.Drawing.Point(48, 278);
            this.OpenSelectedButton.Name = "OpenSelectedButton";
            this.OpenSelectedButton.Size = new System.Drawing.Size(100, 23);
            this.OpenSelectedButton.TabIndex = 1;
            this.OpenSelectedButton.Text = "Open Selected";
            this.OpenSelectedButton.UseVisualStyleBackColor = true;
            this.OpenSelectedButton.DialogResult = DialogResult.OK;

            // 
            // CancelSelectedButton
            // 
            this.CancelSelectedButton.Location = new System.Drawing.Point(212, 278);
            this.CancelSelectedButton.Name = "CancelSelectedButton";
            this.CancelSelectedButton.Size = new System.Drawing.Size(75, 23);
            this.CancelSelectedButton.TabIndex = 2;
            this.CancelSelectedButton.Text = "Cancel";
            this.CancelSelectedButton.UseVisualStyleBackColor = true;
            this.CancelSelectedButton.DialogResult = DialogResult.Cancel;
            // 
            // OpenFormChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 313);
            this.Controls.Add(this.CancelSelectedButton);
            this.Controls.Add(this.OpenSelectedButton);
            this.Controls.Add(this.FormManagerGridControl);
            this.Name = "OpenFormChooser";
            this.Text = "Choose Form To Open";
            this.Load += new System.EventHandler(this.formLoad);
            ((System.ComponentModel.ISupportInitialize)(this.FormManagerGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ForManagerGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl FormManagerGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView ForManagerGridView;
        private System.Windows.Forms.Button OpenSelectedButton;
        private System.Windows.Forms.Button CancelSelectedButton;
    }
}