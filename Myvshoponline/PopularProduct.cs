//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Myvshoponline
{
    using System;
    using System.Collections.Generic;
    
    public partial class PopularProduct
    {
        public int ID { get; set; }
        public Nullable<int> ShopID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public int SubProductID { get; set; }
        public Nullable<int> PopularProductStatusID { get; set; }
        public int PaymentStatus { get; set; }
        public string RefNo { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> DatePaid { get; set; }
        public Nullable<int> DaysActive { get; set; }
        public Nullable<int> TotalDaysPaid { get; set; }
        public Nullable<int> LaunchYear { get; set; }
        public Nullable<int> LaunchMonth { get; set; }
        public Nullable<int> LaunchDay { get; set; }
        public Nullable<int> PreviousYear { get; set; }
        public Nullable<int> PreviousMonth { get; set; }
        public Nullable<int> PreviousDay { get; set; }
        public string EndDate { get; set; }
    
        public virtual PopularStoreStatu PopularStoreStatu { get; set; }
        public virtual Product Product { get; set; }
        public virtual ProductSubProduct ProductSubProduct { get; set; }
        public virtual Shop Shop { get; set; }
    }
}