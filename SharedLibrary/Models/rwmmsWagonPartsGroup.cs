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
    
    public partial class rwmmsWagonPartsGroup
    {
        public int Id { get; set; }
        public Nullable<int> IdGroupParent { get; set; }
        public string wGroupName { get; set; }
        public string wSerialNo { get; set; }
        public string wPSID { get; set; }
        public Nullable<System.DateTime> FetchTime { get; set; }
    }
}