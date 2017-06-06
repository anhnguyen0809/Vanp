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
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            #endregion
        }
    }
}
