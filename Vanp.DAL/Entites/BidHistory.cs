//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vanp.DAL.Entites
{
    using System;
    using System.Collections.Generic;
    
    public partial class BidHistory
    {
        public int Id { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedWhen { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedWhen { get; set; }
        public Nullable<int> Order { get; set; }
        public Nullable<bool> Enable { get; set; }
        public Nullable<double> BidPrice { get; set; }
        public Nullable<double> PriceCurrent { get; set; }
        public Nullable<double> PriceMax { get; set; }
        public Nullable<int> ProductId { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}
