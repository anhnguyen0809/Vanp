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
    public class WishlistController : AuthController
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetProducts(int pageNo = 1, int pageSize = 10, string orderBy = "dateto", bool asc = true)
        {
            var products = _context.Wishlists
                .Where(o => o.UserId.HasValue && o.UserId == CurrentUser.Id)
                .Select(o => o.Product);
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
        // GET: Customer/WishList
        [HttpPost]
        public JsonResult Insert(int productId)
        {
            if (_unitOfWork.WishlistRepository.IsExisted(CurrentUser.Id.Value, productId))
            {
                return JsonSuccess(message: "Đã tồn tại sản phẩm trong danh sách Yêu Thích");
            }
            else
            {
                if (_context.Products.Any(o => o.Id == productId))
                {
                    var wishList = new Wishlist()
                    {
                        UserId = CurrentUser.Id,
                        ModifiedBy = CurrentUser.Id,
                        CreatedBy = CurrentUser.Id,
                        CreatedWhen = DateTime.Now,
                        ModifiedWhen = DateTime.Now,
                        ProductId = productId
                    };
                    _unitOfWork.WishlistRepository.Insert(wishList);
                    _unitOfWork.Save();
                    return JsonSuccess(message: "Thêm vào danh sách Yêu Thích thành công.");
                }
                else
                {
                    return JsonError("Không tìm thấy sản phẩm. Vui lòng kiểm tra lại");
                }
            }
        }
        [HttpPost]
        public JsonResult Delete(int productId)
        {
            if (_unitOfWork.WishlistRepository.IsExisted(CurrentUser.Id.Value, productId))
            {
                var wishlist = _unitOfWork.WishlistRepository.GetByUserAndProduct(CurrentUser.Id ?? 0, productId);
                _unitOfWork.WishlistRepository.Delete(wishlist);
                _unitOfWork.Save();
                return JsonSuccess(message:"Đã xóa sản phẩm ra khỏi danh sách yêu thích của bạn.");
            }
            return JsonError("Không tìm thấy sản phẩm trong danh sách yêu thích của bạn. Vui lòng kiểm tra lại");
        }

    }
}