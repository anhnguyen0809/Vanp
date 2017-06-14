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
    [AllowAnonymous]
    public class ProductController : BaseController
    {
        public ActionResult Products()
        {
            return View();
        }
        public ActionResult Products(int id)
        {
            var products = _unitOfWork.ProductRepository.GetListByCategory(id);
            return View(products);
        }
        public ActionResult Product(int id)
        {
            return View();
        }
    
    }
}