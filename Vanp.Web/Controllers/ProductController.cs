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

        // GET: Product
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Product pro,HttpPostedFileBase productimage)
        {
            pro.ModifiedWhen = DateTime.Now;
            pro.CreatedWhen = DateTime.Now;
            pro.CreatedBy = CurrentUser.Id;
            pro.ModifiedBy = CurrentUser.Id;
            pro.ProductImagePath = productimage.FileName;
            _unitOfWork.ProductRepository.Insert(pro);
            _unitOfWork.Save();
            return View();
        }
    }
}