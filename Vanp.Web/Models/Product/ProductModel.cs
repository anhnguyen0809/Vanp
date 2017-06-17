using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vanp.DAL.Entites;

namespace Vanp.Web.Models
{
    public class ProductModel : BaseModel
    {
        public string ProductName { get; set; }

        public string ProductCode { get; set; }

        public string ProductDescription { get; set; }

        public string ProductText { get; set; }

        public string ProductImagePath { get; set; }

        public double PriceDefault { get; set; }

        public double PriceCurrent { get; set; }

        public double Price { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public int? CategoryId { get; set; }
        public int BidCount { get; set; }
        public bool IsBid { get; set; }
        public bool IsExtended { get; set; }
        public string CreatedByName { get; set; }
        public string BidCurrentByName { get; set; }
        public string EndTime
        {
            get
            {
                return DAL.Utils.Helper.Subtract(DateTime.Now, DateTo ?? new DateTime());
            }
        }
        public bool New
        {
            get
            {
                return (DateFrom.HasValue && DateTime.Now.Subtract(this.DateFrom.Value).TotalMinutes <= 20) ? true : false;
            }
        }
        public CategoryModel Category { get; set; }
        public ProductModel()
        {

        }
        public ProductModel(Product product)
        {
            this.Id = product.Id;
            this.ProductName = product.ProductName;
            this.ProductDescription = product.ProductDescription;
            this.ProductText = product.ProductText;
            this.ProductCode = product.ProductCode;
            this.DateFrom = product.DateFrom;
            this.DateTo = product.DateTo;
            this.BidCount = product.BidCount ?? 0;
            this.ProductImagePath = product.ProductImagePath;
            this.Price = product.Price ?? 0;
            this.PriceCurrent = product.PriceCurrent ?? 0;
            this.PriceDefault = product.PriceDefault ?? 0;
            this.CategoryId = product.CategoryId;
            this.IsBid = product.IsBid ?? false;
            if (product.User != null)
            {
                this.CreatedByName = product.User.UserName;
            }
            if (product.User2 != null)
            {
                if (!this.IsBid) this.BidCurrentByName = GetHash(product.User2.UserName);
                else this.BidCurrentByName = product.User2.UserName;
            }
            this.IsExtended = product.IsExtended ?? false;
            if (product.Category != null)
            {
                this.Category = new CategoryModel(product.Category);
            }
        }
        private string GetHash(string str)
        {
            var hash = "";
            if (str.Length > 3)
            {
                hash = str.Substring(0, 3);
                for (int i = 3; i <= str.Length - 1; i++, hash += "*") ;
            }
            return hash;
        }
    }
}