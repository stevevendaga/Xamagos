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
    
    public partial class ShippingInformation
    {
        public int ID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public string Address { get; set; }
        public Nullable<int> StateID { get; set; }
        public string City { get; set; }
        public Nullable<int> CountryID { get; set; }
        public string DeliveryNotes { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
    
        public virtual CountryRegion CountryRegion { get; set; }
        public virtual Order Order { get; set; }
        public virtual State State { get; set; }
    }
}
