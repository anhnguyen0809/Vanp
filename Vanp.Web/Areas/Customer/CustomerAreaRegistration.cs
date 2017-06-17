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
            context.MapRoute(
                 name: "InsertProduct",
                 url: "account/product/insert",
                 defaults: new { controller = "Product", action = "Insert", area = "Customer" },
                 namespaces:  new[] { "Vanp.Web.Areas.Customer.Controllers" }
             );
            context.MapRoute(
                 name: "ListProduct",
                 url: "account/product/list",
                 defaults: new { controller = "Product", action = "ListProduct", area = "Customer" }
             );
            context.MapRoute(
                name: "Update",
                url: "account/product/update/{id}",
                defaults: new { controller = "Product", action = "Update", area = "Customer",id=UrlParameter.Optional}
            );
            context.MapRoute(
                name: "Vote",
                url: "account/vote",
                defaults: new { controller = "InfoVote", action = "Appraise", area = "Customer"},
                namespaces: new[] {"Vanp.Web.Areas.Customer.Controller"}
            );
            context.MapRoute(
                name: "BidProduct",
                url: "product/bid",
                defaults: new { controller = "Product", action = "Bid", area = "Customer" }
            );
            context.MapRoute(
                "Customer_default",
                "Customer/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new [] { "Vanp.Web.Areas.Customer.Controllers" }
            );

        }
    }
}