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

namespace RailSiteDataGrabber.rwmms
{
    class webpageGrid : webpageControl
    {
        //public webpageGrid(ChromeDriver webbrowser, string name, string alias, string controlId, int? controlIndex
        //    , enumControlType controlType) : this(webbrowser, name, alias, controlId, controlIndex, controlType, null, null)
        //{

        //}
        static string v_nextPaginationText = "...";
        static string v_previousPaginationText = "...";
        static int v_previousPaginationGroupPos = 0;
        public webpageGrid(ChromeDriver webbrowser, webpage page, string name, string alias, By controlBy
            , enumControlType controlType)
            : this(webbrowser, page, name, alias, controlBy, "", controlType, null)
        {

        }

        public webpageGrid(ChromeDriver webbrowser, webpage page, string name, string alias, By controlBy
            , enumControlType controlType, Model.htmlTable tableSource)
              : this(webbrowser, page, name, alias, controlBy, "", controlType, tableSource)
        {
            this.prp_tableSource = tableSource;
        }

        public webpageGrid(ChromeDriver webbrowser, webpage page, string name, string alias, By controlBy
            , string controlAtt, enumControlType controlType, Model.htmlTable tableSource)
              : base(webbrowser, page, name, alias, controlBy, controlAtt, controlType)
        {
            this.prp_tableSource = tableSource;
        }

        webpageGridColumn[] v_webpageGridColumns;
        public webpageGridColumn[] prp_gridColumns
        {
            get
            {
                return this.v_webpageGridColumns;
            }
            set
            {
                this.v_webpageGridColumns = value;
            }
        }

        public Model.htmlTable prp_tableSource
        {
            get; set;

        }

        /// <summary>
        /// a control which should be clicked for the first time to load data into the grid
        /// </summary>
        public By prp_controlLoadDataBy
        {
            get;
            set;
        }

        public By prp_controlRowCountBy
        {
            get; set;
        }

        public string prp_controlRowCountLabelRegexPattern
        {
            get; set;
        }

        /// <summary>
        /// number of rows in each page
        /// </summary>
        public int prp_pageRowCount
        {
            get; set;
        }

        public int prp_pageCount { get; set; }

        public int prp_skipRowTop
        {
            get;set;
            
        }

        public int prp_skipRowBottom
        {
            get;set;
        }

        /// <summary>
        /// how many rows should skip from top of html table
        /// </summary>
        public int[] prp_skipRowIndecies { get; set; }

        /// <summry>
        /// before get the value call sb_setDataSource
        /// </summary>
        public DataTable prp_datasource { get; set; }

