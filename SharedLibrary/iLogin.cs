using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public interface iLogin
    {
        
        void fnc_logoutWithSelenium(IWebDriver webBrowser);
        bool fnc_loginWithSelenium(IWebDriver webBrowser);
        bool fnc_loginWithSelenium(IWebDriver webBrowser, string userId, string pwd);
        bool fnc_isLoginPage(HttpWebResponse webResponse, string desiredTargetUrl);
        bool fnc_isLoginPage(Uri uri, string desiredTargetUrl);
        bool fnc_isLoginPage(string url, string desiredTargetUrl);
        bool fnc_isLoginPage(string url, string desiredTargetUrl, out bool meetDesiredTargetUrl);

    }
}
