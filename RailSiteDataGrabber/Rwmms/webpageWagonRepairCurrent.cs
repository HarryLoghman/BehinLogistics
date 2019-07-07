using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.rwmms
{
    public class webpageWagonRepairCurrent : model.webpage
    {
        public webpageWagonRepairCurrent() : this("http://rwmms.rai.ir/CurrentRepair1.aspx")
        {
            this.sb_createTables();
        }
        public webpageWagonRepairCurrent(string url) : base(url)
        {
            this.sb_createTables();
        }
        public webpageWagonRepairCurrent(int parentId) : base(parentId)
        {
            this.sb_createTables();
        }

        public webpageWagonRepairCurrent(string url, ChromeDriver webBrowser) : base(url, webBrowser)
        {
            this.sb_createTables();
        }
        public webpageWagonRepairCurrent(int parentId, ChromeDriver webBrowser) : base(parentId, webBrowser)
        {
            this.sb_createTables();
        }

        private void sb_createTables()
        {
            this.prp_tables = new List<model.htmlTable>();
            model.htmlTable tbl;

            //#region rwmmsRepairVisitPosts
            //tbl = new model.htmlTable("rwmmsRepairVisitPosts", this, "Id", null, By.Id("ContentPlaceHolder1_cboSurveyStation"));
            //tbl.prp_identifierColumnNames = new string[] { "visitPostName" };
            //tbl.prp_skipRowTop = 1;
            //tbl.Columns.Add(new model.htmlControlCombo("visitPostName", typeof(string), "text", "alternateNames"));
            //tbl.Columns.Add(new model.htmlControlCombo("visitPostId", typeof(string), "value", null));
            //this.prp_tables.Add(tbl);
            //#endregion

            //#region rwmmsPostControllers
            //tbl = new model.htmlTable("rwmmsPostControllers", this, "Id", null, By.Id("ContentPlaceHolder1_cboPersonelSurveyAssignee_DropDown"));
            //tbl.prp_identifierColumnNames = new string[] { "postControllerName" };
            //tbl.prp_skipRowTop = 1;
            //tbl.Columns.Add(new model.htmlControlCombo("postControllerName", typeof(string), "text", "alternateNames"));
            //tbl.Columns.Add(new model.htmlControlCombo("postControllerId", typeof(string), "value", null));
            //this.prp_tables.Add(tbl);
            //#endregion

            #region rwmmsRepairCurrents
            tbl = new model.htmlTable("rwmmsRepairCurrents", this, "Id", By.Id("ContentPlaceHolder1_dgSubContractor"), null);
            tbl.prp_pageIndexColumnName = "wrPageIndex";
            tbl.prp_controlLoadBy = By.Id("ContentPlaceHolder1_cmdSearch");
            tbl.prp_rowCountBy = By.Id("ContentPlaceHolder1_lblRecordCount");
            tbl.prp_stopFetchingWhenGotToExistedRows = false;

            tbl.prp_identifierColumnNames = new string[] { "wrVisitPost", "wrVisitSolarDate" };
            tbl.prp_pageIndexIdentifierColumnNames = new string[] { "vehicleOwnerName" };

            tbl.prp_pageRowCount = 5;
            tbl.prp_skipRowBottom = 1;
            tbl.prp_skipRowTop = 1;

            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrVisitPost", typeof(string), 1, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrVisitSolarDate", typeof(string), 2, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrRepairContractor", typeof(string), 3, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrPostControllerName", typeof(string), 4, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrNavyControllerName", typeof(string), 5, "text", null));
            
            tbl.Columns.Add(new model.columnComputedReference("gRepairContractorId", typeof(int), "rwmmsRepairContractors", "contractorName", "alternateNames", "Id", "wrRepairContractor"));
            tbl.Columns.Add(new model.columnComputedReference("gVisitPostId", typeof(int), "rwmmsRepairVisitPosts", "visitPostName", "alternateNames", "Id", "wrVisitPost"));
            tbl.Columns.Add(new model.columnComputedReference("gNavyControllerId", typeof(int), "rwmmsPostControllers", "postControllerName", "alternateNames", "Id", "wrNavyControllerName"));
            tbl.Columns.Add(new model.columnComputedReference("gPostControllerId", typeof(int), "rwmmsPostControllers", "postControllerName", "alternateNames", "Id", "wrPostControllerName"));
            tbl.Columns.Add(new model.columnComputedDateTime("gVisitDate", typeof(DateTime), "wrVisitSolarDate", model.columnComputedDateTime.enumDateTimeComputeType.solarDateToGregorianDateReverse));
            tbl.Columns.Add(new model.columnComputedDateTime("gFetchTime", typeof(DateTime), (string)null, model.columnComputedDateTime.enumDateTimeComputeType.CurrentGregorianDateTime));
            tbl.Columns.Add(new model.htmlControl("wrPageIndex", typeof(int), By.XPath("/html/body/form/div[3]/table/tbody/tr[4]/td/table/tbody/tr[7]/td/span"), "text", null));

            tbl.Columns.Add(new model.htmlControlGridDetailColumn("colEdit", typeof(string), 7, typeof(webpageWagonRepairCurrentDetail), true, false, false));
            this.prp_tables.Add(tbl);
            #endregion

        }
    }
}
