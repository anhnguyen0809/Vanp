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
         url: "product/insert",
         defaults: new { controller = "CProduct", action = "Insert", area = "Customer" }
         );
            context.MapRoute(
                "Customer_default",
                "Customer/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}