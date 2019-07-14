using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.htmlModel
{
    public class webpage : IDisposable
    {
        public List<htmlTable> prp_tables { get; set; }
        /// <summary>
        /// control to goto the previous page
        /// </summary>
        public By prp_controlBackBy { get; set; }
        //specify control to click after navigate to url
        public By prp_controlLoadBy { get; set; }
        public string prp_url { get; set; }

        public IWebDriver prp_webBrowser
        {
            get
            {
                return this.v_webBrowser;
            }
        }
        public int? prp_parentId { get; set; }
        protected ChromeDriverService v_webBrowserService;
        protected ChromeOptions v_webBrowserOptions;
        protected ChromeDriver v_webBrowser;

        public webpage(string url) : this(url, null, null)
        {

        }
        public webpage(string url, ChromeDriver webBrowser) : this(url, null, webBrowser)
        {

        }


        public webpage(int parentId) : this(null, parentId, null)
        {

        }

        public webpage(int parentId, ChromeDriver webBrowser) : this(null, parentId, webBrowser)
        {

        }
        private webpage(string url, int? parentId, ChromeDriver webBrowser)
        {
            this.prp_url = url;
            this.prp_parentId = parentId;
            if (webBrowser == null)
            {
                this.v_webBrowserService = ChromeDriverService.CreateDefaultService();
                this.v_webBrowserService.HideCommandPromptWindow = true;

                this.v_webBrowserOptions = new ChromeOptions();
                //this.v_webBrowserOptions.AddArgument("--headless");
                //this.v_webBrowserOptions.AddUserProfilePreference("download.default_directory", this.v_downloadPath);
                this.v_webBrowser = new ChromeDriver(this.v_webBrowserService, this.v_webBrowserOptions);
            }
            else this.v_webBrowser = webBrowser;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
        public void Dispose(bool disposeWebBrower)
        {
            if (!disposeWebBrower) return;
            if (this.prp_webBrowser != null)
            {
                this.v_webBrowser.Close();
                this.v_webBrowser.Dispose();
            }
        }

        public bool fnc_load(iLogin login)
        {
            if (!string.IsNullOrEmpty(this.prp_url))
            {
                this.v_webBrowser.Navigate().GoToUrl(this.prp_url);
                Functions.sb_waitForReady(this.v_webBrowser);

                if (login.fnc_isLoginPage(this.v_webBrowser.Url, this.prp_url))
                {

                    login.fnc_loginWithSelenium(this.v_webBrowser);
                    this.v_webBrowser.Navigate().GoToUrl(this.prp_url);
                    Functions.sb_waitForReady(this.v_webBrowser);
                }

            }
            if (login.fnc_isLoginPage(this.v_webBrowser.Url, this.prp_url))
            {
                return false;
            }
            if (this.prp_controlLoadBy != null)
            {
                this.prp_webBrowser.FindElement(this.prp_controlLoadBy).Click();
                Functions.sb_waitForReady(this.prp_webBrowser);
            }

            return true;
        }

        public bool fnc_save(iLogin login)
        {
            return this.fnc_save(false, false, false, login);
        }

        public bool fnc_save(bool getDetail, bool download, bool startFromLastPage, iLogin login)
        {
            int i;
            for (i = 0; i <= this.prp_tables.Count - 1; i++)
            {
                if (!this.prp_tables[i].fnc_save(getDetail, download, startFromLastPage, login))
                    return false;
            }
            return true;
        }

        public void sb_back()
        {
            if (this.prp_controlBackBy != null)
            {

                this.prp_webBrowser.FindElement(this.prp_controlBackBy).Click();
                Functions.sb_waitForReady(this.prp_webBrowser);
            }
            else
            {
                this.prp_webBrowser.Navigate().Back();
            }
        }
    }
}
