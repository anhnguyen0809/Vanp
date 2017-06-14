﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vanp.DAL.Entites;

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
                //_unitOfWork.ProductRepository.Bid(16, , )
                Failure = "Bạn chưa đăng ký hoặc dã hết hạn đăng sản phẩm. Vui lòng gửi yêu cầu cho chúng tôi.";
                return Redirect("/account/sendrequest");
            }
            
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Insert(Product pro, HttpPostedFileBase image1, HttpPostedFileBase image2, HttpPostedFileBase image3)
        {
            if (!_unitOfWork.UserRepository.IsPermissionSeller(CurrentUser.Id ?? 0))
            {
                Failure = "Bạn chưa đăng ký hoặc dã hết hạn đăng sản phẩm. Vui lòng gửi yêu cầu cho chúng tôi.";
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
                var targetDirPath = Path.Combine(spDirPath, pro.Id.ToString());
                Directory.CreateDirectory(targetDirPath);

                var img1Name = Path.Combine(targetDirPath, "img1.jpg");
                image1.SaveAs(img1Name);

                var img2Name = Path.Combine(targetDirPath, "img2.jpg");
                image2.SaveAs(img2Name);

                var img3Name = Path.Combine(targetDirPath, "img3.jpg");
                image3.SaveAs(img3Name);
            }
            return View();
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