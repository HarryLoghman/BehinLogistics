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
    
    public partial class PWS0StateCities
    {
        public int Id { get; set; }
        public Nullable<int> StateId { get; set; }
        public string StateName { get; set; }
        public Nullable<int> CityId { get; set; }
        public string CityName { get; set; }
        public Nullable<int> ParentStateIdInJson { get; set; }
        public Nullable<int> ParentIdInDB { get; set; }
        public Nullable<bool> isState { get; set; }
    }
}
