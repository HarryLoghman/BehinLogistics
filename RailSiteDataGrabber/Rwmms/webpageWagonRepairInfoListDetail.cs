using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.rwmms
{
    public class webpageWagonRepairInfoListDetail : model.webpage
    {
        //public webpageWagonRepairInfoListDetail() : this("http://rwmms.rai.ir/WagonRepairInfo.aspx")
        //{

        //}
        public webpageWagonRepairInfoListDetail(string url) : base(url)
        {
            this.sb_createTables();
        }
        public webpageWagonRepairInfoListDetail(int parentId) : base(parentId)
        {
            this.sb_createTables();
        }

        public webpageWagonRepairInfoListDetail(string url, ChromeDriver webBrowser) : base(url, webBrowser)
        {
            this.sb_createTables();
        }
        public webpageWagonRepairInfoListDetail(int parentId, ChromeDriver webBrowser) : base(parentId, webBrowser)
        {
            this.sb_createTables();
        }

        private void sb_createTables()
        {

            this.prp_tables = new List<model.htmlTable>();
            model.htmlTable tbl;

            //#region repairStations
            //tbl = new model.htmlTable("rwmmsRepairStations", this, "Id", null, By.Id("ContentPlaceHolder1_drpStation"));
            //tbl.prp_identifierColumnNames = new string[] { "repairStationName" };
            //tbl.prp_skipRowTop = 1;
            //tbl.Columns.Add(new model.htmlControlCombo("repairStationName", typeof(string), "text", null));
            //tbl.Columns.Add(new model.htmlControlCombo("repairStationId", typeof(string), "value", null));

            //this.prp_tables.Add(tbl);
            //#endregion

            //#region repairRepresentative
            //tbl = new model.htmlTable("rwmmsRailwayRepresentatives", this, "Id", null, By.Id("ContentPlaceHolder1_cboRailWayAssignee_DropDown"));
            //tbl.prp_identifierColumnNames = new string[] { "representativeName" };
            //tbl.prp_skipRowTop = 1;
            //tbl.Columns.Add(new model.htmlControlCombo("representativeName", typeof(string), "text", null));
            //tbl.Columns.Add(new model.htmlControlCombo("representativeValue", typeof(string), "value", null));

            //this.prp_tables.Add(tbl);
            //#endregion

            //#region BoogieType
            //tbl = new model.htmlTable("rwmmsBoogieTypes", this, "Id", null, By.Id("ContentPlaceHolder1_cboNoeWagonBogi"));
            //tbl.prp_identifierColumnNames = new string[] { "boogieTypeName" };
            //tbl.prp_skipRowTop = 1;
            //tbl.Columns.Add(new model.htmlControlCombo("boogieTypeName", typeof(string), "text", null));
            //tbl.Columns.Add(new model.htmlControlCombo("boogieTypeValue", typeof(string), "value", null));

            //this.prp_tables.Add(tbl);
            //#endregion

            //#region HookType
            //tbl = new model.htmlTable("rwmmsHookTypes", this, "Id", null, By.Id("ContentPlaceHolder1_cboNoeCrampon"));
            //tbl.prp_identifierColumnNames = new string[] { "hookTypeName" };
            //tbl.prp_skipRowTop = 1;
            //tbl.Columns.Add(new model.htmlControlCombo("hookTypeName", typeof(string), "text", null));
            //tbl.Columns.Add(new model.htmlControlCombo("hookTypeValue", typeof(string), "value", null));

            //this.prp_tables.Add(tbl);
            //#endregion

            //#region AutoTypes
            //tbl = new model.htmlTable("rwmmsAutoTypes", this, "Id", null, By.Id("ContentPlaceHolder1_cboNoeKhodkar"));
            //tbl.prp_identifierColumnNames = new string[] { "autoTypeName" };
            //tbl.prp_skipRowTop = 1;
            //tbl.Columns.Add(new model.htmlControlCombo("autoTypeName", typeof(string), "text", null));
            //tbl.Columns.Add(new model.htmlControlCombo("autoTypeValue", typeof(string), "value", null));

            //this.prp_tables.Add(tbl);
            //#endregion

            //#region ValveType
            //tbl = new model.htmlTable("rwmmsValveTypes", this, "Id", null, By.Id("ContentPlaceHolder1_cboNoeSupup"));
            //tbl.prp_identifierColumnNames = new string[] { "valveTypeName" };
            //tbl.prp_skipRowTop = 1;
            //tbl.Columns.Add(new model.htmlControlCombo("valveTypeName", typeof(string), "text", null));
            //tbl.Columns.Add(new model.htmlControlCombo("valveTypeValue", typeof(string), "value", null));

            //this.prp_tables.Add(tbl);
            //#endregion

            //#region CylinderTypes
            //tbl = new model.htmlTable("rwmmsCylinderTypes", this, "Id", null, By.Id("ContentPlaceHolder1_cboNoeSilandr"));
            //tbl.prp_identifierColumnNames = new string[] { "cylinderTypeName" };
            //tbl.prp_skipRowTop = 1;
            //tbl.Columns.Add(new model.htmlControlCombo("cylinderTypeName", typeof(string), "text", null));
            //tbl.Columns.Add(new model.htmlControlCombo("cylinderTypeValue", typeof(string), "value", null));

            //this.prp_tables.Add(tbl);
            //#endregion

            #region wagon
            tbl = new model.htmlTable("Wagons", this, "Id", null, null);
            tbl.prp_identifierColumnNames = new string[] { "wagonNo" };
            tbl.Columns.Add(new model.htmlControl("wagonNo", typeof(long), By.Id("ContentPlaceHolder1_TxtFind_WagonNo"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrChassisSerial", typeof(string), By.Id("ContentPlaceHolder1_txtShasiNoIn"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrRivNo", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_RIVNo"), "value", null));
            tbl.Columns.Add(new model.htmlControlRegex("wrDaysRemainToRepair", typeof(string), By.Id("ContentPlaceHolder1_lblInfo")
                , "text", "^(\\d+)", 1, null));
            tbl.Columns.Add(new model.htmlControl("wrOwnerName", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_Owner"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrOwnershipType", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_OwnershipType"), "value", null));
            //tbl.Columns.Add(new model.htmlControlCombo("contractorId", typeof(string), "value", null));
            this.prp_tables.Add(tbl);
            #endregion

            #region WagonRepair
            tbl = new model.htmlTable("rwmmsRepairs", this, "Id", null, null);
            tbl.prp_identifierColumnNames = new string[] { "Id" };
            tbl.Columns.Add(new model.htmlControlParent("Id", typeof(int), false));
            tbl.Columns.Add(new model.htmlControl("wrWagonNo", typeof(long), By.Id("ContentPlaceHolder1_TxtFind_WagonNo"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrRivNo", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_RIVNo"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrControlDigitNo", typeof(int), By.Id("ContentPlaceHolder1_lblControlDigit"), "text", null));
            tbl.Columns.Add(new model.htmlControl("wrOwnershipType", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_OwnershipType"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrAssessNo", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_EvalNo"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrSolarDateRepairFullLast", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_LastMainRepair"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrSolarDateRepairHalfLast", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_LastHalfRepair"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrBoogieType", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_NoeBogiTitle"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrHookType", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_NoeGholabTitle"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrBoogieSerial1", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_BogiSerialNo1"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrHookTypeSerial1", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_CramponSerialNo1"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrBoogieSerial2", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_BogiSerialNo2"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrHookTypeSerial2", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_CramponSerialNo2"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrAxisHeaderSerial1", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_AxleSerialNo1"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrAxisHeaderSerial2", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_AxleSerialNo2"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrAxisHeaderSerial3", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_AxleSerialNo3"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrAxisHeaderSerial4", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_AxleSerialNo4"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrAxisHeaderSerial5", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_AxleSerialNo5"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrAxisHeaderSerial6", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_AxleSerialNo6"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrAutoType", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_Noe_Khodkar_TormozTitle"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrAutoSerial", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_KhodkarSerialNo"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrValveType", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_Noe_SupspTitle"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrValveSerial", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_SupspSerialNo"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrBrakeCylinderType", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_Noe_Silandr_TormozTitle"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrCylinderSerial", typeof(string), By.Id("ContentPlaceHolder1_TxtFind_SilandrSerialNo"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrChassisSerial", typeof(string), By.Id("ContentPlaceHolder1_txtShasiNoIn"), "value", null));
            tbl.Columns.Add(new model.htmlControlComboSelectedItem("wrRepairStation", typeof(string), By.Id("ContentPlaceHolder1_drpStation"), "value", null));
            tbl.Columns.Add(new model.htmlControlComboSelectedItem("wrDelayType", typeof(string), By.Id("ContentPlaceHolder1_cboDelayReason"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrContractNo", typeof(string), By.Id("ContentPlaceHolder1_txtContractNo"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrDeliveryCount", typeof(int), By.Id("ContentPlaceHolder1_txtDeliveryCount"), "value", null));

            tbl.Columns.Add(new model.htmlControlComboSelectedItem("wrRailwayRepresentative", typeof(string), By.Id("ContentPlaceHolder1_cboRailWayAssignee_DropDown"), "text", null));
            tbl.Columns.Add(new model.htmlControl("wrRailwayRepresentativeSolarDate", typeof(string), By.Id("ContentPlaceHolder1_txtRailwayAssigneeDate"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrContractorRepresentative", typeof(string), By.Id("ContentPlaceHolder1_txtContractorAssignee"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrContractorRepresentativeSolarDate", typeof(string), By.Id("ContentPlaceHolder1_txtContractorDate"), "value", null));

            tbl.Columns.Add(new model.htmlControlComboSelectedItem("wrBoogieTypeEx", typeof(string), By.Id("ContentPlaceHolder1_cboNoeWagonBogi"), "text", null));
            tbl.Columns.Add(new model.htmlControl("wrBoogieSerial1Ex", typeof(string), By.Id("ContentPlaceHolder1_txtBogiSerialNo1_txtSerialNo"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrBoogieSerial2Ex", typeof(string), By.Id("ContentPlaceHolder1_txtBogiSerialNo2_txtSerialNo"), "value", null));

            tbl.Columns.Add(new model.htmlControlComboSelectedItem("wrHookTypeEx", typeof(string), By.Id("ContentPlaceHolder1_cboNoeCrampon"), "text", null));
            tbl.Columns.Add(new model.htmlControl("wrHookType1SerialEx", typeof(string), By.Id("ContentPlaceHolder1_txtCramponSerialNo1"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrHookType2SerialEx", typeof(string), By.Id("ContentPlaceHolder1_txtCramponSerialNo2"), "value", null));

            tbl.Columns.Add(new model.htmlControl("wrAxisHeaderSerial1Ex", typeof(string), By.Id("ContentPlaceHolder1_txtAxleSerialNo1_txtSerialNo"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrAxisWheelSize1Ex", typeof(string), By.Id("ContentPlaceHolder1_txtAxleSize1"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrAxisHeaderSerial2Ex", typeof(string), By.Id("ContentPlaceHolder1_txtAxleSerialNo2_txtSerialNo"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrAxisWheelSize2Ex", typeof(string), By.Id("ContentPlaceHolder1_txtAxleSize2"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrAxisHeaderSerial3Ex", typeof(string), By.Id("ContentPlaceHolder1_txtAxleSerialNo3_txtSerialNo"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrAxisWheelSize3Ex", typeof(string), By.Id("ContentPlaceHolder1_txtAxleSize3"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrAxisHeaderSerial4Ex", typeof(string), By.Id("ContentPlaceHolder1_txtAxleSerialNo4_txtSerialNo"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrAxisWheelSize4Ex", typeof(string), By.Id("ContentPlaceHolder1_txtAxleSize4"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrAxisHeaderSerial5Ex", typeof(string), By.Id("ContentPlaceHolder1_txtAxleSerialNo5_txtSerialNo"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrAxisWheelSize5Ex", typeof(string), By.Id("ContentPlaceHolder1_txtAxleSize5"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrAxisHeaderSerial6Ex", typeof(string), By.Id("ContentPlaceHolder1_txtAxleSerialNo6_txtSerialNo"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrAxisWheelSize6Ex", typeof(string), By.Id("ContentPlaceHolder1_txtAxleSize6"), "value", null));

            tbl.Columns.Add(new model.htmlControlComboSelectedItem("wrAutoTypeEx", typeof(string), By.Id("ContentPlaceHolder1_cboNoeKhodkar"), "text", null));
            tbl.Columns.Add(new model.htmlControl("wrAutoSerialEx", typeof(string), By.Id("ContentPlaceHolder1_txtKhodkarSerialNo_txtSerialNo"), "value", null));
            tbl.Columns.Add(new model.htmlControlComboSelectedItem("wrValveTypeEx", typeof(string), By.Id("ContentPlaceHolder1_cboNoeSupup"), "text", null));
            tbl.Columns.Add(new model.htmlControl("wrValveSerialEx", typeof(string), By.Id("ContentPlaceHolder1_txtSupopSerialNo_txtSerialNo"), "value", null));
            tbl.Columns.Add(new model.htmlControlComboSelectedItem("wrBrakeCylinderTypeEx", typeof(string), By.Id("ContentPlaceHolder1_cboNoeSilandr"), "text", null));
            tbl.Columns.Add(new model.htmlControl("wrCylinderSerialEx", typeof(string), By.Id("ContentPlaceHolder1_txtSilandrSerialNo_txtSerialNo"), "value", null));
            tbl.Columns.Add(new model.htmlControl("wrChassisSerialNoEx", typeof(string), By.Id("ContentPlaceHolder1_txtShasiNoOut"), "value", null));

            tbl.Columns.Add(new model.htmlControl("wrDescription", typeof(string), By.Id("ContentPlaceHolder1_txtDescription"), "value", null));

            this.prp_tables.Add(tbl);


            #endregion


        }
    }
}
