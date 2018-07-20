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
    
    public partial class CashTransaction
    {
        public int CashTransactionID { get; set; }
        public int CustomerID { get; set; }
        public string Date { get; set; }
        public string Reference { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsCredit { get; set; }
        public Nullable<bool> IsForWallet { get; set; }
        public Nullable<bool> IsForBalance { get; set; }
        public string CashTransactionAmount { get; set; }
        public string CashBalance { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    
        public virtual Customer Customer { get; set; }
    }
}
