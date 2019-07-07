using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.rwmms
{
    class vehicleOwnersDetail
    {
        string v_url = "http://rwmms.rai.ir/VehicleOwnerDetailInfoList.aspx";
        string v_tableId = "ContentPlaceHolder1_dgVehicleOwnerDetails";
        vehicleOwnersDetailDataTable v_dtOwners;
        FirefoxDriverService v_webBrowserService;
        FirefoxOptions v_webBrowserOptions;
        FirefoxDriver v_webBrowser;
        int v_pageCount;
        public vehicleOwnersDetail()
        {
            this.v_dtOwners = new vehicleOwnersDetailDataTable();

            this.v_webBrowserService = FirefoxDriverService.CreateDefaultService();
            this.v_webBrowserService.HideCommandPromptWindow = true;

            this.v_webBrowserOptions = new FirefoxOptions();
            this.v_webBrowserOptions.AddArgument("--headless");
            this.v_webBrowser = new FirefoxDriver(this.v_webBrowserService, this.v_webBrowserOptions);

            this.v_pageCount = this.fnc_getPageCount(true);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="logoutAndLogin">logout current webDriver and login again to see all owners</param>
        /// <returns></returns>
        public int fnc_getPageCount(bool logoutAndLogin)
        {
            login lg = new login();
            if (logoutAndLogin)
                lg.fnc_logoutWithSelenium(this.v_webBrowser);
            if (login.fnc_isLoginPage(this.v_webBrowser.Url, this.v_url))
                lg.fnc_loginWithSelenium(this.v_webBrowser);

            this.v_webBrowser.Navigate().GoToUrl(this.v_url);
            SharedFunctions.sb_waitForReady(this.v_webBrowser);
            string html = this.v_webBrowser.PageSource;
            int lastPageIndex = 0;
            var table = Functions.fnc_getElementById(html, this.v_tableId);
            if (table != null)
            {
                var tbody = table.SelectSingleNode("tbody");
                var rows = tbody.SelectNodes("tr");
                if (rows != null && rows.Count > 0)
                {
                    var lastRow = rows[rows.Count - 1];
                    var td = lastRow.SelectSingleNode("td");
                    var hrefCollection = td.SelectNodes("a");
                    if (hrefCollection != null && hrefCollection.Count > 0)
                    {
                        var pageIndexStr = hrefCollection[hrefCollection.Count - 1].InnerText;
                        int temp;
                        if (int.TryParse(pageIndexStr, out temp))
                        {
                            lastPageIndex = temp;
                        }
                    }
                }
            }
            return lastPageIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="logoutAndLogin">logout current webDriver and login again to see all owners</param>
        public int fnc_gotoPage(int pageIndex, bool logoutAndLogin, bool waitTillReady = true)
        {

            login lg = new login();
            if (logoutAndLogin)
                lg.fnc_logoutWithSelenium(this.v_webBrowser);
            if (login.fnc_isLoginPage(this.v_webBrowser.Url, this.v_url))
                lg.fnc_loginWithSelenium(this.v_webBrowser);

            if (this.v_pageCount == -1)
            {
                this.v_pageCount = this.fnc_getPageCount(false);
            }
            if (pageIndex > this.v_pageCount) return -1;

            if (pageIndex == 0)
            {
                this.v_webBrowser.Navigate().GoToUrl(this.v_url);
                if (waitTillReady)
                    SharedFunctions.sb_waitForReady(this.v_webBrowser);
                return pageIndex;
            }
            else
            {
                this.v_webBrowser.Navigate().GoToUrl(this.v_url);
                SharedFunctions.sb_waitForReady(this.v_webBrowser);
                var table = this.v_webBrowser.FindElementById(v_tableId);
                if (table != null)
                {
                    var rows = table.FindElements(By.TagName("tr"));
                    if (rows != null && rows.Count > 0)
                    {
                        var lastRow = rows[rows.Count - 1];
                        var td = lastRow.FindElement(By.TagName("td"));
                        if (td != null)
                        {
                            var hrefCollection = td.FindElements(By.TagName("a"));
                            if (pageIndex <= hrefCollection.Count)
                            {
                                hrefCollection[pageIndex - 1].Click();
                                if (waitTillReady)
                                    SharedFunctions.sb_waitForReady(this.v_webBrowser);
                                return pageIndex;
                            }
                            else return -1;
                        }
                        else return -1;
                    }
                    else return -1;
                }
                else return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cycleNumber"></param>
        /// <param name="getExtraDetail">getExtraDetail like fathername and ....</param>
        public void sb_readAllAndSaveToDB(int cycleNumber, bool getExtraDetail)
        {
            login lg = new login();
            lg.fnc_logoutWithSelenium(this.v_webBrowser);
            if (login.fnc_isLoginPage(this.v_webBrowser.Url, this.v_url))
                lg.fnc_loginWithSelenium(this.v_webBrowser);


            int pageIndex = 0;
            string html;
            bool logoutAndLogin = getExtraDetail;

            for (pageIndex = 0; pageIndex <= this.v_pageCount - 1; pageIndex++)
            {
                //if we should get extraDetail we should logout and then login again
                pageIndex = this.fnc_gotoPage(pageIndex, logoutAndLogin);
                if (pageIndex == -1) return;

                html = this.v_webBrowser.PageSource;

                if (!string.IsNullOrEmpty(html))
                {
                    this.v_dtOwners.Rows.Clear();
                    Functions.sb_fillDatatableWithHtmlTableId(html, v_tableId, 1, 1, 0, 0, this.v_dtOwners);
                    if (this.v_dtOwners.Rows.Count > 0)
                    {
                        this.sb_saveOwnersDetailsToDB(this.v_dtOwners, cycleNumber);
                    }
                    if (getExtraDetail)
                    {

                        this.sb_readExtraDetailAndSaveToDB(cycleNumber, pageIndex);
                    }
                }
                else break;
            }
        }

        private void sb_readExtraDetailAndSaveToDB(int cycleNumber, int pageIndex)
        {
            int rowIndex = 1;
            string htmlEditPage;
            nextRecord: pageIndex = this.fnc_gotoPage(pageIndex, true);
            if (pageIndex == -1) return;

            var table = this.v_webBrowser.FindElementById(v_tableId);
            if (table != null)
            {
                var rows = table.FindElements(By.TagName("tr"));
                if (rows != null && rows.Count > 0 && rowIndex <= rows.Count - 2)
                {//first and last row are header and footer
                    using (var entityLogestic = new Model.logesticEntities())
                    {
                        var columns = rows[rowIndex].FindElements(By.TagName("td"));
                        if (columns.Count >= 6)
                        {
                            var href = columns[6].FindElement(By.TagName("a"));
                            if (href != null)
                            {
                                href.Click();
                                SharedFunctions.sb_waitForReady(this.v_webBrowser);
                                htmlEditPage = this.v_webBrowser.PageSource;
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

                                if (!string.IsNullOrEmpty(meliNo))
                                {
                                    var entryOwnerExtraDetail = entityLogestic.rwmmsVehicleOwnersDetails.FirstOrDefault(o => o.wMeliNo == meliNo);
                                    if (entryOwnerExtraDetail != null)
                                    {
                                        //entryOwnerExtraDetail.companyId
                                        entryOwnerExtraDetail.CycleNumber = cycleNumber;
                                        entryOwnerExtraDetail.FetchTime = DateTime.Now;
                                        entryOwnerExtraDetail.Source = "http://rwmms.rai.ir/VehicleOwnerDetailInfo.aspx";
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
                                        entityLogestic.Entry(entryOwnerExtraDetail).State = System.Data.Entity.EntityState.Modified;
                                        entityLogestic.SaveChanges();
                                    }
                                }

                                rowIndex++;
                                goto nextRecord;
                            }
                        }

                    }
                }
            }
        }

        private void sb_saveOwnersDetailsToDB(DataTable dt, int cycleNumber)
        {
            if (dt == null)
            {
                return;
            }
            int i;

            Model.rwmmsVehicleOwnersDetail entry_vehicleOwnersDetail;
            bool add;
            string meliNo;
            using (var entityLogestic = new Model.logesticEntities())
            {
                for (i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    add = false;
                    if (!Functions.IsNull(dt.Rows[i][vehicleOwnersDetailDataTable.fld_meliNo]))
                    {
                        meliNo = dt.Rows[i][vehicleOwnersDetailDataTable.fld_meliNo].ToString();
                        entry_vehicleOwnersDetail = entityLogestic.rwmmsVehicleOwnersDetails.FirstOrDefault(o => o.wMeliNo == meliNo);
                        if (entry_vehicleOwnersDetail == null)
                        {
                            add = true;
                            entry_vehicleOwnersDetail = new Model.rwmmsVehicleOwnersDetail();
                        }
                    }
                    else
                    {
                        entry_vehicleOwnersDetail = new Model.rwmmsVehicleOwnersDetail();
                        add = true;
                    }
                    //entry_vehicleOwnersDetail.companyId = companyId;
                    entry_vehicleOwnersDetail.CycleNumber = cycleNumber;
                    entry_vehicleOwnersDetail.FetchTime = DateTime.Now;
                    entry_vehicleOwnersDetail.Source = "rwmms";
                    entry_vehicleOwnersDetail.wEmail = Functions.IsNull(dt.Rows[i][vehicleOwnersDetailDataTable.fld_email]) ? null : dt.Rows[i][vehicleOwnersDetailDataTable.fld_email].ToString();
                    entry_vehicleOwnersDetail.wFName = Functions.IsNull(dt.Rows[i][vehicleOwnersDetailDataTable.fld_fname]) ? null : dt.Rows[i][vehicleOwnersDetailDataTable.fld_fname].ToString();
                    entry_vehicleOwnersDetail.wLName = Functions.IsNull(dt.Rows[i][vehicleOwnersDetailDataTable.fld_lname]) ? null : dt.Rows[i][vehicleOwnersDetailDataTable.fld_lname].ToString();
                    entry_vehicleOwnersDetail.wMeliNo = Functions.IsNull(dt.Rows[i][vehicleOwnersDetailDataTable.fld_meliNo]) ? null : dt.Rows[i][vehicleOwnersDetailDataTable.fld_meliNo].ToString();
                    entry_vehicleOwnersDetail.wMobileNumber = Functions.IsNull(dt.Rows[i][vehicleOwnersDetailDataTable.fld_mobileNumber]) ? null : dt.Rows[i][vehicleOwnersDetailDataTable.fld_mobileNumber].ToString();
                    if (add)
                        entityLogestic.rwmmsVehicleOwnersDetails.Add(entry_vehicleOwnersDetail);
                    else entityLogestic.Entry(entry_vehicleOwnersDetail).State = System.Data.Entity.EntityState.Modified;
                    entityLogestic.SaveChanges();
                }
            }
        }
    }

    public class vehicleOwnersDetailDataTable : DataTable
    {
        public vehicleOwnersDetailDataTable()
        {
            this.Columns.Add(new DataColumn(fld_rowNo, typeof(string)));
            this.Columns.Add(new DataColumn(fld_fname, typeof(string)));
            this.Columns.Add(new DataColumn(fld_lname, typeof(string)));
            this.Columns.Add(new DataColumn(fld_meliNo, typeof(string)));
            this.Columns.Add(new DataColumn(fld_email, typeof(string)));
            this.Columns.Add(new DataColumn(fld_mobileNumber, typeof(string)));
        }


        public static string fld_rowNo = "rowNo";
        public static string fld_fname = "fname";
        public static string fld_lname = "lname";
        public static string fld_meliNo = "meliNo";
        public static string fld_email = "email";
        public static string fld_mobileNumber = "mobileNumber";

    }
}
