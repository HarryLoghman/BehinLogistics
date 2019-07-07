using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.rwmms
{
    public class webpageWagonRepairCurrentDetailParts : model.webpage
    {
        //public webpageWagonRepairCurrentDetail() : this("http://rwmms.rai.ir/CurrentRepair3.aspx")
        //{
        //    this.sb_createTables();
        //}
        public webpageWagonRepairCurrentDetailParts(string url) : base(url)
        {
            this.sb_createTables();
        }
        public webpageWagonRepairCurrentDetailParts(int parentId) : base(parentId)
        {
            this.sb_createTables();
        }

        public webpageWagonRepairCurrentDetailParts(string url, ChromeDriver webBrowser) : base(url, webBrowser)
        {
            this.sb_createTables();
        }
        public webpageWagonRepairCurrentDetailParts(int parentId, ChromeDriver webBrowser) : base(parentId, webBrowser)
        {
            this.sb_createTables();
        }

        private void sb_createTables()
        {
            this.prp_tables = new List<model.htmlTable>();
            model.htmlTable tbl;

            #region rwmmsRepairCurrentsDetailParts
            tbl = new model.htmlTable("rwmmsRepairCurrentsDetailParts", this, "Id", By.Id("ContentPlaceHolder1_dgEquipmentUse"), null);
            //tbl.prp_pageIndexColumnName = "wrPageIndex";
            //tbl.prp_controlLoadBy = By.Id("ContentPlaceHolder1_cmdSearch");
            //tbl.prp_controlRowCountBy = By.Id("ContentPlaceHolder1_lblRecordCount");
            //tbl.prp_stopFetchingWhenGotToExistedRows = false;

            tbl.prp_identifierColumnNames = new string[] { "wrMapNo", "gParentRepairCurrentDetail" };
            //tbl.prp_pageIndexIdentifierColumnNames = new string[] { "vehicleOwnerName" };
            tbl.prp_pageIndexUsed = false;
            //tbl.prp_pageRowCount = 5;
            //tbl.prp_skipRowBottom = 1;
            tbl.prp_skipRowTop = 1;

            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrMapNo", typeof(string), 0, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrPartName", typeof(string), 1, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrCountNew", typeof(string), 2, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrCountOld", typeof(int), 3, "text", null));
            //tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrNavyControllerName", typeof(string), 5, "text", null));

            tbl.Columns.Add(new model.htmlControlParent("gParentRepairCurrentDetail", typeof(int)));
            //tbl.Columns.Add(new model.columnComputedReference("gPartId", typeof(int), "rwmmsWagonParts", "wPartName", null, "Id", "wrMapNo"));
            tbl.Columns.Add(new model.columnComputedDateTime("gFetchTime", typeof(DateTime), (string)null, model.columnComputedDateTime.enumDateTimeComputeType.CurrentGregorianDateTime));

            this.prp_tables.Add(tbl);
            #endregion

        }



    }
}
