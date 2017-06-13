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
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(Product pro,HttpPostedFileBase productimage)
        {
            var fileName = Path.GetFileName(productimage.FileName);
            var path = Path.Combine(Server.MapPath("~/images/products/"), fileName);
            if (System.IO.File.Exists(path))
                ViewBag.ThongBao = "Hình ảnh đã tồn tại";
            else
                productimage.SaveAs(path);
            pro.ModifiedWhen = DateTime.Now;
            pro.CreatedWhen = DateTime.Now;
            pro.CreatedBy = CurrentUser.Id;
            pro.ModifiedBy = CurrentUser.Id;
            pro.DateFrom = DateTime.Now;
            pro.DateTo = DateTime.Now.AddDays(7);
            pro.ProductImagePath = productimage.FileName;
            _unitOfWork.ProductRepository.Insert(pro);
            _unitOfWork.Save();
            return View();
        }
        public ActionResult ViewListProduct()
        {
            return View(_unitOfWork.ProductRepository.GetListByProduct());
        }
        public ActionResult DetailProduct(int proID)
        {
            return View(_unitOfWork.ProductRepository.GetById(proID));
        }

        public ActionResult Products()
        {
            return View();
        }
    }
}