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
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.BidHistories = new HashSet<BidHistory>();
            this.ProductKickeds = new HashSet<ProductKicked>();
            this.Votes = new HashSet<Vote>();
        }
    
        public int Id { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedWhen { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedWhen { get; set; }
        public Nullable<int> Order { get; set; }
        public Nullable<bool> Enable { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public string ProductText { get; set; }
        public string ProductImagePath { get; set; }
        public Nullable<double> PriceDefault { get; set; }
        public Nullable<double> PriceCurrent { get; set; }
        public Nullable<double> PriceMax { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> PriceStep { get; set; }
        public Nullable<System.DateTime> DateFrom { get; set; }
        public Nullable<System.DateTime> DateTo { get; set; }
        public Nullable<int> BidCurrentBy { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> BidCount { get; set; }
        public Nullable<bool> IsBid { get; set; }
        public Nullable<bool> IsExtended { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BidHistory> BidHistories { get; set; }
        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }
        public virtual User User3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductKicked> ProductKickeds { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vote> Votes { get; set; }
    }
}
