using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vanp.DAL.Entites;

namespace Vanp.Web.Areas.Customer.Controllers
{
    [Authorize]
    public class CProductController : AuthController
    {
        // GET: Customer/Product
        public ActionResult Insert(Product pro)
        {
            
            if (CurrentUser.Roles!="Seller")
            {
                return Redirect("/account/sendrequest");
            }
            else
            {
                ViewBag.listCat = _unitOfWork.CategoryRepository.GetListCat();
            }
            return View();
        }
    }
}