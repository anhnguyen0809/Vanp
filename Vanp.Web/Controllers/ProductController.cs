using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vanp.DAL;
using Vanp.DAL.Entites;
using Vanp.Web.Models;

namespace Vanp.Web.Controllers
{
    [AllowAnonymous]
    public class ProductController : BaseController
    {
        public ActionResult Product(int id)
        {
            var product = _unitOfWork.ProductRepository.GetById(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Permission = true;
            ViewBag.PriceValid = _unitOfWork.ProductRepository.GetPriceValid(id);
            ViewBag.Seller = product.User ?? new DAL.Entites.User();
            if (CurrentUser.Id.HasValue && !(_unitOfWork.ProductRepository.CheckBidPermisstion(CurrentUser.Id ?? 0, id)))
            {
                Failure = "Bạn không thể đấu giá cho sản phẩm này. Do điểm đánh giá (+/+-) nhỏ hơn 80% hoặc do người đăng sản phẩm này đã kích bạn ra khỏi sản phẩm này.";
                ViewBag.Permission = false;
            }
            var model = new ProductModel(product);
            return View(model);
        }
        public ActionResult Products()
        {
            return View();
        }
        public ActionResult ProductsByCategory(int? id)
        {
            if (id.HasValue)
            {
                var category = _unitOfWork.CategoryRepository.GetById(id);
                ViewBag.CategoryName = string.Empty;
                if (category != null) { ViewBag.CategoryName = category.CategoryName; }
                else Failure = "Không tìm thấy danh mục này!";
            }
            return RedirectToAction("SearchProducts", new { cats = id });
        }
        public ActionResult SearchProducts(string search = "", string cats = "")
        {
            ViewBag.SearchCategories = cats;
            ViewBag.Search = search;
            if (!string.IsNullOrEmpty(cats))
            {
                var categoriesId = cats.Split(',').Select(Int32.Parse).ToArray();
                ViewBag.CategoryName = string.Join(", ", _context.Categories.Where(o => categoriesId.Contains(o.Id)).Select(o => o.CategoryName));
            }
            return View("Products");
        }
        public JsonResult GetProducts(int pageNo = 1, int pageSize = 10, string orderBy = "dateto", bool asc = true, string categories = null, string search = "")
        {
            int[] categoriesId = null;
            if (!string.IsNullOrEmpty(categories))
            {
                categoriesId = categories.Split(',').Select(Int32.Parse).ToArray();
            }
            var products = _unitOfWork.ProductRepository.GetListByCategory(search, categoriesId, pageNo, pageSize, orderBy, asc);
            var totalRows = _unitOfWork.ProductRepository.GetListByCategory(search, categoriesId, null, null).Count();
            return JsonSuccess(
                new
                {
                    products = products.Select(p => new ProductModel(p)),
                    totalRows = totalRows
                });
        }

    }
}