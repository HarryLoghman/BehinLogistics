using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.rwmms
{
    class seleniumDownloader
    {
        ///// <summary>
        ///// how many each group has
        ///// </summary>
        static int v_paginationSize = 15;
        ///// <summary>
        ///// where is the next pagination group position 
        ///// </summary>
        //static int v_nextPaginationGroupPos = 9;
        /// <summary>
        /// where is the previous pagination group position 
        /// </summary>
        static int v_previousPaginationGroupPos = 0;

        static string v_nextPaginationText = "...";
        static string v_previousPaginationText = "...";
        public static void sb_click(IWebDriver webBrowser, string url, string controlId, bool logoutAndLogin)
        {
            login lg = new login();
            if (logoutAndLogin)
                lg.fnc_logoutWithSelenium(webBrowser);
            if (login.fnc_isLoginPage(webBrowser.Url, url))
            {
                lg.fnc_loginWithSelenium(webBrowser);
                webBrowser.Navigate().GoToUrl(url);
            }

            if (webBrowser.Url.ToLower() != url.ToLower())
            {
                webBrowser.Navigate().GoToUrl(url);
                SharedFunctions.sb_waitForReady(webBrowser);
            }
            string html = webBrowser.PageSource;
            var control = webBrowser.FindElement(By.Id(controlId));
            if (control != null)
            {
                control.Click();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webBrowser"></param>
        /// <param name="url"></param>
        /// <param name="tableId"></param>
        /// <param name="controlIdToClick">to get pagecount in some pages we need to click a control if this parameter is specified 
        /// this function click the control</param>
        /// <param name="logoutAndLogin">for some pages to get all rows we need logoutAndLogin set to true</param>
        /// <returns></returns>
        public static int fnc_getPageCount(IWebDriver webBrowser, string url, string tableId, bool logoutAndLogin)
        {
            return fnc_getPageCount(webBrowser, url, tableId, null, logoutAndLogin);
        }

        public static int fnc_getPageCount(IWebDriver webBrowser, string url, By tableBy, bool logoutAndLogin)
        {
            return fnc_getPageCount(webBrowser, url, tableBy, null, logoutAndLogin);
        }
        public static int fnc_getPageCount(IWebDriver webBrowser, string url, string tableId, string controlIdToClick, bool logoutAndLogin)
        {
            return fnc_getPageCount(webBrowser, url, By.Id(tableId), controlIdToClick, logoutAndLogin);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webBrowser"></param>
        /// <param name="url"></param>
        /// <param name="tableId"></param>
        /// <param name="controlIdToClick">to get pagecount in some pages we need to click a control if this parameter is specified 
        /// this function click the control</param>
        /// <param name="logoutAndLogin">for some pages to get all rows we need logoutAndLogin set to true</param>
        /// <returns></returns>
        public static int fnc_getPageCount(IWebDriver webBrowser, string url, By tableBy, string controlIdToClick, bool logoutAndLogin)
        {
            login lg = new login();
            if (logoutAndLogin)
                lg.fnc_logoutWithSelenium(webBrowser);
            if (login.fnc_isLoginPage(webBrowser.Url, url))
            {
                lg.fnc_loginWithSelenium(webBrowser);
                webBrowser.Navigate().GoToUrl(url);
            }

            if (webBrowser.Url.ToLower() != url.ToLower())
            {
                webBrowser.Navigate().GoToUrl(url);
                SharedFunctions.sb_waitForReady(webBrowser);
            }
            if (!string.IsNullOrEmpty(controlIdToClick))
            {
                sb_click(webBrowser, url, controlIdToClick, false);
            }
            int lastPageIndex = -1;
            int temp, i;
            getPageSource: string html = webBrowser.PageSource;
            var table = webBrowser.FindElement(tableBy);
            //int paginationGroup = 0;
            if (table != null)
            {
                List<IWebElement> rows;
                try
                {
                    var tbody = table.FindElement(By.TagName("tbody"));
                    if (tbody != null)
                        rows = tbody.FindElements(By.TagName("tr")).ToList();
                    else rows = table.FindElements(By.TagName("tr")).ToList();
                }
                catch
                {
                    rows = table.FindElements(By.TagName("tr")).ToList();
                }

                if (rows != null && rows.Count > 0)
                {
                    //we have atleast one page
                    lastPageIndex = 1;
                    var lastRow = rows[rows.Count - 1];
                    var td = lastRow.FindElement(By.TagName("td"));
                    var hrefCollection = td.FindElements(By.TagName("a"));
                    if (hrefCollection != null && hrefCollection.Count > 0)
                    {
                        if (/*v_nextPaginationGroupPos <= hrefCollection.Count
                            &&*/ hrefCollection[hrefCollection.Count - 1].Text == v_nextPaginationText)
                        {
                            //we have next page so we should have next paginationgroup
                            hrefCollection[hrefCollection.Count - 1].Click();
                            SharedFunctions.sb_waitForReady(webBrowser);
                            goto getPageSource;
                            //paginationGroup++;
                        }
                        else
                        {
                            //we do not have nextPaginationGroup
                            for (i = 0; i <= hrefCollection.Count - 1; i++)
                            {
                                if (int.TryParse(hrefCollection[i].Text, out temp))
                                {
                                    if (temp > lastPageIndex)
                                    {
                                        lastPageIndex = temp;
                                    }
                                }
                            }
                            return lastPageIndex;
                        }
                        //if (hrefCollection.Count < v_paginationSize)
                        //{
                        //    //we do not have next paginiation for example we have only 3 pages and paginzationsize is 10
                        //    var pageIndexStr = hrefCollection[hrefCollection.Count - 1].Text;
                        //    int temp;
                        //    if (int.TryParse(pageIndexStr, out temp))
                        //    {
                        //        lastPageIndex = temp + (paginationGroup * v_paginationSize);
                        //    }
                        //}
                        //else if (hrefCollection.Count > v_paginationSize)
                        //{
                        //    //we have more pages than paginzationsize. e.g. we have 12 pages while paginationsize is 10
                        //    hrefCollection[v_nextPaginationGroupPos].Click();
                        //    SharedFunctions.sb_waitForReady(webBrowser);
                        //    paginationGroup++;

                        //}
                        //else if (hrefCollection.Count == v_paginationSize)
                        //{
                        //    //there are three possibilities paginationsize = 10 and we have 10 pages
                        //    //or we have 19 pages in second group we will have 9 pages and previous button
                        //    //or we have 9 pages  + nextPagination we have 10 hrefCollection
                        //    //so => hrefCollection.Count == v_paginationSize
                        //    if (hrefCollection[v_nextPaginationGroupPos].Text == v_nextPaginationText)
                        //    {
                        //        hrefCollection[v_nextPaginationGroupPos].Click();
                        //        SharedFunctions.sb_waitForReady(webBrowser);
                        //        paginationGroup++;
                        //    }
                        //    else
                        //    {
                        //        var pageIndexStr = hrefCollection[hrefCollection.Count - 1].Text;
                        //        int temp;
                        //        if (int.TryParse(pageIndexStr, out temp))
                        //        {
                        //            lastPageIndex = temp + (paginationGroup * v_paginationSize);
                        //        }
                        //    }
                        //}
                    }

                }
            }
            return lastPageIndex;

        }

        public static int fnc_getPageCountWithRowCount(IWebDriver webBrowser, string url, string controlIdRowCount, string controlIdToClick, bool logoutAndLogin)
        {
            login lg = new login();
            if (logoutAndLogin)
                lg.fnc_logoutWithSelenium(webBrowser);
            if (login.fnc_isLoginPage(webBrowser.Url, url))
            {
                lg.fnc_loginWithSelenium(webBrowser);
                webBrowser.Navigate().GoToUrl(url);
            }

            if (webBrowser.Url.ToLower() != url.ToLower())
            {
                webBrowser.Navigate().GoToUrl(url);
                SharedFunctions.sb_waitForReady(webBrowser);
            }

            if (!string.IsNullOrEmpty(controlIdToClick))
            {
                sb_click(webBrowser, url, controlIdToClick, false);
            }
            int lastPageIndex = -1;
            var elRowCount = webBrowser.FindElement(By.Id(controlIdRowCount));

            //int paginationGroup = 0;
            if (elRowCount != null)
            {
                if (int.TryParse(elRowCount.Text, out lastPageIndex))
                {
                    return lastPageIndex / v_paginationSize + (lastPageIndex % v_paginationSize > 0 ? 1 : 0);
                }
            }
            return lastPageIndex;

        }
        public static int fnc_gotoPage(IWebDriver webBrowser, string url, string tableId, int pageIndex, int pageCount, bool logoutAndLogin
          , bool waitTillReady = true)
        {
            return fnc_gotoPage(webBrowser, url, By.Id(tableId), pageIndex, pageCount, logoutAndLogin, waitTillReady);
        }
        public static int fnc_gotoPage(IWebDriver webBrowser, string url, By tableBy, int pageIndex, int pageCount, bool logoutAndLogin
            , bool waitTillReady = true)
        {
            int i, temp, max, min;
            min = int.MaxValue;
            max = int.MinValue;
            login lg = new login();
            if (logoutAndLogin)
                lg.fnc_logoutWithSelenium(webBrowser);
            if (login.fnc_isLoginPage(webBrowser.Url, url))
            {
                lg.fnc_loginWithSelenium(webBrowser);
                webBrowser.Navigate().GoToUrl(url);
            }

            if (webBrowser.Url.ToLower() != url.ToLower())
            {
                webBrowser.Navigate().GoToUrl(url);
                SharedFunctions.sb_waitForReady(webBrowser);
            }

            if (pageCount == -1)
            {
                pageCount = fnc_getPageCount(webBrowser, url, tableBy, false);
            }
            if (pageIndex > pageCount) return -1;

            if (pageIndex == 1)
            {
                webBrowser.Navigate().GoToUrl(url);
                if (waitTillReady)
                    SharedFunctions.sb_waitForReady(webBrowser);
                return pageIndex;
            }
            else
            {
                //webBrowser.Navigate().GoToUrl(url);
                //SharedFunctions.sb_waitForReady(webBrowser);
                var table = webBrowser.FindElement(tableBy);
                if (table != null)
                {
                    var rows = table.FindElements(By.TagName("tr"));
                    if (rows != null && rows.Count > 0)
                    {
                        var lastRow = rows[rows.Count - 1];
                        var td = lastRow.FindElement(By.TagName("td"));
                        if (td != null)
                        {
                            var hrefCollection = td.FindElements(By.TagName("a")).ToList();
                            var hrefCollectionAndCurrentPage = hrefCollection.ToList();
                            hrefCollectionAndCurrentPage.AddRange(td.FindElements(By.TagName("span")).ToList());

                            var hrefPage = hrefCollectionAndCurrentPage.FirstOrDefault(o => o.Text == pageIndex.ToString());
                            if (hrefPage != null)
                            {
                                hrefPage.Click();
                                if (waitTillReady)
                                    SharedFunctions.sb_waitForReady(webBrowser);
                                return pageIndex;
                            }
                            else
                            {
                                for (i = 0; i <= hrefCollectionAndCurrentPage.Count - 1; i++)
                                {
                                    if (int.TryParse(hrefCollectionAndCurrentPage[i].Text, out temp))
                                    {
                                        if (temp < min)
                                        {
                                            min = temp;
                                        }
                                        if (temp > max)
                                        {
                                            max = temp;
                                        }
                                    }
                                }
                                if (min == int.MaxValue || max == int.MinValue)
                                {
                                    //we cannot find any href with integer text
                                    return -1;
                                }
                                if (min <= pageIndex && pageIndex <= max)
                                {
                                    //page index is between min and max but we could find the page in previous step so there is a problem
                                    return -1;
                                }
                                if (pageIndex < min)
                                {
                                    //go to previous page
                                    if (hrefCollection.Count >= v_previousPaginationGroupPos
                                 && hrefCollection[v_previousPaginationGroupPos].Text == v_previousPaginationText)
                                    {
                                        hrefCollection[v_previousPaginationGroupPos].Click();
                                        if (waitTillReady)
                                            SharedFunctions.sb_waitForReady(webBrowser);

                                        return fnc_gotoPage(webBrowser, url, tableBy, pageIndex, pageCount, logoutAndLogin);
                                    }
                                }
                                else if (pageIndex > max)
                                {
                                    //go to next page
                                    if (/*hrefCollection.Count >= v_nextPaginationGroupPos
                                   &&*/ hrefCollection[hrefCollection.Count - 1].Text == v_nextPaginationText)
                                    {
                                        hrefCollection[hrefCollection.Count - 1].Click();
                                        if (waitTillReady)
                                            SharedFunctions.sb_waitForReady(webBrowser);

                                        return fnc_gotoPage(webBrowser, url, tableBy, pageIndex, pageCount, logoutAndLogin);
                                    }
                                }

                                return -1;
                            }
                        }
                        else return -1;
                    }
                    else return -1;
                }
                else return -1;
            }
        }

        public static void sb_readAndSaveToDatatable(IWebDriver webBrowser, string url, string tableId, Model.htmlTable dt
            , bool logoutAndLogin)
        {
            login lg = new login();
            if (logoutAndLogin)
                lg.fnc_logoutWithSelenium(webBrowser);
            if (login.fnc_isLoginPage(webBrowser.Url, url))
            {
                lg.fnc_loginWithSelenium(webBrowser);
                webBrowser.Navigate().GoToUrl(url);
            }

            if (webBrowser.Url.ToLower() != url.ToLower())
            {
                webBrowser.Navigate().GoToUrl(url);
                SharedFunctions.sb_waitForReady(webBrowser);
            }

            string html = webBrowser.PageSource;

            if (!string.IsNullOrEmpty(html))
            {
                Functions.sb_fillDatatableWithHtmlTableId(html, tableId, dt);
            }

        }
    }
}
