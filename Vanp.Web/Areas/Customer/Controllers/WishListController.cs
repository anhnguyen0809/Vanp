using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vanp.DAL.Entites;

namespace Vanp.Web.Areas.Customer.Controllers
{
    public class WishlistController : AuthController
    {
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
        public JsonResult Delete(int id)
        {
            _unitOfWork.WishlistRepository.Delete(id);
            return JsonSuccess("Đã xóa sản phẩm ra khỏi danh sách yêu thích của bạn.");
        }
        public ActionResult Wishlist()
        {
            var wishList = _unitOfWork.WishlistRepository.GetListByUser(CurrentUser.Id ?? 0)
                        .Select(o => new Models.WishlistModel(o));
            return View(wishList);
        }
    }
}