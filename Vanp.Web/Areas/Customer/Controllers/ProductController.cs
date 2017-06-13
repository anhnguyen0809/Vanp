using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vanp.Web.Areas.Customer.Controllers
{
    public class ProductController : AuthController
    {
        // GET: Customer/Product
        public ActionResult Insert()
        {
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
            }else if (!_unitOfWork.ProductRepository.ValidPriceBid(id , priceBid))
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