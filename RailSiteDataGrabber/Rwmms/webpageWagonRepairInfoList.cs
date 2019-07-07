using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.rwmms
{
    public class webpageWagonRepairInfoList : model.webpage
    {
        public webpageWagonRepairInfoList() : this("http://rwmms.rai.ir/WagonRepairInfoList.aspx")
        {
            this.sb_createTables();
        }
        public webpageWagonRepairInfoList(string url) : base(url)
        {
            this.sb_createTables();
        }
        public webpageWagonRepairInfoList(int parentId) : base(parentId)
        {
            this.sb_createTables();
        }

        public webpageWagonRepairInfoList(string url, ChromeDriver webBrowser) : base(url, webBrowser)
        {
            this.sb_createTables();
        }
        public webpageWagonRepairInfoList(int parentId, ChromeDriver webBrowser) : base(parentId, webBrowser)
        {
            this.sb_createTables();
        }

        private void sb_createTables()
        {
            this.prp_tables = new List<model.htmlTable>();
            model.htmlTable tbl;

            //#region RepairContractorsCombo
            //tbl = new model.htmlTable("rwmmsRepairContractors", this, "Id", null, By.Id("ContentPlaceHolder1_repCombo_DropDown"));
            //tbl.prp_identifierColumnNames = new string[] { "contractorName" };
            //tbl.prp_skipRowTop = 1;
            //tbl.Columns.Add(new model.htmlControlCombo("contractorName", typeof(string), "text", "alternateNames"));
            //tbl.Columns.Add(new model.htmlControlCombo("contractorId", typeof(string), "value", null));
            //this.prp_tables.Add(tbl);
            //#endregion

            //#region RepairRepairRegion
            //tbl = new model.htmlTable("rwmmsRepairRegions", this, "Id", null, By.Id("ContentPlaceHolder1_regCombo_cmbRegion"));
            //tbl.prp_identifierColumnNames = new string[] { "regionName" };
            //tbl.prp_skipRowTop = 1;
            //tbl.Columns.Add(new model.htmlControlCombo("regionName", typeof(string), "text", "alternateNames"));
            //tbl.Columns.Add(new model.htmlControlCombo("regionId", typeof(string), "value", null));
            //this.prp_tables.Add(tbl);
            //#endregion

            //#region RepairRepairType
            //tbl = new model.htmlTable("rwmmsRepairTypes", this, "Id", null, By.Id("ContentPlaceHolder1_cboRepairKind"));
            //tbl.prp_identifierColumnNames = new string[] { "typeName" };
            //tbl.prp_skipRowTop = 1;
            //tbl.Columns.Add(new model.htmlControlCombo("typeName", typeof(string), "text", "alternateNames"));
            //tbl.Columns.Add(new model.htmlControlCombo("typeId", typeof(string), "value", null));
            //this.prp_tables.Add(tbl);
            //#endregion

            //#region RepairController
            //tbl = new model.htmlTable("rwmmsRepairControllers", this, "Id", null, By.Id("ContentPlaceHolder1_cboController_DropDown"));
            //tbl.prp_identifierColumnNames = new string[] { "controllerName" };
            //tbl.prp_skipRowTop = 1;
            //tbl.Columns.Add(new model.htmlControlCombo("controllerName", typeof(string), "text", "alternateNames"));
            //tbl.Columns.Add(new model.htmlControlCombo("controllerId", typeof(string), "value", null));
            //this.prp_tables.Add(tbl);
            //#endregion

            //#region RepairDelayType
            //tbl = new model.htmlTable("rwmmsRepairDelayTypes", this, "Id", null, By.Id("ContentPlaceHolder1_cboDelayReason"));
            //tbl.prp_identifierColumnNames = new string[] { "typeName" };
            //tbl.prp_skipRowTop = 1;
            //tbl.Columns.Add(new model.htmlControlCombo("typeName", typeof(string), "text", "alternateNames"));
            //tbl.Columns.Add(new model.htmlControlCombo("typeId", typeof(string), "value", null));
            //this.prp_tables.Add(tbl);
            //#endregion

            //#region RepairVehicleOwners
            //tbl = new model.htmlTable("rwmmsVehicleOwners", this, "Id", null, By.Id("ContentPlaceHolder1_cboOwner_DropDown"));
            //tbl.prp_identifierColumnNames = new string[] { "companyName" };
            //tbl.prp_skipRowTop = 1;
            //tbl.Columns.Add(new model.htmlControlCombo("companyName", typeof(string), "text", "alternateNames"));
            //tbl.Columns.Add(new model.htmlControlCombo("wCompanyId", typeof(string), "value", null));
            //this.prp_tables.Add(tbl);
            //#endregion

            #region Repairs
            tbl = new model.htmlTable("rwmmsRepairs", this, "Id", By.Id("ContentPlaceHolder1_dgWagonRepair"), null);
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
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrSolarDateEntrance", typeof(string), 2, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrSolarDateExit", typeof(string), 3, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrSolarDatePlanOwner", typeof(string), 4, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrSolarDateStencil", typeof(string), 5, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrRepairType", typeof(string), 6, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrRepairRegion", typeof(string), 7, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrRepairContractor", typeof(string), 8, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrRepairController", typeof(string), 9, "text", null));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrConfirmRajaStr", typeof(string), 10, "text", null, false));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrConfirmAreaStr", typeof(string), 11, "text", null, false));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrConfirmOwnerStr", typeof(string), 12, "text", null, false));
            tbl.Columns.Add(new model.htmlControlGridSimpleColumn("wrConfirmContractorStr", typeof(string), 13, "text", null, false));

            tbl.Columns.Add(new model.columnComputedTrueFalse("wrConfirmRaja", typeof(int), "wrConfirmRajaStr"));
            tbl.Columns.Add(new model.columnComputedTrueFalse("wrConfirmArea", typeof(int), "wrConfirmAreaStr"));
            tbl.Columns.Add(new model.columnComputedTrueFalse("wrConfirmOwner", typeof(int), "wrConfirmOwnerStr"));
            tbl.Columns.Add(new model.columnComputedTrueFalse("wrConfirmContractor", typeof(int), "wrConfirmContractorStr"));

            tbl.Columns.Add(new model.columnComputedReference("gOwnerId", typeof(int), "rwmmsVehicleOwners", "companyName", "alternateNames", "Id", "wrOwnerName"));
            tbl.Columns.Add(new model.columnComputedReference("gWagonId", typeof(int), "Wagons", "wagonNo", "Id", "wrWagonNo"));
            tbl.Columns.Add(new model.columnComputedDateTime("gDateEntrance", typeof(DateTime), "wrSolarDateEntrance", model.columnComputedDateTime.enumDateTimeComputeType.solarDateToGregorianDate));
            tbl.Columns.Add(new model.columnComputedDateTime("gDateExit", typeof(DateTime), "wrSolarDateExit", model.columnComputedDateTime.enumDateTimeComputeType.solarDateToGregorianDate));
            tbl.Columns.Add(new model.columnComputedDateTime("gDatePlan", typeof(DateTime), "wrSolarDatePlanOwner", model.columnComputedDateTime.enumDateTimeComputeType.solarDateToGregorianDate));
            tbl.Columns.Add(new model.columnComputedDateTime("gDateStencil", typeof(DateTime), "wrSolarDateStencil", model.columnComputedDateTime.enumDateTimeComputeType.solarDateToGregorianDate));
            tbl.Columns.Add(new model.columnComputedReference("gRapairTypeId", typeof(int), "rwmmsRepairTypes", "typeName", "alternateNames", "Id", "wrRepairType"));
            tbl.Columns.Add(new model.columnComputedReference("gRepairRegionId", typeof(int), "rwmmsRepairRegions", "regionName", "alternateNames", "Id", "wrRepairRegion"));
            tbl.Columns.Add(new model.columnComputedReference("gRepairContractorId", typeof(int), "rwmmsRepairContractors", "contractorName", "alternateNames", "Id", "wrRepairContractor"));
            tbl.Columns.Add(new model.columnComputedReference("gRepairControllerId", typeof(int), "rwmmsRepairControllers", "controllerName", "alternateNames", "Id", "wrRepairController"));
            tbl.Columns.Add(new model.htmlControl("wrPageIndex", typeof(int), By.XPath("/html/body/form/div[3]/table/tbody/tr[4]/td/table/tbody/tr[7]/td/span"), "text", null));


            tbl.Columns.Add(new model.htmlControlGridDetailColumn("colEdit", typeof(string), 20, typeof(webpageWagonRepairInfoListDetail), false, true, true));
            this.prp_tables.Add(tbl);
            #endregion

        }
    }
}
