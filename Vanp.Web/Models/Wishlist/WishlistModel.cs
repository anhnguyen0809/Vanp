using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vanp.DAL.Entites;

namespace Vanp.Web.Models
{
    public class WishlistModel : BaseModel
    {
        public ProductModel Product { get; set; }
        public int? ProductId { get; set; }
        public WishlistModel()
        {

        }
        public WishlistModel(Wishlist wishlist)
        {
            this.Id = wishlist.Id;
            this.CreatedWhen = wishlist.CreatedWhen;
            this.ProductId = wishlist.ProductId;
            if (wishlist.ProductId.HasValue)
            {
                this.Product = new ProductModel(wishlist.Product);
            }
        }
    }
}