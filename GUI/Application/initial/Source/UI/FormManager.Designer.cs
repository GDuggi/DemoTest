using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;

namespace NSRiskManager
{
    partial class FormManager
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
            this.OpenWindowsGridControl = new DevExpress.XtraGrid.GridControl();
            this.OpenWindowsMainGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.OpenSelectedButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.SaveSelectedButton = new System.Windows.Forms.Button();
            this.SaveAllButton = new System.Windows.Forms.Button();
            this.DeleteSelected = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.OpenWindowsGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OpenWindowsMainGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // OpenWindowsGridControl
            // 
            this.OpenWindowsGridControl.Location = new System.Drawing.Point(1, 0);
            this.OpenWindowsGridControl.MainView = this.OpenWindowsMainGridView;
            this.OpenWindowsGridControl.Name = "OpenWindowsGridControl";
            this.OpenWindowsGridControl.Size = new System.Drawing.Size(619, 251);
            this.OpenWindowsGridControl.TabIndex = 0;
            this.OpenWindowsGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.OpenWindowsMainGridView});
            // 
            // OpenWindowsMainGridView
            // 
            this.OpenWindowsMainGridView.GridControl = this.OpenWindowsGridControl;
            this.OpenWindowsMainGridView.Name = "OpenWindowsMainGridView";
            this.OpenWindowsMainGridView.OptionsSelection.MultiSelect = true;
            this.OpenWindowsMainGridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.OpenWindowsMainGridView.OptionsView.ShowGroupPanel = false;
          
            // 
            // OpenSelectedButton
            // 
            this.OpenSelectedButton.Location = new System.Drawing.Point(1, 257);
            this.OpenSelectedButton.Name = "OpenSelectedButton";
            this.OpenSelectedButton.Size = new System.Drawing.Size(100, 23);
            this.OpenSelectedButton.TabIndex = 1;
            this.OpenSelectedButton.Text = "Open Selected";
            this.OpenSelectedButton.UseVisualStyleBackColor = true;
            this.OpenSelectedButton.Click += new System.EventHandler(this.OpenSelectedButtonClickedHandler);

            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(536, 257);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(75, 23);
            this.ExitButton.TabIndex = 2;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.DialogResult = DialogResult.Cancel;
            // 
            // SaveSelectedButton
            // 
            this.SaveSelectedButton.Location = new System.Drawing.Point(213, 257);
            this.SaveSelectedButton.Name = "SaveSelectedButton";
            this.SaveSelectedButton.Size = new System.Drawing.Size(100, 23);
            this.SaveSelectedButton.TabIndex = 3;
            this.SaveSelectedButton.Text = "Save Selected";
            this.SaveSelectedButton.UseVisualStyleBackColor = true;
            this.SaveSelectedButton.Click += new System.EventHandler(this.SaveSelectedButtonClickedHandler);
            // 
            // SaveAllButton
            // 
            this.SaveAllButton.Location = new System.Drawing.Point(319, 257);
            this.SaveAllButton.Name = "SaveAllButton";
            this.SaveAllButton.Size = new System.Drawing.Size(100, 23);
            this.SaveAllButton.TabIndex = 4;
            this.SaveAllButton.Text = "Save All";
            this.SaveAllButton.UseVisualStyleBackColor = true;
            this.SaveAllButton.Click += new System.EventHandler(this.SaveAllButtonClickedHandler);

            // 
            // DeleteSelected
            // 
            this.DeleteSelected.Location = new System.Drawing.Point(107, 257);
            this.DeleteSelected.Name = "DeleteSelected";
            this.DeleteSelected.Size = new System.Drawing.Size(100, 23);
            this.DeleteSelected.TabIndex = 5;
            this.DeleteSelected.Text = "Delete Selected";
            this.DeleteSelected.UseVisualStyleBackColor = true;
            this.DeleteSelected.Click += new System.EventHandler(this.DeleteSelectedButtonClickedHandler);

            // 
            // FormManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 288);
            this.Controls.Add(this.DeleteSelected);
            this.Controls.Add(this.SaveAllButton);
            this.Controls.Add(this.SaveSelectedButton);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.OpenSelectedButton);
            this.Controls.Add(this.OpenWindowsGridControl);
            this.Name = "FormManager";
            this.Text = "Manage Your Forms";
            this.Load += new System.EventHandler(this.formLoad);
            ((System.ComponentModel.ISupportInitialize)(this.OpenWindowsGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OpenWindowsMainGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl OpenWindowsGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView OpenWindowsMainGridView;
        private System.Windows.Forms.Button OpenSelectedButton;
        private System.Windows.Forms.Button ExitButton;
        private Button SaveSelectedButton;
        private Button SaveAllButton;
        private Button DeleteSelected;
    }
}