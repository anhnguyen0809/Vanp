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
               "ChangeAccount",
               "account/profile",
               new { action = "Index", controller = "User", area = "Customer" }
           );
            context.MapRoute(
              "AccountInfo",
              "account",
              new { action = "Index", controller = "User", area = "Customer" }
          );
            context.MapRoute(
                 name: "SendCode",
                 url: "account/sendcode",
                 defaults: new { controller = "User", action = "SendCode", area = "Customer" }
             );
            context.MapRoute(
            name: "VerifyCode",
            url: "account/verifycode",
            defaults: new { controller = "User", action = "VerifyCode", area = "Customer" }
            );
            context.MapRoute(
                "Customer_default",
                "Customer/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}