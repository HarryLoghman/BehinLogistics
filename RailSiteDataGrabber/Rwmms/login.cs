using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace RailSiteDataGrabber.rwmms
{
    class login
    {
        public string v_url = "http://rwmms.rai.ir";
        public string v_userID = "Railpardaz";
        public string v_pwd = "@R12345r";
        public static CookieCollection v_cookieCollection;
        public bool fnc_login()
        {
            return this.fnc_login(this.v_userID, this.v_pwd);
        }

        
        public bool fnc_login(string userId, string pwd)
        {

            v_cookieCollection = null;
            try
            {
                Uri uri;
                uri = new Uri(this.v_url);


                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
                webRequest.CookieContainer = new CookieContainer();
                //webRequest.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                webRequest.Proxy = null;
                webRequest.Method = "POST";

                webRequest.Headers.Add("Cache-Control", "no-cache");
                //webRequest.Headers.Add("Cookie", "ASPSESSIONIDQSSRBAAC=ONMPBONDBPENAHJBHHADAHBA;ASPSESSIONIDSQQSBBBC=GDKPBPLADCGKADGPMNKGOFGD; ASPSESSIONIDQSTRBBBC=LPHJCGMAHCAANDMJNOCCENBM");
                webRequest.ContentType = "application/x-www-form-urlencoded";

                string postData;
                postData = "__VIEWSTATE=0a2AShnhIk1xH0oar9kXj9KAU65W47VS3i81zPJA%2FszvsZe2X5xdV3eRUxOliriGejw0F7F9aHdtUw9G8zqEXvjwuaq%2BiCFQeU5vdyayRsRjcdYheLfswfkp5zzSrECQo%2FNaY9JlmOtA5qkdyS8oystNvZAsyqq1E%2B23oz%2B5Vao%3D&__VIEWSTATEGENERATOR=CA0B0334&__VIEWSTATEENCRYPTED=&__EVENTVALIDATION=OzDrvIKa7l7Ff%2FZWsaK1TalQIZNMsb%2FW6LbCnFW2VHS20YZO9nD9lTjFprH5bp1nSq5lmmgMSDmZyKGXQg2yHmSImAX3C6nZSFaRg5gvDb3VysFp%2FHhHNPa4MyY2JduhXevLZ%2BH8IP3mYWMazSrB1a6jSfUtKzgXXTxNsb4NzQgwdpWE%2BweAOF7SurJXs4eB&txtUID=" + WebUtility.HtmlEncode(this.v_userID) + "&txtPassword=" + WebUtility.HtmlEncode(this.v_pwd) + "&cmdLogin=%D9%88%D8%B1%D9%88%D8%AF";
                byte[] formData = Encoding.ASCII.GetBytes(postData);

                webRequest.ContentLength = formData.Length;
                Stream streamRequest = webRequest.GetRequestStream();
                streamRequest.Write(formData, 0, formData.Length);
                streamRequest.Flush();
                streamRequest.Close();

                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                if (webResponse != null)
                {
                    string result;
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        result = rd.ReadToEnd();
                    }

                    webResponse.Close();
                    if (webResponse.ResponseUri != null)
                    {
                        if (!string.IsNullOrEmpty(webResponse.ResponseUri.AbsolutePath)
                            && webResponse.ResponseUri.AbsolutePath.EndsWith("/index.aspx")
                            && webRequest.CookieContainer != null)
                        {
                            v_cookieCollection = webRequest.CookieContainer.GetCookies(uri);
                            if (v_cookieCollection != null)
                                return true;
                        }
                    }

                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void fnc_logoutWithSelenium(IWebDriver webBrowser)
        {
            webBrowser.Navigate().GoToUrl("http://rwmms.rai.ir/Default.aspx?log=off");
        }
        public bool fnc_loginWithSelenium(IWebDriver webBrowser)
        {
            return this.fnc_loginWithSelenium(webBrowser, this.v_userID, this.v_pwd);
        }

        public bool fnc_loginWithSelenium(IWebDriver webBrowser, string userId, string pwd)
        {
            try
            {

                webBrowser.Navigate().GoToUrl("http://rwmms.rai.ir/login.aspx");
                var txtUID = webBrowser.FindElement(By.Id("ContentPlaceHolder1_txtUID"));
                var txtPassword = webBrowser.FindElement(By.Id("ContentPlaceHolder1_txtPassword"));
                var btnLogin = webBrowser.FindElement(By.Id("ContentPlaceHolder1_cmdLogin"));

                txtUID.SendKeys(userId);
                txtPassword.SendKeys(pwd);

                btnLogin.Click();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("Cannot login!!!!");
            }
            return false;
        }

        public static bool fnc_isLoginPage(HttpWebResponse webResponse, string desiredTargetUrl)
        {
            if (webResponse == null) throw new ArgumentException("webResponse is empty or null", "webResponse");
            if (webResponse.ResponseUri == null || string.IsNullOrEmpty(webResponse.ResponseUri.AbsoluteUri))
                throw new ArgumentException("webResponse.ResponseUri is empty or null", "webResponse");
            return fnc_isLoginPage(webResponse.ResponseUri, desiredTargetUrl);

        }
        public static bool fnc_isLoginPage(Uri uri, string desiredTargetUrl)
        {
            return fnc_isLoginPage(uri.AbsoluteUri, desiredTargetUrl);
        }

        public static bool fnc_isLoginPage(string url, string desiredTargetUrl)
        {
            bool meetDesiredTargetUrl;
            return fnc_isLoginPage(url, desiredTargetUrl, out meetDesiredTargetUrl);
        }
        public static bool fnc_isLoginPage(string url, string desiredTargetUrl, out bool meetDesiredTargetUrl)
        {
            string[] loginPages = new string[] { "login.aspx", "default.aspx" , "data:,"/*, "index.aspx"*/ };
            meetDesiredTargetUrl = false;
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("url is empty or null", "url");
            }
            var urlParts = url.Split('/');
            if (urlParts.Length == 0) throw new Exception("Not correct format of url " + url);

            string pageName = urlParts[urlParts.Length - 1];
            string[] pageAndQuery = pageName.Split('?');
            string currentPageName = pageAndQuery[0].ToLower();
            if (loginPages.Any(o => o.ToLower() == currentPageName)) return true;
            //if (pageAndQuery[0].ToLower() == "main.asp") return true;
            return false;
        }
    }
}
