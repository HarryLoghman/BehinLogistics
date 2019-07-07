using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.rwmms
{
    class declerationList : IDisposable
    {
        string v_url = "http://rwmms.rai.ir/DeclarationList.aspx";
        string v_tableId = "ContentPlaceHolder1_dgDeclaration";
        declerationListDataTable v_dt;
        ChromeDriverService v_webBrowserService;
        ChromeOptions v_webBrowserOptions;
        ChromeDriver v_webBrowser;
        string v_downloadPath;
        int v_pageCount;
        string v_buttonSearchId = "ContentPlaceHolder1_cmdSearch";
        string v_spanPageRowCountId = "ContentPlaceHolder1_lblRecordCount";
        public declerationList()
        {
            this.v_downloadPath = Environment.CurrentDirectory + "\\Files\\rwmms\\decleration\\";
            this.v_dt = new declerationListDataTable();

            this.v_webBrowserService = ChromeDriverService.CreateDefaultService();
            this.v_webBrowserService.HideCommandPromptWindow = true;

            this.v_webBrowserOptions = new ChromeOptions();
            this.v_webBrowserOptions.AddArgument("--headless");
            this.v_webBrowserOptions.AddUserProfilePreference("download.default_directory", this.v_downloadPath);
            this.v_webBrowser = new ChromeDriver(this.v_webBrowserService, this.v_webBrowserOptions);

            this.v_pageCount = seleniumDownloader.fnc_getPageCountWithRowCount(this.v_webBrowser, this.v_url, this.v_spanPageRowCountId, this.v_buttonSearchId, true);
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
        public void sb_readAndSaveToDB(int cycleNumber, string ownerName, string typeName, bool getLastItems, bool getDetail, bool getAttach, bool loginAndLogout)
        {
            if (loginAndLogout)
            {
                login lg = new login();
                lg.fnc_logoutWithSelenium(this.v_webBrowser);
                if (login.fnc_isLoginPage(this.v_webBrowser.Url, this.v_url))
                {
                    lg.fnc_loginWithSelenium(this.v_webBrowser);
                    this.v_webBrowser.Navigate().GoToUrl(this.v_url);
                    seleniumDownloader.sb_click(this.v_webBrowser, this.v_url, this.v_buttonSearchId, false);
                    SharedFunctions.sb_waitForReady(this.v_webBrowser);
                }
            }

            int pageIndex = 1;
            string html;
            int pageIndexStart = 1;
            if (getLastItems)
            {
                using (var entityLogestic = new Model.logesticEntities())
                {
                    int? maxPageNo = entityLogestic.rwmmsDeclerationLists.Where(o => o.wOwnerName == ownerName && o.wTypeName == typeName).Max(o => o.PageIndex);
                    if (maxPageNo.HasValue) pageIndexStart = maxPageNo.Value;
                    else pageIndexStart = 1;
                }
            }
            for (pageIndex = pageIndexStart; pageIndex <= this.v_pageCount; pageIndex++)
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
                    if (this.v_dt.Rows.Count > 0)
                    {
                        if (getDetail)
                        {
                            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> rows;
                            int i = this.v_dt.prp_skipRowTop;
                            do
                            //for (int i = this.v_dt.prp_skipRowTop; i <= rows.Count - this.v_dt.prp_skipRowBottom; i++)
                            {
                                rows = Functions.fnc_getSeleniumTableRows(this.v_webBrowser, this.v_tableId);
                                var cells = rows[i].FindElements(By.TagName("td"));
                                if (cells.Count >= 8)
                                {
                                    cells[8].Click();
                                    SharedFunctions.sb_waitForReady(this.v_webBrowser);
                                    Functions.sb_fillDataRowWithHtmlControls(this.v_webBrowser.PageSource, this.v_dt, this.v_dt.Rows[i - this.v_dt.prp_skipRowTop]);
                                    try
                                    {
                                        var btnCancel = this.v_webBrowser.FindElement(By.Id("ContentPlaceHolder1_cmdCancel"));
                                        btnCancel.Click();
                                        SharedFunctions.sb_waitForReady(this.v_webBrowser);
                                    }
                                    catch
                                    {
                                        return;
                                    }
                                }
                                i++;
                            } while (i < rows.Count - this.v_dt.prp_skipRowBottom);
                        }

                        if (getAttach)
                        {
                            this.sb_getAttach();
                        }
                        this.sb_saveToDB(this.v_dt, cycleNumber, pageIndex, ownerName, typeName, getDetail, getAttach);
                    }
                }
                else break;
                Program.logs.Info("End of :" + pageIndex);
            }
        }

        private void sb_getAttach()
        {
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> rows;
            int htmlRowIndex = this.v_dt.prp_skipRowTop;
            int dtRowIndex = 0;
            do
            //for (int i = this.v_dt.prp_skipRowTop; i <= rows.Count - this.v_dt.prp_skipRowBottom; i++)
            {
                rows = Functions.fnc_getSeleniumTableRows(this.v_webBrowser, this.v_tableId);
                var cells = rows[htmlRowIndex].FindElements(By.TagName("td"));
                if (cells.Count >= 7)
                {
                    cells[7].Click();
                    SharedFunctions.sb_waitForReady(this.v_webBrowser);
                    FileInfo myFile;
                    do
                    {
                        myFile = (new DirectoryInfo(this.v_downloadPath)).GetFiles("*.*").OrderByDescending(o => o.LastWriteTime).FirstOrDefault();

                        if (myFile == null || myFile.LastWriteTime < DateTime.Now.AddMinutes(-5))
                        {
                            //there is error
                            throw new Exception("File cannot be downloaded");
                        }
                        else if (myFile.Extension != ".crdownload" && myFile.Extension != ".tmp")
                        {

                            string fileName = "dec_" + this.v_dt.Rows[dtRowIndex][declerationListDataTable.fld_ownerName].ToString()
                                + "_" + this.v_dt.Rows[dtRowIndex][declerationListDataTable.fld_wagonNo].ToString();

                            char[] chArrInvalidFileChars = Path.GetInvalidFileNameChars();
                            char[] chArrInvalidPathChars = Path.GetInvalidPathChars();
                            fileName = string.Join("_", fileName.Split(chArrInvalidFileChars));
                            fileName = string.Join("_", fileName.Split(chArrInvalidPathChars));
                            fileName = myFile.DirectoryName + "\\" + fileName + myFile.Extension;
                            if (File.Exists(fileName))
                                File.Delete(fileName);
                            myFile.MoveTo(fileName);

                            this.v_dt.Rows[dtRowIndex][declerationListDataTable.fld_attachmentPath] = fileName;

                            break;
                        }
                    } while (myFile.Extension == ".crdownload" || myFile.Extension == ".tmp"/*wait till .crdownload or .tmp to change to .pdf*/);
                }
                htmlRowIndex++;
                dtRowIndex++;
            } while (htmlRowIndex < rows.Count - this.v_dt.prp_skipRowBottom);
        }
        private void sb_saveToDB(declerationListDataTable dt, int cycleNumber, int pageIndex, string ownerName, string typeName
            , bool getDetail, bool getAttachment)
        {

            if (dt == null)
            {
                return;
            }
            int i;
            bool add;
            long wagonNo;
            Model.rwmmsDeclerationList entry_declerationList;
            using (var entityLogestic = new Model.logesticEntities())
            {
                for (i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    add = false;
                    if (!Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_wagonNo]))
                    {
                        wagonNo = long.Parse(dt.Rows[i][declerationListDataTable.fld_wagonNo].ToString());
                        entry_declerationList = entityLogestic.rwmmsDeclerationLists.FirstOrDefault(o => o.wWagonNo == wagonNo
                        && o.wOwnerName == ownerName && o.wTypeName == typeName);
                        if (entry_declerationList == null)
                        {
                            add = true;
                            entry_declerationList = new Model.rwmmsDeclerationList();
                        }
                    }
                    else
                    {
                        entry_declerationList = new Model.rwmmsDeclerationList();
                        add = true;
                    }

                    entry_declerationList.CycleNumber = cycleNumber;
                    entry_declerationList.FetchTime = DateTime.Now;
                    entry_declerationList.PageIndex = pageIndex;
                    entry_declerationList.Source = this.v_url;
                    entry_declerationList.vehicleOwnerId = Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_ownerName]) ? null : vehicleOwners.Fnc_getVehicleOwnerId(dt.Rows[i][declerationListDataTable.fld_ownerName].ToString());
                    entry_declerationList.wDeclerationNo = Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_declerationNo]) ? null : dt.Rows[i][declerationListDataTable.fld_declerationNo].ToString();

                    entry_declerationList.wOwnerName = Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_ownerName]) ? null : dt.Rows[i][declerationListDataTable.fld_ownerName].ToString();
                    entry_declerationList.wOwnerRepresentativeName = Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_ownerRepresentativeName]) ? null : dt.Rows[i][declerationListDataTable.fld_ownerRepresentativeName].ToString();
                    entry_declerationList.wRowNo = Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_rowNo]) ? null : (int?)int.Parse(dt.Rows[i][declerationListDataTable.fld_rowNo].ToString());
                    entry_declerationList.wStatus = Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_status]) ? null : dt.Rows[i][declerationListDataTable.fld_status].ToString();
                    entry_declerationList.wWagonNo = Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_wagonNo]) ? null : (long?)long.Parse(dt.Rows[i][declerationListDataTable.fld_wagonNo].ToString());
                    entry_declerationList.wTypeName = Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_typeName]) ? null : dt.Rows[i][declerationListDataTable.fld_typeName].ToString();

                    if (getDetail)
                    {
                        entry_declerationList.issueDate = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_issueDate]) ? null : (DateTime?)Functions.solarToMiladi(dt.Rows[i][declerationListDataTable.fld_issueDate].ToString()));
                        entry_declerationList.wConfirmEdareBazargani = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_confirmEdareBazargani]) ? null : (bool?)(dt.Rows[i][declerationListDataTable.fld_confirmEdareBazargani]));
                        entry_declerationList.wConfirmEdareBazarganiDescription = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_confirmEdareBazargani]) ? null : dt.Rows[i][declerationListDataTable.fld_confirmEdareBazarganiDescription].ToString());
                        entry_declerationList.wConfirmEdareSeir = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_confirmEdareSeir]) ? null : (bool?)(dt.Rows[i][declerationListDataTable.fld_confirmEdareSeir]));
                        entry_declerationList.wConfirmEdareSeirDescription = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_confirmEdareSeirDescription]) ? null : dt.Rows[i][declerationListDataTable.fld_confirmEdareSeirDescription].ToString());

                        entry_declerationList.wConfirmEdareWagon = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_confirmEdareWagon]) ? null : (bool?)(dt.Rows[i][declerationListDataTable.fld_confirmEdareWagon]));
                        entry_declerationList.wConfirmEdareWagonDescription = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_confirmEdareWagonDescription]) ? null : dt.Rows[i][declerationListDataTable.fld_confirmEdareWagonDescription].ToString());

                        entry_declerationList.wConfirmFinal = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_confirmFinal]) ? null : (bool?)(dt.Rows[i][declerationListDataTable.fld_confirmFinal]));
                        entry_declerationList.wConfirmFinalDescription = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_confirmFinalDescription]) ? null : dt.Rows[i][declerationListDataTable.fld_confirmFinalDescription].ToString());

                        entry_declerationList.wConfirmHoghoghi = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_confirmHoghoghi]) ? null : (bool?)(dt.Rows[i][declerationListDataTable.fld_confirmHoghoghi]));
                        entry_declerationList.wConfirmHoghoghiDescription = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_confirmHoghoghi]) ? null : dt.Rows[i][declerationListDataTable.fld_confirmHoghoghiDescription].ToString());

                        entry_declerationList.wConfirmMali = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_confirmMali]) ? null : (bool?)(dt.Rows[i][declerationListDataTable.fld_confirmMali]));
                        entry_declerationList.wConfirmMaliDescription = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_confirmMaliDescription]) ? null : dt.Rows[i][declerationListDataTable.fld_confirmMaliDescription].ToString());

                        entry_declerationList.wConfirmOwner = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_confirmOwner]) ? null : (bool?)(dt.Rows[i][declerationListDataTable.fld_confirmOwner]));
                        entry_declerationList.wConfirmOwnerDescription = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_confirmOwnerDescription]) ? null : dt.Rows[i][declerationListDataTable.fld_confirmOwnerDescription].ToString());

                        entry_declerationList.wConfirmSarmaye = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_confirmSarmaye]) ? null : (bool?)(dt.Rows[i][declerationListDataTable.fld_confirmSarmaye]));
                        entry_declerationList.wConfirmSarmayeDescription = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_confirmSarmayeDescription]) ? null : dt.Rows[i][declerationListDataTable.fld_confirmSarmayeDescription].ToString());

                        entry_declerationList.wConfirmTajhiz = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_confirmTajhiz]) ? null : (bool?)(dt.Rows[i][declerationListDataTable.fld_confirmTajhiz]));
                        entry_declerationList.wConfirmTajhizDescription = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_confirmTajhizDescription]) ? null : dt.Rows[i][declerationListDataTable.fld_confirmTajhizDescription].ToString());

                        entry_declerationList.wIssueDate = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_issueDate]) ? null : dt.Rows[i][declerationListDataTable.fld_issueDate].ToString());
                        entry_declerationList.wNoteNo = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_noteNo]) ? null : dt.Rows[i][declerationListDataTable.fld_noteNo].ToString());
                        entry_declerationList.wOtherRights = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_otherRights]) ? null : dt.Rows[i][declerationListDataTable.fld_otherRights].ToString());
                        entry_declerationList.wOwnerDocuments = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_ownerDocuments]) ? null : dt.Rows[i][declerationListDataTable.fld_ownerDocuments].ToString());
                        entry_declerationList.wPageNo = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_pageNo]) ? null : dt.Rows[i][declerationListDataTable.fld_pageNo].ToString());
                        entry_declerationList.wRegisterNo = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_registerNo]) ? null : dt.Rows[i][declerationListDataTable.fld_registerNo].ToString());


                    }

                    if (getAttachment)
                    {
                        entry_declerationList.wAttachmentPath = (Functions.IsNull(dt.Rows[i][declerationListDataTable.fld_attachmentPath]) ? null : dt.Rows[i][declerationListDataTable.fld_attachmentPath].ToString());
                    }
                    if (add)
                        entityLogestic.rwmmsDeclerationLists.Add(entry_declerationList);
                    else entityLogestic.Entry(entry_declerationList).State = System.Data.Entity.EntityState.Modified;
                    entityLogestic.SaveChanges();

                    PWS0.trainBarryWagons.fnc_saveWagonFromRWMMS(dt, i);
                }
            }
        }
    }

    public class declerationListDataTable : Model.htmlTable
    {
        public declerationListDataTable() : base(1, 1, null)
        {
            this.Columns.Add(new Model.htmlColumn(fld_rowNo, typeof(int), 0, null));
            this.Columns.Add(new Model.htmlColumn(fld_wagonNo, typeof(long), 1, "ContentPlaceHolder1_txtWagonNo"));
            this.Columns.Add(new Model.htmlColumn(fld_typeName, typeof(string), 2, null));
            this.Columns.Add(new Model.htmlColumn(fld_declerationNo, typeof(string), 3, "ContentPlaceHolder1_txtIdentifier"));
            this.Columns.Add(new Model.htmlColumn(fld_ownerName, typeof(string), 4, "ContentPlaceHolder1_txtOwner"));
            this.Columns.Add(new Model.htmlColumn(fld_ownerRepresentativeName, typeof(string), 5, "ContentPlaceHolder1_txtOwnerDetail"));
            this.Columns.Add(new Model.htmlColumn(fld_status, typeof(string), 6, null));
            this.Columns.Add(new Model.htmlColumn(fld_issueDate, typeof(string), null, "ContentPlaceHolder1_txtIssueDate"));
            this.Columns.Add(new Model.htmlColumn(fld_noteNo, typeof(string), null, "ContentPlaceHolder1_txtNoteNo"));
            this.Columns.Add(new Model.htmlColumn(fld_registerNo, typeof(string), null, "ContentPlaceHolder1_txtRegisterNo"));
            this.Columns.Add(new Model.htmlColumn(fld_pageNo, typeof(string), null, "ContentPlaceHolder1_txtPageNo"));
            this.Columns.Add(new Model.htmlColumn(fld_docStatus, typeof(string), null, "ContentPlaceHolder1_txtDocStatus"));

            this.Columns.Add(new Model.htmlColumn(fld_wagonTypeName, typeof(string), null, "ContentPlaceHolder1_WagonSupInfoSpec1_txtfkb_noe_wagonTitle"));
            this.Columns.Add(new Model.htmlColumn(fld_wagonBrakeType, typeof(string), null, "ContentPlaceHolder1_WagonSupInfoSpec1_txtfkb_NoeTormozTitle"));
            this.Columns.Add(new Model.htmlColumn(fld_wagonChassisSerialNo, typeof(string), null, "ContentPlaceHolder1_WagonSupInfoSpec1_txtShasiNo"));
            this.Columns.Add(new Model.htmlColumn(fld_wagonRIVNo, typeof(string), null, "ContentPlaceHolder1_WagonSupInfoSpec1_txtRivNo"));
            this.Columns.Add(new Model.htmlColumn(fld_wagonCountry, typeof(string), null, "ContentPlaceHolder1_WagonSupInfoSpec1_txtfkb_keshvar_sazandeTitle"));
            this.Columns.Add(new Model.htmlColumn(fld_wagonBoogieType, typeof(string), null, "ContentPlaceHolder1_WagonSupInfoSpec1_txtfkb_NoeBogiTitle"));
            this.Columns.Add(new Model.htmlColumn(fld_wagonCompanyManufacturer, typeof(string), null, "ContentPlaceHolder1_WagonSupInfoSpec1_txtMms_Fld_BuilderCompany"));
            this.Columns.Add(new Model.htmlColumn(fld_wagonCapacity, typeof(double), null, "ContentPlaceHolder1_WagonSupInfoSpec1_txtVolume"));
            this.Columns.Add(new Model.htmlColumn(fld_wagonProductionYear, typeof(int), null, "ContentPlaceHolder1_WagonSupInfoSpec1_txtMms_Fld_sal_vorod"));
            this.Columns.Add(new Model.htmlColumn(fld_wagonAxisCount, typeof(int), null, "ContentPlaceHolder1_WagonSupInfoSpec1_txtMms_Fld_ted_mehvar"));
            this.Columns.Add(new Model.htmlColumn(fld_wagonHookType, typeof(string), null, "ContentPlaceHolder1_WagonSupInfoSpec1_txtfkb_NoeGholabTitle"));
            this.Columns.Add(new Model.htmlColumn(fld_wagonNetWeight, typeof(double), null, "ContentPlaceHolder1_WagonSupInfoSpec1_txtLoad"));


            this.Columns.Add(new Model.htmlColumn(fld_otherRights, typeof(string), null, "ContentPlaceHolder1_txtOwners", Model.htmlColumn.enum_controlType.textArea));
            this.Columns.Add(new Model.htmlColumn(fld_ownerDocuments, typeof(string), null, "ContentPlaceHolder1_txtOwnerDocument", Model.htmlColumn.enum_controlType.textArea));
            this.Columns.Add(new Model.htmlColumn(fld_description, typeof(string), null, "ContentPlaceHolder1_txtDescription", Model.htmlColumn.enum_controlType.textArea));
            this.Columns.Add(new Model.htmlColumn(fld_confirmOwner, typeof(bool), null, "ContentPlaceHolder1_chk1", Model.htmlColumn.enum_controlType.inputCheckBox));
            this.Columns.Add(new Model.htmlColumn(fld_confirmOwnerDescription, typeof(string), null, "ContentPlaceHolder1_txt1"));
            this.Columns.Add(new Model.htmlColumn(fld_confirmMali, typeof(bool), null, "ContentPlaceHolder1_chk2", Model.htmlColumn.enum_controlType.inputCheckBox));
            this.Columns.Add(new Model.htmlColumn(fld_confirmMaliDescription, typeof(string), null, "ContentPlaceHolder1_txt2"));
            this.Columns.Add(new Model.htmlColumn(fld_confirmEdareWagon, typeof(bool), null, "ContentPlaceHolder1_chk3", Model.htmlColumn.enum_controlType.inputCheckBox));
            this.Columns.Add(new Model.htmlColumn(fld_confirmEdareWagonDescription, typeof(string), null, "ContentPlaceHolder1_txt3"));
            this.Columns.Add(new Model.htmlColumn(fld_confirmEdareBazargani, typeof(bool), null, "ContentPlaceHolder1_chk5", Model.htmlColumn.enum_controlType.inputCheckBox));
            this.Columns.Add(new Model.htmlColumn(fld_confirmEdareBazarganiDescription, typeof(string), null, "ContentPlaceHolder1_txt5"));
            this.Columns.Add(new Model.htmlColumn(fld_confirmEdareSeir, typeof(bool), null, "ContentPlaceHolder1_chk6", Model.htmlColumn.enum_controlType.inputCheckBox));
            this.Columns.Add(new Model.htmlColumn(fld_confirmEdareSeirDescription, typeof(string), null, "ContentPlaceHolder1_txt6"));
            this.Columns.Add(new Model.htmlColumn(fld_confirmSarmaye, typeof(bool), null, "ContentPlaceHolder1_chk7", Model.htmlColumn.enum_controlType.inputCheckBox));
            this.Columns.Add(new Model.htmlColumn(fld_confirmSarmayeDescription, typeof(string), null, "ContentPlaceHolder1_txt7"));
            this.Columns.Add(new Model.htmlColumn(fld_confirmTajhiz, typeof(bool), null, "ContentPlaceHolder1_chk4", Model.htmlColumn.enum_controlType.inputCheckBox));
            this.Columns.Add(new Model.htmlColumn(fld_confirmTajhizDescription, typeof(string), null, "ContentPlaceHolder1_txt4"));
            this.Columns.Add(new Model.htmlColumn(fld_confirmHoghoghi, typeof(bool), null, "ContentPlaceHolder1_chk8", Model.htmlColumn.enum_controlType.inputCheckBox));
            this.Columns.Add(new Model.htmlColumn(fld_confirmHoghoghiDescription, typeof(string), null, "ContentPlaceHolder1_txt8"));
            this.Columns.Add(new Model.htmlColumn(fld_confirmFinal, typeof(bool), null, "ContentPlaceHolder1_chk9", Model.htmlColumn.enum_controlType.inputCheckBox));
            this.Columns.Add(new Model.htmlColumn(fld_confirmFinalDescription, typeof(string), null, "ContentPlaceHolder1_txt9"));


            this.Columns.Add(new Model.htmlColumn(fld_attachmentPath, typeof(string), null, null));

        }


        public static string fld_rowNo = "rowNo";
        public static string fld_wagonNo = "wagonNo";
        public static string fld_typeName = "typeName";
        public static string fld_declerationNo = "declerationNo";
        public static string fld_ownerName = "ownerName";
        public static string fld_ownerRepresentativeName = "ownerRepresentativeName";
        public static string fld_status = "status";
        public static string fld_issueDate = "issueDate";
        public static string fld_noteNo = "noteNo";
        public static string fld_registerNo = "registerNo";
        public static string fld_pageNo = "pageNo";
        public static string fld_docStatus = "docStatus";
        public static string fld_wagonTypeName = "wagonTypeName";
        public static string fld_wagonBrakeType = "wagonBrakeType";
        public static string fld_wagonChassisSerialNo = "wagonChassisSerialNo";
        public static string fld_wagonRIVNo = "wagonRIVNo";
        public static string fld_wagonCountry = "wagonCountry";
        public static string fld_wagonBoogieType = "wagonBoogieType";
        public static string fld_wagonCompanyManufacturer = "wagonCompanyManufacturer";
        public static string fld_wagonCapacity = "wagonCapacity";
        public static string fld_wagonProductionYear = "wagonProductionYear";
        public static string fld_wagonAxisCount = "wagonAxisCount";
        public static string fld_wagonHookType = "wagonHookType";
        public static string fld_wagonNetWeight = "wagonNetWeight";

        public static string fld_otherRights = "otherRights";
        public static string fld_ownerDocuments = "ownerDocuments";
        public static string fld_description = "description";

        public static string fld_confirmOwner = "confirmOwner";
        public static string fld_confirmOwnerDescription = "confirmOwnerDescription";
        public static string fld_confirmMali = "confirmMali";
        public static string fld_confirmMaliDescription = "confirmMaliDescription";
        public static string fld_confirmEdareWagon = "confirmEdareWagon";
        public static string fld_confirmEdareWagonDescription = "confirmEdareWagonDescription";
        public static string fld_confirmEdareBazargani = "confirmEdareBazargani";
        public static string fld_confirmEdareBazarganiDescription = "confirmEdareBazarganiDescription";
        public static string fld_confirmEdareSeir = "confirmEdareSeir";
        public static string fld_confirmEdareSeirDescription = "confirmEdareSeirDescription";
        public static string fld_confirmSarmaye = "confirmSarmaye";
        public static string fld_confirmSarmayeDescription = "confirmSarmayeDescription";
        public static string fld_confirmTajhiz = "confirmTajhiz";
        public static string fld_confirmTajhizDescription = "confirmTajhizDescription";
        public static string fld_confirmHoghoghi = "confirmHoghoghi";
        public static string fld_confirmHoghoghiDescription = "confirmHoghoghiDescription";
        public static string fld_confirmFinal = "confirmFinal";
        public static string fld_confirmFinalDescription = "confirmFinalDescription";


        public static string fld_attachmentPath = "attachementPath";

    }
}
