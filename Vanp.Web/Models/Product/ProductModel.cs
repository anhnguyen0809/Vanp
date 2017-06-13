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

        public double DateFrom { get; set; }

        public double DateTo { get; set; }
        public string BidCurrentByName { get; set; }
        public int CategoryId { get; set; }
        public int BidCount { get; set; }
        public bool IsBid { get; set; }
        public bool IsExtended { get; set; }
    }
}