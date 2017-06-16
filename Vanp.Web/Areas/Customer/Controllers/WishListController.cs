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
            var wishList = _unitOfWork.WishlistRepository.GetListByUser(CurrentUser.Id ?? 0)
                        .Select(o => new ProductModel(o.Product));
            return View();
        }
        [HttpGet]
        public JsonResult GetProducts(int pageNo = 1, int pageSize = 10, string orderBy = "dateto", bool asc = true)
        {
            var products = _unitOfWork.WishlistRepository.GetListByUser(CurrentUser.Id ?? 0)
                        .Select(o => new ProductModel(o.Product));
            return JsonSuccess(products);
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
                return JsonSuccess("Đã xóa sản phẩm ra khỏi danh sách yêu thích của bạn.");
            }
            return JsonError("Không tìm thấy sản phẩm trong danh sách yêu thích của bạn. Vui lòng kiểm tra lại");
        }

    }
}