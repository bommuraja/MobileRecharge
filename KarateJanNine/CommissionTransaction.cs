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
    
    public partial class CommissionTransaction
    {
        public int CommissionTransactionID { get; set; }
        public int CustomerID { get; set; }
        public Nullable<bool> IsCredit { get; set; }
        public string CommissionTransactionDate { get; set; }
        public string CommissionTransactionReferenceDescription { get; set; }
        public string CommissionTransactionReferenceID { get; set; }
        public string CommissionTransactionAmount { get; set; }
        public string CommissionTransactionDescription { get; set; }
        public string CommissionBalance { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}
