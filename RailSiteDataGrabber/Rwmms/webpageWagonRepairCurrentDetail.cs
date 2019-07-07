using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.rwmms
{
    public class webpageWagonRepairCurrentDetail : model.webpage
    {
        //public webpageWagonRepairCurrentDetail() : this("http://rwmms.rai.ir/CurrentRepair2.aspx")
        //{
        //    this.sb_createTables();
        //}
        public webpageWagonRepairCurrentDetail(string url) : base(url)
        {
            this.sb_createTables();
        }
        public webpageWagonRepairCurrentDetail(int parentId) : base(parentId)
        {
            this.sb_createTables();
        }

        public webpageWagonRepairCurrentDetail(string url, ChromeDriver webBrowser) : base(url, webBrowser)
        {
            this.sb_createTables();
        }
        public webpageWagonRepairCurrentDetail(int parentId, ChromeDriver webBrowser) : base(parentId, webBrowser)
        {
            this.sb_createTables();
        }

        private void sb_createTables()
        {
            this.prp_tables = new List<model.htmlTable>();
            model.htmlTable tbl;

            #region rwmmsRepairCurrentsExtraInfo
            tbl = new model.htmlTable("rwmmsRepairCurrents", this, "Id", null, null);
            tbl.prp_identifierColumnNames = new string[] { "Id" };
            tbl.Columns.Add(new model.htmlControlParent("Id", typeof(int), false));
            tbl.Columns.Add(new model.htmlControl("wrRepairRegion", typeof(string), By.Id("ContentPlaceHolder1_txtRegion"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrControllerCompanyName", typeof(string), By.Id("ContentPlaceHolder1_txtSurvey"), "value", null));

            this.prp_tables.Add(tbl);
            #endregion

            #region rwmmsRepairCurrentsDetail
            tbl = new model.htmlTable("rwmmsRepairCurrentsDetail", this, "Id", By.Id("ContentPlaceHolder1_dgTimeRepair"), null);
            //tbl.prp_pageIndexColumnName = "wrPageIndex";
            //tbl.prp_controlLoadBy = By.Id("ContentPlaceHolder1_cmdSearch");
            //tbl.prp_controlRowCountBy = By.Id("ContentPlaceHolder1_lblRecordCount");
            //tbl.prp_stopFetchingWhenGotToExistedRows = false;

            tbl.prp_identifierColumnNames = new string[] { "wrWagonNo","gParentRepairCurrentId" };
            //tbl.prp_pageIndexIdentifierColumnNames = new string[] { "vehicleOwnerName" };
            tbl.prp_pageIndexUsed = false;
            //tbl.prp_pageRowCount = 5;
            //tbl.prp_skipRowBottom = 1;
            tbl.prp_skipRowTop = 1;
            

            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrWagonNo", typeof(string), 1, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrSourceStationName", typeof(string), 2, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrDestinationStationName", typeof(string), 3, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrPartCount", typeof(int), 4, "text", null));
            //tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrNavyControllerName", typeof(string), 5, "text", null));

            tbl.Columns.Add(new model.htmlControlParent("gParentRepairCurrentId", typeof(int)));
            tbl.Columns.Add(new model.columnComputedReference("gWagonId", typeof(int), "Wagons", "wagonNo", null, "Id", "wrWagonNo"));
            tbl.Columns.Add(new model.columnComputedReference("gSourceStationId", typeof(int), "PWS0Stations", "name", "alternateNames", "Id", "wrSourceStationName"));
            tbl.Columns.Add(new model.columnComputedReference("gDestinationStationId", typeof(int), "PWS0Stations", "name", "alternateNames", "Id", "wrDestinationStationName"));
            tbl.Columns.Add(new model.columnComputedDateTime("gFetchTime", typeof(DateTime), (string)null, model.columnComputedDateTime.enumDateTimeComputeType.CurrentGregorianDateTime));

            tbl.Columns.Add(new model.htmlControlGridDetailColumn("colEdit", typeof(string), 6, typeof(webpageWagonRepairCurrentDetailParts), true, false, false));
            this.prp_tables.Add(tbl);
            #endregion

        }
    }
}
