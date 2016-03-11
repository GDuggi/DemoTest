namespace InboundFileProcessor
{
    partial class ProjectInstaller
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
            this.serviceProcessInstallerInbDirProc = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstallerInbDirProc = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstallerInbDirProc
            // 
            this.serviceProcessInstallerInbDirProc.Password = null;
            this.serviceProcessInstallerInbDirProc.Username = null;
            // 
            // serviceInstallerInbDirProc
            // 
            this.serviceInstallerInbDirProc.Description = "Reads input folders, processes files, and places them in designated output folder" +
    "";
            this.serviceInstallerInbDirProc.DisplayName = "cnf-InboundFileProcessorService";
            this.serviceInstallerInbDirProc.ServiceName = "cnf-InboundFileProcessorService";
            this.serviceInstallerInbDirProc.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstallerInbDirProc,
            this.serviceInstallerInbDirProc});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstallerInbDirProc;
        private System.ServiceProcess.ServiceInstaller serviceInstallerInbDirProc;
    }
}