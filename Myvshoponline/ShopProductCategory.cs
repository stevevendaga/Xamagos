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
    
    public partial class ShopProductCategory
    {
        public int ID { get; set; }
        public Nullable<int> ShopID { get; set; }
        public Nullable<int> ProductCategoryID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual Shop Shop { get; set; }
    }
}
