using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vanp.DAL;
using Vanp.DAL.Entites;
using Vanp.Web.Models;

namespace Vanp.Web.Controllers
{
    [AllowAnonymous]
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
        public JsonResult GetProducts(int pageNo = 1 , int pageSize = 10, string orderBy = "dateto" , bool asc =true , int? category = null)
        {
            var products = _unitOfWork.ProductRepository.GetListByCategory( pageNo, pageSize, orderBy, asc, category);
            return JsonSuccess(products.Select(p => new ProductModel(p)));
        }
        public ActionResult Product(int id)
        {
            return View();
        }
    
    }
}