#if !NETSTANDARD2_0_OR_GREATER
namespace Bannerlord.ButterLib.ExceptionHandler.WinForms
{
    partial class HtmlCrashReportForm
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
            this.HtmlRender = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // HtmlRender
            // 
            this.HtmlRender.AllowWebBrowserDrop = false;
            this.HtmlRender.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HtmlRender.IsWebBrowserContextMenuEnabled = false;
            this.HtmlRender.Location = new System.Drawing.Point(0, 0);
            this.HtmlRender.MinimumSize = new System.Drawing.Size(20, 20);
            this.HtmlRender.Name = "HtmlRender";
            this.HtmlRender.Size = new System.Drawing.Size(782, 553);
            this.HtmlRender.TabIndex = 0;
            this.HtmlRender.WebBrowserShortcutsEnabled = false;
            this.HtmlRender.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.HtmlRender_Navigating);
            // 
            // CrashReportForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.HtmlRender);
            this.Name = "CrashReportForm";
            this.ShowIcon = false;
            this.Text = "Crash Report";
            this.TransparencyKey = System.Drawing.Color.LightGray;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser HtmlRender;
    }
}
#endif