using System.Web.Mvc;

namespace Vanp.Web.Areas.Customer
{
    public class CustomerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Customer";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            #region Profile
            context.MapRoute(
               "Profile",
               "account/profile",
               new { action = "UserProfile", controller = "User", area = "Customer" }
           );
            context.MapRoute(
              "Doashboard",
              "account",
              new { action = "Index", controller = "User", area = "Customer" }
          );
            context.MapRoute(
                name: "VerifyCode",
                url: "account/verifycode",
                defaults: new { controller = "User", action = "VerifyCode", area = "Customer" }
            );
            context.MapRoute(
                  name: "SendRequest",
                  url: "account/sendrequest",
                  defaults: new { controller = "User", action = "SendRequest", area = "Customer" }
              );

            #endregion
            #region Seller
            context.MapRoute(
              name: "InsertProduct",
              url: "account/product/insert",
              defaults: new { controller = "Product", action = "Insert", area = "Customer" },
              namespaces: new[] { "Vanp.Web.Areas.Customer.Controllers" }
          );
            context.MapRoute(
             name: "ListProduct",
             url: "account/product/list",
             defaults: new { controller = "Product", action = "ListProduct", area = "Customer" },
             namespaces: new[] { "Vanp.Web.Areas.Customer.Controllers" }
         );
            context.MapRoute(
                name: "Update",
                url: "account/product/update/{id}",
                defaults: new { controller = "Product", action = "Update", area = "Customer", id = UrlParameter.Optional },
                namespaces: new[] { "Vanp.Web.Areas.Customer.Controllers" }
            );
            context.MapRoute(
               name: "Kicked",
               url: "account/product/kicked/{id}",
               defaults: new { controller = "Product", action = "Kicked", area = "Customer", id = UrlParameter.Optional },
               namespaces: new[] { "Vanp.Web.Areas.Customer.Controllers" }
           );
            context.MapRoute(
                name: "ProductsBidding",
                url: "account/product/bidding",
                defaults: new { controller = "Product", action = "ProductsBidding", area = "Customer" },
                namespaces: new[] { "Vanp.Web.Areas.Customer.Controllers" }
            );
            context.MapRoute(
                name: "ProductsSuccessful",
                url: "account/product/bidsuccess",
                defaults: new { controller = "Product", action = "ProductsSuccessful", area = "Customer" },
                namespaces: new[] { "Vanp.Web.Areas.Customer.Controllers" }
            );
            #endregion
            context.MapRoute(
            name: "BidHistoryByProduct",
            url: "account/product/bidhistory/{productId}",
            defaults: new { controller = "BidHistory", action = "BidHistory", area = "Customer", productId = UrlParameter.Optional },
            namespaces: new[] { "Vanp.Web.Areas.Customer.Controllers" }
        );
            context.MapRoute(
                name: "BidProduct",
                url: "product/bid/{id}",
                defaults: new { controller = "Product", action = "Bid", id = UrlParameter.Optional, area = "Customer" }
            );
            context.MapRoute(
           name: "BidSuccessful",
           url: "product/bidsuccessful/{id}",
           defaults: new { controller = "Product", action = "BidSuccessful", id = UrlParameter.Optional, area = "Customer" }
       );
            context.MapRoute(
                  name: "Wishlist",
                  url: "account/wishlist",
                  defaults: new { controller = "Wishlist", action = "Index", area = "Customer" }
              );
            context.MapRoute(
                 name: "Bidding",
                 url: "account/bidding",
                 defaults: new { controller = "Product", action = "ProductsUserBidding", area = "Customer" }
             );
            context.MapRoute(
                name: "Bidsuccess",
                url: "account/bidsuccess",
                defaults: new { controller = "Product", action = "ProductsUserSuccessful", area = "Customer" }
            );
            context.MapRoute(
                  name: "voted",
                  url: "account/voted",
                  defaults: new { controller = "Product", action = "ProductsUserVoted", area = "Customer" }
              );
            context.MapRoute(
            name: "BidHistory",
            url: "account/bidhistory",
            defaults: new { controller = "BidHistory", action = "Index", area = "Customer" }
            );
            context.MapRoute(
                "Customer_default",
                "Customer/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Vanp.Web.Areas.Customer.Controllers" }
            );

        }
    }
}