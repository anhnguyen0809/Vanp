using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public ActionResult Bid(int id, double priceBid)
        {
            ViewBag.ProductId = id;
            if (!_unitOfWork.ProductRepository.CheckBidPermisstion(CurrentUser.Id ?? 0, id))
            {
                Failure = "Bạn không có thể đấu giá cho sản phẩm này. Do điểm đánh giá (+/+-) nhỏ hơn 80% hoặc do người đăng sản phẩm này đã kích bạn ra khỏi sản phẩm này.";
            }
            else if (!_unitOfWork.ProductRepository.ValidPriceBid(id, priceBid))
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
        [HttpPost]
        public ActionResult BuyNow(int id, double priceBid)
        {
            if (_unitOfWork.ProductRepository.BuyNow(CurrentUser.Id ?? 0, id, priceBid))
            {
                Success = "Sản phẩm đã được bạn mua. Cám ơn bạn đã tham gia hệ thống của chúng tôi.";
            }
            else
            {
                Failure = "Mua ngay thất bại. ";
            }
            return View();
        }

        #region Danh sách người dùng đang tham gia đấu giá
        public ActionResult ProductsUserBidding()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetProductsUserBidding(int pageNo = 1, int pageSize = 10, string orderBy = "dateto", bool asc = true)
        {
            var products = _context.BidHistories
                .Where(o => o.CreatedBy.HasValue && 
                            o.CreatedBy == CurrentUser.Id &&
                            o.ProductId.HasValue && (!o.Product.IsBid.HasValue || o.Product.IsBid == false)  &&
                            o.Product.DateTo.HasValue && o.Product.DateTo.Value >= DateTime.Now)
                .GroupBy(o => o.Product)
                .Select(o => o.FirstOrDefault().Product);
            if (orderBy.ToLower() == "dateto")
            {
                products = asc ? products.OrderBy(o => o.DateTo) : products.OrderByDescending(o => o.DateTo);
            }
            else if (orderBy.ToLower() == "pricecurrent")
            {
                products = asc ? products.OrderBy(o => o.PriceCurrent) : products.OrderByDescending(o => o.PriceCurrent);
            }

            products = asc ? products.OrderBy(o => o.Id) : products.OrderByDescending(o => o.Id);

            products = products
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize);
            return JsonSuccess(products.ToList().Select(o => new ProductModel(o)));
        }
        #endregion
        #region Danh sách người dùng đang tham gia đấu giá
        public ActionResult ProductsUserSuccessful()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetProductsUserSuccessful(int pageNo = 1, int pageSize = 10, string orderBy = "dateto", bool asc = true)
        {
            var products = _context.Products
                .Where(o => o.BidCurrentBy.HasValue &&
                            o.BidCurrentBy == CurrentUser.Id &&
                            o.IsBid.HasValue && o.IsBid.Value);
            if (orderBy.ToLower() == "dateto")
            {
                products = asc ? products.OrderBy(o => o.DateTo) : products.OrderByDescending(o => o.DateTo);
            }
            else if (orderBy.ToLower() == "pricecurrent")
            {
                products = asc ? products.OrderBy(o => o.PriceCurrent) : products.OrderByDescending(o => o.PriceCurrent);
            }

            products = asc ? products.OrderBy(o => o.Id) : products.OrderByDescending(o => o.Id);

            products = products
                    .Skip((pageNo - 1) * pageSize)
                    .Take(pageSize);
            return JsonSuccess(products.ToList().Select(o => new ProductModel(o)));
        }
        #endregion
    }
}