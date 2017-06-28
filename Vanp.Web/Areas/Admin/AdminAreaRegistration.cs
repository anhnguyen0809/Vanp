using System.Web.Mvc;

namespace Vanp.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
              name: "home",
              url: "adminpages",
              defaults: new { controller = "Home", action = "Index", area = "Admin" },
              namespaces: new[] { "Vanp.Web.Areas.Admin.Controllers" }
              );
            context.MapRoute(
            name: "user",
            url: "adminpages/user",
            defaults: new { controller = "User", action = "Index", area = "Admin" },
            namespaces: new[] { "Vanp.Web.Areas.Admin.Controllers" }
            );
            context.MapRoute(
            name: "approved",
            url: "adminpages/approved",
            defaults: new { controller = "Request", action = "Index", area = "Admin" },
            namespaces: new[] { "Vanp.Web.Areas.Admin.Controllers" }
            );
            context.MapRoute(
            name: "category",
            url: "adminpages/category",
            defaults: new { controller = "Category", action = "Index", area = "Admin" },
            namespaces: new[] { "Vanp.Web.Areas.Admin.Controllers" }
            );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                 new[] { "Vanp.Web.Areas.Admin.Controllers" }
            );
        }
    }
}