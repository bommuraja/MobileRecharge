//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KarateJanNine
{
    using System;
    using System.Collections.Generic;
    
    public partial class Recharge
    {
        public int RechargeID { get; set; }
        public int CustomerID { get; set; }
        public string MobileNumber { get; set; }
        public string NetworkName { get; set; }
        public string RechargeAmount { get; set; }
        public string DateAndTime { get; set; }
        public string RechargeStatus { get; set; }
        public string RechargeStatusDescription { get; set; }
        public string RechargeReferenceIDFromAPI { get; set; }
        public string RechargeReferenceIDFromNetwork { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public string Profit { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}
