using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vanp.DAL;
using Vanp.DAL.Entites;

namespace Vanp.Web.Controllers
{
    public class ProductController : BaseController
    {
        public ActionResult Products()
        {
            return View();
        }
        public ActionResult ProductsByCategory(int id)
        {
            var products = _unitOfWork.ProductRepository.GetListByCategory(id);
            return View("Products",products);
        }
        public ActionResult Product(int id)
        {
            return View();
        }
        public ActionResult Detail(int id)
        {
            var model = _unitOfWork.ProductRepository.GetById(id);
            return View(model);
        }
    }
}