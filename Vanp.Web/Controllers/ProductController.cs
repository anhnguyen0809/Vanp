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
        [AllowAnonymous]
        // GET: Product
        public ActionResult ViewListProduct()
        {
            return View(_unitOfWork.ProductRepository.GetListByProduct());
        }
        public ActionResult DetailProduct(int proID)
        {
            return View(_unitOfWork.ProductRepository.GetById(proID));
        }
    }
}