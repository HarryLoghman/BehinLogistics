//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RailSiteDataGrabber.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class raiTrainPassengersLastStatu
    {
        public int Id { get; set; }
        public Nullable<int> wRowNo { get; set; }
        public Nullable<int> wTrainNo { get; set; }
        public string wTrainType { get; set; }
        public string wSourceStationName { get; set; }
        public string wDestinationStationName { get; set; }
        public string wTrainBossName { get; set; }
        public string wCurrentStationName { get; set; }
        public string wEnteranceDateTime { get; set; }
        public string wExitDateTime { get; set; }
        public string wAreaName { get; set; }
        public Nullable<int> trainId { get; set; }
        public Nullable<int> sourceStationId { get; set; }
        public Nullable<int> destinationStationId { get; set; }
        public Nullable<int> currentStationId { get; set; }
        public Nullable<System.DateTime> enteranceDateTime { get; set; }
        public Nullable<System.DateTime> exitDateTime { get; set; }
        public Nullable<int> areaId { get; set; }
        public Nullable<System.DateTime> FetchTime { get; set; }
        public Nullable<int> CycleNumber { get; set; }
    }
}
