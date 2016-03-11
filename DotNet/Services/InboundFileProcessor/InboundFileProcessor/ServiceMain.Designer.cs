namespace InboundFileProcessor
{
    partial class ServiceMain
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.timerMain = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.timerMain)).BeginInit();
            // 
            // timerMain
            // 
            this.timerMain.Enabled = true;
            this.timerMain.Elapsed += new System.Timers.ElapsedEventHandler(this.timerMain_Elapsed);
            // 
            // ServiceMain
            // 
            this.ServiceName = "Service1";
            ((System.ComponentModel.ISupportInitialize)(this.timerMain)).EndInit();

        }

        #endregion

        private System.Timers.Timer timerMain;

    }
}
