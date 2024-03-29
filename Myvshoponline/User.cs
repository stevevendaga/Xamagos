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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.CustomerShops = new HashSet<CustomerShop>();
            this.DeliveryAgentAllocations = new HashSet<DeliveryAgentAllocation>();
            this.IdentityVerifications = new HashSet<IdentityVerification>();
            this.IdentityVerifications1 = new HashSet<IdentityVerification>();
            this.Orders = new HashSet<Order>();
            this.OrderNegotiations = new HashSet<OrderNegotiation>();
            this.OurLeads = new HashSet<OurLead>();
            this.Shops = new HashSet<Shop>();
            this.TempPendingPayments = new HashSet<TempPendingPayment>();
            this.UserBonus = new HashSet<UserBonu>();
            this.Workdones = new HashSet<Workdone>();
        }
    
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<int> UserRoleID { get; set; }
        public string Description { get; set; }
        public Nullable<int> PaymentStatus { get; set; }
        public string ReferenceNo { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> DatePaid { get; set; }
        public Nullable<System.DateTime> LaunchDate { get; set; }
        public Nullable<int> NDays { get; set; }
        public Nullable<int> FixedDays { get; set; }
        public Nullable<System.DateTime> PreviousDate { get; set; }
        public Nullable<int> LaunchYear { get; set; }
        public Nullable<int> LaunchMonth { get; set; }
        public Nullable<int> LaunchDay { get; set; }
        public Nullable<int> PreviousYear { get; set; }
        public Nullable<int> PreviousMonth { get; set; }
        public Nullable<int> PreviousDay { get; set; }
        public Nullable<int> PlanID { get; set; }
        public Nullable<int> BillingCycleID { get; set; }
        public string resetkey { get; set; }
        public Nullable<int> ReferalID { get; set; }
        public Nullable<int> EmailVerify { get; set; }
        public string HardToken { get; set; }
        public Nullable<System.DateTime> PasswordUpdated { get; set; }
        public Nullable<int> IdentityStatus { get; set; }
        public Nullable<int> CountryID { get; set; }
    
        public virtual BillingCycle BillingCycle { get; set; }
        public virtual CountryRegion CountryRegion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerShop> CustomerShops { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DeliveryAgentAllocation> DeliveryAgentAllocations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IdentityVerification> IdentityVerifications { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IdentityVerification> IdentityVerifications1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderNegotiation> OrderNegotiations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OurLead> OurLeads { get; set; }
        public virtual PricingPlan PricingPlan { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Shop> Shops { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TempPendingPayment> TempPendingPayments { get; set; }
        public virtual UserRole UserRole { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserBonu> UserBonus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Workdone> Workdones { get; set; }
    }
}
