using System;
using System.Collections.Generic;
using System.IO;
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
        public ActionResult Insert()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Insert(Product pro, HttpPostedFileBase productimage)
        {
            if (CurrentUser.Roles!="Seller")
            {
                return Redirect("/account/sendrequest");
            }
            else
            {
                pro.ModifiedWhen = DateTime.Now;
                pro.CreatedWhen = DateTime.Now;
                pro.CreatedBy = CurrentUser.Id;
                pro.ModifiedBy = CurrentUser.Id;
                pro.DateFrom = DateTime.Now;
                pro.DateTo = DateTime.Now.AddDays(7);
                pro.ProductImagePath = productimage.FileName;
                _unitOfWork.ProductRepository.Insert(pro);
                _unitOfWork.Save();
                ViewBag.listCat = _unitOfWork.CategoryRepository.GetListCat();
                var fileName = Path.GetFileName(productimage.FileName);
                var path = Path.Combine(Server.MapPath("~/images/products/"), fileName);
            }
            return View();
        }
    }
}