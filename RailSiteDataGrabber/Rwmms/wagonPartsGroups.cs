using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace RailSiteDataGrabber.rwmms
{
    class wagonPartsGroups : IDisposable
    {
        string v_url = "http://rwmms.rai.ir/WagonGroupIdentity.aspx";
        string v_tableId = "ContentPlaceHolder1_GroupGrid";
        string v_tableIdSub = "ContentPlaceHolder1_TabContainer1_SetPanel_Grid2";
        string v_tableIdParts = "ContentPlaceHolder1_TabContainer1_PartPanel_Grid1";
        string v_pathElementId = "ContentPlaceHolder1_PathLbl";
        wagonPartsGroupsDataTable v_dt;
        ChromeDriverService v_webBrowserService;
        ChromeOptions v_webBrowserOptions;
        ChromeDriver v_webBrowser;
        string v_downloadPath;
        int v_pageCount;
        //string v_buttonSearchId = "ContentPlaceHolder1_cmdSearch";
        //string v_spanPageRowCountId = "ContentPlaceHolder1_lblRecordCount";
        public wagonPartsGroups()
        {
            this.v_downloadPath = Environment.CurrentDirectory + "\\Files\\rwmms\\wagonParts\\";
            this.v_dt = new wagonPartsGroupsDataTable(1, 1, true);

            this.v_webBrowserService = ChromeDriverService.CreateDefaultService();
            this.v_webBrowserService.HideCommandPromptWindow = true;

            this.v_webBrowserOptions = new ChromeOptions();
            //this.v_webBrowserOptions.AddArgument("--headless");
            this.v_webBrowserOptions.AddUserProfilePreference("download.default_directory", this.v_downloadPath);
            this.v_webBrowser = new ChromeDriver(this.v_webBrowserService, this.v_webBrowserOptions);

            this.v_pageCount = seleniumDownloader.fnc_getPageCount(this.v_webBrowser, this.v_url, this.v_tableId, true);
        }

        #region firefox
        //FirefoxDriverService v_webBrowserService;
        //FirefoxOptions v_webBrowserOptions;
        //FirefoxDriver v_webBrowser;
        //int v_pageCount;
        //string v_buttonSearchId = "ContentPlaceHolder1_cmdSearch";
        //public declerationList()
        //{
        //    this.v_dt = new declerationListDataTable();

        //    this.v_webBrowserService = FirefoxDriverService.CreateDefaultService();
        //    this.v_webBrowserService.HideCommandPromptWindow = true;

        //    this.v_webBrowserOptions = new FirefoxOptions();
        //    //this.v_webBrowserOptions.AddArgument("--headless");

        //    FirefoxProfile profile = new FirefoxProfile();
        //    profile.SetPreference("browser.download.folderList", 2);
        //    profile.SetPreference("browser.download.dir", @"D:\Application\RailSiteDataGrabber\RailSiteDataGrabber\bin\Debug\Files");
        //    profile.SetPreference("browser.helperApps.neverAsk.saveToDisk",
        //       "text/csv,application/java-archive, application/x-msexcel,application/excel,application/vnd.openxmlformats-officedocument.wordprocessingml.document,application/x-excel,application/vnd.ms-excel,image/png,image/jpeg,text/html,text/plain,application/msword,application/xml,application/vnd.microsoft.portable-executable");
        //    //profile.SetPreference("browser.download.manager.showWhenStarting","False" );
        //    //profile.SetPreference("browser.download.manager.showAlertOnComplete", false);
        //    //profile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf");


        //    this.v_webBrowserOptions.Profile = profile;
        //    this.v_webBrowser = new FirefoxDriver(this.v_webBrowserService, this.v_webBrowserOptions);

        //    this.v_pageCount = seleniumDownloader.fnc_getPageCount(this.v_webBrowser, this.v_url, this.v_tableId, this.v_buttonSearchId, true);
        //}
        #endregion

        public void Dispose()
        {
            this.v_webBrowser.Close();
            this.v_webBrowser.Dispose();
        }
        public void sb_readAndSaveToDB(bool loginAndLogout, bool getAttach)
        {
            if (loginAndLogout)
            {
                login lg = new login();
                lg.fnc_logoutWithSelenium(this.v_webBrowser);
                if (login.fnc_isLoginPage(this.v_webBrowser.Url, this.v_url))
                {
                    lg.fnc_loginWithSelenium(this.v_webBrowser);
                    this.v_webBrowser.Navigate().GoToUrl(this.v_url);
                    SharedFunctions.sb_waitForReady(this.v_webBrowser);
                }
            }

            int pageIndex = 1;
            int rowIndex;
            string html;

            using (var entityLogestic = new Model.logesticEntities())
            {
                for (pageIndex = 1; pageIndex <= this.v_pageCount; pageIndex++)
                {
                    Program.logs.Info("Start of :" + pageIndex);
                    //if we should get extraDetail we should logout and then login again
                    if (pageIndex > 1)
                    {
                        pageIndex = seleniumDownloader.fnc_gotoPage(this.v_webBrowser, this.v_url, this.v_tableId, pageIndex, this.v_pageCount, false);
                        if (pageIndex == -1) return;
                    }

                    html = this.v_webBrowser.PageSource;

                    if (!string.IsNullOrEmpty(html))
                    {
                        this.v_dt.Rows.Clear();
                        Functions.sb_fillDatatableWithHtmlTableId(html, v_tableId, this.v_dt);
                        this.sb_fillDatatablePSID(html, v_tableId, 2, this.v_dt);
                        if (this.v_dt.Rows.Count > 0)
                        {
                            this.sb_saveGroupsToDB(this.v_dt, null);
                        }
                    }
                    else break;

                    try
                    {
                        int groupId;
                        for (rowIndex = 0; rowIndex <= this.v_dt.Rows.Count - 1; rowIndex++)
                        {
                            if (Functions.IsNull(this.v_dt.Rows[rowIndex][wagonPartsGroupsDataTable.fld_id]))
                                continue;

                            groupId = int.Parse(this.v_dt.Rows[rowIndex][wagonPartsGroupsDataTable.fld_id].ToString());

                            var el = this.v_webBrowser.FindElement(By.Id(this.v_tableId));
                            var rows = el.FindElements(By.TagName("tr"));
                            var cells = rows[rowIndex + this.v_dt.prp_skipRowTop].FindElements(By.TagName("td"));
                            cells[2].Click();
                            SharedFunctions.sb_waitForReady(this.v_webBrowser);

                            var queryString = HttpUtility.ParseQueryString(this.v_webBrowser.Url);
                            if (!Functions.IsNull(queryString["psid"]))
                            {
                                var entryPartGroup = entityLogestic.rwmmsWagonPartsGroups.FirstOrDefault(o => o.Id == groupId);
                                if (entryPartGroup != null)
                                {
                                    entryPartGroup.wPSID = queryString["psid"];
                                    entityLogestic.SaveChanges();
                                }
                            }
                            this.sb_readAndSavePartsToDB(groupId, this.v_dt.Rows[rowIndex][wagonPartsGroupsDataTable.fld_groupName].ToString(), getAttach
                                , this.v_downloadPath);
                            this.sb_readAndSaveSubGroupsToDB(groupId, getAttach, this.v_downloadPath + (this.v_downloadPath.EndsWith("\\") ? "" : "\\") + this.v_dt.Rows[rowIndex][wagonPartsGroupsDataTable.fld_groupName].ToString());

                            var elPath = this.v_webBrowser.FindElement(By.Id(this.v_pathElementId));
                            var hrefCollection = elPath.FindElements(By.TagName("a"));
                            if (hrefCollection != null && hrefCollection.Count > 0)
                            {
                                hrefCollection[hrefCollection.Count - 1].Click();
                                SharedFunctions.sb_waitForReady(this.v_webBrowser);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    Program.logs.Info("End of :" + pageIndex);
                }
            }
        }
        private void sb_readAndSaveSubGroupsToDB(int groupIdParent, bool getAttach, string attachmentDirPath)
        {
            wagonPartsGroupsDataTable dt = new wagonPartsGroupsDataTable(1, 0, false);
            string html;
            int rowIndex;


            this.v_webBrowser.FindElement(By.Id("__tab_ContentPlaceHolder1_TabContainer1_SetPanel")).Click();
            html = this.v_webBrowser.PageSource;

            if (!string.IsNullOrEmpty(html))
            {
                dt.Rows.Clear();
                Functions.sb_fillDatatableWithHtmlTableId(html, v_tableIdSub, dt);
                this.sb_fillDatatablePSID(html, v_tableIdSub, 1, dt);
                if (dt.Rows.Count > 0)
                {
                    this.sb_saveGroupsToDB(dt, groupIdParent);
                }

                try
                {
                    int groupId;
                    for (rowIndex = 0; rowIndex <= dt.Rows.Count - 1; rowIndex++)
                    {
                        if (Functions.IsNull(dt.Rows[rowIndex][wagonPartsGroupsDataTable.fld_id]))
                            continue;

                        groupId = int.Parse(dt.Rows[rowIndex][wagonPartsGroupsDataTable.fld_id].ToString());

                        var el = this.v_webBrowser.FindElement(By.Id(this.v_tableIdSub));
                        var rows = el.FindElements(By.TagName("tr"));
                        var cells = rows[rowIndex + this.v_dt.prp_skipRowTop].FindElements(By.TagName("td"));
                        cells[1].FindElements(By.TagName("a"))[0].Click();
                        SharedFunctions.sb_waitForReady(this.v_webBrowser);

                        this.sb_readAndSavePartsToDB(groupId, dt.Rows[rowIndex][wagonPartsGroupsDataTable.fld_groupName].ToString()
                            , getAttach, attachmentDirPath);
                        this.sb_readAndSaveSubGroupsToDB(groupId, getAttach, attachmentDirPath + (attachmentDirPath.EndsWith("\\") ? "" : "\\") + dt.Rows[rowIndex][wagonPartsGroupsDataTable.fld_groupName].ToString());

                        var elPath = this.v_webBrowser.FindElement(By.Id(this.v_pathElementId));
                        var hrefCollection = elPath.FindElements(By.TagName("a"));
                        if (hrefCollection != null && hrefCollection.Count > 0)
                        {
                            hrefCollection[hrefCollection.Count - 1].Click();
                            SharedFunctions.sb_waitForReady(this.v_webBrowser);
                            this.v_webBrowser.FindElement(By.Id("__tab_ContentPlaceHolder1_TabContainer1_SetPanel")).Click();
                            SharedFunctions.sb_waitForReady(this.v_webBrowser);
                        }
                    }
                }
                catch (Exception ex2)
                {

                }

            }
        }
        private void sb_fillDatatablePSID(string html, string tableId, int editColIndex, Model.htmlTable dt)
        {
            if (string.IsNullOrEmpty(html)) throw new Exception("html is null or empty");
            if (string.IsNullOrEmpty(tableId)) throw new Exception("tableId is null or empty");


            if (dt == null) throw new Exception("dt is null");
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);
            var table = document.GetElementbyId(tableId);

            HtmlAgilityPack.HtmlNodeCollection rows;
            if (table == null) return;
            HtmlAgilityPack.HtmlNode tbody = table.SelectSingleNode("tbody");
            if (tbody != null)
            {
                rows = tbody.SelectNodes("tr");
            }
            else
            {
                rows = table.SelectNodes("tr");
            }
            int i;
            DataRow dr;

            int skipRowCountAtBegining = dt.prp_skipRowTop;
            int skipRowCountAtEnd = dt.prp_skipRowBottom;

            for (i = skipRowCountAtBegining; i <= rows.Count - 1 - skipRowCountAtEnd; i++)
            {
                if (dt.prp_skipRowIndecies != null && dt.prp_skipRowIndecies.Any(o => o == i))
                {
                    continue;
                }
                var cells = rows[i].SelectNodes("td");
                dr = dt.Rows[i - skipRowCountAtBegining];
                var queryStringCollection = HttpUtility.ParseQueryString(cells[editColIndex].ChildNodes[0].Attributes["href"].Value.Split('?')[1]);

                if (queryStringCollection != null && !Functions.IsNull(queryStringCollection["psid"]))
                {
                    dr[wagonPartsGroupsDataTable.fld_psid] = queryStringCollection["psid"];
                }
                //dt.Rows.Add(dr);
            }
        }
        private void sb_readAndSavePartsToDB(int idGroup, string groupName, bool getAttach, string attachmentDirPath)
        {
            wagonPartsDataTable dt = new wagonPartsDataTable(1, 0, false);
            string html;

            try
            {
                this.v_webBrowser.FindElement(By.Id("__tab_ContentPlaceHolder1_TabContainer1_PartPanel")).Click();
            }
            catch
            {
                return;
            }
            html = this.v_webBrowser.PageSource;

            if (!string.IsNullOrEmpty(html))
            {
                dt.Rows.Clear();
                Functions.sb_fillDatatableWithHtmlTableId(html, v_tableIdParts, dt);
                if (getAttach)
                {
                    this.sb_getAttach(dt, attachmentDirPath, groupName);
                }
                if (dt.Rows.Count > 0)
                {
                    this.sb_savePartsToDB(dt, idGroup);
                }


            }
        }

        private void sb_getAttach(wagonPartsDataTable dt, string directoryPath, string groupName)
        {

            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> rows;
            int htmlRowIndex = dt.prp_skipRowTop;
            int dtRowIndex = 0;
            int fileExistCounter;
            char[] chArrInvalidFileChars = Path.GetInvalidFileNameChars();
            char[] chArrInvalidPathChars = Path.GetInvalidPathChars();
            groupName = string.Join("_", groupName.Split(chArrInvalidFileChars));
            groupName = string.Join("_", groupName.Split(chArrInvalidPathChars));

            string downloadPath = directoryPath + (directoryPath.EndsWith("\\") ? "" : "\\")
                + groupName;

            if (!Directory.Exists(downloadPath))
                Directory.CreateDirectory(downloadPath);

            do
            {
                try
                {
                    rows = Functions.fnc_getSeleniumTableRows(this.v_webBrowser, this.v_tableIdParts);
                }
                catch
                {
                    return;
                }
                if (!Functions.IsNull(dt.Rows[dtRowIndex][wagonPartsDataTable.fld_partName]))
                {
                    var cells = rows[htmlRowIndex].FindElements(By.TagName("td"));
                    if (cells.Count >= 6)
                    {
                        if (!Functions.IsNull(cells[6].Text))
                        {
                            cells[6].Click();
                            SharedFunctions.sb_waitForReady(this.v_webBrowser);

                            FileInfo myFile;
                            do
                            {
                                fileExistCounter = 0;
                                myFile = (new DirectoryInfo(this.v_downloadPath)).GetFiles("*.*").OrderByDescending(o => o.LastWriteTime).FirstOrDefault();
                                #region wait for download completion
                                while (myFile == null || myFile.LastWriteTime < DateTime.Now.AddMinutes(-5))
                                {
                                    Thread.Sleep(1000);
                                    myFile = (new DirectoryInfo(this.v_downloadPath)).GetFiles("*.*").OrderByDescending(o => o.LastWriteTime).FirstOrDefault();
                                    fileExistCounter++;
                                    if (fileExistCounter > 30) break;

                                }
                                #endregion

                                if (myFile == null || myFile.LastWriteTime < DateTime.Now.AddMinutes(-5))
                                {
                                    //there is error
                                    throw new Exception("File cannot be downloaded");
                                }
                                else if (myFile.Extension != ".crdownload" && myFile.Extension != ".tmp")
                                {
                                    string fileName = "map_" + dt.Rows[dtRowIndex][wagonPartsDataTable.fld_partName].ToString();

                                    fileName = string.Join("_", fileName.Split(chArrInvalidFileChars));
                                    fileName = string.Join("_", fileName.Split(chArrInvalidPathChars));
                                    fileName = downloadPath + "\\" + fileName + myFile.Extension;
                                    try
                                    {
                                        if (File.Exists(fileName))
                                            File.Delete(fileName);

                                        myFile.MoveTo(fileName);

                                        dt.Rows[dtRowIndex][wagonPartsDataTable.fld_mapPicPath] = fileName;
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    break;
                                }
                            } while (myFile.Extension == ".crdownload" || myFile.Extension == ".tmp"/*wait till .crdownload or .tmp to change to .pdf*/);
                        }
                    }
                }
                htmlRowIndex++;
                dtRowIndex++;
            } while (htmlRowIndex < rows.Count - dt.prp_skipRowBottom);
        }
        private void sb_saveGroupsToDB(wagonPartsGroupsDataTable dt, int? idGroupParent)
        {

            if (dt == null)
            {
                return;
            }
            int i;
            bool add;
            string groupName;
            Model.rwmmsWagonPartsGroup entry_wagonPartsGroup;
            using (var entityLogestic = new Model.logesticEntities())
            {
                for (i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    add = false;
                    if (Functions.IsNull(dt.Rows[i][wagonPartsGroupsDataTable.fld_groupName]))
                        continue;
                    groupName = dt.Rows[i][wagonPartsGroupsDataTable.fld_groupName].ToString();
                    entry_wagonPartsGroup = entityLogestic.rwmmsWagonPartsGroups.FirstOrDefault(o => o.IdGroupParent == idGroupParent
                    && o.wGroupName == groupName);
                    if (entry_wagonPartsGroup == null)
                    {
                        add = true;
                        entry_wagonPartsGroup = new Model.rwmmsWagonPartsGroup();
                    }

                    entry_wagonPartsGroup.FetchTime = DateTime.Now;
                    entry_wagonPartsGroup.IdGroupParent = idGroupParent;
                    entry_wagonPartsGroup.wGroupName = Functions.IsNull(dt.Rows[i][wagonPartsGroupsDataTable.fld_groupName]) ? null : dt.Rows[i][wagonPartsGroupsDataTable.fld_groupName].ToString();
                    entry_wagonPartsGroup.wPSID = Functions.IsNull(dt.Rows[i][wagonPartsGroupsDataTable.fld_psid]) ? null : dt.Rows[i][wagonPartsGroupsDataTable.fld_psid].ToString();
                    entry_wagonPartsGroup.wSerialNo = Functions.IsNull(dt.Rows[i][wagonPartsGroupsDataTable.fld_serialNo]) ? null : dt.Rows[i][wagonPartsGroupsDataTable.fld_serialNo].ToString().Replace(" ", "").Replace("\r\n", "");

                    if (add)
                        entityLogestic.rwmmsWagonPartsGroups.Add(entry_wagonPartsGroup);
                    else entityLogestic.Entry(entry_wagonPartsGroup).State = System.Data.Entity.EntityState.Modified;
                    try
                    {
                        entityLogestic.SaveChanges();
                        dt.Rows[i][wagonPartsGroupsDataTable.fld_id] = entry_wagonPartsGroup.Id;
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
        }

        private void sb_savePartsToDB(wagonPartsDataTable dt, int? idGroup)
        {

            if (dt == null)
            {
                return;
            }
            int i;
            bool add;
            string partName;
            Model.rwmmsWagonPart entry_wagonParts;
            using (var entityLogestic = new Model.logesticEntities())
            {
                for (i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    add = false;
                    if (!Functions.IsNull(dt.Rows[i][wagonPartsDataTable.fld_partName])
                         && idGroup.HasValue)
                    {
                        partName = dt.Rows[i][wagonPartsDataTable.fld_partName].ToString();
                        entry_wagonParts = entityLogestic.rwmmsWagonParts.FirstOrDefault(o => o.IdGroup == idGroup
                        && o.wPartName == partName);
                        if (entry_wagonParts == null)
                        {
                            add = true;
                            entry_wagonParts = new Model.rwmmsWagonPart();
                        }
                    }
                    else
                    {
                        entry_wagonParts = new Model.rwmmsWagonPart();
                    }

                    entry_wagonParts.FetchTime = DateTime.Now;
                    entry_wagonParts.IdGroup = idGroup;

                    entry_wagonParts.wCountInGroup = Functions.IsNull(dt.Rows[i][wagonPartsDataTable.fld_countInGroup]) ? null : (int?)int.Parse(dt.Rows[i][wagonPartsDataTable.fld_countInGroup].ToString());
                    entry_wagonParts.wCountInWagon = Functions.IsNull(dt.Rows[i][wagonPartsDataTable.fld_countInWagon]) ? null : (int?)int.Parse(dt.Rows[i][wagonPartsDataTable.fld_countInWagon].ToString());
                    entry_wagonParts.wMapNumber = Functions.IsNull(dt.Rows[i][wagonPartsDataTable.fld_mapNumber]) ? null : dt.Rows[i][wagonPartsDataTable.fld_mapNumber].ToString();
                    entry_wagonParts.wMapPic1Path = Functions.IsNull(dt.Rows[i][wagonPartsDataTable.fld_mapPic1Path]) ? null : dt.Rows[i][wagonPartsDataTable.fld_mapPic1Path].ToString();
                    entry_wagonParts.wMapPic2Path = Functions.IsNull(dt.Rows[i][wagonPartsDataTable.fld_mapPic2Path]) ? null : dt.Rows[i][wagonPartsDataTable.fld_mapPic2Path].ToString();
                    entry_wagonParts.wMapPic3Path = Functions.IsNull(dt.Rows[i][wagonPartsDataTable.fld_mapPic3Path]) ? null : dt.Rows[i][wagonPartsDataTable.fld_mapPic3Path].ToString();
                    entry_wagonParts.wMapPic4Path = Functions.IsNull(dt.Rows[i][wagonPartsDataTable.fld_mapPic4Path]) ? null : dt.Rows[i][wagonPartsDataTable.fld_mapPic4Path].ToString();

                    entry_wagonParts.wMapPicPath = Functions.IsNull(dt.Rows[i][wagonPartsDataTable.fld_mapPicPath]) ? null : dt.Rows[i][wagonPartsDataTable.fld_mapPicPath].ToString();
                    entry_wagonParts.wPartName = Functions.IsNull(dt.Rows[i][wagonPartsDataTable.fld_partName]) ? null : dt.Rows[i][wagonPartsDataTable.fld_partName].ToString();
                    entry_wagonParts.wPartNameLatin = Functions.IsNull(dt.Rows[i][wagonPartsDataTable.fld_partNameLatin]) ? null : dt.Rows[i][wagonPartsDataTable.fld_partNameLatin].ToString();
                    entry_wagonParts.wWeight = Functions.IsNull(dt.Rows[i][wagonPartsDataTable.fld_weight]) ? null : (decimal?)decimal.Parse(dt.Rows[i][wagonPartsDataTable.fld_weight].ToString());
                    if (add)
                        entityLogestic.rwmmsWagonParts.Add(entry_wagonParts);
                    else entityLogestic.Entry(entry_wagonParts).State = System.Data.Entity.EntityState.Modified;
                    try
                    {
                        entityLogestic.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
        }
    }

    public class wagonPartsGroupsDataTable : Model.htmlTable
    {
        public wagonPartsGroupsDataTable(int skipRowTop, int skipRowBottom, bool hasSerialNo) : base(skipRowTop, skipRowBottom, null)
        {
            this.Columns.Add(new Model.htmlColumn(fld_groupName, typeof(string), 0, null));
            this.Columns.Add(new Model.htmlColumn(fld_serialNo, typeof(string), (hasSerialNo ? (int?)1 : null), null));
            this.Columns.Add(new Model.htmlColumn(fld_psid, typeof(string), null, null));
            this.Columns.Add(new Model.htmlColumn(fld_id, typeof(string), null, null));

        }


        public static string fld_groupName = "groupName";
        public static string fld_serialNo = "serialNo";
        public static string fld_psid = "psid";
        public static string fld_id = "id";
    }

    public class wagonPartsDataTable : Model.htmlTable
    {
        public wagonPartsDataTable(int skipRowTop, int skipRowBottom, bool hasSerialNo) : base(skipRowTop, skipRowBottom, null)
        {
            this.Columns.Add(new Model.htmlColumn(fld_partName, typeof(string), 0, null));
            this.Columns.Add(new Model.htmlColumn(fld_partNameLatin, typeof(string), 1, null));
            this.Columns.Add(new Model.htmlColumn(fld_countInGroup, typeof(int), 2, null));
            this.Columns.Add(new Model.htmlColumn(fld_countInWagon, typeof(int), 3, null));
            this.Columns.Add(new Model.htmlColumn(fld_weight, typeof(float), 4, null));
            this.Columns.Add(new Model.htmlColumn(fld_mapNumber, typeof(string), 5, null));
            this.Columns.Add(new Model.htmlColumn(fld_mapPic1Path, typeof(string), null, null));
            this.Columns.Add(new Model.htmlColumn(fld_mapPic2Path, typeof(string), null, null));
            this.Columns.Add(new Model.htmlColumn(fld_mapPic3Path, typeof(string), null, null));
            this.Columns.Add(new Model.htmlColumn(fld_mapPic4Path, typeof(string), null, null));
            this.Columns.Add(new Model.htmlColumn(fld_mapPicPath, typeof(string), null, null));

            this.Columns.Add(new Model.htmlColumn(fld_groupIdParent, typeof(int), null, null));
        }


        public static string fld_partName = "partName";
        public static string fld_partNameLatin = "partNameLatin";
        public static string fld_countInGroup = "countInGroup";
        public static string fld_countInWagon = "countInWagon";
        public static string fld_weight = "weight";
        public static string fld_mapNumber = "mapNumber";
        public static string fld_mapPicPath = "mapPicPath";
        public static string fld_mapPic1Path = "mapPic1Path";
        public static string fld_mapPic2Path = "mapPic2Path";
        public static string fld_mapPic3Path = "mapPic3Path";
        public static string fld_mapPic4Path = "mapPic4Path";

        public static string fld_groupIdParent = "groupIdParent";
    }
}

