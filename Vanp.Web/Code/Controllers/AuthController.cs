using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vanp.Web
{
    using DAL.Entites;
    using DAL.Utils;
    using Vanp.DAL;

    [Authorize]
    public class AuthController : BaseController
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!CurrentUser.IsAuthenticated)
            {
                if (!Request.Url.AbsolutePath.ToLower().Contains("/account/verifycode"))
                {
                    Response.Redirect("/account/sendcode");
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}