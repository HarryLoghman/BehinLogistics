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
    
    public partial class rwmmsDeclerationList
    {
        public int Id { get; set; }
        public Nullable<int> wRowNo { get; set; }
        public Nullable<long> wWagonNo { get; set; }
        public string wTypeName { get; set; }
        public string wDeclerationNo { get; set; }
        public string wOwnerName { get; set; }
        public string wOwnerRepresentativeName { get; set; }
        public string wStatus { get; set; }
        public string wIssueDate { get; set; }
        public string wNoteNo { get; set; }
        public string wRegisterNo { get; set; }
        public string wPageNo { get; set; }
        public string wDocStatus { get; set; }
        public string wOtherRights { get; set; }
        public string wOwnerDocuments { get; set; }
        public string wDescription { get; set; }
        public Nullable<bool> wConfirmOwner { get; set; }
        public string wConfirmOwnerDescription { get; set; }
        public Nullable<bool> wConfirmMali { get; set; }
        public string wConfirmMaliDescription { get; set; }
        public Nullable<bool> wConfirmEdareWagon { get; set; }
        public string wConfirmEdareWagonDescription { get; set; }
        public Nullable<bool> wConfirmEdareBazargani { get; set; }
        public string wConfirmEdareBazarganiDescription { get; set; }
        public Nullable<bool> wConfirmEdareSeir { get; set; }
        public string wConfirmEdareSeirDescription { get; set; }
        public Nullable<bool> wConfirmSarmaye { get; set; }
        public string wConfirmSarmayeDescription { get; set; }
        public Nullable<bool> wConfirmTajhiz { get; set; }
        public string wConfirmTajhizDescription { get; set; }
        public Nullable<bool> wConfirmHoghoghi { get; set; }
        public string wConfirmHoghoghiDescription { get; set; }
        public Nullable<bool> wConfirmFinal { get; set; }
        public string wConfirmFinalDescription { get; set; }
        public string wAttachmentPath { get; set; }
        public Nullable<int> PageIndex { get; set; }
        public Nullable<int> vehicleOwnerId { get; set; }
        public Nullable<System.DateTime> issueDate { get; set; }
        public Nullable<System.DateTime> FetchTime { get; set; }
        public Nullable<int> CycleNumber { get; set; }
        public string Source { get; set; }
    }
}
