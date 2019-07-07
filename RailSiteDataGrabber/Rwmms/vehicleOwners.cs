using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace RailSiteDataGrabber.rwmms
{
    class vehicleOwners
    {
        string v_url = "http://rwmms.rai.ir/VehicleOwnerInfoList.aspx";
        vehicleOwnersDataTable v_dt;
        FirefoxDriverService v_webBrowserService;
        FirefoxOptions v_webBrowserOptions;
        FirefoxDriver v_webBrowser;
        public vehicleOwners()
        {
            this.v_dt = new vehicleOwnersDataTable();
            this.v_webBrowserService = FirefoxDriverService.CreateDefaultService();
            this.v_webBrowserService.HideCommandPromptWindow = true;

            this.v_webBrowserOptions = new FirefoxOptions();
            this.v_webBrowserOptions.AddArgument("--headless");
            this.v_webBrowser = new FirefoxDriver(this.v_webBrowserService, this.v_webBrowserOptions);
        }

        public void sb_readAndSaveToDB(bool getOwners, bool getIntroductionFiles, int cycleNumber)
        {

            try
            {
                int i;
                login lg = new login();
                start:
                if (login.v_cookieCollection == null)
                {

                    if (!lg.fnc_login())
                    {
                        //cannot login
                        return;
                    }
                }
                Uri uri;
                Model.rwmmsVehicleOwner entry_company;
                uri = new Uri(this.v_url);
                string postData;
                string introductionFilePath;

                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(uri);

                //webRequest.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                webRequest.Proxy = null;
                webRequest.Method = "POST";

                //webRequest.Headers.Add("Cache-Control", "no-cache");
                webRequest.ContentType = "application/x-www-form-urlencoded";

                webRequest.CookieContainer = new CookieContainer();
                foreach (System.Net.Cookie cookie in login.v_cookieCollection)
                {
                    webRequest.CookieContainer.Add(cookie);
                }

                postData = "__EVENTTARGET="
                    + "&__EVENTARGUMENT="
                    + "&__VIEWSTATE=19gB2KPXunfoY1KhdzKAuxbd%2FOoFmZ3EhsltmoK2s2VkBDy6lwwZr7rC25jXm%2BQIJEpBZnx2gjOlf8C0mpGh%2FkEaSfPTf9k%2BqaLrna5Oyeq%2BPbp9AU6oxxViHFeoRwvzEz%2FawXFACHm5L3aqOHniXVtwr%2FH5EgML96nGzzM6ycEhi56B%2B56Lt2045a%2Fy%2F0fOAXo3c2qy%2F6Wb4tWlH3WHViqY2XkJT12CIxTRoupHoPcVNeYbLMPR1Y99aYAbNkYtCCiLd263Z725l8153%2FwQDh%2FUWvjAaBNxS7%2B%2BhI9TKl0ZrVavHXtuNi6HKNeAJ8j0Q6%2BwQG60HDkYtTdaZbmNeGwhHgNf7XZqnwNuUsJFFd7nYZeKQWTSL%2FvIQXrK2n2X1eX0S1EjWzxUR8VcRu7IrjsIJYcEVYvpSoDfAKSO0qpL6cuCFXcP5rGqxKJNN8a0oJomJ%2FlQnaOAJPl%2BMDV0q%2BRltsfZuYbrOXeT%2FZv78ssfhb8tNygoaEKeB7h9Tor8Qj1eJmN8xRQ2Sg%2B0cIrl8el5TLWf1S6HgdDtGRE1Cy%2BP83IWK4jZY5aJwbSdpfWpkbiVQtaT%2Bvb1jCrC0hEkGJ1S9LBB9Y8S6rpelUND9cGGd4fwQygu1%2FRI6oqqJxQ25NwVHXxuOwQKCGTCFuHzpHhDb78RXUm9Wy0ZY3%2BlfuxgRgUHY%2FoJkh6fIQu0haLvKJ87Oqb7MLGHb4g8hFC3fpR3GWXX80b4cETpqTbEb60iHNXnml8KDVZGn%2FYwO5s1RPMBQmLvt2VYd%2BKS3BZwq6%2F65UBeJegsrc7XyR%2FcZt3IiJwRRdzU7Op4UmvATturk0gw4HcBozcUIybhUkcYdfXP8w9GWiW5boHIiGiDEU00RJZYRgDfL%2BTsWvgoGs7G2EyvDR6A5T8UOrk2So6t2yBXNwn18XxsDncNC5H0WwHsp85RwpEhjuvY%2BbmDDWIM7lnMd8H3kiTvJ9aYUOF4LrW1rfETQIf0ai%2FnMararHJNl63QzEsQ7UgUUUoxwnMYwFfwDBp%2BDFmar2C8Tp5xKvKiooRptzxiI4CKtSascMBtXX6TAAhUQMXdpwBKYgNrNXoeNXpFToHZLvIUWobMdai2NVKDFa1xo6Rz04BryKFf2XJyLizL9PAZMupaUi18dYkFLhhVcIOzweohi8n4Dwg68Fj4PRq%2F5UmxEK4KEZknRBTccPRPmvdYuxUZP1NLogNOCaN79N29o4vKVYzcFPWIVjYevyCHUH8UKDqpwKE2sQ8HIOrAa%2B03PeBK0AJfrxKhJwL6%2FE77GjJjQ08i6s%2FerP5p3VhBivP2Ml1EsLxAQT4VXbj71yh900EvTciIwLXpA2ZzVjdz0XubdnPYkgA308qhjLI0jEpyoXLDr7JDxZh8bTSOhCBzpQoBGnl3VgUgOq7pvRjLzgZphTBDkHNEI2jMghA74mR0piY0SDbc5MEu2exQIGUDY%2FDWDufi18Z%2Ben7ELs26pq19G4ZHQR2Orl9YdAy82J0kmuOTlkwSIEZ%2Fw70tLyxRBIFrfme2ug4kQtJLLVDT5JVcFqMxOOaVEv91rf1pd65sVvbw4ltG54qGHle3crRu2oYbnVi%2FQJsbj1IpY5jau1haD%2FT38sv5HI02UH3L2YjNggEu6FdbfqaD%2Fic1ZGQkkl44pmvmRb5MZPceuzig50zyXrUDHzGWxnpPvxPA0Mra%2BW1vluuQ4cxMAx81Hy9U8DyCKj%2BxCnSOqdBvJsXWVg5O7ouorl8Il06%2BziosiFnWxgnopPXs7pheMHIyzrKkozoE5QVpvy2WrmwWAT6xBcBL6vB%2Fhc2GvYPALnZlP23OCS1tTQ5j3RgZ6Ay7EZBmoatx6HCI3Hq8Sstlhj2xUW%2FLMiTMzHKmGukFI7vuAzoX4bx7PxvJ%2Bmd%2BOrihMYQeDuc8QMhvIWOWIRnqSbITDA4%2FTfuaj3iCz6CmB3GVLioecKpxWWs%2BAw7oNjV4LgKdzzWMxTGxsM7PVQUFLYXKelX5t2Dt%2BB%2Bxas4mrETA%2Bgn41p4ApHb1zqDSkSlZFRIJfoq%2F8nIzEqLak6bMkmo6dwtZqi2%2B2WnZ%2FDNy4tbC48TTqGbUofARhFLjXL2DJDmMEnxkWTKwCoVp%2B8DAeZ5iU7W05q%2FncFcD3CB8Za3diAzrHDX6F8s7P91Rk0f8amaX8tshr0yqLhBxqLNMmO4c6kCUyqmX6NBsxW965jB6utlWYMw0rwI%2Fnv8lO%2Bn%2BWeeX1oF%2BtttY8Epi22lZQVGRTmvbOAyE9le%2FlhMRkb%2FzhzM8BsCLl2L3gqJb9sQ%2BKdf4sSPO3qvyT78Y8YSCIC1dSMlfTz8Dr638KmooKbsku8L4KDX7rlpULEQoWbx0fPC6QUW%2F4ii6L06vGSMa5J%2BrLqoN6B7H4A9VyYvPQXIy%2FHAL5bPGmRg7C0ZcKz4MRqM0zKHidnFhU%2FJrKtKfHZwTDY6tn4ZnlFgFxkvcfse4GJYLgCdCrpPtjXsQn41WREB8O%2BhEYNro6%2BQj%2BcUh5lnE5FYzd%2BRq7TtB6LqB8Y2UcgPOX459tFC2%2FQk%2FbACltLMYARW3%2BZIbvaXbZngD4ImtX8yiR0O9%2FMeaQRArAZAPc%2F%2BtdVSFttX9mbfYfamDOZVoulAaTRjOwlvkT%2F6lMwIDfmvRDecUQ8fb735UWRCnhztIWMsS8mfshF2DIoKyV5YW4gh3z%2BKBKZ0dPfmmAypn5yhOC1HDOde87nzx%2F0hoffUXp8Yq8blDirq3FZ31B%2F8F%2FjoU1fiiX67BvF%2BavmZnglik0QklTJjhgIghqGwrFd9gnImgqwPX3SQxI29dl%2BjwoSVll%2Bzeppy3%2BdY4VUXqW1%2Bxj0ca1ri4VlsixCBKGK8c8FPIaxwp%2BahnQZyoYPay3%2B7JeaHq6gOiHBzsxg%2FJ4Tl6aNEz7YP%2FUR8umrNoQpFadUi6vFGezztVFHIj%2FRamw6NhAlLDSldxIpTE3ZK9aDitSd0I0lRtUW4DbZedloUyT%2FKizeKGokuvUHbKNgZ3CXBTWWQfwLcSzIK3UkvW%2FZbolC99TwfBdSGls%2FyWjwt%2BDwxtHGawgG0GNemZD%2BMpbueY8bBaTHO5lBYATfjtHZFWJ%2FFAaDbw9jgsX4btc40OEFDMLEJ0rElsc6TqLH5tPm%2BHV9qZYs2SihK7NxhW1SJqeS6qV31wW%2FakTBptOO5IjQwYdmP2uBaB5WdyqQCovkOyq1mBO0SwVCduPXdFtKJ4pP3IpNEqoclEsBFFkWqYoKOTQIJ96R1Yx0M2XkEnj6h%2B7r9pRf%2BQVLhMmR06XtkwEavFse479ltuHAu2QnGcUBGtnsjL2wi%2FtBoP%2FpjlGJ1ECrd2CR8FBQ2IrWh45jFq2hmbw7U1NBna1QqFUDoBtgq25Nb8T0WKl62Uezy%2FDKXGXb6n6Zb5PYFNp4egiYfhfnKaVBFK8gjxwyATqDrQDqfKA2R8gsfSTYxtEYYhqRz5dSECoVxxZqF5SoHOri%2FqqsQZipzsmDkilgWnKziza7KLzz3dZSTvsUStDolRSBFrBq0OmjynsRUKYJSZughu5o1sD833AaeAlzqFcaH%2BD73t%2B9p%2BBsZsCqsRNyPZu%2F62uNFCqFpHucUueez%2BMMMHQqn%2FMXJwgWPVqa8Nakssj5e5Igk%2BEC%2B6AVBDRZm5AV9C6jKtfMar6gTPeGF6jFVIRSHyPNiglUV%2FgGGhZjmysZdWjvY38XbnWXMtq4qjSqjXL81qaK1on0GqYrfGmGZ9k7C9ZAGCuWfTYPFUZlXU1sVoR5K%2Fz7g7eu9r0g9062wqSplsqux%2FEEWPlvzzdeb1Utnpt1fym08E1%2BotZJ%2FDnPHR6FI0Jvcoica8BHB6x%2BGdhp%2F6Od40NGuBs8GkAXM%2FSg5HignY3KZlMKn6zn5f1Licolmx2sKEMd7JQuLHY1rGQjZq4i0n4XDx5A9ePfWm7kzScVQbAP5fqMStONjocJssnFlEHUkUb%2Bz6l5zq0RXp%2BdsOBr4dOcxR2B1X6X1UZTVBt96uT6katD70HTq9NZzdjDgvf%2BCnSDd5kIHPIGKPv9XLRQZkMYLzx8PyYx9UCZ0ehAvhb6qpfumsIPNLUZDzx3X5kiQhGB6%2BTyn0LqeKwUJptaQmQr2J9UOjjCQhQNVd7IzCQ04pO6CGsXMQquY6rp7bu8Bji7STHpnXXmNQVgHE03knOfHKdstSvIviDplMoODydFZNvUWssripGKEHz5iQMY3Lb%2F5h4u%2BStO2xHgPo63mOEQvKaitYzT7s2clEb%2FsFrh0%2FiyojKGuDyrh4Mw1tL6Rok%2BysF68Yj8SyU3Z%2F8Eu1z55iV4k08Hri5RU5PgqtmZg0HV8%2Bulislnd2mGZrlJ6BTAszhnFQRBXCNlZCLY9HKqBdabUAxRsELjO7W%2F2UDg0Ja2%2BSklAOZb5Oy6kSrW24PICP1PLHrtdq6wxmRVAAwhWnp7ZSHTkPW1fEJ3%2FY9Bzidn8%2FQ1Cb%2Baj4BV%2FF6OLZ%2FK3p7qngg6zuJ3OhcrX5P73vJ33QD0bgcLo%2FZcV5CN6AexEsLbaO%2FXQVLZGTnQPwmtTNRD1mbqp60ipL%2Fke%2FUP2sX1eTMR1dzRNHlm4R%2Bh0YESHqSucqMECXIuwpKiaaH5kcvYriqIQXFXf7IuiWG0f2LuoJmx9jRNUR33gqnaHha749ki6jCJ4shYGiZ0Ell6wQctVCM0qPRIrCy8zdBhkEdKzKBaEuK88yQQ%3D%3D"
                    + "&__VIEWSTATEGENERATOR=FB6C6F2C"
                    + "&__VIEWSTATEENCRYPTED="
                    + "&__EVENTVALIDATION=CKs99QiiMJ2E9XqMrGTdqJtDalYRrpoJ1Ty8fuaFf8MEYdyFhb%2B2UnhIQ%2FeF4I7Xork8TUH7TPKn7LxdAlD93Inzzu5FFypSQacV45UIx%2BUmf2r4SRUCYHACHx27AL4Msa%2BfUbhd6zqoR2y6unlVfAGu8Pri9%2BHV1rNG2BhpF2o4cvbKC1eVjT9PwdmfS4qqHhUyM2ukCLM3LTnpKyoEcUV1aaKde5DcdFYwA4cdDaXH8emhnCNdujHMDutHAU1esPtPuplPYDv7PoTyMZtLRgcr8Zbk7Pe6%2FpEnynfBB7eGMrz5cNGJwlW8qWpzOxQmv1OroRLQwiCGO%2FoyJijp6tZICRvXMEPyur2Kbt3mdfB%2B4DGXZulYWcy84KKOL1k1LuIXwSMD3Kr6HSyChodoReYuNsfmiM%2BRbh32rsQ3WHnBgYAQnZTKuthxBVnD9EPzNZtLe1VGgA2ol%2FYF1o8DnGuyzAejQ6%2B7HJJ4GdezJrM2FpcLNmdvZSn5m3UCMbx6tLKE1mx7MzarMM%2B7bGh3xZRAAzK9v%2BoP9SRJFlCZgiBAkaF7YJKQHaBnMnNVP1EfW8b4mLUxbjS45FY0z%2B8HiDxbWyGbiGQiUkOdR%2Fch3Pka0QsTapPPLEByjsMeYDVjGfvgCfZn77HUIUQvsQMZeec4nnSfeDoyiWGfCbLfOxktvaTO%2FKsFUILBjJ%2F6YM24bxWT4qvzxMzivCvgi6PiNEGNkej%2FTtWGTRyQUgwHYCh4XXU0ObTMx4nVHUbrF58jrVUNTXlO0ndECk7ireHEUwA9W39QzF9nDfZ8R%2Frq%2BvNVd1b92cA1JKatT6bVs7aTvPKZrHzu7zNS6alfsHBHikkM4xKFtEwLM8b45GtxtM7mhS6DUKx9lW7f%2FilGPwD%2FUl3E0r2OpC%2B5lt%2BGNPUcjcUlLA02GKS5mX2aY%2BM5BdZKmEmo%2FMxIf1w32uW5F6gMsUx5jBNXrhOfdkzjdqdmMBr10EPzlKSzrH3t1uXwyoaDJUP0eSRnRQ1fznm1GCt%2Bqxbs3HGxdd4C71%2FTlDFhrhYJDf4Y3o0iaC3rLbjHt4Qaed2Q7hzefiPY3EKKsMB449I7kC24RaitpJXNC342pXrOFMYhV%2FzO%2FT%2BB4TAQFojSxw5AtGU5G30C4HX7cKVNp%2FkLcxgHrCDYWoGhOGrRI9O%2BqhKCw5WSEbjt0r0uDU9nlQphyT3kUnjp%2FM6pLltvORVDqYmws4z8mSyHH0CfqWy3sHw2Xu8zYu0cokX8pacAN1BT6KlQ6aInwNzJ%2F%2B2BUwGpdhp2I0ZQJMn5oReVZfkdpLeqCZrY3L%2FmuMFzA0R9WMT7OS2zvpQO%2Fk5ycpHPKsK%2FxsfLUJ21V%2B8yDJkDS7%2Fnn3c8CizL03VKQ3Gbweb%2FjyluhnUg%2Bf%2Fqr%2B00ktLcMIzKZICWoaPt3KT1bGaoQO3IjbC%2Ftb1pryMwgK8uyP81SN0Ysc8baiEvwM%2FeNZlA9jwr8o4TILSN7YE6On%2BfzKH4p8fLT7NeWOtG5%2BpgORP7isVvm1RHrvXXoxIfkXn7lsvW3QdvxdCBnnYWMN%2FE3GOcLnEoirJt%2BS31cn0uRPIpApDODga1Jig2%2B8lx7GdUlucWMxmOrXFzU1pBBLn8QKYIDxgn7UgSySyyfWVNthiZokkEsWsZyONzDg%2Fr9OD563xqw9hkHuLb1ba4y4eL9XzcnguO2XAZgNteGjDB62iXHdaO8evAuHazGDGcqbdhz2H1dMen1n%2B31%2B57NXNLGSsOZoQf3aI14g3nA9%2FZ6ch2UDu2yD8vURW7HnihCH6XfzWwgz89z4W0bvdyW0AipjMV%2BbZYvHjsRXo8ewuRvvnPeTKaupWsPFJXZsZs7H2Qi92pFFkr%2BGeFDWaHnQ50Vm3KAHn2Fl7HX6Ch9sgw57SUz4jDowNyaSYZBx3vKB%2BLmy1DY%2B8iueprOqM0mbn9Zpgfh23%2BvFR3%2FBaqrDUx9xk34l4Bt7RJPvAzvmLsesA8aGrwT7npF2N0bTN4y5uOQpQWX3BpbhNJi1VuM%2F7PlN5apaEXnSQUVUrwnZKd3bw5h4eF0BLUYlli1XmK%2BdCj3MwSsWF%2Fl9YS03wCvKvU2oeWyv%2Fyd49t7BJupZWp1YyXb6OpUb9HQxeHb7xg6YNHDuiMkJYmsLQB0IZMp%2FUYM%2BoBpl2wbq1AvK4c0Q1KEaMK"
                    + "&ctl00%24ContentPlaceHolder1%24txtRegisterDate="
                    + "&ctl00%24ContentPlaceHolder1%24txtRegisterNumber="
                    + "&ctl00%24ContentPlaceHolder1%24txtNationalCode="
                    + "&ctl00%24ContentPlaceHolder1%24txtRegisterLocation="
                    + "&ctl00%24ContentPlaceHolder1%24btnSearch=%D8%AC%D8%B3%D8%AA%D8%AC%D9%88";
                byte[] formData = Encoding.ASCII.GetBytes(postData);

                webRequest.ContentLength = formData.Length;
                Stream streamRequest = webRequest.GetRequestStream();
                streamRequest.Write(formData, 0, formData.Length);
                streamRequest.Flush();
                streamRequest.Close();

                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                if (webResponse != null)
                {
                    if (webResponse.ResponseUri == null || string.IsNullOrEmpty(webResponse.ResponseUri.AbsoluteUri))
                    {
                        webResponse.Close();
                        return;
                    }
                    if (login.fnc_isLoginPage(webResponse, this.v_url))
                    {
                        //we haven't loginned yet
                        if (lg.fnc_login())
                        {
                            goto start;
                        }
                        else
                        {
                            //cannot login inform or log !!!!!
                            webResponse.Close();
                            return;
                        }
                    }
                    string result;
                    using (StreamReader rd = new StreamReader(webResponse.GetResponseStream(), Encoding.GetEncoding("utf-8")))
                    {
                        result = rd.ReadToEnd();
                    }
                    webResponse.Close();
                    if (!string.IsNullOrEmpty(result))
                    {
                        this.v_dt.Rows.Clear();
                        Functions.sb_fillDatatableWithHtmlTableId(result, "ContentPlaceHolder1_dgVehicleOwners", 1, 1, 0, 0, this.v_dt);
                        for (i = 0; i <= this.v_dt.Rows.Count - 1; i++)
                        {
                            introductionFilePath = null;

                            this.sb_saveToDB(this.v_dt, cycleNumber, introductionFilePath, out entry_company);
                            if (getIntroductionFiles && entry_company != null)
                            {
                                introductionFilePath = this.fnc_getIntroductionFile(entry_company.Id, entry_company.wCompanyName, i);
                                entry_company.wIntroductionFile = introductionFilePath;
                                using (var entityLogestic = new Model.logesticEntities())
                                {
                                    entityLogestic.Entry(entry_company).State = System.Data.Entity.EntityState.Modified;
                                    entityLogestic.SaveChanges();
                                }
                            }

                            if (getOwners && entry_company != null)
                            {
                                this.sb_readOwnerDetail(entry_company.Id, cycleNumber);
                                //this.readAndSaveOwnerDetailToDB(entry_company.Id, cycleNumber);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {

            }
        }

        private void sb_readOwnerDetail(int companyId, int cycleNumber)
        {

            login lg = new login();
            lg.fnc_loginWithSelenium(this.v_webBrowser);

            bool add;
            string tableId = "ContentPlaceHolder1_dgVehicleOwners";
            this.v_webBrowser.Navigate().GoToUrl(this.v_url);
            SharedFunctions.sb_waitForReady(this.v_webBrowser);
            var btnSearch = this.v_webBrowser.FindElement(By.Id("ContentPlaceHolder1_btnSearch"));
            if (btnSearch == null) return;
            btnSearch.Click();
            SharedFunctions.sb_waitForReady(this.v_webBrowser);
            var table = this.v_webBrowser.FindElement(By.Id(tableId));
            if (table == null) return;
            var rows = table.FindElements(By.TagName("tr"));
            if (rows == null || rows.Count < 3) return;
            var columns = rows[1].FindElements(By.TagName("td"));
            if (columns.Count >= 6)
            {
                var href = columns[6].FindElement(By.TagName("a"));
                if (href != null)
                {
                    href.Click();
                    SharedFunctions.sb_waitForReady(this.v_webBrowser);
                    table = this.v_webBrowser.FindElement(By.Id("ContentPlaceHolder1_dgVehicleOwnerDetails"));
                    if (table == null) return;
                    rows = table.FindElements(By.TagName("tr"));
                    if (rows == null || rows.Count < 3) return;
                    columns = rows[1].FindElements(By.TagName("td"));
                    if (columns.Count >= 6)
                    {
                        href = columns[6].FindElement(By.TagName("a"));
                        if (href != null)
                        {
                            href.Click();
                            //SharedFunctions.sb_waitForReady(this.v_webBrowser);
                            SharedFunctions.sb_waitForReady(this.v_webBrowser);
                            string htmlEditPage = this.v_webBrowser.PageSource;
                            string firstName = Functions.fnc_getElementValue(htmlEditPage, "ContentPlaceHolder1_txtFirstName");
                            string lastName = Functions.fnc_getElementValue(htmlEditPage, "ContentPlaceHolder1_txtLastName");
                            string fatherName = Functions.fnc_getElementValue(htmlEditPage, "ContentPlaceHolder1_txtFatherName");
                            string certNo = Functions.fnc_getElementValue(htmlEditPage, "ContentPlaceHolder1_txtIdentityNumber");
                            string registerCity = Functions.fnc_getElementValue(htmlEditPage, "ContentPlaceHolder1_txtIdentityRegisterLocation");
                            string meliNo = Functions.fnc_getElementValue(htmlEditPage, "ContentPlaceHolder1_txtNationalCode");

                            string postalCode = Functions.fnc_getElementValue(htmlEditPage, "ContentPlaceHolder1_txtAgentPostCode");
                            string email = Functions.fnc_getElementValue(htmlEditPage, "ContentPlaceHolder1_txtAgentEmail");
                            string mobileNumber = Functions.fnc_getElementValue(htmlEditPage, "ContentPlaceHolder1_txtMobileNumber");
                            string birthDate = Functions.fnc_getElementValue(htmlEditPage, "ContentPlaceHolder1_txtBirthDate");
                            string address = Functions.fnc_getElementValue(htmlEditPage, "ContentPlaceHolder1_txtAgentAddress");
                            using (var entityLogestic = new Model.logesticEntities())
                            {
                                if (!string.IsNullOrEmpty(meliNo))
                                {
                                    add = false;
                                    var entryOwnerExtraDetail = entityLogestic.rwmmsVehicleOwnersDetails.FirstOrDefault(o => o.wMeliNo == meliNo);
                                    if (entryOwnerExtraDetail == null)
                                    {
                                        entryOwnerExtraDetail = new Model.rwmmsVehicleOwnersDetail();
                                        add = true;
                                    }
                                    
                                    entryOwnerExtraDetail.CycleNumber = cycleNumber;
                                    entryOwnerExtraDetail.FetchTime = DateTime.Now;
                                    entryOwnerExtraDetail.Source = this.v_url;
                                    entryOwnerExtraDetail.vehicleOwnerId = companyId;
                                    entryOwnerExtraDetail.wAddress = (Functions.IsNull(address) ? null : address);
                                    entryOwnerExtraDetail.wBirthDate = (Functions.IsNull(birthDate) ? null : birthDate);
                                    entryOwnerExtraDetail.wCertNo = (Functions.IsNull(certNo) ? null : certNo);
                                    entryOwnerExtraDetail.wEmail = (Functions.IsNull(email) ? null : email);
                                    entryOwnerExtraDetail.wFatherName = (Functions.IsNull(fatherName) ? null : fatherName);
                                    entryOwnerExtraDetail.wFName = (Functions.IsNull(firstName) ? null : firstName);
                                    entryOwnerExtraDetail.wLName = (Functions.IsNull(lastName) ? null : lastName);
                                    entryOwnerExtraDetail.wMeliNo = (Functions.IsNull(meliNo) ? null : meliNo);
                                    entryOwnerExtraDetail.wMobileNumber = (Functions.IsNull(mobileNumber) ? null : mobileNumber);
                                    entryOwnerExtraDetail.wPostalCode = (Functions.IsNull(postalCode) ? null : postalCode);
                                    entryOwnerExtraDetail.wRegisterCity = (Functions.IsNull(registerCity) ? null : registerCity);
                                    if (add)
                                        entityLogestic.rwmmsVehicleOwnersDetails.Add(entryOwnerExtraDetail);
                                    else entityLogestic.Entry(entryOwnerExtraDetail).State = System.Data.Entity.EntityState.Modified;
                                    entityLogestic.SaveChanges();

                                }
                            }
                        }

                    }
                }
            }

        }
        private string fnc_getIntroductionFile(int companyId, string companyName, int rowIndex)
        {
            try
            {
                login lg = new login();
                start:
                if (login.v_cookieCollection == null)
                {

                    if (!lg.fnc_login())
                    {
                        //cannot login
                        return null;
                    }
                }
                Uri uri;
                uri = new Uri(this.v_url);
                string postData;

                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(uri);

                //webRequest.Headers.Add("Accept-Language", "en-US,en;q=0.8");
                webRequest.Proxy = null;
                webRequest.Method = "POST";

                //webRequest.Headers.Add("Cache-Control", "no-cache");
                webRequest.ContentType = "application/x-www-form-urlencoded";

                webRequest.CookieContainer = new CookieContainer();
                foreach (System.Net.Cookie cookie in login.v_cookieCollection)
                {
                    webRequest.CookieContainer.Add(cookie);
                }

                string fileName = "intro_" + companyId.ToString()
                            + "_" + (string.IsNullOrEmpty(companyName) ? "" : companyName) + ".pdf";
                string filePath = Path.GetDirectoryName(Application.ExecutablePath)
                    + "\\Files\\rwmms\\introductionFiles\\" + fileName;

                postData = "__EVENTTARGET=ctl00$ContentPlaceHolder1$dgVehicleOwners$ctl03$ctl" + ((rowIndex < 10 ? "0" : "") + rowIndex.ToString())
                    + "&__EVENTARGUMENT="
                    + "&__VIEWSTATE=19gB2KPXunfoY1KhdzKAuxbd%2FOoFmZ3EhsltmoK2s2VkBDy6lwwZr7rC25jXm%2BQIJEpBZnx2gjOlf8C0mpGh%2FkEaSfPTf9k%2BqaLrna5Oyeq%2BPbp9AU6oxxViHFeoRwvzEz%2FawXFACHm5L3aqOHniXVtwr%2FH5EgML96nGzzM6ycEhi56B%2B56Lt2045a%2Fy%2F0fOAXo3c2qy%2F6Wb4tWlH3WHViqY2XkJT12CIxTRoupHoPcVNeYbLMPR1Y99aYAbNkYtCCiLd263Z725l8153%2FwQDh%2FUWvjAaBNxS7%2B%2BhI9TKl0ZrVavHXtuNi6HKNeAJ8j0Q6%2BwQG60HDkYtTdaZbmNeGwhHgNf7XZqnwNuUsJFFd7nYZeKQWTSL%2FvIQXrK2n2X1eX0S1EjWzxUR8VcRu7IrjsIJYcEVYvpSoDfAKSO0qpL6cuCFXcP5rGqxKJNN8a0oJomJ%2FlQnaOAJPl%2BMDV0q%2BRltsfZuYbrOXeT%2FZv78ssfhb8tNygoaEKeB7h9Tor8Qj1eJmN8xRQ2Sg%2B0cIrl8el5TLWf1S6HgdDtGRE1Cy%2BP83IWK4jZY5aJwbSdpfWpkbiVQtaT%2Bvb1jCrC0hEkGJ1S9LBB9Y8S6rpelUND9cGGd4fwQygu1%2FRI6oqqJxQ25NwVHXxuOwQKCGTCFuHzpHhDb78RXUm9Wy0ZY3%2BlfuxgRgUHY%2FoJkh6fIQu0haLvKJ87Oqb7MLGHb4g8hFC3fpR3GWXX80b4cETpqTbEb60iHNXnml8KDVZGn%2FYwO5s1RPMBQmLvt2VYd%2BKS3BZwq6%2F65UBeJegsrc7XyR%2FcZt3IiJwRRdzU7Op4UmvATturk0gw4HcBozcUIybhUkcYdfXP8w9GWiW5boHIiGiDEU00RJZYRgDfL%2BTsWvgoGs7G2EyvDR6A5T8UOrk2So6t2yBXNwn18XxsDncNC5H0WwHsp85RwpEhjuvY%2BbmDDWIM7lnMd8H3kiTvJ9aYUOF4LrW1rfETQIf0ai%2FnMararHJNl63QzEsQ7UgUUUoxwnMYwFfwDBp%2BDFmar2C8Tp5xKvKiooRptzxiI4CKtSascMBtXX6TAAhUQMXdpwBKYgNrNXoeNXpFToHZLvIUWobMdai2NVKDFa1xo6Rz04BryKFf2XJyLizL9PAZMupaUi18dYkFLhhVcIOzweohi8n4Dwg68Fj4PRq%2F5UmxEK4KEZknRBTccPRPmvdYuxUZP1NLogNOCaN79N29o4vKVYzcFPWIVjYevyCHUH8UKDqpwKE2sQ8HIOrAa%2B03PeBK0AJfrxKhJwL6%2FE77GjJjQ08i6s%2FerP5p3VhBivP2Ml1EsLxAQT4VXbj71yh900EvTciIwLXpA2ZzVjdz0XubdnPYkgA308qhjLI0jEpyoXLDr7JDxZh8bTSOhCBzpQoBGnl3VgUgOq7pvRjLzgZphTBDkHNEI2jMghA74mR0piY0SDbc5MEu2exQIGUDY%2FDWDufi18Z%2Ben7ELs26pq19G4ZHQR2Orl9YdAy82J0kmuOTlkwSIEZ%2Fw70tLyxRBIFrfme2ug4kQtJLLVDT5JVcFqMxOOaVEv91rf1pd65sVvbw4ltG54qGHle3crRu2oYbnVi%2FQJsbj1IpY5jau1haD%2FT38sv5HI02UH3L2YjNggEu6FdbfqaD%2Fic1ZGQkkl44pmvmRb5MZPceuzig50zyXrUDHzGWxnpPvxPA0Mra%2BW1vluuQ4cxMAx81Hy9U8DyCKj%2BxCnSOqdBvJsXWVg5O7ouorl8Il06%2BziosiFnWxgnopPXs7pheMHIyzrKkozoE5QVpvy2WrmwWAT6xBcBL6vB%2Fhc2GvYPALnZlP23OCS1tTQ5j3RgZ6Ay7EZBmoatx6HCI3Hq8Sstlhj2xUW%2FLMiTMzHKmGukFI7vuAzoX4bx7PxvJ%2Bmd%2BOrihMYQeDuc8QMhvIWOWIRnqSbITDA4%2FTfuaj3iCz6CmB3GVLioecKpxWWs%2BAw7oNjV4LgKdzzWMxTGxsM7PVQUFLYXKelX5t2Dt%2BB%2Bxas4mrETA%2Bgn41p4ApHb1zqDSkSlZFRIJfoq%2F8nIzEqLak6bMkmo6dwtZqi2%2B2WnZ%2FDNy4tbC48TTqGbUofARhFLjXL2DJDmMEnxkWTKwCoVp%2B8DAeZ5iU7W05q%2FncFcD3CB8Za3diAzrHDX6F8s7P91Rk0f8amaX8tshr0yqLhBxqLNMmO4c6kCUyqmX6NBsxW965jB6utlWYMw0rwI%2Fnv8lO%2Bn%2BWeeX1oF%2BtttY8Epi22lZQVGRTmvbOAyE9le%2FlhMRkb%2FzhzM8BsCLl2L3gqJb9sQ%2BKdf4sSPO3qvyT78Y8YSCIC1dSMlfTz8Dr638KmooKbsku8L4KDX7rlpULEQoWbx0fPC6QUW%2F4ii6L06vGSMa5J%2BrLqoN6B7H4A9VyYvPQXIy%2FHAL5bPGmRg7C0ZcKz4MRqM0zKHidnFhU%2FJrKtKfHZwTDY6tn4ZnlFgFxkvcfse4GJYLgCdCrpPtjXsQn41WREB8O%2BhEYNro6%2BQj%2BcUh5lnE5FYzd%2BRq7TtB6LqB8Y2UcgPOX459tFC2%2FQk%2FbACltLMYARW3%2BZIbvaXbZngD4ImtX8yiR0O9%2FMeaQRArAZAPc%2F%2BtdVSFttX9mbfYfamDOZVoulAaTRjOwlvkT%2F6lMwIDfmvRDecUQ8fb735UWRCnhztIWMsS8mfshF2DIoKyV5YW4gh3z%2BKBKZ0dPfmmAypn5yhOC1HDOde87nzx%2F0hoffUXp8Yq8blDirq3FZ31B%2F8F%2FjoU1fiiX67BvF%2BavmZnglik0QklTJjhgIghqGwrFd9gnImgqwPX3SQxI29dl%2BjwoSVll%2Bzeppy3%2BdY4VUXqW1%2Bxj0ca1ri4VlsixCBKGK8c8FPIaxwp%2BahnQZyoYPay3%2B7JeaHq6gOiHBzsxg%2FJ4Tl6aNEz7YP%2FUR8umrNoQpFadUi6vFGezztVFHIj%2FRamw6NhAlLDSldxIpTE3ZK9aDitSd0I0lRtUW4DbZedloUyT%2FKizeKGokuvUHbKNgZ3CXBTWWQfwLcSzIK3UkvW%2FZbolC99TwfBdSGls%2FyWjwt%2BDwxtHGawgG0GNemZD%2BMpbueY8bBaTHO5lBYATfjtHZFWJ%2FFAaDbw9jgsX4btc40OEFDMLEJ0rElsc6TqLH5tPm%2BHV9qZYs2SihK7NxhW1SJqeS6qV31wW%2FakTBptOO5IjQwYdmP2uBaB5WdyqQCovkOyq1mBO0SwVCduPXdFtKJ4pP3IpNEqoclEsBFFkWqYoKOTQIJ96R1Yx0M2XkEnj6h%2B7r9pRf%2BQVLhMmR06XtkwEavFse479ltuHAu2QnGcUBGtnsjL2wi%2FtBoP%2FpjlGJ1ECrd2CR8FBQ2IrWh45jFq2hmbw7U1NBna1QqFUDoBtgq25Nb8T0WKl62Uezy%2FDKXGXb6n6Zb5PYFNp4egiYfhfnKaVBFK8gjxwyATqDrQDqfKA2R8gsfSTYxtEYYhqRz5dSECoVxxZqF5SoHOri%2FqqsQZipzsmDkilgWnKziza7KLzz3dZSTvsUStDolRSBFrBq0OmjynsRUKYJSZughu5o1sD833AaeAlzqFcaH%2BD73t%2B9p%2BBsZsCqsRNyPZu%2F62uNFCqFpHucUueez%2BMMMHQqn%2FMXJwgWPVqa8Nakssj5e5Igk%2BEC%2B6AVBDRZm5AV9C6jKtfMar6gTPeGF6jFVIRSHyPNiglUV%2FgGGhZjmysZdWjvY38XbnWXMtq4qjSqjXL81qaK1on0GqYrfGmGZ9k7C9ZAGCuWfTYPFUZlXU1sVoR5K%2Fz7g7eu9r0g9062wqSplsqux%2FEEWPlvzzdeb1Utnpt1fym08E1%2BotZJ%2FDnPHR6FI0Jvcoica8BHB6x%2BGdhp%2F6Od40NGuBs8GkAXM%2FSg5HignY3KZlMKn6zn5f1Licolmx2sKEMd7JQuLHY1rGQjZq4i0n4XDx5A9ePfWm7kzScVQbAP5fqMStONjocJssnFlEHUkUb%2Bz6l5zq0RXp%2BdsOBr4dOcxR2B1X6X1UZTVBt96uT6katD70HTq9NZzdjDgvf%2BCnSDd5kIHPIGKPv9XLRQZkMYLzx8PyYx9UCZ0ehAvhb6qpfumsIPNLUZDzx3X5kiQhGB6%2BTyn0LqeKwUJptaQmQr2J9UOjjCQhQNVd7IzCQ04pO6CGsXMQquY6rp7bu8Bji7STHpnXXmNQVgHE03knOfHKdstSvIviDplMoODydFZNvUWssripGKEHz5iQMY3Lb%2F5h4u%2BStO2xHgPo63mOEQvKaitYzT7s2clEb%2FsFrh0%2FiyojKGuDyrh4Mw1tL6Rok%2BysF68Yj8SyU3Z%2F8Eu1z55iV4k08Hri5RU5PgqtmZg0HV8%2Bulislnd2mGZrlJ6BTAszhnFQRBXCNlZCLY9HKqBdabUAxRsELjO7W%2F2UDg0Ja2%2BSklAOZb5Oy6kSrW24PICP1PLHrtdq6wxmRVAAwhWnp7ZSHTkPW1fEJ3%2FY9Bzidn8%2FQ1Cb%2Baj4BV%2FF6OLZ%2FK3p7qngg6zuJ3OhcrX5P73vJ33QD0bgcLo%2FZcV5CN6AexEsLbaO%2FXQVLZGTnQPwmtTNRD1mbqp60ipL%2Fke%2FUP2sX1eTMR1dzRNHlm4R%2Bh0YESHqSucqMECXIuwpKiaaH5kcvYriqIQXFXf7IuiWG0f2LuoJmx9jRNUR33gqnaHha749ki6jCJ4shYGiZ0Ell6wQctVCM0qPRIrCy8zdBhkEdKzKBaEuK88yQQ%3D%3D"
                    + "&__VIEWSTATEGENERATOR=FB6C6F2C"
                    + "&__VIEWSTATEENCRYPTED="
                    + "&__EVENTVALIDATION=CKs99QiiMJ2E9XqMrGTdqJtDalYRrpoJ1Ty8fuaFf8MEYdyFhb%2B2UnhIQ%2FeF4I7Xork8TUH7TPKn7LxdAlD93Inzzu5FFypSQacV45UIx%2BUmf2r4SRUCYHACHx27AL4Msa%2BfUbhd6zqoR2y6unlVfAGu8Pri9%2BHV1rNG2BhpF2o4cvbKC1eVjT9PwdmfS4qqHhUyM2ukCLM3LTnpKyoEcUV1aaKde5DcdFYwA4cdDaXH8emhnCNdujHMDutHAU1esPtPuplPYDv7PoTyMZtLRgcr8Zbk7Pe6%2FpEnynfBB7eGMrz5cNGJwlW8qWpzOxQmv1OroRLQwiCGO%2FoyJijp6tZICRvXMEPyur2Kbt3mdfB%2B4DGXZulYWcy84KKOL1k1LuIXwSMD3Kr6HSyChodoReYuNsfmiM%2BRbh32rsQ3WHnBgYAQnZTKuthxBVnD9EPzNZtLe1VGgA2ol%2FYF1o8DnGuyzAejQ6%2B7HJJ4GdezJrM2FpcLNmdvZSn5m3UCMbx6tLKE1mx7MzarMM%2B7bGh3xZRAAzK9v%2BoP9SRJFlCZgiBAkaF7YJKQHaBnMnNVP1EfW8b4mLUxbjS45FY0z%2B8HiDxbWyGbiGQiUkOdR%2Fch3Pka0QsTapPPLEByjsMeYDVjGfvgCfZn77HUIUQvsQMZeec4nnSfeDoyiWGfCbLfOxktvaTO%2FKsFUILBjJ%2F6YM24bxWT4qvzxMzivCvgi6PiNEGNkej%2FTtWGTRyQUgwHYCh4XXU0ObTMx4nVHUbrF58jrVUNTXlO0ndECk7ireHEUwA9W39QzF9nDfZ8R%2Frq%2BvNVd1b92cA1JKatT6bVs7aTvPKZrHzu7zNS6alfsHBHikkM4xKFtEwLM8b45GtxtM7mhS6DUKx9lW7f%2FilGPwD%2FUl3E0r2OpC%2B5lt%2BGNPUcjcUlLA02GKS5mX2aY%2BM5BdZKmEmo%2FMxIf1w32uW5F6gMsUx5jBNXrhOfdkzjdqdmMBr10EPzlKSzrH3t1uXwyoaDJUP0eSRnRQ1fznm1GCt%2Bqxbs3HGxdd4C71%2FTlDFhrhYJDf4Y3o0iaC3rLbjHt4Qaed2Q7hzefiPY3EKKsMB449I7kC24RaitpJXNC342pXrOFMYhV%2FzO%2FT%2BB4TAQFojSxw5AtGU5G30C4HX7cKVNp%2FkLcxgHrCDYWoGhOGrRI9O%2BqhKCw5WSEbjt0r0uDU9nlQphyT3kUnjp%2FM6pLltvORVDqYmws4z8mSyHH0CfqWy3sHw2Xu8zYu0cokX8pacAN1BT6KlQ6aInwNzJ%2F%2B2BUwGpdhp2I0ZQJMn5oReVZfkdpLeqCZrY3L%2FmuMFzA0R9WMT7OS2zvpQO%2Fk5ycpHPKsK%2FxsfLUJ21V%2B8yDJkDS7%2Fnn3c8CizL03VKQ3Gbweb%2FjyluhnUg%2Bf%2Fqr%2B00ktLcMIzKZICWoaPt3KT1bGaoQO3IjbC%2Ftb1pryMwgK8uyP81SN0Ysc8baiEvwM%2FeNZlA9jwr8o4TILSN7YE6On%2BfzKH4p8fLT7NeWOtG5%2BpgORP7isVvm1RHrvXXoxIfkXn7lsvW3QdvxdCBnnYWMN%2FE3GOcLnEoirJt%2BS31cn0uRPIpApDODga1Jig2%2B8lx7GdUlucWMxmOrXFzU1pBBLn8QKYIDxgn7UgSySyyfWVNthiZokkEsWsZyONzDg%2Fr9OD563xqw9hkHuLb1ba4y4eL9XzcnguO2XAZgNteGjDB62iXHdaO8evAuHazGDGcqbdhz2H1dMen1n%2B31%2B57NXNLGSsOZoQf3aI14g3nA9%2FZ6ch2UDu2yD8vURW7HnihCH6XfzWwgz89z4W0bvdyW0AipjMV%2BbZYvHjsRXo8ewuRvvnPeTKaupWsPFJXZsZs7H2Qi92pFFkr%2BGeFDWaHnQ50Vm3KAHn2Fl7HX6Ch9sgw57SUz4jDowNyaSYZBx3vKB%2BLmy1DY%2B8iueprOqM0mbn9Zpgfh23%2BvFR3%2FBaqrDUx9xk34l4Bt7RJPvAzvmLsesA8aGrwT7npF2N0bTN4y5uOQpQWX3BpbhNJi1VuM%2F7PlN5apaEXnSQUVUrwnZKd3bw5h4eF0BLUYlli1XmK%2BdCj3MwSsWF%2Fl9YS03wCvKvU2oeWyv%2Fyd49t7BJupZWp1YyXb6OpUb9HQxeHb7xg6YNHDuiMkJYmsLQB0IZMp%2FUYM%2BoBpl2wbq1AvK4c0Q1KEaMK"
                    + "&ctl00%24ContentPlaceHolder1%24txtRegisterDate="
                    + "&ctl00%24ContentPlaceHolder1%24txtRegisterNumber="
                    + "&ctl00%24ContentPlaceHolder1%24txtNationalCode="
                    + "&ctl00%24ContentPlaceHolder1%24txtRegisterLocation=";
                byte[] formData = Encoding.ASCII.GetBytes(postData);

                webRequest.ContentLength = formData.Length;
                Stream streamRequest = webRequest.GetRequestStream();
                streamRequest.Write(formData, 0, formData.Length);
                streamRequest.Flush();
                streamRequest.Close();

                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                if (webResponse != null)
                {
                    if (webResponse.ResponseUri == null || string.IsNullOrEmpty(webResponse.ResponseUri.AbsoluteUri))
                    {
                        webResponse.Close();
                        return null;
                    }
                    if (login.fnc_isLoginPage(webResponse, this.v_url))
                    {
                        //we haven't loginned yet
                        if (lg.fnc_login())
                        {
                            goto start;
                        }
                        else
                        {
                            //cannot login inform or log !!!!!
                            webResponse.Close();
                            return null;
                        }
                    }

                    long received = 0;
                    using (webResponse = (HttpWebResponse)webRequest.GetResponse())
                    {
                        byte[] buffer = new byte[1024];
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                        FileStream fileStream = File.OpenWrite(filePath);
                        using (Stream input = webResponse.GetResponseStream())
                        {

                            int size = input.Read(buffer, 0, buffer.Length);
                            while (size > 0)
                            {
                                fileStream.Write(buffer, 0, size);
                                received += size;

                                size = input.Read(buffer, 0, buffer.Length);
                            }
                        }

                        fileStream.Flush();
                        fileStream.Close();
                    }
                    return filePath;
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        private void sb_saveToDB(DataTable dt, int cycleNumber, string introductionFilePath, out Model.rwmmsVehicleOwner entry_vehicleOwner)
        {
            entry_vehicleOwner = null;
            if (dt == null)
            {
                return;
            }
            int i;

            //Model.rwmmsVehicleOwner entry_company;
            bool add;
            string companyName;
            using (var entityLogestic = new Model.logesticEntities())
            {
                for (i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    add = false;
                    if (Functions.IsNull(dt.Rows[i][vehicleOwnersDataTable.fld_companyName])) continue;
                    companyName = dt.Rows[i][vehicleOwnersDataTable.fld_companyName].ToString();
                    var ownerId = Fnc_getVehicleOwnerId(companyName);
                    if (!ownerId.HasValue)
                    {
                        add = true;
                        entry_vehicleOwner = new Model.rwmmsVehicleOwner();
                    }
                    else
                    {
                        entry_vehicleOwner = entityLogestic.rwmmsVehicleOwners.FirstOrDefault(o => o.Id == ownerId);
                    }

                    //if (entry_vehicleOwner == null)
                    //{
                    //    add = true;
                    //    entry_vehicleOwner = new Model.rwmmsVehicleOwner();
                    //}

                    //entry_company.AlternateNames =
                    //entry_company.companyName =
                    entry_vehicleOwner.CycleNumber = cycleNumber;
                    //entry_company.Description 
                    entry_vehicleOwner.FetchTime = DateTime.Now;
                    //entry_company.PostalCode
                    entry_vehicleOwner.registerDate = Functions.IsNull(dt.Rows[i][vehicleOwnersDataTable.fld_registerPersianDate]) ? null : (DateTime?)Functions.solarToMiladi(dt.Rows[i][vehicleOwnersDataTable.fld_registerPersianDate].ToString());
                    //entry_company.ShenaseMeli=
                    entry_vehicleOwner.Source = "rwmms";
                    entry_vehicleOwner.wCompanyName = companyName;
                    entry_vehicleOwner.wIntroductionFile = Functions.IsNull(introductionFilePath) ? null : introductionFilePath;
                    entry_vehicleOwner.wRegisterCityName = Functions.IsNull(dt.Rows[i][vehicleOwnersDataTable.fld_registerCityName]) ? null : dt.Rows[i][vehicleOwnersDataTable.fld_registerCityName].ToString();
                    entry_vehicleOwner.wRegisterNo = Functions.IsNull(dt.Rows[i][vehicleOwnersDataTable.fld_registerNo]) ? null : dt.Rows[i][vehicleOwnersDataTable.fld_registerNo].ToString();
                    entry_vehicleOwner.wRegsiterPersianDate = Functions.IsNull(dt.Rows[i][vehicleOwnersDataTable.fld_registerPersianDate]) ? null : dt.Rows[i][vehicleOwnersDataTable.fld_registerPersianDate].ToString();
                    if (add)
                        entityLogestic.rwmmsVehicleOwners.Add(entry_vehicleOwner);
                    else entityLogestic.Entry(entry_vehicleOwner).State = System.Data.Entity.EntityState.Modified;
                    entityLogestic.SaveChanges();
                    //companyId = entry_company.Id;
                }
            }
        }


        public static int? Fnc_getVehicleOwnerId(string ownerName)
        {
            if (string.IsNullOrEmpty(ownerName)) return null;
            using (var entityLogestic = new Model.logesticEntities())
            {
                var entry_vehicleOwner = entityLogestic.rwmmsVehicleOwners.FirstOrDefault(o => o.companyName == ownerName
                   || o.AlternateNames.Contains(ownerName + ";"));
                if (entry_vehicleOwner == null) return null;
                else return entry_vehicleOwner.Id;
            }
        }
    }

    public class vehicleOwnersDataTable : DataTable
    {
        public vehicleOwnersDataTable()
        {
            this.Columns.Add(new DataColumn(fld_rowNo, typeof(string)));
            this.Columns.Add(new DataColumn(fld_companyName, typeof(string)));
            this.Columns.Add(new DataColumn(fld_registerNo, typeof(long)));
            this.Columns.Add(new DataColumn(fld_registerCityName, typeof(string)));
            this.Columns.Add(new DataColumn(fld_registerPersianDate, typeof(string)));
            this.Columns.Add(new DataColumn(fld_introductionFile, typeof(string)));
            this.Columns.Add(new DataColumn(fld_ownersNames, typeof(string)));
        }


        public static string fld_rowNo = "rowNo";
        public static string fld_companyName = "companyName";
        public static string fld_registerNo = "registerNo";
        public static string fld_registerCityName = "registerCityName";
        public static string fld_registerPersianDate = "regiterPersianDate";
        public static string fld_introductionFile = "enteranceDate";
        public static string fld_ownersNames = "ownersNames";


    }




}
