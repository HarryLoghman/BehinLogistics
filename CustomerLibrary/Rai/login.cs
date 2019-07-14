using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CustomerLibrary.Rai
{
    public class login
    {
        public string v_url = "http://customers.rai.ir/Main/check.asp";
        public string v_userID = "guest";
        public string v_pwd = "123";
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
                postData = "txtUserName=" + userId + "&txtPassword=" + pwd;
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
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream(),Encoding.UTF8))
                    {
                        result = rd.ReadToEnd();
                    }

                    webResponse.Close();
                    if (webResponse.ResponseUri != null)
                    {
                        if (!string.IsNullOrEmpty(webResponse.ResponseUri.AbsolutePath)
                            && webResponse.ResponseUri.AbsolutePath.EndsWith("/Default.asp")
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


        public static bool fnc_isLoginPage(HttpWebResponse webResponse, string desiredTargetUrl)
        {
            if (webResponse == null) throw new ArgumentException("webResponse is empty or null", "webResponse");
            if (webResponse.ResponseUri == null || string.IsNullOrEmpty(webResponse.ResponseUri.AbsoluteUri))
                throw new ArgumentException("webResponse.ResponseUri is empty or null", "webResponse");
            return fnc_isLoginPage(webResponse.ResponseUri, desiredTargetUrl);

        }
        public static bool fnc_isLoginPage(Uri uri, string desiredTargetUrl)
        {
            bool meetDesiredTargetUrl;
            return fnc_isLoginPage(uri.AbsoluteUri, desiredTargetUrl, out meetDesiredTargetUrl);
        }

        public static bool fnc_isLoginPage(string url, string desiredTargetUrl, out bool meetDesiredTargetUrl)
        {
            meetDesiredTargetUrl = false;
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("url is empty or null", "url");
            }
            var urlParts = url.Split('/');
            if (urlParts.Length == 0) throw new Exception("Not correct format of url " + url);

            string pageName = urlParts[urlParts.Length - 1];
            string[] pageAndQuery = pageName.Split('?');
            if (pageAndQuery[0].ToLower() == "main.asp") return true;
            return false;
        }
    }
}
