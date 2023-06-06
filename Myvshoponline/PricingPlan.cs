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
    
    public partial class PricingPlan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PricingPlan()
        {
            this.PlanFeatures = new HashSet<PlanFeature>();
            this.Users = new HashSet<User>();
        }
    
        public int ID { get; set; }
        public string PlanName { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> NumberofShops { get; set; }
        public Nullable<int> NumberofProducts { get; set; }
        public Nullable<int> NumberofCustomers { get; set; }
        public Nullable<decimal> PriceDollar { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanFeature> PlanFeatures { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
    }
}
