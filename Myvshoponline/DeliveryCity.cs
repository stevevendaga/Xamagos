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
    
    public partial class DeliveryCity
    {
        public int ID { get; set; }
        public Nullable<int> CityID { get; set; }
        public Nullable<int> StateID { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public Nullable<int> FromStateID { get; set; }
        public string DeliveryNote { get; set; }
        public string PhoneNumber_Deliverer { get; set; }
    
        public virtual DeliveryCity_Only DeliveryCity_Only { get; set; }
        public virtual State State { get; set; }
        public virtual State State1 { get; set; }
    }
}