        public IWebElement fnc_getRowCountHtmlElement()
        {
            if (this.prp_controlRowCountBy != null)
            {
                return this.prp_webBrowser.FindElement(this.prp_controlRowCountBy);
            }
            return null;
        }
        /// <summary>
        /// set the prp_datatable
        /// </summary>
        /// <param name="getDetail"></param>
        /// <param name="download"></param>
        /// <param name="startPageIndex">read data from startPageIndex</param>
        private void sb_setDataSource(bool getDetail, bool download, int startPageIndex)
        {
            if (this.prp_controlLoadDataBy != null)
            {
                this.prp_webBrowser.FindElement(this.prp_controlLoadDataBy).Click();
                SharedFunctions.sb_waitForReady(this.prp_webBrowser);
            }
            var element = this.fnc_getHtmlElement();
            this.prp_pageCount = webpageGrid.fnc_getPageCount(this.prp_webBrowser, this.prp_webpage.prp_url, element, this.fnc_getRowCountHtmlElement(), null, this.prp_pageRowCount, false);

            int pageIndex;
            int i, j;

            DataTable dt = this.fnc_createDatatable();
            DataRow dr;
            webpageGridColumnDownload columnDownload;
            webpageGridColumnDetail columnDetail;
            webpage pgDetail;
            string val;

            for (pageIndex = startPageIndex; pageIndex <= this.prp_pageCount; pageIndex++)
            {
                if (pageIndex > 1)
                {
                    pageIndex = webpageGrid.fnc_gotoPage(this.prp_webBrowser, this.prp_webpage.prp_url, element, this.fnc_getRowCountHtmlElement(), pageIndex, this.prp_pageCount, false
                        , this.prp_controlRowCountLabelRegexPattern, this.prp_pageRowCount, false);
                    if (pageIndex == -1) return;
                }
                element = this.fnc_getHtmlElement();


                var htmlRows = element.FindElements(By.TagName("tr"));
                System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> htmlCells;

                for (i = this.prp_skipRowTop; i <= htmlRows.Count - 1 - this.prp_skipRowBottom; i++)
                {
                    if (this.prp_skipRowIndecies != null && this.prp_skipRowIndecies.Any(o => o == i))
                        continue;

                    dr = null;
                    htmlCells = htmlRows[i].FindElements(By.TagName("td"));
                    for (j = 0; j <= this.prp_gridColumns.Length - 1; j++)
                    {
                        if (this.prp_gridColumns[j] is webpageGridColumnDetail)
                        {
                            if (getDetail)
                            {
                                columnDetail = ((webpageGridColumnDetail)this.prp_gridColumns[j]);
                                pgDetail = (webpage)Activator.CreateInstance(columnDetail.prp_webpgType);
                                this.sb_gotoDetail(htmlCells[columnDetail.prp_colIndexInGrid]
                                    , pgDetail, columnDetail.prp_getDetail, columnDetail.prp_download
                                    , columnDetail.prp_startPageIndex
                                    , columnDetail.prp_controlLoadDataBy);
                                dr[this.prp_gridColumns[j].prp_colName] = pgDetail;
                            }
                        }
                        else if (this.prp_gridColumns[j] is webpageGridColumnDownload)
                        {
                            if (download)
                            {
                                columnDownload = ((webpageGridColumnDownload)this.prp_gridColumns[j]);
                                dr[this.prp_gridColumns[j].prp_colName] = this.fnc_downloadAttach(htmlCells[columnDownload.prp_colIndexInGrid]
                                    , columnDownload.prp_downloadDirectory
                                    , columnDownload.prp_fileDirPath
                                    , columnDownload.fnc_getFileName(htmlCells));
                            }
                        }
                        else
                        {
                            if (dr == null) dr = dt.NewRow();
                            val = htmlCells[this.prp_gridColumns[j].prp_colIndexInGrid].Text.Replace("&nbsp;", " ").TrimStart().TrimEnd();
                            if (string.IsNullOrEmpty(val))
                                dr[this.prp_gridColumns[j].prp_colName] = DBNull.Value;
                            else
                                dr[this.prp_gridColumns[j].prp_colName] = Convert.ChangeType(val, this.prp_gridColumns[j].prp_datatype);
                        }
                    }
                    if (dr != null)
                        dt.Rows.Add(dr);
                }
            }
            this.prp_datasource = dt;
        }


        private string fnc_downloadAttach(IWebElement elementDownload, string downloadDirPath, string fileDirPath
            , string fileNameWithOutExtension)
        {
            elementDownload.Click();
            SharedFunctions.sb_waitForReady(this.prp_webBrowser);
            FileInfo myFile;
            do
            {
                myFile = (new DirectoryInfo(downloadDirPath)).GetFiles("*.*").OrderByDescending(o => o.LastWriteTime).FirstOrDefault();

                if (myFile == null || myFile.LastWriteTime < DateTime.Now.AddMinutes(-5))
                {
                    //there is error
                    throw new Exception("File cannot be downloaded");
                }
                else if (myFile.Extension != ".crdownload" && myFile.Extension != ".tmp")
                {
                    char[] chArrInvalidFileChars = Path.GetInvalidFileNameChars();
                    char[] chArrInvalidPathChars = Path.GetInvalidPathChars();
                    fileNameWithOutExtension = string.Join("_", fileNameWithOutExtension.Split(chArrInvalidFileChars));
                    fileNameWithOutExtension = string.Join("_", fileNameWithOutExtension.Split(chArrInvalidPathChars));
                    fileNameWithOutExtension = fileDirPath + "\\" + fileNameWithOutExtension + myFile.Extension;
                    if (!Directory.Exists(fileDirPath))
                        Directory.CreateDirectory(fileDirPath);
                    if (File.Exists(fileNameWithOutExtension))
                        File.Delete(fileNameWithOutExtension);

                    myFile.MoveTo(fileNameWithOutExtension);

                    return fileNameWithOutExtension;


                }
            } while (myFile.Extension == ".crdownload" || myFile.Extension == ".tmp"/*wait till .crdownload or .tmp to change to .pdf*/);
            return null;
        }

