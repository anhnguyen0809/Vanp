using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using Vanp.DAL.Entites;
using Vanp.Web.Models;

namespace Vanp.Web.Areas.Customer.Controllers
{
    public class ProductController : AuthController
    {
        // GET: Customer/Product
        public ActionResult Insert()
        {
            if (!_unitOfWork.UserRepository.IsPermissionSeller(CurrentUser.Id ?? 0))
            {
                _unitOfWork.RequestRepository.Approved(4, 2);
                _unitOfWork.ProductRepository.Bid(17, 1, 11600000);
                Failure = "Bạn chưa đăng ký hoặc dã hết hạn đăng sản phẩm. Vui lòng gửi yêu cầu cho chúng tôi.";
                return Redirect("/account/sendrequest");
            }
            
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Insert(ProductModel pro, HttpPostedFileBase image1, HttpPostedFileBase image2, HttpPostedFileBase image3)
        {
            if (!_unitOfWork.UserRepository.IsPermissionSeller(CurrentUser.Id ?? 0))
            {
                Failure = "Bạn chưa đăng ký hoặc dã hết hạn đăng sản phẩm. Vui lòng gửi yêu cầu cho chúng tôi.";
                return Redirect("/account/sendrequest");
            }
            else
            {
                Product p = new Product();
                p.Enable = true;
                p.BidCount = 0;
                p.ModifiedWhen = DateTime.Now;
                p.CreatedWhen = DateTime.Now;
                p.CreatedBy = CurrentUser.Id;
                p.ModifiedBy = CurrentUser.Id;
                p.DateFrom = DateTime.Now;
                p.DateTo = DateTime.Now.AddDays(7);
                p.CategoryId = pro.CategoryId;
                p.PriceDefault = p.PriceCurrent = p.PriceMax = pro.PriceDefault;
                p.ProductName = pro.ProductName;
                p.PriceStep = pro.PriceStep;
                p.IsExtended = pro.IsExtended;
                p.ProductDescription = pro.ProductDescription;
                p.ProductText = pro.ProductText;
                p.Price = pro.Price;
                _unitOfWork.ProductRepository.Insert(p);
                _unitOfWork.Save();
                p.ProductImagePath = "~/images/products/" + p.Id + "/img1.jpg";
                _unitOfWork.ProductRepository.Update(p);
                _unitOfWork.Save();
                var spDirPath = Server.MapPath("~/images/products");
                var targetDirPath = Path.Combine(spDirPath, p.Id.ToString());
                Directory.CreateDirectory(targetDirPath);

                var img1Name = Path.Combine(targetDirPath, "img1.jpg");
                image1.SaveAs(img1Name);

                var img2Name = Path.Combine(targetDirPath, "img2.jpg");
                image2.SaveAs(img2Name);

                var img3Name = Path.Combine(targetDirPath, "img3.jpg");
                image3.SaveAs(img3Name);
            }
            return View(pro);
        }
        public ActionResult ListProduct()
        {
            int userId = Convert.ToInt16(CurrentUser.Id);
            return View(_unitOfWork.ProductRepository.GetListByProductOfCus(userId));
        }
        public ActionResult Update(int id)
        {
            return View(_unitOfWork.ProductRepository.GetById(id));
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(ProductModel pro,int Id)
        {
            Product p=_unitOfWork.ProductRepository.GetById(Id);
            p.ProductDescription = pro.ProductDescription;
            p.ProductText = pro.ProductText;
            _unitOfWork.ProductRepository.Update(p);
            _unitOfWork.Save();
            return Redirect("/account/product/update/"+ Id + "");
        }
        #region Bid
        [HttpPost]
        public ActionResult Bid(int id , double priceBid)
        {
            ViewBag.ProductId = id;
            if (!_unitOfWork.ProductRepository.CheckBidPermisstion(CurrentUser.Id ?? 0, id))
            {
                Failure = "Bạn không có thể đấu giá cho sản phẩm này. Do điểm đánh giá (+/+-) nhỏ hơn 80% hoặc do người đăng sản phẩm này đã kích bạn ra khỏi sản phẩm này.";
            }
            else if (!_unitOfWork.ProductRepository.ValidPriceBid(id , priceBid))
            {
                Failure = "Giá bạn đặt ra không hợp lệ. Nó phải lớn hơn giá hiện tại của sản phẩm.";
            }
            else if (_unitOfWork.ProductRepository.Bid(CurrentUser.Id ?? 0, id, priceBid))
            {
                Success = "Đấu giá thành công. Cám ơn bạn đã tham gia hệ thống của chúng tôi.";
            }
            else
            {
                Failure = "Đấu giá thất bại. ";
            }
            return View();
        } 
        #endregion
    }
}