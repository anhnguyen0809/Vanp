using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vanp.Web.Models;

namespace Vanp.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        public ActionResult ProductsTop5Bid()
        {
            var products = _context.Products.Where(o => (!o.IsBid.HasValue || o.IsBid == false) && o.DateTo.HasValue && o.DateTo.Value >= DateTime.Now)
                .OrderByDescending(o => o.BidCount)
                .Take(5).ToList().Select(o => new ProductModel(o));
            return PartialView("ProductsTop5Bid", products);
        }

        public ActionResult ProductsTop5Price()
        {
            var products = _context.Products.Where(o => (!o.IsBid.HasValue || o.IsBid == false) && o.DateTo.HasValue && o.DateTo.Value >= DateTime.Now)
                .OrderByDescending(o => o.Price)
                .Take(5).ToList().Select(o => new ProductModel(o));
            return PartialView("ProductsTop5Price", products);
        }
    }
}