        private void sb_gotoDetail(IWebElement elementDetail, webpage pageDetail, bool getDetail, bool download, int startPageIndex
            , By controlLoadDataBy)
        {
            elementDetail.Click();
            SharedFunctions.sb_waitForReady(this.prp_webBrowser);
            int i;
            for (i = 0; i <= pageDetail.prp_controls.Count - 1; i++)
            {
                if (pageDetail.prp_controls[i] is webpageCombo)
                {
                    ((webpageCombo)pageDetail.prp_controls[i]).sb_setListItems();
                    ((webpageCombo)pageDetail.prp_controls[i]).sb_setValue();
                }
                else if (pageDetail.prp_controls[i] is webpageGrid)
                {
                    ((webpageGrid)pageDetail.prp_controls[i]).prp_controlLoadDataBy = controlLoadDataBy;
                    ((webpageGrid)pageDetail.prp_controls[i]).sb_setDataSource(getDetail, download, startPageIndex);
                }
                else if (pageDetail.prp_controls[i] is webpageControl)
                {
                    pageDetail.prp_controls[i].sb_setValue();
                }
            }

            if (pageDetail.prp_controlBack != null)
            {
                pageDetail.prp_controlBack.fnc_getHtmlElement().Click();
                SharedFunctions.sb_waitForReady(this.prp_webBrowser);
            }
        }
        private DataTable fnc_createDatatable()
        {
            int j;
            DataTable dt = new DataTable();
            for (j = 0; j <= this.prp_gridColumns.Length - 1; j++)
            {
                if (this.prp_gridColumns[j] is webpageGridColumnDetail)
                {
                    dt.Columns.Add(new DataColumn(this.prp_gridColumns[j].prp_colName, typeof(webpage)));
                }
                else
                {
                    dt.Columns.Add(new DataColumn(this.prp_gridColumns[j].prp_colName, this.prp_gridColumns[j].prp_datatype));

                }
            }
            return dt;
        }
        public bool fnc_saveTableSource()
        {
            if (this.prp_tableSource == null)
                throw new Exception("tableSource is not specified");
            var element = this.prp_webBrowser.FindElement(this.prp_controlBy);
            var rows = element.FindElements(By.TagName("tr"));
            return this.prp_tableSource.fnc_save(rows);
        }

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
            , IWebElement rowCountElement, string rowCountLabelRegexPattern, int pageRowCount, bool logoutAndLogin)
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
                        }

                    }
                }
                return lastPageIndex;
            }

        }

        public static int fnc_gotoPage(IWebDriver webBrowser, string url, IWebElement grid, IWebElement rowCountElement, int pageIndex, int pageCount, bool logoutAndLogin
            , string rowCountLabelRegexPattern, int pageRowCount, bool waitTillReady = true)
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
                pageCount = fnc_getPageCount(webBrowser, url, grid, rowCountElement, rowCountLabelRegexPattern, pageRowCount, logoutAndLogin);
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

                                        return fnc_gotoPage(webBrowser, url, grid, rowCountElement, pageIndex, pageCount, logoutAndLogin, rowCountLabelRegexPattern, pageRowCount, waitTillReady);
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

                                        return fnc_gotoPage(webBrowser, url, grid, rowCountElement, pageIndex, pageCount, logoutAndLogin, rowCountLabelRegexPattern, pageRowCount, waitTillReady);
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
