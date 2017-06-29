using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vanp.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var top5New = _unitOfWork.ProductRepository.GetTopNNew(5);
          
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

    }

}