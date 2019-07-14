using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.htmlModel
{
    class htmlGrid
    {
        static string v_nextPaginationText = "...";
        static string v_previousPaginationText = "...";
        static int v_previousPaginationGroupPos = 0;

        #region shared and static functions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webBrowser"></param>
        /// <param name="url"></param>
        /// <param name="grid"></param>
        /// <param name="controlIdToClick">to get pagecount in some pages we need to click a control if this parameter is specified 
        /// this function click the control</param>
        /// <param name="logoutAndLogin">for some pages to get all rows we need logoutAndLogin set to true</param>
        /// <returns></returns>
        public static int fnc_getPageCount(IWebDriver webBrowser, string url, IWebElement grid
            , IWebElement rowCountElement, string rowCountLabelRegexPattern, int pageRowCount, bool logoutAndLogin, iLogin login)
        {
            if (rowCountElement != null)
            {
                var rowCountStr = rowCountElement.Text;
                int rowCount;
                if (!int.TryParse(rowCountStr, out rowCount))
                {
                    if (!string.IsNullOrEmpty(rowCountLabelRegexPattern))
                        rowCountStr = System.Text.RegularExpressions.Regex.Replace(rowCountStr, rowCountLabelRegexPattern, "");
                    rowCountStr = rowCountStr.Replace("&nbsp;", "");
                    rowCountStr = rowCountStr.Replace(" ", "");
                    rowCountStr = rowCountStr.Replace(",", "");
                    rowCountStr = rowCountStr.Replace("تعدادرکورد", "");
                    rowCountStr = rowCountStr.Replace("تعدادركورد", "");
                    rowCountStr = rowCountStr.TrimStart().TrimEnd();
                    rowCount = int.Parse(rowCountStr);
                }
                return rowCount / pageRowCount + (rowCount % pageRowCount > 0 ? 1 : 0);
            }
            else
            {

                if (logoutAndLogin)
                    login.fnc_logoutWithSelenium(webBrowser);
                if (login.fnc_isLoginPage(webBrowser.Url, url))
                {
                    login.fnc_loginWithSelenium(webBrowser);
                    webBrowser.Navigate().GoToUrl(url);
                }

                if (webBrowser.Url.ToLower() != url.ToLower())
                {
                    webBrowser.Navigate().GoToUrl(url);
                    Functions.sb_waitForReady(webBrowser);
                }

                int lastPageIndex = -1;
                int temp, i;
                getPageSource: string html = webBrowser.PageSource;

                //int paginationGroup = 0;
                if (grid != null)
                {
                    List<IWebElement> rows;
                    try
                    {
                        var tbody = grid.FindElement(By.TagName("tbody"));
                        if (tbody != null)
                            rows = tbody.FindElements(By.TagName("tr")).ToList();
                        else rows = grid.FindElements(By.TagName("tr")).ToList();
                    }
                    catch
                    {
                        rows = grid.FindElements(By.TagName("tr")).ToList();
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
                                Functions.sb_waitForReady(webBrowser);
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
                        }

                    }
                }
                return lastPageIndex;
            }

            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }
        }


        public static int fnc_gotoPage(IWebDriver webBrowser, string url, By gridBy, IWebElement rowCountElement, int pageIndex, int pageCount, bool logoutAndLogin
            , string rowCountLabelRegexPattern, int pageRowCount, iLogin login, bool waitTillReady = true)
        {
            IWebElement grid = webBrowser.FindElement(gridBy);
            int i, temp, max, min;
            min = int.MaxValue;
            max = int.MinValue;

            if (logoutAndLogin)
                login.fnc_logoutWithSelenium(webBrowser);
            if (login.fnc_isLoginPage(webBrowser.Url, url))
            {
                login.fnc_loginWithSelenium(webBrowser);
                webBrowser.Navigate().GoToUrl(url);
            }

            if (webBrowser.Url.ToLower() != url.ToLower())
            {
                webBrowser.Navigate().GoToUrl(url);
                Functions.sb_waitForReady(webBrowser);
            }

            if (pageCount == -1)
            {
                pageCount = fnc_getPageCount(webBrowser, url, grid, rowCountElement, rowCountLabelRegexPattern, pageRowCount, logoutAndLogin, login);
            }
            if (pageIndex > pageCount) return -1;

            if (pageIndex == 1)
            {
                webBrowser.Navigate().GoToUrl(url);
                if (waitTillReady)
                    Functions.sb_waitForReady(webBrowser);
                return pageIndex;
            }
            else
            {
                //webBrowser.Navigate().GoToUrl(url);
                //Functions.sb_waitForReady(webBrowser);
                grid = webBrowser.FindElement(gridBy);
                if (grid != null)
                {
                    var rows = grid.FindElements(By.TagName("tr"));
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
                                    Functions.sb_waitForReady(webBrowser);
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
                                            Functions.sb_waitForReady(webBrowser);

                                        return fnc_gotoPage(webBrowser, url, gridBy, rowCountElement, pageIndex, pageCount, logoutAndLogin, rowCountLabelRegexPattern, pageRowCount, login, waitTillReady);
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
                                            Functions.sb_waitForReady(webBrowser);

                                        return fnc_gotoPage(webBrowser, url, gridBy, rowCountElement, pageIndex, pageCount, logoutAndLogin, rowCountLabelRegexPattern, pageRowCount, login, waitTillReady);
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
        #endregion
    }
}
