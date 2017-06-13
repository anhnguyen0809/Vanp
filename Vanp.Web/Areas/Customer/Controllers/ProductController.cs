using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vanp.Web.Areas.Customer.Controllers
{
    public class ProductController : AuthController
    {
        // GET: Customer/Product
        public ActionResult Insert()
        {
            return View();
        }
    }
}