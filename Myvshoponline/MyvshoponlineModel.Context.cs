﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MyvshoponlineEntities : DbContext
    {
        public MyvshoponlineEntities()
            : base("name=MyvshoponlineEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<AptitudeTestGuideline> AptitudeTestGuidelines { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<BankInformation> BankInformations { get; set; }
        public virtual DbSet<BillingCycle> BillingCycles { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<CountryRegion> CountryRegions { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<CustomerShop> CustomerShops { get; set; }
        public virtual DbSet<CustomerStatu> CustomerStatus { get; set; }
        public virtual DbSet<DeliveryAgentAllocation> DeliveryAgentAllocations { get; set; }
        public virtual DbSet<DeliveryCity> DeliveryCities { get; set; }
        public virtual DbSet<DeliveryCity_Only> DeliveryCity_Only { get; set; }
        public virtual DbSet<DeliveryLocation> DeliveryLocations { get; set; }
        public virtual DbSet<Donation> Donations { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }
        public virtual DbSet<EmailMessageLog> EmailMessageLogs { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<IdentityVerification> IdentityVerifications { get; set; }
        public virtual DbSet<MailingListNG> MailingListNGs { get; set; }
        public virtual DbSet<MailingListNonNG> MailingListNonNGs { get; set; }
        public virtual DbSet<NewsletterEmail> NewsletterEmails { get; set; }
        public virtual DbSet<OnlineTraining> OnlineTrainings { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderNegotiation> OrderNegotiations { get; set; }
        public virtual DbSet<OurLead> OurLeads { get; set; }
        public virtual DbSet<PaymentAccountActivation> PaymentAccountActivations { get; set; }
        public virtual DbSet<PaymentStatu> PaymentStatus { get; set; }
        public virtual DbSet<PlanFeature> PlanFeatures { get; set; }
        public virtual DbSet<PlatformBenefit> PlatformBenefits { get; set; }
        public virtual DbSet<PopularProduct> PopularProducts { get; set; }
        public virtual DbSet<PopularProductPricing> PopularProductPricings { get; set; }
        public virtual DbSet<PopularStore> PopularStores { get; set; }
        public virtual DbSet<PopularStorePricing> PopularStorePricings { get; set; }
        public virtual DbSet<PopularStoreStatu> PopularStoreStatus { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<PricingPlan> PricingPlans { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductComment> ProductComments { get; set; }
        public virtual DbSet<ProductStatu> ProductStatus { get; set; }
        public virtual DbSet<ProductSubProduct> ProductSubProducts { get; set; }
        public virtual DbSet<ProductVisit> ProductVisits { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectImage> ProjectImages { get; set; }
        public virtual DbSet<RequestOnlineOrder> RequestOnlineOrders { get; set; }
        public virtual DbSet<SearchOptimization> SearchOptimizations { get; set; }
        public virtual DbSet<SearchOptimizationPricing> SearchOptimizationPricings { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<Sex> Sexes { get; set; }
        public virtual DbSet<ShippingInformation> ShippingInformations { get; set; }
        public virtual DbSet<Shop> Shops { get; set; }
        public virtual DbSet<ShopAPIKey> ShopAPIKeys { get; set; }
        public virtual DbSet<ShopCustomer> ShopCustomers { get; set; }
        public virtual DbSet<ShopProductCategory> ShopProductCategories { get; set; }
        public virtual DbSet<ShopVisit> ShopVisits { get; set; }
        public virtual DbSet<SocialMediaAd> SocialMediaAds { get; set; }
        public virtual DbSet<SocialMediaAdsStatistic> SocialMediaAdsStatistics { get; set; }
        public virtual DbSet<SocialMediaChannel> SocialMediaChannels { get; set; }
        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<TempPendingPayment> TempPendingPayments { get; set; }
        public virtual DbSet<Testimonial> Testimonials { get; set; }
        public virtual DbSet<TrainingModule> TrainingModules { get; set; }
        public virtual DbSet<TrainingPricing> TrainingPricings { get; set; }
        public virtual DbSet<TrialPeriod> TrialPeriods { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserBonu> UserBonus { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Workdone> Workdones { get; set; }
    }
}