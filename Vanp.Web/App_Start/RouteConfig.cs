using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Vanp.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // BotDetect requests must not be routed
            routes.IgnoreRoute("{*botdetect}", new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            #region Authorize
            routes.MapRoute(
             name: "Login",
             url: "login",
             defaults: new { controller = "Account", action = "Login" }
         );
            routes.MapRoute(
            name: "Register",
            url: "register",
            defaults: new { controller = "Account", action = "Register" }
        );
            routes.MapRoute(
              name: "Logout",
              url: "logout",
              defaults: new { controller = "Account", action = "Logout" }
          );
            routes.MapRoute(
              name: "SendCode",
              url: "account/sendcode",
              defaults: new { controller = "Account", action = "SendCode" }
            );
            routes.MapRoute(
              name: "ForgotPassword",
              url: "forgotpassword",
              defaults: new { controller = "Account", action = "ForgotPassword" }
            );

            #endregion
            routes.MapRoute(
               name: "Products",
               url: "products",
               defaults: new { controller = "Product", action = "Products" },
               namespaces: new[] { "Vanp.Web.Controllers.ProductController" }
             );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Vanp.Web.Controllers" }
            );
        }
    }
}
