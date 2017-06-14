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
            ViewBag.listCat = _unitOfWork.CategoryRepository.GetListCat();
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Insert(Product pro, HttpPostedFileBase image1, HttpPostedFileBase image2, HttpPostedFileBase image3)
        {
            if (CurrentUser.Roles!="Seller")
            {
                return Redirect("/account/sendrequest");
            }
            else
            {
                pro.Enable = true;
                pro.BidCount = 0;
                pro.Order = 1;
                pro.ModifiedWhen = DateTime.Now;
                pro.CreatedWhen = DateTime.Now;
                pro.CreatedBy = CurrentUser.Id;
                pro.ModifiedBy = CurrentUser.Id;
                pro.DateFrom = DateTime.Now;
                pro.DateTo = DateTime.Now.AddDays(7);
                _unitOfWork.ProductRepository.Insert(pro);
                _unitOfWork.Save();
                var spDirPath = Server.MapPath("~/images/products");
                var targetDirPath = Path.Combine(spDirPath,pro.Id.ToString());
                Directory.CreateDirectory(targetDirPath);

                var img1Name = Path.Combine(targetDirPath,"img1.jpg");
                image1.SaveAs(img1Name);

                var img2Name = Path.Combine(targetDirPath, "img2.jpg");
                image2.SaveAs(img2Name);

                var img3Name = Path.Combine(targetDirPath, "img3.jpg");
                image3.SaveAs(img3Name);
            }
            return View();
        }
        public ActionResult CListProduct()
        {
            int id = Convert.ToInt32(CurrentUser.Id);
            return View(_unitOfWork.ProductRepository.GetListByCProduct(id));
        }
        public ActionResult Edit(int proId)
        {
            return View(_unitOfWork.ProductRepository.GetById(proId));
        }
    }
}