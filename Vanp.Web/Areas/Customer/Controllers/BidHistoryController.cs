using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vanp.DAL.Entites;

namespace Vanp.Web.Areas.Customer.Controllers
{
    public class BidHistoryController : AuthController
    {
        // GET: Customer/BidHistory
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BidHistory(int productId)
        {
            ViewBag.IsBid = false;
            ViewBag.ProductId = productId;
            var product = _unitOfWork.ProductRepository.GetById(productId);
            if(product != null && product.CreatedBy == CurrentUser.Id)
            {
                ViewBag.IsBid = product.IsBid ?? false;
                return PartialView(_unitOfWork.BidHistoryRepository.GetListByProduct(productId).OrderByDescending(o=> o.Id));
            }
            return PartialView(Enumerable.Empty<BidHistory>());
        }
    }
}