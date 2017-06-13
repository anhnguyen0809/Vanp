using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vanp.Web.Areas.Customer.Controllers
{
    [Authorize]
    public class CProductController : AuthController
    {
        // GET: Customer/Product
        public ActionResult Insert()
        {
            if (CurrentUser.Roles!="Seller")
            {
                return Redirect("/account/sendrequest");
            }
            return View();
        }
    }
}