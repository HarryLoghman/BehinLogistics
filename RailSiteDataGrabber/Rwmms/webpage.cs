using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.rwmms
{
    class webpage : IDisposable
    {
        public List<webpageControl> prp_controls { get; set; }
        /// <summary>
        /// control to goto the previous page
        /// </summary>
        public webpageControl prp_controlBack { get; set; }
        public string prp_url { get; set; }

        protected ChromeDriverService v_webBrowserService;
        protected ChromeOptions v_webBrowserOptions;
        protected ChromeDriver v_webBrowser;

        public webpage(string url)
        {
            this.prp_url = url;

            this.v_webBrowserService = ChromeDriverService.CreateDefaultService();
            this.v_webBrowserService.HideCommandPromptWindow = true;

            this.v_webBrowserOptions = new ChromeOptions();
            //this.v_webBrowserOptions.AddArgument("--headless");
            //this.v_webBrowserOptions.AddUserProfilePreference("download.default_directory", this.v_downloadPath);
            this.v_webBrowser = new ChromeDriver(this.v_webBrowserService, this.v_webBrowserOptions);
        }

        public void Dispose()
        {
            this.v_webBrowser.Close();
            this.v_webBrowser.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginAndLogout">for those pages which have bugs in access controls</param>
        public void sb_readAllControlsAndSaveToDB(bool loginAndLogout)
        {
            login lg = new login();
            if (loginAndLogout)
            {
                lg = new login();
                lg.fnc_logoutWithSelenium(this.v_webBrowser);
            }
            if (login.fnc_isLoginPage(this.v_webBrowser.Url, this.prp_url))
            {
                lg.fnc_loginWithSelenium(this.v_webBrowser);
                this.v_webBrowser.Navigate().GoToUrl(this.prp_url);
            }

            this.sb_readCombosAndSaveToDB();
        }

        public void sb_readCombosAndSaveToDB()
        {
            var controls = this.prp_controls.Where(o => o.prp_controlType == enumControlType.combo).ToList();
            webpageCombo combo;
            int i;
            for (i = 0; i <= controls.Count - 1; i++)
            {
                combo = (webpageCombo)controls[i];
                if (combo.prp_tableSource == null)
                    continue;
                combo.fnc_saveTableSource();
            }
        }
    }
}
