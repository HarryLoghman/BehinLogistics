//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SharedLibrary.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PWS0BillOfLadings
    {
        public int Id { get; set; }
        public Nullable<int> jTrain_No { get; set; }
        public Nullable<long> jWagon_NO { get; set; }
        public string jBarnameh_NO { get; set; }
        public string jSource_Station_Name { get; set; }
        public string jDestination_Station_Name { get; set; }
        public string jBar_Type { get; set; }
        public Nullable<int> jCurrent_Station_Code { get; set; }
        public string jCurrent_Station_Name { get; set; }
        public Nullable<int> jSource_Station_Code { get; set; }
        public string jEntrance_Date { get; set; }
        public Nullable<System.DateTime> jEntrance_Date_M { get; set; }
        public string jEntrance_Time { get; set; }
        public string jTashkil_Date { get; set; }
        public string jTashkil_Time { get; set; }
        public Nullable<int> jF15Rec_ID { get; set; }
        public Nullable<long> WagonId { get; set; }
        public Nullable<long> TrainId { get; set; }
        public Nullable<int> SourceStationId { get; set; }
        public Nullable<int> DestinationStationId { get; set; }
        public Nullable<int> CurrentStationId { get; set; }
        public Nullable<int> goodsId { get; set; }
        public Nullable<System.DateTime> TashkilDateTime { get; set; }
        public Nullable<System.DateTime> FetchTime { get; set; }
        public string FetchUrl { get; set; }
        public Nullable<int> CycleNumber { get; set; }
    }
}