using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.rwmms
{
    public class webpageWagonRepairSpecialList : model.webpage
    {
        public webpageWagonRepairSpecialList() : this("http://rwmms.rai.ir/SpecialRepairList.aspx")
        {
            this.sb_createTables();
        }
        public webpageWagonRepairSpecialList(string url) : base(url)
        {
            this.sb_createTables();
        }
        public webpageWagonRepairSpecialList(int parentId) : base(parentId)
        {
            this.sb_createTables();
        }

        public webpageWagonRepairSpecialList(string url, ChromeDriver webBrowser) : base(url, webBrowser)
        {
            this.sb_createTables();
        }
        public webpageWagonRepairSpecialList(int parentId, ChromeDriver webBrowser) : base(parentId, webBrowser)
        {
            this.sb_createTables();
        }

        private void sb_createTables()
        {
            this.prp_tables = new List<model.htmlTable>();
            model.htmlTable tbl;

            #region RepairContractorsCombo
            tbl = new model.htmlTable("rwmmsRepairContractors", this, "Id", null, By.Id("ContentPlaceHolder1_repCombo_DropDown"));
            tbl.prp_identifierColumnNames = new string[] { "contractorName" };
            tbl.prp_skipRowTop = 1;
            tbl.Columns.Add(new model.htmlControlCombo("contractorName", typeof(string), "text", "alternateNames"));
            tbl.Columns.Add(new model.htmlControlCombo("contractorId", typeof(string), "value", null));
            this.prp_tables.Add(tbl);
            #endregion

            #region RepairRepairRegion
            tbl = new model.htmlTable("rwmmsRepairRegions", this, "Id", null, By.Id("ContentPlaceHolder1_regCombo_cmbRegion"));
            tbl.prp_identifierColumnNames = new string[] { "regionName" };
            tbl.prp_skipRowTop = 1;
            tbl.Columns.Add(new model.htmlControlCombo("regionName", typeof(string), "text", "alternateNames"));
            tbl.Columns.Add(new model.htmlControlCombo("regionId", typeof(string), "value", null));
            this.prp_tables.Add(tbl);
            #endregion

            #region rwmmsRepairVisitPosts
            tbl = new model.htmlTable("rwmmsRepairVisitPosts", this, "Id", null, By.Id("ContentPlaceHolder1_cboStation"));
            tbl.prp_identifierColumnNames = new string[] { "visitPostName" };
            tbl.prp_skipRowTop = 1;
            tbl.Columns.Add(new model.htmlControlCombo("visitPostName", typeof(string), "text", "alternateNames"));
            tbl.Columns.Add(new model.htmlControlCombo("visitPostId", typeof(string), "value", null));
            this.prp_tables.Add(tbl);
            #endregion

            #region rwmmsPostControllers
            tbl = new model.htmlTable("rwmmsPostControllers", this, "Id", null, By.Id("ContentPlaceHolder1_cboSurvey_DropDown"));
            tbl.prp_identifierColumnNames = new string[] { "postControllerName" };
            tbl.prp_skipRowTop = 1;
            tbl.Columns.Add(new model.htmlControlCombo("postControllerName", typeof(string), "text", "alternateNames"));
            tbl.Columns.Add(new model.htmlControlCombo("postControllerId", typeof(string), "value", null));
            this.prp_tables.Add(tbl);
            #endregion

            #region RepairVehicleOwners
            tbl = new model.htmlTable("rwmmsVehicleOwners", this, "Id", null, By.Id("ContentPlaceHolder1_cboOwner_DropDown"));
            tbl.prp_identifierColumnNames = new string[] { "companyName" };
            tbl.prp_skipRowTop = 1;
            tbl.Columns.Add(new model.htmlControlCombo("companyName", typeof(string), "text", "alternateNames"));
            tbl.Columns.Add(new model.htmlControlCombo("wCompanyId", typeof(string), "value", null));
            this.prp_tables.Add(tbl);
            #endregion

            #region Repairs
            tbl = new model.htmlTable("rwmmsRepairsSpecial", this, "Id", By.Id("ContentPlaceHolder1_dgSpecialRepair"), null);
            tbl.prp_pageIndexColumnName = "wrPageIndex";
            tbl.prp_controlLoadBy = By.Id("ContentPlaceHolder1_cmdSearch");
            tbl.prp_rowCountBy = By.Id("ContentPlaceHolder1_lblRecordCount");

            tbl.prp_identifierColumnNames = new string[] { "wrWagonNo", "wrSolarDateEntrance" };
            tbl.prp_pageIndexIdentifierColumnNames = new string[] { "vehicleOwnerName" };

            tbl.prp_pageRowCount = 5;
            tbl.prp_skipRowBottom = 1;
            tbl.prp_skipRowTop = 1;

            tbl.Columns.Add(new model.htmlControlComboSelectedItem("wrOwnerName", typeof(string), By.Id("ContentPlaceHolder1_cboOwner_DropDown"), "text", null));
            tbl.Columns.Add(new model.htmlControlComboSelectedItem("wrOwnerValue", typeof(string), By.Id("ContentPlaceHolder1_cboOwner_DropDown"), "value", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrWagonNo", typeof(string), 1, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrDetachSolarDate", typeof(string), 2, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrEntranceSolarDate", typeof(string), 3, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrEndSolarDate", typeof(string), 4, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrRepairContractor", typeof(string), 5, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrRepairRegion", typeof(string), 6, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrVisitPost", typeof(string), 7, "text", null));

            //tbl.Columns.Add(new model.htmlControlGridDownloadColumn("wrImage105Url", typeof(string),10,));

            tbl.Columns.Add(new model.columnComputedReference("gOwnerId", typeof(int), "rwmmsVehicleOwners", "companyName", "alternateNames", "Id", "wrOwnerName"));
            tbl.Columns.Add(new model.columnComputedReference("gWagonId", typeof(int), "Wagons", "wagonNo", "Id", "wrWagonNo"));
            tbl.Columns.Add(new model.columnComputedDateTime("gDetachDate", typeof(DateTime), "wrDetachSolarDate", model.columnComputedDateTime.enumDateTimeComputeType.solarDateToGregorianDate));
            tbl.Columns.Add(new model.columnComputedDateTime("gEntranceDate", typeof(DateTime), "wrEntranceSolarDate", model.columnComputedDateTime.enumDateTimeComputeType.solarDateToGregorianDate));
            tbl.Columns.Add(new model.columnComputedDateTime("gEndDate", typeof(DateTime), "wrEndSolarDate", model.columnComputedDateTime.enumDateTimeComputeType.solarDateToGregorianDate));

            tbl.Columns.Add(new model.columnComputedReference("gRepairContractorId", typeof(int), "rwmmsRepairContractors", "contractorName", "alternateNames", "Id", "wrRepairContractor"));
            tbl.Columns.Add(new model.columnComputedReference("gRepairRegionId", typeof(int), "rwmmsRepairRegions", "regionName", "alternateNames", "Id", "wrRepairRegion"));
            tbl.Columns.Add(new model.columnComputedReference("gVisitPostId", typeof(int), "rwmmsRepairVisitPosts", "visitPostName", "alternateNames", "Id", "wrVisitPost"));

            
            tbl.Columns.Add(new model.htmlControl("wrPageIndex", typeof(int), By.XPath("/html/body/form/div[3]/table/tbody/tr[4]/td/table/tbody/tr[7]/td/span"), "text", null));


            tbl.Columns.Add(new model.htmlControlGridDetailColumn("colEdit", typeof(string), 20, typeof(webpageWagonRepairInfoListDetail), false, true, true));
            this.prp_tables.Add(tbl);
            #endregion

        }
    }
}
