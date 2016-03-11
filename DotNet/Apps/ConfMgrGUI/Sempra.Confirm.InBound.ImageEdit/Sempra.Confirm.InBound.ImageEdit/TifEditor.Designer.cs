using Leadtools.WinForms;
using Leadtools.Codecs;
using Leadtools.Annotations;
using Leadtools;

namespace Sempra.Confirm.InBound.ImageEdit
{
    partial class TifEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        /// 
          private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            CleanUp();
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
            this.topToolBar = new System.Windows.Forms.ToolStripContainer();
            this.topToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // topToolBar
            // 
            this.topToolBar.BottomToolStripPanelVisible = false;
            // 
            // topToolBar.ContentPanel
            // 
            this.topToolBar.ContentPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.topToolBar.ContentPanel.Size = new System.Drawing.Size(561, 0);
            this.topToolBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.topToolBar.LeftToolStripPanelVisible = false;
            this.topToolBar.Location = new System.Drawing.Point(0, 0);
            this.topToolBar.Name = "topToolBar";
            this.topToolBar.RightToolStripPanelVisible = false;
            this.topToolBar.Size = new System.Drawing.Size(561, 24);
            this.topToolBar.TabIndex = 3;
            this.topToolBar.Text = "toolStripContainer1";
            // 
            // TifEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.topToolBar);
            this.Name = "TifEditor";
            this.Size = new System.Drawing.Size(561, 344);
            this.Enter += new System.EventHandler(this.TifEditor_Enter);
            this.topToolBar.ResumeLayout(false);
            this.topToolBar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

       
        private RasterImageViewer imageViewer;
        private System.Windows.Forms.ToolStripContainer topToolBar;
    }
}
