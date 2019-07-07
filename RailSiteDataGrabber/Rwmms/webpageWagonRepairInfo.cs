using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailSiteDataGrabber.rwmms
{
    class webpageWagonRepairInfo : webpage
    {

        public webpageWagonRepairInfo(string url) : base(url)
        {
            Model.htmlTable tableRepairContractor = new Model.htmlTable("rwmmsRepairContractors", 1, 0, null, new string[] { "contractorName" });
            tableRepairContractor.Columns.Add(new Model.htmlColumn("contractorName", typeof(string), null, null, "text","alternateNames", Model.htmlColumn.enum_controlType.input));
            tableRepairContractor.Columns.Add(new Model.htmlColumn("contractorId", typeof(string), null, null, "value",null, Model.htmlColumn.enum_controlType.input));

            this.prp_controls = new List<webpageControl>();
            this.prp_controls.Add(new webpageCombo(this.v_webBrowser,this, "repairContractor", "پیمانکار تعمیرات", By.Id("ContentPlaceHolder1_repCombo_DropDown")
                ,"", enumControlType.combo, tableRepairContractor));

            Model.htmlTable tableRepairRegions = new Model.htmlTable("rwmmsRepairRegions", 1, 0, null, new string[] { "regionName" });
            tableRepairRegions.Columns.Add(new Model.htmlColumn("regionName", typeof(string), null, null, "text", "alternateNames", Model.htmlColumn.enum_controlType.input));
            tableRepairRegions.Columns.Add(new Model.htmlColumn("regeionId", typeof(string), null, null, "value",null, Model.htmlColumn.enum_controlType.input));
            this.prp_controls.Add(new webpageCombo(this.v_webBrowser, this, "repairRegion", "ناحیه تعمیراتی", By.Id("ContentPlaceHolder1_regCombo_cmbRegion")
                , "", enumControlType.combo, tableRepairRegions));

            Model.htmlTable tableRepairTypes = new Model.htmlTable("rwmmsRepairTypes", 1, 0, null, new string[] { "typeName" });
            tableRepairTypes.Columns.Add(new Model.htmlColumn("typeName", typeof(string), null, null, "text", "alternateNames", Model.htmlColumn.enum_controlType.input));
            tableRepairTypes.Columns.Add(new Model.htmlColumn("typeId", typeof(string), null, null, "value",null, Model.htmlColumn.enum_controlType.input));
            this.prp_controls.Add(new webpageCombo(this.v_webBrowser, this, "repairType", "نوع تعمیر", By.Id("ContentPlaceHolder1_cboRepairKind")
                , "", enumControlType.combo, tableRepairTypes));

            Model.htmlTable tableRepairControllers = new Model.htmlTable("rwmmsRepairControllers", 1, 0, null, new string[] { "ControllerName" });
            tableRepairControllers.Columns.Add(new Model.htmlColumn("controllerName", typeof(string), null, null, "text", "alternateNames", Model.htmlColumn.enum_controlType.input));
            tableRepairControllers.Columns.Add(new Model.htmlColumn("controllerId", typeof(string), null, null, "value",null, Model.htmlColumn.enum_controlType.input));
            this.prp_controls.Add(new webpageCombo(this.v_webBrowser, this, "repairController", "شرکت بازرسی", By.Id("ContentPlaceHolder1_cboController_DropDown")
                , "", enumControlType.combo, tableRepairControllers));

            Model.htmlTable tableRepairDelayTypes = new Model.htmlTable("rwmmsRepairDelayTypes", 1, 0, null, new string[] { "typeName" });
            tableRepairDelayTypes.Columns.Add(new Model.htmlColumn("typeName", typeof(string), null, null, "text", "alternateNames", Model.htmlColumn.enum_controlType.input));
            tableRepairDelayTypes.Columns.Add(new Model.htmlColumn("typeId", typeof(string), null, null, "value",null, Model.htmlColumn.enum_controlType.input));
            this.prp_controls.Add(new webpageCombo(this.v_webBrowser, this, "repairDelayTypes", "نوع تاخیر", By.Id("ContentPlaceHolder1_cboDelayReason")
                , "", enumControlType.combo, tableRepairDelayTypes));

            Model.htmlTable tableVehicleOwners = new Model.htmlTable("rwmmsVehicleOwners", 1, 0, null, new string[] { "CompanyName" });
            tableVehicleOwners.Columns.Add(new Model.htmlColumn("CompanyName", typeof(string), null, null, "text", "alternateNames", Model.htmlColumn.enum_controlType.input));
            tableVehicleOwners.Columns.Add(new Model.htmlColumn("wCompanyId", typeof(string), null, null, "value", null, Model.htmlColumn.enum_controlType.input));
            this.prp_controls.Add(new webpageCombo(this.v_webBrowser, this, "rwmmsVehicleOwner", "مالک واگن", By.Id("ContentPlaceHolder1_cboOwner_DropDown")
                , "", enumControlType.combo, tableVehicleOwners));

            Model.htmlTable tableRepairs = new Model.htmlTable("rwmmsRepairs", 1, 1, null, new string[] { "wagonNo" });
            tableVehicleOwners.Columns.Add(new Model.htmlColumn("WagonNo", typeof(string), 1, null, "text", null, Model.htmlColumn.enum_controlType.gridCell));
            tableVehicleOwners.Columns.Add(new Model.htmlColumn("dateEntrance", typeof(string), 2, null, "text", null, Model.htmlColumn.enum_controlType.gridCell));
            tableVehicleOwners.Columns.Add(new Model.htmlColumn("dateExit", typeof(string), 3, null, "text", null, Model.htmlColumn.enum_controlType.gridCell));
            tableVehicleOwners.Columns.Add(new Model.htmlColumn("datePlan", typeof(string), 4, null, "text", null, Model.htmlColumn.enum_controlType.gridCell));
            tableVehicleOwners.Columns.Add(new Model.htmlColumn("dateOwnerRepair", typeof(string), 5, null, "text", null, Model.htmlColumn.enum_controlType.gridCell));
            tableVehicleOwners.Columns.Add(new Model.htmlColumn("dateStencil", typeof(string), 6, null, "text", null, Model.htmlColumn.enum_controlType.gridCell));
            tableVehicleOwners.Columns.Add(new Model.htmlColumn("repairType", typeof(string), 7, null, "text", null, Model.htmlColumn.enum_controlType.gridCell));
            tableVehicleOwners.Columns.Add(new Model.htmlColumn("repairArea", typeof(string), 8, null, "text", null, Model.htmlColumn.enum_controlType.gridCell));
            tableVehicleOwners.Columns.Add(new Model.htmlColumn("contractorName", typeof(string), 9, null, "text", null, Model.htmlColumn.enum_controlType.gridCell));
            tableVehicleOwners.Columns.Add(new Model.htmlColumn("controllerName", typeof(string), 10, null, "text", null, Model.htmlColumn.enum_controlType.gridCell));
            tableVehicleOwners.Columns.Add(new Model.htmlColumn("confirmRaja", typeof(string), 11, null, "text", null, Model.htmlColumn.enum_controlType.gridCell));
            tableVehicleOwners.Columns.Add(new Model.htmlColumn("confirmArea", typeof(string), 12, null, "text", null, Model.htmlColumn.enum_controlType.gridCell));
            tableVehicleOwners.Columns.Add(new Model.htmlColumn("confirmOwner", typeof(string), 13, null, "text", null, Model.htmlColumn.enum_controlType.gridCell));
            tableVehicleOwners.Columns.Add(new Model.htmlColumn("confirmContractor", typeof(string), 14, null, "text", null, Model.htmlColumn.enum_controlType.gridCell));

            //this.prp_controls.Add(new webpageGrid(this.v_webBrowser, "ContentPlaceHolder1_dgWagonRepair", "اطلاعات تعمیرات", "ContentPlaceHolder1_dgWagonRepair", enumControlType.grid, tableRepairs));
            //this.prp_controls.Add(new webpageButton("btnSearch", "جستجو", "ContentPlaceHolder1_cmdSearch", null, enumControlType.button));

            //this.prp_controls.Add(new webpageGrid("ContentPlaceHolder1_dgWagonRepair", null, enumControlType.grid));

        }


    } 
}
