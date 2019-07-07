namespace BehinPWSGrabber
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
            this.BehinPWSGrabberProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.BehinPWSGrabberServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // BehinPWSGrabberProcessInstaller
            // 
            this.BehinPWSGrabberProcessInstaller.Password = null;
            this.BehinPWSGrabberProcessInstaller.Username = null;
            // 
            // BehinPWSGrabberServiceInstaller
            // 
            this.BehinPWSGrabberServiceInstaller.DisplayName = "BehinPWSGrabberService";
            this.BehinPWSGrabberServiceInstaller.ServiceName = "BehinPWSGrabberService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.BehinPWSGrabberProcessInstaller,
            this.BehinPWSGrabberServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller BehinPWSGrabberProcessInstaller;
        private System.ServiceProcess.ServiceInstaller BehinPWSGrabberServiceInstaller;
    }
}